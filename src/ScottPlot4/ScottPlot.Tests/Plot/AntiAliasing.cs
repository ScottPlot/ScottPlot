using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Plot
{
    class AntiAliasing
    {
        [Test]
        public void Test_AntiAliasing_Works()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51), label: "sin");
            plt.AddSignal(ScottPlot.DataGen.Cos(51), label: "cos");
            plt.YLabel("Vertical Axis");
            plt.XLabel("Horizontal Axis");
            plt.Title("Plot Title");
            plt.Legend();

            // start with default settings
            var bmp1 = plt.Render(lowQuality: true);

            // change the plottable
            var bmp2 = plt.Render(lowQuality: false);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");

            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Assert.That(after.IsDifferentThan(before));
        }
    }
}
