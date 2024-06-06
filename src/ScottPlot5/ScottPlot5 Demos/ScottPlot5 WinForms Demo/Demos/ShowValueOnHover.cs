using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class ShowValueOnHover : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse, Scatter";

    public string Description => "How to sense where the mouse is in coordinate space " +
        "and retrieve information about the plotted data the cursor is hovering over";

    ScottPlot.Plottables.Scatter MyScatter;
    ScottPlot.Plottables.Crosshair MyCrosshair;

    public ShowValueOnHover()
    {
        InitializeComponent();

        double[] xs = Generate.RandomSample(100);
        double[] ys = Generate.RandomSample(100);

        MyScatter = formsPlot1.Plot.Add.Scatter(xs, ys);
        MyScatter.LineWidth = 0;

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
                ? MyScatter.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender)
                : MyScatter.Data.GetNearestX(mouseLocation, formsPlot1.Plot.LastRender);

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
