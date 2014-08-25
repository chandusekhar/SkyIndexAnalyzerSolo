using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
//using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.IO;
using ArduinoUDPconversation.Properties;


namespace ArduinoUDPconversation
{
    struct accelerometerData
    {
        public int accX;
        public int accY;
        public int accZ;
        public double accDoubleX;
        public double accDoubleY;
        public double accDoubleZ;
    }


    struct gyroscopeData
    {
        public int gyroX;
        public int gyroY;
        public int gyroZ;
        public double gyroDoubleX;
        public double gyroDoubleY;
        public double gyroDoubleZ;
    }

    public partial class ArduinoConversationForm : Form
    {
        delegate void SetTextCallback(Label textbox, string text, bool AppendMode);
        delegate void SetTextTBCallback(TextBox textbox, string text, bool AppendMode);
        delegate void UpdateProgressBarCallback(ProgressBar ProgressBarControl, int PBvalue);
        delegate void UpdatePictureBoxCallback(PictureBox PictureBoxControl, Image Image2Show);
        delegate void SetButtonEnabledStatusCallback(Button Control, bool ControlEnabled);
        delegate void SetTrackBarEnabledStatusCallback(TrackBar Control, bool ControlEnabled);
        delegate void MoveTrackBarCallback(TrackBar TrackBarControl, int TBValue);
        delegate void ToggleButtonStateCallback(Button ButtonControl, bool ControlEnabled, string ButtonText, bool ButtonFontBold);
        delegate void ShowPictureCallback(String FilePath, int timeout = 0);
        //private DataSocketReader reader;
        //private UdpClient UDPreader;
        //public VisualizingForm VForm;
        private string ip2Listen = "";
        private int port2converse = 5555;
        private int portBcstRecvng = 4444;
        private string currCommand;
        private static bool bcstMessageReceived = false;
        //private static String bcstMessage = "";
        //private static bool udpMessageReceived = false;
        private static String udpMessage = "";
        private static bool needsToDiscoverArduinoBoard = false;
        private static DateTime JanFirst1970 = new DateTime(1970, 1, 1);
        private static bool recievedData = false;
        IntPtr m_ip = IntPtr.Zero;
        private static accelerometerData accData;
        private static bool accDatahasBeenChanged = false;
        private static gyroscopeData gyroData;
        private static bool gyroDatahasBeenChanged = false;


        private static Queue<string> quArduinoUDPCatchedMessages = new Queue<string>();
        private static SocketAddress remoteSktAddr;


        public ArduinoConversationForm()
        {
            InitializeComponent();
        }






        #region stuff
        private static void SetText(Label textbox, string text, bool AppendMode)
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
        private static void SetTextTB(TextBox textbox, string text, bool AppendMode)
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
                    textbox.Text = text + textbox.Text;
                }
                else
                {
                    textbox.Text = text;
                }

            }
        }
        private static void UpdateProgressBar(ProgressBar ProgressBarControl, int PBvalue)
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
        private static void UpdatePictureBox(PictureBox PictureBoxControl, Image Image2Show)
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
        private static void MoveTrackBar(TrackBar TrackBarControl, int TBValue)
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
        public static void ToggleButtonState(Button ButtonControl, bool ControlEnabled, string ButtonText, bool ButtonFontBold)
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
                Font newfont = new Font(ButtonControl.Font, newfontstyle);
                ButtonControl.Font = newfont;
                ButtonControl.Text = ButtonText;
                SetButtonEnabledStatus(ButtonControl, ControlEnabled);

            }
        }
        public static void SetButtonEnabledStatus(Button ButtonControl, bool ControlEnabled)
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
        public void Note(string text, TextBox tb2update)
        {
            SetTextTB(tb2update, text + Environment.NewLine, true);
            if ((tb2update == tbBcstListeningLog) && (tb2update.Lines.Length > Settings.Default.BroadcastLogHistorySizeLines))
            {
                swapBcstLog();
                Note(text, tb2update);
            }


            if ((tb2update == tbResponseLog) && (tb2update.Lines.Length > Settings.Default.BroadcastLogHistorySizeLines))
            {
                swapResponseLog();
                Note(text, tb2update);
            }
        }
        public void Note(string text)
        {
            SetTextTB(tbResponseLog, text + Environment.NewLine, true);
        }
        #endregion


        //#region GetSensorsDataCycle


        //private void ParseBroadcastMessage(string bcstMessage)
        //{
        //    string dataSign = bcstMessage.Substring(1, 3);
        //    string dataValuesString = bcstMessage.Substring(5);
        //    if (bcstMessage.StartsWith("<") && bcstMessage.Substring(4, 1) == ">")
        //    {
        //        switch (dataSign)
        //        {
        //            case "dta":
        //                {
        //                    //accelerometer data
        //                    string[] splitters = { ";" };
        //                    string[] stringAccValues = dataValuesString.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
        //                    int[] accValues = { Convert.ToInt32(stringAccValues[0]), Convert.ToInt32(stringAccValues[1]), Convert.ToInt32(stringAccValues[2]) };
        //                    if (GetSensorsDataCycle.IsBusy)
        //                    {
        //                        accData.accX = accValues[0];
        //                        accData.accY = accValues[1];
        //                        accData.accZ = accValues[2];
        //                        accData.accDoubleX = (double)accValues[0];
        //                        accData.accDoubleY = (double)accValues[1];
        //                        accData.accDoubleZ = (double)accValues[2];
        //                        accDatahasBeenChanged = true;
        //                    }
        //                    break;
        //                }
        //            case "dtg":
        //                {
        //                    //gyroscope data
        //                    string[] splitters = { ";" };
        //                    string[] stringValues = dataValuesString.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
        //                    int[] gyroValues = { Convert.ToInt32(stringValues[0]), Convert.ToInt32(stringValues[1]), Convert.ToInt32(stringValues[2]) };
        //                    if (GetSensorsDataCycle.IsBusy)
        //                    {
        //                        gyroData.gyroX = gyroValues[0];
        //                        gyroData.gyroY = gyroValues[1];
        //                        gyroData.gyroZ = gyroValues[2];
        //                        gyroData.gyroDoubleX = (double)gyroValues[0];
        //                        gyroData.gyroDoubleY = (double)gyroValues[1];
        //                        gyroData.gyroDoubleZ = (double)gyroValues[2];
        //                        gyroDatahasBeenChanged = true;
        //                    }
        //                    break;
        //                }
        //            case "dtp":
        //                {
        //                    //pressure data
        //                    break;
        //                }
        //            case "dtm":
        //                {
        //                    //magnerometer data
        //                    break;
        //                }
        //            default:
        //                break;
        //        }

        //    }
        //}


        //private void GetSensorsDataCycle_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //int result, remainder;
        //    ToggleButtonState(btnResponseLog, true, "Stop processing incoming data", true);
        //    System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;

        //    while (true)
        //    {
        //        if (SelfWorker.CancellationPending)
        //        {
        //            e.Cancel = true;
        //            break;
        //        }

        //        if (recievedData)
        //        {
        //            ParseBroadcastMessage(udpMessage);
        //            //updateDataLabels();

        //            recievedData = false;
        //            accDatahasBeenChanged = false;
        //            gyroDatahasBeenChanged = false;
        //        }

        //        //System.Threading.Thread.Sleep(100);

        //    }
        //}


        

        //private void GetSensorsDataCycle_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    Note("Получение данных сенсоров остановлено");
        //    ToggleButtonState(btnResponseLog, true, "Start processing incoming data", false);
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (GetSensorsDataCycle.IsBusy)
        //    {
        //        GetSensorsDataCycle.CancelAsync();
        //    }
        //    else
        //    {
        //        GetSensorsDataCycle.RunWorkerAsync();
        //    }
        //}

        //#endregion



        //private String ParseDataString(String inDataString)
        //{
        //    String outDataString = "";
        //    String[] dataArray = inDataString.Split(new char[] { ',' });
        //    long dateticks = Convert.ToInt64(dataArray[0]);
        //    DateTime curDate = JanFirst1970.AddMilliseconds((double)dateticks).ToLocalTime();
        //    if (dataArray.Count() < 2)
        //    {
        //        SetTextTB(textBox1, curDate + Environment.NewLine + inDataString + Environment.NewLine, true);
        //        return inDataString;
        //    }

        //    outDataString += Environment.NewLine + curDate.ToString() + Environment.NewLine;

        //    int inDataType = Convert.ToInt16(dataArray[1]);
        //    string strDataType = "unknown";
        //    switch (inDataType)
        //    {
        //        case 1:
        //            {
        //                strDataType = "gps";
        //                break;
        //            }
        //        case 2:
        //            {
        //                strDataType = "compass";
        //                break;
        //            }
        //        case 3:
        //            {
        //                strDataType = "accel";
        //                break;
        //            }
        //        case 4:
        //            {
        //                strDataType = "gyro";
        //                break;
        //            }
        //        default:
        //            {
        //                SetTextTB(textBox1, curDate + Environment.NewLine + inDataString + Environment.NewLine, true);
        //                return inDataString;
        //            }
        //    }

        //    switch (strDataType)
        //    {
        //        case "gps":
        //            {
        //                GPSdata sensorData = new GPSdata();
        //                sensorData.date = curDate;
        //                sensorData.latitude = Convert.ToDouble(dataArray[2].Replace(".", ","));
        //                sensorData.longitude = Convert.ToDouble(dataArray[3].Replace(".", ","));
        //                sensorData.altitude = Convert.ToDouble(dataArray[4].Replace(".", ","));
        //                sensorData.horizAccuracy = Convert.ToDouble(dataArray[5].Replace(".", ","));
        //                sensorData.vertAccuracy = Convert.ToDouble(dataArray[6].Replace(".", ","));

        //                SetText(GPSDateLabel, sensorData.date.ToString(), false);
        //                SetText(GPSLatitudeLabel, sensorData.latitude.ToString(), false);
        //                SetText(GPSLongitudeLabel, sensorData.longitude.ToString(), false);
        //                SetText(GPSAltitudeLabel, sensorData.altitude.ToString(), false);
        //                SetText(GPSHorizAccLabel, sensorData.horizAccuracy.ToString(), false);
        //                SetText(GPSVertAccLabel, sensorData.vertAccuracy.ToString(), false);


        //                break;
        //            }
        //        case "compass":
        //            {
        //                CompassData sensorData = new CompassData();
        //                sensorData.date = curDate;
        //                sensorData.MagneticHeading = Convert.ToDouble(dataArray[2].Replace(".", ","));
        //                sensorData.trueHeading = Convert.ToDouble(dataArray[3].Replace(".", ","));

        //                SetText(CompassDateLabel, sensorData.date.ToString(), false);
        //                SetText(CompassMagheadLabel, sensorData.MagneticHeading.ToString(), false);
        //                SetText(CompassTrueheadLabel, sensorData.trueHeading.ToString(), false);

        //                break;
        //            }
        //        case "accel":
        //            {
        //                AccelerometerData sensorData = new AccelerometerData();
        //                sensorData.date = curDate;
        //                sensorData.AccelerationX = Convert.ToDouble(dataArray[2].Replace(".", ","));
        //                sensorData.AccelerationY = Convert.ToDouble(dataArray[3].Replace(".", ","));
        //                sensorData.AccelerationZ = Convert.ToDouble(dataArray[4].Replace(".", ","));

        //                SetText(AccelDateLabel, sensorData.date.ToString(), false);
        //                SetText(AccelXLabel, sensorData.AccelerationX.ToString(), false);
        //                SetText(AccelYLabel, sensorData.AccelerationY.ToString(), false);
        //                SetText(AccelZLabel, sensorData.AccelerationZ.ToString(), false);


        //                VForm.ClearImage();
        //                double width = VForm.bmpObject.Width;
        //                double height = VForm.bmpObject.Height;
        //                PointF center = new PointF((float)(width / 2.0), (float)(height / 2.0));
        //                double length = 100.0;
        //                double dx = sensorData.AccelerationX;
        //                double dy = sensorData.AccelerationY;
        //                double dl = Math.Sqrt(dx * dx + dy * dy);
        //                double dz = sensorData.AccelerationZ;
        //                double tg = Math.Sqrt(dx * dx + dy * dy) / dz;

        //                PointF p1 = new PointF();
        //                p1.X = center.X - (float)(0.5 * length * Math.Cos(Math.Atan(tg)));
        //                p1.Y = center.Y - (float)(0.5 * length * Math.Sin(Math.Atan(tg)));
        //                PointF p2 = new PointF();
        //                p2.X = center.X + (float)(0.5 * length * Math.Cos(Math.Atan(tg)));
        //                p2.Y = center.Y + (float)(0.5 * length * Math.Sin(Math.Atan(tg)));

        //                VForm.DrawLine(p1, p2, Color.Black, (float)2.0);

        //                break;
        //            }
        //        case "gyro":
        //            {
        //                GyroscopeData sensorData = new GyroscopeData();
        //                sensorData.date = curDate;
        //                sensorData.GyroscopeX = Convert.ToDouble(dataArray[2].Replace(".", ","));
        //                sensorData.GyroscopeY = Convert.ToDouble(dataArray[3].Replace(".", ","));
        //                sensorData.GyroscopeZ = Convert.ToDouble(dataArray[4].Replace(".", ","));

        //                SetText(AccelDateLabel, sensorData.date.ToString(), false);
        //                SetText(GyroXLabel, sensorData.GyroscopeX.ToString(), false);
        //                SetText(GyroYLabel, sensorData.GyroscopeY.ToString(), false);
        //                SetText(GyroZLabel, sensorData.GyroscopeZ.ToString(), false);

        //                //SetTextTB(textBox1, curDate + Environment.NewLine + inDataString + Environment.NewLine, true);
        //                //return inDataString;

        //                break;
        //            }
        //        default:
        //            {
        //                SetTextTB(textBox1, curDate + Environment.NewLine + inDataString + Environment.NewLine, true);
        //                return inDataString;
        //                //break;
        //            }
        //    }


        //    return outDataString;
        //}



        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ip2Listen = textBox2.Text;
        }



        private void btnFindArduino_Click(object sender, EventArgs e)
        {
            if (arduinoBoardSearchingWorker.IsBusy)
            {
                arduinoBoardSearchingWorker.CancelAsync();

                //UpdateProgressBar(progressBar1, 0);
            }
            else
            {
                arduinoBoardSearchingWorker.RunWorkerAsync();
            }
        }



        private void arduinoBoardSearchingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            needsToDiscoverArduinoBoard = true;
            bool needsToSwitchCatchingOff = false;
            if (!udpCatchingJob.IsBusy)
            {
                //start catching and the switch it off
                btnStartStopBdcstListening_Click(null, null);
                needsToSwitchCatchingOff = true;
            }


            while (needsToDiscoverArduinoBoard)
            {
                if (SelfWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }


            if (needsToSwitchCatchingOff)
            {
                btnStartStopBdcstListening_Click(null, null);
                needsToSwitchCatchingOff = false;
            }
        }



        //private bool testAnIpAddress(IPAddress testingIPaddr)
        //{
        //    bool retVal = false;
        //    Ping ping;
        //    string retStr = "areyouarduino";
        //    byte[] ret = System.Text.Encoding.ASCII.GetBytes(retStr);
        //    string returnData = "";
        //    Socket Skt = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //    Skt.EnableBroadcast = false;
        //    Skt.ReceiveTimeout = 10;
        //    Skt.SendTimeout = 10;
        //    //string testingIP = "192.168.192." + i.ToString();//!!!!!!!!!!!!!!!!!!!!!! определить подсеть по текущему IP и маске
        //    //testingIPaddr = IPAddress.Parse(testingIP);

        //    ping = new Ping();
        //    PingReply repl = ping.Send(testingIPaddr, 3, new byte[] { 1 }, new PingOptions(1, true));

        //    if ((repl.Status == IPStatus.TtlExpired) || (repl.Status == IPStatus.TimedOut) || (repl.Status == IPStatus.TimeExceeded))
        //    {
        //        returnData = "ping timeout or not reachable";
        //        retVal = false;
        //    }
        //    else
        //    {
        //        IPEndPoint test = new IPEndPoint(testingIPaddr, 5555);
        //        int sent = Skt.SendTo(ret, test);
        //        // Blocks until a message returns on this socket from a remote host.
        //        Byte[] receiveBytes = new byte[128];
        //        IPEndPoint sender_ = new IPEndPoint(IPAddress.Any, 0);
        //        EndPoint senderRemote = (EndPoint)sender_;

        //        try
        //        {
        //            Skt.ReceiveFrom(receiveBytes, ref senderRemote);
        //            returnData = Encoding.UTF8.GetString(receiveBytes).Trim().Replace("\0", string.Empty);
        //        }
        //        catch (Exception ex)
        //        {
        //            returnData = ex.Message;
        //            //throw;
        //        }
        //    }

        //    SetTextTB(textBox1, "tested ip " + testingIPaddr.ToString() + ": " + returnData.ToString() + Environment.NewLine, true);
        //    Skt.Close();
        //    if (returnData.ToString() == "YES") retVal = true; else retVal = false;

        //    return retVal;
        //}



        //private void arduinoBoardSearchingWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    //SetTextTB(textBox1, "Данные датчиков собраны " + e.ProgressPercentage + " раз" + Environment.NewLine, true);
        //    UpdateProgressBar(progressBar1, e.ProgressPercentage);
        //}



        private void Form1_Load(object sender, EventArgs e)
        {
            ip2Listen = Settings.Default.ArduinoBoardDefaultIP;
            port2converse = Settings.Default.ArduinoBoardDefaultUDPport;
            portBcstRecvng = Settings.Default.UDPBroadcastDefaultListeningPort;
            SetTextTB(textBox2, ip2Listen, false);
            SetTextTB(tbBcstListeningPort, portBcstRecvng.ToString(), false);
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ip2Listen != Settings.Default.ArduinoBoardDefaultIP)
            {
                Settings.Default.ArduinoBoardDefaultIP = ip2Listen;
                Settings.Default.Save();
            }

            if (port2converse != Settings.Default.ArduinoBoardDefaultUDPport)
            {
                Settings.Default.ArduinoBoardDefaultUDPport = port2converse;
                Settings.Default.Save();
            }


            if (portBcstRecvng != Settings.Default.UDPBroadcastDefaultListeningPort)
            {
                Settings.Default.UDPBroadcastDefaultListeningPort = portBcstRecvng;
                Settings.Default.Save();
            }
        }



        private void tbBcstListeningPort_TextChanged(object sender, EventArgs e)
        {
            portBcstRecvng = Convert.ToInt32(tbBcstListeningPort.Text);
        }



        private void btnStartStopBdcstListening_Click(object sender, EventArgs e)
        {
            if (udpCatchingJob.IsBusy)
            {
                udpCatchingJob.CancelAsync();
            }
            else
            {
                udpCatchingJob.RunWorkerAsync();
                StartUDPmessagesParser();
                ToggleButtonState(btnStartStopBdcstListening, true, "Stop listening", true);
            }
        }


        private void StartUDPmessagesParser()
        {
            if (!bgwUDPmessagesParser.IsBusy)
            {
                bgwUDPmessagesParser.RunWorkerAsync();
            }
        }




        private void udpCatchingJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ToggleButtonState(btnStartStopBdcstListening, true, "Start listening", false);
            bgwUDPmessagesParser.CancelAsync();
        }


        private static bool recievingUDPmessage = false;
        public static void bcstReceiveCallback(IAsyncResult ar)
        {
            string bcstMessage = "";
            UdpState udpSt = (UdpState)(ar.AsyncState);
            UdpClient udpClt = (UdpClient)(udpSt.UDPclient);
            IPEndPoint ipEP = (IPEndPoint)(udpSt.ipEndPoint);

            remoteSktAddr = PropertyHelper.GetPrivatePropertyValue<SocketAddress>((object)ar, "SocketAddress");
            udpSt.sktAddress = remoteSktAddr;

            if (udpClt.Client != null)
            {
                try
                {
                    Byte[] receiveBytes = udpClt.EndReceive(ar, ref ipEP);
                    recievingUDPmessage = false;

                    bcstMessage = Encoding.ASCII.GetString(receiveBytes);

                    if (bcstMessage != "")
                    {
                        quArduinoUDPCatchedMessages.Enqueue(bcstMessage);

                        bcstMessageReceived = true;
                    }
                }
                catch (Exception exc)
                {
                    bcstMessageReceived = false;
                    bcstMessage = exc.Message;
                }
                udpSt.udpMessage = bcstMessage;
            }
            else
            {
                bcstMessageReceived = false;
                recievingUDPmessage = false;
            }
        }






        private void textBoxCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                btnCancelWaitingResponse_Click(null, null);
            }
        }




        private void PerformSendCommand()
        {
            string retStr = currCommand;
            byte[] ret = Encoding.ASCII.GetBytes(retStr);
            Socket Skt = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Skt.EnableBroadcast = false;
            IPEndPoint ipEP = new IPEndPoint(IPAddress.Parse(ip2Listen), port2converse);

            int sent = Skt.SendTo(ret, ipEP);
            Skt.Close();
            Skt.Dispose();
        }



        private void udpCatchingJob_DoWork(object sender, DoWorkEventArgs e)
        {
            UdpClient bcstUDPreader = new UdpClient(portBcstRecvng, AddressFamily.InterNetwork);
            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpState s = new UdpState();
            s.ipEndPoint = RemoteIpEndPoint;
            s.UDPclient = bcstUDPreader;

            while (true)
            {
                if (SelfWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                if (recievingUDPmessage)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                    continue;
                }

                bcstMessageReceived = false;
                recievingUDPmessage = true;
                bcstUDPreader.BeginReceive(bcstReceiveCallback, s);
            }

            bcstUDPreader.Close();
        }





        private void btnCancelWaitingResponse_Click(object sender, EventArgs e)
        {
            currCommand = textBoxCommand.Text.Trim().Replace("\0", string.Empty);
            if (currCommand.Length == 0)
            {
                return;
            }
            SetTextTB(textBoxCommand, "", false);
            SetTextTB(tbResponseLog, ">>> " + currCommand + Environment.NewLine, true);
            PerformSendCommand();
        }



        private void btnSwapBcstLog_Click(object sender, EventArgs e)
        {
            swapBcstLog();
        }

        private void btnClearBcstLog_Click(object sender, EventArgs e)
        {
            tbBcstListeningLog.Text = "";
        }


        private void swapBcstLog()
        {
            string filename1;
            string strtowrite;
            byte[] info2write;

            filename1 = Directory.GetCurrentDirectory() + "\\BcstLog-" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".log";
            FileStream dataFile = new FileStream(filename1, FileMode.Append, FileAccess.Write);
            strtowrite = tbBcstListeningLog.Text;
            info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            dataFile.Write(info2write, 0, info2write.Length);
            dataFile.Close();
            SetTextTB(tbBcstListeningLog, "", false);
        }


        private void swapResponseLog()
        {
            string filename1;
            string strtowrite;
            byte[] info2write;

            filename1 = Directory.GetCurrentDirectory() + "\\ResponseLog-" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".log";
            FileStream dataFile = new FileStream(filename1, FileMode.Append, FileAccess.Write);
            strtowrite = tbResponseLog.Text;
            info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            dataFile.Write(info2write, 0, info2write.Length);
            dataFile.Close();
            SetTextTB(tbResponseLog, "", false);
        }


        private void arduinoBoardSearchingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ToggleButtonState(btnFindArduino, true, "Search for Arduino board", false);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            swapResponseLog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbResponseLog.Text = "";
        }
        







        private void bgwUDPmessagesParser_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            int currMessageDevID = 0;

            while (true)
            {
                if (selfWorker.CancellationPending && quArduinoUDPCatchedMessages.Count == 0)
                {
                    break;
                }

                if (quArduinoUDPCatchedMessages.Count == 0)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                    continue;
                }

                if (quArduinoUDPCatchedMessages.Count > 0)
                {
                    string bcstMessage = quArduinoUDPCatchedMessages.Dequeue();

                    if (bcstMessage == null) continue;
                    
                    if ((bcstMessage.Length >= 5) && (bcstMessage.Substring(0, 3) == "<id"))
                    {
                        int idx = bcstMessage.IndexOf('>');
                        string strDevIDTag = bcstMessage.Substring(0, idx + 1); // "<id23>"

                        try
                        {
                            strDevIDTag = strDevIDTag.Substring(3); // "23>"
                            int idx2 = strDevIDTag.IndexOf('>'); // 2
                            strDevIDTag = strDevIDTag.Substring(0, idx2); // "23"
                            currMessageDevID = Convert.ToInt32(strDevIDTag);
                        }
                        catch (Exception)
                        {
                            currMessageDevID = 0;
                        }

                        bcstMessage = bcstMessage.Substring(idx + 1);
                    }
                    else
                    {
                        currMessageDevID = 0;
                    }

                    if ((bcstMessage.Length >= 6) && (bcstMessage.Substring(0, 6) == "<repl>"))
                    {
                        bcstMessage = bcstMessage.Substring(6, bcstMessage.Length - 6);
                        if (currMessageDevID > 0)
                        {
                            Note("devID:" + currMessageDevID + "   |   " + bcstMessage);
                        }
                        else
                        {
                            Note(bcstMessage);
                        }
                        
                        udpMessage = bcstMessage;
                    }
                    else if ((bcstMessage.Length >= 5) && (bcstMessage.Substring(0, 5) == "<err>"))
                    {
                        bcstMessage = bcstMessage.Substring(5, bcstMessage.Length - 5);
                        if (currMessageDevID > 0)
                        {
                            Note("devID:" + currMessageDevID + "   |   " + "ERROR: " + bcstMessage);
                        }
                        else
                        {
                            Note("ERROR: " + bcstMessage);
                        }
                        
                        udpMessage = bcstMessage;
                    }
                    else if (((bcstMessage.Length >= 6) && (bcstMessage.Substring(0, 5) == "timer:")))
                    {
                        if (currMessageDevID > 0)
                        {
                            Note("devID:" + currMessageDevID + "   |   " + bcstMessage, tbBcstListeningLog);
                        }
                        else
                        {
                            Note(bcstMessage, tbBcstListeningLog);
                        }
                        udpMessage = bcstMessage;
                    }
                    else if (bcstMessage == "imarduino")
                    {
                        if (needsToDiscoverArduinoBoard)
                        {
                            SocketAddress tmpSktAddress = remoteSktAddr;
                            string addrStr = tmpSktAddress.ToString(); //InterNetwork:16:{21,179,192,168,192,221,0,0,0,0,0,0,0,0}
                            addrStr = addrStr.Substring(addrStr.IndexOf("{") + 1);
                            addrStr = addrStr.Substring(0, addrStr.Length - 1);
                            char[] splitChar = { ',' };
                            string[] sktAddrStrArray = addrStr.Split(splitChar);
                            addrStr = sktAddrStrArray[2] + "." + sktAddrStrArray[3] + "." + sktAddrStrArray[4] + "." + sktAddrStrArray[5];
                            SetTextTB(textBox2, addrStr, false);
                            needsToDiscoverArduinoBoard = false;
                        }
                    }
                    else
                    {
                        if (!needsToDiscoverArduinoBoard)
                        {
                            if (currMessageDevID > 0)
                            {
                                Note("devID:" + currMessageDevID + "   |   " + bcstMessage, tbBcstListeningLog);
                            }
                            else
                            {
                                Note(bcstMessage, tbBcstListeningLog);
                            }
                            
                        }
                        //recievedData = true;
                        udpMessage = bcstMessage;
                    }

                }
                else
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
            }
        }

    }






    public static class PropertyHelper
    {
        /// <summary>
        /// Returns a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivatePropertyValue<T>(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            PropertyInfo pi = obj.GetType().GetProperty(propName,
                                                        BindingFlags.Public | BindingFlags.NonPublic |
                                                        BindingFlags.Instance);
            if (pi == null)
                throw new ArgumentOutOfRangeException("propName",
                                                      string.Format("Property {0} was not found in Type {1}", propName,
                                                                    obj.GetType().FullName));
            return (T)pi.GetValue(obj, null);
        }

        /// <summary>
        /// Returns a private Field Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Field</typeparam>
        /// <param name="obj">Object from where the Field Value is returned</param>
        /// <param name="propName">Field Name as string.</param>
        /// <returns>FieldValue</returns>
        public static T GetPrivateFieldValue<T>(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null)
                throw new ArgumentOutOfRangeException("propName",
                                                      string.Format("Field {0} was not found in Type {1}", propName,
                                                                    obj.GetType().FullName));
            return (T)fi.GetValue(obj);
        }

        /// <summary>
        /// Sets a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is set</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <param name="val">Value to set.</param>
        /// <returns>PropertyValue</returns>
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            Type t = obj.GetType();
            if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException("propName",
                                                      string.Format("Property {0} was not found in Type {1}", propName,
                                                                    obj.GetType().FullName));
            t.InvokeMember(propName,
                           BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty |
                           BindingFlags.Instance, null, obj, new object[] { val });
        }


        /// <summary>
        /// Set a private Field Value on a given Object. Uses Reflection.
        /// </summary>
        /// <typeparam name="T">Type of the Field</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Field name as string.</param>
        /// <param name="val">the value to set</param>
        /// <exception cref="ArgumentOutOfRangeException">if the Property is not found</exception>
        public static void SetPrivateFieldValue<T>(this object obj, string propName, T val)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null)
                throw new ArgumentOutOfRangeException("propName",
                                                      string.Format("Field {0} was not found in Type {1}", propName,
                                                                    obj.GetType().FullName));
            fi.SetValue(obj, val);
        }
    }
}
