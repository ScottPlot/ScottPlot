using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Plot
{
    class Frameless
    {
        [Test]
        public void Test_RadarPlot_WithFrame()
        {
            var plt = new ScottPlot.Plot(600, 400);

            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            plt.AddRadar(values);
            plt.Grid(enable: false);

            TestTools.SaveFig(plt, "1");
            plt.Frameless(false);
            TestTools.SaveFig(plt, "2");
        }

        [Test]
        public void Test_Frameless_HasNoFrame()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.Style(figureBackground: System.Drawing.Color.Magenta);
            plt.Grid(false);
            plt.Frameless();

            plt.AddSignal(ScottPlot.DataGen.Sin(51), color: System.Drawing.Color.Gray);
            plt.Margins(0, 0);

            var bmp = TestTools.GetLowQualityBitmap(plt);
            TestTools.SaveFig(bmp);
            var after = new MeanPixel(bmp);

            Assert.That(after.IsGray());

        }
    }
}
