using ScottPlot.Style;
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
        Font = new Style.StyledSKFont()
        {
            Family = "Consolas",
            Size = 16,
            Bold = true
        },
        Rotation = -90
    };
    public Style.StyledSKFont TickFont { get; set; } = new();

    public abstract Edge Edge { get; }

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
        using SKPaint paint = new(Label.Font.GetFont());

        float labelWidth = Drawing.MeasureString(Label.Text, paint).Height;
        float largestTickWidth = MeasureTicks();

        return labelWidth + largestTickWidth + 15;
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

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        using SKFont tickFont = TickFont.GetFont();

        var ticks = TickGenerator.GetVisibleTicks(Range);
        float tickSize = MeasureTicks();

        AxisRendering.DrawLabel(surface, dataRect, Edge, Label, Measure());
        AxisRendering.DrawTicks(surface, tickFont, dataRect, Label.Color, ticks, this);
        AxisRendering.DrawFrame(surface, dataRect, Edge, Label.Color);
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
