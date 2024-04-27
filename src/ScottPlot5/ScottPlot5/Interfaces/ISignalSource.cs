namespace ScottPlot;

/// <summary>
/// This interface is used by plottables to access data while rendering.
/// This interface describes Y data sampled along an X axis at a fixed period.
/// </summary>
public interface ISignalSource
{
    /// <summary>
    /// X distance between Y points
    /// </summary>
    double Period { get; set; }

    /// <summary>
    /// X position of the first data point
    /// </summary>
    double XOffset { get; set; }

    /// <summary>
    /// Shift Y position of all values by this amount
    /// </summary>
    double YOffset { get; set; }

    /// <summary>
    /// Multiply Y values by this scale factor (before applying offset)
    /// </summary>
    double YScale { get; set; }

    /// <summary>
    /// Do not display data above this index
    /// </summary>
    public int MaximumIndex { get; set; }

    /// <summary>
    /// Do not display data below this index
    /// </summary>
    public int MinimumIndex { get; set; }

    /// <summary>
    /// Returns range information about the data at a specific pixel location
    /// </summary>
    PixelColumn GetPixelColumn(IAxes axes, int xPixelIndex);

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
    /// Returns the Y position for a given index.
    /// </summary>
    double GetY(int index);

    /// <summary>
    /// Return an object for working with all Y values.
    /// </summary>
    IReadOnlyList<double> GetYs(); // TODO: should this be IEnumerable?

    /// <summary>
    /// Y values between a range of indexes (inclusive).
    /// </summary>
    IEnumerable<double> GetYs(int index1, int index2);

    public CoordinateRange GetLimitsX();

    public CoordinateRange GetLimitsY();

    AxisLimits GetLimits();
}
