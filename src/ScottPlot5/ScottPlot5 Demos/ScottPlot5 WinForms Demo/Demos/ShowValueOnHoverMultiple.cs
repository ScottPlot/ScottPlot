using ScottPlot;
using ScottPlot.Plottables;
using ScottPlotCookbook.Recipes.PlotTypes;

namespace WinForms_Demo.Demos;

public partial class ShowValueOnHoverMultiple : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse, Multiple Series";

    public string Description => "How to sense where the mouse is in coordinate space " +
        "and retrieve information about the plottable and data the cursor is hovering over";

    List<ScottPlot.Plottables.Scatter> MyScatters = new();
    ScottPlot.Plottables.Crosshair MyCrosshair;
    ScottPlot.Plottables.Marker MyMarker;
    ScottPlot.Plottables.Text MyText;

    int NumScatters = 3;

    public ShowValueOnHoverMultiple()
    {
        InitializeComponent();

        // create multiple scatter plots with random points
        for (int i = 0; i < NumScatters; i++)
        {
            double[] xs = Generate.RandomSample(30);
            double[] ys = Generate.RandomSample(30);
            ScottPlot.Plottables.Scatter scatter = formsPlot1.Plot.Add.ScatterPoints(xs, ys);
            scatter.Label = $"Scatter {i}";
            MyScatters.Add(scatter);
        }

        formsPlot1.Plot.ShowLegend();

        MyCrosshair = formsPlot1.Plot.Add.Crosshair(0, 0);

        MyMarker = formsPlot1.Plot.Add.Marker(0, 0, MarkerShape.OpenCircle);

        MyText = formsPlot1.Plot.Add.Text("", 0, 0);
        MyText.Label.Alignment = Alignment.LowerLeft;
        MyText.PixelOffset = new PixelSize(5, -5);

        formsPlot1.Plot.Axes.AutoScale();
        formsPlot1.Refresh();

        formsPlot1.MouseMove += (s, e) =>
        {
            // determine where the mouse is
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);

            // get the nearest point of each scatter
            Dictionary<int, DataPoint> nearestPoints = new();
            for (int i = 0; i < NumScatters; i++)
            {
                DataPoint nearestPoint = MyScatters[i].Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);
                nearestPoints.Add(i, nearestPoint);
            }

            // determine which scatter's nearest point is nearest to the mouse
            bool pointSelected = false;
            int scatterIndex = -1;
            double smallestDistance = double.MaxValue;
            for (int i = 0; i < NumScatters; i++)
            {
                if (nearestPoints[i].IsReal)
                {
                    // calculate the distance of the point to the mouse
                    double distance = nearestPoints[i].Coordinates.Distance(mouseLocation);
                    if (distance < smallestDistance)
                    {
                        // store the index
                        scatterIndex = i;
                        pointSelected = true;
                    }
                }
            }

            // place the crosshair, marker and text over the selected point
            if (pointSelected)
            {
                ScottPlot.Plottables.Scatter scatter = MyScatters[scatterIndex];
                DataPoint point = nearestPoints[scatterIndex];

                MyCrosshair.IsVisible = true;
                MyCrosshair.Position = point.Coordinates;
                MyCrosshair.LineStyle.Color = scatter.MarkerStyle.Fill.Color;

                MyMarker.IsVisible = true;
                MyMarker.Location = point.Coordinates;
                MyMarker.MarkerStyle.Outline.Color = scatter.MarkerStyle.Fill.Color;

                MyText.IsVisible = true;
                MyText.Location = point.Coordinates;
                MyText.LabelText = $"{point.X:0.##}, {point.Y:0.##}";
                MyText.Color = scatter.MarkerStyle.Fill.Color;

                formsPlot1.Refresh();
                base.Text = $"Selected Scatter={scatter.Label}, Index={point.Index}, X={point.X:0.##}, Y={point.Y:0.##}";
            }

            // hide the crosshair, marker and text when no point is selected
            if (!pointSelected && MyCrosshair.IsVisible)
            {
                MyCrosshair.IsVisible = false;
                MyMarker.IsVisible = false;
                MyText.IsVisible = false;
                formsPlot1.Refresh();
                base.Text = $"No point selected";
            }
        };
    }
}
