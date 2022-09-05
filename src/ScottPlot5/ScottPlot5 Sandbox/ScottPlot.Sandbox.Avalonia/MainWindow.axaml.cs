using Avalonia;
using Avalonia.Controls;
using ScottPlot.Avalonia;
using ScottPlot;
using ScottPlot.Plottables;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Sandbox.Avalonia
{
    public partial class MainWindow : Window
    {
        private readonly DebugPoint DebugPoint = new();
        private readonly AvaPlot avaPlot;

        public MainWindow()
        {
            InitializeComponent();
            avaPlot = this.Find<AvaPlot>("AvaPlot");

            const int N = 51;

            avaPlot.Plot.Plottables.Add(DebugPoint);
            avaPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Sin(N), Colors.Blue);
            avaPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Cos(N), Colors.Red);
            avaPlot.Plot.Add.Scatter(Generate.Consecutive(N), Generate.Sin(N, 0.5), Colors.Green);

            avaPlot.PointerMoved += AvaPlot_MouseMove;
        }

        private void InitializeComponent()
        {
            Width = 800;
            Height = 450;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            AvaloniaXamlLoader.Load(this);
        }

        private void AvaPlot_MouseMove(object? sender, PointerEventArgs e)
        {
            DebugPoint.Position = avaPlot.Interaction.GetMouseCoordinates();
            avaPlot.Refresh();
        }

        private void buttonClick(object sender, PointerEventArgs e)
        {
            avaPlot.Refresh();
        }
    }
}
