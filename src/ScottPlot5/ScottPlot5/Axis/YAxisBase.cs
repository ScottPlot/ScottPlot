namespace ScottPlot.Axis.StandardAxes;

public abstract class YAxisBase : AxisBase, IAxis
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

    public float Measure()
    {
        if (!IsVisible)
            return 0;

        float largestTickSize = MeasureTicks();
        float largestTickLabelSize = Label.Measure().Height;
        float spaceBetweenTicksAndAxisLabel = 15;
        return largestTickSize + largestTickLabelSize + spaceBetweenTicksAndAxisLabel;
    }

    private float MeasureTicks()
    {
        using SKPaint paint = new();
        TickFont.ApplyToPaint(paint);

        float largestTickWidth = 0;

        foreach (Tick tick in TickGenerator.Ticks)
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickWidth = Math.Max(largestTickWidth, tickLabelSize.Width + 10);
        }

        return largestTickWidth;
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

    public void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
            return;

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);

        float textDistanceFromEdge = 10;
        Pixel labelPoint = new(panelRect.Left + textDistanceFromEdge, rp.DataRect.VerticalCenter);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(rp.Canvas, panelRect, labelPoint, Label.Font.Color);
        }

        Label.Alignment = Edge == Edge.Left ? Alignment.UpperCenter : Alignment.LowerCenter;
        Label.Rotation = Edge == Edge.Left ? -90 : 90;
        Label.Draw(rp.Canvas, labelPoint);

        DrawTicks(rp, TickFont, panelRect, TickGenerator.Ticks, this, MajorTickStyle, MinorTickStyle);
        DrawFrame(rp, panelRect, Edge, FrameLineStyle);
    }

    public double GetPixelDistance(double distance, PixelRect dataArea)
    {
        return distance * dataArea.Height / Height;
    }

    public double GetCoordinateDistance(double distance, PixelRect dataArea)
    {
        return distance / (dataArea.Height / Height);
    }
}
