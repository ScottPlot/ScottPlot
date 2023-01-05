using System.IO;

namespace ScottPlot;

public enum ImageFormat
{
    Bmp,
    Jpeg,
    Png,
    Webp,
}

public static class ImageFormatHelpers
{
    public static ImageFormat? FromFileExtension(string ext)
    {
        return ext.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => ImageFormat.Jpeg,
            ".png" => ImageFormat.Png,
            ".bmp" => ImageFormat.Bmp,
            ".webp" => ImageFormat.Webp,
            _ => null
        };
    }

    public static ImageFormat? FromFilePath(string path)
    {
        return FromFileExtension(Path.GetExtension(path));
    }
}
