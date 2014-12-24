﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;


namespace SkyImagesAnalyzerLibraries
{
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the testing point is inside the circle.
        /// </summary>
        /// <param name="pt">The testing point.</param>
        /// <param name="theCircle">The circle.</param>
        /// <returns>System.Int32:
        /// -1 => pt is outside the circle
        /// 0 => pt is at the circle margin
        /// 1 => pt is inside the circle
        /// default (for empty circle data) is outside
        /// </returns>
        public static int IsPointInsideCircle(this PointD pt, RoundData theCircle, double marginDetectorPrecision = 2.0d)
        {
            if (theCircle.IsNull) return -1; // default is outside

            if (PointD.Distance(pt, theCircle.pointDCircleCenter()) < theCircle.DRadius) return 1;
            if (Math.Abs(PointD.Distance(pt, theCircle.pointDCircleCenter()) - theCircle.DRadius) <= marginDetectorPrecision) return 0;
            return -1;
        }




        
        public static PointD GetMouseEventPositionOnRealImage<T1, T2>(this PictureBox pbElement, EventArgs mouseEventArgs,
            Image<T1, T2> origImage)
            where T1: struct, IColor
            where T2: new()
        {
            // пересчитаем точку из PictureBox в картинку, которая в нем
            PointD retPointD = new PointD();
            PointD mouseClickPoint = new PointD(((MouseEventArgs)mouseEventArgs).Location);
            Size currPresentedImageSize = pbElement.Image.Size;
            retPointD.X = (mouseClickPoint.X / (double)currPresentedImageSize.Width) * (double)origImage.Width;
            retPointD.Y = (mouseClickPoint.Y / (double)currPresentedImageSize.Height) * (double)origImage.Height;
            return retPointD;
        }



        public static DenseVector ToVector(this PointD p0, PointD p1)
        {
            DenseVector dv1 = DenseVector.Create(2, i => (i == 0) ? (p1.X - p0.X) : (p1.Y - p0.Y));
            return dv1;
        }



        public static string SaveNetCDFdataMatrix(this DenseMatrix dmSource, string fileName, string varName = "dataMatrix")
        {
            if (fileName != "")
            {
                try
                {
                    NetCDFoperations.AddDataMatrixToFile(dmSource, fileName, null, true, varName);
                    return fileName;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return "";
        }


        public static DictionaryBindingList<TKey, TValue>
            ToBindingList<TKey, TValue>(this IDictionary<TKey, TValue> data)
        {
            return new DictionaryBindingList<TKey, TValue>(data);
        }




        public static DenseMatrix Conv2(this DenseMatrix dmSource, DenseMatrix dmKernel)
        {
            int kernelHalfSizeRows = (dmKernel.RowCount - 1)/2;
            int kernelHalfSizeCols = (dmKernel.ColumnCount - 1)/2;
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
                    (DenseMatrix) dmKernel.SubMatrix(kernelStartRow, rowCount, kernelStartCol, colCount);
                DenseMatrix sumMatrix = (DenseMatrix)(dmSourceSubmatrix.PointwiseMultiply(dmKernelSubMatrix));
                return sumMatrix.Values.Sum();
            });
            return dmRes;
        }

    }




    public class DictionaryBindingList<TKey, TValue>
            : BindingList<Pair<TKey, TValue>>
    {
        private readonly IDictionary<TKey, TValue> data;
        public DictionaryBindingList(IDictionary<TKey, TValue> data)
        {
            this.data = data;
            Reset();
        }
        public void Reset()
        {
            bool oldRaise = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            try
            {
                Clear();
                foreach (TKey key in data.Keys)
                {
                    Add(new Pair<TKey, TValue>(key, data));
                }
            }
            finally
            {
                RaiseListChangedEvents = oldRaise;
                ResetBindings();
            }
        }
    }




    public sealed class Pair<TKey, TValue>
    {
        private readonly TKey key;
        private readonly IDictionary<TKey, TValue> data;
        public Pair(TKey key, IDictionary<TKey, TValue> data)
        {
            this.key = key;
            this.data = data;
        }
        public TKey Key { get { return key; } }
        public TValue Value
        {
            get
            {
                TValue value;
                data.TryGetValue(key, out value);
                return value;
            }
            set { data[key] = value; }
        }
    }
}
