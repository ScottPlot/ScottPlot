namespace ScottPlot;

public struct PixelSize : IEquatable<PixelSize>
{
    public readonly float Width;
    public readonly float Height;
    public float Area => Width * Height;
    public float Diagonal => (float)Math.Sqrt(Width * Width + Height * Height);

    public PixelSize(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public PixelSize(double width, double height)
    {
        Width = (float)width;
        Height = (float)height;
    }

    public override string ToString()
    {
        return $"PixelSize: Width={Width}, Height={Height}";
    }

    public static PixelSize Zero => new(0, 0);

    public static PixelSize NaN => new(float.NaN, float.NaN);
    public static PixelSize Infinity => new(float.PositiveInfinity, float.PositiveInfinity);

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

    public bool Contains(PixelSize size)
    {
        return Width >= size.Width && Height >= size.Height;
    }

    public PixelSize Max(PixelSize rect2)
    {
        return new PixelSize(
            width: Math.Max(Width, rect2.Width),
            height: Math.Max(Height, rect2.Height));
    }

    public bool Equals(PixelSize other)
    {
        return Equals(Width, other.Width) && Equals(Height, other.Height);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is PixelSize other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(PixelSize a, PixelSize b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(PixelSize a, PixelSize b)
    {
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return Width.GetHashCode() ^ Height.GetHashCode();
    }

    public PixelSize Expanded(PixelPadding pad)
    {
        return new PixelSize(Width + pad.Left + pad.Right, Height + pad.Top + pad.Bottom);
    }

    public PixelSize Contracted(PixelPadding pad)
    {
        return new PixelSize(Width - pad.Left - pad.Right, Height - pad.Top - pad.Bottom);
    }
}
