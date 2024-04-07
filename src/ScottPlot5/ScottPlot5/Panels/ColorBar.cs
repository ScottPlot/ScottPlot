using System.Data;

namespace ScottPlot.Panels;

/// <summary>
/// An axis panel which displays a colormap and range of values
/// </summary>
public class ColorBar(IHasColorAxis source, Edge edge = Edge.Right) : IPanel
{
    public bool IsVisible { get; set; } = true;

    public IHasColorAxis Source { get; set; } = source;

    public Edge Edge { get; set; } = edge;
    public float Width { get; set; } = 50;
    public float Margin { get; set; } = 15;
    public bool ShowDebugInformation { get; set; } = false;
    public float MinimumSize { get; set; } = 0;
    public float MaximumSize { get; set; } = float.MaxValue;

    public float Measure()
    {
        if (!IsVisible)
            return 0;

        float bitmapAndMarginSize = Width + Margin;

        // use an example DataRect to estimate the size required by the ticks
        PixelRect guessedDataArea = new(0, 600, 400, 0);
        IAxis guessAxis = GetAxisWithGeneratedTicks(guessedDataArea);
        float guessedAxisSize = guessAxis.Measure();

        return bitmapAndMarginSize + guessedAxisSize;
    }

    /// <summary>
    /// Return a rectangle encapsulating the colormap
    /// bitmap plus the axis and ticks.
    /// </summary>
    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        // TODO: use size and offset
        // TODO: include the axis and ticks
        return GetColormapBitmapRect(dataRect);
    }

    /// <summary>
    /// Return the rectangle to the side of the data area
    /// where the colormap bitmap will be drawn.
    /// </summary>
    private PixelRect GetColormapBitmapRect(PixelRect dataRect)
    {
        return Edge switch
        {
            Edge.Left => new(
                left: dataRect.Left - Width - Margin,
                right: dataRect.Left - Margin,
                bottom: dataRect.Bottom,
                top: dataRect.Top),
            Edge.Right => new(
                left: dataRect.Right + Margin,
                right: dataRect.Right + Width + Margin,
                bottom: dataRect.Bottom,
                top: dataRect.Top),
            Edge.Bottom => new(
                left: dataRect.Left,
                right: dataRect.Right,
                bottom: dataRect.Bottom + Width + Margin,
                top: dataRect.Bottom + Margin),
            Edge.Top => new(
                left: dataRect.Left,
                right: dataRect.Right,
                bottom: dataRect.Top - Margin,
                top: dataRect.Top - Width - Margin),
            _ => throw new NotImplementedException($"{Edge}")
        };
    }

    public void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
            return;

        // TODO: use the size and offset to generate the rect
        RenderColorbarBitmap(rp, size, offset);
        RenderColorbarAxis(rp, size, offset + Margin);
    }

    private void RenderColorbarBitmap(RenderPack rp, float size, float offset)
    {
        PixelRect colormapRect = GetPanelRect(rp.DataRect, size, offset);
        using SKBitmap bmp = Source.Colormap.GetBitmap(Edge.IsVertical());
        rp.Canvas.DrawBitmap(bmp, colormapRect.ToSKRect());
    }

    private void RenderColorbarAxis(RenderPack rp, float size, float offset)
    {
        IAxis axis = GetAxisWithGeneratedTicks(rp.DataRect);
        axis.Render(rp, size, offset);
    }

    private IAxis GetAxisWithGeneratedTicks(PixelRect dataRect)
    {
        IAxis axis = Edge switch
        {
            Edge.Left => new AxisPanels.LeftAxis(),
            Edge.Right => new AxisPanels.RightAxis(),
            Edge.Bottom => new AxisPanels.BottomAxis(),
            Edge.Top => new AxisPanels.TopAxis(),
            _ => throw new NotImplementedException(nameof(Edge))
        };

        Range range = Source.GetRange();
        axis.Min = range.Min;
        axis.Max = range.Max;

        float edgeLength = Edge.IsVertical()
            ? dataRect.Height
            : dataRect.Width;

        axis.RegenerateTicks(edgeLength);

        return axis;
    }
}
