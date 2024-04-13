/* Additional Resources:
 * https://learn.microsoft.com/en-us/windows/win32/wmdm/-bitmapinfoheader
 * https://en.wikipedia.org/wiki/BMP_file_format
 * https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bmp.htm
 */
using System.Runtime.InteropServices;

namespace ScottPlot.IO;

/// <summary>
/// This struct holds values that define the first several bytes of a bitmap file.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct BitmapHeader
{
    public const int FileHeaderSize = 54; // number of bytes in the bitmap header
    public readonly byte MagicNumberHighByte = (byte)'B';
    public readonly byte MagicNumberLowByte = (byte)'M';
    public readonly int Size; // number of bytes in the image
    public readonly short Reserved1;
    public readonly short Reserved2;
    public readonly int Offset = FileHeaderSize; // byte where the image data begins
    public readonly int BitmapInfoHeaderSize = 40;
    public readonly int Width; // pixel width of the image
    public readonly int Height; // pixel height of the image (a negative value means rows are encoded top-to-bottom)
    public readonly short ColorPlanes = 1; // always 1 for standard images
    public readonly short BitsPerPixel = 32; // typically 8 (grayscale), 24 (RGB), or 32 (RGBA)
    public readonly int CompressionMethod = 0; // 0 means no compression
    public readonly int PixelSize = 0; // Size of the compressed image. 0 is valid if compression is disabled.
    public readonly int HorizontalResolution; // pixels per meter (0 for undefined)
    public readonly int VerticalResolution; // pixels per meter (0 for undefined)
    public readonly int ColorsInPalette; // 0 for default
    public readonly int ImportantColors; // 0 indicates every color is important

    public BitmapHeader(int width, int height)
    {
        Size = width * Math.Abs(height) * BitsPerPixel / 8 + FileHeaderSize;
        Width = width;
        Height = -height;
    }
}
