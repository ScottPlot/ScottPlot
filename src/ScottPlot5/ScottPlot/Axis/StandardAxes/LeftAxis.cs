using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class LeftAxis : YAxisBase, IYAxis
{
    public Edge Edge => Edge.Left;
    public ITickGenerator TickGenerator { get; set; } = new TickGenerators.ScottPlot4.NumericTickGenerator(true);

    public Label Label { get; private set; } = new()
    {
        Text = "Vertical Axis",
        Bold = true,
        FontSize = 16,
        Rotation = -90
    };

    public float Measure()
    {
        float labelWidth = Label.FontSize;
        float largestTickWidth = 0;

        using SKPaint paint = Label.MakePaint();

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickWidth = Math.Max(largestTickWidth, tickLabelSize.Width + 10);
        }

        return labelWidth + largestTickWidth;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        PixelRect rect = new(0, dataRect.Left, dataRect.Bottom, dataRect.Top);
        DrawLabel(surface, rect);
        DrawTicks(surface, rect);
    }

    private void DrawLabel(SKSurface surface, PixelRect dataRect)
    {
        using var paint = Label.MakePaint();
        Label.Draw(surface, dataRect.LeftCenter, paint);
    }

    private void DrawTicks(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Right,
        };

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            float y = GetPixel(tick.Position, dataRect);

            surface.Canvas.DrawLine(dataRect.Right, y, dataRect.Right - 5, y, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, dataRect.Right - 7, y + paint.TextSize * .4f, paint);
        }
    }
}
