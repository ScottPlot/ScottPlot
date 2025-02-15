namespace ScottPlot.Interactivity;

/// <summary>
/// Describes something the uer did to interact with the plot
/// </summary>
/// <param name="device">What the user engaged with</param>
/// <param name="description">What the user did to the device</param>
public interface IUserAction
{
    /// <summary>
    /// Name of the thing performing the action but no state.
    /// E.g., "left button" or "shift key"
    /// </summary>
    string Device { get; }

    /// <summary>
    /// Description of both the input device and its state.
    /// E.g., "left button released" or "shift key pressed"
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Describes when the event happened. 
    /// Useful for distinguishing single from double-clicks.
    /// </summary>
    public DateTime DateTime { get; set; }
}
