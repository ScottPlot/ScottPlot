namespace ScottPlot.Interactivity.UserActions;

public record struct MouseWheelUp(Pixel Pixel) : IMouseButtonAction
{
    public readonly MouseButton Button => StandardMouseButtons.Wheel;
    public readonly bool IsPressed => true;
    public readonly string Device => $"mouse wheel";
    public readonly string Description => $"mouse wheel up";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
