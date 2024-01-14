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
        formsPlot1.Refresh();

        formsPlot1.MouseMove += (s, e) =>
        {
            Pixel mousePixel = new(e.X, e.Y);
            Coordinates mouseCoordinates = formsPlot1.Plot.GetCoordinates(mousePixel);

            // coordinates may be invalid if requested before the first render
            if (!mouseCoordinates.AreReal)
                return;

            CH.Position = mouseCoordinates;
            formsPlot1.Refresh();

            Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3}";
        };
    }
}
