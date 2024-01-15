namespace ScottPlot.DataSources;

public class DataLoggerSource
{
    // TODO: support unevenly spaced X positions

    // TODO: render using SignalXY binary search strategy

    public readonly List<Coordinates> Coordinates = [];
    public double Period = 1;

    double YMin = double.PositiveInfinity;
    double YMax = double.NegativeInfinity;

    public int CountOnLastRender = -1;
    public int CountTotal => Coordinates.Count;

    public DataLoggerSource()
    {
    }

    public void Add(double y)
    {
        double x = Coordinates.Count * Period;
        Coordinates.Add(new Coordinates(x, y));
        YMin = Math.Min(YMin, y);
        YMax = Math.Max(YMax, y);
    }

    public void Clear()
    {
        Coordinates.Clear();
        YMin = double.PositiveInfinity;
        YMax = double.NegativeInfinity;
    }

    public AxisLimits GetAxisLimits()
    {
        return Coordinates.Any()
            ? new AxisLimits(Coordinates.First().X, Coordinates.Last().X, YMin, YMax)
            : AxisLimits.NoLimits;
    }
}
