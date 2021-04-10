using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class Scatter
    {
        [Test]
        public void Test_Scatter_SinglePoint()
        {
            // https://github.com/ScottPlot/ScottPlot/issues/948
            var plt = new ScottPlot.Plot();
            plt.AddScatter(
                xs: new double[1] { 0 },
                ys: new double[1] { 2 },
                markerSize: 1,
                lineWidth: 1);
            plt.Render();
        }
    }
}
