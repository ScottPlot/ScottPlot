namespace ScottPlot.Plottable.AxisManagers;

public class FixedWidth : IAxisManager
{
    /// <summary>
    /// Fractional amount to expand the axis vertically if data runs outside the current view
    /// </summary>
    public double ExpandFractionY = 0.5;

    public AxisLimits GetAxisLimits(AxisLimits viewLimits, AxisLimits dataLimits)
    {
        double xMin = dataLimits.XMin;
        double xMax = dataLimits.XMax;

        double expandY = ExpandFractionY * dataLimits.YSpan;

        double yMin = (dataLimits.YMin < viewLimits.YMin) ? dataLimits.YMin - expandY : viewLimits.YMin;
        double yMax = (dataLimits.YMax > viewLimits.YMax) ? dataLimits.YMax + expandY : viewLimits.YMax;

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }
}
