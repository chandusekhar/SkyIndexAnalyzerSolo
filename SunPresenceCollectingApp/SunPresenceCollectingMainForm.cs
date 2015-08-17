﻿using System;
using System.Collections.Generic;
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
using SkyImagesAnalyzerLibraries;

namespace SunPresenceCollectingApp
{
    public partial class SunPresenceCollectingMainForm : Form
    {
        private LogWindow theLogWindow = null;
        private delegate void DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;
        private string currImageFileName = "";
        private FileInfo currImageFInfo = null;
        private Image<Bgr, byte> currImage = null;
        private BackgroundWorker bgwCurrImageProcessing = null;
        private SunDiskConditionData currImageSunDiskConditionData;



        public SunPresenceCollectingMainForm()
        {
            InitializeComponent();

            m_DelegateOpenFile = this.OpenFile;
        }




        private void BgwCurrImageProcessing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] currentBGWResults = (object[])e.Result;
            string currentFullFileName = (string)currentBGWResults[0];
            SkyImageMedianPerc5Data res = (SkyImageMedianPerc5Data)currentBGWResults[1];


            string strImageGrIxMedianP5DataFileName =
                ConventionalTransitions.ImageGrIxMedianP5DataFileName(currentFullFileName);
            ServiceTools.WriteObjectToXML(res, strImageGrIxMedianP5DataFileName);


            BackgroundWorker currBGW = sender as BackgroundWorker;
            currBGW.Dispose();

            ThreadSafeOperations.SetLoadingCircleState(circBgwProcessingImage, false, false,
                circBgwProcessingImage.Color);

            UpdateMedianPerc5DataLabels(res);
        }




        private void BgwCurrImageProcessing_DoWork(object sender, DoWorkEventArgs e)
        {
            ThreadSafeOperations.SetLoadingCircleState(circBgwProcessingImage, true, true,
                circBgwProcessingImage.Color);

            object[] currBGWarguments = (object[])e.Argument;
            string currentFullFileName = (string)currBGWarguments[0];
            Tuple<double, double> tplMedianPerc5Data = ImageProcessing.CalculateMedianPerc5Values(currentFullFileName);
            SkyImageMedianPerc5Data mp5dt = new SkyImageMedianPerc5Data(currentFullFileName, tplMedianPerc5Data.Item1, tplMedianPerc5Data.Item2);
            e.Result = new object[] { currentFullFileName, mp5dt };
        }





        private void OpenFile(string filename)
        {
            currImageFileName = filename;
            currImageFInfo = new FileInfo(currImageFileName);
            currImage = new Image<Bgr, byte>(currImageFileName);

            ThreadSafeOperations.UpdatePictureBox(currImagePictureBox, currImage.Bitmap, true);



            LoadExistingSunDiskConditionData();

            if (!LoadExistingMedianPerc5Data())
            {
                bgwCurrImageProcessing = new BackgroundWorker();
                bgwCurrImageProcessing.DoWork += BgwCurrImageProcessing_DoWork;
                bgwCurrImageProcessing.RunWorkerCompleted += BgwCurrImageProcessing_RunWorkerCompleted;
                object[] BGWorker2Args = new object[] { filename };
                bgwCurrImageProcessing.RunWorkerAsync(BGWorker2Args);
            }
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

            if (bgwCurrImageProcessing != null)
            {
                if (bgwCurrImageProcessing.IsBusy)
                {
                    return;
                }
            }





            string currImageDir = Path.GetDirectoryName(currImageFileName);
            if (!Directory.Exists(currImageDir))
            {
                return;
            }

            List<string> filesList = new List<string>(Directory.EnumerateFiles(currImageDir, "*.jpg", SearchOption.TopDirectoryOnly));
            int currFileIdx = filesList.FindIndex(str => str == currImageFileName);
            if (currFileIdx < 0) // ничего не найдено
            {
                // возьмем первый
                OpenFile(filesList[0]);
            }
            else if (currFileIdx == filesList.Count-1)
            {
                return;
            }
            else
            {
                OpenFile(filesList[currFileIdx+1]);
            }


            
        }






        private void PrevPicture()
        {
            if (currImageFileName == "")
            {
                return;
            }

            if (bgwCurrImageProcessing != null)
            {
                if (bgwCurrImageProcessing.IsBusy)
                {
                    return;
                }
            }


            string currImageDir = Path.GetDirectoryName(currImageFileName);
            if (!Directory.Exists(currImageDir))
            {
                return;
            }

            List<string> filesList = new List<string>(Directory.EnumerateFiles(currImageDir, "*.jpg", SearchOption.TopDirectoryOnly));
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

            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkNoSun, Color.FromKnownColor(KnownColor.Control));
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun0, Color.FromKnownColor(KnownColor.Control));
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun1, Color.FromKnownColor(KnownColor.Control));
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun2, Color.FromKnownColor(KnownColor.Control));

            // посмотрим - есть ли уже существующие результаты.
            // Если есть, - отобразим на состоянии кнопок и на данных median-perc5
            if (File.Exists(strSunDiskConditionFileName))
            {
                object currImageSunDiskConditionObj =
                    ServiceTools.ReadObjectFromXML(strSunDiskConditionFileName, typeof (SunDiskConditionData));
                if (currImageSunDiskConditionObj != null)
                {
                    SunDiskConditionData currImageSunDiskCondition = (SunDiskConditionData) currImageSunDiskConditionObj;
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
        }




        private void MarkSunCondition(SunDiskCondition currCondition)
        {
            if (currImageFInfo == null)
            {
                return;
            }

            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkNoSun, Color.FromKnownColor(KnownColor.Control));
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun0, Color.FromKnownColor(KnownColor.Control));
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun1, Color.FromKnownColor(KnownColor.Control));
            ThreadSafeOperations.SetButtonBackgroundColor(btnMarkSun2, Color.FromKnownColor(KnownColor.Control));

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
                default:
                    break;
            }

            SaveSunDiskConditionData();
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
                Array FilesArray = (Array)e.Data.GetData(DataFormats.FileDrop);

                if (FilesArray != null)
                {
                    foreach (string FileName1 in FilesArray)
                    {
                        this.BeginInvoke(m_DelegateOpenFile, new Object[] { FileName1 });
                        this.Activate();
                        break;
                    }

                }
            }
            catch (Exception exc1)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Ошибка при обработке Drag&Drop: " + exc1.Message, true);
            }
        }






        
    }
    
}
