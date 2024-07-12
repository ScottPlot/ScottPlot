using ScottPlot.Interactivity.UserInputs;

namespace ScottPlot.Interactivity.UserInputResponses;

// TODO: refactor to DoubleLeftClickAction and make the default action replaceable
public class DoubleClickBenchmark : IUserInputResponse
{
    DateTime LatestMouseDownTime = DateTime.MinValue;
    DateTime PreviousMouseDownTime = DateTime.MinValue;
    public TimeSpan MaximumTimeBetweenClicks = TimeSpan.FromMilliseconds(500);

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is LeftMouseDown mouseDownInput)
        {
            PreviousMouseDownTime = LatestMouseDownTime;
            LatestMouseDownTime = mouseDownInput.DateTime;
            return UserInputResponseResult.NoActionTaken;
        }

        if (userInput is LeftMouseUp mouseUpInput)
        {
            TimeSpan timeSinceFirstMouseDown = mouseUpInput.DateTime - PreviousMouseDownTime;
            if (timeSinceFirstMouseDown < MaximumTimeBetweenClicks)
            {
                plot.Benchmark.IsVisible = !plot.Benchmark.IsVisible;
                LatestMouseDownTime = DateTime.MinValue; // reset time so a third click won't toggle it back
                return new UserInputResponseResult()
                {
                    Summary = "Double-left-click toggling benchmark visibility",
                    RefreshRequired = true,
                };
            }
        }

        return UserInputResponseResult.NoActionTaken;
    }
}
