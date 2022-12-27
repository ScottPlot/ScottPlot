using System.Windows;
using ScottPlot;
using ScottPlot.Plottables;
using Colors = ScottPlot.Colors;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Sandbox.WPF
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

            WpfPlot.Plot.Plottables.Add(DebugPoint);
            WpfPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Sin(N), Colors.Blue);
            WpfPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Cos(N), Colors.Red);
            WpfPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Sin(N, 0.5), Colors.Green);

            WpfPlot.MouseMove += WpfPlot_MouseMove;
        }

        private void WpfPlot_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DebugPoint.Position = WpfPlot.Interaction.GetMouseCoordinates();
            WpfPlot.Refresh();
        }
    }
}
