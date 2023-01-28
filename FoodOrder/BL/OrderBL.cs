using FoodOrder.DL;
using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrder.BL
{
    public class OrderBL
    {
        public List<Order> GetOrder()
        {
            var rs = new List<Order>();
            try
            {
                var dl = new OrderDL();
                rs = dl.GetOrder();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rs;
        }

        public int AddOrder(Order order)
        {
            int rs = 0;
            try
            {
                var dl = new OrderDL();
                rs = dl.AddOrder(order);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return rs;
        }

        public int DeleteOrder(int id)
        {
            int rs = 0;
            try
            {
                var dl = new OrderDL();
                rs = dl.DeleteOrder(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rs;
        }

        public int PaymentConfirmed(ConfirmPayment confirm)
        {
            int rs = 0;

            try
            {
                var dl = new OrderDL();
                dl.PaymentConfirmed(confirm);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rs;
        }
    }
}