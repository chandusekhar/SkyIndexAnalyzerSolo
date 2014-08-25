﻿namespace SkyIndexAnalyzerSolo
{
    partial class SkyIndexAnalyzer_AnalysisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkyIndexAnalyzer_AnalysisForm));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
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
            this.rbtnClassMethodNew = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.открытьФайлToolStripMenuItem = new System.Windows.Forms.Button();
            this.обработатьToolStripMenuItem = new System.Windows.Forms.Button();
            this.eXIFыToolStripMenuItem = new System.Windows.Forms.Button();
            this.DetectEdgesButton = new System.Windows.Forms.Button();
            this.lblClassificationMethod = new System.Windows.Forms.Label();
            this.rbtnClassMethodJapan = new System.Windows.Forms.RadioButton();
            this.rbtnClassMethodGreek = new System.Windows.Forms.RadioButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnTestMarginWithoutSun = new System.Windows.Forms.Button();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnTest1 = new System.Windows.Forms.Button();
            this.btnProcessDirectorySI = new System.Windows.Forms.Button();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.bgwSunDetectionOnly = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "*.jpg|*.jpg";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Укажите загружаемые файлы";
            // 
            // pbUniversalProgressBar
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbUniversalProgressBar, 15);
            this.pbUniversalProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbUniversalProgressBar.Location = new System.Drawing.Point(3, 660);
            this.pbUniversalProgressBar.Name = "pbUniversalProgressBar";
            this.pbUniversalProgressBar.Size = new System.Drawing.Size(931, 15);
            this.pbUniversalProgressBar.TabIndex = 25;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.StatusLabel, 15);
            this.StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusLabel.Location = new System.Drawing.Point(3, 637);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(931, 20);
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
            this.label5.Location = new System.Drawing.Point(3, 306);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(219, 21);
            this.label5.TabIndex = 22;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(676, 330);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 30);
            this.button4.TabIndex = 20;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // trackBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.trackBar1, 8);
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar1.Location = new System.Drawing.Point(228, 330);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(442, 30);
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
            this.label1.Location = new System.Drawing.Point(3, 327);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 36);
            this.label1.TabIndex = 14;
            this.label1.Text = "Подстройка (SI):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(732, 327);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 36);
            this.label2.TabIndex = 15;
            this.label2.Text = "0.10";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox2, 4);
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(3, 99);
            this.pictureBox2.Name = "pictureBox2";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox2, 10);
            this.pictureBox2.Size = new System.Drawing.Size(219, 204);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 5);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(228, 99);
            this.pictureBox1.Name = "pictureBox1";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox1, 10);
            this.pictureBox1.Size = new System.Drawing.Size(274, 204);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
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
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
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
            this.getDataToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.getDataToolStripMenuItem.Text = "Получить данные GPS";
            // 
            // switchCollectingDataMenuItem
            // 
            this.switchCollectingDataMenuItem.Name = "switchCollectingDataMenuItem";
            this.switchCollectingDataMenuItem.Size = new System.Drawing.Size(265, 22);
            this.switchCollectingDataMenuItem.Text = "Начать сбор данных всех датчиков";
            // 
            // настройкиСбораДанныхToolStripMenuItem
            // 
            this.настройкиСбораДанныхToolStripMenuItem.Name = "настройкиСбораДанныхToolStripMenuItem";
            this.настройкиСбораДанныхToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.настройкиСбораДанныхToolStripMenuItem.Text = "Сбор данных";
            this.настройкиСбораДанныхToolStripMenuItem.Click += new System.EventHandler(this.настройкиСбораДанныхToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 15;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692307F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.692306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel1.Controls.Add(this.rbtnClassMethodNew, 4, 16);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 13, 13);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.trackBar1, 4, 13);
            this.tableLayoutPanel1.Controls.Add(this.button4, 12, 13);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.StatusLabel, 0, 27);
            this.tableLayoutPanel1.Controls.Add(this.pbUniversalProgressBar, 0, 28);
            this.tableLayoutPanel1.Controls.Add(this.trackBar3, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 4, 12);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 30);
            this.tableLayoutPanel1.Controls.Add(this.открытьФайлToolStripMenuItem, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.обработатьToolStripMenuItem, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.eXIFыToolStripMenuItem, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.DetectEdgesButton, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblClassificationMethod, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.rbtnClassMethodJapan, 4, 14);
            this.tableLayoutPanel1.Controls.Add(this.rbtnClassMethodGreek, 4, 15);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 0, 29);
            this.tableLayoutPanel1.Controls.Add(this.button2, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 13, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 11, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTestMarginWithoutSun, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox4, 9, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAbout, 14, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTest1, 4, 30);
            this.tableLayoutPanel1.Controls.Add(this.btnProcessDirectorySI, 10, 30);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 31;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.956639F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304425F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.39049F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.650819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.39049F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.304426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.306083F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.303668F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(937, 742);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rbtnClassMethodNew
            // 
            this.rbtnClassMethodNew.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.rbtnClassMethodNew, 3);
            this.rbtnClassMethodNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnClassMethodNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnClassMethodNew.Location = new System.Drawing.Point(228, 408);
            this.rbtnClassMethodNew.Name = "rbtnClassMethodNew";
            this.rbtnClassMethodNew.Size = new System.Drawing.Size(162, 15);
            this.rbtnClassMethodNew.TabIndex = 50;
            this.rbtnClassMethodNew.Text = "test new method";
            this.rbtnClassMethodNew.UseVisualStyleBackColor = true;
            this.rbtnClassMethodNew.CheckedChanged += new System.EventHandler(this.rbtnClassMethodNew_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 15);
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 429);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.tableLayoutPanel1.SetRowSpan(this.textBox1, 10);
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(931, 204);
            this.textBox1.TabIndex = 37;
            // 
            // trackBar3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.trackBar3, 7);
            this.trackBar3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar3.Location = new System.Drawing.Point(228, 67);
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(386, 26);
            this.trackBar3.TabIndex = 28;
            this.trackBar3.Value = 10;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label8, 11);
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(228, 306);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(706, 21);
            this.label8.TabIndex = 30;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label9, 4);
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(219, 32);
            this.label9.TabIndex = 31;
            this.label9.Text = "Изменять исходный размер:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.button1, 4);
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(0, 708);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(225, 34);
            this.button1.TabIndex = 32;
            this.button1.Text = "Обработка директории: (cloud cover)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // открытьФайлToolStripMenuItem
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.открытьФайлToolStripMenuItem, 2);
            this.открытьФайлToolStripMenuItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.открытьФайлToolStripMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.открытьФайлToolStripMenuItem.Location = new System.Drawing.Point(3, 3);
            this.открытьФайлToolStripMenuItem.Name = "открытьФайлToolStripMenuItem";
            this.открытьФайлToolStripMenuItem.Size = new System.Drawing.Size(107, 58);
            this.открытьФайлToolStripMenuItem.TabIndex = 34;
            this.открытьФайлToolStripMenuItem.Text = "Открыть файл";
            this.открытьФайлToolStripMenuItem.UseVisualStyleBackColor = true;
            this.открытьФайлToolStripMenuItem.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // обработатьToolStripMenuItem
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.обработатьToolStripMenuItem, 2);
            this.обработатьToolStripMenuItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.обработатьToolStripMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.обработатьToolStripMenuItem.Location = new System.Drawing.Point(116, 3);
            this.обработатьToolStripMenuItem.Name = "обработатьToolStripMenuItem";
            this.обработатьToolStripMenuItem.Size = new System.Drawing.Size(106, 58);
            this.обработатьToolStripMenuItem.TabIndex = 35;
            this.обработатьToolStripMenuItem.Text = "Обработать";
            this.обработатьToolStripMenuItem.UseVisualStyleBackColor = true;
            this.обработатьToolStripMenuItem.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // eXIFыToolStripMenuItem
            // 
            this.eXIFыToolStripMenuItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eXIFыToolStripMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.eXIFыToolStripMenuItem.Location = new System.Drawing.Point(228, 3);
            this.eXIFыToolStripMenuItem.Name = "eXIFыToolStripMenuItem";
            this.eXIFыToolStripMenuItem.Size = new System.Drawing.Size(50, 58);
            this.eXIFыToolStripMenuItem.TabIndex = 36;
            this.eXIFыToolStripMenuItem.Text = "EXIF";
            this.eXIFыToolStripMenuItem.UseVisualStyleBackColor = true;
            this.eXIFыToolStripMenuItem.Click += new System.EventHandler(this.button3_Click);
            // 
            // DetectEdgesButton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.DetectEdgesButton, 2);
            this.DetectEdgesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetectEdgesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DetectEdgesButton.Location = new System.Drawing.Point(284, 3);
            this.DetectEdgesButton.Name = "DetectEdgesButton";
            this.DetectEdgesButton.Size = new System.Drawing.Size(106, 58);
            this.DetectEdgesButton.TabIndex = 43;
            this.DetectEdgesButton.Text = "Разметка по октам";
            this.DetectEdgesButton.UseVisualStyleBackColor = true;
            this.DetectEdgesButton.Click += new System.EventHandler(this.DetectEdgesButton_Click);
            // 
            // lblClassificationMethod
            // 
            this.lblClassificationMethod.AutoSize = true;
            this.lblClassificationMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblClassificationMethod, 4);
            this.lblClassificationMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassificationMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblClassificationMethod.Location = new System.Drawing.Point(3, 363);
            this.lblClassificationMethod.Name = "lblClassificationMethod";
            this.tableLayoutPanel1.SetRowSpan(this.lblClassificationMethod, 3);
            this.lblClassificationMethod.Size = new System.Drawing.Size(219, 63);
            this.lblClassificationMethod.TabIndex = 45;
            this.lblClassificationMethod.Text = "Classification method:";
            this.lblClassificationMethod.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbtnClassMethodJapan
            // 
            this.rbtnClassMethodJapan.AutoSize = true;
            this.rbtnClassMethodJapan.Checked = true;
            this.tableLayoutPanel1.SetColumnSpan(this.rbtnClassMethodJapan, 3);
            this.rbtnClassMethodJapan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnClassMethodJapan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnClassMethodJapan.Location = new System.Drawing.Point(228, 366);
            this.rbtnClassMethodJapan.Name = "rbtnClassMethodJapan";
            this.rbtnClassMethodJapan.Size = new System.Drawing.Size(162, 15);
            this.rbtnClassMethodJapan.TabIndex = 46;
            this.rbtnClassMethodJapan.TabStop = true;
            this.rbtnClassMethodJapan.Text = "(R-B)/(R+B)";
            this.rbtnClassMethodJapan.UseVisualStyleBackColor = true;
            this.rbtnClassMethodJapan.CheckedChanged += new System.EventHandler(this.rbtnClassMethodJapan_CheckedChanged);
            // 
            // rbtnClassMethodGreek
            // 
            this.rbtnClassMethodGreek.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.rbtnClassMethodGreek, 3);
            this.rbtnClassMethodGreek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnClassMethodGreek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnClassMethodGreek.Location = new System.Drawing.Point(228, 387);
            this.rbtnClassMethodGreek.Name = "rbtnClassMethodGreek";
            this.rbtnClassMethodGreek.Size = new System.Drawing.Size(162, 15);
            this.rbtnClassMethodGreek.TabIndex = 47;
            this.rbtnClassMethodGreek.Text = "(B<R+20) && (B<G+20) && (B<60)";
            this.rbtnClassMethodGreek.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBox1, 15);
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 681);
            this.richTextBox1.Multiline = false;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(931, 24);
            this.richTextBox1.TabIndex = 48;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.button2, 3);
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(564, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(162, 58);
            this.button2.TabIndex = 49;
            this.button2.Text = "Ручной анализ цветности";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Image = ((System.Drawing.Image)(resources.GetObject("btnProperties.Image")));
            this.btnProperties.Location = new System.Drawing.Point(732, 3);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(58, 58);
            this.btnProperties.TabIndex = 52;
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // button3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.button3, 4);
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(620, 67);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(314, 26);
            this.button3.TabIndex = 53;
            this.button3.Text = "GrIx histogram";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // btnTestMarginWithoutSun
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnTestMarginWithoutSun, 2);
            this.btnTestMarginWithoutSun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTestMarginWithoutSun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestMarginWithoutSun.Location = new System.Drawing.Point(452, 3);
            this.btnTestMarginWithoutSun.Name = "btnTestMarginWithoutSun";
            this.btnTestMarginWithoutSun.Size = new System.Drawing.Size(106, 58);
            this.btnTestMarginWithoutSun.TabIndex = 54;
            this.btnTestMarginWithoutSun.Text = "Отладка без солнца";
            this.btnTestMarginWithoutSun.UseVisualStyleBackColor = true;
            this.btnTestMarginWithoutSun.Click += new System.EventHandler(this.btnTestMarginWithoutSun_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox4, 6);
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Location = new System.Drawing.Point(508, 99);
            this.pictureBox4.Name = "pictureBox4";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox4, 10);
            this.pictureBox4.Size = new System.Drawing.Size(426, 204);
            this.pictureBox4.TabIndex = 56;
            this.pictureBox4.TabStop = false;
            // 
            // btnAbout
            // 
            this.btnAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Image = ((System.Drawing.Image)(resources.GetObject("btnAbout.Image")));
            this.btnAbout.Location = new System.Drawing.Point(796, 3);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(138, 58);
            this.btnAbout.TabIndex = 57;
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnTest1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnTest1, 6);
            this.btnTest1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest1.Location = new System.Drawing.Point(225, 708);
            this.btnTest1.Margin = new System.Windows.Forms.Padding(0);
            this.btnTest1.Name = "btnTest1";
            this.btnTest1.Size = new System.Drawing.Size(336, 34);
            this.btnTest1.TabIndex = 59;
            this.btnTest1.Text = "Collect statistics (5perc and Median valus over images set)";
            this.btnTest1.UseVisualStyleBackColor = true;
            this.btnTest1.Click += new System.EventHandler(this.btnTest1_Click);
            // 
            // btnProcessDirectorySI
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnProcessDirectorySI, 5);
            this.btnProcessDirectorySI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProcessDirectorySI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcessDirectorySI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnProcessDirectorySI.Location = new System.Drawing.Point(561, 708);
            this.btnProcessDirectorySI.Margin = new System.Windows.Forms.Padding(0);
            this.btnProcessDirectorySI.Name = "btnProcessDirectorySI";
            this.btnProcessDirectorySI.Size = new System.Drawing.Size(376, 34);
            this.btnProcessDirectorySI.TabIndex = 60;
            this.btnProcessDirectorySI.Text = "Обработка директории (только SkyIndex)";
            this.btnProcessDirectorySI.UseVisualStyleBackColor = true;
            this.btnProcessDirectorySI.Click += new System.EventHandler(this.btnProcessDirectorySI_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // SkyIndexAnalyzer_AnalysisForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 742);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SkyIndexAnalyzer_AnalysisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sky index visualizer";
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
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.ComponentModel.BackgroundWorker backgroundWorker1;
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
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Button открытьФайлToolStripMenuItem;
        private System.Windows.Forms.Button обработатьToolStripMenuItem;
        private System.Windows.Forms.Button eXIFыToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem настройкиСбораДанныхToolStripMenuItem;
        private System.Windows.Forms.Button DetectEdgesButton;
        private System.Windows.Forms.Label lblClassificationMethod;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.RadioButton rbtnClassMethodJapan;
        public System.Windows.Forms.RadioButton rbtnClassMethodGreek;
        public System.Windows.Forms.RadioButton rbtnClassMethodNew;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnTestMarginWithoutSun;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button btnAbout;
        private System.ComponentModel.BackgroundWorker bgwSunDetectionOnly;
        private System.Windows.Forms.Button btnTest1;
        private System.Windows.Forms.Button btnProcessDirectorySI;
    }
}

