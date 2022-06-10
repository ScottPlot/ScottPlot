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

    public SkiaSharp.SKPoint ToSKPoint()
    {
        return new SkiaSharp.SKPoint(X, Y);
    }

    public override string ToString()
    {
        return $"Pixel: X={X}, Y={Y}";
    }
}
