using ScottPlot.Markers;

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
    OpenCircleWithDot,
    OpenCircleWithCross,
    OpenCircleWithEks,
}

public static class MarkerShapeExtensions
{
    /// <summary>
    /// Get the marker object for a standard marker shape
    /// </summary>
    public static IMarker GetMarker(this MarkerShape shape)
    {
        return shape switch
        {
            MarkerShape.FilledCircle => new FilledCircle(),
            MarkerShape.OpenCircle => new OpenCircle(),
            MarkerShape.FilledSquare => new FilledSquare(),
            MarkerShape.OpenSquare => new OpenSquare(),
            MarkerShape.FilledTriangleUp => new FilledTriangleUp(),
            MarkerShape.OpenTriangleUp => new OpenTriangleUp(),
            MarkerShape.FilledTriangleDown => new FilledTriangleDown(),
            MarkerShape.OpenTriangleDown => new OpenTriangleDown(),
            MarkerShape.FilledDiamond => new FilledDiamond(),
            MarkerShape.OpenDiamond => new OpenDiamond(),
            MarkerShape.Eks => new Eks(),
            MarkerShape.Cross => new Cross(),
            MarkerShape.VerticalBar => new VerticalBar(),
            MarkerShape.HorizontalBar => new HorizontalBar(),
            MarkerShape.TriUp => new TriUp(),
            MarkerShape.TriDown => new TriDown(),
            MarkerShape.Asterisk => new Asterisk(),
            MarkerShape.HashTag => new HashTag(),
            MarkerShape.OpenCircleWithDot => new OpenCircleWithDot(),
            MarkerShape.OpenCircleWithCross => new OpenCircleWithCross(),
            MarkerShape.OpenCircleWithEks => new OpenCircleWithEks(),
            MarkerShape.None => new None(),
            _ => throw new NotImplementedException(shape.ToString()),
        };
    }

    /// <summary>
    /// Used to determine whether a color passed into a LineStyle constructor
    /// will be treated as line color or fill color.
    /// </summary>
    public static bool IsLineOnly(this MarkerShape shape)
    {
        // This could probably be achieved with properties on IMarker
        // or additional interfaces like IMarkerFill and IMarkerLine
        return shape switch
        {
            MarkerShape.OpenCircle or
            MarkerShape.OpenSquare or
            MarkerShape.OpenTriangleUp or
            MarkerShape.OpenTriangleDown or
            MarkerShape.OpenDiamond or
            MarkerShape.Eks or
            MarkerShape.Cross or
            MarkerShape.VerticalBar or
            MarkerShape.HorizontalBar or
            MarkerShape.TriUp or
            MarkerShape.TriDown or
            MarkerShape.Asterisk or
            MarkerShape.HashTag or
            MarkerShape.OpenCircleWithDot or
            MarkerShape.OpenCircleWithCross or
            MarkerShape.OpenCircleWithEks => true,
            _ => false,
        };
    }
}
