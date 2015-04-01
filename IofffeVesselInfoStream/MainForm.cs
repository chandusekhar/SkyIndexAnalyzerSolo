using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyImagesAnalyzerLibraries;


namespace IofffeVesselInfoStream
{
    public partial class MainForm : Form
    {
        public static Queue<string> quStreamTextStrings = new Queue<string>();
        private Socket s;
        private Socket scktSeaSave;

        private Queue<GPSdata> quGPSDataQueue = new Queue<GPSdata>();
        private Queue<MeteoData> quMeteoDataQueue = new Queue<MeteoData>();

        private static GPSdata actualGPSdata = null;
        private static MeteoData actualMeteoData = null;

        private static GeoTrackRenderer geoRenderer = new GeoTrackRenderer(Directory.GetCurrentDirectory() + "\\geogrid\\regrid.nc");

        //private static LogWindow theLogWindow = null;
        //private static Queue<string> quTextToLog = new Queue<string>();



        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (bgwSocketStreamReader.IsBusy)
            {
                bgwSocketStreamReader.CancelAsync();
                bgwStreamTextParser.CancelAsync();
                bgwStreamDataProcessing.CancelAsync();
                bgwGraphsRenderer.CancelAsync();
                ThreadSafeOperations.ToggleButtonState(btnConnect, true, "CONNECT", true);
            }
            else
            {
                s = ConnectSocket("169.254.249.87", 1977);
                if (s == null) return;
                else
                {
                    bgwStreamDataProcessing.RunWorkerAsync();
                    bgwStreamTextParser.RunWorkerAsync();
                    bgwSocketStreamReader.RunWorkerAsync(new object[] { s });
                    ThreadSafeOperations.ToggleButtonState(btnConnect, true, "STOP", true);
                    if (cbShowGeoTrack.Checked)
                    {
                        bgwGraphsRenderer.RunWorkerAsync();
                    }
                }
            }
        }



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
                MessageBox.Show("====== coudn`t connect to remote server ======", "ERROR", MessageBoxButtons.OK,
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
                        quStreamTextStrings.Enqueue(s);
                    }
                }
            }
            while (bytes > 0);
        }





        private void bgwStreamTextParser_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            string strCurrentCatchedStreamString = "";
            List<string> listStreamLogStrings = new List<string>();

            while (true)
            {
                if (selfWorker.CancellationPending)
                {
                    break;
                }

                if (quStreamTextStrings.Count > 0)
                {
                    strCurrentCatchedStreamString = quStreamTextStrings.Dequeue();
                    if (strCurrentCatchedStreamString == null)
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                        continue;
                    }

                    if (strCurrentCatchedStreamString != "")
                    {
                        //ThreadSafeOperations.SetTextTB(tbLog, strCurrentCatchedStreamString + Environment.NewLine, true);

                        listStreamLogStrings.Add(strCurrentCatchedStreamString);

                        if (listStreamLogStrings.Count >= 30)
                        {
                            string filename = Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-log-" +
                                              DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".log";

                            string strToWrite = "";
                            foreach (string logString in listStreamLogStrings) strToWrite += logString + Environment.NewLine;

                            if (cbLogNCdata.Checked)
                            {
                                ServiceTools.logToTextFile(filename, strToWrite, true);
                            }
                            listStreamLogStrings.Clear();
                        }

                        ParseStreamString(strCurrentCatchedStreamString);
                    }
                    else
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                        continue;
                    }
                }
                else
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                    continue;
                }
            }
        }




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
                    quGPSDataQueue.Enqueue(catchedGPSdataPack);
                }
                return;
            }

            if (strData.Substring(0, 5) == "Meteo")
            {
                MeteoData catchedMeteoData = new MeteoData(strData);
                if (catchedMeteoData.validMeteoData)
                {
                    quMeteoDataQueue.Enqueue(catchedMeteoData);
                }
            }

        }









        private void bgwStreamDataProcessing_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            DenseMatrix dmGPSDataMatrix = null;
            List<long> gpsDateTimeValuesList = new List<long>();

            DenseMatrix dmMeteoDataMatrix = null;
            List<long> meteoDateTimeValuesList = new List<long>();

            int gpsDataPacksCount = 0;
            Stopwatch swGPSDataRateWatch = Stopwatch.StartNew();
            double gpsDataRate = 0.0d;
            int meteoDataPacksCount = 0;
            Stopwatch swMeteoDataRateWatch = Stopwatch.StartNew();
            double meteoDataRate = 0.0d;


            while (true)
            {
                if (selfWorker.CancellationPending)
                {
                    if (dmGPSDataMatrix != null)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                        dataToSave.Add("GPSdata", dmGPSDataMatrix);

                        if (cbLogNCdata.Checked)
                        {
                            NetCDFoperations.AddVariousDataToFile(dataToSave,
                                Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-GPSDataLog-" +
                                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
                        }


                        //List<DateTime> listDateTimeValuesToUpdateTrack =
                        //    gpsDateTimeValuesList.ConvertAll(longVal => new DateTime(longVal));
                        //geoRenderer.lTracksData[0].AddGPSsubtrackData(GPSdata.OfDenseMatrix(dmGPSDataMatrix),
                        //  listDateTimeValuesToUpdateTrack);

                        dmGPSDataMatrix = null;
                        gpsDateTimeValuesList.Clear();
                    }


                    if (dmMeteoDataMatrix != null)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", meteoDateTimeValuesList.ToArray());
                        dataToSave.Add("MeteoData", dmMeteoDataMatrix);

                        if (cbLogNCdata.Checked)
                        {
                            NetCDFoperations.AddVariousDataToFile(dataToSave,
                                Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-MeteoDataLog-" +
                                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
                        }
                        

                        dmMeteoDataMatrix = null;
                        meteoDateTimeValuesList.Clear();
                    }

                    ThreadSafeOperations.SetLoadingCircleState(wcNavDataSoeedControl, false, true, Color.Red, 100);
                    ThreadSafeOperations.SetLoadingCircleState(wcMeteoDataSpeedControl, false, true, Color.Red, 100);

                    break;
                }


                #region data rate controls

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



                #endregion data rate controls


                if (quGPSDataQueue.Count > 0)
                {
                    while (quGPSDataQueue.Count > 0)
                    {
                        GPSdata currQueuedGPSdata = quGPSDataQueue.Dequeue();
                        if (currQueuedGPSdata == null)
                        {
                            break;
                        }

                        gpsDataPacksCount++;

                        gpsDateTimeValuesList.Add(currQueuedGPSdata.dateTimeUTC.Ticks);

                        actualGPSdata = currQueuedGPSdata;

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



                    if (actualGPSdata != null)
                    {
                        ThreadSafeOperations.SetTextTB(tbGPSlatValue, actualGPSdata.lat.ToString() + actualGPSdata.latHemisphere, false);
                        ThreadSafeOperations.SetTextTB(tbGPSlonValue, actualGPSdata.lon.ToString() + actualGPSdata.lonHemisphere, false);
                        ThreadSafeOperations.SetTextTB(tbGPSDateTimeValue, actualGPSdata.dateTimeUTC.ToString("u").Replace("Z", ""), false);

                        // double sunHeight = actualGPSdata.SunAlt();
                        // ThreadSafeOperations.SetTextTB(tbSunAltValue, sunHeight.ToString(), false);

                        if (actualGPSdata.dataSource == GPSdatasources.IOFFEvesselDataServer)
                        {
                            //public double IOFFEdataHeadingTrue = 0.0d;
                            //public double IOFFEdataHeadingGyro = 0.0d;
                            //public double IOFFEdataSpeedKnots = 0.0d;
                            //public double IOFFEdataDepth = 0.0d;

                            ThreadSafeOperations.SetTextTB(tbTrueHeadValue,
                                actualGPSdata.IOFFEdataHeadingTrue.ToString(), false);
                            ThreadSafeOperations.SetTextTB(tbGyroHeadValue,
                                actualGPSdata.IOFFEdataHeadingGyro.ToString(), false);
                            ThreadSafeOperations.SetTextTB(tbSpeedKnotsValue,
                                actualGPSdata.IOFFEdataSpeedKnots.ToString(), false);
                            ThreadSafeOperations.SetTextTB(tbDepthValue, actualGPSdata.IOFFEdataDepth.ToString(), false);
                        }
                    }



                    if (gpsDateTimeValuesList.Count >= 50)
                    {
                        if (cbShowGeoTrack.Checked)
                        {
                            List<DateTime> listDateTimeValuesToUpdateTrack = gpsDateTimeValuesList.ConvertAll(longVal => new DateTime(longVal));

                            while (!Monitor.TryEnter(geoRenderer, 100))
                            {
                                Application.DoEvents();
                                Thread.Sleep(0);
                            }

                            try
                            {
                                geoRenderer.lTracksData[0].AddGPSsubtrackData(GPSdata.OfDenseMatrix(dmGPSDataMatrix),
                                    listDateTimeValuesToUpdateTrack);
                                if (geoRenderer.mapFollowsActualGPSposition)
                                {
                                    geoRenderer.gpsMapCenteredPoint = actualGPSdata.Clone();
                                }
                            }
                            finally
                            {
                                Monitor.Exit(geoRenderer);

                            }

                        }

                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", gpsDateTimeValuesList.ToArray());
                        dataToSave.Add("GPSdata", dmGPSDataMatrix);

                        if (cbLogNCdata.Checked)
                        {
                            NetCDFoperations.AddVariousDataToFile(dataToSave,
                                Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-GPSDataLog-" +
                                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
                        }

                        dmGPSDataMatrix = null;
                        gpsDateTimeValuesList.Clear();
                    }
                }





                if (quMeteoDataQueue.Count > 0)
                {


                    while (quMeteoDataQueue.Count > 0)
                    {
                        meteoDateTimeValuesList.Add(DateTime.UtcNow.Ticks);
                        MeteoData currQueuedMeteoData = quMeteoDataQueue.Dequeue();
                        if (currQueuedMeteoData == null)
                        {
                            break;
                        }

                        meteoDataPacksCount++;

                        actualMeteoData = currQueuedMeteoData;

                        int columnsCount = 5;

                        if (dmMeteoDataMatrix == null)
                        {
                            dmMeteoDataMatrix = currQueuedMeteoData.ToOneRowDenseMatrix();
                        }
                        else
                        {
                            DenseVector dvMeteoDataVectorToAdd = currQueuedMeteoData.ToDenseVector();
                            dmMeteoDataMatrix =
                                (DenseMatrix)dmMeteoDataMatrix.InsertRow(dmMeteoDataMatrix.RowCount, dvMeteoDataVectorToAdd);
                        }
                    }

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



                    if (meteoDateTimeValuesList.Count >= 50)
                    {
                        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
                        dataToSave.Add("DateTime", meteoDateTimeValuesList.ToArray());
                        dataToSave.Add("MeteoData", dmMeteoDataMatrix);

                        if (cbLogNCdata.Checked)
                        {
                            NetCDFoperations.AddVariousDataToFile(dataToSave,
                                Directory.GetCurrentDirectory() + "\\logs\\IoffeVesselInfoStream-MeteoDataLog-" +
                                DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".nc");
                        }

                        dmMeteoDataMatrix = null;
                        meteoDateTimeValuesList.Clear();
                    }
                }



                if (DateTime.Now.Minute == 0)
                {
                    string filename = Directory.GetCurrentDirectory() + "\\logs\\ObservationsData-" +
                                      DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".log";

                    if (!File.Exists(filename))
                    {
                        string strDataToObservationsLog = DateTime.UtcNow.ToString("u").Replace("Z", "") + "; ";

                        if (actualGPSdata != null)
                        {
                            strDataToObservationsLog += actualGPSdata.Lat.ToString().Replace(".", ",") + "; ";
                            strDataToObservationsLog += actualGPSdata.Lon.ToString().Replace(".", ",") + "; ";
                            strDataToObservationsLog += "" + "; ";
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
        }






        private static bool needToUpdateGeoTrackNow = false;
        private void bgwGraphsRenderer_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            MultipleScatterAndFunctionsRepresentation graphsRenderer =
                new MultipleScatterAndFunctionsRepresentation(pbGraphs.Size);

            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, true, true, wcUpdatimgGraphs.Color);
            if (cbShowGeoTrack.Checked)
            {
                // пропишем в рисователь трека имена файлов, содержащих логи данных GPS
                bool addedNewFiles = false;

                DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\logs\\");

                while (!Monitor.TryEnter(geoRenderer, 100))
                {
                    Application.DoEvents();
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


            Stopwatch swGraphsUpdateWatch = new Stopwatch();
            swGraphsUpdateWatch.Start();

            while (true)
            {
                if (selfWorker.CancellationPending)
                {
                    break;
                }

                if (cbShowGeoTrack.Checked)
                {
                    if ((swGraphsUpdateWatch.ElapsedMilliseconds >= 10000) || (needToUpdateGeoTrackNow))
                    {
                        UpdateGeoTrack();

                        needToUpdateGeoTrackNow = false;
                        swGraphsUpdateWatch.Restart();
                    }
                }

            }
        }


        //private static bool geoTrackIsUpdatingNow = false;
        private bool UpdateGeoTrack()
        {
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
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

                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, true, true, wcUpdatimgGraphs.Color);
                ThreadSafeOperations.UpdatePictureBox(pbGeoTrack, geoRenderer.RepresentTopo(pbGeoTrack.Size));
                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            //geoTrackIsUpdatingNow = false;

            return true;
        }



        private void ShowGeoTrackOnce()
        {
            // пропишем в рисователь трека имена файлов, содержащих логи данных GPS
            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, true, true, wcUpdatimgGraphs.Color);

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

            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
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

            if ((cbShowGeoTrack.Checked) && (!bgwGraphsRenderer.IsBusy))
            {
                ShowGeoTrackOnce();
            }
            else if ((cbShowGeoTrack.Checked) && (bgwGraphsRenderer.IsBusy))
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



        private void cbShowGeoTrack_CheckedChanged(object sender, EventArgs e)
        {
            if (((sender as CheckBox).Checked) && (!bgwGraphsRenderer.IsBusy) && (!bgwStreamDataProcessing.IsBusy))
            {
                var repl = MessageBox.Show("Wanna watch right now?", "Right now?", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (repl == System.Windows.Forms.DialogResult.Yes)
                {


                    DoWorkEventHandler bgw1DoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
                    {
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, true, true, wcUpdatimgGraphs.Color);
                        BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                        object[] currBGWarguments = (object[])args.Argument;

                        ShowGeoTrackOnce();

                        args.Result = new object[] { "" };
                    };


                    RunWorkerCompletedEventHandler currWorkCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                    {
                        object[] currentBGWResults = (object[])args.Result;
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
                    };


                    BackgroundWorker bgw1 = new BackgroundWorker();
                    bgw1.DoWork += bgw1DoWorkHandler;
                    bgw1.RunWorkerCompleted += currWorkCompletedHandler;
                    object[] BGWargs = new object[] { "" };
                    bgw1.RunWorkerAsync(BGWargs);
                }
            }
            else if ((!(sender as CheckBox).Checked) && (bgwGraphsRenderer.IsBusy))
            {
                bgwGraphsRenderer.CancelAsync();
            }
            else if (((sender as CheckBox).Checked) && (!bgwGraphsRenderer.IsBusy) && (bgwStreamDataProcessing.IsBusy))
            {
                bgwGraphsRenderer.RunWorkerAsync();
            }
        }




        private void scrbGeoTrackScrollLonValues_ValueChanged(object sender, EventArgs e)
        {
            if (scrbGeoTrackScrollLonValues.Value == 0)
            {
                return;
            }

            if (cbShowGeoTrack.Checked)
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
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, true, true, wcUpdatimgGraphs.Color);
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
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
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
                    ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
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

            if (cbShowGeoTrack.Checked)
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
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, true, true, wcUpdatimgGraphs.Color);
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
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
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
                    ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
                };

                bgwGraphUpdater.DoWork += bgwGraphUpdaterDoWorkHandler;
                bgwGraphUpdater.RunWorkerCompleted += bgwGraphUpdaterCompletedHandler;
                bgwGraphUpdater.RunWorkerAsync();
                //}
            }
        }




        private void btnCenterToActualPosition_Click(object sender, EventArgs e)
        {
            if (cbShowGeoTrack.Checked)
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
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, true, true, wcUpdatimgGraphs.Color);
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
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
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
                    ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraphs, false, false, wcUpdatimgGraphs.Color);
                };

                bgwGraphUpdater.DoWork += bgwGraphUpdaterDoWorkHandler;
                bgwGraphUpdater.RunWorkerCompleted += bgwGraphUpdaterCompletedHandler;
                bgwGraphUpdater.RunWorkerAsync();
                //}
            }
        }




        #region SeaBird SeaSave processor


        private void button1_Click(object sender, EventArgs e)
        {
            if (!bgwSeaSaveSocketStreamReader.IsBusy)
            {
                //scktSeaSave = ConnectSocket("169.254.249.87", 49161);
                scktSeaSave = ConnectSocket("192.168.192.218", 4001);
                if (scktSeaSave == null) return;
                bgwSeaSaveSocketStreamReader.RunWorkerAsync(new object[] { scktSeaSave });
                ThreadSafeOperations.ToggleButtonState(btnStartStopSeaSave, true, "STOP", true);
            }
            else
            {
                bgwSeaSaveSocketStreamReader.CancelAsync();
                ThreadSafeOperations.ToggleButtonState(btnStartStopSeaSave, true, "CONNECT SeaSave", true);
            }
        }




        private void bgwSeaSaveSocketStreamReader_DoWork(object sender, DoWorkEventArgs e)
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

                    ThreadSafeOperations.SetTextTB(tbSeaSaveLog, gotString + Environment.NewLine, true);

                    //string[] gotStrings = gotString.Split('%');
                    //foreach (string s in gotStrings)
                    //{
                    //    quStreamTextStrings.Enqueue(s);
                    //}
                }
            }
            while (bytes > 0);
        }







        #endregion









    }
}
