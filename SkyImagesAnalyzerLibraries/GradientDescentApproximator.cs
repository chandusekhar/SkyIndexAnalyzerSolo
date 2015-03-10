using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using ILNumerics;
using MathNet.Numerics.Interpolation;

namespace SkyImagesAnalyzerLibraries
{
    public class GradientDescentApproximator
    {
        public DenseVector dvDataValues;
        public DenseVector dvSpace;
        public Func<DenseVector, double, double> approxFunc;
        public System.ComponentModel.BackgroundWorker SelfWorker = null;
        private DenseVector dvWeights = null;
        private LogWindow theSelfLogWindow;


        /// <summary>
        /// Conditions on parameters with the zero-equality condition
        /// </summary>
        public List<Func<DenseVector, double, double>> parametersConditionsEquals0 = new List<Func<DenseVector, double, double>>();
        /// <summary>
        /// Conditions on parameters with the less-than-zero condition
        /// </summary>
        public List<Func<DenseVector, double, double>> parametersConditionsLessThan0 = new List<Func<DenseVector, double, double>>();
        //public double approxFuncMaxValue_FAKE = 1.0d;

        private DenseVector dvParametersScale = null;
        private List<double> MinimizingFunctionValues = new List<double>();

        private List<PointD> pointsList = new List<PointD>();
        public Func<DenseVector, PointD, double> approxFuncPt;

        private int calculationType = 0;
        // 0 - обычная функция
        // 1 - набор точек на плоскости, не обязательно однозначное соответствие x => y

        Random rnd = new Random();



        public GradientDescentApproximator(DenseVector dvInputDataValues, DenseVector dvInputSpace, Func<DenseVector, double, double> inputApproxFunc)
        {
            dvDataValues = dvInputDataValues.Copy();
            dvSpace = dvInputSpace.Copy();
            approxFunc = inputApproxFunc;
            calculationType = 0;
        }


        public GradientDescentApproximator(List<PointD> inputPointsList, Func<DenseVector, PointD, double> inputApproxFunc)
        {
            pointsList = new List<PointD>(inputPointsList);
            approxFuncPt = inputApproxFunc;
            calculationType = 1;
        }




        public DenseVector DvWeights
        {
            get { return dvWeights; }
            set
            {
                dvWeights = value;
                //отнормируем, если это не так
                double theSum = dvWeights.Sum();
                if (theSum == 0.0d) dvWeights = null;
                else if (theSum != 1.0d)
                {
                    dvWeights.MapInplace(x => x / theSum);
                }
            }
        }



        private void updateParametersScale(DenseVector dvActualParametersValues)
        {
            if (dvParametersScale == null)
                dvParametersScale = DenseVector.Create(dvActualParametersValues.Count,
                    i => Math.Abs(dvActualParametersValues[i]));
            else
            {
                dvParametersScale.MapIndexedInplace((i, dVal) =>
                {
                    if (Math.Abs(dvActualParametersValues[i]) > dVal) return Math.Abs(dvActualParametersValues[i]);
                    else return dVal;
                });
            }
        }







        /// <summary>
        /// Approximations using gradient descent method. The points list case.
        /// </summary>
        /// <param name="dvInitioalParametersValues">The dv initioal parameters values.</param>
        /// <param name="dvInitialParametersIncrement">The dv initial parameters increment.</param>
        /// <param name="maxRelativeError">The maximum relative error.</param>
        /// <returns>DenseVector.</returns>
        public DenseVector ApproximationGradientDescent2DPt(DenseVector dvInitioalParametersValues, DenseVector dvInitialParametersIncrement, double maxRelativeError = 0.0001d)
        {
            DenseVector currentParametersValues = dvInitioalParametersValues.Copy();
            updateParametersScale(currentParametersValues);

            DenseVector dvCurrentParametersIncrement = dvInitialParametersIncrement.Copy();
            DenseVector dvNewParametersIncrements;
            double stepFollowingGradKoeff = 1.0d;
            double relativeMinimizingValueDiffIncrement = 1.0d;
            double previousRelativeMinimizingValueIncrement = 1.0d;
            DenseVector nextParametersValues = currentParametersValues.Copy();

            while (Math.Abs(relativeMinimizingValueDiffIncrement) > maxRelativeError)
            {
                stepFollowingGradKoeff = stepFollowingGradKoeff * 2.0d;
                DenseVector dvCurrentGrad = GradOfEvaluatingValue(
                    DeviationsSquaredSumPt,
                    currentParametersValues,
                    dvCurrentParametersIncrement,
                    out dvNewParametersIncrements);
                dvCurrentParametersIncrement = dvNewParametersIncrements.Copy();

                nextParametersValues = currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad;

                DenseVector dvNextGrad = GradOfEvaluatingValue(
                    DeviationsSquaredSumPt,
                    nextParametersValues,
                    dvCurrentParametersIncrement,
                    out dvNewParametersIncrements);
                dvCurrentParametersIncrement = dvNewParametersIncrements.Copy();

                DenseVector gradIncrement = dvNextGrad - dvCurrentGrad;
                double gradRelativeIncrement = gradIncrement.Abs() / dvCurrentGrad.Abs();


                while (gradRelativeIncrement > 0.1d)
                {
                    //stepFollowingGradKoeff adjusting
                    double stepFollowingGradKoeffPrev = stepFollowingGradKoeff;
                    stepFollowingGradKoeff = stepFollowingGradKoeff / 2.0d;
                    if (stepFollowingGradKoeff == 0.0d)
                    {
                        stepFollowingGradKoeff = stepFollowingGradKoeffPrev * 2;
                        break;
                    }


                    nextParametersValues = currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad;

                    dvNextGrad = GradOfEvaluatingValue(
                        dvParametersVector =>
                        {
                            //здесь должна быть функция, от которой берем градиент
                            return DeviationsSquaredSumPt(dvParametersVector);
                        },
                        nextParametersValues,
                        dvCurrentParametersIncrement,
                        out dvNewParametersIncrements);
                    dvCurrentParametersIncrement = dvNewParametersIncrements.Copy();

                    gradIncrement = dvNextGrad - dvCurrentGrad;
                    gradRelativeIncrement = gradIncrement.Abs() / dvCurrentGrad.Abs();
                }

                #region Проверка условий

                //if (parametersConditions.Count > 0)
                //{
                //    DenseVector modifiedParametersIncrement = stepFollowingGradKoeff * dvCurrentGrad;
                //    bool conditionsTrue = false;
                //    int testimgParamenerIndex = 0;
                //    while (!conditionsTrue)
                //    {

                //        if (testimgParamenerIndex > 0)
                //            modifiedParametersIncrement[testimgParamenerIndex - 1] =
                //                -modifiedParametersIncrement[testimgParamenerIndex - 1];
                //        modifiedParametersIncrement[testimgParamenerIndex] =
                //            -modifiedParametersIncrement[testimgParamenerIndex];
                //        nextParametersValues = currentParametersValues - modifiedParametersIncrement;

                //        conditionsTrue = true;
                //        foreach (Func<DenseVector, bool> condition in parametersConditions)
                //        {
                //            conditionsTrue = conditionsTrue && condition(nextParametersValues);
                //        }
                //    }

                //}

                #endregion Проверка условий

                double currentMinimizingValue = DeviationsSquaredSumPt(currentParametersValues);
                //DeviationsSquaredSumRelative(currentParametersValues);
                double nextMinimizingValue = DeviationsSquaredSumPt(currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad);
                //DeviationsSquaredSumRelative(approxFunc, (currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad));
                double currentRelativeIncrement = (nextMinimizingValue - currentMinimizingValue) / currentMinimizingValue;

                //relativeMinimizingValueDiffIncrement = (currentRelativeIncrement - previousRelativeMinimizingValueIncrement) / currentRelativeIncrement;
                relativeMinimizingValueDiffIncrement = currentRelativeIncrement;

                //previousRelativeMinimizingValueIncrement = currentRelativeIncrement;
                currentParametersValues = nextParametersValues;

                //updateParametersScale(currentParametersValues);

                if (SelfWorker != null)
                {
                    MinimizingFunctionValues.Add(currentMinimizingValue);
                    List<double> subSample = MinimizingFunctionValues;
                    if (MinimizingFunctionValues.Count > 100)
                        subSample = MinimizingFunctionValues.GetRange(MinimizingFunctionValues.Count - 101, 100);
                    DescriptiveStatistics stats = new DescriptiveStatistics(subSample);

                    //Thread.Sleep(10);
                    Tuple<DenseVector, double> theObject = new Tuple<DenseVector, double>(currentParametersValues, stats.StandardDeviation / stats.Mean);
                    SelfWorker.ReportProgress(50, theObject);

                    if (SelfWorker.CancellationPending)
                    {
                        break;
                    }
                }
                else
                {
                    //theSelfLogWindow = ServiceTools.LogAText(theSelfLogWindow, densevectorToString(nextParametersValues), false);
                }
            }


            if (SelfWorker == null)
            {
                //theSelfLogWindow = ServiceTools.LogAText(theSelfLogWindow, "DONE" + Environment.NewLine + densevectorToString(nextParametersValues), false);
            }

            return nextParametersValues;
        }






        /// <summary>
        /// Squared deviations sum. points list case.
        /// </summary>
        /// <param name="currentParametersSpacePoint">The current parameters space point.</param>
        /// <returns>System.Double.</returns>
        private double DeviationsSquaredSumPt(DenseVector currentParametersSpacePoint)
        {
            DenseVector dvApproxFuncValues;
            if (dvWeights == null)
            {
                dvApproxFuncValues = DenseVector.Create(pointsList.Count, ix =>
                {
                    return approxFuncPt(currentParametersSpacePoint, pointsList[ix]);
                });
            }
            else
            {
                dvApproxFuncValues = DenseVector.Create(pointsList.Count, ix =>
                {
                    return approxFuncPt(currentParametersSpacePoint, pointsList[ix]);
                });
                dvApproxFuncValues = (DenseVector)dvApproxFuncValues.PointwiseMultiply(dvWeights);
            }

            return dvApproxFuncValues.Abs();
        }








        /// <summary>
        /// Deviations squared sum. Сумма ошибок отклонений аппроксимирующей функции от известных значений
        /// </summary>
        /// <param name="dvDataValues">The data values. Значения аппроксимируемой функции</param>
        /// <param name="dvSpace">The space. Значения независимой переменной</param>
        /// <param name="approxFunc">The approximation function. Аппроксимирующая функция с фиксированными параметрами, заданными вектором параметров.
        /// Значения вычисляются над пространством независимой переменной</param>
        /// <param name="currentParametersSpacePoint">The current parameters space point.
        /// Тот самый фиксированный вектор параметров, используемый для вычисления значений аппроксимирующей функции над пространсовм независимой переменной</param>
        /// <returns>System.Double. Значение абсолютного </returns>
        private static double DeviationsSquaredSumRelative(
            DenseVector dvDataValues,
            DenseVector dvSpace,
            Func<DenseVector, double, double> approxFunc,
            DenseVector currentParametersSpacePoint,
            DenseVector dvWeights)
        {
            double distanceRelative = 0.0d;
            if (dvWeights == null)
            {
                DenseVector dvApproxFuncValues = DenseVector.Create(dvSpace.Count, ix =>
                {
                    return approxFunc(currentParametersSpacePoint, dvSpace[ix]);
                });
                DenseVector dvDistanceAbs = dvApproxFuncValues - dvDataValues;
                distanceRelative = dvDistanceAbs.Abs() / dvDataValues.Abs();
            }
            else
            {
                DenseVector dvApproxFuncValues = DenseVector.Create(dvSpace.Count, ix =>
                {
                    return approxFunc(currentParametersSpacePoint, dvSpace[ix]);
                });
                DenseVector dvDistanceAbs = dvApproxFuncValues - dvDataValues;
                dvDistanceAbs.MapInplace(d => Math.Abs(d));
                DenseVector dvDistanceRelative = (DenseVector)(dvDistanceAbs / dvDataValues.Map(Math.Abs));
                dvDistanceRelative = (DenseVector)dvDistanceRelative.PointwiseMultiply(dvWeights);
                distanceRelative = dvDistanceRelative.Abs();
            }
            return distanceRelative;
        }








        private DenseVector DeviationsArray_ILOptimizer(
            DenseVector dvDataValues,
            DenseVector dvSpace,
            Func<DenseVector, double, double> approxFunc,
            DenseVector currentParametersSpacePoint,
            DenseVector dvWeights)
        {
            DenseVector dvApproxFuncValues = DenseVector.Create(dvSpace.Count, 0.0d);

            //if (parametersConditions.Sum(func => (func(currentParametersSpacePoint)) ? (1) : (0)) <
            //    parametersConditions.Count)
            //{
            //    // не все условия из списка выполняются - значит, за пределами допустимой области
            //    dvApproxFuncValues = DenseVector.Create(dvSpace.Count, approxFuncMaxValue_FAKE);
            //    DenseVector dvDistanceAbs = dvApproxFuncValues - dvDataValues;
            //    return dvDistanceAbs;
            //}
            //else
            //{
                dvApproxFuncValues = DenseVector.Create(dvSpace.Count,
                    ix => approxFunc(currentParametersSpacePoint, dvSpace[ix]));
                DenseVector dvDistanceAbs = dvApproxFuncValues - dvDataValues;

                if (dvWeights != null)
                {
                    dvDistanceAbs = (DenseVector)dvDistanceAbs.PointwiseMultiply(dvWeights);
                }
                return dvDistanceAbs;
            //}
        }







        private double RandomWithLimits(double rndMin, double rndMax)
        {
            return rndMin + rnd.NextDouble() * (rndMax - rndMin);
        }







        public DenseVector Approximation_ILOptimizer(DenseVector dvInitialParametersValues,
                                                    double maxRelativeError = 1.0e-8d)
        {
            // найдем некое максимальное значение функции - для формирования высокого градиента
            // на границе запрещенных областей по параметрам
            // DenseVector dvInitFuncValues = (DenseVector) dvSpace.Map(x => approxFunc(dvInitialParametersValues, x));
            // approxFuncMaxValue_FAKE = dvInitFuncValues.Max();
            
            Optimization.ObjectiveFunction<double> devFunc = (parametersPoint) =>
            {
                using (ILScope.Enter(parametersPoint))
                {
                    DenseVector dvParametersPoint = DenseVector.OfEnumerable(parametersPoint);
                    DenseVector dvDevValues =
                        DeviationsArray_ILOptimizer(dvDataValues, dvSpace, approxFunc, dvParametersPoint,
                            dvWeights);
                    return (ILRetArray<double>)(dvDevValues.ToArray());
                }
            };


            ILArray<double> initParameters = dvInitialParametersValues.ToArray();
            double[] resDoub;
            using (ILScope.Enter(initParameters))
            {
                ILRetArray<double> result = Optimization.leastsq_pdl(
                                                                    devFunc,
                                                                    initParameters,
                                                                    tol: maxRelativeError);
                resDoub = result.ToArray();
            }

            return DenseVector.OfEnumerable(resDoub);
        }







        public DenseVector Approximation_ILOptimizerConstrained(DenseVector dvInitialParametersValues,
                                                                double maxRelativeError = 1.0e-8d)
        {
            Optimization.ObjectiveFunction<double> devFunc = (parametersPoint) =>
            {
                using (ILScope.Enter(parametersPoint))
                {
                    DenseVector dvParametersPoint = DenseVector.OfEnumerable(parametersPoint);
                    DenseVector dvDevValues =
                        DeviationsArray_ILOptimizer(dvDataValues, dvSpace, approxFunc, dvParametersPoint,
                            dvWeights);
                    return (ILRetArray<double>)(dvDevValues.ToArray());
                }
            };

            //Optimization.ObjectiveFunction<double> eqFunc =
            //    (parametersPoint) => (ILRetArray<double>) (DenseVector.Create(dvSpace.Count, 0.0d).ToArray());
            Optimization.ObjectiveFunction<double> eqFunc = (parametersPoint) => 0.0d;
            if (parametersConditionsEquals0.Count > 0)
            {
                eqFunc = (parametersPoint) =>
                {
                    using (ILScope.Enter(parametersPoint))
                    {
                        // посольку ограничений на равенство нулю может быть несколько
                        // одновременно они все обратятся в ноль только если сумма квадратов будет равна нулю
                        // значит, итоговая функция должна возвращать сумму квадратов функций

                        DenseVector dvParametersPoint = DenseVector.OfEnumerable(parametersPoint);
                        DenseVector dvResSumSq = DenseVector.Create(dvSpace.Count, 0.0d);
                        foreach (Func<DenseVector, double, double> func in parametersConditionsEquals0)
                        {
                            DenseVector eqConstraintFuncValues = DenseVector.Create(dvSpace.Count,
                                i => func(dvParametersPoint, dvSpace[i]));
                            eqConstraintFuncValues.MapInplace(dVal => dVal*dVal);
                            dvResSumSq += eqConstraintFuncValues;
                        }

                        return (ILRetArray<double>) (dvResSumSq.ToArray());
                    }
                };
            }

            Optimization.ObjectiveFunction<double> ineqFunc =
                (parametersPoint) => (ILRetArray<double>)(DenseVector.Create(dvSpace.Count, -1.0d).ToArray());
            if (parametersConditionsLessThan0.Count > 0)
            {
                ineqFunc = (parametersPoint) =>
                {
                    using (ILScope.Enter(parametersPoint))
                    {
                        // посольку ограничений на отрицательное значение может быть несколько
                        // поэтому сделаем так: преобразуем условия в булевы по принципу "верно ли неравенство"
                        // потом - логическим И получим результат по всем условиям
                        // выдадим результат: true = -1, false = +1

                        DenseVector dvParametersPoint = DenseVector.OfEnumerable(parametersPoint);
                        List<bool> resValues = new List<bool>();
                        foreach (double d in dvSpace)
                        {
                            resValues.Add(false);
                        }
                        foreach (Func<DenseVector, double, double> func in parametersConditionsLessThan0)
                        {
                            DenseVector ineqConstraintFuncValues = DenseVector.Create(dvSpace.Count,
                                i => func(dvParametersPoint, dvSpace[i]));
                            List<double> currResult = new List<double>(ineqConstraintFuncValues);
                            List<bool> currResultBool = currResult.ConvertAll<bool>(dVal => (dVal < 0) ? (true) : (false));
                            resValues = new List<bool>(resValues.Zip(currResultBool, (b1, b2) => b1 && b2));
                        }

                        return (resValues.Aggregate((bv1, bv2) => bv1 && bv2)) ? (-1) : (1);

                        //return
                        //    (ILRetArray<double>)
                        //        (resValues.ConvertAll<double>(bVal => (bVal) ? (-1.0d) : (1.0d)).ToArray());

                    }
                };
            }


            ILArray<double> initParameters = dvInitialParametersValues.ToArray();
            double[] resDoub;
            using (ILScope.Enter(initParameters))
            {
                ILRetArray<double> result = Optimization.fmin(
                                                            devFunc,
                                                            initParameters,
                                                            EqualityConstraint: eqFunc,
                                                            InequalityConstraint: ineqFunc,
                                                            tol: maxRelativeError);
                resDoub = result.ToArray();
            }

            return DenseVector.OfEnumerable(resDoub);
        }




        public DenseVector Approximation_MKL(DenseVector dvInitialParametersValues, double maxRelativeError = 1.0e-8d)
        {
            
        }





        /// <summary>
        /// Approximations using gradient descent method. The ordinary function case.
        /// </summary>
        /// <param name="dvInitialParametersValues">The dv initioal parameters values.</param>
        /// <param name="dvInitialParametersIncrement">The dv initial parameters increment.</param>
        /// <param name="maxRelativeError">The maximum relative error.</param>
        /// <returns>DenseVector.</returns>
        public DenseVector ApproximationGradientDescentMultidim(
            DenseVector dvInitialParametersValues,
            ref DenseVector dvInitialParametersIncrement,
            double maxRelativeError = 0.0001d)
        {
            DenseVector currentParametersValues = dvInitialParametersValues.Copy();
            updateParametersScale(currentParametersValues);

            DenseVector dvCurrentParametersIncrement = dvInitialParametersIncrement.Copy();
            double stepFollowingGradKoeff = 1.0d;
            double relativeMinimizingValueDiffIncrement = maxRelativeError * 2.0d;
            double previousRelativeMinimizingValueIncrement = 1.0d;
            DenseVector nextParametersValues = currentParametersValues.Copy();

            while (Math.Abs(relativeMinimizingValueDiffIncrement) > maxRelativeError)
            {
                stepFollowingGradKoeff = stepFollowingGradKoeff * 2.0d;
                DenseVector dvNewParametersIncrements;
                //DenseVector dvCurrentGrad = GradOfEvaluatingValueRidder(
                DenseVector dvCurrentGrad = GradOfEvaluatingValue(
                                                dvParametersPoint => DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, dvParametersPoint, dvWeights),
                                                currentParametersValues,
                                                dvCurrentParametersIncrement,
                                                out dvNewParametersIncrements);
                dvCurrentParametersIncrement = dvNewParametersIncrements.Copy();

                nextParametersValues = currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad;

                //DenseVector dvNextGrad = GradOfEvaluatingValueRidder(
                DenseVector dvNextGrad = GradOfEvaluatingValue(
                                                dvParametersPoint => DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, dvParametersPoint, dvWeights),
                                                nextParametersValues,
                                                dvCurrentParametersIncrement,
                                                out dvNewParametersIncrements);
                dvCurrentParametersIncrement = dvNewParametersIncrements.Copy();

                DenseVector gradIncrement = dvNextGrad - dvCurrentGrad;
                double gradRelativeIncrement = gradIncrement.Abs() / dvCurrentGrad.Abs();


                while (gradRelativeIncrement > 0.2d)
                {
                    //stepFollowingGradKoeff adjusting
                    double stepFollowingGradKoeffPrev = stepFollowingGradKoeff;

                    if (gradRelativeIncrement > 0.2d)
                    {
                        stepFollowingGradKoeff = stepFollowingGradKoeff * RandomWithLimits(0.3d, 0.7d);
                        if (stepFollowingGradKoeff == 0.0d)
                        {
                            stepFollowingGradKoeff = stepFollowingGradKoeffPrev * 2.0d;
                            break;
                        }
                    }

                    nextParametersValues = currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad;

                    //dvNextGrad = GradOfEvaluatingValueRidder(
                    dvNextGrad = GradOfEvaluatingValue(
                                                dvParametersPoint => DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, dvParametersPoint, dvWeights),
                                                nextParametersValues,
                                                dvCurrentParametersIncrement,
                                                out dvNewParametersIncrements);
                    dvCurrentParametersIncrement = dvNewParametersIncrements.Copy();

                    gradIncrement = dvNextGrad - dvCurrentGrad;
                    gradRelativeIncrement = gradIncrement.Abs() / dvCurrentGrad.Abs();
                }

                #region // Проверка условий

                //if (parametersConditions.Count > 0)
                //{
                //    DenseVector modifiedParametersIncrement = stepFollowingGradKoeff * dvCurrentGrad;
                //    bool conditionsTrue = false;
                //    int testimgParamenerIndex = 0;
                //    while (!conditionsTrue)
                //    {

                //        if (testimgParamenerIndex > 0)
                //            modifiedParametersIncrement[testimgParamenerIndex - 1] =
                //                -modifiedParametersIncrement[testimgParamenerIndex - 1];
                //        modifiedParametersIncrement[testimgParamenerIndex] =
                //            -modifiedParametersIncrement[testimgParamenerIndex];
                //        nextParametersValues = currentParametersValues - modifiedParametersIncrement;

                //        conditionsTrue = true;
                //        foreach (Func<DenseVector, bool> condition in parametersConditions)
                //        {
                //            conditionsTrue = conditionsTrue && condition(nextParametersValues);
                //        }
                //    }

                //}

                #endregion // Проверка условий

                double currentMinimizingValue = DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, currentParametersValues, dvWeights);
                double nextMinimizingValue = DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad, dvWeights);
                double currentRelativeIncrement = (nextMinimizingValue - currentMinimizingValue) / currentMinimizingValue;

                //relativeMinimizingValueDiffIncrement = (currentRelativeIncrement - previousRelativeMinimizingValueIncrement) / currentRelativeIncrement;
                relativeMinimizingValueDiffIncrement = currentRelativeIncrement;


                if (currentRelativeIncrement == 0.0d)
                {
                    currentRelativeIncrement = currentRelativeIncrement +
                                               RandomWithLimits(0.2d, 0.6d);
                    relativeMinimizingValueDiffIncrement = (currentRelativeIncrement -
                                                            previousRelativeMinimizingValueIncrement) /
                                                           currentRelativeIncrement;

                    nextParametersValues = nextParametersValues +
                                           DenseVector.Create(dvNextGrad.Count, (i => rnd.NextDouble() * dvNextGrad[i]));
                }


                previousRelativeMinimizingValueIncrement = currentRelativeIncrement;
                currentParametersValues = nextParametersValues;

                //updateParametersScale(currentParametersValues);

                if (SelfWorker != null)
                {
                    MinimizingFunctionValues.Add(currentMinimizingValue);
                    List<double> subSample = MinimizingFunctionValues;
                    if (MinimizingFunctionValues.Count > 100)
                        subSample = MinimizingFunctionValues.GetRange(MinimizingFunctionValues.Count - 101, 100);
                    DescriptiveStatistics stats = new DescriptiveStatistics(subSample);

                    //Thread.Sleep(10);
                    Tuple<DenseVector, double> theObject = new Tuple<DenseVector, double>(currentParametersValues,
                        stats.StandardDeviation / stats.Mean);
                    SelfWorker.ReportProgress(50, theObject);
                    if (SelfWorker.CancellationPending)
                    {
                        break;
                    }
                }
                else
                {
                    //theSelfLogWindow = ServiceTools.LogAText(theSelfLogWindow, densevectorToString(nextParametersValues), false);
                }
            }


            if (SelfWorker == null)
            {
                //theSelfLogWindow = ServiceTools.LogAText(theSelfLogWindow, "DONE" + Environment.NewLine + densevectorToString(nextParametersValues), false);
            }

            dvInitialParametersIncrement = DenseVector.OfVector(dvCurrentParametersIncrement);

            return nextParametersValues;
        }







        /// <summary>
        /// Grads the of evaluating value.
        /// </summary>
        /// <param name="theFunction">Функция, от которой берется градиент в пространстве параметров.
        /// В качестве аргументов типа DenseVector должна принимать текущую точку в пространстве парметров
        /// Возвращать функция должна собственно свое значение в зависимости от вектора параметров</param>
        /// <param name="dvCurrentParametersPoint">The current parameters point vector. Текущая точка в пространстве параметров, в которой вычисляется градиент.</param>
        /// <param name="dvPreviousParametersIncrement">The previous parameters increment.
        /// Вектор используемых дифференциалов компонент вектора параметров, давший удовлетворительную точность определения градиента на предыдущем шаге приближения</param>
        /// <param name="maxRelativeError">The maximum relative error. Максимальная относительная ошибка определения градиента</param>
        /// <param name="dvNewParametersIncrement">The new parameters increment. Новый вектор дифференциалов компонент вектора параметров,
        /// давший удовлетворительную точность определения градиента на этом шаге</param>
        /// <returns>DenseVector - значение градиента в пространстве параметров.</returns>
        private DenseVector GradOfEvaluatingValue(Func<DenseVector, double> inputFunction,
            DenseVector dvCurrentParametersPoint,
            DenseVector dvPreviousParametersIncrement, out DenseVector dvNewParametersIncrement, double maxRelativeError = 0.005d)
        {
            DenseVector dvCurrentParametersIncrement = dvPreviousParametersIncrement.Copy() * 2.0d;
            Func<DenseVector, double> theFunction = inputFunction;


            for (int i = 0; i < dvCurrentParametersIncrement.Count; i++)
            {
                if (Math.Abs(dvCurrentParametersIncrement[i]) / dvParametersScale[i] < 1.0e-2)
                {
                    dvCurrentParametersIncrement[i] = dvParametersScale[i] * 1.0e-2;
                }
                else if (Math.Abs(dvCurrentParametersIncrement[i]) / dvParametersScale[i] > 1.0e+2)
                {
                    dvCurrentParametersIncrement[i] = dvParametersScale[i];
                }
            }



            DenseVector grad = DenseVector.Create(dvCurrentParametersPoint.Count, (index) =>
                                                    {
                                                        DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                                                            (i => (i == index) ? (dvCurrentParametersIncrement[i]) : (0.0d)));
                                                        return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                                                                theFunction(dvCurrentParametersPoint - dvPartialParametersIncrement))/
                                                               2.0d * dvPartialParametersIncrement[index];
                                                    });

            DenseVector gradHalfed = DenseVector.Create(dvCurrentParametersPoint.Count, (index) =>
                                                        {
                                                            DenseVector dvPartialParametersIncrementHalfed = DenseVector.Create(dvCurrentParametersIncrement.Count,
                                                                (i => (i == index) ? (dvCurrentParametersIncrement[i] / 2.0d) : (0.0d)));
                                                            return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrementHalfed) -
                                                                    theFunction(dvCurrentParametersPoint - dvPartialParametersIncrementHalfed))/
                                                                   2.0d*dvPartialParametersIncrementHalfed[index];
                                                        });

            double relativeError = (grad - gradHalfed).Abs() / grad.Abs();
            for (int varIndex = 0; varIndex < dvCurrentParametersIncrement.Count; varIndex++)
            {
                double partialRelativeError = relativeError;
                double currentVarIncrement = dvCurrentParametersIncrement[varIndex];

                while (partialRelativeError > maxRelativeError)
                {
                    currentVarIncrement = currentVarIncrement / 2.0d;
                    if (Math.Abs(currentVarIncrement) / dvParametersScale[varIndex] < 1.0e-20)
                    {
                        currentVarIncrement = currentVarIncrement * 2.0d;
                        break;
                    }
                    else if (Math.Abs(currentVarIncrement) / dvParametersScale[varIndex] > 1.0e+20)
                    {
                        currentVarIncrement = currentVarIncrement / 2.0d;
                        break;
                    }

                    DenseVector gradPartial = DenseVector.Create(dvCurrentParametersPoint.Count,
                                                                (index) =>
                                                                {
                                                                    if (index != varIndex) return 0.0d;
                                                                    DenseVector dvPartialParametersIncrement =
                                                                        DenseVector.Create(
                                                                            dvCurrentParametersIncrement.Count,
                                                                            (i => (i == index) ? (currentVarIncrement) : (0.0d)));

                                                                    return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                                                                            theFunction(dvCurrentParametersPoint - dvPartialParametersIncrement))/
                                                                            2.0d*currentVarIncrement;
                                                                });

                    DenseVector gradPartialHalfed = DenseVector.Create(dvCurrentParametersPoint.Count,
                                                                (index) =>
                                                                {
                                                                    if (index != varIndex) return 0.0d;
                                                                    DenseVector dvPartialParametersIncrement =
                                                                        DenseVector.Create(
                                                                            dvCurrentParametersIncrement.Count,
                                                                            (i => (i == index) ? (currentVarIncrement/2.0d) : (0.0d)));

                                                                    return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                                                                            theFunction(dvCurrentParametersPoint - dvPartialParametersIncrement))/
                                                                           currentVarIncrement;
                                                                });

                    
                    if ((gradPartial.Abs() == 0.0d) || (gradPartialHalfed.Abs() == 0.0d))
                    {
                        currentVarIncrement = currentVarIncrement * 2.0d;
                        break;
                    }

                    partialRelativeError = (gradPartial - gradPartialHalfed).Abs() / gradPartial.Abs();


                    if (SelfWorker != null)
                    {
                        if (SelfWorker.CancellationPending)
                        {
                            break;
                        }
                    }
                }

                dvCurrentParametersIncrement[varIndex] = currentVarIncrement;
            }

            grad = DenseVector.Create(dvCurrentParametersPoint.Count, (index) =>
                                        {
                                            DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                                                (i => (i == index) ? (dvCurrentParametersIncrement[index]) : (0.0d)));
                                            return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                                                    theFunction(dvCurrentParametersPoint - dvPartialParametersIncrement))/
                                                    2.0d*dvPartialParametersIncrement[index];
                                        });

            dvNewParametersIncrement = dvCurrentParametersIncrement.Copy();

            return grad;
        }













        private DenseVector GradOfEvaluatingValueRidder(Func<DenseVector, double> inputFunction,
            DenseVector dvCurrentParametersPoint,
            DenseVector dvPreviousParametersIncrement, out DenseVector dvNewParametersIncrement, double maxRelativeError = 0.005d)
        {
            DenseVector dvCurrentParametersIncrement = dvPreviousParametersIncrement.Copy() * 2.0d;
            Func<DenseVector, double> theFunction = inputFunction;
            double maxRelPartialDerivAboveMin = 1000.0d;

            for (int i = 0; i < dvCurrentParametersIncrement.Count; i++)
            {
                if (Math.Abs(dvCurrentParametersIncrement[i]) / dvParametersScale[i] < 1.0e-2)
                {
                    dvCurrentParametersIncrement[i] = dvParametersScale[i] * 1.0e-2;
                }
                else if (Math.Abs(dvCurrentParametersIncrement[i]) / dvParametersScale[i] > 1.0e+2)
                {
                    dvCurrentParametersIncrement[i] = dvParametersScale[i];
                }
            }



            DenseVector grad = DenseVector.Create(dvCurrentParametersPoint.Count, (index) =>
            {
                DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                    (i => (i == index) ? (dvCurrentParametersIncrement[i]) : (0.0d)));
                return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                        theFunction(dvCurrentParametersPoint - dvPartialParametersIncrement)) / (2.0d * dvPartialParametersIncrement.Sum());
            });

            DenseVector gradHalfed = DenseVector.Create(dvCurrentParametersPoint.Count, (index) =>
            {
                DenseVector dvPartialParametersIncrementHalfed = DenseVector.Create(dvCurrentParametersIncrement.Count,
                    (i => (i == index) ? (dvCurrentParametersIncrement[i] / 2.0d) : (0.0d)));
                return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrementHalfed) -
                        theFunction(dvCurrentParametersPoint - dvPartialParametersIncrementHalfed)) / (2.0d * dvPartialParametersIncrementHalfed.Sum());
            });

            double relativeError = (grad - gradHalfed).Abs() / grad.Abs();

            for (int varIndex = 0; varIndex < dvCurrentParametersIncrement.Count; varIndex++)
            {
                double partialRelativeError = relativeError;
                double currentVarIncrement = dvCurrentParametersIncrement[varIndex];

                List<Tuple<double, double, int>> lTplRelErrAndPartDeriv = new List<Tuple<double, double, int>>();

                DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                    (i => (i == varIndex) ? (dvCurrentParametersIncrement[i]) : (0.0d)));
                List<DenseVector> dvParametersPointsToDetermineDerivative = new List<DenseVector>();
                dvParametersPointsToDetermineDerivative.Add(dvCurrentParametersPoint - dvPartialParametersIncrement);
                dvParametersPointsToDetermineDerivative.Add(dvCurrentParametersPoint + dvPartialParametersIncrement);
                NevillePolynomialInterpolation interpolator = new NevillePolynomialInterpolation(
                    dvParametersPointsToDetermineDerivative.ConvertAll<double>(dv => dv[varIndex]).ToArray(),
                    dvParametersPointsToDetermineDerivative.ConvertAll<double>(dv => theFunction(dv)).ToArray());

                double prevPartialDeriv = interpolator.Differentiate(dvCurrentParametersPoint[varIndex]);

                int iNumberOfPointsBetween = 0;

                while (partialRelativeError > maxRelativeError)
                {
                    iNumberOfPointsBetween += 2;

                    List<DenseVector> dvCurrParametersPoints = new List<DenseVector>();
                    dvCurrParametersPoints.InsertRange(0, dvParametersPointsToDetermineDerivative);
                    for (int i = 1; i <= iNumberOfPointsBetween; i++)
                    {
                        dvCurrParametersPoints.Add(dvParametersPointsToDetermineDerivative[0] +
                                                   (dvParametersPointsToDetermineDerivative[1] -
                                                    dvParametersPointsToDetermineDerivative[0]) / (i + 1));
                    }
                    dvCurrParametersPoints.Sort(
                        (dv1, dv2) => (dv1[varIndex] < dv2[varIndex]) ? (-1) : (1));
                    NevillePolynomialInterpolation currInterpolator = new NevillePolynomialInterpolation(
                        dvCurrParametersPoints.ConvertAll<double>(dv => dv[varIndex]).ToArray(),
                        dvCurrParametersPoints.ConvertAll<double>(dv => theFunction(dv)).ToArray());

                    double currPartialDeriv = currInterpolator.Differentiate(dvCurrentParametersPoint[varIndex]);



                    #region // OBSOLETE
                    //currentVarIncrement = currentVarIncrement / 2.0d;
                    //if (Math.Abs(currentVarIncrement) / dvParametersScale[varIndex] < 1.0e-20)
                    //{
                    //    currentVarIncrement = currentVarIncrement * 2.0d;
                    //    break;
                    //}
                    //else if (Math.Abs(currentVarIncrement) / dvParametersScale[varIndex] > 1.0e+20)
                    //{
                    //    currentVarIncrement = currentVarIncrement / 2.0d;
                    //    break;
                    //}

                    //DenseVector gradPartial = DenseVector.Create(dvCurrentParametersPoint.Count, new Func<int, double>((index) =>
                    //{
                    //    if (index != varIndex) return 0.0d;
                    //    DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                    //        (i => (i == index) ? (currentVarIncrement) : (0.0d)));

                    //    return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                    //                           theFunction(dvCurrentParametersPoint)) / currentVarIncrement;
                    //}));
                    //
                    //DenseVector gradPartialHalfed = DenseVector.Create(dvCurrentParametersPoint.Count, new Func<int, double>((index) =>
                    //{
                    //    if (index != varIndex) return 0.0d;
                    //    DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                    //        (i => (i == index) ? (currentVarIncrement / 2.0d) : (0.0d)));

                    //    return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                    //                           theFunction(dvCurrentParametersPoint)) / (currentVarIncrement / 2.0d);
                    //}));

                    //double gradPartialModule = gradPartial.Abs();
                    //double gradPartialHalfedModule = gradPartialHalfed.Abs();
                    //if ((gradPartialModule == 0.0d) || (gradPartialHalfedModule == 0.0d))
                    //{
                    //    currentVarIncrement = currentVarIncrement * 2.0d;
                    //    break;
                    //}

                    // (gradPartial - gradPartialHalfed).Abs() / gradPartial.Abs();
                    #endregion // OBSOLETE

                    partialRelativeError = Math.Abs((currPartialDeriv - prevPartialDeriv) / prevPartialDeriv);
                    prevPartialDeriv = currPartialDeriv;
                    dvCurrentParametersIncrement[varIndex] = currentVarIncrement / (iNumberOfPointsBetween + 2);


                    lTplRelErrAndPartDeriv.Add(new Tuple<double, double, int>(partialRelativeError, currPartialDeriv, iNumberOfPointsBetween));
                    if (partialRelativeError / lTplRelErrAndPartDeriv.Min(tpl => tpl.Item1) > maxRelPartialDerivAboveMin)
                    {
                        prevPartialDeriv = lTplRelErrAndPartDeriv.Min(tpl => tpl.Item2);
                        dvCurrentParametersIncrement[varIndex] = currentVarIncrement / (lTplRelErrAndPartDeriv.Min(tpl => tpl.Item3) + 2);
                        break;
                    }

                    if (SelfWorker != null)
                    {
                        if (SelfWorker.CancellationPending)
                        {
                            break;
                        }
                    }
                }
                grad[varIndex] = prevPartialDeriv;
            }



            #region // OBSOLETE
            //grad = DenseVector.Create(dvCurrentParametersPoint.Count, new Func<int, double>((index) =>
            //{
            //    DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
            //        (i => (i == index) ? (dvCurrentParametersIncrement[i]) : (0.0d)));
            //    return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
            //                           theFunction(dvCurrentParametersPoint - dvPartialParametersIncrement)) / (2.0d*dvPartialParametersIncrement.Sum());
            //}));
            #endregion // OBSOLETE

            dvNewParametersIncrement = DenseVector.OfEnumerable(dvCurrentParametersIncrement);
            dvNewParametersIncrement = (grad / grad.Abs()) * dvNewParametersIncrement.Abs();

            return grad;
        }
    }
}
