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

            plt.AddSignal(DataGen.Sin(51));
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            plt.SaveFig("TestPlot_LowRes.png", 400, 300);
            plt.SaveFig("TestPlot_HighRes.png", 4000, 3000, resize: false);
        }
    }
}
