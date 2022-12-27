using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public abstract class XAxisBase : IAxis
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

    public double Width => Range.Span;

    public virtual CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

    public ITickGenerator TickGenerator { get; set; } = null!;

    public Label Label { get; private set; } = new()
    {
        Text = "Horizontal Axis",
        FontName = FontService.DefaultFontName,
        FontSize = 16,
        Bold = true,
    };

    public Style.StyledSKFont TickFont { get; set; } = new();
    public abstract Edge Edge { get; }
    public bool ShowDebugInformation { get; set; } = false;

    public float Measure()
    {
        float tickSpace = MeasureTicks() + 5;
        float labelSpace = Label.Measure().Height + 5;
        return tickSpace + labelSpace;
    }

    private float MeasureTicks()
    {
        using SKPaint paint = new(TickFont.GetFont());
        float largestTickHeight = 0;

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickHeight = Math.Max(largestTickHeight, tickLabelSize.Height + 10);
        }

        return largestTickHeight;
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

    public void Render(SKSurface surface, PixelRect dataRect, float size, float offset)
    {
        PixelRect panelRect = new(
            left: dataRect.Left,
            right: dataRect.Right,
            bottom: dataRect.Bottom + offset + size,
            top: dataRect.Bottom + offset);

        float textDistanceFromEdge = 10;
        Pixel labelPoint = new(panelRect.HorizontalCenter, panelRect.Bottom - textDistanceFromEdge);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(surface.Canvas, panelRect, labelPoint, Label.Color);
        }

        Label.Alignment = Alignment.LowerCenter;
        Label.Rotation = 0;
        Label.Draw(surface.Canvas, labelPoint);

        using SKFont tickFont = TickFont.GetFont();
        var ticks = TickGenerator.GetVisibleTicks(Range);
        AxisRendering.DrawTicks(surface, tickFont, panelRect, Label.Color, ticks, this);
        AxisRendering.DrawFrame(surface, panelRect, Edge, Label.Color);
    }

    public double GetPixelDistance(double distance, PixelRect dataArea)
    {
        return distance * dataArea.Width / Width;
    }

    public double GetCoordinateDistance(double distance, PixelRect dataArea)
    {
        return distance / (dataArea.Width / Width);
    }
}
