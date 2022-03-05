using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Plot
{
    class SaveFig
    {
        [Test]
        public void Test_SaveFig_OutputScaling()
        {
            var plt = new ScottPlot.Plot();
            plt.AddSignal(DataGen.Sin(51), label: "sin");
            plt.AddSignal(DataGen.Cos(51), label: "cos");
            plt.Title("Scaled Figure Demo");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
            plt.Legend();

            System.Drawing.Bitmap bmpA = plt.Render(400, 300);
            Assert.AreEqual(400, bmpA.Width);
            Assert.AreEqual(300, bmpA.Height);

            System.Drawing.Bitmap bmpB = plt.Render(400, 300, scale: .5);
            Assert.AreEqual(200, bmpB.Width);
            Assert.AreEqual(150, bmpB.Height);

            System.Drawing.Bitmap bmpC = plt.Render(400, 300, scale: 2);
            Assert.AreEqual(800, bmpC.Width);
            Assert.AreEqual(600, bmpC.Height);

            System.Drawing.Bitmap bmpD = plt.Render(300, 400, scale: 2);
            Assert.AreEqual(600, bmpD.Width);
            Assert.AreEqual(800, bmpD.Height);

            System.Drawing.Bitmap legendNormal = plt.RenderLegend();
            System.Drawing.Bitmap legendBig = plt.RenderLegend(scale: 2);
            Assert.Greater(legendBig.Width, legendNormal.Width);
            Assert.Greater(legendBig.Height, legendNormal.Height);
        }
    }
}
