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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbIP2ListenDevID1 = new System.Windows.Forms.TextBox();
            this.btnFindArduino1 = new System.Windows.Forms.Button();
            this.btnStartStopBdcstListening = new System.Windows.Forms.Button();
            this.btnStartStopCollecting = new System.Windows.Forms.Button();
            this.SearchingArduinoID1ProcessCircle = new MRG.Controls.UI.LoadingCircle();
            this.StartStopDataCollectingWaitingCircle = new MRG.Controls.UI.LoadingCircle();
            this.btnCollectMostClose = new System.Windows.Forms.Button();
            this.btnCollectImmediately = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblNextShotIn = new System.Windows.Forms.Label();
            this.lblSinceLastShot = new System.Windows.Forms.Label();
            this.lblAccelerometerSignID1 = new System.Windows.Forms.Label();
            this.lblAccDevMeanMagnTitleID1 = new System.Windows.Forms.Label();
            this.lblAccDevMeanAngleTitleID1 = new System.Windows.Forms.Label();
            this.lblAccDevMeanMagnValueID1 = new System.Windows.Forms.Label();
            this.lblAccDevMeanAngleValueID1 = new System.Windows.Forms.Label();
            this.pbThumbPreviewCam1 = new System.Windows.Forms.PictureBox();
            this.lblLonTitle = new System.Windows.Forms.Label();
            this.lblLonValue = new System.Windows.Forms.Label();
            this.lblLatTitle = new System.Windows.Forms.Label();
            this.lblLatValue = new System.Windows.Forms.Label();
            this.lblGPStitle = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblUTCTimeValue = new System.Windows.Forms.Label();
            this.lblPressureTitle = new System.Windows.Forms.Label();
            this.lblPressureValue = new System.Windows.Forms.Label();
            this.processCircleID1 = new MRG.Controls.UI.LoadingCircle();
            this.lblAccelerometerSignID2 = new System.Windows.Forms.Label();
            this.lblAccDevMeanMagnTitleID2 = new System.Windows.Forms.Label();
            this.lblAccDevMeanAngleTitleID2 = new System.Windows.Forms.Label();
            this.lblAccDevMeanMagnValueID2 = new System.Windows.Forms.Label();
            this.lblAccDevMeanAngleValueID2 = new System.Windows.Forms.Label();
            this.processCircleID2 = new MRG.Controls.UI.LoadingCircle();
            this.tbIP2ListenDevID2 = new System.Windows.Forms.TextBox();
            this.btnFindArduino2 = new System.Windows.Forms.Button();
            this.SearchingArduinoID2ProcessCircle = new MRG.Controls.UI.LoadingCircle();
            this.tabPageBcstLog = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbMainLog = new System.Windows.Forms.TextBox();
            this.btnSwapBcstLog = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSwitchShowingTotalLog = new System.Windows.Forms.Button();
            this.SensorsCalibration = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
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
            this.Preferencies = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.ipAddrValidatingCircle2 = new MRG.Controls.UI.LoadingCircle();
            this.camshotPeriodLabel = new System.Windows.Forms.Label();
            this.tbCamShotPeriod = new System.Windows.Forms.MaskedTextBox();
            this.camshotPeriodDataSavingCircle = new MRG.Controls.UI.LoadingCircle();
            this.camIPLabel = new System.Windows.Forms.Label();
            this.tbCamIP1 = new System.Windows.Forms.MaskedTextBox();
            this.ipAddrValidatingCircle1 = new MRG.Controls.UI.LoadingCircle();
            this.camUNameLabel = new System.Windows.Forms.Label();
            this.tbCamUName1 = new System.Windows.Forms.TextBox();
            this.camPWDLabel = new System.Windows.Forms.Label();
            this.tbCamPWD1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbCamUName2 = new System.Windows.Forms.TextBox();
            this.tbCamPWD2 = new System.Windows.Forms.TextBox();
            this.tbCamIP2 = new System.Windows.Forms.MaskedTextBox();
            this.arduinoBoardSearchingWorker = new System.ComponentModel.BackgroundWorker();
            this.udpCatchingJob = new System.ComponentModel.BackgroundWorker();
            this.ArduinoRequestExpectant = new System.ComponentModel.BackgroundWorker();
            this.dataCollector = new System.ComponentModel.BackgroundWorker();
            this.accelCalibrator = new System.ComponentModel.BackgroundWorker();
            this.bgwUDPmessagesParser = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbPreviewCam1)).BeginInit();
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
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1289, 694);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.tableLayoutPanel1);
            this.tabPageMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPageMain.Location = new System.Drawing.Point(4, 25);
            this.tabPageMain.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageMain.Size = new System.Drawing.Size(1281, 665);
            this.tabPageMain.TabIndex = 1;
            this.tabPageMain.Text = "Main";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 19;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 5, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbIP2ListenDevID1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFindArduino1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStartStopBdcstListening, 15, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStartStopCollecting, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.SearchingArduinoID1ProcessCircle, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.StartStopDataCollectingWaitingCircle, 18, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCollectMostClose, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnCollectImmediately, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblNextShotIn, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblSinceLastShot, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblAccelerometerSignID1, 10, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanMagnTitleID1, 13, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanAngleTitleID1, 15, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanMagnValueID1, 13, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanAngleValueID1, 15, 5);
            this.tableLayoutPanel1.Controls.Add(this.pbThumbPreviewCam1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblLonTitle, 16, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblLonValue, 16, 13);
            this.tableLayoutPanel1.Controls.Add(this.lblLatTitle, 13, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblLatValue, 13, 13);
            this.tableLayoutPanel1.Controls.Add(this.lblGPStitle, 10, 12);
            this.tableLayoutPanel1.Controls.Add(this.label17, 13, 15);
            this.tableLayoutPanel1.Controls.Add(this.lblUTCTimeValue, 13, 16);
            this.tableLayoutPanel1.Controls.Add(this.lblPressureTitle, 10, 19);
            this.tableLayoutPanel1.Controls.Add(this.lblPressureValue, 13, 19);
            this.tableLayoutPanel1.Controls.Add(this.processCircleID1, 17, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblAccelerometerSignID2, 10, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanMagnTitleID2, 13, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanAngleTitleID2, 15, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanMagnValueID2, 13, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblAccDevMeanAngleValueID2, 15, 8);
            this.tableLayoutPanel1.Controls.Add(this.processCircleID2, 17, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbIP2ListenDevID2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFindArduino2, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.SearchingArduinoID2ProcessCircle, 7, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 20;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1273, 657);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 4);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(339, 240);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox1, 13);
            this.pictureBox1.Size = new System.Drawing.Size(260, 413);
            this.pictureBox1.TabIndex = 96;
            this.pictureBox1.TabStop = false;
            // 
            // tbIP2ListenDevID1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbIP2ListenDevID1, 3);
            this.tbIP2ListenDevID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIP2ListenDevID1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbIP2ListenDevID1.Location = new System.Drawing.Point(4, 4);
            this.tbIP2ListenDevID1.Margin = new System.Windows.Forms.Padding(4);
            this.tbIP2ListenDevID1.Name = "tbIP2ListenDevID1";
            this.tbIP2ListenDevID1.Size = new System.Drawing.Size(193, 30);
            this.tbIP2ListenDevID1.TabIndex = 40;
            this.tbIP2ListenDevID1.Text = "192.0.0.101";
            this.tbIP2ListenDevID1.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnFindArduino1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnFindArduino1, 4);
            this.btnFindArduino1.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnFindArduino1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindArduino1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindArduino1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindArduino1.Location = new System.Drawing.Point(205, 4);
            this.btnFindArduino1.Margin = new System.Windows.Forms.Padding(4);
            this.btnFindArduino1.Name = "btnFindArduino1";
            this.btnFindArduino1.Size = new System.Drawing.Size(260, 32);
            this.btnFindArduino1.TabIndex = 41;
            this.btnFindArduino1.Text = "search for board ID1";
            this.btnFindArduino1.UseVisualStyleBackColor = true;
            this.btnFindArduino1.Click += new System.EventHandler(this.btnFindArduino_Click);
            // 
            // btnStartStopBdcstListening
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnStartStopBdcstListening, 4);
            this.btnStartStopBdcstListening.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartStopBdcstListening.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopBdcstListening.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStartStopBdcstListening.Location = new System.Drawing.Point(1009, 4);
            this.btnStartStopBdcstListening.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartStopBdcstListening.Name = "btnStartStopBdcstListening";
            this.tableLayoutPanel1.SetRowSpan(this.btnStartStopBdcstListening, 2);
            this.btnStartStopBdcstListening.Size = new System.Drawing.Size(260, 72);
            this.btnStartStopBdcstListening.TabIndex = 43;
            this.btnStartStopBdcstListening.Text = "Start listening";
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
            this.btnStartStopCollecting.Location = new System.Drawing.Point(4, 96);
            this.btnStartStopCollecting.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartStopCollecting.Name = "btnStartStopCollecting";
            this.btnStartStopCollecting.Size = new System.Drawing.Size(1198, 54);
            this.btnStartStopCollecting.TabIndex = 47;
            this.btnStartStopCollecting.Text = "Start collecting data";
            this.btnStartStopCollecting.UseVisualStyleBackColor = true;
            this.btnStartStopCollecting.Click += new System.EventHandler(this.btnStartStopCollecting_Click);
            // 
            // SearchingArduinoID1ProcessCircle
            // 
            this.SearchingArduinoID1ProcessCircle.Active = false;
            this.SearchingArduinoID1ProcessCircle.Color = System.Drawing.Color.DarkGray;
            this.SearchingArduinoID1ProcessCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchingArduinoID1ProcessCircle.InnerCircleRadius = 8;
            this.SearchingArduinoID1ProcessCircle.Location = new System.Drawing.Point(473, 4);
            this.SearchingArduinoID1ProcessCircle.Margin = new System.Windows.Forms.Padding(4);
            this.SearchingArduinoID1ProcessCircle.Name = "SearchingArduinoID1ProcessCircle";
            this.SearchingArduinoID1ProcessCircle.NumberSpoke = 24;
            this.SearchingArduinoID1ProcessCircle.OuterCircleRadius = 9;
            this.SearchingArduinoID1ProcessCircle.RotationSpeed = 100;
            this.SearchingArduinoID1ProcessCircle.Size = new System.Drawing.Size(59, 32);
            this.SearchingArduinoID1ProcessCircle.SpokeThickness = 4;
            this.SearchingArduinoID1ProcessCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.SearchingArduinoID1ProcessCircle.TabIndex = 75;
            this.SearchingArduinoID1ProcessCircle.Text = "loadingCircle1";
            this.SearchingArduinoID1ProcessCircle.Visible = false;
            // 
            // StartStopDataCollectingWaitingCircle
            // 
            this.StartStopDataCollectingWaitingCircle.Active = false;
            this.StartStopDataCollectingWaitingCircle.Color = System.Drawing.Color.Red;
            this.StartStopDataCollectingWaitingCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartStopDataCollectingWaitingCircle.ForeColor = System.Drawing.Color.Red;
            this.StartStopDataCollectingWaitingCircle.InnerCircleRadius = 8;
            this.StartStopDataCollectingWaitingCircle.Location = new System.Drawing.Point(1210, 96);
            this.StartStopDataCollectingWaitingCircle.Margin = new System.Windows.Forms.Padding(4);
            this.StartStopDataCollectingWaitingCircle.Name = "StartStopDataCollectingWaitingCircle";
            this.StartStopDataCollectingWaitingCircle.NumberSpoke = 24;
            this.StartStopDataCollectingWaitingCircle.OuterCircleRadius = 9;
            this.StartStopDataCollectingWaitingCircle.RotationSpeed = 100;
            this.StartStopDataCollectingWaitingCircle.Size = new System.Drawing.Size(59, 54);
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
            this.btnCollectMostClose.Location = new System.Drawing.Point(4, 158);
            this.btnCollectMostClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnCollectMostClose.Name = "btnCollectMostClose";
            this.btnCollectMostClose.Size = new System.Drawing.Size(193, 27);
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
            this.btnCollectImmediately.Location = new System.Drawing.Point(4, 193);
            this.btnCollectImmediately.Margin = new System.Windows.Forms.Padding(4);
            this.btnCollectImmediately.Name = "btnCollectImmediately";
            this.btnCollectImmediately.Size = new System.Drawing.Size(193, 27);
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
            this.label3.Location = new System.Drawing.Point(205, 154);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 35);
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
            this.label5.Location = new System.Drawing.Point(205, 189);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(193, 35);
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
            this.lblNextShotIn.Location = new System.Drawing.Point(406, 154);
            this.lblNextShotIn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNextShotIn.Name = "lblNextShotIn";
            this.lblNextShotIn.Size = new System.Drawing.Size(193, 35);
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
            this.lblSinceLastShot.Location = new System.Drawing.Point(406, 189);
            this.lblSinceLastShot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSinceLastShot.Name = "lblSinceLastShot";
            this.lblSinceLastShot.Size = new System.Drawing.Size(193, 35);
            this.lblSinceLastShot.TabIndex = 68;
            this.lblSinceLastShot.Text = "00:00:00";
            this.lblSinceLastShot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelerometerSignID1
            // 
            this.lblAccelerometerSignID1.AutoSize = true;
            this.lblAccelerometerSignID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccelerometerSignID1, 3);
            this.lblAccelerometerSignID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelerometerSignID1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAccelerometerSignID1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccelerometerSignID1.Location = new System.Drawing.Point(674, 154);
            this.lblAccelerometerSignID1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccelerometerSignID1.Name = "lblAccelerometerSignID1";
            this.tableLayoutPanel1.SetRowSpan(this.lblAccelerometerSignID1, 2);
            this.lblAccelerometerSignID1.Size = new System.Drawing.Size(193, 70);
            this.lblAccelerometerSignID1.TabIndex = 49;
            this.lblAccelerometerSignID1.Text = "Accelerometer data ID1";
            this.lblAccelerometerSignID1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAccelerometerSignID1.Click += new System.EventHandler(this.lblAccelerometerSign_Click);
            // 
            // lblAccDevMeanMagnTitleID1
            // 
            this.lblAccDevMeanMagnTitleID1.AutoSize = true;
            this.lblAccDevMeanMagnTitleID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanMagnTitleID1, 2);
            this.lblAccDevMeanMagnTitleID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanMagnTitleID1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanMagnTitleID1.Location = new System.Drawing.Point(875, 154);
            this.lblAccDevMeanMagnTitleID1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccDevMeanMagnTitleID1.Name = "lblAccDevMeanMagnTitleID1";
            this.lblAccDevMeanMagnTitleID1.Size = new System.Drawing.Size(126, 35);
            this.lblAccDevMeanMagnTitleID1.TabIndex = 50;
            this.lblAccDevMeanMagnTitleID1.Text = "magnitude";
            this.lblAccDevMeanMagnTitleID1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanAngleTitleID1
            // 
            this.lblAccDevMeanAngleTitleID1.AutoSize = true;
            this.lblAccDevMeanAngleTitleID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanAngleTitleID1, 2);
            this.lblAccDevMeanAngleTitleID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanAngleTitleID1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanAngleTitleID1.Location = new System.Drawing.Point(1009, 154);
            this.lblAccDevMeanAngleTitleID1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccDevMeanAngleTitleID1.Name = "lblAccDevMeanAngleTitleID1";
            this.lblAccDevMeanAngleTitleID1.Size = new System.Drawing.Size(126, 35);
            this.lblAccDevMeanAngleTitleID1.TabIndex = 51;
            this.lblAccDevMeanAngleTitleID1.Text = "dev.angle";
            this.lblAccDevMeanAngleTitleID1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanMagnValueID1
            // 
            this.lblAccDevMeanMagnValueID1.AutoSize = true;
            this.lblAccDevMeanMagnValueID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanMagnValueID1, 2);
            this.lblAccDevMeanMagnValueID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanMagnValueID1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanMagnValueID1.Location = new System.Drawing.Point(875, 189);
            this.lblAccDevMeanMagnValueID1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccDevMeanMagnValueID1.Name = "lblAccDevMeanMagnValueID1";
            this.lblAccDevMeanMagnValueID1.Size = new System.Drawing.Size(126, 35);
            this.lblAccDevMeanMagnValueID1.TabIndex = 53;
            this.lblAccDevMeanMagnValueID1.Text = "---";
            this.lblAccDevMeanMagnValueID1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanAngleValueID1
            // 
            this.lblAccDevMeanAngleValueID1.AutoSize = true;
            this.lblAccDevMeanAngleValueID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanAngleValueID1, 2);
            this.lblAccDevMeanAngleValueID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanAngleValueID1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanAngleValueID1.Location = new System.Drawing.Point(1009, 189);
            this.lblAccDevMeanAngleValueID1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccDevMeanAngleValueID1.Name = "lblAccDevMeanAngleValueID1";
            this.lblAccDevMeanAngleValueID1.Size = new System.Drawing.Size(126, 35);
            this.lblAccDevMeanAngleValueID1.TabIndex = 54;
            this.lblAccDevMeanAngleValueID1.Text = "---";
            this.lblAccDevMeanAngleValueID1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbThumbPreviewCam1
            // 
            this.pbThumbPreviewCam1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pbThumbPreviewCam1, 4);
            this.pbThumbPreviewCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbThumbPreviewCam1.Location = new System.Drawing.Point(4, 240);
            this.pbThumbPreviewCam1.Margin = new System.Windows.Forms.Padding(4);
            this.pbThumbPreviewCam1.Name = "pbThumbPreviewCam1";
            this.tableLayoutPanel1.SetRowSpan(this.pbThumbPreviewCam1, 13);
            this.pbThumbPreviewCam1.Size = new System.Drawing.Size(260, 413);
            this.pbThumbPreviewCam1.TabIndex = 57;
            this.pbThumbPreviewCam1.TabStop = false;
            this.pbThumbPreviewCam1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblLonTitle
            // 
            this.lblLonTitle.AutoSize = true;
            this.lblLonTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblLonTitle, 3);
            this.lblLonTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLonTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLonTitle.Location = new System.Drawing.Point(1076, 392);
            this.lblLonTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLonTitle.Name = "lblLonTitle";
            this.lblLonTitle.Size = new System.Drawing.Size(193, 37);
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
            this.lblLonValue.Location = new System.Drawing.Point(1076, 429);
            this.lblLonValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLonValue.Name = "lblLonValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblLonValue, 2);
            this.lblLonValue.Size = new System.Drawing.Size(193, 70);
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
            this.lblLatTitle.Location = new System.Drawing.Point(875, 392);
            this.lblLatTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLatTitle.Name = "lblLatTitle";
            this.lblLatTitle.Size = new System.Drawing.Size(193, 37);
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
            this.lblLatValue.Location = new System.Drawing.Point(875, 429);
            this.lblLatValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLatValue.Name = "lblLatValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblLatValue, 2);
            this.lblLatValue.Size = new System.Drawing.Size(193, 70);
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
            this.lblGPStitle.Location = new System.Drawing.Point(674, 392);
            this.lblGPStitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPStitle.Name = "lblGPStitle";
            this.tableLayoutPanel1.SetRowSpan(this.lblGPStitle, 6);
            this.lblGPStitle.Size = new System.Drawing.Size(193, 214);
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
            this.label17.Location = new System.Drawing.Point(875, 499);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(394, 37);
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
            this.lblUTCTimeValue.Location = new System.Drawing.Point(875, 536);
            this.lblUTCTimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUTCTimeValue.Name = "lblUTCTimeValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblUTCTimeValue, 2);
            this.lblUTCTimeValue.Size = new System.Drawing.Size(394, 70);
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
            this.lblPressureTitle.Location = new System.Drawing.Point(674, 618);
            this.lblPressureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPressureTitle.Name = "lblPressureTitle";
            this.lblPressureTitle.Size = new System.Drawing.Size(193, 39);
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
            this.lblPressureValue.Location = new System.Drawing.Point(875, 618);
            this.lblPressureValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPressureValue.Name = "lblPressureValue";
            this.lblPressureValue.Size = new System.Drawing.Size(394, 39);
            this.lblPressureValue.TabIndex = 94;
            this.lblPressureValue.Text = "---";
            this.lblPressureValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // processCircleID1
            // 
            this.processCircleID1.Active = false;
            this.processCircleID1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel1.SetColumnSpan(this.processCircleID1, 2);
            this.processCircleID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processCircleID1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.processCircleID1.InnerCircleRadius = 5;
            this.processCircleID1.Location = new System.Drawing.Point(1143, 158);
            this.processCircleID1.Margin = new System.Windows.Forms.Padding(4);
            this.processCircleID1.Name = "processCircleID1";
            this.processCircleID1.NumberSpoke = 12;
            this.processCircleID1.OuterCircleRadius = 11;
            this.processCircleID1.RotationSpeed = 100;
            this.tableLayoutPanel1.SetRowSpan(this.processCircleID1, 2);
            this.processCircleID1.Size = new System.Drawing.Size(126, 62);
            this.processCircleID1.SpokeThickness = 2;
            this.processCircleID1.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.processCircleID1.TabIndex = 95;
            this.processCircleID1.Visible = false;
            // 
            // lblAccelerometerSignID2
            // 
            this.lblAccelerometerSignID2.AutoSize = true;
            this.lblAccelerometerSignID2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccelerometerSignID2, 3);
            this.lblAccelerometerSignID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelerometerSignID2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccelerometerSignID2.Location = new System.Drawing.Point(673, 236);
            this.lblAccelerometerSignID2.Name = "lblAccelerometerSignID2";
            this.tableLayoutPanel1.SetRowSpan(this.lblAccelerometerSignID2, 2);
            this.lblAccelerometerSignID2.Size = new System.Drawing.Size(195, 72);
            this.lblAccelerometerSignID2.TabIndex = 97;
            this.lblAccelerometerSignID2.Text = "Accelerometer data ID2";
            this.lblAccelerometerSignID2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanMagnTitleID2
            // 
            this.lblAccDevMeanMagnTitleID2.AutoSize = true;
            this.lblAccDevMeanMagnTitleID2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanMagnTitleID2, 2);
            this.lblAccDevMeanMagnTitleID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanMagnTitleID2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanMagnTitleID2.Location = new System.Drawing.Point(874, 236);
            this.lblAccDevMeanMagnTitleID2.Name = "lblAccDevMeanMagnTitleID2";
            this.lblAccDevMeanMagnTitleID2.Size = new System.Drawing.Size(128, 37);
            this.lblAccDevMeanMagnTitleID2.TabIndex = 98;
            this.lblAccDevMeanMagnTitleID2.Text = "magnitude";
            this.lblAccDevMeanMagnTitleID2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanAngleTitleID2
            // 
            this.lblAccDevMeanAngleTitleID2.AutoSize = true;
            this.lblAccDevMeanAngleTitleID2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanAngleTitleID2, 2);
            this.lblAccDevMeanAngleTitleID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanAngleTitleID2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanAngleTitleID2.Location = new System.Drawing.Point(1008, 236);
            this.lblAccDevMeanAngleTitleID2.Name = "lblAccDevMeanAngleTitleID2";
            this.lblAccDevMeanAngleTitleID2.Size = new System.Drawing.Size(128, 37);
            this.lblAccDevMeanAngleTitleID2.TabIndex = 99;
            this.lblAccDevMeanAngleTitleID2.Text = "dev.angle";
            this.lblAccDevMeanAngleTitleID2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanMagnValueID2
            // 
            this.lblAccDevMeanMagnValueID2.AutoSize = true;
            this.lblAccDevMeanMagnValueID2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanMagnValueID2, 2);
            this.lblAccDevMeanMagnValueID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanMagnValueID2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanMagnValueID2.Location = new System.Drawing.Point(874, 273);
            this.lblAccDevMeanMagnValueID2.Name = "lblAccDevMeanMagnValueID2";
            this.lblAccDevMeanMagnValueID2.Size = new System.Drawing.Size(128, 35);
            this.lblAccDevMeanMagnValueID2.TabIndex = 100;
            this.lblAccDevMeanMagnValueID2.Text = "---";
            this.lblAccDevMeanMagnValueID2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccDevMeanAngleValueID2
            // 
            this.lblAccDevMeanAngleValueID2.AutoSize = true;
            this.lblAccDevMeanAngleValueID2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblAccDevMeanAngleValueID2, 2);
            this.lblAccDevMeanAngleValueID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccDevMeanAngleValueID2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccDevMeanAngleValueID2.Location = new System.Drawing.Point(1008, 273);
            this.lblAccDevMeanAngleValueID2.Name = "lblAccDevMeanAngleValueID2";
            this.lblAccDevMeanAngleValueID2.Size = new System.Drawing.Size(128, 35);
            this.lblAccDevMeanAngleValueID2.TabIndex = 101;
            this.lblAccDevMeanAngleValueID2.Text = "---";
            this.lblAccDevMeanAngleValueID2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // processCircleID2
            // 
            this.processCircleID2.Active = false;
            this.processCircleID2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel1.SetColumnSpan(this.processCircleID2, 2);
            this.processCircleID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processCircleID2.InnerCircleRadius = 5;
            this.processCircleID2.Location = new System.Drawing.Point(1142, 239);
            this.processCircleID2.Name = "processCircleID2";
            this.processCircleID2.NumberSpoke = 12;
            this.processCircleID2.OuterCircleRadius = 11;
            this.processCircleID2.RotationSpeed = 100;
            this.tableLayoutPanel1.SetRowSpan(this.processCircleID2, 2);
            this.processCircleID2.Size = new System.Drawing.Size(128, 66);
            this.processCircleID2.SpokeThickness = 2;
            this.processCircleID2.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.processCircleID2.TabIndex = 95;
            this.processCircleID2.Text = "loadingCircle1";
            this.processCircleID2.Visible = false;
            // 
            // tbIP2ListenDevID2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbIP2ListenDevID2, 3);
            this.tbIP2ListenDevID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIP2ListenDevID2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbIP2ListenDevID2.Location = new System.Drawing.Point(3, 43);
            this.tbIP2ListenDevID2.Name = "tbIP2ListenDevID2";
            this.tbIP2ListenDevID2.Size = new System.Drawing.Size(195, 30);
            this.tbIP2ListenDevID2.TabIndex = 102;
            this.tbIP2ListenDevID2.Text = "192.0.0.102";
            this.tbIP2ListenDevID2.TextChanged += new System.EventHandler(this.tbIP2ListenDevID2_TextChanged);
            // 
            // btnFindArduino2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnFindArduino2, 4);
            this.btnFindArduino2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindArduino2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindArduino2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindArduino2.Location = new System.Drawing.Point(204, 43);
            this.btnFindArduino2.Name = "btnFindArduino2";
            this.btnFindArduino2.Size = new System.Drawing.Size(262, 34);
            this.btnFindArduino2.TabIndex = 103;
            this.btnFindArduino2.Text = "search for board ID2";
            this.btnFindArduino2.UseVisualStyleBackColor = true;
            // 
            // SearchingArduinoID2ProcessCircle
            // 
            this.SearchingArduinoID2ProcessCircle.Active = false;
            this.SearchingArduinoID2ProcessCircle.Color = System.Drawing.Color.DarkGray;
            this.SearchingArduinoID2ProcessCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchingArduinoID2ProcessCircle.InnerCircleRadius = 8;
            this.SearchingArduinoID2ProcessCircle.Location = new System.Drawing.Point(473, 44);
            this.SearchingArduinoID2ProcessCircle.Margin = new System.Windows.Forms.Padding(4);
            this.SearchingArduinoID2ProcessCircle.Name = "SearchingArduinoID2ProcessCircle";
            this.SearchingArduinoID2ProcessCircle.NumberSpoke = 24;
            this.SearchingArduinoID2ProcessCircle.OuterCircleRadius = 9;
            this.SearchingArduinoID2ProcessCircle.RotationSpeed = 100;
            this.SearchingArduinoID2ProcessCircle.Size = new System.Drawing.Size(59, 32);
            this.SearchingArduinoID2ProcessCircle.SpokeThickness = 4;
            this.SearchingArduinoID2ProcessCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.SearchingArduinoID2ProcessCircle.TabIndex = 104;
            this.SearchingArduinoID2ProcessCircle.Text = "loadingCircle1";
            this.SearchingArduinoID2ProcessCircle.Visible = false;
            // 
            // tabPageBcstLog
            // 
            this.tabPageBcstLog.Controls.Add(this.tableLayoutPanel2);
            this.tabPageBcstLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPageBcstLog.Location = new System.Drawing.Point(4, 25);
            this.tabPageBcstLog.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageBcstLog.Name = "tabPageBcstLog";
            this.tabPageBcstLog.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageBcstLog.Size = new System.Drawing.Size(1281, 665);
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
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.tbMainLog, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnSwapBcstLog, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnSwitchShowingTotalLog, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1273, 657);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tbMainLog
            // 
            this.tbMainLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel2.SetColumnSpan(this.tbMainLog, 6);
            this.tbMainLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMainLog.Location = new System.Drawing.Point(4, 90);
            this.tbMainLog.Margin = new System.Windows.Forms.Padding(4);
            this.tbMainLog.Multiline = true;
            this.tbMainLog.Name = "tbMainLog";
            this.tbMainLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMainLog.Size = new System.Drawing.Size(1265, 563);
            this.tbMainLog.TabIndex = 10;
            // 
            // btnSwapBcstLog
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.btnSwapBcstLog, 3);
            this.btnSwapBcstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSwapBcstLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapBcstLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSwapBcstLog.Location = new System.Drawing.Point(640, 4);
            this.btnSwapBcstLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnSwapBcstLog.Name = "btnSwapBcstLog";
            this.btnSwapBcstLog.Size = new System.Drawing.Size(629, 41);
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
            this.label11.Location = new System.Drawing.Point(4, 49);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(1265, 37);
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
            this.btnSwitchShowingTotalLog.Location = new System.Drawing.Point(4, 4);
            this.btnSwitchShowingTotalLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnSwitchShowingTotalLog.Name = "btnSwitchShowingTotalLog";
            this.btnSwitchShowingTotalLog.Size = new System.Drawing.Size(628, 41);
            this.btnSwitchShowingTotalLog.TabIndex = 12;
            this.btnSwitchShowingTotalLog.Text = "Show total log";
            this.btnSwitchShowingTotalLog.UseVisualStyleBackColor = true;
            this.btnSwitchShowingTotalLog.Click += new System.EventHandler(this.btnSwitchShowingTotalLog_Click);
            // 
            // SensorsCalibration
            // 
            this.SensorsCalibration.Controls.Add(this.tableLayoutPanel3);
            this.SensorsCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SensorsCalibration.Location = new System.Drawing.Point(4, 25);
            this.SensorsCalibration.Margin = new System.Windows.Forms.Padding(4);
            this.SensorsCalibration.Name = "SensorsCalibration";
            this.SensorsCalibration.Padding = new System.Windows.Forms.Padding(4);
            this.SensorsCalibration.Size = new System.Drawing.Size(1281, 665);
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
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
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
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1273, 657);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(258, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 43);
            this.label4.TabIndex = 57;
            this.label4.Text = "X";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(385, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 43);
            this.label6.TabIndex = 58;
            this.label6.Text = "Y";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(512, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 43);
            this.label7.TabIndex = 59;
            this.label7.Text = "Z";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationCurrentX
            // 
            this.lblAccelCalibrationCurrentX.AutoSize = true;
            this.lblAccelCalibrationCurrentX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationCurrentX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationCurrentX.Location = new System.Drawing.Point(258, 43);
            this.lblAccelCalibrationCurrentX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccelCalibrationCurrentX.Name = "lblAccelCalibrationCurrentX";
            this.lblAccelCalibrationCurrentX.Size = new System.Drawing.Size(119, 43);
            this.lblAccelCalibrationCurrentX.TabIndex = 60;
            this.lblAccelCalibrationCurrentX.Text = "0";
            this.lblAccelCalibrationCurrentX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationCurrentY
            // 
            this.lblAccelCalibrationCurrentY.AutoSize = true;
            this.lblAccelCalibrationCurrentY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationCurrentY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationCurrentY.Location = new System.Drawing.Point(385, 43);
            this.lblAccelCalibrationCurrentY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccelCalibrationCurrentY.Name = "lblAccelCalibrationCurrentY";
            this.lblAccelCalibrationCurrentY.Size = new System.Drawing.Size(119, 43);
            this.lblAccelCalibrationCurrentY.TabIndex = 61;
            this.lblAccelCalibrationCurrentY.Text = "0";
            this.lblAccelCalibrationCurrentY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationCurrentZ
            // 
            this.lblAccelCalibrationCurrentZ.AutoSize = true;
            this.lblAccelCalibrationCurrentZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationCurrentZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationCurrentZ.Location = new System.Drawing.Point(512, 43);
            this.lblAccelCalibrationCurrentZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccelCalibrationCurrentZ.Name = "lblAccelCalibrationCurrentZ";
            this.lblAccelCalibrationCurrentZ.Size = new System.Drawing.Size(119, 43);
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
            this.btnCalibrateAccelerometer.Location = new System.Drawing.Point(4, 4);
            this.btnCalibrateAccelerometer.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalibrateAccelerometer.Name = "btnCalibrateAccelerometer";
            this.tableLayoutPanel3.SetRowSpan(this.btnCalibrateAccelerometer, 5);
            this.btnCalibrateAccelerometer.Size = new System.Drawing.Size(246, 207);
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
            this.lblAccelCalibrationX.Location = new System.Drawing.Point(258, 86);
            this.lblAccelCalibrationX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccelCalibrationX.Name = "lblAccelCalibrationX";
            this.lblAccelCalibrationX.Size = new System.Drawing.Size(119, 43);
            this.lblAccelCalibrationX.TabIndex = 64;
            this.lblAccelCalibrationX.Text = "<0>";
            this.lblAccelCalibrationX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationY
            // 
            this.lblAccelCalibrationY.AutoSize = true;
            this.lblAccelCalibrationY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationY.Location = new System.Drawing.Point(385, 86);
            this.lblAccelCalibrationY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccelCalibrationY.Name = "lblAccelCalibrationY";
            this.lblAccelCalibrationY.Size = new System.Drawing.Size(119, 43);
            this.lblAccelCalibrationY.TabIndex = 65;
            this.lblAccelCalibrationY.Text = "<0>";
            this.lblAccelCalibrationY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccelCalibrationZ
            // 
            this.lblAccelCalibrationZ.AutoSize = true;
            this.lblAccelCalibrationZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccelCalibrationZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccelCalibrationZ.Location = new System.Drawing.Point(512, 86);
            this.lblAccelCalibrationZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccelCalibrationZ.Name = "lblAccelCalibrationZ";
            this.lblAccelCalibrationZ.Size = new System.Drawing.Size(119, 43);
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
            this.lblCalculationStatistics.Location = new System.Drawing.Point(258, 172);
            this.lblCalculationStatistics.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCalculationStatistics.Name = "lblCalculationStatistics";
            this.lblCalculationStatistics.Size = new System.Drawing.Size(373, 43);
            this.lblCalculationStatistics.TabIndex = 67;
            this.lblCalculationStatistics.Text = "---";
            // 
            // lblStDevX
            // 
            this.lblStDevX.AutoSize = true;
            this.lblStDevX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStDevX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStDevX.Location = new System.Drawing.Point(258, 129);
            this.lblStDevX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStDevX.Name = "lblStDevX";
            this.lblStDevX.Size = new System.Drawing.Size(119, 43);
            this.lblStDevX.TabIndex = 68;
            this.lblStDevX.Text = "0%";
            this.lblStDevX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStDevY
            // 
            this.lblStDevY.AutoSize = true;
            this.lblStDevY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStDevY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStDevY.Location = new System.Drawing.Point(385, 129);
            this.lblStDevY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStDevY.Name = "lblStDevY";
            this.lblStDevY.Size = new System.Drawing.Size(119, 43);
            this.lblStDevY.TabIndex = 69;
            this.lblStDevY.Text = "0%";
            this.lblStDevY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStDevZ
            // 
            this.lblStDevZ.AutoSize = true;
            this.lblStDevZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStDevZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStDevZ.Location = new System.Drawing.Point(512, 129);
            this.lblStDevZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStDevZ.Name = "lblStDevZ";
            this.lblStDevZ.Size = new System.Drawing.Size(119, 43);
            this.lblStDevZ.TabIndex = 70;
            this.lblStDevZ.Text = "0%";
            this.lblStDevZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSaveAccel
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.btnSaveAccel, 3);
            this.btnSaveAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveAccel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAccel.Location = new System.Drawing.Point(639, 4);
            this.btnSaveAccel.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveAccel.Name = "btnSaveAccel";
            this.tableLayoutPanel3.SetRowSpan(this.btnSaveAccel, 5);
            this.btnSaveAccel.Size = new System.Drawing.Size(373, 207);
            this.btnSaveAccel.TabIndex = 71;
            this.btnSaveAccel.Text = "Save calibration data";
            this.btnSaveAccel.UseVisualStyleBackColor = true;
            this.btnSaveAccel.Click += new System.EventHandler(this.btnSaveAccel_Click);
            // 
            // Preferencies
            // 
            this.Preferencies.BackColor = System.Drawing.Color.Transparent;
            this.Preferencies.Controls.Add(this.tableLayoutPanel4);
            this.Preferencies.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Preferencies.Location = new System.Drawing.Point(4, 25);
            this.Preferencies.Margin = new System.Windows.Forms.Padding(4);
            this.Preferencies.Name = "Preferencies";
            this.Preferencies.Padding = new System.Windows.Forms.Padding(4);
            this.Preferencies.Size = new System.Drawing.Size(1281, 665);
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
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel4.Controls.Add(this.ipAddrValidatingCircle2, 4, 5);
            this.tableLayoutPanel4.Controls.Add(this.camshotPeriodLabel, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tbCamShotPeriod, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.camshotPeriodDataSavingCircle, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.camIPLabel, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.tbCamIP1, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.ipAddrValidatingCircle1, 4, 1);
            this.tableLayoutPanel4.Controls.Add(this.camUNameLabel, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tbCamUName1, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.camPWDLabel, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.tbCamPWD1, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.tbCamUName2, 1, 6);
            this.tableLayoutPanel4.Controls.Add(this.tbCamPWD2, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.tbCamIP2, 1, 5);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 10;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1273, 657);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // ipAddrValidatingCircle2
            // 
            this.ipAddrValidatingCircle2.Active = false;
            this.ipAddrValidatingCircle2.Color = System.Drawing.Color.DarkGray;
            this.ipAddrValidatingCircle2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ipAddrValidatingCircle2.InnerCircleRadius = 8;
            this.ipAddrValidatingCircle2.Location = new System.Drawing.Point(1216, 184);
            this.ipAddrValidatingCircle2.Margin = new System.Windows.Forms.Padding(4);
            this.ipAddrValidatingCircle2.Name = "ipAddrValidatingCircle2";
            this.ipAddrValidatingCircle2.NumberSpoke = 24;
            this.ipAddrValidatingCircle2.OuterCircleRadius = 9;
            this.ipAddrValidatingCircle2.RotationSpeed = 100;
            this.ipAddrValidatingCircle2.Size = new System.Drawing.Size(53, 32);
            this.ipAddrValidatingCircle2.SpokeThickness = 4;
            this.ipAddrValidatingCircle2.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.ipAddrValidatingCircle2.TabIndex = 96;
            this.ipAddrValidatingCircle2.Visible = false;
            // 
            // camshotPeriodLabel
            // 
            this.camshotPeriodLabel.AutoSize = true;
            this.camshotPeriodLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camshotPeriodLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camshotPeriodLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camshotPeriodLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.camshotPeriodLabel.Location = new System.Drawing.Point(4, 0);
            this.camshotPeriodLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.camshotPeriodLabel.Name = "camshotPeriodLabel";
            this.camshotPeriodLabel.Size = new System.Drawing.Size(295, 40);
            this.camshotPeriodLabel.TabIndex = 72;
            this.camshotPeriodLabel.Text = "camera shooting period:";
            this.camshotPeriodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamShotPeriod
            // 
            this.tbCamShotPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamShotPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCamShotPeriod.Location = new System.Drawing.Point(307, 4);
            this.tbCamShotPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.tbCamShotPeriod.Mask = "00:00:00";
            this.tbCamShotPeriod.Name = "tbCamShotPeriod";
            this.tbCamShotPeriod.Size = new System.Drawing.Size(295, 30);
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
            this.camshotPeriodDataSavingCircle.Location = new System.Drawing.Point(1216, 4);
            this.camshotPeriodDataSavingCircle.Margin = new System.Windows.Forms.Padding(4);
            this.camshotPeriodDataSavingCircle.Name = "camshotPeriodDataSavingCircle";
            this.camshotPeriodDataSavingCircle.NumberSpoke = 24;
            this.camshotPeriodDataSavingCircle.OuterCircleRadius = 9;
            this.camshotPeriodDataSavingCircle.RotationSpeed = 100;
            this.camshotPeriodDataSavingCircle.Size = new System.Drawing.Size(53, 32);
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
            this.camIPLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.camIPLabel.Location = new System.Drawing.Point(4, 40);
            this.camIPLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.camIPLabel.Name = "camIPLabel";
            this.camIPLabel.Size = new System.Drawing.Size(295, 40);
            this.camIPLabel.TabIndex = 79;
            this.camIPLabel.Text = "Camera-1 IP";
            this.camIPLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamIP1
            // 
            this.tbCamIP1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamIP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCamIP1.Location = new System.Drawing.Point(307, 44);
            this.tbCamIP1.Margin = new System.Windows.Forms.Padding(4);
            this.tbCamIP1.Mask = "000.000.000.000";
            this.tbCamIP1.Name = "tbCamIP1";
            this.tbCamIP1.Size = new System.Drawing.Size(295, 30);
            this.tbCamIP1.TabIndex = 80;
            this.tbCamIP1.TextChanged += new System.EventHandler(this.tbCamIP_TextChanged);
            // 
            // ipAddrValidatingCircle1
            // 
            this.ipAddrValidatingCircle1.Active = false;
            this.ipAddrValidatingCircle1.Color = System.Drawing.Color.DarkGray;
            this.ipAddrValidatingCircle1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ipAddrValidatingCircle1.InnerCircleRadius = 8;
            this.ipAddrValidatingCircle1.Location = new System.Drawing.Point(1216, 44);
            this.ipAddrValidatingCircle1.Margin = new System.Windows.Forms.Padding(4);
            this.ipAddrValidatingCircle1.Name = "ipAddrValidatingCircle1";
            this.ipAddrValidatingCircle1.NumberSpoke = 24;
            this.ipAddrValidatingCircle1.OuterCircleRadius = 9;
            this.ipAddrValidatingCircle1.RotationSpeed = 100;
            this.ipAddrValidatingCircle1.Size = new System.Drawing.Size(53, 32);
            this.ipAddrValidatingCircle1.SpokeThickness = 4;
            this.ipAddrValidatingCircle1.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.ipAddrValidatingCircle1.TabIndex = 81;
            this.ipAddrValidatingCircle1.Text = "loadingCircle1";
            this.ipAddrValidatingCircle1.Visible = false;
            // 
            // camUNameLabel
            // 
            this.camUNameLabel.AutoSize = true;
            this.camUNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camUNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camUNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camUNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.camUNameLabel.Location = new System.Drawing.Point(4, 80);
            this.camUNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.camUNameLabel.Name = "camUNameLabel";
            this.camUNameLabel.Size = new System.Drawing.Size(295, 40);
            this.camUNameLabel.TabIndex = 82;
            this.camUNameLabel.Text = "camera username";
            this.camUNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamUName1
            // 
            this.tbCamUName1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamUName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCamUName1.Location = new System.Drawing.Point(307, 84);
            this.tbCamUName1.Margin = new System.Windows.Forms.Padding(4);
            this.tbCamUName1.Name = "tbCamUName1";
            this.tbCamUName1.Size = new System.Drawing.Size(295, 30);
            this.tbCamUName1.TabIndex = 83;
            this.tbCamUName1.TextChanged += new System.EventHandler(this.tbCamUName_TextChanged);
            // 
            // camPWDLabel
            // 
            this.camPWDLabel.AutoSize = true;
            this.camPWDLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camPWDLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camPWDLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camPWDLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.camPWDLabel.Location = new System.Drawing.Point(4, 120);
            this.camPWDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.camPWDLabel.Name = "camPWDLabel";
            this.camPWDLabel.Size = new System.Drawing.Size(295, 40);
            this.camPWDLabel.TabIndex = 84;
            this.camPWDLabel.Text = "camera password";
            this.camPWDLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamPWD1
            // 
            this.tbCamPWD1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamPWD1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCamPWD1.Location = new System.Drawing.Point(307, 124);
            this.tbCamPWD1.Margin = new System.Windows.Forms.Padding(4);
            this.tbCamPWD1.Name = "tbCamPWD1";
            this.tbCamPWD1.Size = new System.Drawing.Size(295, 30);
            this.tbCamPWD1.TabIndex = 85;
            this.tbCamPWD1.TextChanged += new System.EventHandler(this.tbCamPWD_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(297, 40);
            this.label2.TabIndex = 87;
            this.label2.Text = "Camera-2 IP";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(3, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(297, 40);
            this.label8.TabIndex = 88;
            this.label8.Text = "username";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(3, 260);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(297, 40);
            this.label9.TabIndex = 89;
            this.label9.Text = "password";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamUName2
            // 
            this.tbCamUName2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamUName2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCamUName2.Location = new System.Drawing.Point(306, 223);
            this.tbCamUName2.Name = "tbCamUName2";
            this.tbCamUName2.Size = new System.Drawing.Size(297, 30);
            this.tbCamUName2.TabIndex = 92;
            this.tbCamUName2.TextChanged += new System.EventHandler(this.tbCamUName2_TextChanged);
            // 
            // tbCamPWD2
            // 
            this.tbCamPWD2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamPWD2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCamPWD2.Location = new System.Drawing.Point(306, 263);
            this.tbCamPWD2.Name = "tbCamPWD2";
            this.tbCamPWD2.Size = new System.Drawing.Size(297, 30);
            this.tbCamPWD2.TabIndex = 93;
            this.tbCamPWD2.TextChanged += new System.EventHandler(this.tbCamPWD2_TextChanged);
            // 
            // tbCamIP2
            // 
            this.tbCamIP2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamIP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCamIP2.Location = new System.Drawing.Point(306, 183);
            this.tbCamIP2.Mask = "000.000.000.000";
            this.tbCamIP2.Name = "tbCamIP2";
            this.tbCamIP2.Size = new System.Drawing.Size(297, 30);
            this.tbCamIP2.TabIndex = 95;
            this.tbCamIP2.TextChanged += new System.EventHandler(this.maskedTextBox2_TextChanged);
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
            // bgwUDPmessagesParser
            // 
            this.bgwUDPmessagesParser.WorkerSupportsCancellation = true;
            this.bgwUDPmessagesParser.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwUDPmessagesParser_DoWork);
            // 
            // DataCollectorMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1289, 694);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DataCollectorMainForm";
            this.Text = "Data collector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataCollectorMainForm_FormClosing);
            this.Load += new System.EventHandler(this.DataCollectorMainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbPreviewCam1)).EndInit();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbIP2ListenDevID1;
        private System.Windows.Forms.Button btnFindArduino1;
        private System.Windows.Forms.Button btnStartStopCollecting;
        private System.Windows.Forms.Button btnCollectImmediately;
        private System.Windows.Forms.Label lblAccelerometerSignID1;
        private System.Windows.Forms.PictureBox pbThumbPreviewCam1;
        private System.Windows.Forms.Button btnCollectMostClose;
        private System.Windows.Forms.Label lblNextShotIn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSinceLastShot;
        private System.Windows.Forms.Label label5;
        private MRG.Controls.UI.LoadingCircle SearchingArduinoID1ProcessCircle;
        private MRG.Controls.UI.LoadingCircle StartStopDataCollectingWaitingCircle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label camshotPeriodLabel;
        private System.Windows.Forms.MaskedTextBox tbCamShotPeriod;
        private MRG.Controls.UI.LoadingCircle camshotPeriodDataSavingCircle;
        private System.Windows.Forms.Label camIPLabel;
        private System.Windows.Forms.MaskedTextBox tbCamIP1;
        private MRG.Controls.UI.LoadingCircle ipAddrValidatingCircle1;
        private System.Windows.Forms.Label camUNameLabel;
        private System.Windows.Forms.TextBox tbCamUName1;
        private System.Windows.Forms.Label camPWDLabel;
        private System.Windows.Forms.TextBox tbCamPWD1;
        private System.Windows.Forms.TextBox tbMainLog;
        private System.Windows.Forms.Label label11;
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
        private System.Windows.Forms.Label lblAccDevMeanMagnTitleID1;
        private System.Windows.Forms.Label lblAccDevMeanAngleTitleID1;
        private System.Windows.Forms.Label lblAccDevMeanMagnValueID1;
        private System.Windows.Forms.Label lblAccDevMeanAngleValueID1;
        private System.ComponentModel.BackgroundWorker bgwUDPmessagesParser;
        private MRG.Controls.UI.LoadingCircle processCircleID1;
        private System.Windows.Forms.Button btnStartStopBdcstListening;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblAccelerometerSignID2;
        private System.Windows.Forms.Label lblAccDevMeanMagnTitleID2;
        private System.Windows.Forms.Label lblAccDevMeanAngleTitleID2;
        private System.Windows.Forms.Label lblAccDevMeanMagnValueID2;
        private System.Windows.Forms.Label lblAccDevMeanAngleValueID2;
        private MRG.Controls.UI.LoadingCircle processCircleID2;
        private System.Windows.Forms.TextBox tbIP2ListenDevID2;
        private System.Windows.Forms.Button btnFindArduino2;
        private MRG.Controls.UI.LoadingCircle SearchingArduinoID2ProcessCircle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbCamUName2;
        private System.Windows.Forms.TextBox tbCamPWD2;
        private System.Windows.Forms.MaskedTextBox tbCamIP2;
        private MRG.Controls.UI.LoadingCircle ipAddrValidatingCircle2;


    }
}

