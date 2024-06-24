namespace ScottPlot;

public readonly struct PixelRect : IEquatable<PixelRect>
{
    public readonly float Left;
    public readonly float Right;
    public readonly float Bottom; // this value should be larger than Top
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
    public Pixel Center => new(HorizontalCenter, VerticalCenter);
    public PixelSize Size => new(Width, Height);

    public PixelLine BottomLine => new(BottomLeft, BottomRight);
    public PixelLine TopLine => new(TopLeft, TopRight);
    public PixelLine LeftLine => new(TopLeft, BottomLeft);
    public PixelLine RightLine => new(TopRight, BottomRight);

    public static PixelRect Zero => new(0, 0, 0, 0);
    public static PixelRect NaN => new(Pixel.NaN, PixelSize.NaN);

    /// <summary>
    /// Create a rectangle from the bounding box of a circle centered at <paramref name="center"/> with radius <paramref name="radius"/>
    /// </summary>
    public PixelRect(Pixel center, float radius) : this(center.X - radius, center.X + radius, center.Y + radius, center.Y - radius)
    {
    }

    /// <summary>
    /// Create a rectangle with edges at the given pixel positions.
    /// This constructor will rectify the points so rectangles will always have positive area.
    /// </summary>
    public PixelRect(Pixel corner1, Pixel corner2) : this(corner1.X, corner2.X, corner1.Y, corner2.Y)
    {
    }

    /// <summary>
    /// Create a rectangle representing pixels on a screen
    /// </summary>
    public PixelRect(Pixel topLeftCorner, PixelSize size) : this(topLeftCorner.X, topLeftCorner.X + size.Width, topLeftCorner.Y, topLeftCorner.Y + size.Height)
    {
    }

    /// <summary>
    /// Create a rectangle representing pixels on a screen
    /// </summary>
    public PixelRect(PixelOffset offset, PixelSize size) : this(offset.X, offset.X + size.Width, offset.Y, offset.Y + size.Height)
    {
    }

    /// <summary>
    /// Create a rectangle representing pixels on a screen
    /// </summary>
    public PixelRect(Pixel topLeftCorner, float width, float height) : this(topLeftCorner.X, topLeftCorner.X + width, topLeftCorner.Y, topLeftCorner.Y + height)
    {
    }

    /// <summary>
    /// Create a rectangle representing pixels on a screen
    /// </summary>
    public PixelRect(PixelSize size) : this(0, size.Width, size.Height, 0)
    {
    }

    /// <summary>
    /// Create a rectangle representing pixels on a screen
    /// </summary>
    public PixelRect(PixelSize size, Pixel offset) : this(offset.X, size.Width, size.Height, offset.Y)
    {
    }

    /// <summary>
    /// Create a rectangle representing pixels on a screen
    /// </summary>
    public PixelRect(PixelSize size, PixelOffset offset) : this(offset.X, size.Width, size.Height, offset.Y)
    {
    }

    /// <summary>
    /// Create a rectangle from the given edges.
    /// This constructor permits inverted rectangles with negative area.
    /// </summary>
    public PixelRect(float left, float right, float bottom, float top)
    {
        Left = Math.Min(left, right);
        Right = Math.Max(left, right);
        Bottom = Math.Max(top, bottom);
        Top = Math.Min(top, bottom);
    }

    /// <summary>
    /// Create a pixel rectangle from two pixel ranges
    /// </summary>
    public PixelRect(PixelRangeX xRange, PixelRangeY yRange)
    {
        Left = xRange.Left;
        Right = xRange.Right;
        Bottom = yRange.Bottom;
        Top = yRange.Top;
    }

    public PixelRect(IEnumerable<Pixel> pixels)
    {
        if (!pixels.Any())
            return;

        Left = pixels.First().X;
        Right = pixels.First().X;
        Bottom = pixels.First().Y;
        Top = pixels.First().Y;

        foreach (var pixel in pixels)
        {
            Left = Math.Min(pixel.X, Left);
            Right = Math.Max(pixel.X, Right);
            Bottom = Math.Max(pixel.Y, Bottom);
            Top = Math.Min(pixel.Y, Top);
        }
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
        return Contract(delta, delta);
    }

    public PixelRect Contract(float x, float y)
    {
        float left = Math.Min(Left + x, HorizontalCenter);
        float right = Math.Max(Right - x, HorizontalCenter);
        float bottom = Math.Max(Bottom - y, VerticalCenter);
        float top = Math.Min(Top + y, VerticalCenter);
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

    public PixelRect Expand(PixelRect other)
    {
        float left = Math.Min(Left, other.Left);
        float right = Math.Max(Right, other.Right);
        float bottom = Math.Max(Bottom, other.Bottom);
        float top = Math.Min(Top, other.Top);
        return new PixelRect(left, right, bottom, top);
    }

    /// <summary>
    /// Returns the intersection with another rectangle
    /// </summary>
    /// <param name="other">Other rectangle</param>
    /// <returns>Intersection rectangle</returns>
    public PixelRect Intersect(PixelRect other)
    {
        float left = Math.Max(Left, other.Left);
        float right = Math.Min(Right, other.Right);

        if (left > right)
            return NaN;

        float bottom = Math.Min(Bottom, other.Bottom);
        float top = Math.Max(Top, other.Top);

        if (top > bottom)
            return NaN;

        return new PixelRect(left, right, bottom, top);
    }

    public PixelRect Expand(PixelPadding pad)
    {
        float left = Left - pad.Left;
        float right = Right + pad.Right;
        float bottom = Bottom + pad.Bottom;
        float top = Top - pad.Top;
        return new PixelRect(left, right, bottom, top);
    }

    public PixelRect Expand(float delta)
    {
        return Contract(-delta);
    }

    public PixelRect ExpandX(float x)
    {
        float left = Math.Min(Left, x);
        float right = Math.Max(Right, x);
        return new PixelRect(left, right, Bottom, Top);
    }

    public PixelRect ExpandY(float y)
    {
        float top = Math.Min(Top, y);
        float bottom = Math.Max(Bottom, y);
        return new PixelRect(Left, Right, bottom, top);
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

    public bool Contains(Pixel px)
    {
        return Contains(px.X, px.Y);
    }

    public bool Contains(float x, float y)
    {
        return Left <= x && x <= Right && Top <= y && y <= Bottom;
    }

    public bool ContainsX(float x)
    {
        return Left <= x && x <= Right;
    }

    public bool ContainsY(float y)
    {
        return Top <= y && y <= Bottom;
    }

    public Pixel GetAlignedPixel(Alignment alignment)
    {
        return alignment switch
        {
            Alignment.UpperLeft => TopLeft,
            Alignment.UpperCenter => TopCenter,
            Alignment.UpperRight => TopRight,
            Alignment.MiddleLeft => LeftCenter,
            Alignment.MiddleCenter => new(HorizontalCenter, VerticalCenter),
            Alignment.MiddleRight => RightCenter,
            Alignment.LowerLeft => BottomLeft,
            Alignment.LowerCenter => BottomCenter,
            Alignment.LowerRight => BottomRight,
            _ => Pixel.NaN,
        };
    }

    public PixelRect WithOffset(PixelOffset offset)
    {
        return new PixelRect(
            left: Left + offset.X,
            right: Right + offset.X,
            bottom: Bottom + offset.Y,
            top: Top + offset.Y
        );
    }

    /// <summary>
    /// Return the position of this rectangle aligned inside a larger one
    /// </summary>
    public PixelRect AlignedInside(PixelRect largerRect, Alignment alignment)
    {
        return AlignedInside(largerRect, alignment, PixelPadding.Zero);
    }

    /// <summary>
    /// Return the position of this rectangle aligned inside a larger one
    /// </summary>
    public PixelRect AlignedInside(PixelRect largerRect, Alignment alignment, PixelPadding padding)
    {
        PixelRect inner = largerRect.Contract(padding);

        return alignment switch
        {
            Alignment.UpperLeft => new PixelRect(
                left: inner.Left,
                right: inner.Left + Width,
                bottom: inner.Top + Height,
                top: inner.Top),

            Alignment.UpperCenter => new PixelRect(
                left: inner.HorizontalCenter - Width / 2,
                right: inner.HorizontalCenter + Width / 2,
                bottom: inner.Top + Height,
                top: inner.Top),

            Alignment.UpperRight => new PixelRect(
                left: inner.Right - Width,
                right: inner.Right,
                bottom: inner.Top + Height,
                top: inner.Top),

            Alignment.MiddleLeft => new PixelRect(
                left: inner.Left,
                right: inner.Left + Width,
                bottom: inner.VerticalCenter + Height / 2,
                top: inner.VerticalCenter - Height / 2),

            Alignment.MiddleCenter => new PixelRect(
                left: inner.HorizontalCenter - Width / 2,
                right: inner.HorizontalCenter + Width / 2,
                bottom: inner.VerticalCenter + Height / 2,
                top: inner.VerticalCenter - Height / 2),

            Alignment.MiddleRight => new PixelRect(
                left: inner.Right - Width,
                right: inner.Right,
                bottom: inner.VerticalCenter + Height / 2,
                top: inner.VerticalCenter - Height / 2),

            Alignment.LowerLeft => new PixelRect(
                left: inner.Left,
                right: inner.Left + Width,
                bottom: inner.Bottom,
                top: inner.Bottom - Height),

            Alignment.LowerCenter => new PixelRect(
                left: inner.HorizontalCenter - Width / 2,
                right: inner.HorizontalCenter + Width / 2,
                bottom: inner.Bottom,
                top: inner.Bottom - Height),

            Alignment.LowerRight => new PixelRect(
                left: inner.Right - Width,
                right: inner.Right,
                bottom: inner.Bottom,
                top: inner.Bottom - Height),

            _ => throw new NotImplementedException(),
        };
    }
}

public static class PixelRectExtensions
{
    public static PixelRect ToPixelRect(this SKRect rect)
    {
        return new PixelRect(rect.Left, rect.Right, rect.Bottom, rect.Top);
    }
}
