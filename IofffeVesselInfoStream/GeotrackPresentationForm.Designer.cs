namespace IofffeVesselInfoStream
{
    partial class GeotrackPresentationForm
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
            this.btnCenterToActualPosition = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.wcUpdatimgGeoTrack = new MRG.Controls.UI.LoadingCircle();
            this.pbGeotrack = new System.Windows.Forms.PictureBox();
            this.trbGeoTrackScale = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbNormalizeImage = new System.Windows.Forms.CheckBox();
            this.btnIncreaseShorelineResolution = new System.Windows.Forms.Button();
            this.btnDecreaseShorelineResolution = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbTrackLineWidth = new System.Windows.Forms.RichTextBox();
            this.lblStatusString = new System.Windows.Forms.Label();
            this.bgwGeotrackRenderer = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeotrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbGeoTrackScale)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.Controls.Add(this.btnCenterToActualPosition, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnUpdate, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.wcUpdatimgGeoTrack, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbGeotrack, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.trbGeoTrackScale, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblStatusString, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(747, 497);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnCenterToActualPosition
            // 
            this.btnCenterToActualPosition.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCenterToActualPosition.BackgroundImage = global::IofffeVesselInfoStream.Properties.Resources._74_location;
            this.btnCenterToActualPosition.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCenterToActualPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenterToActualPosition.FlatAppearance.BorderSize = 0;
            this.btnCenterToActualPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCenterToActualPosition.Location = new System.Drawing.Point(3, 3);
            this.btnCenterToActualPosition.Name = "btnCenterToActualPosition";
            this.btnCenterToActualPosition.Size = new System.Drawing.Size(34, 34);
            this.btnCenterToActualPosition.TabIndex = 47;
            this.btnCenterToActualPosition.UseVisualStyleBackColor = false;
            this.btnCenterToActualPosition.Click += new System.EventHandler(this.btnCenterToActualPosition_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(42, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(494, 40);
            this.label1.TabIndex = 53;
            this.label1.Text = "Geotrack";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.Gainsboro;
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUpdate.Location = new System.Drawing.Point(540, 2);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(162, 36);
            this.btnUpdate.TabIndex = 54;
            this.btnUpdate.Text = "update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // wcUpdatimgGeoTrack
            // 
            this.wcUpdatimgGeoTrack.Active = false;
            this.wcUpdatimgGeoTrack.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.wcUpdatimgGeoTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wcUpdatimgGeoTrack.InnerCircleRadius = 8;
            this.wcUpdatimgGeoTrack.Location = new System.Drawing.Point(707, 3);
            this.wcUpdatimgGeoTrack.Name = "wcUpdatimgGeoTrack";
            this.wcUpdatimgGeoTrack.NumberSpoke = 24;
            this.wcUpdatimgGeoTrack.OuterCircleRadius = 9;
            this.wcUpdatimgGeoTrack.RotationSpeed = 100;
            this.wcUpdatimgGeoTrack.Size = new System.Drawing.Size(37, 34);
            this.wcUpdatimgGeoTrack.SpokeThickness = 4;
            this.wcUpdatimgGeoTrack.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.wcUpdatimgGeoTrack.TabIndex = 55;
            this.wcUpdatimgGeoTrack.Text = "loadingCircle1";
            this.wcUpdatimgGeoTrack.Visible = false;
            // 
            // pbGeotrack
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbGeotrack, 5);
            this.pbGeotrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGeotrack.Location = new System.Drawing.Point(3, 43);
            this.pbGeotrack.Name = "pbGeotrack";
            this.tableLayoutPanel1.SetRowSpan(this.pbGeotrack, 4);
            this.pbGeotrack.Size = new System.Drawing.Size(698, 370);
            this.pbGeotrack.TabIndex = 59;
            this.pbGeotrack.TabStop = false;
            this.pbGeotrack.Click += new System.EventHandler(this.pbGeoTrack_Click);
            // 
            // trbGeoTrackScale
            // 
            this.trbGeoTrackScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbGeoTrackScale.LargeChange = 3;
            this.trbGeoTrackScale.Location = new System.Drawing.Point(707, 43);
            this.trbGeoTrackScale.Name = "trbGeoTrackScale";
            this.trbGeoTrackScale.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tableLayoutPanel1.SetRowSpan(this.trbGeoTrackScale, 5);
            this.trbGeoTrackScale.Size = new System.Drawing.Size(37, 410);
            this.trbGeoTrackScale.TabIndex = 60;
            this.trbGeoTrackScale.ValueChanged += new System.EventHandler(this.trbGeoTrackScale_ValueChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 5);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.Controls.Add(this.cbNormalizeImage, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnIncreaseShorelineResolution, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDecreaseShorelineResolution, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.rtbTrackLineWidth, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 419);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(698, 34);
            this.tableLayoutPanel2.TabIndex = 64;
            // 
            // cbNormalizeImage
            // 
            this.cbNormalizeImage.AutoSize = true;
            this.cbNormalizeImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbNormalizeImage.Location = new System.Drawing.Point(360, 2);
            this.cbNormalizeImage.Margin = new System.Windows.Forms.Padding(2);
            this.cbNormalizeImage.Name = "cbNormalizeImage";
            this.cbNormalizeImage.Size = new System.Drawing.Size(76, 30);
            this.cbNormalizeImage.TabIndex = 63;
            this.cbNormalizeImage.Text = "norm. image";
            this.cbNormalizeImage.UseVisualStyleBackColor = true;
            this.cbNormalizeImage.CheckedChanged += new System.EventHandler(this.cbNormalizeImage_CheckedChanged);
            // 
            // btnIncreaseShorelineResolution
            // 
            this.btnIncreaseShorelineResolution.BackColor = System.Drawing.Color.Gainsboro;
            this.btnIncreaseShorelineResolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIncreaseShorelineResolution.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIncreaseShorelineResolution.Location = new System.Drawing.Point(181, 2);
            this.btnIncreaseShorelineResolution.Margin = new System.Windows.Forms.Padding(2);
            this.btnIncreaseShorelineResolution.Name = "btnIncreaseShorelineResolution";
            this.btnIncreaseShorelineResolution.Size = new System.Drawing.Size(175, 30);
            this.btnIncreaseShorelineResolution.TabIndex = 62;
            this.btnIncreaseShorelineResolution.Text = "Increase shoreline resolution";
            this.btnIncreaseShorelineResolution.UseVisualStyleBackColor = false;
            this.btnIncreaseShorelineResolution.Click += new System.EventHandler(this.btnIncreaseResolution_Click);
            // 
            // btnDecreaseShorelineResolution
            // 
            this.btnDecreaseShorelineResolution.BackColor = System.Drawing.Color.Gainsboro;
            this.btnDecreaseShorelineResolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDecreaseShorelineResolution.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecreaseShorelineResolution.Location = new System.Drawing.Point(2, 2);
            this.btnDecreaseShorelineResolution.Margin = new System.Windows.Forms.Padding(2);
            this.btnDecreaseShorelineResolution.Name = "btnDecreaseShorelineResolution";
            this.btnDecreaseShorelineResolution.Size = new System.Drawing.Size(175, 30);
            this.btnDecreaseShorelineResolution.TabIndex = 61;
            this.btnDecreaseShorelineResolution.Text = "Decrease shoreline resolution";
            this.btnDecreaseShorelineResolution.UseVisualStyleBackColor = false;
            this.btnDecreaseShorelineResolution.Click += new System.EventHandler(this.btnDecreaseResolution_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(441, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 34);
            this.label2.TabIndex = 64;
            this.label2.Text = "track line:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbTrackLineWidth
            // 
            this.rtbTrackLineWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbTrackLineWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbTrackLineWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbTrackLineWidth.Location = new System.Drawing.Point(521, 3);
            this.rtbTrackLineWidth.Name = "rtbTrackLineWidth";
            this.rtbTrackLineWidth.Size = new System.Drawing.Size(174, 28);
            this.rtbTrackLineWidth.TabIndex = 65;
            this.rtbTrackLineWidth.Text = "";
            this.rtbTrackLineWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtbTrackLineWidth_KeyPress);
            // 
            // lblStatusString
            // 
            this.lblStatusString.AutoSize = true;
            this.lblStatusString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatusString, 6);
            this.lblStatusString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusString.Location = new System.Drawing.Point(3, 456);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(741, 41);
            this.lblStatusString.TabIndex = 65;
            this.lblStatusString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bgwGeotrackRenderer
            // 
            this.bgwGeotrackRenderer.WorkerSupportsCancellation = true;
            this.bgwGeotrackRenderer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGeotrackRenderer_DoWork);
            // 
            // GeotrackPresentationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 497);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "GeotrackPresentationForm";
            this.Text = "Geotrack presentation form";
            this.Load += new System.EventHandler(this.GeotrackPresentationForm_Load);
            this.SizeChanged += new System.EventHandler(this.GeotrackPresentationForm_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GeotrackPresentationForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeotrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbGeoTrackScale)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCenterToActualPosition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdate;
        private MRG.Controls.UI.LoadingCircle wcUpdatimgGeoTrack;
        private System.Windows.Forms.PictureBox pbGeotrack;
        private System.Windows.Forms.TrackBar trbGeoTrackScale;
        private System.Windows.Forms.Button btnDecreaseShorelineResolution;
        private System.Windows.Forms.Button btnIncreaseShorelineResolution;
        private System.Windows.Forms.CheckBox cbNormalizeImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.ComponentModel.BackgroundWorker bgwGeotrackRenderer;
        private System.Windows.Forms.Label lblStatusString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbTrackLineWidth;
    }
}