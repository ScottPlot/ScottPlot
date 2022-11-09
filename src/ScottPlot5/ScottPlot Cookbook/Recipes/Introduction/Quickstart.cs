namespace ScottPlotCookbook.Recipes.Introduction;

internal class Quickstart : RecipePage
{
    public override Chapter Chapter => Chapter.Introduction;
    public override string PageName => "ScottPlot 5 Quickstart";
    public override string PageDescription => "A survey of basic functionality in ScottPlot 5";

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
}
