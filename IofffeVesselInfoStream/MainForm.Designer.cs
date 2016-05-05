namespace IofffeVesselInfoStream
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.bgwSocketStreamReader = new System.ComponentModel.BackgroundWorker();
            this.bgwStreamDataProcessing = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpNavAndMeteo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblGPSlatTitle = new System.Windows.Forms.Label();
            this.lblGPSlonTitle = new System.Windows.Forms.Label();
            this.lblGPSDateTimeTitle = new System.Windows.Forms.Label();
            this.lblTrueHeadTitle = new System.Windows.Forms.Label();
            this.lblGyroHeadTitle = new System.Windows.Forms.Label();
            this.lblSpeedKnotsTitle = new System.Windows.Forms.Label();
            this.lblDepthTitle = new System.Windows.Forms.Label();
            this.tbGPSDateTimeValue = new System.Windows.Forms.TextBox();
            this.tbGPSlonValue = new System.Windows.Forms.TextBox();
            this.tbGPSlatValue = new System.Windows.Forms.TextBox();
            this.tbTrueHeadValue = new System.Windows.Forms.TextBox();
            this.tbGyroHeadValue = new System.Windows.Forms.TextBox();
            this.tbSpeedKnotsValue = new System.Windows.Forms.TextBox();
            this.tbDepthValue = new System.Windows.Forms.TextBox();
            this.lblPressureTitle = new System.Windows.Forms.Label();
            this.lblAirTemperatureTitle = new System.Windows.Forms.Label();
            this.lblWindSpeedTitle = new System.Windows.Forms.Label();
            this.lblWindDirectionTitle = new System.Windows.Forms.Label();
            this.lblRelHumidityTitle = new System.Windows.Forms.Label();
            this.tbPressureValue = new System.Windows.Forms.TextBox();
            this.tbAirTemperatureValue = new System.Windows.Forms.TextBox();
            this.tbWindSpeedValue = new System.Windows.Forms.TextBox();
            this.tbWindDirectionValue = new System.Windows.Forms.TextBox();
            this.tbRelHumidityValue = new System.Windows.Forms.TextBox();
            this.lblWaterTemperatureTitle = new System.Windows.Forms.Label();
            this.lblWaterSalinityTitle = new System.Windows.Forms.Label();
            this.tbWaterTemperatureValue = new System.Windows.Forms.TextBox();
            this.tbWaterSalinityValue = new System.Windows.Forms.TextBox();
            this.lblStatusString = new System.Windows.Forms.Label();
            this.cbLogNCdata = new System.Windows.Forms.CheckBox();
            this.cbLogMeasurementsData = new System.Windows.Forms.CheckBox();
            this.btnProperties = new System.Windows.Forms.Button();
            this.lblNavDataSpeedControl = new System.Windows.Forms.Label();
            this.lblMeteoDataSpeedControl = new System.Windows.Forms.Label();
            this.btnShowSunPositionData = new System.Windows.Forms.Button();
            this.btnShowGeoTrack = new System.Windows.Forms.Button();
            this.btnShowMeteoData = new System.Windows.Forms.Button();
            this.tpNavAndMeteoOverview = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.btnFindInfo = new System.Windows.Forms.Button();
            this.btnWriteFile = new System.Windows.Forms.Button();
            this.lblFoundNavDataTitle = new System.Windows.Forms.Label();
            this.lblFoundMeteoDataTitle = new System.Windows.Forms.Label();
            this.prbSearchingProgress = new System.Windows.Forms.ProgressBar();
            this.lblFoundNavData = new System.Windows.Forms.TextBox();
            this.lblFoundMetData = new System.Windows.Forms.TextBox();
            this.lblPlusOneHour = new System.Windows.Forms.Button();
            this.btnPlusOneDay = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
            this.btnFindIOFFEdataFiles = new System.Windows.Forms.Button();
            this.btnRepairAndWriteNCfile = new System.Windows.Forms.Button();
            this.prbMissingInfoSearchingProgress = new System.Windows.Forms.ProgressBar();
            this.maskedTextBox3 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox4 = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFindDataToExport = new System.Windows.Forms.Button();
            this.btnExportData = new System.Windows.Forms.Button();
            this.prbExportDataProgress = new System.Windows.Forms.ProgressBar();
            this.tabControl1.SuspendLayout();
            this.tpNavAndMeteo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tpNavAndMeteoOverview.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgwSocketStreamReader
            // 
            this.bgwSocketStreamReader.WorkerSupportsCancellation = true;
            this.bgwSocketStreamReader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSocketStreamReader_DoWork);
            // 
            // bgwStreamDataProcessing
            // 
            this.bgwStreamDataProcessing.WorkerSupportsCancellation = true;
            this.bgwStreamDataProcessing.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwStreamDataProcessing_DoWork);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpNavAndMeteo);
            this.tabControl1.Controls.Add(this.tpNavAndMeteoOverview);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1451, 750);
            this.tabControl1.TabIndex = 0;
            // 
            // tpNavAndMeteo
            // 
            this.tpNavAndMeteo.BackColor = System.Drawing.SystemColors.Control;
            this.tpNavAndMeteo.Controls.Add(this.tableLayoutPanel1);
            this.tpNavAndMeteo.Location = new System.Drawing.Point(4, 29);
            this.tpNavAndMeteo.Margin = new System.Windows.Forms.Padding(4);
            this.tpNavAndMeteo.Name = "tpNavAndMeteo";
            this.tpNavAndMeteo.Padding = new System.Windows.Forms.Padding(4);
            this.tpNavAndMeteo.Size = new System.Drawing.Size(1443, 717);
            this.tpNavAndMeteo.TabIndex = 0;
            this.tpNavAndMeteo.Text = "Nav&Meteo";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel1.Controls.Add(this.btnConnect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblGPSlatTitle, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblGPSlonTitle, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblGPSDateTimeTitle, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTrueHeadTitle, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblGyroHeadTitle, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblSpeedKnotsTitle, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblDepthTitle, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbGPSDateTimeValue, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbGPSlonValue, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbGPSlatValue, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbTrueHeadValue, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbGyroHeadValue, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbSpeedKnotsValue, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbDepthValue, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblPressureTitle, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblAirTemperatureTitle, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblWindSpeedTitle, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblWindDirectionTitle, 5, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblRelHumidityTitle, 5, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbPressureValue, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbAirTemperatureValue, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbWindSpeedValue, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbWindDirectionValue, 6, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbRelHumidityValue, 6, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblWaterTemperatureTitle, 5, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblWaterSalinityTitle, 5, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbWaterTemperatureValue, 6, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbWaterSalinityValue, 6, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblStatusString, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.cbLogNCdata, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbLogMeasurementsData, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNavDataSpeedControl, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblMeteoDataSpeedControl, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnShowSunPositionData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnShowGeoTrack, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.btnShowMeteoData, 5, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1435, 709);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnConnect, 7);
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnConnect.ForeColor = System.Drawing.Color.Red;
            this.btnConnect.Location = new System.Drawing.Point(4, 4);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(1346, 41);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblGPSlatTitle
            // 
            this.lblGPSlatTitle.AutoSize = true;
            this.lblGPSlatTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblGPSlatTitle, 2);
            this.lblGPSlatTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGPSlatTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGPSlatTitle.Location = new System.Drawing.Point(4, 172);
            this.lblGPSlatTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPSlatTitle.Name = "lblGPSlatTitle";
            this.lblGPSlatTitle.Size = new System.Drawing.Size(271, 37);
            this.lblGPSlatTitle.TabIndex = 2;
            this.lblGPSlatTitle.Text = "GPS lat";
            this.lblGPSlatTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGPSlonTitle
            // 
            this.lblGPSlonTitle.AutoSize = true;
            this.lblGPSlonTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblGPSlonTitle, 2);
            this.lblGPSlonTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGPSlonTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGPSlonTitle.Location = new System.Drawing.Point(4, 209);
            this.lblGPSlonTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPSlonTitle.Name = "lblGPSlonTitle";
            this.lblGPSlonTitle.Size = new System.Drawing.Size(271, 37);
            this.lblGPSlonTitle.TabIndex = 3;
            this.lblGPSlonTitle.Text = "GPS lon";
            this.lblGPSlonTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGPSDateTimeTitle
            // 
            this.lblGPSDateTimeTitle.AutoSize = true;
            this.lblGPSDateTimeTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblGPSDateTimeTitle, 2);
            this.lblGPSDateTimeTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGPSDateTimeTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGPSDateTimeTitle.Location = new System.Drawing.Point(4, 135);
            this.lblGPSDateTimeTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPSDateTimeTitle.Name = "lblGPSDateTimeTitle";
            this.lblGPSDateTimeTitle.Size = new System.Drawing.Size(271, 37);
            this.lblGPSDateTimeTitle.TabIndex = 4;
            this.lblGPSDateTimeTitle.Text = "GPS date";
            this.lblGPSDateTimeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrueHeadTitle
            // 
            this.lblTrueHeadTitle.AutoSize = true;
            this.lblTrueHeadTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTrueHeadTitle, 2);
            this.lblTrueHeadTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTrueHeadTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTrueHeadTitle.Location = new System.Drawing.Point(4, 246);
            this.lblTrueHeadTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrueHeadTitle.Name = "lblTrueHeadTitle";
            this.lblTrueHeadTitle.Size = new System.Drawing.Size(271, 37);
            this.lblTrueHeadTitle.TabIndex = 8;
            this.lblTrueHeadTitle.Text = "Head.(true)";
            this.lblTrueHeadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGyroHeadTitle
            // 
            this.lblGyroHeadTitle.AutoSize = true;
            this.lblGyroHeadTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblGyroHeadTitle, 2);
            this.lblGyroHeadTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGyroHeadTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGyroHeadTitle.Location = new System.Drawing.Point(4, 283);
            this.lblGyroHeadTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGyroHeadTitle.Name = "lblGyroHeadTitle";
            this.lblGyroHeadTitle.Size = new System.Drawing.Size(271, 37);
            this.lblGyroHeadTitle.TabIndex = 9;
            this.lblGyroHeadTitle.Text = "Head.(gyro)";
            this.lblGyroHeadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSpeedKnotsTitle
            // 
            this.lblSpeedKnotsTitle.AutoSize = true;
            this.lblSpeedKnotsTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSpeedKnotsTitle, 2);
            this.lblSpeedKnotsTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSpeedKnotsTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpeedKnotsTitle.Location = new System.Drawing.Point(4, 320);
            this.lblSpeedKnotsTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpeedKnotsTitle.Name = "lblSpeedKnotsTitle";
            this.lblSpeedKnotsTitle.Size = new System.Drawing.Size(271, 37);
            this.lblSpeedKnotsTitle.TabIndex = 10;
            this.lblSpeedKnotsTitle.Text = "Speed (kn)";
            this.lblSpeedKnotsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDepthTitle
            // 
            this.lblDepthTitle.AutoSize = true;
            this.lblDepthTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblDepthTitle, 2);
            this.lblDepthTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDepthTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDepthTitle.Location = new System.Drawing.Point(4, 357);
            this.lblDepthTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDepthTitle.Name = "lblDepthTitle";
            this.lblDepthTitle.Size = new System.Drawing.Size(271, 37);
            this.lblDepthTitle.TabIndex = 11;
            this.lblDepthTitle.Text = "Depth";
            this.lblDepthTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbGPSDateTimeValue
            // 
            this.tbGPSDateTimeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbGPSDateTimeValue, 2);
            this.tbGPSDateTimeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGPSDateTimeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGPSDateTimeValue.Location = new System.Drawing.Point(283, 139);
            this.tbGPSDateTimeValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbGPSDateTimeValue.Multiline = true;
            this.tbGPSDateTimeValue.Name = "tbGPSDateTimeValue";
            this.tbGPSDateTimeValue.ReadOnly = true;
            this.tbGPSDateTimeValue.Size = new System.Drawing.Size(417, 29);
            this.tbGPSDateTimeValue.TabIndex = 12;
            this.tbGPSDateTimeValue.Text = "---";
            this.tbGPSDateTimeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbGPSlonValue
            // 
            this.tbGPSlonValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbGPSlonValue, 2);
            this.tbGPSlonValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGPSlonValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGPSlonValue.Location = new System.Drawing.Point(283, 213);
            this.tbGPSlonValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbGPSlonValue.Multiline = true;
            this.tbGPSlonValue.Name = "tbGPSlonValue";
            this.tbGPSlonValue.ReadOnly = true;
            this.tbGPSlonValue.Size = new System.Drawing.Size(417, 29);
            this.tbGPSlonValue.TabIndex = 13;
            this.tbGPSlonValue.Text = "---";
            this.tbGPSlonValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbGPSlatValue
            // 
            this.tbGPSlatValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbGPSlatValue, 2);
            this.tbGPSlatValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGPSlatValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGPSlatValue.Location = new System.Drawing.Point(283, 176);
            this.tbGPSlatValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbGPSlatValue.Multiline = true;
            this.tbGPSlatValue.Name = "tbGPSlatValue";
            this.tbGPSlatValue.ReadOnly = true;
            this.tbGPSlatValue.Size = new System.Drawing.Size(417, 29);
            this.tbGPSlatValue.TabIndex = 14;
            this.tbGPSlatValue.Text = "---";
            this.tbGPSlatValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbTrueHeadValue
            // 
            this.tbTrueHeadValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbTrueHeadValue, 2);
            this.tbTrueHeadValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTrueHeadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTrueHeadValue.Location = new System.Drawing.Point(283, 250);
            this.tbTrueHeadValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbTrueHeadValue.Multiline = true;
            this.tbTrueHeadValue.Name = "tbTrueHeadValue";
            this.tbTrueHeadValue.ReadOnly = true;
            this.tbTrueHeadValue.Size = new System.Drawing.Size(417, 29);
            this.tbTrueHeadValue.TabIndex = 15;
            this.tbTrueHeadValue.Text = "---";
            this.tbTrueHeadValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbGyroHeadValue
            // 
            this.tbGyroHeadValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbGyroHeadValue, 2);
            this.tbGyroHeadValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGyroHeadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGyroHeadValue.Location = new System.Drawing.Point(283, 287);
            this.tbGyroHeadValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbGyroHeadValue.Multiline = true;
            this.tbGyroHeadValue.Name = "tbGyroHeadValue";
            this.tbGyroHeadValue.ReadOnly = true;
            this.tbGyroHeadValue.Size = new System.Drawing.Size(417, 29);
            this.tbGyroHeadValue.TabIndex = 16;
            this.tbGyroHeadValue.Text = "---";
            this.tbGyroHeadValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbSpeedKnotsValue
            // 
            this.tbSpeedKnotsValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbSpeedKnotsValue, 2);
            this.tbSpeedKnotsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSpeedKnotsValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSpeedKnotsValue.Location = new System.Drawing.Point(283, 324);
            this.tbSpeedKnotsValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbSpeedKnotsValue.Multiline = true;
            this.tbSpeedKnotsValue.Name = "tbSpeedKnotsValue";
            this.tbSpeedKnotsValue.ReadOnly = true;
            this.tbSpeedKnotsValue.Size = new System.Drawing.Size(417, 29);
            this.tbSpeedKnotsValue.TabIndex = 17;
            this.tbSpeedKnotsValue.Text = "---";
            this.tbSpeedKnotsValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbDepthValue
            // 
            this.tbDepthValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbDepthValue, 2);
            this.tbDepthValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDepthValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDepthValue.Location = new System.Drawing.Point(283, 361);
            this.tbDepthValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbDepthValue.Multiline = true;
            this.tbDepthValue.Name = "tbDepthValue";
            this.tbDepthValue.ReadOnly = true;
            this.tbDepthValue.Size = new System.Drawing.Size(417, 29);
            this.tbDepthValue.TabIndex = 18;
            this.tbDepthValue.Text = "---";
            this.tbDepthValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPressureTitle
            // 
            this.lblPressureTitle.AutoSize = true;
            this.lblPressureTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPressureTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPressureTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPressureTitle.Location = new System.Drawing.Point(761, 135);
            this.lblPressureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPressureTitle.Name = "lblPressureTitle";
            this.lblPressureTitle.Size = new System.Drawing.Size(231, 37);
            this.lblPressureTitle.TabIndex = 19;
            this.lblPressureTitle.Text = "Pressure, hPa";
            this.lblPressureTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAirTemperatureTitle
            // 
            this.lblAirTemperatureTitle.AutoSize = true;
            this.lblAirTemperatureTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAirTemperatureTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAirTemperatureTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAirTemperatureTitle.Location = new System.Drawing.Point(761, 172);
            this.lblAirTemperatureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAirTemperatureTitle.Name = "lblAirTemperatureTitle";
            this.lblAirTemperatureTitle.Size = new System.Drawing.Size(231, 37);
            this.lblAirTemperatureTitle.TabIndex = 20;
            this.lblAirTemperatureTitle.Text = "Air temp.";
            this.lblAirTemperatureTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWindSpeedTitle
            // 
            this.lblWindSpeedTitle.AutoSize = true;
            this.lblWindSpeedTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWindSpeedTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWindSpeedTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWindSpeedTitle.Location = new System.Drawing.Point(761, 209);
            this.lblWindSpeedTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindSpeedTitle.Name = "lblWindSpeedTitle";
            this.lblWindSpeedTitle.Size = new System.Drawing.Size(231, 37);
            this.lblWindSpeedTitle.TabIndex = 21;
            this.lblWindSpeedTitle.Text = "Wind speed";
            this.lblWindSpeedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWindDirectionTitle
            // 
            this.lblWindDirectionTitle.AutoSize = true;
            this.lblWindDirectionTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWindDirectionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWindDirectionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWindDirectionTitle.Location = new System.Drawing.Point(761, 246);
            this.lblWindDirectionTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindDirectionTitle.Name = "lblWindDirectionTitle";
            this.lblWindDirectionTitle.Size = new System.Drawing.Size(231, 37);
            this.lblWindDirectionTitle.TabIndex = 22;
            this.lblWindDirectionTitle.Text = "Wind direction";
            this.lblWindDirectionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRelHumidityTitle
            // 
            this.lblRelHumidityTitle.AutoSize = true;
            this.lblRelHumidityTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRelHumidityTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRelHumidityTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRelHumidityTitle.Location = new System.Drawing.Point(761, 283);
            this.lblRelHumidityTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRelHumidityTitle.Name = "lblRelHumidityTitle";
            this.lblRelHumidityTitle.Size = new System.Drawing.Size(231, 37);
            this.lblRelHumidityTitle.TabIndex = 23;
            this.lblRelHumidityTitle.Text = "Rel. humidity";
            this.lblRelHumidityTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPressureValue
            // 
            this.tbPressureValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbPressureValue, 2);
            this.tbPressureValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPressureValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPressureValue.Location = new System.Drawing.Point(1000, 139);
            this.tbPressureValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbPressureValue.Multiline = true;
            this.tbPressureValue.Name = "tbPressureValue";
            this.tbPressureValue.ReadOnly = true;
            this.tbPressureValue.Size = new System.Drawing.Size(431, 29);
            this.tbPressureValue.TabIndex = 24;
            this.tbPressureValue.Text = "---";
            this.tbPressureValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbAirTemperatureValue
            // 
            this.tbAirTemperatureValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbAirTemperatureValue, 2);
            this.tbAirTemperatureValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAirTemperatureValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbAirTemperatureValue.Location = new System.Drawing.Point(1000, 176);
            this.tbAirTemperatureValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbAirTemperatureValue.Multiline = true;
            this.tbAirTemperatureValue.Name = "tbAirTemperatureValue";
            this.tbAirTemperatureValue.ReadOnly = true;
            this.tbAirTemperatureValue.Size = new System.Drawing.Size(431, 29);
            this.tbAirTemperatureValue.TabIndex = 25;
            this.tbAirTemperatureValue.Text = "---";
            this.tbAirTemperatureValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbWindSpeedValue
            // 
            this.tbWindSpeedValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbWindSpeedValue, 2);
            this.tbWindSpeedValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWindSpeedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWindSpeedValue.Location = new System.Drawing.Point(1000, 213);
            this.tbWindSpeedValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbWindSpeedValue.Multiline = true;
            this.tbWindSpeedValue.Name = "tbWindSpeedValue";
            this.tbWindSpeedValue.ReadOnly = true;
            this.tbWindSpeedValue.Size = new System.Drawing.Size(431, 29);
            this.tbWindSpeedValue.TabIndex = 26;
            this.tbWindSpeedValue.Text = "---";
            this.tbWindSpeedValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbWindDirectionValue
            // 
            this.tbWindDirectionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbWindDirectionValue, 2);
            this.tbWindDirectionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWindDirectionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWindDirectionValue.Location = new System.Drawing.Point(1000, 250);
            this.tbWindDirectionValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbWindDirectionValue.Multiline = true;
            this.tbWindDirectionValue.Name = "tbWindDirectionValue";
            this.tbWindDirectionValue.ReadOnly = true;
            this.tbWindDirectionValue.Size = new System.Drawing.Size(431, 29);
            this.tbWindDirectionValue.TabIndex = 27;
            this.tbWindDirectionValue.Text = "---";
            this.tbWindDirectionValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbRelHumidityValue
            // 
            this.tbRelHumidityValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbRelHumidityValue, 2);
            this.tbRelHumidityValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRelHumidityValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbRelHumidityValue.Location = new System.Drawing.Point(1000, 287);
            this.tbRelHumidityValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbRelHumidityValue.Multiline = true;
            this.tbRelHumidityValue.Name = "tbRelHumidityValue";
            this.tbRelHumidityValue.ReadOnly = true;
            this.tbRelHumidityValue.Size = new System.Drawing.Size(431, 29);
            this.tbRelHumidityValue.TabIndex = 28;
            this.tbRelHumidityValue.Text = "---";
            this.tbRelHumidityValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblWaterTemperatureTitle
            // 
            this.lblWaterTemperatureTitle.AutoSize = true;
            this.lblWaterTemperatureTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWaterTemperatureTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWaterTemperatureTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWaterTemperatureTitle.Location = new System.Drawing.Point(761, 320);
            this.lblWaterTemperatureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaterTemperatureTitle.Name = "lblWaterTemperatureTitle";
            this.lblWaterTemperatureTitle.Size = new System.Drawing.Size(231, 37);
            this.lblWaterTemperatureTitle.TabIndex = 36;
            this.lblWaterTemperatureTitle.Text = "Water temp.";
            this.lblWaterTemperatureTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWaterSalinityTitle
            // 
            this.lblWaterSalinityTitle.AutoSize = true;
            this.lblWaterSalinityTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWaterSalinityTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWaterSalinityTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWaterSalinityTitle.Location = new System.Drawing.Point(761, 357);
            this.lblWaterSalinityTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaterSalinityTitle.Name = "lblWaterSalinityTitle";
            this.lblWaterSalinityTitle.Size = new System.Drawing.Size(231, 37);
            this.lblWaterSalinityTitle.TabIndex = 37;
            this.lblWaterSalinityTitle.Text = "Water sal.";
            this.lblWaterSalinityTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbWaterTemperatureValue
            // 
            this.tbWaterTemperatureValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbWaterTemperatureValue, 2);
            this.tbWaterTemperatureValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWaterTemperatureValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWaterTemperatureValue.Location = new System.Drawing.Point(1000, 324);
            this.tbWaterTemperatureValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbWaterTemperatureValue.Multiline = true;
            this.tbWaterTemperatureValue.Name = "tbWaterTemperatureValue";
            this.tbWaterTemperatureValue.ReadOnly = true;
            this.tbWaterTemperatureValue.Size = new System.Drawing.Size(431, 29);
            this.tbWaterTemperatureValue.TabIndex = 38;
            this.tbWaterTemperatureValue.Text = "---";
            this.tbWaterTemperatureValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbWaterSalinityValue
            // 
            this.tbWaterSalinityValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbWaterSalinityValue, 2);
            this.tbWaterSalinityValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWaterSalinityValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWaterSalinityValue.Location = new System.Drawing.Point(1000, 361);
            this.tbWaterSalinityValue.Margin = new System.Windows.Forms.Padding(4);
            this.tbWaterSalinityValue.Multiline = true;
            this.tbWaterSalinityValue.Name = "tbWaterSalinityValue";
            this.tbWaterSalinityValue.ReadOnly = true;
            this.tbWaterSalinityValue.Size = new System.Drawing.Size(431, 29);
            this.tbWaterSalinityValue.TabIndex = 39;
            this.tbWaterSalinityValue.Text = "---";
            this.tbWaterSalinityValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStatusString
            // 
            this.lblStatusString.AutoSize = true;
            this.lblStatusString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatusString, 8);
            this.lblStatusString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusString.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusString.Location = new System.Drawing.Point(4, 661);
            this.lblStatusString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(1427, 48);
            this.lblStatusString.TabIndex = 40;
            this.lblStatusString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbLogNCdata
            // 
            this.cbLogNCdata.AutoSize = true;
            this.cbLogNCdata.Checked = true;
            this.cbLogNCdata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.cbLogNCdata, 2);
            this.cbLogNCdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogNCdata.Location = new System.Drawing.Point(708, 53);
            this.cbLogNCdata.Margin = new System.Windows.Forms.Padding(4);
            this.cbLogNCdata.Name = "cbLogNCdata";
            this.cbLogNCdata.Size = new System.Drawing.Size(284, 41);
            this.cbLogNCdata.TabIndex = 47;
            this.cbLogNCdata.Text = "log data to *.nc and *.log files";
            this.cbLogNCdata.UseVisualStyleBackColor = true;
            // 
            // cbLogMeasurementsData
            // 
            this.cbLogMeasurementsData.AutoSize = true;
            this.cbLogMeasurementsData.Checked = true;
            this.cbLogMeasurementsData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.cbLogMeasurementsData, 2);
            this.cbLogMeasurementsData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogMeasurementsData.Location = new System.Drawing.Point(1000, 53);
            this.cbLogMeasurementsData.Margin = new System.Windows.Forms.Padding(4);
            this.cbLogMeasurementsData.Name = "cbLogMeasurementsData";
            this.cbLogMeasurementsData.Size = new System.Drawing.Size(431, 41);
            this.cbLogMeasurementsData.TabIndex = 48;
            this.cbLogMeasurementsData.Text = "Write mesurements entry text file";
            this.cbLogMeasurementsData.UseVisualStyleBackColor = true;
            // 
            // btnProperties
            // 
            this.btnProperties.BackColor = System.Drawing.Color.Gainsboro;
            this.btnProperties.BackgroundImage = global::IofffeVesselInfoStream.Properties.Resources.gearIcon;
            this.btnProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Location = new System.Drawing.Point(1357, 2);
            this.btnProperties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(75, 45);
            this.btnProperties.TabIndex = 50;
            this.btnProperties.UseVisualStyleBackColor = false;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // lblNavDataSpeedControl
            // 
            this.lblNavDataSpeedControl.AutoSize = true;
            this.lblNavDataSpeedControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblNavDataSpeedControl, 4);
            this.lblNavDataSpeedControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNavDataSpeedControl.ForeColor = System.Drawing.Color.Red;
            this.lblNavDataSpeedControl.Location = new System.Drawing.Point(4, 98);
            this.lblNavDataSpeedControl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNavDataSpeedControl.Name = "lblNavDataSpeedControl";
            this.lblNavDataSpeedControl.Size = new System.Drawing.Size(696, 37);
            this.lblNavDataSpeedControl.TabIndex = 59;
            this.lblNavDataSpeedControl.Text = "---";
            this.lblNavDataSpeedControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMeteoDataSpeedControl
            // 
            this.lblMeteoDataSpeedControl.AutoSize = true;
            this.lblMeteoDataSpeedControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMeteoDataSpeedControl, 3);
            this.lblMeteoDataSpeedControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMeteoDataSpeedControl.ForeColor = System.Drawing.Color.Red;
            this.lblMeteoDataSpeedControl.Location = new System.Drawing.Point(761, 98);
            this.lblMeteoDataSpeedControl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMeteoDataSpeedControl.Name = "lblMeteoDataSpeedControl";
            this.lblMeteoDataSpeedControl.Size = new System.Drawing.Size(670, 37);
            this.lblMeteoDataSpeedControl.TabIndex = 60;
            this.lblMeteoDataSpeedControl.Text = "---";
            this.lblMeteoDataSpeedControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnShowSunPositionData
            // 
            this.btnShowSunPositionData.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnShowSunPositionData, 4);
            this.btnShowSunPositionData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowSunPositionData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowSunPositionData.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnShowSunPositionData.Location = new System.Drawing.Point(4, 53);
            this.btnShowSunPositionData.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowSunPositionData.Name = "btnShowSunPositionData";
            this.btnShowSunPositionData.Size = new System.Drawing.Size(696, 41);
            this.btnShowSunPositionData.TabIndex = 61;
            this.btnShowSunPositionData.Text = "Sun positioning info";
            this.btnShowSunPositionData.UseVisualStyleBackColor = false;
            this.btnShowSunPositionData.Click += new System.EventHandler(this.btnShowSunPositionData_Click);
            // 
            // btnShowGeoTrack
            // 
            this.btnShowGeoTrack.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnShowGeoTrack, 4);
            this.btnShowGeoTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowGeoTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowGeoTrack.Location = new System.Drawing.Point(4, 410);
            this.btnShowGeoTrack.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowGeoTrack.Name = "btnShowGeoTrack";
            this.btnShowGeoTrack.Size = new System.Drawing.Size(696, 247);
            this.btnShowGeoTrack.TabIndex = 62;
            this.btnShowGeoTrack.Text = "show geotrack";
            this.btnShowGeoTrack.UseVisualStyleBackColor = false;
            this.btnShowGeoTrack.Click += new System.EventHandler(this.btnShowGeoTrack_Click);
            // 
            // btnShowMeteoData
            // 
            this.btnShowMeteoData.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnShowMeteoData, 3);
            this.btnShowMeteoData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowMeteoData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowMeteoData.Location = new System.Drawing.Point(761, 410);
            this.btnShowMeteoData.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowMeteoData.Name = "btnShowMeteoData";
            this.btnShowMeteoData.Size = new System.Drawing.Size(670, 247);
            this.btnShowMeteoData.TabIndex = 63;
            this.btnShowMeteoData.Text = "show meteo data graphs";
            this.btnShowMeteoData.UseVisualStyleBackColor = false;
            this.btnShowMeteoData.Click += new System.EventHandler(this.btnShowMeteoData_Click);
            // 
            // tpNavAndMeteoOverview
            // 
            this.tpNavAndMeteoOverview.BackColor = System.Drawing.SystemColors.Control;
            this.tpNavAndMeteoOverview.Controls.Add(this.tableLayoutPanel3);
            this.tpNavAndMeteoOverview.Location = new System.Drawing.Point(4, 29);
            this.tpNavAndMeteoOverview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpNavAndMeteoOverview.Name = "tpNavAndMeteoOverview";
            this.tpNavAndMeteoOverview.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpNavAndMeteoOverview.Size = new System.Drawing.Size(1443, 717);
            this.tpNavAndMeteoOverview.TabIndex = 2;
            this.tpNavAndMeteoOverview.Text = "Overview nav & meteo data";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 10;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.maskedTextBox1, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnFindInfo, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnWriteFile, 9, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblFoundNavDataTitle, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblFoundMeteoDataTitle, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.prbSearchingProgress, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblFoundNavData, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblFoundMetData, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblPlusOneHour, 5, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnPlusOneDay, 4, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.maskedTextBox2, 4, 6);
            this.tableLayoutPanel3.Controls.Add(this.btnFindIOFFEdataFiles, 6, 6);
            this.tableLayoutPanel3.Controls.Add(this.btnRepairAndWriteNCfile, 6, 7);
            this.tableLayoutPanel3.Controls.Add(this.prbMissingInfoSearchingProgress, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.maskedTextBox3, 4, 9);
            this.tableLayoutPanel3.Controls.Add(this.maskedTextBox4, 7, 9);
            this.tableLayoutPanel3.Controls.Add(this.label4, 6, 9);
            this.tableLayoutPanel3.Controls.Add(this.btnFindDataToExport, 9, 9);
            this.tableLayoutPanel3.Controls.Add(this.btnExportData, 6, 10);
            this.tableLayoutPanel3.Controls.Add(this.prbExportDataProgress, 0, 11);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 13;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1437, 713);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel3.SetColumnSpan(this.label1, 4);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 548);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(566, 50);
            this.label1.TabIndex = 18;
            this.label1.Text = "convert meteo&nav data for dates FROM:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label2, 4);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(566, 50);
            this.label2.TabIndex = 0;
            this.label2.Text = "Form data record log file for date-time:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBox1
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.maskedTextBox1, 3);
            this.maskedTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox1.Location = new System.Drawing.Point(575, 2);
            this.maskedTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.maskedTextBox1.Mask = "0000-00-00T90:00:00";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(423, 38);
            this.maskedTextBox1.TabIndex = 1;
            // 
            // btnFindInfo
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnFindInfo, 2);
            this.btnFindInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindInfo.Location = new System.Drawing.Point(1004, 2);
            this.btnFindInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFindInfo.Name = "btnFindInfo";
            this.btnFindInfo.Size = new System.Drawing.Size(280, 46);
            this.btnFindInfo.TabIndex = 2;
            this.btnFindInfo.Text = "find closest records";
            this.btnFindInfo.UseVisualStyleBackColor = true;
            this.btnFindInfo.Click += new System.EventHandler(this.btnFindInfo_Click);
            // 
            // btnWriteFile
            // 
            this.btnWriteFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWriteFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWriteFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnWriteFile.Location = new System.Drawing.Point(1290, 2);
            this.btnWriteFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnWriteFile.Name = "btnWriteFile";
            this.btnWriteFile.Size = new System.Drawing.Size(144, 46);
            this.btnWriteFile.TabIndex = 3;
            this.btnWriteFile.Text = "write";
            this.btnWriteFile.UseVisualStyleBackColor = true;
            this.btnWriteFile.Click += new System.EventHandler(this.btnWriteFile_Click);
            // 
            // lblFoundNavDataTitle
            // 
            this.lblFoundNavDataTitle.AutoSize = true;
            this.lblFoundNavDataTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFoundNavDataTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFoundNavDataTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFoundNavDataTitle.Location = new System.Drawing.Point(3, 120);
            this.lblFoundNavDataTitle.Name = "lblFoundNavDataTitle";
            this.lblFoundNavDataTitle.Size = new System.Drawing.Size(137, 144);
            this.lblFoundNavDataTitle.TabIndex = 6;
            this.lblFoundNavDataTitle.Text = "nav:";
            this.lblFoundNavDataTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFoundMeteoDataTitle
            // 
            this.lblFoundMeteoDataTitle.AutoSize = true;
            this.lblFoundMeteoDataTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFoundMeteoDataTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFoundMeteoDataTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFoundMeteoDataTitle.Location = new System.Drawing.Point(3, 264);
            this.lblFoundMeteoDataTitle.Name = "lblFoundMeteoDataTitle";
            this.lblFoundMeteoDataTitle.Size = new System.Drawing.Size(137, 144);
            this.lblFoundMeteoDataTitle.TabIndex = 7;
            this.lblFoundMeteoDataTitle.Text = "meteo:";
            this.lblFoundMeteoDataTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // prbSearchingProgress
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.prbSearchingProgress, 10);
            this.prbSearchingProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbSearchingProgress.Location = new System.Drawing.Point(3, 102);
            this.prbSearchingProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.prbSearchingProgress.Name = "prbSearchingProgress";
            this.prbSearchingProgress.Size = new System.Drawing.Size(1431, 16);
            this.prbSearchingProgress.TabIndex = 8;
            // 
            // lblFoundNavData
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.lblFoundNavData, 9);
            this.lblFoundNavData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFoundNavData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFoundNavData.Location = new System.Drawing.Point(146, 122);
            this.lblFoundNavData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblFoundNavData.Multiline = true;
            this.lblFoundNavData.Name = "lblFoundNavData";
            this.lblFoundNavData.ReadOnly = true;
            this.lblFoundNavData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lblFoundNavData.Size = new System.Drawing.Size(1288, 140);
            this.lblFoundNavData.TabIndex = 9;
            this.lblFoundNavData.Text = "---";
            // 
            // lblFoundMetData
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.lblFoundMetData, 9);
            this.lblFoundMetData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFoundMetData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFoundMetData.Location = new System.Drawing.Point(146, 266);
            this.lblFoundMetData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblFoundMetData.Multiline = true;
            this.lblFoundMetData.Name = "lblFoundMetData";
            this.lblFoundMetData.ReadOnly = true;
            this.lblFoundMetData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lblFoundMetData.Size = new System.Drawing.Size(1288, 140);
            this.lblFoundMetData.TabIndex = 10;
            this.lblFoundMetData.Text = "---";
            // 
            // lblPlusOneHour
            // 
            this.lblPlusOneHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPlusOneHour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPlusOneHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPlusOneHour.Location = new System.Drawing.Point(718, 52);
            this.lblPlusOneHour.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblPlusOneHour.Name = "lblPlusOneHour";
            this.lblPlusOneHour.Size = new System.Drawing.Size(137, 46);
            this.lblPlusOneHour.TabIndex = 11;
            this.lblPlusOneHour.Text = "+1 hour";
            this.lblPlusOneHour.UseVisualStyleBackColor = true;
            this.lblPlusOneHour.Click += new System.EventHandler(this.lblPlusOneHour_Click);
            // 
            // btnPlusOneDay
            // 
            this.btnPlusOneDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlusOneDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlusOneDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPlusOneDay.Location = new System.Drawing.Point(575, 52);
            this.btnPlusOneDay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPlusOneDay.Name = "btnPlusOneDay";
            this.btnPlusOneDay.Size = new System.Drawing.Size(137, 46);
            this.btnPlusOneDay.TabIndex = 12;
            this.btnPlusOneDay.Text = "+1 day";
            this.btnPlusOneDay.UseVisualStyleBackColor = true;
            this.btnPlusOneDay.Click += new System.EventHandler(this.btnPlusOneDay_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel3.SetColumnSpan(this.label3, 4);
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(3, 428);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(566, 50);
            this.label3.TabIndex = 13;
            this.label3.Text = "find and repair lost data records for date:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBox2
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.maskedTextBox2, 2);
            this.maskedTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox2.Location = new System.Drawing.Point(575, 430);
            this.maskedTextBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.maskedTextBox2.Mask = "0000-00-00";
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.Size = new System.Drawing.Size(280, 38);
            this.maskedTextBox2.TabIndex = 14;
            // 
            // btnFindIOFFEdataFiles
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnFindIOFFEdataFiles, 4);
            this.btnFindIOFFEdataFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindIOFFEdataFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindIOFFEdataFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindIOFFEdataFiles.Location = new System.Drawing.Point(861, 430);
            this.btnFindIOFFEdataFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFindIOFFEdataFiles.Name = "btnFindIOFFEdataFiles";
            this.btnFindIOFFEdataFiles.Size = new System.Drawing.Size(573, 46);
            this.btnFindIOFFEdataFiles.TabIndex = 15;
            this.btnFindIOFFEdataFiles.Text = "find IOFFE data files";
            this.btnFindIOFFEdataFiles.UseVisualStyleBackColor = true;
            this.btnFindIOFFEdataFiles.Click += new System.EventHandler(this.btnFindIOFFEdataFiles_Click);
            // 
            // btnRepairAndWriteNCfile
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnRepairAndWriteNCfile, 4);
            this.btnRepairAndWriteNCfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRepairAndWriteNCfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRepairAndWriteNCfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRepairAndWriteNCfile.Location = new System.Drawing.Point(861, 480);
            this.btnRepairAndWriteNCfile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRepairAndWriteNCfile.Name = "btnRepairAndWriteNCfile";
            this.btnRepairAndWriteNCfile.Size = new System.Drawing.Size(573, 46);
            this.btnRepairAndWriteNCfile.TabIndex = 16;
            this.btnRepairAndWriteNCfile.Text = "re-form nc-file with additional data";
            this.btnRepairAndWriteNCfile.UseVisualStyleBackColor = true;
            this.btnRepairAndWriteNCfile.Click += new System.EventHandler(this.btnRepairAndWriteNCfile_Click);
            // 
            // prbMissingInfoSearchingProgress
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.prbMissingInfoSearchingProgress, 10);
            this.prbMissingInfoSearchingProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbMissingInfoSearchingProgress.Location = new System.Drawing.Point(3, 530);
            this.prbMissingInfoSearchingProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.prbMissingInfoSearchingProgress.Name = "prbMissingInfoSearchingProgress";
            this.prbMissingInfoSearchingProgress.Size = new System.Drawing.Size(1431, 16);
            this.prbMissingInfoSearchingProgress.TabIndex = 17;
            // 
            // maskedTextBox3
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.maskedTextBox3, 2);
            this.maskedTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox3.Location = new System.Drawing.Point(575, 551);
            this.maskedTextBox3.Mask = "0000-00-00";
            this.maskedTextBox3.Name = "maskedTextBox3";
            this.maskedTextBox3.Size = new System.Drawing.Size(280, 38);
            this.maskedTextBox3.TabIndex = 19;
            // 
            // maskedTextBox4
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.maskedTextBox4, 2);
            this.maskedTextBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox4.Location = new System.Drawing.Point(1004, 551);
            this.maskedTextBox4.Mask = "0000-00-00";
            this.maskedTextBox4.Name = "maskedTextBox4";
            this.maskedTextBox4.Size = new System.Drawing.Size(280, 38);
            this.maskedTextBox4.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(861, 548);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 50);
            this.label4.TabIndex = 21;
            this.label4.Text = "TO:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFindDataToExport
            // 
            this.btnFindDataToExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindDataToExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindDataToExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindDataToExport.Location = new System.Drawing.Point(1290, 551);
            this.btnFindDataToExport.Name = "btnFindDataToExport";
            this.btnFindDataToExport.Size = new System.Drawing.Size(144, 44);
            this.btnFindDataToExport.TabIndex = 22;
            this.btnFindDataToExport.Text = "find data";
            this.btnFindDataToExport.UseVisualStyleBackColor = true;
            this.btnFindDataToExport.Click += new System.EventHandler(this.btnFindDataToExport_Click);
            // 
            // btnExportData
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnExportData, 4);
            this.btnExportData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExportData.Location = new System.Drawing.Point(861, 601);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(573, 44);
            this.btnExportData.TabIndex = 23;
            this.btnExportData.Text = "export data";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // prbExportDataProgress
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.prbExportDataProgress, 10);
            this.prbExportDataProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbExportDataProgress.Location = new System.Drawing.Point(3, 651);
            this.prbExportDataProgress.Name = "prbExportDataProgress";
            this.prbExportDataProgress.Size = new System.Drawing.Size(1431, 14);
            this.prbExportDataProgress.TabIndex = 24;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1451, 750);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Ioffe nav&meteo stream";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.tabControl1.ResumeLayout(false);
            this.tpNavAndMeteo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tpNavAndMeteoOverview.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgwSocketStreamReader;
        private System.ComponentModel.BackgroundWorker bgwStreamDataProcessing;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpNavAndMeteo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblGPSlatTitle;
        private System.Windows.Forms.Label lblGPSlonTitle;
        private System.Windows.Forms.Label lblGPSDateTimeTitle;
        private System.Windows.Forms.Label lblTrueHeadTitle;
        private System.Windows.Forms.Label lblGyroHeadTitle;
        private System.Windows.Forms.Label lblSpeedKnotsTitle;
        private System.Windows.Forms.Label lblDepthTitle;
        private System.Windows.Forms.TextBox tbGPSDateTimeValue;
        private System.Windows.Forms.TextBox tbGPSlonValue;
        private System.Windows.Forms.TextBox tbGPSlatValue;
        private System.Windows.Forms.TextBox tbTrueHeadValue;
        private System.Windows.Forms.TextBox tbGyroHeadValue;
        private System.Windows.Forms.TextBox tbSpeedKnotsValue;
        private System.Windows.Forms.TextBox tbDepthValue;
        private System.Windows.Forms.Label lblPressureTitle;
        private System.Windows.Forms.Label lblAirTemperatureTitle;
        private System.Windows.Forms.Label lblWindSpeedTitle;
        private System.Windows.Forms.Label lblWindDirectionTitle;
        private System.Windows.Forms.Label lblRelHumidityTitle;
        private System.Windows.Forms.TextBox tbPressureValue;
        private System.Windows.Forms.TextBox tbAirTemperatureValue;
        private System.Windows.Forms.TextBox tbWindSpeedValue;
        private System.Windows.Forms.TextBox tbWindDirectionValue;
        private System.Windows.Forms.TextBox tbRelHumidityValue;
        private System.Windows.Forms.Label lblWaterTemperatureTitle;
        private System.Windows.Forms.Label lblWaterSalinityTitle;
        private System.Windows.Forms.TextBox tbWaterTemperatureValue;
        private System.Windows.Forms.TextBox tbWaterSalinityValue;
        private System.Windows.Forms.Label lblStatusString;
        private System.Windows.Forms.CheckBox cbLogNCdata;
        private System.Windows.Forms.CheckBox cbLogMeasurementsData;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.TabPage tpNavAndMeteoOverview;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Button btnFindInfo;
        private System.Windows.Forms.Button btnWriteFile;
        private System.Windows.Forms.Label lblFoundNavDataTitle;
        private System.Windows.Forms.Label lblFoundMeteoDataTitle;
        private System.Windows.Forms.ProgressBar prbSearchingProgress;
        private System.Windows.Forms.TextBox lblFoundNavData;
        private System.Windows.Forms.TextBox lblFoundMetData;
        private System.Windows.Forms.Button lblPlusOneHour;
        private System.Windows.Forms.Button btnPlusOneDay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox maskedTextBox2;
        private System.Windows.Forms.Button btnFindIOFFEdataFiles;
        private System.Windows.Forms.Button btnRepairAndWriteNCfile;
        private System.Windows.Forms.ProgressBar prbMissingInfoSearchingProgress;
        private System.Windows.Forms.Label lblNavDataSpeedControl;
        private System.Windows.Forms.Label lblMeteoDataSpeedControl;
        private System.Windows.Forms.Button btnShowSunPositionData;
        private System.Windows.Forms.Button btnShowGeoTrack;
        private System.Windows.Forms.Button btnShowMeteoData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox maskedTextBox3;
        private System.Windows.Forms.MaskedTextBox maskedTextBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnFindDataToExport;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.ProgressBar prbExportDataProgress;
    }
}