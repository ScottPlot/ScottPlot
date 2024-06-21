using SkiaSharp;
using System.IO;

namespace ScottPlot;

internal class SvgImage : IDisposable
{
    private bool IsDisposed = false;
    public readonly int Width;
    public readonly int Height;
    private readonly MemoryStream Stream;
    public readonly SKCanvas Canvas;

    public SvgImage(int width, int height)
    {
        Width = width;
        Height = height;
        SKRect rect = new(0, 0, width, height);
        Stream = new MemoryStream();
        Canvas = SKSvgCanvas.Create(rect, Stream);
    }

    public string GetXml()
    {
        return Encoding.UTF8.GetString(Stream.ToArray()) + "</svg>";
    }

    public void Dispose()
    {
        if (IsDisposed)
            return;

        Canvas.Dispose();
        IsDisposed = true;

        GC.SuppressFinalize(this);
    }
}
