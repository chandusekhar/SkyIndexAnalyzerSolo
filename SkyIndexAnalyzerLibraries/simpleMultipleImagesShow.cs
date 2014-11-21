using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;



namespace SkyIndexAnalyzerLibraries
{
    public partial class simpleMultipleImagesShow : Form
    {
        private List<Bitmap> bmList = new List<Bitmap>();
        private List<string> titlesList = new List<string>();

        //public delegate void PlaceAPictureDelegate

        public simpleMultipleImagesShow()
        {
            
            for (int i = 0; i < 6; i++)
            {
                bmList.Add(null);
                titlesList.Add("");
            }

            InitializeComponent();
        }


        public void PlaceAPicture(object pictureSource, string titleToShow, int placeholderNumber = 1)
        {
            if (pictureSource.GetType() == typeof(Bitmap))
            {
                bmList[placeholderNumber-1] = new Bitmap(pictureSource as Bitmap);
            }
            else if (pictureSource.GetType() == typeof(Image))
            {
                bmList[placeholderNumber-1] = new Bitmap(pictureSource as Bitmap);
            }
            else if (pictureSource.GetType() == typeof(Image<Gray, Byte>))
            {
                bmList[placeholderNumber-1] = new Bitmap((pictureSource as Image<Gray, Byte>).Bitmap);
            }
            else if (pictureSource.GetType() == typeof(Image<Bgr, Byte>))
            {
                bmList[placeholderNumber-1] = new Bitmap((pictureSource as Image<Bgr, Byte>).Bitmap);
            }


            titlesList[placeholderNumber-1] = titleToShow;


            RaisePaintEvent(null, null);
        }


        private void simpleMultipleImagesShow_Load(object sender, EventArgs e)
        {
            RaisePaintEvent(null, null);
        }

        private void simpleMultipleImagesShow_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                ThreadSafeOperations.UpdatePictureBox((PictureBox)(Controls.Find("pb" + (i+1).ToString(), true)[0]),
                    bmList[i], true);

                ThreadSafeOperations.SetText((Label) (Controls.Find("lblTitle" + (i+1).ToString(), true)[0]), titlesList[i],
                    false);
            }
        }

        private void simpleMultipleImagesShow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }

        private void simpleMultipleImagesShow_Resize(object sender, EventArgs e)
        {
            this.RaisePaintEvent(null, null);
        }

        private void pb1_DoubleClick(object sender, EventArgs e)
        {
            PictureBox pbSender = sender as PictureBox;
            int pbNumber = Convert.ToInt32(pbSender.Name.ToString().Substring(2));
            Bitmap targetBitmap = bmList[pbNumber - 1];

            ServiceTools.ShowPicture(targetBitmap);

        }
    }
}
