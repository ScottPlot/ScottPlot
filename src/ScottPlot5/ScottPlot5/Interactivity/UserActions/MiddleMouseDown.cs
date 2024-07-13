namespace ScottPlot.Interactivity.UserInputs;

public record struct MiddleMouseDown(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
