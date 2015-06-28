using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;




namespace SkyImagesAnalyzerLibraries
{
    public class TextBarImage
    {
        public string strText = "";
        public MCvFont fnTextFont = new MCvFont(FONT.CV_FONT_HERSHEY_PLAIN, 2.0d, 2.0d)
        {
            thickness = 2,
        };
        public Point ptTextBaselineStart;
        private Point ptSurroundingBarStart;
        public Rectangle rectSurroundingBar;
        public readonly Size textBarSize;
        public readonly int textHalfHeight;
        public readonly int textHeight;
        private Image<Bgr, byte> originalImage;




        public TextBarImage(string text, Image<Bgr, byte> origImage)
        {
            strText = text;
            Size retTextSize = new Size(0, 0);
            int baseline = 0;
            CvInvoke.cvGetTextSize(strText, ref fnTextFont, ref retTextSize, ref baseline);
            textHeight = retTextSize.Height + baseline;
            textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
            textBarSize = new Size(retTextSize.Width + textHeight, textHeight * 2);
            originalImage = origImage.Copy();
        }





        public TextBarImage(string text, Image<Bgr, byte> origImage, ref MCvFont usedFont)
        {
            strText = text;
            Size retTextSize = new Size(0, 0);

            fnTextFont = usedFont;

            int baseline = 0;
            CvInvoke.cvGetTextSize(strText, ref fnTextFont, ref retTextSize, ref baseline);
            textHeight = retTextSize.Height + baseline;
            textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
            textBarSize = new Size(retTextSize.Width + textHeight, textHeight * 2);
            originalImage = origImage.Copy();
        }




        public Point PtSurroundingBarStart
        {
            get { return ptSurroundingBarStart; }
            set
            {
                ptSurroundingBarStart = value;
                rectSurroundingBar = new Rectangle(ptSurroundingBarStart, textBarSize);
                ptTextBaselineStart = ptSurroundingBarStart + new Size(textHalfHeight, textHeight * 2 - textHalfHeight);
            }

        }
        

        public Image<Bgr, byte> SubImageInTextRect
        {
            get
            {
                originalImage.ROI = rectSurroundingBar;
                Image<Bgr, byte> subImg = originalImage.Copy();
                originalImage.ROI = Rectangle.Empty;
                return subImg;
            }
        }
    }
}
