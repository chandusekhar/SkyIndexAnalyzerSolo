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
using System.Management.Automation;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ConvertSkyImagesToNetCDFfiles
{
    public class Converter
    {
        private string currImageFileName = "";
        private FileInfo currImageFInfo = null;
        private string ImagesBasePath = "";
        private string imageYRGBstatsXMLdataFilesDirectory = "";
        private string outputNetCDFfilesDirectory = "";

        private Image<Bgr, byte> currImage = null;
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";

        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";

        private string ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar +
                                          "ImagesRoundMasksXMLfilesMappingList.csv";

        private string strPerformanceCountersStatsFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-perf-data.csv";

        private bool bSearchImagesTopDirectoryOnly = true;
        private bool bStartWithoutConfirmation = false;
        private List<ImageStatsCollectingData> lStatsCalculation = new List<ImageStatsCollectingData>();
        private int totalFilesCountToProcess = 0;
        private int totalFilesProcessed = 0;
        private bool NeedToStopFlag = false;



        public void Start(string[] args)
        {
            readDefaultProperties();

            List<string> argsList = new List<string>(args);


            if (argsList.Count(str => str == "--help") > 0)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "ConvertSkyImagesToNetCDFfiles.Resources.helpfile.txt";

                Stream stream = assembly.GetManifestResourceStream(resourceName);
                StreamReader reader = new StreamReader(stream);
                string helptext = reader.ReadToEnd();

                Console.WriteLine(helptext);
                Console.WriteLine("Press any key...");
                Console.ReadKey();
                return;
            }


            #region process command line args

            #region --recursive

            if (argsList.Find(str => str == "--recursive") != null)
            {
                bSearchImagesTopDirectoryOnly = false;
                if (defaultProperties.ContainsKey("SearchImagesTopDirectoryOnly"))
                {
                    defaultProperties["defaultProperties"] = bSearchImagesTopDirectoryOnly;
                }
                else
                {
                    defaultProperties.Add("SearchImagesTopDirectoryOnly", bSearchImagesTopDirectoryOnly);
                }
            }

            #endregion --recursive


            #region -y

            if (argsList.Find(str => str == "-y") != null)
            {
                bStartWithoutConfirmation = true;
                defaultProperties.Add("StartWithoutConfirmation", bStartWithoutConfirmation);
            }

            #endregion -y



            #region --ImagesBasePath=

            if (argsList.Count(str => str.Contains("--ImagesBasePath=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--ImagesBasePath="));


                ImagesBasePath = arg.Replace("--ImagesBasePath=", "");

                if (defaultProperties.ContainsKey("ImagesBasePath"))
                {
                    defaultProperties["ImagesBasePath"] = ImagesBasePath;
                }
                else
                {
                    defaultProperties.Add("ImagesBasePath", ImagesBasePath);
                }
            }

            #endregion --ImagesBasePath=





            #region --imageYRGBstatsXMLdataFilesDirectory=

            if (argsList.Count(str => str.Contains("--imageYRGBstatsXMLdataFilesDirectory=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--imageYRGBstatsXMLdataFilesDirectory="));


                imageYRGBstatsXMLdataFilesDirectory = arg.Replace("--imageYRGBstatsXMLdataFilesDirectory=", "");

                if (defaultProperties.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
                {
                    defaultProperties["imageYRGBstatsXMLdataFilesDirectory"] = imageYRGBstatsXMLdataFilesDirectory;
                }
                else
                {
                    defaultProperties.Add("imageYRGBstatsXMLdataFilesDirectory", imageYRGBstatsXMLdataFilesDirectory);
                }
            }

            #endregion --imageYRGBstatsXMLdataFilesDirectory=





            #region --outputNetCDFfilesDirectory=

            if (argsList.Count(str => str.Contains("--outputNetCDFfilesDirectory=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--outputNetCDFfilesDirectory="));


                outputNetCDFfilesDirectory = arg.Replace("--outputNetCDFfilesDirectory=", "");

                if (defaultProperties.ContainsKey("outputNetCDFfilesDirectory"))
                {
                    defaultProperties["outputNetCDFfilesDirectory"] = outputNetCDFfilesDirectory;
                }
                else
                {
                    defaultProperties.Add("outputNetCDFfilesDirectory", outputNetCDFfilesDirectory);
                }
            }

            #endregion --outputNetCDFfilesDirectory=






            #region --ImagesRoundMasksXMLfilesMappingList=

            if (argsList.Count(str => str.Contains("--ImagesRoundMasksXMLfilesMappingList=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--ImagesRoundMasksXMLfilesMappingList="));


                ImagesRoundMasksXMLfilesMappingList = arg.Replace("--ImagesRoundMasksXMLfilesMappingList=", "");

                if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
                {
                    defaultProperties["ImagesRoundMasksXMLfilesMappingList"] = ImagesRoundMasksXMLfilesMappingList;
                }
                else
                {
                    defaultProperties.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                }
            }

            #endregion --ImagesRoundMasksXMLfilesMappingList=






            #region --ImagesRoundMasksXMLfilesMappingList=

            if (argsList.Count(str => str.Contains("--ImagesRoundMasksXMLfilesMappingList=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--ImagesRoundMasksXMLfilesMappingList="));


                ImagesRoundMasksXMLfilesMappingList = arg.Replace("--ImagesRoundMasksXMLfilesMappingList=", "");

                if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
                {
                    defaultProperties["ImagesRoundMasksXMLfilesMappingList"] = ImagesRoundMasksXMLfilesMappingList;
                }
                else
                {
                    defaultProperties.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                }
            }

            #endregion --ImagesRoundMasksXMLfilesMappingList=





            #region --strPerformanceCountersStatsFile=

            if (argsList.Count(str => str.Contains("--strPerformanceCountersStatsFile=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--strPerformanceCountersStatsFile="));


                strPerformanceCountersStatsFile = arg.Replace("--strPerformanceCountersStatsFile=", "");

                if (defaultProperties.ContainsKey("strPerformanceCountersStatsFile"))
                {
                    defaultProperties["strPerformanceCountersStatsFile"] = strPerformanceCountersStatsFile;
                }
                else
                {
                    defaultProperties.Add("strPerformanceCountersStatsFile", strPerformanceCountersStatsFile);
                }
            }

            #endregion --strPerformanceCountersStatsFile=

            #endregion process command line args



            CommonTools.PrintDictionaryToConsole(defaultProperties, "effective properties list");
            Console.WriteLine("");

            if (!bStartWithoutConfirmation)
            {
                Console.Write("Start with the mentioned properties? [y/n] ");
                string strReply = Console.ReadLine();
                if (strReply.ToLower().First() == 'y')
                {
                    ConvertImagesToNetCDFFiles();
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
                ConvertImagesToNetCDFFiles();
            }

        }




        public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            NeedToStopFlag = true;
            Console.WriteLine("Please wait for the last conversion to complete.");
        }




        #region default properties mechanics

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
            ImagesBasePath = "";



            #region ImagesBasePath

            if (defaultProperties.ContainsKey("ImagesBasePath"))
            {
                ImagesBasePath = (string)defaultProperties["ImagesBasePath"];
            }
            else
            {
                ImagesBasePath = CurDir;
                defaultProperties.Add("ImagesBasePath", ImagesBasePath);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion ImagesBasePath




            #region SearchImagesTopDirectoryOnly

            if (defaultProperties.ContainsKey("SearchImagesTopDirectoryOnly"))
            {
                bSearchImagesTopDirectoryOnly = (((string)defaultProperties["SearchImagesTopDirectoryOnly"]).ToLower() == "true");
            }
            else
            {
                bSearchImagesTopDirectoryOnly = false;
                defaultProperties.Add("SearchImagesTopDirectoryOnly", bSearchImagesTopDirectoryOnly);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region imageYRGBstatsXMLdataFilesDirectory

            // imageYRGBstatsXMLdataFilesDirectory
            if (defaultProperties.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
            {
                imageYRGBstatsXMLdataFilesDirectory = (string)defaultProperties["imageYRGBstatsXMLdataFilesDirectory"];
            }
            else
            {
                imageYRGBstatsXMLdataFilesDirectory = CurDir;
                defaultProperties.Add("imageYRGBstatsXMLdataFilesDirectory", imageYRGBstatsXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion imageYRGBstatsXMLdataFilesDirectory




            #region outputNetCDFfilesDirectory

            // outputNetCDFfilesDirectory
            if (defaultProperties.ContainsKey("outputNetCDFfilesDirectory"))
            {
                outputNetCDFfilesDirectory = (string)defaultProperties["outputNetCDFfilesDirectory"];
            }
            else
            {
                outputNetCDFfilesDirectory = CurDir;
                defaultProperties.Add("outputNetCDFfilesDirectory", outputNetCDFfilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion outputNetCDFfilesDirectory



            #region ImagesRoundMasksXMLfilesMappingList

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

            #endregion ImagesRoundMasksXMLfilesMappingList



            #region strPerformanceCountersStatsFile

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

            #endregion strPerformanceCountersStatsFile



            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }



        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }

        #endregion default properties mechanics



        #region conversion mechanics itself

        private void ConvertImagesToNetCDFFiles()
        {
            Console.WriteLine("getting files list");
            EnumerateFilesToProcess();

            totalFilesCountToProcess = lStatsCalculation.Count;
            int prevPerc = 0;
            int totalFilesToConvert = lStatsCalculation.Count;

            foreach (ImageStatsCollectingData dt in lStatsCalculation)
            {
                ConvertImage(dt);


                if (NeedToStopFlag)
                {
                    Console.WriteLine("User keyboard interruption.");
                    Console.WriteLine("Press any key...");
                    Console.ReadKey();

                    break;
                }

                #region // calculate and report progress percentage

                //int currIdx = lStatsCalculation.IndexOf(dt) + 1;
                //int currPerc = Convert.ToInt32(((double) currIdx/(double) totalFilesToConvert)*100.0d);
                //if (currPerc > prevPerc)
                //{
                //    prevPerc = currPerc;
                //    Console.WriteLine("Convert progress: {0}%", currPerc);
                //}

                #endregion // calculate and report progress percentage
            }

            Console.WriteLine("===FINISHED===");
        }



        private void EnumerateFilesToProcess()
        {
            string directory = Path.GetDirectoryName(ImagesBasePath);
            string filemask = "*.jpg";
            List<string> filesList =
                new List<string>(Directory.EnumerateFiles(directory, filemask,
                    bSearchImagesTopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));

            Console.WriteLine("found " + filesList.Count + " files.");


            #region filtering

            Console.WriteLine("filtering (if an image stats has been already calculated - it should be excluded) ...");

            List<string> statsFilesList =
                new List<string>(Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory, ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern(),
                    bSearchImagesTopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));
            statsFilesList = statsFilesList.ConvertAll(strFileName => Path.GetFileName(strFileName));


            int removed = filesList.RemoveAll(
                fname =>
                    statsFilesList.Contains(ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(fname, "", false)));

            Console.WriteLine("removed " + removed + " items. Remains " + filesList.Count + " to process.");
            Console.WriteLine("press any key...");
            Console.ReadKey();

            #endregion filtering


            if (!filesList.Any())
            {
                Console.WriteLine("There is no " + filemask + " files that sutisfy settings specified. Processing will not be started.");
                return;
            }

#region compiling files processing list lStatsCalculation

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


#if DEBUG
                //if (idx >= 10)
                //{
                //    Console.WriteLine("Finished enumerating files due to DEBUG configuration limitations\n");
                //    Console.WriteLine("press any key...");
                //    Console.ReadKey();
                //    break;
                //}
#endif
            }

#endregion compiling files processing list

            Console.WriteLine("finished enumerating files. Files to process: " + lStatsCalculation.Count);
            Console.WriteLine("press any key...");
            Console.ReadKey();
        }



        private async void ConvertImage(ImageStatsCollectingData srcData)
        {
            Interlocked.Increment(ref totalFilesProcessed);
            int perc = Convert.ToInt32(100.0d * (double)totalFilesProcessed / (double)totalFilesCountToProcess);
            Console.WriteLine(DateTime.Now.ToString("s") + " : " + perc + "% : started processing file " + Environment.NewLine + srcData.filename);

            Dictionary<string, object> taskParameters = new Dictionary<string, object>();
            foreach (string key in defaultProperties.Keys)
            {
                taskParameters.Add(key, defaultProperties[key]);
            }
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            taskParameters.Add("Stopwatch", sw);
            taskParameters.Add("logFileName", errorLogFilename);
            taskParameters.Add("currentFullFileName", srcData.filename);


            ImageStatsDataCalculationResult currImageConversionResult = await ConvertImageTask(taskParameters);


#region process result

            currImageConversionResult.stopwatch.Stop();

            if (currImageConversionResult.calcResult)
            {
                string currentFullFileName = currImageConversionResult.imgFilename;
                string strPerfCountersData = currentFullFileName + ";" +
                                             currImageConversionResult.stopwatch.ElapsedMilliseconds + ";" +
                                             (currImageConversionResult.procTotalProcessorTimeEnd -
                                              currImageConversionResult.procTotalProcessorTimeStart).TotalMilliseconds +
                                             Environment.NewLine;
                ServiceTools.logToTextFile(strPerformanceCountersStatsFile, strPerfCountersData, true);
                

                ImageStatsCollectingData foundDataObj =
                    lStatsCalculation.Find(obj => obj.filename == currentFullFileName);
                foundDataObj.State = ImageStatsCollectingState.Finished;


                Console.WriteLine(DateTime.Now.ToString("s") + " : finished processing file " + Environment.NewLine + currentFullFileName);
            }
            else
            {
                string currentFullFileName = currImageConversionResult.imgFilename;
                ImageStatsCollectingData foundDataObj =
                    lStatsCalculation.Find(obj => obj.filename == currentFullFileName);
                foundDataObj.State = ImageStatsCollectingState.Error;
                Console.WriteLine("ERROR processing file " + Path.GetFileName(currentFullFileName));
                try
                {
#region report error

                    ServiceTools.logToTextFile(errorLogFilename,
                        "Error processing file: " + Environment.NewLine + currentFullFileName +
                        Environment.NewLine + "message: " +
                        ServiceTools.GetExceptionMessages(currImageConversionResult.exception) + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription() + Environment.NewLine + "Stack trace: " + Environment.NewLine +
                        Environment.StackTrace + Environment.NewLine + Environment.NewLine, true, true);

#endregion report error

                }
                catch (Exception ex)
                {
                    return;
                }
            }

#endregion process result
        }




        static async Task<ImageStatsDataCalculationResult> ConvertImageTask(object inputArgs)
        {
            ImageStatsDataCalculationResult Result = new ImageStatsDataCalculationResult();
            Dictionary<string, object> ParametersPassed = inputArgs as Dictionary<string, object>;
            string logFileName = "";
            string currentFullFileName = ParametersPassed["currentFullFileName"] as string;
            Stopwatch stopwatch = null;
            List<Tuple<string, string>> lImagesRoundMasksMappingFiles = null;


            if (ParametersPassed != null)
            {
                if (ParametersPassed.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
                {
                    string ImagesRoundMasksXMLfilesMappingListPassed = (string)ParametersPassed["ImagesRoundMasksXMLfilesMappingList"];
                    if (File.Exists(ImagesRoundMasksXMLfilesMappingListPassed))
                    {
                        List<List<string>> llImagesRoundMasksMappingFiles =
                            ServiceTools.ReadDataFromCSV(ImagesRoundMasksXMLfilesMappingListPassed, 0, true, ";", Environment.NewLine);
                        lImagesRoundMasksMappingFiles =
                            llImagesRoundMasksMappingFiles.ConvertAll(
                                list => new Tuple<string, string>(list[0], list[1]));
                    }
                }


                if (ParametersPassed.ContainsKey("Stopwatch"))
                {
                    stopwatch = ParametersPassed["Stopwatch"] as Stopwatch;
                }

                if (ParametersPassed.ContainsKey("logFileName"))
                {
                    logFileName = ParametersPassed["logFileName"] as string;
                }
            }

            TimeSpan procStart = Process.GetCurrentProcess().TotalProcessorTime;


            try
            {
                RoundData predefinedRoundedMask = null;
                if (lImagesRoundMasksMappingFiles != null)
                {
                    if (lImagesRoundMasksMappingFiles.Any())
                    {
                        if (lImagesRoundMasksMappingFiles.Find(tpl => (new WildcardPattern(tpl.Item1)).IsMatch(currentFullFileName)) != null)
                        {
                            string strFoundPredefinedRoundedMaskParametersXMLfile =
                                lImagesRoundMasksMappingFiles.Find(
                                    tpl => (new WildcardPattern(tpl.Item1)).IsMatch(currentFullFileName)).Item2;
                            strFoundPredefinedRoundedMaskParametersXMLfile =
                                strFoundPredefinedRoundedMaskParametersXMLfile.Substring(0, strFoundPredefinedRoundedMaskParametersXMLfile.IndexOf(".xml") + 4);

                            predefinedRoundedMask =
                                ServiceTools.ReadObjectFromXML(strFoundPredefinedRoundedMaskParametersXMLfile,
                                    typeof(RoundData)) as RoundData;
                        }
                    }
                }

                Image<Bgr, byte> currImg = new Image<Bgr, byte>(currentFullFileName);
                ImageProcessing imgP = new ImageProcessing(currImg, predefinedRoundedMask);


                
                Image<Bgr, byte> maskImage =
                    new Image<Bgr, byte>(new Image<Gray, byte>[]
                    {
                        imgP.significantMaskImageCircled,
                        imgP.significantMaskImageCircled,
                        imgP.significantMaskImageCircled
                    });
                Image<Bgr, byte> img = imgP.tmpImage.Mul(maskImage);

                string ncFileName = ConventionalTransitions.NetCDFimageBareChannelsDataFilename(currentFullFileName,
                    ParametersPassed["outputNetCDFfilesDirectory"] as string, true, ParametersPassed["ImagesBasePath"] as string);
                
                Dictionary<string, object> dataToNCwrite = new Dictionary<string, object>();
                dataToNCwrite.Add("ColorChannels", img.Data);
                
                NetCDFoperations.SaveVariousDataToFile(dataToNCwrite, ncFileName);

                TimeSpan procEnd = Process.GetCurrentProcess().TotalProcessorTime;

                Result = new ImageStatsDataCalculationResult()
                {
                    calcResult = true,
                    imgFilename = currentFullFileName,
                    mp5Result = null,
                    grixyrgbStatsData = null,
                    stopwatch = stopwatch,
                    exception = null,
                    procTotalProcessorTimeEnd = procEnd,
                    procTotalProcessorTimeStart = procStart
                };

                return Result;
            }
            catch (Exception ex)
            {
                TimeSpan procEnd = Process.GetCurrentProcess().TotalProcessorTime;
                Result = new ImageStatsDataCalculationResult()
                {
                    calcResult = false,
                    imgFilename = currentFullFileName,
                    stopwatch = stopwatch,
                    exception = ex,
                    procTotalProcessorTimeEnd = procEnd,
                    procTotalProcessorTimeStart = procStart
                };
                return Result;
            }
        }



        #endregion conversion mechanics itself




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
        }

#endregion helper classes
    }
}
