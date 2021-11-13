using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class Layout : Form
    {
        private void InitializeComponent()
        {
#if false
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
            this.rightPlot.Size = new Size(275, 515);
            this.rightPlot.TabIndex = 2;
            // 
            // lowerPlot
            // 
            this.lowerPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lowerPlot.BackColor = System.Drawing.Color.Transparent;
            this.lowerPlot.Location = new System.Drawing.Point(12, 533);
            this.lowerPlot.Name = "lowerPlot";
            this.lowerPlot.Size = new Size(791, 197);
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
            this.mainPlot.Size = new Size(791, 515);
            this.mainPlot.TabIndex = 0;
            this.mainPlot.AxesChanged += new System.EventHandler(this.mainPlot_AxesChanged);
            // 
            // Layout
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(1096, 742);
            this.Controls.Add(this.rightPlot);
            this.Controls.Add(this.lowerPlot);
            this.Controls.Add(this.mainPlot);
            this.Name = "Layout";
            this.Text = "ScottPlot - Advanced Layout";
            this.SizeChanged += new System.EventHandler(this.Layout_SizeChanged);
            this.ResumeLayout(false);
#endif
        }

        private ScottPlot.Eto.PlotView mainPlot;
        private ScottPlot.Eto.PlotView lowerPlot;
        private ScottPlot.Eto.PlotView rightPlot;
    }
}
