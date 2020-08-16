using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.V41
{
    class Experimental
    {
        [Test]
        public void Test_NewRenderSystem_ProducesSimilarOutput()
        {
            int width = 600;
            int height = 400;

            var plt = new ScottPlot.Plot(width, height);
            plt.Benchmark(true);
            plt.PlotSignal(ScottPlot.DataGen.Sin(51), label: "sin");
            plt.PlotSignal(ScottPlot.DataGen.Cos(52), label: "cos");
            plt.Title("ScottPlot 4.1 Render System");
            plt.XLabel("horizontal axis");
            plt.YLabel("vertical axis");

            System.Drawing.Bitmap bmpOld = plt.GetBitmap();
            TestTools.SaveBitmap(bmpOld, "old");
            Assert.AreEqual(width, bmpOld.Width);
            Assert.AreEqual(height, bmpOld.Height);

            System.Drawing.Bitmap bmpNew = plt.GetBitmapV41(600, 400);
            TestTools.SaveBitmap(bmpNew, "new");
            Assert.AreEqual(width, bmpNew.Width);
            Assert.AreEqual(height, bmpNew.Height);
        }
    }
}
