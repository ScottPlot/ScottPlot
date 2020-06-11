using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using ScottPlot.Config.ColorMaps;

#pragma warning disable CS0618 // Type or member is obsolete
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
                plt.PlotHeatmap(intensities, Config.ColorMaps.Colormaps.turbo, axisOffsets: new double[] { -5, -5 }, axisMultipliers: new double[] { 10, 10 });
            }
        }

        public class HeatmapCustomScale : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Custom Scale";
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

                //You'll notice these are the same settings as the previous demo, except the axis scale is noticably different
                plt.PlotHeatmap(intensities, Config.ColorMaps.Colormaps.turbo, axisOffsets: new double[] { -5, -5 }, axisMultipliers: new double[] { 10, 10 }, scaleMin: -150, scaleMax: 300);
            }
        }
        public class HeatmapFromXY : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Heatmap From XY Data";
            public string description { get; } = "Useful for showing clusters of points";

            public void Render(Plot plt)
            {
                Random rand = new Random();
                // 1000 points on the interval [0, 49]
                int[] xs = DataGen.RandomInts(rand, 1000, 50);
                int[] ys = DataGen.RandomInts(rand, 1000, 50);

                //Standard Deviation of 12
                double[,] intensities = Tools.XYToIntensities(Tools.IntensityMode.gaussian, xs, ys, 50, 50, 12);
                //This function also supports densities (amount in a square of size n)
                plt.PlotHeatmap(intensities);
            }
        }


        public class HeatmapImage : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Heatmap Image";
            public string description { get; } = "A sample image displayed using a heatmap";

            public void Render(Plot plt)
            {
                double[,] imageData = DataGen.SampleImageData();
                plt.PlotHeatmap(imageData);
            }
        }
    }
}
