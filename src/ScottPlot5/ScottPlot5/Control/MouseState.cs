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

    public readonly MultiAxisLimits MouseDownAxisLimits = new();

    public void Up(MouseButton button)
    {
        ForgetMouseDown();
        Pressed.Remove(button);
    }

    public void Down(Pixel position, MouseButton button, MultiAxisLimits limits)
    {
        RememberMouseDown(position, limits);
        Down(button);
    }

    private void RememberMouseDown(Pixel position, MultiAxisLimits limits)
    {
        MouseDownPosition = position;
        MouseDownAxisLimits.ForgetAllLimits();
        MouseDownAxisLimits.RememberLimits(limits);
    }

    private void ForgetMouseDown()
    {
        MouseDownPosition = Pixel.NaN;
        MouseDownAxisLimits.ForgetAllLimits();
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
