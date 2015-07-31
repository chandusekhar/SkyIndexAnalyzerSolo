using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace SkyImagesAnalyzerLibraries
{
    public class ConnectedObjectsDetection
    {
        //private Bitmap processingBitmap;
        private Image<Bgr, Byte> processingImage;
        private int dimX = 0, dimY = 0;
        private TextBox tbLog;
        //private double skyindexSeparationValue = 0.0d;
        public List<VectorOfPoint> contoursArray;
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


            // Contour<Point> contoursDetected = imgSkyIndexDataBinary.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
            VectorOfVectorOfPoint contoursDetected = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(imgSkyIndexDataBinary, contoursDetected, null, Emgu.CV.CvEnum.RetrType.List,
                Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            contoursArray = new List<VectorOfPoint>();
            int count = contoursDetected.Size;
            var colorGen = new RandomPastelColorGenerator();
            for (int i = 0; i < count; i++)
            {
                Color currentColor = colorGen.GetNext();
                var currentColorBgr = new Bgr(currentColor.B, currentColor.G, currentColor.R);
                using (VectorOfPoint currContour = contoursDetected[i])
                {
                    contoursArray.Add(currContour);
                    previewImage.Draw(currContour, 0, currentColorBgr, -1); //.Draw(currContour, currentColorBgr, -1);
                }
            }
            
            ThreadSafeOperations.SetTextTB(tbLog, "Количество выделенных объектов: " + contoursArray.Count + Environment.NewLine, true);


            //ShowImageForm ImgShow = new ShowImageForm(localPreviewBitmap, ParentForm, this);
            var imgShow = new SimpleShowImageForm(previewImage);
            imgShow.Show();
        }
    }
}