using NUnit.Framework;
using System.Drawing;

namespace ScottPlotTests
{
    public class UnitTest1
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
    }
}