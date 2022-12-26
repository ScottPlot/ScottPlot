using SkiaSharp;

namespace ScottPlot;

internal static class SkiaSharpExtensions
{
    public static PixelSize ToPixelSize(this SKRect rect)
    {
        return new PixelSize(rect.Width, rect.Height);
    }

    public static SKTextAlign ToSKTextAlign(this Alignment2 alignment)
    {
        return alignment switch
        {
            Alignment2.UpperLeft => SKTextAlign.Left,
            Alignment2.UpperCenter => SKTextAlign.Center,
            Alignment2.UpperRight => SKTextAlign.Right,
            Alignment2.MiddleLeft => SKTextAlign.Left,
            Alignment2.MiddleCenter => SKTextAlign.Center,
            Alignment2.MiddleRight => SKTextAlign.Right,
            Alignment2.LowerLeft => SKTextAlign.Left,
            Alignment2.LowerCenter => SKTextAlign.Center,
            Alignment2.LowerRight => SKTextAlign.Right,
            _ => throw new NotImplementedException(),
        };
    }

    public static SKEncodedImageFormat ToSKFormat(this ImageFormat fmt)
    {
        return fmt switch
        {
            ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            ImageFormat.Png => SKEncodedImageFormat.Png,
            ImageFormat.Bmp => SKEncodedImageFormat.Bmp,
            ImageFormat.Webp => SKEncodedImageFormat.Webp,
            _ => throw new NotImplementedException($"unsupported format: {fmt}")
        };
    }
}
