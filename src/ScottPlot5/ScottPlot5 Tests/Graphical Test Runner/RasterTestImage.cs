using SkiaSharp;

namespace GraphicalTestRunner;

public class RasterTestImage
{
    public int Width { get; }
    public int Height { get; }
    public byte[,,] RgbBytes { get; }
    public byte[,] GrayscaleBytes { get; }

    public RasterTestImage(string path)
    {
        using FileStream fs = new(path, FileMode.Open);
        SKBitmap bmp = SKBitmap.Decode(fs);

        Width = bmp.Width;
        Height = bmp.Height;

        ReadOnlySpan<byte> spn = bmp.GetPixelSpan();
        RgbBytes = new byte[bmp.Height, bmp.Width, 3];
        GrayscaleBytes = new byte[bmp.Height, bmp.Width];
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
                GrayscaleBytes[y, x] = (byte)((r + g + b) / 3);
            }
        }
    }
}
