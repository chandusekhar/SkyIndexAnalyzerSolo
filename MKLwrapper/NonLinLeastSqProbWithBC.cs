using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security;


namespace MKLwrapper
{
    public sealed class NonLinLeastSqProbWithBC
    {
        private IntPtr solverHandle;

        public NonLinLeastSqProbWithBC()
        {
            //solverHandle = new IntPtr();
        }


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






        public int NonLinLeastSqProbWithBC_init(
                                            int xLength,
                                            int fLength,
                                            double[] xInitialGuess,
                                            double[] xLowBounds,
                                            double[] xUpperBounds,
                                            double[] stopCriteria,
                                            int iterMaxCount,
                                            int trialIterMaxCount,
                                            double trustRegionInitialSize)
        {
            return NonLinLeastSqProbWithBCNative.dtrnlspbc_init(out solverHandle, xLength, fLength, xInitialGuess,
                xLowBounds, xUpperBounds, stopCriteria, iterMaxCount, trialIterMaxCount, trustRegionInitialSize);
        }






        public int NonLinLeastSqProbWithBC_check(
                                            int xLength,
                                            int fLength,
                                            double[,] fJacobianMatrix,
                                            double[] devFuncValues,
                                            double[] xLowBounds,
                                            double[] xUpperBounds,
                                            double[] stopCriteria,
                                            out int[] info)
        {
            return NonLinLeastSqProbWithBCNative.dtrnlspbc_check(ref solverHandle, xLength, fLength, fJacobianMatrix,
                devFuncValues, xLowBounds, xUpperBounds, stopCriteria, out info);
        }






        public int NonLinLeastSqProbWithBC_solve(
                                            ref double[] devFuncValues,
                                            double[,] fJacobianMatrix,
                                            out int RCI_Request)
        {
            return NonLinLeastSqProbWithBCNative.dtrnlspbc_solve(ref solverHandle, ref devFuncValues, fJacobianMatrix, out RCI_Request);
        }





        public int NonLinLeastSqProbWithBC_get(
                                            ref int iterationsCount,
                                            ref int stopCriterion,
                                            ref double initialPositionResidual,
                                            ref double solutionPositionResidual)
        {
            return NonLinLeastSqProbWithBCNative.dtrnlspbc_get(ref solverHandle, out iterationsCount, out stopCriterion,
                out initialPositionResidual, out solutionPositionResidual);
        }




        public int NonLinLeastSqProbWithBC_delete()
        {
            int res = NonLinLeastSqProbWithBCNative.dtrnlspbc_delete(ref solverHandle);
            NonLinLeastSqProbWithBCNative.mkl_free_buffers();
            return res;
        }
    }





    [SuppressUnmanagedCodeSecurity]
    internal sealed class NonLinLeastSqProbWithBCNative
    {
        #region intel dtrnlspbc_init description
        // https://software.intel.com/node/867c84a5-d029-4c48-b63b-b68e04ce043d#867C84A5-D029-4C48-B63B-B68E04CE043D
        // MKL_INT dtrnlspbc_init (_TRNSPBC_HANDLE_t* &handle, MKL_INT* &n, MKL_INT* &m, double* x, double* LW, double* UP, double* eps, MKL_INT* &iter1, MKL_INT* &iter2, double* &rs);
        // initializes the solver
        // After initialization all subsequent invocations of the dtrnlspbc_solve routine should use the values of the handle returned by ?trnlspbc_init
        //
        // The eps array contains the stopping criteria
        // eps Value    Description
        // 1            Δ < eps(1)
        // 2            ||F(x)||_2 < eps(2)
        // 3            The Jacobian matrix is singular.
        //              ||J(x)(1:m,j)||_2 < eps(3), j = 1, ..., n
        // 4            ||s||_2 < eps(4)
        // 5            ||F(x)||_2 - ||F(x) - J(x)s||_2 < eps(5)
        // 6            The trial step precision.
        //              If eps(6) = 0, then the trial step meets the required precision (≤ 1.0D-10)
        //
        // NOTES:
        // J(x) is the Jacobian matrix
        // Δ is the trust-region area
        // F(x) is the value of the functional
        // s is the trial step
        //
        //
        //
        // Input Parameters
        // n        INTEGER             Length of x
        // m        INTEGER             Length of F(x)
        // x        DOUBLE PRECISION    Array of size n. Initial guess
        // LW       DOUBLE PRECISION    Array of size n. Contains low bounds for x (lw_i < x_i )
        // UP       DOUBLE PRECISION    Array of size n. Contains upper bounds for x (up_i > x_i )
        // eps      DOUBLE PRECISION    Array of size 6; contains stopping criteria. See the values above.
        // iter1    INTEGER             Specifies the maximum number of iterations
        // iter2    INTEGER             Specifies the maximum number of iterations of trial step calculation
        // rs       DOUBLE PRECISION    Definition of initial size of the trust region (boundary of the trial step).
        //                              The recommended minimum value is 0.1, and the recommended maximum value is 100.0.
        //                              Based on your knowledge of the objective function and initial guess
        //                              you can increase or decrease the initial trust region.
        //                              It can influence the iteration process, for example,
        //                              the direction of the iteration process and the number of iterations.
        //                              If you set rs to 0.0, the solver uses the default value, which is 100.0
        //
        //
        // Output Parameters
        // handle   Type _TRNSPBC_HANDLE_t
        // res      INTEGER             Informs about the task completion
        //                              res = TR_SUCCESS - the routine completed the task normally
        //                              res = TR_INVALID_OPTION - there was an error in the input parameters
        //                              res = TR_OUT_OF_MEMORY - there was a memory error
        #endregion intel dtrnlspbc_init description
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlspbc_init(
                                out IntPtr handle,
                                int xLength,
                                int fLength,
                                double[] xInitialGuess,
                                double[] xLowBounds,
                                double[] xUpperBounds,
                                double[] stopCriteria,
                                int iterMaxCount,
                                int trialIterMaxCount,
                                double trustRegionInitialSize);


        #region intel dtrnlspbc_check description
        // https://software.intel.com/node/b8baa8d2-c6c9-4849-a051-0836bc569cf7#B8BAA8D2-C6C9-4849-A051-0836BC569CF7
        // MKL_INT dtrnlspbc_check (_TRNSPBC_HANDLE_t* &handle, MKL_INT* &n, MKL_INT* &m, double* fjac, double* fvec, double* LW, double* UP, double* eps, MKL_INT* info);
        // Checks the correctness of handle and arrays containing Jacobian matrix,
        //      objective function,
        //      lower and upper bounds,
        //      and stopping criteria.
        // The dtrnlspbc_check routine checks the arrays passed into the solver as input parameters.
        //      If an array contains any INF or NaN values,
        //      the routine sets the flag in output array info
        //
        // Input Parameters
        // handle   Type _TRNSPBC_HANDLE_t
        // n        INTEGER             Length of x
        // m        INTEGER             Length of F(x)
        // fjac     DOUBLE PRECISION    Array of size m by n. Contains the Jacobian matrix of the function
        // fvec     DOUBLE PRECISION    Array of size m. Contains the function values at X, where fvec(i) = (y_i – f_i(x))
        // LW       DOUBLE PRECISION    Array of size n. Contains low bounds for x (lw_i < x_i )
        // UP       DOUBLE PRECISION    Array of size n. Contains upper bounds for x (up_i > x_i )
        // eps      DOUBLE PRECISION    Array of size 6. contains stopping criteria. See description in dtrnlspbc_init()
        //
        // Output Parameters
        // info     INTEGER             Array of size 6. Results of input parameter checking:
        //              info[0]         Flags for handle            0 - The handle is valid.
        //                                                          1 - The handle is not allocated.
        //              info[1]         Flags for fjac              0 - The fjac array is valid.
        //                                                          1 - The fjac array is not allocated
        //                                                          2 - The fjac array contains NaN.
        //                                                          3 - The fjac array contains Inf.
        //              info[2]         Flags for fvec              0 - The fvec array is valid.
        //                                                          1 - The fvec array is not allocated
        //                                                          2 - The fvec array contains NaN.
        //                                                          3 - The fvec array contains Inf.
        //              info[3]         Flags for LW                0 - The LW array is valid.
        //                                                          1 - The LW array is not allocated
        //                                                          2 - The LW array contains NaN.
        //                                                          3 - The LW array contains Inf.
        //                                                          4 - The lower bound is greater than the upper bound.
        //              info[4]         Flags for up                0 - The up array is valid.
        //                                                          1 - The up array is not allocated
        //                                                          2 - The up array contains NaN.
        //                                                          3 - The up array contains Inf.
        //                                                          4 - The upper bound is less than the lower bound.
        //              info[5]         Flags for eps               0 - The eps array is valid.
        //                                                          1 - The eps array is not allocated
        //                                                          2 - The eps array contains NaN.
        //                                                          3 - The eps array contains Inf.
        //                                                          4 - The eps array contains a value less than or equal to zero.
        // res      INTEGER             Information about completion of the task.
        //                              res = TR_SUCCESS - the routine completed the task normally.
        #endregion intel dtrnlspbc_check description
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlspbc_check(
                                ref IntPtr handle,
                                int xLength,
                                int fLength,
                                double[,] fJacobianMatrix,
                                double[] devFuncValues,
                                double[] xLowBounds,
                                double[] xUpperBounds,
                                double[] stopCriteria,
                                out int[] info);




        #region intel dtrnlspbc_solve description
        // https://software.intel.com/node/f2540c14-2044-408d-9c22-1891167f2a18#F2540C14-2044-408D-9C22-1891167F2A18
        // MKL_INT dtrnlspbc_solve (_TRNSPBC_HANDLE_t* &handle, double* fvec, double* fjac, MKL_INT* &RCI_Request);
        // Solves a nonlinear least squares problem with linear (bound) constraints using the Trust-Region algorithm.
        // The dtrnlspbc_solve routine, based on RCI, uses the Trust-Region algorithm to solve nonlinear least squares
        // problems with linear (bound) constraints. The problem is stated as follows:
        // min(x in R^n) ||F(x)||^2 _2 = min(x in R^n) ||y-f(x)||^2 _2,
        // y in R^m, x in R^n
        // f: R^n -> R^m
        // m >= n
        // with constraints:
        // lw_i ≤ x_i ≤ u_i
        // i = 1, ..., n.
        //
        // RCI_Request  INTEGER         2   Request to calculate the Jacobian matrix and put the result into fjac
        //                              1   Request to recalculate the function at vector X and put the result into fvec
        //                              0   One successful iteration step on the current trust-region radius
        //                                  (that does not mean that the value of x has changed)
        //                              -1  The algorithm has exceeded the maximum number of iterations
        //                              -2  Δ < eps(1)
        //                              -3  ||F(x)||_2 < eps(2)
        //                              -4  The Jacobian matrix is singular:
        //                                  ||J(x)(1:m,j)||_2 < eps(3), j = 1, ..., n
        //                              -5  ||s||_2 < eps(4)
        //                              -6  ||F(x)||_2 - ||F(x) - J(x)s||_2 < eps(5)
        //
        //where:
        //  J(x) is the Jacobian matrix.
        //  Δ is the trust-region area.
        //  F(x) is the value of the functional.
        //  s is the trial step.
        //
        // Input Parameters
        // handle       Type _TRNSPBC_HANDLE_t
        // fvec         DOUBLE PRECISION        Array of size m.
        //                                      Contains the function values at X,
        //                                      where fvec(i) = (y_i – f_i(x)).
        // fjac         DOUBLE PRECISION        Array of size m by n.
        //                                      Contains the Jacobian matrix of the function.
        //
        // Output Parameters
        // fvec                 DOUBLE PRECISION    Array of size m. Updated function evaluated at x.
        // RCI_Request          INTEGER             Informs about the task stage.
        // res                  INTEGER             Informs about the task completion.
        //                                          res = TR_SUCCESS means the routine completed the task normally.
        #endregion intel dtrnlspbc_solve description
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlspbc_solve(
                                ref IntPtr handle,
                                ref double[] devFuncValues,
                                double[,] fJacobianMatrix,
                                out int RCI_Request);




        #region intel dtrnlspbc_get description
        // https://software.intel.com/node/8027ec21-68b1-4c6d-82b7-48cfd2fe1acb#8027EC21-68B1-4C6D-82B7-48CFD2FE1ACB
        // MKL_INT dtrnlspbc_get (_TRNSPBC_HANDLE_t* &handle, MKL_INT* &iter, MKL_INT* &st_cr, double* &r1, double* &r2);        
        // Retrieves the number of iterations, stop criterion, initial residual, and final residual.
        // The routine retrieves the current number of iterations, the stop criterion, the initial residual, and final residual.
        // 
        // The st_cr parameter contains the stop criterion:
        // st_cr Value     Description
        // 1               The algorithm has exceeded the maximum number of iterations
        // 2               Δ < eps(1)
        // 3               ||F(x)||_2 < eps(2)
        // 4               The Jacobian matrix is singular.
        //                 ||J(x)(1:m,j)||_2 < eps(3), j = 1, ..., n
        // 5               ||s||_2 < eps(4)
        // 6               ||F(x)||_2 - ||F(x) - J(x)s||_2 < eps(5)
        // where
        // J(x) is the Jacobian matrix.
        // Δ is the trust-region area.
        // F(x) is the value of the functional.
        // s is the trial step.
        // 
        // 
        // Input Parameters
        // handle              Type _TRNSPBC_HANDLE_t
        // 
        // Output Parameters
        // iter                INTEGER             Contains the current number of iterations
        // st_cr               INTEGER             Contains the stop criterion.
        //                                         (see st_cr values description)
        // r1                  DOUBLE PRECISION    Contains the residual, (||y - f(x)||) given the initial x.
        // r2                  DOUBLE PRECISION    Contains the final residual, that is,
        //                                         the value of the function (||y - f(x)||)
        //                                         of the final x resulting from the algorithm operation.
        // res                 INTEGER             Informs about the task completion.
        //                                         res = TR_SUCCESS - the routine completed the task normally.
        #endregion intel dtrnlspbc_get description
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlspbc_get(
                                ref IntPtr handle,
                                out int iterationsCount,
                                out int stopCriterion,
                                out double initialPositionResidual,
                                out double solutionPositionResidual);




        #region intel dtrnlspbc_delete description
        // https://software.intel.com/node/0ef2101e-ce68-4a3e-9c89-fe03d6e02081#0EF2101E-CE68-4A3E-9C89-FE03D6E02081
        // MKL_INT dtrnlspbc_delete (_TRNSPBC_HANDLE_t* &handle);
        // Releases allocated data.
        // 
        // NOTE
        // This routine flags memory as not used,
        // but to actually release all memory
        // you must call the support function mkl_free_buffers.
        // 
        // Input Parameters
        // handle              Type _TRNSPBC_HANDLE_t
        // 
        // Output Parameters
        // res                 INTEGER             Informs about the task completion.
        //                                         res = TR_SUCCESS means the routine completed the task normally.
        #endregion intel dtrnlspbc_delete description
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlspbc_delete(ref IntPtr handle);



        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern void mkl_free_buffers();
    }
}
