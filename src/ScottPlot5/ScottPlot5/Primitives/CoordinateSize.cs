namespace ScottPlot;

public struct CoordinateSize : IEquatable<CoordinateSize>
{
    public double Width;
    public double Height;
    public double Area => Width * Height;

    public CoordinateSize(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public bool Equals(CoordinateSize other)
    {
        return Equals(Width, other.Width) && Equals(Height, other.Height);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is CoordinateSize other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(CoordinateSize a, CoordinateSize b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(CoordinateSize a, CoordinateSize b)
    {
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return Width.GetHashCode() ^ Height.GetHashCode();
    }
}
