using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

/* 
 * Palettes are collections of colors that control the default colors for new plottables added to the plot.
 * This file contains code that bridges the gap between ScottPlot 4 Colorsets and ScottPlot 5 Palettes.
 */

namespace ScottPlot
{
    /* This module will be expanded in ScottPlot 5 */
    public static class Palette
    {
        public static ScottPlot.Drawing.Palette Amber => new(new ScottPlot.Drawing.Colorsets.Amber());
        public static ScottPlot.Drawing.Palette Aurora => new(new ScottPlot.Drawing.Colorsets.Aurora());
        public static ScottPlot.Drawing.Palette Category10 => new(new ScottPlot.Drawing.Colorsets.Category10());
        public static ScottPlot.Drawing.Palette Category20 => new(new ScottPlot.Drawing.Colorsets.Category20());
        public static ScottPlot.Drawing.Palette ColorblindFriendly => new(new ScottPlot.Drawing.Colorsets.ColorblindFriendly());
        public static ScottPlot.Drawing.Palette Dark => new(new ScottPlot.Drawing.Colorsets.Dark());
        public static ScottPlot.Drawing.Palette DarkPastel => new(new ScottPlot.Drawing.Colorsets.DarkPastel());
        public static ScottPlot.Drawing.Palette Frost => new(new ScottPlot.Drawing.Colorsets.Frost());
        public static ScottPlot.Drawing.Palette Microcharts => new(new ScottPlot.Drawing.Colorsets.Microcharts());
        public static ScottPlot.Drawing.Palette Nero => new(new ScottPlot.Drawing.Colorsets.Nero());
        public static ScottPlot.Drawing.Palette Nord => new(new ScottPlot.Drawing.Colorsets.Nord());
        public static ScottPlot.Drawing.Palette OneHalf => new(new ScottPlot.Drawing.Colorsets.OneHalf());
        public static ScottPlot.Drawing.Palette OneHalfDark => new(new ScottPlot.Drawing.Colorsets.OneHalfDark());
        public static ScottPlot.Drawing.Palette PolarNight => new(new ScottPlot.Drawing.Colorsets.PolarNight());
        public static ScottPlot.Drawing.Palette Redness => new(new ScottPlot.Drawing.Colorsets.Redness());
        public static ScottPlot.Drawing.Palette SnowStorm => new(new ScottPlot.Drawing.Colorsets.Snowstorm());
        public static ScottPlot.Drawing.Palette Tsitsulin => new(new ScottPlot.Drawing.Colorsets.Tsitsulin());

        /// <summary>
        /// Create a new color palette from an array of HTML colors
        /// </summary>
        public static ScottPlot.Drawing.Palette FromHtmlColors(string[] htmlColors)
        {
            return new ScottPlot.Drawing.Palette(htmlColors);
        }

        /// <summary>
        /// Return an array containing every available style
        /// </summary>
        public static ScottPlot.Drawing.Palette[] GetPalettes()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetInterfaces().Contains(typeof(ScottPlot.Drawing.IPalette)))
                .Select(x => (ScottPlot.Drawing.IPalette)FormatterServices.GetUninitializedObject(x))
                .Select(x => new ScottPlot.Drawing.Palette(x))
                .Where(x => x.Count() > 0)
                .ToArray();
        }
    }
}
