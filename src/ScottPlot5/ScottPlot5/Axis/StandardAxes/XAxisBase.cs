using ScottPlot.Style;
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
        Font = new Style.StyledSKFont()
        {
            Family = "Consolas",
            Size = 16,
            Bold = true
        },
    };
    public Style.StyledSKFont TickFont { get; set; } = new();
    public abstract Edge Edge { get; }

    public float Measure()
    {
        using SKPaint paint = new(Label.Font.GetFont());

        float labelHeight = Drawing.MeasureString(Label.Text, paint).Height;
        float largestTickHeight = MeasureTicks();

        return labelHeight + largestTickHeight + 15;
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
        return distance * dataArea.Width / Width;
    }

    public double GetCoordinateDistance(double distance, PixelRect dataArea)
    {
        return distance / (dataArea.Width / Width);
    }
}
