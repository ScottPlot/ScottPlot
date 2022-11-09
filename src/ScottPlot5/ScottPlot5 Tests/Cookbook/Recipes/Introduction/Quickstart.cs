namespace ScottPlot_Tests.Cookbook.Recipes.Introduction;

internal class QuickstartScatter : RecipeTestBase
{
    public override string Title => "ScottPlot 5 Quickstart";
    public override string Description => "A minimal example showing how to plot data using ScottPlot 5";
    public override Category Category => Categories.Quickstart;

    [Test]
    public override void Recipe()
    {
        double[] dataX = new double[] { 1, 2, 3, 4, 5 };
        double[] dataY = new double[] { 1, 4, 9, 16, 25 };
        MyPlot.Add.Scatter(dataX, dataY);
    }
}

internal class QuickstartAxisLabels : RecipeTestBase
{
    public override string Title => "Axis Labels";
    public override string Description => "Axis labels are the text labels centered on each axis. " +
        "The text inside these labels can be changed, and the style of the text can be extensively customized.";
    public override Category Category => Categories.Quickstart;

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
