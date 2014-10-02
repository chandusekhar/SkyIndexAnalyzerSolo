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
            this.bgwStreamTextParser = new System.ComponentModel.BackgroundWorker();
            this.bgwGraphsRenderer = new System.ComponentModel.BackgroundWorker();
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
            this.pbGeoTrack = new System.Windows.Forms.PictureBox();
            this.lblPressureGraphTitle = new System.Windows.Forms.Label();
            this.pbGraphs = new System.Windows.Forms.PictureBox();
            this.trbGeoTrackScale = new System.Windows.Forms.TrackBar();
            this.cbShowGeoTrack = new System.Windows.Forms.CheckBox();
            this.wcUpdatimgGraphs = new MRG.Controls.UI.LoadingCircle();
            this.lblWaterTemperatureTitle = new System.Windows.Forms.Label();
            this.lblWaterSalinityTitle = new System.Windows.Forms.Label();
            this.tbWaterTemperatureValue = new System.Windows.Forms.TextBox();
            this.tbWaterSalinityValue = new System.Windows.Forms.TextBox();
            this.lblStatusString = new System.Windows.Forms.Label();
            this.scrbGeoTrackScrollLatValues = new System.Windows.Forms.VScrollBar();
            this.scrbGeoTrackScrollLonValues = new System.Windows.Forms.HScrollBar();
            this.wcNavDataSoeedControl = new MRG.Controls.UI.LoadingCircle();
            this.wcMeteoDataSpeedControl = new MRG.Controls.UI.LoadingCircle();
            this.btnCenterToActualPosition = new System.Windows.Forms.Button();
            this.tpSeaSaveStream = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnStartStopSeaSave = new System.Windows.Forms.Button();
            this.tbSeaSaveLog = new System.Windows.Forms.TextBox();
            this.bgwSeaSaveSocketStreamReader = new System.ComponentModel.BackgroundWorker();
            this.bgwSeaSaveStreamTextParser = new System.ComponentModel.BackgroundWorker();
            this.cbLogNCdata = new System.Windows.Forms.CheckBox();
            this.cbLogMeasurementsData = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tpNavAndMeteo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeoTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraphs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbGeoTrackScale)).BeginInit();
            this.tpSeaSaveStream.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgwSocketStreamReader
            // 
            this.bgwSocketStreamReader.WorkerSupportsCancellation = true;
            this.bgwSocketStreamReader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSocketStreamReader_DoWork);
            // 
            // bgwStreamTextParser
            // 
            this.bgwStreamTextParser.WorkerSupportsCancellation = true;
            this.bgwStreamTextParser.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwStreamTextParser_DoWork);
            // 
            // bgwGraphsRenderer
            // 
            this.bgwGraphsRenderer.WorkerSupportsCancellation = true;
            this.bgwGraphsRenderer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGraphsRenderer_DoWork);
            // 
            // bgwStreamDataProcessing
            // 
            this.bgwStreamDataProcessing.WorkerSupportsCancellation = true;
            this.bgwStreamDataProcessing.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwStreamDataProcessing_DoWork);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpNavAndMeteo);
            this.tabControl1.Controls.Add(this.tpSeaSaveStream);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(817, 573);
            this.tabControl1.TabIndex = 0;
            // 
            // tpNavAndMeteo
            // 
            this.tpNavAndMeteo.BackColor = System.Drawing.SystemColors.Control;
            this.tpNavAndMeteo.Controls.Add(this.tableLayoutPanel1);
            this.tpNavAndMeteo.Location = new System.Drawing.Point(4, 25);
            this.tpNavAndMeteo.Name = "tpNavAndMeteo";
            this.tpNavAndMeteo.Padding = new System.Windows.Forms.Padding(3);
            this.tpNavAndMeteo.Size = new System.Drawing.Size(809, 544);
            this.tpNavAndMeteo.TabIndex = 0;
            this.tpNavAndMeteo.Text = "Nav&Meteo";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
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
            this.tableLayoutPanel1.Controls.Add(this.lblPressureTitle, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblAirTemperatureTitle, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblWindSpeedTitle, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblWindDirectionTitle, 4, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblRelHumidityTitle, 4, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbPressureValue, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbAirTemperatureValue, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbWindSpeedValue, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbWindDirectionValue, 5, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbRelHumidityValue, 5, 7);
            this.tableLayoutPanel1.Controls.Add(this.pbGeoTrack, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblPressureGraphTitle, 4, 11);
            this.tableLayoutPanel1.Controls.Add(this.pbGraphs, 4, 12);
            this.tableLayoutPanel1.Controls.Add(this.trbGeoTrackScale, 3, 12);
            this.tableLayoutPanel1.Controls.Add(this.cbShowGeoTrack, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.wcUpdatimgGraphs, 3, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblWaterTemperatureTitle, 4, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblWaterSalinityTitle, 4, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbWaterTemperatureValue, 5, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbWaterSalinityValue, 5, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblStatusString, 0, 16);
            this.tableLayoutPanel1.Controls.Add(this.scrbGeoTrackScrollLatValues, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.scrbGeoTrackScrollLonValues, 1, 15);
            this.tableLayoutPanel1.Controls.Add(this.wcNavDataSoeedControl, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.wcMeteoDataSpeedControl, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCenterToActualPosition, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.cbLogNCdata, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbLogMeasurementsData, 4, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 17;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(803, 538);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnConnect, 6);
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnConnect.ForeColor = System.Drawing.Color.Red;
            this.btnConnect.Location = new System.Drawing.Point(3, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(797, 34);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblGPSlatTitle
            // 
            this.lblGPSlatTitle.AutoSize = true;
            this.lblGPSlatTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblGPSlatTitle, 2);
            this.lblGPSlatTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGPSlatTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGPSlatTitle.Location = new System.Drawing.Point(3, 130);
            this.lblGPSlatTitle.Name = "lblGPSlatTitle";
            this.lblGPSlatTitle.Size = new System.Drawing.Size(170, 30);
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
            this.lblGPSlonTitle.Location = new System.Drawing.Point(3, 160);
            this.lblGPSlonTitle.Name = "lblGPSlonTitle";
            this.lblGPSlonTitle.Size = new System.Drawing.Size(170, 30);
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
            this.lblGPSDateTimeTitle.Location = new System.Drawing.Point(3, 100);
            this.lblGPSDateTimeTitle.Name = "lblGPSDateTimeTitle";
            this.lblGPSDateTimeTitle.Size = new System.Drawing.Size(170, 30);
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
            this.lblTrueHeadTitle.Location = new System.Drawing.Point(3, 190);
            this.lblTrueHeadTitle.Name = "lblTrueHeadTitle";
            this.lblTrueHeadTitle.Size = new System.Drawing.Size(170, 30);
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
            this.lblGyroHeadTitle.Location = new System.Drawing.Point(3, 220);
            this.lblGyroHeadTitle.Name = "lblGyroHeadTitle";
            this.lblGyroHeadTitle.Size = new System.Drawing.Size(170, 30);
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
            this.lblSpeedKnotsTitle.Location = new System.Drawing.Point(3, 250);
            this.lblSpeedKnotsTitle.Name = "lblSpeedKnotsTitle";
            this.lblSpeedKnotsTitle.Size = new System.Drawing.Size(170, 30);
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
            this.lblDepthTitle.Location = new System.Drawing.Point(3, 280);
            this.lblDepthTitle.Name = "lblDepthTitle";
            this.lblDepthTitle.Size = new System.Drawing.Size(170, 30);
            this.lblDepthTitle.TabIndex = 11;
            this.lblDepthTitle.Text = "Depth";
            this.lblDepthTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbGPSDateTimeValue
            // 
            this.tbGPSDateTimeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGPSDateTimeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGPSDateTimeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGPSDateTimeValue.Location = new System.Drawing.Point(179, 103);
            this.tbGPSDateTimeValue.Multiline = true;
            this.tbGPSDateTimeValue.Name = "tbGPSDateTimeValue";
            this.tbGPSDateTimeValue.ReadOnly = true;
            this.tbGPSDateTimeValue.Size = new System.Drawing.Size(213, 24);
            this.tbGPSDateTimeValue.TabIndex = 12;
            this.tbGPSDateTimeValue.Text = "---";
            this.tbGPSDateTimeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbGPSlonValue
            // 
            this.tbGPSlonValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGPSlonValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGPSlonValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGPSlonValue.Location = new System.Drawing.Point(179, 163);
            this.tbGPSlonValue.Multiline = true;
            this.tbGPSlonValue.Name = "tbGPSlonValue";
            this.tbGPSlonValue.ReadOnly = true;
            this.tbGPSlonValue.Size = new System.Drawing.Size(213, 24);
            this.tbGPSlonValue.TabIndex = 13;
            this.tbGPSlonValue.Text = "---";
            this.tbGPSlonValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbGPSlatValue
            // 
            this.tbGPSlatValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGPSlatValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGPSlatValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGPSlatValue.Location = new System.Drawing.Point(179, 133);
            this.tbGPSlatValue.Multiline = true;
            this.tbGPSlatValue.Name = "tbGPSlatValue";
            this.tbGPSlatValue.ReadOnly = true;
            this.tbGPSlatValue.Size = new System.Drawing.Size(213, 24);
            this.tbGPSlatValue.TabIndex = 14;
            this.tbGPSlatValue.Text = "---";
            this.tbGPSlatValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbTrueHeadValue
            // 
            this.tbTrueHeadValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTrueHeadValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTrueHeadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTrueHeadValue.Location = new System.Drawing.Point(179, 193);
            this.tbTrueHeadValue.Multiline = true;
            this.tbTrueHeadValue.Name = "tbTrueHeadValue";
            this.tbTrueHeadValue.ReadOnly = true;
            this.tbTrueHeadValue.Size = new System.Drawing.Size(213, 24);
            this.tbTrueHeadValue.TabIndex = 15;
            this.tbTrueHeadValue.Text = "---";
            this.tbTrueHeadValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbGyroHeadValue
            // 
            this.tbGyroHeadValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGyroHeadValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGyroHeadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGyroHeadValue.Location = new System.Drawing.Point(179, 223);
            this.tbGyroHeadValue.Multiline = true;
            this.tbGyroHeadValue.Name = "tbGyroHeadValue";
            this.tbGyroHeadValue.ReadOnly = true;
            this.tbGyroHeadValue.Size = new System.Drawing.Size(213, 24);
            this.tbGyroHeadValue.TabIndex = 16;
            this.tbGyroHeadValue.Text = "---";
            this.tbGyroHeadValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbSpeedKnotsValue
            // 
            this.tbSpeedKnotsValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSpeedKnotsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSpeedKnotsValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSpeedKnotsValue.Location = new System.Drawing.Point(179, 253);
            this.tbSpeedKnotsValue.Multiline = true;
            this.tbSpeedKnotsValue.Name = "tbSpeedKnotsValue";
            this.tbSpeedKnotsValue.ReadOnly = true;
            this.tbSpeedKnotsValue.Size = new System.Drawing.Size(213, 24);
            this.tbSpeedKnotsValue.TabIndex = 17;
            this.tbSpeedKnotsValue.Text = "---";
            this.tbSpeedKnotsValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbDepthValue
            // 
            this.tbDepthValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDepthValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDepthValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDepthValue.Location = new System.Drawing.Point(179, 283);
            this.tbDepthValue.Multiline = true;
            this.tbDepthValue.Name = "tbDepthValue";
            this.tbDepthValue.ReadOnly = true;
            this.tbDepthValue.Size = new System.Drawing.Size(213, 24);
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
            this.lblPressureTitle.Location = new System.Drawing.Point(438, 100);
            this.lblPressureTitle.Name = "lblPressureTitle";
            this.lblPressureTitle.Size = new System.Drawing.Size(140, 30);
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
            this.lblAirTemperatureTitle.Location = new System.Drawing.Point(438, 130);
            this.lblAirTemperatureTitle.Name = "lblAirTemperatureTitle";
            this.lblAirTemperatureTitle.Size = new System.Drawing.Size(140, 30);
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
            this.lblWindSpeedTitle.Location = new System.Drawing.Point(438, 160);
            this.lblWindSpeedTitle.Name = "lblWindSpeedTitle";
            this.lblWindSpeedTitle.Size = new System.Drawing.Size(140, 30);
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
            this.lblWindDirectionTitle.Location = new System.Drawing.Point(438, 190);
            this.lblWindDirectionTitle.Name = "lblWindDirectionTitle";
            this.lblWindDirectionTitle.Size = new System.Drawing.Size(140, 30);
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
            this.lblRelHumidityTitle.Location = new System.Drawing.Point(438, 220);
            this.lblRelHumidityTitle.Name = "lblRelHumidityTitle";
            this.lblRelHumidityTitle.Size = new System.Drawing.Size(140, 30);
            this.lblRelHumidityTitle.TabIndex = 23;
            this.lblRelHumidityTitle.Text = "Rel. humidity";
            this.lblRelHumidityTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPressureValue
            // 
            this.tbPressureValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPressureValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPressureValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPressureValue.Location = new System.Drawing.Point(584, 103);
            this.tbPressureValue.Multiline = true;
            this.tbPressureValue.Name = "tbPressureValue";
            this.tbPressureValue.ReadOnly = true;
            this.tbPressureValue.Size = new System.Drawing.Size(216, 24);
            this.tbPressureValue.TabIndex = 24;
            this.tbPressureValue.Text = "---";
            this.tbPressureValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbAirTemperatureValue
            // 
            this.tbAirTemperatureValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAirTemperatureValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAirTemperatureValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbAirTemperatureValue.Location = new System.Drawing.Point(584, 133);
            this.tbAirTemperatureValue.Multiline = true;
            this.tbAirTemperatureValue.Name = "tbAirTemperatureValue";
            this.tbAirTemperatureValue.ReadOnly = true;
            this.tbAirTemperatureValue.Size = new System.Drawing.Size(216, 24);
            this.tbAirTemperatureValue.TabIndex = 25;
            this.tbAirTemperatureValue.Text = "---";
            this.tbAirTemperatureValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbWindSpeedValue
            // 
            this.tbWindSpeedValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWindSpeedValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWindSpeedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWindSpeedValue.Location = new System.Drawing.Point(584, 163);
            this.tbWindSpeedValue.Multiline = true;
            this.tbWindSpeedValue.Name = "tbWindSpeedValue";
            this.tbWindSpeedValue.ReadOnly = true;
            this.tbWindSpeedValue.Size = new System.Drawing.Size(216, 24);
            this.tbWindSpeedValue.TabIndex = 26;
            this.tbWindSpeedValue.Text = "---";
            this.tbWindSpeedValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbWindDirectionValue
            // 
            this.tbWindDirectionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWindDirectionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWindDirectionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWindDirectionValue.Location = new System.Drawing.Point(584, 193);
            this.tbWindDirectionValue.Multiline = true;
            this.tbWindDirectionValue.Name = "tbWindDirectionValue";
            this.tbWindDirectionValue.ReadOnly = true;
            this.tbWindDirectionValue.Size = new System.Drawing.Size(216, 24);
            this.tbWindDirectionValue.TabIndex = 27;
            this.tbWindDirectionValue.Text = "---";
            this.tbWindDirectionValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbRelHumidityValue
            // 
            this.tbRelHumidityValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbRelHumidityValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRelHumidityValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbRelHumidityValue.Location = new System.Drawing.Point(584, 223);
            this.tbRelHumidityValue.Multiline = true;
            this.tbRelHumidityValue.Name = "tbRelHumidityValue";
            this.tbRelHumidityValue.ReadOnly = true;
            this.tbRelHumidityValue.Size = new System.Drawing.Size(216, 24);
            this.tbRelHumidityValue.TabIndex = 28;
            this.tbRelHumidityValue.Text = "---";
            this.tbRelHumidityValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pbGeoTrack
            // 
            this.pbGeoTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbGeoTrack, 2);
            this.pbGeoTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGeoTrack.Location = new System.Drawing.Point(33, 363);
            this.pbGeoTrack.Name = "pbGeoTrack";
            this.tableLayoutPanel1.SetRowSpan(this.pbGeoTrack, 3);
            this.pbGeoTrack.Size = new System.Drawing.Size(359, 111);
            this.pbGeoTrack.TabIndex = 29;
            this.pbGeoTrack.TabStop = false;
            this.pbGeoTrack.Click += new System.EventHandler(this.pbGeoTrack_Click);
            // 
            // lblPressureGraphTitle
            // 
            this.lblPressureGraphTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblPressureGraphTitle, 2);
            this.lblPressureGraphTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPressureGraphTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPressureGraphTitle.Location = new System.Drawing.Point(438, 320);
            this.lblPressureGraphTitle.Name = "lblPressureGraphTitle";
            this.lblPressureGraphTitle.Size = new System.Drawing.Size(362, 40);
            this.lblPressureGraphTitle.TabIndex = 31;
            this.lblPressureGraphTitle.Text = "Pressure, Temp., W.Speed";
            this.lblPressureGraphTitle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pbGraphs
            // 
            this.pbGraphs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbGraphs, 2);
            this.pbGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraphs.Location = new System.Drawing.Point(438, 363);
            this.pbGraphs.Name = "pbGraphs";
            this.tableLayoutPanel1.SetRowSpan(this.pbGraphs, 3);
            this.pbGraphs.Size = new System.Drawing.Size(362, 111);
            this.pbGraphs.TabIndex = 32;
            this.pbGraphs.TabStop = false;
            // 
            // trbGeoTrackScale
            // 
            this.trbGeoTrackScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbGeoTrackScale.LargeChange = 3;
            this.trbGeoTrackScale.Location = new System.Drawing.Point(398, 363);
            this.trbGeoTrackScale.Name = "trbGeoTrackScale";
            this.trbGeoTrackScale.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tableLayoutPanel1.SetRowSpan(this.trbGeoTrackScale, 3);
            this.trbGeoTrackScale.Size = new System.Drawing.Size(34, 111);
            this.trbGeoTrackScale.TabIndex = 33;
            this.trbGeoTrackScale.ValueChanged += new System.EventHandler(this.trbGeoTrackScale_ValueChanged);
            // 
            // cbShowGeoTrack
            // 
            this.cbShowGeoTrack.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.cbShowGeoTrack, 2);
            this.cbShowGeoTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbShowGeoTrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbShowGeoTrack.Location = new System.Drawing.Point(33, 323);
            this.cbShowGeoTrack.Name = "cbShowGeoTrack";
            this.cbShowGeoTrack.Size = new System.Drawing.Size(359, 34);
            this.cbShowGeoTrack.TabIndex = 34;
            this.cbShowGeoTrack.Text = "Geotrack";
            this.cbShowGeoTrack.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.cbShowGeoTrack.UseVisualStyleBackColor = true;
            this.cbShowGeoTrack.CheckedChanged += new System.EventHandler(this.cbShowGeoTrack_CheckedChanged);
            // 
            // wcUpdatimgGraphs
            // 
            this.wcUpdatimgGraphs.Active = false;
            this.wcUpdatimgGraphs.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.wcUpdatimgGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wcUpdatimgGraphs.InnerCircleRadius = 8;
            this.wcUpdatimgGraphs.Location = new System.Drawing.Point(398, 323);
            this.wcUpdatimgGraphs.Name = "wcUpdatimgGraphs";
            this.wcUpdatimgGraphs.NumberSpoke = 24;
            this.wcUpdatimgGraphs.OuterCircleRadius = 9;
            this.wcUpdatimgGraphs.RotationSpeed = 100;
            this.wcUpdatimgGraphs.Size = new System.Drawing.Size(34, 34);
            this.wcUpdatimgGraphs.SpokeThickness = 4;
            this.wcUpdatimgGraphs.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.wcUpdatimgGraphs.TabIndex = 35;
            this.wcUpdatimgGraphs.Text = "loadingCircle1";
            this.wcUpdatimgGraphs.Visible = false;
            // 
            // lblWaterTemperatureTitle
            // 
            this.lblWaterTemperatureTitle.AutoSize = true;
            this.lblWaterTemperatureTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWaterTemperatureTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWaterTemperatureTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWaterTemperatureTitle.Location = new System.Drawing.Point(438, 250);
            this.lblWaterTemperatureTitle.Name = "lblWaterTemperatureTitle";
            this.lblWaterTemperatureTitle.Size = new System.Drawing.Size(140, 30);
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
            this.lblWaterSalinityTitle.Location = new System.Drawing.Point(438, 280);
            this.lblWaterSalinityTitle.Name = "lblWaterSalinityTitle";
            this.lblWaterSalinityTitle.Size = new System.Drawing.Size(140, 30);
            this.lblWaterSalinityTitle.TabIndex = 37;
            this.lblWaterSalinityTitle.Text = "Water sal.";
            this.lblWaterSalinityTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbWaterTemperatureValue
            // 
            this.tbWaterTemperatureValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWaterTemperatureValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWaterTemperatureValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWaterTemperatureValue.Location = new System.Drawing.Point(584, 253);
            this.tbWaterTemperatureValue.Multiline = true;
            this.tbWaterTemperatureValue.Name = "tbWaterTemperatureValue";
            this.tbWaterTemperatureValue.ReadOnly = true;
            this.tbWaterTemperatureValue.Size = new System.Drawing.Size(216, 24);
            this.tbWaterTemperatureValue.TabIndex = 38;
            this.tbWaterTemperatureValue.Text = "---";
            this.tbWaterTemperatureValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbWaterSalinityValue
            // 
            this.tbWaterSalinityValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWaterSalinityValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWaterSalinityValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWaterSalinityValue.Location = new System.Drawing.Point(584, 283);
            this.tbWaterSalinityValue.Multiline = true;
            this.tbWaterSalinityValue.Name = "tbWaterSalinityValue";
            this.tbWaterSalinityValue.ReadOnly = true;
            this.tbWaterSalinityValue.Size = new System.Drawing.Size(216, 24);
            this.tbWaterSalinityValue.TabIndex = 39;
            this.tbWaterSalinityValue.Text = "---";
            this.tbWaterSalinityValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStatusString
            // 
            this.lblStatusString.AutoSize = true;
            this.lblStatusString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatusString, 6);
            this.lblStatusString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusString.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusString.Location = new System.Drawing.Point(3, 507);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(797, 31);
            this.lblStatusString.TabIndex = 40;
            this.lblStatusString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scrbGeoTrackScrollLatValues
            // 
            this.scrbGeoTrackScrollLatValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrbGeoTrackScrollLatValues.LargeChange = 1;
            this.scrbGeoTrackScrollLatValues.Location = new System.Drawing.Point(0, 360);
            this.scrbGeoTrackScrollLatValues.Maximum = 1;
            this.scrbGeoTrackScrollLatValues.Minimum = -1;
            this.scrbGeoTrackScrollLatValues.Name = "scrbGeoTrackScrollLatValues";
            this.tableLayoutPanel1.SetRowSpan(this.scrbGeoTrackScrollLatValues, 3);
            this.scrbGeoTrackScrollLatValues.Size = new System.Drawing.Size(30, 117);
            this.scrbGeoTrackScrollLatValues.TabIndex = 42;
            this.scrbGeoTrackScrollLatValues.ValueChanged += new System.EventHandler(this.scrbGeoTrackScrollLatValues_ValueChanged);
            // 
            // scrbGeoTrackScrollLonValues
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.scrbGeoTrackScrollLonValues, 2);
            this.scrbGeoTrackScrollLonValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrbGeoTrackScrollLonValues.LargeChange = 1;
            this.scrbGeoTrackScrollLonValues.Location = new System.Drawing.Point(30, 477);
            this.scrbGeoTrackScrollLonValues.Maximum = 1;
            this.scrbGeoTrackScrollLonValues.Minimum = -1;
            this.scrbGeoTrackScrollLonValues.Name = "scrbGeoTrackScrollLonValues";
            this.scrbGeoTrackScrollLonValues.Size = new System.Drawing.Size(365, 30);
            this.scrbGeoTrackScrollLonValues.TabIndex = 43;
            this.scrbGeoTrackScrollLonValues.ValueChanged += new System.EventHandler(this.scrbGeoTrackScrollLonValues_ValueChanged);
            // 
            // wcNavDataSoeedControl
            // 
            this.wcNavDataSoeedControl.Active = false;
            this.wcNavDataSoeedControl.Color = System.Drawing.Color.Red;
            this.wcNavDataSoeedControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wcNavDataSoeedControl.InnerCircleRadius = 5;
            this.wcNavDataSoeedControl.Location = new System.Drawing.Point(179, 73);
            this.wcNavDataSoeedControl.Name = "wcNavDataSoeedControl";
            this.wcNavDataSoeedControl.NumberSpoke = 12;
            this.wcNavDataSoeedControl.OuterCircleRadius = 11;
            this.wcNavDataSoeedControl.RotationSpeed = 1;
            this.wcNavDataSoeedControl.Size = new System.Drawing.Size(213, 24);
            this.wcNavDataSoeedControl.SpokeThickness = 2;
            this.wcNavDataSoeedControl.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.wcNavDataSoeedControl.TabIndex = 44;
            this.wcNavDataSoeedControl.Text = "loadingCircle1";
            // 
            // wcMeteoDataSpeedControl
            // 
            this.wcMeteoDataSpeedControl.Active = false;
            this.wcMeteoDataSpeedControl.Color = System.Drawing.Color.Red;
            this.wcMeteoDataSpeedControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.wcMeteoDataSpeedControl.InnerCircleRadius = 5;
            this.wcMeteoDataSpeedControl.Location = new System.Drawing.Point(584, 73);
            this.wcMeteoDataSpeedControl.Name = "wcMeteoDataSpeedControl";
            this.wcMeteoDataSpeedControl.NumberSpoke = 12;
            this.wcMeteoDataSpeedControl.OuterCircleRadius = 11;
            this.wcMeteoDataSpeedControl.RotationSpeed = 1;
            this.wcMeteoDataSpeedControl.Size = new System.Drawing.Size(216, 23);
            this.wcMeteoDataSpeedControl.SpokeThickness = 2;
            this.wcMeteoDataSpeedControl.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.wcMeteoDataSpeedControl.TabIndex = 45;
            this.wcMeteoDataSpeedControl.Text = "loadingCircle2";
            // 
            // btnCenterToActualPosition
            // 
            this.btnCenterToActualPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenterToActualPosition.FlatAppearance.BorderSize = 0;
            this.btnCenterToActualPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCenterToActualPosition.Image = global::IofffeVesselInfoStream.Properties.Resources._74_location;
            this.btnCenterToActualPosition.Location = new System.Drawing.Point(3, 323);
            this.btnCenterToActualPosition.Name = "btnCenterToActualPosition";
            this.btnCenterToActualPosition.Size = new System.Drawing.Size(24, 34);
            this.btnCenterToActualPosition.TabIndex = 46;
            this.btnCenterToActualPosition.UseVisualStyleBackColor = true;
            this.btnCenterToActualPosition.Click += new System.EventHandler(this.btnCenterToActualPosition_Click);
            // 
            // tpSeaSaveStream
            // 
            this.tpSeaSaveStream.BackColor = System.Drawing.SystemColors.Control;
            this.tpSeaSaveStream.Controls.Add(this.tableLayoutPanel2);
            this.tpSeaSaveStream.Location = new System.Drawing.Point(4, 25);
            this.tpSeaSaveStream.Name = "tpSeaSaveStream";
            this.tpSeaSaveStream.Padding = new System.Windows.Forms.Padding(3);
            this.tpSeaSaveStream.Size = new System.Drawing.Size(809, 544);
            this.tpSeaSaveStream.TabIndex = 1;
            this.tpSeaSaveStream.Text = "SeaSave";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnStartStopSeaSave, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbSeaSaveLog, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(803, 538);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnStartStopSeaSave
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.btnStartStopSeaSave, 2);
            this.btnStartStopSeaSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartStopSeaSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopSeaSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStartStopSeaSave.ForeColor = System.Drawing.Color.Red;
            this.btnStartStopSeaSave.Location = new System.Drawing.Point(3, 3);
            this.btnStartStopSeaSave.Name = "btnStartStopSeaSave";
            this.btnStartStopSeaSave.Size = new System.Drawing.Size(797, 34);
            this.btnStartStopSeaSave.TabIndex = 0;
            this.btnStartStopSeaSave.Text = "CONNECT SeaSave";
            this.btnStartStopSeaSave.UseVisualStyleBackColor = true;
            this.btnStartStopSeaSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbSeaSaveLog
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tbSeaSaveLog, 2);
            this.tbSeaSaveLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSeaSaveLog.Location = new System.Drawing.Point(3, 43);
            this.tbSeaSaveLog.Multiline = true;
            this.tbSeaSaveLog.Name = "tbSeaSaveLog";
            this.tbSeaSaveLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSeaSaveLog.Size = new System.Drawing.Size(797, 492);
            this.tbSeaSaveLog.TabIndex = 1;
            // 
            // bgwSeaSaveSocketStreamReader
            // 
            this.bgwSeaSaveSocketStreamReader.WorkerSupportsCancellation = true;
            this.bgwSeaSaveSocketStreamReader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSeaSaveSocketStreamReader_DoWork);
            // 
            // bgwSeaSaveStreamTextParser
            // 
            this.bgwSeaSaveStreamTextParser.WorkerSupportsCancellation = true;
            // 
            // cbLogNCdata
            // 
            this.cbLogNCdata.AutoSize = true;
            this.cbLogNCdata.Checked = true;
            this.cbLogNCdata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.cbLogNCdata, 3);
            this.cbLogNCdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogNCdata.Location = new System.Drawing.Point(3, 43);
            this.cbLogNCdata.Name = "cbLogNCdata";
            this.cbLogNCdata.Size = new System.Drawing.Size(389, 24);
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
            this.cbLogMeasurementsData.Location = new System.Drawing.Point(438, 43);
            this.cbLogMeasurementsData.Name = "cbLogMeasurementsData";
            this.cbLogMeasurementsData.Size = new System.Drawing.Size(362, 24);
            this.cbLogMeasurementsData.TabIndex = 48;
            this.cbLogMeasurementsData.Text = "Write mesurements entry text file";
            this.cbLogMeasurementsData.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 573);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tpNavAndMeteo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeoTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraphs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbGeoTrackScale)).EndInit();
            this.tpSeaSaveStream.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgwSocketStreamReader;
        private System.ComponentModel.BackgroundWorker bgwStreamTextParser;
        private System.ComponentModel.BackgroundWorker bgwGraphsRenderer;
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
        private System.Windows.Forms.PictureBox pbGeoTrack;
        private System.Windows.Forms.Label lblPressureGraphTitle;
        private System.Windows.Forms.PictureBox pbGraphs;
        private System.Windows.Forms.TrackBar trbGeoTrackScale;
        private System.Windows.Forms.CheckBox cbShowGeoTrack;
        private MRG.Controls.UI.LoadingCircle wcUpdatimgGraphs;
        private System.Windows.Forms.Label lblWaterTemperatureTitle;
        private System.Windows.Forms.Label lblWaterSalinityTitle;
        private System.Windows.Forms.TextBox tbWaterTemperatureValue;
        private System.Windows.Forms.TextBox tbWaterSalinityValue;
        private System.Windows.Forms.Label lblStatusString;
        private System.Windows.Forms.TabPage tpSeaSaveStream;
        private System.Windows.Forms.VScrollBar scrbGeoTrackScrollLatValues;
        private System.Windows.Forms.HScrollBar scrbGeoTrackScrollLonValues;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.ComponentModel.BackgroundWorker bgwSeaSaveSocketStreamReader;
        private System.ComponentModel.BackgroundWorker bgwSeaSaveStreamTextParser;
        private System.Windows.Forms.Button btnStartStopSeaSave;
        private System.Windows.Forms.TextBox tbSeaSaveLog;
        private MRG.Controls.UI.LoadingCircle wcNavDataSoeedControl;
        private MRG.Controls.UI.LoadingCircle wcMeteoDataSpeedControl;
        private System.Windows.Forms.Button btnCenterToActualPosition;
        private System.Windows.Forms.CheckBox cbLogNCdata;
        private System.Windows.Forms.CheckBox cbLogMeasurementsData;

    }
}

