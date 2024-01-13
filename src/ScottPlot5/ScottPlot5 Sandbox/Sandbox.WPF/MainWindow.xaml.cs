using System.Windows;
using ScottPlot;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        double[] values = Generate.RandomWalk(10_000_000);
        WpfPlot1.Plot.Add.SignalConst(values);
        WpfPlot1.Plot.Title($"{values.Length:N0} Points");
    }
}
