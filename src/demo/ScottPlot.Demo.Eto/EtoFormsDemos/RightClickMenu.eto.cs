using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class RightClickMenu : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.SuspendLayout();

            this.Content = this.formsPlot1;

            // 
            // formsPlot1
            // 
            //this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.formsPlot1.Location = new System.Drawing.Point(0, 0);
            //this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new Size(800, 450);
            this.formsPlot1.TabIndex = 0;
            // 
            // RightClickMenu
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ClientSize = new Size(800, 450);
            //this.Controls.Add(this.formsPlot1);
            //this.Name = "RightClickMenu";
            this.Title = "RightClickMenu";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
    }
}
