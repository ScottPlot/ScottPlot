namespace ScottPlot.Interactivity.UserActions;

public record struct MouseMove(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
