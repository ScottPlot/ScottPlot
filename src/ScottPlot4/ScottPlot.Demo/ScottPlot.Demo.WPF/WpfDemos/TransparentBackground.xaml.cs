using System.Windows;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for TransparentBackground.xaml
    /// </summary>
    public partial class TransparentBackground : Window
    {
        public TransparentBackground()
        {
            InitializeComponent();

            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            wpfPlot1.Plot.AddScatter(x, sin);
            wpfPlot1.Plot.AddScatter(x, cos);

            wpfPlot1.Plot.Style(figureBackground: System.Drawing.Color.Transparent);
            wpfPlot1.Plot.Style(dataBackground: System.Drawing.Color.Transparent);
            wpfPlot1.Refresh();
        }
    }
}
