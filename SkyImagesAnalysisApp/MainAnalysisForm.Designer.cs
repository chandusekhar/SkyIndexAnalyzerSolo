namespace SkyImagesAnalyzer
{
    partial class MainAnalysisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainAnalysisForm));
            this.bgwProcessOneImage = new System.ComponentModel.BackgroundWorker();
            this.pbUniversalProgressBar = new System.Windows.Forms.ProgressBar();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchCollectingDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиСбораДанныхToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.открытьФайлToolStripMenuItem = new System.Windows.Forms.Button();
            this.обработатьToolStripMenuItem = new System.Windows.Forms.Button();
            this.DetectEdgesButton = new System.Windows.Forms.Button();
            this.lblClassificationMethod = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnTest1 = new System.Windows.Forms.Button();
            this.btnProcessDirectorySI = new System.Windows.Forms.Button();
            this.btnShowMedianPerc5Diagram = new System.Windows.Forms.Button();
            this.btnDensityProcessing = new System.Windows.Forms.Button();
            this.btnSortImagesByClasses = new System.Windows.Forms.Button();
            this.btnTestSunDetection2015 = new System.Windows.Forms.Button();
            this.lblResultTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnSaveClustering = new System.Windows.Forms.RadioButton();
            this.rbtnClusterizePoints = new System.Windows.Forms.RadioButton();
            this.rbtnShowDensity = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnClassMethodGrIx = new System.Windows.Forms.RadioButton();
            this.rbtnClassMethodJapan = new System.Windows.Forms.RadioButton();
            this.rbtnClassMethodUS = new System.Windows.Forms.RadioButton();
            this.btnCalcSunPosition = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.bgwProcessDirectoryOfImages = new System.ComponentModel.BackgroundWorker();
            this.bgwSunDetectionOnly = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgwProcessOneImage
            // 
            this.bgwProcessOneImage.WorkerReportsProgress = true;
            this.bgwProcessOneImage.WorkerSupportsCancellation = true;
            this.bgwProcessOneImage.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.bgwProcessOneImage.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.bgwProcessOneImage.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // pbUniversalProgressBar
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbUniversalProgressBar, 15);
            this.pbUniversalProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbUniversalProgressBar.Location = new System.Drawing.Point(4, 655);
            this.pbUniversalProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.pbUniversalProgressBar.Name = "pbUniversalProgressBar";
            this.pbUniversalProgressBar.Size = new System.Drawing.Size(1400, 24);
            this.pbUniversalProgressBar.TabIndex = 25;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.StatusLabel, 15);
            this.StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusLabel.Location = new System.Drawing.Point(4, 620);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(4, 1, 4, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(1400, 31);
            this.StatusLabel.TabIndex = 24;
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 4);
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(4, 418);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(364, 32);
            this.label5.TabIndex = 22;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(1120, 454);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(85, 44);
            this.button4.TabIndex = 20;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // trackBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.trackBar1, 8);
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar1.Location = new System.Drawing.Point(376, 454);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(736, 44);
            this.trackBar1.TabIndex = 18;
            this.trackBar1.Value = 10;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 450);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 52);
            this.label1.TabIndex = 14;
            this.label1.Text = "Подстройка (SI):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1213, 450);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 52);
            this.label2.TabIndex = 15;
            this.label2.Text = "0.10";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox2, 4);
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(1027, 134);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox2, 9);
            this.pictureBox2.Size = new System.Drawing.Size(377, 280);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 4);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(4, 134);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox1, 9);
            this.pictureBox1.Size = new System.Drawing.Size(364, 280);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.помощьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(68, 31);
            this.помощьToolStripMenuItem.Text = "Помощь";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(173, 24);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // gPSToolStripMenuItem
            // 
            this.gPSToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getDataToolStripMenuItem,
            this.switchCollectingDataMenuItem,
            this.настройкиСбораДанныхToolStripMenuItem});
            this.gPSToolStripMenuItem.Name = "gPSToolStripMenuItem";
            this.gPSToolStripMenuItem.Size = new System.Drawing.Size(91, 31);
            this.gPSToolStripMenuItem.Text = "Сбор данных";
            // 
            // getDataToolStripMenuItem
            // 
            this.getDataToolStripMenuItem.Name = "getDataToolStripMenuItem";
            this.getDataToolStripMenuItem.Size = new System.Drawing.Size(322, 24);
            this.getDataToolStripMenuItem.Text = "Получить данные GPS";
            // 
            // switchCollectingDataMenuItem
            // 
            this.switchCollectingDataMenuItem.Name = "switchCollectingDataMenuItem";
            this.switchCollectingDataMenuItem.Size = new System.Drawing.Size(322, 24);
            this.switchCollectingDataMenuItem.Text = "Начать сбор данных всех датчиков";
            // 
            // настройкиСбораДанныхToolStripMenuItem
            // 
            this.настройкиСбораДанныхToolStripMenuItem.Name = "настройкиСбораДанныхToolStripMenuItem";
            this.настройкиСбораДанныхToolStripMenuItem.Size = new System.Drawing.Size(322, 24);
            this.настройкиСбораДанныхToolStripMenuItem.Text = "Сбор данных";
            this.настройкиСбораДанныхToolStripMenuItem.Click += new System.EventHandler(this.настройкиСбораДанныхToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 15;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666665F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666666F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 11, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 13, 12);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.trackBar1, 4, 12);
            this.tableLayoutPanel1.Controls.Add(this.button4, 12, 12);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.StatusLabel, 0, 16);
            this.tableLayoutPanel1.Controls.Add(this.pbUniversalProgressBar, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.label8, 4, 11);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 19);
            this.tableLayoutPanel1.Controls.Add(this.открытьФайлToolStripMenuItem, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.обработатьToolStripMenuItem, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.DetectEdgesButton, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblClassificationMethod, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 0, 18);
            this.tableLayoutPanel1.Controls.Add(this.button2, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 13, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAbout, 14, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTest1, 4, 19);
            this.tableLayoutPanel1.Controls.Add(this.btnProcessDirectorySI, 10, 19);
            this.tableLayoutPanel1.Controls.Add(this.btnShowMedianPerc5Diagram, 7, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnDensityProcessing, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnSortImagesByClasses, 4, 21);
            this.tableLayoutPanel1.Controls.Add(this.btnTestSunDetection2015, 4, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblResultTitle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 8, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 4, 13);
            this.tableLayoutPanel1.Controls.Add(this.btnCalcSunPosition, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.button5, 9, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 22;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.681819F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1408, 826);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label8, 11);
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(376, 418);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1028, 32);
            this.label8.TabIndex = 30;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.button1, 4);
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(0, 715);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.tableLayoutPanel1.SetRowSpan(this.button1, 3);
            this.button1.Size = new System.Drawing.Size(372, 111);
            this.button1.TabIndex = 32;
            this.button1.Text = "Обработка директории: (cloud cover)";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // открытьФайлToolStripMenuItem
            // 
            this.открытьФайлToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.открытьФайлToolStripMenuItem, 2);
            this.открытьФайлToolStripMenuItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.открытьФайлToolStripMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.открытьФайлToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.открытьФайлToolStripMenuItem.Location = new System.Drawing.Point(4, 4);
            this.открытьФайлToolStripMenuItem.Margin = new System.Windows.Forms.Padding(4);
            this.открытьФайлToolStripMenuItem.Name = "открытьФайлToolStripMenuItem";
            this.открытьФайлToolStripMenuItem.Size = new System.Drawing.Size(178, 72);
            this.открытьФайлToolStripMenuItem.TabIndex = 34;
            this.открытьФайлToolStripMenuItem.Text = "Открыть файл";
            this.открытьФайлToolStripMenuItem.UseVisualStyleBackColor = false;
            this.открытьФайлToolStripMenuItem.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // обработатьToolStripMenuItem
            // 
            this.обработатьToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.обработатьToolStripMenuItem, 2);
            this.обработатьToolStripMenuItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.обработатьToolStripMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.обработатьToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.обработатьToolStripMenuItem.Location = new System.Drawing.Point(190, 4);
            this.обработатьToolStripMenuItem.Margin = new System.Windows.Forms.Padding(4);
            this.обработатьToolStripMenuItem.Name = "обработатьToolStripMenuItem";
            this.обработатьToolStripMenuItem.Size = new System.Drawing.Size(178, 72);
            this.обработатьToolStripMenuItem.TabIndex = 35;
            this.обработатьToolStripMenuItem.Text = "Обработать";
            this.обработатьToolStripMenuItem.UseVisualStyleBackColor = false;
            this.обработатьToolStripMenuItem.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // DetectEdgesButton
            // 
            this.DetectEdgesButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.DetectEdgesButton, 2);
            this.DetectEdgesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetectEdgesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DetectEdgesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DetectEdgesButton.Location = new System.Drawing.Point(376, 4);
            this.DetectEdgesButton.Margin = new System.Windows.Forms.Padding(4);
            this.DetectEdgesButton.Name = "DetectEdgesButton";
            this.DetectEdgesButton.Size = new System.Drawing.Size(178, 72);
            this.DetectEdgesButton.TabIndex = 43;
            this.DetectEdgesButton.Text = "Разметка по октам";
            this.DetectEdgesButton.UseVisualStyleBackColor = false;
            this.DetectEdgesButton.Click += new System.EventHandler(this.DetectEdgesButton_Click);
            // 
            // lblClassificationMethod
            // 
            this.lblClassificationMethod.AutoSize = true;
            this.lblClassificationMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblClassificationMethod, 4);
            this.lblClassificationMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassificationMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblClassificationMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClassificationMethod.Location = new System.Drawing.Point(4, 502);
            this.lblClassificationMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClassificationMethod.Name = "lblClassificationMethod";
            this.tableLayoutPanel1.SetRowSpan(this.lblClassificationMethod, 3);
            this.lblClassificationMethod.Size = new System.Drawing.Size(364, 117);
            this.lblClassificationMethod.TabIndex = 45;
            this.lblClassificationMethod.Text = "Classification method:";
            this.lblClassificationMethod.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBox1, 15);
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(4, 687);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Multiline = false;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1400, 24);
            this.richTextBox1.TabIndex = 48;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.button2, 3);
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(934, 4);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(271, 72);
            this.button2.TabIndex = 49;
            this.button2.Text = "Ручной анализ цветности";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Image = global::SkyImagesAnalyzer.Properties.Resources.process;
            this.btnProperties.Location = new System.Drawing.Point(1213, 4);
            this.btnProperties.Margin = new System.Windows.Forms.Padding(4);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(85, 72);
            this.btnProperties.TabIndex = 52;
            this.btnProperties.UseVisualStyleBackColor = false;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.button3, 3);
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(376, 134);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.tableLayoutPanel1.SetRowSpan(this.button3, 2);
            this.button3.Size = new System.Drawing.Size(271, 56);
            this.button3.TabIndex = 53;
            this.button3.Text = "GrIx histogram";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Image = global::SkyImagesAnalyzer.Properties.Resources.info;
            this.btnAbout.Location = new System.Drawing.Point(1306, 4);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(4);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(98, 72);
            this.btnAbout.TabIndex = 57;
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnTest1
            // 
            this.btnTest1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnTest1, 6);
            this.btnTest1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest1.Location = new System.Drawing.Point(372, 715);
            this.btnTest1.Margin = new System.Windows.Forms.Padding(0);
            this.btnTest1.Name = "btnTest1";
            this.tableLayoutPanel1.SetRowSpan(this.btnTest1, 2);
            this.btnTest1.Size = new System.Drawing.Size(558, 64);
            this.btnTest1.TabIndex = 59;
            this.btnTest1.Text = "Collect statistics (5perc and Median valus over images set)";
            this.btnTest1.UseVisualStyleBackColor = false;
            this.btnTest1.Click += new System.EventHandler(this.btnTest1_Click);
            // 
            // btnProcessDirectorySI
            // 
            this.btnProcessDirectorySI.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnProcessDirectorySI, 5);
            this.btnProcessDirectorySI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProcessDirectorySI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcessDirectorySI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessDirectorySI.Location = new System.Drawing.Point(930, 715);
            this.btnProcessDirectorySI.Margin = new System.Windows.Forms.Padding(0);
            this.btnProcessDirectorySI.Name = "btnProcessDirectorySI";
            this.tableLayoutPanel1.SetRowSpan(this.btnProcessDirectorySI, 3);
            this.btnProcessDirectorySI.Size = new System.Drawing.Size(478, 111);
            this.btnProcessDirectorySI.TabIndex = 60;
            this.btnProcessDirectorySI.Text = "Обработка директории (только SkyIndex)";
            this.btnProcessDirectorySI.UseVisualStyleBackColor = false;
            this.btnProcessDirectorySI.Click += new System.EventHandler(this.btnProcessDirectorySI_Click);
            // 
            // btnShowMedianPerc5Diagram
            // 
            this.btnShowMedianPerc5Diagram.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnShowMedianPerc5Diagram, 4);
            this.btnShowMedianPerc5Diagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowMedianPerc5Diagram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowMedianPerc5Diagram.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowMedianPerc5Diagram.Location = new System.Drawing.Point(654, 132);
            this.btnShowMedianPerc5Diagram.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnShowMedianPerc5Diagram.Name = "btnShowMedianPerc5Diagram";
            this.tableLayoutPanel1.SetRowSpan(this.btnShowMedianPerc5Diagram, 2);
            this.btnShowMedianPerc5Diagram.Size = new System.Drawing.Size(366, 60);
            this.btnShowMedianPerc5Diagram.TabIndex = 61;
            this.btnShowMedianPerc5Diagram.Text = "Show at median-5perc diagram";
            this.btnShowMedianPerc5Diagram.UseVisualStyleBackColor = false;
            this.btnShowMedianPerc5Diagram.Click += new System.EventHandler(this.btnShowMedianPerc5Diagram_Click);
            // 
            // btnDensityProcessing
            // 
            this.btnDensityProcessing.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnDensityProcessing, 4);
            this.btnDensityProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDensityProcessing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDensityProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDensityProcessing.Location = new System.Drawing.Point(375, 196);
            this.btnDensityProcessing.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDensityProcessing.Name = "btnDensityProcessing";
            this.tableLayoutPanel1.SetRowSpan(this.btnDensityProcessing, 4);
            this.btnDensityProcessing.Size = new System.Drawing.Size(366, 124);
            this.btnDensityProcessing.TabIndex = 62;
            this.btnDensityProcessing.Text = "Show density (m;p5),\r\nclusterize points\r\nsave clusters data\r\n";
            this.btnDensityProcessing.UseVisualStyleBackColor = false;
            this.btnDensityProcessing.Click += new System.EventHandler(this.btnDensityProcessing_Click);
            // 
            // btnSortImagesByClasses
            // 
            this.btnSortImagesByClasses.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnSortImagesByClasses, 6);
            this.btnSortImagesByClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSortImagesByClasses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSortImagesByClasses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSortImagesByClasses.Location = new System.Drawing.Point(375, 781);
            this.btnSortImagesByClasses.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSortImagesByClasses.Name = "btnSortImagesByClasses";
            this.btnSortImagesByClasses.Size = new System.Drawing.Size(552, 43);
            this.btnSortImagesByClasses.TabIndex = 63;
            this.btnSortImagesByClasses.Text = "Sort images by classes";
            this.btnSortImagesByClasses.UseVisualStyleBackColor = false;
            this.btnSortImagesByClasses.Click += new System.EventHandler(this.btnSortImagesByClasses_Click);
            // 
            // btnTestSunDetection2015
            // 
            this.btnTestSunDetection2015.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnTestSunDetection2015, 7);
            this.btnTestSunDetection2015.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTestSunDetection2015.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestSunDetection2015.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestSunDetection2015.Location = new System.Drawing.Point(375, 324);
            this.btnTestSunDetection2015.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTestSunDetection2015.Name = "btnTestSunDetection2015";
            this.tableLayoutPanel1.SetRowSpan(this.btnTestSunDetection2015, 3);
            this.btnTestSunDetection2015.Size = new System.Drawing.Size(645, 92);
            this.btnTestSunDetection2015.TabIndex = 64;
            this.btnTestSunDetection2015.Text = "sun detection jan`2015\r\n(пока не работает)";
            this.btnTestSunDetection2015.UseVisualStyleBackColor = false;
            this.btnTestSunDetection2015.Click += new System.EventHandler(this.btnTestSunDetection2015_Click);
            // 
            // lblResultTitle
            // 
            this.lblResultTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblResultTitle, 4);
            this.lblResultTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResultTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultTitle.Location = new System.Drawing.Point(3, 80);
            this.lblResultTitle.Name = "lblResultTitle";
            this.lblResultTitle.Size = new System.Drawing.Size(366, 50);
            this.lblResultTitle.TabIndex = 65;
            this.lblResultTitle.Text = "source image";
            this.lblResultTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 3);
            this.groupBox1.Controls.Add(this.rbtnSaveClustering);
            this.groupBox1.Controls.Add(this.rbtnClusterizePoints);
            this.groupBox1.Controls.Add(this.rbtnShowDensity);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(747, 196);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 4);
            this.groupBox1.Size = new System.Drawing.Size(273, 124);
            this.groupBox1.TabIndex = 66;
            this.groupBox1.TabStop = false;
            // 
            // rbtnSaveClustering
            // 
            this.rbtnSaveClustering.AutoSize = true;
            this.rbtnSaveClustering.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnSaveClustering.Location = new System.Drawing.Point(5, 87);
            this.rbtnSaveClustering.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtnSaveClustering.Name = "rbtnSaveClustering";
            this.rbtnSaveClustering.Size = new System.Drawing.Size(240, 33);
            this.rbtnSaveClustering.TabIndex = 2;
            this.rbtnSaveClustering.Text = "and save clustering";
            this.rbtnSaveClustering.UseVisualStyleBackColor = true;
            // 
            // rbtnClusterizePoints
            // 
            this.rbtnClusterizePoints.AutoSize = true;
            this.rbtnClusterizePoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnClusterizePoints.Location = new System.Drawing.Point(5, 48);
            this.rbtnClusterizePoints.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtnClusterizePoints.Name = "rbtnClusterizePoints";
            this.rbtnClusterizePoints.Size = new System.Drawing.Size(182, 33);
            this.rbtnClusterizePoints.TabIndex = 1;
            this.rbtnClusterizePoints.Text = "and clusterize";
            this.rbtnClusterizePoints.UseVisualStyleBackColor = true;
            // 
            // rbtnShowDensity
            // 
            this.rbtnShowDensity.AutoSize = true;
            this.rbtnShowDensity.Checked = true;
            this.rbtnShowDensity.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnShowDensity.Location = new System.Drawing.Point(5, 9);
            this.rbtnShowDensity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtnShowDensity.Name = "rbtnShowDensity";
            this.rbtnShowDensity.Size = new System.Drawing.Size(185, 33);
            this.rbtnShowDensity.TabIndex = 0;
            this.rbtnShowDensity.TabStop = true;
            this.rbtnShowDensity.Text = "show diagram";
            this.rbtnShowDensity.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 3);
            this.groupBox2.Controls.Add(this.rbtnClassMethodGrIx);
            this.groupBox2.Controls.Add(this.rbtnClassMethodJapan);
            this.groupBox2.Controls.Add(this.rbtnClassMethodUS);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(375, 504);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox2, 3);
            this.groupBox2.Size = new System.Drawing.Size(273, 113);
            this.groupBox2.TabIndex = 67;
            this.groupBox2.TabStop = false;
            // 
            // rbtnClassMethodGrIx
            // 
            this.rbtnClassMethodGrIx.AutoSize = true;
            this.rbtnClassMethodGrIx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnClassMethodGrIx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnClassMethodGrIx.Location = new System.Drawing.Point(7, 81);
            this.rbtnClassMethodGrIx.Margin = new System.Windows.Forms.Padding(4);
            this.rbtnClassMethodGrIx.Name = "rbtnClassMethodGrIx";
            this.rbtnClassMethodGrIx.Size = new System.Drawing.Size(117, 29);
            this.rbtnClassMethodGrIx.TabIndex = 50;
            this.rbtnClassMethodGrIx.Text = "GrIx SAIL";
            this.rbtnClassMethodGrIx.UseVisualStyleBackColor = true;
            this.rbtnClassMethodGrIx.CheckedChanged += new System.EventHandler(this.rbtnClassMethodNew_CheckedChanged);
            // 
            // rbtnClassMethodJapan
            // 
            this.rbtnClassMethodJapan.AutoSize = true;
            this.rbtnClassMethodJapan.Checked = true;
            this.rbtnClassMethodJapan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnClassMethodJapan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnClassMethodJapan.Location = new System.Drawing.Point(7, 1);
            this.rbtnClassMethodJapan.Margin = new System.Windows.Forms.Padding(4);
            this.rbtnClassMethodJapan.Name = "rbtnClassMethodJapan";
            this.rbtnClassMethodJapan.Size = new System.Drawing.Size(199, 29);
            this.rbtnClassMethodJapan.TabIndex = 46;
            this.rbtnClassMethodJapan.TabStop = true;
            this.rbtnClassMethodJapan.Text = "(R-B)/(R+B)   (Jap)";
            this.rbtnClassMethodJapan.UseVisualStyleBackColor = true;
            this.rbtnClassMethodJapan.CheckedChanged += new System.EventHandler(this.rbtnClassMethodJapan_CheckedChanged);
            // 
            // rbtnClassMethodUS
            // 
            this.rbtnClassMethodUS.AutoSize = true;
            this.rbtnClassMethodUS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnClassMethodUS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnClassMethodUS.Location = new System.Drawing.Point(7, 41);
            this.rbtnClassMethodUS.Margin = new System.Windows.Forms.Padding(4);
            this.rbtnClassMethodUS.Name = "rbtnClassMethodUS";
            this.rbtnClassMethodUS.Size = new System.Drawing.Size(116, 29);
            this.rbtnClassMethodUS.TabIndex = 47;
            this.rbtnClassMethodUS.Text = "R/B  (US)";
            this.rbtnClassMethodUS.UseVisualStyleBackColor = true;
            this.rbtnClassMethodUS.CheckedChanged += new System.EventHandler(this.rbtnClassMethodUS_CheckedChanged);
            // 
            // btnCalcSunPosition
            // 
            this.btnCalcSunPosition.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnCalcSunPosition, 3);
            this.btnCalcSunPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalcSunPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalcSunPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalcSunPosition.Location = new System.Drawing.Point(561, 2);
            this.btnCalcSunPosition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCalcSunPosition.Name = "btnCalcSunPosition";
            this.btnCalcSunPosition.Size = new System.Drawing.Size(273, 76);
            this.btnCalcSunPosition.TabIndex = 68;
            this.btnCalcSunPosition.Text = "Check sun position";
            this.btnCalcSunPosition.UseVisualStyleBackColor = false;
            this.btnCalcSunPosition.Click += new System.EventHandler(this.btnCalcSunPosition_Click);
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(840, 2);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 76);
            this.button5.TabIndex = 69;
            this.button5.Text = "XLS SUN";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // bgwProcessDirectoryOfImages
            // 
            this.bgwProcessDirectoryOfImages.WorkerReportsProgress = true;
            this.bgwProcessDirectoryOfImages.WorkerSupportsCancellation = true;
            this.bgwProcessDirectoryOfImages.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.bgwProcessDirectoryOfImages.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.bgwProcessDirectoryOfImages.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // MainAnalysisForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1408, 826);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainAnalysisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sky images analysis";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.SkyIndexAnalyzer_AnalysisForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SkyIndexAnalyzer_AnalysisForm_KeyPress);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.ComponentModel.BackgroundWorker bgwProcessOneImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gPSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchCollectingDataMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.ProgressBar pbUniversalProgressBar;
        private System.Windows.Forms.Label label8;
        private System.ComponentModel.BackgroundWorker bgwProcessDirectoryOfImages;
        private System.Windows.Forms.Button открытьФайлToolStripMenuItem;
        private System.Windows.Forms.Button обработатьToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem настройкиСбораДанныхToolStripMenuItem;
        private System.Windows.Forms.Button DetectEdgesButton;
        private System.Windows.Forms.Label lblClassificationMethod;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.RadioButton rbtnClassMethodJapan;
        public System.Windows.Forms.RadioButton rbtnClassMethodUS;
        public System.Windows.Forms.RadioButton rbtnClassMethodGrIx;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnAbout;
        private System.ComponentModel.BackgroundWorker bgwSunDetectionOnly;
        private System.Windows.Forms.Button btnTest1;
        private System.Windows.Forms.Button btnProcessDirectorySI;
        private System.Windows.Forms.Button btnShowMedianPerc5Diagram;
        private System.Windows.Forms.Button btnDensityProcessing;
        private System.Windows.Forms.Button btnSortImagesByClasses;
        private System.Windows.Forms.Button btnTestSunDetection2015;
        private System.Windows.Forms.Label lblResultTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnSaveClustering;
        private System.Windows.Forms.RadioButton rbtnClusterizePoints;
        private System.Windows.Forms.RadioButton rbtnShowDensity;
        private System.Windows.Forms.Button btnCalcSunPosition;
        private System.Windows.Forms.Button button5;
    }
}

