namespace ScottPlot.Interactivity.UserActions;

public record struct MiddleMouseDown(Pixel Pixel) : IMouseButtonAction
{
    public readonly MouseButton Button => StandardMouseButtons.Middle;
    public readonly bool IsPressed => true;
    public readonly string Device => $"mouse button {Button}";
    public readonly string Description => $"mouse button {Button} pressed";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
