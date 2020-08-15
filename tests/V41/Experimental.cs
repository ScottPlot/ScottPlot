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
            plt.AntiAlias(false, false, false);
            plt.PlotSignal(ScottPlot.DataGen.Sin(51), label: "sin");
            plt.PlotSignal(ScottPlot.DataGen.Cos(52), label: "cos");

            System.Drawing.Bitmap bmpOld = plt.GetBitmap();
            TestTools.SaveBitmap(bmpOld, "old");
            Assert.AreEqual(width, bmpOld.Width);
            Assert.AreEqual(height, bmpOld.Height);

            // practice customizing layers
            plt.FigureBackground.color = System.Drawing.Color.WhiteSmoke;

            System.Drawing.Bitmap bmpNew = plt.GetBitmapV41(600, 400);
            TestTools.SaveBitmap(bmpNew, "new");
            Assert.AreEqual(width, bmpNew.Width);
            Assert.AreEqual(height, bmpNew.Height);
        }
    }
}
