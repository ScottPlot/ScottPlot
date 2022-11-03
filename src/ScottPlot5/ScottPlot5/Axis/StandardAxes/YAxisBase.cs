using ScottPlot.Style;
using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public abstract class YAxisBase : IAxis
{
    public double Bottom
    {
        get => Range.Min;
        set => Range.Min = value;
    }

    public double Top
    {
        get => Range.Max;
        set => Range.Max = value;
    }

    public double Height => Range.Span;

    public virtual CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

    public float Offset { get; set; } = 0;

    public float PixelSize { get; private set; } = 50;

    public float PixelWidth => PixelSize;

    public ITickGenerator TickGenerator { get; set; } = null!;

    public Label Label { get; private set; } = new()
    {
        Text = "Vertical Axis",
        Font = new Style.Font()
        {
            Family = "Consolas",
            Size = 16,
            Bold = true
        },
        Rotation = -90
    };
    public Font TickFont { get; set; } = new();

    public abstract Edge Edge { get; }

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        double unitsFromMinValue = position - Bottom;
        float pxFromEdge = (float)(unitsFromMinValue * pxPerUnit);
        return dataArea.Bottom - pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        float pxFromMinValue = pixel - dataArea.Bottom;
        double unitsFromMinValue = pxFromMinValue / pxPerUnit;
        return Bottom - unitsFromMinValue;
    }

    public void Measure()
    {
        float labelWidth = Label.Font.Size + 15;
        float largestTickWidth = MeasureTicks();

        PixelSize = labelWidth + largestTickWidth;
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

        AxisRendering.DrawLabel(surface, dataRect, Edge, Label, Offset, PixelSize);
        AxisRendering.DrawTicks(surface, tickFont, dataRect, Label.Color, Offset, ticks, this);
        AxisRendering.DrawFrame(surface, dataRect, Edge, Label.Color, Offset);
    }

}
