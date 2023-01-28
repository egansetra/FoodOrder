using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int FoodID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float Subtotal { get; set; }
        public DateTime OrderDate { get; set; }
    }
}