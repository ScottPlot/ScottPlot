namespace ScottPlot.DataSources;

public class DataLoggerSource
{
    // TODO: support unevenly spaced X positions

    // TODO: render using SignalXY binary search strategy

    public readonly List<Coordinates> Coordinates = [];
    public double Period = 1;

    double YMin = double.NaN;
    double YMax = double.NaN;

    public int CountOnLastRender = -1;
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
        if (Coordinates.Any())
        {
            if (coordinates.X < Coordinates.Last().X)
            {
                throw new ArgumentException("new X values cannot be smaller than existing ones");
            }
        }

        Coordinates.Add(coordinates);

        double y = coordinates.Y;

        if (!double.IsNaN(y))
        {
            YMin = double.IsNaN(YMin) ? y : Math.Min(YMin, y);
            YMax = double.IsNaN(YMax) ? y : Math.Max(YMax, y);
        }
    }

    public void Clear()
    {
        Coordinates.Clear();

        YMin = double.NaN;
        YMax = double.NaN;
    }

    public AxisLimits GetAxisLimits()
    {
        return Coordinates.Any()
            ? new AxisLimits(Coordinates.First().X, Coordinates.Last().X, YMin, YMax)
            : AxisLimits.NoLimits;
    }
}
