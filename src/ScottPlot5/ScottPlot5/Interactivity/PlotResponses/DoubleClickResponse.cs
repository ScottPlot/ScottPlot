namespace ScottPlot.Interactivity.PlotResponses;

public class DoubleClickResponse(MouseButton button, Action<Plot, Pixel> action) : IPlotResponse
{
    /// <summary>
    /// Which mouse button to watch for double-clicks.
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    /// <summary>
    /// This action is invoked when a double-click occurs.
    /// Replace this action with your own logic to customize double-click behavior.
    /// </summary>
    public Action<Plot, Pixel> ResponseAction { get; } = action;

    /// <summary>
    /// Consecutive clicks are only considered a double-click if the time between the first
    /// click mouse down and second click mouse up does not exceed this value.
    /// </summary>
    public TimeSpan MaximumTimeBetweenClicks = TimeSpan.FromMilliseconds(500);

    private DateTime LatestMouseDownTime = DateTime.MinValue;

    private DateTime PreviousMouseDownTime = DateTime.MinValue;

    public PlotResponseResult Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is IMouseButtonAction mouseAction && mouseAction.Button == MouseButton)
        {
            if (mouseAction.IsPressed)
            {
                PreviousMouseDownTime = LatestMouseDownTime;
                LatestMouseDownTime = mouseAction.DateTime;
                return PlotResponseResult.NoActionTaken;
            }
            else
            {
                TimeSpan timeSinceFirstMouseDown = mouseAction.DateTime - PreviousMouseDownTime;
                if (timeSinceFirstMouseDown < MaximumTimeBetweenClicks)
                {
                    ResponseAction.Invoke(plot, mouseAction.Pixel);
                    LatestMouseDownTime = DateTime.MinValue; // reset time so a third click won't toggle it back
                    return new PlotResponseResult()
                    {
                        Summary = "Double-left-click toggling benchmark visibility",
                        RefreshRequired = true,
                    };
                }
            }
        }

        return PlotResponseResult.NoActionTaken;
    }
}
