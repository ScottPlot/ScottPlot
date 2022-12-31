namespace ScottPlot;

public struct PixelRect
{
    public readonly float Left;
    public readonly float Right;
    public readonly float Bottom;
    public readonly float Top;

    public float HorizontalCenter => (Left + Right) / 2;
    public float VerticalCenter => (Top + Bottom) / 2;
    public float Width => Right - Left;
    public float Height => Bottom - Top;
    public bool HasArea => Width * Height != 0;
    public Pixel TopLeft => new(Left, Top);
    public Pixel TopRight => new(Right, Top);
    public Pixel BottomLeft => new(Left, Bottom);
    public Pixel BottomRight => new(Right, Bottom);
    public Pixel LeftCenter => new(Left, VerticalCenter);
    public Pixel RightCenter => new(Right, VerticalCenter);
    public Pixel BottomCenter => new(HorizontalCenter, Bottom);
    public Pixel TopCenter => new(HorizontalCenter, Top);

    public override string ToString()
    {
        return $"PixelRect: Left={Left} Right={Right} Bottom={Bottom} Top={Top}";
    }

    public static PixelRect Zero => new();

    public static PixelRect Centered(Pixel center, float radius)
    {
        return new PixelRect(
            left: center.X - radius,
            right: center.X + radius,
            bottom: center.Y + radius,
            top: center.Y - radius);
    }

    [Obsolete("use PixelSize")]
    public PixelRect(float width, float height)
    {
        Left = 0;
        Right = width;
        Bottom = height;
        Top = 0;
    }

    public PixelRect(Pixel px1, Pixel px2)
    {
        Left = Math.Min(px1.X, px2.X);
        Right = Math.Max(px1.X, px2.X);
        Bottom = Math.Max(px1.Y, px2.Y);
        Top = Math.Min(px1.Y, px2.Y);
    }

    public PixelRect(float left, float right, float bottom, float top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }

    public static PixelRect FromSKRect(SkiaSharp.SKRect rect)
    {
        return new PixelRect(rect.Left, rect.Right, rect.Bottom, rect.Top);
    }

    public SkiaSharp.SKRect ToSKRect()
    {
        return new SkiaSharp.SKRect(Left, Top, Right, Bottom);
    }

    public PixelRect Contract(float delta)
    {
        float left = Math.Min(Left + delta, HorizontalCenter);
        float right = Math.Max(Right - delta, HorizontalCenter);
        float bottom = Math.Max(Bottom - delta, VerticalCenter);
        float top = Math.Min(Top + delta, VerticalCenter);
        return new PixelRect(left, right, bottom, top);
    }

    public PixelRect Expand(float delta)
    {
        return Contract(-delta);
    }

    public PixelRect Contract(PixelPadding padding)
    {
        float left = Math.Min(Left + padding.Left, HorizontalCenter);
        float right = Math.Max(Right - padding.Right, HorizontalCenter);
        float bottom = Math.Max(Bottom - padding.Bottom, VerticalCenter);
        float top = Math.Min(Top + padding.Top, VerticalCenter);
        return new PixelRect(left, right, bottom, top);
    }

    public PixelRect WithDelta(PixelSize size)
    {
        return new PixelRect(Left + size.Width, Right + size.Width, Bottom + size.Height, Top + size.Height);
    }

    public PixelRect WithDelta(float x, float y)
    {
        return new PixelRect(Left + x, Right + x, Bottom + y, Top + y);
    }

    public PixelRect WithDelta(float x, float y, Alignment alignment)
    {
        return new PixelRect(Left + x, Right + x, Bottom + y, Top + y);
    }
}

public static class PixelRectExtensions
{
    /// <summary>
    /// Create a rectangle of given sized aligned inside a larger rectangle
    /// </summary>
    public static PixelRect AlignedInside(this PixelSize size, PixelRect rect, Alignment alignment, PixelPadding padding)
    {
        PixelRect inner = rect.Contract(padding);

        return alignment switch
        {
            Alignment.UpperLeft => new PixelRect(
                left: inner.Left,
                right: inner.Left + size.Width,
                bottom: inner.Top + size.Height,
                top: inner.Top),

            Alignment.UpperCenter => new PixelRect(
                left: inner.HorizontalCenter - size.Width / 2,
                right: inner.HorizontalCenter + size.Width / 2,
                bottom: inner.Top + size.Height,
                top: inner.Top),

            Alignment.UpperRight => new PixelRect(
                left: inner.Right - size.Width,
                right: inner.Right,
                bottom: inner.Top + size.Height,
                top: inner.Top),

            Alignment.MiddleLeft => new PixelRect(
                left: inner.Left,
                right: inner.Left + size.Width,
                bottom: inner.VerticalCenter + size.Height / 2,
                top: inner.VerticalCenter - size.Height / 2),

            Alignment.MiddleCenter => new PixelRect(
                left: inner.HorizontalCenter - size.Width / 2,
                right: inner.HorizontalCenter + size.Width / 2,
                bottom: inner.VerticalCenter + size.Height / 2,
                top: inner.VerticalCenter - size.Height / 2),

            Alignment.MiddleRight => new PixelRect(
                left: inner.Right - size.Width,
                right: inner.Right,
                bottom: inner.VerticalCenter + size.Height / 2,
                top: inner.VerticalCenter - size.Height / 2),

            Alignment.LowerLeft => new PixelRect(
                left: inner.Left,
                right: inner.Left + size.Width,
                bottom: inner.Bottom,
                top: inner.Bottom - size.Height),

            Alignment.LowerCenter => new PixelRect(
                left: inner.HorizontalCenter - size.Width / 2,
                right: inner.HorizontalCenter + size.Width / 2,
                bottom: inner.Bottom,
                top: inner.Bottom - size.Height),

            Alignment.LowerRight => new PixelRect(
                left: inner.Right - size.Width,
                right: inner.Right,
                bottom: inner.Bottom,
                top: inner.Bottom - size.Height),

            _ => throw new NotImplementedException(),
        };
    }
}
