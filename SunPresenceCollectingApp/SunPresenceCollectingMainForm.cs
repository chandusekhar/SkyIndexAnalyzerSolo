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
        private Image<Bgr, byte> currImage = null;
        private BackgroundWorker bgwCurrImageProcessing = null;
        private SunDiskConditionData currImageSunDiskConditionData;
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private bool bShuffleImages = false;
        private bool bPermitRepeats = false;
        private int maxConcurrentBackgroundWorkers = 8;
        private List<string> alreadyMarkedSunDiskConditionFiles = new List<string>();
        private bool bReviewMode = false;
        private string SerializedDecisionTreePath = "";


        private ObservableCollection<BackgroundWorker> ocActiveStatsCalculationBgws = new ObservableCollection<BackgroundWorker>();



        public SunPresenceCollectingMainForm()
        {
            InitializeComponent();

            m_DelegateOpenFile = this.OpenFile;
            readDefaultProperties();
            oqGrIxStatsCalculationQueue.CollectionChanged += OqGrIxStatsCalculationQueue_CollectionChanged;
            ocActiveStatsCalculationBgws.CollectionChanged += OcActiveStatsCalculationBgws_CollectionChanged;

            Application.EnableVisualStyles();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            listBinding = new BindingList<ImageGrIxStatsCollectingData>(lGrIxStatsCalculation);
            dataGridView1.DataSource = listBinding;
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
            object[] currentBGWResults = (object[])e.Result;
            string currentFullFileName = (string)currentBGWResults[0];
            SkyImageMedianPerc5Data resStats = (SkyImageMedianPerc5Data)currentBGWResults[1];
            bool calcResult = (bool)currentBGWResults[2];
            SkyImageIndexesStatsData GrIxYRGBstats = (SkyImageIndexesStatsData)currentBGWResults[3];

            BackgroundWorker currBGW = sender as BackgroundWorker;
            ocActiveStatsCalculationBgws.Remove(currBGW);
            currBGW.Dispose();
            bgwCurrImageProcessing = null;


            if (calcResult)
            {
                string strImageGrIxMedianP5DataFileName =
                    ConventionalTransitions.ImageGrIxMedianP5DataFileName(currentFullFileName);
                ServiceTools.WriteObjectToXML(resStats, strImageGrIxMedianP5DataFileName);
                string strImageGrIxYRGBDataFileName =
                    ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currentFullFileName);
                ServiceTools.WriteObjectToXML(GrIxYRGBstats, strImageGrIxYRGBDataFileName);


                ImageGrIxStatsCollectingData foundDataObj =
                    lGrIxStatsCalculation.Find(obj => obj.filename == currentFullFileName);
                foundDataObj.State = ImageGrIxStatsCollectingState.Finished;
                foundDataObj.GrIxMedianValue = resStats.GrIxStatsMedian;
                foundDataObj.GrIxPerc5Value = resStats.GrIxStatsPerc5;

                UpdateDataGrid();

                dataGridView1.Refresh();
            }



            //ThreadSafeOperations.SetLoadingCircleState(circBgwProcessingImage, false, false,
            //    circBgwProcessingImage.Color);

            //UpdateMedianPerc5DataLabels(res);


        }




        private void BgwCurrImageProcessing_DoWork(object sender, DoWorkEventArgs e)
        {
            ThreadSafeOperations.SetLoadingCircleState(circBgwProcessingImage, true, true,
                circBgwProcessingImage.Color);

            object[] currBGWarguments = (object[])e.Argument;
            string currentFullFileName = (string)currBGWarguments[0];

            try
            {
                Tuple<double, double> tplMedianPerc5Data = ImageProcessing.CalculateMedianPerc5Values(currentFullFileName);
                SkyImageIndexesStatsData grixyrgbStatsData = ImageProcessing.CalculateIndexesStats(currentFullFileName);
                SkyImageMedianPerc5Data mp5dt = new SkyImageMedianPerc5Data(currentFullFileName, tplMedianPerc5Data.Item1, tplMedianPerc5Data.Item2);
                e.Result = new object[] { currentFullFileName, mp5dt, true, grixyrgbStatsData };
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "exception has been thrown: " + ex.Message + Environment.NewLine +
                            ServiceTools.CurrentCodeLineDescription());

                SkyImageMedianPerc5Data mp5dt = new SkyImageMedianPerc5Data(currentFullFileName, 0.0d, 0.0d);
                e.Result = new object[] { currentFullFileName, mp5dt, false, null };
            }
        }





        private async void OpenFile(string filename)
        {
            Task tsk = new Task(() =>
            {
                string currImageDir = Path.GetDirectoryName(filename);
                alreadyMarkedSunDiskConditionFiles = new List<string>(Directory.EnumerateFiles(currImageDir, "*.jpg", SearchOption.TopDirectoryOnly));
                alreadyMarkedSunDiskConditionFiles.RemoveAll(strFName => !File.Exists(ConventionalTransitions.SunDiskConditionFileName(strFName)));
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
        }





        private List<ImageGrIxStatsCollectingData> lGrIxStatsCalculation = new List<ImageGrIxStatsCollectingData>();
        private BindingList<ImageGrIxStatsCollectingData> listBinding;
        private ObservableQueue<string> oqGrIxStatsCalculationQueue = new ObservableQueue<string>();
        private void AddImageToGrIxStatsCalculationQueue(string imgFilename)
        {
            if (lGrIxStatsCalculation.FindIndex(statsDatum => statsDatum.filename == imgFilename) > -1)
            {
                return;
            }

            lGrIxStatsCalculation.Add(new ImageGrIxStatsCollectingData()
            {
                filename = imgFilename,
                GrIxMedianValue = 0.0d,
                GrIxPerc5Value = 0.0d,
                State = ImageGrIxStatsCollectingState.Queued
            });
            UpdateDataGrid();


            oqGrIxStatsCalculationQueue.Enqueue(imgFilename);

        }




        private void UpdateDataGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listBinding;
        }





        private void OqGrIxStatsCalculationQueue_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                return;
            }

            if (bgwCurrImageProcessing == null)
            {
                string filename = oqGrIxStatsCalculationQueue.Dequeue();
                StartBgwCalculatingGrIxStats(filename);
            }
            else
            {
                return;
            }
        }





        private void StartBgwCalculatingGrIxStats(string imgFilename)
        {
            ImageGrIxStatsCollectingData foundDataObj = lGrIxStatsCalculation.Find(obj => obj.filename == imgFilename);
            foundDataObj.State = ImageGrIxStatsCollectingState.Calculating;
            UpdateDataGrid();
            dataGridView1.Refresh();


            bgwCurrImageProcessing = new BackgroundWorker();
            bgwCurrImageProcessing.DoWork += BgwCurrImageProcessing_DoWork;
            bgwCurrImageProcessing.RunWorkerCompleted += BgwCurrImageProcessing_RunWorkerCompleted;
            object[] BGWorker2Args = new object[] { imgFilename };
            bgwCurrImageProcessing.RunWorkerAsync(BGWorker2Args);

            ocActiveStatsCalculationBgws.Add(bgwCurrImageProcessing);


            double perc = 100.0d *
                    (double)
                        lGrIxStatsCalculation.FindAll(stats => stats.State == ImageGrIxStatsCollectingState.Finished)
                            .Count / (double)lGrIxStatsCalculation.Count;

            lblCalculationProgressPercentage.Visible = true;
            ThreadSafeOperations.SetText(lblCalculationProgressPercentage, perc.ToString("F02") + "%", false);
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
                        strFName => File.Exists(ConventionalTransitions.ImageGrIxMedianP5DataFileName(strFName)));
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
                        strFName => File.Exists(ConventionalTransitions.ImageGrIxMedianP5DataFileName(strFName)));
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
                ConventionalTransitions.ImageGrIxMedianP5DataFileName(currImageFInfo);

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
                ConventionalTransitions.SunDiskConditionFileName(currImageFInfo);

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
                ConventionalTransitions.SunDiskConditionFileName(currImageFInfo);

            ServiceTools.WriteObjectToXML(currImageSunDiskConditionData, strSunDiskConditionFileName);
        }




        private void btnProperties_Click(object sender, EventArgs e)
        {
            PropertiesEditor propForm = new PropertiesEditor(defaultProperties, defaultPropertiesXMLfileName);
            propForm.FormClosed += new FormClosedEventHandler(PropertiesFormClosed);
            propForm.ShowDialog();
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




        private void MarkSunCondition(SunDiskCondition currCondition)
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

            SaveSunDiskConditionData();

            if (!alreadyMarkedSunDiskConditionFiles.Contains(currImageFInfo.FullName))
            {
                alreadyMarkedSunDiskConditionFiles.Add(currImageFInfo.FullName);
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



        public void PropertiesFormClosed(object sender, FormClosedEventArgs e)
        {
            readDefaultProperties();
        }




        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }




        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                         "\\settings\\SunPresenceCollectingApp-Settings.xml";
            if (!File.Exists(defaultPropertiesXMLfileName)) return;
            defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);

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



            if (defaultProperties.ContainsKey("CalculateGrIxStatsOnline"))
            {
                cbCalculateGrIxStatsOnline.Checked = (((string)defaultProperties["CalculateGrIxStatsOnline"]).ToLower() == "true");
            }
            else
            {
                cbCalculateGrIxStatsOnline.Checked = false;
            }



            if (defaultProperties.ContainsKey("ConversionSourceDirectory"))
            {
                rtbSourceDirectory.Text = ((string)defaultProperties["ConversionSourceDirectory"]);
            }
            else
            {
                rtbSourceDirectory.Text = "";
            }




            if (defaultProperties.ContainsKey("ConversionOutputDirectory"))
            {
                rtbOutputDirectory.Text = ((string)defaultProperties["ConversionOutputDirectory"]);
            }
            else
            {
                rtbOutputDirectory.Text = "";
            }



            if (defaultProperties.ContainsKey("ServiceSourceDirectory"))
            {
                rtbServiceSourceDirectory.Text = ((string)defaultProperties["ServiceSourceDirectory"]);
            }
            else
            {
                rtbServiceSourceDirectory.Text = "";
            }



            if (defaultProperties.ContainsKey("ShuffleImages"))
            {
                bShuffleImages = (((string)defaultProperties["ShuffleImages"]).ToLower() == "true");
            }
            else
            {
                defaultProperties.Add("ShuffleImages", false);
                saveDefaultProperties();
            }



            // maxConcurrentBackgroundWorkers
            if (defaultProperties.ContainsKey("maxConcurrentBackgroundWorkers"))
            {
                maxConcurrentBackgroundWorkers = Convert.ToInt32(defaultProperties["maxConcurrentBackgroundWorkers"]);
            }
            else
            {
                defaultProperties.Add("maxConcurrentBackgroundWorkers", 8);
                saveDefaultProperties();
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
                saveDefaultProperties();
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
                saveDefaultProperties();
            }
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
            outPath += (outPath.Last() == '\\') ? ("") : ("\\");
            if (!Directory.Exists(outPath))
            {
                ServiceTools.ExecMethodInSeparateThread(this,
                    () => { theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the output directory."); });
                return;
            }





            bgwConversion = new BackgroundWorker();
            bgwConversion.DoWork += BgwConversion_DoWork;
            bgwConversion.RunWorkerCompleted += BgwConversion_RunWorkerCompleted;
            bgwConversion.WorkerSupportsCancellation = true;
            bgwConversion.WorkerReportsProgress = true;
            bgwConversion.ProgressChanged += BgwConversion_ProgressChanged;
            object[] bgwArgs = { currDir, outPath };
            bgwConversion.RunWorkerAsync(bgwArgs);

            theLogWindow = ServiceTools.LogAText(theLogWindow, "conversion started");
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

            List<string> filesListMP5statsData =
                new List<string>(Directory.EnumerateFiles(currDir,
                    ConventionalTransitions.ImageGrIxMedianP5DataFileNamesPattern(), SearchOption.TopDirectoryOnly));
            List<string> filesListSunConditionData =
                new List<string>(Directory.EnumerateFiles(currDir,
                    ConventionalTransitions.SunDiskConditionFileNamesPattern(), SearchOption.TopDirectoryOnly));
            List<string> filesListALLstatsData =
                new List<string>(Directory.EnumerateFiles(currDir,
                    ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern(), SearchOption.TopDirectoryOnly));

            int totalFilesCountToRead = filesListMP5statsData.Count + filesListSunConditionData.Count + filesListALLstatsData.Count;
            int currProgressPerc = 0;
            int filesRead = 0;


            List<SkyImageMedianPerc5Data> lImagesMP5StatsData = new List<SkyImageMedianPerc5Data>();
            foreach (string strMP5filename in filesListMP5statsData)
            {
                SkyImageMedianPerc5Data currMP5stats =
                    (SkyImageMedianPerc5Data)
                        ServiceTools.ReadObjectFromXML(strMP5filename, typeof(SkyImageMedianPerc5Data));
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

            foreach (SkyImageMedianPerc5Data statsData in lImagesMP5StatsData)
            {
                statsData.ShrinkFileName();
            }



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



            List<Tuple<SkyImageMedianPerc5Data, SunDiskConditionData>> lDataTuples = lImagesMP5StatsData
                .ConvertAll<Tuple<SkyImageMedianPerc5Data, SunDiskConditionData>>(
                    stats =>
                    {
                        int foundSunConditionDatumIdx =
                            lImagesSunConditionData.FindIndex(scdt => scdt.filename == stats.FileName);
                        if (foundSunConditionDatumIdx != -1)
                        {
                            return new Tuple<SkyImageMedianPerc5Data, SunDiskConditionData>(stats,
                                lImagesSunConditionData[foundSunConditionDatumIdx]);
                        }
                        else return null;

                    });
            lDataTuples.RemoveAll(tpl => tpl == null);


            //List<string> lImagesStatsDataCSV =
            //  lDataTuples.ConvertAll<string>(
            //      tpl =>
            //          "" + Path.GetFileName(tpl.Item1.FileName) + "," +
            //          tpl.Item1.GrIxStatsMedian.ToString("F04").Replace(",", ".") + "," +
            //          tpl.Item1.GrIxStatsPerc5.ToString("F04").Replace(",", ".") + "," +
            //          tpl.Item2.sunDiskCondition);

            List<string> lImagesStatsDataCSV =
                lDataTuples.ConvertAll<string>(tpl => tpl.Item1.ToCSV() + "," + tpl.Item2.ToCSV());

            string csvStringForFile = String.Join(Environment.NewLine, lImagesStatsDataCSV.ToArray<string>());

            ServiceTools.logToTextFile(outPath + "GrIxMedianP5Stats.csv", lDataTuples[0].Item1.CSVHeader() + "," + lDataTuples[0].Item2.CSVHeader() + Environment.NewLine, false, false);
            ServiceTools.logToTextFile(outPath + "GrIxMedianP5Stats.csv", csvStringForFile, true, false);




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

            List<string> lImagesALLstatsDataCSV = lDataALLstatsTuples.ConvertAll<string>(tpl => tpl.Item1.ToCSV() + "," + tpl.Item2.ToCSV());

            string csvStringALLstatsForFile = String.Join(Environment.NewLine, lImagesALLstatsDataCSV.ToArray<string>());
            ServiceTools.logToTextFile(outPath + "ALLstats.csv", lDataALLstatsTuples[0].Item1.CSVHeader() + "," + lDataALLstatsTuples[0].Item2.CSVHeader() + Environment.NewLine, false, false);
            ServiceTools.logToTextFile(outPath + "ALLstats.csv", csvStringALLstatsForFile, true, false);
        }



        private void BgwConversion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.UpdateProgressBar(prbConversionProgress, 0);
            theLogWindow = ServiceTools.LogAText(theLogWindow, "conversion finished");
            ThreadSafeOperations.ToggleButtonState(btnConvert, true, "convert to CSV files (one for (M,P5) predictors and one for all statistical predictors)", true);
        }








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



        private void btnCalculateGrIxStatsForMarkedImages_Click(object sender, EventArgs e)
        {
            string strSourcePath = rtbServiceSourceDirectory.Text;
            if (!Directory.Exists(strSourcePath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the source directory.");
                return;
            }
            strSourcePath += (strSourcePath.Last() == '\\') ? ("") : ("\\");


            List<string> filesList = new List<string>(Directory.EnumerateFiles(strSourcePath, "*.jpg", SearchOption.TopDirectoryOnly));

            filesList.RemoveAll(
                strImgFilename => File.Exists(ConventionalTransitions.ImageGrIxMedianP5DataFileName(strImgFilename)));
            filesList.RemoveAll(
                strImgFilename => !File.Exists(ConventionalTransitions.SunDiskConditionFileName(strImgFilename)));

            foreach (string fName in filesList)
            {
                AddImageToGrIxStatsCalculationQueue(fName);
            }

        }





        private void btnCalculateAllVarsStats_Click(object sender, EventArgs e)
        {
            string strSourcePath = rtbServiceSourceDirectory.Text;
            if (!Directory.Exists(strSourcePath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the source directory.");
                return;
            }
            strSourcePath += (strSourcePath.Last() == '\\') ? ("") : ("\\");


            List<string> filesList =
                new List<string>(Directory.EnumerateFiles(strSourcePath, "*.jpg",
                    (cbSearchImagesTopDirectoryOnly.Checked)
                        ? (SearchOption.TopDirectoryOnly)
                        : (SearchOption.AllDirectories)));

            filesList.RemoveAll(
                strImgFilename => File.Exists(ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(strImgFilename)));
            filesList.RemoveAll(
                strImgFilename => !File.Exists(ConventionalTransitions.SunDiskConditionFileName(strImgFilename)));

            foreach (string fName in filesList)
            {
                AddImageToGrIxStatsCalculationQueue(fName);
            }
        }






        private void btnCalculateGrIxStatsForAllImages_Click(object sender, EventArgs e)
        {
            string strSourcePath = rtbServiceSourceDirectory.Text;
            if (!Directory.Exists(strSourcePath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn't find the source directory.");
                return;
            }
            strSourcePath += (strSourcePath.Last() == '\\') ? ("") : ("\\");

            List<string> filesList = new List<string>(Directory.EnumerateFiles(strSourcePath, "*.jpg", SearchOption.TopDirectoryOnly));

            filesList.RemoveAll(
                strImgFilename => File.Exists(ConventionalTransitions.ImageGrIxMedianP5DataFileName(strImgFilename)));

            foreach (string fName in filesList)
            {
                AddImageToGrIxStatsCalculationQueue(fName);
            }
        }




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

            string fNameExistingStatsCollectionTemplate = baseDir + ((baseDir.Last() == '\\') ? ("") : ("\\")) +
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
                            typeof (List<SkyImageIndexesStatsData>));
                List<SunDiskConditionData> lSunDiskConditionData =
                    (List<SunDiskConditionData>)
                        ServiceTools.ReadObjectFromXML(fNamesSunDiskConditionData.ElementAt(0),
                            typeof (List<SunDiskConditionData>));
                
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
                    new List<string>(Directory.EnumerateFiles(currDir,
                        ConventionalTransitions.SunDiskConditionFileNamesPattern(), SearchOption.TopDirectoryOnly));
                List<string> filesListALLstatsData =
                    new List<string>(Directory.EnumerateFiles(currDir,
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
                MLhelper<SunDiskCondition>.createDataPartitionIndexes(modelOutputs, new[] {0.6d, 0.4d});
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
            theLogWindow = ServiceTools.LogAText(theLogWindow, "out of sample error: " + (1.0d-cMatr.Accuracy).ToString("e"));


            if (treeFileName == "")
            {
                treeFileName = SerializedDecisionTreePath +
                                  ((SerializedDecisionTreePath.Last() == '\\') ? ("") : ("\\")) + "DecisionTree-" +
                                  DateTime.Now.ToString("o").Replace(":", "-") + ".dtr";
            }

            if (haveToSaveTree)
            {
                tree.Save(treeFileName);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Calculated decision tree saved to: " + treeFileName);
            }
        }




        




        private enum ImageGrIxStatsCollectingState
        {
            Queued,
            Calculating,
            Finished,
            Error
        }



        private class ImageGrIxStatsCollectingData
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

        
    }

}
