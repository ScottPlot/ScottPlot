namespace ScottPlot.Interactivity.UserInputs;

public record struct LeftMouseDown(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
