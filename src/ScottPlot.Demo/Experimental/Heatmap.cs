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
                int[] xs = Enumerable.Range(0, 100).ToArray();
                int[] ys = Enumerable.Range(0, 100).ToArray();

                double[,] intensities = new double[ys.Length, xs.Length];

                for (int i = 0; i < ys.Length; i++)
                {
                    for (int j = 0; j < xs.Length; j++)
                    {
                        intensities[i, j] = Math.Sqrt(Math.Pow(ys[i] - 50, 2) + Math.Pow(xs[j] - 50, 2));
                    }
                }

                plt.PlotHeatmap(intensities);
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
    }
}
