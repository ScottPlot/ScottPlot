using ScottPlot.Hatches;

namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Shapes : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Shapes";
    public string CategoryDescription => "Basic shapes that can be added to plots";

    public class RectangleQuickstart : RecipeBase
    {
        public override string Name => "Rectangle";
        public override string Description => "A rectangle can be added to the plot and styled as desired.";

        [Test]
        public override void Execute()
        {
            // add a rectangle by specifying points
            myPlot.Add.Rectangle(0, 1, 0, 1);

            // add a rectangle using more expressive shapes
            Coordinates location = new(2, 0);
            CoordinateSize size = new(1, 1);
            CoordinateRect rect = new(location, size);
            myPlot.Add.Rectangle(rect);

            // style rectangles after they are added to the plot
            var rp = myPlot.Add.Rectangle(4, 5, 0, 1);
            rp.FillStyle.Color = Colors.Magenta.WithAlpha(.2);
            rp.LineStyle.Color = Colors.Green;
            rp.LineStyle.Width = 3;
            rp.LineStyle.Pattern = LinePattern.Dashed;
        }
    }

    public class CircleQuickstart : RecipeBase
    {
        public override string Name => "Circle";
        public override string Description => "A circle can be " +
            "placed on the plot and styled as desired.";

        [Test]
        public override void Execute()
        {
            // TODO: need a circle plot type
        }
    }

    public class EllipseQuickstart : RecipeBase
    {
        public override string Name => "Ellipse";
        public override string Description => "An ellipse can be " +
            "placed on the plot and styled as desired.";

        [Test]
        public override void Execute()
        {
            // TODO: need ellipse plot type
        }
    }

    public class PolygonQuickstart : RecipeBase
    {
        public override string Name => "Polygon Plot Quickstart";
        public override string Description => "Polygon plots can be added from a series of vertices, and must be in clockwise order.";

        [Test]
        public override void Execute()
        {
            Coordinates[] points =
            {
                new(0,   0.25),
                new(0.3, 0.75),
                new(1,   1),
                new(0.7, 0.5),
                new(1,   0)
            };

            myPlot.Add.Polygon(points);
        }
    }

    public class PolygonStyling : RecipeBase
    {
        public override string Name => "Polygon Plot Styling";
        public override string Description => "Polygon plots can be fully customized.";

        [Test]
        public override void Execute()
        {
            Coordinates[] points =
            {
                new (0, 0.25),
                new (0.3, 0.75),
                new (1, 1),
                new (0.7, 0.5),
                new (1, 0)
            };

            var poly = myPlot.Add.Polygon(points);

            poly.FillStyle = new FillStyle
            {
                Color = Colors.Green,
                HatchColor = Colors.Blue,
                Hatch = new Gradient()
                {
                    GradientType = GradientType.Linear,
                    AlignmentStart = Alignment.UpperRight,
                    AlignmentEnd = Alignment.LowerLeft,
                }
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
        }
    }
}
