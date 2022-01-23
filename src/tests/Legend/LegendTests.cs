using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Legend
{
    class LegendTests
    {
        [Test]
        public void Test_Legend_GettingBitmapBeforeRenderThrows()
        {
            var plt = new ScottPlot.Plot();
            plt.AddSignal(ScottPlot.DataGen.Sin(51), label: "test");
            var leg = plt.Legend();
            System.Drawing.Bitmap bmp = leg.GetBitmap();

            Assert.AreEqual(1, bmp.Width);
            Assert.AreEqual(1, bmp.Height);
        }

        [Test]
        public void Test_Legend_RendersWithoutRenderingPlotFirst()
        {
            var plt = new ScottPlot.Plot();
            plt.AddSignal(ScottPlot.DataGen.Sin(51), label: "test");
            System.Drawing.Bitmap bmp = plt.RenderLegend();

            Assert.AreNotEqual(1, bmp.Width);
            Assert.AreNotEqual(1, bmp.Height);
        }

        [Test]
        public void Test_Legend_RendersWithoutLabels()
        {
            var plt = new ScottPlot.Plot();
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));
            var leg = plt.Legend();
            System.Drawing.Bitmap bmp = plt.RenderLegend();

            Assert.AreEqual(1, bmp.Width);
            Assert.AreEqual(1, bmp.Height);
            Assert.False(leg.HasItems);
            Assert.AreEqual(0, leg.Count);
            Assert.AreEqual(0, leg.GetItems().Length);
        }

        [Test]
        public void Test_Legend_ItemsAndCounts()
        {
            var plt = new ScottPlot.Plot();
            var sig1 = plt.AddSignal(ScottPlot.DataGen.Sin(51), label: "sin");
            var sig2 = plt.AddSignal(ScottPlot.DataGen.Cos(51), label: "cos");
            var leg = plt.Legend();

            System.Drawing.Bitmap bmp1 = plt.RenderLegend();
            Assert.AreNotEqual(1, bmp1.Width);
            Assert.AreNotEqual(1, bmp1.Height);
            Assert.True(leg.HasItems);
            Assert.AreEqual(2, leg.Count);
            Assert.AreEqual(2, leg.GetItems().Length);

            sig1.Label = string.Empty;
            sig2.Label = string.Empty;

            System.Drawing.Bitmap bmp2 = plt.RenderLegend();
            Assert.AreEqual(1, bmp2.Width);
            Assert.AreEqual(1, bmp2.Height);
            Assert.False(leg.HasItems);
            Assert.AreEqual(0, leg.Count);
            Assert.AreEqual(0, leg.GetItems().Length);
        }
    }
}
