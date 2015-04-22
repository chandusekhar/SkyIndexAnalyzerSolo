using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using SkyImagesAnalyzerLibraries;
using Microsoft.Office.Interop.Excel;
using SolarPositioning;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;


namespace SkyImagesAnalyzer
{
    /// <summary>
    /// Class SunElevationTest.
    /// В этой форме осуществляется тестирование данных склонения солнца, записанных в сводном xls-файле протоколов
    /// метеонаблюдений рейсов.
    /// Результирующий файл представляет собой csv-ACII файл, расположенный рядом с файлом xls
    /// </summary>
    public partial class SunElevationTest : Form
    {
        private LogWindow theLogWindow = null;
        private Workbook wb;
        private Worksheet ws;
        private Microsoft.Office.Interop.Excel.Application xlApp;
        public Dictionary<string, object> defaultProperties = null;


        public SunElevationTest()
        {
            InitializeComponent();
        }

        private void btnProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }






        private void btnProcess_Click(object sender, EventArgs e)
        {
            string fName = richTextBox1.Text;
            if (!File.Exists(fName))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "couldn`t find a file: " + fName);
                return;
            }

            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.Visible = true;
                wb = xlApp.Workbooks.Open(fName, Type.Missing, false);
                ws = wb.Sheets[1];
            }
            catch (Exception)
            {
                CloseAll();
            }
            

            List<Tuple<int, SunElevationTestDataRecord>> lRecords = new List<Tuple<int, SunElevationTestDataRecord>>();
            int rowIdx = 2;
            double dDataTimeRead = 0.0d;
            while (true)
            {
                DateTime dtVal;
                SunElevationTestDataRecord currRec = new SunElevationTestDataRecord();

                System.Windows.Forms.Application.DoEvents();

                try
                {
                    Range rngDT = ws.Cells[rowIdx, 1];
                    var dDataTimeReadValue = rngDT.Value2;
                    if (dDataTimeReadValue == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "date-time value at row " + rowIdx + " is empty. Stopping reading.");
                        CloseAll();
                        break;
                    }
                    dDataTimeRead = (double)(dDataTimeReadValue);
                    if (dDataTimeRead == 0.0d)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "date-time value at row " + rowIdx + " is empty. Stopping reading.");
                        CloseAll();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //CloseAll();
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "=====================");
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        ex.Message + Environment.NewLine + "for date-time value at row " + rowIdx);
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "=====================");
                    lRecords.Add(new Tuple<int, SunElevationTestDataRecord>(rowIdx, currRec));
                    rowIdx++;
                    continue;
                }
                dtVal = DateTime.FromOADate(dDataTimeRead);
                currRec.Dt = dtVal;



                double latVal = 0.0d;
                try
                {
                    Range rngLat = ws.Cells[rowIdx, 2];
                    var rngLatVal = rngLat.Value2;
                    if (rngLatVal == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "latitude value at row " + rowIdx + " is empty");
                        //rowIdx++;
                        //continue;
                    }
                    else
                    {
                        latVal = (double)(rngLatVal);
                        currRec.latDec = latVal;
                    }
                }
                catch (Exception ex)
                {
                    //CloseAll();
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "=====================");
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        ex.Message + Environment.NewLine + "for latitude value at row " + rowIdx);
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "=====================");
                    lRecords.Add(new Tuple<int, SunElevationTestDataRecord>(rowIdx, currRec));
                    rowIdx++;
                    continue;
                }
                
                

                double lonVal = 0.0d;
                try
                {
                    Range rngLon = ws.Cells[rowIdx, 3];
                    var rngLonVal = rngLon.Value2;
                    if (rngLonVal == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "longitude value at row " + rowIdx + " is empty");
                        //rowIdx++;
                        //continue;
                    }
                    else
                    {
                        lonVal = (double)(rngLonVal);
                        currRec.lonDec = lonVal;
                    }
                }
                catch (Exception ex)
                {
                    //CloseAll();
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "=====================");
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        ex.Message + Environment.NewLine + "for longitude value at row " + rowIdx);
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "=====================");
                    lRecords.Add(new Tuple<int, SunElevationTestDataRecord>(rowIdx, currRec));
                    rowIdx++;
                    continue;
                }
                



                double sunAltTestVal = 0.0d;
                try
                {
                    Range rngSunAltTest = ws.Cells[rowIdx, 4];
                    var rngSunAltTestVal = rngSunAltTest.Value2;
                    if (rngSunAltTestVal == null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "testing sun elevation value at row " + rowIdx + " is empty");
                        //rowIdx++;
                        //continue;
                    }
                    else
                    {
                        sunAltTestVal = (double)(rngSunAltTestVal);
                        currRec.SunElevTest = sunAltTestVal;
                    }
                }
                catch (Exception ex)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "=====================");
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        ex.Message + Environment.NewLine + "for read sun elevation value at row " + rowIdx);
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "=====================");
                    //CloseAll();
                    lRecords.Add(new Tuple<int, SunElevationTestDataRecord>(rowIdx, currRec));
                    rowIdx++;
                    continue;
                }
                
                

                ThreadSafeOperations.SetText(lblLoadingStatus, "loading " + rowIdx + " row", false);

                lRecords.Add(new Tuple<int, SunElevationTestDataRecord>(rowIdx, currRec));

                rowIdx++;
            }



            List<double> lDeviations = new List<double>();
            string outFName = Path.GetDirectoryName(fName) + "\\" + Path.GetFileNameWithoutExtension(fName) + "-output.txt";

            ServiceTools.logToTextFile(outFName, SunElevationTestDataRecord.TableFieldsHeader() + Environment.NewLine, true);

            foreach (Tuple<int, SunElevationTestDataRecord> tpl in lRecords)
            {
                rowIdx = tpl.Item1;
                SunElevationTestDataRecord currRec = tpl.Item2;

                if (currRec.WhetherAllValuesHasBeenRead)
                {
                    SPA spaCalc = new SPA(currRec.Dt.Year, currRec.Dt.Month, currRec.Dt.Day, currRec.Dt.Hour,
                        currRec.Dt.Minute, currRec.Dt.Second, (float) currRec.lonDec, (float) currRec.latDec,
                        (float) SPAConst.DeltaT(currRec.Dt));
                    int res = spaCalc.spa_calculate();
                    AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                        spaCalc.spa.zenith);
                    currRec.SunElevCalc = sunPositionSPAext.ElevationAngle;

                    lDeviations.Add(currRec.SunElevCalc - currRec.SunElevTest);

                    ServiceTools.logToTextFile(outFName, currRec.ToString() + Environment.NewLine, true);
                }
                else
                {
                    ServiceTools.logToTextFile(outFName, currRec.ToString() + Environment.NewLine, true);
                }

                
            }

            
            HistogramDataAndProperties histData = new HistogramDataAndProperties(DenseVector.OfEnumerable(lDeviations), 20);
            HistogramCalcAndShowForm histForm = new HistogramCalcAndShowForm("", defaultProperties);
            histForm.HistToRepresent = histData;
            histForm.Show();
            histForm.Represent();


            CloseAll();
        }





        private void CloseAll()
        {
            if (wb != null)
            {
                wb.Close();
                wb = null;
            }
            if (xlApp != null)
            {
                xlApp.Quit();
                xlApp = null;
            }
        }


    }



    internal class SunElevationTestDataRecord
    {
        private DateTime dt;
        private bool dtHasBeenSet = false;
        private double LatDec;
        private bool LatDecHasBeenSet = false;
        private double LonDec;
        private bool LonDecHasBeenSet = false;
        private double sunElevTest;
        private bool sunElevTestHasBeenSet = false;
        private double sunElevCalc;
        private bool sunElevCalcHasBeenSet = false;




        public bool WhetherAllValuesHasBeenRead
        {
            get
            {
                return dtHasBeenSet && LatDecHasBeenSet && LonDecHasBeenSet && sunElevTestHasBeenSet;
            }
        }



        public DateTime Dt
        {
            get { return dt; }
            set
            {
                dt = value;
                if ((dt.Minute == 59) && (dt.Second == 59))
                {
                    dt = dt.AddSeconds(1.0d);
                }
                dtHasBeenSet = true;
            }
        }



        public double latDec
        {
            get { return LatDec; }
            set
            {
                LatDec = value;
                LatDecHasBeenSet = true;
            }
        }



        public double lonDec
        {
            get { return LonDec; }
            set
            {
                LonDec = value;
                LonDecHasBeenSet = true;
            }
        }



        public double SunElevTest
        {
            get { return sunElevTest; }
            set
            {
                sunElevTest = value;
                sunElevTestHasBeenSet = true;
            }
        }



        public double SunElevCalc
        {
            get { return sunElevCalc; }
            set
            {
                sunElevCalc = value;
                sunElevCalcHasBeenSet = true;
            }
        }



        public override string ToString()
        {
            string str = ((dtHasBeenSet) ? (dt.ToString("s")) : ("")) + ";";
            str += ((LatDecHasBeenSet) ? (LatDec.ToString()) : ("")) + ";";
            str += ((LonDecHasBeenSet)?(LonDec.ToString()):("")) + ";";
            str += ((sunElevTestHasBeenSet)?(sunElevTest.ToString()):("")) + ";";
            str += ((sunElevCalcHasBeenSet)?(sunElevCalc.ToString()):("")) + ";";
            return str;
        }



        public static string TableFieldsHeader()
        {
            string str = "Date,time;";
            str += "lat(deg);";
            str += "lon(deg);";
            str += "testing sun elevation (deg);";
            str += "calculated sun elevation (deg);";
            return str;
        }
    }
}
