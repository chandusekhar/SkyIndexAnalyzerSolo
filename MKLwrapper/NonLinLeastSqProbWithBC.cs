using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using MathNet.Numerics.LinearAlgebra.Double;


namespace MKLwrapper
{

    public class NonLinLeastSqProbWithBC<SpacePointsType> : IDisposable
    {
        private IntPtr solverHandle;
        public IEnumerable<SpacePointsType> mEventsSpaceVector = null;
        public IEnumerable<double> mFittingValuesVector = null;
        public IEnumerable<double> nParametersSpacePoint = null;
        private int nParametersLength = 0;
        private int mEventsSetLength = 0;

        // objective function - от значений параметров, в точке пространства событий. выходное значение - double
        public Func<IEnumerable<double>, SpacePointsType, double> fittingFunction = null;

        // objective function partial derivative - от значений параметров, в точке пространства событий. выходное значение - double
        public Func<IEnumerable<double>, SpacePointsType, double[]> fittingFunctionPartDeriv = null;
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




        public IEnumerable<double> ObjectiveDeviations(IEnumerable<double> theta)
        {
            List<SpacePointsType> lEventsSpacePointsValues = new List<SpacePointsType>(mEventsSpaceVector);

            List<double> lFittingFunctionValues =
                lEventsSpacePointsValues.ConvertAll<double>(nEventsSpacePointVal => fittingFunction(theta, nEventsSpacePointVal));

            List<double> lFittingFunctionDeviations =
                new List<double>(lFittingFunctionValues.Zip(mFittingValuesVector,
                    (funcValue, fittingValue) => funcValue - fittingValue));

            return lFittingFunctionDeviations;
        }



        public IEnumerable<double> ObjectiveFunctional(IEnumerable<double> theta)
        {
            //List<SpacePointsType> lEventsSpacePointsValues = new List<SpacePointsType>(mEventsSpaceVector);

            //List<double> lFittingFunctionValues =
            //    lEventsSpacePointsValues.ConvertAll<double>(nEventsSpacePointVal => fittingFunction(theta, nEventsSpacePointVal));

            List<double> lFittingFunctionDeviations = ObjectiveDeviations(theta).ToList();
                //new List<double>(lFittingFunctionValues.Zip(mFittingValuesVector,
                //    (funcValue, fittingValue) => funcValue - fittingValue));

            List<double> lFittingFunctionDeviationsSquared = lFittingFunctionDeviations.ConvertAll(dVal => dVal*dVal);
            return lFittingFunctionDeviationsSquared;
        }




        public double[,] ObjectiveFunctionalJacobianMatrix(IEnumerable<double> theta)
        {
            List<SpacePointsType> lEventsSpacePointsValues = new List<SpacePointsType>(mEventsSpaceVector);

            List<double> lFittingFunctionValues =
                lEventsSpacePointsValues.ConvertAll<double>(nEventsSpacePointVal => fittingFunction(theta, nEventsSpacePointVal));

            List<double> lFittingFunctionDeviations =
                new List<double>(lFittingFunctionValues.Zip(mFittingValuesVector,
                    (funcValue, fittingValue) => 2.0d*(funcValue - fittingValue))); // m x 1
            //DenseVector dvFittingFunctionDeviations = DenseVector.OfEnumerable(lFittingFunctionDeviations);
            DenseMatrix dmFittingFunctionDeviations = DenseMatrix.Create(mEventsSetLength, nParametersLength, 0.0d);
            for (int i = 0; i < nParametersLength; i++)
            {
                dmFittingFunctionDeviations.SetColumn(i, lFittingFunctionDeviations.ToArray());
            }


            List<double[]> lFittingFunctionPartDerivatives =
                lEventsSpacePointsValues.ConvertAll<double[]>(
                    nEventsSpacePointVal => fittingFunctionPartDeriv(theta, nEventsSpacePointVal)); // m x double[n]
            DenseMatrix dmFittingFunctionPartDerivatives = DenseMatrix.OfRows(lFittingFunctionPartDerivatives);

            DenseMatrix dmFittingFunctionPartDerivativesMatrix =
                (DenseMatrix) dmFittingFunctionDeviations.PointwiseMultiply(dmFittingFunctionPartDerivatives);
            return dmFittingFunctionPartDerivativesMatrix.ToArray();
        }




        public IEnumerable<double> SolveOptimizationProblem(double precision = 1.0e-8d)
        {
            #region check parameters
            if (mEventsSpaceVector == null)
            {
                throw new NotSetRequiredParametersException("x space values vector is null");
                return null;
            }
            if (mFittingValuesVector == null)
            {
                throw new NotSetRequiredParametersException("fitting values vector is null");
                return null;
            }
            if (nParametersSpacePoint == null)
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
            #endregion check parameters

            int m = mFittingValuesVector.Count();
            mEventsSetLength = mEventsSpaceVector.Count();
            //nParametersLength = nParametersSpacePoint.Count();
            nParametersLength = nParametersSpacePoint.Count();
            double[] eps = new double[6] { precision, precision, precision, precision, precision, precision };
            int iter1 = 100000;
            int iter2 = 10000;
            int RCI_Request = 0;
            double rs = 0.0d;
            bool successful = false;
            double r1 = 0.0d;
            double r2 = 0.0d;
            int[] check_info = new int[6] { 0, 0, 0, 0, 0, 0 };
            double[] theta = nParametersSpacePoint.ToArray();
            double[] fVec = ObjectiveFunctional(theta).ToArray();
            double[,] fJacobi = new double[m, nParametersLength];
            double[] lwBC = lowerBoundConstraints.ToArray();
            double[] upBC = upperBoundConstraints.ToArray();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < nParametersLength; j++)
                {
                    fJacobi[i, j] = 0.0d;
                }
            }

            unsafe
            {
                fixed (double* xPtr = &theta[0], fVecPtr = &fVec[0], fJacPtr = &fJacobi[0, 0], lwBCptr = &lwBC[0], upBCptr = &upBC[0], epsPtr = &eps[0])
                {
                    int init_res = NonLinLeastSqProbWithBCNative.dtrnlspbc_init(ref solverHandle, ref nParametersLength, ref m,
                        (IntPtr)xPtr, (IntPtr)lwBCptr, (IntPtr)upBCptr, (IntPtr)epsPtr, ref iter1, ref iter2, ref rs);
                    if (init_res != MKLwrapper.DEFINE.TR_SUCCESS)
                    {
                        DeleteAndFreeBuffers();
                        return theta;
                    }


                    int check_res = NonLinLeastSqProbWithBCNative.dtrnlspbc_check(ref solverHandle, ref nParametersLength, ref m,
                        (IntPtr)fJacPtr, (IntPtr)fVecPtr, (IntPtr)lwBCptr, (IntPtr)upBCptr, (IntPtr)epsPtr, check_info);

                    if (check_res != MKLwrapper.DEFINE.TR_SUCCESS)
                    {
                        DeleteAndFreeBuffers();
                        return theta;
                    }
                    else
                    {
                        if (check_info.Sum() > 0)
                        {
                            resultStatus = "input parameters aren`t valid.";
                            DeleteAndFreeBuffers();
                            return nParametersSpacePoint;
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
                            return nParametersSpacePoint;
                        }

                        if (RCI_Request == -1 || RCI_Request == -2 || RCI_Request == -3 || RCI_Request == -4 || RCI_Request == -5 || RCI_Request == -6)
                            successful = true;

                        if (RCI_Request == 1)
                        {
                            double[] newFVecValues = ObjectiveFunctional(theta).ToArray();
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
                            jCalculator.mEventsPointsSetLength = mFittingValuesVector.Count();
                            jCalculator.nParametersSpacePoint = (new List<double>(theta)).ToArray();
                            jCalculator.objectiveFunction = t => ObjectiveFunctional(t).ToArray();
                            

                            if (fittingFunctionPartDeriv != null)
                            {
                                double[,] tmpNewJacobiAnalytic = ObjectiveFunctionalJacobianMatrix(theta);

                                #region // debugging test
                                //DenseMatrix dmJacDev = DenseMatrix.OfArray(tmpNewJacobi) -
                                //                       DenseMatrix.OfArray(tmpNewJacobiAnalytic);
                                //dmJacDev.MapInplace(dVal => Math.Abs(dVal));
                                //double debugSum = (dmJacDev.ColumnAbsoluteSums()).Sum();
                                //debugSum = Math.Sqrt(debugSum);
                                #endregion // debugging test

                                System.Array.Copy(tmpNewJacobiAnalytic, fJacobi, tmpNewJacobiAnalytic.Length);
                            }
                            else
                            {
                                double[,] tmpNewJacobi = jCalculator.SolveJacobianMatrix(precision);

                                System.Array.Copy(tmpNewJacobi, fJacobi, tmpNewJacobi.Length);
                            }

                            


                            //for (int i = 0; i < m; i++)
                            //{
                            //    for (int j = 0; j < n; j++)
                            //    {
                            //        fJacobi[i, j] = tmpNewJacobi[i, j];
                            //    }
                            //}
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
                return theta;
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
                return theta;
            }
            else
            {
                resultStatus += Environment.NewLine + "optimization failed" +
                                Environment.NewLine + "precision reached: " + r2 +
                                Environment.NewLine + "precision needed : " + precision;
                return theta;
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
    
}
