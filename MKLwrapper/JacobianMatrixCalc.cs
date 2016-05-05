using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MKLwrapper
{
    public sealed class JacobianMatrixCalc : IDisposable
    {
        private IntPtr jacSolverHandle;
        // public IEnumerable<double> mSpaceVector = null;
        // public IEnumerable<double> mFittingValuesVector = null;
        public int mEventsPointsSetLength = 0;
        public IEnumerable<double> nParametersSpacePoint = null;
        public Func<double[], double[]> objectiveFunction = null;
        public string resultStatus = "";


        public JacobianMatrixCalc()
        {
            jacSolverHandle = new IntPtr();
        }


        //public static int djacobi(
        //    Func<int, int, double[], double[]> fcn,
        //    int xLength,
        //    int fLength,
        //    IntPtr fJacMatrixPtr,
        //    IntPtr xPtr,
        //    ref double eps)
        //{
        //    unsafe
        //    {
        //        return JacobianMatrixCalcNative.djacobi(fcn, ref xLength, ref fLength, fJacMatrixPtr,
        //            xPtr, ref eps);
        //    }
        //}




        public double[,] SolveJacobianMatrix(double prec)
        {
            // int m = mSpaceVector.Count();
            // int m = mFittingValuesVector.Count();
            int m = mEventsPointsSetLength;
            int n = nParametersSpacePoint.Count();
            double eps = prec;
            int RCI_Request = 0;
            double rs = 0.0d;
            bool successful = false;
            double[] theta = nParametersSpacePoint.ToArray();
            double[] f1Vec = objectiveFunction(theta).ToArray();
            double[] f2Vec = objectiveFunction(theta).ToArray();
            double[,] fJacobi = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    fJacobi[i, j] = 0.0d;
                }
            }

            unsafe
            {
                fixed (double* xPtr = &theta[0], f1VecPtr = &f1Vec[0], f2VecPtr = &f2Vec[0], fJacPtr = &fJacobi[0, 0])
                {
                    int init_res = JacobianMatrixCalcNative.djacobi_init(ref jacSolverHandle, ref n, ref m,
                        (IntPtr)xPtr, (IntPtr)fJacPtr, ref eps);
                    if (init_res != MKLwrapper.DEFINE.TR_SUCCESS)
                    {
                        Delete();
                        return fJacobi;
                    }


                    while (!successful)
                    {
                        int solve_res = JacobianMatrixCalcNative.djacobi_solve(ref jacSolverHandle,
                            (IntPtr)f1VecPtr, (IntPtr)f2VecPtr, ref RCI_Request);
                        if (solve_res != MKLwrapper.DEFINE.TR_SUCCESS)
                        {
                            resultStatus = "error in djacobi_solve";
                            Delete();
                            return fJacobi;
                        }

                        if (RCI_Request == 0)
                            successful = true;

                        if (RCI_Request == 1)
                        {
                            double[] newFVecValues = objectiveFunction(theta).ToArray();
                            for (int i = 0; i < m; i++)
                            {
                                f1Vec[i] = newFVecValues[i];
                            }
                            resultStatus = "";
                        }

                        if (RCI_Request == 2)
                        {
                            double[] newFVecValues = objectiveFunction(theta).ToArray();
                            for (int i = 0; i < m; i++)
                            {
                                f2Vec[i] = newFVecValues[i];
                            }
                            resultStatus = "";
                        }
                    }


                    if (JacobianMatrixCalcNative.djacobi_delete(ref jacSolverHandle) != MKLwrapper.DEFINE.TR_SUCCESS)
                    {
                        resultStatus = "error in djacobi_delete";
                        return fJacobi;
                    }
                }
            }

            return fJacobi;
        }





        private int Delete()
        {
            return JacobianMatrixCalcNative.djacobi_delete(ref jacSolverHandle);
        }


        public void Dispose()
        {
            Delete();
        }
    }






    [SuppressUnmanagedCodeSecurity]
    internal sealed class JacobianMatrixCalcNative
    {
        /*https://software.intel.com/node/fa34d805-0e52-42c0-b84a-ce001a7da06c#FA34D805-0E52-42C0-B84A-CE001A7DA06C
         * MKL_INT djacobi_init (_JACOBIMATRIX_HANDLE_t* &handle, MKL_INT* &n, MKL_INT* &m, double* x, double* fjac, double* &eps);
         * Initializes the solver for Jacobian calculations.
         */
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int djacobi_init(
                                                ref IntPtr handle,
                                                ref int xLength,
                                                ref int fLength,
                                                IntPtr xVec,
                                                IntPtr fjac,
                                                ref double eps);





        /*https://software.intel.com/node/c04eb209-94a4-44d0-8c74-e4b8ecf5ead5#C04EB209-94A4-44D0-8C74-E4B8ECF5EAD5
         * MKL_INT djacobi_solve (_JACOBIMATRIX_HANDLE_t* &handle, double* f1, double* f2, MKL_INT* &RCI_Request);
         * Computes the Jacobian matrix of the function using RCI and the central difference algorithm.
         * 
         * RCI_Request = 1 indicates that you should compute the function values at the current x point
         *      and put the results into funcValuesXplusEps.
         * RCI_Request = 2 indicates that you should compute the function values at the current x point
         *      and put the results into funcValuesXminusEps.
         */
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int djacobi_solve(
            ref IntPtr handle, 
            IntPtr funcValuesXplusEps, 
            IntPtr funcValuesXminusEps, 
            ref int RCI_Request);




        /*https://software.intel.com/node/58553046-92c4-4675-b2a2-74813030e4b3#58553046-92C4-4675-B2A2-74813030E4B3
         * MKL_INT djacobi_delete (_JACOBIMATRIX_HANDLE_t* &handle);
         */
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int djacobi_delete(ref IntPtr handle);





        //*https://software.intel.com/node/7c40c4ff-8441-4e79-b689-14e59485ace7#7C40C4FF-8441-4E79-B689-14E59485ACE7
        // * MKL_INT djacobi (USRFCND fcn, MKL_INT* &n, MKL_INT* &m, double* fjac, double* x, double* &eps);
        // * Computes the Jacobian matrix of the objective function using the central difference algorithm.
        // */
        //[DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        //internal static extern int djacobi(
        //    Func<int, int, double[], double[]> fcn,
        //    ref int xLength,
        //    ref int fLength,
        //    [In, Out] IntPtr fJacobianMatrix,
        //    IntPtr x,
        //    ref double eps);




        ///*https://software.intel.com/node/4a016232-8aaf-4c59-8e48-5c00ee48a6b4#4A016232-8AAF-4C59-8E48-5C00EE48A6B4
        // * MKL_INT djacobix (USRFCNXD fcn, MKL_INT* &n, MKL_INT * &m, double* fjac, double* x, double* &eps, void* user_data);
        // * Alternative interface for ?jacobi function for passing additional data into the objective function.
        // */
        //[DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        //internal static extern int djacobix(
        //    Func<int, int, double[], double[]> fcn,
        //    int n,
        //    int m,
        //    double[,] fjac,
        //    double[] x,
        //    double eps,
        //    object user_data);
    }
}
