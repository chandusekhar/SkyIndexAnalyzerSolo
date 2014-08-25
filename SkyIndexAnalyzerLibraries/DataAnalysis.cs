using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Solvers;
using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.Statistics;


namespace SkyIndexAnalyzerLibraries
{
    public class DataAnalysis
    {
        public static DenseVector ExponentialMovingAverage(DenseVector dvValues, int gap = 10, double smoothingParameter = 0.4d)
        {
            DenseVector dvOutValues = (DenseVector)dvValues.Clone();
            dvOutValues.MapIndexedInplace(new Func<int, double, double>((i, x) =>
            {
                DenseVector dvWeights = DenseVector.Create(1 + gap * 2, new Func<int, double>(j =>
                {
                    int k = j - gap;
                    if (i + k < 0) return 0.0d;
                    if (i + k >= dvOutValues.Count) return 0.0d;
                    return Math.Exp(-Math.Abs(k) * smoothingParameter);
                }));
                //dvWeights.MapIndexedInplace(new Func<int,double,double>((j, dVal) =>
                //{
                //    if (double.IsNaN(dvValues[])) return 0.0d;
                //}))
                double sum = dvWeights.Sum();
                dvWeights.MapInplace(new Func<double, double>(d => d / sum));

                double retVal = 0.0d;
                for (int n = 0; n < 1 + gap * 2; n++)
                {
                    int m = n - gap + i;
                    if ((m < 0) || (m >= dvOutValues.Count)) continue;
                    double weight = dvWeights[n];
                    retVal += weight * dvValues[m];
                }
                return retVal;
            }));
            return dvOutValues;
        }




        public static DenseMatrix ExponentialMovingAverage(DenseMatrix dmValues, int gap = 5, double smoothingParameter = 0.4d, bool countZeroes = false)
        {
            DenseMatrix dmOutValues = (DenseMatrix)dmValues.Clone();
            DenseMatrix dmWeights = DenseMatrix.Create(1 + 2 * gap, 1 + 2 * gap, new Func<int, int, double>((row, col) =>
            {
                double krow = (double)row - (double)gap;
                double kcol = (double)col - (double)gap;
                double r = Math.Sqrt(krow * krow + kcol * kcol);
                return Math.Exp((-r) * smoothingParameter);
            }));
            double wSum = dmWeights.Values.Sum();
            dmWeights.MapInplace(new Func<double, double>(d => d / wSum));


            dmOutValues.MapIndexedInplace(new Func<int, int, double, double>((row, col, x) =>
            {
                DenseMatrix dmValuesLocal = DenseMatrix.Create(1 + 2 * gap, 1 + 2 * gap, new Func<int, int, double>((maskRow, maskCol) =>
                {
                    int rowShifted = maskRow - gap + row;
                    int colShifted = maskCol - gap + col;

                    if ((rowShifted < 0) || (colShifted < 0) || (rowShifted >= dmOutValues.RowCount) || (colShifted >= dmOutValues.ColumnCount)) return 0.0d;
                    if (double.IsNaN(dmOutValues[rowShifted, colShifted])) return 0.0d;
                    if (dmOutValues[rowShifted, colShifted] == 0.0d) return 0.0d;
                    return dmOutValues[rowShifted, colShifted];
                }));

                DenseMatrix dmMaskLocal = (DenseMatrix)dmValuesLocal.Clone();
                dmMaskLocal.MapInplace(d => (d == 0.0d) ? (0.0d) : (1.0d));

                DenseMatrix dmWeightsLocal = (DenseMatrix)dmWeights.Clone();
                dmWeightsLocal = (DenseMatrix)dmWeightsLocal.PointwiseMultiply(dmMaskLocal);
                double localWeightsSum = dmWeightsLocal.Values.Sum();
                if (localWeightsSum == 0.0d) return double.NaN;
                dmWeightsLocal.MapInplace(new Func<double, double>(d => d / localWeightsSum));

                dmValuesLocal = (DenseMatrix)dmValuesLocal.PointwiseMultiply(dmWeightsLocal);

                double retVal = dmValuesLocal.Values.Sum();
                return retVal;
            }));


            return dmOutValues;
        }






        public static List<int> ContinousDataClustersWithCondition(List<double> dataList, Predicate<double> theCondition)
        {
            List<int> retList = new List<int>();
            List<double> data = new List<double>(dataList);
            List<int> markers = new List<int>();

            int currMarker = 1;
            if (theCondition(data[0])) markers.Add(currMarker);
            else markers.Add(0);
            for (int i = 1; i < data.Count; i++)
            {
                if (!theCondition(data[i]))
                {
                    markers.Add(0);
                    if (theCondition(data[i - 1])) currMarker++;
                }
                else markers.Add(currMarker);
            }
            if ((markers[0] != 0) && (markers[markers.Count - 1] != 0))
            {
                int markerToReplace = markers[markers.Count - 1];
                int markerToReplaceTo = markers[0];
                while (true)
                {
                    int idx = markers.FindIndex((x => x == markerToReplace));
                    if (idx == -1) break;
                    markers[idx] = markerToReplaceTo;
                }
            }

            for (int marker = 1; marker <= currMarker; marker++)
            {
                List<int> currMarkersList = markers.FindAll((x => x == marker));
                retList.Add(currMarkersList.Count);
            }

            return retList;
        }




        public static DenseVector NPolynomeApproximationLessSquareMethod(DenseVector dvDataValues, DenseVector dvSpace, DenseVector dvFixedValues = null, int polynomeOrder = 3)
        {
            double[] koeffOut = new double[polynomeOrder + 1];
            int pointsCount = dvSpace.Count;


            DenseMatrix dmFactorsA = DenseMatrix.Create(polynomeOrder + 1, polynomeOrder + 1,
                new Func<int, int, double>(
                    (row, column) =>
                    {
                        double sum = 0.0d;
                        for (int j = 0; j < pointsCount; j++)
                        {
                            sum += Math.Pow(dvSpace[j], (column + row));
                        }
                        return sum;
                    }));
            DenseVector dvFactorB = DenseVector.Create(polynomeOrder + 1,
                new Func<int, double>(
                    (index) =>
                    {
                        double sum = 0.0d;
                        for (int j = 0; j < pointsCount; j++)
                        {
                            sum += dvDataValues[j] * Math.Pow(dvSpace[j], index);
                        }
                        return sum;
                    }));



            if (dvFixedValues != null)
            {
                int conditionIndex = 0;
                foreach (Tuple<int, double> fixedValueTuple in dvFixedValues.GetIndexedEnumerator())
                {
                    int fixedValueIndex = fixedValueTuple.Item1;
                    double fixedValue = fixedValueTuple.Item2;
                    if (!double.IsNaN(fixedValue))
                    {
                        conditionIndex++;
                        //int modificationRowIndex = dmFactorsA.RowCount - conditionIndex;
                        int modificationRowIndex = conditionIndex - 1;
                        dmFactorsA.MapIndexedInplace(new Func<int, int, double, double>((row, column, dValue) =>
                        {
                            if (row == modificationRowIndex)
                            {
                                return Math.Pow(dvSpace[fixedValueIndex], column);
                            }
                            else return dValue;
                        }));

                        dvFactorB.MapIndexedInplace(new Func<int, double, double>((index, dValue) =>
                        {
                            if (index == modificationRowIndex)
                            {
                                return fixedValue;
                            }
                            else return dValue;
                        }));
                    }


                }
            }


            DenseVector dvResult = (DenseVector)dmFactorsA.LU().Solve(dvFactorB);


            return dvResult;
        }


        public static double PolynomeValue(DenseVector dvPolynomeKoeffs, double arg)
        {
            double sum = 0.0d;
            for (int i = 0; i < dvPolynomeKoeffs.Count; i++)
            {
                sum += dvPolynomeKoeffs[i] * Math.Pow(arg, i);
            }

            return sum;
        }





        public static DescriptiveStatistics StatsOfDataExcludingValues(DenseMatrix dmData, double markerExcludingValue = 0.0d)
        {
            DenseVector listOfData = DataVectorizedExcludingValues(dmData, markerExcludingValue);
            if (listOfData == null)
            {
                return null;
            }
            return new DescriptiveStatistics(listOfData.Values);
        }



        public static DenseVector DataVectorizedExcludingValues(DenseMatrix dmData, double markerExcludingValue = 0.0d)
        {
            List<double> listOfData = new List<double>(dmData.Values);
            listOfData.RemoveAll(x => x == markerExcludingValue);
            if (listOfData.Count == 0)
            {
                return null;
            }
            DenseVector dvOutData = DenseVector.OfEnumerable(listOfData);
            return dvOutData;
        }



        public static DenseVector DataVectorizedWithCondition(DenseMatrix dmData, Predicate<double> theCondition)
        {
            List<double> listOfData = new List<double>(dmData.Values);
            listOfData = listOfData.FindAll(theCondition);
            //listOfData.RemoveAll(theCondition);
            if (listOfData.Count == 0) return null;
            DenseVector dvOutData = DenseVector.OfEnumerable(listOfData);
            return dvOutData;
        }




        public static List<double> DataListedWithCondition(DenseMatrix dmData, Predicate<double> theCondition)
        {
            List<double> listOfData = new List<double>(dmData.Values);
            listOfData = listOfData.FindAll(theCondition);
            //listOfData.RemoveAll(theCondition);
            if (listOfData.Count == 0) return null;
            return listOfData;
        }



        public static DenseVector DataVectorizedWithCondition(DenseVector dvData, Predicate<double> theCondition)
        {
            List<double> listOfData = new List<double>(dvData.Values);
            listOfData = listOfData.FindAll(theCondition);
            if (listOfData.Count == 0) return null;
            DenseVector dvOutData = DenseVector.OfEnumerable(listOfData);
            return dvOutData;
        }


        public static List<double> DataListedWithCondition(DenseVector dvData, Predicate<double> theCondition)
        {
            List<double> listOfData = new List<double>(dvData.Values);
            listOfData = listOfData.FindAll(theCondition);
            if (listOfData.Count == 0) return null;
            return listOfData;
        }




        //public static DenseMatrix DecartToPolar(DenseMatrix dmData, PointD centerPoint, int angleGridNodesCount = 72,
        //    int distanceGridNodesCount = 30)
        //{
        //    double angleValueDiff = 2 * Math.PI / (angleGridNodesCount - 1);

        //    DenseMatrix dmDistances = DenseMatrix.Create(dmData.RowCount, dmData.ColumnCount,
        //        new Func<int, int, double>(
        //            (row, column) =>
        //            {
        //                double dy = row - centerPoint.Y;
        //                double dx = column - centerPoint.X;
        //                return Math.Sqrt(dx * dx + dy * dy);
        //            }));

        //    DenseMatrix dmAngles = DenseMatrix.Create(dmData.RowCount, dmData.ColumnCount,
        //        new Func<int, int, double>(
        //            (row, column) =>
        //            {
        //                double dx = (double)column - centerPoint.X;
        //                double dy = (double)row - centerPoint.Y;
        //                double r = dmDistances[row, column];
        //                double cosPhi = dx / r;
        //                double phi = Math.Acos(cosPhi);
        //                if (dy > 0) phi = 2.0d * Math.PI - phi;
        //                return phi;
        //            }));


        //    double distanceValueDiff = dmDistances.Values.Maximum() / (distanceGridNodesCount - 1);
        //    DenseMatrix dmOutData = DenseMatrix.Create(angleGridNodesCount, distanceGridNodesCount,
        //        new Func<int, int, double>(
        //            (row, column) =>
        //            {
        //                double minAngleValue = row * angleValueDiff;
        //                double maxAngleValue = (row + 1) * angleValueDiff;
        //                double minDistanceValue = column * distanceValueDiff;
        //                double maxDistanceValue = (column + 1) * distanceValueDiff;

        //                DenseMatrix dmDataTemp = (DenseMatrix)dmData.Clone();
        //                dmDataTemp.MapIndexedInplace(new Func<int, int, double, double>((row1, column1, dValue) =>
        //                {
        //                    double currAngle = dmAngles[row1, column1];
        //                    double currDistance = dmDistances[row1, column1];
        //                    if ((currAngle >= minAngleValue) && (currAngle < maxAngleValue) &&
        //                        (currDistance >= minDistanceValue) && (currDistance < maxDistanceValue)) return dValue;
        //                    else return 0.0d;
        //                }));

        //                DescriptiveStatistics stats = StatsOfDataExcludingValues(dmDataTemp, 0.0d);
        //                if (stats == null) return 0.0d;
        //                double dataMean = stats.Mean;
        //                double dataStdDev = stats.StandardDeviation;
        //                DenseVector dvDataFilteredByStdDev = DataVectorizedWithCondition(dmDataTemp,
        //                    x => (Math.Abs(x - dataMean) <= 2 * dataStdDev));
        //                return (dvDataFilteredByStdDev != null) ? (dvDataFilteredByStdDev.Minimum()) : (0.0d);
        //            }));

        //    return dmOutData;
        //}




        public static List<PointPolar> ListDecartToPolar(List<PointD> decartPointdList, PointD center, PointD zeroAnglePoint, out double zeroAngleValue)
        {
            double diffX = zeroAnglePoint.X - center.X;
            double diffY = zeroAnglePoint.Y - center.Y;
            double zeroAnglePointR = Math.Sqrt(diffX * diffX + diffY * diffY);
            double zeroAngle = 0.0d;
            if (zeroAnglePointR != 0.0d)
            {
                zeroAngle = Math.Acos(diffX / zeroAnglePointR);
                if (diffY >= 0) zeroAngle = 2.0d * Math.PI - zeroAngle;
            }

            zeroAngleValue = zeroAngle;

            List<PointPolar> retList = new List<PointPolar>();
            foreach (PointD curPointDecart in decartPointdList)
            {
                PointPolar polarPointD = new PointPolar();
                diffX = curPointDecart.X - center.X;
                diffY = curPointDecart.Y - center.Y;
                polarPointD.R = Math.Sqrt(diffX * diffX + diffY * diffY);
                polarPointD.Phi = Math.Acos(diffX / polarPointD.R);
                if (diffY >= 0) polarPointD.Phi = 2.0d * Math.PI - polarPointD.Phi;

                polarPointD.Phi -= zeroAngle;
                polarPointD.CropAngle(true);

                retList.Add(polarPointD);
            }
            return retList;
        }



        public static PointPolar PtdDecartToPolar(PointD decartPointD, PointD center, PointD zeroAnglePoint, out double zeroAngleValue)
        {
            double diffX = zeroAnglePoint.X - center.X;
            double diffY = zeroAnglePoint.Y - center.Y;
            double zeroAnglePointR = Math.Sqrt(diffX * diffX + diffY * diffY);
            double zeroAngle = 0.0d;
            if (zeroAnglePointR != 0.0d)
            {
                zeroAngle = Math.Acos(diffX / zeroAnglePointR);
                if (diffY >= 0) zeroAngle = 2.0d * Math.PI - zeroAngle;
            }

            zeroAngleValue = zeroAngle;

            PointPolar polarPointD = new PointPolar();
            diffX = decartPointD.X - center.X;
            diffY = decartPointD.Y - center.Y;
            polarPointD.R = Math.Sqrt(diffX * diffX + diffY * diffY);
            polarPointD.Phi = Math.Acos(diffX / polarPointD.R);
            if (diffY >= 0) polarPointD.Phi = 2.0d * Math.PI - polarPointD.Phi;

            polarPointD.Phi -= zeroAngle;
            polarPointD.CropAngle(true);

            return polarPointD;
        }




        public static PointPolar PtdDecartToPolar(PointD decartPointD, PointD center, PointD zeroAnglePoint)
        {
            double diffX = zeroAnglePoint.X - center.X;
            double diffY = zeroAnglePoint.Y - center.Y;
            double zeroAnglePointR = Math.Sqrt(diffX * diffX + diffY * diffY);
            double zeroAngle = 0.0d;
            if (zeroAnglePointR != 0.0d)
            {
                zeroAngle = Math.Acos(diffX / zeroAnglePointR);
                if (diffY >= 0) zeroAngle = 2.0d * Math.PI - zeroAngle;
            }

            PointPolar polarPointD = new PointPolar();
            diffX = decartPointD.X - center.X;
            diffY = decartPointD.Y - center.Y;
            polarPointD.R = Math.Sqrt(diffX * diffX + diffY * diffY);
            polarPointD.Phi = Math.Acos(diffX / polarPointD.R);
            if (diffY >= 0) polarPointD.Phi = 2.0d * Math.PI - polarPointD.Phi;

            polarPointD.Phi -= zeroAngle;
            polarPointD.CropAngle(true);

            return polarPointD;
        }




        public static PointPolar PtdDecartToPolar(PointD decartPointD, PointD center, double zeroAngle)
        {
            PointPolar polarPointD = new PointPolar();
            double diffX = decartPointD.X - center.X;
            double diffY = decartPointD.Y - center.Y;
            polarPointD.R = Math.Sqrt(diffX * diffX + diffY * diffY);
            polarPointD.Phi = Math.Acos(diffX / polarPointD.R);
            if (diffY >= 0) polarPointD.Phi = 2.0d * Math.PI - polarPointD.Phi;

            polarPointD.Phi -= zeroAngle;
            polarPointD.CropAngle(true);

            return polarPointD;
        }






        public static List<PointD> ListPolarToDecart(List<PointPolar> polarPointdList, PointD centerDecart, double zeroAngleInDecart)
        {
            List<PointD> retList = new List<PointD>();
            foreach (PointPolar curPointPolar in polarPointdList)
            {
                PointD decartPointD = new PointD();
                decartPointD.X = curPointPolar.R * Math.Cos(curPointPolar.Phi + zeroAngleInDecart) + centerDecart.X;
                decartPointD.Y = -curPointPolar.R * Math.Sin(curPointPolar.Phi + zeroAngleInDecart) + centerDecart.Y;
                retList.Add(decartPointD);
            }
            return retList;
        }




        public static PointD PtdPolarToDecart(PointPolar polarPointD, PointD centerDecart, double zeroAngleInDecart)
        {
            PointD decartPointD = new PointD();
            decartPointD.X = polarPointD.R * Math.Cos(polarPointD.Phi + zeroAngleInDecart) + centerDecart.X;
            decartPointD.Y = -polarPointD.R * Math.Sin(polarPointD.Phi + zeroAngleInDecart) + centerDecart.Y;
            return decartPointD;
        }




        public static PointD PtdPolarToDecart(PointPolar polarPointD, PointD centerDecart, PointD zeroAnglePointInDecart)
        {
            double diffX = zeroAnglePointInDecart.X - centerDecart.X;
            double diffY = zeroAnglePointInDecart.Y - centerDecart.Y;
            double zeroAnglePointR = Math.Sqrt(diffX * diffX + diffY * diffY);
            double zeroAngle = 0.0d;
            if (zeroAnglePointR != 0.0d)
            {
                zeroAngle = Math.Acos(diffX / zeroAnglePointR);
                if (diffY >= 0) zeroAngle += Math.PI;
            }

            PointD decartPointD = new PointD();
            decartPointD.X = polarPointD.R * Math.Cos(polarPointD.Phi + zeroAngle) + centerDecart.X;
            decartPointD.Y = -polarPointD.R * Math.Sin(polarPointD.Phi + zeroAngle) + centerDecart.Y;
            return decartPointD;
        }







        public static DenseMatrix DecartToPolar(DenseMatrix dmData, PointD centerPoint, int angleGridNodesCount = 144)
        {
            double angleValueDiff = 2 * Math.PI / (angleGridNodesCount - 1);



            DenseMatrix dmDistances = DenseMatrix.Create(dmData.RowCount, dmData.ColumnCount,
                new Func<int, int, double>(
                    (row, column) =>
                    {
                        double dy = row - centerPoint.Y;
                        double dx = column - centerPoint.X;
                        return Math.Sqrt(dx * dx + dy * dy);
                    }));
            double maxDistance = dmDistances.Values.Maximum();
            int distanceNodesCount = Convert.ToInt32(maxDistance) + 1;
            DenseMatrix dmCountOfElementsSummedToThePolarPointValue = DenseMatrix.Create(angleGridNodesCount, distanceNodesCount,
                new Func<int, int, double>((row, col) => 0.0d));


            DenseMatrix dmAngles = DenseMatrix.Create(dmData.RowCount, dmData.ColumnCount,
                new Func<int, int, double>(
                    (row, column) =>
                    {
                        double dx = (double)column - centerPoint.X;
                        double dy = (double)row - centerPoint.Y;
                        double r = dmDistances[row, column];
                        double cosPhi = dx / r;
                        double phi = Math.Acos(cosPhi);
                        if (dy > 0) phi = 2.0d * Math.PI - phi;
                        return phi;
                    }));


            //double distanceValueDiff = dmDistances.Values.Maximum() / (distanceGridNodesCount - 1);

            DenseMatrix dmOutData = DenseMatrix.Create(angleGridNodesCount, distanceNodesCount, new Func<int, int, double>((row, column) => 0.0d));
            //dmDistancesToSunCenter = DenseMatrix.Create(angleGridNodesCount, distanceNodesCount,
            //    new Func<int, int, double>(
            //        (angleRow, distanceCol) => (double)distanceCol));

            for (int decartRow = 0; decartRow < dmData.RowCount; decartRow++)
            {
                for (int decartCol = 0; decartCol < dmData.ColumnCount; decartCol++)
                {
                    double currAngle = dmAngles[decartRow, decartCol];
                    double currDistance = dmDistances[decartRow, decartCol];
                    int angleRow = Convert.ToInt32(currAngle / angleValueDiff);
                    int distanceCol = Convert.ToInt32(currDistance);
                    dmOutData[angleRow, distanceCol] += dmData[decartRow, decartCol];
                    if (dmData[decartRow, decartCol] != 0.0d) dmCountOfElementsSummedToThePolarPointValue[angleRow, distanceCol] += 1.0d;
                }
            }

            dmCountOfElementsSummedToThePolarPointValue.MapInplace(x => (x == 0.0d) ? (1.0d) : (x));

            dmOutData = (DenseMatrix)dmOutData.PointwiseDivide(dmCountOfElementsSummedToThePolarPointValue);
            dmOutData.MapInplace(new Func<double, double>(x => (x == 0.0d) ? (1.0d) : (x)));





            //сгладим
            foreach (Tuple<int, MathNet.Numerics.LinearAlgebra.Generic.Vector<double>> tplRowVectorAndIndex in dmOutData.RowEnumerator())
            {
                DenseVector currVector = (DenseVector)tplRowVectorAndIndex.Item2;
                currVector = ExponentialMovingAverage(currVector, 3, 0.4d);
                dmOutData.SetRow(tplRowVectorAndIndex.Item1, currVector.ToArray());
            }


            return dmOutData;
        }








        public static DenseMatrix PolarToDecart(DenseMatrix dmDataPolar, PointD sunCenter, int dimX, int dimY)
        {
            int distancePoints = dmDataPolar.ColumnCount;
            int anglePoints = dmDataPolar.RowCount;

            DenseMatrix dmDistances = DenseMatrix.Create(dimY, dimX,
                new Func<int, int, double>(
                    (row, column) =>
                    {
                        double dy = row - sunCenter.Y;
                        double dx = column - sunCenter.X;
                        return Math.Sqrt(dx * dx + dy * dy);
                    }));

            DenseMatrix dmAngles = DenseMatrix.Create(dimY, dimX,
                new Func<int, int, double>(
                    (row, column) =>
                    {
                        double dx = (double)column - sunCenter.X;
                        double dy = (double)row - sunCenter.Y;
                        double r = dmDistances[row, column];
                        double cosPhi = dx / r;
                        double phi = Math.Acos(cosPhi);
                        if (dy > 0) phi = 2.0d * Math.PI - phi;
                        return phi;
                    }));
            double rMax = dmDistances.Values.Max();


            DenseMatrix dmOutData = DenseMatrix.Create(dimY, dimX, new Func<int, int, double>((row, column) =>
            {
                double currAngle = dmAngles[row, column];
                double currDistance = dmDistances[row, column];
                int distanceColumnIndex = Convert.ToInt32((currDistance / rMax) * distancePoints);
                if (distanceColumnIndex >= distancePoints) distanceColumnIndex = distancePoints - 1;
                int angleRowIndex = Convert.ToInt32((currAngle / (2.0d * Math.PI)) * anglePoints);
                if (angleRowIndex >= anglePoints) angleRowIndex = anglePoints - 1;
                return dmDataPolar[angleRowIndex, distanceColumnIndex];

            }));


            return dmOutData;
        }





        /// <summary>
        /// Gets the local minimums distribution.
        /// </summary>
        /// <param name="dmFieldData">The dm field data.</param>
        /// <param name="dimensionNumber">The dimension number:
        /// 1 - rows (angle)
        /// 2 - columns (distance)
        /// </param>
        /// <returns>DenseMatrix.</returns>
        public static DenseMatrix GetLocalMinimumsDistribution(DenseMatrix dmFieldData, PointD sunCenterPoint, PointD imageCenterPoint, double imageRadius, int imageHeight, double imageCircleCropFactor = 0.9d, int dimensionNumber = 1)
        {
            DenseMatrix dmFieldminimumsData = DenseMatrix.Create(dmFieldData.RowCount, dmFieldData.ColumnCount,
                new Func<int, int, double>((row, column) => 0.0d));


            if (dimensionNumber == 1)
            {
                for (int i = 0; i < dmFieldData.RowCount; i++)
                {
                    bool itsTheCropCase = false;
                    //если направлени на кроп кадра - то не берем в расмотрение
                    double currentAngle = ((double)i / (double)dmFieldData.RowCount) * 2 * Math.PI;
                    if (Math.Tan(currentAngle) == 0.0d)
                    {
                        itsTheCropCase = false;
                    }
                    if (currentAngle < Math.PI)
                    {
                        //верхняя половина, смотрим направление на y=0.0d
                        double yMargin = 0.0d;
                        double xMargin = sunCenterPoint.X + (yMargin - sunCenterPoint.Y) / Math.Tan(currentAngle);
                        double dx = xMargin - imageCenterPoint.X;
                        double dy = yMargin - imageCenterPoint.Y;
                        if (Math.Sqrt(dx * dx + dy * dy) < imageRadius) itsTheCropCase = true;
                    }
                    else
                    {
                        //нижняя половина, смотрим направление на y=imageHeight
                        double yMargin = (double)imageHeight;
                        double xMargin = sunCenterPoint.X + (yMargin - sunCenterPoint.Y) / Math.Tan(currentAngle);
                        double dx = xMargin - imageCenterPoint.X;
                        double dy = yMargin - imageCenterPoint.Y;
                        if (Math.Sqrt(dx * dx + dy * dy) < imageRadius) itsTheCropCase = true;
                    }
                    //Если слишком близко к краю изображения - тоже исключаем. Минимум должен лежать не ближе 1/15



                    DenseMatrix dmSlicedDataMatrix = (DenseMatrix)dmFieldData.SubMatrix(i, 1, 0, dmFieldData.ColumnCount);
                    DenseVector dvSlicedDataVector = DenseVector.OfEnumerable(dmSlicedDataMatrix.Values);
                    dvSlicedDataVector.MapInplace(new Func<double, double>(x => (x == 0.0d) ? (1.0d) : (x)));
                    double minValue = dvSlicedDataVector.Minimum();
                    int minValueIndex = dvSlicedDataVector.MinimumIndex();

                    if (!itsTheCropCase) dmFieldminimumsData[i, minValueIndex] = minValue;
                    else continue;
                    //{
                    //    int theFirstIndex = dvSlicedDataVector.Count - 1;
                    //    //найдем крайний индекс со значимым значением поля
                    //    for (int j = dvSlicedDataVector.Count-1; j > 0; j--)
                    //    {
                    //        if (dvSlicedDataVector[j] < 1.0d)
                    //        {
                    //            theFirstIndex = dvSlicedDataVector.Count - 1 - theFirstIndex;
                    //            break;
                    //        }
                    //    }
                    //
                    //    if ((theFirstIndex - minValueIndex) < 2) continue;
                    //    else dmFieldminimumsData[i, minValueIndex] = minValue;
                    //}
                }
            }
            else if (dimensionNumber == 2)
            {
                for (int i = 0; i < dmFieldData.ColumnCount; i++)
                {
                    DenseMatrix dmSlicedDataMatrix = (DenseMatrix)dmFieldData.SubMatrix(0, dmFieldData.RowCount, i, 1);
                    DenseVector dvSlicedDataVector = DenseVector.OfEnumerable(dmSlicedDataMatrix.Values);
                    dvSlicedDataVector.MapInplace(new Func<double, double>(x => (x == 0.0d) ? (1.0d) : (x)));
                    double minValue = dvSlicedDataVector.Minimum();
                    int minValueIndex = dvSlicedDataVector.MinimumIndex();
                    dmFieldminimumsData[minValueIndex, i] = minValue;
                }
            }
            return dmFieldminimumsData;
        }



        public static Dictionary<string, DenseVector> SavGolFilter(DenseVector dvInputData, DenseVector timeSamplingSeconds, int nLeft, int nRight, int derivOrder = 0,
            int polynomesOrder = 6)
        {
            DenseVector dvRetData = null;
            List<double> listRetData = new List<double>();
            DenseVector dvRetTimeSampling = null;
            List<double> listRetTimeSampling = new List<double>();
            Dictionary<string, DenseVector> retDataDict = new Dictionary<string, DenseVector>();

            if (dvInputData.Count < nLeft + nRight + 1)
            {
                retDataDict.Add("time", timeSamplingSeconds);
                retDataDict.Add("values", dvInputData);
                return retDataDict;
            }

            //interpolate missing values

            DenseVector dvTimeSamplesVector = DenseVector.Create(timeSamplingSeconds.Count - 1,
                i => timeSamplingSeconds[i + 1] - timeSamplingSeconds[i]);

            DescriptiveStatistics stats = new DescriptiveStatistics(dvTimeSamplesVector);
            double timeSamplingIntervalsMean = stats.Mean;
            double timesamplingIntervalsStdDev = stats.StandardDeviation;

            for (int i = 0; i < timeSamplingSeconds.Count-1; i++)
            {
                double currTimeSample = timeSamplingSeconds[i + 1] - timeSamplingSeconds[i];
                if (currTimeSample/timeSamplingIntervalsMean > 2.0d)
                {
                    //надо интерполировать данные в этом интервале
                    int nIntervals = Convert.ToInt32(currTimeSample/timeSamplingIntervalsMean);
                    double newCurrTimeInterval = currTimeSample/(double) nIntervals;
                    List<double> listTimeValuesToInsert = new List<double>();
                    List<double> listDataValuesToInsert = new List<double>();

                    double startValue = dvInputData[i];
                    double endValue = dvInputData[i+1];
                    for (int j = 0; j < nIntervals-1; j++)
                    {
                        listRetTimeSampling.Add(timeSamplingSeconds[i] + j*newCurrTimeInterval);
                        listRetData.Add(startValue + ((endValue - startValue) / (double)(nIntervals)) * (double)j);
                    }


                    //dvRetTimeSampling = DenseVector.OfEnumerable(dvRetTimeSampling.Concat(listTimeValuesToInsert));
                    //dvRetData = DenseVector.OfEnumerable(dvRetData.Concat(listDataValuesToInsert));


                }
                else
                {
                    listRetTimeSampling.Add(timeSamplingSeconds[i]);
                    listRetData.Add(dvInputData[i]);
                }
            }
            listRetTimeSampling.Add(timeSamplingSeconds[timeSamplingSeconds.Count - 1]);
            listRetData.Add(dvInputData[dvInputData.Count - 1]);

            //dvRetTimeSampling = DenseVector.OfEnumerable(listRetTimeSampling);
            //dvRetData = DenseVector.OfEnumerable(listRetData);




            List<Tuple<int, int, Dictionary<int, double>>> listOfKoefficientsSets = new List<Tuple<int, int, Dictionary<int, double>>>();

            for (int i = 0; i < nLeft + nRight + 1; i++)
            {
                int currNLeft = (i <= nLeft) ? (i) : (nLeft);
                int currNRight = (i <= nLeft)?(nRight):(nLeft+nRight - i);

                Dictionary<int, double> currKoeffsDict = SavGolFilterKoefficients(currNLeft, currNRight, 0, 6);

                listOfKoefficientsSets.Add(new Tuple<int, int, Dictionary<int, double>>(currNLeft, currNRight,
                    currKoeffsDict));
            }

            dvRetTimeSampling = DenseVector.OfEnumerable(listRetTimeSampling);
            //dvRetData = DenseVector.Create(dvRetTimeSampling.Count, i =>
            //{
            //    int currNLeft = (i <= nLeft) ? (i) : (nLeft);
            //    int currNRight = (i >= dvRetTimeSampling.Count - nRight) ? (dvRetTimeSampling.Count - i - 1) : (nRight);
            //    Dictionary<int, double> currKoefficientsDict =
            //        listOfKoefficientsSets.Find(tpl => ((tpl.Item1 == currNLeft) && (tpl.Item2 == currNRight))).Item3;
            //    DenseVector currKoefficientsVect = ;

            //});
            dvRetData = DenseVector.OfEnumerable(listRetData);


            retDataDict.Add("time", dvRetTimeSampling);
            retDataDict.Add("values", dvRetData);
            return retDataDict;
        }



        public static Dictionary<int, double> SavGolFilterKoefficients(int nLeft, int nRight, int derivOrder = 0, int polynomesOrder = 6)
        {
            Dictionary<int, double> retDictKoefficients = new Dictionary<int, double>();

            // A[ij] = i^j, i = -nL...nR; j = 0...M, M - polynomes order
            DenseMatrix dmDesignMatrix = DenseMatrix.Create(nLeft + nRight + 1, polynomesOrder + 1, (r, c) =>
            {
                int currI = r - nLeft;
                return Math.Pow((double)currI, c);
            });

            dmDesignMatrix = (DenseMatrix)dmDesignMatrix.TransposeThisAndMultiply(dmDesignMatrix);
            dmDesignMatrix = (DenseMatrix)dmDesignMatrix.Inverse();

            DenseVector dvDesignMatrix0thRow = (DenseVector)dmDesignMatrix.Row(0);

            for (int n = -nLeft; n <= nRight; n++)
            {
                DenseVector dvCurrKoeffSummingVect = new DenseVector(dvDesignMatrix0thRow);
                dvCurrKoeffSummingVect.MapIndexedInplace((idx, dVal) =>
                {
                    return dVal * Math.Pow((double)n, (double)idx);
                });

                retDictKoefficients.Add(n, dvCurrKoeffSummingVect.Sum());
            }

            return retDictKoefficients;
        }


    }
}
