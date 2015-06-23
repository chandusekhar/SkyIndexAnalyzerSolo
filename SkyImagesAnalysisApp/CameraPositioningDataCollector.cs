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
//using IoffeVesselDataReader;
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

        private static ConcurrentBag<AngleSunDeviationCalcResult> cbAnglesSunDeviationCalcResults =
            new ConcurrentBag<AngleSunDeviationCalcResult>();

        private List<FileInfo> lImagesFileInfos;
        private bool hasToStopCollecting = false;


        public CameraPositioningDataCollector()
        { }



        public bool IsBusy
        {
            get
            {
                int workingCount = cbgBgwList.Count; // bgwFinished.Sum(boolVal => (boolVal) ? ((int) 0) : ((int) 1));
                if (workingCount == 0)
                {
                    return false;
                }
                else return true;
            }
        }



        public void StopProcessing()
        {
            if (IsBusy)
            {
                hasToStopCollecting = true;
            }
        }



        private ConcurrentBag<string> cbgBgwList = new ConcurrentBag<string>();
        // private List<bool> bgwFinished = new List<bool>();
        public void CollectPositioningData()
        {
#if DEBUG
            int concurrentFilesProcessingCount = 1;
#else
            int concurrentFilesProcessingCount = 2;
            try
            {
                concurrentFilesProcessingCount = Convert.ToInt32(defaultProperties["MaxConcurrentFilesProcessingCount"]);
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "ERROR! exception thrown: " + ex.Message + Environment.NewLine +
                    "MaxConcurrentFilesProcessingCount value can`t be read. " + Environment.NewLine +
                    "Using default value = " + concurrentFilesProcessingCount);
            }
#endif




            theLogWindow = ServiceTools.LogAText(theLogWindow, "started on " + DateTime.UtcNow.ToString("s"));


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

            List<FileInfo> lImagesFileInfoWithExistingSunDiskInfo = lImagesFileInfos.ConvertAll<FileInfo>(fInfoSrc =>
            {
                string sunDiskInfoFileName = fInfoSrc.DirectoryName + "\\" +
                                         Path.GetFileNameWithoutExtension(fInfoSrc.FullName) + "-SunDiskInfo.xml";
                if (File.Exists(sunDiskInfoFileName))
                {
                    return fInfoSrc;
                }
                else return null;
            });
            lImagesFileInfoWithExistingSunDiskInfo.RemoveAll(fInfo => fInfo == null);

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "files already processed before: " + lImagesFileInfoWithExistingSunDiskInfo.Count);

            lImagesFileInfos.RemoveAll(fInfo => lImagesFileInfoWithExistingSunDiskInfo.Contains(fInfo));
            int filesCountToProcess = lImagesFileInfos.Count;
            int filesCountAlreadyProcessed = lImagesFileInfoWithExistingSunDiskInfo.Count;

            lImagesFileInfos.AddRange(lImagesFileInfoWithExistingSunDiskInfo);



            //List<BackgroundWorker> bgwList = new List<BackgroundWorker>();
            //for (int i = 0; i < 2; i++)
            //{
            //    //bgwFinished.Add(true);
            //    //bgwList.Add(null);
            //}

            int currDataIdx = 1;

            foreach (FileInfo finfo in lImagesFileInfos)
            {
                //int currentBgwID = -1;
                while ((cbgBgwList.Count >= concurrentFilesProcessingCount) && (!hasToStopCollecting))
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }

                //while ((bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) == bgwFinished.Count) && (!hasToStopCollecting))
                //{
                //    Application.DoEvents();
                //    Thread.Sleep(0);
                //}
                if (hasToStopCollecting)
                {
                    break;
                }

                //for (int i = 0; i < concurrentFilesProcessingCount; i++)
                //{
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                "" + currDataIdx + " / " + filesCountToProcess + " (+ " + filesCountAlreadyProcessed +
                " processed before)");
                theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + "starting: " + finfo.Name);


                //object[] BGWorker2Args = new object[] { finfo, defaultProperties, currentBgwID };
                object[] BGWorker2Args = new object[] { finfo, defaultProperties };

                BackgroundWorker currBgw = new BackgroundWorker();
                // bgwList[currentBgwID] = currBgw;
                currBgw.DoWork += currBgw_DoWork;
                currBgw.RunWorkerCompleted += currBgw_RunWorkerCompleted;
                currBgw.RunWorkerAsync(BGWorker2Args);

                cbgBgwList.Add(finfo.FullName);

                if (filesCountToProcess > 0)
                {
                    int progress = Convert.ToInt32(100.0d * (double)currDataIdx / (double)filesCountToProcess);
                    ThreadSafeOperations.UpdateProgressBar(ParentForm.pbUniversalProgressBar, progress);
                    Interlocked.Increment(ref currDataIdx);
                }
            }


            while (cbgBgwList.Count > 0)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }

            if (hasToStopCollecting)
            {
                return;
            }



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

            string computedAzimuthDeviationsXMLFile = computedDeviationsXMLfilesPath + "PreComputedSunAnglesDeviationsData.xml";

            List<AngleSunDeviationCalcResult> lResList =
                new List<AngleSunDeviationCalcResult>(cbAnglesSunDeviationCalcResults);
            ServiceTools.WriteObjectToXML(lResList, computedAzimuthDeviationsXMLFile);
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);


            // теперь посчитаем статистику
            List<AngleSunDeviationCalcResult> azimuthResults = new List<AngleSunDeviationCalcResult>(lResList);
            List<AngleSunDeviationCalcResult> zenithResults = new List<AngleSunDeviationCalcResult>(lResList);
            azimuthResults.RemoveAll(
                azimuthSunDeviationCalcResult =>
                    ((!azimuthSunDeviationCalcResult.calculationSucceeded) ||
                     (!azimuthSunDeviationCalcResult.AzimuthDeviationCanBeComputed)));
            zenithResults.RemoveAll(
                sunDeviationCalcResult =>
                    ((!sunDeviationCalcResult.calculationSucceeded) ||
                     (!sunDeviationCalcResult.ZenithDeviationCanBeComputed)));

            List<double> azimuthDevValues =
                azimuthResults.ConvertAll<double>(
                    azimuthSunDeviationCalcResult => azimuthSunDeviationCalcResult.computedAzimuthDeviationDeg);

            List<double> zenithDevValues =
                zenithResults.ConvertAll<double>(
                    azimuthSunDeviationCalcResult => azimuthSunDeviationCalcResult.computedZenithDeviationDeg);


            ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate()
            {
                HistogramDataAndProperties histAzimuthData =
                    new HistogramDataAndProperties(DenseVector.OfEnumerable(azimuthDevValues), 100);
                HistogramCalcAndShowForm hAzimuthForm = new HistogramCalcAndShowForm("azimuth sun angle deviations (deg.)",
                    defaultProperties);
                hAzimuthForm.HistToRepresent = histAzimuthData;
                hAzimuthForm.Show();
                hAzimuthForm.Represent();
            });




            ServiceTools.ExecMethodInSeparateThread(ParentForm, delegate()
            {
                HistogramDataAndProperties histZenithData =
                    new HistogramDataAndProperties(DenseVector.OfEnumerable(zenithDevValues), 100);
                HistogramCalcAndShowForm hZenithForm = new HistogramCalcAndShowForm("zenith sun angle deviations (deg.)",
                    defaultProperties);
                hZenithForm.HistToRepresent = histZenithData;
                hZenithForm.Show();
                hZenithForm.Represent();
            });




            ThreadSafeOperations.UpdateProgressBar(ParentForm.pbUniversalProgressBar, 0);
        }





        void currBgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = e.Result as object[];
            //int currentBgwID = (int) result[0];
            //bgwFinished[currentBgwID] = true;
            string str = "";
            while (!cbgBgwList.TryTake(out str))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
        }






        void currBgw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker selfWorker = sender as BackgroundWorker;
            FileInfo finfo = ((object[])e.Argument)[0] as FileInfo;
            Dictionary<string, object> defaultProperties = ((object[])e.Argument)[1] as Dictionary<string, object>;
            //int currentBgwID = (int) ((object[]) e.Argument)[2];
            LogWindow currImageLogWindow = null;
            bool showOnlyErrors = false;

            string sunDiskInfoFileName = finfo.DirectoryName + "\\" +
                                         Path.GetFileNameWithoutExtension(finfo.FullName) + "-SunDiskInfo.xml";
            if (File.Exists(sunDiskInfoFileName))
            {
                showOnlyErrors = true;
            }

            try
            {
                if (finfo == null)
                {
                    e.Result = false;
                    return;
                }

                if (!showOnlyErrors)
                {
                    currImageLogWindow = ServiceTools.LogAText(currImageLogWindow, Environment.NewLine + "starting: " + finfo.Name);
                    currImageLogWindow = ServiceTools.LogAText(currImageLogWindow, "start processing image " + finfo.Name);
                }


                AngleSunDeviationCalcResult devCalcResDatum = CalculateDevDataForImage(finfo, defaultProperties, currImageLogWindow, showOnlyErrors);


                if (devCalcResDatum.calculationSucceeded)
                {
                    cbAnglesSunDeviationCalcResults.Add(devCalcResDatum);
                }


                if (!showOnlyErrors)
                {
                    currImageLogWindow = ServiceTools.LogAText(currImageLogWindow,
                        Environment.NewLine + "finished: " + Path.GetFileName(devCalcResDatum.fileName) +
                        " with result: " + devCalcResDatum.calculationSucceeded + Environment.NewLine +
                        devCalcResDatum.resultMessage);
                }


                if (currImageLogWindow != null)
                {
                    currImageLogWindow.Close();
                }
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    Environment.NewLine + "ERROR. There exceptions has been thrown: " + Environment.NewLine + ex.Message);

                string strDirForFailedImages = Path.GetDirectoryName(finfo.FullName);
                strDirForFailedImages += (strDirForFailedImages.Last() == '\\') ? ("") : ("\\") + "failed-detections\\";
                try
                {
                    if (ServiceTools.CheckIfDirectoryExists(strDirForFailedImages))
                    {
                        File.Move(finfo.FullName, strDirForFailedImages + finfo.Name);
                    }
                }
                catch (Exception ex1)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        Environment.NewLine + "ERROR. There exceptions has been thrown: " + Environment.NewLine +
                        ex1.Message);
                }


                if (currImageLogWindow != null) currImageLogWindow.Close();
            }


            e.Result = new object[] { true };
        }





        // эта функция будет оценивать расположение солнечного диска
        // определять направление на его центр
        // определять азимутальный солнца угол по:
        // 1. данным GPS корабельной навигации
        // 2. корректировке по хедингу судна по данным корабельной навигации
        // вычислять азимутальное отклонение расположения солнечного диска на снимке
        // от ожидаемого положения
        private AngleSunDeviationCalcResult CalculateDevDataForImage(FileInfo finfo, Dictionary<string, object> defaultProperties, LogWindow currImageLogWindow, bool showOnlyErrors = false)
        {
            FileInfo currFileInfo = finfo;
            Dictionary<string, object> defaultProps = defaultProperties;


            AngleSunDeviationCalcResult retRes = new AngleSunDeviationCalcResult()
            {
                fileName = currFileInfo.FullName,
            };

            GPSdata gps = ServiceTools.FindProperGPSdataForImage(currFileInfo.FullName, theLogWindow, defaultProperties);
            if (gps == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Couldn`t find GPS data for this image.");
                retRes.calculationSucceeded = false;
                retRes.resultMessage = "Couldn`t find GPS data for this image.";
                return retRes;
            }

            #region // obsolete
            //// определяем дату-время файла
            //DateTime curDateTime = DateTime.UtcNow;

            //Image anImage = Image.FromFile(currFileInfo.FullName);
            //ImageInfo newIInfo = new ImageInfo(anImage);
            //int minute = 0;
            //String dateTime = (String)newIInfo.getValueByKey("ExifDTOrig");
            //if (dateTime == null)
            //{
            //    //попробуем вытащить из имени файла
            //    string strDateTime = currFileInfo.Name;
            //    strDateTime = strDateTime.Substring(4, 19);
            //    dateTime = strDateTime;
            //}

            //try
            //{
            //    curDateTime = CommonTools.DateTimeOfString(dateTime);
            //}
            //catch (Exception)
            //{
            //    retRes.calculationSucceeded = false;
            //    retRes.resultMessage = "couldn`t get date/time for file: " + Environment.NewLine + currFileInfo.Name;
            //    return retRes;
            //}
            //curDateTime = DateTime.SpecifyKind(curDateTime, DateTimeKind.Utc);

            //GPSdata neededGPSdata = new GPSdata();
            //string currPath = currFileInfo.DirectoryName;


            //string navFilesPath = defaultProps["IoffeMeteoNavFilesDirectory"] as string;
            //List<IoffeVesselDualNavDataConverted> lAllNavData = new List<IoffeVesselDualNavDataConverted>();

            //string[] sNavFilenames = Directory.GetFiles(navFilesPath, "*.nv2", SearchOption.AllDirectories);
            //if (!sNavFilenames.Any())
            //{
            //    retRes.calculationSucceeded = false;
            //    retRes.resultMessage = "Не найдено файлов данных навигации в директории " + navFilesPath;
            //    return retRes;
            //}
            //else
            //{
            //    foreach (string navFilename in sNavFilenames)
            //    {
            //        Tuple<DateTime, DateTime> timeSpan =
            //            IoffeVesselNavDataReader.GetNavFileDateTimeMargins(navFilename);
            //        if (timeSpan == null)
            //        {
            //            continue;
            //        }

            //        if ((curDateTime < timeSpan.Item1) || (curDateTime > timeSpan.Item2))
            //        {
            //            continue;
            //        }

            //        List<IoffeVesselDualNavDataConverted> dataHasBeenRead = IoffeVesselNavDataReader.ReadNavFile(navFilename);
            //        if (dataHasBeenRead == null)
            //        {
            //            continue;
            //        }
            //        Application.DoEvents();
            //        lAllNavData.AddRange(dataHasBeenRead);
            //    }
            //}

            //lAllNavData.Sort((gpsRecord1, gpsRecord2) =>
            //{
            //    double dev1 = Math.Abs((gpsRecord1.gps.dateTimeUTC - curDateTime).TotalMilliseconds);
            //    double dev2 = Math.Abs((gpsRecord2.gps.dateTimeUTC - curDateTime).TotalMilliseconds);
            //    return (dev1 >= dev2) ? (1) : (-1);
            //});
            //neededGPSdata = lAllNavData[0].gps;
            #endregion // obsolete

            retRes.gpsData = gps;


            #region obsolete
            //double lat = gps.LatDec;
            //double lon = gps.LonDec;


            //SPA spaCalc = new SPA(curDateTime.Year, curDateTime.Month, curDateTime.Day, curDateTime.Hour,
            //    curDateTime.Minute, curDateTime.Second, (float)lon, (float)lat,
            //    (float)SPAConst.DeltaT(curDateTime));
            //int res = spaCalc.spa_calculate();
            //AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
            //    spaCalc.spa.zenith);
            #endregion obsolete


            AzimuthZenithAngle sunPositionSPAext = gps.SunZenithAzimuth();

            if (!showOnlyErrors)
            {
                currImageLogWindow = ServiceTools.LogAText(currImageLogWindow,
                    "SPA ext sun position for " + gps.dateTimeUTC.ToString("s") + ": " + sunPositionSPAext);
            }

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
                throw new Exception(finfo.Name + ": couldn`t detect sun position");
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
    public class AngleSunDeviationCalcResult
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
                    Vector2D ImageNorth = new Vector2D(ptdImageNorthRelativeToCenter);

                    PointPolar ptpImageNorth = new PointPolar(ptdImageNorthRelativeToCenter, true);
                    PointD ptdSunCenterRelativeToCenter = sunDiskDetectedPosition.circle.pointDCircleCenter() -
                                                          imageEdgesDetected.circle.pointDCircleCenter();
                    ptdSunCenterRelativeToCenter.Y = -ptdSunCenterRelativeToCenter.Y;
                    Vector2D sunPositionRelToImageCenter = new Vector2D(ptdSunCenterRelativeToCenter);
                    double cos = ImageNorth*sunPositionRelToImageCenter/
                                 (ImageNorth.VectorLength*sunPositionRelToImageCenter.VectorLength);

                    double retval = 0.0d;

                    if (ptdSunCenterRelativeToCenter.X >= 0)
                    {
                        retval = Math.Acos(cos);
                    }
                    else
                    {
                        retval = 2.0d*Math.PI - Math.Acos(cos);
                    }

                    return -retval;

                    //PointPolar ptpSunCenterAzimuthFromImageNorth = new PointPolar(ptdSunCenterRelativeToCenter, true);
                    ////измеряем в сторону по часовой стрелке - в направлении Azimuth и heading
                    //// при этом картинка относительно остальных показателей (азимут и хединг) отраженная - поэтому возвращаем
                    //// угол со знаком минус.
                    //return (ptpSunCenterAzimuthFromImageNorth.Phi - ptpImageNorth.Phi);
                }
                else
                {
                    throw new Exception("not enough data to compute azimuth deviation");
                }
            }
        }



        public double detectedSunPositionZenith
        {
            get
            {
                if ((sunDiskDetectedPosition.circle != null) && (imageEdgesDetected.circle != null))
                {
                    double radius90deg = imageEdgesDetected.circle.DRadius;
                    double radiusSunPosition =
                        (new Vector2D(sunDiskDetectedPosition.circle.pointDCircleCenter() -
                                     imageEdgesDetected.circle.pointDCircleCenter())).VectorLength;
                    return (radiusSunPosition / radius90deg) * (Math.PI / 2.0d);
                }
                else
                {
                    throw new Exception("not enough data to compute zenith deviation");
                }
            }
        }





        public AzimuthZenithAngle sunSPAcomputedPosition = null;



        public bool AzimuthDeviationCanBeComputed
        {
            get
            {
                if (gpsData.dataSource != GPSdatasources.IOFFEvesselDataServer)
                {
                    // нет данных по хедингу судна - непонятно, какое должно быть смещение из-за поворота судна
                    return false;
                }

                if (sunSPAcomputedPosition == null)
                {
                    // невозможно было вычислить положение солнца по GPS
                    return false;
                }

                return true;
            }
        }



        public double computedAzimuthDeviation
        {
            get
            {
                if (gpsData.dataSource != GPSdatasources.IOFFEvesselDataServer)
                {
                    throw new Exception("not enough data to compute azimuth deviation");
                }

                if ((sunSPAcomputedPosition != null) && (gpsData.dataSource == GPSdatasources.IOFFEvesselDataServer))
                {
                    double angRad = sunSPAcomputedPosition.AzimuthRad - detectedSunPositionAzimuth -
                                    (Math.PI*gpsData.IOFFEdataHeadingTrue/180.0d);

                    return PointPolar.CropAngleRad(angRad, true);
                }
                else
                {
                    throw new Exception("not enough data to compute azimuth deviation");
                }
            }
        }




        public double computedAzimuthDeviationDeg
        {
            get { return (computedAzimuthDeviation / Math.PI) * 180.0d; }
        }




        public bool ZenithDeviationCanBeComputed
        {
            get
            {
                if (sunSPAcomputedPosition == null)
                {
                    // невозможно было вычислить положение солнца по GPS
                    return false;
                }

                return true;
            }
        }




        public double computedZenithDeviation
        {
            get
            {
                if (sunSPAcomputedPosition != null)
                {
                    return detectedSunPositionZenith - sunSPAcomputedPosition.ZenithAngleRad;
                }
                else
                {
                    throw new Exception("not enough data to compute azimuth deviation");
                }
            }
        }




        public double computedZenithDeviationDeg
        {
            get { return (computedZenithDeviation / Math.PI) * 180.0d; }
        }
    }
}
