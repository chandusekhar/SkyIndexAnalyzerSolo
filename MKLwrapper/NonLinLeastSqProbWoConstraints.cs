using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MKLwrapper
{
    public sealed class NonLinLeastSqProbWoConstraints
    {
        private NonLinLeastSqProbWoConstraints() { }


        #region naming conventions and functions prototypes (see mkl_rci.h)
        // d(ouble)t(rusted)r(egions)n(onlinear)l(east)s(quares)p(roblem)_<action>
        //
        //
        //  _Mkl_Api(MKL_INT,dtrnlsp_init,(_TRNSP_HANDLE_t*, MKL_INT*, MKL_INT*, double*, double*, MKL_INT*, MKL_INT*, double*))
        //  _Mkl_Api(MKL_INT,dtrnlsp_check,(_TRNSP_HANDLE_t*, MKL_INT*, MKL_INT*, double*, double*, double*, MKL_INT*))
        //  _Mkl_Api(MKL_INT,dtrnlsp_solve,(_TRNSP_HANDLE_t*, double*, double*, MKL_INT*))
        //  _Mkl_Api(MKL_INT,dtrnlsp_get,(_TRNSP_HANDLE_t*, MKL_INT*, MKL_INT*, double*, double*))
        //  _Mkl_Api(MKL_INT,dtrnlsp_delete,(_TRNSP_HANDLE_t*))
        //
        // dtrnlsp_init
        // dtrnlsp_check
        // dtrnlsp_solve
        // dtrnlsp_get
        // dtrnlsp_delete
        //
        //
        //  _Mkl_Api(MKL_INT,dtrnlspbc_init,(_TRNSPBC_HANDLE_t*, MKL_INT*, MKL_INT*, double*, double*, double*, double*, MKL_INT*, MKL_INT*, double*))
        //  _Mkl_Api(MKL_INT,dtrnlspbc_check,(_TRNSPBC_HANDLE_t*, MKL_INT*, MKL_INT*, double*, double*, double*, double*, double*, MKL_INT*))
        //  _Mkl_Api(MKL_INT,dtrnlspbc_solve,(_TRNSPBC_HANDLE_t*, double*, double*, MKL_INT*))
        //  _Mkl_Api(MKL_INT,dtrnlspbc_get,(_TRNSPBC_HANDLE_t*, MKL_INT*, MKL_INT*, double*, double*))
        //  _Mkl_Api(MKL_INT,dtrnlspbc_delete,(_TRNSPBC_HANDLE_t*))
        //
        // dtrnlspbc_init
        // dtrnlspbc_check
        // dtrnlspbc_solve
        // dtrnlspbc_get
        // dtrnlspbc_delete
        //
        //
        //  _Mkl_Api(MKL_INT,djacobi_init,(_JACOBIMATRIX_HANDLE_t*, MKL_INT*, MKL_INT*, double*, double*, double*))
        //  _Mkl_Api(MKL_INT,djacobi_solve,(_JACOBIMATRIX_HANDLE_t*, double*, double*, MKL_INT*))
        //  _Mkl_Api(MKL_INT,djacobi_delete,(_JACOBIMATRIX_HANDLE_t*))
        //  _Mkl_Api(MKL_INT,djacobi,(USRFCND fcn, MKL_INT*, MKL_INT*, double*, double*, double*))
        //  _Mkl_Api(MKL_INT,djacobix,(USRFCNXD fcn, MKL_INT*, MKL_INT*, double*, double*, double*,void*))
        //
        // djacobi_init
        // djacobi_solve
        // djacobi_delete
        // djacobi
        // djacobix
        //
        //
        //
        // The TR routine names have the following structure:
        //
        //<character><name>_<action>( )
        //where
        //
        //<character> indicates the data type:
        //
        //s
        //real, single precision
        //
        //d
        //real, double precision
        //
        //<name> indicates the task type:
        //
        //trnlsp
        //nonlinear least squares problem without constraints
        //
        //trnlspbc
        //nonlinear least squares problem with boundary constraints
        //
        //jacobi
        //computation of the Jacobian matrix using central differences
        //
        //<action> indicates an action on the task:
        //
        //init
        //initializes the solver
        //
        //check
        //checks correctness of the input parameters
        //
        //solve
        //solves the problem
        //
        //get
        //retrieves the number of iterations, the stop criterion, the initial residual, and the final residual
        //
        //delete
        //releases the allocated data
        #endregion naming conventions and functions prototypes (see mkl_rci.h)



        public static int NonLinLeastSqProbWoConstraints_init(  ref IntPtr handle,
                                                                int xLength,
                                                                int fLength,
                                                                double[] xInitialGuess,
                                                                double[] stopCriteria,
                                                                int iterMaxCount,
                                                                int trialIterMaxCount,
                                                                double trustRegionInitialSize)
        {
            return NonLinLeastSqProbWoConstraintsNative.dtrnlsp_init(ref handle, xLength, fLength, xInitialGuess, stopCriteria,
                iterMaxCount, trialIterMaxCount, trustRegionInitialSize);
        }




        public static int NonLinLeastSqProbWoConstraints_check( ref IntPtr handle, 
                                                                int xLength,
                                                                int fLength,
                                                                double[,] fJacobianMatrix,
                                                                double[] devFuncValues,
                                                                double[] stopCriteria, 
                                                                out int[] info)
        {
            return NonLinLeastSqProbWoConstraintsNative.dtrnlsp_check(ref handle, xLength, fLength, fJacobianMatrix,
                devFuncValues, stopCriteria, out info);
        }





        public static int NonLinLeastSqProbWoConstraints_solve( ref IntPtr handle,
                                                                ref double[] devFuncValues,
                                                                double[,] fJacobianMatrix,
                                                                out int RCI_Request)
        {
            return NonLinLeastSqProbWoConstraintsNative.dtrnlsp_solve(ref handle, ref devFuncValues, fJacobianMatrix,
                out RCI_Request);
        }





        public static int NonLinLeastSqProbWoConstraints_get(   ref IntPtr handle,
                                                                out int iterationsCount,
                                                                out int stopCriterion,
                                                                out double initialPositionResidual,
                                                                out double solutionPositionResidual)
        {
            return NonLinLeastSqProbWoConstraintsNative.dtrnlsp_get(ref handle, out iterationsCount, out stopCriterion,
                out initialPositionResidual, out solutionPositionResidual);
        }





        public static int NonLinLeastSqProbWoConstraints_delete(ref IntPtr handle)
        {
            int res = NonLinLeastSqProbWoConstraintsNative.dtrnlsp_delete(ref handle);
            NonLinLeastSqProbWithBCNative.mkl_free_buffers();
            return res;
        }
    }




    [SuppressUnmanagedCodeSecurity]
    internal sealed class NonLinLeastSqProbWoConstraintsNative
    {
        
        /// https://software.intel.com/ru-ru/node/471088#7E356A97-ACD0-4EC3-94D8-F2333CB2B0B2
        /// MKL_INT dtrnlsp_init (_TRNSP_HANDLE_t* &handle, MKL_INT* &n, MKL_INT* &m, double* x, double* &eps, MKL_INT* &iter1, MKL_INT* &iter2, double* &rs);
        /// routine initializes the solver
        /// After initialization, all subsequent invocations of the dtrnlsp_solve routine
        /// should use the values of the handle returned by dtrnlsp_init
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_init(
                                ref IntPtr handle,
                                int xLength,
                                int fLength,
                                double[] xInitialGuess,
                                double[] stopCriteria,
                                int iterMaxCount,
                                int trialIterMaxCount,
                                double trustRegionInitialSize);



        /// https://software.intel.com/node/a60c58d8-e47e-4abd-ac88-100cf89c8c54#A60C58D8-E47E-4ABD-AC88-100CF89C8C54
        /// MKL_INT dtrnlsp_check (_TRNSP_HANDLE_t* &handle, MKL_INT* &n, MKL_INT* &m, double* fjac, double* fvec, double* eps, MKL_INT* info);
        /// Checks the correctness of handle and arrays containing Jacobian matrix,
        /// objective function, and stopping criteria.
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_check(
                                ref IntPtr handle,
                                int xLength,
                                int fLength,
                                double[,] fJacobianMatrix,
                                double[] devFuncValues,
                                double[] stopCriteria, 
                                out int[] info);


        /// https://software.intel.com/node/ac853cec-08a0-4ea0-b496-4c182a33c213#AC853CEC-08A0-4EA0-B496-4C182A33C213
        /// MKL_INT dtrnlsp_solve (_TRNSP_HANDLE_t* &handle, double* fvec, double* fjac, MKL_INT* &RCI_Request);
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_solve(
                                ref IntPtr handle,
                                ref double[] devFuncValues,
                                double[,] fJacobianMatrix, 
                                out int RCI_Request);



        /// https://software.intel.com/node/6c252813-b6b9-4211-985c-d880a0ae0e5c#6C252813-B6B9-4211-985C-D880A0AE0E5C
        /// MKL_INT dtrnlsp_get (_TRNSP_HANDLE_t* &handle, MKL_INT* &iter, MKL_INT* &st_cr, double* &r1, double* &r2);
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_get(
                                ref IntPtr handle,
                                out int iterationsCount, 
                                out int stopCriterion,
                                out double initialPositionResidual,
                                out double solutionPositionResidual);


        /// https://software.intel.com/ru-ru/node/471096#24BEAC28-304A-4468-AA37-EA235076763F
        /// MKL_INT dtrnlsp_delete(_TRNSP_HANDLE_t* &handle);
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_delete(ref IntPtr handle);



        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern void mkl_free_buffers();
    }
}
