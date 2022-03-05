using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class TransparentBackground : Window
    {
        public TransparentBackground()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            AvaPlot avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            avaPlot1.Plot.AddScatter(x, sin);
            avaPlot1.Plot.AddScatter(x, cos);

            avaPlot1.Plot.Style(figureBackground: System.Drawing.Color.Transparent);
            avaPlot1.Plot.Style(dataBackground: System.Drawing.Color.Transparent);
            avaPlot1.Refresh();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
