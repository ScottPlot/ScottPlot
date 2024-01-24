namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Ellipse : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Ellipse";
    public string CategoryDescription => "Ellipses are curves with a defined center and distinct X and Y radii. A circle is an ellipse with an X radius equal to its Y radius.";

    public class EllipseQuickstart : RecipeBase
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

            myPlot.SavePng("ellipse_quickstart.png", 400, 300);
        }
    }

    public class CircleQuickstart : RecipeBase
    {
        public override string Name => "Circle Quickstart";
        public override string Description => "Circles can be added to plots. Circles are really Ellipses with the same X and Y radius. Note that circles appear as ellipses unless the plot has a square coordinate system.";

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

            myPlot.SavePng("circle_quickstart.png", 400, 300);
        }
    }

    public class CircleWithLockedScale : RecipeBase
    {
        public override string Name => "Circle with Locked Scale";
        public override string Description => "For circles to always appear circular, the coordinate system must be forced to always display square-shaped pixels. This can be achieved by enabling the axis scale lock.";

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

            // this forces pixels to have 1:1 scale ratio
            myPlot.Axes.Rules.Add(new ScottPlot.AxisRules.SquareZoomOut(myPlot.Axes.Bottom, myPlot.Axes.Left));

            myPlot.SavePng("circle_square_pixel.png", 400, 300);
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

            myPlot.SavePng("ellipse_styling.png", 400, 300);
        }
    }

    public class EllipseRotation : RecipeBase
    {
        public override string Name => "Ellipse Rotation";
        public override string Description => "Ellipses can also be rotated";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                var el = myPlot.Add.Ellipse(center: Coordinates.Origin, radiusX: 1, radiusY: 5);
                el.Rotation = i * 36;
            }

            myPlot.Axes.Rules.Add(new ScottPlot.AxisRules.SquareZoomOut(myPlot.Axes.Bottom, myPlot.Axes.Left));

            myPlot.SavePng("ellipse_rotation.png", 400, 300);
        }
    }
}
