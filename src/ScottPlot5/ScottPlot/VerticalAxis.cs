namespace ScottPlot;

public class VerticalAxis : IAxis
{
    public double Min { get; set; }
    public double Max { get; set; }
    public double Bottom => Min;
    public double Top => Max;
    public double Height => Max - Min;

    public override string ToString()
    {
        return $"VerticalAxis: Bottom={Bottom}, Top={Top}";
    }

    public float GetPixel(double position, float min, float max)
    {
        float pxHeight = min - max;
        double pxPerUnit = pxHeight / Height;
        double unitsFromBottomEdge = position - Bottom;
        float pxFromEdge = (float)(unitsFromBottomEdge * pxPerUnit);
        return min - pxFromEdge;
    }

    public double GetCoordinate(float pixel, float min, float max)
    {
        float pxHeight = max - min;
        double pxPerUnit = pxHeight / Height;
        float pxFromEdge = pixel - min;
        double unitsFromEdge = pxFromEdge / pxPerUnit;
        return Min + unitsFromEdge;
    }
}