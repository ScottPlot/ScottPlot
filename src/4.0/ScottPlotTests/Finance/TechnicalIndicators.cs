using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Finance
{
    class TechnicalIndicators
    {
        [Test]
        public void Test_SMA_DoubleArray()
        {
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(150);
            double[] prices = DataGen.RandomWalk(rand, xs.Length, offset: 50);
            double[] sma20 = ScottPlot.Statistics.Finance.SMA(prices, 20);
            double[] sma50 = ScottPlot.Statistics.Finance.SMA(prices, 50);

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(xs, prices, lineWidth: 0, label: "Price");
            plt.PlotScatter(xs, sma20, label: "20 day SMA", markerSize: 0, lineWidth: 2);
            plt.PlotScatter(xs, sma50, label: "50 day SMA", markerSize: 0, lineWidth: 2);

            plt.YLabel("Price");
            plt.XLabel("Days");
            plt.Legend();
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SMA_Candlestick()
        {
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(150);
            OHLC[] ohlcs = DataGen.RandomStockPrices(rand, xs.Length);
            double[] sma20 = ScottPlot.Statistics.Finance.SMA(ohlcs, 20);
            double[] sma50 = ScottPlot.Statistics.Finance.SMA(ohlcs, 50);

            // replace timestamps with a series of numbers starting at 0
            for (int i = 0; i < ohlcs.Length; i++)
                ohlcs[i].time = i;

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotCandlestick(ohlcs);
            plt.PlotScatter(xs, sma20, label: "20 day SMA",
                color: Color.Blue, markerSize: 0, lineWidth: 2);
            plt.PlotScatter(xs, sma50, label: "50 day SMA",
                color: Color.Maroon, markerSize: 0, lineWidth: 2);

            plt.Title("Simple Moving Average (SMA)");
            plt.YLabel("Price");
            plt.XLabel("Days");
            plt.Legend();
            plt.AxisAutoX(.03);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bollinger_Bands()
        {
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(150);
            OHLC[] ohlcs = DataGen.RandomStockPrices(rand, xs.Length);
            (var sma, var bolL, var bolU) = ScottPlot.Statistics.Finance.Bollinger(ohlcs);

            // replace timestamps with a series of numbers starting at 0
            for (int i = 0; i < ohlcs.Length; i++)
                ohlcs[i].time = i;

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotCandlestick(ohlcs);
            plt.PlotFill(xs, bolL, xs, bolU, fillColor: Color.Blue, fillAlpha: .1);
            plt.PlotScatter(xs, bolL, color: Color.Navy, markerSize: 0);
            plt.PlotScatter(xs, bolU, color: Color.Navy, markerSize: 0);
            plt.PlotScatter(xs, sma, color: Color.Navy, markerSize: 0, lineStyle: LineStyle.Dash);

            plt.Title("Bollinger Bands");
            plt.YLabel("Price");
            plt.XLabel("Days");
            plt.Legend();
            plt.AxisAutoX(.03);
            TestTools.SaveFig(plt);
        }
    }
}
