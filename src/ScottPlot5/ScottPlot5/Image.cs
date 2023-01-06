using System.Runtime.InteropServices;

namespace ScottPlot;

public class Image : IDisposable
{
    private bool disposed;

    private readonly SKImage skiaImage;
    public int Width => skiaImage.Width;
    public int Height => skiaImage.Height;

    public Image(SKImage skiaImage)
    {
        this.skiaImage = skiaImage;
    }

    private byte[] GetBitmapBytes()
    {
        using var bmp = SKBitmap.FromImage(skiaImage);

        byte[] pixelBytes = bmp.Bytes;
        BitmapHeader header = new(Width, Height, pixelBytes.Length, true);

        byte[] bmpBytes = new byte[pixelBytes.Length + BitmapHeader.FileHeaderSize];
        IntPtr ptr = IntPtr.Zero;
        try
        {
            ptr = Marshal.AllocHGlobal(BitmapHeader.FileHeaderSize);
            Marshal.StructureToPtr(header, ptr, false);
            Marshal.Copy(ptr, bmpBytes, 0, BitmapHeader.FileHeaderSize);

            Array.Copy(pixelBytes, 0, bmpBytes, BitmapHeader.FileHeaderSize, pixelBytes.Length);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return bmpBytes;
    }

    public byte[] GetImageBytes(ImageFormat format = ImageFormat.Png, int quality = 100)
    {
        SKEncodedImageFormat skFormat = format.ToSKFormat();

        if (format == ImageFormat.Bmp) // SkiaSharp cannot create BMP files, so we have to implement this ourselves https://github.com/mono/SkiaSharp/issues/320
        {
            return GetBitmapBytes();
        }

        using var skData = skiaImage.Encode(skFormat, quality);
        return skData.ToArray();
    }

    public string SaveJpeg(string path, int quality = 85)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Jpeg, quality);
        File.WriteAllBytes(path, bytes);
        return Path.GetFullPath(path);
    }

    public string SavePng(string path)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Png, 100);
        File.WriteAllBytes(path, bytes);
        return Path.GetFullPath(path);
    }

    public string SaveBmp(string path)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Bmp, 100);
        File.WriteAllBytes(path, bytes);
        return Path.GetFullPath(path);
    }

    public string SaveWebp(string path, int quality = 85)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Webp, quality);
        File.WriteAllBytes(path, bytes);
        return Path.GetFullPath(path);
    }

    public string Save(string path, ImageFormat format = ImageFormat.Png, int quality = 85)
    {
        return format switch
        {
            ImageFormat.Png => SavePng(path),
            ImageFormat.Jpeg => SaveJpeg(path, quality),
            ImageFormat.Webp => SaveWebp(path, quality),
            ImageFormat.Bmp => SaveBmp(path),
            _ => throw new ArgumentException($"Unsupported image format: {format}"),
        };
    }

    public void Dispose()
    {
        if (disposed)
            return;

        skiaImage.Dispose();
        disposed = true;

        GC.SuppressFinalize(this);
    }
}
