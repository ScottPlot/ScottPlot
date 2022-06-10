using SkiaSharp;

namespace ScottPlot.Plottables;

internal class ZoomRectangle : IPlottable
{
    public bool IsVisible { get; set; } = false;
    public SKColor FillColor = SKColors.Red.WithAlpha(100);
    public SKColor EdgeColor = SKColors.Red.WithAlpha(200);
    public float LineWidth = 2;
    public CoordinateRect Rect;
    public HorizontalAxis? XAxis { get; set; }
    public VerticalAxis? YAxis { get; set; }

    public ZoomRectangle(HorizontalAxis xAxis, VerticalAxis yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
    }

    public void SetSize(CoordinateRect rect)
    {
        Rect = rect;
        IsVisible = true;
    }

    public void Clear()
    {
        IsVisible = false;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        if (XAxis is null || YAxis is null)
            throw new InvalidOperationException("Both axes must be set before first render");

        using SKPaint paint = new()
        {
            IsAntialias = true
        };

        float l = XAxis.GetPixel(Rect.XMin, dataRect.Left, dataRect.Right);
        float r = XAxis.GetPixel(Rect.XMax, dataRect.Left, dataRect.Right);
        float b = YAxis.GetPixel(Rect.YMin, dataRect.Bottom, dataRect.Top);
        float t = YAxis.GetPixel(Rect.YMax, dataRect.Bottom, dataRect.Top);
        SKRect rect = new(l, t, r, b);

        paint.Color = FillColor;
        paint.IsStroke = false;
        surface.Canvas.DrawRect(rect, paint);

        paint.Color = EdgeColor;
        paint.StrokeWidth = LineWidth;
        paint.IsStroke = true;
        surface.Canvas.DrawRect(rect, paint);
    }
}
