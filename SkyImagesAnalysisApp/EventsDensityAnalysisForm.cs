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

namespace SkyImagesAnalyzer
{
    public partial class EventsDensityAnalysisForm : Form
    {
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

            currSurfPlotCube.Children.Add(surf);
            currSurfPlotCube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), 1.2f) *
                                Matrix4.Rotation(new Vector3(0, 0, 1), Math.PI);
            currSurfPlotCube.Projection = Projection.Perspective;

            scene.Add(currSurfPlotCube);

            ilPanel1.Scene = scene;





            // отфильтровать малые значения - ?

            // выделить классы




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
    }
}
