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
    public partial class FunctionRepresentationForm : Form
    {
        public List<Func<DenseVector, double, double>> theRepresentingFunctions = new List<Func<DenseVector, double, double>>();
        public List<DenseVector> parameters = new List<DenseVector>();
        public DenseVector dvScatterXSpace = null;
        public DenseVector dvScatterFuncValues = null;
        public SequencesDrawingVariants scatterFuncDrawingVariant = SequencesDrawingVariants.circles;
        public List<Bgr> lineColors = new List<Bgr>();
        public double xSpaceMin = 0.0d;
        public double xSpaceMax = 1.0d;
        private string currentDescription = "";

        public string yAxisNote = "Y, px";
        public string xAxisNote = "X, px";

        public double overallFuncMax = 1.0d;
        public double overallFuncMin = 0.0d;
        int pictureWidth = 400;
        int pictureHeight = 300;
        int serviceSpaceGap = 40;
        Bgr colorBlack = new Bgr(0, 0, 0);
        private Bgr colorBlue = new Bgr(Color.Blue);
        Image<Bgr, Byte> theImage = null;


        //public FunctionRepresentationForm(FunctionDefinitionTypes theDefinitionType = FunctionDefinitionTypes.analytic, string description = "")
        public FunctionRepresentationForm(string description = "")
        {
            InitializeComponent();
            ThreadSafeOperations.SetText(lblTitle1, description, false);
            currentDescription = description;
        }

        private void FunctionRepresentationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }


        public void Represent()
        {
            if ((dvScatterXSpace != null) && (dvScatterFuncValues != null))
            {
                if (theRepresentingFunctions.Count > 0)
                {
                    double ySpaceGap = 0.2d * (dvScatterFuncValues.Max() - dvScatterFuncValues.Min());

                    overallFuncMax = ((dvScatterFuncValues.Max() + ySpaceGap) > overallFuncMax)
                        ? (dvScatterFuncValues.Max() + ySpaceGap)
                        : (overallFuncMax);
                    overallFuncMin = ((dvScatterFuncValues.Min() - ySpaceGap) < overallFuncMin)
                        ? (dvScatterFuncValues.Min() - ySpaceGap)
                        : (overallFuncMin);

                    //double xSpaceGap = 0.2d * (dvScatterXSpace.Max() - dvScatterXSpace.Min());

                    //xSpaceMin = ((dvScatterXSpace.Min() - xSpaceGap) < xSpaceMin)
                    //    ? (dvScatterXSpace.Min() - xSpaceGap)
                    //    : (xSpaceMin);
                    //xSpaceMax = ((dvScatterXSpace.Max() + xSpaceGap) > xSpaceMax)
                    //    ? (dvScatterXSpace.Max() + xSpaceGap)
                    //    : (xSpaceMax);
                    xSpaceMin = ((dvScatterXSpace.Min()) < xSpaceMin)
                        ? (dvScatterXSpace.Min())
                        : (xSpaceMin);
                    xSpaceMax = ((dvScatterXSpace.Max()) > xSpaceMax)
                        ? (dvScatterXSpace.Max())
                        : (xSpaceMax);
                }
                else
                {
                    double ySpaceGap = 0.2d * (dvScatterFuncValues.Max() - dvScatterFuncValues.Min());

                    overallFuncMax = dvScatterFuncValues.Max() + ySpaceGap;
                    overallFuncMin = dvScatterFuncValues.Min() - ySpaceGap;

                    //double xSpaceGap = 0.2d * (dvScatterXSpace.Max() - dvScatterXSpace.Min());

                    //xSpaceMin = ((dvScatterXSpace.Min() - xSpaceGap) < xSpaceMin)
                    //    ? (dvScatterXSpace.Min() - xSpaceGap)
                    //    : (xSpaceMin);
                    //xSpaceMax = ((dvScatterXSpace.Max() + xSpaceGap) > xSpaceMax)
                    //    ? (dvScatterXSpace.Max() + xSpaceGap)
                    //    : (xSpaceMax);
                    xSpaceMin = ((dvScatterXSpace.Min()) < xSpaceMin)
                        ? (dvScatterXSpace.Min())
                        : (xSpaceMin);
                    xSpaceMax = ((dvScatterXSpace.Max()) > xSpaceMax)
                        ? (dvScatterXSpace.Max())
                        : (xSpaceMax);
                }

            }

            pictureWidth = pbFunctionRepresentation.Width;
            pictureHeight = pbFunctionRepresentation.Height;
            serviceSpaceGap = Convert.ToInt32(0.05d * Math.Min(pictureHeight, pictureWidth));
            theImage = new Image<Bgr, Byte>(pictureWidth, pictureHeight, new Bgr(255, 255, 255));




            RepresentScatter();
            RepresentAnalytic();
        }


        private void RepresentAnalytic()
        {
            int xValuesCount = pictureWidth - (2 * serviceSpaceGap);// оставляем место на шкалу Y

            List<Point> rulerVertices = new List<Point>();
            rulerVertices.Add(new Point(serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, serviceSpaceGap));
            rulerVertices.Add(new Point(serviceSpaceGap, serviceSpaceGap));
            theImage.DrawPolyline(rulerVertices.ToArray(), true, colorBlack, 2);

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
            double dMarkersCount = (double)(pictureHeight - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            double dRulerValueGap = (overallFuncMax - overallFuncMin) / (double)dMarkersCount;
            //dRulerValueGap = (dRulerValueGap < 1.0d) ? (1.0d) : dRulerValueGap;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = overallFuncMin - Math.IEEERemainder(overallFuncMin, rulerValueGap);
            lowerMarkerValue = (lowerMarkerValue < overallFuncMin) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);
            double currentMarkerValue = lowerMarkerValue;
            double nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            while (nextYPositionDouble > serviceSpaceGap)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(serviceSpaceGap, yPosition), new Point(serviceSpaceGap - 5, yPosition));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValue.ToString();
                theImage.Draw(message, ref theFont, new Point(2, yPosition), markerColor);
                currentMarkerValue += rulerValueGap;
                nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            }

            double dMarkersCountX = (double)(pictureWidth - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCountX = (dMarkersCountX > 10.0d) ? (10.0d) : (dMarkersCountX);
            double dRulerValueGapX = (xSpaceMax - xSpaceMin) / (double)dMarkersCountX;
            int deciGapX = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGapX, 2.0d)));
            double rulerValueGapX = Math.Pow(2.0, (double)deciGapX);
            double lowerMarkerValueX = xSpaceMin - Math.IEEERemainder(xSpaceMin, rulerValueGapX);
            lowerMarkerValueX = (lowerMarkerValueX < xSpaceMin) ? (lowerMarkerValueX + rulerValueGapX) : (lowerMarkerValueX);
            double currentMarkerValueX = lowerMarkerValueX;
            double nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            while (nextXPositionDouble < pictureWidth - serviceSpaceGap)
            {
                double xPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
                int xPosition = Convert.ToInt32(Math.Round(xPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(xPosition, pictureHeight - serviceSpaceGap), new Point(xPosition, pictureHeight - serviceSpaceGap + 5));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValueX.ToString();
                theImage.Draw(message, ref theFont, new Point(xPosition, pictureHeight - 10), markerColor);
                currentMarkerValueX += rulerValueGapX;
                nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            }

            #endregion Прописываем текстовые маркеры




            double koeff = (pictureHeight - (2 * serviceSpaceGap)) / (overallFuncMax - overallFuncMin);

            for (int i = 0; i < theRepresentingFunctions.Count; i++)
            {
                Func<DenseVector, double, double> theRepresentingFunction = theRepresentingFunctions[i];
                DenseVector currentParametersList = parameters[i];

                //DenseVector dvXSpaceValues = DenseVector.Create(xValuesCount, new Func<int, double>(i => xSpaceMin + ((double)i / ((double)xValuesCount - 1.0d)) * (xSpaceMax - xSpaceMin)));
                parametersList = null;
                DenseVector dvFuncValues = DenseVector.Create(xValuesCount, new Func<int, double>(j => theRepresentingFunction(currentParametersList, dvXSpaceValues[j])));
                Bgr currentLineColor = lineColors[i];


                DenseVector xCoordinates = DenseVector.Create(xValuesCount, new Func<int, double>(j => ((double)serviceSpaceGap + j)));
                DenseVector yCoordinates = DenseVector.Create(xValuesCount, new Func<int, double>(j =>
                {
                    double pixValue = koeff * (dvFuncValues[j] - overallFuncMin);
                    return (pictureHeight - serviceSpaceGap - pixValue);
                }));

                List<Point> funcRepresentationPoints = new List<Point>();
                for (int j = 0; j < xValuesCount; j++)
                {
                    if (double.IsNaN(yCoordinates[j])) continue;
                    funcRepresentationPoints.Add(new Point(Convert.ToInt32(xCoordinates[j]), Convert.ToInt32(yCoordinates[j])));
                }
                theImage.DrawPolyline(funcRepresentationPoints.ToArray(), false, currentLineColor, 2);
            }


            ThreadSafeOperations.UpdatePictureBox(pbFunctionRepresentation, theImage.Bitmap, false);
        }








        private void RepresentScatter()
        {
            if ((dvScatterXSpace == null) || (dvScatterFuncValues == null)) return;

            double koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGap) / (overallFuncMax - overallFuncMin);
            double koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGap) / (xSpaceMax - xSpaceMin);

            List<Point> pointsList = new List<Point>();
            for (int i = 0; i < dvScatterXSpace.Count; i++)
            {
                int yCoordinate = Convert.ToInt32(pictureHeight - serviceSpaceGap - (dvScatterFuncValues[i] - overallFuncMin) * koeffY);
                int xCoordinate = Convert.ToInt32(serviceSpaceGap + (dvScatterXSpace[i] - xSpaceMin) * koeffX);
                pointsList.Add(new Point(xCoordinate, yCoordinate));
            }

            if (scatterFuncDrawingVariant == SequencesDrawingVariants.circles)
            {
                foreach (Point thePoint in pointsList)
                {
                    theImage.Draw(new CircleF(thePoint, 3), new Bgr(255, 0, 0), 2);
                }
            }
            else if (scatterFuncDrawingVariant == SequencesDrawingVariants.polyline)
            {
                theImage.DrawPolyline(pointsList.ToArray(), false, colorBlue, 2);
            }
            else if (scatterFuncDrawingVariant == SequencesDrawingVariants.squares)
            {
                foreach (Point thePoint in pointsList)
                {
                    List<Point> squareVertices = new List<Point>();
                    squareVertices.Add(new Point(thePoint.X - 2, thePoint.Y - 2));
                    squareVertices.Add(new Point(thePoint.X - 2, thePoint.Y + 2));
                    squareVertices.Add(new Point(thePoint.X + 2, thePoint.Y + 2));
                    squareVertices.Add(new Point(thePoint.X + 2, thePoint.Y - 2));
                    theImage.DrawPolyline(squareVertices.ToArray(), true, colorBlue, 2);
                }
            }
            else if (scatterFuncDrawingVariant == SequencesDrawingVariants.triangles)
            {
                foreach (Point thePoint in pointsList)
                {
                    List<Point> triangleVertices = new List<Point>();
                    triangleVertices.Add(new Point(thePoint.X - 2, thePoint.Y - 2));
                    triangleVertices.Add(new Point(thePoint.X, thePoint.Y + 2));
                    triangleVertices.Add(new Point(thePoint.X + 2, thePoint.Y - 2));
                    theImage.DrawPolyline(triangleVertices.ToArray(), true, colorBlue, 2);
                }
            }

        }




        #region OBSOLETE save as PDF manually
        //private void PDFRepresentScatter(PdfDocument Document, PdfContents Contents)
        //{
        //    if ((dvScatterXSpace == null) || (dvScatterFuncValues == null)) return;

        //    double koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGap) / (overallFuncMax - overallFuncMin);
        //    double koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGap) / (xSpaceMax - xSpaceMin);

        //    List<Point> pointsList = new List<Point>();
        //    for (int i = 0; i < dvScatterXSpace.Count; i++)
        //    {
        //        int yCoordinate = Convert.ToInt32(pictureHeight - serviceSpaceGap - (dvScatterFuncValues[i] - overallFuncMin) * koeffY);
        //        int xCoordinate = Convert.ToInt32(serviceSpaceGap + (dvScatterXSpace[i] - xSpaceMin) * koeffX);
        //        pointsList.Add(new Point(xCoordinate, yCoordinate));
        //    }

        //    foreach (Point thePoint in pointsList)
        //    {
        //        theImage.Draw(new CircleF(thePoint, 3), new Bgr(255, 0, 0), 2);
        //    }
        //    //theImage.DrawPolyline(pointsList.ToArray(), false, colorBlack, 2);
        //}




        //private void PDFRepresentAnalytic()
        //{
        //    int xValuesCount = pictureWidth - (2 * serviceSpaceGap);// оставляем место на шкалу Y

        //    List<Point> rulerVertices = new List<Point>();
        //    rulerVertices.Add(new Point(serviceSpaceGap, pictureHeight - serviceSpaceGap));
        //    rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, pictureHeight - serviceSpaceGap));
        //    rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, serviceSpaceGap));
        //    rulerVertices.Add(new Point(serviceSpaceGap, serviceSpaceGap));
        //    theImage.DrawPolyline(rulerVertices.ToArray(), true, colorBlack, 2);

        //    DenseVector dvXSpaceValues = DenseVector.Create(xValuesCount, new Func<int, double>(i => xSpaceMin + ((double)i / ((double)xValuesCount - 1.0d)) * (xSpaceMax - xSpaceMin)));
        //    DenseVector parametersList = null;

        //    for (int i = 0; i < theRepresentingFunctions.Count; i++)
        //    {
        //        Func<DenseVector, double, double> theRepresentingFunction = theRepresentingFunctions[i];
        //        DenseVector currentParametersList = parameters[i];

        //        DenseVector dvFuncValues = DenseVector.Create(xValuesCount, new Func<int, double>(j => theRepresentingFunction(currentParametersList, dvXSpaceValues[j])));
        //        double funcMax = dvFuncValues.Max();
        //        double funcMin = dvFuncValues.Min();
        //        overallFuncMax = (funcMax > overallFuncMax) ? (funcMax) : (overallFuncMax);
        //        overallFuncMin = (funcMin < overallFuncMin) ? (funcMin) : (overallFuncMin);
        //    }



        //    #region Прописываем текстовые маркеры
        //    MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);
        //    theFont.thickness = 1;
        //    double dMarkersCount = (double)(pictureHeight - (2 * serviceSpaceGap)) / 30.0d;
        //    dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
        //    double dRulerValueGap = (overallFuncMax - overallFuncMin) / (double)dMarkersCount;
        //    //dRulerValueGap = (dRulerValueGap < 1.0d) ? (1.0d) : dRulerValueGap;
        //    int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
        //    double rulerValueGap = Math.Pow(2.0, (double)deciGap);
        //    double lowerMarkerValue = overallFuncMin - Math.IEEERemainder(overallFuncMin, rulerValueGap);
        //    lowerMarkerValue = (lowerMarkerValue < overallFuncMin) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);
        //    double currentMarkerValue = lowerMarkerValue;
        //    while (currentMarkerValue < overallFuncMax)
        //    {
        //        double yPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap);
        //        int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
        //        LineSegment2D theLine = new LineSegment2D(new Point(serviceSpaceGap, yPosition), new Point(serviceSpaceGap - 5, yPosition));
        //        Bgr markerColor = colorBlack;
        //        theImage.Draw(theLine, markerColor, 2);
        //        String message = Math.Round(currentMarkerValue, 2).ToString();
        //        theImage.Draw(message, ref theFont, new Point(2, yPosition), markerColor);
        //        currentMarkerValue += rulerValueGap;
        //    }

        //    double dMarkersCountX = (double)(pictureWidth - (2 * serviceSpaceGap)) / 30.0d;
        //    dMarkersCountX = (dMarkersCountX > 10.0d) ? (10.0d) : (dMarkersCountX);
        //    double dRulerValueGapX = (xSpaceMax - xSpaceMin) / (double)dMarkersCountX;
        //    int deciGapX = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGapX, 2.0d)));
        //    double rulerValueGapX = Math.Pow(2.0, (double)deciGapX);
        //    double lowerMarkerValueX = xSpaceMin - Math.IEEERemainder(xSpaceMin, rulerValueGapX);
        //    lowerMarkerValueX = (lowerMarkerValueX < xSpaceMin) ? (lowerMarkerValueX + rulerValueGapX) : (lowerMarkerValueX);
        //    double currentMarkerValueX = lowerMarkerValueX;
        //    while (currentMarkerValueX < xSpaceMax)
        //    {
        //        double xPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
        //        int xPosition = Convert.ToInt32(Math.Round(xPositionDouble));
        //        LineSegment2D theLine = new LineSegment2D(new Point(xPosition, pictureHeight - serviceSpaceGap), new Point(xPosition, pictureHeight - serviceSpaceGap + 5));
        //        Bgr markerColor = colorBlack;
        //        theImage.Draw(theLine, markerColor, 2);
        //        String message = Math.Round(currentMarkerValueX, 2).ToString();
        //        theImage.Draw(message, ref theFont, new Point(xPosition, pictureHeight - 10), markerColor);
        //        currentMarkerValueX += rulerValueGapX;
        //    }

        //    #endregion Прописываем текстовые маркеры




        //    double koeff = (pictureHeight - (2 * serviceSpaceGap)) / (overallFuncMax - overallFuncMin);

        //    for (int i = 0; i < theRepresentingFunctions.Count; i++)
        //    {
        //        Func<DenseVector, double, double> theRepresentingFunction = theRepresentingFunctions[i];
        //        DenseVector currentParametersList = parameters[i];

        //        //DenseVector dvXSpaceValues = DenseVector.Create(xValuesCount, new Func<int, double>(i => xSpaceMin + ((double)i / ((double)xValuesCount - 1.0d)) * (xSpaceMax - xSpaceMin)));
        //        parametersList = null;
        //        DenseVector dvFuncValues = DenseVector.Create(xValuesCount, new Func<int, double>(j => theRepresentingFunction(currentParametersList, dvXSpaceValues[j])));
        //        Bgr currentLineColor = lineColors[i];


        //        DenseVector xCoordinates = DenseVector.Create(xValuesCount, new Func<int, double>(j => ((double)serviceSpaceGap + j)));
        //        DenseVector yCoordinates = DenseVector.Create(xValuesCount, new Func<int, double>(j =>
        //        {
        //            double pixValue = koeff * (dvFuncValues[j] - overallFuncMin);
        //            return (pictureHeight - serviceSpaceGap - pixValue);
        //        }));

        //        List<Point> funcRepresentationPoints = new List<Point>();
        //        for (int j = 0; j < xValuesCount; j++)
        //        {
        //            if (double.IsNaN(yCoordinates[j])) continue;
        //            funcRepresentationPoints.Add(new Point(Convert.ToInt32(xCoordinates[j]), Convert.ToInt32(yCoordinates[j])));
        //        }
        //        theImage.DrawPolyline(funcRepresentationPoints.ToArray(), false, currentLineColor, 2);
        //    }


        //    ThreadSafeOperations.UpdatePictureBox(pbFunctionRepresentation, theImage.Bitmap, false);
        //}
        #endregion OBSOLETE save as PDF manually






        public void SaveToImage(string filename = "", bool absolutePath = true)
        {
            if (filename == "") return;

            theImage.Bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        }



        public void SaveAsPDF(string filename = "", bool absolutePath = true)
        {
            if (filename == "") return;

            int pointsCount = 101;

            string matlabScript = "clear;" + Environment.NewLine;
            matlabScript += "fig = figure;" + Environment.NewLine;
            matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
            matlabScript += "hold on;" + Environment.NewLine;

            double xSpaceDiff = (xSpaceMax - xSpaceMin) / (double)(pointsCount - 1);


            if (theRepresentingFunctions.Count > 0)
            {
                DenseVector xSpaceML = DenseVector.Create(pointsCount, new Func<int, double>(i =>
                {
                    return xSpaceMin + ((double)i / (double)pointsCount) * (xSpaceMax - xSpaceMin);
                }));

                foreach (Tuple<int, double> tuple in xSpaceML.GetIndexedEnumerator())
                {
                    matlabScript += "xSpace(" + (tuple.Item1 + 1).ToString() + ") = " + tuple.Item2.ToString("e").Replace(",", ".") + ";" + Environment.NewLine;
                }

                int funcCounter = 0;
                for (int funcIndex = 0; funcIndex < theRepresentingFunctions.Count; funcIndex++)
                {
                    Func<DenseVector, double, double> function = theRepresentingFunctions[funcIndex];
                    DenseVector parametersVector = parameters[funcIndex];

                    DenseVector dvCurrFuncValues = DenseVector.Create(pointsCount,
                        new Func<int, double>(i => function(parametersVector, xSpaceML[i])));
                    foreach (Tuple<int, double> tuple in dvCurrFuncValues.GetIndexedEnumerator())
                    {
                        matlabScript += "func" + funcIndex.ToString() + "(" + (tuple.Item1 + 1).ToString() + ") = " + tuple.Item2.ToString("e").Replace(",", ".") + ";" + Environment.NewLine;
                    }
                }


                for (int i = 0; i < theRepresentingFunctions.Count; i++)
                {
                    Bgr CurrColor = lineColors[i];
                    string colR = (CurrColor.Red / 255.0d).ToString("e").Replace(",", ".");
                    string colG = (CurrColor.Green / 255.0d).ToString("e").Replace(",", ".");
                    string colB = (CurrColor.Blue / 255.0d).ToString("e").Replace(",", ".");
                    matlabScript += "plot(xSpace, func" + i + ", 'Color', [" + colR + " " + colG + " " + colB + "], 'LineWidth', 2.0);" + Environment.NewLine;
                }
            }


            if ((dvScatterXSpace != null) && (dvScatterFuncValues != null))
            {
                foreach (Tuple<int, double> tuple in dvScatterXSpace.GetIndexedEnumerator())
                {
                    matlabScript += "scatterXSpace(" + (tuple.Item1 + 1).ToString() + ") = " + tuple.Item2.ToString("e").Replace(",", ".") + ";" + Environment.NewLine;
                }


                foreach (Tuple<int, double> tuple in dvScatterFuncValues.GetIndexedEnumerator())
                {
                    matlabScript += "scatterFuncVals(" + (tuple.Item1 + 1).ToString() + ") = " + tuple.Item2.ToString("e").Replace(",", ".") + ";" + Environment.NewLine;
                }

                matlabScript += "scatter(scatterXSpace, scatterFuncVals, 'bo', 'LineWidth', 2);" + Environment.NewLine;
            }

            matlabScript += "title('" + currentDescription + "', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;
            matlabScript += "ylabel(gca, '" + yAxisNote + "', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;
            matlabScript += "xlabel(gca, '" + xAxisNote + "', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;
            matlabScript += "export_fig '" + filename + "';" + Environment.NewLine;
            matlabScript += "close(fig);" + Environment.NewLine;

            ServiceTools.logToTextFile(filename.Replace(".", "") + ".m", matlabScript);

            //MLApp.MLApp ml = new MLApp.MLApp();
            //ml.Visible = 0;
            //ml.Execute("run('" + curDir + "tmpMLscript.m" + "');");
            //File.Delete(curDir + "tmpMLscript.m");
            //ml.Execute("exit;");
        }



        #region OBSOLETE save as PDF manually - p2
        //private PdfFont ArialNormal;
        //private PdfFont ArialBold;
        //private PdfFont ArialItalic;
        //private PdfFont ArialBoldItalic;
        //private PdfFont TimesNormal;
        //private PdfFont Comic;
        //private PdfTilingPattern WaterMark;
        //public void SaveToPDF(string filename = "", bool absolutePath = true)
        //{
        //    if (filename == "") return;

        //    PdfDocument thePdfDoc = new PdfDocument(10.0, 10.0, UnitOfMeasure.cm);
        //    DefineFontResources(thePdfDoc);
        //    //DefineTilingPatternResource(thePdfDoc);
        //    PdfPage Page = new PdfPage(thePdfDoc);
        //    PdfContents Contents = new PdfContents(Page);

        //    DrawHeading(Contents, currentDescription);
        //    DrawAnImage(thePdfDoc, Contents, theImage.Bitmap, filename);
        //    //DrawTextBox(Contents);
        //    thePdfDoc.CreateFile(filename);

        //    //File.Delete(filename + "_tmp.jpg");

        //    Process Proc = new Process();
        //    Proc.StartInfo = new ProcessStartInfo(filename);
        //    Proc.Start();

        //}

        //private void DefineFontResources(PdfDocument Document)
        //{
        //    ArialNormal = new PdfFont(Document, "Arial", FontStyle.Regular, true);
        //    ArialBold = new PdfFont(Document, "Arial", FontStyle.Bold, true);
        //    ArialItalic = new PdfFont(Document, "Arial", FontStyle.Italic, true);
        //    ArialBoldItalic = new PdfFont(Document, "Arial", FontStyle.Bold | FontStyle.Italic, true);
        //    TimesNormal = new PdfFont(Document, "Times New Roman", FontStyle.Regular, true);
        //    Comic = new PdfFont(Document, "Comic Sans MS", FontStyle.Bold, true);
        //    ArialNormal.CharSubstitution(9679, 9679, 164);
        //    return;
        //}



        //private void DrawHeading(PdfContents Contents, string headingString = "")
        //{
        //    Contents.DrawText(ArialNormal, 12.0, 5.0, 9.5, TextJustify.Center, 0.02, Color.Black, Color.Black, headingString);
        //    Contents.SaveGraphicsState();
        //    Contents.RestoreGraphicsState();
        //    return;
        //}



        //private void DrawAnImage(PdfDocument Document, PdfContents Contents, Bitmap theImageBitmap, string finalFileName)
        //{
        //    // define local image resources
        //    string tmpFilename = finalFileName + "_tmp.jpg";
        //    //System.Drawing.Imaging.EncoderParameters encParams = new System.Drawing.Imaging.EncoderParameters(1);
        //    //encParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
        //    theImageBitmap.Save(tmpFilename, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    PdfImage Image1 = new PdfImage(Document, tmpFilename);

        //    // image size will be limited to 1.4" by 1.4"
        //    PdfFileWriter.SizeD ImageSize = Image1.ImageSize(10.0d, 10.0d);

        //    // save graphics state
        //    Contents.SaveGraphicsState();

        //    // translate coordinate origin to the center of the picture
        //    //Contents.Translate(3.36, 5.7);

        //    // clipping path
        //    //Contents.DrawOval(-ImageSize.Width / 2, -ImageSize.Height / 2, ImageSize.Width, ImageSize.Height, PaintOp.ClipPathEor);

        //    // draw image
        //    Contents.DrawImage(Image1, 0.5d, 0.5d, ImageSize.Width, ImageSize.Height);

        //    // restore graphics state
        //    Contents.RestoreGraphicsState();

        //    return;
        //}


        //private void DrawTextBox(PdfContents Contents)
        //{
        //    // save graphics state
        //    Contents.SaveGraphicsState();

        //    // translate origin to PosX=1.1" and PosY=1.1" this is the bottom left corner of the text box example
        //    Contents.Translate(1.1, 1.1);
        //    //		Contents.TranslateScaleRotate(7.4, 1.1, 1.0, Math.PI / 2);

        //    // Define constants
        //    // Box width 3.25"
        //    // Box height is 3.65"
        //    // Normal font size is 9.0 points.
        //    const Double Width = 3.15;
        //    const Double Height = 3.65;
        //    const Double FontSize = 9.0;

        //    // Create text box object width 3.25"
        //    // First line indent of 0.25"
        //    PdfFileWriter.TextBox Box = new PdfFileWriter.TextBox(Width, 0.25);

        //    // add text to the text box
        //    Box.AddText(ArialNormal, FontSize,
        //        "This area is an example of displaying text that is too long to fit within a fixed width " +
        //        "area. The text is displayed justified to right edge. You define a text box with the required " +
        //        "width and first line indent. You add text to this box. The box will divide the text into " +
        //        "lines. Each line is made of segments of text. For each segment, you define font, font " +
        //        "size, drawing style and color. After loading all the text, the program will draw the formatted text.\n");
        //    Box.AddText(TimesNormal, FontSize + 1.0, "Example of multiple fonts: Times New Roman, ");
        //    Box.AddText(Comic, FontSize, "Comic Sans MS, ");
        //    Box.AddText(ArialNormal, FontSize, "Example of regular, ");
        //    Box.AddText(ArialBold, FontSize, "bold, ");
        //    Box.AddText(ArialItalic, FontSize, "italic, ");
        //    Box.AddText(ArialBoldItalic, FontSize, "bold plus italic. ");
        //    Box.AddText(ArialNormal, FontSize - 2.0, "Arial size 7, ");
        //    Box.AddText(ArialNormal, FontSize - 1.0, "size 8, ");
        //    Box.AddText(ArialNormal, FontSize, "size 9, ");
        //    Box.AddText(ArialNormal, FontSize + 1.0, "size 10. ");
        //    Box.AddText(ArialNormal, FontSize, DrawStyle.Underline, "Underline, ");
        //    Box.AddText(ArialNormal, FontSize, DrawStyle.Strikeout, "Strikeout. ");
        //    Box.AddText(ArialNormal, FontSize, "Subscript H");
        //    Box.AddText(ArialNormal, FontSize, DrawStyle.Subscript, "2");
        //    Box.AddText(ArialNormal, FontSize, "O. Superscript A");
        //    Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
        //    Box.AddText(ArialNormal, FontSize, "+B");
        //    Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
        //    Box.AddText(ArialNormal, FontSize, "=C");
        //    Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
        //    Box.AddText(ArialNormal, FontSize, "\n");
        //    Box.AddText(Comic, FontSize, Color.Red, "Lets add some color, ");
        //    Box.AddText(Comic, FontSize, Color.Green, "green, ");
        //    Box.AddText(Comic, FontSize, Color.Blue, "blue, ");
        //    Box.AddText(Comic, FontSize, Color.Orange, "orange, ");
        //    Box.AddText(Comic, FontSize, DrawStyle.Underline, Color.Purple, "and purple.\n");

        //    // Draw the text box
        //    // Text left edge is at zero (note: origin was translated to 1.1") 
        //    // The top text base line is at Height less first line ascent.
        //    // Text drawing is limited to vertical coordinate of zero.
        //    // First line to be drawn is line zero.
        //    // After each line add extra 0.015".
        //    // After each paragraph add extra 0.05"
        //    // Stretch all lines to make smooth right edge at box width of 3.15"
        //    // After all lines are drawn, PosY will be set to the next text line after the box's last paragraph
        //    Double PosY = Height;
        //    Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.015, 0.05, true, Box);

        //    // Create text box object width 3.25"
        //    // No first line indent
        //    Box = new PdfFileWriter.TextBox(Width);

        //    // Add text as before.
        //    // No extra line spacing.
        //    // No right edge adjustment
        //    Box.AddText(ArialNormal, FontSize,
        //        "In the examples above this area the text box was set for first line indent of " +
        //        "0.25 inches. This paragraph has zero first line indent and no right justify.");
        //    Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.01, 0.05, false, Box);

        //    // Create text box object width 2.75
        //    // First line hanging indent of 0.5"
        //    Box = new PdfFileWriter.TextBox(Width - 0.5, -0.5);

        //    // Add text
        //    Box.AddText(ArialNormal, FontSize,
        //        "This paragraph is set to first line hanging indent of 0.5 inches. " +
        //        "The left margin of this paragraph is 0.5 inches.");

        //    // Draw the text
        //    // left edge at 0.5"
        //    Contents.DrawText(0.5, ref PosY, 0.0, 0, 0.01, 0.05, false, Box);

        //    // restore graphics state
        //    Contents.RestoreGraphicsState();
        //    return;
        //}
        #endregion OBSOLETE save as PDF manually - p2




        private void FunctionRepresentationForm_Load(object sender, EventArgs e)
        {
            ThreadSafeOperations.SetText(lblTitle1, currentDescription, false);
        }

        private void FunctionRepresentationForm_Resize(object sender, EventArgs e)
        {
            Represent();
        }



        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (tbFileName.Text == "")
            {
                return;
            }

            string fileName = tbFileName.Text;
            if (fileName == "")
            {
                SaveFileDialog d1 = new SaveFileDialog();
                //d1.DefaultExt = "";
                d1.FileName = DateTime.UtcNow.ToString("s").Replace(":", "-") + ".jpg";
                d1.Filter = "jpeg images | *.jpg";
                d1.AddExtension = true;
                DialogResult res = d1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    fileName = d1.FileName;
                }
            }

            try
            {
                theImage.Bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                ThreadSafeOperations.SetText(lblTitle1, fileName, false);
            }
            catch (Exception)
            {
                ThreadSafeOperations.SetText(lblTitle1, "Couldnt save the file", false);
                return;
            }
        }

    }
}
