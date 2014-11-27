using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SkyIndexAnalyzerLibraries
{
    public class SectionDescription
    {
        public PointD p1 = new PointD();
        public PointD p2 = new PointD();
        public bool fromMarginToMargin = true;

        private PointD p0 = new PointD();
        private DenseVector direction = DenseVector.Create(2, (i) => 0.0d);


        public SectionDescription()
        {
            
        }


        public SectionDescription(PointD in_p1, PointD in_p2, bool in_fromMarginToMargin)
        {
            p1 = in_p1;
            p2 = in_p2;
            fromMarginToMargin = in_fromMarginToMargin;

            p0 = in_p1;
            direction = DenseVector.Create(2, i =>
            {
                if (i == 0)// x
                    return
                        (p2.X - p1.X)/PointD.Distance(p1, p2);
                else if (i == 1)
                    return
                        (p2.Y - p1.Y)/PointD.Distance(p1, p2);
                else return 0.0d;
            });
        }


        public SectionDescription TransformTillMargins(Rectangle imageMarginsRect)
        {
            LineDescription lDesc = new LineDescription(p0, direction);
            DenseVector dvHoriz = DenseVector.Create(2, i => ((i == 0) ? (1.0d) : (0.0d)));
            DenseVector dvVert = DenseVector.Create(2, i => ((i == 0) ? (0.0d) : (1.0d)));
            //return new SectionDescription(p1, p2, fromMarginToMargin);
            LineDescription rectLtop = new LineDescription(new PointD(imageMarginsRect.Left, imageMarginsRect.Top),
                dvHoriz);
            LineDescription rectLleft = new LineDescription(new PointD(imageMarginsRect.Left, imageMarginsRect.Top),
                dvVert);
            LineDescription rectLright = new LineDescription(new PointD(imageMarginsRect.Right, imageMarginsRect.Top),
                dvVert);
            LineDescription rectLbottom = new LineDescription(new PointD(imageMarginsRect.Left, imageMarginsRect.Bottom),
                dvHoriz);

            List<PointD> ptCross = new List<PointD>();
            PointD pcTop = LineDescription.CrossPoint(rectLtop, lDesc);
            PointD pcBottom = LineDescription.CrossPoint(rectLbottom, lDesc);
            PointD pcLeft = LineDescription.CrossPoint(rectLleft, lDesc);
            PointD pcRight = LineDescription.CrossPoint(rectLright, lDesc);

            if ((pcTop.X >= imageMarginsRect.Left) && (pcTop.X <= imageMarginsRect.Right)) ptCross.Add(pcTop);
            if ((pcBottom.X >= imageMarginsRect.Left) && (pcBottom.X <= imageMarginsRect.Right)) ptCross.Add(pcBottom);
            if ((pcLeft.Y >= imageMarginsRect.Top) && (pcLeft.Y <= imageMarginsRect.Bottom)) ptCross.Add(pcLeft);
            if ((pcRight.Y >= imageMarginsRect.Top) && (pcRight.Y <= imageMarginsRect.Bottom)) ptCross.Add(pcRight);

            return new SectionDescription(ptCross[0], ptCross[1], true);
        }


        public LineDescription SectionLine
        {
            get { return new LineDescription(p0, direction); }
        }
    }





    public class LineDescription
    {
        public PointD p0 = new PointD();
        public DenseVector directionVector = DenseVector.Create(2, i => 0.0d);


        public double directionVectorLength
        {
            get { return Math.Sqrt(directionVector[0]*directionVector[0] + directionVector[1]*directionVector[1]); }
        }


        public LineDescription()
        {
            
        }


        public LineDescription(PointD _p0, DenseVector _direction)
        {
            p0 = _p0;
            directionVector = (DenseVector)_direction.Clone();
            SizeD sz1 = new SizeD(directionVector[0], directionVector[1]);
            if (PointD.Distance(p0, p0 + sz1) != 1.0d)
            {
                directionVector /= sz1.DiagonalLength;
            }
        }




        public static PointD CrossPoint(LineDescription l1, LineDescription l2)
        {
            PointD pc = new PointD();

            if ((l1.directionVector == l2.directionVector) || (l1.directionVector == -l2.directionVector))
            {
                return PointD.nullPointD();
            }

            //double xi = l2.p0.Y/l1.directionVector[1] + l1.p0.X/l1.directionVector[0] -
            //            l1.p0.Y/l1.directionVector[1] - l2.p0.X/l1.directionVector[0];
            //double d1 = l2.directionVector[0]/l1.directionVector[0] - l2.directionVector[1]/l1.directionVector[1];
            double d1 = l2.p0.Y*l1.directionVector[0] - l1.p0.Y*l1.directionVector[0] + l1.p0.X*l1.directionVector[1] -
                        l2.p0.X*l1.directionVector[1];
            double d2 = l1.directionVector[1]*l2.directionVector[0] - l1.directionVector[0]*l2.directionVector[1];
            double t2Cross = d1/d2;
            pc = l2.p0 + l2.directionVector*t2Cross;

            return pc;
        }
    }
}
