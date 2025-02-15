namespace ScottPlot;

/// <summary>
/// Represents a 3d point in coordinate space 
/// </summary>
public struct Coordinates3d : IEquatable<Coordinates3d>
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public bool AreReal => NumericConversion.IsReal(X) && NumericConversion.IsReal(Y) && NumericConversion.IsReal(Z);

    public Coordinates3d(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double DistanceSquared(Coordinates3d pt)
    {
        double dX = Math.Abs(X - pt.X);
        double dY = Math.Abs(Y - pt.Y);
        double dZ = Math.Abs(Z - pt.Z);
        return dX * dX + dY * dY + dZ * dZ;
    }

    public double Distance(Coordinates3d pt)
    {
        return Math.Sqrt(DistanceSquared(pt));
    }

    public override string ToString()
    {
        return $"Coordinates {{ X = {X}, Y = {Y}, Z = {Z} }}";
    }

    public static Coordinates3d NaN => new(double.NaN, double.NaN, double.NaN);

    public static Coordinates3d Origin => new(0, 0, 0);

    public static Coordinates3d Infinity => new(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

    public bool Equals(Coordinates3d other)
    {
        return Equals(X, other.X) && Equals(Y, other.Y) && Equals(Z, other.Z);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is Coordinates3d other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(Coordinates3d a, Coordinates3d b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Coordinates3d a, Coordinates3d b)
    {
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
    }

    public Coordinates3d WithDelta(double dX, double dY, double dZ)
    {
        return new(X + dX, Y + dY, Z + dZ);
    }

    public Coordinates Coordinates2d()
    {
        return new(X, Y);
    }
}
