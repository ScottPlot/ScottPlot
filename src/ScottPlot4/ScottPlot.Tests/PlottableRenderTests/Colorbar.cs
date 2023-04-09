using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlottableRenderTests
{
    class Colorbar
    {
        [Test]
        public void Test_Colorbar_CanBeAdded()
        {
            var plt = new ScottPlot.Plot();
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            plt.AddColorbar();
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            //TestTools.SaveFig(plt);
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Assert.That(after.IsDarkerThan(before));
        }

        [Test]
        public void Test_Colorbar_ColorCanBeChanged()
        {
            var plt = new ScottPlot.Plot();
            var cb = plt.AddColorbar(ScottPlot.Drawing.Colormap.Grayscale);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            cb.UpdateColormap(ScottPlot.Drawing.Colormap.Blues);
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            //TestTools.SaveFig(plt);
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Assert.That(before.IsGray());
            Assert.That(after.IsNotGray());
            Assert.That(after.IsMoreBlueThan(before));
        }

        [Test]
        public void Test_Colorbar_Label()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            var cmap = ScottPlot.Drawing.Colormap.Turbo;

            var cb = plt.AddColorbar(cmap);
            cb.Label = "Turbo Colormap";
            cb.LabelFont.Size = 18;
            cb.LabelFont.Bold = true;

            TestTools.SaveFig(plt);
        }
    }
}
