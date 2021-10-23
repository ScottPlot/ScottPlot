using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class Signal
    {
        [Test]
        public void Test_SignalLowDensity_Filled()
        {
            var plt = new ScottPlot.Plot(400, 300);

            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10, offset: -.5);
            var sig = plt.AddSignal(values);
            sig.Color = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
            sig.MarkerSize = 0;
            sig.LineWidth = 5;
            sig.FillType = ScottPlot.FillType.FillAboveAndBelow;
            sig.FillColor1 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
            sig.FillColor2 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Blue);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalLowDensity_FilledAbove()
        {
            var plt = new ScottPlot.Plot(400, 300);

            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10, offset: -.5);
            var sig = plt.AddSignal(values);
            sig.Color = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
            sig.MarkerSize = 0;
            sig.LineWidth = 5;
            sig.FillType = ScottPlot.FillType.FillAbove;
            sig.FillColor1 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
            sig.FillColor2 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Blue);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalLowDensity_FilledBelow()
        {
            var plt = new ScottPlot.Plot(400, 300);

            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10, offset: -.5);
            var sig = plt.AddSignal(values);
            sig.Color = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
            sig.MarkerSize = 0;
            sig.LineWidth = 5;
            sig.FillType = ScottPlot.FillType.FillBelow;
            sig.FillColor1 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
            sig.FillColor2 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Blue);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalHighDensity_Filled()
        {
            var plt = new ScottPlot.Plot(400, 300);

            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10_000);
            var sig = plt.AddSignal(values);
            sig.Color = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
            sig.FillType = ScottPlot.FillType.FillAboveAndBelow;
            sig.FillColor1 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
            sig.FillColor2 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Blue);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalHighDensity_FilledAbove()
        {
            var plt = new ScottPlot.Plot(400, 300);

            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10_000);
            var sig = plt.AddSignal(values);
            sig.Color = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
            sig.FillType = ScottPlot.FillType.FillAbove;
            sig.FillColor1 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
            sig.FillColor2 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Blue);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalHighDensity_FilledBelow()
        {
            var plt = new ScottPlot.Plot(400, 300);

            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10_000);
            var sig = plt.AddSignal(values);
            sig.Color = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
            sig.FillType = ScottPlot.FillType.FillBelow;
            sig.FillColor1 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
            sig.FillColor2 = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Blue);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Signal_FillBelowMethod()
        {
            var plt = new ScottPlot.Plot(400, 300);

            double[] ys = ScottPlot.DataGen.RandomWalk(new Random(0), 10);
            var sig = plt.AddSignal(ys);
            sig.FillBelow();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Signal_FillAboveMethod()
        {
            var plt = new ScottPlot.Plot(400, 300);

            double[] ys = ScottPlot.DataGen.RandomWalk(new Random(0), 10);
            var sig = plt.AddSignal(ys);
            sig.FillAbove();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Signal_FillAboveAndBelowMethod()
        {
            var plt = new ScottPlot.Plot(400, 300);

            double[] ys = ScottPlot.DataGen.RandomWalk(new Random(0), 10);
            var sig = plt.AddSignal(ys);
            sig.FillAboveAndBelow(System.Drawing.Color.Magenta, System.Drawing.Color.Green);
            sig.BaselineY = 1.5;

            TestTools.SaveFig(plt);
        }
    }
}
