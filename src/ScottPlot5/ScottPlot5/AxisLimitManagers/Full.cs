namespace ScottPlot.AxisLimitManagers;

/// <summary>
/// Show the entire range of data, changing the axis limits only
/// when the data extends outside the current view.
/// </summary>
public class Full : IAxisLimitManager
{
    /// <summary>
    /// Defines the amount of whitespace added to the right of the data when data runs outside the current view.
    /// 1.0 for a view that tightly fits the data and is always changing.
    /// 2.0 for a view that doubles the horizontal span when new data runs outside the current view.
    /// </summary>
    public double ExpansionRatio { get; set; } = .25;

    public CoordinateRange GetRangeX(CoordinateRange viewRangeX, CoordinateRange dataRangeX)
    {
        bool rightEdgeIsTooClose = viewRangeX.Max < dataRangeX.Max;
        bool rightEdgeIsTooFar = viewRangeX.Max > dataRangeX.Max + dataRangeX.Span;
        bool replaceRight = rightEdgeIsTooClose || rightEdgeIsTooFar;
        double xMax = replaceRight
            ? dataRangeX.Max + dataRangeX.Span * ExpansionRatio
            : viewRangeX.Max;

        return new(dataRangeX.Min, xMax);
    }

    public CoordinateRange GetRangeY(CoordinateRange viewRangeY, CoordinateRange dataRangeY)
    {
        bool topEdgeIsTooClose = viewRangeY.Max < dataRangeY.Max;
        bool topEdgeIsTooFar = viewRangeY.Max < dataRangeY.Max + dataRangeY.Span;
        bool replaceMax = topEdgeIsTooClose || topEdgeIsTooFar;
        double yMax = replaceMax
            ? dataRangeY.Max + dataRangeY.Span * ExpansionRatio
            : viewRangeY.Max;

        bool bottomEdgeIsTooClose = viewRangeY.Min > dataRangeY.Min;
        bool bottomEdgeIsTooFar = viewRangeY.Min > dataRangeY.Min - dataRangeY.Span;
        bool replaceMin = bottomEdgeIsTooClose || bottomEdgeIsTooFar;
        double yMin = replaceMin
            ? dataRangeY.Min - dataRangeY.Span * ExpansionRatio
            : viewRangeY.Min;

        return new CoordinateRange(yMin, yMax);
    }
}
