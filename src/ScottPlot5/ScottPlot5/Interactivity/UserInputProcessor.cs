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
    public readonly KeyState KeyState;

    /// <summary>
    /// Controls whether new events are processed
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// A list of user input responses that processes all incoming events in order.
    /// Users may manipulate this list to change the default behavior and
    /// add custom behaviors.
    /// </summary>
    public readonly List<IUserInputResponse> UserInputResponses = [];

    public UserInputProcessor(Plot plot)
    {
        Plot = plot;
        KeyState = new();
        Reset();
    }

    /// <summary>
    /// Remove all user input responses of the specified type
    /// </summary>
    public void RemoveAll<T>() where T : IUserInputResponse
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

    public static List<IUserInputResponse> DefaultUserResponses() =>
    [
        // drag events
        new UserInputResponses.MiddleClickDragZoomRectangle(),
        new UserInputResponses.LeftClickDragPan(),
        new UserInputResponses.RightClickDragZoom(),

        // click events
        new UserInputResponses.ScrollWheelZoom(),
        new UserInputResponses.MiddleClickAutoscale(),
    ];

    /// <summary>
    /// When defined, this response is the only one that gets processed
    /// until it returns a result indicating it is no longer the primary response.
    /// </summary>
    IUserInputResponse? PrimaryResponse = null;

    /// <summary>
    /// Process a user input and return results of the responses that engaged with it
    /// </summary>
    public IReadOnlyList<UserInputResponseResult> Process(IUserInput userInput)
    {
        if (!IsEnabled)
            return [];

        UpdateKeyboardState(userInput);

        var responseResults = ExecuteUserInputResponses(userInput);

        if (responseResults.Where(x => x.RefreshRequired).Any())
        {
            Plot.PlotControl?.Refresh();
        }

        return responseResults;
    }

    private void UpdateKeyboardState(IUserInput userInput)
    {
        if (userInput is UserInputs.KeyDown keyDown)
        {
            KeyState.Add(keyDown.Key);
        }

        if (userInput is UserInputs.KeyUp keyUp)
        {
            KeyState.Remove(keyUp.Key);
        }
    }

    private IReadOnlyList<UserInputResponseResult> ExecuteUserInputResponses(IUserInput userInput)
    {
        List<UserInputResponseResult> results = [];

        foreach (IUserInputResponse response in UserInputResponses)
        {
            if (PrimaryResponse is not null && PrimaryResponse != response)
            {
                continue;
            }

            UserInputResponseResult result = response.Execute(Plot, userInput, KeyState);
            results.Add(result);

            if (PrimaryResponse is null && result.IsPrimaryResponse)
            {
                PrimaryResponse = response;
            }

            if (PrimaryResponse == response && !result.IsPrimaryResponse)
            {
                PrimaryResponse = null;
            }
        }

        return results;
    }
}
