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
    public double HorizontalSpan => Right - Left;
    public double VerticalSpan => Top - Bottom;
    public bool IsRealX => NumericConversion.AreReal(Left, Right);
    public bool IsRealY => NumericConversion.AreReal(Bottom, Top);
    public bool IsReal => IsRealX && IsRealY;
    public bool HasArea => IsReal && HorizontalSpan != 0 && VerticalSpan != 0;

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

    public ExpandingAxisLimits(IEnumerable<IPlottable> plottables)
    {
        foreach (IPlottable plottable in plottables)
        {
            Expand(plottable.GetAxisLimits());
        }
    }

    public override string ToString()
    {
        return $"Expanding Limits: X=[{Left}, {Right}] Y=[{Bottom}, {Top}]";
    }

    public void SetX(double left, double right)
    {
        Left = left;
        Right = right;
    }

    public void SetY(double bottom, double top)
    {
        Bottom = bottom;
        Top = top;
    }

    // TODO: Methods like Expand() should be fluent, named Expanded(), and returning this object

    /// <summary>
    /// Expanded limits to include the given <paramref name="x"/> and <paramref name="y"/>.
    /// </summary>
    public void Expand(double x, double y)
    {
        ExpandX(x);
        ExpandY(y);
    }

    public void Expand(IPlottable plottable)
    {
        AxisLimits limits = plottable.GetAxisLimits();
        Expand(limits);
    }

    /// <summary>
    /// Expanded limits to include the given <paramref name="x"/>.
    /// </summary>
    public void ExpandX(double x)
    {
        // if incoming is NaN do nothing
        if (double.IsNaN(x))
            return;

        // if existing is NaN, use the new value
        if (double.IsNaN(Left))
            Left = x;
        if (double.IsNaN(Right))
            Right = x;

        // otherwise use minmax
        Left = Math.Min(Left, x);
        Right = Math.Max(Right, x);
    }

    /// <summary>
    /// Expanded limits to include the given <paramref name="y"/>.
    /// </summary>
    public void ExpandY(double y)
    {
        // if incoming is NaN do nothing
        if (double.IsNaN(y))
            return;

        // if existing is NaN, use the new value
        if (double.IsNaN(Bottom))
            Bottom = y;
        if (double.IsNaN(Top))
            Top = y;

        // otherwise use minmax
        Bottom = Math.Min(Bottom, y);
        Top = Math.Max(Top, y);
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
    public void Expand(IEnumerable<Coordinates> coordinates)
    {
        foreach (Coordinates coordinate in coordinates)
        {
            Expand(coordinate);
        }
    }

    public void Expand(CoordinateRect rect)
    {
        Expand(rect.Left, rect.Top);
        Expand(rect.Right, rect.Bottom);
    }

    public void Expand(AxisLimits limits)
    {
        ExpandX(limits);
        ExpandY(limits);
    }

    public void ExpandX(AxisLimits limits)
    {
        ExpandX(limits.Left);
        ExpandX(limits.Right);
    }

    public void ExpandY(AxisLimits limits)
    {
        ExpandY(limits.Bottom);
        ExpandY(limits.Top);
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
