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

            Random rand = new Random(0);
            int pointCount = 10;
            double[] xs = ScottPlot.DataGen.Random(rand, pointCount);
            double[] ys = ScottPlot.DataGen.Random(rand, pointCount);
            WpfPlot1.Plot.AddScatter(xs, ys);

            WpfPlot2.Plot.Title("Plot 2");
            WpfPlot2.Plot.AddSignal(ScottPlot.DataGen.Cos(51));
        }
    }
}
