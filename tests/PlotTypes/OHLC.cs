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
            plt.PlotCandlestick(ohlcs, autoWidth: false);
            plt.Axis(-1, 5);
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
            plt.PlotOHLC(ohlcs, autoWidth: false);
            plt.Axis(-1, 5);
            TestTools.SaveFig(plt);
        }
    }
}
