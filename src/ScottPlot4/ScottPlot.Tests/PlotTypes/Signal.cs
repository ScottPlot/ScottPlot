using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10, offset: -.5);

            var plt = new ScottPlot.Plot(400, 300);
            var sig = plt.AddSignal(values);
            sig.Color = ScottPlot.Drawing.GDI.Semitransparent(System.Drawing.Color.Red, .5);
            sig.FillAboveAndBelow(System.Drawing.Color.Green, System.Drawing.Color.Blue);
            sig.MarkerSize = 0;
            sig.LineWidth = 5;
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalLowDensity_FilledAbove()
        {
            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10, offset: -.5);

            var plt = new ScottPlot.Plot(400, 300);
            var sig = plt.AddSignal(values);
            sig.Color = ScottPlot.Drawing.GDI.Semitransparent(System.Drawing.Color.Red, .5);
            sig.MarkerSize = 0;
            sig.LineWidth = 5;
            sig.FillAbove(System.Drawing.Color.Green);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalLowDensity_FilledBelow()
        {
            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10, offset: -.5);

            var plt = new ScottPlot.Plot(400, 300);
            var sig = plt.AddSignal(values);
            sig.Color = ScottPlot.Drawing.GDI.Semitransparent(System.Drawing.Color.Red, .5);
            sig.MarkerSize = 0;
            sig.LineWidth = 5;
            sig.FillBelow(System.Drawing.Color.Blue);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalHighDensity_Filled()
        {
            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10_000);

            var plt = new ScottPlot.Plot(400, 300);
            var sig = plt.AddSignal(values);
            sig.FillAboveAndBelow(System.Drawing.Color.Green, System.Drawing.Color.Blue);
            sig.Color = ScottPlot.Drawing.GDI.Semitransparent(System.Drawing.Color.Red, .5);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalHighDensity_FilledAbove()
        {
            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10_000);

            var plt = new ScottPlot.Plot(400, 300);
            var sig = plt.AddSignal(values);
            sig.Color = ScottPlot.Drawing.GDI.Semitransparent(System.Drawing.Color.Red, .5);
            sig.FillAbove(System.Drawing.Color.Green);
            sig.BaselineY = 0.2;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalHighDensity_FilledBelow()
        {
            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10_000);

            var plt = new ScottPlot.Plot(400, 300);
            var sig = plt.AddSignal(values);
            sig.Color = ScottPlot.Drawing.GDI.Semitransparent(System.Drawing.Color.Red, .5);
            sig.FillBelow(System.Drawing.Color.Blue);
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

        [Test]
        public void Test_Signal_FillBelow_ZoomOut()
        {
            // addresses issue #1476 where zooming far out causes the width of the
            // fill to be zero and a hard crash
            // https://github.com/ScottPlot/ScottPlot/issues/1476

            var plt = new ScottPlot.Plot(400, 300);

            var line = plt.AddSignal(ScottPlot.DataGen.RandomWalk(100));

            line.FillBelow();

            for (int i = 0; i < 10; i++)
            {
                plt.AxisZoom(.1, .1);
                plt.Render();
            }
        }

        [Test]
        public void Test_Signal_Types()
        {
            // see discussion in https://github.com/ScottPlot/ScottPlot/pull/1927

            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotConst<double>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotConst<float>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotConst<int>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotConst<byte>());

            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotXYGeneric<double, double>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotXYGeneric<float, float>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotXYGeneric<int, int>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotXYGeneric<double, byte>());
            Assert.Throws<InvalidOperationException>(() => new ScottPlot.Plottable.SignalPlotXYGeneric<byte, byte>());
        }

        [Test]
        public void Test_GenericSignal_ByteArray()
        {
            double[] doubles = ScottPlot.DataGen.Sin(51, offset: 100, mult: 100);
            byte[] bytes = doubles.Select(x => (byte)(x + 5)).ToArray();

            byte[] bytesX = ScottPlot.DataGen.Consecutive(51).Select(x => (byte)x).ToArray();
            byte[] bytesY = doubles.Select(x => (byte)(x + 10)).ToArray();

            double[] doublesX = ScottPlot.DataGen.Consecutive(51);
            double[] doublesY = doubles.Select(x => x + 15).ToArray();

            ScottPlot.Plot plt = new();
            plt.AddSignalConst(doubles, label: "doubles");
            plt.AddSignalConst(bytes, label: "bytes");
            plt.AddSignalXYConst(doublesX, bytesY, label: "bytes XY");
            plt.AddSignalXYConst(doublesX, doublesY, label: "doubles XY");
            plt.Legend();
            TestTools.SaveFig(plt);
        }

        [TestCase(1, 9.8, 10, 0)]
        [TestCase(1, 10.2, 10, 0)]
        [TestCase(.5, 19.5, 20, 0)]
        [TestCase(.5, 20.5, 20, 0)]
        [TestCase(2, 4.9, 5, 0)]
        [TestCase(2, 5.1, 5, 0)]
        [TestCase(1, 109.8, 110, 100)]
        [TestCase(1, 110.2, 110, 100)]
        [TestCase(.5, 119.5, 120, 100)]
        [TestCase(.5, 120.5, 120, 100)]
        [TestCase(2, 104.9, 105, 100)]
        [TestCase(2, 105.1, 105, 100)]
        public void Test_Signal_GetPointNearestX(double sampleRate, double mouseX, double expectedNearestX, double offsetX)
        {
            ScottPlot.Plot plt = new(600, 400);
            plt.Title($"Sample Rate: {sampleRate}");
            plt.AddVerticalLine(mouseX, System.Drawing.Color.Magenta);

            double[] ys = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            var sig = plt.AddSignal(ys, sampleRate);
            sig.OffsetX = offsetX;

            (double nearestX, double nearestY, int nearestIndex) = sig.GetPointNearestX(mouseX);
            plt.AddVerticalLine(nearestX, System.Drawing.Color.Red, 1, ScottPlot.LineStyle.Dash);
            plt.AddHorizontalLine(nearestY, System.Drawing.Color.Red, 1, ScottPlot.LineStyle.Dash);

            Assert.That(nearestX, Is.EqualTo(expectedNearestX));
        }

        [Test]
        public void Test_Signal_Smooth()
        {
            Random rand = new(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: 10, offset: -.5);

            var plt = new ScottPlot.Plot(400, 300);
            var sig = plt.AddSignal(values);
            sig.Color = ScottPlot.Drawing.GDI.Semitransparent(System.Drawing.Color.Red, .5);
            sig.FillAboveAndBelow(System.Drawing.Color.Green, System.Drawing.Color.Blue);
            sig.MarkerSize = 0;
            sig.LineWidth = 5;
            sig.BaselineY = 0.2;
            sig.Smooth = true;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Signal_Empty()
        {
            double[] values = Array.Empty<double>();

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(values);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Signal_FillDisable()
        {
            // https://github.com/ScottPlot/ScottPlot/issues/2436

            ScottPlot.Plot plt = new(400, 300);
            var sig = plt.AddSignal(ScottPlot.DataGen.Sin(51));
            sig.FillAboveAndBelow(Color.Red, Color.Blue);
            sig.FillDisable();

            TestTools.SaveFig(plt);
        }
    }
}
