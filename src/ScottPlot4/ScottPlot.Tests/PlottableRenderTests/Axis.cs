using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlottableRenderTests
{
    class Axis
    {
        [Test]
        public void Test_Axis_FramelessShowsGridLines()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));
            plt.Frameless();

            // start with default settings
            var bmp1 = TestTools.GetLowQualityBitmap(plt);
            //TestTools.SaveFig(bmp1, "1");

            // make the grid darker
            plt.Grid(color: System.Drawing.Color.Black);
            var bmp2 = TestTools.GetLowQualityBitmap(plt);
            //TestTools.SaveFig(bmp2, "2");

            // measure what changed
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(after.IsDarkerThan(before));
        }
    }
}
