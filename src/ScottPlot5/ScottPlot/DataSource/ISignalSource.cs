namespace ScottPlot.DataSource;

/// <summary>
/// A signal source is a type of data source with user-defined Ys but evenly-spaced Xs
/// </summary>
public interface ISignalSource : IDataSource
{
    double Period { get; set; }
    double XOffset { get; set; }

    CoordinateRange GetMinMax(CoordinateRange xRange);

    int GetIndex(double x);

    double GetX(int index);
}
