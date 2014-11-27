using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
//using MathNet.Numerics.LinearAlgebra.Generic;

namespace SkyImagesAnalyzerLibraries
{
    public class SimpleVectorGraphics2D
    {
        public List<DenseVector> listVectorsToDraw = new List<DenseVector>();
        public List<DenseVector> listVectorsShift = new List<DenseVector>();

        public List<Bgr> listVectColors = new List<Bgr>();

        public Size pictureSize = new Size(800, 600);
        public PointD ptLeftTopSpaceCorner = new PointD(-5.0d, 0.0d);
        public PointD ptRightBottomSpaceCorner = new PointD(5.0d, -10.0d);

        int serviceSpaceGapX = 40;
        int serviceSpaceGapY = 40;

        private Bgr colorBlack = new Bgr(0, 0, 0);
        private Bgr colorWhite = new Bgr(255, 255, 255);
        private Bgr colorGreen = new Bgr(Color.LightGreen);

        private Image<Bgr, byte> theImage = null;
        private Image<Bgr, byte> imgAxesLayer = null;
        private Image<Bgr, byte> imgVectorsLayer = null;

        public bool presentAxes = true;


        public SimpleVectorGraphics2D()
        {
        }


        public SimpleVectorGraphics2D(Size targetPictureSize)
        {
            pictureSize = targetPictureSize;
        }

        public Image<Bgr, byte> TheImage
        {
            get
            {
                Image<Bgr, byte> retImage = theImage.Copy();
                if (imgAxesLayer != null) retImage = retImage.Add(imgAxesLayer);
                if (imgVectorsLayer != null) retImage = retImage.Add(imgVectorsLayer);
                
                return retImage;
            }
        }


        public void ResizeCanvas(Size newSize)
        {
            pictureSize = newSize;
            Represent();
        }



        public void Represent()
        {
            #region //
            //if ((dvScatterXSpace != null) && (dvScatterFuncValues != null))
            //{
            //    if (theRepresentingFunctions.Count > 0)
            //    {

            //        double ySpaceGap = 0.2d *
            //                           (dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) -
            //                            dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())));

            //        overallFuncMax = ((dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) + ySpaceGap) > overallFuncMax)
            //            ? (dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) + ySpaceGap)
            //            : (overallFuncMax);
            //        overallFuncMin = ((dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())) - ySpaceGap) < overallFuncMin)
            //            ? (dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())) - ySpaceGap)
            //            : (overallFuncMin);

            //        xSpaceMin = ((dvScatterXSpace.Min((dvCurVector => dvCurVector.Min()))) < xSpaceMin)
            //            ? (dvScatterXSpace.Min((dvCurVector => dvCurVector.Min())))
            //            : (xSpaceMin);
            //        xSpaceMax = ((dvScatterXSpace.Max((dvCurVector => dvCurVector.Max()))) > xSpaceMax)
            //            ? (dvScatterXSpace.Max((dvCurVector => dvCurVector.Max())))
            //            : (xSpaceMax);
            //    }
            //    else
            //    {
            //        double ySpaceGap = 0.2d *
            //                           (dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) -
            //                            dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())));

            //        overallFuncMax = dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) + ySpaceGap;
            //        overallFuncMin = dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())) - ySpaceGap;

            //        xSpaceMin = ((dvScatterXSpace.Min((dvCurVector => dvCurVector.Min()))) < xSpaceMin)
            //            ? (dvScatterXSpace.Min((dvCurVector => dvCurVector.Min())))
            //            : (xSpaceMin);
            //        xSpaceMax = ((dvScatterXSpace.Max((dvCurVector => dvCurVector.Max()))) > xSpaceMax)
            //            ? (dvScatterXSpace.Max((dvCurVector => dvCurVector.Max())))
            //            : (xSpaceMax);
            //    }

            //}
            #endregion

            serviceSpaceGapX = Convert.ToInt32(0.05d * (double)pictureSize.Width);
            if (serviceSpaceGapX < 40) serviceSpaceGapX = 40;
            serviceSpaceGapY = Convert.ToInt32(0.05d * (double)pictureSize.Height);
            if (serviceSpaceGapY < 40) serviceSpaceGapY = 40;
            theImage = new Image<Bgr, Byte>(pictureSize);

            RepresentAxes();
            RepresentVectors();
        }




        private void RepresentAxes()
        {
            imgAxesLayer = theImage.CopyBlank();
            if (!presentAxes)
            {
                return;
            }

            Point ptYAxisStart = PicturePointFromSpacePointD(new PointD(0.0d, ptLeftTopSpaceCorner.Y));
            Point ptYAxisEnd = PicturePointFromSpacePointD(new PointD(0.0d, ptRightBottomSpaceCorner.Y));
            LineSegment2D yAxisLine = new LineSegment2D(ptYAxisStart, ptYAxisEnd);

            Point ptXAxisStart = PicturePointFromSpacePointD(new PointD(ptLeftTopSpaceCorner.X, 0.0d));
            Point ptXAxisEnd = PicturePointFromSpacePointD(new PointD(ptRightBottomSpaceCorner.X, 0.0d));
            LineSegment2D xAxisLine = new LineSegment2D(ptXAxisStart, ptXAxisEnd);

            imgAxesLayer.Draw(yAxisLine, colorGreen, 1);
            imgAxesLayer.Draw(xAxisLine, colorGreen, 1);
        }




        private void RepresentVectors()
        {
            imgVectorsLayer = TheImage.CopyBlank();

            for (int vectIdx = 0; vectIdx < listVectorsToDraw.Count; vectIdx++)
            {
                DenseVector currVector = listVectorsToDraw[vectIdx];
                
                DenseVector currVectorShift = DenseVector.OfEnumerable(new double[] {0.0d, 0.0d});
                try
                {
                    currVectorShift = listVectorsShift[vectIdx];
                }
                catch (Exception)
                {
                    ;
                    throw;
                }

                Bgr currVectorColor = colorWhite;
                try
                {
                    currVectorColor = listVectColors[vectIdx];
                }
                catch (Exception)
                {
                    ;
                    throw;
                }

                Point ptStart = PicturePointFromSpacePointD(new PointD(currVectorShift[0], currVectorShift[1]));
                Point ptEnd = PicturePointFromSpacePointD(new PointD(currVectorShift[0] + currVector[0], currVectorShift[1] + currVector[1]));
                LineSegment2D theVectorLine = new LineSegment2D(ptStart, ptEnd);

                imgVectorsLayer.Draw(theVectorLine, currVectorColor, 2);
            }
        }




        private Point PicturePointFromSpacePointD(PointD pt)
        {
            Point retPt = new Point();
            Size workingSpaceSize = pictureSize;
            workingSpaceSize.Width -= 2*serviceSpaceGapX;
            workingSpaceSize.Height -= 2 * serviceSpaceGapY;
            double koeffX = ((double) workingSpaceSize.Width)/(ptRightBottomSpaceCorner.X - ptLeftTopSpaceCorner.X);
            double koeffY = ((double) workingSpaceSize.Height)/(ptLeftTopSpaceCorner.Y - ptRightBottomSpaceCorner.Y);

            retPt.X = serviceSpaceGapX;
            retPt.X += Convert.ToInt32((pt.X - ptLeftTopSpaceCorner.X)*koeffX);

            retPt.Y = pictureSize.Height - serviceSpaceGapY;
            retPt.Y -= Convert.ToInt32((pt.Y - ptRightBottomSpaceCorner.Y)*koeffY);

            return retPt;
        }
    }
}
