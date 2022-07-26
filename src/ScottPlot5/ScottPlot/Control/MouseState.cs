namespace ScottPlot.Control;

internal class MouseState
{
    // TODO: add a flag so once the distance is exceeded it is ignored when you return to it, 
    // otherwise it feels laggy when you drag the cursor in small circles.

    /// <summary>
    /// A click-drag must exceed this number of pixels before it is considered a drag.
    /// </summary>
    public float MinimumDragDistance = 5;

    private readonly HashSet<MouseButton> Pressed = new();

    public void Down(MouseButton button) => Pressed.Add(button);

    public bool IsDown(MouseButton button) => Pressed.Contains(button);

    public IReadOnlyCollection<MouseButton> PressedButtons => Pressed.ToArray();

    public Pixel MouseDownPosition { get; private set; }

    public AxisLimits MouseDownAxisLimits { get; private set; }

    public void Up(MouseButton button)
    {
        ForgetMouseDown();
        Pressed.Remove(button);
    }

    public void Down(Pixel position, MouseButton button, AxisLimits limits)
    {
        RememberMouseDown(position, limits);
        Down(button);
    }

    private void RememberMouseDown(Pixel position, AxisLimits limits)
    {
        MouseDownPosition = position;
        MouseDownAxisLimits = limits;
    }

    private void ForgetMouseDown()
    {
        MouseDownPosition = Pixel.NaN;
        MouseDownAxisLimits = AxisLimits.NoLimits;
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
