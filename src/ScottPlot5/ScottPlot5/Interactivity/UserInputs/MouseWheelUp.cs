namespace ScottPlot.Interactivity.UserInputs;

public record struct MouseWheelUp(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
