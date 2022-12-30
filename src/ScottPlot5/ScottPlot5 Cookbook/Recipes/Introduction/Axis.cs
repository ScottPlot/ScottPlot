namespace ScottPlotCookbook.Recipes.Introduction;

internal class Axis : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Customization,
        PageName = "Axis and Ticks",
        PageDescription = "Examples of common customizations for axis labels and ticks",
    };

    internal class AxisLabels : RecipeTestBase
    {
        public override string Name => "Customize Axis Labels";
        public override string Description => "Axis labels are the text labels centered on each axis. " +
            "The text inside these labels can be changed, and the style of the text can be extensively customized.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.XAxis.Label.Text = "Horizontal Axis";
            myPlot.YAxis.Label.Text = "Vertical Axis";
        }
    }

    internal class TickLabels : RecipeTestBase
    {
        public override string Name => "Customize Tick Labels";
        public override string Description => "The labels shown by major ticks can be customized.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            //myPlot.XAxis.TickFont
        }
    }
}
