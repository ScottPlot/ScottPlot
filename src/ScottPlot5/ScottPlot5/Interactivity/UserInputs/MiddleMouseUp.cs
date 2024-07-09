namespace ScottPlot.Interactivity.UserInputs;

public record struct MiddleMouseUp(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
