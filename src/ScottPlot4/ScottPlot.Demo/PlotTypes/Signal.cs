using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Signal
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Signal Plot Quickstart";
            public string description { get; } = "Signal plots are ideal for evenly-spaced data with thousands or millions of points.";

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

                plt.Title($"Styled Signal Plot");
                plt.PlotSignal(ys, yOffset: 40, label: "default");
                plt.PlotSignal(ys, yOffset: 20, label: "pink dashed",
                    color: Color.Magenta, lineStyle: LineStyle.Dash);
                plt.PlotSignal(ys, yOffset: 00, lineWidth: 3, label: "thick solid");
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

        public class SaveData : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Save signal plot data";
            public string description { get; } = "Many plot types have a .SaveCSV() method";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] dataSin = DataGen.Sin(pointCount);

                var scatter = plt.PlotSignal(dataSin);
                scatter.SaveCSV("signal.csv");
            }
        }

        public class Density : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Display data density";
            public string description { get; } = "When plotting extremely high density data, you can't always see the trends underneath all those overlapping data points. If you send an array of colors to PlotSignal(), it will use those colors to display density.";

            public void Render(Plot plt)
            {
                // create an extremely noisy signal with a subtle sine wave beneath it
                Random rand = new Random(0);
                int pointCount = 100_000;
                double[] signal1 = ScottPlot.DataGen.Sin(pointCount, 3);
                double[] noise = ScottPlot.DataGen.RandomNormal(rand, pointCount, 0, 5);
                double[] data = new double[pointCount];
                for (int i = 0; i < data.Length; i++)
                    data[i] = signal1[i] + noise[i];

                // plot the noisy signal using the traditional method
                plt.PlotSignal(data, yOffset: -40, color: Color.Red);

                // use a color array for displaying data from low to high density
                Color[] colors = new Color[]
                {
                    ColorTranslator.FromHtml("#440154"),
                    ColorTranslator.FromHtml("#39568C"),
                    ColorTranslator.FromHtml("#1F968B"),
                    ColorTranslator.FromHtml("#73D055"),
                };

                plt.PlotSignal(data, colorByDensity: colors);

                plt.Title("Color by Density vs. Solid Color");
                plt.AxisAuto(0, .1);
            }
        }

        public class FirstNPoints : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Display first N points";
            public string description { get; } = "When plotting live data it is useful to allocate a large array in memory then fill it with values as they come in. By setting the maxRenderIndex property of a scatter plot to can prevent rendering the end of the array (which is probably filled with zeros).";

            public void Render(Plot plt)
            {
                // Allocate memory for a large number of data points
                double[] data = new double[1_000_000]; // start with all zeros

                // Only populate the first few points with real data
                Random rand = new Random(0);
                int lastValueIndex = 1234;
                for (int i = 1; i <= lastValueIndex; i++)
                    data[i] = data[i - 1] + rand.NextDouble() - .5;

                // A regular Signal plot would display a little data at the start but mostly zeros.
                // Using the maxRenderIndex argument allows one to just plot the first N data points.
                var sig = plt.PlotSignal(data, maxRenderIndex: 500);
                plt.Title("Partial Display of a 1,000,000 Element Array");
                plt.YLabel("Value");
                plt.XLabel("Array Index");

                // you can change the points to plot later (useful for live plots of incoming data)
                sig.maxRenderIndex = 1234;
                plt.AxisAuto();
            }
        }

        public class PlotRange : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot a Range of Points";
            public string description { get; } = "It is sometimes useful to only display values within a range of the source data array.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] data = DataGen.RandomWalk(rand, 100_000);

                plt.PlotSignal(data, minRenderIndex: 4000, maxRenderIndex: 5000);

                plt.Title($"Partial Display of a {data.Length} values");
                plt.YLabel("Value");
                plt.XLabel("Array Index");
            }
        }
    }
}
