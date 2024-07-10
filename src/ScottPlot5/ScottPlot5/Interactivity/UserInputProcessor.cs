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
        new UserInputResponses.LeftClickDragPan(),
        new UserInputResponses.RightClickDragZoom(),
        new UserInputResponses.ScrollWheelZoom(),
        new UserInputResponses.MiddleClickAutoscale(),
        new UserInputResponses.MiddleClickDragZoomRectangle(),
    ];

    /// <summary>
    /// If defined, this is the only action which will process new events.
    /// </summary>
    private IUserInputResponse? PrimaryResponse = null;

    /// <summary>
    /// Process a user input and return results of the responses that engaged with it
    /// </summary>
    public UserInputResponseResult[] Process(IUserInput userInput)
    {
        if (!IsEnabled)
        {
            PrimaryResponse = null;
            return [];
        }

        if (userInput is UserInputs.KeyDown keyDown)
        {
            KeyState.Add(keyDown.Key);
        }

        if (userInput is UserInputs.KeyUp keyUp)
        {
            KeyState.Remove(keyUp.Key);
        }

        List<UserInputResponseResult> results = [];

        List<IUserInputResponse> responsesToProcess = PrimaryResponse is null
            ? UserInputResponses
            : [PrimaryResponse];

        foreach (IUserInputResponse response in responsesToProcess)
        {
            UserInputResponseResult result = response.Execute(Plot, userInput, KeyState);
            results.Add(result);
            PrimaryResponse = result.IsPrimaryDragResponse ? response : null;
            if (PrimaryResponse is not null)
                break;
        }

        if (results.Where(x => x.RefreshRequired).Any())
            Plot.PlotControl?.Refresh();

        return results.Where(x => !string.IsNullOrEmpty(x.Summary)).ToArray();
    }
}
