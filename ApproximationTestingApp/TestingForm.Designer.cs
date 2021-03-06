﻿namespace ApproximationTestingApp
{
    partial class TestingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestingForm));
            this.button1 = new System.Windows.Forms.Button();
            this.btnTestApprox = new System.Windows.Forms.Button();
            this.bgwApproximatorWorker = new System.ComponentModel.BackgroundWorker();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbUseWeights = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblCurrentWorkingDir = new System.Windows.Forms.Label();
            this.lblCurrWorkingDir = new System.Windows.Forms.Label();
            this.cbUseConstraints = new System.Windows.Forms.CheckBox();
            this.btnInitEvenlopData = new System.Windows.Forms.Button();
            this.btnApproxEvenlop = new System.Windows.Forms.Button();
            this.btnInitAndApproximateTestMKL = new System.Windows.Forms.Button();
            this.rtbDataFilesPrefixMKL = new System.Windows.Forms.RichTextBox();
            this.tbDataFilePrefix = new System.Windows.Forms.RichTextBox();
            this.btnReadWeibullData = new System.Windows.Forms.Button();
            this.rtbDataFilePath = new System.Windows.Forms.RichTextBox();
            this.btnProperties = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gainsboro;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(4, 114);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(276, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "initialize data";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTestApprox
            // 
            this.btnTestApprox.BackColor = System.Drawing.Color.Gainsboro;
            this.btnTestApprox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTestApprox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestApprox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestApprox.Location = new System.Drawing.Point(572, 114);
            this.btnTestApprox.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestApprox.Name = "btnTestApprox";
            this.btnTestApprox.Size = new System.Drawing.Size(276, 42);
            this.btnTestApprox.TabIndex = 1;
            this.btnTestApprox.Text = "approximation test";
            this.btnTestApprox.UseVisualStyleBackColor = false;
            this.btnTestApprox.Click += new System.EventHandler(this.btnTestApprox_Click);
            // 
            // bgwApproximatorWorker
            // 
            this.bgwApproximatorWorker.WorkerReportsProgress = true;
            this.bgwApproximatorWorker.WorkerSupportsCancellation = true;
            this.bgwApproximatorWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwApproximatorWorker_DoWork);
            this.bgwApproximatorWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwApproximatorWorker_ProgressChanged);
            this.bgwApproximatorWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwApproximatorWorker_RunWorkerCompleted);
            // 
            // tbLog
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbLog, 6);
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLog.Location = new System.Drawing.Point(4, 364);
            this.tbLog.Margin = new System.Windows.Forms.Padding(4);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tableLayoutPanel1.SetRowSpan(this.tbLog, 4);
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(1472, 699);
            this.tbLog.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatus, 2);
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(1140, 160);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(336, 50);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "0";
            // 
            // cbUseWeights
            // 
            this.cbUseWeights.AutoSize = true;
            this.cbUseWeights.Checked = true;
            this.cbUseWeights.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseWeights.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUseWeights.Location = new System.Drawing.Point(856, 114);
            this.cbUseWeights.Margin = new System.Windows.Forms.Padding(4);
            this.cbUseWeights.Name = "cbUseWeights";
            this.cbUseWeights.Size = new System.Drawing.Size(163, 33);
            this.cbUseWeights.TabIndex = 4;
            this.cbUseWeights.Text = "use weights";
            this.cbUseWeights.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.tbLog, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnTestApprox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbUseWeights, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentWorkingDir, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrWorkingDir, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbUseConstraints, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnInitEvenlopData, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnApproxEvenlop, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnInitAndApproximateTestMKL, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.rtbDataFilesPrefixMKL, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbDataFilePrefix, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnReadWeibullData, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.rtbDataFilePath, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 5, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1480, 1067);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // lblCurrentWorkingDir
            // 
            this.lblCurrentWorkingDir.AutoSize = true;
            this.lblCurrentWorkingDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentWorkingDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCurrentWorkingDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentWorkingDir.Location = new System.Drawing.Point(4, 60);
            this.lblCurrentWorkingDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentWorkingDir.Name = "lblCurrentWorkingDir";
            this.lblCurrentWorkingDir.Size = new System.Drawing.Size(276, 50);
            this.lblCurrentWorkingDir.TabIndex = 16;
            this.lblCurrentWorkingDir.Text = "current working directory:";
            this.lblCurrentWorkingDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrWorkingDir
            // 
            this.lblCurrWorkingDir.AutoSize = true;
            this.lblCurrWorkingDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCurrWorkingDir, 5);
            this.lblCurrWorkingDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrWorkingDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCurrWorkingDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrWorkingDir.Location = new System.Drawing.Point(288, 60);
            this.lblCurrWorkingDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrWorkingDir.Name = "lblCurrWorkingDir";
            this.lblCurrWorkingDir.Size = new System.Drawing.Size(1188, 50);
            this.lblCurrWorkingDir.TabIndex = 17;
            this.lblCurrWorkingDir.Text = "Current working path";
            this.lblCurrWorkingDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbUseConstraints
            // 
            this.cbUseConstraints.AutoSize = true;
            this.cbUseConstraints.Checked = true;
            this.cbUseConstraints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.cbUseConstraints, 2);
            this.cbUseConstraints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbUseConstraints.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUseConstraints.Location = new System.Drawing.Point(1139, 113);
            this.cbUseConstraints.Name = "cbUseConstraints";
            this.cbUseConstraints.Size = new System.Drawing.Size(338, 44);
            this.cbUseConstraints.TabIndex = 19;
            this.cbUseConstraints.Text = "use constraints";
            this.cbUseConstraints.UseVisualStyleBackColor = true;
            // 
            // btnInitEvenlopData
            // 
            this.btnInitEvenlopData.BackColor = System.Drawing.Color.Gainsboro;
            this.btnInitEvenlopData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInitEvenlopData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInitEvenlopData.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitEvenlopData.Location = new System.Drawing.Point(3, 163);
            this.btnInitEvenlopData.Name = "btnInitEvenlopData";
            this.btnInitEvenlopData.Size = new System.Drawing.Size(278, 44);
            this.btnInitEvenlopData.TabIndex = 21;
            this.btnInitEvenlopData.Text = "initialize evenlop data";
            this.btnInitEvenlopData.UseVisualStyleBackColor = false;
            this.btnInitEvenlopData.Click += new System.EventHandler(this.btnInitEvenlopData_Click);
            // 
            // btnApproxEvenlop
            // 
            this.btnApproxEvenlop.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnApproxEvenlop, 2);
            this.btnApproxEvenlop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApproxEvenlop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApproxEvenlop.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApproxEvenlop.Location = new System.Drawing.Point(571, 163);
            this.btnApproxEvenlop.Name = "btnApproxEvenlop";
            this.btnApproxEvenlop.Size = new System.Drawing.Size(562, 44);
            this.btnApproxEvenlop.TabIndex = 22;
            this.btnApproxEvenlop.Text = "evenlop approx test";
            this.btnApproxEvenlop.UseVisualStyleBackColor = false;
            this.btnApproxEvenlop.Click += new System.EventHandler(this.btnApproxEvenlop_Click);
            // 
            // btnInitAndApproximateTestMKL
            // 
            this.btnInitAndApproximateTestMKL.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnInitAndApproximateTestMKL, 3);
            this.btnInitAndApproximateTestMKL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInitAndApproximateTestMKL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInitAndApproximateTestMKL.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitAndApproximateTestMKL.Location = new System.Drawing.Point(3, 213);
            this.btnInitAndApproximateTestMKL.Name = "btnInitAndApproximateTestMKL";
            this.btnInitAndApproximateTestMKL.Size = new System.Drawing.Size(846, 44);
            this.btnInitAndApproximateTestMKL.TabIndex = 23;
            this.btnInitAndApproximateTestMKL.Text = "init data and perform approximation for MKL";
            this.btnInitAndApproximateTestMKL.UseVisualStyleBackColor = false;
            this.btnInitAndApproximateTestMKL.Click += new System.EventHandler(this.btnInitDataForintelMKL_Click);
            // 
            // rtbDataFilesPrefixMKL
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rtbDataFilesPrefixMKL, 3);
            this.rtbDataFilesPrefixMKL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDataFilesPrefixMKL.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDataFilesPrefixMKL.Location = new System.Drawing.Point(855, 213);
            this.rtbDataFilesPrefixMKL.Name = "rtbDataFilesPrefixMKL";
            this.rtbDataFilesPrefixMKL.Size = new System.Drawing.Size(622, 44);
            this.rtbDataFilesPrefixMKL.TabIndex = 24;
            this.rtbDataFilesPrefixMKL.Text = "";
            // 
            // tbDataFilePrefix
            // 
            this.tbDataFilePrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDataFilePrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDataFilePrefix.Location = new System.Drawing.Point(287, 113);
            this.tbDataFilePrefix.Name = "tbDataFilePrefix";
            this.tbDataFilePrefix.Size = new System.Drawing.Size(278, 44);
            this.tbDataFilePrefix.TabIndex = 29;
            this.tbDataFilePrefix.Text = "";
            // 
            // btnReadWeibullData
            // 
            this.btnReadWeibullData.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnReadWeibullData, 3);
            this.btnReadWeibullData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadWeibullData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadWeibullData.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadWeibullData.Location = new System.Drawing.Point(3, 313);
            this.btnReadWeibullData.Name = "btnReadWeibullData";
            this.btnReadWeibullData.Size = new System.Drawing.Size(846, 44);
            this.btnReadWeibullData.TabIndex = 30;
            this.btnReadWeibullData.Text = "read and approximate Weibull";
            this.btnReadWeibullData.UseVisualStyleBackColor = false;
            this.btnReadWeibullData.Click += new System.EventHandler(this.btnReadWeibullData_Click);
            // 
            // rtbDataFilePath
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rtbDataFilePath, 3);
            this.rtbDataFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDataFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDataFilePath.Location = new System.Drawing.Point(855, 313);
            this.rtbDataFilePath.Name = "rtbDataFilePath";
            this.rtbDataFilePath.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbDataFilePath.Size = new System.Drawing.Size(622, 44);
            this.rtbDataFilePath.TabIndex = 31;
            this.rtbDataFilePath.Text = "D:\\_gulevlab\\Weibull-Gavr\\test.dat";
            // 
            // btnProperties
            // 
            this.btnProperties.BackColor = System.Drawing.Color.Gainsboro;
            this.btnProperties.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProperties.BackgroundImage")));
            this.btnProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Location = new System.Drawing.Point(1423, 3);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(54, 54);
            this.btnProperties.TabIndex = 32;
            this.btnProperties.UseVisualStyleBackColor = false;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click_1);
            // 
            // TestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1480, 1067);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TestingForm";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.TestingForm_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TestingForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnTestApprox;
        private System.ComponentModel.BackgroundWorker bgwApproximatorWorker;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox cbUseWeights;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblCurrentWorkingDir;
        private System.Windows.Forms.Label lblCurrWorkingDir;
        private System.Windows.Forms.CheckBox cbUseConstraints;
        private System.Windows.Forms.Button btnInitEvenlopData;
        private System.Windows.Forms.Button btnApproxEvenlop;
        private System.Windows.Forms.Button btnInitAndApproximateTestMKL;
        private System.Windows.Forms.RichTextBox rtbDataFilesPrefixMKL;
        private System.Windows.Forms.RichTextBox tbDataFilePrefix;
        private System.Windows.Forms.Button btnReadWeibullData;
        private System.Windows.Forms.RichTextBox rtbDataFilePath;
        private System.Windows.Forms.Button btnProperties;
    }
}

