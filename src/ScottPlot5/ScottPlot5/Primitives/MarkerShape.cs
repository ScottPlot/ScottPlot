namespace ScottPlot;

/// <summary>
/// Standard markers supported by ScottPlot.
/// See demo app for information about creating custom marker shapes.
/// </summary>
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
