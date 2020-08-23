using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Renderable
{
    class Legend
    {
        [Test]
        public void Test_Legend_Render()
        {
            var plt1 = new ScottPlot.Plot();
            plt1.PlotSignal(DataGen.Sin(100), label: "sin");
            plt1.PlotSignal(DataGen.Cos(100), label: "cos");
            //plt1.Legend();
            TestTools.SaveFig(plt1, "old");

            //var settings = plt1.GetSettings(false);
            //var bmpLegend = settings.Legend.GetBitmap(settings);
        }
    }
}
