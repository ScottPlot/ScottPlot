namespace ScottPlot.Interactivity;

public readonly record struct UserActionResult
{
    public string Summary { get; } = string.Empty;
    public bool ClearPreviousEvents { get; } = false;
    public bool StartTrackingMouse { get; } = false;

    public UserActionResult(string summary, bool clearPreviousEvents = false, bool startTrackingMouse = false)
    {
        Summary = summary;
        ClearPreviousEvents = clearPreviousEvents;
        StartTrackingMouse = startTrackingMouse;
    }

    public static UserActionResult NoAction = new(string.Empty);
};
