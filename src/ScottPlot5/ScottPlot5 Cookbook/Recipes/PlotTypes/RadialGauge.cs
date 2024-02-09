namespace ScottPlotCookbook.Recipes.PlotTypes;

public class RadialGauge : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Radial gauge";
    public string CategoryDescription => "A radial gauge chart displays scalar data as circular gauges.";

    public class RadialGaugeQuickstart : RecipeBase
    {
        public override string Name => "Radial gauge from values";
        public override string Description => "A radial gauge chart can be created from a few values.";

        [Test]
        public override void Execute()
        {
            double[] values = { 100, 80, 65, 45, 20 };
            var radialGauglePlot = myPlot.Add.RadialGaugePlot(values);
        }
    }
}
