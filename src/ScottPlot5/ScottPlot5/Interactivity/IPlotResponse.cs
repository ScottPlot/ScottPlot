namespace ScottPlot.Interactivity;

/// <summary>
/// Describes a class that has logic to process incoming user inputs
/// and manipulate the plot accordingly.
/// </summary>
public interface IPlotResponse
{
    /// <summary>
    /// Perform the given action on the specified plot and return
    /// a result indicating what happened.
    /// </summary>
    PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyState keys);
}
