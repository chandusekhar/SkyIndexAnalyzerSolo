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
using Microsoft.CSharp;




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

            theForm = new FunctionRepresentationForm("test function");
            theForm.Show();


            #region // отрисовка статистики и аппроксимации Median vs perc5 по данным файла
            //StreamReader sr = new StreamReader(CurrentDir + "2007-polarstern-CanonA640-stats.dat");
            //string dataString = sr.ReadToEnd();
            //string[] dataLines = dataString.Split('\n');
            //sr.Close();
            //sr.Dispose();
            //List<double> perc5List = new List<double>();
            //List<double> medianList = new List<double>();
            //foreach (string dataLine in dataLines)
            //{
            //    string[] lineSunbstring = dataLine.Split(',');
            //    double currentMedian = 0.0d;
            //    double currentPerc5 = 0.0d;

            //    try
            //    {
            //        currentMedian = Convert.ToDouble(lineSunbstring[1].Replace(".", ","));
            //        currentPerc5 = Convert.ToDouble(lineSunbstring[2].Replace(".", ","));
            //    }
            //    catch (Exception)
            //    {
            //        continue;
            //    }
            //    perc5List.Add(currentPerc5);
            //    medianList.Add(currentMedian);
            //}
            #endregion // отрисовка статистики и аппроксимации Median vs perc5 по данным файла

            #region аппроксимация для определения зависимости положения минимума по радиальному направлению от угла

            #region //read data
            //string xDataString = ()ServiceTools.ReadDataFromCSV(CurrentDir + dataFilePrefix + "dataMinimumPhi.csv");
            //string[] xDataSubstrings = xDataString.Split('\n');
            //List<double> xDataValuesList = new List<double>();
            //foreach (string xDataSubstring in xDataSubstrings)
            //{
            //    if (xDataSubstring != "") xDataValuesList.Add(Convert.ToDouble(xDataSubstring.Replace(".", ",")));
            //}



            //string yDataString = ServiceTools.ReadTextFromFile(CurrentDir + dataFilePrefix + "dataMinimumR.csv");
            //string[] yDataSubstrings = yDataString.Split('\n');
            //List<double> yDataValuesList = new List<double>();
            //foreach (string yDataSubstring in yDataSubstrings)
            //{
            //    if (yDataSubstring != "") yDataValuesList.Add(Convert.ToDouble(yDataSubstring.Replace(".", ",")));
            //}


            //sr = new StreamReader(CurrentDir + dataFilePrefix + "dataWeights.csv");
            //string wDataString = sr.ReadToEnd();
            //string[] wDataSubstrings = wDataString.Split('\n');
            //List<double> wDataValuesList = new List<double>();
            //foreach (string wDataSubstring in wDataSubstrings)
            //{
            //    if (wDataSubstring != "") wDataValuesList.Add(Convert.ToDouble(wDataSubstring.Replace(".", ",")));
            //}


            //sr = new StreamReader(CurrentDir + dataFilePrefix + "approximationInitialParameters.csv");
            //string pDataString = sr.ReadToEnd();
            //string[] pDataSubstrings = pDataString.Split('\n');
            //List<double> pDataValuesList = new List<double>();
            //foreach (string pDataSubstring in pDataSubstrings)
            //{
            //    if (pDataSubstring != "") pDataValuesList.Add(Convert.ToDouble(pDataSubstring.Replace(".", ",")));
            //}
            //dvCurrentParameters = DenseVector.OfEnumerable(pDataValuesList);
            #endregion //read data
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

            #region // отрисовка статистики и аппроксимации Median vs perc5 по данным файла
            //DenseVector dvDataDistribution = DenseVector.OfEnumerable(perc5List);
            //DenseVector dvDataSpace = DenseVector.OfEnumerable(medianList);
            //int polynomeOrder = 21;
            //DenseVector approxPolyKoeffs = DataAnalysis.NPolynomeApproximationLessSquareMethod(dvDataDistribution, dvDataSpace, null, polynomeOrder);

            //Func<DenseVector, double, double> theFunctionToShow = DataAnalysis.PolynomeValue;

            //theForm.theRepresentingFunctions.Add(theFunctionToShow);
            //theForm.parameters.Add(approxPolyKoeffs);
            //theForm.lineColors.Add(new Bgr(Color.Magenta));

            //theForm.dvScatterXSpace = (DenseVector)dvDataSpace.Clone();
            //theForm.dvScatterFuncValues = (DenseVector)dvDataDistribution.Clone();
            //theForm.xSpaceMin = theForm.dvScatterXSpace.Values.Min();
            //theForm.xSpaceMax = theForm.dvScatterXSpace.Values.Max();
            //theForm.overallFuncMin = theForm.dvScatterFuncValues.Values.Min();
            //theForm.overallFuncMax = theForm.dvScatterFuncValues.Values.Max();

            //ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(approxPolyKoeffs), true);
            #endregion // отрисовка статистики Median-perc5 по данным файла


            theForm.theRepresentingFunctions.Add(theFunctionToShow);
            theForm.scaleFunctionValuesToMax.Add(false);
            theForm.theRepresentingFunctions.Add(theFunctionToShow); // для той, которая будет аппроксимацией
            theForm.scaleFunctionValuesToMax.Add(false);
            theForm.parameters.Add(dvCurrentParameters);
            theForm.parameters.Add(dvCurrentParameters); // для той, которая будет аппроксимацией
            theForm.lineColors.Add(new Bgr(Color.Magenta));
            theForm.lineColors.Add(new Bgr(Color.Black)); // для той, которая будет аппроксимацией

            ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(dvCurrentParameters), true);


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


            if (theForm.xSpaceMax - theForm.xSpaceMin < 2.0d*Math.PI)
                theForm.xSpaceMax = theForm.xSpaceMin + 2.0d*Math.PI;


            theForm.Represent();

            
            string str2show = "" + theForm.theRepresentingFunctions[0].ToString() + Environment.NewLine;
            str2show += theForm.parameters[0].ToString();
            ThreadSafeOperations.SetTextTB(tbLog, str2show, true);
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
            
            //DenseVector initialParametersIncremnt = DenseVector.Create(dvParameters.Count, (i => 1.0d));
            //dvCurrentParameters = theApproximator.ApproximationGradientDescentMultidim(dvParameters, ref initialParametersIncremnt, 0.0000001d);
            //dvCurrentParameters = theApproximator.Approximation_ILOptimizer(dvParameters, ref initialParametersIncremnt, 1.0e-10d);
            dvCurrentParameters = theApproximator.Approximation_ILOptimizerConstrained(dvParameters, 1.0e-10d);
        }




        private void btnTestApprox_Click(object sender, EventArgs e)
        {
            stepsCount = 0;
            if (bgwApproximatorWorker.IsBusy)
            {
                bgwApproximatorWorker.CancelAsync();
                return;
            }

            ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(dvCurrentParameters), false);

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
                Tuple<DenseVector, double> userState = (Tuple<DenseVector, double>) e.UserState;
                dvCurrentParameters = (DenseVector)userState.Item1;
                double statsKurtosis = (double)userState.Item2;

                ServiceTools.logToTextFile(CurrentDir + dataFilePrefix + "approximationProgress.log",
                    Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + ";" + statsKurtosis.ToString("e").Replace(",", "."), true);

                theForm.theRepresentingFunctions[theForm.theRepresentingFunctions.Count - 1] = theApproximationFunction;
                theForm.parameters[theForm.parameters.Count - 1] = dvCurrentParameters;
                theForm.lineColors[theForm.lineColors.Count - 1] = new Bgr(Color.DarkOrange);
                theForm.Represent();
                
                ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(dvCurrentParameters), false);
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

            ThreadSafeOperations.SetTextTB(tbLog, "DONE" + Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters), false);
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
                (Func<DenseVector, double, double>) Polynome6ordMethod.Invoke(classInstance, new object[] {0});



            DenseMatrix dmDataRaw =
                (DenseMatrix)ServiceTools.ReadDataFromCSV(filePath);


            xData = (DenseVector)dmDataRaw.Column(0);
            funcData = (DenseVector)dmDataRaw.Column(1);
            if ((cbUseWeights.Checked) && (dmDataRaw.ColumnCount > 2))
            {
                dvFuncWeights = (DenseVector) dmDataRaw.Column(2);
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

            ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(dvCurrentParameters), true);

            theForm.dvScatterXSpace = xData.Copy();
            theForm.dvScatterFuncValues = funcData.Copy();

            theForm.Represent();


            string str2show = "" + theForm.theRepresentingFunctions[0].ToString() + Environment.NewLine;
            str2show += theForm.parameters[0].ToString();
            ThreadSafeOperations.SetTextTB(tbLog, str2show, true);
        }




        private void btnApproxEvenlop_Click(object sender, EventArgs e)
        {
            stepsCount = 0;
            if (bgwApproximatorWorker.IsBusy)
            {
                bgwApproximatorWorker.CancelAsync();
                return;
            }

            ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(dvCurrentParameters), false);


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
                (Func<DenseVector, double, double>) Polynome6ordMethod.Invoke(classInstance, new object[] {0});


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
                (Func<DenseVector, double, double>) Polynome6ordMethod.Invoke(classInstance, new object[] {1});
            Func<DenseVector, double, double> theApproximationFunction2ndDeriv =
                (Func<DenseVector, double, double>) Polynome6ordMethod.Invoke(classInstance, new object[] {2});


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
            theFform2ndDeriv.theRepresentingFunctions.Add((dv,x) => 0.0d);
            theFform2ndDeriv.scaleFunctionValuesToMax.Add(false);
            theFform2ndDeriv.parameters.Add(dvCurrentParameters);
            theFform2ndDeriv.lineColors.Add(new Bgr(Color.Black));
            theFform2ndDeriv.Show();
            theFform2ndDeriv.Represent();


            DenseVector dvCurrentParameters2 = (DenseVector) ((object[]) e.Result)[1];

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
            

            

            ThreadSafeOperations.SetTextTB(tbLog, "DONE" + Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters), false);
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

            dvCurrentParameters = theApproximator.Approximation_ILOptimizerConstrained(dvParameters);

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
    }
}
