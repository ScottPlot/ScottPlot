using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot;

namespace Avalonia_Demo.Demos;

public class MouseTrackerDemo : IDemo
{
    public string Title => "Mouse Tracker";
    public string Description => "Demonstrates how to interact with the mouse " +
        "and convert between screen units (pixels) and axis units (coordinates)";

    public Window GetWindow()
    {
        return new MouseTrackerWindow();
    }
}

public partial class MouseTrackerWindow : Window
{
    public MouseTrackerWindow()
    {
        InitializeComponent();

        var crosshair = AvaPlot.Plot.Add.Crosshair(0, 0);
        crosshair.TextColor = Colors.White;
        crosshair.TextBackgroundColor = crosshair.HorizontalLine.Color;

        AvaPlot.Refresh();

        AvaPlot.PointerMoved += (s, e) =>
        {
            var position = e.GetPosition(this);
            Pixel mousePixel = new(position.X, position.Y);
            Coordinates mouseCoordinates = AvaPlot.Plot.GetCoordinates(mousePixel);
            Title = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3}";
            crosshair.Position = mouseCoordinates;
            crosshair.VerticalLine.Text = $"{mouseCoordinates.X:N3}";
            crosshair.HorizontalLine.Text = $"{mouseCoordinates.Y:N3}";
            AvaPlot.Refresh();
        };

        AvaPlot.PointerPressed += (s, e) =>
        {
            var position = e.GetPosition(this);
            Pixel mousePixel = new(position.X, position.Y);
            Coordinates mouseCoordinates = AvaPlot.Plot.GetCoordinates(mousePixel);
            Title = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3} (mouse down)";
        };
    }
}
