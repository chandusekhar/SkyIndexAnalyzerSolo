using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyIndexAnalyzerLibraries;
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

namespace CyclonesAnalysis
{
    public partial class MainProcessingForm : Form
    {
        private delegate void DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;
        private LogWindow theLogWindow = null;
        private DenseMatrix dmSourceData = null;


        public MainProcessingForm()
        {
            InitializeComponent();
            m_DelegateOpenFile = this.OpenFile;
        }

        private void MainProcessingForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                OpenFile(Openfile.FileName);
                //pictureBox1.Image = My_Image.ToBitmap();
            }
        }


        private void OpenFile(string filelistmember)
        {
            string dataFileFileName = filelistmember;
            Image<Gray, byte> anImage = null;

            dmSourceData = NetCDFoperations.ReadDenseMatrixFromFile(dataFileFileName, "PRMSL_0");
            anImage = ImageProcessing.grayscaleImageFromDenseMatrix(dmSourceData);

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "Размер сетки: " + anImage.Width + "x" + anImage.Height + "px");


            ThreadSafeOperations.UpdatePictureBox(pb1, anImage.Bitmap, true);

            FileInfo finfo = new FileInfo(dataFileFileName);
            string shortfname1 = finfo.Name;
        }

        
        
        
        
        
        
        private void MainProcessingForm_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array FilesArray = (Array)e.Data.GetData(DataFormats.FileDrop);

                if (FilesArray != null)
                {
                    foreach (string FileName1 in FilesArray)
                    {
                        if (FileName1.Substring(FileName1.Length - 3) != ".nc") continue;
                        BeginInvoke(m_DelegateOpenFile, new Object[] { FileName1 });
                        Activate();
                        break;
                    }

                }
            }
            catch (Exception exc1)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Ошибка при обработке Drag&Drop: " + exc1.Message);
            }
        }

        private void MainProcessingForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void MainProcessingForm_Resize(object sender, EventArgs e)
        {
            RaisePaintEvent(null, null);
        }

        private void MainProcessingForm_Paint(object sender, PaintEventArgs e)
        {
            if (dmSourceData != null)
            {
                Image<Gray, byte> anImage = ImageProcessing.grayscaleImageFromDenseMatrix(dmSourceData);
                ThreadSafeOperations.UpdatePictureBox(pb1, anImage.Bitmap, true);
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> binMaskImage = new Image<Gray, byte>(dmSourceData.ColumnCount, dmSourceData.RowCount,
                new Gray(1));
            imageConditionAndData sourceData = new imageConditionAndData(dmSourceData, binMaskImage);
            sourceData.currentColorScheme = new ColorScheme("");
            sourceData.currentColorSchemeRuler = new ColorSchemeRuler(sourceData);
            sourceData.UpdateColorSchemeRuler();
            ImageConditionAndDataRepresentingForm presForm = new ImageConditionAndDataRepresentingForm(sourceData, "Pressure data");
            presForm.Show();

            //выделить "циклоны" и "антициклоны"
            


        }


    }
}
