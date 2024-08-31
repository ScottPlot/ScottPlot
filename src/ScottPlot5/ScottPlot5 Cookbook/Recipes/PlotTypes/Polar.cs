namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Polar : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Polar Axis";
    public string CategoryDescription => "Create a polar axis and add it to the plot to display data on a circular coordinate system.";

    public class PolarQuickStart : RecipeBase
    {
        public override string Name => "Polar Axis";
        public override string Description => "A polar axis can be added to the plot, " +
            "then other plot types (marker, line, scatter, etc.) can be placed on top of it " +
            "using ints helper methods to translate polar coordinates to Cartesian units.";

        [Test]
        public override void Execute()
        {
            // add a polar axis to the plot
            var polarAxis = myPlot.Add.PolarAxis(maximumRadius: 100);

            IColormap colormap = new ScottPlot.Colormaps.Turbo();
            foreach (double fraction in ScottPlot.Generate.Range(0, 1, 0.02))
            {
                // use the polar axis to get X/Y coordinates given a position in polar space
                double radius = polarAxis.MaximumRadius * fraction;
                double degrees = 360 * fraction;
                Coordinates pt = polarAxis.GetCoordinates(radius, degrees);

                // place markers or other plot types using X/Y coordinates like normal
                var marker = myPlot.Add.Marker(pt);
                marker.Color = colormap.GetColor(fraction);
            }
        }
    }

    public class PolarRotation : RecipeBase
    {
        public override string Name => "Polar Axis Rotation";
        public override string Description => "A polar axis may be rotated to define the angle of the 0 degree spoke.";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis(maximumRadius: 100);
            polarAxis.RotationDegrees = -90;

            IColormap colormap = new ScottPlot.Colormaps.Turbo();
            foreach (double fraction in ScottPlot.Generate.Range(0, 1, 0.02))
            {
                double radius = polarAxis.MaximumRadius * fraction;
                double degrees = 360 * fraction;
                Coordinates pt = polarAxis.GetCoordinates(radius, degrees);
                var marker = myPlot.Add.Marker(pt);
                marker.Color = colormap.GetColor(fraction);
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
