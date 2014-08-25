namespace SkyIndexAnalyzerSolo
{
    partial class SkyIndexAnalyzerDataCollector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);


        //    if (m_ip != IntPtr.Zero)
        //    {
        //        Marshal.FreeCoTaskMem(m_ip);
        //        m_ip = IntPtr.Zero;
        //    }
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkyIndexAnalyzerDataCollector));
            this.DataCollectorNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.GPSDataCollectingCycle = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbCollectCompass = new System.Windows.Forms.CheckBox();
            this.cbCollectGPS = new System.Windows.Forms.CheckBox();
            this.cbCollectAccelerometer = new System.Windows.Forms.CheckBox();
            this.cbGetPhotosWebCams = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbGetPhotosIPCamPeriod = new System.Windows.Forms.NumericUpDown();
            this.cbGetPhotosWebCamsPeriod = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.cbGetPhotosIPCam = new System.Windows.Forms.CheckBox();
            this.cbGetUDPdataFromIOSdevice = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.GetSensorsDataCycle = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbGetPhotosIPCamPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbGetPhotosWebCamsPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // DataCollectorNotifyIcon
            // 
            this.DataCollectorNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("DataCollectorNotifyIcon.Icon")));
            this.DataCollectorNotifyIcon.Text = "SkyIndex Analyzer data collector";
            this.DataCollectorNotifyIcon.Visible = true;
            this.DataCollectorNotifyIcon.BalloonTipClicked += new System.EventHandler(this.DataCollectorNotifyIcon_BalloonTipClicked);
            this.DataCollectorNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataCollectorNotifyIcon_MouseClick);
            // 
            // GPSDataCollectingCycle
            // 
            this.GPSDataCollectingCycle.WorkerReportsProgress = true;
            this.GPSDataCollectingCycle.WorkerSupportsCancellation = true;
            this.GPSDataCollectingCycle.DoWork += new System.ComponentModel.DoWorkEventHandler(this.GPSDataCollectingCycle_DoWork);
            this.GPSDataCollectingCycle.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.GPSDataCollectingCycle_ProgressChanged);
            this.GPSDataCollectingCycle.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.GPSDataCollectingCycle_RunWorkerCompleted);
            // 
            // button1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.button1, 2);
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(563, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 37);
            this.button1.TabIndex = 9;
            this.button1.Text = "Подключиться к IP-камере";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 6);
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(339, 218);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.tableLayoutPanel1.SetRowSpan(this.textBox1, 4);
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(392, 166);
            this.textBox1.TabIndex = 4;
            // 
            // cbCollectCompass
            // 
            this.cbCollectCompass.BackColor = System.Drawing.Color.MistyRose;
            this.cbCollectCompass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCollectCompass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCollectCompass.Location = new System.Drawing.Point(0, 0);
            this.cbCollectCompass.Margin = new System.Windows.Forms.Padding(0);
            this.cbCollectCompass.Name = "cbCollectCompass";
            this.cbCollectCompass.Size = new System.Drawing.Size(443, 20);
            this.cbCollectCompass.TabIndex = 1;
            this.cbCollectCompass.Text = "Накапливать данные цифрового компаса";
            this.cbCollectCompass.UseVisualStyleBackColor = false;
            this.cbCollectCompass.CheckedChanged += new System.EventHandler(this.cbCollectCompass_CheckedChanged);
            // 
            // cbCollectGPS
            // 
            this.cbCollectGPS.BackColor = System.Drawing.Color.MistyRose;
            this.cbCollectGPS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCollectGPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCollectGPS.Location = new System.Drawing.Point(0, 20);
            this.cbCollectGPS.Margin = new System.Windows.Forms.Padding(0);
            this.cbCollectGPS.Name = "cbCollectGPS";
            this.cbCollectGPS.Size = new System.Drawing.Size(443, 20);
            this.cbCollectGPS.TabIndex = 0;
            this.cbCollectGPS.Text = "Собирать данные GPS";
            this.cbCollectGPS.UseVisualStyleBackColor = false;
            this.cbCollectGPS.CheckedChanged += new System.EventHandler(this.cbCollectGPS_CheckedChanged);
            // 
            // cbCollectAccelerometer
            // 
            this.cbCollectAccelerometer.BackColor = System.Drawing.Color.MistyRose;
            this.cbCollectAccelerometer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCollectAccelerometer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCollectAccelerometer.Location = new System.Drawing.Point(0, 40);
            this.cbCollectAccelerometer.Margin = new System.Windows.Forms.Padding(0);
            this.cbCollectAccelerometer.Name = "cbCollectAccelerometer";
            this.cbCollectAccelerometer.Size = new System.Drawing.Size(443, 20);
            this.cbCollectAccelerometer.TabIndex = 2;
            this.cbCollectAccelerometer.Text = "Накапливать данные акселерометра";
            this.cbCollectAccelerometer.UseVisualStyleBackColor = false;
            this.cbCollectAccelerometer.CheckedChanged += new System.EventHandler(this.cbCollectAccelerometer_CheckedChanged);
            // 
            // cbGetPhotosWebCams
            // 
            this.cbGetPhotosWebCams.BackColor = System.Drawing.Color.MistyRose;
            this.cbGetPhotosWebCams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbGetPhotosWebCams.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbGetPhotosWebCams.Location = new System.Drawing.Point(0, 60);
            this.cbGetPhotosWebCams.Margin = new System.Windows.Forms.Padding(0);
            this.cbGetPhotosWebCams.Name = "cbGetPhotosWebCams";
            this.cbGetPhotosWebCams.Size = new System.Drawing.Size(443, 20);
            this.cbGetPhotosWebCams.TabIndex = 3;
            this.cbGetPhotosWebCams.Text = "Производить фотосъемку вебкамерами. Период (c):";
            this.cbGetPhotosWebCams.UseVisualStyleBackColor = false;
            this.cbGetPhotosWebCams.CheckedChanged += new System.EventHandler(this.cbGetPhotos_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 12;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.21118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.472051F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.472051F));
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.button1, 10, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 387);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 10);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.1444F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.8556F));
            this.tableLayoutPanel2.Controls.Add(this.cbGetPhotosIPCamPeriod, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.cbCollectCompass, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbGetPhotosWebCamsPeriod, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.button2, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.cbCollectAccelerometer, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cbGetPhotosIPCam, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.cbCollectGPS, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbGetPhotosWebCams, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.cbGetUDPdataFromIOSdevice, 0, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel2, 5);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(554, 209);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // cbGetPhotosIPCamPeriod
            // 
            this.cbGetPhotosIPCamPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbGetPhotosIPCamPeriod.Location = new System.Drawing.Point(446, 83);
            this.cbGetPhotosIPCamPeriod.Name = "cbGetPhotosIPCamPeriod";
            this.cbGetPhotosIPCamPeriod.Size = new System.Drawing.Size(105, 20);
            this.cbGetPhotosIPCamPeriod.TabIndex = 6;
            // 
            // cbGetPhotosWebCamsPeriod
            // 
            this.cbGetPhotosWebCamsPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbGetPhotosWebCamsPeriod.Location = new System.Drawing.Point(446, 63);
            this.cbGetPhotosWebCamsPeriod.Name = "cbGetPhotosWebCamsPeriod";
            this.cbGetPhotosWebCamsPeriod.Size = new System.Drawing.Size(105, 20);
            this.cbGetPhotosWebCamsPeriod.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.button2, 2);
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(3, 123);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(548, 83);
            this.button2.TabIndex = 6;
            this.button2.Text = "Начать сбор данных";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbGetPhotosIPCam
            // 
            this.cbGetPhotosIPCam.BackColor = System.Drawing.Color.MistyRose;
            this.cbGetPhotosIPCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbGetPhotosIPCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbGetPhotosIPCam.Location = new System.Drawing.Point(0, 80);
            this.cbGetPhotosIPCam.Margin = new System.Windows.Forms.Padding(0);
            this.cbGetPhotosIPCam.Name = "cbGetPhotosIPCam";
            this.cbGetPhotosIPCam.Size = new System.Drawing.Size(443, 20);
            this.cbGetPhotosIPCam.TabIndex = 4;
            this.cbGetPhotosIPCam.Text = "Производить фотосъемку IP-камерой. Период (c):";
            this.cbGetPhotosIPCam.UseVisualStyleBackColor = false;
            // 
            // cbGetUDPdataFromIOSdevice
            // 
            this.cbGetUDPdataFromIOSdevice.BackColor = System.Drawing.Color.MistyRose;
            this.cbGetUDPdataFromIOSdevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbGetUDPdataFromIOSdevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbGetUDPdataFromIOSdevice.Location = new System.Drawing.Point(0, 100);
            this.cbGetUDPdataFromIOSdevice.Margin = new System.Windows.Forms.Padding(0);
            this.cbGetUDPdataFromIOSdevice.Name = "cbGetUDPdataFromIOSdevice";
            this.cbGetUDPdataFromIOSdevice.Size = new System.Drawing.Size(443, 20);
            this.cbGetUDPdataFromIOSdevice.TabIndex = 7;
            this.cbGetUDPdataFromIOSdevice.Text = "Собирать данные сенсоров IOS-устройства по сети";
            this.cbGetUDPdataFromIOSdevice.UseVisualStyleBackColor = false;
            // 
            // textBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox2, 6);
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(3, 218);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.tableLayoutPanel1.SetRowSpan(this.textBox2, 4);
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(330, 166);
            this.textBox2.TabIndex = 12;
            // 
            // GetSensorsDataCycle
            // 
            this.GetSensorsDataCycle.WorkerReportsProgress = true;
            this.GetSensorsDataCycle.WorkerSupportsCancellation = true;
            this.GetSensorsDataCycle.DoWork += new System.ComponentModel.DoWorkEventHandler(this.GetSensorsDataCycle_DoWork);
            this.GetSensorsDataCycle.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.GetSensorsDataCycle_ProgressChanged);
            this.GetSensorsDataCycle.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.GetSensorsDataCycle_RunWorkerCompleted);
            // 
            // SkyIndexAnalyzerDataCollector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 387);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SkyIndexAnalyzerDataCollector";
            this.Text = "SkyIndex analyzer. Data collector";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SkyIndexAnalyzerDataCollector_FormClosed);
            this.Load += new System.EventHandler(this.SkyIndexAnalyzerDataCollector_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SkyIndexAnalyzerDataCollector_Paint);
            this.Resize += new System.EventHandler(this.SkyIndexAnalyzerDataCollector_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbGetPhotosIPCamPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbGetPhotosWebCamsPeriod)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon DataCollectorNotifyIcon;
        private System.ComponentModel.BackgroundWorker GPSDataCollectingCycle;
        //private AxVITAMINDECODERLib.AxVitaminCtrl axVitaminCtrl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbCollectGPS;
        private System.Windows.Forms.CheckBox cbCollectCompass;
        private System.Windows.Forms.CheckBox cbCollectAccelerometer;
        private System.Windows.Forms.CheckBox cbGetPhotosWebCams;
        private System.Windows.Forms.CheckBox cbGetPhotosIPCam;
        private System.Windows.Forms.NumericUpDown cbGetPhotosWebCamsPeriod;
        private System.Windows.Forms.NumericUpDown cbGetPhotosIPCamPeriod;
        private System.Windows.Forms.Button button2;
        public System.ComponentModel.BackgroundWorker GetSensorsDataCycle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox cbGetUDPdataFromIOSdevice;
        private System.Windows.Forms.TextBox textBox2;
    }
}