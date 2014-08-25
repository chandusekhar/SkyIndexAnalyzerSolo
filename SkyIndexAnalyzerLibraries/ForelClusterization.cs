using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Drawing;
using MathNet.Numerics.Statistics;
using QuickGraph;
using QuickGraph.Algorithms;

namespace SkyIndexAnalyzerLibraries
{
    public class ForelClusterization
    {
        private List<PointD> pointsList;
        public List<int> centersIndexes;
        private double r0 = 1.0d;
        private Random rnd;
        public LogWindow theLogWindow;





        public ForelClusterization(List<PointD> inputPtsD, Func<PointD, PointD, double> distanceMeasurementFunc, double R0, int minClusteringElements = 10)
        {
            pointsList = inputPtsD;
            centersIndexes = new List<int>();
            rnd = new Random();
            r0 = R0;
        }




        public List<ForelTaxa> GetHighLevelTaxonomy(int minClusterCount = 2, int maxClusterCount = 8)
        {
            string logFileName = "G:\\_gulevlab\\SkyIndexAnalyzerSolo_appData\\ApproximationTest\\ClusterizationTestLog.log";
            int goalClusterCount = 3;

            List<Tuple<int, int, int, double, List<ForelTaxa>>> clusterizationQuality = new List<Tuple<int, int, int, double, List<ForelTaxa>>>();

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
                int currentBgwID = (int)currBGWarguments[0];
                int clCount = (int)currBGWarguments[1];
                int perc = (int)currBGWarguments[2];
                int tryCount = (int)currBGWarguments[3];
                List<PointD> localPointsList = new List<PointD>((List<PointD>)currBGWarguments[4]);
                int lowLevelTaxaGoalWeight = (int)currBGWarguments[5];



                List<ForelTaxa> lowLevelTaxaList = GetLowLevelTaxonomy(r0, localPointsList, lowLevelTaxaGoalWeight);
                DenseVector lowLevelTaxaListWeights = DenseVector.Create(lowLevelTaxaList.Count, i => lowLevelTaxaList[i].count);


                //double marginWeightValue = Statistics.Percentile(lowLevelTaxaListWeights, perc);
                //List<ForelTaxa> lowLevelTaxaListFilteredByWeight =
                //    lowLevelTaxaList.FindAll(taxon => taxon.count >= marginWeightValue);

                List<PointD> lowLevelTaxaListFilteredByWeightMassCenters = new List<PointD>();

                //foreach (ForelTaxa taxon in lowLevelTaxaListFilteredByWeight) lowLevelTaxaListFilteredByWeightMassCenters.Add(taxon.MassCenter);
                foreach (ForelTaxa taxon in lowLevelTaxaList) lowLevelTaxaListFilteredByWeightMassCenters.Add(taxon.MassCenter);

                PointD massCenterOfFilteredTaxa = PointD.Sum(lowLevelTaxaListFilteredByWeightMassCenters) /
                                                  (double)lowLevelTaxaListFilteredByWeightMassCenters.Count;

                List<PointD> massCentersList = new List<PointD>();
                //foreach (ForelTaxa currTaxon in lowLevelTaxaListFilteredByWeight)
                foreach (ForelTaxa currTaxon in lowLevelTaxaList)
                {
                    massCentersList.Add(currTaxon.MassCenterPoint.Item2);
                }

                DenseMatrix dmDistances = DenseMatrix.Create(massCentersList.Count, massCentersList.Count,
                    ((r, c) =>
                    {
                        return massCentersList[r].Distance(massCentersList[c]);
                    }));
                double maxVal = dmDistances.Values.Max();
                dmDistances.MapInplace(dVal => (dVal == 0.0d) ? (maxVal) : (dVal));



                //List<ForelTaxa> highLevelTaxaList = new List<ForelTaxa>();
                List<bool> unusedTaxaList = new List<bool>();

                //foreach (ForelTaxa taxa in lowLevelTaxaListFilteredByWeight)
                foreach (ForelTaxa taxa in lowLevelTaxaList)
                {
                    unusedTaxaList.Add(false);
                }

                var g = new BidirectionalGraph<int, TaggedEdge<int, double>>();

                while (unusedTaxaList.Sum(bVal => (bVal) ? (0) : (1)) > 0)
                {
                    bool its1stStep = (unusedTaxaList.Sum(bVal => (bVal) ? (0) : (1)) == unusedTaxaList.Count);
                    DenseMatrix dmAvaliableDistances = (DenseMatrix)dmDistances.Clone();
                    DenseMatrix dmMaskOfAvaliability = DenseMatrix.Create(dmDistances.RowCount,
                        dmDistances.ColumnCount,
                        ((r, c) =>
                        {
                            if (its1stStep) return 1.0d;
                            if (unusedTaxaList[r]) return 0.0d;
                            if (unusedTaxaList[c]) return 1.0d;
                            return 0.0d;
                        }));
                    dmAvaliableDistances =
                        (DenseMatrix)dmAvaliableDistances.PointwiseMultiply(dmMaskOfAvaliability);
                    dmAvaliableDistances.MapInplace(dval => (dval == 0.0d) ? (maxVal) : (dval));

                    Tuple<int, int, double> minDistanceTuple =
                        ServiceTools.DenseMatrixAbsoluteMinimumPosition(dmAvaliableDistances);

                    // посчитаем вес пропорционально общей мощности и обратно пропорчионально расстоянию между уентрами
                    double currWeight = (lowLevelTaxaList[minDistanceTuple.Item1].count +
                                         lowLevelTaxaList[minDistanceTuple.Item2].count) / minDistanceTuple.Item3;
                    //double currWeight = 1.0d / minDistanceTuple.Item3;

                    g.AddVerticesAndEdge(new TaggedEdge<int, double>(minDistanceTuple.Item1,
                        minDistanceTuple.Item2,
                        currWeight));

                    // исключили это значение из поиска минимальных
                    dmDistances[minDistanceTuple.Item1, minDistanceTuple.Item2] = maxVal;
                    dmDistances[minDistanceTuple.Item2, minDistanceTuple.Item1] = maxVal;
                    unusedTaxaList[minDistanceTuple.Item1] = true;
                    unusedTaxaList[minDistanceTuple.Item2] = true;
                }

                List<TaggedEdge<int, double>> edgesList = new List<TaggedEdge<int, double>>(g.Edges);
                edgesList.Sort(new Comparison<TaggedEdge<int, double>>((edg1, edg2) =>
                {
                    if (edg1.Tag == edg2.Tag) return 0;
                    if (edg1.Tag < edg2.Tag) return -1;
                    else return 1;
                }));

                for (int i = 0; i < goalClusterCount - 1; i++)
                {
                    g.RemoveEdge(edgesList[i]);
                }

                IDictionary<int, int> components = new Dictionary<int, int>();

                int componentCount = g.WeaklyConnectedComponents<int, TaggedEdge<int, double>>(components);

                //values - subgraph IDs
                IDictionary<int, ForelTaxa> highLevelTaxaDictionary = new Dictionary<int, ForelTaxa>();
                foreach (KeyValuePair<int, int> component in components)
                {
                    if (highLevelTaxaDictionary.ContainsKey(component.Value))
                    {
                        highLevelTaxaDictionary[component.Value].AddPoints(
                            lowLevelTaxaList[component.Key].PointsIncluded);
                    }
                    else
                    {
                        highLevelTaxaDictionary.Add(new KeyValuePair<int, ForelTaxa>(component.Value,
                            new ForelTaxa(lowLevelTaxaList[component.Key])));
                    }
                }



                //посчитаем функционал качества
                //оценим его как среднее расстояние внутри кластера ( -> min) деленное на среднее межкластерное расстояние ( -> max)
                // будем выбирать решение по минимальному функционалу

                List<ForelTaxa> highLevelTaxaList = new List<ForelTaxa>();
                foreach (KeyValuePair<int, ForelTaxa> pair in highLevelTaxaDictionary) highLevelTaxaList.Add(pair.Value);
                double meanClusterInterElementalDistance = highLevelTaxaList.Sum(currentTaxon =>
                {
                    PointD currTaxonMassCenter = currentTaxon.MassCenter;
                    DenseVector dvCurrentTaxonDistancesToMassCenter = DenseVector.Create(currentTaxon.count,
                        i => currTaxonMassCenter.Distance(currentTaxon.PointsIncluded[i]));

                    return ((dvCurrentTaxonDistancesToMassCenter * dvCurrentTaxonDistancesToMassCenter) /
                            (double)dvCurrentTaxonDistancesToMassCenter.Count);
                });

                //massCenterOfFilteredTaxa
                DenseVector dvInterClusterDistances = DenseVector.Create(highLevelTaxaList.Count, i =>
                {
                    return massCenterOfFilteredTaxa.Distance(highLevelTaxaList[i].MassCenter);
                });
                double meanInterClusterDistance = ((dvInterClusterDistances * dvInterClusterDistances) / (double)dvInterClusterDistances.Count);

                double currentQuality = meanClusterInterElementalDistance / meanInterClusterDistance;

                //currentClusterizationTries.Add(new Tuple<int, double>(tryCount, currentQuality));
                //clusterizationQuality.Add(new Tuple<int, int, int, double, List<ForelTaxa>>(clCount, perc, tryCount, currentQuality, highLevelTaxaList));

                //theLogWindow = ServiceTools.LogAText(theLogWindow,
                //    "clusters count: " + clCount + Environment.NewLine + "percentile: " + perc +
                //    Environment.NewLine + "clusterization try: " + tryCount + Environment.NewLine +
                //    "quality (minimizing): " + currentQuality.ToString("e") + Environment.NewLine +
                //    Environment.NewLine);


                bool success = false;
                while (!success)
                {
                    try
                    {
                        ServiceTools.logToTextFile(logFileName,
                            "" + clCount.ToString().Replace(",", ".") + "," +
                            tryCount.ToString().Replace(",", ".") + "," +
                            lowLevelTaxaGoalWeight.ToString().Replace(",", ".") + "," +
                            currentQuality.ToString("e").Replace(",", ".") +
                            Environment.NewLine, true);
                        success = true;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(100);
                    }
                }





                args.Result = new object[] { currentBgwID, clCount, perc, tryCount, currentQuality, highLevelTaxaList, lowLevelTaxaGoalWeight };
            };



            RunWorkerCompletedEventHandler currWorkCompletedHandler =
                delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    int returningBGWthreadID = (int)currentBGWResults[0];
                    int clCount = (int)currentBGWResults[1];
                    int perc = (int)currentBGWResults[2];
                    int tryCount = (int)currentBGWResults[3];
                    double currentQuality = (double)currentBGWResults[4];
                    List<ForelTaxa> highLevelTaxaList = new List<ForelTaxa>((List<ForelTaxa>)currentBGWResults[5]);
                    int lowLevelTaxaGoalWeight = (int)currentBGWResults[6];

                    clusterizationQuality.Add(new Tuple<int, int, int, double, List<ForelTaxa>>(clCount, perc, tryCount, currentQuality, highLevelTaxaList));

                    theLogWindow = ServiceTools.LogAText(theLogWindow, "" +
                            clCount.ToString() + " | " +
                            tryCount.ToString() + " | " +
                            lowLevelTaxaGoalWeight.ToString() + " | " +
                            currentQuality.ToString(), true);

                    BackgroundWorker currBGW = currBGWCompletedSender as BackgroundWorker;
                    currBGW.Dispose();

                    bgwFinished[returningBGWthreadID] = true;
                    bgwList[returningBGWthreadID].Dispose();
                    bgwList[returningBGWthreadID] = null;
                };


            theLogWindow = ServiceTools.LogAText(theLogWindow, "clusters | try | taxa weight | [quality] ", true);


            for (int clCount = minClusterCount; clCount <= maxClusterCount; clCount++)
            {
                //for (int perc = 90; perc < 99; perc++)
                for (int goalWeight = 50; goalWeight <= 3200; goalWeight *= 2)
                {
                    for (int tryCount = 0; tryCount < 20; tryCount++)
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



                        theLogWindow = ServiceTools.LogAText(theLogWindow, "" +
                            clCount.ToString() + " | " +
                            tryCount.ToString() + " | " +
                            goalWeight.ToString(), true);

                        object[] BGWorker2Args = new object[] { currentBgwID, clCount, 90, tryCount, pointsList, goalWeight };

                        BackgroundWorker currBgw = new BackgroundWorker();
                        bgwList[currentBgwID] = currBgw;
                        currBgw.DoWork += thisWorkerDoWorkHandler;
                        currBgw.RunWorkerCompleted += currWorkCompletedHandler;
                        currBgw.WorkerReportsProgress = true;
                        currBgw.RunWorkerAsync(BGWorker2Args);
                    }
                }
            }


            while (bgwFinished.Sum(boolVal => (boolVal) ? ((int)0) : ((int)1)) > 0)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(100);
            }

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                Environment.NewLine + "==========FINISHED==========" + Environment.NewLine);



            clusterizationQuality.Sort((tpl1, tpl2) =>
            {
                if (tpl1.Item4 > tpl2.Item4) return 1;
                if (tpl1.Item4 == tpl2.Item4) return 0;
                if (tpl1.Item4 < tpl2.Item4) return -1;
                return 0;
            });
            List<ForelTaxa> retHighLevelTaxaList = clusterizationQuality[0].Item5;

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "optimal is:" + Environment.NewLine +
                "(cl count)" + clusterizationQuality[0].Item1 + Environment.NewLine +
                "(weight perc)" + clusterizationQuality[0].Item2 + Environment.NewLine +
                "(try)" + clusterizationQuality[0].Item3 + Environment.NewLine +
                "(quality)" + clusterizationQuality[0].Item4 + Environment.NewLine);

            return retHighLevelTaxaList;
        }







        public List<ForelTaxa> GetLowLevelTaxonomy(double r, List<PointD> pointsToTaxonomate, int goalTaxaWeights = 0)
        {
            List<ForelTaxa> retForelTaxaList = new List<ForelTaxa>();
            List<PointD> unclusterizedPointsList = new List<PointD>(pointsToTaxonomate);

            while (unclusterizedPointsList.Count > 0)
            {
                List<int> massCenterIndexes = new List<int>();
                int currCenterIndex = rnd.Next(unclusterizedPointsList.Count);
                //massCenterIndexes.Add(currCenterIndex);
                //int nextCenterIndex = currCenterIndex + 1;

                ForelTaxa currTaxon = new ForelTaxa();
                List<PointD> restPoints = new List<PointD>();

                while (true)
                {
                    massCenterIndexes.Add(currCenterIndex);
                    currTaxon = GetTaxon(unclusterizedPointsList, currCenterIndex, r, out restPoints);
                    PointD nextCenterPoint = currTaxon.MassCenterPoint.Item2;
                    currCenterIndex = unclusterizedPointsList.FindIndex(pt => pt == nextCenterPoint);

                    if (massCenterIndexes.FindIndex(idx => idx == currCenterIndex) != -1)
                    {
                        break;
                    }
                }

                if (goalTaxaWeights == 0)
                {
                    retForelTaxaList.Add(currTaxon);
                    unclusterizedPointsList = new List<PointD>(restPoints);
                }
                else if (currTaxon.count > goalTaxaWeights)
                {
                    List<PointD> restPointsList;
                    currTaxon = currTaxon.getSubTaxonWithFixedWeight(out restPointsList, goalTaxaWeights);
                    restPoints.AddRange(restPointsList);

                    retForelTaxaList.Add(currTaxon);
                    unclusterizedPointsList = new List<PointD>(restPoints);
                }
                else
                {
                    PointD currTaxonMassCenterPoint = currTaxon.MassCenterPoint.Item2;
                    restPoints.Sort((pt1, pt2) =>
                    {
                        if (currTaxonMassCenterPoint.Distance(pt1) > currTaxonMassCenterPoint.Distance(pt2)) return 1;
                        if (currTaxonMassCenterPoint.Distance(pt1) == currTaxonMassCenterPoint.Distance(pt2)) return 0;
                        if (currTaxonMassCenterPoint.Distance(pt1) < currTaxonMassCenterPoint.Distance(pt2)) return -1;
                        return 0;
                    });
                    int pointsToAdd = goalTaxaWeights - currTaxon.count;
                    if (restPoints.Count >= pointsToAdd)
                    {
                        currTaxon.AddPoints(restPoints.GetRange(0, pointsToAdd));
                        restPoints = new List<PointD>(restPoints.GetRange(pointsToAdd, restPoints.Count - pointsToAdd));
                    }
                    else
                    {
                        currTaxon.AddPoints(restPoints);
                        restPoints = new List<PointD>();
                    }


                    retForelTaxaList.Add(currTaxon);
                    unclusterizedPointsList = new List<PointD>(restPoints);
                }


            }

            return retForelTaxaList;
        }


        private ForelTaxa GetTaxon(List<PointD> avaliablePoints, int centerIndex, double radius, out List<PointD> pointsRemaining)
        {
            ForelTaxa currentTaxon = new ForelTaxa();

            PointD centerPoint = avaliablePoints[centerIndex];

            List<PointD> commonPoints = avaliablePoints.FindAll(currPt => centerPoint.Distance(currPt) <= radius);
            pointsRemaining = avaliablePoints.FindAll(currPt => centerPoint.Distance(currPt) > radius);

            currentTaxon.PointsIncluded = commonPoints;

            return currentTaxon;
        }



        public static Image<Bgr, byte> RepresentTaxaList(List<ForelTaxa> taxaList, double xMin, double xMax, double yMin, double yMax, int pictureWidth = 1024, int pictureHeight = 1024, bool drawLegend = true)
        {
            int serviceSpaceGap = 40;
            Bgr colorBlack = new Bgr(0, 0, 0);
            Image<Bgr, Byte> theImage = null;

            List<PointD> allPointsList = new List<PointD>();
            foreach (ForelTaxa taxon in taxaList)
            {
                allPointsList.AddRange(taxon.PointsIncluded);
            }
            double minX = allPointsList.Min(pt => pt.X);
            minX = Math.Min(xMin, minX);
            double minY = allPointsList.Min(pt => pt.Y);
            minY = Math.Min(yMin, minY);
            double maxX = allPointsList.Max(pt => pt.X);
            maxX = Math.Max(maxX, xMax);
            double maxY = allPointsList.Max(pt => pt.Y);
            maxY = Math.Max(yMax, maxY);

            double ySpaceGap = 0.2d * (maxY - minY);
            double xSpaceGap = 0.2d * (maxX - minX);

            double overallFuncMax = maxY;// + ySpaceGap;
            double overallFuncMin = minY;// - ySpaceGap;
            double xSpaceMin = minX;// + xSpaceGap;
            double xSpaceMax = maxX;// - xSpaceGap;
            serviceSpaceGap = Convert.ToInt32(0.05d * Math.Min(pictureHeight, pictureWidth));
            theImage = new Image<Bgr, Byte>(pictureWidth, pictureHeight, new Bgr(255, 255, 255));




            int xValuesCount = pictureWidth - (2 * serviceSpaceGap);// оставляем место на шкалу Y

            List<Point> rulerVertices = new List<Point>();
            rulerVertices.Add(new Point(serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, serviceSpaceGap));
            rulerVertices.Add(new Point(serviceSpaceGap, serviceSpaceGap));
            theImage.DrawPolyline(rulerVertices.ToArray(), true, colorBlack, 2);

            #region Прописываем текстовые маркеры
            MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);
            theFont.thickness = 1;
            double dMarkersCount = (double)(pictureHeight - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            double dRulerValueGap = (overallFuncMax - overallFuncMin) / (double)dMarkersCount;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = overallFuncMin - Math.IEEERemainder(overallFuncMin, rulerValueGap);
            lowerMarkerValue = (lowerMarkerValue < overallFuncMin) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);
            double currentMarkerValue = lowerMarkerValue;
            double nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            while (nextYPositionDouble > serviceSpaceGap)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(serviceSpaceGap, yPosition), new Point(serviceSpaceGap - 5, yPosition));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValue.ToString("e2");
                theImage.Draw(message, ref theFont, new Point(2, yPosition), markerColor);
                currentMarkerValue += rulerValueGap;
                nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            }

            double dMarkersCountX = (double)(pictureWidth - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCountX = (dMarkersCountX > 10.0d) ? (10.0d) : (dMarkersCountX);
            double dRulerValueGapX = (xSpaceMax - xSpaceMin) / (double)dMarkersCountX;
            int deciGapX = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGapX, 2.0d)));
            double rulerValueGapX = Math.Pow(2.0, (double)deciGapX);
            double lowerMarkerValueX = xSpaceMin - Math.IEEERemainder(xSpaceMin, rulerValueGapX);
            lowerMarkerValueX = (lowerMarkerValueX < xSpaceMin) ? (lowerMarkerValueX + rulerValueGapX) : (lowerMarkerValueX);
            double currentMarkerValueX = lowerMarkerValueX;
            double nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            while (nextXPositionDouble < pictureWidth - serviceSpaceGap)
            {
                double xPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
                int xPosition = Convert.ToInt32(Math.Round(xPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(xPosition, pictureHeight - serviceSpaceGap), new Point(xPosition, pictureHeight - serviceSpaceGap + 5));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValueX.ToString("e2");
                theImage.Draw(message, ref theFont, new Point(xPosition, pictureHeight - 10), markerColor);
                currentMarkerValueX += rulerValueGapX;
                nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            }

            #endregion Прописываем текстовые маркеры


            RandomColorGenerator colorsGenerator = new RandomColorGenerator();
            double koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGap) / (overallFuncMax - overallFuncMin);
            double koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGap) / (xSpaceMax - xSpaceMin);


            int minTaxaWeight = taxaList.Min(taxon => taxon.count);
            int maxTaxaWeight = taxaList.Max(taxon => taxon.count);
            double maxRadius = 40.0d;
            double minRadius = 5.0d;

            double currentMinLegendY = serviceSpaceGap + 4.0d;


            foreach (ForelTaxa taxon in taxaList)
            {
                List<PointD> pointsList = taxon.PointsIncluded;
                Bgr currTaxonColor = new Bgr(colorsGenerator.GetNext());
                foreach (PointD thePoint in pointsList)
                {
                    double yCoordinate = (double)pictureHeight - (double)serviceSpaceGap -
                                         (thePoint.Y - overallFuncMin) * koeffY;
                    double xCoordinate = (double)serviceSpaceGap + (thePoint.X - xSpaceMin) * koeffX;
                    PointD pt2draw = new PointD(xCoordinate, yCoordinate);
                    theImage.Draw(new CircleF(pt2draw.PointF(), 1), currTaxonColor, 1);
                }

                if (drawLegend)
                {
                    double currCircleRadius = minRadius +
                                              ((double)(taxon.count - minTaxaWeight) / (maxTaxaWeight - minTaxaWeight)) *
                                              (maxRadius - minRadius);
                    double y = currentMinLegendY + currCircleRadius;
                    currentMinLegendY += 2.0d * currCircleRadius + 4.0d;
                    double x = (double)serviceSpaceGap + currCircleRadius + 4.0d;
                    PointD ptCenter = new PointD(x, y);
                    theImage.Draw(new CircleF(ptCenter.PointF(), (float)currCircleRadius), currTaxonColor, -1);
                }

            }

            return theImage;
        }



        public static Image<Bgr, byte> RepresentTaxaList(IDictionary<int, ForelTaxa> taxaDict, double xMin, double xMax, double yMin, double yMax, int pictureWidth = 1024, int pictureHeight = 1024, bool drawLegend = true)
        {
            int serviceSpaceGap = 40;
            Bgr colorBlack = new Bgr(0, 0, 0);
            Image<Bgr, Byte> theImage = null;

            List<PointD> allPointsList = new List<PointD>();
            foreach (KeyValuePair<int, ForelTaxa> pair in taxaDict)
            {
                allPointsList.AddRange(pair.Value.PointsIncluded);
            }
            double minX = allPointsList.Min(pt => pt.X);
            minX = Math.Min(xMin, minX);
            double minY = allPointsList.Min(pt => pt.Y);
            minY = Math.Min(yMin, minY);
            double maxX = allPointsList.Max(pt => pt.X);
            maxX = Math.Max(maxX, xMax);
            double maxY = allPointsList.Max(pt => pt.Y);
            maxY = Math.Max(yMax, maxY);

            double ySpaceGap = 0.2d * (maxY - minY);
            double xSpaceGap = 0.2d * (maxX - minX);

            double overallFuncMax = maxY;// + ySpaceGap;
            double overallFuncMin = minY;// - ySpaceGap;
            double xSpaceMin = minX;// + xSpaceGap;
            double xSpaceMax = maxX;// - xSpaceGap;
            serviceSpaceGap = Convert.ToInt32(0.05d * Math.Min(pictureHeight, pictureWidth));
            theImage = new Image<Bgr, Byte>(pictureWidth, pictureHeight, new Bgr(255, 255, 255));




            int xValuesCount = pictureWidth - (2 * serviceSpaceGap);// оставляем место на шкалу Y

            List<Point> rulerVertices = new List<Point>();
            rulerVertices.Add(new Point(serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, serviceSpaceGap));
            rulerVertices.Add(new Point(serviceSpaceGap, serviceSpaceGap));
            theImage.DrawPolyline(rulerVertices.ToArray(), true, colorBlack, 2);

            #region Прописываем текстовые маркеры
            MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);
            theFont.thickness = 1;
            double dMarkersCount = (double)(pictureHeight - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            double dRulerValueGap = (overallFuncMax - overallFuncMin) / (double)dMarkersCount;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = overallFuncMin - Math.IEEERemainder(overallFuncMin, rulerValueGap);
            lowerMarkerValue = (lowerMarkerValue < overallFuncMin) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);
            double currentMarkerValue = lowerMarkerValue;
            double nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            while (nextYPositionDouble > serviceSpaceGap)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(serviceSpaceGap, yPosition), new Point(serviceSpaceGap - 5, yPosition));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValue.ToString("e2");
                theImage.Draw(message, ref theFont, new Point(2, yPosition), markerColor);
                currentMarkerValue += rulerValueGap;
                nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            }

            double dMarkersCountX = (double)(pictureWidth - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCountX = (dMarkersCountX > 10.0d) ? (10.0d) : (dMarkersCountX);
            double dRulerValueGapX = (xSpaceMax - xSpaceMin) / (double)dMarkersCountX;
            int deciGapX = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGapX, 2.0d)));
            double rulerValueGapX = Math.Pow(2.0, (double)deciGapX);
            double lowerMarkerValueX = xSpaceMin - Math.IEEERemainder(xSpaceMin, rulerValueGapX);
            lowerMarkerValueX = (lowerMarkerValueX < xSpaceMin) ? (lowerMarkerValueX + rulerValueGapX) : (lowerMarkerValueX);
            double currentMarkerValueX = lowerMarkerValueX;
            double nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            while (nextXPositionDouble < pictureWidth - serviceSpaceGap)
            {
                double xPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
                int xPosition = Convert.ToInt32(Math.Round(xPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(xPosition, pictureHeight - serviceSpaceGap), new Point(xPosition, pictureHeight - serviceSpaceGap + 5));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValueX.ToString("e2");
                theImage.Draw(message, ref theFont, new Point(xPosition, pictureHeight - 10), markerColor);
                currentMarkerValueX += rulerValueGapX;
                nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            }

            #endregion Прописываем текстовые маркеры

            int minTaxaWeight = taxaDict.Min(kvPair => kvPair.Value.count);
            int maxTaxaWeight = taxaDict.Max(kvPair => kvPair.Value.count);
            double maxRadius = 40.0d;
            double minRadius = 5.0d;

            double currentMinLegendY = serviceSpaceGap + 4.0d;

            RandomColorGenerator colorsGenerator = new RandomColorGenerator();
            double koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGap) / (overallFuncMax - overallFuncMin);
            double koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGap) / (xSpaceMax - xSpaceMin);
            foreach (KeyValuePair<int, ForelTaxa> pair in taxaDict)
            {
                List<PointD> pointsList = pair.Value.PointsIncluded;
                Bgr currTaxonColor = new Bgr(colorsGenerator.GetNext());
                foreach (PointD thePoint in pointsList)
                {
                    double yCoordinate = (double)pictureHeight - (double)serviceSpaceGap -
                                         (thePoint.Y - overallFuncMin) * koeffY;
                    double xCoordinate = (double)serviceSpaceGap + (thePoint.X - xSpaceMin) * koeffX;
                    PointD pt2draw = new PointD(xCoordinate, yCoordinate);
                    theImage.Draw(new CircleF(pt2draw.PointF(), 1), currTaxonColor, 1);
                }

                if (drawLegend)
                {
                    double currCircleRadius = minRadius +
                                              ((double)(pair.Value.count - minTaxaWeight) /
                                               (maxTaxaWeight - minTaxaWeight)) *
                                              (maxRadius - minRadius);
                    double y = currentMinLegendY + currCircleRadius;
                    currentMinLegendY += 2.0d * currCircleRadius + 4.0d;
                    double x = (double)serviceSpaceGap + currCircleRadius + 4.0d;
                    PointD ptCenter = new PointD(x, y);
                    theImage.Draw(new CircleF(ptCenter.PointF(), (float)currCircleRadius), currTaxonColor, -1);
                }
            }

            return theImage;
        }



        public static Image<Bgr, byte> RepresentTaxaListMassCenters(List<ForelTaxa> taxaList, double xMin, double xMax, double yMin, double yMax, int pictureWidth = 1024, int pictureHeight = 1024, bool drawLegend = true)
        {
            int serviceSpaceGap = 40;
            Bgr colorBlack = new Bgr(0, 0, 0);
            Image<Bgr, Byte> theImage = null;

            List<PointD> allPointsList = new List<PointD>();
            foreach (ForelTaxa taxon in taxaList)
            {
                allPointsList.AddRange(taxon.PointsIncluded);
            }
            double minX = allPointsList.Min(pt => pt.X);
            minX = Math.Min(xMin, minX);
            double minY = allPointsList.Min(pt => pt.Y);
            minY = Math.Min(yMin, minY);
            double maxX = allPointsList.Max(pt => pt.X);
            maxX = Math.Max(maxX, xMax);
            double maxY = allPointsList.Max(pt => pt.Y);
            maxY = Math.Max(yMax, maxY);

            double ySpaceGap = 0.2d * (maxY - minY);
            double xSpaceGap = 0.2d * (maxX - minX);

            double overallFuncMax = maxY;// + ySpaceGap;
            double overallFuncMin = minY;// - ySpaceGap;
            double xSpaceMin = minX;// + xSpaceGap;
            double xSpaceMax = maxX;// - xSpaceGap;
            serviceSpaceGap = Convert.ToInt32(0.05d * Math.Min(pictureHeight, pictureWidth));
            theImage = new Image<Bgr, Byte>(pictureWidth, pictureHeight, new Bgr(255, 255, 255));




            int xValuesCount = pictureWidth - (2 * serviceSpaceGap);// оставляем место на шкалу Y

            List<Point> rulerVertices = new List<Point>();
            rulerVertices.Add(new Point(serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, serviceSpaceGap));
            rulerVertices.Add(new Point(serviceSpaceGap, serviceSpaceGap));
            theImage.DrawPolyline(rulerVertices.ToArray(), true, colorBlack, 2);

            #region Прописываем текстовые маркеры
            MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);
            theFont.thickness = 1;
            double dMarkersCount = (double)(pictureHeight - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            double dRulerValueGap = (overallFuncMax - overallFuncMin) / (double)dMarkersCount;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = overallFuncMin - Math.IEEERemainder(overallFuncMin, rulerValueGap);
            lowerMarkerValue = (lowerMarkerValue < overallFuncMin) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);
            double currentMarkerValue = lowerMarkerValue;
            double nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            while (nextYPositionDouble > serviceSpaceGap)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(serviceSpaceGap, yPosition), new Point(serviceSpaceGap - 5, yPosition));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValue.ToString("e2");
                theImage.Draw(message, ref theFont, new Point(2, yPosition), markerColor);
                currentMarkerValue += rulerValueGap;
                nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            }

            double dMarkersCountX = (double)(pictureWidth - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCountX = (dMarkersCountX > 10.0d) ? (10.0d) : (dMarkersCountX);
            double dRulerValueGapX = (xSpaceMax - xSpaceMin) / (double)dMarkersCountX;
            int deciGapX = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGapX, 2.0d)));
            double rulerValueGapX = Math.Pow(2.0, (double)deciGapX);
            double lowerMarkerValueX = xSpaceMin - Math.IEEERemainder(xSpaceMin, rulerValueGapX);
            lowerMarkerValueX = (lowerMarkerValueX < xSpaceMin) ? (lowerMarkerValueX + rulerValueGapX) : (lowerMarkerValueX);
            double currentMarkerValueX = lowerMarkerValueX;
            double nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            while (nextXPositionDouble < pictureWidth - serviceSpaceGap)
            {
                double xPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
                int xPosition = Convert.ToInt32(Math.Round(xPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(xPosition, pictureHeight - serviceSpaceGap), new Point(xPosition, pictureHeight - serviceSpaceGap + 5));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValueX.ToString("e2");
                theImage.Draw(message, ref theFont, new Point(xPosition, pictureHeight - 10), markerColor);
                currentMarkerValueX += rulerValueGapX;
                nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            }

            #endregion Прописываем текстовые маркеры


            RandomColorGenerator colorsGenerator = new RandomColorGenerator();
            double koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGap) / (overallFuncMax - overallFuncMin);
            double koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGap) / (xSpaceMax - xSpaceMin);


            int minTaxaWeight = taxaList.Min(taxon => taxon.count);
            int maxTaxaWeight = taxaList.Max(taxon => taxon.count);
            double maxRadius = 40.0d;
            double minRadius = 5.0d;

            double currentMinLegendY = serviceSpaceGap + 4.0d;


            foreach (ForelTaxa taxon in taxaList)
            {
                List<PointD> pointsList = taxon.PointsIncluded;
                Bgr currTaxonColor = new Bgr(colorsGenerator.GetNext());
                PointD thePoint = taxon.MassCenterPoint.Item2;

                double yCoordinate = (double)pictureHeight - (double)serviceSpaceGap -
                                     (thePoint.Y - overallFuncMin) * koeffY;
                double xCoordinate = (double)serviceSpaceGap + (thePoint.X - xSpaceMin) * koeffX;
                PointD pt2draw = new PointD(xCoordinate, yCoordinate);
                theImage.Draw(new CircleF(pt2draw.PointF(), 2), currTaxonColor, 2);


                if (drawLegend)
                {
                    double currCircleRadius = minRadius +
                                              ((double)(taxon.count - minTaxaWeight) / (maxTaxaWeight - minTaxaWeight)) *
                                              (maxRadius - minRadius);
                    double y = currentMinLegendY + currCircleRadius;
                    currentMinLegendY += 2.0d * currCircleRadius + 4.0d;
                    double x = (double)serviceSpaceGap + currCircleRadius + 4.0d;
                    PointD ptCenter = new PointD(x, y);
                    theImage.Draw(new CircleF(ptCenter.PointF(), (float)currCircleRadius), currTaxonColor, -1);
                }

            }

            return theImage;
        }



        public static Image<Bgr, byte> RepresentTaxaListMassCenters(IDictionary<int, ForelTaxa> taxaDict, double xMin, double xMax, double yMin, double yMax, int pictureWidth = 1024, int pictureHeight = 1024, bool drawLegend = true)
        {
            int serviceSpaceGap = 40;
            Bgr colorBlack = new Bgr(0, 0, 0);
            Image<Bgr, Byte> theImage = null;

            List<PointD> allPointsList = new List<PointD>();
            foreach (KeyValuePair<int, ForelTaxa> pair in taxaDict)
            {
                allPointsList.AddRange(pair.Value.PointsIncluded);
            }
            double minX = allPointsList.Min(pt => pt.X);
            minX = Math.Min(xMin, minX);
            double minY = allPointsList.Min(pt => pt.Y);
            minY = Math.Min(yMin, minY);
            double maxX = allPointsList.Max(pt => pt.X);
            maxX = Math.Max(maxX, xMax);
            double maxY = allPointsList.Max(pt => pt.Y);
            maxY = Math.Max(yMax, maxY);

            double ySpaceGap = 0.2d * (maxY - minY);
            double xSpaceGap = 0.2d * (maxX - minX);

            double overallFuncMax = maxY;// + ySpaceGap;
            double overallFuncMin = minY;// - ySpaceGap;
            double xSpaceMin = minX;// + xSpaceGap;
            double xSpaceMax = maxX;// - xSpaceGap;
            serviceSpaceGap = Convert.ToInt32(0.05d * Math.Min(pictureHeight, pictureWidth));
            theImage = new Image<Bgr, Byte>(pictureWidth, pictureHeight, new Bgr(255, 255, 255));




            int xValuesCount = pictureWidth - (2 * serviceSpaceGap);// оставляем место на шкалу Y

            List<Point> rulerVertices = new List<Point>();
            rulerVertices.Add(new Point(serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, pictureHeight - serviceSpaceGap));
            rulerVertices.Add(new Point(pictureWidth - serviceSpaceGap, serviceSpaceGap));
            rulerVertices.Add(new Point(serviceSpaceGap, serviceSpaceGap));
            theImage.DrawPolyline(rulerVertices.ToArray(), true, colorBlack, 2);

            #region Прописываем текстовые маркеры
            MCvFont theFont = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0d, 1.0d);
            theFont.thickness = 1;
            double dMarkersCount = (double)(pictureHeight - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCount = (dMarkersCount > 10.0d) ? (10.0d) : (dMarkersCount);
            double dRulerValueGap = (overallFuncMax - overallFuncMin) / (double)dMarkersCount;
            int deciGap = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGap, 2.0d)));
            double rulerValueGap = Math.Pow(2.0, (double)deciGap);
            double lowerMarkerValue = overallFuncMin - Math.IEEERemainder(overallFuncMin, rulerValueGap);
            lowerMarkerValue = (lowerMarkerValue < overallFuncMin) ? (lowerMarkerValue + rulerValueGap) : (lowerMarkerValue);
            double currentMarkerValue = lowerMarkerValue;
            double nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            while (nextYPositionDouble > serviceSpaceGap)
            {
                double yPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
                int yPosition = Convert.ToInt32(Math.Round(yPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(serviceSpaceGap, yPosition), new Point(serviceSpaceGap - 5, yPosition));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValue.ToString("e2");
                theImage.Draw(message, ref theFont, new Point(2, yPosition), markerColor);
                currentMarkerValue += rulerValueGap;
                nextYPositionDouble = (1.0d - ((currentMarkerValue - overallFuncMin) / (overallFuncMax - overallFuncMin))) * (double)(pictureHeight - 2 * serviceSpaceGap) + serviceSpaceGap;
            }

            double dMarkersCountX = (double)(pictureWidth - (2 * serviceSpaceGap)) / 30.0d;
            dMarkersCountX = (dMarkersCountX > 10.0d) ? (10.0d) : (dMarkersCountX);
            double dRulerValueGapX = (xSpaceMax - xSpaceMin) / (double)dMarkersCountX;
            int deciGapX = Convert.ToInt32(Math.Truncate(Math.Log(dRulerValueGapX, 2.0d)));
            double rulerValueGapX = Math.Pow(2.0, (double)deciGapX);
            double lowerMarkerValueX = xSpaceMin - Math.IEEERemainder(xSpaceMin, rulerValueGapX);
            lowerMarkerValueX = (lowerMarkerValueX < xSpaceMin) ? (lowerMarkerValueX + rulerValueGapX) : (lowerMarkerValueX);
            double currentMarkerValueX = lowerMarkerValueX;
            double nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            while (nextXPositionDouble < pictureWidth - serviceSpaceGap)
            {
                double xPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
                int xPosition = Convert.ToInt32(Math.Round(xPositionDouble));
                LineSegment2D theLine = new LineSegment2D(new Point(xPosition, pictureHeight - serviceSpaceGap), new Point(xPosition, pictureHeight - serviceSpaceGap + 5));
                Bgr markerColor = colorBlack;
                theImage.Draw(theLine, markerColor, 2);
                String message = currentMarkerValueX.ToString("e2");
                theImage.Draw(message, ref theFont, new Point(xPosition, pictureHeight - 10), markerColor);
                currentMarkerValueX += rulerValueGapX;
                nextXPositionDouble = serviceSpaceGap + ((currentMarkerValueX - xSpaceMin) / (xSpaceMax - xSpaceMin)) * (double)(pictureWidth - 2 * serviceSpaceGap);
            }

            #endregion Прописываем текстовые маркеры

            int minTaxaWeight = taxaDict.Min(kvPair => kvPair.Value.count);
            int maxTaxaWeight = taxaDict.Max(kvPair => kvPair.Value.count);
            double maxRadius = 40.0d;
            double minRadius = 5.0d;

            double currentMinLegendY = serviceSpaceGap + 4.0d;

            RandomColorGenerator colorsGenerator = new RandomColorGenerator();
            double koeffY = ((double)pictureHeight - 2.0d * (double)serviceSpaceGap) / (overallFuncMax - overallFuncMin);
            double koeffX = ((double)pictureWidth - 2.0d * (double)serviceSpaceGap) / (xSpaceMax - xSpaceMin);
            foreach (KeyValuePair<int, ForelTaxa> pair in taxaDict)
            {
                List<PointD> pointsList = pair.Value.PointsIncluded;
                Bgr currTaxonColor = new Bgr(colorsGenerator.GetNext());
                PointD thePoint = pair.Value.MassCenterPoint.Item2;
                double yCoordinate = (double)pictureHeight - (double)serviceSpaceGap -
                                     (thePoint.Y - overallFuncMin) * koeffY;
                double xCoordinate = (double)serviceSpaceGap + (thePoint.X - xSpaceMin) * koeffX;
                PointD pt2draw = new PointD(xCoordinate, yCoordinate);
                theImage.Draw(new CircleF(pt2draw.PointF(), 2), currTaxonColor, 2);


                if (drawLegend)
                {
                    double currCircleRadius = minRadius +
                                              ((double)(pair.Value.count - minTaxaWeight) /
                                               (maxTaxaWeight - minTaxaWeight)) *
                                              (maxRadius - minRadius);
                    double y = currentMinLegendY + currCircleRadius;
                    currentMinLegendY += 2.0d * currCircleRadius + 4.0d;
                    double x = (double)serviceSpaceGap + currCircleRadius + 4.0d;
                    PointD ptCenter = new PointD(x, y);
                    theImage.Draw(new CircleF(ptCenter.PointF(), (float)currCircleRadius), currTaxonColor, -1);
                }
            }

            return theImage;
        }

    }


    public class ForelTaxa
    {
        private List<PointD> pointsIncluded = new List<PointD>();
        public int count { get { return pointsIncluded.Count; } }
        private DenseVector dvPointsX = null;
        private DenseVector dvPointsY = null;
        public int highLevelClusterID;



        public ForelTaxa(ForelTaxa taxonToCopy)
        {
            PointsIncluded = new List<PointD>(taxonToCopy.pointsIncluded);
            highLevelClusterID = taxonToCopy.highLevelClusterID;
        }



        public ForelTaxa(List<PointD> inPointsList)
        {
            PointsIncluded = new List<PointD>(inPointsList);
        }



        public ForelTaxa()
        {
        }



        public Tuple<int, PointD> MassCenterPoint
        {
            get
            {
                PointD mcPt = new PointD();
                mcPt.X = dvPointsX.Sum() / dvPointsX.Count;
                mcPt.Y = dvPointsY.Sum() / dvPointsX.Count;
                DenseVector dvDistances = DenseVector.Create(pointsIncluded.Count, i =>
                {
                    return mcPt.Distance(pointsIncluded[i]);
                });
                int idxMinDist = dvDistances.AbsoluteMinimumIndex();
                return new Tuple<int, PointD>(idxMinDist, pointsIncluded[idxMinDist]);
            }
        }


        public PointD MassCenter
        {
            get
            {
                PointD mcPt = new PointD();
                mcPt.X = dvPointsX.Sum() / dvPointsX.Count;
                mcPt.Y = dvPointsY.Sum() / dvPointsX.Count;
                return mcPt;
            }
        }



        public bool isNull
        {
            get { return (pointsIncluded.Count == 0); }
        }



        public List<PointD> PointsIncluded
        {
            get { return pointsIncluded; }
            set
            {
                pointsIncluded = value;
                dvPointsX = DenseVector.Create(pointsIncluded.Count, i => pointsIncluded[i].X);
                dvPointsY = DenseVector.Create(pointsIncluded.Count, i => pointsIncluded[i].Y);
            }
        }


        public void AddPoint(PointD pt)
        {
            pointsIncluded.Add(pt);
            dvPointsX = DenseVector.Create(pointsIncluded.Count, i => pointsIncluded[i].X);
            dvPointsY = DenseVector.Create(pointsIncluded.Count, i => pointsIncluded[i].Y);
        }




        public void AddPoints(List<PointD> ptList)
        {
            pointsIncluded.AddRange(ptList);
            dvPointsX = DenseVector.Create(pointsIncluded.Count, i => pointsIncluded[i].X);
            dvPointsY = DenseVector.Create(pointsIncluded.Count, i => pointsIncluded[i].Y);
        }



        public ForelTaxa getSubTaxonWithFixedWeight(out List<PointD> restPoints, int goalWeight = 1000)
        {
            restPoints = new List<PointD>();
            if (goalWeight >= pointsIncluded.Count)
            {
                return this;
            }


            ForelTaxa retForelTaxon = new ForelTaxa();
            PointD currMassCenter = MassCenterPoint.Item2;
            DenseVector dvDistancesToMassCenter = DenseVector.Create(pointsIncluded.Count, i => currMassCenter.Distance(pointsIncluded[i]));
            List<Tuple<PointD, double>> pointsListWeighted = new List<Tuple<PointD, double>>();
            for (int i = 0; i < pointsIncluded.Count; i++)
            {
                pointsListWeighted.Add(new Tuple<PointD, double>(pointsIncluded[i], dvDistancesToMassCenter[i]));
            }
            pointsListWeighted.Sort((tpl1, tpl2) =>
            {
                if (tpl1.Item2 > tpl2.Item2) return 1;
                if (tpl1.Item2 == tpl2.Item2) return 0;
                if (tpl1.Item2 < tpl2.Item2) return -1;
                return 0;
            });

            List<PointD> retTaxonPointsList = new List<PointD>();
            for (int i = 0; i < goalWeight; i++) retTaxonPointsList.Add(pointsListWeighted[i].Item1);
            for (int i = goalWeight; i < pointsListWeighted.Count; i++) restPoints.Add(pointsListWeighted[i].Item1);
            return new ForelTaxa(retTaxonPointsList);
        }





        public static bool operator ==(ForelTaxa taxon1, ForelTaxa taxon2)
        {
            if (taxon1.isNull && taxon2.isNull) return true;
            if (taxon1.count != taxon2.count) return false;

            List<PointD> t1Points = new List<PointD>(taxon1.pointsIncluded);
            List<PointD> t2Points = new List<PointD>(taxon2.pointsIncluded);

            Comparison<PointD> pointsComparison = new Comparison<PointD>((pt1, pt2) =>
            {
                if (pt1.Distance(pt2) == 0.0d) return 0;

                PointD ptSubtr = pt1 - new SizeD(pt2);
                if (ptSubtr.X < 0.0d) return -1;
                else if (ptSubtr.X == 0.0d)
                {
                    if (ptSubtr.Y < 0.0d) return -1;
                    else return 1;
                }
                else return 1;
            });

            t1Points.Sort(pointsComparison);
            t2Points.Sort(pointsComparison);


            for (int i = 0; i < taxon1.count; i++)
            {
                if (t1Points[i] != t2Points[i]) return false;
            }
            return true;
        }



        public static bool operator !=(ForelTaxa taxon1, ForelTaxa taxon2)
        {
            return !(taxon1 == taxon2);
        }


        protected bool Equals(ForelTaxa other)
        {
            return Equals(pointsIncluded, other.pointsIncluded);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ForelTaxa)obj);
        }

        public override int GetHashCode()
        {
            return (pointsIncluded != null ? pointsIncluded.GetHashCode() : 0);
        }
    }
}
