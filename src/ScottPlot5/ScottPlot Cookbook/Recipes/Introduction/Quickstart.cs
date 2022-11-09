namespace ScottPlotCookbook.Recipes.Introduction;

internal class Quickstart : RecipePage
{
    internal override string PageName => "ScottPlot 5 Quickstart";
    internal override string PageDescription => "A survey of basic functionality in ScottPlot 5";
    internal override RecipeChapter Chapter => RecipeChapters.Introduction;

    internal class Scatter : RecipeTestBase
    {
        public override string Name => "Plot Data";
        public override string Description => "Display paired X/Y data as a scatter plot.";

        [Test]
        public override void Recipe()
        {
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            MyPlot.Add.Scatter(dataX, dataY);
        }
    }

    internal class AxisLabels : RecipeTestBase
    {
        public override string Name => "Customize Axis Labels";
        public override string Description => "Axis labels are the text labels centered on each axis. " +
            "The text inside these labels can be changed, and the style of the text can be extensively customized.";

        [Test]
        public override void Recipe()
        {
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            MyPlot.Add.Scatter(dataX, dataY);

            MyPlot.XAxis.Label.Text = "Yep, this works";
            MyPlot.YAxis.Label.Text = "This works too!";
        }
    }
}
