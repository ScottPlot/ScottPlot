namespace ScottPlot;

public interface ISignalXYSource
{
    // TODO: support XOffset and YOffset

    /// <summary>
    /// Return limits of visible data
    /// </summary>
    AxisLimits GetAxisLimits();

    /// <summary>
    /// Return the range on the horizontal axis containing all visible data
    /// </summary>
    CoordinateRangeStruct GetRangeX();

    /// <summary>
    /// Return the range on the vertical axis containing all visible data
    /// </summary>
    CoordinateRangeStruct GetRangeY();

    /// <summary>
    /// Return the range on the vertical axis containing visible data between the given indices (inclusive)
    /// </summary>
    public CoordinateRangeStruct GetRangeY(int index1, int index2);
}
