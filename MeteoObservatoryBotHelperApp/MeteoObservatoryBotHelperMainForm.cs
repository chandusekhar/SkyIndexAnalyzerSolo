using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using nsoftware.IPWorksIPC;
using nsoftware.IPWorksSSH;
using nsoftware.IPWorksZip;
using SkyImagesAnalyzer;
using SkyImagesAnalyzerLibraries;
using SolarPositioning;
using Timer = System.Threading.Timer;

namespace MeteoObservatoryBotHelperApp
{
    public partial class MeteoObservatoryBotHelperMainForm : Form
    {
        private LogWindow theLogWindow = null;

        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";

        #region periodical images processing

        ConcurrentExclusiveSchedulerPair scheduler;
        TaskFactory lowPriorityTaskFactory;

        #endregion periodical images processing


        #region CC_Moscow_bot connected properties


        private bool bSendProcessedDataTo_CC_Moscow_bot_server = false;
        private string strRemoteBotServerHost = "krinitsky.ru";
        private int intRemoteBotServerPort = 22;
        private string strRemoteBotServerHostAuthKeyFile = "";
        private string strRemoteBotServerSSHusername = "mk";
        private string strAcceptedSSHhostCertPublicKeyFile = "";
        private string strRemoteServerDataUploadingBaseDirectory = "~/cc_moscow_bot/data/";

        #endregion CC_Moscow_bot connected properties



        #region observational app output paths

        private string outputSnapshotsDirectory = "";
        private string ConcurrentDataXMLfilesBasePath = "";
        private string YRGBstatsXMLdataFilesDirectory = "";
        private string R2SLufftUMBappPath = "";
        private string VentusLufftUMBappPath = "";
        private string WSLufftUMBappPath = "";

        #endregion observational app output paths

        private string ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar +
                                          "ImagesRoundMasksXMLfilesMappingList.csv";

        private string strPerformanceCountersStatsFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                                         "logs" + Path.DirectorySeparatorChar +
                                                         Path.GetFileNameWithoutExtension(
                                                             Assembly.GetEntryAssembly().Location) + "-perfData.csv";

        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";

        private bool restrictSnapshotsProcessingWhenSunElevationLowerThanZero = true;
        private TimeSpan CalculateAndReportSDCandTCCPeriod = new TimeSpan(0, 5, 0);
        private Stopwatch stwCalculateAndReportSDCandTCCtimer = new Stopwatch();
        private Timer tmrCalculateAndReportSDCandCC = null;

        private TimeSpan makeCurrentSnapshotPreviewPicturePeriod = new TimeSpan(0, 1, 0);
        private Stopwatch stwMakeCurrentSnapshotPreviewPictureTimer = new Stopwatch();
        private Timer tmrMakeCurrentSnapshotPreviewPicture = null;

        private TimeSpan makeWSdataFilePeriod = new TimeSpan(0, 1, 0);
        private Stopwatch stwMakeWSdataFileTimer = new Stopwatch();
        private Timer tmrMakeWSdataFile = null;

        private Timer tmrTimersLabelsUpdate = null;

        private Timer tmrCheckPipeConnection = null;

        // private BackgroundWorker bgwPipeserverListening = null;
        private Pipeclient ipcPipeClient = null;
        private GPSdata latestGPSdata = null;

        private bool bAutoMode = false;







        public MeteoObservatoryBotHelperMainForm()
        {
            InitializeComponent();

            readDefaultProperties();

            theLogWindow = ServiceTools.LogAText(theLogWindow, CommonTools.DictionaryRepresentation(defaultProperties));
            theLogWindow.title = "Meteo bot computational back-side app log";

            scheduler = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, 4);
            lowPriorityTaskFactory = new TaskFactory(scheduler.ConcurrentScheduler);
        }








        private async void UpdateTimersLabels(object state)
        {
            TimeSpan nextCalculateAndReportSDCandTCCIn = CalculateAndReportSDCandTCCPeriod - stwCalculateAndReportSDCandTCCtimer.Elapsed;
            ThreadSafeOperations.SetText(lblTimeTillNextSDCandTCCestimation, nextCalculateAndReportSDCandTCCIn.RoundToSeconds().ToString("c"), false);

            TimeSpan nextMakeCurrentSnapshotPreviewPictureIn = makeCurrentSnapshotPreviewPicturePeriod - stwMakeCurrentSnapshotPreviewPictureTimer.Elapsed;
            ThreadSafeOperations.SetText(lblTimeTillNextPreviewCreation, nextMakeCurrentSnapshotPreviewPictureIn.RoundToSeconds().ToString("c"), false);

            TimeSpan nextMakeWSdataFileIn = makeWSdataFilePeriod - stwMakeWSdataFileTimer.Elapsed;
            ThreadSafeOperations.SetText(lblTimeTillNextMeteoInfoCreation, nextMakeWSdataFileIn.RoundToSeconds().ToString("c"), false);
        }









        #region ipcPipeClient

        private void ConnectPipeServer()
        {
            //Create the PipeClient and bind the events.
            ipcPipeClient = new Pipeclient();
            ipcPipeClient.OnDataIn += IpcPipeClient_OnDataIn;
            ipcPipeClient.OnConnected += IpcPipeClient_OnConnected;
            ipcPipeClient.OnDisconnected += IpcPipeClient_OnDisconnected;

            //Set the PipeName, EOL, and call Connect.
            ipcPipeClient.PipeName = "DataCollectorAutomator2G_IPCpipeserver";
            ipcPipeClient.EOL = Environment.NewLine;
            try
            {
                ipcPipeClient.Connect();
                ThreadSafeOperations.SetText(lblPipeServerConnectionStatus, "ON", false);
            }
            catch (Exception ex)
            {
                // theLogWindow = ServiceTools.LogAText(theLogWindow, ex.Message);
                ThreadSafeOperations.SetText(lblPipeServerConnectionStatus, "can`t connect", false);
                ipcPipeClient = null;
            }
        }

        private void IpcPipeClient_OnDisconnected(object sender, PipeclientDisconnectedEventArgs e)
        {
            // theLogWindow = ServiceTools.LogAText(theLogWindow, "Disonnected of pipeserver");
            ThreadSafeOperations.SetText(lblPipeServerConnectionStatus, "OFF", false);
        }

        private void IpcPipeClient_OnConnected(object sender, PipeclientConnectedEventArgs e)
        {
            theLogWindow = ServiceTools.LogAText(theLogWindow, "Connected to pipeserver");
        }



        private bool receivingIPCgpsData = false;
        StringBuilder IPCpipeClientReceivingStringBuilder; // = new StringBuilder();
        TextWriter IPCpipeClientReceivingStringWriter; // = new StringWriter(IPCpipeClientReceivingStringBuilder))
        private void IpcPipeClient_OnDataIn(object sender, PipeclientDataInEventArgs e)
        {
            // theLogWindow = ServiceTools.LogAText(theLogWindow, e.Text);
            if (receivingIPCgpsData)
            {
                if (e.Text == "</gps>")
                {
                    receivingIPCgpsData = false;

                    try
                    {
                        latestGPSdata =
                            ServiceTools.XmlDeserializeFromString<GPSdata>(
                                IPCpipeClientReceivingStringBuilder.ToString());
                        ThreadSafeOperations.SetText(lblGPSdata,
                            latestGPSdata.dateTimeUTC.ToString("u") + " " + latestGPSdata.HRString(), false);
                    }
                    catch (Exception ex)
                    {
                        if (theLogWindow != null)
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow, ex.Message);
                        }

                        ServiceTools.logToTextFile(errorLogFilename,
                            "ERROR parsing GPS data XML packet: " + Environment.NewLine +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(ex) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription() + Environment.NewLine + "XML packet string:" +
                            Environment.NewLine + IPCpipeClientReceivingStringBuilder.ToString(), true, true);
                    }
                }
                else
                {
                    string recString = e.Text;
                    IPCpipeClientReceivingStringWriter.Write(recString);
                }
            }
            else
            {
                if (e.Text == "<gps>")
                {
                    receivingIPCgpsData = true;
                    IPCpipeClientReceivingStringBuilder = new StringBuilder();
                    IPCpipeClientReceivingStringWriter = new StringWriter(IPCpipeClientReceivingStringBuilder);
                }
            }
        }



        #endregion ipcPipeClient






        #region periodical snapshot processing for SDC and TCC prediction



        private void btnCCandTCC_Click(object sender, EventArgs e)
        {
            if (theLogWindow == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "");
            }
            // Task.Run(() => ComputeAndReportCCandTCC());
            lowPriorityTaskFactory.StartNew(ComputeAndReportCCandTCC);
        }




        private void ProcessCurrentSnapshotForSDCandTCC(object state)
        {


            if (latestGPSdata == null && restrictSnapshotsProcessingWhenSunElevationLowerThanZero)
            {
                return;
            }

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
                lowPriorityTaskFactory.StartNew(ComputeAndReportCCandTCC);
            }
        }



        private async void ComputeAndReportCCandTCC()
        {
            // ProcessCurrentSnapshotWithServerCalculations(null);

            //BackgroundWorker bgwComputeAndReportCCandTCC = new BackgroundWorker();
            //bgwComputeAndReportCCandTCC.WorkerSupportsCancellation = false;
            //bgwComputeAndReportCCandTCC.WorkerReportsProgress = false;
            //bgwComputeAndReportCCandTCC.DoWork += 

            List<FileDatetimeInfo> lImagesFilesInfo = Directory.GetFiles(outputSnapshotsDirectory, "*.jpg",
                SearchOption.AllDirectories)
                .ToList()
                .ConvertAll(
                    strImageFileName =>
                        new FileDatetimeInfo(strImageFileName, ServiceTools.DatetimeExtractionMethod.Filename, "????xxxxxxxxxxxxxxxxxxx*"));

            if (lImagesFilesInfo.Count == 0)
            {
                return;
            }

            string lastSnapshotFile =
                lImagesFilesInfo.Aggregate(
                    (fInfo1, fInfo2) => (fInfo1.datetime <= fInfo2.datetime) ? (fInfo2) : (fInfo1)).filename;
            FileInfo currImageFInfo = new FileInfo(lastSnapshotFile);

            string concurrentDataXMLfilename = "";
            try
            {
                concurrentDataXMLfilename =
                    await Task.Run(() => CommonTools.FindConcurrentDataXMLfileAsync(lastSnapshotFile, ConcurrentDataXMLfilesBasePath, true,
                        ServiceTools.DatetimeExtractionMethod.Filename));
            }
            catch (Exception)
            {
                // не нашли нужный файл. ну или еще что-то случилось.
                return;
            }

            if (concurrentDataXMLfilename == "")
            {
                // не нашли нужный файл. ну или еще что-то случилось.
                return;
            }

            Dictionary<string, object> currDict = ServiceTools.ReadDictionaryFromXML(concurrentDataXMLfilename);
            currDict.Add("XMLfileName", Path.GetFileName(concurrentDataXMLfilename));
            ConcurrentData nearestConcurrentData = null;
            try
            {
                nearestConcurrentData = new ConcurrentData(currDict);
            }
            catch (Exception)
            {
                return;
            }






            #region calculate stats if needed

            SkyImageIndexesStatsData currImageStatsData = null;



            string strImageGrIxYRGBDataFileName =
                ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currImageFInfo.FullName,
                    YRGBstatsXMLdataFilesDirectory);


            if (File.Exists(strImageGrIxYRGBDataFileName))
            {
                try
                {
                    currImageStatsData =
                        (SkyImageIndexesStatsData)ServiceTools.ReadObjectFromXML(strImageGrIxYRGBDataFileName,
                            typeof(SkyImageIndexesStatsData));
                }
                catch (Exception ex)
                {
                    currImageStatsData = null;
                }

            }


            if (currImageStatsData == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "obtaining stats data for image " + currImageFInfo.FullName);

                Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
                optionalParameters.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                optionalParameters.Add("Stopwatch", sw);
                optionalParameters.Add("logFileName", errorLogFilename);

                ImageStatsDataCalculationResult currImageProcessingResult =
                    ImageProcessing.CalculateImageStatsData(currImageFInfo.FullName, optionalParameters);

                currImageProcessingResult.stopwatch.Stop();

                if (currImageProcessingResult.calcResult)
                {
                    string currentFullFileName = currImageProcessingResult.imgFilename;
                    string strPerfCountersData = currentFullFileName + ";" +
                                                 currImageProcessingResult.stopwatch.ElapsedMilliseconds + ";" +
                                                 (currImageProcessingResult.procTotalProcessorTimeEnd -
                                                  currImageProcessingResult.procTotalProcessorTimeStart)
                                                     .TotalMilliseconds +
                                                 Environment.NewLine;
                    ServiceTools.logToTextFile(strPerformanceCountersStatsFile, strPerfCountersData, true);




                    //string strImageGrIxYRGBDataFileName =
                    //    ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currentFullFileName,
                    //        imageYRGBstatsXMLdataFilesDirectory);
                    ServiceTools.WriteObjectToXML(currImageProcessingResult.grixyrgbStatsData,
                        strImageGrIxYRGBDataFileName);

                    currImageStatsData = currImageProcessingResult.grixyrgbStatsData;

                    theLogWindow = ServiceTools.LogAText(theLogWindow, "finished processing file " + Environment.NewLine + currentFullFileName);

                }
                else
                {
                    #region report error

                    string errorStr = "Error processing file: " + Environment.NewLine + currImageProcessingResult.imgFilename +
                        Environment.NewLine + "message: " +
                        ServiceTools.GetExceptionMessages(currImageProcessingResult.exception) + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription() + Environment.NewLine + "Stack trace: " + Environment.NewLine +
                        Environment.StackTrace + Environment.NewLine + Environment.NewLine;

                    ServiceTools.logToTextFile(errorLogFilename, errorStr, true, true);
                    theLogWindow = ServiceTools.LogAText(theLogWindow, errorStr);

                    return;

                    #endregion report error
                }

            }

            #endregion calculate stats if needed


            SunDiskCondition sdc;
            int TCC;
            PredictAndReportSDCandCC(currImageStatsData, nearestConcurrentData, out sdc, out TCC);


            #region store collected data to HDD

            FileDatetimeInfo inf = new FileDatetimeInfo(lastSnapshotFile,
                ServiceTools.DatetimeExtractionMethod.FileCreation);

            SkyImagesProcessedAndPredictedData data = new SkyImagesProcessedAndPredictedData()
            {
                skyImageFullFileName = inf.filename,
                skyImageFileName = Path.GetFileName(inf.filename),
                imageShootingDateTimeUTC = inf.datetime,
                PredictedCC = new PredictedCloudCoverData()
                {
                    dateTimeUTC = inf.datetime,
                    CloudCoverTotal = TCC,
                    CloudCoverLower = 0
                },
                concurrentDataXMLfile = concurrentDataXMLfilename,
                concurrentData = nearestConcurrentData,
                grixyrgbStatsXMLfile = strImageGrIxYRGBDataFileName,
                grixyrgbStats = currImageStatsData,
                PredictedSDC = sdc
            };
            string processedAndPredictedDataFileName =
                ConventionalTransitions.ImageProcessedAndPredictedDataFileName(lastSnapshotFile,
                    ConcurrentDataXMLfilesBasePath);
            ServiceTools.WriteObjectToXML(data, processedAndPredictedDataFileName);

            #endregion store collected data to HDD




            #region zip, send and unpack it

            Zip zip = new Zip();
            string tempZipFilename = Path.GetTempPath();
            tempZipFilename += (tempZipFilename.Last() == Path.DirectorySeparatorChar)
                ? ("")
                : (Path.DirectorySeparatorChar.ToString());
            tempZipFilename += Path.GetFileNameWithoutExtension(processedAndPredictedDataFileName) + ".zip";
            zip.ArchiveFile = tempZipFilename;
            zip.IncludeFiles(processedAndPredictedDataFileName);
            zip.Compress();

            //theLogWindow = ServiceTools.LogAText(theLogWindow,
            //    "zip file created: " + Environment.NewLine + tempZipFilename);

            Exception retEx = null;
            bool sendingResult = SendFileToBotServer(tempZipFilename,
                strRemoteServerDataUploadingBaseDirectory + Path.GetFileName(tempZipFilename), out retEx);

            File.Delete(tempZipFilename);


            #region report error
            if (!sendingResult)
            {
                if (theLogWindow != null)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
                        Environment.NewLine + "Exception messages: " + Environment.NewLine +
                        ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                        ServiceTools.CurrentCodeLineDescription());
                }

                ServiceTools.logToTextFile(errorLogFilename,
                    "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
                    Environment.NewLine + "Exception messages: " + Environment.NewLine +
                    ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                    ServiceTools.CurrentCodeLineDescription());


                return;
            }
            #endregion report error

            List<string> commands = new List<string>();
            commands.Add("cd " + strRemoteServerDataUploadingBaseDirectory);
            commands.Add("rm ./" + ConventionalTransitions.ImageProcessedAndPredictedDataFileNamesPattern());
            commands.Add("unzip " + Path.GetFileName(tempZipFilename));
            commands.Add("rm " + Path.GetFileName(tempZipFilename));
            commands.Add("ll");
            bool execResult = ExecSShellCommandsOnBotServer(commands, out retEx);

            #region report error

            if (!execResult)
            {
                if (theLogWindow != null)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "ERROR executing commands on bot server" + Environment.NewLine +
                        Environment.NewLine + "Exception messages: " + Environment.NewLine +
                        ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                        ServiceTools.CurrentCodeLineDescription());
                }
                else
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "ERROR executing commands on bot server" + Environment.NewLine +
                        Environment.NewLine + "Exception messages: " + Environment.NewLine +
                        ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                        ServiceTools.CurrentCodeLineDescription());
                }
                return;
            }

            #endregion

            #endregion zip, send and unpack it
        }




        private void PredictAndReportSDCandCC(SkyImageIndexesStatsData statsData, ConcurrentData nearestConcurrentData, out SunDiskCondition sdc, out int TCC)
        {
            stwCalculateAndReportSDCandTCCtimer.Restart();

            sdc = SunDiskCondition.Undefined;
            TCC = 0;

            if (bSendProcessedDataTo_CC_Moscow_bot_server)
            {
                string CurDir = Directory.GetCurrentDirectory();
                string SDC_NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar + "NNconfig.csv";
                string SDC_NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" +
                                                     Path.DirectorySeparatorChar + "NNtrainedParameters.csv";
                string NormMeansFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                       "NormMeans.csv";
                string NormRangeFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                       "NormRange.csv";
                List<double> decisionProbabilities = new List<double>();

                //SunDiskCondition sdc = SDCpredictorNN.CalcSDC_NN(dctCommunicationChecklist.strReturnedStatsDataXMLfilename,
                //    dctCommunicationChecklist.strConcurrentDataXMLfilename, SDC_NNconfigFile, SDC_NNtrainedParametersFile,
                //    NormMeansFile, NormRangeFile, out decisionProbabilities);
                DenseVector dvMeans =
                    (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormMeansFile, 0, ",")).Row(0);
                DenseVector dvRanges =
                    (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormRangeFile, 0, ",")).Row(0);
                DenseVector dvThetaValues =
                    (DenseVector)ServiceTools.ReadDataFromCSV(SDC_NNtrainedParametersFile, 0, ",");
                List<int> NNlayersConfig =
                    new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(SDC_NNconfigFile, 0, ",")).Row(0))
                        .ConvertAll
                        (dVal => Convert.ToInt32(dVal));

                sdc = SDCpredictorNN.PredictSDC_NN(statsData, nearestConcurrentData, NNlayersConfig, dvThetaValues,
                    dvMeans,
                    dvRanges, out decisionProbabilities);



                string CC_NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                         "CC_NNconfig.csv";
                string CC_NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" +
                                                    Path.DirectorySeparatorChar + "CC_NNtrainedParameters.csv";
                DenseVector dv_CC_ThetaValues =
                    (DenseVector)ServiceTools.ReadDataFromCSV(CC_NNtrainedParametersFile, 0, ",");
                List<int> CC_NNlayersConfig =
                    new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(CC_NNconfigFile, 0, ",")).Row(0))
                        .ConvertAll
                        (dVal => Convert.ToInt32(dVal));

                TCC = CCpredictorNN.PredictCC_NN(statsData, nearestConcurrentData, NNlayersConfig, dvThetaValues,
                    dvMeans,
                    dvRanges, CC_NNlayersConfig, dv_CC_ThetaValues);

                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Detected Sun disk condition: " + sdc + Environment.NewLine + "Detected CC: " + TCC);

                //ThreadSafeOperations.SetText(lblSDCvalue, sdc.ToString(), false);
                //ThreadSafeOperations.SetText(lblCCvalue, TCC.ToString() + " (/8)", false);
            }
        }



        #endregion periodical snapshot processing for SDC and TCC prediction







        #region periodical Make and send weather info

        private void btnForceMakeWeatherInfo_Click(object sender, EventArgs e)
        {
            if (theLogWindow == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "");
            }
            lowPriorityTaskFactory.StartNew(MakeWeatherInfo);
        }




        private void MakeWeatherInfoCallback(object state)
        {
            lowPriorityTaskFactory.StartNew(MakeWeatherInfo);
        }




        private async void MakeWeatherInfo()
        {
            stwMakeWSdataFileTimer.Restart();

            if (bSendProcessedDataTo_CC_Moscow_bot_server)
            {
                List<string> filesToSend = new List<string>();

                // WSLufftUMBappPath
                // Date time ; Temperature [°C] ; Abs. air pressure [hPa] ; Relative humidity [%] ; Abs. humidity [g/m³]
                if (Directory.Exists(WSLufftUMBappPath))
                {
                    List<FileInfo> lTXTdataFilesInfoList =
                        ((new DirectoryInfo(WSLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
                            SearchOption.AllDirectories)).ToList();
                    lTXTdataFilesInfoList.Sort(
                        (finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
                    FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
                    List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true,
                        ";");
                    List<string> lastWSdataStrings = Contents.Last();

                    LufftWSdata currWSdata = null;
                    try
                    {
                        currWSdata = new LufftWSdata(string.Join(";", lastWSdataStrings));
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                    if (currWSdata != null)
                    {
                        string tmpWSfile = Path.GetTempPath();
                        tmpWSfile += (tmpWSfile.Last() == Path.DirectorySeparatorChar)
                            ? ("")
                            : (Path.DirectorySeparatorChar.ToString());
                        tmpWSfile += ConventionalTransitions.WSUMBdataFileName(currWSdata.dateTimeUTC, "", false);
                        ServiceTools.WriteObjectToXML(currWSdata, tmpWSfile);
                        filesToSend.Add(tmpWSfile);
                    }
                }


                // R2SLufftUMBappPath
                if (Directory.Exists(R2SLufftUMBappPath))
                {
                    List<FileInfo> lTXTdataFilesInfoList =
                        ((new DirectoryInfo(R2SLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
                            SearchOption.AllDirectories)).ToList();
                    lTXTdataFilesInfoList.Sort(
                        (finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
                    FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
                    List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true,
                        ";");
                    List<string> lastR2SdataStrings = Contents.Last();


                    LufftR2Sdata currR2Sdata = null;
                    try
                    {
                        currR2Sdata = new LufftR2Sdata(string.Join(";", lastR2SdataStrings));
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                    if (currR2Sdata != null)
                    {
                        string tmpR2Sfile = Path.GetTempPath();
                        tmpR2Sfile += (tmpR2Sfile.Last() == Path.DirectorySeparatorChar)
                            ? ("")
                            : (Path.DirectorySeparatorChar.ToString());
                        tmpR2Sfile += ConventionalTransitions.R2SUMBdataFileName(currR2Sdata.dateTimeUTC, "", false);
                        ServiceTools.WriteObjectToXML(currR2Sdata, tmpR2Sfile);
                        filesToSend.Add(tmpR2Sfile);
                    }
                }



                // VentusLufftUMBappPath
                // Date time ; Virtual temperature [°C] ; Wind speed [m/s] ; Wind speed [m/s] Vect. ; Wind direction [°] ; Wind direction [°] Vect. ; Abs. air pressure [hPa] ; Wind value quality [%]
                if (Directory.Exists(VentusLufftUMBappPath))
                {
                    List<FileInfo> lTXTdataFilesInfoList =
                        ((new DirectoryInfo(VentusLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
                            SearchOption.AllDirectories)).ToList();
                    lTXTdataFilesInfoList.Sort(
                        (finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
                    FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
                    List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true,
                        ";");
                    List<string> lastVentusdataStrings = Contents.Last();

                    LufftVentusdata currVentusdata = null;
                    try
                    {
                        currVentusdata = new LufftVentusdata(string.Join(";", lastVentusdataStrings));
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                    if (currVentusdata != null)
                    {
                        string tmpVentusfile = Path.GetTempPath();
                        tmpVentusfile += (tmpVentusfile.Last() == Path.DirectorySeparatorChar)
                            ? ("")
                            : (Path.DirectorySeparatorChar.ToString());
                        tmpVentusfile += ConventionalTransitions.VentusUMBdataFileName(currVentusdata.dateTimeUTC, "",
                            false);
                        ServiceTools.WriteObjectToXML(currVentusdata, tmpVentusfile);
                        filesToSend.Add(tmpVentusfile);
                    }
                }






                #region zip, send and unpack it

                Zip zip = new Zip();
                string tempZipFilename = Path.GetTempPath();
                tempZipFilename += (tempZipFilename.Last() == Path.DirectorySeparatorChar)
                    ? ("")
                    : (Path.DirectorySeparatorChar.ToString());
                tempZipFilename += "WSdata-" + DateTime.UtcNow.ToString("s").Replace(":", "-") + ".zip";
                zip.ArchiveFile = tempZipFilename;
                zip.IncludeFiles(string.Join("|", filesToSend));
                zip.Compress();

                //theLogWindow = ServiceTools.LogAText(theLogWindow,
                //    "zip file created: " + Environment.NewLine + tempZipFilename);

                Exception retEx = null;
                bool sendingResult = SendFileToBotServer(tempZipFilename,
                    strRemoteServerDataUploadingBaseDirectory + Path.GetFileName(tempZipFilename), out retEx);

                File.Delete(tempZipFilename);


                #region report error

                if (!sendingResult)
                {
                    if (theLogWindow != null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }

                    ServiceTools.logToTextFile(errorLogFilename,
                        "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
                        Environment.NewLine + "Exception messages: " + Environment.NewLine +
                        ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                        ServiceTools.CurrentCodeLineDescription());


                    return;
                }

                #endregion report error

                List<string> commands = new List<string>();
                commands.Add("cd " + strRemoteServerDataUploadingBaseDirectory);
                commands.Add("rm ./" + ConventionalTransitions.WSUMBdataFileNamePattern());
                commands.Add("rm ./" + ConventionalTransitions.VentusUMBdataFileNamePattern());
                commands.Add("rm ./" + ConventionalTransitions.R2SUMBdataFileNamePattern());
                commands.Add("unzip " + Path.GetFileName(tempZipFilename));
                commands.Add("rm " + Path.GetFileName(tempZipFilename));
                commands.Add("ll");
                bool execResult = ExecSShellCommandsOnBotServer(commands, out retEx);

                #region report error

                if (!execResult)
                {
                    if (theLogWindow != null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "ERROR executing commands on bot server" + Environment.NewLine +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }
                    else
                    {
                        ServiceTools.logToTextFile(errorLogFilename,
                            "ERROR executing commands on bot server" + Environment.NewLine +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }
                    return;
                }

                #endregion

                #endregion zip, send and unpack it
            }
        }



        #endregion periodical Make and send weather info








        #region periodical snapshot processing for preview snapshots creation



        private void btnSSHsendPreview_Click(object sender, EventArgs e)
        {
            lowPriorityTaskFactory.StartNew(MakeAndSendSnapshotsPreviewPictures);
        }



        private void MakeCurrentSnapshotPreviewPicture(object state)
        {
            if ((latestGPSdata == null) && restrictSnapshotsProcessingWhenSunElevationLowerThanZero)
            {
                return;
            }

            // stwMakeCurrentSnapshotPreviewPictureTimer.Restart();

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
                lowPriorityTaskFactory.StartNew(MakeAndSendSnapshotsPreviewPictures);
            }
        }




        private async void MakeAndSendSnapshotsPreviewPictures()
        {
            stwMakeCurrentSnapshotPreviewPictureTimer.Restart();


            if (bSendProcessedDataTo_CC_Moscow_bot_server)
            {
                string FilenameToSend = "";
                Image<Bgr, byte> lastImagesCouple = CurrentImagesCouple(out FilenameToSend);
                string tmpFNameToSave = Path.GetTempPath();
                tmpFNameToSave += (tmpFNameToSave.Last() == Path.DirectorySeparatorChar)
                    ? ("")
                    : (Path.DirectorySeparatorChar.ToString());
                tmpFNameToSave += FilenameToSend;
                lastImagesCouple.Save(tmpFNameToSave);
                // var fileStream = File.Open(tmpFNameToSave, FileMode.Open);
                // Message sentMessage = await Bot.SendPhoto(update.Message.Chat.Id, new FileToSend(FilenameToSend, fileStream));
                // послать файл на сервер бота


                string filenameToSend = tmpFNameToSave;
                string concurrentDataXMLfilename =
                    await
                        CommonTools.FindConcurrentDataXMLfileAsync(filenameToSend, ConcurrentDataXMLfilesBasePath,
                            true,
                            ServiceTools.DatetimeExtractionMethod.Filename);

                Zip zip = new Zip();
                string tempZipFilename = Path.GetTempPath();
                tempZipFilename += (tempZipFilename.Last() == Path.DirectorySeparatorChar)
                    ? ("")
                    : (Path.DirectorySeparatorChar.ToString());
                tempZipFilename += Path.GetFileNameWithoutExtension(filenameToSend) + ".zip";
                zip.ArchiveFile = tempZipFilename;
                zip.IncludeFiles(filenameToSend + " | " + concurrentDataXMLfilename);
                zip.Compress();

                //theLogWindow = ServiceTools.LogAText(theLogWindow,
                //    "zip file created: " + Environment.NewLine + tempZipFilename);

                Exception retEx = null;
                bool sendingResult = SendFileToBotServer(tempZipFilename,
                    strRemoteServerDataUploadingBaseDirectory + Path.GetFileName(tempZipFilename), out retEx);

                File.Delete(tmpFNameToSave);
                File.Delete(tempZipFilename);

                #region report error
                if (!sendingResult)
                {
                    if (theLogWindow != null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }

                    ServiceTools.logToTextFile(errorLogFilename,
                        "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
                        Environment.NewLine + "Exception messages: " + Environment.NewLine +
                        ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                        ServiceTools.CurrentCodeLineDescription());


                    return;
                }
                #endregion report error

                List<string> commands = new List<string>();
                commands.Add("cd " + strRemoteServerDataUploadingBaseDirectory);
                commands.Add("rm ./*.jpg");
                commands.Add("rm ./" + ConventionalTransitions.ImageConcurrentDataFilesNamesPattern());
                commands.Add("unzip " + Path.GetFileName(tempZipFilename));
                commands.Add("rm " + Path.GetFileName(tempZipFilename));
                commands.Add("ll");
                bool execResult = ExecSShellCommandsOnBotServer(commands, out retEx);

                #region report error

                if (!execResult)
                {
                    if (theLogWindow != null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "ERROR executing commands on bot server" + Environment.NewLine +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }
                    else
                    {
                        ServiceTools.logToTextFile(errorLogFilename,
                            "ERROR executing commands on bot server" + Environment.NewLine +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }
                    return;
                }

                #endregion

            }
        }





        private Image<Bgr, byte> CurrentImagesCouple(out string FilenameToSend)
        {
            DirectoryInfo dir = new DirectoryInfo(outputSnapshotsDirectory);
            List<FileInfo> lImagesFilesID1 = dir.GetFiles("*devID1.jpg", SearchOption.AllDirectories).ToList();
            lImagesFilesID1.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
            string strLastImageID1Fname = lImagesFilesID1.Last().FullName;

            FilenameToSend = strLastImageID1Fname.Replace("devID1", "");
            FilenameToSend = Path.GetFileName(FilenameToSend);

            List<FileInfo> lImagesFilesID2 = dir.GetFiles("*devID2.jpg", SearchOption.AllDirectories).ToList();
            lImagesFilesID2.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
            string strLastImageID2Fname = lImagesFilesID2.Last().FullName;

            Image<Bgr, byte> img1 = new Image<Bgr, byte>(strLastImageID1Fname);
            Image<Bgr, byte> img2 = new Image<Bgr, byte>(strLastImageID2Fname);

            Size img1Size = img1.Size;
            Size resimgSize = new Size(img1Size.Width * 2, img1Size.Height);

            Image<Bgr, byte> resImg = new Image<Bgr, byte>(resimgSize);
            Graphics g = Graphics.FromImage(resImg.Bitmap);
            g.DrawImage(img1.Bitmap, new Point(0, 0));
            g.DrawImage(img2.Bitmap, new Point(img1Size.Width, 0));

            resImg = resImg.Resize(0.25d, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            return resImg;
        }


        #endregion periodical snapshot processing for preview snapshots creation







        #region exec Sshell commands methods

        private bool ExecSShellCommandsOnBotServer(List<string> commands, out Exception retEx)
        {
            retEx = null;

            Sshell sh = new Sshell()
            {
                SSHHost = strRemoteBotServerHost,
                SSHPort = intRemoteBotServerPort,
                SSHAuthMode = SshellSSHAuthModes.amPublicKey,
                SSHUser = strRemoteBotServerSSHusername
            };

            try
            {
                sh.SSHCert = new Certificate(CertStoreTypes.cstPPKFile, strRemoteBotServerHostAuthKeyFile, "", "*");
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }

            try
            {
                sh.SSHAcceptServerHostKey = new Certificate(CertStoreTypes.cstSSHPublicKeyFile,
                    strAcceptedSSHhostCertPublicKeyFile, "", "*");
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }


            sh.OnSSHServerAuthentication += Sh_OnSSHServerAuthentication;
            sh.OnStdout += Sh_OnStdout;

            foreach (string command in commands)
            {
                sh.Execute(command);
            }

            return true;
        }



        private void Sh_OnStdout(object sender, SshellStdoutEventArgs e)
        {
            theLogWindow = ServiceTools.LogAText(theLogWindow, e.Text);
        }



        private void Sh_OnSSHServerAuthentication(object sender, SshellSSHServerAuthenticationEventArgs e)
        {
            Sshell sh = sender as Sshell;
            if (!e.Accept)
            {
                try
                {
                    sh.Interrupt();
                    sh.SSHLogoff();
                    sh.Dispose();
                    sh = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion exec Sshell commands methods




        #region scp file methods

        private bool SendFileToBotServer(string localFile, string remoteFile, out Exception retEx)
        {
            retEx = null;
            if (!File.Exists(strRemoteBotServerHostAuthKeyFile))
            {
                retEx =
                    new FileNotFoundException("unable to locate bot server auth key file: " +
                                              strRemoteBotServerHostAuthKeyFile);
                return false;
            }

            if (!File.Exists(strAcceptedSSHhostCertPublicKeyFile))
            {
                retEx =
                    new FileNotFoundException("unable to locate bot server accepted host key file: " +
                                              strAcceptedSSHhostCertPublicKeyFile);
                return false;
            }



            Scp scp = new Scp()
            {
                SSHHost = strRemoteBotServerHost,
                SSHPort = intRemoteBotServerPort,
                SSHAuthMode = ScpSSHAuthModes.amPublicKey,
                SSHUser = strRemoteBotServerSSHusername
            };


            try
            {
                scp.SSHCert = new Certificate(CertStoreTypes.cstPPKFile, strRemoteBotServerHostAuthKeyFile, "", "*");
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }



            try
            {
                scp.SSHAcceptServerHostKey = new Certificate(CertStoreTypes.cstSSHPublicKeyFile,
                    strAcceptedSSHhostCertPublicKeyFile, "", "*");
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }


            scp.OnSSHServerAuthentication += Scp_OnSSHServerAuthentication;
            scp.RemoteFile = remoteFile;
            scp.LocalFile = localFile;
            scp.OnEndTransfer += Scp_OnEndTransfer;

            try
            {
                scp.Upload();
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }

            return true;

        }


        private void Scp_OnEndTransfer(object sender, ScpEndTransferEventArgs e)
        {
            if (theLogWindow != null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "file " + e.LocalFile + " transfer finished");
            }
        }



        private void Scp_OnSSHServerAuthentication(object sender, ScpSSHServerAuthenticationEventArgs e)
        {
            Scp scpSender = sender as Scp;
            if (!e.Accept)
            {
                try
                {
                    scpSender.Interrupt();
                    scpSender.SSHLogoff();
                    scpSender.Dispose();
                    scpSender = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion scp file methods






        #region default settings

        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";
            if (!File.Exists(defaultPropertiesXMLfileName))
            {
                Console.WriteLine("couldn`t find settings XML file: " + Environment.NewLine +
                                  defaultPropertiesXMLfileName);
            }
            else
            {
                defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);
            }


            bool bDefaultPropertiesHasBeenUpdated = false;

            string CurDir = Directory.GetCurrentDirectory();



            #region outputSnapshotsDirectory

            if (defaultProperties.ContainsKey("outputSnapshotsDirectory"))
            {
                outputSnapshotsDirectory = (string)defaultProperties["outputSnapshotsDirectory"];
            }
            else
            {
                outputSnapshotsDirectory = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "snapshots" + Path.DirectorySeparatorChar;
                defaultProperties.Add("outputSnapshotsDirectory", outputSnapshotsDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion


            #region ConcurrentDataXMLfilesBasePath
            // ConcurrentDataXMLfilesBasePath
            if (defaultProperties.ContainsKey("ConcurrentDataXMLfilesBasePath"))
            {
                ConcurrentDataXMLfilesBasePath = (string)defaultProperties["ConcurrentDataXMLfilesBasePath"];
            }
            else
            {
                ConcurrentDataXMLfilesBasePath = "";
                defaultProperties.Add("ConcurrentDataXMLfilesBasePath", ConcurrentDataXMLfilesBasePath);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion



            #region YRGBstatsXMLdataFilesDirectory
            // YRGBstatsXMLdataFilesDirectory
            if (defaultProperties.ContainsKey("YRGBstatsXMLdataFilesDirectory"))
            {
                YRGBstatsXMLdataFilesDirectory = (string)defaultProperties["YRGBstatsXMLdataFilesDirectory"];
            }
            else
            {
                YRGBstatsXMLdataFilesDirectory = "";
                defaultProperties.Add("YRGBstatsXMLdataFilesDirectory", YRGBstatsXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion



            #region R2SLufftUMBappPath
            //// R2SLufftUMBappPath
            if (defaultProperties.ContainsKey("R2SLufftUMBappPath"))
            {
                R2SLufftUMBappPath = (string)defaultProperties["R2SLufftUMBappPath"];
            }
            else
            {
                R2SLufftUMBappPath = "";
                defaultProperties.Add("R2SLufftUMBappPath", R2SLufftUMBappPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion



            #region VentusLufftUMBappPath
            //// VentusLufftUMBappPath
            if (defaultProperties.ContainsKey("VentusLufftUMBappPath"))
            {
                VentusLufftUMBappPath = (string)defaultProperties["VentusLufftUMBappPath"];
            }
            else
            {
                VentusLufftUMBappPath = "";
                defaultProperties.Add("VentusLufftUMBappPath", VentusLufftUMBappPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion



            #region WSLufftUMBappPath
            //// WSLufftUMBappPath
            if (defaultProperties.ContainsKey("WSLufftUMBappPath"))
            {
                WSLufftUMBappPath = (string)defaultProperties["WSLufftUMBappPath"];
            }
            else
            {
                WSLufftUMBappPath = "";
                defaultProperties.Add("WSLufftUMBappPath", WSLufftUMBappPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region ImagesRoundMasksXMLfilesMappingList

            if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
            {
                ImagesRoundMasksXMLfilesMappingList = (string)defaultProperties["ImagesRoundMasksXMLfilesMappingList"];
            }
            else
            {
                defaultProperties.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion ImagesRoundMasksXMLfilesMappingList






            #region bot server communication properties

            #region bSendProcessedDataTo_CC_Moscow_bot_server
            //private bool bSendProcessedDataTo_CC_Moscow_bot_server = false;
            if (defaultProperties.ContainsKey("bSendProcessedDataTo_CC_Moscow_bot_server"))
            {
                bSendProcessedDataTo_CC_Moscow_bot_server =
                    ((string)defaultProperties["bSendProcessedDataTo_CC_Moscow_bot_server"]).ToLower() == "true";
            }
            else
            {
                bSendProcessedDataTo_CC_Moscow_bot_server = false;
                defaultProperties.Add("bSendProcessedDataTo_CC_Moscow_bot_server",
                    bSendProcessedDataTo_CC_Moscow_bot_server);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion


            #region strRemoteBotServerHost
            //private string strRemoteBotServerHost = "krinitsky.ru";
            if (defaultProperties.ContainsKey("strRemoteBotServerHost"))
            {
                strRemoteBotServerHost = (string)defaultProperties["strRemoteBotServerHost"];
            }
            else
            {
                strRemoteBotServerHost = "krinitsky.ru";
                defaultProperties.Add("strRemoteBotServerHost", strRemoteBotServerHost);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            #endregion


            #region intRemoteBotServerPort
            //private int intRemoteBotServerPort = 22;

            if (defaultProperties.ContainsKey("intRemoteBotServerPort"))
            {
                intRemoteBotServerPort = Convert.ToInt32((string)defaultProperties["intRemoteBotServerPort"]);
            }
            else
            {
                intRemoteBotServerPort = 22;
                defaultProperties.Add("intRemoteBotServerPort", intRemoteBotServerPort);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion


            #region strRemoteBotServerHostAuthKeyFile
            //private string strRemoteBotServerHostAuthKeyFile = "";
            if (defaultProperties.ContainsKey("strRemoteBotServerHostAuthKeyFile"))
            {
                strRemoteBotServerHostAuthKeyFile = (string)defaultProperties["strRemoteBotServerHostAuthKeyFile"];
            }
            else
            {
                strRemoteBotServerHostAuthKeyFile = "";
                defaultProperties.Add("strRemoteBotServerHostAuthKeyFile", "");
                bDefaultPropertiesHasBeenUpdated = true;
            }


            #endregion strRemoteBotServerHostAuthKeyFile


            #region strRemoteBotServerSSHusername

            if (defaultProperties.ContainsKey("strRemoteBotServerSSHusername"))
            {
                strRemoteBotServerSSHusername = (string)defaultProperties["strRemoteBotServerSSHusername"];
            }
            else
            {
                strRemoteBotServerSSHusername = "mk";
                defaultProperties.Add("strRemoteBotServerSSHusername", strRemoteBotServerSSHusername);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion strRemoteBotServerSSHusername



            #region strAcceptedSSHhostCertPublicKeyFile

            if (defaultProperties.ContainsKey("strAcceptedSSHhostCertPublicKeyFile"))
            {
                strAcceptedSSHhostCertPublicKeyFile = (string)defaultProperties["strAcceptedSSHhostCertPublicKeyFile"];
            }
            else
            {
                strAcceptedSSHhostCertPublicKeyFile = "";
                defaultProperties.Add("strAcceptedSSHhostCertPublicKeyFile", strAcceptedSSHhostCertPublicKeyFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region strRemoteServerDataUploadingBaseDirectory

            if (defaultProperties.ContainsKey("strRemoteServerDataUploadingBaseDirectory"))
            {
                strRemoteServerDataUploadingBaseDirectory =
                    (string) defaultProperties["strRemoteServerDataUploadingBaseDirectory"];
            }
            else
            {
                defaultProperties.Add("strRemoteServerDataUploadingBaseDirectory",
                    strRemoteServerDataUploadingBaseDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            #endregion



            #endregion bot server communication properties




            #region makeCurrentSnapshotPreviewPicturePeriod
            if (defaultProperties.ContainsKey("strmakeCurrentSnapshotPreviewPicturePeriodSeconds"))
            {
                string strmakeCurrentSnapshotPreviewPicturePeriodSeconds =
                    (string)(defaultProperties["strmakeCurrentSnapshotPreviewPicturePeriodSeconds"]);
                if (strmakeCurrentSnapshotPreviewPicturePeriodSeconds == "0")
                {
                    makeCurrentSnapshotPreviewPicturePeriod = TimeSpan.MaxValue;
                }
                else
                {
                    makeCurrentSnapshotPreviewPicturePeriod = new TimeSpan(0, 0, Convert.ToInt32(strmakeCurrentSnapshotPreviewPicturePeriodSeconds));
                }
            }
            else
            {
                int strmakeCurrentSnapshotPreviewPicturePeriodSeconds = 90;
                makeCurrentSnapshotPreviewPicturePeriod = new TimeSpan(0, 0, Convert.ToInt32(strmakeCurrentSnapshotPreviewPicturePeriodSeconds));
                defaultProperties.Add("strmakeCurrentSnapshotPreviewPicturePeriodSeconds", strmakeCurrentSnapshotPreviewPicturePeriodSeconds);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion makeCurrentSnapshotPreviewPicturePeriod



            #region CalculateAndReportSDCandTCCPeriod
            if (defaultProperties.ContainsKey("strCalculateAndReportSDCandTCCPeriodSeconds"))
            {
                string strCalculateAndReportSDCandTCCPeriodSeconds =
                    (string)(defaultProperties["strCalculateAndReportSDCandTCCPeriodSeconds"]);
                if (strCalculateAndReportSDCandTCCPeriodSeconds == "0")
                {
                    CalculateAndReportSDCandTCCPeriod = TimeSpan.MaxValue;
                }
                else
                {
                    CalculateAndReportSDCandTCCPeriod = new TimeSpan(0, 0, Convert.ToInt32(strCalculateAndReportSDCandTCCPeriodSeconds));
                }
            }
            else
            {
                int strCalculateAndReportSDCandTCCPeriodSeconds = 90;
                CalculateAndReportSDCandTCCPeriod = new TimeSpan(0, 0, Convert.ToInt32(strCalculateAndReportSDCandTCCPeriodSeconds));
                defaultProperties.Add("strCalculateAndReportSDCandTCCPeriodSeconds", strCalculateAndReportSDCandTCCPeriodSeconds);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion CalculateAndReportSDCandTCCPeriod




            #region makeWSdataFilePeriod
            if (defaultProperties.ContainsKey("strmakeWSdataFilePeriodSeconds"))
            {
                string strmakeWSdataFilePeriodSeconds =
                    (string)(defaultProperties["strmakeWSdataFilePeriodSeconds"]);
                if (strmakeWSdataFilePeriodSeconds == "0")
                {
                    makeWSdataFilePeriod = TimeSpan.MaxValue;
                }
                else
                {
                    makeWSdataFilePeriod = new TimeSpan(0, 0, Convert.ToInt32(strmakeWSdataFilePeriodSeconds));
                }
            }
            else
            {
                int strmakeWSdataFilePeriodSeconds = 90;
                makeWSdataFilePeriod = new TimeSpan(0, 0, Convert.ToInt32(strmakeWSdataFilePeriodSeconds));
                defaultProperties.Add("strmakeWSdataFilePeriodSeconds", strmakeWSdataFilePeriodSeconds);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion makeWSdataFilePeriod





            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }



        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }


        #endregion default settings













        private void MeteoObservatoryBotHelperMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tmrCalculateAndReportSDCandCC != null)
            {
                tmrCalculateAndReportSDCandCC.Change(Timeout.Infinite, Timeout.Infinite);
                tmrCalculateAndReportSDCandCC.Dispose();
                tmrCalculateAndReportSDCandCC = null;
            }

            if (tmrMakeCurrentSnapshotPreviewPicture != null)
            {
                tmrMakeCurrentSnapshotPreviewPicture.Change(Timeout.Infinite, Timeout.Infinite);
                tmrMakeCurrentSnapshotPreviewPicture.Dispose();
                tmrMakeCurrentSnapshotPreviewPicture = null;
            }

            if (tmrMakeWSdataFile != null)
            {
                tmrMakeWSdataFile.Change(Timeout.Infinite, Timeout.Infinite);
                tmrMakeWSdataFile.Dispose();
                tmrMakeWSdataFile = null;
            }
        }





        private void MeteoObservatoryBotHelperMainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
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



        private void btnSwitchAutoMode_Click(object sender, EventArgs e)
        {
            SwitchAutoMode();
        }



        private void SwitchAutoMode()
        {
            bAutoMode = !bAutoMode;
            if (bAutoMode)
            {
                #region timer for SDC and TCC calculation

                if (CalculateAndReportSDCandTCCPeriod < TimeSpan.MaxValue)
                {
                    stwCalculateAndReportSDCandTCCtimer.Start();
                    tmrCalculateAndReportSDCandCC = new System.Threading.Timer(ProcessCurrentSnapshotForSDCandTCC, null, 0,
                        (int)CalculateAndReportSDCandTCCPeriod.TotalMilliseconds);
                }

                #endregion

                #region timer for preview picture creation

                if (makeCurrentSnapshotPreviewPicturePeriod < TimeSpan.MaxValue)
                {
                    stwMakeCurrentSnapshotPreviewPictureTimer.Start();
                    tmrMakeCurrentSnapshotPreviewPicture = new Timer(MakeCurrentSnapshotPreviewPicture, null, 0,
                        (int)makeCurrentSnapshotPreviewPicturePeriod.TotalMilliseconds);
                }

                #endregion timer for preview picture creation



                //makeWSdataFilePeriod
                #region timer for making weather station data

                if (makeWSdataFilePeriod < TimeSpan.MaxValue)
                {
                    stwMakeWSdataFileTimer.Start();
                    tmrMakeWSdataFile = new System.Threading.Timer(MakeWeatherInfoCallback, null, 0,
                        (int)makeWSdataFilePeriod.TotalMilliseconds);
                }

                #endregion


                tmrTimersLabelsUpdate = new Timer(UpdateTimersLabels, null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 1));



                #region check if connected and connect if not

                TimerCallback CheckPipeConnectionCallback = new TimerCallback(state =>
                {
                    if (ipcPipeClient == null)
                    {
                        ConnectPipeServer();
                        return;
                    }
                    else if (!ipcPipeClient.Connected)
                    {
                        ConnectPipeServer();
                    }
                });
                tmrCheckPipeConnection = new System.Threading.Timer(CheckPipeConnectionCallback, null, new TimeSpan(0, 0, 1),
                    new TimeSpan(0, 0, 1));

                #endregion

                ThreadSafeOperations.ToggleButtonState(btnSwitchAutoMode, true, "AUTO mode: ON", true);

            }
            else
            {
                stwCalculateAndReportSDCandTCCtimer.Stop();
                if (tmrCalculateAndReportSDCandCC != null)
                {
                    tmrCalculateAndReportSDCandCC.Change(Timeout.Infinite, Timeout.Infinite);
                    tmrCalculateAndReportSDCandCC.Dispose();
                    tmrCalculateAndReportSDCandCC = null;
                }

                stwMakeCurrentSnapshotPreviewPictureTimer.Stop();

                if (tmrMakeCurrentSnapshotPreviewPicture != null)
                {
                    tmrMakeCurrentSnapshotPreviewPicture.Change(Timeout.Infinite, Timeout.Infinite);
                    tmrMakeCurrentSnapshotPreviewPicture.Dispose();
                    tmrMakeCurrentSnapshotPreviewPicture = null;
                }

                if (tmrTimersLabelsUpdate != null)
                {
                    tmrTimersLabelsUpdate.Change(Timeout.Infinite, Timeout.Infinite);
                    tmrTimersLabelsUpdate.Dispose();
                    tmrTimersLabelsUpdate = null;
                }


                stwMakeWSdataFileTimer.Stop();
                if (tmrMakeWSdataFile != null)
                {
                    tmrMakeWSdataFile.Change(Timeout.Infinite, Timeout.Infinite);
                    tmrMakeWSdataFile.Dispose();
                    tmrMakeWSdataFile = null;
                }


                ThreadSafeOperations.SetText(lblTimeTillNextPreviewCreation, "---", false);
                ThreadSafeOperations.SetText(lblTimeTillNextSDCandTCCestimation, "---", false);
                ThreadSafeOperations.SetText(lblTimeTillNextMeteoInfoCreation, "---", false);
                ThreadSafeOperations.ToggleButtonState(btnSwitchAutoMode, true, "AUTO mode: OFF", true);
            }
        }


    }
}
