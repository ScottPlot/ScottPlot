namespace ScottPlot;

public readonly struct PixelPath(Pixel[] pixels, bool close)
{
    public readonly Pixel[] Pixels = pixels;
    public readonly bool Close = close;

    public static PixelPath Closed(Pixel[] pixels) => new(pixels, true);
    public static PixelPath Open(Pixel[] pixels) => new(pixels, false);

    public static PixelPath Closed(IEnumerable<Pixel> pixels) => new(pixels.ToArray(), true);
    public static PixelPath Open(IEnumerable<Pixel> pixels) => new(pixels.ToArray(), false);
}
