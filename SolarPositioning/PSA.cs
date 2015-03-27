using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarPositioning
{
    public class PSA
    {

        private static double D_EARTH_MEAN_RADIUS = 6371.01; // in km
        private static double D_ASTRONOMICAL_UNIT = 149597890; // in km

        private static double PI = Math.PI;
        private static double TWOPI = (2 * PI);
        private static double RAD = (PI / 180);

        private PSA()
        {
        }

        /**
         * Calculate sun position for a given time and location.
         * 
         * @param date
         *            Note that it's unclear how well the algorithm performs before the year 1990 or after the year 2015.
         * @param latitude
         *            in degrees (positive east of Greenwich)
         * @param longitude
         *            in degrees (positive north of equator)
         * @return
         */
        public static AzimuthZenithAngle calculateSolarPosition(DateTime dateTimeUTC, double latitude, double longitude)
        {
            // Main variables
            double dElapsedJulianDays;
            double dDecimalHours;
            double dEclipticLongitude;
            double dEclipticObliquity;
            double dRightAscension;
            double dDeclination;

            // Auxiliary variables
            double dY;
            double dX;

            // Calculate difference in days between the current Julian Day
            // and JD 2451545.0, which is noon 1 January 2000 Universal Time

            {
                long liAux1;
                long liAux2;
                double dJulianDate;
                // Calculate time of the day in UT decimal hours
                dDecimalHours = dateTimeUTC.Hour + (dateTimeUTC.Minute + dateTimeUTC.Second / 60.0) / 60.0;
                // Calculate current Julian Day
                liAux1 = (dateTimeUTC.Month + 1 - 14) / 12;
                liAux2 = (1461 * (dateTimeUTC.Year + 4800 + liAux1)) / 4
                        + (367 * (dateTimeUTC.Month + 1 - 2 - 12 * liAux1)) / 12
                        - (3 * ((dateTimeUTC.Year + 4900 + liAux1) / 100)) / 4
                        + dateTimeUTC.Day - 32075;
                dJulianDate = (liAux2) - 0.5 + dDecimalHours / 24.0;
                // Calculate difference between current Julian Day and JD 2451545.0
                dElapsedJulianDays = dJulianDate - 2451545.0;
            }

            // Calculate ecliptic coordinates (ecliptic longitude and obliquity of the
            // ecliptic in radians but without limiting the angle to be less than 2*Pi
            // (i.e., the result may be greater than 2*Pi)
            {
                double dMeanLongitude;
                double dMeanAnomaly;
                double dOmega;
                dOmega = 2.1429 - 0.0010394594 * dElapsedJulianDays;
                dMeanLongitude = 4.8950630 + 0.017202791698 * dElapsedJulianDays; // Radians
                dMeanAnomaly = 6.2400600 + 0.0172019699 * dElapsedJulianDays;
                dEclipticLongitude = dMeanLongitude + 0.03341607 * Math.Sin(dMeanAnomaly) + 0.00034894
                        * Math.Sin(2 * dMeanAnomaly) - 0.0001134 - 0.0000203 * Math.Sin(dOmega);
                dEclipticObliquity = 0.4090928 - 6.2140e-9 * dElapsedJulianDays + 0.0000396 * Math.Cos(dOmega);
            }

            // Calculate celestial coordinates ( right ascension and declination ) in radians
            // but without limiting the angle to be less than 2*Pi (i.e., the result
            // may be greater than 2*Pi)
            {
                double dSinEclipticLongitude;
                dSinEclipticLongitude = Math.Sin(dEclipticLongitude);
                dY = Math.Cos(dEclipticObliquity) * dSinEclipticLongitude;
                dX = Math.Cos(dEclipticLongitude);
                dRightAscension = Math.Atan2(dY, dX);
                if (dRightAscension < 0.0)
                {
                    dRightAscension = dRightAscension + 2 * Math.PI;
                }
                dDeclination = Math.Asin(Math.Sin(dEclipticObliquity) * dSinEclipticLongitude);
            }

            // Calculate local coordinates ( azimuth and zenith angle ) in degrees
            {
                double dGreenwichMeanSiderealTime;
                double dLocalMeanSiderealTime;
                double dLatitudeInRadians;
                double dHourAngle;
                double dCosLatitude;
                double dSinLatitude;
                double dCosHourAngle;
                double dParallax;
                dGreenwichMeanSiderealTime = 6.6974243242 + 0.0657098283 * dElapsedJulianDays + dDecimalHours;
                dLocalMeanSiderealTime = (dGreenwichMeanSiderealTime * 15 + longitude) * RAD;
                dHourAngle = dLocalMeanSiderealTime - dRightAscension;
                dLatitudeInRadians = latitude * RAD;
                dCosLatitude = Math.Cos(dLatitudeInRadians);
                dSinLatitude = Math.Sin(dLatitudeInRadians);
                dCosHourAngle = Math.Cos(dHourAngle);
                double zenithAngle = (Math.Acos(dCosLatitude * dCosHourAngle * Math.Cos(dDeclination)
                        + Math.Sin(dDeclination) * dSinLatitude));
                dY = -Math.Sin(dHourAngle);
                dX = Math.Tan(dDeclination) * dCosLatitude - dSinLatitude * dCosHourAngle;
                double azimuth = Math.Atan2(dY, dX);
                if (azimuth < 0.0)
                {
                    azimuth = azimuth + TWOPI;
                }
                azimuth = azimuth / RAD;
                // Parallax Correction
                dParallax = (D_EARTH_MEAN_RADIUS / D_ASTRONOMICAL_UNIT) * Math.Sin(zenithAngle);
                zenithAngle = (zenithAngle + dParallax) / RAD;

                return new AzimuthZenithAngle(azimuth, zenithAngle);
            }
        }
    }
}
