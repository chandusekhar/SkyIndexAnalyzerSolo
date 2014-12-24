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
using MathNet.Numerics.Statistics;

namespace SkyImagesAnalyzerLibraries
{
    public partial class HistogramCalcAndShowForm : Form
    {
        private HistogramDataAndProperties histToRepresent = new HistogramDataAndProperties();
        public double xSpaceMin = 0.0d;
        public double xSpaceMax = 1.0d;
        private string currentDescription = "";

        public string yAxisNote = "PD";
        public string xAxisNote = "data";

        public double overallProbMax = 1.0d;
        private double koeffY = 1.0d;
        private double koeffX = 1.0d;
        private int barWidth = 10;

        int pictureWidth = 400;
        int pictureHeight = 300;
        int serviceSpaceGap = 40;
        Bgr colorBlack = new Bgr(0, 0, 0);
        Bgr colorRed = new Bgr(Color.Red);
        Bgr colorGreen = new Bgr(Color.Green);
        Bgr colorBlue = new Bgr(Color.Blue);
        Bgr colorYellow = new Bgr(Color.Yellow);
        Bgr colorMagenta = new Bgr(Color.Magenta);
        Image<Bgr, Byte> theImage = null;

        private MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);

        public Dictionary<string, object> defaultProperties = null;



        public HistogramCalcAndShowForm(string description, Dictionary<string, object> inDefProperties)
        {
            InitializeComponent();
            ThreadSafeOperations.SetText(lblTitle, description, false);
            currentDescription = description;
            theFont.thickness = 1;
            if (inDefProperties != null)
            {
                defaultProperties = inDefProperties;
            }
        }

        public HistogramDataAndProperties HistToRepresent
        {
            get { return histToRepresent; }
            set
            {
                histToRepresent = value;
                tbQuantilesCount.Text = histToRepresent.quantilesCount.ToString();
            }
        }

        private void HistogramCalcAndShowForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }



        public void SaveToImage(string filename = "")
        {
            if (filename == "") return;

            theImage.Bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        }




        private void HistogramCalcAndShowForm_Load(object sender, EventArgs e)
        {
            ThreadSafeOperations.SetText(lblTitle, currentDescription, false);
        }




        private void HistogramCalcAndShowForm_Resize(object sender, EventArgs e)
        {
            Represent();
        }



        public void Represent()
        {
            if (histToRepresent.IsEmpty()) return;

            ThreadSafeOperations.SetTextTB(tbStats, "", false);

            double maxHistProbValue = histToRepresent.dvProbDens.Values.Max();
            double ySpaceGap = 0.2d * maxHistProbValue;
            overallProbMax = maxHistProbValue + ySpaceGap;

            double maxHistValues = histToRepresent.dvData.Values.Max();
            double minHistValues = histToRepresent.dvData.Values.Min();

            double xSpaceGap = 0.2d * (maxHistValues - minHistValues);

            xSpaceMin = ((minHistValues - xSpaceGap) < xSpaceMin) ? (minHistValues - xSpaceGap) : (xSpaceMin);
            xSpaceMax = ((maxHistValues + xSpaceGap) > xSpaceMax) ? (maxHistValues + xSpaceGap) : (xSpaceMax);

            pictureWidth = pbHistRepresentation.Width;
            pictureHeight = pbHistRepresentation.Height;
            serviceSpaceGap = Convert.ToInt32(0.05d * Math.Min(pictureHeight, pictureWidth));
            theImage = new Image<Bgr, Byte>(pictureWidth, pictureHeight, new Bgr(255, 255, 255));


            List<Point> rulerVertices = new List<Point>();
            rulerVertices.Add(new Point(serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, serviceSpaceGap));
            rulerVertices.Add(new Point(serviceSpaceGap, serviceSpaceGap));
            theImage.DrawPolyline(rulerVertices.ToArray(), true, colorBlack, 2);

            koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGap) / (overallProbMax);
            koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGap) / (xSpaceMax - xSpaceMin);




            #region Прописываем текстовые маркеры

            double dMarkersCount = (double)(pictureHeight - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            double dRulerValueGap = (overallProbMax) / (double)dMarkersCount;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = 0.0d;
            double currentMarkerValue = lowerMarkerValue;
            double nextYPositionDouble = (1.0d - ((currentMarkerValue) / overallProbMax)) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            while (nextYPositionDouble > serviceSpaceGap)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue) / overallProbMax)) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(serviceSpaceGap, yPosition), new Point(serviceSpaceGap - 5, yPosition));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = Math.Round(currentMarkerValue, 2).ToString();
                theImage.Draw(message, ref theFont, new Point(2, yPosition), markerColor);
                currentMarkerValue += rulerValueGap;
                nextYPositionDouble = (1.0d - ((currentMarkerValue) / overallProbMax)) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
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
                String message = Math.Round(currentMarkerValueX, 2).ToString();
                theImage.Draw(message, ref theFont, new Point(xPosition, pictureHeight - 10), markerColor);
                currentMarkerValueX += rulerValueGapX;
                nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            }

            #endregion Прописываем текстовые маркеры


            #region проставляем квантили, если надо

            if (cbShowQuantiles.Checked)
            {
                for (int i = 0; i < histToRepresent.quantilesList.Count; i++)
                {
                    double binCenter = histToRepresent.quantilesList[i];
                    int xCoordinateBinCenter = Convert.ToInt32(serviceSpaceGap + (binCenter - xSpaceMin) * koeffX);
                    Point ptTop = new Point(xCoordinateBinCenter, pictureHeight - serviceSpaceGap);
                    Point ptBottom = new Point(xCoordinateBinCenter, serviceSpaceGap);
                    theImage.Draw(new LineSegment2D(ptTop, ptBottom), colorGreen, 2);
                }
            }
            #endregion проставляем квантили, если надо


            if (rbtnShowAsBars.Checked)
            {
                double dBarWidth = (double) pictureWidth - 2.0d*(double) serviceSpaceGap;
                int barsCount = histToRepresent.BinsCount;
                dBarWidth = dBarWidth/(double) barsCount;
                dBarWidth = dBarWidth/3.0d;
                barWidth = Convert.ToInt32(dBarWidth);
                barWidth = (barWidth < 1) ? (1) : (barWidth);
                int barWidthHalfed = Convert.ToInt32(dBarWidth/2.0d);
                barWidthHalfed = (barWidthHalfed < 1) ? (1) : (barWidthHalfed);


                for (int i = 0; i < histToRepresent.BinsCount; i++)
                {
                    double binCenter = histToRepresent.dvbinsCenters[i];
                    double probDens = histToRepresent.dvProbDens[i];
                    int xCoordinateBinCenter = Convert.ToInt32(serviceSpaceGap + (binCenter - xSpaceMin)*koeffX);
                    int yCoordinateTop = Convert.ToInt32(pictureHeight - serviceSpaceGap - probDens*koeffY);
                    yCoordinateTop = (yCoordinateTop == pictureHeight - serviceSpaceGap)
                        ? (pictureHeight - serviceSpaceGap - 2)
                        : (yCoordinateTop);
                    Point ptUperLeft = new Point(xCoordinateBinCenter - barWidthHalfed, yCoordinateTop);
                    Size barSize = new Size(barWidth, pictureHeight - serviceSpaceGap - yCoordinateTop);

                    theImage.Draw(new Rectangle(ptUperLeft, barSize), new Bgr(histToRepresent.color), -1);
                }
            }
            else
            {
                for (int i = 0; i < histToRepresent.BinsCount; i++)
                {
                    double binCenter = histToRepresent.dvbinsCenters[i];
                    double probDens = histToRepresent.dvProbDens[i];
                    int xCoordinateBinCenter = Convert.ToInt32(serviceSpaceGap + (binCenter - xSpaceMin) * koeffX);
                    int yCoordinateTop = Convert.ToInt32(pictureHeight - serviceSpaceGap - probDens * koeffY);
                    PointF ptToShow = new PointF((float)xCoordinateBinCenter, (float)yCoordinateTop);

                    theImage.Draw(new CircleF(ptToShow, 4), new Bgr(histToRepresent.color), -1);
                }
            }

            string str2Show = "[" + histToRepresent.description + "]" + Environment.NewLine;
            str2Show += "color: " + histToRepresent.color.ToString() + Environment.NewLine;
            str2Show += "median = " + histToRepresent.Median.ToString("e") + Environment.NewLine;
            str2Show += "mean = " + histToRepresent.stats.Mean.ToString("e") + Environment.NewLine;
            str2Show += "stdDev = " + histToRepresent.stats.StandardDeviation.ToString("e") + Environment.NewLine;
            str2Show += "min value = " + histToRepresent.stats.Minimum.ToString("e") + Environment.NewLine;
            str2Show += "max value = " + histToRepresent.stats.Maximum.ToString("e") + Environment.NewLine;
            ThreadSafeOperations.SetTextTB(tbStats, str2Show, true);

            ThreadSafeOperations.UpdatePictureBox(pbHistRepresentation, theImage.Bitmap, false);
        }

        private void tbBinsCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)// Enter key
            {
                if (Convert.ToInt32(tbBinsCount.Text) > 0)
                {
                    histToRepresent.BinsCount = Convert.ToInt32(tbBinsCount.Text);
                }

                Represent();
            }

        }

        private void pbHistRepresentation_Click(object sender, EventArgs e)
        {
            //xCoordinateBinCenter = Convert.ToInt32(serviceSpaceGap + (binCenter - xSpaceMin) * koeffX);
            //yCoordinateTop = Convert.ToInt32(pictureHeight - serviceSpaceGap - probDens * koeffY);

            // currBinCenter = ((xCoordinate - serviceSpaceGap)/koeffX) + xSpaceMin
            // currProbDens = (pictureHeight - serviceSpaceGap - yCoordinateTop)/koeffY

            int mouseClickX = ((MouseEventArgs)e).X;
            int mouseClickY = ((MouseEventArgs)e).Y;
            double currBinCenter = ((mouseClickX - serviceSpaceGap) / koeffX) + xSpaceMin;
            //double currProbDens = (pictureHeight - serviceSpaceGap - mouseClickY)/koeffY;

            int barWidthHalfed = Convert.ToInt32((double)barWidth / 2.0d);
            barWidthHalfed = (barWidthHalfed < 1) ? (1) : (barWidthHalfed);

            for (int i = 0; i < histToRepresent.BinsCount; i++)
            {
                double binCenter = histToRepresent.dvbinsCenters[i];
                double probDens = histToRepresent.dvProbDens[i];
                int xCoordinateBinCenter = Convert.ToInt32(serviceSpaceGap + (binCenter - xSpaceMin) * koeffX);
                if ((mouseClickX >= xCoordinateBinCenter - barWidthHalfed) &&
                    (mouseClickX <= xCoordinateBinCenter + barWidthHalfed))
                {
                    int yCoordinateTop = Convert.ToInt32(pictureHeight - serviceSpaceGap - probDens * koeffY);
                    yCoordinateTop = (yCoordinateTop == pictureHeight - serviceSpaceGap)
                        ? (pictureHeight - serviceSpaceGap - 2)
                        : (yCoordinateTop);
                    Point ptUperLeft = new Point(xCoordinateBinCenter - barWidthHalfed, yCoordinateTop);
                    Size barSize = new Size(barWidth, pictureHeight - serviceSpaceGap - yCoordinateTop);
                    Image<Bgr, byte> tmpImage = theImage.Copy();
                    tmpImage.Draw(new Rectangle(ptUperLeft, barSize), colorMagenta, -1);
                    theFont.thickness = 2;
                    tmpImage.Draw(binCenter.ToString("e"), ref theFont, new Point(mouseClickX - 40, pictureHeight + 30 - serviceSpaceGap), colorBlack);
                    tmpImage.Draw((probDens).ToString("e"), ref theFont, new Point(serviceSpaceGap + 4, yCoordinateTop), colorBlack);
                    theFont.thickness = 1;
                    ThreadSafeOperations.UpdatePictureBox(pbHistRepresentation, tmpImage.Bitmap, false);
                    break;
                }
            }
        }

        private void tbQuantilesCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)// Enter key
            {
                if (Convert.ToInt32(tbQuantilesCount.Text) > 0)
                {
                    histToRepresent.quantilesCount = Convert.ToInt32(tbQuantilesCount.Text);
                }

                Represent();
            }
        }

        private void rbtnShowAsDots_CheckedChanged(object sender, EventArgs e)
        {
            Represent();
        }

        private void cbShowQuantiles_CheckedChanged(object sender, EventArgs e)
        {
            Represent();
        }





        private void btnSaveImageToFile_Click(object sender, EventArgs e)
        {
            if (theImage != null)
            {
                string curOutputDir = (string) defaultProperties["DefaultDataFilesLocation"];
                string currFileName = "m" + Path.GetRandomFileName().Replace(".", "");
                if (defaultProperties.Keys.Contains("CurrentFileName"))
                {
                    currFileName = Path.GetFileNameWithoutExtension((string) defaultProperties["CurrentFileName"]);
                }
                int i = 0;
                string fName = curOutputDir + currFileName + "-grixHist-" + String.Format("{0:000}", i) + ".jpg";
                while (File.Exists(fName))
                {
                    i++;
                    fName = curOutputDir + currFileName + "-grixHist-" + String.Format("{0:000}", i) + ".jpg";
                }
                theImage.Save(fName);
            }
        }




        private void btnSaveDataToFile_Click(object sender, EventArgs e)
        {
            if (theImage != null)
            {
                string curOutputDir = (string)defaultProperties["DefaultDataFilesLocation"];
                string currFileName = "m" + Path.GetRandomFileName().Replace(".", "");
                if (defaultProperties.Keys.Contains("CurrentFileName"))
                {
                    currFileName = Path.GetFileNameWithoutExtension((string)defaultProperties["CurrentFileName"]);
                }
                int i = 0;
                string fName = curOutputDir + currFileName + "-grixHist-" + String.Format("{0:000}", i) + ".csv";
                while (File.Exists(fName))
                {
                    i++;
                    fName = curOutputDir + currFileName + "-grixHist-" + String.Format("{0:000}", i) + ".csv";
                }
                histToRepresent.SaveHistDataToASCII(fName);
            }
        }




    }
}
