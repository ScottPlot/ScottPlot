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

    /// <summary>
    /// Returns a collection of colors in the specified range on this colormap according to the given range and quantity.
    /// </summary>
    /// <param name="count">The number of colors to get from the colormap.</param>
    /// <param name="start">The starting point in the colormap range from which to begin extracting colors (normalized to [0, 1]).</param>
    /// <param name="end">The ending point in the colormap range at which to stop extracting colors (normalized to [0, 1]).</param>
    /// <returns>An <see cref="IEnumerable{Color}"/> collection of colors within the specified range.</returns>
    /// <remarks>
    /// If the <paramref name="count"/> is greater than the number of distinct colors in the range,
    /// duplicate colors may be included in the returned collection,
    /// depending on the implementation of <see cref="IColormap.GetColor(double)"/>.
    /// </remarks>
    /// <seealso cref="IColormap"/>
    public static IEnumerable<Color> GetColors(
        this IColormap colormap, int count, double start = 0, double end = 1)
    {
        #region Assert

        if (count < 2)
        {
            throw new ArgumentOutOfRangeException(
                nameof(count), $"Argument ${nameof(count)} must be greater than or equal to 2.");
        }

        if (double.IsInfinity(start) || double.IsNaN(start))
        {
            throw new ArgumentException(
                $"{nameof(start)} must be a real number", nameof(start));
        }

        if (double.IsInfinity(end) || double.IsNaN(end))
        {
            throw new ArgumentException(
                $"{nameof(end)} must be a real number", nameof(end));
        }

        if (start < 0 || start > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(start), $"{nameof(start)} must be within the range of 0 to 1.");
        }

        if (end < 0 || end > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(start), $"{nameof(end)} must be within the range of 0 to 1.");
        }

        if (start > end)
        {
            throw new ArgumentException(
                $"Argument ${nameof(start)} must be less than or equal to ${nameof(end)}.");
        }

        #endregion // Assert

        double step = (end - start) / (count - 1);
        return Enumerable.Range(0, count)
            .Select(i => colormap.GetColor(i * step + start));
    }
}
