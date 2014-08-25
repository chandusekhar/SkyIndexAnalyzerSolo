using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Threading;
using DirectShowLib;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using SkyIndexAnalyzerLibraries;



namespace SkyIndexAnalyzerSolo
{
    public partial class SkyIndexAnalyzerDataCollector : Form
    {
        delegate void SetTextCallback(System.Windows.Forms.Label textbox, string text, bool AppendMode);
        delegate void SetTextTBCallback(System.Windows.Forms.TextBox textbox, string text, bool AppendMode);
        delegate void UpdateProgressBarCallback(System.Windows.Forms.ProgressBar ProgressBarControl, int PBvalue);
        delegate void UpdatePictureBoxCallback(System.Windows.Forms.PictureBox PictureBoxControl, Image Image2Show);
        delegate void SetButtonEnabledStatusCallback(System.Windows.Forms.Button Control, bool ControlEnabled);
        delegate void SetTrackBarEnabledStatusCallback(System.Windows.Forms.TrackBar Control, bool ControlEnabled);
        delegate void MoveTrackBarCallback(System.Windows.Forms.TrackBar TrackBarControl, int TBValue);
        delegate void ToggleButtonStateCallback(System.Windows.Forms.Button ButtonControl, bool ControlEnabled, string ButtonText, bool ButtonFontBold);
        delegate void ShowPictureCallback(String FilePath, int timeout = 0);
        public int capturedimage;
        private Capture cam;
        IntPtr m_ip = IntPtr.Zero;
        //private int CamImageWidth, CamImageHeight;
        private bool VitaminControlConnected = false;
        public int SensorsCycleDataGotTimes;



        
        public SkyIndexAnalyzerDataCollector()
        {
            InitializeComponent();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);


            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }




        private void ShowPicture(String FilePath, int timeout = 0)
        {
            if (this.InvokeRequired)
            {
                ShowPictureCallback d = ShowPicture;
                this.Invoke(d, new object[] { FilePath, timeout });
            }
            else
            {
                Bitmap bitmap2show = new Bitmap(FilePath);
                SimpleShowImageForm SimpleShowImageForm1 = new SimpleShowImageForm(bitmap2show);
                SimpleShowImageForm1.Show();
                SimpleShowImageForm1.SendToBack();
            }
        }



        private static void SetText(System.Windows.Forms.Label textbox, string text, bool AppendMode)
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



        private static void SetTextTB(System.Windows.Forms.TextBox textbox, string text, bool AppendMode)
        {
            if (textbox.InvokeRequired)
            {
                SetTextTBCallback d = SetTextTB;
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



        private static void UpdateProgressBar(System.Windows.Forms.ProgressBar ProgressBarControl, int PBvalue)
        {
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



        private static void UpdatePictureBox(System.Windows.Forms.PictureBox PictureBoxControl, Image Image2Show)
        {
            if (PictureBoxControl.InvokeRequired)
            {
                UpdatePictureBoxCallback d = UpdatePictureBox;
                PictureBoxControl.Invoke(d, new object[] { PictureBoxControl, Image2Show });
            }
            else
            {
                PictureBoxControl.Image = Image2Show;
            }
        }



        private static void MoveTrackBar(System.Windows.Forms.TrackBar TrackBarControl, int TBValue)
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



        private void cbCollectGPS_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb1;
            cb1 = sender as System.Windows.Forms.CheckBox;
            if (GPSDataCollectingCycle.IsBusy)
            {
                GPSDataCollectingCycle.CancelAsync();
            }
            else
            {
                cb1.BackColor = Color.Honeydew;
                GPSDataCollectingCycle.RunWorkerAsync();
            }
        }



        private void GPSDataCollectingCycle_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            while (true)
            {
                SetTextTB(textBox1, "Продолжаю собирать данные GPS. Это пока сообщение-заглушка" + Environment.NewLine, true);
                System.Threading.Thread.Sleep(2000);
                if (SelfWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }



        public void Note(string text)
        {
            SetTextTB(textBox1, text + Environment.NewLine, true);
        }



        private void GPSDataCollectingCycle_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }



        private void GPSDataCollectingCycle_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CheckBox cbGPS = this.cbCollectGPS;
            cbGPS.Checked = false;
            cbGPS.BackColor = Color.MistyRose;
        }




        private void SkyIndexAnalyzerDataCollector_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                DataCollectorNotifyIcon.Visible = true;
                DataCollectorNotifyIcon.ShowBalloonTip(10, "Skyindex Analyzer data collector", "Приложение свернуто в трей и доступно по клику мышью на этой иконке или кликом прямо по этому сообщению.", ToolTipIcon.Info);
                this.Hide();
            }
        }



        private void DataCollectorNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            DataCollectorNotifyIcon.Visible = false;
        }



        private void DataCollectorNotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            DataCollectorNotifyIcon.Visible = false;
        }



        private void cbCollectCompass_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb1;
            cb1 = sender as System.Windows.Forms.CheckBox;
            if (cb1.Checked)
            {
                cb1.BackColor = Color.Honeydew;
            }
            else
            {
                cb1.BackColor = Color.MistyRose;
            }
        }



        private void cbCollectAccelerometer_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb1;
            cb1 = sender as System.Windows.Forms.CheckBox;
            if (cb1.Checked)
            {
                cb1.BackColor = Color.Honeydew;
            }
            else
            {
                cb1.BackColor = Color.MistyRose;
            }
        }



        private void cbGetPhotos_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb1;
            cb1 = sender as System.Windows.Forms.CheckBox;
            if (cb1.Checked)
            {
                cb1.BackColor = Color.Honeydew;
            }
            else
            {
                cb1.BackColor = Color.MistyRose;
            }
        }



        

        private void SkyIndexAnalyzerDataCollector_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cam != null)
            {
                cam.Dispose();
                cam = null;
            }

            
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }

        private void SkyIndexAnalyzerDataCollector_Load(object sender, EventArgs e)
        {
            
        }





        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cam != null)
        //    {
        //        cam.Dispose();
        //        cam = null;
        //    }
        //
        //    const int VIDEOBITSPERPIXEL = 24;
        //    ComboBox currentComboBox = (ComboBox)sender;
        //    int index = currentComboBox.SelectedIndex;
        //    cam = new Capture(index, 0, 0, VIDEOBITSPERPIXEL, pictureBox1);
        //    CamImageWidth = cam.Width;
        //    CamImageHeight = cam.Height;
        //}



        //private void ConnectIPCam()
        //{
        //    // Подключаемся к Vivotek камере
        //    string strPreIP = "http://";
        //    axVitaminCtrl1.Url = strPreIP + "192.168.192.156";
        //    axVitaminCtrl1.HttpPort = Convert.ToInt32("80");
        //    axVitaminCtrl1.UserName = "root";
        //    axVitaminCtrl1.Password = "vivotek";
        //    //axVitaminCtrl1.ViewStream = (VITAMINDECODERLib.EDualStreamOption)0;
        //    axVitaminCtrl1.ViewStream = (VITAMINDECODERLib.EDualStreamOption)1;
        //    //axVitaminCtrl1.ConnectionProtocol = (VITAMINDECODERLib.EConnProtocol)ComboProtocol.SelectedIndex.GetHashCode() + 1;
        //    axVitaminCtrl1.ConnectionProtocol = (VITAMINDECODERLib.EConnProtocol)2;
        //    axVitaminCtrl1.Connect();
        //}




        //private String GetIPCamShot()
        //{
        //    object pvData;
        //    object pvInfo;
        //    //long plRet;
        //    String FilePath = "";

        //    string CurDir = Directory.GetCurrentDirectory();
        //    if (VitaminControlConnected)
        //    {
        //        axVitaminCtrl1.GetSnapshot(VITAMINDECODERLib.EPictureFormat.ePicFmtBmp, out pvData, out pvInfo);
        //        FilePath = CurDir + "\\" +
        //                            DateTime.Now.Year + "-" +
        //                            DateTime.Now.Month + "-" +
        //                            DateTime.Now.Day + "-" +
        //                            DateTime.Now.Hour + "-" +
        //                            DateTime.Now.Minute + "-" +
        //                            DateTime.Now.Second + ".bmp";
        //        axVitaminCtrl1.SaveSnapshot(VITAMINDECODERLib.EPictureFormat.ePicFmtBmp, FilePath);
        //        //pictureBox1.Image = (Image)pvData;
        //    }
        //    return FilePath;
        //}



        private void button1_Click(object sender, EventArgs e)
        {
            //ConnectIPCam();
        }

        private void CapDevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void axVitaminCtrl1_OnClick(object sender, AxVITAMINDECODERLib._IVitaminCtrlEvents_OnClickEvent e)
        //{
        //    GetIPCamShot();
        //}

        //private void axVitaminCtrl1_OnConnectionOK(object sender, AxVITAMINDECODERLib._IVitaminCtrlEvents_OnConnectionOKEvent e)
        //{
        //    VitaminControlConnected = true;
        //}

        //private void axVitaminCtrl1_OnConnectionBroken(object sender, AxVITAMINDECODERLib._IVitaminCtrlEvents_OnConnectionBrokenEvent e)
        //{
        //    VitaminControlConnected = false;
        //}






        private void button2_Click(object sender, EventArgs e)
        {
            if (GetSensorsDataCycle.IsBusy)
            {
                GetSensorsDataCycle.CancelAsync();
            }
            else
            {
                GetSensorsDataCycle.RunWorkerAsync();
            }
        }

        private void GetSensorsDataCycle_DoWork(object sender, DoWorkEventArgs e)
        {
            //int result, remainder;
            ThreadSafeOperations.ToggleButtonState(button2, true, "Остановить сбор данных", true);
            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            while (true)
            {
                SensorsCycleDataGotTimes++;
                //result = Math.DivRem((int)SensorsCycleDataGotTimes, (int)10, out remainder);
                //if (remainder == 0)
                //{
                GetSensorsDataCycle.ReportProgress(SensorsCycleDataGotTimes);
                //}

                if (SelfWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                System.Threading.Thread.Sleep(1000);
                getData();
            }
        }





        private void getData()
        {
            //Bitmap bitmap2show;
            bool _continue = true;
            string message = "";
            SerialPort sp = new SerialPort();
            sp.PortName = "COM2";
            int counter = 0;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);

            if (cbCollectGPS.Checked)
            {
                sp.ReadTimeout = 500;
                try
                {
                    sp.Open();
                }
                catch (Exception)
                {
                    SetTextTB(textBox1, "Не удалось открыть COM-порт: " + Environment.NewLine, true);
                    return;
                }



                while (_continue)
                {
                    try
                    {
                        message = sp.ReadLine() + Environment.NewLine + sp.ReadLine() + Environment.NewLine + sp.ReadLine() + Environment.NewLine;

                        _continue = false;
                    }
                    catch (TimeoutException)
                    {
                        SetTextTB(textBox1, "havent got data yet: " + (counter * 500).ToString() + Environment.NewLine, true);
                        counter++;
                    }
                    SetTextTB(textBox1, message + Environment.NewLine, true);
                    if (counter == 20) _continue = false;
                }
                sp.Close();
            }


            if (cbGetPhotosIPCam.Checked)
            {
                if (!VitaminControlConnected)
                {
                    //ConnectIPCam();
                }

                //String FilePath = GetIPCamShot();
                //ShowPicture(FilePath, 0);
                //bitmap2show = new Bitmap(FilePath);
                //(new SimpleShowImageForm(bitmap2show)).Show();
                //cbGetPhotosIPCam.Checked = false;
                //GetSensorsDataCycle.CancelAsync();
            }



            if (cbGetUDPdataFromIOSdevice.Checked)
            {
                //DataSocketReader reader = new DataSocketReader();
                socket.Bind(new IPEndPoint(IPAddress.Parse("192.168.192.153"), 0)); // specify IP address
                socket.ReceiveBufferSize = 2 * 1024 * 1024; // 2 megabytes
                socket.ReceiveTimeout = 500; // half a second
                byte[] incoming = BitConverter.GetBytes(1);
                byte[] outgoing = BitConverter.GetBytes(1);
                socket.IOControl(IOControlCode.ReceiveAll, incoming, outgoing);
                byte[] buf = new byte[64];
                socket.Receive(buf);
                string bufdata = BitConverter.ToString(buf);
                Note(bufdata);
                socket.Close();
            }
        }

        private void GetSensorsDataCycle_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ThreadSafeOperations.SetTextTB(textBox1, "Данные датчиков собраны " + e.ProgressPercentage + " раз" + Environment.NewLine, true);
        }

        private void GetSensorsDataCycle_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.SetTextTB(textBox1, "Получение данных сенсоров остановлено" + Environment.NewLine, true);
            ThreadSafeOperations.ToggleButtonState(button2, true, "Начать сбор данных", false);
        }

        private void SkyIndexAnalyzerDataCollector_Paint(object sender, PaintEventArgs e)
        {
            if (GetSensorsDataCycle.IsBusy)
            {
                button2.BackColor = Color.LightCoral;
            }
            else
            {
                button2.BackColor = SystemColors.Control;
            }
        }

    }
}
