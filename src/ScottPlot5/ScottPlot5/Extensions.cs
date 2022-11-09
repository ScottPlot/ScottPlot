using SkiaSharp;

namespace ScottPlot;

internal static class Extensions
{
    internal static bool IsInfiniteOrNaN(this double x)
    {
        return !IsFinite(x);
    }

    internal static bool IsFinite(this double x)
    {
        if (double.IsInfinity(x))
            return false;

        if (double.IsNaN(x))
            return false;

        return true;
    }

    #region SkiaSharp

    internal static SKEncodedImageFormat ToSKFormat(this ImageFormat fmt)
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

    #endregion
}
