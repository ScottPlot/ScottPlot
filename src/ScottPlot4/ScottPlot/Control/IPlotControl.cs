namespace ScottPlot.Control;

/// <summary>
/// Interactive ScottPlot controls implement this interface.
/// </summary>
public interface IPlotControl
{
    /// <summary>
    /// The plot displayed by this control
    /// </summary>
    Plot Plot { get; }

    /// <summary>
    /// Redraw the plot and display it in the control
    /// </summary>
    void Refresh();

    /// <summary>
    /// Configuration object holding advanced control options
    /// </summary>
    Configuration Configuration { get; }
}
