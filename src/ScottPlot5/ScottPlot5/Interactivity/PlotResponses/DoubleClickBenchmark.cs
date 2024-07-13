using ScottPlot.Interactivity.UserActions;

namespace ScottPlot.Interactivity.PlotResponses;

// TODO: refactor to DoubleLeftClickAction and make the default action replaceable
public class DoubleClickBenchmark : IPlotResponse
{
    DateTime LatestMouseDownTime = DateTime.MinValue;
    DateTime PreviousMouseDownTime = DateTime.MinValue;
    public TimeSpan MaximumTimeBetweenClicks = TimeSpan.FromMilliseconds(500);

    IUserAction DoubleClickInput { get; set; } = new LeftMouseDown();

    public PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyState keys)
    {
        if (userInput is IMouseInput mouseInput)
        {
            if (mouseInput == DoubleClickInput)
            {
                PreviousMouseDownTime = LatestMouseDownTime;
                LatestMouseDownTime = mouseInput.DateTime;
                return PlotResponseResult.NoActionTaken;
            }
        }

        if (userInput is LeftMouseUp mouseUpInput)
        {
            TimeSpan timeSinceFirstMouseDown = mouseUpInput.DateTime - PreviousMouseDownTime;
            if (timeSinceFirstMouseDown < MaximumTimeBetweenClicks)
            {
                plot.Benchmark.IsVisible = !plot.Benchmark.IsVisible;
                LatestMouseDownTime = DateTime.MinValue; // reset time so a third click won't toggle it back
                return new PlotResponseResult()
                {
                    Summary = "Double-left-click toggling benchmark visibility",
                    RefreshRequired = true,
                };
            }
        }

        return PlotResponseResult.NoActionTaken;
    }
}
