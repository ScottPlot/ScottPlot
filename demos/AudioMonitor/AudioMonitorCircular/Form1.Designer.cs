namespace AudioMonitor
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.comboMicrophone = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnMicScan = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.pnlLevel = new System.Windows.Forms.Panel();
            this.pbLevelMask = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.scottPlotUC1 = new ScottPlotDev2.ScottPlotUC();
            this.statusStrip1.SuspendLayout();
            this.pnlLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevelMask)).BeginInit();
            this.SuspendLayout();
            // 
            // comboMicrophone
            // 
            this.comboMicrophone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMicrophone.FormattingEnabled = true;
            this.comboMicrophone.Location = new System.Drawing.Point(90, 13);
            this.comboMicrophone.Name = "comboMicrophone";
            this.comboMicrophone.Size = new System.Drawing.Size(273, 21);
            this.comboMicrophone.TabIndex = 0;
            this.comboMicrophone.SelectedIndexChanged += new System.EventHandler(this.comboMicrophone_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Audio device:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 407);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(681, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(118, 17);
            this.lblStatus.Text = "toolStripStatusLabel1";
            // 
            // btnMicScan
            // 
            this.btnMicScan.Location = new System.Drawing.Point(369, 12);
            this.btnMicScan.Name = "btnMicScan";
            this.btnMicScan.Size = new System.Drawing.Size(72, 23);
            this.btnMicScan.TabIndex = 3;
            this.btnMicScan.Text = "Re-Scan";
            this.btnMicScan.UseVisualStyleBackColor = true;
            this.btnMicScan.Click += new System.EventHandler(this.btnMicScan_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(510, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(591, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // pnlLevel
            // 
            this.pnlLevel.BackColor = System.Drawing.Color.Red;
            this.pnlLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLevel.Controls.Add(this.pbLevelMask);
            this.pnlLevel.Location = new System.Drawing.Point(15, 70);
            this.pnlLevel.Name = "pnlLevel";
            this.pnlLevel.Size = new System.Drawing.Size(30, 324);
            this.pnlLevel.TabIndex = 6;
            // 
            // pbLevelMask
            // 
            this.pbLevelMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLevelMask.BackColor = System.Drawing.Color.Silver;
            this.pbLevelMask.Location = new System.Drawing.Point(0, 0);
            this.pbLevelMask.Margin = new System.Windows.Forms.Padding(0);
            this.pbLevelMask.Name = "pbLevelMask";
            this.pbLevelMask.Size = new System.Drawing.Size(29, 119);
            this.pbLevelMask.TabIndex = 7;
            this.pbLevelMask.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Level";
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scottPlotUC1.Location = new System.Drawing.Point(57, 41);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(609, 353);
            this.scottPlotUC1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 429);
            this.Controls.Add(this.scottPlotUC1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlLevel);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnMicScan);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboMicrophone);
            this.Name = "Form1";
            this.Text = "Audio Monitor (ScottPlot Demo)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlLevel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLevelMask)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboMicrophone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnMicScan;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Panel pnlLevel;
        private System.Windows.Forms.PictureBox pbLevelMask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private ScottPlotDev2.ScottPlotUC scottPlotUC1;
    }
}

