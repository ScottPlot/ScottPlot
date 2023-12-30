namespace ScottPlotCookbook.Recipes.PlotTypes;

public class LinePlot : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Line Plot";
    public string CategoryDescription => "Line plots can be placed on the plot in coordinate " +
        "space using a Start, End, and an optional LineStyle.";

    public class LineQuickStart : RecipeBase
    {
        public override string Name => "Line Plot Quickstart";
        public override string Description => "Line plots are placed with a start and end " +
            "location in coordinate space. Their styles can be customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Line(1, 12, 12, 0);
            myPlot.Add.Line(7, 9, 42, 9);
            myPlot.Add.Line(30, 17, 30, 1);
        }
    }

    public class LinePlotStyles : RecipeBase
    {
        public override string Name => "Line Plot Shapes";
        public override string Description => "Line plots can be styled using a LineStyle.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Colormaps.Viridis colormap = new();

            for (int i = 0; i < 10; i++)
            {
                // add a line
                Coordinates start = Generate.RandomCoordinates();
                Coordinates end = Generate.RandomCoordinates();
                var line = myPlot.Add.Line(start, end);

                // customize the line
                line.LineStyle.Color = Generate.RandomColor(colormap);
                line.LineStyle.Width = Generate.RandomInteger(1, 4);
                line.LineStyle.Pattern = Generate.RandomLinePattern();

                // customize markers
                line.MarkerStyle.Fill.Color = line.LineStyle.Color;
                line.MarkerStyle.Shape = Generate.RandomMarkerShape();
                line.MarkerStyle.Size = Generate.RandomInteger(5, 15);
            }
        }
    }

    public class LinePlotLegendQWER : RecipeBase
    {
        public override string Name => "Line Plot Legend";
        public override string Description => "Line plots with labels appear in the legend.";

        [Test]
        public override void Execute()
        {
            var sin = myPlot.Add.Signal(Generate.Sin());
            var cos = myPlot.Add.Signal(Generate.Cos());

            var line = myPlot.Add.Line(1, 12, 12, 0);
            line.LineStyle.Width = 3;
            line.MarkerStyle.Size = 10;

            sin.Label = "Sine";
            cos.Label = "Cosine";
            line.Label = "Line Plot";

            myPlot.ShowLegend(Alignment.UpperRight);
        }
    }
}

