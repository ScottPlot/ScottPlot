namespace ScottPlot.Interactivity.UserActionResponses;

internal class MouseInteractWithPlottables(MouseButton button) : IUserActionResponse
{
    /// <summary>
    /// Which mouse button to watch to engage with interactive plottables
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    public void ResetState(IPlotControl plotControl)
    {
        InteractingNode?.Parent.MouseUp(InteractingNode);
        InteractingNode = null;
        InteractingPlot = null;
    }

    InteractiveNode? HoveredNode;
    InteractiveNode? InteractingNode;
    Plot? InteractingPlot;

    public ResponseInfo Execute(IPlotControl plotControl, IUserAction userInput, KeyboardState keys)
    {
        // if we are already interacting with a node, releasing the mouse button terminates the interaction
        if (InteractingNode is not null && InteractingPlot is not null)
        {
            if (userInput is IMouseButtonAction buttonReleaseAction
                && buttonReleaseAction.Button == MouseButton
                && buttonReleaseAction.IsPressed == false)
            {
                InteractingNode.Parent.MouseUp(InteractingNode);
                InteractingPlot.NodeReleased?.Invoke(this, InteractingNode);
                InteractingNode = null;
                InteractingPlot = null;
                return new ResponseInfo()
                {
                    RefreshNeeded = true, // force a render
                    IsPrimary = false, // release exclusive control of the interaction system
                };
            }
        }

        // if we are already interacting with a node, mouse drags update its position
        if (InteractingNode is not null && InteractingPlot is not null)
        {
            if (userInput is IMouseAction mouseMoveAction)
            {
                Coordinates cs = InteractingPlot.GetCoordinates(mouseMoveAction.Pixel);
                InteractingNode.Parent.MouseMove(InteractingNode, cs);
                InteractingPlot.NodeMoved?.Invoke(this, InteractingNode);
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
                InteractiveNode? pressedNode = plot.GetInteractiveNode(buttonPressAction.Pixel.X, buttonPressAction.Pixel.Y);
                if (pressedNode is not null)
                {
                    pressedNode.Parent.MouseDown(pressedNode);
                    plot.NodePressed?.Invoke(this, pressedNode);
                    InteractingNode = pressedNode;
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
                InteractiveNode? hoveredNode = plot.GetInteractiveNode(mouseMoveAction2.Pixel.X, mouseMoveAction2.Pixel.Y);
                if (hoveredNode != HoveredNode)
                {
                    plot.NodeHoverChanged?.Invoke(this, hoveredNode);
                    HoveredNode = hoveredNode;
                    return ResponseInfo.NoActionRequired;
                }
            }
        }

        return ResponseInfo.NoActionRequired;
    }
}
