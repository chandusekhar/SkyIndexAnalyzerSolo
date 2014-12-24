using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;


namespace SkyImagesAnalyzerLibraries
{
    /// <summary>
    /// Class RoundData.
    /// Хранит и рассчитывает различные параметры окружности на изобржении
    /// </summary>
    [Serializable]
    public class RoundData
    {
        public double RoundArea = 0.0;
        private bool nullCircle;

        private int intCenterX;

        [XmlElement("intCenterX")]
        public int IntCenterX
        {
            get { return intCenterX; }
            set
            {
                intCenterX = value;
                dCenterX = (double)intCenterX;
            }
        }

        private int intCenterY;

        [XmlElement("intCenterY")]
        public int IntCenterY
        {
            get { return intCenterY; }
            set
            {
                intCenterY = value;
                dCenterY = (double)intCenterY;
            }
        }

        private double dCenterX;

        [XmlElement("doubleCenterX")]
        public double DCenterX
        {
            get { return dCenterX; }
            set
            {
                dCenterX = value;
                intCenterX = Convert.ToInt32(Math.Round(dCenterX, 0));
            }
        }

        private double dCenterY;

        [XmlElement("doubleCenterY")]
        public double DCenterY
        {
            get { return dCenterY; }
            set
            {
                dCenterY = value;
                intCenterY = Convert.ToInt32(Math.Round(dCenterY, 0));
            }
        }

        private double dRadius;

        [XmlElement("doubleRadius")]
        public double DRadius
        {
            get { return dRadius; }
            set
            {
                dRadius = value;
                RoundArea = Math.PI * dRadius * dRadius;
            }
        }

        public PointF pointfCircleCenter()
        {
            return (new PointF((float)dCenterX, (float)dCenterY));
        }


        public PointD pointDCircleCenter()
        {
            return (new PointD(dCenterX, dCenterY));
        }


        public RoundData()
        {
            DCenterX = 0.0d;
            DCenterY = 0.0d;
            DRadius = 0.0d;
        }


        public RoundData(double centerX, double centerY, double radius)
        {
            if (double.IsNaN(centerX) || double.IsNaN(centerY) || double.IsNaN(radius))
            {
                nullCircle = true;
            }
            else
            {
                DCenterX = centerX;
                DCenterY = centerY;
                DRadius = radius;
            }
        }



        public RoundData Copy()
        {
            if (nullCircle)
            {
                return RoundData.nullRoundData();
            }
            else
            {
                return new RoundData(DCenterX, DCenterY, DRadius);
            }
        }



        public override string ToString()
        {
            if (!IsNull)
            {
                string str = "center point: ( " + dCenterX.ToString() + " , " + dCenterY.ToString() + " );" +
                             Environment.NewLine;
                str += "radius: " + dRadius.ToString();
                return str;
            }
            else return "N/A";
        }


        public bool IsNull
        {
            get { return nullCircle; }
            set
            {
                this.nullCircle = value;
                if (nullCircle)
                {
                    this.DCenterX = 0.0d;
                    this.DCenterY = 0.0d;
                    this.DRadius = 0.0d;
                }
            }
        }

        public static RoundData nullRoundData()
        {
            RoundData rd = new RoundData();
            rd.IsNull = true;
            return rd;
        }



        public static bool isNull(RoundData rd)
        {
            return rd.IsNull;
        }


        public CircleF CircleF()
        {
            CircleF c = new CircleF(pointfCircleCenter(), (float) DRadius);
            return c;
        }
    }


    [Serializable]
    public struct RoundDataWithUnderlyingImgSize
    {
        public RoundData circle;
        public Size imgSize;
    }



    [Serializable]
    public struct EoundDataWithUnderlyingImgSize
        //ошибся одной буквой раньше :(
    {
        public RoundData circle;
        public Size imgSize;
    }




    /// <summary>
    /// Class ImageProcessing.
    /// Предназначен для осуществления различных преобразований над изображениями, используемыми в проекте
    /// </summary>
    public class ImageProcessing // : IDisposable
    {
        /// <summary>
        /// The initial bitmap to process
        /// </summary>
        //private Bitmap bm2Process;

        private Image<Bgr, byte> img2process;

        /// <summary>
        /// The significant mask image
        /// маска, которая позволяет отсечь неиспользуемые области при отображении результирующих изображений
        /// Значения либо 0, либо 255
        /// </summary>
        public Image<Gray, Byte> significantMaskImage;

        /// <summary>
        /// The significant mask image binary
        /// Бинарная маска, которая позволяет отсечь неиспользуемые области при отображении результирующих изображений
        /// Значения либо 0, либо 1
        /// </summary>
        public Image<Gray, Byte> significantMaskImageBinary;

        /// <summary>
        /// The significant mask image circled
        /// Бинарная маска, которая позволяет отсечь неиспользуемые области при отображении результирующих изображений
        /// Отличие от significantMaskImageBinary в том, что формируется приближением маски к форме круга
        /// Значения либо 0, либо 1
        /// </summary>
        public Image<Gray, Byte> significantMaskImageCircled;

        /// <summary>
        /// The significant mask contour image
        /// Исходное изображение, на котором нанесена граница маски зеленым цветом
        /// </summary>
        public Image<Bgr, Byte> significantMaskContourImage;

        /// <summary>
        /// The significant mask contour image circled
        /// Исходное изображение, на котором нанесена граница приближенной кругом маски
        /// </summary>
        public Image<Bgr, Byte> significantMaskContourImageCircled;

        /// <summary>
        /// The significant mask image oct lined
        /// Исходное изображение с наложенной разметкой по октам
        /// </summary>
        public Image<Bgr, Byte> significantMaskImageOctLined;

        public double overallImageArea = 0.0;
        public double maskSignificantArea = 0.0;

        /// <summary>
        /// The reporting TextBox element
        /// </summary>
        public TextBox reportingTextBox;

        /// <summary>
        /// The image control points
        /// Контрольные точки, используемые для приближения маски изображения к форме круга
        /// </summary>
        private Point[] imageControlPoints;
        public RoundData imageRD;
        public Image<Bgr, Byte> tmpImage;
        public ColorScheme evaluatingColorScheme;


        public ImageProcessing(Bitmap inBM)
        {
            //bm2Process = (Bitmap)inBM.Clone(); ;
            img2process = new Image<Bgr, byte>(inBM);
            overallImageArea = (double)inBM.Width * (double)inBM.Height;
            tmpImage = img2process.Copy();
        }


        public ImageProcessing(Bitmap inBM, bool whetherHaveToCalculateAllSignificantMasksImmediately = false)
        {
            //bm2Process = (Bitmap)inBM.Clone();
            img2process = new Image<Bgr, byte>(inBM);
            overallImageArea = (double)img2process.Width * (double)img2process.Height;
            if (whetherHaveToCalculateAllSignificantMasksImmediately)
            {
                getImageSignificantMask();
                getImageContour();
                getImageContourCircled();
                getImageOctLined();
            }
            tmpImage = img2process.Copy();
        }





        public ImageProcessing(Image<Bgr, byte> inImg, bool whetherHaveToCalculateAllSignificantMasksImmediately = false)
        {
            //bm2Process = ServiceTools.CopyBitmap(inImg.Bitmap);
            img2process = inImg.Copy();
            overallImageArea = (double)img2process.Width * (double)img2process.Height;
            if (whetherHaveToCalculateAllSignificantMasksImmediately)
            {
                getImageSignificantMask();
                getImageContour();
                getImageContourCircled();
                getImageOctLined();
            }
            tmpImage = img2process.Copy();
        }




        public Bitmap processingBitmap()
        {
            return ServiceTools.CopyBitmap(img2process.Bitmap);
        }


        public void getImageSignificantMask()
        {
            Image<Gray, Byte> emguImage = img2process.Copy().Convert<Gray, Byte>();
            Image<Gray, Byte> BinaryEmguImage = emguImage.ThresholdBinary(new Gray(30), new Gray(255));
            Contour<Point> imageContours = BinaryEmguImage.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_EXTERNAL);
            double currArea = 0.0;
            Contour<Point> neededContour = imageContours;
            while (true)
            {
                if (imageContours.Area > currArea)
                {
                    currArea = imageContours.Area;
                    neededContour = imageContours;
                }
                imageContours = imageContours.HNext;
                if (imageContours == null)
                    break;
            }
            maskSignificantArea = currArea;

            Image<Gray, Byte> maskImage = BinaryEmguImage.CopyBlank();
            maskImage.Draw(neededContour, new Gray(255), -1);
            //emguImage.Dispose();
            //BinaryEmguImage.Dispose();
            //imageContours = null;
            //neededContour = null;

            significantMaskImage = maskImage;
            significantMaskImageBinary = significantMaskImage / 255;

            //BGRsignificantMaskImage = new Image<Bgr, byte>(new Image<Gray, byte>[] { significantMaskImage, significantMaskImage, significantMaskImage });
        }



        public Image<Gray, Byte> imageSignificantMaskCircled(double radiusPercentage = 100.0d)
        {
            Image<Gray, Byte> emguImage = significantMaskImageBinary.CopyBlank();
            CircleF theCircle = new CircleF(imageRD.pointfCircleCenter(), (float)(imageRD.DRadius * radiusPercentage / 100.0d));

            emguImage.Draw(theCircle, new Gray(255), -1);
            emguImage = emguImage / 255;

            return emguImage;
        }




        public DenseMatrix dmSignificantMaskCircled(double radiusPercentage = 100.0d)
        {
            Image<Gray, Byte> imgMask = imageSignificantMaskCircled(radiusPercentage);
            DenseMatrix dmRes = DenseMatrixFromImage(imgMask);
            return dmRes;
        }




        public void getImageContour()
        {
            Image<Gray, Byte> emguImage = img2process.Copy().Convert<Gray, Byte>();
            Image<Bgr, Byte> contourImage = emguImage.CopyBlank().Convert<Bgr, Byte>();
            Image<Gray, Byte> BinaryEmguImage = emguImage.ThresholdBinary(new Gray(50), new Gray(256));
            Contour<Point> imageContours = BinaryEmguImage.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_EXTERNAL);

            int lineWidth = (int)Math.Round(emguImage.Width / 150.0);
            if (lineWidth < 2) lineWidth = 2;

            contourImage = contourImage.AddWeighted(emguImage.Convert<Bgr, Byte>(), 0.0, 1.0, 0.0);

            double currArea = 0.0;
            Contour<Point> neededContour = imageContours;
            while (true)
            {
                if (imageContours.Area > currArea)
                {
                    currArea = imageContours.Area;
                    neededContour = imageContours;
                }
                imageContours = imageContours.HNext;
                if (imageContours == null)
                    break;
            }

            contourImage.Draw(neededContour, new Bgr(Color.Green), 5);

            emguImage.Dispose();
            BinaryEmguImage.Dispose();
            ServiceTools.FlushMemory();
            
            significantMaskContourImage = contourImage.Copy();
        }


        public void getImageContourCircled()
        {
            Image<Bgr, Byte> circleImage = img2process.Copy();

            int lineWidth = (int)Math.Round(circleImage.Width / 150.0);
            if (lineWidth < 2) lineWidth = 2;

            if (imageRD == null)
            {
                GetImageRoundParameters();
            }
            RoundData rd = imageRD;
            CircleF Circle2Draw = new CircleF(rd.pointfCircleCenter(), (float)rd.DRadius);
            circleImage.Draw(Circle2Draw, new Bgr(Color.Green), lineWidth);
            
            significantMaskContourImageCircled = circleImage.Copy();

            significantMaskImageCircled = img2process.CopyBlank().Convert<Gray, byte>();
            significantMaskImageCircled.Draw(Circle2Draw, new Gray(1), 0);
        }


        public void getImageOctLined()
        {
            Image<Bgr, Byte> octLinedImage = img2process.Copy();
            Bgr lineColor = new Bgr(Color.White);

            int lineWidth = (int)Math.Round(octLinedImage.Width / 400.0);
            if (lineWidth < 1) lineWidth = 1;

            if (imageRD == null)
            {
                GetImageRoundParameters();
            }
            RoundData rd = imageRD;
            CircleF Circle2Draw = new CircleF(rd.pointfCircleCenter(), (float)rd.DRadius);
            octLinedImage.Draw(Circle2Draw, lineColor, lineWidth);
            Circle2Draw = new CircleF(rd.pointfCircleCenter(), ((float)rd.DRadius) * Constants.CosPIhalfF);
            octLinedImage.Draw(Circle2Draw, lineColor, lineWidth);
            LineSegment2DF ls2d = new LineSegment2DF(new PointF((float)(rd.DCenterX - rd.DRadius), (float)(rd.DCenterY)), new PointF((float)(rd.DCenterX + rd.DRadius), (float)(rd.DCenterY)));
            octLinedImage.Draw(ls2d, lineColor, lineWidth);
            ls2d = new LineSegment2DF(new PointF((float)(rd.DCenterX), (float)(rd.DCenterY - rd.DRadius)), new PointF((float)(rd.DCenterX), (float)(rd.DCenterY + rd.DRadius)));
            octLinedImage.Draw(ls2d, lineColor, lineWidth);
            
            significantMaskImageOctLined = octLinedImage.Copy();
        }


        public void GetImageRoundParameters()
        {
            if (significantMaskImage == null)
            {
                getImageSignificantMask();
            }

            Image<Gray, Byte> tmpMaskImage = significantMaskImage.Copy();
            Contour<Point> contoursDetected = tmpMaskImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
            Contour<Point> contourFound = contoursDetected;
            double areaDetected = contourFound.Area;
            
            while (true)
            {
                Contour<Point> currContour = contoursDetected;
                double currContourArea = currContour.Area;
                if (currContourArea > areaDetected)
                {
                    contourFound = currContour;
                    areaDetected = currContourArea;
                }

                contoursDetected = contoursDetected.HNext;
                if (contoursDetected == null)
                    break;
            }
            //нашли контур с максимальной площадью
            List<PointD> pointsList = new List<PointD>();
            foreach (Point contourPoint in contourFound)
            {
                if ((contourPoint.X < 10) || (contourPoint.X > tmpMaskImage.Width - 10) ||
                    (contourPoint.Y > tmpMaskImage.Height - 10) || (contourPoint.Y < 10)) continue;
                pointsList.Add(new PointD(contourPoint));
            }

            Func<DenseVector, PointD, double> minimizingFunc = (dvParameters, pt) =>
            {
                double centerX = dvParameters[0];
                double centerY = dvParameters[1];
                double r = dvParameters[2];
                double diffX = pt.X - centerX;
                double diffY = pt.Y - centerY;
                double diff = r - Math.Sqrt(diffX * diffX + diffY * diffY);
                return diff;
            };

            List<double> parametersList = new List<double>();
            parametersList.Add(((double)tmpMaskImage.Width)/2.0d);
            parametersList.Add(((double)tmpMaskImage.Height)/2.0d);
            parametersList.Add(((double)tmpMaskImage.Width) / 2.0d);
            DenseVector dvInitialParameters = DenseVector.OfEnumerable(parametersList);
            DenseVector initialParametersIncremnt = DenseVector.Create(dvInitialParameters.Count, (i => 1.0d));

            GradientDescentApproximator approximator = new GradientDescentApproximator(pointsList, minimizingFunc);
            DenseVector approximatedParameters = approximator.ApproximationGradientDescent2DPt(dvInitialParameters, initialParametersIncremnt, 0.000001d);

            RoundData imageMarginsRoundParametres2 = new RoundData(approximatedParameters[0], approximatedParameters[1], approximatedParameters[2]);
            
            imageRD = imageMarginsRoundParametres2;
        }



        private bool IsPointEmpty(Point pt2Test)
        {
            PointD controlNullPoint = new PointD(0.0d, 0.0d);
            PointD testintPointDouble = new PointD(pt2Test);
            double dist = PointD.Distance(controlNullPoint, testintPointDouble);

            if (dist == 0.0d)
            {
                return true;
            }
            return false;
        }



        //private void getMaskImageControlPoints()
        //{
        //    Point mask1stIntersectionPoint = new Point(0, 0);
        //    Point mask2ndIntersectionPoint = new Point(0, 0);
        //    Point mask3rdIntersectionPoint = new Point(0, 0);
            
        //    Image<Gray, Byte> tmpMaskImage = significantMaskImage.Copy();
        //    //Gray prevColor = new Gray(0);
        //    Byte prevColor = 0, curColor;

        //    for (int i = 1; i < tmpMaskImage.Width; i++)
        //    {
        //        Point prevPoint = new Point(i - 1, Convert.ToInt32(Math.Round(((double)(i - 1) * tmpMaskImage.Height / tmpMaskImage.Width), 0)));
        //        Point curPoint = new Point(i, Convert.ToInt32(Math.Round(((double)(i) * tmpMaskImage.Height / tmpMaskImage.Width), 0)));
        //        prevColor = tmpMaskImage.Data[prevPoint.Y, prevPoint.X, 0];
        //        curColor = tmpMaskImage.Data[curPoint.Y, curPoint.X, 0];
        //        if (prevColor < curColor)
        //        {
        //            mask1stIntersectionPoint = curPoint;
        //            if (IsPointEmpty(mask2ndIntersectionPoint))
        //            {
        //                continue;
        //            }
        //            else break;
        //        }

        //        if (prevColor > curColor)
        //        {
        //            mask2ndIntersectionPoint = prevPoint;
        //            if (IsPointEmpty(mask1stIntersectionPoint))
        //            {
        //                continue;
        //            }
        //            else break;
        //        }
        //    }

        //    for (int i = 1; i < tmpMaskImage.Width; i++)
        //    {
        //        Point prevPoint = new Point(i - 1, Convert.ToInt32(tmpMaskImage.Height - 1 - Math.Round(((double)(i - 1) * tmpMaskImage.Height / tmpMaskImage.Width), 0)));
        //        Point curPoint = new Point(i, Convert.ToInt32(tmpMaskImage.Height - 1 - Math.Round(((double)(i) * tmpMaskImage.Height / tmpMaskImage.Width), 0)));
        //        prevColor = tmpMaskImage.Data[prevPoint.Y, prevPoint.X, 0];
        //        curColor = tmpMaskImage.Data[curPoint.Y, curPoint.X, 0];
        //        if (prevColor < curColor)
        //        {
        //            Point tmpPoint = curPoint;
        //            continue;
        //        }

        //        if (prevColor > curColor)
        //        {
        //            mask3rdIntersectionPoint = prevPoint;
        //            break;
        //        }
        //    }

        //    imageControlPoints = new Point[] { mask1stIntersectionPoint, mask3rdIntersectionPoint, mask2ndIntersectionPoint };
        //    //return imageControlPoints;
        //}


        //private void Report(string text)
        //{
        //    if (reportingTextBox == null)
        //    {
        //        return;
        //    }

        //    ThreadSafeOperations.SetTextTB(reportingTextBox, Environment.NewLine + text + Environment.NewLine, true);

        //}


        public Bitmap getMaskedImageChannelBitmap(int channelNum = 0)
        {
            Image<Gray, Byte> emguImage = img2process[channelNum];
            emguImage = emguImage.Mul(significantMaskImageBinary);
            return new Bitmap(emguImage.Bitmap);
        }


        public Image<Gray, Byte> getMaskedImageChannelImage(int channelNum = 0)
        {
            Image<Gray, Byte> emguImage = img2process[channelNum];
            emguImage = emguImage.Mul(significantMaskImageBinary);
            return emguImage;
        }


        public Image<Gray, Byte> getMaskedImageChannelBitmapThresholdedBottom(int channelNum = 0, Byte thresholdValue = 255)
        {
            Image<Gray, Byte> emguImage = img2process[channelNum];
            emguImage = emguImage.Mul(significantMaskImageBinary);
            emguImage = emguImage.ThresholdToZeroInv(new Gray(thresholdValue));
            return emguImage;
        }


        public Image<Gray, Byte> getMaskedImageChannelBitmapThresholdedTop(int channelNum = 0, Byte thresholdValue = 0)
        {
            Image<Gray, Byte> emguImage = img2process[channelNum];
            emguImage = emguImage.Mul(significantMaskImageBinary);
            emguImage = emguImage.ThresholdToZero(new Gray(thresholdValue));
            return emguImage;
        }


        public Image<Gray, Byte> getMaskedImageChannelBitmapThresholded(int channelNum = 0, Byte thresholdValue = 255, bool leaveTopValues = false, bool leaveBottomValues = true)
        {
            if (leaveTopValues) return getMaskedImageChannelBitmapThresholdedTop(channelNum, thresholdValue);
            else return getMaskedImageChannelBitmapThresholdedBottom(channelNum, thresholdValue);
        }




        public static Image<Gray, Byte> getMaskedImageChannelBitmapThresholdedBottom(Image<Gray, Byte> imageToProcess, Image<Gray, Byte> maskImageBinary, Byte thresholdValue = 255)
        {
            Image<Gray, Byte> emguImage = imageToProcess;
            emguImage = emguImage.Mul(maskImageBinary);
            emguImage = emguImage.ThresholdToZeroInv(new Gray(thresholdValue));
            return emguImage;
        }


        public static Image<Gray, Byte> getMaskedImageChannelBitmapThresholdedTop(Image<Gray, Byte> imageToProcess, Image<Gray, Byte> maskImageBinary, Byte thresholdValue = 0)
        {
            Image<Gray, Byte> emguImage = imageToProcess;
            emguImage = emguImage.Mul(maskImageBinary);
            emguImage = emguImage.ThresholdToZero(new Gray(thresholdValue));
            return emguImage;
        }

        public static Image<Gray, Byte> getMaskedImageChannelBitmapThresholded(Image<Gray, Byte> imageToProcess, Image<Gray, Byte> maskImageBinary, Byte thresholdValue = 255, bool leaveTopValues = false, bool leaveBottomValues = true)
        {
            if (leaveTopValues) return getMaskedImageChannelBitmapThresholdedTop(imageToProcess, maskImageBinary, thresholdValue);
            else return getMaskedImageChannelBitmapThresholdedBottom(imageToProcess, maskImageBinary, thresholdValue);
        }




        //public static MathNet.Numerics.LinearAlgebra.Double.DenseMatrix maskedImageChannelDenseMatrixThresholdedBottomInplace
        public static void maskedImageChannelDenseMatrixThresholdedBottomInplace
            (
            DenseMatrix dmImageToProcess,
            Image<Gray, Byte> maskImageBinary,
            double thresholdValue = 255.0d,
            double cutItemsValue = 0.0d
            )
        {
            dmImageToProcess.MapInplace(
                new Func<double, double>((x) => { return (x > thresholdValue) ? cutItemsValue : x; }),
                MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            dmImageToProcess.PointwiseMultiply(DenseMatrixFromImage(maskImageBinary));
        }


        
        public static void maskedImageChannelDenseMatrixThresholdedTopInplace
            (
            DenseMatrix dmImageToProcess,
            Image<Gray, Byte> maskImageBinary,
            double thresholdValue = 0.0d,
            double cutItemsValue = 0.0d
            )
        {
            dmImageToProcess.MapInplace(
                new Func<double, double>((x) => { return (x < thresholdValue) ? cutItemsValue : x; }),
                MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            dmImageToProcess.PointwiseMultiply(DenseMatrixFromImage(maskImageBinary));
        }

        //public static MathNet.Numerics.LinearAlgebra.Double.DenseMatrix maskedImageChannelDenseMatrixThresholdedInplace
        public static void maskedImageChannelDenseMatrixThresholdedInplace
            (
            DenseMatrix dmImageToProcess,
            Image<Gray, Byte> maskImageBinary,
            double thresholdValue = 255.0d,
            double cutItemsValue = 0.0d,
            bool leaveTopValues = false,
            bool leaveBottomValues = true
            )
        {
            if (leaveTopValues) maskedImageChannelDenseMatrixThresholdedTopInplace(dmImageToProcess, maskImageBinary, thresholdValue, cutItemsValue);
            else maskedImageChannelDenseMatrixThresholdedBottomInplace(dmImageToProcess, maskImageBinary, thresholdValue, cutItemsValue);
        }




        public Bitmap tmpImageBitmap()
        {
            return new Bitmap(tmpImage.Bitmap);
        }




        public DenseMatrix eval(String exprString, TextBox tbLog)
        {
            ArithmeticsOnImages aoi = new ArithmeticsOnImages();
            //aoi.MaskImage = significantMaskImage;
            //new Image<Bgr, Byte>(bm2Process)[channelNum]
            aoi.imgR = img2process[0];
            aoi.imgG = img2process[1];
            aoi.imgB = img2process[2];
            aoi.ExprString = exprString;
            aoi.tbLog = tbLog;
            aoi.RPNeval();
            ServiceTools.FlushMemory(tbLog, "");
            if (evaluatingColorScheme != null)
                tmpImage = evalResultColored(aoi.dmRes, significantMaskImageBinary, evaluatingColorScheme);
            ServiceTools.FlushMemory(null, "");
            return aoi.dmRes;
            //tmpImage = tmpImage.Mul(BGRsignificantMaskImage);
            //MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmRes = 
        }



        /// <summary>
        /// Evaluates the specified expr string using images as variables
        /// </summary>
        /// <param name="exprString">The expr string.</param>
        /// <param name="bmR">The red channel variable</param>
        /// <param name="bmG">The green channel variable</param>
        /// <param name="bmB">The blue channel variable</param>
        /// <param name="tbLog">The log output textbox.</param>
        /// <returns>MathNet.Numerics.LinearAlgebra.Double.DenseMatrix. DenseMatrix with the resulting data</returns>
        public DenseMatrix eval(String exprString, Bitmap bmR, Bitmap bmG, Bitmap bmB, TextBox tbLog)
        {
            ArithmeticsOnImages aoi = new ArithmeticsOnImages();
            //aoi.MaskImage = significantMaskImage;
            aoi.imgR = new Image<Gray, byte>(bmR);
            aoi.imgG = new Image<Gray, byte>(bmG);
            aoi.imgB = new Image<Gray, byte>(bmB);
            aoi.ExprString = exprString;
            aoi.tbLog = tbLog;
            aoi.RPNeval();
            ServiceTools.FlushMemory(tbLog, "");
            if (evaluatingColorScheme != null)
                tmpImage = evalResultColored(aoi.dmRes, significantMaskImageBinary, evaluatingColorScheme);
            ServiceTools.FlushMemory(null, "");
            return aoi.dmRes;
            //tmpImage = tmpImage.Mul(BGRsignificantMaskImage);
            //MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmRes = 
        }





        public DenseMatrix eval(String exprString, DenseMatrix dmR, DenseMatrix dmG, DenseMatrix dmB, TextBox tbLog)
        {
            ArithmeticsOnImages aoi = new ArithmeticsOnImages();
            //aoi.MaskImage = significantMaskImage;
            aoi.dmR = dmR;
            aoi.dmG = dmG;
            aoi.dmB = dmB;
            //aoi.imgR = new Image<Gray, byte>(bmR);
            //aoi.imgG = new Image<Gray, byte>(bmG);
            //aoi.imgB = new Image<Gray, byte>(bmB);
            aoi.ExprString = exprString;
            aoi.tbLog = tbLog;
            aoi.RPNeval();
            ServiceTools.FlushMemory(tbLog, "");
            if (evaluatingColorScheme != null)
                tmpImage = evalResultColored(aoi.dmRes, significantMaskImageBinary, evaluatingColorScheme);
            ServiceTools.FlushMemory(null, "");
            return aoi.dmRes;
            //tmpImage = tmpImage.Mul(BGRsignificantMaskImage);
            //MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmRes = 
        }




        /// <summary>
        /// Colores the densematrix data with colors from the colorscheme
        /// NOTE: the dynamic values used referring the max and min values in the DenseMatrix source
        /// </summary>
        /// <param name="dmMatrixToColor">DenseMatrix to color</param>
        /// <param name="binaryMaskImage">The binary mask image.</param>
        /// <param name="usedColorScheme">The used color scheme.</param>
        /// <returns>Image{BgrByte}. Image representing the densematrix data with the given coloscheme</returns>
        public static Image<Bgr, Byte> evalResultColored(DenseMatrix dmMatrixToColor, Image<Gray, Byte> binaryMaskImage, ColorScheme usedColorScheme)
        {
            double minValue = dmMatrixToColor.Values.Min();
            double maxValue = dmMatrixToColor.Values.Max();


            DenseMatrix dmColCh;
            Image<Gray, Byte> imgRch, imgGch, imgBch;

            dmColCh = (DenseMatrix)dmMatrixToColor.Clone();
            dmColCh.MapInplace(
                new Func<double, double>(
                    (dValue) => { return usedColorScheme.GetColorByValueAndRange(dValue, minValue, maxValue).Red; }),
                MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            //dmColCh = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmColCh.PointwiseMultiply(dmMask);

            double[,] doubleArrDmTmp = dmColCh.ToArray();
            byte[,] byteArrDmTmp = ServiceTools.DoubleToByteDepth(doubleArrDmTmp);
            byte[, ,] byte2DArrDmTmp = ServiceTools.AddedThirdIndexArray3DFrom2D(byteArrDmTmp);
            imgRch = new Image<Gray, byte>(byte2DArrDmTmp);
            if (binaryMaskImage != null)
            {
                imgRch = imgRch.Mul(binaryMaskImage);
            }
            //dmColCh = null;
            //doubleArrDmTmp = null;
            //byte2DArrDmTmp = null;
            //ServiceTools.FlushMemory(null, "");


            dmColCh = (DenseMatrix)dmMatrixToColor.Clone();
            dmColCh.MapInplace(
                new Func<double, double>(
                    (dValue) => { return usedColorScheme.GetColorByValueAndRange(dValue, minValue, maxValue).Green; }),
                MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            //dmColCh = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmColCh.PointwiseMultiply(dmMask);

            doubleArrDmTmp = dmColCh.ToArray();
            byteArrDmTmp = ServiceTools.DoubleToByteDepth(doubleArrDmTmp);
            byte2DArrDmTmp = ServiceTools.AddedThirdIndexArray3DFrom2D(byteArrDmTmp);
            imgGch = new Image<Gray, byte>(byte2DArrDmTmp);
            //imgGch = imgGch.Mul(imageProcessingMaster.significantMaskImageBinary);
            if (binaryMaskImage != null)
            {
                imgGch = imgGch.Mul(binaryMaskImage);
            }
            //dmColCh = null;
            //doubleArrDmTmp = null;
            //byte2DArrDmTmp = null;
            //ServiceTools.FlushMemory(null, "");


            dmColCh = (DenseMatrix)dmMatrixToColor.Clone();
            dmColCh.MapInplace(
                new Func<double, double>(
                    (dValue) => { return usedColorScheme.GetColorByValueAndRange(dValue, minValue, maxValue).Blue; }),
                MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            //dmColCh = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmColCh.PointwiseMultiply(dmMask);

            doubleArrDmTmp = dmColCh.ToArray();
            byteArrDmTmp = ServiceTools.DoubleToByteDepth(doubleArrDmTmp);
            byte2DArrDmTmp = ServiceTools.AddedThirdIndexArray3DFrom2D(byteArrDmTmp);
            imgBch = new Image<Gray, byte>(byte2DArrDmTmp);
            //imgBch = imgBch.Mul(imageProcessingMaster.significantMaskImageBinary);
            if (binaryMaskImage != null)
            {
                imgBch = imgBch.Mul(binaryMaskImage);
            }
            //dmColCh = null;
            //doubleArrDmTmp = null;
            //byte2DArrDmTmp = null;
            ServiceTools.FlushMemory(null, "");

            Image<Bgr, Byte> TmpImage = new Image<Bgr, Byte>(new Image<Gray, Byte>[] { imgBch, imgGch, imgRch });

            return TmpImage;
        }




        /// <summary>
        /// Represents the DenseMatrix data colored with fixed data bounds using the given color scheme
        /// </summary>
        /// <param name="dmMatrixToColor">the DenseMatrix data to represent</param>
        /// <param name="binaryMaskImage">The binary mask image.</param>
        /// <param name="usedColorScheme">The used color scheme.</param>
        /// <param name="forcedMinValue">The forced fixed minimum value.</param>
        /// <param name="forcedMaxValue">The forced fixed maximum value.</param>
        /// <returns>Image{BgrByte}. Image representing the densematrix data with the given coloscheme</returns>
        public static Image<Bgr, Byte> evalResultColoredWithFixedDataBounds(DenseMatrix dmMatrixToColor, Image<Gray, Byte> binaryMaskImage, ColorScheme usedColorScheme, double forcedMinValue, double forcedMaxValue)
        {
            double minValue = forcedMinValue;
            double maxValue = forcedMaxValue;


            DenseMatrix dmColCh;
            Image<Gray, Byte> imgRch, imgGch, imgBch;

            dmColCh = (DenseMatrix)dmMatrixToColor.Clone();
            dmColCh.MapInplace(
                new Func<double, double>(
                    (dValue) => { return usedColorScheme.GetColorByValueAndRange(dValue, minValue, maxValue).Red; }),
                MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            //dmColCh = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmColCh.PointwiseMultiply(dmMask);

            double[,] doubleArrDmTmp = dmColCh.ToArray();
            byte[,] byteArrDmTmp = ServiceTools.DoubleToByteDepth(doubleArrDmTmp);
            byte[, ,] byte2DArrDmTmp = ServiceTools.AddedThirdIndexArray3DFrom2D(byteArrDmTmp);
            imgRch = new Image<Gray, byte>(byte2DArrDmTmp);
            if (binaryMaskImage != null)
            {
                imgRch = imgRch.Mul(binaryMaskImage);
            }
            //dmColCh = null;
            //doubleArrDmTmp = null;
            //byte2DArrDmTmp = null;
            //ServiceTools.FlushMemory(null, "");


            dmColCh = (DenseMatrix)dmMatrixToColor.Clone();
            dmColCh.MapInplace(
                new Func<double, double>(
                    (dValue) => { return usedColorScheme.GetColorByValueAndRange(dValue, minValue, maxValue).Green; }),
                MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            //dmColCh = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmColCh.PointwiseMultiply(dmMask);

            doubleArrDmTmp = dmColCh.ToArray();
            byteArrDmTmp = ServiceTools.DoubleToByteDepth(doubleArrDmTmp);
            byte2DArrDmTmp = ServiceTools.AddedThirdIndexArray3DFrom2D(byteArrDmTmp);
            imgGch = new Image<Gray, byte>(byte2DArrDmTmp);
            //imgGch = imgGch.Mul(imageProcessingMaster.significantMaskImageBinary);
            if (binaryMaskImage != null)
            {
                imgGch = imgGch.Mul(binaryMaskImage);
            }
            //dmColCh = null;
            //doubleArrDmTmp = null;
            //byte2DArrDmTmp = null;
            ServiceTools.FlushMemory(null, "");


            dmColCh = (DenseMatrix)dmMatrixToColor.Clone();
            dmColCh.MapInplace(
                new Func<double, double>(
                    (dValue) => { return usedColorScheme.GetColorByValueAndRange(dValue, minValue, maxValue).Blue; }),
                MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            //dmColCh = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmColCh.PointwiseMultiply(dmMask);

            doubleArrDmTmp = dmColCh.ToArray();
            byteArrDmTmp = ServiceTools.DoubleToByteDepth(doubleArrDmTmp);
            byte2DArrDmTmp = ServiceTools.AddedThirdIndexArray3DFrom2D(byteArrDmTmp);
            imgBch = new Image<Gray, byte>(byte2DArrDmTmp);
            //imgBch = imgBch.Mul(imageProcessingMaster.significantMaskImageBinary);
            if (binaryMaskImage != null)
            {
                imgBch = imgBch.Mul(binaryMaskImage);
            }
            //dmColCh = null;
            //doubleArrDmTmp = null;
            //byte2DArrDmTmp = null;
            ServiceTools.FlushMemory(null, "");

            Image<Bgr, Byte> TmpImage = new Image<Bgr, Byte>(new Image<Gray, Byte>[] { imgBch, imgGch, imgRch });

            return TmpImage;
        }








        public static DenseMatrix DenseMatrixFromImage(Bitmap anImage)
        {
            Image<Gray, Byte> img1 = new Image<Gray, byte>(anImage);
            double[,] img1Double2d = ThirdIndexArraySliceDouble2DFromByte3D((img1).Data, 0);
            DenseMatrix img1DoubleDM = DenseMatrix.OfArray(img1Double2d);
            return img1DoubleDM;
        }


        public static Image<Gray, Byte> grayscaleImageFromDenseMatrix(DenseMatrix dm2Convert, bool invertScale = false)
        {
            DenseMatrix dmColCh;
            Image<Gray, Byte> imgRch;
            double minVal = dm2Convert.Values.Min();
            double maxVal = dm2Convert.Values.Max();

            dmColCh = (DenseMatrix)dm2Convert.Clone();
            double valuesRange = maxVal - minVal;
            //dmColCh.MapInplace(new Func<double, double>((dValue) => { return usedColorScheme.getColorByValueAndRange(dValue, minValue, maxValue).Red; }), true);
            if (!invertScale)
                dmColCh.MapInplace(
                    new Func<double, double>((dValue) => { return 255.0d*((dValue - minVal)/valuesRange); }),
                    MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            else
                dmColCh.MapInplace(
                    new Func<double, double>((dValue) => { return 255.0d*((maxVal - dValue)/valuesRange); }),
                    MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            //dmColCh = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmColCh.PointwiseMultiply(dmMask);

            double[,] doubleArrDmTmp = dmColCh.ToArray();
            byte[,] byteArrDmTmp = ServiceTools.DoubleToByteDepth(doubleArrDmTmp);
            byte[, ,] byte2DArrDmTmp = ServiceTools.AddedThirdIndexArray3DFrom2D(byteArrDmTmp);
            imgRch = new Image<Gray, byte>(byte2DArrDmTmp);
            ServiceTools.FlushMemory(null, "");
            return imgRch;
        }



        public static Image<Gray, Byte> grayscaleImageFromDenseMatrixWithFixedValuesBounds(DenseMatrix dm2Convert, double minValueIn, double maxValueIn, bool invertScale = false)
        {
            DenseMatrix dmColCh;
            Image<Gray, Byte> imgRch;
            double minVal = minValueIn;// dm2Convert.Values.Min();
            double maxVal = maxValueIn;// dm2Convert.Values.Max();

            dmColCh = (DenseMatrix)dm2Convert.Clone();
            //dmColCh.MapInplace(new Func<double, double>((dValue) => { return usedColorScheme.getColorByValueAndRange(dValue, minValue, maxValue).Red; }), true);
            double valuesRange = maxVal - minVal;
            if (!invertScale)
                dmColCh.MapInplace(
                    new Func<double, double>((dValue) => { return 255.0d*((dValue - minVal)/valuesRange); }),
                    MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);
            else
                dmColCh.MapInplace(
                    new Func<double, double>((dValue) => { return 255.0d*((maxVal - dValue)/valuesRange); }),
                    MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);

            //dmColCh = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmColCh.PointwiseMultiply(dmMask);

            double[,] doubleArrDmTmp = dmColCh.ToArray();
            byte[,] byteArrDmTmp = ServiceTools.DoubleToByteDepth(doubleArrDmTmp);
            byte[, ,] byte2DArrDmTmp = ServiceTools.AddedThirdIndexArray3DFrom2D(byteArrDmTmp);
            imgRch = new Image<Gray, byte>(byte2DArrDmTmp);
            //dmColCh = null;
            //doubleArrDmTmp = null;
            //byte2DArrDmTmp = null;
            ServiceTools.FlushMemory(null, "");
            return imgRch;
        }




        public static DenseMatrix DenseMatrixFromImage(Image<Gray, Byte> anImage)
        {
            //Image<Gray, Byte> img1 = new Image<Gray, byte>(anImage);
            double[,] img1Double2d = ThirdIndexArraySliceDouble2DFromByte3D((anImage).Data, 0);
            DenseMatrix img1DoubleDM = DenseMatrix.OfArray(img1Double2d);
            return img1DoubleDM;
        }



        public static Bitmap SquareImageDimensions(Bitmap bm)
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(bm);
            img = SquareImageDimensions(img);
            return img.Bitmap;
        }



        public static Image SquareImageDimensions(Image img)
        {
            Bitmap bm = (Bitmap)img;
            bm = SquareImageDimensions(bm);
            img = (Image)bm;
            return img;
        }



        public static Image<Bgr, byte> SquareImageDimensions(Image<Bgr, byte> img)
        {
            if (img.Width % 4 != 0)
            {
                int d_Width = 4 - (img.Width % 4);
                img = img.ConcateHorizontal(new Image<Bgr, byte>(d_Width, img.Height));
            }
            if (img.Height % 4 != 0)
            {
                int d_Height = 4 - (img.Height % 4);
                img = img.ConcateVertical(new Image<Bgr, byte>(img.Width, d_Height));
            }
            return img;
        }




        public static Bgr ContrastColorFromBgrColor(Bgr sourceColor)
        {
            const double gamma = 2.2d;
            double L = 0.2126 * Math.Pow(sourceColor.Red / 255.0d, gamma) + 0.7152 * Math.Pow(sourceColor.Green / 255.0d, gamma) + 0.0722 * Math.Pow(sourceColor.Blue / 255.0d, gamma);
            bool useBlack = (L > 0.5d);
            if (useBlack)
            {
                return new Bgr(0, 0, 0);
            }
            else return new Bgr(255, 255, 255);
        }


        public static Image<Bgr, Byte> ConvertGrayImageToBgr(Image<Gray, Byte> sourceImage)
        {
            return new Image<Bgr, byte>(new Image<Gray, byte>[] { sourceImage, sourceImage, sourceImage });
        }


        public static Image<Bgr, double> ConvertGrayImageToBgr(Image<Gray, double> sourceImage)
        {
            return new Image<Bgr, double>(new Image<Gray, double>[] { sourceImage, sourceImage, sourceImage });
        }




        /// <summary>
        /// Gets the convex contours list from non convex contour.
        /// </summary>
        /// <param name="sourceContour">The source contour.</param>
        /// <returns>List{Contour{Point}}.</returns>
        public static List<Contour<Point>> GetConvexContoursListFromNonConvexContour(Contour<Point> sourceContour)
        {
            List<Contour<Point>> theResultContoursList = new List<Contour<Point>>();
            if (sourceContour.Convex)
            {
                theResultContoursList.Add(sourceContour);
                return theResultContoursList;
            }

            Seq<Point> theHull = sourceContour.GetConvexHull(ORIENTATION.CV_CLOCKWISE, new MemStorage());
            Seq<MCvConvexityDefect> convexityDefects = sourceContour.GetConvexityDefacts(new MemStorage(),
                ORIENTATION.CV_CLOCKWISE);
            if (convexityDefects.Count() == 1)
            {
                //разрежем по направлению медианы невыпуклости
                MCvConvexityDefect theDefect = convexityDefects[0];
                Point startPoint = theDefect.StartPoint;
                int startPointIndex = GetSeqElementIndex(sourceContour, startPoint);
                Point endPoint = theDefect.EndPoint;
                int endPointIndex = GetSeqElementIndex(sourceContour, endPoint);
                double dist = theDefect.Depth;
                Point depthPoint = theDefect.DepthPoint;
                int depthPointIndex = GetSeqElementIndex(sourceContour, depthPoint);

                double aNorm = (double)(startPoint.X - endPoint.X);
                double bNorm = (double)(startPoint.Y - endPoint.Y);
                double cNorm = -aNorm * (double)depthPoint.X - bNorm * (double)depthPoint.Y;
                double minD = Math.Min(PointToABCLineDistance(aNorm, bNorm, cNorm, startPoint), PointToABCLineDistance(aNorm, bNorm, cNorm, endPoint));
                int theNeededIndex = depthPointIndex;

                for (int i = 0; i < sourceContour.Count(); i++)
                {
                    Point currSeqPoint = sourceContour[i];
                    if (IsIndexBetweenOtherTwo(startPointIndex, endPointIndex, i)) continue;
                    double theDistance = PointToABCLineDistance(aNorm, bNorm, cNorm, currSeqPoint);
                    if (theDistance < minD)
                    {
                        minD = theDistance;
                        theNeededIndex = i;
                    }
                }

                MCvSlice slice1 = new MCvSlice(depthPointIndex, theNeededIndex);
                MCvSlice slice2 = new MCvSlice(theNeededIndex, depthPointIndex);
                Contour<Point> slicedContour1 = (Contour<Point>)sourceContour.Slice(slice1, new MemStorage(), true);
                Contour<Point> slicedContour2 = (Contour<Point>)sourceContour.Slice(slice2, new MemStorage(), true);
                theResultContoursList.Add(slicedContour1);
                theResultContoursList.Add(slicedContour2);
            }
            else
            {
                //разрежем от первой точки невыпуклости до точки невыпуклости самой ближней к ее медиане
                MCvConvexityDefect theDefect = convexityDefects[0];
                Point startPoint = theDefect.StartPoint;
                int startPointIndex = GetSeqElementIndex(sourceContour, startPoint);
                Point endPoint = theDefect.EndPoint;
                int endPointIndex = GetSeqElementIndex(sourceContour, endPoint);
                double dist = theDefect.Depth;
                Point depthPoint = theDefect.DepthPoint;
                int depthPointIndex = GetSeqElementIndex(sourceContour, depthPoint);

                double aNorm = (double)(startPoint.X - endPoint.X);
                double bNorm = (double)(startPoint.Y - endPoint.Y);
                double cNorm = -aNorm * (double)depthPoint.X - bNorm * (double)depthPoint.Y;
                //double minD = Math.Min(PointToABCLineDistance(aNorm, bNorm, cNorm, startPoint), PointToABCLineDistance(aNorm, bNorm, cNorm, endPoint));
                double minD = 0.0d;
                int theNeededIndex = depthPointIndex;

                for (int i = 1; i < convexityDefects.Count(); i++)
                {
                    Point currSeqPoint = convexityDefects[i].DepthPoint;
                    if (IsIndexBetweenOtherTwo(startPointIndex, endPointIndex, i)) continue;
                    double theDistance = PointToABCLineDistance(aNorm, bNorm, cNorm, currSeqPoint);
                    if ((theDistance < minD) || (minD == 0.0d))
                    {
                        minD = theDistance;
                        theNeededIndex = GetSeqElementIndex(sourceContour, currSeqPoint);
                    }
                }


                MCvSlice slice1 = new MCvSlice(depthPointIndex, theNeededIndex);
                MCvSlice slice2 = new MCvSlice(theNeededIndex, depthPointIndex);
                MemStorage memStorage1 = new MemStorage();
                Seq<Point> slicedSeq1 = sourceContour.Slice(slice1, memStorage1, true);
                Contour<Point> slicedContour1 = new Contour<Point>(slicedSeq1.Ptr, memStorage1);
                if ((!slicedContour1.Convex) && (slicedContour1.Area >= 9))
                {
                    List<Contour<Point>> slicedContour1Subcontours =
                        GetConvexContoursListFromNonConvexContour(slicedContour1);
                    foreach (Contour<Point> slicedContour1Subcontour in slicedContour1Subcontours)
                    {
                        theResultContoursList.Add(slicedContour1Subcontour);
                    }
                }
                else
                {
                    theResultContoursList.Add(slicedContour1);
                }
                MemStorage memStorage2 = new MemStorage();
                Seq<Point> slicedSeq2 = sourceContour.Slice(slice2, memStorage2, true);
                Contour<Point> slicedContour2 = new Contour<Point>(slicedSeq2.Ptr, memStorage2);
                if ((!slicedContour2.Convex) && (slicedContour2.Area >= 9))
                {
                    List<Contour<Point>> slicedContour2Subcontours =
                        GetConvexContoursListFromNonConvexContour(slicedContour2);
                    foreach (Contour<Point> slicedContour1Subcontour in slicedContour2Subcontours)
                    {
                        theResultContoursList.Add(slicedContour1Subcontour);
                    }
                }
                else
                {
                    theResultContoursList.Add(slicedContour2);
                }

            }
            return theResultContoursList;
        }




        private static int GetSeqElementIndex(Seq<Point> theSequence, Point thePointItem)
        {
            for (int i = 0; i < theSequence.Count(); i++)
            {
                Point thePoint = theSequence[i];
                if (thePoint == thePointItem)
                {
                    return i;
                }
            }
            return 0;
        }


        private static bool IsIndexBetweenOtherTwo(int startIndex, int endIndex, int testIndex)
        {
            if (startIndex < endIndex)
            {
                if ((testIndex > startIndex) && (testIndex < endIndex))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (startIndex > endIndex)
            {
                return IsIndexBetweenOtherTwo(0, endIndex, testIndex) || (testIndex > startIndex);
            }
            return false;
        }


        private static double PointToABCLineDistance(double lineParamA, double lineParamB, double lineParamC, Point testPoint)
        {
            double dist = lineParamA * testPoint.X + lineParamB * testPoint.Y + lineParamC;
            double dValRadical = Math.Sqrt(lineParamA * lineParamA + lineParamB * lineParamB);
            dist = dist / dValRadical;
            return Math.Abs(dist);
        }




        #region 3D-2D array operations
        public static double[,] ThirdIndexArraySliceDouble2DFromDouble3D(double[, ,] inputArray, int thirdIndex)
        {
            double[,] result = new double[inputArray.GetLength(0), inputArray.GetLength(1)];

            for (int i = 0; i < inputArray.GetLength(0); i++)
            {
                for (int j = 0; j < inputArray.GetLength(1); j++)
                {
                    result[i, j] = inputArray[i, j, thirdIndex];
                }
            }

            return result;
        }

        public static double[,] ThirdIndexArraySliceDouble2DFromByte3D(Byte[, ,] inputArray, int thirdIndex)
        {
            double[,] result = new double[inputArray.GetLength(0), inputArray.GetLength(1)];

            for (int i = 0; i < inputArray.GetLength(0); i++)
            {
                for (int j = 0; j < inputArray.GetLength(1); j++)
                {
                    result[i, j] = (double)inputArray[i, j, thirdIndex];
                }
            }

            return result;
        }

        public static Byte[,] ThirdIndexArraySliceByte2DFromByte3D(Byte[, ,] inputArray, int thirdIndex)
        {
            Byte[,] result = new Byte[inputArray.GetLength(0), inputArray.GetLength(1)];

            for (int i = 0; i < inputArray.GetLength(0); i++)
            {
                for (int j = 0; j < inputArray.GetLength(1); j++)
                {
                    result[i, j] = inputArray[i, j, thirdIndex];
                }
            }

            return result;
        }

        public static Byte[,] ThirdIndexArraySliceByte2DFromDouble3D(double[, ,] inputArray, int thirdIndex)
        {
            Byte[,] result = new Byte[inputArray.GetLength(0), inputArray.GetLength(1)];

            for (int i = 0; i < inputArray.GetLength(0); i++)
            {
                for (int j = 0; j < inputArray.GetLength(1); j++)
                {
                    result[i, j] = (Byte)inputArray[i, j, thirdIndex];
                }
            }

            return result;
        }
        #endregion



        //public static Image ImageResizer(Image imgToResize, int maxSideLength = 1024)
        //{
        //    int sourceWidth = imgToResize.Width;
        //    int sourceHeight = imgToResize.Height;
        //
        //    int sourceMaxSide = Math.Max(sourceWidth, sourceHeight);
        //    if (sourceMaxSide > maxSideLength)
        //    {
        //        double koeff = ((double)maxSideLength / (double)sourceMaxSide);
        //        int newWidth = Convert.ToInt32(koeff * sourceWidth);
        //        int neHeight = Convert.ToInt32(koeff * sourceHeight);
        //
        //
        //        Bitmap b = new Bitmap(newWidth, neHeight);
        //        Graphics g = Graphics.FromImage((Image)b);
        //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //
        //        g.DrawImage(imgToResize, 0, 0, newWidth, neHeight);
        //        g.Dispose();
        //
        //        b = ImageProcessing.SquareImageDimensions(b);
        //
        //        return (Image)b;
        //    }
        //    else
        //    {
        //        return imgToResize;
        //    }
        //}



        public static Image<Bgr, byte> ImageResizer(Image<Bgr, Byte> imgToResize, int maxSideLength = 1024)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            int sourceMaxSide = Math.Max(sourceWidth, sourceHeight);
            if (sourceMaxSide > maxSideLength)
            {
                double koeff = ((double)maxSideLength / (double)sourceMaxSide);
                Image<Bgr, byte> retImg = imgToResize.Copy();
                retImg = retImg.Resize(koeff, INTER.CV_INTER_AREA);
                return retImg;
            }
            else
            {
                return imgToResize;
            }
        }








        public static Bitmap BitmapResizer(Bitmap bmToResize, int maxSideLength = 1024)
        {
            int sourceWidth = bmToResize.Width;
            int sourceHeight = bmToResize.Height;

            int sourceMaxSide = Math.Max(sourceWidth, sourceHeight);
            if (sourceMaxSide > maxSideLength)
            {
                double koeff = ((double)maxSideLength / (double)sourceMaxSide);
                int newWidth = Convert.ToInt32(koeff * sourceWidth);
                int neHeight = Convert.ToInt32(koeff * sourceHeight);


                Bitmap b = new Bitmap(newWidth, neHeight);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmToResize, 0, 0, newWidth, neHeight);
                g.Dispose();

                b = SquareImageDimensions(b);

                return b;
            }
            else
            {
                return bmToResize;
            }
        }




        public void Dispose()
        {
            //throw new NotImplementedException();
            ServiceTools.FlushMemory(null, null);
        }
    }
}
