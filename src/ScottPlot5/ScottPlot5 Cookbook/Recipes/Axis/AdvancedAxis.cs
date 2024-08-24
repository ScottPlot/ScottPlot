namespace ScottPlotCookbook.Recipes.Axis;

public class AdvancedAxis : ICategory
{
    public string Chapter => "Axis";
    public string CategoryName => "Advanced Axis Features";
    public string CategoryDescription => "How to further customize axes";

    public class InvertedAxis : RecipeBase
    {
        public override string Name => "Inverted Axis";
        public override string Description => "Users can display data on an inverted axis " +
            "by setting axis limits setting the lower edge to a value more positive than the upper edge.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Axes.SetLimitsY(bottom: 1.5, top: -1.5);
        }
    }

    public class InvertedAutoAxis : RecipeBase
    {
        public override string Name => "Inverted Auto-Axis";
        public override string Description => "Customize the logic for the " +
            "automatic axis scaler to ensure that axis limits " +
            "for a particular axis are always inverted when autoscaled.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Axes.AutoScaler.InvertedY = true;
        }
    }

    public class SquareAxisUnits : RecipeBase
    {
        public override string Name => "Square Axis Units";
        public override string Description => "Axis rules can be put in place which " +
            "force the vertical scale (units per pixel) to match the horizontal scale " +
            "so circles always appear as circles and not stretched ellipses.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Circle(0, 0, 10);

            // force pixels to have a 1:1 scale ratio
            myPlot.Axes.SquareUnits();

            // even if you try to "stretch" the axis, it will adjust the axis limits automatically
            myPlot.Axes.SetLimits(-10, 10, -20, 20);
        }
    }

    public class ExperimentalAxisWithSubtitle : RecipeBase
    {
        public override string Name => "Axis with Subtitle";

        public override string Description => "Users can create their own fully custom " +
            "axes to replace the default ones (as demonstrated in the demo app). " +
            "Some experimental axes are available for users who may be interested in " +
            "alternative axis display styles.";

        [Test]
        public override void Execute()
        {
            // Plot some sample data
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // Instantiate a custom axis and customize it as desired
            ScottPlot.AxisPanels.Experimental.LeftAxisWithSubtitle customAxisY = new()
            {
                LabelText = "My Custom Y Axis",
                SubLabelText = "It comes with a subtitle for the axis"
            };

            // Remove the default Y axis and add the custom one to the plot
            myPlot.Axes.Remove(myPlot.Axes.Left);
            myPlot.Axes.AddLeftAxis(customAxisY);
        }
    }

    public class AxisAntiAliasing : RecipeBase
    {
        public override string Name => "Axis AntiAliasing";
        public override string Description => "To improve crispness of straight vertical and horizontal lines, " +
            "Anti-aliasing is disabled by default for axis frames, tick marks, and grid lines. Anti-aliasing " +
            "can be enabled for all these objects by calling the AntiAlias helper method.";

        [Test]
        public override void Execute()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            myPlot.Add.Scatter(dataX, dataY);

            myPlot.Axes.AntiAlias(true);
        }
    }

    public class PolarQuickStart : RecipeBase
    {
        public override string Name => "Polar Axis";
        public override string Description => "A polar axis can be added to the plot, " +
            "then other plot types (marker, line, scatter, etc.) can be placed on top of it " +
            "using ints helper methods to translate polar coordinates to Cartesian units.";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis(maximumRadius: 100);

            for (int i = 0; i < 10; i++)
            {
                double radius = Generate.RandomNumber(100);
                double degrees = Generate.RandomNumber(360);
                Coordinates pt = polarAxis.GetCoordinates(radius, degrees);
                myPlot.Add.Marker(pt);
            }
        }
    }

    public class PolarAxisArrow : RecipeBase
    {
        public override string Name => "Polar Axis with Arrows";
        public override string Description => "Arrows can be placed on a polar coordinate system " +
            "with their base at the center and their tips used to indicate points in polar space. " +
            "The Phaser plot type uses this strategy to display collections of similarly styled arrows.";

        [Test]
        public override void Execute()
        {
            PolarCoordinates[] points = [
                new(10, Angle.FromDegrees(15)),
                new(20, Angle.FromDegrees(120)),
                new(30, Angle.FromDegrees(240)),
            ];

            var polarAxis = myPlot.Add.PolarAxis(30);
            polarAxis.LinePattern = LinePattern.Dotted;

            IPalette palette = new ScottPlot.Palettes.Category10();
            Coordinates center = polarAxis.GetCoordinates(0, 0);
            for (int i = 0; i < points.Length; i++)
            {
                Coordinates tip = polarAxis.GetCoordinates(points[i]);
                var arrow = myPlot.Add.Arrow(center, tip);
                arrow.ArrowLineWidth = 0;
                arrow.ArrowFillColor = palette.GetColor(i).WithAlpha(.7);
            }
        }
    }

    public class PolarAxisStyling : RecipeBase
    {
        public override string Name => "Polar Axis Styling";
        public override string Description => "The lines of polar axes may be extensively styled. " +
            "Polar axes have radial spokes (straight lines that extend from the origin to the maximum radius) " +
            "and circular axis lines (concentric circles centered at the origin).";

        [Test]
        public override void Execute()
        {
            var pol = myPlot.Add.PolarAxis();

            // style the spokes (straight lines extending from the center to mark rotations)
            var radialPalette = new ScottPlot.Palettes.Category10();
            for (int i = 0; i < pol.Spokes.Count; i++)
            {
                pol.Spokes[i].LineColor = radialPalette.GetColor(i).WithAlpha(.5);
                pol.Spokes[i].LineWidth = 4;
                pol.Spokes[i].LinePattern = LinePattern.DenselyDashed;
            }

            // style the circles (concentric circles marking radius positions)
            var circularColormap = new ScottPlot.Colormaps.Rain();
            for (int i = 0; i < pol.Circles.Count; i++)
            {
                double fraction = (double)i / (pol.Circles.Count - 1);
                pol.Circles[i].LineColor = circularColormap.GetColor(fraction).WithAlpha(.5);
                pol.Circles[i].LineWidth = 2;
                pol.Circles[i].LinePattern = LinePattern.Dashed;
            }
        }
    }

    public class PolarAxisLineDensity : RecipeBase
    {
        public override string Name => "Polar Line Density";
        public override string Description => "Density of spokes and circles on polar axes can " +
            "be customized using arguments passed into the functions that generate them.";

        [Test]
        public override void Execute()
        {
            var pol = myPlot.Add.PolarAxis();
            pol.RegenerateCircles(count: 10);
            pol.RegenerateSpokes(count: 4);
        }
    }

    public class PolarAxisLinePositions : RecipeBase
    {
        public override string Name => "Polar Line Positions";
        public override string Description => "The angle and length of spokes and " +
            "position of circles can be manually defined. Each spoke and circle " +
            "may also be individually styled.";

        [Test]
        public override void Execute()
        {
            var pol = myPlot.Add.PolarAxis();

            // define spoke angle and length
            pol.Spokes.Clear();
            pol.Spokes.Add(new(Angle.FromDegrees(0), 0.5));
            pol.Spokes.Add(new(Angle.FromDegrees(45), 0.75));
            pol.Spokes.Add(new(Angle.FromDegrees(90), 1.0));

            // define circle radius
            pol.Circles.Clear();
            pol.Circles.Add(new(0.5));
            pol.Circles.Add(new(0.75));
            pol.Circles.Add(new(1.0));

            // style individual spokes and circles
            ScottPlot.Palettes.Category10 pal = new();
            for (int i = 0; i < 3; i++)
            {
                pol.Circles[i].LineColor = pal.GetColor(i).WithAlpha(.5);
                pol.Spokes[i].LineColor = pal.GetColor(i).WithAlpha(.5);

                pol.Circles[i].LineWidth = 2 + i * 2;
                pol.Spokes[i].LineWidth = 2 + i * 2;
            }
        }
    }
}
