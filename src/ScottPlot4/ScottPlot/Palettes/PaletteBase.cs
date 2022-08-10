using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;

namespace ScottPlot.Palettes;

public abstract class PaletteBase : IPalette
{
    public Color[] Colors { get; internal set; }

    public virtual string Name { get; internal set; }

    public virtual string Description { get; internal set; }

    internal static Color[] FromHexColors(string[] hexColors)
    {
        return hexColors.Select(x => ParseHex(x)).ToArray();
    }

    public int Count() => Colors.Length;

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }

    public Color GetColor(int index, double alpha = 1)
    {
        return Color.FromArgb(
            alpha: (int)(alpha * 255),
            baseColor: GetColor(index));
    }

    public Color[] GetColors(int count, int offset = 0, double alpha = 1)
    {
        return Enumerable.Range(offset, count)
            .Select(x => GetColor(x, alpha))
            .ToArray();
    }

    public (byte r, byte g, byte b) GetRGB(int index)
    {
        Color c = GetColor(index);
        return (c.R, c.G, c.B);
    }

    private static Color ParseHex(string hexColor)
    {
        if (!hexColor.StartsWith("#"))
            hexColor = "#" + hexColor;

        if (hexColor.Length != 7)
            throw new InvalidOperationException($"invalid hex color: {hexColor}");

        return ColorTranslator.FromHtml(hexColor);
    }

    public int Length => Colors.Length;

    public IEnumerator<Color> GetEnumerator() => ((IEnumerable<Color>)Colors).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Colors.GetEnumerator();
}
