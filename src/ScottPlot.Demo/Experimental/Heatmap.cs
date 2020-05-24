using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Demo.Experimental
{
    public static class Heatmap
    {
        public class HeatmapQuickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Heatmap Quickstart";
            public string description { get; } = "Heatmaps are a good way to show intensity data.";

            public void Render(Plot plt)
            {
                double[,] imageData = { { 1, 2, 3 },
                                        { 4, 5, 6 } };

                plt.PlotHeatmap(imageData);

                plt.Axis(
                    x1: -1, x2: imageData.GetLength(0),
                    y1: -1, y2: imageData.GetLength(1));
            }
        }

        public class HeatmapSinCos : PlotDemo, IPlotDemo
        {
            public string name { get; } = "2D Waveform";
            public string description { get; } = "Another heatmap example.";

            public void Render(Plot plt)
            {
                int[] xs = Enumerable.Range(0, 100).ToArray();
                int[] ys = Enumerable.Range(0, 100).ToArray();

                double[,] intensities = new double[ys.Length, xs.Length];
                for (int i = 0; i < ys.Length; i++)
                {
                    for (int j = 0; j < xs.Length; j++)
                    {
                        intensities[i, j] = (Math.Sin(i * .2) + Math.Cos(j * .2)) * 100;
                    }
                }

                plt.PlotHeatmap(intensities);
            }
        }

        public class HeatmapCustomizability : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Customizing Heatmaps";
            public string description { get; } = "Heatmaps have a lot of customization options";

            public void Render(Plot plt)
            {
                Random rand = new Random();
                int[] xs = Enumerable.Range(0, 100).ToArray();
                int[] ys = Enumerable.Range(0, 100).ToArray();

                double[,] intensities = new double[ys.Length, xs.Length];

                for (int i = 0; i < ys.Length; i++)
                {
                    for (int j = 0; j < xs.Length; j++)
                    {
                        intensities[i, j] = (Math.Sin(i * .2) + Math.Cos(j * .2)) * 100;
                    }
                }

                //Change the color map, and renumber the axes
                plt.PlotHeatmap(intensities, PlottableHeatmap.ColorMap.turbo, axisOffsets: new double[] { -5, -5 }, axisMultipliers: new double[] { 10, 10 });
            }
        }
    }
}
