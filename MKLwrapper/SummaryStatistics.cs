using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MKLwrapper
{
    public class SummaryStatistics
    {
        private IntPtr taskHandler;
        public IEnumerable<double> xVector;
        public double[,] xMatrix;


        public SummaryStatistics(double[] xVec)
        {
            taskHandler = new IntPtr();
            xVector = xVec;
        }



        public SummaryStatistics(double[,] xmatr)
        {
            taskHandler = new IntPtr();
            xMatrix = xmatr;
        }



        public SummaryStatistics()
        {
            taskHandler = new IntPtr();
        }



        #region functions wrappers

        /** VSL vsldSSNewTaskref wrapper */
        public static int vsldSSNewTask(ref IntPtr taskPtr, ref int dim, ref int num, ref int xstorage,
                                         //double[] x, double[] weights, int[] indices)
                                         IntPtr x, double[] weights, int[] indices)
        {
            return SummaryStatisticsNative.vsldSSNewTask(ref taskPtr, ref dim, ref num, ref xstorage, x, weights, indices);
        }


        /** VSL vslsSSEditTask wrapper */
        //public static int vsldSSEditQuantiles(IntPtr taskPtr, ref int quant_order_number, double[] quant_orders, double[] quantiles, double[] order_stats, ref int order_stats_storage_type)
        public static int vsldSSEditQuantiles(IntPtr taskPtr, ref int quant_order_number, IntPtr quant_orders, IntPtr quantiles, IntPtr order_stats, ref int order_stats_storage_type)
        {
            return SummaryStatisticsNative.vsldSSEditQuantiles(taskPtr, ref quant_order_number, quant_orders, quantiles,
                order_stats, ref order_stats_storage_type);
        }



        public static int vsldSSEditTask(IntPtr taskPtr, int taskNum, IntPtr results)
        {
            return SummaryStatisticsNative.vsldSSEditTask(taskPtr, taskNum, results);
        }


        public static int vsldSSEditMoments(IntPtr taskPtr, IntPtr means, IntPtr r2m, IntPtr r3m, IntPtr r4m, IntPtr c2m, IntPtr c3m, IntPtr c4m)
        {
            return SummaryStatisticsNative.vsldSSEditMoments(taskPtr, means, r2m, r3m, r4m, c2m, c3m, c4m);
        }



        public static int vsldSSCompute(IntPtr taskPtr, long taskNum, int method)
        {
            return SummaryStatisticsNative.vsldSSCompute(taskPtr, taskNum, method);
        }

        public static int vslSSDeleteTask(ref IntPtr taskPtr)
        {
            return SummaryStatisticsNative.vslSSDeleteTask(ref taskPtr);
        }

        #endregion functions wrappers



        public IEnumerable<double> CalculateStats(
                                IEnumerable<double> percOrders,
                                out double out_min,
                                out double out_max,
                                out double out_mean,
                                out double out_rms,
                                out double out_skewness,
                                out double out_kurtosis,
                                out double out_variance,
                                out double out_stdev)
        {
            double[] xArray = xVector.ToArray();
            int p = 1;
            int n = xVector.Count();

            double[] means = { 0 };
            double[] r2m = { 0 };
            double[] r3m = { 0 };
            double[] r4m = { 0 };
            double[] c2m = { 0 };
            double[] c3m = { 0 };
            double[] c4m = { 0 };
            double[] max = { 0 };
            double[] min = { 0 };
            double[] skewness = { 0 };
            double[] kurtosis = { 0 };
            int xstorage = DEFINE.VSL_SS_MATRIX_STORAGE_ROWS;

            int quant_order_number = percOrders.Count();
            double[] quant_orders = percOrders.ToList().ConvertAll(dVal => dVal/100.0d).ToArray();
            double[] quantiles = percOrders.ToList().ConvertAll(dVal => 0.0d).ToArray();
            double[] order_stats = percOrders.ToList().ConvertAll(dVal => 0.0d).ToArray();
            int order_stats_storage_type = DEFINE.VSL_SS_MATRIX_STORAGE_ROWS;
            

            unsafe
            {
                fixed (
                    double* xPtr = &xArray[0], 
                    qOrders = &quant_orders[0], 
                    qtles = &quantiles[0],
                    ordStats = &order_stats[0], 
                    meansPtr = &means[0], 
                    r2mPtr = &r2m[0], 
                    r3mPtr = &r3m[0],
                    r4mPtr = &r4m[0],
                    c2mPtr = &c2m[0], 
                    c3mPtr = &c3m[0], 
                    c4mPtr = &c4m[0],
                    maxPtr = &max[0],
                    minPtr = &min[0],
                    skwPtr = &skewness[0],
                    krtPtr = &kurtosis[0])
                {
                    int res = vsldSSNewTask(ref taskHandler, ref p, ref n, ref xstorage, (IntPtr)xPtr, null, null);

                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to initiate new MKL Summary statistics task. MKL error code: " + res);
                    }

                    #region Compute stats

                    #region configure task

                    res = vsldSSEditQuantiles(taskHandler, ref quant_order_number, (IntPtr)qOrders, (IntPtr)qtles, (IntPtr)ordStats,
                        ref order_stats_storage_type);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }



                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_2R_MOM, (IntPtr)r2mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 2-order raw moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_3R_MOM, (IntPtr)r3mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 3-order raw moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_4R_MOM, (IntPtr)r4mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 4-order raw moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_2C_MOM, (IntPtr)c2mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 2-order central moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_3C_MOM, (IntPtr)c3mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 3-order central moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_4C_MOM, (IntPtr)c4mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 4-order central moments. MKL error code: " + res);
                    }



                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_MAX, (IntPtr)maxPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }
                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_MIN, (IntPtr)minPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }

                    res = vsldSSEditTask(taskHandler, (int)DEFINE.VSL_SS_ED_MEAN, (IntPtr)meansPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, (int)DEFINE.VSL_SS_ED_SKEWNESS, (IntPtr)skwPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, (int)DEFINE.VSL_SS_ED_KURTOSIS, (IntPtr)krtPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }

                    #endregion configure task

                    res = vsldSSCompute(taskHandler,
                        DEFINE.VSL_SS_QUANTS | DEFINE.VSL_SS_MEAN | DEFINE.VSL_SS_MIN | DEFINE.VSL_SS_MAX |
                        DEFINE.VSL_SS_2R_MOM | DEFINE.VSL_SS_2C_MOM | DEFINE.VSL_SS_3R_MOM | DEFINE.VSL_SS_3C_MOM |
                        DEFINE.VSL_SS_4R_MOM | DEFINE.VSL_SS_4C_MOM | DEFINE.VSL_SS_SKEWNESS | DEFINE.VSL_SS_KURTOSIS,
                        DEFINE.VSL_SS_METHOD_FAST);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to compute MKL Summary statistics task: quantiles. MKL error code: " + res);
                    }

                    res = vslSSDeleteTask(ref taskHandler); // don't forget to delete when done

                    #endregion Compute stats
                }
            }


            out_min = min[0];
            out_max = max[0];
            out_mean = means[0];
            out_rms = Math.Sqrt(r2m[0]);
            out_variance = c2m[0];
            out_stdev = Math.Sqrt(out_variance);
            out_skewness = skewness[0];
            out_kurtosis = kurtosis[0];



            return quantiles;
        }





        public double[,] CalculateStatsMult(
                                IEnumerable<double> percOrders,
                                out double[] out_min,
                                out double[] out_max,
                                out double[] out_mean,
                                out double[] out_rms,
                                out double[] out_skewness,
                                out double[] out_kurtosis,
                                out double[] out_variance,
                                out double[] out_stdev)
        {
            double[,] xArray = xMatrix;
            int p = xMatrix.GetLength(0);
            int n = xMatrix.GetLength(1);

            double[] means = new double[p];
            double[] r2m = new double[p];
            double[] r3m = new double[p];
            double[] r4m = new double[p];
            double[] c2m = new double[p];
            double[] c3m = new double[p];
            double[] c4m = new double[p];
            double[] max = new double[p];
            double[] min = new double[p];
            double[] skewness = new double[p];
            double[] kurtosis = new double[p];
            int xstorage = DEFINE.VSL_SS_MATRIX_STORAGE_ROWS;

            int quant_order_number = percOrders.Count();
            double[] quant_orders = percOrders.ToList().ConvertAll(dVal => dVal / 100.0d).ToArray();
            double[,] quantiles = new double[p, quant_order_number]; // percOrders.ToList().ConvertAll(dVal => 0.0d).ToArray();
            double[,] order_stats = new double[p, quant_order_number];  // percOrders.ToList().ConvertAll(dVal => 0.0d).ToArray();
            int order_stats_storage_type = DEFINE.VSL_SS_MATRIX_STORAGE_ROWS;


            unsafe
            {
                fixed (
                    double* xPtr = &xArray[0,0],
                    qOrders = &quant_orders[0],
                    qtles = &quantiles[0,0],
                    ordStats = &order_stats[0,0],
                    meansPtr = &means[0],
                    r2mPtr = &r2m[0],
                    r3mPtr = &r3m[0],
                    r4mPtr = &r4m[0],
                    c2mPtr = &c2m[0],
                    c3mPtr = &c3m[0],
                    c4mPtr = &c4m[0],
                    maxPtr = &max[0],
                    minPtr = &min[0],
                    skwPtr = &skewness[0],
                    krtPtr = &kurtosis[0])
                {
                    int res = vsldSSNewTask(ref taskHandler, ref p, ref n, ref xstorage, (IntPtr)xPtr, null, null);

                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to initiate new MKL Summary statistics task. MKL error code: " + res);
                    }

                    #region Compute stats

                    #region configure task

                    res = vsldSSEditQuantiles(taskHandler, ref quant_order_number, (IntPtr)qOrders, (IntPtr)qtles, (IntPtr)ordStats,
                        ref order_stats_storage_type);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }



                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_2R_MOM, (IntPtr)r2mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 2-order raw moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_3R_MOM, (IntPtr)r3mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 3-order raw moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_4R_MOM, (IntPtr)r4mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 4-order raw moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_2C_MOM, (IntPtr)c2mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 2-order central moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_3C_MOM, (IntPtr)c3mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 3-order central moments. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_4C_MOM, (IntPtr)c4mPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task: 4-order central moments. MKL error code: " + res);
                    }



                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_MAX, (IntPtr)maxPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }
                    res = vsldSSEditTask(taskHandler, DEFINE.VSL_SS_ED_MIN, (IntPtr)minPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }

                    res = vsldSSEditTask(taskHandler, (int)DEFINE.VSL_SS_ED_MEAN, (IntPtr)meansPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, (int)DEFINE.VSL_SS_ED_SKEWNESS, (IntPtr)skwPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }


                    res = vsldSSEditTask(taskHandler, (int)DEFINE.VSL_SS_ED_KURTOSIS, (IntPtr)krtPtr);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to configure MKL Summary statistics task. MKL error code: " + res);
                    }

                    #endregion configure task

                    res = vsldSSCompute(taskHandler,
                        DEFINE.VSL_SS_QUANTS | DEFINE.VSL_SS_MEAN | DEFINE.VSL_SS_MIN | DEFINE.VSL_SS_MAX |
                        DEFINE.VSL_SS_2R_MOM | DEFINE.VSL_SS_2C_MOM | DEFINE.VSL_SS_3R_MOM | DEFINE.VSL_SS_3C_MOM |
                        DEFINE.VSL_SS_4R_MOM | DEFINE.VSL_SS_4C_MOM | DEFINE.VSL_SS_SKEWNESS | DEFINE.VSL_SS_KURTOSIS,
                        DEFINE.VSL_SS_METHOD_FAST);
                    if (res != DEFINE.VSL_STATUS_OK)
                    {
                        throw new Exception("Unable to compute MKL Summary statistics task: quantiles. MKL error code: " + res);
                    }

                    res = vslSSDeleteTask(ref taskHandler); // don't forget to delete when done

                    #endregion Compute stats
                }
            }


            out_min = min;
            out_max = max;
            out_mean = means;

            out_rms = r2m.ToList().ConvertAll(dVal => Math.Sqrt(dVal)).ToArray();
            out_variance = c2m;
            out_stdev = out_variance.ToList().ConvertAll(dVal => Math.Sqrt(dVal)).ToArray();
            out_skewness = skewness;
            out_kurtosis = kurtosis;


            return quantiles;
        }





    }
}
