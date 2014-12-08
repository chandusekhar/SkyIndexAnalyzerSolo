﻿using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.IntegralTransforms;


//using System.Drawing;

namespace SkyImagesAnalyzerLibraries
{
    public class ArithmeticsOnImages : IDisposable
    {
        public Image<Gray, Byte> imgR, imgG, imgB;
        private String exprString;
        public TextBox tbLog;
        //private Image<Gray, Byte> maskImage;
        public double minValue = -255.0d;
        public double maxValue = 255.0d;
        //ColorScheme currentColorScheme;

        //public Image<Gray, Byte> MaskImage
        //{
        //    get { return maskImage; }
        //    set
        //    {
        //        maskImage = value;
        //        dmMask = ImageProcessing.DenseMatrixFromImage(maskImage);
        //        dmMask = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dmMask.Divide((double)255.0d);
        //    }
        //}

        public DenseMatrix dmR = null;
        public DenseMatrix dmG = null;
        public DenseMatrix dmB = null;
        public DenseMatrix dmRes = null;
        public double resValue = 0.0d;
        public DenseMatrix dmY = null;
        //public MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmMask;

        public ArithmeticsOnImages() { }

        private class ImageOperationalValue
        {

            public DenseMatrix dmImageComponent;
            public double dNumber;

            public ImageOperationalValue()
            {
                dNumber = 0.0d;
                dmImageComponent = null;
            }


            //true - denseMatrix
            //false - number
            public bool operandType()
            {
                if (dmImageComponent != null) return true;
                else return false;
            }
        }

        public string ExprString
        {
            get { return exprString; }
            set
            {
                exprString = value;
                exprString = exprString.Replace("grix", "(1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y)");
            }
        }


        public static DenseMatrix sRGBluminanceFrom_sRGB(DenseMatrix dmR, DenseMatrix dmG, DenseMatrix dmB)
        {
            //Y_lin = 0.2126 R_lin + 0.7152 G_lin + 0.0722 B_lin


            DenseMatrix dmLuminance;
            DenseMatrix dmLin = (DenseMatrix)dmR.Clone();
            dmLin.MapInplace(new Func<double, double>((val) => val/255.0d));
            dmLin.MapInplace(new Func<double, double>(sRGBtoLinear));
            dmLin.MapInplace(new Func<double, double>((val) => 0.2126d*val));
            dmLuminance = (DenseMatrix)dmLin.Clone();

            ServiceTools.FlushMemory();

            dmLin = (DenseMatrix)dmG.Clone();
            dmLin.MapInplace(new Func<double, double>((val) => val / 255.0d));
            dmLin.MapInplace(new Func<double, double>(sRGBtoLinear));
            dmLin.MapInplace(new Func<double, double>((val) => 0.7152d * val));
            dmLuminance = dmLuminance + dmLin;

            ServiceTools.FlushMemory();

            dmLin = (DenseMatrix)dmB.Clone();
            dmLin.MapInplace(new Func<double, double>((val) => val / 255.0d));
            dmLin.MapInplace(new Func<double, double>(sRGBtoLinear));
            dmLin.MapInplace(new Func<double, double>((val) => 0.0722d * val));
            dmLuminance = dmLuminance + dmLin;

            ServiceTools.FlushMemory();

            dmLuminance.MapInplace(new Func<double, double>(LinearTo_sRGB));
            dmLuminance.MapInplace(new Func<double, double>((val) => val * 255.0d));

            return dmLuminance;
        }


        public static double sRGBtoLinear(double val)
        {
            if (val <= 0.04045d)
            {
                return val/12.92d;
            }
            else
            {
                return Math.Pow(((val + 0.055d)/1.055d), 2.4d);
            }
        }


        public static double sRGB255toLinear(double val255)
        {
            double val = val255/255.0d;
            if (val <= 0.04045d)
            {
                return 255.0d*val / 12.92d;
            }
            else
            {
                return 255.0d*Math.Pow(((val + 0.055d) / 1.055d), 2.4d);
            }
        }


        //public static DenseMatrix sRGBtoLinear(DenseMatrix dm_sRGBchannel)
        //{
        //    dm_sRGBchannel.MapInplace()
        //    if (val <= 0.04045d)
        //    {
        //        return val / 12.92d;
        //    }
        //    else
        //    {
        //        return Math.Pow(((val + 0.055d) / 1.055d), 2.4d);
        //    }
        //}




        public static double LinearTo_sRGB(double val)
        {
            if (val <= 0.0031308d)
            {
                return val * 12.92d;
            }
            else
            {
                return (Math.Pow(val, 1.0d/2.4d)*1.055d - 0.055d);
            }
        }



        public void RPNeval(bool forceUsingDistributedMatrixes = false)
        {
            dmRes = null;
            resValue = 0.0d;
            String input = exprString.Replace(" ", "");
            if (!forceUsingDistributedMatrixes)
            {
                if (dmR == null)
                {
                    dmR = ImageProcessing.DenseMatrixFromImage(imgR);
                }
                if (dmG == null)
                {
                    dmG = ImageProcessing.DenseMatrixFromImage(imgG);
                }
                if (dmB == null)
                {
                    dmB = ImageProcessing.DenseMatrixFromImage(imgB);
                }
                dmY = sRGBluminanceFrom_sRGB(dmR, dmG, dmB);
            }

            PostfixNotation rpn = new PostfixNotation();
            string[] converted2RPN = rpn.ConvertToPostfixNotation(input);

            Stack<ImageOperationalValue> stack = new Stack<ImageOperationalValue>();
            Queue<string> queue = new Queue<string>(converted2RPN);
            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!rpn.operators.Contains(str))
                {
                    pushImageToStackByColorChar(stack, str);
                    str = queue.Dequeue();
                }
                else
                {
                    ImageOperationalValue summ = null;
                    try
                    {

                        switch (str)
                        {

                            case "+":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    ImageOperationalValue b = stack.Pop();
                                    summ = ImageOperationalValueAdd(a, b);// b + a
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "-":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    ImageOperationalValue b = stack.Pop();
                                    summ = ImageOperationalValueSubtract(a, b);// b - a
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "*":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    ImageOperationalValue b = stack.Pop();
                                    summ = ImageOperationalValueMult(a, b);// b * a
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "/":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    ImageOperationalValue b = stack.Pop();
                                    summ = ImageOperationalValueDivide(a, b);// b / a
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "^":
                                {
                                    double a = stack.Pop().dNumber;
                                    ImageOperationalValue b = stack.Pop();
                                    summ = ImageOperationalValuePow(b, a);// b ^ a
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "grad":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    summ = ImageOperationalValueGrad(a);// grad(a)
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "ddx":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    summ = ImageOperationalValueDDX(a);// d(a)/dx
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "ddy":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    summ = ImageOperationalValueDDY(a);// d(a)/dy
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "sqrt":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    summ = ImageOperationalValueSqrt(a);// grad(a)
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "mean":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    summ = ImageOperationalValueMean(a);// mean(a)
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "stddev":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    summ = ImageOperationalValueSigm(a);// mean(a)
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "abs":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    summ = ImageOperationalValueAbs(a);// abs(a)
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                            case "%":
                                {
                                    double a = stack.Pop().dNumber;
                                    ImageOperationalValue b = stack.Pop();
                                    summ = ImageOperationalValueCut(b, a);// b cut a - cuts "b" matrix elements using "a"*sigma limit of distribution. remember 3-sigma rule?
                                    ServiceTools.FlushMemory(tbLog, "");
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        ThreadSafeOperations.SetTextTB(tbLog, ex.Message, true);
                    }
                    stack.Push(summ);
                    if (queue.Count > 0)
                        str = queue.Dequeue();
                    else
                        break;
                }

            }
            //return Convert.ToDecimal(stack.Pop());
            ImageOperationalValue theResult = stack.Pop();
            dmRes = theResult.dmImageComponent;
            resValue = theResult.dNumber;
            if (dmRes != null)
            {
                minValue = dmRes.Values.Min();
                maxValue = dmRes.Values.Max();
            }
            else
            {
                minValue = resValue;
                maxValue = resValue;
            }
            
            ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + "min value: " + Math.Round(minValue, 2).ToString(), false);
            ThreadSafeOperations.SetTextTB(tbLog, "max value: " + Math.Round(maxValue, 2).ToString(), true);

            ServiceTools.FlushMemory(tbLog, "");
        }




        //b - a
        private ImageOperationalValue ImageOperationalValueSubtract(ImageOperationalValue a, ImageOperationalValue b)
        {
            ImageOperationalValue summ = new ImageOperationalValue();

            if (a.operandType() && b.operandType())
            {
                summ.dmImageComponent = b.dmImageComponent - a.dmImageComponent;
            }
            else if (a.operandType() && !b.operandType())
            {

                summ.dmImageComponent = matrixWithValue(a.dmImageComponent, b.dNumber) - a.dmImageComponent;
            }
            else if (b.operandType() && !a.operandType())
            {
                summ.dmImageComponent = b.dmImageComponent - matrixWithValue(b.dmImageComponent, a.dNumber);
            }


            return summ;
        }


        //b * a
        private ImageOperationalValue ImageOperationalValueMult(ImageOperationalValue a, ImageOperationalValue b)
        {
            ImageOperationalValue summ = new ImageOperationalValue();

            if (a.operandType() && b.operandType())
            {
                summ.dmImageComponent = (DenseMatrix)b.dmImageComponent.PointwiseMultiply(a.dmImageComponent);
            }
            else if (a.operandType() && !b.operandType())
            {
                summ.dmImageComponent = (DenseMatrix)a.dmImageComponent.PointwiseMultiply(matrixWithValue(a.dmImageComponent, b.dNumber));
            }
            else if (b.operandType() && !a.operandType())
            {
                summ.dmImageComponent = (DenseMatrix)b.dmImageComponent.PointwiseMultiply(matrixWithValue(b.dmImageComponent, a.dNumber));
            }


            return summ;
        }


        //b / a
        private ImageOperationalValue ImageOperationalValueDivide(ImageOperationalValue a, ImageOperationalValue b)
        {
            ImageOperationalValue summ = new ImageOperationalValue();

            if (a.operandType() && b.operandType())
            {
                DenseMatrix dmTmpMatrix = (DenseMatrix)a.dmImageComponent.Clone();
                dmTmpMatrix.MapInplace(new Func<double, double>((val) => { return (val == 0.0) ? (1.0) : (val); }));
                summ.dmImageComponent = (DenseMatrix)b.dmImageComponent.PointwiseDivide(dmTmpMatrix);
            }
            else if (a.operandType() && !b.operandType())
            {
                DenseMatrix dmTmpMatrix = (DenseMatrix)a.dmImageComponent.Clone();
                dmTmpMatrix.MapInplace(new Func<double, double>((val) => { return (val == 0.0) ? (1.0) : (val); }));
                summ.dmImageComponent = (DenseMatrix)matrixWithValue(a.dmImageComponent, b.dNumber).PointwiseDivide(dmTmpMatrix);
            }
            else if (b.operandType() && !a.operandType())
            {
                DenseMatrix dmTmpMatrix = (DenseMatrix)matrixWithValue(b.dmImageComponent, a.dNumber);
                dmTmpMatrix.MapInplace(new Func<double, double>((val) => { return (val == 0.0) ? (1.0) : (val); }));
                summ.dmImageComponent = (DenseMatrix)b.dmImageComponent.PointwiseDivide(dmTmpMatrix);
            }


            return summ;
        }


        //b + a
        private ImageOperationalValue ImageOperationalValueAdd(ImageOperationalValue a, ImageOperationalValue b)
        {
            ImageOperationalValue summ = new ImageOperationalValue();

            if (a.operandType() && b.operandType())
            {
                summ.dmImageComponent = a.dmImageComponent + b.dmImageComponent;
            }
            else if (a.operandType() && !b.operandType())
            {
                summ.dmImageComponent = a.dmImageComponent + matrixWithValue(a.dmImageComponent, b.dNumber);
            }
            else if (b.operandType() && !a.operandType())
            {
                summ.dmImageComponent = b.dmImageComponent + matrixWithValue(b.dmImageComponent, a.dNumber);
            }


            return summ;
        }



        private ImageOperationalValue ImageOperationalValuePow(ImageOperationalValue a, double powerExponent)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType())
            {
                DenseMatrix dmRes = (DenseMatrix)a.dmImageComponent.Clone();
                dmRes.MapInplace(new Func<double, double>((x) => { return Math.Pow(x, powerExponent); }));
                res.dmImageComponent = dmRes;
            }
            else
            {
                res.dNumber = Math.Pow(a.dNumber, powerExponent);
            }

            return res;
        }




        //пока что возводит в степень
        //надо посчитать Фурье, отрезать высокие гармоники, вернуть обратно
        //пока отрежем значения, лежащие за пределами интервала 3*sigma от среднего
        private ImageOperationalValue ImageOperationalValueCut(ImageOperationalValue a, double sigmaCount)
        {
            ImageOperationalValue res = new ImageOperationalValue();
            

            if (a.operandType())
            {
                DenseMatrix dmRes = (DenseMatrix)a.dmImageComponent.Clone();

                //determining the new mean value
                DenseMatrix dmTmp = (DenseMatrix)a.dmImageComponent.Clone();
                DescriptiveStatistics stats = new DescriptiveStatistics(dmTmp.Values);
                double dataMeanTmp = stats.Mean;
                double standDevTmp = stats.StandardDeviation;
                double deviationMarginTmp = sigmaCount * standDevTmp;
                dmTmp.MapInplace(new Func<double, double>((dVal) =>
                {
                    if (Math.Abs(dVal - dataMeanTmp) > deviationMarginTmp)
                    {
                        double theSign = (dVal - dataMeanTmp) / Math.Abs(dVal - dataMeanTmp);
                        return double.NaN;
                    }
                    else return dVal;
                }));
                
                stats = new DescriptiveStatistics(dmTmp.Values);
                double dataMean = stats.Mean;
                double standDev = stats.StandardDeviation;
                double deviationMargin = sigmaCount * standDev;

                //dmRes.MapInplace(new Func<double, double>((x) => { return Math.Pow(x, sigmaCount); }));
                //stats = new DescriptiveStatistics(dmRes.Values);
                dmRes.MapInplace(new Func<double,double>((dVal) =>
                {
                    if (Math.Abs(dVal - dataMean) > deviationMargin)
                    {
                        //double theSign = (dVal - dataMean)/Math.Abs(dVal - dataMean);
                        return dataMean;
                    }
                    else return dVal;
                }));

                res.dmImageComponent = dmRes;
            }
            else
            {
                res.dNumber = a.dNumber;
            }

            return res;
        }




        private ImageOperationalValue ImageOperationalValueGrad(ImageOperationalValue a)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType())
            {
                DenseMatrix dmOrig = (DenseMatrix)a.dmImageComponent.Clone();
                DenseMatrix dmyPlus1 = (DenseMatrix)a.dmImageComponent.Clone();
                dmyPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row < dmyPlus1.RowCount - 1)
                    {
                        return dmOrig[row + 1, column];
                    }
                    else return val;
                    
                }));

                DenseMatrix dmyMinus1 = (DenseMatrix)a.dmImageComponent.Clone();
                dmyMinus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row > 1)
                    {
                        return dmOrig[row - 1, column];
                    }
                    else return val;

                }));

                

                //DenseMatrix dmRes = DenseMatrix.Create(dmyPlus1.RowCount, dmyPlus1.ColumnCount, new Func<int,int,double>((x, y) => 0.0d));
                DenseMatrix dmGradY = (dmyPlus1 - dmyMinus1);
                dmGradY.MapInplace(new Func<double, double>(x => x/2.0d));
                //dmGradY.MapInplace(new Func<double,double>((val) => val/2.0d));
                
                dmyPlus1 = null;
                //dmyMinus1 = null;
                ServiceTools.FlushMemory();

                DenseMatrix dmxPlus1 = (DenseMatrix)a.dmImageComponent.Clone();
                dmxPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column < dmxPlus1.ColumnCount - 1)
                    {
                        return dmOrig[row, column + 1];
                    }
                    else return val;

                }));

                DenseMatrix dmxMinus1 = (DenseMatrix)a.dmImageComponent.Clone();
                dmxMinus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column > 1)
                    {
                        return dmOrig[row, column - 1];
                    }
                    else return val;

                }));

                DenseMatrix dmGradX = dmxPlus1 - dmxMinus1;
                dmGradX.MapInplace(new Func<double, double>(x => x/2.0d));
                //dmGradX.MapInplace(new Func<double, double>((val) => val/2.0d));

                dmxPlus1 = null;
                //dmxMinus1 = null;
                ServiceTools.FlushMemory();

                dmGradY.MapInplace(new Func<double, double>((val) => val * val));
                dmGradX.MapInplace(new Func<double, double>((val) => val * val));
                DenseMatrix dmRes = dmGradX + dmGradY;
                dmRes.MapInplace(new Func<double, double>(val => Math.Sqrt(val)));
                //dmRes.MapIndexedInplace(new Func<int,int,double,double>((x, y, val) => { return x+y; }));
                res.dmImageComponent = dmRes;
                dmGradY = null;
                dmGradX = null;
                ServiceTools.FlushMemory();
            }
            else
            {
                res.dNumber = 0.0d;
            }

            return res;
        }




        private ImageOperationalValue ImageOperationalValueDDX(ImageOperationalValue a)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType())
            {
                DenseMatrix dmOrig = (DenseMatrix)a.dmImageComponent.Clone();
                DenseMatrix dmxPlus1 = (DenseMatrix)a.dmImageComponent.Clone();
                DenseMatrix dmxMinus1 = (DenseMatrix)a.dmImageComponent.Clone();
                dmxPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column < dmxPlus1.ColumnCount - 1)
                    {
                        return dmOrig[row, column + 1];
                    }
                    else return val;

                }));

                dmxMinus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column > 0)
                    {
                        return dmOrig[row, column - 1];
                    }
                    else return val;

                }));

                DenseMatrix dmGradX = dmxPlus1 - dmxMinus1;
                dmGradX.MapInplace(new Func<double, double>(x => x/2.0d));
                dmxPlus1 = null;
                ServiceTools.FlushMemory();

                res.dmImageComponent = dmGradX;
                dmGradX = null;
                ServiceTools.FlushMemory();
            }
            else
            {
                res.dNumber = 0.0d;
            }

            return res;
        }




        private ImageOperationalValue ImageOperationalValueDDY(ImageOperationalValue a)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType())
            {
                DenseMatrix dmOrig = (DenseMatrix)a.dmImageComponent.Clone();
                DenseMatrix dmyPlus1 = (DenseMatrix)a.dmImageComponent.Clone();
                dmyPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row < dmyPlus1.RowCount - 1)
                    {
                        return dmOrig[row+1, column];
                    }
                    else return val;

                }));

                DenseMatrix dmyMinus1 = (DenseMatrix)a.dmImageComponent.Clone();
                dmyMinus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row > 0)
                    {
                        return dmOrig[row - 1, column];
                    }
                    else return val;

                }));



                DenseMatrix dmGradY = dmyPlus1 - dmyMinus1;
                dmGradY.MapInplace(new Func<double, double>(x => x/2.0d));
                dmyPlus1 = null;
                ServiceTools.FlushMemory();

                res.dmImageComponent = dmGradY;
                dmGradY = null;
                ServiceTools.FlushMemory();
            }
            else
            {
                res.dNumber = 0.0d;
            }

            return res;
        }




        private ImageOperationalValue ImageOperationalValueSqrt(ImageOperationalValue a)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType())
            {
                DenseMatrix dmResLocal = (DenseMatrix)a.dmImageComponent.Clone();
                dmResLocal.MapInplace(new Func<double, double>((val) => Math.Pow(val, 0.5d)));
                res.dmImageComponent = dmResLocal;
                ServiceTools.FlushMemory();
            }
            else
            {
                res.dNumber = Math.Sqrt(a.dNumber);
            }

            return res;
        }


        private ImageOperationalValue ImageOperationalValueMean(ImageOperationalValue a)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType())
            {
                DenseMatrix dmResLocal = (DenseMatrix)a.dmImageComponent.Clone();
                DescriptiveStatistics stat = new DescriptiveStatistics(dmResLocal.Values);
                res.dNumber = stat.Mean;
                ServiceTools.FlushMemory();
            }
            else
            {
                res.dNumber = a.dNumber;
            }

            return res;
        }




        private ImageOperationalValue ImageOperationalValueSigm(ImageOperationalValue a)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType())
            {
                DenseMatrix dmResLocal = (DenseMatrix)a.dmImageComponent.Clone();
                DescriptiveStatistics stat = new DescriptiveStatistics(dmResLocal.Values);
                res.dNumber = stat.StandardDeviation;
                ServiceTools.FlushMemory();
            }
            else
            {
                res.dNumber = 0.0d;
            }

            return res;
        }



        private ImageOperationalValue ImageOperationalValueAbs(ImageOperationalValue a)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType())
            {
                DenseMatrix dmResLocal = (DenseMatrix)a.dmImageComponent.Clone();
                dmResLocal.MapInplace(new Func<double, double>((val) => Math.Abs(val)));
                res.dmImageComponent = dmResLocal;
                ServiceTools.FlushMemory();
            }
            else
            {
                res.dNumber = Math.Abs(a.dNumber);
            }

            return res;
        }



        private void pushImageToStackByColorChar(Stack<ImageOperationalValue> stack, string inStr)
        {
            ImageOperationalValue imgIOV = new ImageOperationalValue();


            switch (inStr)
            {
                case "R":
                    imgIOV.dmImageComponent = dmR;
                    break;
                case "G":
                    imgIOV.dmImageComponent = dmG;
                    break;
                case "B":
                    imgIOV.dmImageComponent = dmB;
                    break;
                case "Y":
                    imgIOV.dmImageComponent = dmY;
                    break;
                default:
                    {
                        imgIOV.dNumber = Convert.ToDouble(inStr.Replace(".", ","));
                        break;
                    }
            }
            stack.Push(imgIOV);
        }



        public static DenseMatrix matrixWithValue(DenseMatrix metricMatrix, double dValue)
        {
            DenseMatrix dmTmp = DenseMatrix.Create(metricMatrix.RowCount, metricMatrix.ColumnCount, new Func<int, int, double>((x, y) => { return dValue; }));
            return dmTmp;
        }

        public void Dispose()
        {
            //imgR.Dispose();
            //imgG.Dispose();
            //imgB.Dispose();
            dmR = null;
            dmG = null;
            dmB = null;
            dmRes = null;
            ServiceTools.FlushMemory();
            //throw new NotImplementedException();
        }
    }
}