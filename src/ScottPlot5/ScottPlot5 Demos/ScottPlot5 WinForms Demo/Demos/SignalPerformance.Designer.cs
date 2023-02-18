namespace WinForms_Demo.Demos
{
    partial class SignalPerformance
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
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            this.rbSignal = new System.Windows.Forms.RadioButton();
            this.rbScatter = new System.Windows.Forms.RadioButton();
            this.rbScatterGL = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 37);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 401);
            this.formsPlot1.TabIndex = 0;
            // 
            // rbSignal
            // 
            this.rbSignal.AutoSize = true;
            this.rbSignal.Checked = true;
            this.rbSignal.Location = new System.Drawing.Point(12, 12);
            this.rbSignal.Name = "rbSignal";
            this.rbSignal.Size = new System.Drawing.Size(81, 19);
            this.rbSignal.TabIndex = 1;
            this.rbSignal.TabStop = true;
            this.rbSignal.Text = "Signal Plot";
            this.rbSignal.UseVisualStyleBackColor = true;
            this.rbSignal.CheckedChanged += new System.EventHandler(this.rbSignal_CheckedChanged);
            // 
            // rbScatter
            // 
            this.rbScatter.AutoSize = true;
            this.rbScatter.Location = new System.Drawing.Point(99, 12);
            this.rbScatter.Name = "rbScatter";
            this.rbScatter.Size = new System.Drawing.Size(85, 19);
            this.rbScatter.TabIndex = 2;
            this.rbScatter.Text = "Scatter Plot";
            this.rbScatter.UseVisualStyleBackColor = true;
            this.rbScatter.CheckedChanged += new System.EventHandler(this.rbScatter_CheckedChanged);
            // 
            // rbScatterGL
            // 
            this.rbScatterGL.AutoSize = true;
            this.rbScatterGL.Location = new System.Drawing.Point(190, 12);
            this.rbScatterGL.Name = "rbScatterGL";
            this.rbScatterGL.Size = new System.Drawing.Size(75, 19);
            this.rbScatterGL.TabIndex = 3;
            this.rbScatterGL.TabStop = true;
            this.rbScatterGL.Text = "ScatterGL";
            this.rbScatterGL.UseVisualStyleBackColor = true;
            // 
            // SignalPerformance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rbScatterGL);
            this.Controls.Add(this.rbScatter);
            this.Controls.Add(this.rbSignal);
            this.Controls.Add(this.formsPlot1);
            this.Name = "SignalPerformance";
            this.Text = "Signal plot with one million points";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private RadioButton rbSignal;
        private RadioButton rbScatter;
        private RadioButton rbScatterGL;
    }
}