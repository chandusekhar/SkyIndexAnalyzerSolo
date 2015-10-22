using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public interface ICsvExportable
    {
        string CSVHeader();
        string ToCSV();
    }



    public abstract class CsvExportable : ICsvExportable
    {
        public string CSVHeader()
        {
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<string> lStrHeaders = new List<string>();
            string retStr = "";

            foreach (string propertyName in lStrPropertiesNames)
            {
                lStrHeaders.Add(propertyName);
            }

            retStr = String.Join(",", lStrHeaders.ToArray<string>());
            return retStr;
        }




        public string ToCSV()
        {
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<string> lStrValues = new List<string>();
            string retStr = "";

            foreach (string propertyName in lStrPropertiesNames)
            {

                object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
                if ((propValue.GetType() == typeof(double)) || (propValue.GetType() == typeof(float)))
                {
                    lStrValues.Add(propValue.ToString().Replace(",", "."));
                }
                else
                {
                    lStrValues.Add(propValue.ToString());
                }

            }

            retStr = String.Join(",", lStrValues.ToArray<string>());
            return retStr;
        }
    }
}
