namespace ScottPlot;

/// <summary>
/// Represents a point in polar coordinate space (Radial and Angular axis units)
/// </summary>
public struct PolarCoordinates(double radial, double angular) :
    IEquatable<PolarCoordinates>
{
    /// <summary>
    /// Radial coordinate
    /// </summary>
    public double Radial { get; set; } = radial;

    /// <summary>
    /// Angular coordinate (degrees)
    /// </summary>
    public double Angular { get; set; } = angular;

    public readonly bool AreReal
        => NumericConversion.IsReal(Radial) && NumericConversion.IsReal(Angular);

    public PolarCoordinates(Coordinates coordinates) :
        this(coordinates.Distance(Coordinates.Origin),
             Angle.ToDegrees(Math.Atan2(coordinates.Y, coordinates.X)))
    {
    }

    public override readonly string ToString()
    {
        return $"PolarCoordinates {{ Radial = {Radial}, Angular = {Angular} }}";
    }

    public static PolarCoordinates NaN
        => new(double.NaN, double.NaN);

    public static PolarCoordinates Origin
        => new(0, 0);

    public static PolarCoordinates Infinity
        => new(double.PositiveInfinity, double.PositiveInfinity);

    public readonly bool Equals(PolarCoordinates other)
    {
        return Equals(Radial, other.Radial) && Equals(Angular, other.Angular);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is PolarCoordinates other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(PolarCoordinates a, PolarCoordinates b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(PolarCoordinates a, PolarCoordinates b)
    {
        return !a.Equals(b);
    }

    public override readonly int GetHashCode()
    {
        return Radial.GetHashCode() ^ Angular.GetHashCode();
    }

    public static implicit operator PolarCoordinates(Coordinates coordinates)
    {
        return new PolarCoordinates(coordinates);
    }

    public static PolarCoordinates FromCartesianCoordinates(double x, double y)
    {
        return new Coordinates(x, y);
    }
}
