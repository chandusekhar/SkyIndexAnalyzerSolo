using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public class FileDatetimeInfo
    {
        public string filename { get; set; }
        public ServiceTools.DatetimeExtractionMethod datetimeExtractionMethod { get; set; }
        public string datetimeFilenameMask { get; set; }
        public DateTime datetime { get; set; }

        public FileDatetimeInfo()
        { }


        public FileDatetimeInfo(string _filename, ServiceTools.DatetimeExtractionMethod method = ServiceTools.DatetimeExtractionMethod.Filename,
            string _datetimeFilenameMask = "?????xxxxxxxxxxxxxxxxxxx*")
        {
            filename = _filename;
            datetimeExtractionMethod = method;
            datetimeFilenameMask = _datetimeFilenameMask;

            switch (method)
            {
                case ServiceTools.DatetimeExtractionMethod.Filename:
                    {
                        try
                        {
                            string strDateOfFile =
                                String.Concat(
                                    Path.GetFileName(filename)
                                        .Zip(datetimeFilenameMask, (c, s) => (s == 'x') ? (c.ToString()) : (""))
                                        .ToArray());
                            datetime = CommonTools.DateTimeOfString(strDateOfFile);
                            DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        break;
                    }
                case ServiceTools.DatetimeExtractionMethod.FileCreation:
                    {
                        datetime = new FileInfo(filename).CreationTimeUtc;
                        DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
                        break;
                    }
                case ServiceTools.DatetimeExtractionMethod.Content:
                    {
                        Dictionary<string, object> currDict = ServiceTools.ReadDictionaryFromXML(filename);
                        currDict.Add("XMLfileName", Path.GetFileName(filename));
                        ConcurrentData retVal = null;
                        try
                        {
                            retVal = new ConcurrentData(currDict);
                            datetime = retVal.datetimeUTC;
                            DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }
    }
}
