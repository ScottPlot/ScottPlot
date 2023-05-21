namespace ScottPlot.Plottable.DataLoggerViews;

/// <summary>
/// A data logger view contains logic for managing axis limits based upon the data contained in a datalogger plot type.
/// These data views are designed for data consisting of XY pairs.
/// </summary>
public interface IScatterDataLoggerView
{
    void SetAxisLimits(Plot plt, AxisLimits dataLimits);
}
