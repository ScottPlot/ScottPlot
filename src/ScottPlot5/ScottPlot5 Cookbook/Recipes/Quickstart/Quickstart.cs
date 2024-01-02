namespace ScottPlotCookbook.Recipes.Quickstart;

public class Quickstart : ICategory
{
    public string Chapter => "Quickstart";
    public string CategoryName => "ScottPlot 5 Quickstart";
    public string CategoryDescription => "A survey of basic functionality in ScottPlot 5";

    public class QuickstartScatter : RecipeBase
    {
        public override string Name => "Scatter Plot";
        public override string Description => "Display paired X/Y data as a scatter plot.";

        [Test]
        public override void Execute()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            myPlot.Add.Scatter(dataX, dataY);
        }
    }

    public class CustomizingPlottables : RecipeBase
    {
        public override string Name => "Customizing Plottables";
        public override string Description => "Functions that add things to plots return the plottables they create. " +
            "Interact with the properties of plottables to customize their styling and behavior.";

        [Test]
        public override void Execute()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            var myScatter = myPlot.Add.Scatter(dataX, dataY);
            myScatter.LineStyle.Width = 5;
            myScatter.LineStyle.Color = Colors.Green.WithOpacity(.2);
            myScatter.MarkerStyle.Fill.Color = Colors.Magenta;
            myScatter.MarkerStyle.Size = 15;
        }
    }

    public class QuickstartSignal : RecipeBase
    {
        public override string Name => "Signal Plot";
        public override string Description => "Signal plots are optimized for displaying evenly spaced data.";

        [Test]
        public override void Execute()
        {
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);
            myPlot.Add.Signal(sin);
            myPlot.Add.Signal(cos);
        }
    }

    public class SignalPerformance : RecipeBase
    {
        public override string Name => "Signal Plot Performance";
        public override string Description => "Signal plots can interactively display millions of data points in real time. " +
            "Double-click the plot to display performance benchmarks.";

        [Test]
        public override void Execute()
        {
            double[] data = Generate.RandomWalk(1_000_000);
            myPlot.Add.Signal(data);
            myPlot.Title("Signal plot with one million points");
        }
    }

    public class QuickstartAxisLabels : RecipeBase
    {
        public override string Name => "Axis Labels";
        public override string Description => "Axis labels can be extensively customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Axes.Bottom.Label.Text = "Horizonal Axis";
            myPlot.Axes.Left.Label.Text = "Vertical Axis";
            myPlot.Axes.Left.Label.Text = "Plot Title";
        }
    }

    public class Legend : RecipeBase
    {
        public override string Name => "Legend";
        public override string Description => "A legend displays plottables in a key along the edge of a plot. " +
            "Most plottables have a Label property which configures what text appears in the legend.";

        [Test]
        public override void Execute()
        {
            var sig1 = myPlot.Add.Signal(Generate.Sin(51));
            sig1.Label = "Sin";

            var sig2 = myPlot.Add.Signal(Generate.Cos(51));
            sig2.Label = "Cos";

            myPlot.Legend.IsVisible = true;
        }
    }
}
