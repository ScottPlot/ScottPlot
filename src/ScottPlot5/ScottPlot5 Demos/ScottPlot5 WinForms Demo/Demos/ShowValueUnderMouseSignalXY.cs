using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class ShowValueUnderMouseSignalXY : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse, SignalXY";

    public string Description => "Demonstrates how to determine where the cursor is " +
        "in coordinate space and identify the data point closest to it.";

    readonly ScottPlot.Plottables.SignalXY MySignal;
    readonly ScottPlot.Plottables.SignalXY MySignalDifferentAxes;
    ScottPlot.Plottables.Crosshair MyCrosshair;
    ScottPlot.Plottables.Crosshair MyCrosshairDifferentAxes;

    string MyCrosshairText = "";
    string MyCrosshairDifferentAxesText = "";

    public ShowValueUnderMouseSignalXY()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(10_000_000);
        double[] ys = Generate.RandomWalk(10_000_000);

        double[] xs1 = Generate.Consecutive(2000);
        double[] ys1 = Generate.RandomWalk(2000);

        formsPlot1.Plot.Axes.AddRightAxis();
        formsPlot1.Plot.Axes.AddTopAxis();

        MySignal = formsPlot1.Plot.Add.SignalXY(xs, ys);
        MySignal.Data.Rotated = true;

        MySignalDifferentAxes = formsPlot1.Plot.Add.SignalXY(xs1, ys1);
        MySignalDifferentAxes.Axes.XAxis = formsPlot1.Plot.Axes.Top;
        MySignalDifferentAxes.Axes.YAxis = formsPlot1.Plot.Axes.Right;

        formsPlot1.Plot.Axes.Bottom.TickLabelStyle.ForeColor = MySignal.Color;
        formsPlot1.Plot.Axes.Left.TickLabelStyle.ForeColor = MySignal.Color;

        formsPlot1.Plot.Axes.Top.TickLabelStyle.ForeColor = MySignalDifferentAxes.Color;
        formsPlot1.Plot.Axes.Right.TickLabelStyle.ForeColor = MySignalDifferentAxes.Color;

        MyCrosshair = formsPlot1.Plot.Add.Crosshair(0, 0);
        MyCrosshair.IsVisible = false;
        MyCrosshair.MarkerShape = MarkerShape.OpenCircle;
        MyCrosshair.MarkerSize = 15;

        MyCrosshairDifferentAxes = formsPlot1.Plot.Add.Crosshair(0, 0);

        MyCrosshairDifferentAxes.Axes.XAxis = formsPlot1.Plot.Axes.Top;
        MyCrosshairDifferentAxes.Axes.YAxis = formsPlot1.Plot.Axes.Right;
        MyCrosshairDifferentAxes.IsVisible = false;
        MyCrosshairDifferentAxes.MarkerShape = MarkerShape.OpenCircle;
        MyCrosshairDifferentAxes.MarkerSize = 15;

        formsPlot1.MouseMove += (s, e) =>
        {
            if (formsPlot1.Plot.LastRender.AxisLimitsByAxis is null)
                return;

            // determine where the mouse is and get the nearest point
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
            Coordinates mouseLocationDifferentAxes = formsPlot1.Plot.GetCoordinates(mousePixel, MySignalDifferentAxes.Axes.XAxis, MySignalDifferentAxes.Axes.YAxis);
            DataPoint nearest = rbNearestXY.Checked
                ? MySignal.GetNearest(mouseLocation, formsPlot1.Plot.LastRender)
                : MySignal.GetNearestX(mouseLocation, formsPlot1.Plot.LastRender);

            DataPoint nearestDifferentAxes = rbNearestXY.Checked
                ? MySignalDifferentAxes.GetNearest(mouseLocationDifferentAxes, formsPlot1.Plot.LastRender)
                : MySignalDifferentAxes.GetNearestX(mouseLocationDifferentAxes, formsPlot1.Plot.LastRender);

            // place the crosshair over the highlighted point
            if (nearest.IsReal)
            {
                MyCrosshair.IsVisible = true;
                MyCrosshair.Position = nearest.Coordinates;
                formsPlot1.Refresh();
                MyCrosshairText = $"Selected Index={nearest.Index}, X={nearest.X:0.##}, Y={nearest.Y:0.##}";
            }
            // place the crosshair over the highlighted point
            if (nearestDifferentAxes.IsReal)
            {
                MyCrosshairDifferentAxes.IsVisible = true;
                MyCrosshairDifferentAxes.Position = nearestDifferentAxes.Coordinates;
                formsPlot1.Refresh();
                MyCrosshairDifferentAxesText = $"Selected Index={nearestDifferentAxes.Index}, X={nearestDifferentAxes.X:0.##}, Y={nearestDifferentAxes.Y:0.##}";
            }

            // hide the crosshair when no point is selected
            if (!nearest.IsReal && MyCrosshair.IsVisible)
            {
                MyCrosshair.IsVisible = false;
                formsPlot1.Refresh();
                MyCrosshairText = $"No point selected";
            }
            if (!nearestDifferentAxes.IsReal && MyCrosshairDifferentAxes.IsVisible)
            {
                MyCrosshairDifferentAxes.IsVisible = false;
                formsPlot1.Refresh();
                MyCrosshairDifferentAxesText = $"No point selected";
            }
            Text = $"Signal1: {MyCrosshairText}, Signal2: {MyCrosshairDifferentAxesText}";
        };
    }
}
