using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Finance
    {
        public class OHLC : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Financial OHLC Chart";
            public string description { get; } = "Display OHLC (open, high, low, close) data by plotting an array of OHLC objects.";

            public void Render(Plot plt)
            {
                ScottPlot.OHLC[] ohlcs = DataGen.RandomStockPrices(rand: null, pointCount: 60, deltaMinutes: 10);
                plt.Title("Open/High/Low/Close (OHLC) Chart");
                plt.YLabel("Stock Price (USD)");
                plt.PlotOHLC(ohlcs);
                plt.Ticks(dateTimeX: true);
            }
        }

        public class Candle : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Financial Candlestick Chart";
            public string description { get; } = "Display candlestick data by plotting an array of OHLC objects.";

            public void Render(Plot plt)
            {
                ScottPlot.OHLC[] ohlcs = DataGen.RandomStockPrices(rand: null, pointCount: 60, deltaMinutes: 10);
                plt.Title("Ten Minute Candlestick Chart");
                plt.YLabel("Stock Price (USD)");
                plt.PlotCandlestick(ohlcs);
                plt.Ticks(dateTimeX: true);
            }
        }

        public class CandleSkipWeekends : PlotDemo, IPlotDemo
        {
            public string name { get; } = "OHLC with gaps";
            public string description { get; } = "This example demonstrates that by default the horizontal axis is strictly linear. Missing OHLC data produces gaps in the plot.";

            public void Render(Plot plt)
            {
                ScottPlot.OHLC[] ohlcs = ScottPlot.DataGen.RandomStockPrices(rand: null, pointCount: 30, deltaDays: 1);

                plt.Title("Daily Candlestick Chart (weekends skipped)");
                plt.YLabel("Stock Price (USD)");
                plt.PlotCandlestick(ohlcs);
                plt.Ticks(dateTimeX: true);
            }
        }

        public class CandleNoSkippedDays : PlotDemo, IPlotDemo
        {
            public string name { get; } = "OHLC without gaps";
            public string description { get; } = "This example demonstrates how to plot OHLC data continuously even though there are gaps on the horizontal axis (for days the market is closed). The strategy is to plot it on a linear horizontal axis (not a DateTime axis) and then to come back later and define custom tick labels.";

            public void Render(Plot plt)
            {
                // start with stock prices which have unevenly spaced time points (weekends are skipped)
                ScottPlot.OHLC[] ohlcs = DataGen.RandomStockPrices(rand: null, pointCount: 30);

                // replace timestamps with a series of numbers starting at 0
                for (int i = 0; i < ohlcs.Length; i++)
                    ohlcs[i].time = i;

                // plot the candlesticks (the horizontal axis will start at 0)
                plt.Title("Daily Candlestick Chart (evenly spaced)");
                plt.YLabel("Stock Price (USD)");
                plt.PlotCandlestick(ohlcs);

                // create ticks manually
                double[] tickPositions = { 0, 6, 13, 20, 27 };
                string[] tickLabels = { "Sep 23", "Sep 30", "Oct 7", "Oct 14", "Oct 21" };
                plt.XTicks(tickPositions, tickLabels);
            }
        }

        public class SimpleMovingAverage : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Simple Moving Average (SMA)";
            public string description { get; } = "This example demonstrates how to plot OHLC data and display Simple Moving Average (SMA) lines on top.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                ScottPlot.OHLC[] ohlcs = DataGen.RandomStockPrices(rand, 75, sequential: true);
                double[] xs = DataGen.Consecutive(ohlcs.Length);
                double[] sma20 = Statistics.Finance.SMA(ohlcs, 8);
                double[] sma50 = Statistics.Finance.SMA(ohlcs, 20);

                plt.PlotCandlestick(ohlcs);
                plt.PlotScatter(xs, sma20, label: "8 day SMA",
                    color: Color.Blue, markerSize: 0, lineWidth: 2);
                plt.PlotScatter(xs, sma50, label: "20 day SMA",
                    color: Color.Navy, markerSize: 0, lineWidth: 2);

                // decorate the plot
                plt.Title("Simple Moving Averages (SMA)");
                plt.YLabel("Stock Price (USD)");
                plt.XLabel("Day");
                plt.Legend();
            }
        }

        public class Bollinger : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bollinger Bands";
            public string description { get; } = "This example demonstrates how to plot OHLC data and display Bollinger Bands on top.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                ScottPlot.OHLC[] ohlcs = DataGen.RandomStockPrices(rand, 100, sequential: true);
                double[] xs = DataGen.Consecutive(ohlcs.Length);
                (var sma, var bolL, var bolU) = ScottPlot.Statistics.Finance.Bollinger(ohlcs);

                plt.PlotCandlestick(ohlcs);
                plt.PlotScatter(xs, bolL, color: Color.Blue, markerSize: 0);
                plt.PlotScatter(xs, bolU, color: Color.Blue, markerSize: 0);
                plt.PlotScatter(xs, sma, color: Color.Blue, markerSize: 0, lineStyle: LineStyle.Dash);

                // decorate the plot
                plt.Title("Bollinger Bands");
                plt.YLabel("Stock Price (USD)");
                plt.XLabel("Day");
                plt.Legend();
            }
        }
    }
}
