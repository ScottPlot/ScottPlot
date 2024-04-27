namespace ScottPlot;

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
}
