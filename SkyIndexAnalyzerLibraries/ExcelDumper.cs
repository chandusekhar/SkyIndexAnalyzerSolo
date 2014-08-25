using System;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace SkyIndexAnalyzerLibraries
{
    public class ExcelDumper : IDisposable
    {
        public double[,] doubleArrayToDump;
        public string filename;
        public string directoryName;
        public int currentWorksheet;
        private Microsoft.Office.Interop.Excel.Application xlApp;
        private Workbook wb;

        public ExcelDumper(string imageFileName)
        {
            FileInfo imagefinfo = new FileInfo(imageFileName);
            directoryName = imagefinfo.DirectoryName + "\\";
            filename = imagefinfo.Name + ".data.xlsx";
            doubleArrayToDump = null;
            currentWorksheet = 1;
            if (xlApp == null)
                xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null) throw new NotImplementedException();
            xlApp.Visible = false;
            wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            if (wb == null) throw new NotImplementedException();
        }



        private bool CheckDumpingAvailability()
        {
            if (filename == "") return false;
            if (doubleArrayToDump == null) return false;
            if (doubleArrayToDump.Length == 0) return false;
            if (xlApp == null) return false;
            if (wb == null) return false;
            return true;
        }



        public bool DumpData(string nameOfData = "SIvalues")
        {
            if (!CheckDumpingAvailability())
                return false;
            
            Worksheet ws = wb.Worksheets.Add();
            ws.Name = nameOfData;
            if (ws == null)
            {
                return false;
            }
            currentWorksheet++;

            //for (int i = 0; i < doubleArrayToDump.GetLength(0); i++)
            //    for (int j = 0; j < doubleArrayToDump.GetLength(1); j++)
            //    {
            //        Range cell = ws.Cells[i+1][j+1];
            //        cell.Value = doubleArrayToDump[i,j];
            //    }
            Range rng = ws.Cells.get_Resize(doubleArrayToDump.GetLength(0), doubleArrayToDump.GetLength(1));
            rng.Value2 = doubleArrayToDump;

            return true;
        }



        public void Close()
        {
            wb.SaveAs("" + directoryName + filename, XlFileFormat.xlWorkbookDefault);
            wb.Close();

            Dispose();
        }


        
        public void Dispose()
        {
            wb = null;
            xlApp = null;
            doubleArrayToDump = null;
            filename = "";
            directoryName = "";
            currentWorksheet = 0;

            //throw new NotImplementedException();
        }
    }
}
