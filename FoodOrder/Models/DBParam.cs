using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class DBParam
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public string FilterString { get; set; }
    }
}