namespace ScottPlot.Style;

public enum MarkerShape
{
    None,
    FilledCircle,
    OpenCircle,
    FilledSquare,
    OpenSquare,
}

public static class MarkerShapeExtensions
{
    public static IMarkerRenderer GetRenderer(this MarkerShape shape)
    {
        return shape switch
        {
            MarkerShape.FilledCircle or MarkerShape.OpenCircle or MarkerShape.None => new MarkerRenderers.Circle(),
            MarkerShape.FilledSquare or MarkerShape.OpenSquare => new MarkerRenderers.Square(),
            _ => throw new NotImplementedException(shape.ToString()),
        };
    }

    public static bool IsOutlined(this MarkerShape shape)
    {
        return shape switch
        {
            MarkerShape.OpenCircle or MarkerShape.OpenSquare => true,
            _ => false,
        };
    }
}
