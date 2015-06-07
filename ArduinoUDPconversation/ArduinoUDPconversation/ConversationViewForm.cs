using System;
using System.Collections.Concurrent;
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
using SkyImagesAnalyzerLibraries;


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
        //delegate void SetTextCallback(Label textbox, string text, bool AppendMode);
        //delegate void SetTextTBCallback(TextBox textbox, string text, bool AppendMode);
        //delegate void UpdateProgressBarCallback(ProgressBar ProgressBarControl, int PBvalue);
        //delegate void UpdatePictureBoxCallback(PictureBox PictureBoxControl, Image Image2Show);
        //delegate void SetButtonEnabledStatusCallback(Button Control, bool ControlEnabled);
        //delegate void SetTrackBarEnabledStatusCallback(TrackBar Control, bool ControlEnabled);
        //delegate void MoveTrackBarCallback(TrackBar TrackBarControl, int TBValue);
        //delegate void ToggleButtonStateCallback(Button ButtonControl, bool ControlEnabled, string ButtonText, bool ButtonFontBold);
        //delegate void ShowPictureCallback(String FilePath, int timeout = 0);

        //private DataSocketReader reader;
        //private UdpClient UDPreader;
        //public VisualizingForm VForm;
        private string ip2ListenDevID1 = "";
        private string ip2ListenDevID2 = "";
        private int port2converse = 5555;
        private int portBcstRecvng = 4444;
        private string currCommand;
        private static bool bcstMessageReceived = false;
        //private static String udpMessage = "";
        private static bool needsToDiscoverArduinoBoardID1 = false;
        private static bool needsToDiscoverArduinoBoardID2 = false;
        private static DateTime JanFirst1970 = new DateTime(1970, 1, 1);
        private static bool recievedData = false;
        IntPtr m_ip = IntPtr.Zero;
        private static accelerometerData accData;
        private static bool accDatahasBeenChanged = false;
        private static gyroscopeData gyroData;
        private static bool gyroDatahasBeenChanged = false;


        //private static Queue<string> quArduinoUDPCatchedMessages = new Queue<string>();
        private static ConcurrentQueue<IncomingUDPmessageBoundle> cquArduinoUDPCatchedMessages = new ConcurrentQueue<IncomingUDPmessageBoundle>();

        private static ConcurrentBag<IncomingUDPmessagesBoundlesTuple> cbArduinoMessagesTuples =
            new ConcurrentBag<IncomingUDPmessagesBoundlesTuple>();
        private static SocketAddress remoteSktAddr;


        public ArduinoConversationForm()
        {
            InitializeComponent();
        }





        public void Note(string text, TextBox tb2update)
        {
            ThreadSafeOperations.SetTextTB(tb2update, text + Environment.NewLine, true);
            if ((tb2update == tbBcstListeningLog) && (tb2update.Lines.Length > Settings.Default.BroadcastLogHistorySizeLines))
            {
                swapBcstLog();
                Note(text, tb2update);
            }


            if ((tb2update == tbResponseLog1) && (tb2update.Lines.Length > Settings.Default.BroadcastLogHistorySizeLines))
            {
                swapResponseLog(btnSwapResponseLog1);
                Note(text, tb2update);
            }
        }


        public void Note(string text)
        {
            ThreadSafeOperations.SetTextTB(tbResponseLog1, text + Environment.NewLine, true);
        }


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
        //
        //    outDataString += Environment.NewLine + curDate.ToString() + Environment.NewLine;
        //
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
        //
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
        //
        //                SetText(GPSDateLabel, sensorData.date.ToString(), false);
        //                SetText(GPSLatitudeLabel, sensorData.latitude.ToString(), false);
        //                SetText(GPSLongitudeLabel, sensorData.longitude.ToString(), false);
        //                SetText(GPSAltitudeLabel, sensorData.altitude.ToString(), false);
        //                SetText(GPSHorizAccLabel, sensorData.horizAccuracy.ToString(), false);
        //                SetText(GPSVertAccLabel, sensorData.vertAccuracy.ToString(), false);
        //
        //
        //                break;
        //            }
        //        case "compass":
        //            {
        //                CompassData sensorData = new CompassData();
        //                sensorData.date = curDate;
        //                sensorData.MagneticHeading = Convert.ToDouble(dataArray[2].Replace(".", ","));
        //                sensorData.trueHeading = Convert.ToDouble(dataArray[3].Replace(".", ","));
        //
        //                SetText(CompassDateLabel, sensorData.date.ToString(), false);
        //                SetText(CompassMagheadLabel, sensorData.MagneticHeading.ToString(), false);
        //                SetText(CompassTrueheadLabel, sensorData.trueHeading.ToString(), false);
        //
        //                break;
        //            }
        //        case "accel":
        //            {
        //                AccelerometerData sensorData = new AccelerometerData();
        //                sensorData.date = curDate;
        //                sensorData.AccelerationX = Convert.ToDouble(dataArray[2].Replace(".", ","));
        //                sensorData.AccelerationY = Convert.ToDouble(dataArray[3].Replace(".", ","));
        //                sensorData.AccelerationZ = Convert.ToDouble(dataArray[4].Replace(".", ","));
        //
        //                SetText(AccelDateLabel, sensorData.date.ToString(), false);
        //                SetText(AccelXLabel, sensorData.AccelerationX.ToString(), false);
        //                SetText(AccelYLabel, sensorData.AccelerationY.ToString(), false);
        //                SetText(AccelZLabel, sensorData.AccelerationZ.ToString(), false);
        //
        //
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
        //
        //                PointF p1 = new PointF();
        //                p1.X = center.X - (float)(0.5 * length * Math.Cos(Math.Atan(tg)));
        //                p1.Y = center.Y - (float)(0.5 * length * Math.Sin(Math.Atan(tg)));
        //                PointF p2 = new PointF();
        //                p2.X = center.X + (float)(0.5 * length * Math.Cos(Math.Atan(tg)));
        //                p2.Y = center.Y + (float)(0.5 * length * Math.Sin(Math.Atan(tg)));
        //
        //                VForm.DrawLine(p1, p2, Color.Black, (float)2.0);
        //
        //                break;
        //            }
        //        case "gyro":
        //            {
        //                GyroscopeData sensorData = new GyroscopeData();
        //                sensorData.date = curDate;
        //                sensorData.GyroscopeX = Convert.ToDouble(dataArray[2].Replace(".", ","));
        //                sensorData.GyroscopeY = Convert.ToDouble(dataArray[3].Replace(".", ","));
        //                sensorData.GyroscopeZ = Convert.ToDouble(dataArray[4].Replace(".", ","));
        //
        //                SetText(AccelDateLabel, sensorData.date.ToString(), false);
        //                SetText(GyroXLabel, sensorData.GyroscopeX.ToString(), false);
        //                SetText(GyroYLabel, sensorData.GyroscopeY.ToString(), false);
        //                SetText(GyroZLabel, sensorData.GyroscopeZ.ToString(), false);
        //
        //                //SetTextTB(textBox1, curDate + Environment.NewLine + inDataString + Environment.NewLine, true);
        //                //return inDataString;
        //
        //                break;
        //            }
        //        default:
        //            {
        //                SetTextTB(textBox1, curDate + Environment.NewLine + inDataString + Environment.NewLine, true);
        //                return inDataString;
        //                //break;
        //            }
        //    }
        //
        //
        //    return outDataString;
        //}


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ip2ListenDevID1 = tbDev1IPstr.Text;
        }



        private void btnFindArduino_Click(object sender, EventArgs e)
        {
            if (arduinoBoardSearchingWorker.IsBusy)
            {
                arduinoBoardSearchingWorker.CancelAsync();
            }
            else
            {
                if (sender == btnFindArduino1)
                {
                    needsToDiscoverArduinoBoardID1 = true;
                }
                else if (sender == btnFindArduino2)
                {
                    needsToDiscoverArduinoBoardID2 = true;
                }

                ThreadSafeOperations.ToggleButtonState(btnFindArduino1, true, "CANCEL", true);
                ThreadSafeOperations.ToggleButtonState(btnFindArduino2, true, "CANCEL", true);

                arduinoBoardSearchingWorker.RunWorkerAsync();
            }
        }



        private void arduinoBoardSearchingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker SelfWorker = sender as BackgroundWorker;

            bool needsToSwitchCatchingOff = false;
            if (!udpCatchingJob.IsBusy)
            {
                //start catching and the switch it off
                btnStartStopBdcstListening_Click(null, null);
                needsToSwitchCatchingOff = true;
            }


            while (needsToDiscoverArduinoBoardID1 || needsToDiscoverArduinoBoardID2)
            {
                if (SelfWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                Application.DoEvents();
                Thread.Sleep(0);
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
        //
        //    ping = new Ping();
        //    PingReply repl = ping.Send(testingIPaddr, 3, new byte[] { 1 }, new PingOptions(1, true));
        //
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
        //
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
        //
        //    SetTextTB(textBox1, "tested ip " + testingIPaddr.ToString() + ": " + returnData.ToString() + Environment.NewLine, true);
        //    Skt.Close();
        //    if (returnData.ToString() == "YES") retVal = true; else retVal = false;
        //
        //    return retVal;
        //}



        //private void arduinoBoardSearchingWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    //SetTextTB(textBox1, "Данные датчиков собраны " + e.ProgressPercentage + " раз" + Environment.NewLine, true);
        //    UpdateProgressBar(progressBar1, e.ProgressPercentage);
        //}



        private void Form1_Load(object sender, EventArgs e)
        {
            string generalSettingsFilename = Directory.GetCurrentDirectory() + "\\settings\\ArduinoUDPconversationAppGeneralSettings2G.xml";
            if (!File.Exists(generalSettingsFilename))
            {
                ip2ListenDevID1 = Settings.Default.ArduinoBoardDefaultIP;
                port2converse = Settings.Default.ArduinoBoardDefaultUDPport;
                ThreadSafeOperations.SetTextTB(tbDev1IPstr, ip2ListenDevID1, false);
            }
            else
            {
                Dictionary<string, object> generalSettings = ServiceTools.ReadDictionaryFromXML(generalSettingsFilename);

                ip2ListenDevID1 = generalSettings["ArduinoBoardID1DefaultIP"] as string;
                ip2ListenDevID2 = generalSettings["ArduinoBoardID2DefaultIP"] as string;
                port2converse = Convert.ToInt32(generalSettings["ArduinoBoardDefaultUDPport"]);
                portBcstRecvng = Convert.ToInt32(generalSettings["UDPBroadcastDefaultListeningPort"]);

                ThreadSafeOperations.SetTextTB(tbDev1IPstr, ip2ListenDevID1, false);
                ThreadSafeOperations.SetTextTB(tbDev2IPstr, ip2ListenDevID2, false);
            }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ip2ListenDevID1 != Settings.Default.ArduinoBoardDefaultIP)
            {
                Settings.Default.ArduinoBoardDefaultIP = ip2ListenDevID1;
                Settings.Default.Save();
            }

            if (port2converse != Settings.Default.ArduinoBoardDefaultUDPport)
            {
                Settings.Default.ArduinoBoardDefaultUDPport = port2converse;
                Settings.Default.Save();
            }


            //if (portBcstRecvng != Settings.Default.UDPBroadcastDefaultListeningPort)
            //{
            //    Settings.Default.UDPBroadcastDefaultListeningPort = portBcstRecvng;
            //    Settings.Default.Save();
            //}
        }



        //private void tbBcstListeningPort_TextChanged(object sender, EventArgs e)
        //{
        //    portBcstRecvng = Convert.ToInt32(tbBcstListeningPort.Text);
        //}



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
                ThreadSafeOperations.ToggleButtonState(btnStartStopBdcstListening, true, "Stop listening", true);
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
            ThreadSafeOperations.ToggleButtonState(btnStartStopBdcstListening, true, "Start listening", false);
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
                        cquArduinoUDPCatchedMessages.Enqueue(new IncomingUDPmessageBoundle(remoteSktAddr, bcstMessage));

                        //bcstMessageReceived = true;
                    }
                }
                catch (Exception exc)
                {
                    bcstMessageReceived = false;
                    recievingUDPmessage = false;
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




        private void PerformSendCommand(int devID = 1)
        {
            string retStr = currCommand;
            byte[] ret = Encoding.ASCII.GetBytes(retStr);
            Socket Skt = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Skt.EnableBroadcast = false;
            IPEndPoint ipEP = new IPEndPoint(IPAddress.Parse(ip2ListenDevID1), port2converse);

            if (devID == 1)
            {
                ipEP = new IPEndPoint(IPAddress.Parse(ip2ListenDevID1), port2converse);
            }
            else if (devID == 2)
            {
                ipEP = new IPEndPoint(IPAddress.Parse(ip2ListenDevID2), port2converse);
            }

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
            currCommand = textBoxCommand1.Text.Trim().Replace("\0", string.Empty);
            if (currCommand.Length == 0)
            {
                return;
            }
            ThreadSafeOperations.SetTextTB(textBoxCommand1, "", false);
            ThreadSafeOperations.SetTextTB(tbResponseLog1, ">>> " + currCommand + Environment.NewLine, true);
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
            ThreadSafeOperations.SetTextTB(tbBcstListeningLog, "", false);
        }


        private void swapResponseLog(object sender)
        {
            string filename1;
            string strtowrite;
            byte[] info2write;
            int curDevID = 1;

            if (sender == btnSwapResponseLog1)
            {
                curDevID = 1;
            }
            else if (sender == btnSwapResponseLog2)
            {
                curDevID = 2;
            }

            filename1 = Directory.GetCurrentDirectory() + "\\ResponseLog-" + "devID" + curDevID + "-" +
                        DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour +
                        "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".log";
            ServiceTools.logToTextFile(filename1, tbResponseLog1.Text, true);
            ThreadSafeOperations.SetTextTB(tbResponseLog1, "", false);
        }


        private void arduinoBoardSearchingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.ToggleButtonState(btnFindArduino1, true, "search for board (ID=1)", false);
            ThreadSafeOperations.ToggleButtonState(btnFindArduino2, true, "search for board (ID=2)", false);
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            swapResponseLog(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbResponseLog1.Text = "";
        }








        private void bgwUDPmessagesParser_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;

            while (true)
            {
                if (selfWorker.CancellationPending && cquArduinoUDPCatchedMessages.Count == 0)
                {
                    break;
                }

                if (cquArduinoUDPCatchedMessages.Count == 0)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                    continue;
                }

                if (cquArduinoUDPCatchedMessages.Count > 0)
                {
                    IncomingUDPmessageBoundle curMessageBoundle = null; //cquArduinoUDPCatchedMessages.Dequeue();
                    while (true)
                    {
                        if (cquArduinoUDPCatchedMessages.TryDequeue(out curMessageBoundle))
                        {
                            break;
                        }
                        else
                        {
                            Application.DoEvents();
                            Thread.Sleep(0);
                            continue;
                        }
                    }

                    if (curMessageBoundle == null) continue;

                    string bcstMessage = curMessageBoundle.udpMessage;
                    int currMessageDevID = curMessageBoundle.devID;
                    bool whetherContinueProcessThisMessage = true;


                    if (curMessageBoundle.isPartOfMessage)
                    {
                        whetherContinueProcessThisMessage = false;


                        IncomingUDPmessagesBoundlesTuple tplSrch = null;
                        List<IncomingUDPmessagesBoundlesTuple> currTuples =
                            new List<IncomingUDPmessagesBoundlesTuple>(cbArduinoMessagesTuples);
                        tplSrch =
                            currTuples.Find(
                                tpl => ((tpl.devID == curMessageBoundle.devID) && (tpl.mID == curMessageBoundle.mID)));
                        
                        //try
                        //{
                        //    tplSrch = cbArduinoMessagesTuples.First(tpl => ((tpl.devID == curMessageBoundle.devID) && (tpl.mID == curMessageBoundle.mID)));
                        //}
                        //catch (Exception ex)
                        //{
                        //    if (ex.GetType() == typeof(InvalidOperationException))
                        //    {
                        //        tplSrch = null;
                        //    }
                        //}
                        
                        if (tplSrch == null)
                        {
                            tplSrch = new IncomingUDPmessagesBoundlesTuple(curMessageBoundle);
                            cbArduinoMessagesTuples.Add(tplSrch);
                            whetherContinueProcessThisMessage = false;
                        }
                        else
                        {
                            tplSrch = tplSrch + curMessageBoundle;
                            if (tplSrch.IsComplete)
                            {
                                whetherContinueProcessThisMessage = true;
                                curMessageBoundle = tplSrch.CompleteMessage;
                                while (!cbArduinoMessagesTuples.TryTake(out tplSrch))
                                {
                                    Application.DoEvents();
                                    Thread.Sleep(0);
                                }
                            }
                        }
                    }


                    if (!whetherContinueProcessThisMessage)
                    {
                        continue;
                    }





                    if (curMessageBoundle.isReplyMessage)
                    {
                        //bcstMessage = bcstMessage.Substring(6, bcstMessage.Length - 6);
                        if (currMessageDevID == 1)
                        {
                            Note("devID:" + currMessageDevID + "   |   " + curMessageBoundle.udpMessage, tbResponseLog1);
                        }
                        else if (currMessageDevID == 2)
                        {
                            Note("devID:" + currMessageDevID + "   |   " + curMessageBoundle.udpMessage, tbResponseLog2);
                        }
                        else
                        {
                            Note(curMessageBoundle.udpMessage);
                        }

                        //udpMessage = bcstMessage;
                    }
                    //else if (curMessageBoundle.isErrorMessage)
                    //{
                    //    if (currMessageDevID == 1)
                    //    {
                    //        Note("devID:" + currMessageDevID + "   |   " + "ERROR: " + bcstMessage, tbResponseLog1);
                    //    }
                    //    else if (currMessageDevID == 2)
                    //    {
                    //        Note("devID:" + currMessageDevID + "   |   " + "ERROR: " + bcstMessage, tbResponseLog2);
                    //    }
                    //    else
                    //    {
                    //        Note("ERROR: " + bcstMessage);
                    //    }
                    //}
                    else if (((curMessageBoundle.udpMessage.Length >= 6) && (curMessageBoundle.udpMessage.Substring(0, 5) == "timer:")))
                    {
                        if (currMessageDevID > 0)
                        {
                            Note("devID:" + currMessageDevID + "   |   " + curMessageBoundle.udpMessage, tbBcstListeningLog);
                        }
                        else
                        {
                            Note(curMessageBoundle.udpMessage, tbBcstListeningLog);
                        }
                    }
                    else if (curMessageBoundle.udpMessage == "imarduino")
                    {
                        if ((needsToDiscoverArduinoBoardID1) && (currMessageDevID == 1))
                        {
                            string addrStr = curMessageBoundle.ipAddrString;
                            ThreadSafeOperations.SetTextTB(tbDev1IPstr, addrStr, false);
                            needsToDiscoverArduinoBoardID1 = false;
                        }

                        if ((needsToDiscoverArduinoBoardID2) && (currMessageDevID == 2))
                        {
                            string addrStr = curMessageBoundle.ipAddrString;
                            ThreadSafeOperations.SetTextTB(tbDev2IPstr, addrStr, false);
                            needsToDiscoverArduinoBoardID2 = false;
                        }
                    }
                    else
                    {
                        if ((!needsToDiscoverArduinoBoardID1) && (!needsToDiscoverArduinoBoardID2))
                        {
                            if (currMessageDevID > 0)
                            {
                                Note("devID:" + currMessageDevID + "   |   " + curMessageBoundle.udpMessage, tbBcstListeningLog);
                            }
                            else
                            {
                                Note(curMessageBoundle.udpMessage, tbBcstListeningLog);
                            }

                        }
                    }
                }
                else
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
            }
        }

        private void btnClearRsponceLog2_Click(object sender, EventArgs e)
        {
            tbResponseLog2.Text = "";
        }

        private void btnSwapResponseLog2_Click(object sender, EventArgs e)
        {
            swapResponseLog(sender);
        }



        private void SendCommandDev2()
        {
            currCommand = textBoxCommand2.Text.Trim().Replace("\0", string.Empty);
            if (currCommand.Length == 0)
            {
                return;
            }
            ThreadSafeOperations.SetTextTB(textBoxCommand2, "", false);
            ThreadSafeOperations.SetTextTB(tbResponseLog2, ">>> " + currCommand + Environment.NewLine, true);
            PerformSendCommand(2);
        }



        private void textBoxCommand2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                SendCommandDev2();
            }
        }

        private void tbDev2IPstr_TextChanged(object sender, EventArgs e)
        {
            ip2ListenDevID2 = (sender as TextBox).Text;
        }

        private void ArduinoConversationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }

    }







}
