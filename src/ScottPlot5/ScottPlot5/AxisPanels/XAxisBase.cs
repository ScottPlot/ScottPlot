namespace ScottPlot.AxisPanels;

public abstract class XAxisBase : AxisBase, IXAxis
{
    public double Width => Range.Span;

    public XAxisBase()
    {
        LabelRotation = 0;
    }

    public virtual float Measure()
    {
        if (!IsVisible)
            return 0;

        if (!Range.HasBeenSet)
            return SizeWhenNoData;

        using SKPaint paint = new();

        float tickHeight = MajorTickStyle.Length;

        float maxTickLabelHeight = TickGenerator.Ticks.Length > 0
            ? TickGenerator.Ticks.Select(x => TickLabelStyle.Measure(x.Label, paint).Height).Max()
            : 0;

        float axisLabelHeight = string.IsNullOrEmpty(LabelStyle.Text)
            ? EmptyLabelPadding.Vertical
            : LabelStyle.Measure(LabelText, paint).LineHeight
                + PaddingBetweenTickAndAxisLabels.Vertical
                + PaddingOutsideAxisLabels.Vertical;

        return tickHeight + maxTickLabelHeight + axisLabelHeight;
    }

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Width / Width;
        double unitsFromLeftEdge = position - Min;
        float pxFromEdge = (float)(unitsFromLeftEdge * pxPerUnit);
        return dataArea.Left + pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Width / Width;
        float pxFromLeftEdge = pixel - dataArea.Left;
        double unitsFromEdge = pxFromLeftEdge / pxPerUnit;
        return Min + unitsFromEdge;
    }

    private PixelRect GetPanelRectangleBottom(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Left,
            right: dataRect.Right,
            bottom: dataRect.Bottom + offset + size,
            top: dataRect.Bottom + offset);
    }

    private PixelRect GetPanelRectangleTop(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Left,
            right: dataRect.Right,
            bottom: dataRect.Top - offset,
            top: dataRect.Top - offset - size);
    }

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        return Edge == Edge.Bottom
            ? GetPanelRectangleBottom(dataRect, size, offset)
            : GetPanelRectangleTop(dataRect, size, offset);
    }

    public virtual void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new();

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);

        float y = Edge == Edge.Bottom
            ? panelRect.Bottom - PaddingOutsideAxisLabels.Vertical
            : panelRect.Top + PaddingOutsideAxisLabels.Vertical;

        Pixel labelPoint = new(panelRect.HorizontalCenter, y);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(rp.Canvas, panelRect, labelPoint, LabelFontColor);
        }

        LabelAlignment = Alignment.LowerCenter;
        LabelStyle.Render(rp.Canvas, labelPoint, paint);

        DrawTicks(rp, TickLabelStyle, panelRect, TickGenerator.Ticks, this, MajorTickStyle, MinorTickStyle);
        DrawFrame(rp, panelRect, Edge, FrameLineStyle);
    }

    public double GetPixelDistance(double distance, PixelRect dataArea)
    {
        return distance * dataArea.Width / Width;
    }

    public double GetCoordinateDistance(float distance, PixelRect dataArea)
    {
        return distance / (dataArea.Width / Width);
    }

    public void RegenerateTicks(PixelLength size)
    {
        using SKPaint paint = new();
        TickLabelStyle.ApplyToPaint(paint);
        TickGenerator.Regenerate(Range.ToCoordinateRange, Edge, size, paint, TickLabelStyle);
    }
}
