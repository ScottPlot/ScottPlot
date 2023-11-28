namespace ScottPlot;

public enum MarkerShape
{
    None,
    FilledCircle,
    OpenCircle,
    FilledSquare,
    OpenSquare,
    FilledTriangleUp,
    OpenTriangleUp,
    FilledTriangleDown,
    OpenTriangleDown,
}

public static class MarkerShapeExtensions
{
    public static IMarker GetRenderer(this MarkerShape shape)
    {
        return shape switch
        {
            MarkerShape.FilledCircle or MarkerShape.OpenCircle or MarkerShape.None => new Markers.Circle(),
            MarkerShape.FilledSquare or MarkerShape.OpenSquare => new Markers.Square(),
            MarkerShape.FilledTriangleUp or MarkerShape.OpenTriangleUp => new Markers.TriangleUp(),
            MarkerShape.FilledTriangleDown or MarkerShape.OpenTriangleDown => new Markers.TriangleDown(),
            _ => throw new NotImplementedException(shape.ToString()),
        };
    }

    public static bool IsOutlined(this MarkerShape shape)
    {
        return shape switch
        {
            MarkerShape.OpenCircle or MarkerShape.OpenSquare or MarkerShape.OpenTriangleUp or MarkerShape.OpenTriangleDown => true,
            _ => false,
        };
    }
}
