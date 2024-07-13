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
    public readonly List<IPlotResponse> UserInputResponses = [];

    public UserInputProcessor(Plot plot)
    {
        Plot = plot;
        KeyState = new();
        Reset();
    }

    /// <summary>
    /// Remove all user input responses of the specified type
    /// </summary>
    public void RemoveAll<T>() where T : IPlotResponse
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

    public static List<IPlotResponse> DefaultUserResponses() =>
    [
        // drag events
        new PlotResponses.MiddleClickDragZoomRectangle(),
        new PlotResponses.LeftClickDragPan(),
        new PlotResponses.RightClickDragZoom(),

        // click events
        new PlotResponses.ScrollWheelZoom(),
        new PlotResponses.MiddleClickAutoscale(),
        new PlotResponses.RightClickContextMenu(),
        new PlotResponses.DoubleClickBenchmark(),

        // keypress events
        new PlotResponses.KeyboardPanAndZoom(),
    ];

    /// <summary>
    /// When defined, this response is the only one that gets processed
    /// until it returns a result indicating it is no longer the primary response.
    /// </summary>
    IPlotResponse? PrimaryResponse = null;

    /// <summary>
    /// Process a user input and return results of the responses that engaged with it
    /// </summary>
    public IReadOnlyList<PlotResponseResult> Process(IUserAction userInput)
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

    private IReadOnlyList<PlotResponseResult> ExecuteUserInputResponses(IUserAction userInput)
    {
        List<PlotResponseResult> results = [];

        foreach (IPlotResponse response in UserInputResponses)
        {
            if (PrimaryResponse is not null && PrimaryResponse != response)
            {
                continue;
            }

            PlotResponseResult result = response.Execute(Plot, userInput, KeyState);
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
