namespace ScottPlot;

public interface IPlotControl
{
    /// <summary>
    /// The <see cref="Plot"/> displayed by this interactive control
    /// </summary>
    Plot Plot { get; }

    /// <summary>
    /// Render the plot and update the image
    /// </summary>
    void Refresh();

    /// <summary>
    /// Advanced options for configuring how user inputs manipulate the plot
    /// </summary>
    IPlotInteraction Interaction { get; set; }

    /// <summary>
    /// Platform-specific logic for managing the context menu
    /// </summary>
    IPlotMenu Menu { get; set; }

    /// <summary>
    /// Launch the default pop-up menu (typically in response to a right-click) at the given position in the control
    /// </summary>
    void ShowContextMenu(Pixel position);

    /// <summary>
    /// Context for hardware-accelerated graphics (or null if not available)
    /// </summary>
    GRContext? GRContext { get; }

    /// <summary>
    /// Determine the DPI scaling ratio of the present display.
    /// A value of 1.0 means no scaling, and 1.5 means 150% scaling.
    /// This operation may be costly so do not call it frequently.
    /// </summary>
    float DetectDisplayScale();

    /// <summary>
    /// The value of the present display scaling.
    /// Mouse positions are multiplied by this value for pixel/coordinate conversions.
    /// </summary>
    float DisplayScale { get; set; }

    /// <summary>
    /// Disposes the current Plot and creates a new one for the control
    /// </summary>
    void Reset();

    /// <summary>
    /// Loads the given Plot into the control
    /// </summary>
    void Reset(Plot plot);
}
