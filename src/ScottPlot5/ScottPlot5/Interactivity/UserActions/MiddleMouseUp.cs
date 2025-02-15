namespace ScottPlot.Interactivity.UserActions;

public record struct MiddleMouseUp(Pixel Pixel) : IMouseButtonAction
{
    public readonly MouseButton Button => StandardMouseButtons.Middle;
    public readonly bool IsPressed => false;
    public readonly string Device => $"mouse button {Button}";
    public readonly string Description => $"mouse button {Button} released";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
