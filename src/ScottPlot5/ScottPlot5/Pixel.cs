namespace ScottPlot;

/// <summary>
/// Represents an X/Y location on screen in pixel units.
/// Pixels in screen units are distinct from <see cref="Coordinates"/> with axis units.
/// Pixels use <see cref="float"/> precision, whereas <see cref="Coordinates"/> use <see cref="double"/> precision.
/// </summary>
public struct Pixel
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

    /// <summary>
    /// Convert the ScottPlot pixel to a SkiaSharp point
    /// </summary>
    public SKPoint ToSKPoint()
    {
        return new SKPoint(X, Y);
    }

    public static Pixel operator +(Pixel a, Pixel b)
    {
        return new Pixel(a.X + b.X, a.Y + b.Y);
    }

    public static Pixel operator -(Pixel a, Pixel b)
    {
        return new Pixel(a.X - b.X, a.Y - b.Y);
    }
}
