using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using SkyImagesAnalyzerLibraries;

namespace ImagesStatsCalculatingApp
{
    public class StatsCalculator
    {
        private string currImageFileName = "";
        private FileInfo currImageFInfo = null;
        private string currPath2Process = "";
        private string SunDiskConditionXMLdataFilesDirectory = "";
        private string imageYRGBstatsXMLdataFilesDirectory = "";
        private string imageMP5statsXMLdataFilesDirectory = "";
        private Image<Bgr, byte> currImage = null;
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
        private string ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar +
                                          "ImagesRoundMasksXMLfilesMappingList.csv";
        private string strPerformanceCountersStatsFile = "";

        private string strSourceDirectory = "";
        private string strOutputDirectory = "";
        private string strServiceSourceDirectory = "";
        private string strMLdataSourceDirectory = "";
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
                    CalculateAllVarsStats();
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
                CalculateAllVarsStats();
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


            //imageMP5statsXMLdataFilesDirectory
            if (defaultProperties.ContainsKey("imageMP5statsXMLdataFilesDirectory"))
            {
                imageMP5statsXMLdataFilesDirectory = (string)defaultProperties["imageMP5statsXMLdataFilesDirectory"];
            }
            else
            {
                imageMP5statsXMLdataFilesDirectory = CurDir;
                defaultProperties.Add("imageMP5statsXMLdataFilesDirectory", imageMP5statsXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }


            // maxConcurrentImagesProcessing
            if (defaultProperties.ContainsKey("maxConcurrentImagesProcessing"))
            {
                maxConcurrentImagesProcessing = Convert.ToInt32(defaultProperties["maxConcurrentImagesProcessing"]);
            }
            else
            {
                defaultProperties.Add("maxConcurrentImagesProcessing", 4);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }


            if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
            {
                ImagesRoundMasksXMLfilesMappingList = (string)defaultProperties["ImagesRoundMasksXMLfilesMappingList"];
            }
            else
            {
                ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                                      "settings" +
                                                      Path.DirectorySeparatorChar +
                                                      "ImagesRoundMasksXMLfilesMappingList.csv";
                defaultProperties.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                bDefaultPropertiesHasBeenUpdated = true;
            }



            if (defaultProperties.ContainsKey("strPerformanceCountersStatsFile"))
            {
                strPerformanceCountersStatsFile = (string)defaultProperties["strPerformanceCountersStatsFile"];
            }
            else
            {
                strPerformanceCountersStatsFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-perf-data.csv";
                defaultProperties.Add("strPerformanceCountersStatsFile", strPerformanceCountersStatsFile);
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




        private void CalculateAllVarsStats()
        {
            Console.WriteLine("getting files list");
            EnumerateFilesToProcess();

            totalFilesCountToProcess = lStatsCalculation.Count;


#if DEBUG
            foreach (ImageStatsCollectingData dt in lStatsCalculation)
            {
                ProcessImage(dt);
            }
#else
            cts = new CancellationTokenSource();
            Console.CancelKeyPress += new ConsoleCancelEventHandler((sender, evArgs) =>
            {
                Console.WriteLine(
                    "Processing abort requested by user. Please wait while all currently launched calculations will be finished.");
                cts.Cancel();
            });
            
            ParallelOptions parOpts = new ParallelOptions()
            {
                MaxDegreeOfParallelism = maxConcurrentImagesProcessing,
                CancellationToken = cts.Token
            };



            try
            {
                Parallel.ForEach<ImageStatsCollectingData>(lStatsCalculation, parOpts, (srcData) =>
                {
                    ProcessImage(srcData);
                    parOpts.CancellationToken.ThrowIfCancellationRequested();
                });
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cts.Dispose();
                Console.WriteLine("===FINISHED===");
                Console.WriteLine("press any key...");
                Console.ReadKey();
            }
#endif
        }



        private void EnumerateFilesToProcess()
        {
            //FilesToProcessMask = currPath2Process +
            //                     ((currPath2Process.Last() == Path.DirectorySeparatorChar)
            //                         ? ("")
            //                         : (Path.DirectorySeparatorChar.ToString()))
            //                    + "*.jpg";

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


            // похоже накосячил - в условии должно быть отрицание?..
            int removed = filesList.RemoveAll(
                fname =>
                    statsFilesList.Contains(ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(fname, "", false)));

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

//#if DEBUG
//                if (idx >= 10)
//                {
//                    Console.WriteLine("\n");
//                    break;
//                }
//#endif
            }


            Console.WriteLine("finished enumerating files. Files to process: " + lStatsCalculation.Count);
        }





        private void ProcessImage(ImageStatsCollectingData srcData)
        {
            Interlocked.Increment(ref totalFilesProcessed);
            int perc = Convert.ToInt32(100.0d * (double)totalFilesProcessed / (double) totalFilesCountToProcess);
            Console.WriteLine(DateTime.Now.ToString("s") + " : " + perc + "% : started processing file " + Environment.NewLine + srcData.filename);

            Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
            optionalParameters.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            optionalParameters.Add("Stopwatch", sw);
            optionalParameters.Add("logFileName", errorLogFilename);

            ImageStatsDataCalculationResult currImageProcessingResult =
                ImageProcessing.CalculateImageStatsData(srcData.filename, optionalParameters);

            currImageProcessingResult.stopwatch.Stop();
            if (currImageProcessingResult.calcResult)
            {
                string currentFullFileName = currImageProcessingResult.imgFilename;
                string strPerfCountersData = currentFullFileName + ";" +
                                             currImageProcessingResult.stopwatch.ElapsedMilliseconds + ";" +
                                             (currImageProcessingResult.procTotalProcessorTimeEnd -
                                              currImageProcessingResult.procTotalProcessorTimeStart).TotalMilliseconds +
                                             Environment.NewLine;
                ServiceTools.logToTextFile(strPerformanceCountersStatsFile, strPerfCountersData, true);





                string strImageGrIxMedianP5DataFileName =
                    ConventionalTransitions.ImageGrIxMedianP5DataFileName(currentFullFileName, imageMP5statsXMLdataFilesDirectory);
                ServiceTools.WriteObjectToXML(currImageProcessingResult.mp5Result, strImageGrIxMedianP5DataFileName);
                string strImageGrIxYRGBDataFileName =
                    ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currentFullFileName, imageYRGBstatsXMLdataFilesDirectory);
                ServiceTools.WriteObjectToXML(currImageProcessingResult.grixyrgbStatsData, strImageGrIxYRGBDataFileName);


                ImageStatsCollectingData foundDataObj =
                    lStatsCalculation.Find(obj => obj.filename == currentFullFileName);
                foundDataObj.State = ImageStatsCollectingState.Finished;
                foundDataObj.GrIxMedianValue = currImageProcessingResult.mp5Result.GrIxStatsMedian;
                foundDataObj.GrIxPerc5Value = currImageProcessingResult.mp5Result.GrIxStatsPerc5;

                
                Console.WriteLine(DateTime.Now.ToString("s") + " : finished processing file " + Environment.NewLine + currentFullFileName);
            }
            else
            {
                string currentFullFileName = currImageProcessingResult.imgFilename;
                ImageStatsCollectingData foundDataObj =
                    lStatsCalculation.Find(obj => obj.filename == currentFullFileName);
                foundDataObj.State = ImageStatsCollectingState.Error;
                Console.WriteLine("ERROR processing file " + Path.GetFileName(currentFullFileName));
                try
                {
                    //report full error to error log file
                    #region report error
#if (DEBUG && MONO)
                    ServiceTools.logToTextFile(errorLogFilename,
                            "Error processing file: " + Environment.NewLine + currentFullFileName +
                            Environment.NewLine + "messages: " +
                            ServiceTools.GetExceptionMessages(currImageProcessingResult.exception) + Environment.NewLine +
                            "Stack trace: " + Environment.NewLine +
                            Environment.StackTrace + Environment.NewLine + Environment.NewLine, true, true);
#else
#if MONO
                    ServiceTools.logToTextFile(errorLogFilename,
                            "Error processing file: " + Environment.NewLine + currentFullFileName +
                            Environment.NewLine + "messages: " +
                            ServiceTools.GetExceptionMessages(currImageProcessingResult.exception) + Environment.NewLine +
                            "Stack trace: " + Environment.NewLine +
                            Environment.StackTrace + Environment.NewLine + Environment.NewLine, true, true);
#else
                    ServiceTools.logToTextFile(errorLogFilename,
                        "Error processing file: " + Environment.NewLine + currentFullFileName +
                        Environment.NewLine + "message: " +
                        ServiceTools.GetExceptionMessages(currImageProcessingResult.exception) + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription() + Environment.NewLine + "Stack trace: " + Environment.NewLine +
                        Environment.StackTrace + Environment.NewLine + Environment.NewLine, true, true);
#endif
#endif
                    #endregion report error

                }
                catch (Exception ex)
                {
                    return;
                }
            }
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
            {
                get; set;
            }
            public ImageStatsCollectingState State
            {
                get; set;
            }
            public double GrIxMedianValue
            {
                get; set;
            }
            public double GrIxPerc5Value
            {
                get; set;
            }
        }

#endregion helper classes
    }
}
