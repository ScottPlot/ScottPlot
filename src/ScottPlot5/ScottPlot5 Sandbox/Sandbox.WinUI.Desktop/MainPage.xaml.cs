using Microsoft.UI.Xaml.Controls;
using ScottPlot;

namespace Sandbox.WinUI;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        WinUIPlot.AppWindow = App.MainWindow;

        WinUIPlot.UserInputProcessor.IsEnabled = true;

        WinUIPlot.Plot.Add.Signal(Generate.Sin());
        WinUIPlot.Plot.Add.Signal(Generate.Cos());
    }
}
