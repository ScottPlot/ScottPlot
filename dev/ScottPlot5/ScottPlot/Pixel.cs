using Microsoft.Maui.Graphics;

namespace ScottPlot;

/// <summary>
/// Represents the X/Y location of a point on the screen (in pixel units, not plot units)
/// </summary>
public struct Pixel
{
    public readonly float X;
    public readonly float Y;
    public bool IsNaN => float.IsNaN(X) || float.IsNaN(Y);

    public Pixel(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"[X={X}, Y={Y}]";

    public static Pixel operator -(Pixel a, Pixel b) => new(a.X - b.X, a.Y - b.Y);

    public static Pixel operator +(Pixel a, Pixel b) => new(a.X + b.X, a.Y + b.Y);

    public static Pixel Origin => new(0, 0);

    public PointF PointF => new(X, Y);
    public static Pixel FromPointf(PointF pt) => new(pt.X, pt.Y);
}