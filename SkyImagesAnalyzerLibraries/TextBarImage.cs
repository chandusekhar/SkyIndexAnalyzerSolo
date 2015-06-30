using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;




namespace SkyImagesAnalyzerLibraries
{
    public class TextBarImage
    {
        private static Bgr colorGreen = new Bgr(0, 255, 0);
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
            originalImage = origImage.Copy();
            strText = text;
            if (!strText.Contains(Environment.NewLine))
            {
                Size retTextSize = new Size(0, 0);
                int baseline = 0;
                CvInvoke.cvGetTextSize(strText, ref fnTextFont, ref retTextSize, ref baseline);
                textHeight = retTextSize.Height + baseline;
                textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
                textBarSize = new Size(retTextSize.Width + textHeight, textHeight * 2);
            }
            else
            {
                string[] substrArr = strText.Split(new string[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);

                //Size retTextSize = new Size(0, 0);
                //int baseline = 0;
                textBarSize = new Size(0, 0);

                foreach (string substr in substrArr)
                {
                    Size retCurrTextSize = new Size(0, 0);
                    int currBaseline = 0;
                    CvInvoke.cvGetTextSize(substr, ref fnTextFont, ref retCurrTextSize, ref currBaseline);
                    textHeight = retCurrTextSize.Height + currBaseline;
                    textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
                    textBarSize.Width = Math.Max(retCurrTextSize.Width + textHeight, textBarSize.Width);
                    textBarSize.Height += Convert.ToInt32(textHeight * 1.5d);
                }
            }

        }





        public TextBarImage(string text, Image<Bgr, byte> origImage, ref MCvFont usedFont)
        {
            originalImage = origImage.Copy();
            strText = text;

            fnTextFont = usedFont;

            if (!strText.Contains(Environment.NewLine))
            {
                Size retTextSize = new Size(0, 0);
                int baseline = 0;
                CvInvoke.cvGetTextSize(strText, ref fnTextFont, ref retTextSize, ref baseline);
                textHeight = retTextSize.Height + baseline;
                textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
                textBarSize = new Size(retTextSize.Width + textHeight, textHeight * 2);
            }
            else
            {
                string[] substrArr = strText.Split(new string[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);

                //Size retTextSize = new Size(0, 0);
                //int baseline = 0;
                textBarSize = new Size(0, 0);

                foreach (string substr in substrArr)
                {
                    Size retCurrTextSize = new Size(0, 0);
                    int currBaseline = 0;
                    CvInvoke.cvGetTextSize(substr, ref fnTextFont, ref retCurrTextSize, ref currBaseline);
                    textHeight = retCurrTextSize.Height + currBaseline;
                    textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
                    textBarSize.Width = Math.Max(retCurrTextSize.Width + textHeight, textBarSize.Width);
                    textBarSize.Height += textHeight * 2;
                }
            }

        }




        public Image<Bgr, byte> TextSignImageAtOriginalBlank(Bgr textColor)
        {

            Image<Bgr, byte> retImg = originalImage.CopyBlank();

            if (!strText.Contains(Environment.NewLine))
            {
                retImg.Draw(strText, ref fnTextFont, ptTextBaselineStart, textColor);
                return retImg;
            }
            else
            {
                string[] substrArr = strText.Split(new string[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);

                Point ptLocalTextBaselineStart = ptTextBaselineStart;
                
                foreach (string substr in substrArr)
                {
                    retImg.Draw(substr, ref fnTextFont, ptLocalTextBaselineStart, textColor);
                    ptLocalTextBaselineStart += new Size(0, Convert.ToInt32(textHeight * 1.5));
                }

                return retImg;
            }
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
