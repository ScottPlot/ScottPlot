using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Signal
    {
        public class Quickstart : IPlotDemo
        {
            public string name { get; }
            public string description { get; }

            public void Render(Plot plt)
            {
                double[] signalData = DataGen.RandomWalk(null, 100000);
                double sampleRateHz = 20000;

                plt = new Plot();
                plt.Title($"Signal Plot ({signalData.Length.ToString("N0")} points)");
                plt.PlotSignal(signalData, sampleRateHz);
            }
        }

        public class CustomLineAndMarkers : IPlotDemo
        {
            public string name { get; }
            public string description { get; }

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] ys = DataGen.RandomWalk(rand, 500);

                plt = new Plot();
                plt.Title($"Signal Plot with Markers");
                plt.PlotSignal(ys, label: "default");
                plt.PlotSignal(ys, yOffset: 20, markerSize: 10, label: "large markers");
                plt.PlotSignal(ys, yOffset: 40, lineWidth: 10, markerSize: 0, label: "large line");
                plt.Legend();
            }
        }
    }
}
