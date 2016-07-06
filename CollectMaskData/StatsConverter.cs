using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using MathNet.Numerics.Statistics;
using SkyImagesAnalyzerLibraries;

namespace CollectMaskDataApp
{
    public class StatsConverter
    {
        private string currPath2Process = "";
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        string outputDataFile = "imageRD-stats.xml";

        private string strOutputDirectory = "";
        private string FilesToProcessMask = "";
        private bool cbSearchImagesTopDirectoryOnly = true;

        private double filterDefaultDoubleValueCenterX = 0.0d;
        private double filterDefaultDoubleValueCenterY = 0.0d;
        private double filterDefaultDoubleValueRadius = 0.0d;

        private List<ImagesConvertingData> lStatsConversion = new List<ImagesConvertingData>();
        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";



        public void Start(string[] args)
        {
            readDefaultProperties();

            List<string> argsList = new List<string>(args);

            if (argsList.Any(str => str.Contains("--convert-existing-stats=")))
            {
                string foundArg = argsList.Find(str => str.Substring(0, 25) == "--convert-existing-stats=");
                FilesToProcessMask = foundArg.Replace("--convert-existing-stats=", "");
                Console.WriteLine("files mask to process: \"" + FilesToProcessMask + "\"");
            }

            if (argsList.Find(str => str == "--recursive") != null)
            {
                cbSearchImagesTopDirectoryOnly = false;
            }


            if (argsList.Find(str => str.Substring(0, 38) == "--filter-default-CenterX-double-value=") != null)
            {
                string foundArg = argsList.Find(str => str.Substring(0, 38) == "--filter-default-CenterX-double-value=");
                string strValue = foundArg.Replace("--filter-default-CenterX-double-value=", "");
                filterDefaultDoubleValueCenterX = Convert.ToDouble(strValue.Replace(".", ","));
            }


            if (argsList.Find(str => str.Substring(0, 38) == "--filter-default-CenterY-double-value=") != null)
            {
                string foundArg = argsList.Find(str => str.Substring(0, 38) == "--filter-default-CenterY-double-value=");
                string strValue = foundArg.Replace("--filter-default-CenterY-double-value=", "");
                filterDefaultDoubleValueCenterY = Convert.ToDouble(strValue.Replace(".", ","));
            }


            if (argsList.Find(str => str.Substring(0, 37) == "--filter-default-Radius-double-value=") != null)
            {
                string foundArg = argsList.Find(str => str.Substring(0, 37) == "--filter-default-Radius-double-value=");
                string strValue = foundArg.Replace("--filter-default-Radius-double-value=", "");
                filterDefaultDoubleValueRadius = Convert.ToDouble(strValue.Replace(".", ","));
            }




            ServiceTools.logToTextFile(outputDataFile,
                "processing data files using pattern: " + FilesToProcessMask + Environment.NewLine, true);

            Console.WriteLine("getting files list");
            EnumerateFilesToProcess();

            foreach (ImagesConvertingData enumObj in lStatsConversion)
            {
                CurrFileProcessing(enumObj);
            }

        }





        private void EnumerateFilesToProcess()
        {
            string directory = Path.GetDirectoryName(FilesToProcessMask);
            string filemask = FilesToProcessMask.Replace(directory + Path.DirectorySeparatorChar, "");
            List<string> filesList =
                new List<string>(Directory.EnumerateFiles(directory, filemask,
                    cbSearchImagesTopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));



            if (!filesList.Any())
            {
                Console.WriteLine("There is no data files that sutisfy specified settings. Processing will not be started.");
                return;
            }



            int intFinalIndex = filesList.Count;
            int idx = 0;
            int currPerc = 0;
            int prevPerc = 0;
            foreach (string fName in filesList)
            {
                Console.Write("adding file to calculation queue:" + fName + "\r");
                idx++;

                AddCSVfileToCalculationQueue(fName);
            }


            Console.WriteLine("finished enumerating files. Files to process: " + lStatsConversion.Count);
            ServiceTools.logToTextFile(outputDataFile,
                "files count to process: " + lStatsConversion.Count + Environment.NewLine, true);
        }


        


        private void AddCSVfileToCalculationQueue(string csvFilename)
        {
            if (lStatsConversion.FindIndex(statsDatum => statsDatum.filename == csvFilename) > -1)
            {
                return;
            }

            lStatsConversion.Add(new ImagesConvertingData()
            {
                filename = csvFilename,
                State = ImagesConvertingState.Queued
            });
        }





        private void CurrFileProcessing(ImagesConvertingData dat)
        {
            string currentFullFileName = dat.filename;

            try
            {
                List<List<string>> csvFileContents = ServiceTools.ReadDataFromCSV(currentFullFileName, 3, true, ";", Environment.NewLine);

                List<double> lCenterXValues = csvFileContents.ConvertAll<double>(lStr => Convert.ToDouble(lStr[1]));
                List<double> lCenterYValues = csvFileContents.ConvertAll<double>(lStr => Convert.ToDouble(lStr[2]));
                List<double> lRadiiValues = csvFileContents.ConvertAll<double>(lStr => Convert.ToDouble(lStr[3]));

                // filter default values
                // filteringMarks = "те, которые оставляем"
                List<bool> filteringMarks = csvFileContents.ConvertAll<bool>(lStr => true);
                if (filterDefaultDoubleValueCenterX > 0.0d)
                {
                    List<bool> filteringMarksAdd = lCenterXValues.ConvertAll<bool>(dVal => dVal != filterDefaultDoubleValueCenterX);
                    filteringMarks =
                        (filteringMarks.Zip<bool, bool, bool>(filteringMarksAdd, (bVal1, bVal2) => bVal1 && bVal2))
                            .ToList();
                }

                if (filterDefaultDoubleValueCenterY > 0.0d)
                {
                    List<bool> filteringMarksAdd = lCenterYValues.ConvertAll<bool>(dVal => dVal != filterDefaultDoubleValueCenterY);
                    filteringMarks =
                        (filteringMarks.Zip<bool, bool, bool>(filteringMarksAdd, (bVal1, bVal2) => bVal1 && bVal2))
                            .ToList();
                }

                if (filterDefaultDoubleValueRadius > 0.0d)
                {
                    List<bool> filteringMarksAdd = lRadiiValues.ConvertAll<bool>(dVal => dVal != filterDefaultDoubleValueRadius);
                    filteringMarks =
                        (filteringMarks.Zip<bool, bool, bool>(filteringMarksAdd, (bVal1, bVal2) => bVal1 && bVal2))
                            .ToList();
                }

                List<int> indexes =
                    filteringMarks
                    .Select((val, idx) => new {val, idx})
                    .Where(x => x.val)
                    .Select(x => x.idx)
                    .ToList();
                lCenterXValues = lCenterXValues
                    .Select((dVal, idx) => new {dVal, idx})
                    .Where(x => indexes.Contains(x.idx))
                    .Select(x => x.dVal).ToList();
                lCenterYValues = lCenterYValues
                    .Select((dVal, idx) => new { dVal, idx })
                    .Where(x => indexes.Contains(x.idx))
                    .Select(x => x.dVal).ToList();
                lRadiiValues = lRadiiValues
                    .Select((dVal, idx) => new { dVal, idx })
                    .Where(x => indexes.Contains(x.idx))
                    .Select(x => x.dVal).ToList();

                DescriptiveStatistics stats = new DescriptiveStatistics(lCenterXValues, true);
                double dCenterXvalue = stats.Mean;
                stats = new DescriptiveStatistics(lCenterYValues, true);
                double dCenterYvalue = stats.Mean;
                stats = new DescriptiveStatistics(lRadiiValues, true);
                double dRadiusvalue = stats.Mean;
                RoundData rd = new RoundData(dCenterXvalue, dCenterYvalue, dRadiusvalue);

                string xmlFilename = Path.GetDirectoryName(currentFullFileName);
                xmlFilename += (xmlFilename.Last() == Path.DirectorySeparatorChar)
                    ? ("")
                    : (Path.DirectorySeparatorChar.ToString());
                xmlFilename += Path.GetFileNameWithoutExtension(currentFullFileName) + "-RoundImagemask.xml";


                ServiceTools.WriteObjectToXML(rd, xmlFilename);
                
                Console.WriteLine("finished processing file " + Environment.NewLine + currentFullFileName);

            }
            catch (Exception ex)
            {
                #region report
#if DEBUG
                Console.WriteLine("exception has been thrown: " + ex.Message + Environment.NewLine +
                                  ServiceTools.CurrentCodeLineDescription());
#else
                ServiceTools.logToTextFile(errorLogFilename,
                    "exception has been thrown: " + ex.Message + Environment.NewLine +
                    ServiceTools.CurrentCodeLineDescription(), true, true);
#endif

                #endregion report
            }
        }

        


        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";
            if (!File.Exists(defaultPropertiesXMLfileName)) return;
            defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);

            bool bDefaultPropertiesHasBeenUpdated = false;

            string CurDir = Directory.GetCurrentDirectory();
            currPath2Process = "";
            if (defaultProperties.ContainsKey("ImagesBasePath"))
            {
                currPath2Process = (string)defaultProperties["ImagesBasePath"];
            }
            else
            {
                currPath2Process = CurDir;
            }


            if (defaultProperties.ContainsKey("OutputDirectory"))
            {
                strOutputDirectory = ((string)defaultProperties["OutputDirectory"]);
            }
            else
            {
                strOutputDirectory = "";
            }



            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }




        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }







        #region support classes
        public class ImagesConvertingData
        {
            public string filename
            {
                get; set;
            }
            public ImagesConvertingState State
            {
                get; set;
            }
        }


        public enum ImagesConvertingState
        {
            Queued,
            Calculating,
            Finished,
            Error,
            Aborted
        }
        #endregion
    }
}
