using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
//using MathNet.Numerics.LinearAlgebra.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.CSharp;

namespace SkyImagesAnalyzerLibraries2G
{
    public class ServiceTools2G
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr handle, int minimumWorkingSetSize, int maximumWorkingSetSize);



        public static object getPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }


        public static void FlushMemory(TextBox tb = null, string mark = "")
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            //}
        }

        

        public static double[,,] AddedThirdIndexArray3DFrom2D(double[,] inputArray)
        {
            double[,,] result = new double[inputArray.GetLength(0), inputArray.GetLength(1), 1];

            for (int i = 0; i < inputArray.GetLength(0); i++)
            {
                for (int j = 0; j < inputArray.GetLength(1); j++)
                {
                    result[i, j, 0] = inputArray[i, j];
                }
            }

            return result;
        }


        public static Byte[,,] AddedThirdIndexArray3DFrom2D(Byte[,] inputArray)
        {
            Byte[,,] result = new Byte[inputArray.GetLength(0), inputArray.GetLength(1), 1];

            for (int i = 0; i < inputArray.GetLength(0); i++)
            {
                for (int j = 0; j < inputArray.GetLength(1); j++)
                {
                    result[i, j, 0] = inputArray[i, j];
                }
            }

            return result;
        }




        public static byte[,] DoubleToByteDepth(double[,] in_arr)
        {
            byte[,] out_arr = new byte[in_arr.GetLength(0), in_arr.GetLength(1)];
            for (int i = 0; i < in_arr.GetLength(0); i++)
                for (int j = 0; j < in_arr.GetLength(1); j++)
                {
                    double x = in_arr[i, j];
                    if (Double.IsNaN(x)) x = 0.0;
                    if (x <= 0.0) x = 0.0;
                    out_arr[i, j] = (byte)Math.Round(x, 0);
                }

            return out_arr;
        }

        

        public static string DoubleValueRepresentingString(double theValue)
        {
            string str = "";
            if (Math.Abs(theValue) > 1.0d) str = Math.Round(theValue, 2).ToString();
            else
            {
                str = theValue.ToString("e");
            }

            return str;
        }



        public static void logToTextFile(string Filename, string text = "", bool append = false, bool withTimeStamp = false)
        {
            FileStream textFileStream = null;
            StreamWriter sw;

            FileInfo finfo = new FileInfo(Filename);
            if (!Directory.Exists(finfo.DirectoryName))
            {
                Directory.CreateDirectory(finfo.DirectoryName);
            }

            bool successfulOpening = false;
            int tries = 0;
            while (!successfulOpening)
            {
                if (tries >= 10)
                {
                    Filename = NewNumberedFilenameThatDoesntExist(Filename);
                }

                try
                {
                    textFileStream = new FileStream(Filename, (append) ? (FileMode.Append) : (FileMode.Create), FileAccess.Write);
                    successfulOpening = true;
                }
                catch (Exception ex)
                {
                    tries++;
                    Thread.Sleep(20);
                }
            }


            if (withTimeStamp)
            {
                text = DateTime.UtcNow.ToString("s") + ":" + Environment.NewLine + text;
            }

            byte[] info2write = new UTF8Encoding(true).GetBytes(text);

            textFileStream.Write(info2write, 0, info2write.Length);
            textFileStream.Close();
        }





        public static string NewNumberedFilenameThatDoesntExist(string sourceFileName)
        {
            if (!File.Exists(sourceFileName))
            {
                return sourceFileName;
            }

            int counter = 1;
            string newFileName = sourceFileName + counter;
            while (File.Exists(newFileName))
            {
                counter++;
                newFileName = sourceFileName + counter;
            }
            return newFileName;
        }





#if WIN32
        public static int CurrentCodeLineNumber(
            [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0)
        {
            return callingFileLineNumber;
        }




        public static string CurrentCodeLineDescription(
            [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0)
        {
            string str = "line " + callingFileLineNumber + Environment.NewLine
                + "in method " + callingMethod + Environment.NewLine
                + "in file " + callingFilePath;
            return str;
        }
#endif



        public static string GetExceptionMessages(Exception e, string msgs = "")
        {
            if (e == null) return String.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            return msgs;
        }


        

        public static Bitmap ReadBitmapFromFile(string FullFilename)
        {
            Bitmap bm1 = new Bitmap(FullFilename);

            return CopyBitmap(bm1);
        }



        public static Bitmap CopyBitmap(Bitmap bm2copy)
        {
            Bitmap Temp = (Bitmap)bm2copy.Clone();
            Bitmap Copy = new Bitmap(Temp.Width, Temp.Height);
            Copy.SetResolution(Temp.HorizontalResolution, Temp.VerticalResolution);
            using (Graphics g = Graphics.FromImage(Copy))
            {
                g.DrawImageUnscaled(Temp, 0, 0);
            }
            Temp.Dispose();
            return Copy;
        }




        public static string densevectorToString(DenseVector dvData)
        {
            string strOut = "";
            foreach (double d in dvData)
            {
                strOut += d.ToString("e").Replace(",", ".") + ";" + Environment.NewLine;
            }
            strOut = strOut.Substring(0, strOut.Length - 5);
            return strOut;
        }



        public static Tuple<int, int, double> DenseMatrixAbsoluteMinimumPosition(DenseMatrix dm)
        {
            if (dm == null) return new Tuple<int, int, double>(0, 0, 0.0d);

            List<Tuple<int, double>> rowMinValues = new List<Tuple<int, double>>();

            var rowEnum = dm.EnumerateRowsIndexed();
            foreach (Tuple<int, Vector<double>> curRow in rowEnum)
            {
                DenseVector curDV = (DenseVector)curRow.Item2;
                rowMinValues.Add(new Tuple<int, double>(curDV.AbsoluteMinimumIndex(), curDV.AbsoluteMinimum()));
            }

            double minValue = rowMinValues.Min(tpl => tpl.Item2);
            int minValueRow = rowMinValues.FindIndex(tpl => tpl.Item2 == minValue);
            DenseVector minRow = (DenseVector)dm.Row(minValueRow);



            return new Tuple<int, int, double>(minValueRow, minRow.AbsoluteMinimumIndex(), minValue);
        }



        public static Tuple<int, int, double> DenseMatrixAbsoluteMaximumPosition(DenseMatrix dm)
        {
            if (dm == null) return new Tuple<int, int, double>(0, 0, 0.0d);

            List<Tuple<int, double>> rowMaxValues = new List<Tuple<int, double>>();

            var rowEnum = dm.EnumerateRowsIndexed();
            foreach (Tuple<int, Vector<double>> curRow in rowEnum)
            {
                DenseVector curDV = (DenseVector)curRow.Item2;
                rowMaxValues.Add(new Tuple<int, double>(curDV.AbsoluteMaximumIndex(), curDV.AbsoluteMaximum()));
            }

            double maxValue = rowMaxValues.Max(tpl => tpl.Item2);
            int maxValueRow = rowMaxValues.FindIndex(tpl => tpl.Item2 == maxValue);
            DenseVector maxRow = (DenseVector)dm.Row(maxValueRow);



            return new Tuple<int, int, double>(maxValueRow, maxRow.AbsoluteMaximumIndex(), maxValue);
        }


        public static Dictionary<string, object> ReadDictionaryFromXML(string filename)
        {
            Dictionary<string, object> retDict = new Dictionary<string, object>();
            DataSet readingDataSet = new DataSet("DataSet");
            try
            {
                readingDataSet.ReadXml(filename);
            }
            catch (Exception)
            {
                return null;
                throw;
            }

            foreach (DataTable table in readingDataSet.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    retDict.Add(row[0] as string, row[1]);
                }
            }
            readingDataSet.Dispose();
            return retDict;
        }


        public static string WriteDictionaryToXml(Dictionary<string, object> dictToWrite, string filename, bool append = false, bool savePreviousFileVersion = false)
        {
            FileInfo finfo = new FileInfo(filename);
            if (!Directory.Exists(finfo.DirectoryName))
            {
                Directory.CreateDirectory(finfo.DirectoryName);
            }



            if (File.Exists(filename) && !append && savePreviousFileVersion)
            {
                string filename_cp = ServiceTools2G.NewNumberedFilenameThatDoesntExist(filename);
                bool success = false;
                int counter = 0;
                while ((!success) && (counter <= 10))
                {
                    try
                    {
                        File.Move(filename, filename_cp);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        counter++;
                        Thread.Sleep(20);
                    }
                }
                if (!success)
                {
                    filename = filename_cp;
                }
            }




            DataSet dsToWrite = new DataSet("DataSet");
            dsToWrite.Namespace = "NetFrameWork";
            DataTable table = new DataTable("table");

            DataColumn keyColumn = new DataColumn("key", Type.GetType("System.String"));

            DataColumn valueColumn = new DataColumn("value");
            table.Columns.Add(keyColumn);
            table.Columns.Add(valueColumn);
            dsToWrite.Tables.Add(table);

            DataRow newRow;

            if (append && File.Exists(filename))
            {
                Dictionary<string, object> tmpDict = new Dictionary<string, object>();
                DataSet readingDataSet = new DataSet("DataSet");
                readingDataSet.ReadXml(filename);
                foreach (DataTable tmpTable in readingDataSet.Tables)
                {
                    foreach (DataRow row in tmpTable.Rows)
                    {
                        tmpDict.Add(row[0] as string, row[1]);
                    }
                }
                readingDataSet.Dispose();

                foreach (KeyValuePair<string, object> pair in tmpDict)
                {
                    newRow = table.NewRow();
                    newRow["key"] = pair.Key;
                    newRow["value"] = pair.Value;
                    table.Rows.Add(newRow);
                }
                File.Delete(filename);
            }

            foreach (KeyValuePair<string, object> pair in dictToWrite)
            {
                newRow = table.NewRow();
                newRow["key"] = pair.Key;
                newRow["value"] = pair.Value;
                table.Rows.Add(newRow);
            }
            dsToWrite.AcceptChanges();

            bool sucess = false;
            int tries = 0;
            while ((!sucess) && (tries <= 10))
            {
                try
                {
                    dsToWrite.WriteXml(filename);
                    sucess = true;
                }
                catch (Exception ex)
                {
                    Thread.Sleep(20);
                    sucess = false;
                    tries++;
                }
            }


            dsToWrite.Dispose();

            return filename;
        }



        public static List<object> ReadListFromXML(string filename)
        {
            List<object> retList = new List<object>();
            DataSet readingDataSet = new DataSet("DataSet");
            try
            {
                readingDataSet.ReadXml(filename);
            }
            catch (Exception)
            {
                return null;
                throw;
            }

            foreach (DataTable table in readingDataSet.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    retList.Add(row[0]);
                }
            }
            readingDataSet.Dispose();
            return retList;
        }




        public static string WriteListToXml(List<object> listToWrite, string filename, bool append = false)
        {
            FileInfo finfo = new FileInfo(filename);
            if (!Directory.Exists(finfo.DirectoryName))
            {
                Directory.CreateDirectory(finfo.DirectoryName);
            }

            if (File.Exists(filename) && !append)
            {
                File.Delete(filename);
            }

            DataSet dsToWrite = new DataSet("DataSet");
            dsToWrite.Namespace = "NetFrameWork";
            DataTable table = new DataTable("table");

            //DataColumn keyColumn = new DataColumn("key", Type.GetType("System.String"));

            DataColumn valueColumn = new DataColumn("value");
            //table.Columns.Add(keyColumn);
            table.Columns.Add(valueColumn);
            dsToWrite.Tables.Add(table);

            DataRow newRow;

            if (append && File.Exists(filename))
            {
                List<object> tmpList = new List<object>();
                DataSet readingDataSet = new DataSet("DataSet");
                readingDataSet.ReadXml(filename);
                foreach (DataTable tmpTable in readingDataSet.Tables)
                {
                    foreach (DataRow row in tmpTable.Rows)
                    {
                        tmpList.Add(row[0]);
                    }
                }
                readingDataSet.Dispose();

                foreach (object obj in tmpList)
                {
                    newRow = table.NewRow();
                    newRow["value"] = obj;
                    table.Rows.Add(newRow);
                }
                File.Delete(filename);
            }

            foreach (object obj in listToWrite)
            {
                newRow = table.NewRow();
                newRow["value"] = obj;
                table.Rows.Add(newRow);
            }
            dsToWrite.AcceptChanges();

            dsToWrite.WriteXml(filename);

            dsToWrite.Dispose();

            return filename;
        }




        public static bool CheckIfDirectoryExists(string filename)
        {
            FileInfo finfo = new FileInfo(filename);
            if (!Directory.Exists(finfo.DirectoryName))
            {
                try
                {
                    Directory.CreateDirectory(finfo.DirectoryName);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }





        public static string WriteObjectToXML(object objToSave, string fileName)
        {
            FileInfo finfo = new FileInfo(fileName);
            if (!Directory.Exists(finfo.DirectoryName))
            {
                Directory.CreateDirectory(finfo.DirectoryName);
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            Stream fs = new FileStream(fileName, FileMode.Create);
            XmlWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
            XmlSerializer serializer = new XmlSerializer(objToSave.GetType());
            serializer.Serialize(writer, objToSave);
            writer.Close();
            return fileName;
        }




        public static object ReadObjectFromXML(string fileName, Type objType)
        {
            if (!File.Exists(fileName)) return null;

            FileInfo finfo = new FileInfo(fileName);
            XmlSerializer serializer = new XmlSerializer(objType);

            Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            fs.Position = 0;
            XmlReader reader = XmlReader.Create(fs);

            object retObj = null;
            try
            {
                retObj = serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                return null;
            }

            fs.Close();

            return retObj;
        }



        public static string ReadTextFromFile(string fileName)
        {
            Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string pDataString = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return pDataString;
        }




        public static void ExecMethodInSeparateThread(Form owner, MethodInvoker method)
        {
            owner.Invoke(method);
        }




        public static object ReadDataFromCSV(string filePath, int titleRows = 0, string valuesSeparator = ";", string linesSeparator = "\n")
        {
            string dataString = ServiceTools2G.ReadTextFromFile(filePath);
            List<string> dataSubstrings = new List<string>(dataString.Split(linesSeparator.ToCharArray()));

            if (titleRows > 0)
            {
                dataSubstrings = dataSubstrings.Skip(titleRows).ToList();
            }

            dataSubstrings.RemoveAll(str => str == "");
            string[] values0 = dataSubstrings[0].Split(valuesSeparator.ToCharArray());
            if (values0.Count() == 1)
            {
                DenseVector retVec = DenseVector.Create(dataSubstrings.Count(),
                    idx => Convert.ToDouble(dataSubstrings[idx].Replace(".", ",")));
                return retVec;
            }
            else
            {
                DenseMatrix retMatrix = DenseMatrix.Create(dataSubstrings.Count(), values0.Count(),
                    (r, c) =>
                        Convert.ToDouble(dataSubstrings[r].Split(valuesSeparator.ToCharArray())[c].Replace(".", ",")));
                //foreach (string substring in dataSubstrings)
                //{
                //    List<string> currRowStrings = new List<string>(substring.Split(valuesSeparator.ToCharArray()));
                //    retMatrix.InsertRow(retMatrix.RowCount,
                //        DenseVector.OfEnumerable(
                //            currRowStrings.ConvertAll<double>(str => Convert.ToDouble(str.Replace(".", ",")))));
                //}
                return retMatrix;
            }
        }





        public static List<List<string>> ReadDataFromCSV(string filePath, int titleRows = 0, bool nonNumberData = true, string valuesSeparator = ";", string linesSeparator = "\n")
        {
            string dataString = ServiceTools2G.ReadTextFromFile(filePath);
            List<string> dataSubstrings = new List<string>(dataString.Split(linesSeparator.ToCharArray()));

            if (titleRows > 0)
            {
                dataSubstrings = dataSubstrings.Skip(titleRows).ToList();
            }

            dataSubstrings.RemoveAll(str => str == "");

            List<List<string>> output =
                dataSubstrings.ConvertAll<List<string>>(
                    str => new List<string>(str.Split(valuesSeparator.ToCharArray())));

            return output;
        }







        public static string ErrorTextDescription(CompilerError error)
        {
            string strErrDescription = "[" + error.ErrorNumber + "]" + Environment.NewLine;
            strErrDescription += "file: " + error.FileName + Environment.NewLine;
            strErrDescription += "line:column: " + error.Line + " : " + error.Column + Environment.NewLine;
            strErrDescription += "message: " + error.ErrorText + Environment.NewLine;

            return strErrDescription;
        }




        public static Assembly CompileAssemblyFromExternalCodeSource(string textFilePath, out CompilerResults results)
        {
            string codeSource = ReadTextFromFile(textFilePath);
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Dictionary<string, string> providerOptions = new Dictionary<string, string>
            {
                    {"CompilerVersion", "v4.0"}
                };
            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);
            CompilerParameters compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };
            //compilerParams.CompilerOptions = "/lib:" + Directory.GetCurrentDirectory() + "/libs/";

            compilerParams.ReferencedAssemblies.Add(executingAssembly.Location);
            foreach (AssemblyName assemblyName in executingAssembly.GetReferencedAssemblies())
            {
                compilerParams.ReferencedAssemblies.Add(Assembly.Load(assemblyName).Location);
            }
            results = provider.CompileAssemblyFromSource(compilerParams, codeSource);
            if (results.Errors.Count != 0)
            {
                //theLogWindow = ServiceTools.LogAText(theLogWindow, "Function compilation failed!");
                //foreach (CompilerError error in results.Errors)
                //{
                //    theLogWindow = ServiceTools.LogAText(theLogWindow, ServiceTools.ErrorTextDescription(error));
                //}

                //throw new Exception("failed to compile external source code")
                return null;
            }

            return results.CompiledAssembly;
        }




        

        public enum DatetimeExtractionMethod
        {
            Filename,
            Content,
            FileCreation
        }

        

        public static List<string> GetPropertiesNamesOfClass(object pObject)
        {
            List<string> propertyList = new List<string>();
            if (pObject != null)
            {
                foreach (var prop in pObject.GetType().GetProperties())
                {
                    propertyList.Add(prop.Name);
                }
            }
            return propertyList;
        }



        public static byte[] CalculateMD5hash(string filename)
        {
            byte[] fileMD5hash;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    fileMD5hash = md5.ComputeHash(stream);
                }
            }
            return fileMD5hash;
        }




        public static string CalculateMD5hashString(string filename)
        {
            return Encoding.ASCII.GetString(CalculateMD5hash(filename));
        }
    }
}
