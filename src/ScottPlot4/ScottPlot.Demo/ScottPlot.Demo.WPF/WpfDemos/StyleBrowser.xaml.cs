using System;
using System.Windows;
using System.Windows.Controls;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for StyleBrowser.xaml
    /// </summary>
    public partial class StyleBrowser : Window
    {
        public StyleBrowser()
        {
            InitializeComponent();

            foreach (var style in ScottPlot.Style.GetStyles())
                ListBoxStyle.Items.Add(style);

            foreach (var palette in ScottPlot.Palette.GetPalettes())
                ListBoxPalette.Items.Add(palette);

            WpfPlot1.Plot.XLabel("Horizontal Axis");
            WpfPlot1.Plot.YLabel("Vertical Axis");

            ListBoxStyle.SelectedIndex = 0;
            ListBoxPalette.SelectedIndex = 0;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var style = (ScottPlot.Styles.IStyle)ListBoxStyle.SelectedItem;
            var palette = (ScottPlot.IPalette)ListBoxPalette.SelectedItem;

            if (style is null || palette is null)
                return;

            WpfPlot1.Plot.Style(style);
            WpfPlot1.Plot.Title($"Style: {style}\nPalette: {palette}");
            WpfPlot1.Plot.Palette = palette;

            WpfPlot1.Plot.Clear();
            for (int i = 0; i < palette.Count(); i++)
            {
                double offset = 1 + i * 1.1;
                double mult = 10 + i;
                double phase = i * .3 / palette.Count();
                double[] ys = DataGen.Sin(51, 1, offset, mult, phase);
                var sig = WpfPlot1.Plot.AddSignal(ys);
                sig.LineWidth = 3;
                sig.MarkerSize = 0;
            }

            WpfPlot1.Plot.AxisAuto(horizontalMargin: 0);
            WpfPlot1.Refresh();
        }
    }
}
