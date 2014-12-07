namespace FilesFilteringApp
{
    partial class MainForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFromPath = new System.Windows.Forms.TextBox();
            this.tbToPath = new System.Windows.Forms.TextBox();
            this.btnFromPathSelect = new System.Windows.Forms.Button();
            this.btnToPathSelect = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btnDoWork = new System.Windows.Forms.Button();
            this.prbUniversalProgress = new System.Windows.Forms.ProgressBar();
            this.bgwCopier = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbFromPath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbToPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFromPathSelect, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnToPathSelect, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbLog, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnDoWork, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.prbUniversalProgress, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1088, 409);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "FROM:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "TO:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFromPath
            // 
            this.tbFromPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFromPath.Location = new System.Drawing.Point(111, 4);
            this.tbFromPath.Margin = new System.Windows.Forms.Padding(4);
            this.tbFromPath.Name = "tbFromPath";
            this.tbFromPath.Size = new System.Drawing.Size(906, 22);
            this.tbFromPath.TabIndex = 2;
            // 
            // tbToPath
            // 
            this.tbToPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbToPath.Location = new System.Drawing.Point(111, 41);
            this.tbToPath.Margin = new System.Windows.Forms.Padding(4);
            this.tbToPath.Name = "tbToPath";
            this.tbToPath.Size = new System.Drawing.Size(906, 22);
            this.tbToPath.TabIndex = 3;
            // 
            // btnFromPathSelect
            // 
            this.btnFromPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFromPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromPathSelect.Location = new System.Drawing.Point(1025, 4);
            this.btnFromPathSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnFromPathSelect.Name = "btnFromPathSelect";
            this.btnFromPathSelect.Size = new System.Drawing.Size(59, 29);
            this.btnFromPathSelect.TabIndex = 4;
            this.btnFromPathSelect.Text = "...";
            this.btnFromPathSelect.UseVisualStyleBackColor = true;
            this.btnFromPathSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // btnToPathSelect
            // 
            this.btnToPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToPathSelect.Location = new System.Drawing.Point(1025, 41);
            this.btnToPathSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnToPathSelect.Name = "btnToPathSelect";
            this.btnToPathSelect.Size = new System.Drawing.Size(59, 29);
            this.btnToPathSelect.TabIndex = 5;
            this.btnToPathSelect.Text = "...";
            this.btnToPathSelect.UseVisualStyleBackColor = true;
            this.btnToPathSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbLog, 3);
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(4, 115);
            this.tbLog.Margin = new System.Windows.Forms.Padding(4);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(1080, 265);
            this.tbLog.TabIndex = 6;
            // 
            // btnDoWork
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnDoWork, 3);
            this.btnDoWork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDoWork.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDoWork.Location = new System.Drawing.Point(4, 78);
            this.btnDoWork.Margin = new System.Windows.Forms.Padding(4);
            this.btnDoWork.Name = "btnDoWork";
            this.btnDoWork.Size = new System.Drawing.Size(1080, 29);
            this.btnDoWork.TabIndex = 7;
            this.btnDoWork.Text = "SELECT";
            this.btnDoWork.UseVisualStyleBackColor = true;
            this.btnDoWork.Click += new System.EventHandler(this.btnDoWork_Click);
            // 
            // prbUniversalProgress
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.prbUniversalProgress, 3);
            this.prbUniversalProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbUniversalProgress.Location = new System.Drawing.Point(4, 388);
            this.prbUniversalProgress.Margin = new System.Windows.Forms.Padding(4);
            this.prbUniversalProgress.Maximum = 1000;
            this.prbUniversalProgress.Name = "prbUniversalProgress";
            this.prbUniversalProgress.Size = new System.Drawing.Size(1080, 17);
            this.prbUniversalProgress.TabIndex = 8;
            // 
            // bgwCopier
            // 
            this.bgwCopier.WorkerReportsProgress = true;
            this.bgwCopier.WorkerSupportsCancellation = true;
            this.bgwCopier.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwCopier_DoWork);
            this.bgwCopier.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwCopier_ProgressChanged);
            this.bgwCopier.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCopier_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 409);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFromPath;
        private System.Windows.Forms.TextBox tbToPath;
        private System.Windows.Forms.Button btnFromPathSelect;
        private System.Windows.Forms.Button btnToPathSelect;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btnDoWork;
        private System.ComponentModel.BackgroundWorker bgwCopier;
        private System.Windows.Forms.ProgressBar prbUniversalProgress;
    }
}

