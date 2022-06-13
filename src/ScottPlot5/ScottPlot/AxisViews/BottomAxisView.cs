using SkiaSharp;
using ScottPlot.Axes;

namespace ScottPlot.AxisViews;

public class BottomAxisView : IAxisView
{
    public IAxis Axis => XAxis;
    public IXAxis XAxis { get; private set; }
    public Edge Edge => Edge.Bottom;
    public ITickGenerator TickGenerator { get; set; } = new TickGenerators.FixedSpacingTickGenerator();

    public Label Label { get; private set; } = new() { Text = "Horizontal Axis", Bold = true, FontSize = 16 };
    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

    /// <summary>
    /// Only render a maximum of this number of ticks
    /// </summary>
    public int MaxTickCount { get; set; } = 1000;

    public BottomAxisView(IXAxis axis)
    {
        XAxis = axis;
    }

    public void RegenerateTicks(PixelRect dataRect)
    {
        Ticks = TickGenerator.GenerateTicks(XAxis.Left, XAxis.Right, dataRect.Width);
    }

    public Tick[] GetVisibleTicks()
    {
        return Ticks.Where(tick => XAxis.Contains(tick.Position)).Take(MaxTickCount).ToArray();
    }

    public float Measure()
    {
        float labelHeight = Label.FontSize;

        float largestTickHeight = Label.FontSize;

        return labelHeight + largestTickHeight + 18;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        SKRect figureRect = surface.Canvas.LocalClipBounds;
        PixelRect rect = new(dataRect.Left, dataRect.Right, dataRect.Bottom, figureRect.Bottom);
        //Draw.DebugRect(surface, rect);
        DrawLabel(surface, rect);
        DrawTicks(surface, rect);
    }

    private void DrawLabel(SKSurface surface, PixelRect dataRect)
    {
        SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
            TextSize = 16,
            FakeBoldText = true,
        };

        PixelRect figRect = PixelRect.FromSKRect(surface.Canvas.LocalClipBounds);

        surface.Canvas.DrawText(
            text: Label.Text,
            x: dataRect.HorizontalCenter,
            y: figRect.Bottom - (paint.FontSpacing - paint.TextSize),
            paint: paint);
    }

    private void DrawTicks(SKSurface surface, PixelRect dataRect)
    {
        SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
        };


        foreach (Tick tick in GetVisibleTicks())
        {
            float x = XAxis.GetPixel(tick.Position, dataRect);

            surface.Canvas.DrawLine(x, dataRect.Bottom, x, dataRect.Bottom + 3, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, x, dataRect.Bottom + 3 + paint.FontSpacing, paint);
        }
    }
}
