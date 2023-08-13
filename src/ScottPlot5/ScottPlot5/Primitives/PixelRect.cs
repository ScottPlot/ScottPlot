namespace ScottPlot;

public struct PixelRect : IEquatable<PixelRect>
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
    public PixelSize Size => new(Width, Height);

    public static PixelRect Zero => new(0, 0, 0, 0);
    public static PixelRect NaN => new(Pixel.NaN, PixelSize.NaN);

    /// <summary>
    /// Create a rectangle from the bounding box of a circle centered at <paramref name="center"/> with radius <paramref name="radius"/>
    /// </summary>
    public PixelRect(Pixel center, float radius)
    {
        Left = center.X - radius;
        Right = center.X + radius;
        Bottom = center.Y + radius;
        Top = center.Y - radius;
    }

    /// <summary>
    /// Create a rectangle with edges at the given pixel positions.
    /// This constructor will rectify the points so rectangles will always have positive area.
    /// </summary>
    public PixelRect(Pixel corner1, Pixel corner2)
    {
        Left = Math.Min(corner1.X, corner2.X);
        Right = Math.Max(corner1.X, corner2.X);
        Bottom = Math.Max(corner1.Y, corner2.Y);
        Top = Math.Min(corner1.Y, corner2.Y);
    }

    /// <summary>
    /// Create a rectangle representing pixels on a screen
    /// </summary>
    public PixelRect(Pixel topLeftCorner, PixelSize size)
    {
        Left = Math.Min(topLeftCorner.X, topLeftCorner.X);
        Right = Math.Max(topLeftCorner.X, topLeftCorner.X + size.Width);
        Bottom = Math.Max(topLeftCorner.Y, topLeftCorner.Y);
        Top = Math.Min(topLeftCorner.Y, topLeftCorner.Y + size.Height);
    }

    /// <summary>
    /// Create a rectangle representing pixels on a screen
    /// </summary>
    public PixelRect(Pixel topLeftCorner, float width, float height)
    {
        Left = Math.Min(topLeftCorner.X, topLeftCorner.X);
        Right = Math.Max(topLeftCorner.X, topLeftCorner.X + width);
        Bottom = Math.Max(topLeftCorner.Y, topLeftCorner.Y);
        Top = Math.Min(topLeftCorner.Y, topLeftCorner.Y + height);
    }

    /// <summary>
    /// Create a rectangle from the given edges.
    /// This constructor permits inverted rectangles with negative area.
    /// </summary>
    public PixelRect(float left, float right, float bottom, float top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }

    public PixelRect WithPan(float x, float y)
    {
        return new PixelRect(Left + x, Right + x, Bottom + y, Top + y);
    }

    public override string ToString()
    {
        return $"PixelRect: Left={Left} Right={Right} Bottom={Bottom} Top={Top}";
    }

    public SkiaSharp.SKRect ToSKRect()
    {
        return new SKRect(Left, Top, Right, Bottom);
    }

    public PixelRect Contract(float delta)
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

    public PixelRect Expand(float delta)
    {
        return Contract(-delta);
    }

    // TODO: use operator logic?
    public PixelRect WithDelta(float x, float y)
    {
        return new PixelRect(Left + x, Right + x, Bottom + y, Top + y);
    }

    public PixelRect WithDelta(float x, float y, Alignment alignment)
    {
        return new PixelRect(Left + x, Right + x, Bottom + y, Top + y);
    }

    public bool Equals(PixelRect other)
    {
        return
            Equals(Left, other.Left) &&
            Equals(Right, other.Right) &&
            Equals(Top, other.Top) &&
            Equals(Bottom, other.Bottom);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is PixelRect other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(PixelRect a, PixelRect b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(PixelRect a, PixelRect b)
    {
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return
            Left.GetHashCode() ^
            Right.GetHashCode() ^
            Bottom.GetHashCode() ^
            Top.GetHashCode();
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
