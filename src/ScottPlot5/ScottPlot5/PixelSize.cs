namespace ScottPlot;

public struct PixelSize
{
    public readonly float Width;
    public readonly float Height;
    public float Area => Width * Height;

    public static PixelSize Zero => new(0, 0);

    public PixelSize(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return $"PixelSize: Width={Width}, Height={Height}";
    }

    public PixelRect ToPixelRect()
    {
        return new PixelRect(0, Width, Height, 0);
    }

    public PixelRect ToPixelRect(Pixel pixel, Alignment alignment)
    {
        PixelRect rect = new(
            left: pixel.X,
            right: pixel.X + Width,
            bottom: pixel.Y + Height,
            top: pixel.Y);

        rect = rect.WithDelta(
            x: -Width * alignment.HorizontalFraction(),
            y: -Height * (1 - alignment.VerticalFraction()));

        return rect;
    }

    public PixelSize Expand(PixelPadding padding)
    {
        return new PixelSize(Width + padding.TotalHorizontal, Height + padding.TotalVertical);
    }

    public PixelSize Contract(PixelPadding padding)
    {
        return new PixelSize(Width - padding.TotalHorizontal, Height - padding.TotalVertical);
    }
}

public static class PixelSizeExtensions
{
    public static PixelSize Max(this PixelSize rect1, PixelSize rect2)
    {
        return new PixelSize(
            width: Math.Max(rect1.Width, rect2.Width),
            height: Math.Max(rect1.Height, rect2.Height));
    }
}
