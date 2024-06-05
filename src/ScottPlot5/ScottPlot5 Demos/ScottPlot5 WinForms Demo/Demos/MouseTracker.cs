using ScottPlot;

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
        CH.TextColor = Colors.White;
        CH.TextBackgroundColor = CH.HorizontalLine.Color;

        formsPlot1.Refresh();

        formsPlot1.MouseMove += (s, e) =>
        {
            Pixel mousePixel = new(e.X, e.Y);
            Coordinates mouseCoordinates = formsPlot1.Plot.GetCoordinates(mousePixel);
            this.Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3}";
            CH.Position = mouseCoordinates;
            CH.VerticalLine.Text = $"{mouseCoordinates.X:N3}";
            CH.HorizontalLine.Text = $"{mouseCoordinates.Y:N3}";
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
