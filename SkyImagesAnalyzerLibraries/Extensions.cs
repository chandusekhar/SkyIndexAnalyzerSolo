using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Geometry;
using MathNet.Numerics.LinearAlgebra.Double;


namespace SkyImagesAnalyzerLibraries
{
    public static class Extensions
    {
        public static Gray white = new Gray(255);
        /// <summary>
        /// Determines whether the testing point is inside the circle.
        /// </summary>
        /// <param name="pt">The testing point.</param>
        /// <param name="theCircle">The circle.</param>
        /// <returns>System.Int32:
        /// -1 => pt is outside the circle
        /// 0 => pt is at the circle margin
        /// 1 => pt is inside the circle
        /// default (for empty circle data) is outside
        /// </returns>
        public static int IsPointInsideCircle(this PointD pt, RoundData theCircle, double marginDetectorPrecision = 2.0d)
        {
            if (theCircle.IsNull) return -1; // default is outside

            if (PointD.Distance(pt, theCircle.pointDCircleCenter()) < theCircle.DRadius) return 1;
            if (Math.Abs(PointD.Distance(pt, theCircle.pointDCircleCenter()) - theCircle.DRadius) <= marginDetectorPrecision) return 0;
            return -1;
        }





        public static PointD GetMouseEventPositionOnRealImage<T1, T2>(this PictureBox pbElement, EventArgs mouseEventArgs,
            Image<T1, T2> origImage)
            where T1 : struct, IColor
            where T2 : new()
        {
            // пересчитаем точку из PictureBox в картинку, которая в нем
            PointD retPointD = new PointD();
            PointD mouseClickPoint = new PointD(((MouseEventArgs)mouseEventArgs).Location);
            Size currPresentedImageSize = pbElement.Image.Size;
            retPointD.X = (mouseClickPoint.X / (double)currPresentedImageSize.Width) * (double)origImage.Width;
            retPointD.Y = (mouseClickPoint.Y / (double)currPresentedImageSize.Height) * (double)origImage.Height;
            return retPointD;
        }



        public static DenseVector ToVector(this PointD p0, PointD p1)
        {
            DenseVector dv1 = DenseVector.Create(2, i => (i == 0) ? (p1.X - p0.X) : (p1.Y - p0.Y));
            return dv1;
        }



        public static Vector2D ToVector2D(this PointD p0, PointD p1)
        {
            return new Vector2D(p0, p1);
        }



        public static string SaveNetCDFdataMatrix(this DenseMatrix dmSource, string fileName, string varName = "dataMatrix")
        {
            if (fileName != "")
            {
                try
                {
                    NetCDFoperations.AddDataMatrixToFile(dmSource, fileName, null, true, varName);
                    return fileName;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return "";
        }






        public static string SaveVectorDataAsImagePlot(this IEnumerable<double> enumSource, string fileName, SequencesDrawingVariants variant = SequencesDrawingVariants.circles)
        {
            if (fileName != "")
            {
                try
                {
                    MultipleScatterAndFunctionsRepresentation plotter = new MultipleScatterAndFunctionsRepresentation(2560, 1600);
                    DenseVector xSpace = DenseVector.Create(enumSource.Count(), idx => (double)idx);
                    DenseVector funcValues = DenseVector.OfEnumerable(enumSource);
                    plotter.dvScatterXSpace.Add(xSpace);
                    plotter.dvScatterFuncValues.Add(funcValues);
                    plotter.scatterDrawingVariants.Add(variant);
                    plotter.scatterLineColors.Add(new Bgr(Color.Green));
                    plotter.Represent();
                    plotter.SaveToImage(fileName);
                    return fileName;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            return "";
        }






        public static DictionaryBindingList<TKey, TValue>
            ToBindingList<TKey, TValue>(this IDictionary<TKey, TValue> data)
        {
            return new DictionaryBindingList<TKey, TValue>(data);
        }



        public static DenseMatrix Copy(this DenseMatrix dmSource)
        {
            DenseMatrix dmRet = DenseMatrix.Create(dmSource.RowCount, dmSource.ColumnCount, (r, c) => dmSource[r, c]);
            return dmRet;
        }

        

        public static bool ContainsContourInside(this Contour<Point> testingContour, Contour<Point> sampleContour)
        {
            List<Point> lpointsOfSampleContour = new List<Point>(sampleContour.ToArray());
            return lpointsOfSampleContour.TrueForAll(pt => (testingContour.InContour(new PointF(pt.X, pt.Y)) >= 0.0d));
        }



        public static Contour<Point> Intersection(this Contour<Point> c1, Contour<Point> c2)
        {
            Rectangle c1Rect = c1.BoundingRectangle; //CvInvoke.BoundingRectangle(c1);
            Rectangle c2Rect = c2.BoundingRectangle; //CvInvoke.BoundingRectangle(c2);
            int right = Math.Max(c1Rect.Right, c2Rect.Right);
            int bttm = Math.Max(c1Rect.Bottom, c2Rect.Bottom);
            Image<Gray, byte> img1 = new Image<Gray, byte>(new Size(right, bttm));
            img1.Draw(c1, white, -1);
            Image<Gray, byte> img2 = new Image<Gray, byte>(new Size(right, bttm));
            img2.Draw(c2, white, -1);
            img1 = img1.And(img2);
            //img1 = img1.ThresholdBinary(new Gray(150), white);

            //VectorOfVectorOfPoint contoursDetected = new VectorOfVectorOfPoint();
            //CvInvoke.FindContours(img1, contoursDetected, null, Emgu.CV.CvEnum.RetrType.List,
            //    Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            List<Contour<Point>> contoursDetected = img1.DetectContours();
            if (contoursDetected.Any())
            {
                return contoursDetected[0];
            }
            else return null;
        }



        

        public static PointD MassCenter(this Contour<Point> theContour)
        {
            //Rectangle rectContourBounding = CvInvoke.BoundingRectangle(theContour);
            Rectangle rectContourBounding = theContour.BoundingRectangle; // CvInvoke.BoundingRectangle(theContour);
            Image<Gray, byte> tmpImg = new Image<Gray, byte>(rectContourBounding.Right,
                rectContourBounding.Bottom);
            //tmpImg.Draw(theContour.ToArray(), white, -1);
            tmpImg.Draw(theContour, white, -1);
            MCvMoments moments = tmpImg.GetMoments(true);
            double cx = moments.m10 / moments.m00;
            double cy = moments.m01 / moments.m00;
            return new PointD(cx, cy);
        }



        public static Point Flip45degrees(this Point pt)
        {
            return new Point(pt.Y, pt.X);
        }


        public static Point FlipUpsideDown(this Point pt, Size imageSize)
        {
            return new Point(imageSize.Height - pt.Y, pt.X);
        }


        public static Point FlipLeftToRight(this Point pt, Size imageSize)
        {
            return new Point(pt.Y, imageSize.Width - pt.X);
        }



        public static Color ToRGBColor(this Bgr bgrColor)
        {
            Color retColor = Color.FromArgb(Convert.ToInt32(bgrColor.Red), Convert.ToInt32(bgrColor.Green), Convert.ToInt32(bgrColor.Blue));
            return retColor;
        }





        public static double Abs(this DenseVector dv)
        {
            return Math.Sqrt(dv * dv);
        }



        public static DenseVector Copy(this DenseVector dv)
        {
            return DenseVector.OfEnumerable(dv.Values);
        }




        public static bool SaveDataToCSV(this DenseVector dv, string fileName)
        {
            ServiceTools.logToTextFile(fileName, ServiceTools.densevectorToString(dv));
            return true;
        }




        public static TimeSpan RoundToSeconds(this TimeSpan timeSpan)
        {
            return TimeSpan.FromSeconds(Math.Round(timeSpan.TotalSeconds));
        }





        //public static double Area(this VectorOfPoint contour)
        //{
        //    return CvInvoke.ContourArea(contour);
        //}




        public static List<Contour<Point>> DetectContours(this Image<Bgr, byte> img,
            Emgu.CV.CvEnum.RETR_TYPE mode = Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST,
            Emgu.CV.CvEnum.CHAIN_APPROX_METHOD method = Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE)
        {
            List<Contour<Point>> contoursDetected = new List<Contour<Point>>();

            Contour<Point> firstContourDetected = img.FindContours(method, mode);
            Contour<Point> currContour = firstContourDetected;

            while (true)
            {
                contoursDetected.Add(currContour);
                currContour = currContour.HNext;
                if (currContour == null)
                    break;
            }

            return contoursDetected;


            //CvInvoke.FindContours(img, contoursDetected, null, mode, method);
            //List<VectorOfPoint> contoursArray = new List<VectorOfPoint>();
            //int count = contoursDetected.Size;
            //for (int i = 0; i < count; i++)
            //{
            //    using (VectorOfPoint currContour = contoursDetected[i])
            //    {
            //        contoursArray.Add(currContour);
            //    }
            //}
            //return contoursArray;
        }




        public static List<Contour<Point>> DetectContours(this Image<Gray, byte> img,
            Emgu.CV.CvEnum.RETR_TYPE mode = Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST,
            Emgu.CV.CvEnum.CHAIN_APPROX_METHOD method = Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE)
        {
            List<Contour<Point>> contoursDetected = new List<Contour<Point>>();

            Contour<Point> firstContourDetected = img.FindContours(method, mode);
            Contour<Point> currContour = firstContourDetected;

            while (true)
            {
                contoursDetected.Add(currContour);
                currContour = currContour.HNext;
                if (currContour == null)
                    break;
            }

            return contoursDetected;


            //CvInvoke.FindContours(img, contoursDetected, null, mode, method);
            //List<VectorOfPoint> contoursArray = new List<VectorOfPoint>();
            //int count = contoursDetected.Size;
            //for (int i = 0; i < count; i++)
            //{
            //    using (VectorOfPoint currContour = contoursDetected[i])
            //    {
            //        contoursArray.Add(currContour);
            //    }
            //}
            //return contoursArray;
        }



        public static List<Contour<Point>> DetectContours(this Image<Bgr, double> img,
            Emgu.CV.CvEnum.RETR_TYPE mode = Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST,
            Emgu.CV.CvEnum.CHAIN_APPROX_METHOD method = Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE)
        {
            List<Contour<Point>> contoursDetected = new List<Contour<Point>>();

            Contour<Point> firstContourDetected = img.FindContours(method, mode);
            Contour<Point> currContour = firstContourDetected;

            while (true)
            {
                contoursDetected.Add(currContour);
                currContour = currContour.HNext;
                if (currContour == null)
                    break;
            }

            return contoursDetected;


            //CvInvoke.FindContours(img, contoursDetected, null, mode, method);
            //List<VectorOfPoint> contoursArray = new List<VectorOfPoint>();
            //int count = contoursDetected.Size;
            //for (int i = 0; i < count; i++)
            //{
            //    using (VectorOfPoint currContour = contoursDetected[i])
            //    {
            //        contoursArray.Add(currContour);
            //    }
            //}
            //return contoursArray;
        }




        public static List<Contour<Point>> DetectContours(this Image<Gray, double> img,
            Emgu.CV.CvEnum.RETR_TYPE mode = Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST,
            Emgu.CV.CvEnum.CHAIN_APPROX_METHOD method = Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE)
        {
            List<Contour<Point>> contoursDetected = new List<Contour<Point>>();

            Contour<Point> firstContourDetected = img.FindContours(method, mode);
            Contour<Point> currContour = firstContourDetected;

            while (true)
            {
                contoursDetected.Add(currContour);
                currContour = currContour.HNext;
                if (currContour == null)
                    break;
            }

            return contoursDetected;


            //CvInvoke.FindContours(img, contoursDetected, null, mode, method);
            //List<VectorOfPoint> contoursArray = new List<VectorOfPoint>();
            //int count = contoursDetected.Size;
            //for (int i = 0; i < count; i++)
            //{
            //    using (VectorOfPoint currContour = contoursDetected[i])
            //    {
            //        contoursArray.Add(currContour);
            //    }
            //}
            //return contoursArray;
        }




        public static Rectangle GetBoundingRectangle(this MCvBox2D cvRotatedBox2D)
        {
            List<PointF> verticesList = new List<PointF>(cvRotatedBox2D.GetVertices());
            int verticesMinX = (int)verticesList.Min(ptf => ptf.X);
            int verticesMinY = (int)verticesList.Min(ptf => ptf.Y);
            int verticesMaxX = (int)verticesList.Max(ptf => ptf.X);
            int verticesMaxY = (int)verticesList.Max(ptf => ptf.Y);
            Rectangle resRect = new Rectangle(verticesMinX, verticesMinY, verticesMaxX - verticesMinX,
                verticesMaxY - verticesMinY);
            return resRect;
        }



        public static RectangleF GetBoundingRectangleF(this MCvBox2D cvRotatedBox2D)
        {
            List<PointF> verticesList = new List<PointF>(cvRotatedBox2D.GetVertices());
            float verticesMinX = verticesList.Min(ptf => ptf.X);
            float verticesMinY = verticesList.Min(ptf => ptf.Y);
            float verticesMaxX = verticesList.Max(ptf => ptf.X);
            float verticesMaxY = verticesList.Max(ptf => ptf.Y);
            RectangleF resRect = new RectangleF(verticesMinX, verticesMinY, verticesMaxX - verticesMinX,
                verticesMaxY - verticesMinY);
            return resRect;
        }


    }




    public class DictionaryBindingList<TKey, TValue>
            : BindingList<Pair<TKey, TValue>>
    {
        private readonly IDictionary<TKey, TValue> data;
        public DictionaryBindingList(IDictionary<TKey, TValue> data)
        {
            this.data = data;
            Reset();
        }
        public void Reset()
        {
            bool oldRaise = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            try
            {
                Clear();
                foreach (TKey key in data.Keys)
                {
                    Add(new Pair<TKey, TValue>(key, data));
                }
            }
            finally
            {
                RaiseListChangedEvents = oldRaise;
                ResetBindings();
            }
        }
    }




    public sealed class Pair<TKey, TValue>
    {
        private readonly TKey key;
        private readonly IDictionary<TKey, TValue> data;
        public Pair(TKey key, IDictionary<TKey, TValue> data)
        {
            this.key = key;
            this.data = data;
        }
        public TKey Key { get { return key; } }
        public TValue Value
        {
            get
            {
                TValue value;
                data.TryGetValue(key, out value);
                return value;
            }
            set { data[key] = value; }
        }
    }

}
