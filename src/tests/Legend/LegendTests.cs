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
            Assert.Throws<InvalidOperationException>(() => { leg.GetBitmap(false); });
        }

        [Test]
        public void Test_Legend_RendersWithoutRenderingPlotFirst()
        {
            var plt = new ScottPlot.Plot();
            plt.AddSignal(ScottPlot.DataGen.Sin(51), label: "test");
            System.Drawing.Bitmap bmp = plt.RenderLegend();
            TestTools.SaveBitmap(bmp);
        }

        [Test]
        public void Test_Legend_RendersWithoutLabels()
        {
            var plt = new ScottPlot.Plot();
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));
            System.Drawing.Bitmap bmp = plt.RenderLegend();
            TestTools.SaveBitmap(bmp);
        }
    }
}
