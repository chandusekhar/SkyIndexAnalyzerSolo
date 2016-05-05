using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
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

        public Func<double, string> xAxisValuesConversionToRepresentTicksValues = null;
        public Func<double, string> yAxisValuesConversionToRepresentTicksValues = null;

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
        int leftServiceSpaceGapX = 40;
        int rightServiceSpaceGapX = 40;
        int topServiceSpaceGapY = 40;
        int btmServiceSpaceGapY = 40;
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
        public int LeftServiceSpaceGapX
        {
            get { return leftServiceSpaceGapX; }
        }


        public int RightServiceSpaceGapX
        {
            get { return rightServiceSpaceGapX; }
        }


        public int TopServiceSpaceGapY
        {
            get { return topServiceSpaceGapY; }
        }


        public int BtmServiceSpaceGapY
        {
            get { return btmServiceSpaceGapY; }
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
                new List<Func<DenseVector, double, double>>(theRepresentingFunctions);
            newCopy.parameters = new List<DenseVector>(parameters);
            newCopy.dvScatterXSpace = new List<DenseVector>(dvScatterXSpace);
            newCopy.dvScatterFuncValues = new List<DenseVector>(dvScatterFuncValues);
            newCopy.lineColors = new List<Bgr>(lineColors);
            newCopy.scatterLineColors = new List<Bgr>(scatterLineColors);
            newCopy.scatterDrawingVariants = new List<SequencesDrawingVariants>(scatterDrawingVariants);
            newCopy.xSpaceMin = xSpaceMin;
            newCopy.xSpaceMax = xSpaceMax;
            newCopy.yAxisNote = yAxisNote;
            newCopy.xAxisNote = xAxisNote;
            newCopy.overallFuncMax = overallFuncMax;
            newCopy.overallFuncMin = overallFuncMin;
            newCopy.verticalMarkersList = new List<double>(verticalMarkersList);
            newCopy.verticalMarkersIndexesUsingXSpace =
                new List<int>(verticalMarkersIndexesUsingXSpace);
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

            leftServiceSpaceGapX = Convert.ToInt32(0.05d * (double)pictureWidth);
            if (leftServiceSpaceGapX < 40) leftServiceSpaceGapX = 40;
            rightServiceSpaceGapX = leftServiceSpaceGapX;

            topServiceSpaceGapY = Convert.ToInt32(0.05d * (double)pictureHeight);
            if (topServiceSpaceGapY < 40) topServiceSpaceGapY = 40;
            theImage = new Image<Bgr, Byte>(pictureWidth, pictureHeight, new Bgr(0, 0, 0));




            
            RepresentAnalytic();

            RepresentScatter();

            RepresentMarkers();
        }


        private void RepresentAnalytic()
        {

            #region Прописываем текстовые маркеры

            #region Y

            //MCvFont theFont = new MCvFont(FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);
            //theFont.thickness = 1;

            // замерим высоту подписей по X по значению в минимуме

            string strMinXvalueMarker = (xAxisValuesConversionToRepresentTicksValues == null)?(xSpaceMin.ToString()) :(xAxisValuesConversionToRepresentTicksValues(xSpaceMin));
            TextBarImage minXvalueMarkerTextBar = new TextBarImage(strMinXvalueMarker,
                new Image<Bgr, byte>(new Size(pictureWidth, pictureHeight)));
            int barHeight = minXvalueMarkerTextBar.textBarSize.Height;
            if (btmServiceSpaceGapY < 1.5 * barHeight)
            {
                btmServiceSpaceGapY = Convert.ToInt32(1.5*barHeight);
            }


            double dMarkersCount = (double)(pictureHeight - (btmServiceSpaceGapY + topServiceSpaceGapY)) / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            double dRulerValueGap = (overallFuncMax - overallFuncMin) / (double)dMarkersCount;
            //dRulerValueGap = (dRulerValueGap < 1.0d) ? (1.0d) : dRulerValueGap;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = overallFuncMin - overallFuncMin%rulerValueGap;// Math.IEEERemainder(overallFuncMin, rulerValueGap);
            lowerMarkerValue = (lowerMarkerValue < overallFuncMin) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);

            List<double> yMarkersValues = new List<double>();
            yMarkersValues.Add(lowerMarkerValue);
            double currentMarkerValue = lowerMarkerValue;
            while (currentMarkerValue <= overallFuncMax)
            {
                currentMarkerValue += rulerValueGap;
                yMarkersValues.Add(currentMarkerValue);
            }

            List<TextBarImage> lTextBars = new List<TextBarImage>();
            foreach (double markerValue in yMarkersValues)
            {
                string currMarkerPresentation = markerValue.ToString();
                if (yAxisValuesConversionToRepresentTicksValues != null)
                {
                    currMarkerPresentation = yAxisValuesConversionToRepresentTicksValues(markerValue);
                }

                TextBarImage currTextBar = new TextBarImage(currMarkerPresentation,
                    new Image<Bgr, byte>(new Size(pictureWidth, pictureHeight)));
                lTextBars.Add(currTextBar);
            }
            int maxYlabelWidth = lTextBars.Max(textBar => textBar.textBarSize.Width);
            if (leftServiceSpaceGapX < maxYlabelWidth)
            {
                leftServiceSpaceGapX = maxYlabelWidth;
            }




            currentMarkerValue = lowerMarkerValue;
            double nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - topServiceSpaceGapY - btmServiceSpaceGapY) + topServiceSpaceGapY;
            while (nextYPositionDouble > topServiceSpaceGapY)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - topServiceSpaceGapY - btmServiceSpaceGapY) + topServiceSpaceGapY;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(leftServiceSpaceGapX, yPosition), new Point(leftServiceSpaceGapX - 5, yPosition));
                Bgr markerColor = colorGreen;
                theImage.Draw(theLine, markerColor, 2);

                string currMarkerPresentation = currentMarkerValue.ToString();
                if (yAxisValuesConversionToRepresentTicksValues != null)
                {
                    currMarkerPresentation = yAxisValuesConversionToRepresentTicksValues(currentMarkerValue);
                }

                //theImage.Draw(currMarkerPresentation, ref theFont, new Point(2, yPosition), markerColor);

                TextBarImage currSignImage = new TextBarImage(currMarkerPresentation, theImage);
                currSignImage.PtSurroundingBarStart = new Point(0, yPosition - currSignImage.textHeight);
                theImage = theImage.Add(currSignImage.TextSignImageAtOriginalBlank(markerColor));


                currentMarkerValue += rulerValueGap;
                nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - topServiceSpaceGapY - btmServiceSpaceGapY) + topServiceSpaceGapY;
            }

            #endregion Y
            


            #region X

            double rulerValueGapX = 0.0d;
            double lowerMarkerValueX = 0.0d;
            bool markersCountRight = false;
            int initialDivider = 30;
            double dMarkersCountX = (double)(pictureWidth - (leftServiceSpaceGapX + rightServiceSpaceGapX)) / (double)initialDivider;
            while (!markersCountRight)
            {
                dMarkersCountX = (dMarkersCountX > 10.0d) ? (10.0d) : (dMarkersCountX);
                double dRulerValueGapX = (xSpaceMax - xSpaceMin) / (double)dMarkersCountX;
                //int deciGapX = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGapX, 2.0d)));
                //rulerValueGapX = Math.Pow(2.0, (double)deciGapX);
                rulerValueGapX = dRulerValueGapX;
                lowerMarkerValueX = xSpaceMin - (xSpaceMin % rulerValueGapX); // Math.IEEERemainder(xSpaceMin, rulerValueGapX);
                lowerMarkerValueX = (lowerMarkerValueX < xSpaceMin) ? (lowerMarkerValueX + rulerValueGapX) : (lowerMarkerValueX);
                
                double firstMarkerValueX = lowerMarkerValueX;
                List<double> xMarkersValues = new List<double>();
                xMarkersValues.Add(firstMarkerValueX);
                currentMarkerValue = firstMarkerValueX;
                while (currentMarkerValue <= xSpaceMax)
                {
                    currentMarkerValue += rulerValueGapX;
                    xMarkersValues.Add(currentMarkerValue);
                }

                List<TextBarImage> lTextBarsXaxis = new List<TextBarImage>();
                foreach (double markerValue in xMarkersValues)
                {
                    string currMarkerPresentation = markerValue.ToString();
                    if (xAxisValuesConversionToRepresentTicksValues != null)
                    {
                        currMarkerPresentation = xAxisValuesConversionToRepresentTicksValues(markerValue);
                    }

                    TextBarImage currTextBar = new TextBarImage(currMarkerPresentation,
                        new Image<Bgr, byte>(new Size(pictureWidth, pictureHeight)));
                    lTextBarsXaxis.Add(currTextBar);
                }

                int totalTextBarsWidth = lTextBarsXaxis.Sum(textBar => textBar.textBarSize.Width);
                if (totalTextBarsWidth > pictureWidth - leftServiceSpaceGapX - rightServiceSpaceGapX)
                {
                    dMarkersCountX = dMarkersCountX - 1.0d;
                    if (dMarkersCountX <=2)
                    {
                        dMarkersCountX = 2.0d;
                        markersCountRight = true;
                    }
                }
                else
                {
                    markersCountRight = true;
                }
            }


            double currentMarkerValueX = lowerMarkerValueX;

            double nextXPositionDouble = leftServiceSpaceGapX + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - leftServiceSpaceGapX - rightServiceSpaceGapX);
            while (nextXPositionDouble <= pictureWidth - rightServiceSpaceGapX)
            {
                double xPositionDouble = leftServiceSpaceGapX + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - leftServiceSpaceGapX - rightServiceSpaceGapX);
                int xPosition = Convert.ToInt32(Math.Round(xPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(xPosition, pictureHeight - btmServiceSpaceGapY), new Point(xPosition, pictureHeight - btmServiceSpaceGapY + 5));
                Bgr markerColor = colorGreen;
                theImage.Draw(theLine, markerColor, 2);

                string currMarkerPresentation = currentMarkerValueX.ToString();
                if (xAxisValuesConversionToRepresentTicksValues != null)
                {
                    currMarkerPresentation = xAxisValuesConversionToRepresentTicksValues(currentMarkerValueX);
                }

                TextBarImage currSignImage = new TextBarImage(currMarkerPresentation, theImage);
                currSignImage.PtSurroundingBarStart = new Point(Convert.ToInt32(xPosition - currSignImage.textBarSize.Width / 2), pictureHeight - btmServiceSpaceGapY + 10);
                theImage = theImage.Add(currSignImage.TextSignImageAtOriginalBlank(markerColor));

                currentMarkerValueX += rulerValueGapX;
                nextXPositionDouble = leftServiceSpaceGapX + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - leftServiceSpaceGapX - rightServiceSpaceGapX);
            }

            #endregion X
            #endregion Прописываем текстовые маркеры







            int xValuesCount = pictureWidth - leftServiceSpaceGapX - rightServiceSpaceGapX;// оставляем место на шкалу Y

            List<Point> rulerVertices = new List<Point>();
            rulerVertices.Add(new Point(leftServiceSpaceGapX, pictureHeight - btmServiceSpaceGapY));
            rulerVertices.Add(new Point(pictureWidth - rightServiceSpaceGapX, pictureHeight - btmServiceSpaceGapY));
            rulerVertices.Add(new Point(pictureWidth - rightServiceSpaceGapX, topServiceSpaceGapY));
            rulerVertices.Add(new Point(leftServiceSpaceGapX, topServiceSpaceGapY));
            theImage.DrawPolyline(rulerVertices.ToArray(), true, colorGreen, 2);

            double koeff = (pictureHeight - btmServiceSpaceGapY - topServiceSpaceGapY) / (overallFuncMax - overallFuncMin);
            koeffY = ((double)pictureHeight - btmServiceSpaceGapY - topServiceSpaceGapY) / (overallFuncMax - overallFuncMin);
            koeffX = ((double)pictureWidth - leftServiceSpaceGapX - rightServiceSpaceGapX) / (xSpaceMax - xSpaceMin);
            if (equalScale)
            {
                koeff = Math.Min(koeffY, koeffX);
                koeffY = koeff;
                koeffX = koeff;
            }
            

            if (drawZeroLines)
            {
                int zeroYcoordinate = Convert.ToInt32(pictureHeight - btmServiceSpaceGapY - (0 - overallFuncMin) * koeffY);
                int zeroXcoordinate = Convert.ToInt32(leftServiceSpaceGapX + (0 - xSpaceMin) * koeffX);

                List<Point> rulerXVertices = new List<Point>();
                rulerXVertices.Add(new Point(leftServiceSpaceGapX, zeroYcoordinate));
                rulerXVertices.Add(new Point(pictureWidth - rightServiceSpaceGapX, zeroYcoordinate));
                theImage.DrawPolyline(rulerXVertices.ToArray(), false, colorGreen, 2);

                List<Point> rulerYVertices = new List<Point>();
                rulerYVertices.Add(new Point(zeroXcoordinate, topServiceSpaceGapY));
                rulerYVertices.Add(new Point(zeroXcoordinate, pictureHeight - btmServiceSpaceGapY));
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








            for (int i = 0; i < theRepresentingFunctions.Count; i++)
            {
                Func<DenseVector, double, double> theRepresentingFunction = theRepresentingFunctions[i];
                DenseVector currentParametersList = parameters[i];

                //DenseVector dvXSpaceValues = DenseVector.Create(xValuesCount, new Func<int, double>(i => xSpaceMin + ((double)i / ((double)xValuesCount - 1.0d)) * (xSpaceMax - xSpaceMin)));
                parametersList = null;
                DenseVector dvFuncValues = DenseVector.Create(xValuesCount, new Func<int, double>(j => theRepresentingFunction(currentParametersList, dvXSpaceValues[j])));
                Bgr currentLineColor = lineColors[i];


                DenseVector xCoordinates = DenseVector.Create(xValuesCount, new Func<int, double>(j => ((double)leftServiceSpaceGapX + j)));
                DenseVector yCoordinates = DenseVector.Create(xValuesCount, new Func<int, double>(j =>
                {
                    double pixValue = koeff * (dvFuncValues[j] - overallFuncMin);
                    return (pictureHeight - btmServiceSpaceGapY - pixValue);
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

            koeffY = ((double)pictureHeight - btmServiceSpaceGapY - topServiceSpaceGapY) / (overallFuncMax - overallFuncMin);
            koeffX = ((double)pictureWidth - leftServiceSpaceGapX - rightServiceSpaceGapX) / (xSpaceMax - xSpaceMin);
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
                    int yCoordinate = Convert.ToInt32(pictureHeight - btmServiceSpaceGapY - (dvScatterFuncValues[seqIndex][i] - overallFuncMin) * koeffY);
                    int xCoordinate = Convert.ToInt32(leftServiceSpaceGapX + (dvScatterXSpace[seqIndex][i] - xSpaceMin) * koeffX);
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


            koeffX = ((double)pictureWidth - leftServiceSpaceGapX - rightServiceSpaceGapX) / (xSpaceMax - xSpaceMin);
            Bgr curSeqColor = new Bgr(0, 0, 255);

            foreach (double dMarkerValue in verticalMarkersList)
            {
                int xCoordinate = Convert.ToInt32(leftServiceSpaceGapX + (dMarkerValue - xSpaceMin) * koeffX);
                Point p1 = new Point(xCoordinate, pictureHeight - btmServiceSpaceGapY);
                Point p2 = new Point(xCoordinate, topServiceSpaceGapY);
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
            SizeD workingSpaceOfTheImage = new SizeD(theImage.Width - leftServiceSpaceGapX - rightServiceSpaceGapX,
                theImage.Height - btmServiceSpaceGapY - topServiceSpaceGapY);
            double xKoeff = (xSpaceMax - xSpaceMin) / workingSpaceOfTheImage.Width;
            double imageDataX = xKoeff * ((double)mouseClickX - (double)leftServiceSpaceGapX) + xSpaceMin;

            double yKoeff = (overallFuncMax - overallFuncMin) / workingSpaceOfTheImage.Height;
            double imageDataY = yKoeff * (workingSpaceOfTheImage.Height - (double)mouseClickY + (double)topServiceSpaceGapY) +
                                overallFuncMin;

            return new PointD(imageDataX, imageDataY);
        }
    }
}
