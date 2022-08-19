using ScottPlot.Axis;
using SkiaSharp;

namespace ScottPlot.Plottables;

public class ZoomRectangle : IPlottable
{
    public bool IsVisible { get; set; } = false;
    public IAxes Axes { get; set; } = Axis.Axes.Default;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public Color FillColor = new Color(255, 0, 0).WithAlpha(100);
    public Color EdgeColor = new Color(255, 0, 0).WithAlpha(200);
    public float LineWidth = 2;
    public CoordinateRect Rect;
    public bool HorizontalSpan = false;
    public bool VerticalSpan = false;

    public ZoomRectangle(IXAxis xAxis, IYAxis yAxis)
    {
        Axes.XAxis = xAxis;
        Axes.YAxis = yAxis;
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

    public void Render(SKSurface surface)
    {
        SKRect rect = Axes.GetPixelRect(Rect).ToSKRect();

        if (HorizontalSpan)
        {
            rect.Left = Axes.DataRect.Left;
            rect.Right = Axes.DataRect.Right;
        }

        if (VerticalSpan)
        {
            rect.Bottom = Axes.DataRect.Bottom;
            rect.Top = Axes.DataRect.Top;
        }

        using SKPaint paint = new()
        {
            IsAntialias = true
        };

        paint.Color = FillColor.ToSKColor();
        paint.IsStroke = false;
        surface.Canvas.DrawRect(rect, paint);

        paint.Color = EdgeColor.ToSKColor();
        paint.StrokeWidth = LineWidth;
        paint.IsStroke = true;
        surface.Canvas.DrawRect(rect, paint);
    }
}
