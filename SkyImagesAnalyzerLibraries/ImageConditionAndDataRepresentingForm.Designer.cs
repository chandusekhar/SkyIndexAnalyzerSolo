namespace SkyImagesAnalyzerLibraries
{
    partial class ImageConditionAndDataRepresentingForm
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
            this.btnChangeColorScheme = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.btnsaveImageAndScale = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel1.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbRes, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbStats, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.pbScale, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.chbRes1DynamicScale, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveData, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnChangeColorScheme, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveImage, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnsaveImageAndScale, 1, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(759, 629);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTitle, 3);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(4, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(751, 50);
            this.lblTitle.TabIndex = 72;
            this.lblTitle.Text = "Selection";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbRes
            // 
            this.pbRes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbRes, 2);
            this.pbRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRes.Location = new System.Drawing.Point(4, 54);
            this.pbRes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbRes.Name = "pbRes";
            this.tableLayoutPanel1.SetRowSpan(this.pbRes, 6);
            this.pbRes.Size = new System.Drawing.Size(642, 388);
            this.pbRes.TabIndex = 75;
            this.pbRes.TabStop = false;
            this.pbRes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.pbRes.MouseLeave += new System.EventHandler(this.pbRes_MouseLeave);
            this.pbRes.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseMove);
            this.pbRes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseUp);
            // 
            // tbStats
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbStats, 2);
            this.tbStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStats.Location = new System.Drawing.Point(4, 550);
            this.tbStats.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbStats.Multiline = true;
            this.tbStats.Name = "tbStats";
            this.tableLayoutPanel1.SetRowSpan(this.tbStats, 2);
            this.tbStats.Size = new System.Drawing.Size(642, 75);
            this.tbStats.TabIndex = 76;
            // 
            // pbScale
            // 
            this.pbScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbScale.Location = new System.Drawing.Point(654, 54);
            this.pbScale.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbScale.Name = "pbScale";
            this.tableLayoutPanel1.SetRowSpan(this.pbScale, 6);
            this.pbScale.Size = new System.Drawing.Size(101, 388);
            this.pbScale.TabIndex = 80;
            this.pbScale.TabStop = false;
            this.pbScale.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.pbScale.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbScale_MouseMove);
            this.pbScale.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbRes_MouseUp);
            // 
            // chbRes1DynamicScale
            // 
            this.chbRes1DynamicScale.AutoSize = true;
            this.chbRes1DynamicScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chbRes1DynamicScale.Location = new System.Drawing.Point(654, 450);
            this.chbRes1DynamicScale.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbRes1DynamicScale.Name = "chbRes1DynamicScale";
            this.chbRes1DynamicScale.Size = new System.Drawing.Size(101, 42);
            this.chbRes1DynamicScale.TabIndex = 82;
            this.chbRes1DynamicScale.Text = "Dynamic";
            this.chbRes1DynamicScale.UseVisualStyleBackColor = true;
            this.chbRes1DynamicScale.CheckedChanged += new System.EventHandler(this.chbRes1DynamicScale_CheckedChanged);
            // 
            // btnSaveData
            // 
            this.btnSaveData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveData.Location = new System.Drawing.Point(4, 450);
            this.btnSaveData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(317, 42);
            this.btnSaveData.TabIndex = 83;
            this.btnSaveData.Text = "save underlying data...";
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // btnChangeColorScheme
            // 
            this.btnChangeColorScheme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChangeColorScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeColorScheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeColorScheme.Location = new System.Drawing.Point(328, 449);
            this.btnChangeColorScheme.Name = "btnChangeColorScheme";
            this.btnChangeColorScheme.Size = new System.Drawing.Size(319, 44);
            this.btnChangeColorScheme.TabIndex = 84;
            this.btnChangeColorScheme.Text = "Change color scheme...";
            this.btnChangeColorScheme.UseVisualStyleBackColor = true;
            this.btnChangeColorScheme.Click += new System.EventHandler(this.btnChangeColorScheme_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(653, 499);
            this.button1.Name = "button1";
            this.tableLayoutPanel1.SetRowSpan(this.button1, 3);
            this.button1.Size = new System.Drawing.Size(103, 127);
            this.button1.TabIndex = 85;
            this.button1.Text = "Reverse scale";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveImage.Location = new System.Drawing.Point(3, 499);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(319, 44);
            this.btnSaveImage.TabIndex = 86;
            this.btnSaveImage.Text = "Save image...";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // btnsaveImageAndScale
            // 
            this.btnsaveImageAndScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnsaveImageAndScale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsaveImageAndScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsaveImageAndScale.Location = new System.Drawing.Point(328, 499);
            this.btnsaveImageAndScale.Name = "btnsaveImageAndScale";
            this.btnsaveImageAndScale.Size = new System.Drawing.Size(319, 44);
            this.btnsaveImageAndScale.TabIndex = 87;
            this.btnsaveImageAndScale.Text = "Save image and scale...";
            this.btnsaveImageAndScale.UseVisualStyleBackColor = true;
            this.btnsaveImageAndScale.Click += new System.EventHandler(this.btnsaveImageAndScale_Click);
            // 
            // ImageConditionAndDataRepresentingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 629);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ImageConditionAndDataRepresentingForm";
            this.Text = "Image condition and data representing form";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageConditionAndDataRepresentingForm_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ImageConditionAndDataRepresentingForm_KeyPress);
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
        private System.Windows.Forms.Button btnChangeColorScheme;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.Button btnsaveImageAndScale;
    }
}