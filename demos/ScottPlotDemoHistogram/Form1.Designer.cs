namespace ScottPlotDemoHistogram
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
            this.nudPointCount = new System.Windows.Forms.NumericUpDown();
            this.cbCount = new System.Windows.Forms.RadioButton();
            this.cbCph = new System.Windows.Forms.RadioButton();
            this.cbNorm = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGenerateData = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudMin = new System.Windows.Forms.NumericUpDown();
            this.cbMinAuto = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.cbMaxAuto = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.nudBinSize = new System.Windows.Forms.NumericUpDown();
            this.cbBinSizeAuto = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.nudBinCount = new System.Windows.Forms.NumericUpDown();
            this.cbBinCountAuto = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cbIgnoreOutOfBounds = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.rbGraphBar = new System.Windows.Forms.RadioButton();
            this.rbGraphStep = new System.Windows.Forms.RadioButton();
            this.scottPlotUC2 = new ScottPlot.ScottPlotUC();
            this.scottPlotUC1 = new ScottPlot.ScottPlotUC();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.lbBins = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPointCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBinSize)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBinCount)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudPointCount
            // 
            this.nudPointCount.Location = new System.Drawing.Point(9, 19);
            this.nudPointCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudPointCount.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudPointCount.Name = "nudPointCount";
            this.nudPointCount.Size = new System.Drawing.Size(72, 20);
            this.nudPointCount.TabIndex = 1;
            this.nudPointCount.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // cbCount
            // 
            this.cbCount.AutoSize = true;
            this.cbCount.Checked = true;
            this.cbCount.Location = new System.Drawing.Point(6, 19);
            this.cbCount.Name = "cbCount";
            this.cbCount.Size = new System.Drawing.Size(52, 17);
            this.cbCount.TabIndex = 10;
            this.cbCount.TabStop = true;
            this.cbCount.Text = "count";
            this.cbCount.UseVisualStyleBackColor = true;
            this.cbCount.CheckedChanged += new System.EventHandler(this.CbCount_CheckedChanged);
            // 
            // cbCph
            // 
            this.cbCph.AutoSize = true;
            this.cbCph.Location = new System.Drawing.Point(116, 19);
            this.cbCph.Name = "cbCph";
            this.cbCph.Size = new System.Drawing.Size(43, 17);
            this.cbCph.TabIndex = 11;
            this.cbCph.Text = "cph";
            this.cbCph.UseVisualStyleBackColor = true;
            this.cbCph.CheckedChanged += new System.EventHandler(this.CbCph_CheckedChanged);
            // 
            // cbNorm
            // 
            this.cbNorm.AutoSize = true;
            this.cbNorm.Location = new System.Drawing.Point(62, 19);
            this.cbNorm.Name = "cbNorm";
            this.cbNorm.Size = new System.Drawing.Size(48, 17);
            this.cbNorm.TabIndex = 13;
            this.cbNorm.Text = "norm";
            this.cbNorm.UseVisualStyleBackColor = true;
            this.cbNorm.CheckedChanged += new System.EventHandler(this.CbNorm_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGenerateData);
            this.groupBox1.Controls.Add(this.nudPointCount);
            this.groupBox1.Location = new System.Drawing.Point(443, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 48);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data values";
            // 
            // btnGenerateData
            // 
            this.btnGenerateData.Location = new System.Drawing.Point(87, 19);
            this.btnGenerateData.Name = "btnGenerateData";
            this.btnGenerateData.Size = new System.Drawing.Size(71, 23);
            this.btnGenerateData.TabIndex = 10;
            this.btnGenerateData.Text = "generate";
            this.btnGenerateData.UseVisualStyleBackColor = true;
            this.btnGenerateData.Click += new System.EventHandler(this.BtnGenerateData_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbCount);
            this.groupBox2.Controls.Add(this.cbNorm);
            this.groupBox2.Controls.Add(this.cbCph);
            this.groupBox2.Location = new System.Drawing.Point(443, 306);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 43);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Display Metric";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudMin);
            this.groupBox3.Controls.Add(this.cbMinAuto);
            this.groupBox3.Location = new System.Drawing.Point(443, 66);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(168, 42);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Minimum";
            // 
            // nudMin
            // 
            this.nudMin.Enabled = false;
            this.nudMin.Location = new System.Drawing.Point(62, 16);
            this.nudMin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMin.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudMin.Name = "nudMin";
            this.nudMin.Size = new System.Drawing.Size(96, 20);
            this.nudMin.TabIndex = 1;
            this.nudMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // cbMinAuto
            // 
            this.cbMinAuto.AutoSize = true;
            this.cbMinAuto.Checked = true;
            this.cbMinAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMinAuto.Location = new System.Drawing.Point(9, 19);
            this.cbMinAuto.Name = "cbMinAuto";
            this.cbMinAuto.Size = new System.Drawing.Size(47, 17);
            this.cbMinAuto.TabIndex = 0;
            this.cbMinAuto.Text = "auto";
            this.cbMinAuto.UseVisualStyleBackColor = true;
            this.cbMinAuto.CheckedChanged += new System.EventHandler(this.CbMinAuto_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nudMax);
            this.groupBox4.Controls.Add(this.cbMaxAuto);
            this.groupBox4.Location = new System.Drawing.Point(443, 114);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(168, 42);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Maximum";
            // 
            // nudMax
            // 
            this.nudMax.Enabled = false;
            this.nudMax.Location = new System.Drawing.Point(62, 16);
            this.nudMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMax.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudMax.Name = "nudMax";
            this.nudMax.Size = new System.Drawing.Size(96, 20);
            this.nudMax.TabIndex = 1;
            this.nudMax.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // cbMaxAuto
            // 
            this.cbMaxAuto.AutoSize = true;
            this.cbMaxAuto.Checked = true;
            this.cbMaxAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMaxAuto.Location = new System.Drawing.Point(9, 19);
            this.cbMaxAuto.Name = "cbMaxAuto";
            this.cbMaxAuto.Size = new System.Drawing.Size(47, 17);
            this.cbMaxAuto.TabIndex = 0;
            this.cbMaxAuto.Text = "auto";
            this.cbMaxAuto.UseVisualStyleBackColor = true;
            this.cbMaxAuto.CheckedChanged += new System.EventHandler(this.CbMaxAuto_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.nudBinSize);
            this.groupBox5.Controls.Add(this.cbBinSizeAuto);
            this.groupBox5.Location = new System.Drawing.Point(443, 162);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(168, 42);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Bin size";
            // 
            // nudBinSize
            // 
            this.nudBinSize.DecimalPlaces = 2;
            this.nudBinSize.Enabled = false;
            this.nudBinSize.Location = new System.Drawing.Point(62, 16);
            this.nudBinSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBinSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudBinSize.Name = "nudBinSize";
            this.nudBinSize.Size = new System.Drawing.Size(96, 20);
            this.nudBinSize.TabIndex = 1;
            this.nudBinSize.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // cbBinSizeAuto
            // 
            this.cbBinSizeAuto.AutoSize = true;
            this.cbBinSizeAuto.Checked = true;
            this.cbBinSizeAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBinSizeAuto.Location = new System.Drawing.Point(9, 19);
            this.cbBinSizeAuto.Name = "cbBinSizeAuto";
            this.cbBinSizeAuto.Size = new System.Drawing.Size(47, 17);
            this.cbBinSizeAuto.TabIndex = 0;
            this.cbBinSizeAuto.Text = "auto";
            this.cbBinSizeAuto.UseVisualStyleBackColor = true;
            this.cbBinSizeAuto.CheckedChanged += new System.EventHandler(this.CbBinSizeAuto_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.nudBinCount);
            this.groupBox6.Controls.Add(this.cbBinCountAuto);
            this.groupBox6.Location = new System.Drawing.Point(443, 210);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(168, 42);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Bin count";
            // 
            // nudBinCount
            // 
            this.nudBinCount.Enabled = false;
            this.nudBinCount.Location = new System.Drawing.Point(62, 16);
            this.nudBinCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudBinCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBinCount.Name = "nudBinCount";
            this.nudBinCount.Size = new System.Drawing.Size(96, 20);
            this.nudBinCount.TabIndex = 1;
            this.nudBinCount.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // cbBinCountAuto
            // 
            this.cbBinCountAuto.AutoSize = true;
            this.cbBinCountAuto.Checked = true;
            this.cbBinCountAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBinCountAuto.Location = new System.Drawing.Point(9, 19);
            this.cbBinCountAuto.Name = "cbBinCountAuto";
            this.cbBinCountAuto.Size = new System.Drawing.Size(47, 17);
            this.cbBinCountAuto.TabIndex = 0;
            this.cbBinCountAuto.Text = "auto";
            this.cbBinCountAuto.UseVisualStyleBackColor = true;
            this.cbBinCountAuto.CheckedChanged += new System.EventHandler(this.CbBinCountAuto_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cbIgnoreOutOfBounds);
            this.groupBox7.Location = new System.Drawing.Point(443, 258);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(168, 42);
            this.groupBox7.TabIndex = 21;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Out of bounds";
            // 
            // cbIgnoreOutOfBounds
            // 
            this.cbIgnoreOutOfBounds.AutoSize = true;
            this.cbIgnoreOutOfBounds.Checked = true;
            this.cbIgnoreOutOfBounds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIgnoreOutOfBounds.Location = new System.Drawing.Point(9, 19);
            this.cbIgnoreOutOfBounds.Name = "cbIgnoreOutOfBounds";
            this.cbIgnoreOutOfBounds.Size = new System.Drawing.Size(55, 17);
            this.cbIgnoreOutOfBounds.TabIndex = 0;
            this.cbIgnoreOutOfBounds.Text = "ignore";
            this.cbIgnoreOutOfBounds.UseVisualStyleBackColor = true;
            this.cbIgnoreOutOfBounds.CheckedChanged += new System.EventHandler(this.CbIgnoreOutOfBounds_CheckedChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.rbGraphBar);
            this.groupBox8.Controls.Add(this.rbGraphStep);
            this.groupBox8.Location = new System.Drawing.Point(443, 355);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(168, 43);
            this.groupBox8.TabIndex = 22;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Display Style";
            // 
            // rbGraphBar
            // 
            this.rbGraphBar.AutoSize = true;
            this.rbGraphBar.Checked = true;
            this.rbGraphBar.Location = new System.Drawing.Point(6, 19);
            this.rbGraphBar.Name = "rbGraphBar";
            this.rbGraphBar.Size = new System.Drawing.Size(40, 17);
            this.rbGraphBar.TabIndex = 10;
            this.rbGraphBar.TabStop = true;
            this.rbGraphBar.Text = "bar";
            this.rbGraphBar.UseVisualStyleBackColor = true;
            this.rbGraphBar.CheckedChanged += new System.EventHandler(this.RbGraphBar_CheckedChanged);
            // 
            // rbGraphStep
            // 
            this.rbGraphStep.AutoSize = true;
            this.rbGraphStep.Location = new System.Drawing.Point(52, 19);
            this.rbGraphStep.Name = "rbGraphStep";
            this.rbGraphStep.Size = new System.Drawing.Size(45, 17);
            this.rbGraphStep.TabIndex = 13;
            this.rbGraphStep.Text = "step";
            this.rbGraphStep.UseVisualStyleBackColor = true;
            this.rbGraphStep.CheckedChanged += new System.EventHandler(this.RbGraphStep_CheckedChanged);
            // 
            // scottPlotUC2
            // 
            this.scottPlotUC2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC2.Location = new System.Drawing.Point(738, 12);
            this.scottPlotUC2.Name = "scottPlotUC2";
            this.scottPlotUC2.Size = new System.Drawing.Size(474, 388);
            this.scottPlotUC2.TabIndex = 8;
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.scottPlotUC1.Location = new System.Drawing.Point(12, 12);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(425, 388);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.lbBins);
            this.groupBox9.Location = new System.Drawing.Point(617, 12);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(115, 386);
            this.groupBox9.TabIndex = 23;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Bins";
            // 
            // lbBins
            // 
            this.lbBins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBins.FormattingEnabled = true;
            this.lbBins.Location = new System.Drawing.Point(3, 16);
            this.lbBins.Name = "lbBins";
            this.lbBins.Size = new System.Drawing.Size(109, 367);
            this.lbBins.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 408);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.scottPlotUC2);
            this.Controls.Add(this.scottPlotUC1);
            this.Name = "Form1";
            this.Text = "ScottPlot Histogram Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPointCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBinSize)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBinCount)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.NumericUpDown nudPointCount;
        private ScottPlot.ScottPlotUC scottPlotUC2;
        private System.Windows.Forms.RadioButton cbCount;
        private System.Windows.Forms.RadioButton cbCph;
        private System.Windows.Forms.RadioButton cbNorm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGenerateData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudMin;
        private System.Windows.Forms.CheckBox cbMinAuto;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.CheckBox cbMaxAuto;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown nudBinSize;
        private System.Windows.Forms.CheckBox cbBinSizeAuto;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown nudBinCount;
        private System.Windows.Forms.CheckBox cbBinCountAuto;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox cbIgnoreOutOfBounds;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton rbGraphBar;
        private System.Windows.Forms.RadioButton rbGraphStep;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.ListBox lbBins;
    }
}

