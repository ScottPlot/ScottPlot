using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlottableRenderTests
{
    internal class Signal
    {
        [Test]
        public void Test_Signal_SinglePointShouldDrawMarker()
        {
            double[] dataY = { 69 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(dataY);

            var meanPixel = new MeanPixel(plt);
            Assert.That(meanPixel.IsNotGray());

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalXY_SinglePointShouldDrawMarker()
        {
            double[] dataX = { 42 };
            double[] dataY = { 69 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignalXY(dataX, dataY);

            var meanPixel = new MeanPixel(plt);
            Assert.That(meanPixel.IsNotGray());

            TestTools.SaveFig(plt);
        }
    }
}
