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
            this.formsPlot1.Size = new Size(800, 450);
            // 
            // RightClickMenu
            // 
            this.Title = "RightClickMenu";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
    }
}
