namespace ScottPlot.Interactivity.UserActionResponses;

public class MouseInteractWithPlottables(MouseButton button) : IUserActionResponse
{
    /// <summary>
    /// Which mouse button to watch to engage with interactive plottables
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    public void ResetState(IPlotControl plotControl)
    {
        InteractingHandle?.Parent.ReleaseHandle(InteractingHandle);
        InteractingHandle = null;
        InteractingPlot = null;
    }

    InteractiveHandle? HoveredHandle;
    InteractiveHandle? InteractingHandle;
    Plot? InteractingPlot;

    public ResponseInfo Execute(IPlotControl plotControl, IUserAction userInput, KeyboardState keys)
    {
        // if we are already interacting with a node, releasing the mouse button terminates the interaction
        if (InteractingHandle is not null && InteractingPlot is not null)
        {
            if (userInput is IMouseButtonAction buttonReleaseAction
                && buttonReleaseAction.Button == MouseButton
                && buttonReleaseAction.IsPressed == false)
            {
                InteractingHandle.Parent.ReleaseHandle(InteractingHandle);
                InteractingPlot.HandleReleased?.Invoke(this, InteractingHandle);
                InteractingHandle = null;
                InteractingPlot = null;
                return new ResponseInfo()
                {
                    RefreshNeeded = true, // force a render
                    IsPrimary = false, // release exclusive control of the interaction system
                };
            }
        }

        // if we are already interacting with a node, mouse drags update its position
        if (InteractingHandle is not null && InteractingPlot is not null)
        {
            if (userInput is IMouseAction mouseMoveAction)
            {
                Coordinates cs = InteractingPlot.GetCoordinates(mouseMoveAction.Pixel);
                InteractingHandle.Parent.MoveHandle(InteractingHandle, cs);
                InteractingPlot.HandleMoved?.Invoke(this, InteractingHandle);
                return ResponseInfo.Refresh;
            }
        }

        // mouse down starts drag
        if (userInput is IMouseButtonAction buttonPressAction
            && buttonPressAction.Button == MouseButton
            && buttonPressAction.IsPressed)
        {
            Plot? plot = plotControl.GetPlotAtPixel(buttonPressAction.Pixel);
            if (plot is not null)
            {
                InteractiveHandle? pressedHandle = plot.GetInteractiveHandle(buttonPressAction.Pixel.X, buttonPressAction.Pixel.Y);
                if (pressedHandle is not null)
                {
                    var cs = plot.GetCoordinates(buttonPressAction.Pixel);
                    pressedHandle.Parent.PressHandle(pressedHandle, cs);
                    plot.HandlePressed?.Invoke(this, pressedHandle);
                    InteractingHandle = pressedHandle;
                    InteractingPlot = plot;
                    return new ResponseInfo()
                    {
                        RefreshNeeded = true, // force a render
                        IsPrimary = true, // take exclusive control of the interaction system to prevent click-drag-pan
                    };
                }
            }
        }

        // invoke an event if the mouse hovered over an interactive node
        if (userInput is IMouseAction mouseMoveAction2)
        {
            Plot? plot = plotControl.GetPlotAtPixel(mouseMoveAction2.Pixel);
            if (plot is not null)
            {
                InteractiveHandle? hoveredHandle = plot.GetInteractiveHandle(mouseMoveAction2.Pixel.X, mouseMoveAction2.Pixel.Y);
                if (hoveredHandle != HoveredHandle)
                {
                    plot.HandleHoverChanged?.Invoke(this, hoveredHandle);

                    if (hoveredHandle is not null)
                    {
                        plot.PlotControl?.SetCursor(hoveredHandle.Cursor);
                    }
                    else
                    {
                        plot.PlotControl?.SetCursor(Cursor.Arrow);
                    }

                    HoveredHandle = hoveredHandle;
                    return ResponseInfo.NoActionRequired;
                }
            }
        }

        return ResponseInfo.NoActionRequired;
    }
}
