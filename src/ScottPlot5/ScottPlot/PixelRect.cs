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

    /// <summary>
    /// Because Y pixel positions ascend from top to bottom, 
    /// <paramref name="bottom"/> should almost always be be a greater than <paramref name="top"/>.
    /// </summary>
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

    /// <summary>
    /// Return a new rectangle reduced on all sides by the given number of pixels
    /// </summary>
    public PixelRect ShrinkBy(float delta)
    {
        float left = Math.Min(Left + delta, HorizontalCenter);
        float right = Math.Max(Right - delta, HorizontalCenter);
        float bottom = Math.Max(Bottom - delta, VerticalCenter);
        float top = Math.Min(Top + delta, VerticalCenter);
        return new PixelRect(left, right, bottom, top);
    }

    public PixelRect Contract(PixelPadding padding)
    {
        float left = Math.Min(Left + padding.Left, HorizontalCenter);
        float right = Math.Max(Right - padding.Right, HorizontalCenter);
        float bottom = Math.Max(Bottom - padding.Bottom, VerticalCenter);
        float top = Math.Min(Top + padding.Top, VerticalCenter);
        return new PixelRect(left, right, bottom, top);
    }
}