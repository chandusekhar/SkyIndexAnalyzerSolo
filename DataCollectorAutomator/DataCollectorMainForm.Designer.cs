namespace DataCollectorAutomator
{
    partial class DataCollectorMainForm
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
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbBcstListeningPort = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnFindArduino = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStartStopBdcstListening = new System.Windows.Forms.Button();
            this.btnStartStopCollecting = new System.Windows.Forms.Button();
            this.SearchingArduinoProcessCircle = new MRG.Controls.UI.LoadingCircle();
            this.StartStopDataCollectingWaitingCircle = new MRG.Controls.UI.LoadingCircle();
            this.btnCollectMostClose = new System.Windows.Forms.Button();
            this.btnCollectImmediately = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblNextShotIn = new System.Windows.Forms.Label();
            this.lblSinceLastShot = new System.Windows.Forms.Label();
            this.lblAccelerometerSign = new System.Windows.Forms.Label();
            this.lblAccDevMeanMagnTitle = new System.Windows.Forms.Label();
            this.lblAccDevMeanAngleTitle = new System.Windows.Forms.Label();
            this.lblAccDevMeanMagnValue = new System.Windows.Forms.Label();
            this.lblAccDevMeanAngleValue = new System.Windows.Forms.Label();
            this.lblCompass = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblMagnDataX = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblMagnDataY = new System.Windows.Forms.Label();
            this.lblMagnDataZ = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblMagnDataHeading = new System.Windows.Forms.Label();
            this.pbThumbPreview = new System.Windows.Forms.PictureBox();
            this.lblLonTitle = new System.Windows.Forms.Label();
            this.lblLonValue = new System.Windows.Forms.Label();
            this.lblLatTitle = new System.Windows.Forms.Label();
            this.lblLatValue = new System.Windows.Forms.Label();
            this.lblGPStitle = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblUTCTimeValue = new System.Windows.Forms.Label();
            this.lblPressureTitle = new System.Windows.Forms.Label();
            this.lblPressureValue = new System.Windows.Forms.Label();
            this.processCircle = new MRG.Controls.UI.LoadingCircle();
            this.tabPageBcstLog = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbMainLog = new System.Windows.Forms.TextBox();
            this.btnSwapBcstLog = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSwitchShowingTotalLog = new System.Windows.Forms.Button();
            this.SensorsCalibration = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblCaughtMagnCalibrationValue = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnCalibrateMagnetometer = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblAccelCalibrationCurrentX = new System.Windows.Forms.Label();
            this.lblAccelCalibrationCurrentY = new System.Windows.Forms.Label();
            this.lblAccelCalibrationCurrentZ = new System.Windows.Forms.Label();
            this.btnCalibrateAccelerometer = new System.Windows.Forms.Button();
            this.lblAccelCalibrationX = new System.Windows.Forms.Label();
            this.lblAccelCalibrationY = new System.Windows.Forms.Label();
            this.lblAccelCalibrationZ = new System.Windows.Forms.Label();
            this.lblCalculationStatistics = new System.Windows.Forms.Label();
            this.lblStDevX = new System.Windows.Forms.Label();
            this.lblStDevY = new System.Windows.Forms.Label();
            this.lblStDevZ = new System.Windows.Forms.Label();
            this.btnSaveAccel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblMagnCalibrationCurrentX = new System.Windows.Forms.Label();
            this.lblMagnCalibrationCurrentY = new System.Windows.Forms.Label();
            this.lblMagnCalibrationCurrentZ = new System.Windows.Forms.Label();
            this.lblMagnCalibrationX = new System.Windows.Forms.Label();
            this.lblMagnCalibrationY = new System.Windows.Forms.Label();
            this.lblMagnCalibrationZ = new System.Windows.Forms.Label();
            this.lblMagnStDevX = new System.Windows.Forms.Label();
            this.lblMagnStDevY = new System.Windows.Forms.Label();
            this.lblMagnStDevZ = new System.Windows.Forms.Label();
            this.lblMagnCalculationStatistics = new System.Windows.Forms.Label();
            this.btnMagnSaveCalibration = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.tbCurrentCompassHeadingValue = new System.Windows.Forms.TextBox();
            this.Preferencies = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.camshotPeriodLabel = new System.Windows.Forms.Label();
            this.tbCamShotPeriod = new System.Windows.Forms.MaskedTextBox();
            this.camshotPeriodDataSavingCircle = new MRG.Controls.UI.LoadingCircle();
            this.camIPLabel = new System.Windows.Forms.Label();
            this.tbCamIP = new System.Windows.Forms.MaskedTextBox();
            this.ipAddrValidatingCircle = new MRG.Controls.UI.LoadingCircle();
            this.camUNameLabel = new System.Windows.Forms.Label();
            this.tbCamUName = new System.Windows.Forms.TextBox();
            this.camPWDLabel = new System.Windows.Forms.Label();
            this.tbCamPWD = new System.Windows.Forms.TextBox();
            this.arduinoBoardSearchingWorker = new System.ComponentModel.BackgroundWorker();
            this.udpCatchingJob = new System.ComponentModel.BackgroundWorker();
            this.ArduinoRequestExpectant = new System.ComponentModel.BackgroundWorker();
            this.dataCollector = new System.ComponentModel.BackgroundWorker();
            this.accelCalibrator = new System.ComponentModel.BackgroundWorker();
            this.magnCalibrator = new System.ComponentModel.BackgroundWorker();
            this.bgwUDPmessagesParser = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbPreview)).BeginInit();
            this.tabPageBcstLog.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SensorsCalibration.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.Preferencies.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMain);
            this.tabControl1.Controls.Add(this.tabPageBcstLog);
            this.tabControl1.Controls.Add(this.SensorsCalibration);
            this.tabControl1.Controls.Add(this.Preferencies);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(967, 564);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.tableLayoutPanel1);
            this.tabPageMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(959, 538);
            this.tabPageMain.TabIndex = 1;
            this.tabPageMain.Text = "Main";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 19;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tbBcstListeningPort, 17, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFindArduino, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStartStopBdcstListening, 13, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStartStopCollecting, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.SearchingArduinoProcessCircle, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.StartStopDataCollectingWaitingCircle, 18, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCollectMostClose, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCollectImmediately, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblNextShotIn, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblSinceLastShot, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblAccelerometerSign, 10, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanMagnTitle, 13, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanAngleTitle, 15, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanMagnValue, 13, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanAngleValue, 15, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblCompass, 10, 6);
            this.tableLayoutPanel1.Controls.Add(this.label14, 13, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblMagnDataX, 13, 7);
            this.tableLayoutPanel1.Controls.Add(this.label15, 15, 6);
            this.tableLayoutPanel1.Controls.Add(this.label16, 17, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblMagnDataY, 15, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblMagnDataZ, 17, 7);
            this.tableLayoutPanel1.Controls.Add(this.label20, 13, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblMagnDataHeading, 13, 9);
            this.tableLayoutPanel1.Controls.Add(this.pbThumbPreview, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblLonTitle, 16, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblLonValue, 16, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblLatTitle, 13, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblLatValue, 13, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblGPStitle, 10, 11);
            this.tableLayoutPanel1.Controls.Add(this.label17, 13, 14);
            this.tableLayoutPanel1.Controls.Add(this.lblUTCTimeValue, 13, 15);
            this.tableLayoutPanel1.Controls.Add(this.lblPressureTitle, 10, 18);
            this.tableLayoutPanel1.Controls.Add(this.lblPressureValue, 13, 18);
            this.tableLayoutPanel1.Controls.Add(this.processCircle, 17, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 19;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(953, 532);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tbBcstListeningPort
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbBcstListeningPort, 2);
            this.tbBcstListeningPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBcstListeningPort.Location = new System.Drawing.Point(853, 3);
            this.tbBcstListeningPort.Multiline = true;
            this.tbBcstListeningPort.Name = "tbBcstListeningPort";
            this.tbBcstListeningPort.Size = new System.Drawing.Size(97, 24);
            this.tbBcstListeningPort.TabIndex = 45;
            this.tbBcstListeningPort.TextChanged += new System.EventHandler(this.tbBcstListeningPort_TextChanged);
            // 
            // textBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox2, 4);
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(3, 3);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(194, 24);
            this.textBox2.TabIndex = 40;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnFindArduino
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnFindArduino, 3);
            this.btnFindArduino.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnFindArduino.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindArduino.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindArduino.Location = new System.Drawing.Point(203, 3);
            this.btnFindArduino.Name = "btnFindArduino";
            this.btnFindArduino.Size = new System.Drawing.Size(144, 24);
            this.btnFindArduino.TabIndex = 41;
            this.btnFindArduino.Text = "search for board";
            this.btnFindArduino.UseVisualStyleBackColor = true;
            this.btnFindArduino.Click += new System.EventHandler(this.btnFindArduino_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 4);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(453, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 30);
            this.label2.TabIndex = 42;
            this.label2.Text = "Broadcast listening";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStartStopBdcstListening
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnStartStopBdcstListening, 4);
            this.btnStartStopBdcstListening.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartStopBdcstListening.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopBdcstListening.Location = new System.Drawing.Point(653, 3);
            this.btnStartStopBdcstListening.Name = "btnStartStopBdcstListening";
            this.btnStartStopBdcstListening.Size = new System.Drawing.Size(194, 24);
            this.btnStartStopBdcstListening.TabIndex = 43;
            this.btnStartStopBdcstListening.Text = "Start listening on port:";
            this.btnStartStopBdcstListening.UseVisualStyleBackColor = true;
            this.btnStartStopBdcstListening.Click += new System.EventHandler(this.btnStartStopBdcstListening_Click);
            // 
            // btnStartStopCollecting
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnStartStopCollecting, 18);
            this.btnStartStopCollecting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartStopCollecting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopCollecting.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStartStopCollecting.ForeColor = System.Drawing.Color.Red;
            this.btnStartStopCollecting.Location = new System.Drawing.Point(3, 43);
            this.btnStartStopCollecting.Name = "btnStartStopCollecting";
            this.btnStartStopCollecting.Size = new System.Drawing.Size(894, 44);
            this.btnStartStopCollecting.TabIndex = 47;
            this.btnStartStopCollecting.Text = "Start collecting data";
            this.btnStartStopCollecting.UseVisualStyleBackColor = true;
            this.btnStartStopCollecting.Click += new System.EventHandler(this.btnStartStopCollecting_Click);
            // 
            // SearchingArduinoProcessCircle
            // 
            this.SearchingArduinoProcessCircle.Active = false;
            this.SearchingArduinoProcessCircle.Color = System.Drawing.Color.DarkGray;
            this.SearchingArduinoProcessCircle.InnerCircleRadius = 8;
            this.SearchingArduinoProcessCircle.Location = new System.Drawing.Point(353, 3);
            this.SearchingArduinoProcessCircle.Name = "SearchingArduinoProcessCircle";
            this.SearchingArduinoProcessCircle.NumberSpoke = 24;
            this.SearchingArduinoProcessCircle.OuterCircleRadius = 9;
            this.SearchingArduinoProcessCircle.RotationSpeed = 100;
            this.SearchingArduinoProcessCircle.Size = new System.Drawing.Size(31, 23);
            this.SearchingArduinoProcessCircle.SpokeThickness = 4;
            this.SearchingArduinoProcessCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.SearchingArduinoProcessCircle.TabIndex = 75;
            this.SearchingArduinoProcessCircle.Text = "loadingCircle1";
            this.SearchingArduinoProcessCircle.Visible = false;
            // 
            // StartStopDataCollectingWaitingCircle
            // 
            this.StartStopDataCollectingWaitingCircle.Active = false;
            this.StartStopDataCollectingWaitingCircle.Color = System.Drawing.Color.Red;
            this.StartStopDataCollectingWaitingCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartStopDataCollectingWaitingCircle.ForeColor = System.Drawing.Color.Red;
            this.StartStopDataCollectingWaitingCircle.InnerCircleRadius = 8;
            this.StartStopDataCollectingWaitingCircle.Location = new System.Drawing.Point(903, 43);
            this.StartStopDataCollectingWaitingCircle.Name = "StartStopDataCollectingWaitingCircle";
            this.StartStopDataCollectingWaitingCircle.NumberSpoke = 24;
            this.StartStopDataCollectingWaitingCircle.OuterCircleRadius = 9;
            this.StartStopDataCollectingWaitingCircle.RotationSpeed = 100;
            this.StartStopDataCollectingWaitingCircle.Size = new System.Drawing.Size(47, 44);
            this.StartStopDataCollectingWaitingCircle.SpokeThickness = 4;
            this.StartStopDataCollectingWaitingCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.StartStopDataCollectingWaitingCircle.TabIndex = 76;
            this.StartStopDataCollectingWaitingCircle.Text = "loadingCircle1";
            this.StartStopDataCollectingWaitingCircle.Visible = false;
            // 
            // btnCollectMostClose
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnCollectMostClose, 3);
            this.btnCollectMostClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCollectMostClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCollectMostClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCollectMostClose.Location = new System.Drawing.Point(3, 93);
            this.btnCollectMostClose.Name = "btnCollectMostClose";
            this.btnCollectMostClose.Size = new System.Drawing.Size(144, 26);
            this.btnCollectMostClose.TabIndex = 59;
            this.btnCollectMostClose.Text = "Get most close";
            this.btnCollectMostClose.UseVisualStyleBackColor = true;
            this.btnCollectMostClose.Click += new System.EventHandler(this.btnCollectMostClose_Click);
            // 
            // btnCollectImmediately
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnCollectImmediately, 3);
            this.btnCollectImmediately.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCollectImmediately.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCollectImmediately.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCollectImmediately.Location = new System.Drawing.Point(3, 125);
            this.btnCollectImmediately.Name = "btnCollectImmediately";
            this.btnCollectImmediately.Size = new System.Drawing.Size(144, 26);
            this.btnCollectImmediately.TabIndex = 48;
            this.btnCollectImmediately.Text = "Get NOW";
            this.btnCollectImmediately.UseVisualStyleBackColor = true;
            this.btnCollectImmediately.Click += new System.EventHandler(this.btnCollectImmediately_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 3);
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(153, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 32);
            this.label3.TabIndex = 67;
            this.label3.Text = "next shot in:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 3);
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(153, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 32);
            this.label5.TabIndex = 69;
            this.label5.Text = "since last shot:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNextShotIn
            // 
            this.lblNextShotIn.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblNextShotIn, 3);
            this.lblNextShotIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNextShotIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNextShotIn.Location = new System.Drawing.Point(303, 90);
            this.lblNextShotIn.Name = "lblNextShotIn";
            this.lblNextShotIn.Size = new System.Drawing.Size(144, 32);
            this.lblNextShotIn.TabIndex = 66;
            this.lblNextShotIn.Text = "00:00:00";
            this.lblNextShotIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSinceLastShot
            // 
            this.lblSinceLastShot.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSinceLastShot, 3);
            this.lblSinceLastShot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSinceLastShot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSinceLastShot.Location = new System.Drawing.Point(303, 122);
            this.lblSinceLastShot.Name = "lblSinceLastShot";
            this.lblSinceLastShot.Size = new System.Drawing.Size(144, 32);
            this.lblSinceLastShot.TabIndex = 68;
            this.lblSinceLastShot.Text = "00:00:00";
            this.lblSinceLastShot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelerometerSign
            // 
            this.lblAccelerometerSign.AutoSize = true;
            this.lblAccelerometerSign.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccelerometerSign, 3);
            this.lblAccelerometerSign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelerometerSign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAccelerometerSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccelerometerSign.Location = new System.Drawing.Point(503, 90);
            this.lblAccelerometerSign.Name = "lblAccelerometerSign";
            this.tableLayoutPanel1.SetRowSpan(this.lblAccelerometerSign, 2);
            this.lblAccelerometerSign.Size = new System.Drawing.Size(144, 64);
            this.lblAccelerometerSign.TabIndex = 49;
            this.lblAccelerometerSign.Text = "Accelerometer data mean";
            this.lblAccelerometerSign.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAccelerometerSign.Click += new System.EventHandler(this.lblAccelerometerSign_Click);
            // 
            // lblAccDevMeanMagnTitle
            // 
            this.lblAccDevMeanMagnTitle.AutoSize = true;
            this.lblAccDevMeanMagnTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanMagnTitle, 2);
            this.lblAccDevMeanMagnTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanMagnTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanMagnTitle.Location = new System.Drawing.Point(653, 90);
            this.lblAccDevMeanMagnTitle.Name = "lblAccDevMeanMagnTitle";
            this.lblAccDevMeanMagnTitle.Size = new System.Drawing.Size(94, 32);
            this.lblAccDevMeanMagnTitle.TabIndex = 50;
            this.lblAccDevMeanMagnTitle.Text = "magnitude";
            this.lblAccDevMeanMagnTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanAngleTitle
            // 
            this.lblAccDevMeanAngleTitle.AutoSize = true;
            this.lblAccDevMeanAngleTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanAngleTitle, 2);
            this.lblAccDevMeanAngleTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanAngleTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanAngleTitle.Location = new System.Drawing.Point(753, 90);
            this.lblAccDevMeanAngleTitle.Name = "lblAccDevMeanAngleTitle";
            this.lblAccDevMeanAngleTitle.Size = new System.Drawing.Size(94, 32);
            this.lblAccDevMeanAngleTitle.TabIndex = 51;
            this.lblAccDevMeanAngleTitle.Text = "dev.angle";
            this.lblAccDevMeanAngleTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanMagnValue
            // 
            this.lblAccDevMeanMagnValue.AutoSize = true;
            this.lblAccDevMeanMagnValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanMagnValue, 2);
            this.lblAccDevMeanMagnValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanMagnValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanMagnValue.Location = new System.Drawing.Point(653, 122);
            this.lblAccDevMeanMagnValue.Name = "lblAccDevMeanMagnValue";
            this.lblAccDevMeanMagnValue.Size = new System.Drawing.Size(94, 32);
            this.lblAccDevMeanMagnValue.TabIndex = 53;
            this.lblAccDevMeanMagnValue.Text = "---";
            this.lblAccDevMeanMagnValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanAngleValue
            // 
            this.lblAccDevMeanAngleValue.AutoSize = true;
            this.lblAccDevMeanAngleValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanAngleValue, 2);
            this.lblAccDevMeanAngleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanAngleValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanAngleValue.Location = new System.Drawing.Point(753, 122);
            this.lblAccDevMeanAngleValue.Name = "lblAccDevMeanAngleValue";
            this.lblAccDevMeanAngleValue.Size = new System.Drawing.Size(94, 32);
            this.lblAccDevMeanAngleValue.TabIndex = 54;
            this.lblAccDevMeanAngleValue.Text = "---";
            this.lblAccDevMeanAngleValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompass
            // 
            this.lblCompass.AutoSize = true;
            this.lblCompass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCompass, 3);
            this.lblCompass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCompass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCompass.Location = new System.Drawing.Point(503, 164);
            this.lblCompass.Name = "lblCompass";
            this.tableLayoutPanel1.SetRowSpan(this.lblCompass, 4);
            this.lblCompass.Size = new System.Drawing.Size(144, 124);
            this.lblCompass.TabIndex = 77;
            this.lblCompass.Text = "Compass";
            this.lblCompass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label14, 2);
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(653, 164);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(94, 30);
            this.label14.TabIndex = 78;
            this.label14.Text = "X";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnDataX
            // 
            this.lblMagnDataX.AutoSize = true;
            this.lblMagnDataX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMagnDataX, 2);
            this.lblMagnDataX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnDataX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMagnDataX.Location = new System.Drawing.Point(653, 194);
            this.lblMagnDataX.Name = "lblMagnDataX";
            this.lblMagnDataX.Size = new System.Drawing.Size(94, 32);
            this.lblMagnDataX.TabIndex = 81;
            this.lblMagnDataX.Text = "---";
            this.lblMagnDataX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label15, 2);
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(753, 164);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(94, 30);
            this.label15.TabIndex = 79;
            this.label15.Text = "Y";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label16, 2);
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(853, 164);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(97, 30);
            this.label16.TabIndex = 80;
            this.label16.Text = "Z";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnDataY
            // 
            this.lblMagnDataY.AutoSize = true;
            this.lblMagnDataY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMagnDataY, 2);
            this.lblMagnDataY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnDataY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMagnDataY.Location = new System.Drawing.Point(753, 194);
            this.lblMagnDataY.Name = "lblMagnDataY";
            this.lblMagnDataY.Size = new System.Drawing.Size(94, 32);
            this.lblMagnDataY.TabIndex = 82;
            this.lblMagnDataY.Text = "---";
            this.lblMagnDataY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnDataZ
            // 
            this.lblMagnDataZ.AutoSize = true;
            this.lblMagnDataZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMagnDataZ, 2);
            this.lblMagnDataZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnDataZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMagnDataZ.Location = new System.Drawing.Point(853, 194);
            this.lblMagnDataZ.Name = "lblMagnDataZ";
            this.lblMagnDataZ.Size = new System.Drawing.Size(97, 32);
            this.lblMagnDataZ.TabIndex = 83;
            this.lblMagnDataZ.Text = "---";
            this.lblMagnDataZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label20, 6);
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label20.Location = new System.Drawing.Point(653, 226);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(297, 30);
            this.label20.TabIndex = 84;
            this.label20.Text = "head";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnDataHeading
            // 
            this.lblMagnDataHeading.AutoSize = true;
            this.lblMagnDataHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMagnDataHeading, 6);
            this.lblMagnDataHeading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnDataHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMagnDataHeading.Location = new System.Drawing.Point(653, 256);
            this.lblMagnDataHeading.Name = "lblMagnDataHeading";
            this.lblMagnDataHeading.Size = new System.Drawing.Size(297, 32);
            this.lblMagnDataHeading.TabIndex = 85;
            this.lblMagnDataHeading.Text = "---";
            this.lblMagnDataHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbThumbPreview
            // 
            this.pbThumbPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pbThumbPreview, 9);
            this.pbThumbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbThumbPreview.Location = new System.Drawing.Point(3, 167);
            this.pbThumbPreview.Name = "pbThumbPreview";
            this.tableLayoutPanel1.SetRowSpan(this.pbThumbPreview, 13);
            this.pbThumbPreview.Size = new System.Drawing.Size(444, 362);
            this.pbThumbPreview.TabIndex = 57;
            this.pbThumbPreview.TabStop = false;
            this.pbThumbPreview.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblLonTitle
            // 
            this.lblLonTitle.AutoSize = true;
            this.lblLonTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblLonTitle, 3);
            this.lblLonTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLonTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLonTitle.Location = new System.Drawing.Point(803, 298);
            this.lblLonTitle.Name = "lblLonTitle";
            this.lblLonTitle.Size = new System.Drawing.Size(147, 30);
            this.lblLonTitle.TabIndex = 88;
            this.lblLonTitle.Text = "lon";
            this.lblLonTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLonValue
            // 
            this.lblLonValue.AutoSize = true;
            this.lblLonValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblLonValue, 3);
            this.lblLonValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLonValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLonValue.Location = new System.Drawing.Point(803, 328);
            this.lblLonValue.Name = "lblLonValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblLonValue, 2);
            this.lblLonValue.Size = new System.Drawing.Size(147, 64);
            this.lblLonValue.TabIndex = 91;
            this.lblLonValue.Text = "---";
            this.lblLonValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLatTitle
            // 
            this.lblLatTitle.AutoSize = true;
            this.lblLatTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblLatTitle, 3);
            this.lblLatTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLatTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLatTitle.Location = new System.Drawing.Point(653, 298);
            this.lblLatTitle.Name = "lblLatTitle";
            this.lblLatTitle.Size = new System.Drawing.Size(144, 30);
            this.lblLatTitle.TabIndex = 87;
            this.lblLatTitle.Text = "lat";
            this.lblLatTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLatValue
            // 
            this.lblLatValue.AutoSize = true;
            this.lblLatValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblLatValue, 3);
            this.lblLatValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLatValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLatValue.Location = new System.Drawing.Point(653, 328);
            this.lblLatValue.Name = "lblLatValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblLatValue, 2);
            this.lblLatValue.Size = new System.Drawing.Size(144, 64);
            this.lblLatValue.TabIndex = 90;
            this.lblLatValue.Text = "---";
            this.lblLatValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGPStitle
            // 
            this.lblGPStitle.AutoSize = true;
            this.lblGPStitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblGPStitle, 3);
            this.lblGPStitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGPStitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGPStitle.Location = new System.Drawing.Point(503, 298);
            this.lblGPStitle.Name = "lblGPStitle";
            this.tableLayoutPanel1.SetRowSpan(this.lblGPStitle, 6);
            this.lblGPStitle.Size = new System.Drawing.Size(144, 188);
            this.lblGPStitle.TabIndex = 86;
            this.lblGPStitle.Text = "GPS data";
            this.lblGPStitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label17, 6);
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(653, 392);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(297, 30);
            this.label17.TabIndex = 89;
            this.label17.Text = "UTC time";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUTCTimeValue
            // 
            this.lblUTCTimeValue.AutoSize = true;
            this.lblUTCTimeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblUTCTimeValue, 6);
            this.lblUTCTimeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUTCTimeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUTCTimeValue.Location = new System.Drawing.Point(653, 422);
            this.lblUTCTimeValue.Name = "lblUTCTimeValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblUTCTimeValue, 2);
            this.lblUTCTimeValue.Size = new System.Drawing.Size(297, 64);
            this.lblUTCTimeValue.TabIndex = 92;
            this.lblUTCTimeValue.Text = "---";
            this.lblUTCTimeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPressureTitle
            // 
            this.lblPressureTitle.AutoSize = true;
            this.lblPressureTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblPressureTitle, 3);
            this.lblPressureTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPressureTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPressureTitle.Location = new System.Drawing.Point(503, 496);
            this.lblPressureTitle.Name = "lblPressureTitle";
            this.lblPressureTitle.Size = new System.Drawing.Size(144, 36);
            this.lblPressureTitle.TabIndex = 93;
            this.lblPressureTitle.Text = "Pressure";
            this.lblPressureTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPressureValue
            // 
            this.lblPressureValue.AutoSize = true;
            this.lblPressureValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblPressureValue, 6);
            this.lblPressureValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPressureValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPressureValue.Location = new System.Drawing.Point(653, 496);
            this.lblPressureValue.Name = "lblPressureValue";
            this.lblPressureValue.Size = new System.Drawing.Size(297, 36);
            this.lblPressureValue.TabIndex = 94;
            this.lblPressureValue.Text = "---";
            this.lblPressureValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // processCircle
            // 
            this.processCircle.Active = false;
            this.processCircle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel1.SetColumnSpan(this.processCircle, 2);
            this.processCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processCircle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.processCircle.InnerCircleRadius = 5;
            this.processCircle.Location = new System.Drawing.Point(853, 93);
            this.processCircle.Name = "processCircle";
            this.processCircle.NumberSpoke = 12;
            this.processCircle.OuterCircleRadius = 11;
            this.processCircle.RotationSpeed = 100;
            this.tableLayoutPanel1.SetRowSpan(this.processCircle, 2);
            this.processCircle.Size = new System.Drawing.Size(97, 58);
            this.processCircle.SpokeThickness = 2;
            this.processCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.processCircle.TabIndex = 95;
            this.processCircle.Visible = false;
            // 
            // tabPageBcstLog
            // 
            this.tabPageBcstLog.Controls.Add(this.tableLayoutPanel2);
            this.tabPageBcstLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPageBcstLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageBcstLog.Name = "tabPageBcstLog";
            this.tabPageBcstLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBcstLog.Size = new System.Drawing.Size(959, 538);
            this.tabPageBcstLog.TabIndex = 0;
            this.tabPageBcstLog.Text = "Logs";
            this.tabPageBcstLog.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.tbMainLog, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnSwapBcstLog, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnSwitchShowingTotalLog, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(953, 532);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tbMainLog
            // 
            this.tbMainLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel2.SetColumnSpan(this.tbMainLog, 6);
            this.tbMainLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMainLog.Location = new System.Drawing.Point(3, 73);
            this.tbMainLog.Multiline = true;
            this.tbMainLog.Name = "tbMainLog";
            this.tbMainLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMainLog.Size = new System.Drawing.Size(947, 456);
            this.tbMainLog.TabIndex = 10;
            // 
            // btnSwapBcstLog
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.btnSwapBcstLog, 3);
            this.btnSwapBcstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSwapBcstLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapBcstLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSwapBcstLog.Location = new System.Drawing.Point(477, 3);
            this.btnSwapBcstLog.Name = "btnSwapBcstLog";
            this.btnSwapBcstLog.Size = new System.Drawing.Size(473, 34);
            this.btnSwapBcstLog.TabIndex = 7;
            this.btnSwapBcstLog.Text = "Swap log";
            this.btnSwapBcstLog.UseVisualStyleBackColor = true;
            this.btnSwapBcstLog.Click += new System.EventHandler(this.btnSwapBcstLog_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label11, 6);
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(3, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(947, 30);
            this.label11.TabIndex = 11;
            this.label11.Text = "messages";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSwitchShowingTotalLog
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.btnSwitchShowingTotalLog, 3);
            this.btnSwitchShowingTotalLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSwitchShowingTotalLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchShowingTotalLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSwitchShowingTotalLog.ForeColor = System.Drawing.Color.Red;
            this.btnSwitchShowingTotalLog.Location = new System.Drawing.Point(3, 3);
            this.btnSwitchShowingTotalLog.Name = "btnSwitchShowingTotalLog";
            this.btnSwitchShowingTotalLog.Size = new System.Drawing.Size(468, 34);
            this.btnSwitchShowingTotalLog.TabIndex = 12;
            this.btnSwitchShowingTotalLog.Text = "Show total log";
            this.btnSwitchShowingTotalLog.UseVisualStyleBackColor = true;
            this.btnSwitchShowingTotalLog.Click += new System.EventHandler(this.btnSwitchShowingTotalLog_Click);
            // 
            // SensorsCalibration
            // 
            this.SensorsCalibration.Controls.Add(this.tableLayoutPanel3);
            this.SensorsCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SensorsCalibration.Location = new System.Drawing.Point(4, 22);
            this.SensorsCalibration.Name = "SensorsCalibration";
            this.SensorsCalibration.Padding = new System.Windows.Forms.Padding(3);
            this.SensorsCalibration.Size = new System.Drawing.Size(959, 538);
            this.SensorsCalibration.TabIndex = 2;
            this.SensorsCalibration.Text = "Sensors calibration";
            this.SensorsCalibration.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanel3.Controls.Add(this.lblCaughtMagnCalibrationValue, 5, 6);
            this.tableLayoutPanel3.Controls.Add(this.label13, 5, 5);
            this.tableLayoutPanel3.Controls.Add(this.btnCalibrateMagnetometer, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.label7, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblAccelCalibrationCurrentX, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblAccelCalibrationCurrentY, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblAccelCalibrationCurrentZ, 4, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnCalibrateAccelerometer, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblAccelCalibrationX, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblAccelCalibrationY, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblAccelCalibrationZ, 4, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblCalculationStatistics, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblStDevX, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblStDevY, 3, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblStDevZ, 4, 3);
            this.tableLayoutPanel3.Controls.Add(this.btnSaveAccel, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 2, 5);
            this.tableLayoutPanel3.Controls.Add(this.label8, 3, 5);
            this.tableLayoutPanel3.Controls.Add(this.label9, 4, 5);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnCalibrationCurrentX, 2, 6);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnCalibrationCurrentY, 3, 6);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnCalibrationCurrentZ, 4, 6);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnCalibrationX, 2, 7);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnCalibrationY, 3, 7);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnCalibrationZ, 4, 7);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnStDevX, 2, 8);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnStDevY, 3, 8);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnStDevZ, 4, 8);
            this.tableLayoutPanel3.Controls.Add(this.lblMagnCalculationStatistics, 2, 9);
            this.tableLayoutPanel3.Controls.Add(this.btnMagnSaveCalibration, 7, 5);
            this.tableLayoutPanel3.Controls.Add(this.label12, 5, 7);
            this.tableLayoutPanel3.Controls.Add(this.tbCurrentCompassHeadingValue, 5, 8);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 11;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(953, 532);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // lblCaughtMagnCalibrationValue
            // 
            this.lblCaughtMagnCalibrationValue.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.lblCaughtMagnCalibrationValue, 2);
            this.lblCaughtMagnCalibrationValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCaughtMagnCalibrationValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCaughtMagnCalibrationValue.Location = new System.Drawing.Point(478, 210);
            this.lblCaughtMagnCalibrationValue.Name = "lblCaughtMagnCalibrationValue";
            this.lblCaughtMagnCalibrationValue.Size = new System.Drawing.Size(184, 35);
            this.lblCaughtMagnCalibrationValue.TabIndex = 89;
            this.lblCaughtMagnCalibrationValue.Text = "---";
            this.lblCaughtMagnCalibrationValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label13, 2);
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(478, 175);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(184, 35);
            this.label13.TabIndex = 88;
            this.label13.Text = "caught value";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCalibrateMagnetometer
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnCalibrateMagnetometer, 2);
            this.btnCalibrateMagnetometer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalibrateMagnetometer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibrateMagnetometer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCalibrateMagnetometer.Location = new System.Drawing.Point(3, 178);
            this.btnCalibrateMagnetometer.Name = "btnCalibrateMagnetometer";
            this.tableLayoutPanel3.SetRowSpan(this.btnCalibrateMagnetometer, 5);
            this.btnCalibrateMagnetometer.Size = new System.Drawing.Size(184, 169);
            this.btnCalibrateMagnetometer.TabIndex = 72;
            this.btnCalibrateMagnetometer.Text = "Calibrate magnetometer";
            this.btnCalibrateMagnetometer.UseVisualStyleBackColor = true;
            this.btnCalibrateMagnetometer.Click += new System.EventHandler(this.btnCalibrateMagnetometer_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(193, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 35);
            this.label4.TabIndex = 57;
            this.label4.Text = "X";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(288, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 35);
            this.label6.TabIndex = 58;
            this.label6.Text = "Y";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(383, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 35);
            this.label7.TabIndex = 59;
            this.label7.Text = "Z";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationCurrentX
            // 
            this.lblAccelCalibrationCurrentX.AutoSize = true;
            this.lblAccelCalibrationCurrentX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationCurrentX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationCurrentX.Location = new System.Drawing.Point(193, 35);
            this.lblAccelCalibrationCurrentX.Name = "lblAccelCalibrationCurrentX";
            this.lblAccelCalibrationCurrentX.Size = new System.Drawing.Size(89, 35);
            this.lblAccelCalibrationCurrentX.TabIndex = 60;
            this.lblAccelCalibrationCurrentX.Text = "0";
            this.lblAccelCalibrationCurrentX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationCurrentY
            // 
            this.lblAccelCalibrationCurrentY.AutoSize = true;
            this.lblAccelCalibrationCurrentY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationCurrentY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationCurrentY.Location = new System.Drawing.Point(288, 35);
            this.lblAccelCalibrationCurrentY.Name = "lblAccelCalibrationCurrentY";
            this.lblAccelCalibrationCurrentY.Size = new System.Drawing.Size(89, 35);
            this.lblAccelCalibrationCurrentY.TabIndex = 61;
            this.lblAccelCalibrationCurrentY.Text = "0";
            this.lblAccelCalibrationCurrentY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationCurrentZ
            // 
            this.lblAccelCalibrationCurrentZ.AutoSize = true;
            this.lblAccelCalibrationCurrentZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationCurrentZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationCurrentZ.Location = new System.Drawing.Point(383, 35);
            this.lblAccelCalibrationCurrentZ.Name = "lblAccelCalibrationCurrentZ";
            this.lblAccelCalibrationCurrentZ.Size = new System.Drawing.Size(89, 35);
            this.lblAccelCalibrationCurrentZ.TabIndex = 62;
            this.lblAccelCalibrationCurrentZ.Text = "0";
            this.lblAccelCalibrationCurrentZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCalibrateAccelerometer
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnCalibrateAccelerometer, 2);
            this.btnCalibrateAccelerometer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalibrateAccelerometer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibrateAccelerometer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCalibrateAccelerometer.Location = new System.Drawing.Point(3, 3);
            this.btnCalibrateAccelerometer.Name = "btnCalibrateAccelerometer";
            this.tableLayoutPanel3.SetRowSpan(this.btnCalibrateAccelerometer, 5);
            this.btnCalibrateAccelerometer.Size = new System.Drawing.Size(184, 169);
            this.btnCalibrateAccelerometer.TabIndex = 63;
            this.btnCalibrateAccelerometer.Text = "Calibrate accelerometer";
            this.btnCalibrateAccelerometer.UseVisualStyleBackColor = true;
            this.btnCalibrateAccelerometer.Click += new System.EventHandler(this.btnCalibrateAccelerometer_Click);
            // 
            // lblAccelCalibrationX
            // 
            this.lblAccelCalibrationX.AutoSize = true;
            this.lblAccelCalibrationX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationX.Location = new System.Drawing.Point(193, 70);
            this.lblAccelCalibrationX.Name = "lblAccelCalibrationX";
            this.lblAccelCalibrationX.Size = new System.Drawing.Size(89, 35);
            this.lblAccelCalibrationX.TabIndex = 64;
            this.lblAccelCalibrationX.Text = "<0>";
            this.lblAccelCalibrationX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationY
            // 
            this.lblAccelCalibrationY.AutoSize = true;
            this.lblAccelCalibrationY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationY.Location = new System.Drawing.Point(288, 70);
            this.lblAccelCalibrationY.Name = "lblAccelCalibrationY";
            this.lblAccelCalibrationY.Size = new System.Drawing.Size(89, 35);
            this.lblAccelCalibrationY.TabIndex = 65;
            this.lblAccelCalibrationY.Text = "<0>";
            this.lblAccelCalibrationY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationZ
            // 
            this.lblAccelCalibrationZ.AutoSize = true;
            this.lblAccelCalibrationZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationZ.Location = new System.Drawing.Point(383, 70);
            this.lblAccelCalibrationZ.Name = "lblAccelCalibrationZ";
            this.lblAccelCalibrationZ.Size = new System.Drawing.Size(89, 35);
            this.lblAccelCalibrationZ.TabIndex = 66;
            this.lblAccelCalibrationZ.Text = "<0>";
            this.lblAccelCalibrationZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCalculationStatistics
            // 
            this.lblCalculationStatistics.AutoSize = true;
            this.lblCalculationStatistics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel3.SetColumnSpan(this.lblCalculationStatistics, 3);
            this.lblCalculationStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCalculationStatistics.Location = new System.Drawing.Point(193, 140);
            this.lblCalculationStatistics.Name = "lblCalculationStatistics";
            this.lblCalculationStatistics.Size = new System.Drawing.Size(279, 35);
            this.lblCalculationStatistics.TabIndex = 67;
            this.lblCalculationStatistics.Text = "---";
            // 
            // lblStDevX
            // 
            this.lblStDevX.AutoSize = true;
            this.lblStDevX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStDevX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStDevX.Location = new System.Drawing.Point(193, 105);
            this.lblStDevX.Name = "lblStDevX";
            this.lblStDevX.Size = new System.Drawing.Size(89, 35);
            this.lblStDevX.TabIndex = 68;
            this.lblStDevX.Text = "0%";
            this.lblStDevX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStDevY
            // 
            this.lblStDevY.AutoSize = true;
            this.lblStDevY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStDevY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStDevY.Location = new System.Drawing.Point(288, 105);
            this.lblStDevY.Name = "lblStDevY";
            this.lblStDevY.Size = new System.Drawing.Size(89, 35);
            this.lblStDevY.TabIndex = 69;
            this.lblStDevY.Text = "0%";
            this.lblStDevY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStDevZ
            // 
            this.lblStDevZ.AutoSize = true;
            this.lblStDevZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStDevZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStDevZ.Location = new System.Drawing.Point(383, 105);
            this.lblStDevZ.Name = "lblStDevZ";
            this.lblStDevZ.Size = new System.Drawing.Size(89, 35);
            this.lblStDevZ.TabIndex = 70;
            this.lblStDevZ.Text = "0%";
            this.lblStDevZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSaveAccel
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnSaveAccel, 3);
            this.btnSaveAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveAccel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAccel.Location = new System.Drawing.Point(478, 3);
            this.btnSaveAccel.Name = "btnSaveAccel";
            this.tableLayoutPanel3.SetRowSpan(this.btnSaveAccel, 5);
            this.btnSaveAccel.Size = new System.Drawing.Size(279, 169);
            this.btnSaveAccel.TabIndex = 71;
            this.btnSaveAccel.Text = "Save calibration data";
            this.btnSaveAccel.UseVisualStyleBackColor = true;
            this.btnSaveAccel.Click += new System.EventHandler(this.btnSaveAccel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(193, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 35);
            this.label1.TabIndex = 73;
            this.label1.Text = "mX";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(288, 175);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 35);
            this.label8.TabIndex = 74;
            this.label8.Text = "mY";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(383, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 35);
            this.label9.TabIndex = 75;
            this.label9.Text = "mZ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnCalibrationCurrentX
            // 
            this.lblMagnCalibrationCurrentX.AutoSize = true;
            this.lblMagnCalibrationCurrentX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnCalibrationCurrentX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnCalibrationCurrentX.Location = new System.Drawing.Point(193, 210);
            this.lblMagnCalibrationCurrentX.Name = "lblMagnCalibrationCurrentX";
            this.lblMagnCalibrationCurrentX.Size = new System.Drawing.Size(89, 35);
            this.lblMagnCalibrationCurrentX.TabIndex = 76;
            this.lblMagnCalibrationCurrentX.Text = "0";
            this.lblMagnCalibrationCurrentX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnCalibrationCurrentY
            // 
            this.lblMagnCalibrationCurrentY.AutoSize = true;
            this.lblMagnCalibrationCurrentY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnCalibrationCurrentY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnCalibrationCurrentY.Location = new System.Drawing.Point(288, 210);
            this.lblMagnCalibrationCurrentY.Name = "lblMagnCalibrationCurrentY";
            this.lblMagnCalibrationCurrentY.Size = new System.Drawing.Size(89, 35);
            this.lblMagnCalibrationCurrentY.TabIndex = 77;
            this.lblMagnCalibrationCurrentY.Text = "0";
            this.lblMagnCalibrationCurrentY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnCalibrationCurrentZ
            // 
            this.lblMagnCalibrationCurrentZ.AutoSize = true;
            this.lblMagnCalibrationCurrentZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnCalibrationCurrentZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnCalibrationCurrentZ.Location = new System.Drawing.Point(383, 210);
            this.lblMagnCalibrationCurrentZ.Name = "lblMagnCalibrationCurrentZ";
            this.lblMagnCalibrationCurrentZ.Size = new System.Drawing.Size(89, 35);
            this.lblMagnCalibrationCurrentZ.TabIndex = 78;
            this.lblMagnCalibrationCurrentZ.Text = "0";
            this.lblMagnCalibrationCurrentZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnCalibrationX
            // 
            this.lblMagnCalibrationX.AutoSize = true;
            this.lblMagnCalibrationX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnCalibrationX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnCalibrationX.Location = new System.Drawing.Point(193, 245);
            this.lblMagnCalibrationX.Name = "lblMagnCalibrationX";
            this.lblMagnCalibrationX.Size = new System.Drawing.Size(89, 35);
            this.lblMagnCalibrationX.TabIndex = 79;
            this.lblMagnCalibrationX.Text = "<0>";
            this.lblMagnCalibrationX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnCalibrationY
            // 
            this.lblMagnCalibrationY.AutoSize = true;
            this.lblMagnCalibrationY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnCalibrationY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnCalibrationY.Location = new System.Drawing.Point(288, 245);
            this.lblMagnCalibrationY.Name = "lblMagnCalibrationY";
            this.lblMagnCalibrationY.Size = new System.Drawing.Size(89, 35);
            this.lblMagnCalibrationY.TabIndex = 80;
            this.lblMagnCalibrationY.Text = "<0>";
            this.lblMagnCalibrationY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnCalibrationZ
            // 
            this.lblMagnCalibrationZ.AutoSize = true;
            this.lblMagnCalibrationZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnCalibrationZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnCalibrationZ.Location = new System.Drawing.Point(383, 245);
            this.lblMagnCalibrationZ.Name = "lblMagnCalibrationZ";
            this.lblMagnCalibrationZ.Size = new System.Drawing.Size(89, 35);
            this.lblMagnCalibrationZ.TabIndex = 81;
            this.lblMagnCalibrationZ.Text = "<0>";
            this.lblMagnCalibrationZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnStDevX
            // 
            this.lblMagnStDevX.AutoSize = true;
            this.lblMagnStDevX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnStDevX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnStDevX.Location = new System.Drawing.Point(193, 280);
            this.lblMagnStDevX.Name = "lblMagnStDevX";
            this.lblMagnStDevX.Size = new System.Drawing.Size(89, 35);
            this.lblMagnStDevX.TabIndex = 82;
            this.lblMagnStDevX.Text = "0%";
            this.lblMagnStDevX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnStDevY
            // 
            this.lblMagnStDevY.AutoSize = true;
            this.lblMagnStDevY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnStDevY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnStDevY.Location = new System.Drawing.Point(288, 280);
            this.lblMagnStDevY.Name = "lblMagnStDevY";
            this.lblMagnStDevY.Size = new System.Drawing.Size(89, 35);
            this.lblMagnStDevY.TabIndex = 83;
            this.lblMagnStDevY.Text = "0%";
            this.lblMagnStDevY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnStDevZ
            // 
            this.lblMagnStDevZ.AutoSize = true;
            this.lblMagnStDevZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMagnStDevZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnStDevZ.Location = new System.Drawing.Point(383, 280);
            this.lblMagnStDevZ.Name = "lblMagnStDevZ";
            this.lblMagnStDevZ.Size = new System.Drawing.Size(89, 35);
            this.lblMagnStDevZ.TabIndex = 84;
            this.lblMagnStDevZ.Text = "0%";
            this.lblMagnStDevZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMagnCalculationStatistics
            // 
            this.lblMagnCalculationStatistics.AutoSize = true;
            this.lblMagnCalculationStatistics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel3.SetColumnSpan(this.lblMagnCalculationStatistics, 3);
            this.lblMagnCalculationStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMagnCalculationStatistics.Location = new System.Drawing.Point(193, 315);
            this.lblMagnCalculationStatistics.Name = "lblMagnCalculationStatistics";
            this.lblMagnCalculationStatistics.Size = new System.Drawing.Size(279, 35);
            this.lblMagnCalculationStatistics.TabIndex = 85;
            this.lblMagnCalculationStatistics.Text = "---";
            // 
            // btnMagnSaveCalibration
            // 
            this.btnMagnSaveCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMagnSaveCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMagnSaveCalibration.Location = new System.Drawing.Point(668, 178);
            this.btnMagnSaveCalibration.Name = "btnMagnSaveCalibration";
            this.tableLayoutPanel3.SetRowSpan(this.btnMagnSaveCalibration, 5);
            this.btnMagnSaveCalibration.Size = new System.Drawing.Size(89, 169);
            this.btnMagnSaveCalibration.TabIndex = 86;
            this.btnMagnSaveCalibration.Text = "Save calibration data";
            this.btnMagnSaveCalibration.UseVisualStyleBackColor = true;
            this.btnMagnSaveCalibration.Click += new System.EventHandler(this.btnMagnSaveCalibration_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label12, 2);
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(478, 245);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(184, 35);
            this.label12.TabIndex = 87;
            this.label12.Text = "current value (grad)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCurrentCompassHeadingValue
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.tbCurrentCompassHeadingValue, 2);
            this.tbCurrentCompassHeadingValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCurrentCompassHeadingValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCurrentCompassHeadingValue.Location = new System.Drawing.Point(478, 283);
            this.tbCurrentCompassHeadingValue.Name = "tbCurrentCompassHeadingValue";
            this.tbCurrentCompassHeadingValue.Size = new System.Drawing.Size(184, 26);
            this.tbCurrentCompassHeadingValue.TabIndex = 90;
            this.tbCurrentCompassHeadingValue.Text = "0";
            // 
            // Preferencies
            // 
            this.Preferencies.BackColor = System.Drawing.Color.Transparent;
            this.Preferencies.Controls.Add(this.tableLayoutPanel4);
            this.Preferencies.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Preferencies.Location = new System.Drawing.Point(4, 22);
            this.Preferencies.Name = "Preferencies";
            this.Preferencies.Padding = new System.Windows.Forms.Padding(3);
            this.Preferencies.Size = new System.Drawing.Size(959, 538);
            this.Preferencies.TabIndex = 3;
            this.Preferencies.Text = "Preferencies";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel4.Controls.Add(this.camshotPeriodLabel, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tbCamShotPeriod, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.camshotPeriodDataSavingCircle, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.camIPLabel, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.tbCamIP, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.ipAddrValidatingCircle, 4, 1);
            this.tableLayoutPanel4.Controls.Add(this.camUNameLabel, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tbCamUName, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.camPWDLabel, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.tbCamPWD, 1, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 10;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(953, 532);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // camshotPeriodLabel
            // 
            this.camshotPeriodLabel.AutoSize = true;
            this.camshotPeriodLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camshotPeriodLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camshotPeriodLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camshotPeriodLabel.Location = new System.Drawing.Point(3, 0);
            this.camshotPeriodLabel.Name = "camshotPeriodLabel";
            this.camshotPeriodLabel.Size = new System.Drawing.Size(221, 40);
            this.camshotPeriodLabel.TabIndex = 72;
            this.camshotPeriodLabel.Text = "camera shooting period:";
            this.camshotPeriodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamShotPeriod
            // 
            this.tbCamShotPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamShotPeriod.Location = new System.Drawing.Point(230, 3);
            this.tbCamShotPeriod.Mask = "00:00:00";
            this.tbCamShotPeriod.Name = "tbCamShotPeriod";
            this.tbCamShotPeriod.Size = new System.Drawing.Size(221, 20);
            this.tbCamShotPeriod.TabIndex = 73;
            this.tbCamShotPeriod.Text = "000000";
            this.tbCamShotPeriod.TextChanged += new System.EventHandler(this.tbCamShotPeriod_TextChanged);
            // 
            // camshotPeriodDataSavingCircle
            // 
            this.camshotPeriodDataSavingCircle.Active = false;
            this.camshotPeriodDataSavingCircle.Color = System.Drawing.Color.DarkGray;
            this.camshotPeriodDataSavingCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camshotPeriodDataSavingCircle.InnerCircleRadius = 8;
            this.camshotPeriodDataSavingCircle.Location = new System.Drawing.Point(911, 3);
            this.camshotPeriodDataSavingCircle.Name = "camshotPeriodDataSavingCircle";
            this.camshotPeriodDataSavingCircle.NumberSpoke = 24;
            this.camshotPeriodDataSavingCircle.OuterCircleRadius = 9;
            this.camshotPeriodDataSavingCircle.RotationSpeed = 100;
            this.camshotPeriodDataSavingCircle.Size = new System.Drawing.Size(39, 34);
            this.camshotPeriodDataSavingCircle.SpokeThickness = 4;
            this.camshotPeriodDataSavingCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.camshotPeriodDataSavingCircle.TabIndex = 78;
            this.camshotPeriodDataSavingCircle.Text = "loadingCircle1";
            this.camshotPeriodDataSavingCircle.Visible = false;
            // 
            // camIPLabel
            // 
            this.camIPLabel.AutoSize = true;
            this.camIPLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camIPLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camIPLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camIPLabel.Location = new System.Drawing.Point(3, 40);
            this.camIPLabel.Name = "camIPLabel";
            this.camIPLabel.Size = new System.Drawing.Size(221, 40);
            this.camIPLabel.TabIndex = 79;
            this.camIPLabel.Text = "camera IP";
            this.camIPLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamIP
            // 
            this.tbCamIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamIP.Location = new System.Drawing.Point(230, 43);
            this.tbCamIP.Mask = "000.000.000.000";
            this.tbCamIP.Name = "tbCamIP";
            this.tbCamIP.Size = new System.Drawing.Size(221, 20);
            this.tbCamIP.TabIndex = 80;
            this.tbCamIP.TextChanged += new System.EventHandler(this.tbCamIP_TextChanged);
            // 
            // ipAddrValidatingCircle
            // 
            this.ipAddrValidatingCircle.Active = false;
            this.ipAddrValidatingCircle.Color = System.Drawing.Color.DarkGray;
            this.ipAddrValidatingCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ipAddrValidatingCircle.InnerCircleRadius = 8;
            this.ipAddrValidatingCircle.Location = new System.Drawing.Point(911, 43);
            this.ipAddrValidatingCircle.Name = "ipAddrValidatingCircle";
            this.ipAddrValidatingCircle.NumberSpoke = 24;
            this.ipAddrValidatingCircle.OuterCircleRadius = 9;
            this.ipAddrValidatingCircle.RotationSpeed = 100;
            this.ipAddrValidatingCircle.Size = new System.Drawing.Size(39, 34);
            this.ipAddrValidatingCircle.SpokeThickness = 4;
            this.ipAddrValidatingCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.ipAddrValidatingCircle.TabIndex = 81;
            this.ipAddrValidatingCircle.Text = "loadingCircle1";
            this.ipAddrValidatingCircle.Visible = false;
            // 
            // camUNameLabel
            // 
            this.camUNameLabel.AutoSize = true;
            this.camUNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camUNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camUNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camUNameLabel.Location = new System.Drawing.Point(3, 80);
            this.camUNameLabel.Name = "camUNameLabel";
            this.camUNameLabel.Size = new System.Drawing.Size(221, 40);
            this.camUNameLabel.TabIndex = 82;
            this.camUNameLabel.Text = "camera username";
            this.camUNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamUName
            // 
            this.tbCamUName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamUName.Location = new System.Drawing.Point(230, 83);
            this.tbCamUName.Name = "tbCamUName";
            this.tbCamUName.Size = new System.Drawing.Size(221, 20);
            this.tbCamUName.TabIndex = 83;
            this.tbCamUName.TextChanged += new System.EventHandler(this.tbCamUName_TextChanged);
            // 
            // camPWDLabel
            // 
            this.camPWDLabel.AutoSize = true;
            this.camPWDLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camPWDLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camPWDLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camPWDLabel.Location = new System.Drawing.Point(3, 120);
            this.camPWDLabel.Name = "camPWDLabel";
            this.camPWDLabel.Size = new System.Drawing.Size(221, 40);
            this.camPWDLabel.TabIndex = 84;
            this.camPWDLabel.Text = "camera password";
            this.camPWDLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamPWD
            // 
            this.tbCamPWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamPWD.Location = new System.Drawing.Point(230, 123);
            this.tbCamPWD.Name = "tbCamPWD";
            this.tbCamPWD.Size = new System.Drawing.Size(221, 20);
            this.tbCamPWD.TabIndex = 85;
            this.tbCamPWD.TextChanged += new System.EventHandler(this.tbCamPWD_TextChanged);
            // 
            // arduinoBoardSearchingWorker
            // 
            this.arduinoBoardSearchingWorker.WorkerSupportsCancellation = true;
            this.arduinoBoardSearchingWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.arduinoBoardSearchingWorker_DoWork);
            this.arduinoBoardSearchingWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.arduinoBoardSearchingWorker_RunWorkerCompleted);
            // 
            // udpCatchingJob
            // 
            this.udpCatchingJob.WorkerSupportsCancellation = true;
            this.udpCatchingJob.DoWork += new System.ComponentModel.DoWorkEventHandler(this.udpCatchingJob_DoWork);
            this.udpCatchingJob.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.udpCatchingJob_RunWorkerCompleted);
            // 
            // ArduinoRequestExpectant
            // 
            this.ArduinoRequestExpectant.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ArduinoRequestExpectant_DoWork);
            this.ArduinoRequestExpectant.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ArduinoRequestExpectant_RunWorkerCompleted);
            // 
            // dataCollector
            // 
            this.dataCollector.WorkerSupportsCancellation = true;
            this.dataCollector.DoWork += new System.ComponentModel.DoWorkEventHandler(this.dataCollector_DoWork);
            this.dataCollector.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.dataCollector_RunWorkerCompleted);
            // 
            // accelCalibrator
            // 
            this.accelCalibrator.WorkerSupportsCancellation = true;
            this.accelCalibrator.DoWork += new System.ComponentModel.DoWorkEventHandler(this.accelCalibrator_DoWork);
            this.accelCalibrator.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.accelCalibrator_RunWorkerCompleted);
            // 
            // magnCalibrator
            // 
            this.magnCalibrator.WorkerSupportsCancellation = true;
            this.magnCalibrator.DoWork += new System.ComponentModel.DoWorkEventHandler(this.magnCalibrator_DoWork);
            this.magnCalibrator.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.magnCalibrator_RunWorkerCompleted);
            // 
            // bgwUDPmessagesParser
            // 
            this.bgwUDPmessagesParser.WorkerSupportsCancellation = true;
            this.bgwUDPmessagesParser.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwUDPmessagesParser_DoWork);
            // 
            // DataCollectorMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 564);
            this.Controls.Add(this.tabControl1);
            this.Name = "DataCollectorMainForm";
            this.Text = "Data collector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataCollectorMainForm_FormClosing);
            this.Load += new System.EventHandler(this.DataCollectorMainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbPreview)).EndInit();
            this.tabPageBcstLog.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.SensorsCalibration.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.Preferencies.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageBcstLog;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.ComponentModel.BackgroundWorker arduinoBoardSearchingWorker;
        private System.ComponentModel.BackgroundWorker udpCatchingJob;
        //private MRG.Controls.UI.LoadingCircle SearchingArduinoProcessCircle;
        private System.ComponentModel.BackgroundWorker ArduinoRequestExpectant;
        //private MRG.Controls.UI.LoadingCircle StartStopDataCollectingWaitingCircle;
        private System.ComponentModel.BackgroundWorker dataCollector;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSwapBcstLog;
        //private MRG.Controls.UI.LoadingCircle camshotPeriodDataSavingCircle;
        //private MRG.Controls.UI.LoadingCircle ipAddrValidatingCircle;
        private System.Windows.Forms.TabPage SensorsCalibration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblAccelCalibrationCurrentX;
        private System.Windows.Forms.Label lblAccelCalibrationCurrentY;
        private System.Windows.Forms.Label lblAccelCalibrationCurrentZ;
        private System.Windows.Forms.Button btnCalibrateAccelerometer;
        private System.Windows.Forms.Label lblAccelCalibrationX;
        private System.Windows.Forms.Label lblAccelCalibrationY;
        private System.Windows.Forms.Label lblAccelCalibrationZ;
        private System.ComponentModel.BackgroundWorker accelCalibrator;
        private System.Windows.Forms.Label lblCalculationStatistics;
        private System.Windows.Forms.Label lblStDevX;
        private System.Windows.Forms.Label lblStDevY;
        private System.Windows.Forms.Label lblStDevZ;
        private System.Windows.Forms.Button btnSaveAccel;
        private System.Windows.Forms.TabPage Preferencies;
        private System.Windows.Forms.Button btnCalibrateMagnetometer;
        private System.ComponentModel.BackgroundWorker magnCalibrator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblMagnCalibrationCurrentX;
        private System.Windows.Forms.Label lblMagnCalibrationCurrentY;
        private System.Windows.Forms.Label lblMagnCalibrationCurrentZ;
        private System.Windows.Forms.Label lblMagnCalibrationX;
        private System.Windows.Forms.Label lblMagnCalibrationY;
        private System.Windows.Forms.Label lblMagnCalibrationZ;
        private System.Windows.Forms.Label lblMagnStDevX;
        private System.Windows.Forms.Label lblMagnStDevY;
        private System.Windows.Forms.Label lblMagnStDevZ;
        private System.Windows.Forms.Label lblMagnCalculationStatistics;
        private System.Windows.Forms.Button btnMagnSaveCalibration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbBcstListeningPort;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnFindArduino;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStartStopBdcstListening;
        private System.Windows.Forms.Button btnStartStopCollecting;
        private System.Windows.Forms.Button btnCollectImmediately;
        private System.Windows.Forms.Label lblAccelerometerSign;
        private System.Windows.Forms.PictureBox pbThumbPreview;
        private System.Windows.Forms.Button btnCollectMostClose;
        private System.Windows.Forms.Label lblNextShotIn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSinceLastShot;
        private System.Windows.Forms.Label label5;
        private MRG.Controls.UI.LoadingCircle SearchingArduinoProcessCircle;
        private MRG.Controls.UI.LoadingCircle StartStopDataCollectingWaitingCircle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label camshotPeriodLabel;
        private System.Windows.Forms.MaskedTextBox tbCamShotPeriod;
        private MRG.Controls.UI.LoadingCircle camshotPeriodDataSavingCircle;
        private System.Windows.Forms.Label camIPLabel;
        private System.Windows.Forms.MaskedTextBox tbCamIP;
        private MRG.Controls.UI.LoadingCircle ipAddrValidatingCircle;
        private System.Windows.Forms.Label camUNameLabel;
        private System.Windows.Forms.TextBox tbCamUName;
        private System.Windows.Forms.Label camPWDLabel;
        private System.Windows.Forms.TextBox tbCamPWD;
        private System.Windows.Forms.TextBox tbMainLog;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblCaughtMagnCalibrationValue;
        private System.Windows.Forms.TextBox tbCurrentCompassHeadingValue;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblMagnDataX;
        private System.Windows.Forms.Label lblMagnDataY;
        private System.Windows.Forms.Label lblMagnDataZ;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblMagnDataHeading;
        private System.Windows.Forms.Label lblGPStitle;
        private System.Windows.Forms.Label lblLatTitle;
        private System.Windows.Forms.Label lblLonTitle;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblLatValue;
        private System.Windows.Forms.Label lblLonValue;
        private System.Windows.Forms.Label lblUTCTimeValue;
        private System.Windows.Forms.Label lblPressureTitle;
        private System.Windows.Forms.Label lblPressureValue;
        private System.Windows.Forms.Button btnSwitchShowingTotalLog;
        private System.Windows.Forms.Label lblAccDevMeanMagnTitle;
        private System.Windows.Forms.Label lblAccDevMeanAngleTitle;
        private System.Windows.Forms.Label lblAccDevMeanMagnValue;
        private System.Windows.Forms.Label lblAccDevMeanAngleValue;
        private System.Windows.Forms.Label lblCompass;
        private System.ComponentModel.BackgroundWorker bgwUDPmessagesParser;
        private MRG.Controls.UI.LoadingCircle processCircle;


    }
}

