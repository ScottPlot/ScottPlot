namespace ScottPlot.Interactivity.UserActions;

public record struct MouseWheelDown(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
