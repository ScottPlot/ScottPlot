using System.Windows;
using ScottPlot;

#nullable enable

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        WpfPlot.Plot.Add.Signal(Generate.Sin());
        WpfPlot.Plot.Add.Signal(Generate.Cos());

        WpfPlot.Refresh();
    }
}
