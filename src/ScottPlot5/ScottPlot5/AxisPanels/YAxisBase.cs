namespace ScottPlot.AxisPanels;

public abstract class YAxisBase : AxisBase, IYAxis
{
    public double Height => Range.Span;

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        double unitsFromMinValue = position - Min;
        float pxFromEdge = (float)(unitsFromMinValue * pxPerUnit);
        return dataArea.Bottom - pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        float pxFromMinValue = pixel - dataArea.Bottom;
        double unitsFromMinValue = pxFromMinValue / pxPerUnit;
        return Min - unitsFromMinValue;
    }

    public virtual float Measure()
    {
        if (!IsVisible)
            return 0;

        if (!Range.HasBeenSet)
            return SizeWhenNoData;

        using SKPaint paint = new();
        float maxTickLabelWidth = TickGenerator.Ticks.Length > 0
            ? TickGenerator.Ticks.Select(x => TickLabelStyle.Measure(x.Label, paint).Width).Max()
            : 0;

        float axisLabelHeight = string.IsNullOrEmpty(LabelStyle.Text)
            ? EmptyLabelPadding.Horizontal
            : LabelStyle.Measure(LabelText, paint).LineHeight
                + PaddingBetweenTickAndAxisLabels.Horizontal
                + PaddingOutsideAxisLabels.Horizontal;

        return maxTickLabelWidth + axisLabelHeight;
    }

    private PixelRect GetPanelRectangleLeft(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Left - offset - size,
            right: dataRect.Left - offset,
            bottom: dataRect.Bottom,
            top: dataRect.Top);
    }

    private PixelRect GetPanelRectangleRight(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Right + offset,
            right: dataRect.Right + offset + size,
            bottom: dataRect.Bottom,
            top: dataRect.Top);
    }

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        return Edge == Edge.Left
            ? GetPanelRectangleLeft(dataRect, size, offset)
            : GetPanelRectangleRight(dataRect, size, offset);
    }

    public virtual void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
            return;

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);
        float x = Edge == Edge.Left
            ? panelRect.Left + PaddingOutsideAxisLabels.Horizontal
            : panelRect.Right - PaddingOutsideAxisLabels.Horizontal;
        Pixel labelPoint = new(x, rp.DataRect.VerticalCenter);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(rp.Canvas, panelRect, labelPoint, LabelFontColor);
        }

        using SKPaint paint = new();
        LabelAlignment = Alignment.UpperCenter;
        LabelStyle.Render(rp.Canvas, labelPoint, paint);

        DrawTicks(rp, TickLabelStyle, panelRect, TickGenerator.Ticks, this, MajorTickStyle, MinorTickStyle);
        DrawFrame(rp, panelRect, Edge, FrameLineStyle);
    }

    public double GetPixelDistance(double distance, PixelRect dataArea)
    {
        return distance * dataArea.Height / Height;
    }

    public double GetCoordinateDistance(float distance, PixelRect dataArea)
    {
        return distance / (dataArea.Height / Height);
    }

    public void RegenerateTicks(PixelLength size)
    {
        using SKPaint paint = new();
        TickLabelStyle.ApplyToPaint(paint);
        TickGenerator.Regenerate(Range.ToCoordinateRange, Edge, size, paint, TickLabelStyle);
    }
}
