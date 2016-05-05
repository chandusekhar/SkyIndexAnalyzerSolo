using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ANN
{
    public class ThetaMatricesTransformer
    {
        public static DenseVector UnrollMatrices(List<DenseMatrix> ThetaMatrices)
        {
            DenseVector dvRetVal = null;
            foreach (DenseMatrix Tmatrix in ThetaMatrices)
            {
                foreach (Vector<double> vec in Tmatrix.EnumerateRows())
                {
                    dvRetVal = DenseVector.OfEnumerable(dvRetVal.Concat(vec));
                }
            }
            return dvRetVal;
        }




        public static List<DenseMatrix> RollMatrices(IEnumerable<double> ThetaVec, IEnumerable<int> layersConfig)
        {
            List<DenseMatrix> lDMretVal = new List<DenseMatrix>();
            //List<int> layersConfigExceptLast = layersConfig.Where((idx, val) => idx < layersConfig.Count() - 1).ToList();
            List<int> layersConfigAll = layersConfig.ToList();
            List<int> thetaVectorsLengths = new List<int>();

            for (int i = 0; i < layersConfigAll.Count()-1; i++)
            {
                int currentThetaVecLength = layersConfigAll[i + 1]*(layersConfigAll[i] + 1);
                thetaVectorsLengths.Add(currentThetaVecLength);
            }

            List<double> ThetaVecLoc = new List<double>(ThetaVec);
            List<List<double>> thetaVectors = new List<List<double>>();
            foreach (int thetaVectorsLength in thetaVectorsLengths)
            {
                thetaVectors.Add(ThetaVecLoc.Take(thetaVectorsLength).ToList());
                ThetaVecLoc = ThetaVecLoc.Skip(thetaVectorsLength).ToList();
            }

            int idx1 = 0;
            foreach (List<double> currThetaVector in thetaVectors)
            {
                DenseMatrix dmCurrTheta = new DenseMatrix(layersConfigAll[idx1 + 1],
                    layersConfigAll[idx1] + 1, currThetaVector.ToArray());
                idx1++;
                lDMretVal.Add(dmCurrTheta);
            }

            return lDMretVal;
        }
    }
}
