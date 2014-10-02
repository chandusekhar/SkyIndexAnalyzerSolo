using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Text;
using System.Drawing.Imaging;
using System.Xml;
using System.Threading;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.Statistics;
using SkyIndexAnalyzerLibraries;


namespace SkyIndexAnalyzerSolo
{
    public enum pb2Source { SkyindexAnalyzer, ConnectedObjectsDetector, SkyCloudClassificator };

    public partial class SkyIndexAnalyzer_AnalysisForm : Form
    {
        #region vars

        private Assembly _assembly;
        private StreamReader _textStreamReader;
        //private Image imagetoadd;
        private Image<Bgr, Byte> imagetoadd;
        private Bitmap bitmapSI;
        private AboutBox1 aboutprog;
        private TimeSpan timetotal; 
        private DateTime begintotal; 
        private string ImageFileName;
        public FileStream CCDataFile, EXIFdataFile;
        private FileStream StatDataFile1;
        private bool gotexifs;
        public double tunedSIMargin;
        private double tunedSIMarginDefault;
        public System.Data.DataTable datatableFnumber, datatableISOspeed, datatableExposureTime;
        private delegate void DelegateOpenFile(String s);
        private double resizevalue;
        private int FileCounter, filecounter_prev;
        private int SIClScMarginRangeLowerValue, SIClScMarginRangeHigherValue;
        private SkyCloudClassification classificator;
        private Bitmap pictureBox2Bitmap;
        private Dictionary<string, object> defaultProperties = null;
        private LogWindow theLogWindow = null;
        #endregion

        private DelegateOpenFile m_DelegateOpenFile;

        #region //DEPRECATED
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
        #endregion //DEPRECATED



        private void btnProperties_Click(object sender, EventArgs e)
        {
            object propertiesObj = Properties.Settings.Default;
            PropertiesEditor propForm = new PropertiesEditor(propertiesObj);
            propForm.FormClosed += new FormClosedEventHandler(PropertiesFormClosed);
            propForm.ShowDialog();
        }



        public void PropertiesFormClosed(object sender, FormClosedEventArgs e)
        {
            readDefaultProperties();
        }



        public SkyIndexAnalyzer_AnalysisForm()
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
            gotexifs = false;

            ThreadSafeOperations.UpdatePictureBox(pictureBox1, imagetoadd.Bitmap, true);
            ThreadSafeOperations.UpdatePictureBox(pictureBox2, null);
            pictureBox2Bitmap = null;

            FileInfo finfo = new FileInfo(ImageFileName);
            string shortfname1 = finfo.Name;

            //SkyCloudClassificator = null;
            //ObjectsDetector = null;
            //ThreadSafeOperations.ToggleButtonState(btnDefineConnectedObjects, true, "Загрузить в классификатор", false);
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

            //ProcessingConditionsChanged = false;



            _assembly = Assembly.GetExecutingAssembly();
            try
            {
                Stream Stream1 = _assembly.GetManifestResourceStream("SkyIndexAnalyzerSolo.Resources.Ex3SumSchema.xml");
                _textStreamReader = new StreamReader(Stream1);
            }
            catch
            {
                MessageBox.Show("Error accessing resources!");

            }

            ReadEx3CalculateScheme();

            ThreadSafeOperations.SetText(label9, "Изменять исходный размер: 1.0", false);
            ThreadSafeOperations.MoveTrackBar(trackBar3, 7);

            button1.Text = "Обработка директории: ";

            //reportingTextBox = textBox1;
        }



        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutprog = new AboutBox1();
            aboutprog.Show(this);
        }



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



        private void ReadEx3CalculateScheme()
        {
            XmlReader reader;
            XmlDocument xmlfile;
            DataRow row1;

            datatableISOspeed = new System.Data.DataTable();
            datatableISOspeed.Columns.Add("Value", typeof(double));
            datatableISOspeed.Columns.Add("sum", typeof(double));
            datatableFnumber = new System.Data.DataTable();
            datatableFnumber.Columns.Add("Value", typeof(double));
            datatableFnumber.Columns.Add("sum", typeof(double));
            datatableExposureTime = new System.Data.DataTable();
            datatableExposureTime.Columns.Add("Value", typeof(double));
            datatableExposureTime.Columns.Add("sum", typeof(double));

            reader = XmlReader.Create(_textStreamReader);
            xmlfile = new XmlDocument();
            xmlfile.Load(reader);

            XmlNode ISOspeedTree = xmlfile.GetElementsByTagName("ISOSpeedTable").Item(0);
            for (int n = 0; n < ISOspeedTree.ChildNodes.Count; n++)
            {
                XmlNode ISODataRaw = ISOspeedTree.ChildNodes.Item(n);
                double ISOvalue = Convert.ToDouble(ISODataRaw.ChildNodes.Item(0).InnerText);
                double sum = Convert.ToDouble(ISODataRaw.ChildNodes.Item(1).InnerText);
                row1 = datatableISOspeed.NewRow();
                row1["Value"] = ISOvalue;
                row1["sum"] = sum;
                datatableISOspeed.Rows.Add(row1);
            }
            XmlNode FNumberTree = xmlfile.GetElementsByTagName("FNumberTable").Item(0);
            for (int n = 0; n < FNumberTree.ChildNodes.Count; n++)
            {
                XmlNode FNumberDataRaw = FNumberTree.ChildNodes.Item(n);
                double FNumberValue = Convert.ToDouble(FNumberDataRaw.ChildNodes.Item(0).InnerText);
                double sum = Convert.ToDouble(FNumberDataRaw.ChildNodes.Item(1).InnerText);
                row1 = datatableFnumber.NewRow();
                row1["Value"] = FNumberValue;
                row1["sum"] = sum;
                datatableFnumber.Rows.Add(row1);
            }
            XmlNode ExposureTimeTree = xmlfile.GetElementsByTagName("ExposureTimeTable").Item(0);
            for (int n = 0; n < ExposureTimeTree.ChildNodes.Count; n++)
            {
                XmlNode ExposureTimeDataRaw = ExposureTimeTree.ChildNodes.Item(n);
                double ExposureTimevalue = Convert.ToDouble(ExposureTimeDataRaw.ChildNodes.Item(0).InnerText);
                if (ExposureTimevalue > 1.0)
                {
                    ExposureTimevalue = 1.0 / ExposureTimevalue;
                }
                double sum = Convert.ToDouble(ExposureTimeDataRaw.ChildNodes.Item(1).InnerText);
                row1 = datatableExposureTime.NewRow();
                row1["Value"] = ExposureTimevalue;
                row1["sum"] = sum;
                datatableExposureTime.Rows.Add(row1);
            }
            reader.Close();
        }



        private double ex3_calculate(double Fnumber, double ExposureTime, double ISOspeed)
        {
            double ex3 = 0.0;
            double FNumberValue = (double)datatableFnumber.Rows[0].ItemArray[0];
            double FnumberSum = (double)datatableFnumber.Rows[0].ItemArray[1];
            double ISOSpeedValue = (double)datatableISOspeed.Rows[0].ItemArray[0];
            double ISOspeedSum = (double)datatableISOspeed.Rows[0].ItemArray[1];
            double ExposureTimeValue = (double)datatableExposureTime.Rows[0].ItemArray[0];
            double ExposureTimeSum = (double)datatableExposureTime.Rows[0].ItemArray[1];
            foreach (DataRow drFnumber in datatableFnumber.Rows)
            {
                double FNumberValue1 = (double)drFnumber.ItemArray[0];
                if (Math.Abs(FNumberValue1 - Fnumber) < Math.Abs(FNumberValue - Fnumber))
                {
                    FNumberValue = FNumberValue1;
                    FnumberSum = (double)drFnumber.ItemArray[1];
                }
            }

            foreach (DataRow drISOspeed in datatableISOspeed.Rows)
            {
                double ISOspeedValue1 = (double)drISOspeed.ItemArray[0];
                if (Math.Abs(ISOspeedValue1 - ISOspeed) < Math.Abs(ISOSpeedValue - ISOspeed))
                {
                    ISOSpeedValue = ISOspeedValue1;
                    ISOspeedSum = (double)drISOspeed.ItemArray[1];
                }
            }

            foreach (DataRow drExposureTime in datatableExposureTime.Rows)
            {
                double ExposureTimeValue1 = (double)drExposureTime.ItemArray[0];
                if (Math.Abs(ExposureTimeValue1 - ExposureTime) < Math.Abs(ExposureTimeValue - ExposureTime))
                {
                    ExposureTimeValue = ExposureTimeValue1;
                    ExposureTimeSum = (double)drExposureTime.ItemArray[1];
                }
            }

            ex3 = FnumberSum + ISOspeedSum + ExposureTimeSum;
            if (Math.Abs(Math.Abs(ex3 - Math.Truncate(ex3)) - 0.9) < 0.01)
            {
                ex3 = Math.Round(ex3);
            }

            return ex3;
        }



        private void GetEXIFs()
        {
            string strtowrite;
            byte[] info2write;

            if (ImageFileName == null)
            {
                return;
            }
            double ex3, DataValueFnumber, DataValueExpTime, DataValueISO, DataValueExpCorr;
            //string DataValueExpTimeHR = "";
            //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            //string MetadataStatFilename = Directory.GetCurrentDirectory() + "\\meta_stat.dat";
            //MetadataStatFile = new FileStream(MetadataStatFilename, FileMode.Create, FileAccess.Write);
            string filename1 = ImageFileName;
            FileInfo imagefinfo = new FileInfo(filename1);
            string filenameEXIFdata = imagefinfo.DirectoryName + "\\exif.dat";
            EXIFdataFile = new FileStream(filenameEXIFdata, FileMode.Append, FileAccess.Write);
            FileInfo finfo2 = new FileInfo(filenameEXIFdata);
            if (finfo2.Length == 0)
            {
                strtowrite = "Filename\t\t\tISO\tFn\tT\texp3\n";
                info2write = new UTF8Encoding(true).GetBytes(strtowrite);
                EXIFdataFile.Write(info2write, 0, info2write.Length);
            }

            string stattxt = "";
            if (imagetoadd == null)
            {
                return;
            }

            Image image2Process = Image.FromFile(filename1);
            ImageInfo imInfo = new ImageInfo(image2Process);
            stattxt += filename1 + ";";

            //PropertyItem[] propItems = imagetoadd.PropertyItems;

            ex3 = 0.0;
            DataValueExpCorr = Convert.ToDouble(imInfo.getValueByKey("ExifExposureBias"));//ExifExposureBias
            DataValueExpTime = Convert.ToDouble(imInfo.getValueByKey("ExifExposureTime"));//ExifExposureTime
            DataValueFnumber = Convert.ToDouble(imInfo.getValueByKey("ExifFNumber"));
            DataValueISO = Convert.ToDouble(imInfo.getValueByKey("ExifISOSpeed"));//ExifISOSpeed
            ex3 += DataValueFnumber;

            if ((DataValueISO != 0.0d) && (DataValueExpCorr != 0.0d) && (DataValueExpTime != 0.0d) &&
                (DataValueFnumber != 0.0d))
            {
                ex3 = ex3_calculate(DataValueFnumber, DataValueExpTime, DataValueISO);

                Note("ISO speed = " + DataValueISO.ToString() + Environment.NewLine
                     + "Exp. time = " + DataValueExpTime.ToString() + Environment.NewLine
                     + "F number  = " + DataValueFnumber.ToString() + Environment.NewLine
                     + "Exp.corr. = " + DataValueExpCorr.ToString() + Environment.NewLine
                     + "Exp3 = " + ex3.ToString());

                stattxt += "\t" + ex3.ToString() + ";\tex.correction = " + DataValueExpCorr.ToString() +
                           Environment.NewLine;


                Note(DateTime.Now + "\tГотово! " + Environment.NewLine);
                //info1 = new UTF8Encoding(true).GetBytes(stattxt);
                //MetadataStatFile.Write(info1, 0, info1.Length);
                //MetadataStatFile.Close();
                if (!gotexifs)
                {
                    strtowrite = imagefinfo.Name + "\t\t"
                                 + DataValueISO.ToString() + "\t"
                                 + DataValueFnumber.ToString() + "\t"
                                 + DataValueExpTime.ToString() + "\t"
                                 + ex3.ToString() + "\n";
                    info2write = new UTF8Encoding(true).GetBytes(strtowrite);
                    EXIFdataFile.Write(info2write, 0, info2write.Length);
                    EXIFdataFile.Close();
                }
            }

            gotexifs = true;
            EXIFdataFile.Close();
        }






        private int countrows(System.Data.DataTable table_in, string column_id, double filtervalue)
        {
            DataRow[] filteredrows = table_in.Select("" + column_id + " = " + filtervalue.ToString());
            return filteredrows.Length;
        }


        private Color ColorScheme_m3(double si_m3, int callcase = 0)
        {
            Color new_color3;
            Color colBlack = Color.FromArgb(0, 0, 0);
            Color colWhite = Color.FromArgb(255, 250, 250);
            Color colBlue = Color.FromArgb(0, 0, 128);

            new_color3 = colBlack;
            if (callcase == 0)
            {
                if ((si_m3 < tunedSIMargin) && (si_m3 > -1))
                {
                    new_color3 = colWhite;
                }
                else
                {
                    new_color3 = colBlue;
                }

                if (si_m3 == -2.0)
                {
                    new_color3 = colBlack;
                }
            }
            else if (callcase == 1)
            {
                if (si_m3 == -2.0)
                {
                    new_color3 = colBlack;
                }
                else if (si_m3 == 0.0)
                {
                    new_color3 = colBlue;
                }
                else if (si_m3 == 1.0)
                {
                    new_color3 = colWhite;
                }
            }

            return new_color3;
        }


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


        private void button1_Click_1(object sender, EventArgs e)
        {
            добавитьФайлToolStripMenuItem_Click(sender, e);
        }



        private void DumpKeyData()
        {
            string strtowrite;
            byte[] info2write;

            string filename1 = ImageFileName;

            imagetoadd = new Image<Bgr, byte>(filename1); // Image.FromFile(filename1);
            FileInfo imagefinfo = new FileInfo(filename1);

            string filenameCCdata = imagefinfo.DirectoryName + "\\result.dat";
            string filenameStatData1File = imagefinfo.DirectoryName + "\\StatData1.dat";


            CCDataFile = new FileStream(filenameCCdata, FileMode.Append, FileAccess.Write);
            StatDataFile1 = new FileStream(filenameStatData1File, FileMode.Append, FileAccess.Write);

            FileInfo finfo1 = new FileInfo(filenameCCdata);
            if (finfo1.Length == 0)
            {
                strtowrite = "CCv - cloud cover index" + Environment.NewLine
                            + "MVl = sky-cloud margin value" + Environment.NewLine
                            + "Filename\t\tMVlSI\tCCvSI\n";
                //+ "Filename\t\tMVlSI\tCCvSI\tMVlIFM\tCCvIFM\n";
                info2write = new UTF8Encoding(true).GetBytes(strtowrite);
                CCDataFile.Write(info2write, 0, info2write.Length);

                strtowrite = "SkClMV = sky-cloud margin values" + Environment.NewLine
                            + "data fields contains sky-index values by files for each SkClMV" + Environment.NewLine
                            + "SkClMV\t\t\t";
                for (int i = SIClScMarginRangeLowerValue; i < (SIClScMarginRangeHigherValue + 1); i++)
                {
                    double marginval = Math.Round((double)i / 100.0, 2);
                    strtowrite += marginval.ToString() + "\t";
                }
                strtowrite += Environment.NewLine + "Filename" + Environment.NewLine;
                info2write = new UTF8Encoding(true).GetBytes(strtowrite);
                StatDataFile1.Write(info2write, 0, info2write.Length);
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
            info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            CCDataFile.Write(info2write, 0, info2write.Length);


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
            info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            StatDataFile1.Write(info2write, 0, info2write.Length);

            StatDataFile1.Close();
            CCDataFile.Close();
        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            if (imagetoadd == null)
            {
                ThreadSafeOperations.SetTextTB(textBox1, "Не загружено изображение для обработки!", true);
                return;
            }

            theLogWindow = ServiceTools.LogAText(theLogWindow, "Started processing " + ImageFileName);

            AnalyzeImage(sender);

            //CurrentPB2Source = pb2Source.SkyindexAnalyzer;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            GetEXIFs();
        }


        public void Note(string text)
        {
            ThreadSafeOperations.SetTextTB(textBox1, text + Environment.NewLine, true);
            //textBox1.Text += text + Environment.NewLine;
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tunedSIMargin = (double)trackBar1.Value / 100.0;
            ThreadSafeOperations.SetText(label2, tunedSIMargin.ToString(), false);
            //ProcessingConditionsChanged = true;
        }

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
                ThreadSafeOperations.SetTextTB(textBox1, "Ошибка при обработке Drag&Drop: " + exc1.Message + timetotal, true);
                //textBox1.Text += "Ошибка при обработке Drag&Drop: " + exc1.Message + timetotal;
            }
        }



        private void UpdateSIIFMlabels(int VarNumber, double CloudCoverValue)
        {
            if (VarNumber == 1)//SI variant
            {
                ThreadSafeOperations.SetText(label5, "Cl.cover: " + Math.Round(CloudCoverValue, 2).ToString(), false);
                //this.label5.Text = "Cl.cover: " + Math.Round(CloudCoverValue, 2).ToString();
            }
            else if (VarNumber == 2)//IFM variant
            {
                //ThreadSafeOperations.SetText(label6, "Cl.cover: " + Math.Round(CloudCoverValue, 2).ToString(), false);
                //this.label6.Text = "Cl.cover: " + Math.Round(CloudCoverValue, 2).ToString();
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if ((imagetoadd != null) && (!backgroundWorker1.IsBusy))
            {
                ThreadSafeOperations.UpdatePictureBox(pictureBox1, imagetoadd.Bitmap, true);
                ThreadSafeOperations.UpdatePictureBox(pictureBox2, pictureBox2Bitmap, true);
                this.Refresh();
            }
        }

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

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;

            AnalyzeImage();
            if (SelfWorker.CancellationPending)
            {
                e.Cancel = true;
            }

            DumpKeyData();
            //ProcessButtonPushedOnceThisFile = true;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ThreadSafeOperations.SetTextTB(textBox1, "Ошибка! " + e.Error.Message + Environment.NewLine, true);
            }
            ThreadSafeOperations.SetTextTB(textBox1, "операция завершена" + Environment.NewLine + "------------------" + Environment.NewLine, true);
            //SetButtonEnabledStatus(обработатьToolStripMenuItem, true);
            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);


            classificator.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            ThreadSafeOperations.ToggleButtonState(обработатьToolStripMenuItem, true, "Обработать", false);
            ThreadSafeOperations.ToggleButtonState(button4, true, "OK", false);
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, e.ProgressPercentage);

            string statusMessage = (string)e.UserState;
            if (statusMessage != "")
            {
                Note(statusMessage);
            }

        }

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

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //if (GetSensorsDataCycle.IsBusy)
            //{
            //    ThreadSafeOperations.SetText(StatusLabel, "Идет сбор данных датчиков.", false);
            //}
            //else
            //{
            //    ThreadSafeOperations.SetText(StatusLabel, "Сбор данных датчиков не производится.", false);
            //}
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            double TrackBarValue;
            double divider;
            System.Windows.Forms.TrackBar MyTB;

            //ProcessingConditionsChanged = true;

            MyTB = (System.Windows.Forms.TrackBar)sender;
            TrackBarValue = (double)MyTB.Value;
            divider = Math.Pow(2.0, (double)(10 - TrackBarValue));
            resizevalue = 1.0 / divider;
            //ThreadSafeOperations.SetText(label7, resizevalue.ToString());
            ThreadSafeOperations.SetText(label9, "Изменять исходный размер: " + resizevalue.ToString(), false);
            //label7.Text = resizevalue.ToString();
            if (ImageFileName != null)
            {
                OpenFile(ImageFileName);
            }
        }


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



        




        private void button1_Click(object sender, EventArgs e)
        {
            string path2process = (string)defaultProperties["BatchProcessingDirectory"];
            if (path2process == "")
            {
                path2process = Directory.GetCurrentDirectory();
            }

            DirectoryInfo dir = new DirectoryInfo(path2process);

            if (backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
            }
            else
            {
                if (!dir.Exists)
                {
                    ThreadSafeOperations.SetTextTB(textBox1, textBox1.Text + "Операция не выполнена. Не найдена директория:" + Environment.NewLine + path2process + Environment.NewLine, true);
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
                var propertiesObj = Properties.Settings.Default.Properties;
                foreach (SettingsProperty settingsProperty in propertiesObj)
                {
                    strToDisplay += Environment.NewLine + settingsProperty.Name + " = " + settingsProperty.DefaultValue;
                }
                theLogWindow = ServiceTools.LogAText(theLogWindow, strToDisplay);

                simpleMultipleImagesShow imagesRepresentingForm = new simpleMultipleImagesShow();
                imagesRepresentingForm.Show();

                List<ClassificationMethods> schemesToUse = new List<ClassificationMethods>();
                schemesToUse.Add(ClassificationMethods.Japan);
                schemesToUse.Add(ClassificationMethods.German);
                schemesToUse.Add(ClassificationMethods.TestNew);

                object[] BGWorker2Args = new object[] { path2process, theLogWindow, imagesRepresentingForm, schemesToUse };

                //ThreadSafeOperations.SetButtonEnabledStatus(открытьФайлToolStripMenuItem, false);
                //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar3, false);
                //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar4, false);
                //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar5, false);



                //SIClScMarginRangeLowerValue = trackBar4.Value;
                //SIClScMarginRangeHigherValue = trackBar5.Value;

                ThreadSafeOperations.ToggleButtonState(button1, true, "Прекратить обработку", true);

                backgroundWorker2.RunWorkerAsync(BGWorker2Args);
            }
        }



        /// <summary>
        /// Backgrounds the worker2_ do work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object[] arguments = e.Argument as object[];
            //double ProgressPerc;
            int SIValueCounter = 5;
            int SIValueRange = 25, SIValueRange_min = 5, SIValueRange_max = 30;
            double defaultResizeFactor = Convert.ToDouble(defaultProperties["DefaultScalingFactor"]); ;
            int defaultMaxImageSize = Convert.ToInt32(defaultProperties["DefaultMaxImageSize"]); ;
            Bitmap bmJ = null, bmG = null, bmGrIx = null; //, bitmap2process = null;

            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            string outputStatsDirectory = (string)defaultProperties["DefaultDataFilesLocation"];

            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);

            //SimpleShowImageForm GrIxResultForm = null;
            //SimpleShowImageForm sourceImageForm = null;

            SIValueRange_min = SIClScMarginRangeLowerValue;
            SIValueRange_max = SIClScMarginRangeHigherValue;
            SIValueRange = SIValueRange_max - SIValueRange_min;
            FileCounter = 0; filecounter_prev = 0;

            //TrackBar3Value = ((int)(e.Argument as object[])[0]);
            string path2process = (string)arguments[0];
            LogWindow theLogWindow = (LogWindow)arguments[1];
            simpleMultipleImagesShow imagesRepresentingForm = (simpleMultipleImagesShow)arguments[2];
            List<ClassificationMethods> schemesToUse = (List<ClassificationMethods>)arguments[3];
            Type theType = imagesRepresentingForm.GetType();
            MethodInfo thePicturePlacingMethodInfo = theType.GetMethod("PlaceAPicture");


            ServiceTools.logToTextFile(outputStatsDirectory + "statsWithFNames.dat", "Full filename | date+time picture taken | SI amer | SI jap | SI GrIx | SB suppr | Sun disk condition " + Environment.NewLine, true);


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
                strToDisplay += dateTime;


                //imagetoadd = Image.FromFile(fInfo.FullName);
                //int ImageWidthRec = (int)Math.Round((imagetoadd.Width * defaultResizeFactor), 0);
                //int ImageHeightRec = (int)Math.Round((imagetoadd.Height * defaultResizeFactor), 0);
                //imagetoadd = ImageProcessing.ImageResizer(imagetoadd, defaultMaxImageSize);

                
                //Bitmap bm1 = new Bitmap(imagetoadd);



                Image<Bgr, Byte> img2process = new Image<Bgr,byte>(fInfo.FullName);

                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.Japan) != -1)
                {
                    //bitmap2process = ServiceTools.ReadBitmapFromFile(fInfo.FullName);
                    ThreadSafeOperations.UpdatePictureBox(pictureBox1, img2process.Bitmap, true);
                    img2process = ImageProcessing.ImageResizer(img2process, defaultMaxImageSize);

                    classificator = new SkyCloudClassification(img2process, defaultProperties);
                    classificator.LogWindow = theLogWindow;
                    classificator.ParentForm = this;
                    classificator.ClassificationMethod = ClassificationMethods.Japan;
                    classificator.isCalculatingUsingBgWorker = true;
                    classificator.SelfWorker = sender as BackgroundWorker;
                    classificator.defaultOutputDataDirectory = (string) defaultProperties["DefaultDataFilesLocation"];
                    classificator.cloudSkySeparationValue =
                        Convert.ToDouble(defaultProperties["JapanCloudSkySeparationValue"]);
                    classificator.Classify();
                    classificator.resultingStatusMessages = "";
                    bitmapSI = new Bitmap(classificator.PreviewBitmap);
                    ServiceTools.FlushMemory();
                    ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                    pictureBox2Bitmap = new Bitmap(bitmapSI);
                    strToDisplay += " | " + Math.Round(classificator.CloudCover, 2).ToString();
                    UpdateSIIFMlabels(1, classificator.CloudCover);
                    bmJ = new Bitmap(bitmapSI);
                }

                


                //imagetoadd = Image.FromFile(fInfo.FullName);
                //ImageWidthRec = (int)Math.Round((imagetoadd.Width * defaultResizeFactor), 0);
                //ImageHeightRec = (int)Math.Round((imagetoadd.Height * defaultResizeFactor), 0);
                //imagetoadd = ImageProcessing.ImageResizer(imagetoadd, defaultMaxImageSize);
                //bitmap2process = (Bitmap)(imagetoadd.Clone());
                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.German) != -1)
                {
                    //bitmap2process = ServiceTools.ReadBitmapFromFile(fInfo.FullName);
                    //ThreadSafeOperations.UpdatePictureBox(pictureBox1, bitmap2process, true);
                    //bitmap2process = ImageProcessing.BitmapResizer(bitmap2process, defaultMaxImageSize);

                    classificator = new SkyCloudClassification(img2process, defaultProperties);
                    classificator.LogWindow = theLogWindow;
                    classificator.ParentForm = this;
                    classificator.ClassificationMethod = ClassificationMethods.German;
                    classificator.isCalculatingUsingBgWorker = true;
                    classificator.SelfWorker = sender as BackgroundWorker;
                    classificator.defaultOutputDataDirectory = (string) defaultProperties["DefaultDataFilesLocation"];
                    classificator.cloudSkySeparationValue =
                        Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]);
                    classificator.Classify();
                    classificator.resultingStatusMessages = "";
                    bitmapSI = new Bitmap(classificator.PreviewBitmap);
                    ServiceTools.FlushMemory();
                    ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                    pictureBox2Bitmap = new Bitmap(bitmapSI);
                    strToDisplay += " | " + Math.Round(classificator.CloudCover, 2).ToString();
                    UpdateSIIFMlabels(1, classificator.CloudCover);

                    bmG = new Bitmap(bitmapSI);
                }

                //imagetoadd = Image.FromFile(fInfo.FullName);
                //ImageWidthRec = (int)Math.Round((imagetoadd.Width * defaultResizeFactor), 0);
                //ImageHeightRec = (int)Math.Round((imagetoadd.Height * defaultResizeFactor), 0);
                //imagetoadd = ImageProcessing.ImageResizer(imagetoadd, defaultMaxImageSize);
                //bitmap2process = (Bitmap)(imagetoadd.Clone());
                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.TestNew) != -1)
                {
                    //bitmap2process = ServiceTools.ReadBitmapFromFile(fInfo.FullName);
                    //ThreadSafeOperations.UpdatePictureBox(pictureBox1, bitmap2process, true);
                    //bitmap2process = ImageProcessing.BitmapResizer(bitmap2process, defaultMaxImageSize);

                    classificator = new SkyCloudClassification(img2process, defaultProperties);
                    classificator.LogWindow = theLogWindow;
                    classificator.ParentForm = this;
                    classificator.ClassificationMethod = ClassificationMethods.TestNew;
                    classificator.isCalculatingUsingBgWorker = true;
                    classificator.SelfWorker = sender as BackgroundWorker;
                    classificator.defaultOutputDataDirectory = (string) defaultProperties["DefaultDataFilesLocation"];
                    classificator.cloudSkySeparationValue =
                        Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]);
                    classificator.theStdDevMarginValueDefiningSkyCloudSeparation =
                        Convert.ToDouble(defaultProperties["GrIxDefaultSkyCloudMarginWithoutSun"]);
                    classificator.Classify();
                    classificator.resultingStatusMessages = "";
                    bitmapSI = new Bitmap(classificator.PreviewBitmap);
                    ServiceTools.FlushMemory();
                    ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
                    pictureBox2Bitmap = new Bitmap(bitmapSI);
                    strToDisplay += " | " + Math.Round(classificator.CloudCover, 2).ToString();
                    strToDisplay += " | " + classificator.currentSunDiskCondition.ToString();
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
                    string germanFName = outputDataDirectory + randomFileName + "_si_ger.jpg";
                    string GrIxFName = outputDataDirectory + randomFileName + "_si_GrIx.jpg";

                    //imagetoadd = Image.FromFile(fInfo.FullName);
                    //ImageWidthRec = (int)Math.Round((imagetoadd.Width * defaultResizeFactor), 0);
                    //ImageHeightRec = (int)Math.Round((imagetoadd.Height * defaultResizeFactor), 0);
                    //imagetoadd = ImageProcessing.ImageResizer(imagetoadd, defaultMaxImageSize);
                    //bitmap2process = (Bitmap)(imagetoadd.Clone());

                    //bitmap2process = ServiceTools.ReadBitmapFromFile(fInfo.FullName);
                    //ThreadSafeOperations.UpdatePictureBox(pictureBox1, bitmap2process, true);
                    //bitmap2process = ImageProcessing.BitmapResizer(bitmap2process, defaultMaxImageSize);

                    img2process.Save(sourceFName);
                    bmJ.Save(japanFName, ImageFormat.Jpeg);
                    bmG.Save(germanFName, ImageFormat.Jpeg);
                    bmGrIx.Save(GrIxFName, ImageFormat.Jpeg);

                }


                object[] parametersArray = new object[] { img2process.Bitmap, fInfo.Name, 1 };
                thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);

                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.Japan) != -1)
                {
                    parametersArray = new object[] {bmJ, "Japan", 2};
                    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                }

                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.German) != -1)
                {
                    parametersArray = new object[] {bmG, "German", 3};
                    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                }

                if (schemesToUse.FindIndex(clMethod => clMethod == ClassificationMethods.TestNew) != -1)
                {
                    parametersArray = new object[] {bmGrIx, "GrIx", 4};
                    thePicturePlacingMethodInfo.Invoke(imagesRepresentingForm, parametersArray);
                }

                //ThreadSafeOperations.UpdatePictureBox(pictureBox4, classificator.PreviewBitmap, true);

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

            #region // УСТАРЕЛО
            //foreach (FileInfo FileListMember in fileList2Process)
            //{
            //    FileCounter++;
            //    for (SIValueCounter = SIValueRange_min; SIValueCounter < (SIValueRange_max + 1); SIValueCounter++)
            //    {
            //        OpenFile(FileListMember.FullName);

            //        ThreadSafeOperations.MoveTrackBar(trackBar1, SIValueCounter);
            //        tunedSIMargin = SIValueCounter / 100.0;
            //        ThreadSafeOperations.SetText(label2, tunedSIMargin.ToString(), false);

            //        button2_Click_1(null, null);
            //        while (backgroundWorker1.IsBusy)
            //        {
            //            System.Threading.Thread.Sleep(20);
            //        }


            //        ProgressPerc = ((double)((FileCounter - 1) * SIValueRange + SIValueCounter - SIValueRange_min) / (double)(fileList2Process.Length * SIValueRange)) * 100;
            //        SelfWorker.ReportProgress((int)ProgressPerc);

            //        if (SelfWorker.CancellationPending)
            //        {
            //            e.Cancel = true;
            //            break;
            //        }
            //    }
            //    if (SelfWorker.CancellationPending)
            //    {
            //        e.Cancel = true;
            //        break;
            //    }
            //}
            #endregion // УСТАРЕЛО
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




        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, e.ProgressPercentage);
        }




        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.SetButtonEnabledStatus(открытьФайлToolStripMenuItem, true);
            ThreadSafeOperations.SetButtonEnabledStatus(обработатьToolStripMenuItem, true);
            ThreadSafeOperations.SetButtonEnabledStatus(eXIFыToolStripMenuItem, true);

            ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar3, true);
            //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar4, true);
            //ThreadSafeOperations.SetTrackBarEnabledStatus(trackBar5, true);

            string CurDir = Directory.GetCurrentDirectory();
            string path2process = CurDir;

            ThreadSafeOperations.ToggleButtonState(button1, true, "Обработка директории: " + path2process, false);

            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);
            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);
        }




        //private void trackBar4_Scroll(object sender, EventArgs e)
        //{
        //    //double TrackBarLValue, TrackBarRValue;
        //    //TrackBarLValue = (double)trackBar4.Value;
        //    //TrackBarRValue = (double)trackBar5.Value;
        //    //ThreadSafeOperations.SetText(label10, (TrackBarLValue / 100.0).ToString() + " - " + (TrackBarRValue / 100.0).ToString(), false);
        //    //ProcessingConditionsChanged = true;
        //}





        
        private void настройкиСбораДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SkyIndexAnalyzerDataCollector CollectorForm = new SkyIndexAnalyzerDataCollector();
            //CollectorForm.Show();
            //ShowImageForm ImgShow = new ShowImageForm(localPreviewBitmap, ParentForm, this);
            //ImgShow.Show();
        }

        private void DetectEdgesButton_Click(object sender, EventArgs e)
        {
            if (imagetoadd == null)
            {
                ThreadSafeOperations.SetTextTB(textBox1, "Не загружено изображение для обработки!", true);
                return;
            }

            DetectImageEdgesCircled();
        }



        private void DetectImageEdgesCircled()
        {
            ImageProcessing imp = new ImageProcessing(imagetoadd, true);
            imp.reportingTextBox = textBox1;
            //imp.getImageOctLined();
            ServiceTools.FlushMemory(textBox1, "");
            pictureBox2Bitmap = new Bitmap(imp.significantMaskImageOctLined.Bitmap);
            Image img2show = (Image)imp.significantMaskImageOctLined.Bitmap;
            //ServiceTools.ShowPicture(img2show);
            ThreadSafeOperations.UpdatePictureBox(pictureBox2, img2show, true);
        }




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

            #region new-style analyzing
            DateTime beginNew = DateTime.Now;
            //classificator = new SkyCloudClassification(imagetoadd, pictureBox2, pbUniversalProgressBar, textBox1);
            classificator = new SkyCloudClassification(imagetoadd, defaultProperties);
            classificator.cloudSkySeparationValue = tunedSIMargin;
            if (rbtnClassMethodJapan.Checked)
            {
                classificator.ClassificationMethod = ClassificationMethods.Japan;
            }
            else if (rbtnClassMethodGreek.Checked)
            {
                classificator.ClassificationMethod = ClassificationMethods.Greek;
            }
            else if (rbtnClassMethodNew.Checked)
            {
                classificator.ParentForm = this;
                classificator.ClassificationMethod = ClassificationMethods.TestNew;

                classificator.theStdDevMarginValueDefiningSkyCloudSeparation = tunedSIMargin;
            }

            if (backgroundWorker1.IsBusy)
            {
                classificator.isCalculatingUsingBgWorker = true;
                classificator.SelfWorker = backgroundWorker1;
            }
            else
            {
                classificator.isCalculatingUsingBgWorker = false;
            }

            classificator.sourceImageFileName = ImageFileName;

            classificator.LogWindow = theLogWindow;
            classificator.defaultOutputDataDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
            classificator.Classify();
            Note(classificator.resultingStatusMessages);
            classificator.resultingStatusMessages = "";
            bitmapSI = new Bitmap(classificator.PreviewBitmap);
            TimeSpan timeNew = DateTime.Now - beginNew;
            #endregion

            ServiceTools.FlushMemory(textBox1, "#02");

            ThreadSafeOperations.UpdatePictureBox(pictureBox2, bitmapSI, true);
            pictureBox2Bitmap = new Bitmap(bitmapSI);

            ThreadSafeOperations.UpdateProgressBar(pbUniversalProgressBar, 0);

            string strtowrite3 = "Cloud cover 1 = " + Math.Round(classificator.CloudCover, 2).ToString() + Environment.NewLine;

            GetEXIFs();

            timetotal = DateTime.Now - begintotal;

            Note("Общее время выполнения: " + timetotal);
            Note("Время обсчета по новому алгоритму: " + timeNew);

            //tunedSIMargin_prev = tunedSIMargin;

            UpdateSIIFMlabels(1, classificator.CloudCover);
            ServiceTools.FlushMemory(textBox1, "");
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
            SkyIndexAnalyzing_ColorsManipulatingForm ManualAnalysisForm = new SkyIndexAnalyzing_ColorsManipulatingForm(imagetoadd, this, defaultProperties);
            ManualAnalysisForm.tbLog = textBox1;
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
            if (rbtnClassMethodNew.Checked)
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
                tunedSIMargin = tunedSIMarginDefault;
                ThreadSafeOperations.MoveTrackBar(trackBar1, (int)(tunedSIMargin * 100.0));
                trackBar1_Scroll(null, null);
            }
        }









        #region // УСТАРЕЛО - отдельно стоящий метод определения местоположения солнца
        /// <summary>
        /// Handles the Click event of the btnSunDetection control.
        /// used to test the sun detection algoritms
        /// deprecated
        /// dont use!!!
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        //private void btnSunDetection_Click(object sender, EventArgs e)
        //{
        //    double minSunAreaPart = 0.003d;
        //    double maxSunAreaPart = 0.05d;
        //    double theStdDevMarginValueDefiningTrueSkyArea = 0.65d;
        //    double theStdDevMarginValueDefiningSkyCloudSeparation = 0.75d;
        //    double theStdDevMarginValueDefiningSkyCloudSeparation_SunSuppressed = 0.1d;

        //    DenseMatrix dmRedChannel;
        //    DenseMatrix dmBlueChannel;
        //    DenseMatrix dmGreenChannel;

        //    Bitmap LocalProcessingBitmap = (Bitmap)(imagetoadd.Clone());

        //    Image<Gray, Byte> imageBlueChannelByte = new Image<Bgr, Byte>(LocalProcessingBitmap)[0];
        //    Image<Gray, Byte> imageGreenChannelByte = new Image<Bgr, Byte>(LocalProcessingBitmap)[1];
        //    Image<Gray, Byte> imageRedChannelByte = new Image<Bgr, Byte>(LocalProcessingBitmap)[2];
        //    ImageProcessing imgP = new ImageProcessing(LocalProcessingBitmap, true);
        //    //Image<Gray, Byte> maskImage = imgP.significantMaskImageBinary;
        //    Image<Gray, Byte> maskImage100 = imgP.imageSignificantMaskCircled(100);
        //    Image<Gray, Byte> maskImage = imgP.imageSignificantMaskCircled(95);
        //    double overallMaskArea = (double)maskImage.CountNonzero()[0];

        //    imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage100);
        //    imageRedChannelByte = imageRedChannelByte.Mul(maskImage100);
        //    imageGreenChannelByte = imageGreenChannelByte.Mul(maskImage100);
        //    dmRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
        //    dmBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
        //    dmGreenChannel = ImageProcessing.DenseMatrixFromImage(imageGreenChannelByte);
        //    ServiceTools.FlushMemory(null, null);


        //    string formulaString = "1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y";
        //    DenseMatrix dmProcessingData = (DenseMatrix)imgP.eval(formulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
        //    ServiceTools.RepresentDataFromDenseMatrix(dmProcessingData, "original GrIx data");
        //    DenseMatrix dmMask100 = ImageProcessing.DenseMatrixFromImage(maskImage100);
        //    DenseMatrix dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);
        //    dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMask100);


        //    //formulaString = "Y / 255";
        //    //DenseMatrix dmTotalLuminance = (DenseMatrix)imgP.eval(formulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
        //    //double totalLuminance = dmTotalLuminance.Values.Sum() / dmMask.Values.Sum();

        //    ServiceTools.FlushMemory();


        //    #region поиск солнца по засветке
        //    // ищем солнце по центру масс засветки
        //    // радиус ищем в предположении, что это круг и его площадь = PI * R^2
        //    //DenseMatrix dmSunMask = DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
        //    //    new Func<int, int, double>(
        //    //        (row, column) =>
        //    //        {
        //    //            if (dmProcessingData[row, column] == 1.0d) return 1.0d;
        //    //            else return 0.0d;
        //    //        }));
        //    //ServiceTools.RepresentDataFromDenseMatrix(dmSunMask, "Sun burning mask");
        //    ////ImageConditionAndDataRepresentingForm sunMaskDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmSunMask, "Sun mask");
        //    //PointD sunCenterPoint = new PointD(0.0d, 0.0d);
        //    //double weightsSum = dmSunMask.Values.Sum();
        //    //DenseMatrix dmSunCenterResearchingWeights = (DenseMatrix)dmSunMask.Clone();
        //    //dmSunCenterResearchingWeights.MapInplace(new Func<double, double>(val => val / weightsSum));
        //    //DenseMatrix dmSunCenterResearching = DenseMatrix.Create(dmSunCenterResearchingWeights.RowCount,
        //    //    dmSunCenterResearchingWeights.ColumnCount, new Func<int, int, double>((row, column) => (double)column));
        //    //dmSunCenterResearching = (DenseMatrix)dmSunCenterResearching.PointwiseMultiply(dmSunCenterResearchingWeights);
        //    //sunCenterPoint.X = dmSunCenterResearching.Values.Sum();
        //    //dmSunCenterResearching = DenseMatrix.Create(dmSunCenterResearchingWeights.RowCount,
        //    //    dmSunCenterResearchingWeights.ColumnCount, new Func<int, int, double>((row, column) => (double)row));
        //    //dmSunCenterResearching = (DenseMatrix)dmSunCenterResearching.PointwiseMultiply(dmSunCenterResearchingWeights);
        //    //sunCenterPoint.Y = dmSunCenterResearching.Values.Sum();
        //    //double sunRadius = Math.Sqrt(weightsSum / Math.PI);

        //    //Image<Bgr, Byte> sunRepresentingimage = ImageProcessing.evalResultColored(dmProcessingData, maskImage, new ColorScheme(""));
        //    //CircleF theSunCircle = new CircleF(new PointF((float)sunCenterPoint.X, (float)sunCenterPoint.Y), (float)sunRadius);
        //    //Image<Bgr, Byte> theSunCircleImage = sunRepresentingimage.CopyBlank();
        //    //theSunCircleImage.Draw(theSunCircle, new Bgr(100, 100, 100), 0);
        //    //sunRepresentingimage = sunRepresentingimage.AddWeighted(theSunCircleImage, 0.5, 0.5, 0.0);
        //    //ServiceTools.ShowPicture(sunRepresentingimage.Bitmap);
        //    #endregion поиск солнца по засветке


        //    #region посчитаем для объектов засветки разные солнечные метрики
        //    // lets play with grad field near the sunburn edges

        //    ArithmeticsOnImages aoi = new ArithmeticsOnImages();
        //    aoi.dmY = dmProcessingData;
        //    aoi.exprString = "grad(Y)";
        //    aoi.RPNeval(true);
        //    DenseMatrix dmGradField = (DenseMatrix)aoi.dmRes.Clone();
        //    dmGradField = (DenseMatrix)dmGradField.PointwiseMultiply(dmMask);

        //    aoi.exprString = "ddx(Y)";
        //    aoi.RPNeval(true);
        //    DenseMatrix dmDDXField = (DenseMatrix)aoi.dmRes.Clone();
        //    dmDDXField = (DenseMatrix)dmDDXField.PointwiseMultiply(dmMask);

        //    aoi.exprString = "ddy(Y)";
        //    aoi.RPNeval(true);
        //    DenseMatrix dmDDYField = (DenseMatrix)aoi.dmRes.Clone();
        //    dmDDYField = (DenseMatrix)dmDDYField.PointwiseMultiply(dmMask);

        //    //aoi.Dispose();
        //    //aoi = null;
        //    ServiceTools.RepresentDataFromDenseMatrix(dmGradField, "Grad field");

        //    // посмотрим, что с градиентами на границах разных отдельных объектов

        //    Image<Gray, Byte> imgSunMask = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(
        //        dmProcessingData, 0.0d, 1.0d, false);
        //    imgSunMask = imgSunMask.ThresholdBinary(new Gray(254), new Gray(255));
        //    ServiceTools.ShowPicture(imgSunMask.Bitmap);

        //    Contour<Point> contoursDetected = imgSunMask.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
        //    List<Contour<Point>> contoursList = new List<Contour<Point>>();

        //    while (true)
        //    {
        //        Contour<Point> currContour = contoursDetected;
        //        double currcontourArePart = currContour.Area / overallMaskArea;
        //        if ((currcontourArePart <= maxSunAreaPart) && (currcontourArePart >= minSunAreaPart))
        //        {
        //            contoursList.Add(currContour);
        //        }

        //        contoursDetected = contoursDetected.HNext;
        //        if (contoursDetected == null)
        //            break;
        //    }



        //    string baseDir = "G:\\_gulevlab\\SkyIndexAnalyzerSolo_appData\\_dataDirectory\\";
        //    ServiceTools.logToTextFile(baseDir + "ShowHistScript.m", "clear; " + Environment.NewLine, true);

        //    List<double> contourMaxSunMarginCluster = new List<double>();
        //    List<double> contourMaxSunMarginClusterRel = new List<double>();
        //    List<double> contourMeanGradDataStdDev = new List<double>();
        //    List<double> contourMeanGradDataMean = new List<double>();
        //    List<string> contoursName = new List<string>();
        //    List<Contour<Point>> countedContours = new List<Contour<Point>>();

        //    Contour<Point> foundSunContour = null;
        //    double minGradStDevValue = 1.0d;
        //    foreach (Contour<Point> currContour in contoursList)
        //    {
        //        if (currContour.Area < 9)
        //        {
        //            continue;
        //        }

        //        DenseMatrix dmDistanceToCurrContour = DenseMatrix.Create(dmProcessingData.RowCount,
        //            dmProcessingData.ColumnCount, (row, col) =>
        //            {
        //                PointF thePoint = new PointF((float)col, (float)row);
        //                return currContour.Distance(thePoint);
        //            });

        //        DenseMatrix dmPointsOutsideCurrContour = DenseMatrix.Create(dmProcessingData.RowCount,
        //            dmProcessingData.ColumnCount, (row, col) =>
        //            {
        //                double currDistance = dmDistanceToCurrContour[row, col];
        //                if (currDistance > 0.0d) return 0.0;
        //                else return 1.0d;
        //            });


        //        // celculate the densematrix containing grad(field) close to contour margin, outside the contour
        //        double currArea = currContour.Area;
        //        currArea = currArea / (double)maskImage.CountNonzero()[0];
        //        DenseMatrix dmCurrContourGradData = (DenseMatrix)dmGradField.Clone();
        //        dmCurrContourGradData.MapIndexedInplace((row, column, val) =>
        //        {
        //            double currDistance = dmDistanceToCurrContour[row, column];
        //            if (Math.Abs(currDistance) > 5.0d) return 0.0;
        //            //if (Math.Abs(currDistance) < 10.0d) return 0.0;
        //            if (currDistance > 0.0d) return 0.0;
        //            else return val;
        //        });


        //        DenseMatrix dmCurrContourDDXData = (DenseMatrix)dmDDXField.Clone();
        //        dmCurrContourDDXData.MapIndexedInplace((row, column, val) =>
        //        {
        //            double currDistance = dmDistanceToCurrContour[row, column];
        //            if (Math.Abs(currDistance) > 5.0d) return 0.0;
        //            //if (Math.Abs(currDistance) < 10.0d) return 0.0;
        //            if (currDistance > 0.0d) return 0.0;
        //            else return val;
        //        });



        //        DenseMatrix dmCurrContourDDYData = (DenseMatrix)dmDDYField.Clone();
        //        dmCurrContourDDYData.MapIndexedInplace((row, column, val) =>
        //        {
        //            double currDistance = dmDistanceToCurrContour[row, column];
        //            if (Math.Abs(currDistance) > 5.0d) return 0.0;
        //            //if (Math.Abs(currDistance) < 10.0d) return 0.0;
        //            if (currDistance > 0.0d) return 0.0;
        //            else return val;
        //        });



        //        #region попробую посчитать статистику по градиенту (mean и stdDev) в пределах 5px, ddx, ddy в пределах 5px для каждой точки границы объекта засветки

        //        string csvFileText = "";
        //        List<double> meanDataAlongContour = new List<double>();
        //        List<double> stdDevDataAlongContour = new List<double>();


        //        //DenseMatrix dmDDXMeanDataAlongContour = DenseMatrix.Create(dmProcessingData.RowCount,
        //        //        dmProcessingData.ColumnCount, new Func<int, int, double>((row, col) => 0.0d));
        //        //DenseMatrix dmDDYMeanDataAlongContour = DenseMatrix.Create(dmProcessingData.RowCount,
        //        //    dmProcessingData.ColumnCount, new Func<int, int, double>((row, col) => 0.0d));

        //        foreach (Point contourPoint in currContour)
        //        {

        //            DenseMatrix dmCurrpointDistance = DenseMatrix.Create(dmProcessingData.RowCount,
        //                dmProcessingData.ColumnCount,
        //                (row, col) =>
        //                    PointD.Distance(new PointD((double)col, (double)row), new PointD(contourPoint)));
        //            DenseMatrix dmCurrpointDistanceLessThan10 = (DenseMatrix)dmCurrpointDistance.Clone();
        //            dmCurrpointDistanceLessThan10.MapInplace(x => (x <= 10.0d) ? (x) : (0.0d));
        //            DenseMatrix dmCurrpointDistanceLessThan5 = (DenseMatrix)dmCurrpointDistance.Clone();
        //            dmCurrpointDistanceLessThan5.MapInplace(x => (x <= 5.0d) ? (x) : (0.0d));


        //            Func<int, int, double, double> fLessThan5DistanceToCurrentPoint =
        //                (row, col, x) => x * dmCurrpointDistanceLessThan5[row, col];
        //            //Func<int, int, double, double> fLessThan5DistanceToCurrentPoint =
        //            //    new Func<int, int, double, double>((row, col, x) => x*dmCurrpointDistanceLessThan5[row, col]);
        //            Func<int, int, double, double> fPointsOutsideTheContour =
        //                (row, col, x) => x * dmPointsOutsideCurrContour[row, col];


        //            //DenseMatrix dmDDXstatsCurrPoint = (DenseMatrix)dmDDXField.Clone();
        //            //dmDDXstatsCurrPoint.MapIndexedInplace(fLessThan5DistanceToCurrentPoint);
        //            //dmDDXstatsCurrPoint.MapIndexedInplace(fPointsOutsideTheContour);


        //            //DenseMatrix dmDDYstatsCurrPoint = (DenseMatrix)dmDDYField.Clone();
        //            //dmDDYstatsCurrPoint.MapIndexedInplace(fLessThan5DistanceToCurrentPoint);
        //            //dmDDYstatsCurrPoint.MapIndexedInplace(fPointsOutsideTheContour);


        //            DenseMatrix dmCurrContourGradStatsData = (DenseMatrix)dmGradField.Clone();
        //            dmCurrContourGradStatsData.MapIndexedInplace(fLessThan5DistanceToCurrentPoint);
        //            dmCurrContourGradStatsData.MapIndexedInplace(fPointsOutsideTheContour);


        //            //DescriptiveStatistics statsCurrPointDDXData = DataAnalysis.StatsOfDataExcludingValues(dmDDXstatsCurrPoint,
        //            //    0.0d);
        //            //if (statsCurrPointDDXData != null)
        //            //{
        //            //    dmDDXMeanDataAlongContour[contourPoint.Y, contourPoint.X] = statsCurrPointDDXData.Mean;
        //            //}

        //            //DescriptiveStatistics statsCurrPointDDYData = DataAnalysis.StatsOfDataExcludingValues(dmDDYstatsCurrPoint,
        //            //    0.0d);
        //            //if (statsCurrPointDDYData != null)
        //            //{
        //            //    dmDDYMeanDataAlongContour[contourPoint.Y, contourPoint.X] = statsCurrPointDDYData.Mean;
        //            //}


        //            DescriptiveStatistics statsCurrPointGradData = DataAnalysis.StatsOfDataExcludingValues(dmCurrContourGradStatsData,
        //                0.0d);
        //            if (statsCurrPointGradData != null)
        //            {
        //                meanDataAlongContour.Add(statsCurrPointGradData.Mean);
        //                stdDevDataAlongContour.Add(statsCurrPointGradData.StandardDeviation);
        //                csvFileText += statsCurrPointGradData.Mean.ToString("e").Replace(",", ".") + ";" +
        //                               (statsCurrPointGradData.StandardDeviation).ToString("e").Replace(",", ".") +
        //                               Environment.NewLine;
        //            }
        //        }

        //        DescriptiveStatistics statsGradMeansAlongContour = new DescriptiveStatistics(meanDataAlongContour);
        //        double meanGradValueAlongCurrentContour = statsGradMeansAlongContour.Mean;
        //        double stdDevGradValueAlongCurrentContour = statsGradMeansAlongContour.StandardDeviation;
        //        List<int> continousDataClustersLessThanMean =
        //            DataAnalysis.ContinousDataClustersWithCondition(meanDataAlongContour,
        //                (x => x <= meanGradValueAlongCurrentContour));
        //        int maxContinousMeanGradValueCluster = continousDataClustersLessThanMean.Max();


        //        #endregion попробую посчитать статистику по градиенту (mean и stdDev) в пределах 5px, ddx, ddy в пределах 5px для каждой точки границы объекта засветки







        //        //aoi.exprString = "Y%2";
        //        //aoi.dmY = dmCurrContourGradData;
        //        //aoi.RPNeval(true);
        //        //dmCurrContourGradData = (DenseMatrix)aoi.dmRes.Clone();


        //        //Bgr newBgrColor = new Bgr(colorGenerator.GetNext());
        //        //List<Contour<Point>> convexContoursList = ImageProcessing.GetConvexContoursListFromNonConvexContour(currContour);
        //        //foreach (Contour<Point> convContour in convexContoursList)
        //        //{
        //        //    imgSunMaskSplittedContours.Draw(convContour, newBgrColor, 0);
        //        //}

        //        string randFName = "m" + Path.GetRandomFileName().Replace(".", "");
        //        ImageConditionAndDataRepresentingForm theWindow = ServiceTools.RepresentDataFromDenseMatrix(dmCurrContourGradData, "the contour margins grad field: " + randFName);
        //        ImageConditionAndDataRepresentingForm theWindowDDX = ServiceTools.RepresentDataFromDenseMatrix(dmCurrContourDDXData, "the contour margins d/dx field: " + randFName, false, false, 0.0d, 1.0d, false);
        //        ImageConditionAndDataRepresentingForm theWindowDDY = ServiceTools.RepresentDataFromDenseMatrix(dmCurrContourDDYData, "the contour margins d/dy field: " + randFName, false, false, 0.0d, 1.0d, false);
        //        //ImageConditionAndDataRepresentingForm theWindowMeanDDX = ServiceTools.RepresentDataFromDenseMatrix(dmDDXMeanDataAlongContour, "the contour margins mean d/dx field: " + randFName, false, false, 0.0d, 1.0d, false);
        //        //ImageConditionAndDataRepresentingForm theWindowMeanDDY = ServiceTools.RepresentDataFromDenseMatrix(dmDDYMeanDataAlongContour, "the contour margins mean d/dy field: " + randFName, false, false, 0.0d, 1.0d, false);
        //        string matrixNameGrad = "vardataSunBurnGrad" + randFName;
        //        string matrixNameDDX = "vardataSunBurnDDX" + randFName;
        //        string matrixNameDDY = "vardataSunBurnDDY" + randFName;
        //        //string matrixNameMeanDDX = "vardataMeanDDX" + randFName;
        //        //string matrixNameMeanDDY = "vardataMeanDDY" + randFName;
        //        string vectName = "vect" + randFName;
        //        string filenameGrad = "SunburnSpotsGrads" + randFName + ".nc";
        //        String filenameDDX = "SunburnSpotsDDXs" + randFName + ".nc";
        //        String filenameDDY = "SunburnSpotsDDYs" + randFName + ".nc";
        //        //String filenameMeanDDX = "SunburnSpotsMeanDDXs" + randFName + ".nc";
        //        //String filenameMeanDDY = "SunburnSpotsMeanDDYs" + randFName + ".nc";
        //        theWindow.SaveData(filenameGrad);
        //        theWindowDDX.SaveData(filenameDDX);
        //        theWindowDDY.SaveData(filenameDDY);
        //        //theWindowMeanDDX.SaveData(filenameMeanDDX);
        //        //theWindowMeanDDY.SaveData(filenameMeanDDY);

        //        ServiceTools.logToTextFile(baseDir + randFName + "_MeanAndStdDevDistributionAlongContour.csv", csvFileText);
        //        string matlabScript = "" + Environment.NewLine;
        //        matlabScript += "gradDistributionMean_" + randFName + " = " + meanGradValueAlongCurrentContour.ToString("e").Replace(",", ".") + ";" + Environment.NewLine;
        //        matlabScript += "gradDistributionStdDev_" + randFName + " = " + stdDevGradValueAlongCurrentContour.ToString("e").Replace(",", ".") + ";" + Environment.NewLine;
        //        matlabScript += matrixNameGrad + " = ncread(\'" + baseDir + filenameGrad + "\',\'dataMatrix\');" + Environment.NewLine;
        //        matlabScript += matrixNameDDX + " = ncread(\'" + baseDir + filenameDDX + "\',\'dataMatrix\');" + Environment.NewLine;
        //        matlabScript += matrixNameDDY + " = ncread(\'" + baseDir + filenameDDY + "\',\'dataMatrix\');" + Environment.NewLine;
        //        //matlabScript += matrixNameMeanDDX + " = ncread(\'" + baseDir + filenameMeanDDX + "\',\'dataMatrix\');" + Environment.NewLine;
        //        //matlabScript += matrixNameMeanDDY + " = ncread(\'" + baseDir + filenameMeanDDY + "\',\'dataMatrix\');" + Environment.NewLine;

        //        matlabScript += "idxGradIsZero" + randFName + " = (" + matrixNameGrad + " == 0);" + Environment.NewLine;
        //        //matlabScript += matrixNameMeanDDX + "(idxGradIsZero" + randFName + ") = NaN;" + Environment.NewLine;
        //        //matlabScript += matrixNameMeanDDY + "(idxGradIsZero" + randFName + ") = NaN;" + Environment.NewLine;

        //        matlabScript += vectName + " = reshape(" + matrixNameGrad + ", 1, []);" + Environment.NewLine;
        //        matlabScript += "idx = (" + vectName + " > 0);" + Environment.NewLine;
        //        matlabScript += vectName + " = " + vectName + "(idx);" + Environment.NewLine;
        //        matlabScript += "figure('Name', '" + randFName + "');" + Environment.NewLine;
        //        //nelements = hist(vect0up1mzqdv3p, 100);
        //        //text(mean(vect0up1mzqdv3p) * 1.3, max(nelements) * 0.7, strcat('mean = ', num2str(mean(vect0up1mzqdv3p))));
        //        //matlabScript += "nelements = hist(" + vectName + ", 100);" + Environment.NewLine;
        //        matlabScript += "[histNElem" + vectName + ", histCenters" + vectName + "] = hist(" + vectName + ", 100);" + Environment.NewLine;
        //        matlabScript += "bar(histCenters" + vectName + ", histNElem" + vectName + ");" + Environment.NewLine;
        //        matlabScript += "histMean_" + vectName + " = mean(" + vectName + ");" + Environment.NewLine;
        //        matlabScript += "histStdDev_" + vectName + " = std(" + vectName + ");" + Environment.NewLine;
        //        matlabScript += "hist" + vectName + "_MaxCount = max(histNElem" + vectName + ");" + Environment.NewLine;
        //        matlabScript += "line([histMean_" + vectName + " histMean_" + vectName + "], [0.0 hist" + vectName + "_MaxCount], 'Color', [0 1.0 0], 'LineWidth', 3.0);" + Environment.NewLine;
        //        matlabScript += "line([histMean_" + vectName + "-histStdDev_" + vectName + " histMean_" + vectName + "-histStdDev_" + vectName + "], [0.0 hist" + vectName + "_MaxCount], 'Color', [1.0 0 0], 'LineWidth', 2.0);" + Environment.NewLine;
        //        matlabScript += "line([histMean_" + vectName + "+histStdDev_" + vectName + " histMean_" + vectName + "+histStdDev_" + vectName + "], [0.0 hist" + vectName + "_MaxCount], 'Color', [1.0 0 0], 'LineWidth', 2.0);" + Environment.NewLine;


        //        matlabScript += "textstrHist(1) = {strcat('mean = ', num2str(histMean_" + vectName + "))};" + Environment.NewLine;
        //        matlabScript += "textstrHist(2) = {strcat('std = ', num2str(histStdDev_" + vectName + "))};" + Environment.NewLine;
        //        matlabScript += "textstrHist(3) = {strcat('contour partial area = ', '" + currArea.ToString("e") + "')}" + Environment.NewLine;
        //        matlabScript += "text(mean(" + vectName + ") * 1.3, max(histNElem" + vectName + ") * 0.7, textstrHist);" + Environment.NewLine;

        //        //matlabScript += "meanGradMaxCount" + randFName + " = max();" + Environment.NewLine;
        //        //matlabScript += "line([histMean_" + vectName + " 0.0], [histMean_" + vectName + ", 1.0]);" + Environment.NewLine;


        //        matlabScript += "csvdata = importdata('" + baseDir + randFName + "_MeanAndStdDevDistributionAlongContour.csv', ';');" + Environment.NewLine;
        //        matlabScript += "meanAlongContour" + randFName + " = csvdata(:,1);" + Environment.NewLine;
        //        //matlabScript += "idx" + randFName + " = (meanAlongContour" + randFName + " > 0.012);" + Environment.NewLine;
        //        //matlabScript += "meanAlongContour" + randFName + "(idx" + randFName + ") = NaN;" + Environment.NewLine;
        //        matlabScript += "stdDevAlongContour" + randFName + " = csvdata(:,2);" + Environment.NewLine;



        //        matlabScript += "figure('Name', 'meanGradDataHist_" + randFName + "');" + Environment.NewLine;
        //        matlabScript += "[histNElemvectHistMeanGradData" + randFName + ", histCentersvectHistMeanGradData" + randFName + "] = hist(meanAlongContour" + randFName + ", 100);" + Environment.NewLine;
        //        matlabScript += "bar(histCentersvectHistMeanGradData" + randFName + ", histNElemvectHistMeanGradData" + randFName + ");" + Environment.NewLine;
        //        matlabScript += "histMean_MeanGradData_vect" + randFName + " = mean(meanAlongContour" + randFName + ");" + Environment.NewLine;
        //        matlabScript += "histStdDev_MeanGradData_vect" + randFName + " = std(meanAlongContour" + randFName + ");" + Environment.NewLine;
        //        matlabScript += "histvect" + randFName + "_MaxCount_MeanGradData = max(histNElemvectHistMeanGradData" + randFName + ");" + Environment.NewLine;
        //        matlabScript += "line([histMean_MeanGradData_vect" + randFName + " histMean_MeanGradData_vect" + randFName + "], [0.0 histvect" + randFName + "_MaxCount_MeanGradData], 'Color', [0 1.0 0], 'LineWidth', 3.0);" + Environment.NewLine;
        //        matlabScript += "line([histMean_MeanGradData_vect" + randFName + "-histStdDev_MeanGradData_vect" + randFName + " histMean_MeanGradData_vect" + randFName + "-histStdDev_MeanGradData_vect" + randFName + "], [0.0 histvect" + randFName + "_MaxCount_MeanGradData], 'Color', [1.0 0 0], 'LineWidth', 2.0);" + Environment.NewLine;
        //        matlabScript += "line([histMean_MeanGradData_vect" + randFName + "+histStdDev_MeanGradData_vect" + randFName + " histMean_MeanGradData_vect" + randFName + "+histStdDev_MeanGradData_vect" + randFName + "], [0.0 histvect" + randFName + "_MaxCount_MeanGradData], 'Color', [1.0 0 0], 'LineWidth', 2.0);" + Environment.NewLine;
        //        matlabScript += "textstrHist2(1) = {strcat('mean = ', num2str(histMean_MeanGradData_vect" + randFName + "))};" + Environment.NewLine;
        //        matlabScript += "textstrHist2(2) = {strcat('std = ', num2str(histStdDev_MeanGradData_vect" + randFName + "))};" + Environment.NewLine;
        //        matlabScript += "text(histMean_MeanGradData_vect" + randFName + " * 1.3, histvect" + randFName + "_MaxCount_MeanGradData * 0.7, textstrHist2);" + Environment.NewLine;



        //        matlabScript += "meanPlusStdDev" + randFName + " = meanAlongContour" + randFName + " + stdDevAlongContour" + randFName + ";" + Environment.NewLine;
        //        matlabScript += "meanMinusStdDev" + randFName + " = meanAlongContour" + randFName + " - stdDevAlongContour" + randFName + ";" + Environment.NewLine;

        //        //matlabScript += "stdDevAlongContour" + randFName + "(idx" + randFName + ") = NaN;" + Environment.NewLine;
        //        //matlabScript += "pointsSum" + randFName + " = sum(idx" + randFName + ");" + Environment.NewLine;
        //        //matlabScript += "pointsRelative" + randFName + " = sum(idx" + randFName + ")/numel(idx" + randFName + ");" + Environment.NewLine;
        //        matlabScript += "pointsTotal" + randFName + " = numel(meanAlongContour" + randFName + ");" + Environment.NewLine;

        //        matlabScript += "fig = figure('Name', 'stats " + randFName + "');" + Environment.NewLine;
        //        //matlabScript += "set(fig,'units','normalized','outerposition',[0 0 1 1]);" + Environment.NewLine;
        //        matlabScript += "xSpace = 1:1:" + stdDevDataAlongContour.Count + ";" + Environment.NewLine;
        //        matlabScript += "maxContinousClusterLessthanMean" + randFName + " = " + maxContinousMeanGradValueCluster + ";" + Environment.NewLine;

        //        matlabScript += "scatter(xSpace, meanAlongContour" + randFName + ", 'g', 'LineWidth', 3.0);" + Environment.NewLine;
        //        matlabScript += "hold on;" + Environment.NewLine;
        //        //matlabScript += "scatter(xSpace, stdDevAlongContour" + randFName + ", 'r', 'LineWidth', 1.5);" + Environment.NewLine;
        //        //matlabScript += "errorbar(xSpace, meanAlongContour" + randFName + ", stdDevAlongContour" + randFName + ", 'r');" + Environment.NewLine;
        //        matlabScript += "plot(xSpace, meanPlusStdDev" + randFName + ", 'r', 'LineWidth', 1.0) ;" + Environment.NewLine;
        //        matlabScript += "plot(xSpace, meanMinusStdDev" + randFName + ", 'r', 'LineWidth', 1.0) ;" + Environment.NewLine;

        //        matlabScript += "line([0.0 " + stdDevDataAlongContour.Count + "], [gradDistributionMean_" + randFName + " gradDistributionMean_" + randFName + "], 'Color', [0 1.0 0], 'LineWidth', 3.0);" + Environment.NewLine;
        //        matlabScript += "line([0.0 " + stdDevDataAlongContour.Count + "], [gradDistributionMean_" + randFName + " - gradDistributionStdDev_" + randFName + " gradDistributionMean_" + randFName + " - gradDistributionStdDev_" + randFName + "], 'Color', [1.0 0 0], 'LineWidth', 2.0);" + Environment.NewLine;
        //        matlabScript += "line([0.0 " + stdDevDataAlongContour.Count + "], [gradDistributionMean_" + randFName + " + gradDistributionStdDev_" + randFName + " gradDistributionMean_" + randFName + " + gradDistributionStdDev_" + randFName + "], 'Color', [1.0 0 0], 'LineWidth', 2.0);" + Environment.NewLine;

        //        matlabScript += "hold off;" + Environment.NewLine;

        //        matlabScript += "title('grad() values mean and stddev along the contour " + randFName + "', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;
        //        matlabScript += "xlabel(gca, 'point along contour, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;
        //        matlabScript += "ylabel(gca, 'grad(GrIx) value', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;
        //        matlabScript += "ylim(gca, [0.0 0.5]);" + Environment.NewLine;
        //        //matlabScript += "textstr(1) = {strcat('relative sign points count = ', num2str(pointsRelative" + randFName + "))};" + Environment.NewLine;
        //        //matlabScript += "textstr(2) = {strcat('abs sign points count = ', num2str(pointsSum" + randFName + "))};" + Environment.NewLine;

        //        matlabScript += "textstrScatter(1) = {strcat('total points count = ', num2str(pointsTotal" + randFName + "))};" + Environment.NewLine;
        //        matlabScript += "textstrScatter(2) = {strcat('max grad mean less than mean cluster = ', num2str(maxContinousClusterLessthanMean" + randFName + "))};" + Environment.NewLine;
        //        matlabScript += "textstrScatter(3) = {strcat('relative max grad mean less than mean cluster = ', num2str(maxContinousClusterLessthanMean" + randFName + "/pointsTotal" + randFName + "))};" + Environment.NewLine;
        //        matlabScript += "textstrScatter(4) = {strcat('mean grad values mean = ', num2str(gradDistributionMean_" + randFName + "))};" + Environment.NewLine;

        //        matlabScript += "text(max(xSpace) * 0.1, 0.4, textstrScatter);" + Environment.NewLine;

        //        //matlabScript += "fig = figure('Name', 'grad directions " + randFName + "');" + Environment.NewLine;
        //        //matlabScript += "quiver(" + matrixNameMeanDDX + ", " + matrixNameMeanDDY + ", 3.0);" + Environment.NewLine;
        //        ////matlabScript += ";" + Environment.NewLine;
        //        ////matlabScript += ";" + Environment.NewLine;
        //        ////matlabScript += ";" + Environment.NewLine;
        //        ////matlabScript += ";" + Environment.NewLine;
        //        ////matlabScript += ";" + Environment.NewLine;
        //        //matlabScript += "title('grad vect field along the contour " + randFName + "', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;
        //        //matlabScript += "xlabel(gca, 'x, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;
        //        //matlabScript += "ylabel(gca, 'y, px', 'FontWeight','bold', 'FontUnits', 'normalized', 'FontSize', 0.03);" + Environment.NewLine;

        //        ServiceTools.logToTextFile(baseDir + "ShowHistScript.m", matlabScript, true);


        //        //List<double> valuesToStat = new List<double>();
        //        //foreach (double dVal in dmCurrContourGradData.Values)
        //        //{
        //        //    if (dVal == 0.0) continue;
        //        //    if (double.IsNaN(dVal)) continue;
        //        //    valuesToStat.Add(dVal);
        //        //}

        //        DescriptiveStatistics stats = DataAnalysis.StatsOfDataExcludingValues(dmCurrContourGradData, 0.0d);
        //        if (stats != null)
        //        {
        //            contourMeanGradDataStdDev.Add(stats.StandardDeviation);
        //            contourMeanGradDataMean.Add(stats.Mean);
        //            contourMaxSunMarginCluster.Add((double)maxContinousMeanGradValueCluster);
        //            contourMaxSunMarginClusterRel.Add((double)maxContinousMeanGradValueCluster / (double)currContour.Count());
        //            countedContours.Add(currContour);
        //            contoursName.Add(randFName);
        //        }



        //        //DescriptiveStatistics stats = new DescriptiveStatistics(valuesToStat, true);
        //        //if (stats.StandardDeviation <= minGradStDevValue)
        //        //{
        //        //    minGradStDevValue = stats.StandardDeviation;
        //        //    foundSunContour = currContour;
        //        //}
        //    }
        //    //ServiceTools.ShowPicture(imgSunMaskSplittedContours.Bitmap);

        //    // 1. посчитаем площади. Отфильтруем объекты с относительной площадью меньше 2.5e-3
        //    // 2. посчитаем стандартное отклонение градиентов без учета нулей. объект с наименьшим градиентом - объект, содержащий солнце
        //    // 3. При площади засветки максимального пятна больше 0,02 - это обложная засветка, не использовать схему с подавлением солнечной засветки
        //    //  3а в этом случае использовать схему прямого разделения.
        //    //      При этом чем больше засветка, - тем выше граница.
        //    //      При стандартной засветке площадью 0,0025 граница - ???
        //    //      При площади засветки 0,0516 - 0,92

        //    int indexMaxRelativeSunMargin =
        //        DenseVector.OfEnumerable(contourMaxSunMarginClusterRel).AbsoluteMaximumIndex();
        //    int indexMaxSunMargin =
        //        DenseVector.OfEnumerable(contourMaxSunMarginCluster).AbsoluteMaximumIndex();
        //    int indexMinGradStdDev = DenseVector.OfEnumerable(contourMeanGradDataStdDev).AbsoluteMinimumIndex();
        //    int indexMinMeanGradValue = DenseVector.OfEnumerable(contourMeanGradDataMean).AbsoluteMinimumIndex();

        //    if ((indexMaxRelativeSunMargin == indexMaxSunMargin) && (indexMaxSunMargin == indexMinGradStdDev) && (indexMinMeanGradValue == indexMinGradStdDev))
        //    {
        //        currentLogWindow = ServiceTools.LogAText(currentLogWindow,
        //            "found the contour with" + Environment.NewLine +
        //            "rel max sunburn margin length = " + contourMaxSunMarginClusterRel[indexMaxRelativeSunMargin] + "" +
        //            Environment.NewLine +
        //            "abs max sunburn margin length = " + contourMaxSunMarginCluster[indexMaxSunMargin] + "" +
        //            Environment.NewLine +
        //            "min grad stddev value = " + contourMeanGradDataStdDev[indexMinGradStdDev] + "" + Environment.NewLine +
        //            "contour name = " + contoursName[indexMaxRelativeSunMargin]);
        //        foundSunContour = countedContours[indexMaxRelativeSunMargin];
        //    }
        //    else
        //    {
        //        currentLogWindow = ServiceTools.LogAText(currentLogWindow,
        //            "contours parameters is:" + Environment.NewLine +
        //            "rel max sunburn margin length = " + contourMaxSunMarginClusterRel[indexMaxRelativeSunMargin] + " (name = " + contoursName[indexMaxRelativeSunMargin] + ")" + Environment.NewLine +
        //            "abs max sunburn margin length = " + contourMaxSunMarginCluster[indexMaxSunMargin] + " (name = " + contoursName[indexMaxSunMargin] + ")" + Environment.NewLine +
        //            "min grad stddev value = " + contourMeanGradDataStdDev[indexMinGradStdDev] + " (name = " + contoursName[indexMinGradStdDev] + ")" + Environment.NewLine +
        //            "min grad mean value = " + contourMeanGradDataMean[indexMinMeanGradValue] + " (name = " + contoursName[indexMinMeanGradValue] + ")" + Environment.NewLine +
        //            "contour, to count on, name = " + contoursName[indexMinMeanGradValue]);
        //        foundSunContour = countedContours[indexMinMeanGradValue];
        //    }



        //    #endregion посчитаем для объектов засветки разные солнечные метрики




        //    #region оценка применимости алгоритма подавления солнечной засветки
        //    // оценить общую площадь засветки 255 - должно быть прилично ---СКОЛЬКО?
        //    // найдем все куски площадью больше - ЧЕГО?
        //    // оценить разброс кластеров засветки - должно быть единое круглое солнце, возможно, с лучами
        //    // если засветки слишком мало - неприменимо, использовать без подавления
        //    // если засветки слишком много - неприменимо, использовать без подавления

        //    bool theSunSuppressionSchemeApplicable = true;



        //    #region поиск солнца по найденному контуру

        //    if (foundSunContour != null)
        //    {
        //        #region обработаем найденный контур еще раз

        //        DenseMatrix dmDistanceToCurrContour = DenseMatrix.Create(dmProcessingData.RowCount,
        //            dmProcessingData.ColumnCount, (row, col) =>
        //            {
        //                PointF thePoint = new PointF((float)col, (float)row);
        //                return foundSunContour.Distance(thePoint);
        //            });

        //        DenseMatrix dmPointsOutsideCurrContour = DenseMatrix.Create(dmProcessingData.RowCount,
        //            dmProcessingData.ColumnCount, (row, col) =>
        //            {
        //                double currDistance = dmDistanceToCurrContour[row, col];
        //                if (currDistance > 0.0d) return 0.0;
        //                else return 1.0d;
        //            });

        //        DenseMatrix dmCurrContourGradData = (DenseMatrix)dmGradField.Clone();
        //        dmCurrContourGradData.MapIndexedInplace((row, col, val) =>
        //        {
        //            double currDistance = dmDistanceToCurrContour[row, col];
        //            if (Math.Abs(currDistance) > 5.0d) return 0.0;
        //            if (currDistance > 0.0d) return 0.0;
        //            else return val;
        //        });

        //        List<Point> countedContourPoints = new List<Point>();
        //        List<double> meanDataAlongContour = new List<double>();
        //        List<double> stdDevDataAlongContour = new List<double>();

        //        foreach (Point foundContourPoint in foundSunContour)
        //        {

        //            DenseMatrix dmCurrpointDistance = DenseMatrix.Create(dmProcessingData.RowCount,
        //                dmProcessingData.ColumnCount,
        //                (row, col) =>
        //                    PointD.Distance(new PointD((double)col, (double)row), new PointD(foundContourPoint)));
        //            DenseMatrix dmCurrpointDistanceLessThan5 = (DenseMatrix)dmCurrpointDistance.Clone();
        //            dmCurrpointDistanceLessThan5.MapInplace(x => (x <= 5.0d) ? (x) : (0.0d));

        //            Func<int, int, double, double> fLessThan5DistanceToCurrentPoint =
        //                (row, col, x) => x * dmCurrpointDistanceLessThan5[row, col];
        //            Func<int, int, double, double> fPointsOutsideTheContour =
        //                (row, col, x) => x * dmPointsOutsideCurrContour[row, col];

        //            DenseMatrix dmCurrContourGradStatsData = (DenseMatrix)dmGradField.Clone();
        //            dmCurrContourGradStatsData.MapIndexedInplace(fLessThan5DistanceToCurrentPoint);
        //            dmCurrContourGradStatsData.MapIndexedInplace(fPointsOutsideTheContour);

        //            DescriptiveStatistics statsCurrPointGradData = DataAnalysis.StatsOfDataExcludingValues(dmCurrContourGradStatsData, 0.0d);
        //            if (statsCurrPointGradData != null)
        //            {
        //                meanDataAlongContour.Add(statsCurrPointGradData.Mean);
        //                stdDevDataAlongContour.Add(statsCurrPointGradData.StandardDeviation);
        //                countedContourPoints.Add(foundContourPoint);
        //            }
        //        }

        //        DescriptiveStatistics statsGradMeansAlongContour = new DescriptiveStatistics(meanDataAlongContour);
        //        double meanGradValueAlongCurrentContour = statsGradMeansAlongContour.Mean;


        //        DenseMatrix dmCurrContourGradDataCapturedSunMargin = (DenseMatrix)dmCurrContourGradData.Clone();
        //        DenseMatrix dmCurrContourGradDataCapturedSunMarginMask = (DenseMatrix)dmCurrContourGradData.Clone();
        //        dmCurrContourGradDataCapturedSunMarginMask.MapInplace(x => (x > 0.0d) ? (1.0d) : (0.0d));
        //        List<PointD> acceptablePointsList = new List<PointD>();
        //        List<double> lPointsWeights = new List<double>();

        //        foreach (Point foundContourPoint in foundSunContour)
        //        {
        //            // найдем данные по этой точке
        //            int ind = countedContourPoints.FindIndex(pt => pt == foundContourPoint);
        //            if (ind == -1)
        //            {
        //                // точка не была учтена в статистиках, нет в листе точек, по которым посчитана статистика - пропускаем и тут
        //                continue;
        //            }
        //            double currGradMean5pxValue = meanDataAlongContour[ind];

        //            DenseMatrix dmCurrpointDistance = DenseMatrix.Create(dmProcessingData.RowCount,
        //                dmProcessingData.ColumnCount,
        //                (row, col) =>
        //                    PointD.Distance(new PointD((double)col, (double)row), new PointD(foundContourPoint)));

        //            // если проходит в качестве границы - сформировать матрицу, где в пределах 5px 1.0, остальные нули, и маску с ней сложить
        //            if (currGradMean5pxValue <= meanGradValueAlongCurrentContour)
        //            {
        //                DenseMatrix dmCurrpointDistanceLessThan5 = (DenseMatrix)dmCurrpointDistance.Clone();
        //                dmCurrpointDistanceLessThan5.MapInplace(x => (x <= 5.0d) ? (1.0) : (0.0d));
        //                dmCurrContourGradDataCapturedSunMarginMask = dmCurrContourGradDataCapturedSunMarginMask +
        //                                                             dmCurrpointDistanceLessThan5;
        //                acceptablePointsList.Add(new PointD(foundContourPoint));
        //                lPointsWeights.Add(currGradMean5pxValue);
        //            }
        //            // если не проходит - надо сформировать матрицу, где в пределах 5px нули, остальные 1.0, и на нее домножить маску
        //            else
        //            {
        //                DenseMatrix dmCurrpointDistanceLessThan5 = (DenseMatrix)dmCurrpointDistance.Clone();
        //                dmCurrpointDistanceLessThan5.MapInplace(x => (x <= 5.0d) ? (0.0d) : (1.0d));
        //                dmCurrContourGradDataCapturedSunMarginMask =
        //                    (DenseMatrix)
        //                        dmCurrContourGradDataCapturedSunMarginMask.PointwiseMultiply(
        //                            dmCurrpointDistanceLessThan5);
        //            }
        //        }
        //        dmCurrContourGradDataCapturedSunMarginMask.MapInplace(x => (x > 0.0d) ? (1.0d) : (0.0d));
        //        dmCurrContourGradDataCapturedSunMargin =
        //            (DenseMatrix)
        //                dmCurrContourGradDataCapturedSunMargin.PointwiseMultiply(
        //                    dmCurrContourGradDataCapturedSunMarginMask);

        //        Image<Bgr, byte> SunMarginGradToShowImage =
        //            ImageProcessing.evalResultColored(dmCurrContourGradDataCapturedSunMargin, maskImage,
        //                new ColorScheme(""));


        //        // есть набор точек, которые подходят под описание границы солнца
        //        // есть уравнение окружности с центром в (a,b) и радиусом c - три параметра
        //        // начальные значения можно взять как центр масс набора точек - центр окружности, и радиус c - как радиус круга с площадью равной площади необрезанного контура
        //        // снова применим аппроксимацию методом градиентного спуска. минимизировать будем такую функцию:
        //        //          сумму абсолютных отклонений c от расстояния от точки контура до центра (a,b)

        //        //DenseVector dvZeroValues = DenseVector.Create(acceptablePointsList.Count, (i => 0.0d));
        //        //DenseVector dvIntSpace = DenseVector.Create(acceptablePointsList.Count, (i => (double)i));

        //        DenseVector dvPointsWeight = DenseVector.OfEnumerable(lPointsWeights);
        //        double gradMaxVal = dvPointsWeight.Max();
        //        double gradMinVal = dvPointsWeight.Min();
        //        dvPointsWeight.MapInplace(x =>
        //        {
        //            return (1.0d - (x - gradMinVal)/(gradMaxVal - gradMinVal));
        //        }, true);

        //        Func<DenseVector, PointD, double> minimizingFunc = (dvParameters, pt) =>
        //        {
        //            double sunCenterX = dvParameters[0];
        //            double sunCenterY = dvParameters[1];
        //            double r = dvParameters[2];
        //            double diffX = pt.X - sunCenterX;
        //            double diffY = pt.Y - sunCenterY;
        //            double diff = r - Math.Sqrt(diffX * diffX + diffY * diffY);
        //            //diff = Math.Abs(diff);
        //            return diff;
        //        };

        //        double initSunCenterX = acceptablePointsList.Sum((pt => pt.X)) / (double)acceptablePointsList.Count;
        //        double initSunCenterY = acceptablePointsList.Sum((pt => pt.Y)) / (double)acceptablePointsList.Count;
        //        double initSunRadius = Math.Sqrt(foundSunContour.Area / Math.PI);
        //        List<double> parametersList = new List<double>();
        //        parametersList.Add(initSunCenterX);
        //        parametersList.Add(initSunCenterY);
        //        parametersList.Add(initSunRadius);
        //        DenseVector dvInitialParameters = DenseVector.OfEnumerable(parametersList);
        //        DenseVector initialParametersIncremnt = DenseVector.Create(dvInitialParameters.Count, (i => 1.0d));

        //        GradientDescentApproximator approximator = new GradientDescentApproximator(acceptablePointsList, minimizingFunc);
        //        approximator.DvWeights = dvPointsWeight;
        //        //approximator.DvWeights = "";
        //        DenseVector approximatedParameters = approximator.ApproximationGradientDescent2DPt(dvInitialParameters, initialParametersIncremnt, 0.000001d);

        //        #endregion обработаем найденный контур еще раз




        //        Image<Gray, Byte> imgSunMaskFiltered = imgSunMask.CopyBlank();
        //        imgSunMaskFiltered.Draw(foundSunContour, new Gray(255), -1);
        //        DenseMatrix dmSunMask = ImageProcessing.DenseMatrixFromImage(imgSunMaskFiltered);
        //        dmSunMask.MapInplace(val => val / 255.0d);

        //        //DenseMatrix.Create(dmProcessingData.RowCount, dmProcessingData.ColumnCount,
        //        //new Func<int, int, double>(
        //        //    (row, column) =>
        //        //    {
        //        //        if (dmProcessingData[row, column] == 1.0d) return 1.0d;
        //        //        else return 0.0d;
        //        //    }));
        //        //ImageConditionAndDataRepresentingForm sunMaskDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmSunMask, "Sun mask");
        //        //PointD sunCenterPoint = new PointD(0.0d, 0.0d);

        //        #region заменен алгоритм нахождения солнца
        //        //double weightsSum = dmSunMask.Values.Sum();
        //        //DenseMatrix dmSunCenterResearchingWeights = (DenseMatrix)dmSunMask.Clone();
        //        //dmSunCenterResearchingWeights.MapInplace(new Func<double, double>(val => val / weightsSum));
        //        //DenseMatrix dmSunCenterResearching = DenseMatrix.Create(dmSunCenterResearchingWeights.RowCount,
        //        //    dmSunCenterResearchingWeights.ColumnCount,
        //        //    new Func<int, int, double>((row, column) => (double)column));
        //        //dmSunCenterResearching = (DenseMatrix)dmSunCenterResearching.PointwiseMultiply(dmSunCenterResearchingWeights);
        //        //sunCenterPoint.X = dmSunCenterResearching.Values.Sum();
        //        //dmSunCenterResearching = DenseMatrix.Create(dmSunCenterResearchingWeights.RowCount,
        //        //    dmSunCenterResearchingWeights.ColumnCount, new Func<int, int, double>((row, column) => (double)row));
        //        //dmSunCenterResearching = (DenseMatrix)dmSunCenterResearching.PointwiseMultiply(dmSunCenterResearchingWeights);
        //        //sunCenterPoint.Y = dmSunCenterResearching.Values.Sum();
        //        //double sunRadius = Math.Sqrt(weightsSum / Math.PI);
        //        #endregion заменен алгоритм нахождения солнца

        //        PointD sunCenterPoint = new PointD(approximatedParameters[0], approximatedParameters[1]);
        //        double sunRadius = approximatedParameters[2];




        //        Image<Bgr, Byte> imageSunDemonstration = ImageProcessing.evalResultColored(dmProcessingData, maskImage,
        //            new ColorScheme(""));
        //        Image<Bgr, Byte> calculatedSunMaskImage = imageSunDemonstration.CopyBlank();
        //        calculatedSunMaskImage.Draw(new CircleF(new PointF((float)sunCenterPoint.X, (float)sunCenterPoint.Y), (float)sunRadius), new Bgr(255, 255, 255), 0);
        //        imageSunDemonstration = imageSunDemonstration.AddWeighted(calculatedSunMaskImage, 0.7, 0.3, 0.0);
        //        ServiceTools.ShowPicture(imageSunDemonstration, "Sun position and radius demonstration");

        //        SunMarginGradToShowImage = SunMarginGradToShowImage.AddWeighted(calculatedSunMaskImage, 0.7, 0.3, 0.0);
        //        ServiceTools.ShowPicture(SunMarginGradToShowImage, "SUN margin image");

        //        currentLogWindow = ServiceTools.LogAText(currentLogWindow, "положение центра солнца: " + sunCenterPoint + Environment.NewLine);
        //        currentLogWindow = ServiceTools.LogAText(currentLogWindow, "радиус солнечной засветки: " + sunRadius + Environment.NewLine);
        //    #endregion поиск солнца по найденному контуру


        //        // пройдемся по всем раздельным объектам засветки и оценим площадь каждого из них
        //        /*
        //        Contour<Point> contoursDetected = imgSunMask.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
        //        List<Contour<Point>> contoursArray = new List<Contour<Point>>();

        //        while (true)
        //        {
        //            Contour<Point> currContour = contoursDetected;
        //            contoursArray.Add(currContour);

        //            contoursDetected = contoursDetected.HNext;
        //            if (contoursDetected == null)
        //                break;
        //        }

        //        List<Contour<Point>> contoursArrayFilteredByArea = new List<Contour<Point>>();
        //        if (contoursArray.Count > 0)
        //        {
        //            foreach (Contour<Point> contour in contoursArray)
        //            {
        //                double currArea = contour.Area / overallimageMaskArea;
        //                if ((currArea <= maxSunAreaPart) && (currArea >= minSunAreaPart))
        //                {
        //                    contoursArrayFilteredByArea.Add(contour);
        //                }
        //            }
        //        }

        //        // =====================================================================
        //        //     еще отфильтровать по форме. постараться найти объекты покруглее
        //        // =====================================================================

        //        if (contoursArrayFilteredByArea.Count == 0)
        //        {
        //            theSunSuppressionSchemeApplicable = false;
        //        }
        //         */


        //        //currentLogWindow = ServiceTools.LogAText(currentLogWindow, "average sun data grad: " + );
        //    }
        //    else
        //    {
        //        theSunSuppressionSchemeApplicable = false;
        //    }

        //    currentLogWindow = ServiceTools.LogAText(currentLogWindow, "sun suppression scheme applicable: " + theSunSuppressionSchemeApplicable.ToString());

        //    #endregion оценка применимости алгоритма подавления солнечной засветки
        //}
        #endregion // УСТАРЕЛО - отдельно стоящий метод определения местоположения солнца





        private void button3_Click_1(object sender, EventArgs e)
        {
            //row - brightness
            //col - grayness
            //DenseMatrix dmDataR = DenseMatrix.Create(250, 250, (r, c) =>
            //{
            //    double brightnessLimit = (double)(r + 2);
            //    return ((double)(c + 2) / 255.0d) * brightnessLimit;
            //});
            //DenseMatrix dmDataG = (DenseMatrix)dmDataR.Clone();
            //DenseMatrix dmDataB = DenseMatrix.Create(250, 250, (r, c) =>
            //{
            //    return (double)(r + 2);
            //});
            //Image<Gray, Byte> imgR = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmDataR, 0.0d, 255.0d);
            //Image<Gray, Byte> imgG = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmDataG, 0.0d, 255.0d);
            //Image<Gray, Byte> imgB = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmDataB, 0.0d, 255.0d);
            ////ServiceTools.ShowPicture(imgR.Bitmap);
            ////ServiceTools.ShowPicture(imgG.Bitmap);
            ////ServiceTools.ShowPicture(imgB.Bitmap);
            //Image<Bgr, Byte> imgRes = new Image<Bgr, byte>(new Image<Gray, byte>[] { imgB, imgG, imgR });
            //ServiceTools.ShowPicture(imgRes.Bitmap);
            //imgRes.Bitmap.Save((string)defaultProperties["DefaultDataFilesLocation"] + "TestColoringGrIxBitmap.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            //ArithmeticsOnImages aoi = new ArithmeticsOnImages();
            //aoi.dmR = dmDataR;
            //aoi.dmG = dmDataG;
            //aoi.dmB = dmDataB;
            //aoi.ExprString = "1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y";
            //aoi.RPNeval();
            //ColorScheme evaluatingColorScheme = new ColorScheme("");
            //DenseMatrix dmRes = (DenseMatrix)aoi.dmRes.Clone();
            //aoi.Dispose();
            //aoi = null;
            //imageConditionAndData imgData = new imageConditionAndData(dmRes, null);
            //imgData.currentColorScheme = new ColorScheme("");
            //imgData.currentColorSchemeRuler = new ColorSchemeRuler(imgData.currentColorScheme);
            //imgData.currentColorSchemeRuler.imgToRule = imgData;
            //imgData.currentColorSchemeRuler.IsMarginsFixed = false;
            //ImageConditionAndDataRepresentingForm form1 = new ImageConditionAndDataRepresentingForm(imgData, "GrIx");
            //form1.Show();



            FileInfo fInfo = new FileInfo(ImageFileName);
            //imagetoadd = Image.FromFile(fInfo.FullName);
            imagetoadd = new Image<Bgr,byte>(fInfo.FullName);
            imagetoadd = ImageProcessing.ImageResizer(imagetoadd, Convert.ToInt32(defaultProperties["DefaultMaxImageSize"]));

            ImageProcessing imgP = new ImageProcessing(imagetoadd, true);
            Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(100.0d);
            Image<Gray, Byte> maskImageCircled85 = imgP.imageSignificantMaskCircled(85.0d);

            DenseMatrix dmMaskCircled100 = ImageProcessing.DenseMatrixFromImage(maskImageCircled);
            DenseMatrix dmMaskCircled85 = ImageProcessing.DenseMatrixFromImage(maskImageCircled85);
            ServiceTools.FlushMemory();

            string randomFileName = "m" + System.IO.Path.GetRandomFileName().Replace(".", "");

            string currentDirectory = (string)defaultProperties["DefaultDataFilesLocation"];

            DenseMatrix dmGrixData = imgP.eval("grix+0", null);
            dmGrixData = (DenseMatrix)dmGrixData.PointwiseMultiply(dmMaskCircled100);
            DenseVector dvGrixData = DataAnalysis.DataVectorizedWithCondition(dmGrixData, dval => ((dval > 0.0d) && (dval < 1.0d)));

            HistogramDataAndProperties theHist = new HistogramDataAndProperties(dvGrixData, 500);
            theHist.quantilesCount = 20;
            HistogramCalcAndShowForm theHistForm = new HistogramCalcAndShowForm(ImageFileName);
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


















        private void btnTestMarginWithoutSun_Click(object sender, EventArgs e)
        {
            DenseMatrix dmRedChannel;
            DenseMatrix dmBlueChannel;
            DenseMatrix dmGreenChannel;
            //LogWindow theLogWindow = null;

            //Bitmap LocalProcessingBitmap = (Bitmap)imagetoadd.Clone();

            Image<Gray, Byte> imageBlueChannelByte = imagetoadd[0].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[0];
            Image<Gray, Byte> imageGreenChannelByte = imagetoadd[1].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[1];
            Image<Gray, Byte> imageRedChannelByte = imagetoadd[2].Copy(); // new Image<Bgr, Byte>(LocalProcessingBitmap)[2];



            //Image<Gray, Byte> maskImage = ImageProcessing.getImageSignificantMask(LocalProcessingBitmap);
            ImageProcessing imgP = new ImageProcessing(imagetoadd, true);
            //imgP.getImageSignificantMask();
            Image<Gray, Byte> maskImage = imgP.significantMaskImageBinary;
            Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(90.0d);
            PointD imageCircleCenter = imgP.imageRD.pointDCircleCenter();
            double imageCircleRadius = imgP.imageRD.DRadius;

            imageBlueChannelByte = imageBlueChannelByte.Mul(maskImage);
            imageRedChannelByte = imageRedChannelByte.Mul(maskImage);
            imageGreenChannelByte = imageGreenChannelByte.Mul(maskImage);
            dmRedChannel = ImageProcessing.DenseMatrixFromImage(imageRedChannelByte);
            dmBlueChannel = ImageProcessing.DenseMatrixFromImage(imageBlueChannelByte);
            dmGreenChannel = ImageProcessing.DenseMatrixFromImage(imageGreenChannelByte);
            ServiceTools.FlushMemory();





            string randomFileName = "m" + System.IO.Path.GetRandomFileName().Replace(".", "");


            string currentDirectory = (string)defaultProperties["DefaultDataFilesLocation"];

            string formulaString = "1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y";
            //string formulaString = "1.0 - (1.0 + ((B-R)/(B+R)))/2.0";
            DenseMatrix dmProcessingData = (DenseMatrix)imgP.eval(formulaString, dmRedChannel, dmGreenChannel, dmBlueChannel, null).Clone();
            DenseMatrix dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);
            DenseMatrix dmMaskCircled = ImageProcessing.DenseMatrixFromImage(maskImageCircled);
            dmProcessingData = (DenseMatrix)dmProcessingData.PointwiseMultiply(dmMask);

            DenseMatrix dmReversed = (DenseMatrix)dmProcessingData.Clone();
            //dmReversed.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            //{
            //    if (dmMask[row, column] == 0) return 1.0d;
            //    else if (val >= theStdDevMarginValueDefiningSkyCloudSeparation) return 1.0d;
            //    else return 0.0d;
            //}));

            ImageConditionAndDataRepresentingForm restoredDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmReversed,
                "finally restored 1-sigma/Y data", true, false, 0.0d, 1.0d, true);
            ImageConditionAndDataRepresentingForm originalDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmProcessingData,
                "original 1-sigm/Y data", true, false, 0.0d, 1.0d, true);
            restoredDataForm.SaveData(randomFileName + "_res.nc");
            originalDataForm.SaveData(randomFileName + "_orig.nc");
            //originalDataForm.Close();
            //originalDataForm.Dispose();
            //restoredDataForm.Close();
            //restoredDataForm.Dispose();

            double cloudSkySeparationValue = tunedSIMargin;
            ColorScheme skyCloudColorScheme = ColorScheme.InversedBinaryCloudSkyColorScheme(cloudSkySeparationValue, 0.0d, 1.0d);
            ColorSchemeRuler skyCloudRuler = new ColorSchemeRuler(skyCloudColorScheme, 0.0d, 1.0d);
            Image<Bgr, Byte> previewImage = ImageProcessing.evalResultColoredWithFixedDataBounds(dmProcessingData, maskImage, skyCloudColorScheme, 0.0d, 1.0d);
            int cloudCounter = previewImage.CountNonzero()[1];
            int skyCounter = maskImage.CountNonzero()[0] - cloudCounter;
            double CloudCover = (double)cloudCounter / (double)(skyCounter + cloudCounter);

            //Bitmap localPreviewBitmap = previewImage.Bitmap;

        }




        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            var settings = Properties.Settings.Default;
            foreach (SettingsProperty property in settings.Properties)
            {
                defaultProperties.Add(property.Name, settings[property.Name]);
            }

            tunedSIMargin = Convert.ToDouble(defaultProperties["JapanCloudSkySeparationValue"]);
            //tunedIFMMargin = Convert.ToDouble(defaultProperties["GermanCloudSkySeparationValue"]);
            tunedSIMarginDefault = tunedSIMargin;
            //tunedIFMMarginDefault = tunedIFMMargin;

            string CurDir = Directory.GetCurrentDirectory();
            string path2process = (string)defaultProperties["BatchProcessingDirectory"];
            if (path2process == "")
            {
                path2process = CurDir;
            }

            richTextBox1.Text = path2process;
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


        //    DenseMatrix dmSunburnData = imgP.eval("Y+0", null);
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








        private void btnTest1_Click(object sender, EventArgs e)
        {
            string path2process = (string)defaultProperties["BatchProcessingDirectory"];
            if (path2process == "")
            {
                path2process = Directory.GetCurrentDirectory();
            }

            DirectoryInfo dir = new DirectoryInfo(path2process);

            if (!dir.Exists)
            {
                ThreadSafeOperations.SetTextTB(textBox1, textBox1.Text + "Операция не выполнена. Не найдена директория:" + Environment.NewLine + path2process + Environment.NewLine, true);
                return;
            }

            FileInfo[] fileList2Process = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
            if (fileList2Process.Length == 0)
            {
                Note("Coudn't find any *.jpg file in processing directiory or its subdirectories: \"" + path2process + "\" ");
                return;
            }


            List<bool> bgwFinished = new List<bool>();
            List<BackgroundWorker> bgwList = new List<BackgroundWorker>();
            for (int i = 0; i < 8; i++)
            {
                bgwFinished.Add(true);
                bgwList.Add(null);
            }


            DoWorkEventHandler thisWorkerDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
            {
                object[] currBGWarguments = (object[])args.Argument;
                string currentFullFileName = (string)currBGWarguments[0];
                Dictionary<string, object> defaultProps = (Dictionary<string, object>)currBGWarguments[1];
                int currentBgwID = (int)currBGWarguments[2];

                FileInfo fInfo = new FileInfo(currentFullFileName);

                int maxImageSize = Convert.ToInt32(defaultProps["DefaultMaxImageSize"]); ;

                BackgroundWorker SelfWorker = currBGWsender as System.ComponentModel.BackgroundWorker;

                string outputStatsDirectory = (string)defaultProps["DefaultDataFilesLocation"];

                //ServiceTools.logToTextFile(outputStatsDirectory + "statsWithFNames.dat", "Full filename | 5prc | median" + Environment.NewLine, true);

                //Image img = Image.FromFile(fInfo.FullName);
                //img = ImageProcessing.ImageResizer(img, maxImageSize);

                //Bitmap bitmap2process = ServiceTools.ReadBitmapFromFile(fInfo.FullName);
                //bitmap2process = ImageProcessing.BitmapResizer(bitmap2process, maxImageSize);
                Image<Bgr, byte> img2process = new Image<Bgr, byte>(fInfo.FullName);
                img2process = ImageProcessing.ImageResizer(img2process, maxImageSize);

                while (img2process.CountNonzero().Sum() == 0)
                {
                    img2process = new Image<Bgr, byte>(fInfo.FullName);
                    img2process = ImageProcessing.ImageResizer(img2process, maxImageSize);
                }

                ImageProcessing imgP = new ImageProcessing(img2process, true);

                Image<Gray, Byte> maskImageCircled = imgP.imageSignificantMaskCircled(100.0d);
                Image<Gray, Byte> maskImageCircled85 = imgP.imageSignificantMaskCircled(85.0d);

                DenseMatrix dmMaskCircled100 = ImageProcessing.DenseMatrixFromImage(maskImageCircled);
                ServiceTools.FlushMemory();

                DenseMatrix dmGrixData = imgP.eval("1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y", null);
                dmGrixData = (DenseMatrix)dmGrixData.PointwiseMultiply(dmMaskCircled100);
                DenseVector dvGrixData = DataAnalysis.DataVectorizedWithCondition(dmGrixData, dval => ((dval > 0.0d) && (dval < 1.0d)));
                DescriptiveStatistics stats = new DescriptiveStatistics(dvGrixData, true);
                double median = stats.Median;
                double perc5 = Statistics.Percentile(dvGrixData, 5);

                bool success = false;
                while (!success)
                {
                    try
                    {
                        ServiceTools.logToTextFile(outputStatsDirectory + "statsWithFNames.dat",
                            fInfo.FullName + ";" + median.ToString("e").Replace(",", ".") + ";" +
                            perc5.ToString("e").Replace(",", ".") + Environment.NewLine, true);
                        success = true;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(100);
                    }
                }

                //SimpleShowImageForm SimpleShowImageForm1 = new SimpleShowImageForm(imgP.processingBitmap(), "");
                //SimpleShowImageForm1.DesktopLocation =
                //    new Point(
                //        Convert.ToInt32(10.0d +
                //                        (double) currentBgwID*System.Windows.SystemParameters.PrimaryScreenWidth/
                //                        (double) bgwFinished.Count), 10);
                
                //SimpleShowImageForm1.Show();
                //System.Windows.Forms.Application.DoEvents();
                //Thread.Sleep(500);
                //SimpleShowImageForm1.Close();
                //SimpleShowImageForm1.Dispose();

                args.Result = new object[] {currentBgwID};
            };


            RunWorkerCompletedEventHandler currWorkCompletedHandler =
                delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    int returningBGWthreadID = (int)currentBGWResults[0];

                    BackgroundWorker currBGW = currBGWCompletedSender as BackgroundWorker;
                    currBGW.Dispose();

                    bgwFinished[returningBGWthreadID] = true;
                    bgwList[returningBGWthreadID].Dispose();
                    bgwList[returningBGWthreadID] = null;
                };




            foreach (FileInfo info in fileList2Process)
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


                theLogWindow = ServiceTools.LogAText(theLogWindow,
                Environment.NewLine + "starting: " + info.FullName);



                object[] BGWorker2Args = new object[] { info.FullName, defaultProperties, currentBgwID };

                BackgroundWorker currBgw = new BackgroundWorker();
                bgwList[currentBgwID] = currBgw;
                currBgw.DoWork += thisWorkerDoWorkHandler;
                currBgw.RunWorkerCompleted += currWorkCompletedHandler;
                currBgw.WorkerReportsProgress = true;
                currBgw.RunWorkerAsync(BGWorker2Args);
            }


            while (bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) > 0)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(100);
            }

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                Environment.NewLine + "==========FINISHED==========" + Environment.NewLine);
        }

        private void btnProcessDirectorySI_Click(object sender, EventArgs e)
        {
            string path2process = (string)defaultProperties["BatchProcessingDirectory"];
            if (path2process == "")
            {
                path2process = Directory.GetCurrentDirectory();
            }

            DirectoryInfo dir = new DirectoryInfo(path2process);

            if (backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
            }
            else
            {
                if (!dir.Exists)
                {
                    ThreadSafeOperations.SetTextTB(textBox1, textBox1.Text + "Операция не выполнена. Не найдена директория:" + Environment.NewLine + path2process + Environment.NewLine, true);
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
                var propertiesObj = Properties.Settings.Default.Properties;
                foreach (SettingsProperty settingsProperty in propertiesObj)
                {
                    strToDisplay += Environment.NewLine + settingsProperty.Name + " = " + settingsProperty.DefaultValue;
                }
                theLogWindow = ServiceTools.LogAText(theLogWindow, strToDisplay);

                simpleMultipleImagesShow imagesRepresentingForm = new simpleMultipleImagesShow();
                imagesRepresentingForm.Show();

                List<ClassificationMethods> schemesToUse = new List<ClassificationMethods>();
                schemesToUse.Add(ClassificationMethods.Japan);


                object[] BGWorker2Args = new object[] { path2process, theLogWindow, imagesRepresentingForm, schemesToUse };

                ThreadSafeOperations.ToggleButtonState(button1, true, "Прекратить обработку", true);

                backgroundWorker2.RunWorkerAsync(BGWorker2Args);
            }
        }


    }
}