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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpConversions = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.prbReadingProcessingData = new System.Windows.Forms.ProgressBar();
            this.tbReportLog = new System.Windows.Forms.TextBox();
            this.btnProcessAccelerationTimeSeries = new System.Windows.Forms.Button();
            this.lblStatusString = new System.Windows.Forms.Label();
            this.btnReadData = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBrowseOutputDirectory = new System.Windows.Forms.Button();
            this.btnBrowseSourceDirectory = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblSourceDirectoryTitle = new System.Windows.Forms.Label();
            this.lblOutputDirectoryTitle = new System.Windows.Forms.Label();
            this.prbConvertionProgress = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.tpExportData = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPerformExport = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rtbExportDestinationDirectoryPath = new System.Windows.Forms.RichTextBox();
            this.btnSelectExportDirectory = new System.Windows.Forms.Button();
            this.groupBoxVariablesToExport = new System.Windows.Forms.GroupBox();
            this.cbExportDynamicsData = new System.Windows.Forms.CheckBox();
            this.cbExportGPSdata = new System.Windows.Forms.CheckBox();
            this.cbExportMeteoData = new System.Windows.Forms.CheckBox();
            this.groupBoxFilesFormatsToWrite = new System.Windows.Forms.GroupBox();
            this.cbExportFormatCSV = new System.Windows.Forms.CheckBox();
            this.cbExportFormatXML = new System.Windows.Forms.CheckBox();
            this.prbExportProgress = new System.Windows.Forms.ProgressBar();
            this.tbLogFilesPath = new System.Windows.Forms.RichTextBox();
            this.tbSourceDirectoryValue = new System.Windows.Forms.RichTextBox();
            this.tbOutputDirectoryValue = new System.Windows.Forms.RichTextBox();
            this.lblStatusBar = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tpConversions.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tpExportData.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBoxVariablesToExport.SuspendLayout();
            this.groupBoxFilesFormatsToWrite.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpConversions);
            this.tabControl1.Controls.Add(this.tpExportData);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1346, 846);
            this.tabControl1.TabIndex = 0;
            // 
            // tpConversions
            // 
            this.tpConversions.BackColor = System.Drawing.SystemColors.Control;
            this.tpConversions.Controls.Add(this.tableLayoutPanel1);
            this.tpConversions.Location = new System.Drawing.Point(4, 25);
            this.tpConversions.Name = "tpConversions";
            this.tpConversions.Padding = new System.Windows.Forms.Padding(3);
            this.tpConversions.Size = new System.Drawing.Size(1338, 817);
            this.tpConversions.TabIndex = 0;
            this.tpConversions.Text = "Conversions";
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
            this.tableLayoutPanel1.Controls.Add(this.button1, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.prbReadingProcessingData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbReportLog, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnProcessAccelerationTimeSeries, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbLogFilesPath, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1332, 811);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // prbReadingProcessingData
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.prbReadingProcessingData, 8);
            this.prbReadingProcessingData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbReadingProcessingData.Location = new System.Drawing.Point(4, 53);
            this.prbReadingProcessingData.Margin = new System.Windows.Forms.Padding(4);
            this.prbReadingProcessingData.Name = "prbReadingProcessingData";
            this.prbReadingProcessingData.Size = new System.Drawing.Size(1324, 29);
            this.prbReadingProcessingData.TabIndex = 10;
            // 
            // tbReportLog
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbReportLog, 6);
            this.tbReportLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReportLog.Location = new System.Drawing.Point(4, 90);
            this.tbReportLog.Margin = new System.Windows.Forms.Padding(4);
            this.tbReportLog.Multiline = true;
            this.tbReportLog.Name = "tbReportLog";
            this.tableLayoutPanel1.SetRowSpan(this.tbReportLog, 2);
            this.tbReportLog.Size = new System.Drawing.Size(988, 216);
            this.tbReportLog.TabIndex = 11;
            // 
            // btnProcessAccelerationTimeSeries
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnProcessAccelerationTimeSeries, 2);
            this.btnProcessAccelerationTimeSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProcessAccelerationTimeSeries.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcessAccelerationTimeSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnProcessAccelerationTimeSeries.Location = new System.Drawing.Point(1000, 90);
            this.btnProcessAccelerationTimeSeries.Margin = new System.Windows.Forms.Padding(4);
            this.btnProcessAccelerationTimeSeries.Name = "btnProcessAccelerationTimeSeries";
            this.btnProcessAccelerationTimeSeries.Size = new System.Drawing.Size(328, 104);
            this.btnProcessAccelerationTimeSeries.TabIndex = 12;
            this.btnProcessAccelerationTimeSeries.Text = "Process acceleration timeseries";
            this.btnProcessAccelerationTimeSeries.UseVisualStyleBackColor = true;
            this.btnProcessAccelerationTimeSeries.Click += new System.EventHandler(this.btnProcessAccelerationTimeSeries_Click);
            // 
            // lblStatusString
            // 
            this.lblStatusString.AutoSize = true;
            this.lblStatusString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatusString, 8);
            this.lblStatusString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusString.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusString.Location = new System.Drawing.Point(4, 770);
            this.lblStatusString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(1324, 41);
            this.lblStatusString.TabIndex = 1;
            this.lblStatusString.Text = "---";
            this.lblStatusString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnReadData
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnReadData, 2);
            this.btnReadData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReadData.Location = new System.Drawing.Point(4, 4);
            this.btnReadData.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(324, 41);
            this.btnReadData.TabIndex = 0;
            this.btnReadData.Text = "Read data";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 8);
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(4, 314);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 4);
            this.groupBox1.Size = new System.Drawing.Size(1324, 440);
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
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseOutputDirectory, 5, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseSourceDirectory, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnConvert, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblSourceDirectoryTitle, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblOutputDirectoryTitle, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.prbConvertionProgress, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbSourceDirectoryValue, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbOutputDirectoryValue, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 27);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1316, 409);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnBrowseOutputDirectory
            // 
            this.btnBrowseOutputDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseOutputDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseOutputDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseOutputDirectory.Location = new System.Drawing.Point(1099, 364);
            this.btnBrowseOutputDirectory.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseOutputDirectory.Name = "btnBrowseOutputDirectory";
            this.btnBrowseOutputDirectory.Size = new System.Drawing.Size(213, 41);
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
            this.btnBrowseSourceDirectory.Location = new System.Drawing.Point(1099, 4);
            this.btnBrowseSourceDirectory.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseSourceDirectory.Name = "btnBrowseSourceDirectory";
            this.btnBrowseSourceDirectory.Size = new System.Drawing.Size(213, 41);
            this.btnBrowseSourceDirectory.TabIndex = 6;
            this.btnBrowseSourceDirectory.Text = "Browse...";
            this.btnBrowseSourceDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseSourceDirectory.Click += new System.EventHandler(this.btnBrowseSourceDirectory_Click);
            // 
            // btnConvert
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.btnConvert, 6);
            this.btnConvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvert.Location = new System.Drawing.Point(4, 53);
            this.btnConvert.Margin = new System.Windows.Forms.Padding(4);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(1308, 278);
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
            this.lblSourceDirectoryTitle.Location = new System.Drawing.Point(4, 0);
            this.lblSourceDirectoryTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceDirectoryTitle.Name = "lblSourceDirectoryTitle";
            this.lblSourceDirectoryTitle.Size = new System.Drawing.Size(211, 49);
            this.lblSourceDirectoryTitle.TabIndex = 9;
            this.lblSourceDirectoryTitle.Text = "Source directory:";
            this.lblSourceDirectoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutputDirectoryTitle
            // 
            this.lblOutputDirectoryTitle.AutoSize = true;
            this.lblOutputDirectoryTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutputDirectoryTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOutputDirectoryTitle.Location = new System.Drawing.Point(4, 360);
            this.lblOutputDirectoryTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputDirectoryTitle.Name = "lblOutputDirectoryTitle";
            this.lblOutputDirectoryTitle.Size = new System.Drawing.Size(211, 49);
            this.lblOutputDirectoryTitle.TabIndex = 10;
            this.lblOutputDirectoryTitle.Text = "output directory:";
            this.lblOutputDirectoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prbConvertionProgress
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.prbConvertionProgress, 6);
            this.prbConvertionProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbConvertionProgress.Location = new System.Drawing.Point(4, 339);
            this.prbConvertionProgress.Margin = new System.Windows.Forms.Padding(4);
            this.prbConvertionProgress.Name = "prbConvertionProgress";
            this.prbConvertionProgress.Size = new System.Drawing.Size(1308, 17);
            this.prbConvertionProgress.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(1166, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 41);
            this.button1.TabIndex = 3;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tpExportData
            // 
            this.tpExportData.BackColor = System.Drawing.SystemColors.Control;
            this.tpExportData.Controls.Add(this.tableLayoutPanel3);
            this.tpExportData.Location = new System.Drawing.Point(4, 25);
            this.tpExportData.Name = "tpExportData";
            this.tpExportData.Padding = new System.Windows.Forms.Padding(3);
            this.tpExportData.Size = new System.Drawing.Size(1338, 817);
            this.tpExportData.TabIndex = 1;
            this.tpExportData.Text = "Data export";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.Controls.Add(this.btnPerformExport, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.rtbExportDestinationDirectoryPath, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnSelectExportDirectory, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.groupBoxVariablesToExport, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.groupBoxFilesFormatsToWrite, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.prbExportProgress, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblStatusBar, 0, 6);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 7;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1332, 811);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // btnPerformExport
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnPerformExport, 3);
            this.btnPerformExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPerformExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPerformExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPerformExport.Location = new System.Drawing.Point(3, 183);
            this.btnPerformExport.Name = "btnPerformExport";
            this.btnPerformExport.Size = new System.Drawing.Size(1326, 585);
            this.btnPerformExport.TabIndex = 0;
            this.btnPerformExport.Text = "Export";
            this.btnPerformExport.UseVisualStyleBackColor = true;
            this.btnPerformExport.Click += new System.EventHandler(this.btnPerformExport_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label4, 3);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1326, 40);
            this.label4.TabIndex = 0;
            this.label4.Text = "Export variables data";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(3, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(194, 40);
            this.label5.TabIndex = 1;
            this.label5.Text = "Destination directory:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbExportDestinationDirectoryPath
            // 
            this.rtbExportDestinationDirectoryPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbExportDestinationDirectoryPath.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbExportDestinationDirectoryPath.Location = new System.Drawing.Point(203, 43);
            this.rtbExportDestinationDirectoryPath.Name = "rtbExportDestinationDirectoryPath";
            this.rtbExportDestinationDirectoryPath.Size = new System.Drawing.Size(1066, 34);
            this.rtbExportDestinationDirectoryPath.TabIndex = 2;
            this.rtbExportDestinationDirectoryPath.Text = "";
            // 
            // btnSelectExportDirectory
            // 
            this.btnSelectExportDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectExportDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectExportDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSelectExportDirectory.Location = new System.Drawing.Point(1275, 43);
            this.btnSelectExportDirectory.Name = "btnSelectExportDirectory";
            this.btnSelectExportDirectory.Size = new System.Drawing.Size(54, 34);
            this.btnSelectExportDirectory.TabIndex = 3;
            this.btnSelectExportDirectory.Text = "...";
            this.btnSelectExportDirectory.UseVisualStyleBackColor = true;
            this.btnSelectExportDirectory.Click += new System.EventHandler(this.btnSelectExportDirectory_Click);
            // 
            // groupBoxVariablesToExport
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.groupBoxVariablesToExport, 3);
            this.groupBoxVariablesToExport.Controls.Add(this.cbExportDynamicsData);
            this.groupBoxVariablesToExport.Controls.Add(this.cbExportGPSdata);
            this.groupBoxVariablesToExport.Controls.Add(this.cbExportMeteoData);
            this.groupBoxVariablesToExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxVariablesToExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxVariablesToExport.Location = new System.Drawing.Point(3, 83);
            this.groupBoxVariablesToExport.Name = "groupBoxVariablesToExport";
            this.groupBoxVariablesToExport.Size = new System.Drawing.Size(1326, 34);
            this.groupBoxVariablesToExport.TabIndex = 4;
            this.groupBoxVariablesToExport.TabStop = false;
            this.groupBoxVariablesToExport.Text = "Variables to export";
            // 
            // cbExportDynamicsData
            // 
            this.cbExportDynamicsData.AutoSize = true;
            this.cbExportDynamicsData.Location = new System.Drawing.Point(461, 3);
            this.cbExportDynamicsData.Name = "cbExportDynamicsData";
            this.cbExportDynamicsData.Size = new System.Drawing.Size(128, 29);
            this.cbExportDynamicsData.TabIndex = 2;
            this.cbExportDynamicsData.Text = "accel, gyro";
            this.cbExportDynamicsData.UseVisualStyleBackColor = true;
            // 
            // cbExportGPSdata
            // 
            this.cbExportGPSdata.AutoSize = true;
            this.cbExportGPSdata.Checked = true;
            this.cbExportGPSdata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExportGPSdata.Location = new System.Drawing.Point(336, 3);
            this.cbExportGPSdata.Name = "cbExportGPSdata";
            this.cbExportGPSdata.Size = new System.Drawing.Size(119, 29);
            this.cbExportGPSdata.TabIndex = 1;
            this.cbExportGPSdata.Text = "GPS data";
            this.cbExportGPSdata.UseVisualStyleBackColor = true;
            // 
            // cbExportMeteoData
            // 
            this.cbExportMeteoData.AutoSize = true;
            this.cbExportMeteoData.Checked = true;
            this.cbExportMeteoData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExportMeteoData.Location = new System.Drawing.Point(199, 3);
            this.cbExportMeteoData.Name = "cbExportMeteoData";
            this.cbExportMeteoData.Size = new System.Drawing.Size(131, 29);
            this.cbExportMeteoData.TabIndex = 0;
            this.cbExportMeteoData.Text = "meteo data";
            this.cbExportMeteoData.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilesFormatsToWrite
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.groupBoxFilesFormatsToWrite, 3);
            this.groupBoxFilesFormatsToWrite.Controls.Add(this.cbExportFormatCSV);
            this.groupBoxFilesFormatsToWrite.Controls.Add(this.cbExportFormatXML);
            this.groupBoxFilesFormatsToWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFilesFormatsToWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFilesFormatsToWrite.Location = new System.Drawing.Point(3, 123);
            this.groupBoxFilesFormatsToWrite.Name = "groupBoxFilesFormatsToWrite";
            this.groupBoxFilesFormatsToWrite.Size = new System.Drawing.Size(1326, 34);
            this.groupBoxFilesFormatsToWrite.TabIndex = 5;
            this.groupBoxFilesFormatsToWrite.TabStop = false;
            this.groupBoxFilesFormatsToWrite.Text = "File formats to write";
            // 
            // cbExportFormatCSV
            // 
            this.cbExportFormatCSV.AutoSize = true;
            this.cbExportFormatCSV.Checked = true;
            this.cbExportFormatCSV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExportFormatCSV.Location = new System.Drawing.Point(281, 5);
            this.cbExportFormatCSV.Name = "cbExportFormatCSV";
            this.cbExportFormatCSV.Size = new System.Drawing.Size(77, 29);
            this.cbExportFormatCSV.TabIndex = 1;
            this.cbExportFormatCSV.Text = "CSV";
            this.cbExportFormatCSV.UseVisualStyleBackColor = true;
            // 
            // cbExportFormatXML
            // 
            this.cbExportFormatXML.AutoSize = true;
            this.cbExportFormatXML.Location = new System.Drawing.Point(199, 5);
            this.cbExportFormatXML.Name = "cbExportFormatXML";
            this.cbExportFormatXML.Size = new System.Drawing.Size(76, 29);
            this.cbExportFormatXML.TabIndex = 0;
            this.cbExportFormatXML.Text = "XML";
            this.cbExportFormatXML.UseVisualStyleBackColor = true;
            // 
            // prbExportProgress
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.prbExportProgress, 3);
            this.prbExportProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbExportProgress.Location = new System.Drawing.Point(3, 163);
            this.prbExportProgress.Name = "prbExportProgress";
            this.prbExportProgress.Size = new System.Drawing.Size(1326, 14);
            this.prbExportProgress.TabIndex = 6;
            // 
            // tbLogFilesPath
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbLogFilesPath, 5);
            this.tbLogFilesPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLogFilesPath.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLogFilesPath.Location = new System.Drawing.Point(335, 3);
            this.tbLogFilesPath.Name = "tbLogFilesPath";
            this.tbLogFilesPath.Size = new System.Drawing.Size(824, 43);
            this.tbLogFilesPath.TabIndex = 13;
            this.tbLogFilesPath.Text = "";
            // 
            // tbSourceDirectoryValue
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tbSourceDirectoryValue, 4);
            this.tbSourceDirectoryValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSourceDirectoryValue.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSourceDirectoryValue.Location = new System.Drawing.Point(222, 3);
            this.tbSourceDirectoryValue.Name = "tbSourceDirectoryValue";
            this.tbSourceDirectoryValue.Size = new System.Drawing.Size(870, 43);
            this.tbSourceDirectoryValue.TabIndex = 13;
            this.tbSourceDirectoryValue.Text = "";
            // 
            // tbOutputDirectoryValue
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tbOutputDirectoryValue, 4);
            this.tbOutputDirectoryValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbOutputDirectoryValue.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbOutputDirectoryValue.Location = new System.Drawing.Point(222, 363);
            this.tbOutputDirectoryValue.Name = "tbOutputDirectoryValue";
            this.tbOutputDirectoryValue.Size = new System.Drawing.Size(870, 43);
            this.tbOutputDirectoryValue.TabIndex = 14;
            this.tbOutputDirectoryValue.Text = "";
            // 
            // lblStatusBar
            // 
            this.lblStatusBar.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.lblStatusBar, 3);
            this.lblStatusBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusBar.Location = new System.Drawing.Point(3, 771);
            this.lblStatusBar.Name = "lblStatusBar";
            this.lblStatusBar.Size = new System.Drawing.Size(1326, 40);
            this.lblStatusBar.TabIndex = 7;
            this.lblStatusBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MaindataAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 846);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MaindataAnalysisForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaindataAnalysisForm_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MaindataAnalysisForm_KeyPress);
            this.tabControl1.ResumeLayout(false);
            this.tpConversions.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tpExportData.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBoxVariablesToExport.ResumeLayout(false);
            this.groupBoxVariablesToExport.PerformLayout();
            this.groupBoxFilesFormatsToWrite.ResumeLayout(false);
            this.groupBoxFilesFormatsToWrite.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpConversions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.Label lblStatusString;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnBrowseOutputDirectory;
        private System.Windows.Forms.Button btnBrowseSourceDirectory;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label lblSourceDirectoryTitle;
        private System.Windows.Forms.Label lblOutputDirectoryTitle;
        private System.Windows.Forms.ProgressBar prbConvertionProgress;
        private System.Windows.Forms.ProgressBar prbReadingProcessingData;
        private System.Windows.Forms.TextBox tbReportLog;
        private System.Windows.Forms.Button btnProcessAccelerationTimeSeries;
        private System.Windows.Forms.TabPage tpExportData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnPerformExport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rtbExportDestinationDirectoryPath;
        private System.Windows.Forms.Button btnSelectExportDirectory;
        private System.Windows.Forms.GroupBox groupBoxVariablesToExport;
        private System.Windows.Forms.CheckBox cbExportDynamicsData;
        private System.Windows.Forms.CheckBox cbExportGPSdata;
        private System.Windows.Forms.CheckBox cbExportMeteoData;
        private System.Windows.Forms.GroupBox groupBoxFilesFormatsToWrite;
        private System.Windows.Forms.ProgressBar prbExportProgress;
        private System.Windows.Forms.CheckBox cbExportFormatCSV;
        private System.Windows.Forms.CheckBox cbExportFormatXML;
        private System.Windows.Forms.RichTextBox tbSourceDirectoryValue;
        private System.Windows.Forms.RichTextBox tbOutputDirectoryValue;
        private System.Windows.Forms.RichTextBox tbLogFilesPath;
        private System.Windows.Forms.Label lblStatusBar;
    }
}

