namespace ScottPlot.Interactivity.UserActions;

public record struct KeyDown(Key Key) : IUserAction
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
