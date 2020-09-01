using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ScottPlotTests
{
    public class PlotTests
    {
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 0)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, -1)]
        public void Test_Plot_InvalidDimensionsThrow(int width, int height)
        {
            var plt = new ScottPlot.Plot();
            Assert.Throws<ArgumentException>(() => { plt.GetBitmap(width, height); });
        }

        [Test]
        public void Test_Plot_SaveFig()
        {
            string filePath = System.IO.Path.GetFullPath("test_plot_savefig.bmp");
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            var plt = new ScottPlot.Plot();
            double[] xs = ScottPlot.Generate.Consecutive(51);
            double[] ys = ScottPlot.Generate.Sin(51);
            plt.PlotScatter(xs, ys);

            plt.Scale(0, 50, -1, 1);

            plt.SaveFig(filePath, 600, 400);
            Console.WriteLine($"Wrote: {filePath}");

            Assert.That(System.IO.File.Exists(filePath));
        }

        [TestCase(1, 1)]
        [TestCase(640, 480)]
        [TestCase(7680, 4320)]
        public void Test_Render_BitmapDimensions(int width, int height)
        {
            var plt = new ScottPlot.Plot();
            Bitmap bmp = plt.GetBitmap(width, height);
            Assert.AreEqual(width, bmp.Width);
            Assert.AreEqual(height, bmp.Height);
        }
    }
}