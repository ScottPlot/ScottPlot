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
    FilledDiamond,
    OpenDiamond,
    Eks,
    Cross,
    VerticalBar,
    HorizontalBar,
    TriUp,
    TriDown,
    Asterisk,
    HashTag,
}

public static class MarkerShapeExtensions
{
    public static IMarker GetRenderer(this MarkerShape shape)
    {
        return shape switch
        {
            MarkerShape.FilledCircle => new Markers.Circle(false),
            MarkerShape.OpenCircle => new Markers.Circle(true),
            MarkerShape.FilledSquare => new Markers.Square(false),
            MarkerShape.OpenSquare => new Markers.Square(true),
            MarkerShape.FilledTriangleUp => new Markers.TriangleUp(false),
            MarkerShape.OpenTriangleUp => new Markers.TriangleUp(true),
            MarkerShape.FilledTriangleDown => new Markers.TriangleDown(false),
            MarkerShape.OpenTriangleDown => new Markers.TriangleDown(true),
            MarkerShape.FilledDiamond => new Markers.Diamond(false),
            MarkerShape.OpenDiamond => new Markers.Diamond(true),
            MarkerShape.Eks => new Markers.Eks(),
            MarkerShape.Cross => new Markers.Cross(),
            MarkerShape.VerticalBar => new Markers.VerticalBar(),
            MarkerShape.HorizontalBar => new Markers.HorizontalBar(),
            MarkerShape.TriUp => new Markers.TriUp(),
            MarkerShape.TriDown => new Markers.TriDown(),
            MarkerShape.Asterisk => new Markers.Asterisk(),
            MarkerShape.HashTag => new Markers.HashTag(),
            MarkerShape.None => new Markers.None(),
            _ => throw new NotImplementedException(shape.ToString()),
        };
    }

    public static bool IsOutlined(this MarkerShape shape)
    {
        return shape switch
        {
            (MarkerShape.OpenCircle or MarkerShape.OpenSquare or MarkerShape.OpenTriangleUp or
            MarkerShape.OpenTriangleDown or MarkerShape.OpenDiamond or MarkerShape.Eks or
            MarkerShape.Cross or MarkerShape.VerticalBar or MarkerShape.HorizontalBar or
            MarkerShape.TriUp or MarkerShape.TriDown or MarkerShape.Asterisk or
            MarkerShape.HashTag) => true,
            _ => false,
        };
    }
}
