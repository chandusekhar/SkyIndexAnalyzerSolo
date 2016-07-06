using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ANN
{
    public class LayersActivations
    {
        public DenseMatrix a { get; set; }
        public int m_a { get; set; }
        public DenseMatrix a_biased { get; set; }
        public DenseMatrix z { get; set; }


        public LayersActivations() { }
    }
}
