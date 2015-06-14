using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Research.Science.Data;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.Research.Science.Data.NetCDF4;


namespace SkyImagesAnalyzerLibraries
{
    public class NetCDFoperations
    {
        public static void SaveDataToFile(DenseMatrix dmData, string fileName, TextBox tbLog, bool absolutePath = false,
            string varName = "dataMatrix")
        {
            string baseDir = "G:\\_gulevlab\\SkyIndexAnalyzerSolo_appData\\_dataDirectory\\";
            string connectionString = "msds:nc?file=";
            if (absolutePath)
            {
                if (!ServiceTools.CheckIfDirectoryExists(fileName)) return;
                connectionString += fileName;
            }
            else
            {
                if (!ServiceTools.CheckIfDirectoryExists(baseDir + fileName)) return;
                connectionString += baseDir + fileName;
            }

            NetCDFDataSet ds = null;

            try
            {
                ds = new NetCDFDataSet(connectionString, ResourceOpenMode.Create);
            }
            catch (Exception ex)
            {
                if (tbLog != null)
                {
                    ThreadSafeOperations.SetTextTB(tbLog, "Не получилось :( " + Environment.NewLine + ex.Message, true);
                }
                else
                {
                    throw ex;
                }
            }
            
            Variable<double> thDataVar = ds.AddVariable<double>(varName, dmData.ToArray(), "y", "x");

            try
            {
                ds.TryCommit();
            }
            catch (Exception ex)
            {
                if (tbLog != null)
                {
                    ThreadSafeOperations.SetTextTB(tbLog, "Не получилось :( " + Environment.NewLine + ex.Message, true);
                }
                else
                {
                    throw ex;
                }
            }

            ds.Dispose();
        }




        public static void AddDataMatrixToFile(DenseMatrix dmData, string fileName, TextBox tbLog,
            bool absolutePath = false, string varName = "dataMatrix")
        {
            string baseDir = "G:\\_gulevlab\\SkyIndexAnalyzerSolo_appData\\_dataDirectory\\";
            string connectionString = "msds:nc?file=";
            if (absolutePath)
            {
                if (!ServiceTools.CheckIfDirectoryExists(fileName)) return;
                connectionString += fileName;
            }
            else
            {
                if (!ServiceTools.CheckIfDirectoryExists(baseDir + fileName)) return;
                connectionString += baseDir + fileName;
            }

            NetCDFDataSet ds = null;

            try
            {
                ds = new NetCDFDataSet(connectionString, ResourceOpenMode.OpenOrCreate);
            }
            catch (Exception ex)
            {
                if (tbLog != null)
                {
                    ThreadSafeOperations.SetTextTB(tbLog, "Не получилось :( " + Environment.NewLine + ex.Message, true);
                }
                else
                {
                    throw ex;
                }
            }
            
            
            Variable<double> thDataVar;
            if (!ds.Variables.Contains(varName))
            {
                thDataVar = ds.AddVariable<double>(varName, dmData.ToArray(), "y", "x");
            }
            else
            {
                thDataVar = (Variable<double>)ds.Variables[varName];
                thDataVar.Append(dmData.ToArray());
            }


            try
            {
                ds.TryCommit();
            }
            catch (Exception ex)
            {
                if (tbLog != null)
                {
                    ThreadSafeOperations.SetTextTB(tbLog, "Не получилось :( " + Environment.NewLine + ex.Message, true);
                }
                else
                {
                    throw ex;
                }
            }

            ds.Dispose();
        }




        public static DenseMatrix ReadDenseMatrixFromFile(string fileName)
        {
            double[,] res = null;

            if (!ServiceTools.CheckIfDirectoryExists(fileName)) return null;
            NetCDFDataSet ds = null;

            try
            {
                ds = new NetCDFDataSet("msds:nc?file=" + fileName + "&enableRollback=false",
                    ResourceOpenMode.ReadOnly);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            //Variable<double> thDataVar = ds.AddVariable<double>("dataMatrix", dmData.ToArray(), "y", "x");
            //Variable<double> thDataVar = ds.Variables
            foreach (Variable theVar in ds)
            {
                res = (double[,])theVar.GetData();
            }

            return DenseMatrix.OfArray(res);
        }





        public static DenseMatrix ReadDenseMatrixFromFile(string fileName, string varName = "DataMatrix")
        {
            double[,] res = null;
            DenseMatrix dmRes = null;

            if (!ServiceTools.CheckIfDirectoryExists(fileName)) return null;

            NetCDFDataSet ds = null;

            try
            {
                ds = new NetCDFDataSet("msds:nc?file=" + fileName + "&enableRollback=false", ResourceOpenMode.ReadOnly);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //Variable<double> thDataVar = ds.AddVariable<double>("dataMatrix", dmData.ToArray(), "y", "x");
            //Variable<double> thDataVar = ds.Variables
            foreach (Variable theVar in ds)
            {
                if (theVar.Name != varName) continue;
                else
                {
                    if (theVar.TypeOfData == typeof(double))
                    {
                        res = (double[,])theVar.GetData();
                        dmRes = DenseMatrix.OfArray(res);
                    }
                    else if (theVar.TypeOfData == typeof(Single))
                    {
                        Single[,] res1 = (Single[,])theVar.GetData();
                        dmRes = DenseMatrix.Create(theVar.Dimensions[0].Length, theVar.Dimensions[1].Length, ((i, j) =>
                        {
                            return Convert.ToDouble(res1[i, j]);
                        }));
                    }

                    break;
                }
            }

            return dmRes;
        }





        public static Dictionary<string, object> ReadDataFromFile(string fileName, List<string> varNames = null)
        {
            if (!ServiceTools.CheckIfDirectoryExists(fileName)) return null;

            Dictionary<string, object> retDict = new Dictionary<string, object>();
            double[,] res = null;
            NetCDFDataSet ds = null;

            try
            {
                ds = new NetCDFDataSet("msds:nc?file=" + fileName + "&enableRollback=false", ResourceOpenMode.ReadOnly);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (Variable theVar in ds)
            {
                bool loadThisVar = false;
                if (varNames == null) loadThisVar = true;
                else if (varNames.Contains(theVar.Name)) loadThisVar = true;
                if (!loadThisVar) continue;

                if (theVar.TypeOfData == typeof(double))
                {
                    if (theVar.Dimensions.Count == 1)
                    {
                        double[] res1 = (double[])theVar.GetData();
                        retDict.Add(theVar.Name, DenseVector.OfEnumerable(res1));
                    }
                    else if (theVar.Dimensions.Count == 2)
                    {
                        double[,] res1 = (double[,])theVar.GetData();
                        retDict.Add(theVar.Name, DenseMatrix.OfArray(res1));
                    }

                }



                if (theVar.TypeOfData == typeof(Single))
                {
                    if (theVar.Dimensions.Count == 1)
                    {
                        Single[] res1 = (Single[])theVar.GetData();
                        DenseVector dvRes1 = DenseVector.Create(theVar.Dimensions[0].Length,
                            i => Convert.ToDouble(res1[i]));
                        retDict.Add(theVar.Name, dvRes1);
                    }
                    else if (theVar.Dimensions.Count == 2)
                    {
                        Single[,] res1 = (Single[,])theVar.GetData();
                        DenseMatrix dmRes = DenseMatrix.Create(theVar.Dimensions[0].Length, theVar.Dimensions[1].Length,
                            ((i, j) =>
                            {
                                return Convert.ToDouble(res1[i, j]);
                            }));
                        retDict.Add(theVar.Name, dmRes);
                    }
                }



                if (theVar.TypeOfData == typeof(long))
                {
                    if (theVar.Dimensions.Count == 1)
                    {
                        long[] res1 = (long[])theVar.GetData();
                        retDict.Add(theVar.Name, res1);
                    }
                    else if (theVar.Dimensions.Count == 2)
                    {
                        long[,] res1 = (long[,])theVar.GetData();
                        retDict.Add(theVar.Name, res1);
                    }
                }


                if (theVar.TypeOfData == typeof(int))
                {
                    if (theVar.Dimensions.Count == 1)
                    {
                        int[] res1 = (int[])theVar.GetData();
                        retDict.Add(theVar.Name, res1);
                    }
                    else if (theVar.Dimensions.Count == 2)
                    {
                        int[,] res1 = (int[,])theVar.GetData();
                        retDict.Add(theVar.Name, res1);
                    }
                }


                if (theVar.TypeOfData == typeof(string))
                {

                    string[] res1 = (string[])theVar.GetData();
                    retDict.Add(theVar.Name, res1);
                }
            }

            return retDict;
        }



        public static void SaveVariousDataToFile(Dictionary<string, object> dataToWrite, string fileName)
        {
            if (!ServiceTools.CheckIfDirectoryExists(fileName)) return;

            string connectionString = "msds:nc?file=";
            connectionString += fileName;
            NetCDFDataSet ds = null;
            try
            {
                ds = new NetCDFDataSet(connectionString, ResourceOpenMode.Create);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (KeyValuePair<string, object> keyValuePair in dataToWrite)
            {
                if (keyValuePair.Value.GetType() == typeof(DenseMatrix))
                {
                    Variable<double> theDataVar = ds.AddVariable<double>(keyValuePair.Key, ((DenseMatrix)(keyValuePair.Value)).ToArray(), "y", "x");
                }

                if (keyValuePair.Value.GetType() == typeof(DenseVector))
                {
                    Variable<double> theDataVar = ds.AddVariable<double>(keyValuePair.Key, ((DenseVector)(keyValuePair.Value)).ToArray(), keyValuePair.Key);
                }

                if (keyValuePair.Value.GetType() == typeof(long[]))
                {
                    Variable<long> theDataVar = ds.AddVariable<long>(keyValuePair.Key, (long[])(keyValuePair.Value), keyValuePair.Key);
                }

                if (keyValuePair.Value.GetType() == typeof(int[]))
                {
                    Variable<int> theDataVar = ds.AddVariable<int>(keyValuePair.Key, (int[])(keyValuePair.Value), keyValuePair.Key);
                }


                if (keyValuePair.Value.GetType() == typeof(string[]))
                {
                    Variable<string> theDataVar = ds.AddVariable<string>(keyValuePair.Key, (string[])(keyValuePair.Value), keyValuePair.Key);
                }
            }



            try
            {
                ds.TryCommit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ds.Dispose();
        }






        public static void AddVariousDataToFile(Dictionary<string, object> dataToWrite, string fileName)
        {
            if (!ServiceTools.CheckIfDirectoryExists(fileName)) return;

            string connectionString = "msds:nc?file=";
            connectionString += fileName;
            NetCDFDataSet ds = null;

            try
            {
                ds = new NetCDFDataSet(connectionString, ResourceOpenMode.OpenOrCreate);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (KeyValuePair<string, object> keyValuePair in dataToWrite)
            {
                if (keyValuePair.Value.GetType() == typeof(DenseMatrix))
                {
                    if (ds.Variables.Contains(keyValuePair.Key))
                    {
                        Variable<double> theDataVar = (Variable<double>)ds.Variables[keyValuePair.Key];
                        theDataVar.Append(((DenseMatrix)keyValuePair.Value).ToArray());
                    }
                    else
                    {
                        Variable<double> theDataVar = ds.AddVariable<double>(keyValuePair.Key,
                            ((DenseMatrix)(keyValuePair.Value)).ToArray(), "y", "x");
                    }
                }

                if (keyValuePair.Value.GetType() == typeof(DenseVector))
                {
                    if (ds.Variables.Contains(keyValuePair.Key))
                    {
                        Variable<double> theDataVar = (Variable<double>)ds.Variables[keyValuePair.Key];
                        theDataVar.Append(((DenseVector)keyValuePair.Value).ToArray());
                    }
                    else
                    {
                        Variable<double> theDataVar = ds.AddVariable<double>(keyValuePair.Key,
                            ((DenseVector)(keyValuePair.Value)).ToArray(), keyValuePair.Key);
                    }
                }

                if (keyValuePair.Value.GetType() == typeof(long[]))
                {
                    if (ds.Variables.Contains(keyValuePair.Key))
                    {
                        Variable<long> theDataVar = (Variable<long>)ds.Variables[keyValuePair.Key];
                        theDataVar.Append((long[])(keyValuePair.Value));
                    }
                    else
                    {
                        Variable<long> theDataVar = ds.AddVariable<long>(keyValuePair.Key, (long[])(keyValuePair.Value),
                            keyValuePair.Key);
                    }
                }

                if (keyValuePair.Value.GetType() == typeof(int[]))
                {
                    if (ds.Variables.Contains(keyValuePair.Key))
                    {
                        Variable<int> theDataVar = (Variable<int>)ds.Variables[keyValuePair.Key];
                        theDataVar.Append((int[])(keyValuePair.Value));
                    }
                    else
                    {
                        Variable<int> theDataVar = ds.AddVariable<int>(keyValuePair.Key, (int[])(keyValuePair.Value),
                            keyValuePair.Key);
                    }
                }

                if (keyValuePair.Value.GetType() == typeof(string[]))
                {
                    if (ds.Variables.Contains(keyValuePair.Key))
                    {
                        Variable<string> theDataVar = (Variable<string>)ds.Variables[keyValuePair.Key];
                        theDataVar.Append((string[])(keyValuePair.Value));
                    }
                    else
                    {
                        Variable<string> theDataVar = ds.AddVariable<string>(keyValuePair.Key, (string[])(keyValuePair.Value),
                            keyValuePair.Key);
                    }
                }
            }

            try
            {
                ds.TryCommit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ds.Dispose();
        }



        public static string DumpMatrixToNCFile(DenseMatrix dmData)
        {
            string baseDir = Directory.GetCurrentDirectory() + "\\";
            string fileName = DateTime.UtcNow.ToString("o").Replace(":", "-") + ".nc";
            string connectionString = "msds:nc?file=";
            connectionString += baseDir + fileName;

            NetCDFDataSet ds = null;

            try
            {
                ds = new NetCDFDataSet(connectionString, ResourceOpenMode.Create);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            Variable<double> thDataVar = ds.AddVariable<double>("DataMatrix", dmData.ToArray(), "y", "x");

            try
            {
                ds.TryCommit();
            }
            catch (Exception exc)
            {
                return "failed-to-dump";
            }

            ds.Dispose();
            return fileName;
        }
    }
}
