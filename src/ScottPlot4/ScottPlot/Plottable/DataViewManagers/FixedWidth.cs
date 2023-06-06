namespace ScottPlot.Plottable.DataViewManagers;

public class FixedWidth : IDataViewManager
{
    public AxisLimits GetAxisLimits(AxisLimits limits, AxisLimits dataLimits)
    {
        // TODO: auto-expand X too?
        double xMin = limits.XMin;
        double xMax = limits.XMax;

        double yMin = (dataLimits.YMin < limits.YMin) ? dataLimits.YMin : limits.YMin;
        double yMax = (dataLimits.YMax > limits.YMax) ? dataLimits.YMax : limits.YMax;

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }
}
