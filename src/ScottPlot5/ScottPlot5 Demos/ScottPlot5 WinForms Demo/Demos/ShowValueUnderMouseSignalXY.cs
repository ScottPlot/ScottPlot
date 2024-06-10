using ScottPlot;
using System.Diagnostics;

namespace WinForms_Demo.Demos;

public partial class ShowValueUnderMouseSignalXY : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse, SignalXY";

    public string Description => "Demonstrates how to determine where the cursor is " +
        "in coordinate space and identify the data point closest to it.";

    readonly ScottPlot.Plottables.SignalXY MySignal;
    ScottPlot.Plottables.Crosshair MyCrosshair;

    public ShowValueUnderMouseSignalXY()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(1000);
        double[] ys = Generate.RandomWalk(1000);
        MySignal = formsPlot1.Plot.Add.SignalXY(xs, ys);

        MyCrosshair = formsPlot1.Plot.Add.Crosshair(0, 0);
        MyCrosshair.IsVisible = false;
        MyCrosshair.MarkerShape = MarkerShape.OpenCircle;
        MyCrosshair.MarkerSize = 15;

        formsPlot1.MouseMove += (s, e) =>
        {
            // determine where the mouse is and get the nearest point
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
            DataPoint nearest = rbNearestXY.Checked
                ? MySignal.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender)
                : MySignal.Data.GetNearestX(mouseLocation, formsPlot1.Plot.LastRender);

            // place the crosshair over the highlighted point
            if (nearest.IsReal)
            {
                MyCrosshair.IsVisible = true;
                MyCrosshair.Position = nearest.Coordinates;
                formsPlot1.Refresh();
                Text = $"Selected Index={nearest.Index}, X={nearest.X:0.##}, Y={nearest.Y:0.##}";
            }

            // hide the crosshair when no point is selected
            if (!nearest.IsReal && MyCrosshair.IsVisible)
            {
                MyCrosshair.IsVisible = false;
                formsPlot1.Refresh();
                Text = $"No point selected";
            }
        };
    }
}
