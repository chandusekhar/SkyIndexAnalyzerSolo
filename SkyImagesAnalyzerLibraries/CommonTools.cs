using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

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





        public static string FindConcurrentDataXMLfile(string imgFileName, out string err, string basePath = "")
        {
            if (basePath == "")
            {
                basePath = Directory.GetCurrentDirectory();
                basePath = basePath +
                           ((basePath.Last() == Path.DirectorySeparatorChar)
                               ? ("")
                               : (Path.DirectorySeparatorChar.ToString()));
                basePath += "results"+Path.DirectorySeparatorChar;
            }

            DirectoryInfo dirConcurrentDataFiles = new DirectoryInfo(basePath);
            
            string strDateTime = Path.GetFileName(imgFileName);
            strDateTime = strDateTime.Substring(4, 19);
            DateTime currImgDateTime = CommonTools.DateTimeOfString(strDateTime);

            FileInfo[] concurrentDataFilesList =
                dirConcurrentDataFiles.GetFiles(
                    "data-" + currImgDateTime.ToString("s").Replace(":", "-").Substring(0, 16) + "*.xml",
                    SearchOption.TopDirectoryOnly);
            if (concurrentDataFilesList.Any())
            {
                List<Tuple<string, Dictionary<string, object>>> lTplConcurrentDataFilesInfo = new List<FileInfo>(
                    concurrentDataFilesList)
                    .ConvertAll<Tuple<string, Dictionary<string, object>>>(
                        finfo =>
                        {
                            Dictionary<string, object> dictSavedData =
                                ServiceTools.ReadDictionaryFromXML(concurrentDataFilesList[0].FullName) as
                                    Dictionary<string, object>;
                            return new Tuple<string, Dictionary<string, object>>(finfo.FullName, dictSavedData);
                        });

                lTplConcurrentDataFilesInfo.RemoveAll(
                    tpl =>
                        !(new GPSdata((string)(tpl.Item2["GPSdata"]), GPSdatasources.CloudCamArduinoGPS,
                            DateTime.Parse((string)tpl.Item2["GPSDateTimeUTC"], null, DateTimeStyles.RoundtripKind))).validGPSdata);

                if (!lTplConcurrentDataFilesInfo.Any())
                {
                    err = "========== ERROR: couldn`t find concurrent data file for " + imgFileName;
                    return "";
                }

                List<Tuple<string, Dictionary<string, object>, long>> lTplConcurrentData = lTplConcurrentDataFilesInfo
                    .ConvertAll(
                        tpl =>
                        {
                            GPSdata currDataGPS = new GPSdata((string)(tpl.Item2["GPSdata"]),
                                GPSdatasources.CloudCamArduinoGPS,
                                DateTime.Parse((string)tpl.Item2["GPSDateTimeUTC"], null, DateTimeStyles.RoundtripKind));
                            long dt = (currDataGPS.DateTimeUTC - currImgDateTime).Ticks;
                            return new Tuple<string, Dictionary<string, object>, long>(tpl.Item1, tpl.Item2, dt);
                        });
                lTplConcurrentData.Sort(CompareTuplesByIntItem3);

                //get the first item as closest
                err = "";
                return lTplConcurrentData[0].Item1;
            }
            else
            {
                err = "========== ERROR: couldn`t find concurrent data file for " + imgFileName;
                return "";
            }
        }






        public static async Task<string> FindConcurrentDataXMLfileAsync(string ImageFilename, string basePath = "",
            bool searchFilesRecursively = true,
            ServiceTools.DatetimeExtractionMethod method = ServiceTools.DatetimeExtractionMethod.Filename)
        {
            if (basePath == "")
            {
                basePath = Directory.GetCurrentDirectory();
                basePath = basePath +
                           ((basePath.Last() == Path.DirectorySeparatorChar)
                               ? ("")
                               : (Path.DirectorySeparatorChar.ToString()));
                basePath += "results" + Path.DirectorySeparatorChar;
            }


            string nearestConcurrentDataXMLfilename = "";



            string currImgFilename = ImageFilename;
            ServiceTools.DatetimeExtractionMethod _method = method;

            #region find most close concurrent data XML file

            List<FileDatetimeInfo> lImagesConcurrentDataFilesInfo = new List<FileDatetimeInfo>();
            
            List<string> filesListConcurrentData =
                Directory.EnumerateFiles(basePath,
                    ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(),
                    (searchFilesRecursively) ? (SearchOption.AllDirectories) : (SearchOption.TopDirectoryOnly))
                    .ToList();


            foreach (string strConcDataXMLFile in filesListConcurrentData)
            {
                try
                {
                    lImagesConcurrentDataFilesInfo.Add(new FileDatetimeInfo(strConcDataXMLFile,
                        _method));
                }
                catch (Exception ex)
                {
                    continue;
                }
            }


            string currImgFilenameWOext = Path.GetFileNameWithoutExtension(currImgFilename);

            string ptrn = @"(devID\d)";
            Regex rgxp = new Regex(ptrn, RegexOptions.IgnoreCase);

            string strCurrImgDT = rgxp.Replace(currImgFilenameWOext.Substring(4), "");
            //2015-12-16T06-01-38
            strCurrImgDT = strCurrImgDT.Substring(0, 11) + strCurrImgDT.Substring(11).Replace("-", ":");

            DateTime currImgDT = DateTime.Parse(strCurrImgDT, null,
                DateTimeStyles.AdjustToUniversal);
            DateTime.SpecifyKind(currImgDT, DateTimeKind.Utc);

            FileDatetimeInfo nearestConcurrentDataXMLfileInfo = lImagesConcurrentDataFilesInfo.Aggregate((cDtFinfo1, cDtFinfo2) =>
            {
                TimeSpan tspan1 = new TimeSpan(Math.Abs((cDtFinfo1.datetime - currImgDT).Ticks));
                TimeSpan tspan2 = new TimeSpan(Math.Abs((cDtFinfo2.datetime - currImgDT).Ticks));
                return ((tspan1 <= tspan2) ? (cDtFinfo1) : (cDtFinfo2));
            });


            if (new TimeSpan(Math.Abs((nearestConcurrentDataXMLfileInfo.datetime - currImgDT).Ticks)) >=
                new TimeSpan(0, 3, 0))
            {
                //theLogWindow = ServiceTools.LogAText(theLogWindow,
                //    "couldn`t find close enough concurrent data file for image:" + Environment.NewLine +
                //    currImgFilename + Environment.NewLine + "closest concurrent data file is:" +
                //    Environment.NewLine + nearestConcurrentDataXMLfileInfo.filename + Environment.NewLine +
                //    "with date-time value " + nearestConcurrentDataXMLfileInfo.datetime.ToString("o"));
                nearestConcurrentDataXMLfilename = "";
            }
            else
            {
                nearestConcurrentDataXMLfilename = nearestConcurrentDataXMLfileInfo.filename;
            }


            #endregion find most close concurrent data


            return nearestConcurrentDataXMLfilename;
        }







        public static string FindConcurrentDataXMLfile(string ImageFilename, string basePath = "",
            bool searchFilesRecursively = true,
            ServiceTools.DatetimeExtractionMethod method = ServiceTools.DatetimeExtractionMethod.Filename)
        {
            if (basePath == "")
            {
                basePath = Directory.GetCurrentDirectory();
                basePath = basePath +
                           ((basePath.Last() == Path.DirectorySeparatorChar)
                               ? ("")
                               : (Path.DirectorySeparatorChar.ToString()));
                basePath += "results" + Path.DirectorySeparatorChar;
            }


            string nearestConcurrentDataXMLfilename = "";



            string currImgFilename = ImageFilename;
            ServiceTools.DatetimeExtractionMethod _method = method;

            #region find most close concurrent data XML file

            List<FileDatetimeInfo> lImagesConcurrentDataFilesInfo = new List<FileDatetimeInfo>();

            List<string> filesListConcurrentData =
                Directory.EnumerateFiles(basePath,
                    ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(),
                    (searchFilesRecursively) ? (SearchOption.AllDirectories) : (SearchOption.TopDirectoryOnly))
                    .ToList();


            foreach (string strConcDataXMLFile in filesListConcurrentData)
            {
                try
                {
                    lImagesConcurrentDataFilesInfo.Add(new FileDatetimeInfo(strConcDataXMLFile,
                        _method));
                }
                catch (Exception ex)
                {
                    continue;
                }
            }


            string currImgFilenameWOext = Path.GetFileNameWithoutExtension(currImgFilename);

            string ptrn = @"(devID\d)";
            Regex rgxp = new Regex(ptrn, RegexOptions.IgnoreCase);

            string strCurrImgDT = rgxp.Replace(currImgFilenameWOext.Substring(4), "");
            //2015-12-16T06-01-38
            strCurrImgDT = strCurrImgDT.Substring(0, 11) + strCurrImgDT.Substring(11).Replace("-", ":");

            DateTime currImgDT = DateTime.Parse(strCurrImgDT, null,
                DateTimeStyles.AdjustToUniversal);
            DateTime.SpecifyKind(currImgDT, DateTimeKind.Utc);

            FileDatetimeInfo nearestConcurrentDataXMLfileInfo = lImagesConcurrentDataFilesInfo.Aggregate((cDtFinfo1, cDtFinfo2) =>
            {
                TimeSpan tspan1 = new TimeSpan(Math.Abs((cDtFinfo1.datetime - currImgDT).Ticks));
                TimeSpan tspan2 = new TimeSpan(Math.Abs((cDtFinfo2.datetime - currImgDT).Ticks));
                return ((tspan1 <= tspan2) ? (cDtFinfo1) : (cDtFinfo2));
            });


            if (new TimeSpan(Math.Abs((nearestConcurrentDataXMLfileInfo.datetime - currImgDT).Ticks)) >=
                new TimeSpan(0, 3, 0))
            {
                //theLogWindow = ServiceTools.LogAText(theLogWindow,
                //    "couldn`t find close enough concurrent data file for image:" + Environment.NewLine +
                //    currImgFilename + Environment.NewLine + "closest concurrent data file is:" +
                //    Environment.NewLine + nearestConcurrentDataXMLfileInfo.filename + Environment.NewLine +
                //    "with date-time value " + nearestConcurrentDataXMLfileInfo.datetime.ToString("o"));
                nearestConcurrentDataXMLfilename = "";
            }
            else
            {
                nearestConcurrentDataXMLfilename = nearestConcurrentDataXMLfileInfo.filename;
            }


            #endregion find most close concurrent data


            return nearestConcurrentDataXMLfilename;
        }









        private static int CompareTuplesByIntItem3(Tuple<string, Dictionary<string, object>, long> x,
            Tuple<string, Dictionary<string, object>, long> y)
        {
            return x.Item3.CompareTo(y.Item3);
        }






        public static bool console_present()
        {
            bool _console_present = true;
            try { int window_height = Console.WindowHeight; }
            catch { _console_present = false; }

            return _console_present;
        }


        public static void PrintDictionaryToConsole(Dictionary<string, object> dict, string title="", Func<object, string> objToStringConversion = null)
        {
            if (!console_present())
            {
                return;
            }

            if (objToStringConversion == null)
            {
                objToStringConversion = obj => obj.ToString();
            }

            int consWidth = Console.WindowWidth;

            Dictionary<string, string> dctToShow = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> pair in dict)
            {
                dctToShow.Add(pair.Key, objToStringConversion(pair.Value));
            }

            int maxKeyLength = dctToShow.Keys.ToList().ConvertAll<int>(k => k.Length).Max();
            int maxValuesLength = dctToShow.Values.ToList().ConvertAll<int>(k => k.Length).Max();

            int consWidthHalved = (consWidth / 2) - 1;
            int sepPosition = consWidthHalved;
            int keysColumnWidth = sepPosition - 1;
            int valsColumnWidth = consWidth - sepPosition - 1;
            int valsColumnPosition = sepPosition + 1;

            if (maxKeyLength + maxValuesLength + 3 <= consWidth)
            {
                keysColumnWidth = maxKeyLength + 1;
                sepPosition = keysColumnWidth + 1;
                valsColumnPosition = sepPosition + 1;
                valsColumnWidth = consWidth - valsColumnPosition - 1;
            }
            else
            {
                sepPosition = consWidthHalved;
                keysColumnWidth = sepPosition - 1;
                valsColumnPosition = sepPosition + 1;
                valsColumnWidth = consWidth - valsColumnPosition - 1;
            }

            Console.WriteLine(title);
            foreach (KeyValuePair<string, string> pair in dctToShow)
            {
                string strToPrint = String.Format("{0," + keysColumnWidth + "}", pair.Key) + " | " +
                                    String.Format("{0," + valsColumnWidth + "}", pair.Value);
                Console.WriteLine(strToPrint);
            }

        }




        public static string DictionaryRepresentation(Dictionary<string, object> dict, string title = "",
            Func<object, string> objToStringConversion = null)
        {
            string retStr = "";

            if (objToStringConversion == null)
            {
                objToStringConversion = obj => obj.ToString();
            }

            int consWidth = 70;

            Dictionary<string, string> dctToShow = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> pair in dict)
            {
                dctToShow.Add(pair.Key, objToStringConversion(pair.Value));
            }

            int maxKeyLength = dctToShow.Keys.ToList().ConvertAll<int>(k => k.Length).Max();
            int maxValuesLength = dctToShow.Values.ToList().ConvertAll<int>(k => k.Length).Max();

            int consWidthHalved = (consWidth / 2) - 1;
            int sepPosition = consWidthHalved;
            int keysColumnWidth = sepPosition - 1;
            int valsColumnWidth = consWidth - sepPosition - 1;
            int valsColumnPosition = sepPosition + 1;

            if (maxKeyLength + maxValuesLength + 3 <= consWidth)
            {
                keysColumnWidth = maxKeyLength + 1;
                sepPosition = keysColumnWidth + 1;
                valsColumnPosition = sepPosition + 1;
                valsColumnWidth = consWidth - valsColumnPosition - 1;
            }
            else
            {
                sepPosition = consWidthHalved;
                keysColumnWidth = sepPosition - 1;
                valsColumnPosition = sepPosition + 1;
                valsColumnWidth = consWidth - valsColumnPosition - 1;
            }

            retStr += title + Environment.NewLine;
            foreach (KeyValuePair<string, string> pair in dctToShow)
            {
                string strToPrint = String.Format("{0," + keysColumnWidth + "}", pair.Key) + " | " +
                                    String.Format("{0," + valsColumnWidth + "}", pair.Value);
                retStr += strToPrint + Environment.NewLine;
            }

            return retStr;
        }





        public static double ParseDouble(string value)
        {
            return ParseDouble(value, 0.0d);
        }



        public static double ParseDouble(string value, double defaultValue = 0.0d)
        {
            double result;

            //Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                //Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
