namespace ScottPlotDemoCustomGrid
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
            this.gbSpacing = new System.Windows.Forms.GroupBox();
            this.cbAutoY = new System.Windows.Forms.CheckBox();
            this.cbAutoX = new System.Windows.Forms.CheckBox();
            this.nudY = new System.Windows.Forms.NumericUpDown();
            this.nudX = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbShowGrid = new System.Windows.Forms.CheckBox();
            this.scottPlotUC1 = new ScottPlot.ScottPlotUC();
            this.gbSpacing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSpacing
            // 
            this.gbSpacing.Controls.Add(this.cbAutoY);
            this.gbSpacing.Controls.Add(this.cbAutoX);
            this.gbSpacing.Controls.Add(this.nudY);
            this.gbSpacing.Controls.Add(this.nudX);
            this.gbSpacing.Controls.Add(this.label2);
            this.gbSpacing.Controls.Add(this.label1);
            this.gbSpacing.Location = new System.Drawing.Point(12, 35);
            this.gbSpacing.Name = "gbSpacing";
            this.gbSpacing.Size = new System.Drawing.Size(170, 75);
            this.gbSpacing.TabIndex = 2;
            this.gbSpacing.TabStop = false;
            this.gbSpacing.Text = "Grid spacing";
            // 
            // cbAutoY
            // 
            this.cbAutoY.AutoSize = true;
            this.cbAutoY.Checked = true;
            this.cbAutoY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoY.Location = new System.Drawing.Point(118, 47);
            this.cbAutoY.Name = "cbAutoY";
            this.cbAutoY.Size = new System.Drawing.Size(47, 17);
            this.cbAutoY.TabIndex = 6;
            this.cbAutoY.Text = "auto";
            this.cbAutoY.UseVisualStyleBackColor = true;
            this.cbAutoY.CheckedChanged += new System.EventHandler(this.CbAutoY_CheckedChanged);
            // 
            // cbAutoX
            // 
            this.cbAutoX.AutoSize = true;
            this.cbAutoX.Checked = true;
            this.cbAutoX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoX.Location = new System.Drawing.Point(118, 22);
            this.cbAutoX.Name = "cbAutoX";
            this.cbAutoX.Size = new System.Drawing.Size(47, 17);
            this.cbAutoX.TabIndex = 4;
            this.cbAutoX.Text = "auto";
            this.cbAutoX.UseVisualStyleBackColor = true;
            this.cbAutoX.CheckedChanged += new System.EventHandler(this.CbAutoX_CheckedChanged);
            // 
            // nudY
            // 
            this.nudY.DecimalPlaces = 1;
            this.nudY.Enabled = false;
            this.nudY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudY.Location = new System.Drawing.Point(26, 45);
            this.nudY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudY.Name = "nudY";
            this.nudY.Size = new System.Drawing.Size(86, 20);
            this.nudY.TabIndex = 5;
            this.nudY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudY.ValueChanged += new System.EventHandler(this.NudY_ValueChanged);
            // 
            // nudX
            // 
            this.nudX.DecimalPlaces = 1;
            this.nudX.Enabled = false;
            this.nudX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudX.Location = new System.Drawing.Point(26, 19);
            this.nudX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudX.Name = "nudX";
            this.nudX.Size = new System.Drawing.Size(86, 20);
            this.nudX.TabIndex = 4;
            this.nudX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudX.ValueChanged += new System.EventHandler(this.NudX_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "X";
            // 
            // cbShowGrid
            // 
            this.cbShowGrid.AutoSize = true;
            this.cbShowGrid.Checked = true;
            this.cbShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowGrid.Location = new System.Drawing.Point(12, 12);
            this.cbShowGrid.Name = "cbShowGrid";
            this.cbShowGrid.Size = new System.Drawing.Size(71, 17);
            this.cbShowGrid.TabIndex = 3;
            this.cbShowGrid.Text = "show grid";
            this.cbShowGrid.UseVisualStyleBackColor = true;
            this.cbShowGrid.CheckedChanged += new System.EventHandler(this.CbShowGrid_CheckedChanged);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC1.Location = new System.Drawing.Point(188, 12);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(657, 392);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 416);
            this.Controls.Add(this.cbShowGrid);
            this.Controls.Add(this.gbSpacing);
            this.Controls.Add(this.scottPlotUC1);
            this.Name = "Form1";
            this.Text = "ScottPlot Grid Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbSpacing.ResumeLayout(false);
            this.gbSpacing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.GroupBox gbSpacing;
        private System.Windows.Forms.CheckBox cbShowGrid;
        private System.Windows.Forms.CheckBox cbAutoY;
        private System.Windows.Forms.CheckBox cbAutoX;
        private System.Windows.Forms.NumericUpDown nudY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudX;
    }
}

