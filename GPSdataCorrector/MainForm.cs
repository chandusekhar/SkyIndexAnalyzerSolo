using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using IoffeVesselDataReader;
using SkyImagesAnalyzerLibraries;

namespace GPSdataCorrector
{
    public partial class MainForm : Form
    {
        private LogWindow theLogWindow = null;
        private Dictionary<string, object> defaultProperties = null;


        public MainForm()
        {
            InitializeComponent();
        }







        private void btnSelectSourceDirectory_Click(object sender, EventArgs e)
        {
            RichTextBox currFilenameTextbox = rtbPath;

            FolderBrowserDialog opFD = new FolderBrowserDialog();
            opFD.ShowNewFolderButton = true;
            opFD.SelectedPath = Directory.GetCurrentDirectory();
            DialogResult dialogRes = opFD.ShowDialog();
            if (dialogRes == DialogResult.OK)
            {
                String pathName = opFD.SelectedPath;
                ThreadSafeOperations.SetTextTB(currFilenameTextbox, pathName, false);
            }
        }




        private void btnSelectVesselNavDataDirectory_Click(object sender, EventArgs e)
        {
            RichTextBox currFilenameTextbox = rtbVesselNavDataDirectoryPath;

            FolderBrowserDialog opFD = new FolderBrowserDialog();
            opFD.ShowNewFolderButton = true;
            opFD.SelectedPath = Directory.GetCurrentDirectory();
            DialogResult dialogRes = opFD.ShowDialog();
            if (dialogRes == DialogResult.OK)
            {
                String pathName = opFD.SelectedPath;
                ThreadSafeOperations.SetTextTB(currFilenameTextbox, pathName, false);
            }
        }




        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }






        private void btnCorrectGPSdata_Click(object sender, EventArgs e)
        {
            string infoFilesPath = rtbPath.Text;
            string navdataFilesPath = rtbVesselNavDataDirectoryPath.Text;
            infoFilesPath += ((infoFilesPath.Substring(infoFilesPath.Length - 1) == "\\") ? ("") : ("\\"));
            string bcpFilesPath = infoFilesPath + "backup\\";
            if (infoFilesPath == "")
            {
                return;
            }
            if (!Directory.Exists(infoFilesPath))
            {
                return;
            }
            if (navdataFilesPath == "")
            {
                return;
            }
            if (!Directory.Exists(navdataFilesPath))
            {
                return;
            }

            DirectoryInfo dirConcurrentDataFiles = new DirectoryInfo(infoFilesPath);
            FileInfo[] concurrentDataFilesList = dirConcurrentDataFiles.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            //string[] sConcurrentInfoFilenames = Directory.GetFiles(rtbPath.Text, "*.xml");
            if (!concurrentDataFilesList.Any())
            {
                return;
            }
            if (!ServiceTools.CheckIfDirectoryExists(bcpFilesPath))
            {
                return;
            }

            CultureInfo provider = CultureInfo.InvariantCulture;

            foreach (FileInfo concurrentInfoFileInfo in concurrentDataFilesList)
            {
                Dictionary<string, object> dictSavedData = ServiceTools.ReadDictionaryFromXML(concurrentInfoFileInfo.FullName);
                DateTime computerDateTime = DateTime.ParseExact((string)dictSavedData["DateTime"], "o", provider).ToUniversalTime();

                DateTime gpsDateTime = DateTime.ParseExact((string)dictSavedData["GPSDateTimeUTC"], "o", provider).ToUniversalTime();

                if (Math.Abs((gpsDateTime - computerDateTime).TotalSeconds) > 300)
                {
                    //надо корректировать по данным судовой навигации
                    File.Copy(concurrentInfoFileInfo.FullName, bcpFilesPath + concurrentInfoFileInfo.Name);
                    dictSavedData["GPSdata"] = "$GPGGA," + computerDateTime.Hour.ToString("D02") +
                                               computerDateTime.Minute.ToString("D02") +
                                               computerDateTime.Second.ToString("D02") + ".00";

                    IoffeVesselDualNavDataConverted ioffeNavdata =
                        IoffeVesselNavDataReader.GetNavDataByDatetime(navdataFilesPath, computerDateTime);
                    if (ioffeNavdata == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "couldn`t find vessel nav data for file: " + Environment.NewLine +
                            concurrentInfoFileInfo.FullName);
                        continue;
                    }

                    dictSavedData["GPSLat"] = ioffeNavdata.gps.Lat;
                    dictSavedData["GPSLon"] = ioffeNavdata.gps.Lon;
                    dictSavedData["GPSDateTimeUTC"] = ioffeNavdata.gps.dateTimeUTC.ToString("o");

                    ServiceTools.WriteDictionaryToXml(dictSavedData, concurrentInfoFileInfo.FullName);
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            readDefaultProperties();
        }


        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            string CurDir = Directory.GetCurrentDirectory();
            string defaultPropertiesXMLfileName = CurDir + "\\settings\\GPSdataCorrectorSettings.xml";
            if (!File.Exists(defaultPropertiesXMLfileName)) return;
            defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);


            rtbPath.Text = (string) defaultProperties["InputDataFilesDirectory"];
            rtbVesselNavDataDirectoryPath.Text = (string)defaultProperties["VesselMeteoNavDataPath"];
        }

        
    }
}
