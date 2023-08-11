namespace ScottPlot;

/// <summary>
/// A stateful analog to <see cref="AxisLimits"/> deisgned to expand to include given data
/// </summary>
public class ExpandingAxisLimits : IEquatable<ExpandingAxisLimits>
{
    public double Left { get; set; } = double.NaN;
    public double Right { get; set; } = double.NaN;
    public double Bottom { get; set; } = double.NaN;
    public double Top { get; set; } = double.NaN;

    public AxisLimits AxisLimits => new(Left, Right, Bottom, Top);

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
        Left = !double.IsNaN(Left) ? Math.Min(Left, x) : x;
        Right = !double.IsNaN(Right) ? Math.Max(Right, x) : x;
    }

    /// <summary>
    /// Expanded limits to include the given <paramref name="y"/>.
    /// </summary>
    public void ExpandY(double y)
    {
        Bottom = !double.IsNaN(Bottom) ? Math.Min(Bottom, y) : y;
        Top = !double.IsNaN(Top) ? Math.Max(Top, y) : y;
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

    public bool Equals(ExpandingAxisLimits? other)
    {
        if (other is null)
            return false;

        return
            Equals(Left, other.Left) &&
            Equals(Right, other.Right) &&
            Equals(Top, other.Top) &&
            Equals(Bottom, other.Bottom);
    }

    public bool Equals(AxisLimits other)
    {
        return
            Equals(Left, other.Left) &&
            Equals(Right, other.Right) &&
            Equals(Top, other.Top) &&
            Equals(Bottom, other.Bottom);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is ExpandingAxisLimits other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(ExpandingAxisLimits a, ExpandingAxisLimits b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(ExpandingAxisLimits a, ExpandingAxisLimits b)
    {
        return !a.Equals(b);
    }

    public static bool operator ==(ExpandingAxisLimits a, AxisLimits b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(ExpandingAxisLimits a, AxisLimits b)
    {
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return
            Left.GetHashCode() ^
            Right.GetHashCode() ^
            Bottom.GetHashCode() ^
            Top.GetHashCode();
    }
}
