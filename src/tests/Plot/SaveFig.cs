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
            plt.AddSignal(DataGen.Cos(51));
            plt.Title("Scaled Figure Demo");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            plt.SaveFig("Test_Scaled_Default.png", 400, 300);
            plt.SaveFig("Test_Scaled_0.5.png", 400, 300, scale: 0.5);
            plt.SaveFig("Test_Scaled_5.png", 400, 300, scale: 5);
            plt.SaveFig("Test_Scaled_5_tall.png", 300, 400, scale: 5);
        }
    }
}
