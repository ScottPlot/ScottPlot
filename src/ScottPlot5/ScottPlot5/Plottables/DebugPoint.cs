using ScottPlot.Axis;
using SkiaSharp;

namespace ScottPlot.Plottables;

public class DebugPoint : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = Axis.Axes.Default;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;
    public IEnumerable<LegendItem> LegendItems => EnumerableHelpers.One(
        new LegendItem {
            Label = Label,
            Marker = new(Style.MarkerShape.Circle, Color)
        });
    
    public Coordinates Position { get; set; }
    public Color Color { get; set; } = new(255, 00, 255);
    public string? Label { get; set; }

    public DebugPoint()
    {

    }

    public DebugPoint(double x, double y, Color color)
    {
        Position = new(x, y);
        Color = color;
    }

    public void Render(SKSurface surface)
    {
        surface.Canvas.ClipRect(Axes.DataRect.ToSKRect());

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Color = Color.WithAlpha(200).ToSKColor(),
            StrokeWidth = 1,
            IsStroke = true,
            PathEffect = SKPathEffect.CreateDash(new float[] { 4, 4, }, 0),
        };

        float x = Axes.GetPixelX(Position.X);
        float y = Axes.GetPixelY(Position.Y);

        SKCanvas canvas = surface.Canvas;
        canvas.DrawLine(x, Axes.DataRect.Top, x, Axes.DataRect.Bottom, paint);
        canvas.DrawLine(Axes.DataRect.Left, y, Axes.DataRect.Right, y, paint);
        canvas.DrawCircle(x, y, 5, paint);

        paint.Color = Color.ToSKColor();
        paint.IsStroke = false;
        SKTypeface tf = SKTypeface.FromFamilyName("consolas");
        SKFont font = new(tf, size: 12);
        canvas.DrawText($"X={Position.X:N3}", x + 3, y - 1.5f * font.Size, font, paint);
        canvas.DrawText($"Y={Position.Y:N3}", x + 3, y - .5f * font.Size, font, paint);
    }
}
