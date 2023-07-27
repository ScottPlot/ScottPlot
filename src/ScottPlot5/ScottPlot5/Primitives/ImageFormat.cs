using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace ScottPlot;

public enum ImageFormat
{
    Bmp,
    Jpeg,
    Png,
    Webp,
    Svg,
}

public static class ImageFormatExtensions
{
    public static bool IsRasterFormat(this ImageFormat format)
    {
        return format switch
        {
            ImageFormat.Bmp => true,
            ImageFormat.Jpeg => true,
            ImageFormat.Png => true,
            ImageFormat.Webp => true,
            ImageFormat.Svg => false,
            _ => throw new ArgumentException($"unknown image format: '{format}'")
        };
    }
}

public static class ImageFormatLookup
{
    public static ImageFormat FromFileExtension(string ext)
    {
        if (!ext.StartsWith("."))
            throw new ArgumentException("extension must start with '.'");

        return ext.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => ImageFormat.Jpeg,
            ".png" => ImageFormat.Png,
            ".bmp" => ImageFormat.Bmp,
            ".webp" => ImageFormat.Webp,
            _ => throw new ArgumentException($"unknown image format: '{ext}'")
        };
    }

    public static ImageFormat FromFilePath(string path)
    {
        return FromFileExtension(Path.GetExtension(path));
    }
}
