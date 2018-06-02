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
                var date = new DateTime(dateUtil.Year, dateUtil.Month, dateUtil.Day).AddHours(23);
                return String.Format("{0:yyyy/MM/dd HH:mm:ss}", date);                
            }
            else
            {
                var date = new DateTime(dateUtil.Year, dateUtil.Month, dateUtil.Day);
                return String.Format("{0:yyyy/MM/dd HH:mm:ss}", date.Date);
            }            
        }
    }
}
