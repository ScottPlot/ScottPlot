/* SkiaSharp cannot create BMP files, so bitmap IO is implemented manually
 * https://github.com/mono/SkiaSharp/issues/320
 */

using System.Drawing;
using System.Runtime.InteropServices;
using ScottPlot.IO;

namespace ScottPlot;

/// <summary>
/// Bitmap representation of a <seealso cref="SkiaSharp.SKImage"/>
/// </summary>
public class Image : IDisposable
{
    private bool IsDisposed = false;

    protected readonly SKImage SKImage;
    public int Width => SKImage.Width;
    public int Height => SKImage.Height;
    public PixelSize Size => new(Width, Height);
    public byte Alpha => 255;

    [Obsolete("Use initializer that accepts a SKSurface", true)]
    public Image(SKImage image)
    {
        SKImage = image;
    }

    public Image(SKSurface surface)
    {
        SKImage = surface.Snapshot();
    }

    public Image(string filename)
    {
        using SKBitmap bmp = SKBitmap.Decode(filename);
        SKImage = SKImage.FromBitmap(bmp);
    }

    public Image(byte[] bytes)
    {
        SKImage = SKImage.FromEncodedData(bytes);
    }

    public Image(SKBitmap bmp)
    {
        SKImage = SKImage.FromBitmap(bmp);
    }

    /// <summary>
    /// SkiaSharp cannot natively create BMP files. 
    /// This function creates bitmaps in memory manually.
    /// https://github.com/mono/SkiaSharp/issues/320
    /// </summary>
    private byte[] GetBitmapBytes()
    {
        using SKBitmap skBitmap = SKBitmap.FromImage(SKImage);

        BitmapHeader header = new(Width, Height);

        byte[] bitmapBytes = new byte[skBitmap.Bytes.Length + BitmapHeader.FileHeaderSize];

        IntPtr pHeader = IntPtr.Zero;

        try
        {
            pHeader = Marshal.AllocHGlobal(BitmapHeader.FileHeaderSize);
            Marshal.StructureToPtr(header, pHeader, false);

            // copy the header from the bytes of our custom bitmap header struct
            Marshal.Copy(pHeader, bitmapBytes, 0, BitmapHeader.FileHeaderSize);

            // copy the content of the bitmap from the SkiaSharp image
            Array.Copy(skBitmap.Bytes, 0, bitmapBytes, BitmapHeader.FileHeaderSize, skBitmap.Bytes.Length);
        }
        finally
        {
            Marshal.FreeHGlobal(pHeader);
        }

        return bitmapBytes;
    }

    public byte[] GetImageBytes(ImageFormat format = ImageFormat.Png, int quality = 100)
    {
        SKEncodedImageFormat skFormat = format.ToSKFormat();

        if (format == ImageFormat.Bmp)
        {
            return GetBitmapBytes();
        }

        using var skData = SKImage.Encode(skFormat, quality);
        return skData.ToArray();
    }

    public SavedImageInfo SaveJpeg(string path, int quality = 85)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Jpeg, quality);
        File.WriteAllBytes(path, bytes);
        return new SavedImageInfo(path, bytes.Length);
    }

    public SavedImageInfo SavePng(string path)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Png, 100);
        File.WriteAllBytes(path, bytes);
        return new SavedImageInfo(path, bytes.Length);
    }

    public SavedImageInfo SaveBmp(string path)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Bmp, 100);
        File.WriteAllBytes(path, bytes);
        return new SavedImageInfo(path, bytes.Length);
    }

    public SavedImageInfo SaveWebp(string path, int quality = 85)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Webp, quality);
        File.WriteAllBytes(path, bytes);
        return new SavedImageInfo(path, bytes.Length);
    }

    public SavedImageInfo Save(string path, ImageFormat format = ImageFormat.Png, int quality = 85)
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
        if (IsDisposed)
            return;

        SKImage.Dispose();
        IsDisposed = true;

        GC.SuppressFinalize(this);
    }

    public void Render(SKCanvas canvas, PixelRect target, SKPaint paint, bool antiAlias)
    {
        paint.Color = SKColors.White.WithAlpha(Alpha);
        paint.FilterQuality = antiAlias ? SKFilterQuality.High : SKFilterQuality.None;
        canvas.DrawImage(SKImage, target.ToSKRect(), paint);
    }
}
