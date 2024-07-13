namespace ScottPlot.Interactivity.UserActions;

public record struct RightMouseUp(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
