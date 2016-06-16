using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public class LufftWSdata
    {
        public DateTime dateTimeUTC { get; set; }
        public double TemperatureCels { get; set; }
        public double AirPressureAbs { get; set; }
        public double relHumidity { get; set; }
        public double absHumidity { get; set; }


        public LufftWSdata() { }

        public override string ToString()
        {
            string retStr =
                    "WS:" + Environment.NewLine +
                    "Date,time: " + dateTimeUTC.ToString("dd.MM.yyyy HH:mm:ss") + " (UTC)" + Environment.NewLine +
                    "Temperature [°C]: " + TemperatureCels.ToString("F2") + Environment.NewLine +
                    "Abs. air pressure [hPa]: " + AirPressureAbs.ToString("F2") + Environment.NewLine +
                    "Relative humidity [%]: " + relHumidity.ToString("F2") + Environment.NewLine +
                    "Abs. humidity [g/m³]" + absHumidity.ToString("F2");
            return retStr;
        }


        public LufftWSdata(string strCSVfileEntry)
        {
            List<string> lastWSdataStrings = ServiceTools.StringsDataFromCSVstring(strCSVfileEntry, ";");

            if (lastWSdataStrings.Count == 0)
            {
                throw new Exception("unable to parse CSV string using semicolon delimiter: " + Environment.NewLine +
                                    strCSVfileEntry);
            }
            
            dateTimeUTC = DateTime.ParseExact(lastWSdataStrings[0], "yyyy-MM-dd HH:mm:ss", null);
            DateTime.SpecifyKind(dateTimeUTC, DateTimeKind.Utc);
            
            TemperatureCels = CommonTools.ParseDouble(lastWSdataStrings[1]);
            
            AirPressureAbs = CommonTools.ParseDouble(lastWSdataStrings[2]);
            
            relHumidity = CommonTools.ParseDouble(lastWSdataStrings[3]);
            
            absHumidity = CommonTools.ParseDouble(lastWSdataStrings[4]);
        }
    }






    public class LufftVentusdata
    {
        public DateTime dateTimeUTC { get; set; }
        public double VirtualTemperatureCels { get; set; }
        public double windSpeed { get; set; }
        public double windVectSpeed { get; set; }
        public double windDirection { get; set; }
        public double windVectDirection { get; set; }
        public double AirPressureAbs { get; set; }
        public double windValueQuality { get; set; }


        public LufftVentusdata() { }

        
        public override string ToString()
        {
            string retStr =
                "Ventus:" + Environment.NewLine +
                "Date,time: " + dateTimeUTC.ToString("dd.MM.yyyy HH:mm:ss") + " (UTC)" + Environment.NewLine +
                "Virtual temperature [°C]: " + VirtualTemperatureCels.ToString("F2") + Environment.NewLine +
                "Wind speed [m/s]: " + windSpeed.ToString("F2") + Environment.NewLine +
                "Wind speed [m/s] Vect.:" + windVectSpeed.ToString("F2") + Environment.NewLine +
                "Wind direction [°]: " + windDirection.ToString("F2") + Environment.NewLine +
                "Wind direction [°] Vect.: " + windVectDirection.ToString("F2") + Environment.NewLine +
                "Abs. air pressure [hPa]: " + AirPressureAbs.ToString("F2") + Environment.NewLine +
                "Wind value quality [%]: " + windValueQuality.ToString("F2");
            return retStr;
        }


        public LufftVentusdata(string strCSVfileEntry)
        {
            List<string> lastWSdataStrings = ServiceTools.StringsDataFromCSVstring(strCSVfileEntry, ";");

            if (lastWSdataStrings.Count == 0)
            {
                throw new Exception("unable to parse CSV string using semicolon delimiter: " + Environment.NewLine +
                                    strCSVfileEntry);
            }

            dateTimeUTC = DateTime.ParseExact(lastWSdataStrings[0], "yyyy-MM-dd HH:mm:ss", null);
            DateTime.SpecifyKind(dateTimeUTC, DateTimeKind.Utc);

            VirtualTemperatureCels = CommonTools.ParseDouble(lastWSdataStrings[1]);
            windSpeed = CommonTools.ParseDouble(lastWSdataStrings[2]);
            windVectSpeed = CommonTools.ParseDouble(lastWSdataStrings[3]);
            windDirection = CommonTools.ParseDouble(lastWSdataStrings[4]);
            windVectDirection = CommonTools.ParseDouble(lastWSdataStrings[5]);
            AirPressureAbs = CommonTools.ParseDouble(lastWSdataStrings[6]);
            windValueQuality = CommonTools.ParseDouble(lastWSdataStrings[7]);
        }
    }






    public class LufftR2Sdata
    {
        public DateTime dateTimeUTC { get; set; }
        public double PrecipitationAbs_mm { get; set; }
        public int PrecipitationTypeCode { get; set; }
        public int PrecipitationTypeDescription { get; set; }
        public double AmbientTemperatureCels { get; set; }
        public double PrecipitationIntensity_milPerH { get; set; }


        public LufftR2Sdata() { }


        public LufftR2Sdata(string strCSVfileEntry)
        {
            List<string> lastWSdataStrings = ServiceTools.StringsDataFromCSVstring(strCSVfileEntry, ";");

            if (lastWSdataStrings.Count == 0)
            {
                throw new Exception("unable to parse CSV string using semicolon delimiter: " + Environment.NewLine +
                                    strCSVfileEntry);
            }

            dateTimeUTC = DateTime.ParseExact(lastWSdataStrings[0], "yyyy-MM-dd HH:mm:ss", null);
            DateTime.SpecifyKind(dateTimeUTC, DateTimeKind.Utc);

            PrecipitationAbs_mm = CommonTools.ParseDouble(lastWSdataStrings[1]);
            PrecipitationTypeCode = Convert.ToInt32(lastWSdataStrings[2]);
            AmbientTemperatureCels = CommonTools.ParseDouble(lastWSdataStrings[3]);
            PrecipitationIntensity_milPerH = CommonTools.ParseDouble(lastWSdataStrings[4]);
        }



        public override string ToString()
        {
            string retStr =
                "R2S:" + Environment.NewLine +
                "Date,time:" + dateTimeUTC.ToString("dd.MM.yyyy HH:mm:ss") + " (UTC)" + Environment.NewLine +
                "Precipitation absol. [mm]: " + PrecipitationAbs_mm.ToString("F2") + Environment.NewLine +
                "Precipitation type code: " + PrecipitationTypeCode + Environment.NewLine +
                "Ambient temperature [°C]" + AmbientTemperatureCels.ToString("F2") + Environment.NewLine +
                "Precipitat.intensity [mil/h]: " + PrecipitationIntensity_milPerH.ToString("F2");
            return retStr;
        }



        /*
         
         */
    }
}
