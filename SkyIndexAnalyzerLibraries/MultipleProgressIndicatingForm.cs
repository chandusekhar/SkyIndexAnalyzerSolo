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
    public struct ProgressVisualizationStruct
    {
        public int ID;
        public ProgressBar pbHandle;
        public int tableRowNumber;
        public string description;
        public Label lblHandle;
    }

    public partial class MultipleProgressIndicatingForm : Form
    {
        //private List<ProgressBar> pbList = new List<ProgressBar>();
        //private List<Label> lblList = new List<Label>();
        public List<ProgressVisualizationStruct> progressIndicatorslist = new List<ProgressVisualizationStruct>();

        public MultipleProgressIndicatingForm()
        {
            InitializeComponent();
            //pbList.Add(progressBar1);
            //lblList.Add(label1);
        }



        public ProgressVisualizationStruct AddProgressIndicator(string description = "", int ID = 1)
        {
            tableLayoutPanel1.RowCount += 1;
            ProgressBar pbNew = new ProgressBar();
            Label lblNew = new Label();

            ProgressVisualizationStruct newProgressIndicator = new ProgressVisualizationStruct();
            newProgressIndicator.description = description;
            newProgressIndicator.ID = ID;
            newProgressIndicator.tableRowNumber = tableLayoutPanel1.RowCount;
            newProgressIndicator.pbHandle = pbNew;
            newProgressIndicator.lblHandle = lblNew;

            this.tableLayoutPanel1.Controls.Add(pbNew, 1, newProgressIndicator.tableRowNumber-1);
            this.tableLayoutPanel1.Controls.Add(lblNew, 0, newProgressIndicator.tableRowNumber-1);

            pbNew.Dock = System.Windows.Forms.DockStyle.Fill;
            pbNew.Location = new System.Drawing.Point(441, 3 + 40 * newProgressIndicator.tableRowNumber);
            pbNew.Name = "pb"+ID;
            pbNew.Size = new System.Drawing.Size(432, 34);
            pbNew.Step = 1;


            lblNew.Text = description;
            lblNew.AutoSize = true;
            lblNew.Dock = System.Windows.Forms.DockStyle.Fill;
            lblNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            lblNew.Location = new System.Drawing.Point(3, 40 * newProgressIndicator.tableRowNumber);
            lblNew.Name = "lbl"+ID;
            lblNew.Size = new System.Drawing.Size(432, 40);
            lblNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.tableLayoutPanel1.PerformLayout();


            progressIndicatorslist.Add(newProgressIndicator);

            return newProgressIndicator;
        }




        public void UpdateIndicator(ProgressVisualizationStruct theIndicator, double ratio = 0.0d)
        {
            int perc = Convert.ToInt32(ratio * 100.0d);
            ThreadSafeOperations.UpdateProgressBar(theIndicator.pbHandle, perc);
        }

        private void MultipleProgressIndicatingForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }
    }
}
