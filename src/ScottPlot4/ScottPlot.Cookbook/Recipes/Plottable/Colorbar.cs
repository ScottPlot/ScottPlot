using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class Colorbar : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Colorbar();
        public string ID => "colorbar_quickstart";
        public string Title => "Colorbar";
        public string Description =>
            "A colorbar displays a colormap beside the data area. " +
            "Colorbars are typically added to plots containing heatmaps.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddColorbar();

            // direct attention to the colorbar
            var text = plt.AddText("Colorbar", 5, 0, 24, Color.Red);
            text.Alignment = Alignment.MiddleRight;
            plt.AddArrow(7, 0, 5, 0, color: Color.Red);
            plt.SetAxisLimits(-10, 10, -10, 10);
        }
    }

    public class ColorbarColormap : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Colorbar();
        public string ID => "colorbar_colormap";
        public string Title => "Colorbar for Colormap";
        public string Description =>
            "By default colorbars use the Viridis colormap, but this behavior can be customized and many colormaps are available.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddColorbar(Drawing.Colormap.Turbo);

            // direct attention to the colorbar
            var text = plt.AddText("Colorbar", 5, 0, 24, Color.Red);
            text.Alignment = Alignment.MiddleRight;
            plt.AddArrow(7, 0, 5, 0, color: Color.Red);
            plt.SetAxisLimits(-10, 10, -10, 10);
        }
    }

    public class ColorbarTicks : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Colorbar();
        public string ID => "colorbar_ticks";
        public string Title => "Colorbar Ticks";
        public string Description =>
            "Tick marks can be added to colorbars. Each tick is described by a position " +
            "(a fraction of the distance from the bottom to the top) and a string (the tick label).";

        public void ExecuteRecipe(Plot plt)
        {
            var cb = plt.AddColorbar(Drawing.Colormap.Turbo);

            // Add manual ticks (disabling automatic ticks)
            cb.AddTick(0, "-123");
            cb.AddTick(1, "+123");
            cb.AddTick(.5, "0");
            cb.AddTick(.25, "-61.5");
            cb.AddTick(.75, "+61.5");

            // To re-enable automatic ticks call cb.AutomaticTicks(true)

            // direct attention to the colorbar
            var text = plt.AddText("Colorbar", 5, 0, 24, Color.Red);
            text.Alignment = Alignment.MiddleRight;
            plt.AddArrow(7, 0, 5, 0, color: Color.Red);
            plt.SetAxisLimits(-10, 10, -10, 10);
        }
    }

    public class ColorbarRange : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Colorbar();
        public string ID => "colorbar_Range";
        public string Title => "Color Range";
        public string Description =>
            "You can restrict a colorbar to only show a small range of a colormap. " +
            "In this example we only use the middle of a rainbow colormap.";

        public void ExecuteRecipe(Plot plt)
        {
            var cb = plt.AddColorbar(Drawing.Colormap.Turbo);
            cb.MinValue = -10;
            cb.MaxValue = 10;
            cb.MinColor = .25;
            cb.MaxColor = .75;

            // direct attention to the colorbar
            var text = plt.AddText("Colorbar", 5, 0, 24, Color.Red);
            text.Alignment = Alignment.MiddleRight;
            plt.AddArrow(7, 0, 5, 0, color: Color.Red);
            plt.SetAxisLimits(-10, 10, -10, 10);
        }
    }

    public class ColorbarClip : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Colorbar();
        public string ID => "colorbar_clip";
        public string Title => "Clipped value range";
        public string Description =>
            "If data values extend beyond the min/max range displayed by a colorbar " +
            "you can indicate the colormap is clipping the data values and inequality symbols will be " +
            "displayed in the tick labeles at the edge of the colorbar.";

        public void ExecuteRecipe(Plot plt)
        {
            var cb = plt.AddColorbar(Drawing.Colormap.Turbo);
            cb.MinValue = -10;
            cb.MaxValue = 10;
            cb.MinIsClipped = true;
            cb.MaxIsClipped = true;

            // direct attention to the colorbar
            var text = plt.AddText("Colorbar", 5, 0, 24, Color.Red);
            text.Alignment = Alignment.MiddleRight;
            plt.AddArrow(7, 0, 5, 0, color: Color.Red);
            plt.SetAxisLimits(-10, 10, -10, 10);
        }
    }

    public class ColorbarScatter : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Colorbar();
        public string ID => "colorbar_scatter";
        public string Title => "Scatter Plot with Colorbar";
        public string Description =>
            "This example shows how to add differently colored markers to the plot to " +
            "simulate a scatter plot with points colored according to a colorbar. " +
            "Note that the colormap generates the colors, and that a colorbar just displays a colormap";

        public void ExecuteRecipe(Plot plt)
        {
            var cmap = ScottPlot.Drawing.Colormap.Viridis;
            plt.AddColorbar(cmap);

            Random rand = new(0);
            for (int i = 0; i < 1000; i++)
            {
                double x = ScottPlot.DataGen.RandomNormalValue(rand, mean: 0, stdDev: .5);
                double y = ScottPlot.DataGen.RandomNormalValue(rand, mean: 0, stdDev: .5);
                double colorFraction = Math.Sqrt(x * x + y * y);
                System.Drawing.Color c = ScottPlot.Drawing.Colormap.Viridis.GetColor(colorFraction);
                plt.AddPoint(x, y, c);
            }
        }
    }

    public class ColorbarLeft : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Colorbar();
        public string ID => "colorbar_left";
        public string Title => "Colorbar on Left";
        public string Description =>
            "A colorbar may be added to the left side of the chart";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddColorbar(rightSide: false);
        }
    }

    public class ColorbarLabel : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Colorbar();
        public string ID => "colorbar_label";
        public string Title => "Colorbar Label";
        public string Description => "Colorbars have a Label property similar to X and Y axes.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddColorbar();

            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            var cmap = ScottPlot.Drawing.Colormap.Turbo;
            var cb = plt.AddColorbar(cmap);
            cb.Label = "Custom Colorbar Label";
        }
    }
}
