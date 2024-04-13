namespace ScottPlot.AxisPanels;

public abstract class XAxisBase : AxisBase, IAxis
{
    public double Width => Range.Span;

    public XAxisBase()
    {
        Label.Rotation = 0;
    }

    public float Measure()
    {
        if (!IsVisible)
            return 0;

        if (!Range.HasBeenSet)
            return SizeWhenNoData;

        using SKPaint paint = new();

        float tickHeight = MajorTickStyle.Length;

        TickLabelStyle.ApplyToPaint(paint);
        float lineHeight = paint.FontSpacing;
        int numberOfLines = TickGenerator.Ticks.Select(x => x.Label).Select(x => x.Split('\n').Length).FirstOrDefault();
        float tickLabelHeight = lineHeight * numberOfLines;

        float axisLabelHeight = 0;
        if (Label.IsVisible && !string.IsNullOrWhiteSpace(Label.Text))
        {
            Label.ApplyToPaint(paint);
            axisLabelHeight = paint.FontSpacing;
        }

        float spaceBetweenTicksAndAxisLabel = 10;

        return tickHeight + tickLabelHeight + spaceBetweenTicksAndAxisLabel + axisLabelHeight;
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

    public void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
            return;

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);

        float textDistanceFromEdge = 10;
        Pixel labelPoint = new(panelRect.HorizontalCenter, panelRect.Bottom - textDistanceFromEdge);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(rp.Canvas, panelRect, labelPoint, Label.ForeColor);
        }

        Label.Alignment = Alignment.LowerCenter;
        Label.Render(rp.Canvas, labelPoint);

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
        TickGenerator.Regenerate(Range.ToCoordinateRange, Edge, size, paint);
    }
}
