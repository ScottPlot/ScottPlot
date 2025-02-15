namespace ScottPlot;

/// <summary>
/// Represents a point in polar coordinate space
/// </summary>
public struct PolarCoordinates(double radius, Angle angle)
{
    public double Radius { get; set; } = radius;
    public Angle Angle { get; set; } = angle;

    public PolarCoordinates WithRadius(double radius) => new(radius, Angle);
    public PolarCoordinates WithAngle(Angle angle) => new(Radius, angle);
    public PolarCoordinates WithAngleDegrees(double degrees) => new(Radius, Angle.FromDegrees(degrees));
    public PolarCoordinates WithAngleRadians(double radians) => new(Radius, Angle.FromRadians(radians));

    public override readonly string ToString()
    {
        return $"PolarCoordinates {{ Radius = {Radius}, {Angle} }}";
    }

    [Obsolete("use ToCartesian()", true)]
    public Coordinates CartesianCoordinates => ToCartesian();

    public Coordinates ToCartesian() => ToCartesian(Coordinates.Origin);
    public Coordinates ToCartesian(Coordinates origin)
    {
        double x = Radius * Math.Cos(Angle.Radians) + origin.X;
        double y = Radius * Math.Sin(Angle.Radians) + origin.Y;
        return new(x, y);
    }

    public static PolarCoordinates FromCartesian(Coordinates pt) => FromCartesian(pt, Coordinates.Origin);
    public static PolarCoordinates FromCartesian(Coordinates pt, Coordinates origin)
    {
        double radius = pt.Distance(origin);
        double degrees = Math.Atan2(pt.Y - origin.Y, pt.X - origin.X);
        return new PolarCoordinates(radius, Angle.FromDegrees(degrees));
    }
}
