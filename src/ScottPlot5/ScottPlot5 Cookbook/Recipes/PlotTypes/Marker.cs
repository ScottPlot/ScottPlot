namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Marker : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Marker";
    public string CategoryDescription => "Markers can be placed on the plot in coordinate space.";

    public class MarkerQuickstart : RecipeBase
    {
        public override string Name => "Marker Quickstart";
        public override string Description => "Markers are symbols placed at a " +
            "location in coordinate space. Their shape, size, and color can be customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Add.Marker(25, .5);
            myPlot.Add.Marker(35, .6);
            myPlot.Add.Marker(45, .7);
        }
    }

    public class MarkerShapes : RecipeBase
    {
        public override string Name => "Marker Shapes";
        public override string Description => "Many marker shapes are available.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Colormaps.Turbo colormap = new();

            for (int i = 0; i < 100; i++)
            {
                MarkerShape shape = Generate.RandomMarkerShape();
                Coordinates location = Generate.RandomCoordinates();
                float size = Generate.RandomInteger(5, 10);
                Color color = Generate.RandomColor(colormap);

                myPlot.Add.Marker(location, shape, size, color);
            }
        }
    }

    public class MarkerLegend : RecipeBase
    {
        public override string Name => "Marker Legend";
        public override string Description => "Markers with labels appear in the legend.";

        [Test]
        public override void Execute()
        {
            var sin = myPlot.Add.Signal(Generate.Sin());
            var cos = myPlot.Add.Signal(Generate.Cos());
            var marker = myPlot.Add.Marker(25, .5);

            sin.Label = "Sine";
            cos.Label = "Cosine";
            marker.Label = "Marker";
            myPlot.ShowLegend();
        }
    }
}
