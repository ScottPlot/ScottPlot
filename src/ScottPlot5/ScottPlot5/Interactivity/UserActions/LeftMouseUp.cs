namespace ScottPlot.Interactivity.UserActions;

public record struct LeftMouseUp(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
