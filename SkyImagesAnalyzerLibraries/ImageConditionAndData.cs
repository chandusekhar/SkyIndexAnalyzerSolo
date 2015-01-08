using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SkyImagesAnalyzerLibraries
{
    public class imageConditionAndData
    {
        public bool isImageGrayscaled = true;
        public Image<Gray, Byte> sourceImageGrayscale;
        public Image<Bgr, Byte> sourceImageBgr;
        public Image<Gray, Byte> maskImageBinary;
        public Image<Gray, Byte> thresholdedImage;
        private double thresholdingValue;
        private MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmSourceData;//DenseMatrix
        private bool thresholdingUsageTop;
        private bool thresholdingUsageBtm;
        public ColorScheme currentColorScheme;
        public ColorSchemeRuler currentColorSchemeRuler;
        //public bool isColorSchemeMarginsFixed = false;
        private PictureBoxSelection selection;
        public Image<Gray, double> HighlightMask;
        public double highlightedArea;
        //private Image<Gray, double> HighlightMaskBinary;
        public bool dataHasBeenModified;// = false;
        private Image<Bgr, Byte> imageRepresentingImage = null;

        public List<PointD> lPtdMarks = new List<PointD>();



        public int Width
        {
            get { return dmSourceData.ColumnCount; }
        }



        public int Height
        {
            get { return dmSourceData.RowCount; }
        }



        public double ThresholdingValue
        {
            get { return thresholdingValue; }
            set
            {
                thresholdingValue = value;
                //makeThresholdedDataImage();
                dataHasBeenModified = true;
            }
        }

        public bool ThresholdingUsageTop
        {
            get { return thresholdingUsageTop; }
            set
            {
                thresholdingUsageTop = value;
                //makeThresholdedDataImage();
                dataHasBeenModified = true;
            }
        }

        public bool ThresholdingUsageBtm
        {
            get { return thresholdingUsageBtm; }
            set
            {
                thresholdingUsageBtm = value;
                //makeThresholdedDataImage();
                dataHasBeenModified = true;
            }
        }

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
                    highlightedArea = 0.0d;
                    //HighlightMaskBinary = null;
                }
                else
                {
                    selection = value;
                }
                HighlightMask = null;
                highlightedArea = 0.0d;
                //makeThresholdedDataImage();
                dataHasBeenModified = true;
            }
        }

        public DenseMatrix DmSourceData
        {
            get
            {
                if (selection != null)
                {
                    MathNet.Numerics.LinearAlgebra.Double.DenseMatrix tmpDM = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmSourceData.Clone();
                    return tmpDM;
                }
                else if (HighlightMask != null)
                {
                    MathNet.Numerics.LinearAlgebra.Double.DenseMatrix tmpDM = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmSourceData.Clone();
                    return tmpDM;
                }

                return dmSourceData;
            }
            set
            {
                dmSourceData = value;
                dataHasBeenModified = true;
            }
        }


        public void ClearSelection()
        {
            selection = null;
            HighlightMask = null;
            highlightedArea = 0.0d;
            dataHasBeenModified = true;
        }


        public double dataMinValue()
        {
            return dmSourceData.Values.Min();
        }


        public double dataMaxValue()
        {
            return dmSourceData.Values.Max();
        }


        /// <summary>
        /// Updates the color scheme ruler for the class instance
        /// taking into account the fact whether the ruler has fixed values margins or not
        /// </summary>
        public void UpdateColorSchemeRuler()
        {
            if (currentColorSchemeRuler.IsMarginsFixed)
            {
                currentColorSchemeRuler = new ColorSchemeRuler(this, currentColorSchemeRuler.minValue, currentColorSchemeRuler.maxValue);
            }
            else
            {
                currentColorSchemeRuler = new ColorSchemeRuler(this);
            }
            dataHasBeenModified = true;
        }


        //attention!
        //int channelNum - zero-based channel number with B-G-R scheme
        public imageConditionAndData(Bitmap coloredBitmap, int channelNum)
        {
            ImageProcessing imgPr = new ImageProcessing(coloredBitmap, true);
            maskImageBinary = imgPr.significantMaskImageBinary;
            //imgPr.Dispose();
            isImageGrayscaled = true;
            sourceImageGrayscale = new Image<Bgr, Byte>(coloredBitmap)[channelNum];
            sourceImageBgr = null;
            sourceImageGrayscale = sourceImageGrayscale.Mul(maskImageBinary);

            thresholdingUsageTop = false;
            thresholdingUsageBtm = true;
            thresholdingValue = 255;

            //Image<Gray, Byte> thresholdedImage = imgPr.getMaskedImageChannelBitmapThresholded(channelNum, thresholdingValue, thresholdingUsageTop, thresholdingUsageBtm);
            dmSourceData = ImageProcessing.DenseMatrixFromImage(sourceImageGrayscale);

            currentColorScheme = new ColorScheme("");
            currentColorSchemeRuler = new ColorSchemeRuler(this, dataMinValue(), dataMaxValue());
            selection = null;

            //makeThresholdedDataImage();
            dataHasBeenModified = true;

            HighlightMask = null;
            highlightedArea = 0.0d;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="imageConditionAndData"/> class.
        /// note: grayscaled, with fixed grayscale ruler 0.0d-255.0d
        /// </summary>
        /// <param name="imgPr">The img pr.</param>
        /// <param name="channelNum">The channel number.</param>
        public imageConditionAndData(ImageProcessing imgPr, int channelNum)
        {
            maskImageBinary = imgPr.significantMaskImageBinary.Copy();
            isImageGrayscaled = true;
            sourceImageGrayscale = imgPr.getMaskedImageChannelImage(channelNum);
            sourceImageBgr = null;

            thresholdingUsageTop = false;
            thresholdingUsageBtm = true;
            thresholdingValue = 255;

            dmSourceData = ImageProcessing.DenseMatrixFromImage(sourceImageGrayscale);


            currentColorScheme = new ColorScheme("");
            currentColorSchemeRuler = new ColorSchemeRuler(this, dataMinValue(), dataMaxValue());
            selection = null;


            //makeThresholdedDataImage();
            dataHasBeenModified = true;

            HighlightMask = null;
            highlightedArea = 0.0d;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="imageConditionAndData"/> class.
        /// note: with the fixed ruler bounds
        /// </summary>
        /// <param name="imgPr">The image processing object contains image data and some processing templates.</param>
        public imageConditionAndData(ImageProcessing imgPr)
        {
            maskImageBinary = imgPr.significantMaskImageBinary.Copy();
            isImageGrayscaled = false;
            sourceImageBgr = imgPr.tmpImage.Copy();
            sourceImageGrayscale = null;

            thresholdingUsageTop = false;
            thresholdingUsageBtm = true;
            thresholdingValue = 255;

            dmSourceData = ImageProcessing.DenseMatrixFromImage(sourceImageBgr.Convert<Gray, Byte>());

            currentColorScheme = new ColorScheme("");
            //currentColorSchemeRuler = new ColorSchemeRuler(currentColorScheme, dataMinValue(), dataMaxValue());
            currentColorSchemeRuler = new ColorSchemeRuler(this, -255.0d, 255.0d);
            selection = null;

            //makeThresholdedDataImage();
            dataHasBeenModified = true;

            HighlightMask = null;
            highlightedArea = 0.0d;
        }




        public imageConditionAndData(DenseMatrix inputDenseMatrix, Image<Gray, Byte> binaryMaskImage)
        {
            if (binaryMaskImage == null)
            {
                maskImageBinary = new Image<Gray, byte>(inputDenseMatrix.ColumnCount, inputDenseMatrix.RowCount);
                maskImageBinary.SetValue(new Gray(1));
            }
            else
            {
                maskImageBinary = binaryMaskImage.Copy();
            }
            isImageGrayscaled = false;
            //sourceImageBgr = imgPr.tmpImage.Copy();
            //sourceImageGrayscale = null;

            thresholdingUsageTop = false;
            thresholdingUsageBtm = true;
            thresholdingValue = 255;

            dmSourceData = inputDenseMatrix;
            selection = null;

            currentColorScheme = new ColorScheme("");
            //makeThresholdedDataImage();
            dataHasBeenModified = true;

            HighlightMask = null;
            highlightedArea = 0.0d;
        }



        public imageConditionAndData(object inputDenseMatrix, Image<Gray, Byte> binaryMaskImage = null)
        {
            if (inputDenseMatrix.GetType() != typeof(DenseMatrix))
            {
                return;
            }
            dmSourceData = (DenseMatrix)((DenseMatrix)inputDenseMatrix).Clone();
            if (binaryMaskImage == null)
            {
                maskImageBinary = new Image<Gray, byte>(dmSourceData.ColumnCount, dmSourceData.RowCount);
                maskImageBinary.SetValue(new Gray(1));
            }
            else
            {
                maskImageBinary = binaryMaskImage.Copy();
            }
            isImageGrayscaled = false;
            //sourceImageBgr = imgPr.tmpImage.Copy();
            //sourceImageGrayscale = null;

            thresholdingUsageTop = false;
            thresholdingUsageBtm = true;
            thresholdingValue = 255;

            
            selection = null;

            currentColorScheme = new ColorScheme("");
            //makeThresholdedDataImage();
            dataHasBeenModified = true;

            HighlightMask = null;
            highlightedArea = 0.0d;
        }



        /// <summary>
        /// Makes the thresholded image representing the data, using setting like
        /// double thresholdingValue
        /// bool thresholdingUsageTop
        /// bool thresholdingUsageBtm
        /// 
        /// DEPRECATED - legacy only
        /// </summary>
        public void makeThresholdedDataImage()
        {
            if (currentColorScheme == null) return;

            MathNet.Numerics.LinearAlgebra.Double.DenseMatrix tmpDM = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)(dmSourceData.Clone());
            ImageProcessing.maskedImageChannelDenseMatrixThresholdedInplace(tmpDM, maskImageBinary, (double)thresholdingValue, 0.0d, thresholdingUsageTop, thresholdingUsageBtm);

            thresholdedImage = ImageProcessing.grayscaleImageFromDenseMatrix(tmpDM);

            ServiceTools.FlushMemory(null, null);
        }



        public void makeHihghlightMask()
        {
            if (currentColorSchemeRuler.Selection == null)
            {
                return;
            }
            double maxValue = ((RectangleF)currentColorSchemeRuler.Selection.SelectionRectReal).Top;
            double minValue = maxValue - ((RectangleF)currentColorSchemeRuler.Selection.SelectionRectReal).Height;
            DenseMatrix tmpDM = (DenseMatrix) (dmSourceData.Clone());


            DenseMatrix dmToDetermineHighlightedArea = (DenseMatrix)dmSourceData.Clone();
            dmToDetermineHighlightedArea.MapInplace(
                new Func<double, double>(x => ((x < minValue) || (x > maxValue)) ? 0.0d : 1.0d));
            highlightedArea = dmToDetermineHighlightedArea.Values.Sum();

            tmpDM.MapInplace(
                new Func<double, double>((x) =>
                {
                    return (((x < minValue) || (x > maxValue)) ? 100.0d : 255.0d);
                }), MathNet.Numerics.LinearAlgebra.Zeros.AllowSkip);

            HighlightMask = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(tmpDM, 0.0d, 255.0d).Convert<Gray, double>();
            HighlightMask = HighlightMask.Mul(maskImageBinary.Convert<Gray, double>());
            HighlightMask = HighlightMask / 255.0d;

            selection = null;

            dataHasBeenModified = true;
        }




        public Bitmap thresholdedDatabitmap()
        {
            return thresholdedImage.Bitmap;
        }



        public void setGrayscaleCalculatedColorScheme()
        {
            currentColorScheme = new ColorScheme(true);
            //currentColorSchemeRuler = new ColorSchemeRuler(currentColorScheme, dataMinValue(), dataMaxValue());
            currentColorSchemeRuler = new ColorSchemeRuler(this, 0.0d, 255.0d);
            dataHasBeenModified = true;
        }



        public Bitmap dataRepresentingImageColored()
        {
            if (dataHasBeenModified)
            {
                if (isImageGrayscaled)
                {
                    Image<Gray, double> img = (ImageProcessing.grayscaleImageFromDenseMatrix(dmSourceData)).Convert<Gray, double>();

                    if (selection != null)
                    {
                        Image<Gray, double> tmpHighlightingImage = img.CopyBlank();
                        tmpHighlightingImage.Draw((Rectangle)selection.SelectionRectReal, new Gray(255), -1);
                        img = img.AddWeighted(tmpHighlightingImage, 1.0, 0.5, 0.0);
                        img.Draw((Rectangle)selection.SelectionRectReal, new Gray(255), 1);
                    }

                    if (HighlightMask != null)
                    {
                        //img = img.AddWeighted(HighlightMask, 1.0, 0.15, 0.0);

                        img = img.Mul(HighlightMask);
                    }

                    Image<Bgr, byte> imgBgr = img.Convert<Bgr, byte>();

                    if (lPtdMarks.Count > 0)
                    {
                        foreach (PointD ptd in lPtdMarks)
                        {
                            imgBgr.Draw(new CircleF(ptd.PointF(), 3.0f), new Bgr(Color.Magenta), 0);
                        }
                    }

                    dataHasBeenModified = false;
                    imageRepresentingImage = imgBgr.Copy();
                    return imageRepresentingImage.Bitmap;
                }
                else
                {
                    Image<Bgr, double> img =
                        ImageProcessing.evalResultColoredWithFixedDataBounds(dmSourceData, maskImageBinary,
                            currentColorScheme, currentColorSchemeRuler.minValue, currentColorSchemeRuler.maxValue)
                            .Convert<Bgr, double>();

                    if (selection != null)
                    {
                        Image<Bgr, double> tmpHighlightingImage = img.CopyBlank();
                        tmpHighlightingImage.Draw((Rectangle)selection.SelectionRectReal, new Bgr(255, 255, 255), -1);
                        img = img.AddWeighted(tmpHighlightingImage, 1.0, 0.3, 0.0);
                        img.Draw((Rectangle)selection.SelectionRectReal, new Bgr(255, 255, 255), 1);
                    }


                    if (HighlightMask != null)
                    {
                        //img = img.AddWeighted(HighlightMask.Convert<Bgr, Byte>(), 1.0, 1.0, 0.0);
                        Image<Bgr, double> BgrHighlightMask =
                            new Image<Bgr, double>(new Image<Gray, double>[] { HighlightMask, HighlightMask, HighlightMask });
                        img = img.Mul(BgrHighlightMask);
                    }

                    Image<Bgr, byte> imgBgr = img.Convert<Bgr, byte>();

                    if (lPtdMarks.Count > 0)
                    {
                        foreach (PointD ptd in lPtdMarks)
                        {
                            imgBgr.Draw(new CircleF(ptd.PointF(), 2.0f), new Bgr(Color.Red), 0);
                        }
                    }

                    dataHasBeenModified = false;
                    imageRepresentingImage = imgBgr.Copy();
                    return imageRepresentingImage.Bitmap;
                }
            }
            else
            {
                return imageRepresentingImage.Bitmap;
            }
            
        }






        public PointD getDataPositionByClickEvent(PictureBox sender, MouseEventArgs e)
        {
            int mouseClickX = e.X;
            int mouseClickY = e.Y;
            //dmSourceData
            if (sender.Image == null)
            {
                return new PointD(0.0d, 0.0d);
            }
            double imageDataX = (double)mouseClickX * ((double)dmSourceData.ColumnCount / (double)sender.Image.Width);
            
            double imageDataY = (double)mouseClickY * ((double)dmSourceData.ColumnCount / (double)sender.Image.Width);
            

            return new PointD(imageDataX, imageDataY);
        }



        public double GetValueByClickEvent(PictureBox sender, MouseEventArgs e, out PointD origDataPointPosition)
        {
            if (sender.Image == null)
            {
                origDataPointPosition = PointD.nullPointD();
                return 0.0d;
            }
            PointD theDataPoint = getDataPositionByClickEvent(sender, e);
            origDataPointPosition = theDataPoint;
            int dataY = Convert.ToInt32(theDataPoint.Y);
            dataY = (dataY > dmSourceData.RowCount - 1) ? (dmSourceData.RowCount - 1) : (dataY);
            dataY = (dataY < 0) ? (0) : (dataY);
            int dataX = Convert.ToInt32(theDataPoint.X);
            dataX = (dataX > dmSourceData.ColumnCount - 1) ? (dmSourceData.ColumnCount - 1) : (dataX);
            dataX = (dataX < 0) ? (0) : (dataX);

            return dmSourceData[dataY, dataX];
        }




        /// <summary>
        /// Constructs  the image from selection.
        /// </summary>
        /// <param name="theSelection">The selection.</param>
        /// <returns>imageConditionAndData.</returns>
        public imageConditionAndData SelectedImageData(PictureBoxSelection theSelection = null)
        {
            if ((theSelection == null) && (selection != null)) theSelection = selection;
            if (theSelection == null) return this;

            Rectangle theSelectionRectangle = (Rectangle) theSelection.SelectionRectReal;
            DenseMatrix subMatrix = (DenseMatrix)dmSourceData.SubMatrix(
                                                                        theSelectionRectangle.Location.Y,
                                                                        theSelectionRectangle.Height,
                                                                        theSelectionRectangle.Location.X,
                                                                        theSelectionRectangle.Width);
            Image<Gray, Byte> img = ImageProcessing.grayscaleImageFromDenseMatrix(subMatrix);

            DenseMatrix maskMatrix = ImageProcessing.DenseMatrixFromImage(maskImageBinary);
            maskMatrix = (DenseMatrix)maskMatrix.SubMatrix(
                                                            theSelectionRectangle.Location.Y,
                                                            theSelectionRectangle.Height,
                                                            theSelectionRectangle.Location.X,
                                                            theSelectionRectangle.Width);
            //Image<Gray, Byte> maskSubImageBinary = maskImageBinary.GetSubRect(theSelectionRectangle);
            Image<Gray, Byte> maskSubImageBinary = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(maskMatrix, 0.0d, 1.0d);//  maskImageBinary.GetSubRect(theSelectionRectangle);
            maskSubImageBinary = maskSubImageBinary/255;

            //Image<Gray, double> tmpHighlightingImage = img.CopyBlank();
            //tmpHighlightingImage.Draw((Rectangle)theSelection.SelectionRectReal, new Gray(255), -1);
            //img = img.AddWeighted(tmpHighlightingImage, 1.0, 0.15, 0.0);
            //img.Draw((Rectangle)theSelection.SelectionRectReal, new Gray(255), 1);
            //ImageProcessing imgPr = new ImageProcessing(img.Bitmap, false);
            //imgPr.significantMaskImageBinary = maskSubImageBinary;
            //imgPr.significantMaskImage = maskSubImageBinary*255;
            imageConditionAndData retImageData = new imageConditionAndData(subMatrix, maskSubImageBinary);
            retImageData.currentColorScheme = currentColorScheme;
            retImageData.currentColorSchemeRuler = new ColorSchemeRuler(currentColorSchemeRuler);
            retImageData.currentColorSchemeRuler.imgToRule = retImageData;
            retImageData.currentColorSchemeRuler.IsMarginsFixed = false;
            //retImageData.UpdateColorSchemeRuler();
            return retImageData;
        }
    }
}
