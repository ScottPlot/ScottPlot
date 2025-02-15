namespace ScottPlot.Interactivity;

/// <summary>
/// Describes a class that has logic to watch user actions and manipulate the plot accordingly.
/// </summary>
public interface IUserActionResponse
{
    /// <summary>
    /// Perform the given action on the specified plot and return a result indicating what to do next.
    /// </summary>
    ResponseInfo Execute(IPlotControl plotControl, IUserAction userActions, KeyboardState keys);

    /// <summary>
    /// Reset state to what it was when the action response was first created.
    /// This method is called when the user processor resets (e.g., when the 
    /// control loses and re-gains focus or is disabled and re-enabled) and
    /// is designed to reset responses like mouse-drag events that accumulate state.
    /// </summary>
    void ResetState(IPlotControl plotControl);
}
