using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace SkyIndexAnalyzerLibraries
{
    enum MouseActionsRegime
    {
        Nothing,
        MovingSunDisk,
        ResizingSunDisk,
        DrawingSunDisk
    }

    public partial class SunDiskRepresentingAndCorrectionForm : Form
    {
        public Image<Bgr, Byte> origImage = null;
        private Image<Bgr, byte> imgToShow = null;
        public RoundData sunDiskPositionAndSize = RoundData.nullRoundData();
        private MouseActionsRegime currMouseActionRegime = MouseActionsRegime.Nothing;
        private PointD ptMouseDown = PointD.nullPointD();
        private RoundData sunDiskPositionAndSizeBeforeMovingResizing = RoundData.nullRoundData();

        public SunDiskRepresentingAndCorrectionForm()
        {
            InitializeComponent();
        }



        public SunDiskRepresentingAndCorrectionForm(Image<Bgr, byte> inImg, RoundData inRoundData)
        {
            sunDiskPositionAndSize = inRoundData;
            origImage = inImg.Copy();

            InitializeComponent();

            UpdateImage();
        }






        private void SunDiskRepresentingAndCorrectionForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }



        private void pbTheImage_MouseDown(object sender, MouseEventArgs e)
        {
            PointD ptRealPosition = ((PictureBox)sender).GetMouseEventPositionOnRealImage(e, origImage);
            ThreadSafeOperations.SetText(lblTitle, ptRealPosition.ToString() + "  down  " + currMouseActionRegime, false);

            ptMouseDown = ptRealPosition;
            switch (ptRealPosition.IsPointInsideCircle(sunDiskPositionAndSize))
            {
                case -1:
                    currMouseActionRegime = MouseActionsRegime.DrawingSunDisk;
                    break;
                case 0:
                    currMouseActionRegime = MouseActionsRegime.ResizingSunDisk;
                    sunDiskPositionAndSizeBeforeMovingResizing = sunDiskPositionAndSize.Copy();
                    break;
                case 1:
                    currMouseActionRegime = MouseActionsRegime.MovingSunDisk;
                    sunDiskPositionAndSizeBeforeMovingResizing = sunDiskPositionAndSize.Copy();
                    break;
                default:
                    break;
            }
        }





        private void ChangeSunDiskData(PointD ptCurrMousePosition)
        {
            if (ptMouseDown != ptCurrMousePosition) // not just click
            {
                switch (currMouseActionRegime)
                {
                    case MouseActionsRegime.Nothing:
                        break;
                    case MouseActionsRegime.DrawingSunDisk:
                        sunDiskPositionAndSize = new RoundData(ptMouseDown.X, ptMouseDown.Y,
                            ptMouseDown.Distance(ptCurrMousePosition));
                        
                        break;
                    case MouseActionsRegime.MovingSunDisk:
                        sunDiskPositionAndSize.DCenterX = sunDiskPositionAndSizeBeforeMovingResizing.DCenterX + (ptCurrMousePosition.X - ptMouseDown.X);
                        sunDiskPositionAndSize.DCenterY = sunDiskPositionAndSizeBeforeMovingResizing.DCenterY + (ptCurrMousePosition.Y - ptMouseDown.Y);
                        
                        break;
                    case MouseActionsRegime.ResizingSunDisk:
                        sunDiskPositionAndSize.DRadius =
                            ptCurrMousePosition.Distance(sunDiskPositionAndSize.pointDCircleCenter());
                        
                        break;
                    default:
                        break;
                }
            }
        }


        


        private void pbTheImage_MouseUp(object sender, MouseEventArgs e)
        {
            PointD ptRealPosition = ((PictureBox)sender).GetMouseEventPositionOnRealImage(e, origImage);

            if (ptMouseDown != ptRealPosition) // not just click
            {
                ChangeSunDiskData(ptRealPosition);

                switch (currMouseActionRegime)
                {
                    case MouseActionsRegime.Nothing:
                        break;
                    case MouseActionsRegime.DrawingSunDisk:
                        currMouseActionRegime = MouseActionsRegime.Nothing;
                        break;
                    case MouseActionsRegime.MovingSunDisk:
                        currMouseActionRegime = MouseActionsRegime.Nothing;
                        break;
                    case MouseActionsRegime.ResizingSunDisk:
                        currMouseActionRegime = MouseActionsRegime.Nothing;
                        break;
                    default:
                        break;
                }
                sunDiskPositionAndSizeBeforeMovingResizing = RoundData.nullRoundData();
            }
            

            //currMouseActionRegime = MouseActionsRegime.Nothing;

            //RaisePaintEvent(null, null);
            UpdateImage();
        }





        private void pbTheImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (currMouseActionRegime != MouseActionsRegime.Nothing)
            {
                PointD ptRealPosition = ((PictureBox)sender).GetMouseEventPositionOnRealImage(e, origImage);

                ChangeSunDiskData(ptRealPosition);

                //RaisePaintEvent(null, null);
                UpdateImage();
            }
        }





        





        private void SunDiskRepresentingAndCorrectionForm_Load(object sender, EventArgs e)
        {
            RaisePaintEvent(null, null);
        }

        private void SunDiskRepresentingAndCorrectionForm_Paint(object sender, PaintEventArgs e)
        {
            if (imgToShow == null) imgToShow = origImage.Copy();
            ThreadSafeOperations.UpdatePictureBox(pbTheImage, imgToShow.Bitmap, true);
        }

        private void bgwImageUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            UpdateImage();
        }


        private void UpdateImage()
        {
            imgToShow = origImage.Copy();
            if (!sunDiskPositionAndSize.IsNull)
            {
                int thickness = 0; // fill out
                switch (currMouseActionRegime)
                {
                    case MouseActionsRegime.Nothing:
                        thickness = 0;
                        break;
                    case MouseActionsRegime.DrawingSunDisk:
                        thickness = 2;
                        break;
                    case MouseActionsRegime.MovingSunDisk:
                        thickness = 2;
                        break;
                    case MouseActionsRegime.ResizingSunDisk:
                        thickness = 2;
                        break;
                    default:
                        break;
                }

                imgToShow.Draw(sunDiskPositionAndSize.CircleF(), new Bgr(Color.Orange), thickness);
            }

            ThreadSafeOperations.UpdatePictureBox(pbTheImage, imgToShow.Bitmap, true);
            ThreadSafeOperations.SetText(lblTitle, sunDiskPositionAndSize.ToString(), false);
        }
    }
}
