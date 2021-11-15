using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Plot
{
    internal class GetPixel
    {
        [Test]
        public void Test_GetPixel_ReturnsCorrectValue()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.Render();

            // right edge
            Assert.Greater(plt.GetPixelX(50), plt.Width / 2);

            // left edge
            Assert.Less(plt.GetPixelX(0), plt.Width / 2);

            // top edge
            Assert.Less(plt.GetPixelY(1), plt.Width / 2);

            // bottom edge
            Assert.Greater(plt.GetPixelY(-1), plt.Width / 2);
        }
    }
}
