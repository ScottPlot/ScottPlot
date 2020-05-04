using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Plot
{
    class Scatter
    {
        [Test]
        public void Test_Scatter_AllZeros()
        {
            double[] dataX = { 0, 0, 0, 0, 0 };
            double[] dataY = { 0, 0, 0, 0, 0 };

            var plt = new ScottPlot.Plot();
            plt.PlotScatter(dataX, dataY);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Scatter_EmptyArrays()
        {
            double[] dataX = { };
            double[] dataY = { };

            var plt = new ScottPlot.Plot();

            Assert.Throws<ArgumentException>(() => { plt.PlotScatter(dataX, dataY); });
        }
    }
}
