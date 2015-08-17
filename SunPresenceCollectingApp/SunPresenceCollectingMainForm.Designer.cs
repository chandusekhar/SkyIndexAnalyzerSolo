namespace SunPresenceCollectingApp
{
    partial class SunPresenceCollectingMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SunPresenceCollectingMainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.currImagePictureBox = new System.Windows.Forms.PictureBox();
            this.btnProperties = new System.Windows.Forms.Button();
            this.lblImageTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMarkNoSun = new System.Windows.Forms.Button();
            this.btnMarkSun0 = new System.Windows.Forms.Button();
            this.btnMarkSun1 = new System.Windows.Forms.Button();
            this.btnMarkSun2 = new System.Windows.Forms.Button();
            this.MedianPerc5DataTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblGrIxMedianTitle = new System.Windows.Forms.Label();
            this.lblGrIxPerc5Title = new System.Windows.Forms.Label();
            this.lblGrIxMedianValue = new System.Windows.Forms.Label();
            this.lblGrIxPerc5Value = new System.Windows.Forms.Label();
            this.circBgwProcessingImage = new MRG.Controls.UI.LoadingCircle();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currImagePictureBox)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.MedianPerc5DataTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.currImagePictureBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblImageTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.MedianPerc5DataTable, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.circBgwProcessingImage, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1060, 727);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // currImagePictureBox
            // 
            this.currImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.currImagePictureBox, 4);
            this.currImagePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currImagePictureBox.Location = new System.Drawing.Point(3, 163);
            this.currImagePictureBox.Name = "currImagePictureBox";
            this.tableLayoutPanel1.SetRowSpan(this.currImagePictureBox, 4);
            this.currImagePictureBox.Size = new System.Drawing.Size(1054, 561);
            this.currImagePictureBox.TabIndex = 0;
            this.currImagePictureBox.TabStop = false;
            // 
            // btnProperties
            // 
            this.btnProperties.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProperties.BackgroundImage")));
            this.btnProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Location = new System.Drawing.Point(1003, 3);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(54, 54);
            this.btnProperties.TabIndex = 1;
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // lblImageTitle
            // 
            this.lblImageTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblImageTitle, 2);
            this.lblImageTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblImageTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImageTitle.Location = new System.Drawing.Point(3, 0);
            this.lblImageTitle.Name = "lblImageTitle";
            this.lblImageTitle.Size = new System.Drawing.Size(934, 60);
            this.lblImageTitle.TabIndex = 2;
            this.lblImageTitle.Text = "---";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.btnMarkNoSun, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMarkSun0, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMarkSun1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMarkSun2, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(464, 94);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btnMarkNoSun
            // 
            this.btnMarkNoSun.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.nosun1;
            this.btnMarkNoSun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkNoSun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkNoSun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkNoSun.Location = new System.Drawing.Point(3, 3);
            this.btnMarkNoSun.Name = "btnMarkNoSun";
            this.btnMarkNoSun.Size = new System.Drawing.Size(110, 88);
            this.btnMarkNoSun.TabIndex = 0;
            this.btnMarkNoSun.UseVisualStyleBackColor = true;
            this.btnMarkNoSun.Click += new System.EventHandler(this.MarkSunCondition_Click);
            // 
            // btnMarkSun0
            // 
            this.btnMarkSun0.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.sun01;
            this.btnMarkSun0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkSun0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkSun0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkSun0.Location = new System.Drawing.Point(119, 3);
            this.btnMarkSun0.Name = "btnMarkSun0";
            this.btnMarkSun0.Size = new System.Drawing.Size(110, 88);
            this.btnMarkSun0.TabIndex = 1;
            this.btnMarkSun0.UseVisualStyleBackColor = true;
            // 
            // btnMarkSun1
            // 
            this.btnMarkSun1.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.sun11;
            this.btnMarkSun1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkSun1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkSun1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkSun1.Location = new System.Drawing.Point(235, 3);
            this.btnMarkSun1.Name = "btnMarkSun1";
            this.btnMarkSun1.Size = new System.Drawing.Size(110, 88);
            this.btnMarkSun1.TabIndex = 2;
            this.btnMarkSun1.UseVisualStyleBackColor = true;
            // 
            // btnMarkSun2
            // 
            this.btnMarkSun2.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.sun21;
            this.btnMarkSun2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkSun2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkSun2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkSun2.Location = new System.Drawing.Point(351, 3);
            this.btnMarkSun2.Name = "btnMarkSun2";
            this.btnMarkSun2.Size = new System.Drawing.Size(110, 88);
            this.btnMarkSun2.TabIndex = 3;
            this.btnMarkSun2.UseVisualStyleBackColor = true;
            // 
            // MedianPerc5DataTable
            // 
            this.MedianPerc5DataTable.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.MedianPerc5DataTable, 3);
            this.MedianPerc5DataTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.4251F));
            this.MedianPerc5DataTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.5749F));
            this.MedianPerc5DataTable.Controls.Add(this.lblGrIxMedianTitle, 0, 0);
            this.MedianPerc5DataTable.Controls.Add(this.lblGrIxPerc5Title, 0, 1);
            this.MedianPerc5DataTable.Controls.Add(this.lblGrIxMedianValue, 1, 0);
            this.MedianPerc5DataTable.Controls.Add(this.lblGrIxPerc5Value, 1, 1);
            this.MedianPerc5DataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MedianPerc5DataTable.Location = new System.Drawing.Point(473, 63);
            this.MedianPerc5DataTable.Name = "MedianPerc5DataTable";
            this.MedianPerc5DataTable.RowCount = 2;
            this.MedianPerc5DataTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MedianPerc5DataTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MedianPerc5DataTable.Size = new System.Drawing.Size(584, 94);
            this.MedianPerc5DataTable.TabIndex = 4;
            // 
            // lblGrIxMedianTitle
            // 
            this.lblGrIxMedianTitle.AutoSize = true;
            this.lblGrIxMedianTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrIxMedianTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGrIxMedianTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrIxMedianTitle.Location = new System.Drawing.Point(3, 0);
            this.lblGrIxMedianTitle.Name = "lblGrIxMedianTitle";
            this.lblGrIxMedianTitle.Size = new System.Drawing.Size(200, 47);
            this.lblGrIxMedianTitle.TabIndex = 0;
            this.lblGrIxMedianTitle.Text = "GrIx median value";
            this.lblGrIxMedianTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGrIxPerc5Title
            // 
            this.lblGrIxPerc5Title.AutoSize = true;
            this.lblGrIxPerc5Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrIxPerc5Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGrIxPerc5Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrIxPerc5Title.Location = new System.Drawing.Point(3, 47);
            this.lblGrIxPerc5Title.Name = "lblGrIxPerc5Title";
            this.lblGrIxPerc5Title.Size = new System.Drawing.Size(200, 47);
            this.lblGrIxPerc5Title.TabIndex = 1;
            this.lblGrIxPerc5Title.Text = "GrIx perc(5) value";
            this.lblGrIxPerc5Title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGrIxMedianValue
            // 
            this.lblGrIxMedianValue.AutoSize = true;
            this.lblGrIxMedianValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrIxMedianValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGrIxMedianValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrIxMedianValue.Location = new System.Drawing.Point(209, 0);
            this.lblGrIxMedianValue.Name = "lblGrIxMedianValue";
            this.lblGrIxMedianValue.Size = new System.Drawing.Size(372, 47);
            this.lblGrIxMedianValue.TabIndex = 2;
            this.lblGrIxMedianValue.Text = "---";
            this.lblGrIxMedianValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGrIxPerc5Value
            // 
            this.lblGrIxPerc5Value.AutoSize = true;
            this.lblGrIxPerc5Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrIxPerc5Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGrIxPerc5Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrIxPerc5Value.Location = new System.Drawing.Point(209, 47);
            this.lblGrIxPerc5Value.Name = "lblGrIxPerc5Value";
            this.lblGrIxPerc5Value.Size = new System.Drawing.Size(372, 47);
            this.lblGrIxPerc5Value.TabIndex = 3;
            this.lblGrIxPerc5Value.Text = "---";
            this.lblGrIxPerc5Value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // circBgwProcessingImage
            // 
            this.circBgwProcessingImage.Active = false;
            this.circBgwProcessingImage.Color = System.Drawing.Color.SteelBlue;
            this.circBgwProcessingImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.circBgwProcessingImage.InnerCircleRadius = 10;
            this.circBgwProcessingImage.Location = new System.Drawing.Point(943, 3);
            this.circBgwProcessingImage.Name = "circBgwProcessingImage";
            this.circBgwProcessingImage.NumberSpoke = 24;
            this.circBgwProcessingImage.OuterCircleRadius = 12;
            this.circBgwProcessingImage.RotationSpeed = 100;
            this.circBgwProcessingImage.Size = new System.Drawing.Size(54, 54);
            this.circBgwProcessingImage.SpokeThickness = 4;
            this.circBgwProcessingImage.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.circBgwProcessingImage.TabIndex = 5;
            this.circBgwProcessingImage.Visible = false;
            // 
            // SunPresenceCollectingMainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 727);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "SunPresenceCollectingMainForm";
            this.Text = "Collect sun presence features";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.SunPresenceCollectingMainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.SunPresenceCollectingMainForm_DragEnter);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SunPresenceCollectingMainForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currImagePictureBox)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.MedianPerc5DataTable.ResumeLayout(false);
            this.MedianPerc5DataTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox currImagePictureBox;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Label lblImageTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnMarkNoSun;
        private System.Windows.Forms.Button btnMarkSun0;
        private System.Windows.Forms.Button btnMarkSun1;
        private System.Windows.Forms.Button btnMarkSun2;
        private System.Windows.Forms.TableLayoutPanel MedianPerc5DataTable;
        private System.Windows.Forms.Label lblGrIxMedianTitle;
        private System.Windows.Forms.Label lblGrIxPerc5Title;
        private System.Windows.Forms.Label lblGrIxMedianValue;
        private System.Windows.Forms.Label lblGrIxPerc5Value;
        private MRG.Controls.UI.LoadingCircle circBgwProcessingImage;
    }
}

