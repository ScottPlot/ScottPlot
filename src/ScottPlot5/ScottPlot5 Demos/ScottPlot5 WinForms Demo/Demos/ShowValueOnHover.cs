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
            // determine where the mouse is and get the nearest point
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
            DataPoint nearest = MyScatter.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);

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
