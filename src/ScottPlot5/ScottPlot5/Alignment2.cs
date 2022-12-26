namespace ScottPlot;

/// <summary>
/// Represents the location of a point relative to a rectangle.
/// UpperLeft means the point is at the top left of the rectangle.
/// </summary>
public enum Alignment2
{
    UpperLeft,
    UpperCenter,
    UpperRight,
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
    LowerLeft,
    LowerCenter,
    LowerRight,
}

public static class AlignmentExtensions
{
    public static float HorizontalFraction(this Alignment2 alignment)
    {
        return alignment switch
        {
            Alignment2.UpperLeft => 0,
            Alignment2.UpperCenter => .5f,
            Alignment2.UpperRight => 1,
            Alignment2.MiddleLeft => 0,
            Alignment2.MiddleCenter => .5f,
            Alignment2.MiddleRight => 1,
            Alignment2.LowerLeft => 0,
            Alignment2.LowerCenter => .5f,
            Alignment2.LowerRight => 1,
            _ => throw new NotImplementedException(),
        };
    }

    public static float VerticalFraction(this Alignment2 alignment)
    {
        return alignment switch
        {
            Alignment2.UpperLeft => 1,
            Alignment2.UpperCenter => 1,
            Alignment2.UpperRight => 1,
            Alignment2.MiddleLeft => .5f,
            Alignment2.MiddleCenter => .5f,
            Alignment2.MiddleRight => .5f,
            Alignment2.LowerLeft => 0,
            Alignment2.LowerCenter => 0,
            Alignment2.LowerRight => 0,
            _ => throw new NotImplementedException(),
        };
    }
}
