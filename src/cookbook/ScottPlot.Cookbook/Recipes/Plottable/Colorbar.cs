using System;
using System.Collections.Generic;
using System.Drawing;
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

            // direct attention to the colorbar
            var text = plt.AddText("Colorbar", 5, 0, 24, Color.Red);
            text.Alignment = Alignment.MiddleRight;
            plt.AddArrow(7, 0, 5, 0, color: Color.Red);
            plt.SetAxisLimits(-10, 10, -10, 10);
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

            // direct attention to the colorbar
            var text = plt.AddText("Colorbar", 5, 0, 24, Color.Red);
            text.Alignment = Alignment.MiddleRight;
            plt.AddArrow(7, 0, 5, 0, color: Color.Red);
            plt.SetAxisLimits(-10, 10, -10, 10);
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
        public string Category => "Plottable: Colorbar";
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
        public string Category => "Plottable: Colorbar";
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
}
