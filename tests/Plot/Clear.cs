using NUnit.Framework;
using ScottPlot;
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
        public void Test_Clear_NoArguments()
        {
            var plt = GetDemoPlot();

            plt.Clear();
            TestTools.SaveFig(plt);

            Assert.AreEqual(0, plt.GetPlottables().Count);
        }

        [Test]
        public void Test_ClearUsingPredicate_ClearOnlySignals()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.GetPlottables().Count;
            int numberOfSignalsBefore = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();
            plt.Clear(x => x is ScottPlot.PlottableSignal);
            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.GetPlottables().Count;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }

        [Test]
        public void Test_ClearUsingPredicate_ClearAllButSignals()
        {
            var plt = GetDemoPlot();

            int numberOfSignalsBefore = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();
            plt.Clear(x => !(x is ScottPlot.PlottableSignal));
            TestTools.SaveFig(plt);
            int numberOfSignalsAfter = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(2, numberOfSignalsAfter);
            Assert.AreEqual(2, plt.GetPlottables().Count);
        }

        [Test]
        public void Test_ClearUsingType_ClearOnlySignals()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.GetPlottables().Count;
            int numberOfSignalsBefore = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();
            plt.Clear(typeof(ScottPlot.PlottableSignal));
            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.GetPlottables().Count;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }

        [Test]
        public void Test_ClearUsingGeneric_ClearSignalsUsingGenerics()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.GetPlottables().Count;
            int numberOfSignalsBefore = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();
            plt.Clear<ScottPlot.PlottableSignal>();
            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.GetPlottables().Count;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }

        [Test]
        public void Test_ClearUsingGeneric_ClearSignalsByExampple()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.GetPlottables().Count;
            int numberOfSignalsBefore = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();

            var exampleSignal = plt.PlotSignal(DataGen.Sin(51));
            plt.Clear(exampleSignal);

            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.GetPlottables().Count;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.GetPlottables().Where(x => x is ScottPlot.PlottableSignal).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }
    }
}
