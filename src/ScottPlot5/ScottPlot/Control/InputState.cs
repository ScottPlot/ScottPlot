namespace ScottPlot.Control;

/// <summary>
/// This class represents the input state of the user of a control at a point in time.
/// </summary>
public class InputState
{
    public readonly Pixel MousePosition;
    public readonly IEnumerable<MouseButton> MouseButtonsPressed;

    public InputState(Pixel mousePosition, IEnumerable<MouseButton?> mouseButtons)
    {
        MousePosition = mousePosition;
        MouseButtonsPressed = mouseButtons.Where(x => x.HasValue).OfType<MouseButton>();
    }
}
