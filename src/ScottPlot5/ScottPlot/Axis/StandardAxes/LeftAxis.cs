using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class LeftAxis : YAxisBase, IYAxis
{
    public Edge Edge => Edge.Left;
    public ITickGenerator TickGenerator { get; set; } = new TickGenerators.ScottPlot4.NumericTickGenerator(true);
    public float Offset { get; set; } = 0;
    public float PixelSize { get; private set; } = 50;
    public float PixelWidth => PixelSize;

    public Label Label { get; private set; } = new()
    {
        Text = "Vertical Axis",
        Bold = true,
        FontSize = 16,
        Rotation = -90
    };

    public void Measure()
    {
        float labelWidth = Label.FontSize;
        float largestTickWidth = 0;

        using SKPaint paint = Label.MakePaint();

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickWidth = Math.Max(largestTickWidth, tickLabelSize.Width + 10);
        }

        PixelSize = labelWidth + largestTickWidth;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        DrawLabel(surface, dataRect);
        DrawTicks(surface, dataRect);
    }

    private void DrawLabel(SKSurface surface, PixelRect dataRect)
    {
        using var paint = Label.MakePaint();
        float xFarLeft = dataRect.Left - Offset - PixelSize;
        float yCenter = dataRect.VerticalCenter;
        Pixel px = new(xFarLeft, yCenter);
        Label.Draw(surface, px, paint);
    }

    private void DrawTicks(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Right,
            Color = Label.Color.ToSKColor(),
        };

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            float x = dataRect.Left - Offset;
            float y = GetPixel(tick.Position, dataRect);

            float majorTickLength = 5;
            surface.Canvas.DrawLine(x, y, x - majorTickLength, y, paint);

            float majorTickLabelPadding = 7;
            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, x - majorTickLabelPadding, y + paint.TextSize * .4f, paint);
        }
    }
}
