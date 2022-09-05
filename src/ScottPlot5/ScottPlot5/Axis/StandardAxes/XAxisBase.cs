namespace ScottPlot.Axis.StandardAxes;

public abstract class XAxisBase
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
        Bold = true,
        FontSize = 16
    };

    public void Measure()
    {
        float labelHeight = Label.FontSize;
        float largestTickHeight = Label.FontSize;
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
}
