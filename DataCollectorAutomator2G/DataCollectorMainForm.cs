﻿using System;
using System.Collections;
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
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
using nsoftware.IPWorks;
using nsoftware.IPWorksIPC;
using nsoftware.IPWorksSSH;
using nsoftware.IPWorksZip;



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

        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar + "DataCollectorAutomator2G-error.log";

        public event EventHandler NeedToShootCameraSnapshots;
        private bool restrictDataRegistrationWhenSunElevationLowerThanZero = true;
        private bool MakeBeepsSilentWhenSunElevationLowerThanZero = true;
        private bool ForceBeepsSilent = false;
        private bool IsNowSilent = false;

        private bool AllowOperationWhileArduinoIsOffline = false;

        private bool bOrganizeAndArchiveCollectedDataAtLocalMidnight = false;

        private string outputSnapshotsDirectory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                                  "snapshots" + Path.DirectorySeparatorChar;

        private string strOutputConcurrentDataDirectory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                                          "results" + Path.DirectorySeparatorChar;


        #region // image stats computing server connections

        //private bool bUseRemoteStatsCalculationServer = false;
        //private string strRemoteStatsCalculatingServerHost = "192.168.192.200";
        //private int intRemoteStatsCalculatingServerPort = 55520;

        #endregion


        #region // CC_Moscow_bot connected properties

        //private TimeSpan makeCurrentSnapshotPreviewPicturePeriod = new TimeSpan(0, 1, 0);
        //private bool bSendProcessedDataTo_CC_Moscow_bot_server = false;
        //private string strRemoteBotServerHost = "krinitsky.ru";
        //private int intRemoteBotServerPort = 22;
        //private string strRemoteBotServerHostAuthKeyFile = "";
        //private string strRemoteBotServerSSHusername = "mk";
        //private string strAcceptedSSHhostCertPublicKeyFile = "";

        #endregion // CC_Moscow_bot connected properties


        #region pipeserver for IPC broadcasting data

        Pipeserver ipcPipeserver = null;

        #endregion



        private string ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar +
                                          "ImagesRoundMasksXMLfilesMappingList.csv";
        private string strPerformanceCountersStatsFile = "";
        private string imageYRGBstatsXMLdataFilesDirectory = "";

        #region periodical images processing

        private TimeSpan SDCandTCCreadAndReportPeriod = new TimeSpan(0, 5, 0);
        private bool restrictSnapshotsProcessingWhenSunElevationLowerThanZero = true;
        ConcurrentExclusiveSchedulerPair scheduler;
        TaskFactory lowPriorityTaskFactory;

        #endregion periodical images processing












        #region Form general features and behaviour

        public DataCollectorMainForm()
        {
            InitializeComponent();

            // scheduler for the low-priority tasks like SDC and TCC computing
            scheduler = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, 4);
            lowPriorityTaskFactory = new TaskFactory(scheduler.ConcurrentScheduler);
        }






        void LogMessageToLogWindow(string message)
        {
            try
            {
                if (theLogWindow != null)
                {
                    if (!theLogWindow.IsDisposed)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, message);
                    }
                }
                
            }
            catch (Exception ex)
            {
                ;
            }
        }





        private void DataCollectorMainForm_Shown(object sender, EventArgs e)
        {
            readDefaultProperties();

            theLogWindow = ServiceTools.LogAText(theLogWindow, CommonTools.DictionaryRepresentation(defaultProperties));
            theLogWindow.title = "Data collector 2G log";

            StartIPCpipeServer();


            cquArduinoUDPCatchedMessages.CollectionChanged += cquArduinoUDPCatchedMessages_CollectionChanged;

            accDataAndDTObservableConcurrentQueue.CollectionChanged += accDataAndDTObservableConcurrentQueue_CollectionChanged;

            gyroDataAndDTObservableConcurrentQueue.CollectionChanged += gyroDataAndDTObservableConcurrentQueue_CollectionChanged;

            gpsDataAndDTObservableConcurrentQueue.CollectionChanged += gpsDataAndDTObservableConcurrentQueue_CollectionChanged;

            pressureDataAndDTObservableConcurrentQueue.CollectionChanged += pressureDataAndDTObservableConcurrentQueue_CollectionChanged;

            this.NeedToShootCameraSnapshots += DataCollectorMainForm_NeedToShootCameraSnapshots;
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


        private void btnSwapBcstLog_Click(object sender, EventArgs e)
        {
            swapBcstLog();
        }


        private void swapBcstLog()
        {
            string filename1 = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                               Path.DirectorySeparatorChar + "DataCollector2G-BcstLog-" +
                               DateTime.UtcNow.ToString("o").Replace(":", "-").Replace("Z", "") + ".log";
            ServiceTools.logToTextFile(filename1, strTotalBcstLog, true);

            strTotalBcstLog = "";


            string filename2 = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                               Path.DirectorySeparatorChar + "DataCollector2G-NotesLog-" +
                               DateTime.UtcNow.ToString("o").Replace(":", "-").Replace("Z", "") + ".log";
            ServiceTools.logToTextFile(filename2, tbMainLog.Text, true);
            ThreadSafeOperations.SetTextTB(tbMainLog, "", false);
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
                                                                   Environment.NewLine + "azimuth: " +
                                                                   sunPositionSPAext.Azimuth.ToString("F2") + "deg." +
                                                                   Environment.NewLine + "elevation: " +
                                                                   sunPositionSPAext.ElevationAngle.ToString("F2") + "deg." +
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



        #region default properties read-write

        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                           "settings" + Path.DirectorySeparatorChar +
                                           "DataCollectorAppGeneralSettings2G.xml";
            if (!File.Exists(defaultPropertiesXMLfileName)) return;
            defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);

            bool bDefaultPropertiesHasBeenUpdated = false;

            accCalibrationDataFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                         "settings" + Path.DirectorySeparatorChar + "AccelerometerCalibrationData2G.xml";

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



            #region RestrictDataRegistrationWhenSunElevationLowerThanZero
            if (defaultProperties.ContainsKey("RestrictDataRegistrationWhenSunElevationLowerThanZero"))
            {
                restrictDataRegistrationWhenSunElevationLowerThanZero =
                    ((string)defaultProperties["RestrictDataRegistrationWhenSunElevationLowerThanZero"]).ToLower() ==
                    "true";
            }
            else
            {
                defaultProperties.Add("RestrictDataRegistrationWhenSunElevationLowerThanZero", true);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion RestrictDataRegistrationWhenSunElevationLowerThanZero



            #region angleCamDeclinationThresholdDeg

            if (defaultProperties.ContainsKey("CamDeclinationThresholdDegToShoot"))
            {
                angleCamDeclinationThresholdDeg =
                    Convert.ToDouble(defaultProperties["CamDeclinationThresholdDegToShoot"]);
            }
            else
            {
                angleCamDeclinationThresholdDeg = 3.0d;
                defaultProperties.Add("CamDeclinationThresholdDegToShoot", angleCamDeclinationThresholdDeg);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion angleCamDeclinationThresholdDeg



            #region MakeBeepsSilentWhenSunElevationLowerThanZero
            if (defaultProperties.ContainsKey("MakeBeepsSilentWhenSunElevationLowerThanZero"))
            {
                MakeBeepsSilentWhenSunElevationLowerThanZero =
                    (((string)defaultProperties["MakeBeepsSilentWhenSunElevationLowerThanZero"]).ToLower() == "true");
            }
            else
            {
                defaultProperties.Add("MakeBeepsSilentWhenSunElevationLowerThanZero", true);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion MakeBeepsSilentWhenSunElevationLowerThanZero



            #region ForceBeepsSilent
            if (defaultProperties.ContainsKey("ForceBeepsSilent"))
            {
                ForceBeepsSilent = (((string)defaultProperties["ForceBeepsSilent"]).ToLower() == "true");
            }
            else
            {
                defaultProperties.Add("ForceBeepsSilent", true);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion ForceBeepsSilent



            #region AllowOperationWhileArduinoIsOffline
            if (defaultProperties.ContainsKey("AllowOperationWhileArduinoIsOffline"))
            {
                AllowOperationWhileArduinoIsOffline = (((string)defaultProperties["AllowOperationWhileArduinoIsOffline"]).ToLower() == "true");
            }
            else
            {
                defaultProperties.Add("AllowOperationWhileArduinoIsOffline", false);
                AllowOperationWhileArduinoIsOffline = false;
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion AllowOperationWhileArduinoIsOffline



            #region bOrganizeAndArchiveCollectedDataAtLocalMidnight
            // bOrganizeAndArchiveCollectedDataAtLocalMidnight

            if (defaultProperties.ContainsKey("bOrganizeAndArchiveCollectedDataAtLocalMidnight"))
            {
                bOrganizeAndArchiveCollectedDataAtLocalMidnight = (((string)defaultProperties["bOrganizeAndArchiveCollectedDataAtLocalMidnight"]).ToLower() == "true");
            }
            else
            {
                defaultProperties.Add("bOrganizeAndArchiveCollectedDataAtLocalMidnight", false);
                bOrganizeAndArchiveCollectedDataAtLocalMidnight = false;
                bDefaultPropertiesHasBeenUpdated = true;
            }


            #endregion




            #region // image stats computing server connections

            #region // bUseRemoteStatsCalculationServer

            //if (defaultProperties.ContainsKey("bUseRemoteStatsCalculationServer"))
            //{
            //    bUseRemoteStatsCalculationServer =
            //        ((string)(defaultProperties["bUseRemoteStatsCalculationServer"])).ToLower() == "true";
            //}
            //else
            //{
            //    bUseRemoteStatsCalculationServer = false;
            //    defaultProperties.Add("bUseRemoteStatsCalculationServer", bUseRemoteStatsCalculationServer);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}

            #endregion bUseRemoteStatsCalculationServer


            #region // strRemoteStatsCalculatingServerHost

            //if (defaultProperties.ContainsKey("strRemoteStatsCalculatingServerHost"))
            //{
            //    strRemoteStatsCalculatingServerHost =
            //        (string)(defaultProperties["strRemoteStatsCalculatingServerHost"]);
            //}
            //else
            //{
            //    strRemoteStatsCalculatingServerHost = "sail.msk.ru";
            //    defaultProperties.Add("strRemoteStatsCalculatingServerHost", strRemoteStatsCalculatingServerHost);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}

            #endregion strRemoteStatsCalculatingServerHost


            #region // intRemoteStatsCalculatingServerPort

            //if (defaultProperties.ContainsKey("intRemoteStatsCalculatingServerPort"))
            //{
            //    intRemoteStatsCalculatingServerPort =
            //        Convert.ToInt32(defaultProperties["intRemoteStatsCalculatingServerPort"]);
            //}
            //else
            //{
            //    intRemoteStatsCalculatingServerPort = 55520;
            //    defaultProperties.Add("intRemoteStatsCalculatingServerPort", intRemoteStatsCalculatingServerPort);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}

            #endregion intRemoteStatsCalculatingServerPort

            #endregion // image stats computing server connections




            #region ImagesRoundMasksXMLfilesMappingList
            // ImagesRoundMasksXMLfilesMappingList
            if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
            {
                ImagesRoundMasksXMLfilesMappingList = (string)defaultProperties["ImagesRoundMasksXMLfilesMappingList"];
            }
            else
            {
                ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                                      "settings" +
                                                      Path.DirectorySeparatorChar +
                                                      "ImagesRoundMasksXMLfilesMappingList.csv";
                defaultProperties.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion ImagesRoundMasksXMLfilesMappingList



            #region strPerformanceCountersStatsFile
            if (defaultProperties.ContainsKey("strPerformanceCountersStatsFile"))
            {
                strPerformanceCountersStatsFile = (string)defaultProperties["strPerformanceCountersStatsFile"];
            }
            else
            {
                strPerformanceCountersStatsFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-perf-data.csv";
                defaultProperties.Add("strPerformanceCountersStatsFile", strPerformanceCountersStatsFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion strPerformanceCountersStatsFile



            #region imageYRGBstatsXMLdataFilesDirectory
            if (defaultProperties.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
            {
                imageYRGBstatsXMLdataFilesDirectory = (string)defaultProperties["imageYRGBstatsXMLdataFilesDirectory"];
            }
            else
            {
                imageYRGBstatsXMLdataFilesDirectory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "stats" +
                                          Path.DirectorySeparatorChar;
                ServiceTools.CheckIfDirectoryExists(imageYRGBstatsXMLdataFilesDirectory);
                defaultProperties.Add("imageYRGBstatsXMLdataFilesDirectory", imageYRGBstatsXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }
            #endregion imageYRGBstatsXMLdataFilesDirectory




            #region snapshotsProcessingPeriod
            //TimeSpan snapshotsProcessingPeriod = new TimeSpan(0, 5, 0);
            if (defaultProperties.ContainsKey("strSnapshotsProcessingPeriodSeconds"))
            {
                string strSnapshotsProcessingPeriodSeconds =
                    (string)(defaultProperties["strSnapshotsProcessingPeriodSeconds"]);
                if (strSnapshotsProcessingPeriodSeconds == "0")
                {
                    SDCandTCCreadAndReportPeriod = TimeSpan.MaxValue;
                }
                else
                {
                    SDCandTCCreadAndReportPeriod = new TimeSpan(0, 0, Convert.ToInt32(strSnapshotsProcessingPeriodSeconds));
                }
            }
            else
            {
                int strSnapshotsProcessingPeriodSeconds = 300;
                SDCandTCCreadAndReportPeriod = new TimeSpan(0, 0, Convert.ToInt32(strCamShotPeriod));
                defaultProperties.Add("strSnapshotsProcessingPeriodSeconds", strSnapshotsProcessingPeriodSeconds);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion snapshotsProcessingPeriod






            //// makeCurrentSnapshotPreviewPicturePeriod
            //#region makeCurrentSnapshotPreviewPicturePeriod
            ////TimeSpan makeCurrentSnapshotPreviewPicturePeriod = new TimeSpan(0, 5, 0);
            //if (defaultProperties.ContainsKey("strmakeCurrentSnapshotPreviewPicturePeriodSeconds"))
            //{
            //    string strmakeCurrentSnapshotPreviewPicturePeriodSeconds =
            //        (string)(defaultProperties["strmakeCurrentSnapshotPreviewPicturePeriodSeconds"]);
            //    if (strmakeCurrentSnapshotPreviewPicturePeriodSeconds == "0")
            //    {
            //        makeCurrentSnapshotPreviewPicturePeriod = TimeSpan.MaxValue;
            //    }
            //    else
            //    {
            //        makeCurrentSnapshotPreviewPicturePeriod = new TimeSpan(0, 0, Convert.ToInt32(strmakeCurrentSnapshotPreviewPicturePeriodSeconds));
            //    }
            //}
            //else
            //{
            //    int strmakeCurrentSnapshotPreviewPicturePeriodSeconds = 90;
            //    makeCurrentSnapshotPreviewPicturePeriod = new TimeSpan(0, 0, Convert.ToInt32(strmakeCurrentSnapshotPreviewPicturePeriodSeconds));
            //    defaultProperties.Add("strmakeCurrentSnapshotPreviewPicturePeriodSeconds", strmakeCurrentSnapshotPreviewPicturePeriodSeconds);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}
            //#endregion makeCurrentSnapshotPreviewPicturePeriod



            #region restrictSnapshotsProcessingWhenSunElevationLowerThanZero
            if (defaultProperties.ContainsKey("restrictSnapshotsProcessingWhenSunElevationLowerThanZero"))
            {
                restrictSnapshotsProcessingWhenSunElevationLowerThanZero =
                    ((string)defaultProperties["restrictSnapshotsProcessingWhenSunElevationLowerThanZero"]).ToLower() ==
                    "true";
            }
            else
            {
                defaultProperties.Add("restrictSnapshotsProcessingWhenSunElevationLowerThanZero", restrictSnapshotsProcessingWhenSunElevationLowerThanZero);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion restrictSnapshotsProcessingWhenSunElevationLowerThanZero



            #region // bot server communication properties

            //#region bSendProcessedDataTo_CC_Moscow_bot_server
            ////private bool bSendProcessedDataTo_CC_Moscow_bot_server = false;
            //if (defaultProperties.ContainsKey("bSendProcessedDataTo_CC_Moscow_bot_server"))
            //{
            //    bSendProcessedDataTo_CC_Moscow_bot_server =
            //        ((string)defaultProperties["bSendProcessedDataTo_CC_Moscow_bot_server"]).ToLower() == "true";
            //}
            //else
            //{
            //    bSendProcessedDataTo_CC_Moscow_bot_server = false;
            //    defaultProperties.Add("bSendProcessedDataTo_CC_Moscow_bot_server",
            //        bSendProcessedDataTo_CC_Moscow_bot_server);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}
            //#endregion


            //#region strRemoteBotServerHost
            ////private string strRemoteBotServerHost = "krinitsky.ru";
            //if (defaultProperties.ContainsKey("strRemoteBotServerHost"))
            //{
            //    strRemoteBotServerHost = (string)defaultProperties["strRemoteBotServerHost"];
            //}
            //else
            //{
            //    strRemoteBotServerHost = "krinitsky.ru";
            //    defaultProperties.Add("strRemoteBotServerHost", strRemoteBotServerHost);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}


            //#endregion


            //#region intRemoteBotServerPort
            ////private int intRemoteBotServerPort = 22;

            //if (defaultProperties.ContainsKey("intRemoteBotServerPort"))
            //{
            //    intRemoteBotServerPort = Convert.ToInt32((string)defaultProperties["intRemoteBotServerPort"]);
            //}
            //else
            //{
            //    intRemoteBotServerPort = 22;
            //    defaultProperties.Add("intRemoteBotServerPort", intRemoteBotServerPort);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}

            //#endregion


            //#region strRemoteBotServerHostAuthKeyFile
            ////private string strRemoteBotServerHostAuthKeyFile = "";
            //if (defaultProperties.ContainsKey("strRemoteBotServerHostAuthKeyFile"))
            //{
            //    strRemoteBotServerHostAuthKeyFile = (string)defaultProperties["strRemoteBotServerHostAuthKeyFile"];
            //}
            //else
            //{
            //    strRemoteBotServerHostAuthKeyFile = "";
            //    defaultProperties.Add("strRemoteBotServerHostAuthKeyFile", "");
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}


            //#endregion strRemoteBotServerHostAuthKeyFile


            //#region strRemoteBotServerSSHusername

            //if (defaultProperties.ContainsKey("strRemoteBotServerSSHusername"))
            //{
            //    strRemoteBotServerSSHusername = (string)defaultProperties["strRemoteBotServerSSHusername"];
            //}
            //else
            //{
            //    strRemoteBotServerSSHusername = "mk";
            //    defaultProperties.Add("strRemoteBotServerSSHusername", strRemoteBotServerSSHusername);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}

            //#endregion strRemoteBotServerSSHusername



            //#region strAcceptedSSHhostCertPublicKeyFile

            //if (defaultProperties.ContainsKey("strAcceptedSSHhostCertPublicKeyFile"))
            //{
            //    strAcceptedSSHhostCertPublicKeyFile = (string)defaultProperties["strAcceptedSSHhostCertPublicKeyFile"];
            //}
            //else
            //{
            //    strAcceptedSSHhostCertPublicKeyFile = "";
            //    defaultProperties.Add("strAcceptedSSHhostCertPublicKeyFile", strAcceptedSSHhostCertPublicKeyFile);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}

            //#endregion



            #endregion // bot server communication properties



            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
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

            if (!dctCalibrationAccDataByDevID.ContainsKey("devID1"))
            {
                dctCalibrationAccDataByDevID.Add("devID1", accCalibrationDataID1);
            }
            if (!dctCalibrationAccDataByDevID.ContainsKey("devID2"))
            {
                dctCalibrationAccDataByDevID.Add("devID2", accCalibrationDataID2);
            }



            ThreadSafeOperations.SetText(lblAccelCalibrationXID1, Math.Round(accCalibrationDataID1.AccDoubleX, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationYID1, Math.Round(accCalibrationDataID1.AccDoubleY, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationZID1, Math.Round(accCalibrationDataID1.AccDoubleZ, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationXID2, Math.Round(accCalibrationDataID2.AccDoubleX, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationYID2, Math.Round(accCalibrationDataID2.AccDoubleY, 2).ToString(), false);
            ThreadSafeOperations.SetText(lblAccelCalibrationZID2, Math.Round(accCalibrationDataID2.AccDoubleZ, 2).ToString(), false);

            angleCamDeclinationThresholdRad = Math.PI * angleCamDeclinationThresholdDeg / 180.0d;

            dataToPassToSensorsDataPresentation[1] = accCalibrationDataID1;
            dataToPassToSensorsDataPresentation[3] = accCalibrationDataID2;
        }





        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }

        #endregion default properties read-write



        
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




        #endregion Form general features and behaviour






        #region search Arduino boards

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




        private void arduinoBoardSearchingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.ToggleButtonState(btnFindArduino1, true, "search for board ID1", true);
            ThreadSafeOperations.ToggleButtonState(btnFindArduino2, true, "search for board ID2", true);
            ThreadSafeOperations.SetLoadingCircleState(SearchingArduinoID1ProcessCircle, false, false, SearchingArduinoID1ProcessCircle.Color);
            ThreadSafeOperations.SetLoadingCircleState(SearchingArduinoID2ProcessCircle, false, false, SearchingArduinoID2ProcessCircle.Color);
        }


        #endregion search Arduino boards






        #region UDP catching job

        private static bool recievingUDPmessage = false;



        private void udpCatchingJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            udpCatchingJobResultObject workerResult = null;
            try
            {
                workerResult = ((object[])e.Result)[0] as udpCatchingJobResultObject;
            }
            catch (Exception ex)
            {
                ;
            }

            if (workerResult == null)
            {
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    LogMessageToLogWindow("UDP packets catching job quit unexpectedly." + Environment.NewLine +
                                          ServiceTools.CurrentCodeLineDescription());
                });
            }


            ThreadSafeOperations.ToggleButtonState(btnStartStopBdcstListening, true, "Start listening boards stream", false);
            ChangeIndicatingButtonBackgroundColor(btnStartStopBdcstListening,
                ButtonBackgroundStateWatchingProcess.notWatching);
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

            e.Result = new object[] {new udpCatchingJobResultObject()
            {
                properlyFinished = true
            } };
        }





        private class udpCatchingJobResultObject
        {
            public bool properlyFinished { get; set; }

            public udpCatchingJobResultObject()
            {
                properlyFinished = true;
            }
        }





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
                        LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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

        #endregion UDP catching job





        #region send commands to Arduinos and recieve replys

        /// <summary>
        /// Performs the send command.
        /// </summary>
        /// <param name="currCommand">The curr command.</param>
        /// <param name="devID">The dev identifier.</param>
        /// 
        /// TODO: this doesnt work. Fix it!
        /// 
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





        private void PerformRequestArduinoBoard(string requestText, int devID)
        {
            ArduinoRequestString = requestText;
            needsReplyOnRequest = true;
            PerformSendCommand(requestText, devID);
            ArduinoRequestExpectant.RunWorkerAsync();
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




        #endregion send commands to Arduinos and recieve replys





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
                udpCatchingJobShouldBeWorking = true;
            }
            else
            {
                dataCollector.CancelAsync();
                udpCatchingJobShouldBeWorking = false;
            }
        }










        #region обработка поступающих данных сенсоров

        private static int tailLength = 5;
        private FixedTimeQueue<AccelerometerData> accID1tail = new FixedTimeQueue<AccelerometerData>(tailLength);
        private FixedTimeQueue<AccelerometerData> accID2tail = new FixedTimeQueue<AccelerometerData>(tailLength);
        private FixedTimeQueue<GyroData> gyroID1tail = new FixedTimeQueue<GyroData>(tailLength);
        private FixedTimeQueue<GyroData> gyroID2tail = new FixedTimeQueue<GyroData>(tailLength);
        private DenseVector dvKernelFixedWidth = DataAnalysis.Extensions.ConvKernelAsymmetric(DataAnalysis.Extensions.StandardConvolutionKernels.gauss,
            tailLength, false);

        private List<AccelerometerData> dataToPassToSensorsDataPresentation = new List<AccelerometerData>()
        {
            new AccelerometerData(),
            new AccelerometerData(),
            new AccelerometerData(),
            new AccelerometerData()
        };
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

        private Stopwatch stwCamshotTimer = new Stopwatch();
        private Stopwatch stwSnapshotsProcessingTimer = new Stopwatch();
        //private Stopwatch stwMakeCurrentSnapshotPreviewPictureTimer = new Stopwatch();

        //DenseMatrix dmGyroDataMatrix = null;
        //List<long> gyroDateTimeValuesList = new List<long>();
        private TimeSeries<GyroData> tsCollectedGyroData = new TimeSeries<GyroData>();

        private TimeSeries<GPSdata> tsCollectedGPSdata = new TimeSeries<GPSdata>();

        TimeSeries<int> tsCollectedPressureData = new TimeSeries<int>();

        private bool udpCatchingJobShouldBeWorking = false;









        #region dataCollector BGW description

        private Timer tmrUDPcatchingJobIsWorkingCheck = null;
        private Timer tmrUpdateSensorsDataPresentation = null;
        private Timer tmrUDPpacketsRecievingSpeedEstimation = null;
        private Timer tmrReadAndReportSDCandCC = null;
        private Timer tmrOrganizeAndArchiveCollectedDataCheck = null;

        private void dataCollector_DoWork(object sender, DoWorkEventArgs e)
        {
            stwCamshotTimer.Start();
            stwSnapshotsProcessingTimer.Start();
            //stwMakeCurrentSnapshotPreviewPictureTimer.Start();


            TimeSpan labelsUpdatingPeriod = new TimeSpan(0, 0, 1);
            Stopwatch stwCamshotTimersUpdating = new Stopwatch();
            stwCamshotTimersUpdating.Start();


            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            ThreadSafeOperations.ToggleButtonState(btnStartStopCollecting, true, "stop collecting data", true);
            dataCollectingState = DataCollectingStates.working;




            Stopwatch stwToEstimateUDPpacketsRecieving = new Stopwatch();
            stwToEstimateUDPpacketsRecieving.Start();



            #region timer for periodical process of UDPpackets Recieving speed estimation

            tmrUDPpacketsRecievingSpeedEstimation = new Timer(EstimateAndReportUDPpacketsRecievingSpeed,
                new object[] { stwToEstimateUDPpacketsRecieving }, 0, 1000);

            #endregion timer for periodical process of UDPpackets Recieving speed estimation




            #region timer for periodical sensors values presentation updating

            tmrUpdateSensorsDataPresentation = new Timer(UpdateSensorsDataPresentation,
                dataToPassToSensorsDataPresentation, 0, 500);
            // every 500ms

            #endregion timer for periodical sensors values presentation updating




            #region timer for periodical udpCatchingJob working check

            tmrUDPcatchingJobIsWorkingCheck = new Timer(CheckIfUDPcatchingJobIsWorking,
                null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 23));
            //every 23 seconds

            #endregion timer for periodical udpCatchingJob working check



            #region timer for periodical check if its time to organize and archive collectied data

            tmrOrganizeAndArchiveCollectedDataCheck = new Timer(OrganizeAndArchiveCollectedDataCheck,
                null, 0, 600000);

            // each 10 minutes

            #endregion timer for periodical check if its time to organize and archive collectied data


            #region timer for periodical SDC and TCC check and report

            if (SDCandTCCreadAndReportPeriod < TimeSpan.MaxValue)
            {
                tmrReadAndReportSDCandCC = new Timer(ProcessCurrentSnapshot, null, 0, (int)SDCandTCCreadAndReportPeriod.TotalMilliseconds);
            }

            #endregion




            while (true)
            {
                #region CancellationPending

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

                #endregion CancellationPending


                if (stwCamshotTimersUpdating.Elapsed >= labelsUpdatingPeriod)
                {
                    updateTimersLabels(stwCamshotTimer, stwSnapshotsProcessingTimer, CamShotPeriod);
                    stwCamshotTimersUpdating.Restart();

                }


                Thread.Sleep(200);
            }

            tmrUDPpacketsRecievingSpeedEstimation.Change(Timeout.Infinite, Timeout.Infinite);
            tmrUDPpacketsRecievingSpeedEstimation.Dispose();
            tmrUDPpacketsRecievingSpeedEstimation = null;
            tmrUpdateSensorsDataPresentation.Change(Timeout.Infinite, Timeout.Infinite);
            tmrUpdateSensorsDataPresentation.Dispose();
            tmrUpdateSensorsDataPresentation = null;
            tmrUDPcatchingJobIsWorkingCheck.Change(Timeout.Infinite, Timeout.Infinite);
            tmrUDPcatchingJobIsWorkingCheck.Dispose();
            tmrUDPcatchingJobIsWorkingCheck = null;
            tmrOrganizeAndArchiveCollectedDataCheck.Change(Timeout.Infinite, Timeout.Infinite);
            tmrOrganizeAndArchiveCollectedDataCheck.Dispose();
            tmrOrganizeAndArchiveCollectedDataCheck = null;
            if (tmrReadAndReportSDCandCC != null)
            {
                tmrReadAndReportSDCandCC.Change(Timeout.Infinite, Timeout.Infinite);
                tmrReadAndReportSDCandCC.Dispose();
                tmrReadAndReportSDCandCC = null;
            }
            //if (tmrMakeCurrentSnapshotPreviewPicture != null)
            //{
            //    tmrMakeCurrentSnapshotPreviewPicture.Change(Timeout.Infinite, Timeout.Infinite);
            //    tmrMakeCurrentSnapshotPreviewPicture.Dispose();
            //}

            stwCamshotTimer.Stop();
            stwSnapshotsProcessingTimer.Stop();
            //stwMakeCurrentSnapshotPreviewPictureTimer.Stop();
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


            StopIPCpipeServer();
        }

        #endregion dataCollector BGW description






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

                ThreadSafeOperations.SetText(lblPressureValue, pressure.ToString(), false);

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
                    LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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
                Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar +
                "PressureDataLog-" +
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
                    LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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
                Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar +
                "GPSDataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
        }



        #endregion GPS data processing





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
            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedGyroData))
            {
                Thread.Sleep(0);
            }

            if (tsCollectedGyroData.Count == 0)
            {
                Monitor.Exit(tsCollectedGyroData);
                return;
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
                    LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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
                Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar +
                "GyroscopeDataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

        }


        #endregion Gyro data processing




        #region Accelerometers data processing

        private Dictionary<string, AccelerometerData> dctCalibrationAccDataByDevID =
            new Dictionary<string, AccelerometerData>();

        /// <summary>
        /// Handles the CollectionChanged event of the accDataAndDTObservableConcurrentQueue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
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


                // пересчитать хвост
                if (accData.devID <= 1)
                // 0 - если без разделения по DevID. например, когда установка только одна.
                // или 1 - если идет разделение.
                {
                    List<AccelerometerData> accDataTail = new List<AccelerometerData>(accID1tail.DataValues);
                    accDataTail.RemoveAll(accDt => accDt == null);

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

                    bool success = false;
                    while (!success)
                    {
                        if (dvKernelFixedWidth.Count != dvAccXvaluesTail.Count)
                        {
                            dvKernelFixedWidth = DataAnalysis.Extensions.ConvKernelAsymmetric(DataAnalysis.Extensions.StandardConvolutionKernels.gauss,
                                dvAccXvaluesTail.Count, false);
                        }

                        try
                        {
                            latestAccDataID1 = new AccelerometerData(dvKernelFixedWidth * dvAccXvaluesTail,
                                dvKernelFixedWidth * dvAccYvaluesTail, dvKernelFixedWidth * dvAccZvaluesTail)
                            {
                                devID = accData.devID
                            };
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            #region report
#if DEBUG
                            ServiceTools.ExecMethodInSeparateThread(this, () =>
                            {
                                LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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

                            success = false;
                        }
                    }
                }
                else
                {
                    List<AccelerometerData> accDataTail = new List<AccelerometerData>(accID2tail.DataValues);
                    accDataTail.RemoveAll(accDt => accDt == null);

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

                    bool success = false;
                    while (!success)
                    {
                        if (dvKernelFixedWidth.Count != dvAccXvaluesTail.Count)
                        {
                            dvKernelFixedWidth = DataAnalysis.Extensions.ConvKernelAsymmetric(DataAnalysis.Extensions.StandardConvolutionKernels.gauss,
                                dvAccXvaluesTail.Count, false);
                        }

                        try
                        {
                            latestAccDataID2 = new AccelerometerData(dvKernelFixedWidth * dvAccXvaluesTail,
                            dvKernelFixedWidth * dvAccYvaluesTail, dvKernelFixedWidth * dvAccZvaluesTail)
                            {
                                devID = accData.devID
                            };
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            #region report
#if DEBUG
                            ServiceTools.ExecMethodInSeparateThread(this, () =>
                            {
                                LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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

                            success = false;
                        }

                    }


                    //accDevAngle = (latestAccDataID2 * accCalibrationData) / (latestAccDataID2.AccMagnitude * accCalibrationData.AccMagnitude);
                    //accDevAngle = Math.Acos(accDevAngle);
                }




                #region if it is time to shoot - then shoot

                bool camshotTimeToGetIt = false;
                bool camshotHasBeenTaken = false;

                if (stwCamshotTimer.Elapsed >= CamShotPeriod)
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


                #region проверка на высоту солнца
                bool sunElevationMoreThanZero = true;
                if (latestGPSdata.validGPSdata && restrictDataRegistrationWhenSunElevationLowerThanZero)
                {
                    SPA spaCalc = new SPA(latestGPSdata.dateTimeUTC.Year, latestGPSdata.dateTimeUTC.Month, latestGPSdata.dateTimeUTC.Day, latestGPSdata.dateTimeUTC.Hour,
                            latestGPSdata.dateTimeUTC.Minute, latestGPSdata.dateTimeUTC.Second, (float)latestGPSdata.LonDec, (float)latestGPSdata.LatDec,
                            (float)SPAConst.DeltaT(latestGPSdata.dateTimeUTC));
                    int res = spaCalc.spa_calculate();
                    AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                        spaCalc.spa.zenith);

                    if (sunPositionSPAext.ElevationAngle <= 0.0d)
                    {
                        sunElevationMoreThanZero = false;
                    }
                }

                if (restrictDataRegistrationWhenSunElevationLowerThanZero)
                {
                    if (sunElevationMoreThanZero)
                    {
                        ThreadSafeOperations.SetText(lblWhetherRestrictsObtainingDataDueSunElevation, "Functioning normally.", false);
                    }
                    else
                    {
                        ThreadSafeOperations.SetText(lblWhetherRestrictsObtainingDataDueSunElevation, "PAUSED", false);
                    }
                }
                else
                {
                    ThreadSafeOperations.SetText(lblWhetherRestrictsObtainingDataDueSunElevation, "not restricted by sun elevation", false);
                }
                #endregion проверка на высоту солнца


                //if ((camshotInclinedProperly && camshotTimeToGetIt) || operatorCommandsToGetCamShotImmediately)
                if (camshotInclinedProperly && camshotTimeToGetIt && sunElevationMoreThanZero)
                {
                    EventHandler onNeedToShootCameraSnapshots = this.NeedToShootCameraSnapshots;
                    if (onNeedToShootCameraSnapshots != null) onNeedToShootCameraSnapshots(null, null);

                    //catchCameraImages();
                    camshotHasBeenTaken = true;
                }



                if (camshotHasBeenTaken)
                {
                    Note("Got a shot " + DateTime.Now);
                    //camshotInclinedProperly = false;
                    //camshotHasBeenTaken = false;
                    //camshotTimeToGetIt = false;
                    ThreadSafeOperations.ToggleLabelTextColor(lblNextShotIn, SystemColors.ControlText);
                    operatorCommandsToGetCamShot = false;
                    //operatorCommandsToGetCamShotImmediately = false;


                    // ПОМЕНЯТЬ ТУТ
                    logCurrentSensorsData();
                }


                #endregion if it is time to shoot - then shoot




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

                if (latestAccDataID1.AccMagnitude > 0.0d)
                {
                    //dataToPassToSensorsDataPresentation.Clear();
                    dataToPassToSensorsDataPresentation[0] = latestAccDataID1;
                    //dataToPassToSensorsDataPresentation.AddRange(new[] { latestAccDataID1, accCalibrationDataID1, latestAccDataID2, accCalibrationDataID2 });
                }


                if (latestAccDataID2.AccMagnitude > 0.0d)
                {
                    //dataToPassToSensorsDataPresentation.Clear();
                    dataToPassToSensorsDataPresentation[2] = latestAccDataID2;
                    //dataToPassToSensorsDataPresentation.AddRange(new[] { latestAccDataID1, accCalibrationDataID1, latestAccDataID2, accCalibrationDataID2 });
                }




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
                    LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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
                Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar +
                "AccelerometerDataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");

        }





        private void DumpCollectedAccelerometersSmoothedDataDevID1()
        {
            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedAccSmoothedDataID1))
            {
                Thread.Sleep(0);
            }

            if (tsCollectedAccSmoothedDataID1.Count == 0)
            {
                Monitor.Exit(tsCollectedAccSmoothedDataID1);
                return;
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
                    LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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
                Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar +
                "AccelerometerID1-Smoothed-DataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
        }





        private void DumpCollectedAccelerometersSmoothedDataDevID2()
        {
            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            while (!Monitor.TryEnter(tsCollectedAccSmoothedDataID2))
            {
                Thread.Sleep(0);
            }

            if (tsCollectedAccSmoothedDataID2.Count == 0)
            {
                Monitor.Exit(tsCollectedAccSmoothedDataID2);
                return;
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
                    LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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
                Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar +
                "AccelerometerID2-Smoothed-DataLog-" +
                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
        }




        #endregion Accelerometers data processing
        #endregion обработка поступающих данных сенсоров




        #region snapshots capturing

        private string swapImageToFile(Image image2write, String imageFNameAttrs = "")
        {
            if (!ServiceTools.CheckIfDirectoryExists(outputSnapshotsDirectory))
            {
                throw new Exception("couldn`t find or create output directory for snapshots:" + Environment.NewLine +
                                    outputSnapshotsDirectory);
            }

            String filename1 = outputSnapshotsDirectory + "img-" +
                               DateTime.UtcNow.ToString("s").Replace(":", "-");
            filename1 += imageFNameAttrs;
            filename1 += ".jpg";

            //Bitmap bm2write = new Bitmap(image2write);
            image2write.Save(filename1, System.Drawing.Imaging.ImageFormat.Jpeg);

            return filename1;
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
            DoWorkEventHandler currDoWorkHandler = delegate (object currBGWsender, DoWorkEventArgs args)
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

            RunWorkerCompletedEventHandler currWorkCompletedHandler = delegate (object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
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

        #endregion snapshots capturing




        #region service actions




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




                #region beep if permitted
                bool sunElevationMoreThanZero = true;
                if (latestGPSdata.validGPSdata)
                {
                    SPA spaCalc = new SPA(latestGPSdata.dateTimeUTC.Year, latestGPSdata.dateTimeUTC.Month, latestGPSdata.dateTimeUTC.Day, latestGPSdata.dateTimeUTC.Hour,
                            latestGPSdata.dateTimeUTC.Minute, latestGPSdata.dateTimeUTC.Second, (float)latestGPSdata.LonDec, (float)latestGPSdata.LatDec,
                            (float)SPAConst.DeltaT(latestGPSdata.dateTimeUTC));
                    int res = spaCalc.spa_calculate();
                    AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                        spaCalc.spa.zenith);

                    if (sunPositionSPAext.ElevationAngle <= 0.0d)
                    {
                        sunElevationMoreThanZero = false;
                    }
                }

                IsNowSilent = ForceBeepsSilent ||
                              (!sunElevationMoreThanZero && MakeBeepsSilentWhenSunElevationLowerThanZero);

                if (!IsNowSilent)
                {
                    SystemSounds.Beep.Play();
                }
                #endregion beep if permitted
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




        private void CheckIfUDPcatchingJobIsWorking(object state)
        {
            if (udpCatchingJob != null)
            {
                if (udpCatchingJobShouldBeWorking && !udpCatchingJob.IsBusy)
                {
                    btnStartStopBdcstListening_Click(null, null);
                }
            }
        }







        private void updateTimersLabels(Stopwatch stwCamshotTimer, Stopwatch stwSnapshotsProcessingTimer, TimeSpan camshotPeriod)
        {
            TimeSpan nextShotIn = camshotPeriod - stwCamshotTimer.Elapsed; //(nextshotAt - datetimeNow);
            ThreadSafeOperations.SetText(lblNextShotIn, nextShotIn.RoundToSeconds().ToString("c"), false);
            ThreadSafeOperations.SetText(lblSinceLastShot, stwCamshotTimer.Elapsed.RoundToSeconds().ToString("c"), false);

            TimeSpan nextSnapshotProcessingIn = SDCandTCCreadAndReportPeriod - stwSnapshotsProcessingTimer.Elapsed;
            ThreadSafeOperations.SetText(lblNextImageProcessingIn, nextSnapshotProcessingIn.RoundToSeconds().ToString("c"), false);
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





        private void OrganizeAndArchiveCollectedDataCheck(object state)
        {
            // check if current Sun altitude is less than zero and time is between sunset and sunrise
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


                // spaCalc.spa.sunset
                // spaCalc.spa.sunrise
                // sunPositionSPAext.ElevationAngle

                if (restrictDataRegistrationWhenSunElevationLowerThanZero && bOrganizeAndArchiveCollectedDataAtLocalMidnight)
                {
                    //calculate local midnight time
                    double timeMidnight = spaCalc.spa.sunrise - spaCalc.spa.sunset;
                    if (timeMidnight <= 0.0d) timeMidnight += 24.0d;
                    timeMidnight /= 2.0d;
                    timeMidnight += spaCalc.spa.sunset;
                    if (timeMidnight >= 24.0d)
                    {
                        timeMidnight -= 24.0d;
                    }
                    TimeOfDay midnightTOD = new TimeOfDay(timeMidnight);
                    DateTime dtMidnight = DateTime.UtcNow.Date +
                                          new TimeSpan(midnightTOD.hour, midnightTOD.minute, midnightTOD.second);

                    if (Math.Abs((DateTime.UtcNow - dtMidnight).TotalMinutes) <= 30)
                    {
                        //Process pProcess = new Process();
                        //pProcess.StartInfo.FileName = @"C:\Users\Vitor\ConsoleApplication1.exe";
                        //pProcess.StartInfo.Arguments = "olaa";
                        //pProcess.StartInfo.UseShellExecute = false;
                        //pProcess.StartInfo.RedirectStandardOutput = true;
                        //pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        //pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
                        //pProcess.Start();
                        //string output = pProcess.StandardOutput.ReadToEnd(); //The output result

                        LogMessageToLogWindow("Local time: " + DateTime.Now.ToString("u") + Environment.NewLine +
                                              "It could be the time to organize and archive collected data.");
                    }

                }
            }
        }






        private void logCurrentSensorsData()
        {
            String filename1 = strOutputConcurrentDataDirectory + "data-" + DateTime.UtcNow.ToString("o").Replace(":", "-") + ".xml";
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

        #endregion service actions






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
        /// 
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
                            LogMessageToLogWindow("exception has been thrown: " + ex.Message + Environment.NewLine +
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
                            GPSdata catchedGPSdata = new GPSdata(dataValuesString, GPSdatasources.CloudCamArduinoGPS,
                                DateTime.UtcNow);
                            if (catchedGPSdata.validGPSdata)
                            {
                                latestGPSdata = catchedGPSdata;

                                lowPriorityTaskFactory.StartNew(() =>
                                {
                                    string gpsSerialized = ServiceTools.XmlSerializeToString(latestGPSdata);

                                    BroadcastIPCpipeServerMessage("<gps>");
                                    BroadcastIPCpipeServerMessage(gpsSerialized);
                                    BroadcastIPCpipeServerMessage("</gps>");
                                });

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





        #region // current snapshot processing using server calculations


        //private void ProcessCurrentSnapshot(object state)
        //{
        //    // find last image
        //    // outputSnapshotsDirectory

        //    //List<FileDatetimeInfo> lImagesFilesInfo = Directory.GetFiles(outputSnapshotsDirectory, "*.jpg",
        //    //    SearchOption.AllDirectories)
        //    //    .ToList()
        //    //    .ConvertAll(
        //    //        strImageFileName =>
        //    //            new FileDatetimeInfo(strImageFileName, ServiceTools.DatetimeExtractionMethod.Filename, "????xxxxxxxxxxxxxxxxxxx*"));
        //    //string lastSnapshotFile =
        //    //    lImagesFilesInfo.Aggregate(
        //    //        (fInfo1, fInfo2) => (fInfo1.datetime <= fInfo2.datetime) ? (fInfo2) : (fInfo1)).filename;

        //    //ProcessCurrentSnapshotWithServer(lastSnapshotFile);

        //    Task.Run(() => ComputeAndReportCCandTCC());

        //    //ProcessCurrentSnapshotWithServerCalculations(outputSnapshotsDirectory)
        //}


        //private Ipport ipclient = null;
        //private Communication_ClientChecklist dctCommunicationChecklist = null;
        ////private async void ProcessCurrentSnapshotWithServerCalculations(string strSnapshotFilename)
        //private async void ProcessCurrentSnapshotWithServer(string strSnapshotFilename)
        //{
        //    string filenameToSend = strSnapshotFilename;
        //    // @"D:\_gulevlab\SkyImagesAnalysis_appData\_AI49-total\images\ai49-snapshots-2015-06-12\img-2015-06-12T17-09-18devID1.jpg";
        //    string concurrentDataXMLfilename =
        //        await
        //            CommonTools.FindConcurrentDataXMLfileAsync(filenameToSend, strOutputConcurrentDataDirectory, true,
        //                ServiceTools.DatetimeExtractionMethod.Filename);

        //    Zip zip = new Zip();
        //    string tempZipFilename = Path.GetTempPath();
        //    tempZipFilename += (tempZipFilename.Last() == Path.DirectorySeparatorChar)
        //        ? ("")
        //        : (Path.DirectorySeparatorChar.ToString());
        //    tempZipFilename += Path.GetFileNameWithoutExtension(filenameToSend) + ".zip";
        //    zip.ArchiveFile = tempZipFilename;
        //    zip.IncludeFiles(filenameToSend + " | " + concurrentDataXMLfilename);
        //    zip.Compress();

        //    theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        "zip file created: " + Environment.NewLine + tempZipFilename);


        //    FileInfo f = new FileInfo(tempZipFilename);
        //    //bytesToSendFileSize = Convert.ToInt32(f.Length);
        //    string fileMD5hashString = ServiceTools.CalculateMD5hashString(tempZipFilename);
        //    theLogWindow = ServiceTools.LogAText(theLogWindow, "file MD5 hash (UTF8-encoded): " + Environment.NewLine + fileMD5hashString);

        //    ipclient = new Ipport()
        //    {
        //        RemoteHost = strRemoteStatsCalculatingServerHost,
        //        RemotePort = intRemoteStatsCalculatingServerPort,
        //        Timeout = 180
        //    };
        //    ipclient.Config("OutBufferSize=4096");
        //    ipclient.OnConnected += Ipclient_OnConnected;
        //    ipclient.OnConnectionStatus += Ipclient_OnConnectionStatus;
        //    ipclient.OnDisconnected += Ipclient_OnDisconnected;
        //    ipclient.OnError += Ipclient_OnError;
        //    ipclient.OnDataIn += Ipclient_OnDataIn;

        //    ipclient.Connect(strRemoteStatsCalculatingServerHost, intRemoteStatsCalculatingServerPort);



        //    dctCommunicationChecklist = new Communication_ClientChecklist();
        //    dctCommunicationChecklist.strImageFilename = filenameToSend;
        //    dctCommunicationChecklist.strConcurrentDataXMLfilename = concurrentDataXMLfilename;

        //    ipclient.SendLine("<SendingFile>");
        //    Thread.Sleep(200);

        //    IPWorksFileSenderReceiver imageFileSender = new IPWorksFileSenderReceiver(ipclient,
        //        FileSenderReceiverRole.sender);
        //    imageFileSender.theLogWindow = theLogWindow;
        //    imageFileSender.fileSenderConnection = new FileSendingConnectionDescription()
        //    {
        //        ConnectionID = "",
        //        FileSenderCommunicationChecklist = new FileTransfer_SenderChecklist()
        //    };
        //    imageFileSender.FileSendingFinished += ImageFileSender_FileSendingFinished;
        //    imageFileSender.SendFile(tempZipFilename, Path.GetFileName(tempZipFilename));
        //}



        //private async void ImageFileSender_FileSendingFinished(object sender, FileTransferFinishedEventArgs e)
        //{
        //    File.Delete(e.fileTransferredFullName);

        //    theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        "All data has been sent. Waiting for GrIx,Y,R,G,B stats XML file returned");

        //    dctCommunicationChecklist.WaitingForResponceFile = true;
        //}





        //private IPWorksFileSenderReceiver receiver = null;
        //private void Ipclient_OnDataIn(object sender, IpportDataInEventArgs e)
        //{
        //    string TextReceived = e.Text;
        //    string FirstLine = TextReceived.Split(new string[] { Environment.NewLine },
        //        StringSplitOptions.RemoveEmptyEntries).First();
        //    FirstLine = FirstLine.Replace(Environment.NewLine, "");
        //    if (!dctCommunicationChecklist.WaitingForResponceFile)
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, FirstLine);
        //    }


        //    if (dctCommunicationChecklist.WaitingForResponceFile)
        //    {
        //        if (FirstLine == "<SendingFile>")
        //        {
        //            string CurDir = Directory.GetCurrentDirectory();
        //            string IncomingImagesBasePath = CurDir + Path.DirectorySeparatorChar + "IncomingFiles" + Path.DirectorySeparatorChar;
        //            ServiceTools.CheckIfDirectoryExists(IncomingImagesBasePath);

        //            // перевести в режим приема файла
        //            receiver = new IPWorksFileSenderReceiver(ipclient, FileSenderReceiverRole.receiver);
        //            receiver.IncomingsFilesBasePath = IncomingImagesBasePath;
        //            receiver.fileReceiverConnection = new FileReceivingConnectionDescription()
        //            {
        //                ConnectionID = "",
        //                FileReceiverCommunicationChecklist = new FileTransfer_ReceiverChecklist()
        //            };
        //            receiver.FileReceivingFinished += FileReceiver_FileReceivingFinished;

        //            // перенаправлять события с этиим ConnectionId на созданный receiver
        //            ipclient.OnDataIn -= Ipclient_OnDataIn;
        //            ipclient.OnDataIn += receiver.Ipclient_OnDataIn;
        //        }
        //    }
        //}




        //private void FileReceiver_FileReceivingFinished(object sender, FileTransferFinishedEventArgs e)
        //{
        //    dctCommunicationChecklist.responceFileReceived = true;
        //    receiver = null;
        //    ipclient.Disconnect();
        //    ipclient.Dispose();
        //    ipclient = null;

        //    if (e.fileTransferSuccess)
        //    {
        //        PredictAndReportSDCandCC(e.fileTransferredFullName);
        //    }


        //}




        //private void PredictAndReportSDCandCC(string serverResponceFile)
        //{
        //    Zip zipExtractor = new Zip();
        //    zipExtractor.ArchiveFile = serverResponceFile;
        //    string ExtractToPath = Path.GetDirectoryName(serverResponceFile);
        //    ExtractToPath += ((ExtractToPath.Last() == Path.DirectorySeparatorChar)
        //        ? ("")
        //        : (Path.DirectorySeparatorChar.ToString())) +
        //                     Path.GetFileNameWithoutExtension(serverResponceFile) +
        //                     Path.DirectorySeparatorChar;
        //    ServiceTools.CheckIfDirectoryExists(ExtractToPath);
        //    zipExtractor.ExtractToPath = ExtractToPath;
        //    zipExtractor.ExtractAll();
        //    zipExtractor.Dispose();

        //    string statsXMLfile =
        //        Directory.GetFiles(ExtractToPath,
        //            ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern()).First();

        //    string strToReport = "Received files: " + Environment.NewLine;
        //    strToReport += string.Join(Environment.NewLine, Directory.GetFiles(ExtractToPath));

        //    foreach (string strFilename in Directory.GetFiles(ExtractToPath))
        //    {
        //        string strMovedFilename = strOutputConcurrentDataDirectory + Path.GetFileName(strFilename);
        //        if (!File.Exists(strMovedFilename))
        //        {
        //            File.Move(strFilename, strMovedFilename);
        //        }
        //    }
        //    dctCommunicationChecklist.strReturnedStatsDataXMLfilename = strOutputConcurrentDataDirectory +
        //                                                                Path.GetFileName(statsXMLfile);

        //    File.Delete(serverResponceFile);
        //    Directory.Delete(ExtractToPath, true);

        //    theLogWindow = ServiceTools.LogAText(theLogWindow, strToReport);
        //    string CurDir = Directory.GetCurrentDirectory();
        //    string SDC_NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNconfig.csv";
        //    string SDC_NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNtrainedParameters.csv";
        //    string NormMeansFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormMeans.csv";
        //    string NormRangeFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormRange.csv";
        //    List<double> decisionProbabilities = new List<double>();
        //    SunDiskCondition sdc = SDCpredictorNN.CalcSDC_NN(dctCommunicationChecklist.strReturnedStatsDataXMLfilename,
        //        dctCommunicationChecklist.strConcurrentDataXMLfilename, SDC_NNconfigFile, SDC_NNtrainedParametersFile,
        //        NormMeansFile, NormRangeFile, out decisionProbabilities);



        //    string CC_NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "CC_NNconfig.csv";
        //    string CC_NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "CC_NNtrainedParameters.csv";
        //    int CloudCover = CCpredictorNN.CalcCC_NN(dctCommunicationChecklist.strReturnedStatsDataXMLfilename,
        //        dctCommunicationChecklist.strConcurrentDataXMLfilename, SDC_NNconfigFile, SDC_NNtrainedParametersFile,
        //        NormMeansFile, NormRangeFile, CC_NNconfigFile, CC_NNtrainedParametersFile);

        //    dctCommunicationChecklist = null;

        //    theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        "Detected Sun disk condition: " + sdc + Environment.NewLine + "Detected CC: " + CloudCover);

        //    ThreadSafeOperations.SetText(lblSDCvalue, sdc.ToString(), false);
        //    ThreadSafeOperations.SetText(lblCCvalue, CloudCover.ToString(), false);
        //}




        //private void Ipclient_OnError(object sender, IpportErrorEventArgs e)
        //{

        //    theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR: " + Environment.NewLine + e.Description);

        //}



        //private void Ipclient_OnDisconnected(object sender, IpportDisconnectedEventArgs e)
        //{
        //    ipclient.Dispose();
        //    theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        "Disconnected from remote host: " + Environment.NewLine + e.Description);
        //}


        //private void Ipclient_OnConnectionStatus(object sender, IpportConnectionStatusEventArgs e)
        //{

        //    theLogWindow = ServiceTools.LogAText(theLogWindow, e.Description);

        //}


        //private void Ipclient_OnConnected(object sender, IpportConnectedEventArgs e)
        //{
        //    theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        "Connected to remote host: " + Environment.NewLine + e.Description);
        //}


        #endregion // current snapshot processing using server calculations






        #region periodical snapshot processing for SDC and TCC prediction

        private void ProcessCurrentSnapshot(object state)
        {
            LogMessageToLogWindow("");

            stwSnapshotsProcessingTimer.Restart();

            #region проверка на высоту солнца
            bool sunElevationMoreThanZero = true;
            if (latestGPSdata.validGPSdata && restrictSnapshotsProcessingWhenSunElevationLowerThanZero)
            {
                SPA spaCalc = new SPA(latestGPSdata.dateTimeUTC.Year, latestGPSdata.dateTimeUTC.Month, latestGPSdata.dateTimeUTC.Day, latestGPSdata.dateTimeUTC.Hour,
                        latestGPSdata.dateTimeUTC.Minute, latestGPSdata.dateTimeUTC.Second, (float)latestGPSdata.LonDec, (float)latestGPSdata.LatDec,
                        (float)SPAConst.DeltaT(latestGPSdata.dateTimeUTC));
                int res = spaCalc.spa_calculate();
                AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                    spaCalc.spa.zenith);

                if (sunPositionSPAext.ElevationAngle <= 0.0d)
                {
                    sunElevationMoreThanZero = false;
                }
            }

            #endregion проверка на высоту солнца

            if (sunElevationMoreThanZero)
            {
                lowPriorityTaskFactory.StartNew(ReadAndReportLatestCCandTCC);
            }
        }




        //private void btnCCandTCC_Click(object sender, EventArgs e)
        //{
        //    if (theLogWindow == null)
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, "");
        //    }
        //    // Task.Run(() => ComputeAndReportCCandTCC());
        //    lowPriorityTaskFactory.StartNew(() => ComputeAndReportCCandTCC());
        //}





        private async void ReadAndReportLatestCCandTCC()
        {
            List<FileInfo> lInfos =
                new DirectoryInfo(strOutputConcurrentDataDirectory).GetFiles(
                    ConventionalTransitions.ImageProcessedAndPredictedDataFileNamesPattern(),
                    SearchOption.AllDirectories).ToList();

            if (lInfos.Count == 0)
            {
                return;
            }

            lInfos.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
            string lastFilename = lInfos.Last().FullName;

            try
            {
                SkyImagesProcessedAndPredictedData data =
                    (SkyImagesProcessedAndPredictedData)
                        ServiceTools.ReadObjectFromXML(lastFilename, typeof(SkyImagesProcessedAndPredictedData));

                // ThreadSafeOperations.SetText(lblSDCvalue, data.PredictedSDC.ToString(), false);
                foreach (SDCdecisionProbability sdcDecisionProbability in data.sdcDecisionProbabilities)
                {
                    Label lblToSet = lblNoSunProb;
                    switch (sdcDecisionProbability.sdc)
                    {
                        case SunDiskCondition.NoSun:
                            lblToSet = lblNoSunProb;
                            break;
                        case SunDiskCondition.Sun0:
                            lblToSet = lblSun0Prob;
                            break;
                        case SunDiskCondition.Sun1:
                            lblToSet = lblSun1Prob;
                            break;
                        case SunDiskCondition.Sun2:
                            lblToSet = lblSun2Prob;
                            break;
                        default:
                            lblToSet = lblNoSunProb;
                            break;
                    }
                    ThreadSafeOperations.SetText(lblToSet,
                        (sdcDecisionProbability.sdcDecisionProbability*100.0d).ToString("F2") + "%", false);
                }

                ThreadSafeOperations.SetText(lblCCvalue, data.PredictedCC.CloudCoverTotal.ToString() + " (/8)", false);
            }
            catch (Exception ex)
            {
                return;
            }
        }



        #region // PredictAndReportSDCandCC - obsolete (separate app)

        //private void PredictAndReportSDCandCC(SkyImageIndexesStatsData statsData, ConcurrentData nearestConcurrentData, out SunDiskCondition sdc, out int TCC)
        //{
        //    string CurDir = Directory.GetCurrentDirectory();
        //    string SDC_NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNconfig.csv";
        //    string SDC_NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNtrainedParameters.csv";
        //    string NormMeansFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormMeans.csv";
        //    string NormRangeFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormRange.csv";
        //    List<double> decisionProbabilities = new List<double>();

        //    //SunDiskCondition sdc = SDCpredictorNN.CalcSDC_NN(dctCommunicationChecklist.strReturnedStatsDataXMLfilename,
        //    //    dctCommunicationChecklist.strConcurrentDataXMLfilename, SDC_NNconfigFile, SDC_NNtrainedParametersFile,
        //    //    NormMeansFile, NormRangeFile, out decisionProbabilities);
        //    DenseVector dvMeans = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormMeansFile, 0, ",")).Row(0);
        //    DenseVector dvRanges = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormRangeFile, 0, ",")).Row(0);
        //    DenseVector dvThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(SDC_NNtrainedParametersFile, 0, ",");
        //    List<int> NNlayersConfig =
        //        new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(SDC_NNconfigFile, 0, ",")).Row(0)).ConvertAll
        //            (dVal => Convert.ToInt32(dVal));

        //    sdc = SDCpredictorNN.PredictSDC_NN(statsData, nearestConcurrentData, NNlayersConfig, dvThetaValues, dvMeans,
        //        dvRanges, out decisionProbabilities);



        //    string CC_NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "CC_NNconfig.csv";
        //    string CC_NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "CC_NNtrainedParameters.csv";
        //    DenseVector dv_CC_ThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(CC_NNtrainedParametersFile, 0, ",");
        //    List<int> CC_NNlayersConfig =
        //        new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(CC_NNconfigFile, 0, ",")).Row(0)).ConvertAll
        //            (dVal => Convert.ToInt32(dVal));

        //    TCC = CCpredictorNN.PredictCC_NN(statsData, nearestConcurrentData, NNlayersConfig, dvThetaValues, dvMeans,
        //        dvRanges, CC_NNlayersConfig, dv_CC_ThetaValues);

        //    theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        "Detected Sun disk condition: " + sdc + Environment.NewLine + "Detected CC: " + TCC);

        //    ThreadSafeOperations.SetText(lblSDCvalue, sdc.ToString(), false);
        //    ThreadSafeOperations.SetText(lblCCvalue, TCC.ToString() + " (/8)", false);
        //}

        #endregion // PredictAndReportSDCandCC - obsolete (separate app)


        #endregion periodical snapshot processing for SDC and TCC prediction







        #region // btnSSHsendPreview_Click - obsolete

        //private void btnSSHsendPreview_Click(object sender, EventArgs e)
        //{
        //    if (theLogWindow == null)
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow);
        //    }

        //    lowPriorityTaskFactory.StartNew(() => MakeAndSendSnapshotsPreviewPictures());
        //}

        #endregion // btnSSHsendPreview_Click - obsolete




        #region // MakeCurrentSnapshotPreviewPicture = obsolete

        //private void MakeCurrentSnapshotPreviewPicture(object state)
        //{
        //    // stwMakeCurrentSnapshotPreviewPictureTimer.Restart();

        //    #region проверка на высоту солнца
        //    bool sunElevationMoreThanZero = true;
        //    if (latestGPSdata.validGPSdata && restrictSnapshotsProcessingWhenSunElevationLowerThanZero)
        //    {
        //        SPA spaCalc = new SPA(latestGPSdata.dateTimeUTC.Year, latestGPSdata.dateTimeUTC.Month, latestGPSdata.dateTimeUTC.Day, latestGPSdata.dateTimeUTC.Hour,
        //                latestGPSdata.dateTimeUTC.Minute, latestGPSdata.dateTimeUTC.Second, (float)latestGPSdata.LonDec, (float)latestGPSdata.LatDec,
        //                (float)SPAConst.DeltaT(latestGPSdata.dateTimeUTC));
        //        int res = spaCalc.spa_calculate();
        //        AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
        //            spaCalc.spa.zenith);

        //        if (sunPositionSPAext.ElevationAngle <= 0.0d)
        //        {
        //            sunElevationMoreThanZero = false;
        //        }
        //    }

        //    #endregion проверка на высоту солнца

        //    if (sunElevationMoreThanZero)
        //    {
        //        lowPriorityTaskFactory.StartNew(() => MakeAndSendSnapshotsPreviewPictures());

        //    }
        //}

        #endregion // MakeCurrentSnapshotPreviewPicture = obsolete





        #region // MakeAndSendSnapshotsPreviewPictures - obsolete

        //private async void MakeAndSendSnapshotsPreviewPictures()
        //{

        //    if (bSendProcessedDataTo_CC_Moscow_bot_server)
        //    {
        //        #region make and send snapshots preview and lufft data XML files

        //        string FilenameToSend = "";
        //        Image<Bgr, byte> lastImagesCouple = CurrentImagesCouple(out FilenameToSend);
        //        string tmpFNameToSave = Path.GetTempPath();
        //        tmpFNameToSave += (tmpFNameToSave.Last() == Path.DirectorySeparatorChar)
        //            ? ("")
        //            : (Path.DirectorySeparatorChar.ToString());
        //        tmpFNameToSave += FilenameToSend;
        //        lastImagesCouple.Save(tmpFNameToSave);
        //        // var fileStream = File.Open(tmpFNameToSave, FileMode.Open);
        //        // Message sentMessage = await Bot.SendPhoto(update.Message.Chat.Id, new FileToSend(FilenameToSend, fileStream));
        //        // послать файл на сервер бота


        //        string filenameToSend = tmpFNameToSave;
        //        string concurrentDataXMLfilename =
        //            await
        //                CommonTools.FindConcurrentDataXMLfileAsync(filenameToSend, strOutputConcurrentDataDirectory,
        //                    true,
        //                    ServiceTools.DatetimeExtractionMethod.Filename);

        //        Zip zip = new Zip();
        //        string tempZipFilename = Path.GetTempPath();
        //        tempZipFilename += (tempZipFilename.Last() == Path.DirectorySeparatorChar)
        //            ? ("")
        //            : (Path.DirectorySeparatorChar.ToString());
        //        tempZipFilename += Path.GetFileNameWithoutExtension(filenameToSend) + ".zip";
        //        zip.ArchiveFile = tempZipFilename;
        //        zip.IncludeFiles(filenameToSend + " | " + concurrentDataXMLfilename);
        //        zip.Compress();

        //        //theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        //    "zip file created: " + Environment.NewLine + tempZipFilename);

        //        Exception retEx = null;
        //        bool sendingResult = SendFileToBotServer(tempZipFilename, "~/cc_moscow_bot/" + Path.GetFileName(tempZipFilename), out retEx);

        //        File.Delete(tmpFNameToSave);
        //        File.Delete(tempZipFilename);

        //        #endregion make and send snapshots preview and lufft data XML files

        //        #region report error
        //        if (!sendingResult)
        //        {
        //            if (theLogWindow != null)
        //            {
        //                theLogWindow = ServiceTools.LogAText(theLogWindow,
        //                    "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
        //                    Environment.NewLine + "Exception messages: " + Environment.NewLine +
        //                    ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
        //                    ServiceTools.CurrentCodeLineDescription());
        //            }
        //            else
        //            {
        //                ServiceTools.logToTextFile(errorLogFilename,
        //                    "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
        //                    Environment.NewLine + "Exception messages: " + Environment.NewLine +
        //                    ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
        //                    ServiceTools.CurrentCodeLineDescription());
        //            }

        //            return;
        //        }
        //        #endregion report error

        //        List<string> commands = new List<string>();
        //        commands.Add("cd ~/cc_moscow_bot/");
        //        commands.Add("unzip " + Path.GetFileName(tempZipFilename));
        //        commands.Add("rm " + Path.GetFileName(tempZipFilename));
        //        commands.Add("ll");
        //        bool execResult = ExecSShellCommandsOnBotServer(commands, out retEx);

        //        #region report error

        //        if (!execResult)
        //        {
        //            if (theLogWindow != null)
        //            {
        //                theLogWindow = ServiceTools.LogAText(theLogWindow,
        //                    "ERROR executing commands on bot server" + Environment.NewLine +
        //                    Environment.NewLine + "Exception messages: " + Environment.NewLine +
        //                    ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
        //                    ServiceTools.CurrentCodeLineDescription());
        //            }
        //            else
        //            {
        //                ServiceTools.logToTextFile(errorLogFilename,
        //                    "ERROR executing commands on bot server" + Environment.NewLine +
        //                    Environment.NewLine + "Exception messages: " + Environment.NewLine +
        //                    ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
        //                    ServiceTools.CurrentCodeLineDescription());
        //            }
        //            return;
        //        }

        //        #endregion

        //    }
        //}

        #endregion // MakeAndSendSnapshotsPreviewPictures - obsolete


        #region // ObtainLatestMeteoParameters - obsolete

        //private async Task<string> ObtainLatestMeteoParameters()
        //{
        //    string retStr = "";


        //    // WSLufftUMBappPath
        //    // Date time ; Temperature [°C] ; Abs. air pressure [hPa] ; Relative humidity [%] ; Abs. humidity [g/m³]
        //    if (Directory.Exists(WSLufftUMBappPath))
        //    {
        //        List<FileInfo> lTXTdataFilesInfoList =
        //            ((new DirectoryInfo(WSLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
        //                SearchOption.AllDirectories)).ToList();
        //        lTXTdataFilesInfoList.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
        //        FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
        //        List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true, ";");
        //        List<string> lastWSdataStrings = Contents.Last();
        //        retStr +=
        //            "WS:" + Environment.NewLine +
        //            "Date time: " + lastWSdataStrings[0] + Environment.NewLine +
        //            "Temperature [°C]: " + lastWSdataStrings[1] + Environment.NewLine +
        //            "Abs. air pressure [hPa]: " + lastWSdataStrings[2] + Environment.NewLine +
        //            "Relative humidity [%]: " + lastWSdataStrings[3] + Environment.NewLine +
        //            "Abs. humidity [g/m³]" + lastWSdataStrings[4] + Environment.NewLine + Environment.NewLine;
        //        // retStr += string.Join(" ; ", Contents.Last()) + Environment.NewLine;
        //    }


        //    // R2SLufftUMBappPath
        //    if (Directory.Exists(R2SLufftUMBappPath))
        //    {
        //        List<FileInfo> lTXTdataFilesInfoList =
        //            ((new DirectoryInfo(R2SLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
        //                SearchOption.AllDirectories)).ToList();
        //        lTXTdataFilesInfoList.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
        //        FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
        //        List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true, ";");

        //        List<string> lastR2SdataStrings = Contents.Last();


        //        retStr +=
        //            "R2S:" + Environment.NewLine +
        //            "Date,time:" + lastR2SdataStrings[0] + Environment.NewLine +
        //            "Precipitation absol. [mm]: " + lastR2SdataStrings[1] + Environment.NewLine +
        //            "Precipitation type: " + lastR2SdataStrings[2] + Environment.NewLine +
        //            "Ambient temperature [°C]" + lastR2SdataStrings[3] + Environment.NewLine +
        //            "Precipitat.intensity [mil/h]: " + lastR2SdataStrings[4] + Environment.NewLine + Environment.NewLine;
        //        //retStr += string.Join(" ; ", Contents.Last()) + Environment.NewLine;
        //    }



        //    // VentusLufftUMBappPath
        //    // Date time ; Virtual temperature [°C] ; Wind speed [m/s] ; Wind speed [m/s] Vect. ; Wind direction [°] ; Wind direction [°] Vect. ; Abs. air pressure [hPa] ; Wind value quality [%]
        //    if (Directory.Exists(VentusLufftUMBappPath))
        //    {
        //        List<FileInfo> lTXTdataFilesInfoList =
        //            ((new DirectoryInfo(VentusLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
        //                SearchOption.AllDirectories)).ToList();
        //        lTXTdataFilesInfoList.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
        //        FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
        //        List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true, ";");
        //        List<string> lastVentusdataStrings = Contents.Last();
        //        retStr +=
        //            "Ventus:" + Environment.NewLine +
        //            "Date,time: " + lastVentusdataStrings[0] + Environment.NewLine +
        //            "Virtual temperature [°C]: " + lastVentusdataStrings[1] + Environment.NewLine +
        //            "Wind speed [m/s]: " + lastVentusdataStrings[2] + Environment.NewLine +
        //            "Wind speed [m/s] Vect.:" + lastVentusdataStrings[3] + Environment.NewLine +
        //            "Wind direction [°]: " + lastVentusdataStrings[4] + Environment.NewLine +
        //            "Wind direction [°] Vect.: " + lastVentusdataStrings[5] + Environment.NewLine +
        //            "Abs. air pressure [hPa]: " + lastVentusdataStrings[6] + Environment.NewLine +
        //            "Wind value quality [%]: " + lastVentusdataStrings[7] + Environment.NewLine + Environment.NewLine;
        //        //retStr += string.Join(" ; ", Contents.Last()) + Environment.NewLine;
        //    }

        //    return retStr;
        //}

        #endregion




        #region // exec Sshell commands methods

        //private bool ExecSShellCommandsOnBotServer(List<string> commands, out Exception retEx)
        //{
        //    retEx = null;

        //    Sshell sh = new Sshell()
        //    {
        //        SSHHost = strRemoteBotServerHost,
        //        SSHPort = intRemoteBotServerPort,
        //        SSHAuthMode = SshellSSHAuthModes.amPublicKey,
        //        SSHUser = strRemoteBotServerSSHusername
        //    };

        //    try
        //    {
        //        sh.SSHCert = new Certificate(CertStoreTypes.cstPPKFile, strRemoteBotServerHostAuthKeyFile, "", "*");
        //    }
        //    catch (Exception ex)
        //    {
        //        retEx = ex;
        //        return false;
        //    }

        //    try
        //    {
        //        sh.SSHAcceptServerHostKey = new Certificate(CertStoreTypes.cstSSHPublicKeyFile,
        //            strAcceptedSSHhostCertPublicKeyFile, "", "*");
        //    }
        //    catch (Exception ex)
        //    {
        //        retEx = ex;
        //        return false;
        //    }


        //    sh.OnSSHServerAuthentication += Sh_OnSSHServerAuthentication;
        //    sh.OnStdout += Sh_OnStdout;

        //    foreach (string command in commands)
        //    {
        //        sh.Execute(command);
        //    }

        //    return true;
        //}



        //private void Sh_OnStdout(object sender, SshellStdoutEventArgs e)
        //{
        //    theLogWindow = ServiceTools.LogAText(theLogWindow, e.Text);
        //}



        //private void Sh_OnSSHServerAuthentication(object sender, SshellSSHServerAuthenticationEventArgs e)
        //{
        //    Sshell sh = sender as Sshell;
        //    if (!e.Accept)
        //    {
        //        try
        //        {
        //            sh.Interrupt();
        //            sh.SSHLogoff();
        //            sh.Dispose();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        #endregion exec Sshell commands methods




        #region // scp file methods - obsolete

        //private bool SendFileToBotServer(string localFile, string remoteFile, out Exception retEx)
        //{
        //    retEx = null;
        //    if (!File.Exists(strRemoteBotServerHostAuthKeyFile))
        //    {
        //        retEx =
        //            new FileNotFoundException("unable to locate bot server auth key file: " +
        //                                      strRemoteBotServerHostAuthKeyFile);
        //        return false;
        //    }

        //    if (!File.Exists(strAcceptedSSHhostCertPublicKeyFile))
        //    {
        //        retEx =
        //            new FileNotFoundException("unable to locate bot server accepted host key file: " +
        //                                      strAcceptedSSHhostCertPublicKeyFile);
        //        return false;
        //    }



        //    Scp scp = new Scp()
        //    {
        //        SSHHost = strRemoteBotServerHost,
        //        SSHPort = intRemoteBotServerPort,
        //        SSHAuthMode = ScpSSHAuthModes.amPublicKey,
        //        SSHUser = strRemoteBotServerSSHusername
        //    };


        //    try
        //    {
        //        scp.SSHCert = new Certificate(CertStoreTypes.cstPPKFile, strRemoteBotServerHostAuthKeyFile, "", "*");
        //    }
        //    catch (Exception ex)
        //    {
        //        retEx = ex;
        //        return false;
        //    }



        //    try
        //    {
        //        scp.SSHAcceptServerHostKey = new Certificate(CertStoreTypes.cstSSHPublicKeyFile,
        //            strAcceptedSSHhostCertPublicKeyFile, "", "*");
        //    }
        //    catch (Exception ex)
        //    {
        //        retEx = ex;
        //        return false;
        //    }


        //    scp.OnSSHServerAuthentication += Scp_OnSSHServerAuthentication;
        //    scp.RemoteFile = remoteFile;
        //    scp.LocalFile = localFile;
        //    scp.OnEndTransfer += Scp_OnEndTransfer;

        //    try
        //    {
        //        scp.Upload();
        //    }
        //    catch (Exception ex)
        //    {
        //        retEx = ex;
        //        return false;
        //    }

        //    return true;

        //}


        //private void Scp_OnEndTransfer(object sender, ScpEndTransferEventArgs e)
        //{
        //    if (theLogWindow != null)
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, "file " + e.LocalFile + " transfer finished");
        //    }
        //}



        //private void Scp_OnSSHServerAuthentication(object sender, ScpSSHServerAuthenticationEventArgs e)
        //{
        //    Scp scpSender = sender as Scp;
        //    if (!e.Accept)
        //    {
        //        try
        //        {
        //            scpSender.Interrupt();
        //            scpSender.SSHLogoff();
        //            scpSender.Dispose();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        //#endregion scp file methods




        //private Image<Bgr, byte> CurrentImagesCouple(out string FilenameToSend)
        //{
        //    DirectoryInfo dir = new DirectoryInfo(outputSnapshotsDirectory);
        //    List<FileInfo> lImagesFilesID1 = dir.GetFiles("*devID1.jpg", SearchOption.AllDirectories).ToList();
        //    lImagesFilesID1.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
        //    string strLastImageID1Fname = lImagesFilesID1.Last().FullName;

        //    FilenameToSend = strLastImageID1Fname.Replace("devID1", "");
        //    FilenameToSend = Path.GetFileName(FilenameToSend);

        //    List<FileInfo> lImagesFilesID2 = dir.GetFiles("*devID2.jpg", SearchOption.AllDirectories).ToList();
        //    lImagesFilesID2.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
        //    string strLastImageID2Fname = lImagesFilesID2.Last().FullName;

        //    Image<Bgr, byte> img1 = new Image<Bgr, byte>(strLastImageID1Fname);
        //    Image<Bgr, byte> img2 = new Image<Bgr, byte>(strLastImageID2Fname);

        //    Size img1Size = img1.Size;
        //    Size resimgSize = new Size(img1Size.Width * 2, img1Size.Height);

        //    Image<Bgr, byte> resImg = new Image<Bgr, byte>(resimgSize);
        //    Graphics g = Graphics.FromImage(resImg.Bitmap);
        //    g.DrawImage(img1.Bitmap, new Point(0, 0));
        //    g.DrawImage(img2.Bitmap, new Point(img1Size.Width, 0));

        //    resImg = resImg.Resize(0.25d, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

        //    return resImg;
        //}




        #endregion periodical data processing for bot snapshots





        #region IPC pipeserver

        private void StartIPCpipeServer()
        {
            //Create a PipeServer and bind the events.
            ipcPipeserver = new Pipeserver();
            ipcPipeserver.OnDataIn += IpcPipeserver_OnDataIn;
            ipcPipeserver.OnConnected += IpcPipeserver_OnConnected;
            ipcPipeserver.OnDisconnected += IpcPipeserver_OnDisconnected;

            //Set the PipeName, DefaultEOL, and start the server.
            ipcPipeserver.PipeName = "DataCollectorAutomator2G_IPCpipeserver";
            ipcPipeserver.DefaultEOL = Environment.NewLine; //optional
            ipcPipeserver.Listening = true;

            ThreadSafeOperations.SetText(lblIPCpipeServerStatus, "ON", false);
        }



        private void IpcPipeserver_OnDisconnected(object sender, PipeserverDisconnectedEventArgs e)
        {
            LogMessageToLogWindow("Client " + e.ConnectionId + " connected.");

            ThreadSafeOperations.SetText(lblIPCpipeServerStatus, "ON (" + ((Pipeserver)sender).Connections.Count + ")",
                false);
        }



        private void IpcPipeserver_OnConnected(object sender, PipeserverConnectedEventArgs e)
        {
            LogMessageToLogWindow("Client " + e.ConnectionId + " connected.");

            ThreadSafeOperations.SetText(lblIPCpipeServerStatus, "ON (" + ((Pipeserver) sender).Connections.Count + ")",
                false);
        }



        private void StopIPCpipeServer()
        {
            if (ipcPipeserver != null)
            {
                if (ipcPipeserver.Listening)
                {
                    ipcPipeserver.Shutdown();

                    ThreadSafeOperations.SetText(lblIPCpipeServerStatus, "OFF", false);
                }
            }
        }



        private void IpcPipeserver_OnDataIn(object sender, PipeserverDataInEventArgs e)
        {
            LogMessageToLogWindow("Echo '" + e.Text + "' from " + e.ConnectionId);
            
        }



        private async void BroadcastIPCpipeServerMessage(string strBroadcast)
        {
            //To broadcast a message to all connected clients, iterate through the
            //Connections collection and set DataToSend for each connection.
            if (CheckIfIPCpipeServerIsOn())
            {
                foreach (PipeConnection connection in ipcPipeserver.Connections.Values)
                {
                    connection.DataToSend = strBroadcast + Environment.NewLine;
                }
            }
        }



        private bool CheckIfIPCpipeServerIsOn()
        {
            if (ipcPipeserver == null) return false;
            if (!ipcPipeserver.Listening) return false;
            if (ipcPipeserver.Connections.Count == 0) return false;

            return true;
        }


        #endregion



        private void btnTestGPSsending_Click(object sender, EventArgs e)
        {
            string gpsSerialized = ServiceTools.XmlSerializeToString(new GPSdata(45.0d, 55.0d));

            BroadcastIPCpipeServerMessage("<gps>");
            BroadcastIPCpipeServerMessage(gpsSerialized);
            BroadcastIPCpipeServerMessage("</gps>");
        }
    }

    #endregion the form class
}
