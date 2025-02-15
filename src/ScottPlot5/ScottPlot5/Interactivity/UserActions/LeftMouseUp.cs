namespace ScottPlot.Interactivity.UserActions;

public record struct LeftMouseUp(Pixel Pixel) : IMouseButtonAction
{
    public readonly MouseButton Button => StandardMouseButtons.Left;
    public readonly bool IsPressed => false;
    public readonly string Device => $"mouse button {Button}";
    public readonly string Description => $"mouse button {Button} released";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
