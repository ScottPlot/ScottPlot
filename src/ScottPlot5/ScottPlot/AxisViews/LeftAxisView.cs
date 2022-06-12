using SkiaSharp;
using ScottPlot.Axes;

namespace ScottPlot.AxisViews;

internal class LeftAxisView : IAxisView
{
    public IAxis Axis => YAxis;
    public IYAxis YAxis { get; private set; }

    public string Label = "Vertical Axis";

    public LeftAxisView(IYAxis axis)
    {
        YAxis = axis;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        SKRect figureRect = surface.Canvas.LocalClipBounds;
        PixelRect rect = new(0, dataRect.Left, dataRect.Bottom, dataRect.Top);
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

        surface.Canvas.Save();
        surface.Canvas.Translate(rect.LeftCenter.ToSKPoint());
        surface.Canvas.RotateDegrees(-90);
        surface.Canvas.DrawText(Label, 0, paint.TextSize, paint);
        surface.Canvas.Restore();
    }

    private void DrawTicks(SKSurface surface, PixelRect rect)
    {
        SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Right,
        };

        surface.Canvas.DrawText(
            text: Math.Round(YAxis.Top, 3).ToString(),
            x: rect.Right - 3,
            y: rect.Top + paint.TextSize,
            paint: paint);

        surface.Canvas.DrawText(
            text: Math.Round(YAxis.Bottom, 3).ToString(),
            x: rect.Right - 3,
            y: rect.Bottom - (paint.FontSpacing - paint.TextSize),
            paint: paint);
    }
}
