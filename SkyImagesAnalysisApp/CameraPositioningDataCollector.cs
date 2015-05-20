using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Structure;
using IoffeVesselDataReader;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyImagesAnalyzerLibraries;
using SolarPositioning;

namespace SkyImagesAnalyzer
{
    public class CameraPositioningDataCollector
    {
        public LogWindow theLogWindow = null;
        public Dictionary<string, object> defaultProperties { get; set; }
        public MainAnalysisForm ParentForm { get; set; }
        public string defaultPropertiesXMLfileName { get; set; }

        private static ConcurrentBag<AzimuthSunDeviationCalcResult> cbAzimuthSunDeviationCalcResults =
            new ConcurrentBag<AzimuthSunDeviationCalcResult>();

        private List<FileInfo> lImagesFileInfos;

        private BackgroundWorker bgwParallelForEach = null;

        public CameraPositioningDataCollector()
        { }



        public bool IsBusy
        {
            get
            {
                if (bgwParallelForEach == null)
                {
                    return false;
                }
                if (!bgwParallelForEach.IsBusy)
                {
                    return false;
                }

                return true;
            }
        }



        public void StopProcessing()
        {
            if (IsBusy)
            {
                bgwParallelForEach.CancelAsync();
            }
        }


        private List<bool> bgwFinished = new List<bool>();
        public void CollectPositioningData()
        {
            // LogWindow errorsLogWindow = null;

            theLogWindow = ServiceTools.LogAText(theLogWindow, "started on " + DateTime.UtcNow.ToString("s"));


            #region obsolete
            // string strDefaultDataFilesPath = ((string)defaultProperties["BatchProcessingDirectory"]);
            // прочитаем уже посчитанные данные по median и perc5
            //string strStatsDataFilename = ((string)defaultProperties["DefaultMedianPerc5StatsXMLFile"]);

            //if (!File.Exists(strStatsDataFilename))
            //{
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "Операция не выполнена. Не найден файл предварительно посчитанных данных median-perc5:" +
            //        Environment.NewLine + strStatsDataFilename +
            //        Environment.NewLine, true);
            //    return;
            //}
            //List<SkyImageMedianPerc5Data> lStatsData =
            //    (List<SkyImageMedianPerc5Data>)
            //        ServiceTools.ReadObjectFromXML(strStatsDataFilename, typeof(List<SkyImageMedianPerc5Data>));


            //List<SkyImageMedianPerc5Data> lMedianPerc5Data =
            //    (List<SkyImageMedianPerc5Data>)
            //        ServiceTools.ReadObjectFromXML(strStatsDataFilename, typeof(List<SkyImageMedianPerc5Data>));
            //lMedianPerc5Data = lMedianPerc5Data.ConvertAll<SkyImageMedianPerc5Data>(mp5datum =>
            //{
            //    return new SkyImageMedianPerc5Data(Path.GetFileName(mp5datum.FileName).ToLower(), mp5datum.GrIxStatsMedian,
            //        mp5datum.GrIxStatsPerc5);
            //});

            //string strClusteringDataFilesDir = ((string)defaultProperties["ClusteringXMLDataFilesLocation"]);
            //DirectoryInfo clusteringDataDir = new DirectoryInfo(strClusteringDataFilesDir);
            #endregion obsolete

            string strImagesDir = ParentForm.richTextBox1.Text;
            DirectoryInfo imagesDir = new DirectoryInfo(strImagesDir);

            if (!imagesDir.Exists)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Операция не выполнена. Не найдена директория:" + Environment.NewLine + strImagesDir +
                    Environment.NewLine, true);
                return;
            }


            // оказалось, что маска - case-insensitive
            lImagesFileInfos = new List<FileInfo>(imagesDir.GetFiles("*.jpg", SearchOption.TopDirectoryOnly));



            #region obsolete
            //if (!clusteringDataDir.Exists)
            //{
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "Операция не выполнена. Не найдена директория:" + Environment.NewLine + strClusteringDataFilesDir +
            //        Environment.NewLine, true);
            //    return;
            //}


            //FileInfo[] fileListClusteringInfo = clusteringDataDir.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            //if (fileListClusteringInfo.Length == 0)
            //{
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "Операция не выполнена. Не найдены описания кластеризации событий median-perc5 в директории:" +
            //        Environment.NewLine + strClusteringDataFilesDir +
            //        Environment.NewLine, true);
            //    return;
            //}

            //List<ClusteringData> lClusteringinfo = new List<ClusteringData>();
            //List<string> directoriesForSorting = new List<string>();
            //foreach (FileInfo info in fileListClusteringInfo)
            //{
            //    ClusteringData currClusteringData =
            //        (ClusteringData)ServiceTools.ReadObjectFromXML(info.FullName, typeof(ClusteringData));
            //    lClusteringinfo.Add(currClusteringData);
            //    string currClassDirName = info.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(info.FullName) + "\\";
            //    if (!ServiceTools.CheckIfDirectoryExists(currClassDirName))
            //    {
            //        continue;
            //    }
            //    directoriesForSorting.Add(currClassDirName);
            //}

            //Image<Bgr, byte> imgShowingClusterTemplate =
            //        new Image<Bgr, byte>(lClusteringinfo[0].rctOperationalAreaOnGrid.Width,
            //            lClusteringinfo[0].rctOperationalAreaOnGrid.Height);
            //RandomPastelColorGenerator colorGen = new RandomPastelColorGenerator();
            //foreach (ClusteringData currClusteringData in lClusteringinfo)
            //{
            //    Bgr currColor = new Bgr(colorGen.GetNext());
            //    foreach (Point pt in currClusteringData.lOverallPoints)
            //    {
            //        if (conditionOnPoints(
            //                pt.FlipUpsideDown(new Size(currClusteringData.rctOperationalAreaOnGrid.Width,
            //                    currClusteringData.rctOperationalAreaOnGrid.Height)))) continue;

            //        if (dmDensityField[pt.Y, pt.X] < dDensityMinThreshold)
            //        {
            //            continue;
            //        }

            //        imgShowingClusterTemplate[pt.Y, pt.X] = currColor;
            //    }
            //}
            ////imgShowingClusterTemplate = imgShowingClusterTemplate*0.5d;
            //foreach (ClusteringData currClusteringData in lClusteringinfo)
            //{
            //    Image<Bgr, byte> imgShowingCluster = imgShowingClusterTemplate.Copy();
            //    foreach (Point pt in currClusteringData.lOverallPoints)
            //    {
            //        if (conditionOnPoints(
            //                pt.FlipUpsideDown(new Size(currClusteringData.rctOperationalAreaOnGrid.Width,
            //                    currClusteringData.rctOperationalAreaOnGrid.Height)))) continue;

            //        if (dmDensityField[pt.Y, pt.X] < dDensityMinThreshold)
            //        {
            //            continue;
            //        }

            //        imgShowingCluster[pt.Y, pt.X] = new Bgr(255, 255, 255);
            //    }
            //    imgShowingCluster.Save(directoriesForSorting[lClusteringinfo.IndexOf(currClusteringData)] +
            //                           "cluster-representation.jpg");
            //}


            //List<PointD> pointsRealSpace = lData.ConvertAll<PointD>(statsDatum => new PointD(statsDatum.GrIxStatsMedian, statsDatum.GrIxStatsPerc5));

            //double minXval = pointsRealSpace.Min(pt => pt.X);
            //double maxXval = pointsRealSpace.Max(pt => pt.X);
            //double minYval = pointsRealSpace.Min(pt => pt.Y);
            //double maxYval = pointsRealSpace.Max(pt => pt.Y);
            //minXval = Math.Min(minXval, minYval);
            //minYval = minXval;
            //maxXval = Math.Max(maxXval, maxYval);
            //maxYval = maxXval;

            //int spaceDiscretization = lClusteringinfo[0].rctOperationalAreaOnGrid.Width;
            //double xSpaceDiscrete = (maxXval - minXval) / spaceDiscretization;
            //double ySpaceDiscrete = (maxYval - minYval) / spaceDiscretization;
            #endregion obsolete



            //List<bool> bgwFinished = new List<bool>();
            List<BackgroundWorker> bgwList = new List<BackgroundWorker>();
            for (int i = 0; i < 2; i++)
            {
                bgwFinished.Add(true);
                bgwList.Add(null);
            }

            int currDataIdx = 1;

            foreach (FileInfo finfo in lImagesFileInfos)
            {
                int currentBgwID = -1;
                while (bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) == bgwFinished.Count)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
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

                theLogWindow = ServiceTools.LogAText(theLogWindow, "" + currDataIdx + " / " + lImagesFileInfos.Count);
                theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "starting: " + finfo.Name);


                object[] BGWorker2Args = new object[] { finfo, defaultProperties, currentBgwID };

                BackgroundWorker currBgw = new BackgroundWorker();
                bgwList[currentBgwID] = currBgw;
                currBgw.DoWork += currBgw_DoWork;
                currBgw.RunWorkerCompleted += currBgw_RunWorkerCompleted;
                currBgw.RunWorkerAsync(BGWorker2Args);

                int progress = Convert.ToInt32(100.0d * (double)currDataIdx / (double)lImagesFileInfos.Count);
                ThreadSafeOperations.UpdateProgressBar(ParentForm.pbUniversalProgressBar, progress);
                Interlocked.Increment(ref currDataIdx);
            }


            while (bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) > 0)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }





            
            //List<AzimuthSunDeviationCalcResult> lAzimuthSunDeviationCalcResults =
            //    result[0] as List<AzimuthSunDeviationCalcResult>;

            // сохраним на будущее
            string strMedianPerc5StatsXMLFileName = (string)defaultProperties["DefaultMedianPerc5StatsXMLFile"];
            FileInfo MedianPerc5StatsXMLFileInfo = new FileInfo(strMedianPerc5StatsXMLFileName);
            string strMedianPerc5StatsXMLFilePath = MedianPerc5StatsXMLFileInfo.DirectoryName;
            strMedianPerc5StatsXMLFilePath += (strMedianPerc5StatsXMLFilePath.Last() == '\\') ? ("") : ("\\");
            string computedDeviationsXMLfilesPath = strMedianPerc5StatsXMLFilePath + "azimuth-dev-stats\\";
            if (!ServiceTools.CheckIfDirectoryExists(computedDeviationsXMLfilesPath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "couldn`t locate or create directory " + computedDeviationsXMLfilesPath);
                return;
            }

            string computedDeviationsXMLFile = computedDeviationsXMLfilesPath + "PreComputedAzimuthDeviationsData.xml";

            List<AzimuthSunDeviationCalcResult> lResList =
                new List<AzimuthSunDeviationCalcResult>(cbAzimuthSunDeviationCalcResults);
            //List<object> lAzimuthSunDeviationCalcResultsObj =
            //    lResList.ConvertAll<object>(
            //        azimuthSunDeviationCalcResult => (object)azimuthSunDeviationCalcResult);
            ServiceTools.WriteObjectToXML(lResList, computedDeviationsXMLFile);
            //ServiceTools.WriteListToXml(lAzimuthSunDeviationCalcResultsObj, computedDeviationsXMLFile, false);
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);


            // теперь посчитаем статистику
            List<double> azimuthDevValues =
                lResList.ConvertAll<double>(
                    azimuthSunDeviationCalcResult => azimuthSunDeviationCalcResult.computedAzimuthDeviation);


            ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate()
            {
                HistogramDataAndProperties histData =
                    new HistogramDataAndProperties(DenseVector.OfEnumerable(azimuthDevValues), 100);
                HistogramCalcAndShowForm hForm = new HistogramCalcAndShowForm("azimuth sun angle deviations",
                    defaultProperties);
                hForm.HistToRepresent = histData;
                hForm.Show();
                hForm.Represent();
            });


            ThreadSafeOperations.UpdateProgressBar(ParentForm.pbUniversalProgressBar, 0);





            //bgwParallelForEach = new BackgroundWorker();
            //bgwParallelForEach.DoWork += bgwParallelForEach_DoWork;
            //bgwParallelForEach.RunWorkerCompleted += bgwParallelForEach_RunWorkerCompleted;
            //bgwParallelForEach.WorkerReportsProgress = true;
            //bgwParallelForEach.ProgressChanged += bgwParallelForEach_ProgressChanged;
            //bgwParallelForEach.WorkerSupportsCancellation = true;
            //object[] bgwArgs = new object[]
            //{
            //    lImagesFileInfos,
            //    theLogWindow,
            //};
            //bgwParallelForEach.RunWorkerAsync(bgwArgs);

        }





        void currBgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = e.Result as object[];
            int currentBgwID = (int) result[0];
            bgwFinished[currentBgwID] = true;
            //currentBgwID
        }






        void currBgw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;

            // BGWorker2Args = { finfo, defaultProperties, currentBgwID };
            FileInfo finfo = ((object[])e.Argument)[0] as FileInfo;
            Dictionary<string, object> defaultProperties = ((object[]) e.Argument)[1] as Dictionary<string, object>;
            int currentBgwID = (int) ((object[]) e.Argument)[2];
            LogWindow currImageLogWindow = null;

            try
            {
                if (finfo == null)
                {
                    e.Result = false;
                    return;
                }

                currImageLogWindow = ServiceTools.LogAText(currImageLogWindow, Environment.NewLine + "starting: " + finfo.Name);

                currImageLogWindow = ServiceTools.LogAText(currImageLogWindow, "start processing image " + finfo.Name);

                AzimuthSunDeviationCalcResult devCalcResDatum = CalculateDevDataForImage(finfo, defaultProperties, currImageLogWindow);

                if (devCalcResDatum.calculationSucceeded)
                {
                    cbAzimuthSunDeviationCalcResults.Add(devCalcResDatum);
                }

                currImageLogWindow = ServiceTools.LogAText(currImageLogWindow,
                    Environment.NewLine + "finished: " + Path.GetFileName(devCalcResDatum.fileName) +
                    " with result: " + devCalcResDatum.calculationSucceeded + Environment.NewLine +
                    devCalcResDatum.resultMessage);


                currImageLogWindow.Close();
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "ERROR. There exceptions has been thrown: " + Environment.NewLine + ex.Message);
            }


            e.Result = new object[] { currentBgwID };
        }








        private void DoTaskForEachFileInfo()
        {
            //List<FileInfo> lImagesFileInfos = ((object[])e.Argument)[0] as List<FileInfo>;
            ConcurrentQueue<FileInfo> cqImagesFileInfos = new ConcurrentQueue<FileInfo>(lImagesFileInfos);
            ConcurrentBag<FileInfo> cbProcessedFileInfos = new ConcurrentBag<FileInfo>();
            //List<AzimuthSunDeviationCalcResult> lAzimuthSunDeviationCalcResults =
            //    ((object[]) e.Argument)[1] as List<AzimuthSunDeviationCalcResult>;
            //LogWindow theLogWindow = ((object[])e.Argument)[1] as LogWindow;

            int filesCount = cqImagesFileInfos.Count;

            Parallel.For((long)0, filesCount, new ParallelOptions { MaxDegreeOfParallelism = 4 },
                counter =>
                {
                    FileInfo finfo = null;
                    if (cqImagesFileInfos.Count > 0)
                    {
                        while (!cqImagesFileInfos.TryDequeue(out finfo))
                        {
                            Application.DoEvents();
                            Thread.Sleep(0);
                            if (cqImagesFileInfos.Count == 0)
                            {
                                break;
                            }
                        }
                    }

                    if (finfo == null)
                    {
                        return;
                    }

                    //LogWindow thisFileLogWindow = null;
                    //thisFileLogWindow = ServiceTools.LogAText(thisFileLogWindow, Environment.NewLine + "starting: " + finfo.Name);
                    theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "starting: " + finfo.Name);
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "" + (counter + 1).ToString() + " / " + lImagesFileInfos.Count);

                    AzimuthSunDeviationCalcResult devCalcResDatum = CalculateDevDataForImage(finfo, defaultProperties, theLogWindow);

                    cbProcessedFileInfos.Add(finfo);

                    if (devCalcResDatum.calculationSucceeded)
                    {
                        cbAzimuthSunDeviationCalcResults.Add(devCalcResDatum);
                    }

                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        Environment.NewLine + "finished: " + Path.GetFileName(devCalcResDatum.fileName) +
                        " with result: " + devCalcResDatum.calculationSucceeded + Environment.NewLine +
                        devCalcResDatum.resultMessage);


                    try
                    {
                        int filesProcessed = cbProcessedFileInfos.Count;
                        int progress = Convert.ToInt32(100.0d * (double)filesProcessed / (double)filesCount);
                        ThreadSafeOperations.UpdateProgressBar(ParentForm.pbUniversalProgressBar, progress);
                    }
                    catch (Exception ex)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, ex.Message);
                        return;
                    }

                    Application.DoEvents();

                    //thisFileLogWindow.Close();
                });


            Application.DoEvents();


            string strMedianPerc5StatsXMLFileName = (string)defaultProperties["DefaultMedianPerc5StatsXMLFile"];
            FileInfo MedianPerc5StatsXMLFileInfo = new FileInfo(strMedianPerc5StatsXMLFileName);
            string strMedianPerc5StatsXMLFilePath = MedianPerc5StatsXMLFileInfo.DirectoryName;
            strMedianPerc5StatsXMLFilePath += (strMedianPerc5StatsXMLFilePath.Last() == '\\') ? ("") : ("\\");
            string computedDeviationsXMLfilesPath = strMedianPerc5StatsXMLFilePath + "azimuth-dev-stats\\";
            if (!ServiceTools.CheckIfDirectoryExists(computedDeviationsXMLfilesPath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "couldn`t locate or create directory " + computedDeviationsXMLfilesPath);
                return;
            }

            string computedDeviationsXMLFile = strMedianPerc5StatsXMLFilePath + "PreComputedAzimuthDeviationsData.xml";

            List<AzimuthSunDeviationCalcResult> lResList =
                new List<AzimuthSunDeviationCalcResult>(cbAzimuthSunDeviationCalcResults);
            List<object> lAzimuthSunDeviationCalcResultsObj =
                lResList.ConvertAll<object>(
                    azimuthSunDeviationCalcResult => (object)azimuthSunDeviationCalcResult);
            ServiceTools.WriteListToXml(lAzimuthSunDeviationCalcResultsObj, computedDeviationsXMLFile, false);
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);


            // теперь посчитаем статистику
            List<double> azimuthDevValues =
                lResList.ConvertAll<double>(
                    azimuthSunDeviationCalcResult => azimuthSunDeviationCalcResult.computedAzimuthDeviation);


            ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate()
            {
                HistogramDataAndProperties histData =
                    new HistogramDataAndProperties(DenseVector.OfEnumerable(azimuthDevValues), 100);
                HistogramCalcAndShowForm hForm = new HistogramCalcAndShowForm("azimuth sun angle deviations",
                    defaultProperties);
                hForm.HistToRepresent = histData;
                hForm.Show();
                hForm.Represent();
            });


            ThreadSafeOperations.UpdateProgressBar(ParentForm.pbUniversalProgressBar, 0);
        }





        void bgwParallelForEach_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            List<FileInfo> lImagesFileInfos = ((object[])e.Argument)[0] as List<FileInfo>;
            ConcurrentQueue<FileInfo> cqImagesFileInfos = new ConcurrentQueue<FileInfo>(lImagesFileInfos);
            ConcurrentBag<FileInfo> cbProcessedFileInfos = new ConcurrentBag<FileInfo>();
            //List<AzimuthSunDeviationCalcResult> lAzimuthSunDeviationCalcResults =
            //    ((object[]) e.Argument)[1] as List<AzimuthSunDeviationCalcResult>;
            LogWindow theLogWindow = ((object[])e.Argument)[1] as LogWindow;

            int filesCount = cqImagesFileInfos.Count;

            //int currDataIdx = 1;
            //while (cqImagesFileInfos.Any())
            //{
            //
            //}

            try
            {
                int parallelismDegree = Convert.ToInt32(System.Environment.ProcessorCount / 2);
                if (parallelismDegree < 1) parallelismDegree = 1;

                Parallel.For((long)1, filesCount + 1, new ParallelOptions { MaxDegreeOfParallelism = 1 },
                    (counter, loopState) =>
                    {
                        if (selfWorker.CancellationPending)
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow,
                                "============== breaking images processing ==============" +
                                Environment.NewLine + "=== You should wait for all images processing to finish ===");
                            loopState.Break();
                        }

                        FileInfo finfo = null;
                        if (cqImagesFileInfos.Count > 0)
                        {
                            while (!cqImagesFileInfos.TryDequeue(out finfo))
                            {
                                Application.DoEvents();
                                Thread.Sleep(0);
                                if (cqImagesFileInfos.Count == 0)
                                {
                                    break;
                                }
                            }
                        }

                        if (finfo == null)
                        {
                            e.Result = false;
                            return;
                        }

                        theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "starting: " + finfo.Name);
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "" + counter + " / " + lImagesFileInfos.Count);

                        LogWindow imageLogWindow = null;
                        imageLogWindow = ServiceTools.LogAText(imageLogWindow, "start processing image " + finfo.Name);

                        AzimuthSunDeviationCalcResult devCalcResDatum = CalculateDevDataForImage(finfo, defaultProperties, imageLogWindow);

                        cbProcessedFileInfos.Add(finfo);

                        if (devCalcResDatum.calculationSucceeded)
                        {
                            cbAzimuthSunDeviationCalcResults.Add(devCalcResDatum);
                        }

                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            Environment.NewLine + "finished: " + Path.GetFileName(devCalcResDatum.fileName) +
                            " with result: " + devCalcResDatum.calculationSucceeded + Environment.NewLine +
                            devCalcResDatum.resultMessage);


                        imageLogWindow.Close();

                        try
                        {
                            int filesProcessed = cbProcessedFileInfos.Count;
                            int progress = Convert.ToInt32(100.0d * (double)filesProcessed / (double)filesCount);
                            selfWorker.ReportProgress(progress);
                        }
                        catch (Exception ex)
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow, ex.Message);
                            return;
                        }
                    });
            }
            catch (AggregateException ex)
            {
                string messages = "";
                foreach (Exception innerException in ex.InnerExceptions)
                {
                    messages += innerException.Message + Environment.NewLine;
                }
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "ERROR. There exceptions has been thrown: " + Environment.NewLine + messages);

            }


            e.Result = true;
        }







        void bgwParallelForEach_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ThreadSafeOperations.UpdateProgressBar(ParentForm.pbUniversalProgressBar, e.ProgressPercentage);
        }






        void bgwParallelForEach_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = e.Result as object[];
            //List<AzimuthSunDeviationCalcResult> lAzimuthSunDeviationCalcResults =
            //    result[0] as List<AzimuthSunDeviationCalcResult>;

            // сохраним на будущее
            string strMedianPerc5StatsXMLFileName = (string)defaultProperties["DefaultMedianPerc5StatsXMLFile"];
            FileInfo MedianPerc5StatsXMLFileInfo = new FileInfo(strMedianPerc5StatsXMLFileName);
            string strMedianPerc5StatsXMLFilePath = MedianPerc5StatsXMLFileInfo.DirectoryName;
            strMedianPerc5StatsXMLFilePath += (strMedianPerc5StatsXMLFilePath.Last() == '\\') ? ("") : ("\\");
            string computedDeviationsXMLfilesPath = strMedianPerc5StatsXMLFilePath + "azimuth-dev-stats\\";
            if (!ServiceTools.CheckIfDirectoryExists(computedDeviationsXMLfilesPath))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "couldn`t locate or create directory " + computedDeviationsXMLfilesPath);
                return;
            }

            string computedDeviationsXMLFile = computedDeviationsXMLfilesPath + "PreComputedAzimuthDeviationsData.xml";

            List<AzimuthSunDeviationCalcResult> lResList =
                new List<AzimuthSunDeviationCalcResult>(cbAzimuthSunDeviationCalcResults);
            //List<object> lAzimuthSunDeviationCalcResultsObj =
            //    lResList.ConvertAll<object>(
            //        azimuthSunDeviationCalcResult => (object)azimuthSunDeviationCalcResult);
            ServiceTools.WriteObjectToXML(lResList, computedDeviationsXMLFile);
            //ServiceTools.WriteListToXml(lAzimuthSunDeviationCalcResultsObj, computedDeviationsXMLFile, false);
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);


            // теперь посчитаем статистику
            List<double> azimuthDevValues =
                lResList.ConvertAll<double>(
                    azimuthSunDeviationCalcResult => azimuthSunDeviationCalcResult.computedAzimuthDeviation);


            ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate()
            {
                HistogramDataAndProperties histData =
                    new HistogramDataAndProperties(DenseVector.OfEnumerable(azimuthDevValues), 100);
                HistogramCalcAndShowForm hForm = new HistogramCalcAndShowForm("azimuth sun angle deviations",
                    defaultProperties);
                hForm.HistToRepresent = histData;
                hForm.Show();
                hForm.Represent();
            });


            ThreadSafeOperations.UpdateProgressBar(ParentForm.pbUniversalProgressBar, 0);
        }












        // эта функция будет оценивать расположение солнечного диска
        // определять направление на его центр
        // определять азимутальный солнца угол по:
        // 1. данным GPS корабельной навигации
        // 2. корректировке по хедингу судна по данным корабельной навигации
        // вычислять азимутальное отклонение расположения солнечного диска на снимке
        // от ожидаемого положения
        private AzimuthSunDeviationCalcResult CalculateDevDataForImage(FileInfo finfo, Dictionary<string, object> defaultProperties, LogWindow currImageLogWindow)
        {
            FileInfo currFileInfo = finfo;
            Dictionary<string, object> defaultProps = defaultProperties;


            AzimuthSunDeviationCalcResult retRes = new AzimuthSunDeviationCalcResult()
            {
                fileName = currFileInfo.FullName,
            };

            // определяем дату-время файла
            DateTime curDateTime = DateTime.UtcNow;

            Image anImage = Image.FromFile(currFileInfo.FullName);
            ImageInfo newIInfo = new ImageInfo(anImage);
            int minute = 0;
            String dateTime = (String)newIInfo.getValueByKey("ExifDTOrig");
            if (dateTime == null)
            {
                //попробуем вытащить из имени файла
                string strDateTime = currFileInfo.Name;
                strDateTime = strDateTime.Substring(4, 19);
                dateTime = strDateTime;
            }

            try
            {
                curDateTime = CommonTools.DateTimeOfString(dateTime);
            }
            catch (Exception)
            {
                retRes.calculationSucceeded = false;
                retRes.resultMessage = "couldn`t get date/time for file: " + Environment.NewLine + currFileInfo.Name;
                return retRes;
            }
            curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);

            GPSdata neededGPSdata = new GPSdata();
            string currPath = currFileInfo.DirectoryName;


            string navFilesPath = defaultProps["IoffeMeteoNavFilesDirectory"] as string;
            List<IoffeVesselDualNavDataConverted> lAllNavData = new List<IoffeVesselDualNavDataConverted>();

            string[] sNavFilenames = Directory.GetFiles(navFilesPath, "*.nv2");
            if (!sNavFilenames.Any())
            {
                retRes.calculationSucceeded = false;
                retRes.resultMessage = "Не найдено файлов данных навигации в директории " + navFilesPath;
                return retRes;
            }
            else
            {
                foreach (string navFilename in sNavFilenames)
                {
                    Tuple<DateTime, DateTime> timeSpan =
                        IoffeVesselNavDataReader.GetNavFileDateTimeMargins(navFilename);
                    if (timeSpan == null)
                    {
                        continue;
                    }

                    if ((curDateTime < timeSpan.Item1) || (curDateTime > timeSpan.Item2))
                    {
                        continue;
                    }

                    List<IoffeVesselDualNavDataConverted> dataHasBeenRead = IoffeVesselNavDataReader.ReadNavFile(navFilename);
                    if (dataHasBeenRead == null)
                    {
                        continue;
                    }
                    Application.DoEvents();
                    lAllNavData.AddRange(dataHasBeenRead);
                }
            }

            lAllNavData.Sort((gpsRecord1, gpsRecord2) =>
            {
                double dev1 = Math.Abs((gpsRecord1.gps.dateTimeUTC - curDateTime).TotalMilliseconds);
                double dev2 = Math.Abs((gpsRecord2.gps.dateTimeUTC - curDateTime).TotalMilliseconds);
                return (dev1 >= dev2) ? (1) : (-1);
            });
            neededGPSdata = lAllNavData[0].gps;

            retRes.gpsData = neededGPSdata;

            double lat = neededGPSdata.LatDec;
            double lon = neededGPSdata.LonDec;


            SPA spaCalc = new SPA(curDateTime.Year, curDateTime.Month, curDateTime.Day, curDateTime.Hour,
                curDateTime.Minute, curDateTime.Second, (float)lon, (float)lat,
                (float)SPAConst.DeltaT(curDateTime));
            int res = spaCalc.spa_calculate();
            AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                spaCalc.spa.zenith);
            currImageLogWindow = ServiceTools.LogAText(currImageLogWindow,
                "SPA ext sun position for " + curDateTime.ToString("s") + ": " + sunPositionSPAext);
            retRes.sunSPAcomputedPosition = sunPositionSPAext;



            Image<Bgr, Byte> img2process = new Image<Bgr, byte>(currFileInfo.FullName);
            img2process = ImageProcessing.ImageResizer(img2process, Convert.ToInt32(defaultProps["DefaultMaxImageSize"]));
            Image<Bgr, Byte> LocalProcessingImage = ImageProcessing.SquareImageDimensions(img2process);

            RoundData sunRoundData = RoundData.nullRoundData();

            //посмотрим, нет ли уже имеющихся данных о положении и размере солнечного диска на изображении
            string sunDiskInfoFileName = currFileInfo.DirectoryName + "\\" +
                                         Path.GetFileNameWithoutExtension(currFileInfo.FullName) + "-SunDiskInfo.xml";

            RoundData existingRoundData = RoundData.nullRoundData();
            Size imgSizeUnderExistingRoundData = LocalProcessingImage.Bitmap.Size;
            object existingRoundDataObj = ServiceTools.ReadObjectFromXML(sunDiskInfoFileName, typeof(RoundDataWithUnderlyingImgSize));

            if (existingRoundDataObj != null)
            {
                existingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).circle;
                imgSizeUnderExistingRoundData = ((RoundDataWithUnderlyingImgSize)existingRoundDataObj).imgSize;
            }

            double currScale = (double)LocalProcessingImage.Width / (double)imgSizeUnderExistingRoundData.Width;
            if (currScale != 1.0d)
            {
                existingRoundData.DCenterX *= currScale;
                existingRoundData.DCenterY *= currScale;
                existingRoundData.DRadius *= currScale;
            }
            if (!existingRoundData.IsNull)
            {
                sunRoundData = existingRoundData;
            }

            ImageProcessing imgP = new ImageProcessing(LocalProcessingImage, true);

            if (sunRoundData.IsNull)
            {
                SkyCloudClassification classificator = new SkyCloudClassification(img2process, defaultProperties);
                classificator.verbosityLevel = 0;
                classificator.ParentForm = ParentForm;
                classificator.theLogWindow = currImageLogWindow;
                classificator.ClassificationMethod = ClassificationMethods.GrIx;
                classificator.isCalculatingUsingBgWorker = false;
                // classificator.SelfWorker = currBGWsender as BackgroundWorker;
                classificator.defaultOutputDataDirectory = (string)defaultProps["DefaultDataFilesLocation"];
                classificator.theStdDevMarginValueDefiningSkyCloudSeparation =
                    Convert.ToDouble(defaultProps["GrIxDefaultSkyCloudMarginWithoutSun"]);
                classificator.sourceImageFileName = currFileInfo.FullName;

                retRes.imageEdgesDetected = new RoundDataWithUnderlyingImgSize()
                {
                    circle = imgP.imageRD,
                    imgSize = LocalProcessingImage.Size,
                };
                DenseMatrix dmProcessingData = (DenseMatrix)imgP.eval("grix").Clone();
                try
                {
                    sunRoundData = classificator.DetectSunWithSerieOfArcs(imgP, dmProcessingData);
                    if (!sunRoundData.IsNull)
                    {
                        RoundDataWithUnderlyingImgSize infoToSave = new RoundDataWithUnderlyingImgSize()
                        {
                            circle = sunRoundData,
                            imgSize = LocalProcessingImage.Size,
                        };
                        ServiceTools.WriteObjectToXML(infoToSave, sunDiskInfoFileName);
                    }
                }
                catch (Exception ex)
                {
                    retRes.calculationSucceeded = false;
                    retRes.resultMessage = ex.Message;
                    return retRes;
                }
                ServiceTools.FlushMemory();
            }



            if (sunRoundData.IsNull)
            {
                throw new Exception("couldn`t detect sun position");
            }
            else
            {
                retRes.sunDiskDetectedPosition = new RoundDataWithUnderlyingImgSize()
                {
                    circle = sunRoundData,
                    imgSize = LocalProcessingImage.Size,
                };
            }




            RoundData imageDetectedRound = imgP.imageRD.Copy();
            retRes.imageEdgesDetected = new RoundDataWithUnderlyingImgSize()
            {
                circle = imageDetectedRound,
                imgSize = LocalProcessingImage.Size,
            };

            try
            {
                double dev = retRes.computedAzimuthDeviation;
                retRes.calculationSucceeded = true;
            }
            catch (Exception ex)
            {
                retRes.calculationSucceeded = false;
                retRes.resultMessage = ex.Message;
                return retRes;
            }

            return retRes;
        }



    }






    [Serializable]
    public class AzimuthSunDeviationCalcResult
    {
        public string fileName = "";
        public bool calculationSucceeded = false;

        [XmlIgnore]
        public string resultMessage = "";

        public GPSdata gpsData = new GPSdata()
        {
            validGPSdata = false,
        };

        public RoundDataWithUnderlyingImgSize imageEdgesDetected;
        public RoundDataWithUnderlyingImgSize sunDiskDetectedPosition;

        public double detectedSunPositionAzimuth
        {
            get
            {
                if ((sunDiskDetectedPosition.circle != null) && (imageEdgesDetected.circle != null))
                {
                    PointD ptdImageNorth = new PointD(imageEdgesDetected.circle.DCenterX, 0.0d);
                    PointD ptdImageNorthRelativeToCenter = ptdImageNorth -
                                                           imageEdgesDetected.circle.pointDCircleCenter();
                    ptdImageNorthRelativeToCenter.Y = -ptdImageNorthRelativeToCenter.Y;
                    PointPolar ptpImageNorth = new PointPolar(ptdImageNorthRelativeToCenter, true);
                    PointD ptdSunCenterRelativeToCenter = sunDiskDetectedPosition.circle.pointDCircleCenter() -
                                                          imageEdgesDetected.circle.pointDCircleCenter();
                    ptdSunCenterRelativeToCenter.Y = -ptdSunCenterRelativeToCenter.Y;
                    PointPolar ptpSunCenterAzimuthFromImageNorth = new PointPolar(ptdSunCenterRelativeToCenter, true);
                    return ptpSunCenterAzimuthFromImageNorth.Phi - ptpImageNorth.Phi;
                }
                else
                {
                    throw new Exception("not enough data to compute azimuth deviation");
                }
            }
        }

        public AzimuthZenithAngle sunSPAcomputedPosition = null;

        public double computedAzimuthDeviation
        {
            get
            {
                if ((sunSPAcomputedPosition != null) && (gpsData.dataSource == GPSdatasources.IOFFEvesselDataServer))
                {
                    return detectedSunPositionAzimuth - sunSPAcomputedPosition.AzimuthRad;
                }
                else
                {
                    throw new Exception("not enough data to compute azimuth deviation");
                }
            }
        }
    }
}
