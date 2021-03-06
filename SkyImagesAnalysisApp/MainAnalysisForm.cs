﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Drawing.Imaging;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.Statistics;
using SkyImagesAnalyzerLibraries;
using SolarPositioning;
using Accord.Math;
using ANN;
using System.Management.Automation;
using DataAnalysis;
using Geometry;


namespace SkyImagesAnalyzer
{
    public partial class MainAnalysisForm : Form
    {
        #region vars

        #region устарели
        //private StreamReader _textStreamReader;
        //private Image imagetoadd;
        //public FileStream CCDataFile;
        //public FileStream EXIFdataFile;
        //private FileStream StatDataFile1;
        //private bool gotexifs;
        //public System.Data.DataTable datatableFnumber, datatableISOspeed, datatableExposureTime;
        //private double resizevalue;
        #endregion

        private Assembly _assembly;
        private Image<Bgr, Byte> imagetoadd;
        private Bitmap bitmapSI;
        private AboutBox1 aboutprog;
        private TimeSpan timetotal;
        private DateTime begintotal;
        private string ImageFileName;

        public double tunedSIMargin;
        private double tunedSIMarginDefault;

        private delegate void DelegateOpenFile(String s);

        private int FileCounter, filecounter_prev;
        private int SIClScMarginRangeLowerValue, SIClScMarginRangeHigherValue;
        private SkyCloudClassification classificator;
        private Bitmap pictureBox2Bitmap;
        private Dictionary<string, object> defaultProperties = null;
        private string defaultPropertiesXMLfileName = "";
        private LogWindow theLogWindow = null;

        private string strConcurrentDataXMLfilesPath = "";

        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";
        #endregion vars

        #region ML related vars
        private string strMLdefaultDataDirectory = "";
        #endregion


        private string FilesAndConcurrentDataListToProcessForCloudCover = "";



        private DelegateOpenFile m_DelegateOpenFile;



        #region //OBSOLETE
        //private double tunedIFMMargin; 
        //private DateTime beginpartial;
        //private TimeSpan timepartial;
        //public int ImageWidthOrig, ImageHeightOrig;
        //private Image.GetThumbnailImageAbort mycallback;
        //public string text1;
        //public Bitmap curr_bitmap;
        //private ExcelDumper XLdumper;
        //public static System.Windows.Forms.TextBox reportingTextBox;
        //private LogWindow currentLogWindow = null;
        //public bool ProcessingConditionsChanged;
        //private pb2Source CurrentPB2Source = pb2Source.SkyindexAnalyzer;
        //private ConnectedObjectsDetection ObjectsDetector;
        //private SkyCloudClassification SkyCloudClassificator;
        //public ProgressBar DefaultProgressBar;
        //public string path2process;
        //public FileInfo[] FileList2Process;
        //public int TrackBar3Value;
        //public bool ProcessButtonPushedOnceThisFile;
        //public double tunedIFMMarginDefault;
        //public int CloudCounterSI, SkyCounterSI;
        //public FileStream OutputFile3;
        //public FileStream MetadataStatFile;
        //public double SI;
        //public int th_width, th_height;
        //public int th_widthSI, th_heightSI;
        //public Bitmap curr_th_bitmap;
        //public Bitmap bmSICloudCondition;
        //public Bitmap bmSIData;
        //public Color curr_color;
        //public int overallpixelcount, counter1, rowindex;
        //public bool SensorsCycleWorking;
        //public int SensorsCycleDataGotTimes;
        //private double tunedIFMMargin_prev;
        //public double tunedSIMargin_prev;
        #endregion //OBSOLETE








        #region PropertiesForm behaviour

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

        #endregion PropertiesForm behaviour


        public MainAnalysisForm()
        {
            InitializeComponent();
        }



        private void OpenFile(string filelistmember)
        {
            //int ImageWidthRec, ImageHeightRec;
            //Image thumb;



            ImageFileName = filelistmember;
            //imagetoadd = Image.FromFile(filelistmember);

            imagetoadd = new Image<Bgr, byte>(filelistmember); // ServiceTools.ReadBitmapFromFile(filelistmember);

            imagetoadd = ImageProcessing.ImageResizer(imagetoadd, Convert.ToInt32(defaultProperties["DefaultMaxImageSize"]));

            imagetoadd = ImageProcessing.SquareImageDimensions(imagetoadd);

            ThreadSafeOperations.SetText(label8, "Размер изображения: " + imagetoadd.Width + "x" + imagetoadd.Height + "px", false);


            //tunedSIMargin = tunedSIMarginDefault;
            //ThreadSafeOperations.MoveTrackBar(trackBar1, (int)(tunedSIMargin * 100.0));
            //ProcessButtonPushedOnceThisFile = false;


            //text1 = "comments will be here";
            //gotexifs = false;

            ThreadSafeOperations.UpdatePictureBox(pictureBox1, imagetoadd.Bitmap, true);
            ThreadSafeOperations.UpdatePictureBox(pictureBox2, (Image)null);
            pictureBox2Bitmap = null;

            //FileInfo finfo = new FileInfo(ImageFileName);
            //string shortfname1 = finfo.Name;

            //SkyCloudClassificator = null;
            //ObjectsDetector = null;
            //ThreadSafeOperations.ToggleButtonState(btnDefineConnectedObjects, true, "Загрузить в классификатор", false);

            readDefaultProperties();
        }



        private void добавитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                OpenFile(Openfile.FileName);
                //pictureBox1.Image = My_Image.ToBitmap();
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            m_DelegateOpenFile = this.OpenFile;

            #region // obsolete

            //ProcessingConditionsChanged = false;



            //_assembly = Assembly.GetExecutingAssembly();
            //try
            //{
            //    Stream Stream1 = _assembly.GetManifestResourceStream("SkyImagesAnalyzer.Resources.Ex3SumSchema");
            //    _textStreamReader = new StreamReader(Stream1);
            //}
            //catch
            //{
            //    MessageBox.Show("Error accessing resources!");

            //}

            // ReadEx3CalculateScheme();

            //ThreadSafeOperations.SetText(label9, "Изменять исходный размер: 1.0", false);
            //ThreadSafeOperations.MoveTrackBar(trackBar3, 7);

            #endregion // obsolete

            button1.Text = "Обработка директории: ";
        }



        #region obsolete // оПрограммеToolStripMenuItem_Click

        //private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    aboutprog = new AboutBox1();
        //    aboutprog.Show(this);
        //}

        #endregion obsolete // оПрограммеToolStripMenuItem_Click
        #region obsolete // ExposureTimeHumanReadable

        private string ExposureTimeHumanReadable(int ETnumerator, int ETdenominator)
        {
            int remainder = 0;

            if (ETnumerator < ETdenominator)
            {
                int quotient = Math.DivRem(ETdenominator, ETnumerator, out remainder);
                if (remainder == 0)
                {
                    return "1/" + quotient.ToString();
                }
                else
                {
                    return ETnumerator.ToString() + "/" + ETdenominator.ToString();
                }
            }
            else
            {
                return ((double)ETnumerator / (double)ETdenominator).ToString();
            }
        }

        #endregion obsolete // ExposureTimeHumanReadable


        #region // устарело, пока больше не применяется
        //private void ReadEx3CalculateScheme()
        //{
        //    string Ex3XMLfilename = Directory.GetCurrentDirectory() + "\\settings\\Ex3SumSchema.xml";
        //    if (!File.Exists(Ex3XMLfilename)){return;}
        //    //Stream Stream1 = _assembly.GetManifestResourceStream("SkyImagesAnalyzer.Resources.Ex3SumSchema");
        //    StreamReader _textStreamReader = new StreamReader(Ex3XMLfilename, Encoding.UTF8);
        //    XmlReader reader;
        //    XmlDocument xmlfile;
        //    DataRow row1;

        //    datatableISOspeed = new System.Data.DataTable();
        //    datatableISOspeed.Columns.Add("Value", typeof(double));
        //    datatableISOspeed.Columns.Add("sum", typeof(double));
        //    datatableFnumber = new System.Data.DataTable();
        //    datatableFnumber.Columns.Add("Value", typeof(double));
        //    datatableFnumber.Columns.Add("sum", typeof(double));
        //    datatableExposureTime = new System.Data.DataTable();
        //    datatableExposureTime.Columns.Add("Value", typeof(double));
        //    datatableExposureTime.Columns.Add("sum", typeof(double));

        //    reader = XmlReader.Create(_textStreamReader);
        //    xmlfile = new XmlDocument();
        //    xmlfile.Load(reader);

        //    XmlNode ISOspeedTree = xmlfile.GetElementsByTagName("ISOSpeedTable").Item(0);
        //    for (int n = 0; n < ISOspeedTree.ChildNodes.Count; n++)
        //    {
        //        XmlNode ISODataRaw = ISOspeedTree.ChildNodes.Item(n);
        //        double ISOvalue = Convert.ToDouble(ISODataRaw.ChildNodes.Item(0).InnerText);
        //        double sum = Convert.ToDouble(ISODataRaw.ChildNodes.Item(1).InnerText);
        //        row1 = datatableISOspeed.NewRow();
        //        row1["Value"] = ISOvalue;
        //        row1["sum"] = sum;
        //        datatableISOspeed.Rows.Add(row1);
        //    }
        //    XmlNode FNumberTree = xmlfile.GetElementsByTagName("FNumberTable").Item(0);
        //    for (int n = 0; n < FNumberTree.ChildNodes.Count; n++)
        //    {
        //        XmlNode FNumberDataRaw = FNumberTree.ChildNodes.Item(n);
        //        double FNumberValue = Convert.ToDouble(FNumberDataRaw.ChildNodes.Item(0).InnerText);
        //        double sum = Convert.ToDouble(FNumberDataRaw.ChildNodes.Item(1).InnerText);
        //        row1 = datatableFnumber.NewRow();
        //        row1["Value"] = FNumberValue;
        //        row1["sum"] = sum;
        //        datatableFnumber.Rows.Add(row1);
        //    }
        //    XmlNode ExposureTimeTree = xmlfile.GetElementsByTagName("ExposureTimeTable").Item(0);
        //    for (int n = 0; n < ExposureTimeTree.ChildNodes.Count; n++)
        //    {
        //        XmlNode ExposureTimeDataRaw = ExposureTimeTree.ChildNodes.Item(n);
        //        double ExposureTimevalue = Convert.ToDouble(ExposureTimeDataRaw.ChildNodes.Item(0).InnerText);
        //        if (ExposureTimevalue > 1.0)
        //        {
        //            ExposureTimevalue = 1.0 / ExposureTimevalue;
        //        }
        //        double sum = Convert.ToDouble(ExposureTimeDataRaw.ChildNodes.Item(1).InnerText);
        //        row1 = datatableExposureTime.NewRow();
        //        row1["Value"] = ExposureTimevalue;
        //        row1["sum"] = sum;
        //        datatableExposureTime.Rows.Add(row1);
        //    }
        //    reader.Close();
        //}
        #endregion // устарело, пока больше не применяется


        #region // устарело, пока больше не применяется
        //private double ex3_calculate(double Fnumber, double ExposureTime, double ISOspeed)
        //{
        //    double ex3 = 0.0;
        //    double FNumberValue = (double)datatableFnumber.Rows[0].ItemArray[0];
        //    double FnumberSum = (double)datatableFnumber.Rows[0].ItemArray[1];
        //    double ISOSpeedValue = (double)datatableISOspeed.Rows[0].ItemArray[0];
        //    double ISOspeedSum = (double)datatableISOspeed.Rows[0].ItemArray[1];
        //    double ExposureTimeValue = (double)datatableExposureTime.Rows[0].ItemArray[0];
        //    double ExposureTimeSum = (double)datatableExposureTime.Rows[0].ItemArray[1];
        //    foreach (DataRow drFnumber in datatableFnumber.Rows)
        //    {
        //        double FNumberValue1 = (double)drFnumber.ItemArray[0];
        //        if (Math.Abs(FNumberValue1 - Fnumber) < Math.Abs(FNumberValue - Fnumber))
        //        {
        //            FNumberValue = FNumberValue1;
        //            FnumberSum = (double)drFnumber.ItemArray[1];
        //        }
        //    }

        //    foreach (DataRow drISOspeed in datatableISOspeed.Rows)
        //    {
        //        double ISOspeedValue1 = (double)drISOspeed.ItemArray[0];
        //        if (Math.Abs(ISOspeedValue1 - ISOspeed) < Math.Abs(ISOSpeedValue - ISOspeed))
        //        {
        //            ISOSpeedValue = ISOspeedValue1;
        //            ISOspeedSum = (double)drISOspeed.ItemArray[1];
        //        }
        //    }

        //    foreach (DataRow drExposureTime in datatableExposureTime.Rows)
        //    {
        //        double ExposureTimeValue1 = (double)drExposureTime.ItemArray[0];
        //        if (Math.Abs(ExposureTimeValue1 - ExposureTime) < Math.Abs(ExposureTimeValue - ExposureTime))
        //        {
        //            ExposureTimeValue = ExposureTimeValue1;
        //            ExposureTimeSum = (double)drExposureTime.ItemArray[1];
        //        }
        //    }

        //    ex3 = FnumberSum + ISOspeedSum + ExposureTimeSum;
        //    if (Math.Abs(Math.Abs(ex3 - Math.Truncate(ex3)) - 0.9) < 0.01)
        //    {
        //        ex3 = Math.Round(ex3);
        //    }

        //    return ex3;
        //}
        #endregion // устарело, пока больше не применяется




        #region // private void GetEXIFs() - устарело, пока больше не применяется
        //private void GetEXIFs()
        //{
        //    string strtowrite;
        //    byte[] info2write;

        //    if (ImageFileName == null)
        //    {
        //        return;
        //    }
        //    double ex3, DataValueFnumber, DataValueExpTime, DataValueISO, DataValueExpCorr;
        //    //string DataValueExpTimeHR = "";
        //    //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

        //    //string MetadataStatFilename = Directory.GetCurrentDirectory() + "\\meta_stat.dat";
        //    //MetadataStatFile = new FileStream(MetadataStatFilename, FileMode.Create, FileAccess.Write);
        //    string filename1 = ImageFileName;
        //    FileInfo imagefinfo = new FileInfo(filename1);
        //    string filenameEXIFdata = imagefinfo.DirectoryName + "\\exif.dat";
        //    EXIFdataFile = new FileStream(filenameEXIFdata, FileMode.Append, FileAccess.Write);
        //    FileInfo finfo2 = new FileInfo(filenameEXIFdata);
        //    if (finfo2.Length == 0)
        //    {
        //        strtowrite = "Filename\t\t\tISO\tFn\tT\texp3\n";
        //        info2write = new UTF8Encoding(true).GetBytes(strtowrite);
        //        EXIFdataFile.Write(info2write, 0, info2write.Length);
        //    }

        //    string stattxt = "";
        //    if (imagetoadd == null)
        //    {
        //        return;
        //    }

        //    Image image2Process = Image.FromFile(filename1);
        //    ImageInfo imInfo = new ImageInfo(image2Process);
        //    stattxt += filename1 + ";";

        //    //PropertyItem[] propItems = imagetoadd.PropertyItems;

        //    ex3 = 0.0;
        //    DataValueExpCorr = Convert.ToDouble(imInfo.getValueByKey("ExifExposureBias"));//ExifExposureBias
        //    DataValueExpTime = Convert.ToDouble(imInfo.getValueByKey("ExifExposureTime"));//ExifExposureTime
        //    DataValueFnumber = Convert.ToDouble(imInfo.getValueByKey("ExifFNumber"));
        //    DataValueISO = Convert.ToDouble(imInfo.getValueByKey("ExifISOSpeed"));//ExifISOSpeed
        //    ex3 += DataValueFnumber;

        //    if ((DataValueISO != 0.0d) && (DataValueExpCorr != 0.0d) && (DataValueExpTime != 0.0d) &&
        //        (DataValueFnumber != 0.0d))
        //    {
        //        ex3 = ex3_calculate(DataValueFnumber, DataValueExpTime, DataValueISO);

        //        Note("ISO speed = " + DataValueISO.ToString() + Environment.NewLine
        //             + "Exp. time = " + DataValueExpTime.ToString() + Environment.NewLine
        //             + "F number  = " + DataValueFnumber.ToString() + Environment.NewLine
        //             + "Exp.corr. = " + DataValueExpCorr.ToString() + Environment.NewLine
        //             + "Exp3 = " + ex3.ToString());

        //        stattxt += "\t" + ex3.ToString() + ";\tex.correction = " + DataValueExpCorr.ToString() +
        //                   Environment.NewLine;


        //        Note(DateTime.Now + "\tГотово! " + Environment.NewLine);
        //        //info1 = new UTF8Encoding(true).GetBytes(stattxt);
        //        //MetadataStatFile.Write(info1, 0, info1.Length);
        //        //MetadataStatFile.Close();
        //        if (!gotexifs)
        //        {
        //            strtowrite = imagefinfo.Name + "\t\t"
        //                         + DataValueISO.ToString() + "\t"
        //                         + DataValueFnumber.ToString() + "\t"
        //                         + DataValueExpTime.ToString() + "\t"
        //                         + ex3.ToString() + "\n";
        //            info2write = new UTF8Encoding(true).GetBytes(strtowrite);
        //            EXIFdataFile.Write(info2write, 0, info2write.Length);
        //            EXIFdataFile.Close();
        //        }
        //    }

        //    gotexifs = true;
        //    EXIFdataFile.Close();
        //}
        #endregion // private void GetEXIFs() - устарело, пока больше не применяется




        #region // устарело, пока больше не применяется
        //private int countrows(System.Data.DataTable table_in, string column_id, double filtervalue)
        //{
        //    DataRow[] filteredrows = table_in.Select("" + column_id + " = " + filtervalue.ToString());
        //    return filteredrows.Length;
        //}
        #endregion // устарело, пока больше не применяется



        #region // устарело, больше не применяется
        //private Color ColorScheme_m3(double si_m3, int callcase = 0)
        //{
        //    Color new_color3;
        //    Color colBlack = Color.FromArgb(0, 0, 0);
        //    Color colWhite = Color.FromArgb(255, 250, 250);
        //    Color colBlue = Color.FromArgb(0, 0, 128);

        //    new_color3 = colBlack;
        //    if (callcase == 0)
        //    {
        //        if ((si_m3 < tunedSIMargin) && (si_m3 > -1))
        //        {
        //            new_color3 = colWhite;
        //        }
        //        else
        //        {
        //            new_color3 = colBlue;
        //        }

        //        if (si_m3 == -2.0)
        //        {
        //            new_color3 = colBlack;
        //        }
        //    }
        //    else if (callcase == 1)
        //    {
        //        if (si_m3 == -2.0)
        //        {
        //            new_color3 = colBlack;
        //        }
        //        else if (si_m3 == 0.0)
        //        {
        //            new_color3 = colBlue;
        //        }
        //        else if (si_m3 == 1.0)
        //        {
        //            new_color3 = colWhite;
        //        }
        //    }

        //    return new_color3;
        //}


        //private Color ColorScheme_m4(double si_m4, int callcase = 0)
        //{
        //    Color new_color4;
        //    new_color4 = Color.FromArgb(0, 0, 0);


        //    if (callcase == 0)
        //    {
        //        if ((si_m4 < tunedIFMMargin) && (si_m4 > -1))
        //        {
        //            new_color4 = Color.FromArgb(0, 0, 128);
        //        }
        //        else
        //        {
        //            new_color4 = Color.FromArgb(255, 250, 250);
        //        }


        //        if (si_m4 == -2.0)
        //        {
        //            new_color4 = Color.FromArgb(0, 0, 0);
        //        }
        //    }
        //    else if (callcase == 1)
        //    {
        //        if (si_m4 == 0.0)
        //        {
        //            new_color4 = Color.FromArgb(0, 0, 128);
        //        }
        //        else if (si_m4 == 1.0)
        //        {
        //            new_color4 = Color.FromArgb(255, 250, 250);
        //        }
        //        else if (si_m4 == -2.0)
        //        {
        //            new_color4 = Color.FromArgb(0, 0, 0);
        //        }
        //    }


        //    return new_color4;
        //}
        #endregion // устарело, больше не применяется




        private void button1_Click_1(object sender, EventArgs e)
        {
            добавитьФайлToolStripMenuItem_Click(sender, e);
        }



        private void DumpKeyData()
        {
            string strtowrite;
            //byte[] info2write;

            string filename1 = ImageFileName;

            // imagetoadd = new Image<Bgr, byte>(filename1); // Image.FromFile(filename1);
            FileInfo imagefinfo = new FileInfo(filename1);

            string filenameCCdata = imagefinfo.DirectoryName + "\\result.dat";
            string filenameStatData1File = imagefinfo.DirectoryName + "\\StatData1.dat";


            //CCDataFile = new FileStream(filenameCCdata, FileMode.Append, FileAccess.Write);
            //StatDataFile1 = new FileStream(filenameStatData1File, FileMode.Append, FileAccess.Write);

            FileInfo finfo1 = new FileInfo(filenameCCdata);
            if (finfo1.Length == 0)
            {
                strtowrite = "CCv - cloud cover index" + Environment.NewLine
                            + "MVl = sky-cloud margin value" + Environment.NewLine
                            + "Filename\t\tMVlSI\tCCvSI\n";
                ServiceTools.logToTextFile(filenameCCdata, strtowrite, true);


                //+ "Filename\t\tMVlSI\tCCvSI\tMVlIFM\tCCvIFM\n";
                //info2write = new UTF8Encoding(true).GetBytes(strtowrite);
                //CCDataFile.Write(info2write, 0, info2write.Length);

                strtowrite = "SkClMV = sky-cloud margin values" + Environment.NewLine
                            + "data fields contains sky-index values by files for each SkClMV" + Environment.NewLine
                            + "SkClMV\t\t\t";
                for (int i = SIClScMarginRangeLowerValue; i < (SIClScMarginRangeHigherValue + 1); i++)
                {
                    double marginval = Math.Round((double)i / 100.0, 2);
                    strtowrite += marginval.ToString() + "\t";
                }
                strtowrite += Environment.NewLine + "Filename" + Environment.NewLine;
                //info2write = new UTF8Encoding(true).GetBytes(strtowrite);
                //StatDataFile1.Write(info2write, 0, info2write.Length);
                ServiceTools.logToTextFile(filenameStatData1File, strtowrite, true);
            }


            string new_filename3 = filename1.ToLower().Replace(".jpg", ".") + tunedSIMargin.ToString().Replace(",", "_") + ".SI.jpg";
            //string new_filename4 = filename1.ToLower().Replace(".jpg", ".") + tunedIFMMargin.ToString().Replace(",", "_") + ".IFM.jpg";
            bitmapSI.Save(new_filename3, System.Drawing.Imaging.ImageFormat.Jpeg);
            //bitmapIFM.Save(new_filename4, System.Drawing.Imaging.ImageFormat.Jpeg);

            FileInfo finfo = new FileInfo(filename1);
            string shortfname1 = finfo.Name;
            strtowrite = "" + shortfname1 + "\t"
                        + tunedSIMargin.ToString() + "\t"
                        + Math.Round(classificator.CloudCover, 2).ToString() + "\n";
            //+ Math.Round(CloudCover3, 2).ToString() + "\t"
            //+ tunedIFMMargin.ToString() + "\t"
            //+ Math.Round(CloudCover4, 2).ToString() + "\n";
            //info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            //CCDataFile.Write(info2write, 0, info2write.Length);
            ServiceTools.logToTextFile(filenameCCdata, strtowrite, true);


            if (filecounter_prev != FileCounter)
            {
                strtowrite = "\n" + shortfname1 + "\t";
                filecounter_prev = FileCounter;
            }
            else
            {
                strtowrite = "";
            }
            strtowrite += Math.Round(classificator.CloudCover, 3).ToString() + "\t";
            ServiceTools.logToTextFile(filenameStatData1File, strtowrite, true);

            //info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            //StatDataFile1.Write(info2write, 0, info2write.Length);

            //StatDataFile1.Close();
            //CCDataFile.Close();
        }







        #region button2_Click_1 - кнопка OK

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (imagetoadd == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Не загружено изображение для обработки!", true);
                //ThreadSafeOperations.SetTextTB(textBox1, "Не загружено изображение для обработки!", true);
                return;
            }

            theLogWindow = ServiceTools.LogAText(theLogWindow, "Started processing " + ImageFileName);

            AnalyzeImage(sender);

            //CurrentPB2Source = pb2Source.SkyindexAnalyzer;
        }

        #endregion button2_Click_1 - кнопка OK




        #region // private void GetEXIFs() - устарело, пока больше не применяется
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    GetEXIFs();
        //}
        #endregion // private void GetEXIFs() - устарело, пока больше не применяется





        public void Note(string text)
        {
            //ThreadSafeOperations.SetTextTB(textBox1, text + Environment.NewLine, true);
            theLogWindow = ServiceTools.LogAText(theLogWindow, text + Environment.NewLine, true);
            //textBox1.Text += text + Environment.NewLine;
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tunedSIMargin = (double)trackBar1.Value / 100.0;
            ThreadSafeOperations.SetText(label2, tunedSIMargin.ToString(), false);
            //ProcessingConditionsChanged = true;
        }




        #region // obsolete

        //private void trackBar2_Scroll(object sender, EventArgs e)
        //{
        //    double TrackBarValue;
        //    System.Windows.Forms.TrackBar MyTBIFM;
        //    MyTBIFM = (System.Windows.Forms.TrackBar)sender;
        //    TrackBarValue = (double)MyTBIFM.Value;
        //    tunedIFMMargin = TrackBarValue / 100.0;
        //    ThreadSafeOperations.SetText(label4, tunedIFMMargin.ToString(), false);
        //    //label4.Text = tunedIFMMargin.ToString();
        //}


        //private void button4_Click(object sender, EventArgs e)
        //{
        //    double perc_counter0, perc_counter1;
        //    double CurrSI;
        //    string strtowrite;
        //    byte[] info2write;
        //    Image thumb, res_thumbSI, curr_thumb;
        //    //Image res_thumbIFM;

        //    perc_counter0 = 0.0;
        //    counter1 = 0;
        //    perc_counter1 = 0.0;
        //    overallpixelcount = ImageWidthOrig * ImageHeightOrig;


        //    if(tunedSIMargin > tunedSIMargin_prev)
        //    {
        //        //новое значение выше старого - надо переключать с неба на облако всех, ког попадет в интервал
        //        for (int x = 0; x< ImageWidthOrig; x++)
        //            for (int y = 0; y < ImageHeightOrig; y++)
        //            {
        //                counter1++;
        //                if (!SIAccounting[x, y])
        //                {
        //                    continue;
        //                }

        //                CurrSI = SIdata[x,y];
        //                if ((CurrSI >= tunedSIMargin_prev) && (CurrSI <= tunedSIMargin))
        //                {
        //                    SICloudCondition[x, y] = true;
        //                    bitmapSI.SetPixel(x, y, ColorScheme_m3(1.0, 1));
        //                }

        //                perc_counter1 = 100 * counter1 / overallpixelcount;

        //                if (perc_counter1 - perc_counter0 >= 1.0)
        //                {
        //                    perc_counter0 = perc_counter1;
        //                    UpdateProgressBar(toolStripProgressBar1, (int)perc_counter1);
        //                    //toolStripProgressBar1.Value = (int)perc_counter1;

        //                    //перерисуем эскизы картинок-результатов
        //                    int remainder = 0;
        //                    int quotient = Math.DivRem((int)perc_counter0, 10, out remainder);
        //                    if (remainder == 0)
        //                    {
        //                        res_thumbSI = bitmapSI.GetThumbnailImage(th_widthSI, th_heightSI, null, IntPtr.Zero);
        //                        //pictureBox2.Image = res_thumbSI;
        //                        UpdatePictureBox(pictureBox2, res_thumbSI);
        //                    }
        //                    this.Refresh();
        //                }
        //            }
        //    }
        //    else if (tunedSIMargin < tunedSIMargin_prev)
        //    {
        //        //новое значение меньше старого - надо переключать с облака на небо всех, ког попадет в интервал
        //        for (int x = 0; x < ImageWidthOrig; x++)
        //            for (int y = 0; y < ImageHeightOrig; y++)
        //            {
        //                counter1++;
        //                if (!SIAccounting[x, y])
        //                {
        //                    continue;
        //                }
        //                CurrSI = SIdata[x, y];
        //                if ((CurrSI >= tunedSIMargin) && (CurrSI <= tunedSIMargin_prev))
        //                {
        //                    SICloudCondition[x, y] = false;
        //                    bitmapSI.SetPixel(x, y, ColorScheme_m3(0.0, 1));
        //                }

        //                perc_counter1 = 100 * counter1 / overallpixelcount;

        //                if (perc_counter1 - perc_counter0 >= 1.0)
        //                {
        //                    perc_counter0 = perc_counter1;
        //                    UpdateProgressBar(toolStripProgressBar1, (int)perc_counter1);
        //                    //toolStripProgressBar1.Value = (int)perc_counter1;

        //                    //перерисуем эскизы картинок-результатов
        //                    int remainder = 0;
        //                    int quotient = Math.DivRem((int)perc_counter0, 10, out remainder);
        //                    if (remainder == 0)
        //                    {
        //                        res_thumbSI = bitmapSI.GetThumbnailImage(th_widthSI, th_heightSI, null, IntPtr.Zero);
        //                        UpdatePictureBox(pictureBox2, res_thumbSI);
        //                        //pictureBox2.Image = res_thumbSI;
        //                    }
        //                    this.Refresh();
        //                }
        //            }
        //    }
        //    else
        //    {
        //        //do nothing
        //    }

        //    res_thumbSI = bitmapSI.GetThumbnailImage(th_widthSI, th_heightSI, null, IntPtr.Zero);
        //    UpdatePictureBox(pictureBox2, res_thumbSI);

        //    int SICloudCounter = 0;
        //    int SISkyCounter = 0;

        //    for(int x = 0; x < ImageWidthOrig; x++)
        //        for (int y = 0; y < ImageHeightOrig; y++)
        //        {
        //            if (SICloudCondition[x, y])
        //            {
        //                SICloudCounter++;
        //            }
        //            else
        //            {
        //                SISkyCounter++;
        //            }
        //        }


        //    CloudCover3 = (double)SICloudCounter / (double)(SICloudCounter + SISkyCounter);
        //    CloudCounterSI = SICloudCounter;
        //    SkyCounterSI = SISkyCounter;
        //    tunedSIMargin_prev = tunedSIMargin;

        //    UpdateSIIFMlabels(1, CloudCover3);
        //}



        //private void button5_Click(object sender, EventArgs e)
        //{
        //    double perc_counter0, perc_counter1;
        //    double CurrIFM;
        //    string strtowrite;
        //    byte[] info2write;
        //    Image thumb, res_thumbSI, res_thumbIFM, curr_thumb;

        //    perc_counter0 = 0.0;
        //    counter1 = 0;
        //    perc_counter1 = 0.0;
        //    overallpixelcount = ImageWidthOrig * ImageHeightOrig;


        //    if (tunedIFMMargin > tunedIFMMargin_prev)
        //    {
        //        for (int x = 0; x < ImageWidthOrig; x++)
        //            for (int y = 0; y < ImageHeightOrig; y++)
        //            {
        //                counter1++;
        //                if (!IFMAccounting[x, y])
        //                {
        //                    continue;
        //                }

        //                CurrIFM = IFMdata[x, y];
        //                if ((CurrIFM >= tunedIFMMargin_prev) && (CurrIFM <= tunedIFMMargin))
        //                {
        //                    IFMCloudCondition[x, y] = false;
        //                    bitmapIFM.SetPixel(x, y, ColorScheme_m4(0.0, 1));
        //                }

        //                perc_counter1 = 100 * counter1 / overallpixelcount;

        //                if (perc_counter1 - perc_counter0 >= 1.0)
        //                {
        //                    perc_counter0 = perc_counter1;
        //                    UpdateProgressBar(toolStripProgressBar1, (int)perc_counter1);
        //                    //toolStripProgressBar1.Value = (int)perc_counter1;

        //                    //перерисуем эскизы картинок-результатов
        //                    int remainder = 0;
        //                    int quotient = Math.DivRem((int)perc_counter0, 10, out remainder);
        //                    if (remainder == 0)
        //                    {
        //                        res_thumbIFM = bitmapIFM.GetThumbnailImage(th_widthIFM, th_heightIFM, null, IntPtr.Zero);
        //                        UpdatePictureBox(pictureBox3, res_thumbIFM);
        //                        //pictureBox3.Image = res_thumbIFM;
        //                    }
        //                    this.Refresh();
        //                }
        //            }
        //    }
        //    else if (tunedIFMMargin < tunedIFMMargin_prev)
        //    {
        //        for (int x = 0; x < ImageWidthOrig; x++)
        //            for (int y = 0; y < ImageHeightOrig; y++)
        //            {
        //                counter1++;
        //                if (!IFMAccounting[x, y])
        //                {
        //                    continue;
        //                }
        //                CurrIFM = IFMdata[x, y];
        //                if ((CurrIFM >= tunedIFMMargin) && (CurrIFM <= tunedIFMMargin_prev))
        //                {
        //                    IFMCloudCondition[x, y] = true;
        //                    bitmapIFM.SetPixel(x, y, ColorScheme_m4(1.0, 1));
        //                }

        //                perc_counter1 = 100 * counter1 / overallpixelcount;

        //                if (perc_counter1 - perc_counter0 >= 1.0)
        //                {
        //                    perc_counter0 = perc_counter1;
        //                    UpdateProgressBar(toolStripProgressBar1, (int)perc_counter1);
        //                    //toolStripProgressBar1.Value = (int)perc_counter1;

        //                    //перерисуем эскизы картинок-результатов
        //                    int remainder = 0;
        //                    int quotient = Math.DivRem((int)perc_counter0, 10, out remainder);
        //                    if (remainder == 0)
        //                    {
        //                        res_thumbIFM = bitmapIFM.GetThumbnailImage(th_widthIFM, th_heightIFM, null, IntPtr.Zero);
        //                        UpdatePictureBox(pictureBox3, res_thumbIFM);
        //                        //pictureBox3.Image = res_thumbIFM;
        //                    }
        //                    this.Refresh();
        //                }
        //            }
        //    }
        //    else
        //    {
        //        //do nothing
        //    }

        //    res_thumbIFM = bitmapIFM.GetThumbnailImage(th_widthIFM, th_heightIFM, null, IntPtr.Zero);
        //    UpdatePictureBox(pictureBox3, res_thumbIFM);
        //    //pictureBox3.Image = res_thumbIFM;


        //    //string filename1 = ImageFileName;
        //    //string new_filename4 = filename1 + ".IFM.jpg";
        //    //string filnameCCdata = filename1 + ".result_CCdata.dat";
        //    //FileInfo imagefinfo = new FileInfo(filename1);
        //    //string filenameCCdata = imagefinfo.DirectoryName + "\\result.dat";
        //    //CCDataFile = new FileStream(filenameCCdata, FileMode.Append, FileAccess.Write);

        //    //FileInfo finfo1 = new FileInfo(filenameCCdata);
        //    //if (finfo1.Length == 0)
        //    //{
        //    //    strtowrite = "CCv - cloud cover index" + Environment.NewLine
        //    //                + "MVl = sky-cloud margin value" + Environment.NewLine
        //    //                + "Filename\t\tMVlSI\tCCvSI\tMVlIFM\tCCvIFM\n";
        //    //    info2write = new UTF8Encoding(true).GetBytes(strtowrite);
        //    //    CCDataFile.Write(info2write, 0, info2write.Length);
        //    //}

        //    //CCDataFile = new FileStream(filenameCCdata, FileMode.Append, FileAccess.Write);
        //    //strtowrite = "CCn - Cloud counter\nSCn - Sky counter\nCCv - cloud cover index\nCCn1\tSCn1\tCCn2\tSC2\tCCv1\tCCv2\n";
        //    //info2write = new UTF8Encoding(true).GetBytes(strtowrite);
        //    //CCDataFile.Write(info2write, 0, info2write.Length);

        //    int IFMCloudCounter = 0;
        //    int IFMSkyCounter = 0;

        //    for (int x = 0; x < ImageWidthOrig; x++)
        //        for (int y = 0; y < ImageHeightOrig; y++)
        //        {
        //            if (IFMCloudCondition[x, y])
        //            {
        //                IFMCloudCounter++;
        //            }
        //            else
        //            {
        //                IFMSkyCounter++;
        //            }
        //        }


        //    //bitmapIFM.Save(new_filename4, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    //ThreadSafeOperations.SetText(toolStripStatusLabel1, "Готово");
        //    ////toolStripStatusLabel1.Text = "Готово";
        //    //toolStripProgressBar1.Value = 0;
        //    //ThreadSafeOperations.SetTextTB(textBox1, "Записаны результирующие файлы " + Environment.NewLine + new_filename4 + Environment.NewLine);
        //    ////textBox1.Text = "Записаны результирующие файлы " + Environment.NewLine + new_filename4 + Environment.NewLine;

        //    //new_filename4 = filename1 + ".IFM.dat";
        //    //OutputFile4 = new FileStream(new_filename4, FileMode.Create, FileAccess.Write);

        //    CloudCover4 = (double)IFMCloudCounter / (double)(IFMCloudCounter + IFMSkyCounter);
        //    //string strtowrite4 = "Cloud counter (IFM) = " + IFMCloudCounter.ToString() + Environment.NewLine;
        //    //strtowrite4 += "Sky counter (IFM) = " + IFMSkyCounter.ToString() + Environment.NewLine;
        //    //strtowrite4 += "Cloud cover (IFM) = " + Math.Round(CloudCover4, 2).ToString() + Environment.NewLine;
        //    //byte[] info4 = new UTF8Encoding(true).GetBytes(strtowrite4);
        //    //OutputFile4.Write(info4, 0, info4.Length);


        //    //FileInfo finfo = new FileInfo(filename1);
        //    //string shortfname1 = finfo.Name;
        //    //strtowrite = "" + shortfname1 + "\t"
        //    //            + tunedSIMargin.ToString() + "\t"
        //    //            + Math.Round(CloudCover3, 2).ToString() + "\t"
        //    //            + tunedIFMMargin.ToString() + "\t"
        //    //            + Math.Round(CloudCover4, 2).ToString() + "\n";
        //    //info2write = new UTF8Encoding(true).GetBytes(strtowrite);
        //    //CCDataFile.Write(info2write, 0, info2write.Length);

        //    //strtowrite = "" + CloudCounterSI.ToString() + "\t" + SkyCounterSI.ToString() + "\t" + IFMCloudCounter.ToString() + "\t" + IFMSkyCounter.ToString() + "\t" + Math.Round(CloudCover3, 2).ToString() + "\t" + Math.Round(CloudCover4, 2).ToString() + "\n";
        //    CloudCounterIFM = IFMCloudCounter;
        //    SkyCounterIFM = IFMSkyCounter;
        //    //OutputFile4.Close();
        //    //CCDataFile.Close();
        //    tunedIFMMargin_prev = tunedIFMMargin;

        //    UpdateSIIFMlabels(2, CloudCover4);
        //}

        #endregion // obsolete





        #region Form behaviour

        private void Form1_DragEnter(object sender, DragEventArgs e)
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



        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array FilesArray = (Array)e.Data.GetData(DataFormats.FileDrop);

                if (FilesArray != null)
                {
                    foreach (string FileName1 in FilesArray)
                    {
                        //ProcessingConditionsChanged = false;
                        this.BeginInvoke(m_DelegateOpenFile, new Object[] { FileName1 });
                        this.Activate();
                        break;
                    }

                }
            }
            catch (Exception exc1)
            {
                timetotal = DateTime.Now - begintotal;
                //ThreadSafeOperations.SetTextTB(textBox1, "Ошибка при обработке Drag&Drop: " + exc1.Message + timetotal, true);
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Ошибка при обработке Drag&Drop: " + exc1.Message + timetotal, true);
                //textBox1.Text += "Ошибка при обработке Drag&Drop: " + exc1.Message + timetotal;
            }
        }


        #endregion Form behaviour






        #region // obsolete

        //private void getDataToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    bool _continue = true;
        //    string message = "";
        //    SerialPort sp = new SerialPort();
        //    sp.PortName = "COM2";
        //    int counter = 0;

        //    sp.ReadTimeout = 500;
        //    try
        //    {
        //        sp.Open();
        //    }
        //    catch (Exception)
        //    {
        //        ThreadSafeOperations.SetTextTB(textBox1, "Не удалось открыть COM-порт: " + Environment.NewLine, true);
        //        return;
        //    }



        //    while (_continue)
        //    {
        //        try
        //        {
        //            message = sp.ReadLine() + Environment.NewLine + sp.ReadLine() + Environment.NewLine + sp.ReadLine() + Environment.NewLine;

        //            _continue = false;
        //        }
        //        catch (TimeoutException)
        //        {
        //            ThreadSafeOperations.SetTextTB(textBox1, "havent got data yet: " + (counter * 500).ToString() + Environment.NewLine, true);
        //            counter++;
        //        }
        //        ThreadSafeOperations.SetTextTB(textBox1, message + Environment.NewLine, true);
        //        if (counter == 20) _continue = false;
        //    }
        //    sp.Close();
        //}

        #endregion // obsolete



        #region bgwProcessOneImage

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker SelfWorker = sender as BackgroundWorker;

            AnalyzeImage();
            if (SelfWorker.CancellationPending)
            {
                e.Cancel = true;
            }

            DumpKeyData();
            //ProcessButtonPushedOnceThisFile = true;
        }


        // bgwProcessOneImage - RunWorkerCompleted
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //ThreadSafeOperations.SetTextTB(textBox1, "Ошибка! " + e.Error.Message + Environment.NewLine, true);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Ошибка! " + e.Error.Message + Environment.NewLine,
                    true);
            }
            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "операция завершена" + Environment.NewLine + "------------------" + Environment.NewLine, true);
            //ThreadSafeOperations.SetTextTB(textBox1, "операция завершена" + Environment.NewLine + "------------------" + Environment.NewLine, true);
            //SetButtonEnabledStatus(обработатьToolStripMenuItem, true);
            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);


            classificator.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            ThreadSafeOperations.ToggleButtonState(обработатьToolStripMenuItem, true, "Обработать", false);
            ThreadSafeOperations.ToggleButtonState(button4, true, "OK", false);
        }

        //bgwProcessOneImage - ProgressChanged
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, e.ProgressPercentage);

            string statusMessage = (string)e.UserState;
            if (statusMessage != "")
            {
                Note(statusMessage);
            }

        }

        #endregion bgwProcessOneImage





        #region // obsolete

        //private void GetSensorsDataCycle_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        //{
        //    int result, remainder;
        //    System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
        //    while (true)
        //    {
        //        SensorsCycleDataGotTimes++;
        //        //result = Math.DivRem((int)SensorsCycleDataGotTimes, (int)10, out remainder);
        //        //if (remainder == 0)
        //        //{
        //            GetSensorsDataCycle.ReportProgress(SensorsCycleDataGotTimes);
        //        //}

        //        if (SelfWorker.CancellationPending)
        //        {
        //            e.Cancel = true;
        //            break;
        //        }
        //        System.Threading.Thread.Sleep(500);
        //        getDataToolStripMenuItem_Click(null, null);
        //    }
        //}

        //private void GetSensorsDataCycle_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        //{
        //    ThreadSafeOperations.SetText(StatusLabel, "Данные датчиков собраны " + e.ProgressPercentage + " раз", false);
        //    //StatusLabel.Text = "Данные датчиков собраны " + e.ProgressPercentage + " раз";
        //}

        //private void GetSensorsDataCycle_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        //{
        //    ThreadSafeOperations.SetText(StatusLabel, "Получение данных сенсоров остановлено", false);
        //    //StatusLabel.Text = "Получение данных сенсоров остановлено";
        //}

        //private void SwitchCollectingDataMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (GetSensorsDataCycle.IsBusy)
        //    {
        //        GetSensorsDataCycle.CancelAsync();
        //        switchCollectingDataMenuItem.Text = "Начать сбор данных всех датчиков";
        //    }
        //    else
        //    {
        //        GetSensorsDataCycle.RunWorkerAsync();
        //        switchCollectingDataMenuItem.Text = "Остановить сбор данных всех датчиков";
        //    }

        //}




        //private void Form1_Paint(object sender, PaintEventArgs e)
        //{
        //    //if (GetSensorsDataCycle.IsBusy)
        //    //{
        //    //    ThreadSafeOperations.SetText(StatusLabel, "Идет сбор данных датчиков.", false);
        //    //}
        //    //else
        //    //{
        //    //    ThreadSafeOperations.SetText(StatusLabel, "Сбор данных датчиков не производится.", false);
        //    //}
        //}

        //private void trackBar3_Scroll(object sender, EventArgs e)
        //{
        //    double TrackBarValue;
        //    double divider;
        //    System.Windows.Forms.TrackBar MyTB;

        //    //ProcessingConditionsChanged = true;

        //    MyTB = (System.Windows.Forms.TrackBar)sender;
        //    TrackBarValue = (double)MyTB.Value;
        //    divider = Math.Pow(2.0, (double)(10 - TrackBarValue));
        //    resizevalue = 1.0 / divider;
        //    //ThreadSafeOperations.SetText(label7, resizevalue.ToString());
        //    ThreadSafeOperations.SetText(label9, "Изменять исходный размер: " + resizevalue.ToString(), false);
        //    //label7.Text = resizevalue.ToString();
        //    if (ImageFileName != null)
        //    {
        //        OpenFile(ImageFileName);
        //    }
        //}


        //private Image ImageResizer(Image imgToResize, int width, int height)
        //{
        //    int sourceWidth = imgToResize.Width;
        //    int sourceHeight = imgToResize.Height;
        //
        //    Bitmap b = new Bitmap(width, height);
        //    Graphics g = Graphics.FromImage((Image)b);
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //
        //    g.DrawImage(imgToResize, 0, 0, width, height);
        //    g.Dispose();
        //
        //    b = ImageProcessing.SquareImageDimensions(b);
        //
        //    return (Image)b;
        //}

        #endregion // obsolete




        #region bgwProcessDirectoryOfImages

        private void button1_Click(object sender, EventArgs e)
        {
            string path2process = (string)defaultProperties["BatchProcessingDirectory"];
            if (path2process == "")
            {
                path2process = Directory.GetCurrentDirectory();
            }

            DirectoryInfo dir = new DirectoryInfo(path2process);

            if (bgwProcessDirectoryOfImages.IsBusy)
            {
                bgwProcessDirectoryOfImages.CancelAsync();
            }
            else
            {
                if (!dir.Exists)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Операция не выполнена. Не найдена директория:" + Environment.NewLine +
                        path2process + Environment.NewLine, true);
                    //ThreadSafeOperations.SetTextTB(textBox1, textBox1.Text + "Операция не выполнена. Не найдена директория:" + Environment.NewLine + path2process + Environment.NewLine, true);
                    return;
                }

                FileInfo[] fileList2Process = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
                if (fileList2Process.Length == 0)
                {
                    Note("Coudn't find any *.jpg file in processing directiory or its subdirectories: \"" + path2process + "\" ");
                    return;
                }



                //object[] BGWorker2Args = new object[] { trackBar3.Value };
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Multiple images computing started with the following parameters:");
                string strToDisplay = CommonTools.DictionaryRepresentation(defaultProperties);
                //var propertiesObj = Properties.Settings.Default.Properties;
                //foreach (KeyValuePair<string, object> settingsProperty in defaultProperties)
                //{
                //    //strToDisplay += Environment.NewLine + settingsProperty.Name + " = " + settingsProperty.DefaultValue;
                //    strToDisplay += Environment.NewLine + settingsProperty.Key + " = " + settingsProperty.Value;
                //}
                theLogWindow = ServiceTools.LogAText(theLogWindow, strToDisplay);

                simpleMultipleImagesShow imagesRepresentingForm = new simpleMultipleImagesShow();
                imagesRepresentingForm.Show();

                List<ClassificationMethods> schemesToUse = new List<ClassificationMethods>();
                schemesToUse.Add(ClassificationMethods.Japan);
                schemesToUse.Add(ClassificationMethods.US);
                schemesToUse.Add(ClassificationMethods.GrIx);

                object[] BGWorker2Args = new object[] { path2process, theLogWindow, imagesRepresentingForm, schemesToUse };

                //ThreadSafeOperations.SetButtonEnabledStatus(открытьФайлToolStripMenuItem, false);
                //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar3, false);
                //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar4, false);
                //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar5, false);



                //SIClScMarginRangeLowerValue = trackBar4.Value;
                //SIClScMarginRangeHigherValue = trackBar5.Value;

                ThreadSafeOperations.ToggleButtonState(button1, true, "Прекратить обработку", true);

                bgwProcessDirectoryOfImages.RunWorkerAsync(BGWorker2Args);
            }
        }



        /// <summary>
        /// Handles the DoWork event of the bgwProcessDirectoryOfImages.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object[] arguments = e.Argument as object[];
            //double ProgressPerc;
            //int SIValueCounter = 5;
            //int SIValueRange = 25, SIValueRange_min = 5, SIValueRange_max = 30;
            double defaultResizeFactor = Convert.ToDouble(defaultProperties["DefaultScalingFactor"]); ;
            int defaultMaxImageSize = Convert.ToInt32(defaultProperties["DefaultMaxImageSize"]); ;
            Bitmap bmJ = null, bmG = null, bmGrIx = null; //, bitmap2process = null;

            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            string outputStatsDirectory = (string)defaultProperties["DefaultDataFilesLocation"];

            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);

            //SIValueRange_min = SIClScMarginRangeLowerValue;
            //SIValueRange_max = SIClScMarginRangeHigherValue;
            //SIValueRange = SIValueRange_max - SIValueRange_min;

            FileCounter = 0; filecounter_prev = 0;

            string path2process = (string)arguments[0];
            LogWindow theLogWindow = (LogWindow)arguments[1];
            simpleMultipleImagesShow imagesRepresentingForm = (simpleMultipleImagesShow)arguments[2];
            List<ClassificationMethods> schemesToUse = (List<ClassificationMethods>)arguments[3];
            Type theType = imagesRepresentingForm.GetType();
            MethodInfo thePicturePlacingMethodInfo = theType.GetMethod("PlaceAPicture");


            //ServiceTools.logToTextFile(outputStatsDirectory + "statsWithFNames.dat", "Full filename | date+time picture taken | SI US | SI Jap | SI GrIx | SB suppr | Sun disk condition " + Environment.NewLine, true);
            ServiceTools.logToTextFile(outputStatsDirectory + "statsWithFNames.dat", "Full filename | date+time picture taken | SI US | SI Jap | SI GrIx | SB suppr " + Environment.NewLine, true);


            DirectoryInfo dir = new DirectoryInfo(path2process);
            FileInfo[] fileList2Process = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
            int filecount = 0;
            foreach (FileInfo fInfo in fileList2Process)
            {
                filecount++;
                int progressPercentage = Convert.ToInt32(((double)filecount / (double)fileList2Process.Length));
                SelfWorker.ReportProgress(progressPercentage);


                theLogWindow = ServiceTools.LogAText(theLogWindow, fInfo.FullName + "   (" + filecount + "//" + fileList2Process.Length + ")");

                string strToDisplay = "";

                ImageInfo imInfo = new ImageInfo(Image.FromFile(fInfo.FullName));
                //String dateTime = (String)imInfo.getValueByKey("DateTime");
                String dateTime = (String)imInfo.getValueByKey("ExifDTOrig");

                if (dateTime == null)
                {
                    //попробуем вытащить из имени файла
                    string strDateTime = fInfo.Name;
                    strDateTime = strDateTime.Substring(4, 19);
                    dateTime = strDateTime;
                }

                strToDisplay += dateTime;

                Image<Bgr, Byte> img2process = new Image<Bgr, byte>(fInfo.FullName);

                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.Japan) != -1)
                {
                    ThreadSafeOperations.UpdatePictureBox(pictureBox1, img2process.Bitmap, true);
                    img2process = ImageProcessing.ImageResizer(img2process, defaultMaxImageSize);

                    classificator = new SkyCloudClassification(img2process, defaultProperties);
                    classificator.LogWindow = theLogWindow;
                    classificator.ParentForm = this;
                    classificator.ClassificationMethod = ClassificationMethods.Japan;
                    classificator.isCalculatingUsingBgWorker = true;
                    classificator.SelfWorker = sender as BackgroundWorker;
                    classificator.defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
                    classificator.cloudSkySeparationValue =
                        Convert.ToDouble(defaultProperties["JapanCloudSkySeparationValue"]);
                    classificator.sourceImageFileName = fInfo.FullName;
                    try
                    {
                        classificator.Classify();
                    }
                    catch (Exception ex)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            Environment.NewLine + Environment.NewLine + "=== === ERROR classifying with method: " +
                            classificator.ClassificationMethod + " === ===" + Environment.NewLine + Environment.NewLine);
                    }
                    classificator.resultingStatusMessages = "";
                    bitmapSI = new Bitmap(classificator.PreviewBitmap);
                    ServiceTools.FlushMemory();
                    ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                    pictureBox2Bitmap = new Bitmap(bitmapSI);
                    strToDisplay += " | " + Math.Round(classificator.CloudCover, 2).ToString();
                    UpdateSIIFMlabels(1, classificator.CloudCover);
                    bmJ = new Bitmap(bitmapSI);
                }





                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.US) != -1)
                {
                    classificator = new SkyCloudClassification(img2process, defaultProperties);
                    classificator.LogWindow = theLogWindow;
                    classificator.ParentForm = this;
                    classificator.ClassificationMethod = ClassificationMethods.US;
                    classificator.isCalculatingUsingBgWorker = true;
                    classificator.SelfWorker = sender as BackgroundWorker;
                    classificator.defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
                    classificator.cloudSkySeparationValue =
                        Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]);
                    classificator.sourceImageFileName = fInfo.FullName;
                    try
                    {
                        classificator.Classify();
                    }
                    catch (Exception ex)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            Environment.NewLine + Environment.NewLine + "=== === ERROR classifying with method: " +
                            classificator.ClassificationMethod + " === ===" + Environment.NewLine + Environment.NewLine);
                    }
                    classificator.resultingStatusMessages = "";
                    bitmapSI = new Bitmap(classificator.PreviewBitmap);
                    ServiceTools.FlushMemory();
                    ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                    pictureBox2Bitmap = new Bitmap(bitmapSI);
                    strToDisplay += " | " + Math.Round(classificator.CloudCover, 2).ToString();
                    UpdateSIIFMlabels(1, classificator.CloudCover);

                    bmG = new Bitmap(bitmapSI);
                }


                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.GrIx) != -1)
                {
                    classificator = new SkyCloudClassification(img2process, defaultProperties);
                    classificator.LogWindow = theLogWindow;
                    classificator.ParentForm = this;
                    classificator.ClassificationMethod = ClassificationMethods.GrIx;
                    classificator.forceExistingSunInformation = cbxForceExistingSunInformation.Checked;
                    classificator.isCalculatingUsingBgWorker = true;
                    classificator.SelfWorker = sender as BackgroundWorker;
                    classificator.defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
                    classificator.cloudSkySeparationValue =
                        Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]);
                    classificator.theStdDevMarginValueDefiningSkyCloudSeparation =
                        Convert.ToDouble(defaultProperties["GrIxDefaultSkyCloudMarginWithoutSun"]);
                    classificator.sourceImageFileName = fInfo.FullName;
                    try
                    {
                        classificator.Classify();
                    }
                    catch (Exception ex)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            Environment.NewLine + Environment.NewLine + "=== === ERROR classifying with method: " +
                            classificator.ClassificationMethod + " === ===" + Environment.NewLine + Environment.NewLine);
                    }
                    classificator.resultingStatusMessages = "";
                    bitmapSI = new Bitmap(classificator.PreviewBitmap);
                    ServiceTools.FlushMemory();
                    ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                    pictureBox2Bitmap = new Bitmap(bitmapSI);
                    strToDisplay += " | " + Math.Round(classificator.CloudCover, 2).ToString();
                    // strToDisplay += " | " + classificator.currentSunDiskCondition.ToString();
                    UpdateSIIFMlabels(1, classificator.CloudCover);
                    bmGrIx = new Bitmap(bitmapSI);
                }

                int verbosityLevel = Convert.ToInt32(defaultProperties["GrIxProcessingVerbosityLevel"]);
                string randomFileName = classificator.randomFileName;
                string outputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
                if (verbosityLevel > 1)
                {
                    //сохраним картинки
                    string sourceFName = outputDataDirectory + randomFileName + "_SourceImage.jpg";
                    string japanFName = outputDataDirectory + randomFileName + "_si_jap.jpg";
                    string germanFName = outputDataDirectory + randomFileName + "_si_US.jpg";
                    string GrIxFName = outputDataDirectory + randomFileName + "_si_GrIx.jpg";

                    img2process.Save(sourceFName);
                    bmJ.Save(japanFName, ImageFormat.Jpeg);
                    bmG.Save(germanFName, ImageFormat.Jpeg);
                    bmGrIx.Save(GrIxFName, ImageFormat.Jpeg);

                }


                object[] parametersArray = new object[] { img2process.Bitmap, fInfo.Name, 1 };
                thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);

                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.Japan) != -1)
                {
                    parametersArray = new object[] { bmJ, "Japan", 2 };
                    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                }

                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.US) != -1)
                {
                    parametersArray = new object[] { bmG, "US", 3 };
                    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                }

                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.GrIx) != -1)
                {
                    parametersArray = new object[] { bmGrIx, "GrIx", 4 };
                    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                }


                theLogWindow = ServiceTools.LogAText(theLogWindow, strToDisplay);
                ServiceTools.logToTextFile(outputStatsDirectory + "statsWithFNames.dat",
                    fInfo.FullName + " | " + strToDisplay + " | " + classificator.theSunSuppressionSchemeApplicable.ToString() + Environment.NewLine, true);
                ServiceTools.logToTextFile(outputStatsDirectory + "stats.dat", strToDisplay + Environment.NewLine, true);


                if (SelfWorker.CancellationPending)
                {
                    break;
                }
            }


            theLogWindow = ServiceTools.LogAText(theLogWindow, "TOTAL files count: " + fileList2Process.Length);
        }





        /// <summary>
        /// Handles the ProgressChanged event of the bgwProcessDirectoryOfImages.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, e.ProgressPercentage);
        }




        /// <summary>
        /// Handles the RunWorkerCompleted event of the bgwProcessDirectoryOfImages.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.SetButtonEnabledStatus(открытьФайлToolStripMenuItem, true);
            ThreadSafeOperations.SetButtonEnabledStatus(обработатьToolStripMenuItem, true);
            //ThreadSafeOperations.SetButtonEnabledStatus(eXIFыToolStripMenuItem, true);

            //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar3, true);
            //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar4, true);
            //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar5, true);

            string CurDir = Directory.GetCurrentDirectory();
            string path2process = CurDir;

            ThreadSafeOperations.ToggleButtonState(button1, true, "Обработка директории: " + path2process, false);

            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);
            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);
        }

        #endregion bgwProcessDirectoryOfImages






        #region obsolete // trackBar4_Scroll(object sender, EventArgs e)
        //private void trackBar4_Scroll(object sender, EventArgs e)
        //{
        //    //double TrackBarLValue, TrackBarRValue;
        //    //TrackBarLValue = (double)trackBar4.Value;
        //    //TrackBarRValue = (double)trackBar5.Value;
        //    //ThreadSafeOperations.SetText(label10, (TrackBarLValue / 100.0).ToString() + " - " + (TrackBarRValue / 100.0).ToString(), false);
        //    //ProcessingConditionsChanged = true;
        //}
        #endregion obsolete // trackBar4_Scroll(object sender, EventArgs e)
        #region obsolete // настройкиСбораДанныхToolStripMenuItem_Click

        //private void настройкиСбораДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    //SkyIndexAnalyzerDataCollector CollectorForm = new SkyIndexAnalyzerDataCollector();
        //    //CollectorForm.Show();
        //    //ShowImageForm ImgShow = new ShowImageForm(localPreviewBitmap, ParentForm, this);
        //    //ImgShow.Show();
        //}

        #endregion obsolete // настройкиСбораДанныхToolStripMenuItem_Click



        private void btnMarkOctas_Click(object sender, EventArgs e)
        {
            if (imagetoadd == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Не загружено изображение для обработки!" + Environment.NewLine, true);
                //ThreadSafeOperations.SetTextTB(textBox1, "Не загружено изображение для обработки!", true);
                return;
            }

            List<Tuple<string, string>> lImagesRoundMasksMappingFiles = null;
            if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
            {
                string ImagesRoundMasksXMLfilesMappingList = (string)defaultProperties["ImagesRoundMasksXMLfilesMappingList"];
                if (File.Exists(ImagesRoundMasksXMLfilesMappingList))
                {
                    List<List<string>> llImagesRoundMasksMappingFiles =
                        ServiceTools.ReadDataFromCSV(ImagesRoundMasksXMLfilesMappingList, 0, true, ";", Environment.NewLine);
                    lImagesRoundMasksMappingFiles =
                        llImagesRoundMasksMappingFiles.ConvertAll(
                            list => new Tuple<string, string>(list[0], list[1]));
                    // item1: images filename pattern
                    // item2: image rounded mask parameters XML file
                }
            }

            RoundData predefinedRoundedMask = null;
            if (lImagesRoundMasksMappingFiles != null)
            {
                #region debug report
#if (MONO && DEBUG)
                    string strReport = "lImagesRoundMasksMappingFiles = " + lImagesRoundMasksMappingFiles;
                    if (logFileName == "")
                    {
                        Console.WriteLine(strReport);
                    }
                    else
                    {
                        ServiceTools.logToTextFile(logFileName, strReport + Environment.NewLine, true);
                    }
#endif
                #endregion debug report

                if (lImagesRoundMasksMappingFiles.Any())
                {
                    #region debug report
#if (MONO && DEBUG)
                        strReport = "lImagesRoundMasksMappingFiles count = " + lImagesRoundMasksMappingFiles.Count;
                        if (logFileName == "")
                        {
                            Console.WriteLine(strReport);
                        }
                        else
                        {
                            ServiceTools.logToTextFile(logFileName, strReport + Environment.NewLine, true);
                        }
#endif
                    #endregion debug report

                    if (lImagesRoundMasksMappingFiles.Find(tpl => (new WildcardPattern(tpl.Item1)).IsMatch(ImageFileName)) != null)
                    {
                        #region debug report
#if (MONO && DEBUG)
                            strReport = "wildcard pattern to match: " + currentFullFileName;
                            if (logFileName == "")
                            {
                                Console.WriteLine(strReport);
                            }
                            else
                            {
                                ServiceTools.logToTextFile(logFileName, strReport + Environment.NewLine, true);
                            }
#endif
                        #endregion debug report

                        string strFoundPredefinedRoundedMaskParametersXMLfile =
                            lImagesRoundMasksMappingFiles.Find(
                                tpl => (new WildcardPattern(tpl.Item1)).IsMatch(ImageFileName)).Item2;
                        strFoundPredefinedRoundedMaskParametersXMLfile =
                            strFoundPredefinedRoundedMaskParametersXMLfile.Substring(0, strFoundPredefinedRoundedMaskParametersXMLfile.IndexOf(".xml") + 4);


                        #region debug report
#if (MONO && DEBUG)
                            strReport = "found strFoundPredefinedRoundedMaskParametersXMLfile? " +
                                        strFoundPredefinedRoundedMaskParametersXMLfile + Environment.NewLine +
                                        "strFoundPredefinedRoundedMaskParametersXMLfile exists? " +
                                        File.Exists(strFoundPredefinedRoundedMaskParametersXMLfile);
                            if (logFileName == "")
                            {
                                Console.WriteLine(strReport);
                            }
                            else
                            {
                                ServiceTools.logToTextFile(logFileName, strReport + Environment.NewLine, true);
                            }
#endif
                        #endregion debug report

                        predefinedRoundedMask =
                            ServiceTools.ReadObjectFromXML(strFoundPredefinedRoundedMaskParametersXMLfile,
                                typeof(RoundData)) as RoundData;

                        #region debug report
#if (MONO && DEBUG)
                            strReport = "read predefinedRoundedMask? " + (predefinedRoundedMask != null);
                            if (logFileName == "")
                            {
                                Console.WriteLine(strReport);
                            }
                            else
                            {
                                ServiceTools.logToTextFile(logFileName, strReport + Environment.NewLine, true);
                            }
#endif
                        #endregion debug report
                    }
                }
            }


            ImageProcessing imp = null;
            if (predefinedRoundedMask == null)
            {
                imp = new ImageProcessing(imagetoadd, true);
            }
            else
            {
                imp = new ImageProcessing(imagetoadd, predefinedRoundedMask);
            }
            
            ServiceTools.FlushMemory(null, "");
            pictureBox2Bitmap = new Bitmap(imp.significantMaskImageOctLined.Bitmap);
            Image img2show = (Image)imp.significantMaskImageOctLined.Bitmap;
            ThreadSafeOperations.UpdatePictureBox(pictureBox2, img2show, true);


            if (predefinedRoundedMask == null)
            {
                string strGeometryFilename = (string)defaultProperties["DefaultDataFilesLocation"];
                strGeometryFilename = strGeometryFilename + ((strGeometryFilename.Last() == Path.DirectorySeparatorChar)
                    ? ("")
                    : (Path.DirectorySeparatorChar.ToString())) + Path.GetFileNameWithoutExtension(ImageFileName) + "-RoundImagemask.xml";
                ServiceTools.WriteObjectToXML(imp.imageRD, strGeometryFilename);
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "image geometry has been written to the file " + strGeometryFilename);
            }
        }







        #region Single sky-image analysis using bgwProcessOneImage !!!!!!!!!! Тут какая-то путаница с использованием разных воркеров. Разобраться и исправить.

        /// TODO: Тут какая-то путаница с использованием разных воркеров. Разобраться и исправить.
        /// 

        private void AnalyzeImage(object sender = null)
        {
            begintotal = DateTime.Now;


            #region //XLS dumping
            //XLdumper.doubleArrayToDump = SIdataDouble;
            //XLdumper.DumpData("SIdatabyHands");
            //XLdumper.doubleArrayToDump = currImageBlueChannelMap;
            //XLdumper.DumpData("byHandsCurrImageBlueChannelMap");
            //XLdumper.doubleArrayToDump = currImageRedChannelMap;
            //XLdumper.DumpData("byHandsCurrImageRedChannelMap");
            //XLdumper.doubleArrayToDump = currImageGreenChannelMap;
            //XLdumper.DumpData("byHandsCurrImageGreenChannelMap");
            //SIdataDouble = null;
            //currImageBlueChannelMap = null;
            //currImageRedChannelMap = null;
            #endregion


            DateTime beginNew = DateTime.Now;
            //classificator = new SkyCloudClassification(imagetoadd, pictureBox2, pbUniversalProgressBar, textBox1);

            FileInfo finfoOriginalFile = new FileInfo(ImageFileName);

            if (defaultProperties.ContainsKey("DefaultDataFilesLocationCurrentFile"))
            {
                defaultProperties["DefaultDataFilesLocationCurrentFile"] =
                    defaultProperties["DefaultDataFilesLocation"] +
                    Path.GetFileNameWithoutExtension(finfoOriginalFile.Name) +
                    Path.DirectorySeparatorChar;
            }
            else
            {
                defaultProperties.Add("DefaultDataFilesLocationCurrentFile",
                    defaultProperties["DefaultDataFilesLocation"] +
                    Path.GetFileNameWithoutExtension(finfoOriginalFile.Name) +
                    Path.DirectorySeparatorChar);
            }


            classificator = new SkyCloudClassification(imagetoadd, defaultProperties);
            classificator.cloudSkySeparationValue = tunedSIMargin;
            if (rbtnClassMethodJapan.Checked)
            {
                classificator.ClassificationMethod = ClassificationMethods.Japan;
            }
            else if (rbtnClassMethodUS.Checked)
            {
                classificator.ClassificationMethod = ClassificationMethods.US;
            }
            else if (rbtnClassMethodGrIx.Checked)
            {
                classificator.ParentForm = this;
                classificator.ClassificationMethod = ClassificationMethods.GrIx;

                //classificator.theStdDevMarginValueDefiningSkyCloudSeparation = tunedSIMargin;
            }

            if (bgwProcessOneImage.IsBusy)
            {
                classificator.isCalculatingUsingBgWorker = true;
                classificator.SelfWorker = bgwProcessOneImage;
            }
            else
            {
                classificator.isCalculatingUsingBgWorker = false;
            }

            classificator.sourceImageFileName = ImageFileName;

            classificator.LogWindow = theLogWindow;
            string defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
            classificator.defaultOutputDataDirectory = defaultOutputDataDirectory;
            theLogWindow = ServiceTools.LogAText(theLogWindow, DateTime.Now.ToString() + Environment.NewLine, true);
            classificator.theLogWindow = theLogWindow;
            classificator.forceExistingSunInformation = cbxForceExistingSunInformation.Checked;

            BackgroundWorker bgwClassifyWorker = new BackgroundWorker();
            classificator.SelfWorker = bgwClassifyWorker;



            #region bgwClassifyWorker.DoWork

            bgwClassifyWorker.DoWork += new DoWorkEventHandler((obj, args) =>
            {
                SkyCloudClassification bgwClassificator = (SkyCloudClassification)((object[])args.Argument)[0];

                try
                {
                    bgwClassificator.Classify();
                }
                catch (Exception ex)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, ex.Message, true);
                }

                args.Result = new object[] { bgwClassificator };
            });

            #endregion



            #region bgwClassifyWorker.RunWorkerCompleted

            bgwClassifyWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((obj, args) =>
            {
                object[] objArgs = (object[])(args.Result);

                SkyCloudClassification bgwClassificator = (SkyCloudClassification)objArgs[0];

                Note(bgwClassificator.resultingStatusMessages);
                bgwClassificator.resultingStatusMessages = "";
                bitmapSI = new Bitmap(bgwClassificator.PreviewBitmap);
                TimeSpan timeNew = DateTime.Now - beginNew;

                ServiceTools.FlushMemory(null, "#02");

                ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);


                if (bgwClassificator.verbosityLevel > 0)
                {
                    string outputFilename = ImageFileName;
                    outputFilename = Path.GetFileNameWithoutExtension(finfoOriginalFile.Name) + "-result-" +
                                     bgwClassificator.ClassificationMethod.ToString() + ".jpg";
                    outputFilename = defaultOutputDataDirectory + outputFilename;
                    bitmapSI.Save(outputFilename, ImageFormat.Jpeg);

                    string origImageCopyFilename = Path.GetFileNameWithoutExtension(finfoOriginalFile.Name) + "-orig-" + ".jpg";
                    origImageCopyFilename = defaultOutputDataDirectory + origImageCopyFilename;
                    imagetoadd.Save(origImageCopyFilename);
                }

                pictureBox2Bitmap = new Bitmap(bitmapSI);

                ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);

                timetotal = DateTime.Now - begintotal;

                Note("Общее время выполнения: " + timetotal);
                Note("Время обсчета по новому алгоритму: " + timeNew);


                UpdateSIIFMlabels(1, bgwClassificator.CloudCover);
                ServiceTools.FlushMemory(null, "");
            });

            #endregion



            object[] bgwArgs = new object[] { classificator };

            bgwClassifyWorker.RunWorkerAsync(bgwArgs);
        }

        #endregion Single sky-image analysis



        #region Form behaviour




        private void UpdateSIIFMlabels(int VarNumber, double CloudCoverValue)
        {
            if (VarNumber == 1)//SI variant
            {
                ThreadSafeOperations.SetText(label5, "Cl.cover: " + Math.Round(CloudCoverValue, 2).ToString(), false);
            }
            else if (VarNumber == 2)//IFM variant
            {
                //ThreadSafeOperations.SetText(label6, "Cl.cover: " + Math.Round(CloudCoverValue, 2).ToString(), false);
                //this.label6.Text = "Cl.cover: " + Math.Round(CloudCoverValue, 2).ToString();
            }
        }




        private void Form1_Resize(object sender, EventArgs e)
        {
            if ((imagetoadd != null) && (!bgwProcessOneImage.IsBusy))
            {
                ThreadSafeOperations.UpdatePictureBox(pictureBox1, imagetoadd.Bitmap, true);
                ThreadSafeOperations.UpdatePictureBox(pictureBox2, pictureBox2Bitmap, true);
                this.Refresh();
            }
        }




        private void SkyIndexAnalyzer_AnalysisForm_Shown(object sender, EventArgs e)
        {
            readDefaultProperties();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            aboutprog = new AboutBox1();
            aboutprog.Show(this);
        }




        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (pictureBox2Bitmap == null)
            {
                return;
            }

            ServiceTools.ShowPicture(pictureBox2Bitmap);
        }




        private void button2_Click(object sender, EventArgs e)
        {
            //Bitmap bm2Process = imagetoadd.Copy().Bitmap;
            SkyImagesAnalyzer_ColorsManipulatingForm ManualAnalysisForm = new SkyImagesAnalyzer_ColorsManipulatingForm(imagetoadd, this, defaultProperties);
            //ManualAnalysisForm.tbLog = textBox1;
            ManualAnalysisForm.Show();
        }



        /// <summary>
        /// Handles the KeyPress event of the SkyIndexAnalyzer_AnalysisForm control.
        /// closes the form
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void SkyIndexAnalyzer_AnalysisForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }

        private void rbtnClassMethodNew_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnClassMethodGrIx.Checked)
            {
                //tunedSIMargin = 0.75d;
                tunedSIMargin = Convert.ToDouble(defaultProperties["GrIxDefaultSkyCloudMarginWithoutSun"]);
                trackBar1.Value = Convert.ToInt32(100.0d * tunedSIMargin);
                trackBar1_Scroll(null, null);
                ThreadSafeOperations.MoveTrackBar(trackBar1, (int)(tunedSIMargin * 100.0));
            }
        }

        private void rbtnClassMethodJapan_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnClassMethodJapan.Checked)
            {
                //tunedSIMargin = tunedSIMarginDefault;
                tunedSIMargin = Convert.ToDouble(defaultProperties["JapanCloudSkySeparationValue"]);
                ThreadSafeOperations.MoveTrackBar(trackBar1, (int)(tunedSIMargin * 100.0));
                trackBar1_Scroll(null, null);
            }
        }



        private void rbtnClassMethodUS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnClassMethodUS.Checked)
            {
                //tunedSIMargin = tunedSIMarginDefault;
                tunedSIMargin = Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]);
                ThreadSafeOperations.MoveTrackBar(trackBar1, (int)(tunedSIMargin * 100.0));
                trackBar1_Scroll(null, null);
            }
        }


        #endregion Form behaviour




        #region Compute and show GrIx histogram for the sky-image

        private void button3_Click_1(object sender, EventArgs e)
        {
            if ((ImageFileName == null) || (ImageFileName == ""))
            {
                return;
            }
            if (!File.Exists(ImageFileName))
            {
                return;
            }

            FileInfo fInfo = new FileInfo(ImageFileName);
            //imagetoadd = Image.FromFile(fInfo.FullName);
            imagetoadd = new Image<Bgr, byte>(fInfo.FullName);
            imagetoadd = ImageProcessing.ImageResizer(imagetoadd, Convert.ToInt32(defaultProperties["DefaultMaxImageSize"]));

            ImageProcessing imgP = new ImageProcessing(imagetoadd, true);
            Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(100.0d);
            Image<Gray, Byte> maskImageCircled85 = imgP.imageSignificantMaskCircled(85.0d);

            DenseMatrix dmMaskCircled100 = ImageProcessing.DenseMatrixFromImage(maskImageCircled);
            DenseMatrix dmMaskCircled85 = ImageProcessing.DenseMatrixFromImage(maskImageCircled85);
            ServiceTools.FlushMemory();

            string randomFileName = "m" + System.IO.Path.GetRandomFileName().Replace(".", "");

            string currentDirectory = (string)defaultProperties["DefaultDataFilesLocation"];

            DenseMatrix dmGrixData = imgP.eval("grix", null);
            dmGrixData = (DenseMatrix)dmGrixData.PointwiseMultiply(dmMaskCircled100);
            DenseVector dvGrixData = DataAnalysisStatic.DataVectorizedWithCondition(dmGrixData, dval => ((dval > 0.0d) && (dval < 1.0d)));

            HistogramDataAndProperties theHist = new HistogramDataAndProperties(dvGrixData, 500);
            theHist.quantilesCount = 20;
            HistogramCalcAndShowForm theHistForm = new HistogramCalcAndShowForm(ImageFileName, defaultProperties);
            if (theHistForm.defaultProperties.ContainsKey("CurrentFileName"))
            {
                theHistForm.defaultProperties["CurrentFileName"] = ImageFileName;
            }
            else
            {
                theHistForm.defaultProperties.Add("CurrentFileName", ImageFileName);
            }
            theHistForm.HistToRepresent = theHist;
            theHistForm.Show();
            theHistForm.rbtnShowAsBars.Checked = false;
            theHistForm.rbtnShowAsDots.Checked = true;
            theHistForm.tbBinsCount.Text = theHist.BinsCount.ToString();
            theHistForm.cbShowQuantiles.Checked = true;
            theHistForm.tbQuantilesCount.Text = theHist.quantilesCount.ToString();
            theHistForm.Represent();
            //imgP.Dispose();
            ServiceTools.FlushMemory();
        }

        #endregion Compute and show GrIx histogram for the sky-image







        #region // тестовая обработка изображения - с форсированным отключением определения положения солнца и без подавления солнечной засветки
        //private void btnTestMarginWithoutSun_Click(object sender, EventArgs e)
        //{
        //    DenseMatrix dmRedChannel;
        //    DenseMatrix dmBlueChannel;
        //    DenseMatrix dmGreenChannel;
        //    //LogWindow theLogWindow = null;

        //    //Bitmap LocalProcessingBitmap = (Bitmap)imagetoadd.Clone();

        //    Image<Gray, Byte> imageBlueChannelByte = imagetoadd[0].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[0];
        //    Image<Gray, Byte> imageGreenChannelByte = imagetoadd[1].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[1];
        //    Image<Gray, Byte> imageRedChannelByte = imagetoadd[2].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[2];



        //    //Image<Gray, Byte> maskImage = ImageProcessing.getImageSignificantMask(LocalProcessingBitmap);
        //    ImageProcessing imgP = new ImageProcessing(imagetoadd, true);
        //    //imgP.getImageSignificantMask();
        //    Image<Gray, Byte> maskImage = imgP.significantMaskImageBinary;
        //    Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(90.0d);
        //    PointD imageCircleCenter = imgP.imageRD.pointDCircleCenter();
        //    double imageCircleRadius = imgP.imageRD.DRadius;

        //    imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage);
        //    imageRedChannelByte = imageRedChannelByte.Mul(maskImage);
        //    imageGreenChannelByte = imageGreenChannelByte.Mul(maskImage);
        //    dmRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
        //    dmBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
        //    dmGreenChannel = ImageProcessing.DenseMatrixFromImage(imageGreenChannelByte);
        //    ServiceTools.FlushMemory();





        //    string randomFileName = "m" + System.IO.Path.GetRandomFileName().Replace(".", "");


        //    string currentDirectory = (string)defaultProperties["DefaultDataFilesLocation"];

        //    string formulaString = "grix";
        //    //string formulaString = "1.0 - (1.0 + ((B-R)/(B+R)))/2.0";
        //    DenseMatrix dmProcessingData = (DenseMatrix)imgP.eval(formulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
        //    DenseMatrix dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);
        //    DenseMatrix dmMaskCircled = ImageProcessing.DenseMatrixFromImage(maskImageCircled);
        //    dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMask);

        //    DenseMatrix dmReversed = (DenseMatrix)dmProcessingData.Clone();
        //    //dmReversed.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
        //    //{
        //    //    if (dmMask[row, column] == 0) return 1.0d;
        //    //    else if (val >= theStdDevMarginValueDefiningSkyCloudSeparation) return 1.0d;
        //    //    else return 0.0d;
        //    //}));

        //    //ImageConditionAndDataRepresentingForm restoredDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmReversed,
        //    //    "finally restored 1-sigma/Y data", true, false, 0.0d, 1.0d, true);
        //    //ImageConditionAndDataRepresentingForm originalDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmProcessingData,
        //    //    "original 1-sigm/Y data", true, false, 0.0d, 1.0d, true);
        //    dmReversed.SaveNetCDFdataMatrix(randomFileName + "_res.nc");
        //    dmProcessingData.SaveNetCDFdataMatrix(randomFileName + "_orig.nc");
        //    //restoredDataForm.SaveData(randomFileName + "_res.nc");
        //    //originalDataForm.SaveData(randomFileName + "_orig.nc");
        //    //originalDataForm.Close();
        //    //originalDataForm.Dispose();
        //    //restoredDataForm.Close();
        //    //restoredDataForm.Dispose();

        //    double cloudSkySeparationValue = tunedSIMargin;
        //    ColorScheme skyCloudColorScheme = ColorScheme.InversedBinaryCloudSkyColorScheme(cloudSkySeparationValue, 0.0d, 1.0d);
        //    ColorSchemeRuler skyCloudRuler = new ColorSchemeRuler(skyCloudColorScheme, 0.0d, 1.0d);
        //    Image<Bgr, Byte> previewImage = ImageProcessing.evalResultColoredWithFixedDataBounds(dmProcessingData, maskImage, skyCloudColorScheme, 0.0d, 1.0d);
        //    int cloudCounter = previewImage.CountNonzero()[1];
        //    int skyCounter = maskImage.CountNonzero()[0] - cloudCounter;
        //    double CloudCover = (double)cloudCounter / (double)(skyCounter + cloudCounter);

        //    //Bitmap localPreviewBitmap = previewImage.Bitmap;

        //}
        #endregion // тестовая обработка изображения - с форсированным отключением определения положения солнца и без подавления солнечной засветки







        #region read and save default properties

        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            //defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
            //                             "\\settings\\SkyImagesAnalyzerSettings.xml";
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
                defaultProperties = new Dictionary<string, object>();
            }

            string CurDir = Directory.GetCurrentDirectory();
            bool bDefaultPropertiesHasBeenUpdated = false;


            #region tunedSIMargin = JapanCloudSkySeparationValue

            if (defaultProperties.ContainsKey("JapanCloudSkySeparationValue"))
            {
                tunedSIMargin = Convert.ToDouble(defaultProperties["JapanCloudSkySeparationValue"]);
            }
            else
            {
                tunedSIMargin = 0.1d;
                defaultProperties.Add("JapanCloudSkySeparationValue", tunedSIMargin);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            tunedSIMarginDefault = tunedSIMargin;

            #endregion JapanCloudSkySeparationValue


            //tunedIFMMargin = Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]);
            //tunedIFMMarginDefault = tunedIFMMargin;


            #region path2process = BatchProcessingDirectory

            string path2process = CurDir;
            if (defaultProperties.ContainsKey("BatchProcessingDirectory"))
            {
                path2process = (string)defaultProperties["BatchProcessingDirectory"];
            }
            else
            {
                defaultProperties.Add("BatchProcessingDirectory", path2process);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            richTextBox1.Text = path2process;

            #endregion path2process = BatchProcessingDirectory



            #region ML_default_data_directory
            if (defaultProperties.ContainsKey("ML_default_data_directory"))
            {
                strMLdefaultDataDirectory = (string)defaultProperties["ML_default_data_directory"];
                if (!Directory.Exists(strMLdefaultDataDirectory))
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Cant find base ML data directory. Some ML functions may be unavailable.", true);
                }
            }
            else
            {
                strMLdefaultDataDirectory = CurDir;
                defaultProperties.Add("ML_default_data_directory", strMLdefaultDataDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion



            #region strConcurrentDataXMLfilesPath
            if (defaultProperties.ContainsKey("strConcurrentDataXMLfilesPath"))
            {
                strConcurrentDataXMLfilesPath = (string)defaultProperties["strConcurrentDataXMLfilesPath"];
                if (!Directory.Exists(strConcurrentDataXMLfilesPath))
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Cant find concurrent XML data files directory. Some functions may fail or work incorrect.", true);
                }
            }
            else
            {
                strConcurrentDataXMLfilesPath = CurDir;
                strConcurrentDataXMLfilesPath = strConcurrentDataXMLfilesPath +
                                                ((strConcurrentDataXMLfilesPath.Last() == Path.DirectorySeparatorChar)
                                                    ? ("")
                                                    : (Path.DirectorySeparatorChar.ToString())) + "results" +
                                                Path.DirectorySeparatorChar.ToString();
                defaultProperties.Add("strConcurrentDataXMLfilesPath", strConcurrentDataXMLfilesPath);

                bDefaultPropertiesHasBeenUpdated = true;
            }
            #endregion



            #region GrIxSunDetectionMaximalSunAreaPartial

            if (!defaultProperties.ContainsKey("GrIxSunDetectionMaximalSunAreaPartial"))
            {
                defaultProperties.Add("GrIxSunDetectionMaximalSunAreaPartial", 0.05d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region GrIxStdDevMarginValueDefiningTrueSkyArea

            if (!defaultProperties.ContainsKey("GrIxStdDevMarginValueDefiningTrueSkyArea"))
            {
                defaultProperties.Add("GrIxStdDevMarginValueDefiningTrueSkyArea", 0.65d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region GrIxMinimalSunburnYvalue

            if (!defaultProperties.ContainsKey("GrIxMinimalSunburnYvalue"))
            {
                defaultProperties.Add("GrIxMinimalSunburnYvalue", 247.0d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region GrIxMinimalSunburnGrIxvalue

            if (!defaultProperties.ContainsKey("GrIxMinimalSunburnGrIxvalue"))
            {
                defaultProperties.Add("GrIxMinimalSunburnGrIxvalue", 0.98d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region GrIxAnalysisImageCircledCropFactor

            if (!defaultProperties.ContainsKey("GrIxAnalysisImageCircledCropFactor"))
            {
                defaultProperties.Add("GrIxAnalysisImageCircledCropFactor", 0.95d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region GrIxSunDetectorConcurrentThreadsLimit

            if (!defaultProperties.ContainsKey("GrIxSunDetectorConcurrentThreadsLimit"))
            {
                defaultProperties.Add("GrIxSunDetectorConcurrentThreadsLimit", 4);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region MaxConcurrentFilesProcessingCount

            if (!defaultProperties.ContainsKey("MaxConcurrentFilesProcessingCount"))
            {
                defaultProperties.Add("MaxConcurrentFilesProcessingCount", 4);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region GrIxSunDetectionDesiredSunAreaPartial

            if (!defaultProperties.ContainsKey("GrIxSunDetectionDesiredSunAreaPartial"))
            {
                defaultProperties.Add("GrIxSunDetectionDesiredSunAreaPartial", 0.01d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region GrIxSunDetectorArcedCropFactor

            if (!defaultProperties.ContainsKey("GrIxSunDetectorArcedCropFactor"))
            {
                defaultProperties.Add("GrIxSunDetectorArcedCropFactor", 0.95d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region GrIxDefaultSkyCloudMarginWithSun

            if (!defaultProperties.ContainsKey("GrIxDefaultSkyCloudMarginWithSun"))
            {
                defaultProperties.Add("GrIxDefaultSkyCloudMarginWithSun", 0.1d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region GrIxProcessingVerbosityLevel

            if (!defaultProperties.ContainsKey("GrIxProcessingVerbosityLevel"))
            {
                defaultProperties.Add("GrIxProcessingVerbosityLevel", 0);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region DefaultMaxImageSize

            if (!defaultProperties.ContainsKey("DefaultMaxImageSize"))
            {
                defaultProperties.Add("DefaultMaxImageSize", 1920);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region DefaultScalingFactor

            if (!defaultProperties.ContainsKey("DefaultScalingFactor"))
            {
                defaultProperties.Add("DefaultScalingFactor", 0.125d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region DefaultDataFilesLocation

            if (!defaultProperties.ContainsKey("DefaultDataFilesLocation"))
            {
                defaultProperties.Add("DefaultDataFilesLocation",
                    path2process +
                    ((path2process.Last() == Path.DirectorySeparatorChar)
                        ? ("")
                        : (Path.DirectorySeparatorChar.ToString())) + "output" + Path.DirectorySeparatorChar.ToString());
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region GrIxDefaultSkyCloudMarginWithoutSun

            if (!defaultProperties.ContainsKey("GrIxDefaultSkyCloudMarginWithoutSun"))
            {
                defaultProperties.Add("GrIxDefaultSkyCloudMarginWithoutSun", 0.8d);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region DefaultMedianPerc5StatsXMLFile

            if (!defaultProperties.ContainsKey("DefaultMedianPerc5StatsXMLFile"))
            {
                defaultProperties.Add("DefaultMedianPerc5StatsXMLFile",
                    "D:\\_gulevlab\\SkyImagesAnalysis_appData\\AI45-total\\ID1\\stats\\statsWithFNames.xml");
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region PrecomputedAzimuthDeviationsXMLFile

            if (!defaultProperties.ContainsKey("PrecomputedAzimuthDeviationsXMLFile"))
            {
                defaultProperties.Add("PrecomputedAzimuthDeviationsXMLFile",
                    "D:\\_gulevlab\\SkyImagesAnalysis_appData\\AI45-total\\ID1\\stats\\PreComputedAzimuthDeviationsData.xml");
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region IoffeMeteoNavFilesDirectory

            if (!defaultProperties.ContainsKey("IoffeMeteoNavFilesDirectory"))
            {
                defaultProperties.Add("IoffeMeteoNavFilesDirectory",
                    "D:\\_gulevlab\\SkyImagesAnalysis_appData\\AI45-with-concurrent-data\\input\\meteo-nav-data\\");
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion





            #region ClusteringXMLDataFilesLocation

            if (!defaultProperties.ContainsKey("ClusteringXMLDataFilesLocation"))
            {
                defaultProperties.Add("ClusteringXMLDataFilesLocation",
                    "D:\\_gulevlab\\SkyImagesAnalysis_appData\\clustering-tests\\AI45-ID1\\");
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region ImagesRoundMasksXMLfilesMappingList

            if (!defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
            {
                defaultProperties.Add("ImagesRoundMasksXMLfilesMappingList",
                    Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                    Path.DirectorySeparatorChar + "ImagesRoundMasksXMLfilesMappingList.csv");
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region FilesAndConcurrentDataListToProcessForCloudCover

            if (defaultProperties.ContainsKey("FilesAndConcurrentDataListToProcessForCloudCover"))
            {
                FilesAndConcurrentDataListToProcessForCloudCover =
                    (string)defaultProperties["FilesAndConcurrentDataListToProcessForCloudCover"];
            }
            else
            {
                FilesAndConcurrentDataListToProcessForCloudCover =
                    (string)defaultProperties["DefaultDataFilesLocation"];
                FilesAndConcurrentDataListToProcessForCloudCover +=
                    (FilesAndConcurrentDataListToProcessForCloudCover.Last() == Path.DirectorySeparatorChar)
                        ? ("")
                        : (Path.DirectorySeparatorChar.ToString()) + "FilesListToDetectCloudCover.xml";
                defaultProperties.Add("FilesAndConcurrentDataListToProcessForCloudCover",
                    FilesAndConcurrentDataListToProcessForCloudCover);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region imageYRGBstatsXMLdataFilesDirectory

            if (!defaultProperties.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
            {
                defaultProperties.Add("imageYRGBstatsXMLdataFilesDirectory", CurDir);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region NN data files for SDC detection

            // NNconfigFile
            if (!defaultProperties.ContainsKey("NNconfigFile"))
            {
                defaultProperties.Add("NNconfigFile", CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNconfig.csv");
                bDefaultPropertiesHasBeenUpdated = true;
            }


            //NNtrainedParametersFile
            if (!defaultProperties.ContainsKey("NNtrainedParametersFile"))
            {
                string NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNtrainedParameters.csv";
                defaultProperties.Add("NNtrainedParametersFile", NNtrainedParametersFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            // NormMeansFile
            if (!defaultProperties.ContainsKey("NormMeansFile"))
            {
                string NormMeansFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormMeans.csv";
                defaultProperties.Add("NormMeansFile", NormMeansFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            // NormRangeFile
            if (!defaultProperties.ContainsKey("NormRangeFile"))
            {
                string NormRangeFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormRange.csv";
                defaultProperties.Add("NormRangeFile", NormRangeFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion NN data files for SDC detection



            #region TimeSpanForConcurrentDataMappingTolerance

            if (!defaultProperties.ContainsKey("TimeSpanForConcurrentDataMappingTolerance"))
            {
                TimeSpan TimeSpanForConcurrentDataMappingTolerance = new TimeSpan(0, 0, 30);
                defaultProperties.Add("TimeSpanForConcurrentDataMappingTolerance", TimeSpanForConcurrentDataMappingTolerance.ToString());
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            #region ConcurrentDataXMLfilesDirectory

            if (!defaultProperties.ContainsKey("ConcurrentDataXMLfilesDirectory"))
            {
                string ConcurrentDataXMLfilesDirectory = CurDir;
                defaultProperties.Add("ConcurrentDataXMLfilesDirectory", ConcurrentDataXMLfilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
                //saveDefaultProperties();
            }

            #endregion




            #region IncludeGPSandSunAltitudeData (while predict SDC)

            if (!defaultProperties.ContainsKey("IncludeGPSandSunAltitudeData"))
            {
                defaultProperties.Add("IncludeGPSandSunAltitudeData", true);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            theLogWindow = ServiceTools.LogAText(theLogWindow,
                    CommonTools.DictionaryRepresentation(defaultProperties, "Effective settings"));




            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }



        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }

        #endregion read and save default properties










        #region obsolete // private delegate DoWorkEventHandler OneQuantileProcessing

        //private delegate DoWorkEventHandler OneQuantileProcessing(object currBGWsender, DoWorkEventArgs args);

        //private void btnHist_Click(object sender, EventArgs e)
        //{
        //    if (imagetoadd == null)
        //    {
        //        ThreadSafeOperations.SetTextTB(textBox1, "Не загружено изображение для обработки!", true);
        //        return;
        //    }

        //    int defaultMaxImageSize = Convert.ToInt32(defaultProperties["DefaultMaxImageSize"]); ;
        //    double minSunAreaPart = Convert.ToDouble(defaultProperties["GrIxSunDetectionMinimalSunAreaPartial"]);
        //    double maxSunAreaPart = Convert.ToDouble(defaultProperties["GrIxSunDetectionMaximalSunAreaPartial"]);
        //    DenseMatrix dmRedChannel;
        //    DenseMatrix dmBlueChannel;
        //    DenseMatrix dmGreenChannel;


        //    FileInfo fInfo = new FileInfo(ImageFileName);
        //    imagetoadd = Image.FromFile(fInfo.FullName);
        //    imagetoadd = ImageProcessing.ImageResizer(imagetoadd, defaultMaxImageSize);

        //    Image<Gray, Byte> imageBlueChannelByte = new Image<Bgr, Byte>((Bitmap)imagetoadd)[0];
        //    Image<Gray, Byte> imageGreenChannelByte = new Image<Bgr, Byte>((Bitmap)imagetoadd)[1];
        //    Image<Gray, Byte> imageRedChannelByte = new Image<Bgr, Byte>((Bitmap)imagetoadd)[2];

        //    ImageProcessing imgP = new ImageProcessing((Bitmap)imagetoadd, true);
        //    Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(100.0d);
        //    Image<Gray, Byte> maskImageCircled85 = imgP.imageSignificantMaskCircled(85.0d);


        //    DenseMatrix dmSunburnData = imgP.eval("Y", null);
        //    DenseVector dvGrIxDataEqualsOne = DataAnalysis.DataVectorizedWithCondition(dmSunburnData, dval => dval >= 254.0d);
        //    if (dvGrIxDataEqualsOne == null)
        //    {
        //        RoundData rd = RoundData.nullRoundData();
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, "sun location: " + rd);
        //    }
        //    if (dvGrIxDataEqualsOne.Values.Sum() < imgP.significantMaskImageBinary.CountNonzero()[0] * minSunAreaPart)
        //    {
        //        RoundData rd = RoundData.nullRoundData();
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, "sun location: " + rd);
        //    }


        //    imageBlueChannelByte = imageBlueChannelByte.Mul(maskImageCircled);
        //    imageRedChannelByte = imageRedChannelByte.Mul(maskImageCircled);
        //    imageGreenChannelByte = imageGreenChannelByte.Mul(maskImageCircled);
        //    dmRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
        //    dmBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
        //    dmGreenChannel = ImageProcessing.DenseMatrixFromImage(imageGreenChannelByte);
        //    ServiceTools.FlushMemory();

        //    string randomFileName = "m" + System.IO.Path.GetRandomFileName().Replace(".", "");


        //    string currentDirectory = (string)defaultProperties["DefaultDataFilesLocation"];

        //    string GrIxformulaString = "1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y";
        //    DenseMatrix dmGrIxData = (DenseMatrix)imgP.eval(GrIxformulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
        //    DenseMatrix dmSourceGrIxData = (DenseMatrix)dmGrIxData.Clone();
        //    dmGrIxData = (DenseMatrix)dmGrIxData.PointwiseMultiply(ImageProcessing.DenseMatrixFromImage(maskImageCircled85));
        //    ServiceTools.RepresentDataFromDenseMatrix(dmGrIxData);

        //    DenseVector dvGrIxDataToHist = DataAnalysis.DataVectorizedExcludingValues(dmGrIxData, 0.0d);
        //    dvGrIxDataToHist = DataAnalysis.DataVectorizedWithCondition(dvGrIxDataToHist, dval => (dval != 1.0d));
        //    HistogramDataAndProperties theGrIxHist = new HistogramDataAndProperties(dvGrIxDataToHist, 1000);

        //    FunctionRepresentationForm funcForm = new FunctionRepresentationForm("PDF");
        //    funcForm.dvScatterXSpace = theGrIxHist.dvbinsCenters;
        //    funcForm.dvScatterFuncValues = theGrIxHist.dvProbDens;
        //    funcForm.xSpaceMin = theGrIxHist.dvbinsCenters.Values.Min();
        //    funcForm.xSpaceMax = theGrIxHist.dvbinsCenters.Values.Max();
        //    funcForm.overallFuncMax = theGrIxHist.dvProbDens.Values.Max();
        //    funcForm.overallFuncMin = 0.0d;


        //    double dx = theGrIxHist.dvbinsCenters[1] - theGrIxHist.dvbinsCenters[0];

        //    List<RoundData> CenterPoints = new List<RoundData>();
        //    List<bool> finishedBGworkers = new List<bool>();
        //    List<double> quantilesBy10 = new List<double>();
        //    for (int i = 10; i <= 60; i += 5)
        //    {
        //        quantilesBy10.Add(Statistics.Percentile(dvGrIxDataToHist, i));
        //        finishedBGworkers.Add(false);
        //    }

        //    DenseVector quantileWeights = DenseVector.Create(quantilesBy10.Count, i =>
        //    {
        //        double currWeight = ((quantilesBy10[i] - dvGrIxDataToHist.Values.Min()) /
        //                             (dvGrIxDataToHist.Values.Max() - dvGrIxDataToHist.Values.Min()));
        //        currWeight = 1.0d / currWeight;
        //        return currWeight;
        //    });
        //    double weightsSum = quantileWeights.Values.Sum();
        //    quantileWeights = (DenseVector)quantileWeights.Divide(weightsSum);

        //    DenseMatrix dmSunDetectionDataByAnsamble = DenseMatrix.Create(dmGrIxData.RowCount, dmGrIxData.ColumnCount, (r, c) => 0.0d);

        //    DateTime startDate = DateTime.Now;

        //    RunWorkerCompletedEventHandler currWorkCompletedHandler =
        //        delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
        //        {
        //            object[] currentBGWResults = (object[])args.Result;
        //            DenseMatrix dmResSunDetectionPartial = (DenseMatrix)currentBGWResults[0];
        //            RoundData retRD = (RoundData)currentBGWResults[1];
        //            int returningBGWthreadID = (int)currentBGWResults[2];


        //            dmSunDetectionDataByAnsamble = dmSunDetectionDataByAnsamble + dmResSunDetectionPartial;
        //            if (!retRD.IsNull) CenterPoints.Add(retRD);
        //            finishedBGworkers[returningBGWthreadID] = true;
        //        };

        //    DoWorkEventHandler currDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
        //    {
        //        object[] currBGWarguments = (object[])args.Argument;
        //        string fileFullName = (string)currBGWarguments[0];
        //        Image currImageToAdd = Image.FromFile(fileFullName);
        //        currImageToAdd = ImageProcessing.ImageResizer(currImageToAdd, defaultMaxImageSize);
        //        double currQtle = (double)currBGWarguments[1];
        //        Dictionary<string, object> properties = (Dictionary<string, object>)currBGWarguments[2];
        //        Form parentForm = (Form)currBGWarguments[3];
        //        int currBGWthreadID = (int)currBGWarguments[4];
        //        ImageProcessing imgPrObj = (ImageProcessing)currBGWarguments[5];
        //        DenseMatrix dmSrcGrIxData = (DenseMatrix)currBGWarguments[6];

        //        SkyCloudClassification currClassificator = new SkyCloudClassification(currImageToAdd, properties);
        //        currClassificator.SelfWorker = sender as BackgroundWorker;
        //        currClassificator.LogWindow = null;
        //        currClassificator.ParentForm = parentForm;
        //        currClassificator.ClassificationMethod = ClassificationMethods.TestNew;
        //        currClassificator.isCalculatingUsingBgWorker = true;
        //        currClassificator.defaultOutputDataDirectory = (string)properties["DefaultDataFilesLocation"];
        //        currClassificator.cloudSkySeparationValue = Convert.ToDouble(properties["JapanCloudSkySeparationValue"]);


        //        RoundData rd = currClassificator.DetectSunArced(imgPrObj, dmSrcGrIxData, currentDirectory, randomFileName, currQtle * 0.99d, currQtle * 1.01d);


        //        Image<Gray, byte> tmpImg = new Image<Gray, byte>(currImageToAdd.Width, currImageToAdd.Height, new Gray(0));
        //        tmpImg.Draw(new CircleF(rd.pointDCircleCenter().PointF(), (float)rd.DRadius), new Gray(255), 0);
        //        DenseMatrix tmpDM = ImageProcessing.DenseMatrixFromImage(tmpImg);
        //        tmpDM = (DenseMatrix)tmpDM.Multiply(quantileWeights[currBGWthreadID]);
        //        //dmSunDetectionDataByAnsamble = dmSunDetectionDataByAnsamble + tmpDM;

        //        args.Result = new object[] { tmpDM, rd, currBGWthreadID };
        //    };



        //    for (int i = 0; i < quantilesBy10.Count; i++)
        //    {
        //        double qtle = quantilesBy10[i];
        //        funcForm.theRepresentingFunctions.Add((dvParameters, x) =>
        //        {
        //            if (Math.Abs(x - qtle) <= dx) return theGrIxHist.dvProbDens.Values.Max();
        //            else return 0.0d;
        //        });
        //        funcForm.parameters.Add(DenseVector.Create(1, j => 0.0d));
        //        funcForm.lineColors.Add(new Bgr(Color.Red));

        //        BackgroundWorker currBgw = new BackgroundWorker();
        //        currBgw.DoWork += currDoWorkHandler;
        //        currBgw.RunWorkerCompleted += currWorkCompletedHandler;
        //        currBgw.WorkerReportsProgress = true;
        //        object[] BGWargs = new object[] { fInfo.FullName, qtle, defaultProperties, this, i, imgP, dmSourceGrIxData };
        //        currBgw.RunWorkerAsync(BGWargs);


        //        #region // пробую заменить многопоточным выполнением
        //        //imagetoadd = Image.FromFile(fInfo.FullName);
        //        //imagetoadd = ImageResizer(imagetoadd, defaultMaxImageSize);

        //        //SkyCloudClassification classificator = new SkyCloudClassification(imagetoadd, defaultProperties);
        //        //classificator.LogWindow = theLogWindow;
        //        //classificator.ParentForm = this;
        //        //classificator.ClassificationMethod = ClassificationMethods.TestNew;
        //        //classificator.isCalculatingUsingBgWorker = false;
        //        //classificator.defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
        //        //classificator.cloudSkySeparationValue =
        //        //    Convert.ToDouble(defaultProperties["JapanCloudSkySeparationValue"]);



        //        //RoundData rd = classificator.DetectSunArced(currentDirectory, randomFileName, qtle * 0.99d, qtle * 1.01d);


        //        //double currWeight = ((qtle - dvGrIxDataToHist.Values.Min()) /
        //        //                     (dvGrIxDataToHist.Values.Max() - dvGrIxDataToHist.Values.Min()));
        //        //currWeight = 1.0d / currWeight;
        //        //Image<Gray, byte> tmpImg = new Image<Gray, byte>(imagetoadd.Width, imagetoadd.Height, new Gray(0));
        //        //tmpImg.Draw(new CircleF(rd.pointDCircleCenter().PointF(), (float)rd.DRadius), new Gray(255), 0);
        //        //DenseMatrix tmpDM = ImageProcessing.DenseMatrixFromImage(tmpImg);
        //        //tmpDM = (DenseMatrix)tmpDM.Multiply(currWeight);
        //        //dmSunDetectionDataByAnsamble = dmSunDetectionDataByAnsamble + tmpDM;

        //        //if (!rd.IsNull) CenterPoints.Add(rd);
        //        #endregion // пробую заменить многопоточным выполнением
        //    }

        //    while (finishedBGworkers.Sum(boolVal => (boolVal) ? ((int)1) : ((int)0)) < finishedBGworkers.Count)
        //    {
        //        System.Windows.Forms.Application.DoEvents();
        //        Thread.Sleep(100);
        //    }

        //    TimeSpan span = DateTime.Now - startDate;
        //    theLogWindow = ServiceTools.LogAText(theLogWindow, "time: " + span);

        //    funcForm.Show();
        //    funcForm.Represent();

        //    ServiceTools.RepresentDataFromDenseMatrix(dmSunDetectionDataByAnsamble, "dmSunDetectionDataByAnsamble", true, false, 0.0d,
        //        dmSunDetectionDataByAnsamble.Values.Max());
        //    dmSunDetectionDataByAnsamble =
        //        (DenseMatrix)dmSunDetectionDataByAnsamble.Multiply(1.0d / dmSunDetectionDataByAnsamble.Values.Max());
        //    double sunArea = dmSunDetectionDataByAnsamble.Values.Sum();
        //    double sunRadius = Math.Sqrt(sunArea / Math.PI);

        //    ServiceTools.RepresentHistogrammedStatsOfDataMarix(dmSunDetectionDataByAnsamble,
        //        "sun detection density stats", true);

        //    DenseMatrix dmSunDetectionDataByAnsambleW = (DenseMatrix)dmSunDetectionDataByAnsamble.Clone();
        //    dmSunDetectionDataByAnsambleW = (DenseMatrix)dmSunDetectionDataByAnsambleW.Divide(dmSunDetectionDataByAnsamble.Values.Sum());

        //    DenseMatrix dmSunDetectionDataByAnsambleX = DenseMatrix.Create(dmSunDetectionDataByAnsamble.RowCount,
        //        dmSunDetectionDataByAnsamble.ColumnCount, (r, c) => (double)c);
        //    double sunCenterX =
        //        ((DenseMatrix)(dmSunDetectionDataByAnsambleX.PointwiseMultiply(dmSunDetectionDataByAnsambleW))).Values
        //            .Sum();
        //    DenseMatrix dmSunDetectionDataByAnsambleY = DenseMatrix.Create(dmSunDetectionDataByAnsamble.RowCount,
        //        dmSunDetectionDataByAnsamble.ColumnCount, (r, c) => (double)r);
        //    double sunCenterY =
        //        ((DenseMatrix)(dmSunDetectionDataByAnsambleY.PointwiseMultiply(dmSunDetectionDataByAnsambleW))).Values
        //            .Sum();

        //    RoundData sunRD = new RoundData(sunCenterX, sunCenterY, sunRadius);

        //    if (sunRD.RoundArea / imgP.imageRD.RoundArea > maxSunAreaPart)
        //    {
        //        sunRD.DRadius = Math.Sqrt((maxSunAreaPart * imgP.imageRD.RoundArea) / Math.PI);
        //    }
        //    if (sunRD.RoundArea / imgP.imageRD.RoundArea < minSunAreaPart / 3.0d)
        //    {
        //        sunRD = RoundData.nullRoundData();
        //    }

        //    if (!sunRD.IsNull)
        //    {
        //        imagetoadd = Image.FromFile(fInfo.FullName);
        //        imagetoadd = ImageProcessing.ImageResizer(imagetoadd, defaultMaxImageSize);
        //        Bitmap bmToShow = (Bitmap)imagetoadd.Clone();

        //        Image<Bgr, byte> img2Show = new Image<Bgr, byte>((Bitmap)bmToShow);
        //        img2Show.Mul(0.5d);

        //        img2Show.Draw(new CircleF(sunRD.pointDCircleCenter().PointF(), (float)sunRD.DRadius),
        //            new Bgr(Color.Magenta), 2);

        //        ServiceTools.ShowPicture(img2Show, "");
        //    }

        //    //проверим, чтобы
        //    // определенное солнце хотя бы пересекалось с областью засветки
        //    // площадь определенного солнца не была меньше минимально допустимой

        //    theLogWindow = ServiceTools.LogAText(theLogWindow, "sun location: " + sunRD);
        //}

        #endregion obsolete





        #region // пакетный рассчет статистических характеристик изображений перенесено в отдельное приложение

        //private void btnTest1_Click(object sender, EventArgs e)
        //{
        //    string path2process = (string)defaultProperties["BatchProcessingDirectory"];
        //    if (path2process == "")
        //    {
        //        path2process = Directory.GetCurrentDirectory();
        //    }

        //    DirectoryInfo dir = new DirectoryInfo(path2process);

        //    if (!dir.Exists)
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow,
        //            "Операция не выполнена. Не найдена директория:" + Environment.NewLine + path2process +
        //            Environment.NewLine, true);
        //        //ThreadSafeOperations.SetTextTB(textBox1, textBox1.Text + "Операция не выполнена. Не найдена директория:" + Environment.NewLine + path2process + Environment.NewLine, true);
        //        return;
        //    }


        //    string xmlStatsFileName = (string)defaultProperties["DefaultDataFilesLocation"] + "median-perc5-stats.xml";

        //    FileInfo[] fileList2Process = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
        //    if (fileList2Process.Length == 0)
        //    {
        //        Note("Coudn't find any *.jpg file in processing directiory or its subdirectories: \"" + path2process + "\" ");
        //        return;
        //    }


        //    List<bool> bgwFinished = new List<bool>();
        //    List<BackgroundWorker> bgwList = new List<BackgroundWorker>();
        //    for (int i = 0; i < 8; i++)
        //    {
        //        bgwFinished.Add(true);
        //        bgwList.Add(null);
        //    }

        //    List<SkyImageMedianPerc5Data> lMedianPerc5Data = new List<SkyImageMedianPerc5Data>();


        //    DoWorkEventHandler thisWorkerDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
        //    {
        //        object[] currBGWarguments = (object[])args.Argument;
        //        string currentFullFileName = (string)currBGWarguments[0];
        //        Dictionary<string, object> defaultProps = (Dictionary<string, object>)currBGWarguments[1];
        //        int currentBgwID = (int)currBGWarguments[2];

        //        #region // obsolete
        //        //FileInfo fInfo = new FileInfo(currentFullFileName);

        //        //int maxImageSize = Convert.ToInt32(defaultProps["DefaultMaxImageSize"]); ;

        //        //BackgroundWorker SelfWorker = currBGWsender as System.ComponentModel.BackgroundWorker;

        //        //string outputStatsDirectory = (string)defaultProps["DefaultDataFilesLocation"];

        //        ////ServiceTools.logToTextFile(outputStatsDirectory + "statsWithFNames.dat", "Full filename | 5prc | median" + Environment.NewLine, true);

        //        ////Image img = Image.FromFile(fInfo.FullName);
        //        ////img = ImageProcessing.ImageResizer(img, maxImageSize);

        //        ////Bitmap bitmap2process = ServiceTools.ReadBitmapFromFile(fInfo.FullName);
        //        ////bitmap2process = ImageProcessing.BitmapResizer(bitmap2process, maxImageSize);
        //        //Image<Bgr, byte> img2process = new Image<Bgr, byte>(fInfo.FullName);
        //        //img2process = ImageProcessing.ImageResizer(img2process, maxImageSize);

        //        //while (img2process.CountNonzero().Sum() == 0)
        //        //{
        //        //    img2process = new Image<Bgr, byte>(fInfo.FullName);
        //        //    img2process = ImageProcessing.ImageResizer(img2process, maxImageSize);
        //        //}

        //        //ImageProcessing imgP = new ImageProcessing(img2process, true);

        //        //Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(100.0d);
        //        //Image<Gray, Byte> maskImageCircled85 = imgP.imageSignificantMaskCircled(85.0d);

        //        //DenseMatrix dmMaskCircled100 = ImageProcessing.DenseMatrixFromImage(maskImageCircled);
        //        //ServiceTools.FlushMemory();

        //        //DenseMatrix dmGrixData = imgP.eval("grix", null);
        //        //dmGrixData = (DenseMatrix)dmGrixData.PointwiseMultiply(dmMaskCircled100);
        //        //DenseVector dvGrixData = DataAnalysis.DataVectorizedWithCondition(dmGrixData, dval => ((dval > 0.0d) && (dval < 1.0d)));
        //        //DescriptiveStatistics stats = new DescriptiveStatistics(dvGrixData, true);
        //        //double median = dvGrixData.Median();
        //        ////double median = stats.Median;
        //        //double perc5 = dvGrixData.Percentile(5);
        //        //SkyImageMedianPerc5Data mp5dt = new SkyImageMedianPerc5Data(currentFullFileName, median, perc5);
        //        #endregion // obsolete

        //        int maxImageSize = Convert.ToInt32(defaultProps["DefaultMaxImageSize"]); ;
        //        Tuple<double, double, ImageProcessing> tplMedianPerc5Data = ImageProcessing.CalculateMedianPerc5Values(currentFullFileName, maxImageSize);
        //        SkyImageMedianPerc5Data mp5dt = new SkyImageMedianPerc5Data(currentFullFileName, tplMedianPerc5Data.Item1, tplMedianPerc5Data.Item2);

        //        #region // obsolete
        //        //bool success = false;
        //        //while (!success)
        //        //{
        //        //    try
        //        //    {
        //        //        ServiceTools.logToTextFile(outputStatsDirectory + "statsWithFNames.dat",
        //        //            fInfo.FullName + ";" + median.ToString("e").Replace(",", ".") + ";" +
        //        //            perc5.ToString("e").Replace(",", ".") + Environment.NewLine, true);
        //        //        success = true;
        //        //    }
        //        //    catch (Exception)
        //        //    {
        //        //        Thread.Sleep(100);
        //        //    }
        //        //}

        //        //SimpleShowImageForm SimpleShowImageForm1 = new SimpleShowImageForm(imgP.processingBitmap(), "");
        //        //SimpleShowImageForm1.DesktopLocation =
        //        //    new Point(
        //        //        Convert.ToInt32(10.0d +
        //        //                        (double) currentBgwID*System.Windows.SystemParameters.PrimaryScreenWidth/
        //        //                        (double) bgwFinished.Count), 10);

        //        //SimpleShowImageForm1.Show();
        //        //System.Windows.Forms.Application.DoEvents();
        //        //Thread.Sleep(500);
        //        //SimpleShowImageForm1.Close();
        //        //SimpleShowImageForm1.Dispose();
        //        #endregion // obsolete

        //        args.Result = new object[]
        //        {
        //            currentBgwID, mp5dt
        //        };
        //    };


        //    RunWorkerCompletedEventHandler currWorkCompletedHandler =
        //        delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
        //        {
        //            object[] currentBGWResults = (object[])args.Result;
        //            int returningBGWthreadID = (int)currentBGWResults[0];
        //            SkyImageMedianPerc5Data res = (SkyImageMedianPerc5Data)currentBGWResults[1];

        //            lMedianPerc5Data.Add(res);

        //            BackgroundWorker currBGW = currBGWCompletedSender as BackgroundWorker;
        //            currBGW.Dispose();

        //            bgwFinished[returningBGWthreadID] = true;
        //            bgwList[returningBGWthreadID].Dispose();
        //            bgwList[returningBGWthreadID] = null;

        //            theLogWindow = ServiceTools.LogAText(theLogWindow, "finished processing: " + res.FileName);
        //        };



        //    foreach (FileInfo info in fileList2Process)
        //    {
        //        int currentBgwID = -1;
        //        while (bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) == bgwFinished.Count)
        //        {
        //            System.Windows.Forms.Application.DoEvents();
        //            Thread.Sleep(100);
        //        }
        //        for (int i = 0; i < bgwFinished.Count; i++)
        //        {
        //            if (bgwFinished[i])
        //            {
        //                currentBgwID = i;
        //                bgwFinished[i] = false;
        //                break;
        //            }
        //        }


        //        theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "starting: " + info.FullName);



        //        object[] BGWorker2Args = new object[] { info.FullName, defaultProperties, currentBgwID };

        //        BackgroundWorker currBgw = new BackgroundWorker();
        //        bgwList[currentBgwID] = currBgw;
        //        currBgw.DoWork += thisWorkerDoWorkHandler;
        //        currBgw.RunWorkerCompleted += currWorkCompletedHandler;
        //        currBgw.WorkerReportsProgress = true;
        //        currBgw.RunWorkerAsync(BGWorker2Args);
        //    }


        //    while (bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) > 0)
        //    {
        //        System.Windows.Forms.Application.DoEvents();
        //        Thread.Sleep(100);
        //    }



        //    try
        //    {
        //        ServiceTools.WriteObjectToXML(lMedianPerc5Data, xmlStatsFileName);
        //    }
        //    catch (Exception exc)
        //    {
        //        theLogWindow = ServiceTools.LogAText(theLogWindow,
        //            Environment.NewLine +
        //            "couldn`t write stats data to a file:" + Environment.NewLine + xmlStatsFileName +
        //            Environment.NewLine);
        //        theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "cause: " + exc.Message + Environment.NewLine);
        //    }


        //    theLogWindow = ServiceTools.LogAText(theLogWindow,
        //        Environment.NewLine + "==========FINISHED==========" + Environment.NewLine);
        //}

        #endregion пакетный рассчет статистических характеристик изображений перенесено в отдельное приложение









        private void btnProcessDirectorySI_Click(object sender, EventArgs e)
        {
            string path2process = (string)defaultProperties["BatchProcessingDirectory"];
            if (path2process == "")
            {
                path2process = Directory.GetCurrentDirectory();
            }

            DirectoryInfo dir = new DirectoryInfo(path2process);

            if (bgwProcessDirectoryOfImages.IsBusy)
            {
                bgwProcessDirectoryOfImages.CancelAsync();
            }
            else
            {
                if (!dir.Exists)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Операция не выполнена. Не найдена директория:" + Environment.NewLine + path2process +
                        Environment.NewLine, true);
                    //ThreadSafeOperations.SetTextTB(textBox1, textBox1.Text + "Операция не выполнена. Не найдена директория:" + Environment.NewLine + path2process + Environment.NewLine, true);
                    return;
                }

                FileInfo[] fileList2Process = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
                if (fileList2Process.Length == 0)
                {
                    Note("Coudn't find any *.jpg file in processing directiory or its subdirectories: \"" + path2process + "\" ");
                    return;
                }



                //object[] BGWorker2Args = new object[] { trackBar3.Value };
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Multiple images computing started with the following parameters:");
                string strToDisplay = "";
                //var propertiesObj = Properties.Settings.Default.Properties;
                foreach (KeyValuePair<string, object> settingsProperty in defaultProperties)
                {
                    strToDisplay += Environment.NewLine + settingsProperty.Key + " = " + settingsProperty.Value;
                }
                theLogWindow = ServiceTools.LogAText(theLogWindow, strToDisplay);

                simpleMultipleImagesShow imagesRepresentingForm = new simpleMultipleImagesShow();
                imagesRepresentingForm.Show();

                List<ClassificationMethods> schemesToUse = new List<ClassificationMethods>();
                schemesToUse.Add(ClassificationMethods.Japan);


                object[] BGWorker2Args = new object[] { path2process, theLogWindow, imagesRepresentingForm, schemesToUse };

                ThreadSafeOperations.ToggleButtonState(button1, true, "Прекратить обработку", true);

                bgwProcessDirectoryOfImages.RunWorkerAsync(BGWorker2Args);
            }
        }







        /// <summary>
        /// Handles the Click event of the pictureBox1 control.
        /// Currently manages the ability of sun disk representing or (if not specified yet) manual specification
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (imagetoadd == null) return;

            //FileInfo fInfo1 = new FileInfo(ImageFileName);
            //string sunDiskInfoFileName = fInfo1.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(ImageFileName) +
            //                             "-SunDiskInfo.xml";
            string sunDiskInfoFileName = ConventionalTransitions.SunDiskInfoFileName(ImageFileName);

            RoundData existingRoundData = RoundData.nullRoundData();
            Size imgSizeUnderExistingRoundData = imagetoadd.Bitmap.Size;
            object existingRoundDataObj = ServiceTools.ReadObjectFromXML(sunDiskInfoFileName, typeof(RoundDataWithUnderlyingImgSize));

            if (existingRoundDataObj != null)
            {
                existingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).circle;
                imgSizeUnderExistingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).imgSize;
            }

            double currScale = (double)imagetoadd.Width / (double)imgSizeUnderExistingRoundData.Width;
            if (currScale != 1.0d)
            {
                existingRoundData.DCenterX *= currScale;
                existingRoundData.DCenterY *= currScale;
                existingRoundData.DRadius *= currScale;
            }

            SunDiskRepresentingAndCorrectionForm form = new SunDiskRepresentingAndCorrectionForm(imagetoadd, existingRoundData);
            DialogResult res = form.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            RoundData retSunDiskCircle = form.sunDiskPositionAndSize.Copy();
            Size imgSize = imagetoadd.Bitmap.Size;
            RoundDataWithUnderlyingImgSize tplSunDiskPositionData = new RoundDataWithUnderlyingImgSize();
            tplSunDiskPositionData.circle = retSunDiskCircle;
            tplSunDiskPositionData.imgSize = imgSize;

            ServiceTools.WriteObjectToXML(tplSunDiskPositionData, sunDiskInfoFileName);

        }




        #region Median-perc5 distribution, heatmap, density processing

        /// <summary>
        /// Handles the Click event of the btnShowMedianPerc5Diagram control.
        /// Shows current image at the median-perc5 diagram using previously calculated statistics on the image set
        /// stats data is being obtained from the file at the path "DefaultMedianPerc5StatsXMLFile" of program properties
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnShowMedianPerc5Diagram_Click(object sender, EventArgs e)
        {
            #region //конвертировали .dat в .xml для упрощенной обработки
            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.InitialDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
            //dlg.DefaultExt = "*.dat files|*.dat";
            //DialogResult res = dlg.ShowDialog();
            //if (res != System.Windows.Forms.DialogResult.OK)
            //{
            //    return;
            //}

            //string fName = dlg.FileName;
            //string fileContent = ServiceTools.ReadTextFromFile(fName);

            //string[] substrings = fileContent.Split('\n');
            //List<SkyImageMedianPerc5Data> lData = new List<SkyImageMedianPerc5Data>();

            //foreach (string pDataSubstring in substrings)
            //{
            //    string[] strValues = pDataSubstring.Split(';');
            //    if (strValues.Count() != 3)
            //    {
            //        continue;
            //    }

            //    lData.Add(new SkyImageMedianPerc5Data(
            //        Convert.ToString(strValues[0]),
            //        Convert.ToDouble(strValues[1].Replace(".", ",")),
            //        Convert.ToDouble(strValues[2].Replace(".", ","))));
            //}

            //ServiceTools.WriteObjectToXML(lData, fName + ".xml");
            #endregion //конвертировали .dat в .xml для упрощенной обработки


            string strStatsDataFilename = ((string)defaultProperties["DefaultDataFilesLocation"]) + "statsWithFNames.xml";
            if (!File.Exists(strStatsDataFilename))
            {
                strStatsDataFilename = ((string)defaultProperties["DefaultMedianPerc5StatsXMLFile"]);
            }
            if (!File.Exists(strStatsDataFilename))
            {
                return;
            }


            List<SkyImageMedianPerc5Data> lStatsData =
                (List<SkyImageMedianPerc5Data>)
                    ServiceTools.ReadObjectFromXML(strStatsDataFilename, typeof(List<SkyImageMedianPerc5Data>));
            //List<SkyImageMedianPerc5Data> lStatsData =
            //    lStatsDataObj.ConvertAll<SkyImageMedianPerc5Data>(obj => (SkyImageMedianPerc5Data)obj);

            List<PointD> lPointsList = lStatsData.ConvertAll<PointD>(statsDatum => new PointD(statsDatum.GrIxStatsMedian, statsDatum.GrIxStatsPerc5));
            HeatMap hm = new HeatMap(lPointsList, 200);
            hm.defaultProperties = defaultProperties;
            hm.SetEqualMeasures();
            hm.SmoothDensityField(DataAnalysis.Extensions.StandardConvolutionKernels.gauss);
            //hm.RepresentHeatMap();


            if ((ImageFileName == "") || (ImageFileName == null))
            {
                return;
            }

            string currentFullFileName = ImageFileName;
            Dictionary<string, object> defaultProps = defaultProperties;

            FileInfo fInfo = new FileInfo(currentFullFileName);
            int maxImageSize = Convert.ToInt32(defaultProps["DefaultMaxImageSize"]); ;

            Image<Bgr, byte> img2process = new Image<Bgr, byte>(fInfo.FullName);
            img2process = ImageProcessing.ImageResizer(img2process, maxImageSize);

            ImageProcessing imgP = new ImageProcessing(img2process, true);

            Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(100.0d);

            DenseMatrix dmMaskCircled100 = ImageProcessing.DenseMatrixFromImage(maskImageCircled);

            DenseMatrix dmGrixData = imgP.eval("grix", null);
            dmGrixData = (DenseMatrix)dmGrixData.PointwiseMultiply(dmMaskCircled100);
            DenseVector dvGrixData = DataAnalysis.DataAnalysisStatic.DataVectorizedWithCondition(dmGrixData, dval => ((dval > 0.0d) && (dval < 1.0d)));
            DescriptiveStatistics stats = new DescriptiveStatistics(dvGrixData, true);
            double median = Statistics.Median(dvGrixData);
            //double median = stats.Median;
            double perc5 = Statistics.Percentile(dvGrixData, 5);

            hm.lPtdMarks.Add(new PointD(median, perc5));
            hm.RepresentHeatMap();

            //Image<Bgr, byte> imgDiagram = new Image<Bgr, byte>(hm.CurrHeatMapData.dataRepresentingImageColored());

            //PointD currImgPosition = new PointD();
            //currImgPosition.Y = imgDiagram.Height*(1.0d - (perc5 - hm.minYval)/(hm.maxYval - hm.minYval));
            //currImgPosition.X = imgDiagram.Width*(median - hm.minXval)/(hm.maxXval - hm.minXval);
            //imgDiagram.Draw(new CircleF(currImgPosition.PointF(), 1), new Bgr(Color.Red), 0);
            //ServiceTools.ShowPicture(imgDiagram, "");

        }


        private void btnDensityProcessing_Click(object sender, EventArgs e)
        {
            string strStatsDataFilename = ((string)defaultProperties["DefaultDataFilesLocation"]) + "statsWithFNames.xml";
            if (!File.Exists(strStatsDataFilename))
            {
                strStatsDataFilename = ((string)defaultProperties["DefaultMedianPerc5StatsXMLFile"]);
            }
            if (!File.Exists(strStatsDataFilename))
            {
                return;
            }

            List<SkyImageMedianPerc5Data> lStatsData =
                (List<SkyImageMedianPerc5Data>)
                    ServiceTools.ReadObjectFromXML(strStatsDataFilename, typeof(List<SkyImageMedianPerc5Data>));

            List<PointD> lPointsList = lStatsData.ConvertAll<PointD>(statsDatum => new PointD(statsDatum.GrIxStatsMedian, statsDatum.GrIxStatsPerc5));
            EventsDensityAnalysisForm Form1 = new EventsDensityAnalysisForm(lPointsList, defaultProperties, 512);



            if (cbxShowDensity.Checked)
            {
                Form1.Show();
                Form1.RepresentDensityField3D();
            }

            if (cbxClusterizePoints.Checked)
            {
                Form1.Clusterize();
            }

            if (cbxSaveClustering.Checked && cbxClusterizePoints.Checked)
            {
                Form1.SaveClusteringData(((string)defaultProperties["ClusteringXMLDataFilesLocation"]) + "clustering");
            }
        }


        private void btnSortImagesByClasses_Click(object sender, EventArgs e)
        {
            LogWindow errorsLogWindow = null;
            // прочитаем уже посчитанные данные по median и perc5
            string strStatsDataFilename = ((string)defaultProperties["DefaultMedianPerc5StatsXMLFile"]);
            List<SkyImageMedianPerc5Data> lData =
                (List<SkyImageMedianPerc5Data>)
                    ServiceTools.ReadObjectFromXML(strStatsDataFilename, typeof(List<SkyImageMedianPerc5Data>));

            string strClusteringDataFilesDir = ((string)defaultProperties["ClusteringXMLDataFilesLocation"]);
            //string strDataFilesDir = ((string)defaultProperties["DefaultDataFilesLocation"]);
            DirectoryInfo dir = new DirectoryInfo(strClusteringDataFilesDir);

            // получим условие на точки по умолчанию
            List<SkyImageMedianPerc5Data> lStatsData =
                (List<SkyImageMedianPerc5Data>)
                    ServiceTools.ReadObjectFromXML(strStatsDataFilename, typeof(List<SkyImageMedianPerc5Data>));

            List<PointD> lPointsList = lStatsData.ConvertAll<PointD>(statsDatum => new PointD(statsDatum.GrIxStatsMedian, statsDatum.GrIxStatsPerc5));
            EventsDensityAnalysisForm Form1 = new EventsDensityAnalysisForm(lPointsList, defaultProperties, 512);
            DenseMatrix dmDensityField = Form1.dmDensityMesh.Copy();
            double dMaxDensityValue = dmDensityField.Values.Max();
            double dDensityMinThreshold = dMaxDensityValue / 10.0d;

            Predicate<Point> conditionOnPoints = Form1.conditionOnPoints;

            if (!dir.Exists)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Операция не выполнена. Не найдена директория:" + Environment.NewLine + strClusteringDataFilesDir +
                    Environment.NewLine, true);
                return;
            }

            FileInfo[] fileListClusteringInfo = dir.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            if (fileListClusteringInfo.Length == 0)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Операция не выполнена. Не найдены описания кластеризации событий median-perc5 в директории:" +
                    Environment.NewLine + strClusteringDataFilesDir +
                    Environment.NewLine, true);
                return;
            }

            List<ClusteringData> lClusteringinfo = new List<ClusteringData>();
            List<string> directoriesForSorting = new List<string>();
            foreach (FileInfo info in fileListClusteringInfo)
            {
                ClusteringData currClusteringData =
                    (ClusteringData)ServiceTools.ReadObjectFromXML(info.FullName, typeof(ClusteringData));
                lClusteringinfo.Add(currClusteringData);
                string currClassDirName = info.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(info.FullName) + "\\";
                if (!ServiceTools.CheckIfDirectoryExists(currClassDirName))
                {
                    continue;
                }
                directoriesForSorting.Add(currClassDirName);

                //Image<Bgr, byte> imgShowingCurrCluster =
                //    new Image<Bgr, byte>(currClusteringData.rctOperationalAreaOnGrid.Width,
                //        currClusteringData.rctOperationalAreaOnGrid.Height);
                //foreach (Point pt in currClusteringData.lOverallPoints)
                //{
                //    imgShowingCurrCluster[pt.Y, pt.X] = new Bgr(255, 255, 255);
                //}
            }

            Image<Bgr, byte> imgShowingClusterTemplate =
                    new Image<Bgr, byte>(lClusteringinfo[0].rctOperationalAreaOnGrid.Width,
                        lClusteringinfo[0].rctOperationalAreaOnGrid.Height);
            RandomPastelColorGenerator colorGen = new RandomPastelColorGenerator();
            foreach (ClusteringData currClusteringData in lClusteringinfo)
            {
                Bgr currColor = new Bgr(colorGen.GetNext());
                foreach (Point pt in currClusteringData.lOverallPoints)
                {
                    if (conditionOnPoints(
                            pt.FlipUpsideDown(new Size(currClusteringData.rctOperationalAreaOnGrid.Width,
                                currClusteringData.rctOperationalAreaOnGrid.Height)))) continue;

                    if (dmDensityField[pt.Y, pt.X] < dDensityMinThreshold)
                    {
                        continue;
                    }

                    imgShowingClusterTemplate[pt.Y, pt.X] = currColor;
                }
            }
            //imgShowingClusterTemplate = imgShowingClusterTemplate*0.5d;
            foreach (ClusteringData currClusteringData in lClusteringinfo)
            {
                Image<Bgr, byte> imgShowingCluster = imgShowingClusterTemplate.Copy();
                foreach (Point pt in currClusteringData.lOverallPoints)
                {
                    if (conditionOnPoints(
                            pt.FlipUpsideDown(new Size(currClusteringData.rctOperationalAreaOnGrid.Width,
                                currClusteringData.rctOperationalAreaOnGrid.Height)))) continue;

                    if (dmDensityField[pt.Y, pt.X] < dDensityMinThreshold)
                    {
                        continue;
                    }

                    imgShowingCluster[pt.Y, pt.X] = new Bgr(255, 255, 255);
                }
                imgShowingCluster.Save(directoriesForSorting[lClusteringinfo.IndexOf(currClusteringData)] +
                                       "cluster-representation.jpg");
            }


            List<PointD> pointsRealSpace = lData.ConvertAll<PointD>(statsDatum => new PointD(statsDatum.GrIxStatsMedian, statsDatum.GrIxStatsPerc5));

            double minXval = pointsRealSpace.Min(pt => pt.X);
            double maxXval = pointsRealSpace.Max(pt => pt.X);
            double minYval = pointsRealSpace.Min(pt => pt.Y);
            double maxYval = pointsRealSpace.Max(pt => pt.Y);
            minXval = Math.Min(minXval, minYval);
            minYval = minXval;
            maxXval = Math.Max(maxXval, maxYval);
            maxYval = maxXval;

            int spaceDiscretization = lClusteringinfo[0].rctOperationalAreaOnGrid.Width;
            double xSpaceDiscrete = (maxXval - minXval) / spaceDiscretization;
            double ySpaceDiscrete = (maxYval - minYval) / spaceDiscretization;





            List<bool> bgwFinished = new List<bool>();
            List<BackgroundWorker> bgwList = new List<BackgroundWorker>();
            for (int i = 0; i < 16; i++)
            {
                bgwFinished.Add(true);
                bgwList.Add(null);
            }




            DoWorkEventHandler thisWorkerDoWorkHandler = delegate (object currBGWsender, DoWorkEventArgs args)
            {
                object[] currBGWarguments = (object[])args.Argument;
                SkyImageMedianPerc5Data medianPerc5Data = (SkyImageMedianPerc5Data)currBGWarguments[0];
                Dictionary<string, object> defaultProps = (Dictionary<string, object>)currBGWarguments[1];
                int currentBgwID = (int)currBGWarguments[2];


                double currMedian = medianPerc5Data.GrIxStatsMedian;
                double currPerc5 = medianPerc5Data.GrIxStatsPerc5;
                // найти точку в пространстве кластеризации, соответствующую этим данным
                int row = Convert.ToInt32((maxYval - currPerc5) / ySpaceDiscrete);
                int col = Convert.ToInt32((currMedian - minXval) / xSpaceDiscrete);
                col = Math.Min(col, spaceDiscretization - 1);
                row = Math.Min(row, spaceDiscretization - 1);

                if (dmDensityField[row, col] < dDensityMinThreshold)
                {
                    args.Result = new object[] { currentBgwID };
                    return;
                }

                foreach (ClusteringData clusteringData in lClusteringinfo)
                {
                    if (clusteringData.lOverallPoints.FindIndex(pt => ((pt.X == col) && (pt.Y == row))) != -1)
                    {
                        string currDirPath = directoriesForSorting[lClusteringinfo.IndexOf(clusteringData)];
                        FileInfo info = new FileInfo(medianPerc5Data.FileName);
                        string fullNameCopyTo = currDirPath + info.Name;
                        int i = 0;
                        while (File.Exists(fullNameCopyTo))
                        {
                            fullNameCopyTo = currDirPath + Path.GetFileNameWithoutExtension(info.FullName) +
                                             "-" + string.Format("{0:000}", i) + Path.GetExtension(info.FullName);
                            i++;
                        }


                        try
                        {
                            File.Copy(medianPerc5Data.FileName, fullNameCopyTo);
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                "copy: " + info.Name + "  >>  " + currDirPath + Environment.NewLine);
                        }
                        catch (Exception exc)
                        {
                            errorsLogWindow = ServiceTools.LogAText(errorsLogWindow,
                                "coudn`t copy file: " + info.Name + ":" + Environment.NewLine + exc.Message +
                                Environment.NewLine);
                        }

                        Application.DoEvents();
                    }
                }

                args.Result = new object[]
                {
                    currentBgwID
                };
            };




            RunWorkerCompletedEventHandler currWorkCompletedHandler =
                delegate (object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    int returningBGWthreadID = (int)currentBGWResults[0];

                    bgwFinished[returningBGWthreadID] = true;
                    bgwList[returningBGWthreadID].Dispose();
                    bgwList[returningBGWthreadID] = null;
                    // theLogWindow = ServiceTools.LogAText(theLogWindow, "finished processing: " + res.FileName);
                };


            int currDataIdx = 0;

            foreach (SkyImageMedianPerc5Data medianPerc5Data in lData)
            {
                int currentBgwID = -1;
                while (bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) == bgwFinished.Count)
                {
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(100);
                }
                for (int i = 0; i < bgwFinished.Count; i++)
                {
                    if (bgwFinished[i])
                    {
                        currentBgwID = i;
                        bgwFinished[i] = false;
                        break;
                    }
                }

                theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "starting: " + medianPerc5Data.FileName);
                theLogWindow = ServiceTools.LogAText(theLogWindow, "" + currDataIdx + " / " + lData.Count);
                theLogWindow.ClearLog();


                object[] BGWorker2Args = new object[] { medianPerc5Data, defaultProperties, currentBgwID };

                BackgroundWorker currBgw = new BackgroundWorker();
                bgwList[currentBgwID] = currBgw;
                currBgw.DoWork += thisWorkerDoWorkHandler;
                currBgw.RunWorkerCompleted += currWorkCompletedHandler;
                currBgw.WorkerReportsProgress = true;
                currBgw.RunWorkerAsync(BGWorker2Args);

                int progress = Convert.ToInt32(100.0d * (double)currDataIdx / (double)lData.Count);
                ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, progress);
                currDataIdx++;
            }


            while (bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) > 0)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(100);
            }

            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);
        }

        #endregion Median-perc5 distribution, heatmap, density processing




        #region Оценка положения диска Солнца "оптимизацией формы поля GrIx одним махом"

        private List<Tuple<string, DateTimeSpan>> NVdataFilesAlreadyReadDateTimeSpans =
            new List<Tuple<string, DateTimeSpan>>();
        private List<Tuple<string, List<IoffeVesselDualNavDataConverted>>> NVdataFilesAlreadyReadData =
            new List<Tuple<string, List<IoffeVesselDualNavDataConverted>>>();

        private void btnTestSunDetection2015_Click(object sender, EventArgs e)
        {
            // посчитать grix
            // сгладить это поле, но не слишком сильно
            // отфильтровать области слишком больших значений grix
            // отфильтровать области слишком больших значений градента grix
            // оставшееся поле интерполировать
            // попробовать по получившейся форме поля найти положение солнца

            if (imagetoadd == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Не загружено изображение для обработки.");
                return;
            }

            //FileInfo finfoOriginalFile = new FileInfo(ImageFileName);
            GPSdata gps = ServiceTools.FindProperGPSdataForImage(ImageFileName, theLogWindow, defaultProperties,
                ref NVdataFilesAlreadyReadDateTimeSpans, ref NVdataFilesAlreadyReadData);
            if (gps == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn`t find GPS data for this image.");
            }

            RoundData detectedSunPosition = SkyCloudClassification.ObtainSunPosition(ImageFileName, defaultProperties,
                this, theLogWindow, gps, true);



            FileInfo fInfo1 = new FileInfo(ImageFileName);
            //string sunDiskInfoFileName = fInfo1.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(ImageFileName) +
            //                             "-SunDiskInfo.xml";
            string sunDiskInfoFileName = ConventionalTransitions.SunDiskInfoFileName(ImageFileName,
                (string)defaultProperties["DefaultDataFilesLocation"]);



            RoundData existingRoundData = RoundData.nullRoundData();
            Size imgSizeUnderExistingRoundData = imagetoadd.Bitmap.Size;
            object existingRoundDataObj = ServiceTools.ReadObjectFromXML(sunDiskInfoFileName, typeof(RoundDataWithUnderlyingImgSize));

            if (existingRoundDataObj != null)
            {
                existingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).circle;
                imgSizeUnderExistingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).imgSize;
            }

            double currScale = (double)imagetoadd.Width / (double)imgSizeUnderExistingRoundData.Width;
            if (currScale != 1.0d)
            {
                existingRoundData.DCenterX *= currScale;
                existingRoundData.DCenterY *= currScale;
                existingRoundData.DRadius *= currScale;
            }

            SunDiskRepresentingAndCorrectionForm form = new SunDiskRepresentingAndCorrectionForm(imagetoadd, existingRoundData);
            DialogResult res = form.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            RoundData retSunDiskCircle = form.sunDiskPositionAndSize.Copy();
            Size imgSize = imagetoadd.Bitmap.Size;
            RoundDataWithUnderlyingImgSize tplSunDiskPositionData = new RoundDataWithUnderlyingImgSize();
            tplSunDiskPositionData.circle = retSunDiskCircle;
            tplSunDiskPositionData.imgSize = imgSize;

            ServiceTools.WriteObjectToXML(tplSunDiskPositionData, sunDiskInfoFileName);
        }

        #endregion Оценка положения диска Солнца "оптимизацией формы поля GrIx одним махом"






        private void btnCalcSunPosition_Click(object sender, EventArgs e)
        {
            //DateTime curDateTime = DateTime.UtcNow;
            if (imagetoadd == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Не загружено изображение для обработки!", true);
                return;
            }

            FileInfo finfoOriginalFile = new FileInfo(ImageFileName);

            defaultProperties["DefaultDataFilesLocation"] = defaultProperties["DefaultDataFilesLocation"] +
                                                            Path.GetFileNameWithoutExtension(finfoOriginalFile.Name) +
                                                            "\\";

            string defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
            ServiceTools.CheckIfDirectoryExists(defaultOutputDataDirectory);


            Image anImage = Image.FromFile(ImageFileName);
            ImageInfo newIInfo = new ImageInfo(anImage);
            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "processing file " + ImageFileName + Environment.NewLine);
            //int minute = 0;

            GPSdata neededGPSdata = ServiceTools.FindProperGPSdataForImage(ImageFileName, theLogWindow,
                defaultProperties, ref NVdataFilesAlreadyReadDateTimeSpans, ref NVdataFilesAlreadyReadData);
            if (neededGPSdata == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn`t find GPS data for this image.");
                return;
            }

            #region // obsolete
            //String dateTime = (String)newIInfo.getValueByKey("ExifDTOrig");
            //if (dateTime == null)
            //{
            //    //попробуем вытащить из имени файла
            //    string strDateTime = Path.GetFileName(ImageFileName);
            //    strDateTime = strDateTime.Substring(4, 19);
            //    dateTime = strDateTime;
            //}



            //try
            //{
            //    curDateTime = CommonTools.DateTimeOfString(dateTime);
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "picture got date/time: " + curDateTime.ToString("s"));
            //}
            //catch (Exception)
            //{
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "couldn`t get picture get date/time for file: " + Environment.NewLine + ImageFileName);
            //    return;
            //}
            //curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);

            //GPSdata neededGPSdata = new GPSdata();
            //string currPath = Path.GetDirectoryName(ImageFileName);
            //string[] xmlFileNames = Directory.GetFiles(currPath,
            //    "*data*" + curDateTime.ToString("s").Substring(0, 13).Replace(":", "-") + "*.xml"); // с точностью до часа
            //if (xmlFileNames.Count() > 0)
            //{
            //    List<GPSdata> lReadGPSdata = new List<GPSdata>();
            //    foreach (string xmlFileName in xmlFileNames)
            //    {
            //        Dictionary<string, object> dictSavedData = ServiceTools.ReadDictionaryFromXML(xmlFileName);
            //        //GPSdata gps = new GPSdata((string)dictSavedData["GPSdata"], GPSdatasources.CloudCamArduinoGPS);
            //        GPSdata gps =
            //            new GPSdata(new double[] { Convert.ToDouble(dictSavedData["GPSLat"]), Convert.ToDouble(dictSavedData["GPSLon"]) });
            //        lReadGPSdata.Add(gps);
            //    }
            //    lReadGPSdata.Sort((gpsRecord1, gpsRecord2) =>
            //    {
            //        double dev1 = Math.Abs((gpsRecord1.dateTimeUTC - curDateTime).TotalMilliseconds);
            //        double dev2 = Math.Abs((gpsRecord2.dateTimeUTC - curDateTime).TotalMilliseconds);
            //        return (dev1 >= dev2) ? (1) : (-1);
            //    });
            //    neededGPSdata = lReadGPSdata[0];


            //}
            //else
            //{
            //    //string navFilesPath =
            //    //    "D:\\_gulevlab\\SkyImagesAnalysis_appData\\images_complete\\IOFFE\\NIKON-D80\\IOFFE-Mission34-Marina-2011\\data-meteo-nav\\";
            //    string navFilesPath = defaultProperties["IoffeMeteoNavFilesDirectory"] as string;
            //    List<IoffeVesselDualNavDataConverted> lAllNavData = new List<IoffeVesselDualNavDataConverted>();

            //    string[] sNavFilenames = Directory.GetFiles(navFilesPath, "*.nv2", SearchOption.AllDirectories);
            //    if (sNavFilenames.Count() == 0)
            //    {
            //        theLogWindow = ServiceTools.LogAText(theLogWindow, "Не найдено файлов данных навигации", true);
            //        return;
            //    }
            //    else
            //    {
            //        foreach (string navFilename in sNavFilenames)
            //        {
            //            Tuple<DateTime, DateTime> timeSpan =
            //                IoffeVesselNavDataReader.GetNavFileDateTimeMargins(navFilename);
            //            if (timeSpan == null)
            //            {
            //                continue;
            //            }

            //            if ((curDateTime < timeSpan.Item1) || (curDateTime > timeSpan.Item2))
            //            {
            //                continue;
            //            }

            //            List<IoffeVesselDualNavDataConverted> dataHasBeenRead = IoffeVesselNavDataReader.ReadNavFile(navFilename);
            //            if (dataHasBeenRead == null)
            //            {
            //                continue;
            //            }
            //            theLogWindow = ServiceTools.LogAText(theLogWindow, "файл навигации прочитан: " + navFilename, true);
            //            Application.DoEvents();
            //            lAllNavData.AddRange(dataHasBeenRead);
            //        }
            //    }

            //    if (!lAllNavData.Any())
            //    {
            //        theLogWindow = ServiceTools.LogAText(theLogWindow, "Не найдено файлов данных с нужными данными", true);
            //        return;
            //    }

            //    lAllNavData.Sort((gpsRecord1, gpsRecord2) =>
            //    {
            //        double dev1 = Math.Abs((gpsRecord1.gps.dateTimeUTC - curDateTime).TotalMilliseconds);
            //        double dev2 = Math.Abs((gpsRecord2.gps.dateTimeUTC - curDateTime).TotalMilliseconds);
            //        return (dev1 >= dev2) ? (1) : (-1);
            //    });
            //    neededGPSdata = lAllNavData[0].gps;
            //}
            #endregion // obsolete


            theLogWindow = ServiceTools.LogAText(theLogWindow, "GPS: " + neededGPSdata.HRString());
            theLogWindow = ServiceTools.LogAText(theLogWindow, "GPS: " + neededGPSdata.ToString());
            if (neededGPSdata.dataSource == GPSdatasources.IOFFEvesselDataServer)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "vessel heading: " + neededGPSdata.IOFFEdataHeadingTrue.ToString("F2"));
            }



            double lat = neededGPSdata.LatDec;
            // double lat = 55.755826; // - Moscow
            double lon = neededGPSdata.LonDec;
            // double lon = 37.6173; // - Moscow




            SPA spaCalc = new SPA(neededGPSdata.dateTimeUTC.Year, neededGPSdata.dateTimeUTC.Month, neededGPSdata.dateTimeUTC.Day, neededGPSdata.dateTimeUTC.Hour,
                neededGPSdata.dateTimeUTC.Minute, neededGPSdata.dateTimeUTC.Second, (float)lon, (float)lat,
                (float)SPAConst.DeltaT(neededGPSdata.dateTimeUTC));
            int res = spaCalc.spa_calculate();
            AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                spaCalc.spa.zenith);
            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "SPA ext sun position for " + neededGPSdata.dateTimeUTC.ToString("s") + ": " + sunPositionSPAext);


            ImageProcessing imgP = new ImageProcessing(imagetoadd, true);
            Image<Bgr, byte> imgRes = imgP.significantMaskImageOctLined;
            RoundData sunZenithCircle = imgP.imageRD.Copy();
            sunZenithCircle.DRadius *= (sunPositionSPAext.ZenithAngle / 90.0d);
            imgRes.Draw(sunZenithCircle.CircleF(), new Bgr(Color.Orange), 3);
            imgRes.Save(defaultOutputDataDirectory + Path.GetFileNameWithoutExtension(ImageFileName) +
                        "-sun-detection-001.jpg");
            ServiceTools.ShowPicture(imgRes, Path.GetFileNameWithoutExtension(ImageFileName));
        }






        private void button5_Click(object sender, EventArgs e)
        {
            SunElevationTest sunElTestForm = new SunElevationTest();
            sunElTestForm.defaultProperties = defaultProperties;
            sunElTestForm.Show();
        }





        /// <summary>
        /// Handles the Click event of the btnCollectCameraPositioningData control.
        /// Просчитывает статистику по расположению солнечного диска на изображениях с целью определения ориентации камеры относительно судна.
        /// Для этого используются эмпирически (вручную, в файлах *-SunDiskInfo.xml) указанные данные о положении солнечного диска на снимке
        /// и данные о положении и ориентации (GPS & heading) судна, которые дают исходные данные для рассчета азимутального и зенитного угла
        /// расположения солнца.
        /// Отклонение должно оказаться относительно узко распределенным и в среднем дать угол поворота оси снимков камеры относительно оси судна,
        /// определяющей heading.
        /// 
        /// Для этого следует предварительно кластеризовать снимки в пространстве median-perc5 и использовать те,
        /// которые попадают в кластер максимально открытого неба - нижний левый
        /// Как вариант - применить классификацию на основе обучающей выборки. Но в любом случае следует выбрать набор случаев, когда наблюдается
        /// солнце в квадрате.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private CameraPositioningDataCollector collector = null;
        private void btnCollectCameraPositioningData_Click(object sender, EventArgs e)
        {
            if ((collector == null) || (!collector.IsBusy))
            {
                collector = new CameraPositioningDataCollector()
                {
                    theLogWindow = this.theLogWindow,
                    defaultProperties = this.defaultProperties,
                    ParentForm = this,
                    defaultPropertiesXMLfileName = this.defaultPropertiesXMLfileName,
                };

                ThreadSafeOperations.ToggleButtonState(sender as Button, true, "STOP", true);

                collector.CollectPositioningData();

                ThreadSafeOperations.ToggleButtonState(sender as Button, true, "Обработка директории (detect camera positioning)", false);
            }
            else
            {
                collector.StopProcessing();
                ThreadSafeOperations.ToggleButtonState(sender as Button, true, "Обработка директории (detect camera positioning)", false);
            }
        }





        private void btnCheckSunDiskCondition_Click(object sender, EventArgs e)
        {
            if (imagetoadd == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Не загружено изображение для обработки!", true);
                return;
            }

            FileInfo finfoOriginalFile = new FileInfo(ImageFileName);

            Dictionary<string, object> defaultProperties_SunDiskConditionApp = new Dictionary<string, object>();
            string defaultProperties_SunDiskMarks_XMLfileName = Directory.GetCurrentDirectory() +
                                         "\\settings\\SunPresenceCollectingApp-Settings.xml";
            if (!File.Exists(defaultProperties_SunDiskMarks_XMLfileName))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "coundn`t find sun presence app properties XML file SunPresenceCollectingApp-Settings.xml");
                return;
            }

            string CurDir = Directory.GetCurrentDirectory();
            CurDir = CurDir + ((CurDir.Last() == '\\') ? ("") : ("\\"));
            string SunDiskConditionXMLdataFilesDirectory = CurDir;
            string imageYRGBstatsXMLdataFilesDirectory = CurDir;
            string imageMP5statsXMLdataFilesDirectory = CurDir;
            string SerializedDecisionTreePath = CurDir;
            defaultProperties_SunDiskConditionApp = ServiceTools.ReadDictionaryFromXML(defaultProperties_SunDiskMarks_XMLfileName);

            if (defaultProperties_SunDiskConditionApp.ContainsKey("SunDiskConditionXMLdataFilesDirectory"))
            {
                SunDiskConditionXMLdataFilesDirectory = (string)defaultProperties_SunDiskConditionApp["SunDiskConditionXMLdataFilesDirectory"];
            }
            if (defaultProperties_SunDiskConditionApp.ContainsKey("imageYRGBstatsXMLdataFilesDirectory"))
            {
                imageYRGBstatsXMLdataFilesDirectory = (string)defaultProperties_SunDiskConditionApp["imageYRGBstatsXMLdataFilesDirectory"];
            }
            if (defaultProperties_SunDiskConditionApp.ContainsKey("imageMP5statsXMLdataFilesDirectory"))
            {
                imageMP5statsXMLdataFilesDirectory = (string)defaultProperties_SunDiskConditionApp["imageMP5statsXMLdataFilesDirectory"];
            }
            if (defaultProperties_SunDiskConditionApp.ContainsKey("SerializedDecisionTreePath"))
            {
                SerializedDecisionTreePath = ((string)defaultProperties_SunDiskConditionApp["SerializedDecisionTreePath"]);
            }

            if (!Directory.Exists(SerializedDecisionTreePath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "ERROR: cant find SerializedDecisionTreePath path. Will not continue.");
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
            else
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "couldn`t find any decision tree data file. Will not proceed.");
                return;
            }

            string imageYRGBstatsXMLdataFileName = ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(
                ImageFileName, imageYRGBstatsXMLdataFilesDirectory);
            if (File.Exists(imageYRGBstatsXMLdataFileName))
            {
                SkyImageIndexesStatsData grixyrgbStatsData =
                    ServiceTools.ReadObjectFromXML(imageYRGBstatsXMLdataFileName, typeof(SkyImageIndexesStatsData)) as
                        SkyImageIndexesStatsData;
                DecisionTree tree = DecisionTree.Load(treeFileName);

                //double[][] modelInputs = null;
                double[] modelInputs = grixyrgbStatsData.ToRawDoubleValuesEnumerable().ToArray<double>();
                int intPredictedOutput = tree.Compute(modelInputs);
                SunDiskCondition sdcPredictedOutput = (SunDiskCondition)intPredictedOutput;
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "this image has been detected as " + sdcPredictedOutput.ToString());
            }
            else
            {
                BackgroundWorker bgwCurrImageProcessing = new BackgroundWorker();
                bgwCurrImageProcessing.DoWork += ImageProcessing.CalculateImageStatsData_BGWdoWork;
                bgwCurrImageProcessing.RunWorkerCompleted += ((sndr, eventArgs) =>
                {
                    ThreadSafeOperations.SetLoadingCircleState(wcCalculatingImageStats, false, false,
                        wcCalculatingImageStats.Color);
                    object[] objOutputValues = eventArgs.Result as object[];
                    if (!((bool)objOutputValues[2]))
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "Failed to calculate image statistics. Will not proceed.");
                        Exception ex = objOutputValues[4] as Exception;

                        #region report

#if DEBUG
                        ServiceTools.ExecMethodInSeparateThread(this, () =>
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                "exception has been thrown: " + ex.Message + Environment.NewLine +
                                ServiceTools.CurrentCodeLineDescription());
                        });
#else
                ServiceTools.ExecMethodInSeparateThread(this, () =>
                {
                    ServiceTools.logToTextFile(errorLogFilename,
                        "exception has been thrown: " + ex.Message + Environment.NewLine +
                        ServiceTools.CurrentCodeLineDescription(), true, true);
                });
#endif

                        #endregion report
                    }

                    string currFileName = objOutputValues[0] as string;
                    SkyImageIndexesStatsData grixyrgbStatsData = objOutputValues[3] as SkyImageIndexesStatsData;
                    ServiceTools.WriteObjectToXML(grixyrgbStatsData,
                        ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currFileName,
                            imageYRGBstatsXMLdataFilesDirectory));
                    SkyImageMedianPerc5Data mp5dt = objOutputValues[1] as SkyImageMedianPerc5Data;
                    ServiceTools.WriteObjectToXML(mp5dt,
                        ConventionalTransitions.ImageGrIxMedianP5DataFileName(currFileName,
                            imageMP5statsXMLdataFilesDirectory));

                    DecisionTree tree = DecisionTree.Load(treeFileName);

                    //double[][] modelInputs = null;
                    double[] modelInputs = grixyrgbStatsData.ToRawDoubleValuesEnumerable().ToArray<double>();
                    int intPredictedOutput = tree.Compute(modelInputs);
                    SunDiskCondition sdcPredictedOutput = (SunDiskCondition)intPredictedOutput;
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "this image has been detected as " + sdcPredictedOutput.ToString());

                });
                object[] BGWorker2Args = new object[] { ImageFileName };
                bgwCurrImageProcessing.RunWorkerAsync(BGWorker2Args);
                ThreadSafeOperations.SetLoadingCircleState(wcCalculatingImageStats, true, true,
                    wcCalculatingImageStats.Color);
            }
        }





        private void btnPrevImgInDirectory_Click(object sender, EventArgs e)
        {
            if (ImageFileName == "") return;
            if (!File.Exists(ImageFileName)) return;

            FileInfo currFileInfo = new FileInfo(ImageFileName);
            string path = currFileInfo.DirectoryName + ((currFileInfo.DirectoryName.Last() == '\\') ? ("") : ("\\"));

            List<string> lImagesFilenames =
                new List<string>(Directory.GetFiles(path, "*.jpg", SearchOption.TopDirectoryOnly));
            if (!lImagesFilenames.Any()) return;

            if (!lImagesFilenames.Contains(currFileInfo.FullName)) return;
            int idx = lImagesFilenames.IndexOf(currFileInfo.FullName);
            if (idx == 0) return;

            idx--;
            string newFileName = lImagesFilenames[idx];

            OpenFile(newFileName);
        }



        private void btnNextImgInDirectory_Click(object sender, EventArgs e)
        {
            if (ImageFileName == "") return;
            if (!File.Exists(ImageFileName)) return;

            FileInfo currFileInfo = new FileInfo(ImageFileName);
            string path = currFileInfo.DirectoryName + ((currFileInfo.DirectoryName.Last() == '\\') ? ("") : ("\\"));

            List<string> lImagesFilenames =
                new List<string>(Directory.GetFiles(path, "*.jpg", SearchOption.TopDirectoryOnly));
            if (!lImagesFilenames.Any()) return;

            if (!lImagesFilenames.Contains(currFileInfo.FullName)) return;
            int idx = lImagesFilenames.IndexOf(currFileInfo.FullName);
            if (idx == lImagesFilenames.Count - 1) return;

            idx++;
            string newFileName = lImagesFilenames[idx];

            OpenFile(newFileName);
        }




        private void helpLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            theLogWindow = ServiceTools.LogAText(theLogWindow, "" +
                                                               "Это означает, что для обработки изображения будет использовано предварительно определенное положение солнечного диска." + Environment.NewLine +
                                                               "Если файл с этими данными не будет найден, - значит, солнечный диск на изображении считается не представленным." + Environment.NewLine +
                                                               "Это, например, будет означать, что форсированно не будет использоваться схема подавления солнечной засветки.");
        }





        #region bgwBatchProcessingUsingXMLListOfFiles


        private BackgroundWorker bgwBatchProcessingUsingXMLListOfFiles = null;
        private CancellationTokenSource cts;

        private void FINISH()
        {
            theLogWindow = ServiceTools.LogAText(theLogWindow, "====  FINISH ====");
        }


        private void btnProcessFilesUsingXMLListFile_Click(object sender, EventArgs e)
        {
            if (bgwBatchProcessingUsingXMLListOfFiles == null)
            {
                StartBatchProcessingUsingXMLListOfFiles();
            }
            else if (!bgwBatchProcessingUsingXMLListOfFiles.IsBusy)
            {
                StartBatchProcessingUsingXMLListOfFiles();
            }
            else
            {
                // bgwBatchProcessingUsingXMLListOfFiles.CancelAsync();
                cts.Cancel();
            }
        }



        private void StartBatchProcessingUsingXMLListOfFiles()
        {
            bgwBatchProcessingUsingXMLListOfFiles = new BackgroundWorker();
            bgwBatchProcessingUsingXMLListOfFiles.WorkerSupportsCancellation = true;
            bgwBatchProcessingUsingXMLListOfFiles.WorkerReportsProgress = true;
            bgwBatchProcessingUsingXMLListOfFiles.DoWork += BgwBatchProcessingUsingXMLListOfFiles_DoWork;
            bgwBatchProcessingUsingXMLListOfFiles.ProgressChanged += BgwBatchProcessingUsingXMLListOfFiles_ProgressChanged;
            bgwBatchProcessingUsingXMLListOfFiles.RunWorkerCompleted += BgwBatchProcessingUsingXMLListOfFiles_RunWorkerCompleted;


            // Найти и прочитать XML-файл - список файлов на обработку со всеми сопровождающими данными
            if (!File.Exists(FilesAndConcurrentDataListToProcessForCloudCover))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Unable to find or read XML file containing list of images to process: " +
                    FilesAndConcurrentDataListToProcessForCloudCover);
                FINISH();
                return;
            }


            theLogWindow = ServiceTools.LogAText(theLogWindow, "Multiple images computing starting with the following parameters:");
            string strToDisplay = CommonTools.DictionaryRepresentation(defaultProperties);
            theLogWindow = ServiceTools.LogAText(theLogWindow, strToDisplay);

            simpleMultipleImagesShow imagesRepresentingForm = new simpleMultipleImagesShow();
            imagesRepresentingForm.Show();

            List<ClassificationMethods> schemesToUse = new List<ClassificationMethods>();
            schemesToUse.Add(ClassificationMethods.Japan);
            schemesToUse.Add(ClassificationMethods.US);
            schemesToUse.Add(ClassificationMethods.GrIx);

            object[] bgwArgs = new object[]
                {
                    FilesAndConcurrentDataListToProcessForCloudCover,
                    theLogWindow,
                    imagesRepresentingForm,
                    schemesToUse
                };

            ThreadSafeOperations.ToggleButtonState(btnProcessFilesUsingXMLListFile, true, "Прекратить обработку", true);

            bgwBatchProcessingUsingXMLListOfFiles.RunWorkerAsync(bgwArgs);
        }




        private void BgwBatchProcessingUsingXMLListOfFiles_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.ToggleButtonState(btnProcessFilesUsingXMLListFile, true,
                "Обработка файлов по списку из XML-файла с сопровождающими данными и статистиками (FilesAndConcurrentDataListToProcessForCloudCover)",
                false);
            FINISH();
        }




        private void BgwBatchProcessingUsingXMLListOfFiles_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, e.ProgressPercentage);
        }




        private void BgwBatchProcessingUsingXMLListOfFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;

            object[] bgwArgs = e.Argument as object[];
            string FilesAndConcurrentDataListToProcessForCloudCover = (string)bgwArgs[0];
            LogWindow theLogWindow = (LogWindow)bgwArgs[1];
            simpleMultipleImagesShow imagesRepresentingForm = (simpleMultipleImagesShow)bgwArgs[2];
            List<ClassificationMethods> schemesToUse = (List<ClassificationMethods>)bgwArgs[3];

            int maxConcurrentImagesProcessing = 4;
            if (defaultProperties.ContainsKey("MaxConcurrentFilesProcessingCount"))
            {
                maxConcurrentImagesProcessing = Convert.ToInt32(defaultProperties["MaxConcurrentFilesProcessingCount"]);
            }

            List<SkyImagesDataWith_Concurrent_Stats_CloudCover> lImagesFilteredByAnyAvailableData =
                    new List<SkyImagesDataWith_Concurrent_Stats_CloudCover>();

            #region Прочитать список файлов на обработку

            try
            {
                lImagesFilteredByAnyAvailableData =
                    (List<SkyImagesDataWith_Concurrent_Stats_CloudCover>)
                        ServiceTools.ReadObjectFromXML(FilesAndConcurrentDataListToProcessForCloudCover,
                            typeof(List<SkyImagesDataWith_Concurrent_Stats_CloudCover>));
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Unable to find or read XML file containing list of images to process: " +
                    FilesAndConcurrentDataListToProcessForCloudCover);
                e.Result = false;
                return;
            }

            #endregion


            int defaultMaxImageSize = Convert.ToInt32(defaultProperties["DefaultMaxImageSize"]);

            string outputStatsDirectory = (string)defaultProperties["DefaultDataFilesLocation"];

            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);

            Type imagesRepresentingFormType = imagesRepresentingForm.GetType();
            MethodInfo thePicturePlacingMethodInfo = imagesRepresentingFormType.GetMethod("PlaceAPicture");


            string strOutputWithFnamesFileName = outputStatsDirectory + "statsWithFNames.dat";
            string strOutputWOfnamesFileName = outputStatsDirectory + "stats.dat";

            if (!File.Exists(strOutputWOfnamesFileName))
            {
                ServiceTools.logToTextFile(strOutputWOfnamesFileName,
                    "datetime;SI_US;SI_Jap;SI_GrIx;SB_suppr;SDC;TrueClCov" +
                    Environment.NewLine, true);
            }

            if (!File.Exists(strOutputWithFnamesFileName))
            {
                ServiceTools.logToTextFile(strOutputWithFnamesFileName,
                    "filename;datetime;SI_US;SI_Jap;SI_GrIx;SB_suppr;SDC;TrueClCov" +
                    Environment.NewLine, true);
            }
            



            #region Estimate SDC and filter list for if NoSun or Sun0

            // SunDiskCondition detectedSDC = SunDiskCondition.Defect;

            List<string> lCalculatedData = lImagesFilteredByAnyAvailableData.ConvertAll(inp =>
            {
                ConcurrentData nearestConcurrentData = inp.concurrentData;
                SkyImageIndexesStatsData currImageStatsData = inp.grixyrgbStats;
                return currImageStatsData.ToCSV() + "," +
                    nearestConcurrentData.gps.SunZenithAzimuth().ElevationAngle.ToString().Replace(",", ".") + "," +
                    nearestConcurrentData.gps.SunZenithAzimuth().Azimuth.ToString().Replace(",", ".");
            });

            string csvHeader = lImagesFilteredByAnyAvailableData[0].grixyrgbStats.CSVHeader() +
                               ",SunElevationDeg,SunAzimuthDeg,sunDiskCondition";
            

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
            

            List<List<string>> csvFileContentStringsFiltered_wo_sdc = csvFileContentStringsFiltered;

            List<DenseVector> lDV_objects_features =
                csvFileContentStringsFiltered_wo_sdc.ConvertAll(
                    list =>
                        DenseVector.OfEnumerable(list.ConvertAll<double>(str => Convert.ToDouble(str.Replace(".", ",")))));


            DenseVector dvMeans = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV((string)defaultProperties["NormMeansFile"], 0, ",")).Row(0);
            DenseVector dvRanges = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV((string)defaultProperties["NormRangeFile"], 0, ", ")).Row(0);

            lDV_objects_features = lDV_objects_features.ConvertAll(dv =>
            {
                DenseVector dvShifted = dv - dvMeans;
                DenseVector dvNormed = (DenseVector)dvShifted.PointwiseDivide(dvRanges);
                return dvNormed;
            });

            DenseMatrix dmObjectsFeatures = DenseMatrix.OfRowVectors(lDV_objects_features);

            DenseVector dvThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV((string)defaultProperties["NNtrainedParametersFile"], 0, ", ");
            List<int> NNlayersConfig =
                new List<double>(((DenseMatrix)ServiceTools.ReadDataFromCSV((string)defaultProperties["NNconfigFile"], 0, ", ")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));


            List<List<double>> lDecisionProbabilities = null;

            List<SunDiskCondition> predictedSDClist =
                NNclassificatorPredictor<SunDiskCondition>.NNpredict(dmObjectsFeatures, dvThetaValues, NNlayersConfig,
                    out lDecisionProbabilities, SunDiskConditionData.MatlabEnumeratedSDCorderedList()).ToList();

            //List<SunDiskCondition> predictedSDClist = predictedSDC.ConvertAll(sdcInt =>
            //{
            //    switch (sdcInt)
            //    {
            //        case 4:
            //            return SunDiskCondition.NoSun;
            //            break;
            //        case 1:
            //            return SunDiskCondition.Sun0;
            //            break;
            //        case 2:
            //            return SunDiskCondition.Sun1;
            //            break;
            //        case 3:
            //            return SunDiskCondition.Sun2;
            //            break;
            //        default:
            //            return SunDiskCondition.Defect;
            //    }
            //});

            #endregion predict using LIST of data


            //lImagesFilteredByAnyAvailableData =
            //    lImagesFilteredByAnyAvailableData.Where(
            //        (val, idx) =>
            //            ((predictedSDClist[idx] == SunDiskCondition.NoSun) ||
            //             (predictedSDClist[idx] == SunDiskCondition.Sun0))).ToList();
            lImagesFilteredByAnyAvailableData =
                lImagesFilteredByAnyAvailableData.Where(
                    (val, idx) =>
                        ((predictedSDClist[idx] == SunDiskCondition.Sun1) ||
                         (predictedSDClist[idx] == SunDiskCondition.Sun2))).ToList();


            #endregion Estimate SDC and filter list for if NoSun or Sun0




            if (File.Exists(strOutputWithFnamesFileName))
            {
                #region read already processed list and exclude it from the calculation list

                int count_before_filtering = lImagesFilteredByAnyAvailableData.Count;

                List<List<string>> lAlreadyCalculatedStatsWithFNames =
                    ServiceTools.ReadDataFromCSV(strOutputWithFnamesFileName, 1, true);
                List<string> lFilenamesAlreadyProcessed =
                    lAlreadyCalculatedStatsWithFNames.ConvertAll(lstr => Path.GetFileName(lstr[0]));

                lImagesFilteredByAnyAvailableData =
                    lImagesFilteredByAnyAvailableData.Where(
                        ipd => !lFilenamesAlreadyProcessed.Contains(ipd.skyImageFileName)).ToList();

                int count_after_filtering = lImagesFilteredByAnyAvailableData.Count;

                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Excluded " + (count_before_filtering - count_after_filtering) +
                    " files already processed. Remains " + count_after_filtering);

                #endregion read already processed list and exclude it from the calculation list
            }




            int filecount = 0;
            int totalFilesToProcess = lImagesFilteredByAnyAvailableData.Count;
            int currPerc = 0;
            theLogWindow = ServiceTools.LogAText(theLogWindow, "TOTAL files to process: " + totalFilesToProcess);

            cts = new CancellationTokenSource();
            ParallelOptions parOpts = new ParallelOptions()
            {
                MaxDegreeOfParallelism = maxConcurrentImagesProcessing,
                CancellationToken = cts.Token
            };



            try
            {
                Parallel.ForEach<SkyImagesDataWith_Concurrent_Stats_CloudCover>(lImagesFilteredByAnyAvailableData, parOpts, (imgInfoToProcess) =>
                {

                    //foreach (SkyImagesDataWith_Concurrent_Stats_CloudCover imgInfoToProcess in lImagesFilteredByAnyAvailableData)
                    //{
                    Bitmap bmJ = null, bmG = null, bmGrIx = null;


                    #region calculate and represent progress

                    Interlocked.Increment(ref filecount);
                    int currentFileProcessingCounter = filecount;
                    int progressPercentage = Convert.ToInt32(((double)currentFileProcessingCounter / (double)totalFilesToProcess));
                    if (progressPercentage > currPerc)
                    {
                        currPerc = progressPercentage;
                        selfWorker.ReportProgress(progressPercentage);
                    }

                    #endregion calculate and represent progress

                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        imgInfoToProcess.skyImageFileName + "   (" + currentFileProcessingCounter + "//" + totalFilesToProcess + ")");

                    string strToDisplay = "";

                    string strDateTime = imgInfoToProcess.currImageDateTime.ToString("s");

                    strToDisplay += strDateTime;

                    Image<Bgr, Byte> img2process = new Image<Bgr, byte>(imgInfoToProcess.skyImageFullFileName);

                    ThreadSafeOperations.UpdatePictureBox(pictureBox1, img2process.Bitmap, true);

                    #region Japan

                    if (schemesToUse.Contains(ClassificationMethods.Japan))
                    {
                        img2process = ImageProcessing.ImageResizer(img2process, defaultMaxImageSize);

                        SkyCloudClassification currClassificator = new SkyCloudClassification(img2process, defaultProperties)
                        {
                            LogWindow = theLogWindow,
                            ParentForm = this,
                            ClassificationMethod = ClassificationMethods.Japan,
                            isCalculatingUsingBgWorker = true,
                            SelfWorker = selfWorker,
                            defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"],
                            cloudSkySeparationValue = Convert.ToDouble(defaultProperties["JapanCloudSkySeparationValue"]),
                            sourceImageFileName = imgInfoToProcess.skyImageFullFileName,
                            imgConcurrentDataAndStats = imgInfoToProcess
                        };

                        try
                        {
                            currClassificator.Classify();
                        }
                        catch (Exception ex)
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                Environment.NewLine + Environment.NewLine + "=== === ERROR classifying with method: " +
                                currClassificator.ClassificationMethod + Environment.NewLine + ex.Message + Environment.NewLine +
                                " === === " + Environment.NewLine + Environment.NewLine);
                        }

                        currClassificator.resultingStatusMessages = "";
                        Bitmap currBitmapSI = new Bitmap(currClassificator.PreviewBitmap);

                        ServiceTools.FlushMemory();

                        // ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                        // pictureBox2Bitmap = new Bitmap(currBitmapSI);
                        strToDisplay += ";" + Math.Round(currClassificator.CloudCover, 2).ToString();
                        // UpdateSIIFMlabels(1, currClassificator.CloudCover);
                        bmJ = new Bitmap(currBitmapSI);
                    }

                    #endregion Japan

                    if (selfWorker.CancellationPending)
                    {
                        // break;
                        return;
                    }


                    #region US

                    if (schemesToUse.Contains(ClassificationMethods.US))
                    {
                        SkyCloudClassification currClassificator = new SkyCloudClassification(img2process, defaultProperties)
                        {
                            LogWindow = theLogWindow,
                            ParentForm = this,
                            ClassificationMethod = ClassificationMethods.US,
                            isCalculatingUsingBgWorker = true,
                            SelfWorker = sender as BackgroundWorker,
                            defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"],
                            cloudSkySeparationValue = Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]),
                            sourceImageFileName = imgInfoToProcess.skyImageFullFileName,
                            imgConcurrentDataAndStats = imgInfoToProcess
                        };

                        try
                        {
                            currClassificator.Classify();
                        }
                        catch (Exception ex)
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                Environment.NewLine + Environment.NewLine + "=== === ERROR classifying with method: " +
                                currClassificator.ClassificationMethod + Environment.NewLine + ex.Message + Environment.NewLine +
                                " === === " + Environment.NewLine + Environment.NewLine);
                        }
                        currClassificator.resultingStatusMessages = "";
                        Bitmap currBitmapSI = new Bitmap(currClassificator.PreviewBitmap);
                        ServiceTools.FlushMemory();
                        // ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                        // pictureBox2Bitmap = new Bitmap(bitmapSI);
                        strToDisplay += ";" + Math.Round(currClassificator.CloudCover, 2).ToString();
                        // UpdateSIIFMlabels(1, currClassificator.CloudCover);

                        bmG = new Bitmap(currBitmapSI);
                    }

                    #endregion US

                    if (selfWorker.CancellationPending)
                    {
                        // break;
                        return;
                    }


                    #region GrIx

                    SkyCloudClassification currClassificatorGrIx = null;
                    if (schemesToUse.Contains(ClassificationMethods.GrIx))
                    {
                        currClassificatorGrIx = new SkyCloudClassification(img2process, defaultProperties)
                        {
                            LogWindow = theLogWindow,
                            ParentForm = this,
                            ClassificationMethod = ClassificationMethods.GrIx,
                            forceExistingSunInformation = cbxForceExistingSunInformation.Checked,
                            isCalculatingUsingBgWorker = true,
                            SelfWorker = sender as BackgroundWorker,
                            defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"],
                            cloudSkySeparationValue = Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]),
                            theStdDevMarginValueDefiningSkyCloudSeparation =
                                Convert.ToDouble(defaultProperties["GrIxDefaultSkyCloudMarginWithoutSun"]),
                            sourceImageFileName = imgInfoToProcess.skyImageFullFileName,
                            imgConcurrentDataAndStats = imgInfoToProcess
                        };

                        try
                        {
                            currClassificatorGrIx.Classify();
                        }
                        catch (Exception ex)
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                Environment.NewLine + Environment.NewLine + "=== === ERROR classifying with method: " +
                                currClassificatorGrIx.ClassificationMethod + Environment.NewLine + ex.Message + Environment.NewLine +
                                " === === " + Environment.NewLine + Environment.NewLine);
                        }
                        currClassificatorGrIx.resultingStatusMessages = "";
                        Bitmap currBitmapSI = new Bitmap(currClassificatorGrIx.PreviewBitmap);
                        ServiceTools.FlushMemory();
                        //ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                        //pictureBox2Bitmap = new Bitmap(bitmapSI);
                        strToDisplay += ";" + Math.Round(currClassificatorGrIx.CloudCover, 2).ToString();
                        // UpdateSIIFMlabels(1, currClassificator.CloudCover);
                        bmGrIx = new Bitmap(currBitmapSI);
                    }

                    #endregion GrIx


                    strToDisplay += ";" + imgInfoToProcess.observedCloudCoverData.CloudCoverTotal;



                    int verbosityLevel = Convert.ToInt32(defaultProperties["GrIxProcessingVerbosityLevel"]);
                    string randomFileName = currClassificatorGrIx.randomFileName;
                    string outputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
                    if (verbosityLevel > 1)
                    {
                        //сохраним картинки
                        string sourceFName = outputDataDirectory + randomFileName + "_SourceImage.jpg";
                        string japanFName = outputDataDirectory + randomFileName + "_si_jap.jpg";
                        string germanFName = outputDataDirectory + randomFileName + "_si_US.jpg";
                        string GrIxFName = outputDataDirectory + randomFileName + "_si_GrIx.jpg";

                        img2process.Save(sourceFName);
                        bmJ.Save(japanFName, ImageFormat.Jpeg);
                        bmG.Save(germanFName, ImageFormat.Jpeg);
                        bmGrIx.Save(GrIxFName, ImageFormat.Jpeg);

                    }

                    #region // obsolete

                    //object[] parametersArray = new object[] { img2process.Bitmap, imgInfoToProcess.skyImageFileName, 1 };
                    //thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);

                    //if (schemesToUse.Contains(ClassificationMethods.Japan))
                    //{
                    //    parametersArray = new object[] { bmJ, "Japan", 2 };
                    //    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                    //}

                    //if (schemesToUse.Contains(ClassificationMethods.US))
                    //{
                    //    parametersArray = new object[] { bmG, "US", 3 };
                    //    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                    //}

                    //if (schemesToUse.Contains(ClassificationMethods.GrIx))
                    //{
                    //    parametersArray = new object[] { bmGrIx, "GrIx", 4 };
                    //    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                    //}

                    #endregion // obsolete



                    theLogWindow = ServiceTools.LogAText(theLogWindow, strToDisplay);
                    ServiceTools.logToTextFile(strOutputWithFnamesFileName,
                        imgInfoToProcess.skyImageFullFileName + ";" + strToDisplay + ";" +
                        currClassificatorGrIx.theSunSuppressionSchemeApplicable.ToString() + Environment.NewLine, true);
                    ServiceTools.logToTextFile(strOutputWOfnamesFileName, strToDisplay + Environment.NewLine, true);


                    if (selfWorker.CancellationPending)
                    {
                        // break;
                        parOpts.CancellationToken.ThrowIfCancellationRequested();
                        return;
                    }
                    //}
                });
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                FINISH();
                return;
            }
            finally
            {
                cts.Dispose();
                FINISH();
            }
            

            theLogWindow = ServiceTools.LogAText(theLogWindow, "TOTAL files processed: " + totalFilesToProcess);
        }


        #endregion bgwBatchProcessingUsingXMLListOfFiles
    }
}