using System.Windows.Forms;
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
    enum OperandTypes
    {
        DoubleValue,
        DenseMatrix,
        ListOfDensematrices
    }



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
        public List<DenseMatrix> lDMRes = null;
        public DenseMatrix dmRes = null;
        public double resValue = 0.0d;
        public DenseMatrix dmY = null;
        //public MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmMask;

        public ArithmeticsOnImages() { }

        private class ImageOperationalValue
        {
            public DenseMatrix dmImageComponent;
            public List<DenseMatrix> lDMOtherComponents = null;
            public double dNumber;

            public ImageOperationalValue()
            {
                dNumber = 0.0d;
                dmImageComponent = null;
                lDMOtherComponents = null;
            }

            public DenseMatrix DmImageComponent
            {
                get
                {
                    if (dmImageComponent != null) return dmImageComponent;
                    else if (lDMOtherComponents != null)
                    {
                        return lDMOtherComponents[0];
                    }
                    else return null;
                }
                set { dmImageComponent = value; }
            }


            //true - denseMatrix
            //false - number
            public OperandTypes operandType()
            {
                if (dmImageComponent != null) return OperandTypes.DenseMatrix;
                if (lDMOtherComponents != null) return OperandTypes.ListOfDensematrices;
                return OperandTypes.DoubleValue;
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
                    try
                    {
                        str = queue.Dequeue();
                    }
                    catch (Exception)
                    {
                        break;
                    }
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
                            case "grad5p":
                                {
                                    ImageOperationalValue a = stack.Pop();
                                    summ = ImageOperationalValueGrad5p(a);// grad 5-point(a)
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
            dmRes = theResult.DmImageComponent;
            resValue = theResult.dNumber;
            lDMRes = theResult.lDMOtherComponents;

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

            if ((a.operandType() == OperandTypes.DenseMatrix) && (b.operandType() == OperandTypes.DenseMatrix))
            {
                summ.DmImageComponent = b.DmImageComponent - a.DmImageComponent;
            }
            else if ((a.operandType() == OperandTypes.DenseMatrix) && (b.operandType() == OperandTypes.DoubleValue))
            {

                summ.DmImageComponent = matrixWithValue(a.DmImageComponent, b.dNumber) - a.DmImageComponent;
            }
            else if ((b.operandType() == OperandTypes.DenseMatrix) && (a.operandType() == OperandTypes.DoubleValue))
            {
                summ.DmImageComponent = b.DmImageComponent - matrixWithValue(b.DmImageComponent, a.dNumber);
            }


            return summ;
        }


        //b * a
        private ImageOperationalValue ImageOperationalValueMult(ImageOperationalValue a, ImageOperationalValue b)
        {
            ImageOperationalValue summ = new ImageOperationalValue();

            if ((a.operandType() == OperandTypes.DenseMatrix) && (b.operandType() == OperandTypes.DenseMatrix))
            {
                summ.DmImageComponent = (DenseMatrix)b.DmImageComponent.PointwiseMultiply(a.DmImageComponent);
            }
            else if ((a.operandType() == OperandTypes.DenseMatrix) && (b.operandType() == OperandTypes.DoubleValue))
            {
                summ.DmImageComponent = (DenseMatrix)a.DmImageComponent.PointwiseMultiply(matrixWithValue(a.DmImageComponent, b.dNumber));
            }
            else if ((b.operandType() == OperandTypes.DenseMatrix) && (a.operandType() == OperandTypes.DoubleValue))
            {
                summ.DmImageComponent = (DenseMatrix)b.DmImageComponent.PointwiseMultiply(matrixWithValue(b.DmImageComponent, a.dNumber));
            }


            return summ;
        }


        //b / a
        private ImageOperationalValue ImageOperationalValueDivide(ImageOperationalValue a, ImageOperationalValue b)
        {
            ImageOperationalValue summ = new ImageOperationalValue();

            if ((a.operandType() == OperandTypes.DenseMatrix) && (b.operandType() == OperandTypes.DenseMatrix))
            {
                DenseMatrix dmTmpMatrix = (DenseMatrix)a.DmImageComponent.Clone();
                dmTmpMatrix.MapInplace(new Func<double, double>((val) => { return (val == 0.0) ? (1.0) : (val); }));
                summ.DmImageComponent = (DenseMatrix)b.DmImageComponent.PointwiseDivide(dmTmpMatrix);
            }
            else if ((a.operandType() == OperandTypes.DenseMatrix) && (b.operandType() == OperandTypes.DoubleValue))
            {
                DenseMatrix dmTmpMatrix = (DenseMatrix)a.DmImageComponent.Clone();
                dmTmpMatrix.MapInplace(new Func<double, double>((val) => { return (val == 0.0) ? (1.0) : (val); }));
                summ.DmImageComponent = (DenseMatrix)matrixWithValue(a.DmImageComponent, b.dNumber).PointwiseDivide(dmTmpMatrix);
            }
            else if ((b.operandType() == OperandTypes.DenseMatrix) && (a.operandType() == OperandTypes.DoubleValue))
            {
                DenseMatrix dmTmpMatrix = (DenseMatrix)matrixWithValue(b.DmImageComponent, a.dNumber);
                dmTmpMatrix.MapInplace(new Func<double, double>((val) => { return (val == 0.0) ? (1.0) : (val); }));
                summ.DmImageComponent = (DenseMatrix)b.DmImageComponent.PointwiseDivide(dmTmpMatrix);
            }


            return summ;
        }


        //b + a
        private ImageOperationalValue ImageOperationalValueAdd(ImageOperationalValue a, ImageOperationalValue b)
        {
            ImageOperationalValue summ = new ImageOperationalValue();

            if ((a.operandType() == OperandTypes.DenseMatrix) && (b.operandType() == OperandTypes.DenseMatrix))
            {
                summ.DmImageComponent = a.DmImageComponent + b.DmImageComponent;
            }
            else if ((a.operandType() == OperandTypes.DenseMatrix) && (b.operandType() == OperandTypes.DoubleValue))
            {
                summ.DmImageComponent = a.DmImageComponent + matrixWithValue(a.DmImageComponent, b.dNumber);
            }
            else if ((b.operandType() == OperandTypes.DenseMatrix) && (a.operandType() == OperandTypes.DoubleValue))
            {
                summ.DmImageComponent = b.DmImageComponent + matrixWithValue(b.DmImageComponent, a.dNumber);
            }


            return summ;
        }



        private ImageOperationalValue ImageOperationalValuePow(ImageOperationalValue a, double powerExponent)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmRes = (DenseMatrix)a.DmImageComponent.Clone();
                dmRes.MapInplace(new Func<double, double>((x) => { return Math.Pow(x, powerExponent); }));
                res.DmImageComponent = dmRes;
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
            

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmRes = (DenseMatrix)a.DmImageComponent.Clone();

                //determining the new mean value
                DenseMatrix dmTmp = (DenseMatrix)a.DmImageComponent.Clone();
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

                res.DmImageComponent = dmRes;
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

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmOrig = (DenseMatrix)a.DmImageComponent.Clone();
                DenseMatrix dmyPlus1 = (DenseMatrix)a.DmImageComponent.Clone();
                dmyPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row < dmyPlus1.RowCount - 1)
                    {
                        return dmOrig[row + 1, column];
                    }
                    else return val;
                    
                }));

                DenseMatrix dmyMinus1 = (DenseMatrix)a.DmImageComponent.Clone();
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

                DenseMatrix dmxPlus1 = (DenseMatrix)a.DmImageComponent.Clone();
                dmxPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column < dmxPlus1.ColumnCount - 1)
                    {
                        return dmOrig[row, column + 1];
                    }
                    else return val;

                }));

                DenseMatrix dmxMinus1 = (DenseMatrix)a.DmImageComponent.Clone();
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
                res.DmImageComponent = dmRes;
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





        private ImageOperationalValue ImageOperationalValueGrad5p(ImageOperationalValue a)
        {
            ImageOperationalValue res = new ImageOperationalValue();

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmOrig = a.DmImageComponent.Copy();
                DenseMatrix dmyPlus1 = a.DmImageComponent.Copy();
                dmyPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row < dmyPlus1.RowCount - 1)
                    {
                        return dmOrig[row + 1, column];
                    }
                    else return val;

                }));
                DenseMatrix dmyPlus2 = a.DmImageComponent.Copy();
                dmyPlus2.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row < dmyPlus2.RowCount - 2)
                    {
                        return dmOrig[row + 2, column];
                    }
                    else return val;

                }));

                DenseMatrix dmyMinus1 = a.DmImageComponent.Copy();
                dmyMinus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row > 1)
                    {
                        return dmOrig[row - 1, column];
                    }
                    else return val;

                }));
                DenseMatrix dmyMinus2 = a.DmImageComponent.Copy();
                dmyMinus2.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row > 2)
                    {
                        return dmOrig[row - 2, column];
                    }
                    else return val;

                }));


                DenseMatrix dmGradY = dmyMinus2 - 8.0d*dmyMinus1 + 8.0d*dmyPlus1 - dmyPlus2;
                dmGradY.MapInplace(x => x / 12.0d);

                dmyPlus1 = null;
                dmyMinus1 = null;
                dmyPlus2 = null;
                dmyMinus2 = null;
                ServiceTools.FlushMemory();

                DenseMatrix dmxPlus1 = a.DmImageComponent.Copy();
                dmxPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column < dmxPlus1.ColumnCount - 1)
                    {
                        return dmOrig[row, column + 1];
                    }
                    else return val;

                }));

                DenseMatrix dmxPlus2 = a.DmImageComponent.Copy();
                dmxPlus2.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column < dmxPlus2.ColumnCount - 2)
                    {
                        return dmOrig[row, column + 2];
                    }
                    else return val;
                }));

                DenseMatrix dmxMinus1 = a.DmImageComponent.Copy();
                dmxMinus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column > 1)
                    {
                        return dmOrig[row, column - 1];
                    }
                    else return val;

                }));

                DenseMatrix dmxMinus2 = a.DmImageComponent.Copy();
                dmxMinus2.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (column > 2)
                    {
                        return dmOrig[row, column - 2];
                    }
                    else return val;

                }));

                DenseMatrix dmGradX = dmxMinus2 - 8.0d*dmxMinus1 + 8.0d*dmxPlus1 - dmxPlus2;
                dmGradX.MapInplace(x => x / 12.0d);

                dmxPlus1 = null;
                dmxMinus1 = null;
                dmxMinus2 = null;
                dmxPlus2 = null;
                ServiceTools.FlushMemory();

                res.lDMOtherComponents = new List<DenseMatrix>() { dmGradX, dmGradY };
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

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmOrig = (DenseMatrix)a.DmImageComponent.Clone();
                DenseMatrix dmxPlus1 = (DenseMatrix)a.DmImageComponent.Clone();
                DenseMatrix dmxMinus1 = (DenseMatrix)a.DmImageComponent.Clone();
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

                res.DmImageComponent = dmGradX;
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

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmOrig = (DenseMatrix)a.DmImageComponent.Clone();
                DenseMatrix dmyPlus1 = (DenseMatrix)a.DmImageComponent.Clone();
                dmyPlus1.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
                {
                    if (row < dmyPlus1.RowCount - 1)
                    {
                        return dmOrig[row+1, column];
                    }
                    else return val;

                }));

                DenseMatrix dmyMinus1 = (DenseMatrix)a.DmImageComponent.Clone();
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

                res.DmImageComponent = dmGradY;
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

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmResLocal = (DenseMatrix)a.DmImageComponent.Clone();
                dmResLocal.MapInplace(new Func<double, double>((val) => Math.Pow(val, 0.5d)));
                res.DmImageComponent = dmResLocal;
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

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmResLocal = (DenseMatrix)a.DmImageComponent.Clone();
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

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmResLocal = (DenseMatrix)a.DmImageComponent.Clone();
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

            if (a.operandType() == OperandTypes.DenseMatrix)
            {
                DenseMatrix dmResLocal = (DenseMatrix)a.DmImageComponent.Clone();
                dmResLocal.MapInplace(new Func<double, double>((val) => Math.Abs(val)));
                res.DmImageComponent = dmResLocal;
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
                    imgIOV.DmImageComponent = dmR;
                    break;
                case "G":
                    imgIOV.DmImageComponent = dmG;
                    break;
                case "B":
                    imgIOV.DmImageComponent = dmB;
                    break;
                case "Y":
                    imgIOV.DmImageComponent = dmY;
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
