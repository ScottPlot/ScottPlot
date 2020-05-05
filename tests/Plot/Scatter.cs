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

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotScatter(dataX, dataY);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Scatter_EmptyArrays()
        {
            double[] dataX = { };
            double[] dataY = { };

            var plt = new ScottPlot.Plot(400, 300);

            Assert.Throws<ArgumentException>(() => { plt.PlotScatter(dataX, dataY); });
        }

        [Test]
        public void Test_Scatter_HasNanY()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 3, double.NaN, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotScatter(dataX, dataY);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Scatter_HasNanX()
        {
            double[] dataX = { 1, 2, double.NaN, 4, 5 };
            double[] dataY = { 1, 3, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotScatter(dataX, dataY);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Scatter_HasInfY()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 3, double.PositiveInfinity, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotScatter(dataX, dataY);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Scatter_HasInfX()
        {
            double[] dataX = { 1, 2, double.PositiveInfinity, 4, 5 };
            double[] dataY = { 1, 3, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotScatter(dataX, dataY);

            TestTools.SaveFig(plt);
        }
    }
}
