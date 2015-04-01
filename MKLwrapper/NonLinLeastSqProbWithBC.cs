using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security;


namespace MKLwrapper
{
    public sealed class NonLinLeastSqProbWithBC<SpacePointsType> : IDisposable
    {
        private IntPtr solverHandle;
        public IEnumerable<SpacePointsType> mSpaceVector = null;
        public IEnumerable<double> mFittingValuesVector = null;
        public IEnumerable<double> nXspacePoint = null;
        public Func<IEnumerable<double>, SpacePointsType, double> fittingFunction = null;
        public IEnumerable<double> lowerBoundConstraints = null;
        public IEnumerable<double> upperBoundConstraints = null;


        public string resultStatus = "";

        public NonLinLeastSqProbWithBC()
        {
            solverHandle = new IntPtr();
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


        private IEnumerable<double> ObjectiveFunctional(IEnumerable<double> xPoint)
        {
            List<SpacePointsType> lFittingSpacePointsValues = new List<SpacePointsType>(mSpaceVector);
            List<double> lFittingFunctionValues =
                lFittingSpacePointsValues.ConvertAll<double>(nSpacePointVal => fittingFunction(xPoint, nSpacePointVal));
            lFittingFunctionValues =
                new List<double>(lFittingFunctionValues.Zip(mFittingValuesVector,
                    (funcValue, fittingValue) => funcValue - fittingValue));
            return lFittingFunctionValues;
        }



        public IEnumerable<double> SolveOptimizationProblem(double precision = 1.0e-8d)
        {
            if (mSpaceVector == null)
            {
                throw new NotSetRequiredParametersException("x space values vector is null");
                return null;
            }
            if (mFittingValuesVector == null)
            {
                throw new NotSetRequiredParametersException("fitting values vector is null");
                return null;
            }
            if (nXspacePoint == null)
            {
                throw new NotSetRequiredParametersException("initial guess X vector is null");
                return null;
            }
            if (fittingFunction == null)
            {
                throw new NotSetRequiredParametersException("fitting function has not been set");
                return null;
            }
            if (lowerBoundConstraints == null)
            {
                throw new NotSetRequiredParametersException("lower bound constraints has not been set");
                return null;
            }
            if (upperBoundConstraints == null)
            {
                throw new NotSetRequiredParametersException("upper bound constraints has not been set");
                return null;
            }

            int m = mFittingValuesVector.Count();
            int n = nXspacePoint.Count();
            double[] eps = new double[6] { precision, precision, precision, precision, precision, precision };
            int iter1 = 100000;
            int iter2 = 10000;
            int RCI_Request = 0;
            double rs = 0.0d;
            bool successful = false;
            double r1 = 0.0d;
            double r2 = 0.0d;
            int[] check_info = new int[6] { 0, 0, 0, 0, 0, 0 };
            double[] x = nXspacePoint.ToArray();
            double[] fVec = ObjectiveFunctional(x).ToArray();
            double[,] fJacobi = new double[m, n];
            double[] lwBC = lowerBoundConstraints.ToArray();
            double[] upBC = upperBoundConstraints.ToArray();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    fJacobi[i, j] = 0.0d;
                }
            }

            unsafe
            {
                fixed (double* xPtr = &x[0], fVecPtr = &fVec[0], fJacPtr = &fJacobi[0, 0], lwBCptr = &lwBC[0], upBCptr = &upBC[0], epsPtr = &eps[0])
                {
                    int init_res = NonLinLeastSqProbWithBCNative.dtrnlspbc_init(ref solverHandle, ref n, ref m,
                        (IntPtr)xPtr, (IntPtr)lwBCptr, (IntPtr)upBCptr, (IntPtr)epsPtr, ref iter1, ref iter2, ref rs);
                    if (init_res != MKLwrapper.DEFINE.TR_SUCCESS)
                    {
                        DeleteAndFreeBuffers();
                        return x;
                    }


                    int check_res = NonLinLeastSqProbWithBCNative.dtrnlspbc_check(ref solverHandle, ref n, ref m,
                        (IntPtr)fJacPtr, (IntPtr)fVecPtr, (IntPtr)lwBCptr, (IntPtr)upBCptr, (IntPtr)epsPtr, check_info);

                    if (check_res != MKLwrapper.DEFINE.TR_SUCCESS)
                    {
                        DeleteAndFreeBuffers();
                        return x;
                    }
                    else
                    {
                        if (check_info.Sum() > 0)
                        {
                            resultStatus = "input parameters aren`t valid.";
                            DeleteAndFreeBuffers();
                            return nXspacePoint;
                        }
                    }



                    while (!successful)
                    {
                        int solve_res = NonLinLeastSqProbWithBCNative.dtrnlspbc_solve(ref solverHandle,
                            (IntPtr)fVecPtr, (IntPtr)fJacPtr, ref RCI_Request);
                        if (solve_res != MKLwrapper.DEFINE.TR_SUCCESS)
                        {
                            resultStatus = "error in dtrnlsp_solve";
                            DeleteAndFreeBuffers();
                            return nXspacePoint;
                        }

                        if (RCI_Request == -1 || RCI_Request == -2 || RCI_Request == -3 || RCI_Request == -4 || RCI_Request == -5 || RCI_Request == -6)
                            successful = true;

                        if (RCI_Request == 1)
                        {
                            double[] newFVecValues = ObjectiveFunctional(x).ToArray();
                            for (int i = 0; i < m; i++)
                            {
                                fVec[i] = newFVecValues[i];
                            }
                            resultStatus = "";
                        }

                        if (RCI_Request == 2)
                        {
                            JacobianMatrixCalc jCalculator = new JacobianMatrixCalc();
                            //jCalculator.mSpaceVector = (new List<double>(mSpaceVector)).ToArray();
                            //jCalculator.mFittingValuesVector = (new List<double>(mFittingValuesVector)).ToArray();
                            jCalculator.mPointsSetLength = mFittingValuesVector.Count();
                            jCalculator.nXspacePoint = (new List<double>(x)).ToArray();
                            jCalculator.objectiveFunction = xPoint => ObjectiveFunctional(xPoint).ToArray();
                            double[,] tmpNewJacobi = jCalculator.SolveJacobianMatrix(precision);

                            for (int i = 0; i < m; i++)
                            {
                                for (int j = 0; j < n; j++)
                                {
                                    fJacobi[i, j] = tmpNewJacobi[i, j];
                                }
                            }
                            resultStatus = "";
                        }
                    }
                }
            }





            int iterationsMade = 0;
            int stopCriterion = 0;
            if (NonLinLeastSqProbWithBCNative.dtrnlspbc_get(ref solverHandle, ref iterationsMade, ref stopCriterion, ref r1, ref r2) != MKLwrapper.DEFINE.TR_SUCCESS)
            {
                resultStatus = "error in dtrnlsp_get";
                DeleteAndFreeBuffers();
                return x;
            }

            DeleteAndFreeBuffers();

            // stopCriterion
            // 1               The algorithm has exceeded the maximum number of iterations
            // 2               Δ < eps(1)
            // 3               ||F(x)||_2 < eps(2)
            // 4               The Jacobian matrix is singular.
            //                 ||J(x)(1:m,j)||_2 < eps(3), j = 1, ..., n
            // 5               ||s||_2 < eps(4)
            // 6               ||F(x)||_2 - ||F(x) - J(x)s||_2 < eps(5)

            resultStatus = "stopping criterion reached: ";
            switch (stopCriterion)
            {
                case 1:
                    resultStatus += "The algorithm has exceeded the maximum number of iterations";
                    break;
                case 2:
                    resultStatus += "Δ < precision(1)";
                    break;
                case 3:
                    resultStatus += "||F(x)||_2 < precision(2)";
                    break;
                case 4:
                    resultStatus += "The Jacobian matrix is singular";
                    break;
                case 5:
                    resultStatus += "||s||_2 < precision(4)";
                    break;
                case 6:
                    resultStatus += "||F(x)||_2 - ||F(x) - J(x)s||_2 < precision(5)";
                    break;
                default:
                    break;

            }

            if (r2 < precision)
            {
                resultStatus += Environment.NewLine + "optimization passed successfully" +
                                Environment.NewLine + "precision reached: " + r2 +
                                Environment.NewLine + "precision needed : " + precision;
                return x;
            }
            else
            {
                resultStatus += Environment.NewLine + "optimization failed" +
                                Environment.NewLine + "precision reached: " + r2 +
                                Environment.NewLine + "precision needed : " + precision;
                return x;
            }
        }




        private int DeleteAndFreeBuffers()
        {
            int res = NonLinLeastSqProbWithBCNative.dtrnlspbc_delete(ref solverHandle);
            NonLinLeastSqProbWithBCNative.mkl_free_buffers();
            return res;
        }


        public void Dispose()
        {
            DeleteAndFreeBuffers();
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
                                ref IntPtr handle,
                                ref int xLength,
                                ref int fLength,
                                IntPtr xInitialGuess,
                                IntPtr xLowBounds,
                                IntPtr xUpperBounds,
                                IntPtr stopCriteria,
                                ref int iterMaxCount,
                                ref int trialIterMaxCount,
                                ref double trustRegionInitialSize);


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
                                ref int xLength,
                                ref int fLength,
                                IntPtr fJacobianMatrix,
                                IntPtr devFuncValues,
                                IntPtr xLowBounds,
                                IntPtr xUpperBounds,
                                IntPtr stopCriteria,
                                [In, Out] int[] info);




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
                                [In, Out] IntPtr devFuncValues,
                                [In] IntPtr fJacobianMatrix,
                                ref int RCI_Request);




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
                                ref int iterationsCount,
                                ref int stopCriterion,
                                ref double initialPositionResidual,
                                ref double solutionPositionResidual);




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
