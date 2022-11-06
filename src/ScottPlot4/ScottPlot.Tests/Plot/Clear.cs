using NUnit.Framework;
using ScottPlot;
using ScottPlot.Plottable;
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
            plt.AddScatter(DataGen.Random(rand, 100, 20), DataGen.Random<double>(rand, 100, 5, 3), label: "scatter1");
            plt.AddSignal(DataGen.RandomWalk(rand, 100), label: "signal1");
            plt.AddScatter(DataGen.Random(rand, 100), ScottPlot.DataGen.Random(rand, 100), label: "scatter2");
            plt.AddSignal(DataGen.RandomWalk(rand, 100), label: "signal2");
            plt.AddVerticalLine(43, width: 4, label: "vline");
            plt.AddHorizontalLine(1.23, width: 4, label: "hline");
            plt.AddText("ScottPlot", 50, 0.25);
            plt.Legend();
            return plt;
        }

        [Test]
        public void Test_Clear_NoArguments()
        {
            var plt = GetDemoPlot();

            plt.Clear();
            TestTools.SaveFig(plt);

            Assert.AreEqual(0, plt.GetPlottables().Length);
        }

        [Test]
        public void Test_ClearUsingType_ClearOnlySignals()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.GetPlottables().Length;
            int numberOfSignalsBefore = plt.GetPlottables().Where(x => x is SignalPlot).Count();
            plt.Clear(typeof(SignalPlot));
            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.GetPlottables().Length;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.GetPlottables().Where(x => x is SignalPlot).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }

        private string GetLegendLabels(ScottPlot.Plot plt)
        {
            List<string> names = new List<string>();
            foreach (var plottable in plt.GetPlottables())
            {
                LegendItem[] legendItems = plottable.GetLegendItems();
                if (legendItems != null && legendItems.Length > 0)
                    names.Add(legendItems[0].label);
            }

            return string.Join(",", names);
        }

        [Test]
        public void Test_Remove_RemovesSinglePlot()
        {
            var plt = new ScottPlot.Plot();

            Random rand = new Random(0);
            var barX = plt.AddPoint(111, 222);
            var sigA = plt.AddSignal(DataGen.RandomWalk(rand, 100));
            var sigB = plt.AddSignal(DataGen.RandomWalk(rand, 100));
            var sigC = plt.AddSignal(DataGen.RandomWalk(rand, 100));
            var sigD = plt.AddSignal(DataGen.RandomWalk(rand, 100));
            var sigE = plt.AddSignal(DataGen.RandomWalk(rand, 100));
            var barY = plt.AddPoint(111, 222);

            sigC.Label = "C";

            Assert.AreEqual(",,,C,,,", GetLegendLabels(plt));
            plt.Remove(sigC);
            Assert.AreEqual(",,,,,", GetLegendLabels(plt));
        }
    }
}
