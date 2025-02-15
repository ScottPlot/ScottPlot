namespace ScottPlot;

/// <summary>
/// This object represents the rectangular visible area on a 2D coordinate system.
/// It simply stores a <see cref="CoordinateRect"/> but has axis-related methods to act upon it.
/// </summary>
public readonly struct AxisLimits : IEquatable<AxisLimits>
{
    public double Left { get; }
    public double Right { get; }
    public double Bottom { get; }
    public double Top { get; }

    public double HorizontalSpan => Right - Left;
    public double VerticalSpan => Top - Bottom;

    public double HorizontalCenter => (Right + Left) / 2;
    public double VerticalCenter => (Top + Bottom) / 2;
    public Coordinates Center => new(HorizontalCenter, VerticalCenter);

    public CoordinateRange XRange => new(Left, Right);
    public CoordinateRange YRange => new(Bottom, Top);
    public CoordinateRange HorizontalRange => XRange;
    public CoordinateRange VerticalRange => YRange;

    // TODO: make sure callers aren't using this when they dont have to
    public CoordinateRect Rect => new(Left, Right, Bottom, Top);

    public static AxisLimits Default { get; } = new(-10, 10, -10, 10);

    public bool IsRealX => NumericConversion.AreReal(Left, Right);
    public bool IsRealY => NumericConversion.AreReal(Bottom, Top);
    public bool IsReal => IsRealX && IsRealY;
    public bool HasArea => IsReal && HorizontalSpan != 0 && VerticalSpan != 0;

    public AxisLimits(Coordinates coordinates)
    {
        Left = coordinates.X;
        Right = coordinates.X;
        Bottom = coordinates.Y;
        Top = coordinates.Y;
    }

    public AxisLimits(CoordinateRect rect)
    {
        Left = rect.Left;
        Right = rect.Right;
        Bottom = rect.Bottom;
        Top = rect.Top;
    }

    public AxisLimits(double left, double right, double bottom, double top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }

    public AxisLimits(CoordinateRange xRange, CoordinateRange yRange)
    {
        Left = xRange.IsInverted ? xRange.Max : xRange.Min;
        Right = xRange.IsInverted ? xRange.Min : xRange.Max;
        Bottom = yRange.IsInverted ? yRange.Max : yRange.Min;
        Top = yRange.IsInverted ? yRange.Min : yRange.Max;
    }

    public AxisLimits(IEnumerable<IPlottable> plottables)
    {
        ExpandingAxisLimits limits = new(plottables);
        Left = limits.Left;
        Right = limits.Right;
        Bottom = limits.Bottom;
        Top = limits.Top;
    }

    public AxisLimits(IEnumerable<Coordinates> coordinates)
    {
        ExpandingAxisLimits limits = new();
        limits.Expand(coordinates);

        Left = limits.Left;
        Right = limits.Right;
        Bottom = limits.Bottom;
        Top = limits.Top;
    }

    public AxisLimits(IEnumerable<Coordinates3d> coordinates)
    {
        ExpandingAxisLimits limits = new();
        limits.Expand(coordinates);

        Left = limits.Left;
        Right = limits.Right;
        Bottom = limits.Bottom;
        Top = limits.Top;
    }

    public AxisLimits InvertedVertically() => new(Left, Right, Top, Bottom);
    public AxisLimits InvertedHorizontally() => new(Right, Left, Bottom, Top);

    public AxisLimits ExpandedToInclude(AxisLimits otherLimits)
    {
        ExpandingAxisLimits expandingLimits = new(this);
        expandingLimits.Expand(otherLimits);
        return expandingLimits.AxisLimits;
    }

    public static AxisLimits FromPoint(double x, double y)
    {
        return new AxisLimits(x, x, y, y);
    }

    public static AxisLimits FromPoint(Coordinates c)
    {
        return new AxisLimits(c.X, c.X, c.Y, c.Y);
    }

    public override string ToString()
    {
        return $"AxisLimits: X=[{Rect.Left}, {Rect.Right}], Y=[{Rect.Bottom}, {Rect.Top}]";
    }

    public string ToString(int digits)
    {
        return $"AxisLimits: " +
            $"X=[{Math.Round(Rect.Left, digits)}, {Math.Round(Rect.Right, digits)}], " +
            $"Y=[{Math.Round(Rect.Bottom, digits)}, {Math.Round(Rect.Top, digits)}]";
    }

    public static AxisLimits NoLimits => new(double.NaN, double.NaN, double.NaN, double.NaN);
    public static AxisLimits Unset => new(double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity);

    public static AxisLimits VerticalOnly(double yMin, double yMax) => new(double.NaN, double.NaN, yMin, yMax);

    public static AxisLimits HorizontalOnly(double xMin, double xMax) => new(xMin, xMax, double.NaN, double.NaN);

    // TODO: obsolete all Expanded() methods and replace functionality with ExpandingAxisLimits

    /// <summary>
    /// Return a new <see cref="AxisLimits"/> expanded to include the given <paramref name="x"/> and <paramref name="y"/>.
    /// </summary>
    public AxisLimits Expanded(double x, double y)
    {
        double xMin2 = !double.IsNaN(Left) ? Math.Min(Left, x) : x;
        double xMax2 = !double.IsNaN(Right) ? Math.Max(Right, x) : x;
        double yMin2 = !double.IsNaN(Bottom) ? Math.Min(Bottom, y) : y;
        double yMax2 = !double.IsNaN(Top) ? Math.Max(Top, y) : y;
        return new AxisLimits(xMin2, xMax2, yMin2, yMax2);
    }

    /// <summary>
    /// Return a new <see cref="AxisLimits"/> expanded to include the given <paramref name="coordinates"/>.
    /// </summary>
    public AxisLimits Expanded(Coordinates coordinates)
    {
        return Expanded(coordinates.X, coordinates.Y);
    }

    /// <summary>
    /// Return a new <see cref="AxisLimits"/> expanded to include all corners of the given <paramref name="rect"/>.
    /// </summary>
    public AxisLimits Expanded(CoordinateRect rect)
    {
        return Expanded(rect.TopLeft).Expanded(rect.BottomRight);
    }

    /// <summary>
    /// Return a new <see cref="AxisLimits"/> expanded to include the area defined by <paramref name="limits"/>.
    /// </summary>
    public AxisLimits Expanded(AxisLimits limits)
    {
        limits = limits.Expanded(Left, Bottom);
        limits = limits.Expanded(Right, Top);
        return limits;
    }

    public AxisLimits WithPan(double deltaX, double deltaY)
    {
        return new(Left + deltaX, Right + deltaX, Bottom + deltaY, Top + deltaY);
    }

    public AxisLimits WithZoom(double fracX, double fracY)
    {
        return WithZoom(fracX, fracY, Rect.HorizontalCenter, Rect.VerticalCenter);
    }

    public AxisLimits WithZoom(double fracX, double fracY, double zoomToX, double zoomToY)
    {
        double xMin = Left;
        double xMax = Right;
        double spanLeft = zoomToX - xMin;
        double spanRight = xMax - zoomToX;
        xMin = zoomToX - spanLeft / fracX;
        xMax = zoomToX + spanRight / fracX;

        double yMin = Bottom;
        double yMax = Top;
        double spanBottom = zoomToY - yMin;
        double spanTop = yMax - zoomToY;
        yMin = zoomToY - spanBottom / fracY;
        yMax = zoomToY + spanTop / fracY;

        return new(xMin, xMax, yMin, yMax);
    }

    public bool Contains(double x, double y)
    {
        return Rect.Contains(x, y);
    }

    public bool Contains(Coordinates pt)
    {
        return Rect.Contains(pt);
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

        if (obj is AxisLimits other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(AxisLimits a, AxisLimits b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(AxisLimits a, AxisLimits b)
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
