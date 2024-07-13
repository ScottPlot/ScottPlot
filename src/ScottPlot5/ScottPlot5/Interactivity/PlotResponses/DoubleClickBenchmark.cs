using ScottPlot.Control;
using ScottPlot.Interactivity.UserActions;

namespace ScottPlot.Interactivity.PlotResponses;

public class DoubleClickBenchmark : IPlotResponse
{
    DateTime LatestMouseDownTime = DateTime.MinValue;
    DateTime PreviousMouseDownTime = DateTime.MinValue;
    public TimeSpan MaximumTimeBetweenClicks = TimeSpan.FromMilliseconds(500);

    // TODO: customize double click button

    public PlotResponseResult Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is LeftMouseDown mouseDownAction)
        {
            PreviousMouseDownTime = LatestMouseDownTime;
            LatestMouseDownTime = mouseDownAction.DateTime;
            return PlotResponseResult.NoActionTaken;
        }

        if (userAction is LeftMouseUp mouseUpInput)
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
