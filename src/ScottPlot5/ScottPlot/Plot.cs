using SkiaSharp;

namespace ScottPlot;

public class Plot
{
    // TOOD: don't store min/max state in these
    readonly HorizontalAxis XAxis = new();
    readonly VerticalAxis YAxis = new();
    readonly List<IPlottable> Plottables = new();

    public Plot()
    {
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

    public void SetAxisLimits(CoordinateRect rect)
    {
        XAxis.Min = rect.XMin;
        XAxis.Max = rect.XMax;
        YAxis.Min = rect.YMin;
        YAxis.Max = rect.YMax;
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

    public Coordinate GetCoordinate(Pixel pixel, PixelSize figureSize)
    {
        PixelRect figureRect = new(figureSize);
        PixelRect dataRect = figureRect.ShrinkBy(50);

        double x = XAxis.GetCoordinate(pixel.X, dataRect.Left, dataRect.Right);
        double y = YAxis.GetCoordinate(pixel.Y, dataRect.Bottom, dataRect.Top);
        return new Coordinate(x, y);
    }

    public void Render(SKSurface surface)
    {
        SKCanvas canvas = surface.Canvas;
        SKRect bounds = surface.Canvas.LocalClipBounds;
        PixelRect figureRect = new(bounds.Left, bounds.Right, bounds.Bottom, bounds.Top);
        PixelRect dataRect = figureRect.ShrinkBy(50);

        using SKPaint paint = new()
        {
            IsAntialias = true,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        // clear the background
        surface.Canvas.Clear(SKColors.Navy);

        // draw a box around the data area
        paint.Color = SKColors.Yellow;
        canvas.DrawRect(dataRect.ToSKRect(), paint);

        foreach (var plottable in Plottables)
        {
            // TODO: dont store min/max state inside the axes themselves
            plottable.Render(surface, dataRect, XAxis, YAxis);
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
