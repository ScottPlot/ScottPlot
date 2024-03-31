using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class SignalXYDrag : Form, IDemoWindow
{
    public string Title => "Mouse Interactive SignalXY Plots";

    public string Description => "Demonstrates how to create SignalXY plots " +
        "which can be dragged with the mouse, and also how to display informatoin " +
        "about which point is nearest the cursor.";

    SignalXY? PlottableBeingDragged = null;
    DataPoint StartingDragPosition = DataPoint.None;
    double StartingDragOffset = 0;
    Marker HighlightedPointMarker;

    public SignalXYDrag()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(100);
        double[] ys1 = Generate.Sin(100);
        double[] ys2 = Generate.Cos(100);

        formsPlot1.Plot.Add.SignalXY(xs, ys1);
        formsPlot1.Plot.Add.SignalXY(xs, ys2);

        HighlightedPointMarker = formsPlot1.Plot.Add.Marker(0, 0);
        HighlightedPointMarker.IsVisible = false;
        HighlightedPointMarker.Size = 15;
        HighlightedPointMarker.LineWidth = 2;
        HighlightedPointMarker.Shape = MarkerShape.OpenCircle;

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        (SignalXY? sigXY, DataPoint dataPoint) = GetSignalXYUnderMouse(formsPlot1.Plot, e.X, e.Y);
        if (sigXY is null)
            return;

        PlottableBeingDragged = sigXY;
        StartingDragPosition = dataPoint;
        StartingDragOffset = sigXY.Data.XOffset;
        formsPlot1.Interaction.Disable(); // disable panning while dragging
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        PlottableBeingDragged = null;
        StartingDragPosition = DataPoint.None;
        formsPlot1.Interaction.Enable(); // enable panning again
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        // this rectangle is the area around the mouse in coordinate units
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y, radius: 5);

        // update the cursor to reflect what is beneath it
        if (PlottableBeingDragged is null)
        {
            (var signalUnderMouse, DataPoint dp) = GetSignalXYUnderMouse(formsPlot1.Plot, e.X, e.Y);
            Cursor = signalUnderMouse is null ? Cursors.Arrow : Cursors.SizeWE;
            HighlightedPointMarker.IsVisible = signalUnderMouse is not null;

            if (signalUnderMouse is not null)
            {
                HighlightedPointMarker.Location = dp.Coordinates;
                HighlightedPointMarker.Color = signalUnderMouse.Color;
                Text = $"Index {dp.Index} at {dp.Coordinates}";
                formsPlot1.Refresh();
            }

            return;
        }

        // update the position of the plottable being dragged
        if (PlottableBeingDragged is SignalXY sigXY)
        {
            HighlightedPointMarker.IsVisible = false;
            sigXY.Data.XOffset = rect.HorizontalCenter - StartingDragPosition.X + StartingDragOffset;
            formsPlot1.Refresh();
        }
    }

    /// <summary>
    /// Returns the SignalXY object and data point beneath the mouse,
    /// or null if nothing is beneath the mouse.
    /// </summary>
    private static (SignalXY? signalXY, DataPoint point) GetSignalXYUnderMouse(Plot plot, double x, double y)
    {
        Pixel mousePixel = new(x, y);

        Coordinates mouseLocation = plot.GetCoordinates(mousePixel);

        foreach (SignalXY signal in plot.GetPlottables<SignalXY>().Reverse())
        {
            DataPoint nearest = signal.Data.GetNearest(mouseLocation, plot.LastRender);
            if (nearest.IsReal)
            {
                return (signal, nearest);
            }
        }

        return (null, DataPoint.None);
    }
}
