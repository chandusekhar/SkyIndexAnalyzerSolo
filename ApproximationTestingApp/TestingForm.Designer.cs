namespace ApproximationTestingApp
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnTestApprox = new System.Windows.Forms.Button();
            this.bgwApproximatorWorker = new System.ComponentModel.BackgroundWorker();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbUseWeights = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbDataFilePrefix = new System.Windows.Forms.TextBox();
            this.lblCurrentWorkingDir = new System.Windows.Forms.Label();
            this.lblCurrWorkingDir = new System.Windows.Forms.Label();
            this.btnProperties = new System.Windows.Forms.Button();
            this.cbUseConstraints = new System.Windows.Forms.CheckBox();
            this.btnInitEvenlopData = new System.Windows.Forms.Button();
            this.btnApproxEvenlop = new System.Windows.Forms.Button();
            this.btnInitDataForintelMKL = new System.Windows.Forms.Button();
            this.rtbDataFilesPrefixMKL = new System.Windows.Forms.RichTextBox();
            this.btnApproximateMKL = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(4, 54);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(288, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "initialize data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTestApprox
            // 
            this.btnTestApprox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTestApprox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestApprox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestApprox.Location = new System.Drawing.Point(596, 54);
            this.btnTestApprox.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestApprox.Name = "btnTestApprox";
            this.btnTestApprox.Size = new System.Drawing.Size(288, 42);
            this.btnTestApprox.TabIndex = 1;
            this.btnTestApprox.Text = "approximation test";
            this.btnTestApprox.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanel1.SetColumnSpan(this.tbLog, 5);
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLog.Location = new System.Drawing.Point(4, 204);
            this.tbLog.Margin = new System.Windows.Forms.Padding(4);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tableLayoutPanel1.SetRowSpan(this.tbLog, 4);
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(1472, 859);
            this.tbLog.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(892, 100);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(288, 50);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "0";
            // 
            // cbUseWeights
            // 
            this.cbUseWeights.AutoSize = true;
            this.cbUseWeights.Checked = true;
            this.cbUseWeights.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseWeights.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUseWeights.Location = new System.Drawing.Point(892, 54);
            this.cbUseWeights.Margin = new System.Windows.Forms.Padding(4);
            this.cbUseWeights.Name = "cbUseWeights";
            this.cbUseWeights.Size = new System.Drawing.Size(163, 33);
            this.cbUseWeights.TabIndex = 4;
            this.cbUseWeights.Text = "use weights";
            this.cbUseWeights.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tbLog, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTestApprox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbUseWeights, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbDataFilePrefix, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentWorkingDir, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrWorkingDir, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbUseConstraints, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnInitEvenlopData, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnApproxEvenlop, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnInitDataForintelMKL, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.rtbDataFilesPrefixMKL, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnApproximateMKL, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
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
            // tbDataFilePrefix
            // 
            this.tbDataFilePrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDataFilePrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDataFilePrefix.Location = new System.Drawing.Point(300, 54);
            this.tbDataFilePrefix.Margin = new System.Windows.Forms.Padding(4);
            this.tbDataFilePrefix.Name = "tbDataFilePrefix";
            this.tbDataFilePrefix.Size = new System.Drawing.Size(288, 34);
            this.tbDataFilePrefix.TabIndex = 15;
            // 
            // lblCurrentWorkingDir
            // 
            this.lblCurrentWorkingDir.AutoSize = true;
            this.lblCurrentWorkingDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentWorkingDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCurrentWorkingDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentWorkingDir.Location = new System.Drawing.Point(4, 0);
            this.lblCurrentWorkingDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentWorkingDir.Name = "lblCurrentWorkingDir";
            this.lblCurrentWorkingDir.Size = new System.Drawing.Size(288, 50);
            this.lblCurrentWorkingDir.TabIndex = 16;
            this.lblCurrentWorkingDir.Text = "current working directory:";
            this.lblCurrentWorkingDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrWorkingDir
            // 
            this.lblCurrWorkingDir.AutoSize = true;
            this.lblCurrWorkingDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCurrWorkingDir, 3);
            this.lblCurrWorkingDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrWorkingDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCurrWorkingDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrWorkingDir.Location = new System.Drawing.Point(300, 0);
            this.lblCurrWorkingDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrWorkingDir.Name = "lblCurrWorkingDir";
            this.lblCurrWorkingDir.Size = new System.Drawing.Size(880, 50);
            this.lblCurrWorkingDir.TabIndex = 17;
            this.lblCurrWorkingDir.Text = "Current working path";
            this.lblCurrWorkingDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnProperties
            // 
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProperties.Location = new System.Drawing.Point(1188, 4);
            this.btnProperties.Margin = new System.Windows.Forms.Padding(4);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(288, 42);
            this.btnProperties.TabIndex = 18;
            this.btnProperties.Text = "Edit defaults";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // cbUseConstraints
            // 
            this.cbUseConstraints.AutoSize = true;
            this.cbUseConstraints.Checked = true;
            this.cbUseConstraints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseConstraints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbUseConstraints.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUseConstraints.Location = new System.Drawing.Point(1187, 53);
            this.cbUseConstraints.Name = "cbUseConstraints";
            this.cbUseConstraints.Size = new System.Drawing.Size(290, 44);
            this.cbUseConstraints.TabIndex = 19;
            this.cbUseConstraints.Text = "use constraints";
            this.cbUseConstraints.UseVisualStyleBackColor = true;
            // 
            // btnInitEvenlopData
            // 
            this.btnInitEvenlopData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInitEvenlopData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInitEvenlopData.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitEvenlopData.Location = new System.Drawing.Point(3, 103);
            this.btnInitEvenlopData.Name = "btnInitEvenlopData";
            this.btnInitEvenlopData.Size = new System.Drawing.Size(290, 44);
            this.btnInitEvenlopData.TabIndex = 21;
            this.btnInitEvenlopData.Text = "initialize evenlop data";
            this.btnInitEvenlopData.UseVisualStyleBackColor = true;
            this.btnInitEvenlopData.Click += new System.EventHandler(this.btnInitEvenlopData_Click);
            // 
            // btnApproxEvenlop
            // 
            this.btnApproxEvenlop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApproxEvenlop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApproxEvenlop.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApproxEvenlop.Location = new System.Drawing.Point(595, 103);
            this.btnApproxEvenlop.Name = "btnApproxEvenlop";
            this.btnApproxEvenlop.Size = new System.Drawing.Size(290, 44);
            this.btnApproxEvenlop.TabIndex = 22;
            this.btnApproxEvenlop.Text = "evenlop approx test";
            this.btnApproxEvenlop.UseVisualStyleBackColor = true;
            this.btnApproxEvenlop.Click += new System.EventHandler(this.btnApproxEvenlop_Click);
            // 
            // btnInitDataForintelMKL
            // 
            this.btnInitDataForintelMKL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInitDataForintelMKL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInitDataForintelMKL.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitDataForintelMKL.Location = new System.Drawing.Point(3, 153);
            this.btnInitDataForintelMKL.Name = "btnInitDataForintelMKL";
            this.btnInitDataForintelMKL.Size = new System.Drawing.Size(290, 44);
            this.btnInitDataForintelMKL.TabIndex = 23;
            this.btnInitDataForintelMKL.Text = "init data for MKL";
            this.btnInitDataForintelMKL.UseVisualStyleBackColor = true;
            this.btnInitDataForintelMKL.Click += new System.EventHandler(this.btnInitDataForintelMKL_Click);
            // 
            // rtbDataFilesPrefixMKL
            // 
            this.rtbDataFilesPrefixMKL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDataFilesPrefixMKL.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDataFilesPrefixMKL.Location = new System.Drawing.Point(299, 153);
            this.rtbDataFilesPrefixMKL.Name = "rtbDataFilesPrefixMKL";
            this.rtbDataFilesPrefixMKL.Size = new System.Drawing.Size(290, 44);
            this.rtbDataFilesPrefixMKL.TabIndex = 24;
            this.rtbDataFilesPrefixMKL.Text = "";
            // 
            // btnApproximateMKL
            // 
            this.btnApproximateMKL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApproximateMKL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApproximateMKL.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApproximateMKL.Location = new System.Drawing.Point(595, 153);
            this.btnApproximateMKL.Name = "btnApproximateMKL";
            this.btnApproximateMKL.Size = new System.Drawing.Size(290, 44);
            this.btnApproximateMKL.TabIndex = 25;
            this.btnApproximateMKL.Text = "test approximation with MKL";
            this.btnApproximateMKL.UseVisualStyleBackColor = true;
            this.btnApproximateMKL.Click += new System.EventHandler(this.btnApproximateMKL_Click);
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
        private System.Windows.Forms.TextBox tbDataFilePrefix;
        private System.Windows.Forms.Label lblCurrentWorkingDir;
        private System.Windows.Forms.Label lblCurrWorkingDir;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.CheckBox cbUseConstraints;
        private System.Windows.Forms.Button btnInitEvenlopData;
        private System.Windows.Forms.Button btnApproxEvenlop;
        private System.Windows.Forms.Button btnInitDataForintelMKL;
        private System.Windows.Forms.RichTextBox rtbDataFilesPrefixMKL;
        private System.Windows.Forms.Button btnApproximateMKL;
    }
}

