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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbXMin = new System.Windows.Forms.TextBox();
            this.tbXMax = new System.Windows.Forms.TextBox();
            this.tbYMin = new System.Windows.Forms.TextBox();
            this.tbYMax = new System.Windows.Forms.TextBox();
            this.btnTaxonomyTest = new System.Windows.Forms.Button();
            this.tbDataFilePrefix = new System.Windows.Forms.TextBox();
            this.lblCurrentWorkingDir = new System.Windows.Forms.Label();
            this.lblCurrWorkingDir = new System.Windows.Forms.Label();
            this.btnProperties = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(3, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(306, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "initialize data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTestApprox
            // 
            this.btnTestApprox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTestApprox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestApprox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTestApprox.Location = new System.Drawing.Point(627, 33);
            this.btnTestApprox.Name = "btnTestApprox";
            this.btnTestApprox.Size = new System.Drawing.Size(306, 27);
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
            this.tableLayoutPanel1.SetColumnSpan(this.tbLog, 4);
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(3, 106);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(1243, 605);
            this.tbLog.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.Location = new System.Drawing.Point(627, 63);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(306, 40);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "0";
            // 
            // cbUseWeights
            // 
            this.cbUseWeights.AutoSize = true;
            this.cbUseWeights.Checked = true;
            this.cbUseWeights.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseWeights.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbUseWeights.Location = new System.Drawing.Point(939, 33);
            this.cbUseWeights.Name = "cbUseWeights";
            this.cbUseWeights.Size = new System.Drawing.Size(123, 24);
            this.cbUseWeights.TabIndex = 4;
            this.cbUseWeights.Text = "use weights";
            this.cbUseWeights.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.tbLog, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTestApprox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbUseWeights, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbXMin, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbXMax, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbYMin, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbYMax, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnTaxonomyTest, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbDataFilePrefix, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentWorkingDir, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrWorkingDir, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.73684F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1249, 836);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 794);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(306, 42);
            this.label2.TabIndex = 6;
            this.label2.Text = "x max";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 754);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 40);
            this.label1.TabIndex = 5;
            this.label1.Text = "x min";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(627, 794);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(306, 42);
            this.label4.TabIndex = 8;
            this.label4.Text = "y max";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(627, 754);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(306, 40);
            this.label3.TabIndex = 7;
            this.label3.Text = "y min";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbXMin
            // 
            this.tbXMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbXMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbXMin.Location = new System.Drawing.Point(315, 757);
            this.tbXMin.Name = "tbXMin";
            this.tbXMin.Size = new System.Drawing.Size(306, 26);
            this.tbXMin.TabIndex = 9;
            this.tbXMin.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tbXMax
            // 
            this.tbXMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbXMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbXMax.Location = new System.Drawing.Point(315, 797);
            this.tbXMax.Name = "tbXMax";
            this.tbXMax.Size = new System.Drawing.Size(306, 26);
            this.tbXMax.TabIndex = 10;
            this.tbXMax.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tbYMin
            // 
            this.tbYMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbYMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbYMin.Location = new System.Drawing.Point(939, 757);
            this.tbYMin.Name = "tbYMin";
            this.tbYMin.Size = new System.Drawing.Size(307, 26);
            this.tbYMin.TabIndex = 11;
            this.tbYMin.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tbYMax
            // 
            this.tbYMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbYMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbYMax.Location = new System.Drawing.Point(939, 797);
            this.tbYMax.Name = "tbYMax";
            this.tbYMax.Size = new System.Drawing.Size(307, 26);
            this.tbYMax.TabIndex = 12;
            this.tbYMax.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // btnTaxonomyTest
            // 
            this.btnTaxonomyTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTaxonomyTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaxonomyTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTaxonomyTest.Location = new System.Drawing.Point(3, 66);
            this.btnTaxonomyTest.Name = "btnTaxonomyTest";
            this.btnTaxonomyTest.Size = new System.Drawing.Size(306, 34);
            this.btnTaxonomyTest.TabIndex = 13;
            this.btnTaxonomyTest.Text = "test taxonomy";
            this.btnTaxonomyTest.UseVisualStyleBackColor = true;
            this.btnTaxonomyTest.Click += new System.EventHandler(this.btnTaxonomyTest_Click);
            // 
            // tbDataFilePrefix
            // 
            this.tbDataFilePrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDataFilePrefix.Location = new System.Drawing.Point(315, 33);
            this.tbDataFilePrefix.Name = "tbDataFilePrefix";
            this.tbDataFilePrefix.Size = new System.Drawing.Size(306, 20);
            this.tbDataFilePrefix.TabIndex = 15;
            // 
            // lblCurrentWorkingDir
            // 
            this.lblCurrentWorkingDir.AutoSize = true;
            this.lblCurrentWorkingDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentWorkingDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCurrentWorkingDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCurrentWorkingDir.Location = new System.Drawing.Point(3, 0);
            this.lblCurrentWorkingDir.Name = "lblCurrentWorkingDir";
            this.lblCurrentWorkingDir.Size = new System.Drawing.Size(306, 30);
            this.lblCurrentWorkingDir.TabIndex = 16;
            this.lblCurrentWorkingDir.Text = "current working directory:";
            this.lblCurrentWorkingDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrWorkingDir
            // 
            this.lblCurrWorkingDir.AutoSize = true;
            this.lblCurrWorkingDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCurrWorkingDir, 2);
            this.lblCurrWorkingDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrWorkingDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCurrWorkingDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCurrWorkingDir.Location = new System.Drawing.Point(315, 0);
            this.lblCurrWorkingDir.Name = "lblCurrWorkingDir";
            this.lblCurrWorkingDir.Size = new System.Drawing.Size(618, 30);
            this.lblCurrWorkingDir.TabIndex = 17;
            this.lblCurrWorkingDir.Text = "Current working path";
            this.lblCurrWorkingDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnProperties
            // 
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnProperties.Location = new System.Drawing.Point(939, 3);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(307, 24);
            this.btnProperties.TabIndex = 18;
            this.btnProperties.Text = "Edit defaults";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // TestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 836);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbXMin;
        private System.Windows.Forms.TextBox tbXMax;
        private System.Windows.Forms.TextBox tbYMin;
        private System.Windows.Forms.TextBox tbYMax;
        private System.Windows.Forms.Button btnTaxonomyTest;
        private System.Windows.Forms.TextBox tbDataFilePrefix;
        private System.Windows.Forms.Label lblCurrentWorkingDir;
        private System.Windows.Forms.Label lblCurrWorkingDir;
        private System.Windows.Forms.Button btnProperties;
    }
}

