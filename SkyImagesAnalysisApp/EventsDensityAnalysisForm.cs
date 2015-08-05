using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyImagesAnalyzerLibraries;
using Point = System.Drawing.Point;

namespace SkyImagesAnalyzer
{
    public partial class EventsDensityAnalysisForm : Form
    {
        Gray white = new Gray(255);
        //output
        private LogWindow theLogWindow = null;
        private string strOutputDirectory = "";

        private Dictionary<string, object> defaultProperties = null;

        public List<PointD> lPointsArray = new List<PointD>();
        public int spaceDiscretization = 100;
        public DenseMatrix dmDensityMesh = null;
        public DenseMatrix dmDensityMeshXcoord = null;
        public DenseMatrix dmDensityMeshYcoord = null;
        public int kernelHalfLength = 10;
        public int maxClustersCount = 4;

        public double minXval;
        public double maxXval;
        public double minYval;
        public double maxYval;

        // public List<PointD> lPtdMarks = new List<PointD>();
        // private imageConditionAndData currHeatMapData = null;
        private ILPlotCube currSurfPlotCube = null;
        public Predicate<Point> conditionOnPoints = new Predicate<Point>(pt =>
        {
            return (pt.Y > pt.X) ? (false) : (true);
        });
        public List<VectorOfPoint> foundClassesContours = new List<VectorOfPoint>();



        public EventsDensityAnalysisForm()
        {
            InitializeComponent();
        }



        public EventsDensityAnalysisForm(List<PointD> lInPoints, Dictionary<string, object> properties, int inSpaceDiscretization = 512)
        {
            InitializeComponent();

            defaultProperties = properties;
            strOutputDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
            if (!ServiceTools.CheckIfDirectoryExists(strOutputDirectory))
            {
                strOutputDirectory = "";
            }

            lPointsArray = new List<PointD>(lInPoints);
            spaceDiscretization = inSpaceDiscretization;
            minXval = lPointsArray.Min(pt => pt.X);
            maxXval = lPointsArray.Max(pt => pt.X);
            minYval = lPointsArray.Min(pt => pt.Y);
            maxYval = lPointsArray.Max(pt => pt.Y);

            minXval = Math.Min(minXval, minYval);
            minYval = minXval;
            maxXval = Math.Max(maxXval, maxYval);
            maxYval = maxXval;
            CalculateDensityMesh();
        }





        public void RepresentDensityField3D()
        {
            ILScene scene = new ILScene();
            currSurfPlotCube = new ILPlotCube();
            currSurfPlotCube.TwoDMode = false;

            ILInArray<double> ilaDataMeshToShow = dmDensityMesh.ToArray();
            ILInArray<double> ilaXvalues = dmDensityMeshXcoord.Row(0).ToArray();
            ILInArray<double> ilaYvalues = dmDensityMeshYcoord.Column(0).ToArray();

            ILSurface surf = new ILSurface(ilaDataMeshToShow, ilaXvalues, ilaYvalues);
            surf.UseLighting = true;
            surf.Colormap = Colormaps.ILNumerics;
            //surf.Children.Add(new ILColorbar());

            currSurfPlotCube.Children.Add(surf);
            currSurfPlotCube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), 1.2f) *
                                Matrix4.Rotation(new Vector3(0, 0, 1), Math.PI);
            currSurfPlotCube.Projection = Projection.Perspective;

            scene.Add(currSurfPlotCube);

            ilPanel1.Scene = scene;
        }





        private void CalculateDensityMesh()
        {
            dmDensityMesh = DenseMatrix.Create(spaceDiscretization, spaceDiscretization, (r, c) => 0.0d);
            dmDensityMeshXcoord = DenseMatrix.Create(spaceDiscretization, spaceDiscretization, (r, c) =>
            {
                return minXval + ((double)c / (double)spaceDiscretization) * (maxXval - minXval);
            });
            dmDensityMeshYcoord = DenseMatrix.Create(spaceDiscretization, spaceDiscretization, (r, c) =>
            {
                return minYval + ((double)r / (double)spaceDiscretization) * (maxYval - minYval);
            });

            double xSpaceDiscrete = (maxXval - minXval) / spaceDiscretization;
            double ySpaceDiscrete = (maxYval - minYval) / spaceDiscretization;

            foreach (PointD pt in lPointsArray)
            {
                int row = Convert.ToInt32((maxYval - pt.Y) / ySpaceDiscrete);
                int col = Convert.ToInt32((pt.X - minXval) / xSpaceDiscrete);
                col = Math.Min(col, spaceDiscretization - 1);
                row = Math.Min(row, spaceDiscretization - 1);

                dmDensityMesh[row, col] += 1.0d;
            }

            dmDensityMesh = (DenseMatrix)dmDensityMesh.Divide(dmDensityMesh.Values.Sum());


            // сгладить гауссом или косинусом
            //double maxL = ((double)kernelHalfLength) * Math.Sqrt(2.0d);
            //DenseMatrix dmKernel = DenseMatrix.Create(2 * kernelHalfLength + 1, 2 * kernelHalfLength + 1, (r, c) =>
            //{
            //    double curDist =
            //        (new PointD(r - (double)kernelHalfLength, c - (double)kernelHalfLength)).Distance(new PointD(0.0d, 0.0d));
            //    return Math.Cos(curDist * Math.PI / (2.0d * maxL));
            //});


            DenseMatrix dmSmoothed = dmDensityMesh.Conv2(StandardConvolutionKernels.cos, kernelHalfLength);
            dmDensityMesh = dmSmoothed.Copy();
        }






        public void Clusterize()
        {
            ArithmeticsOnImages aoi = new ArithmeticsOnImages();
            aoi.dmY = dmDensityMesh;
            aoi.ExprString = "grad5p(Y)";
            aoi.RPNeval(true);
            List<DenseMatrix> lDMGradField = aoi.lDMRes;


            DenseMatrix dmMask = dmDensityMesh.Copy();
            dmMask.MapIndexedInplace((r, c, dVal) =>
            {
                // r = y - perc5
                // c = x - median
                Point currPt = new Point(c, r);
                return (conditionOnPoints(currPt)) ? (1.0d) : (0.0d);
                //if (r > c) return 0.0d;
                //else return 1.0d;
            });
            Image<Gray, Byte> imgMask = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmMask, 0.0d, 1.0d);
            imgMask = imgMask.Flip(FlipType.Vertical);



            // отфильтровать малые значения - ?

            // выделить классы

            List<ConnectedObjectsAtASlice> lSlicesData = new List<ConnectedObjectsAtASlice>();
            double dthresholdingMaxValue = dmDensityMesh.Values.Max();
            //double dthresholdingMinValue = dmSmoothed.Values.Min();
            double dthresholdingMinValue = 0.0d;
            double dthresholdingDiscrete = (dthresholdingMaxValue - dthresholdingMinValue) / 30.0d;
            for (double dThresholding = dthresholdingMaxValue; dThresholding > dthresholdingMinValue - dthresholdingDiscrete; dThresholding -= dthresholdingDiscrete)
            {
                ConnectedObjectsAtASlice corrSliceObj = new ConnectedObjectsAtASlice(dmDensityMesh, dmDensityMeshXcoord,
                    dmDensityMeshYcoord, dThresholding);
                corrSliceObj.DetectConnectedObjects();
                //ServiceTools.ShowPicture(corrSliceObj.previewImage, "thresholding value = " + dThresholding.ToString("e"));
                lSlicesData.Add(corrSliceObj);
            }

            
            ConnectedObjectsAtASlice prevSlice = lSlicesData[0];
            foundClassesContours.AddRange(prevSlice.edgeContoursList);

            foreach (ConnectedObjectsAtASlice currSlice in lSlicesData)
            {
                if (lSlicesData.IndexOf(currSlice) == 0) continue; // самый верхний пропускаем

                //List<Tuple<Contour<Point>, Contour<Point>>> currSliceCoveringContours =
                //    new List<Tuple<Contour<Point>, Contour<Point>>>();
                List<Tuple<VectorOfPoint, VectorOfPoint>> currSliceCoveringContours =
                    new List<Tuple<VectorOfPoint, VectorOfPoint>>();
                //item1 - внутренний, из предыдущего слайса
                //item2 - внешний, из текущего слайса

                foreach (VectorOfPoint caughtCont in foundClassesContours)
                {
                    VectorOfPoint coveringCaughtCont = currSlice.FindContourContainingSample(caughtCont);
                    currSliceCoveringContours.Add(new Tuple<VectorOfPoint, VectorOfPoint>(caughtCont,
                        coveringCaughtCont));
                }

                // добавим контуры, которые только что появились и раньше не были видны на срезах
                // но только если количество допустимых клатеров еще позволяет
                // Иначе - будем ждать, когда они вольются в в какой-нибудь из вновь расширившихся контуров
                foreach (VectorOfPoint newContour in currSlice.edgeContoursList)
                {
                    if ((currSliceCoveringContours.Find(tpl => (tpl.Item2 == newContour)) == null) && (currSliceCoveringContours.Count() < maxClustersCount))
                    {
                        currSliceCoveringContours.Add(new Tuple<VectorOfPoint, VectorOfPoint>(newContour, newContour));
                    }
                }

                // что делать, если какой-нибудь новый контур покрывает больше одного предыдущего
                List<IGrouping<VectorOfPoint, Tuple<VectorOfPoint, VectorOfPoint>>> groups =
                    new List<IGrouping<VectorOfPoint, Tuple<VectorOfPoint, VectorOfPoint>>>
                        (currSliceCoveringContours.GroupBy(tpl => tpl.Item2));
                if (groups.Count(grp => (grp.Count() > 1)) > 0)
                {
                    // есть контуры текущего среза, которые содержат более одного контура предыдущего среза
                    foreach (IGrouping<VectorOfPoint, Tuple<VectorOfPoint, VectorOfPoint>> currGroup in groups)
                    {
                        if (currGroup.Count() == 1)
                        {
                            Tuple<VectorOfPoint, VectorOfPoint> contourTuple = currGroup.First();
                            foundClassesContours.Remove(contourTuple.Item1);
                            foundClassesContours.Add(contourTuple.Item2);
                        }
                        else
                        {
                            // currGroup - группа кортежей контуров, где
                            //              item1 - внутренний, из предыдущего слайса
                            //              item2 - внешний, из текущего слайса
                            // надо точки, которые лежат вне контуров предыдущего слайса отнести к "своим" контурам
                            // попробуем по направлению градиента - относить точку к тому контуру, на который укажет вектор градиента
                            VectorOfPoint currCoveringContour = currGroup.Key; // item2 - внешний, из текущего слайса - см.строку группировки

                            Rectangle currCoveringContourBoundingRectangle =
                                CvInvoke.BoundingRectangle(currCoveringContour);

                            Image<Gray, byte> tmpImg1 =
                                new Image<Gray, byte>(new Size(currCoveringContourBoundingRectangle.Right,
                                    currCoveringContourBoundingRectangle.Bottom));
                            tmpImg1.Draw(currCoveringContour.ToArray(), white, -1);
                            foreach (Tuple<VectorOfPoint, VectorOfPoint> tpl in currGroup)
                            {
                                VectorOfPoint excludingCntr = tpl.Item1;
                                Image<Gray, byte> tmpExcl = tmpImg1.CopyBlank();
                                tmpExcl.Draw(excludingCntr.ToArray(), white, -1);
                                tmpImg1 = tmpImg1 - tmpExcl;
                            }
                            // в картинке tmpImg1 закрашенными остались только точки, которые надо классифицировать
                            List<Point> lPointsToClassify = new List<Point>();

                            for (int x = 0; x < tmpImg1.Width; x++)
                            {
                                for (int y = 0; y < tmpImg1.Height; y++)
                                {
                                    Point currPt = new Point(x, y);
                                    if (tmpImg1[currPt].Equals(white))
                                    {
                                        lPointsToClassify.Add(currPt);
                                    }
                                }
                            }

                            List<List<Point>> llArraysOfPointsAdding = new List<List<Point>>();
                            foreach (Tuple<VectorOfPoint, VectorOfPoint> tpl in currGroup)
                            {
                                llArraysOfPointsAdding.Add(new List<Point>());
                            }

                            List<VectorOfPoint> lContoursOfTheCurrGroup =
                                (new List<Tuple<VectorOfPoint, VectorOfPoint>>(currGroup.ToArray())).ConvertAll(
                                    tpl => tpl.Item1);
                            List<PointD> lPtdMassCenters = lContoursOfTheCurrGroup.ConvertAll(cntr => cntr.MassCenter());
                            VectorOfPoint themassCentersPolygon = new VectorOfPoint();
                            //themassCentersPolygon.PushMulti(lPtdMassCenters.ConvertAll<Point>(ptd => ptd.Point()).ToArray(),
                            //    BACK_OR_FRONT.BACK);
                            themassCentersPolygon.Push(lPtdMassCenters.ConvertAll<Point>(ptd => ptd.Point()).ToArray());
                            Image<Gray, byte> tmpImg = imgMask.CopyBlank();
                            tmpImg.Draw(themassCentersPolygon.ToArray(), white, -1);
                            themassCentersPolygon = tmpImg.FindContours(RetrType.List, ChainApproxMethod.ChainApproxSimple)[0];



                            foreach (Point currPtToClassify in lPointsToClassify)
                            {
                                int cntrToAddPointTo = AttachPointToOneOfConcurrentContours(
                                    lContoursOfTheCurrGroup,
                                    lPtdMassCenters,
                                    themassCentersPolygon,
                                    currPtToClassify,
                                    lDMGradField);
                                if (cntrToAddPointTo == -1) continue;
                                else
                                {
                                    llArraysOfPointsAdding[cntrToAddPointTo].Add(currPtToClassify);
                                }
                            }
                            // распределили. теперь надо сформировать новые контуры - с учетом добавленных точек.
                            List<Image<Gray, byte>> lImagesToDetectNewContours = new List<Image<Gray, byte>>();
                            foreach (Tuple<VectorOfPoint, VectorOfPoint> tpl in currGroup)
                            {
                                Image<Gray, byte> tmpImgCurrCont = tmpImg1.CopyBlank();
                                tmpImgCurrCont.Draw(tpl.Item1.ToArray(), white, -1);
                                lImagesToDetectNewContours.Add(tmpImgCurrCont);
                            }


                            for (int cntIdx = 0; cntIdx < currGroup.Count(); cntIdx++)
                            {
                                foreach (Point pt in llArraysOfPointsAdding[cntIdx])
                                {
                                    lImagesToDetectNewContours[cntIdx][pt.Y, pt.X] = white;
                                }

                                #region // obsolete
                                //Contour<Point> cnt1 =
                                //    lImagesToDetectNewContours[cntIdx].FindContours(
                                //        Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                                //        Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
                                //List<Contour<Point>> lTmpCtrs = new List<Contour<Point>>();
                                //while (true)
                                //{
                                //    lTmpCtrs.Add(cnt1);
                                //    cnt1 = cnt1.HNext;
                                //    if (cnt1 == null)
                                //        break;
                                //}
                                #endregion // obsolete

                                ////найдем наибольший из получившихся контуров
                                List<VectorOfPoint> lTmpCtrs = lImagesToDetectNewContours[cntIdx].FindContours();

                                foundClassesContours.Remove(currGroup.ElementAt(cntIdx).Item1);
                                double maxArea = lTmpCtrs.Max(cntr => cntr.Area());
                                int idxOfMaxAreaContour = lTmpCtrs.FindIndex(cntr => cntr.Area() >= maxArea);
                                foundClassesContours.Add(lTmpCtrs[idxOfMaxAreaContour]);
                            }
                        }
                    }
                }
                else
                {
                    foreach (Tuple<VectorOfPoint, VectorOfPoint> contourTuple in currSliceCoveringContours)
                    {
                        foundClassesContours.Remove(contourTuple.Item1);
                        foundClassesContours.Add(contourTuple.Item2);
                    }
                }
                //theLogWindow = ServiceTools.LogAText(theLogWindow,
                //    "processing thresholding value = " + currSlice.slicingThresholdingValue, true);
            }


            //theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        Environment.NewLine +
            //        "========" + Environment.NewLine +
            //        "FINISHED" + Environment.NewLine +
            //        "========" + Environment.NewLine, true);

            Image<Gray, Byte> imgDataBinary = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmDensityMesh, 0.0d, 1.0d);
            Image<Bgr, byte> previewImage = imgDataBinary.CopyBlank().Convert<Bgr, Byte>();
            var colorGen = new RandomPastelColorGenerator();
            foreach (VectorOfPoint currCntr in foundClassesContours)
            {
                Color currentColor = colorGen.GetNext();
                var currentColorBgr = new Bgr(currentColor);
                previewImage.Draw(currCntr.ToArray(), currentColorBgr, -1);
            }
            previewImage = previewImage.And(imgMask.Convert<Bgr, byte>());
            ServiceTools.ShowPicture(previewImage, "");
        }




        private void EventsDensityAnalysisForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }









        //private int AttachPointToOneOfConcurrentContours(List<Contour<Point>> contours, List<PointD> lPtdMassCenters,
        //    Contour<Point> themassCentersPolygon, Point thePoint, List<DenseMatrix> dmGradField)
        private int AttachPointToOneOfConcurrentContours(List<VectorOfPoint> contours, List<PointD> lPtdMassCenters,
            VectorOfPoint themassCentersPolygon, Point thePoint, List<DenseMatrix> dmGradField)
        {
            // density field should be defined
            if (dmDensityMesh == null) return -1;

            PointD thePointD = new PointD(thePoint);
            // если точка внутри многоугольника, составленного центрами масс уже имеющихся кластеров - посмотрим, куда смотрит градиент.
            
            DenseVector dvGradVect = DenseVector.Create(2, i => dmGradField[i][thePoint.Y, thePoint.X]);
            
            if (contours.Count == 2)
            {
                return AttachPointToOneOf_TWO_ConcurrentContours(contours, lPtdMassCenters, thePoint, dmGradField);
            }
            else if (themassCentersPolygon.InContour(thePointD.PointF()))
            {
                List<DenseVector> lDvDirectionVectorsToMassCenters = lPtdMassCenters.ConvertAll(ptdMassCenter =>
                {
                    DenseVector dvDirection = DenseVector.Create(2, i =>
                    {
                        if (i == 0) return ptdMassCenter.X - thePointD.X;
                        if (i == 1) return ptdMassCenter.Y - thePointD.Y;
                        return 0.0d;
                    });
                    double dValue = Math.Sqrt(dvDirection[0] * dvDirection[0] + dvDirection[1] * dvDirection[1]);
                    dvDirection.MapInplace(d => d / dValue);
                    return dvDirection;
                });

                List<double> lDirectionsMostCloseCosValue =
                    lDvDirectionVectorsToMassCenters.ConvertAll(dvDirection => dvDirection * dvGradVect);
                // максимальное значение соответствует минимальному углу - то, что надо
                // только надо еще обработать ситуацию, когда два кластера примерно в одном направлении, - один за другим на линии градиента
                int maxIdx = lDirectionsMostCloseCosValue.IndexOf(lDirectionsMostCloseCosValue.Max());

                return maxIdx;
            }
            else
            {
                // посчитаем расстояние до границ каждого из контуров. Для минимального - к нему и отнесем.
                List<double> lDistances =
                    contours.ConvertAll(cntr => -CvInvoke.PointPolygonTest(cntr, thePointD.PointF(), true));
                int minIdx = lDistances.IndexOf(lDistances.Min());
                return minIdx;
            }
        }






        private int AttachPointToOneOf_TWO_ConcurrentContours(List<VectorOfPoint> contours, List<PointD> lPtdMassCenters, Point thePoint, List<DenseMatrix> dmGradField)
        {
            // density field should be defined
            if (dmDensityMesh == null) return -1;
            if (contours.Count != 2)
            {
                // этот метод не для таких ситуаций
                throw new NotImplementedException();
            }


            PointD thePointD = new PointD(thePoint);
            DenseVector dvGradVect = DenseVector.Create(2, i => dmGradField[i][thePoint.Y, thePoint.X]);
            // три варианта:
            // 1,2. точка с одной из внешних сторон пары центров масс уже имеющихся кластеров
            // 3. Точка между ними
            DenseVector dvMassCentersConnectingLineVector = DenseVector.Create(2, i =>
            {
                if (i == 0) return (lPtdMassCenters[1].X - lPtdMassCenters[0].X);
                if (i == 1) return (lPtdMassCenters[1].Y - lPtdMassCenters[0].Y);
                else return 0.0d;
            });
            //DenseVector dvMassCentersConnectingOrthogonal = DenseVector.Create(2,
            //    i => (i == 0) ? (dvMassCentersConnectingLineVector[1]) : (-dvMassCentersConnectingLineVector[0]));
            List<DenseVector> listDVdirectionsPtTomassCenters = lPtdMassCenters.ConvertAll<DenseVector>(
                ptdMassCenter =>
                {
                    DenseVector dvDir = DenseVector.Create(2, i =>
                    {
                        if (i == 0) return ptdMassCenter.X - thePointD.X;
                        if (i == 1) return ptdMassCenter.Y - thePointD.Y;
                        return 0.0d;
                    });
                    double dValue = Math.Sqrt(dvDir[0] * dvDir[0] + dvDir[1] * dvDir[1]);
                    dvDir.MapInplace(d => d / dValue);
                    return dvDir;
                });
            double dOverallProductOfScalarProducts = 1.0d;
            foreach (DenseVector dvPtTomassCenter in listDVdirectionsPtTomassCenters)
            {
                dOverallProductOfScalarProducts *= dvPtTomassCenter*dvMassCentersConnectingLineVector;
            }

            // случай 1,2 - когда dOverallProductOfScalarProducts >= 0.0d - то есть, углы между dvMassCentersConnectingLineVector и векторами направления
            // от точки на центры масс - либо оба острые (cos > 0 для обоих), либо оба тупые (cos < 0 для обоих)
            // случай 3 - когда dOverallProductOfScalarProducts < 0.0d

            if (dOverallProductOfScalarProducts < 0.0d)
            {
                // точка между центрами масс - смотрим, куда скатывается градиент
                List<double> lDirectionsMostCloseCosValue =
                    listDVdirectionsPtTomassCenters.ConvertAll(dvDirection => dvDirection * dvGradVect);
                // максимальное значение cos соответствует минимальному углу - то, что надо
                int maxIdx = lDirectionsMostCloseCosValue.IndexOf(lDirectionsMostCloseCosValue.Max());
                return maxIdx;
            }
            else
            {
                // точка снаружи пары - берем ближайшую
                List<double> lDistances =
                    contours.ConvertAll(cntr => -CvInvoke.PointPolygonTest(cntr, thePointD.PointF(), true));
                int minIdx = lDistances.IndexOf(lDistances.Min());
                return minIdx;
            }
        }





        public void SaveClusteringData(string filenameTemplate)
        {
            FileInfo fInfo = new FileInfo(filenameTemplate);
            string dir = fInfo.DirectoryName;
            if (!ServiceTools.CheckIfDirectoryExists(dir))
            {
                return;
            }
            if (fInfo.Exists)
            {
                // не трогаем уже существующий файл
                return;
            }
            if (foundClassesContours.Count == 0)
            {
                return;
            }

            Rectangle gridRect = new Rectangle(0, 0, dmDensityMesh.ColumnCount, dmDensityMesh.RowCount);
            RectangleF realDataRect = new RectangleF((float) minXval, (float) minYval, (float) (maxXval - minXval),
                (float) (maxYval - minYval));

            foreach (VectorOfPoint foundClassContour in foundClassesContours)
            {

                ClusteringData currClusteringData = new ClusteringData(foundClassContour, gridRect, realDataRect);
                string currClassFName = filenameTemplate +
                                        "-X" + currClusteringData.ptdClusterMassCenter.X.ToString("e").Replace(",", "_") +
                                        "-Y" + currClusteringData.ptdClusterMassCenter.Y.ToString("e").Replace(",", "_") + ".xml";
                ServiceTools.WriteObjectToXML(currClusteringData, currClassFName);
            }

            
        }


    }






    public class ConnectedObjectsAtASlice
    {
        public DenseMatrix dmSourceData = null;
        public DenseMatrix dmXvalues = null;
        public DenseMatrix dmYvalues = null;
        public double slicingThresholdingValue = 0.0d;
        public List<VectorOfPoint> edgeContoursList = new List<VectorOfPoint>();
        //public List<DenseMatrix> objectsMasksList = new List<DenseMatrix>();
        public Image<Bgr, byte> previewImage = null;



        public ConnectedObjectsAtASlice(DenseMatrix dmInputData, DenseMatrix XValues, DenseMatrix YValues, double dThresholdingValue)
        {
            dmSourceData = dmInputData.Copy();
            dmXvalues = XValues.Copy();
            dmYvalues = YValues.Copy();
            slicingThresholdingValue = dThresholdingValue;
        }




        public void DetectConnectedObjects()
        {
            DenseMatrix dmDataBinary = dmSourceData.Copy();
            dmDataBinary.MapInplace(dVal => (dVal >= slicingThresholdingValue) ? (1.0d) : (0.0d));
            Image<Gray, Byte> imgDataBinary = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmDataBinary, 0.0d, 1.0d);
            previewImage = imgDataBinary.CopyBlank().Convert<Bgr, Byte>();
            VectorOfVectorOfPoint contoursDetected = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(imgDataBinary, contoursDetected, null, RetrType.List,
                ChainApproxMethod.ChainApproxSimple);
                //imgDataBinary.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                //    Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
            edgeContoursList = new List<VectorOfPoint>();
            int count = contoursDetected.Size;
            var colorGen = new RandomPastelColorGenerator();

            for (int i = 0; i < count; i++)
            {
                Color currentColor = colorGen.GetNext();
                var currentColorBgr = new Bgr(currentColor);
                using (VectorOfPoint currContour = contoursDetected[i])
                {
                    edgeContoursList.Add(currContour);
                    previewImage.Draw(currContour.ToArray(), currentColorBgr, -1);
                }
            }
        }





        public VectorOfPoint FindContourContainingSample(VectorOfPoint sampleContour)
        {
            return edgeContoursList.FirstOrDefault(contour => contour.ContainsContourInside(sampleContour));
        }


        private PointD CellPositionToPointD(int row, int column)
        {
            return new PointD(dmXvalues[row, column], dmYvalues[row, column]);
        }
    }




    [Serializable]
    public class ClusteringData
    {
        private Gray white = new Gray(255);

        [XmlElement("ClusterMassCenter")]
        public PointD ptdClusterMassCenter;

        [XmlElement("RealOperationalArea")]
        public RectangleF rctRealOperationalArea;

        [XmlElement("OperationalAreaOnGrid")]
        public Rectangle rctOperationalAreaOnGrid;

        [XmlElement("OverallPoints")]
        public List<Point> lOverallPoints;


        public ClusteringData()
        {
            lOverallPoints = new List<Point>();
        }



        public ClusteringData(VectorOfPoint foundClusterContour, Rectangle gridSpaceRect, RectangleF realSpaceRect)
        {
            rctRealOperationalArea = realSpaceRect;
            rctOperationalAreaOnGrid = gridSpaceRect;
            ptdClusterMassCenter = foundClusterContour.MassCenter();

            Image<Gray, byte> tmpImg = new Image<Gray, byte>(gridSpaceRect.Width, gridSpaceRect.Height);
            tmpImg.Draw(foundClusterContour.ToArray(), white, -1);

            lOverallPoints = new List<Point>();

            for (int x = 0; x < gridSpaceRect.Width; x++)
            {
                for (int y = 0; y < gridSpaceRect.Height; y++)
                {
                    if (tmpImg[y,x].Equals(white))
                    {
                        lOverallPoints.Add(new Point(x, y));
                    }
                }
            }

        }
    }


}
