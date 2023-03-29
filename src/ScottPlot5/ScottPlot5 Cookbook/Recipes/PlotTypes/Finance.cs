namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Finance : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Financial Plot",
        PageDescription = "Finance plots display price data binned into time ranges",
    };

    internal class OHLC : RecipeTestBase
    {
        public override string Name => "OHLC Chart";
        public override string Description => "OHLC charts use symbols to display price data " +
            "(open, high, low, and close) for specific time ranges.";

        [Test]
        public override void Recipe()
        {
            ScottPlot.RandomDataGenerator gen = new(0);
            var prices = gen.RandomOHLCs(30);
            myPlot.Add.OHLC(prices);
            myPlot.Axes.DateTimeTicks(Edge.Bottom);
        }
    }

    internal class Candlestick : RecipeTestBase
    {
        public override string Name => "Candlestick Chart";
        public override string Description => "Candlestick charts use symbols to display price data. " +
            "The rectangle indicates open and close prices, and the center line indicates minimum and " +
            "maximum price for the given time period. Color indicates whether the price increased or decreased " +
            "between open and close.";

        [Test]
        public override void Recipe()
        {
            ScottPlot.RandomDataGenerator gen = new(0);
            var prices = gen.RandomOHLCs(30);
            myPlot.Add.Candlestick(prices);
            myPlot.Axes.DateTimeTicks(Edge.Bottom);
        }
    }
}
