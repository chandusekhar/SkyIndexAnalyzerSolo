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

namespace SkyImagesAnalyzerLibraries
{
    public class MultipleScatterAndFunctionsRepresentation
    {
        public List<Func<DenseVector, double, double>> theRepresentingFunctions = new List<Func<DenseVector, double, double>>();
        public List<DenseVector> parameters = new List<DenseVector>();
        public List<DenseVector> dvScatterXSpace = new List<DenseVector>();
        public List<DenseVector> dvScatterFuncValues = new List<DenseVector>();
        public List<Bgr> lineColors = new List<Bgr>();
        public List<Bgr> scatterLineColors = new List<Bgr>();
        public List<SequencesDrawingVariants> scatterDrawingVariants = new List<SequencesDrawingVariants>();
        public double xSpaceMin = 0.0d;
        public double xSpaceMax = 1.0d;

        public string yAxisNote = "Y, px";
        public string xAxisNote = "X, px";

        public double overallFuncMax = 1.0d;
        public double overallFuncMin = 0.0d;
        int pictureWidth = 800;
        int pictureHeight = 600;
        int serviceSpaceGapX = 40;
        int serviceSpaceGapY = 40;
        private Bgr colorBlack = new Bgr(0, 0, 0);
        private Bgr colorWhite = new Bgr(255, 255, 255);
        private Bgr colorGreen = new Bgr(Color.LightGreen);
        public Image<Bgr, Byte> theImage = null;

        public List<double> verticalMarkersList = new List<double>();
        public List<int> verticalMarkersIndexesUsingXSpace = new List<int>();
        private Image<Bgr, byte> markersLayerImage = null;

        public bool equalScale = false;
        public bool fixSpecifiedMargins = false;
        public bool drawZeroLines = false;

        public double koeffX = 1.0d;
        public double koeffY = 1.0d;


        #region readonly fields to read properties
        public int ServiceSpaceGapX
        {
            get { return serviceSpaceGapX; }
        }

        public int ServiceSpaceGapY
        {
            get { return serviceSpaceGapY; }
        }

        public int PictureWidth
        {
            get { return pictureWidth; }
        }

        public int PictureHeight
        {
            get { return pictureHeight; }
        }

        #endregion readonly fields to read properties



        public MultipleScatterAndFunctionsRepresentation Copy()
        {
            MultipleScatterAndFunctionsRepresentation newCopy =
                new MultipleScatterAndFunctionsRepresentation(pictureWidth, pictureHeight);
            newCopy.theRepresentingFunctions =
                new List<System.Func<DenseVector, double, double>>(theRepresentingFunctions);
            newCopy.parameters = new System.Collections.Generic.List<DenseVector>(parameters);
            newCopy.dvScatterXSpace = new System.Collections.Generic.List<DenseVector>(dvScatterXSpace);
            newCopy.dvScatterFuncValues = new System.Collections.Generic.List<DenseVector>(dvScatterFuncValues);
            newCopy.lineColors = new System.Collections.Generic.List<Bgr>(lineColors);
            newCopy.scatterLineColors = new System.Collections.Generic.List<Bgr>(scatterLineColors);
            newCopy.scatterDrawingVariants = new List<SequencesDrawingVariants>(scatterDrawingVariants);
            newCopy.xSpaceMin = xSpaceMin;
            newCopy.xSpaceMax = xSpaceMax;
            newCopy.yAxisNote = yAxisNote;
            newCopy.xAxisNote = xAxisNote;
            newCopy.overallFuncMax = overallFuncMax;
            newCopy.overallFuncMin = overallFuncMin;
            newCopy.verticalMarkersList = new System.Collections.Generic.List<double>(verticalMarkersList);
            newCopy.verticalMarkersIndexesUsingXSpace =
                new System.Collections.Generic.List<int>(verticalMarkersIndexesUsingXSpace);
            newCopy.equalScale = equalScale;
            newCopy.fixSpecifiedMargins = fixSpecifiedMargins;
            newCopy.drawZeroLines = drawZeroLines;

            newCopy.Represent();
            return newCopy;
        }



        public MultipleScatterAndFunctionsRepresentation(Size imageSize)
        {
            pictureWidth = imageSize.Width;
            pictureHeight = imageSize.Height;
        }

        public MultipleScatterAndFunctionsRepresentation(int imageWidth, int imageHeight)
        {
            pictureWidth = imageWidth;
            pictureHeight = imageHeight;
        }

        public Image<Bgr, byte> TheImage
        {
            get
            {
                if (markersLayerImage != null)
                {
                    Image<Bgr, byte> retImage = theImage.Copy();
                    retImage = retImage.Add(markersLayerImage);
                    return retImage;
                }
                else return theImage;
            }
        }


        public void ResizeCanvas(Size newSize)
        {
            pictureWidth = newSize.Width;
            pictureHeight = newSize.Height;
            Represent();
        }

        public void ResizeCanvas(int newWidth, int newHeight)
        {
            pictureWidth = newWidth;
            pictureHeight = newHeight;
            Represent();
        }


        public void Represent()
        {
            if ((dvScatterXSpace != null) && (dvScatterFuncValues != null))
            {
                if (theRepresentingFunctions.Count > 0)
                {

                    double ySpaceGap = 0.2d *
                                       (dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) -
                                        dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())));

                    if (!fixSpecifiedMargins)
                    {
                        overallFuncMax = ((dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) + ySpaceGap) >
                                          overallFuncMax)
                            ? (dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) + ySpaceGap)
                            : (overallFuncMax);
                        overallFuncMin = ((dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())) - ySpaceGap) <
                                          overallFuncMin)
                            ? (dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())) - ySpaceGap)
                            : (overallFuncMin);

                        xSpaceMin = ((dvScatterXSpace.Min((dvCurVector => dvCurVector.Min()))) < xSpaceMin)
                            ? (dvScatterXSpace.Min((dvCurVector => dvCurVector.Min())))
                            : (xSpaceMin);
                        xSpaceMax = ((dvScatterXSpace.Max((dvCurVector => dvCurVector.Max()))) > xSpaceMax)
                            ? (dvScatterXSpace.Max((dvCurVector => dvCurVector.Max())))
                            : (xSpaceMax);
                    }
                }
                else
                {
                    double ySpaceGap = 0.2d *
                                       (dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) -
                                        dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())));

                    if (!fixSpecifiedMargins)
                    {
                        overallFuncMax = dvScatterFuncValues.Max((dvCurVector => dvCurVector.Max())) + ySpaceGap;
                        overallFuncMin = dvScatterFuncValues.Min((dvCurVector => dvCurVector.Min())) - ySpaceGap;

                        xSpaceMin = ((dvScatterXSpace.Min((dvCurVector => dvCurVector.Min()))) < xSpaceMin)
                            ? (dvScatterXSpace.Min((dvCurVector => dvCurVector.Min())))
                            : (xSpaceMin);
                        xSpaceMax = ((dvScatterXSpace.Max((dvCurVector => dvCurVector.Max()))) > xSpaceMax)
                            ? (dvScatterXSpace.Max((dvCurVector => dvCurVector.Max())))
                            : (xSpaceMax);
                    }
                }

            }

            serviceSpaceGapX = Convert.ToInt32(0.05d * (double)pictureWidth);
            if (serviceSpaceGapX < 40) serviceSpaceGapX = 40;
            serviceSpaceGapY = Convert.ToInt32(0.05d * (double)pictureHeight);
            if (serviceSpaceGapY < 40) serviceSpaceGapY = 40;
            theImage = new Image<Bgr, Byte>(pictureWidth, pictureHeight, new Bgr(0, 0, 0));




            RepresentScatter();
            RepresentAnalytic();

            RepresentMarkers();
        }


        private void RepresentAnalytic()
        {
            int xValuesCount = pictureWidth - (2 * serviceSpaceGapX);// оставляем место на шкалу Y

            List<Point> rulerVertices = new List<Point>();
            rulerVertices.Add(new Point(serviceSpaceGapX, pictureHeight - serviceSpaceGapY));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGapX, pictureHeight - serviceSpaceGapY));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGapX, serviceSpaceGapY));
            rulerVertices.Add(new Point(serviceSpaceGapX, serviceSpaceGapY));
            theImage.DrawPolyline(rulerVertices.ToArray(), true, colorGreen, 2);

            double koeff = (pictureHeight - (2 * serviceSpaceGapY)) / (overallFuncMax - overallFuncMin);
            koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGapY) / (overallFuncMax - overallFuncMin);
            koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGapX) / (xSpaceMax - xSpaceMin);
            if (equalScale)
            {
                koeff = Math.Min(koeffY, koeffX);
                koeffY = koeff;
                koeffX = koeff;
            }
            

            if (drawZeroLines)
            {
                int zeroYcoordinate = Convert.ToInt32(pictureHeight - serviceSpaceGapY - (0 - overallFuncMin) * koeffY);
                int zeroXcoordinate = Convert.ToInt32(serviceSpaceGapX + (0 - xSpaceMin) * koeffX);

                List<Point> rulerXVertices = new List<Point>();
                rulerXVertices.Add(new Point(serviceSpaceGapX, zeroYcoordinate));
                rulerXVertices.Add(new Point(pictureWidth - serviceSpaceGapX, zeroYcoordinate));
                theImage.DrawPolyline(rulerXVertices.ToArray(), false, colorGreen, 2);

                List<Point> rulerYVertices = new List<Point>();
                rulerYVertices.Add(new Point(zeroXcoordinate, serviceSpaceGapY));
                rulerYVertices.Add(new Point(zeroXcoordinate, pictureHeight - serviceSpaceGapY));
                theImage.DrawPolyline(rulerYVertices.ToArray(), false, colorGreen, 2);
            }



            DenseVector dvXSpaceValues = DenseVector.Create(xValuesCount, new Func<int, double>(i => xSpaceMin + ((double)i / ((double)xValuesCount - 1.0d)) * (xSpaceMax - xSpaceMin)));
            DenseVector parametersList = null;

            for (int i = 0; i < theRepresentingFunctions.Count; i++)
            {
                Func<DenseVector, double, double> theRepresentingFunction = theRepresentingFunctions[i];
                DenseVector currentParametersList = parameters[i];

                DenseVector dvFuncValues = DenseVector.Create(xValuesCount, new Func<int, double>(j => theRepresentingFunction(currentParametersList, dvXSpaceValues[j])));
                double funcMax = dvFuncValues.Max();
                double funcMin = dvFuncValues.Min();
                overallFuncMax = (funcMax > overallFuncMax) ? (funcMax) : (overallFuncMax);
                overallFuncMin = (funcMin < overallFuncMin) ? (funcMin) : (overallFuncMin);
            }



            #region Прописываем текстовые маркеры
            MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);
            theFont.thickness = 1;
            double dMarkersCount = (double)(pictureHeight - (2 * serviceSpaceGapY)) / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            double dRulerValueGap = (overallFuncMax - overallFuncMin) / (double)dMarkersCount;
            //dRulerValueGap = (dRulerValueGap < 1.0d) ? (1.0d) : dRulerValueGap;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = overallFuncMin - Math.IEEERemainder(overallFuncMin, rulerValueGap);
            lowerMarkerValue = (lowerMarkerValue < overallFuncMin) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);
            double currentMarkerValue = lowerMarkerValue;
            double nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGapY) + serviceSpaceGapY;
            while (nextYPositionDouble > serviceSpaceGapY)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGapY) + serviceSpaceGapY;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(serviceSpaceGapX, yPosition), new Point(serviceSpaceGapX - 5, yPosition));
                Bgr markerColor = colorGreen;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValue.ToString();
                theImage.Draw(message, ref theFont, new Point(2, yPosition), markerColor);
                currentMarkerValue += rulerValueGap;
                nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGapY) + serviceSpaceGapY;
            }

            double dMarkersCountX = (double)(pictureWidth - (2 * serviceSpaceGapX)) / 30.0d;
            dMarkersCountX = (dMarkersCountX > 10.0d) ? (10.0d) : (dMarkersCountX);
            double dRulerValueGapX = (xSpaceMax - xSpaceMin) / (double)dMarkersCountX;
            int deciGapX = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGapX, 2.0d)));
            double rulerValueGapX = Math.Pow(2.0, (double)deciGapX);
            double lowerMarkerValueX = xSpaceMin - Math.IEEERemainder(xSpaceMin, rulerValueGapX);
            lowerMarkerValueX = (lowerMarkerValueX < xSpaceMin) ? (lowerMarkerValueX + rulerValueGapX) : (lowerMarkerValueX);
            double currentMarkerValueX = lowerMarkerValueX;
            double nextXPositionDouble = serviceSpaceGapX + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGapX);
            while (nextXPositionDouble < pictureWidth - serviceSpaceGapX)
            {
                double xPositionDouble = serviceSpaceGapX + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGapX);
                int xPosition = Convert.ToInt32(Math.Round(xPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(xPosition, pictureHeight - serviceSpaceGapY), new Point(xPosition, pictureHeight - serviceSpaceGapY + 5));
                Bgr markerColor = colorGreen;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValueX.ToString();
                theImage.Draw(message, ref theFont, new Point(xPosition, pictureHeight - 10), markerColor);
                currentMarkerValueX += rulerValueGapX;
                nextXPositionDouble = serviceSpaceGapX + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGapX);
            }

            #endregion Прописываем текстовые маркеры




            

            for (int i = 0; i < theRepresentingFunctions.Count; i++)
            {
                Func<DenseVector, double, double> theRepresentingFunction = theRepresentingFunctions[i];
                DenseVector currentParametersList = parameters[i];

                //DenseVector dvXSpaceValues = DenseVector.Create(xValuesCount, new Func<int, double>(i => xSpaceMin + ((double)i / ((double)xValuesCount - 1.0d)) * (xSpaceMax - xSpaceMin)));
                parametersList = null;
                DenseVector dvFuncValues = DenseVector.Create(xValuesCount, new Func<int, double>(j => theRepresentingFunction(currentParametersList, dvXSpaceValues[j])));
                Bgr currentLineColor = lineColors[i];


                DenseVector xCoordinates = DenseVector.Create(xValuesCount, new Func<int, double>(j => ((double)serviceSpaceGapX + j)));
                DenseVector yCoordinates = DenseVector.Create(xValuesCount, new Func<int, double>(j =>
                {
                    double pixValue = koeff * (dvFuncValues[j] - overallFuncMin);
                    return (pictureHeight - serviceSpaceGapY - pixValue);
                }));

                List<Point> funcRepresentationPoints = new List<Point>();
                for (int j = 0; j < xValuesCount; j++)
                {
                    if (double.IsNaN(yCoordinates[j])) continue;
                    funcRepresentationPoints.Add(new Point(Convert.ToInt32(xCoordinates[j]), Convert.ToInt32(yCoordinates[j])));
                }
                theImage.DrawPolyline(funcRepresentationPoints.ToArray(), false, currentLineColor, 2);
            }
        }








        private void RepresentScatter()
        {
            if ((dvScatterXSpace.Count == 0) || (dvScatterFuncValues.Count == 0)) return;

            koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGapY) / (overallFuncMax - overallFuncMin);
            koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGapX) / (xSpaceMax - xSpaceMin);
            if (equalScale)
            {
                double koeff = Math.Min(koeffY, koeffX);
                koeffY = koeff;
                koeffX = koeff;
            }


            for (int seqIndex = 0; seqIndex < dvScatterFuncValues.Count; seqIndex++)
            {
                List<Point> pointsList = new List<Point>();
                for (int i = 0; i < dvScatterXSpace[seqIndex].Count; i++)
                {
                    int yCoordinate = Convert.ToInt32(pictureHeight - serviceSpaceGapY - (dvScatterFuncValues[seqIndex][i] - overallFuncMin) * koeffY);
                    int xCoordinate = Convert.ToInt32(serviceSpaceGapX + (dvScatterXSpace[seqIndex][i] - xSpaceMin) * koeffX);
                    pointsList.Add(new Point(xCoordinate, yCoordinate));
                }

                Bgr curSeqColor = new Bgr(255, 0, 0);
                try
                {
                    curSeqColor = scatterLineColors[seqIndex];
                }
                catch (Exception)
                {
                    curSeqColor = new Bgr(255, 0, 0);
                }

                SequencesDrawingVariants currentDrawingVariant = SequencesDrawingVariants.polyline;
                try
                {
                    currentDrawingVariant = scatterDrawingVariants[seqIndex];
                }
                catch (Exception)
                {
                    currentDrawingVariant = SequencesDrawingVariants.polyline;
                }

                switch (currentDrawingVariant)
                {
                    case SequencesDrawingVariants.circles:
                        {
                            foreach (Point thePoint in pointsList)
                            {
                                theImage.Draw(new CircleF(thePoint, 3), curSeqColor, 2);
                            }
                            break;
                        }
                    case SequencesDrawingVariants.squares:
                        {
                            foreach (Point thePoint in pointsList)
                            {
                                List<Point> squareVertices = new List<Point>();
                                squareVertices.Add(new Point(thePoint.X - 2, thePoint.Y - 2));
                                squareVertices.Add(new Point(thePoint.X - 2, thePoint.Y + 2));
                                squareVertices.Add(new Point(thePoint.X + 2, thePoint.Y + 2));
                                squareVertices.Add(new Point(thePoint.X + 2, thePoint.Y - 2));
                                theImage.DrawPolyline(squareVertices.ToArray(), true, curSeqColor, 2);
                            }
                            break;
                        }
                    case SequencesDrawingVariants.triangles:
                        {
                            foreach (Point thePoint in pointsList)
                            {
                                List<Point> triangleVertices = new List<Point>();
                                triangleVertices.Add(new Point(thePoint.X - 2, thePoint.Y - 2));
                                triangleVertices.Add(new Point(thePoint.X, thePoint.Y + 2));
                                triangleVertices.Add(new Point(thePoint.X + 2, thePoint.Y - 2));
                                theImage.DrawPolyline(triangleVertices.ToArray(), true, curSeqColor, 2);
                            }
                            break;
                        }
                    case SequencesDrawingVariants.polyline:
                        {
                            theImage.DrawPolyline(pointsList.ToArray(), false, curSeqColor, 2);
                            break;
                        }
                }


            }

        }



        public void RepresentMarkers()
        {
            //double koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGap) / (overallFuncMax - overallFuncMin);
            markersLayerImage = theImage.CopyBlank();


            koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGapX) / (xSpaceMax - xSpaceMin);
            Bgr curSeqColor = new Bgr(0, 0, 255);

            foreach (double dMarkerValue in verticalMarkersList)
            {
                int xCoordinate = Convert.ToInt32(serviceSpaceGapX + (dMarkerValue - xSpaceMin) * koeffX);
                Point p1 = new Point(xCoordinate, pictureHeight - serviceSpaceGapY);
                Point p2 = new Point(xCoordinate, serviceSpaceGapY);
                markersLayerImage.Draw(new LineSegment2D(p1, p2), curSeqColor, 1);
            }
        }






        public void SaveToImage(string filename = "", bool absolutePath = true)
        {
            if (filename == "") return;

            theImage.Save(filename);
        }


        public PointD GetDataCoordinatesOfMouseClick(PictureBox sender, MouseEventArgs e)
        {
            if (sender.Image == null)
            {
                return PointD.Empty;
            }

            int mouseClickX = e.X;
            int mouseClickY = e.Y;

            //вернуть координату (по данным) X - часто это время
            SizeD workingSpaceOfTheImage = new SizeD(theImage.Width - 2.0d * (double)serviceSpaceGapX,
                theImage.Height - 2.0d * (double)serviceSpaceGapY);
            double xKoeff = (xSpaceMax - xSpaceMin) / workingSpaceOfTheImage.Width;
            double imageDataX = xKoeff * ((double)mouseClickX - (double)serviceSpaceGapX) + xSpaceMin;

            double yKoeff = (overallFuncMax - overallFuncMin) / workingSpaceOfTheImage.Height;
            double imageDataY = yKoeff * (workingSpaceOfTheImage.Height - (double)mouseClickY + (double)serviceSpaceGapY) +
                                overallFuncMin;

            return new PointD(imageDataX, imageDataY);
        }
    }
}
