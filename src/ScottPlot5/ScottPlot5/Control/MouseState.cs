namespace ScottPlot.Control;

public class MouseState
{
    /// <summary>
    /// A click-drag must exceed this number of pixels before it is considered a drag.
    /// </summary>
    public float MinimumDragDistance = 5;

    private readonly HashSet<MouseButton> Pressed = new();

    public Pixel LastPosition = new(float.NaN, float.NaN);

    public void Down(MouseButton button) => Pressed.Add(button);

    public bool IsDown(MouseButton button) => Pressed.Contains(button);

    public IReadOnlyCollection<MouseButton> PressedButtons => Pressed.ToArray();

    public Pixel MouseDownPosition { get; private set; }

    public MultiAxisLimitManager MouseDownAxisLimits = new();

    public void Up(MouseButton button)
    {
        ForgetMouseDown();
        Pressed.Remove(button);
    }

    public void Down(Pixel position, MouseButton button, MultiAxisLimitManager limits)
    {
        RememberMouseDown(position, limits);
        Down(button);
    }

    private void RememberMouseDown(Pixel position, MultiAxisLimitManager limits)
    {
        MouseDownPosition = position;
        MouseDownAxisLimits.Remember(limits);
    }

    private void ForgetMouseDown()
    {
        MouseDownPosition = Pixel.NaN;
    }

    public bool IsDragging(Pixel position)
    {
        if (float.IsNaN(MouseDownPosition.X))
            return false;

        Pixel pixelDifference = MouseDownPosition - position;
        PixelSize ps = new(pixelDifference.X, pixelDifference.Y);

        float dragDistance = ps.Diagonal;
        if (dragDistance > MinimumDragDistance)
            return true;

        return false;
    }
}
