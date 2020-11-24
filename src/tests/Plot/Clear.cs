﻿using NUnit.Framework;
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

            Assert.AreEqual(0, plt.Plottables.Length);
        }

        [Test]
        public void Test_ClearUsingPredicate_ClearOnlySignals()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.Plottables.Length;
            int numberOfSignalsBefore = plt.Plottables.Where(x => x is SignalPlot).Count();
            plt.Clear(x => x is SignalPlot);
            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.Plottables.Length;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.Plottables.Where(x => x is SignalPlot).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }

        [Test]
        public void Test_ClearUsingPredicate_ClearAllButSignals()
        {
            var plt = GetDemoPlot();

            int numberOfSignalsBefore = plt.Plottables.Where(x => x is SignalPlot).Count();
            plt.Clear(x => !(x is SignalPlot));
            TestTools.SaveFig(plt);
            int numberOfSignalsAfter = plt.Plottables.Where(x => x is SignalPlot).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(2, numberOfSignalsAfter);
            Assert.AreEqual(2, plt.Plottables.Length);
        }

        [Test]
        public void Test_ClearUsingType_ClearOnlySignals()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.Plottables.Length;
            int numberOfSignalsBefore = plt.Plottables.Where(x => x is SignalPlot).Count();
            plt.Clear(typeof(SignalPlot));
            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.Plottables.Length;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.Plottables.Where(x => x is SignalPlot).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }

        [Test]
        public void Test_ClearUsingGeneric_ClearSignalsUsingGenerics()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.Plottables.Length;
            int numberOfSignalsBefore = plt.Plottables.Where(x => x is SignalPlot).Count();
            plt.Clear<SignalPlot>();
            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.Plottables.Length;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.Plottables.Where(x => x is SignalPlot).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }

        [Test]
        public void Test_ClearUsingGeneric_ClearSignalsByExampple()
        {
            var plt = GetDemoPlot();

            int numberOfPlottablesBefore = plt.Plottables.Length;
            int numberOfSignalsBefore = plt.Plottables.Where(x => x is SignalPlot).Count();

            var exampleSignal = plt.PlotSignal(DataGen.Sin(51));
            plt.Clear(exampleSignal);

            TestTools.SaveFig(plt);
            int numberOfPlottablesAfter = plt.Plottables.Length;
            int numberOfPlottablesRemoved = numberOfPlottablesBefore - numberOfPlottablesAfter;
            int numberOfSignalsAfter = plt.Plottables.Where(x => x is SignalPlot).Count();

            Assert.AreEqual(2, numberOfSignalsBefore);
            Assert.AreEqual(0, numberOfSignalsAfter);
            Assert.AreEqual(numberOfSignalsBefore, numberOfPlottablesRemoved);
        }

        private string GetLegendLabels(ScottPlot.Plot plt)
        {
            List<string> names = new List<string>();
            foreach (var plottable in plt.Plottables)
            {
                if (plottable is IHasLegendItems plottableWithLegend)
                {
                    if (plottableWithLegend.LegendItems.Length > 0)
                    {
                        names.Add(plottableWithLegend.LegendItems[0].label);
                    }
                }
            }

            return string.Join(",", names);
        }

        [Test]
        public void Test_Remove_RemovesSinglePlot()
        {
            var plt = new ScottPlot.Plot();

            Random rand = new Random(0);
            var barX = plt.PlotPoint(111, 222, label: "X");
            var sigA = plt.PlotSignal(DataGen.RandomWalk(rand, 100), label: "A");
            var sigB = plt.PlotSignal(DataGen.RandomWalk(rand, 100), label: "B");
            var sigC = plt.PlotSignal(DataGen.RandomWalk(rand, 100), label: "C");
            var sigD = plt.PlotSignal(DataGen.RandomWalk(rand, 100), label: "D");
            var sigE = plt.PlotSignal(DataGen.RandomWalk(rand, 100), label: "E");
            var barY = plt.PlotPoint(111, 222, label: "Y");

            Assert.AreEqual("X,A,B,C,D,E,Y", GetLegendLabels(plt));
            plt.Remove(sigC);
            Assert.AreEqual("X,A,B,D,E,Y", GetLegendLabels(plt));
        }
    }
}
