namespace ScottPlot.Interactivity;

/// <summary>
/// This class collects user inputs and performs responses to manipulate a Plot.
/// Custom user input actions may be supplied, and the list of responses can be 
/// modified to achieve total control over interaction behavior.
/// </summary>
public class UserInputProcessor
{
    /// <summary>
    /// The plot this input processor will act on
    /// </summary>
    public IPlotControl PlotControl { get; }

    /// <summary>
    /// Tracks which keys are currently pressed
    /// </summary>
    public readonly KeyboardState KeyState = new();

    /// <summary>
    /// Controls whether new events are processed.
    /// Enabling this disables the older <see cref="IPlotControl.Interaction"/> system.
    /// </summary>
    public bool IsEnabled
    {
        get => _IsEnabled; set
        {
            _IsEnabled = value;
            if (!value)
            {
                ProcessLostFocus();
            }
        }
    }

    private bool _IsEnabled = false;

    /// <summary>
    /// Enable processing of user input events.
    /// </summary>
    public void Enable() => IsEnabled = true;

    /// <summary>
    /// Disable processing of user input events. Effectively makes this control non-interactive.
    /// </summary>
    public void Disable() => IsEnabled = false;

    /// <summary>
    /// A list of user input responses that processes all incoming events in order.
    /// Users may manipulate this list to change the default behavior and
    /// add custom behaviors.
    /// </summary>
    public readonly List<IUserActionResponse> UserActionResponses = [];

    public UserInputProcessor(IPlotControl plotControl)
    {
        PlotControl = plotControl;
        Reset();
        IsEnabled = true;
    }

    /// <summary>
    /// Remove all user input responses of the specified type
    /// </summary>
    public void RemoveAll<T>() where T : IUserActionResponse
    {
        UserActionResponses.RemoveAll(x => x is T);
    }

    /// <summary>
    /// Resets the user input responses to use the
    /// default interactivity settings
    /// </summary>
    public void Reset()
    {
        KeyState.Reset();
        UserActionResponses.Clear();
        UserActionResponses.AddRange(DefaultUserResponses());
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
    /// Helper method to enable or disable the default double-left-click benchmark behavior
    /// </summary>
    public void DoubleLeftClickBenchmark(bool enable)
    {
        RemoveAll<UserActionResponses.DoubleClickBenchmark>();

        if (enable)
        {
            var button = StandardMouseButtons.Left;
            var response = new UserActionResponses.DoubleClickBenchmark(button);
            UserActionResponses.Add(response);
        }
    }

    /// <summary>
    /// Helper method to enable or disable the default left-click-drag pan behavior
    /// </summary>
    public void LeftClickDragPan(bool enable, bool horizontal = true, bool vertical = true)
    {
        RemoveAll<UserActionResponses.MouseDragPan>();

        if (enable)
        {
            var button = StandardMouseButtons.Left;
            var response = new UserActionResponses.MouseDragPan(button)
            {
                LockX = !horizontal,
                LockY = !vertical,
            };
            UserActionResponses.Add(response);
        }
    }

    /// <summary>
    /// Helper method to enable or disable the default left-click-drag pan behavior
    /// </summary>
    public void RightClickDragZoom(bool enable, bool horizontal = true, bool vertical = true)
    {
        RemoveAll<UserActionResponses.MouseDragZoom>();

        if (enable)
        {
            var button = StandardMouseButtons.Right;
            var response = new UserActionResponses.MouseDragZoom(button)
            {
                LockX = !horizontal,
                LockY = !vertical,
            };
            UserActionResponses.Add(response);
        }
    }

    /// <summary>
    /// When defined, this response is the only one that gets processed
    /// until it returns a result indicating it is no longer the primary response.
    /// </summary>
    IUserActionResponse? PrimaryResponse = null;

    /// <summary>
    /// Process a user input and return results of the responses that engaged with it
    /// </summary>
    public void Process(IUserAction userAction)
    {
        if (!IsEnabled)
            return;

        UpdateKeyboardState(userAction);

        bool refreshNeeded = ExecuteUserInputResponses(userAction);

        if (refreshNeeded)
            PlotControl.Refresh();
    }

    public Action<IPlotControl> LostFocusAction = (IPlotControl plotControl) =>
    {
        ResetState(plotControl);
        plotControl.Refresh();
    };

    /// <summary>
    /// Reset state of all user action responses to do things like 
    /// abort mouse-down-drag actions or key press-and-hold actions
    /// </summary>
    public static void ResetState(IPlotControl plotControl)
    {
        foreach (var response in plotControl.UserInputProcessor.UserActionResponses)
        {
            response.ResetState(plotControl);
        }

        plotControl.UserInputProcessor.KeyState.Reset();
    }

    public void ProcessLostFocus()
    {
        LostFocusAction.Invoke(PlotControl);
    }

    private void UpdateKeyboardState(IUserAction userAction)
    {
        if (userAction is UserActions.KeyDown keyDown)
        {
            KeyState.Add(keyDown.Key);
        }

        if (userAction is UserActions.KeyUp keyUp)
        {
            KeyState.Remove(keyUp.Key);
        }
    }

    private bool ExecuteUserInputResponses(IUserAction userAction)
    {
        bool refreshNeeded = false;

        // lock onto the sync object to prevent actions from being applied while a render is in progress
        lock (PlotControl.Plot.Sync)
        {
            foreach (IUserActionResponse response in UserActionResponses)
            {
                if (PrimaryResponse is not null && PrimaryResponse != response)
                {
                    continue;
                }

                ResponseInfo info = response.Execute(PlotControl, userAction, KeyState);
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
