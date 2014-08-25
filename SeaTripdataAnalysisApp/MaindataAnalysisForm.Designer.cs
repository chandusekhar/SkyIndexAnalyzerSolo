namespace SeaTripdataAnalysisApp
{
    partial class MaindataAnalysisForm
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
            this.btnReadData = new System.Windows.Forms.Button();
            this.lblStatusString = new System.Windows.Forms.Label();
            this.tbLogFilesPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBrowseOutputDirectory = new System.Windows.Forms.Button();
            this.btnBrowseSourceDirectory = new System.Windows.Forms.Button();
            this.tbSourceDirectoryValue = new System.Windows.Forms.TextBox();
            this.tbOutputDirectoryValue = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblSourceDirectoryTitle = new System.Windows.Forms.Label();
            this.lblOutputDirectoryTitle = new System.Windows.Forms.Label();
            this.prbConvertionProgress = new System.Windows.Forms.ProgressBar();
            this.prbReadingProcessingData = new System.Windows.Forms.ProgressBar();
            this.tbReportLog = new System.Windows.Forms.TextBox();
            this.btnProcessAccelerationTimeSeries = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.btnReadData, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatusString, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbLogFilesPath, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.prbReadingProcessingData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbReportLog, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnProcessAccelerationTimeSeries, 6, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1114, 584);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnReadData
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnReadData, 2);
            this.btnReadData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReadData.Location = new System.Drawing.Point(3, 3);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(272, 34);
            this.btnReadData.TabIndex = 0;
            this.btnReadData.Text = "Read data";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // lblStatusString
            // 
            this.lblStatusString.AutoSize = true;
            this.lblStatusString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatusString, 8);
            this.lblStatusString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusString.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusString.Location = new System.Drawing.Point(3, 548);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(1108, 36);
            this.lblStatusString.TabIndex = 1;
            this.lblStatusString.Text = "---";
            this.lblStatusString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbLogFilesPath
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbLogFilesPath, 5);
            this.tbLogFilesPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLogFilesPath.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLogFilesPath.Location = new System.Drawing.Point(281, 3);
            this.tbLogFilesPath.Name = "tbLogFilesPath";
            this.tbLogFilesPath.Size = new System.Drawing.Size(689, 26);
            this.tbLogFilesPath.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(976, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 8);
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(3, 229);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 4);
            this.groupBox1.Size = new System.Drawing.Size(1108, 306);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CONVERT DATA ASCII log -> NetCDF logs";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseOutputDirectory, 5, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseSourceDirectory, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbSourceDirectoryValue, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbOutputDirectoryValue, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnConvert, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblSourceDirectoryTitle, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblOutputDirectoryTitle, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.prbConvertionProgress, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1102, 281);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnBrowseOutputDirectory
            // 
            this.btnBrowseOutputDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseOutputDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseOutputDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseOutputDirectory.Location = new System.Drawing.Point(918, 244);
            this.btnBrowseOutputDirectory.Name = "btnBrowseOutputDirectory";
            this.btnBrowseOutputDirectory.Size = new System.Drawing.Size(181, 34);
            this.btnBrowseOutputDirectory.TabIndex = 7;
            this.btnBrowseOutputDirectory.Text = "Browse...";
            this.btnBrowseOutputDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseOutputDirectory.Click += new System.EventHandler(this.btnBrowseOutputDirectory_Click);
            // 
            // btnBrowseSourceDirectory
            // 
            this.btnBrowseSourceDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseSourceDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseSourceDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseSourceDirectory.Location = new System.Drawing.Point(918, 3);
            this.btnBrowseSourceDirectory.Name = "btnBrowseSourceDirectory";
            this.btnBrowseSourceDirectory.Size = new System.Drawing.Size(181, 34);
            this.btnBrowseSourceDirectory.TabIndex = 6;
            this.btnBrowseSourceDirectory.Text = "Browse...";
            this.btnBrowseSourceDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseSourceDirectory.Click += new System.EventHandler(this.btnBrowseSourceDirectory_Click);
            // 
            // tbSourceDirectoryValue
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tbSourceDirectoryValue, 4);
            this.tbSourceDirectoryValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSourceDirectoryValue.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSourceDirectoryValue.Location = new System.Drawing.Point(186, 3);
            this.tbSourceDirectoryValue.Name = "tbSourceDirectoryValue";
            this.tbSourceDirectoryValue.Size = new System.Drawing.Size(726, 29);
            this.tbSourceDirectoryValue.TabIndex = 8;
            // 
            // tbOutputDirectoryValue
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tbOutputDirectoryValue, 4);
            this.tbOutputDirectoryValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbOutputDirectoryValue.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbOutputDirectoryValue.Location = new System.Drawing.Point(186, 244);
            this.tbOutputDirectoryValue.Name = "tbOutputDirectoryValue";
            this.tbOutputDirectoryValue.Size = new System.Drawing.Size(726, 29);
            this.tbOutputDirectoryValue.TabIndex = 5;
            // 
            // btnConvert
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.btnConvert, 6);
            this.btnConvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvert.Location = new System.Drawing.Point(3, 43);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(1096, 175);
            this.btnConvert.TabIndex = 4;
            this.btnConvert.Text = "CONVERT";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // lblSourceDirectoryTitle
            // 
            this.lblSourceDirectoryTitle.AutoSize = true;
            this.lblSourceDirectoryTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSourceDirectoryTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSourceDirectoryTitle.Location = new System.Drawing.Point(3, 0);
            this.lblSourceDirectoryTitle.Name = "lblSourceDirectoryTitle";
            this.lblSourceDirectoryTitle.Size = new System.Drawing.Size(177, 40);
            this.lblSourceDirectoryTitle.TabIndex = 9;
            this.lblSourceDirectoryTitle.Text = "Source directory:";
            this.lblSourceDirectoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutputDirectoryTitle
            // 
            this.lblOutputDirectoryTitle.AutoSize = true;
            this.lblOutputDirectoryTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutputDirectoryTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOutputDirectoryTitle.Location = new System.Drawing.Point(3, 241);
            this.lblOutputDirectoryTitle.Name = "lblOutputDirectoryTitle";
            this.lblOutputDirectoryTitle.Size = new System.Drawing.Size(177, 40);
            this.lblOutputDirectoryTitle.TabIndex = 10;
            this.lblOutputDirectoryTitle.Text = "output directory:";
            this.lblOutputDirectoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prbConvertionProgress
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.prbConvertionProgress, 6);
            this.prbConvertionProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbConvertionProgress.Location = new System.Drawing.Point(3, 224);
            this.prbConvertionProgress.Name = "prbConvertionProgress";
            this.prbConvertionProgress.Size = new System.Drawing.Size(1096, 14);
            this.prbConvertionProgress.TabIndex = 12;
            // 
            // prbReadingProcessingData
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.prbReadingProcessingData, 8);
            this.prbReadingProcessingData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbReadingProcessingData.Location = new System.Drawing.Point(3, 43);
            this.prbReadingProcessingData.Name = "prbReadingProcessingData";
            this.prbReadingProcessingData.Size = new System.Drawing.Size(1108, 24);
            this.prbReadingProcessingData.TabIndex = 10;
            // 
            // tbReportLog
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbReportLog, 6);
            this.tbReportLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReportLog.Location = new System.Drawing.Point(3, 73);
            this.tbReportLog.Multiline = true;
            this.tbReportLog.Name = "tbReportLog";
            this.tableLayoutPanel1.SetRowSpan(this.tbReportLog, 2);
            this.tbReportLog.Size = new System.Drawing.Size(828, 150);
            this.tbReportLog.TabIndex = 11;
            // 
            // btnProcessAccelerationTimeSeries
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnProcessAccelerationTimeSeries, 2);
            this.btnProcessAccelerationTimeSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProcessAccelerationTimeSeries.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcessAccelerationTimeSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnProcessAccelerationTimeSeries.Location = new System.Drawing.Point(837, 73);
            this.btnProcessAccelerationTimeSeries.Name = "btnProcessAccelerationTimeSeries";
            this.btnProcessAccelerationTimeSeries.Size = new System.Drawing.Size(274, 72);
            this.btnProcessAccelerationTimeSeries.TabIndex = 12;
            this.btnProcessAccelerationTimeSeries.Text = "Process acceleration timeseries";
            this.btnProcessAccelerationTimeSeries.UseVisualStyleBackColor = true;
            this.btnProcessAccelerationTimeSeries.Click += new System.EventHandler(this.btnProcessAccelerationTimeSeries_Click);
            // 
            // MaindataAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 584);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "MaindataAnalysisForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaindataAnalysisForm_FormClosing);
            this.Load += new System.EventHandler(this.MaindataAnalysisForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MaindataAnalysisForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.Label lblStatusString;
        private System.Windows.Forms.TextBox tbLogFilesPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnBrowseOutputDirectory;
        private System.Windows.Forms.Button btnBrowseSourceDirectory;
        private System.Windows.Forms.TextBox tbSourceDirectoryValue;
        private System.Windows.Forms.TextBox tbOutputDirectoryValue;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label lblSourceDirectoryTitle;
        private System.Windows.Forms.Label lblOutputDirectoryTitle;
        private System.Windows.Forms.ProgressBar prbConvertionProgress;
        private System.Windows.Forms.ProgressBar prbReadingProcessingData;
        private System.Windows.Forms.TextBox tbReportLog;
        private System.Windows.Forms.Button btnProcessAccelerationTimeSeries;
    }
}

