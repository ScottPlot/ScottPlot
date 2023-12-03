namespace ScottPlot;

/// <summary>
/// Represents a specific point in a DataSource
/// </summary>
public readonly struct DataPoint
{
    public readonly double X;
    public readonly double Y;
    public readonly int Index;
    public Coordinates Coordinates => new(X, Y);
    public bool IsReal => NumericConversion.IsReal(X) && NumericConversion.IsReal(Y);

    public DataPoint(double x, double y, int index)
    {
        X = x;
        Y = y;
        Index = index;
    }

    public DataPoint(Coordinates coordinates, int index)
    {
        X = coordinates.X;
        Y = coordinates.Y;
        Index = index;
    }

    public static DataPoint None => new(double.NaN, double.NaN, -1);
}
