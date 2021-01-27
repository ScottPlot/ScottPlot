using NUnit.Framework;
using ScottPlot;
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
            plt.AddScatter(dataX, dataY);

            TestTools.SaveFig(plt);
        }
    }
}
