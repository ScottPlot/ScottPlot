using ScottPlot.Plottable.DataLoggerViews;

namespace ScottPlot.Plottable;

/// <summary>
/// Data logging plot types have methods for adding/removing
/// data points and contain logic for managing axis limits
/// to optimally view incoming data.
/// </summary>
public interface IDataLogger
{
    /// <summary>
    /// If enabled, axis limits will be set automatically to fit the data before each render.
    /// Axis limits are set automatically according to <see cref="LoggerView"/>.
    /// </summary>
    public bool ManageAxisLimits { get; set; }

    /// <summary>
    /// Logic for automatically setting axis limits if <see cref="ManageAxisLimits"/> is enabled.
    /// </summary>
    public IDataLoggerView LoggerView { get; set; }

    /// <summary>
    /// A reference to the plot containing this plottable.
    /// This object is used to read and set axis limits.
    /// </summary>
    public Plot Plot { get; }
}
