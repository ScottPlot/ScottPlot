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
        SKRect figureRect = surface.Canvas.LocalClipBounds;
        PixelRect rect = new(dataRect.Left, dataRect.Right, dataRect.Bottom, figureRect.Bottom);
        DrawLabel(surface, rect);
        DrawTicks(surface, rect);
    }

    private void DrawLabel(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint paint = Label.MakePaint();
        Pixel pt = new(dataRect.HorizontalCenter, dataRect.Bottom + paint.FontSpacing);
        Label.Draw(surface, pt, paint);
    }

    private void DrawTicks(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
        };

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            float x = GetPixel(tick.Position, dataRect);

            surface.Canvas.DrawLine(x, dataRect.Bottom, x, dataRect.Bottom + 3, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, x, dataRect.Bottom + 3 + paint.FontSpacing, paint);
        }
    }
}
