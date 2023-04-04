using System;
using System.Windows;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for PlotViewer.xaml
    /// </summary>
    public partial class PlotViewer : Window
    {
        Random rand = new Random();

        public PlotViewer()
        {
            InitializeComponent();
        }

        private void LaunchRandomWalk(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(WalkPointsTextbox.Text, out int pointCount))
            {
                MessageBox.Show("invalid number of points", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double[] randomWalkData = DataGen.RandomWalk(rand, pointCount);

            var plt = new ScottPlot.Plot();
            plt.AddSignal(randomWalkData);
            plt.Title($"{pointCount} Random Walk Points");

            var plotViewer = new ScottPlot.WpfPlotViewer(plt, 500, 300, "Random Walk Data");
            plotViewer.Owner = this; // so it closes if this window closes
            plotViewer.wpfPlot1.Configuration.Quality = Control.QualityMode.High; // customize as desired
            plotViewer.Show(); // or ShowDialog() for a blocking window
        }

        private void LaunchRandomSin(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(SinCountTextbox.Text, out int sinCount))
            {
                MessageBox.Show("invalid number of sine waves", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var plt = new ScottPlot.Plot();
            for (int i = 0; i < sinCount; i++)
            {
                double[] randomSinValues = DataGen.Sin(50, rand.NextDouble() * 10, rand.NextDouble(), rand.NextDouble(), rand.NextDouble() * 100);
                plt.AddSignal(randomSinValues);
            }
            plt.Title($"{sinCount} Random Sine Waves");

            var plotViewer = new ScottPlot.WpfPlotViewer(plt, 500, 300, "Random Sine Wave Data");
            plotViewer.Owner = this; // so it closes if this window closes
            plotViewer.wpfPlot1.Configuration.Quality = Control.QualityMode.High; // customize as desired
            plotViewer.Show(); // or ShowDialog() for a blocking window
        }
    }
}
