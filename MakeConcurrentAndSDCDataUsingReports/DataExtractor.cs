using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SkyImagesAnalyzerLibraries;
using System.Drawing;

namespace MakeConcurrentAndSDCDataUsingReports
{
    public class DataExtractor
    {
        private string currPath2Process = "";
        private string SunDiskConditionXMLdataFilesDirectory = "";
        private string imageYRGBstatsXMLdataFilesDirectory = "";
        private string strConcurrentGPSdataXMLfilesPath = "";
        private string IoffeMeteoNavFilesDirectory = "";
        private string strSkyImagesDataWithConcurrentStatsCloudCoverAndSDCDirectory = "";

        private string strObservationsReportCSVfile = "";

        private AbortableBackgroundWorkerWithParameters bgwCurrImageProcessing = null;
        private SunDiskConditionData currImageSunDiskConditionData;
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private bool bShuffleImages = false;
        private bool bPermitRepeats = false;
        private int maxConcurrentImagesProcessing = 4;
        private List<string> alreadyMarkedSunDiskConditionFiles = new List<string>();
        private bool bReviewMode = false;
        private string SerializedDecisionTreePath = "";
        private bool bPermitProcessingStart = false;
        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";

        private string strSourceDirectory = "";
        private string strOutputDirectory = "";
        private bool bSearchImagesTopDirectoryOnly = true;
        private bool bStartWithoutConfirmation = false;
        //private string FilesToProcessMask = "";
        private List<ImageStatsCollectingData> lStatsCalculation = new List<ImageStatsCollectingData>();
        private int totalFilesCountToProcess = 0;
        private int totalFilesProcessed = 0;
        private CancellationTokenSource cts;





        public void Start(string[] args)
        {
            readDefaultProperties();

            List<string> argsList = new List<string>(args);

            if (argsList.Find(str => str == "--recursive") != null)
            {
                bSearchImagesTopDirectoryOnly = false;
                defaultProperties.Add("SearchImagesTopDirectoryOnly", bSearchImagesTopDirectoryOnly);
            }

            if (argsList.Find(str => str == "-y") != null)
            {
                bStartWithoutConfirmation = true;
                defaultProperties.Add("StartWithoutConfirmation", bStartWithoutConfirmation);
            }

            CommonTools.PrintDictionaryToConsole(defaultProperties, "effective properties list");
            Console.WriteLine("");

            if (!bStartWithoutConfirmation)
            {
                Console.Write("Start with the mentioned properties? [y/n] ");
                string strReply = Console.ReadLine();
                if (strReply.ToLower().First() == 'y')
                {
                    ExtractAndWriteData();
                }
                else
                {
                    Console.WriteLine("\nWill not proceed due to user interruprion.");
                    Console.WriteLine("===FINISHED===");
                    return;
                }
            }
            else
            {
                ExtractAndWriteData();
            }

        }




        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";

            if (File.Exists(defaultPropertiesXMLfileName))
            {
                defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);
            }
            

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
                defaultProperties.Add("ImagesBasePath", currPath2Process);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            //imageYRGBstatsXMLdataFilesDirectory
            if (defaultProperties.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
            {
                imageYRGBstatsXMLdataFilesDirectory = (string)defaultProperties["imageYRGBstatsXMLdataFilesDirectory"];
            }
            else
            {
                imageYRGBstatsXMLdataFilesDirectory = CurDir;
                defaultProperties.Add("imageYRGBstatsXMLdataFilesDirectory", imageYRGBstatsXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }


            // strConcurrentGPSdataXMLfilesPath
            if (defaultProperties.ContainsKey("strConcurrentGPSdataXMLfilesPath"))
            {
                strConcurrentGPSdataXMLfilesPath = (string)defaultProperties["strConcurrentGPSdataXMLfilesPath"];
            }
            else
            {
                strConcurrentGPSdataXMLfilesPath = CurDir;
                defaultProperties.Add("strConcurrentGPSdataXMLfilesPath", strConcurrentGPSdataXMLfilesPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }




            // strOverallImagesConcurrentDataDirectory
            if (defaultProperties.ContainsKey("strSkyImagesDataWithConcurrentStatsCloudCoverAndSDCDirectory"))
            {
                strSkyImagesDataWithConcurrentStatsCloudCoverAndSDCDirectory = (string)defaultProperties["strSkyImagesDataWithConcurrentStatsCloudCoverAndSDCDirectory"];
            }
            else
            {
                strSkyImagesDataWithConcurrentStatsCloudCoverAndSDCDirectory = CurDir;
                defaultProperties.Add("strSkyImagesDataWithConcurrentStatsCloudCoverAndSDCDirectory", strSkyImagesDataWithConcurrentStatsCloudCoverAndSDCDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }




            if (defaultProperties.ContainsKey("SunDiskConditionXMLdataFilesDirectory"))
            {
                SunDiskConditionXMLdataFilesDirectory = (string)defaultProperties["SunDiskConditionXMLdataFilesDirectory"];
            }
            else
            {
                SunDiskConditionXMLdataFilesDirectory = CurDir;
                defaultProperties.Add("SunDiskConditionXMLdataFilesDirectory", SunDiskConditionXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }




            // IoffeMeteoNavFilesDirectory
            if (defaultProperties.ContainsKey("IoffeMeteoNavFilesDirectory"))
            {
                IoffeMeteoNavFilesDirectory = (string)defaultProperties["IoffeMeteoNavFilesDirectory"];
            }
            else
            {
                IoffeMeteoNavFilesDirectory = CurDir;
                defaultProperties.Add("IoffeMeteoNavFilesDirectory", IoffeMeteoNavFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            // strObservationsReportCSVfile
            if (defaultProperties.ContainsKey("strObservationsReportCSVfile"))
            {
                strObservationsReportCSVfile = (string) defaultProperties["strObservationsReportCSVfile"];
            }
            else
            {
                strObservationsReportCSVfile = CurDir + Path.DirectorySeparatorChar + "meteo-report.csv";
                defaultProperties.Add("strObservationsReportCSVfile", strObservationsReportCSVfile);
                bDefaultPropertiesHasBeenUpdated = true;
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







        private void ExtractAndWriteData()
        {
            Console.WriteLine("getting files list");
            EnumerateFilesToProcess();



            ReadObservationsReportCSVfile();



            totalFilesCountToProcess = lStatsCalculation.Count;


            foreach (ImageStatsCollectingData dt in lStatsCalculation)
            {
                ProcessImage(dt);
            }
        }

        




        private void EnumerateFilesToProcess()
        {
            string directory = Path.GetDirectoryName(currPath2Process);
            string filemask = "*.jpg";
            List<string> filesList =
                new List<string>(Directory.EnumerateFiles(directory, filemask,
                    bSearchImagesTopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));

            Console.WriteLine("found " + filesList.Count + " files.");
            Console.WriteLine("filtering...");

            List<string> statsFilesList =
                new List<string>(Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory, ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern(),
                    bSearchImagesTopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));
            statsFilesList = statsFilesList.ConvertAll(strFileName => Path.GetFileName(strFileName));

            int removed = filesList.RemoveAll(
                fname =>
                    !statsFilesList.Contains(ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(fname, "", false)));

            Console.WriteLine("removed " + removed + " items. Remains " + filesList.Count + " to process.");

            if (!filesList.Any())
            {
                Console.WriteLine("There is no " + filemask + " files that sutisfy settings specified. Processing will not be started.");
                return;
            }



            int intFinalIndex = filesList.Count;
            int idx = 0;
            int currPerc = 0;
            int prevPerc = 0;
            int consoleWidth = Console.WindowWidth;
            foreach (string fName in filesList)
            {
                string strToShow = String.Format("{0," + consoleWidth + "}", "adding: " + fName);
                Console.WriteLine(strToShow);
                //Console.SetCursorPosition(0, Console.CursorTop - 1);

                lStatsCalculation.Add(new ImageStatsCollectingData()
                {
                    filename = fName,
                    State = ImageStatsCollectingState.Queued
                });

                idx++;
            }


            Console.WriteLine("finished enumerating files. Files to process: " + lStatsCalculation.Count);
        }





        private List<MissionsObservedData> lMissionObservedData = new List<MissionsObservedData>();
        private void ReadObservationsReportCSVfile()
        {
            if (File.Exists(strObservationsReportCSVfile))
            {
                List<List<string>> ObservationsReportCSVfileContents = ServiceTools.ReadDataFromCSV(strObservationsReportCSVfile, 1, true,
                    ";", "\n");
                lMissionObservedData =
                    ObservationsReportCSVfileContents.ConvertAll(strList => new MissionsObservedData(strList));
            }
        }





        private List<Tuple<string, DateTimeSpan>> NVdataFilesAlreadyReadDateTimeSpans =
            new List<Tuple<string, DateTimeSpan>>();
        private List<Tuple<string, List<IoffeVesselDualNavDataConverted>>> NVdataFilesAlreadyReadData =
            new List<Tuple<string, List<IoffeVesselDualNavDataConverted>>>();

        private void ProcessImage(ImageStatsCollectingData srcData)
        {
            Interlocked.Increment(ref totalFilesProcessed);
            int perc = Convert.ToInt32(100.0d * (double)totalFilesProcessed / (double)totalFilesCountToProcess);
            Console.WriteLine(DateTime.Now.ToString("s") + " : " + perc + "% : started processing file " + Environment.NewLine + srcData.filename);

            Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
            optionalParameters.Add("logFileName", errorLogFilename);


            // найти и записать данные GPS
            GPSdata currimageGPS = ServiceTools.FindProperGPSdataForImage(srcData.filename, null, defaultProperties,
                ref NVdataFilesAlreadyReadDateTimeSpans, ref NVdataFilesAlreadyReadData);
            if (currimageGPS != null)
            {
                ServiceTools.WriteObjectToXML(currimageGPS,
                    ConventionalTransitions.ConcurrentGPSdataFileName(srcData.filename, strConcurrentGPSdataXMLfilesPath));
            }


            // найти и записать данные SDC и Cloud Cover
            DateTime curDateTime = GetImageDateTime(srcData.filename);
            if (!lMissionObservedData.Any())
            {
                return;
            }
            lMissionObservedData.Sort((obsRecord1, obsRecord2) =>
            {
                double dev1 = Math.Abs((obsRecord1.dateTime - curDateTime).TotalMilliseconds);
                double dev2 = Math.Abs((obsRecord2.dateTime - curDateTime).TotalMilliseconds);
                return (dev1 >= dev2) ? (1) : (-1);
            });
            MissionsObservedData closestObservedDatum = lMissionObservedData[0];
            if ((closestObservedDatum.dateTime - curDateTime).TotalSeconds > 600)
            {
                return;
            }



            SunDiskConditionData currImageSDC = new SunDiskConditionData()
            {
                filename = srcData.filename,
                sunDiskCondition = closestObservedDatum.SDC
            };
            ServiceTools.WriteObjectToXML(currImageSDC,
                ConventionalTransitions.SunDiskConditionFileName(srcData.filename, SunDiskConditionXMLdataFilesDirectory));





            // find grixyrgbStatsXMLfile
            SkyImageIndexesStatsData currImageStatsData = null;
            string currImageStatsDataXMLfile = "";
            if (Directory.Exists(imageYRGBstatsXMLdataFilesDirectory))
            {
                List<string> foundXMLfiles = Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory,
                    ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(srcData.filename, "", false),
                    SearchOption.AllDirectories).ToList();
                if (foundXMLfiles.Any())
                {
                    // возьмем первый попавшийся
                    currImageStatsDataXMLfile = foundXMLfiles[0];
                    currImageStatsData =
                        (SkyImageIndexesStatsData)
                            ServiceTools.ReadObjectFromXML(currImageStatsDataXMLfile, typeof(SkyImageIndexesStatsData));
                }
            }



            SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC currImageData = new SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC
                ()
            {
                skyImageFullFileName = srcData.filename,
                skyImageFileName = Path.GetFileName(srcData.filename),
                currImageDateTime = curDateTime,
                observedCloudCoverData = new ObservedClCoverData()
                {
                    dt = closestObservedDatum.dateTime,
                    CloudCoverTotal = closestObservedDatum.CloudCoverTotal,
                    CloudCoverLower = closestObservedDatum.CloudCoverLower
                },
                concurrentDataXMLfile = "",
                concurrentData = new ConcurrentData()
                {
                    filename = "",
                    datetimeUTC = currimageGPS.DateTimeUTC,
                    GPSdata = "",
                    GPSLat = currimageGPS.Lat,
                    GPSLon = currimageGPS.Lon,
                    GPSDateTimeUTC = currimageGPS.DateTimeUTC,
                    PressurePa = closestObservedDatum.pressure,
                    gps = currimageGPS
                },
                grixyrgbStatsXMLfile = currImageStatsDataXMLfile,
                grixyrgbStats = currImageStatsData,
                SDCvalue = closestObservedDatum.SDC
            };

            ServiceTools.WriteObjectToXML(currImageData,
                ConventionalTransitions.SkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName(srcData.filename,
                    strSkyImagesDataWithConcurrentStatsCloudCoverAndSDCDirectory));
        }






        private DateTime GetImageDateTime(string imgFileName)
        {
            Image anImage = Image.FromFile(imgFileName);
            ImageInfo newIInfo = new ImageInfo(anImage);

            string dateTime = (String)newIInfo.getValueByKey("ExifDTOrig");
            if (dateTime == null)
            {
                //попробуем вытащить из имени файла
                string strDateTime = Path.GetFileName(imgFileName);
                strDateTime = strDateTime.Substring(4, 19);
                dateTime = strDateTime;
            }

            DateTime curDateTime = DateTime.UtcNow;

            try
            {
                curDateTime = CommonTools.DateTimeOfString(dateTime);
                Console.WriteLine("picture got date/time: " + curDateTime.ToString("s"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("couldn`t get picture date/time for file: " + Environment.NewLine + imgFileName);
                
                return curDateTime;
            }
            curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);

            return curDateTime;
        }







        #region helper classes

        private enum ImageStatsCollectingState
        {
            Queued,
            Calculating,
            Finished,
            Error,
            Aborted
        }



        private class ImageStatsCollectingData
        {
            public string filename
            { get; set; }

            public ImageStatsCollectingState State
            { get; set; }

            public SkyImageIndexesStatsData statsData
            { get; set; }
            
            public SunDiskConditions SDC
            { get; set; }

            public ConcurrentData concurrentData
            { get; set; }
        }

        #endregion helper classes
    }
}
