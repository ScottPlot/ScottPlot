using ScottPlot.Interactivity.UserActions;

namespace ScottPlot.Interactivity.UserActionResponses;

public class KeyPressResponse(Key key, Action<IPlotControl, Pixel> action) : IUserActionResponse
{
    Key Key { get; } = key;

    Action<IPlotControl, Pixel> ResponseAction { get; } = action;

    public void ResetState(IPlotControl plotControl) { }

    Pixel MousePixel = Pixel.Zero;

    public ResponseInfo Execute(IPlotControl plotControl, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is MouseMove mouseMove)
        {
            MousePixel = mouseMove.Pixel;
            return ResponseInfo.NoActionRequired;
        }

        if (userAction is KeyDown keyDownAction)
        {
            if (keyDownAction.Key == Key)
            {
                ResponseAction.Invoke(plotControl, MousePixel);
                return ResponseInfo.Refresh;
            }
        }

        return ResponseInfo.NoActionRequired;
    }
}
