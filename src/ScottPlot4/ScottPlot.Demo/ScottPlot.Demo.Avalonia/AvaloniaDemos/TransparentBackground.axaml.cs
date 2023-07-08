using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class TransparentBackground : Window
    {
        public TransparentBackground()
        {
            this.InitializeComponent();

            const int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            avaPlot1.Plot.AddScatter(x, sin);
            avaPlot1.Plot.AddScatter(x, cos);

            avaPlot1.Plot.Style(figureBackground: System.Drawing.Color.Transparent);
            avaPlot1.Plot.Style(dataBackground: System.Drawing.Color.Transparent);
            avaPlot1.Refresh();
        }
    }
}
