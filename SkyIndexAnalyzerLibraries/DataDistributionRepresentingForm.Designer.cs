namespace SkyIndexAnalyzerLibraries
{
    partial class DataDistributionRepresentingForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbRes = new System.Windows.Forms.PictureBox();
            this.tbStats = new System.Windows.Forms.TextBox();
            this.pbScale = new System.Windows.Forms.PictureBox();
            this.chbRes1DynamicScale = new System.Windows.Forms.CheckBox();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScale)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbRes, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbStats, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.pbScale, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.chbRes1DynamicScale, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveData, 2, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(945, 658);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTitle, 3);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(939, 40);
            this.lblTitle.TabIndex = 72;
            this.lblTitle.Text = "Selection";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbRes
            // 
            this.pbRes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbRes, 2);
            this.pbRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRes.Location = new System.Drawing.Point(3, 43);
            this.pbRes.Name = "pbRes";
            this.tableLayoutPanel1.SetRowSpan(this.pbRes, 6);
            this.pbRes.Size = new System.Drawing.Size(858, 522);
            this.pbRes.TabIndex = 75;
            this.pbRes.TabStop = false;
            // 
            // tbStats
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbStats, 2);
            this.tbStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbStats.Location = new System.Drawing.Point(3, 571);
            this.tbStats.Multiline = true;
            this.tbStats.Name = "tbStats";
            this.tableLayoutPanel1.SetRowSpan(this.tbStats, 3);
            this.tbStats.Size = new System.Drawing.Size(858, 84);
            this.tbStats.TabIndex = 76;
            // 
            // pbScale
            // 
            this.pbScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbScale.Location = new System.Drawing.Point(867, 43);
            this.pbScale.Name = "pbScale";
            this.tableLayoutPanel1.SetRowSpan(this.pbScale, 6);
            this.pbScale.Size = new System.Drawing.Size(75, 522);
            this.pbScale.TabIndex = 80;
            this.pbScale.TabStop = false;
            // 
            // chbRes1DynamicScale
            // 
            this.chbRes1DynamicScale.AutoSize = true;
            this.chbRes1DynamicScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chbRes1DynamicScale.Location = new System.Drawing.Point(867, 571);
            this.chbRes1DynamicScale.Name = "chbRes1DynamicScale";
            this.chbRes1DynamicScale.Size = new System.Drawing.Size(75, 24);
            this.chbRes1DynamicScale.TabIndex = 82;
            this.chbRes1DynamicScale.Text = "Dynamic";
            this.chbRes1DynamicScale.UseVisualStyleBackColor = true;
            this.chbRes1DynamicScale.CheckedChanged += new System.EventHandler(this.chbRes1DynamicScale_CheckedChanged);
            // 
            // btnSaveData
            // 
            this.btnSaveData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveData.Location = new System.Drawing.Point(867, 601);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(75, 24);
            this.btnSaveData.TabIndex = 83;
            this.btnSaveData.Text = "save data...";
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // DataDistributionRepresentingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 658);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "DataDistributionRepresentingForm";
            this.Text = "DataDistributionRepresentingForm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DataDistributionRepresentingForm_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DataDistributionRepresentingForm_KeyPress);
            this.Resize += new System.EventHandler(this.DataDistributionRepresentingForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbRes;
        private System.Windows.Forms.TextBox tbStats;
        private System.Windows.Forms.PictureBox pbScale;
        private System.Windows.Forms.CheckBox chbRes1DynamicScale;
        private System.Windows.Forms.Button btnSaveData;

    }
}