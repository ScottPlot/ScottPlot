namespace ScottPlot;

/// <summary>
/// Represents a point in polar coordinate space (r, θ)
/// </summary>
public struct PolarCoordinates(double radius, double angle) :
    IEquatable<PolarCoordinates>
{
    /// <summary>
    /// Radial coordinate
    /// </summary>
    public double Radius { get; set; } = radius;

    /// <summary>
    /// Radial coordinate
    /// </summary>
    public double Radial
    {
        readonly get => Radius;
        set => Radius = value;
    }

    /// <summary>
    /// Angular coordinate (degrees)
    /// </summary>
    public double Angle { get; set; } = angle;

    /// <summary>
    /// Angular coordinate (degrees)
    /// </summary>
    public double Theta
    {
        readonly get => Angle;
        set => Angle = value;
    }

    public readonly bool AreReal
        => NumericConversion.IsReal(Radial) && NumericConversion.IsReal(Angle);

    public PolarCoordinates(Coordinates coordinates) :
        this(coordinates.Distance(Coordinates.Origin),
             ScottPlot.Angle.ToDegrees(Math.Atan2(coordinates.Y, coordinates.X)))
    {
    }

    public override readonly string ToString()
    {
        return $"PolarCoordinates {{ Radial = {Radial}, Angular = {Angle} }}";
    }

    public static PolarCoordinates NaN
        => new(double.NaN, double.NaN);

    public static PolarCoordinates Origin
        => new(0, 0);

    public static PolarCoordinates Infinity
        => new(double.PositiveInfinity, double.PositiveInfinity);

    public readonly bool Equals(PolarCoordinates other)
    {
        return Equals(Radial, other.Radial) && Equals(Angle, other.Angle);
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
        return Radial.GetHashCode() ^ Angle.GetHashCode();
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
