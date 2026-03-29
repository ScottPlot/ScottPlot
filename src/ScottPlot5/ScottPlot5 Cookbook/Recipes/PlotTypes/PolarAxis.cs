namespace ScottPlotCookbook.Recipes.PlotTypes;

public class PolarAxis : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Polar Axis";
    public string CategoryDescription => "Create a polar axis and add it to the plot to display data on a circular coordinate system.";

    public class PolarQuickStart : RecipeBase
    {
        public override string Name => "Polar Axis";
        public override string Description => "A polar axis can be added to the plot, " +
            "then other plot types (marker, line, scatter, etc.) can be placed on top of it " +
            "using its helper methods to translate polar coordinates to Cartesian units.";

        [Test]
        public override void Execute()
        {
            // add a polar axis to the plot
            var polarAxis = myPlot.Add.PolarAxis();

            IColormap colormap = new ScottPlot.Colormaps.Turbo();
            foreach (double degrees in ScottPlot.Generate.Range(0, 360, 10))
            {
                // use the polar axis to get X/Y coordinates given a position in polar space
                double radius = degrees / 360.0;
                Coordinates pt = polarAxis.GetCoordinates(radius, degrees);

                // place markers or other plot types using X/Y coordinates like normal
                var marker = myPlot.Add.Marker(pt);
                marker.Color = colormap.GetColor(radius);
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
            var polarAxis = myPlot.Add.PolarAxis();
            polarAxis.Rotation = Angle.FromDegrees(90); // default direction is counter-clockwise

            IColormap colormap = new ScottPlot.Colormaps.Turbo();
            foreach (double degrees in ScottPlot.Generate.Range(0, 360, 10))
            {
                double radius = degrees / 360.0;
                Coordinates pt = polarAxis.GetCoordinates(radius, degrees);

                var marker = myPlot.Add.Marker(pt);
                marker.Color = colormap.GetColor(radius);
            }
        }
    }

    public class PolarClockwise : RecipeBase
    {
        public override string Name => "Clockwise Polar Axis";
        public override string Description => "Clockwise polar plots are common for representing spatial orientation.";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis();
            polarAxis.Clockwise = true;
            polarAxis.Rotation = Angle.FromDegrees(-90); // direction will be counter-clockwise

            IColormap colormap = new ScottPlot.Colormaps.Turbo();
            foreach (double degrees in ScottPlot.Generate.Range(0, 360, 10))
            {
                double fraction = degrees / 360.0;
                Coordinates pt = polarAxis.GetCoordinates(fraction, degrees);
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

            var polarAxis = myPlot.Add.PolarAxis(radius: 30);
            polarAxis.Circles.ForEach(x => x.LinePattern = LinePattern.Dotted);
            polarAxis.Spokes.ForEach(x => x.LinePattern = LinePattern.Dotted);

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
            var polarAxis = myPlot.Add.PolarAxis();

            // style the spokes (straight lines extending from the center to mark rotations)
            var radialPalette = new ScottPlot.Palettes.Category10();
            for (int i = 0; i < polarAxis.Spokes.Count; i++)
            {
                polarAxis.Spokes[i].LineColor = radialPalette.GetColor(i).WithAlpha(.5);
                polarAxis.Spokes[i].LineWidth = 4;
                polarAxis.Spokes[i].LabelStyle.ForeColor = radialPalette.GetColor(i);
                polarAxis.Spokes[i].LabelStyle.FontSize = 16;
                polarAxis.Spokes[i].LabelStyle.Bold = true;
            }

            // style the circles (concentric circles marking radius positions)
            var circularColormap = new ScottPlot.Colormaps.Rain();
            for (int i = 0; i < polarAxis.Circles.Count; i++)
            {
                double fraction = (double)i / (polarAxis.Circles.Count - 1);
                polarAxis.Circles[i].LineColor = circularColormap.GetColor(fraction).WithAlpha(.5);
                polarAxis.Circles[i].LineWidth = 2;
                polarAxis.Circles[i].LinePattern = LinePattern.Dashed;
            }
        }
    }

    public class PolarAxisBackground : RecipeBase
    {
        public override string Name => "Polar Axis Background";
        public override string Description => "The background style of polar axes may be customized";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis();
            polarAxis.FillColor = Colors.Blue.WithAlpha(.2);
        }
    }

    public class PolarSpokeLength : RecipeBase
    {
        public override string Name => "Polar Axis Spoke Length";
        public override string Description => "The length of spokes may be customized. " +
            "Spoke length is expressed as a fraction of the polar axis radius.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.PolarAxis(spokeLength: 1.3);
        }
    }

    public class PolarSpokeLabels : RecipeBase
    {
        public override string Name => "Polar Axis Spoke Labels";
        public override string Description => "Polar axis spokes may be individually labeled.";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis();

            string[] labels = { "alpha", "beta", "gamma", "delta", "epsilon" };
            polarAxis.SetSpokes(labels, 1.1);
        }
    }

    public class PolarTickLabels : RecipeBase
    {
        public override string Name => "Polar Axis Tick Labels";
        public override string Description => "Polar axis ticks are marked by circles which may be individually labeled.";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis();
            polarAxis.Rotation = Angle.FromDegrees(-90);

            double[] ticksPositions = { 5, 10, 15, 20 };
            string[] tickLabels = { "A", "B", "C", "D" };
            polarAxis.SetCircles(ticksPositions, tickLabels);

            polarAxis.SetSpokes(count: 5, length: 22, degreeLabels: false);
        }
    }

    public class PolarAxisLineCustomizations : RecipeBase
    {
        public override string Name => "Polar Axis Line Customization";
        public override string Description => "The angle and length of spokes and " +
            "position of circles can be manually defined. Each spoke and circle " +
            "may also be individually styled.";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis();

            // define spoke angle and length
            polarAxis.Spokes.Clear();
            polarAxis.Spokes.Add(new(Angle.FromDegrees(0), 0.5));
            polarAxis.Spokes.Add(new(Angle.FromDegrees(45), 0.75));
            polarAxis.Spokes.Add(new(Angle.FromDegrees(90), 1.0));

            // define circle radius
            polarAxis.Circles.Clear();
            polarAxis.Circles.Add(new(0.5));
            polarAxis.Circles.Add(new(0.75));
            polarAxis.Circles.Add(new(1.0));

            // style individual spokes and circles
            ScottPlot.Palettes.Category10 pal = new();
            for (int i = 0; i < 3; i++)
            {
                polarAxis.Circles[i].LineColor = pal.GetColor(i).WithAlpha(.5);
                polarAxis.Spokes[i].LineColor = pal.GetColor(i).WithAlpha(.5);

                polarAxis.Circles[i].LineWidth = 2 + i * 2;
                polarAxis.Spokes[i].LineWidth = 2 + i * 2;
            }
        }
    }

    public class PolarRadar : RecipeBase
    {
        public override string Name => "Polar Radar Plot";
        public override string Description => "Combining a polar axis with polygons " +
            "is an alternative strategy for building radar plots.";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis();
            polarAxis.Clockwise = true;
            polarAxis.Rotation = Angle.FromDegrees(-90);

            // add labeled spokes
            string[] labels = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };
            polarAxis.SetSpokes(labels, length: 5.5);

            // add defined ticks
            double[] ticks = { 1, 2, 3, 4, 5 };
            polarAxis.SetCircles(ticks);

            // convert radar values to coordinates
            double[] values1 = { 5, 4, 3, 2, 3 };
            double[] values2 = { 2, 3, 2, 4, 2 };
            Coordinates[] cs1 = polarAxis.GetCoordinates(values1);
            Coordinates[] cs2 = polarAxis.GetCoordinates(values2);

            // add polygons for each dataset
            var poly1 = myPlot.Add.Polygon(cs1);
            poly1.FillColor = Colors.Green.WithAlpha(.5);
            poly1.LineColor = Colors.Black;

            var poly2 = myPlot.Add.Polygon(cs2);
            poly2.FillColor = Colors.Blue.WithAlpha(.5);
            poly2.LineColor = Colors.Black;
        }
    }

    public class PolarSpokeLabelPadding : RecipeBase
    {
        public override string Name => "Polar Spoke Label Padding";
        public override string Description => "Modifies the padding of labels on polar spokes.";

        [Test]
        public override void Execute()
        {
            var polarAxis = myPlot.Add.PolarAxis();
            polarAxis.SetSpokes(4, 1);

            for (int i = 0; i < polarAxis.Spokes.Count; i++)
            {
                polarAxis.Spokes[i].LineWidth = 4;
                polarAxis.Spokes[i].LabelStyle.FontSize = 16;
                polarAxis.Spokes[i].LabelPaddingFraction = 0.2 * i;
                polarAxis.Spokes[i].LabelText = $"{polarAxis.Spokes[i].LabelLength}";
            }
        }
    }

    public class PolarWithLines : RecipeBase
    {
        public override string Name => "Polar Axis with Lines";
        public override string Description => "This is an example of a polar axis with lines instead of points";

        [Test]
        public override void Execute()
        {
            // add a polar axis to the plot
            var polarAxis = myPlot.Add.PolarAxis(radius: 100);

            IColormap colormap = new ScottPlot.Colormaps.Turbo();
            Coordinates? previousPt = null;

            foreach (double fraction in ScottPlot.Generate.Range(0, 1, 0.02))
            {
                // use the polar axis to get X/Y coordinates given a position in polar space
                double radius = 100 * fraction;
                double degrees = 360 * fraction;
                Coordinates pt = polarAxis.GetCoordinates(radius, degrees);

                if (previousPt != null)
                {
                    ScottPlot.Plottables.LinePlot lp = myPlot.Add.Line(previousPt.Value.X, previousPt.Value.Y, pt.X, pt.Y);
                    lp.LineWidth = 5;
                    lp.Color = Colors.Red;
                    previousPt = pt;
                }
                else
                {
                    previousPt = pt;
                }
            }
        }
    }
}
