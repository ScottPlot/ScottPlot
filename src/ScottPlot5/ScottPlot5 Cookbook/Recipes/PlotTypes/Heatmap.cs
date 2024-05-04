namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Heatmap : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Heatmap";
    public string CategoryDescription => "Heatmaps display values from 2D data " +
        "as an image with cells of different intensities";

    public class HeatmapQuickstart : RecipeBase
    {
        public override string Name => "Heatmap Quickstart";
        public override string Description => "Heatmaps can be created from 2D arrays";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();
            myPlot.Add.Heatmap(data);
        }
    }

    public class HeatmapInverted : RecipeBase
    {
        public override string Name => "Inverted Heatmap";
        public override string Description => "Heatmaps can be inverted by " +
            "reversing the order of colors in the colormap";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Colormap = new ScottPlot.Colormaps.Viridis();
            hm1.Position = new(0, 65, 0, 100);

            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Colormap = new ScottPlot.Colormaps.Viridis().Reversed();
            hm2.Position = new(100, 165, 0, 100);
        }
    }

    public class HeatmapColormap : RecipeBase
    {
        public override string Name => "Heatmap with custom Colormap";
        public override string Description => "A heatmap's Colormap is the logic " +
            "used to convert from cell value to cell color and they can set by the user. " +
            "ScottPlot comes with many common colormaps, " +
            "but users may implement IColormap and apply their own. " +
            "A colorbar can be added to indicate which colors map to which values.";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Colormap = new ScottPlot.Colormaps.Turbo();

            myPlot.Add.ColorBar(hm1);
        }
    }

    public class HeatmapMultipleColorbar : RecipeBase
    {
        public override string Name => "Multiple Colorbars";
        public override string Description => "Multiple colorbars may be added to plots.";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Extent = new(0, 1, 0, 1);
            hm1.Colormap = new ScottPlot.Colormaps.Turbo();
            myPlot.Add.ColorBar(hm1);

            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Extent = new(1.5, 2.5, 0, 1);
            hm2.Colormap = new ScottPlot.Colormaps.Viridis();
            myPlot.Add.ColorBar(hm2);
        }
    }

    public class ColorbarTitle : RecipeBase
    {
        public override string Name => "Colorbar Title";
        public override string Description => "A colorbar displays a colormap " +
            "on an edge of the plot, and it has an optional label which can " +
            "be customized to display a title.";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            var hm = myPlot.Add.Heatmap(data);
            hm.Colormap = new ScottPlot.Colormaps.Turbo();

            var cb = myPlot.Add.ColorBar(hm);
            cb.Label = "Intensity";
            cb.LabelStyle.FontSize = 24;
        }
    }

    public class HeatmapFlip : RecipeBase
    {
        public override string Name => "Flipped Heatmap";
        public override string Description => "Heatmaps can be flipped horizontally and/or vertically";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            myPlot.Add.Text("default", 0, 1.5);
            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Position = new CoordinateRect(0, 1, 0, 1);

            myPlot.Add.Text("flip X", 2, 1.5);
            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Position = new CoordinateRect(2, 3, 0, 1);
            hm2.FlipHorizontally = true;

            myPlot.Add.Text("flip Y", 4, 1.5);
            var hm3 = myPlot.Add.Heatmap(data);
            hm3.Position = new CoordinateRect(4, 5, 0, 1);
            hm3.FlipVertically = true;

            myPlot.Add.Text("flip X&Y", 6, 1.5);
            var hm4 = myPlot.Add.Heatmap(data);
            hm4.Position = new CoordinateRect(6, 7, 0, 1);
            hm4.FlipHorizontally = true;
            hm4.FlipVertically = true;

            myPlot.Axes.SetLimits(-.5, 7.5, -1, 2);
        }
    }

    public class HeatmapSmooth : RecipeBase
    {
        public override string Name => "Smooth Heatmap";
        public override string Description => "Enable the `Smooth` property for anti-aliased rendering";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            myPlot.Add.Text("Smooth = false", 0, 1.1);
            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Position = new CoordinateRect(0, 1, 0, 1);

            myPlot.Add.Text("Smooth = true", 1.1, 1.1);
            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Position = new CoordinateRect(1.1, 2.1, 0, 1);
            hm2.Smooth = true;
        }
    }

    public class HeatmapTransparentCells : RecipeBase
    {
        public override string Name => "Transparent Cells";
        public override string Description => "Assign double.NaN to a heatmap cell to make it transparent.";

        [Test]
        public override void Execute()
        {
            // start with 2D data and set some cells to NaN
            double[,] data = SampleData.MonaLisa();
            for (int y = 20; y < 80; y++)
            {
                for (int x = 20; x < 60; x++)
                {
                    data[y, x] = double.NaN;
                }
            }

            // create a line chart
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // plot the heatmap on top of the line chart
            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Position = new(10, 35, -1.5, .5);

            // the NaN transparency color can be customized
            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Position = new(40, 55, -.5, .75);
            hm2.NaNCellColor = Colors.Magenta.WithAlpha(.4);
        }
    }

    public class HeatmapGlobalTransparency : RecipeBase
    {
        public override string Name => "Global Transparency";
        public override string Description => "The transparency of the entire heatmap can be adjusted.";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            // create a line chart
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // plot the heatmap on top of the line chart
            var hm = myPlot.Add.Heatmap(data);
            hm.Position = new(10, 35, -1.5, .5);
            hm.Opacity = 0.5;
        }
    }

    public class HeatmapAlphaMap : RecipeBase
    {
        public override string Name => "Alpha Map";
        public override string Description => "An alpha map (a 2d array of byte values) can be used to " +
            "apply custom transparency to each cell of a heatmap.";

        [Test]
        public override void Execute()
        {
            // data values are translated to color based on the heatmap's colormap
            double[,] data = SampleData.MonaLisa();

            // an alpha map controls transparency of each cell
            byte[,] alphaMap = new byte[data.GetLength(0), data.GetLength(1)];

            // fill the alpha map with values from 0 (transparent) to 255 (opaque)
            for (int y = 0; y < alphaMap.GetLength(0); y++)
            {
                for (int x = 0; x < alphaMap.GetLength(1); x++)
                {
                    double fractionAcross = (double)x / alphaMap.GetLength(1);
                    alphaMap[y, x] = (byte)(fractionAcross * 255);
                }
            }

            // create a line chart
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // plot the heatmap on top of the line chart
            var hm = myPlot.Add.Heatmap(data);
            hm.Position = new(10, 35, -1.5, .5);
            hm.AlphaMap = alphaMap;
        }
    }
}
