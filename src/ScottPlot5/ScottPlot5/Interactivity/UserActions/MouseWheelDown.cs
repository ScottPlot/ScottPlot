namespace ScottPlot.Interactivity.UserActions;

public record struct MouseWheelDown(Pixel Pixel) : IMouseButtonAction
{
    public readonly MouseButton Button => StandardMouseButtons.Wheel;
    public readonly bool IsPressed => false;
    public readonly string Device => $"mouse wheel";
    public readonly string Description => $"mouse wheel down";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
