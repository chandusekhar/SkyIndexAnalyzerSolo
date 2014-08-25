using System;
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
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using System.Data;

namespace SkyIndexAnalyzerLibraries
{
    public class ServiceTools
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr handle, int minimumWorkingSetSize, int maximumWorkingSetSize);



        public static object getPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }


        public static void FlushMemory(System.Windows.Forms.TextBox tb = null, string mark = "")
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            //}
        }


        delegate void ShowPictureDelegate(object[] parameters);

        public static void ShowPicture(Bitmap bitmap2show, string pictureTitle = "")
        {
            SimpleShowImageForm SimpleShowImageForm1 = new SimpleShowImageForm(bitmap2show, pictureTitle);
            SimpleShowImageForm1.Show();
        }



        public static void ShowPicture(Image image2show, string pictureTitle = "")
        {
            SimpleShowImageForm SimpleShowImageForm1 = new SimpleShowImageForm(image2show, pictureTitle);
            SimpleShowImageForm1.Show();
        }




        public static void ShowPicture(Image image2show, string pictureTitle = "", int timeout = 5000)
        {
            BackgroundWorker bgwShowPicture = new BackgroundWorker();
            bgwShowPicture.WorkerSupportsCancellation = false;
            bgwShowPicture.DoWork += bgwShowPicture_DoWork;
            bgwShowPicture.RunWorkerCompleted += bgwShowPicture_RunWorkerCompleted;

            object[] workerArgs = new object[] { image2show, pictureTitle, timeout };

            bgwShowPicture.RunWorkerAsync(workerArgs);
        }



        public static void ShowPicture(Image<Gray, Byte> imgToShow, string pictureTitle = "")
        {
            SimpleShowImageForm SimpleShowImageForm1 = new SimpleShowImageForm(imgToShow, pictureTitle);
            SimpleShowImageForm1.Show();
        }



        public static void ShowPicture(Image<Gray, Byte> imgToShow, string pictureTitle = "", int timeout = 5000)
        {
            BackgroundWorker bgwShowPicture = new BackgroundWorker();
            bgwShowPicture.WorkerSupportsCancellation = false;
            bgwShowPicture.DoWork += bgwShowPicture_DoWork;
            bgwShowPicture.RunWorkerCompleted += bgwShowPicture_RunWorkerCompleted;

            object[] workerArgs = new object[] { imgToShow, pictureTitle, timeout };

            bgwShowPicture.RunWorkerAsync(workerArgs);
        }




        public static void ShowPicture(Image<Bgr, Byte> imgToShow, string pictureTitle = "")
        {

            SimpleShowImageForm SimpleShowImageForm1 = new SimpleShowImageForm(imgToShow, pictureTitle);
            SimpleShowImageForm1.Show();
        }




        public static void ShowPicture(Image<Bgr, Byte> imgToShow, string pictureTitle = "", int timeout = 5000)
        {
            BackgroundWorker bgwShowPicture = new BackgroundWorker();
            bgwShowPicture.WorkerSupportsCancellation = false;
            bgwShowPicture.DoWork += bgwShowPicture_DoWork;
            bgwShowPicture.RunWorkerCompleted += bgwShowPicture_RunWorkerCompleted;

            object[] workerArgs = new object[] { imgToShow, pictureTitle, timeout };

            bgwShowPicture.RunWorkerAsync(workerArgs);
        }

        static void bgwShowPicture_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SimpleShowImageForm SimpleShowImageForm1 = (e.Result as object[])[0] as SimpleShowImageForm;
            SimpleShowImageForm1.Close();
            SimpleShowImageForm1.Dispose();
        }



        static void bgwShowPicture_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];
            string pictureTitle = args[1] as string;
            object img = args[0];
            int timeout = Convert.ToInt32(args[2]);
            SimpleShowImageForm SimpleShowImageForm1 = new SimpleShowImageForm();

            if (img.GetType() == typeof(Image<Bgr, Byte>))
            {
                Image<Bgr, Byte> imgToShow = img as Image<Bgr, Byte>;
                SimpleShowImageForm1 = new SimpleShowImageForm(imgToShow, pictureTitle);
                SimpleShowImageForm1.Show();

                Application.DoEvents();

                Thread.Sleep(timeout);
                //SimpleShowImageForm1.Close();
                //SimpleShowImageForm1.Dispose();
            }
            else if (img.GetType() == typeof(Image<Gray, Byte>))
            {
                Image<Gray, Byte> imgToShow = img as Image<Gray, Byte>;
                SimpleShowImageForm1 = new SimpleShowImageForm(imgToShow, pictureTitle);
                SimpleShowImageForm1.Show();

                Application.DoEvents();

                Thread.Sleep(timeout);
                //SimpleShowImageForm1.Close();
                //SimpleShowImageForm1.Dispose();
            }
            else if (img.GetType() == typeof(Image))
            {
                Image imgToShow = img as Image;
                SimpleShowImageForm1 = new SimpleShowImageForm(imgToShow, pictureTitle);
                SimpleShowImageForm1.Show();

                Application.DoEvents();

                Thread.Sleep(timeout);
                //SimpleShowImageForm1.Close();
                //SimpleShowImageForm1.Dispose();
            }

            e.Result = new object[] { SimpleShowImageForm1 };

        }




        public static double[, ,] AddedThirdIndexArray3DFrom2D(double[,] inputArray)
        {
            double[, ,] result = new double[inputArray.GetLength(0), inputArray.GetLength(1), 1];

            for (int i = 0; i < inputArray.GetLength(0); i++)
            {
                for (int j = 0; j < inputArray.GetLength(1); j++)
                {
                    result[i, j, 0] = inputArray[i, j];
                }
            }

            return result;
        }


        public static Byte[, ,] AddedThirdIndexArray3DFrom2D(Byte[,] inputArray)
        {
            Byte[, ,] result = new Byte[inputArray.GetLength(0), inputArray.GetLength(1), 1];

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
                    if (double.IsNaN(x)) x = 0.0;
                    if (x <= 0.0) x = 0.0;
                    out_arr[i, j] = (byte)Math.Round(x, 0);
                }

            return out_arr;
        }




        public static LogWindow LogAText(LogWindow logWindow = null, string theText = "", bool appendMode = true)
        {
            LogWindow theWindow = logWindow;
            if ((theWindow == null) || (theWindow.IsDisposed))
            {
                theWindow = new LogWindow();
                theWindow.Show();
            }

            Type theType = theWindow.GetType();
            MethodInfo methodInfo = theType.GetMethod("LogAText");
            object[] parametersArray = new object[] { theText, appendMode };
            methodInfo.Invoke(theWindow, parametersArray);

            return theWindow;
        }



        public static HistogramCalcAndShowForm RepresentHistogrammedStatsOfDataMarix(DenseMatrix dmToRepresent, string description = "", bool showTheWindow = true)
        {
            DenseVector dvDataToHist = DataAnalysis.DataVectorizedWithCondition(dmToRepresent, dVal => dVal > 0.0d);
            HistogramDataAndProperties currenthistData = new HistogramDataAndProperties(dvDataToHist, 100);
            currenthistData.color = Color.Red;
            HistogramCalcAndShowForm histForm = new HistogramCalcAndShowForm(description);
            histForm.HistToRepresent = currenthistData;
            if (showTheWindow) histForm.Show();
            histForm.Represent();

            return histForm;
        }





        public static HistogramCalcAndShowForm RepresentHistogrammedStatsOfDataVector(DenseVector dvToRepresent, string description = "", bool showTheWindow = true)
        {
            DenseVector dvDataToHist = DataAnalysis.DataVectorizedWithCondition(dvToRepresent, dVal => dVal > 0.0d);
            HistogramDataAndProperties currenthistData = new HistogramDataAndProperties(dvDataToHist, 100);
            currenthistData.color = Color.Red;
            HistogramCalcAndShowForm histForm = new HistogramCalcAndShowForm(description);
            histForm.HistToRepresent = currenthistData;
            if (showTheWindow) histForm.Show();
            histForm.Represent();

            return histForm;
        }





        public static ImageConditionAndDataRepresentingForm RepresentDataFromDenseMatrix(object dmToRepresent, string dataName = "", bool withFixedScaleMargins = false, bool withSymmetricColorScheme = false, double minScaleValue = 0.0d, double maxScaleValue = 1.0d, bool showTheWindow = true)
        {
            imageConditionAndData dataRepresentingImgData = new imageConditionAndData(dmToRepresent, null);
            if (withSymmetricColorScheme)
            {
                dataRepresentingImgData.currentColorScheme = new ColorScheme("", true);
            }
            else
            {
                dataRepresentingImgData.currentColorScheme = new ColorScheme("");
            }
            dataRepresentingImgData.currentColorSchemeRuler = new ColorSchemeRuler(dataRepresentingImgData);
            if (!withFixedScaleMargins)
            {
                dataRepresentingImgData.currentColorSchemeRuler.IsMarginsFixed = false;
                dataRepresentingImgData.UpdateColorSchemeRuler();
            }
            else
            {
                dataRepresentingImgData.currentColorSchemeRuler.IsMarginsFixed = true;
                dataRepresentingImgData.currentColorSchemeRuler.minValue = minScaleValue;
                dataRepresentingImgData.currentColorSchemeRuler.maxValue = maxScaleValue;
                dataRepresentingImgData.UpdateColorSchemeRuler();
            }
            ImageConditionAndDataRepresentingForm dmTestReversedRepresentingForm = new ImageConditionAndDataRepresentingForm(dataRepresentingImgData, dataName);
            if (showTheWindow) dmTestReversedRepresentingForm.Show();
            return dmTestReversedRepresentingForm;
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



        public static void logToTextFile(string Filename, string text = "", bool append = false, bool cp1251 = false)
        {
            FileStream textFileStream = null;
            StreamWriter sw;

            FileInfo finfo = new FileInfo(Filename);
            if (!Directory.Exists(finfo.DirectoryName))
            {
                Directory.CreateDirectory(finfo.DirectoryName);
            }


            textFileStream = new FileStream(Filename, (append) ? (FileMode.Append) : (FileMode.Create), FileAccess.Write);

            byte[] info2write = new UTF8Encoding(true).GetBytes(text);

            textFileStream.Write(info2write, 0, info2write.Length);
            textFileStream.Close();
        }






        public static int CurrentCodeLineNumber(
            [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0)
        {
            return callingFileLineNumber;
        }



        public static void ReportProgressByIndicator(MultipleProgressIndicatingForm theForm, ProgressVisualizationStruct theIndicator, int codeLineNumber, int methodLinesNumber)
        {
            theForm.UpdateIndicator(theIndicator, (double)codeLineNumber / (double)methodLinesNumber);
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

            var rowEnum = dm.RowEnumerator();
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

            var rowEnum = dm.RowEnumerator();
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


        public static string WriteDictionaryToXml(Dictionary<string, object> dictToWrite, string filename, bool append = false)
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
                catch (Exception)
                {
                    return false;
                    throw;
                }

            }
            return true;
        }


    }
}
