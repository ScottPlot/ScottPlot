namespace ScottPlotCookbook.Recipes.PlotTypes;

public class RadialGauge : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Radial gauge";
    public string CategoryDescription => "A radial gauge chart displays scalar data as circular gauges.";

    public class RadialGaugeQuickstart : RecipeBase
    {
        public override string Name => "Radial gauge from values";
        public override string Description => "A radial gauge chart can be created from a few values.";

        [Test]
        public override void Execute()
        {
            double[] values = { 100, 80, 65, 45, 20 };
            myPlot.Add.RadialGaugePlot(values);
        }
    }

    public class RadialGaugeColormap : RecipeBase
    {
        public override string Name => "Gauge Colors";
        public override string Description => "Gauge colors can be customized by changing the default palette.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };
            myPlot.Add.RadialGaugePlot(values);
        }
    }

    public class RadialGaugeNegative : RecipeBase
    {
        public override string Name => "Negative Values";
        public override string Description => "Radial gauge plots support positive and negative values.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, -65, 45, -20 };
            myPlot.Add.RadialGaugePlot(values);
        }
    }

    public class RadialGaugeSequential : RecipeBase
    {
        public override string Name => "Sequential Gauge Mode";
        public override string Description =>
            "Sequential gauge mode indicates that the base of each gauge starts " +
            "at the tip of the previous gauge.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 50 };
            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.GaugeMode = ScottPlot.RadialGaugeMode.Sequential;
        }
    }

    public class RadialGaugeReverse : RecipeBase
    {
        public override string Name => "Reverse Order";
        public override string Description =>
            "Gauges are displayed from the center outward by default but the order can be customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 50 };
            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.GaugeMode = ScottPlot.RadialGaugeMode.Sequential;
            radialGaugePlot.OrderInsideOut = false;
        }
    }

    public class RadialGaugeModeSingle : RecipeBase
    {
        public override string Name => "Single Gauge Mode";
        public override string Description =>
            "The SingleGauge mode draws all gauges stacked together as a single gauge. " +
            "This is useful for showing a progress gauges composed of many individual smaller gauges.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            radialGaugePlot.MaximumAngle = 180;
            radialGaugePlot.StartingAngle = 180;
        }
    }

    public class RadialGaugeDirection : RecipeBase
    {
        public override string Name => "Gauge Direction";
        public override string Description =>
            "The direction of gauges can be customized. Clockwise is used by default.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.Clockwise = false;
        }
    }

    public class RadialGaugeSize : RecipeBase
    {
        public override string Name => "Gauge Size";
        public override string Description =>
            "The empty space between gauges can be adjusted as a fraction of their width. ";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };
            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.SpaceFraction = .05;
        }
    }

    public class RadialGaugeStart : RecipeBase
    {
        public override string Name => "Gauge Starting Angle";
        public override string Description =>
            "The starting angle for gauges can be customized. " +
            "270 for North (default value), 0 for East, 90 for South, 180 for West, etc.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.StartingAngle = 180;
        }
    }

    public class RadialGaugeRange : RecipeBase
    {
        public override string Name => "Gauge Angular Range";
        public override string Description =>
            "By default gauges are full circles (360 degrees) but smaller gauges can be created " +
            "by customizing the gauge size.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.MaximumAngle = 180;
        }
    }

    public class RadialGaugeLabels : RecipeBase
    {
        public override string Name => "Show Levels";
        public override string Description =>
            "The value of each gauge is displayed as text by default but this behavior can be overridden. " +
            "Note that this is different than the labels fiels which is what appears in the legened.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.ShowLevels = false;
        }
    }

    public class RadialGaugeLabelPos : RecipeBase
    {
        public override string Name => "Gauge Label Position";
        public override string Description =>
            "Gauge level text is positioned at the tip of each gauge by default, " +
            "but this position can be adjusted by the user.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.LabelPositionFraction = 0.5;
        }
    }

    public class RadialGaugeLabelFontPct : RecipeBase
    {
        public override string Name => "Gauge Label Font Percentage";
        public override string Description =>
            "Size of the gauge level text as a fraction of the gauge width.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.FontSizeFraction = .4;
        }
    }

    public class RadialGaugeLabelColor : RecipeBase
    {
        public override string Name => "Gauge Label Color";
        public override string Description =>
            "Level text fonts may be customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.Font.Color = Colors.Black;
        }
    }

    public class RadialGaugeLegend : RecipeBase
    {
        public override string Name => "Gauge Labels in Legend";
        public override string Description =>
            "Radial gauge labels will appear in the legend if they are assigned. ";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.Labels = new string[] { "alpha", "beta", "gamma", "delta", "epsilon" };
            myPlot.ShowLegend();
        }
    }

    public class RadialGaugeBackDim : RecipeBase
    {
        public override string Name => "Background Gauges Dim";
        public override string Description =>
            "By default the full range of each gauge is drawn as a semitransparent ring. " +
            "The amount of transparency can be adjusted as desired.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.BackgroundTransparencyFraction = .5;
        }
    }

    public class RadialGaugeBackNorm : RecipeBase
    {
        public override string Name => "Background Gauges Normalization";
        public override string Description =>
            "Gauge backgrounds are drawn as full circles by default. " +
            "This behavior can be disabled to draw partial backgrounds for non-circular gauges.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();
            double[] values = { 100, 80, 65, 45, 20 };

            var radialGaugePlot = myPlot.Add.RadialGaugePlot(values);
            radialGaugePlot.CircularBackground = false;
            radialGaugePlot.MaximumAngle = 180;
            radialGaugePlot.StartingAngle = 180;
        }
    }
}
