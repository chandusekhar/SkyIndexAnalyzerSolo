using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkyIndexAnalyzerLibraries;
using System.Windows;


namespace FilesFilteringApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }




        private void buttonSelect_Click(object sender, EventArgs e)
        {
            TextBox currFilenameTextbox = tbFromPath;

            if (sender == btnFromPathSelect)
            {
                currFilenameTextbox = tbFromPath;
            }
            else if (sender == btnToPathSelect)
            {
                currFilenameTextbox = tbToPath;
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
            ThreadSafeOperations.SetTextTB(tbFromPath, Properties.Settings.Default.FromPath, false);
            ThreadSafeOperations.SetTextTB(tbToPath, Properties.Settings.Default.ToPath, false);
        }

        private void btnDoWork_Click(object sender, EventArgs e)
        {
            //ThreadSafeOperations.SetTextTB(tbLog, "#001" + Environment.NewLine, true);
            if (bgwCopier.IsBusy)
            {
                //ThreadSafeOperations.SetTextTB(tbLog, "#002" + Environment.NewLine, true);
                bgwCopier.CancelAsync();
            }
            else
            {
                //ThreadSafeOperations.SetTextTB(tbLog, "#003" + Environment.NewLine, true);
                bgwCopier.RunWorkerAsync();
                ThreadSafeOperations.ToggleButtonState(btnDoWork, true, "CANCEL", true);
            }
            //ThreadSafeOperations.ToggleButtonState(btnDoWork, true, "CANCEL", true);
            //CopierWork();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tbFromPath.Text != Properties.Settings.Default.FromPath)
            {
                Properties.Settings.Default.FromPath = tbFromPath.Text;
            }
            if (tbToPath.Text != Properties.Settings.Default.ToPath)
            {
                Properties.Settings.Default.ToPath = tbToPath.Text;
            }

            Properties.Settings.Default.Save();
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
            //ThreadSafeOperations.SetTextTB(tbLog, "#004" + Environment.NewLine, true);
            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;

            DirectoryInfo dir = new DirectoryInfo(tbFromPath.Text);
            String destDirectory = tbToPath.Text + "\\";

            //ThreadSafeOperations.SetTextTB(tbLog, "#005" + Environment.NewLine, true);
            if (!dir.Exists)
            {
                ThreadSafeOperations.SetTextTB(tbLog, "Операция не выполнена. Не найдена директория:" + Environment.NewLine + tbFromPath.Text + Environment.NewLine, true);
                return;
            }

            //ThreadSafeOperations.SetTextTB(tbLog, "#006" + Environment.NewLine, true);
            FileInfo[] FileList2Process = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
            int filesCount = FileList2Process.Length;
            ThreadSafeOperations.SetTextTB(tbLog, "searching in directory: " + dir.FullName + Environment.NewLine, true);
            ThreadSafeOperations.SetTextTB(tbLog, "files found count: " + filesCount + Environment.NewLine, true);

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
                ThreadSafeOperations.SetTextTB(tbLog, "processing file " + fileInfo.Name + Environment.NewLine, true);


                //String curDateTime = "";
                int minute = 0;
                //String dateTime = (String)newIInfo.getValueByKey("DateTime");
                String dateTime = (String)newIInfo.getValueByKey("ExifDTOrig");
                
                //curDateTime = dateTime;

                DateTime curDateTime = DateTimeOfString(dateTime);
                DateTime theHour = RoundToHour(curDateTime);

                //minute = Convert.ToInt32(dateTime.Substring(14, 2));

                //if ((minute == 0) && (!usedDateTimes.Contains(curDateTime.Substring(0, 16))))
                if (!listUsedHours.Contains(theHour))
                {
                    //usedDateTimes = usedDateTimes + Environment.NewLine + curDateTime.Substring(0, 16);
                    //String newFileName = destDirectory + curDateTime.Substring(0, 16).Replace(":", "-").Replace(" ", "-") + "-" + fileInfo.Name;
                    listUsedHours.Add(theHour);
                    String newFileName = destDirectory + fileInfo.Name;
                    File.Copy(fileInfo.FullName, newFileName);
                    ThreadSafeOperations.SetTextTB(tbLog, "COPY: " + fileInfo.FullName + "   >>>   " + newFileName + Environment.NewLine, true);
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
                ThreadSafeOperations.SetTextTB(tbLog, "ERROR has been caught: " + Environment.NewLine, true);
                ThreadSafeOperations.SetTextTB(tbLog, e.Error.Message + Environment.NewLine, true);
            }


            //ThreadSafeOperations.SetTextTB(tbLog, "#007" + Environment.NewLine, true);
            ThreadSafeOperations.UpdateProgressBar(prbUniversalProgress, 0);
            ThreadSafeOperations.SetTextTB(tbLog, "Finished work" + Environment.NewLine, true);
            ThreadSafeOperations.ToggleButtonState(btnDoWork, true, "SELECT", false);
        }
    }
}
