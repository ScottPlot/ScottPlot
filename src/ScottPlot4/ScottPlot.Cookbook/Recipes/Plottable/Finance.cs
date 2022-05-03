using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class FinanceQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string ID => "finance_quickstart";
        public string Title => "Candlestick Chart";
        public string Description =>
            "ScottPlot can draw some financial indicators on plots in X/Y space, but users looking to " +
            "develop robust financial charts should probably look at other libraries designed specifically " +
            "for financial charting. The biggest limitations are (1) lack of mouse interaction and (2) the " +
            "horizontal axis is strictly numeric Cartesian space and is not ideal for plotting dates. " +
            "That said, some financial charting is possible with ScottPlot, and this cookbook demonstrates " +
            "common use cases.";

        public void ExecuteRecipe(Plot plt)
        {
            // OHLCs are open, high, low, and closing prices for a time range.
            OHLC[] prices = DataGen.RandomStockPrices(null, 60);
            plt.AddCandlesticks(prices);
        }
    }

    public class FinanceOHLC : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string ID => "finance_ohlc";
        public string Title => "OHLC Chart";
        public string Description =>
            "OHLC charts are an alternative to candlestick charts. They show high and low prices as a vertical line, " +
            "and indicate open and close prices with little ticks to the left and to the right.";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] prices = DataGen.RandomStockPrices(null, 60);
            plt.AddOHLCs(prices);
        }
    }

    public class FinanceDate : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string Title => "Using a DateTime Axis";
        public string ID => "finance_dateTimeAxis";
        public string Description =>
            "You probably never want to do this... but OHLCs have an X value you can customize to be a DateTime " +
            "(converted to a double using DateTime.ToOATime()). " +
            "The advantage is that you can use the native DateTime axis support on the horizontal axis. " +
            "The disadvantage is that gaps in time appear as gaps in the candlesticks. " +
            "Weekends without trading will appear as gaps. The alternative to this method is to " +
            "plot a series of OHLCs using sequential numbers, then manually define the axis tick labels.";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] prices = DataGen.RandomStockPrices(null, 60, TimeSpan.FromDays(1));

            // add the OHLCs to the plot and the horizontal axis to display DateTime tick labels
            plt.AddCandlesticks(prices);
            plt.XAxis.DateTimeFormat(true);
        }
    }

    public class FinanceCustomLabels : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string Title => "Candlesticks with Custom Tick Labels";
        public string ID => "finance_tickLabels";
        public string Description =>
            "A better way to represent time on the horizontal axis is to use traditional Cartesian coordinates " +
            "so each candlestick is placed at X positions (0, 1, 2, etc.), then manually define the locations and " +
            "label text of important positions on the plot. This is clunky, but possible. This inelegance is why " +
            "financial charting is probably best done with real financial charting libraries, " +
            "not a scientific charting library like ScottPlot...";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] prices = DataGen.RandomStockPrices(null, 30);
            plt.AddCandlesticks(prices);

            // manually indicate where axis ticks should be and what their labels should say
            double[] tickPositions = { 0, 6, 13, 20, 27 };
            string[] tickLabels = { "Sep 23", "Sep 30", "Oct 7", "Oct 14", "Oct 21" };
            plt.XTicks(tickPositions, tickLabels);
        }
    }

    public class FinanceSMA : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string Title => "Simple Moving Average (SMA)";
        public string ID => "finance_sma";
        public string Description =>
            "A simple moving average (SMA) technical indicator can be calculated and drawn as a scatter plot.";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] ohlcs = DataGen.RandomStockPrices(null, 75);
            var candlePlot = plt.AddCandlesticks(ohlcs);

            var sma8 = candlePlot.GetSMA(8);
            plt.AddScatterLines(sma8.xs, sma8.ys, Color.Blue, 2);

            var sma20 = candlePlot.GetSMA(20);
            plt.AddScatterLines(sma20.xs, sma20.ys, Color.Navy, 2);
        }
    }

    public class FinanceBollinger : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string Title => "Bollinger Bands";
        public string ID => "finance_bollinger";
        public string Description =>
            "Bollinger bands are a common technical indicator that show the average +/- two times the standard deviation " +
            "of a given time range preceeding it.";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] ohlcs = DataGen.RandomStockPrices(null, 100);
            var candlePlot = plt.AddCandlesticks(ohlcs);

            var bol = candlePlot.GetBollingerBands(20);
            plt.AddScatterLines(bol.xs, bol.sma, Color.Blue);
            plt.AddScatterLines(bol.xs, bol.lower, Color.Blue, lineStyle: LineStyle.Dash);
            plt.AddScatterLines(bol.xs, bol.upper, Color.Blue, lineStyle: LineStyle.Dash);
        }
    }

    public class FinanceRightScale : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string ID => "finance_right";
        public string Title => "Price on Right";
        public string Description =>
            "Newer data appears on the ride side of the chart so financial charts are often " +
            "displayed with the vertical axis label on the right side as well. This is possible " +
            "by disabling the left vertical axis (YAxis) and enabling the right one (YAxis2). " +
            "The left and right Y axes are index 0 and 1 (respectively), and the plottable has to be " +
            "update to indicate which axis index it should render on.";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] prices = DataGen.RandomStockPrices(null, 30, TimeSpan.FromMinutes(5));
            var candlePlot = plt.AddCandlesticks(prices);
            candlePlot.YAxisIndex = 1;
            plt.XAxis.DateTimeFormat(true);

            plt.YAxis.Ticks(false);
            plt.YAxis2.Ticks(true);
            plt.YAxis2.Label("Price (USD)");
        }
    }

    public class FinanceWickColor : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string ID => "finance_wick";
        public string Title => "Custom Wick Color";
        public string Description =>
            "By default candle wicks are the same color as their bodies, but this can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] prices = DataGen.RandomStockPrices(null, 30, TimeSpan.FromMinutes(5));
            var fp = plt.AddCandlesticks(prices);
            fp.WickColor = Color.Black;
        }
    }

    public class FinanceColor : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string ID => "finance_color";
        public string Title => "Custom Colors";
        public string Description =>
            "Candles that close below their open price are colored differently from " +
            "candles which close at or above it. These colors can be customized. " +
            "Combine this styling with a custom wick color (which also controls the candle border) " +
            "to create a different visual style.";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] prices = DataGen.RandomStockPrices(null, 30, TimeSpan.FromMinutes(5));
            var fp = plt.AddCandlesticks(prices);
            fp.ColorDown = Color.Black;
            fp.ColorUp = Color.White;
            fp.WickColor = Color.Black;
        }
    }

    public class FinanceDark : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Finance();
        public string ID => "finance_dark";
        public string Title => "Dark Mode";
        public string Description =>
            "A dark mode finance plot can be realized by customizing color options of the candles and figure. " +
            "Colors in this example were chosen to mimic TC2000.";

        public void ExecuteRecipe(Plot plt)
        {
            // add some random candles
            OHLC[] prices = DataGen.RandomStockPrices(null, 100, TimeSpan.FromMinutes(5));
            double[] xs = prices.Select(x => x.DateTime.ToOADate()).ToArray();
            var candlePlot = plt.AddCandlesticks(prices);
            candlePlot.YAxisIndex = 1;

            plt.XAxis.DateTimeFormat(true);

            // add SMA indicators for 8 and 20 days
            var sma8 = candlePlot.GetSMA(8);
            var sma20 = candlePlot.GetSMA(20);
            var sma8plot = plt.AddScatterLines(sma8.xs, sma8.ys, Color.Cyan, 2, label: "8 day SMA");
            var sma20plot = plt.AddScatterLines(sma20.xs, sma20.ys, Color.Yellow, 2, label: "20 day SMA");
            sma8plot.YAxisIndex = 1;
            sma20plot.YAxisIndex = 1;

            // customize candle styling
            candlePlot.ColorDown = ColorTranslator.FromHtml("#00FF00");
            candlePlot.ColorUp = ColorTranslator.FromHtml("#FF0000");

            // customize figure styling
            plt.Layout(padding: 12);
            plt.Style(figureBackground: Color.Black, dataBackground: Color.Black);
            plt.Frameless();
            plt.XAxis.TickLabelStyle(color: Color.White);
            plt.XAxis.TickMarkColor(ColorTranslator.FromHtml("#333333"));
            plt.XAxis.MajorGrid(color: ColorTranslator.FromHtml("#333333"));

            // hide the left axis and show a right axis
            plt.YAxis.Ticks(false);
            plt.YAxis.Grid(false);
            plt.YAxis2.Ticks(true);
            plt.YAxis2.Grid(true);
            plt.YAxis2.TickLabelStyle(color: ColorTranslator.FromHtml("#00FF00"));
            plt.YAxis2.TickMarkColor(ColorTranslator.FromHtml("#333333"));
            plt.YAxis2.MajorGrid(color: ColorTranslator.FromHtml("#333333"));

            // customize the legend style
            var legend = plt.Legend();
            legend.FillColor = Color.Transparent;
            legend.OutlineColor = Color.Transparent;
            legend.Font.Color = Color.White;
            legend.Font.Bold = true;
        }
    }
}
