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
        public void Test_SMA_ProducesExpectedOutput()
        {
            double[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double[] sma = ScottPlot.Statistics.Finance.SMA(values, 3, trimNan: false);
            double[] expected = { double.NaN, double.NaN, double.NaN, 3, 4, 5, 6, 7, 8, 9 };

            Assert.That(sma, Is.EquivalentTo(expected));
        }

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
            plt.AddScatter(xs, prices, lineWidth: 0, label: "Price");
            plt.AddScatter(sma20xs, sma20, label: "20 day SMA", markerSize: 0, lineWidth: 2);
            plt.AddScatter(sma50xs, sma50, label: "50 day SMA", markerSize: 0, lineWidth: 2);

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
                ohlcs[i].DateTime = DateTime.FromOADate(i);

            var plt = new ScottPlot.Plot(600, 400);
            plt.AddCandlesticks(ohlcs);
            plt.AddScatter(sma20xs, sma20, label: "20 day SMA",
                color: Color.Blue, markerSize: 0, lineWidth: 2);
            plt.AddScatter(sma50xs, sma50, label: "50 day SMA",
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
                ohlcs[i].DateTime = DateTime.FromOADate(i);

            var plt = new ScottPlot.Plot(600, 400);
            plt.AddCandlesticks(ohlcs);
            plt.AddFill(xs2, bolL, xs2, bolU, color: Color.FromArgb(10, Color.Blue));
            plt.AddScatter(xs2, bolL, color: Color.Navy, markerSize: 0, label: "Bollinger Bands");
            plt.AddScatter(xs2, bolU, color: Color.Navy, markerSize: 0);
            plt.AddScatter(xs2, sma, color: Color.Navy, markerSize: 0, lineStyle: LineStyle.Dash, label: "SMA 20");

            plt.Title("Moving Average and Standard Deviation");
            plt.YLabel("Price");
            plt.XLabel("Days");
            plt.Legend();
            plt.AxisAutoX(.03);
            TestTools.SaveFig(plt);
        }
    }
}
