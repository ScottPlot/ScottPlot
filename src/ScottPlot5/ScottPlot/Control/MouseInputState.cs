namespace ScottPlot.Control;

/// <summary>
/// This class represents the input state of the user of a control at a point in time.
/// </summary>
public class MouseInputState
{
    public readonly Pixel Position;
    public readonly IEnumerable<MouseButton> ButtonsPressed;

    public MouseInputState(Pixel position, IEnumerable<MouseButton?> buttons)
    {
        Position = position;
        ButtonsPressed = buttons.Where(x => x.HasValue).OfType<MouseButton>();
    }
}
