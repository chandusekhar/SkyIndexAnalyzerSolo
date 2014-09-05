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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnFindArduino1 = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFindArduino1, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStartStopBdcstListening, 15, 0);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 19;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
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
            // textBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox2, 4);
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(4, 4);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(256, 29);
            this.textBox2.TabIndex = 40;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnFindArduino1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnFindArduino1, 3);
            this.btnFindArduino1.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnFindArduino1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindArduino1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindArduino1.Location = new System.Drawing.Point(268, 4);
            this.btnFindArduino1.Margin = new System.Windows.Forms.Padding(4);
            this.btnFindArduino1.Name = "btnFindArduino1";
            this.btnFindArduino1.Size = new System.Drawing.Size(190, 29);
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
            this.btnStartStopBdcstListening.Location = new System.Drawing.Point(994, 4);
            this.btnStartStopBdcstListening.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartStopBdcstListening.Name = "btnStartStopBdcstListening";
            this.btnStartStopBdcstListening.Size = new System.Drawing.Size(275, 29);
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
            this.btnStartStopCollecting.Location = new System.Drawing.Point(4, 53);
            this.btnStartStopCollecting.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartStopCollecting.Name = "btnStartStopCollecting";
            this.btnStartStopCollecting.Size = new System.Drawing.Size(1180, 54);
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
            this.SearchingArduinoProcessCircle.Location = new System.Drawing.Point(466, 4);
            this.SearchingArduinoProcessCircle.Margin = new System.Windows.Forms.Padding(4);
            this.SearchingArduinoProcessCircle.Name = "SearchingArduinoProcessCircle";
            this.SearchingArduinoProcessCircle.NumberSpoke = 24;
            this.SearchingArduinoProcessCircle.OuterCircleRadius = 9;
            this.SearchingArduinoProcessCircle.RotationSpeed = 100;
            this.SearchingArduinoProcessCircle.Size = new System.Drawing.Size(41, 28);
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
            this.StartStopDataCollectingWaitingCircle.Location = new System.Drawing.Point(1192, 53);
            this.StartStopDataCollectingWaitingCircle.Margin = new System.Windows.Forms.Padding(4);
            this.StartStopDataCollectingWaitingCircle.Name = "StartStopDataCollectingWaitingCircle";
            this.StartStopDataCollectingWaitingCircle.NumberSpoke = 24;
            this.StartStopDataCollectingWaitingCircle.OuterCircleRadius = 9;
            this.StartStopDataCollectingWaitingCircle.RotationSpeed = 100;
            this.StartStopDataCollectingWaitingCircle.Size = new System.Drawing.Size(77, 54);
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
            this.btnCollectMostClose.Location = new System.Drawing.Point(4, 115);
            this.btnCollectMostClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnCollectMostClose.Name = "btnCollectMostClose";
            this.btnCollectMostClose.Size = new System.Drawing.Size(190, 32);
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
            this.btnCollectImmediately.Location = new System.Drawing.Point(4, 155);
            this.btnCollectImmediately.Margin = new System.Windows.Forms.Padding(4);
            this.btnCollectImmediately.Name = "btnCollectImmediately";
            this.btnCollectImmediately.Size = new System.Drawing.Size(190, 32);
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
            this.label3.Location = new System.Drawing.Point(202, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 40);
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
            this.label5.Location = new System.Drawing.Point(202, 151);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 40);
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
            this.lblNextShotIn.Location = new System.Drawing.Point(400, 111);
            this.lblNextShotIn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNextShotIn.Name = "lblNextShotIn";
            this.lblNextShotIn.Size = new System.Drawing.Size(190, 40);
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
            this.lblSinceLastShot.Location = new System.Drawing.Point(400, 151);
            this.lblSinceLastShot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSinceLastShot.Name = "lblSinceLastShot";
            this.lblSinceLastShot.Size = new System.Drawing.Size(190, 40);
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
            this.lblAccelerometerSign.Location = new System.Drawing.Point(664, 111);
            this.lblAccelerometerSign.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccelerometerSign.Name = "lblAccelerometerSign";
            this.tableLayoutPanel1.SetRowSpan(this.lblAccelerometerSign, 2);
            this.lblAccelerometerSign.Size = new System.Drawing.Size(190, 80);
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
            this.lblAccDevMeanMagnTitle.Location = new System.Drawing.Point(862, 111);
            this.lblAccDevMeanMagnTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccDevMeanMagnTitle.Name = "lblAccDevMeanMagnTitle";
            this.lblAccDevMeanMagnTitle.Size = new System.Drawing.Size(124, 40);
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
            this.lblAccDevMeanAngleTitle.Location = new System.Drawing.Point(994, 111);
            this.lblAccDevMeanAngleTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccDevMeanAngleTitle.Name = "lblAccDevMeanAngleTitle";
            this.lblAccDevMeanAngleTitle.Size = new System.Drawing.Size(124, 40);
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
            this.lblAccDevMeanMagnValue.Location = new System.Drawing.Point(862, 151);
            this.lblAccDevMeanMagnValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccDevMeanMagnValue.Name = "lblAccDevMeanMagnValue";
            this.lblAccDevMeanMagnValue.Size = new System.Drawing.Size(124, 40);
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
            this.lblAccDevMeanAngleValue.Location = new System.Drawing.Point(994, 151);
            this.lblAccDevMeanAngleValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccDevMeanAngleValue.Name = "lblAccDevMeanAngleValue";
            this.lblAccDevMeanAngleValue.Size = new System.Drawing.Size(124, 40);
            this.lblAccDevMeanAngleValue.TabIndex = 54;
            this.lblAccDevMeanAngleValue.Text = "---";
            this.lblAccDevMeanAngleValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbThumbPreview
            // 
            this.pbThumbPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pbThumbPreview, 9);
            this.pbThumbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbThumbPreview.Location = new System.Drawing.Point(4, 207);
            this.pbThumbPreview.Margin = new System.Windows.Forms.Padding(4);
            this.pbThumbPreview.Name = "pbThumbPreview";
            this.tableLayoutPanel1.SetRowSpan(this.pbThumbPreview, 13);
            this.pbThumbPreview.Size = new System.Drawing.Size(586, 446);
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
            this.lblLonTitle.Location = new System.Drawing.Point(1060, 369);
            this.lblLonTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLonTitle.Name = "lblLonTitle";
            this.lblLonTitle.Size = new System.Drawing.Size(209, 37);
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
            this.lblLonValue.Location = new System.Drawing.Point(1060, 406);
            this.lblLonValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLonValue.Name = "lblLonValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblLonValue, 2);
            this.lblLonValue.Size = new System.Drawing.Size(209, 80);
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
            this.lblLatTitle.Location = new System.Drawing.Point(862, 369);
            this.lblLatTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLatTitle.Name = "lblLatTitle";
            this.lblLatTitle.Size = new System.Drawing.Size(190, 37);
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
            this.lblLatValue.Location = new System.Drawing.Point(862, 406);
            this.lblLatValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLatValue.Name = "lblLatValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblLatValue, 2);
            this.lblLatValue.Size = new System.Drawing.Size(190, 80);
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
            this.lblGPStitle.Location = new System.Drawing.Point(664, 369);
            this.lblGPStitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPStitle.Name = "lblGPStitle";
            this.tableLayoutPanel1.SetRowSpan(this.lblGPStitle, 6);
            this.lblGPStitle.Size = new System.Drawing.Size(190, 234);
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
            this.label17.Location = new System.Drawing.Point(862, 486);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(407, 37);
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
            this.lblUTCTimeValue.Location = new System.Drawing.Point(862, 523);
            this.lblUTCTimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUTCTimeValue.Name = "lblUTCTimeValue";
            this.tableLayoutPanel1.SetRowSpan(this.lblUTCTimeValue, 2);
            this.lblUTCTimeValue.Size = new System.Drawing.Size(407, 80);
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
            this.lblPressureTitle.Location = new System.Drawing.Point(664, 615);
            this.lblPressureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPressureTitle.Name = "lblPressureTitle";
            this.lblPressureTitle.Size = new System.Drawing.Size(190, 42);
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
            this.lblPressureValue.Location = new System.Drawing.Point(862, 615);
            this.lblPressureValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPressureValue.Name = "lblPressureValue";
            this.lblPressureValue.Size = new System.Drawing.Size(407, 42);
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
            this.processCircle.Location = new System.Drawing.Point(1126, 115);
            this.processCircle.Margin = new System.Windows.Forms.Padding(4);
            this.processCircle.Name = "processCircle";
            this.processCircle.NumberSpoke = 12;
            this.processCircle.OuterCircleRadius = 11;
            this.processCircle.RotationSpeed = 100;
            this.tableLayoutPanel1.SetRowSpan(this.processCircle, 2);
            this.processCircle.Size = new System.Drawing.Size(143, 72);
            this.processCircle.SpokeThickness = 2;
            this.processCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.processCircle.TabIndex = 95;
            this.processCircle.Visible = false;
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
            this.tableLayoutPanel4.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 10;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1273, 657);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // camshotPeriodLabel
            // 
            this.camshotPeriodLabel.AutoSize = true;
            this.camshotPeriodLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camshotPeriodLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camshotPeriodLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camshotPeriodLabel.Location = new System.Drawing.Point(4, 0);
            this.camshotPeriodLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.camshotPeriodLabel.Name = "camshotPeriodLabel";
            this.camshotPeriodLabel.Size = new System.Drawing.Size(295, 49);
            this.camshotPeriodLabel.TabIndex = 72;
            this.camshotPeriodLabel.Text = "camera shooting period:";
            this.camshotPeriodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamShotPeriod
            // 
            this.tbCamShotPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamShotPeriod.Location = new System.Drawing.Point(307, 4);
            this.tbCamShotPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.tbCamShotPeriod.Mask = "00:00:00";
            this.tbCamShotPeriod.Name = "tbCamShotPeriod";
            this.tbCamShotPeriod.Size = new System.Drawing.Size(295, 23);
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
            this.camshotPeriodDataSavingCircle.Size = new System.Drawing.Size(53, 41);
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
            this.camIPLabel.Location = new System.Drawing.Point(4, 49);
            this.camIPLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.camIPLabel.Name = "camIPLabel";
            this.camIPLabel.Size = new System.Drawing.Size(295, 49);
            this.camIPLabel.TabIndex = 79;
            this.camIPLabel.Text = "camera IP";
            this.camIPLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamIP
            // 
            this.tbCamIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamIP.Location = new System.Drawing.Point(307, 53);
            this.tbCamIP.Margin = new System.Windows.Forms.Padding(4);
            this.tbCamIP.Mask = "000.000.000.000";
            this.tbCamIP.Name = "tbCamIP";
            this.tbCamIP.Size = new System.Drawing.Size(295, 23);
            this.tbCamIP.TabIndex = 80;
            this.tbCamIP.TextChanged += new System.EventHandler(this.tbCamIP_TextChanged);
            // 
            // ipAddrValidatingCircle
            // 
            this.ipAddrValidatingCircle.Active = false;
            this.ipAddrValidatingCircle.Color = System.Drawing.Color.DarkGray;
            this.ipAddrValidatingCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ipAddrValidatingCircle.InnerCircleRadius = 8;
            this.ipAddrValidatingCircle.Location = new System.Drawing.Point(1216, 53);
            this.ipAddrValidatingCircle.Margin = new System.Windows.Forms.Padding(4);
            this.ipAddrValidatingCircle.Name = "ipAddrValidatingCircle";
            this.ipAddrValidatingCircle.NumberSpoke = 24;
            this.ipAddrValidatingCircle.OuterCircleRadius = 9;
            this.ipAddrValidatingCircle.RotationSpeed = 100;
            this.ipAddrValidatingCircle.Size = new System.Drawing.Size(53, 41);
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
            this.camUNameLabel.Location = new System.Drawing.Point(4, 98);
            this.camUNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.camUNameLabel.Name = "camUNameLabel";
            this.camUNameLabel.Size = new System.Drawing.Size(295, 49);
            this.camUNameLabel.TabIndex = 82;
            this.camUNameLabel.Text = "camera username";
            this.camUNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamUName
            // 
            this.tbCamUName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamUName.Location = new System.Drawing.Point(307, 102);
            this.tbCamUName.Margin = new System.Windows.Forms.Padding(4);
            this.tbCamUName.Name = "tbCamUName";
            this.tbCamUName.Size = new System.Drawing.Size(295, 23);
            this.tbCamUName.TabIndex = 83;
            this.tbCamUName.TextChanged += new System.EventHandler(this.tbCamUName_TextChanged);
            // 
            // camPWDLabel
            // 
            this.camPWDLabel.AutoSize = true;
            this.camPWDLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.camPWDLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camPWDLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camPWDLabel.Location = new System.Drawing.Point(4, 147);
            this.camPWDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.camPWDLabel.Name = "camPWDLabel";
            this.camPWDLabel.Size = new System.Drawing.Size(295, 49);
            this.camPWDLabel.TabIndex = 84;
            this.camPWDLabel.Text = "camera password";
            this.camPWDLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCamPWD
            // 
            this.tbCamPWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCamPWD.Location = new System.Drawing.Point(307, 151);
            this.tbCamPWD.Margin = new System.Windows.Forms.Padding(4);
            this.tbCamPWD.Name = "tbCamPWD";
            this.tbCamPWD.Size = new System.Drawing.Size(295, 23);
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
        private System.ComponentModel.BackgroundWorker magnCalibrator;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnFindArduino1;
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
        private System.ComponentModel.BackgroundWorker bgwUDPmessagesParser;
        private MRG.Controls.UI.LoadingCircle processCircle;
        private System.Windows.Forms.Button btnStartStopBdcstListening;


    }
}

