using SkiaSharp;
using ScottPlot.Axes;

namespace ScottPlot.AxisViews;

public class BottomAxisView : IAxisView
{
    public IAxis Axis => XAxis;
    public IXAxis XAxis { get; private set; }

    public string Label = "Horizontal Axis";

    public BottomAxisView(IXAxis axis)
    {
        XAxis = axis;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        SKRect figureRect = surface.Canvas.LocalClipBounds;
        PixelRect rect = new(dataRect.Left, dataRect.Right, dataRect.Bottom, figureRect.Bottom);
        //Draw.DebugRect(surface, rect);
        DrawLabel(surface, rect);
        DrawTicks(surface, rect);
    }

    private void DrawLabel(SKSurface surface, PixelRect rect)
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
            text: Label,
            x: rect.HorizontalCenter,
            y: figRect.Bottom - (paint.FontSpacing - paint.TextSize),
            paint: paint);
    }

    private void DrawTicks(SKSurface surface, PixelRect rect)
    {
        SKPaint paint = new()
        {
            IsAntialias = true,
        };

        paint.TextAlign = SKTextAlign.Left;
        surface.Canvas.DrawText(
            text: Math.Round(XAxis.Left, 3).ToString(),
            x: rect.Left,
            y: rect.Bottom + paint.FontSpacing,
            paint: paint);

        paint.TextAlign = SKTextAlign.Right;
        surface.Canvas.DrawText(
            text: Math.Round(XAxis.Right, 3).ToString(),
            x: rect.Right,
            y: rect.Bottom + paint.FontSpacing,
            paint: paint);
    }
}
