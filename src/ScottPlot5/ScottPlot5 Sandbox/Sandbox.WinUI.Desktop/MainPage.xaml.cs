using Microsoft.UI.Xaml.Controls;
using ScottPlot;

namespace Sandbox.WinUI;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        WinUIPlot.AppWindow = App.MainWindow;

        WinUIPlot.Plot.Add.Signal(Generate.Sin());
        WinUIPlot.Plot.Add.Signal(Generate.Cos());
        WinUIPlot.Refresh();
    }
}
