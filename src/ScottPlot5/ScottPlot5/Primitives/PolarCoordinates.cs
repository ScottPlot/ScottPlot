namespace ScottPlot;

/// <summary>
/// Represents a point in polar coordinate space
/// </summary>
public struct PolarCoordinates(double radius, Angle angle)
{
    public double Radius { get; set; } = radius;

    public Angle Angle { get; set; } = angle;

    public override readonly string ToString()
    {
        return $"PolarCoordinates {{ Radius = {Radius}, {Angle} }}";
    }

    public Coordinates CartesianCoordinates
    {
        get
        {
            return new Coordinates(
                x: radius * Math.Cos(Angle.Radians),
                y: radius * Math.Sin(Angle.Radians));
        }
    }
}
