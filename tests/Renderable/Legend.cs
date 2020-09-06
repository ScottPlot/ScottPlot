using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

            var mean1 = TestTools.MeanPixel(plt.GetBitmap());
            plt.Legend();
            var mean2 = TestTools.MeanPixel(plt.GetBitmap());

            // the legend should darken the mean pixel intensity
            Assert.AreEqual(mean2.A, mean1.A);
            Assert.Less(mean2.R, mean1.R);
            Assert.Less(mean2.G, mean1.G);
            Assert.Less(mean2.B, mean1.B);
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

        [Test]
        public void Test_Legend_LooksGoodInEveryPosition()
        {

            var mplt = new MultiPlot(1000, 800, 3, 3);

            legendLocation[] locs = Enum.GetValues(typeof(legendLocation)).Cast<legendLocation>().ToArray();
            for (int i = 0; i < locs.Length; i++)
            {
                var plt = mplt.subplots[i];
                plt.PlotScatter(DataGen.Consecutive(20), DataGen.Sin(20), markerShape: MarkerShape.filledSquare, label: "sin");
                plt.PlotScatter(DataGen.Consecutive(20), DataGen.Cos(20), markerShape: MarkerShape.openDiamond, label: "cos");
                plt.Legend(location: locs[i]);
                plt.Title(locs[i].ToString());
            }

            TestTools.SaveFig(mplt);
        }

        [Test]
        public void Test_Legend_ReverseOrder()
        {
            var plt1 = new ScottPlot.Plot();
            plt1.PlotSignal(DataGen.Sin(100), label: "sin");
            plt1.PlotSignal(DataGen.Cos(100), label: "cos");
            plt1.Legend();
            string hash1 = ScottPlot.Tools.BitmapHash(plt1.GetBitmap());

            var plt2 = new ScottPlot.Plot();
            plt2.PlotSignal(DataGen.Sin(100), label: "sin");
            plt2.PlotSignal(DataGen.Cos(100), label: "cos");
            plt2.Legend(reverseOrder: true);
            string hash2 = ScottPlot.Tools.BitmapHash(plt2.GetBitmap());

            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Test_Legend_Style()
        {
            var plt1 = new ScottPlot.Plot(600, 400);
            plt1.PlotScatter(DataGen.Sin(10), DataGen.Sin(10), label: "sin", color: Color.Black, lineWidth: 1);
            plt1.Legend();
            var mean1 = TestTools.MeanPixel(plt1.GetBitmap());

            var plt2 = new ScottPlot.Plot(600, 400);
            plt2.PlotScatter(DataGen.Sin(10), DataGen.Sin(10), label: "sin", color: Color.Black, lineWidth: 2);
            plt2.Legend();
            var mean2 = TestTools.MeanPixel(plt2.GetBitmap());

            // thicker line means darker pixel intensity
            Assert.Less(mean2.R, mean1.R);

            var plt3 = new ScottPlot.Plot(600, 400);
            plt3.PlotScatter(DataGen.Sin(10), DataGen.Sin(10), label: "sin", color: Color.Gray, lineWidth: 2);
            plt3.Legend();
            var mean3 = TestTools.MeanPixel(plt3.GetBitmap());

            // lighter color means greater pixel intensity
            Assert.Greater(mean3.R, mean2.R);
        }

        // Disabled because this test fails on Linux and MacOS due to a System.Drawing limitation
        //[Test]
        public void Test_Legend_Bold()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Sin(51), label: "sin");
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Cos(51), label: "cos");
            plt.Legend();

            var meanRegular = TestTools.MeanPixel(plt.GetBitmap());

            plt.Legend(bold: true);
            var meanBold = TestTools.MeanPixel(plt.GetBitmap());

            // bold text will darken the mean pixel intensity
            Assert.Less(meanBold.R, meanRegular.R);
        }

        [Test]
        public void Test_Legend_RequestBeforeRender()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Sin(51), label: "sin");
            plt.PlotScatter(DataGen.Consecutive(51), DataGen.Cos(51), label: "cos");
            plt.Legend();

            System.Drawing.Bitmap bmpLegend = plt.GetLegendBitmap();
            Assert.Less(bmpLegend.Width, 600);
            Assert.Less(bmpLegend.Height, 400);
        }
    }
}
