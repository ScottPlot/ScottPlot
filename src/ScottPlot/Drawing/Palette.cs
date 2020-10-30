using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Drawing
{
    public class Palette
    {
        // Matplotlib/D3/Vega/Tableau
        public static Palette Category10 => new Palette(new Colorsets.Category10());
        public static Palette Category20 => new Palette(new Colorsets.Category20());

        // Nord
        public static Palette Aurora => new Palette(new Colorsets.Aurora());
        public static Palette Frost => new Palette(new Colorsets.Frost());
        public static Palette Nord => new Palette(new Colorsets.Nord());
        public static Palette PolarNight => new Palette(new Colorsets.PolarNight());
        public static Palette SnowStorm => new Palette(new Colorsets.Snowstorm());

        // Misc
        public static Palette OneHalfDark => new Palette(new Colorsets.OneHalfDark());
        public static Palette OneHalf => new Palette(new Colorsets.OneHalf());

        private readonly IColorset cset;
        public readonly string Name;
        public Palette(IColorset colorset)
        {
            cset = colorset ?? new Colorsets.Category10();
            Name = cset.GetType().Name;
        }

        public int GetInt32(int index)
        {
            var (r, g, b) = cset.GetRGB(index);
            return 255 << 24 | r << 16 | g << 8 | b;
        }

        public Color GetColor(int index)
        {
            return Color.FromArgb(GetInt32(index));
        }

        public int Count()
        {
            return cset.Count();
        }
    }
}
