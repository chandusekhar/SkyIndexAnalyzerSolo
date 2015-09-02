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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSunDiskCondition = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblCalculationProgressPercentage = new System.Windows.Forms.Label();
            this.currImagePictureBox = new System.Windows.Forms.PictureBox();
            this.btnProperties = new System.Windows.Forms.Button();
            this.lblImageTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMarkNoSun = new System.Windows.Forms.Button();
            this.btnMarkSun0 = new System.Windows.Forms.Button();
            this.btnMarkSun1 = new System.Windows.Forms.Button();
            this.btnMarkSun2 = new System.Windows.Forms.Button();
            this.btnMarkDefect = new System.Windows.Forms.Button();
            this.MedianPerc5DataTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblGrIxMedianTitle = new System.Windows.Forms.Label();
            this.lblGrIxPerc5Title = new System.Windows.Forms.Label();
            this.lblGrIxMedianValue = new System.Windows.Forms.Label();
            this.lblGrIxPerc5Value = new System.Windows.Forms.Label();
            this.cbCalculateGrIxStatsOnline = new System.Windows.Forms.CheckBox();
            this.circBgwProcessingImage = new MRG.Controls.UI.LoadingCircle();
            this.btnSwitchModes = new System.Windows.Forms.Button();
            this.tabGrIxStatsCalculationProgress = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabConvert = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbSourceDirectory = new System.Windows.Forms.RichTextBox();
            this.rtbOutputDirectory = new System.Windows.Forms.RichTextBox();
            this.btnSelectSourceDirectory = new System.Windows.Forms.Button();
            this.btnSelectOutputDirectory = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnServiceInputDirectory = new System.Windows.Forms.Button();
            this.rtbServiceSourceDirectory = new System.Windows.Forms.RichTextBox();
            this.lblServiceSourceDirectoryTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCalculateGrIxStatsForMarkedImages = new System.Windows.Forms.Button();
            this.btnCalculateGrIxStatsForAllImages = new System.Windows.Forms.Button();
            this.btnCalculateAllVarsStats = new System.Windows.Forms.Button();
            this.btnStopCalculations = new System.Windows.Forms.Button();
            this.prbConversionProgress = new System.Windows.Forms.ProgressBar();
            this.tabTrainAndPredict = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.prgBarMLprogress = new System.Windows.Forms.ProgressBar();
            this.lblMLdataSourceDirectoryTitle = new System.Windows.Forms.Label();
            this.rtbMLdataSourceDirectory = new System.Windows.Forms.RichTextBox();
            this.btnReadData = new System.Windows.Forms.Button();
            this.btnMLdataSourceDirectoryBrowse = new System.Windows.Forms.Button();
            this.btnLearn = new System.Windows.Forms.Button();
            this.decisionTreeView1 = new Accord.Controls.DecisionTreeView();
            this.dgvPerformance = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabSunDiskCondition.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currImagePictureBox)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.MedianPerc5DataTable.SuspendLayout();
            this.tabGrIxStatsCalculationProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabConvert.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tabTrainAndPredict.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerformance)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSunDiskCondition);
            this.tabControl1.Controls.Add(this.tabGrIxStatsCalculationProgress);
            this.tabControl1.Controls.Add(this.tabConvert);
            this.tabControl1.Controls.Add(this.tabTrainAndPredict);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1060, 727);
            this.tabControl1.TabIndex = 0;
            // 
            // tabSunDiskCondition
            // 
            this.tabSunDiskCondition.Controls.Add(this.tableLayoutPanel1);
            this.tabSunDiskCondition.Location = new System.Drawing.Point(4, 25);
            this.tabSunDiskCondition.Name = "tabSunDiskCondition";
            this.tabSunDiskCondition.Padding = new System.Windows.Forms.Padding(3);
            this.tabSunDiskCondition.Size = new System.Drawing.Size(1052, 698);
            this.tabSunDiskCondition.TabIndex = 0;
            this.tabSunDiskCondition.Text = "Sun disk condition";
            this.tabSunDiskCondition.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.lblCalculationProgressPercentage, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.currImagePictureBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnProperties, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblImageTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.MedianPerc5DataTable, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbCalculateGrIxStatsOnline, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.circBgwProcessingImage, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSwitchModes, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1046, 692);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblCalculationProgressPercentage
            // 
            this.lblCalculationProgressPercentage.AutoSize = true;
            this.lblCalculationProgressPercentage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCalculationProgressPercentage, 2);
            this.lblCalculationProgressPercentage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCalculationProgressPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalculationProgressPercentage.Location = new System.Drawing.Point(929, 60);
            this.lblCalculationProgressPercentage.Name = "lblCalculationProgressPercentage";
            this.lblCalculationProgressPercentage.Size = new System.Drawing.Size(114, 60);
            this.lblCalculationProgressPercentage.TabIndex = 7;
            this.lblCalculationProgressPercentage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCalculationProgressPercentage.Visible = false;
            // 
            // currImagePictureBox
            // 
            this.currImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.currImagePictureBox, 4);
            this.currImagePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currImagePictureBox.Location = new System.Drawing.Point(3, 223);
            this.currImagePictureBox.Name = "currImagePictureBox";
            this.tableLayoutPanel1.SetRowSpan(this.currImagePictureBox, 4);
            this.currImagePictureBox.Size = new System.Drawing.Size(1040, 466);
            this.currImagePictureBox.TabIndex = 0;
            this.currImagePictureBox.TabStop = false;
            // 
            // btnProperties
            // 
            this.btnProperties.BackColor = System.Drawing.Color.Gainsboro;
            this.btnProperties.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProperties.BackgroundImage")));
            this.btnProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProperties.Location = new System.Drawing.Point(989, 3);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(54, 54);
            this.btnProperties.TabIndex = 1;
            this.btnProperties.UseVisualStyleBackColor = false;
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
            this.lblImageTitle.Size = new System.Drawing.Size(920, 60);
            this.lblImageTitle.TabIndex = 2;
            this.lblImageTitle.Text = "---";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnMarkNoSun, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMarkSun0, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMarkSun1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMarkSun2, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMarkDefect, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 123);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(457, 94);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btnMarkNoSun
            // 
            this.btnMarkNoSun.BackColor = System.Drawing.Color.Gainsboro;
            this.btnMarkNoSun.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.nosun1;
            this.btnMarkNoSun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkNoSun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkNoSun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkNoSun.Location = new System.Drawing.Point(3, 3);
            this.btnMarkNoSun.Name = "btnMarkNoSun";
            this.btnMarkNoSun.Size = new System.Drawing.Size(85, 88);
            this.btnMarkNoSun.TabIndex = 0;
            this.btnMarkNoSun.UseVisualStyleBackColor = false;
            this.btnMarkNoSun.Click += new System.EventHandler(this.MarkSunCondition_Click);
            // 
            // btnMarkSun0
            // 
            this.btnMarkSun0.BackColor = System.Drawing.Color.Gainsboro;
            this.btnMarkSun0.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.sun01;
            this.btnMarkSun0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkSun0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkSun0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkSun0.Location = new System.Drawing.Point(94, 3);
            this.btnMarkSun0.Name = "btnMarkSun0";
            this.btnMarkSun0.Size = new System.Drawing.Size(85, 88);
            this.btnMarkSun0.TabIndex = 1;
            this.btnMarkSun0.UseVisualStyleBackColor = false;
            this.btnMarkSun0.Click += new System.EventHandler(this.MarkSunCondition_Click);
            // 
            // btnMarkSun1
            // 
            this.btnMarkSun1.BackColor = System.Drawing.Color.Gainsboro;
            this.btnMarkSun1.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.sun11;
            this.btnMarkSun1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkSun1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkSun1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkSun1.Location = new System.Drawing.Point(185, 3);
            this.btnMarkSun1.Name = "btnMarkSun1";
            this.btnMarkSun1.Size = new System.Drawing.Size(85, 88);
            this.btnMarkSun1.TabIndex = 2;
            this.btnMarkSun1.UseVisualStyleBackColor = false;
            this.btnMarkSun1.Click += new System.EventHandler(this.MarkSunCondition_Click);
            // 
            // btnMarkSun2
            // 
            this.btnMarkSun2.BackColor = System.Drawing.Color.Gainsboro;
            this.btnMarkSun2.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.sun21;
            this.btnMarkSun2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkSun2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkSun2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkSun2.Location = new System.Drawing.Point(276, 3);
            this.btnMarkSun2.Name = "btnMarkSun2";
            this.btnMarkSun2.Size = new System.Drawing.Size(85, 88);
            this.btnMarkSun2.TabIndex = 3;
            this.btnMarkSun2.UseVisualStyleBackColor = false;
            this.btnMarkSun2.Click += new System.EventHandler(this.MarkSunCondition_Click);
            // 
            // btnMarkDefect
            // 
            this.btnMarkDefect.BackColor = System.Drawing.Color.Gainsboro;
            this.btnMarkDefect.BackgroundImage = global::SunPresenceCollectingApp.Properties.Resources.sadSmile;
            this.btnMarkDefect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMarkDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMarkDefect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkDefect.Location = new System.Drawing.Point(367, 3);
            this.btnMarkDefect.Name = "btnMarkDefect";
            this.btnMarkDefect.Size = new System.Drawing.Size(87, 88);
            this.btnMarkDefect.TabIndex = 4;
            this.btnMarkDefect.UseVisualStyleBackColor = false;
            this.btnMarkDefect.Click += new System.EventHandler(this.MarkSunCondition_Click);
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
            this.MedianPerc5DataTable.Location = new System.Drawing.Point(466, 123);
            this.MedianPerc5DataTable.Name = "MedianPerc5DataTable";
            this.MedianPerc5DataTable.RowCount = 2;
            this.MedianPerc5DataTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MedianPerc5DataTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MedianPerc5DataTable.Size = new System.Drawing.Size(577, 94);
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
            this.lblGrIxMedianTitle.Size = new System.Drawing.Size(198, 47);
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
            this.lblGrIxPerc5Title.Size = new System.Drawing.Size(198, 47);
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
            this.lblGrIxMedianValue.Location = new System.Drawing.Point(207, 0);
            this.lblGrIxMedianValue.Name = "lblGrIxMedianValue";
            this.lblGrIxMedianValue.Size = new System.Drawing.Size(367, 47);
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
            this.lblGrIxPerc5Value.Location = new System.Drawing.Point(207, 47);
            this.lblGrIxPerc5Value.Name = "lblGrIxPerc5Value";
            this.lblGrIxPerc5Value.Size = new System.Drawing.Size(367, 47);
            this.lblGrIxPerc5Value.TabIndex = 3;
            this.lblGrIxPerc5Value.Text = "---";
            this.lblGrIxPerc5Value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbCalculateGrIxStatsOnline
            // 
            this.cbCalculateGrIxStatsOnline.AutoSize = true;
            this.cbCalculateGrIxStatsOnline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCalculateGrIxStatsOnline.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCalculateGrIxStatsOnline.Location = new System.Drawing.Point(3, 63);
            this.cbCalculateGrIxStatsOnline.Name = "cbCalculateGrIxStatsOnline";
            this.cbCalculateGrIxStatsOnline.Size = new System.Drawing.Size(457, 54);
            this.cbCalculateGrIxStatsOnline.TabIndex = 6;
            this.cbCalculateGrIxStatsOnline.Text = "Попутно обсчитывать статистику полей";
            this.cbCalculateGrIxStatsOnline.UseVisualStyleBackColor = true;
            this.cbCalculateGrIxStatsOnline.CheckedChanged += new System.EventHandler(this.cbCalculateGrIxStatsOnline_CheckedChanged);
            // 
            // circBgwProcessingImage
            // 
            this.circBgwProcessingImage.Active = false;
            this.circBgwProcessingImage.Color = System.Drawing.Color.SteelBlue;
            this.circBgwProcessingImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.circBgwProcessingImage.InnerCircleRadius = 8;
            this.circBgwProcessingImage.Location = new System.Drawing.Point(929, 3);
            this.circBgwProcessingImage.Name = "circBgwProcessingImage";
            this.circBgwProcessingImage.NumberSpoke = 24;
            this.circBgwProcessingImage.OuterCircleRadius = 9;
            this.circBgwProcessingImage.RotationSpeed = 100;
            this.circBgwProcessingImage.Size = new System.Drawing.Size(54, 54);
            this.circBgwProcessingImage.SpokeThickness = 4;
            this.circBgwProcessingImage.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.circBgwProcessingImage.TabIndex = 8;
            this.circBgwProcessingImage.Text = "loadingCircle1";
            this.circBgwProcessingImage.Visible = false;
            // 
            // btnSwitchModes
            // 
            this.btnSwitchModes.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSwitchModes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSwitchModes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchModes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwitchModes.Location = new System.Drawing.Point(466, 63);
            this.btnSwitchModes.Name = "btnSwitchModes";
            this.btnSwitchModes.Size = new System.Drawing.Size(457, 54);
            this.btnSwitchModes.TabIndex = 9;
            this.btnSwitchModes.Text = "Standard mode";
            this.btnSwitchModes.UseVisualStyleBackColor = false;
            this.btnSwitchModes.Click += new System.EventHandler(this.btnSwitchModes_Click);
            // 
            // tabGrIxStatsCalculationProgress
            // 
            this.tabGrIxStatsCalculationProgress.Controls.Add(this.dataGridView1);
            this.tabGrIxStatsCalculationProgress.Location = new System.Drawing.Point(4, 25);
            this.tabGrIxStatsCalculationProgress.Name = "tabGrIxStatsCalculationProgress";
            this.tabGrIxStatsCalculationProgress.Padding = new System.Windows.Forms.Padding(3);
            this.tabGrIxStatsCalculationProgress.Size = new System.Drawing.Size(1052, 698);
            this.tabGrIxStatsCalculationProgress.TabIndex = 1;
            this.tabGrIxStatsCalculationProgress.Text = "Stats collecting progress";
            this.tabGrIxStatsCalculationProgress.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1046, 692);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabConvert
            // 
            this.tabConvert.Controls.Add(this.tableLayoutPanel3);
            this.tabConvert.Location = new System.Drawing.Point(4, 25);
            this.tabConvert.Name = "tabConvert";
            this.tabConvert.Size = new System.Drawing.Size(1052, 698);
            this.tabConvert.TabIndex = 2;
            this.tabConvert.Text = "Convert and service";
            this.tabConvert.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.23574F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.76426F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.rtbSourceDirectory, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.rtbOutputDirectory, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnSelectSourceDirectory, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSelectOutputDirectory, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnConvert, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnServiceInputDirectory, 2, 5);
            this.tableLayoutPanel3.Controls.Add(this.rtbServiceSourceDirectory, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.lblServiceSourceDirectoryTitle, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.btnStopCalculations, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.prbConversionProgress, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 9;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1052, 698);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source directory";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(251, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "Output directory";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbSourceDirectory
            // 
            this.rtbSourceDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSourceDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbSourceDirectory.Location = new System.Drawing.Point(260, 3);
            this.rtbSourceDirectory.Name = "rtbSourceDirectory";
            this.rtbSourceDirectory.Size = new System.Drawing.Size(719, 34);
            this.rtbSourceDirectory.TabIndex = 2;
            this.rtbSourceDirectory.Text = "";
            // 
            // rtbOutputDirectory
            // 
            this.rtbOutputDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOutputDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOutputDirectory.Location = new System.Drawing.Point(260, 43);
            this.rtbOutputDirectory.Name = "rtbOutputDirectory";
            this.rtbOutputDirectory.Size = new System.Drawing.Size(719, 34);
            this.rtbOutputDirectory.TabIndex = 3;
            this.rtbOutputDirectory.Text = "";
            // 
            // btnSelectSourceDirectory
            // 
            this.btnSelectSourceDirectory.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSelectSourceDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectSourceDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectSourceDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectSourceDirectory.Location = new System.Drawing.Point(985, 3);
            this.btnSelectSourceDirectory.Name = "btnSelectSourceDirectory";
            this.btnSelectSourceDirectory.Size = new System.Drawing.Size(64, 34);
            this.btnSelectSourceDirectory.TabIndex = 4;
            this.btnSelectSourceDirectory.Text = "...";
            this.btnSelectSourceDirectory.UseVisualStyleBackColor = false;
            this.btnSelectSourceDirectory.Click += new System.EventHandler(this.btnSelectSourceDirectory_Click);
            // 
            // btnSelectOutputDirectory
            // 
            this.btnSelectOutputDirectory.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSelectOutputDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectOutputDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectOutputDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectOutputDirectory.Location = new System.Drawing.Point(985, 43);
            this.btnSelectOutputDirectory.Name = "btnSelectOutputDirectory";
            this.btnSelectOutputDirectory.Size = new System.Drawing.Size(64, 34);
            this.btnSelectOutputDirectory.TabIndex = 5;
            this.btnSelectOutputDirectory.Text = "...";
            this.btnSelectOutputDirectory.UseVisualStyleBackColor = false;
            this.btnSelectOutputDirectory.Click += new System.EventHandler(this.btnSelectOutputDirectory_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.BackColor = System.Drawing.Color.Gainsboro;
            this.btnConvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvert.Location = new System.Drawing.Point(260, 83);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(719, 114);
            this.btnConvert.TabIndex = 6;
            this.btnConvert.Text = "convert to CSV files (one for (M,P5) predictors and one for all statistical predi" +
    "ctors)";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnServiceInputDirectory
            // 
            this.btnServiceInputDirectory.BackColor = System.Drawing.Color.Gainsboro;
            this.btnServiceInputDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnServiceInputDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServiceInputDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServiceInputDirectory.Location = new System.Drawing.Point(985, 343);
            this.btnServiceInputDirectory.Name = "btnServiceInputDirectory";
            this.btnServiceInputDirectory.Size = new System.Drawing.Size(64, 34);
            this.btnServiceInputDirectory.TabIndex = 7;
            this.btnServiceInputDirectory.Text = "...";
            this.btnServiceInputDirectory.UseVisualStyleBackColor = false;
            this.btnServiceInputDirectory.Click += new System.EventHandler(this.btnServiceInputDirectory_Click);
            // 
            // rtbServiceSourceDirectory
            // 
            this.rtbServiceSourceDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbServiceSourceDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbServiceSourceDirectory.Location = new System.Drawing.Point(260, 343);
            this.rtbServiceSourceDirectory.Name = "rtbServiceSourceDirectory";
            this.rtbServiceSourceDirectory.Size = new System.Drawing.Size(719, 34);
            this.rtbServiceSourceDirectory.TabIndex = 8;
            this.rtbServiceSourceDirectory.Text = "";
            // 
            // lblServiceSourceDirectoryTitle
            // 
            this.lblServiceSourceDirectoryTitle.AutoSize = true;
            this.lblServiceSourceDirectoryTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblServiceSourceDirectoryTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServiceSourceDirectoryTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceSourceDirectoryTitle.Location = new System.Drawing.Point(3, 340);
            this.lblServiceSourceDirectoryTitle.Name = "lblServiceSourceDirectoryTitle";
            this.lblServiceSourceDirectoryTitle.Size = new System.Drawing.Size(251, 40);
            this.lblServiceSourceDirectoryTitle.TabIndex = 9;
            this.lblServiceSourceDirectoryTitle.Text = "Source directory:";
            this.lblServiceSourceDirectoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel4, 3);
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.btnCalculateGrIxStatsForMarkedImages, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnCalculateGrIxStatsForAllImages, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnCalculateAllVarsStats, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 383);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1046, 144);
            this.tableLayoutPanel4.TabIndex = 10;
            // 
            // btnCalculateGrIxStatsForMarkedImages
            // 
            this.btnCalculateGrIxStatsForMarkedImages.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCalculateGrIxStatsForMarkedImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalculateGrIxStatsForMarkedImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculateGrIxStatsForMarkedImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalculateGrIxStatsForMarkedImages.Location = new System.Drawing.Point(699, 3);
            this.btnCalculateGrIxStatsForMarkedImages.Name = "btnCalculateGrIxStatsForMarkedImages";
            this.btnCalculateGrIxStatsForMarkedImages.Size = new System.Drawing.Size(344, 138);
            this.btnCalculateGrIxStatsForMarkedImages.TabIndex = 0;
            this.btnCalculateGrIxStatsForMarkedImages.Text = "Calculate GrIx field stats for images already marked";
            this.btnCalculateGrIxStatsForMarkedImages.UseVisualStyleBackColor = false;
            this.btnCalculateGrIxStatsForMarkedImages.Click += new System.EventHandler(this.btnCalculateGrIxStatsForMarkedImages_Click);
            // 
            // btnCalculateGrIxStatsForAllImages
            // 
            this.btnCalculateGrIxStatsForAllImages.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCalculateGrIxStatsForAllImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalculateGrIxStatsForAllImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculateGrIxStatsForAllImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalculateGrIxStatsForAllImages.Location = new System.Drawing.Point(351, 3);
            this.btnCalculateGrIxStatsForAllImages.Name = "btnCalculateGrIxStatsForAllImages";
            this.btnCalculateGrIxStatsForAllImages.Size = new System.Drawing.Size(342, 138);
            this.btnCalculateGrIxStatsForAllImages.TabIndex = 1;
            this.btnCalculateGrIxStatsForAllImages.Text = "Calculate GrIx field stats for ALL images";
            this.btnCalculateGrIxStatsForAllImages.UseVisualStyleBackColor = false;
            this.btnCalculateGrIxStatsForAllImages.Click += new System.EventHandler(this.btnCalculateGrIxStatsForAllImages_Click);
            // 
            // btnCalculateAllVarsStats
            // 
            this.btnCalculateAllVarsStats.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCalculateAllVarsStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalculateAllVarsStats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculateAllVarsStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalculateAllVarsStats.Location = new System.Drawing.Point(3, 3);
            this.btnCalculateAllVarsStats.Name = "btnCalculateAllVarsStats";
            this.btnCalculateAllVarsStats.Size = new System.Drawing.Size(342, 138);
            this.btnCalculateAllVarsStats.TabIndex = 2;
            this.btnCalculateAllVarsStats.Text = "Calculate GrIx,Y,R,G,B fields stats for images already marked";
            this.btnCalculateAllVarsStats.UseVisualStyleBackColor = false;
            this.btnCalculateAllVarsStats.Click += new System.EventHandler(this.btnCalculateAllVarsStats_Click);
            // 
            // btnStopCalculations
            // 
            this.btnStopCalculations.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel3.SetColumnSpan(this.btnStopCalculations, 3);
            this.btnStopCalculations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStopCalculations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopCalculations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopCalculations.Location = new System.Drawing.Point(3, 533);
            this.btnStopCalculations.Name = "btnStopCalculations";
            this.btnStopCalculations.Size = new System.Drawing.Size(1046, 54);
            this.btnStopCalculations.TabIndex = 11;
            this.btnStopCalculations.Text = "STOP calculations";
            this.btnStopCalculations.UseVisualStyleBackColor = false;
            this.btnStopCalculations.Click += new System.EventHandler(this.btnStopCalculations_Click);
            // 
            // prbConversionProgress
            // 
            this.prbConversionProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbConversionProgress.Location = new System.Drawing.Point(260, 203);
            this.prbConversionProgress.Name = "prbConversionProgress";
            this.prbConversionProgress.Size = new System.Drawing.Size(719, 14);
            this.prbConversionProgress.TabIndex = 12;
            // 
            // tabTrainAndPredict
            // 
            this.tabTrainAndPredict.Controls.Add(this.tableLayoutPanel5);
            this.tabTrainAndPredict.Location = new System.Drawing.Point(4, 25);
            this.tabTrainAndPredict.Name = "tabTrainAndPredict";
            this.tabTrainAndPredict.Padding = new System.Windows.Forms.Padding(3);
            this.tabTrainAndPredict.Size = new System.Drawing.Size(1052, 698);
            this.tabTrainAndPredict.TabIndex = 3;
            this.tabTrainAndPredict.Text = "Train and predict";
            this.tabTrainAndPredict.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 6;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00001F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel5.Controls.Add(this.prgBarMLprogress, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblMLdataSourceDirectoryTitle, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.rtbMLdataSourceDirectory, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnReadData, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.btnMLdataSourceDirectoryBrowse, 5, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnLearn, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.decisionTreeView1, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.dgvPerformance, 0, 6);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 8;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1046, 692);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // prgBarMLprogress
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.prgBarMLprogress, 6);
            this.prgBarMLprogress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgBarMLprogress.Location = new System.Drawing.Point(3, 43);
            this.prgBarMLprogress.Name = "prgBarMLprogress";
            this.prgBarMLprogress.Size = new System.Drawing.Size(1040, 14);
            this.prgBarMLprogress.TabIndex = 1;
            // 
            // lblMLdataSourceDirectoryTitle
            // 
            this.lblMLdataSourceDirectoryTitle.AutoSize = true;
            this.lblMLdataSourceDirectoryTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMLdataSourceDirectoryTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMLdataSourceDirectoryTitle.Location = new System.Drawing.Point(3, 0);
            this.lblMLdataSourceDirectoryTitle.Name = "lblMLdataSourceDirectoryTitle";
            this.lblMLdataSourceDirectoryTitle.Size = new System.Drawing.Size(189, 40);
            this.lblMLdataSourceDirectoryTitle.TabIndex = 2;
            this.lblMLdataSourceDirectoryTitle.Text = "Source directory:";
            this.lblMLdataSourceDirectoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbMLdataSourceDirectory
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.rtbMLdataSourceDirectory, 4);
            this.rtbMLdataSourceDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMLdataSourceDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbMLdataSourceDirectory.Location = new System.Drawing.Point(198, 3);
            this.rtbMLdataSourceDirectory.Name = "rtbMLdataSourceDirectory";
            this.rtbMLdataSourceDirectory.Size = new System.Drawing.Size(770, 34);
            this.rtbMLdataSourceDirectory.TabIndex = 3;
            this.rtbMLdataSourceDirectory.Text = "";
            // 
            // btnReadData
            // 
            this.btnReadData.BackColor = System.Drawing.Color.Gainsboro;
            this.btnReadData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadData.Location = new System.Drawing.Point(3, 63);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(189, 54);
            this.btnReadData.TabIndex = 0;
            this.btnReadData.Text = "Read data";
            this.btnReadData.UseVisualStyleBackColor = false;
            this.btnReadData.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnMLdataSourceDirectoryBrowse
            // 
            this.btnMLdataSourceDirectoryBrowse.BackColor = System.Drawing.Color.Gainsboro;
            this.btnMLdataSourceDirectoryBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMLdataSourceDirectoryBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMLdataSourceDirectoryBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMLdataSourceDirectoryBrowse.Location = new System.Drawing.Point(974, 3);
            this.btnMLdataSourceDirectoryBrowse.Name = "btnMLdataSourceDirectoryBrowse";
            this.btnMLdataSourceDirectoryBrowse.Size = new System.Drawing.Size(69, 34);
            this.btnMLdataSourceDirectoryBrowse.TabIndex = 4;
            this.btnMLdataSourceDirectoryBrowse.Text = "...";
            this.btnMLdataSourceDirectoryBrowse.UseVisualStyleBackColor = false;
            this.btnMLdataSourceDirectoryBrowse.Click += new System.EventHandler(this.btnMLdataSourceDirectoryBrowse_Click);
            // 
            // btnLearn
            // 
            this.btnLearn.BackColor = System.Drawing.Color.Gainsboro;
            this.btnLearn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLearn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLearn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLearn.Location = new System.Drawing.Point(198, 63);
            this.btnLearn.Name = "btnLearn";
            this.btnLearn.Size = new System.Drawing.Size(188, 54);
            this.btnLearn.TabIndex = 5;
            this.btnLearn.Text = "Learn and predict";
            this.btnLearn.UseVisualStyleBackColor = false;
            this.btnLearn.Click += new System.EventHandler(this.btnLearn_Click);
            // 
            // decisionTreeView1
            // 
            this.decisionTreeView1.Codebook = null;
            this.tableLayoutPanel5.SetColumnSpan(this.decisionTreeView1, 6);
            this.decisionTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.decisionTreeView1.Location = new System.Drawing.Point(3, 124);
            this.decisionTreeView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.decisionTreeView1.Name = "decisionTreeView1";
            this.tableLayoutPanel5.SetRowSpan(this.decisionTreeView1, 3);
            this.decisionTreeView1.Size = new System.Drawing.Size(1040, 334);
            this.decisionTreeView1.TabIndex = 6;
            this.decisionTreeView1.TreeSource = null;
            // 
            // dgvPerformance
            // 
            this.dgvPerformance.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPerformance.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvPerformance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel5.SetColumnSpan(this.dgvPerformance, 6);
            this.dgvPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPerformance.Location = new System.Drawing.Point(3, 465);
            this.dgvPerformance.Name = "dgvPerformance";
            this.tableLayoutPanel5.SetRowSpan(this.dgvPerformance, 2);
            this.dgvPerformance.RowTemplate.Height = 24;
            this.dgvPerformance.Size = new System.Drawing.Size(1040, 224);
            this.dgvPerformance.TabIndex = 7;
            // 
            // SunPresenceCollectingMainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 727);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "SunPresenceCollectingMainForm";
            this.Text = "Collect sun presence features";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SunPresenceCollectingMainForm_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.SunPresenceCollectingMainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.SunPresenceCollectingMainForm_DragEnter);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SunPresenceCollectingMainForm_KeyPress);
            this.tabControl1.ResumeLayout(false);
            this.tabSunDiskCondition.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currImagePictureBox)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.MedianPerc5DataTable.ResumeLayout(false);
            this.MedianPerc5DataTable.PerformLayout();
            this.tabGrIxStatsCalculationProgress.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabConvert.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tabTrainAndPredict.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerformance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSunDiskCondition;
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
        private System.Windows.Forms.TabPage tabGrIxStatsCalculationProgress;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox cbCalculateGrIxStatsOnline;
        private System.Windows.Forms.TabPage tabConvert;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbSourceDirectory;
        private System.Windows.Forms.RichTextBox rtbOutputDirectory;
        private System.Windows.Forms.Button btnSelectSourceDirectory;
        private System.Windows.Forms.Button btnSelectOutputDirectory;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnServiceInputDirectory;
        private System.Windows.Forms.RichTextBox rtbServiceSourceDirectory;
        private System.Windows.Forms.Label lblServiceSourceDirectoryTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnCalculateGrIxStatsForMarkedImages;
        private System.Windows.Forms.Button btnCalculateGrIxStatsForAllImages;
        private System.Windows.Forms.Label lblCalculationProgressPercentage;
        private MRG.Controls.UI.LoadingCircle circBgwProcessingImage;
        private System.Windows.Forms.Button btnCalculateAllVarsStats;
        private System.Windows.Forms.Button btnStopCalculations;
        private System.Windows.Forms.ProgressBar prbConversionProgress;
        private System.Windows.Forms.Button btnSwitchModes;
        private System.Windows.Forms.Button btnMarkDefect;
        private System.Windows.Forms.TabPage tabTrainAndPredict;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.ProgressBar prgBarMLprogress;
        private System.Windows.Forms.Label lblMLdataSourceDirectoryTitle;
        private System.Windows.Forms.RichTextBox rtbMLdataSourceDirectory;
        private System.Windows.Forms.Button btnMLdataSourceDirectoryBrowse;
        private System.Windows.Forms.Button btnLearn;
        private Accord.Controls.DecisionTreeView decisionTreeView1;
        private System.Windows.Forms.DataGridView dgvPerformance;
    }
}

