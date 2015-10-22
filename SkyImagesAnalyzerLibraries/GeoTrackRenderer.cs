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
using Emgu.CV.Util;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SkyImagesAnalyzerLibraries
{
    public class GeoTrackRenderer
    {
        #region // obsolete - TOPO data
        //public string strTopoGeoGridFilename = Directory.GetCurrentDirectory() + "\\geogrid\\regrid.nc";
        //public string strLatNetCDFVarName = "latitude";
        //public string strLonNetCDFVarName = "longitude";
        //public string strTopoDataNetCDFVarName = "topo";
        #endregion // obsolete - TOPO data

        public static Bgr colorWhite = new Bgr(Color.White);
        public static Bgr colorOcean = new Bgr(255, 201, 135);
        public static Bgr colorBlack = new Bgr(Color.Black);
        public static Bgr tracksColor = new Bgr(200, 100, 255);


        public List<string> listGPSdataLogNetCDFFileNames = new List<string>();

        #region // obsolete - TOPO data
        //private DenseVector dvLatValues = null;
        //private DenseVector dvLonValues = null;
        //private DenseMatrix dmTopoData = null;
        #endregion // obsolete - TOPO data

        // private double scaleFactor = 1.0d;

        #region // obsolete - TOPO data
        //private DenseVector dvLatValuesScaledGrid = null;
        //private DenseVector dvLonValuesScaledGrid = null;
        //private DenseMatrix dmTopoDataScaledGrid = null;
        #endregion // obsolete - TOPO data

        private PointD ptdLeftTopGPS = new PointD(-180.0d, 90.0d);
        private PointD ptdRightBottomGPS = new PointD(180.0d, -90.0d);


        public List<TrackData> lTracksData = new List<TrackData>();


        public List<Tuple<GPSdata, SequencesDrawingVariants, Bgr>> listMarkers =
            new List<Tuple<GPSdata, SequencesDrawingVariants, Bgr>>();

        public GPSdata gpsMapCenteredPoint;
        public bool mapFollowsActualGPSposition = true;

        //private imageConditionAndData imgGeoImg;
        //private imageConditionAndData imgImgTopoEdges;


        private List<Tuple<int, string>> shorelineFileNames = new List<Tuple<int, string>>();
        private int currentShorelineDetailsLevel = 0;
        //private List<Contour<PointD>> lShorelinesGPS = new List<Contour<PointD>>();
        private List<Contour<Point>> lShorelinesAtImage = new List<Contour<Point>>();
        private Image<Bgr, byte> imgShorelinesBackground = null;

        private Size defaultImgSize = new Size(720, 360);
        private Size imgSize = new Size(720, 360);



        public GeoTrackRenderer(string shorelinesDirectoryName)
        {
            if (!Directory.Exists(shorelinesDirectoryName))
            {
                throw new FileNotFoundException("Shorelines directory does not exist: " + shorelinesDirectoryName);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(shorelinesDirectoryName);
            shorelineFileNames =
                dirInfo.EnumerateFiles("shoreline_*.gen")
                    .ToList()
                    .ConvertAll(
                        fInfo =>
                            new Tuple<int, string>(
                                Convert.ToInt32(fInfo.Name.Replace(".gen", "").Replace("shoreline_", "")),
                                fInfo.FullName));
        }


        #region obsolete
        //public GeoTrackRenderer(string filename)
        //{
        //    if (File.Exists(filename))
        //    {
        //        strTopoGeoGridFilename = filename;
        //    }
        //}



        //public double ScaleFactor
        //{
        //    get
        //    {
        //        return scaleFactor;
        //    }
        //    set
        //    {
        //        int scaleIntPrev = Convert.ToInt32(Math.Log(scaleFactor) / Math.Log(2.0d));

        //        scaleFactor = value;

        //        int scaleInt = Convert.ToInt32(Math.Log(scaleFactor)/Math.Log(2.0d));

        //        //int prevShorelineDetailsLevel = currentShorelineDetailsLevel;

        //        //if (scaleInt<=1)
        //        //{
        //        //    currentShorelineDetailsLevel = 0;
        //        //}
        //        //else if (scaleInt <= 3)
        //        //{
        //        //    currentShorelineDetailsLevel = 1;
        //        //}
        //        //else if (scaleInt <= 5)
        //        //{
        //        //    currentShorelineDetailsLevel = 2;
        //        //}
        //        //else if (scaleInt <= 7)
        //        //{
        //        //    currentShorelineDetailsLevel = 3;
        //        //}
        //        //else
        //        //{
        //        //    currentShorelineDetailsLevel = 4;
        //        //}

        //        CalculateScaledBounds();

        //        //if (scaleInt != scaleIntPrev)
        //        //{
        //        ReadShorelineData(currentShorelineDetailsLevel);
        //        //}
        //    }
        //}

        #endregion obsolete




        public void IncreaseShorelineResolution()
        {
            if (currentShorelineDetailsLevel < 4)
            {
                currentShorelineDetailsLevel++;
                ReadShorelineData(currentShorelineDetailsLevel);
            }
        }




        public void DecreaseShorelineResolution()
        {
            if (currentShorelineDetailsLevel > 0)
            {
                currentShorelineDetailsLevel--;
                ReadShorelineData(currentShorelineDetailsLevel);
            }
        }





        public void ScaleImageSize(double scale = 1.5d)
        {
            imgSize = ((new SizeD(defaultImgSize))*scale).ToSize();
        }
        




        private void ReadShorelineData(int level = 0)
        {
            string currLevelShorelineFilename = shorelineFileNames.Find(tpl => tpl.Item1 == level).Item2;

            //lShorelines = new List<Contour<Point>>();

            string strFileContents = ServiceTools.ReadTextFromFile(currLevelShorelineFilename);

            List<string> lContoursSubstrings =
                new List<string>(strFileContents.Split(new string[] { "END" }, StringSplitOptions.RemoveEmptyEntries));
            List<List<string>> lContoursStringContents = lContoursSubstrings.ConvertAll(str =>
            {
                List<string> lStrContents =
                    new List<string>(str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                if (lStrContents.Count == 0)
                {
                    return lStrContents;
                }
                //удалим номер контура
                lStrContents.RemoveAt(0);
                return lStrContents;
            });
            lContoursStringContents.RemoveAll(l => l.Count == 0);



            //RectangleF imageLimits = new RectangleF(ptdLeftTopGPS.PointF(),
            //    new SizeF((ptdRightBottomGPS - ptdLeftTopGPS).PointF()));
            //imageLimits.Height = -imageLimits.Height;
            ////сместить на 180 по долготе, на 90 по широте, отразить сверху вниз.
            //// отмасштабировать все линейные размеры в scaleFactor * 2.0d раз
            //imageLimits.Location = imageLimits.Location + new SizeF(180.0f, 0.0f);
            //imageLimits.Location = new PointF(imageLimits.Location.X, 90.0f - imageLimits.Location.Y);
            //imageLimits.Location = ((new PointD(imageLimits.Location)) * scaleFactor * 2.0d).PointF();
            //imageLimits.Width *= (float)(scaleFactor * additionalScale * 2.0d);
            //imageLimits.Height *= (float)(scaleFactor * additionalScale * 2.0d);
            //Rectangle imageLimitsInt = new Rectangle((new PointD(imageLimits.Location)).Point(),
            //    new Size(Convert.ToInt32(imageLimits.Width), Convert.ToInt32(imageLimits.Height)));

            Rectangle imageLimitsInt = new Rectangle(new Point(0, 0), imgSize);
            double currScaleLat = imgSize.Height/180.0d;
            double currScaleLon = imgSize.Width / 360.0d;
            double currScale = Math.Min(currScaleLat, currScaleLon);


            #region // obsolete

            //Contour<Point> cImageMarginsContour = new Contour<Point>(new MemStorage());

            //List<Point> ptBoundingRectVertices = new List<Point>()
            //{
            //    imageLimitsInt.Location,
            //    imageLimitsInt.Location + new Size(imageLimitsInt.Width, 0),
            //    imageLimitsInt.Location + imageLimitsInt.Size,
            //    imageLimitsInt.Location + new Size(0, imageLimitsInt.Height)
            //};

            //cImageMarginsContour.PushMulti(ptBoundingRectVertices.ToArray(), Emgu.CV.CvEnum.BACK_OR_FRONT.BACK);

            //lShorelinesAtImage = lContoursStringContents.ConvertAll(lContourDataStr =>
            //{
            //    Contour<PointF> currContour = new Contour<PointF>(new MemStorage());
            //    List<PointF> pointsToPush = lContourDataStr.ConvertAll<PointF>(strPointCoords =>
            //    {
            //        List<string> lStrCoordsValues =
            //            new List<string>(strPointCoords.Split(',')).ConvertAll<string>(
            //                str => str.Trim().Replace(".", ","));

            //        List<double> lDoubCoordsValues = lStrCoordsValues.ConvertAll(Convert.ToDouble);
            //        PointD ptdCoordinatesLonLat = new PointD(lDoubCoordsValues[0], lDoubCoordsValues[1]);
            //        PointD ptdCoordinatesAtImage = ptdCoordinatesLonLat + new SizeD(180.0d, 0.0d);
            //        ptdCoordinatesAtImage.Y = 90.0d - ptdCoordinatesAtImage.Y;
            //        ptdCoordinatesAtImage *= scaleFactor * 2.0d;
            //        return ptdCoordinatesAtImage.PointF();
            //    });

            //    pointsToPush = pointsToPush.ConvertAll(ptd =>
            //    {
            //        if (ptd.X < imageLimitsInt.Left) ptd.X = imageLimitsInt.Left - 4;
            //        if (ptd.X > imageLimitsInt.Right) ptd.X = imageLimitsInt.Right + 4;
            //        if (ptd.Y < imageLimitsInt.Top) ptd.Y = imageLimitsInt.Top - 4;
            //        if (ptd.Y > imageLimitsInt.Bottom) ptd.Y = imageLimitsInt.Bottom + 4;
            //        return ptd;
            //    });

            //    List<PointF> pointsToPushShifted = pointsToPush.ConvertAll(pt => new PointF(pt.X - imageLimitsInt.Left, pt.Y - imageLimitsInt.Top));

            //    currContour.PushMulti(pointsToPushShifted.ToArray(), Emgu.CV.CvEnum.BACK_OR_FRONT.BACK);
            //    return currContour;
            //});

            #endregion // obsolete


            lShorelinesAtImage = lContoursStringContents.ConvertAll(lContourDataStr =>
            {
                //VectorOfPoint currContour = new VectorOfPoint();

                Contour<Point> currContour = new Contour<Point>(new MemStorage());

                List<Point> pointsToPush = lContourDataStr.ConvertAll<Point>(strPointCoords =>
                {
                    List<string> lStrCoordsValues =
                        new List<string>(strPointCoords.Split(',')).ConvertAll<string>(
                            str => str.Trim().Replace(".", ","));

                    List<double> lDoubCoordsValues = lStrCoordsValues.ConvertAll(Convert.ToDouble);
                    PointD ptdCoordinatesLonLat = new PointD(lDoubCoordsValues[0], lDoubCoordsValues[1]);
                    // PointD ptdCoordinatesAtImage = ptdCoordinatesLonLat + new SizeD(180.0d, 0.0d);
                    PointD ptdCoordinatesAtImage = ptdCoordinatesLonLat * currScale + new SizeD(imgSize.Width/2.0d, 0.0d);
                    ptdCoordinatesAtImage.Y = imgSize.Height / 2.0d - ptdCoordinatesAtImage.Y;
                    // ptdCoordinatesAtImage *= scaleFactor * additionalScale * 2.0d;
                    return ptdCoordinatesAtImage.Point();
                });

                pointsToPush = pointsToPush.ConvertAll(pt =>
                {
                    if (pt.X < imageLimitsInt.Left) pt.X = imageLimitsInt.Left - 4;
                    if (pt.X > imageLimitsInt.Right) pt.X = imageLimitsInt.Right + 4;
                    if (pt.Y < imageLimitsInt.Top) pt.Y = imageLimitsInt.Top - 4;
                    if (pt.Y > imageLimitsInt.Bottom) pt.Y = imageLimitsInt.Bottom + 4;
                    return pt;
                });

                List<Point> pointsToPushShifted = pointsToPush.ConvertAll(pt => new Point(pt.X - imageLimitsInt.Left, pt.Y - imageLimitsInt.Top));

                //currContour.Push(pointsToPushShifted.ToArray());
                currContour.PushMulti(pointsToPushShifted.ToArray(), Emgu.CV.CvEnum.BACK_OR_FRONT.BACK);

                return currContour;
            });

            // lShorelinesAtImage.RemoveAll(cont => CvInvoke.ContourArea(cont, false) < 1.0d);
            lShorelinesAtImage.RemoveAll(cont => cont.Area < 1.0d);


            Size imgToPlotSize = imageLimitsInt.Size; //new Size(Convert.ToInt32(360*scaleFactor * 2), Convert.ToInt32(180*scaleFactor*2));

            imgShorelinesBackground = new Image<Bgr, byte>(imgToPlotSize.Width, imgToPlotSize.Height, colorOcean);

            foreach (Contour<Point> contour in lShorelinesAtImage)
            {
                if (!contour.Any())
                {
                    continue;
                }
                //imgShorelinesBackground.Draw(contour, colorBlack, (currentShorelineDetailsLevel + 1) * 2);
                imgShorelinesBackground.Draw(contour, colorBlack, 2);
                imgShorelinesBackground.Draw(contour, new Bgr(200, 200, 200), -1);
            }


            //imgShorelinesBackground.Draw(imageLimitsInt, new Bgr(Color.Red), (currentShorelineDetailsLevel + 1) * 4);

            //imgShorelinesBackground.ROI = imageLimitsInt;
            //imgShorelinesBackground = imgShorelinesBackground.Copy();
            //imgShorelinesBackground.ROI = Rectangle.Empty;
        }


        #region // obsolete - TOPO data
        //private void ReadTopoData()
        //{
        //    Dictionary<string, object> dictReadData = NetCDFoperations.ReadDataFromFile(strTopoGeoGridFilename);

        //    dvLatValues = dictReadData[strLatNetCDFVarName] as DenseVector;
        //    dvLatValuesScaledGrid = dictReadData[strLatNetCDFVarName] as DenseVector;
        //    dvLonValues = dictReadData[strLonNetCDFVarName] as DenseVector;
        //    dvLonValuesScaledGrid = dictReadData[strLonNetCDFVarName] as DenseVector;
        //    dmTopoData = dictReadData[strTopoDataNetCDFVarName] as DenseMatrix;
        //    dmTopoDataScaledGrid = dictReadData[strTopoDataNetCDFVarName] as DenseMatrix;
        //    dmTopoDataScaledGrid = ConvertTopoToLog10(dmTopoDataScaledGrid, dmTopoData.Values.Min(),
        //        dmTopoData.Values.Max());


        //    imgGeoImg = new imageConditionAndData(dmTopoDataScaledGrid, null);
        //    imgGeoImg.currentColorScheme = new ColorScheme("");
        //    imgGeoImg.currentColorSchemeRuler = new ColorSchemeRuler(imgGeoImg.currentColorScheme,
        //        dmTopoDataScaledGrid.Values.Min(), dmTopoDataScaledGrid.Values.Max());
        //    imgGeoImg.currentColorSchemeRuler.minValue = dmTopoDataScaledGrid.Values.Min();
        //    imgGeoImg.currentColorSchemeRuler.maxValue = dmTopoDataScaledGrid.Values.Max();
        //    imgGeoImg.currentColorSchemeRuler.imgToRule = imgGeoImg;
        //    imgGeoImg.currentColorSchemeRuler.IsMarginsFixed = false;


        //    DenseMatrix dmTopoDataScaledGridBinary = (DenseMatrix)dmTopoDataScaledGrid.Clone();
        //    dmTopoDataScaledGridBinary.MapInplace(dval => (dval <= 0.0d)?(0.0d):(1.0d));
        //    imgImgTopoEdges = new imageConditionAndData(dmTopoDataScaledGridBinary);
        //    imgImgTopoEdges.setGrayscaleCalculatedColorScheme();
        //    imgImgTopoEdges.currentColorSchemeRuler.minValue = 0.0d;
        //    imgImgTopoEdges.currentColorSchemeRuler.maxValue = 1.0d;
        //    imgImgTopoEdges.currentColorSchemeRuler.IsMarginsFixed = true;
        //    imgImgTopoEdges.currentColorSchemeRuler.imgToRule = imgImgTopoEdges;
        //}
        #endregion // obsolete - TOPO data



        public void ReadGPSFiles(Label lblToShowStatus = null)
        {
            if (listGPSdataLogNetCDFFileNames.Count == 0)
            {
                return;
            }

            if (lTracksData.Count == 0)
            {
                TrackData newTrack = new TrackData();
                newTrack.lineColor = tracksColor;

                lTracksData.Add(newTrack);
            }
            else
            {
                lTracksData.Clear();
                TrackData newTrack = new TrackData();
                lTracksData.Add(newTrack);
            }


            foreach (string GPSdataLogNetCDFfileName in listGPSdataLogNetCDFFileNames)
            {

                if (lblToShowStatus != null)
                {
                    ThreadSafeOperations.SetText(lblToShowStatus, "reading " + GPSdataLogNetCDFfileName, false);
                }

                Dictionary<string, object> dictCurrFileData = NetCDFoperations.ReadDataFromFile(GPSdataLogNetCDFfileName);
                string varNameDateTime = "DateTime";


                List<long> currFileDateTimeLongTicksList = new List<long>((dictCurrFileData[varNameDateTime] as long[]));
                List<DateTime> currFileDateTimeList = currFileDateTimeLongTicksList.ConvertAll(longVal => new DateTime(longVal));

                string varNameGPSdata = "GPSdata";

                List<GPSdata> lGPSdataToAdd = GPSdata.OfDenseMatrix(dictCurrFileData[varNameGPSdata] as DenseMatrix);

                TimeSeries<GPSdata> tsCurrFileGPSserie = new TimeSeries<GPSdata>(lGPSdataToAdd, currFileDateTimeList, true);

                tsCurrFileGPSserie.RemoveValues(gpsDatum => ((gpsDatum.lat == 0.0d) && (gpsDatum.lon == 0.0d)));

                lTracksData[0].tsGPSdata.AddSubseriaData(tsCurrFileGPSserie);
            }

            if (lblToShowStatus != null)
            {
                ThreadSafeOperations.SetText(lblToShowStatus, "", false);
            }
        }




        public Bitmap RepresentTopo(Size imgSizeToGenerate)
        {
            if (imgShorelinesBackground == null)
            {
                ReadShorelineData();
            }

            if (imgSizeToGenerate.Width * imgSizeToGenerate.Height == 0)
            {
                imgSizeToGenerate = imgSize;
            }

            double scaleWidth = (double)imgSizeToGenerate.Width / (double)imgShorelinesBackground.Width;
            double scaleHeight = (double)imgSizeToGenerate.Height / (double)imgShorelinesBackground.Height;
            double scale = Math.Min(scaleWidth, scaleHeight);

            if (scale > 1.0d)
            {
                ReadShorelineData(currentShorelineDetailsLevel);
            }

            // scaleFactor = scale;


            Image<Bgr, byte> imgRetImg = imgShorelinesBackground.Copy();
            // imgRetImg = imgRetImg.Resize(scale, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4);


            foreach (TrackData currTrackDatum in lTracksData)
            {
                Point[] PointsSequence = currTrackDatum.TrackPointsSequenceToDraw(imgRetImg.Size, ptdLeftTopGPS,
                    ptdRightBottomGPS);
                int lineWidth = 4; //Convert.ToInt32(Math.Min(imgSizeToGenerate.Width, imgSizeToGenerate.Height)/180.0d);
                imgRetImg.DrawPolyline(PointsSequence, false, currTrackDatum.lineColor, lineWidth);
            }


            foreach (Tuple<GPSdata, SequencesDrawingVariants, Bgr> markerData in listMarkers)
            {
                GPSdata currGPSpos = markerData.Item1;
                SequencesDrawingVariants drawingVariant = markerData.Item2;
                Bgr currColor = markerData.Item3;

                PointF currMarkerPointF = ImgPointOfGPS(currGPSpos,
                    new SizeD(imgRetImg.Size.Width, imgRetImg.Size.Height), ptdLeftTopGPS, ptdRightBottomGPS).PointF();

                int lineWidth = 4; // Convert.ToInt32(Math.Min(imgSizeToGenerate.Width, imgSizeToGenerate.Height) / 180.0d);


                imgRetImg.Draw(new CircleF(currMarkerPointF, lineWidth), currColor, lineWidth);
            }


            return imgRetImg.Bitmap;
        }




        //private void CalculateScaledBounds()
        //{
        //    //double newLonWidth = 360.0d / scaleFactor;
        //    //double newLatHeight = 180.0d / scaleFactor;

        //    double newLonWidth = 360.0d / scaleFactor;
        //    double newLatHeight = 180.0d / scaleFactor;

        //    PointD ptdCurrentMapCenter = gpsMapCenteredPoint.ToPointD();


        //    double newLonMinValue = -180.0d;
        //    double newLonMaxValue = 180.0d;
        //    if (ptdCurrentMapCenter.X - newLonWidth / 2.0d < -180.0d)
        //    {
        //        newLonMinValue = -180.0d;
        //        newLonMaxValue = newLonMinValue + newLonWidth;
        //    }
        //    else if (ptdCurrentMapCenter.X + newLonWidth / 2.0d > 180.0d)
        //    {
        //        newLonMaxValue = 180.0d;
        //        newLonMinValue = newLonMaxValue - newLonWidth;
        //    }
        //    else
        //    {
        //        newLonMaxValue = ptdCurrentMapCenter.X + newLonWidth / 2.0d;
        //        newLonMinValue = ptdCurrentMapCenter.X - newLonWidth / 2.0d;
        //    }


        //    double newLatMinValue = -90.0d;
        //    double newLatMaxValue = 90.0d;
        //    if (ptdCurrentMapCenter.Y - newLatHeight / 2.0d < -90.0d)
        //    {
        //        newLatMinValue = -90.0d;
        //        newLatMaxValue = newLatMinValue + newLatHeight;
        //    }
        //    else if (ptdCurrentMapCenter.Y + newLatHeight / 2.0d > 90.0d)
        //    {
        //        newLatMaxValue = 90.0d;
        //        newLatMinValue = newLatMaxValue - newLatHeight;
        //    }
        //    else
        //    {
        //        newLatMaxValue = ptdCurrentMapCenter.Y + newLatHeight / 2.0d;
        //        newLatMinValue = ptdCurrentMapCenter.Y - newLatHeight / 2.0d;
        //    }


        //    ptdLeftTopGPS = new PointD(newLonMinValue, newLatMaxValue);
        //    ptdRightBottomGPS = new PointD(newLonMaxValue, newLatMinValue);
        //}






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





        //public void MoveGPSWindow(int directionLon = 0, int directionLat = 0)
        //{
        //    double lonWidth = 360.0d / scaleFactor;
        //    double latHeight = 180.0d / scaleFactor;

        //    gpsMapCenteredPoint.lon += directionLon * (lonWidth / 2.0d) * 100.0d;
        //    gpsMapCenteredPoint.lat += directionLat * (latHeight / 2.0d) * 100.0d;

        //    mapFollowsActualGPSposition = false;

        //    CalculateScaledBounds();
        //    ReadShorelineData(currentShorelineDetailsLevel);
        //}





        //public void MoveGPSWindow(GPSdata newMapCenterGPSPosition)
        //{
        //    gpsMapCenteredPoint = newMapCenterGPSPosition.Clone();

        //    mapFollowsActualGPSposition = false;

        //    CalculateScaledBounds();
        //    ReadShorelineData(currentShorelineDetailsLevel);
        //}





        private PointD ImgPointOfGPS(GPSdata ptGPS, SizeD imgSize, PointD ptdLeftTopGPSCorner, PointD ptdRightBottomGPSCorner)
        {
            double imgY = imgSize.Height -
                          imgSize.Height * (ptGPS.LatDec - ptdRightBottomGPSCorner.Y) /
                          (ptdLeftTopGPSCorner.Y - ptdRightBottomGPSCorner.Y);


            double imgX = imgSize.Width * (ptGPS.LonDec - ptdLeftTopGPSCorner.X) /
                          (ptdRightBottomGPSCorner.X - ptdLeftTopGPSCorner.X);


            return new PointD(imgX, imgY);
        }

    }





    public class TrackData
    {
        public TimeSeries<GPSdata> tsGPSdata = new TimeSeries<GPSdata>();
        public SequencesDrawingVariants drawingVariant = SequencesDrawingVariants.polyline;
        public Bgr lineColor = new Bgr(0, 255, 0);



        public Point[] TrackPointsSequenceToDraw(Size imgSize, PointD ptdLeftTopGPSCorner, PointD ptdRightBottomGPSCorner)
        {
            List<Point> lRetPtList = new List<Point>();

            List<PointD> lPtDGradsList =
                tsGPSdata.DataValues.ConvertAll(
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
    }
}
