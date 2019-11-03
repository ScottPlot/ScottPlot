using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;

namespace ScottPlotTests
{
    public class BitmapStuff
    {
        [Test]
        public void TestGetBitmap()
        {
            {
                var plt = new ScottPlot.Plot(1, 1);
                Bitmap bmp = plt.GetBitmap();
                Assert.AreEqual(bmp.Width, 1);
                Assert.AreEqual(bmp.Height, 1);
            }

            {
                var plt = new ScottPlot.Plot(1, 1);
                Bitmap bmp = plt.GetBitmap(false, false);
                Assert.AreEqual(bmp.Width, 1);
                Assert.AreEqual(bmp.Height, 1);
            }
        }

        [Test]
        public void TestSaveFig()
        {
            {
                var plt = new ScottPlot.Plot(1, 1);
                string file = Path.GetTempPath() + "test.bmp";
                plt.SaveFig(file);
                using (Bitmap bmp = new Bitmap(file))
                {
                    Assert.AreEqual(bmp.Width, 1);
                    Assert.AreEqual(bmp.Height, 1);
                }
            }

            {
                var plt = new ScottPlot.Plot(1, 1);
                string file = Path.GetTempPath() + "test.png";
                plt.SaveFig(file, renderFirst: false);
                using (Bitmap bmp = new Bitmap(file))
                {
                    Assert.AreEqual(bmp.Width, 1);
                    Assert.AreEqual(bmp.Height, 1);
                }
            }
        }
    }
}
