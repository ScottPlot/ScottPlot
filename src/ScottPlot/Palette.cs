using System.Linq;

namespace ScottPlot
{
    public static class Palette
    {
        // TODO: in ScottPlot 5 move palettes (colorsets) and colormaps out of the Drawing namespace
        // and put them in the top level ScottPlot namespace.

        // This class/API was created so the cookbook can start using this new API and the switch
        // may be less disruptive if people start referencing this file earlier than later.

        public static Drawing.Palette Aurora => new(new Drawing.Colorsets.Aurora());
        public static Drawing.Palette Category10 => new(new Drawing.Colorsets.Category10());
        public static Drawing.Palette Category20 => new(new Drawing.Colorsets.Category20());
        public static Drawing.Palette Frost => new(new Drawing.Colorsets.Frost());
        public static Drawing.Palette Nord => new(new Drawing.Colorsets.Nord());
        public static Drawing.Palette PolarNight => new(new Drawing.Colorsets.PolarNight());
        public static Drawing.Palette SnowStorm => new(new Drawing.Colorsets.Snowstorm());
        public static Drawing.Palette OneHalfDark => new(new Drawing.Colorsets.OneHalfDark());
        public static Drawing.Palette OneHalf => new(new Drawing.Colorsets.OneHalf());
        public static Drawing.Palette Microcharts => new(new Drawing.Colorsets.Microcharts());


        /// <summary>
        /// Return an array containing every available palette
        /// </summary>
        public static Drawing.Palette[] GetPalettes()
        {
            return typeof(Drawing.Palette)
                .GetProperties()
                .Select(x => x.GetValue(typeof(Drawing.Palette)))
                .Select(x => (Drawing.Palette)x)
                .ToArray();
        }

        /// <summary>
        /// Create a custom palette from an array of HTML colors
        /// </summary>
        public static Drawing.Palette FromHtmlColors(string[] htmlColors)
        {
            return new Drawing.Palette(htmlColors);
        }
    }
}
