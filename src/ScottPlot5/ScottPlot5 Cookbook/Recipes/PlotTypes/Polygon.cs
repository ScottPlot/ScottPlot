namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Polygon : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Polygon Plot";
    public string CategoryDescription => "Polygon plots draws a polygon";

    public class PolygonQuickstart : RecipeBase
    {
        public override string Name => "Polygon Plot Quickstart";
        public override string Description => "Polygon plots can be added from a series of vertices, and must be in clockwise order.";

        [Test]
        public override void Execute()
        {
            Coordinates[] vertices = new Coordinates[]
            {
                new Coordinates(0,   0.25),
                new Coordinates(0.3, 0.75),
                new Coordinates(1,   1),
                new Coordinates(0.7, 0.5),
                new Coordinates(1,   0)
            };
            myPlot.Add.Polygon(vertices);
            myPlot.Axes.AutoScale();
        }
    }

    public class PolygonStyling : RecipeBase
    {
        public override string Name => "Polygon Plot Styling";
        public override string Description => "Polygon plots can be fully customized.";

        [Test]
        public override void Execute()
        {
            Coordinates[] vertices = new Coordinates[]
            {
                new Coordinates(0,   0.25),
                new Coordinates(0.3, 0.75),
                new Coordinates(1,   1),
                new Coordinates(0.7, 0.5),
                new Coordinates(1,   0)
            };
            var poly = myPlot.Add.Polygon(vertices);
            poly.FillStyle = new FillStyle
            {
                Color = Colors.IndianRed
            };
            poly.LineStyle = new LineStyle
            {
                AntiAlias = true,
                Color = Colors.Black,
                Pattern = LinePattern.Dashed,
                Width = 2
            };
            poly.MarkerStyle = new MarkerStyle(MarkerShape.OpenCircle, 8);
            poly.MarkerStyle.Fill.Color = Colors.Gold;
            poly.MarkerStyle.Outline.Color = Colors.Brown;
            myPlot.Axes.AutoScale();
        }
    }
}
