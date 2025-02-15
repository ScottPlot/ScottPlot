namespace ScottPlot;

/// <summary>
/// Represents a point in coordinate space (X and Y axis units)
/// </summary>
public struct Coordinates : IEquatable<Coordinates>
{
    public double X { get; set; }
    public double Y { get; set; }

    public bool AreReal => NumericConversion.IsReal(X) && NumericConversion.IsReal(Y);

    /// <summary>
    /// Define a new coordinate at the given X and Y location (in axis units)
    /// </summary>
    public Coordinates(double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Create an array of coordinates from individual arrays of X and Y positions
    /// </summary>
    public static Coordinates[] Zip(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have equal length");

        Coordinates[] cs = new Coordinates[xs.Length];

        for (int i = 0; i < cs.Length; i++)
        {
            cs[i] = new Coordinates(xs[i], ys[i]);
        }

        return cs;
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

    public static Coordinates Zero => new(0, 0);

    public static Coordinates Origin => new(0, 0);

    public static Coordinates Infinity => new(double.PositiveInfinity, double.PositiveInfinity);

    /// <summary>
    /// The inverse of the present coordinate. E.g., the point (X, Y) becomes (Y, X).
    /// </summary>
    public readonly Coordinates Rotated => new(Y, X);

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

    public readonly (double X, double Y) Deconstruct() => (X, Y);

    public static Coordinates operator +(Coordinates a, Coordinates b)
    {
        return new(a.X + b.X, a.Y + b.Y);
    }
}
