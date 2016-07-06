using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Geometry;

namespace SkyImagesAnalyzerLibraries
{
    public partial class ImageConditionAndDataRepresentingForm : Form
    {
        public imageConditionAndData imgData = null;
        /// <summary>
        /// Плавающая подсказка рядом с курсором мыши
        /// </summary>
        private ToolTip textToolTip = null;
        /// <summary>
        /// The previous tooltip mouse location
        /// </summary>
        private Point previousTooltipMouseLocation;
        /// <summary>
        /// Left button down arguments var
        /// хранит данные события onMouseDown до тех пор, пока не
        /// - обнулят, если кнопка была отпущена за пределами PictureBox, где была нажата
        /// - используют при формировании данных выделения на PictureBox
        /// </summary>
        private MouseEventArgs meLeftButtonDownArgs = null;
        /// <summary>
        /// Left button down sender
        /// Хранит объект, на котором появилось onMouseDown событие, данные которого записалось в meДуаеButtonDownArgs
        /// хранится до тех пор, пока не будет ненужен
        /// </summary>
        private object meLeftButtonDownSender = null;


        public Dictionary<string, object> defaultProperties = null;


        public ImageConditionAndDataRepresentingForm(string dataName = "")
        {
            InitializeComponent();
            if (dataName != "")
            {
                this.Text = dataName;
                lblTitle.Text = dataName;
            }
        }


        public ImageConditionAndDataRepresentingForm(imageConditionAndData imageData, string dataName = "")
        {
            imgData = imageData;

            InitializeComponent();

            if (dataName != "")
            {
                this.Text = dataName;
                lblTitle.Text = dataName;
            }
        }

        public void ImageConditionAndDataRepresentingForm_Paint(object sender, PaintEventArgs e)
        {
            ThreadSafeOperations.UpdatePictureBox(pbRes, imgData.dataRepresentingImageColored(), true);
            ThreadSafeOperations.UpdatePictureBox(pbScale, imgData.currentColorSchemeRuler.RulerBitmap(pbScale.Width, pbScale.Height), false);

        }

        private void ImageConditionAndDataRepresentingForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                meLeftButtonDownArgs = e;
                meLeftButtonDownSender = sender;
            }
        }

        private void pbRes_MouseMove(object sender, MouseEventArgs e)
        {
            imageConditionAndData currentImgData = null;
            if (sender == pbRes) currentImgData = imgData;
            PointD origDataPointPosition = new PointD();
            double clickedValue = currentImgData.GetValueByClickEvent((PictureBox)sender, e, out origDataPointPosition);
            PointD thePointD = currentImgData.getDataPositionByClickEvent((PictureBox)sender, e);
            Point thePoint = new Point(Convert.ToInt32(thePointD.X), Convert.ToInt32(thePointD.Y));
            string tooltipText = "position: " + thePoint + Environment.NewLine + ServiceTools.DoubleValueRepresentingString(clickedValue);
            ShowToolTip(e, tooltipText, sender);
        }


        private void ShowToolTip(MouseEventArgs e, String textToShow, object theElement)
        {

            if (textToolTip == null) textToolTip = new ToolTip();
            else
            {
                if (previousTooltipMouseLocation == e.Location)
                {
                    return;
                }
                textToolTip.Dispose();
                textToolTip = new ToolTip();
            }
            Point loc = (Point)ServiceTools.getPropValue(theElement, "Location");
            textToolTip.Show(textToShow, this, e.X + loc.X, e.Y + loc.Y);
            previousTooltipMouseLocation = e.Location;
        }

        private void HighlightLinkedSelection(PictureBoxSelection theSelection)
        {
            if (theSelection.usedImageData.GetType().Equals(typeof(imageConditionAndData)))
            {
                ((imageConditionAndData)(theSelection.usedImageData)).currentColorSchemeRuler.makeHihghlightMask();
            }
            else if (theSelection.usedImageData.GetType().Equals(typeof(ColorSchemeRuler)))
            {
                ((ColorSchemeRuler)(theSelection.usedImageData)).imgToRule.makeHihghlightMask();
            }
        }




        private void pbRes_MouseUp(object sender, MouseEventArgs e)
        {
            // ReSharper disable InconsistentNaming
            imageConditionAndData currentICD = null;
            // ReSharper restore InconsistentNaming
            Type theSenderDataType = typeof(imageConditionAndData);



            if (e.Button == MouseButtons.Left && sender.Equals(meLeftButtonDownSender))
            {
                PictureBoxSelection pbSelection = null;

                if (sender == pbRes)
                {
                    currentICD = imgData;
                    theSenderDataType = imgData.GetType();
                }
                else if (sender == pbScale)
                {
                    currentICD = imgData;
                    theSenderDataType = imgData.currentColorSchemeRuler.GetType();
                }


                //если уже есть selection у этого объекта, а это выделение пусто - проверить, было ли оно внутри
                //если было внутри - значит, был клик или даблклик внутри выделения - не обрабатывать здесь
                if (theSenderDataType == typeof(imageConditionAndData))
                {
                    pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, currentICD);
                    if ((pbSelection.IsEmptySelection) && (currentICD.Selection != null))
                    {
                        if (currentICD.Selection.CheckIfDoubleclickedinsideSelection(sender, e, currentICD))
                        {
                            return;
                        }
                    }
                    currentICD.Selection = pbSelection;
                }
                else if (theSenderDataType == typeof(ColorSchemeRuler))
                {
                    pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, currentICD.currentColorSchemeRuler);
                    currentICD.currentColorSchemeRuler.Selection = pbSelection;
                }


                HighlightLinkedSelection(pbSelection);
                RaisePaintEvent(null, null);

                if ((theSenderDataType == typeof(ColorSchemeRuler)) && (imgData.HighlightMask != null))
                {
                    string testToShow = "significant area: " + imgData.maskImageBinary.CountNonzero()[0] + Environment.NewLine;
                    testToShow += "highlighted area: " + imgData.highlightedArea + Environment.NewLine;
                    testToShow += "highlighted area relative: " + (imgData.highlightedArea / (double)imgData.maskImageBinary.CountNonzero()[0]).ToString("e") + Environment.NewLine;
                    ThreadSafeOperations.SetTextTB(tbStats, testToShow, true);
                }
            }
        }

        private void pbRes_MouseLeave(object sender, EventArgs e)
        {
            if (textToolTip != null)
            {
                textToolTip.Dispose();
                textToolTip = null;
            }

            meLeftButtonDownArgs = null;
            meLeftButtonDownSender = null;
        }

        private void pbScale_MouseMove(object sender, MouseEventArgs e)
        {
            ColorSchemeRuler currentRuler = null;
            if (sender == pbScale)
            {
                currentRuler = imgData.currentColorSchemeRuler;
            }


            double clickedValue = currentRuler.GetValueByClickEvent((PictureBox)sender, e);
            clickedValue = Math.Round(clickedValue, 2);

            ShowToolTip(e, clickedValue.ToString(), sender);
        }

        private void chbRes1DynamicScale_CheckedChanged(object sender, EventArgs e)
        {
            ColorSchemeRuler currentResultRuler = null;
            if (sender == chbRes1DynamicScale)
            {
                currentResultRuler = imgData.currentColorSchemeRuler;
            }


            currentResultRuler.IsMarginsFixed = !((CheckBox)sender).Checked;

            RaisePaintEvent(null, null);
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Directory.GetCurrentDirectory();
            sfd.FileName = "ResultData.nc";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string theFileName = sfd.FileName;
                NetCDFoperations.SaveDataToFile(imgData.DmSourceData, theFileName, tbStats, true);
            }
        }




        public void SaveData(string fileName, bool absolutePath = false)
        {
            if (fileName != "")
            {
                imgData.DmSourceData.SaveNetCDFdataMatrix(fileName);
                //NetCDFoperations.SaveDataToFile(imgData.DmSourceData, fileName, tbStats, absolutePath);
            }
        }




        private void btnChangeColorScheme_Click(object sender, EventArgs e)
        {
            imgData = imgData.Copy();



            //TextBox currFilenameTextbox = tbColorSchemePath1;
            //PictureBox pbCurrentColorScheme = pbRes1Scale;



            OpenFileDialog opFD = new OpenFileDialog();
            opFD.Filter = "RGB files (*.rgb)|*.rgb|All files (*.*)|*.*";
            opFD.InitialDirectory = Directory.GetCurrentDirectory();
            opFD.Multiselect = false;
            DialogResult dialogRes = opFD.ShowDialog();
            if (dialogRes == DialogResult.OK)
            {
                String filename = opFD.FileName;
                FileInfo fInfo = new FileInfo(filename);
                //if (fInfo.DirectoryName == Directory.GetCurrentDirectory()) ThreadSafeOperations.SetTextTB(currFilenameTextbox, fInfo.Name, false);
                //else ThreadSafeOperations.SetTextTB(currFilenameTextbox, fInfo.FullName, false);

                imgData.currentColorScheme = new ColorScheme(fInfo.FullName);
                imgData.UpdateColorSchemeRuler();

                RaisePaintEvent(null, null);
            }



        }

        


        private void button1_Click(object sender, EventArgs e)
        {
            imgData.currentColorScheme = imgData.currentColorScheme.Reverse();
            imgData.UpdateColorSchemeRuler();
            RaisePaintEvent(null, null);
        }



        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            string fileName = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = (string) defaultProperties["DefaultDataFilesLocation"];
            sfd.Filter = "jpeg images | *.jpg";
            sfd.AddExtension = true;
            sfd.FileName = DateTime.UtcNow.ToString("s").Replace(":", "-") + ".jpg";
            DialogResult res = sfd.ShowDialog();
            if (res == DialogResult.OK)
            {
                fileName = sfd.FileName;
            }

            imgData.dataRepresentingImageColored().Save(fileName);
        }



        private void btnsaveImageAndScale_Click(object sender, EventArgs e)
        {
            string fileName = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = (string) defaultProperties["DefaultDataFilesLocation"];
            sfd.Filter = "jpeg images | *.jpg";
            sfd.AddExtension = true;
            sfd.FileName = DateTime.UtcNow.ToString("s").Replace(":", "-") + ".jpg";
            DialogResult res = sfd.ShowDialog();
            if (res == DialogResult.OK)
            {
                fileName = sfd.FileName;
            }

            imgData.dataRepresentingImageColored().Save(fileName);
            FileInfo imgFileInfo = new FileInfo(fileName);
            imgData.currentColorSchemeRuler.RulerBitmap(pbScale.Width, pbScale.Height)
                .Save(imgFileInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".scale.jpg");
        }
    }
}
