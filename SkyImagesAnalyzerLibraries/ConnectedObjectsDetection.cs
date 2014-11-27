using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using Emgu.CV;
using Emgu.CV.Structure;



namespace SkyImagesAnalyzerLibraries
{
    public class ConnectedObjectsDetection
    {
        //private Bitmap processingBitmap;
        private Image<Bgr, Byte> processingImage;
        private int dimX = 0, dimY = 0;
        private TextBox tbLog;
        //private double skyindexSeparationValue = 0.0d;
        public List<Contour<Point>> contoursArray;
        private Dictionary<string, object> defaultProperties = null;



        //public ConnectedObjectsDetection(Image origImage, PictureBox displayControl, ProgressBar pbControl, TextBox inReportingTextBox, Dictionary<string, object> settings)
        public ConnectedObjectsDetection(Image<Bgr, Byte> origImage, PictureBox displayControl, ProgressBar pbControl, TextBox inReportingTextBox, Dictionary<string, object> settings)
        {
            //var bm1 = new Bitmap(origImage);
            processingImage = origImage.Copy();
            dimX = processingImage.Width;
            dimY = processingImage.Height;
            //previewBitmap = new Bitmap(processingBitmap);
            tbLog = inReportingTextBox;
            defaultProperties = settings;
            //skyindexSeparationValue = skyCloudSeparationValue;
        }





        public void DetectConnected()
        {
            var classificator = new SkyCloudClassification(processingImage, defaultProperties);
            classificator.Classify();
            DenseMatrix dmSkyIndexDataBinary = classificator.dmSkyIndexDataBinary();
            Image<Gray, Byte> imgSkyIndexDataBinary = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmSkyIndexDataBinary, 0.0d, 1.0d, true);
            imgSkyIndexDataBinary = imgSkyIndexDataBinary.Mul(classificator.maskImage);
            Image<Bgr, Byte> previewImage = imgSkyIndexDataBinary.CopyBlank().Convert<Bgr, Byte>();

            Contour<Point> contoursDetected = imgSkyIndexDataBinary.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
            contoursArray = new List<Contour<Point>>();

            var colorGen = new RandomPastelColorGenerator();
            while (true)
            {
                Color currentColor = colorGen.GetNext();
                var currentColorBgr = new Bgr(currentColor.B, currentColor.G, currentColor.R);
                Contour<Point> currContour = contoursDetected;
                contoursArray.Add(currContour);
                previewImage.Draw(currContour, currentColorBgr, -1);

                contoursDetected = contoursDetected.HNext;
                if (contoursDetected == null)
                    break;
            }

            ThreadSafeOperations.SetTextTB(tbLog, "Количество выделенных объектов: " + contoursArray.Count + Environment.NewLine, true);


            //ShowImageForm ImgShow = new ShowImageForm(localPreviewBitmap, ParentForm, this);
            var imgShow = new SimpleShowImageForm(previewImage);
            imgShow.Show();
        }
    }
}