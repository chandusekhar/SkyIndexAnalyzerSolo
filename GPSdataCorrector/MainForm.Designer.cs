namespace GPSdataCorrector
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
            this.lblSourceDir = new System.Windows.Forms.Label();
            this.rtbPath = new System.Windows.Forms.RichTextBox();
            this.btnSelectSourceDirectory = new System.Windows.Forms.Button();
            this.btnCorrectGPSdata = new System.Windows.Forms.Button();
            this.lblVesselNavDataDirectory = new System.Windows.Forms.Label();
            this.rtbVesselNavDataDirectoryPath = new System.Windows.Forms.RichTextBox();
            this.btnSelectVesselNavDataDirectory = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Controls.Add(this.lblSourceDir, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rtbPath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectSourceDirectory, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCorrectGPSdata, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblVesselNavDataDirectory, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtbVesselNavDataDirectoryPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectVesselNavDataDirectory, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1433, 218);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblSourceDir
            // 
            this.lblSourceDir.AutoSize = true;
            this.lblSourceDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSourceDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSourceDir.Location = new System.Drawing.Point(3, 0);
            this.lblSourceDir.Name = "lblSourceDir";
            this.lblSourceDir.Size = new System.Drawing.Size(194, 60);
            this.lblSourceDir.TabIndex = 0;
            this.lblSourceDir.Text = "source directory";
            this.lblSourceDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbPath
            // 
            this.rtbPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbPath.Location = new System.Drawing.Point(203, 3);
            this.rtbPath.Name = "rtbPath";
            this.rtbPath.Size = new System.Drawing.Size(1027, 54);
            this.rtbPath.TabIndex = 2;
            this.rtbPath.Text = "";
            // 
            // btnSelectSourceDirectory
            // 
            this.btnSelectSourceDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectSourceDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectSourceDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectSourceDirectory.Location = new System.Drawing.Point(1236, 3);
            this.btnSelectSourceDirectory.Name = "btnSelectSourceDirectory";
            this.btnSelectSourceDirectory.Size = new System.Drawing.Size(194, 54);
            this.btnSelectSourceDirectory.TabIndex = 4;
            this.btnSelectSourceDirectory.Text = "browse...";
            this.btnSelectSourceDirectory.UseVisualStyleBackColor = true;
            this.btnSelectSourceDirectory.Click += new System.EventHandler(this.btnSelectSourceDirectory_Click);
            // 
            // btnCorrectGPSdata
            // 
            this.btnCorrectGPSdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCorrectGPSdata.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrectGPSdata.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCorrectGPSdata.Location = new System.Drawing.Point(1236, 123);
            this.btnCorrectGPSdata.Name = "btnCorrectGPSdata";
            this.btnCorrectGPSdata.Size = new System.Drawing.Size(194, 92);
            this.btnCorrectGPSdata.TabIndex = 5;
            this.btnCorrectGPSdata.Text = "correct";
            this.btnCorrectGPSdata.UseVisualStyleBackColor = true;
            this.btnCorrectGPSdata.Click += new System.EventHandler(this.btnCorrectGPSdata_Click);
            // 
            // lblVesselNavDataDirectory
            // 
            this.lblVesselNavDataDirectory.AutoSize = true;
            this.lblVesselNavDataDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVesselNavDataDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVesselNavDataDirectory.Location = new System.Drawing.Point(3, 60);
            this.lblVesselNavDataDirectory.Name = "lblVesselNavDataDirectory";
            this.lblVesselNavDataDirectory.Size = new System.Drawing.Size(194, 60);
            this.lblVesselNavDataDirectory.TabIndex = 6;
            this.lblVesselNavDataDirectory.Text = "vessel nav data directory:";
            this.lblVesselNavDataDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbVesselNavDataDirectoryPath
            // 
            this.rtbVesselNavDataDirectoryPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbVesselNavDataDirectoryPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbVesselNavDataDirectoryPath.Location = new System.Drawing.Point(203, 63);
            this.rtbVesselNavDataDirectoryPath.Name = "rtbVesselNavDataDirectoryPath";
            this.rtbVesselNavDataDirectoryPath.Size = new System.Drawing.Size(1027, 54);
            this.rtbVesselNavDataDirectoryPath.TabIndex = 7;
            this.rtbVesselNavDataDirectoryPath.Text = "";
            // 
            // btnSelectVesselNavDataDirectory
            // 
            this.btnSelectVesselNavDataDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectVesselNavDataDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectVesselNavDataDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectVesselNavDataDirectory.Location = new System.Drawing.Point(1236, 63);
            this.btnSelectVesselNavDataDirectory.Name = "btnSelectVesselNavDataDirectory";
            this.btnSelectVesselNavDataDirectory.Size = new System.Drawing.Size(194, 54);
            this.btnSelectVesselNavDataDirectory.TabIndex = 8;
            this.btnSelectVesselNavDataDirectory.Text = "browse...";
            this.btnSelectVesselNavDataDirectory.UseVisualStyleBackColor = true;
            this.btnSelectVesselNavDataDirectory.Click += new System.EventHandler(this.btnSelectVesselNavDataDirectory_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1433, 218);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSourceDir;
        private System.Windows.Forms.RichTextBox rtbPath;
        private System.Windows.Forms.Button btnSelectSourceDirectory;
        private System.Windows.Forms.Button btnCorrectGPSdata;
        private System.Windows.Forms.Label lblVesselNavDataDirectory;
        private System.Windows.Forms.RichTextBox rtbVesselNavDataDirectoryPath;
        private System.Windows.Forms.Button btnSelectVesselNavDataDirectory;
    }
}

