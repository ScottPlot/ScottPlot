namespace ScottPlot.Interactivity;

/// <summary>
/// Describes a class that has logic to watch user actions and manipulate the plot accordingly.
/// </summary>
public interface IUserActionResponse
{
    /// <summary>
    /// Perform the given action on the specified plot and return a result indicating what to do next.
    /// </summary>
    ResponseInfo Execute(Plot plot, IUserAction userActions, KeyboardState keys);
}
