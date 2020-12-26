using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class Colorbar : IRecipe
    {
        public string Category => "Plottable: Colorbar";
        public string ID => "colorbar_quickstart";
        public string Title => "Colorbar";
        public string Description =>
            "A colorbar displays a colormap beside the data area. " +
            "Colorbars are typically added to plots containing heatmaps.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddColorbar();
        }
    }

    public class ColorbarColormap : IRecipe
    {
        public string Category => "Plottable: Colorbar";
        public string ID => "colorbar_colormap";
        public string Title => "Colorbar for Colormap";
        public string Description =>
            "By default colorbars use the Viridis colormap, but this behavior can be customized and many colormaps are available.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddColorbar(Drawing.Colormap.Turbo);
        }
    }

    public class ColorbarTicks : IRecipe
    {
        public string Category => "Plottable: Colorbar";
        public string ID => "colorbar_ticks";
        public string Title => "Colorbar Ticks";
        public string Description =>
            "Tick marks can be added to colorbars. Each tick is described by a position " +
            "(a fraction of the distance from the bottom to the top) and a string (the tick label).";

        public void ExecuteRecipe(Plot plt)
        {
            var cb = plt.AddColorbar();
            cb.AddTick(0, "-123");
            cb.AddTick(1, "+123");
            cb.AddTick(.5, "0");
            cb.AddTick(.25, "-61.5");
            cb.AddTick(.75, "+61.5");
        }
    }
}
