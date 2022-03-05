using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class MultiAxisLock : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.cbPrimary = new CheckBox();
            this.cbSecondary = new CheckBox();
            this.cbTertiary = new CheckBox();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(cbPrimary, cbSecondary, cbTertiary);
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;
            // 
            // formsPlot1
            // 
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            // 
            // cbPrimary
            // 
            this.cbPrimary.Checked = true;
            this.cbPrimary.Font = Fonts.Sans(12, FontStyle.Bold);
            this.cbPrimary.TextColor = Colors.Magenta;
            this.cbPrimary.Text = "Primary";
            // 
            // cbSecondary
            // 
            this.cbSecondary.Font = Fonts.Sans(12, FontStyle.Bold);
            this.cbSecondary.TextColor = Colors.Green;
            this.cbSecondary.Text = "Secondary";
            // 
            // cbTertiary
            // 
            this.cbTertiary.Font = Fonts.Sans(12, FontStyle.Bold);
            this.cbTertiary.TextColor = Colors.Navy;
            this.cbTertiary.Text = "Tertiary";
            // 
            // MultiAxis
            // 
            this.ClientSize = new Size(618, 381);
            this.Title = "MultiAxis";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private CheckBox cbPrimary;
        private CheckBox cbSecondary;
        private CheckBox cbTertiary;
    }
}
