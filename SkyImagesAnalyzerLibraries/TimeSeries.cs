using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.Random;

namespace SkyImagesAnalyzerLibraries
{
    public class TimeSeries<T>
    {
        private List<T> dataSeria = new List<T>();
        private List<DateTime> lDateTimeStamps = new List<DateTime>();


        public int Count
        {
            get { return dataSeria.Count; }
        }


        public TimeSpan TotalSeriaDuration
        {
            get { return (lDateTimeStamps.Last() - lDateTimeStamps.First()); }
        }



        public TimeSeries()
        {
        }


        public TimeSeries(List<T> dataSeriaList, List<DateTime> dateTimeStamps)
        {
            dataSeria = new List<T>(dataSeriaList);
            lDateTimeStamps = new List<DateTime>(dateTimeStamps);
        }





        public void AddSubseriaData(List<T> lDataToAdd, List<DateTime> lDateTimeStampsRoAdd)
        {
            if (lDataToAdd.Count != lDateTimeStampsRoAdd.Count)
            {
                return;
            }

            // будем сохранять исходный индекс для сохранения исходного порядка значений с равными штампами времени
            // для этого будем запоминать третий член совокупности - исходный индекс
            List<Tuple<DateTime, T, int>> listToSort = new List<Tuple<DateTime, T, int>>();


            for (int i = 0; i < lDateTimeStamps.Count; i++)
            {
                listToSort.Add(new Tuple<DateTime, T, int>(lDateTimeStamps[i], dataSeria[i], i));
            }

            for (int i = 0; i < lDateTimeStampsRoAdd.Count; i++)
            {
                listToSort.Add(new Tuple<DateTime, T, int>(lDateTimeStampsRoAdd[i], lDataToAdd[i], i));
            }


            listToSort.Sort(new Comparison<Tuple<DateTime, T, int>>((tpl1, tpl2) =>
            {
                if (tpl1.Item1 < tpl2.Item1)
                {
                    return -1;
                }
                else if (tpl1.Item1 > tpl2.Item1)
                {
                    return 1;
                }
                else
                {
                    if (tpl1.Item3 < tpl2.Item3)
                    {
                        return -1;
                    }
                    else if (tpl1.Item3 > tpl2.Item3)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }));


            lDateTimeStamps.Clear();
            dataSeria.Clear();
            foreach (Tuple<DateTime, T, int> tpl in listToSort)
            {
                lDateTimeStamps.Add(tpl.Item1);
                dataSeria.Add(tpl.Item2);
            }
        }



        /// <summary>
        /// Splits the timeserie with time span condition.
        /// </summary>
        /// <param name="theSerieBreakCondition">The timespan break condition.</param>
        /// <returns>List&lt;TimeSeries&lt;T&gt;&gt;.</returns>
        public List<TimeSeries<T>> SplitWithTimeSpanCondition(Predicate<TimeSpan> theSerieBreakCondition)
        {
            List<DateTime> lDateTimeStampsShiftedplus1 = new List<DateTime>();
            lDateTimeStampsShiftedplus1.AddRange(lDateTimeStamps);
            lDateTimeStampsShiftedplus1.RemoveAt(0);
            lDateTimeStampsShiftedplus1.Add(lDateTimeStampsShiftedplus1.Last());
            List<TimeSpan> lTimeSpanValues = new List<TimeSpan>(lDateTimeStampsShiftedplus1.Zip(lDateTimeStamps, (dateTimeShifted, dateTime) => dateTimeShifted - dateTime));

            DescriptiveStatistics stats = new DescriptiveStatistics(lTimeSpanValues.ConvertAll<double>(ts => ts.TotalMilliseconds));
            double minstats = stats.Minimum;

            List<TimeSeries<T>> retList = new List<TimeSeries<T>>();

            int previndex = 0;

            for (int i = 1; i < lTimeSpanValues.Count; i++)
            {
                if (theSerieBreakCondition(lTimeSpanValues[i]))
                {
                    if (i - previndex <= 1)
                    {
                        previndex = i;
                        continue;
                    }
                    else
                    {
                        List<DateTime> tmpDTlist = lDateTimeStamps.GetRange(previndex, i - previndex + 1);
                        List<T> tmpDataList = dataSeria.GetRange(previndex, i - previndex + 1);
                        retList.Add(new TimeSeries<T>(tmpDataList, tmpDTlist));

                        previndex = i + 1;
                        i++;
                        continue;
                    }

                }
            }

            return retList;
        }







        /// <summary>
        /// Sunseria from the startIndex with the given items count or till the end of the seria
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <param name="endIndex">The end index.</param>
        /// <returns>TimeSeries&lt;T&gt;.</returns>
        public TimeSeries<T> SubSeria(int startIndex, int itemsCount, out int endIndex)
        {
            if (startIndex + itemsCount > lDateTimeStamps.Count)
            {
                itemsCount = lDateTimeStamps.Count - startIndex;
            }

            List<DateTime> tmpDTlist = lDateTimeStamps.GetRange(startIndex, itemsCount);
            List<T> tmpDataList = dataSeria.GetRange(startIndex, itemsCount);

            endIndex = startIndex + itemsCount - 1;
            return new TimeSeries<T>(tmpDataList, tmpDTlist);
        }




        /// <summary>
        /// Subseria from the startIndex till the end of the seria
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">The end index.</param>
        /// <returns>TimeSeries&lt;T&gt;.</returns>
        public TimeSeries<T> SubSeria(int startIndex, out int endIndex)
        {
            int itemsCount = lDateTimeStamps.Count - startIndex;

            List<DateTime> tmpDTlist = lDateTimeStamps.GetRange(startIndex, itemsCount);
            List<T> tmpDataList = dataSeria.GetRange(startIndex, itemsCount);

            endIndex = startIndex + itemsCount - 1;
            return new TimeSeries<T>(tmpDataList, tmpDTlist);
        }






        public TimeSeries<T> SubSeria(int startIndex, TimeSpan dt, out int endIndex)
        {
            int tmpTSendIndex;
            DateTime startDT = lDateTimeStamps[startIndex];
            int idx = lDateTimeStamps.FindIndex(stamp => (stamp - startDT >= dt));
            if (idx == -1)
            {
                endIndex = lDateTimeStamps.Count - 1;
                return SubSeria(startIndex, out tmpTSendIndex);
            }

            endIndex = idx;
            return SubSeria(startIndex, idx - startIndex + 1, out tmpTSendIndex);
        }



        public void RepositionDuplicatedTimeStamps()
        {
            for (int i = 0; i < lDateTimeStamps.Count-1; i++)
            {
                if (lDateTimeStamps[i] == lDateTimeStamps[i+1])
                {
                    if (i==0)
                    {
                        lDateTimeStamps[i + 1] =
                            lDateTimeStamps[i].AddMilliseconds(((lDateTimeStamps[i + 2] - lDateTimeStamps[i]).TotalMilliseconds/2.0d));
                    }
                    else if (i == lDateTimeStamps.Count - 2)
                    {
                        lDateTimeStamps[i] =
                            lDateTimeStamps[i-1].AddMilliseconds(((lDateTimeStamps[i + 1] - lDateTimeStamps[i-1]).TotalMilliseconds / 2.0d));
                    }
                    else
                    {
                        TimeSpan dt = new TimeSpan(Convert.ToInt64((lDateTimeStamps[i + 2] - lDateTimeStamps[i - 1]).Ticks/3.0d));
                        lDateTimeStamps[i] = lDateTimeStamps[i - 1] + dt;
                        lDateTimeStamps[i + 1] = lDateTimeStamps[i] + dt;
                    }
                }
            }
        }




        public void RemoveDuplicatedTimeStamps()
        {
            List<DateTime> lDateTimeStampsFiltered = new List<DateTime>(lDateTimeStamps.Distinct<DateTime>());

            List<T> dataSeriaFiltered = new List<T>();
            int idx = 0;
            foreach (DateTime dtStamp in lDateTimeStampsFiltered)
            {
                idx = lDateTimeStamps.FindIndex(idx, datetimeValue => datetimeValue == dtStamp);
                dataSeriaFiltered.Add(dataSeria[idx]);
            }

            lDateTimeStamps = lDateTimeStampsFiltered;
            dataSeria = dataSeriaFiltered;
        }




        public void RemoveValues(Predicate<T> falseCondition)
        {
            List<Tuple<DateTime, T>> tuplesList =
                new List<Tuple<DateTime, T>>(Enumerable.Zip(lDateTimeStamps, dataSeria,
                    (dt, dat) => new Tuple<DateTime, T>(dt, dat)));

            List<Tuple<DateTime, T>> filteredTuplesList = tuplesList.FindAll(tpl => !falseCondition(tpl.Item2));


            dataSeria = filteredTuplesList.ConvertAll<T>(tpl => tpl.Item2);
            lDateTimeStamps = filteredTuplesList.ConvertAll<DateTime>(tpl => tpl.Item1);
        }



        public TimeSeries<T> InterpolateSeria(TimeSpan dt)
        {
            if (typeof(T) != typeof(double))
            {
                return null;
            }

            //RepositionDuplicatedTimeStamps();

            RemoveDuplicatedTimeStamps();

            List<DateTime> resDateTimeList = new List<DateTime>();

            DateTime startTime = lDateTimeStamps[0];
            List<double> currentDatetimeList =
                lDateTimeStamps.ConvertAll<double>(time => (time - startTime).TotalMilliseconds);
            //List<double> currentDataList =
            //    dataSeria.ConvertAll<double>(dVal => Convert.ToDouble(dVal));
            
            DateTime endTime = lDateTimeStamps[lDateTimeStamps.Count - 1];
            int dtCount = Convert.ToInt32(((double)((endTime - startTime).TotalMilliseconds) / (double)(dt.TotalMilliseconds)));
            long dtInternalTicks = Convert.ToInt64(((endTime - startTime).Ticks) / (double)dtCount);
            TimeSpan dtInternal = new TimeSpan(dtInternalTicks);

            DateTime t = startTime;
            while (true)
            {
                resDateTimeList.Add(t);
                t += dtInternal;
                if (t > endTime) { break; }
            }

            List<double> dataDoubleValues = dataSeria as List<double>;

            IInterpolation method;
            try
            {
                method = Interpolate.Linear(currentDatetimeList, dataDoubleValues);
            }
            catch (Exception)
            {
                return this;
            }
            
            List<double> interpolatedValues = new List<double>();
            foreach (DateTime time in resDateTimeList)
            {
                interpolatedValues.Add(method.Interpolate((time-startTime).TotalMilliseconds));
            }

            return new TimeSeries<T>(interpolatedValues as List<T>, resDateTimeList);

            //int tmpTSendIndex;
            //DateTime startDT = lDateTimeStamps[startIndex];
            //int idx = lDateTimeStamps.FindIndex(stamp => (stamp - startDT >= dt));
            //if (idx == -1)
            //{
            //    endIndex = lDateTimeStamps.Count - 1;
            //    return SubSeria(startIndex, out tmpTSendIndex);
            //}

            //endIndex = idx;
            //return SubSeria(startIndex, idx - startIndex + 1, out tmpTSendIndex);
        }





        public TimeSeries<T> ExtractDataDeviationValues()
        {
            if (typeof(T) != typeof(double))
            {
                return null;
            }

            DescriptiveStatistics stats = new DescriptiveStatistics(dataSeria as List<double>);

            List<double> lDevValues = (dataSeria as List<double>).ConvertAll<double>(dVal => dVal - stats.Mean);

            return new TimeSeries<T>(lDevValues as List<T>, lDateTimeStamps);
        }






        public Complex[] DataRealValuesComplexArray()
        {
            if (typeof(T) != typeof(double))
            {
                return null;
            }
            List<double> dDataList = dataSeria as List<double>;

            MathNet.Numerics.LinearAlgebra.Complex.DenseVector theVector = MathNet.Numerics.LinearAlgebra.Complex.DenseVector.Create(dataSeria.Count, idx => new Complex(dDataList[idx], 0.0d));
            return theVector.ToArray();
        }



        public DenseVector DataValues
        {
            get
            {
                if (typeof(T) != typeof(double))
                {
                    return null;
                }
                DenseVector dvRetVector = DenseVector.OfEnumerable(dataSeria as List<double>);
                return dvRetVector;
            }
        }



        public DenseVector TimeStampsValuesSeconds
        {
            get
            {
                DateTime startTime = lDateTimeStamps[0];
                List<double> tsValuesList =
                    lDateTimeStamps.ConvertAll<double>(dt => (dt - startTime).TotalMilliseconds/1000.0d);
                DenseVector dvRetVector = DenseVector.OfEnumerable(tsValuesList);
                return dvRetVector;
            }
        }


        public DateTime StartTime
        {
            get
            {
                DateTime startTime = lDateTimeStamps[0];
                return startTime;
            }
        }


        public DateTime EndTime
        {
            get
            {
                DateTime andTime = lDateTimeStamps[lDateTimeStamps.Count-1];
                return andTime;
            }
        }



        public Tuple<DateTime, T> GetMostClose(DateTime dtInFocus)
        {
            Tuple<DateTime, T> retVal = new Tuple<DateTime, T>(lDateTimeStamps[0], dataSeria[0]);

            int idxR = lDateTimeStamps.FindIndex(dt => dtInFocus <= dt);
            if (idxR == 0)
            {
                return new Tuple<DateTime, T>(lDateTimeStamps[0], dataSeria[0]);
            }
            else if (idxR == -1)
            {
                return new Tuple<DateTime, T>(lDateTimeStamps[Count - 1], dataSeria[Count - 1]);
            }

            int idxL = idxR - 1;

            if ((lDateTimeStamps[idxR] - dtInFocus) <= (dtInFocus - lDateTimeStamps[idxL]))
            {
                retVal = new Tuple<DateTime, T>(lDateTimeStamps[idxR], dataSeria[idxR]);
            }
            else
            {
                retVal = new Tuple<DateTime, T>(lDateTimeStamps[idxL], dataSeria[idxL]);
            }
            return retVal;
        }

    }







    public class FixedTimeQueue<T>
    {
        private FixedQueue<T> dataSeria; // = new List<T>();
        private FixedQueue<DateTime> lDateTimeStamps; // = new List<DateTime>();


        public int Count
        {
            get { return dataSeria.Count; }
        }


        public TimeSpan TotalSeriaDuration
        {
            get { return (lDateTimeStamps.Last() - lDateTimeStamps.First()); }
        }




        public FixedTimeQueue(IEnumerable<T> dataSeriaList, IEnumerable<DateTime> dateTimeStamps)
        {
            dataSeria = new FixedQueue<T>(dataSeriaList);
            lDateTimeStamps = new FixedQueue<DateTime>(dateTimeStamps);
        }




        public FixedTimeQueue(int capacity)
        {
            dataSeria = new FixedQueue<T>(capacity);
            lDateTimeStamps = new FixedQueue<DateTime>(capacity);
        }




        public void AddSubseriaData(IEnumerable<T> lDataToAdd, IEnumerable<DateTime> lDateTimeStampsToAdd)
        {
            if (lDataToAdd.Count() != lDateTimeStampsToAdd.Count())
            {
                return;
            }

            dataSeria.Enqueue(lDataToAdd);
            lDateTimeStamps.Enqueue(lDateTimeStampsToAdd);
        }





        public void Enqueue(T dataToAdd, DateTime dtStampToAdd)
        {
            dataSeria.Enqueue(dataToAdd);
            lDateTimeStamps.Enqueue(dtStampToAdd);
        }



        


        public void RemoveDuplicatedTimeStamps()
        {
            List<DateTime> lDateTimeStampsLoc = lDateTimeStamps.ToList();
            List<DateTime> lDateTimeStampsFiltered = new List<DateTime>(lDateTimeStamps.ToList().Distinct<DateTime>());
            List<T> dataSeriaLoc = dataSeria.ToList();

            List<T> dataSeriaFiltered = new List<T>();
            int idx = 0;
            foreach (DateTime dtStamp in lDateTimeStampsFiltered)
            {
                idx = lDateTimeStampsLoc.FindIndex(idx, datetimeValue => datetimeValue == dtStamp);
                dataSeriaFiltered.Add(dataSeriaLoc[idx]);
            }

            lDateTimeStamps = new FixedQueue<DateTime>(lDateTimeStamps.Count);
            lDateTimeStamps.Enqueue(lDateTimeStampsFiltered);
            dataSeria = new FixedQueue<T>(dataSeria.Count);
            dataSeria.Enqueue(dataSeriaFiltered);
        }




        public void RemoveValues(Predicate<T> falseCondition)
        {
            List<Tuple<DateTime, T>> tuplesList =
                new List<Tuple<DateTime, T>>(Enumerable.Zip(lDateTimeStamps.ToList(), dataSeria.ToList(),
                    (dt, dat) => new Tuple<DateTime, T>(dt, dat)));

            List<Tuple<DateTime, T>> filteredTuplesList = tuplesList.FindAll(tpl => !falseCondition(tpl.Item2));


            dataSeria = new FixedQueue<T>(dataSeria.Count);
            dataSeria.Enqueue(filteredTuplesList.ConvertAll<T>(tpl => tpl.Item2));
            lDateTimeStamps = new FixedQueue<DateTime>(lDateTimeStamps.Count);
            lDateTimeStamps.Enqueue(filteredTuplesList.ConvertAll<DateTime>(tpl => tpl.Item1));
        }



        #region // obsolete
        //public FixedTimeQueue<T> InterpolateSeria(TimeSpan dt)
        //{
        //    if (typeof(T) != typeof(double))
        //    {
        //        return null;
        //    }

        //    RemoveDuplicatedTimeStamps();

        //    List<DateTime> resDateTimeList = new List<DateTime>();

        //    DateTime startTime = lDateTimeStamps.Last();
        //    List<double> currentDatetimeList =
        //        lDateTimeStamps.ToList().ConvertAll<double>(time => (time - startTime).TotalMilliseconds);
        //    //List<double> currentDataList =
        //    //    dataSeria.ConvertAll<double>(dVal => Convert.ToDouble(dVal));

        //    DateTime endTime = lDateTimeStamps.First();
        //    int dtCount = Convert.ToInt32(((double)((endTime - startTime).TotalMilliseconds) / (double)(dt.TotalMilliseconds)));
        //    long dtInternalTicks = Convert.ToInt64(((endTime - startTime).Ticks) / (double)dtCount);
        //    TimeSpan dtInternal = new TimeSpan(dtInternalTicks);

        //    DateTime t = startTime;
        //    while (true)
        //    {
        //        resDateTimeList.Add(t);
        //        t += dtInternal;
        //        if (t > endTime) { break; }
        //    }

        //    List<double> dataDoubleValues = dataSeria.ToList() as List<double>;

        //    IInterpolation method;
        //    try
        //    {
        //        method = Interpolate.Linear(currentDatetimeList, dataDoubleValues);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this;
        //    }

        //    List<double> interpolatedValues = new List<double>();
        //    foreach (DateTime time in resDateTimeList)
        //    {
        //        interpolatedValues.Add(method.Interpolate((time - startTime).TotalMilliseconds));
        //    }

        //    return new TimeSeries<T>(interpolatedValues as List<T>, resDateTimeList);

        //}
        #endregion // obsolete





        public Complex[] DataRealValuesComplexArray()
        {
            if (typeof(T) != typeof(double))
            {
                return null;
            }
            List<double> dDataList = dataSeria as List<double>;

            MathNet.Numerics.LinearAlgebra.Complex.DenseVector theVector =
                MathNet.Numerics.LinearAlgebra.Complex.DenseVector.Create(dataSeria.Count,
                    idx => new Complex(dDataList[idx], 0.0d));
            return theVector.ToArray();
        }





        public DenseVector DoubleDataValues
        {
            get
            {
                if (typeof(T) != typeof(double))
                {
                    return null;
                }
                DenseVector dvRetVector = DenseVector.OfEnumerable(dataSeria.ToList() as List<double>);
                return dvRetVector;
            }
        }




        public T[] DataValues
        {
            get { return dataSeria.ToArray(); }
        }




        public DenseVector TimeStampsValuesSeconds
        {
            get
            {
                DateTime startTime = lDateTimeStamps.Last();
                List<double> tsValuesList =
                    lDateTimeStamps.ToList().ConvertAll<double>(dt => (dt - startTime).TotalMilliseconds / 1000.0d);
                DenseVector dvRetVector = DenseVector.OfEnumerable(tsValuesList);
                return dvRetVector;
            }
        }




        public DateTime StartTime
        {
            get
            {
                DateTime startTime = lDateTimeStamps.First();
                return startTime;
            }
        }


        public DateTime EndTime
        {
            get
            {
                DateTime andTime = lDateTimeStamps.Last();
                return andTime;
            }
        }


        #region // obsolete
        //public Tuple<DateTime, T> GetMostClose(DateTime dtInFocus)
        //{
        //    Tuple<DateTime, T> retVal = new Tuple<DateTime, T>(lDateTimeStamps[0], dataSeria[0]);

        //    int idxR = lDateTimeStamps.FindIndex(dt => dtInFocus <= dt);
        //    if (idxR == 0)
        //    {
        //        return new Tuple<DateTime, T>(lDateTimeStamps[0], dataSeria[0]);
        //    }
        //    else if (idxR == -1)
        //    {
        //        return new Tuple<DateTime, T>(lDateTimeStamps[Count - 1], dataSeria[Count - 1]);
        //    }

        //    int idxL = idxR - 1;

        //    if ((lDateTimeStamps[idxR] - dtInFocus) <= (dtInFocus - lDateTimeStamps[idxL]))
        //    {
        //        retVal = new Tuple<DateTime, T>(lDateTimeStamps[idxR], dataSeria[idxR]);
        //    }
        //    else
        //    {
        //        retVal = new Tuple<DateTime, T>(lDateTimeStamps[idxL], dataSeria[idxL]);
        //    }
        //    return retVal;
        //}
        #endregion // obsolete

    }

}
