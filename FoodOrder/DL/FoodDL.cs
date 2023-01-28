using FoodOrder.Helper;
using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodOrder.DL
{
    public class FoodDL
    {
        public Food GetDetail(int Id)
        {
            string connString = Utility.GetConfig("DbConfig");
            var rs = new Food();

            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = String.Format(@"
                       SELECT * FROM Menu WHERE ID = {0} ", Id);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            rs = new Food();
                            rs.ID = (int)dr["ID"];
                            rs.Name = dr["Name"].ToString();
                            rs.Description = dr["Description"].ToString();
                            rs.Price = float.Parse(dr["Price"].ToString());

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
        public void Get(string filterSring, int page, int limit, out int totalPage, out List<Food> rs)
        {
            totalPage = 1;

            string connString = Utility.GetConfig("DbConfig");
            rs = new List<Food>();
            int fRow = 1 + ((page - 1) * 5);
            int eRow = page * limit;

            string filter = String.Format("WHERE RowNo BETWEEN {0} And {1}", fRow, eRow);
            string filter2 = String.Empty;

            if (!String.IsNullOrWhiteSpace(filterSring))
                filter2 += String.Format(" AND Name LIKE '%{0}%'", filterSring);

            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = String.Format(@"
                        With X as(
	                        select ROW_NUMBER() OVER (Order by (select 0)) as RowNo, * from Menu WHERE 1 = 1 {1}
                        ) Select * from X {0} ", filter, filter2);

                    using (var dr = cmd.ExecuteReader())
                    {
                        Food Temp;
                        while (dr.Read())
                        {
                            Temp = new Food();
                            Temp.ID = (int)dr["ID"];
                            Temp.Name = dr["Name"].ToString();
                            Temp.Price = float.Parse(dr["Price"].ToString());

                            rs.Add(Temp);
                        }
                    }

                    cmd.CommandText = String.Format("select CEILING(COUNT(*)/CAST({1} AS FLOAT)) from Menu WHERE 1 = 1 {0}", filter2, limit);
                    int.TryParse(cmd.ExecuteScalar().ToString(), out totalPage);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}