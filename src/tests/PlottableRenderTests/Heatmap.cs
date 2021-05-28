using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace ScottPlotTests.PlottableRenderTests
{
    class Heatmap
    {
        [Test]
        public void Test_Heatmap_Interpolation()
        {
            // see discussion in https://github.com/ScottPlot/ScottPlot/issues/1003

            // Interpolation doesn't seem to work on Linux or MacOS in Azure Pipelines
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            Random rand = new(0);
            double[,] data = ScottPlot.DataGen.Random2D(rand, 4, 5);

            var plt = new ScottPlot.Plot(300, 300);
            var hm = plt.AddHeatmap(data, lockScales: false);
            plt.SetAxisLimits(0, data.GetLength(1), 0, data.GetLength(0));

            TestTools.SaveFig(plt, "default");
            string hash1 = ScottPlot.Tools.BitmapHash(plt.Render());

            hm.Smooth = true;

            TestTools.SaveFig(plt, "smooth");
            string hash2 = ScottPlot.Tools.BitmapHash(plt.Render());

            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
