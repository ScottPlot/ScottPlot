namespace ScottPlot.Style;

public enum MarkerShape
{
    None,
    Circle,
    Square,
}

public static class MarkerShapeExtensions
{
    public static IMarkerRenderer GetRenderer(this MarkerShape shape)
    {
        return shape switch
        {
            MarkerShape.Circle => new MarkerRenderers.Circle(),
            MarkerShape.Square => new MarkerRenderers.Square(),
            _ => throw new NotImplementedException(shape.ToString()),
        };
    }
}
