using System;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Random rand = new(0);
            int pointCount = 2_000;
            double[] xs = ScottPlot.DataGen.Random(rand, pointCount);
            double[] ys = ScottPlot.DataGen.Random(rand, pointCount);

            WpfPlot1.Plot.Title("Blocking Render");
            WpfPlot1.Plot.AddScatter(xs, ys);
            WpfPlot1.Configuration.UseRenderQueue = false;

            WpfPlot2.Plot.Title("Render Queue");
            WpfPlot2.Plot.AddScatter(xs, ys);
            WpfPlot2.Configuration.UseRenderQueue = true;
        }
    }
}
