using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    [Serializable]
    public class DateTimeSpan
    {
        private bool empty = true;
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public bool IsEmpty
        {
            get { return empty; }
        }

        public DateTimeSpan()
        {
            start = DateTime.UtcNow;
            end = DateTime.UtcNow;
        }

        public DateTimeSpan(DateTime startDT, DateTime endDT)
        {
            start = startDT;
            end = endDT;
            if (start <= end)
            {
                empty = false;
            }
            else
            {
                start = DateTime.UtcNow;
                end = DateTime.UtcNow;
                empty = true;
            }
            
        }

        #region TimeSpan methods

        public long Ticks
        {
            get { return (end - start).Ticks; }
        }


        public double TotalMilliseconds
        {
            get { return (end - start).TotalMilliseconds; }
        }


        public double TotalSeconds
        {
            get { return (end - start).TotalSeconds; }
        }


        public double TotalMinutes
        {
            get { return (end - start).TotalMinutes; }
        }


        public double TotalHours
        {
            get { return (end - start).TotalHours; }
        }


        public double TotalDays
        {
            get { return (end - start).TotalDays; }
        }

        #endregion TimeSpan methods


        public static DateTimeSpan operator +(DateTimeSpan orig, long ticks)
        {
            DateTimeSpan newDTspan = new DateTimeSpan(orig.start, orig.end);
            newDTspan.end += new TimeSpan(ticks);
            return newDTspan;
        }


        public static DateTimeSpan operator +(DateTimeSpan orig, double milliseconds)
        {
            DateTimeSpan newDTspan = new DateTimeSpan(orig.start, orig.end);
            newDTspan.end += new TimeSpan(Convert.ToInt64(milliseconds*10000.0d));
            return newDTspan;
        }



        public static DateTimeSpan operator -(DateTimeSpan orig, long ticks)
        {
            DateTimeSpan newDTspan = new DateTimeSpan(orig.start, orig.end);
            newDTspan.end -= new TimeSpan(ticks);
            return newDTspan;
        }


        public static DateTimeSpan operator -(DateTimeSpan orig, double milliseconds)
        {
            DateTimeSpan newDTspan = new DateTimeSpan(orig.start, orig.end);
            newDTspan.end -= new TimeSpan(Convert.ToInt64(milliseconds * 10000.0d));
            return newDTspan;
        }



        public static bool operator ==(DateTimeSpan left, DateTimeSpan right)
        {
            return ((left.start == right.start) && (left.end == right.end));
        }



        public static bool operator !=(DateTimeSpan left, DateTimeSpan right)
        {
            return !(left== right);
        }


        public static DateTimeSpan Shift(DateTimeSpan orig, long ticks)
        {
            DateTimeSpan newDTspan = new DateTimeSpan(orig.start, orig.end);
            newDTspan.end += new TimeSpan(ticks);
            newDTspan.start += new TimeSpan(ticks);
            return newDTspan;
        }


        public static DateTimeSpan Shift(DateTimeSpan orig, double milliseconds)
        {
            DateTimeSpan newDTspan = new DateTimeSpan(orig.start, orig.end);
            newDTspan.end += new TimeSpan(Convert.ToInt64(milliseconds * 10000.0d));
            newDTspan.start += new TimeSpan(Convert.ToInt64(milliseconds * 10000.0d));
            return newDTspan;
        }


        public static DateTimeSpan operator ^(DateTimeSpan dts1, DateTimeSpan dts2)
        {
            // return (dts1 + dts2) - (dts2 - dts1) - (dts1 - dts2);
            DateTimeSpan retDTspan = new DateTimeSpan();
            DateTimeSpan dtsL = new DateTimeSpan();
            DateTimeSpan dtsR = new DateTimeSpan();
            if (dts1.start <= dts2.start)
            {
                dtsL = new DateTimeSpan(dts1.start, dts1.end);
                dtsR = new DateTimeSpan(dts2.start, dts2.end);
            }
            else
            {
                dtsR = new DateTimeSpan(dts1.start, dts1.end);
                dtsL = new DateTimeSpan(dts2.start, dts2.end);
            }

            if (dtsL.end < dtsR.start)
            {
                //nothing
            }
            else if ((dtsL.end >= dtsR.start) && (dtsL.end < dtsR.end))
            {
                retDTspan = new DateTimeSpan(dtsR.start, dtsL.end);
            }
            else if ((dtsL.end >= dtsR.start) && (dtsL.end >= dtsR.end))
            {
                retDTspan = new DateTimeSpan(dtsR.start, dtsR.end);
            }
            else
            {
                throw new Exception("Error detecting DateTimeSpan intersection");
            }


            return retDTspan;
        }






        public bool ContainsDateTime(DateTime dt)
        {
            return ((dt >= start) && (dt <= end));
        }




    }
}
