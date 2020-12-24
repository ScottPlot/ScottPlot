using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class HeatmapQuickstart : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_quickstart";
        public string Title => "Heatmap Quickstart";
        public string Description =>
            "Heatmaps display a 2D array using a colormap.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = { { 1, 2, 3 },
                                    { 4, 5, 6 } };

            plt.AddHeatmap(imageData);
        }
    }

    public class Heatmap2dWaveform : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_2dWaveform";
        public string Title => "2D Waveform";
        public string Description =>
            "This example demonstrates a heatmap with 1000 tiles";

        public void ExecuteRecipe(Plot plt)
        {
            int width = 100;
            int height = 100;

            double[,] intensities = new double[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    intensities[x, y] = (Math.Sin(x * .2) + Math.Cos(y * .2)) * 100;

            plt.AddHeatmap(intensities);
        }
    }

    public class HeatmapColormap : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_colormap";
        public string Title => "Colormap";
        public string Description =>
            "Viridis is the default colormap, but several alternatives are available.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] intensities = new double[100, 100];
            for (int x = 0; x < 100; x++)
                for (int y = 0; y < 100; y++)
                    intensities[x, y] = (Math.Sin(x * .2) + Math.Cos(y * .2)) * 100;

            var heatmap = plt.AddHeatmap(intensities);
            heatmap.Update(intensities, Drawing.Colormap.Turbo);
        }
    }

    public class HeatmapLimitScale : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_limitScale";
        public string Title => "Colormap";
        public string Description =>
            "Heatmap colormap scale can use a defined min/max value.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] intensities = new double[100, 100];
            for (int x = 0; x < 100; x++)
                for (int y = 0; y < 100; y++)
                    intensities[x, y] = (Math.Sin(x * .2) + Math.Cos(y * .2)) * 100;

            var heatmap = plt.AddHeatmap(intensities);
            heatmap.Update(intensities, min: 0, max: 200);
        }
    }

    public class HeatmapGaussian : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_gaussian";
        public string Title => "Colormap";
        public string Description =>
            "Heatmaps can be created from 2D data points using bilinear interpolation with Gaussian weighting.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int[] xs = DataGen.RandomNormal(rand, 10000, 25, 10).Select(x => (int)x).ToArray();
            int[] ys = DataGen.RandomNormal(rand, 10000, 25, 10).Select(y => (int)y).ToArray();

            double[,] intensities = Tools.XYToIntensities(mode: IntensityMode.Gaussian,
                xs: xs, ys: ys, width: 50, height: 50, sampleWidth: 4);

            var heatmap = plt.AddHeatmap(intensities);
            heatmap.Update(intensities);
        }
    }

    public class HeatmapDensity : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_density";
        public string Title => "Colormap";
        public string Description =>
            "Heatmaps can be created from random 2D data points using bilinear interpolation without weighting.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int[] xs = DataGen.RandomNormal(rand, 10000, 25, 10).Select(x => (int)x).ToArray();
            int[] ys = DataGen.RandomNormal(rand, 10000, 25, 10).Select(y => (int)y).ToArray();

            double[,] intensities = Tools.XYToIntensities(mode: IntensityMode.Density,
                xs: xs, ys: ys, width: 50, height: 50, sampleWidth: 4);

            var heatmap = plt.AddHeatmap(intensities);
            heatmap.Update(intensities);
        }
    }
}
