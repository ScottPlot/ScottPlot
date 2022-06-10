namespace ScottPlot;

public class HorizontalAxis : IAxis
{
    public double Min { get; set; }
    public double Max { get; set; }
    public double Left => Min;
    public double Right => Max;
    public double Width => Max - Min;

    public override string ToString()
    {
        return $"HorizontalAxis: Left={Left}, Right={Right}";
    }

    public float GetPixel(double position, float min, float max)
    {
        float pxWidth = max - min;
        double pxPerUnit = pxWidth / Width;
        double unitsFromLeftEdge = position - Left;
        float pxFromEdge = (float)(unitsFromLeftEdge * pxPerUnit);
        return min + pxFromEdge;
    }

    public double GetCoordinate(float pixel, float min, float max)
    {
        float pxWidth = max - min;
        double pxPerUnit = pxWidth / Width;

        float pxFromLeftEdge = pixel - min;
        double unitsFromEdge = pxFromLeftEdge / pxPerUnit;
        return Min + unitsFromEdge;
    }
}
