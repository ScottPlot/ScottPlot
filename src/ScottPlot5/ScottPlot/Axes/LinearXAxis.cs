namespace ScottPlot.Axes;

public class LinearXAxis : IXAxis
{
    public double Left { get; set; } = 0;
    public double Right { get; set; } = 0;
    public double Width => Right - Left;
    public bool HasBeenSet { get; set; } = false;

    public bool Contains(double position)
    {
        return position >= Left && position <= Right;
    }

    public override string ToString()
    {
        return $"HorizontalAxis: Left={Left}, Right={Right}";
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
