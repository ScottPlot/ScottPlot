using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System;
using static ScottPlot.Demo.Avalonia.MessageBox;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class PlotViewer : Window
    {
        Random rand = new Random();
        public PlotViewer()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void LaunchRandomWalk(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.Find<TextBox>("WalkPointsTextbox").Text, out int pointCount))
            {
                await MessageBox.Show(this, "invalid number of points", "ERROR", MessageBoxButtons.Ok);
                return;
            }

            double[] randomWalkData = DataGen.RandomWalk(rand, pointCount);

            var plt = new ScottPlot.Plot();
            plt.AddSignal(randomWalkData);
            plt.Title($"{pointCount} Random Walk Points");

            var plotViewer = new ScottPlot.Avalonia.AvaPlotViewer(plt, 500, 300, "Random Walk Data");
            plotViewer.SetWindowOwner(this); // so it closes if this window closes
            plotViewer.Find<AvaPlot>("avaPlot1").Configuration.Quality = Control.QualityMode.High; // customize as desired
            plotViewer.Show(); // or ShowDialog() for a blocking window
        }

        private void LaunchRandomSin(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.Find<TextBox>("SinCountTextbox").Text, out int sinCount))
            {
                MessageBox.Show(this, "invalid number of sine waves", "ERROR", MessageBoxButtons.Ok);
                return;
            }

            var plt = new ScottPlot.Plot();
            for (int i = 0; i < sinCount; i++)
            {
                double[] randomSinValues = DataGen.Sin(50, rand.NextDouble() * 10, rand.NextDouble(), rand.NextDouble(), rand.NextDouble() * 100);
                plt.AddSignal(randomSinValues);
            }
            plt.Title($"{sinCount} Random Sine Waves");

            var plotViewer = new ScottPlot.Avalonia.AvaPlotViewer(plt, 500, 300, "Random Sine Wave Data");
            plotViewer.SetWindowOwner(this); // so it closes if this window closes
            plotViewer.Find<AvaPlot>("avaPlot1").Configuration.Quality = Control.QualityMode.High; // customize as desired
            plotViewer.Show(); // or ShowDialog() for a blocking window
        }
    }
}
