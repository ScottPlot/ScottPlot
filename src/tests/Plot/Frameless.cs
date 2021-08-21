using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Plot
{
    class Frameless
    {
        [Test]
        public void Test_RadarPlot_WithFrame()
        {
            var plt = new ScottPlot.Plot(600, 400);

            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            plt.AddRadar(values);
            plt.Grid(enable: false);

            TestTools.SaveFig(plt, "1");
            plt.Frameless(false);
            TestTools.SaveFig(plt, "2");
        }
    }
}
