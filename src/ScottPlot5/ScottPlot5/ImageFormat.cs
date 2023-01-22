namespace ScottPlot;

public enum ImageFormat
{
    Bmp,
    Jpeg,
    Png,
    Webp,
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
