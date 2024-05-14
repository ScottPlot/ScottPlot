namespace ScottPlot;

/// <summary>
/// Represents an X/Y location on screen in pixel units.
/// Pixels in screen units are distinct from <see cref="Coordinates"/> with axis units.
/// Pixels use <see cref="float"/> precision, whereas <see cref="Coordinates"/> use <see cref="double"/> precision.
/// </summary>
public struct Pixel : IEquatable<Pixel>
{
    /// <summary>
    /// Horizontal position on the screen in pixel units.
    /// Larger numbers are farther right on the screen.
    /// </summary>
    public float X;

    /// <summary>
    /// Vertical position on the screen in pixel units.
    /// Larger numbers are lower on the screen.
    /// </summary>
    public float Y;

    /// <summary>
    /// Create a pixel, casting <see cref="double"/> values into ones with <see cref="float"/> precision
    /// </summary>
    public Pixel(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Create a pixel, casting <see cref="double"/> values into ones with <see cref="float"/> precision
    /// </summary>
    public Pixel(double x, double y)
    {
        X = (float)x;
        Y = (float)y;
    }

    public override string ToString()
    {
        return $"Pixel {{ X = {X}, Y = {Y} }}";
    }

    /// <summary>
    /// Represents an invalid pixel location
    /// </summary>
    public static Pixel NaN => new(float.NaN, float.NaN);

    public static Pixel Zero => new(0, 0);

    /// <summary>
    /// Convert the ScottPlot pixel to a SkiaSharp point
    /// </summary>
    public SKPoint ToSKPoint()
    {
        return new SKPoint(X, Y);
    }

    public bool Equals(Pixel other)
    {
        return Equals(X, other.X) && Equals(Y, other.Y);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is Pixel other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(Pixel a, Pixel b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Pixel a, Pixel b)
    {
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public static Pixel operator +(Pixel a, Pixel b)
    {
        return new Pixel(a.X + b.X, a.Y + b.Y);
    }

    public static Pixel operator -(Pixel a, Pixel b)
    {
        return new Pixel(a.X - b.X, a.Y - b.Y);
    }

    public static Pixel operator *(Pixel a, float b)
    {
        return new Pixel(a.X * b, a.Y * b);
    }

    public static Pixel operator /(Pixel a, float b)
    {
        return new Pixel(a.X / b, a.Y / b);
    }

    public readonly float DistanceFrom(Pixel px2)
    {
        float dx = px2.X - X;
        float dy = px2.Y - Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    public readonly Pixel WithOffset(float dX, float dY)
    {
        return new Pixel(X + dX, Y + dY);
    }

    public readonly Pixel WithOffset(PixelOffset offset)
    {
        return new Pixel(X + offset.X, Y + offset.Y);
    }
}
