using System.Windows;
using ScottPlot;
using ScottPlot.Plottables;
using Colors = ScottPlot.Colors;

#pragma warning disable CA1416 // Validate platform compatibility

namespace WPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly DebugPoint DebugPoint = new();

        public MainWindow()
        {
            InitializeComponent();

            const int N = 51;

            WpfPlot.Plot.Add(DebugPoint);
            WpfPlot.Plot.AddScatter(Generate.Consecutive(N), Generate.Sin(N), Colors.Blue);
            WpfPlot.Plot.AddScatter(Generate.Consecutive(N), Generate.Cos(N), Colors.Red);
            WpfPlot.Plot.AddScatter(Generate.Consecutive(N), Generate.Sin(N, 0.5), Colors.Green);

            WpfPlot.MouseMove += WpfPlot_MouseMove;
        }

        private void WpfPlot_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DebugPoint.Position = WpfPlot.Interaction.GetMouseCoordinates();
            WpfPlot.Refresh();
        }
    }
}
