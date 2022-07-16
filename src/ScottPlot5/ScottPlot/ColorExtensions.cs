namespace ScottPlot;

public static class ColorExtensions
{
    public static SkiaSharp.SKColor ToSKColor(this SPColor c)
    {
        return new SkiaSharp.SKColor(c.ARGB);
    }
}
