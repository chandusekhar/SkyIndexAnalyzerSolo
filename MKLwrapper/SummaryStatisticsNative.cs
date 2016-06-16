using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MKLwrapper
{
    [SuppressUnmanagedCodeSecurity]
    internal sealed class SummaryStatisticsNative
    {
        /** VSL vsldSSNewTask native declaration */
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int vsldSSNewTask(
                                [In, Out] ref IntPtr task,
                                [In] ref int p,
                                [In] ref int n,
                                [In] ref int xstorage,
                                IntPtr x,
                                [In] double[] w,
                                [In] int[] indices);



        /** VSL vsldSSEditTask native declaration */
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int vsldSSEditTask(
                                [In]  IntPtr task,
                                [In] int parameter,
                                IntPtr par_addr);




        /** VSL vsldSSEditMoments native declaration */
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int vsldSSEditMoments(
                                [In] IntPtr task,
                                IntPtr means,
                                IntPtr r2m,
                                IntPtr r3m,
                                IntPtr r4m,
                                IntPtr c2m,
                                IntPtr c3m,
                                IntPtr c4m);



        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int vsldSSEditQuantiles(
                                [In] IntPtr task,
                                [In] ref int quant_order_number,
                                IntPtr quant_orders,
                                IntPtr quantiles,
                                IntPtr order_stats,
                                [In] ref int order_stats_storage_type);



        /** VSL vslsSSCompute native declaration */
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int vsldSSCompute(
                                [In]  IntPtr task,
                                [In] long estimates,
                                [In] int method);





        /** VSL vslSSDeleteTask native declaration */
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int vslSSDeleteTask(
                                [In] ref IntPtr task);





        
    }
}
