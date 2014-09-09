using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using MRG.Controls.UI;
using MathNet.Numerics.Statistics;
using SkyIndexAnalyzerLibraries;


namespace DataCollectorAutomator
{
    #region local enums

    enum DataCollectingStates
    {
        idle,
        checkingState,
        working
    }


    enum WorkersRequestingArduinoDataBroadcastState
    {
        dataCollector,
        accelCalibrator//,
        //magnCalibrator
    }


    public enum SensorsHistoryShowing
    {
        None,
        Accelerometer,
        //Magnetometer,
        Pressure,
        GPS
    }

    #endregion


    #region the form behaviour

    public partial class DataCollectorMainForm : Form
    {
        private string ip2ListenID1 = "";
        private string ip2ListenID2 = "";
        private int port2converse = 5555;
        private int portBcstRecvng = 4444;
        //private string currCommand;

        //private static String udpMessage = "";
        private static String replyMessage = "";
        private static bool needsToDiscoverArduinoBoard = false;
        private static SocketAddress remoteSktAddr;
        private static bool needsReplyOnRequest = false;
        private static String ArduinoRequestString = "";
        private static DataCollectingStates dataCollectingState = DataCollectingStates.idle;
        //private static DateTime JanFirst1970 = new DateTime(1970, 1, 1);
        IntPtr m_ip = IntPtr.Zero;

        private static Queue<accelerometerData> accDataQueue = new Queue<accelerometerData>();
        private static accelerometerData latestAccData = new accelerometerData();
        private static Queue<GyroData> gyroDataQueue = new Queue<GyroData>();
        private static GyroData latestGyroData = new GyroData();
        private static MagnetometerData magnData = new MagnetometerData();
        private static GPSdata gpsData = new GPSdata();
        private static int pressure = 0;

        private static bool needsToSwitchListeningArduinoOFF = false;
        //private static bool accDatahasBeenChanged = false;
        private static bool gyroDatahasBeenChanged = false;
        private static bool magnDatahasBeenChanged = false;
        private static bool gpsDataHasBeenChanged = false;
        private static bool pressureHasBeenChanged = false;

        private static bool itsTimeToGetCamShot = false, getCamShotImmediately = false;
        private static WorkersRequestingArduinoDataBroadcastState theWorkerRequestedArduinoDataBroadcastState = WorkersRequestingArduinoDataBroadcastState.dataCollector;
        private static accelerometerData accCalibrationData;
        private static MagnetometerData magnCalibrationData;
        private static double magnCalibratedAngleShift;
        public static Image currentShowingImage;

        //public static TextBox reportingTextBox;

        private string generalSettingsFilename = "";
        private string accCalibrationDataFilename = "";
        private string magnCalibrationDataFilename = "";
        private int BroadcastLogHistorySizeLines = 4096;
        private TimeSpan CamShotPeriod = new TimeSpan(0, 1, 0);
        private IPAddress VivotekCameraID1IPaddress;
        private IPAddress VivotekCameraID2IPaddress;
        private string VivotekCameraUserName1 = "root";
        private string VivotekCameraPassword1 = "vivotek";
        private string VivotekCameraUserName2 = "root";
        private string VivotekCameraPassword2 = "vivotek";

        private LogWindow theLogWindow = null;
        private string strTotalBcstLog = "";
        private bool showTotalBcstLog = false;

        private static Queue<IncomingUDPmessageBoundle> quArduinoUDPCatchedMessages = new Queue<IncomingUDPmessageBoundle>();

        //private int sensorsHistoryRepresentingScale = 86400;
        //private SensorsHistoryShowing currRepresentingHistory = SensorsHistoryShowing.None;

        private static int recievedUDPPacketsCounter = 0;
        //private static double recievingUDPpacketsSpeed = 0.0d;


        private void ParseBroadcastMessage(string bcstMessage, int devID = 1)
        {
            bool isGPSdata = (bcstMessage.Substring(0, 6) == "$GPGGA");
            string dataSign = "";
            string dataValuesString = "";
            if (isGPSdata)
            {
                dataSign = "gps";
                dataValuesString = bcstMessage;
            }
            else
            {
                dataSign = bcstMessage.Substring(1, 3);
                dataValuesString = bcstMessage.Substring(5);
            }

            if ((bcstMessage.StartsWith("<") && bcstMessage.Substring(4, 1) == ">") || isGPSdata)
            {
                switch (dataSign)
                {
                    case "gps":
                        {
                            gpsData = new GPSdata(dataValuesString, GPSdatasources.CloudCamArduinoGPS);
                            gpsDataHasBeenChanged = gpsData.validGPSdata;
                            break;
                        }
                    case "dta":
                        {
                            //accelerometer data
                            string[] splitters = { ";" };
                            string[] stringAccValues = dataValuesString.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
                            latestAccData = new accelerometerData(stringAccValues);

                            if (latestAccData.validAccData)
                            {
                                accDataQueue.Enqueue(latestAccData);
                            }

                            break;
                        }
                    case "dtg":
                        {
                            //gyroscope data
                            string[] splitters = { ";" };
                            string[] stringGyroValues = dataValuesString.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
                            latestGyroData = new GyroData(stringGyroValues);
                            gyroDataQueue.Enqueue(latestGyroData);
                            //gyroDatahasBeenChanged = true;
                            break;
                        }
                    case "dtp":
                        {
                            //pressure data
                            string[] splitters = { ";" };
                            string[] stringValues = dataValuesString.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);

                            if (stringValues.Count() > 1)
                            {
                                pressureHasBeenChanged = false;
                                break;
                            }

                            try
                            {
                                pressure = Convert.ToInt32(stringValues[0]);
                                pressureHasBeenChanged = true;
                            }
                            catch (Exception)
                            {
                                pressureHasBeenChanged = false;
                                break;
                            }
                            break;
                        }
                    case "dtm":
                        {
                            string[] splitters = { ";" };
                            string[] stringMagnValues = dataValuesString.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
                            magnData = new MagnetometerData(stringMagnValues);
                            magnDatahasBeenChanged = true;
                            break;
                        }
                    default:
                        break;
                }

            }
        }


        public DataCollectorMainForm()
        {
            InitializeComponent();
        }

        private void DataCollectorMainForm_Load(object sender, EventArgs e)
        {
            generalSettingsFilename = Directory.GetCurrentDirectory() +
                                      "\\settings\\DataCollectorAppGeneralSettings2G.xml";
            accCalibrationDataFilename = Directory.GetCurrentDirectory() +
                                         "\\settings\\AccelerometerCalibrationData2G.xml";
            


            Dictionary<string, object> accCalibrationDataDict = ServiceTools.ReadDictionaryFromXML(accCalibrationDataFilename);
            Dictionary<string, object> magnCalibrationDataDict = ServiceTools.ReadDictionaryFromXML(magnCalibrationDataFilename);
            Dictionary<string, object> generalSettings = ServiceTools.ReadDictionaryFromXML(generalSettingsFilename);

            accCalibrationData = new accelerometerData(Convert.ToDouble(accCalibrationDataDict["accCalibratedXzero"]),
                Convert.ToDouble(accCalibrationDataDict["accCalibratedYzero"]),
                Convert.ToDouble(accCalibrationDataDict["accCalibratedZzero"]));


            ip2ListenID1 = generalSettings["ArduinoBoardDefaultIP1"] as string;
            ip2ListenID2 = generalSettings["ArduinoBoardDefaultIP2"] as string;

            port2converse = Convert.ToInt32(generalSettings["ArduinoBoardDefaultUDPport"]);
            portBcstRecvng = Convert.ToInt32(generalSettings["UDPBroadcastDefaultListeningPort"]);
            string strCamShotPeriod = (generalSettings["VivotekCameraShootingPeriod"]) as string;
            CamShotPeriod = new TimeSpan(Convert.ToInt32(strCamShotPeriod.Substring(0, 2)),
                Convert.ToInt32(strCamShotPeriod.Substring(3, 2)), Convert.ToInt32(strCamShotPeriod.Substring(6, 2)));

            IPAddress.TryParse(generalSettings["VivotekCameraID1IPaddr"] as string, out VivotekCameraID1IPaddress);
            IPAddress.TryParse(generalSettings["VivotekCameraID2IPaddr"] as string, out VivotekCameraID2IPaddress);
            VivotekCameraUserName1 = generalSettings["VivotekCameraID1UserName"] as string;
            VivotekCameraPassword1 = generalSettings["VivotekCameraID1Password"] as string;
            VivotekCameraUserName2 = generalSettings["VivotekCameraID2UserName"] as string;
            VivotekCameraPassword2 = generalSettings["VivotekCameraID2Password"] as string;
            BroadcastLogHistorySizeLines = Convert.ToInt32(generalSettings["BroadcastLogHistorySizeLines"]);


            ThreadSafeOperations.SetTextTB(tbCamPWD1, VivotekCameraPassword1, false);
            ThreadSafeOperations.SetTextTB(tbCamPWD2, VivotekCameraPassword2, false);
            tbCamIP1.Text = generalSettings["VivotekCameraID1IPaddr"] as string;
            tbCamIP2.Text = generalSettings["VivotekCameraID2IPaddr"] as string;
            ThreadSafeOperations.SetTextTB(tbCamUName1, VivotekCameraUserName1, false);
            ThreadSafeOperations.SetTextTB(tbCamUName2, VivotekCameraUserName2, false);
            ThreadSafeOperations.SetTextTB(tbIP2ListenDevID1, ip2ListenID1, false);
            ThreadSafeOperations.SetTextTB(tbIP2ListenDevID2, ip2ListenID2, false);

            tbCamShotPeriod.Text = CamShotPeriod.ToString("c");
            
            //ThreadSafeOperations.SetTextTB(tbBcstListeningPort, portBcstRecvng.ToString(), false);




            if (accCalibrationData.AccMagnitude == 0.0d)
            {
                accCalibrationData = new accelerometerData(0.0, 0.0, -256.0);
            }
            ThreadSafeOperations.SetText(lblAccelCalibrationX, Math.Round(accCalibrationData.AccDoubleX, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationY, Math.Round(accCalibrationData.AccDoubleY, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationZ, Math.Round(accCalibrationData.AccDoubleZ, 2).ToString(), false);

            processCircleID1.OuterCircleRadius = 20;
            processCircleID1.InnerCircleRadius = 15;
            processCircleID1.NumberSpoke = 12;
            processCircleID1.SpokeThickness = 4;

            processCircleID2.OuterCircleRadius = 20;
            processCircleID2.InnerCircleRadius = 15;
            processCircleID2.NumberSpoke = 12;
            processCircleID2.SpokeThickness = 4;


            //reportingTextBox = tbMainLog;
        }




        public void NoteLog(string text)
        {
            if (showTotalBcstLog)
            {
                Note(text);
                //theLogWindow = ServiceTools.LogAText(theLogWindow, text, true);
                //if (theLogWindow.LinesCount >= BroadcastLogHistorySizeLines)
                //{
                //    swapBcstLog();
                //}
            }
            strTotalBcstLog += text + Environment.NewLine;
            if (strTotalBcstLog.Length >= (BroadcastLogHistorySizeLines * 200))
            {
                swapBcstLog();
            }
        }


        public void Note(string text)
        {
            ThreadSafeOperations.SetTextTB(tbMainLog, text + Environment.NewLine, true);
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ip2ListenID1 = tbIP2ListenDevID1.Text;
        }

        private void btnFindArduino_Click(object sender, EventArgs e)
        {
            if (arduinoBoardSearchingWorker.IsBusy)
            {
                arduinoBoardSearchingWorker.CancelAsync();
                SearchingArduinoID1ProcessCircle.Active = false;
                SearchingArduinoID1ProcessCircle.Visible = false;
                //UpdateProgressBar(progressBar1, 0);
            }
            else
            {
                SearchingArduinoID1ProcessCircle.Visible = true;
                SearchingArduinoID1ProcessCircle.Active = true;
                arduinoBoardSearchingWorker.RunWorkerAsync();
            }
        }

        private void arduinoBoardSearchingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
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



        private void SaveSettings()
        {
            Dictionary<string, object> dictDataToSave = new Dictionary<string, object>();
            dictDataToSave.Add("ArduinoBoardID1DefaultIP", ip2ListenID1);
            dictDataToSave.Add("ArduinoBoardID2DefaultIP", ip2ListenID2);
            dictDataToSave.Add("ArduinoBoardDefaultUDPport", port2converse);
            dictDataToSave.Add("UDPBroadcastDefaultListeningPort", portBcstRecvng);
            dictDataToSave.Add("VivotekCameraShootingPeriod", CamShotPeriod.ToString("c"));
            dictDataToSave.Add("VivotekCameraID1IPaddr", VivotekCameraID1IPaddress.ToString());
            dictDataToSave.Add("VivotekCameraID2IPaddr", VivotekCameraID2IPaddress.ToString());
            dictDataToSave.Add("VivotekCameraID1UserName", VivotekCameraUserName1);
            dictDataToSave.Add("VivotekCameraID2UserName", VivotekCameraUserName2);
            dictDataToSave.Add("VivotekCameraID1Password", VivotekCameraPassword1);
            dictDataToSave.Add("VivotekCameraID2Password", VivotekCameraPassword2);
            dictDataToSave.Add("BroadcastLogHistorySizeLines", BroadcastLogHistorySizeLines);

            ServiceTools.WriteDictionaryToXml(dictDataToSave, generalSettingsFilename, false);
        }




        private void DataCollectorMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
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

        private void udpCatchingJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.ToggleButtonState(btnStartStopBdcstListening, true, "Start listening", false);
            bgwUDPmessagesParser.CancelAsync();
        }





        private static bool recievingUDPmessage = false;
        //private static bool bcstMessageReceived = false;
        public void bcstReceiveCallback(IAsyncResult ar)
        {
            string bcstMessage = "";
            UdpState udpSt = (UdpState)(ar.AsyncState);
            UdpClient udpClt = (UdpClient)(udpSt.UDPclient);
            IPEndPoint ipEP = (IPEndPoint)(udpSt.ipEndPoint);
            
            remoteSktAddr = PropertyHelper.GetPrivatePropertyValue<SocketAddress>((object)ar, "SocketAddress");
            //udpSt.sktAddress = remoteSktAddr;


            if (udpClt.Client != null)
            {
                try
                {
                    Byte[] receiveBytes = udpClt.EndReceive(ar, ref ipEP);
                    recievingUDPmessage = false;

                    bcstMessage = Encoding.ASCII.GetString(receiveBytes);

                    if (bcstMessage != "")
                    {
                        quArduinoUDPCatchedMessages.Enqueue(new IncomingUDPmessageBoundle(remoteSktAddr, bcstMessage));

                        recievedUDPPacketsCounter++;

                        //bcstMessageReceived = true;
                    }

                }
                catch (Exception exc)
                {
                    //bcstMessageReceived = false;
                    //bcstMessage = exc.Message;
                }
                recievingUDPmessage = false;
            }
            else
            {
                //bcstMessageReceived = false;
                recievingUDPmessage = false;
            }
        }





        private void PerformSendCommand(string currCommand, int devID)
        {
            string retStr = currCommand;
            byte[] ret = System.Text.Encoding.ASCII.GetBytes(retStr);
            Socket Skt = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Skt.EnableBroadcast = false;
            IPEndPoint ipEP = new IPEndPoint(IPAddress.Parse(ip2ListenID1), port2converse);
            if (devID == 1)
            {
                ipEP = new IPEndPoint(IPAddress.Parse(ip2ListenID1), port2converse);
            }
            else if (devID == 2)
            {
                ipEP = new IPEndPoint(IPAddress.Parse(ip2ListenID2), port2converse);
            }

            int sent = Skt.SendTo(ret, ipEP);
            Skt.Close();
        }




        private void udpCatchingJob_DoWork(object sender, DoWorkEventArgs e)
        {
            UdpClient bcstUDPreader = new UdpClient(portBcstRecvng, AddressFamily.InterNetwork);
            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpState s = new UdpState();
            s.ipEndPoint = RemoteIpEndPoint;
            s.UDPclient = bcstUDPreader;
            //IPAddress bcstIP2listen = IPAddress.Parse(ip2Listen);

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


                //bcstMessageReceived = false;
                recievingUDPmessage = true;
                bcstUDPreader.BeginReceive(bcstReceiveCallback, s);
            }

            bcstUDPreader.Close();
        }




        private void btnSwapBcstLog_Click(object sender, EventArgs e)
        {
            swapBcstLog();
        }





        private void swapBcstLog()
        {
            string filename1;

            filename1 = Directory.GetCurrentDirectory() + "\\logs\\BcstLog-" + DateTime.UtcNow.ToString("o").Replace(":", "-") + ".log";

            strTotalBcstLog = "";
        }




        private void arduinoBoardSearchingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.ToggleButtonState(btnFindArduino1, true, "Search for outdoor board", false);
            SearchingArduinoID1ProcessCircle.Active = false;
            SearchingArduinoID1ProcessCircle.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (currentShowingImage == null)
            {
                return;
            }
            Bitmap bm2show = new Bitmap(currentShowingImage);
            SimpleShowImageForm imageForm = new SimpleShowImageForm(bm2show);
            imageForm.Show(this);
        }


        private void PerformRequestArduinoBoard(string requestText, int devID)
        {
            ArduinoRequestString = requestText;
            needsReplyOnRequest = true;
            PerformSendCommand(requestText, devID);
            ArduinoRequestExpectant.RunWorkerAsync();
        }


        private void btnStartStopCollecting_Click(object sender, EventArgs e)
        {
            if (!dataCollector.IsBusy)
            {
                if (!udpCatchingJob.IsBusy)
                {
                    //включим прослушку Arduino
                    needsToSwitchListeningArduinoOFF = true;
                    btnStartStopBdcstListening_Click(null, null);
                }
                Note("Detecting outdoor board broadcasting state");
                ThreadSafeOperations.ToggleButtonState(btnStartStopCollecting, false, "wait for state checking", true);
                StartStopDataCollectingWaitingCircle.Visible = true;
                StartStopDataCollectingWaitingCircle.Active = true;
                dataCollectingState = DataCollectingStates.checkingState;
                theWorkerRequestedArduinoDataBroadcastState = WorkersRequestingArduinoDataBroadcastState.dataCollector;
                PerformRequestArduinoBoard("1", 1);
            }
            else
            {
                dataCollector.CancelAsync();
            }
        }


        private void ArduinoRequestExpectant_DoWork(object sender, DoWorkEventArgs e)
        {
            while (needsReplyOnRequest)
            {
                System.Threading.Thread.Sleep(50);
            }
        }

        private void ArduinoRequestExpectant_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartStopDataCollectingWaitingCircle.Visible = false;
            StartStopDataCollectingWaitingCircle.Active = false;
            if (ArduinoRequestString == "1")
            {
                //найдем строку, в которой сказано про data broadcasting
                if ((replyMessage == "data broadcasting is OFF") && (dataCollectingState == DataCollectingStates.checkingState))
                {
                    Note("Outdoor facility data broadcasting is OFF. Turning it ON.");
                    StartStopDataCollectingWaitingCircle.Visible = true;
                    StartStopDataCollectingWaitingCircle.Active = true;
                    PerformRequestArduinoBoard("2");
                }
                else if ((replyMessage == "data broadcasting is ON") && (dataCollectingState == DataCollectingStates.checkingState))
                {
                    if (theWorkerRequestedArduinoDataBroadcastState == WorkersRequestingArduinoDataBroadcastState.dataCollector)
                    {
                        dataCollector.RunWorkerAsync();
                    }
                    else if (theWorkerRequestedArduinoDataBroadcastState == WorkersRequestingArduinoDataBroadcastState.accelCalibrator)
                    {
                        accelCalibrator.RunWorkerAsync();
                    }
                    //else if (theWorkerRequestedArduinoDataBroadcastState == WorkersRequestingArduinoDataBroadcastState.magnCalibrator)
                    //{
                    //    magnCalibrator.RunWorkerAsync();
                    //}
                }

            }
            else if (ArduinoRequestString == "2")
            {
                Note(replyMessage);
                if ((replyMessage == "data broadcasting is ON") && (dataCollectingState == DataCollectingStates.checkingState))
                {
                    if (theWorkerRequestedArduinoDataBroadcastState == WorkersRequestingArduinoDataBroadcastState.dataCollector)
                    {
                        dataCollector.RunWorkerAsync();
                    }
                    else if (theWorkerRequestedArduinoDataBroadcastState == WorkersRequestingArduinoDataBroadcastState.accelCalibrator)
                    {
                        accelCalibrator.RunWorkerAsync();
                    }
                    //else if (theWorkerRequestedArduinoDataBroadcastState == WorkersRequestingArduinoDataBroadcastState.magnCalibrator)
                    //{
                    //    magnCalibrator.RunWorkerAsync();
                    //}
                }
            }
        }



        //private void updateAccVisualization(accelerometerData accData)
        //{
        //    int accX2Show = Convert.ToInt32(Math.Round(100.0 * (accData.AccDoubleX / accData.accMagnitude), 0));
        //    int accY2Show = Convert.ToInt32(Math.Round(100.0 * (accData.AccDoubleY / accData.accMagnitude), 0));
        //    int accZ2Show = Convert.ToInt32(Math.Round(100.0 * (accData.AccDoubleZ / accData.accMagnitude), 0));
        //}



        private void dataCollector_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime datetimeCamshotTimerBegin = DateTime.Now;
            DateTime datetimeCamshotTimerNow = DateTime.Now;
            DateTime datetimeCamshotHasBeenTaken = DateTime.Now;
            TimeSpan camshotTimer = datetimeCamshotTimerNow - datetimeCamshotTimerBegin;
            TimeSpan camshotPeriod = CamShotPeriod;
            TimeSpan sinceCamShotHasBeenTaken = DateTime.Now - datetimeCamshotHasBeenTaken;
            TimeSpan labelsUpdatingPeriod = new TimeSpan(0, 0, 1);
            DateTime datetimePreviousLabelsUpdate = DateTime.MinValue;
            //double horizAcc, totalAcc, percHorizAcc;
            //double percAccDeviation = 100.0;
            double accDevAngle = 0.0d;
            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            ThreadSafeOperations.ToggleButtonState(btnStartStopCollecting, true, "stop collecting data", true);
            dataCollectingState = DataCollectingStates.working;
            bool camshotTimeToGetIt = false;
            bool camshotInclinedProperly = false;
            bool camshotHasBeenTaken = false;

            DenseMatrix dmAccDataMatrix = null;
            List<long> accDateTimeValuesList = new List<long>();
            DenseMatrix dmGyroDataMatrix = null;
            List<long> gyroDateTimeValuesList = new List<long>();

            //accelerometerData accData = null;
            DenseMatrix dmMagnDataMatrix = null;
            List<long> magnDateTimeValuesList = new List<long>();
            DenseMatrix dmGPSDataMatrix = null;
            List<long> gpsDateTimeValuesList = new List<long>();
            List<int> pressureValuesList = new List<int>();
            List<long> pressureDateTimeValuesList = new List<long>();

            Stopwatch stwToEstimateUDPpacketsRecieving = new Stopwatch();
            stwToEstimateUDPpacketsRecieving.Start();


            while (true)
            {
                if (SelfWorker.CancellationPending)
                {
                    if (dmAccDataMatrix != null)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", accDateTimeValuesList.ToArray());
                        dataToSave.Add("AccelerometerData", dmAccDataMatrix);

                        NetCDFoperations.AddVariousDataToFile(dataToSave,
                            Directory.GetCurrentDirectory() + "\\logs\\AccelerometerDataLog-" +
                            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        dmAccDataMatrix = null;
                        accDateTimeValuesList.Clear();
                    }
                    if (dmGyroDataMatrix != null)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", gyroDateTimeValuesList.ToArray());
                        dataToSave.Add("GyroscopeData", dmGyroDataMatrix);

                        NetCDFoperations.AddVariousDataToFile(dataToSave,
                            Directory.GetCurrentDirectory() + "\\logs\\GyroscopeDataLog-" +
                            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        dmGyroDataMatrix = null;
                        gyroDateTimeValuesList.Clear();
                    }
                    if (dmMagnDataMatrix != null)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", magnDateTimeValuesList.ToArray());
                        dataToSave.Add("MagnetometerData", dmMagnDataMatrix);

                        NetCDFoperations.AddVariousDataToFile(dataToSave,
                            Directory.GetCurrentDirectory() + "\\logs\\MagnetometerDataLog-" +
                            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        dmMagnDataMatrix = null;
                        magnDateTimeValuesList.Clear();
                    }
                    if (dmGPSDataMatrix != null)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                        dataToSave.Add("GPSdata", dmGPSDataMatrix);

                        NetCDFoperations.AddVariousDataToFile(dataToSave,
                            Directory.GetCurrentDirectory() + "\\logs\\GPSDataLog-" +
                            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        dmGPSDataMatrix = null;
                        gpsDateTimeValuesList.Clear();
                    }
                    if (pressureValuesList.Count > 0)
                    {
                        ThreadSafeOperations.SetText(lblPressureValue, pressureValuesList[pressureValuesList.Count-1].ToString(), false);

                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();

                        long[] datetimeDataArray = pressureDateTimeValuesList.ToArray();
                        dataToSave.Add("DateTime", datetimeDataArray);

                        int[] pressureArray = pressureValuesList.ToArray();
                        dataToSave.Add("PressureData", pressureArray);

                        NetCDFoperations.AddVariousDataToFile(dataToSave, Directory.GetCurrentDirectory() +
                            "\\logs\\PressureDataLog-" + DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        pressureValuesList.Clear();
                        pressureDateTimeValuesList.Clear();
                    }

                    ThreadSafeOperations.SetLoadingCircleState(processCircleID1, false, false, processCircleID1.Color, 100);
                    ThreadSafeOperations.SetLoadingCircleState(processCircleID2, false, false, processCircleID2.Color, 100);

                    break;
                }


                //оценка скорости приема пакетов и анализ состояния приема данных
                if (stwToEstimateUDPpacketsRecieving.ElapsedMilliseconds >= 1000)
                {
                    //оценим скорость поступления пакетов
                    double speed = (double) recievedUDPPacketsCounter * 1000.0d/
                                      (double)stwToEstimateUDPpacketsRecieving.ElapsedMilliseconds;
                    recievedUDPPacketsCounter = 0;
                    stwToEstimateUDPpacketsRecieving.Restart();

                    if (speed > 0.0d)
                    {
                        ThreadSafeOperations.SetLoadingCircleState(processCircleID1, true, true, processCircleID1.Color, Convert.ToInt32(100.0d * speed / 50.0));
                        //Note("UDP messages speed: " + speed + Environment.NewLine);
                    }
                    else
                    {
                        ThreadSafeOperations.SetLoadingCircleState(processCircleID1, false, false, processCircleID1.Color, 100);
                    }
                }



                if (accDataQueue.Count > 0)
                {
                    //accDatahasBeenChanged = false;

                    accelerometerData accData = accDataQueue.Dequeue();

                    if (accData == null) continue;

                    accDevAngle = (accData * accCalibrationData) / (accData.AccMagnitude * accCalibrationData.AccMagnitude);
                    accDevAngle = Math.Acos(accDevAngle);


                    accDateTimeValuesList.Add(DateTime.UtcNow.Ticks);

                    if (dmAccDataMatrix == null)
                    {
                        dmAccDataMatrix = DenseMatrix.Create(1, 7, (r, c) =>
                        {
                            switch (c)
                            {
                                case 0:
                                    return accData.AccDoubleX;
                                    break;
                                case 1:
                                    return accData.AccDoubleY;
                                    break;
                                case 2:
                                    return accData.AccDoubleZ;
                                    break;
                                case 3:
                                    return accCalibrationData.AccDoubleX;
                                    break;
                                case 4:
                                    return accCalibrationData.AccDoubleY;
                                    break;
                                case 5:
                                    return accCalibrationData.AccDoubleZ;
                                    break;
                                case 6:
                                    {
                                        //угол отклонения от калибровочного вектора
                                        // В РАДИАНАХ
                                        return accDevAngle;
                                        break;
                                    }
                                default:
                                    break;
                            }
                            return 0;
                        });
                    }
                    else
                    {
                        DenseVector dvAccDataVectorToAdd = DenseVector.Create(7, c =>
                        {
                            switch (c)
                            {
                                case 0:
                                    return accData.AccDoubleX;
                                    break;
                                case 1:
                                    return accData.AccDoubleY;
                                    break;
                                case 2:
                                    return accData.AccDoubleZ;
                                    break;
                                case 3:
                                    return accCalibrationData.AccDoubleX;
                                    break;
                                case 4:
                                    return accCalibrationData.AccDoubleY;
                                    break;
                                case 5:
                                    return accCalibrationData.AccDoubleZ;
                                    break;
                                case 6:
                                    {
                                        //угол отклонения от калибровочного вектора
                                        // В РАДИАНАХ
                                        return accDevAngle;
                                        break;
                                    }
                                default:
                                    break;
                            }
                            return 0;
                        });
                        dmAccDataMatrix =
                            (DenseMatrix)dmAccDataMatrix.InsertRow(dmAccDataMatrix.RowCount, dvAccDataVectorToAdd);
                    }

                    //ThreadSafeOperations.SetText(lblGotAccDataPackCounterValue, accDateTimeValuesList.Count.ToString(), false);

                    if (accDateTimeValuesList.Count % 100 == 0)
                    {
                        DenseVector dvMagnDev = DenseVector.Create(dmAccDataMatrix.RowCount, i =>
                        {
                            accelerometerData accCurrentData = new accelerometerData(dmAccDataMatrix[0, 0],
                                dmAccDataMatrix[0, 1], dmAccDataMatrix[0, 2]);
                            return (accCurrentData.AccMagnitude - accCalibrationData.AccMagnitude) *
                                   (accCurrentData.AccMagnitude - accCalibrationData.AccMagnitude);
                        });
                        ThreadSafeOperations.SetText(lblAccDevMeanMagnValueID1,
                            Math.Sqrt(dvMagnDev.Sum() / dvMagnDev.Count).ToString("0.###e-00"), false);


                        DescriptiveStatistics stats1 = new DescriptiveStatistics(dmAccDataMatrix.Column(6));
                        ThreadSafeOperations.SetText(lblAccDevMeanAngleValueID1, stats1.Mean.ToString("0.###e-00"), false);
                    }

                    if (accDateTimeValuesList.Count >= 1000)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", accDateTimeValuesList.ToArray());
                        dataToSave.Add("AccelerometerData", dmAccDataMatrix);

                        NetCDFoperations.AddVariousDataToFile(dataToSave,
                            Directory.GetCurrentDirectory() + "\\logs\\AccelerometerDataLog-" +
                            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");




                        dmAccDataMatrix = null;
                        accDateTimeValuesList.Clear();
                    }
                }




                if (gyroDataQueue.Count > 0)
                {
                    //gyroDatahasBeenChanged = false;

                    GyroData gyroData = gyroDataQueue.Dequeue();
                    if (gyroData == null)
                        continue;

                    gyroDateTimeValuesList.Add(DateTime.UtcNow.Ticks);

                    if (dmGyroDataMatrix == null)
                    {
                        dmGyroDataMatrix = gyroData.ToOneRowDenseMatrix();
                    }
                    else
                    {
                        DenseVector dvGyroDataVectorToAdd = gyroData.ToDenseVector();

                        dmGyroDataMatrix =
                            (DenseMatrix)dmGyroDataMatrix.InsertRow(dmGyroDataMatrix.RowCount, dvGyroDataVectorToAdd);
                    }

                    if (gyroDateTimeValuesList.Count >= 100)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", gyroDateTimeValuesList.ToArray());
                        dataToSave.Add("GyroscopeData", dmGyroDataMatrix);

                        NetCDFoperations.AddVariousDataToFile(dataToSave,
                            Directory.GetCurrentDirectory() + "\\logs\\GyroscopeDataLog-" +
                            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");


                        dmGyroDataMatrix = null;
                        gyroDateTimeValuesList.Clear();
                    }
                }





                if (magnDatahasBeenChanged)
                {

                    double currCompassAngle = 0.0d;
                    if (latestAccData == null)
                    {
                        currCompassAngle = magnData.compassAngle(accCalibrationData) + magnCalibratedAngleShift;
                    }
                    else
                    {
                        currCompassAngle = magnData.compassAngle(latestAccData) + magnCalibratedAngleShift;
                    }

                    currCompassAngle = PointPolar.CropAngleDegrees(currCompassAngle);



                    magnDatahasBeenChanged = false;



                    magnDateTimeValuesList.Add(DateTime.UtcNow.Ticks);

                    if (dmMagnDataMatrix == null)
                    {
                        dmMagnDataMatrix = DenseMatrix.Create(1, 8, (r, c) =>
                        {
                            switch (c)
                            {
                                case 0:
                                    return magnData.MagnDoubleX;
                                    break;
                                case 1:
                                    return magnData.MagnDoubleY;
                                    break;
                                case 2:
                                    return magnData.MagnDoubleZ;
                                    break;
                                case 3:
                                    return magnCalibrationData.MagnDoubleX;
                                    break;
                                case 4:
                                    return magnCalibrationData.MagnDoubleY;
                                    break;
                                case 5:
                                    return magnCalibrationData.MagnDoubleZ;
                                    break;
                                case 6:
                                    return magnCalibratedAngleShift;
                                    break;
                                case 7:
                                    {
                                        return currCompassAngle;
                                        break;
                                    }
                                default:
                                    break;
                            }
                            return 0;
                        });
                    }
                    else
                    {
                        DenseVector dvMagnDataVectorToAdd = DenseVector.Create(8, c =>
                        {
                            switch (c)
                            {
                                case 0:
                                    return magnData.MagnDoubleX;
                                    break;
                                case 1:
                                    return magnData.MagnDoubleY;
                                    break;
                                case 2:
                                    return magnData.MagnDoubleZ;
                                    break;
                                case 3:
                                    return magnCalibrationData.MagnDoubleX;
                                    break;
                                case 4:
                                    return magnCalibrationData.MagnDoubleY;
                                    break;
                                case 5:
                                    return magnCalibrationData.MagnDoubleZ;
                                    break;
                                case 6:
                                    return magnCalibratedAngleShift;
                                    break;
                                case 7:
                                    {
                                        return currCompassAngle;
                                        break;
                                    }
                                default:
                                    break;
                            }
                            return 0;
                        });
                        dmMagnDataMatrix =
                            (DenseMatrix)dmMagnDataMatrix.InsertRow(dmMagnDataMatrix.RowCount, dvMagnDataVectorToAdd);
                    }


                    if (magnDateTimeValuesList.Count >= 10)
                    {
                        //ThreadSafeOperations.SetText(lblMagnDataX, magnData.MagnX.ToString(), false);
                        //ThreadSafeOperations.SetText(lblMagnDataY, magnData.MagnY.ToString(), false);
                        //ThreadSafeOperations.SetText(lblMagnDataZ, magnData.MagnZ.ToString(), false);
                        //ThreadSafeOperations.SetText(lblMagnDataHeading, currCompassAngle.ToString("F2"), false);

                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", magnDateTimeValuesList.ToArray());
                        dataToSave.Add("MagnetometerData", dmMagnDataMatrix);

                        NetCDFoperations.AddVariousDataToFile(dataToSave,
                            Directory.GetCurrentDirectory() + "\\logs\\MagnetometerDataLog-" +
                            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        dmMagnDataMatrix = null;
                        magnDateTimeValuesList.Clear();
                    }
                }




                if (gpsDataHasBeenChanged)
                {
                    ThreadSafeOperations.SetText(lblLatValue, gpsData.lat.ToString() + gpsData.latHemisphere, false);
                    ThreadSafeOperations.SetText(lblLonValue, gpsData.lon.ToString() + gpsData.lonHemisphere, false);
                    ThreadSafeOperations.SetText(lblUTCTimeValue, gpsData.dateTimeUTC.ToString("u").Replace(" ", Environment.NewLine).Replace("Z", ""), false);
                    gpsDataHasBeenChanged = false;

                    gpsDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                    if (dmGPSDataMatrix == null)
                    {
                        dmGPSDataMatrix = gpsData.ToOneRowDenseMatrix();
                    }
                    else
                    {
                        DenseVector dvGPSDataVectorToAdd = gpsData.ToDenseVector();

                        dmGPSDataMatrix =
                            (DenseMatrix)dmGPSDataMatrix.InsertRow(dmGPSDataMatrix.RowCount, dvGPSDataVectorToAdd);
                    }

                    if (gpsDateTimeValuesList.Count >= 50)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                        dataToSave.Add("GPSdata", dmGPSDataMatrix);

                        NetCDFoperations.AddVariousDataToFile(dataToSave,
                            Directory.GetCurrentDirectory() + "\\logs\\GPSDataLog-" +
                            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        dmGPSDataMatrix = null;
                        gpsDateTimeValuesList.Clear();
                    }
                }



                if (pressureHasBeenChanged)
                {
                    pressureDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                    pressureValuesList.Add(pressure);

                    if (pressureDateTimeValuesList.Count >= 10)
                    {
                        ThreadSafeOperations.SetText(lblPressureValue, pressure.ToString(), false);

                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();

                        long[] datetimeDataArray = pressureDateTimeValuesList.ToArray();
                        dataToSave.Add("DateTime", datetimeDataArray);

                        int[] pressureArray = pressureValuesList.ToArray();
                        dataToSave.Add("PressureData", pressureArray);

                        NetCDFoperations.AddVariousDataToFile(dataToSave, Directory.GetCurrentDirectory() +
                            "\\logs\\PressureDataLog-" + DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        pressureValuesList.Clear();
                        pressureDateTimeValuesList.Clear();
                    }
                    pressureHasBeenChanged = false;
                }





                datetimeCamshotTimerNow = DateTime.Now;
                camshotTimer = datetimeCamshotTimerNow - datetimeCamshotTimerBegin;
                if (camshotTimer >= camshotPeriod)
                {
                    camshotTimeToGetIt = true;
                    ThreadSafeOperations.ToggleLabelTextColor(lblNextShotIn, SystemColors.Highlight);
                    datetimeCamshotTimerBegin = datetimeCamshotTimerNow;
                }
                if (itsTimeToGetCamShot)
                {
                    camshotTimeToGetIt = true;
                    ThreadSafeOperations.ToggleLabelTextColor(lblNextShotIn, SystemColors.Highlight);
                }


                if (accDevAngle > Math.PI * 5.0d / 180.0d) camshotInclinedProperly = false;
                else camshotInclinedProperly = true;

                if ((camshotInclinedProperly && camshotTimeToGetIt) || getCamShotImmediately)
                {
                    catchCameraImage();
                    camshotHasBeenTaken = true;
                    datetimeCamshotHasBeenTaken = DateTime.Now;
                }

                if (camshotHasBeenTaken)
                {
                    Note("Got a shot " + datetimeCamshotTimerNow.ToString());
                    camshotInclinedProperly = false;
                    camshotHasBeenTaken = false;
                    camshotTimeToGetIt = false;
                    ThreadSafeOperations.ToggleLabelTextColor(lblNextShotIn, SystemColors.ControlText);
                    itsTimeToGetCamShot = false;
                    getCamShotImmediately = false;

                    logCurrentSensorsData();
                }

                if (datetimeCamshotTimerNow - datetimePreviousLabelsUpdate >= labelsUpdatingPeriod)
                {
                    datetimePreviousLabelsUpdate = datetimeCamshotTimerNow;
                    updateTimersLabels(datetimeCamshotHasBeenTaken, datetimeCamshotTimerBegin, camshotPeriod);
                }
            }
        }


        private void updateTimersLabels(DateTime datetimeCamShothasBeenTaken, DateTime datetimePreviousTimerBegin, TimeSpan camshotPeriod)
        {
            //int timespanTicksInSecond = 1000000;
            DateTime datetimeNow = DateTime.Now;
            DateTime nextshotAt = datetimePreviousTimerBegin + camshotPeriod;
            TimeSpan nextShotIn = (nextshotAt - datetimeNow);
            nextShotIn = new TimeSpan(0, 0, (int)nextShotIn.TotalSeconds + 1);
            ThreadSafeOperations.SetText(lblNextShotIn, nextShotIn.ToString("c"), false);
            TimeSpan timeSinceLastShot = datetimeNow - datetimeCamShothasBeenTaken;
            timeSinceLastShot = new TimeSpan(0, 0, (int)timeSinceLastShot.TotalSeconds);
            ThreadSafeOperations.SetText(lblSinceLastShot, timeSinceLastShot.ToString("c"), false);
        }


        private string swapImageToFile(Image image2write, String imageFNameAttrs = "")
        {
            String filename1 = Directory.GetCurrentDirectory() + "\\img-" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
            filename1 += imageFNameAttrs;
            filename1 += ".jpg";

            Bitmap bm2write = new Bitmap(image2write);
            bm2write.Save(filename1, System.Drawing.Imaging.ImageFormat.Jpeg);

            return filename1;
        }



        private void logCurrentSensorsData()
        {
            String filename1 = Directory.GetCurrentDirectory() + "\\results\\data-" + DateTime.UtcNow.ToString("o").Replace(":", "-") + ".xml";
            Dictionary<string, object> dataToSave = new Dictionary<string, object>();


            //String txtToLog = "datetime;" + Environment.NewLine +
            //                  "accelerometer Shift X rel; accelerometer Shift Y rel; accelerometer Shift Z rel;" + Environment.NewLine +
            //                  "magnetometer Shift X; magnetometer Shift Y; magnetometer Shift Z; magnetometer Heading Degrees;" + Environment.NewLine +
            //                  "GPSdata;" + Environment.NewLine +
            //                  "pressure, Pa" + Environment.NewLine + Environment.NewLine;
            //txtToLog += DateTime.UtcNow.ToString("o") + ";" + Environment.NewLine;



            accelerometerData accDataShift = latestAccData - accCalibrationData;
            accDataShift = accDataShift / accCalibrationData.AccMagnitude;

            dataToSave.Add("DateTime", DateTime.UtcNow.ToString("o"));

            dataToSave.Add("AccCalibrationValueX", accDataShift.AccDoubleX);
            dataToSave.Add("AccCalibrationValueY", accDataShift.AccDoubleY);
            dataToSave.Add("AccCalibrationValueZ", accDataShift.AccDoubleZ);

            dataToSave.Add("AccShiftDoubleX", accDataShift.AccDoubleX);
            dataToSave.Add("AccShiftDoubleY", accDataShift.AccDoubleY);
            dataToSave.Add("AccShiftDoubleZ", accDataShift.AccDoubleZ);

            dataToSave.Add("GyroValueX", latestGyroData.GyroDoubleX);
            dataToSave.Add("GyroValueY", latestGyroData.GyroDoubleY);
            dataToSave.Add("GyroValueZ", latestGyroData.GyroDoubleZ);

            dataToSave.Add("MagnCalibrationValueX", magnData.MagnDoubleX);
            dataToSave.Add("MagnCalibrationValueY", magnData.MagnDoubleY);
            dataToSave.Add("MagnCalibrationValueZ", magnData.MagnDoubleZ);
            dataToSave.Add("MagnCalibrationValueAngleShiftDegrees",
                PointPolar.CropAngleDegrees(magnData.compassAngle(latestAccData) + magnCalibratedAngleShift));

            dataToSave.Add("MagnDoubleX", magnData.MagnDoubleX);
            dataToSave.Add("MagnDoubleY", magnData.MagnDoubleY);
            dataToSave.Add("MagnDoubleZ", magnData.MagnDoubleZ);
            dataToSave.Add("MagnetometerHeadingDegrees",
                PointPolar.CropAngleDegrees(magnData.compassAngle(latestAccData) + magnCalibratedAngleShift));

            dataToSave.Add("GPSdata", gpsData.GPSstring);
            dataToSave.Add("GPSLat", gpsData.Lat);
            dataToSave.Add("GPSLon", gpsData.Lon);
            dataToSave.Add("GPSDateTimeUTC", gpsData.dateTimeUTC.ToString("o"));
            dataToSave.Add("PressurePa", pressure);

            ServiceTools.WriteDictionaryToXml(dataToSave, filename1, false);

            //txtToLog += accDataShift.AccDoubleX.ToString() + ";" + accDataShift.AccDoubleY.ToString() + ";" +
            //            accDataShift.AccDoubleZ.ToString() + ";" + Environment.NewLine;
            //txtToLog += magnData.MagnDoubleX.ToString() + ";" + magnData.MagnDoubleY.ToString() + ";" +
            //            magnData.MagnDoubleZ.ToString() + ";" +
            //            PointPolar.CropAngleDegrees(magnData.compassAngle(accData) + magnCalibratedAngleShift)
            //                .ToString() + ";" +
            //            Environment.NewLine;
            //txtToLog += gpsData.GPSstring + Environment.NewLine;
            //txtToLog += pressure;


            //ServiceTools.logToTextFile(filename1, txtToLog, true);
        }




        private void dataCollector_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (needsToSwitchListeningArduinoOFF)
            {
                needsToSwitchListeningArduinoOFF = false;
                udpCatchingJob.CancelAsync();
            }
            ThreadSafeOperations.ToggleButtonState(btnStartStopCollecting, true, "Start collecting data", false);
            dataCollectingState = DataCollectingStates.idle;
        }

        private void btnCollectImmediately_Click(object sender, EventArgs e)
        {
            getCamShotImmediately = true;
        }

        private void btnCollectMostClose_Click(object sender, EventArgs e)
        {
            itsTimeToGetCamShot = true;
        }



        //private void catchCameraImage()
        //{
        //    String username = tbCamUName.Text;
        //    String password = tbCamPWD.Text;
        //    String ipAddrVivotekCam = tbCamIP.Text.Replace(",", ".");
        //    String imageURL2Get = "http://" + ipAddrVivotekCam + "/cgi-bin/viewer/video.jpg?quality=5";
        //
        //    try
        //    {
        //        WebClient client = new WebClient();
        //        client.Credentials = new NetworkCredential(username, password);
        //        Stream stream = client.OpenRead(imageURL2Get);
        //        currentShowingImage = Image.FromStream(stream);
        //        stream.Flush();
        //        stream.Close();
        //
        //        ThreadSafeOperations.UpdatePictureBox(pbThumbPreview, currentShowingImage, true);
        //        Note(swapImageToFile(currentShowingImage, ""));
        //    }
        //    catch (Exception e)
        //    {
        //        Note(e.Message);
        //    }
        //}
        private void catchCameraImage()
        {
            string timeStampStr = DateTime.UtcNow.ToString("o") + Environment.NewLine;
            Note(timeStampStr + "The picture could be taken here");

        }





        private void tbCamShotPeriod_TextChanged(object sender, EventArgs e)
        {
            TimeSpan tmpCamShotPeriod = CamShotPeriod;

            if (!camshotPeriodDataSavingCircle.Visible)
            {
                camshotPeriodDataSavingCircle.Visible = true;
                camshotPeriodDataSavingCircle.Active = true;
            }

            try
            {
                CamShotPeriod = new TimeSpan(Convert.ToInt32(tbCamShotPeriod.Text.Substring(0, 2)),
                    Convert.ToInt32(tbCamShotPeriod.Text.Substring(3, 2)),
                    Convert.ToInt32(tbCamShotPeriod.Text.Substring(6, 2)));
                SaveSettings();
                camshotPeriodDataSavingCircle.Active = false;
                camshotPeriodDataSavingCircle.Visible = false;
            }
            catch (Exception exc)
            {
                //Note(exc.Message);
                //throw;
            }
        }



        private void tbCamIP_TextChanged(object sender, EventArgs e)
        {
            String IPAddrString = tbCamIP1.Text;


            IPAddrString = IPAddrString.Replace(",", ".");

            ThreadSafeOperations.SetLoadingCircleState(ipAddrValidatingCircle1, true, true, ipAddrValidatingCircle1.Color);
            
            if (IPAddress.TryParse(IPAddrString, out VivotekCameraID1IPaddress))
            {
                SaveSettings();
                ThreadSafeOperations.SetLoadingCircleState(ipAddrValidatingCircle1, false, false, ipAddrValidatingCircle1.Color);
            }
        }



        private void tbCamUName_TextChanged(object sender, EventArgs e)
        {
            VivotekCameraUserName1 = tbCamUName1.Text;
            SaveSettings();
        }



        private void tbCamPWD_TextChanged(object sender, EventArgs e)
        {
            VivotekCameraPassword1 = tbCamPWD1.Text;
            SaveSettings();
        }


        #region accelerpmeter calibration

        private void btnCalibrateAccelerometer_Click(object sender, EventArgs e)
        {
            if (!accelCalibrator.IsBusy)
            {
                if (!udpCatchingJob.IsBusy)
                {
                    //включим прослушку Arduino
                    needsToSwitchListeningArduinoOFF = true;
                    btnStartStopBdcstListening_Click(null, null);
                }
                Note("Detecting outdoor board broadcasting state");
                ThreadSafeOperations.ToggleButtonState(btnCalibrateAccelerometer, false, "wait for state checking", true);
                StartStopDataCollectingWaitingCircle.Visible = true;
                StartStopDataCollectingWaitingCircle.Active = true;
                dataCollectingState = DataCollectingStates.checkingState;
                theWorkerRequestedArduinoDataBroadcastState = WorkersRequestingArduinoDataBroadcastState.accelCalibrator;
                PerformRequestArduinoBoard("1");
            }
            else
            {
                accelCalibrator.CancelAsync();
            }
        }


        private void accelCalibrator_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime datetimeCalibrationBegin = DateTime.Now;
            DateTime datetimeCalibrationTimerNow = DateTime.Now;
            TimeSpan calibrationTimer = datetimeCalibrationTimerNow - datetimeCalibrationBegin;
            TimeSpan calibrationRecalcPeriod = new TimeSpan(0, 0, 1);
            TimeSpan labelsUpdatingPeriod = new TimeSpan(0, 0, 1);
            DateTime datetimePreviousLabelsUpdate = DateTime.MinValue;
            double horizAcc, totalAcc, percHorizAcc;
            //double percAccDeviation;
            double meanX, meanY, meanZ, stDevX, stDevY, stDevZ;
            double[] dataSetX = new double[1000];
            double[] dataSetY = new double[1000];
            double[] dataSetZ = new double[1000];
            int i = 0;
            bool isTheFirstPass = true;

            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            ThreadSafeOperations.ToggleButtonState(btnCalibrateAccelerometer, true, "Stop calibrating", true);
            dataCollectingState = DataCollectingStates.working;


            while (true)
            {
                if (SelfWorker.CancellationPending)
                {
                    break;
                }

                if (accDataQueue.Count > 0)
                {
                    accelerometerData accData = accDataQueue.Dequeue();
                    if (accData == null)
                    {
                        continue;
                    }

                    ThreadSafeOperations.SetText(lblAccelCalibrationCurrentX, accData.AccX.ToString(), false);
                    ThreadSafeOperations.SetText(lblAccelCalibrationCurrentY, accData.AccY.ToString(), false);
                    ThreadSafeOperations.SetText(lblAccelCalibrationCurrentZ, accData.AccZ.ToString(), false);

                    dataSetX[i] = accData.AccDoubleX;
                    dataSetY[i] = accData.AccDoubleY;
                    dataSetZ[i] = accData.AccDoubleZ;
                    if (isTheFirstPass)
                    {
                        for (int j = i + 1; j < 1000; j++)
                        {
                            dataSetX[j] = accData.AccDoubleX;
                            dataSetY[j] = accData.AccDoubleY;
                            dataSetZ[j] = accData.AccDoubleZ;
                        }
                    }
                    i++; if (i > 999)
                    {
                        i = 0;
                        isTheFirstPass = false;
                    }
                    DateTime calcBegin = DateTime.Now;
                    DescriptiveStatistics statisticsX = new DescriptiveStatistics((System.Collections.Generic.IEnumerable<double>)dataSetX);
                    DescriptiveStatistics statisticsY = new DescriptiveStatistics((System.Collections.Generic.IEnumerable<double>)dataSetY);
                    DescriptiveStatistics statisticsZ = new DescriptiveStatistics((System.Collections.Generic.IEnumerable<double>)dataSetZ);
                    meanX = statisticsX.Mean;
                    stDevX = Math.Round(100 * statisticsX.StandardDeviation / meanX, 2);
                    meanY = statisticsY.Mean;
                    stDevY = Math.Round(100 * statisticsY.StandardDeviation / meanY, 2);
                    meanZ = statisticsZ.Mean;
                    stDevZ = Math.Round(100 * statisticsZ.StandardDeviation / meanZ, 2);
                    TimeSpan calcSpan = DateTime.Now - calcBegin;

                    accCalibrationData.AccDoubleX = meanX;
                    accCalibrationData.AccDoubleY = meanY;
                    accCalibrationData.AccDoubleZ = meanZ;

                    ThreadSafeOperations.SetText(lblAccelCalibrationX, "<" + Math.Round(meanX, 2).ToString() + ">", false);
                    ThreadSafeOperations.SetText(lblAccelCalibrationY, "<" + Math.Round(meanY, 2).ToString() + ">", false);
                    ThreadSafeOperations.SetText(lblAccelCalibrationZ, "<" + Math.Round(meanZ, 2).ToString() + ">", false);
                    ThreadSafeOperations.SetText(lblStDevX, stDevX.ToString() + "%", false);
                    ThreadSafeOperations.SetText(lblStDevY, stDevY.ToString() + "%", false);
                    ThreadSafeOperations.SetText(lblStDevZ, stDevZ.ToString() + "%", false);
                    string txt2Show = "i = " + i.ToString();
                    txt2Show += Environment.NewLine + "calc time = " + calcSpan.Ticks.ToString();
                    ThreadSafeOperations.SetText(lblCalculationStatistics, txt2Show, false);
                    //accDatahasBeenChanged = false;
                }

                datetimeCalibrationTimerNow = DateTime.Now;
                calibrationTimer = datetimeCalibrationTimerNow - datetimeCalibrationBegin;
            }
        }



        private void accelCalibrator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (needsToSwitchListeningArduinoOFF)
            {
                needsToSwitchListeningArduinoOFF = false;
                udpCatchingJob.CancelAsync();
            }
            ThreadSafeOperations.ToggleButtonState(btnCalibrateAccelerometer, true, "Calibrate accelerometer", false);
            dataCollectingState = DataCollectingStates.idle;
        }

        private void btnSaveAccel_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> propertiesDictToSave = new Dictionary<string, object>();
            propertiesDictToSave.Add("accCalibratedXzero", accCalibrationData.AccDoubleX);
            propertiesDictToSave.Add("accCalibratedYzero", accCalibrationData.AccDoubleY);
            propertiesDictToSave.Add("accCalibratedZzero", accCalibrationData.AccDoubleZ);
            ServiceTools.WriteDictionaryToXml(propertiesDictToSave, accCalibrationDataFilename, false);
        }

        #endregion accelerpmeter calibration



        #region magnetometer calibration

        //private void magnCalibrator_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    DateTime datetimeCalibrationBegin = DateTime.Now;
        //    DateTime datetimeCalibrationTimerNow = DateTime.Now;
        //    TimeSpan calibrationTimer = datetimeCalibrationTimerNow - datetimeCalibrationBegin;
        //    TimeSpan calibrationRecalcPeriod = new TimeSpan(0, 0, 1);
        //    TimeSpan labelsUpdatingPeriod = new TimeSpan(0, 0, 1);
        //    DateTime datetimePreviousLabelsUpdate = DateTime.MinValue;

        //    double meanX, meanY, meanZ, stDevX, stDevY, stDevZ;
        //    double[] dataSetX = new double[1000];
        //    double[] dataSetY = new double[1000];
        //    double[] dataSetZ = new double[1000];
        //    int i = 0;
        //    bool isTheFirstPass = true;

        //    System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
        //    ThreadSafeOperations.ToggleButtonState(btnCalibrateMagnetometer, true, "Stop calibrating", true);
        //    dataCollectingState = DataCollectingStates.working;


        //    while (true)
        //    {
        //        if (SelfWorker.CancellationPending)
        //        {
        //            break;
        //        }

        //        if (magnDatahasBeenChanged)
        //        {
        //            ThreadSafeOperations.SetText(lblMagnCalibrationCurrentX, magnData.MagnX.ToString(), false);
        //            ThreadSafeOperations.SetText(lblMagnCalibrationCurrentY, magnData.MagnY.ToString(), false);
        //            ThreadSafeOperations.SetText(lblMagnCalibrationCurrentZ, magnData.MagnZ.ToString(), false);

        //            dataSetX[i] = magnData.MagnDoubleX;
        //            dataSetY[i] = magnData.MagnDoubleY;
        //            dataSetZ[i] = magnData.MagnDoubleZ;
        //            if (isTheFirstPass)
        //            {
        //                for (int j = i + 1; j < 1000; j++)
        //                {
        //                    dataSetX[j] = magnData.MagnDoubleX;
        //                    dataSetY[j] = magnData.MagnDoubleY;
        //                    dataSetZ[j] = magnData.MagnDoubleZ;
        //                }
        //            }
        //            i++; if (i > 999)
        //            {
        //                i = 0;
        //                isTheFirstPass = false;
        //            }
        //            DateTime calcBegin = DateTime.Now;
        //            DescriptiveStatistics statisticsX = new DescriptiveStatistics((System.Collections.Generic.IEnumerable<double>)dataSetX);
        //            DescriptiveStatistics statisticsY = new DescriptiveStatistics((System.Collections.Generic.IEnumerable<double>)dataSetY);
        //            DescriptiveStatistics statisticsZ = new DescriptiveStatistics((System.Collections.Generic.IEnumerable<double>)dataSetZ);
        //            meanX = statisticsX.Mean;
        //            stDevX = Math.Round(100 * statisticsX.StandardDeviation / meanX, 2);
        //            meanY = statisticsY.Mean;
        //            stDevY = Math.Round(100 * statisticsY.StandardDeviation / meanY, 2);
        //            meanZ = statisticsZ.Mean;
        //            stDevZ = Math.Round(100 * statisticsZ.StandardDeviation / meanZ, 2);
        //            TimeSpan calcSpan = DateTime.Now - calcBegin;

        //            magnCalibrationData.MagnDoubleX = meanX;
        //            magnCalibrationData.MagnDoubleY = meanY;
        //            magnCalibrationData.MagnDoubleZ = meanZ;

        //            ThreadSafeOperations.SetText(lblMagnCalibrationX, "<" + Math.Round(meanX, 2).ToString() + ">", false);
        //            ThreadSafeOperations.SetText(lblMagnCalibrationY, "<" + Math.Round(meanY, 2).ToString() + ">", false);
        //            ThreadSafeOperations.SetText(lblMagnCalibrationZ, "<" + Math.Round(meanZ, 2).ToString() + ">", false);
        //            ThreadSafeOperations.SetText(lblMagnStDevX, stDevX.ToString() + "%", false);
        //            ThreadSafeOperations.SetText(lblMagnStDevY, stDevY.ToString() + "%", false);
        //            ThreadSafeOperations.SetText(lblMagnStDevZ, stDevZ.ToString() + "%", false);

        //            if (latestAccData != null)
        //            {
        //                ThreadSafeOperations.SetText(lblCaughtMagnCalibrationValue,
        //                    magnCalibrationData.compassAngle(latestAccData).ToString(), false);
        //            }
        //            else
        //            {
        //                ThreadSafeOperations.SetText(lblCaughtMagnCalibrationValue,
        //                    magnCalibrationData.compassAngle().ToString(), false);
        //            }

        //            string txt2Show = "i = " + i.ToString();
        //            txt2Show += Environment.NewLine + "calc time = " + calcSpan.Ticks.ToString();
        //            ThreadSafeOperations.SetText(lblMagnCalculationStatistics, txt2Show, false);
        //            magnDatahasBeenChanged = false;
        //        }

        //        datetimeCalibrationTimerNow = DateTime.Now;
        //        calibrationTimer = datetimeCalibrationTimerNow - datetimeCalibrationBegin;
        //    }
        //}

        //private void btnCalibrateMagnetometer_Click(object sender, EventArgs e)
        //{
        //    if (!magnCalibrator.IsBusy)
        //    {
        //        if (!udpCatchingJob.IsBusy)
        //        {
        //            //включим прослушку Arduino
        //            needsToSwitchListeningArduinoOFF = true;
        //            btnStartStopBdcstListening_Click(null, null);
        //        }
        //        Note("Detecting outdoor board broadcasting state");
        //        ThreadSafeOperations.ToggleButtonState(btnCalibrateMagnetometer, false, "wait for state checking", true);
        //        StartStopDataCollectingWaitingCircle.Visible = true;
        //        StartStopDataCollectingWaitingCircle.Active = true;
        //        dataCollectingState = DataCollectingStates.checkingState;
        //        theWorkerRequestedArduinoDataBroadcastState = WorkersRequestingArduinoDataBroadcastState.magnCalibrator;
        //        PerformRequestArduinoBoard("1");
        //    }
        //    else
        //    {
        //        magnCalibrator.CancelAsync();
        //    }
        //}

        //private void magnCalibrator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (needsToSwitchListeningArduinoOFF)
        //    {
        //        needsToSwitchListeningArduinoOFF = false;
        //        udpCatchingJob.CancelAsync();
        //    }
        //    ThreadSafeOperations.ToggleButtonState(btnCalibrateMagnetometer, true, "Calibrate magnetometer", false);
        //    dataCollectingState = DataCollectingStates.idle;
        //}

        //private void btnMagnSaveCalibration_Click(object sender, EventArgs e)
        //{
        //    magnCalibratedAngleShift = Convert.ToDouble(tbCurrentCompassHeadingValue.Text) -
        //                               Convert.ToDouble(lblCaughtMagnCalibrationValue.Text);
        //    Dictionary<string, object> propertiesDictToSave = new Dictionary<string, object>();
        //    propertiesDictToSave.Add("magnCalibratedZeroX", magnCalibrationData.MagnDoubleX);
        //    propertiesDictToSave.Add("magnCalibratedZeroY", magnCalibrationData.MagnDoubleY);
        //    propertiesDictToSave.Add("magnCalibratedZeroZ", magnCalibrationData.MagnDoubleZ);
        //    propertiesDictToSave.Add("magnCalibratedAngleShift", magnCalibratedAngleShift);
        //    ServiceTools.WriteDictionaryToXml(propertiesDictToSave, magnCalibrationDataFilename, false);
        //}

        #endregion magnetometer calibration



        private void lblAccelerometerSign_Click(object sender, EventArgs e)
        {
            ShowAccelerometerHistoryPicture();
        }

        private void btnSwitchShowingTotalLog_Click(object sender, EventArgs e)
        {
            if (showTotalBcstLog)
            {
                showTotalBcstLog = !showTotalBcstLog;
                ThreadSafeOperations.ToggleButtonState(btnSwitchShowingTotalLog, true, "Show total log", true);
            }
            else
            {
                showTotalBcstLog = !showTotalBcstLog;
                ThreadSafeOperations.ToggleButtonState(btnSwitchShowingTotalLog, true, "Dont show total log", true);
            }
        }




        private void ShowAccelerometerHistoryPicture()
        {
            //AccelerometerDataRepresentingForm accForm =
            //    new AccelerometerDataRepresentingForm(
            //        Directory.GetCurrentDirectory() + "\\logs\\AccelerometerDataLog-" +
            //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc", SensorsHistoryShowing.Accelerometer);

            AccelerometerDataRepresentingForm accForm = new AccelerometerDataRepresentingForm();

            if (!accForm.IsDisposed)
            {
                accForm.Show();
            }
        }









        //#region External network data stream

        //private void btnConnect_Click(object sender, EventArgs e)
        //{
        //    Socket s = ConnectSocket("169.254.249.87", 1977);
        //    if (s == null) return;

        //    Byte[] bytesReceived = new Byte[256];

        //    int bytes = 0;
        //    string gotString = "";
        //    do
        //    {
        //        bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
        //        gotString = Encoding.ASCII.GetString(bytesReceived, 0, bytes);
        //        ThreadSafeOperations.SetTextTB(tbExternalDataStream, gotString + Environment.NewLine, true);
        //    }
        //    while (bytes > 0);

        //}

        //private Socket ConnectSocket(string server, int port)
        //{
        //    Socket s = null;
        //    IPHostEntry hostEntry = null;
        //    IPAddress address;
        //    IPAddress.TryParse(server, out address);

        //    IPEndPoint ipe = new IPEndPoint(address, port);
        //    Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //    try
        //    {
        //        tempSocket.Connect(ipe);
        //    }
        //    catch (Exception)
        //    {
        //        ThreadSafeOperations.SetTextTB(tbExternalDataStream, "coudn`t connect to remote server" + Environment.NewLine, true);
        //        return null;
        //    }


        //    if (tempSocket.Connected)
        //    {
        //        ThreadSafeOperations.SetTextTB(tbExternalDataStream, "connected to remote server " + server + Environment.NewLine, true);
        //        s = tempSocket;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    return s;
        //}


        //private void bgwNavMeteoDataNetStreamReader_DoWork(object sender, DoWorkEventArgs e)
        //{

        //}

        //#endregion  External network data stream




        private void StartUDPmessagesParser()
        {
            if (!bgwUDPmessagesParser.IsBusy)
            {
                bgwUDPmessagesParser.RunWorkerAsync();
            }
        }




        private void bgwUDPmessagesParser_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            //int currMessageDevID = 0;

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
                    IncomingUDPmessageBoundle curMessageBoundle = quArduinoUDPCatchedMessages.Dequeue();

                    if (curMessageBoundle == null) continue;

                    string bcstMessage = curMessageBoundle.udpMessage;
                    int currMessageDevID = curMessageBoundle.devID;

                    if (curMessageBoundle.isReplyMessage)
                    {
                        //bcstMessage = bcstMessage.Substring(6, bcstMessage.Length - 6);
                        if (currMessageDevID == 1)
                        {
                            Note("devID:" + currMessageDevID + "   |   " + bcstMessage);
                        }
                        else if (currMessageDevID == 2)
                        {
                            Note("devID:" + currMessageDevID + "   |   " + bcstMessage);
                        }
                        else
                        {
                            Note(bcstMessage);
                        }
                        //udpMessage = bcstMessage;
                        replyMessage = bcstMessage;
                        if (needsReplyOnRequest) needsReplyOnRequest = false;
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
                        //udpMessage = bcstMessage;
                    }
                    else if (bcstMessage == "imarduino")
                    {
                        if (needsToDiscoverArduinoBoard)
                        {
                            SocketAddress tmpSktAddress = remoteSktAddr;
                            string addrStr = tmpSktAddress.ToString();
                            //InterNetwork:16:{21,179,192,168,192,221,0,0,0,0,0,0,0,0}
                            addrStr = addrStr.Substring(addrStr.IndexOf("{") + 1);
                            addrStr = addrStr.Substring(0, addrStr.Length - 1);
                            char[] splitChar = { ',' };
                            string[] sktAddrStrArray = addrStr.Split(splitChar);
                            //Note(addrStr);
                            addrStr = sktAddrStrArray[2] + "." + sktAddrStrArray[3] + "." + sktAddrStrArray[4] + "." +
                                      sktAddrStrArray[5];
                            //Note(addrStr);
                            ThreadSafeOperations.SetTextTB(tbIP2ListenDevID1, addrStr, false);
                            needsToDiscoverArduinoBoard = false;
                        }
                    }
                    else
                    {
                        if (!needsToDiscoverArduinoBoard)
                        {
                            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

                            string timeStampStr = DateTime.UtcNow.ToString("o") + ": ";

                            if (currMessageDevID > 0)
                            {
                                NoteLog("devID:" + currMessageDevID + "   |   " + timeStampStr + bcstMessage);
                            }
                            else
                            {
                                NoteLog(timeStampStr + bcstMessage);
                            }

                            //quArduinoUDPCatchedMessages.Enqueue(bcstMessage);
                            ParseBroadcastMessage(bcstMessage, currMessageDevID);
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

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            String IPAddrString2 = tbCamIP2.Text;


            IPAddrString2 = IPAddrString2.Replace(",", ".");

            ThreadSafeOperations.SetLoadingCircleState(ipAddrValidatingCircle2, true, true, ipAddrValidatingCircle2.Color);

            if (IPAddress.TryParse(IPAddrString2, out VivotekCameraID2IPaddress))
            {
                SaveSettings();
                ThreadSafeOperations.SetLoadingCircleState(ipAddrValidatingCircle2, false, false, ipAddrValidatingCircle2.Color);
            }
        }

        private void tbCamUName2_TextChanged(object sender, EventArgs e)
        {
            VivotekCameraUserName2 = tbCamUName2.Text;
            SaveSettings();
        }

        private void tbIP2ListenDevID2_TextChanged(object sender, EventArgs e)
        {
            ip2ListenID2 = tbIP2ListenDevID2.Text;
        }

        private void tbCamPWD2_TextChanged(object sender, EventArgs e)
        {
            VivotekCameraPassword2 = tbCamPWD2.Text;
            SaveSettings();
        }




    }

    #endregion



    #region tools-classes

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

    #endregion
}
