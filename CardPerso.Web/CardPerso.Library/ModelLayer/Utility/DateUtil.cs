using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Utility
{
    public class DateUtil
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public static string GetDate(DateUtil dateUtil, bool addDay)
        {
            if (addDay)
            {
                return $"{dateUtil.Year}/{dateUtil.Month}/{dateUtil.Day + 1}";
            }
            else
            {
                return $"{dateUtil.Year}/{dateUtil.Month}/{dateUtil.Day}";
            }            
        }
    }
}
