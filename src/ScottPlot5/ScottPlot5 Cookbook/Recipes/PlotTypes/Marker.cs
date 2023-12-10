namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Marker : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Marker",
        PageDescription = "Custom Markers placed on the plot in coordinate space",
    };

    internal class Quickstart : RecipeTestBase
    {
        public override string Name => "Marker Quickstart";
        public override string Description => "Markers with custom shapes, sizes, and styles can be placed anywhere in coordinate space.";

        [Test]
        public override void Recipe()
        {
            var colormap = new ScottPlot.Colormaps.Turbo();
            MarkerShape[] markerShapes = Enum.GetValues<MarkerShape>().ToArray();
            Random rand = new(0);
            for (int i = 0; i < 100; i++)
            {
                myPlot.Add.Marker(
                    x: rand.NextDouble(),
                    y: rand.NextDouble(),
                    size: 5 + rand.NextSingle() * 20,
                    shape: markerShapes[rand.Next(markerShapes.Length)],
                    color: colormap.GetColor(rand.NextDouble())
                    );
            }
        }
    }
}
