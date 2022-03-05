namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class Layout
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
            this.rightPlot = new ScottPlot.FormsPlot();
            this.lowerPlot = new ScottPlot.FormsPlot();
            this.mainPlot = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // rightPlot
            // 
            this.rightPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rightPlot.BackColor = System.Drawing.Color.Transparent;
            this.rightPlot.Location = new System.Drawing.Point(809, 12);
            this.rightPlot.Name = "rightPlot";
            this.rightPlot.Size = new System.Drawing.Size(275, 515);
            this.rightPlot.TabIndex = 2;
            // 
            // lowerPlot
            // 
            this.lowerPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lowerPlot.BackColor = System.Drawing.Color.Transparent;
            this.lowerPlot.Location = new System.Drawing.Point(12, 533);
            this.lowerPlot.Name = "lowerPlot";
            this.lowerPlot.Size = new System.Drawing.Size(791, 197);
            this.lowerPlot.TabIndex = 1;
            // 
            // mainPlot
            // 
            this.mainPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPlot.BackColor = System.Drawing.Color.Transparent;
            this.mainPlot.Location = new System.Drawing.Point(12, 12);
            this.mainPlot.Name = "mainPlot";
            this.mainPlot.Size = new System.Drawing.Size(791, 515);
            this.mainPlot.TabIndex = 0;
            this.mainPlot.AxesChanged += new System.EventHandler(this.mainPlot_AxesChanged);
            // 
            // Layout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 742);
            this.Controls.Add(this.rightPlot);
            this.Controls.Add(this.lowerPlot);
            this.Controls.Add(this.mainPlot);
            this.Name = "Layout";
            this.Text = "ScottPlot - Advanced Layout";
            this.SizeChanged += new System.EventHandler(this.Layout_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private FormsPlot mainPlot;
        private FormsPlot lowerPlot;
        private FormsPlot rightPlot;
    }
}