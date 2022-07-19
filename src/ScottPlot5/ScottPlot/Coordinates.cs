namespace ScottPlot;

// TODO: Coordinate should be Coordinates (the object describes a pair of coordinates: X and Y)

/// <summary>
/// Represents a point in coordinate space using axis units
/// </summary>
public struct Coordinates
{
    public double X { get; set; }
    public double Y { get; set; }

    public Coordinates(double x, double y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"Coordinate: X={X}, Y={Y}";
    }

    public static Coordinates NaN()
    {
        return new(double.NaN, double.NaN);
    }
}
