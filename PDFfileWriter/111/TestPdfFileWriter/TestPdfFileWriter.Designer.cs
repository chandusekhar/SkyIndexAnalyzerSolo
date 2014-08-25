namespace TestPdfFileWriter {
    partial class TestPdfFileWriter {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
        components.Dispose();
        }
        base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.ArticleButton = new System.Windows.Forms.Button();
			this.GraphicsButton = new System.Windows.Forms.Button();
			this.DebugCheckBox = new System.Windows.Forms.CheckBox();
			this.FontFamiliesButton = new System.Windows.Forms.Button();
			this.CopyrightTextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// ArticleButton
			// 
			this.ArticleButton.Location = new System.Drawing.Point(33, 330);
			this.ArticleButton.Name = "ArticleButton";
			this.ArticleButton.Size = new System.Drawing.Size(128, 44);
			this.ArticleButton.TabIndex = 0;
			this.ArticleButton.Text = "Article Example";
			this.ArticleButton.UseVisualStyleBackColor = true;
			this.ArticleButton.Click += new System.EventHandler(this.OnArticleExample);
			// 
			// GraphicsButton
			// 
			this.GraphicsButton.Location = new System.Drawing.Point(181, 330);
			this.GraphicsButton.Name = "GraphicsButton";
			this.GraphicsButton.Size = new System.Drawing.Size(128, 44);
			this.GraphicsButton.TabIndex = 1;
			this.GraphicsButton.Text = "Other Example";
			this.GraphicsButton.UseVisualStyleBackColor = true;
			this.GraphicsButton.Click += new System.EventHandler(this.OnOtherExample);
			// 
			// DebugCheckBox
			// 
			this.DebugCheckBox.AutoSize = true;
			this.DebugCheckBox.Location = new System.Drawing.Point(497, 343);
			this.DebugCheckBox.Name = "DebugCheckBox";
			this.DebugCheckBox.Size = new System.Drawing.Size(64, 20);
			this.DebugCheckBox.TabIndex = 3;
			this.DebugCheckBox.Text = "Debug";
			this.DebugCheckBox.UseVisualStyleBackColor = true;
			// 
			// FontFamiliesButton
			// 
			this.FontFamiliesButton.Location = new System.Drawing.Point(329, 330);
			this.FontFamiliesButton.Name = "FontFamiliesButton";
			this.FontFamiliesButton.Size = new System.Drawing.Size(128, 44);
			this.FontFamiliesButton.TabIndex = 2;
			this.FontFamiliesButton.Text = "Font Families";
			this.FontFamiliesButton.UseVisualStyleBackColor = true;
			this.FontFamiliesButton.Click += new System.EventHandler(this.OnFontFamilies);
			// 
			// CopyrightTextBox
			// 
			this.CopyrightTextBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.CopyrightTextBox.Location = new System.Drawing.Point(33, 26);
			this.CopyrightTextBox.MaxLength = 2048;
			this.CopyrightTextBox.Name = "CopyrightTextBox";
			this.CopyrightTextBox.ReadOnly = true;
			this.CopyrightTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.CopyrightTextBox.Size = new System.Drawing.Size(548, 268);
			this.CopyrightTextBox.TabIndex = 4;
			this.CopyrightTextBox.Text = "";
			// 
			// TestPdfFileWriter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(614, 408);
			this.Controls.Add(this.CopyrightTextBox);
			this.Controls.Add(this.FontFamiliesButton);
			this.Controls.Add(this.DebugCheckBox);
			this.Controls.Add(this.GraphicsButton);
			this.Controls.Add(this.ArticleButton);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "TestPdfFileWriter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Test PDF File Writer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ArticleButton;
		private System.Windows.Forms.Button GraphicsButton;
		private System.Windows.Forms.CheckBox DebugCheckBox;
		private System.Windows.Forms.Button FontFamiliesButton;
		private System.Windows.Forms.RichTextBox CopyrightTextBox;
    }
}

