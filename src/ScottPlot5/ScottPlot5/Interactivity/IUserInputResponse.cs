namespace ScottPlot.Interactivity;

/// <summary>
/// Describes a class that has logic to process incoming user inputs
/// and manipulate the plot accordingly.
/// </summary>
public interface IUserInputResponse
{
    /// <summary>
    /// Perform the given action on the specified plot and return
    /// a result indicating what happened.
    /// </summary>
    UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys);

    /// <summary>
    /// Indicates whether this action can take over during drag events to become the primary drag processor
    /// </summary>
    //public bool RespondsToDrag { get; }
}
