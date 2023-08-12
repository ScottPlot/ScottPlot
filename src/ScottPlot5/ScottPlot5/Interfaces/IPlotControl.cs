using ScottPlot.Control;

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
    /// This object contains methods which allow plots to be rendered in sequence.
    /// This avoids render artifacts caused by two plots rendering at the same time 
    /// or infinite loops caused by renderes being requested from in-render events.
    /// </summary>
    RenderQueue RenderQueue { get; }

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
    /// Context for hardware-accelerated graphics (or null if not available)
    /// </summary>
    GRContext? GRContext { get; }

    /// <summary>
    /// Logic for translating screen position (pixels) to coordinates in (axis units).
    /// Implementers must add logic to compensate for DPI scaling.
    /// </summary>
    Coordinates GetCoordinates(Pixel px, IXAxis? xAxis = null, IYAxis? yAxis = null);

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
}
