using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Restpelicula.Helpers
{
    public class Datehelpers
    {
        public static DateTime Responsedate()
        {
            var date = DateTime.Today;
            string s2 = date.ToString("yyyy-MM-dd");
            DateTime dtnew = DateTime.Parse(s2);

            return dtnew;
        }
    }
}