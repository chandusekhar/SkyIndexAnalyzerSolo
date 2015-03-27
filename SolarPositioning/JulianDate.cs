using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarPositioning
{
    /// <summary>
    /// Calculate Julian date for a given point in time. This follows the algorithm described in Reda, I.; Andreas, A.
    /// (2003): Solar Position Algorithm for Solar Radiation Applications. NREL Report No. TP-560-34302, Revised January
    /// 2008.
    /// </summary>
    public class JulianDate
    {
        public DateTime dt = DateTime.UtcNow;
        private GregorianCalendar calendar;
        private double julianDate;
        private double deltaT;


        /// <summary>
        /// Construct a Julian date, assuming deltaT to be 0.
        /// </summary>
        /// <param name="date">date.</param>
        public JulianDate(DateTime date)
        {
            dt = date;
            this.calendar = new GregorianCalendar();
            this.julianDate = calcJulianDate();
            this.deltaT = 0.0;
        }




        /// <summary>
        /// Construct a Julian date, observing deltaT.
        /// </summary>
        /// <param name="date">date.</param>
        /// <param name="deltaT">The delta t.
        /// Difference between earth rotation time and terrestrial time (or Universal Time and Terrestrial Time),
        /// in seconds. See <a href ="http://maia.usno.navy.mil/ser7/deltat.preds">http://maia.usno.navy.mil/ser7/deltat.preds</a>
        /// for values. For the year 2013, a reasonably accurate default would be 67.
        /// </param>
        public JulianDate(DateTime date, double deltaT)
        {
            dt = date;
            this.calendar = new GregorianCalendar();
            this.julianDate = calcJulianDate();
            this.deltaT = deltaT;
        }




        //private GregorianCalendar createGmtCalendar()
        //{
        //    //GregorianCalendar utcCalendar = new GregorianCalendar(TimeZone.getTimeZone("GMT"));
        //    GregorianCalendar utcCalendar = new GregorianCalendar();
        //    utcCalendar.setTimeInMillis(fromCalendar.getTimeInMillis());
        //    utcCalendar.set(Calendar.ERA, fromCalendar.get(Calendar.ERA));
        //    return utcCalendar;
        //}



        private double calcJulianDate()
        {
            int y = (calendar.GetEra(dt) == GregorianCalendar.ADEra) ? calendar.GetYear(dt) : -calendar.GetYear(dt);
            int m = calendar.GetMonth(dt) + 1;

            if (m < 3)
            {
                y = y - 1;
                m = m + 12;
            }

            double d = calendar.GetDayOfMonth(dt)
                       + (calendar.GetHour(dt) + (calendar.GetMinute(dt) + calendar.GetSecond(dt)/60.0d)/60.0d)
                       /24.0d;
            double jd = Math.Floor(365.25d * (y + 4716.0d)) + Math.Floor(30.6001d * (m + 1)) + d - 1524.5d;
            double a = Math.Floor(y / 100.0d);
            double b = (jd > 2299160.0d) ? (2.0d - a + Math.Floor(a/4.0d)) : (0.0d);

            return jd + b;
        }


        public double getJulianDate()
        {
            return julianDate;
        }

        public double getJulianEphemerisDay()
        {
            return julianDate + deltaT/86400.0d;
        }

        public double getJulianCentury()
        {
            return (julianDate - 2451545.0d)/36525.0d;
        }

        public double getJulianEphemerisCentury()
        {
            return (getJulianEphemerisDay() - 2451545.0d)/36525.0d;
        }

        public double getJulianEphemerisMillennium()
        {
            return getJulianEphemerisCentury()/10.0d;
        }


        public override string ToString()
        {
            return julianDate.ToString();
        }
    }
}
