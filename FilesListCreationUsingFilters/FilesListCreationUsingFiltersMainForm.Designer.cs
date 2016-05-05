namespace FilesListCreationUsingFilters
{
    partial class FilesListCreationUsingFiltersMainForm
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
            this.btnDestinationPathSelect = new System.Windows.Forms.Button();
            this.btnCreateList = new System.Windows.Forms.Button();
            this.prbUniversalProgress = new System.Windows.Forms.ProgressBar();
            this.rtbImagesBaseSourcePath = new System.Windows.Forms.RichTextBox();
            this.rtbDestinationPath = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rtbConcurrentAndStatsXMLfilesDir = new System.Windows.Forms.RichTextBox();
            this.btnConcurrentDataPathSelect = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.rtbObservedDataCSVfile = new System.Windows.Forms.RichTextBox();
            this.btnObservedDataCSVfileSelect = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.Controls.Add(this.btnDestinationPathSelect, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnCreateList, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.prbUniversalProgress, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.rtbImagesBaseSourcePath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtbDestinationPath, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rtbConcurrentAndStatsXMLfilesDir, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnConcurrentDataPathSelect, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.rtbObservedDataCSVfile, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnObservedDataCSVfileSelect, 2, 3);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1025, 368);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Images base source path:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 200);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "OUTPUT dir:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnFromPathSelect
            // 
            this.btnFromPathSelect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnFromPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFromPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromPathSelect.Location = new System.Drawing.Point(969, 64);
            this.btnFromPathSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnFromPathSelect.Name = "btnFromPathSelect";
            this.btnFromPathSelect.Size = new System.Drawing.Size(52, 32);
            this.btnFromPathSelect.TabIndex = 4;
            this.btnFromPathSelect.Text = "...";
            this.btnFromPathSelect.UseVisualStyleBackColor = false;
            // 
            // btnDestinationPathSelect
            // 
            this.btnDestinationPathSelect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnDestinationPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDestinationPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDestinationPathSelect.Location = new System.Drawing.Point(969, 204);
            this.btnDestinationPathSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnDestinationPathSelect.Name = "btnDestinationPathSelect";
            this.btnDestinationPathSelect.Size = new System.Drawing.Size(52, 32);
            this.btnDestinationPathSelect.TabIndex = 5;
            this.btnDestinationPathSelect.Text = "...";
            this.btnDestinationPathSelect.UseVisualStyleBackColor = false;
            // 
            // btnCreateList
            // 
            this.btnCreateList.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.btnCreateList, 3);
            this.btnCreateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateList.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCreateList.Location = new System.Drawing.Point(4, 244);
            this.btnCreateList.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreateList.Name = "btnCreateList";
            this.btnCreateList.Size = new System.Drawing.Size(1017, 80);
            this.btnCreateList.TabIndex = 7;
            this.btnCreateList.Text = "CREATE LIST";
            this.btnCreateList.UseVisualStyleBackColor = false;
            this.btnCreateList.Click += new System.EventHandler(this.btnCreateList_Click);
            // 
            // prbUniversalProgress
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.prbUniversalProgress, 3);
            this.prbUniversalProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbUniversalProgress.Location = new System.Drawing.Point(4, 332);
            this.prbUniversalProgress.Margin = new System.Windows.Forms.Padding(4);
            this.prbUniversalProgress.Maximum = 1000;
            this.prbUniversalProgress.Name = "prbUniversalProgress";
            this.prbUniversalProgress.Size = new System.Drawing.Size(1017, 32);
            this.prbUniversalProgress.TabIndex = 8;
            // 
            // rtbImagesBaseSourcePath
            // 
            this.rtbImagesBaseSourcePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbImagesBaseSourcePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbImagesBaseSourcePath.Location = new System.Drawing.Point(203, 63);
            this.rtbImagesBaseSourcePath.Name = "rtbImagesBaseSourcePath";
            this.rtbImagesBaseSourcePath.Size = new System.Drawing.Size(759, 34);
            this.rtbImagesBaseSourcePath.TabIndex = 9;
            this.rtbImagesBaseSourcePath.Text = "";
            // 
            // rtbDestinationPath
            // 
            this.rtbDestinationPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDestinationPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDestinationPath.Location = new System.Drawing.Point(203, 203);
            this.rtbDestinationPath.Name = "rtbDestinationPath";
            this.rtbDestinationPath.Size = new System.Drawing.Size(759, 34);
            this.rtbDestinationPath.TabIndex = 10;
            this.rtbDestinationPath.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 40);
            this.label3.TabIndex = 11;
            this.label3.Text = "Whole concurrent and stats XML files dir:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbConcurrentAndStatsXMLfilesDir
            // 
            this.rtbConcurrentAndStatsXMLfilesDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbConcurrentAndStatsXMLfilesDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConcurrentAndStatsXMLfilesDir.Location = new System.Drawing.Point(203, 103);
            this.rtbConcurrentAndStatsXMLfilesDir.Name = "rtbConcurrentAndStatsXMLfilesDir";
            this.rtbConcurrentAndStatsXMLfilesDir.Size = new System.Drawing.Size(759, 34);
            this.rtbConcurrentAndStatsXMLfilesDir.TabIndex = 12;
            this.rtbConcurrentAndStatsXMLfilesDir.Text = "";
            // 
            // btnConcurrentDataPathSelect
            // 
            this.btnConcurrentDataPathSelect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnConcurrentDataPathSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConcurrentDataPathSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConcurrentDataPathSelect.Location = new System.Drawing.Point(968, 103);
            this.btnConcurrentDataPathSelect.Name = "btnConcurrentDataPathSelect";
            this.btnConcurrentDataPathSelect.Size = new System.Drawing.Size(54, 34);
            this.btnConcurrentDataPathSelect.TabIndex = 13;
            this.btnConcurrentDataPathSelect.Text = "...";
            this.btnConcurrentDataPathSelect.UseVisualStyleBackColor = false;
            // 
            // btnProperties
            // 
            this.btnProperties.BackColor = System.Drawing.Color.Gainsboro;
            this.btnProperties.BackgroundImage = global::FilesListCreationUsingFilters.Properties.Resources.gearIcon;
            this.btnProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Location = new System.Drawing.Point(968, 3);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(54, 54);
            this.btnProperties.TabIndex = 17;
            this.btnProperties.UseVisualStyleBackColor = false;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(194, 40);
            this.label4.TabIndex = 18;
            this.label4.Text = "Observed data CSV file:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbObservedDataCSVfile
            // 
            this.rtbObservedDataCSVfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbObservedDataCSVfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbObservedDataCSVfile.Location = new System.Drawing.Point(203, 143);
            this.rtbObservedDataCSVfile.Name = "rtbObservedDataCSVfile";
            this.rtbObservedDataCSVfile.Size = new System.Drawing.Size(759, 34);
            this.rtbObservedDataCSVfile.TabIndex = 19;
            this.rtbObservedDataCSVfile.Text = "";
            // 
            // btnObservedDataCSVfileSelect
            // 
            this.btnObservedDataCSVfileSelect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnObservedDataCSVfileSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnObservedDataCSVfileSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnObservedDataCSVfileSelect.Location = new System.Drawing.Point(968, 143);
            this.btnObservedDataCSVfileSelect.Name = "btnObservedDataCSVfileSelect";
            this.btnObservedDataCSVfileSelect.Size = new System.Drawing.Size(54, 34);
            this.btnObservedDataCSVfileSelect.TabIndex = 20;
            this.btnObservedDataCSVfileSelect.Text = "...";
            this.btnObservedDataCSVfileSelect.UseVisualStyleBackColor = false;
            // 
            // FilesListCreationUsingFiltersMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 368);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FilesListCreationUsingFiltersMainForm";
            this.Text = "Main filtering form";
            this.Load += new System.EventHandler(this.FilesListCreationUsingFiltersMainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFromPathSelect;
        private System.Windows.Forms.Button btnDestinationPathSelect;
        private System.Windows.Forms.Button btnCreateList;
        private System.Windows.Forms.ProgressBar prbUniversalProgress;
        private System.Windows.Forms.RichTextBox rtbImagesBaseSourcePath;
        private System.Windows.Forms.RichTextBox rtbDestinationPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtbConcurrentAndStatsXMLfilesDir;
        private System.Windows.Forms.Button btnConcurrentDataPathSelect;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.RichTextBox rtbObservedDataCSVfile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnObservedDataCSVfileSelect;
    }
}

