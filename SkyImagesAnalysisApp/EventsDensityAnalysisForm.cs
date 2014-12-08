using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyImagesAnalyzerLibraries;

namespace SkyImagesAnalyzer
{
    public partial class EventsDensityAnalysisForm : Form
    {
        private Dictionary<string, object> defaultProperties = null;

        public List<PointD> lPointsArray = new List<PointD>();
        public int spaceDiscretization = 100;
        public DenseMatrix dmDensityMesh = null;
        public DenseMatrix dmDensityMeshXcoord = null;
        public DenseMatrix dmDensityMeshYcoord = null;

        public double minXval;
        public double maxXval;
        public double minYval;
        public double maxYval;

        public List<PointD> lPtdMarks = new List<PointD>();

        private imageConditionAndData currHeatMapData = null;




        public EventsDensityAnalysisForm(List<PointD> lInPoints, Dictionary<string, object> properties, int inSpaceDiscretization = 512)
        {
            InitializeComponent();

            defaultProperties = properties;
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
            // отфильтровать малые значения
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
