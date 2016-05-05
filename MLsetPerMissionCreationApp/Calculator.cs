using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ANN;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyImagesAnalyzerLibraries;

namespace MLsetPerMissionCreationApp
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Calculator
    {
        private string inputBasePath = "";
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        string outputDataFile = "MissionSkyImagesStats.xml";

        private string strOutputDirectory = "";
        private string FilesToProcessMask = "";
        private bool bEnumerateFilesRecursively = true;

        private string NNconfigFile = "";
        private string NNtrainedParametersFile = "";
        private string NormMeansFile = "";
        private string NormRangeFile = "";

        private string ObservedCloudCoverDataCSVfile = "";
        private bool bFilterByObservedCloudCoverRecords = true;
        private TimeSpan DateTimeFilterTolerance = new TimeSpan(0, 5, 0);

        //private int maxConcurrentProcessingThreads = 8;
        private bool bStartWithoutConfirmation = false;

        private string imageYRGBstatsXMLdataFilesDirectory = "";
        private TimeSpan TimeSpanForConcurrentDataMappingTolerance;
        private string ConcurrentDataXMLfilesDirectory = "";
        private bool bIncludeGPSandSunAltitudeData = true;
        private string ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar +
                                          "ImagesRoundMasksXMLfilesMappingList.csv";

        private int CamIDtoProcess = 1;
        private SunDiskCondition sdcFilter = SunDiskCondition.Sun2;

        private List<ImagesProcessingData> lStatsProcessing = new List<ImagesProcessingData>();

        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";
        private string logFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          ".log";



        #region read and save defaults properties

        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";

            defaultProperties = new Dictionary<string, object>();

            if (File.Exists(defaultPropertiesXMLfileName))
            {
                defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);
            }

            bool bDefaultPropertiesHasBeenUpdated = false;
            string CurDir = Directory.GetCurrentDirectory();


            #region inputBasePath
            if (defaultProperties.ContainsKey("inputBasePath"))
            {
                inputBasePath = (string)defaultProperties["inputBasePath"];
            }
            else
            {
                inputBasePath = CurDir + Path.DirectorySeparatorChar + "input" + Path.DirectorySeparatorChar;
                defaultProperties.Add("inputBasePath", inputBasePath);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion inputBasePath


            #region OutputDirectory
            if (defaultProperties.ContainsKey("OutputDirectory"))
            {
                strOutputDirectory = ((string)defaultProperties["OutputDirectory"]);
            }
            else
            {
                strOutputDirectory = CurDir + Path.DirectorySeparatorChar + "output" + Path.DirectorySeparatorChar;
                defaultProperties.Add("OutputDirectory", strOutputDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion OutputDirectory


            #region NN parameters

            // NNconfigFile
            if (defaultProperties.ContainsKey("NNconfigFile"))
            {
                NNconfigFile = ((string)defaultProperties["NNconfigFile"]);
            }
            else
            {
                NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNconfig.csv";
                defaultProperties.Add("NNconfigFile", NNconfigFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            //NNtrainedParametersFile
            if (defaultProperties.ContainsKey("NNtrainedParametersFile"))
            {
                NNtrainedParametersFile = ((string)defaultProperties["NNtrainedParametersFile"]);
            }
            else
            {
                NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNtrainedParameters.csv";
                defaultProperties.Add("NNtrainedParametersFile", NNtrainedParametersFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }



            // NormMeansFile
            if (defaultProperties.ContainsKey("NormMeansFile"))
            {
                NormMeansFile = ((string)defaultProperties["NormMeansFile"]);
            }
            else
            {
                NormMeansFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormMeans.csv";
                defaultProperties.Add("NormMeansFile", NormMeansFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            // NormRangeFile
            if (defaultProperties.ContainsKey("NormRangeFile"))
            {
                NormRangeFile = ((string)defaultProperties["NormRangeFile"]);
            }
            else
            {
                NormRangeFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormRange.csv";
                defaultProperties.Add("NormRangeFile", NormRangeFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion NN parameters


            #region ObservedCloudCoverDataCSVfile

            if (defaultProperties.ContainsKey("ObservedCloudCoverDataCSVfile"))
            {
                ObservedCloudCoverDataCSVfile = (string)defaultProperties["ObservedCloudCoverDataCSVfile"];
            }
            else
            {
                ObservedCloudCoverDataCSVfile = CurDir + ((CurDir.Last() == Path.DirectorySeparatorChar) ? ("") : (Path.DirectorySeparatorChar.ToString())) + "settings" + Path.DirectorySeparatorChar + "СlCov_observed_data.csv";
                defaultProperties.Add("ObservedCloudCoverDataCSVfile", ObservedCloudCoverDataCSVfile);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion ObservedDataCSVfile



            #region DateTimeFilterTolerance

            if (defaultProperties.ContainsKey("DateTimeFilterTolerance"))
            {
                DateTimeFilterTolerance = TimeSpan.Parse((string)defaultProperties["DateTimeFilterTolerance"]);
            }
            else
            {
                defaultProperties.Add("DateTimeFilterTolerance", DateTimeFilterTolerance.ToString());
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion DateTimeFilterTolerance




            // StartWithoutConfirmation
            if (defaultProperties.ContainsKey("StartWithoutConfirmation"))
            {
                bStartWithoutConfirmation = ((string)defaultProperties["StartWithoutConfirmation"]).ToLower() == "true";
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
            }


            //TimeSpanForConcurrentDataMappingTolerance
            if (defaultProperties.ContainsKey("TimeSpanForConcurrentDataMappingTolerance"))
            {
                TimeSpanForConcurrentDataMappingTolerance =
                    TimeSpan.Parse((string)defaultProperties["TimeSpanForConcurrentDataMappingTolerance"]);
            }
            else
            {
                TimeSpanForConcurrentDataMappingTolerance = new TimeSpan(0, 0, 30);
                defaultProperties.Add("TimeSpanForConcurrentDataMappingTolerance", TimeSpanForConcurrentDataMappingTolerance.ToString());
                bDefaultPropertiesHasBeenUpdated = true;
            }



            //ConcurrentDataXMLfilesDirectory
            if (defaultProperties.ContainsKey("ConcurrentDataXMLfilesDirectory"))
            {
                ConcurrentDataXMLfilesDirectory = (string)defaultProperties["ConcurrentDataXMLfilesDirectory"];
            }
            else
            {
                ConcurrentDataXMLfilesDirectory = CurDir;
                defaultProperties.Add("ConcurrentDataXMLfilesDirectory", ConcurrentDataXMLfilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }



            //IncludeGPSandSunAltitudeData
            if (defaultProperties.ContainsKey("IncludeGPSandSunAltitudeData"))
            {

                bIncludeGPSandSunAltitudeData = (((string)defaultProperties["IncludeGPSandSunAltitudeData"]).ToLower() == "true");
            }
            else
            {
                bIncludeGPSandSunAltitudeData = true;
                defaultProperties.Add("IncludeGPSandSunAltitudeData", bIncludeGPSandSunAltitudeData);
                bDefaultPropertiesHasBeenUpdated = true;
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



            // maxConcurrentProcessingThreads
            //if (defaultProperties.ContainsKey("maxConcurrentProcessingThreads"))
            //{
            //    maxConcurrentProcessingThreads =
            //        Convert.ToInt32((string)defaultProperties["maxConcurrentProcessingThreads"]);
            //}
            //else
            //{
            //    maxConcurrentProcessingThreads = 8;
            //    defaultProperties.Add("maxConcurrentProcessingThreads", maxConcurrentProcessingThreads);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}



            if (CommonTools.console_present())
            {
                CommonTools.PrintDictionaryToConsole(defaultProperties, "Effective settings.");
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


        #endregion read and save defaults properties



        public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            lStatsProcessing.Clear();
        }





        public void Start(string[] args)
        {
            readDefaultProperties();

            List<string> argsList = new List<string>(args);

            if (argsList.Find(str => str == "--recursive") != null)
            {
                bEnumerateFilesRecursively = true;
            }




            if (argsList.Find(str => str == "-y") != null)
            {
                bStartWithoutConfirmation = true;
            }


            // --filter-by-observed-cloud-cover-records
            // bFilterByObservedCloudCoverRecords
            if (argsList.Find(str => str == "--filter-by-observed-cloud-cover-records") != null)
            {
                bFilterByObservedCloudCoverRecords = true;
            }




            // sdcFilter
            if (argsList.Where(str => str.Contains("--sdc=")).Count() > 0)
            {

                string foundArg = argsList.Where(str => str.Contains("--sdc=")).ToList()[0];
                string strValue = foundArg.Replace("--sdc=", "");
                //sdcFilter
                if (strValue == "none")
                {
                    sdcFilter = SunDiskCondition.NoSun;
                }
                else if (strValue == "0")
                {
                    sdcFilter = SunDiskCondition.Sun0;
                }
                else if (strValue == "1")
                {
                    sdcFilter = SunDiskCondition.Sun1;
                }
                else if (strValue == "2")
                {
                    sdcFilter = SunDiskCondition.Sun2;
                }
                else
                {
                    sdcFilter = SunDiskCondition.Sun2;
                }
            }
            else
            {
                Console.WriteLine("SDC filter is not specified. Filtering by SDC will not applied.");
                sdcFilter = SunDiskCondition.Undefined; // Не применять фильтрацию
            }




            if (argsList.Where(str => str.Contains("--camera-id=")).Count() > 0)
            {

                string foundArg = argsList.Where(str => str.Contains("--camera-id=")).ToList()[0];
                string strValue = foundArg.Replace("--camera-id=", "");
                CamIDtoProcess = Convert.ToInt32(strValue);

                if ((CamIDtoProcess != 1) && (CamIDtoProcess != 2))
                {
                    Console.WriteLine("camera ID out of range detected. I will not filter by camera ID.");
                    CamIDtoProcess = 0;
                }

            }
            else
            {
                Console.WriteLine("camera ID out of range detected. I will not filter by camera ID");
                CamIDtoProcess = 0; // will not filter
            }





            if (!bStartWithoutConfirmation)
            {
                Console.Write("Start with the mentioned properties? [y/n] ");
                string strReply = Console.ReadLine();
                if (strReply.ToLower().First() != 'y')
                {
                    Console.WriteLine("\nWill not proceed due to user interruprion.");
                    Console.WriteLine("===FINISHED===");
                    Console.ReadKey();
                    return;
                }
            }



            outputDataFile = strOutputDirectory + Path.GetFileNameWithoutExtension(outputDataFile) + "-" + sdcFilter.ToString() + "-camID" +
                             CamIDtoProcess + ".xml";
            string outputCSVfile = strOutputDirectory + Path.GetFileNameWithoutExtension(outputDataFile) + "-" + sdcFilter.ToString() + "-camID" +
                             CamIDtoProcess + ".csv";


            Console.WriteLine("getting files list");


            #region Enumerating files

            string directory = Path.GetDirectoryName(inputBasePath);
            string filemask = "*.jpg";
            List<string> filesList =
                new List<string>(Directory.EnumerateFiles(directory, filemask,
                    bEnumerateFilesRecursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));



            #region filter by camID
            //...devID1.jpg
            if (CamIDtoProcess > 0)
            {
                string ptrnCamID = "devid" + CamIDtoProcess + ".jpg";
                filesList = filesList.Where(fname => fname.ToLower().Contains(ptrnCamID)).ToList();
            }
            #endregion



            Console.WriteLine("found " + filesList.Count + " images.");



            #region try to find concurrent and stats data already assembled into a small set of files

            List<string> assembledDataFilesList =
                Directory.EnumerateFiles(ConcurrentDataXMLfilesDirectory,
                    "*.xml", SearchOption.TopDirectoryOnly).ToList();

            List<ImagesProcessingData> lReadAssembledData = new List<ImagesProcessingData>();
            foreach (string strAssembledDataXMlfName in assembledDataFilesList)
            {
                try
                {
                    List<ImagesProcessingData> currFileContent =
                        ServiceTools.ReadObjectFromXML(strAssembledDataXMlfName, typeof(List<ImagesProcessingData>)) as
                            List<ImagesProcessingData>;
                    lReadAssembledData.AddRange(currFileContent);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            if (lReadAssembledData.Any())
            {
                Console.WriteLine("Found pre-assembled ImagesProcessingData XML files: ");
                foreach (string s in assembledDataFilesList)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine("Read records from this set: " + lReadAssembledData.Count);
                Console.WriteLine("Images to process originally: " + filesList.Count);
                Console.WriteLine("Should I use these pre-assembled data? (y/n): ");
                string ans = Console.ReadKey().KeyChar.ToString();
                if (ans == "y")
                {
                    lStatsProcessing = lReadAssembledData;
                }
            }

            #endregion try to find data already compiled into a small set of files




            if (!lStatsProcessing.Any())
            {

                #region list, read and map image stats files

                Console.WriteLine("filtering by ready-to-use GrIxYRGB XML files...");
                List<string> statsFilesList =
                    new List<string>(Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory, ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern(),
                        bEnumerateFilesRecursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
                List<string> statsFilesListWOpath = statsFilesList.ConvertAll(Path.GetFileName);


                Console.WriteLine("found " + statsFilesList.Count + " XML stats files in directory " + Environment.NewLine +
                                  imageYRGBstatsXMLdataFilesDirectory + Environment.NewLine + "by mask " +
                                  Environment.NewLine + ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern());

                int removed =
                    filesList.RemoveAll(
                        fname =>
                            !statsFilesListWOpath.Contains(ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(fname, "",
                                false)));

                Console.WriteLine("removed " + removed + " items (couldn`t find stats data files). Remains " + filesList.Count + " to process.");


                if (!filesList.Any())
                {
                    Console.WriteLine("There is no " + filemask + " files that sutisfy settings specified. Processing will not be started.");
                    return;
                }


                lStatsProcessing = filesList.ConvertAll(strImgFname =>
                {
                    ImagesProcessingData retVal = new ImagesProcessingData()
                    {
                        filename = strImgFname
                    };
                    return retVal;
                });

#if DEBUG
                lStatsProcessing = lStatsProcessing.Where((ipd, ind) => ind < 10).ToList();
#endif

                Console.WriteLine("started reading and mapping stats data");

                int totalFilesCountToRead = lStatsProcessing.Count;
                int filesRead = 0;
                int currProgressPerc = 0;

                foreach (ImagesProcessingData ipdt in lStatsProcessing)
                {
                    ipdt.grixyrgbStatsXMLfile =
                        statsFilesList.First(
                            statsFname =>
                                statsFname.Contains(ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(ipdt.filename, "",
                                    false)));

                    ipdt.grixyrgbStats =
                        ServiceTools.ReadObjectFromXML(ipdt.grixyrgbStatsXMLfile, typeof(SkyImageIndexesStatsData)) as
                            SkyImageIndexesStatsData;

                    #region calculate and report progress

                    filesRead++;
                    double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                    if (progress - (double)currProgressPerc > 1.0d)
                    {
                        currProgressPerc = Convert.ToInt32(progress);
                        Console.WriteLine("read " + currProgressPerc + "%");
                    }

                    #endregion calculate and report progress
                }

                #endregion




                #region list, read and map concurrent data

                List<ConcurrentData> lConcurrentData = null;

                List<string> concurrentDataFilesList =
                    Directory.EnumerateFiles(ConcurrentDataXMLfilesDirectory,
                        ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(),
                        bEnumerateFilesRecursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();

                #region reading
                Console.WriteLine("started concurrent data reading");

                totalFilesCountToRead = concurrentDataFilesList.Count;
                filesRead = 0;
                currProgressPerc = 0;

                List<Dictionary<string, object>> lDictionariesConcurrentData =
                    new List<Dictionary<string, object>>();
                foreach (string strConcDataXMLFile in concurrentDataFilesList)
                {
                    Dictionary<string, object> currDict = ServiceTools.ReadDictionaryFromXML(strConcDataXMLFile);
                    currDict.Add("XMLfileName", Path.GetFileName(strConcDataXMLFile));

                    lDictionariesConcurrentData.Add(currDict);

                    #region calculate and report progress

                    filesRead++;
                    double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                    if (progress - (double)currProgressPerc > 1.0d)
                    {
                        currProgressPerc = Convert.ToInt32(progress);
                        Console.WriteLine("read " + currProgressPerc + "%");
                    }

                    #endregion calculate and report progress
                }

                lDictionariesConcurrentData.RemoveAll(dict => dict == null);
                lConcurrentData =
                    lDictionariesConcurrentData.ConvertAll<ConcurrentData>(dict =>
                    {
                        ConcurrentData retVal = null;
                        try
                        {
                            retVal = new ConcurrentData(dict);
                        }
                        catch (Exception ex)
                        {
                            string strError = "couldn`t parse XML file " + dict["XMLfileName"] + " : " +
                                              Environment.NewLine + ex.Message;
                            Console.WriteLine(strError);
                        }
                        return retVal;
                    });
                lConcurrentData.RemoveAll(val => val == null);
                #endregion reading


                #region mapping

                // map obtained concurrent data to images by its datetime
                Console.WriteLine("concurrent data mapping started");

                lStatsProcessing = lStatsProcessing.ConvertAll(ipdt =>
                {
                    string currImgFilename = ipdt.filename;
                    currImgFilename = Path.GetFileNameWithoutExtension(currImgFilename);

                    DateTime currImgDT = ConventionalTransitions.DateTimeOfSkyImageFilename(currImgFilename);

                    ConcurrentData nearestConcurrentData = lConcurrentData.Aggregate((cDt1, cDt2) =>
                    {
                        TimeSpan tspan1 = new TimeSpan(Math.Abs((cDt1.datetimeUTC - currImgDT).Ticks));
                        TimeSpan tspan2 = new TimeSpan(Math.Abs((cDt2.datetimeUTC - currImgDT).Ticks));
                        return ((tspan1 <= tspan2) ? (cDt1) : (cDt2));
                    });


                    if (new TimeSpan(Math.Abs((nearestConcurrentData.datetimeUTC - currImgDT).Ticks)) >=
                        TimeSpanForConcurrentDataMappingTolerance)
                    {
                        string strError = "couldn`t find close enough concurrent data file for image:" + Environment.NewLine +
                            currImgFilename + Environment.NewLine + "closest concurrent data file is:" +
                            Environment.NewLine + nearestConcurrentData.filename + Environment.NewLine +
                            "with date-time value " + nearestConcurrentData.datetimeUTC.ToString("o");
                        Console.WriteLine(strError);
                        nearestConcurrentData = null;
                    }

                    ipdt.concurrentData = nearestConcurrentData;
                    if (nearestConcurrentData != null)
                    {
                        ipdt.concurrentDataXMLfile = nearestConcurrentData.filename;
                    }

                    return ipdt;
                });

                #endregion mapping

                removed = lStatsProcessing.RemoveAll(ipdt => ipdt.concurrentData == null);
                Console.WriteLine("removed " + removed + " items (couldn`t find concurrent data). " + lStatsProcessing.Count + " files remains to process.");

                #endregion list, read and map concurrent data

            }





            if (!lStatsProcessing.Any())
            {
                Console.WriteLine("There is no files that sutisfy settings specified and have all required concurrent data (stats or GPS etc.). Processing will not be proceeded.");
                return;
            }


            #region Filter by SDC values predicting it using pre-trained NN parameters


            string csvHeader = lStatsProcessing[0].grixyrgbStats.CSVHeader() +
                           ",SunElevationDeg,SunAzimuthDeg,sunDiskCondition";
            List<string> lCSVheader = csvHeader.Split(',').ToList();
            List<int> columnsToDelete =
                lCSVheader.Select((str, idx) => new Tuple<int, string>(idx, str))
                    .Where(tpl => tpl.Item2.ToLower().Contains("filename")).ToList().ConvertAll(tpl => tpl.Item1);

            List<List<string>> lCalculatedData = lStatsProcessing.ConvertAll(dt =>
            {
                string currImageALLstatsDataCSVWithConcurrentData = dt.grixyrgbStats.ToCSV() + "," +
                                                                    dt.concurrentData.gps.SunZenithAzimuth()
                                                                        .ElevationAngle.ToString()
                                                                        .Replace(",", ".") + "," +
                                                                    dt.concurrentData.gps.SunZenithAzimuth()
                                                                        .Azimuth.ToString()
                                                                        .Replace(",", ".");
                List<string> retVal = currImageALLstatsDataCSVWithConcurrentData.Split(',').ToList();
                retVal = retVal.Where((str, idx) => !columnsToDelete.Contains(idx)).ToList();
                return retVal;
            });


            List<DenseVector> lDV_objects_features =
                lCalculatedData.ConvertAll(
                    list =>
                        DenseVector.OfEnumerable(list.ConvertAll<double>(str => Convert.ToDouble(str.Replace(".", ",")))));


            DenseVector dvMeans = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormMeansFile, 0, ",")).Row(0);
            DenseVector dvRanges = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormRangeFile, 0, ",")).Row(0);

            lDV_objects_features = lDV_objects_features.ConvertAll(dv =>
            {
                DenseVector dvShifted = dv - dvMeans;
                DenseVector dvNormed = (DenseVector)dvShifted.PointwiseDivide(dvRanges);
                return dvNormed;
            });

            DenseMatrix dmObjectsFeatures = DenseMatrix.OfRowVectors(lDV_objects_features);

            DenseVector dvThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(NNtrainedParametersFile, 0, ",");
            List<int> NNlayersConfig =
                new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(NNconfigFile, 0, ",")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));


            List<List<double>> lDecisionProbabilities = null;

            List<int> predictedSDC =
                NNclassificatorPredictor.NNpredict(dmObjectsFeatures, dvThetaValues, NNlayersConfig,
                    out lDecisionProbabilities).ToList();


            List<SunDiskCondition> predictedSDClist = predictedSDC.ConvertAll(sdcInt =>
            {
                switch (sdcInt)
                {
                    case 4:
                        return SunDiskCondition.NoSun;
                        break;
                    case 1:
                        return SunDiskCondition.Sun0;
                        break;
                    case 2:
                        return SunDiskCondition.Sun1;
                        break;
                    case 3:
                        return SunDiskCondition.Sun2;
                        break;
                    default:
                        return SunDiskCondition.Defect;
                }
            });

            List<Tuple<ImagesProcessingData, SunDiskCondition>> lTplsPredictedSDClist =
                predictedSDClist.Zip(lStatsProcessing,
                    (sdc, ipd) => new Tuple<ImagesProcessingData, SunDiskCondition>(ipd, sdc)).ToList();

            #region output obtained SDC data to log file

            string strToShow = "SDC values probabilities: " + Environment.NewLine +
                "| No Sun | Sun_0  | Sun_1  | Sun_2  | Detected |" + Environment.NewLine;
            foreach (List<double> lDecisionProbability in lDecisionProbabilities)
            {
                strToShow += "| " + lDecisionProbability[3].ToString("F4") +
                             " | " + lDecisionProbability[0].ToString("F4") +
                             " | " + lDecisionProbability[1].ToString("F4") +
                             " | " + lDecisionProbability[2].ToString("F4") + " |" +
                             predictedSDClist[lDecisionProbabilities.IndexOf(lDecisionProbability)] + "|" +
                             Environment.NewLine;
            }
            ServiceTools.logToTextFile(errorLogFilename, strToShow, true, false);

            #endregion output obtained SDC data to log file

            if (sdcFilter != SunDiskCondition.Undefined)
            {
                lStatsProcessing = lStatsProcessing.Where((ipd, idx) => predictedSDClist[idx] == sdcFilter).ToList();
                Console.WriteLine("Detected " + lStatsProcessing.Count + " images with SDC = " + sdcFilter.ToString());
            }

            #endregion Filter by SDC values predicting it using pre-trained NN parameters




            #region ObservedCloudCoverDataCSVfile

            if (bFilterByObservedCloudCoverRecords)
            {
                Console.WriteLine("Reading observed cloud cover data CSV file...");

                if (!File.Exists(ObservedCloudCoverDataCSVfile))
                {
                    Console.WriteLine("Unable to read observed data CSV file: " + ObservedCloudCoverDataCSVfile);
                    return;
                }

                List<List<string>> lCSVfileContents = ServiceTools.ReadDataFromCSV(ObservedCloudCoverDataCSVfile, 1,
                    true);

                if (!lCSVfileContents.Any())
                {
                    Console.WriteLine("The observed cloud cover CSV file seems to be empty: " +
                                      ObservedCloudCoverDataCSVfile);
                    return;
                }

                List<ObservedClCoverData> lObservedData =
                    lCSVfileContents.ConvertAll(lStr => new ObservedClCoverData(lStr));




                List<SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC> lImagesFilteredByAvailableObservedData =
                    new List<SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC>();


                #region filter images by available observed data using DateTimeFilterTolerance

                Console.WriteLine("Filtering by observed data available...");

                foreach (ObservedClCoverData observedData in lObservedData)
                {
                    DateTime currObservedDatumDateTime = observedData.dt;
                    List<SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC> lImagesCloseToCurrObservedDatum = lStatsProcessing
                        .Where(ipd =>
                        {
                            TimeSpan tspan =
                                new TimeSpan(
                                    Math.Abs(
                                        (ConventionalTransitions.DateTimeOfSkyImageFilename(ipd.filename) -
                                         currObservedDatumDateTime).Ticks));
                            return (tspan <= DateTimeFilterTolerance);
                        })
                        .ToList()
                        .ConvertAll(ifd => new SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC()
                        {
                            // observedData
                            // ifd
                            skyImageFullFileName = ifd.filename,
                            skyImageFileName = Path.GetFileName(ifd.filename),
                            currImageDateTime = ConventionalTransitions.DateTimeOfSkyImageFilename(ifd.filename),
                            observedCloudCoverData = observedData,
                            concurrentDataXMLfile = ifd.concurrentDataXMLfile,
                            concurrentData = ifd.concurrentData,
                            grixyrgbStatsXMLfile = ifd.grixyrgbStatsXMLfile,
                            grixyrgbStats = ifd.grixyrgbStats,
                            SDCvalue = lTplsPredictedSDClist.Where(tpl => tpl.Item1 == ifd).ElementAt(0).Item2
                        });

                    lImagesFilteredByAvailableObservedData.AddRange(lImagesCloseToCurrObservedDatum);
                }

                #endregion filter images by available observed data using DateTimeFilterTolerance

                if (!lImagesFilteredByAvailableObservedData.Any())
                {
                    Console.WriteLine(
                        "There is no images remain after filtering using all available data. Output will be empty.");
                }

                ServiceTools.WriteObjectToXML(lImagesFilteredByAvailableObservedData, outputDataFile);

                #region Сформируем и запишем данные в CSV-файл
                // Здесь есть данные по наблюдаемому CloudCover

                csvHeader = lImagesFilteredByAvailableObservedData[0].grixyrgbStats.CSVHeader() +
                               ",SunElevationDeg,SunAzimuthDeg,ObservedTotalCloudCover,ObservedLowerCloudCover,SDC";
                List<string> lCSVoutputData = lImagesFilteredByAvailableObservedData.ConvertAll(ifd =>
                {
                    // все стат. предикторы - как для SDC
                    // данные CloudCover
                    string retVal = "";
                    //ImagesProcessingData dt = tpl.Item2;
                    //ObservedClCoverData clCov = tpl.Item1;
                    retVal = ifd.grixyrgbStats.ToCSV() + "," +
                             ifd.concurrentData.gps.SunZenithAzimuth().ElevationAngle.ToString().Replace(",", ".") + "," +
                             ifd.concurrentData.gps.SunZenithAzimuth().Azimuth.ToString().Replace(",", ".") + "," +
                             ifd.observedCloudCoverData.CloudCoverTotal.ToString() + "," +
                             ifd.observedCloudCoverData.CloudCoverLower.ToString() + "," +
                             ifd.SDCvalue.ToString();
                    return retVal;
                });
                string strToOutputToCSVfile = string.Join(Environment.NewLine, lCSVoutputData);

                ServiceTools.logToTextFile(outputCSVfile, csvHeader + Environment.NewLine, true, false);
                ServiceTools.logToTextFile(outputCSVfile, strToOutputToCSVfile, true, false);

                #endregion Сформируем и запишем данные в CSV-файл

            }
            else
            {
                ServiceTools.WriteObjectToXML(lStatsProcessing, outputDataFile);

                #region Сформируем и запишем данные в CSV-файл
                // здесь нет данных по наблюдаемому Cloud Cover
                // не надо нам такое. Оставим все это только в виде XML-файла.

                #endregion Сформируем и запишем данные в CSV-файл
            }
            #endregion ObservedCloudCoverDataCSVfile

            #endregion Enumerating files and output to XML and CSV file




            Console.WriteLine("saved output data to file: " + Environment.NewLine + outputDataFile + Environment.NewLine +
                              Environment.NewLine);
            Console.WriteLine("===FINISHED===");
            Console.ReadKey();
        }

    }
}
