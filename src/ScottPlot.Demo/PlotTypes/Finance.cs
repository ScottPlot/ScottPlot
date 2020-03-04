using System;
using System.Collections.Generic;
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
            public string description { get; } = "Display OHLC (open, high, low, close) data by plotting an array of OHLC objects.";

            public void Render(Plot plt)
            {
                ScottPlot.OHLC[] ohlcs = DataGen.RandomStockPrices(rand: null, pointCount: 60, deltaMinutes: 10);
                plt.Title("Ten Minute Candlestick Chart");
                plt.YLabel("Stock Price (USD)");
                plt.PlotCandlestick(ohlcs);
                plt.Ticks(dateTimeX: true);
            }
        }
    }
}
