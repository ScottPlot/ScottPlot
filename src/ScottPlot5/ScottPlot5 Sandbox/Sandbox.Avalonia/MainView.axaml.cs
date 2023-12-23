using Avalonia.Controls;
using ScottPlot.Avalonia;
using ScottPlot;

namespace Sandbox.Avalonia;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        AvaPlot.Plot.Add.Signal(Generate.Sin());
        AvaPlot.Plot.Add.Signal(Generate.Cos());
        AvaPlot.Refresh();
    }
}
