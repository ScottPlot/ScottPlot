using ScottPlot.Interactivity.UserActions;

namespace ScottPlot.Interactivity.PlotResponses;

public class KeyPressResponse(Key key, Action<Plot, Pixel> action) : IPlotResponse
{
    Key Key { get; } = key;

    Action<Plot, Pixel> ResponseAction { get; } = action;

    public PlotResponseResult Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is KeyDown keyDownAction)
        {
            if (keyDownAction.Key == Key)
            {
                ResponseAction.Invoke(plot, Pixel.NaN);

                return new PlotResponseResult()
                {
                    Summary = $"custom key press action for {Key}",
                    RefreshRequired = true,
                };
            }

        }

        return PlotResponseResult.NoActionTaken;
    }
}
