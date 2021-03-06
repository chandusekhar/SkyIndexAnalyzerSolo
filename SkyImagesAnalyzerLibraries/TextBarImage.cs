﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;


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
        //public FontFace fnTextFontFace = FontFace.HersheyPlain;
        //public double fnTextFontScale = 1.0d;
        //public int fnTextFontThickness = 2;
        public Point ptTextBaselineStart;
        private Point ptSurroundingBarStart;
        public Rectangle rectSurroundingBar;
        public readonly Size textBarSize;
        public readonly int textHalfHeight;
        public readonly int textHeight;
        private Image<Bgr, byte> originalImage;


        #region // obsolete
        //public enum FontFace
        //{
        //    /// <summary>
        //    /// Hershey simplex
        //    /// </summary>
        //    HersheySimplex = 0,
        //    /// <summary>
        //    /// Hershey plain
        //    /// </summary>
        //    HersheyPlain = 1,
        //    /// <summary>
        //    /// Hershey duplex 
        //    /// </summary>
        //    HersheyDuplex = 2,
        //    /// <summary>
        //    /// Hershey complex
        //    /// </summary>
        //    HersheyComplex = 3,
        //    /// <summary>
        //    /// Hershey triplex
        //    /// </summary>
        //    HersheyTriplex = 4,
        //    /// <summary>
        //    /// Hershey complex small
        //    /// </summary>
        //    HersheyComplexSmall = 5,
        //    /// <summary>
        //    /// Hershey script simplex
        //    /// </summary>
        //    HersheyScriptSimplex = 6,
        //    /// <summary>
        //    /// Hershey script complex
        //    /// </summary>
        //    HersheyScriptComplex = 7
        //}
        #endregion // obsolete




        public TextBarImage(string text, Image<Bgr, byte> origImage)
        {
            originalImage = origImage.Copy();
            strText = text;
            if (!strText.Contains(Environment.NewLine))
            {
                // Size retTextSize = GetTextSize(strText, fnTextFontFace, fnTextFontScale, fnTextFontThickness);
                Size retTextSize = GetTextSize(strText, ref fnTextFont);
                //int baseline = 0;
                //CvInvoke.cvGetTextSize(strText, ref fnTextFont, ref retTextSize, ref baseline);
                //textHeight = retTextSize.Height + baseline;
                textHeight = retTextSize.Height;
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
                    //Size retCurrTextSize = new Size(0, 0);
                    //int currBaseline = 0;
                    //CvInvoke.cvGetTextSize(substr, ref fnTextFont, ref retCurrTextSize, ref currBaseline);
                    //textHeight = retCurrTextSize.Height + currBaseline;

                    // Size currTextSize = GetTextSize(strText, fnTextFontFace, fnTextFontScale, fnTextFontThickness);
                    Size currTextSize = GetTextSize(strText, ref fnTextFont);
                    textHeight = currTextSize.Height;
                    textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
                    textBarSize.Width = Math.Max(currTextSize.Width + textHeight, textBarSize.Width);
                    textBarSize.Height += Convert.ToInt32(textHeight * 1.5d);
                }
            }

        }





        // public TextBarImage(string text, Image<Bgr, byte> origImage, FontFace usedFontFace = FontFace.HersheyPlain, double usedFontScale = 1.0d, int usedFontThickness = 1)
        public TextBarImage(string text, Image<Bgr, byte> origImage, ref MCvFont usedFont)
        {
            originalImage = origImage.Copy();
            strText = text;

            //fnTextFontFace = usedFontFace;
            //fnTextFontScale = usedFontScale;
            //fnTextFontThickness = usedFontThickness;

            fnTextFont = usedFont;

            if (!strText.Contains(Environment.NewLine))
            {
                //Size retTextSize = new Size(0, 0);
                //int baseline = 0;
                //CvInvoke.cvGetTextSize(strText, ref fnTextFont, ref retTextSize, ref baseline);
                //textHeight = retTextSize.Height + baseline;

                // Size currTextSize = GetTextSize(strText, fnTextFontFace, fnTextFontScale, fnTextFontThickness);
                Size currTextSize = GetTextSize(strText, ref fnTextFont);
                textHeight = currTextSize.Height;
                textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
                textBarSize = new Size(currTextSize.Width + textHeight, textHeight * 2);
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
                    //Size retCurrTextSize = new Size(0, 0);
                    //int currBaseline = 0;
                    //CvInvoke.cvGetTextSize(substr, ref fnTextFont, ref retCurrTextSize, ref currBaseline);
                    //textHeight = retCurrTextSize.Height + currBaseline;

                    // Size currTextSize = GetTextSize(substr, fnTextFontFace, fnTextFontScale, fnTextFontThickness);
                    Size currTextSize = GetTextSize(substr, ref fnTextFont);
                    textHeight = currTextSize.Height;
                    textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
                    textBarSize.Width = Math.Max(currTextSize.Width + textHeight, textBarSize.Width);
                    textBarSize.Height += textHeight * 2;
                }
            }

        }




        public Image<Bgr, byte> TextSignImageAtOriginalBlank(Bgr textColor)
        {

            Image<Bgr, byte> retImg = originalImage.CopyBlank();

            if (!strText.Contains(Environment.NewLine))
            {
                // retImg.Draw(strText, ptTextBaselineStart, fnTextFontFace, fnTextFontScale, textColor, fnTextFontThickness);
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
                    // retImg.Draw(substr, ptLocalTextBaselineStart, fnTextFontFace, fnTextFontScale, textColor, fnTextFontThickness);
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




        //public static Size GetTextSize(string strMessage, FontFace usedFontFace = FontFace.HersheyPlain,
        //    double usedFontScale = 1.0d, int usedFontThickness = 1)
        public static Size GetTextSize(string strMessage, ref MCvFont usedFont)
        {
            Image<Gray, byte> theImg = new Image<Gray, byte>(1024, 1024, new Gray(0));
            //theImg.Draw(strMessage, new Point(0, 1024), usedFontFace, usedFontScale, new Gray(255), usedFontThickness);
            theImg.Draw(strMessage, ref usedFont, new Point(0, 1024), new Gray(255));

            List<Contour<Point>> lSymbolsContours = theImg.DetectContours(RETR_TYPE.CV_RETR_EXTERNAL);
            List<RectangleF> lSymbolsRects = lSymbolsContours.ConvertAll(cont => cont.GetMinAreaRect().GetBoundingRectangleF()); //  3.0 : CvInvoke.MinAreaRect(cont).MinAreaRect()
            RectangleF resRect = lSymbolsRects[0];
            foreach (RectangleF currRect in lSymbolsRects)
            {
                resRect = UniteRectangles(resRect, currRect);
            }
            
            return resRect.Size.ToSize();
        }


        public static Rectangle UniteRectangles(Rectangle rct1, Rectangle rct2)
        {
            Rectangle resRect = new Rectangle();
            resRect.Location = new Point(Math.Min(rct1.Left, rct2.Left), Math.Min(rct1.Top, rct2.Top));
            resRect.Width = Math.Max(rct1.Right, rct2.Right) - resRect.Location.X;
            resRect.Height = Math.Max(rct1.Bottom, rct2.Bottom) - resRect.Location.Y;
            return resRect;
        }


        public static RectangleF UniteRectangles(RectangleF rct1, RectangleF rct2)
        {
            RectangleF resRect = new RectangleF();
            resRect.Location = new PointF(Math.Min(rct1.Left, rct2.Left), Math.Min(rct1.Top, rct2.Top));
            resRect.Width = Math.Max(rct1.Right, rct2.Right) - resRect.Location.X;
            resRect.Height = Math.Max(rct1.Bottom, rct2.Bottom) - resRect.Location.Y;
            return resRect;
        }
    }
}
