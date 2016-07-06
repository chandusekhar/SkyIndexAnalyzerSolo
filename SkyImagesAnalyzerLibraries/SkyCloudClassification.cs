using System;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
//using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.Statistics;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ANN;
using Geometry;
using MathNet.Numerics.LinearAlgebra;
using MKLwrapper;
using DataAnalysis;


namespace SkyImagesAnalyzerLibraries
{
    #region УСТАРЕЛО obsolete structures
    //public enum CloudOrSky
    //{
    //    Sky,
    //    Cloud,
    //    Nothing
    //}



    //public struct Classification
    //{
    //    public double SkyCloudMargin;
    //    public CloudOrSky IsCloud;
    //}


    //public struct ClassificationData
    //{
    //    public int x;
    //    public int y;
    //    public int ColorRed;
    //    public int ColorGreen;
    //    public int ColorBlue;
    //    public double SI;
    //    public Classification[] IsCloudByMargin;
    //}
    #endregion УСТАРЕЛО obsolete structures

    public enum ClassificationMethods
    {
        US,
        Japan,
        Greek,
        GrIx
    }

    public enum SunDiskConditions
    {
        second,
        first,
        zero,
        Cloudy
    }


    public class SkyCloudClassification : IDisposable
    {
        //private Bitmap LocalProcessingBitmap;
        private Image<Bgr, Byte> LocalProcessingImage;
        public Bitmap localPreviewBitmap;
        //private double localCloudSkySeparationValue;
        public double cloudSkySeparationValue;
        private Byte localCloudSkySeparationValueByte;
        //public ClassificationData[,] ClassifiedBM;
        //private PictureBox pbPreviewDisplayControl;
        //private ProgressBar defaultProgressBarControl;
        public Form ParentForm;
        //private TextBox tbLog;
        private int DimX = 0, DimY = 0;
        public int totalstepscount = 0;
        public int currentstep = 0;
        public double CloudCover;
        public int skyCounter, cloudCounter;
        public ClassificationMethods ClassificationMethod;
        public Image<Gray, Byte> maskImage = null;
        public DenseMatrix dmSkyIndexData;
        public string resultingStatusMessages = "";

        #region переменные для нашего тестового анализатора
        public double minSunAreaPart = 0.0025d;
        public double maxSunAreaPart = 0.05d;
        public double aveSunAreaPart = 0.01d;
        public double theStdDevMarginValueDefiningTrueSkyArea = 0.65d;
        public double theStdDevMarginValueDefiningSkyCloudSeparation = 0.25d;
        public double theStdDevMarginValueDefiningSkyCloudSeparation_SunSuppressed = 0.1d;
        public double minSunburnYValue = 254.0d;
        public double minSunburnGrIxValue = 1.0d;
        public double dSunDetectorArcedCropFactor = 0.85d;
        #endregion переменные для нашего тестового анализатора

        public bool isCalculatingUsingBgWorker = true;
        private BackgroundWorker selfWorker = null;
        public LogWindow theLogWindow = null;
        public string defaultOutputDataDirectory = "";
        private double theImageCircleCropFactor = 0.9d;

        public Dictionary<string, object> defaultProperties = null;
        public int verbosityLevel = 0;
        public string randomFileName = "";

        public string sourceImageFileName = "";
        public int concurrentThreadsLimit = 2;

        //public SunDiskConditions currentSunDiskCondition;


        private Image<Bgr, byte> tmpSunDetectionimage = null;

        public bool theSunSuppressionSchemeApplicable = false;
        public bool forceExistingSunInformation = false;

        List<Tuple<string, string>> lImagesRoundMasksMappingFiles = null;
        private TimeSpan TimeSpanForConcurrentDataMappingTolerance;
        private string ConcurrentDataXMLfilesDirectory = "";


        public Bitmap PreviewBitmap { get { return localPreviewBitmap; } }



        public Bitmap BitmapinProcess
        {
            get { return LocalProcessingImage.Bitmap; }
            set { LocalProcessingImage = new Image<Bgr, byte>(value); }
        }

        public LogWindow LogWindow
        {
            get { return theLogWindow; }
            set { theLogWindow = value; }
        }

        public BackgroundWorker SelfWorker
        {
            get { return selfWorker; }
            set
            {
                selfWorker = value;
                if (selfWorker == null)
                {
                    isCalculatingUsingBgWorker = false;
                }
                else
                {
                    isCalculatingUsingBgWorker = true;
                }
            }
        }


        public SkyImagesDataWith_Concurrent_Stats_CloudCover imgConcurrentDataAndStats { get; set; }



        //public SkyCloudClassification(Image OrigImage, PictureBox DisplayControl, ProgressBar ProgressBarControl, Form inParentForm)
        //public SkyCloudClassification(Image OrigImage, PictureBox DisplayControl, ProgressBar ProgressBarControl, TextBox inReportingTextBox)
        //public SkyCloudClassification(Image OrigImage, Dictionary<string, object> settings)
        public SkyCloudClassification(Image<Bgr, Byte> OrigImage, Dictionary<string, object> settings)
        {
            //LocalProcessingBitmap = new Bitmap(ImageProcessing.SquareImageDimensions(OrigImage));
            LocalProcessingImage = ImageProcessing.SquareImageDimensions(OrigImage);

            localPreviewBitmap = new Bitmap(LocalProcessingImage.Bitmap, LocalProcessingImage.Width,
                LocalProcessingImage.Height);
            DimX = LocalProcessingImage.Width;
            DimY = LocalProcessingImage.Height;

            //tbLog = inReportingTextBox;

            CloudCover = 0.0;
            //ClassifiedBM = null;
            skyCounter = 0;
            cloudCounter = 0;
            ClassificationMethod = ClassificationMethods.GrIx;
            dmSkyIndexData = null;

            defaultProperties = settings;

            cloudSkySeparationValue = Convert.ToDouble(defaultProperties["JapanCloudSkySeparationValue"]);
            minSunAreaPart = Convert.ToDouble(defaultProperties["GrIxSunDetectionMinimalSunAreaPartial"]);
            maxSunAreaPart = Convert.ToDouble(defaultProperties["GrIxSunDetectionMaximalSunAreaPartial"]);
            aveSunAreaPart = Convert.ToDouble(defaultProperties["GrIxSunDetectionDesiredSunAreaPartial"]);
            theStdDevMarginValueDefiningTrueSkyArea =
                Convert.ToDouble(defaultProperties["GrIxStdDevMarginValueDefiningTrueSkyArea"]);
            theStdDevMarginValueDefiningSkyCloudSeparation =
                Convert.ToDouble(defaultProperties["GrIxDefaultSkyCloudMarginWithoutSun"]);
            theStdDevMarginValueDefiningSkyCloudSeparation_SunSuppressed =
                Convert.ToDouble(defaultProperties["GrIxDefaultSkyCloudMarginWithSun"]);
            theImageCircleCropFactor = Convert.ToDouble(defaultProperties["GrIxAnalysisImageCircledCropFactor"]);
            minSunburnYValue = Convert.ToDouble(defaultProperties["GrIxMinimalSunburnYvalue"]);
            minSunburnGrIxValue = Convert.ToDouble(defaultProperties["GrIxMinimalSunburnGrIxvalue"]);
            dSunDetectorArcedCropFactor = Convert.ToDouble(defaultProperties["GrIxSunDetectorArcedCropFactor"]);
            concurrentThreadsLimit = Convert.ToInt32(defaultProperties["GrIxSunDetectorConcurrentThreadsLimit"]);

            if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
            {

                string ImagesRoundMasksXMLfilesMappingList =
                    (string)defaultProperties["ImagesRoundMasksXMLfilesMappingList"];
                if (File.Exists(ImagesRoundMasksXMLfilesMappingList))
                {
                    List<List<string>> llImagesRoundMasksMappingFiles =
                        ServiceTools.ReadDataFromCSV(ImagesRoundMasksXMLfilesMappingList, 0, true, ";",
                            Environment.NewLine);
                    lImagesRoundMasksMappingFiles =
                        llImagesRoundMasksMappingFiles.ConvertAll(
                            list => new Tuple<string, string>(list[0], list[1]));
                    // item1: images filename pattern
                    // item2: image rounded mask parameters XML file
                }
            }

            TimeSpanForConcurrentDataMappingTolerance = TimeSpan.Parse((string)defaultProperties["TimeSpanForConcurrentDataMappingTolerance"]);
            if (defaultProperties.ContainsKey("ConcurrentDataXMLfilesDirectory"))
            {
                ConcurrentDataXMLfilesDirectory = (string)defaultProperties["ConcurrentDataXMLfilesDirectory"];
            }

        }



        #region классификация методом из японских публикаций
        private void ClassifyJapan()
        {
            DenseMatrix denseMatrixRedChannel;
            DenseMatrix denseMatrixBlueChannel;


            if (cloudSkySeparationValue == 0.0)
            {
                //ThreadSafeOperations.SetTextTB(tbLog, "Не установлена граница SI между небом и облаком для проведения классификации." + Environment.NewLine, true);
                throw new Exception("Не установлено значение границы небо-облако по значению SkyIndex.");
            }


            Image<Gray, Byte> imageBlueChannelByte = LocalProcessingImage[0].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[0];
            Image<Gray, Byte> imageRedChannelByte = LocalProcessingImage[2].Copy();// new Image<Bgr, Byte>(LocalProcessingBitmap)[2];


            //Image<Gray, Byte> maskImage = ImageProcessing.getImageSignificantMask(LocalProcessingBitmap);
            ImageProcessing imgP = new ImageProcessing(LocalProcessingImage, true);
            //imp.getImageSignificantMask();
            //maskImage = imp.significantMaskImageBinary;
            maskImage = imgP.significantMaskImageCircled;

            imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage);
            imageRedChannelByte = imageRedChannelByte.Mul(maskImage);
            denseMatrixRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
            denseMatrixBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
            ServiceTools.FlushMemory();

            String evalExpression = "(B-R)/(B+R)";
            ArithmeticsOnImages aoi = new ArithmeticsOnImages();
            aoi.dmR = denseMatrixRedChannel;
            aoi.dmB = denseMatrixBlueChannel;
            aoi.dmG = denseMatrixRedChannel;
            aoi.ExprString = evalExpression;
            aoi.RPNeval();
            ColorScheme skyCloudColorScheme = ColorScheme.BinaryCloudSkyColorScheme(cloudSkySeparationValue, aoi.dmRes.Values.Min(), aoi.dmRes.Values.Max());
            ColorSchemeRuler skyCloudRuler = new ColorSchemeRuler(skyCloudColorScheme, aoi.dmRes.Values.Min(), aoi.dmRes.Values.Max());


            #region debug xls dump
            //double[,] temparray = matrixChannelsDivisionDouble.ToArray();
            //ExcelDumper xlsdumper = new ExcelDumper("C:\\_gulevlab\\SkyImagesAnalyzer\\app01_data\\Resize\\Alternate\\2005_I.jpg");
            //xlsdumper.doubleArrayToDump = temparray;
            //xlsdumper.DumpData();
            //temparray = null;
            //xlsdumper.Close();
            #endregion

            dmSkyIndexData = (DenseMatrix)aoi.dmRes.Clone();
            aoi.Dispose();
            Image<Bgr, Byte> previewImage = ImageProcessing.evalResultColoredWithFixedDataBounds(dmSkyIndexData, maskImage, skyCloudColorScheme, dmSkyIndexData.Values.Min(), dmSkyIndexData.Values.Max());
            cloudCounter = previewImage.CountNonzero()[1];
            skyCounter = maskImage.CountNonzero()[0] - cloudCounter;
            CloudCover = (double)cloudCounter / (double)(skyCounter + cloudCounter);

            localPreviewBitmap = previewImage.Bitmap;
        }
        #endregion классификация методом из японских публикаций



        #region классификация методом из американской публикации
        private void ClassifyUS()
        {
            DenseMatrix denseMatrixRedChannel;
            DenseMatrix denseMatrixBlueChannel;


            if (cloudSkySeparationValue == 0.0)
            {
                //ThreadSafeOperations.SetTextTB(tbLog, "Не установлена граница SI между небом и облаком для проведения классификации." + Environment.NewLine, true);
                throw new Exception("Не установлено значение границы небо-облако по значению SkyIndex.");
            }


            Image<Gray, Byte> imageBlueChannelByte = LocalProcessingImage[0].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[0];
            Image<Gray, Byte> imageRedChannelByte = LocalProcessingImage[2].Copy(); //new Image<Bgr, Byte>(LocalProcessingBitmap)[2];


            //Image<Gray, Byte> maskImage = ImageProcessing.getImageSignificantMask(LocalProcessingBitmap);
            ImageProcessing imgP = new ImageProcessing(LocalProcessingImage, true);
            imgP.getImageSignificantMask();
            //maskImage = imp.significantMaskImageBinary;
            maskImage = imgP.significantMaskImageCircled;

            imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage);
            imageRedChannelByte = imageRedChannelByte.Mul(maskImage);
            denseMatrixRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
            denseMatrixBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
            ServiceTools.FlushMemory();

            String evalExpression = "R/B";
            ArithmeticsOnImages aoi = new ArithmeticsOnImages();
            aoi.dmR = denseMatrixRedChannel;
            aoi.dmB = denseMatrixBlueChannel;
            aoi.dmG = denseMatrixRedChannel;
            aoi.ExprString = evalExpression;
            aoi.RPNeval();
            //ColorScheme skyCloudColorScheme = ColorScheme.BinaryCloudSkyColorScheme(cloudSkySeparationValue, aoi.dmRes.Values.Min(), aoi.dmRes.Values.Max());
            ColorScheme skyCloudColorScheme = ColorScheme.InversedBinaryCloudSkyColorScheme(cloudSkySeparationValue, aoi.dmRes.Values.Min(), aoi.dmRes.Values.Max());
            ColorSchemeRuler skyCloudRuler = new ColorSchemeRuler(skyCloudColorScheme, aoi.dmRes.Values.Min(), aoi.dmRes.Values.Max());


            dmSkyIndexData = (DenseMatrix)aoi.dmRes.Clone();
            aoi.Dispose();
            Image<Bgr, Byte> previewImage = ImageProcessing.evalResultColoredWithFixedDataBounds(dmSkyIndexData, maskImage, skyCloudColorScheme, dmSkyIndexData.Values.Min(), dmSkyIndexData.Values.Max());
            cloudCounter = previewImage.CountNonzero()[1];
            skyCounter = maskImage.CountNonzero()[0] - cloudCounter;
            CloudCover = (double)cloudCounter / (double)(skyCounter + cloudCounter);

            localPreviewBitmap = previewImage.Bitmap;
        }
        #endregion классификация методом из американской публикации










        private void BGWorkerReport(string message = "")
        {
            if (selfWorker != null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, message + Environment.NewLine, true);
            }
        }




        /// <summary>
        /// Classifies using our Grayness Index scheme
        /// </summary>
        /// <exception cref="System.Exception">Не установлено значение границы небо-облако по значению SkyIndex.</exception>
        private void ClassifyGrIx()
        {
            DenseMatrix dmRedChannel;
            DenseMatrix dmBlueChannel;
            DenseMatrix dmGreenChannel;

            verbosityLevel = Convert.ToInt32(defaultProperties["GrIxProcessingVerbosityLevel"]);


            if (cloudSkySeparationValue == 0.0)
            {
                BGWorkerReport("Не установлена граница SI между небом и облаком для проведения классификации.");
                throw new Exception("Не установлено значение границы небо-облако по значению SkyIndex.");
            }


            Image<Gray, Byte> imageBlueChannelByte = LocalProcessingImage[0].Copy(); //new Image<Bgr, Byte>(LocalProcessingBitmap)[0];
            Image<Gray, Byte> imageGreenChannelByte = LocalProcessingImage[1].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[1];
            Image<Gray, Byte> imageRedChannelByte = LocalProcessingImage[2].Copy(); //new Image<Bgr, Byte>(LocalProcessingBitmap)[2];

            BGWorkerReport("Начало обработки изображения");

            //Image<Gray, Byte> maskImage = ImageProcessing.getImageSignificantMask(LocalProcessingBitmap);


            RoundData predefinedRoundedMask = null;
            if (lImagesRoundMasksMappingFiles != null)
            {
                if (lImagesRoundMasksMappingFiles.Any())
                {
                    if (lImagesRoundMasksMappingFiles.Find(tpl => (new WildcardPattern(tpl.Item1)).IsMatch(sourceImageFileName)) != null)
                    {
                        string strFoundPredefinedRoundedMaskParametersXMLfile =
                            lImagesRoundMasksMappingFiles.Find(
                                tpl => (new WildcardPattern(tpl.Item1)).IsMatch(sourceImageFileName)).Item2;
                        predefinedRoundedMask =
                            ServiceTools.ReadObjectFromXML(strFoundPredefinedRoundedMaskParametersXMLfile,
                                typeof(RoundData)) as RoundData;
                    }
                }
            }


            ImageProcessing imgP = null;
            if (predefinedRoundedMask != null)
            {
                imgP = new ImageProcessing(LocalProcessingImage, predefinedRoundedMask);
            }
            else
            {
                imgP = new ImageProcessing(LocalProcessingImage, true);
            }





            //imgP.getImageSignificantMask();
            //maskImage = imgP.significantMaskImageBinary * imgP.significantMaskImageCircled;
            maskImage = imgP.significantMaskImageCircled;
            Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(theImageCircleCropFactor * 100.0d);
            PointD imageCircleCenter = imgP.imageRD.pointDCircleCenter();
            double imageCircleRadius = imgP.imageRD.DRadius;

            imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage);
            imageRedChannelByte = imageRedChannelByte.Mul(maskImage);
            imageGreenChannelByte = imageGreenChannelByte.Mul(maskImage);
            dmRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
            dmBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
            dmGreenChannel = ImageProcessing.DenseMatrixFromImage(imageGreenChannelByte);
            ServiceTools.FlushMemory();


            randomFileName = "m" + Path.GetRandomFileName().Replace(".", "");


            string currentDirectory = "";
            if (defaultOutputDataDirectory == "")
            {
                currentDirectory = Directory.GetCurrentDirectory() + "\\";
            }
            else
            {
                currentDirectory = defaultOutputDataDirectory;
            }



            string formulaString = "grix";
            //string formulaString = "1.0 - (1.0 + ((B-R)/(B+R)))/2.0";
            DenseMatrix dmProcessingData = (DenseMatrix)imgP.eval(formulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
            DenseMatrix dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);
            DenseMatrix dmMaskCircled = ImageProcessing.DenseMatrixFromImage(maskImageCircled);
            dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMask);
            //ImageConditionAndDataRepresentingForm originalDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmProcessingData, "original 1-sigma/Y data");
            if (verbosityLevel > 0)
            {
                dmMask.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_MaskMatrix.nc");
            }



            #region определим CloudCover без подавления солнца - для сравнения
            //ColorScheme skyCloudColorSchemeWithoutSunSuppression = ColorScheme.InversedBinaryCloudSkyColorScheme(theStdDevMarginValueDefiningSkyCloudSeparation, 0.0d, 1.0d);
            ColorScheme skyCloudColorSchemeWithoutSunSuppression = ColorScheme.InversedBinaryCloudSkyColorScheme(theStdDevMarginValueDefiningSkyCloudSeparation, 0.0d, 1.0d);
            Image<Bgr, Byte> previewImageWithoutSunSuppression = ImageProcessing.evalResultColoredWithFixedDataBounds(dmProcessingData, maskImage, skyCloudColorSchemeWithoutSunSuppression, 0.0d, 1.0d);

            int cloudCounterWithoutSunSuppression = 0;
            try
            {
                cloudCounterWithoutSunSuppression = previewImageWithoutSunSuppression.CountNonzero()[1];
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, ex.Message);
            }

            int skyCounterWithoutSunSuppression = maskImage.CountNonzero()[0] - cloudCounterWithoutSunSuppression;
            double CloudCoverWithoutSunSuppression = (double)cloudCounterWithoutSunSuppression / (double)(skyCounterWithoutSunSuppression + cloudCounterWithoutSunSuppression);
            //сохраним результат без подавления засветки
            //потом еще надо скомпоновать результат-композит
            if (verbosityLevel > 1)
            {
                previewImageWithoutSunSuppression.Save(currentDirectory + randomFileName + "-GrIx-result-withoutSunSuppression.jpg");
            }
            #endregion


            ServiceTools.FlushMemory();



            #region Detect Sun disk condition using precalculated image stats and NN on that predictors

            BGWorkerReport("Оценка состояния солнечного диска на снимке.");

            SunDiskCondition detectedSDC = SunDiskCondition.Defect;

            ConcurrentData nearestConcurrentData = null;
            #region 1. find concurrent data and determine sun elevation

            if (imgConcurrentDataAndStats != null)
            {
                nearestConcurrentData = imgConcurrentDataAndStats.concurrentData;
            }
            else
            {
                bool IncludeGPSandSunAltitudeData =
                    (((string)defaultProperties["IncludeGPSandSunAltitudeData"]).ToLower() == "true");
                string strConcurrentDataXMLfilesDirectory = "";
                if (IncludeGPSandSunAltitudeData)
                {
                    strConcurrentDataXMLfilesDirectory = (string)defaultProperties["ConcurrentDataXMLfilesDirectory"];
                    if (!Directory.Exists(strConcurrentDataXMLfilesDirectory))
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                                    "concurrent data directory is not specified or does not exist." + Environment.NewLine +
                                    "\tSo while checkbox \"include GPS and sun altitude data\" it has been specified and exist." + Environment.NewLine + "" +
                                    "\tPlease make sure you really want to include GPS and sun altitude data or check if the directory path specified and it does exist.");
                        return;
                    }

                }

                List<string> filesListConcurrentData = new List<string>();
                if (IncludeGPSandSunAltitudeData)
                {
                    filesListConcurrentData =
                        new List<string>(Directory.EnumerateFiles(strConcurrentDataXMLfilesDirectory,
                            ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(), SearchOption.AllDirectories));
                }


                #region read concurrent data from XML files

                theLogWindow = ServiceTools.LogAText(theLogWindow, "started concurrent data reading");

                if (IncludeGPSandSunAltitudeData)
                {
                    List<Dictionary<string, object>> lDictionariesConcurrentData = filesListConcurrentData.ConvertAll(
                        strConcDataXMLFile =>
                        {
                            Dictionary<string, object> currDict = ServiceTools.ReadDictionaryFromXML(strConcDataXMLFile);
                            if (currDict != null)
                            {
                                currDict.Add("XMLfileName", Path.GetFileName(strConcDataXMLFile));
                            }
                            return currDict;
                        });

                    lDictionariesConcurrentData.RemoveAll(dict => dict == null);

                    List<ConcurrentData> lConcurrentData =
                        lDictionariesConcurrentData.ConvertAll<ConcurrentData>(dict =>
                        {
                            ConcurrentData retVal = null;
                            try
                            {
                                retVal = new ConcurrentData(dict);
                            }
                            catch (Exception ex)
                            {
                                retVal = null;
                            }
                            return retVal;
                        });
                    lConcurrentData.RemoveAll(val => val == null);


                    // map obtained concurrent data to image by its datetime
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "started concurrent data mapping");

                    string currImgFilename = sourceImageFileName;
                    currImgFilename = Path.GetFileNameWithoutExtension(currImgFilename);
                    string ptrn = @"(devID\d)";
                    Regex rgxp = new Regex(ptrn, RegexOptions.IgnoreCase);
                    string strCurrImgDT = rgxp.Replace(currImgFilename.Substring(4), "");
                    //2015-12-16T06-01-38
                    strCurrImgDT = strCurrImgDT.Substring(0, 11) + strCurrImgDT.Substring(11).Replace("-", ":");

                    DateTime currImgDT = DateTime.Parse(strCurrImgDT, null,
                        DateTimeStyles.AdjustToUniversal);

                    nearestConcurrentData = lConcurrentData.Aggregate((cDt1, cDt2) =>
                    {
                        TimeSpan tspan1 = new TimeSpan(Math.Abs((cDt1.datetimeUTC - currImgDT).Ticks));
                        TimeSpan tspan2 = new TimeSpan(Math.Abs((cDt2.datetimeUTC - currImgDT).Ticks));
                        return ((tspan1 <= tspan2) ? (cDt1) : (cDt2));
                    });
                }
                #endregion read concurrent data from XML files
            }


            #endregion



            SkyImageIndexesStatsData currImageStatsData = null;
            #region 2. find precalculated stats data

            if (imgConcurrentDataAndStats != null)
            {
                currImageStatsData = imgConcurrentDataAndStats.grixyrgbStats;
            }
            else
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "started GrIxYRGB stats data reading");

                if (defaultProperties.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
                {
                    string imageYRGBstatsXMLdataFilesDirectory =
                        (string)defaultProperties["imageYRGBstatsXMLdataFilesDirectory"];

                    if (Directory.Exists(imageYRGBstatsXMLdataFilesDirectory))
                    {
                        if (Directory.Exists(imageYRGBstatsXMLdataFilesDirectory))
                        {
                            List<string> foundXMLfiles = Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory,
                                ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(sourceImageFileName, "", false),
                                SearchOption.AllDirectories).ToList();
                            if (foundXMLfiles.Any())
                            {
                                // возьмем первый попавшийся
                                currImageStatsData =
                                    (SkyImageIndexesStatsData)
                                        ServiceTools.ReadObjectFromXML(foundXMLfiles[0], typeof(SkyImageIndexesStatsData));
                            }
                        }
                    }
                }

                #region calculate stats if needed

                if (currImageStatsData == null)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "obtaining stats data for image...");

                    Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
                    optionalParameters.Add("ImagesRoundMasksXMLfilesMappingList",
                        (string)defaultProperties["ImagesRoundMasksXMLfilesMappingList"]);

                    ImageStatsDataCalculationResult currImageProcessingResult =
                        ImageProcessing.CalculateImageStatsData(sourceImageFileName, optionalParameters);

                    if (currImageProcessingResult.calcResult)
                    {
                        string currentFullFileName = currImageProcessingResult.imgFilename;

                        if (defaultProperties.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
                        {
                            string imageYRGBstatsXMLdataFilesDirectory =
                                (string)defaultProperties["imageYRGBstatsXMLdataFilesDirectory"];

                            if (Directory.Exists(imageYRGBstatsXMLdataFilesDirectory))
                            {
                                string strImageGrIxYRGBDataFileName =
                                    ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currentFullFileName,
                                        imageYRGBstatsXMLdataFilesDirectory);
                                ServiceTools.WriteObjectToXML(currImageProcessingResult.grixyrgbStatsData,
                                    strImageGrIxYRGBDataFileName);
                            }
                        }

                        currImageStatsData = currImageProcessingResult.grixyrgbStatsData;
                    }
                }

                #endregion calculate stats if needed
            }
            #endregion


            #region combine found data to get predictors vector
            string currImageALLstatsDataCSVWithConcurrentData = currImageStatsData.ToCSV() + "," +
                nearestConcurrentData.gps.SunZenithAzimuth().ElevationAngle.ToString().Replace(",", ".") + "," +
                nearestConcurrentData.gps.SunZenithAzimuth().Azimuth.ToString().Replace(",", ".");
            string csvHeader = currImageStatsData.CSVHeader() + ",SunElevationDeg,SunAzimuthDeg,sunDiskCondition";
            #endregion


            #region PREDICT SDC

            List<string> lCalculatedData = new List<string>();
            lCalculatedData.Add(currImageALLstatsDataCSVWithConcurrentData);


            #region predict using LIST of data

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



            List<List<string>> csvFileContentStringsFiltered_wo_sdc = csvFileContentStringsFiltered;

            List<DenseVector> lDV_objects_features =
                csvFileContentStringsFiltered_wo_sdc.ConvertAll(
                    list =>
                        DenseVector.OfEnumerable(list.ConvertAll<double>(str => Convert.ToDouble(str.Replace(".", ",")))));


            DenseVector dvMeans = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV((string)defaultProperties["NormMeansFile"], 0, ",")).Row(0);
            DenseVector dvRanges = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV((string)defaultProperties["NormRangeFile"], 0, ", ")).Row(0);

            lDV_objects_features = lDV_objects_features.ConvertAll(dv =>
            {
                DenseVector dvShifted = dv - dvMeans;
                DenseVector dvNormed = (DenseVector)dvShifted.PointwiseDivide(dvRanges);
                return dvNormed;
            });

            DenseMatrix dmObjectsFeatures = DenseMatrix.OfRowVectors(lDV_objects_features);

            DenseVector dvThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV((string)defaultProperties["NNtrainedParametersFile"], 0, ", ");
            List<int> NNlayersConfig =
                new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV((string)defaultProperties["NNconfigFile"], 0, ", ")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));


            List<List<double>> lDecisionProbabilities = null;

            List<SunDiskCondition> predictedSDClist =
                NNclassificatorPredictor<SunDiskCondition>.NNpredict(dmObjectsFeatures, dvThetaValues, NNlayersConfig,
                    out lDecisionProbabilities, SunDiskConditionData.MatlabEnumeratedSDCorderedList()).ToList();

            //List<SunDiskCondition> predictedSDClist = predictedSDC.ConvertAll(sdcInt =>
            //{
            //    switch (sdcInt)
            //    {
            //        case 4:
            //            return SunDiskCondition.NoSun;
            //            break;
            //        case 1:
            //            return SunDiskCondition.Sun0;
            //            break;
            //        case 2:
            //            return SunDiskCondition.Sun1;
            //            break;
            //        case 3:
            //            return SunDiskCondition.Sun2;
            //            break;
            //        default:
            //            return SunDiskCondition.Defect;
            //    }
            //});

            detectedSDC = predictedSDClist[0];

            #endregion

            #endregion

            #endregion Detect Sun disk condition using precalculated image stats and NN on that predictors



            #region detect Sun disk location

            BGWorkerReport("поиск солнечного диска, оценка применимости подавления солнечной засветки");

            //RoundData sunRoundData = detectSun(dmProcessingData, maskImageCircled, maskImage, imgP, currentDirectory, randomFileName);

            if (sourceImageFileName != "")
            {
                if (File.Exists(sourceImageFileName))
                {
                    FileInfo fInfoSourceFile = new FileInfo(sourceImageFileName);

                }
            }



            RoundData sunRoundData = RoundData.nullRoundData();

            if ((detectedSDC == SunDiskCondition.NoSun) || (detectedSDC == SunDiskCondition.Sun0))
            {
                sunRoundData = RoundData.nullRoundData();
            }
            else
            {
                bool predefinedSunDiskLocationWasUsed = false;
                if (sourceImageFileName != "")
                {
                    //посмотрим, нет ли уже имеющихся данных о положении и размере солнечного диска на изображении
                    // FileInfo fInfo1 = new FileInfo(sourceImageFileName);
                    //string sunDiskInfoFileName = fInfo1.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(sourceImageFileName) +
                    //                             "-SunDiskInfo.xml";

                    string sunDiskInfoFileName = ConventionalTransitions.SunDiskInfoFileName(sourceImageFileName,
                        defaultOutputDataDirectory);

                    RoundData existingRoundData = RoundData.nullRoundData();
                    Size imgSizeUnderExistingRoundData = LocalProcessingImage.Bitmap.Size;
                    object existingRoundDataObj = ServiceTools.ReadObjectFromXML(sunDiskInfoFileName, typeof(RoundDataWithUnderlyingImgSize));

                    if (existingRoundDataObj != null)
                    {
                        existingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).circle;
                        imgSizeUnderExistingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).imgSize;
                        predefinedSunDiskLocationWasUsed = true;
                    }

                    double currScale = (double)LocalProcessingImage.Width / (double)imgSizeUnderExistingRoundData.Width;
                    if (currScale != 1.0d)
                    {
                        existingRoundData.DCenterX *= currScale;
                        existingRoundData.DCenterY *= currScale;
                        existingRoundData.DRadius *= currScale;
                    }
                    if (!existingRoundData.IsNull)
                    {
                        sunRoundData = existingRoundData;
                    }
                }


                if (sunRoundData.IsNull)
                {
                    if (!forceExistingSunInformation)
                    {
                        sunRoundData = DetectSunWithSerieOfArcs(imgP, dmProcessingData, currentDirectory, randomFileName);
                    }
                    else
                    {
                        sunRoundData = RoundData.nullRoundData();
                    }

                }
            }

            theSunSuppressionSchemeApplicable = (!sunRoundData.IsNull);


            BGWorkerReport("закончен поиск солнечного диска. применимость подавления засветки: " + theSunSuppressionSchemeApplicable.ToString());



            #region сохраним данные положения солнечного диска

            if (!sunRoundData.IsNull)
            {
                BGWorkerReport("расположение солнечного диска: " + Environment.NewLine + sunRoundData.ToString());

                if (verbosityLevel > 0)
                {
                    RoundDataWithUnderlyingImgSize sunDiskDataToSave = new RoundDataWithUnderlyingImgSize();
                    sunDiskDataToSave.circle = sunRoundData;
                    sunDiskDataToSave.imgSize = new Size(imgP.tmpImage.Width, imgP.tmpImage.Height);

                    ServiceTools.WriteObjectToXML(sunDiskDataToSave,
                        currentDirectory + randomFileName + "_SunDiskDataRelative.xml");

                    RoundDataWithUnderlyingImgSize imageRDToSave = new RoundDataWithUnderlyingImgSize();
                    imageRDToSave.circle = imgP.imageRD;
                    imageRDToSave.imgSize = new Size(imgP.tmpImage.Width, imgP.tmpImage.Height);
                    ServiceTools.WriteObjectToXML(imageRDToSave,
                        currentDirectory + randomFileName + "_DetectedImageRelative.xml");
                }

                #region replaced with XML serialization
                //string strGeometryData = "" + imgP.imageRD.DCenterX.ToString("e").Replace(",", ".") +
                //                         Environment.NewLine +
                //                         imgP.imageRD.DCenterY.ToString("e").Replace(",", ".") + Environment.NewLine +
                //                         imgP.imageRD.DRadius.ToString("e").Replace(",", ".") + Environment.NewLine +
                //                         sunRoundData.DCenterX.ToString("e").Replace(",", ".") + Environment.NewLine +
                //                         sunRoundData.DCenterY.ToString("e").Replace(",", ".") + Environment.NewLine +
                //                         sunRoundData.DRadius.ToString("e").Replace(",", ".") + Environment.NewLine;
                //if (verbosityLevel > 0)
                //{
                //    ServiceTools.logToTextFile(currentDirectory + randomFileName + "_GeometryData.csv", strGeometryData,
                //        true);
                //}
                #endregion replaced with XML serialization

                if (sourceImageFileName != "")
                {
                    // посмотрим, нет ли уже имеющихся данных о положении и размере солнечного диска на изображении
                    // если есть - не будем перезаписывать
                    // FileInfo fInfo1 = new FileInfo(sourceImageFileName);
                    string sunDiskInfoFileName = ConventionalTransitions.SunDiskInfoFileName(sourceImageFileName,
                        defaultOutputDataDirectory);
                    //string sunDiskInfoFileName = fInfo1.DirectoryName + "\\" +
                    //                             Path.GetFileNameWithoutExtension(sourceImageFileName) +
                    //                             "-SunDiskInfo.xml";
                    if (!File.Exists(sunDiskInfoFileName))
                    {
                        RoundDataWithUnderlyingImgSize foundSunLocation = new RoundDataWithUnderlyingImgSize();
                        foundSunLocation.circle = sunRoundData;
                        foundSunLocation.imgSize = LocalProcessingImage.Size;
                        ServiceTools.WriteObjectToXML(foundSunLocation, sunDiskInfoFileName);
                    }
                }
            }
            #endregion сохраним данные положения солнечного диска


            #endregion detect Sun disk location



            while (theSunSuppressionSchemeApplicable)
            {
                PointD sunCenterPoint = sunRoundData.pointDCircleCenter();
                double sunRadius = sunRoundData.DRadius;

                Image<Bgr, Byte> imageSunDemonstration = ImageProcessing.evalResultColored(dmProcessingData, maskImage,
                    new ColorScheme(""));
                Image<Bgr, Byte> calculatedSunMaskImage = imageSunDemonstration.CopyBlank();
                calculatedSunMaskImage.Draw(new CircleF(new PointF((float)sunCenterPoint.X, (float)sunCenterPoint.Y), (float)sunRadius), new Bgr(255, 255, 255), 0);
                imageSunDemonstration = imageSunDemonstration.AddWeighted(calculatedSunMaskImage, 0.7, 0.3, 0.0);

                if (!isCalculatingUsingBgWorker)
                {
                    ServiceTools.ShowPicture(imageSunDemonstration, "");
                }

                BGWorkerReport("анализ и компенсация засветки");

                #region анализ засветки и компенсация

                #region // unused
                //DenseMatrix theSkyWeightAbs = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                //    new Func<int, int, double>(
                //        (row, column) =>
                //        {
                //            if (dmProcessingData[row, column] < theStdDevMarginValueDefiningTrueSkyArea)
                //                return 1 - dmProcessingData[row, column];
                //            else return 0.0d;
                //        }));
                //theSkyWeightAbs = (DenseMatrix)theSkyWeightAbs.PointwiseMultiply(dmMask);
                #endregion // unused

                DenseMatrix dmSunburnProfileDetection = (DenseMatrix)dmProcessingData.Clone();

                //для фильтрации краевых эффектов срежем по кругу радиусом 0.9 номинально определенного
                dmSunburnProfileDetection = (DenseMatrix)dmSunburnProfileDetection.PointwiseMultiply(dmMask);

                if (verbosityLevel > 0)
                {
                    dmSunburnProfileDetection.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                   "_dmSunburnProfileDetection-cartesian.nc");
                }


                int angleBinsCount = 180;
                /// todo: регулировать размер сетки для свертки по гауссу
                DenseMatrix dmPolarSystemGrIxDistribution = DataAnalysisStatic.CartesianToPolar(dmSunburnProfileDetection,
                    sunCenterPoint, dmMask, angleBinsCount);
                dmPolarSystemGrIxDistribution.MapIndexedInplace(
                    (r, c, dVal) => ((c > sunRadius) && (dVal == 1.0d)) ? (0.0d) : (dVal));

                if (verbosityLevel > 0)
                {
                    dmPolarSystemGrIxDistribution.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                       "_dmPolarSystemGrIxDistribution.nc");
                }

                List<Point3D> lGrIxValuesInPolar = new List<Point3D>();
                foreach (Tuple<int, Vector<double>> tplRow in dmPolarSystemGrIxDistribution.EnumerateRowsIndexed())
                {
                    // rows - angle bins
                    // columns - radii bins
                    // values - GrIx values
                    foreach (Tuple<int, double> tplCol in tplRow.Item2.EnumerateIndexed())
                    {
                        if (tplCol.Item2 > 0.0d)
                        {
                            lGrIxValuesInPolar.Add(new Point3D(tplRow.Item1 * 2.0d * Math.PI / angleBinsCount, tplCol.Item1,
                                tplCol.Item2));
                            // 3D-point: (phi, r, value)
                        }
                    }
                }
                lGrIxValuesInPolar.Sort((pt1, pt2) => (pt1.Y < pt2.Y) ? (-1) : (1));
                List<Point3D> lGrIxValuesInPolarToSunMargin =
                    lGrIxValuesInPolar.ConvertAll(pt3D => new Point3D(pt3D.X, pt3D.Y - sunRadius, pt3D.Z));
                lGrIxValuesInPolarToSunMargin.RemoveAll(pt3D => pt3D.Y <= 0.0d);


                #region // unused
                //double theSkyWeightSum = theSkyWeightAbs.Values.Sum();
                //DenseMatrix theSkyWeight = (DenseMatrix)(theSkyWeightAbs.Copy()/theSkyWeightSum);
                ////theSkyWeight.MapInplace(new Func<double, double>(val => val / theSkyWeightSum));
                #endregion // unused

                #region // obsolete
                //DenseMatrix dmDistanceToSunMargin = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                //    new Func<int, int, double>(
                //        (row, column) =>
                //        {
                //            double dx = (double)column - sunCenterPoint.X;
                //            double dy = (double)row - sunCenterPoint.Y;
                //            double r = Math.Sqrt(dx * dx + dy * dy) - sunRadius;
                //            r = (r < 0.0d) ? (0.0d) : (r);
                //            return r;
                //        }));
                //dmDistanceToSunMargin = (DenseMatrix)dmDistanceToSunMargin.PointwiseMultiply(dmMask);
                #endregion // obsolete

                #region // unused
                //DenseMatrix dmDistanceToSunCenter = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                //    new Func<int, int, double>(
                //        (row, column) =>
                //        {
                //            double dx = (double)column - sunCenterPoint.X;
                //            double dy = (double)row - sunCenterPoint.Y;
                //            double r = Math.Sqrt(dx * dx + dy * dy);
                //            return r;
                //        }));
                #endregion // unused

                #region // obsolete
                //DenseMatrix dmAngleAroundTheSunburn = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                //    new Func<int, int, double>(
                //        (row, column) =>
                //        {
                //            double dx = (double)column - sunCenterPoint.X;
                //            double dy = (double)row - sunCenterPoint.Y;
                //            double r = dmDistanceToSunMargin[row, column] + sunRadius;
                //            double cosPhi = dx / r;
                //            double phi = Math.Acos(cosPhi);
                //            if (dy > 0) phi = 2.0d * Math.PI - phi;
                //            if (double.IsNaN(phi)) phi = 0.0d;
                //            return phi;
                //        }));
                //dmAngleAroundTheSunburn = (DenseMatrix)dmAngleAroundTheSunburn.PointwiseMultiply(dmMask);
                #endregion // obsolete

                //double rMax = dmDistanceToSunMargin.Values.Max();
                double rMax = lGrIxValuesInPolarToSunMargin.Max(pt3D => pt3D.Y);
                //это расстояние уже в pixels, ничего пересчитывать не надо
                double minDataValue = 1.0d;
                double Rs = 0.0d;
                List<double> envelopValues = new List<double>();
                List<double> envelopRargument = new List<double>();


                #region здесь фильтруем шумы и выбросы ниже основной очевидной огибающей

                IEnumerable<IGrouping<int, Point3D>> lCurrDistancesPointsGroups =
                    lGrIxValuesInPolarToSunMargin.GroupBy(pt3D => Convert.ToInt32(pt3D.Y));
                foreach (IGrouping<int, Point3D> currDistancesPointsGroup in lCurrDistancesPointsGroups)
                {
                    if (currDistancesPointsGroup.Key > rMax)
                    {
                        continue;
                    }
                    List<Point3D> lCurrDistancePoints = new List<Point3D>(currDistancesPointsGroup);
                    List<double> lCurrDistanceGrIxValues = lCurrDistancePoints.ConvertAll(pt3D => pt3D.Z);
                    double curMedian = Statistics.Median(lCurrDistanceGrIxValues);
                    double dataPercentile10 = Statistics.Percentile(lCurrDistanceGrIxValues, 10);

                    List<double> listTempStatData = lCurrDistanceGrIxValues.FindAll(x => x >= dataPercentile10);
                    if (listTempStatData.Count > 0)
                    {
                        envelopValues.Add(listTempStatData.Minimum());
                        envelopRargument.Add((double)currDistancesPointsGroup.Key);
                    }

                }

                #region // obsolete
                //for (int r = 0; r < rMax; r++)
                //{
                //    double rMinlocal = (double)r;
                //    double rMaxLocal = (double)(r + 1);
                //
                //
                //    DenseMatrix dmTemporaryProcessingDataMatrix = DenseMatrix.Create(dmSunburnProfileDetection.RowCount, dmSunburnProfileDetection.ColumnCount,
                //        new Func<int, int, double>((row, column) =>
                //        {
                //            double currDist = dmDistanceToSunMargin[row, column];
                //            if ((currDist >= rMinlocal) && (currDist < rMaxLocal)) return dmSunburnProfileDetection[row, column];
                //            else return 0.0d;
                //        }));
                //
                //    DenseVector dvCurrentDataExcludingZero =
                //        DataAnalysis.DataVectorizedExcludingValues(dmTemporaryProcessingDataMatrix, 0.0d);
                //    double curMedian = 0.0d;
                //    DescriptiveStatistics statTempData = DataAnalysis.StatsOfDataExcludingValues(dmTemporaryProcessingDataMatrix, 0.0d, out curMedian);
                //    if (statTempData == null) continue;
                //
                //
                //    double dataPercentile10 = Statistics.Percentile(dvCurrentDataExcludingZero, 10);
                //
                //    DenseVector listTempStatData =
                //        DataAnalysis.DataVectorizedWithCondition(dmTemporaryProcessingDataMatrix,
                //            (x => x >= dataPercentile10));
                //
                //    double dataMin = 1.0d;
                //    if (listTempStatData != null) dataMin = listTempStatData.Min();
                //    else continue;
                //
                //    envelopValues.Add(dataMin);
                //    envelopRargument.Add((double)r);
                //}
                #endregion // obsolete
                #endregion здесь фильтруем шумы и выбросы ниже основной очевидной огибающей


                // получили общую огибающую - "минимальное" значение GrIx от r, расстояния от края солнечного диска
                // "минимальное" в кавычках - потому что это не абсолютный минимум на указанном интерваке (r;r+1),
                // а минимум при условии исключения шумов и выбросов за пределами 10-го персентиля


                #region // здесь фильтруем данные выше значения GrIx=theStdDevMarginValueDefiningTrueSkyArea
                // Попробуем отфильтровать данные, чтобы брать во внимание только те, которые лежат ниже границы
                // которую определим как значение, ниже которого - точно синее небо

                //List<Tuple<double, double>> lEnvelopRGrIxvalues =
                //    new List<Tuple<double, double>>(
                //        envelopRargument.Zip<double, double, Tuple<double, double>>(envelopValues,
                //            (r, grix) => new Tuple<double, double>(r, grix)));
                //lEnvelopRGrIxvalues =
                //    lEnvelopRGrIxvalues.FindAll(tpl => (tpl.Item2 <= theStdDevMarginValueDefiningTrueSkyArea));
                //envelopValues = lEnvelopRGrIxvalues.ConvertAll<double>(tpl => tpl.Item2);
                //envelopRargument = lEnvelopRGrIxvalues.ConvertAll<double>(tpl => tpl.Item1);

                #endregion // здесь фильтруем данные выше значения GrIx=theStdDevMarginValueDefiningTrueSkyArea






                #region аппроксимация распределения GrIx(r) для огибающей

                BGWorkerReport("сглаживаем полученное распределение, представляющее огибающую GrIx(r)," +
                               "определеяем характерные точки: расположение глобального минимума, его величину");

                DenseVector dvDataValues = DenseVector.OfEnumerable(envelopValues);

                #region //obsolete
                //DenseVector dvSmoothedDistribution = DataAnalysis.ExponentialMovingAverage(dvDataValues, 10, 0.4d);

                // DenseVector dvSmoothedEnvelopValues = dvDataValues.Conv(StandardConvolutionKernels.gauss, 10);

                // будем аппроксимировать по-другому - нужно учитывать веса точек.
                // чем меньше значение GrIx в точке, - тем больше ее вес.
                // аппроксимировать будем уже не решением СЛУ, а, как и в остальных случаях, - методом градиентного спуска
                // для этого функция будет задаваться здесь же.
                // и начальные коэффициенты тоже.
                // данные пока оставим отфильтрованные по значению GrIx
                #endregion //obsolete


                minDataValue = dvDataValues.AbsoluteMinimum();
                Rs = envelopRargument.ElementAt(dvDataValues.AbsoluteMinimumIndex());

                DenseVector dvDataSpace = DenseVector.OfEnumerable(envelopRargument);


                #region //добавим фиксированную точку в начало - на край солнечного диска
                //добавим фиксированную точку в начало - на край солнечного диска
                // для аппроксимации в этой паре массивов она может быть произвольной
                // главное, чтобы был под нее "номер" в последовательности точек
                //List<double> tmpList = new List<double>(dvDataSpace);
                //tmpList.Insert(0, 0.0d);
                //dvDataSpace = DenseVector.OfEnumerable(tmpList);
                //tmpList = new List<double>(dvDataValues);
                //tmpList.Insert(0, minSunburnGrIxValue);
                //dvDataValues = DenseVector.OfEnumerable(tmpList);
                #endregion //добавим фиксированную точку в начало - на край солнечного диска

                #region // obsolete
                // у аппроксиматора надо задать некоторые начальне значения, чтобы ему было от чего скакать
                // например, начальные значения коэффициентов полинома. Пусть это будет просто прямая y=1-x
                // а порядок полинома будет задаваться количеством этих коэффициентов - то есть, длиной вектора коэффициентов
                // int polynomeOrder = 2;

                // оценим начальные значения из аппроксимации без весов - по старой версии
                // методом МНК, который для полинома дает всего лишь решение СЛУ

                // int polynomeOrder = 10;
                // int polynomeOrder = 6;
                //int polynomeOrder = 4;

                // укажем, что есть фиксированная точка - в отдельном массиве фиксированных точек
                //DenseVector dvFixedPoints = DenseVector.Create(dvDataSpace.Count,
                //    new Func<int, double>(i => (i == 0) ? (minSunburnGrIxValue) : (double.NaN)));

                //DenseVector dvInitialParameters_EvenlopApprox =
                //    DataAnalysis.NPolynomeApproximationLessSquareMethod(dvDataValues,
                //                                                        dvDataSpace,
                //                                                        dvFixedPoints,
                //                                                        polynomeOrder);

                //List<double> initParametersList_EvenlopApprox = new List<double>();
                //initParametersList_EvenlopApprox.Add(1.0d);
                //initParametersList_EvenlopApprox.Add(-1.0d);
                //for (int i = 0; i < polynomeOrder-1; i++)
                //{
                //    initParametersList_EvenlopApprox.Add(0.01d);
                //}
                //DenseVector dvInitialParameters_EvenlopApprox = DenseVector.OfEnumerable(initParametersList_EvenlopApprox);
                //DenseVector initialParametersIncrement_EvenlopApprox =
                //    DenseVector.Create( dvInitialParameters_EvenlopApprox.Count,
                //                        i => dvInitialParameters_EvenlopApprox[i]*0.1);
                #endregion // obsolete

                // для того, чтобы в точке r=Rs/3 вес был 1/10
                // решим уравнение
                double l_w = 0.7d; // расстояние от нуля в долях Rs, где вес должен быть уже равным m_w
                double m_w = 0.1d;
                double k_w = -Math.Log(m_w) / (Rs * Rs * (l_w - 1.0d) * (l_w - 1.0d));

                #region // попробуем учесть только точки в пределах 0.3Rs от глобального минимума
                // попробуем учесть только точки в пределах 0.3Rs от глобального минимума
                //double rsRateToGetWeightsZeroedOutside = 0.3d;
                //double kForWeightDependency = 1.0d / (Rs * rsRateToGetWeightsZeroedOutside);
                #endregion // попробуем учесть только точки в пределах 0.3Rs от глобального минимума

                DenseVector dvWeights_EvenlopApprox = DenseVector.Create(dvDataValues.Count, (i =>
                {
                    double x = dvDataSpace[i];
                    double weight = m_w + Math.Exp(-k_w * Math.Pow(x - Rs, 2.0d));

                    return weight;
                }));
                double dvWeights_EvenlopApprox_totalWeight = dvWeights_EvenlopApprox.Sum();
                dvWeights_EvenlopApprox /= dvWeights_EvenlopApprox_totalWeight;



                if (verbosityLevel > 0)
                {
                    foreach (var dataValueTuple in dvDataValues.EnumerateIndexed())
                    {
                        string textToWrite = "" + dvDataSpace[dataValueTuple.Item1].ToString("e").Replace(",", ".") + ";";
                        textToWrite += dataValueTuple.Item2.ToString().Replace(",", ".") + ";";
                        textToWrite += dvWeights_EvenlopApprox[dataValueTuple.Item1].ToString("e").Replace(",", ".");
                        textToWrite += Environment.NewLine;

                        ServiceTools.logToTextFile(currentDirectory + randomFileName + "_dataDistribution.csv",
                            textToWrite, true);

                    }
                }



                #region // obsolete
                //dvWeights_EvenlopApprox[0] = dvWeights_EvenlopApprox.Sum();

                // устарело. Поставим жесткое условие на параметры
                // чтобы в точке r = 0 значение обязательно равнялось "полностью засвеченному"

                // не работает - решение вообще никуда не движется
                // придется зафиксировать точку вручную - переопределением функции
                //evenlopApproximator.parametersConditions.Add(dvPar =>
                //{
                //    if (theEvemlopApproximationFunction(dvPar, 0.0d) == minSunburnGrIxValue)
                //    {
                //        return true;
                //    }
                //
                //    return false;
                //});


                // Определим, как выглядит функция от аргумента
                //Func<DenseVector, double, double> theEvenlopApproximationFunction =
                //    ((dvPolynomeKoeffs_except0th, dRVal) =>
                //    {
                //        DenseVector dvPolynomeKoeffs =
                //            DenseVector.OfEnumerable(
                //                DenseVector.Create(1, minSunburnGrIxValue).Concat(dvPolynomeKoeffs_except0th));
                //        return DataAnalysis.PolynomeValue(dvPolynomeKoeffs, dRVal);
                //    });
                #endregion

                // Определим, как выглядит функция от аргумента
                // при этом надо еще зафиксировать вторую точку - на правой границе области
                // для этого надо наложить условие на коэффициенты - будет еще минус одно значение
                Func<DenseVector, double, double> theEvenlopApproximationFunction =
                    ((dvPolynomeKoeffs_except0th, dRVal) =>
                    {
                        DenseVector dvPolynomeKoeffs =
                            DenseVector.OfEnumerable(
                                DenseVector.Create(1, minSunburnGrIxValue).Concat(dvPolynomeKoeffs_except0th));
                        return DataAnalysisStatic.PolynomeValue(dvPolynomeKoeffs, dRVal);
                    });



                // создаем сам объект, который будет аппроксимировать
                //GradientDescentApproximator evenlopApproximator = new GradientDescentApproximator(dvDataValues,
                //                                                                                    dvDataSpace,
                //                                                                                    theEvenlopApproximationFunction);
                const int polynomeOrder3 = 3;
                const int polynomeOrder6 = 6;
                NonLinLeastSqProbWoConstraints<double> evenlopApproximator = new NonLinLeastSqProbWoConstraints<double>();
                evenlopApproximator.mSpaceVector = dvDataSpace.Copy();
                evenlopApproximator.mFittingValuesVector = dvDataValues.Copy();
                evenlopApproximator.fittingFunction =
                    (iEnumPolynomeKoeffsExcept0th, dRVal) =>
                        theEvenlopApproximationFunction(DenseVector.OfEnumerable(iEnumPolynomeKoeffsExcept0th), dRVal);

                evenlopApproximator.nXspacePoint = DenseVector.Create(polynomeOrder6, 0.0d);
                DenseVector approxPolyKoeffs6 = DenseVector.OfEnumerable(evenlopApproximator.SolveOptimizationProblem());

                evenlopApproximator.nXspacePoint = DenseVector.Create(polynomeOrder3, 0.0d);
                DenseVector approxPolyKoeffs3 = DenseVector.OfEnumerable(evenlopApproximator.SolveOptimizationProblem());

                // DenseVector approxPolyKoeffs6 = evenlopApproximator.Approximation_ILOptimizer(dvInitialParameters_EvenlopApprox6);
                // DenseVector approxPolyKoeffs6 = dvInitialParameters_EvenlopApprox6.Copy();
                // throw new NotImplementedException("Аппроксимация огибающей временно была отключена - надо восстановить!");


                // для кубической применим веса значений - рассчет см.выше
                // evenlopApproximator.DvWeights = dvWeights_EvenlopApprox;
                // DenseVector approxPolyKoeffs3 = evenlopApproximator.Approximation_ILOptimizer(dvInitialParameters_EvenlopApprox3);
                // DenseVector approxPolyKoeffs3 = dvInitialParameters_EvenlopApprox6.Copy();
                // throw new NotImplementedException("Аппроксимация огибающей временно была отключена - надо восстановить!");

                // добавим в нулевую позицию зафиксированный элемент
                approxPolyKoeffs3 =
                    DenseVector.OfEnumerable(DenseVector.Create(1, minSunburnGrIxValue).Concat(approxPolyKoeffs3));
                approxPolyKoeffs6 =
                    DenseVector.OfEnumerable(DenseVector.Create(1, minSunburnGrIxValue).Concat(approxPolyKoeffs6));



                #region plot evenlop approximation summary

                MultipleScatterAndFunctionsRepresentation weightAndDataPlot =
                    new MultipleScatterAndFunctionsRepresentation(2560, 1600);
                weightAndDataPlot.dvScatterXSpace.Add(dvDataSpace);
                weightAndDataPlot.dvScatterFuncValues.Add(dvDataValues);
                weightAndDataPlot.scatterDrawingVariants.Add(SequencesDrawingVariants.circles);
                weightAndDataPlot.scatterLineColors.Add(new Bgr(Color.Red));

                DenseVector dvWeightsToShow = dvWeights_EvenlopApprox.Copy();
                dvWeightsToShow.MapInplace(dVal => dVal * dvDataValues.Max() / dvWeights_EvenlopApprox.Max());
                weightAndDataPlot.dvScatterXSpace.Add(dvDataSpace);
                weightAndDataPlot.dvScatterFuncValues.Add(dvWeightsToShow);
                weightAndDataPlot.scatterDrawingVariants.Add(SequencesDrawingVariants.squares);
                weightAndDataPlot.scatterLineColors.Add(new Bgr(Color.Magenta));

                weightAndDataPlot.Represent();
                if ((verbosityLevel > 1) && (!isCalculatingUsingBgWorker))
                {
                    ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate ()
                    {
                        ServiceTools.ShowPicture(weightAndDataPlot.TheImage.Bitmap, "Overall GrIx values envelop vs r");
                    });
                }
                if (verbosityLevel > 1)
                {
                    weightAndDataPlot.TheImage.Save(currentDirectory + randomFileName + "-envelop-approximation-result-with-weights.jpg");
                }


                MultipleScatterAndFunctionsRepresentation envelopPlottingForm =
                    new MultipleScatterAndFunctionsRepresentation(2560, 1600);
                envelopPlottingForm.dvScatterXSpace.Add(dvDataSpace);
                envelopPlottingForm.dvScatterFuncValues.Add(dvDataValues);
                envelopPlottingForm.scatterLineColors.Add(new Bgr(Color.Red));
                envelopPlottingForm.scatterDrawingVariants.Add(SequencesDrawingVariants.circles);

                envelopPlottingForm.theRepresentingFunctions.Add(DataAnalysisStatic.PolynomeValue);
                envelopPlottingForm.parameters.Add(approxPolyKoeffs3);
                envelopPlottingForm.lineColors.Add(new Bgr(Color.Green));

                envelopPlottingForm.theRepresentingFunctions.Add(DataAnalysisStatic.PolynomeValue);
                envelopPlottingForm.parameters.Add(approxPolyKoeffs6);
                envelopPlottingForm.lineColors.Add(new Bgr(Color.Blue));

                envelopPlottingForm.Represent();
                if ((verbosityLevel > 1) && (!isCalculatingUsingBgWorker))
                {
                    ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate ()
                    {
                        ServiceTools.ShowPicture(envelopPlottingForm.TheImage.Bitmap, "Overall GrIx values envelop vs r");
                    });
                }
                if (verbosityLevel > 1)
                {
                    envelopPlottingForm.TheImage.Save(currentDirectory + randomFileName + "-envelop-approximation-result.jpg");
                }
                #endregion plot evenlop approximation summary




                #region // не получилось
                // внедрим следующий этап:
                // по полученной зависимости (без весов) посчитаем вторую производную
                // отследим область вокруг Rs, где она положительна
                // в области от нуля до полученной - экспериментальные данные заменим
                // простой линейной интерполяцией
                // и снова прогоним аппроксимацию
                //Func<DenseVector, double, double> theEvenlopApproximationFunction2ndDeriv =
                //    ((dvKoeffs, dVal) =>
                //    {
                //        return DataAnalysis.PolynomeNthDerivative(dvKoeffs, dVal, 2);
                //    });
                //DenseVector dvPolynome2ndDrivativeAtDataSpace = (DenseVector)dvDataSpace.Map<double>(dVal =>
                //    theEvenlopApproximationFunction2ndDeriv(approxPolyKoeffs, dVal), Zeros.Include);
                //int idx = dvDataSpace.EnumerateIndexed().First(r => r.Item2 >= Rs).Item1;
                //while (dvPolynome2ndDrivativeAtDataSpace[idx] >= 0.0d)
                //{
                //    idx--;
                //}
                //DenseVector dvDataValuesInterpolated = dvDataValues.Copy();
                //dvDataValuesInterpolated.MapIndexedInplace((i, dVal) =>
                //{
                //    if ((i > 0) && (i < idx))
                //    {
                //        double r = dvDataSpace[i];
                //        return (dvDataValues[0] +
                //                (r - dvDataSpace[0]) * (dvDataValues[idx] - dvDataValues[0]) /
                //                (dvDataSpace[idx] - dvDataSpace[0]));
                //    }
                //    else
                //    {
                //        return dVal;
                //    }
                //});
                #endregion // не получилось

                #region // 2015-02-12 : пробуем использовать только первый проход аппроксимации
                // по-другому:
                // внедрим следующий этап:
                // по полученной зависимости (без весов) посчитаем вторую производную
                // отследим область, где она отрицательна
                // В таких областях заполним значения интерполяцией
                // и снова прогоним аппроксимацию

                // 2015-02-12 : пробуем использовать только первый проход аппроксимации
                // порядок полинома ставим 3
                // веса задаем по гауссу - см. подробности первого прохода

                //Func<DenseVector, double, double> theEvenlopApproximationFunction2ndDeriv =
                //    ((dvKoeffs, dVal) =>
                //    {
                //        return DataAnalysis.PolynomeNthDerivative(dvKoeffs, dVal, 2);
                //    });
                //DenseVector dv2ndDeriv = (DenseVector)dvDataSpace.Map<double>(dVal =>
                //    theEvenlopApproximationFunction2ndDeriv(approxPolyKoeffs, dVal), Zeros.Include);
                //List<double> l2ndDeriv = new List<double>(dv2ndDeriv);
                //DenseVector dvDataValuesInterpolated = dvDataValues.Copy();
                //dvDataValuesInterpolated.MapIndexedInplace((i, dVal) =>
                //{
                //    if (l2ndDeriv[i] <= 0.0d)
                //    {
                //        int idx_prev = l2ndDeriv.FindLastIndex(i-1, i, d2ndDerVal => d2ndDerVal > 0.0d);
                //        int idx_foll = l2ndDeriv.FindIndex(i+1, d2ndDerVal => d2ndDerVal > 0.0d);

                //        double r = dvDataSpace[i];
                //        double retVal = (dvDataValues[idx_prev] +
                //                (r - dvDataSpace[idx_prev]) * (dvDataValues[idx_foll] - dvDataValues[idx_prev]) /
                //                (dvDataSpace[idx_foll] - dvDataSpace[idx_prev]));
                //        return retVal;
                //    }
                //    else
                //    {
                //        return dVal;
                //    }
                //});


                //evenlopApproximator = null;

                //GradientDescentApproximator evenlopApproximator2 = new GradientDescentApproximator(dvDataValuesInterpolated,
                //                                                                                    dvDataSpace,
                //                                                                                    theEvenlopApproximationFunction);

                //dvInitialParameters_EvenlopApprox = DenseVector.Create(polynomeOrder, i => approxPolyKoeffs[i+1]);

                //// собственно аппроксимация - второй заход
                //approxPolyKoeffs = evenlopApproximator2.Approximation_ILOptimizer(dvInitialParameters_EvenlopApprox);

                //// добавим в нулевую позицию зафиксированный элемент
                //newParametersList = new List<double>(approxPolyKoeffs);
                //newParametersList.Insert(0, minSunburnGrIxValue);
                //approxPolyKoeffs = DenseVector.OfEnumerable(newParametersList);


                //#region // plot 2nd evenlop approximation result
                //envelopPlottingForm = new MultipleScatterAndFunctionsRepresentation(2560, 1600);
                //envelopPlottingForm.dvScatterXSpace.Add(dvDataSpace);
                //envelopPlottingForm.dvScatterFuncValues.Add(dvDataValuesInterpolated);
                //envelopPlottingForm.scatterLineColors.Add(new Bgr(Color.Red));
                //envelopPlottingForm.scatterDrawingVariants.Add(SequencesDrawingVariants.circles);
                //envelopPlottingForm.theRepresentingFunctions.Add(DataAnalysis.PolynomeValue);
                //envelopPlottingForm.parameters.Add(approxPolyKoeffs);
                //envelopPlottingForm.lineColors.Add(new Bgr(Color.Green));
                //envelopPlottingForm.Represent();
                //ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate()
                //{
                //    ServiceTools.ShowPicture(envelopPlottingForm.TheImage.Bitmap, "Overall GrIx values envelop vs r - 2nd result");
                //    envelopPlottingForm.TheImage.Save(currentDirectory + randomFileName + "-envelop-approximation-2nd-result.jpg");
                //});
                //#endregion // plot 2nd evenlop approximation result
                #endregion // 2015-02-12 : пробуем использовать только первый проход аппроксимации


                theLogWindow = ServiceTools.LogAText(theLogWindow, "Rs = " + Rs.ToString() + Environment.NewLine);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "GrIx Min = " + minDataValue.ToString("e") + Environment.NewLine);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "approx function koefficients (3rd order): ");
                for (int i = 0; i <= polynomeOrder3; i++)
                {
                    int l = polynomeOrder3 - i;
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "[" + l + "]: " + approxPolyKoeffs3[l].ToString("e"));
                }
                theLogWindow = ServiceTools.LogAText(theLogWindow, "approx function koefficients (6th order): ");
                for (int i = 0; i <= polynomeOrder6; i++)
                {
                    int l = polynomeOrder6 - i;
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "[" + l + "]: " + approxPolyKoeffs6[l].ToString("e"));
                }


                double rMaxOfApproximatedMinimumsDistribution = dvDataSpace.Max();

                #region // LastMileApproximation
                //DenseVector dvDataValuesForLastMileApproximation = DenseVector.Create(2,
                //    new Func<int, double>(
                //        i =>
                //            (i == 0)
                //                ? (DataAnalysis.PolynomeValue(approxPolyKoeffs, dvDataSpace[dvDataSpace.Count - 1]))
                //                : (1.0d)));
                //DenseVector dvSpaceValuesForLastMileApproximation = DenseVector.Create(2,
                //    new Func<int, double>(
                //        i =>
                //            (i == 0)
                //                ? (dvDataSpace[dvDataSpace.Count - 1])
                //                : (rMax)));
                //DenseVector theLastMileLinearApproximationKoeffs =
                //    DataAnalysis.NPolynomeApproximationLessSquareMethod(dvDataValuesForLastMileApproximation,
                //        dvSpaceValuesForLastMileApproximation, null, 1);
                #endregion // LastMileApproximation

                BGWorkerReport("аппроксимация зависимости GrIx(r) завершена");

                #endregion аппроксимация распределения GrIx(r) для огибающей (ЗАВЕРШЕНО)








                #region здесь исследуем распределение локальных минимумов распределения GrIx по r в полярной системе координат
                BGWorkerReport("Исследуем распределение величины GrIx по расстоянию от края солнечного диска");

                #region //obsolete
                // int angleBinsCount = 144;
                #endregion //obsolete


                #region //obsolete
                //int distanceBinsCount = 50;
                //double distanceKoeff = dmDistanceToSunCenter.Values.Max() / 50.0d;
                //double angleValueDiff = 2 * Math.PI / (angleBinsCount - 1);


                //double distanceValueDiff = dmDistanceToSunCenter.Values.Maximum() / (distanceBinsCount - 1);
                //DenseMatrix angleValuesForDistribution = DenseMatrix.Create(angleBinsCount, distanceBinsCount,
                //    new Func<int, int, double>(
                //        (row, column) =>
                //        {
                //            return row * angleValueDiff;
                //        }));
                #endregion //obsolete

                #region // Для получения первого приближения используем следующие рассуждения:
                // предполагаем, что наиболее вероятная симметрия - 
                // относительно прямой, соединяющей центр солнечного диска и центр изображения
                // поэтому начальное значение смещения phi0 для аппроксимации зависимости расположения
                // Rm от phi можно оценить исходя из расположения солнечного диска относительно центра изображения.
                // Здесь Rm - насстояние от минимального значения GrIx в направлении phi до центра солнечного диска.
                // phi - угол в полярной системе координат с началом координат в центре солнечного диска.
                // Угол отсчитывается от горизонтальной прямой. Нулевое значение угла - направо.
                #endregion // Для получения первого приближения используем следующие рассуждения:
                double dxSunToCenter = (double)imageCircleCenter.X - (double)sunCenterPoint.X;
                double dySunToCenter = (double)imageCircleCenter.Y - (double)sunCenterPoint.Y;
                double rSunToCenter = Math.Sqrt(dxSunToCenter * dxSunToCenter + dySunToCenter * dySunToCenter);
                double cosPhi1 = dxSunToCenter / rSunToCenter;
                double Phi1 = Math.Acos(cosPhi1);
                if (dySunToCenter > 0) Phi1 = 2.0d * Math.PI - Phi1;


                #region // obsolete
                //DenseMatrix dmPolarMinimumsDistribution =
                //    DataAnalysis.GetLocalMinimumsDistribution(dmPolarSystemGrIxDistribution, sunCenterPoint,
                //        imgP.imageRD.pointDCircleCenter(), imgP.imageRD.DRadius, dmSunburnProfileDetection.RowCount,
                //        theImageCircleCropFactor);

                //List<Point3D> lLocalMinimumsPolar = new List<Point3D>();
                //for (int r = 0; r < dmPolarMinimumsDistribution.RowCount; r++)
                //{
                //    for (int c = 0; c < dmPolarMinimumsDistribution.ColumnCount; c++)
                //    {
                //        if (dmPolarMinimumsDistribution[r,c] > 0.0d)
                //        {
                //            // r - угол
                //            // c - расстояние
                //            lLocalMinimumsPolar.Add(new Point3D(r, c, dmPolarMinimumsDistribution[r, c]));
                //        }
                //    }
                //}
                #endregion // obsolete


                #region // debug plotting
                //foreach (Tuple<int, Vector<double>> tpl in dmPolarSystemGrIxDistribution.EnumerateRowsIndexed())
                //{
                //    tpl.Item2.SaveVectorDataAsImagePlot(currentDirectory + randomFileName + "_PolarSlice_" +
                //                                        tpl.Item1.ToString("D03") + ".png");
                //}
                #endregion // debug plotting


                List<Point3D> lLocalMinimumsPolar = DataAnalysisStatic.GetLocalMinimumsDistribution(dmPolarSystemGrIxDistribution, sunRoundData,
                        imgP.imageRD, dmSunburnProfileDetection.RowCount,
                        theImageCircleCropFactor);

                // отфильтруем точки, слишком близкие к солнечному диску и слишком близкие к границе изображения


                // получим все эти точки в картезианской СК - будет нужно позже
                List<Point3D> lLocalMinimumsCartesian = lLocalMinimumsPolar.ConvertAll(pt3D =>
                {
                    PointPolar ptPol = new PointPolar(pt3D.Y, pt3D.X);
                    PointD ptd = ptPol.PointD();
                    ptd.Y = -ptd.Y; // потому что угол отмеряется визуально - против часовой стрелки, а Y отмерятеся вниз
                    ptd = (sunCenterPoint + new Vector2D(ptd)).ToPointD();
                    return new Point3D(ptd.X, ptd.Y, pt3D.Z);
                });

                lLocalMinimumsPolar =
                    new List<Point3D>(lLocalMinimumsPolar.Zip(lLocalMinimumsCartesian, (ptPl, ptCrt) =>
                            ((ptCrt.X < 0.0d) || (ptCrt.X > dmMask.ColumnCount) || (ptCrt.Y < 0.0d) || (ptCrt.Y > dmMask.RowCount))
                                ? (Point3D.nullPoint3D())
                                : (ptPl)));
                lLocalMinimumsPolar.RemoveAll(pt3d => pt3d.IsNull);
                lLocalMinimumsCartesian =
                    lLocalMinimumsCartesian.ConvertAll(ptCrt =>
                            ((ptCrt.X < 0.0d) || (ptCrt.X > dmMask.ColumnCount) || (ptCrt.Y < 0.0d) || (ptCrt.Y > dmMask.RowCount))
                                ? (Point3D.nullPoint3D())
                                : (ptCrt));
                lLocalMinimumsCartesian.RemoveAll(pt3d => pt3d.IsNull);

                //DenseMatrix dmDecartMinimumsDistribution = DataAnalysis.PolarToDecart(dmPolarMinimumsDistribution,
                //        sunCenterPoint, dmSunburnProfileDetection.ColumnCount, dmSunburnProfileDetection.RowCount);


                //надо отфильтровать краевые минимумы на границе кадра - DONE


                // надо выделить данные зависимости локальных минимумов от угла phi
                // аппроксимировать эту зависимость Rm(phi)
                // получим параметры распределения для конкретного изображения - их можно использовать дальше



                #region //obsolete
                //if (verbosityLevel > 1)
                //{
                //    #region //obsolete
                //    //ImageConditionAndDataRepresentingForm theForm =
                //    //    ServiceTools.RepresentDataFromDenseMatrix(dmPolarSystemMinimumsDistribution,
                //    //        "minimums distribution in polar coordinates", false, false, 0.0d, 1.0d, false);
                //    //theForm.SaveData(currentDirectory + randomFileName + "_SunburnProfilePolar.nc", true);
                //    ////theForm.Close();
                //    //theForm.Dispose();
                //    #endregion //obsolete

                //    //dmPolarSystemGrIxDistribution.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                //    //                                                       "_SunburnProfilePolar.nc");
                //    #region //obsolete
                //    //ImageConditionAndDataRepresentingForm theForm1 =
                //    //    ServiceTools.RepresentDataFromDenseMatrix(dmPolarMinimumsDistribution,
                //    //        "minimums distribution in polar coordinates", false, false, 0.0d, 1.0d, false);
                //    //theForm1.SaveData(currentDirectory + randomFileName + "_SunburnProfileMinimums.nc", true);
                //    ////theForm1.Close();
                //    //theForm1.Dispose();
                //    //
                //    //dmPolarMinimumsDistribution.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                //    //                                                 "_SunburnProfileMinimums.nc");
                //    //
                //    //ImageConditionAndDataRepresentingForm theForm2 =
                //    //    ServiceTools.RepresentDataFromDenseMatrix(dmDecartMinimumsDistribution,
                //    //        "minimums distribution in decart coordinates", false, false, 0.0d, 1.0d, false);
                //    //theForm2.SaveData(currentDirectory + randomFileName + "_SunburnProfileMinimumsDecart.nc", true);
                //    ////theForm2.Close();
                //    //theForm2.Dispose();
                //    ////ImageConditionAndDataRepresentingForm TheAngleRepresentation =
                //    ////    ServiceTools.RepresentDataFromDenseMatrix(angleValuesForDistribution, "the angle");
                //    ////TheAngleRepresentation.SaveData(randomFileName + "_SunburnProfileMinimumsPolarAngles.nc");
                //    //dmDecartMinimumsDistribution.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                //    //                                                  "_SunburnProfileMinimumsDecart.nc");
                //    #endregion //obsolete
                //}
                #endregion //obsolete


                theLogWindow = ServiceTools.LogAText(theLogWindow, "смещение по углу: " + Phi1.ToString("e"));


                #region аппроксимация распределения Rm(phi)

                BGWorkerReport("Готовим данные для аппроксимации зависимости Rm(phi)");

                #region формируем данные для аппроксимации зависимости Rm(phi) и выполняем саму аппроксимацию

                List<double> phiMinimumsSpace = lLocalMinimumsPolar.ConvertAll(pt3D => pt3D.X);
                List<double> rMinimumsSpace = lLocalMinimumsPolar.ConvertAll(pt3D => pt3D.Y);
                List<double> dataMinimumsList = lLocalMinimumsPolar.ConvertAll(pt3D => pt3D.Z);
                rMinimumsSpace = rMinimumsSpace.ConvertAll(r => r - sunRadius);

                #region // obsolete
                //foreach (Tuple<int, Vector<double>> tuple in dmPolarMinimumsDistribution.EnumerateRowsIndexed())
                //{
                //    List<double> angleRowVector = new List<double>(tuple.Item2);
                //
                //    if (angleRowVector.Sum() == 0.0d) continue;
                //
                //    double currentAngle = 2.0d * Math.PI * (double)tuple.Item1 / (double)(angleBinsCount);
                //    double currRadius = angleRowVector.FindIndex(x => x > 0.0d) - sunRadius;
                //
                //    if (currRadius <= 0.0d) continue;
                //
                //    double currMinimumValue = angleRowVector.Find(x => x > 0.0d);
                //    phiMinimumsSpace.Add(currentAngle);
                //    rMinimumsSpace.Add(currRadius);
                //    dataMinimumsList.Add(currMinimumValue);
                //}
                #endregion // obsolete


                if (dataMinimumsList.Count < 10)
                {
                    theSunSuppressionSchemeApplicable = false;

                    theLogWindow =
                        ServiceTools.LogAText(theLogWindow, "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + Environment.NewLine +
                                              "Слишком нестабильный результат с подавлением засветки. Используем алгоритм без подавления. Case 01" +
                                              Environment.NewLine + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");


                    break;
                }



                Func<DenseVector, double, double> theApproximationFunction = (dvParameters, phi) =>
                {
                    double d1 = dvParameters[0];
                    double d2 = dvParameters[1];
                    double r = dvParameters[2];
                    double phi0 = dvParameters[3];
                    return d1 * Math.Cos(phi - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(phi - phi0), 2.0d));
                };

                #region //obsolete
                //List<Func<DenseVector, bool>> theApproximatinFunctionParametersConditions = new List<Func<DenseVector, bool>>();
                //theApproximatinFunctionParametersConditions.Add(dvPar =>
                //                                                {
                //                                                    double d1 = dvPar[0];
                //                                                    double d2 = dvPar[1];
                //                                                    double r = dvPar[2];
                //
                //                                                    if ((d1 < 0.0d) || (d2 < 0.0d) || (r < 0.0d))
                //                                                    {
                //                                                        return false;
                //                                                    }
                //
                //                                                    if ((d1 > r) || (d2 > r))
                //                                                    {
                //                                                        return false;
                //                                                    }
                //                                                    return true;
                //                                                });
                #endregion //obsolete

                #region //obsolete - inequality constraints function for ILNumerics Optimization Toolbox
                //List<Func<DenseVector, double, double>> theApproximatinFunctionParametersConditionsInequality =
                //    new List<Func<DenseVector, double, double>>();
                //theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => -dvPar[0]);
                //theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => -dvPar[1]);
                //theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => -dvPar[2]);
                //theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => dvPar[0] - dvPar[2]);
                //theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => dvPar[1] - dvPar[2]);
                #endregion //obsolete - inequality constraints function for ILNumerics Optimization Toolbox


                DenseVector dvWeights = DenseVector.Create(dataMinimumsList.Count, new Func<int, double>(i =>
                {
                    double weight = (0.6d +
                            0.4d * (dataMinimumsList[i] - dataMinimumsList.Min()) /
                            (dataMinimumsList.Max() - dataMinimumsList.Min()));
                    weight *= (1.0d + 0.2d * Math.Pow(Math.Cos((phiMinimumsSpace[i] - Phi1)), 2.0d));

                    return weight;
                }));


                DenseVector dvRMinimumsValues = DenseVector.OfEnumerable(rMinimumsSpace);
                DenseVector dvphiMinimumsSpace = DenseVector.OfEnumerable(phiMinimumsSpace);
                #region // GradientDescentApproximator
                //GradientDescentApproximator approximator = new GradientDescentApproximator( dvRMinimumsSpace,
                //                                                                            dvphiMinimumsSpace,
                //                                                                            theApproximatinFunction);
                #endregion // GradientDescentApproximator

                if (verbosityLevel > 0)
                {
                    foreach (double weight in dvWeights)
                        ServiceTools.logToTextFile(currentDirectory + randomFileName + "_dataWeights.csv",
                            weight.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                    foreach (double rMinimum in dvRMinimumsValues)
                        ServiceTools.logToTextFile(currentDirectory + randomFileName + "_dataMinimumR.csv",
                            rMinimum.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                    foreach (double phiMinimum in dvphiMinimumsSpace)
                        ServiceTools.logToTextFile(currentDirectory + randomFileName + "_dataMinimumPhi.csv",
                            phiMinimum.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                }


                #region //obsolete - parameters space constraints
                //approximator.parametersConditions.Add(new Func<DenseVector, bool>(dvCurrentParameters =>
                //{
                //    double d1 = dvCurrentParameters[0];
                //    double d2 = dvCurrentParameters[1];
                //    double r = dvCurrentParameters[2];
                //    double phi0 = dvCurrentParameters[3];
                //    //return d1 * Math.Cos(phi - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(phi - phi0), 2.0d));
                //
                //    return ((d1 + r > 0) && (d1 + r < rSunToCenter + imageCircleRadius));
                //
                //    return true;
                //}));
                #endregion //obsolete - parameters space constraints


                double initialParametersKoeff = (double)Rs / (rSunToCenter + imgP.imageRD.DRadius);// / максимальное расстояние до края картинки в этом направлении = это D+R
                List<double> initialParametersList = new List<double>();
                initialParametersList.Add(rSunToCenter * initialParametersKoeff);
                initialParametersList.Add(rSunToCenter * initialParametersKoeff);
                initialParametersList.Add(imgP.imageRD.DRadius * initialParametersKoeff);
                initialParametersList.Add(Phi1);
                DenseVector dvInitialParameters = DenseVector.OfEnumerable(initialParametersList);
                //DenseVector initialParametersIncremnt = DenseVector.Create(dvInitialParameters.Count, (i => 1.0d));



                #region Добавим интерполированные данные, чтобы решение не разносило
                // Там, где очень большие пропуски данных, например,
                // если солнце близко к горизонту, - тогда в районе между солнцем и горизонтом и на приличный угол от этой зоны
                // - там имеет смысл слегка зафиксировать решение
                // иначе в этой зоне отсутствие всяких данных "разрешает" решению расходиться
                // вне зависимости от поставленных ограничений на пространство решений

                // Поэтому имеет смысл считать аппроксимацию на таком дополненном множестве точек
                // это даст решение в пространстве параметров
                // сами же точки можно потом не принимать во внимание
                // Для этого проведем аппроксимацию на временных массивах, не модифицируя исходные, полученные из изображения
                double shift = dvphiMinimumsSpace[0];
                DenseVector dvTmpXdataShifted = (DenseVector)dvphiMinimumsSpace.Map(d => d - shift);
                DenseVector dvFuncData = DenseVector.OfEnumerable(dvRMinimumsValues);
                DenseVector dvTmpParameters = dvInitialParameters.Copy();
                dvTmpParameters[3] = dvTmpParameters[3] - shift;
                if (dvTmpXdataShifted.Last() < 2.0d * Math.PI)
                {
                    dvTmpXdataShifted =
                        DenseVector.OfEnumerable(dvTmpXdataShifted.Concat(DenseVector.Create(1, 2.0d * Math.PI)));
                    dvFuncData =
                        DenseVector.OfEnumerable(
                            dvFuncData.Concat(DenseVector.Create(1,
                                theApproximationFunction(dvTmpParameters, 2.0d * Math.PI))));
                }

                DenseVector dvGaps = DenseVector.Create(dvTmpXdataShifted.Count - 1, i => dvTmpXdataShifted[i + 1] - dvTmpXdataShifted[i]);
                int num = Convert.ToInt32(2.0d * Math.PI / dvGaps.AbsoluteMinimum());
                // итерационно добавляем точки посередине пропусков длиной более (2*PI/num)*20
                double maxGap = 30.0d * 2.0d * Math.PI / (double)num;
                while (dvGaps.AbsoluteMaximum() > maxGap)
                {
                    int maxGapIdx = dvGaps.AbsoluteMaximumIndex();
                    List<double> tmpXData = new List<double>(dvTmpXdataShifted);
                    List<double> tmpFuncData = new List<double>(dvFuncData);
                    double newXvalue = (dvTmpXdataShifted[maxGapIdx] + dvTmpXdataShifted[maxGapIdx + 1]) / 2.0d;
                    double newFuncValue = theApproximationFunction(dvTmpParameters, newXvalue);
                    tmpXData.Insert(maxGapIdx + 1, newXvalue);
                    tmpFuncData.Insert(maxGapIdx + 1, newFuncValue);
                    dvTmpXdataShifted = DenseVector.OfEnumerable(tmpXData);
                    dvFuncData = DenseVector.OfEnumerable(tmpFuncData);

                    dvGaps = DenseVector.Create(dvTmpXdataShifted.Count - 1, i => dvTmpXdataShifted[i + 1] - dvTmpXdataShifted[i]);
                }
                dvTmpXdataShifted = (DenseVector)dvTmpXdataShifted.Map(d => d + shift);
                List<double> tmpXData2 = new List<double>(dvTmpXdataShifted);
                tmpXData2.RemoveAt(tmpXData2.Count - 1);
                dvTmpXdataShifted = DenseVector.OfEnumerable(tmpXData2);
                List<double> tmpFuncData2 = new List<double>(dvFuncData);
                tmpFuncData2.RemoveAt(tmpFuncData2.Count - 1);
                dvFuncData = DenseVector.OfEnumerable(tmpFuncData2);

                #endregion Добавим интерполированные данные, чтобы решение не разносило



                DenseVector dvLowerBounds = DenseVector.Create(dvInitialParameters.Count, 0.0d);
                DenseVector dvUpperBounds = DenseVector.Create(dvInitialParameters.Count, idx =>
                {
                    if ((idx >= 0) && (idx < 3))
                    {
                        return 2.0d * imageCircleRadius;
                    }
                    else return 2.0d * Math.PI;
                });

                #region Создание и заполнение свойств самого объекта - интерфейса к решателю задач оптимизации MKL

                NonLinLeastSqProbWithBC<double> approximator = new NonLinLeastSqProbWithBC<double>();
                approximator.mEventsSpaceVector = dvTmpXdataShifted.Copy();
                approximator.mFittingValuesVector = dvFuncData.Copy();
                approximator.nParametersSpacePoint = dvInitialParameters.Copy();
                approximator.upperBoundConstraints = dvUpperBounds.Copy();
                approximator.lowerBoundConstraints = dvLowerBounds.Copy();
                approximator.fittingFunction =
                    (paramsVector, phiValue) => theApproximationFunction(DenseVector.OfEnumerable(paramsVector), phiValue);
                #endregion Создание и заполнение свойств самого объекта - интерфейса к решателю задач оптимизации MKL

                /// todo: возможность минимизации с учетом веса точек не реализована. Надо реализовать
                // approximator.DvWeights = dvWeights;
                // approximator.parametersConditionsLessThan0 = theApproximatinFunctionParametersConditionsInequality;

                #region вывод данных в файл, если это необходимо
                if (verbosityLevel > 0)
                {
                    foreach (double initialParameter in dvInitialParameters)
                    {
                        ServiceTools.logToTextFile(
                            currentDirectory + randomFileName + "_approximationInitialParameters.csv",
                            initialParameter.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                    }
                }
                #endregion вывод данных в файл, если это необходимо



                #region //obsolete
                //DenseVector approximatedParameters = approximator.ApproximationGradientDescentMultidim(dvInitialParameters, ref initialParametersIncremnt, 0.0000001d);
                //DenseVector approximatedParameters = approximator.Approximation_ILOptimizer(dvInitialParameters);
                #endregion //obsolete

                DenseVector approximatedParameters = DenseVector.OfEnumerable(approximator.SolveOptimizationProblem());


                #region // вторичная аппроксимация с фильтром точек, сильно отстоящих от первого решения
                //попробуем вот так:
                // в каждой точке, где определены экспериментальные данные, посчитаем отклонение от аппроксимационной функции
                // посчитаем по этим отклонениям статистику и отяильтруем точки за пределами например стандартного отклонения
                // еще раз прогоним аппроксимацию

                //DenseVector dvTempDeviations = DenseVector.Create(dvRMinimumsValues.Count, new Func<int, double>(i =>
                //{
                //    return dvRMinimumsValues[i] - theApproximationFunction(approximatedParameters, dvphiMinimumsSpace[i]);
                //}));
                //DescriptiveStatistics deviationsStats = new DescriptiveStatistics(dvTempDeviations.Values);
                //List<double> tunedPhiSpace = new List<double>();
                //List<double> tunedRSpace = new List<double>();
                //for (int i = 0; i < dvTempDeviations.Count; i++)
                //{
                //    if (Math.Abs(dvTempDeviations[i]) > deviationsStats.StandardDeviation)
                //    {
                //        continue;
                //    }
                //    tunedPhiSpace.Add(dvphiMinimumsSpace[i]);
                //    tunedRSpace.Add(dvRMinimumsValues[i]);
                //}
                //DenseVector dvTunedPhiSpace = DenseVector.OfEnumerable(tunedPhiSpace);
                //DenseVector dvTunedRSpace = DenseVector.OfEnumerable(tunedRSpace);
                //DenseVector dvTunedWeights = DenseVector.Create(tunedRSpace.Count, new Func<int, double>(i =>
                //{
                //    double weight = (0.6d +
                //            0.4d * (tunedRSpace[i] - tunedRSpace.Min()) /
                //            (tunedRSpace.Max() - tunedRSpace.Min()));

                //    return weight;
                //}));

                //approximator.DvWeights = dvTunedWeights;
                //approximator.dvSpace = dvTunedPhiSpace;
                //approximator.dvDataValues = dvTunedRSpace;
                ////approximatedParameters = approximator.ApproximationGradientDescentMultidim(approximatedParameters, ref initialParametersIncremnt, 0.000001d);
                //// approximatedParameters = approximator.Approximation_ILOptimizer(approximatedParameters);

                //throw new NotImplementedException("вторичная аппроксимация зависимости Rm(phi) временно отключена. Надо включить - без нее работать корректно не будет!");

                //// фак
                //// откуда-то NaN вылез :((
                #endregion // вторичная аппроксимация с фильтром точек, сильно отстоящих от первого решения


                #region отображение результатов аппроксимации
                // resulting data
                MultipleScatterAndFunctionsRepresentation plotter = new MultipleScatterAndFunctionsRepresentation(2560, 1600);
                plotter.yAxisNote = "r, distance to sunburn margin (px)";
                plotter.xAxisNote = "\\phi, (rad)";
                plotter.theRepresentingFunctions.Add(theApproximationFunction);
                plotter.parameters.Add(dvInitialParameters);
                plotter.lineColors.Add(new Bgr(Color.Red));
                plotter.dvScatterXSpace.Add(DenseVector.OfEnumerable(phiMinimumsSpace));
                plotter.dvScatterFuncValues.Add(DenseVector.OfEnumerable(rMinimumsSpace));
                plotter.scatterLineColors.Add(new Bgr(Color.Green));
                plotter.scatterDrawingVariants.Add(SequencesDrawingVariants.circles);
                plotter.theRepresentingFunctions.Add(theApproximationFunction);
                plotter.parameters.Add(approximatedParameters);
                plotter.lineColors.Add(new Bgr(Color.Magenta));

                plotter.xSpaceMin = 0.0d;
                plotter.xSpaceMax = 2.0d * Math.PI;

                plotter.Represent();

                if ((verbosityLevel > 1) && (!isCalculatingUsingBgWorker))
                {
                    ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate ()
                    {
                        ServiceTools.ShowPicture(plotter.TheImage.Bitmap,
                            "r(phi) distribution data, initial approximation and the approximation result");
                    });
                }
                if (verbosityLevel > 1)
                {
                    plotter.TheImage.Save(currentDirectory + randomFileName + "_r_phi_approximation.png");
                }
                #endregion отображение результатов аппроксимации

                #endregion формируем данные для аппроксимации зависимости r(phi) и выполняем саму аппроксимацию

                BGWorkerReport("Аппроксимация зависимости Rs(phi) завершена");

                #endregion аппроксимация распределения Rm(phi)

                #endregion здесь исследуем распределение локальных минимумов распределения GrIx по r в полярной системе координат


                BGWorkerReport("оцениваем общий тренд уклона фоновой засветки, - будем учитывать его на финальном шаге компенсации");




                #region оценим общий тренд уклона фоновой засветки по распределению локальных минимумов
                //самый глубокий минимум известен - на расстоянии Rs со значением minDataValue
                //направление на него - направление на самую дальнюю точку границы круга, который представляет собой снимок.

                //поправка: направление будем определять из аппроксимации распределения минимумов по углу. смещение phi0 - будет нужный угол

                // Попробовать новый подход: по совокупности точек, представляющих минимальное значение GrIx в каждом направлении phi
                // опять же аппроксимировать наиболее подходящую плоскость.
                // Плоскость задается точкой (x0,y0,GrIx0) и 3D-вектором нормали (x_n, y_n, GrIx_n) - это шесть параметров.
                // Расстояние от точки (x,y,GrIx), где расположен минимум GrIx в каждом направлении, до этой плоскости
                // можно легко посчитать. Минимизируем сумму этих расстояний - получим искомую плоскость.
                // Начальные данные для аппроксимации тоже легко получить: есть точка Rs с самым глубоким минимумом -
                // известны ее координаты и значение GrIx. Точка минимального значения GrIx на другой стороне
                // от солнечного диска - вторая опорная точка. Так получаем одну прямую, лежащую в
                // начальном приближении искомой плоскости. Вектор нормали будет перпендикулярен этой прямой
                // и будет лежать в вертикальной плоскости.




                Phi1 = approximatedParameters[3];

                #region // obsolete
                //Если изображение кропнутое, надо это учитывать
                //double tgT = (sunCenterPoint.Y - imageCircleCenter.Y) / (sunCenterPoint.X - imageCircleCenter.X);
                //double tethaAngle = Math.Atan(tgT);
                //DenseMatrix dmXdashedCoordinate = DenseMatrix.Create(dmProcessingData.RowCount,
                //    dmProcessingData.ColumnCount, (r, c) =>
                //    {
                //        double currX = (double)c;
                //        double currY = (double)r;
                //        double xDashed = currX * Math.Cos(tethaAngle) + currY * Math.Sin(tethaAngle);
                //        return xDashed;
                //    });

                //List<double> minimumsDataValues = new List<double>();
                //List<double> minimumsXDashedCoodinate = new List<double>();
                //for (int i = 0; i < dmXdashedCoordinate.RowCount; i++)
                //{
                //    for (int j = 0; j < dmXdashedCoordinate.ColumnCount; j++)
                //    {
                //        if (dmDecartMinimumsDistribution[i, j] > 0.0d)
                //        {
                //            minimumsDataValues.Add(dmDecartMinimumsDistribution[i, j]);
                //            minimumsXDashedCoodinate.Add(dmXdashedCoordinate[i, j]);
                //        }
                //    }
                //}
                //DenseVector theLine1Coeffs =
                //    DataAnalysis.NPolynomeApproximationLessSquareMethod(DenseVector.OfEnumerable(minimumsDataValues),
                //        DenseVector.OfEnumerable(minimumsXDashedCoodinate), new List<PointD>(), 1);

                //DenseMatrix dmValuesToSubtract_plate = DenseMatrix.Create(dmProcessingData.RowCount,
                //    dmProcessingData.ColumnCount, new Func<int, int, double>(
                //        (row, column) =>
                //        {
                //            return DataAnalysis.PolynomeValue(theLine1Coeffs, dmXdashedCoordinate[row, column]);
                //        }));

                //if (verbosityLevel > 1)
                //{
                //    dmValuesToSubtract_plate.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                //                                                  "_TheDataToSubtract_plate.nc");
                //}
                #endregion // obsolete





                // сделаем по-другому
                // предположим, что есть некое начальное приближение плоскости:
                // опорная точка расположена в глобальном минимуме
                // нормаль направлена точно вверх
                // 
                // возьмем все точки, представляющие локальные минимумы в каждом из направлений
                // минимизируя по методу наименьших квадратов сумму расстояний от всех этих точек
                // до аппроксимируемой плоскости,
                // получим наиболее подходящую плоскость.
                // в таком варианте нам будут нужны только точки - минимумы
                LineDescription2D lSunToCenter = new LineDescription2D(sunCenterPoint,
                    new Vector2D(sunCenterPoint, imageCircleCenter));

                Point3D planeP0 = new Point3D(
                    lSunToCenter.p0.X + lSunToCenter.directionVector.X * Rs,
                    lSunToCenter.p0.Y + lSunToCenter.directionVector.Y * Rs,
                    minDataValue);
                Vector3D planeN = new Vector3D(0.0d, 0.0d, 1.0d);

                List<double> dvParametersForPlane = new List<double>();
                dvParametersForPlane.Add(planeP0.X);
                dvParametersForPlane.Add(planeP0.Y);
                dvParametersForPlane.Add(planeP0.Z);
                dvParametersForPlane.Add(planeN.X);
                dvParametersForPlane.Add(planeN.Y);
                dvParametersForPlane.Add(planeN.Z);

                NonLinLeastSqProbWithBC<Point3D> planeApproximator = new NonLinLeastSqProbWithBC<Point3D>();
                planeApproximator.mEventsSpaceVector = lLocalMinimumsCartesian;
                planeApproximator.mFittingValuesVector = DenseVector.Create(lLocalMinimumsCartesian.Count, 0.0d);
                planeApproximator.nParametersSpacePoint = dvParametersForPlane;
                planeApproximator.fittingFunction = (parameters, point) =>
                {
                    Point3D planePt3D0 = new Point3D(parameters.ElementAt(0), parameters.ElementAt(1), parameters.ElementAt(2));
                    Vector3D planeNormVec = new Vector3D(parameters.ElementAt(3), parameters.ElementAt(4),
                        parameters.ElementAt(5));
                    Plane3D pl = new Plane3D(planePt3D0, planeNormVec);
                    return pl.Distance(point);
                };
                planeApproximator.lowerBoundConstraints = new double[] { 0.0d, 0.0d, -minSunburnGrIxValue, -1.0d, -1.0d, -1.0d };
                planeApproximator.upperBoundConstraints = new double[] { dmProcessingData.ColumnCount, dmProcessingData.RowCount, minSunburnGrIxValue, 1.0d, 1.0d, 1.0d };
                dvParametersForPlane = new List<double>(planeApproximator.SolveOptimizationProblem());
                planeP0 = new Point3D(dvParametersForPlane.ElementAt(0), dvParametersForPlane.ElementAt(1), dvParametersForPlane.ElementAt(2));
                planeN = new Vector3D(dvParametersForPlane.ElementAt(3), dvParametersForPlane.ElementAt(4), dvParametersForPlane.ElementAt(5));
                Plane3D approximatedPlane = new Plane3D(planeP0, planeN);
                DenseMatrix dmValuesToSubtract_plate = DenseMatrix.Create(dmProcessingData.RowCount,
                    dmProcessingData.ColumnCount, (r, c) =>
                    {
                        return approximatedPlane.PointAtPlane(new PointD(c, r)).Z;
                    });

                BGWorkerReport("окончание оценки общего тренда уклона фоновой засветки");

                #endregion оценим общий тренд уклона фоновой засветки по распределению локальных минимумов





                if (verbosityLevel > 1)
                {
                    #region // obsolete
                    //dmDistanceToSunMargin.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                    //                                           "_SunMarginDistance.nc");
                    //dmAngleAroundTheSunburn.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_SunAngle.nc");
                    #endregion // obsolete

                    DenseMatrix dmValuesToSubtract_plate_tmp = dmValuesToSubtract_plate.Copy();
                    foreach (Point3D pt3D in lLocalMinimumsCartesian)
                    {
                        int row = Convert.ToInt32(pt3D.Y);
                        row = (row == dmValuesToSubtract_plate_tmp.RowCount) ? (row - 1) : (row);
                        int col = Convert.ToInt32(pt3D.X);
                        col = (col == dmValuesToSubtract_plate_tmp.ColumnCount) ? (col - 1) : (col);
                        try
                        {
                            dmValuesToSubtract_plate_tmp[row, col] = pt3D.Z;
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                            continue;
                        }
                    }
                    dmValuesToSubtract_plate_tmp.MapIndexedInplace((r, c, dVal) =>
                    {
                        PointD ptdCurrPoint = new PointD(c, r);
                        if (ptdCurrPoint.IsPointInsideCircle(sunRoundData) >= 0)
                        {
                            return 1.0d;
                        }
                        return dVal;
                    });
                    dmValuesToSubtract_plate_tmp = (DenseMatrix)dmValuesToSubtract_plate_tmp.PointwiseMultiply(dmMask);
                    dmValuesToSubtract_plate_tmp.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_SubtractedPlane.nc");

                }



                BGWorkerReport("финальный шаг обработки: компенсация засветки");

                DenseMatrix dmReversed3rdOrder = dmProcessingData.Copy();
                DenseMatrix dmReversed6thOrder = dmProcessingData.Copy();

                DenseMatrix dmDataToSubtract3rdOrder = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                    new Func<int, int, double>(
                        (row, col) =>
                        {
                            PointD ptDcurrPoint = new PointD(col, row);
                            PointPolar ptPolCurrPoint = new PointPolar(ptDcurrPoint - sunCenterPoint, true);
                            //double currAngle = ptPolCurrPoint.Phi - Phi1;
                            double currDistance = ptPolCurrPoint.R - sunRadius;

                            double theMinimumDistanceForThisAngle = theApproximationFunction(approximatedParameters,
                                ptPolCurrPoint.Phi);
                            double currentLinearKoeff = theMinimumDistanceForThisAngle /
                                                        (approximatedParameters[0] + approximatedParameters[2]);
                            double maxRadiusOfApproximation = rMaxOfApproximatedMinimumsDistribution * currentLinearKoeff;
                            double scaledSubtractionValue = 0.0d;

                            if (maxRadiusOfApproximation < 0) maxRadiusOfApproximation = 0.0d;

                            if (currDistance > maxRadiusOfApproximation)
                            {
                                double startPointRFromSunMargin = maxRadiusOfApproximation;
                                double startPointR = maxRadiusOfApproximation + sunRadius;
                                double startPointVal = 1.0d -
                                                            currentLinearKoeff *
                                                            (1.0d -
                                                             DataAnalysisStatic.PolynomeValue(approxPolyKoeffs3,
                                                                 startPointRFromSunMargin / currentLinearKoeff));
                                // УПС. Так далеко наша аппроксимация не действует. Тогда просто вычтем общий фон.
                                //возьмем смещение общим уклоном dmValuesToSubtract_plate, посмотрим смещение в точке startPointX и
                                // от этого спляшем по смещению в текущую точку
                                //для этого надо знать положение стартовой точки в декартовых координатах
                                int startPointX =
                                    Convert.ToInt32((startPointR) * Math.Cos(ptPolCurrPoint.Phi) +
                                                    sunCenterPoint.X);
                                startPointX = (startPointX < 0) ? 0 : startPointX;
                                startPointX = (startPointX >= dmValuesToSubtract_plate.ColumnCount) ? (dmValuesToSubtract_plate.ColumnCount - 1) : startPointX;

                                int startPointY =
                                    Convert.ToInt32(-(startPointR) * Math.Sin(ptPolCurrPoint.Phi) +
                                                     sunCenterPoint.Y);
                                startPointY = (startPointY < 0) ? 0 : startPointY;
                                startPointY = (startPointY >= dmValuesToSubtract_plate.RowCount) ? (dmValuesToSubtract_plate.RowCount - 1) : startPointY;

                                double dVal = dmValuesToSubtract_plate[row, col] - dmValuesToSubtract_plate[startPointY, startPointX];

                                //scaledSubtractionValue = startPointVal +
                                //                         (currDistance - startPointX)*(endPointVal - startPointVal)/
                                //                         (endPointX - startPointX);
                                scaledSubtractionValue = startPointVal + dVal;
                            }
                            else
                            {
                                scaledSubtractionValue = 1.0d -
                                                            currentLinearKoeff *
                                                            (1.0d -
                                                             DataAnalysisStatic.PolynomeValue(approxPolyKoeffs3,
                                                                 currDistance / currentLinearKoeff));
                            }

                            if (double.IsNaN(scaledSubtractionValue)) return 0.0d;
                            if (scaledSubtractionValue > 1.0d) return 1.0d;
                            if (scaledSubtractionValue < 0.0d) return 0.0d;

                            return scaledSubtractionValue;
                        }));


                DenseMatrix dmDataToSubtract6thOrder = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                    new Func<int, int, double>(
                        (row, col) =>
                        {
                            PointD ptDcurrPoint = new PointD(col, row);
                            PointPolar ptPolCurrPoint = new PointPolar(ptDcurrPoint - sunCenterPoint, true);
                            //double currAngle = ptPolCurrPoint.Phi - Phi1;
                            double currDistance = ptPolCurrPoint.R - sunRadius;

                            double theMinimumDistanceForThisAngle = theApproximationFunction(approximatedParameters,
                                ptPolCurrPoint.Phi);
                            double currentLinearKoeff = theMinimumDistanceForThisAngle /
                                                        (approximatedParameters[0] + approximatedParameters[2]);
                            double maxRadiusOfApproximation = rMaxOfApproximatedMinimumsDistribution * currentLinearKoeff;
                            double scaledSubtractionValue = 0.0d;

                            if (maxRadiusOfApproximation < 0) maxRadiusOfApproximation = 0.0d;

                            if (currDistance > maxRadiusOfApproximation)
                            {
                                double startPointRFromSunMargin = maxRadiusOfApproximation;
                                double startPointR = maxRadiusOfApproximation + sunRadius;
                                double startPointVal = 1.0d -
                                                            currentLinearKoeff *
                                                            (1.0d -
                                                             DataAnalysisStatic.PolynomeValue(approxPolyKoeffs6,
                                                                 startPointRFromSunMargin / currentLinearKoeff));
                                // УПС. Так далеко наша аппроксимация не действует. Тогда просто вычтем общий фон.
                                //возьмем смещение общим уклоном dmValuesToSubtract_plate, посмотрим смещение в точке startPointX и
                                // от этого спляшем по смещению в текущую точку
                                //для этого надо знать положение стартовой точки в декартовых координатах
                                int startPointX =
                                    Convert.ToInt32((startPointR) * Math.Cos(ptPolCurrPoint.Phi) +
                                                    sunCenterPoint.X);
                                startPointX = (startPointX < 0) ? 0 : startPointX;
                                startPointX = (startPointX >= dmValuesToSubtract_plate.ColumnCount) ? (dmValuesToSubtract_plate.ColumnCount - 1) : startPointX;

                                int startPointY =
                                    Convert.ToInt32(-(startPointR) * Math.Sin(ptPolCurrPoint.Phi) +
                                                     sunCenterPoint.Y);
                                startPointY = (startPointY < 0) ? 0 : startPointY;
                                startPointY = (startPointY >= dmValuesToSubtract_plate.RowCount) ? (dmValuesToSubtract_plate.RowCount - 1) : startPointY;

                                double dVal = dmValuesToSubtract_plate[row, col] - dmValuesToSubtract_plate[startPointY, startPointX];

                                //scaledSubtractionValue = startPointVal +
                                //                         (currDistance - startPointX)*(endPointVal - startPointVal)/
                                //                         (endPointX - startPointX);
                                scaledSubtractionValue = startPointVal + dVal;
                            }
                            else
                            {
                                scaledSubtractionValue = 1.0d -
                                                            currentLinearKoeff *
                                                            (1.0d -
                                                             DataAnalysisStatic.PolynomeValue(approxPolyKoeffs6,
                                                                 currDistance / currentLinearKoeff));
                            }

                            if (double.IsNaN(scaledSubtractionValue)) return 0.0d;
                            if (scaledSubtractionValue > 1.0d) return 1.0d;
                            if (scaledSubtractionValue < 0.0d) return 0.0d;

                            return scaledSubtractionValue;
                        }));


                dmDataToSubtract3rdOrder = (DenseMatrix)dmDataToSubtract3rdOrder.PointwiseMultiply(dmMask);
                dmDataToSubtract6thOrder = (DenseMatrix)dmDataToSubtract6thOrder.PointwiseMultiply(dmMask);

                BGWorkerReport("данные для вычитания засветки рассчитаны");



                //dmReversed.MapIndexedInplace(new Func<int, int, double, double>((row, col, dVal) =>
                //{
                //    double retVal = dVal - dmDataToSubtract[row, col];
                //    if (retVal < 0.0d) retVal = 0.0d;
                //    return retVal;
                //}));
                dmReversed3rdOrder = dmReversed3rdOrder - dmDataToSubtract3rdOrder;
                dmReversed6thOrder = dmReversed6thOrder - dmDataToSubtract6thOrder;

                dmReversed3rdOrder.MapInplace(dval => (dval < 0.0d) ? (0.0d) : (dval));
                dmReversed6thOrder.MapInplace(dval => (dval < 0.0d) ? (0.0d) : (dval));

                dmReversed3rdOrder = (DenseMatrix)dmReversed3rdOrder.PointwiseMultiply(dmMask);
                dmReversed6thOrder = (DenseMatrix)dmReversed6thOrder.PointwiseMultiply(dmMask);

                //double dataMinValue = dmReversed.Values.Min();
                //double dataMaxValue = dmReversed.Values.Max();
                //double dataValuesRange = dataMaxValue - dataMinValue;


                // странно. а где компенсация величины пиков за счет "прижимания" к верхней границе?..
                // надо восстановить.
                // minDataValue - минимальное значение, которое вроде бы вычитается
                // при вычитании этого значения ничего не масштабируем.
                // При вычитании мЕньших - масштабируем во столько раз, во сколько меньше вычитаем
                Func<DenseMatrix, DenseMatrix, DenseMatrix> ProcessResult = (dmResult, dmSubtracting) =>
                {
                    dmResult.MapIndexedInplace((r, c, dVal) =>
                    {
                        double subtractedValue = dmSubtracting[r, c];
                        double koeff = (minSunburnGrIxValue - minDataValue) / (minSunburnGrIxValue - subtractedValue);
                        koeff = (double.IsNaN(koeff)) ? (1.0d) : (koeff);
                        return dVal * koeff;
                    });

                    DenseVector dvValuesFilteredNaNs = DataAnalysisStatic.DataVectorizedExcludingValues(dmResult, double.NaN);

                    DescriptiveStatistics stats = new DescriptiveStatistics(dvValuesFilteredNaNs);
                    // удалим значения за пределами 3s
                    dmResult.MapInplace(dVal =>
                    {
                        if (stats.Mean - dVal > stats.StandardDeviation * 3.0d)
                        {
                            return 0.0d;
                        }
                        else if (dVal > minSunburnGrIxValue)
                        {
                            return minSunburnGrIxValue;
                        }
                        else if (double.IsNaN(dVal))
                        {
                            return 0.0d;
                        }
                        else return dVal;
                    });
                    // dmResult.MapInplace(dVal => ((dVal < 0.0d) || (dVal > minSunburnGrIxValue)) ? (0.0d) : (dVal));
                    dmResult.MapInplace(dVal => (dVal < 0.0d) ? (0.0d) : (dVal));

                    return dmResult;
                };

                dmReversed3rdOrder = ProcessResult(dmReversed3rdOrder, dmDataToSubtract3rdOrder);
                dmReversed6thOrder = ProcessResult(dmReversed6thOrder, dmDataToSubtract6thOrder);




                if (!isCalculatingUsingBgWorker)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "the image circled mask radius: " + imgP.imageRD.DRadius.ToString());
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "the image circled mask center: " + imgP.imageRD.pointfCircleCenter().ToString());
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "the sun center: " + sunCenterPoint.ToString());
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "the sun radius: " + sunRadius.ToString());

                    #region //obsolete
                    //theLogWindow = ServiceTools.LogAText(theLogWindow,
                    //    "average sky distance to sun margin: " + averageSkyDistance);
                    //theLogWindow = ServiceTools.LogAText(theLogWindow,
                    //    "average sky data value at average sky distance: " + averageSkyDataValue);

                    //theLogWindow = ServiceTools.LogAText(theLogWindow,
                    //    "resulting data min value = " + dataMinValue.ToString("e"));
                    //theLogWindow = ServiceTools.LogAText(theLogWindow,
                    //    "resulting data max value = " + dataMaxValue.ToString("e"));
                    #endregion //obsolete
                }

                #region // данные и Matlab-скрипт для визуализации
                // BGWorkerReport("формирование данных и скрипта для отрисовки картинок в Matlab");
                //if (verbosityLevel > 0)
                //{
                //    //double H = 1.0d - averageSkyDataValue;
                //    //double k = Math.Abs(theGradIndicativeValue);
                //    //double f = 1.0d / 3.0d;
                //    //double p = (H / (k * f * averageSkyDistance) - 1) / (f * averageSkyDistance);

                //    //int leftSunMargin = Convert.ToInt32(sunCenterPoint.X - sunRadius);
                //    //int rightSunMargin = Convert.ToInt32(sunCenterPoint.X + sunRadius);
                //    //int leftAverageSkyMargin = Convert.ToInt32(leftSunMargin - averageSkyDistance);
                //    //int rightAverageSkyMargin = Convert.ToInt32(rightSunMargin + averageSkyDistance);
                //    //int leftOneThirdToAverageSkyMargin = Convert.ToInt32(leftSunMargin - f * averageSkyDistance);
                //    //int rightOneThirdToAverageSkyMargin = Convert.ToInt32(rightSunMargin + f * averageSkyDistance);
                //    //double leftA = 1.0 - Math.Abs(theGradIndicativeValue) * leftSunMargin;
                //    //double rightA = 1.0 + Math.Abs(theGradIndicativeValue) * rightSunMargin;
                //    //int topSunMargin = Convert.ToInt32(sunCenterPoint.Y - sunRadius);
                //    //int bottomSunMargin = Convert.ToInt32(sunCenterPoint.Y + sunRadius);
                //    //int topAverageSkyMargin = Convert.ToInt32(topSunMargin - averageSkyDistance);
                //    //int bottomAverageSkyMargin = Convert.ToInt32(bottomSunMargin + averageSkyDistance);
                //    //int topOneThirdToAverageSkyMargin = Convert.ToInt32(topSunMargin - f * averageSkyDistance);
                //    //int bottomOneThirdToAverageSkyMargin = Convert.ToInt32(bottomSunMargin + f * averageSkyDistance);
                //    //double topA = 1.0 - Math.Abs(theGradIndicativeValue) * topSunMargin;
                //    //double bottomA = 1.0 + Math.Abs(theGradIndicativeValue) * bottomSunMargin;


                //    string matlabScript = "clear;" + Environment.NewLine;
                //    matlabScript += "origDataMatrix = ncread('" + currentDirectory + randomFileName +
                //                    "_orig.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "resultDataMatrix = ncread('" + currentDirectory + randomFileName +
                //                    "_res.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "skyDataSunburnProfileDetectionMatrix = ncread('" + currentDirectory +
                //                    randomFileName +
                //                    "_SunburnProfileDetection.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "sunMarginDistanceDataMatrix = ncread('" + currentDirectory + randomFileName +
                //                    "_SunMarginDistance.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "decartDistributionSunburnMinimums = ncread('" + currentDirectory + randomFileName +
                //                    "_SunburnProfileMinimumsDecart.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "angleDistributionSunburnDetectionData = ncread('" + currentDirectory +
                //                    randomFileName +
                //                    "_SunburnProfileMinimums.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "angleDistributionSunburn = ncread('" + currentDirectory + randomFileName +
                //                    "_SunburnProfilePolar.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "dataToSubtract = ncread('" + currentDirectory + randomFileName +
                //                    "_TheDataToSubtract_plate.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "dataToSubtractApproximated = ncread('" + currentDirectory + randomFileName +
                //                    "_TheDataToSubtractApproximated.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "[rSpace, phiSpace, vals] = find(angleDistributionSunburnDetectionData);" +
                //                    Environment.NewLine;

                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    matlabScript += "scatter(phiSpace, vals, 'bo', 'LineWidth', 2);" + Environment.NewLine;
                //    matlabScript +=
                //        "title('GrIx local minima distribution on angle around sunburn center', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'GrIx', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, '\\phi, the angle around sunburn center, rad', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img01.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;

                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    matlabScript += "scatter(rSpace, vals, 'bo', 'LineWidth',2);" + Environment.NewLine;
                //    matlabScript +=
                //        "title('GrIx local minima distribution on distance to the sunburn margin', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'GrIx', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, 'r, distance to sunburn margin', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img02.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;

                //    matlabScript += "dataToSubtractApproximated(dataToSubtractApproximated == 0) = NaN;" +
                //                    Environment.NewLine;
                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    matlabScript += "mesh(dataToSubtractApproximated);" + Environment.NewLine;
                //    matlabScript +=
                //        "title('GrIx values to subtract during the sunburn effect suppression', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "zlabel(gca, 'GrIx', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'x, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, 'y, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img03.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;


                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    matlabScript += "mesh(angleDistributionSunburn);" + Environment.NewLine;
                //    matlabScript +=
                //        "title('GrIx minima, distribution using polar coordinates system', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "zlabel(gca, 'GrIx', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'r, distance to sunburn margin, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, '\\phi, the angle around sunburn center, rad', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img04.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;

                //    //matlabScript += "skyDataSunburnProfileDetectionMatrixPolar = ncread('" + currentDirectory + randomFileName +
                //    //                "_PolarSystemmedData.nc','dataMatrix');" + Environment.NewLine;
                //    matlabScript += "origDataVect = origDataMatrix(1:end," +
                //                    Convert.ToInt32(Math.Round(sunCenterPoint.Y, 0)) + ");" + Environment.NewLine;
                //    matlabScript += "resultDataVect = resultDataMatrix(1:end," +
                //                    Convert.ToInt32(Math.Round(sunCenterPoint.Y, 0)) + ");" + Environment.NewLine;
                //    matlabScript += "xSpace = 1:1:" + dmReversed.ColumnCount + ";" + Environment.NewLine;
                //    //matlabScript += "markersValues(xSpace) = " + averageSkyDataValue.ToString("e").Replace(",", ".") + ";" +
                //    //                Environment.NewLine;
                //    //matlabScript += "markersValues(xSpace <= " + leftAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "markersValues(xSpace >= " + rightAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "markersValues(xSpace == " + leftOneThirdToAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "markersValues(xSpace == " + rightOneThirdToAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "approxFunc1(xSpace) = " + leftA.ToString("e").Replace(",", ".") + " + " +
                //    //                Math.Abs(theGradIndicativeValue).ToString("e").Replace(",", ".") + " * xSpace;" +
                //    //                Environment.NewLine;
                //    //matlabScript += "approxFunc1(xSpace >= " + leftSunMargin + ") = 1.0;" + Environment.NewLine;
                //    //matlabScript += "approxFunc2(xSpace) = " + rightA.ToString("e").Replace(",", ".") + " - " +
                //    //                Math.Abs(theGradIndicativeValue).ToString("e").Replace(",", ".") + " * xSpace;" +
                //    //                Environment.NewLine;
                //    //matlabScript += "approxFunc2(xSpace <= " + rightSunMargin + ") = 1.0;" + Environment.NewLine;
                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    //matlabScript +=
                //    //    "plot(xSpace, origDataVect, xSpace, resultDataVect, xSpace, markersValues, xSpace, approxFunc1, xSpace, approxFunc2);" +
                //    //    Environment.NewLine;
                //    matlabScript += "plot(xSpace, origDataVect, xSpace, resultDataVect);" + Environment.NewLine;
                //    matlabScript +=
                //        "title('GrIx values at the fixed X slice. Before and after sunburn suppression', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, 'y, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'GrIx value', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img05.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;

                //    matlabScript += "origDataVectOrtho = origDataMatrix(" +
                //                    Convert.ToInt32(Math.Round(sunCenterPoint.X, 0)) + ", 1:end);" + Environment.NewLine;
                //    matlabScript += "resultDataVectOrtho = resultDataMatrix(" +
                //                    Convert.ToInt32(Math.Round(sunCenterPoint.X, 0)) + ", 1:end);" + Environment.NewLine;
                //    matlabScript += "xSpaceOrtho = 1:1:" + dmReversed.RowCount + ";" + Environment.NewLine;
                //    //matlabScript += "markersValuesOrtho(xSpaceOrtho) = " + averageSkyDataValue.ToString("e").Replace(",", ".") + ";" +
                //    //                Environment.NewLine;
                //    //matlabScript += "markersValuesOrtho(xSpaceOrtho <= " + topAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "markersValuesOrtho(xSpaceOrtho >= " + bottomAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "markersValuesOrtho(xSpaceOrtho == " + topOneThirdToAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "markersValuesOrtho(xSpaceOrtho == " + bottomOneThirdToAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "approxFunc3(xSpaceOrtho) = " + topA.ToString("e").Replace(",", ".") + " + " +
                //    //                Math.Abs(theGradIndicativeValue).ToString("e").Replace(",", ".") + " * xSpaceOrtho;" +
                //    //                Environment.NewLine;
                //    //matlabScript += "approxFunc3(xSpaceOrtho >= " + topSunMargin + ") = 1.0;" + Environment.NewLine;
                //    //matlabScript += "approxFunc4(xSpaceOrtho) = " + bottomA.ToString("e").Replace(",", ".") + " - " +
                //    //                Math.Abs(theGradIndicativeValue).ToString("e").Replace(",", ".") + " * xSpaceOrtho;" +
                //    //                Environment.NewLine;
                //    //matlabScript += "approxFunc4(xSpaceOrtho <= " + bottomSunMargin + ") = 1.0;" + Environment.NewLine;
                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    //matlabScript +=
                //    //    "plot(xSpaceOrtho, origDataVectOrtho, xSpaceOrtho, resultDataVectOrtho, xSpaceOrtho, markersValuesOrtho, xSpaceOrtho, approxFunc3, xSpaceOrtho, approxFunc4);" +
                //    //    Environment.NewLine;
                //    matlabScript += "plot(xSpaceOrtho, origDataVectOrtho, xSpaceOrtho, resultDataVectOrtho);" +
                //                    Environment.NewLine;
                //    matlabScript +=
                //        "title('GrIx values at the fixed Y slice. Before and after sunburn suppression', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, 'x, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'GrIx value', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img06.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;

                //    //matlabScript += "multSpace = 1:1:" + Convert.ToInt32(averageSkyDistance) + ";" + Environment.NewLine;
                //    //matlabScript += "multFunc(multSpace) = 1.0 + " + p.ToString("e").Replace(",", ".") + " * multSpace;" + Environment.NewLine;
                //    //matlabScript += "multFunc(multSpace > " + Convert.ToInt32(averageSkyDistance * f) + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "multFunc2(multSpace) = " + H.ToString("e").Replace(",", ".") + " ./ (" +
                //    //                k.ToString("e").Replace(",", ".") + " * multSpace);" + Environment.NewLine;
                //    //matlabScript += "multFunc2(multSpace < " + Convert.ToInt32(averageSkyDistance * f) + ") = 0.0;" + Environment.NewLine;
                //    //matlabScript += "figure;" + Environment.NewLine;
                //    //matlabScript += "plot(multSpace, multFunc, multSpace, multFunc2);" + Environment.NewLine;
                //    matlabScript +=
                //        "origSunburnProfileSkyDataMatrixReshaped = reshape(skyDataSunburnProfileDetectionMatrix, 1, []);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "sunMarginDistanceDataMatrixReshaped = reshape(sunMarginDistanceDataMatrix, 1, []);" +
                //        Environment.NewLine;

                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    matlabScript += "origSkyDataMatrixReshaped = reshape(origDataMatrix, 1, []);" + Environment.NewLine;
                //    matlabScript += "scatter(sunMarginDistanceDataMatrixReshaped, origSkyDataMatrixReshaped);" +
                //                    Environment.NewLine;
                //    matlabScript +=
                //        "title('Original GrIx distribution on distance to sunburn margin', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, 'Distance to sunburn margin, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'GrIx value', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img07.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;

                //    matlabScript += "xMin = min(sunMarginDistanceDataMatrixReshaped);" + Environment.NewLine;
                //    matlabScript += "xMax = max(sunMarginDistanceDataMatrixReshaped);" + Environment.NewLine;
                //    matlabScript += "xq = xMin:(xMax-xMin)/100:xMax;" + Environment.NewLine;
                //    matlabScript += "i = (origSunburnProfileSkyDataMatrixReshaped > 0.0);" + Environment.NewLine;
                //    matlabScript +=
                //        "origSunburnProfileSkyDataMatrixReshaped = origSunburnProfileSkyDataMatrixReshaped(1, i);" +
                //        Environment.NewLine;
                //    matlabScript += "sunMarginDistanceDataMatrixReshaped = sunMarginDistanceDataMatrixReshaped(1, i);" +
                //                    Environment.NewLine;


                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    matlabScript += "origSkyDataMatrixReshaped = reshape(origDataMatrix, 1, []);" + Environment.NewLine;
                //    matlabScript +=
                //        "scatter(sunMarginDistanceDataMatrixReshaped, origSunburnProfileSkyDataMatrixReshaped);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "title('Original GrIx distribution on distance to sunburn margin', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, 'Distance to sunburn margin, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'GrIx value', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img08.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;


                //    matlabScript += "csvdata = importdata('" + currentDirectory + randomFileName +
                //                    "_dataDistribution.csv', ';');" + Environment.NewLine;
                //    matlabScript += "distributionDataX = csvdata(:,1);" + Environment.NewLine;
                //    matlabScript += "distributionData = csvdata(:,2);" + Environment.NewLine;
                //    //matlabScript += "interpolatedFuncMLkoeffs = polyfit(distributionDataX, distributionData, 5);" +
                //    //                Environment.NewLine;
                //    //matlabScript += "interpolatedFuncML = polyval(interpolatedFuncMLkoeffs, distributionDataX);" + Environment.NewLine;
                //    //matlabScript += "" + Environment.NewLine;
                //    string matlabSubstr = "";
                //    for (int i = 0; i <= polynomeOrder; i++)
                //    {
                //        int l = polynomeOrder - i;
                //        matlabSubstr += approxPolyKoeffs[l].ToString("e").Replace(",", ".");
                //        if (l > 0) matlabSubstr += ",";
                //    }
                //    matlabScript += "interpolatedFuncKoeffs = [" + matlabSubstr + "];" + Environment.NewLine;

                //    matlabScript += "interpolatedFunc = polyval(interpolatedFuncKoeffs, distributionDataX);;" +
                //                    Environment.NewLine;

                //    matlabScript += "fig = figure;" + Environment.NewLine;
                //    matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
                //    //matlabScript += "scatter(sunMarginDistanceDataMatrixReshaped, origSunburnProfileSkyDataMatrixReshaped, 'x');" + Environment.NewLine;
                //    matlabScript += "hold on;" + Environment.NewLine;
                //    matlabScript += "plot(distributionDataX, interpolatedFunc, 'g', 'LineWidth', 3);" +
                //                    Environment.NewLine;
                //    matlabScript += "plot(distributionDataX, distributionData, 'r', 'LineWidth', 3);" +
                //                    Environment.NewLine;
                //    //matlabScript += "plot(distributionDataX, interpolatedFuncML, 'y', 'LineWidth', 3);" + Environment.NewLine;
                //    matlabScript += "hold off;" + Environment.NewLine;
                //    matlabScript +=
                //        "title('GrIx minima distribution on distance to sunburn margin. With approximation function.', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "xlabel(gca, 'Distance to sunburn margin, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    matlabScript +=
                //        "ylabel(gca, 'GrIx value', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" +
                //        Environment.NewLine;
                //    //matlabScript += "export_fig '" + currentDirectory + randomFileName + "_img09.pdf';" +
                //    //                Environment.NewLine;
                //    //matlabScript += "close(fig);" + Environment.NewLine;

                //    //matlabScript += "decartDistributionSunburnMinimums(decartDistributionSunburnMinimums == 0) = NaN;" + Environment.NewLine;
                //    //matlabScript += "figure;" + Environment.NewLine;
                //    //matlabScript += "mesh(decartDistributionSunburnMinimums);" + Environment.NewLine;

                //    #region // УСТАРЕЛО побочные вычисления, не используется
                //    //DenseMatrix dmTestReversedInsideSkyCircle = (DenseMatrix)dmReversed.Clone();
                //    //dmTestReversedInsideSkyCircle.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                //    //{
                //    //    double currDist = dmDistanceToSunMargin[row, column];
                //    //    if ((currDist >= averageSkyDistance) || (currDist == 0.0d) || (dmMask[row, column] == 0.0d))
                //    //        return 0.0d;
                //    //    else return val;
                //    //}));
                //    //double dataMinValueInsideSkyCircle = dmTestReversedInsideSkyCircle.Values.Min();
                //    //double dataMaxValueInsideSkyCircle = dmTestReversedInsideSkyCircle.Values.Max();
                //    #endregion // УСТАРЕЛО побочные вычисления, не используется

                //    ServiceTools.logToTextFile(currentDirectory + randomFileName + "_MatlabScript.m", matlabScript,
                //                            false);

                //}
                #endregion // данные и Matlab-скрипт для визуализации

                //dmReversed.MapInplace(new Func<double, double>(val => val - dataMinValueInsideSkyCircle - 1.0d));


                #region // НЕ РАБОТАЕТ тестировал тупое вычитание тренда локальных минимумов
                //DenseMatrix dmReversedTest1 = (DenseMatrix)dmReversed.Clone();
                //dmReversedTest1.MapIndexedInplace(new Func<int, int, double, double>((row, column, dVal) =>
                //{
                //    double retVal = dVal - dmValuesToSubtract[row, column];
                //    retVal *= dmMaskCircled[row, column];
                //    return retVal;
                //}));
                //double testingMinValue = dmReversedTest1.Values.Min();
                //dmReversedTest1.MapIndexedInplace(new Func<int, int, double, double>((row, column, dVal) =>
                //{
                //    double retVal = dVal - testingMinValue;
                //    retVal *= dmMaskCircled[row, column];
                //    return retVal;
                //}));
                //if (!isCalculatingUsingBgWorker)
                //{
                //    ImageConditionAndDataRepresentingForm theFormTest1 =
                //        ServiceTools.RepresentDataFromDenseMatrix(dmReversedTest1,
                //            "testing the local minimums trend subtraction");
                //}
                #endregion // НЕ РАБОТАЕТ тестировал тупое вычитание тренда локальных минимумов

                #endregion анализ засветки и компенсация


                if (verbosityLevel > 1)
                {
                    #region // obsolete
                    //ImageConditionAndDataRepresentingForm restoredDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmReversed,
                    //    "finally restored GrIx data", false, false, 0.0d, 1.0d, false);
                    //ImageConditionAndDataRepresentingForm originalDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmProcessingData,
                    //    "original GrIx data", false, false, 0.0d, 1.0d, false);
                    //ImageConditionAndDataRepresentingForm dataToSubtractForm =
                    //    ServiceTools.RepresentDataFromDenseMatrix(dmDataToSubtract, "the approximated data to subtract", false, false, 0.0d, 1.0d, false);
                    //dataToSubtractForm.SaveData(currentDirectory + randomFileName + "_TheDataToSubtractApproximated.nc", true);
                    //restoredDataForm.SaveData(currentDirectory + randomFileName + "_res.nc", true);
                    //originalDataForm.SaveData(currentDirectory + randomFileName + "_orig.nc", true);
                    //
                    ////MLApp.MLApp ML = new MLApp.MLApp();
                    ////ML.Visible = 0;
                    ////ML.Execute("run('" + currentDirectory + randomFileName + "_MatlabScript.m" + "');");
                    //
                    ////originalDataForm.Close();
                    //originalDataForm.Dispose();
                    ////dataToSubtractForm.Close();
                    //dataToSubtractForm.Dispose();
                    ////restoredDataForm.Close();
                    ////restoredDataForm.Dispose();
                    #endregion // obsolete

                    dmDataToSubtract3rdOrder.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                          "_TheDataToSubtract-3rdOrder-Approximated.nc");
                    dmReversed3rdOrder.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_res3rdOrder.nc");
                    dmDataToSubtract6thOrder.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                          "_TheDataToSubtract-6thOrder-Approximated.nc");
                    dmReversed3rdOrder.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_res-6thOrder.nc");
                    dmProcessingData.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_orig.nc");
                }

                BGWorkerReport("формирование изображений для визуализации");

                cloudSkySeparationValue = theStdDevMarginValueDefiningSkyCloudSeparation_SunSuppressed;

                ColorScheme skyCloudColorScheme =
                    ColorScheme.InversedBinaryCloudSkyColorScheme(
                        cloudSkySeparationValue, Math.Min(dmReversed6thOrder.Values.Min(), dmReversed3rdOrder.Values.Min()),
                        Math.Max(dmReversed6thOrder.Values.Max(), dmReversed3rdOrder.Values.Max()));

                Image<Bgr, Byte> previewImage6thOrder = ImageProcessing.evalResultColoredWithFixedDataBounds(
                    dmReversed6thOrder, maskImage, skyCloudColorScheme, dmReversed6thOrder.Values.Min(),
                    dmReversed6thOrder.Values.Max());
                previewImage6thOrder.Save(currentDirectory + randomFileName + "-result-6thOrder.jpg");
                Image<Bgr, Byte> previewImage3rdOrder = ImageProcessing.evalResultColoredWithFixedDataBounds(
                    dmReversed3rdOrder, maskImage, skyCloudColorScheme, dmReversed3rdOrder.Values.Min(),
                    dmReversed3rdOrder.Values.Max());
                previewImage3rdOrder.Save(currentDirectory + randomFileName + "-result-3rdOrder.jpg");


                cloudCounter = previewImage6thOrder.CountNonzero()[1];
                skyCounter = maskImage.CountNonzero()[0] - cloudCounter;
                CloudCover = (double)cloudCounter / (double)(skyCounter + cloudCounter);

                int cloudCounter3 = previewImage3rdOrder.CountNonzero()[1];
                int skyCounter3 = maskImage.CountNonzero()[0] - cloudCounter;
                double CloudCover3 = (double)cloudCounter3 / (double)(skyCounter3 + cloudCounter3);

                string strResult = "3rd order approx: cloud cover = " + CloudCover3.ToString() + Environment.NewLine;
                strResult += "6th order approx: cloud cover = " + CloudCover.ToString() + Environment.NewLine;
                ServiceTools.logToTextFile(currentDirectory + randomFileName + "-results.txt", strResult, true);

                dmSkyIndexData = dmReversed6thOrder;
                localPreviewBitmap = previewImage6thOrder.Bitmap;


                if (Math.Abs(CloudCover - CloudCoverWithoutSunSuppression) > 0.7d)
                //как-то очень неправильно определилось солнце и засветка
                //возьмем результат, посчитанный без подавления засветки
                {
                    theSunSuppressionSchemeApplicable = false;

                    theLogWindow =
                        ServiceTools.LogAText(theLogWindow, "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + Environment.NewLine +
                                              "Слишком нестабильный результат с подавлением засветки. Используем алгоритм без подавления. Case 02" +
                                              Environment.NewLine + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    break;
                }

                break;
            }



            if (!theSunSuppressionSchemeApplicable)
            {
                // исследуем вопрос минимальных значений GrIx c усреднением по крупным ячейкам
                DenseMatrix dmReversed = (DenseMatrix)dmProcessingData.Clone();

                if (verbosityLevel > 1)
                {
                    dmReversed.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_res.nc");
                    //dmProcessingData.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_orig.nc");
                }

                cloudSkySeparationValue = theStdDevMarginValueDefiningSkyCloudSeparation;
                ColorScheme skyCloudColorScheme = ColorScheme.InversedBinaryCloudSkyColorScheme(cloudSkySeparationValue, 0.0d, 1.0d);
                ColorSchemeRuler skyCloudRuler = new ColorSchemeRuler(skyCloudColorScheme, 0.0d, 1.0d);
                Image<Bgr, Byte> previewImage = ImageProcessing.evalResultColoredWithFixedDataBounds(dmProcessingData, maskImage, skyCloudColorScheme, 0.0d, 1.0d);
                cloudCounter = previewImage.CountNonzero()[1];
                skyCounter = maskImage.CountNonzero()[0] - cloudCounter;
                CloudCover = (double)cloudCounter / (double)(skyCounter + cloudCounter);

                dmSkyIndexData = dmReversed;
                localPreviewBitmap = previewImage.Bitmap;
                //currentSunDiskCondition = SunDiskConditions.Cloudy;
            }
        }










        public RoundData DetectSunWithSerieOfArcsUsingGPSdatum(ImageProcessing imgP, DenseMatrix dmGrIx, string currentDirectory = "",
            string randomFileName = "")
        {
            DenseMatrix dmSunburnData = imgP.eval("Y", null);

            DenseVector dvGrIxDataEqualsOne = DataAnalysisStatic.DataVectorizedWithCondition(dmSunburnData, dval => dval >= minSunburnYValue);
            if (dvGrIxDataEqualsOne == null) return RoundData.nullRoundData();
            if (dvGrIxDataEqualsOne.Values.Sum() < imgP.significantMaskImageBinary.CountNonzero()[0] * minSunAreaPart) return RoundData.nullRoundData();

            Image<Gray, Byte> maskImageCircled85 = imgP.imageSignificantMaskCircled(dSunDetectorArcedCropFactor * 100.0d);
            DenseMatrix dmGrIxData =
                (DenseMatrix)dmGrIx.PointwiseMultiply(ImageProcessing.DenseMatrixFromImage(maskImageCircled85));
            DenseVector dvGrIxDataToStat = DataAnalysisStatic.DataVectorizedExcludingValues(dmGrIxData, 0.0d);
            
            dvGrIxDataToStat = DataAnalysisStatic.DataVectorizedWithCondition(dvGrIxDataToStat, dval => (dval <= minSunburnGrIxValue));


            DenseMatrix dmSunDetectionDataByAnsamble = DenseMatrix.Create(dmGrIx.RowCount, dmGrIx.ColumnCount,
                (r, c) => 0.0d);
            List<RoundData> CenterPoints = new List<RoundData>();
            int startedBGworkers = 0;
            List<bool> finishedBGworkers = new List<bool>();
            List<double> quantilesBy10 = new List<double>();



            for (int i = 10; i <= 82; i += 3)
            {
                quantilesBy10.Add(Statistics.Percentile(dvGrIxDataToStat, i));
                //startedBGworkers.Add(false);
                finishedBGworkers.Add(false);
            }

            DenseVector quantileWeights = DenseVector.Create(quantilesBy10.Count, i =>
            {
                double currWeight = ((quantilesBy10[i] - dvGrIxDataToStat.Values.Min()) /
                                     (dvGrIxDataToStat.Values.Max() - dvGrIxDataToStat.Values.Min()));
                currWeight = 1.0d / currWeight;
                return currWeight;
            });
            double weightsSum = quantileWeights.Values.Sum();
            quantileWeights = (DenseVector)quantileWeights.Divide(weightsSum);



            RunWorkerCompletedEventHandler currWorkCompletedHandler =
                delegate (object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    DenseMatrix dmResSunDetectionPartial = (DenseMatrix)currentBGWResults[0];
                    RoundData retRD = (RoundData)currentBGWResults[1];
                    int returningBGWthreadID = (int)currentBGWResults[2];

                    if (verbosityLevel > 1)
                    {
                        //NetCDFoperations.SaveDataToFile(dmResSunDetectionPartial,
                        //    currentDirectory + randomFileName + "_SunDetectionPartialData-" + returningBGWthreadID + ".nc", null, true);
                        dmResSunDetectionPartial.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                      "_SunDetectionPartialData-" + returningBGWthreadID +
                                                                      ".nc");
                    }

                    dmSunDetectionDataByAnsamble = dmSunDetectionDataByAnsamble + dmResSunDetectionPartial;
                    if (!retRD.IsNull) CenterPoints.Add(retRD);
                    finishedBGworkers[returningBGWthreadID] = true;
                    startedBGworkers--;
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "finished sun detection thread " + (returningBGWthreadID + 1) + " of " + quantilesBy10.Count);
                };


            DoWorkEventHandler currDoWorkHandler = delegate (object currBGWsender, DoWorkEventArgs args)
            {
                object[] currBGWarguments = (object[])args.Argument;
                string fileFullName = (string)currBGWarguments[0];
                //Image currImageToAdd = Image.FromFile(fileFullName);
                //currImageToAdd = ImageResizer(currImageToAdd, defaultMaxImageSize);
                double currQtle = (double)currBGWarguments[1];
                Dictionary<string, object> properties = (Dictionary<string, object>)currBGWarguments[2];
                Form parentForm = (Form)currBGWarguments[3];
                int currBGWthreadID = (int)currBGWarguments[4];
                ImageProcessing imgPrObj = (ImageProcessing)currBGWarguments[5];
                DenseMatrix dmSrcGrIxData = (DenseMatrix)currBGWarguments[6];

                RoundData rd = DetectSunArced(imgPrObj, dmSrcGrIxData, currentDirectory, randomFileName, currQtle * 0.99d,
                    currQtle * 1.01d);



                Image<Gray, byte> tmpImg = new Image<Gray, byte>(dmGrIx.ColumnCount, dmGrIx.RowCount, new Gray(0));
                if (!rd.IsNull)
                {
                    tmpImg.Draw(new CircleF(rd.pointDCircleCenter().PointF(), (float)rd.DRadius), new Gray(255), 0);
                }
                DenseMatrix tmpDM = ImageProcessing.DenseMatrixFromImage(tmpImg);
                tmpDM = (DenseMatrix)tmpDM.Multiply(quantileWeights[currBGWthreadID]);

                args.Result = new object[] { tmpDM, rd, currBGWthreadID };
            };


            
            for (int i = 0; i < quantilesBy10.Count; i++)
            {
                while (startedBGworkers >= concurrentThreadsLimit)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }


                double qtle = quantilesBy10[i];
                BackgroundWorker currBgw = new BackgroundWorker();
                currBgw.DoWork += currDoWorkHandler;
                currBgw.RunWorkerCompleted += currWorkCompletedHandler;
                object[] BGWargs = new object[] { "", qtle, defaultProperties, ParentForm, i, imgP, dmGrIxData };
                currBgw.RunWorkerAsync(BGWargs);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "started sun detection thread " + (i + 1) + " of " + quantilesBy10.Count);
                startedBGworkers++;
            }



            while (finishedBGworkers.Sum(boolVal => (boolVal) ? ((int)1) : ((int)0)) < finishedBGworkers.Count)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }
            


            if ((!isCalculatingUsingBgWorker) && (verbosityLevel > 0))
            {
                ServiceTools.RepresentDataFromDenseMatrix(dmSunDetectionDataByAnsamble, "dmSunDetectionDataByAnsamble",
                    true, false, 0.0d, dmSunDetectionDataByAnsamble.Values.Max());
            }
            if (verbosityLevel > 1)
            {
                ImageConditionAndDataRepresentingForm f1 =
                    ServiceTools.RepresentDataFromDenseMatrix(dmSunDetectionDataByAnsamble,
                        "dmSunDetectionDataByAnsamble", true, false, 0.0d, dmSunDetectionDataByAnsamble.Values.Max(), false);
                Image<Bgr, byte> sunDetectionImage = new Image<Bgr, byte>(f1.imgData.dataRepresentingImageColored());
                sunDetectionImage.Save(currentDirectory + randomFileName + "_SunDetectionDataByAnsamble.jpg");
                sunDetectionImage = sunDetectionImage.AddWeighted(new Image<Bgr, byte>(imgP.processingBitmap()), 0.6d,
                    0.4d, 0.0d);
                sunDetectionImage.Save(currentDirectory + randomFileName + "_SunDetectionByAnsamble.jpg");
                sunDetectionImage.Dispose();
                f1.Dispose();
                dmSunDetectionDataByAnsamble.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                  "_SunDetectionDataByAnsamble.nc");
            }
            

            dmSunDetectionDataByAnsamble =
                (DenseMatrix)dmSunDetectionDataByAnsamble.Multiply(1.0d / dmSunDetectionDataByAnsamble.Values.Max());
            double sunArea = dmSunDetectionDataByAnsamble.Values.Sum();
            double sunRadius = Math.Sqrt(sunArea / Math.PI);

            DenseMatrix dmSunDetectionDataByAnsambleW = (DenseMatrix)dmSunDetectionDataByAnsamble.Clone();
            dmSunDetectionDataByAnsambleW =
                (DenseMatrix)dmSunDetectionDataByAnsambleW.Divide(dmSunDetectionDataByAnsamble.Values.Sum());

            DenseMatrix dmSunDetectionDataByAnsambleX = DenseMatrix.Create(dmSunDetectionDataByAnsamble.RowCount,
                dmSunDetectionDataByAnsamble.ColumnCount, (r, c) => (double)c);
            double sunCenterX =
                ((DenseMatrix)(dmSunDetectionDataByAnsambleX.PointwiseMultiply(dmSunDetectionDataByAnsambleW))).Values
                    .Sum();
            DenseMatrix dmSunDetectionDataByAnsambleY = DenseMatrix.Create(dmSunDetectionDataByAnsamble.RowCount,
                dmSunDetectionDataByAnsamble.ColumnCount, (r, c) => (double)r);
            double sunCenterY =
                ((DenseMatrix)(dmSunDetectionDataByAnsambleY.PointwiseMultiply(dmSunDetectionDataByAnsambleW))).Values
                    .Sum();

            RoundData sunRD = new RoundData(sunCenterX, sunCenterY, sunRadius);

            if (sunRD.RoundArea / imgP.imageRD.RoundArea < minSunAreaPart / 3.0d)
            {
                return RoundData.nullRoundData();
            }

            if (sunRD.RoundArea / imgP.imageRD.RoundArea > maxSunAreaPart)
            {
                sunRD.DRadius = Math.Sqrt((maxSunAreaPart * imgP.imageRD.RoundArea) / Math.PI);
            }

            if (verbosityLevel > 1)
            {
                Image<Bgr, byte> sunDetectionImage = new Image<Bgr, byte>(imgP.processingBitmap());
                Image<Bgr, byte> sunLocationImage = sunDetectionImage.CopyBlank();
                sunLocationImage.Draw(new CircleF(sunRD.pointfCircleCenter(), (float)sunRD.DRadius), new Bgr(255, 255, 255), 0);
                sunDetectionImage = sunDetectionImage.AddWeighted(sunLocationImage, 0.6d, 0.4d, 0.0d);
                sunDetectionImage.Save(currentDirectory + randomFileName + "_SunDetectionByAnsambleResult.jpg");
                sunDetectionImage.Dispose();
                sunLocationImage.Dispose();
            }

            return sunRD;
        }











        public RoundData DetectSunWithSerieOfArcs(ImageProcessing imgP, DenseMatrix dmGrIx, string currentDirectory = "",
            string randomFileName = "")
        {
            DenseMatrix dmSunburnData = imgP.eval("Y", null);

            //DenseVector dvGrIxDataEqualsOne = DataAnalysis.DataVectorizedWithCondition(dmSunburnData, dval => dval >= 254.0d);
            DenseVector dvGrIxDataEqualsOne = DataAnalysisStatic.DataVectorizedWithCondition(dmSunburnData, dval => dval >= minSunburnYValue);
            if (dvGrIxDataEqualsOne == null) return RoundData.nullRoundData();
            if (dvGrIxDataEqualsOne.Values.Sum() < imgP.significantMaskImageBinary.CountNonzero()[0] * minSunAreaPart) return RoundData.nullRoundData();

            Image<Gray, Byte> maskImageCircled85 = imgP.imageSignificantMaskCircled(dSunDetectorArcedCropFactor * 100.0d);
            DenseMatrix dmGrIxData =
                (DenseMatrix)dmGrIx.PointwiseMultiply(ImageProcessing.DenseMatrixFromImage(maskImageCircled85));
            DenseVector dvGrIxDataToStat = DataAnalysisStatic.DataVectorizedExcludingValues(dmGrIxData, 0.0d);
            //dvGrIxDataToStat = DataAnalysis.DataVectorizedWithCondition(dvGrIxDataToStat, dval => (dval != 1.0d));
            dvGrIxDataToStat = DataAnalysisStatic.DataVectorizedWithCondition(dvGrIxDataToStat, dval => (dval <= minSunburnGrIxValue));


            DenseMatrix dmSunDetectionDataByAnsamble = DenseMatrix.Create(dmGrIx.RowCount, dmGrIx.ColumnCount,
                (r, c) => 0.0d);
            List<RoundData> CenterPoints = new List<RoundData>();
            int startedBGworkers = 0;
            List<bool> finishedBGworkers = new List<bool>();


            #region // obsolete - replaced by abs values split

            //List<double> quantilesBy10 = new List<double>();



            //for (int i = 10; i <= 82; i += 3)
            //{
            //    quantilesBy10.Add(Statistics.Percentile(dvGrIxDataToStat, i));
            //    //startedBGworkers.Add(false);
            //    finishedBGworkers.Add(false);
            //}

            //DenseVector quantileWeights = DenseVector.Create(quantilesBy10.Count, i =>
            //{
            //    double currWeight = ((quantilesBy10[i] - dvGrIxDataToStat.Values.Min()) /
            //                         (dvGrIxDataToStat.Values.Max() - dvGrIxDataToStat.Values.Min()));
            //    currWeight = 1.0d / currWeight;
            //    return currWeight;
            //});
            //double weightsSum = quantileWeights.Values.Sum();
            //quantileWeights = (DenseVector)quantileWeights.Divide(weightsSum);

            #endregion // obsolete - replaced by abs values split




            #region calculate bins

            double minValue = Statistics.Percentile(dvGrIxDataToStat, 5);
            double maxValue = Statistics.Percentile(dvGrIxDataToStat, 95);
            double valuesRange = maxValue - minValue;

            List<Tuple<double, double>> binsBy5perc = new List<Tuple<double, double>>();
            
            for (int i = 10; i <= 90; i += 5)
            {
                binsBy5perc.Add(new Tuple<double, double>(minValue + valuesRange*(i/100.0d),
                    minValue + valuesRange*((i + 5)/100.0d)));
                //binsBy5perc.Add(minValue + valuesRange*(i/100.0d));
                //binsBy10perc.Add(Statistics.Percentile(dvGrIxDataToStat, i));
                //startedBGworkers.Add(false);
                finishedBGworkers.Add(false);
            }

            DenseVector binsWeights = DenseVector.Create(binsBy5perc.Count,
                i => 1.0d/((binsBy5perc[i].Item1 - minValue)/(valuesRange)));
            
            double weightsSum = binsWeights.Values.Sum();
            binsWeights = (DenseVector)binsWeights.Divide(weightsSum);

            #endregion calculate bins





            RunWorkerCompletedEventHandler currWorkCompletedHandler =
                delegate (object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    DenseMatrix dmResSunDetectionPartial = (DenseMatrix)currentBGWResults[0];
                    RoundData retRD = (RoundData)currentBGWResults[1];
                    int returningBGWthreadID = (int)currentBGWResults[2];

                    if (verbosityLevel > 1)
                    {
                        //NetCDFoperations.SaveDataToFile(dmResSunDetectionPartial,
                        //    currentDirectory + randomFileName + "_SunDetectionPartialData-" + returningBGWthreadID + ".nc", null, true);
                        dmResSunDetectionPartial.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                      "_SunDetectionPartialData-" + returningBGWthreadID +
                                                                      ".nc");
                    }

                    dmSunDetectionDataByAnsamble = dmSunDetectionDataByAnsamble + dmResSunDetectionPartial;
                    if (!retRD.IsNull) CenterPoints.Add(retRD);
                    finishedBGworkers[returningBGWthreadID] = true;
                    startedBGworkers--;
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "finished sun detection thread " + (returningBGWthreadID + 1) + " of " + binsBy5perc.Count);
                };


            DoWorkEventHandler currDoWorkHandler = delegate (object currBGWsender, DoWorkEventArgs args)
            {
                object[] currBGWarguments = (object[])args.Argument;
                string fileFullName = (string)currBGWarguments[0];
                //Image currImageToAdd = Image.FromFile(fileFullName);
                //currImageToAdd = ImageResizer(currImageToAdd, defaultMaxImageSize);
                //double currQtle = (double)currBGWarguments[1];
                Tuple<double, double> currRange = (Tuple<double, double>)currBGWarguments[1];
                Dictionary<string, object> properties = (Dictionary<string, object>)currBGWarguments[2];
                Form parentForm = (Form)currBGWarguments[3];
                int currBGWthreadID = (int)currBGWarguments[4];
                ImageProcessing imgPrObj = (ImageProcessing)currBGWarguments[5];
                DenseMatrix dmSrcGrIxData = (DenseMatrix)currBGWarguments[6];

                RoundData rd = DetectSunArced(imgPrObj, dmSrcGrIxData, currentDirectory, randomFileName, currRange.Item1,
                    currRange.Item2);



                Image<Gray, byte> tmpImg = new Image<Gray, byte>(dmGrIx.ColumnCount, dmGrIx.RowCount, new Gray(0));
                if (!rd.IsNull)
                {
                    tmpImg.Draw(new CircleF(rd.pointDCircleCenter().PointF(), (float)rd.DRadius), new Gray(255), 0);
                }
                DenseMatrix tmpDM = ImageProcessing.DenseMatrixFromImage(tmpImg);
                tmpDM = (DenseMatrix)tmpDM.Multiply(binsWeights[currBGWthreadID]);

                args.Result = new object[] { tmpDM, rd, currBGWthreadID };
            };



            #region // не подходит
            //try
            //{
            //    int parallelismDegree = Convert.ToInt32(System.Environment.ProcessorCount / 2);
            //    if (parallelismDegree < 1) parallelismDegree = 1;
            //
            //    Parallel.ForEach<double>(quantilesBy10,
            //        new ParallelOptions() { MaxDegreeOfParallelism = 8 },
            //        qtle =>
            //        {
            //            double currQtle = qtle;
            //            int currBGWthreadID = quantilesBy10.FindIndex(qtl => qtl == currQtle);
            //            theLogWindow = ServiceTools.LogAText(theLogWindow,
            //                "started sun detection thread " + (currBGWthreadID + 1) + " of " + quantilesBy10.Count);
            //
            //            ImageProcessing imgPrObj = imgP;
            //            DenseMatrix dmSrcGrIxData = dmGrIxData.Copy();
            //
            //            RoundData rd = new RoundData()
            //            {
            //                IsNull = true,
            //            };
            //
            //            try
            //            {
            //                rd = DetectSunArced(imgPrObj, dmSrcGrIxData, currentDirectory, randomFileName,
            //                    currQtle * 0.99d, currQtle * 1.01d);
            //            }
            //            catch (Exception ex)
            //            {
            //                rd.IsNull = true;
            //                theLogWindow =
            //                    ServiceTools.LogAText(theLogWindow, "failed DetectSunArced with quantile = " + currQtle + ":" +
            //                                          Environment.NewLine +
            //                                          "exception message: " + ex.Message);
            //            }
            //
            //
            //
            //
            //            Image<Gray, byte> tmpImg = new Image<Gray, byte>(dmGrIx.ColumnCount, dmGrIx.RowCount, new Gray(0));
            //            if (!rd.IsNull)
            //            {
            //                tmpImg.Draw(new CircleF(rd.pointDCircleCenter().PointF(), (float)rd.DRadius), new Gray(255), 0);
            //            }
            //            DenseMatrix tmpDM = ImageProcessing.DenseMatrixFromImage(tmpImg);
            //
            //            tmpDM = (DenseMatrix)tmpDM.Multiply(quantileWeights[currBGWthreadID]);
            //
            //            DenseMatrix dmResSunDetectionPartial = tmpDM;
            //            RoundData retRD = rd;
            //
            //            if (verbosityLevel > 1)
            //            {
            //                dmResSunDetectionPartial.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
            //                                                              "_SunDetectionPartialData-" + currBGWthreadID +
            //                                                              ".nc");
            //            }
            //
            //            dmSunDetectionDataByAnsamble = dmSunDetectionDataByAnsamble + dmResSunDetectionPartial;
            //            if (!retRD.IsNull) CenterPoints.Add(retRD);
            //            theLogWindow = ServiceTools.LogAText(theLogWindow, "finished sun detection thread " + (currBGWthreadID + 1) + " of " + quantilesBy10.Count);
            //        });
            //}
            //catch (AggregateException aex)
            //{
            //    string messages = "";
            //    foreach (Exception innerException in aex.InnerExceptions)
            //    {
            //        messages += innerException.Message + Environment.NewLine;
            //    }
            //    theLogWindow =
            //        ServiceTools.LogAText(theLogWindow,
            //            "ERROR. Failed some DetectSunArced processing. Exception messages: " + Environment.NewLine + messages);
            //}
            #endregion // не подходит



            for (int i = 0; i < binsBy5perc.Count; i++)
            {
                while (startedBGworkers >= concurrentThreadsLimit)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }


                //double qtle = binsBy5perc[i];
                BackgroundWorker currBgw = new BackgroundWorker();
                currBgw.DoWork += currDoWorkHandler;
                currBgw.RunWorkerCompleted += currWorkCompletedHandler;
                //object[] BGWargs = new object[] { "", qtle, defaultProperties, ParentForm, i, imgP, dmGrIxData };
                object[] BGWargs = new object[] { "", binsBy5perc[i], defaultProperties, ParentForm, i, imgP, dmGrIxData };
                currBgw.RunWorkerAsync(BGWargs);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "started sun detection thread " + (i + 1) + " of " + binsBy5perc.Count);
                startedBGworkers++;
            }



            while (finishedBGworkers.Sum(boolVal => (boolVal) ? ((int)1) : ((int)0)) < finishedBGworkers.Count)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }






            if ((!isCalculatingUsingBgWorker) && (verbosityLevel > 0))
            {
                ServiceTools.RepresentDataFromDenseMatrix(dmSunDetectionDataByAnsamble, "dmSunDetectionDataByAnsamble",
                    true, false, 0.0d, dmSunDetectionDataByAnsamble.Values.Max());
            }
            if (verbosityLevel > 1)
            {
                ImageConditionAndDataRepresentingForm f1 =
                    ServiceTools.RepresentDataFromDenseMatrix(dmSunDetectionDataByAnsamble,
                        "dmSunDetectionDataByAnsamble", true, false, 0.0d, dmSunDetectionDataByAnsamble.Values.Max(), false);
                Image<Bgr, byte> sunDetectionImage = new Image<Bgr, byte>(f1.imgData.dataRepresentingImageColored());
                //f1.imgData.dataRepresentingImageColored().Save(currentDirectory + randomFileName + "_SunDetectionDataByAnsamble.jpg", ImageFormat.Jpeg);
                sunDetectionImage.Save(currentDirectory + randomFileName + "_SunDetectionDataByAnsamble.jpg");
                sunDetectionImage = sunDetectionImage.AddWeighted(new Image<Bgr, byte>(imgP.processingBitmap()), 0.6d,
                    0.4d, 0.0d);
                sunDetectionImage.Save(currentDirectory + randomFileName + "_SunDetectionByAnsamble.jpg");
                sunDetectionImage.Dispose();
                f1.Dispose();
                //NetCDFoperations.SaveDataToFile(dmSunDetectionDataByAnsamble,
                //    currentDirectory + randomFileName + "_SunDetectionDataByAnsamble.nc", null, true);
                dmSunDetectionDataByAnsamble.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                  "_SunDetectionDataByAnsamble.nc");
            }




            dmSunDetectionDataByAnsamble =
                (DenseMatrix)dmSunDetectionDataByAnsamble.Multiply(1.0d / dmSunDetectionDataByAnsamble.Values.Max());
            double sunArea = dmSunDetectionDataByAnsamble.Values.Sum();
            double sunRadius = Math.Sqrt(sunArea / Math.PI);

            DenseMatrix dmSunDetectionDataByAnsambleW = (DenseMatrix)dmSunDetectionDataByAnsamble.Clone();
            dmSunDetectionDataByAnsambleW =
                (DenseMatrix)dmSunDetectionDataByAnsambleW.Divide(dmSunDetectionDataByAnsamble.Values.Sum());

            DenseMatrix dmSunDetectionDataByAnsambleX = DenseMatrix.Create(dmSunDetectionDataByAnsamble.RowCount,
                dmSunDetectionDataByAnsamble.ColumnCount, (r, c) => (double)c);
            double sunCenterX =
                ((DenseMatrix)(dmSunDetectionDataByAnsambleX.PointwiseMultiply(dmSunDetectionDataByAnsambleW))).Values
                    .Sum();
            DenseMatrix dmSunDetectionDataByAnsambleY = DenseMatrix.Create(dmSunDetectionDataByAnsamble.RowCount,
                dmSunDetectionDataByAnsamble.ColumnCount, (r, c) => (double)r);
            double sunCenterY =
                ((DenseMatrix)(dmSunDetectionDataByAnsambleY.PointwiseMultiply(dmSunDetectionDataByAnsambleW))).Values
                    .Sum();

            RoundData sunRD = new RoundData(sunCenterX, sunCenterY, sunRadius);


            #region Подкорректируем, если вылезли за пределы засветки

            DenseMatrix dmSourceDataInsideSunDisk = (DenseMatrix)dmGrIx.Clone();
            dmSourceDataInsideSunDisk.MapIndexedInplace((r, c, dVal) =>
            {
                PointD currPtd = new PointD(c, r);
                Vector2D currVecFromSunCenter = new Vector2D(sunRD.pointDCircleCenter(), currPtd);
                return (currVecFromSunCenter.VectorLength >= sunRD.DRadius) ? (0.0d) : (dVal);
            });
            DenseVector dvValuesInside = DataAnalysisStatic.DataVectorizedExcludingValues(dmSourceDataInsideSunDisk, 0.0d);
            if (Statistics.Percentile(dvValuesInside, 95) <= minSunburnGrIxValue)
            {
                // отфильтруем по значениям, получим центр масс и массу
                dmSourceDataInsideSunDisk.MapInplace(dVal => (dVal <= minSunburnGrIxValue) ? 0.0d : 1.0d);
                DenseMatrix dmXvalues = DenseMatrix.Create(dmGrIx.RowCount, dmGrIx.ColumnCount, 0.0d);
                DenseMatrix dmYvalues = (DenseMatrix)dmXvalues.Clone();
                dmGrIx.MapIndexedConvert<double>((r, c, dVal) => (double) c, dmXvalues, Zeros.Include);
                dmGrIx.MapIndexedConvert<double>((r, c, dVal) => (double) r, dmYvalues, Zeros.Include);
                DenseMatrix dmXvaluesFilteredForMassCenterSearching = (DenseMatrix)dmXvalues.PointwiseMultiply(dmSourceDataInsideSunDisk);
                DenseMatrix dmYvaluesFilteredForMassCenterSearching = (DenseMatrix)dmYvalues.PointwiseMultiply(dmSourceDataInsideSunDisk);

                double intNonZeroValues = dmSourceDataInsideSunDisk.Values.Sum();
                double massCenterX = dmXvaluesFilteredForMassCenterSearching.Values.Sum() / intNonZeroValues;
                double massCenterY = dmYvaluesFilteredForMassCenterSearching.Values.Sum() / intNonZeroValues;
                sunRD = new RoundData(massCenterX, massCenterY, Math.Sqrt(intNonZeroValues/Math.PI));
            }

            #endregion Подкорректируем, если вылезли за пределы засветки




            if (sunRD.RoundArea / imgP.imageRD.RoundArea < minSunAreaPart / 3.0d)
            {
                return RoundData.nullRoundData();
            }

            if (sunRD.RoundArea / imgP.imageRD.RoundArea > maxSunAreaPart)
            {
                sunRD.DRadius = Math.Sqrt((maxSunAreaPart * imgP.imageRD.RoundArea) / Math.PI);
            }

            if (verbosityLevel > 1)
            {
                Image<Bgr, byte> sunDetectionImage = new Image<Bgr, byte>(imgP.processingBitmap());
                Image<Bgr, byte> sunLocationImage = sunDetectionImage.CopyBlank();
                sunLocationImage.Draw(new CircleF(sunRD.pointfCircleCenter(), (float)sunRD.DRadius), new Bgr(255, 255, 255), 0);
                sunDetectionImage = sunDetectionImage.AddWeighted(sunLocationImage, 0.6d, 0.4d, 0.0d);
                sunDetectionImage.Save(currentDirectory + randomFileName + "_SunDetectionByAnsambleResult.jpg");
                sunDetectionImage.Dispose();
                sunLocationImage.Dispose();
            }

            return sunRD;
        }










        public RoundData DetectSunArced(ImageProcessing imgP, DenseMatrix dmGrIx, string currentDirectory = "", string randomFileName = "", double grixFilteringMinValue = 0.62d, double grixFilteringMaxValue = 0.64d)
        {
            verbosityLevel = Convert.ToInt32(defaultProperties["GrIxProcessingVerbosityLevel"]);
            //maskImage = imgP.significantMaskImageBinary;
            //Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(85.0d);
            Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(dSunDetectorArcedCropFactor * 100.0d);

            DenseMatrix dmSourceProcessingData = (DenseMatrix)dmGrIx.Clone();
            DenseMatrix dmProcessingData = (DenseMatrix)dmSourceProcessingData.Clone();
            DenseMatrix dmMaskCircled = ImageProcessing.DenseMatrixFromImage(maskImageCircled);

            dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMaskCircled);

            ArithmeticsOnImages aoi = new ArithmeticsOnImages();
            aoi.dmY = dmProcessingData;
            aoi.ExprString = "grad(Y)";
            aoi.RPNeval(true);
            DenseMatrix dmGradData = (DenseMatrix)aoi.dmRes.Clone();
            aoi.Dispose();

            dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMaskCircled);

            dmGradData = (DenseMatrix)dmGradData.PointwiseMultiply(dmMaskCircled);
            dmGradData.MapInplace(x => (double.IsNaN(x)) ? (0.0d) : (x));

            DenseMatrix dmFilteredData = (DenseMatrix)dmProcessingData.Clone();
            dmFilteredData.MapInplace(new Func<double, double>(d =>
            {
                if ((d >= grixFilteringMinValue) && (d <= grixFilteringMaxValue)) return d;
                return 0.0d;
            }));

            

            double curMedian = 0.0d;
            DescriptiveStatistics stats = DataAnalysisStatic.StatsOfDataExcludingValues(dmGradData, 0.0d, out curMedian);
            if (stats == null) return RoundData.nullRoundData();

            //double gradMean = stats.Mean;
            double gradMedian = curMedian;

            DenseMatrix dmGradCondition = (DenseMatrix)dmGradData.Map(dGradVal => (dGradVal > gradMedian) ? (0.0d) : (1.0d));

            dmFilteredData = (DenseMatrix)dmFilteredData.PointwiseMultiply(dmGradCondition);

            //dmFilteredData.MapIndexedInplace(new Func<int, int, double, double>((row, col, x) =>
            //{
            //    if (dmGradData[row, col] > gradMedian) return 0.0d;
            //    return x;
            //}));



            List<PointD> arcPointsList = new List<PointD>();
            foreach (Tuple<int, int, double> theElement in dmFilteredData.EnumerateIndexed())
            {
                int curRow = theElement.Item1;
                int curCol = theElement.Item2;
                double dVal = theElement.Item3;

                if (double.IsNaN(dVal)) continue;
                if (dVal == 0.0d) continue;
                arcPointsList.Add(new PointD((double)curCol, (double)curRow));
            }

            if (arcPointsList.Count < 5) return RoundData.nullRoundData();

            //Func<DenseVector, PointD, double> approxFuncDevSqred = new Func<DenseVector, PointD, double>(
            Func<DenseVector, PointD, double> approxFuncDevAbs = new Func<DenseVector, PointD, double>(
                (dvParameters, curPoint) =>
                {
                    double centerX = dvParameters[0];
                    double centerY = dvParameters[1];
                    double r = dvParameters[2];
                    double diffX = curPoint.X - centerX;
                    double diffY = curPoint.Y - centerY;
                    double diff = r - Math.Sqrt(diffX * diffX + diffY * diffY);
                    // return diff * diff;
                    return Math.Abs(diff);
                });

            //if (arcPointsList.Count < 5) return PointD.nullPointD();
            
            //if (arcPointsList.Count < 5) continue;


            #region start estimation of Sun disk position

            DenseMatrix dmXvalues = DenseMatrix.Create(dmSourceProcessingData.RowCount,
                dmSourceProcessingData.ColumnCount, 0.0d);
            DenseMatrix dmYvalues = (DenseMatrix)dmXvalues.Clone();
            dmSourceProcessingData.MapIndexedConvert<double>((r, c, dVal) => (double)c, dmXvalues, Zeros.Include);
            dmSourceProcessingData.MapIndexedConvert<double>((r, c, dVal) => (double)r, dmYvalues, Zeros.Include);

            DenseMatrix dmSunburnMassSenterSearching = (DenseMatrix)dmSourceProcessingData.Clone();
            dmSunburnMassSenterSearching.MapInplace(x => (x >= minSunburnGrIxValue) ? (1.0d) : (0.0d));

            DenseMatrix dmXvaluesFilteredForMassCenterSearching =
                (DenseMatrix) dmXvalues.PointwiseMultiply(dmSunburnMassSenterSearching);
            DenseMatrix dmYvaluesFilteredForMassCenterSearching =
                (DenseMatrix)dmYvalues.PointwiseMultiply(dmSunburnMassSenterSearching);

            double intNonZeroValues = dmSunburnMassSenterSearching.Values.Sum();
            double massCenterX = dmXvaluesFilteredForMassCenterSearching.Values.Sum()/intNonZeroValues;
            double massCenterY = dmYvaluesFilteredForMassCenterSearching.Values.Sum()/intNonZeroValues;

            #region // obsolete
            //foreach (Tuple<int, int, double> theElement in dmSunburnMassSenterSearching.EnumerateIndexed())
            //{
            //    if (theElement.Item3 < 1.0d) continue;
            //    massCenterX += (double)theElement.Item2;
            //    massCenterY += (double)theElement.Item1;
            //}
            //massCenterX = massCenterX / dmSunburnMassSenterSearching.Values.Sum();
            //massCenterY = massCenterY / dmSunburnMassSenterSearching.Values.Sum();
            #endregion // obsolete

            #endregion start estimation of Sun disk position


            #region 1st optimization pass

            double rInit = imgP.imageRD.DRadius / 2.0d;
            DenseVector dvInitialParametersValues =
                DenseVector.OfEnumerable(new double[] { massCenterX, massCenterY, rInit });

            double[] dvLowerBoundsConstraints = { 0.0d, 0.0d, 0.0d };
            double[] dvUpperBoundsConstraints = { maskImageCircled.Width, maskImageCircled.Height, maskImageCircled.Width / 2.0d };

            NonLinLeastSqProbWithBC<PointD> approximator1st = new NonLinLeastSqProbWithBC<PointD>();
            approximator1st.nParametersSpacePoint = dvInitialParametersValues.Copy();
            approximator1st.mFittingValuesVector = DenseVector.Create(arcPointsList.Count, 0.0d);
            approximator1st.mEventsSpaceVector = arcPointsList;
            approximator1st.fittingFunction = (iEnumVec, ptD) => approxFuncDevAbs(DenseVector.OfEnumerable(iEnumVec), ptD);
            approximator1st.lowerBoundConstraints = dvLowerBoundsConstraints;
            approximator1st.upperBoundConstraints = dvUpperBoundsConstraints;

            DenseVector dvApproximatedParameters = DenseVector.OfEnumerable(approximator1st.SolveOptimizationProblem());
            //// replaced with MKL wrapped optimizer

            #endregion 1st optimization pass


            #region 2nd optimization pass

            // replaced with MKL wrapped optimizer
            DenseVector dvDeviationsAbs = DenseVector.Create(arcPointsList.Count,
                i => approxFuncDevAbs(dvApproximatedParameters, arcPointsList[i]));
            // DescriptiveStatistics stats1 = new DescriptiveStatistics(dvDeviationsAbs.Values);
            //double meanDevAbsed = stats1.Mean;
            double medianDevAbsed = Statistics.Median(dvDeviationsAbs.Values);

            List<PointD> arcPointsListFiltered1st =
                arcPointsList.FindAll(
                    new Predicate<PointD>(x => approxFuncDevAbs(dvApproximatedParameters, x) <= medianDevAbsed));

            NonLinLeastSqProbWithBC<PointD> approximator2nd = new NonLinLeastSqProbWithBC<PointD>();
            approximator2nd.nParametersSpacePoint = dvApproximatedParameters.Copy();
            approximator2nd.mFittingValuesVector = DenseVector.Create(arcPointsListFiltered1st.Count, 0.0d);
            approximator2nd.mEventsSpaceVector = arcPointsListFiltered1st;
            approximator2nd.fittingFunction = (iEnumVec, ptD) => approxFuncDevAbs(DenseVector.OfEnumerable(iEnumVec), ptD);
            approximator2nd.lowerBoundConstraints = dvLowerBoundsConstraints;
            approximator2nd.upperBoundConstraints = dvUpperBoundsConstraints;

            DenseVector dvApproximatedParameters2nd = DenseVector.OfEnumerable(approximator2nd.SolveOptimizationProblem());

            #endregion 2nd optimization pass

            RoundData circle2ndPass = new RoundData(dvApproximatedParameters2nd[0], dvApproximatedParameters2nd[1],
                dvApproximatedParameters2nd[2]);
            // replaced with MKL wrapped optimizer





            PointD ptdZeroAnglePoint1stPass = PointD.Sum(arcPointsListFiltered1st) / (double)arcPointsListFiltered1st.Count;
            Vector2D vct2dZeroAngleDirection_1stPass = new Vector2D(circle2ndPass.pointDCircleCenter(), ptdZeroAnglePoint1stPass);
            vct2dZeroAngleDirection_1stPass = vct2dZeroAngleDirection_1stPass* (1.0d/vct2dZeroAngleDirection_1stPass.VectorLength);
            double zeroAngleValue = 0.0d;
            //List<PointPolar> arcPointsListFilteredPolar = DataAnalysis.ListDecartToPolar(arcPointsListFiltered1st,
            //    circle2ndPass.pointDCircleCenter(), ptdZeroAnglePoint1stPass, out zeroAngleValue);

            #region Фильтр точек по значениям Cos

            List<double> arcPointsListFiltered1st_CosValues = arcPointsListFiltered1st.ConvertAll(ptd =>
            {
                Vector2D currPtdVec2d = new Vector2D(circle2ndPass.pointDCircleCenter(), ptd);
                return (currPtdVec2d.VectorLength == 0.0d)
                    ? 0.0d
                    : (currPtdVec2d*vct2dZeroAngleDirection_1stPass)/currPtdVec2d.VectorLength;
            });

            double cosValuesMedian = Statistics.Median(arcPointsListFiltered1st_CosValues);
            double cosValuesStd = arcPointsListFiltered1st_CosValues.StandardDeviation();
            
            // возьмем значения cos меньше медианы
            // по ним отфильтруем точки

            List<int> indexesFilteredByCosValues =
                arcPointsListFiltered1st_CosValues.Select(
                    (cosVal, idx) => new Tuple<int, double>(idx, cosVal))
                    .Where(tpl => (tpl.Item2 <= cosValuesMedian + cosValuesStd))
                    .Select(tpl => tpl.Item1).ToList();

            List<PointD> arcPointsListFiltered2nd =
                arcPointsListFiltered1st.Where((ptd, idx) => indexesFilteredByCosValues.Contains(idx)).ToList();

            // найдем новое значение центра масс дуги
            PointD ptdZeroAnglePoint2ndPass = PointD.Sum(arcPointsListFiltered2nd) / (double)arcPointsListFiltered2nd.Count;

            #endregion Фильтр точек по значениям Cos

            Vector2D vctZeroAngleOfTheArc_2ndPass = new Vector2D(circle2ndPass.pointDCircleCenter(), ptdZeroAnglePoint2ndPass);
            vctZeroAngleOfTheArc_2ndPass = vctZeroAngleOfTheArc_2ndPass*(1.0d/vctZeroAngleOfTheArc_2ndPass.VectorLength);

            PointD meanArcPoint_2ndPass =
                (circle2ndPass.pointDCircleCenter() + (circle2ndPass.DRadius*vctZeroAngleOfTheArc_2ndPass)).ToPointD();
            PointD meanArcPoint2_2ndPass =
                (circle2ndPass.pointDCircleCenter() + (-circle2ndPass.DRadius*vctZeroAngleOfTheArc_2ndPass)).ToPointD();

            PointD meanArcPoint_1stPass =
                (circle2ndPass.pointDCircleCenter() + (circle2ndPass.DRadius*vct2dZeroAngleDirection_1stPass)).ToPointD();

            #region // replaced - see above

            //DenseVector dvAnglesData = DenseVector.Create(arcPointsListFilteredPolar.Count,
            //    i => arcPointsListFilteredPolar[i].Phi);
            //DescriptiveStatistics anglesStats = new DescriptiveStatistics(dvAnglesData);



            //DenseVector dvAnglesDataFiltered = DataAnalysis.DataVectorizedWithCondition(dvAnglesData,
            //    d => (Math.Abs(d - anglesStats.Mean) <= anglesStats.StandardDeviation * 2.0d));
            //double minAngleValue = dvAnglesDataFiltered.Values.Min();
            //double maxAngleValue = dvAnglesDataFiltered.Values.Max();
            //double meanAngle = (minAngleValue + maxAngleValue) / 2.0d;

            //PointD meanArcPoint = new PointD();
            //meanArcPoint.X = circle2ndPass.DRadius * Math.Cos(meanAngle + zeroAngleValue) + circle2ndPass.DCenterX;
            //meanArcPoint.Y = -circle2ndPass.DRadius * Math.Sin(meanAngle + zeroAngleValue) + circle2ndPass.DCenterY;

            //PointD meanArcPoint2 = new PointD();
            //meanArcPoint2.X = circle2ndPass.DRadius * Math.Cos(meanAngle + zeroAngleValue + Math.PI) +
            //                  circle2ndPass.DCenterX;
            //meanArcPoint2.Y = -circle2ndPass.DRadius * Math.Sin(meanAngle + zeroAngleValue + Math.PI) +
            //                  circle2ndPass.DCenterY;



            //PointD zeroPointOnCircle =
            //    new PointD(circle2ndPass.DRadius * Math.Cos(zeroAngleValue) + circle2ndPass.DCenterX,
            //        -circle2ndPass.DRadius * Math.Sin(zeroAngleValue) + circle2ndPass.DCenterY);

            #endregion // replaced - see above


            #region отображение результата исследований изображения

            Image<Bgr, byte> imgPoints = LocalProcessingImage.CopyBlank(); // new Image<Bgr, byte>(LocalProcessingBitmap).CopyBlank();
            imgPoints.Draw(new CircleF(circle2ndPass.pointfCircleCenter(), (float)circle2ndPass.DRadius),
                new Bgr(Color.White), 2);
            foreach (PointD pointFiltered in arcPointsListFiltered1st) imgPoints.Draw(new CircleF(pointFiltered.PointF(), 2.0f), new Bgr(Color.Yellow), 1);
            imgPoints.Draw(new LineSegment2DF(meanArcPoint_2ndPass.PointF(), meanArcPoint2_2ndPass.PointF()), new Bgr(Color.Magenta),
                2);
            imgPoints.Draw(new LineSegment2DF(meanArcPoint_1stPass.PointF(), circle2ndPass.pointfCircleCenter()),
                new Bgr(Color.White), 2);
            //imgPoints.Draw(new CircleF(tmpZeroAnglePoint.PointF(), 2.0f), new Bgr(Color.White), 2);
            Image<Bgr, byte> sourceDataImage = LocalProcessingImage.Copy(); // new Image<Bgr, byte>(LocalProcessingBitmap);
            sourceDataImage = sourceDataImage.AddWeighted(imgPoints, 0.5d, 0.5d, 0.0d);
            tmpSunDetectionimage = sourceDataImage.Copy();

            if (verbosityLevel > 1)
                sourceDataImage.Save(currentDirectory + randomFileName + "arcedSunDetection_" +
                                     grixFilteringMinValue.ToString().Replace(",", "") + ".jpg");
            //if (!isCalculatingUsingBgWorker) ServiceTools.ShowPicture(sourceDataImage, "minFV = " + grixFilteringMinValue + "; maxFV = " + grixFilteringMaxValue);
            //if (verbosityLevel > 1) tmpSunDetectionimage.Save(currentDirectory + randomFileName + "-sun-detection-visualization.jpg");

            #endregion отображение результата исследований изображения


            #region filter points along the arc bisectrix obtained above

            DenseMatrix dmSourceProcessingDataFilteredAlongBisectrix = (DenseMatrix)dmSourceProcessingData.Clone();

            double dHalfWidthY = imgP.imageRD.DRadius / 70.0d;
            double dHalfWidthYsqr = dHalfWidthY* dHalfWidthY;

            DenseMatrix dmFilterByDistanceToBisectrix = (DenseMatrix)dmSourceProcessingData.MapIndexed((r, c, dVal) =>
            {
                PointD currPtd = new PointD(c, r);
                Vector2D currVec = new Vector2D(circle2ndPass.pointDCircleCenter(), currPtd);
                double distSqr = currVec.VectorLength*currVec.VectorLength -
                                 (currVec*vctZeroAngleOfTheArc_2ndPass)*(currVec*vctZeroAngleOfTheArc_2ndPass);
                return ((distSqr <= dHalfWidthYsqr) ? (1.0d):(0.0d));
            });

            dmSourceProcessingDataFilteredAlongBisectrix =
                (DenseMatrix)
                    dmSourceProcessingDataFilteredAlongBisectrix.PointwiseMultiply(dmFilterByDistanceToBisectrix);

            #endregion filter points along the arc bisectrix obtained above



            #region // replaced by faster filtering - see above

            //DenseMatrix dmProcessingDataXrotated = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
            //    (r, c) => (double)c);
            //dmProcessingDataXrotated = (DenseMatrix)dmProcessingDataXrotated.PointwiseMultiply(dmMaskCircled);
            //DenseMatrix dmProcessingDataXsrc = (DenseMatrix)dmProcessingDataXrotated.Clone();
            //DenseMatrix dmProcessingDataYrotated = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
            //    (r, c) => (double)r);
            //dmProcessingDataYrotated = (DenseMatrix)dmProcessingDataYrotated.PointwiseMultiply(dmMaskCircled);
            //DenseMatrix dmProcessingDataYsrc = (DenseMatrix)dmProcessingDataYrotated.Clone();
            //PointD rotationCenterPtD = new PointD(meanArcPoint_2ndPass);
            //double rotationAngle = DataAnalysis.PtdDecartToPolar(circle2ndPass.pointDCircleCenter(), rotationCenterPtD,
            //    0.0d).Phi;
            //dmProcessingDataXrotated.MapIndexedInplace((row, col, xVal) =>
            //{
            //    double yVal = dmProcessingDataYsrc[row, col];
            //    PointD curPointD = new PointD(xVal, yVal);
            //    PointPolar ptPolarRotated = DataAnalysis.PtdDecartToPolar(curPointD, rotationCenterPtD, rotationAngle);
            //    PointD retPtD = DataAnalysis.PtdPolarToCartesian(ptPolarRotated, new PointD(0.0d, 0.0d), 0.0d);
            //    return retPtD.X;
            //});


            //dmProcessingDataYrotated.MapIndexedInplace((row, col, yVal) =>
            //{
            //    double xVal = dmProcessingDataXsrc[row, col];
            //    PointD curPointD = new PointD(xVal, yVal);
            //    PointPolar ptPolarRotated = DataAnalysis.PtdDecartToPolar(curPointD, rotationCenterPtD, rotationAngle);
            //    PointD retPtD = DataAnalysis.PtdPolarToCartesian(ptPolarRotated, new PointD(0.0d, 0.0d), 0.0d);
            //    return retPtD.Y;
            //});



            //double minRotatedXdouble = dmProcessingDataXrotated.Values.Min();
            //int minRotatedX = Convert.ToInt32((Math.Round(minRotatedXdouble, 0) == minRotatedXdouble) ? (minRotatedXdouble) : (Math.Round(minRotatedXdouble, MidpointRounding.ToEven)));
            //minRotatedX = (minRotatedX > minRotatedXdouble) ? (minRotatedX - 1) : (minRotatedX);
            //double maxRotatedXdouble = dmProcessingDataXrotated.Values.Max();
            //int maxRotatedX = Convert.ToInt32((Math.Round(maxRotatedXdouble, 0) == maxRotatedXdouble) ? (maxRotatedXdouble) : (Math.Round(maxRotatedXdouble, MidpointRounding.ToEven)));
            //maxRotatedX = (maxRotatedX < maxRotatedXdouble) ? (maxRotatedX + 1) : (maxRotatedX);

            //DenseMatrix dmSourceProcessingDataFilteredAlongBisectrix = (DenseMatrix)dmSourceProcessingData.Clone();

            //double dHalfWidthY = imgP.imageRD.DRadius / 70.0d;
            //dmSourceProcessingDataFilteredAlongBisectrix.MapIndexedInplace((row, col, dVal) =>
            //{
            //    //if ((dmProcessingDataYrotated[row, col] <= -10.0d) ||
            //    //        (dmProcessingDataYrotated[row, col] >= 10.0d)) return 0.0d;
            //    if ((dmProcessingDataYrotated[row, col] <= -dHalfWidthY) ||
            //            (dmProcessingDataYrotated[row, col] >= dHalfWidthY)) return 0.0d;
            //    return dVal;
            //});
            //dmSourceProcessingDataFilteredAlongBisectrix =
            //    (DenseMatrix)dmSourceProcessingDataFilteredAlongBisectrix.PointwiseMultiply(dmMaskCircled);

            #endregion replaced by faster filtering - see above

            DenseMatrix dmSunburnDataAlongBisectrix = (DenseMatrix)dmSourceProcessingDataFilteredAlongBisectrix.Clone();
            //dmSunburnDataAlongBisectrix.MapInplace(d => (d == 1.0d) ? (d) : (0.0d));
            dmSunburnDataAlongBisectrix.MapInplace(d => (d >= minSunburnGrIxValue) ? (d) : (0.0d));

            DenseMatrix dmProcessingDataX = (DenseMatrix)dmXvalues.PointwiseMultiply(dmSunburnDataAlongBisectrix);
            DenseMatrix dmProcessingDataY = (DenseMatrix)dmYvalues.PointwiseMultiply(dmSunburnDataAlongBisectrix);

            if (dmSunburnDataAlongBisectrix.Values.Sum() == 0.0d) return RoundData.nullRoundData();
            if (dmProcessingDataX.Values.Sum() == 0.0d) return RoundData.nullRoundData();

            double testSunCenterX = dmProcessingDataX.Values.Sum() / dmSunburnDataAlongBisectrix.Values.Sum();
            double testSunCenterY = dmProcessingDataY.Values.Sum() / dmSunburnDataAlongBisectrix.Values.Sum();
            PointD testSunCenter = new PointD(testSunCenterX, testSunCenterY);
            //double testSunRadius = (dmSunburnDataAlongBisectrix.Values.Sum() / 20.0d) / 2.0d;
            double testSunRadius = (dmSunburnDataAlongBisectrix.Values.Sum() / (2.0d * dHalfWidthY)) / 2.0d;

            RoundData rd = new RoundData(testSunCenterX, testSunCenterY, testSunRadius);
            
            ServiceTools.FlushMemory();

            return rd;
        }










        #region //public RoundData detectSun(DenseMatrix dmGrIxData, Image<Gray, Byte> maskImageCutMargins, Image<Gray, Byte> maskImageFull, ImageProcessing imgP, string currentDirectory = "", string randomFileName = "")
        //public RoundData detectSun(DenseMatrix dmGrIxData, Image<Gray, Byte> maskImageCutMargins, Image<Gray, Byte> maskImageFull, ImageProcessing imgP, string currentDirectory = "", string randomFileName = "")
        //{
        //    if (!isCalculatingUsingBgWorker)
        //        theLogWindow = ServiceTools.LogAText(theLogWindow,
        //            "Начинаю определение местонахождения солнца по форме поля засветки");


        //    RoundData retRoundData = new RoundData();

        //    double pixelMaxDistanceToDtermineAverageGrad = 5.0d;

        //    Image<Gray, Byte> maskImage100 = maskImageFull.Copy();
        //    Image<Gray, Byte> maskImage = maskImageCutMargins.Copy();
        //    double overallMaskArea = (double)maskImage.CountNonzero()[0];

        //    DenseMatrix dmProcessingData = (DenseMatrix)dmGrIxData.Clone();

        //    if (verbosityLevel > 1)
        //        ServiceTools.RepresentDataFromDenseMatrix(dmProcessingData, "original GrIx data", false, false, 0.0d, 1.0d, !isCalculatingUsingBgWorker);
        //    DenseMatrix dmMask100 = ImageProcessing.DenseMatrixFromImage(maskImage100);
        //    DenseMatrix dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);

        //    DenseMatrix dmImageMarginsMask = DenseMatrix.Create(dmMask100.RowCount, dmMask100.ColumnCount, (r, c) =>
        //    {
        //        if ((r < 8) || (c < 8) || (dmMask100.RowCount - r < 8) || (dmMask100.ColumnCount - c < 8)) return 0.0d;
        //        else return 1.0d;
        //    });

        //    dmMask100 = (DenseMatrix)dmMask100.PointwiseMultiply(dmImageMarginsMask);
        //    dmMask = (DenseMatrix)dmMask.PointwiseMultiply(dmImageMarginsMask);


        //    dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMask100);


        //    ServiceTools.FlushMemory();



        //    #region посчитаем для объектов засветки разные солнечные метрики
        //    // lets play with grad field near the sunburn edges

        //    ArithmeticsOnImages aoi = new ArithmeticsOnImages();
        //    aoi.dmY = dmProcessingData;
        //    aoi.ExprString = "grad(Y)";
        //    aoi.RPNeval(true);
        //    DenseMatrix dmGradField = (DenseMatrix)aoi.dmRes.Clone();
        //    dmGradField = (DenseMatrix)dmGradField.PointwiseMultiply(dmMask);


        //    if (verbosityLevel > 1)
        //        ServiceTools.RepresentDataFromDenseMatrix(dmGradField, "Grad field", false, false, 0.0d, 1.0d, !isCalculatingUsingBgWorker);

        //    // посмотрим, что с градиентами на границах разных отдельных объектов

        //    Image<Gray, Byte> imgSunMask = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(
        //        dmProcessingData, 0.0d, 1.0d, false);
        //    imgSunMask = imgSunMask.ThresholdBinary(new Gray(254), new Gray(255));
        //    if (imgSunMask.CountNonzero()[0] == 0)
        //    {
        //        return null;
        //    }

        //    if (!isCalculatingUsingBgWorker)
        //        ServiceTools.ShowPicture(imgSunMask, "");

        //    Contour<Point> contoursDetected = imgSunMask.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
        //    List<Contour<Point>> contoursList = new List<Contour<Point>>();
        //    List<Contour<Point>> overallContoursList = new List<Contour<Point>>();

        //    while (true)
        //    {
        //        Contour<Point> currContour = contoursDetected;
        //        double currcontourArePart = currContour.Area / overallMaskArea;
        //        //if ((currcontourArePart <= maxSunAreaPart) && (currcontourArePart >= minSunAreaPart))
        //        overallContoursList.Add(currContour);
        //        if (currcontourArePart >= minSunAreaPart)
        //        {
        //            contoursList.Add(currContour);
        //        }

        //        contoursDetected = contoursDetected.HNext;
        //        if (contoursDetected == null)
        //            break;
        //    }

        //    if (contoursList.Count == 0) return null;




        //    DenseMatrix dmBinaryProcessingData = (DenseMatrix)dmProcessingData.Clone();
        //    dmBinaryProcessingData = (DenseMatrix)dmBinaryProcessingData.PointwiseMultiply(dmMask100);
        //    dmBinaryProcessingData.MapInplace(x => (x == 1.0d) ? (1.0d) : (0.0d));
        //    DenseMatrix dmBinaryProcessingDataInversed = (DenseMatrix)dmBinaryProcessingData.Clone();
        //    dmBinaryProcessingDataInversed.MapInplace(x => (x > 0.0d) ? (0.0d) : (1.0d));
        //    DenseMatrix dmpoints5PxToCountours = DenseMatrix.Create(dmProcessingData.RowCount,
        //        dmProcessingData.ColumnCount,
        //        (row, col) =>
        //        {
        //            int startRow = row - 5;
        //            startRow = (startRow < 0) ? (0) : (startRow);
        //            int rowCnt = 11;
        //            int lastRow = startRow + rowCnt;
        //            if (lastRow >= dmProcessingData.RowCount)
        //            {
        //                lastRow = dmProcessingData.RowCount - 1;
        //                rowCnt = lastRow - startRow;
        //            }

        //            int startCol = col - 5;
        //            startCol = (startCol < 0) ? (0) : (startCol);
        //            int colCnt = 11;
        //            int lastCol = startCol + colCnt;
        //            if (lastCol >= dmProcessingData.ColumnCount)
        //            {
        //                lastCol = dmProcessingData.ColumnCount - 1;
        //                colCnt = lastCol - startCol;
        //            }

        //            DenseMatrix tmpmatrix =
        //                (DenseMatrix)dmBinaryProcessingData.SubMatrix(startRow, rowCnt, startCol, colCnt);
        //            if (tmpmatrix.Values.Sum() > 0.0d)
        //            {
        //                //в этой области есть хотя бы одна точка контура
        //                return 1.0d;
        //            }
        //            return 0.0d;
        //        });
        //    dmpoints5PxToCountours = (DenseMatrix)dmpoints5PxToCountours.PointwiseMultiply(dmBinaryProcessingDataInversed);
        //    // получили границы всех областей засветки. проанализируем данные по градиенту на этих границах
        //    DenseMatrix dmGradFieldOverMargins = (DenseMatrix)dmGradField.PointwiseMultiply(dmpoints5PxToCountours);
        //    dmGradFieldOverMargins = (DenseMatrix)dmGradFieldOverMargins.PointwiseMultiply(dmMask100);
        //    //ServiceTools.RepresentDataFromDenseMatrix(dmGradFieldOverMargins, "grad data along all contours");
        //    DescriptiveStatistics statsGradFieldOverMargins =
        //        DataAnalysis.StatsOfDataExcludingValues(dmGradFieldOverMargins, 0.0d);
        //    theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        Environment.NewLine +
        //        "over all contours margins:" + Environment.NewLine + "grad MEAN = " +
        //        statsGradFieldOverMargins.Mean.ToString("e") + Environment.NewLine + "grad StdDev = " +
        //        statsGradFieldOverMargins.StandardDeviation.ToString("e") + Environment.NewLine);



        //    //string baseDir = "G:\\_gulevlab\\SkyIndexAnalyzerSolo_appData\\_dataDirectory\\";
        //    //ServiceTools.logToTextFile(currentDirectory + randomFileName + "_ShowHistScript.m", "clear; " + Environment.NewLine, true);



        //    Contour<Point> foundSunContour = null;

        //    theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "STARTING sun locating with isolines arcs");
        //    RoundData refSunDisk = DetectSunWithSerieOfArcs(imgP, dmGrIxData, currentDirectory, randomFileName);
        //    theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "FINISHED sun locating with isolines arcs");

        //    if (refSunDisk.IsNull)
        //    {
        //        return null;
        //    }
        //    foreach (Contour<Point> currContour in contoursList)
        //    {
        //        if (currContour.InContour(refSunDisk.pointDCircleCenter().PointF()) >= 0.0d)
        //        {
        //            foundSunContour = currContour;
        //        }
        //    }

        //    #endregion посчитаем для объектов засветки разные солнечные метрики




        //    #region оценка применимости алгоритма подавления солнечной засветки
        //    // оценить общую площадь засветки 255 - должно быть прилично ---СКОЛЬКО?
        //    // найдем все куски площадью больше - ЧЕГО?
        //    // оценить разброс кластеров засветки - должно быть единое круглое солнце, возможно, с лучами
        //    // если засветки слишком мало - неприменимо, использовать без подавления
        //    // если засветки слишком много - неприменимо, использовать без подавления

        //    bool theSunSuppressionSchemeApplicable = true;





        //    if (foundSunContour != null)
        //    {
        //        #region поиск солнца по найденному контуру
        //        #region обработаем найденный контур еще раз

        //        Image<Gray, byte> imgCurrentContour = imgSunMask.CopyBlank();
        //        imgCurrentContour.Draw(foundSunContour, new Gray(255), -1);

        //        tmpSunDetectionimage = tmpSunDetectionimage.AddWeighted(imgCurrentContour.Convert<Bgr, byte>(), 1.0d, 0.4d, 0.0d);


        //        DenseMatrix dmBinaryProcessingDataCurrContour = ImageProcessing.DenseMatrixFromImage(imgCurrentContour);
        //        dmBinaryProcessingDataCurrContour.MapInplace(x => (x > 0.0d) ? (1.0d) : (0.0d));
        //        DenseMatrix dmBinaryProcessingDataCurrContourInversed =
        //            (DenseMatrix)dmBinaryProcessingDataCurrContour.Clone();
        //        dmBinaryProcessingDataCurrContourInversed.MapInplace(x => (x > 0.0d) ? (0.0d) : (1.0d));

        //        DenseMatrix dmpoints5PxToCurrentCountour = DenseMatrix.Create(dmProcessingData.RowCount,
        //        dmProcessingData.ColumnCount,
        //        (row, col) =>
        //        {
        //            int startRow = row - 5;
        //            startRow = (startRow < 0) ? (0) : (startRow);
        //            int rowCnt = 11;
        //            int lastRow = startRow + rowCnt;
        //            if (lastRow >= dmProcessingData.RowCount)
        //            {
        //                lastRow = dmProcessingData.RowCount - 1;
        //                rowCnt = lastRow - startRow;
        //            }

        //            int startCol = col - 5;
        //            startCol = (startCol < 0) ? (0) : (startCol);
        //            int colCnt = 11;
        //            int lastCol = startCol + colCnt;
        //            if (lastCol >= dmProcessingData.ColumnCount)
        //            {
        //                lastCol = dmProcessingData.ColumnCount - 1;
        //                colCnt = lastCol - startCol;
        //            }

        //            DenseMatrix tmpmatrix =
        //                (DenseMatrix)dmBinaryProcessingDataCurrContour.SubMatrix(startRow, rowCnt, startCol, colCnt);
        //            if (tmpmatrix.Values.Sum() > 0.0d)
        //            {
        //                //в этой области есть хотя бы одна точка контура
        //                return 1.0d;
        //            }
        //            return 0.0d;
        //        });

        //        dmpoints5PxToCurrentCountour = (DenseMatrix)dmpoints5PxToCurrentCountour.PointwiseMultiply(dmBinaryProcessingDataCurrContourInversed);
        //        //ServiceTools.RepresentDataFromDenseMatrix(dmpoints5PxToCurrentCountour, "points along contours", true, false, 0.0d, 2.0d, true);






        //        DenseMatrix dmCurrContourGradData = (DenseMatrix)dmGradField.Clone();
        //        dmCurrContourGradData = (DenseMatrix)dmCurrContourGradData.PointwiseMultiply(dmpoints5PxToCurrentCountour);
        //        //ServiceTools.RepresentDataFromDenseMatrix(dmCurrContourGradData, "grad data aling found contour");

        //        DescriptiveStatistics currContourGradDataStats =
        //            DataAnalysis.StatsOfDataExcludingValues(dmCurrContourGradData, 0.0d);


        //        List<PointD> acceptablePointsList = new List<PointD>();
        //        List<double> lPointsWeights = new List<double>();


        //        foreach (Point foundContourPoint in foundSunContour)
        //        {
        //            // найдем данные по этой точке


        //            int startRow = foundContourPoint.Y - 5;
        //            startRow = (startRow < 0) ? (0) : (startRow);
        //            int rowCnt = 11;
        //            int lastRow = startRow + rowCnt;
        //            if (lastRow >= dmProcessingData.RowCount)
        //            {
        //                lastRow = dmProcessingData.RowCount - 1;
        //                rowCnt = lastRow - startRow;
        //            }

        //            int startCol = foundContourPoint.X - 5;
        //            startCol = (startCol < 0) ? (0) : (startCol);
        //            int colCnt = 11;
        //            int lastCol = startCol + colCnt;
        //            if (lastCol >= dmProcessingData.ColumnCount)
        //            {
        //                lastCol = dmProcessingData.ColumnCount - 1;
        //                colCnt = lastCol - startCol;
        //            }

        //            DenseMatrix currPointLocalSubmatrix =
        //                (DenseMatrix)dmCurrContourGradData.SubMatrix(startRow, rowCnt, startCol, colCnt);
        //            DescriptiveStatistics currPointLocalStats =
        //                DataAnalysis.StatsOfDataExcludingValues(currPointLocalSubmatrix, 0.0d);
        //            if (currPointLocalStats == null) continue;
        //            if (currPointLocalStats.Mean < statsGradFieldOverMargins.Mean)
        //            {
        //                lPointsWeights.Add(currPointLocalStats.Mean);
        //                acceptablePointsList.Add(new PointD(foundContourPoint));
        //            }
        //        }


        //        #region Отображение результата фильтрации
        //        DenseMatrix dmAcceptablePoints5pxMask = DenseMatrix.Create(dmProcessingData.RowCount,
        //            dmProcessingData.ColumnCount, (r, c) => 0.0d);
        //        foreach (PointD pt in acceptablePointsList)
        //        {
        //            Point ptInt = new Point(Convert.ToInt32(pt.X), Convert.ToInt32(pt.Y));

        //            int startRow = ptInt.Y - 5;
        //            startRow = (startRow < 0) ? (0) : (startRow);
        //            int rowCnt = 11;
        //            int lastRow = startRow + rowCnt;
        //            if (lastRow >= dmProcessingData.RowCount)
        //            {
        //                lastRow = dmProcessingData.RowCount - 1;
        //                rowCnt = lastRow - startRow;
        //            }

        //            int startCol = ptInt.X - 5;
        //            startCol = (startCol < 0) ? (0) : (startCol);
        //            int colCnt = 11;
        //            int lastCol = startCol + colCnt;
        //            if (lastCol >= dmProcessingData.ColumnCount)
        //            {
        //                lastCol = dmProcessingData.ColumnCount - 1;
        //                colCnt = lastCol - startCol;
        //            }

        //            for (int row = startRow; row < lastRow; row++)
        //                for (int col = startCol; col < lastCol; col++)
        //                {
        //                    dmAcceptablePoints5pxMask[row, col] = 1.0d;
        //                }
        //        }
        //        dmAcceptablePoints5pxMask = (DenseMatrix)dmAcceptablePoints5pxMask.PointwiseMultiply(dmBinaryProcessingDataCurrContourInversed);
        //        DenseMatrix dmAcceptablePoints5pxGradData = (DenseMatrix)dmGradField.Clone();
        //        dmAcceptablePoints5pxGradData = (DenseMatrix)dmAcceptablePoints5pxGradData.PointwiseMultiply(dmAcceptablePoints5pxMask);
        //        //ServiceTools.RepresentDataFromDenseMatrix(dmAcceptablePoints5pxGradData, "filtered grad data");
        //        #endregion Отображение результата фильтрации



        //        DenseVector dvPointsWeight = DenseVector.OfEnumerable(lPointsWeights);
        //        double gradMaxVal = dvPointsWeight.Max();
        //        double gradMinVal = dvPointsWeight.Min();
        //        dvPointsWeight.MapInplace(x =>
        //        {
        //            return (1.0d - (x - gradMinVal) / (gradMaxVal - gradMinVal));
        //        }, true);

        //        Func<DenseVector, PointD, double> minimizingFunc = (dvParameters, pt) =>
        //        {
        //            double sunCenterX = dvParameters[0];
        //            double sunCenterY = dvParameters[1];
        //            double r = dvParameters[2];
        //            double diffX = pt.X - sunCenterX;
        //            double diffY = pt.Y - sunCenterY;
        //            double diff = r - Math.Sqrt(diffX * diffX + diffY * diffY);
        //            //diff = Math.Abs(diff);
        //            return diff;
        //        };

        //        double initSunCenterX = acceptablePointsList.Sum((pt => pt.X)) / (double)acceptablePointsList.Count;
        //        double initSunCenterY = acceptablePointsList.Sum((pt => pt.Y)) / (double)acceptablePointsList.Count;
        //        double initSunRadius = Math.Sqrt(foundSunContour.Area / Math.PI);
        //        List<double> parametersList = new List<double>();
        //        parametersList.Add(initSunCenterX);
        //        parametersList.Add(initSunCenterY);
        //        parametersList.Add(initSunRadius);
        //        DenseVector dvInitialParameters = DenseVector.OfEnumerable(parametersList);
        //        DenseVector initialParametersIncremnt = DenseVector.Create(dvInitialParameters.Count, (i => 1.0d));

        //        GradientDescentApproximator approximator = new GradientDescentApproximator(acceptablePointsList, minimizingFunc);
        //        approximator.DvWeights = dvPointsWeight;
        //        //approximator.DvWeights = "";
        //        DenseVector approximatedParameters = approximator.ApproximationGradientDescent2DPt(dvInitialParameters, initialParametersIncremnt, 0.000001d);

        //        #endregion обработаем найденный контур еще раз



        //        Image<Gray, Byte> imgSunMaskFiltered = imgSunMask.CopyBlank();
        //        imgSunMaskFiltered.Draw(foundSunContour, new Gray(255), -1);
        //        DenseMatrix dmSunMask = ImageProcessing.DenseMatrixFromImage(imgSunMaskFiltered);
        //        dmSunMask.MapInplace(val => val / 255.0d);

        //        PointD sunCenterPoint = new PointD(approximatedParameters[0], approximatedParameters[1]);
        //        double sunRadius = approximatedParameters[2];

        //        retRoundData.DCenterX = sunCenterPoint.X;
        //        retRoundData.DCenterY = sunCenterPoint.Y;
        //        retRoundData.DRadius = sunRadius;


        //        #region сделаем вывод о состоянии диска солнца по его площади

        //        double foundSunDiskArea = retRoundData.RoundArea;
        //        double sunburnContourArea = foundSunContour.Area;
        //        double overallSunburnArea = imgSunMask.CountNonzero()[0];
        //        double sunDiskToItsSunburn_ratio = foundSunDiskArea / sunburnContourArea;
        //        double sunDiskToOverallSunburn_ratio = foundSunDiskArea / overallSunburnArea;
        //        if ((sunDiskToItsSunburn_ratio > 0.9d) && (sunDiskToItsSunburn_ratio < 1.1d))
        //        {
        //            //солнце составляет почти всю площадь своей засветки - отдельно стоящее солнце
        //            if (sunDiskToOverallSunburn_ratio < 0.4d)
        //            {
        //                //очень много других объектов засветки - солнце в первой
        //                currentSunDiskCondition = SunDiskConditions.first;
        //            }
        //            else
        //            {
        //                currentSunDiskCondition = SunDiskConditions.second;
        //            }
        //        }
        //        else if (sunDiskToItsSunburn_ratio <= 0.9d)
        //        {
        //            // солце выглядывает из-за курска облака. Но все еще определяется
        //            currentSunDiskCondition = SunDiskConditions.first;
        //        }
        //        else if (sunDiskToItsSunburn_ratio >= 1.1d)
        //        {
        //            currentSunDiskCondition = SunDiskConditions.second;
        //        }

        //        #endregion сделаем вывод о состоянии диска солнца по его площади






        //        double sunAreaPartial = (double)retRoundData.RoundArea / (double)maskImageFull.CountNonzero()[0];
        //        if ((sunAreaPartial > maxSunAreaPart) || (sunAreaPartial < minSunAreaPart))
        //        {
        //            //theSunSuppressionSchemeApplicable = false;
        //            //retRoundData = null;
        //            //return retRoundData;

        //            double sunAreaAbs = (double)maskImageFull.CountNonzero()[0] * aveSunAreaPart;
        //            retRoundData.DRadius = Math.Sqrt(sunAreaAbs / Math.PI);
        //        }


        //        Image<Bgr, Byte> imageSunDemonstration = ImageProcessing.evalResultColored(dmProcessingData, maskImage,
        //            new ColorScheme(""));
        //        Image<Bgr, Byte> calculatedSunMaskImage = imageSunDemonstration.CopyBlank();
        //        calculatedSunMaskImage.Draw(new CircleF(retRoundData.pointfCircleCenter(), (float)retRoundData.DRadius), new Bgr(255, 255, 255), 0);
        //        //imageSunDemonstration = imageSunDemonstration.AddWeighted(calculatedSunMaskImage, 0.7, 0.3, 0.0);
        //        //if (!isCalculatingUsingBgWorker)
        //        //    ServiceTools.ShowPicture(imageSunDemonstration, "Sun position and radius demonstration");


        //        //tmpSunDetectionimage = tmpSunDetectionimage.AddWeighted(calculatedSunMaskImage, 0.7d, 0.4d, 0.0d);
        //        //if (!isCalculatingUsingBgWorker)
        //        //    ServiceTools.ShowPicture(tmpSunDetectionimage, "Sun position and radius demonstration");
        //        //if (verbosityLevel > 1)
        //        //    tmpSunDetectionimage.Save(currentDirectory + randomFileName +
        //        //                              "-Sun-position-and-radius-demonstration.jpg");

        //        //SunMarginGradToShowImage = SunMarginGradToShowImage.AddWeighted(calculatedSunMaskImage, 0.7, 0.3, 0.0);

        //        //if (!isCalculatingUsingBgWorker)
        //        //    ServiceTools.ShowPicture(SunMarginGradToShowImage, "SUN margin image");

        //        //theLogWindow = ServiceTools.LogAText(theLogWindow, "положение центра солнца: " + sunCenterPoint + Environment.NewLine);
        //        //theLogWindow = ServiceTools.LogAText(theLogWindow, "радиус солнечной засветки: " + sunRadius + Environment.NewLine);

        //        #endregion поиск солнца по найденному контуру

        //    }
        //    else
        //    {
        //        theSunSuppressionSchemeApplicable = false;
        //        retRoundData = null;

        //        double overallSunburnAreaPartial = imgSunMask.CountNonzero()[0] / maskImage100.CountNonzero()[0];
        //        if (overallSunburnAreaPartial >= 0.01)
        //        {
        //            currentSunDiskCondition = SunDiskConditions.zero;
        //        }
        //        else
        //        {
        //            currentSunDiskCondition = SunDiskConditions.Cloudy;
        //        }
        //    }

        //    if (!isCalculatingUsingBgWorker)
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, "sun suppression scheme applicable: " + theSunSuppressionSchemeApplicable.ToString());

        //    #endregion оценка применимости алгоритма подавления солнечной засветки

        //    return retRoundData;
        //}
        #endregion //public RoundData detectSun(DenseMatrix dmGrIxData, Image<Gray, Byte> maskImageCutMargins, Image<Gray, Byte> maskImageFull, ImageProcessing imgP, string currentDirectory = "", string randomFileName = "")




        public DenseMatrix dmSkyIndexDataBinary()
        {
            if (cloudSkySeparationValue == 0.0)
            {
                //ThreadSafeOperations.SetTextTB(tbLog, "Не установлена граница SI между небом и облаком для проведения классификации." + Environment.NewLine, true);
                throw new Exception("Не установлено значение границы небо-облако по значению SkyIndex.");
            }

            if (dmSkyIndexData == null)
            {
                Classify();
            }
            DenseMatrix dmSkyIndexDataBinary = (DenseMatrix)dmSkyIndexData.Clone();
            dmSkyIndexDataBinary.MapInplace(new Func<double, double>((x) => { return (x > cloudSkySeparationValue) ? 1.0d : 0.0d; }));
            return dmSkyIndexDataBinary;
        }




        /// <summary>
        /// Classifies using greek scheme
        /// </summary>
        private void ClassifyGreek()
        {
            Image<Gray, Byte> imageBlueChannelByte = LocalProcessingImage[0].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[0];
            Image<Gray, Byte> imageRedChannelByte = LocalProcessingImage[2].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[2];
            Image<Gray, Byte> imageGreenChannelByte = LocalProcessingImage[1].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[1];
            //Image<Gray, int> image1stConditionValue;
            //Image<Gray, Byte> image1stConditionGTZ;
            //Image<Gray, int> image2ndConditionValue;
            //Image<Gray, Byte> image2ndConditionGTZ;
            //Image<Gray, int> image3rdConditionValue;
            //Image<Gray, Byte> image3rdConditionGTZ;
            DenseMatrix dmRed, dmGreen, dmBlue, dm1stCondition, dm2ndCondition, dm3rdCondition, dm60, dm20, dmMask;

            ImageProcessing imp = new ImageProcessing(LocalProcessingImage, false);
            imp.getImageSignificantMask();
            maskImage = imp.significantMaskImage;

            //imageBlueChannelByte = (imageBlueChannelByte & maskImage);
            //imageRedChannelByte = (imageRedChannelByte & maskImage);
            //imageGreenChannelByte = (imageRedChannelByte & maskImage);




            double[,] imageRedChannelDouble2d = ImageProcessing.ThirdIndexArraySliceDouble2DFromByte3D(imageRedChannelByte.Data, 0);
            dmRed = DenseMatrix.OfArray(imageRedChannelDouble2d);
            imageRedChannelDouble2d = null;
            double[,] imageBlueChannelDouble2d = ImageProcessing.ThirdIndexArraySliceDouble2DFromByte3D(imageBlueChannelByte.Data, 0);
            dmBlue = DenseMatrix.OfArray(imageBlueChannelDouble2d);
            imageBlueChannelDouble2d = null;
            double[,] imageGreenChannelDouble2d = ImageProcessing.ThirdIndexArraySliceDouble2DFromByte3D(imageGreenChannelByte.Data, 0);
            dmGreen = DenseMatrix.OfArray(imageGreenChannelDouble2d);
            imageGreenChannelDouble2d = null;
            double[,] imageMaskChannelDouble2d = ImageProcessing.ThirdIndexArraySliceDouble2DFromByte3D((maskImage / 255).Data, 0);
            dmMask = DenseMatrix.OfArray(imageMaskChannelDouble2d);
            imageMaskChannelDouble2d = null;
            imageRedChannelByte.Dispose();
            imageBlueChannelByte.Dispose();
            imageGreenChannelByte.Dispose();
            ServiceTools.FlushMemory();

            dm60 = DenseMatrix.Create(dmRed.RowCount, dmRed.ColumnCount, new Func<int, int, double>((x, y) => { return 60.0; }));
            dm20 = DenseMatrix.Create(dmRed.RowCount, dmRed.ColumnCount, new Func<int, int, double>((x, y) => { return 20.0; }));

            dm60 = (DenseMatrix)dm60.PointwiseMultiply(dmMask);
            dm20 = (DenseMatrix)dm20.PointwiseMultiply(dmMask);
            dmRed = (DenseMatrix)dmRed.PointwiseMultiply(dmMask);
            dmBlue = (DenseMatrix)dmBlue.PointwiseMultiply(dmMask);
            dmGreen = (DenseMatrix)dmGreen.PointwiseMultiply(dmMask);



            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //SkyIndexAnalyzer_AnalysisForm.FlushMemory(ParentForm.textBox1, "#04");

            //MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmSubtractionMatrix = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)denseMatrixBlueChannel.Add(denseMatrixRedChannel);
            //matrixChannelsSubtractionDouble = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)denseMatrixBlueChannel.Subtract(denseMatrixRedChannel);
            //matrixChannelsDivisionDouble = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)matrixChannelsSubtractionDouble.PointwiseDivide(matrixChannelsAdditionDouble);




            //R+20-B > 0
            dm1stCondition = (DenseMatrix)dmRed.Add(dm20);
            dm1stCondition = (DenseMatrix)dm1stCondition.Subtract(dmBlue);
            dm1stCondition.MapInplace(new Func<double, double>(val => (val >= 0.0d) ? (1.0d) : (0.0d)), Zeros.AllowSkip);
            //G+20-B > 0
            dm2ndCondition = (DenseMatrix)dmGreen.Add(dm20);
            dm2ndCondition = (DenseMatrix)dm2ndCondition.Subtract(dmBlue);
            dm2ndCondition.MapInplace(new Func<double, double>(val => (val >= 0.0d) ? (1.0d) : (0.0d)), Zeros.AllowSkip);
            //60 - B > 0
            dm3rdCondition = (DenseMatrix)dm60.Subtract(dmBlue);
            dm3rdCondition.MapInplace(new Func<double, double>(val => (val >= 0.0d) ? (1.0d) : (0.0d)), Zeros.AllowSkip);

            dm1stCondition = (DenseMatrix)dm1stCondition.PointwiseMultiply(dm2ndCondition);
            dm1stCondition = (DenseMatrix)dm1stCondition.PointwiseMultiply(dm3rdCondition);

            double[,] doubleData = dm1stCondition.ToArray();
            byte[,] ByteData = ServiceTools.DoubleToByteDepth(doubleData);
            Image<Gray, Byte> tmpImage1 = new Image<Gray, Byte>(ServiceTools.AddedThirdIndexArray3DFrom2D(ByteData));
            tmpImage1 = tmpImage1.ThresholdBinary(new Gray(0), new Gray(255));

            doubleData = dm2ndCondition.ToArray();
            ByteData = ServiceTools.DoubleToByteDepth(doubleData);
            Image<Gray, Byte> tmpImage2 = new Image<Gray, Byte>(ServiceTools.AddedThirdIndexArray3DFrom2D(ByteData));
            tmpImage2 = tmpImage2.ThresholdBinary(new Gray(0), new Gray(255));

            doubleData = dm3rdCondition.ToArray();
            ByteData = ServiceTools.DoubleToByteDepth(doubleData);
            Image<Gray, Byte> tmpImage3 = new Image<Gray, Byte>(ServiceTools.AddedThirdIndexArray3DFrom2D(ByteData));
            tmpImage3 = tmpImage3.ThresholdBinary(new Gray(0), new Gray(255));


            #region debug xls dump
            //double[,] temparray = dmBlue.ToArray();
            //ExcelDumper xlsdumper = new ExcelDumper("C:\\_gulevlab\\SkyImagesAnalyzer\\app01_data\\2005_I.jpg");
            //xlsdumper.doubleArrayToDump = temparray;
            //xlsdumper.DumpData();
            //temparray = null;
            //xlsdumper.Close();
            #endregion


            Image<Bgr, Byte> previewImage = tmpImage1.Convert<Bgr, Byte>().CopyBlank();
            Image<Bgr, Byte> CloudImage = tmpImage1.Convert<Bgr, Byte>();
            Image<Gray, Byte>[] imagearray1 = new Image<Gray, Byte>[] { maskImage, maskImage.CopyBlank(), maskImage.CopyBlank() };
            Image<Bgr, Byte> SkyImage = new Image<Bgr, byte>(imagearray1);
            previewImage = previewImage.Add(CloudImage);
            previewImage = previewImage.Add(SkyImage);
            localPreviewBitmap = previewImage.Bitmap;
        }





        public void Classify()
        {
            if (ClassificationMethod == ClassificationMethods.Japan)
            {
                ClassifyJapan();
            }
            if (ClassificationMethod == ClassificationMethods.US)
            {
                ClassifyUS();
            }
            else if (ClassificationMethod == ClassificationMethods.Greek)
            {
                ClassifyGreek();
            }
            else if (ClassificationMethod == ClassificationMethods.GrIx)
            {
                ClassifyGrIx();
            }
        }



        public void Dispose()
        {
            LocalProcessingImage.Dispose();
            localPreviewBitmap.Dispose();
            //ClassifiedBM = null;
            //pbPreviewDisplayControl.Dispose();
            //defaultProgressBarControl.Dispose();
            //ParentForm.Dispose();
        }







        public static RoundData ObtainSunPosition(string currImgFilename, Dictionary<string, object> defaultProps, Form currParentForm, LogWindow currImageLogWindow = null, GPSdata gps = null, bool saveObtainedPosition = true)
        {
            #region проверка входных параметров

            if (currImgFilename == "") return null;
            if (!File.Exists(currImgFilename)) return null;
            if (defaultProps == null) return null;
            if (defaultProps.GetType() != typeof(Dictionary<string, object>)) return null;
            if (!defaultProps.Any()) return null;

            #endregion проверка входных параметров

            FileInfo currFileInfo = new FileInfo(currImgFilename);
            Image<Bgr, Byte> img2process = new Image<Bgr, byte>(currFileInfo.FullName);
            img2process = ImageProcessing.ImageResizer(img2process, Convert.ToInt32(defaultProps["DefaultMaxImageSize"]));
            Image<Bgr, Byte> LocalProcessingImage = ImageProcessing.SquareImageDimensions(img2process);

            RoundData sunRoundData = RoundData.nullRoundData();

            //посмотрим, нет ли уже имеющихся данных о положении и размере солнечного диска на изображении
            #region trying to search existing sun disk info
            string sunDiskInfoFileName = currFileInfo.DirectoryName + Path.DirectorySeparatorChar +
                                         Path.GetFileNameWithoutExtension(currFileInfo.FullName) + "-SunDiskInfo.xml";

            RoundData existingRoundData = RoundData.nullRoundData();
            Size imgSizeUnderExistingRoundData = LocalProcessingImage.Bitmap.Size;
            object existingRoundDataObj = ServiceTools.ReadObjectFromXML(sunDiskInfoFileName, typeof(RoundDataWithUnderlyingImgSize));

            if (existingRoundDataObj != null)
            {
                existingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).circle;
                imgSizeUnderExistingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).imgSize;
            }

            double currScale = (double)LocalProcessingImage.Width / (double)imgSizeUnderExistingRoundData.Width;
            if (currScale != 1.0d)
            {
                existingRoundData.DCenterX *= currScale;
                existingRoundData.DCenterY *= currScale;
                existingRoundData.DRadius *= currScale;
            }
            if (!existingRoundData.IsNull)
            {
                sunRoundData = existingRoundData;
            }

            #endregion trying to search existing sun disk info


            ImageProcessing imgP = new ImageProcessing(LocalProcessingImage, true);

            if (sunRoundData.IsNull)
            {
                List<Tuple<string, string>> lImagesRoundMasksMapping = null;
                if (defaultProps.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
                {

                    string ImagesRoundMasksXMLfilesMappingList =
                        (string)defaultProps["ImagesRoundMasksXMLfilesMappingList"];
                    if (File.Exists(ImagesRoundMasksXMLfilesMappingList))
                    {
                        List<List<string>> llImagesRoundMasksMappingFiles =
                            ServiceTools.ReadDataFromCSV(ImagesRoundMasksXMLfilesMappingList, 0, true, ";",
                                Environment.NewLine);
                        lImagesRoundMasksMapping =
                            llImagesRoundMasksMappingFiles.ConvertAll(
                                list => new Tuple<string, string>(list[0], list[1]));
                        // item1: images filename pattern
                        // item2: image rounded mask parameters XML file
                    }
                }
                RoundData predefinedRoundedMask = null;
                if (lImagesRoundMasksMapping != null)
                {
                    if (lImagesRoundMasksMapping.Any())
                    {
                        if (lImagesRoundMasksMapping.Find(tpl => (new WildcardPattern(tpl.Item1)).IsMatch(currImgFilename)) != null)
                        {
                            string strFoundPredefinedRoundedMaskParametersXMLfile =
                                lImagesRoundMasksMapping.Find(
                                    tpl => (new WildcardPattern(tpl.Item1)).IsMatch(currImgFilename)).Item2;
                            predefinedRoundedMask =
                                ServiceTools.ReadObjectFromXML(strFoundPredefinedRoundedMaskParametersXMLfile,
                                    typeof(RoundData)) as RoundData;
                        }
                    }
                }

                if (predefinedRoundedMask != null)
                {
                    imgP = new ImageProcessing(LocalProcessingImage, predefinedRoundedMask);
                }
                else
                {
                    imgP = new ImageProcessing(LocalProcessingImage, true);
                }





                SkyCloudClassification classificator = new SkyCloudClassification(img2process, defaultProps)
                {
                    verbosityLevel = 0,
                    ParentForm = currParentForm,
                    theLogWindow = currImageLogWindow,
                    ClassificationMethod = ClassificationMethods.GrIx,
                    isCalculatingUsingBgWorker = false,
                    defaultOutputDataDirectory = (string) defaultProps["DefaultDataFilesLocation"],
                    theStdDevMarginValueDefiningSkyCloudSeparation =
                        Convert.ToDouble(defaultProps["GrIxDefaultSkyCloudMarginWithoutSun"]),
                    sourceImageFileName = currFileInfo.FullName
                };

                
                #region try to filter data and detect sun disk position

                DenseMatrix dmGrIxValues = (DenseMatrix)imgP.eval("grix").Clone();
                DenseMatrix dmGrIxGradValues = (DenseMatrix)imgP.eval("grad(grix)").Clone();


                //filter by GrIx values itself
                // dmGrIxValues.MapInplace(dVal => ((dVal >= 0.75d) || (dVal <= 0.2d)) ? (double.NaN) : (dVal));

                //filter by grad(GrIx) values

                // dmGrIxValues.MapIndexedInplace((r, c, dVal) => (dmGrIxGradValues[r,c] >= 0.05d) ?(0.0d):(dVal));

                string outputFilenameGrIx = @"D:\_gulevlab\SkyImagesAnalysis_appData\003\testFilteredGrIxField.nc";
                string outputFilenameGrIxGrad = @"D:\_gulevlab\SkyImagesAnalysis_appData\003\testFilteredGrIxGrad.nc";
                NetCDFoperations.SaveDataToFile(dmGrIxValues, outputFilenameGrIx, null, true);
                NetCDFoperations.SaveDataToFile(dmGrIxGradValues, outputFilenameGrIxGrad, null, true);

                #endregion try to filter data and detect sun disk position


                //try
                //{
                //    sunRoundData = classificator.DetectSunWithSerieOfArcs(imgP, dmGrIxValues);
                //    if (!sunRoundData.IsNull)
                //    {
                //        RoundDataWithUnderlyingImgSize infoToSave = new RoundDataWithUnderlyingImgSize()
                //        {
                //            circle = sunRoundData,
                //            imgSize = LocalProcessingImage.Size,
                //        };
                //        ServiceTools.WriteObjectToXML(infoToSave, sunDiskInfoFileName);
                //        return sunRoundData;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    currImageLogWindow = ServiceTools.LogAText(currImageLogWindow, "exception caught: " + ex.Message);
                //    return null;
                //}
                //ServiceTools.FlushMemory();
            }
            return null;
        }




















        #region // obsolete - moved to separate .cs file

        //private class ConcurrentData
        //{
        //    public string filename { get; set; }
        //    public DateTime datetimeUTC { get; set; }
        //    public double AccCalibrationValueX { get; set; }
        //    public double AccCalibrationValueY { get; set; }
        //    public double AccCalibrationValueZ { get; set; }
        //    public double AccShiftDoubleX { get; set; }
        //    public double AccShiftDoubleY { get; set; }
        //    public double AccShiftDoubleZ { get; set; }

        //    public double GyroValueX { get; set; }
        //    public double GyroValueY { get; set; }
        //    public double GyroValueZ { get; set; }

        //    public string GPSdata { get; set; }
        //    public double GPSLat { get; set; }
        //    public double GPSLon { get; set; }
        //    public DateTime GPSDateTimeUTC { get; set; }
        //    public double PressurePa { get; set; }

        //    public GPSdata gps;


        //    public ConcurrentData() { }

        //    public ConcurrentData(Dictionary<string, object> XMLfileContentDictionary)
        //    {
        //        filename = (string)XMLfileContentDictionary["XMLfileName"];
        //        datetimeUTC = DateTime.Parse((string)XMLfileContentDictionary["DateTime"], null,
        //            System.Globalization.DateTimeStyles.AdjustToUniversal);
        //        AccCalibrationValueX = Convert.ToDouble(((string)XMLfileContentDictionary["AccCalibrationValueX"]).Replace(".", ","));
        //        AccCalibrationValueY = Convert.ToDouble(((string)XMLfileContentDictionary["AccCalibrationValueY"]).Replace(".", ","));
        //        AccCalibrationValueZ = Convert.ToDouble(((string)XMLfileContentDictionary["AccCalibrationValueZ"]).Replace(".", ","));
        //        AccShiftDoubleX = Convert.ToDouble(((string)XMLfileContentDictionary["AccShiftDoubleX"]).Replace(".", ","));
        //        AccShiftDoubleY = Convert.ToDouble(((string)XMLfileContentDictionary["AccShiftDoubleY"]).Replace(".", ","));
        //        AccShiftDoubleZ = Convert.ToDouble(((string)XMLfileContentDictionary["AccShiftDoubleZ"]).Replace(".", ","));

        //        GyroValueX = Convert.ToDouble(((string)XMLfileContentDictionary["GyroValueX"]).Replace(".", ","));
        //        GyroValueY = Convert.ToDouble(((string)XMLfileContentDictionary["GyroValueY"]).Replace(".", ","));
        //        GyroValueZ = Convert.ToDouble(((string)XMLfileContentDictionary["GyroValueZ"]).Replace(".", ","));

        //        if (XMLfileContentDictionary["GPSdata"] is DBNull)
        //        {
        //            throw new Exception("GPSdata string of the XML file dictionary is empty");
        //        }

        //        GPSdata = (string)XMLfileContentDictionary["GPSdata"];
        //        GPSLat = Convert.ToDouble(((string)XMLfileContentDictionary["GPSLat"]).Replace(".", ","));
        //        GPSLon = Convert.ToDouble(((string)XMLfileContentDictionary["GPSLon"]).Replace(".", ","));
        //        GPSDateTimeUTC = DateTime.Parse((string)XMLfileContentDictionary["GPSDateTimeUTC"], null,
        //            System.Globalization.DateTimeStyles.AdjustToUniversal);
        //        PressurePa = Convert.ToDouble(((string)XMLfileContentDictionary["PressurePa"]).Replace(".", ","));

        //        gps = new GPSdata()
        //        {
        //            GPSstring = GPSdata,
        //            lat = Math.Abs(GPSLat),
        //            latHemisphere = ((GPSLat >= 0.0d) ? ("N") : ("S")),
        //            lon = Math.Abs(GPSLon),
        //            lonHemisphere = ((GPSLon >= 0) ? ("E") : ("W")),
        //            dateTimeUTC = GPSDateTimeUTC,
        //            validGPSdata = true,
        //            dataSource = GPSdatasources.CloudCamArduinoGPS
        //        };
        //    }
        //}


        #endregion

    }
}