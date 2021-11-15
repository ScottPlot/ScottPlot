using System.Drawing;
using System.Linq;

namespace ScottPlot.Drawing
{
    /* This module will be retired in ScottPlot 5 in favor of ScottPlot.Palette */
    public class Palette
    {
        /* These properties have been included for backwards compatibility.
         * They are named identical to members of the old enumeration with the same name as this class.
         * This list does not have to be expanded as new palettes are added.
         */
        public static Palette Aurora => new(new Colorsets.Aurora());
        public static Palette Category10 => new(new Colorsets.Category10());
        public static Palette Category20 => new(new Colorsets.Category20());
        public static Palette ColorblindFriendly => new(new Colorsets.ColorblindFriendly());
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
