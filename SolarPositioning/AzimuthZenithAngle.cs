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

        public double getZenithAngle()
        {
            return zenithAngle;
        }

        public double getAzimuth()
        {
            return azimuth;
        }

        public override string ToString()
        {
            string outStr = string.Format("azimuth {0:N} deg, zenith angle {1:N} deg", azimuth, zenithAngle);
            outStr += Environment.NewLine + "elevation angle " + (90.0 - zenithAngle).ToString() + " deg";
            return outStr;
        }
    }
}
