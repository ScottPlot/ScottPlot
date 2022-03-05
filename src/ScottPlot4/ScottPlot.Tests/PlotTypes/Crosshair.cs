using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class Crosshair
    {
        [Test]
        public void Test_CrossHair_Renders()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));
            plt.Title("Crosshair Demo");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            plt.AddCrosshair(42, 0.45);

            // this is outside the data area so should not be rendered
            plt.AddCrosshair(-5, -1.2);

            TestTools.SaveFig(plt);
        }
    }
}
