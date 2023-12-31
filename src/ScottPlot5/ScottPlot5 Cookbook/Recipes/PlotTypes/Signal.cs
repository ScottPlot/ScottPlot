namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Signal : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Signal Plot";
    public string CategoryDescription => "Signal plots display evenly-spaced data";

    public class SignalQuickstart : RecipeBase
    {
        public override string Name => "Signal Plot Quickstart";
        public override string Description => "Signal plots are best for extremely large datasets. " +
            "They use render using special optimizations that allow highspeed interactivity " +
            "with plots containing millions of data points.";

        [Test]
        public override void Execute()
        {
            double[] values = Generate.RandomWalk(1_000_000);

            myPlot.Add.Signal(values);

            myPlot.Title("Signal Plot with 1 Million Points");
        }
    }

    public class SignalStyling : RecipeBase
    {
        public override string Name => "Signal Plot Styling";
        public override string Description => "Signal plots can be styled in a variety of ways.";

        [Test]
        public override void Execute()
        {
            var sig1 = myPlot.Add.Signal(Generate.Sin());
            sig1.Color = Colors.Magenta;
            sig1.LineWidth = 10;
            sig1.Label = "Sine";

            var sig2 = myPlot.Add.Signal(Generate.Cos());
            sig2.Color = Colors.Green;
            sig2.LineWidth = 5;
            sig2.Label = "Cosine";

            myPlot.ShowLegend();
        }
    }

    public class SignalOffset : RecipeBase
    {
        public override string Name => "Signal Offset";
        public override string Description => "Signal plots can be offset by a given X and Y value.";

        [Test]
        public override void Execute()
        {
            double[] values = ScottPlot.Generate.Sin(51);

            var sig1 = myPlot.Add.Signal(values);
            sig1.Label = "Default";

            var sig2 = myPlot.Add.Signal(values);
            sig2.Data.XOffset = 10;
            sig2.Data.YOffset = .25;
            sig2.Label = "Offset";

            myPlot.Legend.IsVisible = true;
        }
    }

    public class SignalMarkerSize : RecipeBase
    {
        public override string Name => "Signal Marker Size";
        public override string Description => "Signal plots can have markers displayed at each point " +
            "which are only visible when the plot is zoomed in.";

        [Test]
        public override void Execute()
        {
            var sig1 = myPlot.Add.Signal(Generate.Cos());
            sig1.Label = "Default";
            sig1.Data.YOffset = 3;

            var sig2 = myPlot.Add.Signal(Generate.Cos());
            sig2.Label = "Large Markers";
            sig2.MaximumMarkerSize = 20;
            sig2.Data.YOffset = 2;

            var sig3 = myPlot.Add.Signal(Generate.Cos());
            sig3.Label = "Hidden Markers";
            sig3.MaximumMarkerSize = 0;
            sig3.Data.YOffset = 1;

            myPlot.Legend.IsVisible = true;
        }
    }
}
