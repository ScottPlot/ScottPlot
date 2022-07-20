namespace ScottPlot;

public struct Pixel
{
    public float X;
    public float Y;

    public Pixel(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Pixel NaN => new(float.NaN, float.NaN);

    public SkiaSharp.SKPoint ToSKPoint()
    {
        return new SkiaSharp.SKPoint(X, Y);
    }

    public override string ToString()
    {
        return $"Pixel: X={X}, Y={Y}";
    }

    public static Pixel operator +(Pixel a, Pixel b)
    {
        return new Pixel(a.X + b.X, a.Y + b.Y);
    }

    public static Pixel operator -(Pixel a, Pixel b)
    {
        return new Pixel(a.X - b.X, a.Y - b.Y);
    }

    // TODO: create a PixelVector type?
    public float Hypotenuse => (float)Math.Sqrt(X * X + Y * Y);
}
