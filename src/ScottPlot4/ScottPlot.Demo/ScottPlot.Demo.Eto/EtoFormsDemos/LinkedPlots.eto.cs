using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class LinkedPlots : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.formsPlot2 = new ScottPlot.Eto.PlotView();
            this.SuspendLayout();

            this.Content = new DynamicLayout(formsPlot1, formsPlot2) { Padding = 5 };

            // 
            // formsPlot1
            // 
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            this.formsPlot1.Size = new Size(-1, 201);
            // 
            // formsPlot2
            // 
            this.formsPlot2.BackgroundColor = Colors.Transparent;
            this.formsPlot2.Size = new Size(-1, 201);
            // 
            // LinkedPlots
            // 
            this.ClientSize = new Size(769, -1);
            this.Title = "LinkedPlots";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private ScottPlot.Eto.PlotView formsPlot2;
    }
}
