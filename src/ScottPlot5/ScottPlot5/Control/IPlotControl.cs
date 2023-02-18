namespace ScottPlot.Control;

public interface IPlotControl
{
    /// <summary>
    /// The <see cref="Plot"/> displayed by this interactive control
    /// </summary>
    Plot Plot { get; }

    /// <summary>
    /// Request a re-render of the <see cref="Plot"/>
    /// </summary>
    void Refresh();

    /// <summary>
    /// Advanced options for configuring how user inputs manipulate the plot
    /// </summary>
    Interaction Interaction { get; }

    /// <summary>
    /// Replace the interaction back-end with a custom one
    /// </summary>
    void Replace(Interaction interaction);

    /// <summary>
    /// Launch the default pop-up menu (typically in response to a right-click) at the given position in the control
    /// </summary>
    void ShowContextMenu(Pixel position);

    /// <summary>
    /// Context for hardware-accelerated graphics (if available)
    /// </summary>
    GRContext? GRContext { get; }
}
