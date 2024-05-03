namespace ScottPlot;

/// <summary>
/// Represents a point in coordinate space (X and Y axis units)
/// </summary>
public struct Coordinates : IEquatable<Coordinates>
{
    public double X { get; set; }
    public double Y { get; set; }

    public bool AreReal => NumericConversion.IsReal(X) && NumericConversion.IsReal(Y);

    public Coordinates(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double DistanceSquared(Coordinates pt)
    {
        double dX = Math.Abs(X - pt.X);
        double dY = Math.Abs(Y - pt.Y);
        return dX * dX + dY * dY;
    }

    public double Distance(Coordinates pt)
    {
        return Math.Sqrt(DistanceSquared(pt));
    }

    public override string ToString()
    {
        return $"Coordinates {{ X = {X}, Y = {Y} }}";
    }

    public static Coordinates NaN => new(double.NaN, double.NaN);

    public static Coordinates Origin => new(0, 0);

    public static Coordinates Infinity => new(double.PositiveInfinity, double.PositiveInfinity);

    public bool Equals(Coordinates other)
    {
        return Equals(X, other.X) && Equals(Y, other.Y);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is Coordinates other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(Coordinates a, Coordinates b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Coordinates a, Coordinates b)
    {
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public CoordinateRect ToRect(double radiusX, double radiusY)
    {
        return new CoordinateRect(X - radiusX, X + radiusX, Y - radiusY, Y + radiusY);
    }

    public CoordinateRect ToRect(double radius)
    {
        return ToRect(radius, radius);
    }

    public Coordinates WithDelta(double dX, double dY)
    {
        return new(X + dX, Y + dY);
    }
}
