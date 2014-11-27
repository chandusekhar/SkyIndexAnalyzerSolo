using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MRG.Controls.UI;


namespace SkyImagesAnalyzerLibraries
{
    public class ThreadSafeOperations
    {
        delegate void SetTextCallback(System.Windows.Forms.Label textbox, string text, bool AppendMode);
        delegate void SetTextTBCallback(System.Windows.Forms.TextBox textbox, string text, bool AppendMode);
        delegate void UpdateProgressBarCallback(System.Windows.Forms.ProgressBar ProgressBarControl, int PBvalue);
        delegate int[] UpdatePictureBoxCallback(System.Windows.Forms.PictureBox PictureBoxControl, Image Image2Show, bool NormalizeImage = false);
        delegate void SetButtonEnabledStatusCallback(System.Windows.Forms.Button Control, bool ControlEnabled);
        delegate void SetTrackBarEnabledStatusCallback(System.Windows.Forms.TrackBar Control, bool ControlEnabled);
        delegate void MoveTrackBarCallback(System.Windows.Forms.TrackBar TrackBarControl, int TBValue);
        delegate void ToggleButtonStateCallback(System.Windows.Forms.Button ButtonControl, bool ControlEnabled, string ButtonText, bool ButtonFontBold);
        delegate void SimpleShowPictureCallback(Form theFormCalling, Image Image2Show, bool NormalizeImage = false);
        delegate void ToggleLabelTextColorCallback(Label lblTarget, Color color);
        delegate void SetLoadingCircleStateCallback(LoadingCircle lcControl, bool bActive, bool bVisible, Color controlColor, int rotationSpeed = 100);




        public ThreadSafeOperations()
        {

        }



        public static void SetLoadingCircleState(LoadingCircle lcControl, bool bActive, bool bVisible, Color controlColor, int rotationSpeed = 100)
        {
            if (lcControl.InvokeRequired)
            {
                SetLoadingCircleStateCallback d = SetLoadingCircleState;
                lcControl.Invoke(d, new object[] { lcControl, bActive, bVisible, controlColor, rotationSpeed });
            }
            else
            {
                lcControl.Active = bActive;
                lcControl.Visible = bVisible;
                lcControl.RotationSpeed = rotationSpeed;
                if (lcControl.Color != controlColor)
                {
                    lcControl.Color = controlColor;
                }
            }
        }




        public static void SetText(System.Windows.Forms.Label textbox, string text, bool AppendMode)
        {
            if (textbox.InvokeRequired)
            {
                SetTextCallback d = SetText;
                textbox.Invoke(d, new object[] { textbox, text, AppendMode });
            }
            else
            {
                if (AppendMode)
                {
                    textbox.Text += text;
                }
                else
                {
                    textbox.Text = text;
                }
            }
        }

        public static void SetTextTB(System.Windows.Forms.TextBox textbox, string text, bool AppendMode)
        {
            if (textbox == null)
            {
                return;
            }

            if (textbox.InvokeRequired)
            {
                SetTextTBCallback d = SetTextTB;
                textbox.Invoke(d, new object[] { textbox, text, AppendMode });
            }
            else
            {
                if (AppendMode)
                {
                    textbox.Text = text + textbox.Text;
                }
                else
                {
                    textbox.Text = text;
                }

            }
        }

        public static void UpdateProgressBar(System.Windows.Forms.ProgressBar ProgressBarControl, int PBvalue)
        {
            if (ProgressBarControl == null)
            {
                return;
            }

            if (ProgressBarControl.InvokeRequired)
            {
                UpdateProgressBarCallback d = UpdateProgressBar;
                ProgressBarControl.Invoke(d, new object[] { ProgressBarControl, PBvalue });
            }
            else
            {
                ProgressBarControl.Value = PBvalue;
            }
        }

        public static int[] UpdatePictureBox(System.Windows.Forms.PictureBox PictureBoxControl, Image Image2Show, bool NormalizeImage = false)
        {
            int[] retval = new int[2]; retval[0] = 0; retval[1] = 0;
            int[] nullretval = new int[2]; retval[0] = 0; retval[1] = 0;
            if (PictureBoxControl.InvokeRequired)
            {
                UpdatePictureBoxCallback d = UpdatePictureBox;
                PictureBoxControl.Invoke(d, new object[] { PictureBoxControl, Image2Show, NormalizeImage });
            }
            else
            {
                if (Image2Show == null)
                {
                    PictureBoxControl.Image = null;
                    return nullretval;
                }
                if (NormalizeImage)
                {
                    int th_width = PictureBoxControl.Width;
                    int th_height = (int)(Math.Round(((double)th_width / (double)Image2Show.Width) * (double)Image2Show.Height, 0));
                    if (th_height > PictureBoxControl.Height)
                    {
                        th_height = PictureBoxControl.Height;
                        th_width = (int)Math.Round((double)th_height * (double)Image2Show.Width / (double)Image2Show.Height);
                    }
                    PictureBoxControl.Image = Image2Show.GetThumbnailImage(th_width, th_height, null, IntPtr.Zero);
                    retval[0] = th_width;
                    retval[1] = th_height;
                }
                else
                {
                    PictureBoxControl.Image = Image2Show;
                    retval[0] = Image2Show.Width;
                    retval[1] = Image2Show.Height;
                }
            }
            return retval;
        }








        public static void SimpleShowPicture(Form theFormCalling, Image Image2Show, bool NormalizeImage = false)
        {
            if (theFormCalling.InvokeRequired)
            {
                SimpleShowPictureCallback d = SimpleShowPicture;
                theFormCalling.Invoke(d, new object[] { theFormCalling, Image2Show, NormalizeImage });
            }
            else
            {
                SimpleShowImageForm imgShowForm = new SimpleShowImageForm(Image2Show);
                imgShowForm.Show();
            }
        }










        public static void MoveTrackBar(System.Windows.Forms.TrackBar TrackBarControl, int TBValue)
        {
            if (TrackBarControl.InvokeRequired)
            {
                MoveTrackBarCallback d = MoveTrackBar;
                TrackBarControl.Invoke(d, new object[] { TrackBarControl, TBValue });
            }
            else
            {
                TrackBarControl.Value = TBValue;
            }
        }


        public static void ToggleButtonState(System.Windows.Forms.Button ButtonControl, bool ControlEnabled, string ButtonText, bool ButtonFontBold)
        {
            FontStyle newfontstyle;
            if (ButtonControl.InvokeRequired)
            {
                ToggleButtonStateCallback d = ToggleButtonState;
                ButtonControl.Invoke(d, new object[] { ButtonControl, ControlEnabled, ButtonText, ButtonFontBold });
            }
            else
            {
                if (ButtonFontBold)
                {
                    newfontstyle = FontStyle.Bold;
                }
                else
                {
                    newfontstyle = FontStyle.Regular;
                }
                System.Drawing.Font newfont = new System.Drawing.Font(ButtonControl.Font, newfontstyle);
                ButtonControl.Font = newfont;
                ButtonControl.Text = ButtonText;
                SetButtonEnabledStatus(ButtonControl, ControlEnabled);

            }
        }


        public static void SetButtonEnabledStatus(System.Windows.Forms.Button ButtonControl, bool ControlEnabled)
        {
            if (ButtonControl.InvokeRequired)
            {
                SetButtonEnabledStatusCallback d = SetButtonEnabledStatus;
                ButtonControl.Invoke(d, new object[] { ButtonControl, ControlEnabled });
            }
            else
            {
                ButtonControl.Enabled = ControlEnabled;
            }
        }



        public static void SetTrackBarEnabledStatus(System.Windows.Forms.TrackBar TrackBarControl, bool ControlEnabled)
        {
            if (TrackBarControl.InvokeRequired)
            {
                SetTrackBarEnabledStatusCallback d = SetTrackBarEnabledStatus;
                TrackBarControl.Invoke(d, new object[] { TrackBarControl, ControlEnabled });
            }
            else
            {
                TrackBarControl.Enabled = ControlEnabled;
            }
        }



        public static void ToggleLabelTextColor(Label lblTarget, Color color)
        {
            if (lblTarget.InvokeRequired)
            {
                ToggleLabelTextColorCallback d = ToggleLabelTextColor;
                lblTarget.Invoke(d, new object[] { lblTarget, color });
            }
            else
            {
                if ((lblTarget != null) && (lblTarget.ForeColor != color)) lblTarget.ForeColor = color;
            }
        }
    }



    //public class BGwindows
    //{
    //    public BackgroundWorker intBackgroundWorker = new BackgroundWorker();
    //    public Type theWindowType;
    //    public Form theWindow;
    //    private object exchangeBuffer = null;

    //    public delegate Form ShowAWindowDelegate(object parameters);

    //    public ShowAWindowDelegate windowShowDelegate;

    //    public BGwindows(Type inputType, ref object buffer)
    //    {
    //        exchangeBuffer = buffer;
    //        if (inputType == typeof(LogWindow))
    //        {
    //            intBackgroundWorker.DoWork += intBackgroundWorker_DoWork;
    //            intBackgroundWorker.WorkerSupportsCancellation = true;
    //            intBackgroundWorker.    
    //            intBackgroundWorker.RunWorkerAsync();
    //        }
    //    }

    //    private void intBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    //    {
    //        throw new NotImplementedException();

    //        while
    //    }


    //    public void LogAText(string theText, bool appendMode = true)
    //    {
    //        ThreadSafeOperations.SetTextTB(textBox1, theText + Environment.NewLine, appendMode);
    //    }
    //}

}