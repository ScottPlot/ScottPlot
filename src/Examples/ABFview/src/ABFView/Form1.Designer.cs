namespace ABFView
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.scottPlotUC1 = new ScottPlot.ScottPlotUC();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nud_vsep = new System.Windows.Forms.NumericUpDown();
            this.nud_sweep = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_sweep = new System.Windows.Forms.RadioButton();
            this.rb_continuous = new System.Windows.Forms.RadioButton();
            this.rb_stacked = new System.Windows.Forms.RadioButton();
            this.rb_overlap = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_vsep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_sweep)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(940, 666);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.scottPlotUC1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 103);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(934, 560);
            this.panel1.TabIndex = 0;
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scottPlotUC1.Location = new System.Drawing.Point(0, 0);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(934, 560);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.nud_vsep);
            this.panel2.Controls.Add(this.nud_sweep);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(934, 94);
            this.panel2.TabIndex = 1;
            // 
            // nud_vsep
            // 
            this.nud_vsep.Enabled = false;
            this.nud_vsep.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_vsep.Location = new System.Drawing.Point(269, 57);
            this.nud_vsep.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_vsep.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nud_vsep.Name = "nud_vsep";
            this.nud_vsep.Size = new System.Drawing.Size(69, 22);
            this.nud_vsep.TabIndex = 7;
            this.nud_vsep.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nud_vsep.ValueChanged += new System.EventHandler(this.nud_vsep_ValueChanged);
            // 
            // nud_sweep
            // 
            this.nud_sweep.Enabled = false;
            this.nud_sweep.Location = new System.Drawing.Point(269, 29);
            this.nud_sweep.Name = "nud_sweep";
            this.nud_sweep.Size = new System.Drawing.Size(69, 22);
            this.nud_sweep.TabIndex = 6;
            this.nud_sweep.ValueChanged += new System.EventHandler(this.nud_sweep_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Vert Sep:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Sweep:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_sweep);
            this.groupBox1.Controls.Add(this.rb_continuous);
            this.groupBox1.Controls.Add(this.rb_stacked);
            this.groupBox1.Controls.Add(this.rb_overlap);
            this.groupBox1.Location = new System.Drawing.Point(9, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 85);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Display Mode";
            // 
            // rb_sweep
            // 
            this.rb_sweep.AutoSize = true;
            this.rb_sweep.Location = new System.Drawing.Point(110, 21);
            this.rb_sweep.Name = "rb_sweep";
            this.rb_sweep.Size = new System.Drawing.Size(69, 21);
            this.rb_sweep.TabIndex = 3;
            this.rb_sweep.TabStop = true;
            this.rb_sweep.Text = "sweep";
            this.rb_sweep.UseVisualStyleBackColor = true;
            this.rb_sweep.CheckedChanged += new System.EventHandler(this.rb_sweep_CheckedChanged);
            // 
            // rb_continuous
            // 
            this.rb_continuous.AutoSize = true;
            this.rb_continuous.Location = new System.Drawing.Point(6, 21);
            this.rb_continuous.Name = "rb_continuous";
            this.rb_continuous.Size = new System.Drawing.Size(98, 21);
            this.rb_continuous.TabIndex = 0;
            this.rb_continuous.Text = "continuous";
            this.rb_continuous.UseVisualStyleBackColor = true;
            this.rb_continuous.CheckedChanged += new System.EventHandler(this.rb_continuous_CheckedChanged);
            // 
            // rb_stacked
            // 
            this.rb_stacked.AutoSize = true;
            this.rb_stacked.Location = new System.Drawing.Point(89, 48);
            this.rb_stacked.Name = "rb_stacked";
            this.rb_stacked.Size = new System.Drawing.Size(78, 21);
            this.rb_stacked.TabIndex = 2;
            this.rb_stacked.Text = "stacked";
            this.rb_stacked.UseVisualStyleBackColor = true;
            this.rb_stacked.CheckedChanged += new System.EventHandler(this.rb_stacked_CheckedChanged);
            // 
            // rb_overlap
            // 
            this.rb_overlap.AutoSize = true;
            this.rb_overlap.Location = new System.Drawing.Point(7, 48);
            this.rb_overlap.Name = "rb_overlap";
            this.rb_overlap.Size = new System.Drawing.Size(76, 21);
            this.rb_overlap.TabIndex = 1;
            this.rb_overlap.Text = "overlap";
            this.rb_overlap.UseVisualStyleBackColor = true;
            this.rb_overlap.CheckedChanged += new System.EventHandler(this.rb_overlap_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(819, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 75);
            this.button1.TabIndex = 0;
            this.button1.Text = "select ABF";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 666);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "ABFview";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_vsep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_sweep)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private ScottPlot.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nud_vsep;
        private System.Windows.Forms.RadioButton rb_sweep;
        private System.Windows.Forms.NumericUpDown nud_sweep;
        private System.Windows.Forms.RadioButton rb_continuous;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rb_stacked;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rb_overlap;
    }
}

