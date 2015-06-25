using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
//using IoffeVesselDataReader;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using SkyImagesAnalyzer;
using SkyImagesAnalyzerLibraries;
using SolarPositioning;
using System.Collections.Specialized;



namespace IofffeVesselInfoStream
{
    public partial class MainForm : Form
    {
        private static ObservableConcurrentQueue<string> cquStreamTextStrings = new ObservableConcurrentQueue<string>();
        private Socket s;
        private Socket scktSeaSave;

        //private Queue<GPSdata> quGPSDataQueue = new Queue<GPSdata>();
        private static ObservableConcurrentQueue<GPSdata> cquGPSDataQueue = new ObservableConcurrentQueue<GPSdata>();
        //private Queue<MeteoData> quMeteoDataQueue = new Queue<MeteoData>();
        private ObservableConcurrentQueue<MeteoData> cquMeteoDataQueue = new ObservableConcurrentQueue<MeteoData>();

        private static GPSdata actualGPSdata = null;
        private static MeteoData actualMeteoData = null;

        private static GeoTrackRenderer geoRenderer = new GeoTrackRenderer(Directory.GetCurrentDirectory() + "\\geogrid\\regrid.nc");

        private Dictionary<string, object> defaultProperties = null;
        private string defaultPropertiesXMLfileName = "";

        private string bcstServerIP = "";
        private int bcstServerPort = 0;

        private static bool showGeoTrack = false;
        private int updateGeoTrackPeriodSec = 60;

        private BackgroundWorker bgwGraphsPresenter = null;
        private static bool showGraphs = false;
        private int graphsUpdatingPeriodSec = 60;
        private static Image<Bgr, byte> graphImage = null;
        private bool defaultGraphsTimeSpan = true;
        private Tuple<DateTime, DateTime> graphsTimeSpan = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow);

        private static LogWindow theLogWindow = null;

        //private static LogWindow theLogWindow = null;
        //private static Queue<string> quTextToLog = new Queue<string>();



        public MainForm()
        {
            InitializeComponent();
        }





        #region Connection listener

        private Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;
            IPAddress address;
            IPAddress.TryParse(server, out address);

            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                tempSocket.Connect(ipe);
            }
            catch (Exception)
            {
                MessageBox.Show("====== couldn`t connect to remote server ======", "ERROR", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                //ThreadSafeOperations.SetTextTB(tbLog, "====== coudn`t connect to remote server ======" + Environment.NewLine, true);
                return null;
            }


            if (tempSocket.Connected)
            {
                //ThreadSafeOperations.SetTextTB(tbLog, "====== connected to remote server " + server + " ======" + Environment.NewLine, true);
                s = tempSocket;
            }
            else
            {
                return null;
            }

            return s;
        }



        private void bgwSocketStreamReader_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            Socket sckt = ((object[])e.Argument)[0] as Socket;


            Byte[] bytesReceived = new Byte[1024];

            int bytes = 0;
            string gotString = "";
            do
            {
                if (selfWorker.CancellationPending)
                {
                    sckt.Disconnect(false);
                    sckt.Dispose();
                    sckt = null;
                    break;
                }

                if (sckt != null)
                {
                    bytes = sckt.Receive(bytesReceived, bytesReceived.Length, 0);
                    gotString = Encoding.ASCII.GetString(bytesReceived, 0, bytes);

                    string[] gotStrings = gotString.Split('%');
                    foreach (string s in gotStrings)
                    {
                        //quStreamTextStrings.Enqueue(s);
                        cquStreamTextStrings.Enqueue(s);
                    }
                }
            }
            while (bytes > 0);
        }

        #endregion Connection listener






        #region incoming strings processing

        private List<string> listStreamLogStrings = new List<string>();

        void cquStreamTextStrings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                return;
                // чтобы не реагировать на собственные TryDequeue()
            }


            while (cquStreamTextStrings.Count > 0)
            {
                if (cquStreamTextStrings.Count > 0)
                {
                    string strCurrentCatchedStreamString = "";
                    while (!cquStreamTextStrings.TryDequeue(out strCurrentCatchedStreamString))
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                        continue;
                    }

                    if (strCurrentCatchedStreamString == "")
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                        continue;
                    }

                    if (strCurrentCatchedStreamString != "")
                    {
                        listStreamLogStrings.Add(strCurrentCatchedStreamString);

                        if (listStreamLogStrings.Count >= 30)
                        {
                            string filename = Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-log-" +
                                              DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".log";

                            string strToWrite = "";
                            foreach (string logString in listStreamLogStrings)
                                strToWrite += logString + Environment.NewLine;

                            if (cbLogNCdata.Checked)
                            {
                                ServiceTools.logToTextFile(filename, strToWrite, true);
                            }
                            listStreamLogStrings.Clear();
                        }

                        Task tsk = Task.Factory.StartNew(() => ParseStreamString(strCurrentCatchedStreamString));
                    }
                    else
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                        continue;
                    }
                }
            }
        }

        #region // obsolete
        //private void bgwStreamTextParser_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker selfWorker = sender as BackgroundWorker;
        //    string strCurrentCatchedStreamString = "";
        //    //List<string> listStreamLogStrings = new List<string>();

        //    while (true)
        //    {
        //        if (selfWorker.CancellationPending)
        //        {
        //            break;
        //        }

        //        //if (quStreamTextStrings.Count > 0)
        //        if (cquStreamTextStrings.Count > 0)
        //        {
        //            strCurrentCatchedStreamString = "";
        //            while (!cquStreamTextStrings.TryDequeue(out strCurrentCatchedStreamString))
        //            {
        //                // Application.DoEvents();

        //                if (selfWorker.CancellationPending)
        //                {
        //                    break;
        //                }

        //                Thread.Sleep(0);
        //                continue;
        //            }

        //            //strCurrentCatchedStreamString = quStreamTextStrings.Dequeue();
        //            if (strCurrentCatchedStreamString == "")
        //            {
        //                // Application.DoEvents();
        //                Thread.Sleep(0);
        //                continue;
        //            }

        //            if (strCurrentCatchedStreamString != "")
        //            {
        //                //ThreadSafeOperations.SetTextTB(tbLog, strCurrentCatchedStreamString + Environment.NewLine, true);

        //                listStreamLogStrings.Add(strCurrentCatchedStreamString);

        //                if (listStreamLogStrings.Count >= 30)
        //                {
        //                    string filename = Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-log-" +
        //                                      DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".log";

        //                    string strToWrite = "";
        //                    foreach (string logString in listStreamLogStrings) strToWrite += logString + Environment.NewLine;

        //                    if (cbLogNCdata.Checked)
        //                    {
        //                        ServiceTools.logToTextFile(filename, strToWrite, true);
        //                    }
        //                    listStreamLogStrings.Clear();
        //                }

        //                ParseStreamString(strCurrentCatchedStreamString);
        //            }
        //            else
        //            {
        //                // Application.DoEvents();
        //                Thread.Sleep(0);
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            // Application.DoEvents();
        //            Thread.Sleep(0);
        //            continue;
        //        }
        //    }
        //}
        #endregion // obsolete


        private void ParseStreamString(string strData)
        {
            if (strData == null)
            {
                return;
            }

            if (strData.Length < 5)
            {
                return;
            }

            if ((strData.Length >= 8) && (strData.Substring(0, 8) == "DepthNav"))
            {
                return;
            }

            if (strData.Substring(0, 5) == "Coord")
            {
                //это строка GPS
                strData = strData.Replace("$", "").Replace("#", "");

                GPSdata catchedGPSdataPack = new GPSdata(strData, GPSdatasources.IOFFEvesselDataServer);
                if (catchedGPSdataPack.validGPSdata)
                {
                    //quGPSDataQueue.Enqueue(catchedGPSdataPack);
                    cquGPSDataQueue.Enqueue(catchedGPSdataPack);
                }
                return;
            }

            if (strData.Substring(0, 5) == "Meteo")
            {
                MeteoData catchedMeteoData = new MeteoData(strData);
                if (catchedMeteoData.validMeteoData)
                {
                    //quMeteoDataQueue.Enqueue(catchedMeteoData);
                    cquMeteoDataQueue.Enqueue(catchedMeteoData);
                }
            }

        }



        #endregion incoming strings processing






        #region process recieved GPS and METEO data packets

        //Внимение! Monitor.Enter и Monitor.Exit для dmGPSDataMatrix и dmMeteoDataMatrix используется по ссылке на
        // gpsDateTimeValuesList и meteoDateTimeValuesList соответственно - ибо при модификакии этих матриц используется
        // создание новых с новыми ссылками, что нарушает контекст блокировки. А листы модифицируются без изменения ссылки
        // поэтому контекст блокировки не нарушается.
        private DenseMatrix dmGPSDataMatrix = null;
        private List<long> gpsDateTimeValuesList = new List<long>();
        private DenseMatrix dmMeteoDataMatrix = null;
        private List<long> meteoDateTimeValuesList = new List<long>();

        private int gpsDataPacksCount = 0;
        private int meteoDataPacksCount = 0;




        private void DataRateControlTimerCallback(object state)
        {
            object[] stopWatches = state as object[];
            Stopwatch swGPSDataRateWatch = stopWatches[0] as Stopwatch;
            Stopwatch swMeteoDataRateWatch = stopWatches[1] as Stopwatch;

            double meteoDataRate = 0.0d;
            double gpsDataRate = 0.0d;



            double GPSStopWatchSec = ((double)swGPSDataRateWatch.ElapsedMilliseconds) / 1000.0d;
            if (GPSStopWatchSec >= 2.0d)
            {
                gpsDataRate = (double)gpsDataPacksCount / GPSStopWatchSec;

                if (gpsDataRate == 0.0d)
                {
                    ThreadSafeOperations.SetLoadingCircleState(wcNavDataSoeedControl, false, true, Color.Red, 100);
                }
                else
                {
                    ThreadSafeOperations.SetLoadingCircleState(wcNavDataSoeedControl, true, true, Color.Green,
                        Convert.ToInt32(50.0d + 50.0d * gpsDataRate / 10.0d));
                }

                gpsDataPacksCount = 0;
                swGPSDataRateWatch.Restart();
            }


            double meteoStopWatchSec = ((double)swMeteoDataRateWatch.ElapsedMilliseconds) / 1000.0d;
            if (meteoStopWatchSec >= 2.0d)
            {
                meteoDataRate = (double)meteoDataPacksCount / meteoStopWatchSec;

                if (meteoDataRate == 0.0d)
                {
                    ThreadSafeOperations.SetLoadingCircleState(wcMeteoDataSpeedControl, false, true, Color.Red, 100);
                }
                else
                {
                    ThreadSafeOperations.SetLoadingCircleState(wcMeteoDataSpeedControl, true, true, Color.Green,
                        Convert.ToInt32(50.0d + 50.0d * meteoDataRate / 10.0d));
                }

                meteoDataPacksCount = 0;
                swMeteoDataRateWatch.Restart();
            }
        }



        private void CheckAndWriteObservationsFile(object state)
        {
            if (DateTime.Now.Minute == 0)
            {
                string filename = Directory.GetCurrentDirectory() + "\\logs\\ObservationsData-" +
                                  DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".log";

                if (!File.Exists(filename))
                {
                    string strDataToObservationsLog = DateTime.UtcNow.ToString("u").Replace("Z", "") + "; ";

                    if (actualGPSdata != null)
                    {
                        SPA spaCalc = new SPA(actualGPSdata.dateTimeUTC.Year, actualGPSdata.dateTimeUTC.Month,
                            actualGPSdata.dateTimeUTC.Day, actualGPSdata.dateTimeUTC.Hour,
                            actualGPSdata.dateTimeUTC.Minute, actualGPSdata.dateTimeUTC.Second,
                            (float)actualGPSdata.LonDec, (float)actualGPSdata.LatDec,
                            (float)SPAConst.DeltaT(actualGPSdata.dateTimeUTC));
                        int res = spaCalc.spa_calculate();
                        AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                            spaCalc.spa.zenith);
                        double sunElevCalc = sunPositionSPAext.ElevationAngle;
                        double sunAzimuth = sunPositionSPAext.Azimuth;



                        strDataToObservationsLog += actualGPSdata.Lat.ToString().Replace(".", ",") + "; ";
                        strDataToObservationsLog += actualGPSdata.Lon.ToString().Replace(".", ",") + "; ";
                        strDataToObservationsLog += sunElevCalc.ToString("F2").Replace(".", ",") + "; "; // Sun elevation
                        strDataToObservationsLog +=
                            actualGPSdata.IOFFEdataHeadingTrue.ToString().Replace(".", ",") + "; ";
                        strDataToObservationsLog +=
                            actualGPSdata.IOFFEdataHeadingGyro.ToString().Replace(".", ",") + "; ";
                        strDataToObservationsLog += actualGPSdata.IOFFEdataSpeedKnots.ToString().Replace(".", ",") +
                                                    "; ";
                    }

                    if (actualMeteoData != null)
                    {
                        strDataToObservationsLog += actualMeteoData.pressure.ToString().Replace(".", ",") + "; ";
                        strDataToObservationsLog += actualMeteoData.airTemperature.ToString().Replace(".", ",") +
                                                    "; ";
                        strDataToObservationsLog += actualMeteoData.waterTemperature.ToString().Replace(".", ",") +
                                                    "; ";
                        strDataToObservationsLog += actualMeteoData.windSpeed.ToString().Replace(".", ",") + "; ";
                        strDataToObservationsLog += actualMeteoData.windDirection.ToString().Replace(".", ",") +
                                                    "; ";
                        strDataToObservationsLog += actualMeteoData.Rhumidity.ToString().Replace(".", ",") + "; ";
                    }

                    if (cbLogMeasurementsData.Checked)
                    {
                        ServiceTools.logToTextFile(filename, strDataToObservationsLog, true);
                    }
                }
            }
        }






        private void bgwStreamDataProcessing_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            Stopwatch swMeteoDataRateWatch = Stopwatch.StartNew();
            Stopwatch swGPSDataRateWatch = Stopwatch.StartNew();


            TimerCallback tmrDataRateControlTimerCallback = DataRateControlTimerCallback;
            System.Threading.Timer tmrDataRateControlTimer = new System.Threading.Timer(
                tmrDataRateControlTimerCallback, new object[] { swGPSDataRateWatch, swMeteoDataRateWatch }, 0, 1000);

            TimerCallback tmrCheckAndWriteObservationsFileCallback = CheckAndWriteObservationsFile;
            System.Threading.Timer tmrCheckAndWriteObservationsFile =
                new System.Threading.Timer(tmrCheckAndWriteObservationsFileCallback, null, 0, 10000);



            while (true)
            {
                if (selfWorker.CancellationPending)
                {
                    #region dump the rest of data

                    if (dmGPSDataMatrix != null)
                    {
                        DumpCollectedGPSdata();
                    }


                    if (dmMeteoDataMatrix != null)
                    {
                        DumpCollectedMeteoData();
                    }

                    ThreadSafeOperations.SetLoadingCircleState(wcNavDataSoeedControl, false, true, Color.Red, 100);
                    ThreadSafeOperations.SetLoadingCircleState(wcMeteoDataSpeedControl, false, true, Color.Red, 100);

                    #endregion dump the rest of data

                    #region switch off all timers

                    tmrDataRateControlTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    tmrDataRateControlTimer.Dispose();
                    tmrCheckAndWriteObservationsFile.Change(Timeout.Infinite, Timeout.Infinite);
                    tmrCheckAndWriteObservationsFile.Dispose();

                    #endregion switch off all timers

                    break;
                }

                Thread.Sleep(500);

                #region // obsolete - GPS data processing moved to CollectionChanged event handler on cquGPSDataQueue

                //if (cquGPSDataQueue.Count > 0)
                //{
                //    //while (quGPSDataQueue.Count > 0)
                //    while (cquGPSDataQueue.Count > 0)
                //    {
                //        GPSdata currQueuedGPSdata = null;

                //        while (!cquGPSDataQueue.TryDequeue(out currQueuedGPSdata))
                //        {
                //            // Application.DoEvents();

                //            if (selfWorker.CancellationPending)
                //            {
                //                break;
                //            }

                //            Thread.Sleep(0);
                //            continue;
                //        }

                //        //currQueuedGPSdata = quGPSDataQueue.Dequeue();

                //        if (selfWorker.CancellationPending)
                //        {
                //            break;
                //        }

                //        if (currQueuedGPSdata == null)
                //        {
                //            break;
                //        }

                //        gpsDataPacksCount++;

                //        gpsDateTimeValuesList.Add(currQueuedGPSdata.dateTimeUTC.Ticks);

                //        actualGPSdata = currQueuedGPSdata;

                //        if (dmGPSDataMatrix == null)
                //        {
                //            dmGPSDataMatrix = currQueuedGPSdata.ToOneRowDenseMatrix();
                //        }
                //        else
                //        {
                //            DenseVector dvGPSDataVectorToAdd = currQueuedGPSdata.ToDenseVector();
                //            dmGPSDataMatrix =
                //                (DenseMatrix)dmGPSDataMatrix.InsertRow(dmGPSDataMatrix.RowCount, dvGPSDataVectorToAdd);
                //        }
                //    }



                //    if (actualGPSdata != null)
                //    {
                //        ThreadSafeOperations.SetTextTB(tbGPSlatValue, actualGPSdata.lat.ToString() + actualGPSdata.latHemisphere, false);
                //        ThreadSafeOperations.SetTextTB(tbGPSlonValue, actualGPSdata.lon.ToString() + actualGPSdata.lonHemisphere, false);
                //        ThreadSafeOperations.SetTextTB(tbGPSDateTimeValue, actualGPSdata.dateTimeUTC.ToString("u").Replace("Z", ""), false);

                //        // double sunHeight = actualGPSdata.SunAlt();
                //        // ThreadSafeOperations.SetTextTB(tbSunAltValue, sunHeight.ToString(), false);

                //        if (actualGPSdata.dataSource == GPSdatasources.IOFFEvesselDataServer)
                //        {
                //            //public double IOFFEdataHeadingTrue = 0.0d;
                //            //public double IOFFEdataHeadingGyro = 0.0d;
                //            //public double IOFFEdataSpeedKnots = 0.0d;
                //            //public double IOFFEdataDepth = 0.0d;

                //            ThreadSafeOperations.SetTextTB(tbTrueHeadValue,
                //                actualGPSdata.IOFFEdataHeadingTrue.ToString(), false);
                //            ThreadSafeOperations.SetTextTB(tbGyroHeadValue,
                //                actualGPSdata.IOFFEdataHeadingGyro.ToString(), false);
                //            ThreadSafeOperations.SetTextTB(tbSpeedKnotsValue,
                //                actualGPSdata.IOFFEdataSpeedKnots.ToString(), false);
                //            ThreadSafeOperations.SetTextTB(tbDepthValue, actualGPSdata.IOFFEdataDepth.ToString(), false);
                //        }
                //    }



                //    if (gpsDateTimeValuesList.Count >= 50)
                //    {
                //        if (showGeoTrack)
                //        {
                //            List<DateTime> listDateTimeValuesToUpdateTrack = gpsDateTimeValuesList.ConvertAll(longVal => new DateTime(longVal));

                //            while (!Monitor.TryEnter(geoRenderer, 100))
                //            {
                //                // Application.DoEvents();
                //                Thread.Sleep(0);
                //            }

                //            try
                //            {
                //                geoRenderer.lTracksData[0].AddGPSsubtrackData(GPSdata.OfDenseMatrix(dmGPSDataMatrix),
                //                    listDateTimeValuesToUpdateTrack);
                //                if (geoRenderer.mapFollowsActualGPSposition)
                //                {
                //                    geoRenderer.gpsMapCenteredPoint = actualGPSdata.Clone();
                //                }
                //            }
                //            finally
                //            {
                //                Monitor.Exit(geoRenderer);

                //            }

                //        }

                //        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //        dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                //        dataToSave.Add("GPSdata", dmGPSDataMatrix);

                //        if (cbLogNCdata.Checked)
                //        {
                //            NetCDFoperations.AddVariousDataToFile(dataToSave,
                //                Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-GPSDataLog-" +
                //                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
                //        }

                //        dmGPSDataMatrix = null;
                //        gpsDateTimeValuesList.Clear();
                //    }
                //}

                #endregion // obsolete - GPS data processing moved to CollectionChanged event handler on cquGPSDataQueue


                #region // obsolete - Meteo data processing moved to CollectionChanged event handler on cquMeteoDataQueue

                //if (cquMeteoDataQueue.Count > 0)
                //{
                //    while (cquMeteoDataQueue.Count > 0)
                //    {
                //        meteoDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                //        MeteoData currQueuedMeteoData = null;

                //        while (!cquMeteoDataQueue.TryDequeue(out currQueuedMeteoData))
                //        {
                //            // Application.DoEvents();

                //            if (selfWorker.CancellationPending)
                //            {
                //                break;
                //            }

                //            Thread.Sleep(0);
                //            continue;
                //        }

                //        if (selfWorker.CancellationPending)
                //        {
                //            break;
                //        }

                //        if (currQueuedMeteoData == null)
                //        {
                //            break;
                //        }

                //        meteoDataPacksCount++;

                //        actualMeteoData = currQueuedMeteoData;

                //        int columnsCount = 5;

                //        if (dmMeteoDataMatrix == null)
                //        {
                //            dmMeteoDataMatrix = currQueuedMeteoData.ToOneRowDenseMatrix();
                //        }
                //        else
                //        {
                //            DenseVector dvMeteoDataVectorToAdd = currQueuedMeteoData.ToDenseVector();
                //            dmMeteoDataMatrix =
                //                (DenseMatrix)dmMeteoDataMatrix.InsertRow(dmMeteoDataMatrix.RowCount, dvMeteoDataVectorToAdd);
                //        }
                //    }

                //    if (actualMeteoData != null)
                //    {
                //        ThreadSafeOperations.SetTextTB(tbPressureValue, actualMeteoData.pressure.ToString(), false);
                //        ThreadSafeOperations.SetTextTB(tbAirTemperatureValue, actualMeteoData.airTemperature.ToString(), false);
                //        ThreadSafeOperations.SetTextTB(tbWindSpeedValue, actualMeteoData.windSpeed.ToString(), false);
                //        ThreadSafeOperations.SetTextTB(tbWindDirectionValue, actualMeteoData.windDirection.ToString(), false);
                //        ThreadSafeOperations.SetTextTB(tbRelHumidityValue, actualMeteoData.Rhumidity.ToString(), false);
                //        ThreadSafeOperations.SetTextTB(tbWaterTemperatureValue,
                //            actualMeteoData.waterTemperature.ToString(), false);
                //        ThreadSafeOperations.SetTextTB(tbWaterSalinityValue, actualMeteoData.waterSalinity.ToString(),
                //            false);
                //    }



                //    if (meteoDateTimeValuesList.Count >= 50)
                //    {
                //        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                //        dataToSave.Add("DateTime", meteoDateTimeValuesList.ToArray());
                //        dataToSave.Add("MeteoData", dmMeteoDataMatrix);

                //        if (cbLogNCdata.Checked)
                //        {
                //            NetCDFoperations.AddVariousDataToFile(dataToSave,
                //                Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-MeteoDataLog-" +
                //                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
                //        }

                //        dmMeteoDataMatrix = null;
                //        meteoDateTimeValuesList.Clear();
                //    }
                //}

                #endregion // obsolete - Meteo data processing moved to CollectionChanged event handler on cquMeteoDataQueue

            }
        }




        #region for Meteo data

        private void DumpCollectedMeteoData()
        {
            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave

            Monitor.Enter(meteoDateTimeValuesList);
            try
            {
                dataToSave.Add("DateTime", meteoDateTimeValuesList.ToArray());
                meteoDateTimeValuesList.Clear();
                dataToSave.Add("MeteoData", dmMeteoDataMatrix.Copy());
                dmMeteoDataMatrix = null;
            }
            finally
            {
                Monitor.Exit(meteoDateTimeValuesList);
            }

            #endregion unload shared collections data to dataToSave

            if (cbLogNCdata.Checked)
            {
                NetCDFoperations.AddVariousDataToFile(dataToSave,
                    Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-MeteoDataLog-" +
                    DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
            }
        }




        private void UpdateActualMeteoDataOnForm()
        {
            if (actualMeteoData != null)
            {
                ThreadSafeOperations.SetTextTB(tbPressureValue, actualMeteoData.pressure.ToString(), false);
                ThreadSafeOperations.SetTextTB(tbAirTemperatureValue, actualMeteoData.airTemperature.ToString(), false);
                ThreadSafeOperations.SetTextTB(tbWindSpeedValue, actualMeteoData.windSpeed.ToString(), false);
                ThreadSafeOperations.SetTextTB(tbWindDirectionValue, actualMeteoData.windDirection.ToString(), false);
                ThreadSafeOperations.SetTextTB(tbRelHumidityValue, actualMeteoData.Rhumidity.ToString(), false);
                ThreadSafeOperations.SetTextTB(tbWaterTemperatureValue,
                    actualMeteoData.waterTemperature.ToString(), false);
                ThreadSafeOperations.SetTextTB(tbWaterSalinityValue, actualMeteoData.waterSalinity.ToString(),
                    false);
            }
        }




        async void cquMeteoDataQueue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
                // чтобы не реагировать на собственные TryDequeue()
            }




            while (cquMeteoDataQueue.Count > 0)
            {
                MeteoData currQueuedMeteoData = null;

                while (!cquMeteoDataQueue.TryDequeue(out currQueuedMeteoData))
                {
                    Thread.Sleep(0);
                    continue;
                }

                if (currQueuedMeteoData == null)
                {
                    continue;
                }

                Interlocked.Increment(ref meteoDataPacksCount);

                actualMeteoData = currQueuedMeteoData;

                #region store data in shared data collections


                Monitor.Enter(meteoDateTimeValuesList);
                try
                {
                    meteoDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                    if (dmMeteoDataMatrix == null)
                    {
                        dmMeteoDataMatrix = currQueuedMeteoData.ToOneRowDenseMatrix();
                    }
                    else
                    {
                        DenseVector dvMeteoDataVectorToAdd = currQueuedMeteoData.ToDenseVector();
                        dmMeteoDataMatrix =
                            (DenseMatrix)
                                dmMeteoDataMatrix.InsertRow(dmMeteoDataMatrix.RowCount, dvMeteoDataVectorToAdd);
                    }
                }
                finally
                {
                    Monitor.Exit(meteoDateTimeValuesList);
                }


                #endregion store data in shared data collections
            }





            Task tskUpdateMeteoDataOnForm = Task.Factory.StartNew(UpdateActualMeteoDataOnForm);

            Task tskDumpCollectedMeteoData = null;


            if (meteoDateTimeValuesList.Count >= 50)
            {
                tskDumpCollectedMeteoData = Task.Factory.StartNew(DumpCollectedMeteoData);
            }

            await tskUpdateMeteoDataOnForm;
            if (tskDumpCollectedMeteoData != null)
            {
                await tskDumpCollectedMeteoData;
            }
        }

        #endregion for Meteo data






        #region for GPS data



        private void DumpCollectedGPSdata()
        {
            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            #region unload shared collections data to dataToSave
            DenseMatrix dmGPSDataMatrixCopy;
            try
            {
                dmGPSDataMatrixCopy = dmGPSDataMatrix.Copy();
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "exception was caught: " + Environment.NewLine + ServiceTools.CurrentCodeLineDescription() +
                    Environment.NewLine + "message: " + ex.Message);
                return;
            }
            

            Monitor.Enter(gpsDateTimeValuesList);
            try
            {
                dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                gpsDateTimeValuesList.Clear();
                dataToSave.Add("GPSdata", dmGPSDataMatrixCopy);
                dmGPSDataMatrix = null;
            }
            finally
            {
                Monitor.Exit(gpsDateTimeValuesList);
            }

            #endregion unload shared collections data to dataToSave

            if (cbLogNCdata.Checked)
            {
                NetCDFoperations.AddVariousDataToFile(dataToSave,
                    Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-GPSDataLog-" +
                    DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
            }
        }




        private void UpdateActualGPSdataOnForm()
        {
            if (actualGPSdata != null)
            {
                ThreadSafeOperations.SetTextTB(tbGPSlatValue, actualGPSdata.lat.ToString() + actualGPSdata.latHemisphere, false);
                ThreadSafeOperations.SetTextTB(tbGPSlonValue, actualGPSdata.lon.ToString() + actualGPSdata.lonHemisphere, false);
                ThreadSafeOperations.SetTextTB(tbGPSDateTimeValue, actualGPSdata.dateTimeUTC.ToString("u").Replace("Z", ""), false);

                if (actualGPSdata.dataSource == GPSdatasources.IOFFEvesselDataServer)
                {
                    ThreadSafeOperations.SetTextTB(tbTrueHeadValue,
                        actualGPSdata.IOFFEdataHeadingTrue.ToString(), false);
                    ThreadSafeOperations.SetTextTB(tbGyroHeadValue,
                        actualGPSdata.IOFFEdataHeadingGyro.ToString(), false);
                    ThreadSafeOperations.SetTextTB(tbSpeedKnotsValue,
                        actualGPSdata.IOFFEdataSpeedKnots.ToString(), false);
                    ThreadSafeOperations.SetTextTB(tbDepthValue, actualGPSdata.IOFFEdataDepth.ToString(), false);
                }
            }
        }




        private void UpdateGPSdataForGeotrackRenderer()
        {
            if (showGeoTrack)
            {
                List<DateTime> listDateTimeValuesToUpdateTrack;
                DenseMatrix dmGPSDataMatrixCopy = null;

                Monitor.Enter(gpsDateTimeValuesList);
                try
                {
                    listDateTimeValuesToUpdateTrack = gpsDateTimeValuesList.ConvertAll(longVal => new DateTime(longVal));
                    dmGPSDataMatrixCopy = dmGPSDataMatrix.Copy();
                }
                finally
                {
                    Monitor.Exit(gpsDateTimeValuesList);
                }


                while (!Monitor.TryEnter(geoRenderer, 100))
                {
                    Thread.Sleep(0);
                }

                try
                {
                    geoRenderer.lTracksData[0].AddGPSsubtrackData(GPSdata.OfDenseMatrix(dmGPSDataMatrixCopy),
                        listDateTimeValuesToUpdateTrack);
                    if (geoRenderer.mapFollowsActualGPSposition)
                    {
                        geoRenderer.gpsMapCenteredPoint = actualGPSdata.Clone();
                    }
                }
                finally
                {
                    Monitor.Exit(geoRenderer);
                    dmGPSDataMatrixCopy = null;
                    listDateTimeValuesToUpdateTrack = null;
                }
            }
        }





        async void cquGPSDataQueue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
                // чтобы не реагировать на собственные TryDequeue()
            }



            while (cquGPSDataQueue.Count > 0)
            {
                GPSdata currQueuedGPSdata = null;

                while (!cquGPSDataQueue.TryDequeue(out currQueuedGPSdata))
                {
                    Thread.Sleep(0);
                    continue;
                }


                if (currQueuedGPSdata == null)
                {
                    continue;
                }

                Interlocked.Increment(ref gpsDataPacksCount);
                actualGPSdata = currQueuedGPSdata;


                #region store data in shared data collections

                Monitor.Enter(gpsDateTimeValuesList);
                try
                {
                    gpsDateTimeValuesList.Add(currQueuedGPSdata.dateTimeUTC.Ticks);
                    if (dmGPSDataMatrix == null)
                    {
                        dmGPSDataMatrix = currQueuedGPSdata.ToOneRowDenseMatrix();
                    }
                    else
                    {
                        DenseVector dvGPSDataVectorToAdd = currQueuedGPSdata.ToDenseVector();
                        dmGPSDataMatrix =
                            (DenseMatrix)dmGPSDataMatrix.InsertRow(dmGPSDataMatrix.RowCount, dvGPSDataVectorToAdd);
                    }
                }
                finally
                {
                    Monitor.Exit(gpsDateTimeValuesList);
                }



                #endregion store data in shared data collections

            }



            Task tskUpdateGPSdataOnForm = Task.Factory.StartNew(UpdateActualGPSdataOnForm);

            Task tskDumpCollectedGPSdata = null;
            Task tskUpdateGPSdataForGeotrackRenderer = null;

            if (gpsDateTimeValuesList.Count >= 50)
            {
                tskDumpCollectedGPSdata = Task.Factory.StartNew(DumpCollectedGPSdata);
                tskUpdateGPSdataForGeotrackRenderer = Task.Factory.StartNew(UpdateGPSdataForGeotrackRenderer);
            }

            await tskUpdateGPSdataOnForm;
            if (tskDumpCollectedGPSdata != null)
            {
                await tskDumpCollectedGPSdata;
            }

            if (tskUpdateGPSdataForGeotrackRenderer != null)
            {
                await tskUpdateGPSdataForGeotrackRenderer;
            }
        }

        #endregion for GPS data

        #endregion process recieved GPS and METEO data packets






        #region Geotrack

        private static bool needToUpdateGeoTrackNow = false;
        private void bgwGraphsRenderer_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            MultipleScatterAndFunctionsRepresentation graphsRenderer =
                new MultipleScatterAndFunctionsRepresentation(pbGraphs.Size);

            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
            if (showGeoTrack)
            {
                // пропишем в рисователь трека имена файлов, содержащих логи данных GPS
                bool addedNewFiles = false;

                DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\logs\\");

                while (!Monitor.TryEnter(geoRenderer, 100))
                {
                    // Application.DoEvents();
                    Thread.Sleep(0);
                }

                try
                {
                    foreach (FileInfo fInfo in dirInfo.EnumerateFiles("*GPS*.nc", SearchOption.TopDirectoryOnly))
                    {
                        if (geoRenderer.listGPSdataLogNetCDFFileNames.Contains(fInfo.FullName))
                        {
                            continue;
                        }
                        geoRenderer.listGPSdataLogNetCDFFileNames.Add(fInfo.FullName);
                        addedNewFiles = true;
                    }
                    if (addedNewFiles)
                    {
                        geoRenderer.ReadGPSFiles(lblStatusString);
                    }
                }
                finally
                {
                    Monitor.Exit(geoRenderer);
                }
            }



            TimerCallback tmrGeoTrackUpdateCallback = new TimerCallback((obj) =>
            {
                if (showGeoTrack)
                {
                    UpdateGeoTrack();
                    needToUpdateGeoTrackNow = false;
                }
            });
            System.Threading.Timer tmrGeoTrackUpdate = new System.Threading.Timer(tmrGeoTrackUpdateCallback, null, 0, updateGeoTrackPeriodSec * 1000);

            while (true)
            {
                if (selfWorker.CancellationPending)
                {
                    tmrGeoTrackUpdate.Change(Timeout.Infinite, Timeout.Infinite);
                    tmrGeoTrackUpdate.Dispose();
                    break;
                }

                Thread.Sleep(500);
            }
        }


        //private static bool geoTrackIsUpdatingNow = false;
        private bool UpdateGeoTrack()
        {
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            try
            {
                geoRenderer.listMarkers.Clear();
                geoRenderer.listMarkers.Add(new Tuple<GPSdata, SequencesDrawingVariants, Bgr>(actualGPSdata,
                    SequencesDrawingVariants.triangles, new Bgr(0, 255, 0)));

                if (geoRenderer.mapFollowsActualGPSposition)
                {
                    geoRenderer.gpsMapCenteredPoint = actualGPSdata.Clone();
                }

                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
                ThreadSafeOperations.UpdatePictureBox(pbGeoTrack, geoRenderer.RepresentTopo(pbGeoTrack.Size));
                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            return true;
        }



        private void ShowGeoTrackOnce()
        {
            // пропишем в рисователь трека имена файлов, содержащих логи данных GPS
            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);

            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {

                if ((geoRenderer.lTracksData.Count == 0) || (geoRenderer.lTracksData[0].lGPScoords.Count == 0))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\logs\\");
                    foreach (FileInfo fInfo in dirInfo.EnumerateFiles("*GPS*.nc", SearchOption.TopDirectoryOnly))
                    {
                        geoRenderer.listGPSdataLogNetCDFFileNames.Add(fInfo.FullName);
                    }

                    geoRenderer.ReadGPSFiles(lblStatusString);
                }

                actualGPSdata = geoRenderer.lTracksData[0].lGPScoords[geoRenderer.lTracksData[0].lGPScoords.Count - 1];

                geoRenderer.listMarkers.Clear();
                geoRenderer.listMarkers.Add(new Tuple<GPSdata, SequencesDrawingVariants, Bgr>(actualGPSdata,
                    SequencesDrawingVariants.triangles, new Bgr(0, 255, 0)));

                if (geoRenderer.mapFollowsActualGPSposition)
                {
                    geoRenderer.gpsMapCenteredPoint = actualGPSdata.Clone();
                }

                ThreadSafeOperations.UpdatePictureBox(pbGeoTrack, geoRenderer.RepresentTopo(pbGeoTrack.Size), true);
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
        }




        private void trbGeoTrackScale_ValueChanged(object sender, EventArgs e)
        {
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                if (geoRenderer.lTracksData.Count == 0)
                {
                    trbGeoTrackScale.Value = 0;
                    return;
                }
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            trbGeoTrackScale.Enabled = false;
            double scale = Math.Pow(2.0d, trbGeoTrackScale.Value);
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                geoRenderer.ScaleFactor = scale;
            }
            finally
            {
                Monitor.Exit(geoRenderer);
                trbGeoTrackScale.Enabled = true;
            }

            if ((showGeoTrack) && (!bgwGeotrackRenderer.IsBusy))
            {
                ShowGeoTrackOnce();
            }
            else if ((showGeoTrack) && (bgwGeotrackRenderer.IsBusy))
            {
                needToUpdateGeoTrackNow = true;
            }
            trbGeoTrackScale.Enabled = true;
        }



        private void pbGeoTrack_Click(object sender, EventArgs e)
        {
            if (geoRenderer == null)
            {
                return;
            }
            else if (geoRenderer.lTracksData.Count == 0)
            {
                return;
            }

            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                SimpleShowImageForm f1 = new SimpleShowImageForm((new Image<Bgr, byte>(new Size(800, 600))).Bitmap);
                f1.FormResizing += ExternalGeotrackImageFormResized;
                f1.Show();
                f1.UpdateBitmap(geoRenderer.RepresentTopo(f1.pb1.Size));
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }
        }



        public void ExternalGeotrackImageFormResized(object sender, EventArgs e)
        {
            SimpleShowImageForm f1 = sender as SimpleShowImageForm;
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                f1.UpdateBitmap(geoRenderer.RepresentTopo(f1.pb1.Size));
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }
        }


        #region // obsolete

        //private void cbShowGeoTrack_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (((sender as CheckBox).Checked) && (!bgwGeotrackRenderer.IsBusy) && (!bgwStreamDataProcessing.IsBusy))
        //    {
        //        var repl = MessageBox.Show("Wanna watch right now?", "Right now?", MessageBoxButtons.YesNoCancel,
        //            MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
        //        if (repl == System.Windows.Forms.DialogResult.Yes)
        //        {


        //            DoWorkEventHandler bgw1DoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
        //            {
        //                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
        //                BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
        //                object[] currBGWarguments = (object[])args.Argument;

        //                ShowGeoTrackOnce();

        //                args.Result = new object[] { "" };
        //            };


        //            RunWorkerCompletedEventHandler currWorkCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
        //            {
        //                object[] currentBGWResults = (object[])args.Result;
        //                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
        //            };


        //            BackgroundWorker bgw1 = new BackgroundWorker();
        //            bgw1.DoWork += bgw1DoWorkHandler;
        //            bgw1.RunWorkerCompleted += currWorkCompletedHandler;
        //            object[] BGWargs = new object[] { "" };
        //            bgw1.RunWorkerAsync(BGWargs);
        //        }
        //    }
        //    else if ((!(sender as CheckBox).Checked) && (bgwGeotrackRenderer.IsBusy))
        //    {
        //        bgwGeotrackRenderer.CancelAsync();
        //    }
        //    else if (((sender as CheckBox).Checked) && (!bgwGeotrackRenderer.IsBusy) && (bgwStreamDataProcessing.IsBusy))
        //    {
        //        bgwGeotrackRenderer.RunWorkerAsync();
        //    }
        //}

        #endregion // obsolete




        private void scrbGeoTrackScrollLonValues_ValueChanged(object sender, EventArgs e)
        {
            if (scrbGeoTrackScrollLonValues.Value == 0)
            {
                return;
            }

            if (showGeoTrack)
            {
                while (!Monitor.TryEnter(geoRenderer, 100))
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                try
                {
                    geoRenderer.MoveGPSWindow(-scrbGeoTrackScrollLonValues.Value, 0);
                }
                finally
                {
                    Monitor.Exit(geoRenderer);
                }

                scrbGeoTrackScrollLonValues.Value = 0;

                //if (bgwGraphsRenderer.IsBusy)
                //{
                //    needToUpdateGeoTrackNow = true;
                //}
                //else
                //{
                BackgroundWorker bgwGraphUpdater = new BackgroundWorker();

                DoWorkEventHandler bgwGraphUpdaterDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
                {

                    BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                    object[] currBGWarguments = (object[])args.Argument;

                    Stopwatch sw1 = Stopwatch.StartNew();

                    bool retResult = false;

                    while (true)
                    {
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
                        if (UpdateGeoTrack())
                        {
                            retResult = true;
                            break;
                        }
                        else if (sw1.ElapsedMilliseconds > 10000)
                        {
                            retResult = false;
                            break;
                        }
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                    args.Result = new object[] { retResult };
                };


                RunWorkerCompletedEventHandler bgwGraphUpdaterCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    bool UpdatingResult = (bool)(currentBGWResults[0]);

                    if (!UpdatingResult)
                    {
                        ThreadSafeOperations.SetText(lblStatusString, DateTime.Now.ToString("s").Replace("T", " ") + ": Geogtrack failed to be updated.", false);
                    }
                    ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                };

                bgwGraphUpdater.DoWork += bgwGraphUpdaterDoWorkHandler;
                bgwGraphUpdater.RunWorkerCompleted += bgwGraphUpdaterCompletedHandler;
                bgwGraphUpdater.RunWorkerAsync();
                //}
            }
        }




        private void scrbGeoTrackScrollLatValues_ValueChanged(object sender, EventArgs e)
        {
            if (scrbGeoTrackScrollLatValues.Value == 0)
            {
                return;
            }

            if (showGeoTrack)
            {
                while (!Monitor.TryEnter(geoRenderer, 100))
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                try
                {
                    geoRenderer.MoveGPSWindow(0, -scrbGeoTrackScrollLatValues.Value);
                }
                finally
                {
                    Monitor.Exit(geoRenderer);
                }

                scrbGeoTrackScrollLatValues.Value = 0;

                //if (bgwGraphsRenderer.IsBusy)
                //{
                //    needToUpdateGeoTrackNow = true;
                //}
                //else
                //{
                BackgroundWorker bgwGraphUpdater = new BackgroundWorker();

                DoWorkEventHandler bgwGraphUpdaterDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
                {

                    BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                    object[] currBGWarguments = (object[])args.Argument;

                    Stopwatch sw1 = Stopwatch.StartNew();

                    bool retResult = false;

                    while (true)
                    {
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
                        if (UpdateGeoTrack())
                        {
                            retResult = true;
                            break;
                        }
                        else if (sw1.ElapsedMilliseconds > 10000)
                        {
                            retResult = false;
                            break;
                        }
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                    args.Result = new object[] { retResult };
                };


                RunWorkerCompletedEventHandler bgwGraphUpdaterCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    bool UpdatingResult = (bool)(currentBGWResults[0]);

                    if (!UpdatingResult)
                    {
                        ThreadSafeOperations.SetText(lblStatusString, DateTime.Now.ToString("s").Replace("T", " ") + ": Geogtrack failed to be updated.", false);
                    }
                    ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                };

                bgwGraphUpdater.DoWork += bgwGraphUpdaterDoWorkHandler;
                bgwGraphUpdater.RunWorkerCompleted += bgwGraphUpdaterCompletedHandler;
                bgwGraphUpdater.RunWorkerAsync();
                //}
            }
        }




        private void btnCenterToActualPosition_Click(object sender, EventArgs e)
        {
            if (showGeoTrack)
            {
                while (!Monitor.TryEnter(geoRenderer, 100))
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                try
                {
                    geoRenderer.MoveGPSWindow(actualGPSdata);
                    geoRenderer.mapFollowsActualGPSposition = true;
                }
                finally
                {
                    Monitor.Exit(geoRenderer);
                }

                //if (bgwGraphsRenderer.IsBusy)
                //{
                //    needToUpdateGeoTrackNow = true;
                //}
                //else
                //{
                BackgroundWorker bgwGraphUpdater = new BackgroundWorker();

                DoWorkEventHandler bgwGraphUpdaterDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
                {

                    BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                    object[] currBGWarguments = (object[])args.Argument;

                    Stopwatch sw1 = Stopwatch.StartNew();

                    bool retResult = false;

                    while (true)
                    {
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
                        if (UpdateGeoTrack())
                        {
                            retResult = true;
                            break;
                        }
                        else if (sw1.ElapsedMilliseconds > 10000)
                        {
                            retResult = false;
                            break;
                        }
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                    args.Result = new object[] { retResult };
                };


                RunWorkerCompletedEventHandler bgwGraphUpdaterCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    bool UpdatingResult = (bool)(currentBGWResults[0]);

                    if (!UpdatingResult)
                    {
                        ThreadSafeOperations.SetText(lblStatusString, DateTime.Now.ToString("s").Replace("T", " ") + ": Geogtrack failed to be updated.", false);
                    }
                    ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                };

                bgwGraphUpdater.DoWork += bgwGraphUpdaterDoWorkHandler;
                bgwGraphUpdater.RunWorkerCompleted += bgwGraphUpdaterCompletedHandler;
                bgwGraphUpdater.RunWorkerAsync();
                //}
            }
        }



        private void btnToggleShowGeotrack_Click(object sender, EventArgs e)
        {
            showGeoTrack = !showGeoTrack;
            if (showGeoTrack)
            {
                ThreadSafeOperations.ToggleButtonState(btnToggleShowGeotrack, true, "ON", true);
            }
            else
            {
                ThreadSafeOperations.ToggleButtonState(btnToggleShowGeotrack, true, "OFF", true);
            }

            if (showGeoTrack && !bgwGeotrackRenderer.IsBusy && !bgwStreamDataProcessing.IsBusy)
            {
                var repl = MessageBox.Show("Wanna watch right now?", "Right now?", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (repl == System.Windows.Forms.DialogResult.Yes)
                {
                    DoWorkEventHandler bgw1DoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
                    {
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
                        BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                        object[] currBGWarguments = (object[])args.Argument;

                        ShowGeoTrackOnce();

                        args.Result = new object[] { "" };
                    };


                    RunWorkerCompletedEventHandler currWorkCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                    {
                        object[] currentBGWResults = (object[])args.Result;
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                    };


                    BackgroundWorker bgw1 = new BackgroundWorker();
                    bgw1.DoWork += bgw1DoWorkHandler;
                    bgw1.RunWorkerCompleted += currWorkCompletedHandler;
                    object[] BGWargs = new object[] { "" };
                    bgw1.RunWorkerAsync(BGWargs);
                }
            }
            else if (!showGeoTrack && bgwGeotrackRenderer.IsBusy)
            {
                bgwGeotrackRenderer.CancelAsync();
            }
            else if ((showGeoTrack) && (!bgwGeotrackRenderer.IsBusy) && (bgwStreamDataProcessing.IsBusy))
            {
                bgwGeotrackRenderer.RunWorkerAsync();
            }
        }

        #endregion Geotrack






        #region form behaviour

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }


        private void btnProperties_Click(object sender, EventArgs e)
        {
            PropertiesEditor propForm = new PropertiesEditor(defaultProperties, defaultPropertiesXMLfileName);
            propForm.FormClosed += new FormClosedEventHandler(PropertiesFormClosed);
            propForm.ShowDialog();
        }


        public void PropertiesFormClosed(object sender, FormClosedEventArgs e)
        {
            readDefaultProperties();
        }


        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                         "\\settings\\IofffeVesselInfoStreamSettings.xml";
            if (!File.Exists(defaultPropertiesXMLfileName)) return;
            defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);

            bcstServerIP = (string)defaultProperties["DefaultDataBroadcastingServerIP"];
            bcstServerPort = Convert.ToInt32(defaultProperties["DefaultDataBroadcastingServerPort"]);
            graphsUpdatingPeriodSec = Convert.ToInt32(defaultProperties["GraphsUpdatingPeriodSec"]);
            updateGeoTrackPeriodSec = Convert.ToInt32(defaultProperties["GeotrackUpdatingPeriodSec"]);
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            readDefaultProperties();

            cquStreamTextStrings.CollectionChanged += cquStreamTextStrings_CollectionChanged;

            cquMeteoDataQueue.CollectionChanged += cquMeteoDataQueue_CollectionChanged;

            cquGPSDataQueue.CollectionChanged += cquGPSDataQueue_CollectionChanged;
        }



        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (bgwSocketStreamReader.IsBusy)
            {
                bgwSocketStreamReader.CancelAsync();
                //bgwStreamTextParser.CancelAsync();
                bgwStreamDataProcessing.CancelAsync();
                bgwGeotrackRenderer.CancelAsync();
                ThreadSafeOperations.ToggleButtonState(btnConnect, true, "CONNECT", true);
            }
            else
            {
                try
                {
                    s = ConnectSocket(bcstServerIP, bcstServerPort);
                }
                catch (Exception ex)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t connect: " + ex.Message);
                }

                if (s == null) return;
                else
                {
                    bgwStreamDataProcessing.RunWorkerAsync();
                    //bgwStreamTextParser.RunWorkerAsync();
                    bgwSocketStreamReader.RunWorkerAsync(new object[] { s });
                    ThreadSafeOperations.ToggleButtonState(btnConnect, true, "STOP", true);
                    if (showGeoTrack)
                    {
                        bgwGeotrackRenderer.RunWorkerAsync();
                    }
                }
            }
        }


        #endregion form behaviour




        #region graphs representing

        private System.Threading.Timer timerGraphUpdate;
        private void btnToggleShowGraphs_Click(object sender, EventArgs e)
        {
            showGraphs = !showGraphs;
            ThreadSafeOperations.ToggleButtonState(btnToggleShowGraphs, true, showGraphs ? "ON" : "OFF", true);


            if (showGraphs)
            {
                if (timerGraphUpdate == null)
                {
                    TimerCallback tcbRenderAndShowGraph = RenderAndShowGraph;
                    timerGraphUpdate = new System.Threading.Timer(tcbRenderAndShowGraph, null, 0, graphsUpdatingPeriodSec * 1000);
                }
                else
                {
                    bgwGraphsPresenter.RunWorkerAsync();
                }
                //RenderAndShowGraph(null);
            }
            else
            {
                if (timerGraphUpdate != null)
                {
                    timerGraphUpdate.Dispose();
                }
            }
        }




        private void RenderAndShowGraph(object state)
        {
            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraph, true, true, wcUpdatimgGraph.Color);

            bgwGraphsPresenter = new BackgroundWorker();
            bgwGraphsPresenter.DoWork += bgwGraphsPresenter_DoWork;
            bgwGraphsPresenter.RunWorkerCompleted += bgwGraphsPresenter_RunWorkerCompleted;

            bgwGraphsPresenter.RunWorkerAsync();
        }


        void bgwGraphsPresenter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraph, false, false, wcUpdatimgGraph.Color);
        }




        void bgwGraphsPresenter_DoWork(object sender, DoWorkEventArgs e)
        {

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            graphImage = FillGraphImage(pbGraphs.Size);
            if (graphImage != null)
            {
                ThreadSafeOperations.UpdatePictureBox(pbGraphs, graphImage, false);
            }
        }




        private void pbGraphs_Click(object sender, EventArgs e)
        {
            BackgroundWorker bgwGraphsPresenterInSeparateWindow = new BackgroundWorker();
            bgwGraphsPresenterInSeparateWindow.DoWork += bgwGraphsPresenterInSeparateWindow_DoWork;

            bgwGraphsPresenterInSeparateWindow.RunWorkerAsync();
        }




        void bgwGraphsPresenterInSeparateWindow_DoWork(object sender, DoWorkEventArgs e)
        {
            Image<Bgr, byte> img = FillGraphImage(new Size(1280, 1024));

            if (img != null)
            {
                ServiceTools.ExecMethodInSeparateThread(this, delegate()
                {
                    ServiceTools.ShowPicture(img, "");
                });
            }
        }





        private Image<Bgr, byte> FillGraphImage(Size imgSize)
        {
            string curDirPath = Directory.GetCurrentDirectory() + "\\logs\\";

            DirectoryInfo dirInfo = new DirectoryInfo(curDirPath);

            List<Dictionary<string, object>> lReadData = new List<Dictionary<string, object>>();

            if (defaultGraphsTimeSpan)
            {
                graphsTimeSpan = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            }


            if (defaultGraphsTimeSpan)
            {
                string yesterdaysMeteoDataFileName = curDirPath + "IoffeVesselInfoStream-MeteoDataLog-" +
                                                     DateTime.UtcNow.Date.AddDays(-1).ToString("yyyy-MM-dd") + ".nc";

                string todaysMeteoDataFileName = curDirPath + "IoffeVesselInfoStream-MeteoDataLog-" +
                                                 DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc";
                if (!File.Exists(todaysMeteoDataFileName) && !File.Exists(yesterdaysMeteoDataFileName))
                {
                    return null;
                }

                Dictionary<string, object> dictTodaysFileData = null;
                Dictionary<string, object> dictYesterdaysFileData = null;

                try
                {
                    dictTodaysFileData = NetCDFoperations.ReadDataFromFile(todaysMeteoDataFileName);
                    dictYesterdaysFileData = NetCDFoperations.ReadDataFromFile(yesterdaysMeteoDataFileName);
                }
                catch (Exception)
                {
                    //return null;
                }
                if ((dictTodaysFileData == null) && (dictYesterdaysFileData == null))
                {
                    return null;
                }

                lReadData.AddRange(new[] { dictYesterdaysFileData, dictTodaysFileData });
            }
            else
            {
                IEnumerable<string> ncFileNames = Directory.EnumerateFiles(curDirPath, "IoffeVesselInfoStream-MeteoDataLog-*.nc",
                    SearchOption.TopDirectoryOnly);
                foreach (string ncFileName in ncFileNames)
                {
                    Tuple<DateTime, DateTime> currFileDateTimeRange =
                        ServiceTools.GetNetCDFfileTimeStampsRange(ncFileName);
                    if (currFileDateTimeRange == null) continue;

                    if ((currFileDateTimeRange.Item1 >= graphsTimeSpan.Item1) && (currFileDateTimeRange.Item1 <= graphsTimeSpan.Item2) ||
                        (currFileDateTimeRange.Item2 >= graphsTimeSpan.Item1) && (currFileDateTimeRange.Item2 <= graphsTimeSpan.Item2))
                    {
                        Dictionary<string, object> dictFileData = NetCDFoperations.ReadDataFromFile(ncFileName);
                        lReadData.Add(dictFileData);
                    }
                }
            }



            TimeSeries<MeteoData> tsMeteoData = null;

            foreach (Dictionary<string, object> currFileDataDict in lReadData)
            {
                if (currFileDataDict == null)
                {
                    continue;
                }

                string varNameDateTime = "DateTime";

                List<long> currFileDateTimeLongTicksList = new List<long>((currFileDataDict[varNameDateTime] as long[]));
                List<DateTime> currFileDateTimeList = currFileDateTimeLongTicksList.ConvertAll(longVal => new DateTime(longVal));

                string varNameMeteoData = "MeteoData";
                List<MeteoData> currFileMeteoDataList =
                    MeteoData.OfDenseMatrix(currFileDataDict[varNameMeteoData] as DenseMatrix);

                if (tsMeteoData == null)
                {
                    tsMeteoData = new TimeSeries<MeteoData>(currFileMeteoDataList, currFileDateTimeList);
                }
                else
                {
                    tsMeteoData.AddSubseriaData(currFileMeteoDataList, currFileDateTimeList);
                }
            }

            if (tsMeteoData == null)
            {
                return null;
            }

            tsMeteoData.SortByTimeStamps();
            tsMeteoData.RemoveDuplicatedTimeStamps();

            DateTime utcNow = DateTime.UtcNow;
            if (defaultGraphsTimeSpan)
            {
                tsMeteoData.RemoveValues(dt => (utcNow - dt).TotalSeconds > 86400);
            }
            else
            {
                tsMeteoData.RemoveValues(dt => !((dt >= graphsTimeSpan.Item1) && (dt <= graphsTimeSpan.Item2)));
            }


            List<TimeSeries<MeteoData>> subSeriesBy1Minute = tsMeteoData.SplitByTimeSpan(new TimeSpan(0, 1, 0));
            List<double> lSubSeriesEntriesCount = subSeriesBy1Minute.ConvertAll(subs => (double)subs.Count);
            DescriptiveStatistics statsCounts = new DescriptiveStatistics(lSubSeriesEntriesCount);
            int aveMinuteEntriesCount = Convert.ToInt32(statsCounts.Mean);
            List<MeteoData> meteoDataList = tsMeteoData.DataValues;

            DateTime maxDateTime = tsMeteoData.TimeStamps.Max();
            List<double> currFileSecondsList = new List<double>(); // = tsMeteoData.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);

            MultipleScatterAndFunctionsRepresentation fRenderer = new MultipleScatterAndFunctionsRepresentation(imgSize);

            double minVarValue = 0.0d;
            double maxVarValue = 1.0d;
            Bgr currValueColor = new Bgr(Color.Black);
            List<double> currVarToShowValues = new List<double>();
            if (rbtnPressureGraph.Checked)
            {
                currVarToShowValues = meteoDataList.ConvertAll(mdt => mdt.pressure);

                TimeSeries<double> currVarTS = new TimeSeries<double>(currVarToShowValues, tsMeteoData.TimeStamps);
                currVarTS.RemoveValues(dVal => dVal <= 900.0d);

                currVarToShowValues = new List<double>(currVarTS.DataValues);
                currFileSecondsList = currVarTS.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);

                currValueColor = new Bgr(Color.Blue);
            }
            else if (rbtnAirTempGraph.Checked)
            {
                currVarToShowValues = meteoDataList.ConvertAll(mdt => mdt.airTemperature);

                TimeSeries<double> currVarTS = new TimeSeries<double>(currVarToShowValues, tsMeteoData.TimeStamps);
                currVarTS.RemoveValues(dVal => ((dVal < -20.0d) || (dVal > 50.0d)));

                currVarToShowValues = new List<double>(currVarTS.DataValues);
                currFileSecondsList = currVarTS.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);

                currValueColor = new Bgr(Color.Red);
            }
            else if (rbtnWaterTempGraph.Checked)
            {
                currVarToShowValues = meteoDataList.ConvertAll(mdt => mdt.waterTemperature);

                TimeSeries<double> currVarTS = new TimeSeries<double>(currVarToShowValues, tsMeteoData.TimeStamps);
                currVarTS.RemoveValues(dVal => ((dVal < -20.0d) || (dVal > 50.0d)));

                currVarToShowValues = new List<double>(currVarTS.DataValues);
                currFileSecondsList = currVarTS.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);

                currValueColor = new Bgr(Color.RoyalBlue);
            }
            else if (rbtnWindSpeedGraph.Checked)
            {
                currVarToShowValues = meteoDataList.ConvertAll(mdt => mdt.windSpeed);

                TimeSeries<double> currVarTS = new TimeSeries<double>(currVarToShowValues, tsMeteoData.TimeStamps);
                currVarTS.RemoveValues(dVal => ((dVal < 0.0d) || (dVal > 50.0d)));

                currVarToShowValues = new List<double>(currVarTS.DataValues);
                currFileSecondsList = currVarTS.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);

                currValueColor = new Bgr(Color.Gray);
            }

            DenseVector dvVarValues = DenseVector.OfEnumerable(currVarToShowValues);
            dvVarValues = dvVarValues.Conv(StandardConvolutionKernels.gauss, aveMinuteEntriesCount * 10);



            fRenderer.dvScatterFuncValues.Add(dvVarValues);
            fRenderer.dvScatterXSpace.Add(DenseVector.OfEnumerable(currFileSecondsList));
            fRenderer.scatterLineColors.Add(currValueColor);
            fRenderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);
            fRenderer.xSpaceMin = currFileSecondsList.Min();
            fRenderer.xSpaceMax = currFileSecondsList.Max();
            fRenderer.overallFuncMin = dvVarValues.Min();
            fRenderer.overallFuncMax = dvVarValues.Max();
            fRenderer.fixSpecifiedMargins = true;

            fRenderer.Represent();

            Image<Bgr, byte> retImg = fRenderer.TheImage;

            // расположим надпись
            string strSign = "current value: " + dvVarValues.Last().ToString("F2");

            List<TextBarImage> textBarsCases = new List<TextBarImage>();

            TextBarImage tbimTopLeftSign = new TextBarImage(strSign, retImg);
            tbimTopLeftSign.PtSurroundingBarStart =
                new Point(fRenderer.ServiceSpaceGapX + tbimTopLeftSign.textHalfHeight,
                    fRenderer.ServiceSpaceGapY + tbimTopLeftSign.textHalfHeight);
            textBarsCases.Add(tbimTopLeftSign);

            TextBarImage tbimBtmLeftSign = new TextBarImage(strSign, retImg);
            tbimBtmLeftSign.PtSurroundingBarStart = new Point(fRenderer.ServiceSpaceGapX + tbimBtmLeftSign.textHalfHeight,
                retImg.Height - fRenderer.ServiceSpaceGapY - tbimBtmLeftSign.textHalfHeight - tbimBtmLeftSign.textHeight * 2);
            textBarsCases.Add(tbimBtmLeftSign);

            TextBarImage tbimTopRightSign = new TextBarImage(strSign, retImg);
            tbimTopRightSign.PtSurroundingBarStart =
                new Point(
                    retImg.Width - fRenderer.ServiceSpaceGapX - tbimTopRightSign.textHalfHeight -
                    tbimTopRightSign.textBarSize.Width, fRenderer.ServiceSpaceGapY + tbimTopLeftSign.textHalfHeight);
            textBarsCases.Add(tbimTopRightSign);

            TextBarImage tbimBtmRightSign = new TextBarImage(strSign, retImg);
            tbimBtmRightSign.PtSurroundingBarStart =
                new Point(
                    retImg.Width - fRenderer.ServiceSpaceGapX - tbimBtmRightSign.textHalfHeight -
                    tbimBtmRightSign.textBarSize.Width,
                    retImg.Height - fRenderer.ServiceSpaceGapY - tbimBtmRightSign.textHalfHeight -
                    tbimBtmRightSign.textHeight * 2);
            textBarsCases.Add(tbimBtmRightSign);

            textBarsCases.Sort((case1, case2) => (case1.SubImageInTextRect.CountNonzero().Sum() > case2.SubImageInTextRect.CountNonzero().Sum()) ? 1 : -1);

            MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 2.0d, 2.0d)
            {
                thickness = 2,
            };
            retImg.Draw(strSign, ref theFont, textBarsCases[0].ptTextBaselineStart, new Bgr(Color.Green));
            retImg.Draw(textBarsCases[0].rectSurroundingBar, new Bgr(Color.Green), 2);


            return retImg;
        }



        private class TextBarImage
        {
            public string strText = "";
            public MCvFont fnTextFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 2.0d, 2.0d)
            {
                thickness = 2,
            };
            public Point ptTextBaselineStart;
            private Point ptSurroundingBarStart;
            public Rectangle rectSurroundingBar;
            public readonly Size textBarSize;
            public readonly int textHalfHeight;
            public readonly int textHeight;
            private Image<Bgr, byte> originalImage;



            public Point PtSurroundingBarStart
            {
                get { return ptSurroundingBarStart; }
                set
                {
                    ptSurroundingBarStart = value;
                    rectSurroundingBar = new Rectangle(ptSurroundingBarStart, textBarSize);
                    ptTextBaselineStart = ptSurroundingBarStart + new Size(textHalfHeight, textHeight * 2 - textHalfHeight);
                }

            }


            public TextBarImage(string text, Image<Bgr, byte> origImage)
            {
                strText = text;
                Size retTextSize = new Size(0, 0);
                int baseline = 0;
                CvInvoke.cvGetTextSize(strText, ref fnTextFont, ref retTextSize, ref baseline);
                textHeight = retTextSize.Height + baseline;
                textHalfHeight = Convert.ToInt32(textHeight / 2.0d);
                textBarSize = new System.Drawing.Size(retTextSize.Width + textHeight, textHeight * 2);
                originalImage = origImage.Copy();
            }


            public Image<Bgr, byte> SubImageInTextRect
            {
                get
                {
                    originalImage.ROI = rectSurroundingBar;
                    Image<Bgr, byte> subImg = originalImage.Copy();
                    originalImage.ROI = Rectangle.Empty;
                    return subImg;
                }
            }
        }





        private void rbtnGraphVarChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderAndShowGraph(null);
            }
        }




        private void btnGraphMoveFocusLeft_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = false;
            graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1.AddHours(-12), graphsTimeSpan.Item2.AddHours(-12));
            RenderAndShowGraph(null);
        }



        private void btnGraphMoveFocusRight_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = false;
            graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1.AddHours(12), graphsTimeSpan.Item2.AddHours(12));
            if (graphsTimeSpan.Item2 > DateTime.UtcNow)
            {
                graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1 + (DateTime.UtcNow - graphsTimeSpan.Item2), DateTime.UtcNow);
            }
            RenderAndShowGraph(null);
        }



        private void btnGraphDefault_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = true;
            graphsTimeSpan = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            RenderAndShowGraph(null);
        }




        private void btnGraphZoomOut_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = false;
            graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1.AddHours(-6), graphsTimeSpan.Item2.AddHours(6));
            if (graphsTimeSpan.Item2 > DateTime.UtcNow)
            {
                graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1, DateTime.UtcNow);
            }
            RenderAndShowGraph(null);
        }



        private void btnGraphZoomIn_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = false;
            graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1.AddHours(6), graphsTimeSpan.Item2.AddHours(-6));

            if (graphsTimeSpan.Item1 >= graphsTimeSpan.Item2)
            {
                btnGraphDefault_Click(null, null);
            }
            else
            {
                RenderAndShowGraph(null);
            }
        }



        #endregion graphs representing






        #region backward reading and showing nav,meteo data

        private IoffeVesselDualNavDataConverted neededNavdata = null;
        private IoffeVesselMetDataConverted neededMeteodata = null;
        private BackgroundWorker bgwInfoSearcher = null;
        private void btnFindInfo_Click(object sender, EventArgs e)
        {
            if (bgwInfoSearcher != null)
            {
                if (bgwInfoSearcher.IsBusy)
                {
                    bgwInfoSearcher.CancelAsync();
                    return;
                }
            }

            DateTime curDateTime = DateTime.UtcNow;

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "started at " + DateTime.Now.ToString("u").Replace("Z", ""));

            if (!DateTime.TryParse(maskedTextBox1.Text, out curDateTime))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t parse date-time.");
                return;
            }

            curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);


            bgwInfoSearcher = new BackgroundWorker();
            bgwInfoSearcher.WorkerReportsProgress = true;
            bgwInfoSearcher.WorkerSupportsCancellation = true;
            bgwInfoSearcher.DoWork += ((obj, evArgs) =>
            {
                BackgroundWorker selfWorker = obj as BackgroundWorker;
                object[] inArgs = (object[])evArgs.Argument;
                Dictionary<string, object> defProperties = inArgs[0] as Dictionary<string, object>;
                LogWindow bgwLogWindow = inArgs[1] as LogWindow;

                string navMetFilesPath = defProperties["IoffeNavMeteoDataFilesDirectory"] as string;
                List<IoffeVesselDualNavDataConverted> lAllNavData = new List<IoffeVesselDualNavDataConverted>();

                string[] sNavFilenames = Directory.GetFiles(navMetFilesPath, "*.nv2", SearchOption.AllDirectories);
                string[] sMetFilenames = Directory.GetFiles(navMetFilesPath, "*.met", SearchOption.AllDirectories);

                int totalFilesCount = sNavFilenames.Count() + sMetFilenames.Count();
                int ProgressPercentagePrev = 0;
                int processedFiles = 0;

                if (!sNavFilenames.Any())
                {
                    bgwLogWindow = ServiceTools.LogAText(bgwLogWindow, "Не найдено файлов данных навигации", true);
                    return;
                }
                else
                {
                    foreach (string navFilename in sNavFilenames)
                    {
                        processedFiles++;
                        int ProgressPercentage = Convert.ToInt32(100.0d * processedFiles / totalFilesCount);
                        if (ProgressPercentage > ProgressPercentagePrev)
                        {
                            ProgressPercentagePrev = ProgressPercentage;
                            selfWorker.ReportProgress(ProgressPercentage);
                        }


                        if (selfWorker.CancellationPending)
                        {
                            break;
                        }

                        Tuple<DateTime, DateTime> timeSpan =
                            IoffeVesselNavDataReader.GetNavFileDateTimeMargins(navFilename);
                        if (timeSpan == null)
                        {
                            continue;
                        }

                        if ((curDateTime < timeSpan.Item1) || (curDateTime > timeSpan.Item2))
                        {
                            continue;
                        }

                        List<IoffeVesselDualNavDataConverted> dataHasBeenRead =
                            IoffeVesselNavDataReader.ReadNavFile(navFilename);
                        if (dataHasBeenRead == null)
                        {
                            continue;
                        }
                        bgwLogWindow = ServiceTools.LogAText(bgwLogWindow, "файл навигации прочитан: " + navFilename,
                            true);
                        Application.DoEvents();
                        lAllNavData.AddRange(dataHasBeenRead);
                    }
                }

                if (selfWorker.CancellationPending)
                {
                    evArgs.Result = null;
                    return;
                }

                lAllNavData.Sort((gpsRecord1, gpsRecord2) =>
                {
                    double dev1 = Math.Abs((gpsRecord1.gps.dateTimeUTC - curDateTime).TotalMilliseconds);
                    double dev2 = Math.Abs((gpsRecord2.gps.dateTimeUTC - curDateTime).TotalMilliseconds);
                    return (dev1 >= dev2) ? (1) : (-1);
                });
                IoffeVesselDualNavDataConverted foundNavdata = lAllNavData[0];


                List<IoffeVesselMetDataConverted> lAllMetData = new List<IoffeVesselMetDataConverted>();


                if (!sMetFilenames.Any())
                {
                    bgwLogWindow = ServiceTools.LogAText(bgwLogWindow, "Не найдено файлов данных метео", true);
                    return;
                }
                else
                {
                    foreach (string meteoFilename in sMetFilenames)
                    {
                        processedFiles++;
                        int ProgressPercentage = Convert.ToInt32(100.0d * processedFiles / totalFilesCount);
                        if (ProgressPercentage > ProgressPercentagePrev)
                        {
                            ProgressPercentagePrev = ProgressPercentage;
                            selfWorker.ReportProgress(ProgressPercentage);
                        }


                        if (selfWorker.CancellationPending)
                        {
                            break;
                        }

                        Tuple<DateTime, DateTime> timeSpan =
                            IoffeVesselMeteoDataReader.GetMetFileDateTimeMargins(meteoFilename);
                        if (timeSpan == null)
                        {
                            continue;
                        }

                        if ((curDateTime < timeSpan.Item1) || (curDateTime > timeSpan.Item2))
                        {
                            continue;
                        }

                        List<IoffeVesselMetDataConverted> dataHasBeenRead =
                            IoffeVesselMeteoDataReader.ReadMetFile(meteoFilename);
                        if (dataHasBeenRead == null)
                        {
                            continue;
                        }
                        bgwLogWindow = ServiceTools.LogAText(bgwLogWindow,
                            "файл метеоданных прочитан: " + meteoFilename, true);
                        Application.DoEvents();
                        lAllMetData.AddRange(dataHasBeenRead);
                    }
                }

                if (selfWorker.CancellationPending)
                {
                    evArgs.Result = null;
                    return;
                }

                lAllMetData.Sort((gpsRecord1, gpsRecord2) =>
                {
                    double dev1 = Math.Abs((gpsRecord1.dateTime - curDateTime).TotalMilliseconds);
                    double dev2 = Math.Abs((gpsRecord2.dateTime - curDateTime).TotalMilliseconds);
                    return (dev1 >= dev2) ? (1) : (-1);
                });
                IoffeVesselMetDataConverted foundMeteodata = lAllMetData[0];


                evArgs.Result = new object[] { foundNavdata, foundMeteodata };
            });


            bgwInfoSearcher.RunWorkerCompleted += ((obj, evArgs) =>
            {
                if (evArgs.Result == null)
                {
                    return;
                }

                object[] res = evArgs.Result as object[];
                neededNavdata = res[0] as IoffeVesselDualNavDataConverted;
                neededMeteodata = res[1] as IoffeVesselMetDataConverted;
                ThreadSafeOperations.UpdateProgressBar(prbSearchingProgress, 0);
                ThreadSafeOperations.ToggleButtonState(btnFindInfo, true, "find closest records", true);

                ThreadSafeOperations.SetTextTB(lblFoundNavData, neededNavdata.ToString() + Environment.NewLine + "sun position: " + neededNavdata.gps.SunZenithAzimuth().ToString(), false);
                ThreadSafeOperations.SetTextTB(lblFoundMetData, neededMeteodata.ToString(), false);

                bgwInfoSearcher.Dispose();
                bgwInfoSearcher = null;
            });


            bgwInfoSearcher.ProgressChanged += ((bgwSender, prChangedArgs) =>
            {
                ThreadSafeOperations.UpdateProgressBar(prbSearchingProgress, prChangedArgs.ProgressPercentage);
            });


            object[] bgwInfoSearcherArgs = new object[] { defaultProperties, theLogWindow };
            bgwInfoSearcher.RunWorkerAsync(bgwInfoSearcherArgs);

            ThreadSafeOperations.ToggleButtonState(btnFindInfo, true, "CANCEL", true);
        }






        private void btnWriteFile_Click(object sender, EventArgs e)
        {
            if (neededNavdata == null || neededMeteodata == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "can`t write data - it hasn`t been found yet. Use \"find closest records\" first.");
                return;
            }



            string filename = Directory.GetCurrentDirectory() + "\\logs\\ObservationsData-" +
                                      neededNavdata.dateTime.ToString("yyyy-MM-dd-HH-mm") + ".log";

            if (!File.Exists(filename))
            {
                string strDataToObservationsLog = neededNavdata.dateTime.ToString("u").Replace("Z", "") + "; ";

                GPSdata usingGPSdata = neededNavdata.gps;
                MeteoData usingMeteoData = new MeteoData()
                {
                    pressure = neededMeteodata.pressure,
                    Rhumidity = neededMeteodata.rHumidity,
                    airTemperature = neededMeteodata.tAir,
                    windSpeed = neededMeteodata.windSpeed,
                    windDirection = neededMeteodata.windDirection,
                    waterTemperature = 0.0d,
                    waterSalinity = 0.0d,
                };

                if (usingGPSdata != null)
                {
                    SPA spaCalc = new SPA(usingGPSdata.dateTimeUTC.Year, usingGPSdata.dateTimeUTC.Month,
                        usingGPSdata.dateTimeUTC.Day, usingGPSdata.dateTimeUTC.Hour,
                        usingGPSdata.dateTimeUTC.Minute, usingGPSdata.dateTimeUTC.Second,
                        (float)usingGPSdata.LonDec, (float)usingGPSdata.LatDec,
                        (float)SPAConst.DeltaT(usingGPSdata.dateTimeUTC));
                    int res = spaCalc.spa_calculate();
                    AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                        spaCalc.spa.zenith);
                    double sunElevCalc = sunPositionSPAext.ElevationAngle;
                    double sunAzimuth = sunPositionSPAext.Azimuth;



                    strDataToObservationsLog += usingGPSdata.Lat.ToString().Replace(".", ",") + "; ";
                    strDataToObservationsLog += usingGPSdata.Lon.ToString().Replace(".", ",") + "; ";
                    strDataToObservationsLog += sunElevCalc.ToString("F2").Replace(".", ",") + "; "; // Sun elevation
                    strDataToObservationsLog +=
                        usingGPSdata.IOFFEdataHeadingTrue.ToString().Replace(".", ",") + "; ";
                    strDataToObservationsLog +=
                        usingGPSdata.IOFFEdataHeadingGyro.ToString().Replace(".", ",") + "; ";
                    strDataToObservationsLog += usingGPSdata.IOFFEdataSpeedKnots.ToString().Replace(".", ",") +
                                                "; ";
                }

                if (usingMeteoData != null)
                {
                    strDataToObservationsLog += usingMeteoData.pressure.ToString().Replace(".", ",") + "; ";
                    strDataToObservationsLog += usingMeteoData.airTemperature.ToString().Replace(".", ",") +
                                                "; ";
                    strDataToObservationsLog += usingMeteoData.waterTemperature.ToString().Replace(".", ",") +
                                                "; ";
                    strDataToObservationsLog += usingMeteoData.windSpeed.ToString().Replace(".", ",") + "; ";
                    strDataToObservationsLog += usingMeteoData.windDirection.ToString().Replace(".", ",") +
                                                "; ";
                    strDataToObservationsLog += usingMeteoData.Rhumidity.ToString().Replace(".", ",") + "; ";
                }

                if (cbLogMeasurementsData.Checked)
                {
                    ServiceTools.logToTextFile(filename, strDataToObservationsLog, true);
                }
            }
        }



        private void lblPlusOneHour_Click(object sender, EventArgs e)
        {
            DateTime curDateTime = DateTime.UtcNow;
            if (!DateTime.TryParse(maskedTextBox1.Text, out curDateTime))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t parse date-time.");
                return;
            }
            curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);

            curDateTime = curDateTime.AddHours(1);
            maskedTextBox1.Text = curDateTime.ToString("s");

            btnFindInfo_Click(null, null);
        }



        private void btnPlusOneDay_Click(object sender, EventArgs e)
        {
            DateTime curDateTime = DateTime.UtcNow;
            if (!DateTime.TryParse(maskedTextBox1.Text, out curDateTime))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t parse date-time.");
                return;
            }
            curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);

            curDateTime = curDateTime.AddDays(1);
            maskedTextBox1.Text = curDateTime.ToString("s");

            btnFindInfo_Click(null, null);
        }

        #endregion backward reading and showing nav,meteo data






        #region backward repair missing data of nc-files using IOFFE met-files data

        private List<IoffeVesselDualNavDataConverted> lFoundNavData = null;
        private List<IoffeVesselMetDataConverted> lFoundMeteoData = null;
        private BackgroundWorker bgwMissingInfoSearcher = null;
        private void btnFindIOFFEdataFiles_Click(object sender, EventArgs e)
        {
            if (bgwMissingInfoSearcher != null)
            {
                if (bgwMissingInfoSearcher.IsBusy)
                {
                    bgwMissingInfoSearcher.CancelAsync();
                    return;
                }
            }

            DateTime curDateTime = DateTime.UtcNow;

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "started at " + DateTime.Now.ToString("u").Replace("Z", ""));

            if (!DateTime.TryParse(maskedTextBox2.Text, out curDateTime))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t parse date-time.");
                return;
            }

            curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);


            bgwMissingInfoSearcher = new BackgroundWorker();
            bgwMissingInfoSearcher.WorkerReportsProgress = true;
            bgwMissingInfoSearcher.WorkerSupportsCancellation = true;

            bgwMissingInfoSearcher.DoWork += ((obj, evArgs) =>
            {
                BackgroundWorker selfWorker = obj as BackgroundWorker;
                object[] inArgs = (object[])evArgs.Argument;
                Dictionary<string, object> defProperties = inArgs[0] as Dictionary<string, object>;
                LogWindow bgwLogWindow = inArgs[1] as LogWindow;

                string navMetFilesPath = defProperties["IoffeNavMeteoDataFilesDirectory"] as string;
                List<IoffeVesselDualNavDataConverted> lAllNavData = new List<IoffeVesselDualNavDataConverted>();

                string[] sNavFilenames = Directory.GetFiles(navMetFilesPath, "*.nv2", SearchOption.AllDirectories);
                string[] sMetFilenames = Directory.GetFiles(navMetFilesPath, "*.met", SearchOption.AllDirectories);

                #region NAV data

                int totalFilesCount = sNavFilenames.Count() + sMetFilenames.Count();
                int ProgressPercentagePrev = 0;
                int processedFiles = 0;

                if (!sNavFilenames.Any())
                {
                    bgwLogWindow = ServiceTools.LogAText(bgwLogWindow, "Не найдено подходящих файлов данных навигации", true);
                    return;
                }
                else
                {
                    foreach (string navFilename in sNavFilenames)
                    {
                        processedFiles++;
                        int ProgressPercentage = Convert.ToInt32(100.0d * processedFiles / totalFilesCount);
                        if (ProgressPercentage > ProgressPercentagePrev)
                        {
                            ProgressPercentagePrev = ProgressPercentage;
                            selfWorker.ReportProgress(ProgressPercentage);
                        }


                        if (selfWorker.CancellationPending)
                        {
                            break;
                        }

                        Tuple<DateTime, DateTime> timeSpan =
                            IoffeVesselNavDataReader.GetNavFileDateTimeMargins(navFilename);
                        if (timeSpan == null)
                        {
                            continue;
                        }

                        if ((curDateTime.Date != timeSpan.Item1.Date) && (curDateTime.Date != timeSpan.Item2.Date))
                        {
                            continue;
                        }

                        List<IoffeVesselDualNavDataConverted> dataHasBeenRead =
                            IoffeVesselNavDataReader.ReadNavFile(navFilename);
                        if (dataHasBeenRead == null)
                        {
                            continue;
                        }
                        bgwLogWindow = ServiceTools.LogAText(bgwLogWindow, "файл навигации прочитан: " + navFilename,
                            true);
                        Application.DoEvents();
                        lAllNavData.AddRange(dataHasBeenRead);
                    }
                }

                if (selfWorker.CancellationPending)
                {
                    evArgs.Result = null;
                    return;
                }

                lAllNavData.Sort((gpsRecord1, gpsRecord2) => (gpsRecord1.gps.dateTimeUTC >= gpsRecord2.gps.dateTimeUTC) ? (1) : (-1));

                #endregion NAV data

                #region MET data

                List<IoffeVesselMetDataConverted> lAllMetData = new List<IoffeVesselMetDataConverted>();


                if (!sMetFilenames.Any())
                {
                    bgwLogWindow = ServiceTools.LogAText(bgwLogWindow, "Не найдено подходящих файлов данных метео", true);
                    return;
                }
                else
                {
                    foreach (string meteoFilename in sMetFilenames)
                    {
                        processedFiles++;
                        int ProgressPercentage = Convert.ToInt32(100.0d * processedFiles / totalFilesCount);
                        if (ProgressPercentage > ProgressPercentagePrev)
                        {
                            ProgressPercentagePrev = ProgressPercentage;
                            selfWorker.ReportProgress(ProgressPercentage);
                        }


                        if (selfWorker.CancellationPending)
                        {
                            break;
                        }

                        Tuple<DateTime, DateTime> timeSpan =
                            IoffeVesselMeteoDataReader.GetMetFileDateTimeMargins(meteoFilename);
                        if (timeSpan == null)
                        {
                            continue;
                        }

                        if ((curDateTime.Date != timeSpan.Item1.Date) && (curDateTime.Date != timeSpan.Item2.Date))
                        {
                            continue;
                        }

                        List<IoffeVesselMetDataConverted> dataHasBeenRead =
                            IoffeVesselMeteoDataReader.ReadMetFile(meteoFilename);
                        if (dataHasBeenRead == null)
                        {
                            continue;
                        }
                        bgwLogWindow = ServiceTools.LogAText(bgwLogWindow,
                            "файл метеоданных прочитан: " + meteoFilename, true);
                        Application.DoEvents();
                        lAllMetData.AddRange(dataHasBeenRead);
                    }
                }

                if (selfWorker.CancellationPending)
                {
                    evArgs.Result = null;
                    return;
                }

                lAllMetData.Sort((metRecord1, metRecord2) => (metRecord1.dateTime >= metRecord2.dateTime) ? (1) : (-1));

                #endregion MET data

                evArgs.Result = new object[] { lAllNavData, lAllMetData };
            });


            bgwMissingInfoSearcher.RunWorkerCompleted += ((obj, evArgs) =>
            {
                if (evArgs.Result == null)
                {
                    return;
                }

                object[] res = evArgs.Result as object[];
                lFoundNavData = res[0] as List<IoffeVesselDualNavDataConverted>;
                lFoundMeteoData = res[1] as List<IoffeVesselMetDataConverted>;
                ThreadSafeOperations.UpdateProgressBar(prbMissingInfoSearchingProgress, 0);
                ThreadSafeOperations.ToggleButtonState(btnFindIOFFEdataFiles, true, "find IOFFE data files", true);

                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "finished at " + DateTime.Now.ToString("u").Replace("Z", ""));

                bgwMissingInfoSearcher.Dispose();
                bgwMissingInfoSearcher = null;
            });


            bgwMissingInfoSearcher.ProgressChanged += ((bgwSender, prChangedArgs) =>
            {
                ThreadSafeOperations.UpdateProgressBar(prbMissingInfoSearchingProgress, prChangedArgs.ProgressPercentage);
            });


            object[] bgwMissingInfoSearcherArgs = new object[] { defaultProperties, theLogWindow };
            bgwMissingInfoSearcher.RunWorkerAsync(bgwMissingInfoSearcherArgs);

            ThreadSafeOperations.ToggleButtonState(btnFindIOFFEdataFiles, true, "CANCEL", true);
        }






        private void btnRepairAndWriteNCfile_Click(object sender, EventArgs e)
        {
            if (lFoundNavData == null || lFoundMeteoData == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "can`t write data - it hasn`t been found yet. Use \"find closest records\" first.");
                return;
            }

            DateTime curDateTime = DateTime.UtcNow;

            if (!DateTime.TryParse(maskedTextBox2.Text, out curDateTime))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t parse date-time.");
                return;
            }

            curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);

            /// todo: сделать и для GPS
            //string ncFileNameNav = Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-GPSDataLog-" +
            //                       curDateTime.Date.ToString("yyyy-MM-dd") + ".nc";

            string ncFileNameMet = Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-MeteoDataLog-" +
                                   curDateTime.Date.ToString("yyyy-MM-dd") + ".nc";

            /// todo: сделать и для GPS
            //File.Copy(ncFileNameNav, ncFileNameNav + ".bak");


            File.Copy(ncFileNameMet, ncFileNameMet + ".bak");




            #region meteo


            Dictionary<string, object> dictMeteoFileData = null;


            try
            {
                dictMeteoFileData = NetCDFoperations.ReadDataFromFile(ncFileNameMet);
            }
            catch (Exception)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "не получилось прочитать файл данных метео, который надо корректировать.");
                return;
            }

            if (dictMeteoFileData == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "не получилось прочитать файл данных метео, который надо корректировать.");
                return;
            }

            string varNameDateTime = "DateTime";
            List<long> currFileDateTimeLongTicksList = new List<long>((dictMeteoFileData[varNameDateTime] as long[]));
            List<DateTime> currFileDateTimeList =
                currFileDateTimeLongTicksList.ConvertAll(longVal => new DateTime(longVal));
            string varNameMeteoData = "MeteoData";
            List<MeteoData> currFileMeteoDataList =
                MeteoData.OfDenseMatrix(dictMeteoFileData[varNameMeteoData] as DenseMatrix);

            TimeSeries<MeteoData> tsMeteoData = new TimeSeries<MeteoData>(currFileMeteoDataList, currFileDateTimeList);
            tsMeteoData.SortByTimeStamps();

            List<DateTime> lFoundMeteoDataDTvalues = lFoundMeteoData.ConvertAll(foundMetDatum => foundMetDatum.dateTime);
            List<MeteoData> lFoundMeteoDataMeteoValues = lFoundMeteoData.ConvertAll(foundMetDatum => new MeteoData()
            {
                pressure = foundMetDatum.pressure,
                Rhumidity = foundMetDatum.rHumidity,
                airTemperature = foundMetDatum.tAir,
                windSpeed = foundMetDatum.windSpeed,
                windDirection = foundMetDatum.windDirection,
                waterTemperature = 0.0d,
                waterSalinity = 0.0d,
            });

            tsMeteoData.AddSubseriaData(lFoundMeteoDataMeteoValues, lFoundMeteoDataDTvalues);

            Dictionary<string, object> dataToSave = new Dictionary<string, object>();

            dataToSave.Add("DateTime", tsMeteoData.TimeStamps.ConvertAll(dt => dt.Ticks).ToArray());

            DenseMatrix dvMeteoDataVectorToAdd = MeteoData.ToDenseMatrix(tsMeteoData.DataValues);

            dataToSave.Add("MeteoData", dvMeteoDataVectorToAdd);

            File.Delete(ncFileNameMet);
            NetCDFoperations.AddVariousDataToFile(dataToSave, ncFileNameMet);


            #endregion

        }




        #endregion backward repair missing data of nc-files using IOFFE met-files data






        #region // SeaBird SeaSave processor

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (!bgwSeaSaveSocketStreamReader.IsBusy)
        //    {
        //        scktSeaSave = ConnectSocket("169.254.249.87", 49161);
        //        //scktSeaSave = ConnectSocket("192.168.192.218", 4001);
        //        if (scktSeaSave == null) return;
        //        bgwSeaSaveSocketStreamReader.RunWorkerAsync(new object[] { scktSeaSave });
        //        ThreadSafeOperations.ToggleButtonState(btnStartStopSeaSave, true, "STOP", true);
        //    }
        //    else
        //    {
        //        bgwSeaSaveSocketStreamReader.CancelAsync();
        //        ThreadSafeOperations.ToggleButtonState(btnStartStopSeaSave, true, "CONNECT SeaSave", true);
        //    }
        //}




        //private void bgwSeaSaveSocketStreamReader_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker selfWorker = sender as BackgroundWorker;
        //    Socket sckt = ((object[])e.Argument)[0] as Socket;


        //    Byte[] bytesReceived = new Byte[1024];

        //    int bytes = 0;
        //    string gotString = "";
        //    do
        //    {
        //        if (selfWorker.CancellationPending)
        //        {
        //            sckt.Disconnect(false);
        //            sckt.Dispose();
        //            sckt = null;
        //            break;
        //        }

        //        if (sckt != null)
        //        {
        //            bytes = sckt.Receive(bytesReceived, bytesReceived.Length, 0);
        //            gotString = Encoding.ASCII.GetString(bytesReceived, 0, bytes);

        //            ThreadSafeOperations.SetTextTB(tbSeaSaveLog, gotString + Environment.NewLine, true);

        //            //string[] gotStrings = gotString.Split('%');
        //            //foreach (string s in gotStrings)
        //            //{
        //            //    quStreamTextStrings.Enqueue(s);
        //            //}
        //        }
        //    }
        //    while (bytes > 0);
        //}

        #endregion // SeaBird SeaSave processor


    }
}
