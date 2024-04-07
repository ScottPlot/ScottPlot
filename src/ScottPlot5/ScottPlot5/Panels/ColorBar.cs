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

    /// <summary>
    /// Thickness of the colorbar image (in pixels)
    /// </summary>
    public float Width { get; set; } = 30;

    /// <summary>
    /// Padding between the data area and colorbar image
    /// </summary>
    public float Margin { get; set; } = 15;

    public bool ShowDebugInformation { get; set; } = true;
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
        PixelRect bmpRect = GetColormapBitmapRect(dataRect, size, offset);
        float axisSize = GetAxisWithGeneratedTicks(dataRect).Measure();

        return Edge switch
        {
            Edge.Left => bmpRect.ExpandX(bmpRect.Left - axisSize),
            Edge.Right => bmpRect.ExpandX(bmpRect.Right + axisSize),
            Edge.Bottom => bmpRect.ExpandY(bmpRect.Bottom + axisSize),
            Edge.Top => bmpRect.ExpandY(bmpRect.Top - axisSize),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Return the rectangle to the side of the data area
    /// where the colormap bitmap will be drawn.
    /// </summary>
    private PixelRect GetColormapBitmapRect(PixelRect dataRect, float size, float offset)
    {
        float offset2 = Margin + offset;
        // TODO: use size too
        return Edge switch
        {
            Edge.Left => new(
                left: dataRect.Left - Width - offset2,
                right: dataRect.Left - offset2,
                bottom: dataRect.Bottom,
                top: dataRect.Top),
            Edge.Right => new(
                left: dataRect.Right + offset2,
                right: dataRect.Right + Width + offset2,
                bottom: dataRect.Bottom,
                top: dataRect.Top),
            Edge.Bottom => new(
                left: dataRect.Left,
                right: dataRect.Right,
                bottom: dataRect.Bottom + Width + offset2,
                top: dataRect.Bottom + offset2),
            Edge.Top => new(
                left: dataRect.Left,
                right: dataRect.Right,
                bottom: dataRect.Top - offset2,
                top: dataRect.Top - Width - offset2),
            _ => throw new NotImplementedException($"{Edge}")
        };
    }

    public void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
            return;

        RenderColorbarBitmap(rp, size, offset);
        RenderColorbarAxis(rp, size, offset);
    }

    private void RenderColorbarBitmap(RenderPack rp, float size, float offset)
    {
        PixelRect colormapRect = GetColormapBitmapRect(rp.DataRect, size, offset);
        using SKBitmap bmp = Source.Colormap.GetBitmap(Edge.IsVertical());
        rp.Canvas.DrawBitmap(bmp, colormapRect.ToSKRect());
    }

    private void RenderColorbarAxis(RenderPack rp, float size, float offset)
    {
        IAxis axis = GetAxisWithGeneratedTicks(rp.DataRect);
        PixelRect colormapRect = GetColormapBitmapRect(rp.DataRect, size, offset);

        float offset2 = Edge switch
        {
            Edge.Left => rp.DataRect.Left - colormapRect.Left,
            Edge.Right => colormapRect.Right - rp.DataRect.Right,
            Edge.Bottom => colormapRect.Bottom - rp.DataRect.Bottom,
            Edge.Top => rp.DataRect.Top - colormapRect.Top,
            _ => throw new NotImplementedException(),
        };

        axis.Render(rp, size, offset2);
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
