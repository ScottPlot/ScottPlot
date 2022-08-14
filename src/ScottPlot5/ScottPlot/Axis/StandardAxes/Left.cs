using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class Left : IYAxis
{
    public AxisTranslation.IAxisTranslator Translator => YTranslator;
    public AxisTranslation.IYAxisTranslator YTranslator { get; private set; }
    public Edge Edge => Edge.Left;
    public ITickGenerator TickGenerator { get; set; } = new TickGenerators.ScottPlot4.NumericTickGenerator(true);

    public Label Label { get; private set; } = new()
    {
        Text = "Vertical Axis",
        Bold = true,
        FontSize = 16,
        Rotation = -90
    };

    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

    /// <summary>
    /// Only render a maximum of this number of ticks
    /// </summary>
    public int MaxTickCount { get; set; } = 1000;

    public Left()
    {
        YTranslator = new AxisTranslation.LinearYAxisTranslator();
    }

    public void RegenerateTicks(PixelRect dataRect)
    {
        Ticks = TickGenerator.GenerateTicks(Translator.Range, dataRect.Height);
    }

    public Tick[] GetVisibleTicks()
    {
        return Ticks.Where(tick => Translator.Contains(tick.Position)).Take(MaxTickCount).ToArray();
    }

    public float Measure()
    {
        float labelWidth = Label.FontSize;
        float largestTickWidth = 0;

        using SKPaint paint = Label.MakePaint();

        foreach (Tick tick in GetVisibleTicks())
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

        var visibleTicks = Ticks.Where(tick => Translator.Contains(tick.Position)).Take(MaxTickCount);

        foreach (Tick tick in visibleTicks)
        {
            float y = Translator.GetPixel(tick.Position, dataRect);

            surface.Canvas.DrawLine(dataRect.Right, y, dataRect.Right - 5, y, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, dataRect.Right - 7, y + paint.TextSize * .4f, paint);
        }
    }
}
