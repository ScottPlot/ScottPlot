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
            var c1 = myPlot.Add.Circle(1, 0, .5);
            var c2 = myPlot.Add.Circle(2, 0, .5);
            var c3 = myPlot.Add.Circle(3, 0, .5);

            c1.FillStyle.Color = Colors.Blue;
            c2.FillStyle.Color = Colors.Blue.Darken(.75);
            c3.FillStyle.Color = Colors.Blue.Lighten(.75);

            c1.LineWidth = 0;
            c2.LineWidth = 0;
            c3.LineWidth = 0;

            // force circles to remain circles
            ScottPlot.AxisRules.SquareZoomOut squareRule = new(myPlot.Axes.Bottom, myPlot.Axes.Left);
            myPlot.Axes.Rules.Add(squareRule);
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
            for (int i = 0; i < 10; i++)
            {
                var el = myPlot.Add.Ellipse(0, 0, 1, 10, rotation: i * 10);
                double fraction = i / 10.0;
                el.LineColor = Colors.Blue.WithAlpha(fraction);
            }

            // force circles to remain circles
            ScottPlot.AxisRules.SquareZoomOut squareRule = new(myPlot.Axes.Bottom, myPlot.Axes.Left);
            myPlot.Axes.Rules.Add(squareRule);
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
            poly.FillColor = Colors.Green;
            poly.FillHatchColor = Colors.Blue;
            poly.FillHatch = new Gradient()
            {
                GradientType = GradientType.Linear,
                AlignmentStart = Alignment.UpperRight,
                AlignmentEnd = Alignment.LowerLeft,
            };

            poly.LineColor = Colors.Black;
            poly.LinePattern = LinePattern.Dashed;
            poly.LineWidth = 2;

            poly.MarkerShape = MarkerShape.OpenCircle;
            poly.MarkerSize = 8;
            poly.MarkerFillColor = Colors.Gold;
            poly.MarkerLineColor = Colors.Brown;
        }
    }
}
