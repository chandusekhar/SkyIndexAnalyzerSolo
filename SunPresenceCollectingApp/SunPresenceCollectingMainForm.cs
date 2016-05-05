using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Structure;
using SkyImagesAnalyzer;
using SkyImagesAnalyzerLibraries;
using Accord.Controls;
using Accord.IO;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Analysis;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using ANN;
using MathNet.Numerics.LinearAlgebra.Double;


namespace SunPresenceCollectingApp
{
    public partial class SunPresenceCollectingMainForm : Form
    {
        private LogWindow theLogWindow = null;
        private delegate void DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;
        private string currImageFileName = "";
        private FileInfo currImageFInfo = null;
        private string currPath2Process = "";
        private string SunDiskConditionXMLdataFilesDirectory = "";
        private string imageYRGBstatsXMLdataFilesDirectory = "";
        private string imageMP5statsXMLdataFilesDirectory = "";
        private Image<Bgr, byte> currImage = null;
        private AbortableBackgroundWorkerWithParameters bgwCurrImageProcessing = null;
        private SunDiskConditionData currImageSunDiskConditionData;
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private bool bShuffleImages = false;
        private bool bPermitRepeats = false;
        private int maxConcurrentBackgroundWorkers = 8;
        private List<string> alreadyMarkedSunDiskConditionFiles = new List<string>();
        private bool bReviewMode = false;
        private string SerializedDecisionTreePath = "";
        private bool bPermitProcessingStart = false;
        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";


        private ObservableCollection<WorkersWithWatchers> ocActiveStatsCalculationBgws = new ObservableCollection<WorkersWithWatchers>();

        private string ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar +
                                          "ImagesRoundMasksXMLfilesMappingList.csv";

        private string strPerformanceCountersStatsFile = "";
        private TimeSpan TimeSpanForSDCdataMappingTolerance;
        private TimeSpan TimeSpanForConcurrentDataMappingTolerance;
        private string ConcurrentDataXMLfilesDirectory = "";

        private List<ConcurrentData> lConcurrentDataAlreadyRead = null;

        #region ANN trained data filenames
        private string NNconfigFile = "";
        private string NNtrainedParametersFile = "";
        private string NormMeansFile = "";
        private string NormRangeFile = "";
        #endregion






        public SunPresenceCollectingMainForm()
        {
            InitializeComponent();
        }






        private void SunPresenceCollectingMainForm_Shown(object sender, EventArgs e)
        {
            m_DelegateOpenFile = this.OpenFile;
            readDefaultProperties();
            oqGrIxStatsCalculationQueue.CollectionChanged += OqGrIxStatsCalculationQueue_CollectionChanged;
            ocActiveStatsCalculationBgws.CollectionChanged += OcActiveStatsCalculationBgws_CollectionChanged;

            Application.EnableVisualStyles();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            listBinding = new BindingList<ImageStatsCollectingData>(lGrIxStatsCalculation);
            dataGridView1.DataSource = listBinding;

            rtbServiceSourceDirectory.Text = currPath2Process;
        }






        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";
            if (File.Exists(defaultPropertiesXMLfileName))
            {
                defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);
            }
            else
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Unable to load settings from file " + defaultPropertiesXMLfileName + Environment.NewLine +
                    "All settings will be defined as default values." + Environment.NewLine +
                    "Please pay your attention to it mentioned above.");
            }


            bool bDefaultPropertiesHasBeenUpdated = false;

            string CurDir = Directory.GetCurrentDirectory();
            currPath2Process = "";
            if (defaultProperties.ContainsKey("ImagesBasePath"))
            {
                currPath2Process = (string)defaultProperties["ImagesBasePath"];
            }
            else
            {
                currPath2Process = CurDir;
            }
            rtbServiceSourceDirectory.Text = currPath2Process;




            // SaveControlImagesToPath
            if (!defaultProperties.ContainsKey("SaveControlImagesToPath"))
            {
                defaultProperties.Add("SaveControlImagesToPath", CurDir);
                bDefaultPropertiesHasBeenUpdated = true;
            }




            if (defaultProperties.ContainsKey("SunDiskConditionXMLdataFilesDirectory"))
            {
                SunDiskConditionXMLdataFilesDirectory = (string)defaultProperties["SunDiskConditionXMLdataFilesDirectory"];
            }
            else
            {
                SunDiskConditionXMLdataFilesDirectory = CurDir;
                defaultProperties.Add("SunDiskConditionXMLdataFilesDirectory", SunDiskConditionXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }



            //imageYRGBstatsXMLdataFilesDirectory
            if (defaultProperties.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
            {
                imageYRGBstatsXMLdataFilesDirectory = (string)defaultProperties["imageYRGBstatsXMLdataFilesDirectory"];
            }
            else
            {
                imageYRGBstatsXMLdataFilesDirectory = CurDir;
                defaultProperties.Add("imageYRGBstatsXMLdataFilesDirectory", imageYRGBstatsXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }


            //imageMP5statsXMLdataFilesDirectory
            if (defaultProperties.ContainsKey("imageMP5statsXMLdataFilesDirectory"))
            {
                imageMP5statsXMLdataFilesDirectory = (string)defaultProperties["imageMP5statsXMLdataFilesDirectory"];
            }
            else
            {
                imageMP5statsXMLdataFilesDirectory = CurDir;
                defaultProperties.Add("imageMP5statsXMLdataFilesDirectory", imageMP5statsXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }


            if (defaultProperties.ContainsKey("CalculateGrIxStatsOnline"))
            {
                cbCalculateGrIxStatsOnline.Checked = (((string)defaultProperties["CalculateGrIxStatsOnline"]).ToLower() == "true");
            }
            else
            {
                cbCalculateGrIxStatsOnline.Checked = false;
                defaultProperties.Add("CalculateGrIxStatsOnline", false);
                bDefaultPropertiesHasBeenUpdated = true;
            }



            if (defaultProperties.ContainsKey("ConversionSourceDirectory"))
            {
                rtbSourceDirectory.Text = ((string)defaultProperties["ConversionSourceDirectory"]);
            }
            else
            {
                rtbSourceDirectory.Text = "";
                defaultProperties.Add("ConversionSourceDirectory", "");
                bDefaultPropertiesHasBeenUpdated = true;
            }




            if (defaultProperties.ContainsKey("ConversionOutputDirectory"))
            {
                rtbOutputDirectory.Text = ((string)defaultProperties["ConversionOutputDirectory"]);
            }
            else
            {
                rtbOutputDirectory.Text = "";
                defaultProperties.Add("ConversionOutputDirectory", "");
                bDefaultPropertiesHasBeenUpdated = true;
            }




            if (defaultProperties.ContainsKey("SDCdataCSVfile"))
            {
                rtbSDCdataCSVfile.Text = ((string)defaultProperties["SDCdataCSVfile"]);
            }
            else
            {
                rtbOutputDirectory.Text = "";
                defaultProperties.Add("SDCdataCSVfile", "");
                bDefaultPropertiesHasBeenUpdated = true;
            }


            //ConversionSearchInputsTopDirectoryOnly
            if (defaultProperties.ContainsKey("ConversionSearchInputsTopDirectoryOnly"))
            {
                cbConversionSearchInputsTopDirectoryOnly.Checked = (((string)defaultProperties["ConversionSearchInputsTopDirectoryOnly"]).ToLower() == "true");
            }
            else
            {
                cbConversionSearchInputsTopDirectoryOnly.Checked = false;
                defaultProperties.Add("ConversionSearchInputsTopDirectoryOnly", false);
                bDefaultPropertiesHasBeenUpdated = true;
            }




            if (defaultProperties.ContainsKey("ShuffleImages"))
            {
                bShuffleImages = (((string)defaultProperties["ShuffleImages"]).ToLower() == "true");
            }
            else
            {
                defaultProperties.Add("ShuffleImages", false);
                bDefaultPropertiesHasBeenUpdated = true;
            }



            // maxConcurrentBackgroundWorkers
            if (defaultProperties.ContainsKey("maxConcurrentBackgroundWorkers"))
            {
                maxConcurrentBackgroundWorkers = Convert.ToInt32(defaultProperties["maxConcurrentBackgroundWorkers"]);
            }
            else
            {
                defaultProperties.Add("maxConcurrentBackgroundWorkers", 8);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }





            // MLdataSourceDirectory
            if (defaultProperties.ContainsKey("MLdataSourceDirectory"))
            {
                rtbMLdataSourceDirectory.Text = ((string)defaultProperties["MLdataSourceDirectory"]);
            }
            else
            {
                rtbMLdataSourceDirectory.Text = "";
                defaultProperties.Add("MLdataSourceDirectory", rtbMLdataSourceDirectory.Text);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }



            //SerializedDecisionTreePath
            if (defaultProperties.ContainsKey("SerializedDecisionTreePath"))
            {
                SerializedDecisionTreePath = ((string)defaultProperties["SerializedDecisionTreePath"]);
            }
            else
            {
                SerializedDecisionTreePath = rtbOutputDirectory.Text;
                defaultProperties.Add("SerializedDecisionTreePath", SerializedDecisionTreePath);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }





            //TimeSpanForSDCdataMappingTolerance
            if (defaultProperties.ContainsKey("TimeSpanForSDCdataMappingTolerance"))
            {
                TimeSpanForSDCdataMappingTolerance =
                    TimeSpan.Parse((string)defaultProperties["TimeSpanForSDCdataMappingTolerance"]);
            }
            else
            {
                TimeSpanForSDCdataMappingTolerance = new TimeSpan(0, 2, 0);
                defaultProperties.Add("TimeSpanForSDCdataMappingTolerance", TimeSpanForSDCdataMappingTolerance.ToString());
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }





            //TimeSpanForConcurrentDataMappingTolerance
            if (defaultProperties.ContainsKey("TimeSpanForConcurrentDataMappingTolerance"))
            {
                TimeSpanForConcurrentDataMappingTolerance =
                    TimeSpan.Parse((string)defaultProperties["TimeSpanForConcurrentDataMappingTolerance"]);
            }
            else
            {
                TimeSpanForConcurrentDataMappingTolerance = new TimeSpan(0, 0, 30);
                defaultProperties.Add("TimeSpanForConcurrentDataMappingTolerance", TimeSpanForConcurrentDataMappingTolerance.ToString());
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }




            //ConcurrentDataXMLfilesDirectory
            if (defaultProperties.ContainsKey("ConcurrentDataXMLfilesDirectory"))
            {
                ConcurrentDataXMLfilesDirectory = (string)defaultProperties["ConcurrentDataXMLfilesDirectory"];
                rtbConcurrentDataXMLfilesDirectory.Text = ConcurrentDataXMLfilesDirectory;
            }
            else
            {
                ConcurrentDataXMLfilesDirectory = CurDir;
                rtbConcurrentDataXMLfilesDirectory.Text = ConcurrentDataXMLfilesDirectory;
                defaultProperties.Add("ConcurrentDataXMLfilesDirectory", ConcurrentDataXMLfilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }




            //IncludeGPSandSunAltitudeData
            if (defaultProperties.ContainsKey("IncludeGPSandSunAltitudeData"))
            {
                cbIncludeGPSandSunAltitudeData.Checked = (((string)defaultProperties["IncludeGPSandSunAltitudeData"]).ToLower() == "true");
            }
            else
            {
                cbIncludeGPSandSunAltitudeData.Checked = true;
                defaultProperties.Add("IncludeGPSandSunAltitudeData", true);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }





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




            // NNconfigFile
            if (defaultProperties.ContainsKey("NNconfigFile"))
            {
                NNconfigFile = ((string)defaultProperties["NNconfigFile"]);
            }
            else
            {
                NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNconfig.csv";
                defaultProperties.Add("NNconfigFile", NNconfigFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            //NNtrainedParametersFile
            if (defaultProperties.ContainsKey("NNtrainedParametersFile"))
            {
                NNtrainedParametersFile = ((string)defaultProperties["NNtrainedParametersFile"]);
            }
            else
            {
                NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNtrainedParameters.csv";
                defaultProperties.Add("NNtrainedParametersFile", NNtrainedParametersFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }



            // NormMeansFile
            if (defaultProperties.ContainsKey("NormMeansFile"))
            {
                NormMeansFile = ((string)defaultProperties["NormMeansFile"]);
            }
            else
            {
                NormMeansFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormMeans.csv";
                defaultProperties.Add("NormMeansFile", NormMeansFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            // NormRangeFile
            if (defaultProperties.ContainsKey("NormRangeFile"))
            {
                NormRangeFile = ((string)defaultProperties["NormRangeFile"]);
            }
            else
            {
                NormRangeFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormRange.csv";
                defaultProperties.Add("NormRangeFile", NormRangeFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }







            if (CommonTools.console_present())
            {
                CommonTools.PrintDictionaryToConsole(defaultProperties, "Effective settings.");
            }
            else
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    CommonTools.DictionaryRepresentation(defaultProperties, "Effective settings"));
            }




            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }







        private void OcActiveStatsCalculationBgws_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ocActiveStatsCalculationBgws.Count < maxConcurrentBackgroundWorkers)
            {
                if (oqGrIxStatsCalculationQueue.Any())
                {
                    string filename = oqGrIxStatsCalculationQueue.Dequeue();
                    StartBgwCalculatingGrIxStats(filename);

                }
                else
                {
                    lblCalculationProgressPercentage.Text = "---";
                    lblCalculationProgressPercentage.Visible = false;
                    ThreadSafeOperations.SetLoadingCircleState(circBgwProcessingImage, false, false,
                        circBgwProcessingImage.Color, circBgwProcessingImage.RotationSpeed);
                }

            }
        }













        private void BgwCurrImageProcessing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AbortableBackgroundWorkerWithParameters currBGW = sender as AbortableBackgroundWorkerWithParameters;
            string currentFullFileName = "";
            bool calcResult = false;
            SkyImageMedianPerc5Data resStats = null;
            SkyImageIndexesStatsData GrIxYRGBstats = null;
            object[] currentBGWResults = null;



            try
            {
                currentBGWResults = (object[])e.Result;
            }
            catch (Exception)
            {
                currentFullFileName = (string)currBGW.parameters[0];
                calcResult = false;
                ImageStatsCollectingData foundDataObj =
                    lGrIxStatsCalculation.Find(obj => obj.filename == currentFullFileName);
                foundDataObj.State = ImageGrIxStatsCollectingState.Aborted;
                UpdateDataGrid();
                dataGridView1.Refresh();
                return;
            }



            if (currentBGWResults != null)
            {
                currentFullFileName = (string)currentBGWResults[0];
                resStats = (SkyImageMedianPerc5Data)currentBGWResults[1];
                calcResult = (bool)currentBGWResults[2];
                GrIxYRGBstats = (SkyImageIndexesStatsData)currentBGWResults[3];
            }


            WorkersWithWatchers currWorkerWithWatchers = ocActiveStatsCalculationBgws.ToList().Find(www => www.worker == currBGW);
            currWorkerWithWatchers.stopwatch.Stop();
            ocActiveStatsCalculationBgws.Remove(currWorkerWithWatchers);

            // save performance counters
            string strPerfCountersData = currentFullFileName + ";" +
                                            currWorkerWithWatchers.stopwatch.ElapsedMilliseconds + ";" +
                                            (currWorkerWithWatchers.procTotalProcessorTimeEnd - currWorkerWithWatchers.procTotalProcessorTimeStart).TotalMilliseconds + Environment.NewLine;
            ServiceTools.logToTextFile(strPerformanceCountersStatsFile, strPerfCountersData, true);


            currBGW.Dispose();
            bgwCurrImageProcessing = null;


            if (calcResult)
            {
                string strImageGrIxMedianP5DataFileName =
                    ConventionalTransitions.ImageGrIxMedianP5DataFileName(currentFullFileName, imageMP5statsXMLdataFilesDirectory);
                ServiceTools.WriteObjectToXML(resStats, strImageGrIxMedianP5DataFileName);
                string strImageGrIxYRGBDataFileName =
                    ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currentFullFileName, imageYRGBstatsXMLdataFilesDirectory);
                ServiceTools.WriteObjectToXML(GrIxYRGBstats, strImageGrIxYRGBDataFileName);


                ImageStatsCollectingData foundDataObj =
                    lGrIxStatsCalculation.Find(obj => obj.filename == currentFullFileName);
                foundDataObj.State = ImageGrIxStatsCollectingState.Finished;
                foundDataObj.GrIxMedianValue = resStats.GrIxStatsMedian;
                foundDataObj.GrIxPerc5Value = resStats.GrIxStatsPerc5;

                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "finished processing file " + Environment.NewLine + currentFullFileName);

                theLogWindow.ClearLog(10000);


                UpdateDataGrid();

                dataGridView1.Refresh();
            }



            //ThreadSafeOperations.SetLoadingCircleState(circBgwProcessingImage, false, false,
            //    circBgwProcessingImage.Color);

            //UpdateMedianPerc5DataLabels(res);


        }



        #region // BgwCurrImageProcessing_DoWork - obsolete

        //private void BgwCurrImageProcessing_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    ThreadSafeOperations.SetLoadingCircleState(circBgwProcessingImage, true, true,
        //        circBgwProcessingImage.Color);

        //    object[] currBGWarguments = (object[])e.Argument;
        //    string currentFullFileName = (string)currBGWarguments[0];

        //    try
        //    {
        //        Tuple<double, double> tplMedianPerc5Data = ImageProcessing.CalculateMedianPerc5Values(currentFullFileName);
        //        SkyImageIndexesStatsData grixyrgbStatsData = ImageProcessing.CalculateIndexesStats(currentFullFileName);
        //        SkyImageMedianPerc5Data mp5dt = new SkyImageMedianPerc5Data(currentFullFileName, tplMedianPerc5Data.Item1, tplMedianPerc5Data.Item2);
        //        e.Result = new object[] { currentFullFileName, mp5dt, true, grixyrgbStatsData };
        //    }
        //    catch (Exception ex)
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow,
        //                    "exception has been thrown: " + ex.Message + Environment.NewLine +
        //                    ServiceTools.CurrentCodeLineDescription());

        //        SkyImageMedianPerc5Data mp5dt = new SkyImageMedianPerc5Data(currentFullFileName, 0.0d, 0.0d);
        //        e.Result = new object[] { currentFullFileName, mp5dt, false, null };
        //    }
        //}

        #endregion // BgwCurrImageProcessing_DoWork



        private async void OpenFile(string filename)
        {
            Task tsk = new Task(() =>
            {
                string currImageDir = Path.GetDirectoryName(filename);
                alreadyMarkedSunDiskConditionFiles = new List<string>(Directory.EnumerateFiles(currImageDir, "*.jpg", SearchOption.TopDirectoryOnly));
                alreadyMarkedSunDiskConditionFiles.RemoveAll(strFName => !File.Exists(ConventionalTransitions.SunDiskConditionFileName(strFName, SunDiskConditionXMLdataFilesDirectory)));
            });

            bool tskStarted = false;
            if (bReviewMode && !alreadyMarkedSunDiskConditionFiles.Any())
            {
                tsk.Start();
                tskStarted = true;
            }




            currImageFileName = filename;
            currImageFInfo = new FileInfo(currImageFileName);
            currImage = new Image<Bgr, byte>(currImageFileName);

            ThreadSafeOperations.SetText(lblImageTitle, currImageFileName, false);
            ThreadSafeOperations.UpdatePictureBox(currImagePictureBox, currImage.Bitmap, true);



            LoadExistingSunDiskConditionData();

            if (!LoadExistingMedianPerc5Data())
            {
                if (cbCalculateGrIxStatsOnline.Checked)
                {
                    AddImageToGrIxStatsCalculationQueue(filename);
                }
            }



            if (tskStarted)
            {
                await tsk;
            }
        }







        private void btnStopCalculations_Click(object sender, EventArgs e)
        {
            lGrIxStatsCalculation.RemoveAll(
                statsCollectingData => statsCollectingData.State == ImageGrIxStatsCollectingState.Queued);
            UpdateDataGrid();
            oqGrIxStatsCalculationQueue.Clear();


            DialogResult ans = MessageBox.Show("Do you want to interrupt currently calculating threads?", "Question",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (ans == DialogResult.Yes)
            {
                List<WorkersWithWatchers> bgwsToAbort = ocActiveStatsCalculationBgws.ToList();

                foreach (WorkersWithWatchers bgw in bgwsToAbort)
                {
                    if (bgw.worker.IsBusy)
                    {
                        bgw.worker.Abort();
                        bgw.stopwatch.Stop();
                        bgw.stopwatch = null;
                        ocActiveStatsCalculationBgws.Remove(bgw);
                        bgw.worker.Dispose();
                    }
                }

                //lGrIxStatsCalculation.Clear();
            }
        }





        private List<ImageStatsCollectingData> lGrIxStatsCalculation = new List<ImageStatsCollectingData>();
        private BindingList<ImageStatsCollectingData> listBinding;
        private ObservableQueue<string> oqGrIxStatsCalculationQueue = new ObservableQueue<string>();
        private void AddImageToGrIxStatsCalculationQueue(string imgFilename)
        {
            if (lGrIxStatsCalculation.FindIndex(statsDatum => statsDatum.filename == imgFilename) > -1)
            {
                return;
            }

            lGrIxStatsCalculation.Add(new ImageStatsCollectingData()
            {
                filename = imgFilename,
                GrIxMedianValue = 0.0d,
                GrIxPerc5Value = 0.0d,
                State = ImageGrIxStatsCollectingState.Queued
            });

            if (bPermitProcessingStart)
            {
                UpdateDataGrid();
            }

            oqGrIxStatsCalculationQueue.Enqueue(imgFilename);
        }




        private void UpdateDataGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listBinding;
        }





        private void OqGrIxStatsCalculationQueue_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                return;
            }

            if (!bPermitProcessingStart)
            {
                return;
            }

            if (bgwCurrImageProcessing == null)
            {
                string filename = oqGrIxStatsCalculationQueue.Dequeue();

                ServiceTools.ExecMethodInSeparateThread(this, delegate { StartBgwCalculatingGrIxStats(filename); });
            }
            else
            {
                return;
            }
        }





        private void StartBgwCalculatingGrIxStats(string imgFilename)
        {
            Stopwatch CurrProcSW = new Stopwatch();
            CurrProcSW.Start();

            ImageStatsCollectingData foundDataObj = lGrIxStatsCalculation.Find(obj => obj.filename == imgFilename);
            foundDataObj.State = ImageGrIxStatsCollectingState.Calculating;
            UpdateDataGrid();
            dataGridView1.Refresh();


            bgwCurrImageProcessing = new AbortableBackgroundWorkerWithParameters();
            bgwCurrImageProcessing.DoWork += ImageProcessing.CalculateImageStatsData_BGWdoWork; // BgwCurrImageProcessing_DoWork;
            bgwCurrImageProcessing.RunWorkerCompleted += BgwCurrImageProcessing_RunWorkerCompleted;
            bgwCurrImageProcessing.parameters = new object[] { imgFilename };

            Dictionary<string, object> bgwCurrImageProcessingOptionalParameters = new Dictionary<string, object>();
            if (defaultProperties.ContainsKey("SaveControlImagesToPath"))
            {
                if ((((string)defaultProperties["SaveControlImagesToPath"]).ToLower() != "no") &&
                    (((string)defaultProperties["SaveControlImagesToPath"]).ToLower() != "false"))
                {
                    bgwCurrImageProcessingOptionalParameters.Add("SaveControlImagesToPath", (string)defaultProperties["SaveControlImagesToPath"]);
                }
            }
            else
            {
                bgwCurrImageProcessingOptionalParameters.Add("SaveControlImagesToPath", "same as source image");
            }


            if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
            {
                bgwCurrImageProcessingOptionalParameters.Add("ImagesRoundMasksXMLfilesMappingList", (string)defaultProperties["ImagesRoundMasksXMLfilesMappingList"]);
            }

            #region passing performance counters
            WorkersWithWatchers currWorkerWithWatchers = new WorkersWithWatchers()
            {
                worker = bgwCurrImageProcessing,
                stopwatch = CurrProcSW
            };
            bgwCurrImageProcessingOptionalParameters.Add("currWorkerWithWatchers", currWorkerWithWatchers);
            #endregion passing performance counters


            object[] BGWorker2Args = new object[] { imgFilename, bgwCurrImageProcessingOptionalParameters };
            bgwCurrImageProcessing.RunWorkerAsync(BGWorker2Args);

            ocActiveStatsCalculationBgws.Add(currWorkerWithWatchers);

            int finished =
                lGrIxStatsCalculation.FindAll(stats => stats.State == ImageGrIxStatsCollectingState.Finished).Count;

            double perc = 100.0d * (double)finished / (double)lGrIxStatsCalculation.Count;

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "started processing file " + (finished + 1).ToString() + " of " + lGrIxStatsCalculation.Count + ":" +
                Environment.NewLine + imgFilename);

            lblCalculationProgressPercentage.Visible = true;
            string lblText = "processing " + imgFilename + Environment.NewLine + perc.ToString("F02") + "% of images set";
            ThreadSafeOperations.SetText(lblCalculationProgressPercentage, lblText, false);
        }





        private void SunPresenceCollectingMainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
                return;
            }





            if (e.KeyChar == 'q')
            {
                MarkSunCondition(SunDiskCondition.NoSun);
                return;
            }

            if (e.KeyChar == 'w')
            {
                MarkSunCondition(SunDiskCondition.Sun0);
                return;
            }

            if (e.KeyChar == 'e')
            {
                MarkSunCondition(SunDiskCondition.Sun1);
                return;
            }

            if (e.KeyChar == 'r')
            {
                MarkSunCondition(SunDiskCondition.Sun2);
                return;
            }

            if (e.KeyChar == ' ')
            {
                MarkSunCondition(SunDiskCondition.Defect);
                return;
            }
        }



        private void SunPresenceCollectingMainForm_ArrowKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                PrevPicture();
                return;
            }

            if (e.KeyCode == Keys.Right)
            {
                NextPicture();
                return;
            }
        }





        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                object sender = Control.FromHandle(msg.HWnd);
                KeyEventArgs e = new KeyEventArgs(keyData);
                SunPresenceCollectingMainForm_ArrowKeyPress(sender, e);
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }




        private void NextPicture()
        {
            if (currImageFileName == "")
            {
                return;
            }

            //if (bgwCurrImageProcessing != null)
            //{
            //    if (bgwCurrImageProcessing.IsBusy)
            //    {
            //        return;
            //    }
            //}





            string currImageDir = Path.GetDirectoryName(currImageFileName);
            if (!Directory.Exists(currImageDir))
            {
                return;
            }

            List<string> filesList; // = new List<string>(Directory.EnumerateFiles(currImageDir, "*.jpg", SearchOption.TopDirectoryOnly));
            if (bReviewMode && alreadyMarkedSunDiskConditionFiles.Any())
            {
                //filesList.RemoveAll(strFName => !alreadyMarkedSunDiskConditionFiles.Contains(strFName));
                filesList = new List<string>(alreadyMarkedSunDiskConditionFiles);
            }
            else
            {
                filesList = new List<string>(Directory.EnumerateFiles(currImageDir, "*.jpg", SearchOption.TopDirectoryOnly));
            }


            if (!bShuffleImages)
            {
                int currFileIdx = filesList.FindIndex(str => str == currImageFileName);
                if (currFileIdx < 0) // ничего не найдено
                {
                    // возьмем первый
                    OpenFile(filesList[0]);
                }
                else if (currFileIdx == filesList.Count - 1)
                {
                    return;
                }
                else
                {
                    OpenFile(filesList[currFileIdx + 1]);
                }
            }
            else
            {
                if (!bPermitRepeats)
                {
                    filesList.RemoveAll(strFName => strFName == currImageFileName);
                    filesList.RemoveAll(
                        strFName => File.Exists(ConventionalTransitions.ImageGrIxMedianP5DataFileName(strFName, imageMP5statsXMLdataFilesDirectory)));
                }

                if (!filesList.Any())
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "There is no more images.");
                    return;
                }

                Random rand = new Random();
                filesList = filesList.OrderBy(c => rand.Next()).ToList();
                OpenFile(filesList[0]);
            }




        }






        private void PrevPicture()
        {
            if (currImageFileName == "")
            {
                return;
            }

            //if (bgwCurrImageProcessing != null)
            //{
            //    if (bgwCurrImageProcessing.IsBusy)
            //    {
            //        return;
            //    }
            //}


            string currImageDir = Path.GetDirectoryName(currImageFileName);
            if (!Directory.Exists(currImageDir))
            {
                return;
            }


            List<string> filesList; // = new List<string>(Directory.EnumerateFiles(currImageDir, "*.jpg", SearchOption.TopDirectoryOnly));
            if (bReviewMode && alreadyMarkedSunDiskConditionFiles.Any())
            {
                //filesList.RemoveAll(strFName => !alreadyMarkedSunDiskConditionFiles.Contains(strFName));
                filesList = new List<string>(alreadyMarkedSunDiskConditionFiles);
            }
            else
            {
                filesList = new List<string>(Directory.EnumerateFiles(currImageDir, "*.jpg", SearchOption.TopDirectoryOnly));
            }

            if (!bShuffleImages)
            {

                int currFileIdx = filesList.FindIndex(str => str == currImageFileName);
                if (currFileIdx < 0) // ничего не найдено
                {
                    // возьмем первый
                    OpenFile(filesList[0]);
                }
                else if (currFileIdx == 0)
                {
                    return;
                }
                else
                {
                    OpenFile(filesList[currFileIdx - 1]);
                }
            }
            else
            {
                if (!bPermitRepeats)
                {
                    filesList.RemoveAll(strFName => strFName == currImageFileName);
                    filesList.RemoveAll(
                        strFName => File.Exists(ConventionalTransitions.ImageGrIxMedianP5DataFileName(strFName, imageMP5statsXMLdataFilesDirectory)));
                }

                if (!filesList.Any())
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "There is no more images.");
                    return;
                }

                Random rand = new Random();
                filesList = filesList.OrderBy(c => rand.Next()).ToList();
                OpenFile(filesList[0]);
            }
        }





        private bool LoadExistingMedianPerc5Data()
        {
            bool bExistingDataLoaded = true;
            string strImageGrIxMedianP5DataFileName =
                ConventionalTransitions.ImageGrIxMedianP5DataFileName(currImageFInfo, imageMP5statsXMLdataFilesDirectory);

            if (File.Exists(strImageGrIxMedianP5DataFileName))
            {
                object currGrIxMedianP5DataObj =
                    ServiceTools.ReadObjectFromXML(strImageGrIxMedianP5DataFileName, typeof(SkyImageMedianPerc5Data));
                if (currGrIxMedianP5DataObj != null)
                {
                    SkyImageMedianPerc5Data currGrIxMedianP5Data = (SkyImageMedianPerc5Data)currGrIxMedianP5DataObj;

                    UpdateMedianPerc5DataLabels(currGrIxMedianP5Data);
                }
            }
            else
            {
                bExistingDataLoaded = false;
                ThreadSafeOperations.SetText(lblGrIxMedianValue, "---", false);
                ThreadSafeOperations.SetText(lblGrIxPerc5Value, "---", false);
            }

            return bExistingDataLoaded;
        }





        private void UpdateMedianPerc5DataLabels(SkyImageMedianPerc5Data mp5Datum)
        {
            ThreadSafeOperations.SetText(lblGrIxMedianValue, mp5Datum.GrIxStatsMedian.ToString("F04"), false);
            ThreadSafeOperations.SetText(lblGrIxPerc5Value, mp5Datum.GrIxStatsPerc5.ToString("F04"), false);
        }





        private bool LoadExistingSunDiskConditionData()
        {
            bool bExistingDataLoaded = true;

            string strSunDiskConditionFileName =
                ConventionalTransitions.SunDiskConditionFileName(currImageFInfo, SunDiskConditionXMLdataFilesDirectory);

            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkNoSun, Color.Gainsboro);
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun0, Color.Gainsboro);
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun1, Color.Gainsboro);
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun2, Color.Gainsboro);
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkDefect, Color.Gainsboro);


            // посмотрим - есть ли уже существующие результаты.
            // Если есть, - отобразим на состоянии кнопок и на данных median-perc5
            if (File.Exists(strSunDiskConditionFileName))
            {
                object currImageSunDiskConditionObj =
                    ServiceTools.ReadObjectFromXML(strSunDiskConditionFileName, typeof(SunDiskConditionData));
                if (currImageSunDiskConditionObj != null)
                {
                    SunDiskConditionData currImageSunDiskCondition = (SunDiskConditionData)currImageSunDiskConditionObj;
                    switch (currImageSunDiskCondition.sunDiskCondition)
                    {
                        case SunDiskCondition.NoSun:
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btnMarkNoSun, Color.LightCoral);
                                break;
                            }
                        case SunDiskCondition.Sun0:
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun0, Color.LightCoral);
                                break;
                            }
                        case SunDiskCondition.Sun1:
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun1, Color.LightCoral);
                                break;
                            }
                        case SunDiskCondition.Sun2:
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun2, Color.LightCoral);
                                break;
                            }
                        case SunDiskCondition.Defect:
                            {
                                ThreadSafeOperations.SetButtonBackgroundColor(btnMarkDefect, Color.LightCoral);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            else
            {
                bExistingDataLoaded = false;
            }

            return bExistingDataLoaded;
        }




        private void SaveSunDiskConditionData()
        {
            string strSunDiskConditionFileName =
                ConventionalTransitions.SunDiskConditionFileName(currImageFInfo, SunDiskConditionXMLdataFilesDirectory);

            ServiceTools.WriteObjectToXML(currImageSunDiskConditionData, strSunDiskConditionFileName);
        }




        private void btnProperties_Click(object sender, EventArgs e)
        {
            PropertiesEditor propForm = new PropertiesEditor(defaultProperties, defaultPropertiesXMLfileName);
            propForm.FormClosed += PropertiesFormClosed;
            propForm.ShowDialog();
        }




        public void PropertiesFormClosed(object sender, FormClosedEventArgs e)
        {
            readDefaultProperties();
        }




        private void MarkSunCondition_Click(object sender, EventArgs e)
        {
            if (sender == btnMarkNoSun)
            {
                MarkSunCondition(SunDiskCondition.NoSun);
            }

            if (sender == btnMarkSun0)
            {
                MarkSunCondition(SunDiskCondition.Sun0);
            }

            if (sender == btnMarkSun1)
            {
                MarkSunCondition(SunDiskCondition.Sun1);
            }

            if (sender == btnMarkSun1)
            {
                MarkSunCondition(SunDiskCondition.Sun1);
            }

            if (sender == btnMarkSun2)
            {
                MarkSunCondition(SunDiskCondition.Sun2);
            }

            if (sender == btnMarkDefect)
            {
                MarkSunCondition(SunDiskCondition.Defect);
            }
        }




        private void MarkSunCondition(SunDiskCondition currCondition, bool saveOnDisk = true)
        {
            if (currImageFInfo == null)
            {
                return;
            }

            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkNoSun, Color.Gainsboro);
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun0, Color.Gainsboro);
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun1, Color.Gainsboro);
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun2, Color.Gainsboro);
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkDefect, Color.Gainsboro);

            currImageSunDiskConditionData = new SunDiskConditionData(currImageFInfo.FullName, currCondition);

            switch (currCondition)
            {
                case SunDiskCondition.NoSun:
                    {
                        ThreadSafeOperations.SetButtonBackgroundColor(btnMarkNoSun, Color.LightCoral);
                        break;
                    }
                case SunDiskCondition.Sun0:
                    {
                        ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun0, Color.LightCoral);
                        break;
                    }
                case SunDiskCondition.Sun1:
                    {
                        ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun1, Color.LightCoral);
                        break;
                    }
                case SunDiskCondition.Sun2:
                    {
                        ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun2, Color.LightCoral);
                        break;
                    }
                case SunDiskCondition.Defect:
                    {
                        ThreadSafeOperations.SetButtonBackgroundColor(btnMarkDefect, Color.LightCoral);
                        break;
                    }
                default:
                    break;
            }



            if (saveOnDisk)
            {
                SaveSunDiskConditionData();

                if (!alreadyMarkedSunDiskConditionFiles.Contains(currImageFInfo.FullName))
                {
                    alreadyMarkedSunDiskConditionFiles.Add(currImageFInfo.FullName);
                }
            }



        }





        private void SunPresenceCollectingMainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }



        private void SunPresenceCollectingMainForm_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] FilesArray = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (FilesArray != null)
                {
                    string FileName1 = FilesArray[0];

                    string currDir = Path.GetDirectoryName(FileName1);
                    if (!defaultProperties.ContainsKey("ImagesBasePath"))
                    {
                        defaultProperties.Add("ImagesBasePath", currDir);
                    }
                    else
                    {
                        defaultProperties["ImagesBasePath"] = currDir;
                    }
                    saveDefaultProperties();


                    this.BeginInvoke(m_DelegateOpenFile, new Object[] { FileName1 });
                    this.Activate();
                }
            }
            catch (Exception exc1)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Ошибка при обработке Drag&Drop: " + exc1.Message, true);
            }
        }








        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }





        private void cbCalculateGrIxStatsOnline_CheckedChanged(object sender, EventArgs e)
        {
            if (defaultProperties.ContainsKey("CalculateGrIxStatsOnline"))
            {
                defaultProperties["CalculateGrIxStatsOnline"] = cbCalculateGrIxStatsOnline.Checked;
            }
            else
            {
                defaultProperties.Add("CalculateGrIxStatsOnline", cbCalculateGrIxStatsOnline.Checked);
            }
            saveDefaultProperties();
        }




        private void SunPresenceCollectingMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!defaultProperties.ContainsKey("ImagesBasePath"))
            {
                defaultProperties.Add("ImagesBasePath", currPath2Process);
            }
            else
            {
                defaultProperties["ImagesBasePath"] = currPath2Process;
            }



            if (defaultProperties.ContainsKey("CalculateGrIxStatsOnline"))
            {
                defaultProperties["CalculateGrIxStatsOnline"] = cbCalculateGrIxStatsOnline.Checked;
            }
            else
            {
                defaultProperties.Add("CalculateGrIxStatsOnline", cbCalculateGrIxStatsOnline.Checked);
            }

            saveDefaultProperties();
        }










        #region XML files set conversion to one CSV file


        private void btnSelectSourceDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            if (rtbSourceDirectory.Text != "")
            {
                if (Directory.Exists(rtbSourceDirectory.Text))
                {
                    dlg.SelectedPath = rtbSourceDirectory.Text;
                }
            }
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.OK)
            {
                return;
            }
            else
            {
                string path = dlg.SelectedPath;
                rtbSourceDirectory.Text = path;
                if (!defaultProperties.ContainsKey("ConversionSourceDirectory"))
                {
                    defaultProperties.Add("ConversionSourceDirectory", path);
                }
                else
                    defaultProperties["ConversionSourceDirectory"] = path;

                saveDefaultProperties();
            }
        }





        private void btnSelectOutputDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            if (rtbOutputDirectory.Text != "")
            {
                if (Directory.Exists(rtbOutputDirectory.Text))
                {
                    dlg.SelectedPath = rtbOutputDirectory.Text;
                }
            }
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.OK)
            {
                return;
            }
            else
            {
                string path = dlg.SelectedPath;
                rtbOutputDirectory.Text = path;
                if (!defaultProperties.ContainsKey("ConversionOutputDirectory"))
                {
                    defaultProperties.Add("ConversionOutputDirectory", path);
                }
                else
                    defaultProperties["ConversionOutputDirectory"] = path;

                saveDefaultProperties();
            }
        }






        private BackgroundWorker bgwConversion = null;
        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (bgwConversion == null)
            {
                StartConversion();
            }
            else if (!bgwConversion.IsBusy)
            {
                StartConversion();
            }
            else
            {
                bgwConversion.CancelAsync();
            }
        }



        private void StartConversion()
        {
            string currDir = rtbSourceDirectory.Text;
            if (!Directory.Exists(currDir))
            {
                ServiceTools.ExecMethodInSeparateThread(this,
                    () => theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the source directory."));
                return;
            }
            string outPath = rtbOutputDirectory.Text;
            outPath += (outPath.Last() == Path.DirectorySeparatorChar) ? ("") : (Path.DirectorySeparatorChar.ToString());
            if (!Directory.Exists(outPath))
            {
                ServiceTools.ExecMethodInSeparateThread(this,
                    () => { theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the output directory."); });
                return;
            }

            string strSDCdataCSVfileName = rtbSDCdataCSVfile.Text;
            if (!File.Exists(strSDCdataCSVfileName))
            {
                ServiceTools.ExecMethodInSeparateThread(this,
                    () =>
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "SDC data CSV file is not specified or doesn`t exist." + Environment.NewLine +
                            "\tSo SDC data will be obtained using individual XML files or the conversion will be interrupted.");
                    });
            }


            string strConcurrentDataXMLfilesDirectory = "";
            if (cbIncludeGPSandSunAltitudeData.Checked)
            {
                strConcurrentDataXMLfilesDirectory = rtbConcurrentDataXMLfilesDirectory.Text;
                if (!Directory.Exists(strConcurrentDataXMLfilesDirectory))
                {
                    ServiceTools.ExecMethodInSeparateThread(this,
                        () =>
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                "concurrent data directory is not specified or does not exist." + Environment.NewLine +
                                "\tSo while checkbox \"include GPS and sun altitude data\" it has been specified and exist." + Environment.NewLine + "" +
                                "\tPlease make sure you really want to include GPS and sun altitude data or check if the directory path specified and it does exist.");
                        });
                    return;
                }
            }




            bgwConversion = new BackgroundWorker();
            bgwConversion.DoWork += BgwConversion_DoWork;
            bgwConversion.RunWorkerCompleted += BgwConversion_RunWorkerCompleted;
            bgwConversion.WorkerSupportsCancellation = true;
            bgwConversion.WorkerReportsProgress = true;
            bgwConversion.ProgressChanged += BgwConversion_ProgressChanged;
            object[] bgwArgs = { currDir, outPath, strSDCdataCSVfileName, strConcurrentDataXMLfilesDirectory };
            bgwConversion.RunWorkerAsync(bgwArgs);


            ServiceTools.ExecMethodInSeparateThread(this,
                    () =>
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "conversion started");
                    });


            ThreadSafeOperations.ToggleButtonState(btnConvert, true, "CANCEL conversion", true);
        }


        private void BgwConversion_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ThreadSafeOperations.UpdateProgressBar(prbConversionProgress, e.ProgressPercentage);
        }



        private void BgwConversion_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            object[] objArgs = (object[])e.Argument;
            string currDir = (string)objArgs[0];
            string outPath = (string)objArgs[1];
            string strSDCdataCSVfileName = (string)objArgs[2];
            string strConcurrentDataXMLfilesDirectory = (string)objArgs[3];

            bool bIncludeGPSandSunAltitudeData = false;
            if (cbIncludeGPSandSunAltitudeData.Checked && Directory.Exists(strConcurrentDataXMLfilesDirectory))
            {
                bIncludeGPSandSunAltitudeData = true;
            }

            #region enumerate available XML files


            List<string> filesListConcurrentData = new List<string>();
            if (bIncludeGPSandSunAltitudeData)
            {
                filesListConcurrentData =
                    new List<string>(Directory.EnumerateFiles(strConcurrentDataXMLfilesDirectory,
                        ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(),
                        ((cbConversionSearchInputsTopDirectoryOnly.Checked)
                            ? (SearchOption.TopDirectoryOnly)
                            : (SearchOption.AllDirectories))));
            }


            if (!filesListConcurrentData.Any())
            {
                // значит, речь идет об обработке данных до эпохи автоматической установки\
                // тогда данные должны быть подготовлены в виде XML-файлов с выгрузками классов типа SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC
                // их надо обработать по-другому. Ибо уже все подготовлено.

                Convert_SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC_XMLFiles(currDir, outPath);
                return;
            }


            List<string> filesListMP5statsData =
                new List<string>(Directory.EnumerateFiles(currDir,
                    ConventionalTransitions.ImageGrIxMedianP5DataFileNamesPattern(),
                    ((cbConversionSearchInputsTopDirectoryOnly.Checked)
                        ? (SearchOption.TopDirectoryOnly)
                        : (SearchOption.AllDirectories))));

            List<string> filesListSunConditionData =
                new List<string>(Directory.EnumerateFiles(currDir,
                    ConventionalTransitions.SunDiskConditionFileNamesPattern(),
                    ((cbConversionSearchInputsTopDirectoryOnly.Checked)
                        ? (SearchOption.TopDirectoryOnly)
                        : (SearchOption.AllDirectories))));

            List<string> filesListALLstatsData =
                new List<string>(Directory.EnumerateFiles(currDir,
                    ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern(),
                    ((cbConversionSearchInputsTopDirectoryOnly.Checked)
                        ? (SearchOption.TopDirectoryOnly)
                        : (SearchOption.AllDirectories))));


            #endregion enumerate available XML files

            int totalFilesCountToRead = filesListMP5statsData.Count + filesListSunConditionData.Count +
                                        filesListALLstatsData.Count;
            int currProgressPerc = 0;
            int filesRead = 0;


            #region read SDC data from XML files
            List<SunDiskConditionData> lImagesSunConditionData = new List<SunDiskConditionData>();
            foreach (string sunConditionDataFilename in filesListSunConditionData)
            {
                SunDiskConditionData currSunDiskConditionData =
                    (SunDiskConditionData)
                        ServiceTools.ReadObjectFromXML(sunConditionDataFilename, typeof(SunDiskConditionData));
                lImagesSunConditionData.Add(currSunDiskConditionData);

                filesRead++;
                double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                if (progress - (double)currProgressPerc > 1.0d)
                {
                    currProgressPerc = Convert.ToInt32(progress);
                    selfWorker.ReportProgress(currProgressPerc);
                }
                if (selfWorker.CancellationPending)
                {
                    return;
                }
            }

            foreach (SunDiskConditionData sunConditionData in lImagesSunConditionData)
            {
                sunConditionData.ShrinkFileName();
            }
            #endregion read SDC data from XML files


            List<string> lCorruptedXMLfiles = new List<string>();


            #region read GrIx_MP5 stats data from XML files

            List<SkyImageMedianPerc5Data> lImagesMP5StatsData = new List<SkyImageMedianPerc5Data>();
            foreach (string strMP5filename in filesListMP5statsData)
            {
                SkyImageMedianPerc5Data currMP5stats =
                    (SkyImageMedianPerc5Data)
                        ServiceTools.ReadObjectFromXML(strMP5filename, typeof(SkyImageMedianPerc5Data));

                if (currMP5stats == null)
                {
                    lCorruptedXMLfiles.Add(strMP5filename);
                    continue;
                }

                lImagesMP5StatsData.Add(currMP5stats);

                filesRead++;
                double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                if (progress - (double)currProgressPerc > 1.0d)
                {
                    currProgressPerc = Convert.ToInt32(progress);
                    selfWorker.ReportProgress(currProgressPerc);
                }

                if (selfWorker.CancellationPending)
                {
                    return;
                }
            }

            //foreach (SkyImageMedianPerc5Data statsData in lImagesMP5StatsData)
            //{
            //    statsData.ShrinkFileName();
            //}
            lImagesMP5StatsData = lImagesMP5StatsData.ConvertAll<SkyImageMedianPerc5Data>(simp5dt =>
            {
                simp5dt.ShrinkFileName();
                return simp5dt;
            });

            #endregion read GrIx_MP5 stats data from XML files


            #region read GrIxYRGB stats data from XML files

            List<SkyImageIndexesStatsData> lImagesALLstatsData = new List<SkyImageIndexesStatsData>();
            foreach (string sALLstatsDataFilename in filesListALLstatsData)
            {
                SkyImageIndexesStatsData currSkyImageIndexesStatsData =
                    (SkyImageIndexesStatsData)
                        ServiceTools.ReadObjectFromXML(sALLstatsDataFilename, typeof(SkyImageIndexesStatsData));

                if (currSkyImageIndexesStatsData == null)
                {
                    lCorruptedXMLfiles.Add(sALLstatsDataFilename);
                    continue;
                }

                lImagesALLstatsData.Add(currSkyImageIndexesStatsData);

                filesRead++;
                double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                if (progress - (double)currProgressPerc > 1.0d)
                {
                    currProgressPerc = Convert.ToInt32(progress);
                    selfWorker.ReportProgress(currProgressPerc);
                }
                if (selfWorker.CancellationPending)
                {
                    return;
                }
            }

            //foreach (SkyImageIndexesStatsData statsData in lImagesALLstatsData)
            //{
            //    statsData.ShrinkFileName();
            //}
            lImagesALLstatsData = lImagesALLstatsData.ConvertAll<SkyImageIndexesStatsData>(siIsdt =>
            {
                siIsdt.ShrinkFileName();
                return siIsdt;
            });

            #endregion read GrIxYRGB stats data from XML files


            #region // remove corrupted XML files
            //foreach (string corruptedXmLfile in lCorruptedXMLfiles)
            //{
            //    File.Delete(corruptedXmLfile);
            //    ServiceTools.ExecMethodInSeparateThread(this,
            //        () =>
            //        {
            //            theLogWindow = ServiceTools.LogAText(theLogWindow, "the file removed because it is corrupted or cannot be read: " + Environment.NewLine + corruptedXmLfile);
            //        });
            //}
            #endregion // remove corrupted XML files



            string ptrn = @"(devID\d)";
            Regex rgxp = new Regex(ptrn, RegexOptions.IgnoreCase);



            #region read concurrent data from XML files

            theLogWindow = ServiceTools.LogAText(theLogWindow, "started concurrent data reading");

            List<Tuple<string, ConcurrentData>> lImagesConcurrentData = new List<Tuple<string, ConcurrentData>>();

            if (bIncludeGPSandSunAltitudeData)
            {
                totalFilesCountToRead = filesListConcurrentData.Count;
                filesRead = 0;
                currProgressPerc = 0;
                selfWorker.ReportProgress(0);
                List<Dictionary<string, object>> lDictionariesConcurrentData = new List<Dictionary<string, object>>();
                foreach (string strConcDataXMLFile in filesListConcurrentData)
                {
                    Dictionary<string, object> currDict = ServiceTools.ReadDictionaryFromXML(strConcDataXMLFile);
                    currDict.Add("XMLfileName", Path.GetFileName(strConcDataXMLFile));

                    lDictionariesConcurrentData.Add(currDict);

                    #region calculate and report progress
                    filesRead++;
                    double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                    if (progress - (double)currProgressPerc > 1.0d)
                    {
                        currProgressPerc = Convert.ToInt32(progress);
                        selfWorker.ReportProgress(currProgressPerc);
                    }

                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }
                    #endregion calculate and report progress
                }



                lDictionariesConcurrentData.RemoveAll(dict => dict == null);
                List<ConcurrentData> lConcurrentData =
                    lDictionariesConcurrentData.ConvertAll<ConcurrentData>(dict =>
                    {
                        ConcurrentData retVal = null;
                        try
                        {
                            retVal = new ConcurrentData(dict);
                        }
                        catch (Exception ex)
                        {
                            string strError = "couldn`t parse XML file " + dict["XMLfileName"] + " : " +
                                              Environment.NewLine + ex.Message;
                            ServiceTools.logToTextFile(errorLogFilename, strError, true, true);
                        }
                        return retVal;
                    });
                lConcurrentData.RemoveAll(val => val == null);


                // map obtained concurrent data to images by its datetime
                theLogWindow = ServiceTools.LogAText(theLogWindow, "started concurrent data mapping");
                int totalrecordsToMap = lImagesALLstatsData.Count;
                int recordsMapped = 0;
                currProgressPerc = 0;
                selfWorker.ReportProgress(0);
                foreach (SkyImageIndexesStatsData statsData in lImagesALLstatsData)
                {
                    string currImgFilename = statsData.FileName;
                    currImgFilename = Path.GetFileNameWithoutExtension(currImgFilename);

                    string strCurrImgDT = rgxp.Replace(currImgFilename.Substring(4), "");
                    //2015-12-16T06-01-38
                    strCurrImgDT = strCurrImgDT.Substring(0, 11) + strCurrImgDT.Substring(11).Replace("-", ":");

                    DateTime currImgDT = DateTime.Parse(strCurrImgDT, null,
                        System.Globalization.DateTimeStyles.AdjustToUniversal);

                    // отсортировать записи сопутствующих данных считанных из XML файлов по близости к дате-времени изображения
                    //lConcurrentData.Sort(new Comparison<ConcurrentData>((cDt1, cDt2) =>
                    //{
                    //    TimeSpan tspan1 = new TimeSpan(Math.Abs((cDt1.datetimeUTC - currImgDT).Ticks));
                    //    TimeSpan tspan2 = new TimeSpan(Math.Abs((cDt2.datetimeUTC - currImgDT).Ticks));
                    //    return tspan1.CompareTo(tspan2);
                    //}));

                    //ConcurrentData nearestConcurrentData = lConcurrentData.First();

                    ConcurrentData nearestConcurrentData = lConcurrentData.Aggregate((cDt1, cDt2) =>
                    {
                        TimeSpan tspan1 = new TimeSpan(Math.Abs((cDt1.datetimeUTC - currImgDT).Ticks));
                        TimeSpan tspan2 = new TimeSpan(Math.Abs((cDt2.datetimeUTC - currImgDT).Ticks));
                        return ((tspan1 <= tspan2) ? (cDt1) : (cDt2));
                    });


                    if (new TimeSpan(Math.Abs((nearestConcurrentData.datetimeUTC - currImgDT).Ticks)) <= TimeSpanForConcurrentDataMappingTolerance)
                    {
                        lImagesConcurrentData.Add(new Tuple<string, ConcurrentData>(statsData.FileName,
                            nearestConcurrentData));
                    }

                    #region calculate and report progress
                    recordsMapped++;
                    double progress = 100.0d * (double)recordsMapped / (double)totalrecordsToMap;
                    if (progress - (double)currProgressPerc > 1.0d)
                    {
                        currProgressPerc = Convert.ToInt32(progress);
                        selfWorker.ReportProgress(currProgressPerc);
                    }

                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }
                    #endregion calculate and report progress
                }
            }


            #endregion read concurrent data from XML files




            #region read SDC data from CSV table

            // and compose SDC data list for files using datetime tolerance value

            if (File.Exists(strSDCdataCSVfileName))
            {
                List<List<string>> llCSVfileRawData = ServiceTools.ReadDataFromCSV(strSDCdataCSVfileName, 1, true, ";", Environment.NewLine);

                List<MeteoObservationsImportedData> lMeteoObsData = llCSVfileRawData
                    .ConvertAll<MeteoObservationsImportedData>(
                        lStr =>
                        {
                            DateTime dt = DateTime.Parse(lStr[0]);
                            double dSunAltitudeDeg = Convert.ToDouble(((string)lStr[1]).Replace(".", ","));
                            SunDiskCondition sdc = SunDiskCondition.Defect;
                            switch (Convert.ToInt16(lStr[2]))
                            {
                                case -1:
                                    {
                                        sdc = SunDiskCondition.NoSun;
                                        break;
                                    }
                                case 0:
                                    {
                                        sdc = SunDiskCondition.Sun0;
                                        break;
                                    }
                                case 1:
                                    {
                                        sdc = SunDiskCondition.Sun1;
                                        break;
                                    }
                                case 2:
                                    {
                                        sdc = SunDiskCondition.Sun2;
                                        break;
                                    }
                                default:
                                    {
                                        sdc = SunDiskCondition.Defect;
                                        break;
                                    }
                            }
                            return new MeteoObservationsImportedData(dt, dSunAltitudeDeg, sdc);
                        });



                // filter files that has not yet its SDC
                // map SDC records to stats data
                int totalrecordsToMap = lImagesALLstatsData.Count;
                int recordsMapped = 0;
                currProgressPerc = 0;
                selfWorker.ReportProgress(0);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "started SDC (read from CSV file) data mapping");
                foreach (SkyImageIndexesStatsData statsData in lImagesALLstatsData)
                {
                    string currImgFilename = statsData.FileName;
                    currImgFilename = Path.GetFileNameWithoutExtension(currImgFilename);

                    string strCurrImgDT = rgxp.Replace(currImgFilename.Substring(4), "");
                    //2015-12-16T06-01-38
                    strCurrImgDT = strCurrImgDT.Substring(0, 11) + strCurrImgDT.Substring(11).Replace("-", ":");
                    DateTime currImgDT = DateTime.Parse(strCurrImgDT, null,
                        System.Globalization.DateTimeStyles.AdjustToUniversal);

                    // отсортировать данные считанных из CSV метеоданных по близости к дате, извлеченной из имени файла изображения
                    //lMeteoObsData.Sort(new Comparison<MeteoObservationsImportedData>((mt1, mt2) =>
                    //{
                    //    TimeSpan tspan1 = new TimeSpan(Math.Abs((mt1.datetimeUTC - currImgDT).Ticks));
                    //    TimeSpan tspan2 = new TimeSpan(Math.Abs((mt2.datetimeUTC - currImgDT).Ticks));
                    //    return tspan1.CompareTo(tspan2);
                    //}));
                    //MeteoObservationsImportedData nearestMeteoObservationsData = lMeteoObsData.First();


                    MeteoObservationsImportedData nearestMeteoObservationsData = lMeteoObsData.Aggregate((mt1, mt2) =>
                    {
                        TimeSpan tspan1 = new TimeSpan(Math.Abs((mt1.datetimeUTC - currImgDT).Ticks));
                        TimeSpan tspan2 = new TimeSpan(Math.Abs((mt2.datetimeUTC - currImgDT).Ticks));
                        return ((tspan1 <= tspan2) ? (mt1) : (mt2));
                    });


                    if (new TimeSpan(Math.Abs((nearestMeteoObservationsData.datetimeUTC - currImgDT).Ticks)) <= TimeSpanForSDCdataMappingTolerance)
                    {
                        lImagesSunConditionData.Add(new SunDiskConditionData(statsData.FileName, nearestMeteoObservationsData.sdc));
                    }

                    #region calculate and report progress
                    recordsMapped++;
                    double progress = 100.0d * (double)recordsMapped / (double)totalrecordsToMap;
                    if (progress - (double)currProgressPerc > 1.0d)
                    {
                        currProgressPerc = Convert.ToInt32(progress);
                        selfWorker.ReportProgress(currProgressPerc);
                    }

                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }
                    #endregion calculate and report progress
                }

            }

            #endregion read SDC data from CSV table



            #region zip SDC data with GrIx MP5 data -> write to file

            List<SkyImageMP5andSDCdata> lDataSkyImagesMP5andSDCdata = lImagesMP5StatsData
                .ConvertAll<SkyImageMP5andSDCdata>(
                    stats =>
                    {
                        int foundSunConditionDatumIdx =
                            lImagesSunConditionData.FindIndex(scdt => scdt.filename == stats.FileName);
                        if (foundSunConditionDatumIdx != -1)
                        {
                            return new SkyImageMP5andSDCdata(stats, lImagesSunConditionData[foundSunConditionDatumIdx]);
                        }
                        else return null;

                    });
            lDataSkyImagesMP5andSDCdata.RemoveAll(val => val == null);


            List<string> lImagesStatsDataForCSV =
                lDataSkyImagesMP5andSDCdata.ConvertAll<string>(dt => dt.mp5Data.ToCSV() + "," + dt.sdcData.ToCSV());

            string csvStringForFile = String.Join(Environment.NewLine, lImagesStatsDataForCSV.ToArray<string>());

            ServiceTools.logToTextFile(outPath + "GrIxMedianP5Stats.csv",
                lDataSkyImagesMP5andSDCdata[0].mp5Data.CSVHeader() + "," +
                lDataSkyImagesMP5andSDCdata[0].sdcData.CSVHeader() + Environment.NewLine, false, false);
            ServiceTools.logToTextFile(outPath + "GrIxMedianP5Stats.csv", csvStringForFile, true, false);




            #region zip it with concurrent data and write to one more file

            if (bIncludeGPSandSunAltitudeData && lImagesConcurrentData.Any())
            {
                List<SkyImage_MP5_SDC_ConcurrentData> lDt = lDataSkyImagesMP5andSDCdata
                    .ConvertAll<SkyImage_MP5_SDC_ConcurrentData>(
                        mp5sdc =>
                        {
                            SkyImage_MP5_SDC_ConcurrentData retVal = new SkyImage_MP5_SDC_ConcurrentData()
                            {
                                mp5Data = mp5sdc.mp5Data,
                                sdcData = mp5sdc.sdcData
                            };

                            int foundConcDatumIdx =
                                lImagesConcurrentData.FindIndex(val => val.Item1 == mp5sdc.mp5Data.FileName);
                            if (foundConcDatumIdx != -1)
                            {
                                retVal.concurrentData = lImagesConcurrentData[foundConcDatumIdx].Item2;
                            }
                            else retVal = null;

                            return retVal;
                        });
                lDt.RemoveAll(val => val == null);


                List<string> lImagesStatsDataForCSVwithConcurrentData =
                    lDt.ConvertAll<string>(
                        dt =>
                            dt.mp5Data.ToCSV() + "," + dt.concurrentData.gps.SunZenithAzimuth().ElevationAngle.ToString().Replace(",", ".") + "," +
                            dt.concurrentData.gps.SunZenithAzimuth().Azimuth.ToString().Replace(",", ".") + "," + dt.sdcData.ToCSV());

                csvStringForFile = String.Join(Environment.NewLine, lImagesStatsDataForCSVwithConcurrentData.ToArray<string>());

                // write header
                ServiceTools.logToTextFile(outPath + "GrIxMedianP5StatsWithSunPositions.csv",
                    lDt[0].mp5Data.CSVHeader() + ",SunElevationDeg,SunAzimuthDeg," +
                    lDt[0].sdcData.CSVHeader() + Environment.NewLine, false, false);
                // write content
                ServiceTools.logToTextFile(outPath + "GrIxMedianP5StatsWithSunPositions.csv", csvStringForFile, true, false);
            }

            #endregion zip it with concurrent data and write to one more file

            #endregion zip SDC data with GrIx MP5 data -> write to file



            #region zip SDC data with ALL fields stats data -> write to file

            List<SkyImageGrIxYRGBstatsAndSDCdata> lDataALLstatsTuples = lImagesALLstatsData
                .ConvertAll<SkyImageGrIxYRGBstatsAndSDCdata>(
                    stats =>
                    {
                        int foundSunConditionDatumIdx =
                            lImagesSunConditionData.FindIndex(scdt => scdt.filename == stats.FileName);
                        if (foundSunConditionDatumIdx != -1)
                        {
                            return new SkyImageGrIxYRGBstatsAndSDCdata(stats,
                                lImagesSunConditionData[foundSunConditionDatumIdx]);
                        }
                        else return null;
                    });

            lDataALLstatsTuples.RemoveAll(tpl => tpl == null);

            List<string> lImagesALLstatsDataCSV = lDataALLstatsTuples.ConvertAll<string>(dt => dt.GrIxYRGBstatsData.ToCSV() + "," + dt.sdcData.ToCSV());

            string csvStringALLstatsForFile = String.Join(Environment.NewLine, lImagesALLstatsDataCSV.ToArray<string>());
            ServiceTools.logToTextFile(outPath + "ALLstats.csv",
                lDataALLstatsTuples[0].GrIxYRGBstatsData.CSVHeader() + "," + lDataALLstatsTuples[0].sdcData.CSVHeader() +
                Environment.NewLine, false, false);
            ServiceTools.logToTextFile(outPath + "ALLstats.csv", csvStringALLstatsForFile, true, false);



            #region zip it with concurrent data and write to one more file

            if (bIncludeGPSandSunAltitudeData && lImagesConcurrentData.Any())
            {
                List<SkyImage_GrIxYRGBstats_SDC_ConcurrentData> lDt = lDataALLstatsTuples
                    .ConvertAll<SkyImage_GrIxYRGBstats_SDC_ConcurrentData>(
                        statsSDC =>
                        {
                            SkyImage_GrIxYRGBstats_SDC_ConcurrentData retVal = new SkyImage_GrIxYRGBstats_SDC_ConcurrentData()
                            {
                                statsData = statsSDC.GrIxYRGBstatsData,
                                sdcData = statsSDC.sdcData
                            };

                            int foundConcDatumIdx =
                                lImagesConcurrentData.FindIndex(val => val.Item1 == statsSDC.GrIxYRGBstatsData.FileName);
                            if (foundConcDatumIdx != -1)
                            {
                                retVal.concurrentData = lImagesConcurrentData[foundConcDatumIdx].Item2;
                            }
                            else retVal = null;

                            return retVal;
                        });
                lDt.RemoveAll(val => val == null);


                List<string> lImagesALLstatsDataCSVWithConcurrentData =
                    lDt.ConvertAll<string>(
                        dt =>
                            dt.statsData.ToCSV() + "," +
                            dt.concurrentData.gps.SunZenithAzimuth().ElevationAngle.ToString().Replace(",", ".") + "," +
                            dt.concurrentData.gps.SunZenithAzimuth().Azimuth.ToString().Replace(",", ".") + "," + dt.sdcData.ToCSV());

                csvStringALLstatsForFile = String.Join(Environment.NewLine, lImagesALLstatsDataCSVWithConcurrentData.ToArray<string>());

                // write header
                ServiceTools.logToTextFile(outPath + "ALLstatsWithSunPositions.csv",
                    lDataALLstatsTuples[0].GrIxYRGBstatsData.CSVHeader() + ",SunElevationDeg,SunAzimuthDeg," + lDataALLstatsTuples[0].sdcData.CSVHeader() +
                    Environment.NewLine, false, false);
                // write content
                ServiceTools.logToTextFile(outPath + "ALLstatsWithSunPositions.csv", csvStringALLstatsForFile, true, false);

            }

            #endregion zip it with concurrent data and write to one more file



            #endregion zip SDC data with ALL fields stats data -> write to file
        }








        private void Convert_SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC_XMLFiles(string currDir, string outPath)
        {
            List<string> dataXMLfilesList =
                Directory.EnumerateFiles(currDir,
                    ConventionalTransitions.SkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileNamesPattern(),
                    ((cbConversionSearchInputsTopDirectoryOnly.Checked)
                        ? (SearchOption.TopDirectoryOnly)
                        : (SearchOption.AllDirectories))).ToList();

            List<SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC> readDataList =
                dataXMLfilesList.ConvertAll(xmlFileName => ServiceTools.ReadObjectFromXML(xmlFileName,
                    typeof(SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC)) as
                    SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC);

            int removed = readDataList.RemoveAll(dt => dt == null);





            List<SkyImage_GrIxYRGBstats_SDC_ConcurrentData> lDt = readDataList
                    .ConvertAll(
                        data =>
                        {
                            SkyImage_GrIxYRGBstats_SDC_ConcurrentData retVal = new SkyImage_GrIxYRGBstats_SDC_ConcurrentData()
                            {
                                statsData = data.grixyrgbStats,
                                sdcData = new SunDiskConditionData()
                                {
                                    filename = data.skyImageFileName,
                                    sunDiskCondition = data.SDCvalue
                                },
                                concurrentData = data.concurrentData
                            };
                            
                            return retVal;
                        });
            lDt.RemoveAll(val => val == null);


            List<string> lImagesALLstatsDataCSVWithConcurrentData =
                lDt.ConvertAll<string>(
                    dt =>
                        dt.statsData.ToCSV() + "," +
                        dt.concurrentData.gps.SunZenithAzimuth().ElevationAngle.ToString().Replace(",", ".") + "," +
                        dt.concurrentData.gps.SunZenithAzimuth().Azimuth.ToString().Replace(",", ".") + "," + dt.sdcData.ToCSV());

            string csvStringALLstatsForFile = String.Join(Environment.NewLine, lImagesALLstatsDataCSVWithConcurrentData.ToArray<string>());

            // write header
            ServiceTools.logToTextFile(outPath + "ALLstatsWithSunPositions.csv",
                lDt[0].statsData.CSVHeader() + ",SunElevationDeg,SunAzimuthDeg," + lDt[0].sdcData.CSVHeader() +
                Environment.NewLine, false, false);
            // write content
            ServiceTools.logToTextFile(outPath + "ALLstatsWithSunPositions.csv", csvStringALLstatsForFile, true, false);


        }








        private void BgwConversion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.UpdateProgressBar(prbConversionProgress, 0);
            theLogWindow = ServiceTools.LogAText(theLogWindow, "conversion finished");
            ThreadSafeOperations.ToggleButtonState(btnConvert, true, "convert to CSV files (one for (M,P5) predictors and one for all statistical predictors)", true);
        }

        #endregion XML files set conversion to one CSV file





        #region form behaviour



        private void btnServiceInputDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            if (rtbServiceSourceDirectory.Text != "")
            {
                if (Directory.Exists(rtbServiceSourceDirectory.Text))
                {
                    dlg.SelectedPath = rtbServiceSourceDirectory.Text;
                }
            }
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.OK)
            {
                return;
            }
            else
            {
                string path = dlg.SelectedPath;
                rtbServiceSourceDirectory.Text = path;
                if (!defaultProperties.ContainsKey("ServiceSourceDirectory"))
                {
                    defaultProperties.Add("ServiceSourceDirectory", path);
                }
                else
                    defaultProperties["ServiceSourceDirectory"] = path;

                saveDefaultProperties();
            }
        }




        #region // obsolete
        //private void btnCalculateGrIxStatsForMarkedImages_Click(object sender, EventArgs e)
        //{
        //    string strSourcePath = rtbServiceSourceDirectory.Text;
        //    if (!Directory.Exists(strSourcePath))
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the source directory.");
        //        return;
        //    }
        //    strSourcePath += (strSourcePath.Last() == '\\') ? ("") : ("\\");


        //    List<string> filesList = new List<string>(Directory.EnumerateFiles(strSourcePath, "*.jpg", SearchOption.TopDirectoryOnly));

        //    filesList.RemoveAll(
        //        strImgFilename => File.Exists(ConventionalTransitions.ImageGrIxMedianP5DataFileName(strImgFilename, imageMP5statsXMLdataFilesDirectory)));
        //    filesList.RemoveAll(
        //        strImgFilename => !File.Exists(ConventionalTransitions.SunDiskConditionFileName(strImgFilename, SunDiskConditionXMLdataFilesDirectory)));

        //    foreach (string fName in filesList)
        //    {
        //        AddImageToGrIxStatsCalculationQueue(fName);
        //    }

        //}
        #endregion // obsolete





        private void btnCalculateAllVarsStats_Click(object sender, EventArgs e)
        {
            string strImagesSourcePath = rtbServiceSourceDirectory.Text;
            if (!Directory.Exists(strImagesSourcePath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the source directory.");
                return;
            }
            strImagesSourcePath += (strImagesSourcePath.Last() == Path.DirectorySeparatorChar)
                ? ("")
                : (Path.DirectorySeparatorChar.ToString());

            ThreadSafeOperations.SetText(lblCalculationProgressPercentage, "getting files list", false);

            BackgroundWorker bgwEnumeratingFilesToProcess = new BackgroundWorker();
            bgwEnumeratingFilesToProcess.WorkerSupportsCancellation = true;
            bgwEnumeratingFilesToProcess.WorkerReportsProgress = true;
            bgwEnumeratingFilesToProcess.DoWork +=
                (bgwEnumeratingFilesToProcessSender, bgwEnumeratingFilesToProcessArgs) =>
                {
                    BackgroundWorker selfWorker = bgwEnumeratingFilesToProcessSender as BackgroundWorker;


                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }


                    List<string> filesList =
                        new List<string>(Directory.EnumerateFiles(strImagesSourcePath, "*.jpg",
                            (cbSearchImagesTopDirectoryOnly.Checked)
                                ? (SearchOption.TopDirectoryOnly)
                                : (SearchOption.AllDirectories)));

                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }

                    ThreadSafeOperations.SetText(lblCalculationProgressPercentage,
                        "filtering files list by already existing GrIxYRGB stats XML files", false);

                    List<string> lExistingYRGBstatsXMLdataFiles =
                        new List<string>(Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory,
                            ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern(), SearchOption.AllDirectories));

                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }

                    filesList.RemoveAll(
                        strImgFilename =>
                            lExistingYRGBstatsXMLdataFiles.Contains(
                                ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(strImgFilename,
                                    imageYRGBstatsXMLdataFilesDirectory)));


                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }


                    if (cbSunDiskConditionMarkedOnly.Checked)
                    {
                        ThreadSafeOperations.SetText(lblCalculationProgressPercentage,
                            "filtering files list by sun disk condition marks", false);

                        List<string> lExistingSunDiskInfoXMLdataFiles =
                            new List<string>(Directory.EnumerateFiles(SunDiskConditionXMLdataFilesDirectory,
                                ConventionalTransitions.SunDiskInfoFileNamesPattern(), SearchOption.AllDirectories));

                        if (selfWorker.CancellationPending)
                        {
                            return;
                        }

                        filesList.RemoveAll(
                            strImgFilename =>
                                lExistingSunDiskInfoXMLdataFiles.Contains(
                                    ConventionalTransitions.SunDiskConditionFileName(strImgFilename,
                                        SunDiskConditionXMLdataFilesDirectory)));
                    }

                    if (!filesList.Any())
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "There is no images that sutisfy specified settings. Processing will not be started.");
                        return;
                    }

                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }

                    int intFinalIndex = filesList.Count - 1;
                    int idx = 0;
                    int currPerc = 0;
                    int prevPerc = 0;
                    foreach (string fName in filesList)
                    {
                        ThreadSafeOperations.SetText(lblCalculationProgressPercentage,
                            "adding file to calculation queue" + Environment.NewLine + fName, false);
                        idx++;
                        currPerc = Convert.ToInt32((double)idx / (double)(intFinalIndex + 1));
                        if (currPerc > prevPerc)
                        {
                            prevPerc = currPerc;
                            selfWorker.ReportProgress(currPerc);
                        }

#if DEBUG
                        if (idx == 10)
                        {
                            bPermitProcessingStart = true;
                            AddImageToGrIxStatsCalculationQueue(fName);
                            break;
                        }
#endif

                        if (idx == intFinalIndex)
                        {
                            bPermitProcessingStart = true;
                        }
                        AddImageToGrIxStatsCalculationQueue(fName);
                    }

                    selfWorker.ReportProgress(0);
                };

            bgwEnumeratingFilesToProcess.ProgressChanged += (bgwEnumeratingFilesToProcessSender, bgwEnumeratingFilesToProcessArgs) =>
            {
                ThreadSafeOperations.UpdateProgressBar(prbEnumetaringFiles,
                    bgwEnumeratingFilesToProcessArgs.ProgressPercentage);
            };
            bgwEnumeratingFilesToProcess.RunWorkerCompleted += (bgwEnumeratingFilesToProcessSender, bgwEnumeratingFilesToProcessArgs) =>
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "finished enumerating files. Files to process: " + oqGrIxStatsCalculationQueue.Count);
            };

            bgwEnumeratingFilesToProcess.RunWorkerAsync();



            ThreadSafeOperations.SetText(lblCalculationProgressPercentage, "", false);
        }





        #region // obsolete
        //private void btnCalculateGrIxStatsForAllImages_Click(object sender, EventArgs e)
        //{
        //    string strSourcePath = rtbServiceSourceDirectory.Text;
        //    if (!Directory.Exists(strSourcePath))
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the source directory.");
        //        return;
        //    }
        //    strSourcePath += (strSourcePath.Last() == '\\') ? ("") : ("\\");

        //    List<string> filesList = new List<string>(Directory.EnumerateFiles(strSourcePath, "*.jpg", SearchOption.TopDirectoryOnly));

        //    filesList.RemoveAll(
        //        strImgFilename => File.Exists(ConventionalTransitions.ImageGrIxMedianP5DataFileName(strImgFilename, imageMP5statsXMLdataFilesDirectory)));

        //    foreach (string fName in filesList)
        //    {
        //        AddImageToGrIxStatsCalculationQueue(fName);
        //    }
        //}
        #endregion // obsolete




        private void btnSwitchModes_Click(object sender, EventArgs e)
        {
            bReviewMode = !bReviewMode;

            if (bReviewMode)
            {
                ThreadSafeOperations.SetButtonBackgroundColor(btnSwitchModes, Color.MistyRose);
                ThreadSafeOperations.ToggleButtonState(btnSwitchModes, true, "REVIEW mode", true);
            }
            else
            {
                ThreadSafeOperations.SetButtonBackgroundColor(btnSwitchModes, Color.Gainsboro);
                ThreadSafeOperations.ToggleButtonState(btnSwitchModes, true, "Standard mode", false);
            }

        }







        private void btnSelectSDCdataCSVfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            if (rtbSDCdataCSVfile.Text != "")
            {
                string dir = Path.GetDirectoryName(rtbSDCdataCSVfile.Text);
                if (Directory.Exists(dir))
                {
                    dlg.InitialDirectory = dir;
                }
            }

            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.OK)
            {
                return;
            }
            else
            {
                string selectedFilename = dlg.FileName;
                rtbSDCdataCSVfile.Text = selectedFilename;
                if (!defaultProperties.ContainsKey("SDCdataCSVfile"))
                {
                    defaultProperties.Add("SDCdataCSVfile", selectedFilename);
                }
                else
                    defaultProperties["SDCdataCSVfile"] = selectedFilename;

                saveDefaultProperties();
            }
        }




        private void btnSelectConcurrentDataXMLfilesDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = false;
            if (rtbConcurrentDataXMLfilesDirectory.Text != "")
            {
                if (Directory.Exists(rtbConcurrentDataXMLfilesDirectory.Text))
                {
                    dlg.SelectedPath = rtbConcurrentDataXMLfilesDirectory.Text;
                }
            }
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.OK)
            {
                return;
            }
            else
            {
                string path = dlg.SelectedPath;
                rtbConcurrentDataXMLfilesDirectory.Text = path;
                ConcurrentDataXMLfilesDirectory = path;
                if (!defaultProperties.ContainsKey("ConcurrentDataXMLfilesDirectory"))
                {
                    defaultProperties.Add("ConcurrentDataXMLfilesDirectory", path);
                }
                else
                    defaultProperties["ConcurrentDataXMLfilesDirectory"] = path;

                saveDefaultProperties();
            }
        }


        #endregion form behaviour




        #region sun condition ML collecting and predicting methods

        private void btnMLdataSourceDirectoryBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            if (rtbSourceDirectory.Text != "")
            {
                if (Directory.Exists(rtbMLdataSourceDirectory.Text))
                {
                    dlg.SelectedPath = rtbMLdataSourceDirectory.Text;
                }
            }
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.OK)
            {
                return;
            }
            else
            {
                string path = dlg.SelectedPath;
                rtbMLdataSourceDirectory.Text = path;
                if (!defaultProperties.ContainsKey("MLdataSourceDirectory"))
                {
                    defaultProperties.Add("MLdataSourceDirectory", path);
                }
                else
                    defaultProperties["MLdataSourceDirectory"] = path;

                saveDefaultProperties();
            }
        }



        private void btnBrowseMatFileToImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "Matlab mat files | *.mat";

            if (rtbMatFileToImportPath.Text != "")
            {
                string currDirectory = Path.GetDirectoryName(rtbMatFileToImportPath.Text);
                if (Directory.Exists(currDirectory))
                {
                    dlg.InitialDirectory = currDirectory;
                }
            }



            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.OK)
            {
                return;
            }
            else
            {
                string filePath = dlg.FileName;
                rtbMatFileToImportPath.Text = filePath;
            }
        }




        private void rtbMatFileToImportPath_TextChanged(object sender, EventArgs e)
        {
            string matFileName = (sender as RichTextBox).Text;
            if (!File.Exists(matFileName))
            {
                return;
            }
            theLogWindow = ServiceTools.LogAText(theLogWindow, "opening file: " + matFileName);
            ILNumerics.ILMatFile matFileToImport = new ILNumerics.ILMatFile(matFileName);
            string textToShow = "found following keys:" + Environment.NewLine;
            foreach (string keyName in matFileToImport.Keys)
            {
                textToShow += keyName + Environment.NewLine;
            }
            theLogWindow = ServiceTools.LogAText(theLogWindow, textToShow);
        }




        private double[][] modelInputs = null;
        private List<string> modelInputsHeaders = null;
        private SunDiskCondition[] modelOutputs = null;
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (rtbMLdataSourceDirectory.Text == "")
            {
                return;
            }
            string path = rtbMLdataSourceDirectory.Text;
            if (!Directory.Exists(path))
            {
                return;
            }

            ReadCollectedData(path);
        }


        private void btnReadContinue(List<Tuple<SkyImageIndexesStatsData, SunDiskConditionData>> lOverallData, string baseDir)
        {
            List<SunDiskConditionData> lSunDiskConditionData =
                    lOverallData.ConvertAll<SunDiskConditionData>(tpl => tpl.Item2);
            List<SkyImageIndexesStatsData> lStatsData =
                lOverallData.ConvertAll<SkyImageIndexesStatsData>(tpl => tpl.Item1);

            string fNameExistingStatsCollectionTemplate = baseDir +
                                                          ((baseDir.Last() == Path.DirectorySeparatorChar)
                                                              ? ("")
                                                              : (Path.DirectorySeparatorChar.ToString())) +
                                                          DateTime.Now.ToString("o").Replace(":", "-") +
                                                          "-CombinedStatsData-";
            ServiceTools.WriteObjectToXML(lSunDiskConditionData, fNameExistingStatsCollectionTemplate + "SunDiskConditionData.xml");
            ServiceTools.WriteObjectToXML(lStatsData, fNameExistingStatsCollectionTemplate + "AllStatsData.xml");

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "saved combined stats data file:" + fNameExistingStatsCollectionTemplate + "SunDiskConditionData.xml");
            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "saved combined sun disk condition data file:" + fNameExistingStatsCollectionTemplate + "AllStatsData.xml");


            modelInputsHeaders = new List<string>(lStatsData[0].DoubleVariablesNames());

            modelInputs =
                (lStatsData.ConvertAll<double[]>(
                    statsDatum => statsDatum.ToRawDoubleValuesEnumerable().ToArray<double>())).ToArray();

            //int[] outputs = table.Columns["G"].ToArray<int>();
            modelOutputs =
                (lSunDiskConditionData.ConvertAll(sdCondDatum => sdCondDatum.sunDiskCondition)).ToArray();

            theLogWindow = ServiceTools.LogAText(theLogWindow, "finished reading");
        }





        private void btnReadContinue(List<SkyImageIndexesStatsData> lStatsData, List<SunDiskConditionData> lSunDiskConditionData)
        {
            modelInputsHeaders = new List<string>(lStatsData[0].DoubleVariablesNames());

            modelInputs =
                (lStatsData.ConvertAll<double[]>(
                    statsDatum => statsDatum.ToRawDoubleValuesEnumerable().ToArray<double>())).ToArray();

            //int[] outputs = table.Columns["G"].ToArray<int>();
            modelOutputs =
                (lSunDiskConditionData.ConvertAll(sdCondDatum => sdCondDatum.sunDiskCondition)).ToArray();

            theLogWindow = ServiceTools.LogAText(theLogWindow, "finished reading");
        }





        private BackgroundWorker bgwPrecalculatedInformationRead;
        private void ReadCollectedData(string baseDir)
        {
            //List<Tuple<SkyImageIndexesStatsData, SunDiskConditionData>> retData =
            //    new List<Tuple<SkyImageIndexesStatsData, SunDiskConditionData>>();

            IEnumerable<string> fNamesExistingStatsCollections = Directory.EnumerateFiles(baseDir,
                "*CombinedStatsData*.xml", SearchOption.TopDirectoryOnly);
            if (fNamesExistingStatsCollections.Any())
            {
                IEnumerable<string> fNamesSunDiskConditionData = Directory.EnumerateFiles(baseDir, "*CombinedStatsData-SunDiskConditionData.xml", SearchOption.TopDirectoryOnly);
                IEnumerable<string> fNamesStatsData = Directory.EnumerateFiles(baseDir, "*CombinedStatsData-AllStatsData.xml", SearchOption.TopDirectoryOnly);
                List<SkyImageIndexesStatsData> lStatsData =
                    (List<SkyImageIndexesStatsData>)
                        ServiceTools.ReadObjectFromXML(fNamesStatsData.ElementAt(0),
                            typeof(List<SkyImageIndexesStatsData>));
                List<SunDiskConditionData> lSunDiskConditionData =
                    (List<SunDiskConditionData>)
                        ServiceTools.ReadObjectFromXML(fNamesSunDiskConditionData.ElementAt(0),
                            typeof(List<SunDiskConditionData>));

                btnReadContinue(lStatsData, lSunDiskConditionData);

                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "found previously combined stats data files: " + Environment.NewLine + fNamesStatsData.ElementAt(0) +
                    Environment.NewLine + fNamesSunDiskConditionData.ElementAt(0));

                return;
            }

            bgwPrecalculatedInformationRead = new BackgroundWorker();
            DoWorkEventHandler bgwPrecalculatedInformationRead_DoWorkHandler = delegate (object currBGWsender, DoWorkEventArgs args)
            {
                BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                object[] currBGWarguments = (object[])args.Argument;

                string currDir = (string)currBGWarguments[0];

                List<string> filesListSunConditionData =
                    new List<string>(Directory.EnumerateFiles(SunDiskConditionXMLdataFilesDirectory,
                        ConventionalTransitions.SunDiskConditionFileNamesPattern(), SearchOption.TopDirectoryOnly));
                List<string> filesListALLstatsData =
                    new List<string>(Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory,
                        ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern(), SearchOption.TopDirectoryOnly));

                int totalFilesCountToRead = filesListSunConditionData.Count + filesListALLstatsData.Count;
                int currProgressPerc = 0;
                int filesRead = 0;


                List<SunDiskConditionData> lImagesSunConditionData = new List<SunDiskConditionData>();
                foreach (string sunConditionDataFilename in filesListSunConditionData)
                {
                    SunDiskConditionData currSunDiskConditionData =
                        (SunDiskConditionData)
                            ServiceTools.ReadObjectFromXML(sunConditionDataFilename, typeof(SunDiskConditionData));
                    lImagesSunConditionData.Add(currSunDiskConditionData);

                    filesRead++;
                    double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                    if (progress - (double)currProgressPerc > 1.0d)
                    {
                        currProgressPerc = Convert.ToInt32(progress);
                        selfWorker.ReportProgress(currProgressPerc);
                    }
                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }
                }

                foreach (SunDiskConditionData sunConditionData in lImagesSunConditionData)
                {
                    sunConditionData.ShrinkFileName();
                }




                List<SkyImageIndexesStatsData> lImagesALLstatsData = new List<SkyImageIndexesStatsData>();
                foreach (string sALLstatsDataFilename in filesListALLstatsData)
                {
                    SkyImageIndexesStatsData currSkyImageIndexesStatsData =
                        (SkyImageIndexesStatsData)
                            ServiceTools.ReadObjectFromXML(sALLstatsDataFilename, typeof(SkyImageIndexesStatsData));
                    lImagesALLstatsData.Add(currSkyImageIndexesStatsData);

                    filesRead++;
                    double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                    if (progress - (double)currProgressPerc > 1.0d)
                    {
                        currProgressPerc = Convert.ToInt32(progress);
                        selfWorker.ReportProgress(currProgressPerc);
                    }
                    if (selfWorker.CancellationPending)
                    {
                        return;
                    }
                }

                foreach (SkyImageIndexesStatsData statsData in lImagesALLstatsData)
                {
                    statsData.ShrinkFileName();
                }






                List<Tuple<SkyImageIndexesStatsData, SunDiskConditionData>> lDataALLstatsTuples = lImagesALLstatsData
                    .ConvertAll<Tuple<SkyImageIndexesStatsData, SunDiskConditionData>>(
                        stats =>
                        {
                            int foundSunConditionDatumIdx =
                                lImagesSunConditionData.FindIndex(scdt => scdt.filename == stats.FileName);
                            if (foundSunConditionDatumIdx != -1)
                            {
                                return new Tuple<SkyImageIndexesStatsData, SunDiskConditionData>(stats,
                                    lImagesSunConditionData[foundSunConditionDatumIdx]);
                            }
                            else return null;
                        });

                lDataALLstatsTuples.RemoveAll(tpl => tpl == null);

                args.Result = new object[] { lDataALLstatsTuples };
            };
            bgwPrecalculatedInformationRead.DoWork += bgwPrecalculatedInformationRead_DoWorkHandler;
            RunWorkerCompletedEventHandler bgwPrecalculatedInformationRead_CompletedHandler =
                delegate (object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    BackgroundWorker selftWorker = currBGWCompletedSender as BackgroundWorker;
                    object[] currentBGWResults = (object[])args.Result;
                    List<Tuple<SkyImageIndexesStatsData, SunDiskConditionData>> retList =
                        currentBGWResults[0] as List<Tuple<SkyImageIndexesStatsData, SunDiskConditionData>>;

                    ThreadSafeOperations.UpdateProgressBar(prgBarMLprogress, 0);

                    btnReadContinue(retList, baseDir);
                };
            bgwPrecalculatedInformationRead.RunWorkerCompleted += bgwPrecalculatedInformationRead_CompletedHandler;
            bgwPrecalculatedInformationRead.WorkerSupportsCancellation = true;
            bgwPrecalculatedInformationRead.WorkerReportsProgress = true;
            ProgressChangedEventHandler bgwPrecalculatedInformationRead_ProgressChangedHandler =
                delegate (object currBGWProgressChangedSender, ProgressChangedEventArgs args)
                {
                    ThreadSafeOperations.UpdateProgressBar(prgBarMLprogress, args.ProgressPercentage);
                };
            bgwPrecalculatedInformationRead.ProgressChanged += bgwPrecalculatedInformationRead_ProgressChangedHandler;
            object[] bgwArgs = { baseDir };
            bgwPrecalculatedInformationRead.RunWorkerAsync(bgwArgs);

            theLogWindow = ServiceTools.LogAText(theLogWindow, "reading started");
            ThreadSafeOperations.ToggleButtonState(btnConvert, true, "CANCEL reading", true);
        }



        private void btnLearn_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SerializedDecisionTreePath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR: cant find SerializedDecisionTreePath path. Will not continue.");
                return;
            }

            if (modelInputs == null)
            {
                return;
            }

            if (modelOutputs == null)
            {
                return;
            }

            if (modelInputsHeaders == null)
            {
                return;
            }

            string treeFileName = "";
            IEnumerable<string> filenames = Directory.EnumerateFiles(SerializedDecisionTreePath, "*.dtr",
                SearchOption.TopDirectoryOnly);
            if (filenames.Any())
            {
                treeFileName = filenames.ElementAt(0);
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "found precalculated decision tree in file " + treeFileName);
            }

            IEnumerable<IEnumerable<int>> idxesTrainTestSets =
                MLhelper<SunDiskCondition>.createDataPartitionIndexes(modelOutputs, new[] { 0.6d, 0.4d });
            List<Tuple<double[], int>> ienumTplsInputs =
                modelInputs.Select((value, index) => new Tuple<double[], int>(value, index)).ToList();
            List<Tuple<SunDiskCondition, int>> ienumTplsOutputs =
                modelOutputs.Select((value, index) => new Tuple<SunDiskCondition, int>(value, index)).ToList();

            List<Tuple<double[], int>> ienumTplsInputsTrain = new List<Tuple<double[], int>>(ienumTplsInputs);
            ienumTplsInputsTrain.RemoveAll(tpl => !idxesTrainTestSets.ElementAt(0).Contains(tpl.Item2));
            double[][] inputsTrain = ienumTplsInputsTrain.ConvertAll(tpl => tpl.Item1).ToArray();

            List<Tuple<SunDiskCondition, int>> ienumTplsOutputsTrain = new List<Tuple<SunDiskCondition, int>>(ienumTplsOutputs);
            ienumTplsOutputsTrain.RemoveAll(tpl => !idxesTrainTestSets.ElementAt(0).Contains(tpl.Item2));
            SunDiskCondition[] outputsTrain = ienumTplsOutputsTrain.ConvertAll(tpl => tpl.Item1).ToArray();


            List<Tuple<double[], int>> ienumTplsInputsTest = new List<Tuple<double[], int>>(ienumTplsInputs);
            ienumTplsInputsTest.RemoveAll(tpl => !idxesTrainTestSets.ElementAt(1).Contains(tpl.Item2));
            double[][] inputsTest = ienumTplsInputsTest.ConvertAll(tpl => tpl.Item1).ToArray();

            List<Tuple<SunDiskCondition, int>> ienumTplsOutputsTest = new List<Tuple<SunDiskCondition, int>>(ienumTplsOutputs);
            ienumTplsOutputsTest.RemoveAll(tpl => !idxesTrainTestSets.ElementAt(1).Contains(tpl.Item2));
            SunDiskCondition[] outputsTest = ienumTplsOutputsTest.ConvertAll(tpl => tpl.Item1).ToArray();



            DecisionVariable[] vars =
                modelInputsHeaders.ConvertAll<DecisionVariable>(strVarName => DecisionVariable.Continuous(strVarName))
                    .ToArray();

            int[] modelOutputsTrainInt = outputsTrain.Apply(x => Convert.ToInt32(x));
            int[] modelOutputsTestInt = outputsTest.Apply(x => Convert.ToInt32(x));

            DecisionTree tree;
            bool haveToSaveTree = true;
            if (File.Exists(treeFileName))
            {
                tree = DecisionTree.Load(treeFileName);
                haveToSaveTree = false;
            }
            else
            {
                tree = new DecisionTree(vars, Enum.GetNames(typeof(SunDiskCondition)).Length);
                C45Learning teacher = new C45Learning(tree);
                double error = teacher.Run(inputsTrain, modelOutputsTrainInt);
                //theLogWindow = ServiceTools.LogAText(theLogWindow, "Estimated error: " + error.ToString("e"));
            }

            int[] testPredictedOutput = inputsTest.Apply(dArr => tree.Compute(dArr));

            decisionTreeView1.TreeSource = tree;

            ConfusionMatrix cMatr = new ConfusionMatrix(testPredictedOutput, modelOutputsTestInt);
            dgvPerformance.DataSource = new[] { cMatr };

            theLogWindow = ServiceTools.LogAText(theLogWindow, "out of sample accuracy: " + cMatr.Accuracy.ToString("e"));
            theLogWindow = ServiceTools.LogAText(theLogWindow, "out of sample error: " + (1.0d - cMatr.Accuracy).ToString("e"));


            if (treeFileName == "")
            {
                treeFileName = SerializedDecisionTreePath +
                               ((SerializedDecisionTreePath.Last() == Path.DirectorySeparatorChar)
                                   ? ("")
                                   : (Path.DirectorySeparatorChar.ToString())) + "DecisionTree-" +
                               DateTime.Now.ToString("o").Replace(":", "-") + ".dtr";
            }

            if (haveToSaveTree)
            {
                tree.Save(treeFileName);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Calculated decision tree saved to: " + treeFileName);
            }
        }

        #endregion sun condition ML collecting and predicting methods






        #region helper classes

        private enum ImageGrIxStatsCollectingState
        {
            Queued,
            Calculating,
            Finished,
            Error,
            Aborted
        }



        private class ImageStatsCollectingData
        {
            public string filename
            {
                get; set;
            }
            public ImageGrIxStatsCollectingState State
            {
                get; set;
            }
            public double GrIxMedianValue
            {
                get; set;
            }
            public double GrIxPerc5Value
            {
                get; set;
            }
        }



        private class SkyImageMP5andSDCdata
        {
            public SkyImageMedianPerc5Data mp5Data { get; set; }
            public SunDiskConditionData sdcData { get; set; }

            public SkyImageMP5andSDCdata()
            {
                mp5Data = null;
                sdcData = null;
            }



            public SkyImageMP5andSDCdata(SkyImageMedianPerc5Data mp5, SunDiskConditionData sdc)
            {
                mp5Data = mp5;
                sdcData = sdc;
            }


        }



        private class SkyImageGrIxYRGBstatsAndSDCdata
        {
            public SkyImageIndexesStatsData GrIxYRGBstatsData { get; set; }
            public SunDiskConditionData sdcData { get; set; }

            public SkyImageGrIxYRGBstatsAndSDCdata()
            {
                GrIxYRGBstatsData = null;
                sdcData = null;
            }


            public SkyImageGrIxYRGBstatsAndSDCdata(SkyImageIndexesStatsData stats, SunDiskConditionData sdc)
            {
                GrIxYRGBstatsData = stats;
                sdcData = sdc;
            }
        }



        private class MeteoObservationsImportedData
        {
            // DTstr;sunAlt;sdc
            public DateTime datetimeUTC { get; set; }
            public double SunAltitudeDegrees { get; set; }
            public SunDiskCondition sdc { get; set; }


            public MeteoObservationsImportedData()
            {

            }


            public MeteoObservationsImportedData(DateTime dt, double SunAltitudeDeg, SunDiskCondition sdcValue)
            {
                sdc = sdcValue;
                SunAltitudeDegrees = SunAltitudeDeg;
                datetimeUTC = dt;
            }
        }



        

        private class SkyImage_MP5_SDC_ConcurrentData
        {
            public SkyImageMedianPerc5Data mp5Data { get; set; }
            public SunDiskConditionData sdcData { get; set; }
            public ConcurrentData concurrentData { get; set; }

            public SkyImage_MP5_SDC_ConcurrentData()
            {
                mp5Data = null;
                sdcData = null;
                concurrentData = null;
            }



            public SkyImage_MP5_SDC_ConcurrentData(SkyImageMedianPerc5Data mp5, SunDiskConditionData sdc, ConcurrentData cncData)
            {
                mp5Data = mp5;
                sdcData = sdc;
                concurrentData = cncData;
            }


        }





        private class SkyImage_GrIxYRGBstats_SDC_ConcurrentData
        {
            public SkyImageIndexesStatsData statsData { get; set; }
            public SunDiskConditionData sdcData { get; set; }
            public ConcurrentData concurrentData { get; set; }

            public SkyImage_GrIxYRGBstats_SDC_ConcurrentData()
            {
                statsData = null;
                sdcData = null;
                concurrentData = null;
            }



            public SkyImage_GrIxYRGBstats_SDC_ConcurrentData(SkyImageIndexesStatsData statsDt, SunDiskConditionData sdc, ConcurrentData cncData)
            {
                statsData = statsDt;
                sdcData = sdc;
                concurrentData = cncData;
            }


        }





        #endregion helper classes




        #region SDC prediction


        private void btnPredictSDC_Click(object sender, EventArgs e)
        {
            if (!File.Exists(currImageFInfo.FullName))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Image file could not be found. Will not proceed");
                return;
            }

            currImageSunDiskConditionData = null;

            if (!File.Exists(NNconfigFile) || !File.Exists(NNtrainedParametersFile) || !File.Exists(NormMeansFile) || !File.Exists(NormRangeFile))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "couldn`t find at least one of pre-calculated NN parameters file. Will not proceed");
                return;
            }

            // read or calculate stats data for the image
            // trying to find stats XML file
            SkyImageIndexesStatsData currImageStatsData = null;
            ConcurrentData nearestConcurrentData = null;

            if (Directory.Exists(imageYRGBstatsXMLdataFilesDirectory))
            {
                List<string> foundXMLfiles = Directory.EnumerateFiles(imageYRGBstatsXMLdataFilesDirectory,
                    ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currImageFInfo.FullName, "", false),
                    SearchOption.AllDirectories).ToList();
                if (foundXMLfiles.Any())
                {
                    // возьмем первый попавшийся
                    currImageStatsData =
                        (SkyImageIndexesStatsData)
                            ServiceTools.ReadObjectFromXML(foundXMLfiles[0], typeof(SkyImageIndexesStatsData));
                }
            }


            #region calculate stats if needed

            if (currImageStatsData == null)
            {
                BackgroundWorker bgwImageStatsCalculation = new BackgroundWorker();
                bgwImageStatsCalculation.WorkerSupportsCancellation = false;
                bgwImageStatsCalculation.WorkerReportsProgress = true;

                #region bgwImageStatsCalculation implementation

                #region DoWork
                bgwImageStatsCalculation.DoWork += (bgwSender, bgwEvArgs) =>
                {
                    BackgroundWorker selfWorker = bgwSender as BackgroundWorker;
                    FileInfo bgwCurrImageFInfo = ((object[])bgwEvArgs.Argument)[0] as FileInfo;

                    Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
                    optionalParameters.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    optionalParameters.Add("Stopwatch", sw);
                    optionalParameters.Add("logFileName", errorLogFilename);


                    ImageStatsDataCalculationResult currImageProcessingResult =
                        ImageProcessing.CalculateImageStatsData(bgwCurrImageFInfo.FullName, optionalParameters);


                    bgwEvArgs.Result = new object[] { currImageProcessingResult };

                };

                #endregion DoWork


                #region ProgressChanged

                bgwImageStatsCalculation.ProgressChanged += (bgwProgressChangedSender, bgwProgressChangedArgs) =>
                {
                    ThreadSafeOperations.SetText(lblCalculationProgressPercentage,
                        "read progress: " + bgwProgressChangedArgs.ProgressPercentage, false);
                };

                #endregion ProgressChanged


                #region RunWorkerCompleted
                bgwImageStatsCalculation.RunWorkerCompleted += (bgwCompletedSender, bgwCompletedEvArgs) =>
                {
                    if (bgwCompletedEvArgs == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR obtaining stats for image. Will not proceed.");
                        return;
                    }

                    if (bgwCompletedEvArgs.Result == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR obtaining stats for image. Will not proceed.");
                        return;
                    }

                    ImageStatsDataCalculationResult currImageProcessingResult =
                        ((object[])bgwCompletedEvArgs.Result)[0] as ImageStatsDataCalculationResult;

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


                        string strImageGrIxMedianP5DataFileName =
                            ConventionalTransitions.ImageGrIxMedianP5DataFileName(currentFullFileName,
                                imageMP5statsXMLdataFilesDirectory);
                        ServiceTools.WriteObjectToXML(currImageProcessingResult.mp5Result,
                            strImageGrIxMedianP5DataFileName);

                        string strImageGrIxYRGBDataFileName =
                            ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currentFullFileName,
                                imageYRGBstatsXMLdataFilesDirectory);
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
                };
                #endregion RunWorkerCompleted



                #endregion bgwImageStatsCalculation implementation

                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "obtaining stats data for image " + currImageFInfo.FullName + Environment.NewLine +
                    "Please wait while the process finishes...");

                object[] bgwImageStatsCalculationArgs = new object[] { currImageFInfo };

                bgwImageStatsCalculation.RunWorkerAsync(bgwImageStatsCalculationArgs);

                while (bgwImageStatsCalculation.IsBusy)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
            }

            #endregion calculate stats if needed

            // теперь currImageStatsData должно быть прочитано или просчитано
            // осталось найти сопутствующие данные, вычислить склонение и азимут солнца
            // составить строку данных
            // и применить к ней модель

            #region search for concurrent data

            if (nearestConcurrentData == null)
            {
                BackgroundWorker bgwImageConcurrentDataSearch = new BackgroundWorker();
                bgwImageConcurrentDataSearch.WorkerSupportsCancellation = false;
                bgwImageConcurrentDataSearch.WorkerReportsProgress = true;


                #region DoWork

                bgwImageConcurrentDataSearch.DoWork += (bgwSender, bgwEvArgs) =>
                {
                    BackgroundWorker selfWorker = bgwSender as BackgroundWorker;
                    FileInfo bgwCurrImageFInfo = ((object[])bgwEvArgs.Argument)[0] as FileInfo;
                    object var2 = ((object[])bgwEvArgs.Argument)[1];
                    List<ConcurrentData> lBgwConcurrentDataAlreadyRead = null;
                    if (var2 != null)
                    {
                        lBgwConcurrentDataAlreadyRead = (List<ConcurrentData>)var2;
                    }

                    #region find closest concurrent data

                    List<ConcurrentData> lConcurrentData = null;
                    if (lBgwConcurrentDataAlreadyRead == null)
                    {

                        theLogWindow = ServiceTools.LogAText(theLogWindow, "started concurrent data reading");

                        List<Tuple<string, ConcurrentData>> lImagesConcurrentData =
                            new List<Tuple<string, ConcurrentData>>();

                        List<string> filesListConcurrentData =
                            Directory.EnumerateFiles(ConcurrentDataXMLfilesDirectory,
                                ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(),
                                SearchOption.AllDirectories)
                                .ToList();

                        int totalFilesCountToRead = filesListConcurrentData.Count;
                        int filesRead = 0;
                        int currProgressPerc = 0;
                        selfWorker.ReportProgress(0);
                        List<Dictionary<string, object>> lDictionariesConcurrentData =
                            new List<Dictionary<string, object>>();
                        foreach (string strConcDataXMLFile in filesListConcurrentData)
                        {
                            Dictionary<string, object> currDict = ServiceTools.ReadDictionaryFromXML(strConcDataXMLFile);
                            currDict.Add("XMLfileName", Path.GetFileName(strConcDataXMLFile));

                            lDictionariesConcurrentData.Add(currDict);

                            #region calculate and report progress

                            filesRead++;
                            double progress = 100.0d * (double)filesRead / (double)totalFilesCountToRead;
                            if (progress - (double)currProgressPerc > 1.0d)
                            {
                                currProgressPerc = Convert.ToInt32(progress);
                                selfWorker.ReportProgress(currProgressPerc);
                            }

                            if (selfWorker.CancellationPending)
                            {
                                return;
                            }

                            #endregion calculate and report progress
                        }



                        lDictionariesConcurrentData.RemoveAll(dict => dict == null);
                        lConcurrentData =
                            lDictionariesConcurrentData.ConvertAll<ConcurrentData>(dict =>
                            {
                                ConcurrentData retVal = null;
                                try
                                {
                                    retVal = new ConcurrentData(dict);
                                }
                                catch (Exception ex)
                                {
                                    string strError = "couldn`t parse XML file " + dict["XMLfileName"] + " : " +
                                                      Environment.NewLine + ex.Message;
                                    ServiceTools.logToTextFile(errorLogFilename, strError, true, true);
                                }
                                return retVal;
                            });
                        lConcurrentData.RemoveAll(val => val == null);
                    }
                    else
                    {
                        lConcurrentData = lBgwConcurrentDataAlreadyRead;
                    }


                    // map obtained concurrent data to images by its datetime
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "concurrent data mapping started");

                    string currImgFilename = bgwCurrImageFInfo.FullName;
                    currImgFilename = Path.GetFileNameWithoutExtension(currImgFilename);

                    string ptrn = @"(devID\d)";
                    Regex rgxp = new Regex(ptrn, RegexOptions.IgnoreCase);

                    string strCurrImgDT = rgxp.Replace(currImgFilename.Substring(4), "");
                    //2015-12-16T06-01-38
                    strCurrImgDT = strCurrImgDT.Substring(0, 11) + strCurrImgDT.Substring(11).Replace("-", ":");

                    DateTime currImgDT = DateTime.Parse(strCurrImgDT, null,
                        System.Globalization.DateTimeStyles.AdjustToUniversal);

                    nearestConcurrentData = lConcurrentData.Aggregate((cDt1, cDt2) =>
                    {
                        TimeSpan tspan1 = new TimeSpan(Math.Abs((cDt1.datetimeUTC - currImgDT).Ticks));
                        TimeSpan tspan2 = new TimeSpan(Math.Abs((cDt2.datetimeUTC - currImgDT).Ticks));
                        return ((tspan1 <= tspan2) ? (cDt1) : (cDt2));
                    });


                    if (new TimeSpan(Math.Abs((nearestConcurrentData.datetimeUTC - currImgDT).Ticks)) >=
                        TimeSpanForConcurrentDataMappingTolerance)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "couldn`t find close enough concurrent data file for image:" + Environment.NewLine +
                            bgwCurrImageFInfo.FullName + Environment.NewLine + "closest concurrent data file is:" +
                            Environment.NewLine + nearestConcurrentData.filename + Environment.NewLine +
                            "with date-time value " + nearestConcurrentData.datetimeUTC.ToString("o"));
                        nearestConcurrentData = null;
                    }


                    #endregion find closest concurrent data



                    bgwEvArgs.Result = new object[] { nearestConcurrentData, lConcurrentData };

                };

                #endregion



                #region ProgressChanged

                bgwImageConcurrentDataSearch.ProgressChanged += (bgwProgressChangedSender, bgwProgressChangedArgs) =>
                {
                    ThreadSafeOperations.SetText(lblCalculationProgressPercentage,
                        "read progress: " + bgwProgressChangedArgs.ProgressPercentage, false);
                };

                #endregion ProgressChanged



                #region RunWorkerCompleted
                bgwImageConcurrentDataSearch.RunWorkerCompleted += (bgwCompletedSender, bgwCompletedEvArgs) =>
                {
                    if (bgwCompletedEvArgs == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR searching concurrent data for image. Will not proceed.");
                        return;
                    }

                    if (bgwCompletedEvArgs.Result == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR searching concurrent data for image. Will not proceed.");
                        return;
                    }

                    nearestConcurrentData =
                        ((object[])bgwCompletedEvArgs.Result)[0] as ConcurrentData;

                    #region save already read concurrent data array for future use
                    object lConcurrentDataObtained =
                        ((object[])bgwCompletedEvArgs.Result)[1];
                    if (lConcurrentDataObtained != null)
                    {
                        lConcurrentDataAlreadyRead = lConcurrentDataObtained as List<ConcurrentData>;
                    }
                    #endregion save already read concurrent data array for future use


                    if (nearestConcurrentData == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "Unable to find concurrent data in tolerance gap " +
                            TimeSpanForConcurrentDataMappingTolerance.ToString() + Environment.NewLine +
                            "It is required now to use that data to determine sun positioning. So SDC estimation will not proceed.");
                        return;
                    }
                };
                #endregion RunWorkerCompleted



                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "searching for concurrent data for image " + currImageFInfo.FullName + Environment.NewLine +
                    "Please wait while the process finishes...");

                object[] bgwImageStatsCalculationArgs = new object[] { currImageFInfo, lConcurrentDataAlreadyRead };

                bgwImageConcurrentDataSearch.RunWorkerAsync(bgwImageStatsCalculationArgs);

                while (bgwImageConcurrentDataSearch.IsBusy)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
            }

            #endregion search for concurrent data




            string currImageALLstatsDataCSVWithConcurrentData = currImageStatsData.ToCSV() + "," +
                nearestConcurrentData.gps.SunZenithAzimuth().ElevationAngle.ToString().Replace(",", ".") + "," +
                nearestConcurrentData.gps.SunZenithAzimuth().Azimuth.ToString().Replace(",", ".");
            string csvHeader = currImageStatsData.CSVHeader() + ",SunElevationDeg,SunAzimuthDeg,sunDiskCondition";

            //csvStringALLstatsForFile = String.Join(Environment.NewLine, lImagesALLstatsDataCSVWithConcurrentData.ToArray<string>());

            //// write header
            //ServiceTools.logToTextFile(outPath + "ALLstatsWithSunPositions.csv",
            //    lDataALLstatsTuples[0].GrIxYRGBstatsData.CSVHeader() + ",SunElevationDeg,SunAzimuthDeg," + lDataALLstatsTuples[0].sdcData.CSVHeader() +
            //    Environment.NewLine, false, false);
            //// write content
            //ServiceTools.logToTextFile(outPath + "ALLstatsWithSunPositions.csv", csvStringALLstatsForFile, true, false);



            // А теперь применим модель
            // В модели, просчитанной в Матлабе, закодировано следующим образом:
            // Так, как закодировал в Матлабе
            //List<int> trueAnswersInt = trueAnswers.ConvertAll(sdc =>
            //{
            //    switch (sdc)
            //    {
            //        case SunDiskCondition.NoSun:
            //            return 4;
            //            break;
            //        case SunDiskCondition.Sun0:
            //            return 1;
            //            break;
            //        case SunDiskCondition.Sun1:
            //            return 2;
            //            break;
            //        case SunDiskCondition.Sun2:
            //            return 3;
            //            break;
            //        default:
            //            return 0;
            //    }
            //});

            List<string> lCalculatedData = new List<string>();
            lCalculatedData.Add(currImageALLstatsDataCSVWithConcurrentData);








            #region predict using LIST of data

            List<List<string>> csvFileContentStrings =
                lCalculatedData.ConvertAll(str => str.Split(',').ToList()).ToList();
            List<string> lCSVheader = csvHeader.Split(',').ToList();

            List<int> columnsToDelete =
                lCSVheader.Select((str, idx) => new Tuple<int, string>(idx, str))
                    .Where(tpl => tpl.Item2.ToLower().Contains("filename")).ToList().ConvertAll(tpl => tpl.Item1);
            List<List<string>> csvFileContentStringsFiltered = new List<List<string>>();
            foreach (List<string> listDataStrings in csvFileContentStrings)
            {
                csvFileContentStringsFiltered.Add(
                    listDataStrings.Where((str, idx) => !columnsToDelete.Contains(idx)).ToList());
            }


            #region true answers is not present yet

            //List<SunDiskCondition> trueAnswers =
            //    csvFileContentStringsFiltered.ConvertAll(
            //        lstr => (SunDiskCondition)Enum.Parse(typeof(SunDiskCondition), lstr.Last()));

            //// Так, как закодировал в Матлабе
            //List<int> trueAnswersInt = trueAnswers.ConvertAll(sdc =>
            //{
            //    switch (sdc)
            //    {
            //        case SunDiskCondition.NoSun:
            //            return 4;
            //            break;
            //        case SunDiskCondition.Sun0:
            //            return 1;
            //            break;
            //        case SunDiskCondition.Sun1:
            //            return 2;
            //            break;
            //        case SunDiskCondition.Sun2:
            //            return 3;
            //            break;
            //        default:
            //            return 0;
            //    }
            //});

            //List<List<string>> csvFileContentStringsFiltered_wo_sdc =
            //    csvFileContentStringsFiltered.ConvertAll(list => list.Where((val, idx) => idx < list.Count - 1).ToList());

            #endregion true answers is not present yet


            List<List<string>> csvFileContentStringsFiltered_wo_sdc = csvFileContentStringsFiltered;

            List<DenseVector> lDV_objects_features =
                csvFileContentStringsFiltered_wo_sdc.ConvertAll(
                    list =>
                        DenseVector.OfEnumerable(list.ConvertAll<double>(str => Convert.ToDouble(str.Replace(".", ",")))));


            DenseVector dvMeans = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormMeansFile, 0, ",")).Row(0);
            DenseVector dvRanges = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormRangeFile, 0, ",")).Row(0);

            lDV_objects_features = lDV_objects_features.ConvertAll(dv =>
            {
                DenseVector dvShifted = dv - dvMeans;
                DenseVector dvNormed = (DenseVector)dvShifted.PointwiseDivide(dvRanges);
                return dvNormed;
            });

            DenseMatrix dmObjectsFeatures = DenseMatrix.OfRowVectors(lDV_objects_features);

            DenseVector dvThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(NNtrainedParametersFile, 0, ",");
            List<int> NNlayersConfig =
                new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV(NNconfigFile, 0, ",")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));


            List<List<double>> lDecisionProbabilities = null;

            List<int> predictedSDC =
                NNclassificatorPredictor.NNpredict(dmObjectsFeatures, dvThetaValues, NNlayersConfig,
                    out lDecisionProbabilities).ToList();

            List<SunDiskCondition> predictedSDClist = predictedSDC.ConvertAll(sdcInt =>
            {
                switch (sdcInt)
                {
                    case 4:
                        return SunDiskCondition.NoSun;
                        break;
                    case 1:
                        return SunDiskCondition.Sun0;
                        break;
                    case 2:
                        return SunDiskCondition.Sun1;
                        break;
                    case 3:
                        return SunDiskCondition.Sun2;
                        break;
                    default:
                        return SunDiskCondition.Defect;
                }
            });

            #endregion

            string strToShow = "SDC values probabilities: " + Environment.NewLine +
                "| No Sun | Sun_0  | Sun_1  | Sun_2  |" + Environment.NewLine;
            foreach (List<double> lDecisionProbability in lDecisionProbabilities)
            {
                strToShow += "| " + lDecisionProbability[3].ToString("F4") +
                             " | " + lDecisionProbability[0].ToString("F4") +
                             " | " + lDecisionProbability[1].ToString("F4") +
                             " | " + lDecisionProbability[2].ToString("F4") + " |";
            }

            theLogWindow = ServiceTools.LogAText(theLogWindow, strToShow);

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                Environment.NewLine + Environment.NewLine + "=== detected SDC: " + predictedSDClist[0] + " ===");

            switch (predictedSDClist[0])
            {
                case SunDiskCondition.NoSun:
                    MarkSunCondition(SunDiskCondition.NoSun, false);
                    break;
                case SunDiskCondition.Sun0:
                    MarkSunCondition(SunDiskCondition.Sun0, false);
                    break;
                case SunDiskCondition.Sun1:
                    MarkSunCondition(SunDiskCondition.Sun1, false);
                    break;
                case SunDiskCondition.Sun2:
                    MarkSunCondition(SunDiskCondition.Sun2, false);
                    break;
                default:
                    MarkSunCondition(SunDiskCondition.Defect, false);
                    break;
            }

            DialogResult answer = MessageBox.Show("Correct detection? Should I save it?", "Save?",
                MessageBoxButtons.OKCancel);
            if (answer == DialogResult.OK)
            {
                SaveSunDiskConditionData();

                if (!alreadyMarkedSunDiskConditionFiles.Contains(currImageFInfo.FullName))
                {
                    alreadyMarkedSunDiskConditionFiles.Add(currImageFInfo.FullName);
                }
            }


        }


        #endregion
    }

}
