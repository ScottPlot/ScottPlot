using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ScottPlot.Drawing;

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

                //Change the color map to turbo, and renumber the axes
                plt.PlotHeatmap(intensities, Colormap.Turbo, axisOffsets: new double[] { -5, -5 }, axisMultipliers: new double[] { 10, 10 });
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
                plt.PlotHeatmap(intensities, Colormap.Turbo, axisOffsets: new double[] { -5, -5 }, axisMultipliers: new double[] { 10, 10 }, scaleMin: -150, scaleMax: 300);
            }
        }
        public class HeatmapFromXYGaussian : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Heatmap From XY Data (Gaussian)";
            public string description { get; } = "Useful for showing clusters of points";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                //Some noisy data centred around the middle
                int[] xs = DataGen.RandomNormal(rand, 10000, 25, 10).Select(x => (int)x).ToArray();
                int[] ys = DataGen.RandomNormal(rand, 10000, 25, 10).Select(y => (int)y).ToArray();

                //Standard Deviation of 4
                double[,] intensities = Tools.XYToIntensities(Tools.IntensityMode.gaussian, xs, ys, 50, 50, 4);
                plt.PlotHeatmap(intensities);
            }
        }

        public class HeatmapFromXYDensity : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Heatmap From XY Data (Density)";
            public string description { get; } = "Useful for showing clusters of points";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                //Some noisy data centred around the middle
                int[] xs = DataGen.RandomNormal(rand, 10000, 25, 10).Select(x => (int)x).ToArray();
                int[] ys = DataGen.RandomNormal(rand, 10000, 25, 10).Select(y => (int)y).ToArray();

                //Each cell is a square with side-length of 4
                double[,] intensities = Tools.XYToIntensities(Tools.IntensityMode.density, xs, ys, 50, 50, 4);
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

        public class BackGroundImage : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Heatmap Background Image";
            public string description { get; } = "Heatmap values below a threshold can be replaced by a Bitmap image.";

            public void Render(Plot plt)
            {
                double[,] imageData = DataGen.SampleImageData();

                //This could be applied more usefully to an image pertinent to the data
                //For example a map of the world, if your data is about geographic phenomenon
                Bitmap background = DataGen.SampleImage();

                plt.PlotHeatmap(imageData, transparencyThreshold: 20, backgroundImage: background);
            }
        }
    }
}
