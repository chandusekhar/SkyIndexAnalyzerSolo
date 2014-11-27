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
using MathNet.Numerics.LinearAlgebra.Double;
using SkyImagesAnalyzerLibraries;



namespace AbstractFieldAnalysis
{
    public partial class MainForm : Form
    {
        private string currFileName = "";
        private imageConditionAndData currImgData = null;
        private Image<Bgr, byte> currImage = null;
        private LogWindow theLogWindow = null;
        private DenseMatrix dmLoadedData = null;
        private string fieldVarName = "dataMatrix";
        private double sectionGapWidth = 2.0d;
        private List<SectionDescription> sectionsList = new List<SectionDescription>();

        private Tuple<PointD, PointD> currSectionPoints = new Tuple<PointD, PointD>(PointD.nullPointD(), PointD.nullPointD());


        public MainForm()
        {
            InitializeComponent();
        }





        private void btnLoadFieldData_Click(object sender, EventArgs e)
        {
            if (tbFileName.Text != "")
            {
                if (File.Exists(tbFileName.Text))
                {
                    currFileName = tbFileName.Text;
                }
            }
            else
            {
                OpenFileDialog d1 = new OpenFileDialog();
                d1.DefaultExt = "NetCDF files *.nc | *.nc";
                DialogResult res = d1.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.Cancel) return;
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    ThreadSafeOperations.SetTextTB(tbFileName, d1.FileName, false);
                    currFileName = d1.FileName;
                }
            }

            dmLoadedData = NetCDFoperations.ReadDenseMatrixFromFile(currFileName, fieldVarName);

            currImgData = new imageConditionAndData(dmLoadedData, null);
            currImgData.currentColorSchemeRuler = new ColorSchemeRuler(currImgData.currentColorScheme);
            currImgData.UpdateColorSchemeRuler();
            currImgData.currentColorSchemeRuler.IsMarginsFixed = false;
            currImgData.UpdateColorSchemeRuler();
            currImage = new Image<Bgr, byte>(currImgData.dataRepresentingImageColored());
            ThreadSafeOperations.UpdatePictureBox(pbCurrResult, currImage.Bitmap, true);
            sectionsList = new List<SectionDescription>();
            currSectionPoints = new Tuple<PointD, PointD>(PointD.nullPointD(), PointD.nullPointD());
        }




        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }

        private void pbCurrResult_Click(object sender, EventArgs e)
        {
            if (currImage == null)
            {
                return;
            }
            PointD pt1 = ((PictureBox)sender).GetMouseEventPositionOnRealImage(e, currImage);
            //theLogWindow = ServiceTools.LogAText(theLogWindow, pt1.ToString()
            //                                    + Environment.NewLine
            //                                    + "======================================", true);
            if (currSectionPoints.Item1.IsNull)
            {
                currSectionPoints = new Tuple<PointD, PointD>(pt1, PointD.nullPointD());
            }
            else
            {
                currSectionPoints = new Tuple<PointD, PointD>(currSectionPoints.Item1, pt1);
                SectionDescription currSection = new SectionDescription(currSectionPoints.Item1, currSectionPoints.Item2,
                    true);
                currSection = currSection.TransformTillMargins(new Rectangle(0, 0, currImage.Width, currImage.Height));
                sectionsList.Add(currSection);
                RaisePaintEvent(null, null);
                currSectionPoints = new Tuple<PointD, PointD>(PointD.nullPointD(), PointD.nullPointD());
            }
        }




        private void MainForm_Resize(object sender, EventArgs e)
        {
            RaisePaintEvent(null, null);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (currImage != null)
            {
                Image<Bgr, byte> currShowingImage = currImage.Copy();
                foreach (SectionDescription sectionDescription in sectionsList)
                {
                    currShowingImage.Draw(
                        new LineSegment2DF(sectionDescription.p1.PointF(), sectionDescription.p2.PointF()),
                        new Bgr(Color.Black), 2);
                }

                ThreadSafeOperations.UpdatePictureBox(pbCurrResult, currShowingImage.Bitmap, true);

            }

        }








        private void btnGetFuncAlongSection_Click(object sender, EventArgs e)
        {
            foreach (SectionDescription sectionDescription in sectionsList)
            {
                PointD p1 = sectionDescription.p1;
                PointD p2 = sectionDescription.p2;
                bool fromMarginToMargin = false;
                //SectionRequestForm sectionRequestForm = new SectionRequestForm();
                //DialogResult res = sectionRequestForm.ShowDialog();
                //if (res == System.Windows.Forms.DialogResult.Cancel)
                //{
                //    return;
                //}

                //p1 = sectionRequestForm.retPt1;
                //p2 = sectionRequestForm.retPt2;
                //fromMarginToMargin = sectionRequestForm.fromMarginToMargin;
                SectionDescription currSection = new SectionDescription(p1, p2, true);
                //if (fromMarginToMargin)
                //{
                //    currSection =
                //        currSection.TransformTillMargins(new Rectangle(0, 0, currImgData.Width, currImgData.Height));
                //}

                LineDescription l1 = currSection.SectionLine;

                DenseMatrix dmValues = (DenseMatrix) currImgData.DmSourceData.Clone();
                DenseMatrix dmDistanceToLine = (DenseMatrix) currImgData.DmSourceData.Clone();
                dmDistanceToLine.MapIndexedInplace((row, col, dVal) =>
                {
                    PointD currPt = new PointD(col, row);

                    double dist = currPt.DistanceToLine(l1);
                    return dist;
                });

                List<Tuple<PointD, double>> dataArray = new List<Tuple<PointD, double>>();
                for (int row = 0; row < dmValues.RowCount; row++)
                {
                    for (int col = 0; col < dmValues.ColumnCount; col++)
                    {
                        if (dmDistanceToLine[row, col] <= sectionGapWidth)
                            dataArray.Add(new Tuple<PointD, double>(new PointD(col, row), dmValues[row, col]));

                    }
                }

                List<Tuple<double, double>> dataArrayRotated = dataArray.ConvertAll((tpl) =>
                {
                    DenseVector pointVector = l1.p0.ToVector(tpl.Item1);
                    double projection = pointVector*l1.directionVector;
                    return new Tuple<double, double>(projection, tpl.Item2);
                    //PointPolar ptp = new PointPolar(tpl.Item1 - new SizeD(l1.p0));
                    //double angleToSubtract = (new PointPolar(new PointD(l1.directionVector[0], l1.directionVector[1]))).Phi;
                    //ptp.Phi -= angleToSubtract;
                    ////ptp.CropAngle(true);
                    //if (ptp.Phi >= 0.0d) return new Tuple<double, double>(ptp.R, tpl.Item2);
                    //else return new Tuple<double, double>(-ptp.R, tpl.Item2);
                });


                double arrayMinPosition = dataArrayRotated.Min<Tuple<double, double>>(tpl1 => tpl1.Item1);
                double arrayMaxPosition = dataArrayRotated.Max<Tuple<double, double>>(tpl1 => tpl1.Item1);
                if (!fromMarginToMargin)
                {
                    DenseVector pointVector = l1.p0.ToVector(p2);
                    double projection = pointVector*l1.directionVector;
                    double p2DoublePosition = projection;

                    arrayMinPosition = Math.Min(0.0d, p2DoublePosition);
                    arrayMaxPosition = Math.Max(0.0d, p2DoublePosition);
                    dataArrayRotated.RemoveAll(tpl => ((tpl.Item1 < arrayMinPosition) || (tpl.Item1 > arrayMaxPosition)));
                }

                dataArrayRotated =
                    dataArrayRotated.ConvertAll<Tuple<double, double>>(
                        tpl => new Tuple<double, double>(tpl.Item1 - arrayMinPosition, tpl.Item2));

                dataArrayRotated.Sort((tpl1, tpl2) => tpl1.Item1.CompareTo(tpl2.Item1));

                FunctionRepresentationForm form1 = new FunctionRepresentationForm();
                form1.dvScatterXSpace = DenseVector.OfEnumerable(dataArrayRotated.ConvertAll<double>(tpl => tpl.Item1));
                form1.dvScatterFuncValues =
                    DenseVector.OfEnumerable(dataArrayRotated.ConvertAll<double>(tpl => tpl.Item2));
                form1.Show();
                form1.Represent();
            }
        }
    }
}
