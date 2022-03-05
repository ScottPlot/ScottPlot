using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class ToggleVisibility : Form
    {
        private void InitializeComponent()
        {
            this.cbSin = new CheckBox();
            this.cbCos = new CheckBox();
            this.cbLines = new CheckBox();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(cbSin, cbCos, cbLines, null);
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;

            // 
            // cbSin
            // 
            this.cbSin.Checked = true;
            this.cbSin.Text = "Sin";
            // 
            // cbCos
            // 
            this.cbCos.Checked = true;
            this.cbCos.Text = "Cos";
            // 
            // cbLines
            // 
            this.cbLines.Checked = true;
            this.cbLines.Text = "lines";
            // 
            // ToggleVisibility
            // 
            this.ClientSize = new Size(473, 302);
            this.Title = "ToggleVisibility";
            this.ResumeLayout();
        }

        private CheckBox cbSin;
        private CheckBox cbCos;
        private CheckBox cbLines;
        private ScottPlot.Eto.PlotView formsPlot1;
    }
}
