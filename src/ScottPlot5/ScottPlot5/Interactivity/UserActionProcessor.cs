namespace ScottPlot.Interactivity;

/// <summary>
/// This class collects user inputs and performs responses to manipulate a Plot.
/// Custom user inputs may be supplied, and the list of responses can be 
/// modified to achieve extreme control over interaction behavior.
/// </summary>
public class UserInputProcessor
{
    private readonly Plot Plot;

    /// <summary>
    /// Tracks which keys are currently pressed
    /// </summary>
    public readonly KeyboardState KeyState;

    /// <summary>
    /// Controls whether new events are processed
    /// </summary>
    public bool IsEnabled { get; set; } = false;

    /// <summary>
    /// A list of user input responses that processes all incoming events in order.
    /// Users may manipulate this list to change the default behavior and
    /// add custom behaviors.
    /// </summary>
    public readonly List<IUserActionResponse> UserInputResponses = [];

    public UserInputProcessor(Plot plot)
    {
        Plot = plot;
        KeyState = new();
        Reset();
    }

    /// <summary>
    /// Remove all user input responses of the specified type
    /// </summary>
    public void RemoveAll<T>() where T : IUserActionResponse
    {
        UserInputResponses.RemoveAll(x => x is T);
    }

    /// <summary>
    /// Resets the user input responses to use the
    /// default interactivity settings
    /// </summary>
    public void Reset()
    {
        UserInputResponses.Clear();
        UserInputResponses.AddRange(DefaultUserResponses());
    }

    /// <summary>
    /// Default user actions that are in place when the event processor is constructed or reset.
    /// </summary>
    public static List<IUserActionResponse> DefaultUserResponses() =>
    [
        // drag events
        new UserActionResponses.MouseDragZoomRectangle(StandardMouseButtons.Middle),
        new UserActionResponses.MouseDragPan(StandardMouseButtons.Left),
        new UserActionResponses.MouseDragZoom(StandardMouseButtons.Right),

        // click events
        new UserActionResponses.MouseWheelZoom(StandardKeys.Shift, StandardKeys.Control),
        new UserActionResponses.SingleClickAutoscale(StandardMouseButtons.Middle),
        new UserActionResponses.SingleClickContextMenu(StandardMouseButtons.Right),
        new UserActionResponses.DoubleClickBenchmark(StandardMouseButtons.Left),

        // keypress events
        new UserActionResponses.KeyboardPanAndZoom(),
        new UserActionResponses.KeyboardAutoscale(StandardKeys.A),
    ];

    /// <summary>
    /// When defined, this response is the only one that gets processed
    /// until it returns a result indicating it is no longer the primary response.
    /// </summary>
    IUserActionResponse? PrimaryResponse = null;

    /// <summary>
    /// Process a user input and return results of the responses that engaged with it
    /// </summary>
    public void Process(IUserAction userInput)
    {
        if (!IsEnabled)
            return;

        UpdateKeyboardState(userInput);

        bool refreshNeeded = ExecuteUserInputResponses(userInput);

        if (refreshNeeded)
            Plot.PlotControl?.Refresh();
    }

    private void UpdateKeyboardState(IUserAction userInput)
    {
        if (userInput is UserActions.KeyDown keyDown)
        {
            KeyState.Add(keyDown.Key);
        }

        if (userInput is UserActions.KeyUp keyUp)
        {
            KeyState.Remove(keyUp.Key);
        }
    }

    private bool ExecuteUserInputResponses(IUserAction userInput)
    {
        bool refreshNeeded = false;

        // lock onto the sync object to prevent actions from being applied while a render is in progress
        lock (Plot.Sync)
        {
            foreach (IUserActionResponse response in UserInputResponses)
            {
                if (PrimaryResponse is not null && PrimaryResponse != response)
                {
                    continue;
                }

                ResponseInfo info = response.Execute(Plot, userInput, KeyState);
                if (info.RefreshNeeded)
                    refreshNeeded = true;

                if (PrimaryResponse is null && info.IsPrimary)
                {
                    PrimaryResponse = response;
                }

                if (PrimaryResponse == response && !info.IsPrimary)
                {
                    PrimaryResponse = null;
                }
            }
        }

        return refreshNeeded;
    }
}
