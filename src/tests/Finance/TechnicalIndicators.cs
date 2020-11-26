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

            // create SMAs and Xs for each
            double[] sma20 = ScottPlot.Statistics.Finance.SMA(prices, 20);
            double[] sma20xs = xs.Skip(20).ToArray();
            double[] sma50 = ScottPlot.Statistics.Finance.SMA(prices, 50);
            double[] sma50xs = xs.Skip(50).ToArray();

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(xs, prices, lineWidth: 0, label: "Price");
            plt.PlotScatter(sma20xs, sma20, label: "20 day SMA", markerSize: 0, lineWidth: 2);
            plt.PlotScatter(sma50xs, sma50, label: "50 day SMA", markerSize: 0, lineWidth: 2);

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

            // create SMAs and Xs for each
            double[] sma20 = ScottPlot.Statistics.Finance.SMA(ohlcs, 20);
            double[] sma20xs = xs.Skip(20).ToArray();
            double[] sma50 = ScottPlot.Statistics.Finance.SMA(ohlcs, 50);
            double[] sma50xs = xs.Skip(50).ToArray();

            // replace timestamps with a series of numbers starting at 0
            for (int i = 0; i < ohlcs.Length; i++)
                ohlcs[i].time = i;

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotCandlestick(ohlcs);
            plt.PlotScatter(sma20xs, sma20, label: "20 day SMA",
                color: Color.Blue, markerSize: 0, lineWidth: 2);
            plt.PlotScatter(sma50xs, sma50, label: "50 day SMA",
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

            // calculate moving average X and Ys
            (var sma, var bolL, var bolU) = ScottPlot.Statistics.Finance.Bollinger(ohlcs, 20);
            double[] xs2 = xs.Skip(20).ToArray();

            // replace timestamps with a series of numbers starting at 0
            for (int i = 0; i < ohlcs.Length; i++)
                ohlcs[i].time = i;

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotCandlestick(ohlcs);
            plt.PlotFill(xs2, bolL, xs2, bolU, fillColor: Color.Blue, fillAlpha: .05);
            plt.PlotScatter(xs2, bolL, color: Color.Navy, markerSize: 0, label: "Bollinger Bands");
            plt.PlotScatter(xs2, bolU, color: Color.Navy, markerSize: 0);
            plt.PlotScatter(xs2, sma, color: Color.Navy, markerSize: 0, lineStyle: LineStyle.Dash, label: "SMA 20");

            plt.Title("Moving Average and Standard Deviation");
            plt.YLabel("Price");
            plt.XLabel("Days");
            plt.Legend();
            plt.AxisAutoX(.03);
            TestTools.SaveFig(plt);
        }
    }
}
