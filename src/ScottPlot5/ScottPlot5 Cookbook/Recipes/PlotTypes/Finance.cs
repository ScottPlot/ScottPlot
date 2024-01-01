namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Finance : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Financial Plot";
    public string CategoryDescription => "Finance plots display price data binned into time ranges";

    public class OhlcChart : RecipeBase
    {
        public override string Name => "OHLC Chart";
        public override string Description => "OHLC charts use symbols to display price data " +
            "(open, high, low, and close) for specific time ranges.";

        [Test]
        public override void Execute()
        {
            var prices = Generate.RandomOHLCs(30);
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
            var prices = Generate.RandomOHLCs(30);
            myPlot.Add.Candlestick(prices);
            myPlot.AxisStyler.DateTimeTicks(Edge.Bottom);
        }
    }

    public class FinanceSma : RecipeBase
    {
        public override string Name => "Simple Moving Average";
        public override string Description => "Tools exist for creating simple moving average (SMA) " +
            "curves and displaying them next to finanial data.";

        [Test]
        public override void Execute()
        {
            // generate and plot time series price data
            var prices = Generate.RandomOHLCs(75);
            myPlot.Add.Candlestick(prices);
            myPlot.AxisStyler.DateTimeTicks(Edge.Bottom);

            // calculate SMA and display it as a scatter plot
            int[] windowSizes = { 3, 8, 20 };
            foreach (int windowSize in windowSizes)
            {
                ScottPlot.Finance.SimpleMovingAverage sma = new(prices, windowSize);
                var sp = myPlot.Add.Scatter(sma.Dates, sma.Means);
                sp.Label = $"SMA {windowSize}";
                sp.MarkerSize = 0;
                sp.LineWidth = 3;
                sp.Color = Colors.Navy.WithAlpha(1 - windowSize / 30.0);
            }

            myPlot.ShowLegend();
        }
    }

    public class FinanceBollinger : RecipeBase
    {
        public override string Name => "Bollinger Bands";
        public override string Description => "Tools exist for creating Bollinger Bands which " +
            "display weighted moving mean and variance for time series financial data.";

        [Test]
        public override void Execute()
        {
            // generate and plot time series price data
            var prices = Generate.RandomOHLCs(100);
            myPlot.Add.Candlestick(prices);
            myPlot.AxisStyler.DateTimeTicks(Edge.Bottom);

            // calculate Bollinger Bands
            ScottPlot.Finance.BollingerBands bb = new(prices, 20);

            // display center line (mean) as a solid line
            var sp1 = myPlot.Add.Scatter(bb.Dates, bb.Means);
            sp1.MarkerSize = 0;
            sp1.Color = Colors.Navy;

            // display upper bands (positive variance) as a dashed line
            var sp2 = myPlot.Add.Scatter(bb.Dates, bb.UpperValues);
            sp2.MarkerSize = 0;
            sp2.Color = Colors.Navy;
            sp2.LineStyle.Pattern = LinePattern.Dot;

            // display lower bands (positive variance) as a dashed line
            var sp3 = myPlot.Add.Scatter(bb.Dates, bb.LowerValues);
            sp3.MarkerSize = 0;
            sp3.Color = Colors.Navy;
            sp3.LineStyle.Pattern = LinePattern.Dot;
        }
    }
}
