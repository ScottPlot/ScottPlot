using System.Windows;
using ScottPlot;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        WpfPlot.Plot.Add.Signal(Generate.Sin(51));
        WpfPlot.Plot.Add.Signal(Generate.Cos(51));
    }
}
