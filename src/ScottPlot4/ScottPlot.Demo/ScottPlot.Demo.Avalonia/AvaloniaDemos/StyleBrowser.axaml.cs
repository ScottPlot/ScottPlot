using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Styles;
using System.Collections.Generic;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class StyleBrowser : Window
    {
        private readonly AvaPlot avaPlot;
        private readonly ListBox listBoxStyle;
        private readonly ListBox listBoxPalette;
        public StyleBrowser()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            avaPlot = this.Find<AvaPlot>("AvaPlot1");
            listBoxStyle = this.Find<ListBox>("ListBoxStyle");
            listBoxPalette = this.Find<ListBox>("ListBoxPalette");

            listBoxStyle.Items = ScottPlot.Style.GetStyles();
            listBoxPalette.Items = ScottPlot.Palette.GetPalettes();

            avaPlot.Plot.XLabel("Horizontal Axis");
            avaPlot.Plot.YLabel("Vertical Axis");

            listBoxPalette.SelectedIndex = 0;
            listBoxStyle.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var style = (ScottPlot.Styles.IStyle)listBoxStyle.SelectedItem;
            var palette = (ScottPlot.IPalette)listBoxPalette.SelectedItem;

            if (style is null || palette is null)
                return;

            avaPlot.Plot.Style(style);
            avaPlot.Plot.Palette = palette;
            avaPlot.Plot.Title($"Style: {style}\nPalette: {palette}");
            avaPlot.Plot.Clear();

            for (int i = 0; i < palette.Count(); i++)
            {
                double offset = 1 + i * 1.1;
                double mult = 10 + i;
                double phase = i * .3 / palette.Count();
                double[] ys = DataGen.Sin(51, 1, offset, mult, phase);
                var sig = avaPlot.Plot.AddSignal(ys);
                sig.LineWidth = 3;
                sig.MarkerSize = 0;
            }

            avaPlot.Plot.AxisAuto(horizontalMargin: 0);
            avaPlot.Refresh();
        }
    }
}
