using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using Geometry;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SkyImagesAnalyzerLibraries
{
    public class SectionDescription
    {
        public PointD p1 = new PointD();
        public PointD p2 = new PointD();
        public bool fromMarginToMargin = true;

        private PointD p0 = new PointD();
        private Vector2D direction = new Vector2D();


        public SectionDescription()
        {
            
        }


        public SectionDescription(PointD in_p1, PointD in_p2, bool in_fromMarginToMargin)
        {
            p1 = in_p1;
            p2 = in_p2;
            fromMarginToMargin = in_fromMarginToMargin;

            p0 = in_p1;
            direction = new Vector2D(p1, p2);
        }





        //public SectionDescription(PointD in_p0, DenseVector in_direction, bool in_fromMarginToMargin)
        //{
        //    fromMarginToMargin = in_fromMarginToMargin;
        //    p0 = in_p0;
        //    direction = in_direction;
        //}




        public SectionDescription TransformTillMargins(Rectangle imageMarginsRect)
        {
            LineDescription2D lDesc = new LineDescription2D(p0, direction);
            Vector2D vHoriz = new Vector2D(1.0d, 0.0d); // DenseVector.Create(2, i => ((i == 0) ? (1.0d) : (0.0d)));
            Vector2D vVert = new Vector2D(0.0d, 1.0d); // DenseVector.Create(2, i => ((i == 0) ? (0.0d) : (1.0d)));
            //return new SectionDescription(p1, p2, fromMarginToMargin);
            LineDescription2D rectLtop = new LineDescription2D(new PointD(imageMarginsRect.Left, imageMarginsRect.Top),
                vHoriz);
            LineDescription2D rectLleft = new LineDescription2D(new PointD(imageMarginsRect.Left, imageMarginsRect.Top),
                vVert);
            LineDescription2D rectLright = new LineDescription2D(new PointD(imageMarginsRect.Right, imageMarginsRect.Top),
                vVert);
            LineDescription2D rectLbottom = new LineDescription2D(new PointD(imageMarginsRect.Left, imageMarginsRect.Bottom),
                vHoriz);

            List<PointD> ptCross = new List<PointD>();
            PointD pcTop = LineDescription2D.CrossPoint(rectLtop, lDesc);
            PointD pcBottom = LineDescription2D.CrossPoint(rectLbottom, lDesc);
            PointD pcLeft = LineDescription2D.CrossPoint(rectLleft, lDesc);
            PointD pcRight = LineDescription2D.CrossPoint(rectLright, lDesc);

            if ((pcTop.X >= imageMarginsRect.Left) && (pcTop.X <= imageMarginsRect.Right)) ptCross.Add(pcTop);
            if ((pcBottom.X >= imageMarginsRect.Left) && (pcBottom.X <= imageMarginsRect.Right)) ptCross.Add(pcBottom);
            if ((pcLeft.Y >= imageMarginsRect.Top) && (pcLeft.Y <= imageMarginsRect.Bottom)) ptCross.Add(pcLeft);
            if ((pcRight.Y >= imageMarginsRect.Top) && (pcRight.Y <= imageMarginsRect.Bottom)) ptCross.Add(pcRight);

            return new SectionDescription(ptCross[0], ptCross[1], true);
        }


        public LineDescription2D SectionLine
        {
            get { return new LineDescription2D(p0, direction); }
        }
    }





    
}
