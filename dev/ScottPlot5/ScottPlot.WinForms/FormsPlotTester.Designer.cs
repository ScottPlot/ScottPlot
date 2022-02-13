namespace ScottPlot.WinForms
{
    partial class FormsPlotTester
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnScatterBasic = new System.Windows.Forms.Button();
            this.btnScatter100k = new System.Windows.Forms.Button();
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            this.cbBenchmark = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnScatterBasic
            // 
            this.btnScatterBasic.Location = new System.Drawing.Point(12, 12);
            this.btnScatterBasic.Name = "btnScatterBasic";
            this.btnScatterBasic.Size = new System.Drawing.Size(79, 48);
            this.btnScatterBasic.TabIndex = 1;
            this.btnScatterBasic.Text = "Scatter with 51 points";
            this.btnScatterBasic.UseVisualStyleBackColor = true;
            this.btnScatterBasic.Click += new System.EventHandler(this.btnScatterBasic_Click);
            // 
            // btnScatter100k
            // 
            this.btnScatter100k.Location = new System.Drawing.Point(97, 12);
            this.btnScatter100k.Name = "btnScatter100k";
            this.btnScatter100k.Size = new System.Drawing.Size(79, 48);
            this.btnScatter100k.TabIndex = 2;
            this.btnScatter100k.Text = "Scatter with 10k points";
            this.btnScatter100k.UseVisualStyleBackColor = true;
            this.btnScatter100k.Click += new System.EventHandler(this.btnScatter100k_Click);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 66);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(896, 425);
            this.formsPlot1.TabIndex = 3;
            // 
            // cbBenchmark
            // 
            this.cbBenchmark.AutoSize = true;
            this.cbBenchmark.Checked = true;
            this.cbBenchmark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBenchmark.Location = new System.Drawing.Point(182, 28);
            this.cbBenchmark.Name = "cbBenchmark";
            this.cbBenchmark.Size = new System.Drawing.Size(86, 19);
            this.cbBenchmark.TabIndex = 4;
            this.cbBenchmark.Text = "Benchmark";
            this.cbBenchmark.UseVisualStyleBackColor = true;
            this.cbBenchmark.CheckedChanged += new System.EventHandler(this.cbBenchmark_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(274, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 48);
            this.button1.TabIndex = 5;
            this.button1.Text = "Tighten Layout";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnTightenLayout_Click);
            // 
            // FormsPlotTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 503);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbBenchmark);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.btnScatterBasic);
            this.Controls.Add(this.btnScatter100k);
            this.Name = "FormsPlotTester";
            this.Text = "ScottPlot 5 - WinForms";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnScatterBasic;
        private Button btnScatter100k;
        private FormsPlot formsPlot1;
        private CheckBox cbBenchmark;
        private Button button1;
    }
}