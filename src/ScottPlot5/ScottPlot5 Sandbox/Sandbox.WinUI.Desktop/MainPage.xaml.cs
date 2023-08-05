using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using ScottPlot;

namespace Sandbox.WinUI;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        WinUIPlot.AppWindow = App.MainWindow;

        var crosshair = WinUIPlot.Plot.Add.Crosshair(0, 0);

        WinUIPlot.PointerMoved += (s, e) =>
        {
            Point mousePoint = e.GetCurrentPoint(this).Position;
            Pixel mousePixel = new(mousePoint.X, mousePoint.Y);
            Coordinates mouseCoordinates = WinUIPlot.GetCoordinates(mousePixel);
            crosshair.Position = mouseCoordinates;
            WinUIPlot.Refresh();
        };
    }
}
