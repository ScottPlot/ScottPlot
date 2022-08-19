namespace ScottPlot.Axis.StandardAxes;

public abstract class YAxis
{
    public double Bottom { get; set; } = 0;
    public double Top { get; set; } = 0;
    public double Height => Top - Bottom;
    public bool HasBeenSet { get; set; } = false;

    public CoordinateRange Range => new(Bottom, Top);

    public bool Contains(double position)
    {
        return position >= Bottom && position <= Top;
    }

    public override string ToString()
    {
        return $"VerticalAxis: Bottom={Bottom}, Top={Top}";
    }

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
