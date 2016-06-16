using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyImagesAnalyzerLibraries
{
    public partial class LogWindow : Form
    {
        private class LinesToDisplay
        {
            public string textLine = "";
            public bool appendMode = true;
        }

        private ConcurrentQueue<LinesToDisplay> cqLinesToLog = new ConcurrentQueue<LinesToDisplay>();
        private BackgroundWorker bgwQueueOutputWorker = new BackgroundWorker();
        private string _title = "Log window";


        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                Text = _title;
            }
        }


        public LogWindow()
        {
            InitializeComponent();

            bgwQueueOutputWorker.WorkerSupportsCancellation = true;
            bgwQueueOutputWorker.DoWork += bgwQueueOutputWorker_DoWork;
            bgwQueueOutputWorker.RunWorkerAsync();
        }


        void bgwQueueOutputWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker thisWorker = sender as BackgroundWorker;

            while (true)
            {
                if (thisWorker.CancellationPending)
                {
                    break;
                }

                try
                {
                    if (cqLinesToLog.Count > 0)
                    {
                        while (cqLinesToLog.Count > 0)
                        {
                            LinesToDisplay currLine = new LinesToDisplay();
                            while (!cqLinesToLog.TryDequeue(out currLine))
                            {
                                Application.DoEvents();
                                Thread.Sleep(0);
                            }
                            ThreadSafeOperations.SetTextTB(textBox1, currLine.textLine, currLine.appendMode);
                            Application.DoEvents();
                            Thread.Sleep(0);
                        }
                    }
                    else
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                        continue;
                    }
                }
                catch (Exception)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                    continue;
                }
            }
        }


        public void LogAText(string theText, bool appendMode = true)
        {
            cqLinesToLog.Enqueue(new LinesToDisplay()
            {
                appendMode = appendMode,
                textLine = theText + Environment.NewLine,
            });
        }

        /// <summary>
        /// Handles the KeyPress event of the LogWindow control.
        /// closes the form
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void LogWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                bgwQueueOutputWorker.CancelAsync();
                this.Close();
            }
            if (e.KeyChar == 'c')
            {
                ClearLog(0);
            }
        }


        public void SwapLog(string filename)
        {
            if (!ServiceTools.CheckIfDirectoryExists(filename)) return;

            ServiceTools.logToTextFile(filename, textBox1.Text, false);

            ThreadSafeOperations.SetTextTB(textBox1, "", false);
        }



        public void ClearLog(int maxLinesCount = 2048)
        {
            if (textBox1.Lines.Count() >= maxLinesCount)
            {
                ThreadSafeOperations.SetTextTB(textBox1, "", false);
            }
        }




        protected override void OnClosing(CancelEventArgs e)
        {
            bgwQueueOutputWorker.CancelAsync();

            base.OnClosing(e);
        }




        public int LinesCount
        {
            get { return textBox1.Lines.Length; }
        }




        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearLog(0);
        }
    }
}
