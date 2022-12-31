using ScottPlot.Axis;
using ScottPlot.Style;
using SkiaSharp;

namespace ScottPlot.Plottables;

public class ZoomRectangle : IPlottable
{
    public bool IsVisible { get; set; } = false;
    public IAxes Axes { get; set; } = Axis.Axes.Default;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;
    public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

    public Color FillColor = new Color(255, 0, 0).WithAlpha(100);

    public LineStyle LineStyle { get; } = new() { Color = new Color(255, 0, 0).WithAlpha(200) };

    public Pixel MouseDown { get; private set; }
    public Pixel MouseUp { get; private set; }
    public bool HorizontalSpan = false;
    public bool VerticalSpan = false;

    public ZoomRectangle(IXAxis xAxis, IYAxis yAxis)
    {
        Axes.XAxis = xAxis;
        Axes.YAxis = yAxis;
    }

    public void Update(Pixel mouseDown, Pixel mouseUp)
    {
        MouseDown = mouseDown;
        MouseUp = mouseUp;
        IsVisible = true;
    }

    public void Clear()
    {
        IsVisible = false;
    }

    public void Render(SKSurface surface)
    {
        Coordinates coordDown = Axes.GetCoordinates(MouseDown);
        Coordinates coordUp = Axes.GetCoordinates(MouseUp);
        CoordinateRect coordRect = new(coordDown, coordUp);
        PixelRect pxRect = Axes.GetPixelRect(coordRect);
        SKRect rect = pxRect.ToSKRect();

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

        paint.Color = LineStyle.Color.ToSKColor();
        paint.StrokeWidth = LineStyle.Width;
        paint.IsStroke = true;
        surface.Canvas.DrawRect(rect, paint);
    }
}
