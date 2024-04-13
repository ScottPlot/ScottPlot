using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class HeatmapQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
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

    public class HeatmapFlip : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_flip";
        public string Title => "Flipped Heatmap";
        public string Description =>
            "Heatmaps can be flipped vertically and/or horizontally.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] data = ScottPlot.DataGen.SampleImageData();

            var hm1 = plt.AddHeatmap(data, lockScales: false);
            hm1.XMin = 0;

            var hm2 = plt.AddHeatmap(data, lockScales: false);
            hm2.XMin = 100;
            hm2.FlipHorizontally = true;

            var hm3 = plt.AddHeatmap(data, lockScales: false);
            hm3.XMin = 200;
            hm3.FlipVertically = true;

            var hm4 = plt.AddHeatmap(data, lockScales: false);
            hm4.XMin = 300;
            hm4.FlipVertically = true;
            hm4.FlipHorizontally = true;
        }
    }

    public class HeatmapMargins : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_margins";
        public string Title => "Heatmap with Tight Margins";
        public string Description =>
            "The heatmap can fit the plot area exactly if margins are " +
            "set to zero and the square axis lock is disabled.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] data2D = { { 1, 2, 3 },
                                 { 4, 5, 6 } };

            plt.AddHeatmap(data2D, lockScales: false);
            plt.Margins(0, 0);
        }
    }

    public class HeatmapColorbar : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_colorbar";
        public string Title => "Heatmap with Colorbar";
        public string Description =>
            "Colorbars are often added when heatmaps are used.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] data2D = { { 1, 2, 3 },
                                 { 4, 5, 6 } };

            var hm = plt.AddHeatmap(data2D, lockScales: false);
            var cb = plt.AddColorbar(hm);
            plt.Margins(0, 0);
        }
    }

    public class HeatmapSmooth : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_smooth";
        public string Title => "Smooth Heatmap";
        public string Description =>
            "Heatmaps display values as rectangles with sharp borders by default. " +
            "Enabling the Smooth feature uses bicubic interpolation to display the heatmap " +
            "as a smooth gradient between values.";

        public void ExecuteRecipe(Plot plt)
        {
            var rand = new Random(0);
            double[,] data2D = DataGen.Random2D(rand, 5, 4);

            var hm = plt.AddHeatmap(data2D, lockScales: false);
            hm.Smooth = true;
        }
    }

    public class HeatmapImage : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
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

    public class FramelessHeatmap : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_frameless";
        public string Title => "Frameless Heatmap";
        public string Description =>
            "Disable the frame and set margins to zero to create a heatmap plot that fills the entire image.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = DataGen.SampleImageData();
            plt.AddHeatmap(imageData, lockScales: false);
            plt.Frameless();
            plt.Margins(0, 0);
        }
    }

    public class HeatmapOpacity : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_opacity";
        public string Title => "Heatmap Opacity";
        public string Description => "Heatmaps have an Opacity property " +
            "that can be set anywhere from 0 (transparent) to 1 (opaque).";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = DataGen.SampleImageData();
            var hm = plt.AddHeatmap(imageData);
            hm.Opacity = 0.5;
        }
    }

    public class SingleColorHeatmap : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_single_color";
        public string Title => "Single Color Heatmap";
        public string Description =>
            "A single-color heatmap can be created where cell transparency is defined by a 2D array containing values 0 to 1.";

        public void ExecuteRecipe(Plot plt)
        {
            double?[,] data = DataGen.SampleImageDataNullable();

            var hm1 = plt.AddHeatmap(Color.Red, data, lockScales: false);
            hm1.OffsetX = 0;
            hm1.OffsetY = 0;

            var hm2 = plt.AddHeatmap(Color.Green, data, lockScales: false);
            hm2.OffsetX = 30;
            hm2.OffsetY = 20;

            var hm3 = plt.AddHeatmap(Color.Blue, data, lockScales: false);
            hm3.OffsetX = 60;
            hm3.OffsetY = 40;
        }
    }

    public class Heatmap2dWaveform : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
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
        public ICategory Category => new Categories.PlotTypes.Heatmap();
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

    public class StyledHeatmapColormap : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "styled_heatmap_colormap";
        public string Title => "Styled Colormap";
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

            plt.Style(Style.Black);
        }
    }

    public class HeatmapPaletteColormap : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_palette";
        public string Title => "Palette Colormap";
        public string Description =>
            "Heatmap data can be presented using a colormap defined by a fixed set of colors.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] data = DataGen.SampleImageData();

            // create a colormap from a defined set of colors
            Color[] colors = { Color.Indigo, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Red, };

            // display the colormap on the plot as a colorbar
            ScottPlot.Drawing.Colormap cmap = new(colors);
            var cbar = plt.AddColorbar(cmap);
            cbar.MaxValue = 255;

            // use custom tick positions
            double[] tickPositions = Enumerable.Range(0, colors.Length + 1)
                .Select(x => (double)x / colors.Length)
                .ToArray();
            string[] tickLabels = tickPositions.Select(x => $"{x * 255:N2}").ToArray();
            cbar.SetTicks(tickPositions, tickLabels);

            // add a heatmap using the custom colormap
            plt.AddHeatmap(data, cmap);
        }
    }

    public class HeatmapLimitScale : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
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

            // scale the colors between 0 and 200
            var hm = plt.AddHeatmap(intensities);
            hm.Update(intensities, min: 0, max: 200);

            // add a colorbar with custom ticks
            var cb = plt.AddColorbar(hm);
            double[] tickPositions = ScottPlot.DataGen.Range(0, 200, 25, true);
            string[] tickLabels = tickPositions.Select(x => x.ToString()).ToArray();
            cb.SetTicks(tickPositions, tickLabels, min: 0, max: 200);
        }
    }

    public class HeatmapClip : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_clip";
        public string Title => "Color Clipping";
        public string Description =>
            "The value range displayed by the colormap can restricted to a narrow subset of the full data range. " +
            "Tick labels at the edges of the colorbar can be made to show inequality symbols to indicate " +
            "the range of data is being clipped when translating values to colors.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = DataGen.SampleImageData();
            var heatmap = plt.AddHeatmap(imageData);
            heatmap.Update(imageData, min: 75, max: 125);

            var cb = plt.AddColorbar(heatmap);

            // configure the colorbar to display inequality operators at the edges
            cb.MaxIsClipped = true;
            cb.MinIsClipped = true;
        }
    }

    public class HeatmapDensity : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
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
        public ICategory Category => new Categories.PlotTypes.Heatmap();
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

    public class HeatmapDimensions : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_dimensions";
        public string Title => "Custom Dimensions";
        public string Description =>
            "By default heatmaps start at the origin and each rectangle (cell) is 1 unit in size, but " +
            "heatmap offset and cell size can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] data2D = { { 1, 2, 3 },
                                 { 4, 5, 6 } };

            var hm = plt.AddHeatmap(data2D, lockScales: false);
            hm.OffsetX = 10;
            hm.OffsetY = 20;
            hm.CellWidth = 5;
            hm.CellHeight = 10;
        }
    }

    public class HeatmapTransparent : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
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

    public class HeatmapSemiTransparent : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_semitransparent";
        public string Title => "Heatmap with Semitransparent Squares";
        public string Description =>
            "The intensities of heatmaps are mapped to color, " +
            "but an optional 2D array of alpha values may be provided " +
            "to separately control transparency of squares.";

        public void ExecuteRecipe(Plot plt)
        {
            double?[,] values = {
                { 1, 7, 4, 5 },
                { 9, 3, 2, 4 },
                { 1, 4, 5, 8 },
                { 7, 2, 4, 2 }
            };

            double?[,] opacities = {
                { 1, 1, 1, 1 },
                { 1, 0, 1, 0 },
                { 1, .75, .5, 0 },
                { 1, .8, .6, .4 }
            };

            var hm = plt.AddHeatmap(values);
            hm.Update(values, opacity: opacities);

            plt.AddColorbar(hm);
        }
    }

    public class HeatmapPlacement : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_placement";
        public string Title => "Size and Placement";
        public string Description =>
            "Edges of the heatmap can be defined as an alternative to defining offset and cell size,";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = DataGen.SampleImageData();
            var hm = plt.AddHeatmap(imageData, lockScales: false);

            hm.XMin = -100;
            hm.XMax = 100;
            hm.YMin = -10;
            hm.YMax = 10;
        }
    }

    public class HeatmapRotation : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_rotation";
        public string Title => "Heatmap Rotation";
        public string Description =>
            "A Heatmap can be rotated clockwise around around a user-specified center of rotation. " +
            "Locking axis scales to enforce square pixels is recommended. " +
            "Rotation occurs after any flipping operations.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = DataGen.SampleImageData();

            for (int i = 0; i < 5; i++)
            {
                var hm = plt.AddHeatmap(imageData, lockScales: true);
                hm.XMin = 0;
                hm.XMax = 1;
                hm.YMin = 0;
                hm.YMax = 1;
                hm.Rotation = i * 10;
            }

            for (int i = 0; i < 5; i++)
            {
                var hm = plt.AddHeatmap(imageData, lockScales: true);
                hm.XMin = 2;
                hm.XMax = 3;
                hm.YMin = 0;
                hm.YMax = 1;

                hm.CenterOfRotation = Alignment.MiddleCenter;

                hm.Rotation = i * 10;
            }

            plt.SetAxisLimits(-1, 4, -1, 2);
        }
    }

    public class HeatmapClipping : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_clipping";
        public string Title => "Heatmap Clipping";
        public string Description =>
            "Heatmaps can be clipped to an arbitrary polygon";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = DataGen.SampleImageData();
            var hm = plt.AddHeatmap(imageData, lockScales: false);
            hm.ClippingPoints = new Coordinate[]
            {
                new Coordinate(30, 15),
                new Coordinate(55, 40),
                new Coordinate(60, 45),
                new Coordinate(80, 60),
                new Coordinate(40, 95),
                new Coordinate(15, 90),
                new Coordinate(5, 50),
            };
        }
    }

    public class BinnedHeatmap : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_binned";
        public string Title => "Binned Histogram";
        public string Description =>
            "Binned histograms are 2D heatmaps that use a colormap to display cell counts. " +
            "Charts like this are commonly used in scientific and medical applications.";

        public void ExecuteRecipe(Plot plt)
        {
            // create a binned histogram
            var hist2d = plt.AddBinnedHistogram(100, 100);

            // data is a collection of X/Y points
            Coordinate[] flowData = DataGen.FlowCytometry();

            // add X/Y points to the histogram
            hist2d.AddRange(flowData);
        }
    }

    public class HeatmapParallel : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_parallel";
        public string Title => "Parallel Processing";
        public string Description =>
            "Heatmaps have opt-in parallel processing which may improve performance when calling Update() for large datasets.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] data = Generate.Sin2D(width: 1_000, height: 1_000);
            var hm = plt.AddHeatmap(data, lockScales: false);

            // opt into parallel processing
            hm.UseParallel = true;
        }
    }

    public class HeatmapInverted : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Heatmap();
        public string ID => "heatmap_inverted";
        public string Title => "Inverted Heatmap";
        public string Description =>
            "An inverted heatmap can be created by reversing the colors in the colormap.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] data = DataGen.SampleImageData();

            var hm1 = plt.AddHeatmap(data, lockScales: false);
            hm1.Update(data, ScottPlot.Drawing.Colormap.Turbo);

            var hm2 = plt.AddHeatmap(data, lockScales: false);
            hm2.XMin = 75;
            hm2.Update(data, ScottPlot.Drawing.Colormap.Turbo.Reversed());
        }
    }
}
