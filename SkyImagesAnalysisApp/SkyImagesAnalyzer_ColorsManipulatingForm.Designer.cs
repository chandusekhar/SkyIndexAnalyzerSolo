﻿namespace SkyImagesAnalyzer
{
    partial class SkyImagesAnalyzer_ColorsManipulatingForm
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
            this.bgwSkyIndexAnalyzer = new System.ComponentModel.BackgroundWorker();
            this.chbRes3DynamicScale = new System.Windows.Forms.CheckBox();
            this.chbRes2DynamicScale = new System.Windows.Forms.CheckBox();
            this.chbRes1DynamicScale = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rbtnSwitchChannelB = new System.Windows.Forms.RadioButton();
            this.rbtnSwitchChannelG = new System.Windows.Forms.RadioButton();
            this.rbtnSwitchChannelR = new System.Windows.Forms.RadioButton();
            this.tbChannelThresh = new System.Windows.Forms.TextBox();
            this.tbRes3Thresh = new System.Windows.Forms.TextBox();
            this.tbRes2Thresh = new System.Windows.Forms.TextBox();
            this.tbRes1Thresh = new System.Windows.Forms.TextBox();
            this.btnBrowseColorScheme1 = new System.Windows.Forms.Button();
            this.tbColorSchemePath1 = new System.Windows.Forms.TextBox();
            this.lblUsingColorScheme1 = new System.Windows.Forms.Label();
            this.tbStats3 = new System.Windows.Forms.TextBox();
            this.tbStats2 = new System.Windows.Forms.TextBox();
            this.tbStats1 = new System.Windows.Forms.TextBox();
            this.btnOK1 = new System.Windows.Forms.Button();
            this.tbFormula1 = new System.Windows.Forms.TextBox();
            this.lblSourceImage = new System.Windows.Forms.Label();
            this.lblSkyIndex = new System.Windows.Forms.Label();
            this.lblBlue = new System.Windows.Forms.Label();
            this.lblResult1 = new System.Windows.Forms.Label();
            this.lblResult2 = new System.Windows.Forms.Label();
            this.btnOK2 = new System.Windows.Forms.Button();
            this.tbFormula2 = new System.Windows.Forms.TextBox();
            this.lblResult3 = new System.Windows.Forms.Label();
            this.btnOK3 = new System.Windows.Forms.Button();
            this.tbFormula3 = new System.Windows.Forms.TextBox();
            this.lblUsingColorScheme2 = new System.Windows.Forms.Label();
            this.lblUsingColorScheme3 = new System.Windows.Forms.Label();
            this.tbColorSchemePath2 = new System.Windows.Forms.TextBox();
            this.tbColorSchemePath3 = new System.Windows.Forms.TextBox();
            this.btnBrowseColorScheme2 = new System.Windows.Forms.Button();
            this.btnBrowseColorScheme3 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveDataRes3 = new System.Windows.Forms.Button();
            this.btnSaveDataRes2 = new System.Windows.Forms.Button();
            this.pbRes3 = new System.Windows.Forms.PictureBox();
            this.pbRes2 = new System.Windows.Forms.PictureBox();
            this.pbChannel = new System.Windows.Forms.PictureBox();
            this.pbRes1 = new System.Windows.Forms.PictureBox();
            this.pbScale = new System.Windows.Forms.PictureBox();
            this.pbRes1Scale = new System.Windows.Forms.PictureBox();
            this.pbRes2Scale = new System.Windows.Forms.PictureBox();
            this.pbRes3Scale = new System.Windows.Forms.PictureBox();
            this.pbSourceImage = new System.Windows.Forms.PictureBox();
            this.pbSkyIndexImage = new System.Windows.Forms.PictureBox();
            this.pbSkyIndexScale = new System.Windows.Forms.PictureBox();
            this.btnSaveDataRes1 = new System.Windows.Forms.Button();
            this.btnTestProcess = new System.Windows.Forms.Button();
            this.btnSaveDataSkyIndexData = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRes1Hist = new System.Windows.Forms.Button();
            this.btnRes2Hist = new System.Windows.Forms.Button();
            this.btnRes3Hist = new System.Windows.Forms.Button();
            this.btnSectionProfile1 = new System.Windows.Forms.Button();
            this.btnSectionProfile2 = new System.Windows.Forms.Button();
            this.btnSectionProfile3 = new System.Windows.Forms.Button();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes1Scale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes2Scale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes3Scale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSourceImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkyIndexImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkyIndexScale)).BeginInit();
            this.SuspendLayout();
            // 
            // bgwSkyIndexAnalyzer
            // 
            this.bgwSkyIndexAnalyzer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSkyIndexAnalyzer_DoWork);
            this.bgwSkyIndexAnalyzer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSkyIndexAnalyzer_RunWorkerCompleted);
            // 
            // chbRes3DynamicScale
            // 
            this.chbRes3DynamicScale.AutoSize = true;
            this.chbRes3DynamicScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chbRes3DynamicScale.Location = new System.Drawing.Point(1466, 728);
            this.chbRes3DynamicScale.Margin = new System.Windows.Forms.Padding(4);
            this.chbRes3DynamicScale.Name = "chbRes3DynamicScale";
            this.chbRes3DynamicScale.Size = new System.Drawing.Size(121, 29);
            this.chbRes3DynamicScale.TabIndex = 73;
            this.chbRes3DynamicScale.Text = "Dynamic";
            this.chbRes3DynamicScale.UseVisualStyleBackColor = true;
            this.chbRes3DynamicScale.CheckedChanged += new System.EventHandler(this.chbResDynamicScale_CheckedChanged);
            // 
            // chbRes2DynamicScale
            // 
            this.chbRes2DynamicScale.AutoSize = true;
            this.chbRes2DynamicScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chbRes2DynamicScale.Location = new System.Drawing.Point(943, 728);
            this.chbRes2DynamicScale.Margin = new System.Windows.Forms.Padding(4);
            this.chbRes2DynamicScale.Name = "chbRes2DynamicScale";
            this.chbRes2DynamicScale.Size = new System.Drawing.Size(99, 29);
            this.chbRes2DynamicScale.TabIndex = 72;
            this.chbRes2DynamicScale.Text = "Dynamic";
            this.chbRes2DynamicScale.UseVisualStyleBackColor = true;
            this.chbRes2DynamicScale.CheckedChanged += new System.EventHandler(this.chbResDynamicScale_CheckedChanged);
            // 
            // chbRes1DynamicScale
            // 
            this.chbRes1DynamicScale.AutoSize = true;
            this.chbRes1DynamicScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chbRes1DynamicScale.Location = new System.Drawing.Point(420, 728);
            this.chbRes1DynamicScale.Margin = new System.Windows.Forms.Padding(4);
            this.chbRes1DynamicScale.Name = "chbRes1DynamicScale";
            this.chbRes1DynamicScale.Size = new System.Drawing.Size(99, 29);
            this.chbRes1DynamicScale.TabIndex = 71;
            this.chbRes1DynamicScale.Text = "Dynamic";
            this.chbRes1DynamicScale.UseVisualStyleBackColor = true;
            this.chbRes1DynamicScale.CheckedChanged += new System.EventHandler(this.chbResDynamicScale_CheckedChanged);
            // 
            // groupBox7
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox7, 2);
            this.groupBox7.Controls.Add(this.rbtnSwitchChannelB);
            this.groupBox7.Controls.Add(this.rbtnSwitchChannelG);
            this.groupBox7.Controls.Add(this.rbtnSwitchChannelR);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox7.Location = new System.Drawing.Point(1050, 281);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox7.Size = new System.Drawing.Size(408, 29);
            this.groupBox7.TabIndex = 67;
            this.groupBox7.TabStop = false;
            // 
            // rbtnSwitchChannelB
            // 
            this.rbtnSwitchChannelB.AutoSize = true;
            this.rbtnSwitchChannelB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbtnSwitchChannelB.ForeColor = System.Drawing.Color.Blue;
            this.rbtnSwitchChannelB.Location = new System.Drawing.Point(185, 5);
            this.rbtnSwitchChannelB.Margin = new System.Windows.Forms.Padding(4);
            this.rbtnSwitchChannelB.Name = "rbtnSwitchChannelB";
            this.rbtnSwitchChannelB.Size = new System.Drawing.Size(68, 24);
            this.rbtnSwitchChannelB.TabIndex = 2;
            this.rbtnSwitchChannelB.Text = "Blue";
            this.rbtnSwitchChannelB.UseVisualStyleBackColor = true;
            this.rbtnSwitchChannelB.CheckedChanged += new System.EventHandler(this.rbtnSwitchChannel_CheckedChanged);
            // 
            // rbtnSwitchChannelG
            // 
            this.rbtnSwitchChannelG.AutoSize = true;
            this.rbtnSwitchChannelG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbtnSwitchChannelG.ForeColor = System.Drawing.Color.Green;
            this.rbtnSwitchChannelG.Location = new System.Drawing.Point(89, 5);
            this.rbtnSwitchChannelG.Margin = new System.Windows.Forms.Padding(4);
            this.rbtnSwitchChannelG.Name = "rbtnSwitchChannelG";
            this.rbtnSwitchChannelG.Size = new System.Drawing.Size(81, 24);
            this.rbtnSwitchChannelG.TabIndex = 1;
            this.rbtnSwitchChannelG.Text = "Green";
            this.rbtnSwitchChannelG.UseVisualStyleBackColor = true;
            this.rbtnSwitchChannelG.CheckedChanged += new System.EventHandler(this.rbtnSwitchChannel_CheckedChanged);
            // 
            // rbtnSwitchChannelR
            // 
            this.rbtnSwitchChannelR.AutoSize = true;
            this.rbtnSwitchChannelR.Checked = true;
            this.rbtnSwitchChannelR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbtnSwitchChannelR.ForeColor = System.Drawing.Color.Red;
            this.rbtnSwitchChannelR.Location = new System.Drawing.Point(8, 5);
            this.rbtnSwitchChannelR.Margin = new System.Windows.Forms.Padding(4);
            this.rbtnSwitchChannelR.Name = "rbtnSwitchChannelR";
            this.rbtnSwitchChannelR.Size = new System.Drawing.Size(63, 24);
            this.rbtnSwitchChannelR.TabIndex = 0;
            this.rbtnSwitchChannelR.TabStop = true;
            this.rbtnSwitchChannelR.Text = "Red";
            this.rbtnSwitchChannelR.UseVisualStyleBackColor = true;
            this.rbtnSwitchChannelR.CheckedChanged += new System.EventHandler(this.rbtnSwitchChannel_CheckedChanged);
            // 
            // tbChannelThresh
            // 
            this.tbChannelThresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbChannelThresh.Location = new System.Drawing.Point(1466, 4);
            this.tbChannelThresh.Margin = new System.Windows.Forms.Padding(4);
            this.tbChannelThresh.Name = "tbChannelThresh";
            this.tbChannelThresh.Size = new System.Drawing.Size(121, 22);
            this.tbChannelThresh.TabIndex = 64;
            this.tbChannelThresh.Text = "255";
            this.tbChannelThresh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRedThresh_KeyPress);
            // 
            // tbRes3Thresh
            // 
            this.tbRes3Thresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRes3Thresh.Location = new System.Drawing.Point(1466, 429);
            this.tbRes3Thresh.Margin = new System.Windows.Forms.Padding(4);
            this.tbRes3Thresh.Name = "tbRes3Thresh";
            this.tbRes3Thresh.Size = new System.Drawing.Size(121, 22);
            this.tbRes3Thresh.TabIndex = 63;
            this.tbRes3Thresh.Text = "255";
            this.tbRes3Thresh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbResultThresh_KeyPress);
            // 
            // tbRes2Thresh
            // 
            this.tbRes2Thresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRes2Thresh.Location = new System.Drawing.Point(943, 429);
            this.tbRes2Thresh.Margin = new System.Windows.Forms.Padding(4);
            this.tbRes2Thresh.Name = "tbRes2Thresh";
            this.tbRes2Thresh.Size = new System.Drawing.Size(99, 22);
            this.tbRes2Thresh.TabIndex = 62;
            this.tbRes2Thresh.Text = "255";
            this.tbRes2Thresh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbResultThresh_KeyPress);
            // 
            // tbRes1Thresh
            // 
            this.tbRes1Thresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRes1Thresh.Location = new System.Drawing.Point(420, 429);
            this.tbRes1Thresh.Margin = new System.Windows.Forms.Padding(4);
            this.tbRes1Thresh.Name = "tbRes1Thresh";
            this.tbRes1Thresh.Size = new System.Drawing.Size(99, 22);
            this.tbRes1Thresh.TabIndex = 61;
            this.tbRes1Thresh.Text = "255";
            this.tbRes1Thresh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbResultThresh_KeyPress);
            // 
            // btnBrowseColorScheme1
            // 
            this.btnBrowseColorScheme1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseColorScheme1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseColorScheme1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseColorScheme1.Location = new System.Drawing.Point(420, 392);
            this.btnBrowseColorScheme1.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseColorScheme1.Name = "btnBrowseColorScheme1";
            this.btnBrowseColorScheme1.Size = new System.Drawing.Size(99, 29);
            this.btnBrowseColorScheme1.TabIndex = 46;
            this.btnBrowseColorScheme1.Text = ". . .";
            this.btnBrowseColorScheme1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBrowseColorScheme1.UseVisualStyleBackColor = true;
            this.btnBrowseColorScheme1.Click += new System.EventHandler(this.btnBrowseColorScheme1_Click);
            // 
            // tbColorSchemePath1
            // 
            this.tbColorSchemePath1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbColorSchemePath1.Location = new System.Drawing.Point(212, 392);
            this.tbColorSchemePath1.Margin = new System.Windows.Forms.Padding(4);
            this.tbColorSchemePath1.Name = "tbColorSchemePath1";
            this.tbColorSchemePath1.Size = new System.Drawing.Size(200, 22);
            this.tbColorSchemePath1.TabIndex = 43;
            // 
            // lblUsingColorScheme1
            // 
            this.lblUsingColorScheme1.AutoSize = true;
            this.lblUsingColorScheme1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsingColorScheme1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUsingColorScheme1.Location = new System.Drawing.Point(4, 388);
            this.lblUsingColorScheme1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsingColorScheme1.Name = "lblUsingColorScheme1";
            this.lblUsingColorScheme1.Size = new System.Drawing.Size(200, 37);
            this.lblUsingColorScheme1.TabIndex = 40;
            this.lblUsingColorScheme1.Text = "Color scheme:";
            this.lblUsingColorScheme1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbStats3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbStats3, 2);
            this.tbStats3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbStats3.Location = new System.Drawing.Point(1050, 765);
            this.tbStats3.Margin = new System.Windows.Forms.Padding(4);
            this.tbStats3.Multiline = true;
            this.tbStats3.Name = "tbStats3";
            this.tbStats3.Size = new System.Drawing.Size(408, 81);
            this.tbStats3.TabIndex = 39;
            // 
            // tbStats2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbStats2, 2);
            this.tbStats2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbStats2.Location = new System.Drawing.Point(527, 765);
            this.tbStats2.Margin = new System.Windows.Forms.Padding(4);
            this.tbStats2.Multiline = true;
            this.tbStats2.Name = "tbStats2";
            this.tbStats2.Size = new System.Drawing.Size(408, 81);
            this.tbStats2.TabIndex = 38;
            // 
            // tbStats1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbStats1, 2);
            this.tbStats1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbStats1.Location = new System.Drawing.Point(4, 765);
            this.tbStats1.Margin = new System.Windows.Forms.Padding(4);
            this.tbStats1.Multiline = true;
            this.tbStats1.Name = "tbStats1";
            this.tbStats1.Size = new System.Drawing.Size(408, 81);
            this.tbStats1.TabIndex = 37;
            // 
            // btnOK1
            // 
            this.btnOK1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK1.Location = new System.Drawing.Point(420, 355);
            this.btnOK1.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK1.Name = "btnOK1";
            this.btnOK1.Size = new System.Drawing.Size(99, 29);
            this.btnOK1.TabIndex = 27;
            this.btnOK1.Text = "OK";
            this.btnOK1.UseVisualStyleBackColor = true;
            this.btnOK1.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbFormula1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbFormula1, 2);
            this.tbFormula1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFormula1.Location = new System.Drawing.Point(4, 355);
            this.tbFormula1.Margin = new System.Windows.Forms.Padding(4);
            this.tbFormula1.Name = "tbFormula1";
            this.tbFormula1.Size = new System.Drawing.Size(408, 22);
            this.tbFormula1.TabIndex = 26;
            // 
            // lblSourceImage
            // 
            this.lblSourceImage.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSourceImage, 2);
            this.lblSourceImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSourceImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSourceImage.Location = new System.Drawing.Point(4, 0);
            this.lblSourceImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceImage.Name = "lblSourceImage";
            this.lblSourceImage.Size = new System.Drawing.Size(408, 37);
            this.lblSourceImage.TabIndex = 1;
            this.lblSourceImage.Text = "Исходное изображение";
            this.lblSourceImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSkyIndex
            // 
            this.lblSkyIndex.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSkyIndex, 2);
            this.lblSkyIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSkyIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSkyIndex.Location = new System.Drawing.Point(527, 0);
            this.lblSkyIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSkyIndex.Name = "lblSkyIndex";
            this.lblSkyIndex.Size = new System.Drawing.Size(408, 37);
            this.lblSkyIndex.TabIndex = 3;
            this.lblSkyIndex.Text = "SkyIndex";
            this.lblSkyIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBlue
            // 
            this.lblBlue.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblBlue, 2);
            this.lblBlue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblBlue.Location = new System.Drawing.Point(1050, 0);
            this.lblBlue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBlue.Name = "lblBlue";
            this.lblBlue.Size = new System.Drawing.Size(408, 37);
            this.lblBlue.TabIndex = 5;
            this.lblBlue.Text = "Цветовой канал";
            this.lblBlue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult1
            // 
            this.lblResult1.AutoSize = true;
            this.lblResult1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblResult1, 2);
            this.lblResult1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResult1.Location = new System.Drawing.Point(4, 314);
            this.lblResult1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult1.Name = "lblResult1";
            this.lblResult1.Size = new System.Drawing.Size(408, 37);
            this.lblResult1.TabIndex = 25;
            this.lblResult1.Text = "Result 1";
            this.lblResult1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult2
            // 
            this.lblResult2.AutoSize = true;
            this.lblResult2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblResult2, 2);
            this.lblResult2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResult2.Location = new System.Drawing.Point(527, 314);
            this.lblResult2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult2.Name = "lblResult2";
            this.lblResult2.Size = new System.Drawing.Size(408, 37);
            this.lblResult2.TabIndex = 29;
            this.lblResult2.Text = "Result 2";
            this.lblResult2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOK2
            // 
            this.btnOK2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK2.Location = new System.Drawing.Point(943, 355);
            this.btnOK2.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK2.Name = "btnOK2";
            this.btnOK2.Size = new System.Drawing.Size(99, 29);
            this.btnOK2.TabIndex = 31;
            this.btnOK2.Text = "OK";
            this.btnOK2.UseVisualStyleBackColor = true;
            this.btnOK2.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbFormula2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbFormula2, 2);
            this.tbFormula2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFormula2.Location = new System.Drawing.Point(527, 355);
            this.tbFormula2.Margin = new System.Windows.Forms.Padding(4);
            this.tbFormula2.Name = "tbFormula2";
            this.tbFormula2.Size = new System.Drawing.Size(408, 22);
            this.tbFormula2.TabIndex = 30;
            // 
            // lblResult3
            // 
            this.lblResult3.AutoSize = true;
            this.lblResult3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblResult3, 2);
            this.lblResult3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResult3.Location = new System.Drawing.Point(1050, 314);
            this.lblResult3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult3.Name = "lblResult3";
            this.lblResult3.Size = new System.Drawing.Size(408, 37);
            this.lblResult3.TabIndex = 33;
            this.lblResult3.Text = "Result 3";
            this.lblResult3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOK3
            // 
            this.btnOK3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK3.Location = new System.Drawing.Point(1466, 355);
            this.btnOK3.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK3.Name = "btnOK3";
            this.btnOK3.Size = new System.Drawing.Size(121, 29);
            this.btnOK3.TabIndex = 35;
            this.btnOK3.Text = "OK";
            this.btnOK3.UseVisualStyleBackColor = true;
            this.btnOK3.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbFormula3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbFormula3, 2);
            this.tbFormula3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFormula3.Location = new System.Drawing.Point(1050, 355);
            this.tbFormula3.Margin = new System.Windows.Forms.Padding(4);
            this.tbFormula3.Name = "tbFormula3";
            this.tbFormula3.Size = new System.Drawing.Size(408, 22);
            this.tbFormula3.TabIndex = 34;
            // 
            // lblUsingColorScheme2
            // 
            this.lblUsingColorScheme2.AutoSize = true;
            this.lblUsingColorScheme2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsingColorScheme2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUsingColorScheme2.Location = new System.Drawing.Point(527, 388);
            this.lblUsingColorScheme2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsingColorScheme2.Name = "lblUsingColorScheme2";
            this.lblUsingColorScheme2.Size = new System.Drawing.Size(200, 37);
            this.lblUsingColorScheme2.TabIndex = 41;
            this.lblUsingColorScheme2.Text = "Color scheme:";
            this.lblUsingColorScheme2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUsingColorScheme3
            // 
            this.lblUsingColorScheme3.AutoSize = true;
            this.lblUsingColorScheme3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsingColorScheme3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUsingColorScheme3.Location = new System.Drawing.Point(1050, 388);
            this.lblUsingColorScheme3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsingColorScheme3.Name = "lblUsingColorScheme3";
            this.lblUsingColorScheme3.Size = new System.Drawing.Size(200, 37);
            this.lblUsingColorScheme3.TabIndex = 42;
            this.lblUsingColorScheme3.Text = "Color scheme:";
            this.lblUsingColorScheme3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbColorSchemePath2
            // 
            this.tbColorSchemePath2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbColorSchemePath2.Location = new System.Drawing.Point(735, 392);
            this.tbColorSchemePath2.Margin = new System.Windows.Forms.Padding(4);
            this.tbColorSchemePath2.Name = "tbColorSchemePath2";
            this.tbColorSchemePath2.Size = new System.Drawing.Size(200, 22);
            this.tbColorSchemePath2.TabIndex = 44;
            // 
            // tbColorSchemePath3
            // 
            this.tbColorSchemePath3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbColorSchemePath3.Location = new System.Drawing.Point(1258, 392);
            this.tbColorSchemePath3.Margin = new System.Windows.Forms.Padding(4);
            this.tbColorSchemePath3.Name = "tbColorSchemePath3";
            this.tbColorSchemePath3.Size = new System.Drawing.Size(200, 22);
            this.tbColorSchemePath3.TabIndex = 45;
            // 
            // btnBrowseColorScheme2
            // 
            this.btnBrowseColorScheme2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseColorScheme2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseColorScheme2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseColorScheme2.Location = new System.Drawing.Point(943, 392);
            this.btnBrowseColorScheme2.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseColorScheme2.Name = "btnBrowseColorScheme2";
            this.btnBrowseColorScheme2.Size = new System.Drawing.Size(99, 29);
            this.btnBrowseColorScheme2.TabIndex = 47;
            this.btnBrowseColorScheme2.Text = ". . .";
            this.btnBrowseColorScheme2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBrowseColorScheme2.UseVisualStyleBackColor = true;
            this.btnBrowseColorScheme2.Click += new System.EventHandler(this.btnBrowseColorScheme1_Click);
            // 
            // btnBrowseColorScheme3
            // 
            this.btnBrowseColorScheme3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseColorScheme3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseColorScheme3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseColorScheme3.Location = new System.Drawing.Point(1466, 392);
            this.btnBrowseColorScheme3.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseColorScheme3.Name = "btnBrowseColorScheme3";
            this.btnBrowseColorScheme3.Size = new System.Drawing.Size(121, 29);
            this.btnBrowseColorScheme3.TabIndex = 48;
            this.btnBrowseColorScheme3.Text = ". . .";
            this.btnBrowseColorScheme3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBrowseColorScheme3.UseVisualStyleBackColor = true;
            this.btnBrowseColorScheme3.Click += new System.EventHandler(this.btnBrowseColorScheme1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tableLayoutPanel1.Controls.Add(this.btnSaveDataRes3, 7, 14);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveDataRes2, 4, 14);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowseColorScheme3, 8, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowseColorScheme2, 5, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbColorSchemePath3, 7, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbColorSchemePath2, 4, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblUsingColorScheme3, 6, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblUsingColorScheme2, 3, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbFormula3, 6, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnOK3, 8, 7);
            this.tableLayoutPanel1.Controls.Add(this.pbRes3, 6, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblResult3, 6, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbFormula2, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnOK2, 5, 7);
            this.tableLayoutPanel1.Controls.Add(this.pbRes2, 3, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblResult2, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblResult1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblBlue, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSkyIndex, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbChannel, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSourceImage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbFormula1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnOK1, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.pbRes1, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbStats1, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.tbStats2, 3, 15);
            this.tableLayoutPanel1.Controls.Add(this.tbStats3, 6, 15);
            this.tableLayoutPanel1.Controls.Add(this.lblUsingColorScheme1, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbColorSchemePath1, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowseColorScheme1, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.pbScale, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.pbRes1Scale, 2, 10);
            this.tableLayoutPanel1.Controls.Add(this.pbRes2Scale, 5, 10);
            this.tableLayoutPanel1.Controls.Add(this.pbRes3Scale, 8, 10);
            this.tableLayoutPanel1.Controls.Add(this.tbRes1Thresh, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbRes2Thresh, 5, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbRes3Thresh, 8, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbChannelThresh, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox7, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.pbSourceImage, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pbSkyIndexImage, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.pbSkyIndexScale, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.chbRes1DynamicScale, 2, 14);
            this.tableLayoutPanel1.Controls.Add(this.chbRes2DynamicScale, 5, 14);
            this.tableLayoutPanel1.Controls.Add(this.chbRes3DynamicScale, 8, 14);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveDataRes1, 1, 14);
            this.tableLayoutPanel1.Controls.Add(this.btnTestProcess, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveDataSkyIndexData, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnRes1Hist, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnRes2Hist, 5, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnRes3Hist, 8, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnSectionProfile1, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.btnSectionProfile2, 3, 14);
            this.tableLayoutPanel1.Controls.Add(this.btnSectionProfile3, 6, 14);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 16;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1591, 850);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnSaveDataRes3
            // 
            this.btnSaveDataRes3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveDataRes3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDataRes3.Location = new System.Drawing.Point(1258, 728);
            this.btnSaveDataRes3.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveDataRes3.Name = "btnSaveDataRes3";
            this.btnSaveDataRes3.Size = new System.Drawing.Size(200, 29);
            this.btnSaveDataRes3.TabIndex = 76;
            this.btnSaveDataRes3.Text = "save data...";
            this.btnSaveDataRes3.UseVisualStyleBackColor = true;
            this.btnSaveDataRes3.Click += new System.EventHandler(this.btnSaveDataRes_Click);
            // 
            // btnSaveDataRes2
            // 
            this.btnSaveDataRes2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveDataRes2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDataRes2.Location = new System.Drawing.Point(735, 728);
            this.btnSaveDataRes2.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveDataRes2.Name = "btnSaveDataRes2";
            this.btnSaveDataRes2.Size = new System.Drawing.Size(200, 29);
            this.btnSaveDataRes2.TabIndex = 75;
            this.btnSaveDataRes2.Text = "save data...";
            this.btnSaveDataRes2.UseVisualStyleBackColor = true;
            this.btnSaveDataRes2.Click += new System.EventHandler(this.btnSaveDataRes_Click);
            // 
            // pbRes3
            // 
            this.pbRes3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbRes3, 2);
            this.pbRes3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRes3.Location = new System.Drawing.Point(1050, 429);
            this.pbRes3.Margin = new System.Windows.Forms.Padding(4);
            this.pbRes3.Name = "pbRes3";
            this.tableLayoutPanel1.SetRowSpan(this.pbRes3, 5);
            this.pbRes3.Size = new System.Drawing.Size(408, 291);
            this.pbRes3.TabIndex = 36;
            this.pbRes3.TabStop = false;
            this.pbRes3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbImageData_MouseDoubleClick);
            this.pbRes3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseDown);
            this.pbRes3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbChannel_MouseMove);
            this.pbRes3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseUp);
            // 
            // pbRes2
            // 
            this.pbRes2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbRes2, 2);
            this.pbRes2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRes2.Location = new System.Drawing.Point(527, 429);
            this.pbRes2.Margin = new System.Windows.Forms.Padding(4);
            this.pbRes2.Name = "pbRes2";
            this.tableLayoutPanel1.SetRowSpan(this.pbRes2, 5);
            this.pbRes2.Size = new System.Drawing.Size(408, 291);
            this.pbRes2.TabIndex = 32;
            this.pbRes2.TabStop = false;
            this.pbRes2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbImageData_MouseDoubleClick);
            this.pbRes2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseDown);
            this.pbRes2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbChannel_MouseMove);
            this.pbRes2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseUp);
            // 
            // pbChannel
            // 
            this.pbChannel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbChannel, 2);
            this.pbChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbChannel.Location = new System.Drawing.Point(1050, 41);
            this.pbChannel.Margin = new System.Windows.Forms.Padding(4);
            this.pbChannel.Name = "pbChannel";
            this.tableLayoutPanel1.SetRowSpan(this.pbChannel, 4);
            this.pbChannel.Size = new System.Drawing.Size(408, 232);
            this.pbChannel.TabIndex = 0;
            this.pbChannel.TabStop = false;
            this.pbChannel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbImageData_MouseDoubleClick);
            this.pbChannel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.pbChannel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbChannel_MouseMove);
            this.pbChannel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_MouseUp);
            // 
            // pbRes1
            // 
            this.pbRes1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbRes1, 2);
            this.pbRes1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRes1.Location = new System.Drawing.Point(4, 429);
            this.pbRes1.Margin = new System.Windows.Forms.Padding(4);
            this.pbRes1.Name = "pbRes1";
            this.tableLayoutPanel1.SetRowSpan(this.pbRes1, 5);
            this.pbRes1.Size = new System.Drawing.Size(408, 291);
            this.pbRes1.TabIndex = 28;
            this.pbRes1.TabStop = false;
            this.pbRes1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbImageData_MouseDoubleClick);
            this.pbRes1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseDown);
            this.pbRes1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbChannel_MouseMove);
            this.pbRes1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseUp);
            // 
            // pbScale
            // 
            this.pbScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbScale.Location = new System.Drawing.Point(1466, 41);
            this.pbScale.Margin = new System.Windows.Forms.Padding(4);
            this.pbScale.Name = "pbScale";
            this.tableLayoutPanel1.SetRowSpan(this.pbScale, 4);
            this.pbScale.Size = new System.Drawing.Size(121, 232);
            this.pbScale.TabIndex = 49;
            this.pbScale.TabStop = false;
            this.pbScale.Click += new System.EventHandler(this.pbScale_Click);
            this.pbScale.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbResScale_MouseClick);
            this.pbScale.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbScale_MouseDoubleClick);
            this.pbScale.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.pbScale.MouseLeave += new System.EventHandler(this.pbScale_MouseLeave);
            this.pbScale.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbScale_MouseMove);
            this.pbScale.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_MouseUp);
            // 
            // pbRes1Scale
            // 
            this.pbRes1Scale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRes1Scale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRes1Scale.Location = new System.Drawing.Point(420, 454);
            this.pbRes1Scale.Margin = new System.Windows.Forms.Padding(4);
            this.pbRes1Scale.Name = "pbRes1Scale";
            this.tableLayoutPanel1.SetRowSpan(this.pbRes1Scale, 4);
            this.pbRes1Scale.Size = new System.Drawing.Size(99, 266);
            this.pbRes1Scale.TabIndex = 52;
            this.pbRes1Scale.TabStop = false;
            this.pbRes1Scale.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbResScale_MouseClick);
            this.pbRes1Scale.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbRes1Scale_MouseDoubleClick);
            this.pbRes1Scale.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.pbRes1Scale.MouseLeave += new System.EventHandler(this.pbScale_MouseLeave);
            this.pbRes1Scale.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbScale_MouseMove);
            this.pbRes1Scale.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_MouseUp);
            // 
            // pbRes2Scale
            // 
            this.pbRes2Scale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRes2Scale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRes2Scale.Location = new System.Drawing.Point(943, 454);
            this.pbRes2Scale.Margin = new System.Windows.Forms.Padding(4);
            this.pbRes2Scale.Name = "pbRes2Scale";
            this.tableLayoutPanel1.SetRowSpan(this.pbRes2Scale, 4);
            this.pbRes2Scale.Size = new System.Drawing.Size(99, 266);
            this.pbRes2Scale.TabIndex = 53;
            this.pbRes2Scale.TabStop = false;
            this.pbRes2Scale.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbResScale_MouseClick);
            this.pbRes2Scale.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbRes1Scale_MouseDoubleClick);
            this.pbRes2Scale.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.pbRes2Scale.MouseLeave += new System.EventHandler(this.pbScale_MouseLeave);
            this.pbRes2Scale.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbScale_MouseMove);
            this.pbRes2Scale.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_MouseUp);
            // 
            // pbRes3Scale
            // 
            this.pbRes3Scale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRes3Scale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRes3Scale.Location = new System.Drawing.Point(1466, 454);
            this.pbRes3Scale.Margin = new System.Windows.Forms.Padding(4);
            this.pbRes3Scale.Name = "pbRes3Scale";
            this.tableLayoutPanel1.SetRowSpan(this.pbRes3Scale, 4);
            this.pbRes3Scale.Size = new System.Drawing.Size(121, 266);
            this.pbRes3Scale.TabIndex = 54;
            this.pbRes3Scale.TabStop = false;
            this.pbRes3Scale.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbResScale_MouseClick);
            this.pbRes3Scale.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbRes1Scale_MouseDoubleClick);
            this.pbRes3Scale.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.pbRes3Scale.MouseLeave += new System.EventHandler(this.pbScale_MouseLeave);
            this.pbRes3Scale.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbScale_MouseMove);
            this.pbRes3Scale.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_MouseUp);
            // 
            // pbSourceImage
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbSourceImage, 2);
            this.pbSourceImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSourceImage.Location = new System.Drawing.Point(4, 41);
            this.pbSourceImage.Margin = new System.Windows.Forms.Padding(4);
            this.pbSourceImage.Name = "pbSourceImage";
            this.tableLayoutPanel1.SetRowSpan(this.pbSourceImage, 4);
            this.pbSourceImage.Size = new System.Drawing.Size(408, 232);
            this.pbSourceImage.TabIndex = 68;
            this.pbSourceImage.TabStop = false;
            // 
            // pbSkyIndexImage
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbSkyIndexImage, 2);
            this.pbSkyIndexImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSkyIndexImage.Location = new System.Drawing.Point(527, 41);
            this.pbSkyIndexImage.Margin = new System.Windows.Forms.Padding(4);
            this.pbSkyIndexImage.Name = "pbSkyIndexImage";
            this.tableLayoutPanel1.SetRowSpan(this.pbSkyIndexImage, 4);
            this.pbSkyIndexImage.Size = new System.Drawing.Size(408, 232);
            this.pbSkyIndexImage.TabIndex = 69;
            this.pbSkyIndexImage.TabStop = false;
            // 
            // pbSkyIndexScale
            // 
            this.pbSkyIndexScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSkyIndexScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSkyIndexScale.Location = new System.Drawing.Point(943, 41);
            this.pbSkyIndexScale.Margin = new System.Windows.Forms.Padding(4);
            this.pbSkyIndexScale.Name = "pbSkyIndexScale";
            this.tableLayoutPanel1.SetRowSpan(this.pbSkyIndexScale, 4);
            this.pbSkyIndexScale.Size = new System.Drawing.Size(99, 232);
            this.pbSkyIndexScale.TabIndex = 70;
            this.pbSkyIndexScale.TabStop = false;
            // 
            // btnSaveDataRes1
            // 
            this.btnSaveDataRes1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveDataRes1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDataRes1.Location = new System.Drawing.Point(212, 728);
            this.btnSaveDataRes1.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveDataRes1.Name = "btnSaveDataRes1";
            this.btnSaveDataRes1.Size = new System.Drawing.Size(200, 29);
            this.btnSaveDataRes1.TabIndex = 74;
            this.btnSaveDataRes1.Text = "save data...";
            this.btnSaveDataRes1.UseVisualStyleBackColor = true;
            this.btnSaveDataRes1.Click += new System.EventHandler(this.btnSaveDataRes_Click);
            // 
            // btnTestProcess
            // 
            this.btnTestProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTestProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestProcess.Location = new System.Drawing.Point(420, 41);
            this.btnTestProcess.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestProcess.Name = "btnTestProcess";
            this.btnTestProcess.Size = new System.Drawing.Size(99, 75);
            this.btnTestProcess.TabIndex = 77;
            this.btnTestProcess.Text = "test FFT";
            this.btnTestProcess.UseVisualStyleBackColor = true;
            this.btnTestProcess.Click += new System.EventHandler(this.btnTestProcess_Click);
            // 
            // btnSaveDataSkyIndexData
            // 
            this.btnSaveDataSkyIndexData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveDataSkyIndexData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDataSkyIndexData.Location = new System.Drawing.Point(943, 281);
            this.btnSaveDataSkyIndexData.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveDataSkyIndexData.Name = "btnSaveDataSkyIndexData";
            this.btnSaveDataSkyIndexData.Size = new System.Drawing.Size(99, 29);
            this.btnSaveDataSkyIndexData.TabIndex = 78;
            this.btnSaveDataSkyIndexData.Text = "save data...";
            this.btnSaveDataSkyIndexData.UseVisualStyleBackColor = true;
            this.btnSaveDataSkyIndexData.Click += new System.EventHandler(this.btnSaveDataRes_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(420, 124);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 75);
            this.button1.TabIndex = 79;
            this.button1.Text = "test Grad";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRes1Hist
            // 
            this.btnRes1Hist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRes1Hist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRes1Hist.Location = new System.Drawing.Point(420, 318);
            this.btnRes1Hist.Margin = new System.Windows.Forms.Padding(4);
            this.btnRes1Hist.Name = "btnRes1Hist";
            this.btnRes1Hist.Size = new System.Drawing.Size(99, 29);
            this.btnRes1Hist.TabIndex = 80;
            this.btnRes1Hist.Text = "histogram";
            this.btnRes1Hist.UseVisualStyleBackColor = true;
            this.btnRes1Hist.Click += new System.EventHandler(this.btnRes1Hist_Click);
            // 
            // btnRes2Hist
            // 
            this.btnRes2Hist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRes2Hist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRes2Hist.Location = new System.Drawing.Point(943, 318);
            this.btnRes2Hist.Margin = new System.Windows.Forms.Padding(4);
            this.btnRes2Hist.Name = "btnRes2Hist";
            this.btnRes2Hist.Size = new System.Drawing.Size(99, 29);
            this.btnRes2Hist.TabIndex = 81;
            this.btnRes2Hist.Text = "histogram";
            this.btnRes2Hist.UseVisualStyleBackColor = true;
            this.btnRes2Hist.Click += new System.EventHandler(this.btnRes1Hist_Click);
            // 
            // btnRes3Hist
            // 
            this.btnRes3Hist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRes3Hist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRes3Hist.Location = new System.Drawing.Point(1466, 318);
            this.btnRes3Hist.Margin = new System.Windows.Forms.Padding(4);
            this.btnRes3Hist.Name = "btnRes3Hist";
            this.btnRes3Hist.Size = new System.Drawing.Size(121, 29);
            this.btnRes3Hist.TabIndex = 82;
            this.btnRes3Hist.Text = "histogram";
            this.btnRes3Hist.UseVisualStyleBackColor = true;
            this.btnRes3Hist.Click += new System.EventHandler(this.btnRes1Hist_Click);
            // 
            // btnSectionProfile1
            // 
            this.btnSectionProfile1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSectionProfile1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectionProfile1.Location = new System.Drawing.Point(4, 728);
            this.btnSectionProfile1.Margin = new System.Windows.Forms.Padding(4);
            this.btnSectionProfile1.Name = "btnSectionProfile1";
            this.btnSectionProfile1.Size = new System.Drawing.Size(200, 29);
            this.btnSectionProfile1.TabIndex = 83;
            this.btnSectionProfile1.Text = "Section profile...";
            this.btnSectionProfile1.UseVisualStyleBackColor = true;
            this.btnSectionProfile1.Click += new System.EventHandler(this.buttonSectionProfile_Click);
            // 
            // btnSectionProfile2
            // 
            this.btnSectionProfile2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSectionProfile2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectionProfile2.Location = new System.Drawing.Point(527, 728);
            this.btnSectionProfile2.Margin = new System.Windows.Forms.Padding(4);
            this.btnSectionProfile2.Name = "btnSectionProfile2";
            this.btnSectionProfile2.Size = new System.Drawing.Size(200, 29);
            this.btnSectionProfile2.TabIndex = 84;
            this.btnSectionProfile2.Text = "Section profile...";
            this.btnSectionProfile2.UseVisualStyleBackColor = true;
            this.btnSectionProfile2.Click += new System.EventHandler(this.buttonSectionProfile_Click);
            // 
            // btnSectionProfile3
            // 
            this.btnSectionProfile3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSectionProfile3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectionProfile3.Location = new System.Drawing.Point(1050, 728);
            this.btnSectionProfile3.Margin = new System.Windows.Forms.Padding(4);
            this.btnSectionProfile3.Name = "btnSectionProfile3";
            this.btnSectionProfile3.Size = new System.Drawing.Size(200, 29);
            this.btnSectionProfile3.TabIndex = 85;
            this.btnSectionProfile3.Text = "Section profile...";
            this.btnSectionProfile3.UseVisualStyleBackColor = true;
            this.btnSectionProfile3.Click += new System.EventHandler(this.buttonSectionProfile_Click);
            // 
            // SkyImagesAnalyzer_ColorsManipulatingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1591, 850);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SkyImagesAnalyzer_ColorsManipulatingForm";
            this.Text = "SkyIndexAnalyzing_ColorsManipulatingForm";
            this.Load += new System.EventHandler(this.SkyIndexAnalyzing_ColorsManipulatingForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SkyIndexAnalyzing_ColorsManipulatingForm_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SkyIndexAnalyzing_ColorsManipulatingForm_KeyPress);
            this.Resize += new System.EventHandler(this.SkyIndexAnalyzing_ColorsManipulatingForm_Resize);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes1Scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes2Scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes3Scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSourceImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkyIndexImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkyIndexScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgwSkyIndexAnalyzer;
        private System.Windows.Forms.CheckBox chbRes3DynamicScale;
        private System.Windows.Forms.CheckBox chbRes2DynamicScale;
        private System.Windows.Forms.CheckBox chbRes1DynamicScale;
        private System.Windows.Forms.PictureBox pbSkyIndexScale;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnBrowseColorScheme3;
        private System.Windows.Forms.Button btnBrowseColorScheme2;
        private System.Windows.Forms.TextBox tbColorSchemePath3;
        private System.Windows.Forms.TextBox tbColorSchemePath2;
        private System.Windows.Forms.Label lblUsingColorScheme3;
        private System.Windows.Forms.Label lblUsingColorScheme2;
        private System.Windows.Forms.TextBox tbFormula3;
        private System.Windows.Forms.Button btnOK3;
        private System.Windows.Forms.PictureBox pbRes3;
        private System.Windows.Forms.Label lblResult3;
        private System.Windows.Forms.TextBox tbFormula2;
        private System.Windows.Forms.Button btnOK2;
        private System.Windows.Forms.PictureBox pbRes2;
        private System.Windows.Forms.Label lblResult2;
        private System.Windows.Forms.Label lblResult1;
        private System.Windows.Forms.Label lblBlue;
        private System.Windows.Forms.Label lblSkyIndex;
        private System.Windows.Forms.PictureBox pbChannel;
        private System.Windows.Forms.Label lblSourceImage;
        private System.Windows.Forms.TextBox tbFormula1;
        private System.Windows.Forms.Button btnOK1;
        private System.Windows.Forms.PictureBox pbRes1;
        private System.Windows.Forms.TextBox tbStats1;
        private System.Windows.Forms.TextBox tbStats2;
        private System.Windows.Forms.TextBox tbStats3;
        private System.Windows.Forms.Label lblUsingColorScheme1;
        private System.Windows.Forms.TextBox tbColorSchemePath1;
        private System.Windows.Forms.Button btnBrowseColorScheme1;
        private System.Windows.Forms.PictureBox pbScale;
        private System.Windows.Forms.PictureBox pbRes1Scale;
        private System.Windows.Forms.PictureBox pbRes2Scale;
        private System.Windows.Forms.PictureBox pbRes3Scale;
        private System.Windows.Forms.TextBox tbRes1Thresh;
        private System.Windows.Forms.TextBox tbRes2Thresh;
        private System.Windows.Forms.TextBox tbRes3Thresh;
        private System.Windows.Forms.TextBox tbChannelThresh;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rbtnSwitchChannelB;
        private System.Windows.Forms.RadioButton rbtnSwitchChannelG;
        private System.Windows.Forms.RadioButton rbtnSwitchChannelR;
        private System.Windows.Forms.PictureBox pbSourceImage;
        private System.Windows.Forms.PictureBox pbSkyIndexImage;
        private System.Windows.Forms.Button btnSaveDataRes1;
        private System.Windows.Forms.Button btnSaveDataRes3;
        private System.Windows.Forms.Button btnSaveDataRes2;
        private System.Windows.Forms.Button btnSaveDataSkyIndexData;
        private System.Windows.Forms.Button btnTestProcess;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnRes1Hist;
        private System.Windows.Forms.Button btnRes2Hist;
        private System.Windows.Forms.Button btnRes3Hist;
        private System.Windows.Forms.Button btnSectionProfile1;
        private System.Windows.Forms.Button btnSectionProfile2;
        private System.Windows.Forms.Button btnSectionProfile3;
    }
}