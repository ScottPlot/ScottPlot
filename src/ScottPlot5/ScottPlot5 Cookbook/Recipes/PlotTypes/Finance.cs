namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Finance : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Financial Plot";
    public string CategoryDescription => "Finance plots display price data binned into time ranges";

    public class OHLC : RecipeBase
    {
        public override string Name => "OHLC Chart";
        public override string Description => "OHLC charts use symbols to display price data " +
            "(open, high, low, and close) for specific time ranges.";

        [Test]
        public override void Execute()
        {
            ScottPlot.RandomDataGenerator gen = new(0);
            var prices = gen.RandomOHLCs(30);
            myPlot.Add.OHLC(prices);
            myPlot.AxisStyler.DateTimeTicks(Edge.Bottom);
        }
    }

    public class Candlestick : RecipeBase
    {
        public override string Name => "Candlestick Chart";
        public override string Description => "Candlestick charts use symbols to display price data. " +
            "The rectangle indicates open and close prices, and the center line indicates minimum and " +
            "maximum price for the given time period. Color indicates whether the price increased or decreased " +
            "between open and close.";

        [Test]
        public override void Execute()
        {
            ScottPlot.RandomDataGenerator gen = new(0);
            var prices = gen.RandomOHLCs(30);
            myPlot.Add.Candlestick(prices);
            myPlot.AxisStyler.DateTimeTicks(Edge.Bottom);
        }
    }
}
