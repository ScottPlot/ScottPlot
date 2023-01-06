using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct BitmapHeader
{
    public const int FileHeaderSize = 54;

    public readonly byte MagicNumberHighByte = (byte)'B';
    public readonly byte MagicNumberLowByte = (byte)'M';
    public readonly int Size;
    public readonly short Reserved1;
    public readonly short Reserved2;
    public readonly int Offset = FileHeaderSize;
    public readonly int BitmapInfoHeaderSize = 40;
    public readonly int Width;
    public readonly int Height;
    public readonly short ColorPlanes = 1;
    public readonly short BitsPerPixel = 32;
    public readonly int CompressionMethod = 0; // i.e. No compression
    public readonly int PixelSize;

    // Can all be defaulted to 0
    public readonly int HorizontalResolution;
    public readonly int VerticalResolution;
    public readonly int ColorsInPalette;
    public readonly int ImportantColors;

    public BitmapHeader(int width, int height, int pixelSize, bool flipVertically = false)
    {
        PixelSize = pixelSize;
        Size = pixelSize + FileHeaderSize;
        Width = width;
        Height = flipVertically ? -height : height; // This isn't a hack, this is allowed by the format for uncompressed bitmaps
    }
}
