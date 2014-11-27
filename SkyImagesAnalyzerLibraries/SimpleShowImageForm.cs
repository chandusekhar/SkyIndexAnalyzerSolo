using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;



namespace SkyImagesAnalyzerLibraries
{
    public partial class SimpleShowImageForm : Form
    {
        private Bitmap LocalBitmap2show;
        public string title = "";

        public event EventHandler FormResizing;



        public SimpleShowImageForm()
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(800, 600);
        }


        public SimpleShowImageForm(Bitmap BitmapToShow)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(BitmapToShow);
        }



        public SimpleShowImageForm(Bitmap BitmapToShow, string titleToShow)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(BitmapToShow);
            title = titleToShow;
        }



        public SimpleShowImageForm(Image imgToShow)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(imgToShow);
        }



        public SimpleShowImageForm(Image imgToShow, string titleToShow)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(imgToShow);
            title = titleToShow;
        }



        public SimpleShowImageForm(Image<Gray, Byte> imgToShow)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(imgToShow.Bitmap);
        }





        public SimpleShowImageForm(Image<Gray, Byte> imgToShow, string titleToShow)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(imgToShow.Bitmap);
            title = titleToShow;
        }




        public SimpleShowImageForm(Image<Bgr, Byte> imgToShow)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(imgToShow.Bitmap);
        }



        public SimpleShowImageForm(Image<Bgr, Byte> imgToShow, string titleToShow)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(imgToShow.Bitmap);
            title = titleToShow;
        }


        
        private void SimpleShowImageForm_Load(object sender, EventArgs e)
        {
            this.RaisePaintEvent(null, null);
        }

        private void SimpleShowImageForm_Resize(object sender, EventArgs e)
        {
            if (FormResizing != null)
            {
                FormResizing.Invoke(sender, e);
            }
            this.RaisePaintEvent(null, null);
        }

        private void SimpleShowImageForm_Paint(object sender, PaintEventArgs e)
        {
            if (LocalBitmap2show != null)
            {
                ThreadSafeOperations.UpdatePictureBox(pb1, LocalBitmap2show, true);
            }
            ThreadSafeOperations.SetText(lblTitle1, title, false);
            this.Text = title;
        }

        private void SimpleShowImageForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }


        public void UpdateBitmap(object BitmapToShow)
        {
            if (BitmapToShow.GetType() == typeof (Bitmap))
            {
                LocalBitmap2show = new Bitmap((Bitmap)BitmapToShow);
            }
            else if (BitmapToShow.GetType() == typeof(Image))
            {
                LocalBitmap2show = new Bitmap((Bitmap)BitmapToShow);
            }
            else if (BitmapToShow.GetType() == typeof(Image<Gray, byte>))
            {
                LocalBitmap2show = new Bitmap(((Image<Gray, byte>)BitmapToShow).Bitmap);
            }
            else if (BitmapToShow.GetType() == typeof(Image<Bgr, byte>))
            {
                LocalBitmap2show = new Bitmap(((Image<Bgr, byte>)BitmapToShow).Bitmap);
            }
            
            RaisePaintEvent(null, null);
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            if (tbFileName.Text == "")
            {
                return;
            }

            string fileName = tbFileName.Text;

            try
            {
                LocalBitmap2show.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                ThreadSafeOperations.SetText(lblTitle1, fileName, false);
            }
            catch (Exception)
            {
                ThreadSafeOperations.SetText(lblTitle1, "Couldnt save the file", false);
                return;
            }
            
        }
    }
}
