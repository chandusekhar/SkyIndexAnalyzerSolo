namespace CloudCamsDataAnalysis
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageCamerasLocationAnalysis = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pbImageID1 = new System.Windows.Forms.PictureBox();
            this.pbImageID2 = new System.Windows.Forms.PictureBox();
            this.hScrollBarID1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBarID1 = new System.Windows.Forms.VScrollBar();
            this.vScrollBarID2 = new System.Windows.Forms.VScrollBar();
            this.hScrollBarID2 = new System.Windows.Forms.HScrollBar();
            this.btnImg1DetectSun = new System.Windows.Forms.Button();
            this.btnImg2DetectSun = new System.Windows.Forms.Button();
            this.tbSunLocationID1 = new System.Windows.Forms.TextBox();
            this.tbSunLocationID2 = new System.Windows.Forms.TextBox();
            this.btnOpenFileID1 = new System.Windows.Forms.Button();
            this.btnOpenFileID2 = new System.Windows.Forms.Button();
            this.lblControlPoint1id1Title = new System.Windows.Forms.Label();
            this.lblControlPoint2id1Title = new System.Windows.Forms.Label();
            this.lblControlPoint1id2Title = new System.Windows.Forms.Label();
            this.lblControlPoint2id2Title = new System.Windows.Forms.Label();
            this.tbControlPoint1id1 = new System.Windows.Forms.TextBox();
            this.tbControlPoint2id1 = new System.Windows.Forms.TextBox();
            this.tbControlPoint1id2 = new System.Windows.Forms.TextBox();
            this.tbControlPoint2id2 = new System.Windows.Forms.TextBox();
            this.btnStoreStats = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSunLocationProcessingDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowseDirectoryForSunLocationProcessing = new System.Windows.Forms.Button();
            this.btnSunLocationProcessing = new System.Windows.Forms.Button();
            this.trbImgID1ScaleValue = new System.Windows.Forms.TrackBar();
            this.trbImgID2ScaleValue = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnImgID1Center = new System.Windows.Forms.Button();
            this.btnImgID2Center = new System.Windows.Forms.Button();
            this.lblImageID1Comments = new System.Windows.Forms.Label();
            this.lblImageID2Comments = new System.Windows.Forms.Label();
            this.lcCalculatingImageID1RoundData = new MRG.Controls.UI.LoadingCircle();
            this.lcCalculatingImageID2RoundData = new MRG.Controls.UI.LoadingCircle();
            this.tabPageObjectsLocationAnalysis = new System.Windows.Forms.TabPage();
            this.tabPageProperties = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPageCamerasLocationAnalysis.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageID1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageID2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbImgID1ScaleValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbImgID2ScaleValue)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageCamerasLocationAnalysis);
            this.tabControl1.Controls.Add(this.tabPageObjectsLocationAnalysis);
            this.tabControl1.Controls.Add(this.tabPageProperties);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1371, 866);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageCamerasLocationAnalysis
            // 
            this.tabPageCamerasLocationAnalysis.Controls.Add(this.tableLayoutPanel1);
            this.tabPageCamerasLocationAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPageCamerasLocationAnalysis.Location = new System.Drawing.Point(4, 29);
            this.tabPageCamerasLocationAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageCamerasLocationAnalysis.Name = "tabPageCamerasLocationAnalysis";
            this.tabPageCamerasLocationAnalysis.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageCamerasLocationAnalysis.Size = new System.Drawing.Size(1363, 833);
            this.tabPageCamerasLocationAnalysis.TabIndex = 0;
            this.tabPageCamerasLocationAnalysis.Text = "Outdoor sets location analysis";
            this.tabPageCamerasLocationAnalysis.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 15;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.Controls.Add(this.pbImageID1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pbImageID2, 8, 2);
            this.tableLayoutPanel1.Controls.Add(this.hScrollBarID1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.vScrollBarID1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.vScrollBarID2, 14, 2);
            this.tableLayoutPanel1.Controls.Add(this.hScrollBarID2, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnImg1DetectSun, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnImg2DetectSun, 8, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbSunLocationID1, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbSunLocationID2, 9, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenFileID1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenFileID2, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblControlPoint1id1Title, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.lblControlPoint2id1Title, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblControlPoint1id2Title, 8, 10);
            this.tableLayoutPanel1.Controls.Add(this.lblControlPoint2id2Title, 8, 11);
            this.tableLayoutPanel1.Controls.Add(this.tbControlPoint1id1, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.tbControlPoint2id1, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.tbControlPoint1id2, 9, 10);
            this.tableLayoutPanel1.Controls.Add(this.tbControlPoint2id2, 9, 11);
            this.tableLayoutPanel1.Controls.Add(this.btnStoreStats, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbSunLocationProcessingDirectory, 5, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowseDirectoryForSunLocationProcessing, 12, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnSunLocationProcessing, 13, 8);
            this.tableLayoutPanel1.Controls.Add(this.trbImgID1ScaleValue, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.trbImgID2ScaleValue, 12, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnImgID1Center, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnImgID2Center, 14, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblImageID1Comments, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblImageID2Comments, 8, 7);
            this.tableLayoutPanel1.Controls.Add(this.lcCalculatingImageID1RoundData, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lcCalculatingImageID2RoundData, 14, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1355, 825);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pbImageID1
            // 
            this.pbImageID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbImageID1, 6);
            this.pbImageID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImageID1.Location = new System.Drawing.Point(57, 102);
            this.pbImageID1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbImageID1.Name = "pbImageID1";
            this.tableLayoutPanel1.SetRowSpan(this.pbImageID1, 5);
            this.pbImageID1.Size = new System.Drawing.Size(610, 363);
            this.pbImageID1.TabIndex = 0;
            this.pbImageID1.TabStop = false;
            this.pbImageID1.Click += new System.EventHandler(this.pbImageID1_Click);
            // 
            // pbImageID2
            // 
            this.pbImageID2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbImageID2, 6);
            this.pbImageID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImageID2.Location = new System.Drawing.Point(688, 102);
            this.pbImageID2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbImageID2.Name = "pbImageID2";
            this.tableLayoutPanel1.SetRowSpan(this.pbImageID2, 5);
            this.pbImageID2.Size = new System.Drawing.Size(610, 363);
            this.pbImageID2.TabIndex = 1;
            this.pbImageID2.TabStop = false;
            this.pbImageID2.Click += new System.EventHandler(this.pbImageID2_Click);
            // 
            // hScrollBarID1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.hScrollBarID1, 6);
            this.hScrollBarID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hScrollBarID1.Location = new System.Drawing.Point(53, 49);
            this.hScrollBarID1.Name = "hScrollBarID1";
            this.hScrollBarID1.Size = new System.Drawing.Size(618, 49);
            this.hScrollBarID1.TabIndex = 16;
            this.hScrollBarID1.Value = 50;
            this.hScrollBarID1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBarsScroll);
            // 
            // vScrollBarID1
            // 
            this.vScrollBarID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBarID1.Location = new System.Drawing.Point(0, 98);
            this.vScrollBarID1.Name = "vScrollBarID1";
            this.tableLayoutPanel1.SetRowSpan(this.vScrollBarID1, 5);
            this.vScrollBarID1.Size = new System.Drawing.Size(53, 371);
            this.vScrollBarID1.TabIndex = 17;
            this.vScrollBarID1.Value = 50;
            this.vScrollBarID1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBarsScroll);
            // 
            // vScrollBarID2
            // 
            this.vScrollBarID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBarID2.Location = new System.Drawing.Point(1302, 98);
            this.vScrollBarID2.Name = "vScrollBarID2";
            this.tableLayoutPanel1.SetRowSpan(this.vScrollBarID2, 5);
            this.vScrollBarID2.Size = new System.Drawing.Size(53, 371);
            this.vScrollBarID2.TabIndex = 18;
            this.vScrollBarID2.Value = 50;
            this.vScrollBarID2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBarsScroll);
            // 
            // hScrollBarID2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.hScrollBarID2, 6);
            this.hScrollBarID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hScrollBarID2.Location = new System.Drawing.Point(684, 49);
            this.hScrollBarID2.Name = "hScrollBarID2";
            this.hScrollBarID2.Size = new System.Drawing.Size(618, 49);
            this.hScrollBarID2.TabIndex = 19;
            this.hScrollBarID2.Value = 50;
            this.hScrollBarID2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBarsScroll);
            // 
            // btnImg1DetectSun
            // 
            this.btnImg1DetectSun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImg1DetectSun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImg1DetectSun.Image = global::CloudCamsDataAnalysis.Properties.Resources._171_sun;
            this.btnImg1DetectSun.Location = new System.Drawing.Point(4, 620);
            this.btnImg1DetectSun.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImg1DetectSun.Name = "btnImg1DetectSun";
            this.btnImg1DetectSun.Size = new System.Drawing.Size(45, 41);
            this.btnImg1DetectSun.TabIndex = 20;
            this.btnImg1DetectSun.UseVisualStyleBackColor = true;
            this.btnImg1DetectSun.Click += new System.EventHandler(this.btnImg1DetectSun_Click);
            // 
            // btnImg2DetectSun
            // 
            this.btnImg2DetectSun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImg2DetectSun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImg2DetectSun.Image = global::CloudCamsDataAnalysis.Properties.Resources._171_sun;
            this.btnImg2DetectSun.Location = new System.Drawing.Point(688, 620);
            this.btnImg2DetectSun.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImg2DetectSun.Name = "btnImg2DetectSun";
            this.btnImg2DetectSun.Size = new System.Drawing.Size(45, 41);
            this.btnImg2DetectSun.TabIndex = 21;
            this.btnImg2DetectSun.UseVisualStyleBackColor = true;
            this.btnImg2DetectSun.Click += new System.EventHandler(this.btnImg2DetectSun_Click);
            // 
            // tbSunLocationID1
            // 
            this.tbSunLocationID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbSunLocationID1, 6);
            this.tbSunLocationID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSunLocationID1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSunLocationID1.Location = new System.Drawing.Point(57, 620);
            this.tbSunLocationID1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSunLocationID1.Multiline = true;
            this.tbSunLocationID1.Name = "tbSunLocationID1";
            this.tbSunLocationID1.Size = new System.Drawing.Size(610, 41);
            this.tbSunLocationID1.TabIndex = 22;
            this.tbSunLocationID1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbSunLocationID1_MouseClick);
            // 
            // tbSunLocationID2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbSunLocationID2, 6);
            this.tbSunLocationID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSunLocationID2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSunLocationID2.Location = new System.Drawing.Point(741, 620);
            this.tbSunLocationID2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSunLocationID2.Multiline = true;
            this.tbSunLocationID2.Name = "tbSunLocationID2";
            this.tbSunLocationID2.Size = new System.Drawing.Size(610, 41);
            this.tbSunLocationID2.TabIndex = 23;
            this.tbSunLocationID2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbSunLocationID2_MouseClick);
            // 
            // btnOpenFileID1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnOpenFileID1, 2);
            this.btnOpenFileID1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenFileID1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFileID1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOpenFileID1.Location = new System.Drawing.Point(57, 4);
            this.btnOpenFileID1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenFileID1.Name = "btnOpenFileID1";
            this.btnOpenFileID1.Size = new System.Drawing.Size(98, 41);
            this.btnOpenFileID1.TabIndex = 24;
            this.btnOpenFileID1.Text = "file";
            this.btnOpenFileID1.UseVisualStyleBackColor = true;
            this.btnOpenFileID1.Click += new System.EventHandler(this.btnOpenFileID1_Click);
            // 
            // btnOpenFileID2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnOpenFileID2, 2);
            this.btnOpenFileID2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenFileID2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFileID2.Location = new System.Drawing.Point(688, 4);
            this.btnOpenFileID2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenFileID2.Name = "btnOpenFileID2";
            this.btnOpenFileID2.Size = new System.Drawing.Size(98, 41);
            this.btnOpenFileID2.TabIndex = 25;
            this.btnOpenFileID2.Text = "file";
            this.btnOpenFileID2.UseVisualStyleBackColor = true;
            this.btnOpenFileID2.Click += new System.EventHandler(this.btnOpenFileID1_Click);
            // 
            // lblControlPoint1id1Title
            // 
            this.lblControlPoint1id1Title.AutoSize = true;
            this.lblControlPoint1id1Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblControlPoint1id1Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblControlPoint1id1Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblControlPoint1id1Title.Location = new System.Drawing.Point(4, 665);
            this.lblControlPoint1id1Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblControlPoint1id1Title.Name = "lblControlPoint1id1Title";
            this.lblControlPoint1id1Title.Size = new System.Drawing.Size(45, 49);
            this.lblControlPoint1id1Title.TabIndex = 26;
            this.lblControlPoint1id1Title.Text = "1";
            this.lblControlPoint1id1Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblControlPoint1id1Title.Click += new System.EventHandler(this.lblControlPoint1id1Title_Click);
            // 
            // lblControlPoint2id1Title
            // 
            this.lblControlPoint2id1Title.AutoSize = true;
            this.lblControlPoint2id1Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblControlPoint2id1Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblControlPoint2id1Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblControlPoint2id1Title.Location = new System.Drawing.Point(4, 714);
            this.lblControlPoint2id1Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblControlPoint2id1Title.Name = "lblControlPoint2id1Title";
            this.lblControlPoint2id1Title.Size = new System.Drawing.Size(45, 49);
            this.lblControlPoint2id1Title.TabIndex = 27;
            this.lblControlPoint2id1Title.Text = "2";
            this.lblControlPoint2id1Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblControlPoint2id1Title.Click += new System.EventHandler(this.lblControlPoint2id1Title_Click);
            // 
            // lblControlPoint1id2Title
            // 
            this.lblControlPoint1id2Title.AutoSize = true;
            this.lblControlPoint1id2Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblControlPoint1id2Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblControlPoint1id2Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblControlPoint1id2Title.Location = new System.Drawing.Point(688, 665);
            this.lblControlPoint1id2Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblControlPoint1id2Title.Name = "lblControlPoint1id2Title";
            this.lblControlPoint1id2Title.Size = new System.Drawing.Size(45, 49);
            this.lblControlPoint1id2Title.TabIndex = 30;
            this.lblControlPoint1id2Title.Text = "1";
            this.lblControlPoint1id2Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblControlPoint1id2Title.Click += new System.EventHandler(this.lblControlPoint1id2Title_Click);
            // 
            // lblControlPoint2id2Title
            // 
            this.lblControlPoint2id2Title.AutoSize = true;
            this.lblControlPoint2id2Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblControlPoint2id2Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblControlPoint2id2Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblControlPoint2id2Title.Location = new System.Drawing.Point(688, 714);
            this.lblControlPoint2id2Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblControlPoint2id2Title.Name = "lblControlPoint2id2Title";
            this.lblControlPoint2id2Title.Size = new System.Drawing.Size(45, 49);
            this.lblControlPoint2id2Title.TabIndex = 31;
            this.lblControlPoint2id2Title.Text = "2";
            this.lblControlPoint2id2Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblControlPoint2id2Title.Click += new System.EventHandler(this.lblControlPoint2id2Title_Click);
            // 
            // tbControlPoint1id1
            // 
            this.tbControlPoint1id1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbControlPoint1id1, 6);
            this.tbControlPoint1id1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbControlPoint1id1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbControlPoint1id1.Location = new System.Drawing.Point(57, 669);
            this.tbControlPoint1id1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbControlPoint1id1.Multiline = true;
            this.tbControlPoint1id1.Name = "tbControlPoint1id1";
            this.tbControlPoint1id1.Size = new System.Drawing.Size(610, 41);
            this.tbControlPoint1id1.TabIndex = 34;
            this.tbControlPoint1id1.Click += new System.EventHandler(this.lblControlPoint1id1Title_Click);
            // 
            // tbControlPoint2id1
            // 
            this.tbControlPoint2id1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbControlPoint2id1, 6);
            this.tbControlPoint2id1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbControlPoint2id1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbControlPoint2id1.Location = new System.Drawing.Point(57, 718);
            this.tbControlPoint2id1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbControlPoint2id1.Multiline = true;
            this.tbControlPoint2id1.Name = "tbControlPoint2id1";
            this.tbControlPoint2id1.Size = new System.Drawing.Size(610, 41);
            this.tbControlPoint2id1.TabIndex = 35;
            this.tbControlPoint2id1.Click += new System.EventHandler(this.lblControlPoint2id1Title_Click);
            // 
            // tbControlPoint1id2
            // 
            this.tbControlPoint1id2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbControlPoint1id2, 6);
            this.tbControlPoint1id2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbControlPoint1id2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbControlPoint1id2.Location = new System.Drawing.Point(741, 669);
            this.tbControlPoint1id2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbControlPoint1id2.Multiline = true;
            this.tbControlPoint1id2.Name = "tbControlPoint1id2";
            this.tbControlPoint1id2.Size = new System.Drawing.Size(610, 41);
            this.tbControlPoint1id2.TabIndex = 38;
            this.tbControlPoint1id2.Click += new System.EventHandler(this.lblControlPoint1id2Title_Click);
            // 
            // tbControlPoint2id2
            // 
            this.tbControlPoint2id2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.tbControlPoint2id2, 6);
            this.tbControlPoint2id2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbControlPoint2id2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbControlPoint2id2.Location = new System.Drawing.Point(741, 718);
            this.tbControlPoint2id2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbControlPoint2id2.Multiline = true;
            this.tbControlPoint2id2.Name = "tbControlPoint2id2";
            this.tbControlPoint2id2.Size = new System.Drawing.Size(610, 41);
            this.tbControlPoint2id2.TabIndex = 39;
            this.tbControlPoint2id2.Click += new System.EventHandler(this.lblControlPoint2id2Title_Click);
            // 
            // btnStoreStats
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnStoreStats, 15);
            this.btnStoreStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStoreStats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStoreStats.Location = new System.Drawing.Point(4, 767);
            this.btnStoreStats.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStoreStats.Name = "btnStoreStats";
            this.btnStoreStats.Size = new System.Drawing.Size(1347, 54);
            this.btnStoreStats.TabIndex = 42;
            this.btnStoreStats.Text = "SAVE STATS";
            this.btnStoreStats.UseVisualStyleBackColor = true;
            this.btnStoreStats.Click += new System.EventHandler(this.btnStoreStats_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 5);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 567);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 49);
            this.label1.TabIndex = 43;
            this.label1.Text = "Process images for sun location in directory:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbSunLocationProcessingDirectory
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbSunLocationProcessingDirectory, 7);
            this.tbSunLocationProcessingDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSunLocationProcessingDirectory.Location = new System.Drawing.Point(269, 571);
            this.tbSunLocationProcessingDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSunLocationProcessingDirectory.Multiline = true;
            this.tbSunLocationProcessingDirectory.Name = "tbSunLocationProcessingDirectory";
            this.tbSunLocationProcessingDirectory.Size = new System.Drawing.Size(623, 41);
            this.tbSunLocationProcessingDirectory.TabIndex = 44;
            // 
            // btnBrowseDirectoryForSunLocationProcessing
            // 
            this.btnBrowseDirectoryForSunLocationProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseDirectoryForSunLocationProcessing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseDirectoryForSunLocationProcessing.Location = new System.Drawing.Point(900, 571);
            this.btnBrowseDirectoryForSunLocationProcessing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseDirectoryForSunLocationProcessing.Name = "btnBrowseDirectoryForSunLocationProcessing";
            this.btnBrowseDirectoryForSunLocationProcessing.Size = new System.Drawing.Size(345, 41);
            this.btnBrowseDirectoryForSunLocationProcessing.TabIndex = 45;
            this.btnBrowseDirectoryForSunLocationProcessing.Text = "Browse...";
            this.btnBrowseDirectoryForSunLocationProcessing.UseVisualStyleBackColor = true;
            this.btnBrowseDirectoryForSunLocationProcessing.Click += new System.EventHandler(this.btnBrowseDirectoryForSunLocationProcessing_Click);
            // 
            // btnSunLocationProcessing
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnSunLocationProcessing, 2);
            this.btnSunLocationProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSunLocationProcessing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSunLocationProcessing.Location = new System.Drawing.Point(1253, 571);
            this.btnSunLocationProcessing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSunLocationProcessing.Name = "btnSunLocationProcessing";
            this.btnSunLocationProcessing.Size = new System.Drawing.Size(98, 41);
            this.btnSunLocationProcessing.TabIndex = 46;
            this.btnSunLocationProcessing.Text = "GO";
            this.btnSunLocationProcessing.UseVisualStyleBackColor = true;
            this.btnSunLocationProcessing.Click += new System.EventHandler(this.btnSunLocationProcessing_Click);
            // 
            // trbImgID1ScaleValue
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.trbImgID1ScaleValue, 2);
            this.trbImgID1ScaleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbImgID1ScaleValue.Location = new System.Drawing.Point(269, 4);
            this.trbImgID1ScaleValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trbImgID1ScaleValue.Maximum = 5;
            this.trbImgID1ScaleValue.Name = "trbImgID1ScaleValue";
            this.trbImgID1ScaleValue.Size = new System.Drawing.Size(398, 41);
            this.trbImgID1ScaleValue.TabIndex = 47;
            this.trbImgID1ScaleValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgID1modifyScale);
            // 
            // trbImgID2ScaleValue
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.trbImgID2ScaleValue, 2);
            this.trbImgID2ScaleValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbImgID2ScaleValue.Location = new System.Drawing.Point(900, 4);
            this.trbImgID2ScaleValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trbImgID2ScaleValue.Maximum = 5;
            this.trbImgID2ScaleValue.Name = "trbImgID2ScaleValue";
            this.trbImgID2ScaleValue.Size = new System.Drawing.Size(398, 41);
            this.trbImgID2ScaleValue.TabIndex = 48;
            this.trbImgID2ScaleValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgID2modifyScale);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(163, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 49);
            this.label2.TabIndex = 49;
            this.label2.Text = "scale:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(794, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 49);
            this.label3.TabIndex = 50;
            this.label3.Text = "scale:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnImgID1Center
            // 
            this.btnImgID1Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImgID1Center.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImgID1Center.Image = global::CloudCamsDataAnalysis.Properties.Resources.target;
            this.btnImgID1Center.Location = new System.Drawing.Point(4, 53);
            this.btnImgID1Center.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImgID1Center.Name = "btnImgID1Center";
            this.btnImgID1Center.Size = new System.Drawing.Size(45, 41);
            this.btnImgID1Center.TabIndex = 51;
            this.btnImgID1Center.UseVisualStyleBackColor = true;
            this.btnImgID1Center.Click += new System.EventHandler(this.btnImgID1Center_Click);
            // 
            // btnImgID2Center
            // 
            this.btnImgID2Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImgID2Center.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImgID2Center.Image = global::CloudCamsDataAnalysis.Properties.Resources.target;
            this.btnImgID2Center.Location = new System.Drawing.Point(1306, 53);
            this.btnImgID2Center.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImgID2Center.Name = "btnImgID2Center";
            this.btnImgID2Center.Size = new System.Drawing.Size(45, 41);
            this.btnImgID2Center.TabIndex = 52;
            this.btnImgID2Center.UseVisualStyleBackColor = true;
            this.btnImgID2Center.Click += new System.EventHandler(this.btnImgID2Center_Click);
            // 
            // lblImageID1Comments
            // 
            this.lblImageID1Comments.AutoSize = true;
            this.lblImageID1Comments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblImageID1Comments, 6);
            this.lblImageID1Comments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblImageID1Comments.Location = new System.Drawing.Point(57, 469);
            this.lblImageID1Comments.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageID1Comments.Name = "lblImageID1Comments";
            this.lblImageID1Comments.Size = new System.Drawing.Size(610, 98);
            this.lblImageID1Comments.TabIndex = 53;
            this.lblImageID1Comments.Text = "---";
            this.lblImageID1Comments.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblImageID2Comments
            // 
            this.lblImageID2Comments.AutoSize = true;
            this.lblImageID2Comments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblImageID2Comments, 6);
            this.lblImageID2Comments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblImageID2Comments.Location = new System.Drawing.Point(688, 469);
            this.lblImageID2Comments.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageID2Comments.Name = "lblImageID2Comments";
            this.lblImageID2Comments.Size = new System.Drawing.Size(610, 98);
            this.lblImageID2Comments.TabIndex = 54;
            this.lblImageID2Comments.Text = "---";
            this.lblImageID2Comments.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lcCalculatingImageID1RoundData
            // 
            this.lcCalculatingImageID1RoundData.Active = false;
            this.lcCalculatingImageID1RoundData.Color = System.Drawing.Color.DarkGray;
            this.lcCalculatingImageID1RoundData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcCalculatingImageID1RoundData.InnerCircleRadius = 8;
            this.lcCalculatingImageID1RoundData.Location = new System.Drawing.Point(4, 473);
            this.lcCalculatingImageID1RoundData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lcCalculatingImageID1RoundData.Name = "lcCalculatingImageID1RoundData";
            this.lcCalculatingImageID1RoundData.NumberSpoke = 24;
            this.lcCalculatingImageID1RoundData.OuterCircleRadius = 9;
            this.lcCalculatingImageID1RoundData.RotationSpeed = 100;
            this.lcCalculatingImageID1RoundData.Size = new System.Drawing.Size(45, 90);
            this.lcCalculatingImageID1RoundData.SpokeThickness = 4;
            this.lcCalculatingImageID1RoundData.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.lcCalculatingImageID1RoundData.TabIndex = 55;
            this.lcCalculatingImageID1RoundData.Text = "loadingCircle1";
            this.lcCalculatingImageID1RoundData.Visible = false;
            // 
            // lcCalculatingImageID2RoundData
            // 
            this.lcCalculatingImageID2RoundData.Active = false;
            this.lcCalculatingImageID2RoundData.Color = System.Drawing.Color.DarkGray;
            this.lcCalculatingImageID2RoundData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcCalculatingImageID2RoundData.InnerCircleRadius = 8;
            this.lcCalculatingImageID2RoundData.Location = new System.Drawing.Point(1306, 473);
            this.lcCalculatingImageID2RoundData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lcCalculatingImageID2RoundData.Name = "lcCalculatingImageID2RoundData";
            this.lcCalculatingImageID2RoundData.NumberSpoke = 24;
            this.lcCalculatingImageID2RoundData.OuterCircleRadius = 9;
            this.lcCalculatingImageID2RoundData.RotationSpeed = 100;
            this.lcCalculatingImageID2RoundData.Size = new System.Drawing.Size(45, 90);
            this.lcCalculatingImageID2RoundData.SpokeThickness = 4;
            this.lcCalculatingImageID2RoundData.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.lcCalculatingImageID2RoundData.TabIndex = 56;
            this.lcCalculatingImageID2RoundData.Text = "loadingCircle2";
            this.lcCalculatingImageID2RoundData.Visible = false;
            // 
            // tabPageObjectsLocationAnalysis
            // 
            this.tabPageObjectsLocationAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPageObjectsLocationAnalysis.Location = new System.Drawing.Point(4, 29);
            this.tabPageObjectsLocationAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageObjectsLocationAnalysis.Name = "tabPageObjectsLocationAnalysis";
            this.tabPageObjectsLocationAnalysis.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageObjectsLocationAnalysis.Size = new System.Drawing.Size(1363, 833);
            this.tabPageObjectsLocationAnalysis.TabIndex = 1;
            this.tabPageObjectsLocationAnalysis.Text = "Objects Location Analysis";
            this.tabPageObjectsLocationAnalysis.UseVisualStyleBackColor = true;
            // 
            // tabPageProperties
            // 
            this.tabPageProperties.Location = new System.Drawing.Point(4, 29);
            this.tabPageProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.Size = new System.Drawing.Size(1363, 833);
            this.tabPageProperties.TabIndex = 2;
            this.tabPageProperties.Text = "Properties";
            this.tabPageProperties.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 866);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.tabControl1.ResumeLayout(false);
            this.tabPageCamerasLocationAnalysis.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageID1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageID2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbImgID1ScaleValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbImgID2ScaleValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageCamerasLocationAnalysis;
        private System.Windows.Forms.TabPage tabPageObjectsLocationAnalysis;
        private System.Windows.Forms.TabPage tabPageProperties;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pbImageID1;
        private System.Windows.Forms.PictureBox pbImageID2;
        private System.Windows.Forms.HScrollBar hScrollBarID1;
        private System.Windows.Forms.VScrollBar vScrollBarID1;
        private System.Windows.Forms.VScrollBar vScrollBarID2;
        private System.Windows.Forms.HScrollBar hScrollBarID2;
        private System.Windows.Forms.Button btnImg1DetectSun;
        private System.Windows.Forms.Button btnImg2DetectSun;
        private System.Windows.Forms.TextBox tbSunLocationID1;
        private System.Windows.Forms.TextBox tbSunLocationID2;
        private System.Windows.Forms.Button btnOpenFileID1;
        private System.Windows.Forms.Button btnOpenFileID2;
        private System.Windows.Forms.Label lblControlPoint1id1Title;
        private System.Windows.Forms.Label lblControlPoint2id1Title;
        private System.Windows.Forms.Label lblControlPoint1id2Title;
        private System.Windows.Forms.Label lblControlPoint2id2Title;
        private System.Windows.Forms.TextBox tbControlPoint1id1;
        private System.Windows.Forms.TextBox tbControlPoint2id1;
        private System.Windows.Forms.TextBox tbControlPoint1id2;
        private System.Windows.Forms.TextBox tbControlPoint2id2;
        private System.Windows.Forms.Button btnStoreStats;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSunLocationProcessingDirectory;
        private System.Windows.Forms.Button btnBrowseDirectoryForSunLocationProcessing;
        private System.Windows.Forms.Button btnSunLocationProcessing;
        private System.Windows.Forms.TrackBar trbImgID1ScaleValue;
        private System.Windows.Forms.TrackBar trbImgID2ScaleValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnImgID1Center;
        private System.Windows.Forms.Button btnImgID2Center;
        private System.Windows.Forms.Label lblImageID1Comments;
        private System.Windows.Forms.Label lblImageID2Comments;
        private MRG.Controls.UI.LoadingCircle lcCalculatingImageID1RoundData;
        private MRG.Controls.UI.LoadingCircle lcCalculatingImageID2RoundData;
    }
}

