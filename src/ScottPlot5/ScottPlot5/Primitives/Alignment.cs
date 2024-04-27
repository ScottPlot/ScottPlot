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
    public static Alignment[,] AlignmentMatrix =
    {
        {Alignment.UpperLeft, Alignment.UpperCenter, Alignment.UpperRight },
        {Alignment.MiddleLeft, Alignment.MiddleCenter, Alignment.MiddleRight },
        {Alignment.LowerLeft, Alignment.LowerCenter, Alignment.LowerRight },
    };

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

    public static bool IsUpperEdge(this Alignment a)
    {
        return a == Alignment.UpperLeft || a == Alignment.UpperCenter || a == Alignment.UpperRight;
    }

    public static bool IsLowerEdge(this Alignment a)
    {
        return a == Alignment.LowerLeft || a == Alignment.LowerCenter || a == Alignment.LowerRight;
    }

    public static bool IsLeftEdge(this Alignment a)
    {
        return a == Alignment.UpperLeft || a == Alignment.MiddleLeft || a == Alignment.LowerLeft;
    }

    public static bool IsRightEdge(this Alignment a)
    {
        return a == Alignment.UpperRight || a == Alignment.MiddleRight || a == Alignment.LowerRight;
    }

    public static SKTextAlign ToSKTextAlign(this Alignment alignment)
    {
        return alignment switch
        {
            Alignment.UpperLeft => SKTextAlign.Left,
            Alignment.UpperCenter => SKTextAlign.Center,
            Alignment.UpperRight => SKTextAlign.Right,
            Alignment.MiddleLeft => SKTextAlign.Left,
            Alignment.MiddleCenter => SKTextAlign.Center,
            Alignment.MiddleRight => SKTextAlign.Right,
            Alignment.LowerLeft => SKTextAlign.Left,
            Alignment.LowerCenter => SKTextAlign.Center,
            Alignment.LowerRight => SKTextAlign.Right,
            _ => throw new NotImplementedException(),
        };
    }
}
