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
