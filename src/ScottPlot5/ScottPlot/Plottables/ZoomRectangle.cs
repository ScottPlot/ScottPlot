using ScottPlot.Axes;
using SkiaSharp;

namespace ScottPlot.Plottables;

public class ZoomRectangle : PlottableBase
{
    public Color FillColor = new Color(255, 0, 0).WithAlpha(100);
    public Color EdgeColor = new Color(255, 0, 0).WithAlpha(200);
    public float LineWidth = 2;

    public CoordinateRect Rect;

    public bool HorizontalSpan = false;
    public bool VerticalSpan = false;

    public ZoomRectangle(IXAxis xAxis, IYAxis yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
        IsVisible = false;
    }

    public void SetSize(Coordinates c1, Coordinates c2)
    {
        Rect = new(c1, c2);
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

        surface.Canvas.ClipRect(dataRect.ToSKRect());

        using SKPaint paint = new()
        {
            IsAntialias = true
        };

        float l = HorizontalSpan ? dataRect.Left : XAxis.GetPixel(Rect.XMin, dataRect);
        float r = HorizontalSpan ? dataRect.Right : XAxis.GetPixel(Rect.XMax, dataRect);
        float b = VerticalSpan ? dataRect.Bottom : YAxis.GetPixel(Rect.YMin, dataRect);
        float t = VerticalSpan ? dataRect.Top : YAxis.GetPixel(Rect.YMax, dataRect);

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
