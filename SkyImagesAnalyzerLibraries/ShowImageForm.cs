using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SkyIndexAnalyzerLibraries
{
    public partial class ShowImageForm : Form
    {
        delegate void UpdatePictureBoxCallback(System.Windows.Forms.PictureBox PictureBoxControl, Image Image2Show, bool NormalizeImage = false);
        private Bitmap LocalBitmap2show;
        //private SkyIndexAnalyzer_AnalysisForm ParentForm;
        private int SetImageWidth, SetImageHeight;
        private ConnectedObjectsDetection ObjDetector;
        private ToolTip tt;
        private int mouse_x_prev = 0, mouse_y_prev = 0;



        //public ShowImageForm(Bitmap BitmapToShow, SkyIndexAnalyzer_AnalysisForm in_parentForm, ConnectedObjectsDetection in_ObjDetector)
        public ShowImageForm(Bitmap BitmapToShow, ConnectedObjectsDetection in_ObjDetector)
        {
            InitializeComponent();
            LocalBitmap2show = new Bitmap(BitmapToShow);
            //ParentForm = in_parentForm;
            ObjDetector = in_ObjDetector;
        }




        private void ShowImageForm_Load(object sender, EventArgs e)
        {
            int[] SetImageSize = ThreadSafeOperations.UpdatePictureBox(pictureBox1, LocalBitmap2show, true);
            SetImageWidth = SetImageSize[0];
            SetImageHeight = SetImageSize[1];
        }

        private void ShowImageForm_Resize(object sender, EventArgs e)
        {
            int[] SetImageSize = ThreadSafeOperations.UpdatePictureBox(pictureBox1, LocalBitmap2show, true);
            SetImageWidth = SetImageSize[0];
            SetImageHeight = SetImageSize[1];
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point MousePos = e.Location;
            double xd = (double)LocalBitmap2show.Width * (double)MousePos.X / (double)SetImageWidth;
            double yd = (double)LocalBitmap2show.Height * (double)MousePos.Y / (double)SetImageHeight;
            int x = (int)Math.Round(xd);
            int y = (int)Math.Round(yd);
            string Str2Show = "x = " + x.ToString() + "; y = " + y.ToString() + "; ";
            if (ObjDetector != null)
            {
                Str2Show += "SIval = " + Math.Round(ObjDetector.DetectedObjects[x][y].SIvalue, 2).ToString() + "; ";
                Str2Show += "IsCloud = " + ObjDetector.DetectedObjects[x][y].IsCloud.ToString() + "; ";
                Str2Show += "ObjID = " + ObjDetector.DetectedObjects[x][y].ObjID.ToString() + "; ";
            }
            ThreadSafeOperations.SetText(label1, Str2Show, false);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point MousePos = e.Location;
            if (MousePos.X == mouse_x_prev)
            {
                return;
            }
            else if (MousePos.Y == mouse_y_prev)
            {
                return;
            }
            else
            {
                mouse_x_prev = MousePos.X;
                mouse_y_prev = MousePos.Y;
            }
            double xd = (double)LocalBitmap2show.Width * (double)MousePos.X / (double)SetImageWidth;
            double yd = (double)LocalBitmap2show.Height * (double)MousePos.Y / (double)SetImageHeight;
            int x = (int)Math.Round(xd);
            int y = (int)Math.Round(yd);
            string Str2Show = "x = " + x.ToString() + "; y = " + y.ToString() + "; ";
            if (ObjDetector != null)
            {
                if ((x < ObjDetector.DetectedObjects.Count()) && (y < ObjDetector.DetectedObjects[0].Count()))
                {
                    Str2Show += "SIval = " + Math.Round(ObjDetector.DetectedObjects[x][y].SIvalue, 2).ToString() + "; ";
                    Str2Show += "IsCloud = " + ObjDetector.DetectedObjects[x][y].IsCloud.ToString() + "; ";
                    Str2Show += "ObjID = " + ObjDetector.DetectedObjects[x][y].ObjID.ToString() + "; ";
                }
            }
            if (tt == null)
            {
                tt = new ToolTip();
            }
            tt.Show(Str2Show, this, MousePos.X + pictureBox1.Location.X, MousePos.Y + pictureBox1.Location.Y);
        }
    }
}
