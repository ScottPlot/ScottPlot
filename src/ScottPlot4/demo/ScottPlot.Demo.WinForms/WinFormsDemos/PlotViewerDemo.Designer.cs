namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class PlotViewerDemo
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudWalkPoints = new System.Windows.Forms.NumericUpDown();
            this.btnLaunchRandomWalk = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudSineCount = new System.Windows.Forms.NumericUpDown();
            this.btnLaunchRandomSine = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWalkPoints)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSineCount)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(338, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "The PlotViewer lets you launch a plot in an interactive pop-up window.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(409, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "You can focus on generating interesting data, and the PlotViewer will handle the " +
    "GUI!";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudWalkPoints);
            this.groupBox1.Controls.Add(this.btnLaunchRandomWalk);
            this.groupBox1.Location = new System.Drawing.Point(15, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 65);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Random Walk Generator";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Points:";
            // 
            // nudWalkPoints
            // 
            this.nudWalkPoints.Location = new System.Drawing.Point(9, 35);
            this.nudWalkPoints.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudWalkPoints.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWalkPoints.Name = "nudWalkPoints";
            this.nudWalkPoints.Size = new System.Drawing.Size(63, 20);
            this.nudWalkPoints.TabIndex = 1;
            this.nudWalkPoints.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // btnLaunchRandomWalk
            // 
            this.btnLaunchRandomWalk.Location = new System.Drawing.Point(78, 19);
            this.btnLaunchRandomWalk.Name = "btnLaunchRandomWalk";
            this.btnLaunchRandomWalk.Size = new System.Drawing.Size(69, 36);
            this.btnLaunchRandomWalk.TabIndex = 0;
            this.btnLaunchRandomWalk.Text = "Launch";
            this.btnLaunchRandomWalk.UseVisualStyleBackColor = true;
            this.btnLaunchRandomWalk.Click += new System.EventHandler(this.BtnLaunchRandomWalk_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.nudSineCount);
            this.groupBox2.Controls.Add(this.btnLaunchRandomSine);
            this.groupBox2.Location = new System.Drawing.Point(194, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(156, 65);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Random Sine Generator";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Waves:";
            // 
            // nudSineCount
            // 
            this.nudSineCount.Location = new System.Drawing.Point(9, 35);
            this.nudSineCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSineCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSineCount.Name = "nudSineCount";
            this.nudSineCount.Size = new System.Drawing.Size(63, 20);
            this.nudSineCount.TabIndex = 1;
            this.nudSineCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnLaunchRandomSine
            // 
            this.btnLaunchRandomSine.Location = new System.Drawing.Point(78, 19);
            this.btnLaunchRandomSine.Name = "btnLaunchRandomSine";
            this.btnLaunchRandomSine.Size = new System.Drawing.Size(69, 36);
            this.btnLaunchRandomSine.TabIndex = 0;
            this.btnLaunchRandomSine.Text = "Launch";
            this.btnLaunchRandomSine.UseVisualStyleBackColor = true;
            this.btnLaunchRandomSine.Click += new System.EventHandler(this.BtnLaunchRandomSine_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(15, 149);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(406, 134);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sample Code";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(3, 16);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(400, 115);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "// create a ScottPlot\r\nvar plt = new ScottPlot.Plot();\r\nplt.PlotSignal(dataArray)" +
    ";\r\n\r\n// launch it in a PlotViewer\r\nnew ScottPlot.FormsPlotViewer(plt).Show();";
            // 
            // PlotViewerDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 299);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PlotViewerDemo";
            this.Text = "Plot Viewer Demo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWalkPoints)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSineCount)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudWalkPoints;
        private System.Windows.Forms.Button btnLaunchRandomWalk;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudSineCount;
        private System.Windows.Forms.Button btnLaunchRandomSine;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
    }
}