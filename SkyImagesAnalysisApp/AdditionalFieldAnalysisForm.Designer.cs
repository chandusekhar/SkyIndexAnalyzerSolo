namespace SkyImagesAnalyzer
{
    partial class AdditionalFieldAnalysisForm
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
            this.pbCurrResult = new System.Windows.Forms.PictureBox();
            this.btnGetFuncAlongSection = new System.Windows.Forms.Button();
            this.cbMarkSectionFromMarginToMargin = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrResult)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 11;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.Controls.Add(this.pbCurrResult, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnGetFuncAlongSection, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbMarkSectionFromMarginToMargin, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(863, 710);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pbCurrResult
            // 
            this.pbCurrResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pbCurrResult, 11);
            this.pbCurrResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbCurrResult.Location = new System.Drawing.Point(3, 53);
            this.pbCurrResult.Name = "pbCurrResult";
            this.tableLayoutPanel1.SetRowSpan(this.pbCurrResult, 2);
            this.pbCurrResult.Size = new System.Drawing.Size(857, 604);
            this.pbCurrResult.TabIndex = 2;
            this.pbCurrResult.TabStop = false;
            this.pbCurrResult.Click += new System.EventHandler(this.pbCurrResult_Click);
            // 
            // btnGetFuncAlongSection
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnGetFuncAlongSection, 4);
            this.btnGetFuncAlongSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetFuncAlongSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetFuncAlongSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetFuncAlongSection.Location = new System.Drawing.Point(3, 663);
            this.btnGetFuncAlongSection.Name = "btnGetFuncAlongSection";
            this.btnGetFuncAlongSection.Size = new System.Drawing.Size(306, 44);
            this.btnGetFuncAlongSection.TabIndex = 3;
            this.btnGetFuncAlongSection.Text = "get data along section...";
            this.btnGetFuncAlongSection.UseVisualStyleBackColor = true;
            this.btnGetFuncAlongSection.Click += new System.EventHandler(this.btnGetFuncAlongSection_Click);
            // 
            // cbMarkSectionFromMarginToMargin
            // 
            this.cbMarkSectionFromMarginToMargin.AutoSize = true;
            this.cbMarkSectionFromMarginToMargin.Checked = true;
            this.cbMarkSectionFromMarginToMargin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.cbMarkSectionFromMarginToMargin, 6);
            this.cbMarkSectionFromMarginToMargin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMarkSectionFromMarginToMargin.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMarkSectionFromMarginToMargin.Location = new System.Drawing.Point(3, 3);
            this.cbMarkSectionFromMarginToMargin.Name = "cbMarkSectionFromMarginToMargin";
            this.cbMarkSectionFromMarginToMargin.Size = new System.Drawing.Size(462, 44);
            this.cbMarkSectionFromMarginToMargin.TabIndex = 4;
            this.cbMarkSectionFromMarginToMargin.Text = "mark sections from margin to margin";
            this.cbMarkSectionFromMarginToMargin.UseVisualStyleBackColor = true;
            // 
            // AdditionalFieldAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 710);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "AdditionalFieldAnalysisForm";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pbCurrResult;
        private System.Windows.Forms.Button btnGetFuncAlongSection;
        private System.Windows.Forms.CheckBox cbMarkSectionFromMarginToMargin;
    }
}

