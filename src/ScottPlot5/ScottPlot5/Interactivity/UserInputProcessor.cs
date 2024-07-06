namespace ScottPlot.Interactivity;

/// <summary>
/// This class collects user inputs and performs actions to manipulate a Plot.
/// Custom user inputs may be supplied, and the list of responsive actions can be 
/// modified to achieve extreme control over interaction behavior.
/// </summary>
public class UserInputProcessor
{
    public readonly Plot Plot;
    public readonly UserInputQueue Queue;
    public readonly List<IUserInputAction> InputActions;
    private bool TrackMouseMovement = false;

    public UserInputProcessor(Plot plot)
    {
        Plot = plot;

        Queue = new();

        InputActions = [
            new UserInputActions.LeftClickDragPan(),
            new UserInputActions.RightClickDragZoom(),
        ];
    }

    public UserActionResult[] Add(IUserInput inputEvent, bool processActions = true)
    {
        // TODO: make actions classes that get all new actions passed through them
        // and let them store their own state so we don't have to have this flag
        // at such a high level of abstraction
        if (inputEvent is DefaultInputs.MouseMove && !TrackMouseMovement)
            return [];

        Queue.Add(inputEvent);

        if (!processActions)
            return [];

        List<UserActionResult> notableResults = [];

        foreach (IUserInputAction action in InputActions)
        {
            UserActionResult result = action.Execute(Plot, Queue);

            if (result == UserActionResult.NoAction)
                continue;

            notableResults.Add(result);

            if (result.StartTrackingMouse)
            {
                TrackMouseMovement = true;
            }

            if (result.ClearPreviousEvents)
            {
                Queue.Clear();
                TrackMouseMovement = false;
            }
        }

        return notableResults.ToArray();
    }
}
