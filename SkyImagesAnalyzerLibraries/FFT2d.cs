using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.LinearAlgebra;

namespace SkyImagesAnalyzerLibraries
{
    public class FFT2d
    {
        private MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmInput;
        private MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix dmComplexInput;
        private MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix dmResultComplex;
        public MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmOutputReal;
        public MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmOutputImaginary;


        public FFT2d(MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dm2Process)
        {
            dmInput = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)dm2Process.Clone();
            dmComplexInput = new MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix(dmInput.RowCount,
                dmInput.ColumnCount);
            dmComplexInput.MapIndexedInplace(new Func<int,int,Complex,Complex>(
                (row, column, val) =>
                {
                    return new Complex(dmInput[row, column], 0.0d);
                }));
        }



        public FFT2d(MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmReal, MathNet.Numerics.LinearAlgebra.Double.DenseMatrix dmImag)
        {
            dmInput = null;
            dmComplexInput = MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix.Create(dmReal.RowCount,
                dmReal.ColumnCount, new Func<int,int,Complex>((row, column) =>
                {
                    return new Complex(dmReal[row, column], dmImag[row, column]);
                }));
        }



        public FFT2d(MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix dm2Process)
        {
            dmInput = null;
            dmComplexInput = (MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix)dm2Process.Clone();
        }



        public void FFT2dForward()
        {
            dmResultComplex = new MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix(dmComplexInput.RowCount,
                dmComplexInput.ColumnCount, new Complex[] {new Complex(0.0d, 0.0d)});


            IEnumerable<Tuple<int, Vector<Complex>>> columnEnumerator =
                dmComplexInput.EnumerateColumnsIndexed();
            foreach (Tuple<int, Vector<Complex>> theColumnTuple in columnEnumerator)
            {
                Vector<Complex> theVector = theColumnTuple.Item2;
                Complex[] theVectorArray = theVector.ToArray();
                Fourier.Forward(theVectorArray);
                MathNet.Numerics.LinearAlgebra.Complex.Vector theVectorSpectrum = new MathNet.Numerics.LinearAlgebra.Complex.DenseVector(theVectorArray);
                dmResultComplex.SetColumn(theColumnTuple.Item1, theVectorSpectrum);
            }

            IEnumerable<Tuple<int, Vector<Complex>>> rowEnumerator = dmResultComplex.EnumerateRowsIndexed();
            foreach (Tuple<int, Vector<Complex>> theRowTuple in rowEnumerator)
            {
                Vector<Complex> theVector = theRowTuple.Item2;
                Complex[] theVectorArray = theVector.ToArray();
                Fourier.Forward(theVectorArray);
                Vector<Complex> theVectorSpectrum = new MathNet.Numerics.LinearAlgebra.Complex.DenseVector(theVectorArray);
                dmResultComplex.SetRow(theRowTuple.Item1, theVectorSpectrum);
            }

            dmOutputReal = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(dmResultComplex.RowCount, dmResultComplex.ColumnCount, new Func<int,int,double>(
                (row, column) =>
                {
                    return dmResultComplex[row, column].Real;
                }));
            dmOutputImaginary = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(dmResultComplex.RowCount, dmResultComplex.ColumnCount, new Func<int, int, double>(
                (row, column) =>
                {
                    return dmResultComplex[row, column].Imaginary;
                }));
        }



        public void FFT2dInverse()
        {
            dmResultComplex = new MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix(dmComplexInput.RowCount,
                dmComplexInput.ColumnCount, new Complex[] {new Complex(0.0d, 0.0d)});


            IEnumerable<Tuple<int, Vector<Complex>>> columnEnumerator =
                dmComplexInput.EnumerateColumnsIndexed();
            foreach (Tuple<int, Vector<Complex>> theColumnTuple in columnEnumerator)
            {
                Vector<Complex> theVector = theColumnTuple.Item2;
                Complex[] theVectorArray = theVector.ToArray();
                Fourier.Inverse(theVectorArray);
                Vector<Complex> theVectorSpectrum = new MathNet.Numerics.LinearAlgebra.Complex.DenseVector(theVectorArray);
                dmResultComplex.SetColumn(theColumnTuple.Item1, theVectorSpectrum);
            }

            IEnumerable<Tuple<int, Vector<Complex>>> rowEnumerator =
                dmResultComplex.EnumerateRowsIndexed();
            foreach (Tuple<int, Vector<Complex>> theRowTuple in rowEnumerator)
            {
                Vector<Complex> theVector = theRowTuple.Item2;
                Complex[] theVectorArray = theVector.ToArray();
                Fourier.Inverse(theVectorArray);
                Vector<Complex> theVectorSpectrum = new MathNet.Numerics.LinearAlgebra.Complex.DenseVector(theVectorArray);
                dmResultComplex.SetRow(theRowTuple.Item1, theVectorSpectrum);
            }

            dmOutputReal = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(dmResultComplex.RowCount, dmResultComplex.ColumnCount, new Func<int, int, double>(
                (row, column) =>
                {
                    return dmResultComplex[row, column].Real;
                }));
            dmOutputImaginary = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(dmResultComplex.RowCount, dmResultComplex.ColumnCount, new Func<int, int, double>(
                (row, column) =>
                {
                    return dmResultComplex[row, column].Imaginary;
                }));
        }
    }
}
