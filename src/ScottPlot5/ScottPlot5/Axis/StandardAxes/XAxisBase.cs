using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public abstract class XAxisBase : IAxis
{
    public double Left
    {
        get => Range.Min;
        set => Range.Min = value;
    }

    public double Right
    {
        get => Range.Max;
        set => Range.Max = value;
    }

    public double Width => Range.Span;

    public CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

    public ITickGenerator TickGenerator { get; set; } = null!;

    public float Offset { get; set; } = 0;

    public float PixelSize { get; private set; } = 50;

    public float PixelHeight => PixelSize;

    public Label Label { get; private set; } = new()
    {
        Text = "Horizontal Axis",
        Font = new Style.Font()
        {
            Family = "Consolas",
            Size = 16,
            Bold = true
        },
    };
    public abstract Edge Edge { get; }

    public void Measure()
    {
        float labelHeight = Label.Font.Size;
        float largestTickHeight = Label.Font.Size;
        PixelSize = labelHeight + largestTickHeight + 18;
    }

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Width / Width;
        double unitsFromLeftEdge = position - Left;
        float pxFromEdge = (float)(unitsFromLeftEdge * pxPerUnit);
        return dataArea.Left + pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Width / Width;
        float pxFromLeftEdge = pixel - dataArea.Left;
        double unitsFromEdge = pxFromLeftEdge / pxPerUnit;
        return Left + unitsFromEdge;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        using SKFont font = Label.Font.GetFont();
        var ticks = TickGenerator.GetVisibleTicks(Range);

        AxisRendering.DrawLabel(surface, dataRect, Edge, Label, Offset, PixelSize);
        AxisRendering.DrawTicks(surface, font, dataRect, Label.Color, Offset, ticks, this);
        AxisRendering.DrawFrame(surface, dataRect, Edge, Label.Color, Offset);
    }
}
