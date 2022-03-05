using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class ShowValueOnHover2 : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.label1 = new Label();
            this.SuspendLayout();

            this.Content = new DynamicLayout(label1, formsPlot1) { Padding = 5 };

            // 
            // formsPlot1
            // 
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            // 
            // label1
            // 
            this.label1.Font = Fonts.Sans(12);
            this.label1.Text = "Message";
            // 
            // ShowValueOnHover2
            // 
            this.ClientSize = new Size(707, 404);
            this.Title = "Show Value Nearest Cursor";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private Label label1;
    }
}
