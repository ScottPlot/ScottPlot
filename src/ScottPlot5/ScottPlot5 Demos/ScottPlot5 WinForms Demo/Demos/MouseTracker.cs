using ScottPlot;
using System.Diagnostics;

namespace WinForms_Demo.Demos;

public partial class MouseTracker : Form, IDemoWindow
{
    public string Title => "Mouse Tracker";

    public string Description => "Demonstrates how to interact with the mouse " +
        "and convert between screen units (pixels) and axis units (coordinates)";

    readonly ScottPlot.Plottables.Crosshair CH;

    public MouseTracker()
    {
        InitializeComponent();

        CH = formsPlot1.Plot.Add.Crosshair(0, 0);
        formsPlot1.Refresh();

        formsPlot1.MouseMove += (s, e) =>
        {
            Pixel mousePixel = new(e.X, e.Y);
            Coordinates mouseCoordinates = formsPlot1.Plot.GetCoordinates(mousePixel);
            string msg = e.Button == MouseButtons.Left ? "dragging" : "hovering";
            Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3} ({msg})";
            CH.Position = mouseCoordinates;
            CH.TextX = $"{mouseCoordinates.X:N3}";
            CH.TextY = $"{mouseCoordinates.Y:N3}";
            formsPlot1.Refresh();
        };

        formsPlot1.MouseDown += (s, e) =>
        {
            Pixel mousePixel = new(e.X, e.Y);
            Coordinates mouseCoordinates = formsPlot1.Plot.GetCoordinates(mousePixel);
            Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3} (mouse down)";
        };
    }
}
