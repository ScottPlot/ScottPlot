namespace ScottPlot;

public class AxisLimits2D
{
    public readonly double XMin;
    public readonly double XMax;
    public readonly double YMin;
    public readonly double YMax;

    public double XSpan => XMax - XMin;
    public double YSpan => YMax - YMin;
    public double Area => XSpan * YSpan;

    public AxisLimits2D(double xMin, double xMax, double yMin, double yMax)
    {
        XMin = xMin; 
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }
}
