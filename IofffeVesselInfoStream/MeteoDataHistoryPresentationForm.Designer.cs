namespace IofffeVesselInfoStream
{
    partial class MeteoDataHistoryPresentationForm
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
            this.lblStatusString = new System.Windows.Forms.Label();
            this.btnGraphMoveFocusLeft = new System.Windows.Forms.Button();
            this.btnGraphZoomOut = new System.Windows.Forms.Button();
            this.btnGraphDefault = new System.Windows.Forms.Button();
            this.btnGraphZoomIn = new System.Windows.Forms.Button();
            this.btnGraphMoveFocusRight = new System.Windows.Forms.Button();
            this.lblPressureGraphTitle = new System.Windows.Forms.Label();
            this.btnToggleShowGraphs = new System.Windows.Forms.Button();
            this.wcUpdatimgGraph = new MRG.Controls.UI.LoadingCircle();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtnPressureGraph = new System.Windows.Forms.RadioButton();
            this.rbtnAirTempGraph = new System.Windows.Forms.RadioButton();
            this.rbtnWindSpeedGraph = new System.Windows.Forms.RadioButton();
            this.rbtnWaterTempGraph = new System.Windows.Forms.RadioButton();
            this.pbGraphs = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraphs)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.Controls.Add(this.lblStatusString, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnGraphMoveFocusLeft, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnGraphZoomOut, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnGraphDefault, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnGraphZoomIn, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnGraphMoveFocusRight, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblPressureGraphTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnToggleShowGraphs, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.wcUpdatimgGraph, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pbGraphs, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(732, 504);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblStatusString
            // 
            this.lblStatusString.AutoSize = true;
            this.lblStatusString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatusString, 5);
            this.lblStatusString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusString.Location = new System.Drawing.Point(3, 462);
            this.lblStatusString.Name = "lblStatusString";
            this.lblStatusString.Size = new System.Drawing.Size(726, 42);
            this.lblStatusString.TabIndex = 0;
            this.lblStatusString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnGraphMoveFocusLeft
            // 
            this.btnGraphMoveFocusLeft.BackColor = System.Drawing.Color.Gainsboro;
            this.btnGraphMoveFocusLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphMoveFocusLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGraphMoveFocusLeft.Location = new System.Drawing.Point(3, 425);
            this.btnGraphMoveFocusLeft.Name = "btnGraphMoveFocusLeft";
            this.btnGraphMoveFocusLeft.Size = new System.Drawing.Size(34, 34);
            this.btnGraphMoveFocusLeft.TabIndex = 1;
            this.btnGraphMoveFocusLeft.Text = "<<";
            this.btnGraphMoveFocusLeft.UseVisualStyleBackColor = false;
            this.btnGraphMoveFocusLeft.Click += new System.EventHandler(this.btnGraphMoveFocusLeft_Click);
            // 
            // btnGraphZoomOut
            // 
            this.btnGraphZoomOut.BackColor = System.Drawing.Color.Gainsboro;
            this.btnGraphZoomOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGraphZoomOut.Location = new System.Drawing.Point(43, 425);
            this.btnGraphZoomOut.Name = "btnGraphZoomOut";
            this.btnGraphZoomOut.Size = new System.Drawing.Size(211, 34);
            this.btnGraphZoomOut.TabIndex = 2;
            this.btnGraphZoomOut.Text = "zoom out";
            this.btnGraphZoomOut.UseVisualStyleBackColor = false;
            this.btnGraphZoomOut.Click += new System.EventHandler(this.btnGraphZoomOut_Click);
            // 
            // btnGraphDefault
            // 
            this.btnGraphDefault.BackColor = System.Drawing.Color.Gainsboro;
            this.btnGraphDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGraphDefault.Location = new System.Drawing.Point(260, 425);
            this.btnGraphDefault.Name = "btnGraphDefault";
            this.btnGraphDefault.Size = new System.Drawing.Size(211, 34);
            this.btnGraphDefault.TabIndex = 3;
            this.btnGraphDefault.Text = "default";
            this.btnGraphDefault.UseVisualStyleBackColor = false;
            this.btnGraphDefault.Click += new System.EventHandler(this.btnGraphDefault_Click);
            // 
            // btnGraphZoomIn
            // 
            this.btnGraphZoomIn.BackColor = System.Drawing.Color.Gainsboro;
            this.btnGraphZoomIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGraphZoomIn.Location = new System.Drawing.Point(477, 425);
            this.btnGraphZoomIn.Name = "btnGraphZoomIn";
            this.btnGraphZoomIn.Size = new System.Drawing.Size(211, 34);
            this.btnGraphZoomIn.TabIndex = 4;
            this.btnGraphZoomIn.Text = "zoom in";
            this.btnGraphZoomIn.UseVisualStyleBackColor = false;
            this.btnGraphZoomIn.Click += new System.EventHandler(this.btnGraphZoomIn_Click);
            // 
            // btnGraphMoveFocusRight
            // 
            this.btnGraphMoveFocusRight.BackColor = System.Drawing.Color.Gainsboro;
            this.btnGraphMoveFocusRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGraphMoveFocusRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGraphMoveFocusRight.Location = new System.Drawing.Point(694, 425);
            this.btnGraphMoveFocusRight.Name = "btnGraphMoveFocusRight";
            this.btnGraphMoveFocusRight.Size = new System.Drawing.Size(35, 34);
            this.btnGraphMoveFocusRight.TabIndex = 5;
            this.btnGraphMoveFocusRight.Text = ">>";
            this.btnGraphMoveFocusRight.UseVisualStyleBackColor = false;
            this.btnGraphMoveFocusRight.Click += new System.EventHandler(this.btnGraphMoveFocusRight_Click);
            // 
            // lblPressureGraphTitle
            // 
            this.lblPressureGraphTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblPressureGraphTitle, 4);
            this.lblPressureGraphTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPressureGraphTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPressureGraphTitle.Location = new System.Drawing.Point(3, 0);
            this.lblPressureGraphTitle.Name = "lblPressureGraphTitle";
            this.lblPressureGraphTitle.Size = new System.Drawing.Size(685, 40);
            this.lblPressureGraphTitle.TabIndex = 6;
            this.lblPressureGraphTitle.Text = "Pressure, Temp., W.Speed, W.temp.";
            this.lblPressureGraphTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnToggleShowGraphs
            // 
            this.btnToggleShowGraphs.BackColor = System.Drawing.Color.Gainsboro;
            this.btnToggleShowGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToggleShowGraphs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleShowGraphs.Location = new System.Drawing.Point(694, 3);
            this.btnToggleShowGraphs.Name = "btnToggleShowGraphs";
            this.btnToggleShowGraphs.Size = new System.Drawing.Size(35, 34);
            this.btnToggleShowGraphs.TabIndex = 7;
            this.btnToggleShowGraphs.Text = "upd";
            this.btnToggleShowGraphs.UseVisualStyleBackColor = false;
            this.btnToggleShowGraphs.Click += new System.EventHandler(this.btnToggleShowGraphs_Click);
            // 
            // wcUpdatimgGraph
            // 
            this.wcUpdatimgGraph.Active = false;
            this.wcUpdatimgGraph.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.wcUpdatimgGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wcUpdatimgGraph.InnerCircleRadius = 8;
            this.wcUpdatimgGraph.Location = new System.Drawing.Point(693, 42);
            this.wcUpdatimgGraph.Margin = new System.Windows.Forms.Padding(2);
            this.wcUpdatimgGraph.Name = "wcUpdatimgGraph";
            this.wcUpdatimgGraph.NumberSpoke = 24;
            this.wcUpdatimgGraph.OuterCircleRadius = 9;
            this.wcUpdatimgGraph.RotationSpeed = 100;
            this.wcUpdatimgGraph.Size = new System.Drawing.Size(37, 36);
            this.wcUpdatimgGraph.SpokeThickness = 4;
            this.wcUpdatimgGraph.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.wcUpdatimgGraph.TabIndex = 55;
            this.wcUpdatimgGraph.Text = "loadingCircle1";
            this.wcUpdatimgGraph.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 4);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.rbtnPressureGraph, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtnAirTempGraph, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtnWindSpeedGraph, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtnWaterTempGraph, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(685, 34);
            this.tableLayoutPanel2.TabIndex = 56;
            // 
            // rbtnPressureGraph
            // 
            this.rbtnPressureGraph.AutoSize = true;
            this.rbtnPressureGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnPressureGraph.Location = new System.Drawing.Point(3, 3);
            this.rbtnPressureGraph.Name = "rbtnPressureGraph";
            this.rbtnPressureGraph.Size = new System.Drawing.Size(165, 28);
            this.rbtnPressureGraph.TabIndex = 0;
            this.rbtnPressureGraph.TabStop = true;
            this.rbtnPressureGraph.Text = "Pressure";
            this.rbtnPressureGraph.UseVisualStyleBackColor = true;
            this.rbtnPressureGraph.CheckedChanged += new System.EventHandler(this.rbtnGraphVarChanged);
            // 
            // rbtnAirTempGraph
            // 
            this.rbtnAirTempGraph.AutoSize = true;
            this.rbtnAirTempGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnAirTempGraph.Location = new System.Drawing.Point(174, 3);
            this.rbtnAirTempGraph.Name = "rbtnAirTempGraph";
            this.rbtnAirTempGraph.Size = new System.Drawing.Size(165, 28);
            this.rbtnAirTempGraph.TabIndex = 1;
            this.rbtnAirTempGraph.TabStop = true;
            this.rbtnAirTempGraph.Text = "Air temp.";
            this.rbtnAirTempGraph.UseVisualStyleBackColor = true;
            this.rbtnAirTempGraph.CheckedChanged += new System.EventHandler(this.rbtnGraphVarChanged);
            // 
            // rbtnWindSpeedGraph
            // 
            this.rbtnWindSpeedGraph.AutoSize = true;
            this.rbtnWindSpeedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnWindSpeedGraph.Location = new System.Drawing.Point(345, 3);
            this.rbtnWindSpeedGraph.Name = "rbtnWindSpeedGraph";
            this.rbtnWindSpeedGraph.Size = new System.Drawing.Size(165, 28);
            this.rbtnWindSpeedGraph.TabIndex = 2;
            this.rbtnWindSpeedGraph.TabStop = true;
            this.rbtnWindSpeedGraph.Text = "Wind speed";
            this.rbtnWindSpeedGraph.UseVisualStyleBackColor = true;
            this.rbtnWindSpeedGraph.CheckedChanged += new System.EventHandler(this.rbtnGraphVarChanged);
            // 
            // rbtnWaterTempGraph
            // 
            this.rbtnWaterTempGraph.AutoSize = true;
            this.rbtnWaterTempGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnWaterTempGraph.Location = new System.Drawing.Point(516, 3);
            this.rbtnWaterTempGraph.Name = "rbtnWaterTempGraph";
            this.rbtnWaterTempGraph.Size = new System.Drawing.Size(166, 28);
            this.rbtnWaterTempGraph.TabIndex = 3;
            this.rbtnWaterTempGraph.TabStop = true;
            this.rbtnWaterTempGraph.Text = "Water temp.";
            this.rbtnWaterTempGraph.UseVisualStyleBackColor = true;
            this.rbtnWaterTempGraph.CheckedChanged += new System.EventHandler(this.rbtnGraphVarChanged);
            // 
            // pbGraphs
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbGraphs, 5);
            this.pbGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraphs.Location = new System.Drawing.Point(3, 83);
            this.pbGraphs.Name = "pbGraphs";
            this.tableLayoutPanel1.SetRowSpan(this.pbGraphs, 3);
            this.pbGraphs.Size = new System.Drawing.Size(726, 336);
            this.pbGraphs.TabIndex = 57;
            this.pbGraphs.TabStop = false;
            this.pbGraphs.Click += new System.EventHandler(this.pbGraphs_Click);
            // 
            // MeteoDataHistoryPresentationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 504);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MeteoDataHistoryPresentationForm";
            this.Text = "Meteo data presentation form";
            this.Load += new System.EventHandler(this.MeteoDataHistoryPresentationForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraphs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblStatusString;
        private System.Windows.Forms.Button btnGraphMoveFocusLeft;
        private System.Windows.Forms.Button btnGraphZoomOut;
        private System.Windows.Forms.Button btnGraphDefault;
        private System.Windows.Forms.Button btnGraphZoomIn;
        private System.Windows.Forms.Button btnGraphMoveFocusRight;
        private System.Windows.Forms.Label lblPressureGraphTitle;
        private System.Windows.Forms.Button btnToggleShowGraphs;
        private MRG.Controls.UI.LoadingCircle wcUpdatimgGraph;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtnPressureGraph;
        private System.Windows.Forms.RadioButton rbtnAirTempGraph;
        private System.Windows.Forms.RadioButton rbtnWindSpeedGraph;
        private System.Windows.Forms.RadioButton rbtnWaterTempGraph;
        private System.Windows.Forms.PictureBox pbGraphs;
    }
}