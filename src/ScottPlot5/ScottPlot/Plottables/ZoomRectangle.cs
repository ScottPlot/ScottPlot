using SkiaSharp;

namespace ScottPlot.Plottables;

internal class ZoomRectangle : IPlottable
{
    public bool IsVisible { get; set; } = false;
    public SKColor FillColor = SKColors.Red.WithAlpha(100);
    public SKColor EdgeColor = SKColors.Red.WithAlpha(200);
    public float LineWidth = 2;
    public CoordinateRect Rect;

    public void SetSize(CoordinateRect rect)
    {
        Rect = rect;
        IsVisible = true;
    }

    public void Clear()
    {
        IsVisible = false;
    }

    public void Render(SKSurface surface, PixelRect dataRect, HorizontalAxis xAxis, VerticalAxis yAxis)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true
        };

        float l = xAxis.GetPixel(Rect.XMin, dataRect.Left, dataRect.Right);
        float r = xAxis.GetPixel(Rect.XMax, dataRect.Left, dataRect.Right);
        float b = yAxis.GetPixel(Rect.YMin, dataRect.Bottom, dataRect.Top);
        float t = yAxis.GetPixel(Rect.YMax, dataRect.Bottom, dataRect.Top);
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
