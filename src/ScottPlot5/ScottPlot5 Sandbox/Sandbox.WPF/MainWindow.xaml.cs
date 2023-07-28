using System.Windows;
using System.Windows.Input;
using ScottPlot;

#nullable enable

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var crosshair = WpfPlot1.Plot.Add.Crosshair(0, 0);

        WpfPlot1.Refresh();
        WpfPlot1.MouseMove += (s, e) =>
        {
            Point mousePoint = Mouse.GetPosition(WpfPlot1);
            Pixel mousePixel = new(mousePoint.X, mousePoint.Y);
            Coordinates mouseCoordinates = WpfPlot1.GetCoordinates(mousePixel);
            crosshair.Position = mouseCoordinates;
            WpfPlot1.Refresh();
        };
    }
}