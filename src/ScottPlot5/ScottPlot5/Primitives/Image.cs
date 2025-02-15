using System.Runtime.InteropServices;
using ScottPlot.IO;

namespace ScottPlot;

/// <summary>
/// Bitmap representation of an image with helper methods for manipulating the image and enabling IO with other platforms
/// </summary>
public class Image : IDisposable
{
    private bool IsDisposed = false;
    protected SKImage SKImage { get; }
    public int Width => SKImage.Width;
    public int Height => SKImage.Height;
    public PixelSize Size => new(Width, Height);

    /// <summary>
    /// Create an Image from the snapshot of an existing Surface
    /// </summary>
    public Image(SKSurface surface)
    {
        SKImage = surface.Snapshot();
    }

    /// <summary>
    /// Create an Image from a file on disk
    /// </summary>
    public Image(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException(filename);

        byte[] bytes = File.ReadAllBytes(filename);
        SKImage = SKImage.FromEncodedData(bytes);
    }

    /// <summary>
    /// Create an Image from a byte array of image data encoded in a common file format
    /// </summary>
    public Image(byte[] bytes)
    {
        SKImage = SKImage.FromEncodedData(bytes);
    }

    /// <summary>
    /// Create an Image from an existing bitmap
    /// </summary>
    public Image(SKBitmap bmp)
    {
        SKImage = SKImage.FromBitmap(bmp);
    }

    /// <summary>
    /// Create a black Image with the given dimensions
    /// </summary>
    public Image(int width, int height) : this(width, height, Colors.Black)
    {

    }

    /// <summary>
    /// Create a black Image with the given dimensions
    /// </summary>
    public Image(PixelSize size) : this((int)size.Width, (int)size.Height, Colors.Black)
    {

    }

    /// <summary>
    /// Create an image filled with the given color
    /// </summary>
    public Image(int width, int height, Color color)
    {
        SKSurface surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;
        canvas.Clear(color.ToSKColor());
        SKImage = surface.Snapshot();
    }

    /// <summary>
    /// Create a grayscale Image from a 2D array of bytes. 
    /// R, G, and B channels will all be set to the given value for each pixel.
    /// </summary>
    public Image(byte[,] pixelArray)
    {
        int width = pixelArray.GetLength(1);
        int height = pixelArray.GetLength(0);

        uint[] pixelValues = new uint[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                byte red = pixelArray[y, x];
                byte green = pixelArray[y, x];
                byte blue = pixelArray[y, x];
                byte alpha = 255;

                uint pixelValue =
                    (uint)(red << 0) +
                    (uint)(green << 8) +
                    (uint)(blue << 16) +
                    (uint)(alpha << 24);

                pixelValues[y * width + x] = pixelValue;
            }
        }

        // https://swharden.com/csdv/skiasharp/array-to-image/
        SKBitmap bmp = new();
        GCHandle gcHandle = GCHandle.Alloc(pixelValues, GCHandleType.Pinned);
        SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        IntPtr ptr = gcHandle.AddrOfPinnedObject();
        int rowBytes = info.RowBytes;
        bmp.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });
        SKImage = SKImage.FromBitmap(bmp);
    }

    /// <summary>
    /// Create an Image from a 3D array of bytes with axes representing Y position, X position, and Color
    /// </summary>
    public Image(byte[,,] pixelArray)
    {
        int width = pixelArray.GetLength(1);
        int height = pixelArray.GetLength(0);

        uint[] pixelValues = new uint[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                byte red = pixelArray[y, x, 0];
                byte green = pixelArray[y, x, 1];
                byte blue = pixelArray[y, x, 2];
                byte alpha = 255;

                uint pixelValue =
                    (uint)(red << 0) +
                    (uint)(green << 8) +
                    (uint)(blue << 16) +
                    (uint)(alpha << 24);

                pixelValues[y * width + x] = pixelValue;
            }
        }

        // https://swharden.com/csdv/skiasharp/array-to-image/
        SKBitmap bmp = new();
        GCHandle gcHandle = GCHandle.Alloc(pixelValues, GCHandleType.Pinned);
        SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        IntPtr ptr = gcHandle.AddrOfPinnedObject();
        int rowBytes = info.RowBytes;
        bmp.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });
        SKImage = SKImage.FromBitmap(bmp);
    }

    /// <summary>
    /// Return the image as a platform-agnostic byte array that can be consumed by any common image library
    /// </summary>
    private byte[] GetBitmapBytes()
    {
        return GetBitmapBytes(SKImage);
    }

    /// <summary>
    /// SkiaSharp cannot natively create BMP files. 
    /// This function creates bitmaps in memory manually.
    /// https://github.com/mono/SkiaSharp/issues/320
    /// </summary>
    private static byte[] GetBitmapBytes(SKImage SKImage)
    {
        using SKBitmap skBitmap = SKBitmap.FromImage(SKImage);

        BitmapHeader header = new(SKImage.Width, SKImage.Height);

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

    /// <summary>
    /// Return the image as a platform-agnostic byte array that can be consumed by any common image library
    /// </summary>
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

    /// <summary>
    /// Draw the image onto the given canvas
    /// </summary>
    public void Render(SKCanvas canvas, PixelRect target, SKPaint paint, bool antiAlias)
    {
        paint.Color = SKColors.White;
        paint.FilterQuality = antiAlias ? SKFilterQuality.High : SKFilterQuality.None;
        canvas.DrawImage(SKImage, target.ToSKRect(), paint);
    }

    /// <summary>
    /// Return the image as a 2D byte array grayscale image with axes representing X and Y positions.
    /// Grayscale values for each pixel are the mean of R, G, and B channels.
    /// </summary>
    public byte[,] GetArrayGrayscale()
    {
        using SKBitmap bmp = SKBitmap.FromImage(SKImage);
        int Width = bmp.Width;
        int Height = bmp.Height;

        ReadOnlySpan<byte> spn = bmp.GetPixelSpan();
        byte[,] GrayscaleBytes = new byte[bmp.Height, bmp.Width];
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                int offset = (y * bmp.Width + x) * bmp.BytesPerPixel;
                byte r = spn[offset + 0];
                byte g = spn[offset + 1];
                byte b = spn[offset + 2];
                GrayscaleBytes[y, x] = (byte)((r + g + b) / 3);
            }
        }

        return GrayscaleBytes;
    }

    /// <summary>
    /// Return the image as a 3D byte array with axes representing X position, Y position, and Color
    /// </summary>
    public byte[,,] GetArrayRGB()
    {
        using SKBitmap bmp = SKBitmap.FromImage(SKImage);
        int Width = bmp.Width;
        int Height = bmp.Height;

        ReadOnlySpan<byte> spn = bmp.GetPixelSpan();
        byte[,,] RgbBytes = new byte[bmp.Height, bmp.Width, 3];
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                int offset = (y * bmp.Width + x) * bmp.BytesPerPixel;
                byte r = spn[offset + 0];
                byte g = spn[offset + 1];
                byte b = spn[offset + 2];
                RgbBytes[y, x, 2] = r;
                RgbBytes[y, x, 1] = g;
                RgbBytes[y, x, 0] = b;
            }
        }

        return RgbBytes;
    }

    /// <summary>
    /// Return a new image with brightness/contrast maximized to fit the range of the data
    /// </summary>
    public Image GetAutoscaledImage()
    {
        byte[,,] values = GetArrayRGB();

        byte maxValue = 0;
        for (int y = 0; y < values.GetLength(0); y++)
            for (int x = 0; x < values.GetLength(1); x++)
                for (int c = 0; c < values.GetLength(2); c++)
                    maxValue = Math.Max(maxValue, values[y, x, c]);

        for (int y = 0; y < values.GetLength(0); y++)
            for (int x = 0; x < values.GetLength(1); x++)
                for (int c = 0; c < values.GetLength(2); c++)
                    values[y, x, c] = (byte)((double)values[y, x, c] / maxValue * 255);

        return new Image(values);
    }

    /// <summary>
    /// Return a new image with dimensions scaled by the given scale factor.
    /// A scale factor of 2.0 will double the size of the returned image.
    /// </summary>
    public Image Scaled(double scale, bool antiAlias = true)
    {
        return Scaled(scale, scale, antiAlias);
    }

    /// <summary>
    /// Return a new image with dimensions scaled by the given scale factors.
    /// A scale factor of 2.0 will double the size of the returned image.
    /// </summary>
    public Image Scaled(double scaleX, double scaleY, bool antiAlias = true)
    {
        int newWidth = (int)(scaleX * Width);
        int newHeight = (int)(scaleY * Height);
        return Resized(newWidth, newHeight, antiAlias);
    }

    /// <summary>
    /// Return a new image resized to fit the given dimensions.
    /// </summary>
    public Image Resized(int newWidth, int newHeight, bool antiAlias = true)
    {
        SKRect newRect = new(0, 0, newWidth, newHeight);

        using SKPaint paint = new() { FilterQuality = antiAlias ? SKFilterQuality.High : SKFilterQuality.None };
        using SKBitmap targetBitmap = new(newWidth, newHeight);
        using SKCanvas targetCanvas = new(targetBitmap);
        using SKBitmap sourceBitmap = SKBitmap.FromImage(SKImage);
        targetCanvas.DrawBitmap(sourceBitmap, newRect, paint);

        using SKImage newImage = SKImage.FromBitmap(targetBitmap);
        byte[] bytes = GetBitmapBytes(newImage);
        return new Image(bytes);
    }
}
