using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SkyImagesAnalyzerLibraries
{
    public partial class Field3Drepresentation : Form
    {
        private string strOutputDirectory = "";
        private Dictionary<string, object> defaultProperties = null;
        public string strDataDescription = "";


        public List<PointD> lPointsArray = new List<PointD>();
        public int spaceDiscretization = 100;
        public DenseMatrix dmData = null;
        public DenseMatrix dmDataXcoord = null;
        public DenseMatrix dmDataYcoord = null;

        public int kernelHalfLength = 10;
        public int maxClustersCount = 4;

        public double minXval;
        public double maxXval;
        public double minYval;
        public double maxYval;

        public List<PointD> lPtdMarks = new List<PointD>();

        private imageConditionAndData currHeatMapData = null;

        private ILPlotCube currSurfPlotCube = null;




        public Field3Drepresentation(DenseMatrix dmDataToRepresent, Dictionary<string, object> properties, string description = "")
        {
            InitializeComponent();

            strDataDescription = description;
            ThreadSafeOperations.SetText(lblDescription, strDataDescription, false);

            defaultProperties = properties;
            strOutputDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
            if (!ServiceTools.CheckIfDirectoryExists(strOutputDirectory))
            {
                strOutputDirectory = "";
            }

            dmData = dmDataToRepresent.Copy();

            
            ILScene scene = new ILScene();
            currSurfPlotCube = new ILPlotCube();
            currSurfPlotCube.TwoDMode = false;
            ILSurface surf;

            ILInArray<double> ilaDataMeshToShow = dmData.ToArray();
            if ((dmDataXcoord != null) && (dmDataYcoord != null))
            {
                ILInArray<double> ilaXvalues = dmDataXcoord.Row(0).ToArray();
                ILInArray<double> ilaYvalues = dmDataYcoord.Column(0).ToArray();
                surf = new ILSurface(ilaDataMeshToShow, ilaXvalues, ilaYvalues);
            }
            else
            {
                surf = new ILSurface(ilaDataMeshToShow);
            }
            
            surf.UseLighting = true;
            surf.Colormap = Colormaps.ILNumerics;
            surf.Children.Add(new ILColorbar());

            currSurfPlotCube.Children.Add(surf);
            currSurfPlotCube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), 1.2f) *
                                Matrix4.Rotation(new Vector3(0, 0, 1), Math.PI);
            currSurfPlotCube.Projection = Projection.Perspective;

            scene.Add(currSurfPlotCube);

            ilPanel1.Scene = scene;
        }





        




        




        private void btnSaveAsImage_Click(object sender, EventArgs e)
        {
            string filename = strOutputDirectory + DateTime.UtcNow.ToString("s").Replace(":", "-") + ".png";
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = strOutputDirectory;
            dlg.FileName = filename;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
            }


            ILScene currScene = ilPanel1.Scene;
            var drv = new ILGDIDriver(ilPanel1.Width, ilPanel1.Height, currScene);
            drv.Render();
            drv.BackBuffer.Bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        }





        private void Field3Drepresentation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }
    }
}
