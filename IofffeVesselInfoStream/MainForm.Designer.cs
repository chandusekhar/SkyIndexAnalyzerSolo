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
            this.bgwGeotrackRenderer = new System.ComponentModel.BackgroundWorker();
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
            this.wcUpdatimgGeoTrack = new MRG.Controls.UI.LoadingCircle();
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
            this.cbLogNCdata = new System.Windows.Forms.CheckBox();
            this.cbLogMeasurementsData = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnWaterTempGraph = new System.Windows.Forms.RadioButton();
            this.rbtnWindSpeedGraph = new System.Windows.Forms.RadioButton();
            this.rbtnAirTempGraph = new System.Windows.Forms.RadioButton();
            this.rbtnPressureGraph = new System.Windows.Forms.RadioButton();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnToggleShowGeotrack = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnToggleShowGraphs = new System.Windows.Forms.Button();
            this.wcUpdatimgGraph = new MRG.Controls.UI.LoadingCircle();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnGraphMoveFocusRight = new System.Windows.Forms.Button();
            this.btnGraphZoomIn = new System.Windows.Forms.Button();
            this.btnGraphDefault = new System.Windows.Forms.Button();
            this.btnGraphMoveFocusLeft = new System.Windows.Forms.Button();
            this.btnGraphZoomOut = new System.Windows.Forms.Button();
            this.tpNavAndMeteoOverview = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
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
            this.tpSeaSaveStream = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnStartStopSeaSave = new System.Windows.Forms.Button();
            this.tbSeaSaveLog = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tpNavAndMeteo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeoTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraphs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbGeoTrackScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tpNavAndMeteoOverview.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tpSeaSaveStream.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgwSocketStreamReader
            // 
            this.bgwSocketStreamReader.WorkerSupportsCancellation = true;
            this.bgwSocketStreamReader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSocketStreamReader_DoWork);
            // 
            // bgwGeotrackRenderer
            // 
            this.bgwGeotrackRenderer.WorkerSupportsCancellation = true;
            this.bgwGeotrackRenderer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGraphsRenderer_DoWork);
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
            this.tabControl1.Controls.Add(this.tpSeaSaveStream);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1511, 1086);
            this.tabControl1.TabIndex = 0;
            // 
            // tpNavAndMeteo
            // 
            this.tpNavAndMeteo.BackColor = System.Drawing.SystemColors.Control;
            this.tpNavAndMeteo.Controls.Add(this.tableLayoutPanel1);
            this.tpNavAndMeteo.Location = new System.Drawing.Point(4, 34);
            this.tpNavAndMeteo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpNavAndMeteo.Name = "tpNavAndMeteo";
            this.tpNavAndMeteo.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpNavAndMeteo.Size = new System.Drawing.Size(1503, 1048);
            this.tpNavAndMeteo.TabIndex = 0;
            this.tpNavAndMeteo.Text = "Nav&Meteo";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
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
            this.tableLayoutPanel1.Controls.Add(this.pbGeoTrack, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblPressureGraphTitle, 5, 11);
            this.tableLayoutPanel1.Controls.Add(this.pbGraphs, 5, 13);
            this.tableLayoutPanel1.Controls.Add(this.trbGeoTrackScale, 4, 12);
            this.tableLayoutPanel1.Controls.Add(this.wcUpdatimgGeoTrack, 4, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblWaterTemperatureTitle, 5, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblWaterSalinityTitle, 5, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbWaterTemperatureValue, 6, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbWaterSalinityValue, 6, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblStatusString, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.scrbGeoTrackScrollLatValues, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.scrbGeoTrackScrollLonValues, 1, 16);
            this.tableLayoutPanel1.Controls.Add(this.wcNavDataSoeedControl, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.wcMeteoDataSpeedControl, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCenterToActualPosition, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.cbLogNCdata, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbLogMeasurementsData, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 5, 12);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnToggleShowGeotrack, 3, 11);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.btnToggleShowGraphs, 7, 11);
            this.tableLayoutPanel1.Controls.Add(this.wcUpdatimgGraph, 7, 12);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 5, 16);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 5);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 18;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1495, 1038);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnConnect, 7);
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnConnect.ForeColor = System.Drawing.Color.Red;
            this.btnConnect.Location = new System.Drawing.Point(4, 5);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(1417, 51);
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
            this.lblGPSlatTitle.Location = new System.Drawing.Point(4, 199);
            this.lblGPSlatTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPSlatTitle.Name = "lblGPSlatTitle";
            this.lblGPSlatTitle.Size = new System.Drawing.Size(287, 46);
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
            this.lblGPSlonTitle.Location = new System.Drawing.Point(4, 245);
            this.lblGPSlonTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPSlonTitle.Name = "lblGPSlonTitle";
            this.lblGPSlonTitle.Size = new System.Drawing.Size(287, 46);
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
            this.lblGPSDateTimeTitle.Location = new System.Drawing.Point(4, 153);
            this.lblGPSDateTimeTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPSDateTimeTitle.Name = "lblGPSDateTimeTitle";
            this.lblGPSDateTimeTitle.Size = new System.Drawing.Size(287, 46);
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
            this.lblTrueHeadTitle.Location = new System.Drawing.Point(4, 291);
            this.lblTrueHeadTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrueHeadTitle.Name = "lblTrueHeadTitle";
            this.lblTrueHeadTitle.Size = new System.Drawing.Size(287, 46);
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
            this.lblGyroHeadTitle.Location = new System.Drawing.Point(4, 337);
            this.lblGyroHeadTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGyroHeadTitle.Name = "lblGyroHeadTitle";
            this.lblGyroHeadTitle.Size = new System.Drawing.Size(287, 46);
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
            this.lblSpeedKnotsTitle.Location = new System.Drawing.Point(4, 383);
            this.lblSpeedKnotsTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpeedKnotsTitle.Name = "lblSpeedKnotsTitle";
            this.lblSpeedKnotsTitle.Size = new System.Drawing.Size(287, 46);
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
            this.lblDepthTitle.Location = new System.Drawing.Point(4, 429);
            this.lblDepthTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDepthTitle.Name = "lblDepthTitle";
            this.lblDepthTitle.Size = new System.Drawing.Size(287, 46);
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
            this.tbGPSDateTimeValue.Location = new System.Drawing.Point(299, 158);
            this.tbGPSDateTimeValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbGPSDateTimeValue.Multiline = true;
            this.tbGPSDateTimeValue.Name = "tbGPSDateTimeValue";
            this.tbGPSDateTimeValue.ReadOnly = true;
            this.tbGPSDateTimeValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbGPSlonValue.Location = new System.Drawing.Point(299, 250);
            this.tbGPSlonValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbGPSlonValue.Multiline = true;
            this.tbGPSlonValue.Name = "tbGPSlonValue";
            this.tbGPSlonValue.ReadOnly = true;
            this.tbGPSlonValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbGPSlatValue.Location = new System.Drawing.Point(299, 204);
            this.tbGPSlatValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbGPSlatValue.Multiline = true;
            this.tbGPSlatValue.Name = "tbGPSlatValue";
            this.tbGPSlatValue.ReadOnly = true;
            this.tbGPSlatValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbTrueHeadValue.Location = new System.Drawing.Point(299, 296);
            this.tbTrueHeadValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbTrueHeadValue.Multiline = true;
            this.tbTrueHeadValue.Name = "tbTrueHeadValue";
            this.tbTrueHeadValue.ReadOnly = true;
            this.tbTrueHeadValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbGyroHeadValue.Location = new System.Drawing.Point(299, 342);
            this.tbGyroHeadValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbGyroHeadValue.Multiline = true;
            this.tbGyroHeadValue.Name = "tbGyroHeadValue";
            this.tbGyroHeadValue.ReadOnly = true;
            this.tbGyroHeadValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbSpeedKnotsValue.Location = new System.Drawing.Point(299, 388);
            this.tbSpeedKnotsValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSpeedKnotsValue.Multiline = true;
            this.tbSpeedKnotsValue.Name = "tbSpeedKnotsValue";
            this.tbSpeedKnotsValue.ReadOnly = true;
            this.tbSpeedKnotsValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbDepthValue.Location = new System.Drawing.Point(299, 434);
            this.tbDepthValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDepthValue.Multiline = true;
            this.tbDepthValue.Name = "tbDepthValue";
            this.tbDepthValue.ReadOnly = true;
            this.tbDepthValue.Size = new System.Drawing.Size(437, 36);
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
            this.lblPressureTitle.Location = new System.Drawing.Point(804, 153);
            this.lblPressureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPressureTitle.Name = "lblPressureTitle";
            this.lblPressureTitle.Size = new System.Drawing.Size(242, 46);
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
            this.lblAirTemperatureTitle.Location = new System.Drawing.Point(804, 199);
            this.lblAirTemperatureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAirTemperatureTitle.Name = "lblAirTemperatureTitle";
            this.lblAirTemperatureTitle.Size = new System.Drawing.Size(242, 46);
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
            this.lblWindSpeedTitle.Location = new System.Drawing.Point(804, 245);
            this.lblWindSpeedTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindSpeedTitle.Name = "lblWindSpeedTitle";
            this.lblWindSpeedTitle.Size = new System.Drawing.Size(242, 46);
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
            this.lblWindDirectionTitle.Location = new System.Drawing.Point(804, 291);
            this.lblWindDirectionTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindDirectionTitle.Name = "lblWindDirectionTitle";
            this.lblWindDirectionTitle.Size = new System.Drawing.Size(242, 46);
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
            this.lblRelHumidityTitle.Location = new System.Drawing.Point(804, 337);
            this.lblRelHumidityTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRelHumidityTitle.Name = "lblRelHumidityTitle";
            this.lblRelHumidityTitle.Size = new System.Drawing.Size(242, 46);
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
            this.tbPressureValue.Location = new System.Drawing.Point(1054, 158);
            this.tbPressureValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbPressureValue.Multiline = true;
            this.tbPressureValue.Name = "tbPressureValue";
            this.tbPressureValue.ReadOnly = true;
            this.tbPressureValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbAirTemperatureValue.Location = new System.Drawing.Point(1054, 204);
            this.tbAirTemperatureValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbAirTemperatureValue.Multiline = true;
            this.tbAirTemperatureValue.Name = "tbAirTemperatureValue";
            this.tbAirTemperatureValue.ReadOnly = true;
            this.tbAirTemperatureValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbWindSpeedValue.Location = new System.Drawing.Point(1054, 250);
            this.tbWindSpeedValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbWindSpeedValue.Multiline = true;
            this.tbWindSpeedValue.Name = "tbWindSpeedValue";
            this.tbWindSpeedValue.ReadOnly = true;
            this.tbWindSpeedValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbWindDirectionValue.Location = new System.Drawing.Point(1054, 296);
            this.tbWindDirectionValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbWindDirectionValue.Multiline = true;
            this.tbWindDirectionValue.Name = "tbWindDirectionValue";
            this.tbWindDirectionValue.ReadOnly = true;
            this.tbWindDirectionValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbRelHumidityValue.Location = new System.Drawing.Point(1054, 342);
            this.tbRelHumidityValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbRelHumidityValue.Multiline = true;
            this.tbRelHumidityValue.Name = "tbRelHumidityValue";
            this.tbRelHumidityValue.ReadOnly = true;
            this.tbRelHumidityValue.Size = new System.Drawing.Size(437, 36);
            this.tbRelHumidityValue.TabIndex = 28;
            this.tbRelHumidityValue.Text = "---";
            this.tbRelHumidityValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pbGeoTrack
            // 
            this.pbGeoTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbGeoTrack, 3);
            this.pbGeoTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGeoTrack.Location = new System.Drawing.Point(49, 556);
            this.pbGeoTrack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbGeoTrack.Name = "pbGeoTrack";
            this.tableLayoutPanel1.SetRowSpan(this.pbGeoTrack, 4);
            this.pbGeoTrack.Size = new System.Drawing.Size(687, 383);
            this.pbGeoTrack.TabIndex = 29;
            this.pbGeoTrack.TabStop = false;
            this.pbGeoTrack.Click += new System.EventHandler(this.pbGeoTrack_Click);
            // 
            // lblPressureGraphTitle
            // 
            this.lblPressureGraphTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblPressureGraphTitle, 2);
            this.lblPressureGraphTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPressureGraphTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPressureGraphTitle.Location = new System.Drawing.Point(804, 490);
            this.lblPressureGraphTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPressureGraphTitle.Name = "lblPressureGraphTitle";
            this.lblPressureGraphTitle.Size = new System.Drawing.Size(617, 61);
            this.lblPressureGraphTitle.TabIndex = 31;
            this.lblPressureGraphTitle.Text = "Pressure, Temp., W.Speed, W.temp.";
            this.lblPressureGraphTitle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pbGraphs
            // 
            this.pbGraphs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbGraphs, 3);
            this.pbGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraphs.Location = new System.Drawing.Point(804, 616);
            this.pbGraphs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbGraphs.Name = "pbGraphs";
            this.tableLayoutPanel1.SetRowSpan(this.pbGraphs, 3);
            this.pbGraphs.Size = new System.Drawing.Size(687, 323);
            this.pbGraphs.TabIndex = 32;
            this.pbGraphs.TabStop = false;
            this.pbGraphs.Click += new System.EventHandler(this.pbGraphs_Click);
            // 
            // trbGeoTrackScale
            // 
            this.trbGeoTrackScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbGeoTrackScale.LargeChange = 3;
            this.trbGeoTrackScale.Location = new System.Drawing.Point(744, 556);
            this.trbGeoTrackScale.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trbGeoTrackScale.Name = "trbGeoTrackScale";
            this.trbGeoTrackScale.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tableLayoutPanel1.SetRowSpan(this.trbGeoTrackScale, 4);
            this.trbGeoTrackScale.Size = new System.Drawing.Size(52, 383);
            this.trbGeoTrackScale.TabIndex = 33;
            this.trbGeoTrackScale.ValueChanged += new System.EventHandler(this.trbGeoTrackScale_ValueChanged);
            // 
            // wcUpdatimgGeoTrack
            // 
            this.wcUpdatimgGeoTrack.Active = false;
            this.wcUpdatimgGeoTrack.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.wcUpdatimgGeoTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wcUpdatimgGeoTrack.InnerCircleRadius = 8;
            this.wcUpdatimgGeoTrack.Location = new System.Drawing.Point(744, 495);
            this.wcUpdatimgGeoTrack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.wcUpdatimgGeoTrack.Name = "wcUpdatimgGeoTrack";
            this.wcUpdatimgGeoTrack.NumberSpoke = 24;
            this.wcUpdatimgGeoTrack.OuterCircleRadius = 9;
            this.wcUpdatimgGeoTrack.RotationSpeed = 100;
            this.wcUpdatimgGeoTrack.Size = new System.Drawing.Size(52, 51);
            this.wcUpdatimgGeoTrack.SpokeThickness = 4;
            this.wcUpdatimgGeoTrack.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.wcUpdatimgGeoTrack.TabIndex = 35;
            this.wcUpdatimgGeoTrack.Text = "loadingCircle1";
            this.wcUpdatimgGeoTrack.Visible = false;
            // 
            // lblWaterTemperatureTitle
            // 
            this.lblWaterTemperatureTitle.AutoSize = true;
            this.lblWaterTemperatureTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWaterTemperatureTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWaterTemperatureTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWaterTemperatureTitle.Location = new System.Drawing.Point(804, 383);
            this.lblWaterTemperatureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaterTemperatureTitle.Name = "lblWaterTemperatureTitle";
            this.lblWaterTemperatureTitle.Size = new System.Drawing.Size(242, 46);
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
            this.lblWaterSalinityTitle.Location = new System.Drawing.Point(804, 429);
            this.lblWaterSalinityTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaterSalinityTitle.Name = "lblWaterSalinityTitle";
            this.lblWaterSalinityTitle.Size = new System.Drawing.Size(242, 46);
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
            this.tbWaterTemperatureValue.Location = new System.Drawing.Point(1054, 388);
            this.tbWaterTemperatureValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbWaterTemperatureValue.Multiline = true;
            this.tbWaterTemperatureValue.Name = "tbWaterTemperatureValue";
            this.tbWaterTemperatureValue.ReadOnly = true;
            this.tbWaterTemperatureValue.Size = new System.Drawing.Size(437, 36);
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
            this.tbWaterSalinityValue.Location = new System.Drawing.Point(1054, 434);
            this.tbWaterSalinityValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbWaterSalinityValue.Multiline = true;
            this.tbWaterSalinityValue.Name = "tbWaterSalinityValue";
            this.tbWaterSalinityValue.ReadOnly = true;
            this.tbWaterSalinityValue.Size = new System.Drawing.Size(437, 36);
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
            this.lblStatusString.Location = new System.Drawing.Point(4, 990);
            this.lblStatusString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(1487, 48);
            this.lblStatusString.TabIndex = 40;
            this.lblStatusString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scrbGeoTrackScrollLatValues
            // 
            this.scrbGeoTrackScrollLatValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrbGeoTrackScrollLatValues.LargeChange = 1;
            this.scrbGeoTrackScrollLatValues.Location = new System.Drawing.Point(0, 551);
            this.scrbGeoTrackScrollLatValues.Maximum = 1;
            this.scrbGeoTrackScrollLatValues.Minimum = -1;
            this.scrbGeoTrackScrollLatValues.Name = "scrbGeoTrackScrollLatValues";
            this.tableLayoutPanel1.SetRowSpan(this.scrbGeoTrackScrollLatValues, 4);
            this.scrbGeoTrackScrollLatValues.Size = new System.Drawing.Size(45, 393);
            this.scrbGeoTrackScrollLatValues.TabIndex = 42;
            this.scrbGeoTrackScrollLatValues.ValueChanged += new System.EventHandler(this.scrbGeoTrackScrollLatValues_ValueChanged);
            // 
            // scrbGeoTrackScrollLonValues
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.scrbGeoTrackScrollLonValues, 3);
            this.scrbGeoTrackScrollLonValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrbGeoTrackScrollLonValues.LargeChange = 1;
            this.scrbGeoTrackScrollLonValues.Location = new System.Drawing.Point(45, 944);
            this.scrbGeoTrackScrollLonValues.Maximum = 1;
            this.scrbGeoTrackScrollLonValues.Minimum = -1;
            this.scrbGeoTrackScrollLonValues.Name = "scrbGeoTrackScrollLonValues";
            this.scrbGeoTrackScrollLonValues.Size = new System.Drawing.Size(695, 46);
            this.scrbGeoTrackScrollLonValues.TabIndex = 43;
            this.scrbGeoTrackScrollLonValues.ValueChanged += new System.EventHandler(this.scrbGeoTrackScrollLonValues_ValueChanged);
            // 
            // wcNavDataSoeedControl
            // 
            this.wcNavDataSoeedControl.Active = false;
            this.wcNavDataSoeedControl.Color = System.Drawing.Color.Red;
            this.tableLayoutPanel1.SetColumnSpan(this.wcNavDataSoeedControl, 2);
            this.wcNavDataSoeedControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wcNavDataSoeedControl.InnerCircleRadius = 5;
            this.wcNavDataSoeedControl.Location = new System.Drawing.Point(299, 112);
            this.wcNavDataSoeedControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.wcNavDataSoeedControl.Name = "wcNavDataSoeedControl";
            this.wcNavDataSoeedControl.NumberSpoke = 12;
            this.wcNavDataSoeedControl.OuterCircleRadius = 11;
            this.wcNavDataSoeedControl.RotationSpeed = 1;
            this.wcNavDataSoeedControl.Size = new System.Drawing.Size(437, 36);
            this.wcNavDataSoeedControl.SpokeThickness = 2;
            this.wcNavDataSoeedControl.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.wcNavDataSoeedControl.TabIndex = 44;
            this.wcNavDataSoeedControl.Text = "loadingCircle1";
            // 
            // wcMeteoDataSpeedControl
            // 
            this.wcMeteoDataSpeedControl.Active = false;
            this.wcMeteoDataSpeedControl.Color = System.Drawing.Color.Red;
            this.tableLayoutPanel1.SetColumnSpan(this.wcMeteoDataSpeedControl, 2);
            this.wcMeteoDataSpeedControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.wcMeteoDataSpeedControl.InnerCircleRadius = 5;
            this.wcMeteoDataSpeedControl.Location = new System.Drawing.Point(1054, 112);
            this.wcMeteoDataSpeedControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.wcMeteoDataSpeedControl.Name = "wcMeteoDataSpeedControl";
            this.wcMeteoDataSpeedControl.NumberSpoke = 12;
            this.wcMeteoDataSpeedControl.OuterCircleRadius = 11;
            this.wcMeteoDataSpeedControl.RotationSpeed = 1;
            this.wcMeteoDataSpeedControl.Size = new System.Drawing.Size(437, 35);
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
            this.btnCenterToActualPosition.Location = new System.Drawing.Point(4, 495);
            this.btnCenterToActualPosition.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCenterToActualPosition.Name = "btnCenterToActualPosition";
            this.btnCenterToActualPosition.Size = new System.Drawing.Size(37, 51);
            this.btnCenterToActualPosition.TabIndex = 46;
            this.btnCenterToActualPosition.UseVisualStyleBackColor = true;
            this.btnCenterToActualPosition.Click += new System.EventHandler(this.btnCenterToActualPosition_Click);
            // 
            // cbLogNCdata
            // 
            this.cbLogNCdata.AutoSize = true;
            this.cbLogNCdata.Checked = true;
            this.cbLogNCdata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.cbLogNCdata, 4);
            this.cbLogNCdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogNCdata.Location = new System.Drawing.Point(4, 66);
            this.cbLogNCdata.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbLogNCdata.Name = "cbLogNCdata";
            this.cbLogNCdata.Size = new System.Drawing.Size(732, 36);
            this.cbLogNCdata.TabIndex = 47;
            this.cbLogNCdata.Text = "log data to *.nc and *.log files";
            this.cbLogNCdata.UseVisualStyleBackColor = true;
            // 
            // cbLogMeasurementsData
            // 
            this.cbLogMeasurementsData.AutoSize = true;
            this.cbLogMeasurementsData.Checked = true;
            this.cbLogMeasurementsData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.cbLogMeasurementsData, 3);
            this.cbLogMeasurementsData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogMeasurementsData.Location = new System.Drawing.Point(804, 66);
            this.cbLogMeasurementsData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbLogMeasurementsData.Name = "cbLogMeasurementsData";
            this.cbLogMeasurementsData.Size = new System.Drawing.Size(687, 36);
            this.cbLogMeasurementsData.TabIndex = 48;
            this.cbLogMeasurementsData.Text = "Write mesurements entry text file";
            this.cbLogMeasurementsData.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.rbtnWaterTempGraph);
            this.groupBox1.Controls.Add(this.rbtnWindSpeedGraph);
            this.groupBox1.Controls.Add(this.rbtnAirTempGraph);
            this.groupBox1.Controls.Add(this.rbtnPressureGraph);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(803, 554);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(619, 54);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            // 
            // rbtnWaterTempGraph
            // 
            this.rbtnWaterTempGraph.AutoSize = true;
            this.rbtnWaterTempGraph.Location = new System.Drawing.Point(372, 19);
            this.rbtnWaterTempGraph.Name = "rbtnWaterTempGraph";
            this.rbtnWaterTempGraph.Size = new System.Drawing.Size(143, 29);
            this.rbtnWaterTempGraph.TabIndex = 3;
            this.rbtnWaterTempGraph.TabStop = true;
            this.rbtnWaterTempGraph.Text = "Water temp.";
            this.rbtnWaterTempGraph.UseVisualStyleBackColor = true;
            this.rbtnWaterTempGraph.CheckedChanged += new System.EventHandler(this.rbtnGraphVarChanged);
            // 
            // rbtnWindSpeedGraph
            // 
            this.rbtnWindSpeedGraph.AutoSize = true;
            this.rbtnWindSpeedGraph.Location = new System.Drawing.Point(250, 19);
            this.rbtnWindSpeedGraph.Name = "rbtnWindSpeedGraph";
            this.rbtnWindSpeedGraph.Size = new System.Drawing.Size(116, 29);
            this.rbtnWindSpeedGraph.TabIndex = 2;
            this.rbtnWindSpeedGraph.TabStop = true;
            this.rbtnWindSpeedGraph.Text = "W.speed";
            this.rbtnWindSpeedGraph.UseVisualStyleBackColor = true;
            this.rbtnWindSpeedGraph.CheckedChanged += new System.EventHandler(this.rbtnGraphVarChanged);
            // 
            // rbtnAirTempGraph
            // 
            this.rbtnAirTempGraph.AutoSize = true;
            this.rbtnAirTempGraph.Location = new System.Drawing.Point(133, 19);
            this.rbtnAirTempGraph.Name = "rbtnAirTempGraph";
            this.rbtnAirTempGraph.Size = new System.Drawing.Size(114, 29);
            this.rbtnAirTempGraph.TabIndex = 1;
            this.rbtnAirTempGraph.TabStop = true;
            this.rbtnAirTempGraph.Text = "Air temp.";
            this.rbtnAirTempGraph.UseVisualStyleBackColor = true;
            this.rbtnAirTempGraph.CheckedChanged += new System.EventHandler(this.rbtnGraphVarChanged);
            // 
            // rbtnPressureGraph
            // 
            this.rbtnPressureGraph.AutoSize = true;
            this.rbtnPressureGraph.Checked = true;
            this.rbtnPressureGraph.Location = new System.Drawing.Point(12, 19);
            this.rbtnPressureGraph.Name = "rbtnPressureGraph";
            this.rbtnPressureGraph.Size = new System.Drawing.Size(115, 29);
            this.rbtnPressureGraph.TabIndex = 0;
            this.rbtnPressureGraph.TabStop = true;
            this.rbtnPressureGraph.Text = "Pressure";
            this.rbtnPressureGraph.UseVisualStyleBackColor = true;
            this.rbtnPressureGraph.CheckedChanged += new System.EventHandler(this.rbtnGraphVarChanged);
            // 
            // btnProperties
            // 
            this.btnProperties.BackgroundImage = global::IofffeVesselInfoStream.Properties.Resources.gearIcon;
            this.btnProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Location = new System.Drawing.Point(1428, 3);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(64, 55);
            this.btnProperties.TabIndex = 50;
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnToggleShowGeotrack
            // 
            this.btnToggleShowGeotrack.BackColor = System.Drawing.Color.Gainsboro;
            this.btnToggleShowGeotrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToggleShowGeotrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleShowGeotrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnToggleShowGeotrack.Location = new System.Drawing.Point(673, 493);
            this.btnToggleShowGeotrack.Name = "btnToggleShowGeotrack";
            this.btnToggleShowGeotrack.Size = new System.Drawing.Size(64, 55);
            this.btnToggleShowGeotrack.TabIndex = 51;
            this.btnToggleShowGeotrack.Text = "OFF";
            this.btnToggleShowGeotrack.UseVisualStyleBackColor = false;
            this.btnToggleShowGeotrack.Click += new System.EventHandler(this.btnToggleShowGeotrack_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(48, 490);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(619, 61);
            this.label1.TabIndex = 52;
            this.label1.Text = "Geotrack";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnToggleShowGraphs
            // 
            this.btnToggleShowGraphs.BackColor = System.Drawing.Color.Gainsboro;
            this.btnToggleShowGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToggleShowGraphs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleShowGraphs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnToggleShowGraphs.Location = new System.Drawing.Point(1428, 493);
            this.btnToggleShowGraphs.Name = "btnToggleShowGraphs";
            this.btnToggleShowGraphs.Size = new System.Drawing.Size(64, 55);
            this.btnToggleShowGraphs.TabIndex = 53;
            this.btnToggleShowGraphs.Text = "OFF";
            this.btnToggleShowGraphs.UseVisualStyleBackColor = false;
            this.btnToggleShowGraphs.Click += new System.EventHandler(this.btnToggleShowGraphs_Click);
            // 
            // wcUpdatimgGraph
            // 
            this.wcUpdatimgGraph.Active = false;
            this.wcUpdatimgGraph.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.wcUpdatimgGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wcUpdatimgGraph.InnerCircleRadius = 8;
            this.wcUpdatimgGraph.Location = new System.Drawing.Point(1428, 554);
            this.wcUpdatimgGraph.Name = "wcUpdatimgGraph";
            this.wcUpdatimgGraph.NumberSpoke = 24;
            this.wcUpdatimgGraph.OuterCircleRadius = 9;
            this.wcUpdatimgGraph.RotationSpeed = 100;
            this.wcUpdatimgGraph.Size = new System.Drawing.Size(64, 54);
            this.wcUpdatimgGraph.SpokeThickness = 4;
            this.wcUpdatimgGraph.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.wcUpdatimgGraph.TabIndex = 54;
            this.wcUpdatimgGraph.Text = "loadingCircle1";
            this.wcUpdatimgGraph.Visible = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel4, 3);
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel4.Controls.Add(this.btnGraphMoveFocusRight, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnGraphZoomIn, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnGraphDefault, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnGraphMoveFocusLeft, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnGraphZoomOut, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(803, 947);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(689, 40);
            this.tableLayoutPanel4.TabIndex = 56;
            // 
            // btnGraphMoveFocusRight
            // 
            this.btnGraphMoveFocusRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphMoveFocusRight.Location = new System.Drawing.Point(611, 3);
            this.btnGraphMoveFocusRight.Name = "btnGraphMoveFocusRight";
            this.btnGraphMoveFocusRight.Size = new System.Drawing.Size(75, 34);
            this.btnGraphMoveFocusRight.TabIndex = 1;
            this.btnGraphMoveFocusRight.Text = ">>";
            this.btnGraphMoveFocusRight.UseVisualStyleBackColor = true;
            this.btnGraphMoveFocusRight.Click += new System.EventHandler(this.btnGraphMoveFocusRight_Click);
            // 
            // btnGraphZoomIn
            // 
            this.btnGraphZoomIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphZoomIn.Location = new System.Drawing.Point(435, 3);
            this.btnGraphZoomIn.Name = "btnGraphZoomIn";
            this.btnGraphZoomIn.Size = new System.Drawing.Size(170, 34);
            this.btnGraphZoomIn.TabIndex = 3;
            this.btnGraphZoomIn.Text = "zoom in";
            this.btnGraphZoomIn.UseVisualStyleBackColor = true;
            this.btnGraphZoomIn.Click += new System.EventHandler(this.btnGraphZoomIn_Click);
            // 
            // btnGraphDefault
            // 
            this.btnGraphDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphDefault.Location = new System.Drawing.Point(259, 3);
            this.btnGraphDefault.Name = "btnGraphDefault";
            this.btnGraphDefault.Size = new System.Drawing.Size(170, 34);
            this.btnGraphDefault.TabIndex = 4;
            this.btnGraphDefault.Text = "default";
            this.btnGraphDefault.UseVisualStyleBackColor = true;
            this.btnGraphDefault.Click += new System.EventHandler(this.btnGraphDefault_Click);
            // 
            // btnGraphMoveFocusLeft
            // 
            this.btnGraphMoveFocusLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphMoveFocusLeft.Location = new System.Drawing.Point(3, 3);
            this.btnGraphMoveFocusLeft.Name = "btnGraphMoveFocusLeft";
            this.btnGraphMoveFocusLeft.Size = new System.Drawing.Size(74, 34);
            this.btnGraphMoveFocusLeft.TabIndex = 0;
            this.btnGraphMoveFocusLeft.Text = "<<";
            this.btnGraphMoveFocusLeft.UseVisualStyleBackColor = true;
            this.btnGraphMoveFocusLeft.Click += new System.EventHandler(this.btnGraphMoveFocusLeft_Click);
            // 
            // btnGraphZoomOut
            // 
            this.btnGraphZoomOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphZoomOut.Location = new System.Drawing.Point(83, 3);
            this.btnGraphZoomOut.Name = "btnGraphZoomOut";
            this.btnGraphZoomOut.Size = new System.Drawing.Size(170, 34);
            this.btnGraphZoomOut.TabIndex = 2;
            this.btnGraphZoomOut.Text = "zoom out";
            this.btnGraphZoomOut.UseVisualStyleBackColor = true;
            this.btnGraphZoomOut.Click += new System.EventHandler(this.btnGraphZoomOut_Click);
            // 
            // tpNavAndMeteoOverview
            // 
            this.tpNavAndMeteoOverview.BackColor = System.Drawing.SystemColors.Control;
            this.tpNavAndMeteoOverview.Controls.Add(this.tableLayoutPanel3);
            this.tpNavAndMeteoOverview.Location = new System.Drawing.Point(4, 34);
            this.tpNavAndMeteoOverview.Name = "tpNavAndMeteoOverview";
            this.tpNavAndMeteoOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tpNavAndMeteoOverview.Size = new System.Drawing.Size(1503, 1048);
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
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 10;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1497, 1042);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label2, 4);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(590, 60);
            this.label2.TabIndex = 0;
            this.label2.Text = "Form data record log file for date-time:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBox1
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.maskedTextBox1, 3);
            this.maskedTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox1.Location = new System.Drawing.Point(599, 3);
            this.maskedTextBox1.Mask = "0000-00-00T90:00:00";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(441, 44);
            this.maskedTextBox1.TabIndex = 1;
            // 
            // btnFindInfo
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnFindInfo, 2);
            this.btnFindInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindInfo.Location = new System.Drawing.Point(1046, 3);
            this.btnFindInfo.Name = "btnFindInfo";
            this.btnFindInfo.Size = new System.Drawing.Size(292, 54);
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
            this.btnWriteFile.Location = new System.Drawing.Point(1344, 3);
            this.btnWriteFile.Name = "btnWriteFile";
            this.btnWriteFile.Size = new System.Drawing.Size(150, 54);
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
            this.lblFoundNavDataTitle.Location = new System.Drawing.Point(3, 140);
            this.lblFoundNavDataTitle.Name = "lblFoundNavDataTitle";
            this.lblFoundNavDataTitle.Size = new System.Drawing.Size(143, 180);
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
            this.lblFoundMeteoDataTitle.Location = new System.Drawing.Point(3, 320);
            this.lblFoundMeteoDataTitle.Name = "lblFoundMeteoDataTitle";
            this.lblFoundMeteoDataTitle.Size = new System.Drawing.Size(143, 180);
            this.lblFoundMeteoDataTitle.TabIndex = 7;
            this.lblFoundMeteoDataTitle.Text = "meteo:";
            this.lblFoundMeteoDataTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // prbSearchingProgress
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.prbSearchingProgress, 10);
            this.prbSearchingProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbSearchingProgress.Location = new System.Drawing.Point(3, 123);
            this.prbSearchingProgress.Name = "prbSearchingProgress";
            this.prbSearchingProgress.Size = new System.Drawing.Size(1491, 14);
            this.prbSearchingProgress.TabIndex = 8;
            // 
            // lblFoundNavData
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.lblFoundNavData, 9);
            this.lblFoundNavData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFoundNavData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFoundNavData.Location = new System.Drawing.Point(152, 143);
            this.lblFoundNavData.Multiline = true;
            this.lblFoundNavData.Name = "lblFoundNavData";
            this.lblFoundNavData.ReadOnly = true;
            this.lblFoundNavData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lblFoundNavData.Size = new System.Drawing.Size(1342, 174);
            this.lblFoundNavData.TabIndex = 9;
            this.lblFoundNavData.Text = "---";
            // 
            // lblFoundMetData
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.lblFoundMetData, 9);
            this.lblFoundMetData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFoundMetData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFoundMetData.Location = new System.Drawing.Point(152, 323);
            this.lblFoundMetData.Multiline = true;
            this.lblFoundMetData.Name = "lblFoundMetData";
            this.lblFoundMetData.ReadOnly = true;
            this.lblFoundMetData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lblFoundMetData.Size = new System.Drawing.Size(1342, 174);
            this.lblFoundMetData.TabIndex = 10;
            this.lblFoundMetData.Text = "---";
            // 
            // lblPlusOneHour
            // 
            this.lblPlusOneHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPlusOneHour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPlusOneHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPlusOneHour.Location = new System.Drawing.Point(748, 63);
            this.lblPlusOneHour.Name = "lblPlusOneHour";
            this.lblPlusOneHour.Size = new System.Drawing.Size(143, 54);
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
            this.btnPlusOneDay.Location = new System.Drawing.Point(599, 63);
            this.btnPlusOneDay.Name = "btnPlusOneDay";
            this.btnPlusOneDay.Size = new System.Drawing.Size(143, 54);
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
            this.label3.Location = new System.Drawing.Point(3, 701);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(590, 60);
            this.label3.TabIndex = 13;
            this.label3.Text = "find and repair lost data records for date:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBox2
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.maskedTextBox2, 2);
            this.maskedTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox2.Location = new System.Drawing.Point(599, 704);
            this.maskedTextBox2.Mask = "0000-00-00";
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.Size = new System.Drawing.Size(292, 44);
            this.maskedTextBox2.TabIndex = 14;
            // 
            // btnFindIOFFEdataFiles
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnFindIOFFEdataFiles, 4);
            this.btnFindIOFFEdataFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindIOFFEdataFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindIOFFEdataFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindIOFFEdataFiles.Location = new System.Drawing.Point(897, 704);
            this.btnFindIOFFEdataFiles.Name = "btnFindIOFFEdataFiles";
            this.btnFindIOFFEdataFiles.Size = new System.Drawing.Size(597, 54);
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
            this.btnRepairAndWriteNCfile.Location = new System.Drawing.Point(897, 764);
            this.btnRepairAndWriteNCfile.Name = "btnRepairAndWriteNCfile";
            this.btnRepairAndWriteNCfile.Size = new System.Drawing.Size(597, 54);
            this.btnRepairAndWriteNCfile.TabIndex = 16;
            this.btnRepairAndWriteNCfile.Text = "re-form nc-file with additional data";
            this.btnRepairAndWriteNCfile.UseVisualStyleBackColor = true;
            this.btnRepairAndWriteNCfile.Click += new System.EventHandler(this.btnRepairAndWriteNCfile_Click);
            // 
            // prbMissingInfoSearchingProgress
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.prbMissingInfoSearchingProgress, 10);
            this.prbMissingInfoSearchingProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbMissingInfoSearchingProgress.Location = new System.Drawing.Point(3, 824);
            this.prbMissingInfoSearchingProgress.Name = "prbMissingInfoSearchingProgress";
            this.prbMissingInfoSearchingProgress.Size = new System.Drawing.Size(1491, 14);
            this.prbMissingInfoSearchingProgress.TabIndex = 17;
            // 
            // tpSeaSaveStream
            // 
            this.tpSeaSaveStream.BackColor = System.Drawing.SystemColors.Control;
            this.tpSeaSaveStream.Controls.Add(this.tableLayoutPanel2);
            this.tpSeaSaveStream.Location = new System.Drawing.Point(4, 34);
            this.tpSeaSaveStream.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpSeaSaveStream.Name = "tpSeaSaveStream";
            this.tpSeaSaveStream.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpSeaSaveStream.Size = new System.Drawing.Size(1503, 1048);
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 5);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1495, 1038);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnStartStopSeaSave
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.btnStartStopSeaSave, 2);
            this.btnStartStopSeaSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartStopSeaSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopSeaSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStartStopSeaSave.ForeColor = System.Drawing.Color.Red;
            this.btnStartStopSeaSave.Location = new System.Drawing.Point(4, 5);
            this.btnStartStopSeaSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStartStopSeaSave.Name = "btnStartStopSeaSave";
            this.btnStartStopSeaSave.Size = new System.Drawing.Size(1487, 51);
            this.btnStartStopSeaSave.TabIndex = 0;
            this.btnStartStopSeaSave.Text = "CONNECT SeaSave";
            this.btnStartStopSeaSave.UseVisualStyleBackColor = true;
            //this.btnStartStopSeaSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbSeaSaveLog
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tbSeaSaveLog, 2);
            this.tbSeaSaveLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSeaSaveLog.Location = new System.Drawing.Point(4, 66);
            this.tbSeaSaveLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSeaSaveLog.Multiline = true;
            this.tbSeaSaveLog.Name = "tbSeaSaveLog";
            this.tbSeaSaveLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSeaSaveLog.Size = new System.Drawing.Size(1487, 967);
            this.tbSeaSaveLog.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1511, 1086);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "Ioffe nav&meteo stream";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.tabControl1.ResumeLayout(false);
            this.tpNavAndMeteo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeoTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraphs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbGeoTrackScale)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tpNavAndMeteoOverview.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tpSeaSaveStream.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgwSocketStreamReader;
        private System.ComponentModel.BackgroundWorker bgwGeotrackRenderer;
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
        private MRG.Controls.UI.LoadingCircle wcUpdatimgGeoTrack;
        private System.Windows.Forms.Label lblWaterTemperatureTitle;
        private System.Windows.Forms.Label lblWaterSalinityTitle;
        private System.Windows.Forms.TextBox tbWaterTemperatureValue;
        private System.Windows.Forms.TextBox tbWaterSalinityValue;
        private System.Windows.Forms.Label lblStatusString;
        private System.Windows.Forms.TabPage tpSeaSaveStream;
        private System.Windows.Forms.VScrollBar scrbGeoTrackScrollLatValues;
        private System.Windows.Forms.HScrollBar scrbGeoTrackScrollLonValues;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnStartStopSeaSave;
        private System.Windows.Forms.TextBox tbSeaSaveLog;
        private MRG.Controls.UI.LoadingCircle wcNavDataSoeedControl;
        private MRG.Controls.UI.LoadingCircle wcMeteoDataSpeedControl;
        private System.Windows.Forms.Button btnCenterToActualPosition;
        private System.Windows.Forms.CheckBox cbLogNCdata;
        private System.Windows.Forms.CheckBox cbLogMeasurementsData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.RadioButton rbtnPressureGraph;
        private System.Windows.Forms.RadioButton rbtnAirTempGraph;
        private System.Windows.Forms.RadioButton rbtnWindSpeedGraph;
        private System.Windows.Forms.RadioButton rbtnWaterTempGraph;
        private System.Windows.Forms.Button btnToggleShowGeotrack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnToggleShowGraphs;
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
        private MRG.Controls.UI.LoadingCircle wcUpdatimgGraph;
        private System.Windows.Forms.Button btnGraphMoveFocusLeft;
        private System.Windows.Forms.Button btnGraphMoveFocusRight;
        private System.Windows.Forms.Button btnGraphZoomOut;
        private System.Windows.Forms.Button btnGraphZoomIn;
        private System.Windows.Forms.Button btnGraphDefault;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;

    }
}

