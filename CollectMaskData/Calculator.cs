using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Geometry;
using SkyImagesAnalyzerLibraries;

namespace CollectMaskDataApp
{
    public class Calculator
    {
        private string currPath2Process = "";
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private int maxConcurrentBackgroundWorkers = 4;
        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";
        string outputDataFile = "imageRD-stats.csv";

        private string strOutputDirectory = "";
        private string FilesToProcessMask = "";
        private bool cbSearchImagesTopDirectoryOnly = true;

        private List<ImageStatsCollectingData> lStatsCalculation = new List<ImageStatsCollectingData>();



        public void Start(string[] args)
        {
            readDefaultProperties();

            List<string> argsList = new List<string>(args);

            if (argsList.Find(str => str.Substring(0, 7) == "--mask=") != null)
            {
                string foundArg = argsList.Find(str => str.Substring(0, 7) == "--mask=");
                FilesToProcessMask = foundArg.Substring(7);
                Console.WriteLine("files mask to process: \"" + FilesToProcessMask + "\"");
            }

            if (argsList.Find(str => str == "--recursive") != null)
            {
                cbSearchImagesTopDirectoryOnly = false;
            }


            outputDataFile = strOutputDirectory + outputDataFile;
            ServiceTools.logToTextFile(outputDataFile,
                "processing images using filename mask: " + FilesToProcessMask + Environment.NewLine, true);


            Console.WriteLine("getting files list");


            EnumerateFilesToProcess();


            ServiceTools.logToTextFile(outputDataFile,
                "filename;centerX;centerY;radius" + Environment.NewLine, true);

            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = maxConcurrentBackgroundWorkers;

            Parallel.ForEach(lStatsCalculation, options, item => { CurrImageProcessing(item); });

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
            

            // SaveControlImagesToPath
            if (!defaultProperties.ContainsKey("SaveControlImagesToPath"))
            {
                defaultProperties.Add("SaveControlImagesToPath", CurDir);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            

            if (defaultProperties.ContainsKey("OutputDirectory"))
            {
                strOutputDirectory = ((string)defaultProperties["OutputDirectory"]);
            }
            else
            {
                strOutputDirectory = "";
            }

            

            // maxConcurrentBackgroundWorkers
            if (defaultProperties.ContainsKey("maxConcurrentBackgroundWorkers"))
            {
                maxConcurrentBackgroundWorkers = Convert.ToInt32(defaultProperties["maxConcurrentBackgroundWorkers"]);
            }
            else
            {
                defaultProperties.Add("maxConcurrentBackgroundWorkers", 8);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }


            if (defaultProperties.ContainsKey("FilesToProcessMask"))
            {
                FilesToProcessMask = ((string)defaultProperties["FilesToProcessMask"]);
            }
            else
            {
                FilesToProcessMask = currPath2Process + "*.jpg";
                defaultProperties.Add("FilesToProcessMask", FilesToProcessMask);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
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



        private void EnumerateFilesToProcess()
        {
            string directory = Path.GetDirectoryName(FilesToProcessMask);
            string filemask = FilesToProcessMask.Replace(directory + Path.DirectorySeparatorChar, "");
            List<string> filesList =
                new List<string>(Directory.EnumerateFiles(directory, filemask,
                    cbSearchImagesTopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));



            if (!filesList.Any())
            {
                Console.WriteLine("There is no images that sutisfy specified settings. Processing will not be started.");
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
                
                AddImageToCalculationQueue(fName);
            }


            Console.WriteLine("finished enumerating files. Files to process: " + lStatsCalculation.Count);
            ServiceTools.logToTextFile(outputDataFile,
                "files count to process: " + lStatsCalculation.Count + Environment.NewLine, true);
        }

        

        private void AddImageToCalculationQueue(string imgFilename)
        {
            if (lStatsCalculation.FindIndex(statsDatum => statsDatum.filename == imgFilename) > -1)
            {
                return;
            }

            lStatsCalculation.Add(new ImageStatsCollectingData()
            {
                filename = imgFilename,
                rd = RoundData.nullRoundData(),
                State = ImageGrIxStatsCollectingState.Queued
            });
        }
        


        public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            lStatsCalculation.RemoveAll(
                statsCollectingData => statsCollectingData.State == ImageGrIxStatsCollectingState.Queued);
        }




        private void CurrImageProcessing(ImageStatsCollectingData dat)
        {
            string strSaveControlImagesToPath = "";
            string currentFullFileName = dat.filename;

            if (defaultProperties.ContainsKey("SaveControlImagesToPath"))
            {
                strSaveControlImagesToPath = (string)defaultProperties["SaveControlImagesToPath"];
            }
            else
            {
                strSaveControlImagesToPath = "same as source image";
            }

            try
            {
                Image<Bgr, byte> currImg = new Image<Bgr, byte>(currentFullFileName);
                ImageProcessing imgP = new ImageProcessing(currImg, true);

                #region debugging output control images
#if DEBUG
                if (strSaveControlImagesToPath != "")
                {
                    string strControlImageFileName = "";
                    if (strSaveControlImagesToPath == "same as source image")
                    {
                        strControlImageFileName = Path.GetDirectoryName(currentFullFileName);
                        strControlImageFileName += ((strControlImageFileName.Last() == Path.DirectorySeparatorChar)
                            ? ("")
                            : (Path.DirectorySeparatorChar.ToString()));
                        strControlImageFileName += Path.GetFileNameWithoutExtension(currentFullFileName) +
                                                   "-control.jpg";
                    }
                    else
                    {
                        if (Directory.Exists(strSaveControlImagesToPath))
                        {
                            strControlImageFileName = strSaveControlImagesToPath +
                                                      ((strSaveControlImagesToPath.Last() == Path.DirectorySeparatorChar)
                                                          ? ("")
                                                          : (Path.DirectorySeparatorChar.ToString())) +
                                                      Path.GetFileNameWithoutExtension(currentFullFileName) +
                                                      "-control.jpg";
                        }
                        else
                        {
                            strControlImageFileName = "";
                        }
                    }

                    if (strControlImageFileName != "")
                    {
                        // save control image
                        imgP.significantMaskImageOctLined.Save(strControlImageFileName);
                    }

                }
#endif
                #endregion debugging output control images

                string csvDataStr = Path.GetFileName(currentFullFileName) + ";" + imgP.imageRD.ToCSVstring(";");
                ServiceTools.logToTextFile(outputDataFile, csvDataStr + Environment.NewLine, true);

                ImageStatsCollectingData foundDataObj =
                    lStatsCalculation.Find(obj => obj.filename == currentFullFileName);
                foundDataObj.State = ImageGrIxStatsCollectingState.Finished;
                foundDataObj.rd = imgP.imageRD;

                Console.WriteLine("finished processing file " + Environment.NewLine + currentFullFileName);
                Console.WriteLine("remains: " +
                                  lStatsCalculation.FindAll(
                                      imgStats => imgStats.State == ImageGrIxStatsCollectingState.Queued).Count + " of " +
                                  lStatsCalculation.Count);
            }
            catch (Exception ex)
            {
                return;
            }
        }


        #region support classes
        private class ImageStatsCollectingData
        {
            public string filename
            {
                get; set;
            }
            public ImageGrIxStatsCollectingState State
            {
                get; set;
            }
            public RoundData rd
            {
                get; set;
            }
        }


        private enum ImageGrIxStatsCollectingState
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
