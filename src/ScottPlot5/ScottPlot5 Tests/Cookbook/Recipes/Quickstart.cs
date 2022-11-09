namespace ScottPlot_Tests.Cookbook.Recipes;

internal class QuickstartScatter : RecipeBase
{
    public override string Title => "ScottPlot 5 Quickstart";
    public override string Description => "A minimal example showing how to plot data using ScottPlot 5";
    public override RecipeCategory Category => RecipeCategories.Quickstart;

    [Test]
    public override void Recipe()
    {
        double[] dataX = new double[] { 1, 2, 3, 4, 5 };
        double[] dataY = new double[] { 1, 4, 9, 16, 25 };
        myPlot.Add.Scatter(dataX, dataY);
    }
}

internal class QuickstartAxisLabels : RecipeBase
{
    public override string Title => "Axis Labels";
    public override string Description => "Axis labels are the text labels centered on each axis. " +
        "The text inside these labels can be changed, and the style of the text can be extensively customized.";
    public override RecipeCategory Category => RecipeCategories.Quickstart;

    [Test]
    public override void Recipe()
    {
        double[] dataX = new double[] { 1, 2, 3, 4, 5 };
        double[] dataY = new double[] { 1, 4, 9, 16, 25 };
        myPlot.Add.Scatter(dataX, dataY);

        myPlot.XAxis.Label.Text = "Yep, this works";
        myPlot.YAxis.Label.Text = "This works too!";
    }
}
