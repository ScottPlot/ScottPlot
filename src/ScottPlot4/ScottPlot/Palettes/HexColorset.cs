using System;
using System.Drawing;

namespace ScottPlot.Drawing.Colorsets
{
    // hex colorsets store web-formatted colors (e.g., '#FFAA66') in a string array.

    public abstract class HexColorset : IPalette
    {
        public (byte r, byte g, byte b) GetRGB(int index)
        {
            index %= hexColors.Length;

            string hexColor = hexColors[index];
            if (!hexColor.StartsWith("#"))
                hexColor = "#" + hexColor;

            if (hexColor.Length != 7)
                throw new InvalidOperationException("invalid hex color string");

            Color color = ColorTranslator.FromHtml(hexColor);

            return (color.R, color.G, color.B);
        }

        public int Count() => hexColors is null ? 0 : hexColors.Length;

        public abstract string[] hexColors { get; }
    }
}
