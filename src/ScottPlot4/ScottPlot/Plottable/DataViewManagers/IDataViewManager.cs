namespace ScottPlot.Plottable.DataViewManagers;

public interface IDataViewManager
{
    AxisLimits GetAxisLimits(AxisLimits viewLimits, AxisLimits dataLimits);
}
