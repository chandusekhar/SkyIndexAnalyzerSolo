using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Structure;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using SkyImagesAnalyzerLibraries;

namespace SeaTripdataAnalysisApp
{
    public partial class MaindataAnalysisForm : Form
    {
        public string strLogFilesDirectory = "";
        public string strOutputDirectory = "";
        public string strSourceDirectory = "";
        private DenseMatrix dmAccData = null;
        string generalSettingsFilename = Directory.GetCurrentDirectory() + "\\settings\\SeatripDataAnalysisAppGeneralSettings.xml";
        private TimeSeries<double> accSeriaData = new TimeSeries<double>();
        private TimeSeries<GPSdata> gpsSeriaData = new TimeSeries<GPSdata>();
        private List<TimeSeries<double>> accSubseries = new List<TimeSeries<double>>();
        //private List<TimeSeries<GPSdata>> gpsSubseries = new List<TimeSeries<GPSdata>>();
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";



        public MaindataAnalysisForm()
        {
            InitializeComponent();

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
                                         "\\settings\\SeaTripdataAnalysisApp-Settings.xml";

            if (!File.Exists(defaultPropertiesXMLfileName))
            {
                defaultProperties = new Dictionary<string, object>();
                defaultProperties.Add("DefaultExportDestinationDirectory", Directory.GetCurrentDirectory());
                saveDefaultProperties();
            }

            defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);

            string CurDir = Directory.GetCurrentDirectory();
            string currExportDirectoryPath = "";
            if (defaultProperties.ContainsKey("DefaultExportDestinationDirectory"))
            {
                currExportDirectoryPath = (string)defaultProperties["DefaultExportDestinationDirectory"];
            }
            else
            {
                currExportDirectoryPath = CurDir+"\\data-export\\";
            }
            ThreadSafeOperations.SetTextTB(rtbExportDestinationDirectoryPath, currExportDirectoryPath, false);




            if (defaultProperties.ContainsKey("InitialLogFilesDirectory"))
            {
                tbLogFilesPath.Text = (string)defaultProperties["InitialLogFilesDirectory"];
                strLogFilesDirectory = (string)defaultProperties["InitialLogFilesDirectory"];
            }
            else
            {
                defaultProperties.Add("InitialLogFilesDirectory", "");
                saveDefaultProperties();
            }


            if (defaultProperties.ContainsKey("ProcessedDataOutputDirectory"))
            {
                tbOutputDirectoryValue.Text = (string)defaultProperties["ProcessedDataOutputDirectory"];
                strOutputDirectory = (string)defaultProperties["ProcessedDataOutputDirectory"];
            }
            else
            {
                defaultProperties.Add("ProcessedDataOutputDirectory", "");
                saveDefaultProperties();
            }


            if (defaultProperties.ContainsKey("SourceDataDirectory"))
            {
                tbSourceDirectoryValue.Text = (string)defaultProperties["SourceDataDirectory"];
                strSourceDirectory = (string)defaultProperties["SourceDataDirectory"];
            }
            else
            {
                defaultProperties.Add("SourceDataDirectory", "");
                saveDefaultProperties();
            }
        }




        private void MaindataAnalysisForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }


        private BackgroundWorker bgwDataReader;
        private void btnReadData_Click(object sender, EventArgs e)
        {
            strLogFilesDirectory = tbLogFilesPath.Text;

            if (bgwDataReader != null && bgwDataReader.IsBusy)
            {
                bgwDataReader.CancelAsync();
                return;
            }

            ThreadSafeOperations.ToggleButtonState(btnReadData, true, "CANCEL", true);


            DoWorkEventHandler currDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
            {
                BackgroundWorker selfworker = currBGWsender as BackgroundWorker;
                
                List<double> lTotalDataToAdd = new List<double>();
                List<DateTime> lDateTimeList = new List<DateTime>();


                DirectoryInfo dInfo = new DirectoryInfo(strLogFilesDirectory);

                FileInfo[] fInfoArr = dInfo.GetFiles("*AccelerometerDataLog*.nc");

                int fInfoCounter = 0;

                foreach (FileInfo fInfo in fInfoArr)
                {
                    if (selfworker.CancellationPending)
                    {
                        break;
                    }
                    fInfoCounter++;
                    selfworker.ReportProgress(Convert.ToInt32((double)(fInfoCounter - 1) / (double)fInfoArr.Length));

                    ThreadSafeOperations.SetText(lblStatusString, "reading " + fInfo.FullName, false);


                    Dictionary<string, object> dictDataLoaded = NetCDFoperations.ReadDataFromFile(fInfo.FullName);

                    string varNameDateTime = "DateTime";
                    if (dictDataLoaded.Keys.Contains("DateTime"))
                    {
                        varNameDateTime = "DateTime";
                    }
                    else if (dictDataLoaded.Keys.Contains("Datetime"))
                    {
                        varNameDateTime = "Datetime";
                    }
                    List<long> currFileDateTimeLongTicksList = new List<long>((dictDataLoaded[varNameDateTime] as long[]));
                    List<DateTime> currFileDateTimeList = currFileDateTimeLongTicksList.ConvertAll(longVal => new DateTime(longVal));

                    string varNameAccData = "AccelerometerData";
                    List<AccelerometerData> lAccData = AccelerometerData.OfDenseMatrix(dictDataLoaded[varNameAccData] as DenseMatrix);
                    List<double> lAccDataToAdd = lAccData.ConvertAll<double>(acc => acc.AccMagnitude);

                    lTotalDataToAdd.AddRange(lAccDataToAdd);
                    lDateTimeList.AddRange(currFileDateTimeList);

                    selfworker.ReportProgress(Convert.ToInt32((double)(fInfoCounter) / (double)fInfoArr.Length));

                }

                accSeriaData.AddSubseriaData(lTotalDataToAdd, lDateTimeList);




                //теперь обработаем считанные данные
                ThreadSafeOperations.SetText(lblStatusString, "basic acceleration data processing...", false);

                accSubseries = accSeriaData.SplitWithTimeSpanCondition(dt => dt.TotalMilliseconds >= 1200);
                accSubseries.RemoveAll(theSeria => theSeria.TotalSeriaDuration.TotalSeconds < 100);

                List<double> listSeriesStats =
                    accSubseries.ConvertAll(timeseria => timeseria.TotalSeriaDuration.TotalSeconds);
                DescriptiveStatistics stats = new DescriptiveStatistics(listSeriesStats);
                string strToShow = "Acceleration data start time: " + accSubseries[0].StartTime.ToString("s") + Environment.NewLine;
                strToShow += "Acceleration data end time: " + accSubseries[accSubseries.Count-1].EndTime.ToString("s") + Environment.NewLine;
                strToShow += "total chunks count: " + accSubseries.Count + Environment.NewLine;
                strToShow += "mean chunk duration: " + stats.Mean.ToString("0.##e-00") + " s" + Environment.NewLine;
                strToShow += "min chunk duration: " + stats.Minimum.ToString("0.##e-00") + " s" + Environment.NewLine;
                strToShow += "max chunk duration: " + stats.Maximum.ToString("0.##e-00") + " s" + Environment.NewLine;
                strToShow += "StdDev of chunk duration: " + stats.StandardDeviation.ToString("0.##e-00") + " s" + Environment.NewLine;

                ThreadSafeOperations.SetTextTB(tbReportLog, strToShow, true);



                List<GPSdata> lTotalGPSDataToAdd = new List<GPSdata>();
                List<DateTime> lGPSDateTimeList = new List<DateTime>();


                dInfo = new DirectoryInfo(strLogFilesDirectory);

                fInfoArr = dInfo.GetFiles("*GPSDataLog*.nc");

                fInfoCounter = 0;

                foreach (FileInfo fInfo in fInfoArr)
                {
                    if (selfworker.CancellationPending)
                    {
                        break;
                    }
                    fInfoCounter++;
                    selfworker.ReportProgress(Convert.ToInt32((double)(fInfoCounter - 1) / (double)fInfoArr.Length));

                    ThreadSafeOperations.SetText(lblStatusString, "reading " + fInfo.FullName, false);


                    Dictionary<string, object> dictDataLoaded = NetCDFoperations.ReadDataFromFile(fInfo.FullName);

                    string varNameDateTime = "DateTime";
                    if (dictDataLoaded.Keys.Contains("DateTime"))
                    {
                        varNameDateTime = "DateTime";
                    }
                    else if (dictDataLoaded.Keys.Contains("Datetime"))
                    {
                        varNameDateTime = "Datetime";
                    }
                    List<long> currFileDateTimeLongTicksList = new List<long>((dictDataLoaded[varNameDateTime] as long[]));
                    List<DateTime> currFileDateTimeList = currFileDateTimeLongTicksList.ConvertAll(longVal => new DateTime(longVal));

                    string varNameGPSData = "GPSdata";
                    List<GPSdata> lGPSData = GPSdata.OfDenseMatrix(dictDataLoaded[varNameGPSData] as DenseMatrix);
                    //List<double> lGPSDataToAdd = lGPSData.ConvertAll<double>(acc => acc.AccMagnitude);

                    lTotalGPSDataToAdd.AddRange(lGPSData);
                    lGPSDateTimeList.AddRange(currFileDateTimeList);

                    selfworker.ReportProgress(Convert.ToInt32((double)(fInfoCounter) / (double)fInfoArr.Length));

                }

                gpsSeriaData.AddSubseriaData(lTotalGPSDataToAdd, lGPSDateTimeList);

                //теперь обработаем считанные данные
                ThreadSafeOperations.SetText(lblStatusString, "basic GPS data processing...", false);

                gpsSeriaData.RemoveValues(gpsDatum => ((gpsDatum.lat == 0.0d) && (gpsDatum.lon == 0.0d)));

                gpsSeriaData.RemoveDuplicatedTimeStamps();

                strToShow = Environment.NewLine + "GPS data start time: " + gpsSeriaData.StartTime.ToString("s") + Environment.NewLine;
                strToShow += "GPS data end time: " + gpsSeriaData.EndTime.ToString("s") + Environment.NewLine;

                ThreadSafeOperations.SetTextTB(tbReportLog, strToShow, true);
            };

            RunWorkerCompletedEventHandler currWorkCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    ThreadSafeOperations.ToggleButtonState(btnReadData, true, "Read data", true);
                };


            ProgressChangedEventHandler bgwDataReader_ProgressChanged = delegate(object bgwDataReaderSender, ProgressChangedEventArgs args)
                {
                    ThreadSafeOperations.UpdateProgressBar(prbReadingProcessingData, args.ProgressPercentage);
                };



            bgwDataReader = new BackgroundWorker();
            bgwDataReader.WorkerSupportsCancellation = true;
            bgwDataReader.WorkerReportsProgress = true;
            bgwDataReader.DoWork += currDoWorkHandler;
            bgwDataReader.RunWorkerCompleted += currWorkCompletedHandler;
            bgwDataReader.ProgressChanged += bgwDataReader_ProgressChanged;
            object[] BGWargs = new object[] { "", "" };
            bgwDataReader.RunWorkerAsync(BGWargs);
        }














        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dirDialog = new FolderBrowserDialog();
            dirDialog.ShowNewFolderButton = false;
            DialogResult res = dirDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                tbLogFilesPath.Text = dirDialog.SelectedPath;
            }
        }



        


        
        private void MaindataAnalysisForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            saveDefaultProperties();
        }

        private void btnBrowseSourceDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dirDialog = new FolderBrowserDialog();
            dirDialog.ShowNewFolderButton = false;
            DialogResult res = dirDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                tbSourceDirectoryValue.Text = dirDialog.SelectedPath;
                strSourceDirectory = tbSourceDirectoryValue.Text;
            }
        }

        private void btnBrowseOutputDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dirDialog = new FolderBrowserDialog();
            dirDialog.ShowNewFolderButton = false;
            DialogResult res = dirDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                tbOutputDirectoryValue.Text = dirDialog.SelectedPath;
                strOutputDirectory = tbOutputDirectoryValue.Text;
            }
        }

        




        private BackgroundWorker bgwCalculate;
        private void btnProcessAccelerationTimeSeries_Click(object sender, EventArgs e)
        {
            if ((bgwCalculate != null) && (bgwCalculate.IsBusy))
            {
                bgwCalculate.CancelAsync();
                return;
            }

            //simpleMultipleImagesShow imagesRepresentingForm = new simpleMultipleImagesShow();
            //imagesRepresentingForm.Show();



            DoWorkEventHandler bgwCalculate_DoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
            {
                BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;

                //simpleMultipleImagesShow multImagesRepresentingForm = (simpleMultipleImagesShow)((args.Argument as object[])[0]);
                //Type theShowImagesType = multImagesRepresentingForm.GetType();
                //MethodInfo thePicturePlacingMethodInfo = theShowImagesType.GetMethod("PlaceAPicture");

                int imageRepresentingCounter = 0;

                DateTime dbgDT = new DateTime(2014, 7, 9, 9, 0, 0);
                dbgDT = dbgDT.AddMinutes(33);
                DateTime dtSeriesStart = accSubseries[0].StartTime;
                double tsOverallSeriesDurationMillisec = (accSubseries[accSubseries.Count - 1].EndTime - accSubseries[0].StartTime).TotalMilliseconds;

                string strToWrite = " fileName ; lat ; lon ; date ; time ; time(s) since start ; period(s) ; spectrum amplitude";
                ServiceTools.logToTextFile(strOutputDirectory + "\\100sData-spectra-maximums.dat", strToWrite + Environment.NewLine, true);

                foreach (TimeSeries<double> accSubseria in accSubseries)
                {
                    if (selfWorker.CancellationPending)
                    {
                        break;
                    }

                    int startindex = 0;
                    while (true)
                    {
                        int endIndex;

                        if (selfWorker.CancellationPending)
                        {
                            break;
                        }

                        TimeSeries<double> currTimeSeria = accSubseria.SubSeria(startindex, new TimeSpan(1000000000),
                            out endIndex); //100s
                        currTimeSeria = currTimeSeria.InterpolateSeria(new TimeSpan(500000));
                        currTimeSeria = currTimeSeria.ExtractDataDeviationValues();

                        //обработать и оценить наличие выраженных периодов
                        Complex[] sourceSignalArray = currTimeSeria.DataRealValuesComplexArray();
                        Fourier.Forward(sourceSignalArray);
                        //Transform.FourierForward(sourceSignalArray);
                        List<Complex> FourierTransformedSignal = new List<Complex>(sourceSignalArray);
                        List<double> FourierTransformedSignalAmplitudes =
                            FourierTransformedSignal.ConvertAll<double>(
                                cVal =>
                                    ((double.IsNaN(cVal.Magnitude)) || (double.IsInfinity(cVal.Magnitude)))
                                        ? (0.0d)
                                        : (cVal.Magnitude));
                        List<double> FourierTransformedSignalPeriods = new List<double>();
                        for (int ind = 0; ind < FourierTransformedSignalAmplitudes.Count; ind++)
                        {
                            FourierTransformedSignalPeriods.Add(currTimeSeria.TotalSeriaDuration.TotalSeconds /
                                                                (double)ind);
                        }

                        FourierTransformedSignalAmplitudes =
                            new List<double>(
                                FourierTransformedSignalAmplitudes.Zip<double, double, double>(
                                    FourierTransformedSignalPeriods,
                                    (amp, periodSec) =>
                                        ((double.IsNaN(periodSec)) || (double.IsInfinity(periodSec))) ? (0.0d) : (amp)));
                        FourierTransformedSignalPeriods =
                            FourierTransformedSignalPeriods.ConvertAll<double>(
                                dval => ((double.IsNaN(dval)) || (double.IsInfinity(dval))) ? (0.0d) : (dval));


                        //проанализируем этот участок - есть ли выраженные пики по амплитуде конкретных частот

                        //найти максимум в спектре и выдать данные об этом максимуме в файл

                        // сначала отфильтруем периоды меньше 1с - для данных по динамике судна они несущественны
                        FourierTransformedSignalAmplitudes =
                            new List<double>(
                                FourierTransformedSignalAmplitudes.Zip<double, double, double>(
                                    FourierTransformedSignalPeriods,
                                    (amp, periodSec) => (periodSec <= 1.0d) ? (0.0d) : (amp)));
                        FourierTransformedSignalPeriods =
                            FourierTransformedSignalPeriods.ConvertAll<double>(dVal => (dVal <= 1.0d) ? (0.0d) : (dVal));


                        DescriptiveStatistics currAmpsStat =
                            new DescriptiveStatistics(FourierTransformedSignalAmplitudes);
                        List<double> lAmpsOutstanding = FourierTransformedSignalAmplitudes.ConvertAll<double>(dVal =>
                        {
                            if (dVal / currAmpsStat.Mean >= 100.0d)
                            {
                                return dVal;
                            }
                            return 0.0d;
                        });
                        List<double> lPeriodsOutstanding =
                            new List<double>(FourierTransformedSignalPeriods.Zip<double, double, double>(lAmpsOutstanding,
                                (per, amp) => (amp == 0.0d) ? (0.0d) : (per)));


                        if (lAmpsOutstanding.Sum() > 0.0d)
                        {
                            MultipleScatterAndFunctionsRepresentation renderer =
                            new MultipleScatterAndFunctionsRepresentation(2048, 1536);

                            renderer.dvScatterXSpace.Add(currTimeSeria.TimeStampsValuesSeconds);
                            renderer.dvScatterFuncValues.Add(currTimeSeria.dvDoubleDataValues);
                            renderer.scatterLineColors.Add(new Bgr(255, 50, 50));
                            renderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);

                            renderer.dvScatterXSpace.Add(DenseVector.OfEnumerable(FourierTransformedSignalPeriods));
                            renderer.dvScatterFuncValues.Add(DenseVector.OfEnumerable(FourierTransformedSignalAmplitudes));
                            renderer.scatterLineColors.Add(new Bgr(50, 255, 50));
                            renderer.scatterDrawingVariants.Add(SequencesDrawingVariants.squares);

                            renderer.dvScatterXSpace.Add(DenseVector.OfEnumerable(lPeriodsOutstanding));
                            renderer.dvScatterFuncValues.Add(DenseVector.OfEnumerable(lAmpsOutstanding));
                            renderer.scatterLineColors.Add(new Bgr(50, 50, 255));
                            renderer.scatterDrawingVariants.Add(SequencesDrawingVariants.circles);

                            renderer.Represent();

                            if (strOutputDirectory != "")
                            {
                                double maxAmp = lAmpsOutstanding.Max();
                                int idx = lAmpsOutstanding.FindIndex(dval => dval == maxAmp);
                                double maxAmpPeriod = lPeriodsOutstanding[idx];

                                GPSdata gpsMark = gpsSeriaData.GetMostClose(currTimeSeria.StartTime).Item2;

                                string fName = currTimeSeria.StartTime.ToString("s").Replace(":", "-") +
                                               "-100sData-spectrum.jpg";
                                renderer.SaveToImage(strOutputDirectory + "\\" + fName, true);

                                strToWrite = "" + fName + " ; ";
                                strToWrite += gpsMark.LatDec + " ; ";
                                strToWrite += gpsMark.LonDec + " ; ";
                                strToWrite += currTimeSeria.StartTime.Date.ToString("yyyy-MM-dd") + " ; ";
                                strToWrite += currTimeSeria.StartTime.ToString("HH-mm-ss") + " ; ";
                                strToWrite += (currTimeSeria.StartTime - dtSeriesStart).TotalSeconds + " ; ";
                                strToWrite += maxAmpPeriod.ToString() + " ; ";
                                strToWrite += maxAmp.ToString() + " ; ";
                                ServiceTools.logToTextFile(strOutputDirectory + "\\100sData-spectra-maximums.dat", strToWrite + Environment.NewLine, true);

                                ThreadSafeOperations.SetText(lblStatusString, "processing: " + currTimeSeria.StartTime.ToString("s"), false);
                            }
                        }

                        if ((currTimeSeria.StartTime >= dbgDT) || (currTimeSeria.EndTime >= dbgDT))
                        {
                            startindex++;
                            startindex--;
                        }

                        if (endIndex == accSubseria.Count - 1)
                        {
                            break;
                        }

                        Application.DoEvents();

                        selfWorker.ReportProgress(Convert.ToInt32(100.0d * (currTimeSeria.EndTime - dtSeriesStart).TotalMilliseconds / tsOverallSeriesDurationMillisec));

                        startindex += Convert.ToInt32((endIndex - startindex) / 2.0d);
                    }
                }
            };

            RunWorkerCompletedEventHandler bgwCalculate_CompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
            {
                ThreadSafeOperations.ToggleButtonState(btnProcessAccelerationTimeSeries, true, "Process acceleration timeseries", false);
            };

            ProgressChangedEventHandler bgwCalculate_ProgressChanged = delegate(object bgwDataReaderSender, ProgressChangedEventArgs args)
            {
                ThreadSafeOperations.UpdateProgressBar(prbReadingProcessingData, args.ProgressPercentage);
            };



            ThreadSafeOperations.ToggleButtonState(btnProcessAccelerationTimeSeries, true, "STOP", true);

            bgwCalculate = new BackgroundWorker();
            bgwCalculate.WorkerSupportsCancellation = true;
            bgwCalculate.WorkerReportsProgress = true;
            bgwCalculate.DoWork += bgwCalculate_DoWorkHandler;
            bgwCalculate.RunWorkerCompleted += bgwCalculate_CompletedHandler;
            bgwCalculate.ProgressChanged += bgwCalculate_ProgressChanged;
            //object[] bgwCalculateArgs = new object[] { imagesRepresentingForm };
            object[] bgwCalculateArgs = new object[] {  };

            bgwCalculate.RunWorkerAsync(bgwCalculateArgs);
        }





        private BackgroundWorker bgwConvert;
        private void btnConvert_Click(object sender, EventArgs e)
        {
            if ((bgwConvert != null) && (bgwConvert.IsBusy))
            {
                bgwConvert.CancelAsync();
                return;
            }


            DoWorkEventHandler bgwConvert_DoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
            {
                BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;

                DirectoryInfo dInfo = new DirectoryInfo(strLogFilesDirectory);

                FileInfo[] fInfoArr = dInfo.GetFiles("BcstLog-*.log");

                int fInfoCounter = 0;

                foreach (FileInfo fInfo in fInfoArr)
                {
                    if (selfWorker.CancellationPending)
                    {
                        break;
                    }

                    // считать данные, запихнуть в один timeseries

                }

                // записать данные в файлы
            };

            RunWorkerCompletedEventHandler bgwConvert_CompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
            {
                ThreadSafeOperations.ToggleButtonState(btnProcessAccelerationTimeSeries, true, "CONVERT", false);
            };

            ProgressChangedEventHandler bgwConvert_ProgressChanged = delegate(object bgwDataReaderSender, ProgressChangedEventArgs args)
            {
                ThreadSafeOperations.UpdateProgressBar(prbConvertionProgress, args.ProgressPercentage);
            };



            ThreadSafeOperations.ToggleButtonState(btnConvert, true, "STOP", true);

            bgwConvert = new BackgroundWorker();
            bgwConvert.WorkerSupportsCancellation = true;
            bgwConvert.WorkerReportsProgress = true;
            bgwConvert.DoWork += bgwConvert_DoWorkHandler;
            bgwConvert.RunWorkerCompleted += bgwConvert_CompletedHandler;
            bgwConvert.ProgressChanged += bgwConvert_ProgressChanged;
            //object[] bgwCalculateArgs = new object[] { imagesRepresentingForm };
            object[] bgwConvertArgs = new object[] { };

            bgwConvert.RunWorkerAsync(bgwConvertArgs);
        }





        #region Export data

        private void btnSelectExportDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            if (rtbExportDestinationDirectoryPath.Text != "")
            {
                if (Directory.Exists(rtbExportDestinationDirectoryPath.Text))
                {
                    dlg.SelectedPath = rtbExportDestinationDirectoryPath.Text;
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
                rtbExportDestinationDirectoryPath.Text = path;
                if (!defaultProperties.ContainsKey("DefaultExportDestinationDirectory"))
                {
                    defaultProperties.Add("DefaultExportDestinationDirectory", path);
                }
                else
                    defaultProperties["DefaultExportDestinationDirectory"] = path;

                saveDefaultProperties();
            }
        }


        private void btnPerformExport_Click(object sender, EventArgs e)
        {
            string outPath = rtbExportDestinationDirectoryPath.Text;
            outPath += (outPath.Last() == '\\') ? ("") : ("\\");

            if (cbExportMeteoData.Checked)
            {
                //export meteo data

                BackgroundWorker bgwReadMeteoData = new BackgroundWorker();
                bgwReadMeteoData.WorkerSupportsCancellation = false;
                bgwReadMeteoData.WorkerReportsProgress = true;
                bgwReadMeteoData.DoWork += delegate(object currBGWsender, DoWorkEventArgs args)
                {
                    BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                    TimeSeries<MeteoData> tsMeteoData = new TimeSeries<MeteoData>();
                    string[] meteoDataFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\logs\\",
                        "*MeteoDataLog*.nc");
                    int totalFilesCount = meteoDataFiles.Count();
                    int readFiles = 0;
                    foreach (string meteoDataFileName in meteoDataFiles)
                    {
                        ThreadSafeOperations.SetText(lblStatusBar, "reading " + Path.GetFileName(meteoDataFileName), false);

                        Dictionary<string, object> currMeteoFileData =
                            NetCDFoperations.ReadDataFromFile(meteoDataFileName);

                        List<MeteoData> currFileMeteoData =
                            MeteoData.OfDenseMatrix(currMeteoFileData["MeteoData"] as DenseMatrix);
                        List<long> currFileDatetimeLong = new List<long>(currMeteoFileData["DateTime"] as long[]);
                        List<DateTime> currFileDatetime =
                            currFileDatetimeLong.ConvertAll<DateTime>(longDT => new DateTime(longDT));
                        tsMeteoData.AddSubseriaData(currFileMeteoData, currFileDatetime, true);

                        readFiles++;

                        selfWorker.ReportProgress(Convert.ToInt32(100.0d*readFiles/totalFilesCount));
                    }

                    args.Result = new object[] {tsMeteoData};
                };


                bgwReadMeteoData.ProgressChanged += delegate(object currBGWsender, ProgressChangedEventArgs args)
                {
                    ThreadSafeOperations.UpdateProgressBar(prbExportProgress, args.ProgressPercentage);
                };


                bgwReadMeteoData.RunWorkerCompleted += delegate(object currBGWsender, RunWorkerCompletedEventArgs args)
                {
                    ThreadSafeOperations.UpdateProgressBar(prbExportProgress, 0);

                    TimeSeries<MeteoData> tsMeteoData = (args.Result as object[])[0] as TimeSeries<MeteoData>;

                    List<Tuple<DateTime, MeteoData>> lTplMeteoData =
                        tsMeteoData.TimeStamps.Zip(tsMeteoData.DataValues,
                            (dt, dat) => new Tuple<DateTime, MeteoData>(dt, dat)).ToList();

                    if (cbExportFormatCSV.Checked)
                    {
                        List<string> tsMeteoDataCSV = lTplMeteoData.ConvertAll<string>(tpl => tpl.Item1.ToString("o") + "," + tpl.Item2.ToCSV());
                        string tsMeteoDataCSVForFile = String.Join(Environment.NewLine, tsMeteoDataCSV.ToArray<string>());
                        ServiceTools.logToTextFile(outPath + "MeteoData.csv", "DateTime," + lTplMeteoData[0].Item2.CSVHeader() + Environment.NewLine, false, false);
                        ServiceTools.logToTextFile(outPath + "MeteoData.csv", tsMeteoDataCSVForFile, true, false);
                    }

                };



                ThreadSafeOperations.SetText(lblStatusBar, "meteo data read started", false);
                bgwReadMeteoData.RunWorkerAsync();
                

            }
        }


        #endregion


    }
}
