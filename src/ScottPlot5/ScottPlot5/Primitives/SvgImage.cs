namespace ScottPlot;

internal class SvgImage : IDisposable
{
    private bool IsDisposed = false;
    public readonly int Width;
    public readonly int Height;
    private readonly MemoryStream Stream;
    public SKCanvas Canvas => _canvas ?? throw new InvalidOperationException("Canvas can NOT be access after rendering!");
    private SKCanvas? _canvas;

    public SvgImage(int width, int height)
    {
        Width = width;
        Height = height;
        Stream = new MemoryStream();
        SKRect rect = new(0, 0, Width, Height);
        _canvas = SKSvgCanvas.Create(rect, Stream);
    }
    ~SvgImage()
    {
        // Release unmanaged resources if the Dispose method wasn't called explicitly
        Dispose(false);
    }
    public string GetXml()
    {
        // The SVG document is not guaranteed to be valid until the canvas is disposed
        // See: https://learn.microsoft.com/en-us/dotnet/api/skiasharp.sksvgcanvas?view=skiasharp-2.88#remarks
        _canvas?.Dispose();
        // Canvas no more relevant
        _canvas = null;
        return Encoding.UTF8.GetString(Stream.ToArray());
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Prevent the finalizer from running
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                _canvas?.Dispose();
                Stream.Dispose();
            }

            // Dispose unmanaged resources

            IsDisposed = true;
        }
    }

}
