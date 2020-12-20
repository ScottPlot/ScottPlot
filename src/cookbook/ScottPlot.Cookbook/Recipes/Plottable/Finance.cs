using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class FinanceQuickstart : IRecipe
    {
        public string Category => "Plottable: Finance";
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
        public string Category => "Plottable: Finance";
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
        public string Category => "Plottable: Finance";
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
        public string Category => "Plottable: Finance";
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
        public string Category => "Plottable: Finance";
        public string Title => "Simple Moving Average (SMA)";
        public string ID => "finance_sma";
        public string Description =>
            "A simple moving average (SMA) technical indicator can be calculated and drawn as a scatter plot.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate sample stock prices
            OHLC[] ohlcs = DataGen.RandomStockPrices(null, 75);
            double[] xs = DataGen.Consecutive(ohlcs.Length);

            // calculate SMAs of different durations using helper methods
            double[] sma8xs = xs.Skip(8).ToArray();
            double[] sma8ys = Statistics.Finance.SMA(ohlcs, 8);
            double[] sma20xs = xs.Skip(20).ToArray();
            double[] sma20ys = Statistics.Finance.SMA(ohlcs, 20);

            // plot technical indicators as scatter plots above the financial chart
            plt.AddCandlesticks(ohlcs);
            plt.AddScatter(sma8xs, sma8ys, markerSize: 0, color: Color.Blue, lineWidth: 2);
            plt.AddScatter(sma20xs, sma20ys, markerSize: 0, color: Color.Navy, lineWidth: 2);
        }
    }

    public class FinanceBollinger : IRecipe
    {
        public string Category => "Plottable: Finance";
        public string Title => "Bollinger Bands";
        public string ID => "finance_bollinger";
        public string Description =>
            "Bollinger bands are a common technical indicator that show the average +/- two times the standard deviation " +
            "of a given time range preceeding it.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate sample stock prices
            OHLC[] ohlcs = DataGen.RandomStockPrices(null, 100);
            double[] xs = DataGen.Consecutive(ohlcs.Length);

            // calculate the bands and the time range they cover
            double[] xs2 = xs.Skip(20).ToArray();
            (var sma, var bolL, var bolU) = Statistics.Finance.Bollinger(ohlcs, 20);

            // plot technical indicators as scatter plots above the financial chart
            plt.AddCandlesticks(ohlcs);
            plt.AddScatter(xs2, sma, markerSize: 0, color: Color.Blue);
            plt.AddScatter(xs2, bolL, markerSize: 0, color: Color.Blue, lineStyle: LineStyle.Dash);
            plt.AddScatter(xs2, bolU, markerSize: 0, color: Color.Blue, lineStyle: LineStyle.Dash);
        }
    }

    public class FinanceRightScale : IRecipe
    {
        public string Category => "Plottable: Finance";
        public string ID => "finance_right";
        public string Title => "Price on Right";
        public string Description =>
            "Newer data appears on the ride side of the chart so financial charts are often " +
            "displayed with the vertical axis label on the right side as well. This is possible " +
            "by disabling the left vertical axis (YAxis) and enabling the right one (YAxis2)";

        public void ExecuteRecipe(Plot plt)
        {
            OHLC[] prices = DataGen.RandomStockPrices(null, 30, TimeSpan.FromMinutes(5));
            plt.AddCandlesticks(prices);
            plt.XAxis.DateTimeFormat(true);

            plt.YAxis.Ticks(false);
            plt.YAxis2.Ticks(true);
            plt.YAxis2.Label("Price (USD)");
        }
    }
}
