using ScottPlot;

namespace Sandbox.UnoPlatform;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();

        WinUIPlot.UserInputProcessor.IsEnabled = true;

        WinUIPlot.Plot.Add.Signal(Generate.Sin());
        WinUIPlot.Plot.Add.Signal(Generate.Cos());
    }
}
