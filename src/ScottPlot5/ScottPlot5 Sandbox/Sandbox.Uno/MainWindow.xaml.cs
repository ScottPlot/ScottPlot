using System.Windows;
using ScottPlot;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Sandbox.Uno;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        UnoPlot.Plot.Add.Signal(Generate.Sin(51));
        UnoPlot.Plot.Add.Signal(Generate.Cos(51));
    }
}
