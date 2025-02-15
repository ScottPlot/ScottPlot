namespace ScottPlot;

public readonly struct CoordinatePath(Coordinates[] points, bool close)
{
    public readonly Coordinates[] Points = points;
    public readonly bool Close = close;

    public static CoordinatePath Closed(Coordinates[] points) => new(points, true);
    public static CoordinatePath Open(Coordinates[] points) => new(points, false);

    public static CoordinatePath Closed(IEnumerable<Coordinates> points) => new(points.ToArray(), true);
    public static CoordinatePath Open(IEnumerable<Coordinates> points) => new(points.ToArray(), false);
}
