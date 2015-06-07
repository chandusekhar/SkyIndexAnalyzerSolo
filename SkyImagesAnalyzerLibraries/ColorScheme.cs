using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;


namespace SkyImagesAnalyzerLibraries
{
    public class ColorScheme
    {
        //private List<object> objectsToDispose = new List<object>();
        public List<Bgr> colorsArray = new List<Bgr>();
        public bool isSchemeGrayscaled = false;
        public bool isColorSchemeSymmetric = false;

        public ColorScheme(String fullFilename = "")
        {
            bool startedProcessColors = false;
            isSchemeGrayscaled = false;
            isColorSchemeSymmetric = false;

            if (fullFilename == "")
            {
                fullFilename = Directory.GetCurrentDirectory() + "\\" + "matlab_jet.rgb";
            }

            string depth = "byte";
            double multiplier = 1.0d;

            String[] lines = File.ReadAllLines(fullFilename);
            foreach (String line in lines)
            {
                if (line.Substring(0, 6) == "depth=")
                {
                    //depth=double
                    //depth=byte
                    //
                    depth = line.Substring(6);
                    if (depth == "double")
                    {
                        multiplier = 255.0d;
                    }
                }
                if (!startedProcessColors && (line[0] == '#'))
                {
                    startedProcessColors = true; continue;
                }
                if (!startedProcessColors) continue;

                String str = line.ToString();
                str = str.Trim();
                while (str.Contains("  ")) str = str.Replace("  ", " ");


                String[] fields = str.Split(' ');
                colorsArray.Add(new Bgr(
                    multiplier * Convert.ToDouble(fields[2].Replace(".", ",")),
                    multiplier * Convert.ToDouble(fields[1].Replace(".", ",")),
                    multiplier * Convert.ToDouble(fields[0].Replace(".", ","))));
            }

            //objectsToDispose.Add(colorsArray);
        }





        public ColorScheme Reverse()
        {
            ColorScheme retScheme = new ColorScheme("");
            List<Bgr> reversedColorsArray = new List<Bgr>(this.colorsArray);
            reversedColorsArray.Reverse();
            retScheme.colorsArray = reversedColorsArray;
            retScheme.isSchemeGrayscaled = isSchemeGrayscaled;
            retScheme.isColorSchemeSymmetric = isColorSchemeSymmetric;
            return retScheme;
        }




        public ColorScheme(bool Grayscaled = true)
        {
            isSchemeGrayscaled = Grayscaled;
            isColorSchemeSymmetric = false;
            //if (!isSchemeGrayscaled)
            //{
            //    throw new Exception();
            //}
        }




        /// <summary>
        /// Returns the color by the value and ruler range
        /// </summary>
        /// <param name="dValue">The value.</param>
        /// <param name="minVal">The minimum ruler value.</param>
        /// <param name="maxVal">The maximum ruler value.</param>
        /// <returns>Bgr. The returning color</returns>
        public Bgr GetColorByValueAndRange(double dValue, double minVal, double maxVal)
        {
            if (double.IsNaN(dValue)) return new Bgr(0, 0, 0);

            if (minVal == maxVal) return new Bgr(0, 0, 0);

            if (!isSchemeGrayscaled)
            {
                int rangesCount = colorsArray.Count();
                double diffVal = (maxVal - minVal) / rangesCount;
                int colNum = Convert.ToInt32(Math.Truncate((dValue - minVal) / diffVal));
                colNum = (colNum == rangesCount) ? (colNum - 1) : colNum;
                if ((colNum < 0) || (colNum >= rangesCount))
                {
                    return new Bgr(0, 0, 0);
                }
                return colorsArray[colNum];
            }
            else return new Bgr(255.0d * dValue / (maxVal - minVal), 255.0d * dValue / (maxVal - minVal), 255.0d * dValue / (maxVal - minVal));
        }



        public static ColorScheme BinaryCloudSkyColorScheme(double marginValue, double minVal, double maxVal)
        {
            Bgr skyColor = new Bgr(255, 0, 0);
            Bgr CloudColor = new Bgr(255, 255, 255);
            int count = 1000;

            ColorScheme retColorScheme = new ColorScheme(false);
            retColorScheme.isColorSchemeSymmetric = false;
            retColorScheme.colorsArray.Clear();

            for (int i = 0; i < count; i++)
            {
                double curValue = minVal + (double)i * (maxVal - minVal) / Convert.ToDouble(count);
                if (curValue > marginValue) retColorScheme.colorsArray.Add(skyColor);
                else retColorScheme.colorsArray.Add(CloudColor);
            }
            return retColorScheme;
        }




        public static ColorScheme InversedBinaryCloudSkyColorScheme(double marginValue, double minVal, double maxVal)
        {
            Bgr skyColor = new Bgr(255, 0, 0);
            Bgr CloudColor = new Bgr(255, 255, 255);
            int count = 1000;

            ColorScheme retColorScheme = new ColorScheme(false);
            retColorScheme.isColorSchemeSymmetric = false;
            retColorScheme.colorsArray.Clear();

            for (int i = 0; i < count; i++)
            {
                double curValue = minVal + (double)i * (maxVal - minVal) / Convert.ToDouble(count);
                if (curValue < marginValue) retColorScheme.colorsArray.Add(skyColor);
                else retColorScheme.colorsArray.Add(CloudColor);
            }
            return retColorScheme;
        }




        public ColorScheme(String fullFilename = "", bool isSymmetric = true)
        {
            isColorSchemeSymmetric = isSymmetric;
            bool startedProcessColors = false;
            isSchemeGrayscaled = false;

            if (fullFilename == "")
            {
                fullFilename = Directory.GetCurrentDirectory() + "\\" + "matlab_jet.rgb";
            }

            String[] lines = File.ReadAllLines(fullFilename);
            foreach (String line in lines)
            {
                if (!startedProcessColors && (line[0] == '#')) { startedProcessColors = true; continue; }
                if (!startedProcessColors) continue;

                String str = line.ToString();
                str = str.Trim();
                while (str.Contains("  ")) str = str.Replace("  ", " ");


                String[] fields = str.Split(' ');
                colorsArray.Add(new Bgr(Convert.ToDouble(fields[2]), Convert.ToDouble(fields[1]), Convert.ToDouble(fields[0])));
            }


            if (isColorSchemeSymmetric)
            {
                Bgr[] tmpArrayOfColors = new Bgr[colorsArray.Count];
                colorsArray.CopyTo(tmpArrayOfColors);
                for (int i = 1; i <= tmpArrayOfColors.Length; i++)
                {
                    colorsArray.Add(tmpArrayOfColors[tmpArrayOfColors.Length - i]);
                }
            }
        }
    }


    /// <summary>
    /// Class ColorSchemeRuler.
    /// Описывает цветовую линейку для данных
    /// </summary>
    public class ColorSchemeRuler
    {
        //private List<object> objectsToDispose = new List<object>();
        public double minValue = -255.0d;
        public double maxValue = 255.0d;
        private ColorScheme colorScheme;
        public double markerPositionValue = 255.0d;
        public imageConditionAndData imgToRule;
        private Image<Gray, double> HighlightMask;
        private Image<Gray, double> HighlightMaskRelative;
        private PictureBoxSelection selection;
        private bool isMarginsFixed = false;
        private int currentDimX = 70, currentDimY = 200;



        /// <summary>
        /// DEPRECATED - use ColorSchemeRuler(imageConditionAndData imgData) instead
        /// </summary>
        /// <param name="currentColorScheme">The current color scheme.</param>
        /// <param name="inMinValue">The minimum data value.</param>
        /// <param name="inMaxValue">The maximum data value.</param>
        public ColorSchemeRuler(ColorScheme currentColorScheme, double inMinValue = -255.0d, double inMaxValue = 255.0d)
        {
            minValue = inMinValue;
            maxValue = inMaxValue;
            colorScheme = currentColorScheme;
            markerPositionValue = inMaxValue;
            selection = null;
            imgToRule = null;
            isMarginsFixed = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSchemeRuler"/> class using the imgData
        /// </summary>
        /// <param name="imgData">The image data and condition.</param>
        public ColorSchemeRuler(imageConditionAndData imgData)
        {
            minValue = imgData.dataMinValue();
            maxValue = imgData.dataMaxValue();
            if (imgData.currentColorScheme.isColorSchemeSymmetric)
            {
                minValue = -Math.Max(Math.Abs(minValue), Math.Abs(maxValue));
                maxValue = Math.Max(Math.Abs(minValue), Math.Abs(maxValue));
            }
            colorScheme = imgData.currentColorScheme;
            markerPositionValue = maxValue;
            selection = null;
            imgToRule = imgData;
            //isMarginsFixed = imgData.isColorSchemeMarginsFixed;
            isMarginsFixed = false;
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSchemeRuler"/> class.
        /// With the forced maximum and minimum values
        /// </summary>
        /// <param name="imgData">The image data and condition.</param>
        /// <param name="minValueIn">The forced minimum value.</param>
        /// <param name="maxValueIn">The forced maximum value.</param>
        public ColorSchemeRuler(imageConditionAndData imgData, double minValueIn, double maxValueIn)
        {
            minValue = minValueIn;
            if ((!double.IsNaN(imgData.DmSourceData.Values.Min())) && (minValue >= imgData.DmSourceData.Values.Min())) minValue = imgData.DmSourceData.Values.Min();

            maxValue = maxValueIn;
            if ((!double.IsNaN(imgData.DmSourceData.Values.Max())) && (maxValue <= imgData.DmSourceData.Values.Max())) maxValue = imgData.DmSourceData.Values.Max();

            colorScheme = imgData.currentColorScheme;
            markerPositionValue = maxValue;
            selection = null;
            imgToRule = imgData;
            //isMarginsFixed = imgData.isColorSchemeMarginsFixed;
            isMarginsFixed = true;
        }



        public ColorSchemeRuler(ColorSchemeRuler sourceRuler)
        {
            minValue = sourceRuler.minValue;
            maxValue = sourceRuler.maxValue;
            colorScheme = sourceRuler.colorScheme;
            markerPositionValue = maxValue;
            selection = null;
            imgToRule = null;
            //isMarginsFixed = imgData.isColorSchemeMarginsFixed;
            isMarginsFixed = sourceRuler.isMarginsFixed;
        }





        public ColorSchemeRuler Copy()
        {
            return new ColorSchemeRuler(this);
        }




        /// <summary>
        /// Gets or sets the selection property.
        /// </summary>
        /// <value>
        /// The selection.
        /// </value>
        public PictureBoxSelection Selection
        {
            get { return selection; }
            set
            {
                if ((Convert.ToDouble(ServiceTools.getPropValue(value.SelectionRectReal, "Height")) == 0) ||
                    (Convert.ToDouble(ServiceTools.getPropValue(value.SelectionRectReal, "Width")) == 0))
                {
                    selection = null;
                    HighlightMask = null;
                    HighlightMaskRelative = null;
                }
                else
                {
                    selection = value;
                }
                HighlightMask = null;
            }
        }

        public bool IsMarginsFixed
        {
            get { return isMarginsFixed; }
            set
            {
                bool oldValue = isMarginsFixed;
                isMarginsFixed = value;
                if (oldValue != isMarginsFixed)
                {
                    imgToRule.UpdateColorSchemeRuler();
                }
            }
        }

        /// <summary>
        /// Clears the selection property so there will be no selected area at the ruler reapresenting image
        /// and there will be no selection data if something requests it
        /// </summary>
        public void ClearSelection()
        {
            selection = null;
            HighlightMask = null;
        }


        /// <summary>
        /// Формирует bitmap, отображающий линейку для представления ее визуально на форме.
        /// </summary>
        /// <param name="dimX">The x dimension to form the ruler bitmap</param>
        /// <param name="dimY">The y dimension to form the ruler bitmap</param>
        /// <param name="withMarker">if set to <c>true</c> then there will be little triangle marker displayed at the marker position.</param>
        /// <returns></returns>
        public Bitmap RulerBitmap(int dimX, int dimY, bool withMarker = true)
        {
            currentDimX = dimX;
            currentDimY = dimY;
            DenseMatrix dmRulerDenseMatrix = null;


            //if (!colorScheme.isSchemeGrayscaled)
            //{
            //Bitmap scaleBM = new Bitmap(currentDimX, currentDimY);
            //int nodesCount = colorScheme.colorsArray.Count;


            //Image<Bgr, byte> scaleImage = new Image<Bgr, byte>(scaleBM);
            //dmRulerDenseMatrix = ImageProcessing.DenseMatrixFromImage(scaleBM);

            dmRulerDenseMatrix = DenseMatrix.Create(currentDimY, currentDimX, new Func<int, int, double>
                (
                (y, x) =>
                {
                    double curValue = (double)y;
                    curValue = 1.0d - (1.0d + curValue) / (double)currentDimY;
                    curValue = minValue + curValue * (maxValue - minValue);
                    return curValue;
                }
                ));

            //for (int i = 0; i < dmRulerDenseMatrix.RowCount; i++)
            //{
            //    double curValue = (double)i;
            //    curValue = 1.0d - (1.0d + curValue) / (double)currentDimY;
            //    curValue = minValue + curValue * (maxValue - minValue);
            //    DenseVector tmpDenseVector = DenseVector.Create(dmRulerDenseMatrix.ColumnCount, i1 => curValue);
            //    dmRulerDenseMatrix.SetRow(i, tmpDenseVector);
            //}
            Image<Bgr, double> scaleImage = ImageProcessing.evalResultColored(dmRulerDenseMatrix, null, colorScheme).Convert<Bgr, double>();


            if (withMarker)
            {
                double yPositionDouble = (1.0d - ((markerPositionValue - minValue) / (maxValue - minValue))) * (double)currentDimY;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                Point[] polyLine = new Point[] { new Point(currentDimX - 10, yPosition), new Point(currentDimX, yPosition - 5), new Point(currentDimX, yPosition + 5) };
                Bgr markerColor = ImageProcessing.ContrastColorFromBgrColor(colorScheme.GetColorByValueAndRange(markerPositionValue, minValue, maxValue));
                scaleImage.DrawPolyline(polyLine, true, markerColor, 1);
                scaleImage.FillConvexPoly(polyLine, markerColor);
            }


            if (selection != null)
            {
                Image<Bgr, double> tmpHighlightingImage = scaleImage.CopyBlank();
                tmpHighlightingImage.Draw(RectangleOnImageFromRealValuesRectangle((RectangleF)(selection.SelectionRectReal), scaleImage), new Bgr(255, 255, 255), -1);
                scaleImage = scaleImage.AddWeighted(tmpHighlightingImage, 0.8, 0.3, 0.0);
                scaleImage.Draw(RectangleOnImageFromRealValuesRectangle((RectangleF)(selection.SelectionRectReal), scaleImage), new Bgr(255, 255, 255), 1);
            }

            if (HighlightMaskRelative != null)
            {
                scaleImage = scaleImage.Mul(ImageProcessing.ConvertGrayImageToBgr(HighlightMaskRelative));

            }


            #region Прописываем текстовые маркеры
            MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);
            theFont.thickness = 2;
            double dMarkersCount = (double)currentDimY / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            //int markersCount = Convert.ToInt32(Math.Truncate(dMarkersCount));
            double dRulerValueGap = (maxValue - minValue) / (double)dMarkersCount;
            dRulerValueGap = (dRulerValueGap < 1.0d) ? (1.0d) : dRulerValueGap;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = minValue - Math.IEEERemainder(minValue, rulerValueGap);
            lowerMarkerValue = (lowerMarkerValue < minValue) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);
            double currentMarkerValue = lowerMarkerValue;
            while (currentMarkerValue < maxValue)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue - minValue) / (maxValue - minValue))) * (double)currentDimY;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(0, yPosition), new Point(5, yPosition));
                //Point[] polyLine = new Point[] { new Point(currentDimX - 10, yPosition), new Point(currentDimX, yPosition - 5), new Point(currentDimX, yPosition + 5) };
                Bgr markerColor = ImageProcessing.ContrastColorFromBgrColor(colorScheme.GetColorByValueAndRange(currentMarkerValue, minValue, maxValue));
                scaleImage.Draw(theLine, markerColor, 2);
                String message = Math.Round(currentMarkerValue, 2).ToString();



                scaleImage.Draw(message, ref theFont, new Point(5, yPosition), markerColor);

                currentMarkerValue += rulerValueGap;
            }
            #endregion Прописываем текстовые маркеры

            Bitmap scaleBM = new Bitmap(scaleImage.Bitmap);

            return scaleBM;

            //}
            //else
            //{
            //    Bitmap scaleBM = new Bitmap(currentDimX, dimY);
            //    Image<Bgr, byte> scaleImage = new Image<Bgr, byte>(scaleBM);
            //    for (int i = 1; i <= dimY; i++)
            //    {
            //        int yLow = dimY - i;
            //        Rectangle curRect = new Rectangle(0, yLow, currentDimX, 1);
            //        scaleImage.Draw(curRect, colorScheme.GetColorByValueAndRange(((double)i / (double)dimY) * (maxValue - minValue), minValue, maxValue), 0);
            //    }
            //
            //    if (withMarker)
            //    {
            //        double yPositionDouble = (1.0d - (markerPositionValue / (maxValue - minValue))) * (double)dimY;
            //        int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
            //        Point[] polyLine = new Point[] { new Point(currentDimX - 10, yPosition), new Point(currentDimX, yPosition - 5), new Point(currentDimX, yPosition + 5) };
            //        Bgr markerColor = ImageProcessing.ContrastColorFromBgrColor(colorScheme.GetColorByValueAndRange(markerPositionValue, minValue, maxValue));
            //        scaleImage.DrawPolyline(polyLine, true, markerColor, 1);
            //        scaleImage.FillConvexPoly(polyLine, markerColor);
            //    }
            //
            //
            //    if (selection != null)
            //    {
            //        Image<Bgr, byte> tmpHighlightingImage = scaleImage.CopyBlank();
            //        tmpHighlightingImage.Draw(RectangleOnImageFromRealValuesRectangle((RectangleF)(selection.SelectionRectReal), scaleBM), new Bgr(255, 255, 255), -1);
            //        scaleImage = scaleImage.AddWeighted(tmpHighlightingImage, 1.0, 0.3, 0.0);
            //        scaleImage.Draw(RectangleOnImageFromRealValuesRectangle((RectangleF)(selection.SelectionRectReal), scaleBM), new Bgr(255, 255, 255), 1);
            //    }
            //
            //
            //    scaleBM = new Bitmap(scaleImage.Bitmap);
            //
            //    return scaleBM;
            //}

        }


        /// <summary>
        /// Gets the ruler value by click event position
        /// </summary>
        /// <param name="sender">The sender PictureBox</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the mouse event data.</param>
        /// <returns></returns>
        public double GetValueByClickEvent(PictureBox sender, MouseEventArgs e)
        {
            int mouseClickY = e.Y;
            return (1.0d - ((double)mouseClickY / (double)(sender).Height)) * (maxValue - minValue) + minValue;
        }


        public double GetValueByClickEvent(PictureBox sender, Point p)
        {
            int mouseClickY = p.Y;
            return (1.0d - ((double)mouseClickY / (double)(sender).Height)) * (maxValue - minValue) + minValue;
        }



        /// <summary>
        /// Gets the value by point position.
        /// Note the point is in the objContainer coordinates
        /// </summary>
        /// <param name="pt">The point in the objContainer coordinates</param>
        /// <param name="objContainer">The object container (image, bitmap, PictureBox or other object with the "Height" property defined)</param>
        /// <returns></returns>
        public double GetValueByPointPosition(Point pt, object objContainer)
        {
            int mouseClickY = pt.Y;
            return (1.0d - ((double)mouseClickY / (double)(ServiceTools.getPropValue(objContainer, "Height"))) * (maxValue - minValue) + minValue);
        }


        private int getPointPositionY_ByValue(float value, object objContainer)
        {
            //int mouseClickY = pt.Y;
            double H = Convert.ToDouble(ServiceTools.getPropValue(objContainer, "Height"));
            double dReal = (maxValue - minValue);
            double y = H - (H / dReal) * (value - minValue);
            return Convert.ToInt32(y);
        }

        private int getPointPositionY_ByValue(double value, object objContainer)
        {
            //int mouseClickY = pt.Y;
            double H = Convert.ToDouble(ServiceTools.getPropValue(objContainer, "Height"));
            double dReal = (maxValue - minValue);
            double y = H - (H / dReal) * (value - minValue);
            return Convert.ToInt32(y);
        }


        private Rectangle RectangleOnImageFromRealValuesRectangle(RectangleF rectF2convert, object objContainer)
        {
            Rectangle rectOut = new Rectangle();
            rectOut.X = Convert.ToInt32(rectF2convert.X);
            rectOut.Y = getPointPositionY_ByValue(rectF2convert.Location.Y, objContainer);
            rectOut.Width = Convert.ToInt32(rectF2convert.Width);
            rectOut.Height = Convert.ToInt32((Convert.ToDouble(ServiceTools.getPropValue(objContainer, "Height")) * (double)(rectF2convert.Height) / (maxValue - minValue)));

            return rectOut;
        }


        public void makeHihghlightMask()
        {
            if (imgToRule.Selection == null)
            {
                return;
            }

            imageConditionAndData tmpImgData = imgToRule.SelectedImageData();
            double filterMaxValue = tmpImgData.DmSourceData.Values.Max();
            double filterMinValue = tmpImgData.DmSourceData.Values.Min();
            selection = null;


            DenseMatrix dmHighlightDenseMatrix = DenseMatrix.Create(currentDimY, currentDimX, new Func<int, int, double>
                (
                (y, x) =>
                {
                    double curValue = (double)y;
                    curValue = 1.0d - (1.0d + curValue) / (double)currentDimY;
                    curValue = minValue + curValue * (maxValue - minValue);
                    if ((curValue >= filterMinValue) && (curValue <= filterMaxValue))
                    {
                        return 255.0d;
                    }
                    else
                    {
                        return 100.0d;
                    }
                }
                ));
            Image<Gray, Byte> highlightImage = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmHighlightDenseMatrix, 0.0d, 255.0d);


            HighlightMask = highlightImage.Convert<Gray, double>();
            HighlightMaskRelative = HighlightMask / 255.0d;
            //ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(tmpDM, 0.0d, 255.0d).Convert<Gray, double>();
            //HighlightMask = HighlightMask / 255.0d;
        }


        public int Width
        {
            get { return currentDimX; }
        }



        public int Height
        {
            get { return currentDimY; }
        }
    }
}
