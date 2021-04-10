using System;
using System.Linq;

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
            double[,] data2D = { { 1, 2, 3 },
                                 { 4, 5, 6 } };

            plt.AddHeatmap(data2D);
        }
    }

    public class HeatmapColorbar : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_colorbar";
        public string Title => "Heatmap with Colorbar";
        public string Description =>
            "Colorbars are often added when heatmaps are used.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] data2D = { { 1, 2, 3 },
                                 { 4, 5, 6 } };

            var hm = plt.AddHeatmap(data2D);
            var cb = plt.AddColorbar(hm);
        }
    }

    public class HeatmapImage : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_image";
        public string Title => "Heatmap Image";
        public string Description =>
            "Image data can be plotted using the heatmap plot type.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = DataGen.SampleImageData();
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

            var hm = plt.AddHeatmap(intensities);
            var cb = plt.AddColorbar(hm);
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

            var hm = plt.AddHeatmap(intensities, Drawing.Colormap.Turbo);
            var cb = plt.AddColorbar(hm);
        }
    }

    public class HeatmapLimitScale : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_limitScale";
        public string Title => "Scale Limits";
        public string Description =>
            "Heatmap colormap scale can use a defined min/max value.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] intensities = new double[100, 100];
            for (int x = 0; x < 100; x++)
                for (int y = 0; y < 100; y++)
                    intensities[x, y] = (Math.Sin(x * .2) + Math.Cos(y * .2)) * 100;

            var hm = plt.AddHeatmap(intensities);
            hm.Update(intensities, min: 0, max: 200);

            var cb = plt.AddColorbar(hm);
        }
    }

    public class HeatmapDensity : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_density";
        public string Title => "Interpolation by Density";
        public string Description =>
            "Heatmaps can be created from random 2D data points using the count within a square of fixed size.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int[] xs = DataGen.RandomNormal(rand, 10000, 25, 10).Select(x => (int)x).ToArray();
            int[] ys = DataGen.RandomNormal(rand, 10000, 25, 10).Select(y => (int)y).ToArray();

            double[,] intensities = Tools.XYToIntensities(mode: IntensityMode.Density,
                xs: xs, ys: ys, width: 50, height: 50, sampleWidth: 4);

            var hm = plt.AddHeatmap(intensities);
            var cb = plt.AddColorbar(hm);
        }
    }

    public class HeatmapGaussian : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_gaussian";
        public string Title => "Gaussian Interpolation";
        public string Description =>
            "Heatmaps can be created from 2D data points using bilinear interpolation with Gaussian weighting. " +
            "This option results in a heatmap with a standard deviation of 4.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int[] xs = DataGen.RandomNormal(rand, 10000, 25, 10).Select(x => (int)x).ToArray();
            int[] ys = DataGen.RandomNormal(rand, 10000, 25, 10).Select(y => (int)y).ToArray();

            double[,] intensities = Tools.XYToIntensities(mode: IntensityMode.Gaussian,
                xs: xs, ys: ys, width: 50, height: 50, sampleWidth: 4);

            var hm = plt.AddHeatmap(intensities);
            var cb = plt.AddColorbar(hm);
        }
    }

    public class HeatmapCoordinated : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_coordinated";
        public string Title => "Coordinated Heatmap";
        public string Description =>
            "CoordinatedHeatmap is type of Heatmap stretched to fit user-defined boundaries in coordinate space. " +
            "This plot type displays two-dimensional intensities on a rectangular surface according to a colormap.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int[] xs = DataGen.RandomNormal(rand, 10000, 25, 10).Select(x => (int)x).ToArray();
            int[] ys = DataGen.RandomNormal(rand, 10000, 25, 10).Select(y => (int)y).ToArray();

            double[,] intensities = Tools.XYToIntensities(mode: IntensityMode.Gaussian,
                xs: xs, ys: ys, width: 50, height: 50, sampleWidth: 4);

            var hmc = plt.AddHeatmapCoordinated(intensities, xMin: -100, xMax: 500, yMin: 200, yMax: 201);
            var cb = plt.AddColorbar(hmc);
        }
    }

    public class HeatmapTransparent : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_transparent";
        public string Title => "Heatmap with Empty Squares";
        public string Description =>
            "You can use a 2D array of nullable doubles to indicate some squares do not contain data. " +
            "This allows the user to display heatmaps with transparency and implement non-rectangular heatmaps.";

        public void ExecuteRecipe(Plot plt)
        {
            double?[,] intensities = {
                { 1, 7, 4, null },
                { 9, null, 2, 4 },
                { 1, 4, null, 8 },
                { null, 2, 4, null }
            };

            var hmc = plt.AddHeatmap(intensities);
            var cb = plt.AddColorbar(hmc);
        }
    }
}
