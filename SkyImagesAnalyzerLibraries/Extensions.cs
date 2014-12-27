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
            where T1: struct, IColor
            where T2: new()
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


        public static DictionaryBindingList<TKey, TValue>
            ToBindingList<TKey, TValue>(this IDictionary<TKey, TValue> data)
        {
            return new DictionaryBindingList<TKey, TValue>(data);
        }




        public static DenseMatrix Conv2(this DenseMatrix dmSource, DenseMatrix dmKernel)
        {
            int kernelHalfSizeRows = (dmKernel.RowCount - 1)/2;
            int kernelHalfSizeCols = (dmKernel.ColumnCount - 1)/2;
            DenseMatrix dmRes = DenseMatrix.Create(dmSource.RowCount, dmSource.ColumnCount, (r, c) =>
            {
                int startRow = r - kernelHalfSizeRows;
                int dmSourceStartRow = Math.Max(startRow, 0);
                int kernelStartRow = (startRow >= 0) ? (0) : (-startRow);
                int endRow = r + kernelHalfSizeRows;
                int dmSourceEndRow = Math.Min(endRow, dmSource.RowCount - 1);
                int rowCount = dmSourceEndRow - dmSourceStartRow + 1;
                int startCol = c - kernelHalfSizeCols;
                int dmSourceStartCol = Math.Max(startCol, 0);
                int kernelStartCol = (startCol >= 0) ? (0) : (-startCol);
                int endCol = c + kernelHalfSizeCols;
                int dmSourceEndCol = Math.Min(endCol, dmSource.ColumnCount - 1);
                int colCount = dmSourceEndCol - dmSourceStartCol + 1;

                DenseMatrix dmSourceSubmatrix = (DenseMatrix)dmSource.SubMatrix(dmSourceStartRow, rowCount, dmSourceStartCol, colCount);
                DenseMatrix dmKernelSubMatrix =
                    (DenseMatrix) dmKernel.SubMatrix(kernelStartRow, rowCount, kernelStartCol, colCount);
                DenseMatrix sumMatrix = (DenseMatrix)(dmSourceSubmatrix.PointwiseMultiply(dmKernelSubMatrix));
                return sumMatrix.Values.Sum();
            });
            return dmRes;
        }



        public static DenseMatrix Copy(this DenseMatrix dmSource)
        {
            DenseMatrix dmRet = DenseMatrix.Create(dmSource.RowCount, dmSource.ColumnCount, (r, c) => dmSource[r, c]);
            return dmRet;
        }





        public static bool ContainsContourInside(this Contour<Point> testingContour, Contour<Point> sampleContour)
        {
            Rectangle rTestingRect = testingContour.BoundingRectangle;
            Rectangle rSampleRect = sampleContour.BoundingRectangle;
            Point pMin = new Point(Math.Min(rTestingRect.Location.X, rSampleRect.Location.X),
                Math.Min(rTestingRect.Location.Y, rSampleRect.Location.Y));
            int xMax = Math.Max(rTestingRect.Right, rSampleRect.Right);
            int yMax = Math.Max(rTestingRect.Bottom, rSampleRect.Bottom);
            Size theSize = new Size(xMax - pMin.X, yMax - pMin.Y);
            Rectangle minContainingRect = new Rectangle(pMin, theSize);

            Image<Gray, byte> testImg = new Image<Gray, byte>(theSize);
            Contour<Point> testingContourShifted = testingContour.CopyWithTransform(pt =>
            {
                return pt - new Size(pMin);
            });
            Contour<Point> sampleContourShifted = sampleContour.CopyWithTransform(pt =>
            {
                return pt - new Size(pMin);
            });

            Contour<Point> intersectionContour = testingContourShifted.Intersection(sampleContourShifted);
            if (intersectionContour == null) return false;
            if (intersectionContour.Area < sampleContourShifted.Area) return false;
            return true;
        }




        public static Contour<Point> Intersection(this Contour<Point> c1, Contour<Point> c2)
        {
            Rectangle c1Rect = c1.BoundingRectangle;
            Rectangle c2Rect = c2.BoundingRectangle;
            int right = Math.Max(c1Rect.Right, c2Rect.Right);
            int bttm = Math.Max(c1Rect.Bottom, c2Rect.Bottom);
            Image<Gray, byte> img1 = new Image<Gray, byte>(new Size(right, bttm));
            img1.Draw(c1, new Gray(255), 0);
            Image<Gray, byte> img2 = new Image<Gray, byte>(new Size(right, bttm));
            img2.Draw(c2, new Gray(255), 0);
            img1 = img1.And(img2);

            Contour<Point> contoursDetected =
                img1.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                    Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
            return contoursDetected;
        }




        public static Contour<Point> CopyWithTransform(this Contour<Point> sourceContour, Func<Point, Point> transform)
        {
            Contour<Point> copyContourTransformed = new Contour<Point>(new MemStorage());
            foreach (Point pt in sourceContour)
            {
                Point ptTransformed = transform(pt);
                copyContourTransformed.Push(ptTransformed);
            }
            return copyContourTransformed;
        }



        public static Contour<Point> Copy(this Contour<Point> sourceContour)
        {
            Contour<Point> copyContour = new Contour<Point>(new MemStorage());
            copyContour.PushMulti(sourceContour.ToArray(), Emgu.CV.CvEnum.BACK_OR_FRONT.BACK);
            return copyContour;
        }




        public static PointD MassCenter(this Contour<Point> theContour)
        {
            Image<Gray, byte> tmpImg = new Image<Gray, byte>(theContour.BoundingRectangle.Right,
                theContour.BoundingRectangle.Bottom);
            tmpImg.Draw(theContour, white, 0);
            MCvMoments moments = tmpImg.GetMoments(true);
            double cx = moments.m10/moments.m00;
            double cy = moments.m01/moments.m00;
            return new PointD(cx, cy);
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
