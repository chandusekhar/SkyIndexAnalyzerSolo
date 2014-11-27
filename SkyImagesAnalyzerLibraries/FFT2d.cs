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
using MathNet.Numerics.LinearAlgebra.Generic;

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
            dmComplexInput.MapIndexedInplace(new Func<int,int,System.Numerics.Complex,System.Numerics.Complex>(
                (row, column, val) =>
                {
                    return new System.Numerics.Complex(dmInput[row, column], 0.0d);
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
            dmResultComplex = new MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix(dmComplexInput.RowCount, dmComplexInput.ColumnCount, new Complex(0.0d, 0.0d));


            IEnumerable<System.Tuple<int, MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex>>> columnEnumerator = dmComplexInput.ColumnEnumerator();
            foreach (Tuple<int, Vector<Complex>> theColumnTuple in columnEnumerator)
            {
                MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> theVector = theColumnTuple.Item2;
                Complex[] theVectorArray = theVector.ToArray();
                Transform.FourierForward(theVectorArray);
                MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> theVectorSpectrum = new MathNet.Numerics.LinearAlgebra.Complex.DenseVector(theVectorArray);
                dmResultComplex.SetColumn(theColumnTuple.Item1, theVectorSpectrum);
            }

            IEnumerable<System.Tuple<int, MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex>>> rowEnumerator = dmResultComplex.RowEnumerator();
            foreach (Tuple<int, Vector<Complex>> theRowTuple in rowEnumerator)
            {
                MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> theVector = theRowTuple.Item2;
                Complex[] theVectorArray = theVector.ToArray();
                Transform.FourierForward(theVectorArray);
                MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> theVectorSpectrum = new MathNet.Numerics.LinearAlgebra.Complex.DenseVector(theVectorArray);
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
            dmResultComplex = new MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix(dmComplexInput.RowCount, dmComplexInput.ColumnCount, new Complex(0.0d, 0.0d));


            IEnumerable<System.Tuple<int, MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex>>> columnEnumerator = dmComplexInput.ColumnEnumerator();
            foreach (Tuple<int, Vector<Complex>> theColumnTuple in columnEnumerator)
            {
                MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> theVector = theColumnTuple.Item2;
                Complex[] theVectorArray = theVector.ToArray();
                Transform.FourierInverse(theVectorArray);
                MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> theVectorSpectrum = new MathNet.Numerics.LinearAlgebra.Complex.DenseVector(theVectorArray);
                dmResultComplex.SetColumn(theColumnTuple.Item1, theVectorSpectrum);
            }

            IEnumerable<System.Tuple<int, MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex>>> rowEnumerator = dmResultComplex.RowEnumerator();
            foreach (Tuple<int, Vector<Complex>> theRowTuple in rowEnumerator)
            {
                MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> theVector = theRowTuple.Item2;
                Complex[] theVectorArray = theVector.ToArray();
                Transform.FourierInverse(theVectorArray);
                MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> theVectorSpectrum = new MathNet.Numerics.LinearAlgebra.Complex.DenseVector(theVectorArray);
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
