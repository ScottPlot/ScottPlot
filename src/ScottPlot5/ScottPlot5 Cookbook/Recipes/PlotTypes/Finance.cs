using System.Drawing;

namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Finance : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Financial Plot";
    public string CategoryDescription => "Finance plots display price data binned into time ranges";

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
            myPlot.Axes.DateTimeTicksBottom();
        }
    }

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
            myPlot.Axes.DateTimeTicksBottom();
        }
    }

    public class FinanceChartBins : RecipeBase
    {
        public override string Name => "Finance Chart with Custom Time Bins";
        public override string Description => "Finance charts can display price " +
            "range information over arbitrary time scales.";

        [Test]
        public override void Execute()
        {
            DateTime timeOpen = new(1985, 09, 24, 9, 30, 0); // 9:30 AM
            DateTime timeClose = new(1985, 09, 24, 16, 0, 0); // 4:00 PM
            TimeSpan timeSpan = TimeSpan.FromMinutes(10); // 10 minute bins

            List<OHLC> prices = new();
            for (DateTime dt = timeOpen; dt <= timeClose; dt += timeSpan)
            {
                double open = Generate.RandomNumber(20, 40) + prices.Count;
                double close = Generate.RandomNumber(20, 40) + prices.Count;
                double high = Math.Max(open, close) + Generate.RandomNumber(5);
                double low = Math.Min(open, close) - Generate.RandomNumber(5);
                prices.Add(new OHLC(open, high, low, close, dt, timeSpan));
            }

            myPlot.Add.Candlestick(prices);
            myPlot.Axes.DateTimeTicksBottom();
        }
    }

    public class FinanceRightAxis : RecipeBase
    {
        public override string Name => "Price on Right";
        public override string Description => "Finance charts can be created " +
            "which display price information on the right axis.";

        [Test]
        public override void Execute()
        {
            // add candlesticks to the plot
            var prices = Generate.RandomOHLCs(30);
            var candles = myPlot.Add.Candlestick(prices);

            // configure the candlesticks to use the plot's right axis
            candles.Axes.YAxis = myPlot.Axes.Right;
            candles.Axes.YAxis.Label.Text = "Price";

            // style the bottom axis to display date
            myPlot.Axes.DateTimeTicksBottom();
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
            myPlot.Axes.DateTimeTicksBottom();

            // calculate SMA and display it as a scatter plot
            int[] windowSizes = { 3, 8, 20 };
            foreach (int windowSize in windowSizes)
            {
                ScottPlot.Finance.SimpleMovingAverage sma = new(prices, windowSize);
                var sp = myPlot.Add.Scatter(sma.Dates, sma.Means);
                sp.LegendText = $"SMA {windowSize}";
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
            myPlot.Axes.DateTimeTicksBottom();

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
            sp2.LinePattern = LinePattern.Dotted;

            // display lower bands (positive variance) as a dashed line
            var sp3 = myPlot.Add.Scatter(bb.Dates, bb.LowerValues);
            sp3.MarkerSize = 0;
            sp3.Color = Colors.Navy;
            sp3.LinePattern = LinePattern.Dotted;
        }
    }

    public class FinancialPlotWithoutGaps : RecipeBase
    {
        public override string Name => "Candlestick Chart Without Gaps";
        public override string Description => "When the DateTimes stored in OHLC objects " +
            "are used to determine the horizontal position of candlesticks, periods without data " +
            "like weekends and holidays appear as gaps in the plot. Enabling sequential mode causes " +
            "the plot to ignore the OHLC DateTimes and display candles at integer positions starting " +
            "from zero. Users can customize the tick generator to display dates instead of numbers " +
            "on the horizontal axis if desired.";

        [Test]
        public override void Execute()
        {
            // create a candlestick plot
            var prices = Generate.RandomOHLCs(31);
            var candlePlot = myPlot.Add.Candlestick(prices);

            // enable sequential mode to place candles at X = 0, 1, 2, ...
            candlePlot.Sequential = true;

            // determine a few candles to display ticks for
            int tickCount = 5;
            int tickDelta = prices.Count / tickCount;
            DateTime[] tickDates = prices
                .Where((x, i) => i % tickDelta == 0)
                .Select(x => x.DateTime)
                .ToArray();

            // By default, horizontal tick labels will be numbers (1, 2, 3...)
            // We can use a manual tick generator to display dates on the horizontal axis
            double[] tickPositions = Generate.Consecutive(tickDates.Length, tickDelta);
            string[] tickLabels = tickDates.Select(x => x.ToString("MM/dd")).ToArray();
            ScottPlot.TickGenerators.NumericManual tickGen = new(tickPositions, tickLabels);
            myPlot.Axes.Bottom.TickGenerator = tickGen;
        }
    }

    public class FinancialPlotWithoutGapsOhlc : RecipeBase
    {
        public override string Name => "OHLC Chart Without Gaps";
        public override string Description => "When the DateTimes stored in OHLC objects " +
            "are used to determine the horizontal position, periods without data " +
            "like weekends and holidays appear as gaps in the plot. Enabling sequential mode causes " +
            "the plot to ignore the OHLC DateTimes and place OHLCs at integer positions starting " +
            "from zero. Users can customize the tick generator to display dates instead of numbers " +
            "on the horizontal axis if desired.";

        [Test]
        public override void Execute()
        {
            // create a OHLC plot
            var prices = Generate.RandomOHLCs(31);
            var ohlcPlot = myPlot.Add.OHLC(prices);

            // enable sequential mode to place OHLCs at X = 0, 1, 2, ...
            ohlcPlot.Sequential = true;

            // determine a few OHLCs to display ticks for
            int tickCount = 5;
            int tickDelta = prices.Count / tickCount;
            DateTime[] tickDates = prices
                .Where((x, i) => i % tickDelta == 0)
                .Select(x => x.DateTime)
                .ToArray();

            // By default, horizontal tick labels will be numbers (1, 2, 3...)
            // We can use a manual tick generator to display dates on the horizontal axis
            double[] tickPositions = Generate.Consecutive(tickDates.Length, tickDelta);
            string[] tickLabels = tickDates.Select(x => x.ToString("MM/dd")).ToArray();
            ScottPlot.TickGenerators.NumericManual tickGen = new(tickPositions, tickLabels);
            myPlot.Axes.Bottom.TickGenerator = tickGen;
        }
    }

    public class StockSymbolBackgroundText : RecipeBase
    {
        public override string Name => "Stock Symbol Background";
        public override string Description => "Stock symbol information can be displayed " +
            "beneath the plot using the background text feature.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Candlestick(Generate.RandomOHLCs(30));
            myPlot.Axes.DateTimeTicksBottom();

            var line1 = myPlot.Add.BackgroundText("DANK");
            line1.LabelFontColor = Colors.Gray.WithAlpha(.4);
            line1.LabelFontSize = 96;
            line1.LabelBold = true;
        }
    }

    public class StockSymbolBackgroundTextMultiline : RecipeBase
    {
        public override string Name => "Stock Symbol Multiline";
        public override string Description => "Stock symbol information can be displayed " +
            "beneath the plot using the multiline background text feature.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Candlestick(Generate.RandomOHLCs(30));
            myPlot.Axes.DateTimeTicksBottom();

            (var line1, var line2) = myPlot.Add.BackgroundText("DANK", "Highest Recommendation by Reddit");

            line1.LabelFontColor = Colors.Gray.WithAlpha(.4);
            line1.LabelFontSize = 64;
            line1.LabelBold = true;

            line2.LabelFontColor = Colors.Gray.WithAlpha(.4);
            line2.LabelFontSize = 18;
            line2.LabelBold = false;
        }
    }

    public class FinanceDarkMode : RecipeBase
    {
        public override string Name => "Finance Chart Dark Mode";
        public override string Description => "A dark mode finance plot can be achieved " +
            "by customizing color options of the candles and figure.";

        [Test]
        public override void Execute()
        {
            // add sample financial data
            OHLC[] prices = Generate.Financial.OHLCsByMinute(60);
            var candlePlot = myPlot.Add.Candlestick(prices);
            candlePlot.Axes.YAxis = myPlot.Axes.Right;

            // setup DateTime ticks on the bottom
            myPlot.Axes.DateTimeTicksBottom();

            // use currency tick formatting on the right
            myPlot.Axes.Right.TickGenerator = new ScottPlot.TickGenerators.NumericAutomatic()
            {
                LabelFormatter = (double value) => value.ToString("C")
            };

            // customize candle styling
            candlePlot.RisingColor = ScottPlot.Color.FromHtml("#FF0000");
            candlePlot.FallingColor = ScottPlot.Color.FromHtml("#00FF00");

            // add SMA indicators
            int[] windowSizes = { 3, 8, 20 };
            foreach (int windowSize in windowSizes)
            {
                ScottPlot.Finance.SimpleMovingAverage sma = new(prices, windowSize);
                var sp = myPlot.Add.Scatter(sma.Dates, sma.Means);
                sp.Axes.YAxis = myPlot.Axes.Right;
                sp.MarkerSize = 0;
                sp.LineWidth = 1.5f;
                sp.LinePattern = LinePattern.DenselyDashed;
                sp.Color = Colors.Yellow.WithAlpha(1 - windowSize / 30.0);
            }

            // add symbol information and push it to the back of the plot
            (var line1, var line2) = myPlot.Add.BackgroundText("DANK", "Recommended by Reddit");

            line1.LabelFontColor = Colors.Gray.WithAlpha(.4);
            line1.LabelFontSize = 72;
            line1.LabelBold = true;
            line1.Axes.YAxis = myPlot.Axes.Right;

            line2.LabelFontColor = Colors.Gray.WithAlpha(.4);
            line2.LabelFontSize = 24;
            line2.LabelBold = false;
            line2.Axes.YAxis = myPlot.Axes.Right;

            // customize miscellaneous plot component colors
            myPlot.FigureBackground.Color = Colors.Black;
            myPlot.DataBackground.Color = Colors.Black;
            myPlot.Axes.Color(ScottPlot.Color.FromHtml("#999999"));
            myPlot.Axes.Right.MajorTickStyle.Color = Colors.Transparent;
            myPlot.Axes.Right.MinorTickStyle.Color = Colors.Transparent;
            myPlot.Axes.Bottom.MajorTickStyle.Color = Colors.Transparent;
            myPlot.Axes.FrameWidth(0);
            myPlot.Grid.MajorLineColor = ScottPlot.Color.FromHtml("#222222");
            myPlot.Grid.YAxis = myPlot.Axes.Right;
        }
    }

    public class FinancialDateTimeAxis : RecipeBase
    {
        public override string Name => "Financial DateTime Axis";
        public override string Description => "A special axis system has been created for financial charts. " +
            "Unlike standard DateTime axes which assume the horizontal scale is linearly spaced time, the financial " +
            "DateTime system allows for dates to be skipped. This is ideal for financial charts where date ranges are " +
            "skipped such as after-hours trading or non-trading days.";

        [Test]
        public override void Execute()
        {
            // generate sample data using a collection of dates and price ranges
            DateTime[] dates = Generate.ConsecutiveHours(100);
            List<OHLC> candles = Generate.RandomOHLCs(30);
            var candlestickPlot = myPlot.Add.Candlestick(candles);

            // enable sequential mode so candles are placed 1 unit apart (0, 1, 2, etc.)
            candlestickPlot.Sequential = true;

            // disable the default tick generator (and grid) and make space for the new one
            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();
            myPlot.Axes.Bottom.MinimumSize = 30;

            // add the financial DateTime tick generator
            ScottPlot.Plottables.FinancialTimeAxis financeAxis = new(dates);
            myPlot.Add.Plottable(financeAxis);
        }
    }
}
