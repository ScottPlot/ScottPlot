namespace ScottPlot;

public static class ColorExtensions
{
    public static SkiaSharp.SKColor ToSKColor(this Color c)
    {
        return new SkiaSharp.SKColor(c.ARGB);
    }
}
