using System.ComponentModel;

namespace ScottPlot;

[Obsolete("Favor Alignment2 until this class is eliminated")]
public struct Alignment
{
    public HorizontalAlignment X { get; set; } = HorizontalAlignment.Center;
    public VerticalAlignment Y { get; set; } = VerticalAlignment.Center;

    public Alignment(
        HorizontalAlignment x = HorizontalAlignment.Center,
        VerticalAlignment y = VerticalAlignment.Center)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Return the fractional offset (0-1)
    /// Bottom is 0
    /// Left is 0
    /// </summary>
    public (double x, double y) GetOffset()
    {
        double xOffset = X switch
        {
            HorizontalAlignment.Left => 0,
            HorizontalAlignment.Center => .5,
            HorizontalAlignment.Right => 1,
            _ => throw new InvalidEnumArgumentException(X.ToString()),
        };

        double yOffset = Y switch
        {
            VerticalAlignment.Bottom => 0,
            VerticalAlignment.Center => .5,
            VerticalAlignment.Top => 1,
            _ => throw new InvalidEnumArgumentException(X.ToString()),
        };

        return (xOffset, yOffset);
    }

    /// <summary>
    /// Return the offset for a rectangle of given dimensions
    /// Bottom is 0
    /// Left is 0
    /// </summary>
    public (double x, double y) GetOffset(double width, double height)
    {
        (double xOffset, double yOffset) = GetOffset();
        return (xOffset * width, yOffset * height);
    }

    public static Alignment Center => new(HorizontalAlignment.Center, VerticalAlignment.Center);
    public static Alignment UpperLeft => new(HorizontalAlignment.Left, VerticalAlignment.Top);
    public static Alignment LowerLeft => new(HorizontalAlignment.Left, VerticalAlignment.Bottom);
    public static Alignment UpperRight => new(HorizontalAlignment.Right, VerticalAlignment.Top);
    public static Alignment LowerRight => new(HorizontalAlignment.Right, VerticalAlignment.Bottom);
    public Alignment WithHorizontalAlignment(HorizontalAlignment horizontalAlignment) => new(horizontalAlignment, Y);
    public Alignment WithVerticalAlignment(VerticalAlignment verticalAlignment) => new(X, verticalAlignment);
}

public enum HorizontalAlignment
{
    Left,
    Center,
    Right,
}

public enum VerticalAlignment
{
    Bottom,
    Center,
    Top,
}
