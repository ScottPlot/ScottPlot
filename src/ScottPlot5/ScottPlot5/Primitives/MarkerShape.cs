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
            MarkerShape.FilledCircle => new Markers.Circle() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenCircle => new Markers.Circle() { Fill = false, LineWidth = 1 },
            MarkerShape.FilledSquare => new Markers.Square() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenSquare => new Markers.Square() { Fill = false, LineWidth = 1 },
            MarkerShape.FilledTriangleUp => new Markers.TriangleUp() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenTriangleUp => new Markers.TriangleUp() { Fill = false, LineWidth = 1 },
            MarkerShape.FilledTriangleDown => new Markers.TriangleDown() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenTriangleDown => new Markers.TriangleDown() { Fill = false, LineWidth = 1 },
            MarkerShape.FilledDiamond => new Markers.Diamond() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenDiamond => new Markers.Diamond() { Fill = false, LineWidth = 1 },
            MarkerShape.Eks => new Markers.Eks() { Fill = false, LineWidth = 1 },
            MarkerShape.Cross => new Markers.Cross() { Fill = false, LineWidth = 1 },
            MarkerShape.VerticalBar => new Markers.VerticalBar() { Fill = false, LineWidth = 1 },
            MarkerShape.HorizontalBar => new Markers.HorizontalBar() { Fill = false, LineWidth = 1 },
            MarkerShape.TriUp => new Markers.TriUp() { Fill = false, LineWidth = 1 },
            MarkerShape.TriDown => new Markers.TriDown() { Fill = false, LineWidth = 1 },
            MarkerShape.Asterisk => new Markers.Asterisk() { Fill = false, LineWidth = 1 },
            MarkerShape.HashTag => new Markers.HashTag() { Fill = false, LineWidth = 1 },
            MarkerShape.None => new Markers.None() { Fill = false, LineWidth = 0 },
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
