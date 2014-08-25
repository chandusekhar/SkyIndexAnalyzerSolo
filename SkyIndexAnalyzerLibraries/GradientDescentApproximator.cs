using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

namespace SkyIndexAnalyzerLibraries
{
    public class GradientDescentApproximator
    {
        public DenseVector dvDataValues;
        public DenseVector dvSpace;
        public Func<DenseVector, double, double> approxFunc;
        public System.ComponentModel.BackgroundWorker SelfWorker = null;
        private DenseVector dvWeights = null;
        private LogWindow theSelfLogWindow;
        //public List<Func<DenseVector, bool>> parametersConditions = new List<Func<DenseVector, bool>>();
        private DenseVector dvParametersScale = null;
        private List<double> MinimizingFunctionValues = new List<double>();

        private List<PointD> pointsList = new List<PointD>();
        public Func<DenseVector, PointD, double> approxFuncPt;

        private int calculationType = 0;
        // 0 - обычная функция
        // 1 - набор точек на плоскости, не обязательно однозначное соответствие x => y


        public GradientDescentApproximator(DenseVector dvInputDataValues, DenseVector dvInputSpace, Func<DenseVector, double, double> inputApproxFunc)
        {
            dvDataValues = (DenseVector)dvInputDataValues.Clone();
            dvSpace = (DenseVector)dvInputSpace.Clone();
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
                    dvWeights.MapInplace(new Func<double, double>(x => x / theSum));
                }
            }
        }



        private void updateParametersScale(DenseVector dvActualParametersValues)
        {
            if (dvParametersScale == null)
                dvParametersScale = DenseVector.Create(dvActualParametersValues.Count,
                    new Func<int, double>(i => Math.Abs(dvActualParametersValues[i])));
            else
            {
                dvParametersScale.MapIndexedInplace(new Func<int, double, double>((i, dVal) =>
                {
                    if (Math.Abs(dvActualParametersValues[i]) > dVal) return Math.Abs(dvActualParametersValues[i]);
                    else return dVal;
                }));
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
            DenseVector currentParametersValues = (DenseVector)dvInitioalParametersValues.Clone();
            updateParametersScale(currentParametersValues);

            DenseVector dvCurrentParametersIncrement = (DenseVector)dvInitialParametersIncrement.Clone();
            DenseVector dvNewParametersIncrements;
            double stepFollowingGradKoeff = 1.0d;
            double relativeMinimizingValueDiffIncrement = 1.0d;
            double previousRelativeMinimizingValueIncrement = 1.0d;
            DenseVector nextParametersValues = (DenseVector)currentParametersValues.Clone();

            while (Math.Abs(relativeMinimizingValueDiffIncrement) > maxRelativeError)
            {
                stepFollowingGradKoeff = stepFollowingGradKoeff * 2.0d;
                DenseVector dvCurrentGrad = GradOfEvaluatingValue(
                    new Func<DenseVector, double>(DeviationsSquaredSumPt),
                    currentParametersValues,
                    dvCurrentParametersIncrement,
                    out dvNewParametersIncrements);
                dvCurrentParametersIncrement = (DenseVector)dvNewParametersIncrements.Clone();

                nextParametersValues = currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad;

                DenseVector dvNextGrad = GradOfEvaluatingValue(
                    new Func<DenseVector, double>(DeviationsSquaredSumPt),
                    nextParametersValues,
                    dvCurrentParametersIncrement,
                    out dvNewParametersIncrements);
                dvCurrentParametersIncrement = (DenseVector)dvNewParametersIncrements.Clone();

                DenseVector gradIncrement = dvNextGrad - dvCurrentGrad;
                double gradRelativeIncrement = Math.Sqrt(gradIncrement * gradIncrement / (dvCurrentGrad * dvCurrentGrad));


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
                        new Func<DenseVector, double>(dvParametersVector =>
                        {
                            //здесь должна быть функция, от которой берем градиент
                            return DeviationsSquaredSumPt(dvParametersVector);
                        }),
                        nextParametersValues,
                        dvCurrentParametersIncrement,
                        out dvNewParametersIncrements);
                    dvCurrentParametersIncrement = (DenseVector)dvNewParametersIncrements.Clone();

                    gradIncrement = dvNextGrad - dvCurrentGrad;
                    gradRelativeIncrement = Math.Sqrt(gradIncrement * gradIncrement / (dvCurrentGrad * dvCurrentGrad));
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
                    SelfWorker.ReportProgress(50, (object)theObject);

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
                dvApproxFuncValues = DenseVector.Create(pointsList.Count, new Func<int, double>(ix =>
                {
                    return approxFuncPt(currentParametersSpacePoint, pointsList[ix]);
                }));
            }
            else
            {
                dvApproxFuncValues = DenseVector.Create(pointsList.Count, new Func<int, double>(ix =>
                {
                    return approxFuncPt(currentParametersSpacePoint, pointsList[ix]);
                }));
                dvApproxFuncValues = (DenseVector)dvApproxFuncValues.PointwiseMultiply(dvWeights);
            }

            return Math.Sqrt(dvApproxFuncValues * dvApproxFuncValues);
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
        private static double DeviationsSquaredSumRelative(DenseVector dvDataValues, DenseVector dvSpace, Func<DenseVector, double, double> approxFunc, DenseVector currentParametersSpacePoint, DenseVector dvWeights)
        //public delegate double DeviationsSquaredSumRelative(DenseVector currentParametersSpacePoint)
        {
            DenseVector dvDistanceRelative;
            if (dvWeights == null)
            {
                DenseVector dvApproxFuncValues = DenseVector.Create(dvSpace.Count, new Func<int, double>(ix =>
                {
                    return approxFunc(currentParametersSpacePoint, dvSpace[ix]);
                }));
                DenseVector dvDistanceAbs = dvApproxFuncValues - dvDataValues;
                dvDistanceRelative = (DenseVector)(dvDistanceAbs.PointwiseDivide(dvDataValues));

            }
            else
            {
                DenseVector dvApproxFuncValues = DenseVector.Create(dvSpace.Count, new Func<int, double>(ix =>
                {
                    return approxFunc(currentParametersSpacePoint, dvSpace[ix]);
                }));
                DenseVector dvDistanceAbs = dvApproxFuncValues - dvDataValues;
                dvDistanceRelative = (DenseVector)(dvDistanceAbs.PointwiseDivide(dvDataValues));
                dvDistanceRelative = (DenseVector)dvDistanceRelative.PointwiseMultiply(dvWeights);
            }
            return Math.Sqrt(dvDistanceRelative * dvDistanceRelative);
        }












        private double RandomWithLimits(Random rnd, double rndMin, double rndMax)
        {
            return rndMin + rnd.NextDouble() * (rndMax - rndMin);
        }




        /// <summary>
        /// Approximations using gradient descent method. The ordinary function case.
        /// </summary>
        /// <param name="dvInitioalParametersValues">The dv initioal parameters values.</param>
        /// <param name="dvInitialParametersIncrement">The dv initial parameters increment.</param>
        /// <param name="maxRelativeError">The maximum relative error.</param>
        /// <returns>DenseVector.</returns>
        public DenseVector ApproximationGradientDescent2D(DenseVector dvInitioalParametersValues, DenseVector dvInitialParametersIncrement, double maxRelativeError = 0.0001d)
        {
            DenseVector currentParametersValues = (DenseVector)dvInitioalParametersValues.Clone();
            updateParametersScale(currentParametersValues);

            Random rnd = new Random();

            DenseVector dvCurrentParametersIncrement = (DenseVector)dvInitialParametersIncrement.Clone();
            DenseVector dvNewParametersIncrements;
            double stepFollowingGradKoeff = 1.0d;
            double relativeMinimizingValueDiffIncrement = 1.0d;
            double previousRelativeMinimizingValueIncrement = 1.0d;
            DenseVector nextParametersValues = (DenseVector)currentParametersValues.Clone();

            while (Math.Abs(relativeMinimizingValueDiffIncrement) > maxRelativeError)
            {
                stepFollowingGradKoeff = stepFollowingGradKoeff * 2.0d;
                DenseVector dvCurrentGrad = GradOfEvaluatingValue(new Func<DenseVector, double>(dvParametersPoint =>
                {
                    return DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, dvParametersPoint, dvWeights);
                }),
                currentParametersValues,
                dvCurrentParametersIncrement,
                out dvNewParametersIncrements);
                dvCurrentParametersIncrement = (DenseVector)dvNewParametersIncrements.Clone();

                nextParametersValues = currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad;

                DenseVector dvNextGrad = GradOfEvaluatingValue(new Func<DenseVector, double>(dvParametersPoint =>
                {
                    return DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, dvParametersPoint, dvWeights);
                }),
                    nextParametersValues,
                    dvCurrentParametersIncrement,
                    out dvNewParametersIncrements);
                dvCurrentParametersIncrement = (DenseVector)dvNewParametersIncrements.Clone();

                DenseVector gradIncrement = dvNextGrad - dvCurrentGrad;
                double gradRelativeIncrement = Math.Sqrt(gradIncrement * gradIncrement / (dvCurrentGrad * dvCurrentGrad));


                while (gradRelativeIncrement > 0.2d)
                {
                    //stepFollowingGradKoeff adjusting
                    double stepFollowingGradKoeffPrev = stepFollowingGradKoeff;

                    if (gradRelativeIncrement > 0.2d)
                    {
                        stepFollowingGradKoeff = stepFollowingGradKoeff * RandomWithLimits(rnd, 0.3d, 0.7d);
                        if (stepFollowingGradKoeff == 0.0d)
                        {
                            stepFollowingGradKoeff = stepFollowingGradKoeffPrev * 2.0d;
                            break;
                        }
                    }
                    //else if (gradRelativeIncrement < 0.00001d)
                    //{
                    //    stepFollowingGradKoeff = stepFollowingGradKoeff * RandomWithLimits(rnd, 1.5d, 2.5d);
                    //    if (stepFollowingGradKoeff == 0.0d)
                    //    {
                    //        stepFollowingGradKoeff = stepFollowingGradKoeffPrev * 2.0d;
                    //        break;
                    //    }
                    //}




                    nextParametersValues = currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad;

                    dvNextGrad = GradOfEvaluatingValue(new Func<DenseVector, double>(dvParametersPoint =>
                {
                    return DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, dvParametersPoint, dvWeights);
                }),
                        nextParametersValues,
                        dvCurrentParametersIncrement,
                        out dvNewParametersIncrements);
                    dvCurrentParametersIncrement = (DenseVector)dvNewParametersIncrements.Clone();

                    gradIncrement = dvNextGrad - dvCurrentGrad;
                    gradRelativeIncrement = Math.Sqrt(gradIncrement * gradIncrement / (dvCurrentGrad * dvCurrentGrad));
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

                double currentMinimizingValue = DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, currentParametersValues, dvWeights);
                double nextMinimizingValue = DeviationsSquaredSumRelative(dvDataValues, dvSpace, approxFunc, currentParametersValues - stepFollowingGradKoeff * dvCurrentGrad, dvWeights);
                double currentRelativeIncrement = (nextMinimizingValue - currentMinimizingValue) / currentMinimizingValue;

                //relativeMinimizingValueDiffIncrement = (currentRelativeIncrement - previousRelativeMinimizingValueIncrement) / currentRelativeIncrement;
                relativeMinimizingValueDiffIncrement = currentRelativeIncrement;


                if (currentRelativeIncrement == 0.0d)
                {
                    currentRelativeIncrement = currentRelativeIncrement +
                                               RandomWithLimits(rnd, 0.2d, 0.6d);
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
                    SelfWorker.ReportProgress(50, (object)theObject);
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
            DenseVector dvCurrentParametersIncrement = (DenseVector)dvPreviousParametersIncrement.Clone() * 2.0d;
            Func<DenseVector, double> theFunction = new Func<DenseVector, double>(inputFunction);


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

            //for (int i = 0; i < dvCurrentParametersIncrement.Count; i++)
            //{
            //    if (dvCurrentParametersIncrement[i] / dvParametersScale[i])
            //}

            DenseVector grad = DenseVector.Create(dvCurrentParametersPoint.Count, new Func<int, double>((index) =>
            {
                DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                    (i => (i == index) ? (dvCurrentParametersIncrement[i]) : (0.0d)));
                return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                                       theFunction(dvCurrentParametersPoint)) / dvPartialParametersIncrement.Sum();
            }));

            DenseVector gradHalfed = DenseVector.Create(dvCurrentParametersPoint.Count, new Func<int, double>((index) =>
            {
                DenseVector dvPartialParametersIncrementHalfed = DenseVector.Create(dvCurrentParametersIncrement.Count,
                    (i => (i == index) ? (dvCurrentParametersIncrement[i] / 2.0d) : (0.0d)));
                return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrementHalfed) -
                                       theFunction(dvCurrentParametersPoint)) / dvPartialParametersIncrementHalfed.Sum();
            }));

            double relativeError = Math.Sqrt(((grad - gradHalfed) * (grad - gradHalfed)) / (grad * grad));
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

                    DenseVector gradPartial = DenseVector.Create(dvCurrentParametersPoint.Count, new Func<int, double>((index) =>
                    {
                        if (index != varIndex) return 0.0d;
                        DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                            (i => (i == index) ? (currentVarIncrement) : (0.0d)));

                        return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                                               theFunction(dvCurrentParametersPoint)) / currentVarIncrement;
                    }));

                    DenseVector gradPartialHalfed = DenseVector.Create(dvCurrentParametersPoint.Count, new Func<int, double>((index) =>
                    {
                        if (index != varIndex) return 0.0d;
                        DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                            (i => (i == index) ? (currentVarIncrement / 2.0d) : (0.0d)));

                        return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                                               theFunction(dvCurrentParametersPoint)) / (currentVarIncrement / 2.0d);
                    }));

                    double gradPartialModule = Math.Sqrt(gradPartial * gradPartial);
                    double gradPartialHalfedModule = Math.Sqrt(gradPartialHalfed * gradPartialHalfed);
                    if ((gradPartialModule == 0.0d) || (gradPartialHalfedModule == 0.0d))
                    {
                        currentVarIncrement = currentVarIncrement * 2.0d;
                        break;
                    }

                    partialRelativeError = Math.Sqrt(((gradPartial - gradPartialHalfed) * (gradPartial - gradPartialHalfed)) / (gradPartial * gradPartial));


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

            grad = DenseVector.Create(dvCurrentParametersPoint.Count, new Func<int, double>((index) =>
            {
                DenseVector dvPartialParametersIncrement = DenseVector.Create(dvCurrentParametersIncrement.Count,
                    (i => (i == index) ? (dvCurrentParametersIncrement[i]) : (0.0d)));
                return (theFunction(dvCurrentParametersPoint + dvPartialParametersIncrement) -
                                       theFunction(dvCurrentParametersPoint)) / dvPartialParametersIncrement.Sum();
            }));

            dvNewParametersIncrement = (DenseVector)dvCurrentParametersIncrement.Clone();

            return grad;
        }

        private string densevectorToString(DenseVector dvData)
        {
            string strOut = "";
            foreach (double d in dvData)
            {
                strOut += d.ToString() + ";";
            }
            strOut = strOut.Substring(0, strOut.Length - 5);
            return strOut;
        }
    }
}
