namespace ScottPlot.Interactivity.UserActionResponses;

public class MouseDragZoom(MouseButton button) : IUserActionResponse
{
    Pixel MouseDownPixel = Pixel.NaN;

    MouseButton MouseButton { get; } = button;

    // TODO: re-implement this being more careful about allocations
    MultiAxisLimits? RememberedLimits = null;

    public ResponseInfo Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        if (userInput is IMouseButtonAction mouseDownAction && mouseDownAction.Button == MouseButton && mouseDownAction.IsPressed)
        {
            MouseDownPixel = mouseDownAction.Pixel;
            RememberedLimits = new(plot);
            return new ResponseInfo() { IsPrimary = false };
        }

        if (MouseDownPixel == Pixel.NaN)
            return ResponseInfo.NoActionRequired;

        if (userInput is IMouseButtonAction mouseUpAction && mouseUpAction.Button == MouseButton && !mouseUpAction.IsPressed)
        {
            RememberedLimits?.Recall();
            ApplyToPlot(plot, MouseDownPixel, mouseUpAction.Pixel, keys);
            MouseDownPixel = Pixel.NaN;
            return ResponseInfo.Refresh;
        }

        if (userInput is IMouseAction mouseMoveAction)
        {
            RememberedLimits?.Recall();
            ApplyToPlot(plot, MouseDownPixel, mouseMoveAction.Pixel, keys);
            return new ResponseInfo() { RefreshNeeded = true, IsPrimary = true };
        }

        return new ResponseInfo() { IsPrimary = true };
    }

    private static void ApplyToPlot(Plot plot, Pixel px1, Pixel px2, KeyboardState keys)
    {
        if (keys.IsPressed(StandardKeys.Shift))
        {
            px2.X = px1.X;
        }

        if (keys.IsPressed(StandardKeys.Control))
        {
            px2.Y = px1.Y;
        }

        MouseAxisManipulation.DragZoom(plot, px1, px2);
    }
}
