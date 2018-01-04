namespace ScottPlotABF
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
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnPathScan = new System.Windows.Forms.Button();
            this.scottPlotUC1 = new ScottPlot.ScottPlotUC();
            this.btnPathBrowse = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.nudSweepLength = new System.Windows.Forms.NumericUpDown();
            this.nudSweep = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cbShowAllSweeps = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudScale = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbABFs = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSweepLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSweep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(11, 14);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(205, 22);
            this.tbPath.TabIndex = 1;
            this.tbPath.Text = "C:\\abfs";
            this.tbPath.TextChanged += new System.EventHandler(this.tbPath_TextChanged);
            // 
            // btnPathScan
            // 
            this.btnPathScan.Location = new System.Drawing.Point(11, 44);
            this.btnPathScan.Name = "btnPathScan";
            this.btnPathScan.Size = new System.Drawing.Size(91, 31);
            this.btnPathScan.TabIndex = 2;
            this.btnPathScan.Text = "re-scan";
            this.btnPathScan.UseVisualStyleBackColor = true;
            this.btnPathScan.Click += new System.EventHandler(this.btnPathScan_Click);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.scottPlotUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scottPlotUC1.Location = new System.Drawing.Point(228, 0);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(1113, 950);
            this.scottPlotUC1.TabIndex = 4;
            this.scottPlotUC1.Load += new System.EventHandler(this.scottPlotUC1_Load);
            // 
            // btnPathBrowse
            // 
            this.btnPathBrowse.Location = new System.Drawing.Point(108, 44);
            this.btnPathBrowse.Name = "btnPathBrowse";
            this.btnPathBrowse.Size = new System.Drawing.Size(108, 31);
            this.btnPathBrowse.TabIndex = 23;
            this.btnPathBrowse.Text = "set folder";
            this.btnPathBrowse.UseVisualStyleBackColor = true;
            this.btnPathBrowse.Click += new System.EventHandler(this.btnPathBrowse_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tbPath);
            this.panel1.Controls.Add(this.btnPathScan);
            this.panel1.Controls.Add(this.btnPathBrowse);
            this.panel1.Controls.Add(this.lbABFs);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 950);
            this.panel1.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "duration (s):";
            // 
            // nudSweepLength
            // 
            this.nudSweepLength.DecimalPlaces = 2;
            this.nudSweepLength.Location = new System.Drawing.Point(98, 23);
            this.nudSweepLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSweepLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudSweepLength.Name = "nudSweepLength";
            this.nudSweepLength.Size = new System.Drawing.Size(82, 22);
            this.nudSweepLength.TabIndex = 13;
            this.nudSweepLength.Value = new decimal(new int[] {
            200,
            0,
            0,
            131072});
            this.nudSweepLength.ValueChanged += new System.EventHandler(this.nudSweepLength_ValueChanged);
            // 
            // nudSweep
            // 
            this.nudSweep.Location = new System.Drawing.Point(98, 53);
            this.nudSweep.Name = "nudSweep";
            this.nudSweep.Size = new System.Drawing.Size(82, 22);
            this.nudSweep.TabIndex = 16;
            this.nudSweep.ValueChanged += new System.EventHandler(this.nudSweep_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "selected (n):";
            // 
            // cbShowAllSweeps
            // 
            this.cbShowAllSweeps.AutoSize = true;
            this.cbShowAllSweeps.Location = new System.Drawing.Point(10, 109);
            this.cbShowAllSweeps.Name = "cbShowAllSweeps";
            this.cbShowAllSweeps.Size = new System.Drawing.Size(131, 21);
            this.cbShowAllSweeps.TabIndex = 25;
            this.cbShowAllSweeps.Text = "show all sweeps";
            this.cbShowAllSweeps.UseVisualStyleBackColor = true;
            this.cbShowAllSweeps.CheckedChanged += new System.EventHandler(this.cbShowAllSweeps_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 26;
            this.label1.Text = "scale (mult):";
            // 
            // nudScale
            // 
            this.nudScale.DecimalPlaces = 3;
            this.nudScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.nudScale.Location = new System.Drawing.Point(98, 83);
            this.nudScale.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.nudScale.Name = "nudScale";
            this.nudScale.Size = new System.Drawing.Size(82, 22);
            this.nudScale.TabIndex = 27;
            this.nudScale.Value = new decimal(new int[] {
            40,
            0,
            0,
            196608});
            this.nudScale.ValueChanged += new System.EventHandler(this.nudScale_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudScale);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbShowAllSweeps);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.nudSweep);
            this.groupBox1.Controls.Add(this.nudSweepLength);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(11, 314);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 138);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sweep";
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProtocol.Location = new System.Drawing.Point(3, 18);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(64, 17);
            this.lblProtocol.TabIndex = 0;
            this.lblProtocol.Text = "unknown";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblProtocol);
            this.groupBox2.Location = new System.Drawing.Point(11, 267);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(205, 41);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Protocol";
            // 
            // lbABFs
            // 
            this.lbABFs.FormattingEnabled = true;
            this.lbABFs.ItemHeight = 16;
            this.lbABFs.Location = new System.Drawing.Point(11, 81);
            this.lbABFs.Name = "lbABFs";
            this.lbABFs.Size = new System.Drawing.Size(205, 180);
            this.lbABFs.TabIndex = 3;
            this.lbABFs.SelectedIndexChanged += new System.EventHandler(this.lbABFs_SelectedIndexChanged);
            this.lbABFs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lbABFs_KeyPress);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 950);
            this.Controls.Add(this.scottPlotUC1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "ScottPlot - ABF Browser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSweepLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSweep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScale)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnPathScan;
        private ScottPlot.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.Button btnPathBrowse;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudSweepLength;
        private System.Windows.Forms.NumericUpDown nudSweep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbShowAllSweeps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudScale;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbABFs;
    }
}

