using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Statistics
{
    class Interpolation
    {
        [Test]
        public void Test_NaturalSpline_ValuesMatch()
        {
            double[] xs = { 0, 10, 20, 30 };
            double[] ys = { 65, 25, 55, 80 };

            var nsi = new ScottPlot.Statistics.Interpolation.NaturalSpline(xs, ys, resolution: 20);
            var psi = new ScottPlot.Statistics.Interpolation.PeriodicSpline(xs, ys, resolution: 20);
            var esi = new ScottPlot.Statistics.Interpolation.EndSlopeSpline(xs, ys, resolution: 20);

            // verify output values have not deviated frrom what is expected
            Assert.AreEqual(-1752748231, ScottPlot.Tools.SimpleHash(nsi.interpolatedXs));
            Assert.AreEqual(-1060440570, ScottPlot.Tools.SimpleHash(nsi.interpolatedYs));
            Assert.AreEqual(-1752748231, ScottPlot.Tools.SimpleHash(psi.interpolatedXs));
            Assert.AreEqual(-351909896, ScottPlot.Tools.SimpleHash(psi.interpolatedYs));
            Assert.AreEqual(-1752748231, ScottPlot.Tools.SimpleHash(esi.interpolatedXs));
            Assert.AreEqual(-2040768015, ScottPlot.Tools.SimpleHash(esi.interpolatedYs));

            var plt = new ScottPlot.Plot();
            plt.PlotScatter(xs, ys, Color.Black, markerSize: 10, lineWidth: 0, label: "Original Data");
            plt.PlotScatter(nsi.interpolatedXs, nsi.interpolatedYs, Color.Red, markerSize: 3, label: "Natural Spline");
            plt.PlotScatter(psi.interpolatedXs, psi.interpolatedYs, Color.Green, markerSize: 3, label: "Periodic Spline");
            plt.PlotScatter(esi.interpolatedXs, esi.interpolatedYs, Color.Blue, markerSize: 3, label: "End Slope Spline");
            plt.Legend();
            TestTools.SaveFig(plt);
        }
    }
}
