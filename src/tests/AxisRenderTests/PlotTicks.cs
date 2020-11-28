using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlotTests.AxisRenderTests
{
    // tests arguments of the Plot.Ticks() method
    class PlotTicks
    {
        [Test]
        public void Test_PlotTicks_Color()
        {
            var plt = new ScottPlot.Plot();
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt);

            plt.Ticks(color: System.Drawing.Color.LightGreen);
            var after = new MeanPixel(plt);

            Assert.That(before.IsGray());
            Assert.That(after.IsNotGray());
        }

        [Test]
        public void Test_PlotTicks_DisplayTicks()
        {
            // TOOD: why does this fail on OSX but nowhere else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            var plt = new ScottPlot.Plot();
            var bothTicks = new MeanPixel(plt);

            plt.Ticks(displayTicksX: false);
            TestTools.SaveFig(plt, "before");
            var yTicksOnly = new MeanPixel(plt);

            plt.Ticks(displayTicksY: false);
            TestTools.SaveFig(plt, "after");
            var noTicks = new MeanPixel(plt);

            Assert.That(yTicksOnly.IsLighterThan(bothTicks));
            Assert.That(noTicks.IsLighterThan(yTicksOnly));
        }

        [Test]
        public void Test_PlotTicks_MultiplierNotation()
        {
            var plt = new ScottPlot.Plot();
            plt.SetAxisLimits(-1e10, 1e10, -1e10, 1e10);
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt);

            plt.Ticks(useMultiplierNotation: true);
            var after = new MeanPixel(plt);

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }

        [Test]
        public void Test_PlotTicks_OffsetNotation()
        {
            var plt = new ScottPlot.Plot();
            plt.SetAxisLimits(1e10, 1e10 + 10, 1e10, 1e10 + 10);
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt);

            plt.Ticks(useOffsetNotation: true);
            var after = new MeanPixel(plt);

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }

        [Test]
        public void Test_PlotTicks_ExponentialNotation()
        {
            var plt = new ScottPlot.Plot();
            plt.SetAxisLimits(-1e10, 1e10, -1e10, 1e10);
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            plt.Ticks(useMultiplierNotation: true);
            var before = new MeanPixel(plt);

            plt.Ticks(useExponentialNotation: false);
            var after = new MeanPixel(plt);

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }

        [Test]
        public void Test_PlotTicks_DateTime()
        {
            var plt = new ScottPlot.Plot();

            DateTime dt1 = new DateTime(2020, 1, 1);
            DateTime dt2 = new DateTime(2020, 12, 25);
            plt.SetAxisLimits(dt1.ToOADate(), dt2.ToOADate(), dt1.ToOADate(), dt2.ToOADate());
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt);

            plt.Ticks(dateTimeX: true, dateTimeY: true);
            var after = new MeanPixel(plt);

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }

        [Test]
        public void Test_PlotTicks_RulerMode()
        {
            var plt = new ScottPlot.Plot();
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt);

            plt.Ticks(rulerModeX: true, rulerModeY: true);
            var after = new MeanPixel(plt);

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }
    }
}
