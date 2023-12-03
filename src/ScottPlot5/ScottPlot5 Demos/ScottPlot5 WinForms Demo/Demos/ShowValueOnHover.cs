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
            // determine where the mouse is
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);

            // get the point closest to the mouse
            int? nearestIndex = MyScatter.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);

            // place the crosshair over the highlighted point
            if (nearestIndex.HasValue)
            {
                IReadOnlyList<Coordinates> points = MyScatter.Data.GetScatterPoints();
                Coordinates c = points[nearestIndex.Value];
                MyCrosshair.IsVisible = true;
                MyCrosshair.Position = c;
                formsPlot1.Refresh();
                Text = $"Selected Index={nearestIndex}, X={c.X:0.##}, Y={c.Y:0.##}";
            }

            // hide the crosshair when no point is selected
            if (!nearestIndex.HasValue && MyCrosshair.IsVisible)
            {
                MyCrosshair.IsVisible = false;
                formsPlot1.Refresh();
                Text = $"No point selected";
            }
        };
    }
}
