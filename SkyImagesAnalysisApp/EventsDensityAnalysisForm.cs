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
using Emgu.CV;
using Emgu.CV.Structure;
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

        public double minXval;
        public double maxXval;
        public double minYval;
        public double maxYval;

        public List<PointD> lPtdMarks = new List<PointD>();

        private imageConditionAndData currHeatMapData = null;

        private ILPlotCube currSurfPlotCube = null;


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

            // сгладить гауссом или косинусом
            double maxL = ((double)kernelHalfLength) * Math.Sqrt(2.0d);
            DenseMatrix dmKernel = DenseMatrix.Create(2 * kernelHalfLength + 1, 2 * kernelHalfLength + 1, (r, c) =>
            {
                double curDist =
                    (new PointD(r - (double)kernelHalfLength, c - (double)kernelHalfLength)).Distance(new PointD(0.0d, 0.0d));
                return Math.Cos(curDist * Math.PI / (2.0d * maxL));
            });

            DenseMatrix dmSmoothed = dmDensityMesh.Conv2(dmKernel);
            dmDensityMesh = dmSmoothed.Copy();

            //imageConditionAndData imgdt1 = new imageConditionAndData(dmSmoothed, null);
            //imgdt1.currentColorScheme = new ColorScheme("");
            //imgdt1.currentColorSchemeRuler = new ColorSchemeRuler(imgdt1.currentColorScheme);
            //imgdt1.currentColorSchemeRuler.imgToRule = imgdt1;
            //imgdt1.currentColorSchemeRuler.IsMarginsFixed = false;
            //imgdt1.UpdateColorSchemeRuler();
            //ImageConditionAndDataRepresentingForm f1 = new ImageConditionAndDataRepresentingForm(imgdt1);
            //f1.Show();


            //if (ServiceTools.CheckIfDirectoryExists(strOutputDirectory + "dmDensityMeshDataSmoothed.nc"))
            //{
            //    dmSmoothed.SaveNetCDFdataMatrix(strOutputDirectory + "dmDensityMeshData.nc");
            //}


            ILScene scene = new ILScene();
            //ILGroup ilgr1 = new ILGroup(translate: new Vector3(.6f, 0, 0), scale: new Vector3(.6f, .8f, .8f),
            //    rotateAxis: new Vector3(1, 1, 0), angle: .1);
            //ilgr1.Children.Add(Shapes.Gear15);
            //ilgr1.Children.Add(Shapes.Gear15Wireframe);
            //ilgr1.Alpha = 0.08f;
            //scene.Add(ilgr1);

            currSurfPlotCube = new ILPlotCube();
            currSurfPlotCube.TwoDMode = false;

            ILInArray<double> ilaDataMeshToShow = dmSmoothed.ToArray();
            ILInArray<double> ilaXvalues = dmDensityMeshXcoord.Row(0).ToArray();
            ILInArray<double> ilaYvalues = dmDensityMeshYcoord.Column(0).ToArray();

            ILSurface surf = new ILSurface(ilaDataMeshToShow, ilaXvalues, ilaYvalues);
            surf.UseLighting = true;
            surf.Colormap = Colormaps.ILNumerics;
            surf.Children.Add(new ILColorbar());

            //DenseMatrix dmplaneData = DenseMatrix.Create(dmSmoothed.RowCount, dmSmoothed.ColumnCount,
            //    (dmSmoothed.Values.Max() + dmSmoothed.Values.Min())/2.0d);
            //ILInArray<double> ilArrPlaneData = dmplaneData.ToArray();
            //ilaXvalues = dmDensityMeshXcoord.Row(0).ToArray();
            //ilaYvalues = dmDensityMeshYcoord.Column(0).ToArray();
            //ILSurface plane = new ILSurface(ilArrPlaneData, ilaXvalues, ilaYvalues);


            currSurfPlotCube.Children.Add(surf);
            //currSurfPlotCube.Children.Add(plane);
            currSurfPlotCube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), 1.2f) *
                                Matrix4.Rotation(new Vector3(0, 0, 1), Math.PI);
            currSurfPlotCube.Projection = Projection.Perspective;

            scene.Add(currSurfPlotCube);

            ilPanel1.Scene = scene;





            // отфильтровать малые значения - ?

            // выделить классы

            List<ConnectedObjectsAtASlice> lSlicesData = new List<ConnectedObjectsAtASlice>();
            double dthresholdingMaxValue = dmSmoothed.Values.Max();
            double dthresholdingMinValue = dmSmoothed.Values.Min();
            double dthresholdingDiscrete = (dthresholdingMaxValue - dthresholdingMinValue) / 20.0d;
            for (double dThresholding = dthresholdingMaxValue; dThresholding >= dthresholdingMinValue; dThresholding -= dthresholdingDiscrete)
            {
                ConnectedObjectsAtASlice corrSliceObj = new ConnectedObjectsAtASlice(dmSmoothed, dmDensityMeshXcoord,
                    dmDensityMeshYcoord, dThresholding);
                corrSliceObj.DetectConnectedObjects();
                //ServiceTools.ShowPicture(corrSliceObj.previewImage,
                //    "thresholding value = " + dThresholding.ToString("e"));
                lSlicesData.Add(corrSliceObj);
            }

            List<Contour<Point>> foundClassesContours = new List<Contour<Point>>();
            ConnectedObjectsAtASlice prevSlice = lSlicesData[0];
            foundClassesContours.AddRange(prevSlice.edgeContoursList);
            foreach (ConnectedObjectsAtASlice currSlice in lSlicesData)
            {
                if (lSlicesData.IndexOf(currSlice) == 0) continue; // самый верхний пропускаем

                List<Tuple<Contour<Point>, Contour<Point>>> currSliceCoveringContours =
                    new List<Tuple<Contour<Point>, Contour<Point>>>();
                //item1 - внутренний, из предыдущего слайса
                //item2 - внешний, из текущего слайса

                foreach (Contour<Point> caughtCont in foundClassesContours)
                {
                    Contour<Point> coveringCaughtCont = currSlice.FindContourContainingSample(caughtCont);
                    currSliceCoveringContours.Add(new Tuple<Contour<Point>, Contour<Point>>(caughtCont,
                        coveringCaughtCont));
                }

                // что делать, если какой-нибудь новый контур покрывает больше одного предыдущего
                List<IGrouping<Contour<Point>, Tuple<Contour<Point>, Contour<Point>>>> groups =
                    new List<IGrouping<Contour<Point>, Tuple<Contour<Point>, Contour<Point>>>>
                        (currSliceCoveringContours.GroupBy(tpl => tpl.Item2));
                if (groups.Count(grp => (grp.Count() > 1)) > 0)
                {
                    // есть контуры текущего среза, которые содержат более одного контура предыдущего среза
                    foreach (IGrouping<Contour<Point>, Tuple<Contour<Point>, Contour<Point>>> currGroup in groups)
                    {
                        if (currGroup.Count() == 1)
                        {
                            Tuple<Contour<Point>, Contour<Point>> contourTuple = currGroup.First();
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
                            Contour<Point> currCoveringContour = currGroup.Key; // item2 - внешний, из текущего слайса - см.строку группировки

                            Image<Gray, byte> tmpImg1 =
                                new Image<Gray, byte>(new Size(currCoveringContour.BoundingRectangle.Right,
                                    currCoveringContour.BoundingRectangle.Bottom));
                            tmpImg1.Draw(currCoveringContour, white, 0);
                            foreach (Tuple<Contour<Point>, Contour<Point>> tpl in currGroup)
                            {
                                Contour<Point> excludingCntr = tpl.Item1;
                                Image<Gray, byte> tmpExcl = tmpImg1.CopyBlank();
                                tmpExcl.Draw(excludingCntr, white, 0);
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

                            foreach (Point currPtToClassify in lPointsToClassify)
                            {
                                AttachPointToOneOfConcurrentContours(currGroup.ElementAt(0).Item1,
                                    currGroup.ElementAt(0).Item2, currPtToClassify);

                            }
                        }
                    }
                }
                else
                {
                    foreach (Tuple<Contour<Point>, Contour<Point>> contourTuple in currSliceCoveringContours)
                    {
                        foundClassesContours.Remove(contourTuple.Item1);
                        foundClassesContours.Add(contourTuple.Item2);
                    }
                }
            }




            // записать данные по классам

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

        }



        private void EventsDensityAnalysisForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }









        private void AttachPointToOneOfConcurrentContours(Contour<Point> c1, Contour<Point> c2, Point pt)
        {
            // density field should be defined
            if (dmDensityMesh == null) return;



        }


    }






    public class ConnectedObjectsAtASlice
    {
        public DenseMatrix dmSourceData = null;
        public DenseMatrix dmXvalues = null;
        public DenseMatrix dmYvalues = null;
        public double slicingThresholdingValue = 0.0d;
        public List<Contour<System.Drawing.Point>> edgeContoursList = new List<Contour<System.Drawing.Point>>();
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
            Contour<System.Drawing.Point> contoursDetected =
                imgDataBinary.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                    Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
            edgeContoursList = new List<Contour<System.Drawing.Point>>();

            var colorGen = new RandomPastelColorGenerator();
            while (true)
            {
                Color currentColor = colorGen.GetNext();
                var currentColorBgr = new Bgr(currentColor);
                Contour<System.Drawing.Point> currContour = contoursDetected;
                edgeContoursList.Add(currContour);
                previewImage.Draw(currContour, currentColorBgr, -1);

                contoursDetected = contoursDetected.HNext;
                if (contoursDetected == null)
                    break;
            }
        }





        public Contour<Point> FindContourContainingSample(Contour<Point> sampleContour)
        {
            foreach (Contour<Point> contour in edgeContoursList)
            {
                if (contour.ContainsContourInside(sampleContour))
                {
                    return contour;
                }
            }
            return null;
        }






        private PointD CellPositionToPointD(int row, int column)
        {
            return new PointD(dmXvalues[row, column], dmYvalues[row, column]);
        }
    }
}
