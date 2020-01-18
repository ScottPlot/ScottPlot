using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;

namespace ScottPlotTests
{
    public class Bitmap
    {
        [Test]
        public void Test_Plot_GetBitmap()
        {
            {
                var plt = new ScottPlot.Plot(321, 123);
                System.Drawing.Bitmap bmp = plt.GetBitmap();
                Assert.AreEqual(bmp.Width, 321);
                Assert.AreEqual(bmp.Height, 123);
            }

            {
                var plt = new ScottPlot.Plot(321, 123);
                System.Drawing.Bitmap bmp = plt.GetBitmap(false, false);
                Assert.AreEqual(bmp.Width, 321);
                Assert.AreEqual(bmp.Height, 123);
            }
        }

        [Test]
        public void Test_Plot_SaveFig()
        {
            {
                var plt = new ScottPlot.Plot(321, 123);
                string file = Path.GetTempPath() + "test.bmp";
                plt.SaveFig(file);
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file))
                {
                    Assert.AreEqual(bmp.Width, 321);
                    Assert.AreEqual(bmp.Height, 123);
                }
            }

            {
                var plt = new ScottPlot.Plot(321, 123);
                string file = Path.GetTempPath() + "test.png";
                plt.SaveFig(file, renderFirst: false);
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file))
                {
                    Assert.AreEqual(bmp.Width, 321);
                    Assert.AreEqual(bmp.Height, 123);
                }
            }
        }
    }
}
