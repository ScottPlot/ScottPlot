namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Radar : ICategory
{
    public string Chapter => "Plot Types";
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

            string[] labels = { "Wins", "Poles", "Podiums", "Points", "DNFs" };
            //radar.LabelSpokes(labels);
        }
    }

    public class RadarRadialTicks : RecipeBase
    {
        public override string Name => "Radar Radial Ticks and Labels";
        public override string Description => "Radial axis ticks and labels can be defined";

        [Test]
        public override void Execute()
        {
            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            var radar = myPlot.Add.Radar(values);

            string[] labels = { "Wins", "Poles", "Podiums", "Points", "DNFs" };
        }
    }
}
