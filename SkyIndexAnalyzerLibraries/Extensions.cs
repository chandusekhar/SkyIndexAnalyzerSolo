using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;


namespace SkyIndexAnalyzerLibraries
{
    public static class Extensions
    {
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

    }
}
