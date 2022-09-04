using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class BottomAxis : XAxisBase, IXAxis
{
    public Edge Edge => Edge.Bottom;
    public ITickGenerator TickGenerator { get; set; } = new TickGenerators.ScottPlot4.NumericTickGenerator(false);
    public float Offset { get; set; } = 0;
    public float PixelSize { get; private set; } = 50;
    public float PixelHeight => PixelSize;

    public Label Label { get; private set; } = new()
    {
        Text = "Horizontal Axis",
        Bold = true,
        FontSize = 16
    };

    /// <summary>
    /// Only render a maximum of this number of ticks
    /// </summary>
    public int MaxTickCount { get; set; } = 1000;

    public void Measure()
    {
        float labelHeight = Label.FontSize;
        float largestTickHeight = Label.FontSize;
        PixelSize = labelHeight + largestTickHeight + 18;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        DrawLabel(surface, dataRect);
        DrawTicks(surface, dataRect);
        DrawFrame(surface, dataRect);
    }

    private void DrawLabel(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint paint = Label.MakePaint();

        Pixel pt = new(
            x: dataRect.HorizontalCenter,
            y: dataRect.Bottom + paint.FontSpacing + Offset);

        Label.Draw(surface, pt, paint);
    }

    private void DrawTicks(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
            Color = Label.Color.ToSKColor(),
        };

        float yEdge = dataRect.Bottom + Offset;

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            float x = GetPixel(tick.Position, dataRect);

            surface.Canvas.DrawLine(x, yEdge, x, yEdge + 3, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, x, yEdge + 3 + paint.FontSpacing, paint);
        }
    }

    private void DrawFrame(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint linePaint = new()
        {
            IsAntialias = true,
            Color = Label.Color.ToSKColor(),
        };

        surface.Canvas.DrawLine(
            x0: dataRect.Left,
            y0: dataRect.Bottom + Offset,
            x1: dataRect.Right,
            y1: dataRect.Bottom + Offset,
            paint: linePaint);
    }
}
