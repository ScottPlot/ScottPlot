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
            MyPlot.Add.Scatter(dataX, dataY);
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
            var myScatter = MyPlot.Add.Scatter(dataX, dataY);
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
            MyPlot.Add.Signal(sin);
            MyPlot.Add.Signal(cos);
        }
    }

    internal class AxisLabels : RecipeTestBase
    {
        public override string Name => "Axis Labels";
        public override string Description => "Axis labels can be extensively customized";

        [Test]
        public override void Recipe()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            MyPlot.XAxis.Label.Text = "Horizonal Axis";
            MyPlot.YAxis.Label.Text = "Vertical Axis";
        }
    }
}
