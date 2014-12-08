namespace SkyImagesAnalyzer
{
    partial class EventsDensityAnalysisForm
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
            this.SuspendLayout();
            // 
            // EventsDensityAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 700);
            this.KeyPreview = true;
            this.Name = "EventsDensityAnalysisForm";
            this.Text = "EventsDensityAnalysisForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EventsDensityAnalysisForm_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion
    }
}