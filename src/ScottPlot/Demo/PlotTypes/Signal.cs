using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Signal
    {
        public static Plot Quickstart()
        {
            double[] signalData = DataGen.RandomWalk(null, 100000);
            double sampleRateHz = 20000;

            var plt = new Plot();
            plt.Title($"Signal Plot ({signalData.Length.ToString("N0")} points)");
            plt.PlotSignal(signalData, sampleRateHz);
            return plt;
        }

        public static Plot BigMarkers()
        {
            Random rand = new Random(0);
            double[] ys1 = DataGen.RandomWalk(rand, 500);
            double[] ys2 = DataGen.RandomWalk(rand, 500, offset: 20);

            var plt = new Plot();
            plt.Title($"Signal Plot with Markers");
            plt.PlotSignal(ys1, label: "default markers");
            plt.PlotSignal(ys2, markerSize: 20, label: "big markers");
            plt.Legend();
            return plt;
        }
    }
}
