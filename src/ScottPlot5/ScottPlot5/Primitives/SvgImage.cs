namespace ScottPlot;

internal class SvgImage : IDisposable
{
    private bool IsDisposed = false;
    public readonly int Width;
    public readonly int Height;
    private readonly MemoryStream Stream;
    public SKCanvas Canvas
    {
        get
        {
            // always create a new Canvas when required
            if (_canvas is null && !IsDisposed)
            {
                SKRect rect = new(0, 0, Width, Height);
                _canvas = SKSvgCanvas.Create(rect, Stream);
            }
            return _canvas!;
        }
    }
    private SKCanvas? _canvas = null;

    public SvgImage(int width, int height)
    {
        Width = width;
        Height = height;
        Stream = new MemoryStream();
    }
    public string GetXml()
    {
        // ensure Canvas is disposed before reading stream content to avoid errors
        _canvas?.Dispose();
        // Canvas no more relevant
        _canvas = null; 
        return Encoding.UTF8.GetString(Stream.ToArray());
    }

    public void Dispose()
    {
        if (IsDisposed)
            return;

        _canvas?.Dispose();
        IsDisposed = true;

        GC.SuppressFinalize(this);
    }
}
