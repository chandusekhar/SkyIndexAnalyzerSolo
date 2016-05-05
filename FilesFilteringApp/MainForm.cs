using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using SkyImagesAnalyzer;
using SkyImagesAnalyzerLibraries;


namespace FilesFilteringApp
{
    public partial class MainForm : Form
    {
        private Dictionary<string, object> defaultProperties = null;
        private string defaultPropertiesXMLfileName = "";
        private string CopyImagesFrom_Path = "";
        string CopyImagesAndDataTo_Path = "";
        string ConcurrentDataXMLfilesPath = "";
        string GrIxYRGBstatsXMLfilesPath = "";
        private TimeSpan filterTolerance = new TimeSpan(0, 1, 0);
        private LogWindow theLogWindow = null;



        public MainForm()
        {
            InitializeComponent();
        }




        private void buttonSelect_Click(object sender, EventArgs e)
        {
            RichTextBox currFilenameTextbox = rtbFromPath;

            if (sender == btnFromPathSelect)
            {
                currFilenameTextbox = rtbFromPath;
            }
            else if (sender == btnToPathSelect)
            {
                currFilenameTextbox = rtbToPath;
            }
            else if (sender == btnConcurrentDataPathSelect)
            {
                currFilenameTextbox = rtbConcurrentDataDir;
            }
            else if (sender == btnStatsDirPathSelect)
            {
                currFilenameTextbox = rtbGrIxYRGBstatsDir;
            }

            FolderBrowserDialog opFD = new FolderBrowserDialog();
            opFD.ShowNewFolderButton = true;
            opFD.SelectedPath = Directory.GetCurrentDirectory();
            DialogResult dialogRes = opFD.ShowDialog();
            if (dialogRes == DialogResult.OK)
            {
                String pathName = opFD.SelectedPath;
                //FileInfo fInfo = new FileInfo(filename);
                ThreadSafeOperations.SetTextTB(currFilenameTextbox, pathName, false);
            }
        }




        private void MainForm_Load(object sender, EventArgs e)
        {
            readDefaultProperties();
        }





        private void btnDoWork_Click(object sender, EventArgs e)
        {
            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "=========== Processing started on " + DateTime.Now.ToString("s") + " ===========");

            if (bgwCopier.IsBusy)
            {
                bgwCopier.CancelAsync();
            }
            else
            {

                object[] args = new object[] { rtbFromPath.Text, rtbConcurrentDataDir.Text, rtbToPath.Text, rtbGrIxYRGBstatsDir.Text };

                bgwCopier.RunWorkerAsync(args);
                ThreadSafeOperations.ToggleButtonState(btnDoWork, true, "CANCEL", true);
            }
        }



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveDefaultProperties();
        }



        //private void CopierWork()
        //{
        //    //ThreadSafeOperations.SetTextTB(tbLog, "#008" + Environment.NewLine, true);
        //    //System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;

        //    DirectoryInfo dir = new DirectoryInfo(tbFromPath.Text);
        //    String destDirectory = tbToPath.Text + "\\";

        //    //ThreadSafeOperations.SetTextTB(tbLog, "#009" + Environment.NewLine, true);
        //    if (!dir.Exists)
        //    {
        //        ThreadSafeOperations.SetTextTB(tbLog, "Операция не выполнена. Не найдена директория:" + Environment.NewLine + tbFromPath.Text + Environment.NewLine, true);
        //        return;
        //    }

        //    //ThreadSafeOperations.SetTextTB(tbLog, "#010" + Environment.NewLine, true);
        //    FileInfo[] FileList2Process = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
        //    int filesCount = FileList2Process.Length;
        //    ThreadSafeOperations.SetTextTB(tbLog, "searching in directory: " + dir.FullName + Environment.NewLine, true);
        //    ThreadSafeOperations.SetTextTB(tbLog, "files found count: " + filesCount + Environment.NewLine, true);

        //    String usedDateTimes = "";

        //    int counter = 0;
        //    foreach (FileInfo fileInfo in FileList2Process)
        //    {
        //        //if (SelfWorker.CancellationPending)
        //        //{
        //        //    break;
        //        //}
        //        //counter++;
        //        //double percCounter = (double)counter * 1000.0d / (double)filesCount;
        //        //SelfWorker.ReportProgress(Convert.ToInt32(percCounter));


        //        Image anImage = Image.FromFile(fileInfo.FullName);
        //        ImageInfo newIInfo = new ImageInfo(anImage);
        //        ThreadSafeOperations.SetTextTB(tbLog, "processing file " + fileInfo.Name + Environment.NewLine, true);


        //        String curDateTime = "";
        //        int minute = 0;
        //        String dateTime = (String)newIInfo.getValueByKey("DateTime");
        //        curDateTime = dateTime;
        //        minute = Convert.ToInt32(dateTime.Substring(14, 2));

        //        if ((minute == 0) && (!usedDateTimes.Contains(curDateTime.Substring(0, 16))))
        //        {
        //            usedDateTimes = usedDateTimes + Environment.NewLine + curDateTime.Substring(0, 16);
        //            String newFileName = destDirectory + curDateTime.Substring(0, 16).Replace(":", "-").Replace(" ", "-") + "-" + fileInfo.Name;
        //            File.Copy(fileInfo.FullName, newFileName);
        //            ThreadSafeOperations.SetTextTB(tbLog, "COPY: " + fileInfo.FullName + "   >>>   " + newFileName + Environment.NewLine, true);
        //        }
        //    }

        //    bgwCopier_RunWorkerCompleted(null, null);
        //}



        private void bgwCopier_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            object[] bgwArgs = e.Argument as object[];
            string fromPath = bgwArgs[0] as string;
            string concurrentDataFilesPath = bgwArgs[1] as string;
            string toPath = bgwArgs[2] as string;
            string imagesStatsXMLfilesDir = bgwArgs[3] as string;

            DirectoryInfo dir = new DirectoryInfo(fromPath);
            String destDirectory = toPath +
                                   ((toPath.Last() == Path.DirectorySeparatorChar)
                                       ? ("")
                                       : (Path.DirectorySeparatorChar.ToString()));

            if (!dir.Exists)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Операция не выполнена. Не найдена директория:" + Environment.NewLine + fromPath +
                    Environment.NewLine);
                //ThreadSafeOperations.SetTextTB(tbLog, "Операция не выполнена. Не найдена директория:" + Environment.NewLine + fromPath + Environment.NewLine, true);
                return;
            }


            FileInfo[] FileList2Process = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
            List<Tuple<string, string>> imagesStatsXMLfiles = new List<Tuple<string, string>>();
            if (Directory.Exists(imagesStatsXMLfilesDir))
            {
                imagesStatsXMLfiles =
                    (new DirectoryInfo(imagesStatsXMLfilesDir)).EnumerateFiles(
                        ConventionalTransitions.ImageGrIxYRGBstatsFileNamesPattern(), SearchOption.AllDirectories)
                        .ToList()
                        .ConvertAll(fInfo => new Tuple<string, string>(fInfo.Name, fInfo.FullName));
            }


            DirectoryInfo dirConcurrentDataFiles = new DirectoryInfo(concurrentDataFilesPath);
            List<Tuple<string, DateTime>> lConcurrentDataFiles =
                dirConcurrentDataFiles.EnumerateFiles(ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(),
                    SearchOption.AllDirectories).ToList().ConvertAll(fInfo =>
                    {
                        // data-2015-12-15T06-12-56.0590302Z.xml
                        string strDateTimeOfFile = Path.GetFileNameWithoutExtension(fInfo.Name).Substring(5, 28);
                        strDateTimeOfFile = strDateTimeOfFile.Substring(0, 11) +
                                            strDateTimeOfFile.Substring(11).Replace('-', ':');
                        DateTime currFileDT = DateTime.Parse(strDateTimeOfFile, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                        currFileDT = DateTime.SpecifyKind(currFileDT, DateTimeKind.Utc);
                        return new Tuple<string, DateTime>(fInfo.FullName, currFileDT);
                    });





            int filesCount = FileList2Process.Length;
            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "searching in directory: " + dir.FullName + Environment.NewLine);
            //ThreadSafeOperations.SetTextTB(tbLog, "searching in directory: " + dir.FullName + Environment.NewLine, true);
            theLogWindow = ServiceTools.LogAText(theLogWindow, "files found count: " + filesCount + Environment.NewLine);
            //ThreadSafeOperations.SetTextTB(tbLog, "files found count: " + filesCount + Environment.NewLine, true);

            String usedDateTimes = "";

            List<DateTime> listUsedHours = new List<DateTime>();

            int counter = 0;
            foreach (FileInfo fileInfo in FileList2Process)
            {
                if (SelfWorker.CancellationPending)
                {
                    break;
                }
                counter++;
                double percCounter = (double)counter * 1000.0d / (double)filesCount;
                SelfWorker.ReportProgress(Convert.ToInt32(percCounter));


                Image anImage = Image.FromFile(fileInfo.FullName);
                ImageInfo newIInfo = new ImageInfo(anImage);
                //ThreadSafeOperations.SetTextTB(tbLog, "processing file " + fileInfo.Name + Environment.NewLine, true);


                //String curDateTime = "";
                int minute = 0;
                //String dateTime = (String)newIInfo.getValueByKey("DateTime");
                String strDateTimeEXIF = (String)newIInfo.getValueByKey("ExifDTOrig");
                if (strDateTimeEXIF == null)
                {
                    //попробуем вытащить из имени файла
                    string strDateTime = fileInfo.Name;
                    strDateTime = strDateTime.Substring(4, 19);
                    strDateTimeEXIF = strDateTime;
                }


                //curDateTime = dateTime;
                DateTime curImgDateTime;
                DateTime theHour = RoundToHour(DateTime.UtcNow);
                try
                {
                    //curImgDateTime = DateTimeOfString(strDateTimeEXIF);
                    curImgDateTime = ConventionalTransitions.DateTimeOfSkyImageFilename(fileInfo.Name);
                    theHour = RoundToHour(curImgDateTime);
                }
                catch (Exception ex)
                {
                    continue;
                }


                //minute = Convert.ToInt32(strDateTimeEXIF.Substring(14, 2));

                //if ((minute == 0) && (!listUsedHours.Contains(theHour)))
                if (new TimeSpan(Math.Abs((theHour-curImgDateTime).Ticks)) <= filterTolerance)
                {
                    #region copy the image file itself
                    listUsedHours.Add(theHour);

                    string dateDirectorySuffix = curImgDateTime.ToString("yyyy-MM-dd");
                    string currDateDestDirectory = destDirectory + dateDirectorySuffix + Path.DirectorySeparatorChar;
                    if (!ServiceTools.CheckIfDirectoryExists(currDateDestDirectory))
                    {
                        currDateDestDirectory = destDirectory;
                    }
                    


                    String newFileName = currDateDestDirectory + fileInfo.Name;
                    File.Copy(fileInfo.FullName, newFileName);
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "COPY: " + fileInfo.FullName + "   >>>   " + newFileName + Environment.NewLine);
                    //ThreadSafeOperations.SetTextTB(tbLog, "COPY: " + fileInfo.FullName + "   >>>   " + newFileName + Environment.NewLine, true);
                    #endregion copy the image file itself


                    #region find and copy the GrIx,YRGB stats data file
                    if (imagesStatsXMLfiles.Any())
                    {
                        string xmlStatsFileName =
                            ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(fileInfo.FullName, "", false);

                        Tuple<string, string> foundXMLfile =
                            imagesStatsXMLfiles.Find(tpl => tpl.Item1 == xmlStatsFileName);

                        if (foundXMLfile != null)
                        {
                            string sStatsXMLfilename = foundXMLfile.Item2;
                            string newStatsXMLfilename = currDateDestDirectory + foundXMLfile.Item1;
                            File.Copy(sStatsXMLfilename, newStatsXMLfilename);
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                "COPY: " + sStatsXMLfilename + "   >>>   " + newStatsXMLfilename + Environment.NewLine);
                            //ThreadSafeOperations.SetTextTB(tbLog,
                            //    "COPY: " + sStatsXMLfilename + "   >>>   " + newStatsXMLfilename + Environment.NewLine,
                            //    true);
                        }
                        else
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                "========== ERROR: couldn`t find GrIx,YRGB stats XML file" + Environment.NewLine);
                            //ThreadSafeOperations.SetTextTB(tbLog,
                            //    "========== ERROR: couldn`t find GrIx,YRGB stats XML file" + Environment.NewLine, true);
                        }
                    }
                    #endregion find and copy the GrIx,YRGB stats data file


                    #region find and copy concurrent data XML file
                    if (lConcurrentDataFiles.Any())
                    {
                        //найдем ближайший по времени
                        List<Tuple<string, TimeSpan>> lCurrFileConcurrentDataNearest =
                            lConcurrentDataFiles.ConvertAll(
                                tpl =>
                                    new Tuple<string, TimeSpan>(tpl.Item1,
                                        new TimeSpan(Math.Abs((tpl.Item2 - curImgDateTime).Ticks))));
                        lCurrFileConcurrentDataNearest.Sort(new Comparison<Tuple<string, TimeSpan>>((tpl1, tpl2) =>
                        {
                            if (tpl1 == null)
                            {
                                if (tpl2 == null) return 0;
                                else return -1;
                            }
                            else
                            {
                                if (tpl2 == null) return 1;
                                else return tpl1.Item2.CompareTo(tpl2.Item2);
                            }

                        }));

                        GPSdata gps = new GPSdata();
                        Tuple<string, TimeSpan> nearestConcurrentDataFile = null;
                        int concurrentDataFileIdx = 0;
                        
                        while (!gps.validGPSdata)
                        {
                            nearestConcurrentDataFile = lCurrFileConcurrentDataNearest[concurrentDataFileIdx];

                            Dictionary<string, object> dictSavedData = ServiceTools.ReadDictionaryFromXML(nearestConcurrentDataFile.Item1);

                            gps = new GPSdata((string)dictSavedData["GPSdata"],
                                GPSdatasources.CloudCamArduinoGPS,
                                DateTime.Parse((string)dictSavedData["GPSDateTimeUTC"], null,
                                    System.Globalization.DateTimeStyles.RoundtripKind));

                            concurrentDataFileIdx++;
                        }

                        string currValidConcurrentDataFile = nearestConcurrentDataFile.Item1;
                        string currValidConcurrentDataFileToCopyTo = currDateDestDirectory + "data-" +
                                                                     Path.GetFileNameWithoutExtension(fileInfo.FullName) + ".xml";
                        File.Copy(currValidConcurrentDataFile, currValidConcurrentDataFileToCopyTo);
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "COPY: " + currValidConcurrentDataFile + "   >>>   " + currValidConcurrentDataFileToCopyTo +
                            Environment.NewLine);
                        //ThreadSafeOperations.SetTextTB(tbLog,
                        //    "COPY: " + currValidConcurrentDataFile + "   >>>   " + currValidConcurrentDataFileToCopyTo +
                        //    Environment.NewLine, true);

                    }
                    else
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "========== ERROR: couldn`t find concurrent data file for " + fileInfo.FullName +
                            Environment.NewLine);
                        //ThreadSafeOperations.SetTextTB(tbLog, "========== ERROR: couldn`t find concurrent data file for " + fileInfo.FullName + Environment.NewLine, true);
                    }
                    #endregion find and copy concurrent data XML file


                    theLogWindow.ClearLog();
                }
            }
        }




        static DateTime DateTimeOfString(string strDateTime)
        {
            // 2014:07:24 01:07:45
            DateTime dt = new DateTime(
                Convert.ToInt32(strDateTime.Substring(0, 4)),
                Convert.ToInt32(strDateTime.Substring(5, 2)),
                Convert.ToInt32(strDateTime.Substring(8, 2)),
                Convert.ToInt32(strDateTime.Substring(11, 2)),
                Convert.ToInt32(strDateTime.Substring(14, 2)),
                Convert.ToInt32(strDateTime.Substring(17, 2)));
            return dt;
        }

        





        static DateTime RoundToHour(DateTime dt)
        {
            long ticks = dt.Ticks + 18000000000;
            return new DateTime(ticks - ticks % 36000000000);
        }



        private void bgwCopier_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ThreadSafeOperations.UpdateProgressBar(prbUniversalProgress, e.ProgressPercentage);
        }

        private void bgwCopier_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "ERROR has been caught: " + Environment.NewLine + e.Error.Message + Environment.NewLine);
                //ThreadSafeOperations.SetTextTB(tbLog, "ERROR has been caught: " + Environment.NewLine, true);
                //ThreadSafeOperations.SetTextTB(tbLog, e.Error.Message + Environment.NewLine, true);
            }


            //ThreadSafeOperations.SetTextTB(tbLog, "#007" + Environment.NewLine, true);
            ThreadSafeOperations.UpdateProgressBar(prbUniversalProgress, 0);
            theLogWindow = ServiceTools.LogAText(theLogWindow, "Finished work" + Environment.NewLine);
            //ThreadSafeOperations.SetTextTB(tbLog, "Finished work" + Environment.NewLine, true);
            ThreadSafeOperations.ToggleButtonState(btnDoWork, true, "SELECT", false);
        }



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



        private void PropertiesFormClosed(object sender, FormClosedEventArgs e)
        {
            readDefaultProperties();
        }


        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";
            if (!File.Exists(defaultPropertiesXMLfileName)) return;
            defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);
            bool bDefaultPropertiesHasBeenUpdated = false;



            if (defaultProperties.ContainsKey("CopyImagesFrom_Path"))
            {
                CopyImagesFrom_Path = (string)defaultProperties["CopyImagesFrom_Path"];
            }
            else
            {
                CopyImagesFrom_Path = "";
                defaultProperties.Add("CopyImagesFrom_Path", CopyImagesFrom_Path);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            ThreadSafeOperations.SetTextTB(rtbFromPath, CopyImagesFrom_Path, false);





            if (defaultProperties.ContainsKey("CopyImagesAndDataTo_Path"))
            {
                CopyImagesAndDataTo_Path = (string)defaultProperties["CopyImagesAndDataTo_Path"];
            }
            else
            {
                CopyImagesAndDataTo_Path = "";
                defaultProperties.Add("CopyImagesAndDataTo_Path", CopyImagesAndDataTo_Path);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            ThreadSafeOperations.SetTextTB(rtbToPath, CopyImagesAndDataTo_Path, false);




            if (defaultProperties.ContainsKey("ConcurrentDataXMLfilesPath"))
            {
                ConcurrentDataXMLfilesPath = (string)defaultProperties["ConcurrentDataXMLfilesPath"];
            }
            else
            {
                ConcurrentDataXMLfilesPath = "";
                defaultProperties.Add("ConcurrentDataXMLfilesPath", ConcurrentDataXMLfilesPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            ThreadSafeOperations.SetTextTB(rtbConcurrentDataDir, ConcurrentDataXMLfilesPath, false);




            if (defaultProperties.ContainsKey("GrIxYRGBstatsXMLfilesPath"))
            {
                GrIxYRGBstatsXMLfilesPath = (string)defaultProperties["GrIxYRGBstatsXMLfilesPath"];
            }
            else
            {
                GrIxYRGBstatsXMLfilesPath = "";
                defaultProperties.Add("GrIxYRGBstatsXMLfilesPath", GrIxYRGBstatsXMLfilesPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            ThreadSafeOperations.SetTextTB(rtbGrIxYRGBstatsDir, GrIxYRGBstatsXMLfilesPath, false);



            if (defaultProperties.ContainsKey("filterTolerance"))
            {
                filterTolerance = TimeSpan.Parse((string)defaultProperties["filterTolerance"]);
            }
            else
            {
                defaultProperties.Add("filterTolerance", filterTolerance.ToString());
                bDefaultPropertiesHasBeenUpdated = true;
            }



            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }




        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }




        private void rtb_TextChanged(object sender, EventArgs e)
        {
            string currKey = "";
            string currValue = "";
            if (sender == rtbConcurrentDataDir)
            {
                currKey = "ConcurrentDataXMLfilesPath";
                currValue = rtbConcurrentDataDir.Text;
                ConcurrentDataXMLfilesPath = currValue;
            }
            else if (sender == rtbFromPath)
            {
                currKey = "CopyImagesFrom_Path";
                currValue = rtbFromPath.Text;
                CopyImagesFrom_Path = currValue;
            }
            else if (sender == rtbGrIxYRGBstatsDir)
            {
                currKey = "GrIxYRGBstatsXMLfilesPath";
                currValue = rtbGrIxYRGBstatsDir.Text;
                GrIxYRGBstatsXMLfilesPath = currValue;
            }
            else if (sender == rtbToPath)
            {
                currKey = "CopyImagesAndDataTo_Path";
                currValue = rtbToPath.Text;
                CopyImagesAndDataTo_Path = currValue;
            }


            if (defaultProperties.ContainsKey(currKey))
            {
                defaultProperties[currKey] = currValue;
                saveDefaultProperties();
            }
            else
            {
                defaultProperties.Add(currKey, currValue);
                saveDefaultProperties();
            }
        }
    }
}
