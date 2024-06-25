﻿namespace ScottPlot;

public interface IColormap
{
    /// <summary>
    /// Full name for this colormap
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns the color of a position on this colormap (from 0 to 1).
    /// Returns transparent if NaN.
    /// Positions outside the range will be clamped.
    /// </summary>
    /// <param name="position">position from 0 (first color) to 1 (last color)</param>
    Color GetColor(double position);

    /// <summary>
    /// Returns the color of a position on this colormap (according to the given range).
    /// Returns transparent if NaN.
    /// Positions outside the range will be clamped.
    /// </summary>
    /// <param name="position">position relative to the given range</param>
    /// <param name="range">range of values spanned by this colormap</param>
    Color GetColor(double position, Range range);
}

public static class IColormapExtensions
{
    /// <summary>
    /// Create a 1 by 256 bitmap displaying all values of a heatmap
    /// </summary>
    public static SKBitmap GetSKBitmap(this IColormap colormap, bool vertical)
    {
        uint[] argbs = Enumerable.Range(0, 256)
           .Select(i => colormap.GetColor((vertical ? 255 - i : i) / 255f).ARGB)
           .ToArray();

        int bmpWidth = vertical ? 1 : 256;
        int bmpHeight = !vertical ? 1 : 256;

        SKBitmap bmp = Drawing.BitmapFromArgbs(argbs, bmpWidth, bmpHeight);

        return bmp;
    }

    /// <summary>
    /// Returns an array of colors evenly spaced along the colormap
    /// </summary>
    /// <param name="count">The number of colors to get from the colormap.</param>
    /// <param name="minFraction">The starting fraction in the colormap range from which to begin extracting colors (normalized to [0, 1]).</param>
    /// <param name="maxFraction">The ending fraction in the colormap range at which to stop extracting colors (normalized to [0, 1]).</param>
    public static Color[] GetColors(
        this IColormap colormap, int count, double minFraction = 0, double maxFraction = 1)
    {
        if (double.IsInfinity(minFraction) || double.IsNaN(minFraction))
        {
            throw new ArgumentException(
                $"{nameof(minFraction)} must be a real number", nameof(minFraction));
        }

        if (double.IsInfinity(maxFraction) || double.IsNaN(maxFraction))
        {
            throw new ArgumentException(
                $"{nameof(maxFraction)} must be a real number", nameof(maxFraction));
        }

        if (count == 0)
            return [];

        if (count == 1)
            return [colormap.GetColor(0)];

        maxFraction = NumericConversion.Clamp(maxFraction, 0, 1);
        minFraction = NumericConversion.Clamp(minFraction, 0, maxFraction);
        double fractionStep = (maxFraction - minFraction) / (count - 1);
        return Enumerable.Range(0, count)
            .Select(i => colormap.GetColor(i * fractionStep + minFraction))
            .ToArray();
    }
}
