namespace ScottPlotCookbook.Recipes.Introduction;

internal class Quickstart : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Introduction,
        PageName = "ScottPlot 5 Quickstart",
        PageDescription = "A survey of basic functionality in ScottPlot 5",
    };

    internal class Scatter : RecipeTestBase
    {
        public override string Name => "Scatter Plot";
        public override string Description => "Display paired X/Y data as a scatter plot.";

        [Test]
        public override void Recipe()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            myPlot.Add.Scatter(dataX, dataY);
        }
    }

    internal class CustomizingPlottables : RecipeTestBase
    {
        public override string Name => "Customizing Plottables";
        public override string Description => "Functions that add things to plots return the plottables they create. " +
            "Interact with the properties of plottables to customize their styling and behavior";

        [Test]
        public override void Recipe()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            var myScatter = myPlot.Add.Scatter(dataX, dataY);
            myScatter.LineWidth = 5;
            myScatter.LineColor = Colors.Green.WithOpacity(.2);
            myScatter.MarkerColor = Colors.Magenta;
            myScatter.MarkerSize = 15;
        }
    }

    internal class Signal : RecipeTestBase
    {
        public override string Name => "Signal Plot";
        public override string Description => "Signal plots are optimized for displaying evenly spaced data";

        [Test]
        public override void Recipe()
        {
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);
            myPlot.Add.Signal(sin);
            myPlot.Add.Signal(cos);
        }
    }

    internal class SignalPerformance : RecipeTestBase
    {
        public override string Name => "Signal Plot Performance";
        public override string Description => "Signal plots can interactively display millions of data points in real time. " +
            "Double-click the plot to display performance benchmarks.";

        [Test]
        public override void Recipe()
        {
            double[] data = Generate.RandomWalk(1_000_000);
            myPlot.Add.Signal(data);
            myPlot.Title.Label.Text = $"Signal plot with one million points";
        }
    }

    internal class AxisLabels : RecipeTestBase
    {
        public override string Name => "Axis Labels";
        public override string Description => "Axis labels can be extensively customized";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.XAxis.Label.Text = "Horizonal Axis";
            myPlot.YAxis.Label.Text = "Vertical Axis";
            myPlot.Title.Label.Text = "Plot Title";
        }
    }

    internal class Legend : RecipeTestBase
    {
        public override string Name => "Legend";
        public override string Description => "A legend displays plottables in a key along the edge of a plot. " +
            "Most plottables have a Label property which configures what text appears in the legend.";

        [Test]
        public override void Recipe()
        {
            var sig1 = myPlot.Add.Signal(Generate.Sin(51));
            sig1.Label = "Sin";

            var sig2 = myPlot.Add.Signal(Generate.Cos(51));
            sig2.Label = "Cos";
        }
    }

    internal class ManualLegend : RecipeTestBase
    {
        public override string Name => "Manual Legend";
        public override string Description => "Legends may be constructed manually.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            LegendItem item1 = new()
            {
                Label = "alpha",
                Line = new ScottPlot.Style.Stroke(Colors.Magenta, 2),
            };

            LegendItem item2 = new()
            {
                Label = "beta",
                Line = new ScottPlot.Style.Stroke(Colors.Green, 4),
            };

            var legend = myPlot.GetLegend();
            legend.ManualLegendItems = new[] { item1, item2 };
        }
    }
}
