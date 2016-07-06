using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ANN;
using SkyImagesAnalyzerLibraries;
using MathNet.Numerics.LinearAlgebra.Double;


namespace SDCpredictNN
{
    public class NNPredictor
    {
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";

        private string NNconfigFile = "";
        private string NNtrainedParametersFile = "";
        private string NormMeansFile = "";
        private string NormRangeFile = "";


        
        public void Start(string[] args)
        {
            readDefaultProperties();

            List<string> argsList = new List<string>(args);

            string filename = argsList.Last();
            if (!File.Exists(filename))
            {
                Console.WriteLine("Couldn`t find input file \"" + filename + "\"");
                return;
            }

            CommonTools.PrintDictionaryToConsole(defaultProperties,
                    "Default settings specified in file \"" + defaultPropertiesXMLfileName + "\"");


            if (!File.Exists(NNconfigFile) || !File.Exists(NNtrainedParametersFile) || !File.Exists(NormMeansFile) || !File.Exists(NormRangeFile))
            {
                Console.WriteLine(
                    "couldn`t find at least one of pre-calculated NN parameters file specified in settings: ");
                return;
            }



            List<List<string>> csvFileContentStrings = ServiceTools.ReadDataFromCSV(filename, 0, true, ",");
            List<string> csvFileHeader = csvFileContentStrings[0];
            csvFileContentStrings = csvFileContentStrings.Where((list, idx) => idx > 0).ToList();
            List<int> columnsToDelete =
                csvFileHeader.Select((str, idx) => new Tuple<int, string>(idx, str))
                    .Where(tpl => tpl.Item2.ToLower().Contains("filename")).ToList().ConvertAll(tpl => tpl.Item1);
            List<List<string>> csvFileContentStringsFiltered = new List<List<string>>();
            foreach (List<string> listDataStrings in csvFileContentStrings)
            {
                csvFileContentStringsFiltered.Add(
                    listDataStrings.Where((str, idx) => !columnsToDelete.Contains(idx)).ToList());
            }

            List<SunDiskCondition> trueAnswers =
                csvFileContentStringsFiltered.ConvertAll(
                    lstr => (SunDiskCondition)Enum.Parse(typeof(SunDiskCondition), lstr.Last()));

            
            //List<int> trueAnswersInt = trueAnswers.ConvertAll(sdc => SunDiskConditionData.MatlabNumeralSDC(sdc));

            List<List<string>> csvFileContentStringsFiltered_wo_sdc =
                csvFileContentStringsFiltered.ConvertAll(list => list.Where((val, idx) => idx < list.Count - 1).ToList());

            List<DenseVector> lDV_objects_features =
                csvFileContentStringsFiltered_wo_sdc.ConvertAll(
                    list =>
                        DenseVector.OfEnumerable(list.ConvertAll<double>(str => Convert.ToDouble(str.Replace(".", ",")))));

            
            DenseVector dvMeans = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormMeansFile, 0, ",")).Row(0);
            DenseVector dvRanges = (DenseVector)((DenseMatrix)ServiceTools.ReadDataFromCSV(NormRangeFile, 0, ",")).Row(0);

            lDV_objects_features = lDV_objects_features.ConvertAll(dv =>
            {
                DenseVector dvShifted = dv - dvMeans;
                DenseVector dvNormed = (DenseVector) dvShifted.PointwiseDivide(dvRanges);
                return dvNormed;
            });

            DenseMatrix dmObjectsFeatures = DenseMatrix.OfRowVectors(lDV_objects_features);

            DenseVector dvThetaValues = (DenseVector)ServiceTools.ReadDataFromCSV(NNtrainedParametersFile, 0, ",");
            List<int> NNlayersConfig =
                new List<double>(((DenseMatrix) ServiceTools.ReadDataFromCSV(NNconfigFile, 0, ",")).Row(0)).ConvertAll
                    (dVal => Convert.ToInt32(dVal));

            List<SunDiskCondition> predictedSDC =
                NNclassificatorPredictor<SunDiskCondition>.NNpredict(dmObjectsFeatures, dvThetaValues, NNlayersConfig,
                    SunDiskConditionData.MatlabEnumeratedSDCorderedList()).ToList();

            List<Tuple<SunDiskCondition, SunDiskCondition>> PredictedVStrue = predictedSDC.Zip(trueAnswers,
                (predVal, trueVal) => new Tuple<SunDiskCondition, SunDiskCondition>(predVal, trueVal)).ToList();


            Console.WriteLine("=== Prediction result vs true ===");
            foreach (Tuple<SunDiskCondition, SunDiskCondition> tpl in PredictedVStrue)
            {
                Console.WriteLine("pred: " + tpl.Item1.ToString() + ":" + tpl.Item2.ToString() + " :true");
            }

            double accuracy = 100.0d* ((double) PredictedVStrue.Count(tpl => tpl.Item1 == tpl.Item2))/
                              (double) PredictedVStrue.Count();
            Console.WriteLine("accuracy: " + accuracy);


            Console.WriteLine("Finished. Press any key...");
            Console.ReadKey();
        }



        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";

            if (File.Exists(defaultPropertiesXMLfileName))
            {
                defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);
            }

            bool bDefaultPropertiesHasBeenUpdated = false;

            string CurDir = Directory.GetCurrentDirectory();


            // NNconfigFile
            if (defaultProperties.ContainsKey("NNconfigFile"))
            {
                NNconfigFile = ((string)defaultProperties["NNconfigFile"]);
            }
            else
            {
                NNconfigFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNconfig.csv";
                defaultProperties.Add("NNconfigFile", NNconfigFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            //NNtrainedParametersFile
            if (defaultProperties.ContainsKey("NNtrainedParametersFile"))
            {
                NNtrainedParametersFile = ((string)defaultProperties["NNtrainedParametersFile"]);
            }
            else
            {
                NNtrainedParametersFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NNtrainedParameters.csv";
                defaultProperties.Add("NNtrainedParametersFile", NNtrainedParametersFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }



            // NormMeansFile
            if (defaultProperties.ContainsKey("NormMeansFile"))
            {
                NormMeansFile = ((string)defaultProperties["NormMeansFile"]);
            }
            else
            {
                NormMeansFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormMeans.csv";
                defaultProperties.Add("NormMeansFile", NormMeansFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            // NormRangeFile
            if (defaultProperties.ContainsKey("NormRangeFile"))
            {
                NormRangeFile = ((string)defaultProperties["NormRangeFile"]);
            }
            else
            {
                NormRangeFile = CurDir + Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar + "NormRange.csv";
                defaultProperties.Add("NormRangeFile", NormRangeFile);
                bDefaultPropertiesHasBeenUpdated = true;
            }




            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }


        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }
    }
}
