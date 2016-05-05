using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ANN
{
    public class Sigmoid
    {
        public static double sigmoid(double x)
        {
            return 1.0d/(1.0d + Math.Exp(-x));
        }



        public static DenseVector sigmoid(DenseVector x)
        {
            return (DenseVector) x.Map(sigmoid);
        }


        public static DenseMatrix sigmoid(DenseMatrix x)
        {
            return (DenseMatrix)x.Map(sigmoid);
        }
    }
}
