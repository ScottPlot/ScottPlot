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
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Add.LinePlot(1, 12, 12, 0);
            myPlot.Add.LinePlot(7, 9, 42, 9);
            myPlot.Add.LinePlot(30, 17, 30, 1);
        }
    }

    public class LinePlotStyles : RecipeBase
    {
        public override string Name => "Line Plot Shapes";
        public override string Description => "Line plots can be styled using a LineStyle.";

        [Test]
        public override void Execute()
        {

            ScottPlot.Colormaps.Turbo colormap = new();
            for (int i = 0; i < 20; i++)
            {
                LineStyle style = new()
                {
                    AntiAlias = true,
                    Color = Generate.RandomColor(colormap),
                    Pattern = Generate.RandomLinePattern(),
                    Width = Generate.RandomInteger(1, 4)
                };
                Coordinates start = new(Generate.RandomInteger(0, 20), Generate.RandomInteger(0, 20));
                Coordinates end = new(Generate.RandomInteger(0, 20), Generate.RandomInteger(0, 20));
                myPlot.Add.LinePlot(start, end, style);
            }
        }
    }

    public class LinePlotLegend : RecipeBase
    {
        public override string Name => "Line Plot Legend";
        public override string Description => "Line plots with labels appear in the legend.";

        [Test]
        public override void Execute()
        {
            var sin = myPlot.Add.Signal(Generate.Sin());
            var cos = myPlot.Add.Signal(Generate.Cos());
            var linePlot = myPlot.Add.LinePlot(1, 12, 12, 0);

            sin.Label = "Sine";
            cos.Label = "Cosine";
            linePlot.Label = "Line Plot";
            myPlot.ShowLegend();
        }
    }
}

