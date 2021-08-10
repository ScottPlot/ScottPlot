using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class Radar
    {
        [Test]
        public void Test_Radar_ZeroNorm()
        {
            // https://github.com/ScottPlot/ScottPlot/issues/1139

            var plt = new ScottPlot.Plot(400, 300);

            double[,] values = {
                { 78,  83, 0, 76, 43 },
                { 100, 50, 0, 60, 90 }
            };

            double[] maxValues = { 100, 100, 0, 100, 100 };

            plt.AddRadar(values, independentAxes: true, maxValues: maxValues);

            TestTools.SaveFig(plt);
        }
    }
}
