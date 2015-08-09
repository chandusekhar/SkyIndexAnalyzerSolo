using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using MRG.Controls.UI;
using MathNet.Numerics.Statistics;
using SkyImagesAnalyzer;
using SkyImagesAnalyzerLibraries;
using SolarPositioning;
using Timer = System.Threading.Timer;


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


    public enum ButtonBackgroundStateWatchingProcess
    {
        allGood,
        alarm,
        notWatching
    }



    #endregion




    #region the form class

    public partial class DataCollectorMainForm : Form
    {
        private System.Threading.Timer workingControlTimer = null;

        private string ip2ListenID1 = "";
        private string ip2ListenID2 = "";
        private int port2converse = 5555;
        private int portBcstRecvng = 4444;
        //private string currCommand;

        //private static String udpMessage = "";
        private static String replyMessage = "";
        private static int currOperatingDevID = 1;
        private static bool needsToDiscoverArduinoBoardID1 = false;
        private static bool needsToDiscoverArduinoBoardID2 = false;
        private static SocketAddress remoteSktAddr;
        private static bool needsReplyOnRequest = false;
        private static String ArduinoRequestString = "";
        private static DataCollectingStates dataCollectingState = DataCollectingStates.idle;
        //private static DateTime JanFirst1970 = new DateTime(1970, 1, 1);
        IntPtr m_ip = IntPtr.Zero;

        // private static Queue<accelerometerData> accDataQueue = new Queue<accelerometerData>();
        //private static Queue<Tuple<DateTime, accelerometerData>> accDataAndDTqueue = new Queue<Tuple<DateTime, accelerometerData>>();
        private static ObservableConcurrentQueue<Tuple<DateTime, AccelerometerData>> accDataAndDTObservableConcurrentQueue =
            new ObservableConcurrentQueue<Tuple<DateTime, AccelerometerData>>();
        private static AccelerometerData latestAccData = new AccelerometerData();

        //private static Queue<GyroData> gyroDataQueue = new Queue<GyroData>();
        //private static Queue<Tuple<DateTime, GyroData>> gyroDataAndDTqueue = new Queue<Tuple<DateTime, GyroData>>();
        private static ObservableConcurrentQueue<Tuple<DateTime, GyroData>> gyroDataAndDTObservableConcurrentQueue =
            new ObservableConcurrentQueue<Tuple<DateTime, GyroData>>();
        private static GyroData latestGyroData = new GyroData();


        //private static MagnetometerData magnData = new MagnetometerData();

        private static ObservableConcurrentQueue<Tuple<DateTime, GPSdata>> gpsDataAndDTObservableConcurrentQueue =
            new ObservableConcurrentQueue<Tuple<DateTime, GPSdata>>();
        private static GPSdata latestGPSdata = new GPSdata();

        //private static GPSdata gpsData = new GPSdata();

        //private static int pressure = 0;
        private static ObservableConcurrentQueue<Tuple<DateTime, int>> pressureDataAndDTObservableConcurrentQueue =
            new ObservableConcurrentQueue<Tuple<DateTime, int>>();
        private static int latestPressureValue = 0;

        private static bool needsToSwitchListeningArduinoOFF = false;
        //private static bool accDatahasBeenChanged = false;
        //private static bool gyroDatahasBeenChanged = false;
        //private static bool magnDatahasBeenChanged = false;
        //private static bool gpsDataHasBeenChanged = false;
        //private static bool pressureHasBeenChanged = false;

        private static bool operatorCommandsToGetCamShot = false;
        //private static bool operatorCommandsToGetCamShotImmediately = false;
        private static WorkersRequestingArduinoDataBroadcastState theWorkerRequestedArduinoDataBroadcastState = WorkersRequestingArduinoDataBroadcastState.dataCollector;
        private static AccelerometerData accCalibrationDataID1;
        private static AccelerometerData accCalibrationDataID2;
        //private static MagnetometerData magnCalibrationData;
        //private static double magnCalibratedAngleShift;
        public static Image currentShowingImageID1;
        public static Image currentShowingImageID2;

        //public static TextBox reportingTextBox;

        //private string generalSettingsFilename = "";
        //private string magnCalibrationDataFilename = "";
        private int BroadcastLogHistorySizeLines = 4096;
        private TimeSpan CamShotPeriod = new TimeSpan(0, 1, 0);
        private IPAddress VivotekCameraID1IPaddress;
        private IPAddress VivotekCameraID2IPaddress;
        private string VivotekCameraUserName1 = "root";
        private string VivotekCameraPassword1 = "vivotek";
        private string VivotekCameraUserName2 = "root";
        private string VivotekCameraPassword2 = "vivotek";
        private double angleCamDeclinationThresholdDeg = 5;

        private static LogWindow theLogWindow = null;
        private string strTotalBcstLog = "";
        private bool showTotalBcstLog = false;

        private static ObservableConcurrentQueue<IncomingUDPmessageBoundle> cquArduinoUDPCatchedMessages =
            new ObservableConcurrentQueue<IncomingUDPmessageBoundle>();

        private static ConcurrentBag<IncomingUDPmessagesBoundlesTuple> cbArduinoMessagesTuples =
            new ConcurrentBag<IncomingUDPmessagesBoundlesTuple>();

        //private int sensorsHistoryRepresentingScale = 86400;
        //private SensorsHistoryShowing currRepresentingHistory = SensorsHistoryShowing.None;

        private static int recievedUDPPacketsCounter = 0;
        private static int processedUDPPacketsCounter = 0;
        //private static double recievingUDPpacketsSpeed = 0.0d;

        private static Image<Bgr, Byte> currCaughtImageID1 = null;
        private static Image<Bgr, Byte> currCaughtImageID2 = null;

        //public delegate void CheckIfEveryBgwWorkingHandler(object sender, NotAllBgwWorkingAlertEventArgs e);
        //private event CheckIfEveryBgwWorkingHandler CheckIfEveryBgwWorking;

        private Dictionary<string, object> defaultProperties = null;
        private string defaultPropertiesXMLfileName = "";
        private Dictionary<string, object> accCalibrationDataDict = null;
        private string accCalibrationDataFilename = "";

        private double angleCamDeclinationThresholdRad = Math.PI * 3.0d / 180.0d;

        private string errorLogFilename = Directory.GetCurrentDirectory() + "\\logs\\DataCollectorAutomator2G-error.log";

        public event EventHandler NeedToShootCameraSnapshots;




        public DataCollectorMainForm()
        {
            InitializeComponent();
        }





        private void NoteLog(string text)
        {
            if (showTotalBcstLog)
            {
                Note(text);
            }
            strTotalBcstLog += text + Environment.NewLine;
            if (strTotalBcstLog.Length >= (BroadcastLogHistorySizeLines * 200))
            {
                swapBcstLog();
            }
        }


        private void Note(string text)
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
            }
            else
            {
                if (sender == btnFindArduino1)
                {
                    needsToDiscoverArduinoBoardID1 = true;
                    ThreadSafeOperations.SetLoadingCircleState(SearchingArduinoID1ProcessCircle, true, true, SearchingArduinoID1ProcessCircle.Color);
                }
                else if (sender == btnFindArduino2)
                {
                    needsToDiscoverArduinoBoardID2 = true;
                    ThreadSafeOperations.SetLoadingCircleState(SearchingArduinoID2ProcessCircle, true, true, SearchingArduinoID2ProcessCircle.Color);
                }
                ThreadSafeOperations.ToggleButtonState(btnFindArduino1, true, "CANCEL", true);
                ThreadSafeOperations.ToggleButtonState(btnFindArduino2, true, "CANCEL", true);
                arduinoBoardSearchingWorker.RunWorkerAsync();
            }
        }





        private void arduinoBoardSearchingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            //needsToDiscoverArduinoBoard = true;
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

                // не нужно?..
                Application.DoEvents();
                Thread.Sleep(0);
                // не нужно?..
            }


            if (needsToSwitchCatchingOff)
            {
                btnStartStopBdcstListening_Click(null, null);
                needsToSwitchCatchingOff = false;
            }
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
                //StartUDPmessagesParser();
                ThreadSafeOperations.ToggleButtonState(btnStartStopBdcstListening, true, "Stop listening", true);
            }
        }




        private void udpCatchingJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.ToggleButtonState(btnStartStopBdcstListening, true, "Start listening", false);
            ChangeIndicatingButtonBackgroundColor(btnStartStopBdcstListening,
                ButtonBackgroundStateWatchingProcess.notWatching);

            //bgwUDPmessagesParser.CancelAsync();
        }





        private static bool recievingUDPmessage = false;

        private void BcstReceiveCallback(IAsyncResult ar)
        {
            string bcstMessage = "";
            UdpState udpSt = (UdpState)(ar.AsyncState);
            UdpClient udpClt = (UdpClient)(udpSt.UDPclient);
            IPEndPoint ipEP = (IPEndPoint)(udpSt.ipEndPoint);

            remoteSktAddr = PropertyHelper.GetPrivatePropertyValue<SocketAddress>((object)ar, "SocketAddress");


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

                        Interlocked.Increment(ref recievedUDPPacketsCounter);
                    }

                }
                catch (Exception ex)
                {
                    #region report
#if DEBUG
                    ServiceTools.ExecMethodInSeparateThread(this, () =>
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "exception has been thrown: " + ex.Message + Environment.NewLine +
                            ServiceTools.CurrentCodeLineDescription());
                    });
#else
                    ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif
                    #endregion report
                }
                recievingUDPmessage = false;
            }
            else
            {
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
            bcstUDPreader.EnableBroadcast = true;
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
                    continue;
                }


                if (bcstUDPreader.Available > 0)
                {
                    recievingUDPmessage = true;
                    bcstUDPreader.BeginReceive(BcstReceiveCallback, s);
                }
            }

            bcstUDPreader.Close();
        }




        private void btnSwapBcstLog_Click(object sender, EventArgs e)
        {
            swapBcstLog();
        }


        private void swapBcstLog()
        {
            string filename1 = Directory.GetCurrentDirectory() + "\\logs\\DataCollector2G-BcstLog-" +
                        DateTime.UtcNow.ToString("o").Replace(":", "-").Replace("Z", "") + ".log";
            ServiceTools.logToTextFile(filename1, strTotalBcstLog, true);

            strTotalBcstLog = "";


            string filename2 = Directory.GetCurrentDirectory() + "\\logs\\DataCollector2G-NotesLog-" +
                        DateTime.UtcNow.ToString("o").Replace(":", "-").Replace("Z", "") + ".log";
            ServiceTools.logToTextFile(filename2, tbMainLog.Text, true);
            ThreadSafeOperations.SetTextTB(tbMainLog, "", false);
        }




        private void arduinoBoardSearchingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.ToggleButtonState(btnFindArduino1, true, "search for board ID1", true);
            ThreadSafeOperations.ToggleButtonState(btnFindArduino2, true, "search for board ID2", true);
            ThreadSafeOperations.SetLoadingCircleState(SearchingArduinoID1ProcessCircle, false, false, SearchingArduinoID1ProcessCircle.Color);
            ThreadSafeOperations.SetLoadingCircleState(SearchingArduinoID2ProcessCircle, false, false, SearchingArduinoID2ProcessCircle.Color);
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (currentShowingImageID1 == null)
            {
                return;
            }
            Bitmap bm2show = new Bitmap(currentShowingImageID1);
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

                dataCollector.RunWorkerAsync();
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
            if (ArduinoRequestString == "1")
            {
                //найдем строку, в которой сказано про data broadcasting
                if ((replyMessage == "data broadcasting is OFF") && (dataCollectingState == DataCollectingStates.checkingState))
                {
                    Note("Outdoor facility data broadcasting is OFF. Turning it ON.");
                    PerformRequestArduinoBoard("2", currOperatingDevID);
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
                }
            }
        }






        private void ChangeIndicatingButtonBackgroundColor(Control btn, ButtonBackgroundStateWatchingProcess state = ButtonBackgroundStateWatchingProcess.notWatching)
        {
            if (btn.GetType() == typeof(Button))
            {
                switch (state)
                {
                    case ButtonBackgroundStateWatchingProcess.notWatching:
                        {
                            ThreadSafeOperations.SetButtonBackgroundColor(btn, Color.Gainsboro);
                            break;
                        }
                    case ButtonBackgroundStateWatchingProcess.allGood:
                        {
                            if (btn.BackColor != Color.Gainsboro)
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btn, Color.Gainsboro);
                            }
                            break;
                        }
                    case ButtonBackgroundStateWatchingProcess.alarm:
                        {
                            if (btn.BackColor == Color.Gainsboro)
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btn, Color.MistyRose);
                            }
                            else
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btn, Color.Gainsboro);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            else if (btn.GetType() == typeof(Label))
            {
                switch (state)
                {
                    case ButtonBackgroundStateWatchingProcess.notWatching:
                        {
                            ThreadSafeOperations.SetButtonBackgroundColor(btn, Color.Transparent);
                            break;
                        }
                    case ButtonBackgroundStateWatchingProcess.allGood:
                        {
                            if (btn.BackColor != Color.Transparent)
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btn, Color.Transparent);
                            }
                            break;
                        }
                    case ButtonBackgroundStateWatchingProcess.alarm:
                        {
                            if (btn.BackColor == Color.Transparent)
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btn, Color.MistyRose);
                            }
                            else
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btn, Color.Transparent);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

        }





        private void EstimateAndReportUDPpacketsRecievingSpeed(object state)
        {
            Stopwatch stwToEstimateUDPpacketsRecieving;
            try
            {
                stwToEstimateUDPpacketsRecieving = (state as object[])[0] as Stopwatch;
            }
            catch (Exception ex)
            {
                return;
            }


            double elapsedms = (double)(stwToEstimateUDPpacketsRecieving.ElapsedMilliseconds);

            if (elapsedms == 0)
            {
                stwToEstimateUDPpacketsRecieving.Restart();
                return;
            }

            //оценим скорость поступления пакетов
            double speedUDPrecieving = (double)recievedUDPPacketsCounter * 1000.0d /
                              (double)elapsedms;
            recievedUDPPacketsCounter = 0;

            double speedUDPprocessing = (double)processedUDPPacketsCounter * 1000.0d /
                                        (double)elapsedms;
            processedUDPPacketsCounter = 0;

            //theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine +
            //    "UDP recieving speed = " + speedUDPrecieving.ToString("F2") + Environment.NewLine +
            //    "UDP processing speed = " + speedUDPprocessing.ToString("F2"));
#if DEBUG
            ThreadSafeOperations.SetText(lblUDPpacketsRecievingSpeedValue, speedUDPrecieving.ToString("F2"), false);
            ThreadSafeOperations.SetText(lblUDPpacketsProcessingSpeedValue, speedUDPprocessing.ToString("F2"), false);
#endif


            stwToEstimateUDPpacketsRecieving.Restart();

            if (speedUDPrecieving <= 0.0d)
            {
                ChangeIndicatingButtonBackgroundColor(btnStartStopBdcstListening,
                    ButtonBackgroundStateWatchingProcess.alarm);
            }
            else
            {
                ChangeIndicatingButtonBackgroundColor(btnStartStopBdcstListening,
                    ButtonBackgroundStateWatchingProcess.allGood);
            }


            if (speedUDPprocessing <= 0.0d)
            {
                ChangeIndicatingButtonBackgroundColor(lblAccMagnValueID1,
                    ButtonBackgroundStateWatchingProcess.alarm);
                ChangeIndicatingButtonBackgroundColor(lblAccDevAngleValueID1,
                    ButtonBackgroundStateWatchingProcess.alarm);
                ChangeIndicatingButtonBackgroundColor(lblAccMagnValueID2,
                    ButtonBackgroundStateWatchingProcess.alarm);
                ChangeIndicatingButtonBackgroundColor(lblAccDevAngleValueID2,
                    ButtonBackgroundStateWatchingProcess.alarm);
                ChangeIndicatingButtonBackgroundColor(lblAccMagnTitleID1,
                    ButtonBackgroundStateWatchingProcess.alarm);
                ChangeIndicatingButtonBackgroundColor(lblAccDevAngleTitleID1,
                    ButtonBackgroundStateWatchingProcess.alarm);
                ChangeIndicatingButtonBackgroundColor(lblAccMagnTitleID2,
                    ButtonBackgroundStateWatchingProcess.alarm);
                ChangeIndicatingButtonBackgroundColor(lblAccDevAngleTitleID2,
                    ButtonBackgroundStateWatchingProcess.alarm);
            }
            else
            {
                ChangeIndicatingButtonBackgroundColor(lblAccMagnValueID1,
                    ButtonBackgroundStateWatchingProcess.allGood);
                ChangeIndicatingButtonBackgroundColor(lblAccDevAngleValueID1,
                    ButtonBackgroundStateWatchingProcess.allGood);
                ChangeIndicatingButtonBackgroundColor(lblAccMagnValueID2,
                    ButtonBackgroundStateWatchingProcess.allGood);
                ChangeIndicatingButtonBackgroundColor(lblAccDevAngleValueID2,
                    ButtonBackgroundStateWatchingProcess.allGood);
                ChangeIndicatingButtonBackgroundColor(lblAccMagnTitleID1,
                    ButtonBackgroundStateWatchingProcess.allGood);
                ChangeIndicatingButtonBackgroundColor(lblAccDevAngleTitleID1,
                    ButtonBackgroundStateWatchingProcess.allGood);
                ChangeIndicatingButtonBackgroundColor(lblAccMagnTitleID2,
                    ButtonBackgroundStateWatchingProcess.allGood);
                ChangeIndicatingButtonBackgroundColor(lblAccDevAngleTitleID2,
                    ButtonBackgroundStateWatchingProcess.allGood);
            }
        }





        private void UpdateSensorsDataPresentation(object state)
        {
            // =======================
            //вывести мгновенные значения, но раздельно по устройствам
            // =======================
            // dataToPassToSensorsDataPresentation = new object[] { latestAccDataID1, accCalibrationDataID1, latestAccDataID2, accCalibrationDataID2 };
            if (state == null)
            {
                return;
            }
            if (state.GetType() == typeof(List<AccelerometerData>))
            {
                List<AccelerometerData> dataToPassToSensorsDataPresentation = state as List<AccelerometerData>;
                if (dataToPassToSensorsDataPresentation.Count != 4)
                {
                    return;
                }
            }

            AccelerometerData latestAccDataID1;
            AccelerometerData accCalibrationDataID1;
            AccelerometerData latestAccDataID2;
            AccelerometerData accCalibrationDataID2;

            try
            {
                List<AccelerometerData> dataToPassToSensorsDataPresentation = new List<AccelerometerData>((List<AccelerometerData>)state);
                latestAccDataID1 = dataToPassToSensorsDataPresentation[0] as AccelerometerData;
                accCalibrationDataID1 = dataToPassToSensorsDataPresentation[1] as AccelerometerData;
                latestAccDataID2 = dataToPassToSensorsDataPresentation[2] as AccelerometerData;
                accCalibrationDataID2 = dataToPassToSensorsDataPresentation[3] as AccelerometerData;
            }
            catch (Exception ex)
            {
                return;
            }


            if ((latestAccDataID1 != null) && (accCalibrationDataID1 != null) && (latestAccDataID2 != null) && (accCalibrationDataID2 != null))
            {
                double accDevAngleID1 = (latestAccDataID1 * accCalibrationDataID1) / (latestAccDataID1.AccMagnitude * accCalibrationDataID1.AccMagnitude);
                accDevAngleID1 = Math.Acos(accDevAngleID1) * 180.0d / Math.PI;
                double accDevAngleID2 = (latestAccDataID2 * accCalibrationDataID2) / (latestAccDataID2.AccMagnitude * accCalibrationDataID2.AccMagnitude);
                accDevAngleID2 = Math.Acos(accDevAngleID2) * 180.0d / Math.PI;
                ThreadSafeOperations.SetText(lblAccMagnValueID1, (latestAccDataID1.AccMagnitude / accCalibrationDataID1.AccMagnitude).ToString("F2"), false);
                ThreadSafeOperations.SetText(lblAccDevAngleValueID1, accDevAngleID1.ToString("F3"), false);
                ThreadSafeOperations.SetText(lblAccMagnValueID2, (latestAccDataID2.AccMagnitude / accCalibrationDataID2.AccMagnitude).ToString("F2"), false);
                ThreadSafeOperations.SetText(lblAccDevAngleValueID2, accDevAngleID2.ToString("F3"), false);
            }
        }





        #region обработка поступающих данных сенсоров

        private static int tailLength = 5;
        private FixedTimeQueue<AccelerometerData> accID1tail = new FixedTimeQueue<AccelerometerData>(tailLength);
        private FixedTimeQueue<AccelerometerData> accID2tail = new FixedTimeQueue<AccelerometerData>(tailLength);
        private FixedTimeQueue<GyroData> gyroID1tail = new FixedTimeQueue<GyroData>(tailLength);
        private FixedTimeQueue<GyroData> gyroID2tail = new FixedTimeQueue<GyroData>(tailLength);
        private DenseVector dvKernelFixedWidth = Extensions.ConvKernelAsymmetric(StandardConvolutionKernels.gauss,
            tailLength, false);
        private List<AccelerometerData> dataToPassToSensorsDataPresentation = new List<AccelerometerData>();
        private double accDevAngle = 0.0d;

        private TimeSeries<AccelerometerData> tsCollectedAccData = new TimeSeries<AccelerometerData>();
        //private DenseMatrix dmAccDataMatrix = null;
        //private List<long> accDateTimeValuesList = new List<long>();
        //private DenseMatrix dmAccSmoothedDataMatrixID1 = null;
        //List<long> accSmoothedDateTimeValuesListID1 = new List<long>();
        private TimeSeries<AccelerometerData> tsCollectedAccSmoothedDataID1 = new TimeSeries<AccelerometerData>();
        //DenseMatrix dmAccSmoothedDataMatrixID2 = null;
        //List<long> accSmoothedDateTimeValuesListID2 = new List<long>();
        private TimeSeries<AccelerometerData> tsCollectedAccSmoothedDataID2 = new TimeSeries<AccelerometerData>();

        Stopwatch stwCamshotTimer = new Stopwatch();

        //DenseMatrix dmGyroDataMatrix = null;
        //List<long> gyroDateTimeValuesList = new List<long>();
        private TimeSeries<GyroData> tsCollectedGyroData = new TimeSeries<GyroData>();

        private TimeSeries<GPSdata> tsCollectedGPSdata = new TimeSeries<GPSdata>();

        TimeSeries<int> tsCollectedPressureData = new TimeSeries<int>();

        private void dataCollector_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime datetimeCamshotTimerBegin = DateTime.Now;

            stwCamshotTimer.Start();

            TimeSpan camshotPeriod = CamShotPeriod;
            TimeSpan labelsUpdatingPeriod = new TimeSpan(0, 0, 1);
            Stopwatch stwCamshotTimersUpdating = new Stopwatch();
            stwCamshotTimersUpdating.Start();


            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            ThreadSafeOperations.ToggleButtonState(btnStartStopCollecting, true, "stop collecting data", true);
            dataCollectingState = DataCollectingStates.working;
            bool camshotTimeToGetIt = false;
            bool camshotHasBeenTaken = false;

            

            // DenseMatrix dmGPSDataMatrix = null;
            //List<long> gpsDateTimeValuesList = new List<long>();
            //List<int> pressureValuesList = new List<int>();
            //List<long> pressureDateTimeValuesList = new List<long>();

            Stopwatch stwToEstimateUDPpacketsRecieving = new Stopwatch();
            stwToEstimateUDPpacketsRecieving.Start();




            #region timer for periodical process of UDPpackets Recieving speed estimation

            TimerCallback UDPpacketsRecievingSpeedEstimationCallback =
                new TimerCallback(EstimateAndReportUDPpacketsRecievingSpeed);
            Timer tmrUDPpacketsRecievingSpeedEstimation = new Timer(UDPpacketsRecievingSpeedEstimationCallback,
                new object[] { stwToEstimateUDPpacketsRecieving }, 0, 1000);

            #endregion timer for periodical process of UDPpackets Recieving speed estimation


            #region timer for periodical sensors values presentation updating

            TimerCallback UpdateSensorsDataPresentationCallback =
                new TimerCallback(UpdateSensorsDataPresentation);
            Timer tmrUpdateSensorsDataPresentation = new Timer(UpdateSensorsDataPresentationCallback,
                dataToPassToSensorsDataPresentation, 0, 500);

            #endregion timer for periodical sensors values presentation updating






            while (true)
            {
                if (SelfWorker.CancellationPending)
                {
                    #region dump the rest accelerometers data to nc-file
                    #region // obsolete
                    //if (dmAccDataMatrix != null)
                    //{
                    //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                    //    dataToSave.Add("DateTime", accDateTimeValuesList.ToArray());
                    //    dataToSave.Add("AccelerometerData", dmAccDataMatrix);

                    //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                    //        Directory.GetCurrentDirectory() + "\\logs\\AccelerometerDataLog-" +
                    //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                    //    dmAccDataMatrix = null;
                    //    accDateTimeValuesList.Clear();
                    //}
                    #endregion // obsolete

                    if (tsCollectedAccData.Count > 0)
                    {
                        DumpCollectedAccelerometersData();
                    }
                    #endregion dump the rest accelerometers data to nc-file
                    
                    #region dump the rest smoothed accelerometers data to nc-file

                    if (tsCollectedAccSmoothedDataID1.Count > 0)
                    {
                        DumpCollectedAccelerometersSmoothedDataDevID1();
                    }

                    if (tsCollectedAccSmoothedDataID2.Count > 0)
                    {
                        DumpCollectedAccelerometersSmoothedDataDevID2();
                    }

                    #region // obsolete
                    //if (dmAccSmoothedDataMatrixID1 != null)
                    //{
                    //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                    //    dataToSave.Add("DateTime", accSmoothedDateTimeValuesListID1.ToArray());
                    //    dataToSave.Add("AccelerometerData", dmAccSmoothedDataMatrixID1);

                    //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                    //        Directory.GetCurrentDirectory() + "\\logs\\AccelerometerID1-Smoothed-DataLog-" +
                    //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                    //    dmAccSmoothedDataMatrixID1 = null;
                    //    accSmoothedDateTimeValuesListID1.Clear();
                    //}

                    //if (dmAccSmoothedDataMatrixID2 != null)
                    //{
                    //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                    //    dataToSave.Add("DateTime", accSmoothedDateTimeValuesListID2.ToArray());
                    //    dataToSave.Add("AccelerometerData", dmAccSmoothedDataMatrixID2);

                    //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                    //        Directory.GetCurrentDirectory() + "\\logs\\AccelerometerID2-Smoothed-DataLog-" +
                    //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                    //    dmAccSmoothedDataMatrixID2 = null;
                    //    accSmoothedDateTimeValuesListID2.Clear();
                    //}
                    #endregion // obsolete

                    #endregion dump the rest smoothed accelerometers data to nc-file
                    
                    #region dump the rest gyro data to nc-file

                    if (tsCollectedGyroData.Count > 0)
                    {
                        DumpCollectedGyroData();
                    }

                    #region // obsolete
                    //if (dmGyroDataMatrix != null)
                    //{
                    //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                    //    dataToSave.Add("DateTime", gyroDateTimeValuesList.ToArray());
                    //    dataToSave.Add("GyroscopeData", dmGyroDataMatrix);

                    //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                    //        Directory.GetCurrentDirectory() + "\\logs\\GyroscopeDataLog-" +
                    //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                    //    dmGyroDataMatrix = null;
                    //    gyroDateTimeValuesList.Clear();
                    //}
                    #endregion // obsolete

                    #endregion dump the rest gyro data to nc-file
                    
                    #region dump the rest GPS data to nc-file

                    if (tsCollectedGPSdata.Count > 0)
                    {
                        DumpCollectedGPSdata();
                    }

                    #region // obsolete
                    //if (dmGPSDataMatrix != null)
                    //{
                    //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                    //    dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                    //    dataToSave.Add("GPSdata", dmGPSDataMatrix);

                    //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                    //        Directory.GetCurrentDirectory() + "\\logs\\GPSDataLog-" +
                    //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                    //    dmGPSDataMatrix = null;
                    //    gpsDateTimeValuesList.Clear();
                    //}
                    #endregion // obsolete
                    #endregion dump the rest GPS data to nc-file

                    
                    #region dump the rest pressure data to nc-file
                    if (tsCollectedPressureData.Count > 0)
                    {
                        DumpCollectedPressureData();

                        #region // obsolete

                        //ThreadSafeOperations.SetText(lblPressureValue, pressureValuesList[pressureValuesList.Count - 1].ToString(), false);

                        //Dictionary<string, object> dataToSave = new Dictionary<string, object>();

                        //long[] datetimeDataArray = pressureDateTimeValuesList.ToArray();
                        //dataToSave.Add("DateTime", datetimeDataArray);

                        //int[] pressureArray = pressureValuesList.ToArray();
                        //dataToSave.Add("PressureData", pressureArray);

                        //NetCDFoperations.AddVariousDataToFile(dataToSave, Directory.GetCurrentDirectory() +
                        //    "\\logs\\PressureDataLog-" + DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                        //pressureValuesList.Clear();
                        //pressureDateTimeValuesList.Clear();

                        #endregion // obsolete
                    }
                    #endregion dump the rest pressure data to nc-file

                    break;
                }
                


                #region // обработка очередей разобранных данных - разнесено по событиям CollectionChanged соответствующих коллекций


                #region // accelerometers - вынесено в CollectionChanged событие коллекции accDataAndDTObservableConcurrentQueue
                //if (accDataAndDTObservableConcurrentQueue.Count > 0)
                //{
                //    //accDatahasBeenChanged = false;

                //    Tuple<DateTime, accelerometerData> tplAccDT = accDataAndDTObservableConcurrentQueue.Dequeue();

                //    if (tplAccDT == null) continue;

                //    //accelerometerData accData = accDataQueue.Dequeue();
                //    accelerometerData accData = tplAccDT.Item2;
                //    DateTime dtAccDataRecieved = tplAccDT.Item1;

                //    if (accData.devID <= 1) accID1tail.Enqueue(accData, dtAccDataRecieved);
                //    else accID2tail.Enqueue(accData, dtAccDataRecieved);


                //    accelerometerData accCalibrationData = (accData.devID <= 1)
                //        ? (accCalibrationDataID1)
                //        : (accCalibrationDataID2);


                //    // пересчитать хвост
                //    if (accData.devID <= 1)
                //    {
                //        List<accelerometerData> accDataTail = new List<accelerometerData>(accID1tail.DataValues);
                //        DenseVector dvAccXvaluesTail =
                //            DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleX));
                //        DenseVector dvAccYvaluesTail =
                //            DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleY));
                //        DenseVector dvAccZvaluesTail =
                //            DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleZ));

                //        if (accDataTail.Count < tailLength)
                //        {
                //            dvAccXvaluesTail = DenseVector.OfEnumerable(dvAccXvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                //            dvAccYvaluesTail = DenseVector.OfEnumerable(dvAccYvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                //            dvAccZvaluesTail = DenseVector.OfEnumerable(dvAccZvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                //        }


                //        latestAccDataID1 = new accelerometerData(dvKernelFixedWidth * dvAccXvaluesTail,
                //            dvKernelFixedWidth * dvAccYvaluesTail, dvKernelFixedWidth * dvAccZvaluesTail);
                //        latestAccDataID1.devID = accData.devID;

                //        accDevAngle = (latestAccDataID1 * accCalibrationData) / (latestAccDataID1.AccMagnitude * accCalibrationData.AccMagnitude);
                //        accDevAngle = Math.Acos(accDevAngle);
                //    }
                //    else
                //    {
                //        List<accelerometerData> accDataTail = new List<accelerometerData>(accID2tail.DataValues);
                //        DenseVector dvAccXvaluesTail =
                //            DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleX));
                //        DenseVector dvAccYvaluesTail =
                //            DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleY));
                //        DenseVector dvAccZvaluesTail =
                //            DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleZ));

                //        if (accDataTail.Count < tailLength)
                //        {
                //            dvAccXvaluesTail = DenseVector.OfEnumerable(dvAccXvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                //            dvAccYvaluesTail = DenseVector.OfEnumerable(dvAccYvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                //            dvAccZvaluesTail = DenseVector.OfEnumerable(dvAccZvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                //        }

                //        latestAccDataID2 = new accelerometerData(dvKernelFixedWidth * dvAccXvaluesTail,
                //            dvKernelFixedWidth * dvAccYvaluesTail, dvKernelFixedWidth * dvAccZvaluesTail);
                //        latestAccDataID2.devID = accData.devID;

                //        accDevAngle = (latestAccDataID2 * accCalibrationData) / (latestAccDataID2.AccMagnitude * accCalibrationData.AccMagnitude);
                //        accDevAngle = Math.Acos(accDevAngle);
                //    }

                //    #region // obsolete
                //    // сформировано наиболее актуальное значение accData для соответствующего устройства
                //    //if (accData.devID <= 1)
                //    //{
                //    //    latestAccDataID1 = accData.Copy();
                //    //}
                //    //else if (accData.devID == 2)
                //    //{
                //    //    latestAccDataID2 = accData.Copy();
                //    //}


                //    // accDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                //    #endregion // obsolete

                //    accDateTimeValuesList.Add(dtAccDataRecieved.Ticks);




                //    #region adding latest recieved acc data to backuping datamatrix
                //    if (dmAccDataMatrix == null)
                //    {
                //        dmAccDataMatrix = DenseMatrix.Create(1, 8, 0.0d);
                //        DenseVector dvInitialData = (DenseVector)accData.ToDenseVector().SubVector(0, 3); // x,y,z
                //        DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                //        dvInitialData =
                //            DenseVector.OfEnumerable(
                //                dvInitialData.Concat(dvCalibratedData)
                //                    .Concat(new double[] { accDevAngle, accData.devID }));
                //        dmAccDataMatrix.InsertRow(0, dvInitialData);

                //        #region // obsolete
                //        //dmAccDataMatrix = DenseMatrix.Create(1, 8, (r, c) =>
                //        //{
                //        //    switch (c)
                //        //    {
                //        //        case 0:
                //        //            return accData.AccDoubleX;
                //        //            break;
                //        //        case 1:
                //        //            return accData.AccDoubleY;
                //        //            break;
                //        //        case 2:
                //        //            return accData.AccDoubleZ;
                //        //            break;
                //        //        case 3:
                //        //            return accCalibrationData.AccDoubleX;
                //        //            break;
                //        //        case 4:
                //        //            return accCalibrationData.AccDoubleY;
                //        //            break;
                //        //        case 5:
                //        //            return accCalibrationData.AccDoubleZ;
                //        //            break;
                //        //        case 6:
                //        //            {
                //        //                //угол отклонения от калибровочного вектора
                //        //                // В РАДИАНАХ
                //        //                return accDevAngle;
                //        //                break;
                //        //            }
                //        //        case 7:
                //        //            {
                //        //                // devID
                //        //                return accData.devID;
                //        //                break;
                //        //            }
                //        //        default:
                //        //            break;
                //        //    }
                //        //    return 0;
                //        //});
                //        #endregion // obsolete
                //    }
                //    else
                //    {
                //        DenseVector dvRowToAdd = (DenseVector)accData.ToDenseVector().SubVector(0, 3); // x,y,z
                //        DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                //        dvRowToAdd =
                //            DenseVector.OfEnumerable(
                //                dvRowToAdd.Concat(dvCalibratedData)
                //                    .Concat(new double[] { accDevAngle, accData.devID }));

                //        #region // obsolete
                //        //DenseVector dvAccDataVectorToAdd = DenseVector.Create(8, c =>
                //        //{
                //        //    switch (c)
                //        //    {
                //        //        case 0:
                //        //            return accData.AccDoubleX;
                //        //            break;
                //        //        case 1:
                //        //            return accData.AccDoubleY;
                //        //            break;
                //        //        case 2:
                //        //            return accData.AccDoubleZ;
                //        //            break;
                //        //        case 3:
                //        //            return accCalibrationData.AccDoubleX;
                //        //            break;
                //        //        case 4:
                //        //            return accCalibrationData.AccDoubleY;
                //        //            break;
                //        //        case 5:
                //        //            return accCalibrationData.AccDoubleZ;
                //        //            break;
                //        //        case 6:
                //        //            {
                //        //                //угол отклонения от калибровочного вектора
                //        //                // В РАДИАНАХ
                //        //                return accDevAngle;
                //        //                break;
                //        //            }
                //        //        case 7:
                //        //            {
                //        //                // devID
                //        //                return accData.devID;
                //        //                break;
                //        //            }
                //        //        default:
                //        //            break;
                //        //    }
                //        //    return 0;
                //        //});
                //        #endregion // obsolete

                //        dmAccDataMatrix =
                //            (DenseMatrix)dmAccDataMatrix.InsertRow(dmAccDataMatrix.RowCount, dvRowToAdd);
                //    }
                //    #endregion adding latest recieved acc data to backuping datamatrix


                //    #region adding latest recieved smoothed acc data to backuping datamatrix

                //    //accelerometerData latestAccDataIDx = null;
                //    if (accData.devID <= 1)
                //    {
                //        accSmoothedDateTimeValuesListID1.Add(dtAccDataRecieved.Ticks);

                //        if (dmAccSmoothedDataMatrixID1 == null)
                //        {
                //            dmAccSmoothedDataMatrixID1 = DenseMatrix.Create(1, 8, 0.0d);
                //            DenseVector dvInitialData = (DenseVector)latestAccDataID1.ToDenseVector().SubVector(0, 3); // x,y,z
                //            DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                //            dvInitialData =
                //                DenseVector.OfEnumerable(
                //                    dvInitialData.Concat(dvCalibratedData)
                //                        .Concat(new double[] { accDevAngle, latestAccDataID1.devID }));
                //            dmAccSmoothedDataMatrixID1.InsertRow(0, dvInitialData);
                //        }
                //        else
                //        {
                //            DenseVector dvRowToAdd = (DenseVector)latestAccDataID1.ToDenseVector().SubVector(0, 3); // x,y,z
                //            DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                //            dvRowToAdd =
                //                DenseVector.OfEnumerable(
                //                    dvRowToAdd.Concat(dvCalibratedData)
                //                        .Concat(new double[] { accDevAngle, latestAccDataID1.devID }));

                //            dmAccSmoothedDataMatrixID1 =
                //                (DenseMatrix)dmAccSmoothedDataMatrixID1.InsertRow(dmAccSmoothedDataMatrixID1.RowCount, dvRowToAdd);
                //        }
                //    }
                //    else
                //    {
                //        accSmoothedDateTimeValuesListID2.Add(dtAccDataRecieved.Ticks);
                //        if (dmAccSmoothedDataMatrixID2 == null)
                //        {
                //            dmAccSmoothedDataMatrixID2 = DenseMatrix.Create(1, 8, 0.0d);
                //            DenseVector dvInitialData = (DenseVector)latestAccDataID2.ToDenseVector().SubVector(0, 3); // x,y,z
                //            DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                //            dvInitialData =
                //                DenseVector.OfEnumerable(
                //                    dvInitialData.Concat(dvCalibratedData)
                //                        .Concat(new double[] { accDevAngle, latestAccDataID2.devID }));
                //            dmAccSmoothedDataMatrixID2.InsertRow(0, dvInitialData);
                //        }
                //        else
                //        {
                //            DenseVector dvRowToAdd = (DenseVector)latestAccDataID2.ToDenseVector().SubVector(0, 3); // x,y,z
                //            DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                //            dvRowToAdd =
                //                DenseVector.OfEnumerable(
                //                    dvRowToAdd.Concat(dvCalibratedData)
                //                        .Concat(new double[] { accDevAngle, latestAccDataID2.devID }));

                //            dmAccSmoothedDataMatrixID2 =
                //                (DenseMatrix)dmAccSmoothedDataMatrixID2.InsertRow(dmAccSmoothedDataMatrixID2.RowCount, dvRowToAdd);
                //        }
                //    }

                //    #endregion adding latest recieved smoothed acc data to backuping datamatrix

                //    dataToPassToSensorsDataPresentation.Clear();
                //    dataToPassToSensorsDataPresentation.AddRange(new accelerometerData[] { latestAccDataID1, accCalibrationDataID1, latestAccDataID2, accCalibrationDataID2 });

                //    #region // obsolete - moved to timer callback mech
                //    //ThreadSafeOperations.SetText(lblGotAccDataPackCounterValue, accDateTimeValuesList.Count.ToString(), false);

                //    //if (stwShowActualAccData.ElapsedMilliseconds >= 500)
                //    //{
                //    //    stwShowActualAccData.Restart();
                //    //    #region // magnetometer - obsolete
                //    //    //DenseVector dvMagnDev = DenseVector.Create(dmAccDataMatrix.RowCount, i =>
                //    //    //{
                //    //    //    accelerometerData accCurrentData = new accelerometerData(dmAccDataMatrix[0, 0],
                //    //    //        dmAccDataMatrix[0, 1], dmAccDataMatrix[0, 2]);
                //    //    //    return (accCurrentData.AccMagnitude - accCalibrationData.AccMagnitude) *
                //    //    //           (accCurrentData.AccMagnitude - accCalibrationData.AccMagnitude);
                //    //    //});
                //    //    //ThreadSafeOperations.SetText(lblAccDevMeanMagnValueID1,
                //    //    //    Math.Sqrt(dvMagnDev.Sum() / dvMagnDev.Count).ToString("0.###e-00"), false);


                //    //    //DescriptiveStatistics stats1 = new DescriptiveStatistics(dmAccDataMatrix.Column(6));
                //    //    //ThreadSafeOperations.SetText(lblAccDevMeanAngleValueID1, stats1.Mean.ToString("0.###e-00"), false);
                //    //    #endregion #region // magnetometer - obsolete

                //    //    // =======================
                //    //    //вывести мгновенные значения, но раздельно по устройствам
                //    //    // =======================
                //    //    double accDevAngleID1 = (latestAccDataID1 * accCalibrationDataID1) / (latestAccDataID1.AccMagnitude * accCalibrationDataID1.AccMagnitude);
                //    //    accDevAngleID1 = Math.Acos(accDevAngleID1) * 180.0d / Math.PI;
                //    //    double accDevAngleID2 = (latestAccDataID2 * accCalibrationDataID2) / (latestAccDataID2.AccMagnitude * accCalibrationDataID2.AccMagnitude);
                //    //    accDevAngleID2 = Math.Acos(accDevAngleID2) * 180.0d / Math.PI;
                //    //    ThreadSafeOperations.SetText(lblAccMagnValueID1, (latestAccDataID1.AccMagnitude / accCalibrationDataID1.AccMagnitude).ToString("F2"), false);
                //    //    ThreadSafeOperations.SetText(lblAccDevAngleValueID1, accDevAngleID1.ToString("F3"), false);
                //    //    ThreadSafeOperations.SetText(lblAccMagnValueID2, (latestAccDataID2.AccMagnitude / accCalibrationDataID2.AccMagnitude).ToString("F2"), false);
                //    //    ThreadSafeOperations.SetText(lblAccDevAngleValueID2, accDevAngleID2.ToString("F3"), false);
                //    //}
                //    #endregion // obsolete - moved to timer callback mech


                //    #region swap acc data to hdd
                //    if (accDateTimeValuesList.Count >= 1000)
                //    {
                //        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //        dataToSave.Add("DateTime", accDateTimeValuesList.ToArray());
                //        dataToSave.Add("AccelerometerData", dmAccDataMatrix);

                //        NetCDFoperations.AddVariousDataToFile(dataToSave,
                //            Directory.GetCurrentDirectory() + "\\logs\\AccelerometerDataLog-" +
                //            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");




                //        dmAccDataMatrix = null;
                //        accDateTimeValuesList.Clear();
                //    }


                //    if (accSmoothedDateTimeValuesListID1.Count >= 500)
                //    {
                //        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //        dataToSave.Add("DateTime", accSmoothedDateTimeValuesListID1.ToArray());
                //        dataToSave.Add("AccelerometerData", dmAccSmoothedDataMatrixID1);

                //        NetCDFoperations.AddVariousDataToFile(dataToSave,
                //            Directory.GetCurrentDirectory() + "\\logs\\AccelerometerID1-Smoothed-DataLog-" +
                //            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");


                //        dmAccSmoothedDataMatrixID1 = null;
                //        accSmoothedDateTimeValuesListID1.Clear();
                //    }


                //    if (accSmoothedDateTimeValuesListID2.Count >= 500)
                //    {
                //        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //        dataToSave.Add("DateTime", accSmoothedDateTimeValuesListID2.ToArray());
                //        dataToSave.Add("AccelerometerData", dmAccSmoothedDataMatrixID2);

                //        NetCDFoperations.AddVariousDataToFile(dataToSave,
                //            Directory.GetCurrentDirectory() + "\\logs\\AccelerometerID2-Smoothed-DataLog-" +
                //            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");


                //        dmAccSmoothedDataMatrixID2 = null;
                //        accSmoothedDateTimeValuesListID2.Clear();
                //    }
                //    #endregion swap acc data to hdd

                //    processedUDPPacketsCounter++;
                //}
                #endregion accelerometers
                
                #region // gyroscope - вынесено в CollectionChanged событие коллекции gyroDataAndDTObservableConcurrentQueue
                //if (gyroDataQueue.Count > 0)
                //if (gyroDataAndDTObservableConcurrentQueue.Count > 0)
                //{
                //    //gyroDatahasBeenChanged = false;

                //    Tuple<DateTime, GyroData> tplGyroDT = gyroDataAndDTObservableConcurrentQueue.Dequeue();

                //    if (tplGyroDT == null) continue;

                //    //accelerometerData accData = accDataQueue.Dequeue();
                //    GyroData gyroData = tplGyroDT.Item2;
                //    DateTime dtGyroDataRecieved = tplGyroDT.Item1;

                //    //GyroData gyroData = gyroDataQueue.Dequeue();
                //    //if (gyroData == null)
                //    //    continue;

                //    gyroDateTimeValuesList.Add(dtGyroDataRecieved.Ticks);

                //    if (dmGyroDataMatrix == null)
                //    {
                //        dmGyroDataMatrix = gyroData.ToOneRowDenseMatrix();
                //    }
                //    else
                //    {
                //        DenseVector dvGyroDataVectorToAdd = gyroData.ToDenseVector();

                //        dmGyroDataMatrix =
                //            (DenseMatrix)dmGyroDataMatrix.InsertRow(dmGyroDataMatrix.RowCount, dvGyroDataVectorToAdd);
                //    }

                //    if (gyroDateTimeValuesList.Count >= 500)
                //    {
                //        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //        dataToSave.Add("DateTime", gyroDateTimeValuesList.ToArray());
                //        dataToSave.Add("GyroscopeData", dmGyroDataMatrix);

                //        NetCDFoperations.AddVariousDataToFile(dataToSave,
                //            Directory.GetCurrentDirectory() + "\\logs\\GyroscopeDataLog-" +
                //            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");


                //        dmGyroDataMatrix = null;
                //        gyroDateTimeValuesList.Clear();
                //    }

                //    processedUDPPacketsCounter++;
                //}
                #endregion // gyroscope - вынесено в CollectionChanged событие коллекции gyroDataAndDTObservableConcurrentQueue
                
                #region // GPS from Arduino stream - перенесено в CollectionChanged событие коллекции gpsDataAndDTObservableConcurrentQueue
                //if (gpsDataHasBeenChanged)
                //{
                //    #region //obsolete
                //    //SPA spaCalc = new SPA(gpsData.dateTimeUTC.Year, gpsData.dateTimeUTC.Month, gpsData.dateTimeUTC.Day, gpsData.dateTimeUTC.Hour,
                //    //    gpsData.dateTimeUTC.Minute, gpsData.dateTimeUTC.Second, (float)gpsData.LonDec, (float)gpsData.LatDec,
                //    //    (float)SPAConst.DeltaT(gpsData.dateTimeUTC));
                //    //int res = spaCalc.spa_calculate();
                //    #endregion //obsolete
                //    AzimuthZenithAngle sunPositionSPAext = gpsData.SunZenithAzimuth();
                //    double sunElevCalc = sunPositionSPAext.ElevationAngle;
                //    double sunAzimuth = sunPositionSPAext.Azimuth;


                //    ThreadSafeOperations.SetText(lblLatValue, gpsData.lat.ToString("F2") + gpsData.latHemisphere, false);
                //    ThreadSafeOperations.SetText(lblLonValue, gpsData.lon.ToString("F2") + gpsData.lonHemisphere, false);
                //    ThreadSafeOperations.SetText(lblUTCTimeValue, gpsData.dateTimeUTC.ToString("u").Replace(" ", Environment.NewLine).Replace("Z", ""), false);
                //    ThreadSafeOperations.SetText(lblSunElev, sunElevCalc.ToString("F2"), false);
                //    ThreadSafeOperations.SetText(lblSunAzimuth, sunAzimuth.ToString("F2"), false);

                //    gpsDataHasBeenChanged = false;

                //    gpsDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                //    if (dmGPSDataMatrix == null)
                //    {
                //        dmGPSDataMatrix = gpsData.ToOneRowDenseMatrix();
                //    }
                //    else
                //    {
                //        DenseVector dvGPSDataVectorToAdd = gpsData.ToDenseVector();

                //        dmGPSDataMatrix =
                //            (DenseMatrix)dmGPSDataMatrix.InsertRow(dmGPSDataMatrix.RowCount, dvGPSDataVectorToAdd);
                //    }

                //    if (gpsDateTimeValuesList.Count >= 50)
                //    {
                //        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //        dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                //        dataToSave.Add("GPSdata", dmGPSDataMatrix);

                //        NetCDFoperations.AddVariousDataToFile(dataToSave,
                //            Directory.GetCurrentDirectory() + "\\logs\\GPSDataLog-" +
                //            DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                //        dmGPSDataMatrix = null;
                //        gpsDateTimeValuesList.Clear();
                //    }

                //    processedUDPPacketsCounter++;
                //}
                #endregion // GPS from Arduino stream - перенесено в CollectionChanged событие коллекции gpsDataAndDTObservableConcurrentQueue
                
                #region // pressure data from Arduino stream - перенесено в CollectionChanged событие коллекции pressureDataAndDTObservableConcurrentQueue
                //if (pressureHasBeenChanged)
                //{
                //    pressureDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                //    pressureValuesList.Add(pressure);

                //    if (pressureDateTimeValuesList.Count >= 10)
                //    {
                //        ThreadSafeOperations.SetText(lblPressureValue, pressure.ToString(), false);

                //        Dictionary<string, object> dataToSave = new Dictionary<string, object>();

                //        long[] datetimeDataArray = pressureDateTimeValuesList.ToArray();
                //        dataToSave.Add("DateTime", datetimeDataArray);

                //        int[] pressureArray = pressureValuesList.ToArray();
                //        dataToSave.Add("PressureData", pressureArray);

                //        NetCDFoperations.AddVariousDataToFile(dataToSave, Directory.GetCurrentDirectory() +
                //            "\\logs\\PressureDataLog-" + DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                //        pressureValuesList.Clear();
                //        pressureDateTimeValuesList.Clear();
                //    }
                //    pressureHasBeenChanged = false;

                //    processedUDPPacketsCounter++;
                //}
                #endregion  // pressure data from Arduino stream - перенесено в CollectionChanged событие коллекции pressureDataAndDTObservableConcurrentQueue

                #endregion // обработка очередей разобранных данных - разнесено по событиям CollectionChanged соответствующих коллекций

                

                ///
                ///todo: перенести в обработку данных по акселлерометру
                ///  
                #region if it is time to shoot - then shoot

                
                if (stwCamshotTimer.Elapsed >= camshotPeriod)
                {
                    camshotTimeToGetIt = true;
                    ThreadSafeOperations.ToggleLabelTextColor(lblNextShotIn, SystemColors.Highlight);
                    stwCamshotTimer.Restart();
                }
                if (operatorCommandsToGetCamShot)
                {
                    camshotTimeToGetIt = true;
                    ThreadSafeOperations.ToggleLabelTextColor(lblNextShotIn, SystemColors.Highlight);
                }


                bool camshotInclinedProperly = !(accDevAngle > angleCamDeclinationThresholdRad);

                //if ((camshotInclinedProperly && camshotTimeToGetIt) || operatorCommandsToGetCamShotImmediately)
                if (camshotInclinedProperly && camshotTimeToGetIt)
                {
                    EventHandler onNeedToShootCameraSnapshots = this.NeedToShootCameraSnapshots;
                    if (onNeedToShootCameraSnapshots != null) onNeedToShootCameraSnapshots(null, null);

                    //catchCameraImages();
                    camshotHasBeenTaken = true;
                }

                #endregion if it is time to shoot - then shoot



                if (camshotHasBeenTaken)
                {
                    Note("Got a shot " + DateTime.Now);
                    camshotInclinedProperly = false;
                    camshotHasBeenTaken = false;
                    camshotTimeToGetIt = false;
                    ThreadSafeOperations.ToggleLabelTextColor(lblNextShotIn, SystemColors.ControlText);
                    operatorCommandsToGetCamShot = false;
                    //operatorCommandsToGetCamShotImmediately = false;


                    // ПОМЕНЯТЬ ТУТ
                    logCurrentSensorsData();
                }


                if (stwCamshotTimersUpdating.Elapsed >= labelsUpdatingPeriod)
                {
                    updateTimersLabels(stwCamshotTimer, datetimeCamshotTimerBegin, camshotPeriod);
                    stwCamshotTimersUpdating.Restart();
                }
            }

            tmrUDPpacketsRecievingSpeedEstimation.Change(Timeout.Infinite, Timeout.Infinite);
            tmrUDPpacketsRecievingSpeedEstimation.Dispose();
            tmrUpdateSensorsDataPresentation.Change(Timeout.Infinite, Timeout.Infinite);
            tmrUpdateSensorsDataPresentation.Dispose();
            stwCamshotTimer.Stop();
        }




        private void updateTimersLabels(Stopwatch stwCamshotTimer, DateTime datetimePreviousTimerBegin, TimeSpan camshotPeriod)
        {
            TimeSpan nextShotIn = camshotPeriod - stwCamshotTimer.Elapsed; //(nextshotAt - datetimeNow);
            ThreadSafeOperations.SetText(lblNextShotIn, nextShotIn.RoundToSeconds().ToString("c"), false);
            ThreadSafeOperations.SetText(lblSinceLastShot, stwCamshotTimer.Elapsed.RoundToSeconds().ToString("c"), false);
        }




        #region Pressure data processing



        async void pressureDataAndDTObservableConcurrentQueue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
                // чтобы не реагировать на собственные TryDequeue()
            }

            while (pressureDataAndDTObservableConcurrentQueue.Count > 0)
            {
                Tuple<DateTime, int> tplPressureDT;
                while (!pressureDataAndDTObservableConcurrentQueue.TryDequeue(out tplPressureDT))
                {
                    Thread.Sleep(0);
                    continue;
                }

                if (tplPressureDT == null) continue;


                int pressure = tplPressureDT.Item2;
                DateTime dtPressureDataRecieved = tplPressureDT.Item1;

                ThreadSafeOperations.SetText(lblPressureValue, dtPressureDataRecieved.ToString(), false);

                while (!Monitor.TryEnter(tsCollectedPressureData))
                {
                    Thread.Sleep(0);
                }
                try
                {
                    tsCollectedPressureData.AddDataRecord(pressure, dtPressureDataRecieved);
                }
                finally
                {
                    Monitor.Exit(tsCollectedPressureData);
                }


                #region swap pressure data to hdd

                Task tskDumpCollectedPressuredata = null;

                if (tsCollectedPressureData.Count >= 10)
                {
                    tskDumpCollectedPressuredata = Task.Factory.StartNew(DumpCollectedPressureData);
                }


                if (tskDumpCollectedPressuredata != null)
                {
                    await tskDumpCollectedPressuredata;
                }

                #endregion swap pressure data to hdd

                Interlocked.Increment(ref processedUDPPacketsCounter);
            }
        }




        private void DumpCollectedPressureData()
        {
            if (tsCollectedPressureData.Count == 0)
            {
                return;
            }

            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedPressureData))
            {
                Thread.Sleep(0);
            }

            try
            {
                tsCollectedPressureData.SortByTimeStamps();
                dataToSave.Add("DateTime", tsCollectedPressureData.TimeStamps.ConvertAll(dt => dt.Ticks).ToArray());
                int[] pressureArray = tsCollectedPressureData.DataValues.ToArray();
                dataToSave.Add("PressureData", pressureArray);
                tsCollectedPressureData.Clear();
            }
            catch (Exception ex)
            {
                #region report
#if DEBUG
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription());
                });
#else
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif
                #endregion report
            }
            finally
            {
                Monitor.Exit(tsCollectedPressureData);
            }

            #endregion unload shared collections data to dataToSave


            NetCDFoperations.AddVariousDataToFile(dataToSave,
                Directory.GetCurrentDirectory() + "\\logs\\PressureDataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
        }



        #endregion Pressure data processing




        #region GPS data processing
        

        async void gpsDataAndDTObservableConcurrentQueue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
                // чтобы не реагировать на собственные TryDequeue()
            }

            while (gpsDataAndDTObservableConcurrentQueue.Count > 0)
            {
                Tuple<DateTime, GPSdata> tplGPSdt;
                while (!gpsDataAndDTObservableConcurrentQueue.TryDequeue(out tplGPSdt))
                {
                    Thread.Sleep(0);
                    continue;
                }

                if (tplGPSdt == null) continue;


                GPSdata gpsData = tplGPSdt.Item2;
                DateTime dtGPSdataRecieved = tplGPSdt.Item1;


                AzimuthZenithAngle sunPositionSPAext = gpsData.SunZenithAzimuth();
                double sunElevCalc = sunPositionSPAext.ElevationAngle;
                double sunAzimuth = sunPositionSPAext.Azimuth;


                ThreadSafeOperations.SetText(lblLatValue, gpsData.lat.ToString("F2") + gpsData.latHemisphere, false);
                ThreadSafeOperations.SetText(lblLonValue, gpsData.lon.ToString("F2") + gpsData.lonHemisphere, false);
                ThreadSafeOperations.SetText(lblUTCTimeValue, gpsData.dateTimeUTC.ToString("u").Replace(" ", Environment.NewLine).Replace("Z", ""), false);
                ThreadSafeOperations.SetText(lblSunElev, sunElevCalc.ToString("F2"), false);
                ThreadSafeOperations.SetText(lblSunAzimuth, sunAzimuth.ToString("F2"), false);


                while (!Monitor.TryEnter(tsCollectedGPSdata))
                {
                    Thread.Sleep(0);
                }
                try
                {
                    tsCollectedGPSdata.AddDataRecord(gpsData, dtGPSdataRecieved);
                }
                finally
                {
                    Monitor.Exit(tsCollectedGPSdata);
                }

                #region // obsolete
                //gpsDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                //if (dmGPSDataMatrix == null)
                //{
                //    dmGPSDataMatrix = gpsData.ToOneRowDenseMatrix();
                //}
                //else
                //{
                //    DenseVector dvGPSDataVectorToAdd = gpsData.ToDenseVector();

                //    dmGPSDataMatrix =
                //        (DenseMatrix)dmGPSDataMatrix.InsertRow(dmGPSDataMatrix.RowCount, dvGPSDataVectorToAdd);
                //}

                //if (gpsDateTimeValuesList.Count >= 50)
                //{
                //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //    dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                //    dataToSave.Add("GPSdata", dmGPSDataMatrix);

                //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                //        Directory.GetCurrentDirectory() + "\\logs\\GPSDataLog-" +
                //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

                //    dmGPSDataMatrix = null;
                //    gpsDateTimeValuesList.Clear();
                //}
                #endregion // obsolete

                #region swap GPS data to hdd
                
                Task tskDumpCollectedGPSdata = null;

                if (tsCollectedGPSdata.Count >= 50)
                {
                    tskDumpCollectedGPSdata = Task.Factory.StartNew(DumpCollectedGPSdata);
                }


                if (tskDumpCollectedGPSdata != null)
                {
                    await tskDumpCollectedGPSdata;
                }

                #endregion swap GPS data to hdd

                Interlocked.Increment(ref processedUDPPacketsCounter);
            }
        }




        private void DumpCollectedGPSdata()
        {
            if (tsCollectedGPSdata.Count == 0)
            {
                return;
            }

            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedGPSdata))
            {
                Thread.Sleep(0);
            }

            try
            {
                tsCollectedGPSdata.SortByTimeStamps();
                dataToSave.Add("DateTime", tsCollectedGPSdata.TimeStamps.ConvertAll(dt => dt.Ticks).ToArray());
                DenseMatrix dmGPSdata = GPSdata.ToDenseMatrix(tsCollectedGPSdata.DataValues);
                dataToSave.Add("GPSdata", dmGPSdata);
                tsCollectedGPSdata.Clear();
            }
            catch (Exception ex)
            {
                #region report
#if DEBUG
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription());
                });
#else
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif
                #endregion report
            }
            finally
            {
                Monitor.Exit(tsCollectedGPSdata);
            }

            #endregion unload shared collections data to dataToSave


            NetCDFoperations.AddVariousDataToFile(dataToSave,
                Directory.GetCurrentDirectory() + "\\logs\\GPSDataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
        }



        #endregion





        #region Gyro data processing
        //tsCollectedGyroData
        //gyroDataAndDTqueue

        async void gyroDataAndDTObservableConcurrentQueue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
                // чтобы не реагировать на собственные TryDequeue()
            }

            while (gyroDataAndDTObservableConcurrentQueue.Count > 0)
            {
                Tuple<DateTime, GyroData> tplGyroDT;
                while (!gyroDataAndDTObservableConcurrentQueue.TryDequeue(out tplGyroDT))
                {
                    Thread.Sleep(0);
                    continue;
                }

                if (tplGyroDT == null) continue;
                
                
                GyroData gyroData = tplGyroDT.Item2;
                DateTime dtGyroDataRecieved = tplGyroDT.Item1;


                while (!Monitor.TryEnter(tsCollectedGyroData))
                {
                    Thread.Sleep(0);
                }
                try
                {
                    tsCollectedGyroData.AddDataRecord(gyroData, dtGyroDataRecieved);
                }
                finally
                {
                    Monitor.Exit(tsCollectedGyroData);
                }


                #region swap gyro data to hdd
                
                //Task tskUpdateAccDataOnForm = Task.Factory.StartNew(UpdateActualAccDataOnForm);

                Task tskDumpCollectedGyroData = null;
                
                if (tsCollectedGyroData.Count >= 1000)
                {
                    tskDumpCollectedGyroData = Task.Factory.StartNew(DumpCollectedGyroData);
                }


                if (tskDumpCollectedGyroData != null)
                {
                    await tskDumpCollectedGyroData;
                }

                #endregion swap gyro data to hdd


                Interlocked.Increment(ref processedUDPPacketsCounter);
            }
        }



        private void DumpCollectedGyroData()
        {
            if (tsCollectedGyroData.Count == 0)
            {
                return;
            }

            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedGyroData))
            {
                Thread.Sleep(0);
            }

            try
            {
                tsCollectedGyroData.SortByTimeStamps();
                dataToSave.Add("DateTime", tsCollectedGyroData.TimeStamps.ConvertAll(dt => dt.Ticks).ToArray());
                DenseMatrix dmGyroData = GyroData.ToDenseMatrix(tsCollectedGyroData.DataValues);
                dataToSave.Add("GyroscopeData", dmGyroData);
                tsCollectedGyroData.Clear();
            }
            catch (Exception ex)
            {
                #region report
#if DEBUG
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription());
                });
#else
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif
                #endregion report
            }
            finally
            {
                Monitor.Exit(tsCollectedGyroData);
            }

            #endregion unload shared collections data to dataToSave


            NetCDFoperations.AddVariousDataToFile(dataToSave,
                Directory.GetCurrentDirectory() + "\\logs\\GyroscopeDataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

        }


        #endregion Gyro data processing




        #region Accelerometers data processing

        private Dictionary<string, AccelerometerData> dctCalibrationAccDataByDevID =
            new Dictionary<string, AccelerometerData>();

        async void accDataAndDTObservableConcurrentQueue_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AccelerometerData latestAccDataID1 = new AccelerometerData();
            latestAccDataID1.devID = 1;
            AccelerometerData latestAccDataID2 = new AccelerometerData();
            latestAccDataID2.devID = 2;

            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
                // чтобы не реагировать на собственные TryDequeue()
            }


            while (accDataAndDTObservableConcurrentQueue.Count > 0)
            {
                Tuple<DateTime, AccelerometerData> tplAccDT;
                while (!accDataAndDTObservableConcurrentQueue.TryDequeue(out tplAccDT))
                {
                    Thread.Sleep(0);
                    continue;
                }

                if (tplAccDT == null) continue;

                AccelerometerData accData = tplAccDT.Item2;
                DateTime dtAccDataRecieved = tplAccDT.Item1;

                if (accData.devID <= 1) accID1tail.Enqueue(accData, dtAccDataRecieved);
                else accID2tail.Enqueue(accData, dtAccDataRecieved);


                //AccelerometerData accCalibrationData = (accData.devID <= 1)
                //    ? (accCalibrationDataID1)
                //    : (accCalibrationDataID2);


                // пересчитать хвост
                if (accData.devID <= 1)
                {
                    List<AccelerometerData> accDataTail = new List<AccelerometerData>(accID1tail.DataValues);
                    DenseVector dvAccXvaluesTail =
                        DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleX));
                    DenseVector dvAccYvaluesTail =
                        DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleY));
                    DenseVector dvAccZvaluesTail =
                        DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleZ));

                    if (accDataTail.Count < tailLength)
                    {
                        dvAccXvaluesTail = DenseVector.OfEnumerable(dvAccXvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                        dvAccYvaluesTail = DenseVector.OfEnumerable(dvAccYvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                        dvAccZvaluesTail = DenseVector.OfEnumerable(dvAccZvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                    }


                    latestAccDataID1 = new AccelerometerData(dvKernelFixedWidth*dvAccXvaluesTail,
                        dvKernelFixedWidth*dvAccYvaluesTail, dvKernelFixedWidth*dvAccZvaluesTail)
                    {
                        devID = accData.devID
                    };

                    //accDevAngle = (latestAccDataID1 * accCalibrationData) / (latestAccDataID1.AccMagnitude * accCalibrationData.AccMagnitude);
                    //accDevAngle = Math.Acos(accDevAngle);
                }
                else
                {
                    List<AccelerometerData> accDataTail = new List<AccelerometerData>(accID2tail.DataValues);
                    DenseVector dvAccXvaluesTail =
                        DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleX));
                    DenseVector dvAccYvaluesTail =
                        DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleY));
                    DenseVector dvAccZvaluesTail =
                        DenseVector.OfEnumerable(accDataTail.ConvertAll(accDt => accDt.AccDoubleZ));

                    if (accDataTail.Count < tailLength)
                    {
                        dvAccXvaluesTail = DenseVector.OfEnumerable(dvAccXvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                        dvAccYvaluesTail = DenseVector.OfEnumerable(dvAccYvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                        dvAccZvaluesTail = DenseVector.OfEnumerable(dvAccZvaluesTail.Concat(DenseVector.Create(tailLength - accDataTail.Count, 0.0d)));
                    }

                    latestAccDataID2 = new AccelerometerData(dvKernelFixedWidth*dvAccXvaluesTail,
                        dvKernelFixedWidth*dvAccYvaluesTail, dvKernelFixedWidth*dvAccZvaluesTail)
                    {
                        devID = accData.devID
                    };

                    //accDevAngle = (latestAccDataID2 * accCalibrationData) / (latestAccDataID2.AccMagnitude * accCalibrationData.AccMagnitude);
                    //accDevAngle = Math.Acos(accDevAngle);
                }



                #region adding latest recieved acc data to backuping timeseries

                while (!Monitor.TryEnter(tsCollectedAccData))
                {
                    Thread.Sleep(0);
                }
                try
                {
                    tsCollectedAccData.AddDataRecord(accData, dtAccDataRecieved);
                }
                finally
                {
                    Monitor.Exit(tsCollectedAccData);
                }

                #region // obsolete
                //if (dmAccDataMatrix == null)
                //{
                //    dmAccDataMatrix = DenseMatrix.Create(1, 8, 0.0d);
                //    DenseVector dvInitialData = (DenseVector)accData.ToDenseVector().SubVector(0, 3); // x,y,z
                //    DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                //    dvInitialData =
                //        DenseVector.OfEnumerable(
                //            dvInitialData.Concat(dvCalibratedData)
                //                .Concat(new double[] { accDevAngle, accData.devID }));
                //    dmAccDataMatrix.InsertRow(0, dvInitialData);

                //}
                //else
                //{
                //    DenseVector dvRowToAdd = (DenseVector)accData.ToDenseVector().SubVector(0, 3); // x,y,z
                //    DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                //    dvRowToAdd =
                //        DenseVector.OfEnumerable(
                //            dvRowToAdd.Concat(dvCalibratedData)
                //                .Concat(new double[] { accDevAngle, accData.devID }));

                //    dmAccDataMatrix =
                //        (DenseMatrix)dmAccDataMatrix.InsertRow(dmAccDataMatrix.RowCount, dvRowToAdd);
                //}
                #endregion // obsolete

                #endregion adding latest recieved acc data to backuping timeseries


                #region adding latest recieved smoothed acc data to backuping timeseries

                //accelerometerData latestAccDataIDx = null;
                if (accData.devID <= 1)
                {
                    while (!Monitor.TryEnter(tsCollectedAccSmoothedDataID1))
                    {
                        Thread.Sleep(0);
                    }
                    try
                    {
                        tsCollectedAccSmoothedDataID1.AddDataRecord(latestAccDataID1, dtAccDataRecieved);
                    }
                    finally
                    {
                        Monitor.Exit(tsCollectedAccSmoothedDataID1);
                    }
                    

                    #region //obsolete
                    //accSmoothedDateTimeValuesListID1.Add(dtAccDataRecieved.Ticks);

                    //if (dmAccSmoothedDataMatrixID1 == null)
                    //{
                    //    dmAccSmoothedDataMatrixID1 = DenseMatrix.Create(1, 8, 0.0d);
                    //    DenseVector dvInitialData = (DenseVector)latestAccDataID1.ToDenseVector().SubVector(0, 3); // x,y,z
                    //    DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                    //    dvInitialData =
                    //        DenseVector.OfEnumerable(
                    //            dvInitialData.Concat(dvCalibratedData)
                    //                .Concat(new double[] { accDevAngle, latestAccDataID1.devID }));
                    //    dmAccSmoothedDataMatrixID1.InsertRow(0, dvInitialData);
                    //}
                    //else
                    //{
                    //    DenseVector dvRowToAdd = (DenseVector)latestAccDataID1.ToDenseVector().SubVector(0, 3); // x,y,z
                    //    DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                    //    dvRowToAdd =
                    //        DenseVector.OfEnumerable(
                    //            dvRowToAdd.Concat(dvCalibratedData)
                    //                .Concat(new double[] { accDevAngle, latestAccDataID1.devID }));

                    //    dmAccSmoothedDataMatrixID1 =
                    //        (DenseMatrix)dmAccSmoothedDataMatrixID1.InsertRow(dmAccSmoothedDataMatrixID1.RowCount, dvRowToAdd);
                    //}
                    #endregion //obsolete
                }
                else
                {
                    while (!Monitor.TryEnter(tsCollectedAccSmoothedDataID2))
                    {
                        Thread.Sleep(0);
                    }
                    try
                    {
                        tsCollectedAccSmoothedDataID2.AddDataRecord(latestAccDataID2, dtAccDataRecieved);
                    }
                    finally
                    {
                        Monitor.Exit(tsCollectedAccSmoothedDataID2);
                    }

                    #region //obsolete
                    //accSmoothedDateTimeValuesListID2.Add(dtAccDataRecieved.Ticks);
                    //if (dmAccSmoothedDataMatrixID2 == null)
                    //{
                    //    dmAccSmoothedDataMatrixID2 = DenseMatrix.Create(1, 8, 0.0d);
                    //    DenseVector dvInitialData = (DenseVector)latestAccDataID2.ToDenseVector().SubVector(0, 3); // x,y,z
                    //    DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                    //    dvInitialData =
                    //        DenseVector.OfEnumerable(
                    //            dvInitialData.Concat(dvCalibratedData)
                    //                .Concat(new double[] { accDevAngle, latestAccDataID2.devID }));
                    //    dmAccSmoothedDataMatrixID2.InsertRow(0, dvInitialData);
                    //}
                    //else
                    //{
                    //    DenseVector dvRowToAdd = (DenseVector)latestAccDataID2.ToDenseVector().SubVector(0, 3); // x,y,z
                    //    DenseVector dvCalibratedData = (DenseVector)accCalibrationData.ToDenseVector().SubVector(0, 3); // x,y,z calibration data
                    //    dvRowToAdd =
                    //        DenseVector.OfEnumerable(
                    //            dvRowToAdd.Concat(dvCalibratedData)
                    //                .Concat(new double[] { accDevAngle, latestAccDataID2.devID }));

                    //    dmAccSmoothedDataMatrixID2 =
                    //        (DenseMatrix)dmAccSmoothedDataMatrixID2.InsertRow(dmAccSmoothedDataMatrixID2.RowCount, dvRowToAdd);
                    //}
                    #endregion //obsolete
                }

                #endregion adding latest recieved smoothed acc data to backuping timeseries


                dataToPassToSensorsDataPresentation.Clear();
                dataToPassToSensorsDataPresentation.AddRange(new[] { latestAccDataID1, accCalibrationDataID1, latestAccDataID2, accCalibrationDataID2 });


                #region swap acc data to hdd


                //Task tskUpdateAccDataOnForm = Task.Factory.StartNew(UpdateActualAccDataOnForm);

                Task tskDumpCollectedAccData = null;
                Task tskDumpCollectedAccSmoothedID1Data = null;
                Task tskDumpCollectedAccSmoothedID2Data = null;

                if (tsCollectedAccData.Count >= 1000)
                {
                    tskDumpCollectedAccData = Task.Factory.StartNew(DumpCollectedAccelerometersData);
                }

                if (tsCollectedAccSmoothedDataID1.Count >= 500)
                {
                    tskDumpCollectedAccSmoothedID1Data =
                        Task.Factory.StartNew(DumpCollectedAccelerometersSmoothedDataDevID1);
                }

                if (tsCollectedAccSmoothedDataID2.Count >= 500)
                {
                    tskDumpCollectedAccSmoothedID2Data =
                        Task.Factory.StartNew(DumpCollectedAccelerometersSmoothedDataDevID2);
                }


                

                #region // obsolete
                //if (accDateTimeValuesList.Count >= 1000)
                //{
                //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //    dataToSave.Add("DateTime", accDateTimeValuesList.ToArray());
                //    dataToSave.Add("AccelerometerData", dmAccDataMatrix);

                //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                //        Directory.GetCurrentDirectory() + "\\logs\\AccelerometerDataLog-" +
                //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");




                //    dmAccDataMatrix = null;
                //    accDateTimeValuesList.Clear();
                //}
                


                //if (accSmoothedDateTimeValuesListID1.Count >= 500)
                //{
                //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //    dataToSave.Add("DateTime", accSmoothedDateTimeValuesListID1.ToArray());
                //    dataToSave.Add("AccelerometerData", dmAccSmoothedDataMatrixID1);

                //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                //        Directory.GetCurrentDirectory() + "\\logs\\AccelerometerID1-Smoothed-DataLog-" +
                //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");


                //    dmAccSmoothedDataMatrixID1 = null;
                //    accSmoothedDateTimeValuesListID1.Clear();
                //}


                //if (accSmoothedDateTimeValuesListID2.Count >= 500)
                //{
                //    Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //    dataToSave.Add("DateTime", accSmoothedDateTimeValuesListID2.ToArray());
                //    dataToSave.Add("AccelerometerData", dmAccSmoothedDataMatrixID2);

                //    NetCDFoperations.AddVariousDataToFile(dataToSave,
                //        Directory.GetCurrentDirectory() + "\\logs\\AccelerometerID2-Smoothed-DataLog-" +
                //        DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");


                //    dmAccSmoothedDataMatrixID2 = null;
                //    accSmoothedDateTimeValuesListID2.Clear();
                //}

                #endregion // obsolete


                if (tskDumpCollectedAccData != null)
                {
                    await tskDumpCollectedAccData;
                }

                if (tskDumpCollectedAccSmoothedID1Data != null)
                {
                    await tskDumpCollectedAccSmoothedID1Data;
                }

                if (tskDumpCollectedAccSmoothedID2Data != null)
                {
                    await tskDumpCollectedAccSmoothedID2Data;
                }

                //await tskUpdateMeteoDataOnForm;

                #endregion swap acc data to hdd


                Interlocked.Increment(ref processedUDPPacketsCounter);
            }
        }




        private void DumpCollectedAccelerometersData()
        {
            if (tsCollectedAccData.Count == 0)
            {
                return;
            }

            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedAccData))
            {
                Thread.Sleep(0);
            }

            try
            {
                tsCollectedAccData.SortByTimeStamps();
                dataToSave.Add("DateTime", tsCollectedAccData.TimeStamps.ConvertAll(dt => dt.Ticks).ToArray());
                DenseMatrix dmAccData = AccelerometerData.ToDenseMatrix(tsCollectedAccData.DataValues, dctCalibrationAccDataByDevID);
                dataToSave.Add("AccelerometerData", dmAccData);
                tsCollectedAccData.Clear();
            }
            catch (Exception ex)
            {
                #region report
#if DEBUG
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription());
                });
#else
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif
                #endregion report
            }
            finally
            {
                Monitor.Exit(tsCollectedAccData);
            }

            #endregion unload shared collections data to dataToSave


            NetCDFoperations.AddVariousDataToFile(dataToSave,
                Directory.GetCurrentDirectory() + "\\logs\\AccelerometerDataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

        }





        private void DumpCollectedAccelerometersSmoothedDataDevID1()
        {
            if (tsCollectedAccSmoothedDataID1.Count == 0)
            {
                return;
            }

            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedAccSmoothedDataID1))
            {
                Thread.Sleep(0);
            }

            try
            {
                tsCollectedAccSmoothedDataID1.SortByTimeStamps();
                dataToSave.Add("DateTime", tsCollectedAccSmoothedDataID1.TimeStamps.ConvertAll(dt => dt.Ticks).ToArray());
                DenseMatrix dmAccData = AccelerometerData.ToDenseMatrix(tsCollectedAccSmoothedDataID1.DataValues, dctCalibrationAccDataByDevID);
                dataToSave.Add("AccelerometerData", dmAccData);
                tsCollectedAccSmoothedDataID1.Clear();
            }
            catch (Exception ex)
            {
                #region report
#if DEBUG
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription());
                });
#else
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif
                #endregion report
            }
            finally
            {
                Monitor.Exit(tsCollectedAccSmoothedDataID1);
            }

            #endregion unload shared collections data to dataToSave


            NetCDFoperations.AddVariousDataToFile(dataToSave,
                Directory.GetCurrentDirectory() + "\\logs\\AccelerometerID1-Smoothed-DataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
        }





        private void DumpCollectedAccelerometersSmoothedDataDevID2()
        {
            if (tsCollectedAccSmoothedDataID2.Count == 0)
            {
                return;
            }

            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedAccSmoothedDataID2))
            {
                Thread.Sleep(0);
            }

            try
            {
                tsCollectedAccSmoothedDataID2.SortByTimeStamps();
                dataToSave.Add("DateTime", tsCollectedAccSmoothedDataID2.TimeStamps.ConvertAll(dt => dt.Ticks).ToArray());
                DenseMatrix dmAccData = AccelerometerData.ToDenseMatrix(tsCollectedAccSmoothedDataID2.DataValues, dctCalibrationAccDataByDevID);
                dataToSave.Add("AccelerometerData", dmAccData);
                tsCollectedAccSmoothedDataID2.Clear();
            }
            catch (Exception ex)
            {
                #region report
#if DEBUG
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription());
                });
#else
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif
                #endregion report
            }
            finally
            {
                Monitor.Exit(tsCollectedAccSmoothedDataID2);
            }

            #endregion unload shared collections data to dataToSave


            NetCDFoperations.AddVariousDataToFile(dataToSave,
                Directory.GetCurrentDirectory() + "\\logs\\AccelerometerID2-Smoothed-DataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
        }




        #endregion Accelerometers data processing



        #endregion обработка поступающих данных сенсоров





        





        private string swapImageToFile(Image image2write, String imageFNameAttrs = "")
        {
            string dirName = Directory.GetCurrentDirectory() + "\\snapshots\\";
            if (!ServiceTools.CheckIfDirectoryExists(dirName))
            {
                throw new Exception("couldn`t find or create output directory for snapshots:" + Environment.NewLine +
                                    dirName);
            }

            String filename1 = Directory.GetCurrentDirectory() + "\\snapshots\\img-" +
                               DateTime.UtcNow.ToString("s").Replace(":", "-");
            filename1 += imageFNameAttrs;
            filename1 += ".jpg";

            //Bitmap bm2write = new Bitmap(image2write);
            image2write.Save(filename1, System.Drawing.Imaging.ImageFormat.Jpeg);

            return filename1;
        }





        private void logCurrentSensorsData()
        {
            String filename1 = Directory.GetCurrentDirectory() + "\\results\\data-" + DateTime.UtcNow.ToString("o").Replace(":", "-") + ".xml";
            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            AccelerometerData accCalibrationData = ((latestAccData.devID == 0) || (latestAccData.devID == 1))
                ? (accCalibrationDataID1)
                : (accCalibrationDataID2);
            AccelerometerData accDataShift = latestAccData - accCalibrationData;
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

            dataToSave.Add("GPSdata", latestGPSdata.GPSstring);
            dataToSave.Add("GPSLat", latestGPSdata.Lat);
            dataToSave.Add("GPSLon", latestGPSdata.Lon);
            dataToSave.Add("GPSDateTimeUTC", latestGPSdata.dateTimeUTC.ToString("o"));
            dataToSave.Add("PressurePa", latestPressureValue);

            ServiceTools.WriteDictionaryToXml(dataToSave, filename1, false);
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



        


        private void catchCameraImages()
        {
            String usernameID1 = VivotekCameraUserName1; // tbCamUName1.Text;
            String usernameID2 = VivotekCameraUserName2; // tbCamUName2.Text;
            String passwordID1 = VivotekCameraPassword1; // tbCamPWD1.Text;
            String passwordID2 = VivotekCameraPassword2; // tbCamPWD2.Text;
            String ipAddrVivotekCamID1 = VivotekCameraID1IPaddress.ToString(); // tbCamIP1.Text.Replace(",", ".");
            String ipAddrVivotekCamID2 = VivotekCameraID2IPaddress.ToString(); // tbCamIP2.Text.Replace(",", ".");


            // Надо взять сразу оба снимка - берем в backgroundworker-ах
            DoWorkEventHandler currDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
            {
                BackgroundWorker selfworker = currBGWsender as BackgroundWorker;
                object[] currBGWarguments = (object[])args.Argument;
                string ipAddr = (string)currBGWarguments[0];
                string uname = (string)currBGWarguments[1];
                string pwd = (string)currBGWarguments[2];
                int devID = (int)currBGWarguments[3];
                Image gotImage = null;
                string gotimageFileName = "";

                String imageURL2Get = "http://" + ipAddr + "/cgi-bin/viewer/video.jpg?quality=5";

                try
                {
                    WebClient client = new WebClient();
                    client.Credentials = new NetworkCredential(uname, pwd);
                    DateTime reqSnapshotDT = DateTime.UtcNow;
                    Stream stream = client.OpenRead(imageURL2Get);
                    gotImage = Image.FromStream(stream);
                    stream.Flush();
                    stream.Close();

                    Note("image from camera ID" + devID + " requested at " + reqSnapshotDT.ToString("o"));
                    Note("image from camera ID" + devID + " recieved at " + DateTime.UtcNow.ToString("o"));

                    gotimageFileName = swapImageToFile(gotImage, "devID" + devID);
                    FileInfo finfo = new FileInfo(gotimageFileName);

                    if ((devID == 0) || (devID == 1))
                    {
                        //ThreadSafeOperations.UpdatePictureBox(pbThumbPreviewCam1, gotImage, true);
                        currCaughtImageID1 = new Image<Bgr, byte>((Bitmap)gotImage);
                        ThreadSafeOperations.SetText(lblSnapshotFilenameID1, finfo.Name, false);
                        ThreadSafeOperations.SetText(lblGotSnapshotDateTimeID1,
                            "requested at: " + reqSnapshotDT.ToString("o").Replace("Z", "") +
                            Environment.NewLine +
                            "recieved at: " + DateTime.UtcNow.ToString("o").Replace("Z", ""), false);
                        RaisePaintEvent(null, null);
                    }
                    else if (devID == 2)
                    {
                        //ThreadSafeOperations.UpdatePictureBox(pbThumbPreviewCam2, gotImage, true);
                        currCaughtImageID2 = new Image<Bgr, byte>((Bitmap)gotImage);
                        ThreadSafeOperations.SetText(lblSnapshotFilenameID2, finfo.Name, false);
                        ThreadSafeOperations.SetText(lblGotSnapshotDateTimeID2,
                            "requested at: " + reqSnapshotDT.ToString("o").Replace("Z", "") +
                            Environment.NewLine +
                            "recieved at: " + DateTime.UtcNow.ToString("o").Replace("Z", ""), false);
                        RaisePaintEvent(null, null);
                    }
                }
                catch (Exception e)
                {
                    Note(e.Message);
                }

                args.Result = new object[] { gotImage, devID, gotimageFileName };
            };

            RunWorkerCompletedEventHandler currWorkCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
            {
                object[] currentBGWResults = (object[])args.Result;
                Image gotimage = currentBGWResults[0] as Image;
                int gotDevID = (int)currentBGWResults[1];
                string gotimageFileName = (string)currentBGWResults[2];
                Note("got image: " + gotimageFileName);
            };



            BackgroundWorker bgwGetImgID1 = new BackgroundWorker();
            bgwGetImgID1.WorkerSupportsCancellation = true;
            bgwGetImgID1.DoWork += currDoWorkHandler;
            bgwGetImgID1.RunWorkerCompleted += currWorkCompletedHandler;
            object[] BGWargsID1 = new object[] { ipAddrVivotekCamID1, usernameID1, passwordID1, 1 };


            BackgroundWorker bgwGetImgID2 = new BackgroundWorker();
            bgwGetImgID2.WorkerSupportsCancellation = true;
            bgwGetImgID2.DoWork += currDoWorkHandler;
            bgwGetImgID2.RunWorkerCompleted += currWorkCompletedHandler;
            object[] BGWargsID2 = new object[] { ipAddrVivotekCamID2, usernameID2, passwordID2, 2 };


            bgwGetImgID1.RunWorkerAsync(BGWargsID1);
            bgwGetImgID2.RunWorkerAsync(BGWargsID2);
        }



        #region // заглушка когда нет камеры
        //private void catchCameraImage()
        //{
        //    string timeStampStr = DateTime.UtcNow.ToString("o") + Environment.NewLine;
        //    Note(timeStampStr + "The picture could be taken here");
        //
        //}
        #endregion // заглушка когда нет камеры




        #region accelerometer calibration


        /// <summary>
        /// Handles the DoWork event of the accelCalibrator control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        /// todo: переделать в event-driven вариант по коллекции accDataAndDTObservableConcurrentQueue
        private void accelCalibrator_DoWork(object sender, DoWorkEventArgs e)
        {
            //DateTime datetimeCalibrationBegin = DateTime.Now;
            //DateTime datetimeCalibrationTimerNow = DateTime.Now;
            //TimeSpan calibrationTimer = datetimeCalibrationTimerNow - datetimeCalibrationBegin;
            //TimeSpan calibrationRecalcPeriod = new TimeSpan(0, 0, 1);
            //TimeSpan labelsUpdatingPeriod = new TimeSpan(0, 0, 1);
            //DateTime datetimePreviousLabelsUpdate = DateTime.MinValue;
            //double horizAcc, totalAcc, percHorizAcc;
            //double percAccDeviation;
            double[] dataSetX = new double[1000];
            double[] dataSetY = new double[1000];
            double[] dataSetZ = new double[1000];
            int i = 0;
            bool isTheFirstPass = true;

            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            ThreadSafeOperations.ToggleButtonState(btnCalibrateAccelerometerID1, true, "Stop calibrating", true);
            dataCollectingState = DataCollectingStates.working;


            while (true)
            {
                if (SelfWorker.CancellationPending)
                {
                    break;
                }

                // if (accDataQueue.Count > 0)
                if (accDataAndDTObservableConcurrentQueue.Count > 0)
                {
                    Tuple<DateTime, AccelerometerData> tplAccDT;
                    while (!accDataAndDTObservableConcurrentQueue.TryDequeue(out tplAccDT))
                    {
                        Thread.Sleep(0);
                        continue;
                    }

                    if (tplAccDT == null) continue;

                    //accelerometerData accData = accDataQueue.Dequeue();
                    AccelerometerData accData = tplAccDT.Item2;
                    DateTime dtAccDataRecieved = tplAccDT.Item1;

                    ThreadSafeOperations.SetText(lblAccelCalibrationCurrentXID1, accData.AccX.ToString(), false);
                    ThreadSafeOperations.SetText(lblAccelCalibrationCurrentYID1, accData.AccY.ToString(), false);
                    ThreadSafeOperations.SetText(lblAccelCalibrationCurrentZID1, accData.AccZ.ToString(), false);

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
                    DescriptiveStatistics statisticsX = new DescriptiveStatistics(dataSetX);
                    DescriptiveStatistics statisticsY = new DescriptiveStatistics(dataSetY);
                    DescriptiveStatistics statisticsZ = new DescriptiveStatistics(dataSetZ);

                    double meanX = statisticsX.Mean;
                    double stDevX = Math.Round(100 * statisticsX.StandardDeviation / meanX, 2);

                    double meanY = statisticsY.Mean;
                    double stDevY = Math.Round(100 * statisticsY.StandardDeviation / meanY, 2);

                    double meanZ = statisticsZ.Mean;
                    double stDevZ = Math.Round(100 * statisticsZ.StandardDeviation / meanZ, 2);

                    TimeSpan calcSpan = DateTime.Now - calcBegin;

                    accCalibrationDataID1.AccDoubleX = meanX;
                    accCalibrationDataID1.AccDoubleY = meanY;
                    accCalibrationDataID1.AccDoubleZ = meanZ;

                    ThreadSafeOperations.SetText(lblAccelCalibrationXID1, "<" + Math.Round(meanX, 2).ToString() + ">", false);
                    ThreadSafeOperations.SetText(lblAccelCalibrationYID1, "<" + Math.Round(meanY, 2).ToString() + ">", false);
                    ThreadSafeOperations.SetText(lblAccelCalibrationZID1, "<" + Math.Round(meanZ, 2).ToString() + ">", false);
                    ThreadSafeOperations.SetText(lblStDevXID1, stDevX.ToString() + "%", false);
                    ThreadSafeOperations.SetText(lblStDevYID1, stDevY.ToString() + "%", false);
                    ThreadSafeOperations.SetText(lblStDevZID1, stDevZ.ToString() + "%", false);
                    string txt2Show = "i = " + i.ToString();
                    txt2Show += Environment.NewLine + "calc time = " + calcSpan.Ticks.ToString();
                    ThreadSafeOperations.SetText(lblCalculationStatisticsID1, txt2Show, false);
                    //accDatahasBeenChanged = false;
                }

                //datetimeCalibrationTimerNow = DateTime.Now;
                //calibrationTimer = datetimeCalibrationTimerNow - datetimeCalibrationBegin;
            }
        }



        private void accelCalibrator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (needsToSwitchListeningArduinoOFF)
            {
                needsToSwitchListeningArduinoOFF = false;
                udpCatchingJob.CancelAsync();
            }
            ThreadSafeOperations.ToggleButtonState(btnCalibrateAccelerometerID1, true, "Calibrate accelerometer", false);
            dataCollectingState = DataCollectingStates.idle;
        }
        

        #endregion accelerometer calibration






        #region process catched UDP messages from cquArduinoUDPCatchedMessages

        void cquArduinoUDPCatchedMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                return;
                // чтобы не реагировать на собственные TryDequeue()
            }

            while (cquArduinoUDPCatchedMessages.Count > 0)
            {
                if (cquArduinoUDPCatchedMessages.Count > 0)
                {
                    try
                    {
                        ProcessUDPmessagesFromQueue();
                    }
                    catch (Exception ex)
                    {
                        #region report
#if DEBUG
                        ServiceTools.ExecMethodInSeparateThread(this, () =>
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                "exception has been thrown: " + ex.Message + Environment.NewLine +
                                ServiceTools.CurrentCodeLineDescription());
                        });
#else
                    ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif
                        #endregion report
                    }
                }
            }
        }




        private void ProcessUDPmessagesFromQueue()
        {

            while (cquArduinoUDPCatchedMessages.Count > 0)
            {
                if (cquArduinoUDPCatchedMessages.Count == 0)
                {
                    return;
                }

                IncomingUDPmessageBoundle curMessageBoundle = null;
                while ((cquArduinoUDPCatchedMessages.Count > 0) &&
                       (!cquArduinoUDPCatchedMessages.TryDequeue(out curMessageBoundle)))
                {
                    continue;
                }

                if (curMessageBoundle == null) return;


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
                                //Application.DoEvents();
                                //Thread.Sleep(0);
                                continue;
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
                    if (currMessageDevID == 1)
                    {
                        Note("devID:" + currMessageDevID + "   |   " + curMessageBoundle.udpMessage);
                    }
                    else if (currMessageDevID == 2)
                    {
                        Note("devID:" + currMessageDevID + "   |   " + curMessageBoundle.udpMessage);
                    }
                    else
                    {
                        Note(curMessageBoundle.udpMessage);
                    }

                    replyMessage = curMessageBoundle.udpMessage;
                    if (needsReplyOnRequest) needsReplyOnRequest = false;
                }
                else if (curMessageBoundle.udpMessage == "imarduino")
                {
                    if ((needsToDiscoverArduinoBoardID1) && (currMessageDevID == 1))
                    {
                        string addrStr = curMessageBoundle.ipAddrString;
                        ThreadSafeOperations.SetTextTB(tbIP2ListenDevID1, addrStr, false);
                        needsToDiscoverArduinoBoardID1 = false;
                    }
                    if (needsToDiscoverArduinoBoardID2 && (currMessageDevID == 2))
                    {
                        string addrStr = curMessageBoundle.ipAddrString;
                        ThreadSafeOperations.SetTextTB(tbIP2ListenDevID2, addrStr, false);
                        needsToDiscoverArduinoBoardID2 = false;
                    }
                }
                else
                {
                    if ((!needsToDiscoverArduinoBoardID2) && (!needsToDiscoverArduinoBoardID1))
                    {
                        string timeStampStr = DateTime.UtcNow.ToString("o") + ": ";

                        if (currMessageDevID > 0)
                        {
                            NoteLog("devID:" + currMessageDevID + "   |   " + timeStampStr +
                                    curMessageBoundle.udpMessage);
                        }
                        else
                        {
                            NoteLog(timeStampStr + curMessageBoundle.udpMessage);
                        }

                        ParseBroadcastMessage(curMessageBoundle.udpMessage, currMessageDevID);
                    }
                }
            }
        }



        



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
                            GPSdata catchedGPSdata = new GPSdata(dataValuesString, GPSdatasources.CloudCamArduinoGPS);
                            if (catchedGPSdata.validGPSdata)
                            {
                                latestGPSdata = catchedGPSdata;
                                latestGPSdata.devID = devID;
                                gpsDataAndDTObservableConcurrentQueue.Enqueue(
                                    new Tuple<DateTime, GPSdata>(DateTime.UtcNow, latestGPSdata));
                            }
                            
                            break;
                        }
                    case "dta":
                        {
                            //accelerometer data
                            string[] splitters = { ";" };
                            string[] stringAccValues = dataValuesString.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
                            latestAccData = new AccelerometerData(stringAccValues);
                            latestAccData.devID = devID;

                            if (latestAccData.validAccData)
                            {
                                //accDataQueue.Enqueue(latestAccData);
                                accDataAndDTObservableConcurrentQueue.Enqueue(new Tuple<DateTime, AccelerometerData>(DateTime.UtcNow,
                                    latestAccData));
                            }

                            break;
                        }
                    case "dtg":
                        {
                            //gyroscope data
                            string[] splitters = { ";" };
                            string[] stringGyroValues = dataValuesString.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
                            latestGyroData = new GyroData(stringGyroValues);
                            latestGyroData.devID = devID;
                            //gyroDataQueue.Enqueue(latestGyroData);
                            gyroDataAndDTObservableConcurrentQueue.Enqueue(new Tuple<DateTime, GyroData>(DateTime.UtcNow, latestGyroData));
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
                                //pressureHasBeenChanged = false;
                                break;
                            }

                            try
                            {
                                int pressure = Convert.ToInt32(stringValues[0]);
                                pressureDataAndDTObservableConcurrentQueue.Enqueue(new Tuple<DateTime, int>(DateTime.UtcNow, pressure));
                                //pressureHasBeenChanged = true;
                            }
                            catch (Exception)
                            {
                                //pressureHasBeenChanged = false;
                                break;
                            }
                            break;
                        }
                    default:
                        break;
                }

            }
        }

        #endregion process catched UDP messages from cquArduinoUDPCatchedMessages





        #region // bgwUDPmessagesParser is obsolete

        //private void bgwUDPmessagesParser_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker selfWorker = sender as BackgroundWorker;


        //    while (true)
        //    {
        //        if (selfWorker.CancellationPending && cquArduinoUDPCatchedMessages.Count == 0)
        //        {
        //            break;
        //        }

        //        if (cquArduinoUDPCatchedMessages.Count == 0)
        //        {
        //            //Application.DoEvents();
        //            //Thread.Sleep(0);
        //            continue;
        //        }


        //        if (cquArduinoUDPCatchedMessages.Count > 0)
        //        {

        //            try
        //            {
        //                ProcessUDPmessagesFromQueue(selfWorker);
        //            }
        //            catch (Exception ex)
        //            {
        //                theLogWindow = ServiceTools.LogAText(theLogWindow, ex.Message);
        //                //ServiceTools.logToTextFile(Directory.GetCurrentDirectory() + "\\error.log",
        //                //    Environment.NewLine + ex.Message + Environment.NewLine, true);
        //                continue;
        //            }

        //        }
        //        else
        //        {
        //            //Application.DoEvents();
        //            //Thread.Sleep(0);
        //            continue;
        //        }

        //    }
        //}

        #endregion // bgwUDPmessagesParser is obsolete







        #region Form behaviour

        private void tbIP2ListenDevID2_TextChanged(object sender, EventArgs e)
        {
            ip2ListenID2 = tbIP2ListenDevID2.Text;
        }





        private void DataCollectorMainForm_Paint(object sender, PaintEventArgs e)
        {
            if (currCaughtImageID1 != null)
            {
                ThreadSafeOperations.UpdatePictureBox(pbThumbPreviewCam1, currCaughtImageID1.Bitmap, true);
            }

            if (currCaughtImageID2 != null)
            {
                ThreadSafeOperations.UpdatePictureBox(pbThumbPreviewCam2, currCaughtImageID2.Bitmap, true);
            }
        }




        private void DataCollectorMainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }




        private void lblSunElev_Click(object sender, EventArgs e)
        {
            if (latestGPSdata.validGPSdata)
            {
                SPA spaCalc = new SPA(latestGPSdata.dateTimeUTC.Year, latestGPSdata.dateTimeUTC.Month, latestGPSdata.dateTimeUTC.Day, latestGPSdata.dateTimeUTC.Hour,
                        latestGPSdata.dateTimeUTC.Minute, latestGPSdata.dateTimeUTC.Second, (float)latestGPSdata.LonDec, (float)latestGPSdata.LatDec,
                        (float)SPAConst.DeltaT(latestGPSdata.dateTimeUTC));
                int res = spaCalc.spa_calculate();
                AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                    spaCalc.spa.zenith);
                spaCalc.spa.function = SPAFunctionType.SPA_ZA_RTS;
                spaCalc.spa_calculate();





                theLogWindow = ServiceTools.LogAText(theLogWindow, latestGPSdata.dateTimeUTC.ToString() +
                                                                   Environment.NewLine + "sunrise: " +
                                                                   (new TimeOfDay(spaCalc.spa.sunrise)) +
                                                                   Environment.NewLine + "sunset: " +
                                                                   (new TimeOfDay(spaCalc.spa.sunset)));
            }
        }


        private void btnPrefs_Click(object sender, EventArgs e)
        {
            PropertiesEditor propForm = new PropertiesEditor(defaultProperties, defaultPropertiesXMLfileName);
            propForm.FormClosed += new FormClosedEventHandler(PropertiesFormClosed);
            propForm.ShowDialog();
        }


        private void PropertiesFormClosed(object sender, FormClosedEventArgs e)
        {
            readDefaultProperties();
        }



        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                         "\\settings\\DataCollectorAppGeneralSettings2G.xml";
            if (!File.Exists(defaultPropertiesXMLfileName)) return;
            defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);



            accCalibrationDataFilename = Directory.GetCurrentDirectory() +
                                         "\\settings\\AccelerometerCalibrationData2G.xml";

            accCalibrationDataDict = ServiceTools.ReadDictionaryFromXML(accCalibrationDataFilename);


            accCalibrationDataID1 = new AccelerometerData(Convert.ToDouble(accCalibrationDataDict["accID1CalibratedXzero"]),
                Convert.ToDouble(accCalibrationDataDict["accID1CalibratedYzero"]),
                Convert.ToDouble(accCalibrationDataDict["accID1CalibratedZzero"]));
            accCalibrationDataID2 = new AccelerometerData(Convert.ToDouble(accCalibrationDataDict["accID2CalibratedXzero"]),
                Convert.ToDouble(accCalibrationDataDict["accID2CalibratedYzero"]),
                Convert.ToDouble(accCalibrationDataDict["accID2CalibratedZzero"]));


            ip2ListenID1 = defaultProperties["ArduinoBoardID1DefaultIP"] as string;
            ip2ListenID2 = defaultProperties["ArduinoBoardID2DefaultIP"] as string;

            port2converse = Convert.ToInt32(defaultProperties["ArduinoBoardDefaultUDPport"]);
            portBcstRecvng = Convert.ToInt32(defaultProperties["UDPBroadcastDefaultListeningPort"]);
            string strCamShotPeriod = (defaultProperties["VivotekCameraShootingPeriodSec"]) as string;

            CamShotPeriod = new TimeSpan(0, 0, Convert.ToInt32(strCamShotPeriod));


            IPAddress.TryParse(defaultProperties["VivotekCameraID1IPaddr"] as string, out VivotekCameraID1IPaddress);
            IPAddress.TryParse(defaultProperties["VivotekCameraID2IPaddr"] as string, out VivotekCameraID2IPaddress);
            VivotekCameraUserName1 = defaultProperties["VivotekCameraID1UserName"] as string;
            VivotekCameraPassword1 = defaultProperties["VivotekCameraID1Password"] as string;
            VivotekCameraUserName2 = defaultProperties["VivotekCameraID2UserName"] as string;
            VivotekCameraPassword2 = defaultProperties["VivotekCameraID2Password"] as string;
            BroadcastLogHistorySizeLines = Convert.ToInt32(defaultProperties["BroadcastLogHistorySizeLines"]);

            try
            {
                angleCamDeclinationThresholdDeg = Convert.ToDouble(defaultProperties["CamDeclinationThresholdDegToShoot"]);
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "couldn`t read critical camera declination angle. Using default value = " +
                    angleCamDeclinationThresholdDeg.ToString("F2") + Environment.NewLine +
                    "exception was thrown: \"" + ex.Message + "\"");
            }



            ThreadSafeOperations.SetTextTB(tbIP2ListenDevID1, ip2ListenID1, false);
            ThreadSafeOperations.SetTextTB(tbIP2ListenDevID2, ip2ListenID2, false);



            if (accCalibrationDataID1.AccMagnitude == 0.0d)
            {
                accCalibrationDataID1 = new AccelerometerData(0.0, 0.0, -256.0);
            }
            if (accCalibrationDataID2.AccMagnitude == 0.0d)
            {
                accCalibrationDataID2 = new AccelerometerData(0.0, 0.0, -256.0);
            }

            dctCalibrationAccDataByDevID.Add("devID1", accCalibrationDataID1);
            dctCalibrationAccDataByDevID.Add("devID2", accCalibrationDataID2);

            ThreadSafeOperations.SetText(lblAccelCalibrationXID1, Math.Round(accCalibrationDataID1.AccDoubleX, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationYID1, Math.Round(accCalibrationDataID1.AccDoubleY, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationZID1, Math.Round(accCalibrationDataID1.AccDoubleZ, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationXID2, Math.Round(accCalibrationDataID2.AccDoubleX, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationYID2, Math.Round(accCalibrationDataID2.AccDoubleY, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationZID2, Math.Round(accCalibrationDataID2.AccDoubleZ, 2).ToString(), false);

            angleCamDeclinationThresholdRad = Math.PI * angleCamDeclinationThresholdDeg / 180.0d;
        }




        private void DataCollectorMainForm_Shown(object sender, EventArgs e)
        {
            readDefaultProperties();

            cquArduinoUDPCatchedMessages.CollectionChanged += cquArduinoUDPCatchedMessages_CollectionChanged;

            accDataAndDTObservableConcurrentQueue.CollectionChanged += accDataAndDTObservableConcurrentQueue_CollectionChanged;

            gyroDataAndDTObservableConcurrentQueue.CollectionChanged += gyroDataAndDTObservableConcurrentQueue_CollectionChanged;

            gpsDataAndDTObservableConcurrentQueue.CollectionChanged += gpsDataAndDTObservableConcurrentQueue_CollectionChanged;

            pressureDataAndDTObservableConcurrentQueue.CollectionChanged += pressureDataAndDTObservableConcurrentQueue_CollectionChanged;

            this.NeedToShootCameraSnapshots += DataCollectorMainForm_NeedToShootCameraSnapshots;
        }



        void DataCollectorMainForm_NeedToShootCameraSnapshots(object sender, EventArgs e)
        {
            ServiceTools.ExecMethodInSeparateThread(this, catchCameraImages);
        }

        

        

        



        private void btnAccCalibrationData_Click(object sender, EventArgs e)
        {
            PropertiesEditor propForm = new PropertiesEditor(accCalibrationDataDict, accCalibrationDataFilename);
            propForm.FormClosed += new FormClosedEventHandler(PropertiesFormClosed);
            propForm.ShowDialog();
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




        private void btnCollectMostClose_Click(object sender, EventArgs e)
        {
            operatorCommandsToGetCamShot = true;
        }




        private void btnSaveAccel_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> propertiesDictToSave = new Dictionary<string, object>();
            propertiesDictToSave.Add("accID1CalibratedXzero", accCalibrationDataID1.AccDoubleX);
            propertiesDictToSave.Add("accID1CalibratedYzero", accCalibrationDataID1.AccDoubleY);
            propertiesDictToSave.Add("accID1CalibratedZzero", accCalibrationDataID1.AccDoubleZ);
            ServiceTools.WriteDictionaryToXml(propertiesDictToSave, accCalibrationDataFilename, false);
        }






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
                ThreadSafeOperations.ToggleButtonState(btnCalibrateAccelerometerID1, false, "wait for state checking", true);
                //StartStopDataCollectingWaitingCircle.Visible = true;
                //StartStopDataCollectingWaitingCircle.Active = true;
                dataCollectingState = DataCollectingStates.checkingState;
                theWorkerRequestedArduinoDataBroadcastState = WorkersRequestingArduinoDataBroadcastState.accelCalibrator;
                currOperatingDevID = 1;
                PerformRequestArduinoBoard("1", currOperatingDevID);
            }
            else
            {
                accelCalibrator.CancelAsync();
            }
        }



        private void btnCollectImmediately_Click(object sender, EventArgs e)
        {
            //if (dataCollector.IsBusy)
            //{
            //    operatorCommandsToGetCamShotImmediately = true;
            //}
            //else
            //{
            //    catchCameraImages();
            //}

            EventHandler onNeedToShootCameraSnapshots = this.NeedToShootCameraSnapshots;
            if (onNeedToShootCameraSnapshots != null) onNeedToShootCameraSnapshots(null, null);

        }


        #endregion Form behaviour


    }

    #endregion the form class






    #region event-model usage

    public class NotAllBgwWorkingAlertEventArgs
    {
        public String Message { get; private set; }

        public NotAllBgwWorkingAlertEventArgs(string message)
        {
            Message = message;
        }
    }

    #endregion event-model usage





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
