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
            this.nudPoints = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cbCount = new System.Windows.Forms.RadioButton();
            this.cbCumulative = new System.Windows.Forms.RadioButton();
            this.cbNorm = new System.Windows.Forms.RadioButton();
            this.scottPlotUC2 = new ScottPlot.ScottPlotUC();
            this.scottPlotUC1 = new ScottPlot.ScottPlotUC();
            ((System.ComponentModel.ISupportInitialize)(this.nudPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // nudPoints
            // 
            this.nudPoints.Location = new System.Drawing.Point(12, 25);
            this.nudPoints.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudPoints.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudPoints.Name = "nudPoints";
            this.nudPoints.Size = new System.Drawing.Size(86, 20);
            this.nudPoints.TabIndex = 1;
            this.nudPoints.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "points:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // cbCount
            // 
            this.cbCount.AutoSize = true;
            this.cbCount.Checked = true;
            this.cbCount.Location = new System.Drawing.Point(12, 51);
            this.cbCount.Name = "cbCount";
            this.cbCount.Size = new System.Drawing.Size(52, 17);
            this.cbCount.TabIndex = 10;
            this.cbCount.TabStop = true;
            this.cbCount.Text = "count";
            this.cbCount.UseVisualStyleBackColor = true;
            this.cbCount.CheckedChanged += new System.EventHandler(this.CbCount_CheckedChanged);
            // 
            // cbCumulative
            // 
            this.cbCumulative.AutoSize = true;
            this.cbCumulative.Location = new System.Drawing.Point(12, 97);
            this.cbCumulative.Name = "cbCumulative";
            this.cbCumulative.Size = new System.Drawing.Size(76, 17);
            this.cbCumulative.TabIndex = 11;
            this.cbCumulative.Text = "cumulative";
            this.cbCumulative.UseVisualStyleBackColor = true;
            this.cbCumulative.CheckedChanged += new System.EventHandler(this.CbCumulative_CheckedChanged);
            // 
            // cbNorm
            // 
            this.cbNorm.AutoSize = true;
            this.cbNorm.Location = new System.Drawing.Point(12, 74);
            this.cbNorm.Name = "cbNorm";
            this.cbNorm.Size = new System.Drawing.Size(75, 17);
            this.cbNorm.TabIndex = 13;
            this.cbNorm.Text = "normalized";
            this.cbNorm.UseVisualStyleBackColor = true;
            this.cbNorm.CheckedChanged += new System.EventHandler(this.CbNorm_CheckedChanged);
            // 
            // scottPlotUC2
            // 
            this.scottPlotUC2.Location = new System.Drawing.Point(519, 9);
            this.scottPlotUC2.Name = "scottPlotUC2";
            this.scottPlotUC2.Size = new System.Drawing.Size(398, 329);
            this.scottPlotUC2.TabIndex = 8;
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Location = new System.Drawing.Point(104, 9);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(409, 329);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 343);
            this.Controls.Add(this.cbNorm);
            this.Controls.Add(this.cbCumulative);
            this.Controls.Add(this.cbCount);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.scottPlotUC2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudPoints);
            this.Controls.Add(this.scottPlotUC1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "ScottPlot Histogram Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPoints)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.NumericUpDown nudPoints;
        private System.Windows.Forms.Label label1;
        private ScottPlot.ScottPlotUC scottPlotUC2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton cbCount;
        private System.Windows.Forms.RadioButton cbCumulative;
        private System.Windows.Forms.RadioButton cbNorm;
    }
}

