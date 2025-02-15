namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Radar : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Radar Plot";
    public string CategoryDescription => "Radar charts (also called a spider charts or star charts) " +
        "represent multi-axis data as a 2D shape on axes arranged circularly around a center point.";

    public class RadarQuickstart : RecipeBase
    {
        public override string Name => "Radar Plot Quickstart";
        public override string Description => "A radar chart can be created from a single array of values.";

        [Test]
        public override void Execute()
        {
            double[] values = { 78, 83, 84, 76, 43 };
            myPlot.Add.Radar(values);
        }
    }

    public class RadarMultipleQuickstart : RecipeBase
    {
        public override string Name => "Radar Plot with Multiple Series";
        public override string Description => "A single radar chart can be used to display values from multiple series using a 2D array";

        [Test]
        public override void Execute()
        {
            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            myPlot.Add.Radar(values);
        }
    }

    public class RadarLegend : RecipeBase
    {
        public override string Name => "Radar Legend";
        public override string Description => "Collections of radar values (radar series) can be labeled so they appear in the legend";

        [Test]
        public override void Execute()
        {
            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            var radar = myPlot.Add.Radar(values);
            radar.Series[0].LegendText = "Sebastian";
            radar.Series[1].LegendText = "Fernando";
        }
    }

    public class RadarSeriesCustomization : RecipeBase
    {
        public override string Name => "Radar Series Customization";
        public override string Description => "Radar plots have a collection of RadarSeries objects " +
            "which each describe a set of values and the styling information used to display it as a shape on the radar plot. " +
            "Users may change properties of radar series objects to achieve a high level of customization over each shape.";

        [Test]
        public override void Execute()
        {
            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            var radar = myPlot.Add.Radar(values);

            radar.Series[0].FillColor = Colors.Transparent;
            radar.Series[0].LineColor = Colors.Blue;
            radar.Series[0].LineWidth = 3;
            radar.Series[0].LinePattern = LinePattern.DenselyDashed;

            radar.Series[1].FillColor = Colors.Green.WithAlpha(.2);
            radar.Series[1].LineColor = Colors.Green;
            radar.Series[1].LineWidth = 2;
        }
    }

    public class RadarSpokeLabels : RecipeBase
    {
        public override string Name => "Radar Spoke Labels";
        public override string Description => "Labels can be assigned to spokes " +
            "to label values around the circumference of the radar plot";

        [Test]
        public override void Execute()
        {
            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            var radar = myPlot.Add.Radar(values);

            string[] spokeLabels = { "Wins", "Poles", "Podiums", "Points", "DNFs" };
            radar.PolarAxis.SetSpokes(spokeLabels, length: 110);
        }
    }

    public class RadarRadialTicks : RecipeBase
    {
        public override string Name => "Radar Radial Tick Labels";
        public override string Description => "Radar radial tick positions and labels may be defined by the user";

        [Test]
        public override void Execute()
        {
            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            var radar = myPlot.Add.Radar(values);

            double[] tickPositions = { 25, 50, 75, 100 };
            string[] tickLabels = tickPositions.Select(x => x.ToString()).ToArray();
            radar.PolarAxis.SetCircles(tickPositions, tickLabels);
        }
    }

    public class RadarStraightLines : RecipeBase
    {
        public override string Name => "Radar with Straight Lines";
        public override string Description => "Radial ticks may be rendered using straight lines instead of circles";

        [Test]
        public override void Execute()
        {
            double[] values = { 78, 83, 100, 76, 43 };
            var radar = myPlot.Add.Radar(values);
            radar.PolarAxis.StraightLines = true;
        }
    }
}
