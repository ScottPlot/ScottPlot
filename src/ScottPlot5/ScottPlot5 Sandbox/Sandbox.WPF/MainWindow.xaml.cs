using System.Windows;
using ScottPlot;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        WpfPlot1.Plot.Add.Signal(Generate.Sin());
        WpfPlot1.Plot.Axes.SetLimitsX(0, -100);
        WpfPlot1.Plot.Axes.SetLimitsY(1, -1);
    }
}
