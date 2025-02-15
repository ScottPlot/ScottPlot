namespace ScottPlot;

// TODO: Performance of many data sources may be improved and code duplication reduced.
// Improvements should be made in concert with performance tests.
// See https://github.com/ScottPlot/ScottPlot/pull/4753 for comments.

/// <summary>
/// Internal interface used for Utility Functions within <see cref="DataSourceUtilities"/>
/// </summary>
public interface IDataSource
{
    /// <summary>
    /// When set true, <see cref="DataSourceUtilities"/> should prefer paths that utilize <see cref="GetCoordinates"/>
    /// </summary>
    bool PreferCoordinates { get; }

    /// <summary> The length of the collection </summary>
    int Length { get; }
    int MinRenderIndex { get; }
    int MaxRenderIndex { get; }

    /// <summary> Gets the closest index to a specified coordinate for the array of X's </summary>
    /// <param name="mouseLocation">typically the X coordinate should be used, unless the plot is rotated (then use Y)</param>
    /// <remarks>Fastest execution should be using a BinarySearch, such as <see cref="DataSourceUtilities.GetClosestIndex(double[], double, IndexRange)"/></remarks>
    int GetXClosestIndex(Coordinates mouseLocation);

    /// <summary> Gets the X-Y coordinate from the data source at the specified index </summary>
    Coordinates GetCoordinate(int index);

    /// <summary> Gets the X-Y coordinate from the data source at the specified index with any offsets and scaling applied </summary>
    Coordinates GetCoordinateScaled(int index);

    /// <summary> Gets the X value from the data source at the specified index </summary>
    double GetX(int index);

    /// <summary> Gets the X value from the data source at the specified index with any offsets and scaling applied </summary>
    double GetXScaled(int index);

    /// <summary> Gets the Y value from the data source at the specified index </summary>
    double GetY(int index);

    /// <summary> Gets the Y value from the data source at the specified index with any offsets and scaling applied </summary>
    double GetYScaled(int index);

    /// <summary>
    /// When the collection is sorted, this will enable much quicker execution by allowing usage of BinarySearch methods ( GetNearest should call GetXClosestIndex when this is true )
    /// </summary>
    bool IsSorted();
}
