using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Plot
{
    public class Clear
    {
        private ScottPlot.Plot GetDemoPlot()
        {
            Random rand = new Random(0);
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(ScottPlot.DataGen.Random(rand, 100, 20), ScottPlot.DataGen.Random(rand, 100, 5, 3), label: "scatter1");
            plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 100), label: "signal1");
            plt.PlotScatter(ScottPlot.DataGen.Random(rand, 100), ScottPlot.DataGen.Random(rand, 100), label: "scatter2");
            plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 100), label: "signal2");
            plt.PlotVLine(43, lineWidth: 4, label: "vline");
            plt.PlotHLine(1.23, lineWidth: 4, label: "hline");
            plt.PlotText("ScottPlot", 50, 0.25, rotation: -45, fontSize: 36, label: "text");
            plt.Legend();
            return plt;
        }

        [Test]
        public void Test_clear_withoutArguments()
        {
            var plt = GetDemoPlot();

            plt.Clear();
            TestTools.SaveFig(plt);

            Assert.AreEqual(0, plt.GetPlottables().Count);
        }

        [Test]
        public void Test_clear_usingPredicate()
        {
            var plt = GetDemoPlot();

            int numberOfSignalsBefore = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();
            plt.Clear(x => x is ScottPlot.PlottableSignal);
            TestTools.SaveFig(plt);
            int numberOfSignalsAfter = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
        }
    }
}
