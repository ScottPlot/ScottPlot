namespace ScottPlotCookbook.Recipes.Introduction;

internal class Axis : RecipePage
{
    public override Chapter Chapter => Chapter.Customization;
    public override string PageName => "Axis and Ticks";
    public override string PageDescription => "Examples of common customizations for axis labels and ticks";

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
