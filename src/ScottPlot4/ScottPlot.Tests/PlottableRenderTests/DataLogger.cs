using NUnit.Framework;
using ScottPlotTests;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlottableRenderTests
{
    internal class Datalogger
    {
        [Test]
        public void Test_DataLogger_LineStyle()
        {
            System.Drawing.Bitmap bmp = new(600, 400);

            var plt = new ScottPlot.Plot();
            ScottPlot.Plottable.DataLogger line = plt.AddDataLogger(color: Color.Red, lineWidth: 1, label: "Line1");
            line.Add(1, 100);
            line.Add(2, 200);
            line.MarkerSize = 5;
            line.LineStyle = ScottPlot.LineStyle.Solid;

            var bmp1 = plt.Render(lowQuality: true);

            line.LineStyle = ScottPlot.LineStyle.Dot;

            var bmp2 = plt.Render(lowQuality: true);

            // measure what changed
            // TestTools.SaveFig(bmp1, "1");
            // TestTools.SaveFig(bmp2, "2");

            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Assert.That(after.IsDifferentThan(before));
        }
    }
}
