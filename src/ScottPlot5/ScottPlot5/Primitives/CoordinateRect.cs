namespace ScottPlot;

/// <summary>
/// Describes a rectangle in 2D coordinate space.
/// </summary>
public struct CoordinateRect : IEquatable<CoordinateRect>
{
    public double Left { get; set; }
    public double Right { get; set; }
    public double Bottom { get; set; }
    public double Top { get; set; }

    public double HorizontalCenter => (Right + Left) / 2;
    public double VerticalCenter => (Top + Bottom) / 2;

    public Coordinates Center => new(HorizontalCenter, VerticalCenter);
    public Coordinates TopLeft => new(Left, Top);
    public Coordinates TopRight => new(Right, Top);
    public Coordinates BottomLeft => new(Left, Bottom);
    public Coordinates BottomRight => new(Bottom, Right);

    public CoordinateRange XRange => new(Left, Right);
    public CoordinateRange YRange => new(Bottom, Top);

    public double Width => Right - Left;
    public double Height => Top - Bottom;
    public double Area => Width * Height;
    public bool HasArea => (Area != 0 && !double.IsNaN(Area) && !double.IsInfinity(Area));
    public bool IsInvertedX => Left > Right;
    public bool IsInvertedY => Top < Bottom;

    public CoordinateRect(CoordinateRange xRange, CoordinateRange yRange)
    {
        Left = xRange.Min;
        Right = xRange.Max;
        Bottom = yRange.Min;
        Top = yRange.Max;
    }

    public CoordinateRect(CoordinateRangeMutable xRange, CoordinateRangeMutable yRange)
    {
        Left = xRange.Min;
        Right = xRange.Max;
        Bottom = yRange.Min;
        Top = yRange.Max;
    }

    public CoordinateRect(Coordinates pt1, Coordinates pt2)
    {
        Left = Math.Min(pt1.X, pt2.X);
        Right = Math.Max(pt1.X, pt2.X);
        Bottom = Math.Min(pt1.Y, pt2.Y);
        Top = Math.Max(pt1.Y, pt2.Y);
    }

    public CoordinateRect(double left, double right, double bottom, double top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }

    public CoordinateRect(Coordinates point, CoordinateSize size)
    {
        Coordinates pt2 = new(point.X + size.Width, point.Y + size.Height);
        Left = Math.Min(point.X, pt2.X);
        Right = Math.Max(point.X, pt2.X);
        Bottom = Math.Min(point.Y, pt2.Y);
        Top = Math.Max(point.Y, pt2.Y);
    }

    public bool Contains(double x, double y)
    {
        return x >= Left && x <= Right && y >= Bottom && y <= Top;
    }

    public bool ContainsX(double x)
    {
        return x >= Left && x <= Right;
    }

    public bool ContainsY(double y)
    {
        return y >= Bottom && y <= Top;
    }

    public CoordinateRect Expanded(Coordinates point)
    {
        double exLeft = Left;
        double exRight = Right;
        double exBottom = Bottom;
        double exTop = Top;

        if (!Contains(point))
        {
            exLeft = Math.Min(exLeft, point.X);
            exRight = Math.Max(exRight, point.X);
            exBottom = Math.Min(exBottom, point.Y);
            exTop = Math.Max(exTop, point.Y);
        }

        return new CoordinateRect(exLeft, exRight, exBottom, exTop);
    }

    public bool Contains(Coordinates point) => Contains(point.X, point.Y);

    public static CoordinateRect Empty => new(double.NaN, double.NaN, double.NaN, double.NaN);

    public CoordinateRect WithTranslation(Coordinates p) => new(Left + p.X, Right + p.X, Bottom + p.Y, Top + p.Y);

    public override string ToString()
    {
        return $"PixelRect: Left={Left} Right={Right} Bottom={Bottom} Top={Top}";
    }

    public bool Equals(CoordinateRect other)
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

        if (obj is CoordinateRect other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(CoordinateRect a, CoordinateRect b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(CoordinateRect a, CoordinateRect b)
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
