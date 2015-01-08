namespace SkyImagesAnalyzerLibraries
{
    partial class HistogramCalcAndShowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pbHistRepresentation = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbBinsCount = new System.Windows.Forms.TextBox();
            this.tbStats = new System.Windows.Forms.TextBox();
            this.cbShowQuantiles = new System.Windows.Forms.CheckBox();
            this.lblQuantilesCount = new System.Windows.Forms.Label();
            this.tbQuantilesCount = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbtnShowAsBars = new System.Windows.Forms.RadioButton();
            this.rbtnShowAsDots = new System.Windows.Forms.RadioButton();
            this.btnSaveImageToFile = new System.Windows.Forms.Button();
            this.btnSaveDataToFile = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHistRepresentation)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel1.Controls.Add(this.pbHistRepresentation, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbBinsCount, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbStats, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.cbShowQuantiles, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblQuantilesCount, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbQuantilesCount, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveImageToFile, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveDataToFile, 2, 9);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1184, 757);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pbHistRepresentation
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbHistRepresentation, 2);
            this.pbHistRepresentation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbHistRepresentation.Location = new System.Drawing.Point(4, 105);
            this.pbHistRepresentation.Margin = new System.Windows.Forms.Padding(4);
            this.pbHistRepresentation.Name = "pbHistRepresentation";
            this.tableLayoutPanel1.SetRowSpan(this.pbHistRepresentation, 9);
            this.pbHistRepresentation.Size = new System.Drawing.Size(910, 648);
            this.pbHistRepresentation.TabIndex = 0;
            this.pbHistRepresentation.TabStop = false;
            this.pbHistRepresentation.Click += new System.EventHandler(this.pbHistRepresentation_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTitle, 2);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(4, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(910, 101);
            this.lblTitle.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(922, 101);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 37);
            this.label2.TabIndex = 2;
            this.label2.Text = "bins count";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbBinsCount
            // 
            this.tbBinsCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBinsCount.Location = new System.Drawing.Point(1055, 105);
            this.tbBinsCount.Margin = new System.Windows.Forms.Padding(4);
            this.tbBinsCount.Name = "tbBinsCount";
            this.tbBinsCount.Size = new System.Drawing.Size(125, 22);
            this.tbBinsCount.TabIndex = 3;
            this.tbBinsCount.Text = "100";
            this.tbBinsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBinsCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbBinsCount_KeyPress);
            // 
            // tbStats
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbStats, 2);
            this.tbStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbStats.Location = new System.Drawing.Point(922, 354);
            this.tbStats.Margin = new System.Windows.Forms.Padding(4);
            this.tbStats.Multiline = true;
            this.tbStats.Name = "tbStats";
            this.tableLayoutPanel1.SetRowSpan(this.tbStats, 2);
            this.tbStats.Size = new System.Drawing.Size(258, 194);
            this.tbStats.TabIndex = 4;
            // 
            // cbShowQuantiles
            // 
            this.cbShowQuantiles.AutoSize = true;
            this.cbShowQuantiles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableLayoutPanel1.SetColumnSpan(this.cbShowQuantiles, 2);
            this.cbShowQuantiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbShowQuantiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbShowQuantiles.Location = new System.Drawing.Point(922, 142);
            this.cbShowQuantiles.Margin = new System.Windows.Forms.Padding(4);
            this.cbShowQuantiles.Name = "cbShowQuantiles";
            this.cbShowQuantiles.Size = new System.Drawing.Size(258, 29);
            this.cbShowQuantiles.TabIndex = 5;
            this.cbShowQuantiles.Text = "Show quantiles";
            this.cbShowQuantiles.UseVisualStyleBackColor = true;
            this.cbShowQuantiles.CheckedChanged += new System.EventHandler(this.cbShowQuantiles_CheckedChanged);
            // 
            // lblQuantilesCount
            // 
            this.lblQuantilesCount.AutoSize = true;
            this.lblQuantilesCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuantilesCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblQuantilesCount.Location = new System.Drawing.Point(922, 175);
            this.lblQuantilesCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQuantilesCount.Name = "lblQuantilesCount";
            this.lblQuantilesCount.Size = new System.Drawing.Size(125, 37);
            this.lblQuantilesCount.TabIndex = 6;
            this.lblQuantilesCount.Text = "quantiles:";
            this.lblQuantilesCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbQuantilesCount
            // 
            this.tbQuantilesCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbQuantilesCount.Location = new System.Drawing.Point(1055, 179);
            this.tbQuantilesCount.Margin = new System.Windows.Forms.Padding(4);
            this.tbQuantilesCount.Name = "tbQuantilesCount";
            this.tbQuantilesCount.Size = new System.Drawing.Size(125, 22);
            this.tbQuantilesCount.TabIndex = 7;
            this.tbQuantilesCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbQuantilesCount_KeyPress);
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.rbtnShowAsBars);
            this.panel1.Controls.Add(this.rbtnShowAsDots);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(922, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 93);
            this.panel1.TabIndex = 10;
            // 
            // rbtnShowAsBars
            // 
            this.rbtnShowAsBars.AutoSize = true;
            this.rbtnShowAsBars.Checked = true;
            this.rbtnShowAsBars.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbtnShowAsBars.Location = new System.Drawing.Point(5, 11);
            this.rbtnShowAsBars.Margin = new System.Windows.Forms.Padding(4);
            this.rbtnShowAsBars.Name = "rbtnShowAsBars";
            this.rbtnShowAsBars.Size = new System.Drawing.Size(180, 29);
            this.rbtnShowAsBars.TabIndex = 9;
            this.rbtnShowAsBars.TabStop = true;
            this.rbtnShowAsBars.Text = "Show as BARS";
            this.rbtnShowAsBars.UseVisualStyleBackColor = true;
            this.rbtnShowAsBars.CheckedChanged += new System.EventHandler(this.rbtnShowAsDots_CheckedChanged);
            // 
            // rbtnShowAsDots
            // 
            this.rbtnShowAsDots.AutoSize = true;
            this.rbtnShowAsDots.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbtnShowAsDots.Location = new System.Drawing.Point(5, 48);
            this.rbtnShowAsDots.Margin = new System.Windows.Forms.Padding(4);
            this.rbtnShowAsDots.Name = "rbtnShowAsDots";
            this.rbtnShowAsDots.Size = new System.Drawing.Size(183, 29);
            this.rbtnShowAsDots.TabIndex = 8;
            this.rbtnShowAsDots.TabStop = true;
            this.rbtnShowAsDots.Text = "Show as DOTS";
            this.rbtnShowAsDots.UseVisualStyleBackColor = true;
            this.rbtnShowAsDots.CheckedChanged += new System.EventHandler(this.rbtnShowAsDots_CheckedChanged);
            // 
            // btnSaveImageToFile
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnSaveImageToFile, 2);
            this.btnSaveImageToFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveImageToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveImageToFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveImageToFile.Location = new System.Drawing.Point(921, 555);
            this.btnSaveImageToFile.Name = "btnSaveImageToFile";
            this.btnSaveImageToFile.Size = new System.Drawing.Size(260, 95);
            this.btnSaveImageToFile.TabIndex = 11;
            this.btnSaveImageToFile.Text = "Save image to file...";
            this.btnSaveImageToFile.UseVisualStyleBackColor = true;
            this.btnSaveImageToFile.Click += new System.EventHandler(this.btnSaveImageToFile_Click);
            // 
            // btnSaveDataToFile
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnSaveDataToFile, 2);
            this.btnSaveDataToFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveDataToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDataToFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDataToFile.Location = new System.Drawing.Point(921, 656);
            this.btnSaveDataToFile.Name = "btnSaveDataToFile";
            this.btnSaveDataToFile.Size = new System.Drawing.Size(260, 98);
            this.btnSaveDataToFile.TabIndex = 12;
            this.btnSaveDataToFile.Text = "Save data to file...";
            this.btnSaveDataToFile.UseVisualStyleBackColor = true;
            this.btnSaveDataToFile.Click += new System.EventHandler(this.btnSaveDataToFile_Click);
            // 
            // HistogramCalcAndShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 757);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HistogramCalcAndShowForm";
            this.Text = "HistogramCalcAndShowForm";
            this.Load += new System.EventHandler(this.HistogramCalcAndShowForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HistogramCalcAndShowForm_KeyPress);
            this.Resize += new System.EventHandler(this.HistogramCalcAndShowForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHistRepresentation)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pbHistRepresentation;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbStats;
        private System.Windows.Forms.Label lblQuantilesCount;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.CheckBox cbShowQuantiles;
        public System.Windows.Forms.RadioButton rbtnShowAsDots;
        public System.Windows.Forms.RadioButton rbtnShowAsBars;
        public System.Windows.Forms.TextBox tbBinsCount;
        public System.Windows.Forms.TextBox tbQuantilesCount;
        private System.Windows.Forms.Button btnSaveImageToFile;
        private System.Windows.Forms.Button btnSaveDataToFile;
    }
}