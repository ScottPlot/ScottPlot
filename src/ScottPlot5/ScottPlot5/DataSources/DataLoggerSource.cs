namespace ScottPlot.DataSources;

public class DataLoggerSource
{
    // TODO: support unevenly spaced X positions

    // TODO: render using SignalXY binary search strategy

    public readonly List<Coordinates> Coordinates = [];
    public double Period = 1;

    double YMin = double.NaN;
    double YMax = double.NaN;
    double XMin = double.NaN;
    double XMax = double.NaN;

    public int CountOnLastRender { get; internal set; } = -1;
    public int CountTotal => Coordinates.Count;

    public void Add(double y)
    {
        double x = Coordinates.Count * Period;
        Add(x, y);
    }

    public void Add(double x, double y)
    {
        Add(new Coordinates(x, y));
    }

    public void Add(Coordinates coordinates)
    {
        Coordinates.Add(coordinates);

        double y = coordinates.Y;
        double x = coordinates.X;

        if (!double.IsNaN(y))
        {
            YMin = double.IsNaN(YMin) ? y : Math.Min(YMin, y);
            YMax = double.IsNaN(YMax) ? y : Math.Max(YMax, y);
        }

        if (!double.IsNaN(x))
        {
            XMin = double.IsNaN(XMin) ? x : Math.Min(XMin, x);
            XMax = double.IsNaN(XMax) ? x : Math.Max(XMax, x);
        }
    }

    public void Clear()
    {
        Coordinates.Clear();

        YMin = double.NaN;
        YMax = double.NaN;
        XMin = double.NaN;
        XMax = double.NaN;
    }

    public AxisLimits GetAxisLimits()
    {
        if (!Coordinates.Any())
            return AxisLimits.NoLimits;

        return new AxisLimits(XMin, XMax, YMin, YMax);
    }
}
