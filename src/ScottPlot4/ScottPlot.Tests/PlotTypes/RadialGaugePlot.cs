using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlotTests.PlotTypes
{
    class RadialGaugePlot
    {
        [Test]
        public void Test_Different_Gauge_Counts()
        {
            for (int i = 1; i < 6; i++)
            {
                double[] data = Enumerable.Range(0, i)
                    .Select(x => (double)Random.Shared.Next(10, 100))
                    .ToArray();

                ScottPlot.Plot myPlot = new(200, 200);
                myPlot.AddRadialGauge(data);

                TestTools.SaveFig(myPlot, i.ToString());
            }
        }
    }
}
