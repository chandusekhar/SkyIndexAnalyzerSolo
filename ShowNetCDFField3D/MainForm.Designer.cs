namespace ShowNetCDFField3D
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFileName = new System.Windows.Forms.Label();
            this.ilPanel1 = new ILNumerics.Drawing.ILPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.lblFileName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ilPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(948, 705);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblFileName, 6);
            this.lblFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileName.Location = new System.Drawing.Point(3, 0);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(942, 50);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "---";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ilPanel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ilPanel1, 6);
            this.ilPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilPanel1.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.ilPanel1.Editor = null;
            this.ilPanel1.Location = new System.Drawing.Point(3, 53);
            this.ilPanel1.Name = "ilPanel1";
            this.ilPanel1.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("ilPanel1.Rectangle")));
            this.tableLayoutPanel1.SetRowSpan(this.ilPanel1, 2);
            this.ilPanel1.ShowUIControls = false;
            this.ilPanel1.Size = new System.Drawing.Size(942, 598);
            this.ilPanel1.TabIndex = 2;
            this.ilPanel1.Timeout = ((uint)(0u));
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatus, 6);
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(3, 654);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(942, 51);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "---";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 705);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Show the field 3D";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblFileName;
        private ILNumerics.Drawing.ILPanel ilPanel1;
        private System.Windows.Forms.Label lblStatus;
    }
}

