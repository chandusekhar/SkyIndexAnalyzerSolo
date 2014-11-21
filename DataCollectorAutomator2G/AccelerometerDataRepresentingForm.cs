using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyIndexAnalyzerLibraries;

namespace DataCollectorAutomator
{

    public partial class AccelerometerDataRepresentingForm : Form
    {
        public SensorsHistoryShowing sensorTypeToShow = SensorsHistoryShowing.Accelerometer;
        public string strLogFilename = "";
        private MultipleScatterAndFunctionsRepresentation theRepresentationClassObject;
        private int sensorsHistoryRepresentingScale = 30;
        private MultipleScatterAndFunctionsRepresentation accDeviationAngleRenderer;
        private MultipleScatterAndFunctionsRepresentation accDeviationMagnitudeRenderer;
        private MultipleScatterAndFunctionsRepresentation accDeviationDirectionRenderer;
        private SimpleVectorGraphics2D accVectorsSideViewRenderer;
        private SimpleVectorGraphics2D accVectorsUpperViewRenderer;
        private DenseMatrix dmAccData = null;

        //public FunctionRepresentationForm(FunctionDefinitionTypes theDefinitionType = FunctionDefinitionTypes.analytic, string description = "")
        public AccelerometerDataRepresentingForm(string logFileName, SensorsHistoryShowing sensorType = SensorsHistoryShowing.Accelerometer)
        {
            InitializeComponent();
            if (logFileName == "")
            {
                return;
            }
            if (!File.Exists(logFileName))
            {
                var result = MessageBox.Show("Cant find actual log data file: " + logFileName, "Cant find a file",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            strLogFilename = logFileName;
            sensorTypeToShow = sensorType;

        }





        public AccelerometerDataRepresentingForm()
        {
            InitializeComponent();
            
            strLogFilename = "";
            sensorTypeToShow = SensorsHistoryShowing.Accelerometer;

        }









        private void FunctionRepresentationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }


        private void Represent()
        {
            Dictionary<string, object> dictDataToShow = NetCDFoperations.ReadDataFromFile(strLogFilename);

            long[] arrDateTimeTicksValues = new long[] {0};
            if (dictDataToShow.Keys.Contains("DateTime"))
            {
                arrDateTimeTicksValues = (long[])(dictDataToShow["DateTime"]);
            }
            else if (dictDataToShow.Keys.Contains("Datetime"))
            {
                arrDateTimeTicksValues = (long[]) (dictDataToShow["Datetime"]);
            }
            else
            {
                MessageBox.Show("Couldn`t acces the DateTime field of the file " + strLogFilename, "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            DenseVector dvSecondsVector = DenseVector.Create(arrDateTimeTicksValues.Length, i =>
            {
                DateTime dateTime1 = new DateTime(arrDateTimeTicksValues[i]);
                TimeSpan dt = dateTime1 - dateTime1.Date;
                return dt.TotalSeconds;
            });

            dmAccData = (DenseMatrix)dictDataToShow["AccelerometerData"];
            
            if (sensorsHistoryRepresentingScale < 86400)
            {
                double lastDateTimeSecondsValue = dvSecondsVector[dvSecondsVector.Count-1];

                int searchingIndex = dvSecondsVector.Count - 1;
                for (int idx = dvSecondsVector.Count - 1; idx >= 0; idx--)
                {
                    if (lastDateTimeSecondsValue - dvSecondsVector[idx] >= sensorsHistoryRepresentingScale)
                    {
                        searchingIndex = idx;
                        break;
                    }
                }
                dvSecondsVector =
                    (DenseVector) dvSecondsVector.SubVector(searchingIndex, dvSecondsVector.Count - searchingIndex);

                dmAccData =
                    (DenseMatrix) dmAccData.SubMatrix(searchingIndex, dvSecondsVector.Count, 0, dmAccData.ColumnCount);
            }





            #region filter input data noise

            List<DenseVector> dmAccDataFiltered = new List<DenseVector>();
            Dictionary<string, DenseVector> dictCurrColumnFilteredData = new Dictionary<string, DenseVector>();

            for (int col = 0; col < dmAccData.ColumnCount; col++)
            {
                DenseVector dvVectToFilter = (DenseVector)dmAccData.Column(col);
                dictCurrColumnFilteredData = DataAnalysis.SavGolFilter(dvVectToFilter, dvSecondsVector, 6, 6, 0, 6);
                dmAccDataFiltered.Add(dictCurrColumnFilteredData["values"]);
                //dmAccData.SetColumn(col, dictCurrColumnFilteredData["values"]);
                //dvSecondsVector = dictCurrColumnFilteredData["time"];
            }

            dvSecondsVector = dictCurrColumnFilteredData["time"];
            dmAccData = (DenseMatrix)DenseMatrix.OfColumns(dvSecondsVector.Count, dmAccData.ColumnCount, dmAccDataFiltered);

            #endregion filter input data noise




            // надо рассчитать углы отклонения отдельно - по сглаженным данным и по данным гироскопа
            DenseVector dvDataToShowDevAngleValue = (DenseVector)dmAccData.Column(6);

            if (cbFilterData.Checked)
            {
                List<double> ac = new List<double>();
            }

            if (accDeviationAngleRenderer == null)
            {
                accDeviationAngleRenderer = new MultipleScatterAndFunctionsRepresentation(pbRepresentingDevAngle.Size);
                accDeviationAngleRenderer.dvScatterXSpace.Add(dvSecondsVector);
                accDeviationAngleRenderer.dvScatterFuncValues.Add(dvDataToShowDevAngleValue);
                accDeviationAngleRenderer.scatterLineColors.Add(new Bgr(Color.Blue));
                accDeviationAngleRenderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);
            }
            else
            {
                if (accDeviationAngleRenderer.xSpaceMin != dvSecondsVector.Max() - sensorsHistoryRepresentingScale)
                {
                    accDeviationAngleRenderer.xSpaceMin = dvSecondsVector.Max() - sensorsHistoryRepresentingScale;

                    accDeviationAngleRenderer.dvScatterXSpace.Clear();
                    accDeviationAngleRenderer.dvScatterFuncValues.Clear();
                    accDeviationAngleRenderer.scatterLineColors.Clear();
                    accDeviationAngleRenderer.scatterDrawingVariants.Clear();

                    accDeviationAngleRenderer.dvScatterXSpace.Add(dvSecondsVector);
                    accDeviationAngleRenderer.dvScatterFuncValues.Add(dvDataToShowDevAngleValue);
                    accDeviationAngleRenderer.scatterLineColors.Add(new Bgr(Color.Blue));
                    accDeviationAngleRenderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);
                }
            }

            accDeviationAngleRenderer.xSpaceMin = dvSecondsVector.Max() - sensorsHistoryRepresentingScale;


            accDeviationAngleRenderer.Represent();
            ThreadSafeOperations.UpdatePictureBox(pbRepresentingDevAngle, accDeviationAngleRenderer.TheImage.Bitmap);


            DenseVector dvDataToShowAccMagnitudeDev = DenseVector.Create(dmAccData.RowCount, i =>
            {
                accelerometerData currAccData = new accelerometerData(dmAccData[i, 0], dmAccData[i, 1], dmAccData[i, 2]);
                accelerometerData calibrationAccData = new accelerometerData(dmAccData[i, 3], dmAccData[i, 4], dmAccData[i, 5]);

                return currAccData.AccMagnitude - calibrationAccData.AccMagnitude;
            });
            if (accDeviationMagnitudeRenderer == null)
            {
                accDeviationMagnitudeRenderer = new MultipleScatterAndFunctionsRepresentation(pbRepresentingDevMagnitude.Size);
                accDeviationMagnitudeRenderer.dvScatterXSpace.Add(dvSecondsVector);
                accDeviationMagnitudeRenderer.dvScatterFuncValues.Add(dvDataToShowAccMagnitudeDev);
                accDeviationMagnitudeRenderer.scatterLineColors.Add(new Bgr(Color.Blue));
                accDeviationMagnitudeRenderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);
            }
            else
            {
                if (accDeviationMagnitudeRenderer.xSpaceMin != dvSecondsVector.Max() - sensorsHistoryRepresentingScale)
                {
                    accDeviationMagnitudeRenderer.xSpaceMin = dvSecondsVector.Max() - sensorsHistoryRepresentingScale;

                    accDeviationMagnitudeRenderer.dvScatterXSpace.Clear();
                    accDeviationMagnitudeRenderer.dvScatterFuncValues.Clear();
                    accDeviationMagnitudeRenderer.scatterLineColors.Clear();
                    accDeviationMagnitudeRenderer.scatterDrawingVariants.Clear();

                    accDeviationMagnitudeRenderer.dvScatterXSpace.Add(dvSecondsVector);
                    accDeviationMagnitudeRenderer.dvScatterFuncValues.Add(dvDataToShowAccMagnitudeDev);
                    accDeviationMagnitudeRenderer.scatterLineColors.Add(new Bgr(Color.Blue));
                    accDeviationMagnitudeRenderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);
                }
            }

            accDeviationMagnitudeRenderer.xSpaceMin = dvSecondsVector.Max() - sensorsHistoryRepresentingScale;

            accDeviationMagnitudeRenderer.Represent();
            ThreadSafeOperations.UpdatePictureBox(pbRepresentingDevMagnitude, accDeviationMagnitudeRenderer.TheImage.Bitmap);



            accelerometerData accCalibratedData = new accelerometerData(dmAccData[0, 3], dmAccData[0, 4],
                dmAccData[0, 5]);
            double phiAngle = Math.Asin(accCalibratedData.xyProjectionMagnitude() / accCalibratedData.AccMagnitude);
            double tmpL = accCalibratedData.AccMagnitude * Math.Sin(phiAngle) * Math.Cos(phiAngle);
            double tmpLz = tmpL * Math.Sin(phiAngle);
            double tmpLx = tmpL * Math.Cos(phiAngle) *
                           Math.Sqrt(1 +
                                     accCalibratedData.AccY * accCalibratedData.AccY /
                                     (accCalibratedData.AccX * accCalibratedData.AccX));
            double tmpLy = tmpLx * accCalibratedData.AccY / accCalibratedData.AccX;
            accelerometerData unitaryAccVectorZeroAngle = new accelerometerData(tmpLx, tmpLy, tmpLz);
            unitaryAccVectorZeroAngle = unitaryAccVectorZeroAngle / unitaryAccVectorZeroAngle.AccMagnitude;
            accelerometerData unitaryAccVectorCalibratedAcceleration = accCalibratedData / (accCalibratedData.AccMagnitude);

            DenseVector dvDataToShowAccDevDirection = DenseVector.Create(dmAccData.RowCount, i =>
            {
                accelerometerData currAccData = new accelerometerData(dmAccData[i, 0], dmAccData[i, 1], dmAccData[i, 2]);
                accelerometerData currAccDataProjectionPerpendicularToCalibratedAcc = currAccData -
                                                                                      unitaryAccVectorCalibratedAcceleration *
                                                                                      (currAccData * accCalibratedData / accCalibratedData.AccMagnitude);

                double retAngle =
                    Math.Acos(currAccDataProjectionPerpendicularToCalibratedAcc * unitaryAccVectorZeroAngle /
                              currAccDataProjectionPerpendicularToCalibratedAcc.AccMagnitude);
                accelerometerData vectProduct = unitaryAccVectorZeroAngle ^
                                                currAccDataProjectionPerpendicularToCalibratedAcc;
                if (vectProduct * unitaryAccVectorCalibratedAcceleration > 0)
                {
                    //значит угол лежит в пределах от 0 до PI - ничего не делаем
                    retAngle = retAngle + 0.0d;
                }
                else
                {
                    //векторное произведение противоположно по направлению g0 - значит, угол лежит в диапазоне от Pi до 2Pi или от -PI до PI
                    retAngle = -retAngle;
                }

                return retAngle;
            });




            if (accDeviationDirectionRenderer == null)
            {
                accDeviationDirectionRenderer = new MultipleScatterAndFunctionsRepresentation(pbRepresentingDevDirection.Size);
                accDeviationDirectionRenderer.dvScatterXSpace.Add(dvSecondsVector);
                accDeviationDirectionRenderer.dvScatterFuncValues.Add(dvDataToShowAccDevDirection);
                accDeviationDirectionRenderer.scatterLineColors.Add(new Bgr(Color.Blue));
                accDeviationDirectionRenderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);
            }
            else
            {
                if (accDeviationDirectionRenderer.xSpaceMin != dvSecondsVector.Max() - sensorsHistoryRepresentingScale)
                {
                    accDeviationDirectionRenderer.xSpaceMin = dvSecondsVector.Max() - sensorsHistoryRepresentingScale;


                    accDeviationDirectionRenderer.dvScatterXSpace.Clear();
                    accDeviationDirectionRenderer.dvScatterFuncValues.Clear();
                    accDeviationDirectionRenderer.scatterLineColors.Clear();
                    accDeviationDirectionRenderer.scatterDrawingVariants.Clear();

                    accDeviationDirectionRenderer.dvScatterXSpace.Add(dvSecondsVector);
                    accDeviationDirectionRenderer.dvScatterFuncValues.Add(dvDataToShowAccDevDirection);
                    accDeviationDirectionRenderer.scatterLineColors.Add(new Bgr(Color.Blue));
                    accDeviationDirectionRenderer.scatterDrawingVariants.Add(SequencesDrawingVariants.polyline);
                }
            }

            accDeviationDirectionRenderer.xSpaceMin = dvSecondsVector.Max() - sensorsHistoryRepresentingScale;

            accDeviationDirectionRenderer.Represent();
            ThreadSafeOperations.UpdatePictureBox(pbRepresentingDevDirection, accDeviationDirectionRenderer.TheImage.Bitmap);

        }


        private void FunctionRepresentationForm_Load(object sender, EventArgs e)
        {
            Represent();
        }

        private void FunctionRepresentationForm_Resize(object sender, EventArgs e)
        {
            accDeviationAngleRenderer.ResizeCanvas(pbRepresentingDevAngle.Size);
            accDeviationDirectionRenderer.ResizeCanvas(pbRepresentingDevDirection.Size);
            accDeviationMagnitudeRenderer.ResizeCanvas(pbRepresentingDevMagnitude.Size);
            Represent();
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            sensorsHistoryRepresentingScale = trackBar1.Value;
            Represent();
        }

        private void pbRepresentingDevMagnitude_MouseClick(object sender, MouseEventArgs e)
        {
            MultipleScatterAndFunctionsRepresentation currentRenderer = null;
            if (sender == pbRepresentingDevMagnitude)
            {
                currentRenderer = accDeviationMagnitudeRenderer;
            }
            else if (sender == pbRepresentingDevAngle) currentRenderer = accDeviationAngleRenderer;
            else currentRenderer = accDeviationDirectionRenderer;

            PointD ptDataUnderMouseClick = currentRenderer.GetDataCoordinatesOfMouseClick((PictureBox)sender, e);
            int currentDateTimeIndex = 0;
            for (int i = 0; i < currentRenderer.dvScatterXSpace[0].Count - 1; i++)
            {
                if ((ptDataUnderMouseClick.X >= currentRenderer.dvScatterXSpace[0][i]) && (ptDataUnderMouseClick.X <= currentRenderer.dvScatterXSpace[0][i + 1]))
                {
                    if (Math.Abs(ptDataUnderMouseClick.X - currentRenderer.dvScatterXSpace[0][i]) <
                        Math.Abs(ptDataUnderMouseClick.X - currentRenderer.dvScatterXSpace[0][i + 1]))
                    {
                        currentDateTimeIndex = i;
                    }
                    else currentDateTimeIndex = i + 1;
                    break;
                }
            }

            accDeviationMagnitudeRenderer.verticalMarkersList.Clear();
            accDeviationMagnitudeRenderer.verticalMarkersList.Add(currentRenderer.dvScatterXSpace[0][currentDateTimeIndex]);
            accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace.Clear();
            accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace.Add(currentDateTimeIndex);

            accDeviationAngleRenderer.verticalMarkersList.Clear();
            accDeviationAngleRenderer.verticalMarkersList.Add(currentRenderer.dvScatterXSpace[0][currentDateTimeIndex]);
            accDeviationAngleRenderer.verticalMarkersIndexesUsingXSpace.Clear();
            accDeviationAngleRenderer.verticalMarkersIndexesUsingXSpace.Add(currentDateTimeIndex);

            accDeviationDirectionRenderer.verticalMarkersList.Clear();
            accDeviationDirectionRenderer.verticalMarkersList.Add(currentRenderer.dvScatterXSpace[0][currentDateTimeIndex]);
            accDeviationDirectionRenderer.verticalMarkersIndexesUsingXSpace.Clear();
            accDeviationDirectionRenderer.verticalMarkersIndexesUsingXSpace.Add(currentDateTimeIndex);
            Represent();
        }



        private void AccelerometerDataRepresentingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (pbRepresentingDevMagnitude.Focused || pbRepresentingDevAngle.Focused || pbRepresentingDevDirection.Focused)
            {
                if (accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace.Count == 0) return;
                int currentDateTimeIndex = accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace[0];
                if (e.KeyCode == Keys.Left)
                {
                    currentDateTimeIndex -= 1;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    currentDateTimeIndex += 1;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    return;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    return;
                }
                else return;

                accDeviationMagnitudeRenderer.verticalMarkersList.Clear();
                accDeviationMagnitudeRenderer.verticalMarkersList.Add(accDeviationMagnitudeRenderer.dvScatterXSpace[0][currentDateTimeIndex]);
                accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace.Clear();
                accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace.Add(currentDateTimeIndex);

                accDeviationAngleRenderer.verticalMarkersList.Clear();
                accDeviationAngleRenderer.verticalMarkersList.Add(accDeviationAngleRenderer.dvScatterXSpace[0][currentDateTimeIndex]);
                accDeviationAngleRenderer.verticalMarkersIndexesUsingXSpace.Clear();
                accDeviationAngleRenderer.verticalMarkersIndexesUsingXSpace.Add(currentDateTimeIndex);

                accDeviationDirectionRenderer.verticalMarkersList.Clear();
                accDeviationDirectionRenderer.verticalMarkersList.Add(accDeviationDirectionRenderer.dvScatterXSpace[0][currentDateTimeIndex]);
                accDeviationDirectionRenderer.verticalMarkersIndexesUsingXSpace.Clear();
                accDeviationDirectionRenderer.verticalMarkersIndexesUsingXSpace.Add(currentDateTimeIndex);
                Represent();
            }
        }



        private BackgroundWorker bgwPlayingPresenter;
        private void button1_Click(object sender, EventArgs e)
        {
            if (bgwPlayingPresenter != null && bgwPlayingPresenter.IsBusy)
            {
                bgwPlayingPresenter.CancelAsync();
                return;
            }

            if (accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace.Count == 0) return;
            int currentDateTimeIndex = accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace[0];
            ThreadSafeOperations.ToggleButtonState(btnPlayStopAccAnimation, true, "STOP", true);

            DoWorkEventHandler currDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
            {
                BackgroundWorker selfworker = currBGWsender as BackgroundWorker;
                SimpleVectorGraphics2D frontViewRenderer = new SimpleVectorGraphics2D(pbAccSideVisualization.Size);
                frontViewRenderer.presentAxes = false;
                frontViewRenderer.ptLeftTopSpaceCorner = new PointD(-5.0d, 0.0d);
                frontViewRenderer.ptRightBottomSpaceCorner = new PointD(5.0d, -10.0d);

                //frontViewRenderer.listVectorsToDraw.Add(DenseVector.OfEnumerable(new double[] {1.0d, -9.0d}));
                //frontViewRenderer.listVectorsShift.Add(DenseVector.OfEnumerable(new double[] {0.0d, 0.0d}));
                //frontViewRenderer.listVectColors.Add(new Bgr(Color.Blue));

                //frontViewRenderer.Represent();
                //ThreadSafeOperations.UpdatePictureBox(pbAccSideVisualization, frontViewRenderer.TheImage.Bitmap, false);

                for (int i = currentDateTimeIndex; i < accDeviationMagnitudeRenderer.dvScatterXSpace[0].Count - 1; i++)
                {
                    if (selfworker.CancellationPending)
                    {
                        break;
                    }

                    DateTime dtStartCalculate = DateTime.Now;

                    accDeviationMagnitudeRenderer.verticalMarkersList.Clear();
                    accDeviationMagnitudeRenderer.verticalMarkersList.Add(accDeviationMagnitudeRenderer.dvScatterXSpace[0][i]);
                    accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace.Clear();
                    accDeviationMagnitudeRenderer.verticalMarkersIndexesUsingXSpace.Add(i);
                    accDeviationMagnitudeRenderer.RepresentMarkers();
                    ThreadSafeOperations.UpdatePictureBox(pbRepresentingDevMagnitude,
                        accDeviationMagnitudeRenderer.TheImage.Bitmap, false);

                    accDeviationAngleRenderer.verticalMarkersList.Clear();
                    accDeviationAngleRenderer.verticalMarkersList.Add(accDeviationAngleRenderer.dvScatterXSpace[0][i]);
                    accDeviationAngleRenderer.verticalMarkersIndexesUsingXSpace.Clear();
                    accDeviationAngleRenderer.verticalMarkersIndexesUsingXSpace.Add(i);
                    accDeviationAngleRenderer.RepresentMarkers();
                    ThreadSafeOperations.UpdatePictureBox(pbRepresentingDevAngle, accDeviationAngleRenderer.TheImage.Bitmap,
                        false);

                    accDeviationDirectionRenderer.verticalMarkersList.Clear();
                    accDeviationDirectionRenderer.verticalMarkersList.Add(accDeviationDirectionRenderer.dvScatterXSpace[0][i]);
                    accDeviationDirectionRenderer.verticalMarkersIndexesUsingXSpace.Clear();
                    accDeviationDirectionRenderer.verticalMarkersIndexesUsingXSpace.Add(i);
                    accDeviationDirectionRenderer.RepresentMarkers();
                    ThreadSafeOperations.UpdatePictureBox(pbRepresentingDevDirection, accDeviationDirectionRenderer.TheImage.Bitmap,
                        false);

                    frontViewRenderer.listVectorsToDraw.Clear();
                    frontViewRenderer.listVectorsShift.Clear();
                    frontViewRenderer.listVectColors.Clear();

                    //int globalArrayIndex = sensorsHistoryRepresentingScale + i-1;
                    DenseVector zeroValuesVect = DenseVector.OfEnumerable(new double[] {0.0d, 0.0d});
                    DenseVector dvAccCalibrated = DenseVector.Create(3, idx => dmAccData[i, idx + 3]);
                    DenseVector dvAccCurrent = DenseVector.Create(3, idx => dmAccData[i, idx]);
                    accelerometerData accCalibrated = new accelerometerData(dvAccCalibrated);
                    accCalibrated.AccDoubleZ = -accCalibrated.AccDoubleZ;
                    accelerometerData currAcc = new accelerometerData(dvAccCurrent);
                    currAcc.AccDoubleZ = -currAcc.AccDoubleZ;
                    double koeffToRealGravity = 9.81d/accCalibrated.AccMagnitude;
                    accCalibrated = accCalibrated*koeffToRealGravity;
                    currAcc = currAcc*koeffToRealGravity;
                    accelerometerData dAccCalibration = accCalibrated - (new accelerometerData(0.0d, 0.0d, -9.81d));
                    currAcc = currAcc - dAccCalibration;
                    accCalibrated = accCalibrated - dAccCalibration;

                    frontViewRenderer.listVectorsToDraw.Add(DenseVector.OfEnumerable(new double[] { accCalibrated.xyProjectionMagnitude(), accCalibrated.AccDoubleZ }));
                    frontViewRenderer.listVectorsShift.Add(zeroValuesVect);
                    frontViewRenderer.listVectColors.Add(new Bgr(Color.Blue));

                    frontViewRenderer.listVectorsToDraw.Add(DenseVector.OfEnumerable(new double[] { currAcc.xyProjectionMagnitude(), currAcc.AccDoubleZ }));
                    frontViewRenderer.listVectorsShift.Add(zeroValuesVect);
                    frontViewRenderer.listVectColors.Add(new Bgr(Color.Red));

                    frontViewRenderer.Represent();
                    ThreadSafeOperations.UpdatePictureBox(pbAccSideVisualization, frontViewRenderer.TheImage.Bitmap, false);


                    TimeSpan dtCalculationTime = DateTime.Now - dtStartCalculate;

                    double timeToWait = (accDeviationDirectionRenderer.dvScatterXSpace[0][i + 1] -
                                        accDeviationDirectionRenderer.dvScatterXSpace[0][i]) * 1000.0d
                                        - (double)(dtCalculationTime.TotalMilliseconds);


                    if (timeToWait > 0)
                    {
                        //System.Windows.Forms.Application.DoEvents();
                        Thread.Sleep(Convert.ToInt32(timeToWait));
                    }
                }
            };


            RunWorkerCompletedEventHandler currWorkCompletedHandler =
                delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    ThreadSafeOperations.ToggleButtonState(btnPlayStopAccAnimation, true, "PLAY", true);
                };

            bgwPlayingPresenter = new BackgroundWorker();
            bgwPlayingPresenter.WorkerSupportsCancellation = true;
            bgwPlayingPresenter.DoWork += currDoWorkHandler;
            bgwPlayingPresenter.RunWorkerCompleted += currWorkCompletedHandler;
            object[] BGWargs = new object[] { "", "" };
            bgwPlayingPresenter.RunWorkerAsync(BGWargs);

            
        }

        private void btnRepresentingDevMagnitudeEnlarge_Click(object sender, EventArgs e)
        {
            MultipleScatterAndFunctionsRepresentation currentRenderer = accDeviationMagnitudeRenderer;
            if (sender == btnRepresentingDevAngleEnlarge)
            {
                currentRenderer = accDeviationAngleRenderer.Copy();
            }
            else if (sender == btnRepresentingDevDirectionEnlarge)
            {
                currentRenderer = accDeviationDirectionRenderer.Copy();
            }
            else if (sender == btnRepresentingDevMagnitudeEnlarge)
            {
                currentRenderer = accDeviationMagnitudeRenderer.Copy();
            }

            SimpleShowImageForm imgForm = new SimpleShowImageForm(currentRenderer.TheImage.CopyBlank().Bitmap);
            imgForm.FormResizing += new EventHandler((object resizingSender, EventArgs resizingEventArgs) =>
            {
                currentRenderer.ResizeCanvas(imgForm.pb1.Size);
                imgForm.UpdateBitmap(currentRenderer.TheImage.Bitmap);
            });
            imgForm.Show();
            currentRenderer.ResizeCanvas(imgForm.pb1.Size);
            imgForm.UpdateBitmap(currentRenderer.TheImage.Bitmap);
        }

        private void btnRepresentingDevMagnitudeRefresh_Click(object sender, EventArgs e)
        {
            MultipleScatterAndFunctionsRepresentation currentRenderer = accDeviationMagnitudeRenderer;
            if (sender == btnRepresentingDevAngleRefresh)
            {
                currentRenderer = accDeviationAngleRenderer;
            }
            else if (sender == btnRepresentingDevDirectionRefresh)
            {
                currentRenderer = accDeviationDirectionRenderer;
            }
            else if (sender == btnRepresentingDevMagnitudeRefresh)
            {
                currentRenderer = accDeviationMagnitudeRenderer;
            }

            accDeviationMagnitudeRenderer = null;
            accDeviationAngleRenderer = null;
            accDeviationDirectionRenderer = null;
            dmAccData = null;
            Represent();
        }

        private void btnOpenAnotherLogFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                strLogFilename = fileOpen.FileName;
                Dictionary<string, object> dictDataToShow = NetCDFoperations.ReadDataFromFile(strLogFilename);
                if (!dictDataToShow.Keys.Contains("AccelerometerData"))
                {
                    MessageBox.Show("It`s not an acceleration data log. Try another file.", "", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                sensorsHistoryRepresentingScale = 30;
                ThreadSafeOperations.MoveTrackBar(trackBar1, 30);
                strLogFilename = fileOpen.FileName;

                Represent();
            }
        }
    }
}
