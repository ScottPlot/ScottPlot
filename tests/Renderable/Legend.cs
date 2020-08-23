using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Renderable
{
    class Legend
    {
        [Test]
        public void Test_Legend_Render()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Sin(51), label: "sin");
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Cos(51), label: "cos");
            plt.Legend();
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Legend_Bitmap()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Sin(51), label: "sin");
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Cos(51), label: "cos");

            // the legend Bitmap should have size
            var bmpLegend1 = plt.GetLegendBitmap();
            Assert.IsNotNull(bmpLegend1);
            Assert.Greater(bmpLegend1.Width, 0);
            Assert.Greater(bmpLegend1.Height, 0);

            // add a new line to the plot
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Consecutive(51), label: "test123");

            // the legend Bitmap should be bigger now
            var bmpLegend2 = plt.GetLegendBitmap();
            Assert.Greater(bmpLegend2.Height, bmpLegend1.Height);
            Assert.Greater(bmpLegend2.Width, bmpLegend1.Width);
        }
    }
}
