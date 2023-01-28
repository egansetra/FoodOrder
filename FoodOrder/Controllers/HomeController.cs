using FoodOrder.BL;
using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search()
        {
            ViewBag.Message = "Search";

            return View();
        }

        public ActionResult OrderCart(bool deletable)
        {
            ViewBag.Message = "Daftar Transaksi";
            var bl = new OrderBL();
            var list = bl.GetOrder();

            float totalPrc = 0;

            foreach (var data in list)
            {
                totalPrc += data.Subtotal;
            }

            this.ViewData["Total"] = totalPrc;
            this.ViewData["Deletable"] = deletable;

            return View(list);
        }

        public ActionResult Order()
        {
            ViewBag.Message = "Order";
            string strId = Request.QueryString.Get("id");
            int id = 0;
            int.TryParse(strId, out id);
            var order = new Food();

            if (id > 0)
            {
                var bl = new FoodBL();
                order = bl.GetDetail(id);
            }

            return View(order);
        }

        public ActionResult View(DBParam dbParam)
        {
            ViewBag.Message = "Search";
            this.ViewData["Page"] = 0;
            int page = 1;
            int totalPage = 0;

            var list = new List<Food>();
            string strName = Request.QueryString.Get("name");
            string strPage = Request.QueryString.Get("page");

            if (int.TryParse(strPage, out page))
            {
                var bl = new FoodBL();
                bl.Get(strName, page, 5, out totalPage, out list);

                this.ViewData["Page"] = totalPage;
            }

            return PartialView(list);
        }
    }
}