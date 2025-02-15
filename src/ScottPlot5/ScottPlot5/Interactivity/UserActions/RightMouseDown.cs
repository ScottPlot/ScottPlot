namespace ScottPlot.Interactivity.UserActions;

public record struct RightMouseDown(Pixel Pixel) : IMouseButtonAction
{
    public readonly MouseButton Button => StandardMouseButtons.Right;
    public readonly bool IsPressed => true;
    public readonly string Device => $"mouse button {Button}";
    public readonly string Description => $"mouse button {Button} pressed";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
