using Eto.Forms;
using ScottPlot;
using ScottPlot.Plottables;

namespace Sandbox.Eto
{
    partial class MainWindow : Form
    {
        private readonly DebugPoint DebugPoint = new();

        public MainWindow()
        {
            InitializeComponent();


            const int N = 51;

            etoPlot.Plot.Plottables.Add(DebugPoint);
            etoPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Sin(N), Colors.Blue);
            etoPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Cos(N), Colors.Red);
            etoPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Sin(N, 0.5), Colors.Green);
            etoPlot.Refresh();

            etoPlot.MouseMove += EtoPlot_MouseMove;
        }

        private void EtoPlot_MouseMove(object? sender, MouseEventArgs e)
        {
            DebugPoint.Position = etoPlot.Interaction.GetMouseCoordinates();
            etoPlot.Refresh();
        }
    }
}
