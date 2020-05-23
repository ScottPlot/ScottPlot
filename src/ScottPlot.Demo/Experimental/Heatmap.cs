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
                int[] xs = Enumerable.Range(0, 200).ToArray();
                int[] ys = Enumerable.Range(0, 70).ToArray();
                double[][] intensities = new double[ys.Length][];

                for (int i = 0; i < ys.Length; i++) {
                    intensities[i] = new double[xs.Length];
                    for (int j = 0; j < xs.Length; j++) {
                        intensities[i][j] = Math.Sqrt(Math.Pow(xs[j] - 100, 2) + Math.Pow(ys[i] - 35, 2));
                    }
                }

                plt.PlotHeatmap(intensities);
                plt.Legend();
            }
        }
    }
}
