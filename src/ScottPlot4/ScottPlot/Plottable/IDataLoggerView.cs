namespace ScottPlot.Plottable;

/// <summary>
/// A data logger view contains logic for managing axis limits
/// based upon the data contained in a datalogger plot type.
/// </summary>
public interface IDataLoggerView
{
    void SetAxisLimits(Plot plt, AxisLimits dataLimits);
}
