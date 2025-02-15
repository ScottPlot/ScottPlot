namespace ScottPlot.AxisLimitManagers;

public class FixedWidth : IAxisLimitManager
{
    /// <summary>
    /// Fractional amount to expand the axis vertically if data runs outside the current view
    /// </summary>
    public double ExpandFractionY = 0.5;

    public CoordinateRange GetRangeX(CoordinateRange viewRangeX, CoordinateRange dataRangeX) => dataRangeX;

    public CoordinateRange GetRangeY(CoordinateRange viewRangeY, CoordinateRange dataRangeY)
    {
        double expandY = ExpandFractionY * dataRangeY.Span;

        double yMin = (dataRangeY.Min < viewRangeY.Min) ? dataRangeY.Min - expandY : viewRangeY.Min;
        double yMax = (dataRangeY.Max > viewRangeY.Max) ? dataRangeY.Max + expandY : viewRangeY.Max;

        return new CoordinateRange(yMin, yMax);
    }
}
