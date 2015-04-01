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
                xlApp.Visible = false;
                wb = xlApp.Workbooks.Open(fName, Type.Missing, false);
                ws = wb.Sheets[1];
            }
            catch (Exception)
            {
                CloseAll();
            }
            

            List<Tuple<int, SunElevationTestDataRecord>> lRecords = new List<Tuple<int, SunElevationTestDataRecord>>();
            int rowIdx = 2;
            while (true)
            {
                DateTime dtVal;
                try
                {
                    Range rngDT = ws.Cells[rowIdx, 1];
                    double d = (double)(rngDT.Value2);
                    if (d == 0.0d)
                    {
                        break;
                    }
                    dtVal = DateTime.FromOADate(d);
                }
                catch (Exception)
                {
                    CloseAll();
                    break;
                }
                


                double latVal;
                try
                {
                    Range rngLat = ws.Cells[rowIdx, 2];
                    var rngLatVal = rngLat.Value2;
                    if (rngLatVal == null)
                    {
                        rowIdx++;
                        continue;
                    }
                    latVal = (double)(rngLatVal);
                }
                catch (Exception)
                {
                    CloseAll();
                    break;
                }
                

                double lonVal;
                try
                {
                    Range rngLon = ws.Cells[rowIdx, 3];
                    var rngLonVal = rngLon.Value2;
                    if (rngLonVal == null)
                    {
                        rowIdx++;
                        continue;
                    }
                    lonVal = (double)(rngLonVal);
                }
                catch (Exception)
                {
                    CloseAll();
                    break;
                }
                


                double sunAltTestVal;
                try
                {
                    Range rngSunAltTest = ws.Cells[rowIdx, 4];
                    var rngSunAltTestVal = rngSunAltTest.Value2;
                    if (rngSunAltTestVal == null)
                    {
                        rowIdx++;
                        continue;
                    }
                    sunAltTestVal = (double)(rngSunAltTestVal);
                }
                catch (Exception)
                {
                    CloseAll();
                    break;
                }
                


                SunElevationTestDataRecord currRec = new SunElevationTestDataRecord()
                {
                    dt = dtVal,
                    LatDec = latVal,
                    LonDec = lonVal,
                    sunElevTest = sunAltTestVal
                };

                ThreadSafeOperations.SetText(lblLoadingStatus, "loading " + rowIdx + " row", false);
                System.Windows.Forms.Application.DoEvents();

                lRecords.Add(new Tuple<int, SunElevationTestDataRecord>(rowIdx, currRec));

                rowIdx++;

                //if (rowIdx >= 100)
                //{
                //    break;
                //}
            }

            List<double> lDeviations = new List<double>();
            string outFName = Path.GetDirectoryName(fName) + "\\" + Path.GetFileNameWithoutExtension(fName) + "-output.txt";

            foreach (Tuple<int, SunElevationTestDataRecord> tpl in lRecords)
            {
                rowIdx = tpl.Item1;
                SunElevationTestDataRecord currRec = tpl.Item2;

                SPA spaCalc = new SPA(currRec.dt.Year, currRec.dt.Month, currRec.dt.Day, currRec.dt.Hour,
                    currRec.dt.Minute, currRec.dt.Second, (float)currRec.LonDec, (float)currRec.LatDec,
                    (float)SPAConst.DeltaT(currRec.dt));
                int res = spaCalc.spa_calculate();
                AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                    spaCalc.spa.zenith);
                currRec.sunElevCalc = sunPositionSPAext.ElevationAngle;

                lDeviations.Add(currRec.sunElevCalc - currRec.sunElevTest);

                ServiceTools.logToTextFile(outFName, currRec.ToString() + Environment.NewLine, true);
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



    internal struct SunElevationTestDataRecord
    {
        public DateTime dt;
        public double LatDec;
        public double LonDec;
        public double sunElevTest;
        public double sunElevCalc;


        public override string ToString()
        {
            string str = dt.ToString("s") + ";";
            str += LatDec.ToString() + ";";
            str += LonDec.ToString() + ";";
            str += sunElevTest.ToString() + ";";
            str += sunElevCalc.ToString() + ";";
            return str;
        }
    }
}
