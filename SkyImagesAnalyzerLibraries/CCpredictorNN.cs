using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ANN;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SkyImagesAnalyzerLibraries
{
    public class CCpredictorNN
    {
        private bool bNeedToCalculateStats = false;


        public string ImageFilename { get; set; }
        public string ConcurrentDataXMLfilesBasePath { get; set; }
        public string imageYRGBstatsXMLdataFilesDirectory { get; set; }
        public string ImagesRoundMasksXMLfilesMappingList { get; set; }


        private SkyImageIndexesStatsData currImageStatsData = null;
        List<int> lConcurrentDataAlreadyRead = null;



        public CCpredictorNN(string strImageFilename, string strConcurrentDataXMLfilesBasePath, string strimageYRGBstatsXMLdataFilesDirectory)
        {
            if (!File.Exists(strImageFilename))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + strImageFilename);
            }

            ImageFilename = strImageFilename;
            ConcurrentDataXMLfilesBasePath = strConcurrentDataXMLfilesBasePath;
            imageYRGBstatsXMLdataFilesDirectory = strimageYRGBstatsXMLdataFilesDirectory;
        }







        public async Task<int> CalcCC_NN(string SDC_NNconfigFile, string SDC_NNtrainedParametersFile, string NormMeansFile,
            string NormRangeFile, string CC_NNconfigFile, string CC_NNtrainedParametersFile)
        {
            if (!File.Exists(SDC_NNconfigFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + SDC_NNconfigFile);
            }
            if (!File.Exists(SDC_NNtrainedParametersFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + SDC_NNtrainedParametersFile);
            }
            if (!File.Exists(NormMeansFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + NormMeansFile);
            }
            if (!File.Exists(NormRangeFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + NormRangeFile);
            }

            if ((ImagesRoundMasksXMLfilesMappingList == "") || (ImagesRoundMasksXMLfilesMappingList == null))
            {
                ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar +
                                          "ImagesRoundMasksXMLfilesMappingList.csv";
            }
            if (!File.Exists(ImagesRoundMasksXMLfilesMappingList))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + ImagesRoundMasksXMLfilesMappingList);
            }



            #region read or calculate GrIxYRGB stats

            if (Directory.Exists(imageYRGBstatsXMLdataFilesDirectory))
            {
                List<string> foundXMLfiles = Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory,
                    ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(ImageFilename, "", false),
                    SearchOption.AllDirectories).ToList();
                if (foundXMLfiles.Any())
                {
                    currImageStatsData =
                        (SkyImageIndexesStatsData)
                            ServiceTools.ReadObjectFromXML(foundXMLfiles[0], typeof(SkyImageIndexesStatsData));
                }
            }


            if (currImageStatsData == null)
            {
                Task<SkyImageIndexesStatsData> tskImageStatsCalculation = new Task<SkyImageIndexesStatsData>(() =>
                {
                    Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
                    optionalParameters.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    optionalParameters.Add("Stopwatch", sw);

                    ImageStatsDataCalculationResult currImageProcessingResult = null;
                    try
                    {
                        currImageProcessingResult = ImageProcessing.CalculateImageStatsData(ImageFilename,
                            optionalParameters);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }



                    if (currImageProcessingResult == null)
                    {
                        return null;
                    }
                    else
                    {
                        string strImageGrIxYRGBDataFileName =
                            ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(ImageFilename,
                                imageYRGBstatsXMLdataFilesDirectory);
                        ServiceTools.WriteObjectToXML(currImageProcessingResult.grixyrgbStatsData,
                            strImageGrIxYRGBDataFileName);

                        currImageStatsData = currImageProcessingResult.grixyrgbStatsData;



                        return currImageProcessingResult.grixyrgbStatsData;
                    }
                });

                currImageStatsData = await tskImageStatsCalculation;
            }

            if (currImageStatsData == null)
            {
                // theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR searching concurrent data for image. Will not proceed.");
                throw new Exception("ERROR searching concurrent data for image. Will not proceed.");
            }

            #endregion read or calculate GrIxYRGB stats


            ConcurrentData nearestConcurrentData = null;



            #region search for concurrent data

            Task<ConcurrentData> tskImageConcurrentDataSearching = new Task<ConcurrentData>(() =>
            {
                string currImgFilename = Path.GetFileNameWithoutExtension(ImageFilename);
                string ptrn = @"(devID\d)";
                Regex rgxp = new Regex(ptrn, RegexOptions.IgnoreCase);

                string strCurrImgDT = rgxp.Replace(currImgFilename.Substring(4), "");
                //2015-12-16T06-01-38
                strCurrImgDT = strCurrImgDT.Substring(0, 11) + strCurrImgDT.Substring(11).Replace("-", ":");

                DateTime currImgDT = DateTime.Parse(strCurrImgDT, null,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);



                List<Tuple<string, ConcurrentData>> lImagesConcurrentData =
                    new List<Tuple<string, ConcurrentData>>();

                List<string> filesListConcurrentData =
                    Directory.EnumerateFiles(ConcurrentDataXMLfilesBasePath,
                        ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(),
                        SearchOption.AllDirectories)
                        .ToList();

                List<Tuple<string, DateTime>> XMLfilesListConcurrentData = filesListConcurrentData.ConvertAll(
                    strXMLfilename =>
                    {
                        string xmlFile1DateTimeString =
                            Path.GetFileNameWithoutExtension(strXMLfilename).Replace("data-", "").Substring(0, 19);
                        xmlFile1DateTimeString = xmlFile1DateTimeString.Substring(0, 11) + xmlFile1DateTimeString.Substring(11).Replace("-", ":");
                        DateTime dt1 = DateTime.Parse(xmlFile1DateTimeString);
                        return new Tuple<string, DateTime>(strXMLfilename, dt1);
                    });

                string nearestConcurrentDataFileName = XMLfilesListConcurrentData.Aggregate((cDt1, cDt2) =>
                {
                    TimeSpan tspan1 = new TimeSpan(Math.Abs((cDt1.Item2 - currImgDT).Ticks));
                    TimeSpan tspan2 = new TimeSpan(Math.Abs((cDt2.Item2 - currImgDT).Ticks));
                    return ((tspan1 <= tspan2) ? (cDt1) : (cDt2));
                }).Item1;

                Dictionary<string, object> currDict = ServiceTools.ReadDictionaryFromXML(nearestConcurrentDataFileName);
                currDict.Add("XMLfileName", Path.GetFileName(nearestConcurrentDataFileName));
                ConcurrentData nearestConcurrentDataObtained = new ConcurrentData(currDict);

                if (new TimeSpan(Math.Abs((nearestConcurrentDataObtained.datetimeUTC - currImgDT).Ticks)) >=
                    new TimeSpan(0, 2, 0))
                {
                    nearestConcurrentDataObtained = null;
                }

                return nearestConcurrentDataObtained;
            });

            nearestConcurrentData = await tskImageConcurrentDataSearching;

            if (nearestConcurrentData == null)
            {
                // theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR searching concurrent data for image. Will not proceed.");
                throw new Exception("ERROR searching concurrent data for image. Will not proceed.");
            }

            #endregion search for concurrent data



            DenseVector dvMeans = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormMeansFile, 0, ",")).Row(0);
            DenseVector dvRanges = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormRangeFile, 0, ",")).Row(0);
            DenseVector dvThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(SDC_NNtrainedParametersFile, 0, ",");
            DenseVector dv_CC_ThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(CC_NNtrainedParametersFile, 0, ",");
            List<int> NNlayersConfig =
                new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(SDC_NNconfigFile, 0, ",")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));
            List<int> CC_NNlayersConfig =
                new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(CC_NNconfigFile, 0, ",")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));
            return PredictCC_NN(currImageStatsData, nearestConcurrentData, NNlayersConfig, dvThetaValues, dvMeans,
                dvRanges, CC_NNlayersConfig, dv_CC_ThetaValues);

        }





        public static int CalcCC_NN(string statsXMLfile, string concurrentDataXMLfile, string SDC_NNconfigFile,
            string SDC_NNtrainedParametersFile, string NormMeansFile, string NormRangeFile, string CC_NNconfigFile,
            string CC_NNtrainedParametersFile)
        {
            if (!File.Exists(SDC_NNconfigFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + SDC_NNconfigFile);
            }
            if (!File.Exists(SDC_NNtrainedParametersFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + SDC_NNtrainedParametersFile);
            }
            if (!File.Exists(NormMeansFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + NormMeansFile);
            }
            if (!File.Exists(NormRangeFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + NormRangeFile);
            }
            if (!File.Exists(CC_NNconfigFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + CC_NNconfigFile);
            }
            if (!File.Exists(CC_NNtrainedParametersFile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + CC_NNtrainedParametersFile);
            }
            if (!File.Exists(statsXMLfile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + statsXMLfile);
            }
            if (!File.Exists(concurrentDataXMLfile))
            {
                throw new FileNotFoundException("couldn`t find the file specified: " + concurrentDataXMLfile);
            }


            #region read or calculate GrIxYRGB stats

            SkyImageIndexesStatsData currImageStatsData = null;
            try
            {
                currImageStatsData =
                    (SkyImageIndexesStatsData)
                        ServiceTools.ReadObjectFromXML(statsXMLfile, typeof(SkyImageIndexesStatsData));
            }
            catch (Exception ex)
            {
                throw ex;
            }


            if (currImageStatsData == null)
            {
                throw new Exception("ERROR reading stats data for image. Will not proceed.");
            }

            #endregion read or calculate GrIxYRGB stats


            #region search for concurrent data

            ConcurrentData nearestConcurrentData = null;
            try
            {
                Dictionary<string, object> currDict = ServiceTools.ReadDictionaryFromXML(concurrentDataXMLfile);
                currDict.Add("XMLfileName", Path.GetFileName(concurrentDataXMLfile));
                nearestConcurrentData = new ConcurrentData(currDict);
            }
            catch (Exception)
            {
                throw new Exception("ERROR reading concurrent data for image. Will not proceed."); ;
            }

            if (nearestConcurrentData == null)
            {
                // theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR searching concurrent data for image. Will not proceed.");
                throw new Exception("ERROR searching concurrent data for image. Will not proceed.");
            }

            #endregion search for concurrent data




            DenseVector dvMeans = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormMeansFile, 0, ",")).Row(0);
            DenseVector dvRanges = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormRangeFile, 0, ",")).Row(0);
            DenseVector dvThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(SDC_NNtrainedParametersFile, 0, ",");
            DenseVector dv_CC_ThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(CC_NNtrainedParametersFile, 0, ",");
            List<int> NNlayersConfig =
                new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(SDC_NNconfigFile, 0, ",")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));
            List<int> CC_NNlayersConfig =
                new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(CC_NNconfigFile, 0, ",")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));

            return PredictCC_NN(currImageStatsData, nearestConcurrentData, NNlayersConfig, dvThetaValues, dvMeans,
                dvRanges, CC_NNlayersConfig, dv_CC_ThetaValues);
        }






        public static int PredictCC_NN(SkyImageIndexesStatsData imageStats, ConcurrentData snapshotConcurrentData,
            IEnumerable<int> SDC_NNconfig, IEnumerable<double> SDC_NNtrainedParameters, IEnumerable<double> NNfeturesNormMeans,
            IEnumerable<double> NNfeaturesNormRange, IEnumerable<int> CC_NNconfig, IEnumerable<double> CC_NNtrainedParameters)
        {
            string currImageALLstatsDataCSVWithConcurrentData = imageStats.ToCSV() + "," +
                snapshotConcurrentData.gps.SunZenithAzimuth().ElevationAngle.ToString().Replace(",", ".") + "," +
                snapshotConcurrentData.gps.SunZenithAzimuth().Azimuth.ToString().Replace(",", ".");

            List<string> lCalculatedData = new List<string>();
            lCalculatedData.Add(currImageALLstatsDataCSVWithConcurrentData);

            string csvHeader = imageStats.CSVHeader() + ",SunElevationDeg,SunAzimuthDeg,CloudCover";



            List<List<string>> csvFileContentStrings =
                lCalculatedData.ConvertAll(str => str.Split(',').ToList()).ToList();
            List<string> lCSVheader = csvHeader.Split(',').ToList();

            List<int> columnsToDelete =
                lCSVheader.Select((str, idx) => new Tuple<int, string>(idx, str))
                    .Where(tpl => tpl.Item2.ToLower().Contains("filename")).ToList().ConvertAll(tpl => tpl.Item1);
            List<List<string>> csvFileContentStringsFiltered = new List<List<string>>();
            foreach (List<string> listDataStrings in csvFileContentStrings)
            {
                csvFileContentStringsFiltered.Add(
                    listDataStrings.Where((str, idx) => !columnsToDelete.Contains(idx)).ToList());
            }
            

            List<List<string>> csvFileContentStringsFiltered_wo_CC = csvFileContentStringsFiltered;


            List<DenseVector> lDV_objects_features =
                csvFileContentStringsFiltered_wo_CC.ConvertAll(
                    list =>
                        DenseVector.OfEnumerable(list.ConvertAll<double>(str => Convert.ToDouble(str.Replace(".", ",")))));


            DenseVector dvMeans = DenseVector.OfEnumerable(NNfeturesNormMeans);
            DenseVector dvRanges = DenseVector.OfEnumerable(NNfeaturesNormRange);


            #region normalize features

            lDV_objects_features = lDV_objects_features.ConvertAll(dv =>
            {
                DenseVector dvShifted = dv - dvMeans;
                DenseVector dvNormed = (DenseVector)dvShifted.PointwiseDivide(dvRanges);
                return dvNormed;
            });

            #endregion normalize features


            #region adding SDC feature

            // добавить последнюю фичу - SDC
            List<int> sdcMatlabValues = new List<int>();
            List<double> lSDCpredictionProbabilities = new List<double>();
            SunDiskCondition sdc = SDCpredictorNN.PredictSDC_NN(imageStats, snapshotConcurrentData, SDC_NNconfig,
                SDC_NNtrainedParameters, NNfeturesNormMeans, NNfeaturesNormRange, out lSDCpredictionProbabilities);
            sdcMatlabValues.Add(SunDiskConditionData.MatlabNumeralSDC(sdc));

            lDV_objects_features = lDV_objects_features.Zip(sdcMatlabValues, (dv, intSDC) =>
            {
                List<double> lFeaturesWithSDCdata = lDV_objects_features[0].ToList();
                lFeaturesWithSDCdata.Add((double) intSDC);
                return DenseVector.OfEnumerable(lFeaturesWithSDCdata);
            }).ToList();

            #endregion adding SDC feature

            DenseMatrix dmObjectsFeatures = DenseMatrix.OfRowVectors(lDV_objects_features);




            List<List<double>> lDecisionProbabilities = null;

            List<int> predictedCC =
                NNclassificatorPredictor.NNpredict(dmObjectsFeatures, CC_NNtrainedParameters, CC_NNconfig,
                    out lDecisionProbabilities).ToList();


            // Matlab trained TCC model: classes 1-9
            predictedCC = predictedCC.ConvertAll(iVal => iVal - 1);


            return predictedCC[0];
        }
    }
}
