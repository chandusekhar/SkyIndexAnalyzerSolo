using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using QuickGraph;
using QuickGraph.Glee;
using SkyIndexAnalyzerLibraries;
using Emgu.CV;
using Emgu.CV.Structure;



namespace ApproximationTestingApp
{
    public partial class TestingForm : Form
    {
        public static DenseVector xData;
        public static DenseVector funcData;
        public static DenseVector dvFuncWeights;
        public static Func<DenseVector, double, double> theApproximationFunction;
        public static DenseVector dvCurrentParameters;
        public static LogWindow theLogWindow;
        public static FunctionRepresentationForm theForm = null;
        private int stepsCount = 0;
        private string CurrentDir;
        private string dataFilePrefix = "";
        private Dictionary<string, object> defaultProperties = null;



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


            #region // аппроксимация для определения зависимости положения минимума по радиальному направлению от угла
            StreamReader sr =
                new StreamReader(CurrentDir + dataFilePrefix + "dataMinimumPhi.csv");
            string xDataString = sr.ReadToEnd();
            string[] xDataSubstrings = xDataString.Split('\n');
            List<double> xDataValuesList = new List<double>();
            foreach (string xDataSubstring in xDataSubstrings)
            {
                if (xDataSubstring != "") xDataValuesList.Add(Convert.ToDouble(xDataSubstring.Replace(".", ",")));
            }



            sr = new StreamReader(CurrentDir + dataFilePrefix + "dataMinimumR.csv");
            string yDataString = sr.ReadToEnd();
            string[] yDataSubstrings = yDataString.Split('\n');
            List<double> yDataValuesList = new List<double>();
            foreach (string yDataSubstring in yDataSubstrings)
            {
                if (yDataSubstring != "") yDataValuesList.Add(Convert.ToDouble(yDataSubstring.Replace(".", ",")));
            }


            sr = new StreamReader(CurrentDir + dataFilePrefix + "dataWeights.csv");
            string wDataString = sr.ReadToEnd();
            string[] wDataSubstrings = wDataString.Split('\n');
            List<double> wDataValuesList = new List<double>();
            foreach (string wDataSubstring in wDataSubstrings)
            {
                if (wDataSubstring != "") wDataValuesList.Add(Convert.ToDouble(wDataSubstring.Replace(".", ",")));
            }


            sr = new StreamReader(CurrentDir + dataFilePrefix + "approximationInitialParameters.csv");
            string pDataString = sr.ReadToEnd();
            string[] pDataSubstrings = pDataString.Split('\n');
            List<double> pDataValuesList = new List<double>();
            foreach (string pDataSubstring in pDataSubstrings)
            {
                if (pDataSubstring != "") pDataValuesList.Add(Convert.ToDouble(pDataSubstring.Replace(".", ",")));
            }
            dvCurrentParameters = DenseVector.OfEnumerable(pDataValuesList);


            Func<DenseVector, double, double> theFunctionToShow = new Func<DenseVector, double, double>((parametersListLoc, x) =>
            {
                double d1 = parametersListLoc[0];
                double d2 = parametersListLoc[1];
                double r = parametersListLoc[2];
                double phi0 = parametersListLoc[3];
                return d1 * Math.Cos(x - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(x - phi0), 2.0d));
            });
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
            theForm.theRepresentingFunctions.Add(theFunctionToShow); // для той, которая будет аппроксимацией
            theForm.parameters.Add(dvCurrentParameters);
            theForm.parameters.Add(dvCurrentParameters); // для той, которая будет аппроксимацией
            theForm.lineColors.Add(new Bgr(Color.Magenta));
            theForm.lineColors.Add(new Bgr(Color.Black)); // для той, которая будет аппроксимацией

            ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(dvCurrentParameters), true);



            //
            //      отфильтровать значения по статстике за пределами 3s
            //

            DescriptiveStatistics stats = new DescriptiveStatistics(yDataValuesList);
            List<int> indexesToDelete = new List<int>();
            for (int i = yDataValuesList.Count - 1; i >= 0; i--)
            {
                if (Math.Abs(yDataValuesList[i] - stats.Mean) > 3.0d * stats.StandardDeviation)
                {
                    xDataValuesList.RemoveAt(i);
                    wDataValuesList.RemoveAt(i);
                }
            }
            yDataValuesList.RemoveAll(x => (Math.Abs(x - stats.Mean) > 3.0d * stats.StandardDeviation));

            dvFuncWeights = DenseVector.OfEnumerable(wDataValuesList);
            ////FunctionRepresentationForm theWeightsForm = new FunctionRepresentationForm("data weights");
            ////theWeightsForm.Show();
            ////theWeightsForm.dvScatterXSpace = DenseVector.OfEnumerable(xDataValuesList);
            ////theWeightsForm.dvScatterFuncValues = dvFuncWeights;
            ////theWeightsForm.xSpaceMin = theWeightsForm.dvScatterXSpace.Min() - (theWeightsForm.dvScatterXSpace.Max() - theWeightsForm.dvScatterXSpace.Min()) / 5.0d;
            ////theWeightsForm.xSpaceMax = theWeightsForm.dvScatterXSpace.Max() + (theWeightsForm.dvScatterXSpace.Max() - theWeightsForm.dvScatterXSpace.Min()) / 5.0d;
            ////theWeightsForm.Represent();

            xData = DenseVector.OfEnumerable(xDataValuesList);
            funcData = DenseVector.OfEnumerable(yDataValuesList);
            theForm.dvScatterXSpace = (DenseVector)xData.Clone();
            theForm.dvScatterFuncValues = (DenseVector)funcData.Clone();

            theForm.xSpaceMin = theForm.dvScatterXSpace.Min() - (theForm.dvScatterXSpace.Max() - theForm.dvScatterXSpace.Min()) / 5.0d;
            theForm.xSpaceMax = theForm.dvScatterXSpace.Max() + (theForm.dvScatterXSpace.Max() - theForm.dvScatterXSpace.Min()) / 5.0d;


            theForm.Represent();

            tbXMin.Text = theForm.xSpaceMin.ToString();
            tbXMax.Text = theForm.xSpaceMax.ToString();
            tbYMin.Text = theForm.overallFuncMin.ToString();
            tbYMax.Text = theForm.overallFuncMax.ToString();


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
            if (cbUseWeights.Checked)
            {
                theApproximator.DvWeights = dvFuncWeights;
            }
            
            DenseVector initialParametersIncremnt = DenseVector.Create(dvParameters.Count, (i => 1.0d));
            theApproximator.ApproximationGradientDescent2D(dvParameters, initialParametersIncremnt, 0.0000001d);

            //while (true)
            //{
            //    Thread.Sleep(1000);

            //    dvParameters[2] = dvParameters[2] + 0.2;
            //    SelfWorker.ReportProgress(10, (object)dvParameters);

            //    if (SelfWorker.CancellationPending)
            //    {
            //        e.Cancel = true;
            //        break;
            //    }
            //}
        }

        private void btnTestApprox_Click(object sender, EventArgs e)
        {
            stepsCount = 0;
            if (bgwApproximatorWorker.IsBusy)
            {
                bgwApproximatorWorker.CancelAsync();
                return;
            }
            //FunctionRepresentationForm theForm = new FunctionRepresentationForm("test function");
            //theForm.Show();

            //List<double> parametersList = new List<double>();
            //parametersList.Add(0.52d * 25.0d);//d1
            //parametersList.Add(0.52d * 15.0d);//d2
            //parametersList.Add(0.52d * 39.45d);//R
            //parametersList.Add(1.0d);//phi0
            //dvCurrentParameters = DenseVector.OfEnumerable(parametersList);

            //theLogWindow = ServiceTools.LogAText(theLogWindow, densevectorToString(dvCurrentParameters) + Environment.NewLine);
            ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(dvCurrentParameters), false);


            theApproximationFunction = new Func<DenseVector, double, double>((dvParametersLocal, x) =>
            {
                double d1 = dvParametersLocal[0];
                double d2 = dvParametersLocal[1];
                double r = dvParametersLocal[2];
                double phi0 = dvParametersLocal[3];
                return d1 * Math.Cos(x - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(x - phi0), 2.0d));
            });
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
                //List<double> parameterslist = new List<double>(dvCurrentParameters);
                Func<DenseVector, double, double> newFunc = new Func<DenseVector, double, double>((parametersListLoc, x) =>
                {
                    double d1 = parametersListLoc[0];
                    double d2 = parametersListLoc[1];
                    double r = parametersListLoc[2];
                    double phi0 = parametersListLoc[3];
                    return d1 * Math.Cos(x - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(x - phi0), 2.0d));
                });

                ServiceTools.logToTextFile(CurrentDir + dataFilePrefix + "approximationProgress.log",
                    Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters) + ";" + statsKurtosis.ToString("e").Replace(",", "."), true);

                theForm.theRepresentingFunctions[theForm.theRepresentingFunctions.Count - 1] = newFunc;
                theForm.parameters[theForm.parameters.Count - 1] = dvCurrentParameters;
                theForm.lineColors[theForm.lineColors.Count - 1] = new Bgr(0, 0, 0);
                theForm.Represent();
                //theForm.SaveToImage(CurrentDir + dataFilePrefix + "approximation-vis-" + stepsCount.ToString("D5") + ".jpg");
                

                //theLogWindow = ServiceTools.LogAText(theLogWindow, densevectorToString(dvCurrentParameters) + Environment.NewLine);
                ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(dvCurrentParameters), false);
                lblStatus.Text = stepsCount.ToString();
            }

        }




        private void bgwApproximatorWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //dvCurrentParameters = (DenseVector)e.UserState;
            //List<double> parameterslist = new List<double>(dvCurrentParameters);
            Func<DenseVector, double, double> newFunc = new Func<DenseVector, double, double>((parametersListLoc, x) =>
            {
                double d1 = parametersListLoc[0];
                double d2 = parametersListLoc[1];
                double r = parametersListLoc[2];
                double phi0 = parametersListLoc[3];
                return d1 * Math.Cos(x - phi0) + Math.Sqrt(r * r - d2 * d2 * Math.Pow(Math.Sin(x - phi0), 2.0d));
            });

            theForm.theRepresentingFunctions.Add(newFunc);
            theForm.parameters.Add(dvCurrentParameters);
            theForm.lineColors.Add(new Bgr(0, 0, 0));
            theForm.Represent();

            //theLogWindow = ServiceTools.LogAText(theLogWindow, densevectorToString(dvCurrentParameters) + Environment.NewLine);
            ThreadSafeOperations.SetTextTB(tbLog, "DONE" + Environment.NewLine + ServiceTools.densevectorToString(dvCurrentParameters), false);
            //theLogWindow = ServiceTools.LogAText(theLogWindow, "im done");
        }



        

        
        
        
        private void tb_TextChanged(object sender, EventArgs e)
        {
            if (theForm == null)
            {
                return;
            }

            if (sender == tbXMin)
            {
                theForm.xSpaceMin = Convert.ToDouble(tbXMin.Text);
                theForm.Represent();
            }

            if (sender == tbXMax)
            {
                theForm.xSpaceMax = Convert.ToDouble(tbXMax.Text);
                theForm.Represent();
            }

            if (sender == tbYMin)
            {
                theForm.overallFuncMin = Convert.ToDouble(tbYMin.Text);
                theForm.Represent();
            }


            if (sender == tbYMax)
            {
                theForm.overallFuncMax = Convert.ToDouble(tbYMax.Text);
                theForm.Represent();
            }
        }

        private void btnTaxonomyTest_Click(object sender, EventArgs e)
        {
            theForm = new FunctionRepresentationForm("test function");
            theForm.Show();



            StreamReader sr = new StreamReader(CurrentDir + "2007-polarstern-CanonA640-stats.dat");
            string dataString = sr.ReadToEnd();
            string[] dataLines = dataString.Split('\n');
            sr.Close();
            sr.Dispose();
            List<double> perc5List = new List<double>();
            List<double> medianList = new List<double>();
            foreach (string dataLine in dataLines)
            {
                string[] lineSunbstring = dataLine.Split(',');
                double currentMedian = 0.0d;
                double currentPerc5 = 0.0d;

                try
                {
                    currentMedian = Convert.ToDouble(lineSunbstring[1].Replace(".", ","));
                    currentPerc5 = Convert.ToDouble(lineSunbstring[2].Replace(".", ","));
                }
                catch (Exception)
                {
                    continue;
                }
                perc5List.Add(currentPerc5);
                medianList.Add(currentMedian);
            }

            List<PointD> pointsList = new List<PointD>();
            for (int i = 0; i < medianList.Count; i++)
            {
                PointD pt = new PointD(medianList[i], perc5List[i]);
                pointsList.Add(pt);
            }

            DenseVector dvDataDistribution = DenseVector.OfEnumerable(perc5List);
            DenseVector dvDataSpace = DenseVector.OfEnumerable(medianList);
            //int polynomeOrder = 21;
            //DenseVector approxPolyKoeffs = DataAnalysis.NPolynomeApproximationLessSquareMethod(dvDataDistribution, dvDataSpace, null, polynomeOrder);

            //Func<DenseVector, double, double> theFunctionToShow = DataAnalysis.PolynomeValue;

            //theForm.theRepresentingFunctions.Add(theFunctionToShow);
            //theForm.parameters.Add(approxPolyKoeffs);
            //theForm.lineColors.Add(new Bgr(Color.Magenta));

            theForm.dvScatterXSpace = (DenseVector)dvDataSpace.Clone();
            theForm.dvScatterFuncValues = (DenseVector)dvDataDistribution.Clone();
            theForm.xSpaceMin = theForm.dvScatterXSpace.Values.Min();
            theForm.xSpaceMax = theForm.dvScatterXSpace.Values.Max();
            theForm.overallFuncMin = theForm.dvScatterFuncValues.Values.Min();
            theForm.overallFuncMax = theForm.dvScatterFuncValues.Values.Max();
            theForm.Represent();

            //ThreadSafeOperations.SetTextTB(tbLog, ServiceTools.densevectorToString(approxPolyKoeffs), true);


            ForelClusterization mlForel = new ForelClusterization(
                pointsList,
                new Func<PointD, PointD, double>((pt1, pt2) => pt1.Distance(pt2)),
                0.01d,
                10);
            mlForel.theLogWindow = theLogWindow;
            List<ForelTaxa> taxaList = mlForel.GetHighLevelTaxonomy();
            //List<ForelTaxa> lowLevelTaxaList = mlForel.GetLowLevelTaxonomy(0.01d, pointsList, 100);
            
            //AdjacencyGraph<int, TaggedEdge<int, double>> g = mlForel.GetHighLevelTaxonomy();
            //var populator = GleeGraphExtensions.CreateGleePopulator<int, TaggedEdge<int, double>>(g);
            //populator.Compute();
            //Microsoft.Glee.Drawing.Graph g1 = populator.GleeGraph;
            //gViewer.Graph = g1;


            Image<Bgr, byte> representingImage = ForelClusterization.RepresentTaxaList(taxaList, 0.4d, 1.0d, 0.4d, 1.0d, 1024, 1024, true);
            ServiceTools.ShowPicture(representingImage, "");


            
        }

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
    }
}
