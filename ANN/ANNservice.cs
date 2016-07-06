using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using DataAnalysis;


namespace ANN
{
    public class ANNservice
    {
        public static IEnumerable<IEnumerable<double>> FeatureNormalization(IEnumerable<IEnumerable<double>> X,
            IEnumerable<double> means, IEnumerable<double> range)
        {
            IEnumerable<double> out_m = null;
            IEnumerable<double> out_r = null;

            return FeatureNormalization(X, means, range, out out_m, out out_r);
        }


        public static IEnumerable<IEnumerable<double>> FeatureNormalization(IEnumerable<IEnumerable<double>> X,
            IEnumerable<double> means, IEnumerable<double> range, out IEnumerable<double> out_means,
            out IEnumerable<double> out_range)
        {
            out_means = null;
            out_range = null;
            if (means != null)
            {
                out_means = means;
            }
            if (range != null)
            {
                out_range = range;
            }

            DenseMatrix dmX = DenseMatrix.OfRows(X);

            if (means == null)
            {
                means = dmX.MeansPerColumns();
                out_means = means;
            }
            if (range == null)
            {
                range = dmX.STDperColumns().Map(dVal => (dVal == 0.0d)?(1.0d):(dVal));
                out_range = range;
            }

            List<double> lMeansList = means.ToList();
            List<double> lRangeList = range.ToList();
            DenseMatrix dmMeans = DenseMatrix.Create(dmX.RowCount, dmX.ColumnCount, (r, c) => lMeansList[c]);
            DenseMatrix dmRange = DenseMatrix.Create(dmX.RowCount, dmX.ColumnCount, (r, c) => lRangeList[c]);
            dmRange.MapInplace(dVal => (dVal == 0.0d) ? (1.0d) : dVal);
            DenseMatrix dmXnormed = (DenseMatrix)((dmX - dmMeans).PointwiseDivide(dmRange));

            return dmXnormed.EnumerateColumns();
        }




        public static DenseMatrix FeatureNormalization(DenseMatrix dmX, IEnumerable<double> means,
            IEnumerable<double> range)
        {
            IEnumerable<double> out_m = null;
            IEnumerable<double> out_r = null;
            return FeatureNormalization(dmX, means, range, out out_m, out out_r);
        }



        public static DenseMatrix FeatureNormalization(DenseMatrix dmX, IEnumerable<double> means,
            IEnumerable<double> range, out IEnumerable<double> out_means, out IEnumerable<double> out_range)
        {
            out_means = null;
            out_range = null;
            if (means != null)
            {
                out_means = means;
            }
            if (range != null)
            {
                out_range = range;
            }

            if (means == null)
            {
                means = dmX.MeansPerColumns();
                out_means = means;
            }
            if (range == null)
            {
                range = dmX.STDperColumns().Map(dVal => (dVal == 0.0d) ? (1.0d) : (dVal));
                out_range = range;
            }

            List<double> lMeansList = means.ToList();
            List<double> lRangeList = range.ToList();
            DenseMatrix dmMeans = DenseMatrix.Create(dmX.RowCount, dmX.ColumnCount, (r, c) => lMeansList[c]);
            DenseMatrix dmRange = DenseMatrix.Create(dmX.RowCount, dmX.ColumnCount, (r, c) => lRangeList[c]);
            dmRange.MapInplace(dVal => (dVal == 0.0d) ? (1.0d) : dVal);
            DenseMatrix dmXnormed = (DenseMatrix)((dmX - dmMeans).PointwiseDivide(dmRange));

            return dmXnormed;
        }
    }
}
