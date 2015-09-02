using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public class MLhelper<T>
    {
        public static IEnumerable<IEnumerable<int>> createDataPartitionIndexes(IEnumerable<T> dataToSplit, IEnumerable<double> ratios)
        {
            List<double> ratiosNormed = new List<double>(ratios);
            if (ratiosNormed.Sum() != 1.0d)
            {
                double sum = ratiosNormed.Sum();
                ratiosNormed = ratiosNormed.ConvertAll(dval => dval/sum);
            }

            Random rnd = new Random();
            List<int> indexes = dataToSplit.Select((val, idx) => idx).ToList();
            indexes = new List<int>(indexes.OrderBy(x => rnd.Next()));
            List<List<int>> partitionsIdxes = new List<List<int>>();

            int currRatioElementNumber = 0;
            int firstIdx = 0;
            foreach (double ratio in ratiosNormed)
            {
                int lastIdx = firstIdx + Convert.ToInt32(ratio*indexes.Count);
                if (currRatioElementNumber == ratiosNormed.Count-1)
                {
                    lastIdx = indexes.Count - 1;
                }

                if (lastIdx > indexes.Count - 1)
                {
                    lastIdx = indexes.Count - 1;
                }
                List<int> currPartition = new List<int>();
                currPartition.AddRange(indexes.GetRange(firstIdx, lastIdx - firstIdx+1));
                partitionsIdxes.Add(currPartition);

                firstIdx = lastIdx;
                currRatioElementNumber++;
            }

            return partitionsIdxes;
        }
    }
}
