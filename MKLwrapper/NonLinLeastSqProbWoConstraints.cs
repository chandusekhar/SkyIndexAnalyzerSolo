using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Differentiation;


namespace MKLwrapper
{
    public class NotSetRequiredParametersException : Exception
    {
        public NotSetRequiredParametersException(string message)
            : base(message)
        {
        }

        public NotSetRequiredParametersException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }





    public sealed class NonLinLeastSqProbWoConstraints<SpacePointsType> : IDisposable
    {
        IntPtr solverHandle;
        public IEnumerable<SpacePointsType> mSpaceVector = null;
        public IEnumerable<double> mFittingValuesVector = null;
        public IEnumerable<double> nXspacePoint = null;
        public Func<IEnumerable<double>, SpacePointsType, double> fittingFunction = null;

        public string resultStatus = "";

        

        public NonLinLeastSqProbWoConstraints()
        {
            solverHandle = new IntPtr();
        }





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
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    fJacobi[i, j] = 0.0d;
                }
            }

            unsafe
            {
                fixed (double* xPtr = &x[0], fVecPtr = &fVec[0], fJacPtr = &fJacobi[0,0])
                {
                    int init_res = NonLinLeastSqProbWoConstraintsNative.dtrnlsp_init(ref solverHandle, ref n, ref m,
                        (IntPtr) xPtr, eps, ref iter1, ref iter2, ref rs);
                    if (init_res != MKLwrapper.DEFINE.TR_SUCCESS)
                    {
                        DeleteAndFreeBuffers();
                        return x;
                    }


                    int check_res = NonLinLeastSqProbWoConstraintsNative.dtrnlsp_check(ref solverHandle, ref n, ref m,
                        (IntPtr) fJacPtr, (IntPtr) fVecPtr, eps, check_info);

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
                        int solve_res = NonLinLeastSqProbWoConstraintsNative.dtrnlsp_solve(ref solverHandle,
                            (IntPtr) fVecPtr, (IntPtr) fJacPtr, ref RCI_Request);
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
            if (NonLinLeastSqProbWoConstraintsNative.dtrnlsp_get(ref solverHandle, ref iterationsMade, ref stopCriterion, ref r1, ref r2) != MKLwrapper.DEFINE.TR_SUCCESS)
            {
                resultStatus = "error in dtrnlsp_get";
                DeleteAndFreeBuffers();
                return x;
            }


            if (NonLinLeastSqProbWoConstraintsNative.dtrnlsp_delete(ref solverHandle) != MKLwrapper.DEFINE.TR_SUCCESS)
            {
                resultStatus = "error in dtrnlsp_delete";
                return x;
            }


            if (r2 < precision)
            {
                resultStatus = "optimization passed successfully";
                return x;
            }
            else
            {
                resultStatus = "optimization failed";
                return x;
            }
        }




        private int DeleteAndFreeBuffers()
        {
            int res = NonLinLeastSqProbWoConstraintsNative.dtrnlsp_delete(ref solverHandle);
            NonLinLeastSqProbWoConstraintsNative.mkl_free_buffers();
            return res;
        }


        public void Dispose()
        {
            DeleteAndFreeBuffers();
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
                                ref int xLength,
                                ref int fLength,
                                IntPtr xInitialGuess,
                                [In] double[] stopCriteria,
                                ref int iterMaxCount,
                                ref int trialIterMaxCount,
                                ref double trustRegionInitialSize);



        /// https://software.intel.com/node/a60c58d8-e47e-4abd-ac88-100cf89c8c54#A60C58D8-E47E-4ABD-AC88-100CF89C8C54
        /// MKL_INT dtrnlsp_check (_TRNSP_HANDLE_t* &handle, MKL_INT* &n, MKL_INT* &m, double* fjac, double* fvec, double* eps, MKL_INT* info);
        /// Checks the correctness of handle and arrays containing Jacobian matrix,
        /// objective function, and stopping criteria.
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_check(
                                ref IntPtr handle,
                                ref int xLength,
                                ref int fLength,
                                IntPtr fJacobianMatrix,
                                IntPtr devFuncValues,
                                [In] double[] stopCriteria,
                                [In, Out] int[] info);


        /// https://software.intel.com/node/ac853cec-08a0-4ea0-b496-4c182a33c213#AC853CEC-08A0-4EA0-B496-4C182A33C213
        /// MKL_INT dtrnlsp_solve (_TRNSP_HANDLE_t* &handle, double* fvec, double* fjac, MKL_INT* &RCI_Request);
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_solve(
                                ref IntPtr handle,
                                [In, Out] IntPtr devFuncValues,
                                [In] IntPtr fJacobianMatrix,
                                ref int RCI_Request);



        /// https://software.intel.com/node/6c252813-b6b9-4211-985c-d880a0ae0e5c#6C252813-B6B9-4211-985C-D880A0AE0E5C
        /// MKL_INT dtrnlsp_get (_TRNSP_HANDLE_t* &handle, MKL_INT* &iter, MKL_INT* &st_cr, double* &r1, double* &r2);
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_get(
                                ref IntPtr handle,
                                ref int iterationsCount,
                                ref int stopCriterion,
                                ref double initialPositionResidual,
                                ref double solutionPositionResidual);


        /// https://software.intel.com/ru-ru/node/471096#24BEAC28-304A-4468-AA37-EA235076763F
        /// MKL_INT dtrnlsp_delete(_TRNSP_HANDLE_t* &handle);
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern int dtrnlsp_delete(ref IntPtr handle);



        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern void mkl_free_buffers();
    }
}
