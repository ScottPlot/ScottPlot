using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Signal
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Signal Plot Quickstart";
            public string description { get; }

            public void Render(Plot plt)
            {
                double[] signalData = DataGen.RandomWalk(null, 100_000);
                double sampleRateHz = 20000;

                plt.Title($"Signal Plot ({signalData.Length.ToString("N0")} points)");
                plt.PlotSignal(signalData, sampleRateHz);
            }
        }

        public class CustomLineAndMarkers : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Styled Signal Plot";
            public string description { get; } = "Signal plot with styled lines and markers";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] ys = DataGen.RandomWalk(rand, 500);

                plt.Title($"Signal Plot with Markers");
                plt.PlotSignal(ys, label: "default");
                plt.PlotSignal(ys, yOffset: 20, markerSize: 10, label: "large markers");
                plt.PlotSignal(ys, yOffset: 40, lineWidth: 10, markerSize: 0, label: "large line");
                plt.Legend();
            }
        }

        public class RandomWalk_5millionPoints_Signal : PlotDemo, IPlotDemo
        {
            public string name { get; } = "5M points (Signal)";
            public string description { get; } = "Signal plots with millions of points can be interacted with in real time.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;
                int lineCount = 5;

                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));
            }
        }
    }
}
