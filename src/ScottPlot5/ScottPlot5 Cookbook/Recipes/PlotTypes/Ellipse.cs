namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Ellipse : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Ellipse";
    public string CategoryDescription => "Ellipses are curves with a defined center and distinct X and Y radii. " +
        "A circle is an ellipse with an X radius equal to its Y radius.";

    public class PlotEllipseQuickstart : RecipeBase
    {
        public override string Name => "Ellipse Quickstart";
        public override string Description => "Ellipses can be added to plots";

        [Test]
        public override void Execute()
        {
            Random rand = new(0);
            for (int i = 0; i < 5; i++)
            {
                myPlot.Add.Ellipse(
                    xCenter: rand.Next(-10, 10),
                    yCenter: rand.Next(-10, 10),
                    radiusX: rand.Next(1, 7),
                    radiusY: rand.Next(1, 7));
            }
        }
    }

    public class PlotCircleQuickstart : RecipeBase
    {
        public override string Name => "Circle Quickstart";
        public override string Description => "Circles can be added to plots. " +
            "Circles are really Ellipses with the same X and Y radius. " +
            "Note that circles appear as ellipses unless the plot has a square coordinate system.";

        [Test]
        public override void Execute()
        {
            Random rand = new(0);
            for (int i = 0; i < 5; i++)
            {
                myPlot.Add.Circle(
                    xCenter: rand.Next(-10, 10),
                    yCenter: rand.Next(-10, 10),
                    radius: rand.Next(1, 7));
            }
        }
    }

    public class CircleWithLockedScale : RecipeBase
    {
        public override string Name => "Circle with Locked Scale";
        public override string Description => "For circles to always appear circular, " +
            "the coordinate system must be forced to always display square-shaped pixels. " +
            "This can be achieved by enabling the axis scale lock.";

        [Test]
        public override void Execute()
        {
            Random rand = new(0);
            for (int i = 0; i < 5; i++)
            {
                myPlot.Add.Circle(
                    xCenter: rand.Next(-10, 10),
                    yCenter: rand.Next(-10, 10),
                    radius: rand.Next(1, 7));
            }

            // force pixels to have a 1:1 scale ratio
            myPlot.Axes.SquareUnits();
        }
    }

    public class EllipseStyling : RecipeBase
    {
        public override string Name => "Ellipse Styling";
        public override string Description => "Ellipses styles can be extensively customized";

        [Test]
        public override void Execute()
        {
            var circle = myPlot.Add.Circle(center: Coordinates.Origin, radius: 5);
            circle.LineStyle.Width = 5;
            circle.LineStyle.Pattern = LinePattern.Dashed;
            circle.LineStyle.Color = Colors.Green;
            circle.FillStyle.Color = Colors.Navy;
            circle.FillStyle.HatchColor = Colors.Red;
            circle.FillStyle.Hatch = new ScottPlot.Hatches.Striped();

            myPlot.Axes.SetLimits(-10, 10, -10, 10);
        }
    }

    public class EllipseRotation : RecipeBase
    {
        public override string Name => "Ellipse Rotation";
        public override string Description => "Ellipses can also be rotated";

        [Test]
        public override void Execute()
        {
            int count = 5;
            for (int i = 0; i < count; i++)
            {
                var el = myPlot.Add.Ellipse(
                    center: Coordinates.Origin,
                    radiusX: 1,
                    radiusY: 5);

                el.Rotation = i * 180.0 / count;
            }

            // force pixels to have a 1:1 scale ratio
            ScottPlot.AxisRules.SquareZoomOut rule = new(myPlot.Axes.Bottom, myPlot.Axes.Left);
            myPlot.Axes.Rules.Add(rule);
        }
    }
}
