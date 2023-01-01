using ScottPlot.Axis;
using ScottPlot.Axis.StandardAxes;
using SkiaSharp;

namespace ScottPlot.Panels;

public class ColorBar : IPanel
{
    public bool IsVisible { get; set; } = true;

    public IHasColorAxis Source { get; set; }

    public Edge Edge { get; set; }
    public float Width { get; set; } = 50;
    public float Margin { get; set; } = 15;
    public bool ShowDebugInformation { get; set; } = false;

    public ColorBar(IHasColorAxis source, Edge edge = Edge.Right)
    {
        Source = source;
        Edge = edge;
    }

    // Unfortunately the size of the axis depends on the size of the plotting window, so we just have to guess here. 2000 should be larger than most
    public float Measure() => IsVisible ? Margin + GetAxis(2000).Measure() + Width : 0;

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        if (!IsVisible)
            return PixelRect.Zero;

        return Edge switch
        {
            Edge.Left => new SKRect(dataRect.Left - Width, dataRect.Top, dataRect.Left, dataRect.Top + dataRect.Height).ToPixelRect(),
            Edge.Right => new SKRect(dataRect.Right, dataRect.Top, dataRect.Right + Width, dataRect.Top + dataRect.Height).ToPixelRect(),
            Edge.Bottom => new SKRect(dataRect.Left, dataRect.Bottom, dataRect.Left + dataRect.Width, dataRect.Bottom + Width).ToPixelRect(),
            Edge.Top => new SKRect(dataRect.Left, dataRect.Top - Width, dataRect.Left + dataRect.Width, dataRect.Top).ToPixelRect(),
            _ => throw new NotImplementedException()
        };
    }

    public void Render(SKSurface surface, PixelRect dataRect, float size, float offset)
    {
        if (!IsVisible)
            return;

        using var _ = new SKAutoCanvasRestore(surface.Canvas);

        PixelRect panelRect = GetPanelRect(dataRect, size, offset);

        SKPoint marginTranslation = GetTranslation(Margin);
        SKPoint axisTranslation = GetTranslation(Width);

        using var bmp = GetBitmap();

        surface.Canvas.Translate(marginTranslation);
        surface.Canvas.DrawBitmap(bmp, panelRect.ToSKRect());

        var colorbarLength = Edge.IsVertical() ? dataRect.Height : dataRect.Width;
        var axis = GetAxis(colorbarLength);

        surface.Canvas.Translate(axisTranslation);
        axis.Render(surface, dataRect, size, offset);
    }

    private SKPoint GetTranslation(float magnitude) => Edge switch
    {
        Edge.Left => new(-magnitude, 0),
        Edge.Right => new(magnitude, 0),
        Edge.Bottom => new(0, magnitude),
        Edge.Top => new(0, -magnitude),
        _ => throw new ArgumentOutOfRangeException(nameof(Edge))
    };

    private SKBitmap GetBitmap()
    {
        uint[] argbs = Enumerable.Range(0, 256).Select(i => Source.Colormap.GetColor((Edge.IsVertical() ? 255 - i : i) / 255f).ARGB).ToArray();

        int bmpWidth = Edge.IsVertical() ? 1 : 256;
        int bmpHeight = !Edge.IsVertical() ? 1 : 256;

        return Drawing.BitmapFromArgbs(argbs, bmpWidth, bmpHeight);
    }

    private IAxis GetAxis(float length)
    {
        IAxis axis = Edge switch
        {
            Edge.Left => new LeftAxis(),
            Edge.Right => new RightAxis(),
            Edge.Bottom => new BottomAxis(),
            Edge.Top => new TopAxis(),
            _ => throw new ArgumentOutOfRangeException(nameof(Edge))
        };

        axis.Label.Text = "";

        var range = Source.GetRange();
        axis.Min = range.Min;
        axis.Max = range.Max;

        axis.TickGenerator.Regenerate(axis.Range, length);

        return axis;
    }
}
