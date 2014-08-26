namespace ArduinoUDPconversation
{
    partial class ArduinoConversationForm
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
            this.arduinoBoardSearchingWorker = new System.ComponentModel.BackgroundWorker();
            this.udpCatchingJob = new System.ComponentModel.BackgroundWorker();
            this.btnClearBcstLog = new System.Windows.Forms.Button();
            this.btnSwapBcstLog = new System.Windows.Forms.Button();
            this.tbBcstListeningPort = new System.Windows.Forms.TextBox();
            this.btnStartStopBdcstListening = new System.Windows.Forms.Button();
            this.tbBcstListeningLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFindArduino = new System.Windows.Forms.Button();
            this.textBoxCommand1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tbResponseLog1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClearRsponceLog1 = new System.Windows.Forms.Button();
            this.btnSwapResponseLog1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bgwUDPmessagesParser = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // arduinoBoardSearchingWorker
            // 
            this.arduinoBoardSearchingWorker.WorkerSupportsCancellation = true;
            this.arduinoBoardSearchingWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.arduinoBoardSearchingWorker_DoWork);
            this.arduinoBoardSearchingWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.arduinoBoardSearchingWorker_RunWorkerCompleted);
            // 
            // udpCatchingJob
            // 
            this.udpCatchingJob.WorkerSupportsCancellation = true;
            this.udpCatchingJob.DoWork += new System.ComponentModel.DoWorkEventHandler(this.udpCatchingJob_DoWork);
            this.udpCatchingJob.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.udpCatchingJob_RunWorkerCompleted);
            // 
            // btnClearBcstLog
            // 
            this.btnClearBcstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearBcstLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearBcstLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearBcstLog.Location = new System.Drawing.Point(920, 43);
            this.btnClearBcstLog.Name = "btnClearBcstLog";
            this.btnClearBcstLog.Size = new System.Drawing.Size(127, 34);
            this.btnClearBcstLog.TabIndex = 42;
            this.btnClearBcstLog.Text = "Clear";
            this.btnClearBcstLog.UseVisualStyleBackColor = true;
            this.btnClearBcstLog.Click += new System.EventHandler(this.btnClearBcstLog_Click);
            // 
            // btnSwapBcstLog
            // 
            this.btnSwapBcstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSwapBcstLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapBcstLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSwapBcstLog.Location = new System.Drawing.Point(789, 43);
            this.btnSwapBcstLog.Name = "btnSwapBcstLog";
            this.btnSwapBcstLog.Size = new System.Drawing.Size(125, 34);
            this.btnSwapBcstLog.TabIndex = 41;
            this.btnSwapBcstLog.Text = "Swap log";
            this.btnSwapBcstLog.UseVisualStyleBackColor = true;
            this.btnSwapBcstLog.Click += new System.EventHandler(this.btnSwapBcstLog_Click);
            // 
            // tbBcstListeningPort
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbBcstListeningPort, 2);
            this.tbBcstListeningPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBcstListeningPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBcstListeningPort.Location = new System.Drawing.Point(789, 478);
            this.tbBcstListeningPort.Multiline = true;
            this.tbBcstListeningPort.Name = "tbBcstListeningPort";
            this.tbBcstListeningPort.Size = new System.Drawing.Size(258, 189);
            this.tbBcstListeningPort.TabIndex = 39;
            this.tbBcstListeningPort.TextChanged += new System.EventHandler(this.tbBcstListeningPort_TextChanged);
            // 
            // btnStartStopBdcstListening
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnStartStopBdcstListening, 2);
            this.btnStartStopBdcstListening.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartStopBdcstListening.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopBdcstListening.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStartStopBdcstListening.Location = new System.Drawing.Point(527, 478);
            this.btnStartStopBdcstListening.Name = "btnStartStopBdcstListening";
            this.btnStartStopBdcstListening.Size = new System.Drawing.Size(256, 189);
            this.btnStartStopBdcstListening.TabIndex = 38;
            this.btnStartStopBdcstListening.Text = "Start listening on port:";
            this.btnStartStopBdcstListening.UseVisualStyleBackColor = true;
            this.btnStartStopBdcstListening.Click += new System.EventHandler(this.btnStartStopBdcstListening_Click);
            // 
            // tbBcstListeningLog
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbBcstListeningLog, 4);
            this.tbBcstListeningLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBcstListeningLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBcstListeningLog.Location = new System.Drawing.Point(527, 83);
            this.tbBcstListeningLog.Multiline = true;
            this.tbBcstListeningLog.Name = "tbBcstListeningLog";
            this.tableLayoutPanel1.SetRowSpan(this.tbBcstListeningLog, 4);
            this.tbBcstListeningLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbBcstListeningLog.Size = new System.Drawing.Size(520, 309);
            this.tbBcstListeningLog.TabIndex = 36;
            this.tbBcstListeningLog.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(518, 40);
            this.label1.TabIndex = 35;
            this.label1.Text = "Conversation response";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFindArduino
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnFindArduino, 2);
            this.btnFindArduino.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindArduino.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindArduino.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindArduino.Location = new System.Drawing.Point(265, 438);
            this.btnFindArduino.Name = "btnFindArduino";
            this.btnFindArduino.Size = new System.Drawing.Size(256, 34);
            this.btnFindArduino.TabIndex = 33;
            this.btnFindArduino.Text = "search for board ID1";
            this.btnFindArduino.UseVisualStyleBackColor = true;
            this.btnFindArduino.Click += new System.EventHandler(this.btnFindArduino_Click);
            // 
            // textBoxCommand1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxCommand1, 4);
            this.textBoxCommand1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCommand1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxCommand1.Location = new System.Drawing.Point(3, 398);
            this.textBoxCommand1.Name = "textBoxCommand1";
            this.textBoxCommand1.Size = new System.Drawing.Size(518, 26);
            this.textBoxCommand1.TabIndex = 32;
            this.textBoxCommand1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCommand_KeyPress);
            // 
            // textBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox2, 2);
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox2.Location = new System.Drawing.Point(3, 438);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(256, 34);
            this.textBox2.TabIndex = 31;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // tbResponseLog1
            // 
            this.tbResponseLog1.AcceptsReturn = true;
            this.tableLayoutPanel1.SetColumnSpan(this.tbResponseLog1, 4);
            this.tbResponseLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbResponseLog1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbResponseLog1.HideSelection = false;
            this.tbResponseLog1.Location = new System.Drawing.Point(3, 83);
            this.tbResponseLog1.Multiline = true;
            this.tbResponseLog1.Name = "tbResponseLog1";
            this.tableLayoutPanel1.SetRowSpan(this.tbResponseLog1, 4);
            this.tbResponseLog1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResponseLog1.Size = new System.Drawing.Size(518, 309);
            this.tbResponseLog1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.btnClearRsponceLog1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSwapResponseLog1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbResponseLog1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.textBoxCommand1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnFindArduino, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbBcstListeningLog, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnStartStopBdcstListening, 4, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbBcstListeningPort, 6, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnSwapBcstLog, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnClearBcstLog, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1050, 870);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnClearRsponceLog1
            // 
            this.btnClearRsponceLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearRsponceLog1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearRsponceLog1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearRsponceLog1.Location = new System.Drawing.Point(3, 43);
            this.btnClearRsponceLog1.Name = "btnClearRsponceLog1";
            this.btnClearRsponceLog1.Size = new System.Drawing.Size(125, 34);
            this.btnClearRsponceLog1.TabIndex = 45;
            this.btnClearRsponceLog1.Text = "Clear";
            this.btnClearRsponceLog1.UseVisualStyleBackColor = true;
            this.btnClearRsponceLog1.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSwapResponseLog1
            // 
            this.btnSwapResponseLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSwapResponseLog1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapResponseLog1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSwapResponseLog1.Location = new System.Drawing.Point(134, 43);
            this.btnSwapResponseLog1.Name = "btnSwapResponseLog1";
            this.btnSwapResponseLog1.Size = new System.Drawing.Size(125, 34);
            this.btnSwapResponseLog1.TabIndex = 44;
            this.btnSwapResponseLog1.Text = "Swap response";
            this.btnSwapResponseLog1.UseVisualStyleBackColor = true;
            this.btnSwapResponseLog1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 4);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(527, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(520, 40);
            this.label2.TabIndex = 43;
            this.label2.Text = "Broadcast log";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bgwUDPmessagesParser
            // 
            this.bgwUDPmessagesParser.WorkerSupportsCancellation = true;
            this.bgwUDPmessagesParser.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwUDPmessagesParser_DoWork);
            // 
            // ArduinoConversationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 870);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ArduinoConversationForm";
            this.Text = "Arduino conversation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.ComponentModel.BackgroundWorker GetSensorsDataCycle;
        private System.ComponentModel.BackgroundWorker arduinoBoardSearchingWorker;
        private System.ComponentModel.BackgroundWorker udpCatchingJob;
        private System.Windows.Forms.Button btnClearBcstLog;
        private System.Windows.Forms.Button btnSwapBcstLog;
        private System.Windows.Forms.TextBox tbBcstListeningPort;
        private System.Windows.Forms.Button btnStartStopBdcstListening;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbResponseLog1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBoxCommand1;
        private System.Windows.Forms.Button btnFindArduino;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbBcstListeningLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClearRsponceLog1;
        private System.Windows.Forms.Button btnSwapResponseLog1;
        private System.ComponentModel.BackgroundWorker bgwUDPmessagesParser;
    }
}

