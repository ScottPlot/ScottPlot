namespace ScottPlot;

public class MutableCoordinateLine
{
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }

    public Coordinates Point1
    {
        get => new(X1, Y1);
        set
        {
            X1 = value.X;
            Y1 = value.Y;
        }
    }

    public Coordinates Point2
    {
        get => new(X2, Y2);
        set
        {
            X2 = value.X;
            Y2 = value.Y;
        }
    }

    public CoordinateLine CoordinateLine => new(X1, Y1, X2, Y2);

    public double XMin => Math.Min(X1, X2);
    public double XMax => Math.Max(X1, X2);
    public double YMin => Math.Min(Y1, Y2);
    public double YMax => Math.Max(Y1, Y2);
    public AxisLimits AxisLimits => new(XMin, XMax, YMin, YMax);

    public void Update(CoordinateLine line)
    {
        X1 = line.X1;
        Y1 = line.Y1;
        X2 = line.X2;
        Y2 = line.Y2;
    }
}
