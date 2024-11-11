using ScottPlot.Interactivity.UserActions;

namespace ScottPlot.Interactivity.UserActionResponses;

public class KeyPressResponse(Key key, Action<Plot, Pixel> action) : IUserActionResponse
{
    Key Key { get; } = key;

    Action<Plot, Pixel> ResponseAction { get; } = action;

    public void ResetState(Plot plot) { }

    public ResponseInfo Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is KeyDown keyDownAction)
        {
            if (keyDownAction.Key == Key)
            {
                ResponseAction.Invoke(plot, Pixel.NaN);

                return ResponseInfo.Refresh;
            }

        }

        return ResponseInfo.NoActionRequired;
    }
}
