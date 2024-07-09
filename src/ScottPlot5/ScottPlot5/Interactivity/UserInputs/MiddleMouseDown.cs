namespace ScottPlot.Interactivity.UserInputs;

public record struct MiddleMouseDown(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
