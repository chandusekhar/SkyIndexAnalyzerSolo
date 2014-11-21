namespace SkyIndexAnalyzerLibraries
{
    partial class SimpleShowImageForm
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
            this.pb1 = new System.Windows.Forms.PictureBox();
            this.lblTitle1 = new System.Windows.Forms.Label();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.btnSaveToFile = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.pb1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTitle1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbFileName, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveToFile, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(624, 442);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pb1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pb1, 2);
            this.pb1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb1.Location = new System.Drawing.Point(3, 43);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(618, 356);
            this.pb1.TabIndex = 0;
            this.pb1.TabStop = false;
            // 
            // lblTitle1
            // 
            this.lblTitle1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTitle1, 2);
            this.lblTitle1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle1.Location = new System.Drawing.Point(3, 0);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.Size = new System.Drawing.Size(618, 40);
            this.lblTitle1.TabIndex = 1;
            this.lblTitle1.Text = "TITLE";
            // 
            // tbFileName
            // 
            this.tbFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFileName.Location = new System.Drawing.Point(3, 405);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(538, 26);
            this.tbFileName.TabIndex = 2;
            // 
            // btnSaveToFile
            // 
            this.btnSaveToFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveToFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveToFile.Location = new System.Drawing.Point(547, 405);
            this.btnSaveToFile.Name = "btnSaveToFile";
            this.btnSaveToFile.Size = new System.Drawing.Size(74, 34);
            this.btnSaveToFile.TabIndex = 3;
            this.btnSaveToFile.Text = "Save...";
            this.btnSaveToFile.UseVisualStyleBackColor = true;
            this.btnSaveToFile.Click += new System.EventHandler(this.btnSaveToFile_Click);
            // 
            // SimpleShowImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "SimpleShowImageForm";
            this.Text = "ShowImageForm";
            this.Load += new System.EventHandler(this.SimpleShowImageForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SimpleShowImageForm_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SimpleShowImageForm_KeyPress);
            this.Resize += new System.EventHandler(this.SimpleShowImageForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTitle1;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Button btnSaveToFile;
        public System.Windows.Forms.PictureBox pb1;
    }
}