using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            var before = new MeanPixel(plt.GetBitmap());

            plt.Ticks(color: System.Drawing.Color.LightGreen);
            var after = new MeanPixel(plt.GetBitmap());

            Assert.That(before.IsGray());
            Assert.That(after.IsNotGray());
        }

        [Test]
        public void Test_PlotTicks_DisplayTicks()
        {
            var plt = new ScottPlot.Plot();
            var bothTicks = new MeanPixel(plt.GetBitmap());

            plt.Ticks(displayTicksX: false);
            var yTicksOnly = new MeanPixel(plt.GetBitmap());

            plt.Ticks(displayTicksY: false);
            var noTicks = new MeanPixel(plt.GetBitmap());

            Assert.That(yTicksOnly.IsLighterThan(bothTicks));
            Assert.That(noTicks.IsLighterThan(yTicksOnly));
        }

        [Test]
        public void Test_PlotTicks_MultiplierNotation()
        {
            var plt = new ScottPlot.Plot();
            plt.Axis(-1e10, 1e10, -1e10, 1e10);
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt.GetBitmap());

            plt.Ticks(useMultiplierNotation: true);
            var after = new MeanPixel(plt.GetBitmap());

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }

        [Test]
        public void Test_PlotTicks_OffsetNotation()
        {
            var plt = new ScottPlot.Plot();
            plt.Axis(1e10, 1e10 + 10, 1e10, 1e10 + 10);
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt.GetBitmap());

            plt.Ticks(useOffsetNotation: true);
            var after = new MeanPixel(plt.GetBitmap());

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }

        [Test]
        public void Test_PlotTicks_ExponentialNotation()
        {
            var plt = new ScottPlot.Plot();
            plt.Axis(-1e10, 1e10, -1e10, 1e10);
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            plt.Ticks(useMultiplierNotation: true);
            var before = new MeanPixel(plt.GetBitmap());

            plt.Ticks(useExponentialNotation: false);
            var after = new MeanPixel(plt.GetBitmap());

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }

        [Test]
        public void Test_PlotTicks_DateTime()
        {
            var plt = new ScottPlot.Plot();

            DateTime dt1 = new DateTime(2020, 1, 1);
            DateTime dt2 = new DateTime(2020, 12, 25);
            plt.Axis(dt1.ToOADate(), dt2.ToOADate(), dt1.ToOADate(), dt2.ToOADate());
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt.GetBitmap());

            plt.Ticks(dateTimeX: true, dateTimeY: true);
            var after = new MeanPixel(plt.GetBitmap());

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }

        [Test]
        public void Test_PlotTicks_RulerMode()
        {
            var plt = new ScottPlot.Plot();
            plt.XLabel("Horizontal Axis Label");
            plt.YLabel("Vertical Axis Label");
            var before = new MeanPixel(plt.GetBitmap());

            plt.Ticks(rulerModeX: true, rulerModeY: true);
            var after = new MeanPixel(plt.GetBitmap());

            TestTools.SaveFig(plt);
            Assert.That(after.IsDifferentThan(before));
        }
    }
}
