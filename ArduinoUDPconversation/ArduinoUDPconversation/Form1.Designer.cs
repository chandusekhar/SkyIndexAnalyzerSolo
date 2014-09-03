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
            this.btnStartStopBdcstListening = new System.Windows.Forms.Button();
            this.tbBcstListeningLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFindArduino1 = new System.Windows.Forms.Button();
            this.textBoxCommand1 = new System.Windows.Forms.TextBox();
            this.tbDev1IPstr = new System.Windows.Forms.TextBox();
            this.tbResponseLog1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClearRsponceLog1 = new System.Windows.Forms.Button();
            this.btnSwapResponseLog1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbResponseLog2 = new System.Windows.Forms.TextBox();
            this.btnClearRsponceLog2 = new System.Windows.Forms.Button();
            this.btnSwapResponseLog2 = new System.Windows.Forms.Button();
            this.textBoxCommand2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDev2IPstr = new System.Windows.Forms.TextBox();
            this.btnFindArduino2 = new System.Windows.Forms.Button();
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
            this.btnClearBcstLog.Location = new System.Drawing.Point(1229, 53);
            this.btnClearBcstLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearBcstLog.Name = "btnClearBcstLog";
            this.btnClearBcstLog.Size = new System.Drawing.Size(167, 41);
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
            this.btnSwapBcstLog.Location = new System.Drawing.Point(1054, 53);
            this.btnSwapBcstLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnSwapBcstLog.Name = "btnSwapBcstLog";
            this.btnSwapBcstLog.Size = new System.Drawing.Size(167, 41);
            this.btnSwapBcstLog.TabIndex = 41;
            this.btnSwapBcstLog.Text = "Swap log";
            this.btnSwapBcstLog.UseVisualStyleBackColor = true;
            this.btnSwapBcstLog.Click += new System.EventHandler(this.btnSwapBcstLog_Click);
            // 
            // btnStartStopBdcstListening
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnStartStopBdcstListening, 4);
            this.btnStartStopBdcstListening.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartStopBdcstListening.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopBdcstListening.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStartStopBdcstListening.Location = new System.Drawing.Point(704, 813);
            this.btnStartStopBdcstListening.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartStopBdcstListening.Name = "btnStartStopBdcstListening";
            this.btnStartStopBdcstListening.Size = new System.Drawing.Size(692, 41);
            this.btnStartStopBdcstListening.TabIndex = 38;
            this.btnStartStopBdcstListening.Text = "Start listening";
            this.btnStartStopBdcstListening.UseVisualStyleBackColor = true;
            this.btnStartStopBdcstListening.Click += new System.EventHandler(this.btnStartStopBdcstListening_Click);
            // 
            // tbBcstListeningLog
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbBcstListeningLog, 4);
            this.tbBcstListeningLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBcstListeningLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBcstListeningLog.Location = new System.Drawing.Point(704, 102);
            this.tbBcstListeningLog.Margin = new System.Windows.Forms.Padding(4);
            this.tbBcstListeningLog.Multiline = true;
            this.tbBcstListeningLog.Name = "tbBcstListeningLog";
            this.tableLayoutPanel1.SetRowSpan(this.tbBcstListeningLog, 12);
            this.tbBcstListeningLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbBcstListeningLog.Size = new System.Drawing.Size(692, 703);
            this.tbBcstListeningLog.TabIndex = 36;
            this.tbBcstListeningLog.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(692, 49);
            this.label1.TabIndex = 35;
            this.label1.Text = "Conversation response";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFindArduino1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnFindArduino1, 2);
            this.btnFindArduino1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindArduino1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindArduino1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindArduino1.Location = new System.Drawing.Point(354, 427);
            this.btnFindArduino1.Margin = new System.Windows.Forms.Padding(4);
            this.btnFindArduino1.Name = "btnFindArduino1";
            this.btnFindArduino1.Size = new System.Drawing.Size(342, 41);
            this.btnFindArduino1.TabIndex = 33;
            this.btnFindArduino1.Text = "search for board (ID=1)";
            this.btnFindArduino1.UseVisualStyleBackColor = true;
            this.btnFindArduino1.Click += new System.EventHandler(this.btnFindArduino_Click);
            // 
            // textBoxCommand1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxCommand1, 4);
            this.textBoxCommand1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCommand1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxCommand1.Location = new System.Drawing.Point(4, 378);
            this.textBoxCommand1.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCommand1.Multiline = true;
            this.textBoxCommand1.Name = "textBoxCommand1";
            this.textBoxCommand1.Size = new System.Drawing.Size(692, 41);
            this.textBoxCommand1.TabIndex = 32;
            this.textBoxCommand1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCommand_KeyPress);
            // 
            // tbDev1IPstr
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbDev1IPstr, 2);
            this.tbDev1IPstr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDev1IPstr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDev1IPstr.Location = new System.Drawing.Point(4, 427);
            this.tbDev1IPstr.Margin = new System.Windows.Forms.Padding(4);
            this.tbDev1IPstr.Multiline = true;
            this.tbDev1IPstr.Name = "tbDev1IPstr";
            this.tbDev1IPstr.Size = new System.Drawing.Size(342, 41);
            this.tbDev1IPstr.TabIndex = 31;
            this.tbDev1IPstr.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // tbResponseLog1
            // 
            this.tbResponseLog1.AcceptsReturn = true;
            this.tableLayoutPanel1.SetColumnSpan(this.tbResponseLog1, 4);
            this.tbResponseLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbResponseLog1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbResponseLog1.HideSelection = false;
            this.tbResponseLog1.Location = new System.Drawing.Point(4, 102);
            this.tbResponseLog1.Margin = new System.Windows.Forms.Padding(4);
            this.tbResponseLog1.Multiline = true;
            this.tbResponseLog1.Name = "tbResponseLog1";
            this.tableLayoutPanel1.SetRowSpan(this.tbResponseLog1, 4);
            this.tbResponseLog1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResponseLog1.Size = new System.Drawing.Size(692, 268);
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
            this.tableLayoutPanel1.Controls.Add(this.btnSwapResponseLog1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbResponseLog1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbDev1IPstr, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.textBoxCommand1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnFindArduino1, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbBcstListeningLog, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnStartStopBdcstListening, 4, 14);
            this.tableLayoutPanel1.Controls.Add(this.btnSwapBcstLog, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnClearBcstLog, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbResponseLog2, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.btnClearRsponceLog2, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnSwapResponseLog2, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.textBoxCommand2, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbDev2IPstr, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.btnFindArduino2, 2, 14);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1400, 858);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnClearRsponceLog1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnClearRsponceLog1, 2);
            this.btnClearRsponceLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearRsponceLog1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearRsponceLog1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearRsponceLog1.Location = new System.Drawing.Point(4, 53);
            this.btnClearRsponceLog1.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearRsponceLog1.Name = "btnClearRsponceLog1";
            this.btnClearRsponceLog1.Size = new System.Drawing.Size(342, 41);
            this.btnClearRsponceLog1.TabIndex = 45;
            this.btnClearRsponceLog1.Text = "Clear";
            this.btnClearRsponceLog1.UseVisualStyleBackColor = true;
            this.btnClearRsponceLog1.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSwapResponseLog1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnSwapResponseLog1, 2);
            this.btnSwapResponseLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSwapResponseLog1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapResponseLog1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSwapResponseLog1.Location = new System.Drawing.Point(354, 53);
            this.btnSwapResponseLog1.Margin = new System.Windows.Forms.Padding(4);
            this.btnSwapResponseLog1.Name = "btnSwapResponseLog1";
            this.btnSwapResponseLog1.Size = new System.Drawing.Size(342, 41);
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
            this.label2.Location = new System.Drawing.Point(704, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(692, 49);
            this.label2.TabIndex = 43;
            this.label2.Text = "Broadcast log";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbResponseLog2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbResponseLog2, 4);
            this.tbResponseLog2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbResponseLog2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbResponseLog2.Location = new System.Drawing.Point(4, 537);
            this.tbResponseLog2.Margin = new System.Windows.Forms.Padding(4);
            this.tbResponseLog2.Multiline = true;
            this.tbResponseLog2.Name = "tbResponseLog2";
            this.tableLayoutPanel1.SetRowSpan(this.tbResponseLog2, 3);
            this.tbResponseLog2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResponseLog2.Size = new System.Drawing.Size(692, 219);
            this.tbResponseLog2.TabIndex = 46;
            // 
            // btnClearRsponceLog2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnClearRsponceLog2, 2);
            this.btnClearRsponceLog2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearRsponceLog2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearRsponceLog2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearRsponceLog2.Location = new System.Drawing.Point(4, 488);
            this.btnClearRsponceLog2.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearRsponceLog2.Name = "btnClearRsponceLog2";
            this.btnClearRsponceLog2.Size = new System.Drawing.Size(342, 41);
            this.btnClearRsponceLog2.TabIndex = 47;
            this.btnClearRsponceLog2.Text = "Clear";
            this.btnClearRsponceLog2.UseVisualStyleBackColor = true;
            this.btnClearRsponceLog2.Click += new System.EventHandler(this.btnClearRsponceLog2_Click);
            // 
            // btnSwapResponseLog2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnSwapResponseLog2, 2);
            this.btnSwapResponseLog2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSwapResponseLog2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapResponseLog2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSwapResponseLog2.Location = new System.Drawing.Point(354, 488);
            this.btnSwapResponseLog2.Margin = new System.Windows.Forms.Padding(4);
            this.btnSwapResponseLog2.Name = "btnSwapResponseLog2";
            this.btnSwapResponseLog2.Size = new System.Drawing.Size(342, 41);
            this.btnSwapResponseLog2.TabIndex = 48;
            this.btnSwapResponseLog2.Text = "Swap response";
            this.btnSwapResponseLog2.UseVisualStyleBackColor = true;
            this.btnSwapResponseLog2.Click += new System.EventHandler(this.btnSwapResponseLog2_Click);
            // 
            // textBoxCommand2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxCommand2, 4);
            this.textBoxCommand2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCommand2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxCommand2.Location = new System.Drawing.Point(4, 764);
            this.textBoxCommand2.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCommand2.Multiline = true;
            this.textBoxCommand2.Name = "textBoxCommand2";
            this.textBoxCommand2.Size = new System.Drawing.Size(692, 41);
            this.textBoxCommand2.TabIndex = 49;
            this.textBoxCommand2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCommand2_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 4);
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(4, 472);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(692, 12);
            this.label3.TabIndex = 50;
            // 
            // tbDev2IPstr
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbDev2IPstr, 2);
            this.tbDev2IPstr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDev2IPstr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDev2IPstr.Location = new System.Drawing.Point(4, 813);
            this.tbDev2IPstr.Margin = new System.Windows.Forms.Padding(4);
            this.tbDev2IPstr.Multiline = true;
            this.tbDev2IPstr.Name = "tbDev2IPstr";
            this.tbDev2IPstr.Size = new System.Drawing.Size(342, 41);
            this.tbDev2IPstr.TabIndex = 51;
            this.tbDev2IPstr.TextChanged += new System.EventHandler(this.tbDev2IPstr_TextChanged);
            // 
            // btnFindArduino2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnFindArduino2, 2);
            this.btnFindArduino2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindArduino2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindArduino2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFindArduino2.Location = new System.Drawing.Point(354, 813);
            this.btnFindArduino2.Margin = new System.Windows.Forms.Padding(4);
            this.btnFindArduino2.Name = "btnFindArduino2";
            this.btnFindArduino2.Size = new System.Drawing.Size(342, 41);
            this.btnFindArduino2.TabIndex = 52;
            this.btnFindArduino2.Text = "search for board (ID=2)";
            this.btnFindArduino2.UseVisualStyleBackColor = true;
            this.btnFindArduino2.Click += new System.EventHandler(this.btnFindArduino_Click);
            // 
            // bgwUDPmessagesParser
            // 
            this.bgwUDPmessagesParser.WorkerSupportsCancellation = true;
            this.bgwUDPmessagesParser.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwUDPmessagesParser_DoWork);
            // 
            // ArduinoConversationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 858);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.Button btnStartStopBdcstListening;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbResponseLog1;
        private System.Windows.Forms.TextBox tbDev1IPstr;
        private System.Windows.Forms.TextBox textBoxCommand1;
        private System.Windows.Forms.Button btnFindArduino1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbBcstListeningLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClearRsponceLog1;
        private System.Windows.Forms.Button btnSwapResponseLog1;
        private System.ComponentModel.BackgroundWorker bgwUDPmessagesParser;
        private System.Windows.Forms.TextBox tbResponseLog2;
        private System.Windows.Forms.Button btnClearRsponceLog2;
        private System.Windows.Forms.Button btnSwapResponseLog2;
        private System.Windows.Forms.TextBox textBoxCommand2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDev2IPstr;
        private System.Windows.Forms.Button btnFindArduino2;
    }
}

