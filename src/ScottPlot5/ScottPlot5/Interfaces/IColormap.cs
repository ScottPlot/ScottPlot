using ScottPlot.Colormaps;

namespace ScottPlot;

public interface IColormap
{
    /// <summary>
    /// Human readable name for this colormap
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns the color of a position on this colormap (from 0 to 1).
    /// Returns transparent if NaN.
    /// Positions outside the range will be clamped.
    /// </summary>
    /// <param name="position">Fractional distance along the colormap</param>
    Color GetColor(double position);
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

    /// <summary>
    /// Return the color for an item at index <paramref name="index"/> of a collection of size <paramref name="count"/>.
    /// The <paramref name="startFraction"/> and <paramref name="endFraction"/> may be customized to restrict sampling to a portion of the colormap.
    /// </summary>
    public static Color GetColor(this IColormap cmap, int index, int count, double startFraction = 0, double endFraction = 1)
    {
        if (count == 1)
            return cmap.GetColor(.5);

        double fraction = (double)index / (count - 1);

        double fractionRange = endFraction - startFraction;
        fraction = fraction * fractionRange + startFraction;

        return cmap.GetColor(fraction);
    }

    public static Color GetColor(this IColormap cmap, double position, Range range)
    {
        if (double.IsNaN(position))
        {
            return Colors.Transparent;
        }

        if (range.Min == range.Max)
        {
            return cmap.GetColor(0);
        }

        double normalizedPosition = range.Normalize(position, true);

        return cmap.GetColor(normalizedPosition);
    }

    public static IColormap Reversed(this IColormap cmap)
    {
        return new Reversed(cmap);
    }

    public static ScottPlot.Image GetImage(this IColormap colormap, int height = 1, int width = 256)
    {
        using SKBitmap bmp = new(width, height);
        using SKCanvas canvas = new(bmp);
        using SKPaint paint = new() { IsStroke = true, IsAntialias = false, StrokeWidth = 1 };

        for (int x = 0; x < width; x++)
        {
            double frac = (double)x / (width - 1);
            ScottPlot.Color color = colormap.GetColor(frac);
            paint.Color = color.ToSKColor();
            canvas.DrawLine(x, 0, x, height, paint);
        }

        return new ScottPlot.Image(bmp);
    }
}
