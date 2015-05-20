using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public static class CommonTools
    {
        public static DateTime DateTimeOfString(string strDateTime)
        {
            // 2014:07:24 01:07:45
            DateTime dt = new DateTime(
                Convert.ToInt32(strDateTime.Substring(0, 4)),
                Convert.ToInt32(strDateTime.Substring(5, 2)),
                Convert.ToInt32(strDateTime.Substring(8, 2)),
                Convert.ToInt32(strDateTime.Substring(11, 2)),
                Convert.ToInt32(strDateTime.Substring(14, 2)),
                Convert.ToInt32(strDateTime.Substring(17, 2)));
            return dt;
        }


        public static DateTime RoundToHour(DateTime dt)
        {
            long ticks = dt.Ticks + 18000000000;
            return new DateTime(ticks - ticks % 36000000000);
        }
    }
}
