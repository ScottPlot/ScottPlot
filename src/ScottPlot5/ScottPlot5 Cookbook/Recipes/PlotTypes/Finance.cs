namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Finance : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Financial Plot",
        PageDescription = "Finance plots display OHLC data representing prices " +
        "(open, high, low, close) for specific time ranges",
    };

    internal class Candlestick : RecipeTestBase
    {
        public override string Name => "Candlestick Chart";
        public override string Description => "A candlestick chart can be created from OHLC data.";

        [Test]
        public override void Recipe()
        {
            OHLC[] ohlcs = ScottPlot.Generate.OHLC.Random(30);
            DateTime[] dates = ScottPlot.Generate.DateTime.Weekdays(ohlcs.Length);
            double[] xs = dates.Select(x => x.ToOADate()).ToArray(); // TODO: there must be a better way
            myPlot.Add.OHLC(xs, ohlcs);
            myPlot.Axes.DateTimeTicks(Edge.Bottom);
        }
    }
}
