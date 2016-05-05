using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using SkyImagesAnalyzerLibraries;

namespace IofffeVesselInfoStream
{
    public partial class GeotrackPresentationForm : Form
    {
        private static GeoTrackRenderer geoRenderer = new GeoTrackRenderer(Directory.GetCurrentDirectory() + "\\shoreline\\"); //(Directory.GetCurrentDirectory() + "\\geogrid\\regrid.nc");
        private static GPSdata actualGPSdata = null;
        //private static bool showGeoTrack = true;
        private MouseEventArgs meResLeftButtonDownArgs = null;
        private object meResLeftButtonDownSender = null;
        


        public GeotrackPresentationForm()
        {
            InitializeComponent();
        }


        private void GeotrackPresentationForm_Load(object sender, EventArgs e)
        {
            bgwGeotrackRenderer.RunWorkerAsync();
        }





        #region Geotrack

        private static bool needToUpdateGeoTrackNow = false;
        private void bgwGraphsRenderer_DoWork(object sender, DoWorkEventArgs e)
        {

        }


        //private static bool geoTrackIsUpdatingNow = false;
        private bool UpdateGeoTrack()
        {
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            try
            {
                geoRenderer.listMarkers.Clear();
                geoRenderer.listMarkers.Add(new Tuple<GPSdata, SequencesDrawingVariants, Bgr>(actualGPSdata,
                    SequencesDrawingVariants.triangles, GeoTrackRenderer.tracksColor));

                if (geoRenderer.mapFollowsActualGPSposition)
                {
                    geoRenderer.gpsMapCenteredPoint = actualGPSdata.Clone();
                }

                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
                //ThreadSafeOperations.UpdateImagePanel(imgPanelGeotrack, geoRenderer.RepresentTopo(imgPanelGeotrack.Size), cbNormalizeImage.Checked);
                //ThreadSafeOperations.UpdateImagePanel(imgPanelGeotrack, geoRenderer.RepresentTopo(new Size(0, 0)), cbNormalizeImage.Checked);
                ThreadSafeOperations.UpdatePictureBox(pbGeotrack, geoRenderer.RepresentTopo(new Size(0, 0)), cbNormalizeImage.Checked);
                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            return true;
        }



        private void ShowGeoTrackOnce()
        {
            // пропишем в рисователь трека имена файлов, содержащих логи данных GPS
            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);

            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {

                if ((geoRenderer.lTracksData.Count == 0) || (geoRenderer.lTracksData[0].tsGPSdata.Count == 0))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\logs\\");
                    foreach (FileInfo fInfo in dirInfo.EnumerateFiles("*GPS*.nc", SearchOption.TopDirectoryOnly))
                    {
                        geoRenderer.listGPSdataLogNetCDFFileNames.Add(fInfo.FullName);
                    }

                    geoRenderer.ReadGPSFiles(lblStatusString);
                }

                if ((geoRenderer.lTracksData[0].tsGPSdata.Count > 0))
                {
                    actualGPSdata = geoRenderer.lTracksData[0].tsGPSdata.DataValues.Last();
                }
                else
                {
                    actualGPSdata = new GPSdata(6000.0d, 0.0d);
                }


                geoRenderer.listMarkers.Clear();
                geoRenderer.listMarkers.Add(new Tuple<GPSdata, SequencesDrawingVariants, Bgr>(actualGPSdata,
                    SequencesDrawingVariants.triangles, GeoTrackRenderer.tracksColor));

                if (geoRenderer.mapFollowsActualGPSposition)
                {
                    geoRenderer.gpsMapCenteredPoint = actualGPSdata.Clone();
                }

                // ThreadSafeOperations.UpdateImagePanel(imgPanelGeotrack, geoRenderer.RepresentTopo(imgPanelGeotrack.Size), cbNormalizeImage.Checked);
                //ThreadSafeOperations.UpdateImagePanel(imgPanelGeotrack, geoRenderer.RepresentTopo(new Size(0, 0)), cbNormalizeImage.Checked);
                //ThreadSafeOperations.UpdatePictureBox(pbGeotrack, geoRenderer.RepresentTopo(new Size(0, 0)), cbNormalizeImage.Checked);
                ThreadSafeOperations.UpdatePictureBox(pbGeotrack, geoRenderer.RepresentTopo(pbGeotrack.Size), cbNormalizeImage.Checked);
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
        }




        private void trbGeoTrackScale_ValueChanged(object sender, EventArgs e)
        {
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                if (geoRenderer.lTracksData.Count == 0)
                {
                    trbGeoTrackScale.Value = 0;
                    return;
                }
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            trbGeoTrackScale.Enabled = false;
            double scale = Math.Pow(2.0d, trbGeoTrackScale.Value);
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                //geoRenderer.ScaleFactor = scale;
                geoRenderer.ScaleImageSize(scale);
            }
            finally
            {
                Monitor.Exit(geoRenderer);
                trbGeoTrackScale.Enabled = true;
            }

            if (!bgwGeotrackRenderer.IsBusy)
            {
                ShowGeoTrackOnce();
            }
            else
            {
                needToUpdateGeoTrackNow = true;
            }
            trbGeoTrackScale.Enabled = true;
        }





        private void pbGeoTrack_Click(object sender, EventArgs e)
        {
            if (geoRenderer == null)
            {
                return;
            }
            else if (geoRenderer.lTracksData.Count == 0)
            {
                return;
            }

            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                SimpleShowImageForm f1 = new SimpleShowImageForm((new Image<Bgr, byte>(new Size(800, 600))).Bitmap);
                f1.FormResizing += ExternalGeotrackImageFormResized;
                f1.Show();
                f1.UpdateBitmap(geoRenderer.RepresentTopo(f1.pb1.Size));
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }
        }



        public void ExternalGeotrackImageFormResized(object sender, EventArgs e)
        {
            SimpleShowImageForm f1 = sender as SimpleShowImageForm;
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                f1.UpdateBitmap(geoRenderer.RepresentTopo(f1.pb1.Size));
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }
        }


        #region // obsolete

        //private void cbShowGeoTrack_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (((sender as CheckBox).Checked) && (!bgwGeotrackRenderer.IsBusy) && (!bgwStreamDataProcessing.IsBusy))
        //    {
        //        var repl = MessageBox.Show("Wanna watch right now?", "Right now?", MessageBoxButtons.YesNoCancel,
        //            MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
        //        if (repl == System.Windows.Forms.DialogResult.Yes)
        //        {


        //            DoWorkEventHandler bgw1DoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
        //            {
        //                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
        //                BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
        //                object[] currBGWarguments = (object[])args.Argument;

        //                ShowGeoTrackOnce();

        //                args.Result = new object[] { "" };
        //            };


        //            RunWorkerCompletedEventHandler currWorkCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
        //            {
        //                object[] currentBGWResults = (object[])args.Result;
        //                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
        //            };


        //            BackgroundWorker bgw1 = new BackgroundWorker();
        //            bgw1.DoWork += bgw1DoWorkHandler;
        //            bgw1.RunWorkerCompleted += currWorkCompletedHandler;
        //            object[] BGWargs = new object[] { "" };
        //            bgw1.RunWorkerAsync(BGWargs);
        //        }
        //    }
        //    else if ((!(sender as CheckBox).Checked) && (bgwGeotrackRenderer.IsBusy))
        //    {
        //        bgwGeotrackRenderer.CancelAsync();
        //    }
        //    else if (((sender as CheckBox).Checked) && (!bgwGeotrackRenderer.IsBusy) && (bgwStreamDataProcessing.IsBusy))
        //    {
        //        bgwGeotrackRenderer.RunWorkerAsync();
        //    }
        //}






        //private void scrbGeoTrackScrollLonValues_ValueChanged(object sender, EventArgs e)
        //{
        //    if (scrbGeoTrackScrollLonValues.Value == 0)
        //    {
        //        return;
        //    }

        //    if (showGeoTrack)
        //    {
        //        while (!Monitor.TryEnter(geoRenderer, 100))
        //        {
        //            Application.DoEvents();
        //            Thread.Sleep(0);
        //        }
        //        try
        //        {
        //            geoRenderer.MoveGPSWindow(-scrbGeoTrackScrollLonValues.Value, 0);
        //        }
        //        finally
        //        {
        //            Monitor.Exit(geoRenderer);
        //        }

        //        scrbGeoTrackScrollLonValues.Value = 0;


        //        BackgroundWorker bgwGraphUpdater = new BackgroundWorker();

        //        DoWorkEventHandler bgwGraphUpdaterDoWorkHandler = delegate (object currBGWsender, DoWorkEventArgs args)
        //        {

        //            BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
        //            object[] currBGWarguments = (object[])args.Argument;

        //            Stopwatch sw1 = Stopwatch.StartNew();

        //            bool retResult = false;

        //            while (true)
        //            {
        //                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
        //                if (UpdateGeoTrack())
        //                {
        //                    retResult = true;
        //                    break;
        //                }
        //                else if (sw1.ElapsedMilliseconds > 10000)
        //                {
        //                    retResult = false;
        //                    break;
        //                }
        //                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
        //                Application.DoEvents();
        //                Thread.Sleep(0);
        //            }
        //            args.Result = new object[] { retResult };
        //        };


        //        RunWorkerCompletedEventHandler bgwGraphUpdaterCompletedHandler = delegate (object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
        //        {
        //            object[] currentBGWResults = (object[])args.Result;
        //            bool UpdatingResult = (bool)(currentBGWResults[0]);

        //            if (!UpdatingResult)
        //            {
        //                ThreadSafeOperations.SetText(lblStatusString, DateTime.Now.ToString("s").Replace("T", " ") + ": Geogtrack failed to be updated.", false);
        //            }
        //            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
        //        };

        //        bgwGraphUpdater.DoWork += bgwGraphUpdaterDoWorkHandler;
        //        bgwGraphUpdater.RunWorkerCompleted += bgwGraphUpdaterCompletedHandler;
        //        bgwGraphUpdater.RunWorkerAsync();
        //        //}
        //    }
        //}




        //private void scrbGeoTrackScrollLatValues_ValueChanged(object sender, EventArgs e)
        //{
        //    if (scrbGeoTrackScrollLatValues.Value == 0)
        //    {
        //        return;
        //    }

        //    if (showGeoTrack)
        //    {
        //        while (!Monitor.TryEnter(geoRenderer, 100))
        //        {
        //            Application.DoEvents();
        //            Thread.Sleep(0);
        //        }
        //        try
        //        {
        //            geoRenderer.MoveGPSWindow(0, -scrbGeoTrackScrollLatValues.Value);
        //        }
        //        finally
        //        {
        //            Monitor.Exit(geoRenderer);
        //        }

        //        scrbGeoTrackScrollLatValues.Value = 0;

        //        //if (bgwGraphsRenderer.IsBusy)
        //        //{
        //        //    needToUpdateGeoTrackNow = true;
        //        //}
        //        //else
        //        //{
        //        BackgroundWorker bgwGraphUpdater = new BackgroundWorker();

        //        DoWorkEventHandler bgwGraphUpdaterDoWorkHandler = delegate(object currBGWsender, DoWorkEventArgs args)
        //        {

        //            BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
        //            object[] currBGWarguments = (object[])args.Argument;

        //            Stopwatch sw1 = Stopwatch.StartNew();

        //            bool retResult = false;

        //            while (true)
        //            {
        //                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
        //                if (UpdateGeoTrack())
        //                {
        //                    retResult = true;
        //                    break;
        //                }
        //                else if (sw1.ElapsedMilliseconds > 10000)
        //                {
        //                    retResult = false;
        //                    break;
        //                }
        //                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
        //                Application.DoEvents();
        //                Thread.Sleep(0);
        //            }
        //            args.Result = new object[] { retResult };
        //        };


        //        RunWorkerCompletedEventHandler bgwGraphUpdaterCompletedHandler = delegate(object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
        //        {
        //            object[] currentBGWResults = (object[])args.Result;
        //            bool UpdatingResult = (bool)(currentBGWResults[0]);

        //            if (!UpdatingResult)
        //            {
        //                ThreadSafeOperations.SetText(lblStatusString, DateTime.Now.ToString("s").Replace("T", " ") + ": Geogtrack failed to be updated.", false);
        //            }
        //            ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
        //        };

        //        bgwGraphUpdater.DoWork += bgwGraphUpdaterDoWorkHandler;
        //        bgwGraphUpdater.RunWorkerCompleted += bgwGraphUpdaterCompletedHandler;
        //        bgwGraphUpdater.RunWorkerAsync();
        //        //}
        //    }
        //}


        #endregion // obsolete



        private void btnCenterToActualPosition_Click(object sender, EventArgs e)
        {
            //if (showGeoTrack)
            //{
                while (!Monitor.TryEnter(geoRenderer, 100))
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                try
                {
                    //geoRenderer.MoveGPSWindow(actualGPSdata);
                    geoRenderer.mapFollowsActualGPSposition = true;
                }
                finally
                {
                    Monitor.Exit(geoRenderer);
                }

                //if (bgwGraphsRenderer.IsBusy)
                //{
                //    needToUpdateGeoTrackNow = true;
                //}
                //else
                //{
                BackgroundWorker bgwGraphUpdater = new BackgroundWorker();

                DoWorkEventHandler bgwGraphUpdaterDoWorkHandler = delegate (object currBGWsender, DoWorkEventArgs args)
                {

                    BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                    object[] currBGWarguments = (object[])args.Argument;

                    Stopwatch sw1 = Stopwatch.StartNew();

                    bool retResult = false;

                    while (true)
                    {
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
                        if (UpdateGeoTrack())
                        {
                            retResult = true;
                            break;
                        }
                        else if (sw1.ElapsedMilliseconds > 10000)
                        {
                            retResult = false;
                            break;
                        }
                        ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                    args.Result = new object[] { retResult };
                };


                RunWorkerCompletedEventHandler bgwGraphUpdaterCompletedHandler = delegate (object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    bool UpdatingResult = (bool)(currentBGWResults[0]);

                    if (!UpdatingResult)
                    {
                        ThreadSafeOperations.SetText(lblStatusString, DateTime.Now.ToString("s").Replace("T", " ") + ": Geogtrack failed to be updated.", false);
                    }
                    ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                };

                bgwGraphUpdater.DoWork += bgwGraphUpdaterDoWorkHandler;
                bgwGraphUpdater.RunWorkerCompleted += bgwGraphUpdaterCompletedHandler;
                bgwGraphUpdater.RunWorkerAsync();
                //}
            //}
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //showGeoTrack = !showGeoTrack;
            //if (showGeoTrack)
            //{
            //    ThreadSafeOperations.ToggleButtonState(btnUpdate, true, "ON", true);
            //}
            //else
            //{
            //    ThreadSafeOperations.ToggleButtonState(btnUpdate, true, "OFF", true);
            //}

            //if (showGeoTrack && !bgwGeotrackRenderer.IsBusy && !bgwStreamDataProcessing.IsBusy)
            if (!bgwGeotrackRenderer.IsBusy)
            {
                //var repl = MessageBox.Show("Wanna watch right now?", "Right now?", MessageBoxButtons.YesNoCancel,
                //    MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                //if (repl == DialogResult.Yes)
                //{
                ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, true, true, wcUpdatimgGeoTrack.Color);
                DoWorkEventHandler bgw1DoWorkHandler = delegate (object currBGWsender, DoWorkEventArgs args)
                    {

                        BackgroundWorker selfWorker = currBGWsender as BackgroundWorker;
                        object[] currBGWarguments = (object[])args.Argument;

                        ShowGeoTrackOnce();

                        args.Result = new object[] { "" };
                    };


                RunWorkerCompletedEventHandler currWorkCompletedHandler = delegate (object currBGWCompletedSender, RunWorkerCompletedEventArgs args)
                {
                    object[] currentBGWResults = (object[])args.Result;
                    ThreadSafeOperations.SetLoadingCircleState(wcUpdatimgGeoTrack, false, false, wcUpdatimgGeoTrack.Color);
                };


                BackgroundWorker bgw1 = new BackgroundWorker();
                bgw1.DoWork += bgw1DoWorkHandler;
                bgw1.RunWorkerCompleted += currWorkCompletedHandler;
                object[] BGWargs = new object[] { "" };
                bgw1.RunWorkerAsync(BGWargs);
                //}
            }
            //else if (!showGeoTrack && bgwGeotrackRenderer.IsBusy)
            //{
            //    bgwGeotrackRenderer.CancelAsync();
            //}
            //else if ((showGeoTrack) && (!bgwGeotrackRenderer.IsBusy) && (bgwStreamDataProcessing.IsBusy))
            //{
            //    bgwGeotrackRenderer.RunWorkerAsync();
            //}
        }





        private void btnDecreaseResolution_Click(object sender, EventArgs e)
        {
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                if (geoRenderer.lTracksData.Count == 0)
                {
                    trbGeoTrackScale.Value = 0;
                    return;
                }
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }


            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                geoRenderer.DecreaseShorelineResolution();
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            if (!bgwGeotrackRenderer.IsBusy)
            {
                ShowGeoTrackOnce();
            }
            else
            {
                needToUpdateGeoTrackNow = true;
            }
        }



        private void btnIncreaseResolution_Click(object sender, EventArgs e)
        {
            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                if (geoRenderer.lTracksData.Count == 0)
                {
                    trbGeoTrackScale.Value = 0;
                    return;
                }
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }


            while (!Monitor.TryEnter(geoRenderer, 100))
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            try
            {
                geoRenderer.IncreaseShorelineResolution();
            }
            finally
            {
                Monitor.Exit(geoRenderer);
            }

            if (!bgwGeotrackRenderer.IsBusy)
            {
                ShowGeoTrackOnce();
            }
            else
            {
                needToUpdateGeoTrackNow = true;
            }
        }




        private void cbNormalizeImage_CheckedChanged(object sender, EventArgs e)
        {
            ShowGeoTrackOnce();
        }







        public void UpdateGPSdataForGeotrackRenderer(TimeSeries<GPSdata> tsCollectedGPSdata)
        {
            //if (showGeoTrack)
            //{
                List<DateTime> listDateTimeValuesToUpdateTrack;
                List<GPSdata> listGPSdataValuesToUpdateTrack;

                Monitor.Enter(tsCollectedGPSdata);
                try
                {
                    listDateTimeValuesToUpdateTrack = new List<DateTime>(tsCollectedGPSdata.TimeStamps);
                    listGPSdataValuesToUpdateTrack = new List<GPSdata>(tsCollectedGPSdata.DataValues);
                }
                finally
                {
                    Monitor.Exit(tsCollectedGPSdata);
                }


                while (!Monitor.TryEnter(geoRenderer, 100))
                {
                    Thread.Sleep(0);
                }

                try
                {
                    geoRenderer.lTracksData[0].tsGPSdata.AddSubseriaData(listGPSdataValuesToUpdateTrack, listDateTimeValuesToUpdateTrack, true);
                    if (geoRenderer.mapFollowsActualGPSposition)
                    {
                        geoRenderer.gpsMapCenteredPoint = actualGPSdata.Clone();
                    }
                }
                finally
                {
                    Monitor.Exit(geoRenderer);
                }
            //}
        }





        private void bgwGeotrackRenderer_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowGeoTrackOnce();
        }



        #endregion Geotrack




        private void GeotrackPresentationForm_SizeChanged(object sender, EventArgs e)
        {
            while (bgwGeotrackRenderer.IsBusy)
            {
                Thread.Sleep(100);
            }
            ShowGeoTrackOnce();
        }


        
        private void rtbTrackLineWidth_TextChanged(object sender, EventArgs e)
        {

        }



        private void TrackLineWidthChanged(object sender, EventArgs e)
        {
            if (geoRenderer == null) return;
            if (rtbTrackLineWidth.Text == "") return;

            try
            {
                geoRenderer.trackLineWidth = Convert.ToInt32((sender as RichTextBox).Text);
            }
            catch (Exception ex)
            {
                return;
            }

            while (bgwGeotrackRenderer.IsBusy)
            {
                Thread.Sleep(100);
            }
            ShowGeoTrackOnce();
        }



        private void rtbTrackLineWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)// Enter key
            {
                TrackLineWidthChanged(sender, e);
            }
        }

        private void GeotrackPresentationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }
    }
}
