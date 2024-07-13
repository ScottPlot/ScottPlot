namespace ScottPlot.Interactivity.UserActions;

public record struct LeftMouseDown(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
