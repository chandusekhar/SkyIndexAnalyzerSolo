using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarPositioning
{
    /// <summary>
    /// A simple wrapper class for keeping an azimuth/zenith angle pair of values.
    /// </summary>
    public class AzimuthZenithAngle
    {
        private double azimuth;
        private double zenithAngle;

        public AzimuthZenithAngle(double azimuth, double zenithAngle)
        {
            this.zenithAngle = zenithAngle;
            this.azimuth = azimuth;
        }

        public double ZenithAngle
        {
            get { return zenithAngle; }
        }


        public double ZenithAngleRad
        {
            get { return (Math.PI*zenithAngle/180.0d); }
        }



        public double ElevationAngle
        {
            get { return 90.0d - zenithAngle; }
        }



        public double ElevationAngleRad
        {
            get { return Math.PI*ElevationAngle/180.0d; }
        }



        public double Azimuth
        {
            get { return azimuth; }
        }



        public double AzimuthRad
        {
            get { return Math.PI*azimuth/180.0d; }
        }



        public override string ToString()
        {
            string outStr = string.Format("azimuth {0:N} deg, zenith angle {1:N} deg", azimuth, zenithAngle);
            outStr += Environment.NewLine + "elevation angle " + ElevationAngle + " deg";
            return outStr;
        }
    }
}
