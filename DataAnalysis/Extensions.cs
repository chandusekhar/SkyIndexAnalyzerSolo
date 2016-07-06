using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace DataAnalysis
{
    public static class Extensions
    {
        public enum StandardConvolutionKernels
        {
            cos,
            gauss,
            flat,
            linear,
            bilinear
        }



        public static DenseMatrix Conv2(this DenseMatrix dmSource, DenseMatrix dmKernel)
        {
            int kernelHalfSizeRows = (dmKernel.RowCount - 1) / 2;
            int kernelHalfSizeCols = (dmKernel.ColumnCount - 1) / 2;
            DenseMatrix dmRes = DenseMatrix.Create(dmSource.RowCount, dmSource.ColumnCount, (r, c) =>
            {
                int startRow = r - kernelHalfSizeRows;
                int dmSourceStartRow = Math.Max(startRow, 0);
                int kernelStartRow = (startRow >= 0) ? (0) : (-startRow);
                int endRow = r + kernelHalfSizeRows;
                int dmSourceEndRow = Math.Min(endRow, dmSource.RowCount - 1);
                int rowCount = dmSourceEndRow - dmSourceStartRow + 1;
                int startCol = c - kernelHalfSizeCols;
                int dmSourceStartCol = Math.Max(startCol, 0);
                int kernelStartCol = (startCol >= 0) ? (0) : (-startCol);
                int endCol = c + kernelHalfSizeCols;
                int dmSourceEndCol = Math.Min(endCol, dmSource.ColumnCount - 1);
                int colCount = dmSourceEndCol - dmSourceStartCol + 1;

                DenseMatrix dmSourceSubmatrix = (DenseMatrix)dmSource.SubMatrix(dmSourceStartRow, rowCount, dmSourceStartCol, colCount);
                DenseMatrix dmKernelSubMatrix =
                    (DenseMatrix)dmKernel.SubMatrix(kernelStartRow, rowCount, kernelStartCol, colCount);
                DenseMatrix sumMatrix = (DenseMatrix)(dmSourceSubmatrix.PointwiseMultiply(dmKernelSubMatrix));
                return sumMatrix.Values.Sum();
            });
            return dmRes;
        }




        public static DenseMatrix Conv2(this DenseMatrix dmSource, StandardConvolutionKernels kernelType, int kernelHalfWidth = 10)
        {
            //int kernelHalfLength = Convert.ToInt32(kernelWidth / 2.0d);
            double maxL = ((double)kernelHalfWidth) * Math.Sqrt(2.0d);
            DenseMatrix dmKernel = DenseMatrix.Create(2 * kernelHalfWidth + 1, 2 * kernelHalfWidth + 1, 1.0d);

            if (kernelType == StandardConvolutionKernels.cos)
            {
                dmKernel = DenseMatrix.Create(2 * kernelHalfWidth + 1, 2 * kernelHalfWidth + 1, (r, c) =>
                {
                    double curDist =
                        (new PointD(r - (double)kernelHalfWidth, c - (double)kernelHalfWidth)).Distance(
                            new PointD(0.0d, 0.0d));
                    return Math.Cos(curDist * Math.PI / (2.0d * maxL));
                });
            }
            else if (kernelType == StandardConvolutionKernels.gauss)
            {
                dmKernel = DenseMatrix.Create(2 * kernelHalfWidth + 1, 2 * kernelHalfWidth + 1, (r, c) =>
                {
                    double curDist =
                        (new PointD(r - (double)kernelHalfWidth, c - (double)kernelHalfWidth)).Distance(
                            new PointD(0.0d, 0.0d));
                    return Math.Exp(-curDist * curDist / (2.0d * (maxL / 3.0d)));
                });
            }
            else if (kernelType == StandardConvolutionKernels.flat)
            {
                dmKernel = DenseMatrix.Create(2 * kernelHalfWidth + 1, 2 * kernelHalfWidth + 1, 1.0d);
            }
            else if (kernelType == StandardConvolutionKernels.linear)
            {
                // actually it will be cone
                dmKernel = DenseMatrix.Create(2 * kernelHalfWidth + 1, 2 * kernelHalfWidth + 1, (r, c) =>
                {
                    double curDist =
                        (new PointD(r - (double)kernelHalfWidth, c - (double)kernelHalfWidth)).Distance(
                            new PointD(0.0d, 0.0d));

                    return Math.Max(1.0d - curDist * (1.0d / (double)kernelHalfWidth), 0.0d);
                });
            }
            else if (kernelType == StandardConvolutionKernels.bilinear)
            {
                dmKernel = DenseMatrix.Create(2 * kernelHalfWidth + 1, 2 * kernelHalfWidth + 1, (r, c) =>
                {
                    double curDist =
                        (new PointD(r - (double)kernelHalfWidth, c - (double)kernelHalfWidth)).Distance(
                            new PointD(0.0d, 0.0d));

                    return Math.Max(1.0d - curDist * curDist * (1.0d / (double)(kernelHalfWidth * kernelHalfWidth)), 0.0d);
                });
            }

            double kernelSum = dmKernel.Values.Sum();
            dmKernel.MapInplace(dval => dval / kernelSum);

            return dmSource.Conv2(dmKernel);
        }





        public static DenseVector Conv(this DenseVector dvSource, DenseVector dvKernel)
        {
            int kernelHalfSize = (dvKernel.Count - 1) / 2;

            DenseVector dvRes = DenseVector.Create(dvSource.Count, (idx) =>
            {
                int startIdx = idx - kernelHalfSize;
                int dvSourceStartIdx = Math.Max(startIdx, 0);
                int kernelStartIdx = (startIdx >= 0) ? (0) : (-startIdx);
                int endIdx = idx + kernelHalfSize;
                int dvSourceEndIdx = Math.Min(endIdx, dvSource.Count - 1);
                int elementsCount = dvSourceEndIdx - dvSourceStartIdx + 1;

                DenseVector dvSourceSubvector = (DenseVector)dvSource.SubVector(dvSourceStartIdx, elementsCount);
                DenseVector dvKernelSubvector =
                    (DenseVector)dvKernel.SubVector(kernelStartIdx, elementsCount);
                // renorm dvKernelSubvector values if it is shorter than original
                if (dvKernelSubvector.Count < dvKernel.Count)
                {
                    double dvKernelSubvectorSum = dvKernelSubvector.Sum();
                    dvKernelSubvector = (DenseVector)dvKernelSubvector / dvKernelSubvectorSum;
                }

                DenseVector sumVector = (DenseVector)(dvSourceSubvector.PointwiseMultiply(dvKernelSubvector));
                return sumVector.Values.Sum();
            });
            return dvRes;
        }





        public static DenseVector ConvKernel(StandardConvolutionKernels kernelType, int kernelHalfWidth = 10)
        {
            double maxL = ((double)kernelHalfWidth) * 2.0d;
            DenseVector dvKernel = DenseVector.Create(2 * kernelHalfWidth + 1, 1.0d);

            if (kernelType == StandardConvolutionKernels.cos)
            {
                dvKernel = DenseVector.Create(2 * kernelHalfWidth + 1, (idx) =>
                {
                    double curDist =
                        (new PointD(idx - (double)kernelHalfWidth, 0.0d)).Distance(
                            new PointD(0.0d, 0.0d));
                    return Math.Cos(curDist * Math.PI / (2.0d * maxL));
                });
            }
            else if (kernelType == StandardConvolutionKernels.gauss)
            {
                dvKernel = DenseVector.Create(2 * kernelHalfWidth + 1, (idx) =>
                {
                    double curDist =
                        (new PointD(idx - (double)kernelHalfWidth, 0.0d)).Distance(
                            new PointD(0.0d, 0.0d));
                    return Math.Exp(-curDist * curDist / (2.0d * (maxL * maxL / 9.0d)));
                });
            }
            else if (kernelType == StandardConvolutionKernels.flat)
            {
                dvKernel = DenseVector.Create(2 * kernelHalfWidth + 1, 1.0d);
            }
            else if (kernelType == StandardConvolutionKernels.linear)
            {
                dvKernel = DenseVector.Create(2 * kernelHalfWidth + 1, (idx) =>
                {
                    double curDist =
                        (new PointD(idx - (double)kernelHalfWidth, 0.0d)).Distance(
                            new PointD(0.0d, 0.0d));

                    return Math.Max(1.0d - curDist * (1.0d / (double)kernelHalfWidth), 0.0d);
                });
            }
            else if (kernelType == StandardConvolutionKernels.bilinear)
            {
                dvKernel = DenseVector.Create(2 * kernelHalfWidth + 1, (idx) =>
                {
                    double curDist =
                        (new PointD(idx - (double)kernelHalfWidth, 0.0d)).Distance(
                            new PointD(0.0d, 0.0d));

                    return Math.Max(1.0d - curDist * curDist * (1.0d / (double)(kernelHalfWidth * kernelHalfWidth)), 0.0d);
                });
            }

            double kernelSum = dvKernel.Values.Sum();
            dvKernel.MapInplace(dval => dval / kernelSum);
            return dvKernel;
        }






        public static DenseVector ConvKernelAsymmetric(StandardConvolutionKernels kernelType, int kernelWidth = 10, bool centerToTheRight = true)
        {
            DenseVector dvKernel = ConvKernel(kernelType, kernelWidth - 1);
            if (centerToTheRight)
            {
                dvKernel = (DenseVector)dvKernel.SubVector(0, kernelWidth);
            }
            else
            {
                dvKernel = (DenseVector)dvKernel.SubVector(kernelWidth - 1, kernelWidth);
            }

            double kernelSum = dvKernel.Values.Sum();
            dvKernel.MapInplace(dval => dval / kernelSum);
            return dvKernel;
        }





        public static DenseVector Conv(this DenseVector dvSource, StandardConvolutionKernels kernelType, int kernelHalfWidth = 10)
        {
            DenseVector dvKernel = ConvKernel(kernelType, kernelHalfWidth);

            return dvSource.Conv(dvKernel);
        }




        public static DenseVector MeansPerColumns(this DenseMatrix dmSrc)
        {
            return DataAnalysisStatic.MeansPerColumns(dmSrc);
        }



        public static DenseVector MeansPerRows(this DenseMatrix dmSrc)
        {
            return DataAnalysisStatic.MeansPerRows(dmSrc);
        }



        public static DenseVector STDperColumns(this DenseMatrix dmSrc)
        {
            return DataAnalysisStatic.STDperColumns(dmSrc);
        }



        public static DenseVector STDPerRows(this DenseMatrix dmSrc)
        {
            return DataAnalysisStatic.STDPerRows(dmSrc);
        }


        public static DenseMatrix RemoveRows(this DenseMatrix dmSrc, IEnumerable<int> rowsIndexesToRemove)
        {
            List<Tuple<int, Vector<double>>> lRowsIndexed = dmSrc.EnumerateRowsIndexed().ToList();
            lRowsIndexed.RemoveAll(tpl => rowsIndexesToRemove.Contains(tpl.Item1));

            return DenseMatrix.OfRows(lRowsIndexed.ConvertAll(tpl => tpl.Item2));
        }


        public static DenseMatrix RemoveColumns(this DenseMatrix dmSrc, IEnumerable<int> columnsIndexesToRemove)
        {
            List<Tuple<int, Vector<double>>> lColumnsIndexed = dmSrc.EnumerateColumnsIndexed().ToList();
            lColumnsIndexed.RemoveAll(tpl => columnsIndexesToRemove.Contains(tpl.Item1));

            return DenseMatrix.OfColumns(lColumnsIndexed.ConvertAll(tpl => tpl.Item2));
        }
    }
}
