using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using SkyImagesAnalyzerLibraries;

namespace IofffeVesselInfoStream
{
    public partial class MeteoDataHistoryPresentationForm : Form
    {
        private LogWindow theLogWindow = null;
        private BackgroundWorker bgwGraphsPresenter = null;
        private static bool showGraphs = false;
        private int graphsUpdatingPeriodSec = 60;
        private static Image<Bgr, byte> graphImage = null;
        private bool defaultGraphsTimeSpan = true;
        private Tuple<DateTime, DateTime> graphsTimeSpan = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow);
        private Tuple<DateTime, DateTime> prevGraphsTimeSpan = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow);
        TimeSeries<MeteoData> tsMeteoDataForGraphs = null;

        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-MeteoDataHistoryPresentation-error.log";


        public MeteoDataHistoryPresentationForm()
        {
            InitializeComponent();
        }




        private void MeteoDataHistoryPresentationForm_Load(object sender, EventArgs e)
        {
            btnToggleShowGraphs_Click(null, null);
            rbtnPressureGraph.Checked = true;
        }





        #region graphs representing

        private System.Threading.Timer timerGraphUpdate;
        private void btnToggleShowGraphs_Click(object sender, EventArgs e)
        {
            showGraphs = !showGraphs;
            ThreadSafeOperations.ToggleButtonState(btnToggleShowGraphs, true, showGraphs ? "ON" : "OFF", true);


            if (showGraphs)
            {
                if (timerGraphUpdate == null)
                {
                    TimerCallback tcbRenderAndShowGraph = RenderAndShowGraph;
                    timerGraphUpdate = new System.Threading.Timer(tcbRenderAndShowGraph, null, 0, graphsUpdatingPeriodSec * 1000);
                }
                else
                {
                    bgwGraphsPresenter.RunWorkerAsync();
                }
                //RenderAndShowGraph(null);
            }
            else
            {
                if (timerGraphUpdate != null)
                {
                    timerGraphUpdate.Dispose();
                }
            }
        }




        private void RenderAndShowGraph(object state)
        {
            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraph, true, true, wcUpdatimgGraph.Color);

            bgwGraphsPresenter = new BackgroundWorker();
            bgwGraphsPresenter.DoWork += bgwGraphsPresenter_DoWork;
            bgwGraphsPresenter.RunWorkerCompleted += bgwGraphsPresenter_RunWorkerCompleted;

            bgwGraphsPresenter.RunWorkerAsync();
        }


        void bgwGraphsPresenter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraph, false, false, wcUpdatimgGraph.Color);
        }




        void bgwGraphsPresenter_DoWork(object sender, DoWorkEventArgs e)
        {

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            graphImage = FillGraphImage(pbGraphs.Size);
            if (graphImage != null)
            {
                ThreadSafeOperations.UpdatePictureBox(pbGraphs, graphImage, false);
            }
        }




        private void pbGraphs_Click(object sender, EventArgs e)
        {
            BackgroundWorker bgwGraphsPresenterInSeparateWindow = new BackgroundWorker();
            bgwGraphsPresenterInSeparateWindow.DoWork += bgwGraphsPresenterInSeparateWindow_DoWork;

            bgwGraphsPresenterInSeparateWindow.RunWorkerAsync();
        }




        void bgwGraphsPresenterInSeparateWindow_DoWork(object sender, DoWorkEventArgs e)
        {
            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraph, true, true, wcUpdatimgGraph.Color);

            Image<Bgr, byte> img = FillGraphImage(new Size(1280, 1024));

            if (img != null)
            {
                ServiceTools.ExecMethodInSeparateThread(this, delegate ()
                {
                    SimpleShowImageForm f1 = new SimpleShowImageForm(img.Bitmap);
                    f1.FormResizing += ExternalGraphImageFormResized;
                    f1.Show();
                });
            }

            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraph, false, false, wcUpdatimgGraph.Color);
        }




        private void ExternalGraphImageFormResized(object sender, EventArgs e)
        {
            BackgroundWorker bgwGenerateGraphImage = new BackgroundWorker();
            SimpleShowImageForm f1 = sender as SimpleShowImageForm;

            bgwGenerateGraphImage.DoWork += (sndr, args) =>
            {
                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraph, true, true, wcUpdatimgGraph.Color);

                Image<Bgr, byte> img = FillGraphImage(f1.pb1.Size);
                args.Result = new object[] { img };
            };

            bgwGenerateGraphImage.RunWorkerCompleted += (sndr, args) =>
            {
                Image<Bgr, byte> resImg = (args.Result as object[])[0] as Image<Bgr, byte>;

                ServiceTools.ExecMethodInSeparateThread(this, delegate ()
                {
                    f1.UpdateBitmap(resImg.Bitmap);
                });

                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGraph, false, false, wcUpdatimgGraph.Color);
            };

            bgwGenerateGraphImage.RunWorkerAsync();
        }




        enum GraphVariablesTypes
        {
            Pressure,
            AirTemp,
            WaterTemp,
            WindSpeed,
            none,
        }



        private DenseVector dvVarValues = DenseVector.Create(1, 0.0d);
        private List<double> currFileSecondsList = new List<double>();
        private MultipleScatterAndFunctionsRepresentation fRenderer = null;
        private Bgr currValueColor = new Bgr(Color.Black);
        private GraphVariablesTypes prevGraphVariable = GraphVariablesTypes.none;
        private int aveMinuteEntriesCount = 100;
        private Image<Bgr, byte> FillGraphImage(Size imgSize)
        {
            string curDirPath = Directory.GetCurrentDirectory() + "\\logs\\";

            DirectoryInfo dirInfo = new DirectoryInfo(curDirPath);

            List<Dictionary<string, object>> lReadData = new List<Dictionary<string, object>>();

            if (defaultGraphsTimeSpan)
            {
                graphsTimeSpan = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            }

            GraphVariablesTypes currVarType = GraphVariablesTypes.none;
            if (rbtnPressureGraph.Checked) currVarType = GraphVariablesTypes.Pressure;
            if (rbtnAirTempGraph.Checked) currVarType = GraphVariablesTypes.AirTemp;
            if (rbtnWaterTempGraph.Checked) currVarType = GraphVariablesTypes.WaterTemp;
            if (rbtnWindSpeedGraph.Checked) currVarType = GraphVariablesTypes.WindSpeed;


            if ((fRenderer == null) || (!prevGraphsTimeSpan.Equals(graphsTimeSpan) || currVarType != prevGraphVariable))
            {
                fRenderer = new MultipleScatterAndFunctionsRepresentation(imgSize);
                switch (currVarType)
                {
                    case GraphVariablesTypes.Pressure:
                        {
                            currValueColor = new Bgr(Color.Blue);
                            break;
                        }
                    case GraphVariablesTypes.AirTemp:
                        {
                            currValueColor = new Bgr(Color.Red);
                            break;
                        }
                    case GraphVariablesTypes.WaterTemp:
                        {
                            currValueColor = new Bgr(Color.RoyalBlue);
                            break;
                        }
                    case GraphVariablesTypes.WindSpeed:
                        {
                            currValueColor = new Bgr(Color.Gray);
                            break;
                        }
                    default:
                        {
                            currValueColor = new Bgr(Color.Blue);
                            break;
                        }


                }
            }
            else if (fRenderer != null)
            {
                if (fRenderer.TheImage.Size != imgSize)
                {
                    fRenderer.ResizeCanvas(imgSize);
                }
            }




            if (!prevGraphsTimeSpan.Equals(graphsTimeSpan))
            {
                IEnumerable<string> ncFileNames = Directory.EnumerateFiles(curDirPath,
                    "IoffeVesselInfoStream-MeteoDataLog-*.nc",
                    SearchOption.TopDirectoryOnly);
                foreach (string ncFileName in ncFileNames)
                {
                    Tuple<DateTime, DateTime> currFileDateTimeRange = null;
                    try
                    {
                        currFileDateTimeRange = ServiceTools.GetNetCDFfileTimeStampsRange(ncFileName);
                    }
                    catch (Exception ex)
                    {
                        #region report

#if DEBUG
                        ServiceTools.ExecMethodInSeparateThread(this, () =>
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                "an exception has been thrown during file reading: " + Environment.NewLine + ncFileName +
                                Environment.NewLine + "message: " + ex.Message + Environment.NewLine +
                                ServiceTools.CurrentCodeLineDescription());
                        });
#else
                        ServiceTools.ExecMethodInSeparateThread(this, () =>
                        {
                            ServiceTools.logToTextFile(errorLogFilename,
                                    "an exception has been thrown during file reading: " + Environment.NewLine + ncFileName +
                                    Environment.NewLine + "message: " + ex.Message + Environment.NewLine +
                                    ServiceTools.CurrentCodeLineDescription(), true, true);
                        });
#endif

                        #endregion report
                    }

                    if (currFileDateTimeRange == null) continue;

                    if ((currFileDateTimeRange.Item1 >= graphsTimeSpan.Item1) &&
                        (currFileDateTimeRange.Item1 <= graphsTimeSpan.Item2) ||
                        (currFileDateTimeRange.Item2 >= graphsTimeSpan.Item1) &&
                        (currFileDateTimeRange.Item2 <= graphsTimeSpan.Item2))
                    {
                        Dictionary<string, object> dictFileData = null;
                        try
                        {
                            dictFileData = NetCDFoperations.ReadDataFromFile(ncFileName);
                        }
                        catch (Exception ex)
                        {
                            #region report

#if DEBUG
                            ServiceTools.ExecMethodInSeparateThread(this, () =>
                            {
                                theLogWindow = ServiceTools.LogAText(theLogWindow,
                                    "an exception has been thrown during file reading: " + Environment.NewLine +
                                    ncFileName +
                                    Environment.NewLine + "message: " + ex.Message + Environment.NewLine +
                                    ServiceTools.CurrentCodeLineDescription());
                            });
#else
                            ServiceTools.ExecMethodInSeparateThread(this, () =>
                            {
                                ServiceTools.logToTextFile(errorLogFilename,
                                    "an exception has been thrown during file reading: " + Environment.NewLine + ncFileName +
                                    Environment.NewLine + "message: " + ex.Message + Environment.NewLine +
                                    ServiceTools.CurrentCodeLineDescription(), true, true);
                            });

#endif

                            #endregion report
                        }

                        if (dictFileData != null)
                        {
                            lReadData.Add(dictFileData);
                        }
                    }
                }


                foreach (Dictionary<string, object> currFileDataDict in lReadData)
                {
                    if (currFileDataDict == null)
                    {
                        continue;
                    }

                    string varNameDateTime = "DateTime";

                    List<long> currFileDateTimeLongTicksList =
                        new List<long>((currFileDataDict[varNameDateTime] as long[]));
                    List<DateTime> currFileDateTimeList =
                        currFileDateTimeLongTicksList.ConvertAll(longVal => new DateTime(longVal));

                    string varNameMeteoData = "MeteoData";
                    List<MeteoData> currFileMeteoDataList =
                        MeteoData.OfDenseMatrix(currFileDataDict[varNameMeteoData] as DenseMatrix);

                    if (tsMeteoDataForGraphs == null)
                    {
                        try
                        {
                            tsMeteoDataForGraphs = new TimeSeries<MeteoData>(currFileMeteoDataList, currFileDateTimeList,
                                true);
                        }
                        catch (Exception ex)
                        {
                            #region report

#if DEBUG
                            ServiceTools.ExecMethodInSeparateThread(this, () =>
                            {
                                theLogWindow = ServiceTools.LogAText(theLogWindow,
                                    "couldn`t create timeseries: exception has been thrown" + Environment.NewLine +
                                    ServiceTools.CurrentCodeLineDescription() + Environment.NewLine + "message: " +
                                    ex.Message);
                            });
#else
                            ServiceTools.ExecMethodInSeparateThread(this, () =>
                            {
                                ServiceTools.logToTextFile(errorLogFilename,
                                    "couldn`t create timeseries: exception has been thrown" + Environment.NewLine +
                                    ServiceTools.CurrentCodeLineDescription() + Environment.NewLine + "message: " +
                                    ex.Message, true, true);
                            });

#endif

                            #endregion report
                        }

                    }
                    else
                    {
                        try
                        {
                            tsMeteoDataForGraphs.AddSubseriaData(currFileMeteoDataList, currFileDateTimeList, true);
                        }
                        catch (Exception ex)
                        {
                            #region report

#if DEBUG
                            ServiceTools.ExecMethodInSeparateThread(this, () =>
                            {
                                theLogWindow = ServiceTools.LogAText(theLogWindow,
                                    "couldn`t create timeseries: exception has been thrown" + Environment.NewLine +
                                    ServiceTools.CurrentCodeLineDescription() + Environment.NewLine + "message: " +
                                    ex.Message);
                            });
#else
                            ServiceTools.ExecMethodInSeparateThread(this, () =>
                            {
                                ServiceTools.logToTextFile(errorLogFilename,
                                    "couldn`t create timeseries: exception has been thrown" + Environment.NewLine +
                                    ServiceTools.CurrentCodeLineDescription() + Environment.NewLine + "message: " +
                                    ex.Message, true, true);
                            });

#endif

                            #endregion report
                        }
                    }
                }



                if (tsMeteoDataForGraphs == null)
                {
                    return null;
                }

                tsMeteoDataForGraphs.SortByTimeStamps();
                tsMeteoDataForGraphs.RemoveDuplicatedTimeStamps();

                DateTime utcNow = DateTime.UtcNow;
                if (defaultGraphsTimeSpan)
                {
                    tsMeteoDataForGraphs.RemoveValues(dt => (utcNow - dt).TotalSeconds > 86400);
                }
                else
                {
                    tsMeteoDataForGraphs.RemoveValues(
                        dt => !((dt >= graphsTimeSpan.Item1) && (dt <= graphsTimeSpan.Item2)));
                }


                List<TimeSeries<MeteoData>> subSeriesBy1Minute =
                    tsMeteoDataForGraphs.SplitByTimeSpan(new TimeSpan(0, 1, 0));
                List<double> lSubSeriesEntriesCount = subSeriesBy1Minute.ConvertAll(subs => (double)subs.Count);
                DescriptiveStatistics statsCounts = new DescriptiveStatistics(lSubSeriesEntriesCount);
                aveMinuteEntriesCount = Convert.ToInt32(statsCounts.Mean);


                // = tsMeteoData.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);
            }



            List<MeteoData> meteoDataList = tsMeteoDataForGraphs.DataValues;
            DateTime maxDateTime = tsMeteoDataForGraphs.TimeStamps.Max();



            if ((currVarType != prevGraphVariable) || !prevGraphsTimeSpan.Equals(graphsTimeSpan))
            {

                double minVarValue = 0.0d;
                double maxVarValue = 1.0d;
                List<double> currVarToShowValues = new List<double>();
                switch (currVarType)
                {
                    case GraphVariablesTypes.Pressure:
                        {
                            currVarToShowValues = meteoDataList.ConvertAll(mdt => mdt.pressure);

                            TimeSeries<double> currVarTS = new TimeSeries<double>(currVarToShowValues,
                                tsMeteoDataForGraphs.TimeStamps);
                            currVarTS.RemoveValues(dVal => dVal <= 900.0d);

                            currVarToShowValues = new List<double>(currVarTS.DataValues);
                            currFileSecondsList = currVarTS.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);

                            fRenderer.yAxisValuesConversionToRepresentTicksValues =
                                new Func<double, string>(dVal => dVal.ToString("F1"));
                            break;
                        }
                    case GraphVariablesTypes.AirTemp:
                        {
                            currVarToShowValues = meteoDataList.ConvertAll(mdt => mdt.airTemperature);

                            TimeSeries<double> currVarTS = new TimeSeries<double>(currVarToShowValues,
                                tsMeteoDataForGraphs.TimeStamps);
                            currVarTS.RemoveValues(dVal => ((dVal < -20.0d) || (dVal > 50.0d)));

                            currVarToShowValues = new List<double>(currVarTS.DataValues);
                            currFileSecondsList = currVarTS.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);

                            fRenderer.yAxisValuesConversionToRepresentTicksValues =
                                new Func<double, string>(dVal => dVal.ToString("F2"));
                            break;
                        }
                    case GraphVariablesTypes.WaterTemp:
                        {
                            currVarToShowValues = meteoDataList.ConvertAll(mdt => mdt.waterTemperature);

                            TimeSeries<double> currVarTS = new TimeSeries<double>(currVarToShowValues,
                                tsMeteoDataForGraphs.TimeStamps);
                            currVarTS.RemoveValues(dVal => ((dVal < -20.0d) || (dVal > 50.0d)));

                            currVarToShowValues = new List<double>(currVarTS.DataValues);
                            currFileSecondsList = currVarTS.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);



                            fRenderer.yAxisValuesConversionToRepresentTicksValues =
                                new Func<double, string>(dVal => dVal.ToString("F2"));
                            break;
                        }
                    case GraphVariablesTypes.WindSpeed:
                        {
                            currVarToShowValues = meteoDataList.ConvertAll(mdt => mdt.windSpeed);

                            TimeSeries<double> currVarTS = new TimeSeries<double>(currVarToShowValues,
                                tsMeteoDataForGraphs.TimeStamps);
                            currVarTS.RemoveValues(dVal => ((dVal < 0.0d) || (dVal > 50.0d)));

                            currVarToShowValues = new List<double>(currVarTS.DataValues);
                            currFileSecondsList = currVarTS.TimeStamps.ConvertAll(dt => (dt - maxDateTime).TotalSeconds);

                            fRenderer.yAxisValuesConversionToRepresentTicksValues =
                                new Func<double, string>(dVal => dVal.ToString("F1"));
                            break;
                        }
                    default:
                        return null;

                }

                dvVarValues = DenseVector.OfEnumerable(currVarToShowValues);
                dvVarValues = dvVarValues.Conv(StandardConvolutionKernels.gauss, aveMinuteEntriesCount * 10);
            }



            fRenderer.dvScatterFuncValues.Add(dvVarValues);
            fRenderer.dvScatterXSpace.Add(DenseVector.OfEnumerable(currFileSecondsList));

            fRenderer.xAxisValuesConversionToRepresentTicksValues = (dValSec) =>
            {
                DateTime currDT = tsMeteoDataForGraphs.TimeStamps.Max().AddSeconds(dValSec);
                return currDT.ToString("yyyy-MM-dd" + Environment.NewLine + "HH:mm");
            };

            fRenderer.scatterLineColors.Add(currValueColor);
            fRenderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);
            fRenderer.xSpaceMin = currFileSecondsList.Min();
            fRenderer.xSpaceMax = currFileSecondsList.Max();
            fRenderer.overallFuncMin = dvVarValues.Min();
            fRenderer.overallFuncMax = dvVarValues.Max();
            fRenderer.fixSpecifiedMargins = true;

            fRenderer.Represent();

            Image<Bgr, byte> retImg = fRenderer.TheImage;

            // расположим надпись
            string strSign = "current value: " + dvVarValues.Last().ToString("F2");

            List<TextBarImage> textBarsCases = new List<TextBarImage>();

            TextBarImage tbimTopLeftSign = new TextBarImage(strSign, retImg);
            tbimTopLeftSign.PtSurroundingBarStart =
                new Point(fRenderer.LeftServiceSpaceGapX + tbimTopLeftSign.textHalfHeight,
                    fRenderer.TopServiceSpaceGapY + tbimTopLeftSign.textHalfHeight);
            textBarsCases.Add(tbimTopLeftSign);

            TextBarImage tbimBtmLeftSign = new TextBarImage(strSign, retImg);
            tbimBtmLeftSign.PtSurroundingBarStart = new Point(fRenderer.LeftServiceSpaceGapX + tbimBtmLeftSign.textHalfHeight,
                retImg.Height - fRenderer.BtmServiceSpaceGapY - tbimBtmLeftSign.textHalfHeight - tbimBtmLeftSign.textHeight * 2);
            textBarsCases.Add(tbimBtmLeftSign);

            TextBarImage tbimTopRightSign = new TextBarImage(strSign, retImg);
            tbimTopRightSign.PtSurroundingBarStart =
                new Point(
                    retImg.Width - fRenderer.RightServiceSpaceGapX - tbimTopRightSign.textHalfHeight -
                    tbimTopRightSign.textBarSize.Width, fRenderer.TopServiceSpaceGapY + tbimTopLeftSign.textHalfHeight);
            textBarsCases.Add(tbimTopRightSign);

            TextBarImage tbimBtmRightSign = new TextBarImage(strSign, retImg);
            tbimBtmRightSign.PtSurroundingBarStart =
                new Point(
                    retImg.Width - fRenderer.RightServiceSpaceGapX - tbimBtmRightSign.textHalfHeight -
                    tbimBtmRightSign.textBarSize.Width,
                    retImg.Height - fRenderer.BtmServiceSpaceGapY - tbimBtmRightSign.textHalfHeight -
                    tbimBtmRightSign.textHeight * 2);
            textBarsCases.Add(tbimBtmRightSign);

            textBarsCases.Sort((case1, case2) => (case1.SubImageInTextRect.CountNonzero().Sum() > case2.SubImageInTextRect.CountNonzero().Sum()) ? 1 : -1);

            MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 2.0d, 2.0d)
            {
                thickness = 2,
            };
            // retImg.Draw(strSign, textBarsCases[0].ptTextBaselineStart, Emgu.CV.CvEnum.FontFace.HersheyPlain, 2.0d, new Bgr(Color.Green), 2);
            retImg.Draw(strSign, ref theFont, textBarsCases[0].ptTextBaselineStart, new Bgr(Color.Green));
            retImg.Draw(textBarsCases[0].rectSurroundingBar, new Bgr(Color.Green), 2);

            prevGraphsTimeSpan = graphsTimeSpan;
            prevGraphVariable = currVarType;

            return retImg;
        }




        private void rbtnGraphVarChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderAndShowGraph(null);
            }
        }




        private void btnGraphMoveFocusLeft_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = false;

            Tuple<DateTime, DateTime> currTimeSpan = graphsTimeSpan;
            TimeSpan currDT = currTimeSpan.Item2 - currTimeSpan.Item1;
            long currDTticks = currDT.Ticks;
            TimeSpan currThirdDT = new TimeSpan(Convert.ToInt64(currDTticks / 3));

            graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1 - currThirdDT, graphsTimeSpan.Item2 - currThirdDT);
            RenderAndShowGraph(null);
        }



        private void btnGraphMoveFocusRight_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = false;

            Tuple<DateTime, DateTime> currTimeSpan = graphsTimeSpan;
            TimeSpan currDT = currTimeSpan.Item2 - currTimeSpan.Item1;
            long currDTticks = currDT.Ticks;
            TimeSpan currThirdDT = new TimeSpan(Convert.ToInt64(currDTticks / 3));

            graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1 + currThirdDT, graphsTimeSpan.Item2 + currThirdDT);
            if (graphsTimeSpan.Item2 > DateTime.UtcNow)
            {
                graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1 + (DateTime.UtcNow - graphsTimeSpan.Item2), DateTime.UtcNow);
            }
            RenderAndShowGraph(null);
        }



        private void btnGraphDefault_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = true;
            graphsTimeSpan = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            RenderAndShowGraph(null);
        }




        private void btnGraphZoomOut_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = false;

            Tuple<DateTime, DateTime> currTimeSpan = graphsTimeSpan;
            TimeSpan currDT = currTimeSpan.Item2 - currTimeSpan.Item1;
            long currDTticks = currDT.Ticks;
            TimeSpan currQuartDT = new TimeSpan(Convert.ToInt64(currDTticks / 4));

            graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1 - currQuartDT,
                graphsTimeSpan.Item2 + currQuartDT);
            if (graphsTimeSpan.Item2 > DateTime.UtcNow)
            {
                graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1, DateTime.UtcNow);
            }
            RenderAndShowGraph(null);
        }



        private void btnGraphZoomIn_Click(object sender, EventArgs e)
        {
            defaultGraphsTimeSpan = false;

            Tuple<DateTime, DateTime> currTimeSpan = graphsTimeSpan;
            TimeSpan currDT = currTimeSpan.Item2 - currTimeSpan.Item1;
            long currDTticks = currDT.Ticks;
            TimeSpan currQuartDT = new TimeSpan(Convert.ToInt64(currDTticks / 4));

            graphsTimeSpan = new Tuple<DateTime, DateTime>(graphsTimeSpan.Item1 + currQuartDT,
                graphsTimeSpan.Item2 - currQuartDT);

            if (graphsTimeSpan.Item1 >= graphsTimeSpan.Item2)
            {
                btnGraphDefault_Click(null, null);
            }
            else
            {
                RenderAndShowGraph(null);
            }
        }



        #endregion graphs representing

        
    }
}
