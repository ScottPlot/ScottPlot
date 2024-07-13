namespace ScottPlot.Interactivity.UserActions;

public record struct MiddleMouseDown(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
