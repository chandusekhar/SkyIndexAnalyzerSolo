using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyIndexAnalyzerLibraries;

namespace CloudCamsDataAnalysis
{
    public partial class MainForm : Form
    {
        private string strOutputDirectory = "";
        public static LogWindow theLogWindow = null;
        private int imgID1Scale = 0;
        private int imgID2Scale = 0;
        private static Image<Bgr, byte> wholeImgID1 = null;
        private Image<Bgr, byte> partialImgID1 = null;
        private Rectangle partialImgID1Rect;
        private static Image<Bgr, byte> wholeImgID2 = null;
        private Image<Bgr, byte> partialImgID2 = null;
        private Rectangle partialImgID2Rect;
        private RoundData rdImgID1 = null;
        private RoundData rdImgID2 = null;
        private string imgID1filename = "";
        private string imgID2filename = "";

        private TextBox currActiveTextboxID1;
        private TextBox currActiveTextboxID2;

        private PointD pSID1 = new PointD();
        private PointD pC1ID1 = new PointD();
        private PointD pC2ID1 = new PointD();
        private PointD pSID2 = new PointD();
        private PointD pC1ID2 = new PointD();
        private PointD pC2ID2 = new PointD();

        private List<string> lProcessedFiles = new List<string>();


        public MainForm()
        {
            InitializeComponent();
        }



        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Image<Bgr, byte> wholeImgID1Copy = wholeImgID1.Copy();
            if (rdImgID1 != null)
            {
                RoundData rdImgID1_halfRadius = rdImgID1.Copy();
                rdImgID1_halfRadius.DRadius /= 2.0d;
                RoundData rdImgID1_CenterPoint = rdImgID1.Copy();
                rdImgID1_CenterPoint.DRadius =
                    Convert.ToInt32(Math.Sqrt(wholeImgID1.Size.Width * wholeImgID1.Size.Height) / 300.0d);

                wholeImgID1Copy.Draw(rdImgID1.CircleF(), new Bgr(Color.Magenta),
                    Convert.ToInt32(Math.Sqrt(wholeImgID1.Size.Width * wholeImgID1.Size.Height) / 300.0d));
                wholeImgID1Copy.Draw(rdImgID1_halfRadius.CircleF(), new Bgr(Color.Magenta),
                    Convert.ToInt32(Math.Sqrt(wholeImgID1.Size.Width * wholeImgID1.Size.Height) / 300.0d));
                wholeImgID1Copy.Draw(rdImgID1_CenterPoint.CircleF(), new Bgr(Color.Magenta), 0);
            }

            if (partialImgID1Rect.Size == wholeImgID1Copy.Size)
            {
                partialImgID1 = wholeImgID1Copy.Copy();
            }
            else
            {
                try
                {
                    //partialImgID1 = wholeImgID1.GetSubRect(focusRectID1);
                    partialImgID1 = wholeImgID1Copy.Copy(partialImgID1Rect);
                }
                catch (Exception)
                {
                    partialImgID1 = wholeImgID1Copy.Copy();
                }
            }
            wholeImgID1Copy.Dispose();



            if (partialImgID1 != null)
            {
                ThreadSafeOperations.UpdatePictureBox(pbImageID1, partialImgID1.Bitmap, true);
            }



            Image<Bgr, byte> wholeImgID2Copy = wholeImgID2.Copy();
            if (rdImgID2 != null)
            {
                RoundData rdImgID2_halfRadius = rdImgID2.Copy();
                rdImgID2_halfRadius.DRadius /= 2.0d;
                RoundData rdImgID2_CenterPoint = rdImgID2.Copy();
                rdImgID2_CenterPoint.DRadius =
                    Convert.ToInt32(Math.Sqrt(wholeImgID2.Size.Width * wholeImgID2.Size.Height) / 200.0d);

                wholeImgID2Copy.Draw(rdImgID2.CircleF(), new Bgr(Color.Magenta),
                    Convert.ToInt32(Math.Sqrt(wholeImgID2.Size.Width * wholeImgID2.Size.Height) / 300.0d));
                wholeImgID2Copy.Draw(rdImgID2_halfRadius.CircleF(), new Bgr(Color.Magenta),
                    Convert.ToInt32(Math.Sqrt(wholeImgID1.Size.Width * wholeImgID1.Size.Height) / 300.0d));
                wholeImgID2Copy.Draw(rdImgID2_CenterPoint.CircleF(), new Bgr(Color.Magenta), 0);
            }

            if (partialImgID2Rect.Size == wholeImgID2.Size)
            {
                partialImgID2 = wholeImgID2Copy.Copy();
            }
            else
            {
                try
                {
                    //partialImgID2 = wholeImgID2.GetSubRect(focusRectID2);
                    partialImgID2 = wholeImgID2Copy.Copy(partialImgID2Rect);
                }
                catch (Exception)
                {
                    partialImgID2 = wholeImgID2Copy.Copy();
                }

            }


            if (partialImgID2 != null)
            {
                ThreadSafeOperations.UpdatePictureBox(pbImageID2, partialImgID2.Bitmap, true);
            }
        }



        private void btnBrowseDirectoryForSunLocationProcessing_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dirDialog = new FolderBrowserDialog();
            dirDialog.ShowNewFolderButton = false;
            DialogResult res = dirDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                tbSunLocationProcessingDirectory.Text = dirDialog.SelectedPath + "\\";
            }
        }




        private void MainForm_Load(object sender, EventArgs e)
        {
            string generalSettingsFilename = Directory.GetCurrentDirectory() +
                                      "\\settings\\CloudCamsDataAnalysisAppSettings.xml";
            string savedDataFileName = Directory.GetCurrentDirectory() +
                                         "\\settings\\CloudCamsDataAnalysisAppSavedData.xml";


            Dictionary<string, object> generalSettings = ServiceTools.ReadDictionaryFromXML(generalSettingsFilename);
            string strSunLocationsAnalysisDirectory = generalSettings["SunLocationProcessingDirectory"] as string;
            if (strSunLocationsAnalysisDirectory != null)
            {
                tbSunLocationProcessingDirectory.Text = strSunLocationsAnalysisDirectory;
            }
            strOutputDirectory = generalSettings["OutputDirectory"] as string;
            if (strOutputDirectory == null)
            {
                strOutputDirectory = Directory.GetCurrentDirectory() + "\\output\\";
            }
        }




        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }






        private BackgroundWorker bgwFileCoupleProcessing = null;
        private void btnSunLocationProcessing_Click(object sender, EventArgs e)
        {
            if (bgwFileCoupleProcessing != null)
            {
                if (bgwFileCoupleProcessing.IsBusy)
                {
                    bgwFileCoupleProcessing.CancelAsync();
                }
            }
            else
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "start processing " + tbSunLocationProcessingDirectory.Text + Environment.NewLine +
                    "output directory:" + Environment.NewLine + strOutputDirectory);

                bgwFileCoupleProcessing = new BackgroundWorker();
                bgwFileCoupleProcessing.DoWork += bgwFileCoupleProcessingDoWork;
                bgwFileCoupleProcessing.RunWorkerCompleted += bgwFileCoupleProcessingCompletedHandler;
                bgwFileCoupleProcessing.WorkerSupportsCancellation = true;
                object[] BGWargs = new object[] { tbSunLocationProcessingDirectory.Text, strOutputDirectory };
                bgwFileCoupleProcessing.RunWorkerAsync(BGWargs);

                ThreadSafeOperations.ToggleButtonState(btnSunLocationProcessing, true, "STOP", true);
            }

        }




        void bgwFileCoupleProcessingDoWork(object currBGWsender, DoWorkEventArgs args)
        {
            BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
            object[] currBGWarguments = (object[])args.Argument;
            string strInputDirectory = (string)currBGWarguments[0];
            string strOutputDirectory = (string)currBGWarguments[1];
            //LogWindow theLogWindow = (LogWindow) currBGWarguments[2];

            if (!Directory.Exists(strOutputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(strOutputDirectory);
                }
                catch (Exception)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find or create output directory:" +
                                                                        Environment.NewLine +
                                                                        strOutputDirectory +
                                                                        ">>> files processing will not started");
                    return;
                }
            }

            strInputDirectory = strInputDirectory + ((strInputDirectory.EndsWith("\\")) ? ("") : ("\\"));
            if (!Directory.Exists(strInputDirectory))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find directory:" +
                                                                        Environment.NewLine +
                                                                        strInputDirectory +
                                                                        ">>> files processing will not started");
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(strInputDirectory);
            FileInfo[] files = dirInfo.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
            List<string> lFilesAlreadyProcessed = new List<string>();

            foreach (FileInfo currFileInfo in files)
            {
                if (selfWorker.CancellationPending)
                {
                    break;
                }

                if (lFilesAlreadyProcessed.Contains(currFileInfo.FullName))
                {
                    continue;
                }
                if (currFileInfo.Name.Contains("devID2"))
                {
                    continue;
                }
                //найдем пару
                //img-2014-09-18T08-49-..devID1.jpg
                FileInfo[] fInfoDevID2coupledFiles =
                    dirInfo.GetFiles("" + currFileInfo.Name.Substring(0, 21) + "??devID2.jpg",
                        SearchOption.TopDirectoryOnly);
                if (fInfoDevID2coupledFiles.Count() == 0)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find coupled file for:" +
                                                                        Environment.NewLine +
                                                                        currFileInfo.FullName);
                    continue;
                }
                else if (fInfoDevID2coupledFiles.Count() > 1)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "too many coupled files for:" +
                                                                        Environment.NewLine +
                                                                        currFileInfo.FullName);
                    continue;
                }

                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "starting process images couple:" + Environment.NewLine +
                    currFileInfo.Name + Environment.NewLine +
                    fInfoDevID2coupledFiles[0].Name);

                double currCoupleSunDevAngle = DevAngleImgID1FromImgID2BySunLocation(currFileInfo.FullName,
                    fInfoDevID2coupledFiles[0].FullName);

                lFilesAlreadyProcessed.Add(currFileInfo.FullName);
                lFilesAlreadyProcessed.Add(fInfoDevID2coupledFiles[0].FullName);

                theLogWindow = ServiceTools.LogAText(theLogWindow, "sun location dev angle for:" + Environment.NewLine
                                                                    + currFileInfo.FullName + Environment.NewLine
                                                                    + fInfoDevID2coupledFiles[0].FullName + Environment.NewLine
                                                                    + " = " + currCoupleSunDevAngle.ToString("e"));

                string text2write = "";
                string savedDataFilename = strOutputDirectory + "output-data.txt";
                text2write += DateTime.UtcNow.ToString("o") + "; " + currFileInfo.Name + "; " + fInfoDevID2coupledFiles[0].Name + "; " +
                              currCoupleSunDevAngle.ToString("e") + Environment.NewLine;
                ServiceTools.logToTextFile(savedDataFilename, text2write, true);

                File.Move(currFileInfo.FullName, strInputDirectory + "sun-location-processed\\" + currFileInfo.Name);
                File.Move(fInfoDevID2coupledFiles[0].FullName,
                    strInputDirectory + "sun-location-processed\\" + fInfoDevID2coupledFiles[0].Name);

                ServiceTools.FlushMemory();
            }
        }




        void bgwFileCoupleProcessingCompletedHandler(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
        {
            object[] currentBGWResults = (object[])args.Result;
            theLogWindow = ServiceTools.LogAText(theLogWindow, "================" + Environment.NewLine +
                                                               "    FINISHED    " + Environment.NewLine +
                                                               "================");
            ThreadSafeOperations.ToggleButtonState(btnSunLocationProcessing, true, "GO", true);
        }




        private double DevAngleImgID1FromImgID2BySunLocation(string filenameImgID1, string filenameImgID2)
        {
            Image<Bgr, Byte> imgBgrID1 = new Image<Bgr, byte>(filenameImgID1);
            wholeImgID1 = imgBgrID1.Copy();
            imgBgrID1 = ImageProcessing.ImageResizer(imgBgrID1, 800);
            Image<Bgr, Byte> imgBgrID2 = new Image<Bgr, byte>(filenameImgID2);
            wholeImgID2 = imgBgrID2.Copy();
            imgBgrID2 = ImageProcessing.ImageResizer(imgBgrID2, 800);

            RaisePaintEvent(null, null);

            //ThreadSafeOperations.UpdatePictureBox(pbImageID1, imgBgrID1.Bitmap, true);
            //ThreadSafeOperations.UpdatePictureBox(pbImageID2, imgBgrID2.Bitmap, true);

            string formulaString = "1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y";

            #region preparing data of the image ID1 to process
            ImageProcessing imgPid1 = new ImageProcessing(imgBgrID1, true);
            Image<Gray, Byte> maskImage = imgPid1.significantMaskImageCircled;
            Image<Gray, Byte> imageBlueChannelByte = imgBgrID1[0].Copy();
            Image<Gray, Byte> imageGreenChannelByte = imgBgrID1[1].Copy();
            Image<Gray, Byte> imageRedChannelByte = imgBgrID1[2].Copy();
            imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage);
            imageRedChannelByte = imageRedChannelByte.Mul(maskImage);
            imageGreenChannelByte = imageGreenChannelByte.Mul(maskImage);
            DenseMatrix dmRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
            DenseMatrix dmBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
            DenseMatrix dmGreenChannel = ImageProcessing.DenseMatrixFromImage(imageGreenChannelByte);

            DenseMatrix dmProcessingData = (DenseMatrix)imgPid1.eval(formulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
            DenseMatrix dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);
            dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMask);
            #endregion preparing data of the image ID1 to process

            #region preparing settings dictionary
            Dictionary<string, object> classificatorSettings = new Dictionary<string, object>();
            classificatorSettings.Add("JapanCloudSkySeparationValue", 0.1d);
            classificatorSettings.Add("GrIxSunDetectionMinimalSunAreaPartial", 0.0003d);
            classificatorSettings.Add("GrIxSunDetectionMaximalSunAreaPartial", 0.05d);
            classificatorSettings.Add("GrIxSunDetectionDesiredSunAreaPartial", 0.01d);
            classificatorSettings.Add("GrIxStdDevMarginValueDefiningTrueSkyArea", 0.65d);
            classificatorSettings.Add("GrIxDefaultSkyCloudMarginWithoutSun", 0.9d);
            classificatorSettings.Add("GrIxDefaultSkyCloudMarginWithSun", 0.1d);
            classificatorSettings.Add("GrIxAnalysisImageCircledCropFactor", 0.9d);
            classificatorSettings.Add("GrIxMinimalSunburnYvalue", 247.0d);
            classificatorSettings.Add("GrIxMinimalSunburnGrIxvalue", 0.98);
            classificatorSettings.Add("GrIxProcessingVerbosityLevel", 2);
            classificatorSettings.Add("GrIxSunDetectorArcedCropFactor", 0.95d);
            classificatorSettings.Add("GrIxSunDetectorConcurrentThreadsLimit", 4);
            #endregion

            SkyCloudClassification classificatorID1 = new SkyCloudClassification(imgBgrID1, classificatorSettings);
            classificatorID1.LogWindow = theLogWindow;

            string randomFileName = "processing-" + (new FileInfo(filenameImgID1)).Name;
            randomFileName = randomFileName.Replace(".jpg", "");
            RoundData sunRoundDataID1 = classificatorID1.DetectSunWithSerieOfArcs(imgPid1, dmProcessingData, strOutputDirectory, randomFileName);

            theLogWindow = ServiceTools.LogAText(theLogWindow, "devID1 camera snapshot sun location: " + Environment.NewLine + sunRoundDataID1.ToString());

            if (sunRoundDataID1.IsNull)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t detect sun disk for devID1 camera snapshot");
                return double.NaN;
            }
            PointPolar pPolar1 = new PointPolar(new PointD(sunRoundDataID1.DCenterX - imgPid1.imageRD.DCenterX, sunRoundDataID1.DCenterY - imgPid1.imageRD.DCenterY), true);

            #region preparing data of the image ID2 to process
            ImageProcessing imgPid2 = new ImageProcessing(imgBgrID2, true);
            maskImage = imgPid2.significantMaskImageCircled;
            imageBlueChannelByte = imgBgrID2[0].Copy();
            imageGreenChannelByte = imgBgrID2[1].Copy();
            imageRedChannelByte = imgBgrID2[2].Copy();
            imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage);
            imageRedChannelByte = imageRedChannelByte.Mul(maskImage);
            imageGreenChannelByte = imageGreenChannelByte.Mul(maskImage);
            dmRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
            dmBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
            dmGreenChannel = ImageProcessing.DenseMatrixFromImage(imageGreenChannelByte);

            dmProcessingData = (DenseMatrix)imgPid2.eval(formulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
            dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);
            dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMask);
            #endregion preparing data of the image ID2 to process

            SkyCloudClassification classificatorID2 = new SkyCloudClassification(imgBgrID2, classificatorSettings);
            classificatorID2.LogWindow = theLogWindow;

            randomFileName = "processing-" + (new FileInfo(filenameImgID2)).Name;
            randomFileName = randomFileName.Replace(".jpg", "");
            RoundData sunRoundDataID2 = classificatorID2.DetectSunWithSerieOfArcs(imgPid2, dmProcessingData, strOutputDirectory, randomFileName);

            theLogWindow = ServiceTools.LogAText(theLogWindow, "devID2 camera snapshot sun location: " + Environment.NewLine + sunRoundDataID2.ToString());

            if (sunRoundDataID2.IsNull)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t detect sun disk for devID2 camera snapshot");
                return double.NaN;
            }

            PointPolar pPolar2 = new PointPolar(new PointD(sunRoundDataID2.DCenterX - imgPid2.imageRD.DCenterX, sunRoundDataID2.DCenterY - imgPid2.imageRD.DCenterY), true);

            return (pPolar2.Phi - pPolar1.Phi);
        }



        //private void imgID1modifyScale(object sender, EventArgs e)
        //{
        //    trbImgID2ScaleValue.Value = trbImgID1ScaleValue.Value;
        //    imgID1Scale = trbImgID1ScaleValue.Value;
        //    imgID2Scale = trbImgID2ScaleValue.Value;
        //    CalculateSubImageWithScaleAndCenter();
        //    RaisePaintEvent(null, null);
        //}





        private void CalculateSubImageWithScaleAndCenter()
        {
            if (wholeImgID1 != null)
            {
                SizeD focusSizeID1 = new SizeD(wholeImgID1.Size);
                focusSizeID1 = focusSizeID1 / (Math.Pow(2, imgID1Scale));
                if ((imgID1Scale > 0) && (pbImageID1.Size.Width >= pbImageID1.Size.Height))
                {
                    focusSizeID1.Height = focusSizeID1.Width * (double)pbImageID1.Size.Height /
                                          (double)pbImageID1.Size.Width;
                }
                else if ((imgID1Scale > 0) && (pbImageID1.Size.Width < pbImageID1.Size.Height))
                {
                    focusSizeID1.Width = focusSizeID1.Height * (double)pbImageID1.Size.Width /
                                          (double)pbImageID1.Size.Height;
                }

                double focusShiftID1x = ((double)hScrollBarID1.Value) / ((double)hScrollBarID1.Maximum);
                focusShiftID1x *= (double)wholeImgID1.Size.Width;
                focusShiftID1x -= (double)focusSizeID1.Width * 0.5d;
                focusShiftID1x = (focusShiftID1x < 0.0d) ? (0.0d) : (focusShiftID1x);
                focusShiftID1x = (focusShiftID1x + focusSizeID1.Width > (double)wholeImgID1.Size.Width) ? ((double)wholeImgID1.Size.Width - focusSizeID1.Width) : (focusShiftID1x);

                double focusShiftID1y = ((double)vScrollBarID1.Value) / ((double)vScrollBarID1.Maximum);
                focusShiftID1y *= (double)wholeImgID1.Size.Height;
                focusShiftID1y -= (double)focusSizeID1.Height * 0.5d;
                focusShiftID1y = (focusShiftID1y < 0.0d) ? (0.0d) : (focusShiftID1y);
                focusShiftID1y = (focusShiftID1y + focusSizeID1.Height > (double)wholeImgID1.Size.Height) ? ((double)wholeImgID1.Size.Height - focusSizeID1.Height) : (focusShiftID1y);

                PointD focusShiftID1p = new PointD(focusShiftID1x, focusShiftID1y);
                //Rectangle focusRectID1 = new Rectangle(focusShiftID1p.Point(), focusSizeID1.ToSize());
                partialImgID1Rect = new Rectangle(focusShiftID1p.Point(), focusSizeID1.ToSize());
            }


            if (wholeImgID2 != null)
            {
                SizeD focusSizeID2 = new SizeD(wholeImgID2.Size);
                focusSizeID2 = focusSizeID2 / (Math.Pow(2, imgID2Scale));

                if ((imgID2Scale > 0) && (pbImageID2.Size.Width >= pbImageID2.Size.Height))
                {
                    focusSizeID2.Height = focusSizeID2.Width * (double)pbImageID2.Size.Height /
                                          (double)pbImageID2.Size.Width;
                }
                else if ((imgID2Scale > 0) && (pbImageID2.Size.Width < pbImageID2.Size.Height))
                {
                    focusSizeID2.Width = focusSizeID2.Height * (double)pbImageID2.Size.Width /
                                          (double)pbImageID2.Size.Height;
                }

                double focusShiftID2x = ((double)hScrollBarID2.Value) / ((double)hScrollBarID2.Maximum);
                focusShiftID2x *= (double)wholeImgID2.Size.Width;
                focusShiftID2x -= (double)focusSizeID2.Width * 0.5d;
                focusShiftID2x = (focusShiftID2x < 0.0d) ? (0.0d) : (focusShiftID2x);
                focusShiftID2x = (focusShiftID2x + focusSizeID2.Width > (double)wholeImgID2.Size.Width) ? ((double)wholeImgID1.Size.Width - focusSizeID2.Width) : (focusShiftID2x);

                double focusShiftID2y = ((double)vScrollBarID2.Value) / ((double)vScrollBarID2.Maximum);
                focusShiftID2y *= (double)wholeImgID2.Size.Height;
                focusShiftID2y -= (double)focusSizeID2.Height * 0.5d;
                focusShiftID2y = (focusShiftID2y < 0.0d) ? (0.0d) : (focusShiftID2y);
                focusShiftID2y = (focusShiftID2y + focusSizeID2.Height > (double)wholeImgID2.Size.Height) ? ((double)wholeImgID1.Size.Height - focusSizeID2.Height) : (focusShiftID2y);

                PointD focusShiftID2p = new PointD(focusShiftID2x, focusShiftID2y);
                //Rectangle focusRectID2 = new Rectangle(focusShiftID2p.Point(), focusSizeID2.ToSize());
                partialImgID2Rect = new Rectangle(focusShiftID2p.Point(), focusSizeID2.ToSize());
            }
        }



        private BackgroundWorker bgwFileCoupleCenterDetectionID1 = null;
        private BackgroundWorker bgwFileCoupleCenterDetectionID2 = null;
        private void DoOnFilesSelectedForManualPointsMarking(FileInfo fInfoID1, FileInfo fInfoID2)
        {
            ThreadSafeOperations.SetText(lblImageID1Comments, "---", false);
            ThreadSafeOperations.SetText(lblImageID2Comments, "---", false);

            imgID1filename = fInfoID1.FullName;
            imgID2filename = fInfoID2.FullName;

            rdImgID1 = null;
            rdImgID2 = null;
            partialImgID1 = null;
            partialImgID2 = null;
            ThreadSafeOperations.SetText(lblImageID1Comments, "", false);
            ThreadSafeOperations.SetText(lblImageID2Comments, "", false);

            wholeImgID1 = new Image<Bgr, byte>(fInfoID1.FullName);
            wholeImgID2 = new Image<Bgr, byte>(fInfoID2.FullName);

            CalculateSubImageWithScaleAndCenter();

            RaisePaintEvent(null, null);


            DoWorkEventHandler bgwFileCoupleCenterDetectionDoWorkHandler =
                delegate(object currBGWsender, DoWorkEventArgs args)
                {
                    object[] currBGWarguments = (object[])args.Argument;
                    Image<Bgr, byte> inputImg = (Image<Bgr, byte>)currBGWarguments[0];
                    int devID = (int)currBGWarguments[1];

                    Image<Bgr, byte> imgOperatingImage = inputImg.Copy();
                    ImageProcessing imgP = new ImageProcessing(imgOperatingImage, true);
                    RoundData rdRetData = imgP.imageRD;
                    imgP.Dispose();

                    args.Result = new object[] { rdRetData, devID };
                };

            RunWorkerCompletedEventHandler bgwFileCoupleCenterDetectionCompletedHandler =
                delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    RoundData rdRetData = (RoundData)currentBGWResults[0];
                    int retDevID = (int)currentBGWResults[1];
                    if (retDevID == 1)
                    {
                        rdImgID1 = rdRetData;
                        ThreadSafeOperations.SetText(lblImageID1Comments, rdImgID1.ToString(), false);
                        ThreadSafeOperations.SetLoadingCircleState(lcCalculatingImageID1RoundData, false, false,
                            lcCalculatingImageID1RoundData.Color);
                    }
                    else if (retDevID == 2)
                    {
                        rdImgID2 = rdRetData;
                        ThreadSafeOperations.SetText(lblImageID2Comments, rdImgID2.ToString(), false);
                        ThreadSafeOperations.SetLoadingCircleState(lcCalculatingImageID2RoundData, false, false,
                            lcCalculatingImageID2RoundData.Color);
                    }

                    //CalculateSubImageWithScaleAndCenter();
                    RaisePaintEvent(null, null);
                };


            bgwFileCoupleCenterDetectionID1 = new BackgroundWorker();
            bgwFileCoupleCenterDetectionID1.DoWork += bgwFileCoupleCenterDetectionDoWorkHandler;
            bgwFileCoupleCenterDetectionID1.RunWorkerCompleted += bgwFileCoupleCenterDetectionCompletedHandler;
            object[] BGWargsID1 = new object[] { wholeImgID1, 1 };
            bgwFileCoupleCenterDetectionID1.RunWorkerAsync(BGWargsID1);
            ThreadSafeOperations.SetLoadingCircleState(lcCalculatingImageID1RoundData, true, true,
                lcCalculatingImageID1RoundData.Color);

            bgwFileCoupleCenterDetectionID2 = new BackgroundWorker();
            bgwFileCoupleCenterDetectionID2.DoWork += bgwFileCoupleCenterDetectionDoWorkHandler;
            bgwFileCoupleCenterDetectionID2.RunWorkerCompleted += bgwFileCoupleCenterDetectionCompletedHandler;
            object[] BGWargsID2 = new object[] { wholeImgID2, 2 };
            bgwFileCoupleCenterDetectionID2.RunWorkerAsync(BGWargsID2);
            ThreadSafeOperations.SetLoadingCircleState(lcCalculatingImageID2RoundData, true, true,
                lcCalculatingImageID2RoundData.Color);


            currActiveTextboxID1 = null;
            currActiveTextboxID2 = null;
            SwitchCurrentActiveTextbox(1);
            SwitchCurrentActiveTextbox(2);

            pSID1 = new PointD();
            pC1ID1 = new PointD();
            pC2ID1 = new PointD();
            pSID2 = new PointD();
            pC1ID2 = new PointD();
            pC2ID2 = new PointD();

            ThreadSafeOperations.SetTextTB(tbSunLocationID1, "", false);
            ThreadSafeOperations.SetTextTB(tbSunLocationID2, "", false);
            ThreadSafeOperations.SetTextTB(tbControlPoint1id1, "", false);
            ThreadSafeOperations.SetTextTB(tbControlPoint1id2, "", false);
            ThreadSafeOperations.SetTextTB(tbControlPoint2id1, "", false);
            ThreadSafeOperations.SetTextTB(tbControlPoint2id2, "", false);
        }





        private void btnOpenFileID1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpenDialog = new OpenFileDialog();
            if (sender == btnOpenFileID1)
            {
                fileOpenDialog.Filter = "JPEG images for camera ID1 | *ID1.jpg";
            }
            else if (sender == btnOpenFileID2)
            {
                fileOpenDialog.Filter = "JPEG images for camera ID2 | *ID2.jpg";
            }

            fileOpenDialog.Multiselect = false;
            DialogResult res = fileOpenDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                FileInfo fInfoID1 = null;
                FileInfo fInfoID2 = null;

                if (sender == btnOpenFileID1)
                {
                    imgID1filename = fileOpenDialog.FileName;
                    fInfoID1 = new FileInfo(imgID1filename);
                    DirectoryInfo dirInfo = fInfoID1.Directory;

                    FileInfo[] fInfoDevID2coupledFiles =
                    dirInfo.GetFiles("" + fInfoID1.Name.Substring(0, 21) + "??devID2.jpg",
                        SearchOption.TopDirectoryOnly);
                    if (fInfoDevID2coupledFiles.Count() == 0)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find coupled file for:" +
                                                                            Environment.NewLine +
                                                                            fInfoID1.FullName);
                        return;
                    }
                    else if (fInfoDevID2coupledFiles.Count() > 1)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "too many coupled files for:" +
                                                                            Environment.NewLine +
                                                                            fInfoID1.FullName);
                        return;
                    }

                    fInfoID2 = fInfoDevID2coupledFiles[0];
                }
                else if (sender == btnOpenFileID2)
                {
                    imgID2filename = fileOpenDialog.FileName;
                    fInfoID2 = new FileInfo(imgID2filename);
                    DirectoryInfo dirInfo = fInfoID2.Directory;

                    FileInfo[] fInfoDevID1coupledFiles =
                    dirInfo.GetFiles("" + fInfoID2.Name.Substring(0, 21) + "??devID1.jpg",
                        SearchOption.TopDirectoryOnly);
                    if (fInfoDevID1coupledFiles.Count() == 0)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find coupled file for:" +
                                                                            Environment.NewLine +
                                                                            fInfoID2.FullName);
                        return;
                    }
                    else if (fInfoDevID1coupledFiles.Count() > 1)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "too many coupled files for:" +
                                                                            Environment.NewLine +
                                                                            fInfoID2.FullName);
                        return;
                    }

                    fInfoID1 = fInfoDevID1coupledFiles[0];
                }


                DoOnFilesSelectedForManualPointsMarking(fInfoID1, fInfoID2);
            }
        }







        private void ScrollBarsScroll(object sender, ScrollEventArgs e)
        {
            CalculateSubImageWithScaleAndCenter();
            RaisePaintEvent(null, null);
        }

        private void imgID1modifyScale(object sender, MouseEventArgs e)
        {
            trbImgID2ScaleValue.Value = trbImgID1ScaleValue.Value;
            imgID1Scale = trbImgID1ScaleValue.Value;
            imgID2Scale = trbImgID2ScaleValue.Value;
            CalculateSubImageWithScaleAndCenter();
            RaisePaintEvent(null, null);
        }



        private void imgID2modifyScale(object sender, MouseEventArgs e)
        {
            trbImgID1ScaleValue.Value = trbImgID2ScaleValue.Value;
            imgID1Scale = trbImgID1ScaleValue.Value;
            imgID2Scale = trbImgID2ScaleValue.Value;
            CalculateSubImageWithScaleAndCenter();
            RaisePaintEvent(null, null);
        }



        private void btnImgID1Center_Click(object sender, EventArgs e)
        {
            hScrollBarID1.Value = hScrollBarID1.Maximum / 2;
            vScrollBarID1.Value = vScrollBarID1.Maximum / 2;
            CalculateSubImageWithScaleAndCenter();
            RaisePaintEvent(null, null);
        }



        private void btnImgID2Center_Click(object sender, EventArgs e)
        {
            hScrollBarID2.Value = hScrollBarID2.Maximum / 2;
            vScrollBarID2.Value = vScrollBarID2.Maximum / 2;
            CalculateSubImageWithScaleAndCenter();
            RaisePaintEvent(null, null);
        }





        private PointD GetClickPointOnWholeImageFromPartialImage(object theSender, EventArgs eventArgs)
        {
            // пересчитаем точку из PictureBox в картинку, которая в нем
            PointD retPointD = new PointD();
            PointD mouseClickPoint = new PointD(((MouseEventArgs)eventArgs).Location);
            if (theSender == pbImageID1)
            {
                if (imgID1Scale == 0)
                {
                    Size currPresentedImageSize = pbImageID1.Image.Size;
                    retPointD.X = (mouseClickPoint.X / (double)currPresentedImageSize.Width) * (double)wholeImgID1.Width;
                    retPointD.Y = (mouseClickPoint.Y / (double)currPresentedImageSize.Height) * (double)wholeImgID1.Height;
                    return retPointD;
                }
                else
                {
                    Size currPresentedImageSize = pbImageID1.Image.Size;
                    retPointD.X = (mouseClickPoint.X / (double)currPresentedImageSize.Width) * (double)partialImgID1.Width;
                    retPointD.X += partialImgID1Rect.Left;
                    retPointD.Y = (mouseClickPoint.Y / (double)currPresentedImageSize.Height) * (double)partialImgID1.Height;
                    retPointD.Y += partialImgID1Rect.Top;
                    return retPointD;
                }
            }



            if (theSender == pbImageID2)
            {
                if (imgID2Scale == 0)
                {
                    Size currPresentedImageSize = pbImageID2.Image.Size;
                    retPointD.X = (mouseClickPoint.X / (double)currPresentedImageSize.Width) * (double)wholeImgID2.Width;
                    retPointD.Y = (mouseClickPoint.Y / (double)currPresentedImageSize.Height) * (double)wholeImgID2.Height;
                    return retPointD;
                }
                else
                {
                    Size currPresentedImageSize = pbImageID2.Image.Size;
                    retPointD.X = (mouseClickPoint.X / (double)currPresentedImageSize.Width) * (double)partialImgID2.Width;
                    retPointD.X += partialImgID2Rect.Left;
                    retPointD.Y = (mouseClickPoint.Y / (double)currPresentedImageSize.Height) * (double)partialImgID2.Height;
                    retPointD.Y += partialImgID2Rect.Top;
                    return retPointD;
                }
            }


            return new PointD();
        }



        private void pbImageID1_Click(object sender, EventArgs e)
        {
            // возьмем точку на отмасштабированной картинке
            // переведем ее координаты в точку на исходной картинке
            // запишем эти данные в очередное текстовое поле
            if ((wholeImgID1 == null) || (partialImgID1 == null))
            {
                return;
            }

            PointD p1 = GetClickPointOnWholeImageFromPartialImage(sender, e);

            if (currActiveTextboxID1 == tbSunLocationID1) pSID1 = p1;
            else if (currActiveTextboxID1 == tbControlPoint1id1) pC1ID1 = p1;
            else if (currActiveTextboxID1 == tbControlPoint2id1) pC2ID1 = p1;


            ThreadSafeOperations.SetTextTB(currActiveTextboxID1, p1.ToString(), false);


            SwitchCurrentActiveTextbox(1);
        }



        private void pbImageID2_Click(object sender, EventArgs e)
        {
            if ((wholeImgID2 == null) || (partialImgID2 == null))
            {
                return;
            }

            PointD p1 = GetClickPointOnWholeImageFromPartialImage(sender, e);

            if (currActiveTextboxID2 == tbSunLocationID2) pSID2 = p1;
            else if (currActiveTextboxID2 == tbControlPoint1id2) pC1ID2 = p1;
            else if (currActiveTextboxID2 == tbControlPoint2id2) pC2ID2 = p1;

            ThreadSafeOperations.SetTextTB(currActiveTextboxID2, p1.ToString(), false);

            SwitchCurrentActiveTextbox(2);
        }



        private void SwitchCurrentActiveTextbox(int devID = 1)
        {
            if (devID == 1)
            {

                tbSunLocationID1.BackColor = SystemColors.Window;
                tbControlPoint1id1.BackColor = SystemColors.Window;
                tbControlPoint2id1.BackColor = SystemColors.Window;

                if ((currActiveTextboxID1 == null) || (currActiveTextboxID1 == tbControlPoint2id1))
                {
                    currActiveTextboxID1 = tbSunLocationID1;
                }
                else if (currActiveTextboxID1 == tbSunLocationID1)
                {
                    currActiveTextboxID1 = tbControlPoint1id1;
                }
                else if (currActiveTextboxID1 == tbControlPoint1id1)
                {
                    currActiveTextboxID1 = tbControlPoint2id1;
                }
                currActiveTextboxID1.BackColor = Color.LightSkyBlue;
            }
            else if (devID == 2)
            {

                tbSunLocationID2.BackColor = SystemColors.Window;
                tbControlPoint1id2.BackColor = SystemColors.Window;
                tbControlPoint2id2.BackColor = SystemColors.Window;

                if ((currActiveTextboxID2 == null) || (currActiveTextboxID2 == tbControlPoint2id2))
                {
                    currActiveTextboxID2 = tbSunLocationID2;
                }
                else if (currActiveTextboxID2 == tbSunLocationID2)
                {
                    currActiveTextboxID2 = tbControlPoint1id2;
                }
                else if (currActiveTextboxID2 == tbControlPoint1id2)
                {
                    currActiveTextboxID2 = tbControlPoint2id2;
                }

                currActiveTextboxID2.BackColor = Color.LightSkyBlue;
            }
        }





        private void lblControlPoint1id1Title_Click(object sender, EventArgs e)
        {
            currActiveTextboxID1 = tbSunLocationID1;
            SwitchCurrentActiveTextbox(1);
        }

        private void lblControlPoint2id1Title_Click(object sender, EventArgs e)
        {
            currActiveTextboxID1 = tbControlPoint1id1;
            SwitchCurrentActiveTextbox(1);
        }

        private void lblControlPoint1id2Title_Click(object sender, EventArgs e)
        {
            currActiveTextboxID2 = tbSunLocationID2;
            SwitchCurrentActiveTextbox(2);
        }

        private void lblControlPoint2id2Title_Click(object sender, EventArgs e)
        {
            currActiveTextboxID2 = tbControlPoint1id2;
            SwitchCurrentActiveTextbox(2);
        }







        private void btnStoreStats_Click(object sender, EventArgs e)
        {
            FileInfo currProcessingFileID1FInfo = new FileInfo(imgID1filename);
            FileInfo currProcessingFileID2FInfo = new FileInfo(imgID2filename);

            string pointsLocationFilename = strOutputDirectory +
                                                 currProcessingFileID1FInfo.Name.Replace("devID1", "").Replace(".jpg", "") +
                                                 "points.txt";
            string str2write = "";
            str2write += imgID1filename + Environment.NewLine;
            str2write += imgID2filename + Environment.NewLine;
            str2write += "image contours (img1):" + Environment.NewLine + rdImgID1.ToString() + Environment.NewLine;
            str2write += "image contours (img2):" + Environment.NewLine + rdImgID2.ToString() + Environment.NewLine;
            str2write += "sun location (img1): " + Environment.NewLine + tbSunLocationID1.Text + Environment.NewLine;
            str2write += "sun location (img2): " + Environment.NewLine + tbSunLocationID2.Text + Environment.NewLine;
            str2write += "control point 1 (img1): " + Environment.NewLine + tbControlPoint1id1.Text + Environment.NewLine;
            str2write += "control point 1 (img2): " + Environment.NewLine + tbControlPoint1id2.Text + Environment.NewLine;
            str2write += "control point 2 (img1): " + Environment.NewLine + tbControlPoint2id1.Text + Environment.NewLine;
            str2write += "control point 2 (img2): " + Environment.NewLine + tbControlPoint2id2.Text + Environment.NewLine;
            ServiceTools.logToTextFile(pointsLocationFilename, str2write, false);



            string statsFilename = strOutputDirectory + "ImagesPointsStats.txt";
            str2write = currProcessingFileID1FInfo.Name + "; ";
            str2write += currProcessingFileID2FInfo.Name + "; ";
            str2write += rdImgID1.DCenterX.ToString("e") + "; " + rdImgID1.DCenterY.ToString("e") + "; " + rdImgID1.DRadius.ToString("e") + "; "; str2write += rdImgID1.DCenterX.ToString("e") + "; " + rdImgID1.DCenterY.ToString("e") + "; " + rdImgID1.DRadius.ToString("e") + "; ";
            str2write += rdImgID2.DCenterX.ToString("e") + "; " + rdImgID2.DCenterY.ToString("e") + "; " + rdImgID2.DRadius.ToString("e") + "; "; str2write += rdImgID1.DCenterX.ToString("e") + "; " + rdImgID1.DCenterY.ToString("e") + "; " + rdImgID1.DRadius.ToString("e") + "; ";
            str2write += pSID1.X.ToString("e") + "; " + pSID1.Y.ToString("e") + "; ";
            str2write += pSID2.X.ToString("e") + "; " + pSID2.Y.ToString("e") + "; ";
            str2write += pC1ID1.X.ToString("e") + "; " + pC1ID1.Y.ToString("e") + "; ";
            str2write += pC1ID2.X.ToString("e") + "; " + pC1ID2.Y.ToString("e") + "; ";
            str2write += pC2ID1.X.ToString("e") + "; " + pC2ID1.Y.ToString("e") + "; ";
            str2write += pC2ID2.X.ToString("e") + "; " + pC2ID2.Y.ToString("e") + "; " + Environment.NewLine;
            ServiceTools.logToTextFile(statsFilename, str2write, true);

            lProcessedFiles.Add(currProcessingFileID1FInfo.FullName);
            lProcessedFiles.Add(currProcessingFileID2FInfo.FullName);



            DialogResult res = MessageBox.Show("Next couple?", "decide please", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                SelectNewFileInDirectory(currProcessingFileID1FInfo);
            }
        }



        private void SelectNewFileInDirectory(FileInfo prevFileInfo)
        {
            string strInputDirectory = prevFileInfo.DirectoryName;

            if (!Directory.Exists(strOutputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(strOutputDirectory);
                }
                catch (Exception)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find or create output directory:" +
                                                                        Environment.NewLine +
                                                                        strOutputDirectory +
                                                                        ">>> files processing will not started");
                    return;
                }
            }

            strInputDirectory = strInputDirectory + ((strInputDirectory.EndsWith("\\")) ? ("") : ("\\"));
            if (!Directory.Exists(strInputDirectory))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find directory:" +
                                                                        Environment.NewLine +
                                                                        strInputDirectory +
                                                                        ">>> files processing will not started");
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(strInputDirectory);
            FileInfo[] files = dirInfo.GetFiles("*devID1.jpg", SearchOption.TopDirectoryOnly);

            foreach (FileInfo currFileInfo in files)
            {
                if (lProcessedFiles.Contains(currFileInfo.FullName))
                {
                    continue;
                }

                //найдем пару
                //img-2014-09-18T08-49-..devID1.jpg
                FileInfo[] fInfoDevID2coupledFiles =
                    dirInfo.GetFiles("" + currFileInfo.Name.Substring(0, 21) + "??devID2.jpg",
                        SearchOption.TopDirectoryOnly);
                if (fInfoDevID2coupledFiles.Count() == 0)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find coupled file for:" +
                                                                        Environment.NewLine +
                                                                        currFileInfo.FullName);
                    continue;
                }
                else if (fInfoDevID2coupledFiles.Count() > 1)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "too many coupled files for:" +
                                                                        Environment.NewLine +
                                                                        currFileInfo.FullName);
                    continue;
                }

                DoOnFilesSelectedForManualPointsMarking(currFileInfo, fInfoDevID2coupledFiles[0]);

                break;
            }
        }




        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<object> lDataToWrite = lProcessedFiles.ConvertAll<object>(str => (object)str);
            if (lProcessedFiles.Count() > 0)
            {
                string processedFilesListFilename = strOutputDirectory + "ProcessedFiles.xml";
                ServiceTools.WriteListToXml(lDataToWrite, processedFilesListFilename, true);
            }
        }

        private void tbSunLocationID1_MouseClick(object sender, MouseEventArgs e)
        {
            currActiveTextboxID1 = null;
            SwitchCurrentActiveTextbox(1);
        }

        private void tbSunLocationID2_MouseClick(object sender, MouseEventArgs e)
        {
            currActiveTextboxID2 = null;
            SwitchCurrentActiveTextbox(2);
        }



        private void btnImg1DetectSun_Click(object sender, EventArgs e)
        {
            DoWorkEventHandler bgwFindSunLocationDoWork = delegate(object currBGWsender, DoWorkEventArgs args)
                {
                    object[] currBGWarguments = (object[])args.Argument;
                    Image<Bgr, byte> inImageToProcess = (Image<Bgr, byte>)currBGWarguments[0];
                    string inImgFilename = (string)currBGWarguments[1];
                    string outputDirectory = (string)currBGWarguments[2];

                    Image<Bgr, Byte> imgBgrID1 = inImageToProcess.Copy();
                    imgBgrID1 = ImageProcessing.ImageResizer(imgBgrID1, 800);

                    string formulaString = "1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y";

                    #region preparing data of the image to process
                    ImageProcessing imgPid1 = new ImageProcessing(imgBgrID1, true);
                    Image<Gray, Byte> maskImage = imgPid1.significantMaskImageCircled;
                    Image<Gray, Byte> imageBlueChannelByte = imgBgrID1[0].Copy();
                    Image<Gray, Byte> imageGreenChannelByte = imgBgrID1[1].Copy();
                    Image<Gray, Byte> imageRedChannelByte = imgBgrID1[2].Copy();
                    imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage);
                    imageRedChannelByte = imageRedChannelByte.Mul(maskImage);
                    imageGreenChannelByte = imageGreenChannelByte.Mul(maskImage);
                    DenseMatrix dmRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
                    DenseMatrix dmBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
                    DenseMatrix dmGreenChannel = ImageProcessing.DenseMatrixFromImage(imageGreenChannelByte);

                    DenseMatrix dmProcessingData = (DenseMatrix)imgPid1.eval(formulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
                    DenseMatrix dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);
                    dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMask);
                    #endregion preparing data of the image to process

                    #region preparing settings dictionary
                    Dictionary<string, object> classificatorSettings = new Dictionary<string, object>();
                    classificatorSettings.Add("JapanCloudSkySeparationValue", 0.1d);
                    classificatorSettings.Add("GrIxSunDetectionMinimalSunAreaPartial", 0.0003d);
                    classificatorSettings.Add("GrIxSunDetectionMaximalSunAreaPartial", 0.05d);
                    classificatorSettings.Add("GrIxSunDetectionDesiredSunAreaPartial", 0.01d);
                    classificatorSettings.Add("GrIxStdDevMarginValueDefiningTrueSkyArea", 0.65d);
                    classificatorSettings.Add("GrIxDefaultSkyCloudMarginWithoutSun", 0.9d);
                    classificatorSettings.Add("GrIxDefaultSkyCloudMarginWithSun", 0.1d);
                    classificatorSettings.Add("GrIxAnalysisImageCircledCropFactor", 0.9d);
                    classificatorSettings.Add("GrIxMinimalSunburnYvalue", 247.0d);
                    classificatorSettings.Add("GrIxMinimalSunburnGrIxvalue", 0.98);
                    classificatorSettings.Add("GrIxProcessingVerbosityLevel", 2);
                    classificatorSettings.Add("GrIxSunDetectorArcedCropFactor", 0.95d);
                    classificatorSettings.Add("GrIxSunDetectorConcurrentThreadsLimit", 4);
                    #endregion

                    SkyCloudClassification classificatorID1 = new SkyCloudClassification(imgBgrID1, classificatorSettings);
                    classificatorID1.LogWindow = theLogWindow;

                    string randomFileName = "processing-" + (new FileInfo(inImgFilename)).Name;
                    randomFileName = randomFileName.Replace(".jpg", "");
                    RoundData sunRoundDataID1 = classificatorID1.DetectSunWithSerieOfArcs(imgPid1, dmProcessingData,
                        outputDirectory, randomFileName);

                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        (new FileInfo(inImgFilename)).Name + ": sun location: " + Environment.NewLine +
                        sunRoundDataID1.ToString());

            
                    if (sunRoundDataID1.IsNull)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "couldn`t detect sun disk for " + (new FileInfo(inImgFilename)).Name);
                    }
            


                    args.Result = new object[] { sunRoundDataID1 };
                };


            RunWorkerCompletedEventHandler bgwFindSunLocationCompletedHandler =
                delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[]) args.Result;
                    RoundData rdRetData = (RoundData) currentBGWResults[0];
                };


            BackgroundWorker bgwFindSunLocation = new BackgroundWorker();
            bgwFindSunLocation.DoWork += bgwFindSunLocationDoWork;
            bgwFindSunLocation.RunWorkerCompleted += bgwFindSunLocationCompletedHandler;
            bgwFindSunLocation.WorkerSupportsCancellation = true;
            object[] BGWargs = new object[] { wholeImgID1, imgID1filename, strOutputDirectory };
            bgwFindSunLocation.RunWorkerAsync(BGWargs);
        }

        private void btnImg2DetectSun_Click(object sender, EventArgs e)
        {

        }
    }
}
