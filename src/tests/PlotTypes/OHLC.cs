using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    public class OHLC
    {
        [Test]
        public void Test_Candle_SetWidth()
        {
            var ohlcs = new ScottPlot.OHLC[]
            {
                // open, high, low, close, time, timeSpan
                new ScottPlot.OHLC(273, 275, 264, 265, 1, 1),
                new ScottPlot.OHLC(267, 276, 265, 274, 2.5, 2),
                new ScottPlot.OHLC(277, 280, 275, 278, 4, 1),
            };

            var plt = new ScottPlot.Plot(400, 300);
            plt.Grid(false);
            plt.AddCandlesticks(ohlcs);
            plt.SetAxisLimits(xMin: -1, xMax: 5);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_OHLC_SetWidth()
        {
            var ohlcs = new ScottPlot.OHLC[]
            {
                // open, high, low, close, time, timeSpan
                new ScottPlot.OHLC(273, 275, 264, 265, 1, 1),
                new ScottPlot.OHLC(267, 276, 265, 274, 2.5, 2),
                new ScottPlot.OHLC(277, 280, 275, 278, 4, 1),
            };

            var plt = new ScottPlot.Plot(400, 300);
            plt.Grid(false);
            plt.AddCandlesticks(ohlcs);
            plt.SetAxisLimits(xMin: -1, xMax: 5);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Candle_CustomColors()
        {
            var ohlcs = new ScottPlot.OHLC[]
            {
                // open, high, low, close, time, timeSpan
                new ScottPlot.OHLC(273, 275, 264, 265, 1, 1),
                new ScottPlot.OHLC(267, 276, 265, 274, 2.5, 2),
                new ScottPlot.OHLC(277, 280, 275, 278, 4, 1),
            };

            var colorUp = System.Drawing.ColorTranslator.FromHtml("#9926a69a");
            var colorDown = System.Drawing.ColorTranslator.FromHtml("#99ef5350");

            var plt = new ScottPlot.Plot(400, 300);
            var candles = plt.AddCandlesticks(ohlcs);
            candles.ColorUp = colorUp;
            candles.ColorDown = colorDown;
            plt.SetAxisLimits(xMin: -1, xMax: 5);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Candle_WithoutSequential()
        {
            Random rand = new Random(0);
            var ohlcs = ScottPlot.DataGen.RandomStockPrices(rand, 20);

            var plt = new ScottPlot.Plot(600, 400);
            plt.AddCandlesticks(ohlcs);
            plt.Title("Default Behavior");
            plt.XLabel("OHLC DateTime Code");
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Candle_WithSequential()
        {
            Random rand = new Random(0);
            var ohlcs = ScottPlot.DataGen.RandomStockPrices(rand, 20);

            var plt = new ScottPlot.Plot(600, 400);
            var cs = plt.AddCandlesticks(ohlcs);
            cs.Sequential = true;
            plt.Title("sequential: true");
            plt.XLabel("OHLC Index");
            TestTools.SaveFig(plt);
        }
    }
}
