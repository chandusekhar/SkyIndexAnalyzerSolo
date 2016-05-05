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
            this.btnFromPathSelect = new System.Windows.Forms.Button();
            this.btnToPathSelect = new System.Windows.Forms.Button();
            this.btnDoWork = new System.Windows.Forms.Button();
            this.prbUniversalProgress = new System.Windows.Forms.ProgressBar();
            this.rtbFromPath = new System.Windows.Forms.RichTextBox();
            this.rtbToPath = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rtbConcurrentDataDir = new System.Windows.Forms.RichTextBox();
            this.btnConcurrentDataPathSelect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.rtbGrIxYRGBstatsDir = new System.Windows.Forms.RichTextBox();
            this.btnStatsDirPathSelect = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.bgwCopier = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnFromPathSelect, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnToPathSelect, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnDoWork, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.prbUniversalProgress, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.rtbFromPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtbToPath, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rtbConcurrentDataDir, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnConcurrentDataPathSelect, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.rtbGrIxYRGBstatsDir, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnStatsDirPathSelect, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1342, 353);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "FROM:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 200);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "TO:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnFromPathSelect
            // 
            this.btnFromPathSelect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnFromPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFromPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromPathSelect.Location = new System.Drawing.Point(1286, 64);
            this.btnFromPathSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnFromPathSelect.Name = "btnFromPathSelect";
            this.btnFromPathSelect.Size = new System.Drawing.Size(52, 32);
            this.btnFromPathSelect.TabIndex = 4;
            this.btnFromPathSelect.Text = "...";
            this.btnFromPathSelect.UseVisualStyleBackColor = false;
            this.btnFromPathSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // btnToPathSelect
            // 
            this.btnToPathSelect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnToPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToPathSelect.Location = new System.Drawing.Point(1286, 204);
            this.btnToPathSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnToPathSelect.Name = "btnToPathSelect";
            this.btnToPathSelect.Size = new System.Drawing.Size(52, 32);
            this.btnToPathSelect.TabIndex = 5;
            this.btnToPathSelect.Text = "...";
            this.btnToPathSelect.UseVisualStyleBackColor = false;
            this.btnToPathSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // btnDoWork
            // 
            this.btnDoWork.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnDoWork, 3);
            this.btnDoWork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDoWork.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDoWork.Location = new System.Drawing.Point(4, 244);
            this.btnDoWork.Margin = new System.Windows.Forms.Padding(4);
            this.btnDoWork.Name = "btnDoWork";
            this.btnDoWork.Size = new System.Drawing.Size(1334, 65);
            this.btnDoWork.TabIndex = 7;
            this.btnDoWork.Text = "SELECT AND COPY";
            this.btnDoWork.UseVisualStyleBackColor = false;
            this.btnDoWork.Click += new System.EventHandler(this.btnDoWork_Click);
            // 
            // prbUniversalProgress
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.prbUniversalProgress, 3);
            this.prbUniversalProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbUniversalProgress.Location = new System.Drawing.Point(4, 317);
            this.prbUniversalProgress.Margin = new System.Windows.Forms.Padding(4);
            this.prbUniversalProgress.Maximum = 1000;
            this.prbUniversalProgress.Name = "prbUniversalProgress";
            this.prbUniversalProgress.Size = new System.Drawing.Size(1334, 32);
            this.prbUniversalProgress.TabIndex = 8;
            // 
            // rtbFromPath
            // 
            this.rtbFromPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbFromPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbFromPath.Location = new System.Drawing.Point(203, 63);
            this.rtbFromPath.Name = "rtbFromPath";
            this.rtbFromPath.Size = new System.Drawing.Size(1076, 34);
            this.rtbFromPath.TabIndex = 9;
            this.rtbFromPath.Text = "";
            this.rtbFromPath.TextChanged += new System.EventHandler(this.rtb_TextChanged);
            // 
            // rtbToPath
            // 
            this.rtbToPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbToPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbToPath.Location = new System.Drawing.Point(203, 203);
            this.rtbToPath.Name = "rtbToPath";
            this.rtbToPath.Size = new System.Drawing.Size(1076, 34);
            this.rtbToPath.TabIndex = 10;
            this.rtbToPath.Text = "";
            this.rtbToPath.TextChanged += new System.EventHandler(this.rtb_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 40);
            this.label3.TabIndex = 11;
            this.label3.Text = "CONCURRENT DATA dir:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbConcurrentDataDir
            // 
            this.rtbConcurrentDataDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbConcurrentDataDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConcurrentDataDir.Location = new System.Drawing.Point(203, 103);
            this.rtbConcurrentDataDir.Name = "rtbConcurrentDataDir";
            this.rtbConcurrentDataDir.Size = new System.Drawing.Size(1076, 34);
            this.rtbConcurrentDataDir.TabIndex = 12;
            this.rtbConcurrentDataDir.Text = "";
            this.rtbConcurrentDataDir.TextChanged += new System.EventHandler(this.rtb_TextChanged);
            // 
            // btnConcurrentDataPathSelect
            // 
            this.btnConcurrentDataPathSelect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnConcurrentDataPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConcurrentDataPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConcurrentDataPathSelect.Location = new System.Drawing.Point(1285, 103);
            this.btnConcurrentDataPathSelect.Name = "btnConcurrentDataPathSelect";
            this.btnConcurrentDataPathSelect.Size = new System.Drawing.Size(54, 34);
            this.btnConcurrentDataPathSelect.TabIndex = 13;
            this.btnConcurrentDataPathSelect.Text = "...";
            this.btnConcurrentDataPathSelect.UseVisualStyleBackColor = false;
            this.btnConcurrentDataPathSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(194, 40);
            this.label4.TabIndex = 14;
            this.label4.Text = "Pre-calculated GrIxYRGB stats XML files dir";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbGrIxYRGBstatsDir
            // 
            this.rtbGrIxYRGBstatsDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbGrIxYRGBstatsDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbGrIxYRGBstatsDir.Location = new System.Drawing.Point(203, 143);
            this.rtbGrIxYRGBstatsDir.Name = "rtbGrIxYRGBstatsDir";
            this.rtbGrIxYRGBstatsDir.Size = new System.Drawing.Size(1076, 34);
            this.rtbGrIxYRGBstatsDir.TabIndex = 15;
            this.rtbGrIxYRGBstatsDir.Text = "";
            this.rtbGrIxYRGBstatsDir.TextChanged += new System.EventHandler(this.rtb_TextChanged);
            // 
            // btnStatsDirPathSelect
            // 
            this.btnStatsDirPathSelect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnStatsDirPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStatsDirPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatsDirPathSelect.Location = new System.Drawing.Point(1285, 143);
            this.btnStatsDirPathSelect.Name = "btnStatsDirPathSelect";
            this.btnStatsDirPathSelect.Size = new System.Drawing.Size(54, 34);
            this.btnStatsDirPathSelect.TabIndex = 16;
            this.btnStatsDirPathSelect.Text = "...";
            this.btnStatsDirPathSelect.UseVisualStyleBackColor = false;
            this.btnStatsDirPathSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.BackColor = System.Drawing.Color.Gainsboro;
            this.btnProperties.BackgroundImage = global::FilesFilteringApp.Properties.Resources.gearIcon;
            this.btnProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Location = new System.Drawing.Point(1285, 3);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(54, 54);
            this.btnProperties.TabIndex = 17;
            this.btnProperties.UseVisualStyleBackColor = false;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
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
            this.ClientSize = new System.Drawing.Size(1342, 353);
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
        private System.Windows.Forms.Button btnFromPathSelect;
        private System.Windows.Forms.Button btnToPathSelect;
        private System.Windows.Forms.Button btnDoWork;
        private System.ComponentModel.BackgroundWorker bgwCopier;
        private System.Windows.Forms.ProgressBar prbUniversalProgress;
        private System.Windows.Forms.RichTextBox rtbFromPath;
        private System.Windows.Forms.RichTextBox rtbToPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtbConcurrentDataDir;
        private System.Windows.Forms.Button btnConcurrentDataPathSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox rtbGrIxYRGBstatsDir;
        private System.Windows.Forms.Button btnStatsDirPathSelect;
        private System.Windows.Forms.Button btnProperties;
    }
}

