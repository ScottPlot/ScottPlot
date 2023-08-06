namespace ScottPlot;

/// <summary>
/// A stateful analog to <see cref="AxisLimits"/> deisgned to expand to include given data
/// </summary>
public class ExpandingAxisLimits
{
    public double XMin { get; set; } = double.NaN;
    public double XMax { get; set; } = double.NaN;
    public double YMin { get; set; } = double.NaN;
    public double YMax { get; set; } = double.NaN;
    public AxisLimits AxisLimits => new(XMin, XMax, YMin, YMax);

    /// <summary>
    /// Create a new set of expanding axis limits with no leimits set initially
    /// </summary>
    public ExpandingAxisLimits()
    {
    }

    /// <summary>
    /// Create a new set of expanding axis limits starting from the given axis limits
    /// </summary>
    public ExpandingAxisLimits(AxisLimits initialLimits)
    {
        Expand(initialLimits);
    }

    /// <summary>
    /// Expanded limits to include the given <paramref name="x"/> and <paramref name="y"/>.
    /// </summary>
    public void Expand(double x, double y)
    {
        ExpandX(x);
        ExpandY(y);
    }

    /// <summary>
    /// Expanded limits to include the given <paramref name="x"/>.
    /// </summary>
    public void ExpandX(double x)
    {
        XMin = !double.IsNaN(XMin) ? Math.Min(XMin, x) : x;
        XMax = !double.IsNaN(XMax) ? Math.Max(XMax, x) : x;
    }

    /// <summary>
    /// Expanded limits to include the given <paramref name="y"/>.
    /// </summary>
    public void ExpandY(double y)
    {
        YMin = !double.IsNaN(YMin) ? Math.Min(YMin, y) : y;
        YMax = !double.IsNaN(YMax) ? Math.Max(YMax, y) : y;
    }

    /// <summary>
    /// Expanded limits to include the given <paramref name="coordinates"/>.
    /// </summary>
    public void Expand(Coordinates coordinates)
    {
        Expand(coordinates.X, coordinates.Y);
    }

    /// <summary>
    /// Expanded limits to include the given <paramref name="coordinates"/>.
    /// </summary>
    public void Expand(IReadOnlyList<Coordinates> coordinates)
    {
        foreach (Coordinates coordinate in coordinates)
        {
            Expand(coordinate);
        }
    }

    /// <summary>
    /// Expanded limits to include all corners of the given <paramref name="rect"/>.
    /// </summary>
    public void Expand(CoordinateRect rect)
    {
        Expand(rect.Left, rect.Top);
        Expand(rect.Right, rect.Bottom);
    }

    /// <summary>
    /// Expanded limits to include all corners of the given <paramref name="limits"/>.
    /// </summary>
    public void Expand(AxisLimits limits)
    {
        Expand(limits.Left, limits.Top);
        Expand(limits.Right, limits.Bottom);
    }
}
