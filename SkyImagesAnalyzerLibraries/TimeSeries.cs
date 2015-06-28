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


        public TimeSeries(IEnumerable<T> dataSeriaList, IEnumerable<DateTime> dateTimeStamps, bool bArrangeIfNotEqualLength = false)
        {
            if (!bArrangeIfNotEqualLength && (dataSeriaList.Count() != dateTimeStamps.Count()))
            {
                throw new ArgumentException("Arguments must be of the same dimensions.");
            }

            if (bArrangeIfNotEqualLength && (dataSeriaList.Count() != dateTimeStamps.Count()))
            {
                List<T> dataSeriaListCopy = new List<T>(dataSeriaList);
                List<DateTime> dateTimeStampsCopy = new List<DateTime>(dateTimeStamps);

                if (dataSeriaListCopy.Count > dateTimeStampsCopy.Count)
                {
                    List<Tuple<T, int>> tplList = new List<Tuple<T,int>>(dataSeriaListCopy.Select((val, idx) => new Tuple<T, int>(val, idx)));
                    int deltCount = dataSeriaListCopy.Count - dateTimeStampsCopy.Count;
                    int dIdx = (dataSeriaListCopy.Count/deltCount);
                    int removed = tplList.RemoveAll(tpl => tpl.Item2%dIdx == 0);
                    if (removed < deltCount)
                    {
                        tplList.RemoveRange(0, deltCount - removed);
                    }
                    dataSeriaListCopy = tplList.ConvertAll(tpl => tpl.Item1);
                }
                else
                {
                    List<Tuple<DateTime, int>> tplList =
                        new List<Tuple<DateTime, int>>(
                            dateTimeStampsCopy.Select((val, idx) => new Tuple<DateTime, int>(val, idx)));
                    int deltCount = dateTimeStampsCopy.Count - dataSeriaListCopy.Count;
                    int dIdx = (dateTimeStampsCopy.Count / deltCount);
                    int removed = tplList.RemoveAll(tpl => tpl.Item2 % dIdx == 0);
                    if (removed < deltCount)
                    {
                        tplList.RemoveRange(0, deltCount - removed);
                    }
                    dateTimeStampsCopy = tplList.ConvertAll(tpl => tpl.Item1);
                }
                dataSeria = new List<T>(dataSeriaListCopy);
                lDateTimeStamps = new List<DateTime>(dateTimeStampsCopy);
            }
            else
            {
                dataSeria = new List<T>(dataSeriaList);
                lDateTimeStamps = new List<DateTime>(dateTimeStamps);
            }
        }






        public void AddDataRecord(T datumToAdd, DateTime DateTimeStampToAdd, bool resortImmediately = false)
        {
            lDateTimeStamps.Add(DateTimeStampToAdd);
            dataSeria.Add(datumToAdd);

            if (resortImmediately)
            {
                SortByTimeStamps();
            }
        }





        public void AddSubseriaData(TimeSeries<T> tsSubSerie, bool bArrangeIfNotEqualLength = false)
        {
            AddSubseriaData(tsSubSerie.DataValues, tsSubSerie.TimeStamps, bArrangeIfNotEqualLength);
        }





        /// <summary>
        /// Adds the subseria data and orders the resulting serie by entries timestamps.
        /// </summary>
        /// <param name="lDataToAdd">The data to add.</param>
        /// <param name="lDateTimeStampsToAdd">TimeStamps ro add.</param>
        public void AddSubseriaData(IEnumerable<T> lDataToAdd, IEnumerable<DateTime> lDateTimeStampsToAdd, bool bArrangeIfNotEqualLength = false)
        {
            if ((lDataToAdd.Count() != lDateTimeStampsToAdd.Count()) && !bArrangeIfNotEqualLength)
            {
                throw new ArgumentException("Arguments must be of the same dimensions.");
            }


            List<DateTime> lDateTimeStampsToAddCopy = new List<DateTime>(lDateTimeStampsToAdd);
            List<T> lDataToAddCopy = new List<T>(lDataToAdd);

            if (bArrangeIfNotEqualLength && (lDataToAdd.Count() != lDateTimeStampsToAdd.Count()))
            {
                if (lDataToAddCopy.Count > lDateTimeStampsToAddCopy.Count)
                {
                    List<Tuple<T, int>> tplList =
                        new List<Tuple<T, int>>(lDataToAddCopy.Select((val, idx) => new Tuple<T, int>(val, idx)));
                    int deltCount = lDataToAddCopy.Count - lDateTimeStampsToAddCopy.Count;
                    int dIdx = (lDataToAddCopy.Count / deltCount);
                    int removed = tplList.RemoveAll(tpl => tpl.Item2%dIdx == 0);
                    if (removed < deltCount)
                    {
                        tplList.RemoveRange(0, deltCount - removed);
                    }
                    lDataToAddCopy = tplList.ConvertAll(tpl => tpl.Item1);
                }
                else
                {
                    List<Tuple<DateTime, int>> tplList =
                        new List<Tuple<DateTime, int>>(
                            lDateTimeStampsToAddCopy.Select((val, idx) => new Tuple<DateTime, int>(val, idx)));
                    int deltCount = lDateTimeStampsToAddCopy.Count - lDataToAddCopy.Count;
                    int dIdx = (lDateTimeStampsToAddCopy.Count / deltCount);
                    int removed = tplList.RemoveAll(tpl => tpl.Item2%dIdx == 0);
                    if (removed < deltCount)
                    {
                        tplList.RemoveRange(0, deltCount - removed);
                    }
                    lDateTimeStampsToAddCopy = tplList.ConvertAll(tpl => tpl.Item1);
                }
            }
            

            // будем сохранять исходный индекс для сохранения исходного порядка значений с равными штампами времени
            // для этого будем запоминать третий член совокупности - исходный индекс
            List<Tuple<DateTime, T>> tplListOrig =
                new List<Tuple<DateTime, T>>(lDateTimeStamps.Zip(dataSeria, (dt, val) => new Tuple<DateTime, T>(dt, val)));
            List<Tuple<DateTime, T, int>> listToSort =
                new List<Tuple<DateTime, T, int>>(
                    tplListOrig.Select((tpl, idx) => new Tuple<DateTime, T, int>(tpl.Item1, tpl.Item2, idx)));

            List<Tuple<DateTime, T>> tplListToAdd =
                new List<Tuple<DateTime, T>>(lDateTimeStampsToAddCopy.Zip(lDataToAddCopy,
                    (dt, val) => new Tuple<DateTime, T>(dt, val)));
            List<Tuple<DateTime, T, int>> tplListToAddIdxed =
                new List<Tuple<DateTime, T, int>>(
                    tplListToAdd.Select((tpl, idx) => new Tuple<DateTime, T, int>(tpl.Item1, tpl.Item2, idx)));


            listToSort.AddRange(tplListToAddIdxed);


            listToSort.Sort((tpl1, tpl2) =>
            {
                #region comparison description
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
                #endregion
            });

            lDateTimeStamps = listToSort.ConvertAll(tpl => tpl.Item1);
            dataSeria = listToSort.ConvertAll(tpl => tpl.Item2);
        }






        /// <summary>
        /// Sorts the timeserie by timestamps values
        /// </summary>
        public void SortByTimeStamps()
        {
            List<Tuple<DateTime, T, int>> listToSort = lDateTimeStamps.Select((t, i) => new Tuple<DateTime, T, int>(t, dataSeria[i], i)).ToList();

            listToSort.Sort((tpl1, tpl2) =>
            {
                #region comparison description
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
                #endregion comparison description
            });


            lDateTimeStamps = listToSort.ConvertAll(tpl => tpl.Item1);
            dataSeria = listToSort.ConvertAll(tpl => tpl.Item2);
        }






        /// <summary>
        /// Splits the timeserie using condition on entry-to-entry timespan.
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





        public List<TimeSeries<T>> SplitByTimeSpan(TimeSpan tSpan)
        {
            List<TimeSeries<T>> retListSubseries = new List<TimeSeries<T>>();
            int startIdx = 0;
            int endIdx = 0;
            while (true)
            {
                TimeSeries<T> subSeria = SubSeria(startIdx, tSpan, out endIdx);
                retListSubseries.Add(subSeria);
                startIdx = endIdx + 1;
                if (endIdx >= lDateTimeStamps.Count - 1)
                {
                    break;
                }
            }

            return retListSubseries;
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
            for (int i = 0; i < lDateTimeStamps.Count - 1; i++)
            {
                if (lDateTimeStamps[i] == lDateTimeStamps[i + 1])
                {
                    if (i == 0)
                    {
                        lDateTimeStamps[i + 1] =
                            lDateTimeStamps[i].AddMilliseconds(((lDateTimeStamps[i + 2] - lDateTimeStamps[i]).TotalMilliseconds / 2.0d));
                    }
                    else if (i == lDateTimeStamps.Count - 2)
                    {
                        lDateTimeStamps[i] =
                            lDateTimeStamps[i - 1].AddMilliseconds(((lDateTimeStamps[i + 1] - lDateTimeStamps[i - 1]).TotalMilliseconds / 2.0d));
                    }
                    else
                    {
                        TimeSpan dt = new TimeSpan(Convert.ToInt64((lDateTimeStamps[i + 2] - lDateTimeStamps[i - 1]).Ticks / 3.0d));
                        lDateTimeStamps[i] = lDateTimeStamps[i - 1] + dt;
                        lDateTimeStamps[i + 1] = lDateTimeStamps[i] + dt;
                    }
                }
            }
        }








        #region class TimeSerieEntriesTupleComparerByTimeStamp
        class TimeSerieEntriesTupleComparerByTimeStamp : IEqualityComparer<Tuple<DateTime, T>>
        {
            // Products are equal if their names and product numbers are equal. 
            public bool Equals(Tuple<DateTime, T> x, Tuple<DateTime, T> y)
            {

                //Check whether the compared objects reference the same data. 
                if (Object.ReferenceEquals(x, y)) return true;

                //Check whether any of the compared objects is null. 
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                //Check whether the products' properties are equal. 
                return x.Item1 == y.Item1;
            }


            // If Equals() returns true for a pair of objects  
            // then GetHashCode() must return the same value for these objects. 
            public int GetHashCode(Tuple<DateTime, T> tpl)
            {
                return tpl.Item1.GetHashCode();
            }

        }
        #endregion class TimeSerieEntriesTupleComparerByTimeStamp

        public void RemoveDuplicatedTimeStamps()
        {
            //List<Tuple<DateTime, T>> listToSort = lDateTimeStamps.Select((t, i) => new Tuple<DateTime, T>(t, dataSeria[i])).ToList();
            List<Tuple<DateTime, T>> listToSort = new List<Tuple<DateTime, T>>(lDateTimeStamps.Zip(dataSeria,
                (dt, val) => new Tuple<DateTime, T>(dt, val)));
            List<Tuple<DateTime, T>> lDateTimeStampsFiltered =
                new List<Tuple<DateTime, T>>(listToSort.Distinct(new TimeSerieEntriesTupleComparerByTimeStamp()));

            lDateTimeStamps = lDateTimeStampsFiltered.ConvertAll(tpl => tpl.Item1);
            dataSeria = lDateTimeStampsFiltered.ConvertAll(tpl => tpl.Item2);
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




        public void RemoveValues(Predicate<DateTime> falseConditionOnTimeStamps)
        {
            List<Tuple<DateTime, T>> tuplesList =
                new List<Tuple<DateTime, T>>(Enumerable.Zip(lDateTimeStamps, dataSeria,
                    (dt, dat) => new Tuple<DateTime, T>(dt, dat)));

            List<Tuple<DateTime, T>> filteredTuplesList = tuplesList.FindAll(tpl => !falseConditionOnTimeStamps(tpl.Item1));


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
                interpolatedValues.Add(method.Interpolate((time - startTime).TotalMilliseconds));
            }

            return new TimeSeries<T>(interpolatedValues as List<T>, resDateTimeList);
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






        public DenseVector dvDoubleDataValues
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




        public List<T> DataValues
        {
            get
            {
                return dataSeria;
            }
        }




        public List<DateTime> TimeStamps
        {
            get
            {
                return lDateTimeStamps;
            }
        }





        public DenseVector TimeStampsValuesSeconds
        {
            get
            {
                DateTime startTime = lDateTimeStamps[0];
                List<double> tsValuesList =
                    lDateTimeStamps.ConvertAll<double>(dt => (dt - startTime).TotalMilliseconds / 1000.0d);
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
                DateTime andTime = lDateTimeStamps[lDateTimeStamps.Count - 1];
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





        public void Clear()
        {
            lDateTimeStamps.Clear();
            dataSeria.Clear();
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
