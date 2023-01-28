using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FoodOrder.Helper
{
    public static class Utility
    {
        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings.Get(key).ToString();
        }
    }
}