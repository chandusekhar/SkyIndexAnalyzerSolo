﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

namespace SkyIndexAnalyzerLibraries
{
    public class HistogramDataAndProperties
    {
        public DenseVector dvData = DenseVector.Create(1, i => 0.0d);
        private int binsCount = 10;
        public Color color = Color.Black;
        public string description = "histogram";

        public DescriptiveStatistics stats;
        public DenseVector dvbinsCenters;
        public DenseVector dvProbDens;
        private int _quantilesCount = 10;
        public List<double> quantilesList = new List<double>();


        public HistogramDataAndProperties()
        {
            dvData = DenseVector.Create(1, i => 0.0d);
            binsCount = 1;
            color = Color.Black;
            description = "histogram";
            dvbinsCenters = (DenseVector)dvData.Clone();
            stats = new DescriptiveStatistics(dvData);
            calculateProbDens();
            CalculateQuantiles();
        }

        public HistogramDataAndProperties(DenseVector data, int countOfBins)
        {
            dvData = (DenseVector)data.Clone();
            binsCount = countOfBins;
            color = Color.Black;
            description = "histogram";
            stats = new DescriptiveStatistics(dvData);
            calculateProbDens();
            CalculateQuantiles();
        }

        public int BinsCount
        {
            get { return binsCount; }
            set
            {
                if (value > 0)
                {
                    binsCount = value;
                    calculateProbDens();
                }
            }
        }

        public int quantilesCount
        {
            get { return _quantilesCount; }
            set
            {
                _quantilesCount = value;
                CalculateQuantiles();
            }
        }


        private void CalculateQuantiles()
        {
            quantilesList = new List<double>();
            for (int i = 0; i <= _quantilesCount; i++)
            {
                double currQuantile = stats.Minimum;
                if (i > 0)
                {
                    double perc = (double) i/(double) _quantilesCount;
                    currQuantile = Statistics.Quantile(dvData, perc);
                }

                quantilesList.Add(currQuantile);
            }
        }




        private void calculateProbDens()
        {
            if (dvData.Count == 0) return;
            if (binsCount == 0) return;

            double dataMaxValue = dvData.Values.Max();
            double dataMinValue = dvData.Values.Min();
            double binCentersStep = (dataMaxValue - dataMinValue) / (double)binsCount;
            double currentBinCenter = dataMinValue + binCentersStep / 2.0d;

            dvbinsCenters = DenseVector.Create(binsCount, i =>
            {
                return (double)i * binCentersStep + binCentersStep * 0.5d + dataMinValue;
            });

            dvProbDens = DenseVector.Create(binsCount, i =>
            {
                double binMin = dvbinsCenters[i] - binCentersStep * 0.5d;
                double binMax = dvbinsCenters[i] + binCentersStep * 0.5d;
                List<double> listCurrData = new List<double>();
                if (i == binsCount-1)
                {
                    listCurrData = DataAnalysis.DataListedWithCondition(dvData, (dVal => ((dVal >= binMin) && (dVal <= binMax))));
                    
                }
                else
                {
                    listCurrData = DataAnalysis.DataListedWithCondition(dvData, (dVal => ((dVal >= binMin) && (dVal < binMax))));
                }
                
                if (listCurrData == null) return 0.0d;
                return (double)listCurrData.Count;
            });
            double sum = dvProbDens.Values.Sum();
            dvProbDens = (DenseVector)dvProbDens.Divide(sum);
        }



        public bool IsEmpty()
        {
            if ((dvData.Count == 1) && (dvData[0] == 0.0d)) return true;
            else return false;
        }
    }
}
