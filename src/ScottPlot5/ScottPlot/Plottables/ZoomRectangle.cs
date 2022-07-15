using ScottPlot.Axes;
using SkiaSharp;

namespace ScottPlot.Plottables;

public class ZoomRectangle : PlottableBase
{
    public Color FillColor = new Color(255, 0, 0).WithAlpha(100);
    public Color EdgeColor = new Color(255, 0, 0).WithAlpha(200);
    public float LineWidth = 2;
    public CoordinateRect Rect;

    public ZoomRectangle(IXAxis xAxis, IYAxis yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
        IsVisible = false;
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

    public override void Render(SKSurface surface, PixelRect dataRect)
    {
        if (XAxis is null || YAxis is null)
            throw new InvalidOperationException("Both axes must be set before first render");

        using SKPaint paint = new()
        {
            IsAntialias = true
        };

        float l = XAxis.GetPixel(Rect.XMin, dataRect);
        float r = XAxis.GetPixel(Rect.XMax, dataRect);
        float b = YAxis.GetPixel(Rect.YMin, dataRect);
        float t = YAxis.GetPixel(Rect.YMax, dataRect);
        SKRect rect = new(l, t, r, b);

        paint.Color = FillColor.ToSKColor();
        paint.IsStroke = false;
        surface.Canvas.DrawRect(rect, paint);

        paint.Color = EdgeColor.ToSKColor();
        paint.StrokeWidth = LineWidth;
        paint.IsStroke = true;
        surface.Canvas.DrawRect(rect, paint);
    }
}
