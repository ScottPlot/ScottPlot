using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS0618 // Type or member is obsolete

namespace ScottPlotTests.Colormap
{
    class Colormap
    {
        [Test]
        public void Test_Colormap_MultipleRequests()
        {
            var plt = new ScottPlot.Plot();
            var txt = plt.AddSignal(ScottPlot.DataGen.Sin(51));
            var bmp = plt.Render();

            var colormaps1 = ScottPlot.Drawing.Colormap.GetColormaps();
            Assert.IsNotNull(colormaps1);
            Assert.IsNotEmpty(colormaps1);

            var colormaps2 = ScottPlot.Drawing.Colormap.GetColormaps();
            Assert.IsNotNull(colormaps2);
            Assert.IsNotEmpty(colormaps2);

            var oldColormaps = ScottPlot.Drawing.Colormap.GetColormapsOld();
            for (int i = 0; i < oldColormaps.Length; i++)
            {
                Console.WriteLine(colormaps2[i].Name);
                Assert.AreEqual(colormaps2[i].Name, oldColormaps[i].Name);
            }
        }
    }
}
