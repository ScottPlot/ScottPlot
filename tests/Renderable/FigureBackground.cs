using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.Renderable
{
    class FigureBackground
    {
        [Test]
        public void Test_FigureBackground_ColorCanBeSet()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(DataGen.Sin(51));

            var mean1 = TestTools.MeanPixel(plt.GetBitmap());
            plt.Style(figBg: Color.Blue);
            var mean2 = TestTools.MeanPixel(plt.GetBitmap());

            // we made the background white->blue, meaning we preserved blue while reducing red and green
            Assert.AreEqual(mean2.A, mean1.A);
            Assert.Less(mean2.R, mean1.R);
            Assert.Less(mean2.G, mean1.G);
            Assert.AreEqual(mean2.B, mean1.B);
        }

        [Test]
        public void Test_FigureBackground_DefaultIsOpaqueWhite()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(DataGen.Sin(51));

            var mean1 = TestTools.MeanPixel(plt.GetBitmap());
            plt.Style(figBg: Color.White);
            var mean2 = TestTools.MeanPixel(plt.GetBitmap());

            Assert.AreEqual(mean2.A, mean1.A);
            Assert.AreEqual(mean2.R, mean1.R);
            Assert.AreEqual(mean2.G, mean1.G);
            Assert.AreEqual(mean2.B, mean1.B);
        }

        [Test]
        public void Test_FigureBackground_SupportsTransparency()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(DataGen.Sin(51));

            var mean1 = TestTools.MeanPixel(plt.GetBitmap());
            plt.Style(figBg: Color.Transparent);
            var mean2 = TestTools.MeanPixel(plt.GetBitmap());

            Assert.Less(mean2.A, mean1.A);
        }
    }
}
