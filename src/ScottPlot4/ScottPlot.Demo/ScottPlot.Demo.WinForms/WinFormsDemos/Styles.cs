using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class Styles : Form
    {
        public Styles()
        {
            InitializeComponent();
            lbStyles.Items.AddRange(Style.GetStyles());
            lbPalettes.Items.AddRange(Palette.GetPalettes());

            formsPlot1.Plot.XLabel("Horizontal Axis");
            formsPlot1.Plot.YLabel("Vertical Axis");

            lbStyles.SelectedIndex = 0;
            lbPalettes.SelectedIndex = 0;
        }

        private void UpdatePlot()
        {
            var style = (ScottPlot.Styles.IStyle)lbStyles.SelectedItem;
            var palette = (ScottPlot.IPalette)lbPalettes.SelectedItem;

            if (style is null || palette is null)
                return;

            formsPlot1.Plot.Style(style);
            formsPlot1.Plot.Title($"Style: {style}\nPalette: {palette}");
            formsPlot1.Plot.Palette = palette;

            formsPlot1.Plot.Clear();
            for (int i = 0; i < palette.Count(); i++)
            {
                double offset = 1 + i * 1.1;
                double mult = 10 + i;
                double phase = i * .3 / palette.Count();
                double[] ys = DataGen.Sin(51, 1, offset, mult, phase);
                var sig = formsPlot1.Plot.AddSignal(ys);
                sig.LineWidth = 3;
                sig.MarkerSize = 0;
            }

            formsPlot1.Plot.AxisAuto(horizontalMargin: 0);
            formsPlot1.Refresh();
        }

        private void lbStyles_SelectedIndexChanged(object sender, EventArgs e) => UpdatePlot();

        private void lbPalettes_SelectedIndexChanged(object sender, EventArgs e) => UpdatePlot();
    }
}
