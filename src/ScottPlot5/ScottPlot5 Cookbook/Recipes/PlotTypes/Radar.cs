namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Radar : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Radar Plot";
    public string CategoryDescription => "Radar charts (also called a spider charts or star charts) " +
        "represent multi-axis data as a 2D shape on axes arranged circularly around a center point.";

    public class RadarQuickstart : RecipeBase
    {
        public override string Name => "Radar Plot Quickstart";
        public override string Description => "Radar charts (also called a spider charts or star charts) " +
            "represent multi-axis data as a 2D shape on axes arranged circularly around a center point.";

        [Test]
        public override void Execute()
        {
            // create a collection of objects to describe the data being displayed (each has 5 values)
            List<RadarSeries> radarSeries = new()
            {
                new() { Values = new double[] { 5, 4, 5, 2, 3 }, Label = "Green", FillColor = Colors.Green.WithAlpha(.5) },
                new() { Values = new double[] { 2, 3, 2, 4, 2 }, Label = "Blue", FillColor = Colors.Blue.WithAlpha(.5) },
            };

            // add radar data to the plot
            var radar = myPlot.Add.Radar(radarSeries);

            // customize radar axis labels (5 axes because each RadarSeries has 5 values)
            radar.Labels = new string[] { "Axis 1", "Axis 2", "Axis 3", "Axis 4", "Axis 5" }
                .Select(s => new Label() { Text = s, Alignment = Alignment.MiddleCenter })
                .ToArray();

            myPlot.Axes.Frameless();
            myPlot.Axes.Margins(0.5, 0.5);
            myPlot.ShowLegend();
            myPlot.HideGrid();
        }
    }
}
