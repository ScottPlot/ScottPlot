namespace ScottPlot;

/// <summary>
/// Represents a point in coordinate space (X and Y axis units)
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
        return $"Coordinates: X={X}, Y={Y}";
    }

    public static Coordinates NaN => new(double.NaN, double.NaN);

    public static Coordinates Origin => new(0, 0);
}
