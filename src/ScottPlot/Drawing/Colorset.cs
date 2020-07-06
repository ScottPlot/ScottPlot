using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Drawing
{
    public class Colorset
    {
        // Matplotlib/D3/Vega/Tableau
        public static Colorset Category10 => new Colorset(new Colorsets.Category10());
        public static Colorset Category20 => new Colorset(new Colorsets.Category20());

        // Nord
        public static Colorset Aurora => new Colorset(new Colorsets.Aurora());
        public static Colorset Frost => new Colorset(new Colorsets.Frost());
        public static Colorset Nord => new Colorset(new Colorsets.Nord());
        public static Colorset PolarNight => new Colorset(new Colorsets.PolarNight());
        public static Colorset SnowStorm => new Colorset(new Colorsets.Snowstorm());

        // Misc
        public static Colorset OneHalfDark => new Colorset(new Colorsets.OneHalfDark());
        public static Colorset OneHalf => new Colorset(new Colorsets.OneHalf());

        private readonly IColorset cset;
        public readonly string Name;
        public Colorset(IColorset colorset)
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
