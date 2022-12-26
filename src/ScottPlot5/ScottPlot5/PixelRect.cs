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

    public PixelRect ShrinkBy(float delta)
    {
        float left = Math.Min(Left + delta, HorizontalCenter);
        float right = Math.Max(Right - delta, HorizontalCenter);
        float bottom = Math.Max(Bottom - delta, VerticalCenter);
        float top = Math.Min(Top + delta, VerticalCenter);
        return new PixelRect(left, right, bottom, top);
    }

    public PixelRect Inflated(float delta)
    {
        return ShrinkBy(-delta);
    }

    public PixelRect Deflated(float delta)
    {
        return ShrinkBy(-delta);
    }

    public PixelRect Contract(PixelPadding padding)
    {
        float left = Math.Min(Left + padding.Left, HorizontalCenter);
        float right = Math.Max(Right - padding.Right, HorizontalCenter);
        float bottom = Math.Max(Bottom - padding.Bottom, VerticalCenter);
        float top = Math.Min(Top + padding.Top, VerticalCenter);
        return new PixelRect(left, right, bottom, top);
    }

    public PixelRect WithDelta(float x, float y)
    {
        return new PixelRect(Left + x, Right + x, Bottom + y, Top + y);
    }
}
