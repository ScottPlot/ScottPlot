namespace ScottPlot.Interactivity.UserActions;

public record struct LeftMouseDown(Pixel Pixel) : IMouseButtonAction
{
    public readonly MouseButton Button => StandardMouseButtons.Left;
    public readonly bool IsPressed => true;
    public readonly string Device => $"mouse button {Button}";
    public readonly string Description => $"mouse button {Button} pressed";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
