using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MKLwrapper
{
    class SummaryStatistics
    {
    }







    [SuppressUnmanagedCodeSecurity]
    internal sealed class SummaryStatisticsNative
    {
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int vslsSSNewTask(
                                ref IntPtr taskHandle,
                                ref int variablesNumber,
                                ref int observationsNumber,
                                ref int xStorageType,
                                IntPtr valuesArray,
                                IntPtr weights,
                                IntPtr indices);
    }
}
