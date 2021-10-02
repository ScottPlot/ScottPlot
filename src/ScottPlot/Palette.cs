using System.Drawing;
using System.Linq;

/* 
 * Palettes are collections of colors that control the default colors for new plottables added to the plot.
 * This file contains code that bridges the gap between ScottPlot 4 Colorsets and ScottPlot 5 Palettes.
 */

namespace ScottPlot
{
    /* This module will be expanded in ScottPlot 5 */
    public static class Palette
    {
        public static ScottPlot.Drawing.Palette Aurora => new(new ScottPlot.Drawing.Colorsets.Aurora());
        public static ScottPlot.Drawing.Palette Category10 => new(new ScottPlot.Drawing.Colorsets.Category10());
        public static ScottPlot.Drawing.Palette Category20 => new(new ScottPlot.Drawing.Colorsets.Category20());
        public static ScottPlot.Drawing.Palette Dark => new(new ScottPlot.Drawing.Colorsets.Dark());
        public static ScottPlot.Drawing.Palette DarkPastel => new(new ScottPlot.Drawing.Colorsets.DarkPastel());
        public static ScottPlot.Drawing.Palette Frost => new(new ScottPlot.Drawing.Colorsets.Frost());
        public static ScottPlot.Drawing.Palette Microcharts => new(new ScottPlot.Drawing.Colorsets.Microcharts());
        public static ScottPlot.Drawing.Palette Nord => new(new ScottPlot.Drawing.Colorsets.Nord());
        public static ScottPlot.Drawing.Palette OneHalf => new(new ScottPlot.Drawing.Colorsets.OneHalf());
        public static ScottPlot.Drawing.Palette OneHalfDark => new(new ScottPlot.Drawing.Colorsets.OneHalfDark());
        public static ScottPlot.Drawing.Palette PolarNight => new(new ScottPlot.Drawing.Colorsets.PolarNight());
        public static ScottPlot.Drawing.Palette SnowStorm => new(new ScottPlot.Drawing.Colorsets.Snowstorm());

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
        public static ScottPlot.Drawing.Palette[] GetPalettes() => typeof(ScottPlot.Drawing.Palette)
            .GetProperties()
            .Select(x => x.GetValue(typeof(ScottPlot.Drawing.Palette)))
            .Select(x => (ScottPlot.Drawing.Palette)x)
            .ToArray();
    }
}

namespace ScottPlot.Drawing
{
    /* This module will be retired in ScottPlot 5 */
    public class Palette
    {
        public static Palette Aurora => new(new Colorsets.Aurora());
        public static Palette Category10 => new(new Colorsets.Category10());
        public static Palette Category20 => new(new Colorsets.Category20());
        public static Palette Dark => new(new Colorsets.Dark());
        public static Palette DarkPastel => new(new Colorsets.DarkPastel());
        public static Palette Frost => new(new Colorsets.Frost());
        public static Palette Microcharts => new(new Colorsets.Microcharts());
        public static Palette Nord => new(new Colorsets.Nord());
        public static Palette OneHalf => new(new Colorsets.OneHalf());
        public static Palette OneHalfDark => new(new Colorsets.OneHalfDark());
        public static Palette PolarNight => new(new Colorsets.PolarNight());
        public static Palette SnowStorm => new(new Colorsets.Snowstorm());

        private readonly IPalette cset;
        public readonly string Name;

        public Palette(IPalette colorset)
        {
            cset = colorset ?? new Colorsets.Category10();
            Name = cset.GetType().Name;
        }

        public Palette(string[] htmlColors, string name = "Custom")
        {
            cset = new Colorsets.Custom(htmlColors);
            Name = name;
        }

        public override string ToString() => Name;

        public int GetInt32(int index)
        {
            var (r, g, b) = cset.GetRGB(index);
            return 255 << 24 | r << 16 | g << 8 | b;
        }

        public Color GetColor(int index)
        {
            return Color.FromArgb(GetInt32(index));
        }

        public Color GetColor(int index, double alpha = 1)
        {
            return Color.FromArgb(alpha: (int)(alpha * 255), baseColor: GetColor(index));
        }

        public Color[] GetColors(int count, int offset = 0, double alpha = 1)
        {
            return Enumerable.Range(offset, count)
                .Select(x => GetColor(x, alpha))
                .ToArray();
        }

        // TODO: make this a property in ScottPlot 5
        public int Count()
        {
            return cset.Count();
        }
    }
}
