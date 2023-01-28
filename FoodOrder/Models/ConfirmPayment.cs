using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class ConfirmPayment
    {
        public string Name { get; set; }
        public string PayType { get; set; }
        public float Total { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}