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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventsDensityAnalysisForm));
            this.ilPanel1 = new ILNumerics.Drawing.ILPanel();
            this.SuspendLayout();
            // 
            // ilPanel1
            // 
            this.ilPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilPanel1.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.ilPanel1.Editor = null;
            this.ilPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ilPanel1.Location = new System.Drawing.Point(0, 0);
            this.ilPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ilPanel1.Name = "ilPanel1";
            this.ilPanel1.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("ilPanel1.Rectangle")));
            this.ilPanel1.ShowUIControls = false;
            this.ilPanel1.Size = new System.Drawing.Size(988, 727);
            this.ilPanel1.TabIndex = 0;
            this.ilPanel1.Timeout = ((uint)(0u));
            // 
            // EventsDensityAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 727);
            this.Controls.Add(this.ilPanel1);
            this.KeyPreview = true;
            this.Name = "EventsDensityAnalysisForm";
            this.Text = "EventsDensityAnalysisForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EventsDensityAnalysisForm_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private ILNumerics.Drawing.ILPanel ilPanel1;
    }
}