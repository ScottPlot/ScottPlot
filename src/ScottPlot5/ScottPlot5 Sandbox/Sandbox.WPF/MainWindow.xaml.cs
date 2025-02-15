using System.Windows;
using ScottPlot;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        WpfPlot1.Plot.Add.Signal(Generate.Sin());
        WpfPlot1.Plot.Add.Signal(Generate.Cos());
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        WpfPlot1.Reset();
        WpfPlot1.Plot.Add.Signal(Generate.RandomWalk(100));
        WpfPlot1.Refresh();
    }
}
