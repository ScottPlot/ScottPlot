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
