using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class ShowValueOnHover : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse";

    public string Description => "How to sense where the mouse is in coordinate space " +
        "and retrieve information about the plotted data the cursor is hovering over";

    ScottPlot.Plottables.Scatter MyScatter;
    ScottPlot.Plottables.Crosshair MyCrosshair;

    public ShowValueOnHover()
    {
        InitializeComponent();

        double[] xs = Generate.Random(100);
        double[] ys = Generate.Random(100);
        MyScatter = formsPlot1.Plot.Add.Scatter(xs, ys);
        MyScatter.LineStyle = LineStyle.None;
        MyCrosshair = formsPlot1.Plot.Add.Crosshair(0, 0);
        formsPlot1.Plot.AutoScale();
        formsPlot1.Refresh();

        formsPlot1.MouseMove += (s, e) =>
        {
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
            Coordinates nearestPoint = GetNearestCoordinates(mouseLocation);
            MyCrosshair.Position = nearestPoint;
            Text = nearestPoint.ToString();
            formsPlot1.Refresh();
        };
    }

    Coordinates GetNearestCoordinates(Coordinates mouseLocation)
    {
        double closestDistance = double.PositiveInfinity;
        Coordinates closestPoint = Coordinates.Infinity;
        foreach (Coordinates pointCoordinates in MyScatter.Data.GetScatterPoints())
        {
            double dX = (pointCoordinates.X - mouseLocation.X) * formsPlot1.Plot.LastRender.PxPerUnitX;
            double dY = (pointCoordinates.Y - mouseLocation.Y) * formsPlot1.Plot.LastRender.PxPerUnitY;
            double distance = dX * dX + dY * dY;

            if (distance <= closestDistance)
            {
                closestPoint = pointCoordinates;
                closestDistance = distance;
            }
        }
        return closestPoint;
    }
}
