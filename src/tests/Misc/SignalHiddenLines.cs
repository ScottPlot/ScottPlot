using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Misc
{
    public class SignalHiddenLines
    {
        [Test]
        public void Test_HiddenLines_RenderAsThickAsMarkers()
        {
            var mplt = new MultiPlot(800, 300, 1, 2);

            mplt.GetSubplot(0, 0).PlotSignal(DataGen.Sin(10000), markerSize: 10);
            mplt.GetSubplot(0, 0).Title("Zoomed Out");

            mplt.GetSubplot(0, 1).PlotSignal(DataGen.Sin(10000), markerSize: 10);
            mplt.GetSubplot(0, 1).Title("Zoomed In");
            mplt.GetSubplot(0, 1).Axis(0, 20, -.1, .1);

            TestTools.SaveFig(mplt);
        }
    }
}
