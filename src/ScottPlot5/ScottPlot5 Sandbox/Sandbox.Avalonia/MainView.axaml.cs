using Avalonia;
using Avalonia.Controls;
using ScottPlot.Avalonia;
using ScottPlot;

namespace Sandbox.Avalonia;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        var crosshair = AvaPlot.Plot.Add.Crosshair(0, 0);

        AvaPlot.PointerMoved += (s, e) =>
        {
            Point mousePoint = e.GetPosition(this);
            Pixel mousePixel = new(mousePoint.X, mousePoint.Y);
            Coordinates mouseCoordinates = AvaPlot.GetCoordinates(mousePixel);
            crosshair.Position = mouseCoordinates;
            AvaPlot.Refresh();
        };
    }
}
