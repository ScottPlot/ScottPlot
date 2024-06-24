using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class ShowValueOnHoverMultiple : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse, Multiple Scatter";

    public string Description => "How to sense where the mouse is in coordinate space " +
        "and retrieve information about the plottable and data the cursor is hovering over";

    List<ScottPlot.Plottables.Scatter> MyScatters = new();
    ScottPlot.Plottables.Crosshair MyCrosshair;
    ScottPlot.Plottables.Marker MyHighlightMarker;
    ScottPlot.Plottables.Text MyHighlightText;

    public ShowValueOnHoverMultiple()
    {
        InitializeComponent();

        // create 3 scatter plots with random points
        for (int i = 0; i < 3; i++)
        {
            double[] xs = Generate.RandomSample(30);
            double[] ys = Generate.RandomSample(30);
            ScottPlot.Plottables.Scatter scatter = formsPlot1.Plot.Add.ScatterPoints(xs, ys);
            scatter.LegendText = $"Scatter {i}";
            scatter.MarkerStyle.Size = 10;
            MyScatters.Add(scatter);
        }
        formsPlot1.Plot.ShowLegend();

        // Create a marker to highlight the point under the cursor
        MyCrosshair = formsPlot1.Plot.Add.Crosshair(0, 0);
        MyHighlightMarker = formsPlot1.Plot.Add.Marker(0, 0);
        MyHighlightMarker.Shape = MarkerShape.OpenCircle;
        MyHighlightMarker.Size = 17;
        MyHighlightMarker.LineWidth = 2;

        // Create a text label to place near the highlighted value
        MyHighlightText = formsPlot1.Plot.Add.Text("", 0, 0);
        MyHighlightText.LabelAlignment = Alignment.LowerLeft;
        MyHighlightText.LabelBold = true;
        MyHighlightText.OffsetX = 7;
        MyHighlightText.OffsetY = -7;

        // Render the plot
        formsPlot1.Refresh();

        // Evaluate points every time the mouse moves.
        // Indicate the nearest point by modifying the crosshair, text, marker, and window title.
        formsPlot1.MouseMove += (s, e) =>
        {
            // determine where the mouse is
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);

            // get the nearest point of each scatter
            Dictionary<int, DataPoint> nearestPoints = new();
            for (int i = 0; i < MyScatters.Count; i++)
            {
                DataPoint nearestPoint = MyScatters[i].Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);
                nearestPoints.Add(i, nearestPoint);
            }

            // determine which scatter's nearest point is nearest to the mouse
            bool pointSelected = false;
            int scatterIndex = -1;
            double smallestDistance = double.MaxValue;
            for (int i = 0; i < nearestPoints.Count; i++)
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
                        // update the smallest distance
                        smallestDistance = distance;
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
                MyCrosshair.LineColor = scatter.MarkerStyle.FillColor;

                MyHighlightMarker.IsVisible = true;
                MyHighlightMarker.Location = point.Coordinates;
                MyHighlightMarker.MarkerStyle.LineColor = scatter.MarkerStyle.FillColor;

                MyHighlightText.IsVisible = true;
                MyHighlightText.Location = point.Coordinates;
                MyHighlightText.LabelText = $"{point.X:0.##}, {point.Y:0.##}";
                MyHighlightText.LabelFontColor = scatter.MarkerStyle.FillColor;

                formsPlot1.Refresh();
                base.Text = $"Selected Scatter={scatter.LegendText}, Index={point.Index}, X={point.X:0.##}, Y={point.Y:0.##}";
            }

            // hide the crosshair, marker and text when no point is selected
            if (!pointSelected && MyCrosshair.IsVisible)
            {
                MyCrosshair.IsVisible = false;
                MyHighlightMarker.IsVisible = false;
                MyHighlightText.IsVisible = false;
                formsPlot1.Refresh();
                base.Text = $"No point selected";
            }
        };
    }
}
