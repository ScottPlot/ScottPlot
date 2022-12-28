using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public abstract class YAxisBase : IAxis
{
    public double Min
    {
        get => Range.Min;
        set => Range.Min = value;
    }

    public double Max
    {
        get => Range.Max;
        set => Range.Max = value;
    }

    public double Height => Range.Span;

    public virtual CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

    public ITickGenerator TickGenerator { get; set; } = null!;

    public Label Label { get; private set; } = new()
    {
        Text = "Vertical Axis",
        FontName = FontService.SansFontName,
        FontSize = 16,
        Bold = true,
        Rotation = -90,
    };
    public Style.StyledSKFont TickFont { get; set; } = new();

    public abstract Edge Edge { get; }

    public bool ShowDebugInformation { get; set; } = false;

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
        float tickSpace = MeasureTicks() + 5;
        float labelSpace = Label.Measure().Height + 5;
        return tickSpace + labelSpace;
    }

    private float MeasureTicks()
    {
        using SKPaint paint = new(TickFont.GetFont());
        float largestTickWidth = 0;

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickWidth = Math.Max(largestTickWidth, tickLabelSize.Width + 10);
        }

        return largestTickWidth;
    }

    public void Render(SKSurface surface, PixelRect dataRect, float size, float offset)
    {
        PixelRect panelRect = new(
            left: dataRect.Left - offset - size,
            right: dataRect.Left - offset,
            bottom: dataRect.Bottom,
            top: dataRect.Top);

        float textDistanceFromEdge = 10;
        Pixel labelPoint = new(panelRect.Left + textDistanceFromEdge, dataRect.VerticalCenter);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(surface.Canvas, panelRect, labelPoint, Label.Color);
        }

        Label.Alignment = Alignment.UpperCenter;
        Label.Rotation = -90;
        Label.Draw(surface.Canvas, labelPoint);

        using SKFont tickFont = TickFont.GetFont();
        var ticks = TickGenerator.GetVisibleTicks(Range);
        AxisRendering.DrawTicks(surface, tickFont, panelRect, Label.Color, ticks, this);
        AxisRendering.DrawFrame(surface, panelRect, Edge, Label.Color);
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
