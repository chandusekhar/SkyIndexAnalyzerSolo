using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SkyImagesAnalyzerLibraries
{
    public class HeatMap
    {
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

        public imageConditionAndData CurrHeatMapData
        {
            get
            {
                if (currHeatMapData == null)
                {
                    ConstructCurrHeatMapData();
                }
                return currHeatMapData;
            }
        }



        public List<PointD> lMarks = new List<PointD>();


        public HeatMap()
        {
        }


        public HeatMap(List<PointD> lInPoints, int inSpaceDiscretization = 100)
        {
            lPointsArray = new List<PointD>(lInPoints);
            spaceDiscretization = inSpaceDiscretization;
            
            minXval = lPointsArray.Min(pt => pt.X);
            maxXval = lPointsArray.Max(pt => pt.X);
            minYval = lPointsArray.Min(pt => pt.Y);
            maxYval = lPointsArray.Max(pt => pt.Y);

            CalculateDensityMesh();
        }



        private void CalculateDensityMesh()
        {
            dmDensityMesh = DenseMatrix.Create(spaceDiscretization, spaceDiscretization, (r, c) => 0.0d);
            dmDensityMeshXcoord = DenseMatrix.Create(spaceDiscretization, spaceDiscretization, (r, c) =>
            {
                return minXval + ((double) c/(double) spaceDiscretization)*(maxXval - minXval);
            });
            dmDensityMeshYcoord = DenseMatrix.Create(spaceDiscretization, spaceDiscretization, (r, c) =>
            {
                return minYval + ((double)r / (double)spaceDiscretization) * (maxYval - minYval);
            });

            double xSpaceDiscrete = (maxXval - minXval)/spaceDiscretization;
            double ySpaceDiscrete = (maxYval - minYval)/spaceDiscretization;

            foreach (PointD pt in lPointsArray)
            {
                int row = Convert.ToInt32((maxYval - pt.Y)/ySpaceDiscrete);
                int col = Convert.ToInt32((pt.X - minXval)/xSpaceDiscrete);
                col = Math.Min(col, spaceDiscretization - 1);
                row = Math.Min(row, spaceDiscretization - 1);

                dmDensityMesh[row, col] += 1.0d;
            }

            dmDensityMesh = (DenseMatrix)dmDensityMesh.Divide(dmDensityMesh.Values.Sum());
        }



        public void RepresentHeatMap()
        {
            if (currHeatMapData == null)
            {
                ConstructCurrHeatMapData();
            }

            ImageConditionAndDataRepresentingForm imgForm = new ImageConditionAndDataRepresentingForm(currHeatMapData, "data density");
            imgForm.Show();
        }




        private void ConstructCurrHeatMapData()
        {
            currHeatMapData = new imageConditionAndData(dmDensityMesh, null);
            currHeatMapData.currentColorScheme = new ColorScheme("");
            currHeatMapData.currentColorScheme.colorsArray[0] = new Emgu.CV.Structure.Bgr(Color.White);
            currHeatMapData.currentColorSchemeRuler = new ColorSchemeRuler(currHeatMapData.currentColorScheme);
            currHeatMapData.UpdateColorSchemeRuler();
            currHeatMapData.currentColorSchemeRuler.IsMarginsFixed = false;
            currHeatMapData.UpdateColorSchemeRuler();

            if (lPtdMarks.Count > 0)
            {
                UpdateHeatMapDataMarks();
            }
        }


        private void UpdateHeatMapDataMarks()
        {
            double xSpaceDiscrete = (maxXval - minXval)/spaceDiscretization;
            double ySpaceDiscrete = (maxYval - minYval)/spaceDiscretization;

            List<PointD> lPtdMarksAtImage = lPtdMarks.ConvertAll<PointD>(ptd =>
            {
                PointD ptNew = new PointD();

                ptNew.Y = (maxYval - ptd.Y)/ySpaceDiscrete;
                ptNew.X = (ptd.X - minXval)/xSpaceDiscrete;

                return ptNew;
            });

            currHeatMapData.lPtdMarks = lPtdMarksAtImage;
        }



        public void SetEqualMeasures()
        {
            minXval = Math.Min(minXval, minYval);
            minYval = minXval;
            maxXval = Math.Max(maxXval, maxYval);
            maxYval = maxXval;
            CalculateDensityMesh();
        }

    }
}
