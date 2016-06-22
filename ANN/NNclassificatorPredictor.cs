﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ANN
{
    public class NNclassificatorPredictor
    {
        public static IEnumerable<int> NNpredict(DenseMatrix X, IEnumerable<double> ThetaValuesVector, IEnumerable<int> NNlayersConfig, out List<List<double>> lDecisionProbabilities)
        {
            bool bConsolePresent = console_present();
            List<DenseMatrix> Theta = ThetaMatricesTransformer.RollMatrices(ThetaValuesVector, NNlayersConfig);
            int m = X.RowCount;
            int n = X.ColumnCount;
            int L = NNlayersConfig.Count();
            int num_labels = NNlayersConfig.Last();

            List<LayersActivations> activations = new List<LayersActivations>();

            for (int l = 0; l < L; l++)
            {
                if (l == 0)
                {
#if DEBUG
                    if (bConsolePresent)
                    {
                        Console.WriteLine("ANN prediction calculating: layer " + l + " of " + L);
                    }
#endif

                    int m_a = X.RowCount;
                    DenseMatrix a = (DenseMatrix)X.Clone();
                    DenseVector bias = DenseVector.Create(m_a, 1.0d);
                    DenseMatrix a_biased = (DenseMatrix)a.InsertColumn(0, bias);
                    activations.Add(new LayersActivations()
                    {
                        a = a,
                        m_a = m_a,
                        a_biased = a_biased,
                        z = null
                    });
                }
                else
                {
#if DEBUG
                    if (bConsolePresent)
                    {
                        Console.WriteLine("ANN prediction calculating: layer " + l + " of " + L);
                    }
#endif

                    LayersActivations predActivation = activations.Last();
                    DenseMatrix z = predActivation.a_biased*(DenseMatrix) Theta[l - 1].Transpose();
                    DenseMatrix a = Sigmoid.sigmoid(z);
                    int m_a = a.RowCount;
                    DenseVector bias = DenseVector.Create(m_a, 1.0d);
                    DenseMatrix a_biased = (DenseMatrix)a.InsertColumn(0, bias);
                    activations.Add(new LayersActivations()
                    {
                        a = a,
                        m_a = m_a,
                        a_biased = a_biased,
                        z = z
                    });
                }
            }

            DenseMatrix h = activations.Last().a;
            lDecisionProbabilities = h.EnumerateRows().ToList().ConvertAll(vect => new List<double>(vect));
            List<int> res = h.EnumerateRows().ToList().ConvertAll(vec => vec.AbsoluteMaximumIndex()+1);
            return res;
        }




        public static IEnumerable<int> NNpredict(DenseMatrix X, IEnumerable<double> ThetaValuesVector, IEnumerable<int> NNlayersConfig)
        {
            List<List<double>> lDecisionProbabilities = null;
            return NNpredict(X, ThetaValuesVector, NNlayersConfig, out lDecisionProbabilities);
        }




        private static bool console_present()
        {
            bool _console_present = true;
            try { int window_height = Console.WindowHeight; }
            catch { _console_present = false; }

            return _console_present;
        }
    }


    public class LayersActivations
    {
        public DenseMatrix a { get; set; }
        public int m_a { get; set; }
        public DenseMatrix a_biased { get; set; }
        public DenseMatrix z { get; set; }


        public LayersActivations() { }
    }
}
