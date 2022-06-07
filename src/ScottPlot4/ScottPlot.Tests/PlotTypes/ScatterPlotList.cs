using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    internal class ScatterPlotList
    {
        [Test]
        public void Test_ScatterPlotList_Smooth()
        {
            ScottPlot.Plot plt = new(500, 300);

            var spl = plt.AddScatterList(lineWidth: 2, markerSize: 7);
            spl.Add(18.5, 1.43);
            spl.Add(20.6, 1.48);
            spl.Add(22.3, 1.6);
            spl.Add(24.5, 1.59);
            spl.Add(26.6, 1.53);
            spl.Add(15, 1.52);
            spl.Add(15, 1.6);

            spl.Smoothness = 20;

            plt.SetAxisLimits(0, 30, 1.42, 1.62); // mimic excel
            TestTools.SaveFig(plt);
        }
    }
}
