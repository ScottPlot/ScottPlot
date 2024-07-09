namespace ScottPlot.Interactivity.UserInputs;

public record struct MouseWheelDown(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
