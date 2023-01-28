using FoodOrder.BL;
using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrder.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult ConfirmPayment(string name, string payType)
        {
            this.TempData["Name"] = name;
            this.TempData["PayType"] = payType;

            return Content("1");
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Confirmation()
        {
            if (this.TempData["Name"] != null && this.TempData["PayType"] != null)
            {
                this.ViewData["Name"] = this.TempData["Name"];
                this.ViewData["PayType"] = this.TempData["PayType"];
            }

            return View();
        }

        public ActionResult AddOrder(int id, int qty)
        {
            var order = new Order()
            {
                FoodID = id,
                OrderDate = DateTime.Now,
                Quantity = qty
            };

            var bl = new OrderBL();
            var rs = bl.AddOrder(order);

            return Content(rs.ToString());
        }

        [HttpPost]
        public ActionResult DeleteOrder(int id)
        {
            var bl = new OrderBL();
            var rs = bl.DeleteOrder(id);

            return Content(rs.ToString());
        }

        public ActionResult PaymentConfirmed(string name, string payType)
        {
            var confirm = new ConfirmPayment() {
                TransactionDate = DateTime.Now,
                Name = name,
                PayType = payType
            };

            var bl = new OrderBL();
            bl.PaymentConfirmed(confirm);

            return Content("1");
        }
    }
}