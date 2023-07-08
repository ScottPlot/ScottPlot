using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class StyleBrowser : Window
    {
        public StyleBrowser()
        {
            InitializeComponent();

            ListBoxStyle.ItemsSource = ScottPlot.Style.GetStyles();
            ListBoxPalette.ItemsSource = ScottPlot.Palette.GetPalettes();

            AvaPlot1.Plot.XLabel("Horizontal Axis");
            AvaPlot1.Plot.YLabel("Vertical Axis");

            ListBoxPalette.SelectedIndex = 0;
            ListBoxStyle.SelectedIndex = 0;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var style = (ScottPlot.Styles.IStyle)ListBoxStyle.SelectedItem;
            var palette = (ScottPlot.IPalette)ListBoxPalette.SelectedItem;

            if (style is null || palette is null)
                return;

            AvaPlot1.Plot.Style(style);
            AvaPlot1.Plot.Palette = palette;
            AvaPlot1.Plot.Title($"Style: {style}\nPalette: {palette}");
            AvaPlot1.Plot.Clear();

            for (int i = 0; i < palette.Count(); i++)
            {
                double offset = 1 + (i * 1.1);
                double mult = 10 + i;
                double phase = i * .3 / palette.Count();
                double[] ys = DataGen.Sin(51, 1, offset, mult, phase);
                var sig = AvaPlot1.Plot.AddSignal(ys);
                sig.LineWidth = 3;
                sig.MarkerSize = 0;
            }

            AvaPlot1.Plot.AxisAuto(horizontalMargin: 0);
            AvaPlot1.Refresh();
        }
    }
}
