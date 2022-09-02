namespace ScottPlot.Axis.StandardAxes;

public abstract class YAxisBase
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

    public CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

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
}
