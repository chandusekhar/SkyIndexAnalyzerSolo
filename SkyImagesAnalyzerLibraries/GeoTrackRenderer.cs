using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SkyImagesAnalyzerLibraries
{
    public class GeoTrackRenderer
    {
        public string strTopoGeoGridFilename = Directory.GetCurrentDirectory() + "\\geogrid\\regrid.nc";
        public string strLatNetCDFVarName = "latitude";
        public string strLonNetCDFVarName = "longitude";
        public string strTopoDataNetCDFVarName = "topo";


        public List<string> listGPSdataLogNetCDFFileNames = new List<string>();


        private DenseVector dvLatValues = null;
        private DenseVector dvLonValues = null;
        private DenseMatrix dmTopoData = null;

        private double scaleFactor = 1.0d;
        private DenseVector dvLatValuesScaledGrid = null;
        private DenseVector dvLonValuesScaledGrid = null;
        private DenseMatrix dmTopoDataScaledGrid = null;

        private PointD ptdLeftTopGPS = new PointD(-180.0d, 90.0d);
        private PointD ptdRightBottomGPS = new PointD(180.0d, -90.0d);


        public List<TrackData> lTracksData = new List<TrackData>();


        public List<Tuple<GPSdata, SequencesDrawingVariants, Bgr>> listMarkers =
            new List<Tuple<GPSdata, SequencesDrawingVariants, Bgr>>();

        public GPSdata gpsMapCenteredPoint;
        public bool mapFollowsActualGPSposition = true;

        private imageConditionAndData imgGeoImg;
        private imageConditionAndData imgImgTopoEdges;

        




        public GeoTrackRenderer(string filename)
        {
            if (File.Exists(filename))
            {
                strTopoGeoGridFilename = filename;
            }
        }

        public double ScaleFactor
        {
            get
            {
                return scaleFactor;
            }
            set
            {
                scaleFactor = value;
                CalculateScaledBounds();
            }
        }


        private void ReadTopoData()
        {
            Dictionary<string, object> dictReadData = NetCDFoperations.ReadDataFromFile(strTopoGeoGridFilename);

            dvLatValues = dictReadData[strLatNetCDFVarName] as DenseVector;
            dvLatValuesScaledGrid = dictReadData[strLatNetCDFVarName] as DenseVector;
            dvLonValues = dictReadData[strLonNetCDFVarName] as DenseVector;
            dvLonValuesScaledGrid = dictReadData[strLonNetCDFVarName] as DenseVector;
            dmTopoData = dictReadData[strTopoDataNetCDFVarName] as DenseMatrix;
            dmTopoDataScaledGrid = dictReadData[strTopoDataNetCDFVarName] as DenseMatrix;
            dmTopoDataScaledGrid = ConvertTopoToLog10(dmTopoDataScaledGrid, dmTopoData.Values.Min(),
                dmTopoData.Values.Max());


            imgGeoImg = new imageConditionAndData(dmTopoDataScaledGrid, null);
            imgGeoImg.currentColorScheme = new ColorScheme("");
            imgGeoImg.currentColorSchemeRuler = new ColorSchemeRuler(imgGeoImg.currentColorScheme,
                dmTopoDataScaledGrid.Values.Min(), dmTopoDataScaledGrid.Values.Max());
            imgGeoImg.currentColorSchemeRuler.minValue = dmTopoDataScaledGrid.Values.Min();
            imgGeoImg.currentColorSchemeRuler.maxValue = dmTopoDataScaledGrid.Values.Max();
            imgGeoImg.currentColorSchemeRuler.imgToRule = imgGeoImg;
            imgGeoImg.currentColorSchemeRuler.IsMarginsFixed = false;


            DenseMatrix dmTopoDataScaledGridBinary = (DenseMatrix)dmTopoDataScaledGrid.Clone();
            dmTopoDataScaledGridBinary.MapInplace(dval => (dval <= 0.0d)?(0.0d):(1.0d));
            imgImgTopoEdges = new imageConditionAndData(dmTopoDataScaledGridBinary);
            imgImgTopoEdges.setGrayscaleCalculatedColorScheme();
            imgImgTopoEdges.currentColorSchemeRuler.minValue = 0.0d;
            imgImgTopoEdges.currentColorSchemeRuler.maxValue = 1.0d;
            imgImgTopoEdges.currentColorSchemeRuler.IsMarginsFixed = true;
            imgImgTopoEdges.currentColorSchemeRuler.imgToRule = imgImgTopoEdges;
        }



        public void ReadGPSFiles(Label lblToShowStatus = null)
        {
            if (listGPSdataLogNetCDFFileNames.Count == 0)
            {
                return;
            }

            if (lTracksData.Count == 0)
            {
                TrackData newTrack = new TrackData();
                newTrack.lineColor = new Bgr(0, 255, 0);

                lTracksData.Add(newTrack);
            }
            else
            {
                lTracksData.Clear();
                TrackData newTrack = new TrackData();
                lTracksData.Add(newTrack);
            }

            List<GPSdata> lTotalGPSdataToAdd = new List<GPSdata>();
            List<DateTime> DateTimeList = new List<DateTime>();

            foreach (string GPSdataLogNetCdfFileName in listGPSdataLogNetCDFFileNames)
            {

                if (lblToShowStatus != null)
                {
                    ThreadSafeOperations.SetText(lblToShowStatus, "reading " + GPSdataLogNetCdfFileName, false);
                }

                Dictionary<string, object> dictCurrFileData = NetCDFoperations.ReadDataFromFile(GPSdataLogNetCdfFileName);
                string varNameDateTime = "DateTime";

                #region //obsolete
                //if (dictCurrFileData.Keys.Contains("DateTime"))
                //{
                //    varNameDateTime = "DateTime";
                //}
                //else if (dictCurrFileData.Keys.Contains("Datetime"))
                //{
                //    varNameDateTime = "Datetime";
                //}
                #endregion //obsolete

                List<long> currFileDateTimeLongTicksList = new List<long>((dictCurrFileData[varNameDateTime] as long[]));
                List<DateTime> currFileDateTimeList = currFileDateTimeLongTicksList.ConvertAll(longVal => new DateTime(longVal));

                string varNameGPSdata = "GPSdata";

                #region //obsolete
                //if (dictCurrFileData.Keys.Contains("AccelerometerData"))
                //{
                //    varNameGPSdata = "AccelerometerData";
                //}
                //else if (dictCurrFileData.Keys.Contains("GPSdata"))
                //{
                //    varNameGPSdata = "GPSdata";
                //}
                #endregion //obsolete


                //DenseMatrix dmTmpGPSdata = dictCurrFileData[varNameGPSdata] as DenseMatrix;
                //ExcelDumper dump = new ExcelDumper();

                List<GPSdata> lGPSdataToAdd = GPSdata.OfDenseMatrix(dictCurrFileData[varNameGPSdata] as DenseMatrix);

                if (currFileDateTimeList.Count > lGPSdataToAdd.Count)
                {
                    currFileDateTimeList.RemoveRange(lGPSdataToAdd.Count,
                        currFileDateTimeList.Count - lGPSdataToAdd.Count);
                }

                while (true)
                {
                    int idx = lGPSdataToAdd.FindLastIndex(gpsDatum => ((gpsDatum.lat == 0.0d) && (gpsDatum.lon == 0.0d)));
                    if (idx == -1)
                    {
                        break;
                    }
                    lGPSdataToAdd.RemoveAt(idx);
                    currFileDateTimeList.RemoveAt(idx);
                }

                lTotalGPSdataToAdd.AddRange(lGPSdataToAdd);
                DateTimeList.AddRange(currFileDateTimeList);
            }

            lTracksData[0].AddGPSsubtrackData(lTotalGPSdataToAdd, DateTimeList);

            if (lblToShowStatus != null)
            {
                ThreadSafeOperations.SetText(lblToShowStatus, "", false);
            }
        }




        public Bitmap RepresentTopo(Size imgSizeToGenerate)
        {
            double ImgSizeEdgesRate = 360.0d/180.0d;
            if ((double)imgSizeToGenerate.Width / (double)imgSizeToGenerate.Height != ImgSizeEdgesRate)
            {
                int th_width = imgSizeToGenerate.Width;
                int th_height = (int)(Math.Round((double)th_width / ImgSizeEdgesRate, 0));
                if (th_height > imgSizeToGenerate.Height)
                {
                    th_height = imgSizeToGenerate.Height;
                    th_width = (int)Math.Round((double)th_height * ImgSizeEdgesRate);
                }

                imgSizeToGenerate = new Size(th_width, th_height);
            }


            if ((dmTopoData == null) || (dvLatValues == null) || (dvLonValues == null))
            {
                ReadTopoData();
            }


            if (((dvLatValuesScaledGrid == null) || (dvLonValuesScaledGrid == null) && (scaleFactor != 1.0d)))
            {
                RegridTopoForImgSizeWithGPSBounds(imgSizeToGenerate);
            }
            else if ((dvLatValuesScaledGrid.Count != imgSizeToGenerate.Height) || (dvLonValuesScaledGrid.Count != imgSizeToGenerate.Width))
            {
                RegridTopoForImgSizeWithGPSBounds(imgSizeToGenerate);
            }
            else if ((dvLatValuesScaledGrid.Values.Min() != ptdRightBottomGPS.Y) || (dvLatValuesScaledGrid.Values.Max() != ptdLeftTopGPS.Y) || (dvLonValuesScaledGrid.Values.Min() != ptdRightBottomGPS.Y) || (dvLonValuesScaledGrid.Values.Max() != ptdLeftTopGPS.Y))
            {
                RegridTopoForImgSizeWithGPSBounds(imgSizeToGenerate);
            }

            Image<Gray, byte> imgLandEdgesDetection = (new Image<Bgr, byte>(imgImgTopoEdges.dataRepresentingImageColored())).Convert<Gray, byte>();
            Contour<Point> contoursDetected = imgLandEdgesDetection.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
            List<Contour<Point>> listOfContours = new List<Contour<Point>>();
            if (contoursDetected != null)
            {
                while (true)
                {
                    Contour<Point> currContour = contoursDetected;

                    listOfContours.Add(currContour);

                    contoursDetected = contoursDetected.HNext;
                    if (contoursDetected == null)
                        break;
                }
            }
            


            Image<Bgr, byte> imgRetImg = new Image<Bgr, byte>(imgGeoImg.dataRepresentingImageColored());
            foreach (Contour<Point> contour in listOfContours)
            {
                imgRetImg.Draw(contour, new Bgr(50, 50, 50), 3);
            }

            imgRetImg = imgRetImg.Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);

            

            foreach (TrackData currTrackDatum in lTracksData)
            {
                Point[] PointsSequence = currTrackDatum.TrackPointsSequenceToDraw(imgRetImg.Size, ptdLeftTopGPS,
                    ptdRightBottomGPS);
                imgRetImg.DrawPolyline(PointsSequence, false, currTrackDatum.lineColor, 2);
            }


            foreach (Tuple<GPSdata, SequencesDrawingVariants, Bgr> markerData in listMarkers)
            {
                GPSdata currGPSpos = markerData.Item1;
                SequencesDrawingVariants drawingVariant = markerData.Item2;
                Bgr currColor = markerData.Item3;

                PointF currMarkerPointF = ImgPointOfGPS(currGPSpos,
                    new SizeD(imgRetImg.Size.Width, imgRetImg.Size.Height), ptdLeftTopGPS, ptdRightBottomGPS).PointF();

                imgRetImg.Draw(new CircleF(currMarkerPointF, 3), currColor, 2);
            }


            return imgRetImg.Bitmap;
        }




        private void CalculateScaledBounds()
        {
            double newLonWidth = 360.0d/scaleFactor;
            double newLatHeight = 180.0d/scaleFactor;

            PointD ptdCurrentMapCenter = gpsMapCenteredPoint.ToPointD();


            double newLonMinValue = -180.0d;
            double newLonMaxValue = 180.0d;
            if (ptdCurrentMapCenter.X - newLonWidth/2.0d < -180.0d)
            {
                newLonMinValue = -180.0d;
                newLonMaxValue = newLonMinValue + newLonWidth;
            }
            else if (ptdCurrentMapCenter.X + newLonWidth/2.0d > 180.0d)
            {
                newLonMaxValue = 180.0d;
                newLonMinValue = newLonMaxValue - newLonWidth;
            }
            else
            {
                newLonMaxValue = ptdCurrentMapCenter.X + newLonWidth/2.0d;
                newLonMinValue = ptdCurrentMapCenter.X - newLonWidth/2.0d;
            }


            double newLatMinValue = -90.0d;
            double newLatMaxValue = 90.0d;
            if (ptdCurrentMapCenter.Y - newLatHeight/2.0d < -90.0d)
            {
                newLatMinValue = -90.0d;
                newLatMaxValue = newLatMinValue + newLatHeight;
            }
            else if (ptdCurrentMapCenter.Y + newLatHeight/2.0d > 90.0d)
            {
                newLatMaxValue = 90.0d;
                newLatMinValue = newLatMaxValue - newLatHeight;
            }
            else
            {
                newLatMaxValue = ptdCurrentMapCenter.Y + newLatHeight/2.0d;
                newLatMinValue = ptdCurrentMapCenter.Y - newLatHeight/2.0d;
            }


            ptdLeftTopGPS = new PointD(newLonMinValue, newLatMaxValue);
            ptdRightBottomGPS = new PointD(newLonMaxValue, newLatMinValue);
        }




        private void RegridTopoForImgSizeWithGPSBounds(Size newImgSize)
        {
            if (newImgSize.IsEmpty)
            {
                return;
            }

            //Stopwatch sw1 = Stopwatch.StartNew();

            dvLonValuesScaledGrid = DenseVector.Create(newImgSize.Width, i =>
            {
                return ptdLeftTopGPS.X + ((ptdRightBottomGPS.X - ptdLeftTopGPS.X)/(double)(newImgSize.Width - 1))*(double) i;
            });

            dvLatValuesScaledGrid = DenseVector.Create(newImgSize.Height, i =>
            {
                return ptdRightBottomGPS.Y + ((ptdLeftTopGPS.Y - ptdRightBottomGPS.Y)/(double)(newImgSize.Height - 1))*(double) i;
            });


            List<double> listLatValues = new List<double>(dvLatValues);
            List<double> listLonValues = new List<double>(dvLonValues);

            List<double> listDLonRightIdx = new List<double>(dvLonValuesScaledGrid);
            List<int> listLonRightIdx = listDLonRightIdx.ConvertAll(new Converter<double, int>(dValLon =>
            {
                int rightIdx = listLonValues.FindIndex(dVal => dVal > dValLon);
                if (rightIdx < 0)
                {
                    rightIdx = listLonValues.FindLastIndex(dVal => dVal == dValLon);
                }
                return rightIdx;
            }));

            List<double> listDLatTopIdx = new List<double>(dvLatValuesScaledGrid);
            List<int> listLatTopIdx = listDLatTopIdx.ConvertAll(new Converter<double, int>(dValLat =>
            {
                int topIdx = listLatValues.FindIndex(dVal => dVal > dValLat);
                if (topIdx < 0)
                {
                    topIdx = listLatValues.FindLastIndex(dVal => dVal == dValLat);
                }
                return topIdx;
            }));



            SizeD GPSwindowSize = new SizeD(ptdRightBottomGPS.X - ptdLeftTopGPS.X, ptdLeftTopGPS.Y - ptdRightBottomGPS.Y);
            double dLon = GPSwindowSize.Width/(newImgSize.Width - 1);

            
            dmTopoDataScaledGrid = DenseMatrix.Create(newImgSize.Height, newImgSize.Width, (r, c) =>
            {
                int rightLonIdx = listLonRightIdx[c];
                int leftLonIdx = rightLonIdx - 1;
                

                int topLatIdx = listLatTopIdx[r];
                int btmLatIdx = topLatIdx - 1;
                

                double t = (dvLonValuesScaledGrid[c] - dvLonValues[leftLonIdx])/
                           (dvLonValues[rightLonIdx] - dvLonValues[leftLonIdx]);
                double u = (dvLatValuesScaledGrid[r] - dvLatValues[btmLatIdx])/
                           (dvLatValues[topLatIdx] - dvLatValues[btmLatIdx]);

                double retTopoVala = (1.0d - t) * (1.0d - u) * dmTopoData[btmLatIdx, leftLonIdx];
                retTopoVala += t * (1.0d - u) * dmTopoData[btmLatIdx, rightLonIdx];
                retTopoVala += t * u * dmTopoData[topLatIdx, rightLonIdx];
                retTopoVala += (1.0d - t) * u * dmTopoData[topLatIdx, leftLonIdx];
                return retTopoVala;
            });
            


            dmTopoDataScaledGrid = ConvertTopoToLog10(dmTopoDataScaledGrid, dmTopoData.Values.Min(),
                dmTopoData.Values.Max());


            imgGeoImg = new imageConditionAndData(dmTopoDataScaledGrid, null);
            imgGeoImg.currentColorScheme = new ColorScheme("");
            imgGeoImg.currentColorSchemeRuler = new ColorSchemeRuler(imgGeoImg.currentColorScheme,
                dmTopoDataScaledGrid.Values.Min(), dmTopoDataScaledGrid.Values.Max());
            imgGeoImg.currentColorSchemeRuler.minValue = -1.0d;
            imgGeoImg.currentColorSchemeRuler.maxValue = 1.0d;
            imgGeoImg.currentColorSchemeRuler.imgToRule = imgGeoImg;
            imgGeoImg.currentColorSchemeRuler.IsMarginsFixed = true;


            DenseMatrix dmTopoDataScaledGridBinary = (DenseMatrix)dmTopoDataScaledGrid.Clone();
            dmTopoDataScaledGridBinary.MapInplace(dval => (dval <= 0.0d) ? (0.0d) : (1.0d));
            imgImgTopoEdges = new imageConditionAndData(dmTopoDataScaledGridBinary);
            imgImgTopoEdges.setGrayscaleCalculatedColorScheme();
            imgImgTopoEdges.currentColorSchemeRuler.minValue = 0.0d;
            imgImgTopoEdges.currentColorSchemeRuler.maxValue = 1.0d;
            imgImgTopoEdges.currentColorSchemeRuler.IsMarginsFixed = true;
            imgImgTopoEdges.currentColorSchemeRuler.imgToRule = imgImgTopoEdges;

            //sw1.Stop();
        }



        private DenseMatrix ConvertTopoToLog10(DenseMatrix dmMatrixToConvert, double maxDepth, double maxHeight)
        {
            double maxHeightLog10 = Math.Log10(1.0d + maxHeight);
            double maxDepthLog10 = Math.Log10(1.0d - maxDepth);

            dmMatrixToConvert.MapInplace(dVal =>
            {
                if (dVal > 0.0d)
                {
                    return Math.Log10(1.0d + dVal) / maxHeightLog10;
                }
                else if (dVal == 0.0d)
                {
                    return 0.0d;
                }
                else
                {
                    return -Math.Log10(1.0d - dVal) / maxDepthLog10;
                }
            });
            return dmMatrixToConvert;
        }





        public void MoveGPSWindow(int directionLon = 0, int directionLat = 0)
        {
            double lonWidth = 360.0d / scaleFactor;
            double latHeight = 180.0d / scaleFactor;

            gpsMapCenteredPoint.lon += directionLon*(lonWidth/2.0d)*100.0d;
            gpsMapCenteredPoint.lat += directionLat * (latHeight / 2.0d) * 100.0d;

            mapFollowsActualGPSposition = false;

            CalculateScaledBounds();
        }





        public void MoveGPSWindow(GPSdata newMapCenterGPSPosition)
        {
            gpsMapCenteredPoint = newMapCenterGPSPosition.Clone();
            
            mapFollowsActualGPSposition = false;

            CalculateScaledBounds();
        }





        private PointD ImgPointOfGPS(GPSdata ptGPS, SizeD imgSize, PointD ptdLeftTopGPSCorner, PointD ptdRightBottomGPSCorner)
        {
            //List<double> listLats = new List<double>(dvLatValues);
            //double idxLat = Convert.ToDouble(listLats.FindIndex(dVal => dVal >= ptGPS.LatDec));
            //double imgY = imgSize.Height - imgSize.Height * idxLat / listLats.Count;

            double imgY = imgSize.Height -
                          imgSize.Height*(ptGPS.LatDec - ptdRightBottomGPSCorner.Y)/
                          (ptdLeftTopGPSCorner.Y - ptdRightBottomGPSCorner.Y);

            

            //List<double> listLons = new List<double>(dvLonValues);
            //double idxLon = Convert.ToDouble(listLons.FindIndex(dVal => dVal >= ptGPS.LonDec));
            //double imgX = imgSize.Width * idxLon / listLons.Count;

            double imgX = imgSize.Width*(ptGPS.LonDec - ptdLeftTopGPSCorner.X)/
                          (ptdRightBottomGPSCorner.X - ptdLeftTopGPSCorner.X);


            return new PointD(imgX, imgY);
        }

    }





    public class TrackData
    {
        public List<GPSdata> lGPScoords = new List<GPSdata>();
        public List<DateTime> lDateTimeStamps = new List<DateTime>();
        public SequencesDrawingVariants drawingVariant = SequencesDrawingVariants.polyline;
        public Bgr lineColor = new Bgr(0, 255, 0);



        public Point[] TrackPointsSequenceToDraw(Size imgSize, PointD ptdLeftTopGPSCorner, PointD ptdRightBottomGPSCorner)
        {
            List<Point> lRetPtList = new List<Point>();

            List<PointD> lPtDGradsList =
                lGPScoords.ConvertAll(
                    gpsDatum =>
                        (gpsDatum != null) ? (new PointD(gpsDatum.LonDec, gpsDatum.LatDec)) : (PointD.nullPointD()));

            lPtDGradsList.RemoveAll(ptD => ptdLeftTopGPSCorner.IsNull);

            lRetPtList = lPtDGradsList.ConvertAll(gpsPtD =>
            {
                Point retPt = new Point();

                retPt.X = Convert.ToInt32(imgSize.Width * (gpsPtD.X - ptdLeftTopGPSCorner.X) / (ptdRightBottomGPSCorner.X - ptdLeftTopGPSCorner.X));
                retPt.Y = imgSize.Height - Convert.ToInt32(imgSize.Height * (gpsPtD.Y - ptdRightBottomGPSCorner.Y) / (ptdLeftTopGPSCorner.Y - ptdRightBottomGPSCorner.Y));

                return retPt;
            });

            List<Point> lRetPtListDistincted = new List<Point>(lRetPtList.Distinct());

            return lRetPtListDistincted.ToArray();
        }





        public void AddGPSsubtrackData(List<GPSdata> lGPSdataToAdd, List<DateTime> lDateTimeStampsRoAdd)
        {
            if (lGPSdataToAdd.Count != lDateTimeStampsRoAdd.Count)
            {
                return;
            }

            // будем сохранять исходный индекс для сохранения исходного порядка значений с равными штампами времени
            // для этого будем запоминать третий член совокупности - исходный индекс
            List<Tuple<DateTime, GPSdata, int>> listToSort = new List<Tuple<DateTime, GPSdata, int>>();


            for (int i = 0; i < lDateTimeStamps.Count; i++)
            {
                listToSort.Add(new Tuple<DateTime, GPSdata, int>(lDateTimeStamps[i], lGPScoords[i], i));
            }

            for (int i = 0; i < lDateTimeStampsRoAdd.Count; i++)
            {
                listToSort.Add(new Tuple<DateTime, GPSdata, int>(lDateTimeStampsRoAdd[i], lGPSdataToAdd[i], i));
            }


            listToSort.Sort(new Comparison<Tuple<DateTime, GPSdata, int>>((tpl1, tpl2) =>
            {
                if (tpl1.Item1 < tpl2.Item1)
                {
                    return -1;
                }
                else if (tpl1.Item1 > tpl2.Item1)
                {
                    return 1;
                }
                else
                {
                    if (tpl1.Item3 < tpl2.Item3)
                    {
                        return -1;
                    }
                    else if (tpl1.Item3 > tpl2.Item3)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }));
            
            
            lDateTimeStamps.Clear();
            lGPScoords.Clear();
            foreach (Tuple<DateTime, GPSdata, int> tpl in listToSort)
            {
                lDateTimeStamps.Add(tpl.Item1);
                lGPScoords.Add(tpl.Item2);
            }

            //for (int i = 0; i < lDateTimeStampsRoAdd.Count; i++)
            //{

            //    int idx = lDateTimeStamps.FindIndex(dt => dt <= lDateTimeStampsRoAdd[i]);
            //    lDateTimeStamps.Insert(idx + 1, lDateTimeStampsRoAdd[i]);
            //    lGPScoords.Insert(idx + 1, lGPSdataToAdd[i]);
            //}
        }


    }
}
