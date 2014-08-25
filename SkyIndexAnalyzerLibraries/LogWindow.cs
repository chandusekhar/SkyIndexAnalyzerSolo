using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyIndexAnalyzerLibraries
{
    public partial class LogWindow : Form
    {
        public LogWindow()
        {
            InitializeComponent();
        }


        public void LogAText(string theText, bool appendMode = true)
        {
            ThreadSafeOperations.SetTextTB(textBox1, theText + Environment.NewLine, appendMode);
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
                this.Close();
            }
        }


        public void SwapLog(string filename)
        {
            if (!ServiceTools.CheckIfDirectoryExists(filename)) return;

            ServiceTools.logToTextFile(filename, textBox1.Text, false);

            ThreadSafeOperations.SetTextTB(textBox1, "", false);
        }


        public int LinesCount
        {
            get { return textBox1.Lines.Length; }
        }
    }
}
