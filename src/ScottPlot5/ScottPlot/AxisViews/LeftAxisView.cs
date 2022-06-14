using SkiaSharp;
using ScottPlot.Axes;

namespace ScottPlot.AxisViews;

public class LeftAxisView : IAxisView
{
    public IAxis Axis => YAxis;
    public IYAxis YAxis { get; private set; }
    public Edge Edge => Edge.Left;
    public ITickGenerator TickGenerator { get; set; } = new TickGenerators.FixedSpacingTickGenerator();

    public Label Label { get; private set; } = new() { Text = "Vertical Axis", Bold = true, FontSize = 16 };
    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

    /// <summary>
    /// Only render a maximum of this number of ticks
    /// </summary>
    public int MaxTickCount { get; set; } = 1000;

    public LeftAxisView(IYAxis axis)
    {
        YAxis = axis;
    }

    public void RegenerateTicks(PixelRect dataRect)
    {
        Ticks = TickGenerator.GenerateTicks(YAxis.Bottom, YAxis.Top, dataRect.Height);
    }

    public Tick[] GetVisibleTicks()
    {
        return Ticks.Where(tick => YAxis.Contains(tick.Position)).Take(MaxTickCount).ToArray();
    }

    public float Measure()
    {
        float labelWidth = Label.FontSize;
        float largestTickWidth = 0;

        using SKPaint paint = Label.GetPaint();

        foreach (Tick tick in GetVisibleTicks())
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickWidth = Math.Max(largestTickWidth, tickLabelSize.Width + 10);
        }

        return labelWidth + largestTickWidth;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        SKRect figureRect = surface.Canvas.LocalClipBounds;
        PixelRect rect = new(0, dataRect.Left, dataRect.Bottom, dataRect.Top);
        //Draw.DebugRect(surface, rect);
        DrawLabel(surface, rect);
        DrawTicks(surface, rect);
    }

    private void DrawLabel(SKSurface surface, PixelRect dataRect)
    {
        using SKPaint paint = Label.GetPaint();

        surface.Canvas.Save();
        surface.Canvas.Translate(dataRect.LeftCenter.ToSKPoint());
        surface.Canvas.RotateDegrees(-90);
        surface.Canvas.DrawText(Label.Text, 0, Label.FontSize, paint);
        surface.Canvas.Restore();
    }

    private void DrawTicks(SKSurface surface, PixelRect dataRect)
    {
        SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Right,
        };

        var visibleTicks = Ticks.Where(tick => YAxis.Contains(tick.Position)).Take(MaxTickCount);

        foreach (Tick tick in visibleTicks)
        {
            float y = YAxis.GetPixel(tick.Position, dataRect);

            surface.Canvas.DrawLine(dataRect.Right, y, dataRect.Right - 5, y, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, dataRect.Right - 7, y + paint.TextSize * .4f, paint);
        }
    }
}
