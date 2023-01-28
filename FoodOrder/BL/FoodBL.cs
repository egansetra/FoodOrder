using FoodOrder.DL;
using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrder.BL
{
    public class FoodBL
    {
        public Food GetDetail(int Id)
        {
            var rs = new Food();
            try
            {
                var dl = new FoodDL();
                rs = dl.GetDetail(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rs;
        }
        public void Get(string filterSring, int page, int limit, out int totalPage, out List<Food> rs)
        {
            try
            {
                totalPage = 0;
                rs = new List<Food>();
                var dl = new FoodDL();

                dl.Get(filterSring, page, limit, out totalPage, out rs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}