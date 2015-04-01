using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using SkyImagesAnalyzerLibraries;
using Emgu.CV;
using Emgu.CV.Structure;
using System.CodeDom.Compiler;
using MathNet.Numerics.LinearAlgebra;
//using MathNet.Numerics.Optimization;
using Microsoft.CSharp;
using MKLwrapper;


namespace ApproximationTestingApp
{
    public partial class TestingForm : Form
    {
        public static DenseVector xData = null;
        public static DenseVector funcData = null;
        public static DenseVector dvFuncWeights = null;
        public static Func<DenseVector, double, double> theApproximationFunction;

        public static List<Func<DenseVector, double, double>> approxParametersCondition =
            new List<Func<DenseVector, double, double>>();

        public static DenseVector dvCurrentParameters;
        public static LogWindow theLogWindow;
        public static FunctionRepresentationForm theForm = null;
        private int stepsCount = 0;
        private string CurrentDir;
        private string dataFilePrefix = "";
        private Dictionary<string, object> defaultProperties = null;

        private Assembly CompiledAssembly = null;



        public TestingForm()
        {
            InitializeComponent();
        }




        private void button1_Click(object sender, EventArgs e)
        {
            dataFilePrefix = tbDataFilePrefix.Text + "_";


            string[] csvFilesInfo = Directory.GetFiles(CurrentDir, "*_dataMinimumPhi.csv");
            if (csvFilesInfo.Count() == 0)
            {
                ThreadSafeOperations.SetTextTB(tbLog,
                    Environment.NewLine + "Couldn`t find any proper csv file in directory:" + Environment.NewLine +
                    CurrentDir + Environment.NewLine, true);
                return;
            }
            else
            {
                dataFilePrefix = Path.GetFileName(csvFilesInfo[0]).Replace("_dataMinimumPhi.csv", "") + "_";
            }


            theForm = new FunctionRepresentationForm("test function");
            theForm.Show();




            #region аппроксимация для определения зависимости положения минимума по радиальному направлению от угла
            xData = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "dataMinimumPhi.csv");
            funcData = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "dataMinimumR.csv");
            dvFuncWeights = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "dataWeights.csv");

            dvCurrentParameters =
                (DenseVector)
                    ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "approximationInitialParameters.csv");



            Func<DenseVector, double, double> theFunctionToShow = (parametersListLoc, x) =>
            {
                double d1 = parametersListLoc[0];
                double d2 = parametersListLoc[1];
                double r = parametersListLoc[2];
                double phi0 = parametersListLoc[3];
                return d1 * Math.Cos(x - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(x - phi0), 2.0d));
            };


            #endregion


            theForm.theRepresentingFunctions.Add(theFunctionToShow);
            theForm.scaleFunctionValuesToMax.Add(false);
            theForm.theRepresentingFunctions.Add(theFunctionToShow); // для той, которая будет аппроксимацией
            theForm.scaleFunctionValuesToMax.Add(false);
            theForm.parameters.Add(dvCurrentParameters);
            theForm.parameters.Add(dvCurrentParameters); // для той, которая будет аппроксимацией
            theForm.lineColors.Add(new Bgr(Color.Magenta));
            theForm.lineColors.Add(new Bgr(Color.Black)); // для той, которая будет аппроксимацией

            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);


            #region //      отфильтровать значения по статстике за пределами 3s
            //
            //      отфильтровать значения по статстике за пределами 3s
            //

            //DescriptiveStatistics stats = new DescriptiveStatistics(yDataValuesList);
            //List<int> indexesToDelete = new List<int>();
            //for (int i = yDataValuesList.Count - 1; i >= 0; i--)
            //{
            //    if (Math.Abs(yDataValuesList[i] - stats.Mean) > 3.0d * stats.StandardDeviation)
            //    {
            //        xDataValuesList.RemoveAt(i);
            //        wDataValuesList.RemoveAt(i);
            //    }
            //}
            //yDataValuesList.RemoveAll(x => (Math.Abs(x - stats.Mean) > 3.0d * stats.StandardDeviation));
            #endregion //      отфильтровать значения по статстике за пределами 3s

            #region // obsolete
            // dvFuncWeights = DenseVector.OfEnumerable(wDataValuesList);

            ////FunctionRepresentationForm theWeightsForm = new FunctionRepresentationForm("data weights");
            ////theWeightsForm.Show();
            ////theWeightsForm.dvScatterXSpace = DenseVector.OfEnumerable(xDataValuesList);
            ////theWeightsForm.dvScatterFuncValues = dvFuncWeights;
            ////theWeightsForm.xSpaceMin = theWeightsForm.dvScatterXSpace.Min() - (theWeightsForm.dvScatterXSpace.Max() - theWeightsForm.dvScatterXSpace.Min()) / 5.0d;
            ////theWeightsForm.xSpaceMax = theWeightsForm.dvScatterXSpace.Max() + (theWeightsForm.dvScatterXSpace.Max() - theWeightsForm.dvScatterXSpace.Min()) / 5.0d;
            ////theWeightsForm.Represent();



            //xData = DenseVector.OfEnumerable(xDataValuesList);


            //funcData = DenseVector.OfEnumerable(yDataValuesList);
            #endregion // obsolete


            theForm.dvScatterXSpace = xData.Copy();
            theForm.dvScatterFuncValues = funcData.Copy();

            theForm.xSpaceMin = theForm.dvScatterXSpace.Min() - (theForm.dvScatterXSpace.Max() - theForm.dvScatterXSpace.Min()) / 5.0d;
            theForm.xSpaceMax = theForm.dvScatterXSpace.Max() + (theForm.dvScatterXSpace.Max() - theForm.dvScatterXSpace.Min()) / 5.0d;


            if (theForm.xSpaceMax - theForm.xSpaceMin < 2.0d * Math.PI)
                theForm.xSpaceMax = theForm.xSpaceMin + 2.0d * Math.PI;


            theForm.Represent();


            string str2show = "" + theForm.theRepresentingFunctions[0].ToString() + Environment.NewLine;
            str2show += theForm.parameters[0].ToString();
            ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + str2show + Environment.NewLine, true);
        }




        private void TestingForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }




        private void bgwApproximatorWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker SelfWorker = sender as System.ComponentModel.BackgroundWorker;
            DenseVector dvParameters = (DenseVector)((DenseVector)((DoWorkEventArgs)(e.Argument)).Argument).Clone();

            GradientDescentApproximator theApproximator = new GradientDescentApproximator(funcData, xData,
                theApproximationFunction);
            theApproximator.SelfWorker = SelfWorker;



            #region //parameters space constraints
            //theApproximator.parametersConditions.Add(new Func<DenseVector, bool>(dvCurrentParameters =>
            //{
            //    double d1 = dvCurrentParameters[0];
            //    double d2 = dvCurrentParameters[1];
            //    double r = dvCurrentParameters[2];
            //    double phi0 = dvCurrentParameters[3];
            //    //return d1 * Math.Cos(phi - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(phi - phi0), 2.0d));

            //    return ((d1 + r > 0) && (d1 + r < 287.0d));

            //    return true;
            //}));
            #endregion //parameters space constraints



            if ((cbUseWeights.Checked) && (dvFuncWeights != null))
            {
                theApproximator.DvWeights = dvFuncWeights;
            }

            if ((cbUseConstraints.Checked) && (approxParametersCondition.Count > 0))
            {
                theApproximator.parametersConditionsLessThan0 = approxParametersCondition;
            }

            //DenseVector initialParametersIncrement = DenseVector.Create(dvParameters.Count, (i => 1.0d));
            //dvCurrentParameters = theApproximator.ApproximationGradientDescentMultidim(dvParameters, ref initialParametersIncrement, 0.0000001d);
            //dvCurrentParameters = theApproximator.Approximation_ILOptimizer(dvParameters, ref initialParametersIncrement, 1.0e-10d);
            //dvCurrentParameters = theApproximator.Approximation_ILOptimizerConstrained(dvParameters, 1.0e-10d);
            DenseVector initialParametersIncrement = DenseVector.Create(dvParameters.Count, (i => 1.0d));
            dvCurrentParameters = theApproximator.ApproximationGradientDescentMultidim(dvParameters, ref initialParametersIncrement, 1.0e-10d);
        }




        private void btnTestApprox_Click(object sender, EventArgs e)
        {
            stepsCount = 0;
            if (bgwApproximatorWorker.IsBusy)
            {
                bgwApproximatorWorker.CancelAsync();
                return;
            }

            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, false);

            theApproximationFunction = (dvParametersLocal, x) =>
            {
                double d1 = dvParametersLocal[0];
                double d2 = dvParametersLocal[1];
                double r = dvParametersLocal[2];
                double phi0 = dvParametersLocal[3];
                return d1 * Math.Cos(x - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(x - phi0), 2.0d));
            };


            approxParametersCondition = new List<Func<DenseVector, double, double>>();
            approxParametersCondition.Add((dvPar, x) => -dvPar[0]);
            approxParametersCondition.Add((dvPar, x) => -dvPar[1]);
            approxParametersCondition.Add((dvPar, x) => -dvPar[2]);
            approxParametersCondition.Add((dvPar, x) => dvPar[0] - dvPar[2]);
            approxParametersCondition.Add((dvPar, x) => dvPar[1] - dvPar[2]);

            DoWorkEventArgs args = new DoWorkEventArgs(dvCurrentParameters);
            bgwApproximatorWorker.RunWorkerAsync(args);
        }




        private void bgwApproximatorWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            stepsCount++;

            if (stepsCount % 100 == 0)
            {
                Tuple<DenseVector, double> userState = (Tuple<DenseVector, double>)e.UserState;
                dvCurrentParameters = (DenseVector)userState.Item1;
                double statsKurtosis = (double)userState.Item2;

                ServiceTools.logToTextFile(CurrentDir + dataFilePrefix + "approximationProgress.log",
                    Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + ";" + statsKurtosis.ToString("e").Replace(",", "."), true);

                theForm.theRepresentingFunctions[theForm.theRepresentingFunctions.Count - 1] = theApproximationFunction;
                theForm.parameters[theForm.parameters.Count - 1] = dvCurrentParameters;
                theForm.lineColors[theForm.lineColors.Count - 1] = new Bgr(Color.DarkOrange);
                theForm.Represent();

                ThreadSafeOperations.SetTextTB(tbLog,
                    Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine,
                    false);
                lblStatus.Text = stepsCount.ToString();
            }

        }




        private void bgwApproximatorWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            theForm.theRepresentingFunctions.Add(theApproximationFunction);
            theForm.scaleFunctionValuesToMax.Add(false);
            theForm.parameters.Add(dvCurrentParameters);
            theForm.lineColors.Add(new Bgr(Color.Green));
            theForm.Represent();

            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + "DONE" + Environment.NewLine +
                ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, false);
        }







        #region // private void tb_TextChanged(object sender, EventArgs e)
        private void tb_TextChanged(object sender, EventArgs e)
        {
            //    if (theForm == null)
            //    {
            //        return;
            //    }
            //
            //    if (sender == tbXMin)
            //    {
            //        theForm.xSpaceMin = Convert.ToDouble(tbXMin.Text);
            //        theForm.Represent();
            //    }
            //
            //    if (sender == tbXMax)
            //    {
            //        theForm.xSpaceMax = Convert.ToDouble(tbXMax.Text);
            //        theForm.Represent();
            //    }
            //
            //    if (sender == tbYMin)
            //    {
            //        theForm.overallFuncMin = Convert.ToDouble(tbYMin.Text);
            //        theForm.Represent();
            //    }
            //
            //
            //    if (sender == tbYMax)
            //    {
            //        theForm.overallFuncMax = Convert.ToDouble(tbYMax.Text);
            //        theForm.Represent();
            //    }
        }
        #endregion // private void tb_TextChanged(object sender, EventArgs e)





        private void btnProperties_Click(object sender, EventArgs e)
        {
            object propertiesObj = Properties.Settings.Default;
            PropertiesEditor propForm = new PropertiesEditor(propertiesObj);
            propForm.FormClosed += new FormClosedEventHandler(PropertiesFormClosed);
            propForm.ShowDialog();
        }




        public void PropertiesFormClosed(object sender, FormClosedEventArgs e)
        {
            readDefaultProperties();
        }


        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            var settings = Properties.Settings.Default;
            foreach (SettingsProperty property in settings.Properties)
            {
                defaultProperties.Add(property.Name, settings[property.Name]);
            }

            string CurDir = Directory.GetCurrentDirectory();
            CurrentDir = (string)defaultProperties["CurrentWorkingDirectory"];
            if (CurrentDir == "")
            {
                CurrentDir = CurDir;
            }
            lblCurrWorkingDir.Text = CurrentDir;
        }


        private void TestingForm_Shown(object sender, EventArgs e)
        {
            readDefaultProperties();
        }




        private void btnInitEvenlopData_Click(object sender, EventArgs e)
        {
            dataFilePrefix = tbDataFilePrefix.Text + "_";

            theForm = new FunctionRepresentationForm("test function");
            theForm.Show();


            string filePath = CurrentDir + dataFilePrefix + "dataDistribution.csv";
            if (!File.Exists(filePath))
            {
                // попробуем найти первый попавшийся
                List<string> fileNames = new List<string>(Directory.GetFiles(CurrentDir, "*dataDistribution.csv"));
                if (fileNames.Count == 0)
                {
                    MessageBox.Show("Не найдено подходящих файлов в директории. Проверьте настройки.", ":(", MessageBoxButtons.OK);
                }
                else
                {
                    filePath = fileNames[0];
                }
            }



            string funcFilePath = CurrentDir + dataFilePrefix + "func.cs";
            if (!File.Exists(funcFilePath))
            {
                // попробуем найти первый попавшийся
                List<string> fileNames = new List<string>(Directory.GetFiles(CurrentDir, "*func.cs"));
                if (fileNames.Count == 0)
                {
                    MessageBox.Show("Не найдено подходящих файлов в директории. Проверьте настройки.", ":(", MessageBoxButtons.OK);
                }
                else
                {
                    funcFilePath = fileNames[0];
                }
            }



            if (CompiledAssembly == null)
            {
                CompilerResults results;
                CompiledAssembly = ServiceTools.CompileAssemblyFromExternalCodeSource(funcFilePath, out results);
                if (CompiledAssembly == null)
                {
                    if (results.Errors.Count != 0)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow, "Function compilation failed!");
                        foreach (CompilerError error in results.Errors)
                        {
                            theLogWindow = ServiceTools.LogAText(theLogWindow, ServiceTools.ErrorTextDescription(error));
                        }
                    }
                    return;
                }
            }
            object classInstance = CompiledAssembly.CreateInstance("SkyImagesAnalyzerLibraries.ApproximationFunctions");
            MethodInfo Polynome6ordMethod = classInstance.GetType().GetMethod("Polynome6ord");

            //List<PointD> lFixedPoints = new List<PointD>();
            //lFixedPoints.Add(new PointD(0.0d, 0.98d));
            //lFixedPoints.Add(new PointD(804.0d, 0.5d));

            Func<DenseVector, double, double> theFunctionToShow =
                (Func<DenseVector, double, double>)Polynome6ordMethod.Invoke(classInstance, new object[] { 0 });



            DenseMatrix dmDataRaw =
                (DenseMatrix)ServiceTools.ReadDataFromCSV(filePath);


            xData = (DenseVector)dmDataRaw.Column(0);
            funcData = (DenseVector)dmDataRaw.Column(1);
            if ((cbUseWeights.Checked) && (dmDataRaw.ColumnCount > 2))
            {
                dvFuncWeights = (DenseVector)dmDataRaw.Column(2);
                double dvWeights_EvenlopApprox_totalWeight = dvFuncWeights.Sum();
                dvFuncWeights /= dvWeights_EvenlopApprox_totalWeight;
            }

            int polynomeOrder = 6;
            dvCurrentParameters = DenseVector.Create(polynomeOrder, 0.0d);

            #region //obsolete
            //Func<DenseVector, double, double> theFunctionToShow =
            //        ((dvPolynomeKoeffs_except0th, dRVal) =>
            //        {
            //            List<double> prms = new List<double>(dvPolynomeKoeffs_except0th);
            //            prms.Insert(0, 0.98d);
            //            DenseVector dvPolynomeKoeffs = DenseVector.OfEnumerable(prms);
            //            return DataAnalysis.PolynomeValue(dvPolynomeKoeffs, dRVal);
            //        });
            #endregion //obsolete

            theForm.theRepresentingFunctions.Add(theFunctionToShow);
            theForm.scaleFunctionValuesToMax.Add(false);
            theForm.parameters.Add(dvCurrentParameters);
            theForm.lineColors.Add(new Bgr(Color.Magenta));

            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);

            theForm.dvScatterXSpace = xData.Copy();
            theForm.dvScatterFuncValues = funcData.Copy();

            theForm.Represent();


            string str2show = "" + theForm.theRepresentingFunctions[0].ToString() + Environment.NewLine;
            str2show += theForm.parameters[0].ToString();
            ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + str2show + Environment.NewLine, true);
        }




        private void btnApproxEvenlop_Click(object sender, EventArgs e)
        {
            stepsCount = 0;
            if (bgwApproximatorWorker.IsBusy)
            {
                bgwApproximatorWorker.CancelAsync();
                return;
            }

            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, false);


            #region //obsolete
            //theApproximationFunction = ((dvPolynomeKoeffs_except0th, dRVal) =>
            //{
            //    List<double> prms = new List<double>(dvPolynomeKoeffs_except0th);
            //    prms.Insert(0, 0.98d);
            //    DenseVector dvPolynomeKoeffs = DenseVector.OfEnumerable(prms);
            //    return DataAnalysis.PolynomeValue(dvPolynomeKoeffs, dRVal);
            //});
            #endregion //obsolete

            //List<PointD> lFixedPoints = new List<PointD>();
            //lFixedPoints.Add(new PointD(0.0d, 0.98d));
            //lFixedPoints.Add(new PointD(804.0d, 0.5d));

            object classInstance = CompiledAssembly.CreateInstance("SkyImagesAnalyzerLibraries.ApproximationFunctions");
            MethodInfo Polynome6ordMethod = classInstance.GetType().GetMethod("Polynome6ord");
            theApproximationFunction =
                (Func<DenseVector, double, double>)Polynome6ordMethod.Invoke(classInstance, new object[] { 0 });


            BackgroundWorker bgwApproximateEvenlop = new BackgroundWorker();
            bgwApproximateEvenlop.DoWork += bgwApproximateEvenlop_DoWork;
            bgwApproximateEvenlop.RunWorkerCompleted += bgwApproximateEvenlop_RunWorkerCompleted;


            DoWorkEventArgs args = new DoWorkEventArgs(dvCurrentParameters);
            bgwApproximateEvenlop.RunWorkerAsync(args);
        }




        void bgwApproximateEvenlop_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


            theForm.theRepresentingFunctions.Add(theApproximationFunction);
            theForm.scaleFunctionValuesToMax.Add(false);
            theForm.parameters.Add(dvCurrentParameters);
            theForm.lineColors.Add(new Bgr(Color.DarkOrange));
            theForm.Represent();

            //List<PointD> lFixedPoints = new List<PointD>();
            //lFixedPoints.Add(new PointD(0.0d, 0.98d));
            //lFixedPoints.Add(new PointD(804.0d, 0.5d));

            object classInstance = CompiledAssembly.CreateInstance("SkyImagesAnalyzerLibraries.ApproximationFunctions");
            MethodInfo Polynome6ordMethod = classInstance.GetType().GetMethod("Polynome6ord");
            Func<DenseVector, double, double> theApproximationFunction1stDeriv =
                (Func<DenseVector, double, double>)Polynome6ordMethod.Invoke(classInstance, new object[] { 1 });
            Func<DenseVector, double, double> theApproximationFunction2ndDeriv =
                (Func<DenseVector, double, double>)Polynome6ordMethod.Invoke(classInstance, new object[] { 2 });


            FunctionRepresentationForm theFform1stDeriv = new FunctionRepresentationForm("1st derivative");
            theFform1stDeriv.xSpaceMin = theForm.xSpaceMin;
            theFform1stDeriv.xSpaceMax = theForm.xSpaceMax;
            theFform1stDeriv.theRepresentingFunctions.Add(theApproximationFunction1stDeriv);
            theFform1stDeriv.scaleFunctionValuesToMax.Add(false);
            theFform1stDeriv.parameters.Add(dvCurrentParameters);
            theFform1stDeriv.lineColors.Add(new Bgr(Color.DarkCyan));
            theFform1stDeriv.theRepresentingFunctions.Add((dv, x) => 0.0d);
            theFform1stDeriv.scaleFunctionValuesToMax.Add(false);
            theFform1stDeriv.parameters.Add(dvCurrentParameters);
            theFform1stDeriv.lineColors.Add(new Bgr(Color.Black));
            theFform1stDeriv.Show();
            theFform1stDeriv.Represent();


            FunctionRepresentationForm theFform2ndDeriv = new FunctionRepresentationForm("2nd derivative");
            theFform2ndDeriv.xSpaceMin = theForm.xSpaceMin;
            theFform2ndDeriv.xSpaceMax = theForm.xSpaceMax;
            theFform2ndDeriv.theRepresentingFunctions.Add(theApproximationFunction2ndDeriv);
            theFform2ndDeriv.scaleFunctionValuesToMax.Add(false);
            theFform2ndDeriv.parameters.Add(dvCurrentParameters);
            theFform2ndDeriv.lineColors.Add(new Bgr(Color.YellowGreen));
            theFform2ndDeriv.theRepresentingFunctions.Add((dv, x) => 0.0d);
            theFform2ndDeriv.scaleFunctionValuesToMax.Add(false);
            theFform2ndDeriv.parameters.Add(dvCurrentParameters);
            theFform2ndDeriv.lineColors.Add(new Bgr(Color.Black));
            theFform2ndDeriv.Show();
            theFform2ndDeriv.Represent();


            DenseVector dvCurrentParameters2 = (DenseVector)((object[])e.Result)[1];

            FunctionRepresentationForm theFformLinSolv = new FunctionRepresentationForm("linear LSM solution constrained");
            theFformLinSolv.dvScatterXSpace = xData.Copy();
            theFformLinSolv.dvScatterFuncValues = funcData.Copy();
            theFformLinSolv.xSpaceMin = theForm.xSpaceMin;
            theFformLinSolv.xSpaceMax = theForm.xSpaceMax;
            theFformLinSolv.theRepresentingFunctions.Add(DataAnalysis.PolynomeValue);
            theFformLinSolv.scaleFunctionValuesToMax.Add(false);
            theFformLinSolv.parameters.Add(dvCurrentParameters2);
            theFformLinSolv.lineColors.Add(new Bgr(Color.YellowGreen));
            theFformLinSolv.theRepresentingFunctions.Add((dv, x) => 0.0d);
            theFformLinSolv.scaleFunctionValuesToMax.Add(false);
            theFformLinSolv.parameters.Add(dvCurrentParameters);
            theFformLinSolv.lineColors.Add(new Bgr(Color.Black));
            theFformLinSolv.Show();
            theFformLinSolv.Represent();




            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + "DONE" + Environment.NewLine +
                ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);
        }



        void bgwApproximateEvenlop_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            DenseVector dvParameters = (DenseVector)((DenseVector)((DoWorkEventArgs)(e.Argument)).Argument).Clone();

            GradientDescentApproximator theApproximator = new GradientDescentApproximator(funcData, xData,
                theApproximationFunction);
            theApproximator.SelfWorker = SelfWorker;


            if ((cbUseWeights.Checked) && (dvFuncWeights != null))
            {
                theApproximator.DvWeights = dvFuncWeights;
            }

            if ((cbUseConstraints.Checked) && (approxParametersCondition.Count > 0))
            {
                theApproximator.parametersConditionsLessThan0 = approxParametersCondition;
            }

            //dvCurrentParameters = theApproximator.Approximation_ILOptimizerConstrained(dvParameters);
            DenseVector initialParametersIncrement = DenseVector.Create(dvParameters.Count, (i => 1.0d));
            dvCurrentParameters = theApproximator.ApproximationGradientDescentMultidim(dvParameters, ref initialParametersIncrement);

            theForm.theRepresentingFunctions.Add(theApproximationFunction);
            theForm.scaleFunctionValuesToMax.Add(false);
            theForm.parameters.Add(dvCurrentParameters);
            theForm.lineColors.Add(new Bgr(Color.Green));
            theForm.Represent();

            List<PointD> lFixedPoints = new List<PointD>();
            lFixedPoints.Add(new PointD(0.0d, 0.98d));
            lFixedPoints.Add(new PointD(804.0d, 0.7d));

            DenseVector dvCurrentParameters2 = DataAnalysis.NPolynomeApproximationLessSquareMethod(
                funcData,
                xData,
                lFixedPoints,
                dvCurrentParameters.Count);



            e.Result = new object[] { dvCurrentParameters, dvCurrentParameters2 };
        }













        #region MKL optimization test

        //private BackgroundWorker bgwMKLapproximationWorker;
        private void btnInitDataForintelMKL_Click(object sender, EventArgs e)
        {
            List<string> lDataFilePrefixes = new List<string>() { rtbDataFilesPrefixMKL.Text + "_" };


            theApproximationFunction = (parametersListLoc, x) =>
            {
                double d1 = parametersListLoc[0];
                double d2 = parametersListLoc[1];
                double r = parametersListLoc[2];
                double phi0 = parametersListLoc[3];
                return d1 * Math.Cos(x - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(x - phi0), 2.0d));
            };


            if (!File.Exists(CurrentDir + lDataFilePrefixes[0] + "dataMinimumPhi.csv"))
            {
                string[] csvFilesInfo = Directory.GetFiles(CurrentDir, "*_dataMinimumPhi.csv");
                if (csvFilesInfo.Count() == 0)
                {
                    ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine +
                        "Couldn`t find any proper csv file in directory:" + Environment.NewLine + CurrentDir + Environment.NewLine, true);
                    return;
                }
                else
                {
                    lDataFilePrefixes = new List<string>(csvFilesInfo);
                    lDataFilePrefixes =
                        lDataFilePrefixes.ConvertAll(
                            str => Path.GetFileName(str).Replace("_dataMinimumPhi.csv", "") + "_");

                }
            }

            foreach (string prefix in lDataFilePrefixes)
            {
                xData = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + prefix + "dataMinimumPhi.csv");
                funcData = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + prefix + "dataMinimumR.csv");
                dvFuncWeights = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + prefix + "dataWeights.csv");
                dvCurrentParameters =
                    (DenseVector)
                        ServiceTools.ReadDataFromCSV(CurrentDir + prefix + "approximationInitialParameters.csv");

                #region Добавим интерполированные данные, чтобы решение не разносило

                double shift = xData[0];
                DenseVector dvTmpXdataShited = (DenseVector)xData.Map(d => d - shift);
                dvTmpXdataShited = DenseVector.OfEnumerable(dvTmpXdataShited.Concat(DenseVector.Create(1, 2.0d * Math.PI)));
                DenseVector dvTmpParameters = dvCurrentParameters.Copy();
                dvTmpParameters[3] = dvTmpParameters[3] - shift;
                funcData =
                    DenseVector.OfEnumerable(
                        funcData.Concat(DenseVector.Create(1, theApproximationFunction(dvTmpParameters, 2.0d * Math.PI))));
                DenseVector dvGaps = DenseVector.Create(dvTmpXdataShited.Count - 1, i => dvTmpXdataShited[i + 1] - dvTmpXdataShited[i]);
                int num = Convert.ToInt32(2.0d * Math.PI / dvGaps.AbsoluteMinimum());
                // итерационно добавляем точки посередине пропусков длиной более (2*PI/num)*20
                double maxGap = 30.0d * 2.0d * Math.PI / (double)num;
                while (dvGaps.AbsoluteMaximum() > maxGap)
                {
                    int maxGapIdx = dvGaps.AbsoluteMaximumIndex();
                    List<double> tmpXData = new List<double>(dvTmpXdataShited);
                    List<double> tmpFuncData = new List<double>(funcData);
                    double newXvalue = (dvTmpXdataShited[maxGapIdx] + dvTmpXdataShited[maxGapIdx + 1]) / 2.0d;
                    double newFuncValue = theApproximationFunction(dvTmpParameters, newXvalue);
                    tmpXData.Insert(maxGapIdx + 1, newXvalue);
                    tmpFuncData.Insert(maxGapIdx + 1, newFuncValue);
                    dvTmpXdataShited = DenseVector.OfEnumerable(tmpXData);
                    funcData = DenseVector.OfEnumerable(tmpFuncData);

                    dvGaps = DenseVector.Create(dvTmpXdataShited.Count - 1, i => dvTmpXdataShited[i + 1] - dvTmpXdataShited[i]);
                }
                xData = (DenseVector)dvTmpXdataShited.Map(d => d + shift);
                List<double> tmpXData2 = new List<double>(xData);
                tmpXData2.RemoveAt(tmpXData2.Count - 1);
                xData = DenseVector.OfEnumerable(tmpXData2);
                List<double> tmpFuncData2 = new List<double>(funcData);
                tmpFuncData2.RemoveAt(tmpFuncData2.Count - 1);
                funcData = DenseVector.OfEnumerable(tmpFuncData2);

                #endregion Добавим интерполированные данные, чтобы решение не разносило








                FunctionRepresentationForm MKLform = new FunctionRepresentationForm("test function");
                MKLform.Show();
                MKLform.theRepresentingFunctions.Add(theApproximationFunction);
                MKLform.scaleFunctionValuesToMax.Add(false);
                MKLform.theRepresentingFunctions.Add(theApproximationFunction); // для той, которая будет аппроксимацией
                MKLform.scaleFunctionValuesToMax.Add(false);
                MKLform.parameters.Add(dvCurrentParameters);
                MKLform.parameters.Add(dvCurrentParameters); // для той, которая будет аппроксимацией
                MKLform.lineColors.Add(new Bgr(Color.Magenta));
                MKLform.lineColors.Add(new Bgr(Color.Black)); // для той, которая будет аппроксимацией

                ThreadSafeOperations.SetTextTB(tbLog,
                    Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);

                MKLform.dvScatterXSpace = xData.Copy();
                MKLform.dvScatterFuncValues = funcData.Copy();

                MKLform.xSpaceMin = MKLform.dvScatterXSpace.Min() - (MKLform.dvScatterXSpace.Max() - MKLform.dvScatterXSpace.Min()) / 5.0d;
                MKLform.xSpaceMax = MKLform.dvScatterXSpace.Max() + (MKLform.dvScatterXSpace.Max() - MKLform.dvScatterXSpace.Min()) / 5.0d;


                if (MKLform.xSpaceMax - MKLform.xSpaceMin < 2.0d * Math.PI)
                    MKLform.xSpaceMax = MKLform.xSpaceMin + 2.0d * Math.PI;


                MKLform.Represent();


                string str2show = "" + MKLform.theRepresentingFunctions[0].ToString() + Environment.NewLine;
                str2show += MKLform.parameters[0].ToString();
                ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + str2show + Environment.NewLine, true);







                stepsCount = 0;
                //if (bgwMKLapproximationWorker != null)
                //{
                //    if (bgwMKLapproximationWorker.IsBusy)
                //    {
                //        bgwMKLapproximationWorker.CancelAsync();
                //        return;
                //    }
                //}


                ThreadSafeOperations.SetTextTB(tbLog,
                    Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, false);

                DoWorkEventArgs args = new DoWorkEventArgs(new object[] { dvCurrentParameters, MKLform });

                BackgroundWorker bgwMKLapproximationWorker = new BackgroundWorker();
                bgwMKLapproximationWorker.DoWork += bgwMKLapproximationWorker_DoWork;
                bgwMKLapproximationWorker.RunWorkerCompleted += bgwMKLapproximationWorker_RunWorkerCompleted;

                bgwMKLapproximationWorker.RunWorkerAsync(args);
            }


        }








        void bgwMKLapproximationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            object[] args = (object[])(((DoWorkEventArgs)(e.Argument)).Argument);
            DenseVector dvParameters = ((DenseVector)args[0]).Copy();
            FunctionRepresentationForm MKLform = args[1] as FunctionRepresentationForm;

            DenseVector dvLowerBounds = DenseVector.Create(dvParameters.Count, 0.0d);
            DenseVector dvUpperBounds = DenseVector.Create(dvParameters.Count, idx =>
            {
                if ((idx >= 0) && (idx < 3))
                {
                    return 2.0d * dvParameters[2];
                }
                else return 2.0d * Math.PI;
            });

            NonLinLeastSqProbWithBC<double> solver = new NonLinLeastSqProbWithBC<double>();
            solver.mSpaceVector = xData.Copy();
            solver.mFittingValuesVector = funcData.Copy();
            solver.nXspacePoint = dvParameters.Copy();
            solver.lowerBoundConstraints = dvLowerBounds;
            solver.upperBoundConstraints = dvUpperBounds;
            solver.fittingFunction =
                (paramsVector, xValue) => theApproximationFunction(DenseVector.OfEnumerable(paramsVector), xValue);
            dvParameters = DenseVector.OfEnumerable(solver.SolveOptimizationProblem());

            //dvCurrentParameters = theApproximator.Approximation_ILOptimizerConstrained(dvParameters, 1.0e-10d);
            e.Result = new object[] { dvParameters, MKLform };
        }



        void bgwMKLapproximationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] bgwResult = e.Result as object[];
            dvCurrentParameters = bgwResult[0] as DenseVector;

            FunctionRepresentationForm MKLform = bgwResult[1] as FunctionRepresentationForm;
            MKLform.theRepresentingFunctions.Add(theApproximationFunction);
            MKLform.scaleFunctionValuesToMax.Add(false);
            MKLform.parameters.Add(dvCurrentParameters);
            MKLform.lineColors.Add(new Bgr(Color.Magenta));
            MKLform.Represent();

            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + "DONE" + Environment.NewLine +
                ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);
        }
        #endregion MKL optimization test








        #region // MathNet optimization test
        //private BackgroundWorker bgwMathNetApproximationWorker;
        //private void btnInitDataForintelMathNet_Click(object sender, EventArgs e)
        //{
        //    dataFilePrefix = rtbDataFilesPrefixMathNet.Text + "_";

        //    theApproximationFunction = (parametersListLoc, x) =>
        //    {
        //        double d1 = parametersListLoc[0];
        //        double d2 = parametersListLoc[1];
        //        double r = parametersListLoc[2];
        //        double phi0 = parametersListLoc[3];
        //        return d1 * Math.Cos(x - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(x - phi0), 2.0d));
        //    };



        //    theForm = new FunctionRepresentationForm("test function");
        //    theForm.Show();

        //    if (!File.Exists(CurrentDir + dataFilePrefix + "dataMinimumPhi.csv"))
        //    {
        //        string[] csvFilesInfo = Directory.GetFiles(CurrentDir, "*_dataMinimumPhi.csv");
        //        if (csvFilesInfo.Count() == 0)
        //        {
        //            ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine +
        //                "Couldn`t find any proper csv file in directory:" + Environment.NewLine + CurrentDir + Environment.NewLine, true);
        //            return;
        //        }
        //        else
        //        {
        //            dataFilePrefix = Path.GetFileName(csvFilesInfo[0]).Replace("_dataMinimumPhi.csv", "") + "_";
        //        }
        //    }

        //    xData = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "dataMinimumPhi.csv");
        //    funcData = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "dataMinimumR.csv");
        //    dvFuncWeights = (DenseVector)ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "dataWeights.csv");

        //    dvCurrentParameters =
        //        (DenseVector)
        //            ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "approximationInitialParameters.csv");



        //    #region Добавим интерполированные данные, чтобы решение не разносило

        //    double shift = xData[0];
        //    DenseVector dvTmpXdataShited = (DenseVector)xData.Map(d => d - shift);
        //    dvTmpXdataShited = DenseVector.OfEnumerable(dvTmpXdataShited.Concat(DenseVector.Create(1, 2.0d * Math.PI)));
        //    DenseVector dvTmpParameters = dvCurrentParameters.Copy();
        //    dvTmpParameters[3] = dvTmpParameters[3] - shift;
        //    funcData =
        //        DenseVector.OfEnumerable(
        //            funcData.Concat(DenseVector.Create(1, theApproximationFunction(dvTmpParameters, 2.0d * Math.PI))));
        //    DenseVector dvGaps = DenseVector.Create(dvTmpXdataShited.Count - 1, i => dvTmpXdataShited[i + 1] - dvTmpXdataShited[i]);
        //    int num = Convert.ToInt32(2.0d * Math.PI / dvGaps.AbsoluteMinimum());
        //    // итерационно добавляем точки посередине пропусков длиной более (2*PI/num)*20
        //    double maxGap = 30.0d * 2.0d * Math.PI / (double)num;
        //    while (dvGaps.AbsoluteMaximum() > maxGap)
        //    {
        //        int maxGapIdx = dvGaps.AbsoluteMaximumIndex();
        //        List<double> tmpXData = new List<double>(dvTmpXdataShited);
        //        List<double> tmpFuncData = new List<double>(funcData);
        //        double newXvalue = (dvTmpXdataShited[maxGapIdx] + dvTmpXdataShited[maxGapIdx + 1]) / 2.0d;
        //        double newFuncValue = theApproximationFunction(dvTmpParameters, newXvalue);
        //        tmpXData.Insert(maxGapIdx + 1, newXvalue);
        //        tmpFuncData.Insert(maxGapIdx + 1, newFuncValue);
        //        dvTmpXdataShited = DenseVector.OfEnumerable(tmpXData);
        //        funcData = DenseVector.OfEnumerable(tmpFuncData);

        //        dvGaps = DenseVector.Create(dvTmpXdataShited.Count - 1, i => dvTmpXdataShited[i + 1] - dvTmpXdataShited[i]);
        //    }
        //    xData = (DenseVector)dvTmpXdataShited.Map(d => d + shift);
        //    List<double> tmpXData2 = new List<double>(xData);
        //    tmpXData2.RemoveAt(tmpXData2.Count - 1);
        //    xData = DenseVector.OfEnumerable(tmpXData2);
        //    List<double> tmpFuncData2 = new List<double>(funcData);
        //    tmpFuncData2.RemoveAt(tmpFuncData2.Count - 1);
        //    funcData = DenseVector.OfEnumerable(tmpFuncData2);
        //    #endregion Добавим интерполированные данные, чтобы решение не разносило



        //    theForm.theRepresentingFunctions.Add(theApproximationFunction);
        //    theForm.scaleFunctionValuesToMax.Add(false);
        //    theForm.theRepresentingFunctions.Add(theApproximationFunction); // для той, которая будет аппроксимацией
        //    theForm.scaleFunctionValuesToMax.Add(false);
        //    theForm.parameters.Add(dvCurrentParameters);
        //    theForm.parameters.Add(dvCurrentParameters); // для той, которая будет аппроксимацией
        //    theForm.lineColors.Add(new Bgr(Color.Magenta));
        //    theForm.lineColors.Add(new Bgr(Color.Black)); // для той, которая будет аппроксимацией

        //    ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);

        //    theForm.dvScatterXSpace = xData.Copy();
        //    theForm.dvScatterFuncValues = funcData.Copy();

        //    theForm.xSpaceMin = theForm.dvScatterXSpace.Min() - (theForm.dvScatterXSpace.Max() - theForm.dvScatterXSpace.Min()) / 5.0d;
        //    theForm.xSpaceMax = theForm.dvScatterXSpace.Max() + (theForm.dvScatterXSpace.Max() - theForm.dvScatterXSpace.Min()) / 5.0d;


        //    if (theForm.xSpaceMax - theForm.xSpaceMin < 2.0d * Math.PI)
        //        theForm.xSpaceMax = theForm.xSpaceMin + 2.0d * Math.PI;


        //    theForm.Represent();


        //    string str2show = "" + theForm.theRepresentingFunctions[0].ToString() + Environment.NewLine;
        //    str2show += theForm.parameters[0].ToString();
        //    ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + str2show + Environment.NewLine, true);
        //}





        //private void btnApproximateMathNet_Click(object sender, EventArgs e)
        //{
        //    stepsCount = 0;
        //    if (bgwMathNetApproximationWorker != null)
        //    {
        //        if (bgwMathNetApproximationWorker.IsBusy)
        //        {
        //            bgwMathNetApproximationWorker.CancelAsync();
        //            return;
        //        }
        //    }


        //    ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, false);

        //    DoWorkEventArgs args = new DoWorkEventArgs(new object[] { dvCurrentParameters });

        //    bgwMathNetApproximationWorker = new BackgroundWorker();
        //    bgwMathNetApproximationWorker.DoWork += bgwMathNetapproximationWorker_DoWork;
        //    bgwMathNetApproximationWorker.RunWorkerCompleted += bgwMathNetapproximationWorker_RunWorkerCompleted;

        //    bgwMathNetApproximationWorker.RunWorkerAsync(args);
        //}




        //void bgwMathNetapproximationWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker SelfWorker = sender as BackgroundWorker;
        //    object[] args = (object[])(((DoWorkEventArgs)(e.Argument)).Argument);
        //    DenseVector dvParameters = ((DenseVector)args[0]).Copy();

        //    //DenseVector dvLowerBounds = DenseVector.Create(dvParameters.Count, 0.0d);
        //    //DenseVector dvUpperBounds = DenseVector.Create(dvParameters.Count, idx =>
        //    //{
        //    //    if ((idx >= 0) && (idx < 3))
        //    //    {
        //    //        return 2.0d * dvParameters[2];
        //    //    }
        //    //    else return 2.0d * Math.PI;
        //    //});

        //    double[] parVect = dvParameters.ToArray();

        //    MpFunc func = (pVect, fVec, dVec, additionalParameters) =>
        //    {
        //        Dictionary<string, DenseVector> addPar = additionalParameters as Dictionary<string, DenseVector>;
        //        DenseVector dvXspace = addPar["xSpaceValues"];
        //        DenseVector dvYfittingValues = addPar["yFittingDataValues"];
        //        DenseVector dvFvals = (DenseVector)dvXspace.Map(dXval => theApproximationFunction(pVect, dXval));
        //        dvFvals = dvFvals - dvYfittingValues;
        //        for (int i = 0; i < dvFvals.Count; i++)
        //        {
        //            fVec[i] = dvFvals[i];
        //        }

        //        return 1;
        //    };

        //    Dictionary<string, DenseVector> dictAdditionalParameters = new Dictionary<string, DenseVector>();
        //    dictAdditionalParameters.Add("xSpaceValues", xData);
        //    dictAdditionalParameters.Add("yFittingDataValues", funcData);
        //    MpResult MpRes = new MpResult(dvParameters.Count);
        //    MpConfig config = new MpConfig();
        //    config.xtol = 1.0e-8d;
        //    config.covtol = 1.0e-8d;
        //    config.ftol = 1.0e-8d;

        //    List<ParameterConstraint> lConstraints = new List<ParameterConstraint>();
        //    /*
        //     * double d1 = parametersListLoc[0];
        //     * double d2 = parametersListLoc[1];
        //     * double r = parametersListLoc[2];
        //     * double phi0 = parametersListLoc[3];
        //     */
        //    lConstraints.Add(new ParameterConstraint()
        //    {
        //        isFixed = 0,
        //        limited = new int[] { 1, 1 },
        //        limits = new double[] { 0.0d, 2.0d * dvParameters[2] }
        //    });
        //    lConstraints.Add(new ParameterConstraint()
        //    {
        //        isFixed = 0,
        //        limited = new int[] { 1, 1 },
        //        limits = new double[] { 0.0d, 2.0d * dvParameters[2] }
        //    });
        //    lConstraints.Add(new ParameterConstraint()
        //    {
        //        isFixed = 0,
        //        limited = new int[] { 1, 1 },
        //        limits = new double[] { 0.0d, 2.0d * dvParameters[2] }
        //    });
        //    lConstraints.Add(new ParameterConstraint()
        //    {
        //        isFixed = 0,
        //        limited = new int[] { 0, 0 }
        //    });

        //    int result = MpFit.Solve(func, funcData.Count, dvParameters.Count, parVect, lConstraints.ToArray(), config, dictAdditionalParameters,
        //        ref MpRes);

        //    dvParameters = DenseVector.OfEnumerable(parVect);

        //    e.Result = new object[] { dvParameters };
        //}



        //void bgwMathNetapproximationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    object[] bgwResult = e.Result as object[];
        //    dvCurrentParameters = bgwResult[0] as DenseVector;


        //    theForm.theRepresentingFunctions.Add(theApproximationFunction);
        //    theForm.scaleFunctionValuesToMax.Add(false);
        //    theForm.parameters.Add(dvCurrentParameters);
        //    theForm.lineColors.Add(new Bgr(Color.Green));
        //    theForm.Represent();

        //    ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + "DONE" + Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);
        //}
        #endregion MathNet optimization test





        #region Weibull approximation
        private void btnReadWeibullData_Click(object sender, EventArgs e)
        {
            string dataFileName = rtbDataFilePath.Text;
            if (!File.Exists(dataFileName))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "coudn`t find data file: " + dataFileName);
                return;
            }

            theApproximationFunction = (parametersListLoc, x) =>
            {
                double a = parametersListLoc[0];
                double b = parametersListLoc[1];
                double g = parametersListLoc[2];
                double arg1 = (x - g) / b;
                //double arg1 = x / b;
                if (x > g)
                {
                    return (a / b) * Math.Pow(arg1, (a - 1.0d)) * Math.Exp(-Math.Pow(arg1, a));
                }
                //if (x > 0.0d)
                //{
                //    return (a / b) * Math.Pow(arg1, (a - 1.0d)) * Math.Exp(- Math.Pow(arg1, a));
                //}
                else return 0.0d;
            };


            funcData = (DenseVector)ServiceTools.ReadDataFromCSV(dataFileName);
            xData = (DenseVector)ServiceTools.ReadDataFromCSV(dataFileName + ".xdata");
            dvCurrentParameters = DenseVector.OfEnumerable(new double[] {1.5d, 1.5d, 1.5d});


            FunctionRepresentationForm MKLform = new FunctionRepresentationForm("test function");
            MKLform.Show();
            MKLform.theRepresentingFunctions.Add(theApproximationFunction);
            MKLform.scaleFunctionValuesToMax.Add(false);
            MKLform.parameters.Add(dvCurrentParameters);
            MKLform.lineColors.Add(new Bgr(Color.Black));
            
            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);

            MKLform.dvScatterXSpace = xData.Copy();
            MKLform.dvScatterFuncValues = funcData.Copy();

            MKLform.xSpaceMin = xData.Min();
            MKLform.xSpaceMax = MKLform.dvScatterXSpace.Max() + (MKLform.dvScatterXSpace.Max() - MKLform.dvScatterXSpace.Min()) / 5.0d;


            if (MKLform.xSpaceMax - MKLform.xSpaceMin < 2.0d * Math.PI)
                MKLform.xSpaceMax = MKLform.xSpaceMin + 2.0d * Math.PI;


            MKLform.Represent();


            string str2show = "" + MKLform.theRepresentingFunctions[0].ToString() + Environment.NewLine;
            str2show += MKLform.parameters[0].ToString();
            ThreadSafeOperations.SetTextTB(tbLog, Environment.NewLine + str2show + Environment.NewLine, true);


            DenseVector dvLowerBounds = DenseVector.Create(dvCurrentParameters.Count, 0.0d);
            DenseVector dvUpperBounds = DenseVector.Create(dvCurrentParameters.Count, idx => double.MaxValue);


            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, false);

            DoWorkEventArgs args = new DoWorkEventArgs(new object[] { dvCurrentParameters, MKLform, dvLowerBounds, dvUpperBounds });

            BackgroundWorker bgwMKLapproximationWorker = new BackgroundWorker();
            bgwMKLapproximationWorker.DoWork += bgwWeibullapproximationWorker_DoWork;
            bgwMKLapproximationWorker.RunWorkerCompleted += bgwWeibullapproximationWorker_RunWorkerCompleted;

            bgwMKLapproximationWorker.RunWorkerAsync(args);
        }




        void bgwWeibullapproximationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker SelfWorker = sender as BackgroundWorker;
            object[] args = (object[])(((DoWorkEventArgs)(e.Argument)).Argument);
            DenseVector dvParameters = ((DenseVector)args[0]).Copy();
            FunctionRepresentationForm MKLform = args[1] as FunctionRepresentationForm;

            DenseVector dvLowerBounds = (DenseVector) args[2];
            DenseVector dvUpperBounds = (DenseVector) args[3];
            
            NonLinLeastSqProbWithBC<double> solver = new NonLinLeastSqProbWithBC<double>();
            solver.mSpaceVector = xData.Copy();
            solver.mFittingValuesVector = funcData.Copy();
            solver.nXspacePoint = dvParameters.Copy();
            solver.lowerBoundConstraints = dvLowerBounds;
            solver.upperBoundConstraints = dvUpperBounds;
            solver.fittingFunction =
                (paramsVector, xValue) => theApproximationFunction(DenseVector.OfEnumerable(paramsVector), xValue);
            dvParameters = DenseVector.OfEnumerable(solver.SolveOptimizationProblem());

            e.Result = new object[] { dvParameters, MKLform, solver.resultStatus };
        }



        void bgwWeibullapproximationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] bgwResult = e.Result as object[];
            dvCurrentParameters = bgwResult[0] as DenseVector;
            FunctionRepresentationForm MKLform = bgwResult[1] as FunctionRepresentationForm;
            string resultStatus = bgwResult[2] as string;

            theLogWindow = ServiceTools.LogAText(theLogWindow,
                "approximation result: " + Environment.NewLine + resultStatus);

            MKLform.theRepresentingFunctions.Add(theApproximationFunction);
            MKLform.scaleFunctionValuesToMax.Add(false);
            MKLform.parameters.Add(dvCurrentParameters);
            MKLform.lineColors.Add(new Bgr(Color.Magenta));
            MKLform.Represent();

            ThreadSafeOperations.SetTextTB(tbLog,
                Environment.NewLine + "DONE" + Environment.NewLine +
                ServiceTools.densevectorToString(dvCurrentParameters) + Environment.NewLine, true);
        }



        #endregion Weibull approximation


    }
}
