using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScottPlotTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
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

        [TestMethod]
        public void TestSaveFig()
        {
            {
                var plt = new ScottPlot.Plot(1, 1);
                string file = Path.GetTempPath() + "test.bmp";
                plt.SaveFig(file);
                Bitmap bmp = new Bitmap(file);
                Assert.AreEqual(bmp.Width, 1);
                Assert.AreEqual(bmp.Height, 1);
            }

            {
                var plt = new ScottPlot.Plot(1, 1);
                string file = Path.GetTempPath() + "test.bmp";
                try
                {
                    plt.SaveFig(file, renderFirst: false);
                }
                catch (Exception ex)
                {
                    Assert.Fail("Expected no exception, but got: " + ex.Message);
                }
            }
        }
    }
}
