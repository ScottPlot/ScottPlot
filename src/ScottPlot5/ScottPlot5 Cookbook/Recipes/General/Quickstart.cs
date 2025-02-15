namespace ScottPlotCookbook.Recipes.Quickstart;

public class ScottPlotQuickstart : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Quickstart";
    public string CategoryDescription => "A survey of basic functionality in ScottPlot 5";

    public class QuickstartScatter : RecipeBase
    {
        public override string Name => "Scatter Plot";
        public override string Description => "A scatter plot can be used to display X/Y data points.";

        [Test]
        public override void Execute()
        {
            // create sample data
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };

            // add a scatter plot to the plot
            myPlot.Add.Scatter(dataX, dataY);
        }
    }

    public class CustomizingPlottables : RecipeBase
    {
        public override string Name => "Customizing Plottables";
        public override string Description => "Most methods which add items to plots return the item that was added. " +
            "Save the object that is returned and set its properties to customize it.";

        [Test]
        public override void Execute()
        {
            // create sample data
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };

            // add a scatter plot to the plot (and save what is returned)
            var myScatter = myPlot.Add.Scatter(dataX, dataY);

            // customize the scatter plot
            myScatter.Color = Colors.Green;
            myScatter.LineWidth = 5;
            myScatter.MarkerSize = 15;
            myScatter.MarkerShape = MarkerShape.FilledDiamond;
            myScatter.LinePattern = LinePattern.DenselyDashed;
        }
    }

    public class QuickstartSignal : RecipeBase
    {
        public override string Name => "Signal Plot";
        public override string Description => "Signal plots display Y values at evenly spaced X positions. " +
            "Signal plots should be used instead of Scatter plots whenever possible.";

        [Test]
        public override void Execute()
        {
            // create sample data
            double[] sin = Generate.Sin(51);

            // add a signal plot to the plot
            myPlot.Add.Signal(sin);
        }
    }

    public class SignalPerformance : RecipeBase
    {
        public override string Name => "Signal Plot Performance";
        public override string Description => "Signal plots are so performant that they can " +
            "interactively display millions of data points in real time.";

        [Test]
        public override void Execute()
        {
            // create sample data with one million points
            double[] data = Generate.RandomWalk(1_000_000);

            // add a signal plot to the plot
            myPlot.Add.Signal(data);
        }
    }

    public class QuickstartAxisLabels : RecipeBase
    {
        public override string Name => "Axis Labels";
        public override string Description => "Plots have helper methods for quickly setting axis labels. " +
            "Refer to other cookbook pages for additional axis customization options.";

        [Test]
        public override void Execute()
        {
            // plot sample data
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // customize axis labels
            myPlot.XLabel("Horizonal Axis");
            myPlot.YLabel("Vertical Axis");
            myPlot.Title("Plot Title");
        }
    }
}
