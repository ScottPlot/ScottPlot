namespace ScottPlot.Interactivity.UserActions;

public record struct MouseWheelUp(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
