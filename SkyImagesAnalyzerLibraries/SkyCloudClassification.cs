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
using MathNet.Numerics.LinearAlgebra;


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

        public SunDiskConditions currentSunDiskCondition;


        private Image<Bgr, byte> tmpSunDetectionimage = null;

        public bool theSunSuppressionSchemeApplicable = false;
        
        


        public Bitmap PreviewBitmap{get{return localPreviewBitmap;}}



        public Bitmap BitmapinProcess
        {
            get{return LocalProcessingImage.Bitmap;}
            set{LocalProcessingImage = new Image<Bgr, byte>(value);}
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


        //public SkyCloudClassification(Image OrigImage, PictureBox DisplayControl, ProgressBar ProgressBarControl, Form inParentForm)
        //public SkyCloudClassification(Image OrigImage, PictureBox DisplayControl, ProgressBar ProgressBarControl, TextBox inReportingTextBox)
        //public SkyCloudClassification(Image OrigImage, Dictionary<string, object> settings)
        public SkyCloudClassification(Image<Bgr, Byte> OrigImage, Dictionary<string, object> settings)
        {
            //LocalProcessingBitmap = new Bitmap(ImageProcessing.SquareImageDimensions(OrigImage));
            LocalProcessingImage = ImageProcessing.SquareImageDimensions(OrigImage);

            localPreviewBitmap = new Bitmap(LocalProcessingImage.Bitmap, LocalProcessingImage.Width, LocalProcessingImage.Height);
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
            theStdDevMarginValueDefiningTrueSkyArea = Convert.ToDouble(defaultProperties["GrIxStdDevMarginValueDefiningTrueSkyArea"]);
            theStdDevMarginValueDefiningSkyCloudSeparation = Convert.ToDouble(defaultProperties["GrIxDefaultSkyCloudMarginWithoutSun"]);
            theStdDevMarginValueDefiningSkyCloudSeparation_SunSuppressed = Convert.ToDouble(defaultProperties["GrIxDefaultSkyCloudMarginWithSun"]);
            theImageCircleCropFactor = Convert.ToDouble(defaultProperties["GrIxAnalysisImageCircledCropFactor"]);
            minSunburnYValue = Convert.ToDouble(defaultProperties["GrIxMinimalSunburnYvalue"]);
            minSunburnGrIxValue = Convert.ToDouble(defaultProperties["GrIxMinimalSunburnGrIxvalue"]);
            dSunDetectorArcedCropFactor = Convert.ToDouble(defaultProperties["GrIxSunDetectorArcedCropFactor"]);
            concurrentThreadsLimit = Convert.ToInt32(defaultProperties["GrIxSunDetectorConcurrentThreadsLimit"]);
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
            ImageProcessing imp = new ImageProcessing(LocalProcessingImage);
            imp.getImageSignificantMask();
            maskImage = imp.significantMaskImageBinary;

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



        #region классификация методом из немецких публикаций
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
            ImageProcessing imp = new ImageProcessing(LocalProcessingImage);
            imp.getImageSignificantMask();
            maskImage = imp.significantMaskImageBinary;

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
        #endregion классификация методом из японских публикаций










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
            ImageProcessing imgP = new ImageProcessing(LocalProcessingImage, true);
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


            randomFileName = "m" + System.IO.Path.GetRandomFileName().Replace(".", "");


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


            #region надо отобразить положение снимка в пространстве 5perc-median. Исходную картинку с распределением - брать готовую или генерировать?..

            #endregion



            #region определим CloudCover без подавления солнца - для сравнения
            //ColorScheme skyCloudColorSchemeWithoutSunSuppression = ColorScheme.InversedBinaryCloudSkyColorScheme(theStdDevMarginValueDefiningSkyCloudSeparation, 0.0d, 1.0d);
            ColorScheme skyCloudColorSchemeWithoutSunSuppression = ColorScheme.InversedBinaryCloudSkyColorScheme(theStdDevMarginValueDefiningSkyCloudSeparation, 0.0d, 1.0d);
            Image<Bgr, Byte> previewImageWithoutSunSuppression = ImageProcessing.evalResultColoredWithFixedDataBounds(dmProcessingData, maskImage, skyCloudColorSchemeWithoutSunSuppression, 0.0d, 1.0d);
            int cloudCounterWithoutSunSuppression = previewImageWithoutSunSuppression.CountNonzero()[1];
            int skyCounterWithoutSunSuppression = maskImage.CountNonzero()[0] - cloudCounterWithoutSunSuppression;
            double CloudCoverWithoutSunSuppression = (double)cloudCounterWithoutSunSuppression / (double)(skyCounterWithoutSunSuppression + cloudCounterWithoutSunSuppression);
            //сохраним результат без подавления засветки
            //потом еще надо скомпоновать результат-композит
            if (verbosityLevel > 1)
            {
                previewImageWithoutSunSuppression.Save(currentDirectory+randomFileName + "-GrIx-result-withoutSunSuppression.jpg");
            }
            #endregion


            ServiceTools.FlushMemory();

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
            bool predefinedSunDiskLocationWasUsed = false;
            if (sourceImageFileName != "")
            {
                //посмотрим, нет ли уже имеющихся данных о положении и размере солнечного диска на изображении
                FileInfo fInfo1 = new FileInfo(sourceImageFileName);
                string sunDiskInfoFileName = fInfo1.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(sourceImageFileName) +
                                             "-SunDiskInfo.xml";

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
                sunRoundData = DetectSunWithSerieOfArcs(imgP, dmProcessingData, currentDirectory, randomFileName);
            }
            

            theSunSuppressionSchemeApplicable = (!sunRoundData.IsNull);

            BGWorkerReport("закончен поиск солнечного диска. применимость подавления засветки: " + theSunSuppressionSchemeApplicable.ToString());



            #region сохраним данные положения солнечного диска
            if (!sunRoundData.IsNull)
            {
                BGWorkerReport("расположение солнечного диска: " + Environment.NewLine + sunRoundData.ToString());
                string strGeometryData = "" + imgP.imageRD.DCenterX.ToString("e").Replace(",", ".") +
                                         Environment.NewLine +
                                         imgP.imageRD.DCenterY.ToString("e").Replace(",", ".") + Environment.NewLine +
                                         imgP.imageRD.DRadius.ToString("e").Replace(",", ".") + Environment.NewLine +
                                         sunRoundData.DCenterX.ToString("e").Replace(",", ".") + Environment.NewLine +
                                         sunRoundData.DCenterY.ToString("e").Replace(",", ".") + Environment.NewLine +
                                         sunRoundData.DRadius.ToString("e").Replace(",", ".") + Environment.NewLine;
                if (verbosityLevel > 0)
                {
                    ServiceTools.logToTextFile(currentDirectory + randomFileName + "_GeometryData.csv", strGeometryData,
                        true);
                }


                if (sourceImageFileName != "")
                {
                    // посмотрим, нет ли уже имеющихся данных о положении и размере солнечного диска на изображении
                    // если есть - не будем перезаписывать
                    FileInfo fInfo1 = new FileInfo(sourceImageFileName);
                    string sunDiskInfoFileName = fInfo1.DirectoryName + "\\" +
                                                 Path.GetFileNameWithoutExtension(sourceImageFileName) +
                                                 "-SunDiskInfo.xml";
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
                dmSunburnProfileDetection = (DenseMatrix)dmSunburnProfileDetection.PointwiseMultiply(dmMaskCircled);


                #region // unused
                //double theSkyWeightSum = theSkyWeightAbs.Values.Sum();
                //DenseMatrix theSkyWeight = (DenseMatrix)(theSkyWeightAbs.Copy()/theSkyWeightSum);
                ////theSkyWeight.MapInplace(new Func<double, double>(val => val / theSkyWeightSum));
                #endregion // unused

                DenseMatrix dmDistanceToSunMargin = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                    new Func<int, int, double>(
                        (row, column) =>
                        {
                            double dx = (double)column - sunCenterPoint.X;
                            double dy = (double)row - sunCenterPoint.Y;
                            double r = Math.Sqrt(dx * dx + dy * dy) - sunRadius;
                            r = (r < 0.0d) ? (0.0d) : (r);
                            return r;
                        }));
                dmDistanceToSunMargin = (DenseMatrix)dmDistanceToSunMargin.PointwiseMultiply(dmMask);

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


                DenseMatrix dmAngleAroundTheSunburn = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                    new Func<int, int, double>(
                        (row, column) =>
                        {
                            double dx = (double)column - sunCenterPoint.X;
                            double dy = (double)row - sunCenterPoint.Y;
                            double r = dmDistanceToSunMargin[row, column] + sunRadius;
                            double cosPhi = dx / r;
                            double phi = Math.Acos(cosPhi);
                            if (dy > 0) phi = 2.0d * Math.PI - phi;
                            if (double.IsNaN(phi)) phi = 0.0d;
                            return phi;
                        }));
                dmAngleAroundTheSunburn = (DenseMatrix)dmAngleAroundTheSunburn.PointwiseMultiply(dmMask);

                double rMax = dmDistanceToSunMargin.Values.Max();
                //это расстояние уже в pixels, ничего пересчитывать не надо
                double minDataValue = 1.0d;
                double Rs = 0.0d;
                List<double> envelopValues = new List<double>();
                List<double> envelopRargument = new List<double>();


                #region здесь фильтруем шумы и выбросы ниже основной очевидной огибающей
                for (int r = 0; r < rMax; r++)
                {
                    double rMinlocal = (double)r;
                    double rMaxLocal = (double)(r + 1);


                    DenseMatrix dmTemporaryProcessingDataMatrix = DenseMatrix.Create(dmSunburnProfileDetection.RowCount, dmSunburnProfileDetection.ColumnCount,
                        new Func<int, int, double>((row, column) =>
                        {
                            double currDist = dmDistanceToSunMargin[row, column];
                            if ((currDist >= rMinlocal) && (currDist < rMaxLocal)) return dmSunburnProfileDetection[row, column];
                            else return 0.0d;
                        }));

                    DenseVector dvCurrentDataExcludingZero =
                        DataAnalysis.DataVectorizedExcludingValues(dmTemporaryProcessingDataMatrix, 0.0d);
                    double curMedian = 0.0d;
                    DescriptiveStatistics statTempData = DataAnalysis.StatsOfDataExcludingValues(dmTemporaryProcessingDataMatrix, 0.0d, out curMedian);
                    if (statTempData == null) continue;


                    double dataPercentile10 = Statistics.Percentile(dvCurrentDataExcludingZero, 10);
                    
                    DenseVector listTempStatData =
                        DataAnalysis.DataVectorizedWithCondition(dmTemporaryProcessingDataMatrix,
                            (x => x >= dataPercentile10));

                    double dataMin = 1.0d;
                    if (listTempStatData != null) dataMin = listTempStatData.Min();
                    else continue;

                    envelopValues.Add(dataMin);
                    envelopRargument.Add((double)r);
                }
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
                #endregion

                // Определим, как выглядит функция от аргумента
                //Func<DenseVector, double, double> theEvenlopApproximationFunction =
                //    ((dvPolynomeKoeffs_except0th, dRVal) =>
                //    {
                //        DenseVector dvPolynomeKoeffs =
                //            DenseVector.OfEnumerable(
                //                DenseVector.Create(1, minSunburnGrIxValue).Concat(dvPolynomeKoeffs_except0th));
                //        return DataAnalysis.PolynomeValue(dvPolynomeKoeffs, dRVal);
                //    });


                // Определим, как выглядит функция от аргумента
                // при этом надо еще зафиксировать вторую точку - на правой границе области
                // для этого надо наложить условие на коэффициенты - будет еще минус одно значение
                Func<DenseVector, double, double> theEvenlopApproximationFunction =
                    ((dvPolynomeKoeffs_except0th, dRVal) =>
                    {
                        DenseVector dvPolynomeKoeffs =
                            DenseVector.OfEnumerable(
                                DenseVector.Create(1, minSunburnGrIxValue).Concat(dvPolynomeKoeffs_except0th));
                        return DataAnalysis.PolynomeValue(dvPolynomeKoeffs, dRVal);
                    });



                // создаем сам объект, который будет аппроксимировать
                GradientDescentApproximator evenlopApproximator = new GradientDescentApproximator(dvDataValues,
                                                                                                    dvDataSpace,
                                                                                                    theEvenlopApproximationFunction);


                //int polynomeOrder = 6;
                const int polynomeOrder3 = 3;
                const int polynomeOrder6 = 6;
                
                DenseVector dvInitialParameters_EvenlopApprox3 = DenseVector.Create(polynomeOrder3, 0.0d);
                DenseVector dvInitialParameters_EvenlopApprox6 = DenseVector.Create(polynomeOrder6, 0.0d);

                // собственно аппроксимация
                DenseVector approxPolyKoeffs6 = evenlopApproximator.Approximation_ILOptimizer(dvInitialParameters_EvenlopApprox6);
                
                // для кубической применим веса значений - рассчет см.выше
                evenlopApproximator.DvWeights = dvWeights_EvenlopApprox;
                DenseVector approxPolyKoeffs3 = evenlopApproximator.Approximation_ILOptimizer(dvInitialParameters_EvenlopApprox3);
                

                // добавим в нулевую позицию зафиксированный элемент
                approxPolyKoeffs3 =
                    DenseVector.OfEnumerable(DenseVector.Create(1, minSunburnGrIxValue).Concat(approxPolyKoeffs3));
                approxPolyKoeffs6 =
                    DenseVector.OfEnumerable(DenseVector.Create(1, minSunburnGrIxValue).Concat(approxPolyKoeffs6));
                


                #region plot evenlop approximation summary
                MultipleScatterAndFunctionsRepresentation weightAndDataPlot =
                    new MultipleScatterAndFunctionsRepresentation(1024, 768);
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
                ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate()
                {
                    ServiceTools.ShowPicture(weightAndDataPlot.TheImage.Bitmap, "Overall GrIx values envelop vs r");
                    weightAndDataPlot.TheImage.Save(currentDirectory + randomFileName + "-envelop-approximation-result-with-weights.jpg");
                });
                

                MultipleScatterAndFunctionsRepresentation envelopPlottingForm =
                    new MultipleScatterAndFunctionsRepresentation(1024, 768);
                envelopPlottingForm.dvScatterXSpace.Add(dvDataSpace);
                envelopPlottingForm.dvScatterFuncValues.Add(dvDataValues);
                envelopPlottingForm.scatterLineColors.Add(new Bgr(Color.Red));
                envelopPlottingForm.scatterDrawingVariants.Add(SequencesDrawingVariants.circles);
                
                envelopPlottingForm.theRepresentingFunctions.Add(DataAnalysis.PolynomeValue);
                envelopPlottingForm.parameters.Add(approxPolyKoeffs3);
                envelopPlottingForm.lineColors.Add(new Bgr(Color.Green));

                envelopPlottingForm.theRepresentingFunctions.Add(DataAnalysis.PolynomeValue);
                envelopPlottingForm.parameters.Add(approxPolyKoeffs6);
                envelopPlottingForm.lineColors.Add(new Bgr(Color.Blue));

                envelopPlottingForm.Represent();
                ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate()
                {
                    ServiceTools.ShowPicture(envelopPlottingForm.TheImage.Bitmap, "Overall GrIx values envelop vs r");
                    envelopPlottingForm.TheImage.Save(currentDirectory + randomFileName + "-envelop-approximation-result.jpg");
                });
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
                //envelopPlottingForm = new MultipleScatterAndFunctionsRepresentation(1024, 768);
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

                // int angleBinsCount = 144;
                int angleBinsCount = 180;

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

                // предполагаем, что наиболее вероятная симметрия - 
                // относительно прямой, соединяющей центр солнечного диска и центр изображения
                // поэтому начальное значение смещения phi0 для аппроксимации зависимости расположения
                // Rm от phi можно оценить исходя из расположения солнечного диска относительно центра изображения.
                // Здесь Rm - насстояние от минимального значения GrIx в направлении phi до центра солнечного диска.
                // phi - угол в полярной системе координат с началом координат в центре солнечного диска.
                // Угол отсчитывается от горизонтальной прямой. Нулевое значение угла - навправо.
                double dxSunToCenter = (double)imageCircleCenter.X - (double)sunCenterPoint.X;
                double dySunToCenter = (double)imageCircleCenter.Y - (double)sunCenterPoint.Y;
                double rSunToCenter = Math.Sqrt(dxSunToCenter * dxSunToCenter + dySunToCenter * dySunToCenter);
                double cosPhi1 = dxSunToCenter / rSunToCenter;
                double Phi1 = Math.Acos(cosPhi1);
                if (dySunToCenter > 0) Phi1 = 2.0d * Math.PI - Phi1;
                //angleValuesForDistribution.MapInplace(new Func<double, double>(dVal => dVal - Phi1));


                //DenseMatrix dmPolarSystemMinimumsDistribution = DataAnalysis.DecartToPolar2(dmSunburnProfileDetection,
                //        sunCenterPoint, angleBinsCount);
                DenseMatrix dmPolarSystemMinimumsDistribution = DataAnalysis.DecartToPolar(dmSunburnProfileDetection, sunCenterPoint, angleBinsCount);
                DenseMatrix dmPolarMinimumsDistribution =
                    DataAnalysis.GetLocalMinimumsDistribution(dmPolarSystemMinimumsDistribution, sunCenterPoint,
                        imgP.imageRD.pointDCircleCenter(), imgP.imageRD.DRadius, dmSunburnProfileDetection.RowCount, theImageCircleCropFactor);
                DenseMatrix dmDecartMinimumsDistribution = DataAnalysis.PolarToDecart(dmPolarMinimumsDistribution,
                        sunCenterPoint, dmSunburnProfileDetection.ColumnCount, dmSunburnProfileDetection.RowCount);
                //надо отфильтровать краевые минимумы на границе кадра - DONE

                //надо выделить данные зависимости локальных минимумов от угла
                //аппроксимировать эту зависимость
                //получим параметры распределения для конкретного изображения - их можно использовать дальше


                #region запись данных для просмотра и анализа
                if (verbosityLevel > 1)
                {
                    #region //obsolete
                    //ImageConditionAndDataRepresentingForm theForm =
                    //    ServiceTools.RepresentDataFromDenseMatrix(dmPolarSystemMinimumsDistribution,
                    //        "minimums distribution in polar coordinates", false, false, 0.0d, 1.0d, false);
                    //theForm.SaveData(currentDirectory + randomFileName + "_SunburnProfilePolar.nc", true);
                    ////theForm.Close();
                    //theForm.Dispose();
                    #endregion //obsolete

                    dmPolarSystemMinimumsDistribution.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                           "_SunburnProfilePolar.nc");
                    #region //obsolete
                    //ImageConditionAndDataRepresentingForm theForm1 =
                    //    ServiceTools.RepresentDataFromDenseMatrix(dmPolarMinimumsDistribution,
                    //        "minimums distribution in polar coordinates", false, false, 0.0d, 1.0d, false);
                    //theForm1.SaveData(currentDirectory + randomFileName + "_SunburnProfileMinimums.nc", true);
                    ////theForm1.Close();
                    //theForm1.Dispose();
                    #endregion //obsolete

                    dmPolarMinimumsDistribution.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                     "_SunburnProfileMinimums.nc");

                    #region //obsolete
                    //ImageConditionAndDataRepresentingForm theForm2 =
                    //    ServiceTools.RepresentDataFromDenseMatrix(dmDecartMinimumsDistribution,
                    //        "minimums distribution in decart coordinates", false, false, 0.0d, 1.0d, false);
                    //theForm2.SaveData(currentDirectory + randomFileName + "_SunburnProfileMinimumsDecart.nc", true);
                    ////theForm2.Close();
                    //theForm2.Dispose();
                    ////ImageConditionAndDataRepresentingForm TheAngleRepresentation =
                    ////    ServiceTools.RepresentDataFromDenseMatrix(angleValuesForDistribution, "the angle");
                    ////TheAngleRepresentation.SaveData(randomFileName + "_SunburnProfileMinimumsPolarAngles.nc");
                    #endregion //obsolete

                    dmDecartMinimumsDistribution.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                      "_SunburnProfileMinimumsDecart.nc");
                }
                #endregion запись данных для просмотра и анализа


                //if (!isCalculatingUsingBgWorker)
                //{
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "смещение по углу: " + Phi1.ToString("e"));
                //}
                #endregion здесь исследуем распределение локальных минимумов распределения GrIx по r в полярной системе координат


                


                
                




                #region аппроксимация распределения Rm(phi)
                BGWorkerReport("Готовим данные для аппроксимации зависимости Rm(phi), выполняем ее методом градиентного спуска");

                #region формируем данные для аппроксимации зависимости Rm(phi) и выполняем саму аппроксимацию
                List<double> phiMinimumsSpace = new List<double>();
                List<double> rMinimumsSpace = new List<double>();
                List<double> dataMinimumsList = new List<double>();
                foreach (Tuple<int, Vector<double>> tuple in dmPolarMinimumsDistribution.EnumerateRowsIndexed())
                {
                    List<double> angleRowVector = new List<double>(tuple.Item2);

                    if (angleRowVector.Sum() == 0.0d) continue;

                    double currentAngle = 2.0d * Math.PI * (double)tuple.Item1 / (double)(angleBinsCount);
                    double currRadius = angleRowVector.FindIndex(x => x > 0.0d) - sunRadius;

                    if (currRadius <= 0.0d) continue;

                    double currMinimumValue = angleRowVector.Find(x => x > 0.0d);
                    phiMinimumsSpace.Add(currentAngle);
                    rMinimumsSpace.Add(currRadius);
                    dataMinimumsList.Add(currMinimumValue);
                }


                
                if (dataMinimumsList.Count < 10)
                {
                    theSunSuppressionSchemeApplicable = false;

                    theLogWindow =
                        ServiceTools.LogAText(theLogWindow, "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + Environment.NewLine +
                                              "Слишком нестабильный результат с подавлением засветки. Используем алгоритм без подавления. Case 01" +
                                              Environment.NewLine + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    
                    
                    break;
                }
                

                Func<DenseVector, double, double> theApproximatinFunction = new Func<DenseVector, double, double>(
                    (dvParameters, phi) =>
                    {
                        double d1 = dvParameters[0];
                        double d2 = dvParameters[1];
                        double r = dvParameters[2];
                        double phi0 = dvParameters[3];
                        return d1 * Math.Cos(phi - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(phi - phi0), 2.0d));
                    });

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

                List<Func<DenseVector, double, double>> theApproximatinFunctionParametersConditionsInequality =
                    new List<Func<DenseVector, double, double>>();
                theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => -dvPar[0]);
                theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => -dvPar[1]);
                theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => -dvPar[2]);
                theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => dvPar[0] - dvPar[2]);
                theApproximatinFunctionParametersConditionsInequality.Add((dvPar, x) => dvPar[1] - dvPar[2]);


                DenseVector dvWeights = DenseVector.Create(dataMinimumsList.Count, new Func<int, double>(i =>
                {
                    double weight = (0.6d +
                            0.4d * (dataMinimumsList[i] - dataMinimumsList.Min()) /
                            (dataMinimumsList.Max() - dataMinimumsList.Min()));
                    weight *= (1.0d + 0.2d * Math.Pow(Math.Cos((phiMinimumsSpace[i] - Phi1)), 2.0d));

                    return weight;
                }));


                DenseVector dvRMinimumsSpace = DenseVector.OfEnumerable(rMinimumsSpace);
                DenseVector dvphiMinimumsSpace = DenseVector.OfEnumerable(phiMinimumsSpace);
                GradientDescentApproximator approximator = new GradientDescentApproximator( dvRMinimumsSpace,
                                                                                            dvphiMinimumsSpace,
                                                                                            theApproximatinFunction);
                approximator.DvWeights = dvWeights;
                approximator.parametersConditionsLessThan0 = theApproximatinFunctionParametersConditionsInequality;

                if (verbosityLevel > 0)
                {
                    foreach (double weight in dvWeights)
                        ServiceTools.logToTextFile(currentDirectory + randomFileName + "_dataWeights.csv",
                            weight.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                    foreach (double rMinimum in dvRMinimumsSpace)
                        ServiceTools.logToTextFile(currentDirectory + randomFileName + "_dataMinimumR.csv",
                            rMinimum.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                    foreach (double phiMinimum in dvphiMinimumsSpace)
                        ServiceTools.logToTextFile(currentDirectory + randomFileName + "_dataMinimumPhi.csv",
                            phiMinimum.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                }


                #region obsolete
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
                #endregion obsolete


                double initialParametersKoeff = (double)Rs / (rSunToCenter + imgP.imageRD.DRadius);// / максимальное расстояние до края картинки в этом направлении = это D+R
                List<double> initialParametersList = new List<double>();
                initialParametersList.Add(rSunToCenter * initialParametersKoeff);
                initialParametersList.Add(rSunToCenter * initialParametersKoeff);
                initialParametersList.Add(imgP.imageRD.DRadius * initialParametersKoeff);
                initialParametersList.Add(Phi1);
                DenseVector dvInitialParameters = DenseVector.OfEnumerable(initialParametersList);
                DenseVector initialParametersIncremnt = DenseVector.Create(dvInitialParameters.Count, (i => 1.0d));

                if (verbosityLevel > 0)
                {
                    foreach (double initialParameter in dvInitialParameters)
                    {
                        ServiceTools.logToTextFile(
                            currentDirectory + randomFileName + "_approximationInitialParameters.csv",
                            initialParameter.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                    }
                }

                FunctionRepresentationForm theFunctionsForm =
                    new FunctionRepresentationForm("r(phi) distribution data, initial approximation and the gradient descent result");
                theFunctionsForm.yAxisNote = "r, distance to sunburn margin (px)";
                theFunctionsForm.xAxisNote = "\\phi, (rad)";

                if (!isCalculatingUsingBgWorker)
                {
                    theFunctionsForm.theRepresentingFunctions.Add(theApproximatinFunction);
                    theFunctionsForm.parameters.Add(dvInitialParameters);
                    theFunctionsForm.lineColors.Add(new Bgr(0, 0, 255));
                    theFunctionsForm.dvScatterXSpace = DenseVector.OfEnumerable(phiMinimumsSpace);
                    theFunctionsForm.dvScatterFuncValues = DenseVector.OfEnumerable(rMinimumsSpace);
                    theFunctionsForm.Show();
                    theFunctionsForm.Represent();
                }
                //DenseVector approximatedParameters = approximator.ApproximationGradientDescentMultidim(dvInitialParameters, ref initialParametersIncremnt, 0.0000001d);
                DenseVector approximatedParameters = approximator.Approximation_ILOptimizer(dvInitialParameters);


                //попробуем вот так:
                // в каждой точке, где определены экспериментальные данные, посчитаем отклонение от аппроксимационной функции
                // посчитаем по этим отклонениям статистику и отяильтруем точки за пределами например стандартного отклонения
                // еще раз прогоним аппроксимацию

                DenseVector dvTempDeviations = DenseVector.Create(dvRMinimumsSpace.Count, new Func<int, double>(i =>
                {
                    return dvRMinimumsSpace[i] - theApproximatinFunction(approximatedParameters, dvphiMinimumsSpace[i]);
                }));
                DescriptiveStatistics deviationsStats = new DescriptiveStatistics(dvTempDeviations.Values);
                List<double> tunedPhiSpace = new List<double>();
                List<double> tunedRSpace = new List<double>();
                for (int i = 0; i < dvTempDeviations.Count; i++)
                {
                    if (Math.Abs(dvTempDeviations[i]) > deviationsStats.StandardDeviation)
                    {
                        continue;
                    }
                    tunedPhiSpace.Add(dvphiMinimumsSpace[i]);
                    tunedRSpace.Add(dvRMinimumsSpace[i]);
                }
                DenseVector dvTunedPhiSpace = DenseVector.OfEnumerable(tunedPhiSpace);
                DenseVector dvTunedRSpace = DenseVector.OfEnumerable(tunedRSpace);
                DenseVector dvTunedWeights = DenseVector.Create(tunedRSpace.Count, new Func<int, double>(i =>
                {
                    double weight = (0.6d +
                            0.4d * (tunedRSpace[i] - tunedRSpace.Min()) /
                            (tunedRSpace.Max() - tunedRSpace.Min()));

                    return weight;
                }));

                approximator.DvWeights = dvTunedWeights;
                approximator.dvSpace = dvTunedPhiSpace;
                approximator.dvDataValues = dvTunedRSpace;
                //approximatedParameters = approximator.ApproximationGradientDescentMultidim(approximatedParameters, ref initialParametersIncremnt, 0.000001d);
                approximatedParameters = approximator.Approximation_ILOptimizer(approximatedParameters);
                // фак
                // откуда-то NaN вылез :((



                if (!isCalculatingUsingBgWorker)
                {
                    theFunctionsForm.theRepresentingFunctions.Add(theApproximatinFunction);
                    theFunctionsForm.parameters.Add(approximatedParameters);
                    theFunctionsForm.lineColors.Add(new Bgr(0, 255, 0));
                    theFunctionsForm.Represent();
                    //theFunctionsForm.SaveToImage(currentDirectory + randomFileName + "_r_phi_approximation.png");
                    theFunctionsForm.SaveAsPDF(currentDirectory + randomFileName + "_r_phi_approximation.pdf", true);
                }

                #endregion формируем данные для аппроксимации зависимости r(phi) и выполняем саму аппроксимацию

                BGWorkerReport("Аппроксимация зависимости Rs(phi) завершена");
                #endregion аппроксимация распределения Rm(phi)



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
                //Если изображение кропнутое, надо это учитывать
                double tgT = (sunCenterPoint.Y - imageCircleCenter.Y) / (sunCenterPoint.X - imageCircleCenter.X);
                double tethaAngle = Math.Atan(tgT);
                DenseMatrix dmXdashedCoordinate = DenseMatrix.Create(dmSunburnProfileDetection.RowCount,
                    dmSunburnProfileDetection.ColumnCount, new Func<int, int, double>(
                        (row, column) =>
                        {
                            double currX = (double)column;
                            double currY = (double)row;
                            double xDashed = currX * Math.Cos(tethaAngle) + currY * Math.Sin(tethaAngle);
                            return xDashed;
                        }));

                List<double> minimumsDataValues = new List<double>();
                List<double> minimumsXDashedCoodinate = new List<double>();
                for (int i = 0; i < dmXdashedCoordinate.RowCount; i++)
                {
                    for (int j = 0; j < dmXdashedCoordinate.ColumnCount; j++)
                    {
                        if (dmDecartMinimumsDistribution[i, j] > 0.0d)
                        {
                            minimumsDataValues.Add(dmDecartMinimumsDistribution[i, j]);
                            minimumsXDashedCoodinate.Add(dmXdashedCoordinate[i, j]);
                        }
                    }
                }
                DenseVector theLine1Coeffs =
                    DataAnalysis.NPolynomeApproximationLessSquareMethod(DenseVector.OfEnumerable(minimumsDataValues),
                        DenseVector.OfEnumerable(minimumsXDashedCoodinate), new List<PointD>(), 1);

                DenseMatrix dmValuesToSubtract_plate = DenseMatrix.Create(dmProcessingData.RowCount,
                    dmProcessingData.ColumnCount, new Func<int, int, double>(
                        (row, column) =>
                        {
                            return DataAnalysis.PolynomeValue(theLine1Coeffs, dmXdashedCoordinate[row, column]);
                        }));

                if (verbosityLevel > 1)
                {
                    dmValuesToSubtract_plate.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                  "_TheDataToSubtract_plate.nc");
                }
                BGWorkerReport("окончание оценки общего тренда уклона фоновой засветки");
                #endregion оценим общий тренд уклона фоновой засветки по распределению локальных минимумов

                



                if (verbosityLevel > 1)
                {
                    dmDistanceToSunMargin.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                               "_SunMarginDistance.nc");
                    dmSunburnProfileDetection.SaveNetCDFdataMatrix(currentDirectory + randomFileName +
                                                                   "_SunburnProfileDetection.nc");
                    dmAngleAroundTheSunburn.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_SunAngle.nc");
                }



                BGWorkerReport("финальный шаг обработки: компенсация засветки");

                DenseMatrix dmReversed3rdOrder = dmProcessingData.Copy();
                DenseMatrix dmReversed6thOrder = dmProcessingData.Copy();

                DenseMatrix dmDataToSubtract3rdOrder = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                    new Func<int, int, double>(
                        (row, col) =>
                        {
                            double currAngle = dmAngleAroundTheSunburn[row, col] - Phi1;
                            double currDistance = dmDistanceToSunMargin[row, col];
                            double theMinimumDistanceForThisAngle = theApproximatinFunction(approximatedParameters,
                                dmAngleAroundTheSunburn[row, col]);
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
                                                             DataAnalysis.PolynomeValue(approxPolyKoeffs3,
                                                                 startPointRFromSunMargin / currentLinearKoeff));
                                // УПС. Так далеко наша аппроксимация не действует. Тогда просто вычтем общий фон.
                                //возьмем смещение общим уклоном dmValuesToSubtract_plate, посмотрим смещение в точке startPointX и
                                // от этого спляшем по смещению в текущую точку
                                //для этого надо знать положение стартовой точки в декартовых координатах
                                int startPointX =
                                    Convert.ToInt32((startPointR) * Math.Cos(currAngle + Phi1) +
                                                    sunCenterPoint.X);
                                startPointX = (startPointX < 0) ? 0 : startPointX;
                                startPointX = (startPointX >= dmValuesToSubtract_plate.ColumnCount) ? (dmValuesToSubtract_plate.ColumnCount - 1) : startPointX;

                                int startPointY =
                                    Convert.ToInt32(-(startPointR) * Math.Sin(currAngle + Phi1) +
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
                                                             DataAnalysis.PolynomeValue(approxPolyKoeffs3,
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
                            double currAngle = dmAngleAroundTheSunburn[row, col] - Phi1;
                            double currDistance = dmDistanceToSunMargin[row, col];
                            double theMinimumDistanceForThisAngle = theApproximatinFunction(approximatedParameters,
                                dmAngleAroundTheSunburn[row, col]);
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
                                                             DataAnalysis.PolynomeValue(approxPolyKoeffs6,
                                                                 startPointRFromSunMargin / currentLinearKoeff));
                                // УПС. Так далеко наша аппроксимация не действует. Тогда просто вычтем общий фон.
                                //возьмем смещение общим уклоном dmValuesToSubtract_plate, посмотрим смещение в точке startPointX и
                                // от этого спляшем по смещению в текущую точку
                                //для этого надо знать положение стартовой точки в декартовых координатах
                                int startPointX =
                                    Convert.ToInt32((startPointR) * Math.Cos(currAngle + Phi1) +
                                                    sunCenterPoint.X);
                                startPointX = (startPointX < 0) ? 0 : startPointX;
                                startPointX = (startPointX >= dmValuesToSubtract_plate.ColumnCount) ? (dmValuesToSubtract_plate.ColumnCount - 1) : startPointX;

                                int startPointY =
                                    Convert.ToInt32(-(startPointR) * Math.Sin(currAngle + Phi1) +
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
                                                             DataAnalysis.PolynomeValue(approxPolyKoeffs6,
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
                        return dVal * koeff;
                    });

                    DescriptiveStatistics stats = new DescriptiveStatistics(dmResult.Values);
                    // удалим значения за пределами 3s
                    dmResult.MapInplace(dVal =>
                    {
                        if (Math.Abs(dVal - stats.Mean) > stats.StandardDeviation * 3.0d)
                        {
                            return 0.0d;
                        }
                        else return dVal;
                    });
                    dmResult.MapInplace(dVal => ((dVal < 0.0d) || (dVal > minSunburnGrIxValue)) ? (0.0d) : (dVal));

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
                    //theLogWindow = ServiceTools.LogAText(theLogWindow,
                    //    "average sky distance to sun margin: " + averageSkyDistance);
                    //theLogWindow = ServiceTools.LogAText(theLogWindow,
                    //    "average sky data value at average sky distance: " + averageSkyDataValue);

                    //theLogWindow = ServiceTools.LogAText(theLogWindow,
                    //    "resulting data min value = " + dataMinValue.ToString("e"));
                    //theLogWindow = ServiceTools.LogAText(theLogWindow,
                    //    "resulting data max value = " + dataMaxValue.ToString("e"));

                    BGWorkerReport("формирование данных и скрипта для отрисовки картинок в Matlab");
                }

                #region // данные и Matlab-скрипт для визуализации
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

                //#endregion данные и Matlab-скрипт для вывода данных в горизонтальном разрезе через центр солнца

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
                    #region //
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
                    #endregion //

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
                //ColorSchemeRuler skyCloudRuler = new ColorSchemeRuler(skyCloudColorScheme,
                //    dmReversed6thOrder.Values.Min(), dmReversed6thOrder.Values.Max());

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
                    //ImageConditionAndDataRepresentingForm restoredDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmReversed,
                    //    "finally restored 1-sigma/Y data", true, false, 0.0d, 1.0d, false);
                    //ImageConditionAndDataRepresentingForm originalDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmProcessingData,
                    //    "original 1-sigm/Y data", true, false, 0.0d, 1.0d, false);
                    //restoredDataForm.SaveData(currentDirectory + randomFileName + "_res.nc", true);
                    //originalDataForm.SaveData(currentDirectory + randomFileName + "_orig.nc", true);
                    dmReversed.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_res.nc");
                    dmProcessingData.SaveNetCDFdataMatrix(currentDirectory + randomFileName + "_orig.nc");
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
                currentSunDiskCondition = SunDiskConditions.Cloudy;
            }


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

            DenseMatrix dmFilteredData = (DenseMatrix)dmProcessingData.Clone();
            dmFilteredData.MapInplace(new Func<double, double>(d =>
            {
                if ((d >= grixFilteringMinValue) && (d <= grixFilteringMaxValue)) return d;
                return 0.0d;
            }));

            dmGradData.MapInplace(x => (double.IsNaN(x)) ? (0.0d) : (x));

            double curMedian = 0.0d;
            DescriptiveStatistics stats = DataAnalysis.StatsOfDataExcludingValues(dmGradData, 0.0d, out curMedian);
            if (stats == null) return RoundData.nullRoundData();

            double gradMean = stats.Mean;
            double gradMedian = curMedian;

            dmFilteredData.MapIndexedInplace(new Func<int, int, double, double>((row, col, x) =>
            {
                if (dmGradData[row, col] > gradMedian) return 0.0d;
                return x;
            }));



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

            Func<DenseVector, PointD, double> inputApproxFunc = new Func<DenseVector, PointD, double>(
                (dvParameters, curPoint) =>
                {
                    double centerX = dvParameters[0];
                    double centerY = dvParameters[1];
                    double r = dvParameters[2];
                    double diffX = curPoint.X - centerX;
                    double diffY = curPoint.Y - centerY;
                    double diff = r - Math.Sqrt(diffX * diffX + diffY * diffY);
                    return diff;
                });

            Func<DenseVector, PointD, double> approxFuncDevSqred = new Func<DenseVector, PointD, double>(
                (dvParameters, curPoint) =>
                {
                    double centerX = dvParameters[0];
                    double centerY = dvParameters[1];
                    double r = dvParameters[2];
                    double diffX = curPoint.X - centerX;
                    double diffY = curPoint.Y - centerY;
                    double diff = r - Math.Sqrt(diffX * diffX + diffY * diffY);
                    return diff * diff;
                });

            //if (arcPointsList.Count < 5) return PointD.nullPointD();
            if (arcPointsList.Count < 5) return RoundData.nullRoundData();
            //if (arcPointsList.Count < 5) continue;

            GradientDescentApproximator approximator1st = new GradientDescentApproximator(arcPointsList,
                inputApproxFunc);

            DenseMatrix dmSunburnMassSenterSearching = (DenseMatrix)dmSourceProcessingData.Clone();
            //dmSunburnMassSenterSearching.MapInplace(x => (x >= 0.99d) ? (1.0d) : (0.0d));
            dmSunburnMassSenterSearching.MapInplace(x => (x >= minSunburnGrIxValue) ? (1.0d) : (0.0d));
            double massCenterX = 0.0d;
            double massCenterY = 0.0d;
            foreach (Tuple<int, int, double> theElement in dmSunburnMassSenterSearching.EnumerateIndexed())
            {
                if (theElement.Item3 < 1.0d) continue;
                massCenterX += (double)theElement.Item2;
                massCenterY += (double)theElement.Item1;
            }
            massCenterX = massCenterX / dmSunburnMassSenterSearching.Values.Sum();
            massCenterY = massCenterY / dmSunburnMassSenterSearching.Values.Sum();
            double rInit = imgP.imageRD.DRadius / 2.0d;
            DenseVector dvInitialParametersValues =
                DenseVector.OfEnumerable(new double[] { massCenterX, massCenterY, rInit });
            DenseVector dvInitialParametersIncrement = DenseVector.OfEnumerable(new double[] { 1.0d, 1.0d, 1.0d });
            approximator1st.SelfWorker = selfWorker;
            DenseVector dvApproximatedParameters =
                approximator1st.ApproximationGradientDescent2DPt(dvInitialParametersValues,
                    dvInitialParametersIncrement, 0.0001d);

            DenseVector dvDeviationsSqrd = DenseVector.Create(arcPointsList.Count,
                i => approxFuncDevSqred(dvApproximatedParameters, arcPointsList[i]));
            DescriptiveStatistics stats1 = new DescriptiveStatistics(dvDeviationsSqrd.Values);
            double meanDevSqred = stats1.Mean;

            List<PointD> arcPointsListFiltered1st =
                arcPointsList.FindAll(
                    new Predicate<PointD>(x => approxFuncDevSqred(dvApproximatedParameters, x) <= meanDevSqred));

            GradientDescentApproximator approximator2nd = new GradientDescentApproximator(arcPointsListFiltered1st,
                inputApproxFunc);
            DenseVector dvApproximatedParameters2nd =
                approximator2nd.ApproximationGradientDescent2DPt(dvApproximatedParameters,
                    dvInitialParametersIncrement, 0.00001d);

            RoundData circle2ndPass = new RoundData(dvApproximatedParameters2nd[0], dvApproximatedParameters2nd[1],
                dvApproximatedParameters2nd[2]);


            PointD tmpZeroAnglePoint = PointD.Sum(arcPointsListFiltered1st) / (double)arcPointsListFiltered1st.Count;
            double zeroAngleValue = 0.0d;
            List<PointPolar> arcPointsListFilteredPolar = DataAnalysis.ListDecartToPolar(arcPointsListFiltered1st,
                circle2ndPass.pointDCircleCenter(), tmpZeroAnglePoint, out zeroAngleValue);

            DenseVector dvAnglesData = DenseVector.Create(arcPointsListFilteredPolar.Count,
                i => arcPointsListFilteredPolar[i].Phi);
            DescriptiveStatistics anglesStats = new DescriptiveStatistics(dvAnglesData);


            DenseVector dvAnglesDataFiltered = DataAnalysis.DataVectorizedWithCondition(dvAnglesData,
                d => (Math.Abs(d - anglesStats.Mean) <= anglesStats.StandardDeviation * 3.0d));
            double minAngleValue = dvAnglesDataFiltered.Values.Min();
            double maxAngleValue = dvAnglesDataFiltered.Values.Max();
            double meanAngle = (minAngleValue + maxAngleValue) / 2.0d;

            PointD meanArcPoint = new PointD();
            meanArcPoint.X = circle2ndPass.DRadius * Math.Cos(meanAngle + zeroAngleValue) + circle2ndPass.DCenterX;
            meanArcPoint.Y = -circle2ndPass.DRadius * Math.Sin(meanAngle + zeroAngleValue) + circle2ndPass.DCenterY;
            PointD meanArcPoint2 = new PointD();
            meanArcPoint2.X = circle2ndPass.DRadius * Math.Cos(meanAngle + zeroAngleValue + Math.PI) +
                              circle2ndPass.DCenterX;
            meanArcPoint2.Y = -circle2ndPass.DRadius * Math.Sin(meanAngle + zeroAngleValue + Math.PI) +
                              circle2ndPass.DCenterY;

            PointD zeroPointonCircle =
                new PointD(circle2ndPass.DRadius * Math.Cos(zeroAngleValue) + circle2ndPass.DCenterX,
                    -circle2ndPass.DRadius * Math.Sin(zeroAngleValue) + circle2ndPass.DCenterY);


            #region отображение результата исследований изображения

            Image<Bgr, byte> imgPoints = LocalProcessingImage.CopyBlank(); // new Image<Bgr, byte>(LocalProcessingBitmap).CopyBlank();
            imgPoints.Draw(new CircleF(circle2ndPass.pointfCircleCenter(), (float)circle2ndPass.DRadius),
                new Bgr(Color.White), 2);
            foreach (PointD pointFiltered in arcPointsListFiltered1st) imgPoints.Draw(new CircleF(pointFiltered.PointF(), 2.0f), new Bgr(Color.Yellow), 1);
            imgPoints.Draw(new LineSegment2DF(meanArcPoint.PointF(), meanArcPoint2.PointF()), new Bgr(Color.Magenta),
                2);
            imgPoints.Draw(new LineSegment2DF(zeroPointonCircle.PointF(), circle2ndPass.pointfCircleCenter()),
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



            DenseMatrix dmProcessingDataXrotated = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                (r, c) => (double)c);
            dmProcessingDataXrotated = (DenseMatrix)dmProcessingDataXrotated.PointwiseMultiply(dmMaskCircled);
            DenseMatrix dmProcessingDataXsrc = (DenseMatrix)dmProcessingDataXrotated.Clone();
            DenseMatrix dmProcessingDataYrotated = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
                (r, c) => (double)r);
            dmProcessingDataYrotated = (DenseMatrix)dmProcessingDataYrotated.PointwiseMultiply(dmMaskCircled);
            DenseMatrix dmProcessingDataYsrc = (DenseMatrix)dmProcessingDataYrotated.Clone();
            PointD rotationCenterPtD = new PointD(meanArcPoint);
            double rotationAngle = DataAnalysis.PtdDecartToPolar(circle2ndPass.pointDCircleCenter(), rotationCenterPtD,
                0.0d).Phi;
            dmProcessingDataXrotated.MapIndexedInplace((row, col, xVal) =>
            {
                double yVal = dmProcessingDataYsrc[row, col];
                PointD curPointD = new PointD(xVal, yVal);
                PointPolar ptPolarRotated = DataAnalysis.PtdDecartToPolar(curPointD, rotationCenterPtD, rotationAngle);
                PointD retPtD = DataAnalysis.PtdPolarToDecart(ptPolarRotated, new PointD(0.0d, 0.0d), 0.0d);
                return retPtD.X;
            });


            dmProcessingDataYrotated.MapIndexedInplace((row, col, yVal) =>
            {
                double xVal = dmProcessingDataXsrc[row, col];
                PointD curPointD = new PointD(xVal, yVal);
                PointPolar ptPolarRotated = DataAnalysis.PtdDecartToPolar(curPointD, rotationCenterPtD, rotationAngle);
                PointD retPtD = DataAnalysis.PtdPolarToDecart(ptPolarRotated, new PointD(0.0d, 0.0d), 0.0d);
                return retPtD.Y;
            });



            double minRotatedXdouble = dmProcessingDataXrotated.Values.Min();
            int minRotatedX = Convert.ToInt32((Math.Round(minRotatedXdouble, 0) == minRotatedXdouble) ? (minRotatedXdouble) : (Math.Round(minRotatedXdouble, MidpointRounding.ToEven)));
            minRotatedX = (minRotatedX > minRotatedXdouble) ? (minRotatedX - 1) : (minRotatedX);
            double maxRotatedXdouble = dmProcessingDataXrotated.Values.Max();
            int maxRotatedX = Convert.ToInt32((Math.Round(maxRotatedXdouble, 0) == maxRotatedXdouble) ? (maxRotatedXdouble) : (Math.Round(maxRotatedXdouble, MidpointRounding.ToEven)));
            maxRotatedX = (maxRotatedX < maxRotatedXdouble) ? (maxRotatedX + 1) : (maxRotatedX);

            DenseMatrix dmSourceProcessingDataFilteredAlongBisectrix = (DenseMatrix)dmSourceProcessingData.Clone();

            double dHalfWidthY = imgP.imageRD.DRadius / 70.0d;
            dmSourceProcessingDataFilteredAlongBisectrix.MapIndexedInplace((row, col, dVal) =>
            {
                //if ((dmProcessingDataYrotated[row, col] <= -10.0d) ||
                //        (dmProcessingDataYrotated[row, col] >= 10.0d)) return 0.0d;
                if ((dmProcessingDataYrotated[row, col] <= -dHalfWidthY) ||
                        (dmProcessingDataYrotated[row, col] >= dHalfWidthY)) return 0.0d;
                return dVal;
            });
            dmSourceProcessingDataFilteredAlongBisectrix =
                (DenseMatrix)dmSourceProcessingDataFilteredAlongBisectrix.PointwiseMultiply(dmMaskCircled);

            DenseMatrix dmSunburnDataAlongBisectrix = (DenseMatrix)dmSourceProcessingDataFilteredAlongBisectrix.Clone();
            //dmSunburnDataAlongBisectrix.MapInplace(d => (d == 1.0d) ? (d) : (0.0d));
            dmSunburnDataAlongBisectrix.MapInplace(d => (d >= minSunburnGrIxValue) ? (d) : (0.0d));
            dmProcessingDataXsrc = (DenseMatrix)dmProcessingDataXsrc.PointwiseMultiply(dmSunburnDataAlongBisectrix);
            dmProcessingDataYsrc = (DenseMatrix)dmProcessingDataYsrc.PointwiseMultiply(dmSunburnDataAlongBisectrix);

            if (dmSunburnDataAlongBisectrix.Values.Sum() == 0.0d) return RoundData.nullRoundData();
            if (dmProcessingDataXsrc.Values.Sum() == 0.0d) return RoundData.nullRoundData();

            double testSunCenterX = dmProcessingDataXsrc.Values.Sum() / dmSunburnDataAlongBisectrix.Values.Sum();
            double testSunCenterY = dmProcessingDataYsrc.Values.Sum() / dmSunburnDataAlongBisectrix.Values.Sum();
            PointD testSunCenter = new PointD(testSunCenterX, testSunCenterY);
            //double testSunRadius = (dmSunburnDataAlongBisectrix.Values.Sum() / 20.0d) / 2.0d;
            double testSunRadius = (dmSunburnDataAlongBisectrix.Values.Sum() / (2.0d * dHalfWidthY)) / 2.0d;

            RoundData rd = new RoundData(testSunCenterX, testSunCenterY, testSunRadius);

            ServiceTools.FlushMemory();

            return rd;
        }






        public RoundData DetectSunWithSerieOfArcs(ImageProcessing imgP, DenseMatrix dmGrIx, string currentDirectory = "",
            string randomFileName = "")
        {
            DenseMatrix dmSunburnData = imgP.eval("Y", null);

            //DenseVector dvGrIxDataEqualsOne = DataAnalysis.DataVectorizedWithCondition(dmSunburnData, dval => dval >= 254.0d);
            DenseVector dvGrIxDataEqualsOne = DataAnalysis.DataVectorizedWithCondition(dmSunburnData, dval => dval >= minSunburnYValue);
            if (dvGrIxDataEqualsOne == null) return RoundData.nullRoundData();
            if (dvGrIxDataEqualsOne.Values.Sum() < imgP.significantMaskImageBinary.CountNonzero()[0]*minSunAreaPart) return RoundData.nullRoundData();

            Image<Gray, Byte> maskImageCircled85 = imgP.imageSignificantMaskCircled(dSunDetectorArcedCropFactor*100.0d);
            DenseMatrix dmGrIxData =
                (DenseMatrix)dmGrIx.PointwiseMultiply(ImageProcessing.DenseMatrixFromImage(maskImageCircled85));
            DenseVector dvGrIxDataToStat = DataAnalysis.DataVectorizedExcludingValues(dmGrIxData, 0.0d);
            //dvGrIxDataToStat = DataAnalysis.DataVectorizedWithCondition(dvGrIxDataToStat, dval => (dval != 1.0d));
            dvGrIxDataToStat = DataAnalysis.DataVectorizedWithCondition(dvGrIxDataToStat, dval => (dval <= minSunburnGrIxValue));
            

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
                delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
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
                    //startedBGworkers[returningBGWthreadID] = false;
                    startedBGworkers--;
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "finished sun detection thread " + (returningBGWthreadID+1) + " of " + quantilesBy10.Count);
                };



            DoWorkEventHandler currDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
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
                //while (startedBGworkers.Sum(boolVal => (boolVal) ? ((int)1) : ((int)0)) >= 2)
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
                theLogWindow = ServiceTools.LogAText(theLogWindow, "started sun detection thread " + (i+1) + " of " + quantilesBy10.Count);
                //startedBGworkers[i] = true;
                startedBGworkers++;
            }



            while (finishedBGworkers.Sum(boolVal => (boolVal) ? ((int)1) : ((int)0)) < finishedBGworkers.Count)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(100);
            }


            if (!isCalculatingUsingBgWorker)
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
                Image<Bgr, byte> sunDetectionImage = new Image<Bgr,byte>(imgP.processingBitmap());
                Image<Bgr, byte> sunLocationImage = sunDetectionImage.CopyBlank();
                sunLocationImage.Draw(new CircleF(sunRD.pointfCircleCenter(), (float)sunRD.DRadius), new Bgr(255, 255, 255), 0);
                sunDetectionImage = sunDetectionImage.AddWeighted(sunLocationImage, 0.6d, 0.4d, 0.0d);
                sunDetectionImage.Save(currentDirectory + randomFileName + "_SunDetectionByAnsambleResult.jpg");
                sunDetectionImage.Dispose();
                sunLocationImage.Dispose();
            }

            return sunRD;
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

            ImageProcessing imp = new ImageProcessing(LocalProcessingImage);
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
    }
}