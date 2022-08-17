namespace ScottPlot.DataSource;

/// <summary>
/// A signal source is a type of data source with user-defined Ys but evenly-spaced Xs
/// </summary>
public interface ISignalSource : IReadOnlyList<double>, IHasAxisLimits
{
    /// <summary>
    /// X distance between Y points
    /// </summary>
    double Period { get; }

    /// <summary>
    /// X position of the first data point
    /// </summary>
    double XOffset { get; }

    /// <summary>
    /// Returns the min/max Y values between a range of Xs (inclusive)
    /// </summary>
    CoordinateRange GetYRange(CoordinateRange xRange);

    /// <summary>
    /// Returns the predicted index for the data point nearest a given X position.
    /// If clamped, the returned index will be clamped between 0 and Length - 1.
    /// </summary>
    int GetIndex(double x, bool clamp);

    /// <summary>
    /// Returns the X position for a given index.
    /// </summary>
    double GetX(int index);

    /// <summary>
    /// Return an object for working with all Y values
    /// </summary>
    IReadOnlyList<double> GetYs();
}
