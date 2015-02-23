using System;
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

namespace ShowNetCDFField3D
{
    public partial class MainForm : Form
    {
        private string dataFileName = "";
        private DenseMatrix dmDataToShow = null;
        private LogWindow theLogWindow = null;
        private delegate void DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;


        public MainForm()
        {
            InitializeComponent();

            m_DelegateOpenFile = this.OpenFile;
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array filesArray = (Array)e.Data.GetData(DataFormats.FileDrop);
                string filename = (string)filesArray.GetValue(0);
                if (Path.GetExtension(filename) != ".nc")
                {
                    return;
                }

                if (filesArray != null)
                {
                    this.BeginInvoke(m_DelegateOpenFile, new Object[] { filename });
                    this.Activate();
                }
            }
            catch (Exception exc1)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Ошибка при обработке Drag&Drop: " + Environment.NewLine + exc1.Message, true);
            }
        }





        private void OpenFile(string filelistmember)
        {
            dataFileName = filelistmember;

            try
            {
                dmDataToShow = NetCDFoperations.ReadDenseMatrixFromFile(dataFileName);
            }
            catch (Exception ex)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "coundn`t load data from file:" + Environment.NewLine + dataFileName);
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "exception description:" + Environment.NewLine + ex.Message);
                return;
            }

            if (dmDataToShow != null)
            {
                RepresentData();
            }
        }




        private void RepresentData()
        {
            ThreadSafeOperations.SetText(lblFileName, dataFileName, false);

            ILInArray<double> dataValues = dmDataToShow.ToArray();

            ILScene scene = new ILScene();
            ILPlotCube currSurfPlotCube = new ILPlotCube();
            currSurfPlotCube.TwoDMode = false;
            
            ILSurface surf = new ILSurface(dataValues);

            surf.UseLighting = true;
            surf.Colormap = Colormaps.Jet;
            surf.Children.Add(new ILColorbar());

            currSurfPlotCube.Children.Add(surf);
            currSurfPlotCube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), 1.2f) *
                                Matrix4.Rotation(new Vector3(0, 0, 1), Math.PI);
            currSurfPlotCube.Projection = Projection.Perspective;

            scene.Add(currSurfPlotCube);



            //scene.First<ILSurface>().MouseClick += (s, arg) =>
            //{
            //    // we start at the mouse event target -> this will be the 
            //    // surface group node (the parent of "Fill" and "Wireframe")
            //    var group = arg.Target.Parent;
            //    if (group != null)
            //    {
            //        // walk up to the next camera node 
            //        Matrix4 trans = group.Transform;
            //        while (!(group is ILCamera) && group != null)
            //        {
            //            group = group.Parent;
            //            // collect all nodes on the path up
            //            trans = group.Transform * trans;
            //        }
            //        if (group != null && (group is ILCamera))
            //        {
            //            // convert args.LocationF to world coords
            //            // The Z coord is not provided by the mouse! -> choose arbitrary value
            //            var pos = new Vector3(arg.LocationF.X * 2 - 1, arg.LocationF.Y * -2 + 1, 0);
            //            // invert the matrix.
            //            trans = Matrix4.Invert(trans);
            //            // trans now converts from the world coord system (at the camera) to 
            //            // the local coord system in the 'target' group node (surface). 
            //            // In order to transform the mouse (viewport) position, we 
            //            // left multiply the transformation matrix.
            //            pos = trans * pos;
            //            // view result in the window title
            //            //Text = "Model Position: " + pos.ToString();
            //            ThreadSafeOperations.SetText(lblStatus, pos.ToString(), false);
            //        }
            //    }
            //};




            ilPanel1.Scene = scene;
        }





        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
            }
        }



    }
}
