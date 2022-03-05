using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class AxisLimits : Form
    {
        private void InitializeComponent()
        {
            this.Size = new Size(800, 450);
            this.Content = this.formsPlot1 = new ScottPlot.Eto.PlotView();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
    }
}
