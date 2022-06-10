using System.Diagnostics;
using SkiaSharp;

namespace ScottPlot;

public class Plot
{
    // TOOD: don't store min/max state in these
    readonly HorizontalAxis XAxis = new();
    readonly VerticalAxis YAxis = new();
    readonly List<IPlottable> Plottables = new();

    // TODO: expose this so the user can customize it
    public bool ShowBenchmark
    {
        get => DebugBenchmark.IsVisible;
        set => DebugBenchmark.IsVisible = value;
    }
    readonly Plottables.DebugBenchmark DebugBenchmark = new();

    readonly Plottables.ZoomRectangle ZoomRectangle;

    /// <summary>
    /// Any state stored across renders can be stored here.
    /// </summary>
    private RenderInformation LastRenderInfo;

    public Plot()
    {
        ZoomRectangle = new(XAxis, YAxis);
        CoordinateRect bounds = new(-10, 10, -10, 10);
        SetAxisLimits(bounds);
    }

    public void Add(IPlottable plottable)
    {
        Plottables.Add(plottable);
    }

    public void Clear()
    {
        Plottables.Clear();
    }

    public IPlottable[] GetPlottables()
    {
        return Plottables.ToArray();
    }

    public void SetAxisLimits(double xMin, double xMax, double yMin, double yMax)
    {
        XAxis.Min = xMin;
        XAxis.Max = xMax;
        YAxis.Min = yMin;
        YAxis.Max = yMax;
    }

    public void SetAxisLimits(CoordinateRect rect)
    {
        XAxis.Min = rect.XMin;
        XAxis.Max = rect.XMax;
        YAxis.Min = rect.YMin;
        YAxis.Max = rect.YMax;
    }

    public void MousePan(CoordinateRect originalLimits, Pixel mouseDown, Pixel mouseNow)
    {
        double pxPerUnitx = LastRenderInfo.DataRect.Width / XAxis.Width;
        double pxPerUnity = LastRenderInfo.DataRect.Height / YAxis.Height;

        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = mouseNow.Y - mouseDown.Y;

        double deltaX = pixelDeltaX / pxPerUnitx;
        double deltaY = pixelDeltaY / pxPerUnity;

        // pan in the direction opposite of the mouse movement
        SetAxisLimits(originalLimits.WithPan(-deltaX, deltaY));
    }

    public void MouseZoom(CoordinateRect originalLimits, Pixel mouseDown, Pixel mouseNow)
    {
        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = mouseNow.Y - mouseDown.Y;

        double deltaFracX = pixelDeltaX / (Math.Abs(pixelDeltaX) + LastRenderInfo.DataRect.Width);
        double fracX = Math.Pow(10, deltaFracX);

        double deltaFracY = -pixelDeltaY / (Math.Abs(pixelDeltaY) + LastRenderInfo.DataRect.Height);
        double fracY = Math.Pow(10, deltaFracY);

        SetAxisLimits(originalLimits.WithZoom(fracX, fracY));
    }

    public void MouseZoom(double fracX, double fracY, Pixel mouseNow)
    {
        Coordinate mouseCoordinate = GetCoordinate(mouseNow);
        SetAxisLimits(GetAxisLimits().WithZoom(fracX, fracY, mouseCoordinate.X, mouseCoordinate.Y));
    }

    public void MouseZoomRectangle(Pixel mouseDown, Pixel mouseNow)
    {
        Coordinate downCoordinate = GetCoordinate(mouseDown);
        Coordinate nowCoordinate = GetCoordinate(mouseNow);
        CoordinateRect rect = new(downCoordinate, nowCoordinate);
        ZoomRectangle.SetSize(rect);
    }

    public void MouseZoomRectangleClear(bool applyZoom)
    {
        if (applyZoom)
        {
            SetAxisLimits(ZoomRectangle.Rect);
        }

        ZoomRectangle.Clear();
    }

    public CoordinateRect GetAxisLimits()
    {
        return new CoordinateRect(XAxis.Min, XAxis.Max, YAxis.Min, YAxis.Max);
    }

    public Pixel GetPixel(Coordinate coord, PixelRect rect)
    {
        float x = XAxis.GetPixel(coord.X, rect.Left, rect.Right);
        float y = YAxis.GetPixel(coord.Y, rect.Bottom, rect.Top);
        return new Pixel(x, y);
    }

    /// <summary>
    /// Return the coordinate for a specific pixel on the most recent render.
    /// </summary>
    public Coordinate GetCoordinate(Pixel pixel)
    {
        PixelRect dataRect = LastRenderInfo.DataRect;
        double x = XAxis.GetCoordinate(pixel.X, dataRect.Left, dataRect.Right);
        double y = YAxis.GetCoordinate(pixel.Y, dataRect.Bottom, dataRect.Top);
        return new Coordinate(x, y);
    }

    private PixelRect GetDataAreaRect(PixelRect figureRect)
    {
        // NOTE: eventually this will measure strings to calculate the ideal layout
        PixelPadding DataAreaPadding = new(40, 10, 30, 20);
        return figureRect.Contract(DataAreaPadding);
    }

    public RenderInformation Render(SKSurface surface)
    {
        Stopwatch SW = Stopwatch.StartNew();
        RenderInformation renderInfo = new();

        // analyze axes, determine ticks, measure strings, etc. to calculate layout
        renderInfo.FigureRect = PixelRect.FromSKRect(surface.Canvas.LocalClipBounds);
        renderInfo.DataRect = GetDataAreaRect(renderInfo.FigureRect);
        renderInfo.ElapsedLayout = SW.Elapsed;
        SW.Restart();

        // perform all renders
        RenderBackground(surface);
        RenderPlottables(surface, renderInfo.DataRect);
        RenderAxes(surface, renderInfo.DataRect);
        renderInfo.ElapsedRender = SW.Elapsed;
        RenderZoomRectangle(surface, renderInfo.DataRect);
        RenderDebugInfo(surface, renderInfo.DataRect, renderInfo.ElapsedMilliseconds);
        LastRenderInfo = renderInfo;
        return renderInfo;
    }

    private void RenderBackground(SKSurface surface)
    {
        surface.Canvas.Clear(SKColors.Navy);
    }

    private void RenderPlottables(SKSurface surface, PixelRect dataRect)
    {
        foreach (var plottable in Plottables.Where(x => x.IsVisible))
        {
            if (plottable.XAxis is null)
                plottable.XAxis = XAxis;
            if (plottable.YAxis is null)
                plottable.YAxis = YAxis;
            plottable.Render(surface, dataRect);
        }
    }

    private void RenderAxes(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
        };

        paint.Color = SKColors.Yellow;
        surface.Canvas.DrawRect(dataRect.ToSKRect(), paint);
    }

    private void RenderZoomRectangle(SKSurface surface, PixelRect dataRect)
    {
        if (ZoomRectangle.IsVisible)
        {
            ZoomRectangle.Render(surface, dataRect);
        }
    }

    private void RenderDebugInfo(SKSurface surface, PixelRect dataRect, double elapsedMilliseconds)
    {
        if (DebugBenchmark.IsVisible)
        {
            DebugBenchmark.ElapsedMilliseconds = elapsedMilliseconds;
            DebugBenchmark.Render(surface, dataRect);
        }
    }

    public byte[] GetImageBytes(int width, int height, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(info);
        Render(surface);
        SKImage snap = surface.Snapshot();
        SKData data = snap.Encode(format, quality);
        byte[] bytes = data.ToArray();
        return bytes;
    }

    public void SaveImage(int width, int height, string path, int quality = 100)
    {
        SKEncodedImageFormat format;

        if (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            format = SKEncodedImageFormat.Png;
        else if (path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            format = SKEncodedImageFormat.Jpeg;
        else if (path.EndsWith(".jepg", StringComparison.OrdinalIgnoreCase))
            format = SKEncodedImageFormat.Jpeg;
        else if (path.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
            format = SKEncodedImageFormat.Bmp;
        else
            throw new ArgumentException("unsupported image format");

        byte[] bytes = GetImageBytes(width, height, format, quality);
        File.WriteAllBytes(path, bytes);
    }
}
