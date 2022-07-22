namespace ScottPlot.Control;

internal class MouseButtonStates
{
    // TODO: add a flag so once the distance is exceeded it is ignored when you return to it, 
    // otherwise it feels laggy when you drag the cursor in small circles.

    /// <summary>
    /// A click-drag must exceed this number of pixels before it is considered a drag.
    /// </summary>
    public float MinimumDragDistance = 5;

    private Dictionary<MouseButton, bool> MouseButtonsPressed = new()
    {
        { MouseButton.Left, false },
        { MouseButton.Right, false },
        { MouseButton.Middle, false },
        { MouseButton.Unknown, false },
    };

    public void SetMouseButtonDown(MouseButton button) => MouseButtonsPressed[button] = true;

    public void Up(MouseButton button)
    {
        MouseButtonsPressed[button] = false;
    }

    public Pixel MouseDownPosition { get; private set; }

    public AxisLimits MouseDownAxisLimits { get; private set; }

    public bool IsButtonPressed()
    {
        foreach (MouseButton button in MouseButtonsPressed.Keys.ToArray())
            if (MouseButtonsPressed[button])
                return true;

        return false;
    }

    public MouseButton? GetPressedButton()
    {
        foreach (MouseButton button in MouseButtonsPressed.Keys.ToArray())
        {
            if (MouseButtonsPressed[button])
            {
                return button;
            }
        }
        return null;
    }

    public void Clear()
    {
        MouseDownPosition = Pixel.NaN;

        foreach (MouseButton button in MouseButtonsPressed.Keys.ToArray())
        {
            MouseButtonsPressed[button] = false;
        }
    }

    public void Down(Pixel position, MouseButton button, AxisLimits limits)
    {
        MouseDownPosition = position;
        MouseDownAxisLimits = limits;
        SetMouseButtonDown(button);
    }

    public bool IsDragging(Pixel position)
    {
        if (float.IsNaN(MouseDownPosition.X))
            return false;

        float dragDistance = (MouseDownPosition - position).Hypotenuse;
        if (dragDistance > MinimumDragDistance)
            return true;

        return false;
    }
}
