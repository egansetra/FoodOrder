using FoodOrder.Helper;
using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace FoodOrder.DL
{
    public class OrderDL
    {
        public List<Order> GetOrder()
        {
            var rs = new List<Order>();
            string connString = Utility.GetConfig("DbConfig");

            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = String.Format(@"
                     With X As (
                         SELECT a.FoodID, b.Name, b.Description, b.Price, SUM(a.Quantity) as Quantity, (b.Price * SUM(a.Quantity)) AS Subtotal FROM OrderCart a
                                                INNER JOIN Menu b ON a.FoodID = b.ID
                                                GROUP BY b.Name, b.Description, b.Price, a.Quantity, a.FoodID
                        ) select  FoodID, Name, Description, Price, Sum(Quantity) As Quantity, Sum(Subtotal) AS Subtotal from X
                        GROUP BY FoodID, Name, Description, Price ");

                    using (var dr = cmd.ExecuteReader()) {
                        Order temp;
                        while (dr.Read())
                        {
                            temp = new Order()
                            {
                                FoodID = int.Parse(dr["FoodID"].ToString()),
                                Name = dr["Name"].ToString(),
                                Quantity = int.Parse(dr["Quantity"].ToString()),
                                Price = float.Parse(dr["Price"].ToString()),
                                Subtotal = float.Parse(dr["Subtotal"].ToString())
                            };

                            rs.Add(temp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rs;
        }

        public int AddOrder(Order order)
        {
            int rs = 0;
            string connString = Utility.GetConfig("DbConfig");

            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = String.Format(@"
                       INSERT INTO OrderCart(FoodID, OrderDate, Quantity) VALUES({0}, '{1}', {2}) ", order.FoodID, order.OrderDate, order.Quantity);

                    rs = cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rs;
        }

        public int DeleteOrder(int id)
        {
            int rs = 0;
            string connString = Utility.GetConfig("DbConfig");

            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = String.Format(@"
                       DELETE OrderCart WHERE FoodID = {0}", id);

                    rs = cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rs;
        }

        public int PaymentConfirmed(ConfirmPayment confirm)
        {
            int rs = 0;
            float total = 0;
            StringBuilder sb = new StringBuilder();

            string connString = Utility.GetConfig("DbConfig");
            SqlTransaction transaction = null;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                using (transaction = conn.BeginTransaction())
                {
                    try
                    {


                        cmd.CommandText = String.Format(@"
                         With X As (
                         SELECT a.FoodID, b.Name, b.Description, b.Price, SUM(a.Quantity) as Quantity, (b.Price * SUM(a.Quantity)) AS Subtotal FROM OrderCart a
                                                INNER JOIN Menu b ON a.FoodID = b.ID
                                                GROUP BY b.Name, b.Description, b.Price, a.Quantity, a.FoodID
                        ) select  FoodID, Name, Description, Price, Sum(Quantity) As Quantity, Sum(Subtotal) AS Subtotal from X
                        GROUP BY FoodID, Name, Description, Price ");
                        cmd.Transaction = transaction;
                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                sb.Append(String.Format("INSERT INTO TransactionDetail(TransactionID, Name, Price, Qty, Subtotal) VALUES({0}, '{1}', {2}, {3}, {4})", "<<TID>>", dr["Name"].ToString(), float.Parse(dr["Price"].ToString()), int.Parse(dr["Quantity"].ToString()), float.Parse(dr["Subtotal"].ToString())));
                                total += float.Parse(dr["Subtotal"].ToString());
                            }
                        }

                        cmd.CommandText = String.Format(@"
                       INSERT TransactionHistory(CustName, TransactionDate, Total, Payment) VALUES('{0}', '{1}', {2}, '{3}');SELECT  CAST(SCOPE_IDENTITY() as int); ", confirm.Name, confirm.TransactionDate, total, confirm.PayType);

                        rs = (int)cmd.ExecuteScalar();

                        cmd.CommandText = sb.ToString();
                        cmd.CommandText = cmd.CommandText.Replace("<<TID>>", rs.ToString());

                        rs = cmd.ExecuteNonQuery();

                        if (rs > 0)
                        {
                            cmd.CommandText = "DELETE  OrderCart";
                            rs = cmd.ExecuteNonQuery();
                        }


                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null)
                            transaction.Rollback();

                        throw ex;
                    }
                   
                }
            }
            return rs;
        }
    }
}