namespace ScottPlot;

/// <summary>
/// Represents the location of a point relative to a rectangle.
/// UpperLeft means the point is at the top left of the rectangle.
/// </summary>
public enum Alignment
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
    public static float HorizontalFraction(this Alignment alignment)
    {
        return alignment switch
        {
            Alignment.UpperLeft => 0,
            Alignment.UpperCenter => .5f,
            Alignment.UpperRight => 1,
            Alignment.MiddleLeft => 0,
            Alignment.MiddleCenter => .5f,
            Alignment.MiddleRight => 1,
            Alignment.LowerLeft => 0,
            Alignment.LowerCenter => .5f,
            Alignment.LowerRight => 1,
            _ => throw new NotImplementedException(),
        };
    }

    public static float VerticalFraction(this Alignment alignment)
    {
        return alignment switch
        {
            Alignment.UpperLeft => 1,
            Alignment.UpperCenter => 1,
            Alignment.UpperRight => 1,
            Alignment.MiddleLeft => .5f,
            Alignment.MiddleCenter => .5f,
            Alignment.MiddleRight => .5f,
            Alignment.LowerLeft => 0,
            Alignment.LowerCenter => 0,
            Alignment.LowerRight => 0,
            _ => throw new NotImplementedException(),
        };
    }
}
