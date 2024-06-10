namespace ScottPlot.AxisLimitManagers;

public class FixedWidth : IAxisLimitManager
{
    /// <summary>
    /// Fractional amount to expand the axis vertically if data runs outside the current view
    /// </summary>
    public double ExpandFractionY = 0.5;

    public AxisLimits GetAxisLimits(AxisLimits viewLimits, AxisLimits dataLimits)
    {
        double xMin = dataLimits.Left;
        double xMax = dataLimits.Right;

        double expandY = ExpandFractionY * dataLimits.VerticalSpan;

        double yMin = (dataLimits.Bottom < viewLimits.Bottom) ? dataLimits.Bottom - expandY : viewLimits.Bottom;
        double yMax = (dataLimits.Top > viewLimits.Top) ? dataLimits.Top + expandY : viewLimits.Top;

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }
}
