using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public class ConcurrentData
    {
        public string filename { get; set; }
        public DateTime datetimeUTC { get; set; }
        public double AccCalibrationValueX { get; set; }
        public double AccCalibrationValueY { get; set; }
        public double AccCalibrationValueZ { get; set; }
        public double AccShiftDoubleX { get; set; }
        public double AccShiftDoubleY { get; set; }
        public double AccShiftDoubleZ { get; set; }

        public double GyroValueX { get; set; }
        public double GyroValueY { get; set; }
        public double GyroValueZ { get; set; }

        public string GPSdata { get; set; }
        public double GPSLat { get; set; }
        public double GPSLon { get; set; }
        public DateTime GPSDateTimeUTC { get; set; }
        public double PressurePa { get; set; }

        public GPSdata gps;


        public ConcurrentData() { }

        public ConcurrentData(Dictionary<string, object> XMLfileContentDictionary)
        {
            filename = (string)XMLfileContentDictionary["XMLfileName"];
            datetimeUTC = DateTime.Parse((string)XMLfileContentDictionary["DateTime"], null,
                System.Globalization.DateTimeStyles.AdjustToUniversal);
            AccCalibrationValueX = Convert.ToDouble(((string)XMLfileContentDictionary["AccCalibrationValueX"]).Replace(".", ","));
            AccCalibrationValueY = Convert.ToDouble(((string)XMLfileContentDictionary["AccCalibrationValueY"]).Replace(".", ","));
            AccCalibrationValueZ = Convert.ToDouble(((string)XMLfileContentDictionary["AccCalibrationValueZ"]).Replace(".", ","));
            AccShiftDoubleX = Convert.ToDouble(((string)XMLfileContentDictionary["AccShiftDoubleX"]).Replace(".", ","));
            AccShiftDoubleY = Convert.ToDouble(((string)XMLfileContentDictionary["AccShiftDoubleY"]).Replace(".", ","));
            AccShiftDoubleZ = Convert.ToDouble(((string)XMLfileContentDictionary["AccShiftDoubleZ"]).Replace(".", ","));

            GyroValueX = Convert.ToDouble(((string)XMLfileContentDictionary["GyroValueX"]).Replace(".", ","));
            GyroValueY = Convert.ToDouble(((string)XMLfileContentDictionary["GyroValueY"]).Replace(".", ","));
            GyroValueZ = Convert.ToDouble(((string)XMLfileContentDictionary["GyroValueZ"]).Replace(".", ","));

            if (XMLfileContentDictionary["GPSdata"] is DBNull)
            {
                throw new Exception("GPSdata string of the XML file dictionary is empty");
            }

            GPSdata = (string)XMLfileContentDictionary["GPSdata"];
            GPSLat = Convert.ToDouble(((string)XMLfileContentDictionary["GPSLat"]).Replace(".", ","));
            GPSLon = Convert.ToDouble(((string)XMLfileContentDictionary["GPSLon"]).Replace(".", ","));
            GPSDateTimeUTC = DateTime.Parse((string)XMLfileContentDictionary["GPSDateTimeUTC"], null,
                System.Globalization.DateTimeStyles.AdjustToUniversal);
            PressurePa = Convert.ToDouble(((string)XMLfileContentDictionary["PressurePa"]).Replace(".", ","));

            gps = new GPSdata()
            {
                GPSstring = GPSdata,
                lat = Math.Abs(GPSLat),
                latHemisphere = ((GPSLat >= 0.0d) ? ("N") : ("S")),
                lon = Math.Abs(GPSLon),
                lonHemisphere = ((GPSLon >= 0) ? ("E") : ("W")),
                dateTimeUTC = GPSDateTimeUTC,
                validGPSdata = true,
                dataSource = GPSdatasources.CloudCamArduinoGPS
            };
        }
    }
}
