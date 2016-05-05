using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using SkyImagesAnalyzerLibraries;

namespace MKLwrapper
{
    public class NonLinLeastSqProbWithBCandOutliersDetection<SpacePointsType> : NonLinLeastSqProbWithBC<SpacePointsType>
    {
        public IEnumerable<double> SolveOptimizationProblemWithOutliersFiltering(out List<List<double>> parametersHistory, out List<List<SpacePointsType>> filteredEventsListsHistory, double precision = 1.0e-8d)
        {
            parametersHistory = new List<List<double>>();
            filteredEventsListsHistory = new List<List<SpacePointsType>>();
            int startingEventsSetLength = mEventsSpaceVector.Count();
            int prevEventsSetLength = mEventsSpaceVector.Count();
            int newEventsSetLength = mEventsSpaceVector.Count();
            int epoch = 0;
            bool success = false;
            while (!success)
            {
                nParametersSpacePoint = SolveOptimizationProblem(precision);

                parametersHistory.Add(nParametersSpacePoint.ToList());


                // find outliers indexes
                List<double> funcDevVector = this.ObjectiveDeviations(nParametersSpacePoint).ToList();

                #region debugging presentations
#if DEBUG
                //HistogramDataAndProperties hist = new HistogramDataAndProperties(
                //    DenseVector.OfEnumerable(funcDevVector), 50);
                //HistogramCalcAndShowForm histForm = new HistogramCalcAndShowForm("", null);
                //histForm.HistToRepresent = hist;
                //histForm.Represent();
                //histForm.SaveToImage("D:\\_gulevlab\\SkyImagesAnalysis_appData\\RV-ANS-31-test\\hist_" + epoch.ToString("D3") + ".jpg");
#endif
                #endregion debugging presentations

                List<bool> lIsOutlier = mEventsSpaceVector.ToList().ConvertAll<bool>(val => false);
                Normal normDistrib = Normal.Estimate(funcDevVector, null);
                lIsOutlier = funcDevVector.ConvertAll<bool>(devVal => normDistrib.Density(devVal) <= 0.2d* normDistrib.Density(normDistrib.Mean));


                #region DEBUG: manually set 0.03 fraction of set to true
                //List<int> lOutliersIndexes = RandPerm(mEventsSpaceVector.Count()).ToList();
                //int maxOutlierPosition = Convert.ToInt32(Math.Floor(0.03d*lOutliersIndexes.Count) + 1);
                //for (int i = 0; i < maxOutlierPosition; i++)
                //{
                //    lIsOutlier[lOutliersIndexes[i]] = true;
                //}
                #endregion DEBUG: manually set 0.03 fraction of set to true

                // filter outliers
                mEventsSpaceVector = new List<SpacePointsType>(mEventsSpaceVector.Where((val, idx) => !lIsOutlier[idx]));
                mFittingValuesVector = new List<double>(mFittingValuesVector.Where((val, idx) => !lIsOutlier[idx]));

                filteredEventsListsHistory.Add(mEventsSpaceVector.ToList());

                newEventsSetLength = mEventsSpaceVector.Count();

                if ( (double)Math.Abs(newEventsSetLength- prevEventsSetLength)/(double)prevEventsSetLength <= 0.005)
                {
                    success = true;
                    epoch++;

                    #region debugging presentations
#if DEBUG
                    //hist = new HistogramDataAndProperties(DenseVector.OfEnumerable(funcDevVector), 50);
                    //histForm.HistToRepresent = hist;
                    //histForm.Represent();
                    //histForm.SaveToImage("D:\\_gulevlab\\SkyImagesAnalysis_appData\\RV-ANS-31-test\\hist_" + epoch.ToString("D3") + ".jpg");
#endif
                    #endregion debugging presentations
                }
                prevEventsSetLength = newEventsSetLength;
                epoch++;
            }



            return nParametersSpacePoint;
        }

        





        public static IEnumerable<int> RandPerm(int indexesCount = 100)
        {
            int[] array = new int[indexesCount];
            for (int i = 0; i < indexesCount; i++)
            {
                array[i] = i;
            }

            Random random = new Random();
            int n = array.Count();
            while (n > 1)
            {
                n--;
                int i = random.Next(n);
                int temp = array[i];
                array[i] = array[n];
                array[n] = temp;
            }
            return array;
        }

    }

}
