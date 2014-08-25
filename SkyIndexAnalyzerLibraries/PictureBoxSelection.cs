using System;
using System.Drawing;
using System.Windows.Forms;

namespace SkyIndexAnalyzerLibraries
{
    public class PictureBoxSelection
    {
        private Rectangle selectionRect;
        private Rectangle selectionRectReal;
        private RectangleF selectionRectSchemeRuler;
        public PictureBox underlyingPictureBox;
        private double k = 1.0d;
        /// <summary>
        /// Объект типа imageConditionAndData или ColorSchemeRuler, к которому привязано выделение, представляемое экземпляром класса
        /// </summary>
        public object usedImageData = null;

        private Point _pDown, _pUp;
        //private MouseEventArgs _eDown, _eUp;


        public PictureBoxSelection(object sender, Point pDown, Point pUp, object imgData)
        {
            int mouseEventMaxWidthPix = ((PictureBox)sender).Image.Width;
            int mouseEventMaxHeightPix = ((PictureBox)sender).Image.Height;
            usedImageData = imgData;
            _pDown = pDown;
            _pUp = pUp;

            if (_pDown.X < 0) _pDown.X = 0;
            else if (_pDown.X > mouseEventMaxWidthPix) _pDown.X = mouseEventMaxWidthPix;
            //else if ((_pDown.X % 4 > 2) && (_pDown.X + 4 - _pDown.X % 4 < mouseEventMaxWidthPix)) _pDown.X += 4 - _pDown.X%4;
            //else _pDown.X -= _pDown.X % 4;

            if (_pDown.Y < 0) _pDown.Y = 0;
            else if (_pDown.Y > mouseEventMaxHeightPix) _pDown.Y = mouseEventMaxHeightPix;
            //else if ((_pDown.Y % 4 > 2) && (_pDown.Y + 4 - _pDown.Y % 4 < mouseEventMaxHeightPix)) _pDown.Y += 4 - _pDown.Y % 4;
            //else _pDown.Y -= _pDown.Y % 4;

            if (_pUp.X < 0) _pUp.X = 0;
            else if (_pUp.X > mouseEventMaxWidthPix) _pUp.X = mouseEventMaxWidthPix;
            //else if ((_pUp.X % 4 > 2) && (_pUp.X + 4 - _pUp.X % 4 < mouseEventMaxWidthPix)) _pUp.X += 4 - _pUp.X % 4;
            //else _pUp.X -= _pUp.X % 4;

            if (_pUp.Y < 0) _pUp.Y = 0;
            else if (_pUp.Y > mouseEventMaxHeightPix) _pUp.Y = mouseEventMaxHeightPix;
            //else if ((_pUp.Y % 4 > 2) && (_pUp.Y + 4 - _pUp.Y % 4 < mouseEventMaxHeightPix)) _pUp.Y += 4 - _pUp.Y % 4;
            //else _pUp.Y -= _pUp.Y % 4;

            
            underlyingPictureBox = (PictureBox)sender;
            if (usedImageData.GetType().Equals(typeof(imageConditionAndData)))
            {
                k = (double)(((imageConditionAndData)usedImageData).dataRepresentingImageColored().Width) / (double)(underlyingPictureBox.Image.Width);
                selectionRect = new Rectangle(Math.Min(_pDown.X, _pUp.X), Math.Min(_pDown.Y, _pUp.Y), Math.Max(_pDown.X, _pUp.X) - Math.Min(_pDown.X, _pUp.X), Math.Max(_pDown.Y, _pUp.Y) - Math.Min(_pDown.Y, _pUp.Y));
                selectionRectReal = new Rectangle(
                    Convert.ToInt32(Math.Round((double)selectionRect.Location.X * k, 0)),
                    Convert.ToInt32(Math.Round((double)selectionRect.Location.Y * k, 0)),
                    Convert.ToInt32(Math.Round((double)selectionRect.Width * k, 0)),
                    Convert.ToInt32(Math.Round((double)selectionRect.Height * k, 0)));
                if (selectionRectReal.Width%4 != 0) selectionRectReal.Width += 4 - (selectionRectReal.Width%4);
                if (selectionRectReal.Height % 4 != 0) selectionRectReal.Height += 4 - (selectionRectReal.Height % 4);
                if (selectionRectReal.Right > (int) ServiceTools.getPropValue(usedImageData, "Width"))
                {
                    selectionRectReal.Location = new Point(selectionRectReal.Location.X - (selectionRectReal.Right - (int)ServiceTools.getPropValue(usedImageData, "Width")), selectionRectReal.Location.Y);
                }
                if (selectionRectReal.Bottom > (int)ServiceTools.getPropValue(usedImageData, "Height"))
                {
                    selectionRectReal.Location = new Point(selectionRectReal.Location.X, selectionRectReal.Location.Y - (selectionRectReal.Bottom - (int)ServiceTools.getPropValue(usedImageData, "Height")));
                }

            }
            else if (usedImageData.GetType().Equals(typeof(ColorSchemeRuler)))
            {
                ColorSchemeRuler tmpRuler = (ColorSchemeRuler)usedImageData;
                Point tmpEventArgs = (_pDown.Y > _pUp.Y) ? (_pUp) : (_pDown);
                k = (double)(tmpRuler.maxValue - tmpRuler.minValue) / (double)(underlyingPictureBox.Image.Height);
                selectionRect = new Rectangle(0, Math.Min(_pDown.Y, _pUp.Y), ((PictureBox)sender).Width, Math.Max(_pDown.Y, _pUp.Y) - Math.Min(_pDown.Y, _pUp.Y));
                selectionRectSchemeRuler = new RectangleF(
                                                            0.0f,
                                                            (float)(tmpRuler.GetValueByClickEvent((PictureBox)sender, tmpEventArgs)),
                                                            (float)((PictureBox)sender).Width,
                                                            (float)(selectionRect.Height * k)
                                                         );
            }
        }










        public bool IsEmptySelection
        {
            get
            {
                if (usedImageData.GetType().Equals(typeof(imageConditionAndData)))
                {
                    if ((selectionRectReal.Width == 0) && (selectionRectReal.Height == 0)) return true;
                    else return false;
                }
                else if (usedImageData.GetType().Equals(typeof(ColorSchemeRuler)))
                {
                    if ((selectionRectSchemeRuler.Width == 0) && (selectionRectSchemeRuler.Height == 0)) return true;
                    else return false;
                }
                return false;
            }
        }




        public bool CheckIfDoubleclickedinsideSelection(object sender, MouseEventArgs eDblClick, object imgData)
        {
            if (imgData != usedImageData)
            {
                return false;
            }

            Point realPointAtPicture = new Point(0, 0);

            if (usedImageData.GetType().Equals(typeof(imageConditionAndData)))
            {
                realPointAtPicture.X = Convert.ToInt32(Math.Round((double)eDblClick.X * k, 0));
                realPointAtPicture.Y = Convert.ToInt32(Math.Round((double)eDblClick.Y * k, 0));
                if (selectionRectReal.Contains(realPointAtPicture)) return true;
                else return false;
            }
            else if (usedImageData.GetType().Equals(typeof(ColorSchemeRuler)))
            {
                return false;
            }
            return false;
        }



        public PictureBoxSelection(PictureBoxSelection selectionToCopy)
            : this(selectionToCopy.underlyingPictureBox, selectionToCopy._pDown, selectionToCopy._pUp, selectionToCopy.usedImageData)
        {
        }


        public PictureBoxSelection Copy()
        {
            return new PictureBoxSelection(underlyingPictureBox, _pDown, _pUp, usedImageData);
        }




        public object SelectionRectReal
        {
            get
            {
                if (usedImageData.GetType().Equals(typeof(imageConditionAndData)))
                {
                    return selectionRectReal;
                }
                else if (usedImageData.GetType().Equals(typeof(ColorSchemeRuler)))
                {
                    return selectionRectSchemeRuler;
                }
                return selectionRectReal;
            }
        }


        //public int UpperEdge
        //{
        //    get
        //    {
        //        return selectionRect.Location.Y;
        //    }
        //}
        //public double UpperEdgeNormalized
        //{
        //    get
        //    {
        //        return ((double)selectionRect.Location.Y / (double)underlyingPictureBox.Height);
        //    }
        //}
        //public int LowerEdge
        //{
        //    get
        //    {
        //        return (selectionRect.Location.Y + selectionRect.Height);
        //    }
        //}
        //public double LowerEdgeNormalized
        //{
        //    get
        //    {
        //        return ((double)(selectionRect.Location.Y + selectionRect.Height) / (double)underlyingPictureBox.Height);
        //    }
        //}
        //public int LeftEdge
        //{
        //    get
        //    {
        //        return (selectionRect.Location.X);
        //    }
        //}
        //public double LeftEdgeNormalized
        //{
        //    get
        //    {
        //        return ((double)(selectionRect.Location.X) / (double)underlyingPictureBox.Width);
        //    }
        //}
        //public int RightEdge
        //{
        //    get
        //    {
        //        return (selectionRect.Location.X + selectionRect.Width);
        //    }
        //}
        //public double RightEdgeNormalized
        //{
        //    get
        //    {
        //        return ((double)(selectionRect.Location.X + selectionRect.Width) / (double)underlyingPictureBox.Width);
        //    }
        //}
        //
        //
        //public int UpperEdgeReal
        //{
        //    get
        //    {
        //        return selectionRectReal.Location.Y;
        //    }
        //}
        //public int LowerEdgeReal
        //{
        //    get
        //    {
        //        return (selectionRectReal.Location.Y + selectionRect.Height);
        //    }
        //}
        //public int LeftEdgeReal
        //{
        //    get
        //    {
        //        return (selectionRectReal.Location.X);
        //    }
        //}
        //public int RightEdgeReal
        //{
        //    get
        //    {
        //        return (selectionRectReal.Location.X + selectionRect.Width);
        //    }
        //}
    }
}
