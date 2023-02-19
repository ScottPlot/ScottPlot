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

    /// <summary>
    /// Link a plot control so its axis and layout updates with this one
    /// </summary>
    void AddLinkedControl(IPlotControl plotControl, bool horizontal = true, bool vertical = true, bool layout = true);

    /// <summary>
    /// Clear all linked plot controls
    /// </summary>
    void ClearLinkedControls();
}
